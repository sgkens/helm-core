// ==============================================================================
// HELM: High-Efficiency Lightweight Monitor for Space Engineers
// Single-file combined distribution for Programmable Block
// ==============================================================================

// Program.cs — SE-specific adapter for HELM.
//
// Status: this file compiles against the stand-in API in tools/spritedump (no errors) and its
// sprite output is exported for the docs. It has NOT been run in Space Engineers: instruction
// counts, allocation behaviour and real font metrics are unmeasured. HelmCore.cs has the unit
// tests (Tests.cs); Program.cs has none.
//
// Deployment note: this is written as an MDK-style project (HelmCore.cs +
// this file, separate namespaces). If pasting directly into the in-game PB
// editor without MDK, the HelmCore.cs types need to be nested inside the
// Program class body instead of living in their own namespace — the in-game
// editor only auto-wraps a single "partial class Program : MyGridProgram".

using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // ---- state ----
        private ParsedConfig _config;
        private string _lastCustomData = null;

        // Resolved bind name -> list of blocks that satisfy it (single-block
        // binds resolve to a 1-item list). Re-resolved on config change and
        // on the periodic rescan below — never every tick (PDR §5).
        private Dictionary<string, List<IMyTerminalBlock>> _resolvedBinds
            = new Dictionary<string, List<IMyTerminalBlock>>();
        private Dictionary<string, bool> _fallbackUsed = new Dictionary<string, bool>();
        private Dictionary<string, BindDef> _bindMap = new Dictionary<string, BindDef>();
        private Dictionary<string, Color> _colorCache = new Dictionary<string, Color>();
        private List<IMyTerminalBlock> _allBlocks = new List<IMyTerminalBlock>();
        private bool _allLoaded;
        private string _cfgErr = "";
        private StringBuilder _outErr = new StringBuilder();

        // Dynamic Micro-API: In-memory variables for IGC broadcast and CLI injection
        private readonly Dictionary<string, double> _dynamicValues
            = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        private IMyBroadcastListener _helmBroadcastListener;
        private bool _globalRedAlert = false;

        // Resolved screen -> output surface. Re-resolved alongside binds.
        private Dictionary<ScreenDef, IMyTextSurface> _resolvedOutputs
            = new Dictionary<ScreenDef, IMyTextSurface>();

        // Dirty-flag cache: last rendered signature per screen (PDR §4).
        private Dictionary<ScreenDef, string> _lastSignature
            = new Dictionary<ScreenDef, string>();

        // Pre-allocated static cache for all integer percentage strings (0-byte allocation path)
        private static readonly string[] PCT_CACHE = new string[101];

        static Program()
        {
            for (int i = 0; i <= 100; i++)
            {
                PCT_CACHE[i] = i + "%";
            }
        }

        private static string GetPercentString(double value)
        {
            int idx = (int)Math.Round(value);
            if (idx < 0) idx = 0;
            else if (idx > 100) idx = 100;
            return PCT_CACHE[idx];
        }

        // Retained Virtual Sprite Buffers: in-place mutated sprite lists for graphic screens
        private class ScreenBuffer
        {
            public List<MySprite> Sprites = new List<MySprite>();
            public List<LineSlot> Slots = new List<LineSlot>();
            public Vector2 LastSurfaceSize;
            public bool Initialized = false;
        }

        private class LineSlot
        {
            public LineDef Line;
            public int FillSpriteIndex = -1;
            public int ValSpriteIndex = -1;
            public int LabelSpriteIndex = -1;
            public bool HasDynamicLabel = false;
            public List<int> ExtraSpriteIndices = new List<int>(); // for LedBar, Chevron, Dots, Hazard, and Sparkline
            public float[] SparkHistory = new float[8];
            public float BarLeft;
            public float BarMaxWidth;
            public float BarHeight;
            public float BarCenterY;
            public double LastValue = -99999.0;
            public float DisplayValue = -1f; // fluid damping lerp
        }

        private Dictionary<ScreenDef, ScreenBuffer> _screenBuffers
            = new Dictionary<ScreenDef, ScreenBuffer>();

        private int _screenCursor = 0;
        private int _rescanCounter = 0;
        private int _tickCounter = 0;
        private const int RESCAN_EVERY_N_RUNS = 100; // PDR §5: rescan blocks periodically, not per tick

        // Approximate line height in pixels per unit FontSize.
        // *** UNVERIFIED — a placeholder, not a researched constant. ***
        // The real value should come from MeasureStringInPixels() against an
        // actual surface in-game, per PDR §7's "file/API check tells you
        // presence, not exact rendered metrics" caution. Do not trust this
        // number until it's been checked on a real screen.
        private const float APPROX_LINE_HEIGHT_PER_FONTSIZE = 22f;

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100; // PDR §4 default hypothesis
            try
            {
                _helmBroadcastListener = IGC.RegisterBroadcastListener("HELM");
                _helmBroadcastListener.SetMessageCallback("HELM_IGC");
            }
            catch {}
            ReloadConfig();
        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & UpdateType.IGC) != 0 || argument == "HELM_IGC")
            {
                ProcessIgcMessages();
            }

            string rawArg = (argument ?? "").Trim();
            string arg = rawArg.ToLowerInvariant();
            if (arg == "reload")
            {
                ReloadConfig();
            }
            else if (arg == "refresh")
            {
                ResolveBindsAndOutputs();
            }
            else if (arg == "scan" || arg == "diagnose" || arg == "test")
            {
                RunSelfDiagnostics();
                return;
            }
            else if (arg.StartsWith("set "))
            {
                string rest = rawArg.Substring(4).Trim();
                int eq = rest.IndexOf('=');
                int sp = rest.IndexOf(' ');
                int sep = eq > 0 ? eq : sp;
                if (sep > 0)
                {
                    string k = rest.Substring(0, sep).Trim();
                    string v = rest.Substring(sep + 1).Trim();
                    ApplyDynamicPayload($"{k}={v}");
                    Echo($"[HELM] Set dynamic variable '{k}' = {v}");
                }
                return;
            }
            else if (arg.StartsWith("alert"))
            {
                string val = arg.Length > 5 ? arg.Substring(5).Trim() : "toggle";
                if (val == "toggle") _globalRedAlert = !_globalRedAlert;
                else _globalRedAlert = val == "red" || val == "on" || val == "1" || val == "true";
                Echo($"[HELM] Red alert state: {_globalRedAlert}");
                return;
            }
            else if (Me.CustomData != _lastCustomData)
            {
                ReloadConfig();
            }

            _tickCounter++;
            _rescanCounter++;
            if (_rescanCounter >= RESCAN_EVERY_N_RUNS)
            {
                _rescanCounter = 0;
                ResolveBindsAndOutputs();
            }

            if (_config == null || _config.Screens.Count == 0)
            {
                EchoNotes();
                Echo("HELM: no valid @screen configured. Check Custom Data.");
                return;
            }

            // Round-robin: exactly one screen's full redraw per Main() call.
            // This is the FSD2-failure fix (PDR §1/§4) — never render every
            // screen in one pass.
            var screen = _config.Screens[_screenCursor % _config.Screens.Count];
            _screenCursor++;

            RenderScreen(screen);

            // PDR §9 profiling hook — leave this in during development so the
            // §4 "working hypothesis" numbers can actually be replaced with
            // measured ones.
            EchoNotes();
            Echo($"HELM [{screen.Name}] instr {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} — {Runtime.LastRunTimeMs:0.###}ms");
        }

        // Config errors and output-block problems are kept and echoed on every run: Echo text is replaced each run.
        private void EchoNotes()
        {
            if (_cfgErr.Length > 0) Echo(_cfgErr);
            if (_outErr.Length > 0) Echo(_outErr.ToString());
        }

        private List<IMyTerminalBlock> AllBlocks()
        {
            if (!_allLoaded)
            {
                _allBlocks.Clear();
                GridTerminalSystem.GetBlocks(_allBlocks);
                _allLoaded = true;
            }
            return _allBlocks;
        }

        // ---------------- config / binding resolution ----------------

        private void ReloadConfig()
        {
            _lastCustomData = Me.CustomData;
            _config = HelmParser.Parse(_lastCustomData);

            _cfgErr = "";
            if (_config.Errors.Count > 0)
            {
                var sb = new StringBuilder("HELM config errors:\n");
                foreach (var e in _config.Errors) sb.AppendLine(" - " + e);
                _cfgErr = sb.ToString();
            }

            _bindMap.Clear();
            foreach (var b in _config.Binds)
                if (!_bindMap.ContainsKey(b.Name)) _bindMap[b.Name] = b;

            _screenBuffers.Clear();
            _lastSignature.Clear();
            ResolveBindsAndOutputs();
        }

        private void ProcessIgcMessages()
        {
            try
            {
                while (_helmBroadcastListener != null && _helmBroadcastListener.HasPendingMessage)
                {
                    var msg = _helmBroadcastListener.AcceptMessage();
                    if (msg.Data is string)
                    {
                        ApplyDynamicPayload((string)msg.Data);
                    }
                }
            }
            catch {}
        }

        private void ApplyDynamicPayload(string payload)
        {
            if (string.IsNullOrEmpty(payload)) return;
            var pairs = payload.Split(';');
            for (int i = 0; i < pairs.Length; i++)
            {
                var pair = pairs[i].Trim();
                if (pair.Length == 0) continue;
                int eq = pair.IndexOf('=');
                if (eq < 0) eq = pair.IndexOf(':');
                if (eq > 0)
                {
                    string key = pair.Substring(0, eq).Trim();
                    string valStr = pair.Substring(eq + 1).Trim();
                    if (key.Equals("alert", StringComparison.OrdinalIgnoreCase))
                    {
                        _globalRedAlert = valStr.Equals("red", StringComparison.OrdinalIgnoreCase) || valStr == "1" || valStr.Equals("true", StringComparison.OrdinalIgnoreCase) || valStr.Equals("on", StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        double num;
                        if (double.TryParse(valStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out num))
                        {
                            _dynamicValues[key] = num;
                        }
                        else if (valStr.Equals("true", StringComparison.OrdinalIgnoreCase) || valStr.Equals("on", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicValues[key] = 100.0;
                        }
                        else if (valStr.Equals("false", StringComparison.OrdinalIgnoreCase) || valStr.Equals("off", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicValues[key] = 0.0;
                        }
                    }
                }
            }
        }

        private void ResolveBindsAndOutputs()
        {
            _resolvedBinds.Clear();
            _resolvedOutputs.Clear();
            _fallbackUsed.Clear();
            _outErr.Clear();
            _allLoaded = false;

            if (_config == null) return;

            foreach (var bind in _config.Binds)
            {
                var blocks = new List<IMyTerminalBlock>();

                if (bind.Kind == BindKind.Dynamic)
                {
                    string k = !string.IsNullOrEmpty(bind.DynamicKey) ? bind.DynamicKey : bind.Name;
                    if (!_dynamicValues.ContainsKey(k))
                    {
                        _dynamicValues[k] = bind.DefaultValue;
                    }
                    _resolvedBinds[bind.Name] = blocks;
                    continue;
                }

                var targets = bind.TargetNames.Count > 0 ? bind.TargetNames : new List<string> { bind.TargetName };

                foreach (var targetName in targets)
                {
                    if (string.IsNullOrEmpty(targetName)) continue;

                    bool isWildcard = targetName.Contains("*") || targetName.Contains("?");

                    if (bind.Kind == BindKind.Group || isWildcard)
                    {
                        var group = GridTerminalSystem.GetBlockGroupWithName(targetName);
                        if (group != null)
                        {
                            var groupBlocks = new List<IMyTerminalBlock>();
                            group.GetBlocks(groupBlocks);
                            foreach (var gb in groupBlocks)
                            {
                                if (!blocks.Contains(gb)) blocks.Add(gb);
                            }
                        }
                        else
                        {
                            // Search all terminal blocks for wildcard or exact match
                            var allBlocks = AllBlocks();
                            for (int b = 0; b < allBlocks.Count; b++)
                            {
                                var tb = allBlocks[b];
                                if (isWildcard ? Layout.MatchWildcard(targetName, tb.CustomName) : string.Equals(targetName, tb.CustomName, StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!blocks.Contains(tb)) blocks.Add(tb);
                                }
                            }
                        }
                    }
                    else
                    {
                        var block = GridTerminalSystem.GetBlockWithName(targetName);
                        if (block != null && !blocks.Contains(block)) blocks.Add(block);
                    }
                }

                if (blocks.Count == 0)
                {
                    ResolveFallbackBlocks(bind, targets, blocks);
                    if (blocks.Count > 0) _fallbackUsed[bind.Name] = true;
                }

                _resolvedBinds[bind.Name] = blocks;
            }

            foreach (var screen in _config.Screens)
            {
                if (string.IsNullOrEmpty(screen.OutputBlockName)) continue;

                var outBlock = GridTerminalSystem.GetBlockWithName(screen.OutputBlockName);
                if (outBlock == null)
                {
                    _outErr.AppendLine($"HELM: output block '{screen.OutputBlockName}' not found");
                    continue;
                }

                IMyTextSurface surface = null;
                var provider = outBlock as IMyTextSurfaceProvider;
                if (provider != null && screen.OutputSurfaceIndex < provider.SurfaceCount)
                {
                    surface = provider.GetSurface(screen.OutputSurfaceIndex);
                }
                else if (outBlock is IMyTextSurface directSurface && screen.OutputSurfaceIndex == 0)
                {
                    surface = directSurface;
                }

                if (surface == null)
                {
                    _outErr.AppendLine($"HELM: output block '{screen.OutputBlockName}' has no surface index {screen.OutputSurfaceIndex}");
                    continue;
                }

                surface.ContentType = ContentType.SCRIPT;
                surface.Script = "";
                surface.ScriptBackgroundColor = Color.Black;
                _resolvedOutputs[screen] = surface;
            }

            _allBlocks.Clear();
        }

        // Keyword hints (matched against the bind name and targets) for each block type tried when a bind matches no block by name.
        private static readonly string[] FallbackWords =
        {
            "batt,power,pwr",
            "reactor,nuke",
            "solar",
            "wind",
            "tank,hydro,hydrogen,o2,oxygen",
            "vent,air,press",
            "cargo,container,store,inv",
            "jump,drive",
            "thrust,engine",
            "susp,wheel,brake",
            "door,gate,airlock",
            "connect",
            "asm,assembler,fab,prod",
            "refin,smelt,furnace",
            "drill,mine,bore",
            "weld",
            "grind",
            "o2h2,gen,generator",
            "turret,gun,cannon,weapon,defense",
            "proj,blueprint",
            "gyro",
            "grav,gravity",
            "gear,magplate,landing",
            "ant,comm,radio",
            "beacon",
            "light,spot",
            "piston",
            "rotor,hinge",
            "warhead",
            "sensor",
            "camera",
            "parachute",
            "tcb,customturret"
        };

        private static bool FallbackIs(int k, IMyTerminalBlock b)
        {
            switch (k)
            {
                case 0: return b is IMyBatteryBlock;
                case 1: return b is IMyReactor;
                case 2: return b is IMySolarPanel;
                case 3: return b is IMyWindTurbine;
                case 4: return b is IMyGasTank;
                case 5: return b is IMyAirVent;
                case 6: return b is IMyCargoContainer;
                case 7: return b is IMyJumpDrive;
                case 8: return b is IMyThrust;
                case 9: return b is IMyMotorSuspension;
                case 10: return b is IMyDoor;
                case 11: return b is IMyShipConnector;
                case 12: return b is IMyAssembler;
                case 13: return b is IMyRefinery;
                case 14: return b is IMyShipDrill;
                case 15: return b is IMyShipWelder;
                case 16: return b is IMyShipGrinder;
                case 17: return b is IMyGasGenerator;
                case 18: return b is IMyLargeTurretBase;
                case 19: return b is IMyProjector;
                case 20: return b is IMyGyro;
                case 21: return b is IMyGravityGeneratorBase;
                case 22: return b is IMyLandingGear;
                case 23: return b is IMyRadioAntenna;
                case 24: return b is IMyBeacon;
                case 25: return b is IMyLightingBlock;
                case 26: return b is IMyPistonBase;
                case 27: return b is IMyMotorStator;
                case 28: return b is IMyWarhead;
                case 29: return b is IMySensorBlock;
                case 30: return b is IMyCameraBlock;
                case 31: return b is IMyParachute;
                case 32: return b is IMyTurretControlBlock;
            }
            return false;
        }

        private void ResolveFallbackBlocks(BindDef bind, List<string> targets, List<IMyTerminalBlock> blocks)
        {
            string hint = (bind.Name + " " + string.Join(" ", targets)).ToLowerInvariant();
            var kinds = new List<int>();
            for (int k = 0; k < FallbackWords.Length; k++)
            {
                foreach (var w in FallbackWords[k].Split(','))
                    if (hint.Contains(w)) { kinds.Add(k); break; }
            }
            if (kinds.Count == 0) return;

            var all = AllBlocks();
            var other = new List<IMyTerminalBlock>();
            for (int i = 0; i < all.Count; i++)
            {
                var b = all[i];
                foreach (int k in kinds)
                {
                    if (FallbackIs(k, b)) { (b.CubeGrid == Me.CubeGrid ? blocks : other).Add(b); break; }
                }
            }
            if (blocks.Count == 0) blocks.AddRange(other);
        }


        private void RunSelfDiagnostics()
        {
            Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");
            if (_config == null || _config.Screens.Count == 0)
            {
                Echo("[WARN] No active @screen configured in Custom Data.");
                return;
            }

            Echo($"Configuration: {_config.Screens.Count} screens, {_config.Binds.Count} binds");
            Echo("--- BIND RESOLUTION ---");
            foreach (var bind in _config.Binds)
            {
                if (bind.Kind == BindKind.Dynamic)
                {
                    string k = !string.IsNullOrEmpty(bind.DynamicKey) ? bind.DynamicKey : bind.Name;
                    double v = _dynamicValues.ContainsKey(k) ? _dynamicValues[k] : bind.DefaultValue;
                    Echo($"[OK] @bind {bind.Name} (dynamic variable '{k}' = {v})");
                    continue;
                }

                if (_resolvedBinds.TryGetValue(bind.Name, out var blocks))
                {
                    if (blocks.Count > 0)
                    {
                        string fbTag = _fallbackUsed.ContainsKey(bind.Name) && _fallbackUsed[bind.Name] ? " [fallback: typed match]" : "";
                        Echo($"[OK] @bind {bind.Name} ({blocks.Count} blocks matched{fbTag})");
                    }
                    else
                    {
                        string targets = string.Join(", ", bind.TargetNames.Count > 0 ? bind.TargetNames : new List<string> { bind.TargetName });
                        Echo($"[WARN] @bind {bind.Name} -> 0 blocks matched \"{targets}\"");
                    }
                }
                else
                {
                    Echo($"[ERR] @bind {bind.Name} -> unresolved");
                }
            }

            if (_dynamicValues.Count > 0)
            {
                Echo($"--- DYNAMIC VARIABLES ({_dynamicValues.Count}) ---");
                foreach (var kvp in _dynamicValues)
                    Echo($"[VAR] {kvp.Key} = {kvp.Value}");
            }

            Echo("--- SURFACE OUTPUT AUDIT ---");
            foreach (var screen in _config.Screens)
            {
                if (_resolvedOutputs.TryGetValue(screen, out var surface))
                {
                    var size = surface.SurfaceSize;
                    var stats = Layout.ComputeStats(screen);
                    Echo($"[OK] Screen \"{screen.Name}\" [{screen.Profile}] -> {screen.OutputBlockName} [#{screen.OutputSurfaceIndex}] ({size.X:0}x{size.Y:0} px, {stats.TotalAllocatedSprites} sprites)");
                }
                else
                {
                    Echo($"[ERR] Screen \"{screen.Name}\" -> Missing block \"{screen.OutputBlockName}\" [#{screen.OutputSurfaceIndex}]");
                }
            }
            Echo("--- PERFORMANCE STATUS ---");
            Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");
            Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");
        }

        // ---------------- per-block "percent" resolution ----------------
        //
        // Comprehensive vanilla Space Engineers block telemetry:
        // - Batteries: CurrentStoredPower / MaxStoredPower
        // - Jump Drives: CurrentStoredPower / MaxStoredPower
        // - Gas Tanks (O2 & H2): FilledRatio
        // - Air Vents: GetOxygenLevel()
        // - Cargo Containers: CurrentVolume / MaxVolume
        // - Reactors: CurrentOutput / MaxOutput
        // - Solar Panels & Wind Turbines: CurrentOutput / MaxOutput
        // - Doors & Airlocks: OpenRatio
        // - Thrusters: CurrentThrust / MaxEffectiveThrust
        // - Motor Suspensions: Brake / Friction
        // - Connectors: Connected status
        // - Functional Blocks: Enabled status fallback

        private double ResolvePercent(IMyTerminalBlock block)
        {
            if (block == null) return 0.0;

            var battery = block as IMyBatteryBlock;
            if (battery != null)
                return battery.MaxStoredPower > 0 ? (battery.CurrentStoredPower / battery.MaxStoredPower) * 100.0 : 0.0;

            var jumpDrive = block as IMyJumpDrive;
            if (jumpDrive != null)
                return jumpDrive.MaxStoredPower > 0 ? (jumpDrive.CurrentStoredPower / jumpDrive.MaxStoredPower) * 100.0 : 0.0;

            var tank = block as IMyGasTank;
            if (tank != null)
                return tank.FilledRatio * 100.0;

            var vent = block as IMyAirVent;
            if (vent != null)
                return vent.GetOxygenLevel() * 100.0;

            var cargo = block as IMyCargoContainer;
            if (cargo != null)
            {
                var inv = cargo.GetInventory();
                if (inv != null && inv.MaxVolume.RawValue > 0)
                    return (double)inv.CurrentVolume.RawValue / inv.MaxVolume.RawValue * 100.0;
                return 0.0;
            }

            var reactor = block as IMyReactor;
            if (reactor != null)
                return reactor.MaxOutput > 0 ? (reactor.CurrentOutput / reactor.MaxOutput) * 100.0 : 0.0;

            var solar = block as IMySolarPanel;
            if (solar != null)
                return solar.MaxOutput > 0 ? (solar.CurrentOutput / solar.MaxOutput) * 100.0 : 0.0;

            var wind = block as IMyWindTurbine;
            if (wind != null)
                return wind.MaxOutput > 0 ? (wind.CurrentOutput / wind.MaxOutput) * 100.0 : 0.0;

            var door = block as IMyDoor;
            if (door != null)
                return (double)door.OpenRatio * 100.0;

            var thrust = block as IMyThrust;
            if (thrust != null)
                return thrust.MaxEffectiveThrust > 0 ? (thrust.CurrentThrust / thrust.MaxEffectiveThrust) * 100.0 : 0.0;

            var susp = block as IMyMotorSuspension;
            if (susp != null)
                return susp.Brake ? 100.0 : (susp.Friction * 100.0);

            var connector = block as IMyShipConnector;
            if (connector != null)
                return connector.Status == MyShipConnectorStatus.Connected ? 100.0 : (connector.Status == MyShipConnectorStatus.Connectable ? 50.0 : 0.0);

            var prod = block as IMyProductionBlock;
            if (prod != null)
                return prod.IsProducing ? 100.0 : 0.0;

            var drill = block as IMyShipDrill;
            if (drill != null)
                return drill.Enabled ? 100.0 : 0.0;

            var tool = block as IMyShipToolBase;
            if (tool != null)
                return tool.Enabled ? 100.0 : 0.0;

            var gen = block as IMyGasGenerator;
            if (gen != null)
                return gen.AutoRefill ? 100.0 : (gen.Enabled ? 50.0 : 0.0);

            var turret = block as IMyLargeTurretBase;
            if (turret != null)
                return turret.IsShooting ? 100.0 : (turret.HasTarget ? 50.0 : (turret.Enabled ? 10.0 : 0.0));

            var proj = block as IMyProjector;
            if (proj != null)
                return proj.TotalBlocks > 0 ? ((double)proj.BuiltBlocks / proj.TotalBlocks) * 100.0 : 0.0;

            var gyro = block as IMyGyro;
            if (gyro != null)
                return gyro.GyroOverride ? 100.0 : (gyro.Enabled ? 50.0 : 0.0);

            var grav = block as IMyGravityGeneratorBase;
            if (grav != null)
                return Math.Min(100.0, Math.Abs(grav.GravityAcceleration) / 9.81 * 100.0);

            var gear = block as IMyLandingGear;
            if (gear != null)
                return gear.IsLocked ? 100.0 : (gear.LockMode == LandingGearMode.ReadyToLock ? 50.0 : 0.0);

            var ant = block as IMyRadioAntenna;
            if (ant != null)
                return (ant.Radius / 50000.0) * 100.0;

            var beacon = block as IMyBeacon;
            if (beacon != null)
                return (beacon.Radius / 50000.0) * 100.0;

            var light = block as IMyLightingBlock;
            if (light != null)
                return light.Intensity * 10.0;

            var piston = block as IMyPistonBase;
            if (piston != null)
                return piston.HighestPosition > 0 ? (piston.CurrentPosition / piston.HighestPosition) * 100.0 : 0.0;

            var rotor = block as IMyMotorStator;
            if (rotor != null)
                return Math.Min(100.0, Math.Abs(rotor.Angle) / 6.28318 * 100.0);

            var warhead = block as IMyWarhead;
            if (warhead != null)
                return warhead.IsArmed ? 100.0 : 0.0;

            var sensor = block as IMySensorBlock;
            if (sensor != null)
                return !sensor.LastDetectedEntity.IsEmpty() ? 100.0 : (sensor.Enabled ? 25.0 : 0.0);

            var camera = block as IMyCameraBlock;
            if (camera != null)
                return camera.IsActive ? 100.0 : (camera.Enabled ? 50.0 : 0.0);

            var chute = block as IMyParachute;
            if (chute != null)
                return (double)chute.OpenRatio * 100.0;

            var tcb = block as IMyTurretControlBlock;
            if (tcb != null)
                return tcb.HasTarget ? 100.0 : (tcb.IsUnderControl ? 50.0 : (tcb.Enabled ? 10.0 : 0.0));

            var func = block as IMyFunctionalBlock;
            if (func != null)
                return func.Enabled ? 100.0 : 0.0;

            return 0.0;
        }

        private double ResolveBlockField(IMyTerminalBlock block, string field)
        {
            if (block == null) return 0.0;
            string f = field.ToLowerInvariant();
            if (f == "percent" || f == "pct") return ResolvePercent(block);

            var battery = block as IMyBatteryBlock;
            if (battery != null)
            {
                if (f == "current" || f == "stored" || f == "val" || f == "value" || f == "raw") return battery.CurrentStoredPower;
                if (f == "max" || f == "capacity") return battery.MaxStoredPower;
                if (f == "output") return battery.CurrentOutput;
                if (f == "maxoutput") return battery.MaxOutput;
                if (f == "input") return battery.CurrentInput;
                if (f == "maxinput") return battery.MaxInput;
                return battery.CurrentStoredPower;
            }

            var jumpDrive = block as IMyJumpDrive;
            if (jumpDrive != null)
            {
                if (f == "max" || f == "capacity") return jumpDrive.MaxStoredPower;
                return jumpDrive.CurrentStoredPower;
            }

            var tank = block as IMyGasTank;
            if (tank != null)
            {
                if (f == "max" || f == "capacity") return tank.Capacity;
                return tank.FilledRatio * tank.Capacity;
            }

            var vent = block as IMyAirVent;
            if (vent != null)
            {
                if (f == "max" || f == "capacity") return 100.0;
                return vent.GetOxygenLevel() * 100.0;
            }

            var cargo = block as IMyCargoContainer;
            if (cargo != null)
            {
                var inv = cargo.GetInventory();
                if (inv != null)
                {
                    if (f == "mass") return (double)inv.CurrentMass;
                    if (f == "max" || f == "capacity") return (double)inv.MaxVolume;
                    return (double)inv.CurrentVolume;
                }
                return 0.0;
            }

            var pwr = block as IMyPowerProducer;
            if (pwr != null)
            {
                if (f == "max" || f == "capacity") return pwr.MaxOutput;
                return pwr.CurrentOutput;
            }

            var door = block as IMyDoor;
            if (door != null)
            {
                if (f == "max") return 1.0;
                return (double)door.OpenRatio;
            }

            var thrust = block as IMyThrust;
            if (thrust != null)
            {
                if (f == "max" || f == "capacity") return thrust.MaxEffectiveThrust;
                return thrust.CurrentThrust;
            }

            var susp = block as IMyMotorSuspension;
            if (susp != null)
            {
                if (f == "brake") return susp.Brake ? 1.0 : 0.0;
                return susp.Friction * 100.0;
            }

            var connector = block as IMyShipConnector;
            if (connector != null)
            {
                return connector.Status == MyShipConnectorStatus.Connected ? 1.0 : 0.0;
            }

            var prod = block as IMyProductionBlock;
            if (prod != null)
            {
                if (f == "producing" || f == "active" || f == "running" || f == "state") return prod.IsProducing ? 1.0 : 0.0;
                if (f == "queue") return prod.IsQueueEmpty ? 0.0 : 1.0;
            }

            var asm = block as IMyAssembler;
            if (asm != null)
            {
                if (f == "disassembling") return asm.Mode == MyAssemblerMode.Disassembly ? 1.0 : 0.0;
                if (f == "repeating" || f == "loop") return asm.Repeating ? 1.0 : 0.0;
            }

            var gen = block as IMyGasGenerator;
            if (gen != null)
            {
                if (f == "autorefill") return gen.AutoRefill ? 1.0 : 0.0;
                if (f == "active") return gen.Enabled ? 1.0 : 0.0;
            }

            var turret = block as IMyLargeTurretBase;
            if (turret != null)
            {
                if (f == "shooting") return turret.IsShooting ? 1.0 : 0.0;
                if (f == "target" || f == "hastarget") return turret.HasTarget ? 1.0 : 0.0;
                if (f == "range") return turret.Range;
            }

            var proj = block as IMyProjector;
            if (proj != null)
            {
                if (f == "built") return proj.BuiltBlocks;
                if (f == "remaining") return proj.RemainingBlocks;
                if (f == "total") return proj.TotalBlocks;
                if (f == "projecting") return proj.IsProjecting ? 1.0 : 0.0;
            }

            var gyro = block as IMyGyro;
            if (gyro != null)
            {
                if (f == "override") return gyro.GyroOverride ? 1.0 : 0.0;
                if (f == "pitch") return gyro.Pitch;
                if (f == "yaw") return gyro.Yaw;
                if (f == "roll") return gyro.Roll;
                if (f == "power") return gyro.GyroPower * 100.0;
            }

            var grav = block as IMyGravityGeneratorBase;
            if (grav != null)
            {
                if (f == "g" || f == "accel") return grav.GravityAcceleration;
            }

            var gear = block as IMyLandingGear;
            if (gear != null)
            {
                if (f == "locked") return gear.IsLocked ? 1.0 : 0.0;
            }

            var ant = block as IMyRadioAntenna;
            if (ant != null)
            {
                if (f == "radius" || f == "range") return ant.Radius;
            }

            var beacon = block as IMyBeacon;
            if (beacon != null)
            {
                if (f == "radius" || f == "range") return beacon.Radius;
            }

            var light = block as IMyLightingBlock;
            if (light != null)
            {
                if (f == "intensity") return light.Intensity;
                if (f == "radius") return light.Radius;
            }

            var piston = block as IMyPistonBase;
            if (piston != null)
            {
                if (f == "pos" || f == "position" || f == "extension") return piston.CurrentPosition;
                if (f == "velocity") return piston.Velocity;
                if (f == "max") return piston.HighestPosition;
            }

            var rotor = block as IMyMotorStator;
            if (rotor != null)
            {
                if (f == "angle" || f == "deg") return rotor.Angle * 180.0 / Math.PI;
                if (f == "rad") return rotor.Angle;
                if (f == "velocity" || f == "rpm") return rotor.TargetVelocityRPM;
                if (f == "locked") return rotor.RotorLock ? 1.0 : 0.0;
            }

            var warhead = block as IMyWarhead;
            if (warhead != null)
            {
                if (f == "armed") return warhead.IsArmed ? 1.0 : 0.0;
                if (f == "countdown") return warhead.CountdownTime;
            }

            var sensor = block as IMySensorBlock;
            if (sensor != null)
            {
                if (f == "detected") return !sensor.LastDetectedEntity.IsEmpty() ? 1.0 : 0.0;
            }

            var camera = block as IMyCameraBlock;
            if (camera != null)
            {
                if (f == "range") return camera.AvailableScanRange;
                if (f == "active") return camera.IsActive ? 1.0 : 0.0;
            }

            var chute = block as IMyParachute;
            if (chute != null)
            {
                if (f == "open" || f == "openratio") return (double)chute.OpenRatio;
            }

            var tcb = block as IMyTurretControlBlock;
            if (tcb != null)
            {
                if (f == "target" || f == "hastarget") return tcb.HasTarget ? 1.0 : 0.0;
                if (f == "azimuth") return tcb.Azimuth * 180.0 / Math.PI;
                if (f == "elevation") return tcb.Elevation * 180.0 / Math.PI;
            }

            var func = block as IMyFunctionalBlock;
            if (func != null)
            {
                return func.Enabled ? 1.0 : 0.0;
            }

            return 0.0;
        }

        private double ResolveFieldValue(string bindName, string field)
        {
            BindDef bindDef;
            _bindMap.TryGetValue(bindName, out bindDef);
            if (bindDef != null && bindDef.Kind == BindKind.Dynamic)
            {
                string k = !string.IsNullOrEmpty(bindDef.DynamicKey) ? bindDef.DynamicKey : bindName;
                if (_dynamicValues.TryGetValue(k, out var dVal)) return dVal;
                return bindDef.DefaultValue;
            }
            if (_dynamicValues.TryGetValue(bindName, out var dynVal)) return dynVal;

            if (!_resolvedBinds.TryGetValue(bindName, out var blocks) || blocks.Count == 0)
                throw new InvalidOperationException($"HELM: bind '{bindName}' resolved to zero blocks — check the name/typo");

            if (blocks.Count == 1)
                return ResolveBlockField(blocks[0], field);

            var values = new List<double>();
            foreach (var b in blocks) values.Add(ResolveBlockField(b, field));
            return Aggregator.Aggregate(bindDef != null ? bindDef.Aggregation : Agg.Avg, values);
        }

        private double ResolvePercentForBind(string bindName)
        {
            BindDef bindDef;
            _bindMap.TryGetValue(bindName, out bindDef);
            if (bindDef != null && bindDef.Kind == BindKind.Dynamic)
            {
                string k = !string.IsNullOrEmpty(bindDef.DynamicKey) ? bindDef.DynamicKey : bindName;
                if (_dynamicValues.TryGetValue(k, out var dVal)) return dVal;
                return bindDef.DefaultValue;
            }
            if (_dynamicValues.TryGetValue(bindName, out var dynVal)) return dynVal;

            if (!_resolvedBinds.TryGetValue(bindName, out var blocks) || blocks.Count == 0) return 0.0;
            if (blocks.Count == 1) return ResolvePercent(blocks[0]);

            var values = new List<double>();
            foreach (var b in blocks) values.Add(ResolvePercent(b));
            return Aggregator.Aggregate(bindDef != null ? bindDef.Aggregation : Agg.Avg, values);
        }

        private static string FormatDisplayValue(double value, LineDef line)
        {
            if (line == null || (string.IsNullOrEmpty(line.Unit) && string.IsNullOrEmpty(line.Format) && line.Field == "percent"))
            {
                return GetPercentString(value);
            }
            return Layout.FormatDisplayValue(value, line.Unit, line.Format);
        }

        // ---------------- rendering ----------------

        private void RenderScreen(ScreenDef screen)
        {
            if (!_resolvedOutputs.TryGetValue(screen, out var surface))
                return;

            if (screen.Border.Equals("ascii", StringComparison.OrdinalIgnoreCase))
            {
                RenderScreenAscii(screen, surface);
            }
            else
            {
                RenderScreenGraphic(screen, surface);
            }
        }

        // =========================================================================
        // Approach B: Retained Virtual Sprite Buffer (Geometric quads + in-place mutation)
        // =========================================================================

        private static void AddQuad(ScreenBuffer buf, Vector2 pos, Vector2 size, Color col, float rot = 0f, string data = "SquareSimple")
        {
            buf.Sprites.Add(new MySprite { Type = SpriteType.TEXTURE, Data = data, Position = pos, Size = size, Color = col, Rotation = rot });
        }

        private void BuildGraphicBuffer(ScreenDef screen, IMyTextSurface surface, ScreenBuffer buf)
        {
            buf.Sprites.Clear();
            buf.Slots.Clear();

            Vector2 surfaceSize = surface.SurfaceSize;
            Vector2 textureSize = surface.TextureSize;
            Vector2 viewportOffset = (textureSize - surfaceSize) / 2f;

            float paddingX = Math.Max(4f, surfaceSize.X * 0.02f);
            float paddingY = Math.Max(4f, surfaceSize.Y * 0.02f);
            if (screen.Profile == "compact" || (screen.Profile == "auto" && surfaceSize.Y <= 260f && surfaceSize.Y > 150f))
            {
                paddingX = 4f;
                paddingY = 4f;
            }
            else if (screen.Profile == "corner" || (screen.Profile == "auto" && surfaceSize.Y <= 150f))
            {
                paddingX = 2f;
                paddingY = 2f;
            }
            float usableW = surfaceSize.X - (paddingX * 2f);

            Color bgCol = new Color(10, 12, 16);
            bool drawBg = true;
            if (!string.IsNullOrEmpty(screen.Background))
            {
                var hCol = HelmColor.Parse(screen.Background);
                if (hCol.IsTransparent) drawBg = false;
                else bgCol = new Color(hCol.R, hCol.G, hCol.B, hCol.A);
            }
            if (drawBg) AddQuad(buf, viewportOffset + (surfaceSize / 2f), surfaceSize - new Vector2(4f, 4f), bgCol);

            if (!screen.Border.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                Color borderColTop = screen.BorderGradientColors != null && screen.BorderGradientColors.Length > 0
                    ? ResolveColor(screen.BorderGradientColors[0])
                    : ResolveColor(screen.BorderColor ?? "40, 60, 85");
                Color borderColBot = screen.BorderGradientColors != null && screen.BorderGradientColors.Length > 1
                    ? ResolveColor(screen.BorderGradientColors[screen.BorderGradientColors.Length - 1])
                    : borderColTop;

                AddQuad(buf, new Vector2(viewportOffset.X + surfaceSize.X / 2f, viewportOffset.Y + paddingY * 0.5f), new Vector2(usableW, 2f), borderColTop);
                AddQuad(buf, new Vector2(viewportOffset.X + surfaceSize.X / 2f, viewportOffset.Y + surfaceSize.Y - (paddingY * 0.5f)), new Vector2(usableW, 2f), borderColBot);
            }

            var panels = screen.Panels;
            if (panels.Count == 0)
            {
                panels = new List<PanelDef> { new PanelDef { Name = "main", Col = 0, Row = 0, SpanX = screen.GridCols, SpanY = screen.GridRows, Lines = screen.Lines } };
            }

            float gutter = 8f;
            foreach (var panel in panels)
            {
                var pRect = Layout.ComputePanelRect(
                    panel.Col, panel.Row, panel.SpanX, panel.SpanY,
                    screen.GridCols, screen.GridRows,
                    surfaceSize.X, surfaceSize.Y,
                    paddingX, paddingY, gutter, gutter
                );

                float pLeft = viewportOffset.X + pRect.X;
                float pTop = viewportOffset.Y + pRect.Y;
                float pWidth = pRect.Width;
                float pHeight = pRect.Height;

                if (!string.IsNullOrEmpty(panel.Background))
                {
                    var pBg = HelmColor.Parse(panel.Background);
                    if (!pBg.IsTransparent)
                        AddQuad(buf, new Vector2(pLeft + pWidth / 2f, pTop + pHeight / 2f), new Vector2(pWidth, pHeight), new Color(pBg.R, pBg.G, pBg.B, pBg.A));
                }
                else if (screen.GridCols > 1 || screen.GridRows > 1)
                {
                    AddQuad(buf, new Vector2(pLeft + pWidth / 2f, pTop + pHeight / 2f), new Vector2(pWidth, pHeight), new Color(14, 18, 24));
                }

                string font = panel.Font ?? screen.Font ?? "Monospace";
                float baseFontSize = Layout.GetProfileFontSize(screen.Profile, surfaceSize.X, surfaceSize.Y, screen.FontSize);
                float fontSize = panel.FontSize ?? baseFontSize;

                float headerH = 0f;
                if (!string.IsNullOrEmpty(panel.Title))
                {
                    headerH = 20f * fontSize;
                    var align = TextAlignment.LEFT;
                    float titleX = pLeft + 4f;
                    string aMode = panel.TitleAlign ?? screen.TitleAlign ?? "left";
                    if (aMode.Equals("center", StringComparison.OrdinalIgnoreCase))
                    {
                        align = TextAlignment.CENTER;
                        titleX = pLeft + (pWidth / 2f);
                    }
                    else if (aMode.Equals("right", StringComparison.OrdinalIgnoreCase))
                    {
                        align = TextAlignment.RIGHT;
                        titleX = pLeft + pWidth - 4f;
                    }

                    string tcStr = panel.TitleColor ?? screen.TitleColor;
                    Color titleCol = !string.IsNullOrEmpty(tcStr)
                        ? ResolveColor(tcStr)
                        : ResolveColor(panel.BorderColor ?? screen.BorderColor ?? "white");

                    var titleSprite = MySprite.CreateText(
                        panel.Title,
                        font,
                        titleCol,
                        fontSize * 0.9f,
                        align
                    );
                    titleSprite.Position = new Vector2(titleX, pTop + 2f);
                    buf.Sprites.Add(titleSprite);

                    string[] bGrad = panel.BorderGradientColors ?? screen.BorderGradientColors;
                    Color divCol = bGrad != null && bGrad.Length > 0
                        ? ResolveColor(bGrad[0])
                        : ResolveColor(panel.BorderColor ?? screen.BorderColor ?? "40, 60, 85");

                    AddQuad(buf, new Vector2(pLeft + pWidth / 2f, pTop + headerH - 2f), new Vector2(pWidth - 8f, 1f), divCol);
                }

                int lineCount = Math.Max(1, panel.Lines.Count);
                float panelUsableH = pHeight - headerH - 8f;
                float rowHeight = panelUsableH / lineCount;
                float innerPadX = 6f;

                for (int i = 0; i < panel.Lines.Count; i++)
                {
                    var line = panel.Lines[i];
                    var slot = new LineSlot { Line = line };

                    float rowY = pTop + headerH + 4f + (i * rowHeight);
                    float centerY = rowY + (rowHeight / 2f);
                    float textY = centerY - (11f * fontSize);

                    slot.LabelSpriteIndex = buf.Sprites.Count;
                    slot.HasDynamicLabel = line.Label != null && (line.Label.Contains("{val}") || line.Label.Contains("{percent}"));
                    var labelSprite = MySprite.CreateText(line.Label, font, Color.White, fontSize, TextAlignment.LEFT);
                    labelSprite.Position = new Vector2(pLeft + innerPadX, textY);
                    buf.Sprites.Add(labelSprite);

                    float labelWidth = Math.Max(24f * fontSize, (line.Label.Length + 1) * 12f * fontSize);
                    float valueWidth = (!string.IsNullOrEmpty(line.Unit) || !string.IsNullOrEmpty(line.Format)) ? 64f * fontSize : 52f * fontSize;

                    if (line.Kind != LineKind.Text)
                    {
                        float barLeft = pLeft + innerPadX + labelWidth;
                        float barRight = pLeft + pWidth - innerPadX - valueWidth;
                        float barWidth = Math.Max(16f, barRight - barLeft);
                        float barHeight = Math.Max(line.Kind == LineKind.Spark ? 8f : 4f, Math.Min((line.Kind == LineKind.Spark ? 20f : 16f) * fontSize, rowHeight * (line.Kind == LineKind.Spark ? 0.55f : 0.45f)));

                        slot.BarLeft = barLeft;
                        slot.BarMaxWidth = barWidth;
                        slot.BarHeight = barHeight;
                        slot.BarCenterY = centerY;

                        if (line.Kind == LineKind.Bar)
                        {
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(barWidth, barHeight), new Color(25, 30, 38));
                            slot.FillSpriteIndex = buf.Sprites.Count;
                            AddQuad(buf, new Vector2(barLeft, centerY), new Vector2(0f, barHeight), ResolveColor(line.FlatColor));
                        }
                        else if (line.Kind == LineKind.LedBar)
                        {
                            int segCount = 10;
                            float gap = 2f;
                            float segW = Math.Max(2f, (barWidth - (gap * (segCount - 1))) / segCount);
                            for (int s = 0; s < segCount; s++)
                            {
                                slot.ExtraSpriteIndices.Add(buf.Sprites.Count);
                                AddQuad(buf, new Vector2(barLeft + s * (segW + gap) + (segW / 2f), centerY), new Vector2(segW, barHeight), new Color(22, 26, 32));
                            }
                        }
                        else if (line.Kind == LineKind.Spark)
                        {
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(barWidth, barHeight), new Color(20, 24, 30));
                            int pointCount = 8;
                            float gap = 2f;
                            float colW = Math.Max(2f, (barWidth - (gap * (pointCount - 1))) / pointCount);
                            for (int p = 0; p < pointCount; p++)
                            {
                                slot.ExtraSpriteIndices.Add(buf.Sprites.Count);
                                AddQuad(buf, new Vector2(barLeft + p * (colW + gap) + (colW / 2f), centerY), new Vector2(colW, 2f), ResolveColor(line.FlatColor));
                            }
                        }
                        else if (line.Kind == LineKind.Hazard)
                        {
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(barWidth, barHeight), new Color(20, 24, 30));
                            int stripeCount = 10;
                            float gap = 2f;
                            float stripeW = Math.Max(2f, (barWidth - (gap * (stripeCount - 1))) / stripeCount);
                            for (int s = 0; s < stripeCount; s++)
                            {
                                slot.ExtraSpriteIndices.Add(buf.Sprites.Count);
                                AddQuad(buf, new Vector2(barLeft + s * (stripeW + gap) + (stripeW / 2f), centerY), new Vector2(stripeW, barHeight), new Color(24, 28, 36), 0.523599f);
                            }
                        }
                        else if (line.Kind == LineKind.Chevron)
                        {
                            int arrCount = 10;
                            float gap = 2f;
                            float arrW = Math.Max(2f, (barWidth - (gap * (arrCount - 1))) / arrCount);
                            for (int s = 0; s < arrCount; s++)
                            {
                                slot.ExtraSpriteIndices.Add(buf.Sprites.Count);
                                AddQuad(buf, new Vector2(barLeft + s * (arrW + gap) + (arrW / 2f), centerY), new Vector2(arrW, barHeight), new Color(24, 28, 36), 1.570796f, "Triangle");
                            }
                        }
                        else if (line.Kind == LineKind.Dots)
                        {
                            int dotCount = 12;
                            float gap = 2f;
                            float dotW = Math.Max(2f, (barWidth - (gap * (dotCount - 1))) / dotCount);
                            float dotDim = Math.Min(dotW, barHeight);
                            for (int s = 0; s < dotCount; s++)
                            {
                                slot.ExtraSpriteIndices.Add(buf.Sprites.Count);
                                AddQuad(buf, new Vector2(barLeft + s * (dotW + gap) + (dotW / 2f), centerY), new Vector2(dotDim, dotDim), new Color(24, 28, 36), 0f, "Circle");
                            }
                        }
                        else if (line.Kind == LineKind.Balance)
                        {
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(barWidth, barHeight), new Color(25, 30, 38));
                            slot.FillSpriteIndex = buf.Sprites.Count;
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(0f, barHeight), ResolveColor(line.FlatColor));
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(2f, barHeight + 4f), new Color(80, 110, 150));
                        }
                        else if (line.Kind == LineKind.Toggle)
                        {
                            // Pill background track + sliding knob quad
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(barWidth, barHeight), new Color(20, 24, 30));
                            slot.FillSpriteIndex = buf.Sprites.Count;
                            float knobSize = Math.Max(4f, barHeight);
                            AddQuad(buf, new Vector2(barLeft + knobSize * 0.5f, centerY), new Vector2(knobSize, knobSize), new Color(40, 50, 65), 0f, "Circle");
                        }
                        else if (line.Kind == LineKind.Checkbox)
                        {
                            // Square outer frame box + inner check pip quad
                            float boxSize = Math.Max(6f, Math.Min(barHeight, 14f * fontSize));
                            AddQuad(buf, new Vector2(barLeft + boxSize * 0.5f, centerY), new Vector2(boxSize, boxSize), new Color(28, 34, 46));
                            slot.FillSpriteIndex = buf.Sprites.Count;
                            AddQuad(buf, new Vector2(barLeft + boxSize * 0.5f, centerY), new Vector2(boxSize * 0.6f, boxSize * 0.6f), new Color(40, 50, 65));
                        }
                        else if (line.Kind == LineKind.Status)
                        {
                            // Pill card container + status light pip circle
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(barWidth, barHeight), new Color(18, 22, 30));
                            slot.FillSpriteIndex = buf.Sprites.Count;
                            float pipSize = Math.Max(4f, barHeight * 0.7f);
                            AddQuad(buf, new Vector2(barLeft + pipSize + 2f, centerY), new Vector2(pipSize, pipSize), new Color(34, 197, 94), 0f, "Circle");
                        }
                        else if (line.Kind == LineKind.Ammo)
                        {
                            // 8 ammo cartridge pips
                            int pipCount = 8;
                            float gap = 2f;
                            float pipW = Math.Max(2f, (barWidth - (gap * (pipCount - 1))) / pipCount);
                            for (int p = 0; p < pipCount; p++)
                            {
                                slot.ExtraSpriteIndices.Add(buf.Sprites.Count);
                                AddQuad(buf, new Vector2(barLeft + p * (pipW + gap) + (pipW / 2f), centerY), new Vector2(pipW, barHeight), new Color(24, 28, 36));
                            }
                        }
                        else if (line.Kind == LineKind.Flow)
                        {
                            // Flow background channel + dynamic directional triangle indicator
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(barWidth, barHeight), new Color(20, 24, 30));
                            slot.FillSpriteIndex = buf.Sprites.Count;
                            float indW = Math.Max(4f, barHeight);
                            AddQuad(buf, new Vector2(barLeft + (barWidth / 2f), centerY), new Vector2(indW, indW), ResolveColor(line.FlatColor), 0f, "Triangle");
                        }
                    }

                    var valSprite = MySprite.CreateText("---", font, Color.White, fontSize, TextAlignment.RIGHT);
                    valSprite.Position = new Vector2(pLeft + pWidth - innerPadX, textY);
                    slot.ValSpriteIndex = buf.Sprites.Count;
                    buf.Sprites.Add(valSprite);

                    buf.Slots.Add(slot);
                }
            }

            buf.LastSurfaceSize = surfaceSize;
            buf.Initialized = true;
        }


        private void RenderScreenGraphic(ScreenDef screen, IMyTextSurface surface)
        {
            if (!_screenBuffers.TryGetValue(screen, out var buf))
            {
                buf = new ScreenBuffer();
                _screenBuffers[screen] = buf;
            }

            if (!buf.Initialized || buf.LastSurfaceSize != surface.SurfaceSize)
            {
                BuildGraphicBuffer(screen, surface, buf);
            }

            bool dirty = false;
            bool alertPhase = (_tickCounter % 16) < 8;

            for (int i = 0; i < buf.Slots.Count; i++)
            {
                var slot = buf.Slots[i];
                var line = slot.Line;
                double value;
                bool hasErr = false;

                try
                {
                    value = ResolveFieldValue(line.BindName, line.Field);
                }
                catch
                {
                    hasErr = true;
                    value = 0.0;
                }

                if (hasErr)
                {
                    if (slot.LastValue != -99999.0 || (slot.ValSpriteIndex >= 0 && buf.Sprites[slot.ValSpriteIndex].Data != "ERR"))
                    {
                        dirty = true;
                        slot.LastValue = -99999.0;
                        if (slot.ValSpriteIndex >= 0)
                        {
                            var s = buf.Sprites[slot.ValSpriteIndex];
                            s.Data = "ERR";
                            s.Color = Color.Red;
                            buf.Sprites[slot.ValSpriteIndex] = s;
                        }
                    }
                    continue;
                }

                string colorName = line.Gradient
                    ? Layout.GradientColorAt(line.GradientColors, value)
                    : line.FlatColor;
                Color barCol = ResolveColor(colorName);
                bool isRedAlert = (barCol == Color.Red);

                if (isRedAlert && alertPhase)
                {
                    barCol = new Color(255, 75, 75);
                    dirty = true;
                }

                bool valueChanged = Math.Abs(value - slot.LastValue) > 0.1;
                if (valueChanged) slot.LastValue = value;

                if (line.Kind == LineKind.Bar)
                {
                    double fillVal = (line.Field == "percent" || line.Field == "pct") ? value : ResolvePercentForBind(line.BindName);
                    if (slot.DisplayValue < 0f) slot.DisplayValue = (float)fillVal;
                    float target = (float)fillVal;
                    if (Math.Abs(slot.DisplayValue - target) > 0.05f)
                    {
                        slot.DisplayValue += (target - slot.DisplayValue) * 0.35f;
                        dirty = true;
                    }
                    else if (slot.DisplayValue != target)
                    {
                        slot.DisplayValue = target;
                        dirty = true;
                    }

                    if (valueChanged) dirty = true;
                    if (dirty)
                    {
                        float pct = (float)Math.Max(0.0, Math.Min(1.0, slot.DisplayValue / 100.0));
                        float fillW = slot.BarMaxWidth * pct;

                        var fillSprite = buf.Sprites[slot.FillSpriteIndex];
                        fillSprite.Size = new Vector2(fillW, slot.BarHeight);
                        fillSprite.Position = new Vector2(slot.BarLeft + (fillW / 2f), slot.BarCenterY);
                        fillSprite.Color = barCol;
                        buf.Sprites[slot.FillSpriteIndex] = fillSprite;

                        var valSprite = buf.Sprites[slot.ValSpriteIndex];
                        valSprite.Data = FormatDisplayValue(value, line);
                        valSprite.Color = barCol;
                        buf.Sprites[slot.ValSpriteIndex] = valSprite;
                    }
                }
                else if (line.Kind == LineKind.LedBar || line.Kind == LineKind.Hazard || line.Kind == LineKind.Chevron || line.Kind == LineKind.Dots)
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        double fillVal = (line.Field == "percent" || line.Field == "pct") ? value : ResolvePercentForBind(line.BindName);
                        int segCount = slot.ExtraSpriteIndices.Count;
                        for (int s = 0; s < segCount; s++)
                        {
                            bool lit = fillVal >= ((s + 0.4) / (double)segCount * 100.0);
                            Color segCol = lit ? barCol : new Color(24, 28, 36);
                            var segSprite = buf.Sprites[slot.ExtraSpriteIndices[s]];
                            segSprite.Color = segCol;
                            buf.Sprites[slot.ExtraSpriteIndices[s]] = segSprite;
                        }

                        var valSprite = buf.Sprites[slot.ValSpriteIndex];
                        valSprite.Data = FormatDisplayValue(value, line);
                        valSprite.Color = barCol;
                        buf.Sprites[slot.ValSpriteIndex] = valSprite;
                    }
                }
                else if (line.Kind == LineKind.Spark)
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        for (int h = 0; h < 7; h++)
                            slot.SparkHistory[h] = slot.SparkHistory[h + 1];
                        slot.SparkHistory[7] = (float)value;

                        int pointCount = slot.ExtraSpriteIndices.Count;
                        for (int p = 0; p < pointCount; p++)
                        {
                            float hVal = slot.SparkHistory[p];
                            float hPct = Math.Max(0.06f, Math.Min(1f, hVal / 100f));
                            float colH = slot.BarHeight * hPct;
                            float colCenterY = slot.BarCenterY + (slot.BarHeight / 2f) - (colH / 2f);

                            var colSprite = buf.Sprites[slot.ExtraSpriteIndices[p]];
                            colSprite.Size = new Vector2(colSprite.Size.Value.X, colH);
                            colSprite.Position = new Vector2(colSprite.Position.Value.X, colCenterY);
                            colSprite.Color = barCol;
                            buf.Sprites[slot.ExtraSpriteIndices[p]] = colSprite;
                        }

                        var valSprite = buf.Sprites[slot.ValSpriteIndex];
                        valSprite.Data = FormatDisplayValue(value, line);
                        valSprite.Color = barCol;
                        buf.Sprites[slot.ValSpriteIndex] = valSprite;
                    }
                }
                else if (line.Kind == LineKind.Balance)
                {
                    if (slot.DisplayValue < 0f) slot.DisplayValue = (float)value;
                    float target = (float)value;
                    if (Math.Abs(slot.DisplayValue - target) > 0.05f)
                    {
                        slot.DisplayValue += (target - slot.DisplayValue) * 0.35f;
                        dirty = true;
                    }
                    else if (slot.DisplayValue != target)
                    {
                        slot.DisplayValue = target;
                        dirty = true;
                    }

                    if (dirty || valueChanged)
                    {
                        float halfW = slot.BarMaxWidth * 0.5f;
                        float centerPos = slot.BarLeft + halfW;
                        float dev = (float)Math.Max(-1.0, Math.Min(1.0, (slot.DisplayValue - 50.0) / 50.0));
                        float fillW = Math.Abs(dev) * halfW;
                        float fillX = (dev >= 0) ? (centerPos + fillW * 0.5f) : (centerPos - fillW * 0.5f);

                        var fillSprite = buf.Sprites[slot.FillSpriteIndex];
                        fillSprite.Size = new Vector2(fillW, slot.BarHeight);
                        fillSprite.Position = new Vector2(fillX, slot.BarCenterY);
                        fillSprite.Color = barCol;
                        buf.Sprites[slot.FillSpriteIndex] = fillSprite;

                        var valSprite = buf.Sprites[slot.ValSpriteIndex];
                        valSprite.Data = FormatDisplayValue(value, line);
                        valSprite.Color = barCol;
                        buf.Sprites[slot.ValSpriteIndex] = valSprite;
                    }
                }
                else if (line.Kind == LineKind.Toggle)
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        bool isOn = value >= 50.0;
                        float knobSize = Math.Max(4f, slot.BarHeight);
                        float knobX = isOn ? (slot.BarLeft + slot.BarMaxWidth - knobSize * 0.5f) : (slot.BarLeft + knobSize * 0.5f);
                        Color knobCol = isOn ? (barCol != Color.White ? barCol : new Color(34, 197, 94)) : new Color(60, 70, 85);

                        var fillSprite = buf.Sprites[slot.FillSpriteIndex];
                        fillSprite.Position = new Vector2(knobX, slot.BarCenterY);
                        fillSprite.Color = knobCol;
                        buf.Sprites[slot.FillSpriteIndex] = fillSprite;

                        var valSprite = buf.Sprites[slot.ValSpriteIndex];
                        valSprite.Data = isOn ? "ON" : "OFF";
                        valSprite.Color = knobCol;
                        buf.Sprites[slot.ValSpriteIndex] = valSprite;
                    }
                }
                else if (line.Kind == LineKind.Checkbox)
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        bool isChecked = value >= 50.0;
                        Color checkCol = isChecked ? (barCol != Color.White ? barCol : new Color(56, 189, 248)) : new Color(30, 36, 48);

                        var fillSprite = buf.Sprites[slot.FillSpriteIndex];
                        fillSprite.Color = checkCol;
                        buf.Sprites[slot.FillSpriteIndex] = fillSprite;

                        var valSprite = buf.Sprites[slot.ValSpriteIndex];
                        valSprite.Data = isChecked ? "ACTIVE" : "READY";
                        valSprite.Color = isChecked ? checkCol : new Color(148, 163, 184);
                        buf.Sprites[slot.ValSpriteIndex] = valSprite;
                    }
                }
                else if (line.Kind == LineKind.Status)
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        string statusLabel = value >= 75.0 ? "ONLINE" : (value >= 25.0 ? "STBY" : "OFFLINE");
                        Color statusCol = value >= 75.0 ? new Color(34, 197, 94) : (value >= 25.0 ? new Color(234, 179, 8) : new Color(239, 68, 68));

                        var fillSprite = buf.Sprites[slot.FillSpriteIndex];
                        fillSprite.Color = statusCol;
                        buf.Sprites[slot.FillSpriteIndex] = fillSprite;

                        var valSprite = buf.Sprites[slot.ValSpriteIndex];
                        valSprite.Data = statusLabel;
                        valSprite.Color = statusCol;
                        buf.Sprites[slot.ValSpriteIndex] = valSprite;
                    }
                }
                else if (line.Kind == LineKind.Ammo)
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        int pipCount = slot.ExtraSpriteIndices.Count;
                        for (int p = 0; p < pipCount; p++)
                        {
                            bool lit = value >= ((p + 0.5) / (double)pipCount * 100.0);
                            Color pipCol = lit ? barCol : new Color(24, 28, 36);
                            var pipSprite = buf.Sprites[slot.ExtraSpriteIndices[p]];
                            pipSprite.Color = pipCol;
                            buf.Sprites[slot.ExtraSpriteIndices[p]] = pipSprite;
                        }

                        var valSprite = buf.Sprites[slot.ValSpriteIndex];
                        valSprite.Data = FormatDisplayValue(value, line);
                        valSprite.Color = barCol;
                        buf.Sprites[slot.ValSpriteIndex] = valSprite;
                    }
                }
                else if (line.Kind == LineKind.Flow)
                {
                    dirty = true; // Rotates/updates dynamically
                    float rot;
                    Color flowCol;
                    string flowText;

                    bool isMachinery = (line.Field == "producing" || line.Field == "active" || line.Field == "running" || line.Field == "state" || (value <= 1.0 && line.Field != "percent"));
                    if (isMachinery)
                    {
                        if (value > 0.0)
                        {
                            slot.DisplayValue = (slot.DisplayValue + 0.3927f) % 6.28318f;
                            rot = slot.DisplayValue;
                            flowCol = new Color(34, 197, 94);
                            flowText = "RUN";
                        }
                        else
                        {
                            rot = 0f;
                            flowCol = new Color(148, 163, 184);
                            flowText = "IDLE";
                        }
                    }
                    else
                    {
                        rot = (value > 55.0) ? 0f : ((value < 45.0) ? 3.14159f : 1.57079f);
                        flowCol = (value > 55.0) ? new Color(34, 197, 94) : ((value < 45.0) ? new Color(239, 68, 68) : new Color(148, 163, 184));
                        flowText = (value > 55.0) ? "CHG" : ((value < 45.0) ? "DSC" : "STBY");
                    }

                    var fillSprite = buf.Sprites[slot.FillSpriteIndex];
                    fillSprite.Rotation = rot;
                    fillSprite.Color = flowCol;
                    buf.Sprites[slot.FillSpriteIndex] = fillSprite;

                    var valSprite = buf.Sprites[slot.ValSpriteIndex];
                    valSprite.Data = flowText;
                    valSprite.Color = flowCol;
                    buf.Sprites[slot.ValSpriteIndex] = valSprite;
                }
                else // Text line
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        Color txtCol = line.ColorRules != null
                            ? ResolveColor(Layout.ResolveConditionalColor(line.ColorRules, value))
                            : Color.White;

                        if (txtCol == Color.Red && alertPhase)
                            txtCol = new Color(255, 75, 75);

                        var valSprite = buf.Sprites[slot.ValSpriteIndex];
                        valSprite.Data = FormatDisplayValue(value, line);
                        valSprite.Color = txtCol;
                        buf.Sprites[slot.ValSpriteIndex] = valSprite;
                    }
                }

                if (slot.HasDynamicLabel && (valueChanged || isRedAlert))
                {
                    dirty = true;
                    var lblSprite = buf.Sprites[slot.LabelSpriteIndex];
                    string pctStr = GetPercentString(value);
                    string rawVal = FormatDisplayValue(value, line);
                    lblSprite.Data = line.Label.Replace("{percent}", pctStr).Replace("{val}", rawVal);
                    buf.Sprites[slot.LabelSpriteIndex] = lblSprite;
                }
            }

            if (dirty)
            {
                using (var frame = surface.DrawFrame())
                {
                    frame.AddRange(buf.Sprites);
                }
            }
        }

        // =========================================================================
        // Legacy/Retro ASCII Bordered Window Rendering
        // =========================================================================

        private void RenderScreenAscii(ScreenDef screen, IMyTextSurface surface)
        {
            surface.FontSize = screen.FontSize;

            var contentLines = new List<Tuple<string, string>>();

            foreach (var line in screen.Lines)
            {
                double value = 0.0;
                string text;
                string color = null;

                try
                {
                    value = ResolveFieldValue(line.BindName, line.Field);
                }
                catch (Exception ex)
                {
                    contentLines.Add(Tuple.Create($"{line.Label}: [UNRESOLVED]", "red"));
                    continue;
                }

                string valStr = FormatDisplayValue(value, line);

                if (line.Kind == LineKind.Bar)
                {
                    double fillVal = (line.Field == "percent" || line.Field == "pct") ? value : ResolvePercentForBind(line.BindName);
                    text = Layout.RenderBarText(line.Label, fillVal, 10, valStr);
                    color = line.Gradient
                        ? Layout.GradientColorAt(line.GradientColors, fillVal)
                        : line.FlatColor;
                }
                else if (line.Kind == LineKind.Balance)
                {
                    text = Layout.RenderBalanceText(line.Label, value, 10);
                    color = line.Gradient
                        ? Layout.GradientColorAt(line.GradientColors, value)
                        : line.FlatColor;
                }
                else if (line.Kind == LineKind.Toggle)
                {
                    text = Layout.RenderToggleText(line.Label, value);
                    color = line.FlatColor ?? (value >= 50.0 ? "green" : "gray");
                }
                else if (line.Kind == LineKind.Checkbox)
                {
                    text = Layout.RenderCheckboxText(line.Label, value);
                    color = line.FlatColor ?? (value >= 50.0 ? "cyan" : "gray");
                }
                else if (line.Kind == LineKind.Status)
                {
                    text = Layout.RenderStatusText(line.Label, value);
                    color = line.FlatColor ?? (value >= 75.0 ? "lime" : (value >= 25.0 ? "yellow" : "red"));
                }
                else if (line.Kind == LineKind.Ammo)
                {
                    text = Layout.RenderAmmoText(line.Label, value, 8);
                    color = line.Gradient ? Layout.GradientColorAt(line.GradientColors, value) : (line.FlatColor ?? "yellow");
                }
                else if (line.Kind == LineKind.Flow)
                {
                    bool isMachinery = (line.Field == "producing" || line.Field == "active" || line.Field == "running" || line.Field == "state" || (value <= 1.0 && line.Field != "percent"));
                    if (isMachinery)
                    {
                        char[] spinChars = new[] { '|', '/', '-', '\\' };
                        char sc = spinChars[Math.Abs(_tickCounter) % 4];
                        text = value > 0 ? $"{line.Label} [{sc}] RUN" : $"{line.Label} [==] IDLE";
                        color = value > 0 ? "green" : "gray";
                    }
                    else
                    {
                        text = Layout.RenderFlowText(line.Label, value, _tickCounter);
                        color = line.FlatColor ?? (value > 55.0 ? "green" : (value < 45.0 ? "red" : "cyan"));
                    }
                }
                else
                {
                    text = $"{line.Label}: {valStr}";
                    color = line.ColorRules != null
                        ? Layout.ResolveConditionalColor(line.ColorRules, value)
                        : null;
                }

                contentLines.Add(Tuple.Create(text, color));
            }

            var borderedRaw = Layout.RenderBorderedWindow(
                new List<string>(contentLines.ConvertAll(t => t.Item1)),
                width: screen.Width
            );

            // Dirty-flag check
            string signature = string.Join("\n", borderedRaw);
            if (_lastSignature.TryGetValue(screen, out var prevSig) && prevSig == signature)
                return;
            _lastSignature[screen] = signature;

            using (var frame = surface.DrawFrame())
            {
                float y = 4f;
                float lineHeight = APPROX_LINE_HEIGHT_PER_FONTSIZE * screen.FontSize;
                try
                {
                    var measured = surface.MeasureStringInPixels(new StringBuilder("A"), "Monospace", screen.FontSize);
                    if (measured.Y > 0)
                        lineHeight = measured.Y;
                }
                catch { }

                for (int i = 0; i < borderedRaw.Count; i++)
                {
                    string colorName = null;
                    if (i > 0 && i < borderedRaw.Count - 1)
                        colorName = contentLines[i - 1].Item2;

                    var sprite = MySprite.CreateText(
                        borderedRaw[i],
                        "Monospace",
                        ResolveColor(colorName),
                        screen.FontSize,
                        TextAlignment.LEFT
                    );
                    sprite.Position = new Vector2(4f, y);
                    frame.Add(sprite);
                    y += lineHeight;
                }
            }
        }

        private Color ResolveColor(string name)
        {
            if (string.IsNullOrEmpty(name)) return Color.White;
            Color k;
            if (_colorCache.TryGetValue(name, out k)) return k;
            HelmColor c = HelmColor.Parse(name);
            k = c.IsTransparent ? Color.Transparent : new Color(c.R, c.G, c.B, c.A);
            _colorCache[name] = k;
            return k;
        }
    }

    // ==========================================================================
    // HELM CORE LIBRARY: DSL Parser, Layout, Color Engine, Aggregator
    // ==========================================================================

// ---------- Data model ----------

    public enum BindKind { Block, Group, Dynamic }
    public enum Agg { None, Avg, Sum, Min, Max }

    public class BindDef
    {
        public string Name;
        public BindKind Kind;
        public List<string> TargetNames = new List<string>();
        public string TargetName
        {
            get { return TargetNames.Count > 0 ? TargetNames[0] : ""; }
            set
            {
                if (TargetNames.Count == 0) TargetNames.Add(value);
                else TargetNames[0] = value;
            }
        }
        public Agg Aggregation = Agg.None;
        public double DefaultValue = 0.0;
        public string DynamicKey;
    }

    public enum LineKind { Text, Bar, LedBar, Spark, Hazard, Chevron, Dots, Balance, Toggle, Checkbox, Status, Ammo, Flow }

    public class ColorRule
    {
        public string Expr;   // e.g. "> 90" — left side (var.field) is implicit, this is the operator+value
        public string Color;
        public bool IsDefault;
    }

    public class LineDef
    {
        public LineKind Kind;
        public string Label;          // quoted display label, e.g. "PWR"
        public string BindName;       // which @bind this line reads, e.g. "power"
        public string Field;          // e.g. "percent" — v1 assumes bound values are already 0-100 doubles
        public bool Gradient;
        public string[] GradientColors;  // e.g. ["green","yellow","red"]
        public string FlatColor;
        public List<ColorRule> ColorRules; // for text() lines using color(expr:val, default:val)
        public string Direction = "right"; // "right", "left", "up", "down"
        public string Unit;           // e.g. "MW", "kL", "km", "t", "%"
        public string Format;         // e.g. "F1", "F0", "N0"
    }

    // ---------- Color Science / Parsing ----------

    public struct HelmColor
    {
        public byte R, G, B, A;
        public bool IsTransparent;

        public HelmColor(byte r, byte g, byte b, byte a = 255)
        {
            R = r; G = g; B = b; A = a;
            IsTransparent = false;
        }

        public static readonly HelmColor Transparent = new HelmColor(0, 0, 0, 0) { IsTransparent = true };
        public static readonly HelmColor White = new HelmColor(255, 255, 255);
        public static readonly HelmColor Black = new HelmColor(0, 0, 0);
        public static readonly HelmColor Red = new HelmColor(255, 0, 0);
        public static readonly HelmColor Green = new HelmColor(0, 255, 0);
        public static readonly HelmColor Lime = new HelmColor(50, 205, 50);
        public static readonly HelmColor Yellow = new HelmColor(255, 255, 0);
        public static readonly HelmColor Orange = new HelmColor(255, 140, 0);
        public static readonly HelmColor Cyan = new HelmColor(0, 255, 255);
        public static readonly HelmColor Blue = new HelmColor(0, 120, 255);
        public static readonly HelmColor Magenta = new HelmColor(255, 0, 255);
        public static readonly HelmColor Purple = new HelmColor(128, 0, 128);
        public static readonly HelmColor Gray = new HelmColor(128, 128, 128);
        public static readonly HelmColor DarkGray = new HelmColor(60, 60, 60);

        public static HelmColor Parse(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return White;
            str = str.Trim();
            if (str.Equals("transparent", StringComparison.OrdinalIgnoreCase) ||
                str.Equals("none", StringComparison.OrdinalIgnoreCase))
                return Transparent;

            if (str.StartsWith("#"))
            {
                string hex = str.Substring(1).Trim();
                if (hex.Length == 6)
                {
                    byte r = Convert.ToByte(hex.Substring(0, 2), 16);
                    byte g = Convert.ToByte(hex.Substring(2, 2), 16);
                    byte b = Convert.ToByte(hex.Substring(4, 2), 16);
                    return new HelmColor(r, g, b);
                }
                if (hex.Length == 3)
                {
                    byte r = Convert.ToByte(new string(hex[0], 2), 16);
                    byte g = Convert.ToByte(new string(hex[1], 2), 16);
                    byte b = Convert.ToByte(new string(hex[2], 2), 16);
                    return new HelmColor(r, g, b);
                }
            }

            if (str.Contains(","))
            {
                var parts = str.Split(',');
                if (parts.Length >= 3)
                {
                    byte r = (byte)Math.Max(0, Math.Min(255, int.Parse(parts[0].Trim())));
                    byte g = (byte)Math.Max(0, Math.Min(255, int.Parse(parts[1].Trim())));
                    byte b = (byte)Math.Max(0, Math.Min(255, int.Parse(parts[2].Trim())));
                    byte a = parts.Length > 3 ? (byte)Math.Max(0, Math.Min(255, int.Parse(parts[3].Trim()))) : (byte)255;
                    return new HelmColor(r, g, b, a);
                }
            }

            switch (str.ToLowerInvariant())
            {
                case "red": return Red;
                case "green": return Green;
                case "lime": return Lime;
                case "yellow": return Yellow;
                case "orange": return Orange;
                case "cyan": return Cyan;
                case "blue": return Blue;
                case "magenta": return Magenta;
                case "purple": return Purple;
                case "white": return White;
                case "black": return Black;
                case "gray": case "grey": return Gray;
                case "darkgray": case "darkgrey": return DarkGray;
                default: return White;
            }
        }
    }

    public class PanelDef
    {
        public string Name = "default";
        public int Col = 0;
        public int Row = 0;
        public int SpanX = 1;
        public int SpanY = 1;
        public string Title;
        public string TitleAlign = "left"; // "left", "center", "right"
        public string TitleColor;
        public string Border;
        public string Font;
        public float? FontSize;
        public string Background;
        public string BorderColor;
        public string[] BorderGradientColors;
        public string Orientation = "horizontal"; // "horizontal" or "vertical"
        public List<LineDef> Lines = new List<LineDef>();
    }

    public class ScreenDef
    {
        public string Name;
        public string Profile = "standard"; // "standard", "compact", "wide", "corner", "auto"
        public string Border = "graphic";
        public string BorderColor;
        public string[] BorderGradientColors;
        public string TitleAlign = "left";
        public string TitleColor;
        public string Font = "Monospace";
        public string Background;
        public string BootMessage;
        public int BootStaggerTicks = 10;
        public float BootDuration = 2.5f; // Seconds
        public string BootColor = "cyan";
        public string[] BootGradientColors;
        public List<string> BootLogs = new List<string>();

        public List<string> GetEffectiveBootLogs()
        {
            if (BootLogs != null && BootLogs.Count > 0) return BootLogs;
            return new List<string>
            {
                "helm-core initializing...",
                "seeking compliant surface displays...",
                "uploading firmware...",
                "awaiting compliance...",
                "helm-core online -- BOOT // SUCCESS"
            };
        }

        public float FontSize = 1.0f;
        public int RefreshTicks = 100;
        public int GridCols = 1;
        public int GridRows = 1;
        public int Width = 30; // default content width in characters
        public string OutputBlockName;
        public int OutputSurfaceIndex = 0;

        public List<PanelDef> Panels = new List<PanelDef>();

        // Backward compatibility helper: access first panel's lines
        public List<LineDef> Lines
        {
            get
            {
                if (Panels.Count == 0) Panels.Add(new PanelDef { Name = "main" });
                return Panels[0].Lines;
            }
        }
    }

    public class TemplateDef
    {
        public string Name;
        public List<string> ParamNames = new List<string>();
        public Dictionary<string, string> Defaults = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public List<string> BodyLines = new List<string>();
    }

    public class ParsedConfig
    {
        public List<BindDef> Binds = new List<BindDef>();
        public List<ScreenDef> Screens = new List<ScreenDef>();
        public List<string> Errors = new List<string>();
    }

    // ---------- Parser ----------

    public static class HelmParser
    {
        private static readonly Dictionary<string, TemplateDef> BuiltinTemplates
            = new Dictionary<string, TemplateDef>(StringComparer.OrdinalIgnoreCase);

        static HelmParser()
        {
            RegisterBuiltinTemplates();
        }

        private static void RegisterBuiltinTemplates()
        {
            RegisterTemplate("fighter_hud", new[] { "$screen=\"Cockpit Display\"", "$surf=0", "$pwr=\"Main Batteries\"", "$h2=\"Hydrogen Tanks\"", "$cargo=\"Cargo Containers\"" },
@"@bind pwr_$screen=group(""$pwr"",agg:avg)
@bind h2_$screen=group(""$h2"",agg:avg)
@bind crg_$screen=group(""$cargo"",agg:sum)
@screen $screen
output:""$screen"",$surf
layout:grid(2,1)
@panel sys at(0,0,1,1) title:""PROPULSION""
line:""PWR"" bar(pwr_$screen.percent)
line:""H2"" bar(h2_$screen.percent)
@panel hold at(1,0,1,1) title:""STORES""
line:""CARGO"" led(crg_$screen.percent)");

            RegisterTemplate("reactor_matrix", new[] { "$screen=\"Reactor LCD\"", "$surf=0", "$reactors=\"Reactors\"", "$batteries=\"Batteries\"" },
@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");

            RegisterTemplate("bridge_status", new[] { "$screen=\"Bridge LCD\"", "$surf=0", "$power=\"Batteries\"", "$o2=\"Air Vents\"", "$cargo=\"Cargo\"", "$jump=\"Jump Drives\"" },
@"@bind pwr_$screen=group(""$power"",agg:avg)
@bind o2_$screen=group(""$o2"",agg:avg)
@bind crg_$screen=group(""$cargo"",agg:sum)
@bind jmp_$screen=group(""$jump"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(2,2)
@panel pwr at(0,0,1,1) title:""POWER GRID""
line:""PWR"" bar(pwr_$screen.percent)
@panel life at(0,1,1,1) title:""LIFE SUPPORT""
line:""O2"" bar(o2_$screen.percent)
@panel nav at(1,0,1,1) title:""NAVIGATION""
line:""JUMP"" bar(jmp_$screen.percent)
@panel store at(1,1,1,1) title:""LOGISTICS""
line:""CARGO"" led(crg_$screen.percent)");

            RegisterTemplate("mining_ops", new[] { "$screen=\"Drill LCD\"", "$surf=0", "$cargo=\"Cargo Containers\"", "$drills=\"Drills\"" },
@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");

            RegisterTemplate("airlock", new[] { "$screen=\"Airlock LCD\"", "$surf=0", "$vent=\"Airlock Vent\"", "$outer=\"Outer Door\"", "$inner=\"Inner Door\"" },
@"@bind vent_$screen=airvent(""$vent"")
@bind outer_$screen=block(""$outer"")
@bind inner_$screen=block(""$inner"")
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel env at(0,0,1,1) title:""ATMOSPHERE""
line:""PRESS"" bar(vent_$screen.percent)
@panel doors at(0,1,1,1) title:""SECURITY AIRLOCK""
line:""OUTER"" led(outer_$screen.percent)
line:""INNER"" led(inner_$screen.percent)");

            RegisterTemplate("rover", new[] { "$screen=\"Rover Cockpit\"", "$surf=0", "$power=\"Rover Batteries\"", "$susp=\"Suspension Wheels\"", "$cargo=\"Rover Cargo\"", "$h2=\"Hydrogen Engine\"" },
@"@bind bat_$screen=group(""$power"",agg:avg)
@bind susp_$screen=group(""$susp"",agg:avg)
@bind crg_$screen=group(""$cargo"",agg:sum)
@screen $screen
output:""$screen"",$surf
layout:grid(2,2)
@panel pwr at(0,0,1,1) title:""POWER TRAIN""
line:""BATT"" bar(bat_$screen.percent)
@panel susp at(0,1,1,1) title:""CHASSIS""
line:""LOAD"" bar(susp_$screen.percent)
@panel cargo at(1,0,1,2) title:""CARGO""
line:""HOLD"" bar(crg_$screen.percent)");

            RegisterTemplate("cargo_overview", new[] { "screen", "surf=0", "crg=Cargo*", "ore=Ore*", "ingot=Ingot*" },
@"@bind crg_$screen=group(""$crg"",agg:avg)
@bind ore_$screen=group(""$ore"",agg:avg)
@bind ing_$screen=group(""$ingot"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(2,2)
@panel ores at(0,0,1,1) title:""RAW ORE SILOS""
line:""ORES"" bar(ore_$screen.percent)
@panel ingots at(1,0,1,1) title:""REFINED INGOTS""
line:""INGT"" bar(ing_$screen.percent)
@panel comps at(0,1,1,1) title:""COMPONENT STORES""
line:""COMP"" bar(crg_$screen.percent)
@panel ammo at(1,1,1,1) title:""ORDNANCE & AMMO""
line:""NATO"" bar(crg_$screen.percent)");

            RegisterTemplate("battery_monolith", new[] { "screen", "surf=0", "batt=Battery*" },
@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");
        }

        public static void RegisterTemplate(string name, string[] paramDecls, string body)
        {
            var t = new TemplateDef { Name = name };
            foreach (var p in paramDecls)
            {
                var kv = p.Split(new[] { '=' }, 2);
                string pName = NormalizeParam(kv[0]);
                t.ParamNames.Add(pName);
                if (kv.Length > 1) t.Defaults[pName] = Unquote(kv[1].Trim());
            }
            var lines = body.Replace("\r\n", "\n").Split('\n');
            foreach (var l in lines)
            {
                string s = l.Trim();
                if (s.Length > 0) t.BodyLines.Add(s);
            }
            BuiltinTemplates[name] = t;
        }

        public static void LoadTemplateFile(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return;
            var dummyErrors = new List<string>();
            var rawLines = content.Replace("\r\n", "\n").Split('\n');
            int i = 0;
            while (i < rawLines.Length)
            {
                string raw = rawLines[i++];
                string line = StripComment(raw).Trim();
                if (line.Length == 0) continue;
                if (line.StartsWith("@template"))
                {
                    try
                    {
                        var t = ParseTemplateHeader(line);
                        while (i < rawLines.Length)
                        {
                            string bodyRaw = rawLines[i++];
                            string bodyLine = StripComment(bodyRaw).Trim();
                            if (bodyLine.Equals("@end", StringComparison.OrdinalIgnoreCase))
                                break;
                            if (bodyLine.Length > 0)
                                t.BodyLines.Add(bodyLine);
                        }
                        BuiltinTemplates[t.Name] = t;
                    }
                    catch { }
                }
            }
        }

        private static string NormalizeParam(string p)
        {
            p = p.Trim();
            if (p.StartsWith("$")) p = p.Substring(1);
            return p.ToLowerInvariant();
        }

        public static ParsedConfig Parse(string customData)
        {
            var cfg = new ParsedConfig();
            ScreenDef current = null;
            PanelDef currentPanel = null;

            var preprocessed = PreprocessTemplates(customData, cfg.Errors);

            for (int i = 0; i < preprocessed.Count; i++)
            {
                string line = preprocessed[i].Trim();
                if (line.Length == 0) continue;

                try
                {
                    if (line.StartsWith("@bind"))
                    {
                        cfg.Binds.Add(ParseBind(line));
                    }
                    else if (line.StartsWith("@screen"))
                    {
                        current = new ScreenDef { Name = line.Substring("@screen".Length).Trim() };
                        cfg.Screens.Add(current);
                        currentPanel = null;
                    }
                    else if (line.StartsWith("profile:"))
                    {
                        RequireScreen(current, "profile:");
                        current.Profile = line.Substring("profile:".Length).Trim().ToLowerInvariant();
                    }
                    else if (line.StartsWith("border:"))
                    {
                        RequireScreen(current, "border:");
                        string b = line.Substring("border:".Length).Trim();
                        if (currentPanel != null) currentPanel.Border = b;
                        else current.Border = b;
                    }
                    else if (line.StartsWith("border_color:"))
                    {
                        RequireScreen(current, "border_color:");
                        string bc = line.Substring("border_color:".Length).Trim();
                        if (currentPanel != null) currentPanel.BorderColor = bc;
                        else current.BorderColor = bc;
                    }
                    else if (line.StartsWith("border_gradient:") || line.StartsWith("border_gradient("))
                    {
                        RequireScreen(current, "border_gradient");
                        string bgStr = line.StartsWith("border_gradient:") ? line.Substring("border_gradient:".Length) : line.Substring("border_gradient".Length);
                        bgStr = bgStr.Trim();
                        if (bgStr.StartsWith("(") && bgStr.Contains(")")) bgStr = ExtractParenArgs(bgStr, 0);
                        string[] stops = SplitArgs(bgStr).Select(s => s.Trim()).ToArray();
                        if (currentPanel != null) currentPanel.BorderGradientColors = stops;
                        else current.BorderGradientColors = stops;
                    }
                    else if (line.StartsWith("title_align:"))
                    {
                        RequireScreen(current, "title_align:");
                        string ta = line.Substring("title_align:".Length).Trim().ToLowerInvariant();
                        if (currentPanel != null) currentPanel.TitleAlign = ta;
                        else current.TitleAlign = ta;
                    }
                    else if (line.StartsWith("title_color:"))
                    {
                        RequireScreen(current, "title_color:");
                        string tc = line.Substring("title_color:".Length).Trim();
                        if (currentPanel != null) currentPanel.TitleColor = tc;
                        else current.TitleColor = tc;
                    }
                    else if (line.StartsWith("font_size:"))
                    {
                        RequireScreen(current, "font_size:");
                        float sz = float.Parse(line.Substring("font_size:".Length).Trim(),
                            System.Globalization.CultureInfo.InvariantCulture);
                        if (currentPanel != null) currentPanel.FontSize = sz;
                        else current.FontSize = sz;
                    }
                    else if (line.StartsWith("font:"))
                    {
                        RequireScreen(current, "font:");
                        string f = line.Substring("font:".Length).Trim();
                        if (currentPanel != null) currentPanel.Font = f;
                        else current.Font = f;
                    }
                    else if (line.StartsWith("bg:") || line.StartsWith("background:"))
                    {
                        RequireScreen(current, "bg:");
                        string bg = line.Substring(line.IndexOf(':') + 1).Trim();
                        if (currentPanel != null) currentPanel.Background = bg;
                        else current.Background = bg;
                    }
                    else if (line.StartsWith("layout:"))
                    {
                        RequireScreen(current, "layout:");
                        ParseLayout(line.Substring("layout:".Length).Trim(), current);
                    }
                    else if (line.StartsWith("refresh:"))
                    {
                        RequireScreen(current, "refresh:");
                        current.RefreshTicks = int.Parse(line.Substring("refresh:".Length).Trim());
                    }
                    else if (line.StartsWith("output:"))
                    {
                        RequireScreen(current, "output:");
                        var parts = SplitArgs(line.Substring("output:".Length).Trim());
                        if (parts.Count == 0) throw new FormatException("output: requires a target block name");
                        current.OutputBlockName = Unquote(parts[0]);
                        current.OutputSurfaceIndex = parts.Count > 1 ? int.Parse(parts[1]) : 0;
                    }
                    else if (line.StartsWith("width:"))
                    {
                        RequireScreen(current, "width:");
                        current.Width = int.Parse(line.Substring("width:".Length).Trim());
                    }
                    else if (line.StartsWith("boot_msg:") || line.StartsWith("boot:"))
                    {
                        RequireScreen(current, "boot_msg:");
                        current.BootMessage = Unquote(line.Substring(line.IndexOf(':') + 1).Trim());
                    }
                    else if (line.StartsWith("boot_stagger:"))
                    {
                        RequireScreen(current, "boot_stagger:");
                        current.BootStaggerTicks = int.Parse(line.Substring("boot_stagger:".Length).Trim());
                    }
                    else if (line.StartsWith("boot_duration:"))
                    {
                        RequireScreen(current, "boot_duration:");
                        string dStr = line.Substring("boot_duration:".Length).Trim().ToLowerInvariant();
                        if (dStr.EndsWith("s")) dStr = dStr.Substring(0, dStr.Length - 1).Trim();
                        float dur;
                        if (float.TryParse(dStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out dur))
                        {
                            current.BootDuration = dur;
                        }
                    }
                    else if (line.StartsWith("boot_log:"))
                    {
                        RequireScreen(current, "boot_log:");
                        current.BootLogs.Add(Unquote(line.Substring("boot_log:".Length).Trim()));
                    }
                    else if (line.StartsWith("boot_gradient:") || line.StartsWith("boot_gradient("))
                    {
                        RequireScreen(current, "boot_gradient");
                        string gStr = line.StartsWith("boot_gradient:") ? line.Substring("boot_gradient:".Length) : line.Substring("boot_gradient".Length);
                        gStr = gStr.Trim();
                        if (gStr.StartsWith("(") && gStr.Contains(")")) gStr = ExtractParenArgs(gStr, 0);
                        current.BootGradientColors = SplitArgs(gStr).Select(s => s.Trim()).ToArray();
                    }
                    else if (line.StartsWith("boot_color:"))
                    {
                        RequireScreen(current, "boot_color:");
                        current.BootColor = line.Substring("boot_color:".Length).Trim();
                    }
                    else if (line.StartsWith("orientation:"))
                    {
                        RequireScreen(current, "orientation:");
                        string o = line.Substring("orientation:".Length).Trim().ToLowerInvariant();
                        if (currentPanel != null) currentPanel.Orientation = o;
                    }
                    else if (line.StartsWith("@panel"))
                    {
                        RequireScreen(current, "@panel");
                        currentPanel = ParsePanel(line);
                        current.Panels.Add(currentPanel);
                    }
                    else if (line.StartsWith("title:"))
                    {
                        RequireScreen(current, "title:");
                        if (currentPanel == null)
                        {
                            currentPanel = new PanelDef { Name = "main", Col = 0, Row = 0, SpanX = current.GridCols, SpanY = current.GridRows };
                            current.Panels.Add(currentPanel);
                        }
                        currentPanel.Title = Unquote(line.Substring("title:".Length).Trim());
                    }
                    else if (line.StartsWith("line:"))
                    {
                        RequireScreen(current, "line:");
                        if (currentPanel == null)
                        {
                            currentPanel = new PanelDef { Name = "main", Col = 0, Row = 0, SpanX = current.GridCols, SpanY = current.GridRows };
                            current.Panels.Add(currentPanel);
                        }
                        currentPanel.Lines.Add(ParseLine(line.Substring("line:".Length).Trim()));
                    }
                    else
                    {
                        cfg.Errors.Add($"Line {i + 1}: unrecognized directive: \"{line}\"");
                    }
                }
                catch (Exception ex)
                {
                    cfg.Errors.Add($"Line {i + 1}: parse error ({ex.Message}) in \"{line}\"");
                }
            }

            foreach (var s in cfg.Screens)
            {
                if (string.IsNullOrEmpty(s.OutputBlockName))
                    cfg.Errors.Add($"@screen {s.Name}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
            }

            return cfg;
        }

        private static void ParseLayout(string body, ScreenDef screen)
        {
            // e.g. grid(2, 2)
            int paren = body.IndexOf('(');
            if (paren < 0) throw new FormatException("missing '(' in layout: specification");
            string kind = body.Substring(0, paren).Trim().ToLowerInvariant();
            string argsStr = ExtractParenArgs(body, paren);
            var args = SplitArgs(argsStr);

            if (kind == "grid")
            {
                if (args.Count < 2) throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
                screen.GridCols = Math.Max(1, int.Parse(args[0]));
                screen.GridRows = Math.Max(1, int.Parse(args[1]));
            }
            else
            {
                throw new FormatException($"unknown layout kind '{kind}' (expected grid)");
            }
        }

        private static PanelDef ParsePanel(string line)
        {
            // @panel name at(col, row[, spanX, spanY]) [title("Title")] [title: "Title"]
            string rest = line.Substring("@panel".Length).Trim();
            int atIdx = rest.IndexOf("at(", StringComparison.OrdinalIgnoreCase);
            if (atIdx < 0) throw new FormatException("@panel requires an at(...) positioning clause");

            string name = rest.Substring(0, atIdx).Trim();
            string atClause = rest.Substring(atIdx);
            int paren = atClause.IndexOf('(');
            string argsStr = ExtractParenArgs(atClause, paren);
            var args = SplitArgs(argsStr);
            if (args.Count < 2) throw new FormatException("at(col, row) requires at least 2 numbers");

            var panel = new PanelDef
            {
                Name = name,
                Col = int.Parse(args[0]),
                Row = int.Parse(args[1]),
                SpanX = args.Count > 2 ? int.Parse(args[2]) : 1,
                SpanY = args.Count > 3 ? int.Parse(args[3]) : 1
            };

            int afterParen = atIdx + paren + argsStr.Length + 1;
            if (afterParen < rest.Length)
            {
                string remainder = rest.Substring(afterParen).Trim();
                int titleIdx = remainder.IndexOf("title", StringComparison.OrdinalIgnoreCase);
                if (titleIdx >= 0)
                {
                    string tStr = remainder.Substring(titleIdx + 5).Trim();
                    if (tStr.StartsWith("(") && tStr.Contains(")"))
                        panel.Title = Unquote(ExtractParenArgs(tStr, 0));
                    else if (tStr.StartsWith(":"))
                    {
                        string body = tStr.Substring(1).Trim();
                        if (body.StartsWith("\""))
                        {
                            int endQ = body.IndexOf('"', 1);
                            panel.Title = endQ > 0 ? body.Substring(1, endQ - 1) : Unquote(body);
                        }
                        else
                        {
                            int sp = body.IndexOf(' ');
                            panel.Title = sp >= 0 ? body.Substring(0, sp) : body;
                        }
                    }
                }

                string oStr = ExtractAttr(remainder, "orientation:");
                if (oStr != null) panel.Orientation = oStr.ToLowerInvariant();

                string aStr = ExtractAttr(remainder, "title_align:");
                if (aStr != null) panel.TitleAlign = aStr.ToLowerInvariant();

                string tcStr = ExtractAttr(remainder, "title_color:");
                if (tcStr != null) panel.TitleColor = tcStr;

                int borderIdx = remainder.IndexOf("border:", StringComparison.OrdinalIgnoreCase);
                if (borderIdx >= 0 && !remainder.Substring(borderIdx).StartsWith("border_gradient:", StringComparison.OrdinalIgnoreCase) && !remainder.Substring(borderIdx).StartsWith("border_color:", StringComparison.OrdinalIgnoreCase))
                {
                    panel.Border = ExtractAttr(remainder.Substring(borderIdx), "border:");
                }

                int bgIdx = remainder.IndexOf("border_gradient:", StringComparison.OrdinalIgnoreCase);
                if (bgIdx < 0) bgIdx = remainder.IndexOf("border_gradient(", StringComparison.OrdinalIgnoreCase);
                if (bgIdx >= 0)
                {
                    string gStr = remainder.Substring(bgIdx + "border_gradient".Length).Trim();
                    if (gStr.StartsWith(":")) gStr = gStr.Substring(1).Trim();
                    if (gStr.StartsWith("(") && gStr.Contains(")")) gStr = ExtractParenArgs(gStr, 0);
                    panel.BorderGradientColors = SplitArgs(gStr).Select(s => s.Trim()).ToArray();
                }
            }

            return panel;
        }

        private static string ExtractAttr(string s, string key)
        {
            int idx = s.IndexOf(key, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return null;
            string val = s.Substring(idx + key.Length).Trim();
            int sp = val.IndexOf(' ');
            return sp >= 0 ? val.Substring(0, sp) : val;
        }

        private static List<string> PreprocessTemplates(string customData, List<string> errors)
        {
            var userTemplates = new Dictionary<string, TemplateDef>(StringComparer.OrdinalIgnoreCase);
            var rawLines = (customData ?? "").Replace("\r\n", "\n").Split('\n');
            var outLines = new List<string>();

            int i = 0;
            while (i < rawLines.Length)
            {
                string raw = rawLines[i++];
                string line = StripComment(raw).Trim();
                if (line.Length == 0) continue;

                if (line.StartsWith("@template"))
                {
                    try
                    {
                        var t = ParseTemplateHeader(line);
                        while (i < rawLines.Length)
                        {
                            string bodyRaw = rawLines[i++];
                            string bodyLine = StripComment(bodyRaw).Trim();
                            if (bodyLine.Equals("@end", StringComparison.OrdinalIgnoreCase))
                                break;
                            if (bodyLine.Length > 0)
                                t.BodyLines.Add(bodyLine);
                        }
                        userTemplates[t.Name] = t;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Template parse error ({ex.Message}) in \"{line}\"");
                    }
                }
                else if (line.StartsWith("@use"))
                {
                    try
                    {
                        ExpandTemplateCall(line, userTemplates, outLines, errors);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"@use error ({ex.Message}) in \"{line}\"");
                    }
                }
                else
                {
                    outLines.Add(line);
                }
            }

            return outLines;
        }

        private static TemplateDef ParseTemplateHeader(string line)
        {
            // @template name(param1, param2=val, ...)
            string rest = line.Substring("@template".Length).Trim();
            int paren = rest.IndexOf('(');
            if (paren < 0) throw new FormatException("missing '(' in @template header");
            string name = rest.Substring(0, paren).Trim();
            string argsStr = ExtractParenArgs(rest, paren);

            var t = new TemplateDef { Name = name };
            foreach (var p in SplitArgs(argsStr))
            {
                var kv = p.Split(new[] { '=' }, 2);
                string pName = NormalizeParam(kv[0]);
                t.ParamNames.Add(pName);
                if (kv.Length > 1) t.Defaults[pName] = Unquote(kv[1].Trim());
            }
            return t;
        }

        private static void ExpandTemplateCall(string line, Dictionary<string, TemplateDef> userTemplates, List<string> outLines, List<string> errors, int depth = 0)
        {
            // @use name(arg1, arg2, key=val, ...)
            string rest = line.Substring("@use".Length).Trim();
            int paren = rest.IndexOf('(');
            string name = paren >= 0 ? rest.Substring(0, paren).Trim() : rest.Trim();
            string argsStr = paren >= 0 ? ExtractParenArgs(rest, paren) : "";

            TemplateDef t = null;
            if (!userTemplates.TryGetValue(name, out t))
            {
                if (!BuiltinTemplates.TryGetValue(name, out t))
                    throw new FormatException($"unknown template '{name}'");
            }

            var bound = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in t.Defaults) bound[kv.Key] = kv.Value;

            var args = SplitArgs(argsStr);
            for (int a = 0; a < args.Count; a++)
            {
                string arg = args[a];
                int eq = arg.IndexOf('=');
                if (eq >= 0)
                {
                    string key = NormalizeParam(arg.Substring(0, eq));
                    string val = Unquote(arg.Substring(eq + 1).Trim());
                    bound[key] = val;
                }
                else
                {
                    if (a < t.ParamNames.Count)
                    {
                        string key = t.ParamNames[a];
                        bound[key] = Unquote(arg.Trim());
                    }
                }
            }

            foreach (var bodyLine in t.BodyLines)
            {
                string expanded = bodyLine;
                foreach (var kv in bound)
                {
                    expanded = expanded.Replace("$" + kv.Key, kv.Value)
                                       .Replace("{" + kv.Key + "}", kv.Value);
                }
                if (expanded.StartsWith("@use"))
                {
                    if (depth > 3) throw new FormatException("@use nested too deep");
                    ExpandTemplateCall(expanded, userTemplates, outLines, errors, depth + 1);
                }
                else outLines.Add(expanded);
            }
        }

        private static void RequireScreen(ScreenDef current, string directive)
        {
            if (current == null)
                throw new FormatException($"'{directive}' used before any @screen declared");
        }

        private static string StripComment(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            bool inQuote = false;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '"')
                {
                    inQuote = !inQuote;
                }
                else if (!inQuote)
                {
                    if (c == '/' && i + 1 < s.Length && s[i + 1] == '/')
                    {
                        return s.Substring(0, i);
                    }
                    if (c == '#')
                    {
                        if (IsHexColorAt(s, i))
                        {
                            continue;
                        }
                        return s.Substring(0, i);
                    }
                }
            }
            return s;
        }

        private static bool IsHexColorAt(string s, int index)
        {
            int len = 0;
            for (int i = index + 1; i < s.Length; i++)
            {
                char c = s[i];
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
                    len++;
                else
                    break;
            }
            if (len == 3 || len == 4 || len == 6 || len == 8)
            {
                int nextIdx = index + 1 + len;
                if (nextIdx >= s.Length || char.IsWhiteSpace(s[nextIdx]) || s[nextIdx] == ')' || s[nextIdx] == ',' || s[nextIdx] == ':')
                    return true;
            }
            return false;
        }

        // @bind name = block("Target Name")
        // @bind name = group("Target Name 1", "Target Name 2", agg: avg)
        private static BindDef ParseBind(string line)
        {
            // line looks like: @bind power = battery("Main Battery Group")
            string rest = line.Substring("@bind".Length).Trim();
            int eq = rest.IndexOf('=');
            if (eq < 0) throw new FormatException("missing '=' in @bind");

            string name = rest.Substring(0, eq).Trim();
            string expr = rest.Substring(eq + 1).Trim();

            int paren = expr.IndexOf('(');
            if (paren < 0) throw new FormatException("missing '(' in @bind target");
            string funcName = expr.Substring(0, paren).Trim().ToLowerInvariant();
            string argsStr = ExtractParenArgs(expr, paren);

            var args = SplitArgs(argsStr);
            if (args.Count == 0) throw new FormatException("@bind target has no arguments");

            var def = new BindDef { Name = name };
            if (funcName == "dynamic" || funcName == "var" || funcName == "mem")
            {
                def.Kind = BindKind.Dynamic;
                def.DynamicKey = args.Count > 0 ? Unquote(args[0].Trim()) : name;
                for (int a = 0; a < args.Count; a++)
                {
                    var kv = args[a].Split(new[] { ':' }, 2);
                    if (kv.Length == 2 && kv[0].Trim().Equals("default", StringComparison.OrdinalIgnoreCase))
                    {
                        double defVal;
                        if (double.TryParse(kv[1].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out defVal))
                            def.DefaultValue = defVal;
                    }
                }
                return def;
            }

            def.Kind = funcName == "group" ? BindKind.Group : BindKind.Block;

            string aggStr = null;
            for (int a = 0; a < args.Count; a++)
            {
                var kv = args[a].Split(new[] { ':' }, 2);
                if (kv.Length == 2 && kv[0].Trim().Equals("agg", StringComparison.OrdinalIgnoreCase))
                {
                    aggStr = kv[1].Trim().ToLowerInvariant();
                }
                else
                {
                    def.TargetNames.Add(Unquote(args[a].Trim()));
                }
            }

            if (def.TargetNames.Count == 0)
                throw new FormatException("@bind target requires at least one block or group name");

            if (def.Kind == BindKind.Group)
            {
                if (aggStr == null)
                    throw new FormatException(
                        "group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");

                switch (aggStr)
                {
                    case "avg": def.Aggregation = Agg.Avg; break;
                    case "sum": def.Aggregation = Agg.Sum; break;
                    case "min": def.Aggregation = Agg.Min; break;
                    case "max": def.Aggregation = Agg.Max; break;
                    default: throw new FormatException($"unknown agg '{aggStr}' (expected avg/sum/min/max)");
                }
            }

            return def;
        }

        // line: "PWR" bar(power.percent) gradient(green,yellow,red)
        // line: "O2"  bar(o2.percent) flat(cyan)
        // line: "SHLD" chevron(shield.percent) flat(cyan)
        // line: "WARN" hazard(temp.percent) flat(yellow)
        // line: "CARGO" text(cargo.percent) color(cargo.percent > 90 : red, default : white)
        private static LineDef ParseLine(string body)
        {
            var def = new LineDef();

            // 1) quoted label
            if (!body.StartsWith("\"")) throw new FormatException("line must start with a quoted label");
            int endQuote = body.IndexOf('"', 1);
            if (endQuote < 0) throw new FormatException("unterminated quoted label");
            def.Label = body.Substring(1, endQuote - 1);
            string remainder = body.Substring(endQuote + 1).Trim();

            // 2) the display function: bar, led, spark, hazard, chevron, dots, text
            int paren = remainder.IndexOf('(');
            if (paren < 0) throw new FormatException("missing display function, e.g. bar(...)/text(...)");
            string func = remainder.Substring(0, paren).Trim().ToLowerInvariant();
            string argsInner = ExtractParenArgs(remainder, paren);
            string afterFunc = remainder.Substring(remainder.IndexOf(')', paren) + 1).Trim();

            var dotParts = argsInner.Trim().Split('.');
            if (dotParts.Length != 2)
                throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
            def.BindName = dotParts[0].Trim();
            def.Field = dotParts[1].Trim();

            if (func == "bar") def.Kind = LineKind.Bar;
            else if (func == "led") def.Kind = LineKind.LedBar;
            else if (func == "spark" || func == "sparkline") def.Kind = LineKind.Spark;
            else if (func == "hazard" || func == "stripes") def.Kind = LineKind.Hazard;
            else if (func == "chevron" || func == "chevrons") def.Kind = LineKind.Chevron;
            else if (func == "dots" || func == "dot") def.Kind = LineKind.Dots;
            else if (func == "balance" || func == "horizon" || func == "centered") def.Kind = LineKind.Balance;
            else if (func == "toggle" || func == "switch") def.Kind = LineKind.Toggle;
            else if (func == "checkbox" || func == "check") def.Kind = LineKind.Checkbox;
            else if (func == "status" || func == "badge" || func == "state") def.Kind = LineKind.Status;
            else if (func == "ammo" || func == "pips") def.Kind = LineKind.Ammo;
            else if (func == "flow" || func == "rate" || func == "spinner") def.Kind = LineKind.Flow;
            else if (func == "text") def.Kind = LineKind.Text;
            else throw new FormatException($"unknown display function '{func}'");

            // 3) chained modifiers: gradient(...) / flat(...) / color(...) / pattern(...) / dir / unit / format
            while (afterFunc.Length > 0)
            {
                int dirIdx = afterFunc.IndexOf("dir:", StringComparison.OrdinalIgnoreCase);
                int firstParen = afterFunc.IndexOf('(');
                if (dirIdx >= 0 && (firstParen < 0 || dirIdx < firstParen))
                {
                    string d = afterFunc.Substring(dirIdx + 4).Trim();
                    int sp = d.IndexOf(' ');
                    def.Direction = (sp >= 0 ? d.Substring(0, sp) : d).ToLowerInvariant();
                    afterFunc = (sp >= 0 ? d.Substring(sp) : "").Trim();
                    continue;
                }

                int unitIdx = afterFunc.IndexOf("unit:", StringComparison.OrdinalIgnoreCase);
                if (unitIdx >= 0 && (firstParen < 0 || unitIdx < firstParen))
                {
                    string u = afterFunc.Substring(unitIdx + 5).Trim();
                    int sp = u.IndexOf(' ');
                    def.Unit = (sp >= 0 ? u.Substring(0, sp) : u).Trim().Trim('"');
                    afterFunc = (sp >= 0 ? u.Substring(sp) : "").Trim();
                    continue;
                }

                int fmtIdx = afterFunc.IndexOf("format:", StringComparison.OrdinalIgnoreCase);
                if (fmtIdx >= 0 && (firstParen < 0 || fmtIdx < firstParen))
                {
                    string f = afterFunc.Substring(fmtIdx + 7).Trim();
                    int sp = f.IndexOf(' ');
                    def.Format = (sp >= 0 ? f.Substring(0, sp) : f).Trim().Trim('"');
                    afterFunc = (sp >= 0 ? f.Substring(sp) : "").Trim();
                    continue;
                }

                int mParen = afterFunc.IndexOf('(');
                if (mParen < 0) break;
                string mFunc = afterFunc.Substring(0, mParen).Trim().ToLowerInvariant();
                int lastSpace = mFunc.LastIndexOf(' ');
                if (lastSpace >= 0) mFunc = mFunc.Substring(lastSpace + 1).Trim();
                string mArgs = ExtractParenArgs(afterFunc, mParen);
                afterFunc = afterFunc.Substring(afterFunc.IndexOf(')', mParen) + 1).Trim();

                if (mFunc == "gradient")
                {
                    def.Gradient = true;
                    def.GradientColors = SplitArgs(mArgs).Select(s => s.Trim()).ToArray();
                    if (def.GradientColors.Length < 2)
                        throw new FormatException("gradient() needs at least 2 colors");
                }
                else if (mFunc == "flat")
                {
                    def.FlatColor = mArgs.Trim();
                }
                else if (mFunc == "pattern")
                {
                    string pat = mArgs.Trim().ToLowerInvariant();
                    if (pat == "hazard" || pat == "stripes") def.Kind = LineKind.Hazard;
                    else if (pat == "chevron" || pat == "chevrons") def.Kind = LineKind.Chevron;
                    else if (pat == "dots" || pat == "dot") def.Kind = LineKind.Dots;
                    else if (pat == "led") def.Kind = LineKind.LedBar;
                    else if (pat == "balance" || pat == "horizon") def.Kind = LineKind.Balance;
                    else if (pat == "toggle" || pat == "switch") def.Kind = LineKind.Toggle;
                    else if (pat == "checkbox" || pat == "check") def.Kind = LineKind.Checkbox;
                    else if (pat == "status" || pat == "badge" || pat == "state") def.Kind = LineKind.Status;
                    else if (pat == "ammo" || pat == "pips") def.Kind = LineKind.Ammo;
                    else if (pat == "flow" || pat == "rate" || pat == "spinner") def.Kind = LineKind.Flow;
                }
                else if (mFunc == "color")
                {
                    def.ColorRules = new List<ColorRule>();
                    foreach (var part in SplitArgs(mArgs))
                    {
                        int colon = part.LastIndexOf(':');
                        if (colon < 0) throw new FormatException("color() entries need 'expr : color'");
                        string left = part.Substring(0, colon).Trim();
                        string color = part.Substring(colon + 1).Trim();
                        if (left.Equals("default", StringComparison.OrdinalIgnoreCase))
                        {
                            def.ColorRules.Add(new ColorRule { IsDefault = true, Color = color });
                        }
                        else
                        {
                            string strippedExpr = StripLeadingFieldRef(left);
                            def.ColorRules.Add(new ColorRule { Expr = strippedExpr, Color = color });
                        }
                    }
                    if (!def.ColorRules.Any(r => r.IsDefault))
                        throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
                }
                else if (mFunc == "dir" || mFunc == "direction")
                {
                    def.Direction = mArgs.Trim().ToLowerInvariant();
                }
                else if (mFunc == "unit")
                {
                    def.Unit = mArgs.Trim().Trim('"');
                }
                else if (mFunc == "format" || mFunc == "fmt")
                {
                    def.Format = mArgs.Trim().Trim('"');
                }
                else
                {
                    throw new FormatException($"unknown modifier '{mFunc}'");
                }
            }

            return def;
        }

        // ---------- small string helpers ----------

        private static string ExtractParenArgs(string s, int openParenIndex)
        {
            int depth = 0;
            for (int i = openParenIndex; i < s.Length; i++)
            {
                if (s[i] == '(') depth++;
                else if (s[i] == ')')
                {
                    depth--;
                    if (depth == 0) return s.Substring(openParenIndex + 1, i - openParenIndex - 1);
                }
            }
            throw new FormatException("unbalanced parentheses");
        }

        private static List<string> SplitArgs(string argsStr)
        {
            // splits on top-level commas only (none of our arg values contain
            // nested parens in v1, so a simple split is sufficient and honest
            // about not supporting more than that yet)
            return argsStr.Split(',').Select(s => s.Trim()).Where(s => s.Length > 0).ToList();
        }

        private static string StripLeadingFieldRef(string expr)
        {
            expr = expr.Trim();
            foreach (var op in new[] { ">=", "<=", "==", ">", "<" })
            {
                int idx = expr.IndexOf(op, StringComparison.Ordinal);
                if (idx > 0) return expr.Substring(idx).Trim();
                if (idx == 0) return expr; // already just "> 90" with no prefix
            }
            throw new FormatException($"color() rule has no recognizable comparison operator: '{expr}'");
        }

        private static string Unquote(string s)
        {
            if (s.Length >= 2 && s[0] == '"' && s[s.Length - 1] == '"')
                return s.Substring(1, s.Length - 2);
            return s;
        }
    }

    // ---------- Aggregation ----------

    public static class Aggregator
    {
        public static double Aggregate(Agg agg, List<double> values)
        {
            if (values == null || values.Count == 0)
                throw new InvalidOperationException("cannot aggregate an empty value set");

            switch (agg)
            {
                case Agg.Avg: return values.Average();
                case Agg.Sum: return values.Sum();
                case Agg.Min: return values.Min();
                case Agg.Max: return values.Max();
                default: throw new InvalidOperationException("Aggregate() called with Agg.None");
            }
        }
    }

    // ---------- Layout / rendering (still pure — produces colored text lines,
    // not sprites. The SE-side adapter turns these into MySprite TEXT objects.) ----------

    public class RenderedLine
    {
        public string Text;
        public string Color; // color name/string as given in the DSL; SE-side maps to a real Color
    }

    public static class Layout
    {
        // Renders a single bar line as fixed-width ASCII: "LABEL [#####-----] NN%" or with custom suffix
        public static string RenderBarText(string label, double percent, int barWidth = 10, string valueSuffix = null)
        {
            percent = Math.Max(0, Math.Min(100, percent));
            int filled = (int)Math.Round(barWidth * (percent / 100.0));
            string bar = new string('#', filled) + new string('-', barWidth - filled);
            string valStr = valueSuffix != null ? valueSuffix : $"{percent:0}%";
            return $"{label} [{bar}] {valStr}";
        }

        // Formats telemetry values with unit prefixes or custom format strings
        public static string FormatDisplayValue(double value, string unit = null, string format = null)
        {
            if (unit == "auto")
            {
                double abs = Math.Abs(value);
                if (abs >= 1e9) return (value / 1e9).ToString("F1") + "G";
                if (abs >= 1e6) return (value / 1e6).ToString("F1") + "M";
                if (abs >= 1e3) return (value / 1e3).ToString("F1") + "k";
                string f = !string.IsNullOrEmpty(format) ? format : "F0";
                return value.ToString(f);
            }

            string fmt = !string.IsNullOrEmpty(format) ? format : (Math.Abs(value) >= 100.0 ? "F0" : "F1");
            if (string.IsNullOrEmpty(unit))
            {
                return value.ToString(fmt);
            }
            return value.ToString(fmt) + unit;
        }

        // Computes responsive font scaling according to viewport profile and screen size
        public static float GetProfileFontSize(string profile, float surfaceWidth, float surfaceHeight, float defaultFontSize = 1.0f)
        {
            string p = (profile ?? "standard").ToLowerInvariant();
            if (p == "compact" || (p == "auto" && surfaceWidth <= 300 && surfaceHeight <= 300 && surfaceHeight > 160))
            {
                return defaultFontSize * 0.75f;
            }
            if (p == "corner" || (p == "auto" && surfaceHeight <= 160))
            {
                return defaultFontSize * 0.55f;
            }
            if (p == "wide" || (p == "auto" && surfaceWidth >= 1000))
            {
                return defaultFontSize * 1.15f;
            }
            return defaultFontSize;
        }

        // Renders a zero-centered balance/horizon chart as fixed-width ASCII: "LABEL [---<|>] NN%"
        public static string RenderBalanceText(string label, double percent, int halfWidth = 5)
        {
            percent = Math.Max(0, Math.Min(100, percent));
            double dev = (percent - 50.0) / 50.0;
            int leftChars = 0;
            int rightChars = 0;
            if (dev < -0.01) leftChars = (int)Math.Round(-dev * halfWidth);
            else if (dev > 0.01) rightChars = (int)Math.Round(dev * halfWidth);

            if (leftChars > halfWidth) leftChars = halfWidth;
            if (rightChars > halfWidth) rightChars = halfWidth;

            string leftPart = new string('-', halfWidth - leftChars) + new string('<', leftChars);
            string rightPart = new string('>', rightChars) + new string('-', halfWidth - rightChars);
            return $"{label} [{leftPart}|{rightPart}] {percent:0}%";
        }

        public static string RenderToggleText(string label, double value)
        {
            return value >= 50.0 ? $"{label} [====O] ON" : $"{label} [O====] OFF";
        }

        public static string RenderCheckboxText(string label, double value)
        {
            return value >= 50.0 ? $"{label} [X] ACTIVE" : $"{label} [ ] READY ";
        }

        public static string RenderStatusText(string label, double value)
        {
            if (value >= 75.0) return $"{label} [ ONLINE  ]";
            if (value >= 25.0) return $"{label} [ STANDBY ]";
            return $"{label} [ OFFLINE ]";
        }

        public static string RenderAmmoText(string label, double percent, int maxPips = 8)
        {
            percent = Math.Max(0, Math.Min(100, percent));
            int filled = (int)Math.Round(maxPips * (percent / 100.0));
            string pips = new string('|', filled) + new string('.', maxPips - filled);
            return $"{label} [{pips}] {percent:0}%";
        }

        public static string RenderFlowText(string label, double percent, int tick = 0)
        {
            char[] spin = new[] { '|', '/', '-', '\\' };
            char s = spin[(tick >= 0 ? tick : 0) % 4];
            if (percent > 55.0) return $"{label} {s} ▲ CHG {percent:0}%";
            if (percent < 45.0) return $"{label} {s} ▼ DSC {percent:0}%";
            return $"{label} ● STBY {percent:0}%";
        }

        // Fast zero-allocation wildcard matcher supporting '*' and '?'
        public static bool MatchWildcard(string pattern, string input)
        {
            if (pattern == null || input == null) return false;
            if (pattern == "*" || pattern == input) return true;
            if (!pattern.Contains("*") && !pattern.Contains("?"))
                return string.Equals(pattern, input, StringComparison.OrdinalIgnoreCase);

            int p = 0, i = 0, starIdx = -1, match = 0;
            while (i < input.Length)
            {
                if (p < pattern.Length && (pattern[p] == '?' || char.ToLowerInvariant(pattern[p]) == char.ToLowerInvariant(input[i])))
                {
                    p++;
                    i++;
                }
                else if (p < pattern.Length && pattern[p] == '*')
                {
                    starIdx = p;
                    match = i;
                    p++;
                }
                else if (starIdx != -1)
                {
                    p = starIdx + 1;
                    match++;
                    i = match;
                }
                else
                {
                    return false;
                }
            }
            while (p < pattern.Length && pattern[p] == '*') p++;
            return p == pattern.Length;
        }

        // Picks a color along a multi-stop gradient by percent (0-100).
        // Nearest-stop selection (instant branch lookup)
        public static string GradientColorAt(string[] stops, double percent)
        {
            if (stops == null || stops.Length == 0) return "white";
            percent = Math.Max(0, Math.Min(100, percent));
            int idx = (int)Math.Floor(percent / 100.0 * stops.Length);
            if (idx >= stops.Length) idx = stops.Length - 1;
            return stops[idx];
        }

        // Continuous smooth RGB color interpolation across multi-stop gradients
        public static HelmColor GradientLerp(string[] stops, double percent)
        {
            if (stops == null || stops.Length == 0) return HelmColor.White;
            if (stops.Length == 1) return HelmColor.Parse(stops[0]);

            double ratio = Math.Max(0.0, Math.Min(1.0, percent / 100.0));
            double scaled = ratio * (stops.Length - 1);
            int idx1 = (int)Math.Floor(scaled);
            int idx2 = Math.Min(stops.Length - 1, idx1 + 1);
            double subRatio = scaled - idx1;

            HelmColor c1 = HelmColor.Parse(stops[idx1]);
            HelmColor c2 = HelmColor.Parse(stops[idx2]);

            byte r = (byte)Math.Round(c1.R + (c2.R - c1.R) * subRatio);
            byte g = (byte)Math.Round(c1.G + (c2.G - c1.G) * subRatio);
            byte b = (byte)Math.Round(c1.B + (c2.B - c1.B) * subRatio);

            return new HelmColor(r, g, b);
        }

        public static string ResolveConditionalColor(List<ColorRule> rules, double value)
        {
            foreach (var rule in rules)
            {
                if (rule.IsDefault) continue;
                if (EvalSimpleComparison(rule.Expr, value)) return rule.Color;
            }
            var def = rules.FirstOrDefault(r => r.IsDefault);
            return def != null ? def.Color : null;
        }

        // Supports the small comparison grammar the DSL actually uses: "> N", "< N", ">= N", "<= N", "== N"
        private static bool EvalSimpleComparison(string expr, double value)
        {
            expr = expr.Trim();
            foreach (var op in new[] { ">=", "<=", "==", ">", "<" })
            {
                if (expr.StartsWith(op))
                {
                    double rhs = double.Parse(expr.Substring(op.Length).Trim(),
                        System.Globalization.CultureInfo.InvariantCulture);
                    switch (op)
                    {
                        case ">=": return value >= rhs;
                        case "<=": return value <= rhs;
                        case "==": return Math.Abs(value - rhs) < 1e-9;
                        case ">": return value > rhs;
                        case "<": return value < rhs;
                    }
                }
            }
            throw new FormatException($"unsupported comparison expression: '{expr}'");
        }

        // Bordered ASCII window: content lines padded to `width`, wrapped in a
        // simple ASCII border. PDR §7: ASCII-only baseline until FontData.xml
        // is checked for Unicode box-drawing support — deliberately not using
        // any box-drawing glyphs here.
        public static List<string> RenderBorderedWindow(List<string> contentLines, int width)
        {
            var outLines = new List<string>();
            outLines.Add("+" + new string('-', width + 2) + "+");
            foreach (var line in contentLines)
            {
                string trimmed = line.Length > width ? line.Substring(0, width) : line;
                string padded = trimmed.PadRight(width);
                outLines.Add("| " + padded + " |");
            }
            outLines.Add("+" + new string('-', width + 2) + "+");
            return outLines;
        }

        public struct PanelRect
        {
            public float X, Y, Width, Height;
        }

        public static PanelRect ComputePanelRect(
            int col, int row, int spanX, int spanY,
            int totalCols, int totalRows,
            float surfaceW, float surfaceH,
            float padX, float padY, float gutterX, float gutterY)
        {
            totalCols = Math.Max(1, totalCols);
            totalRows = Math.Max(1, totalRows);
            spanX = Math.Max(1, Math.Min(totalCols - col, spanX));
            spanY = Math.Max(1, Math.Min(totalRows - row, spanY));

            float availW = surfaceW - (padX * 2f);
            float availH = surfaceH - (padY * 2f);

            float cellW = (availW - (gutterX * (totalCols - 1))) / totalCols;
            float cellH = (availH - (gutterY * (totalRows - 1))) / totalRows;

            float px = padX + col * (cellW + gutterX);
            float py = padY + row * (cellH + gutterY);
            float pw = spanX * cellW + ((spanX - 1) * gutterX);
            float ph = spanY * cellH + ((spanY - 1) * gutterY);

            return new PanelRect { X = px, Y = py, Width = pw, Height = ph };
        }

        public struct ScreenStats
        {
            public int PanelCount;
            public int LineCount;
            public int TotalAllocatedSprites;
            public int MutatedPerTick;
            public int StaticTrackSprites;
            public int DynamicFillSprites;
            public int TextLabelSprites;
            public int TextValueSprites;
            public int EstInstructionCountNormal;
            public int EstInstructionCountRedraw;
            public double EstimatedGcAllocBytes;
        }

        public static ScreenStats ComputeStats(ScreenDef screen)
        {
            var stats = new ScreenStats();
            var panels = screen.Panels != null && screen.Panels.Count > 0
                ? screen.Panels
                : new List<PanelDef> { new PanelDef { Lines = screen.Lines } };

            stats.PanelCount = panels.Count;
            stats.LineCount = 0;
            stats.TotalAllocatedSprites = 0;
            stats.MutatedPerTick = 0;

            if (!string.IsNullOrEmpty(screen.Background) && !screen.Background.Equals("none", StringComparison.OrdinalIgnoreCase))
                stats.TotalAllocatedSprites++;

            if (!string.IsNullOrEmpty(screen.Border) && !screen.Border.Equals("none", StringComparison.OrdinalIgnoreCase))
                stats.TotalAllocatedSprites += 2; // top and bottom accent bars

            foreach (var p in panels)
            {
                if (!string.IsNullOrEmpty(p.Background) && !p.Background.Equals("none", StringComparison.OrdinalIgnoreCase))
                    stats.TotalAllocatedSprites++;

                if (!string.IsNullOrEmpty(p.Title))
                    stats.TotalAllocatedSprites += 2; // title text + underline quad

                stats.LineCount += p.Lines.Count;
                foreach (var l in p.Lines)
                {
                    stats.TextLabelSprites++;
                    stats.TextValueSprites++;
                    stats.TotalAllocatedSprites += 2; // label text + value text

                    switch (l.Kind)
                    {
                        case LineKind.Bar:
                            stats.StaticTrackSprites++;
                            stats.DynamicFillSprites++;
                            stats.TotalAllocatedSprites += 2; // track + fill
                            stats.MutatedPerTick++;
                            break;
                        case LineKind.LedBar:
                            stats.DynamicFillSprites += 10;
                            stats.TotalAllocatedSprites += 10;
                            stats.MutatedPerTick += 10;
                            break;
                        case LineKind.Hazard:
                            stats.DynamicFillSprites += 10;
                            stats.TotalAllocatedSprites += 11;
                            stats.MutatedPerTick += 10;
                            break;
                        case LineKind.Chevron:
                            stats.DynamicFillSprites += 10;
                            stats.TotalAllocatedSprites += 10;
                            stats.MutatedPerTick += 10;
                            break;
                        case LineKind.Dots:
                            stats.DynamicFillSprites += 12;
                            stats.TotalAllocatedSprites += 12;
                            stats.MutatedPerTick += 12;
                            break;
                        case LineKind.Spark:
                            stats.StaticTrackSprites++;
                            stats.DynamicFillSprites += 8;
                            stats.TotalAllocatedSprites += 9;
                            stats.MutatedPerTick += 8;
                            break;
                        case LineKind.Balance:
                            stats.StaticTrackSprites += 2; // track + center divider
                            stats.DynamicFillSprites++;
                            stats.TotalAllocatedSprites += 3;
                            stats.MutatedPerTick++;
                            break;
                        case LineKind.Toggle:
                        case LineKind.Checkbox:
                            stats.StaticTrackSprites++;
                            stats.DynamicFillSprites++;
                            stats.TotalAllocatedSprites += 2;
                            stats.MutatedPerTick++;
                            break;
                        case LineKind.Status:
                        case LineKind.Flow:
                            stats.StaticTrackSprites++;
                            stats.DynamicFillSprites += 2;
                            stats.TotalAllocatedSprites += 3;
                            stats.MutatedPerTick += 2;
                            break;
                        case LineKind.Ammo:
                            stats.DynamicFillSprites += 8;
                            stats.TotalAllocatedSprites += 8;
                            stats.MutatedPerTick += 8;
                            break;
                        default:
                            stats.MutatedPerTick++;
                            break;
                    }
                }
            }

            // Normal unchanged tick: ~25-45 instructions (< 0.1% of PB limit)
            stats.EstInstructionCountNormal = 25 + (stats.LineCount * 2);
            // Full redraw / active update: ~200-400 instructions
            stats.EstInstructionCountRedraw = 180 + (stats.MutatedPerTick * 4);
            stats.EstimatedGcAllocBytes = 0.0; // Zero-allocation retained buffer model

            return stats;
        }
    }
}