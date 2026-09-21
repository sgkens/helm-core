// Program.cs — SE-specific adapter for HELM.
//
// Status: this file compiles against the stand-in API in tools/spritedump and its sprite output is
// exported for the docs. In-game run 1 (issues/in-game-run-1-errors.md) found that the script did not
// compile in the game; the causes are fixed and checked by tools/spritedump/CombinedCheck. It has not
// yet run successfully in Space Engineers: instruction counts, allocation behaviour and real font
// metrics are unmeasured. HelmCore.cs has the unit tests (Tests.cs); Program.cs has none.
//
// The Programmable Block compiles the pasted text as the BODY of "partial class Program : MyGridProgram",
// with the game's usings, as C# 6. Build-Release.ps1 therefore writes HelmCombined.min.cs as the body of
// this class with the HelmCore.cs types nested inside it. Keep both source files to C# 6: no "out var",
// pattern matching ("x is T y"), tuples, local functions or throw expressions. System.Tuple and other
// APIs the game prohibits are banned in tools/spritedump/CombinedCheck/BannedSymbols.txt (CI).
// tools/pb-check compiles the script against the game's own assemblies and the MDK prohibited-API
// analyzer (set SE_BIN64 to the game's Bin64 folder; Build-Release.ps1 then runs it). Run it before
// pasting into the game: the stand-in API used in CI cannot tell whether a block member really exists.
//
// The shipped HelmCombined.min.cs also has its names shortened by tools/minify-rename. Game error messages and stack
// traces show the short names; HelmCombined.map.txt (written by Build-Release.ps1) maps them back. Build-Release.ps1
// -NoRename builds without it. tools/rename-check proves the two behave the same.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;
using HelmCore;

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
        private List<IMyTerminalBlock> _rawBlocks = new List<IMyTerminalBlock>();
        private bool _allLoaded;
        private int _foreignBlocks;
        private string _cfgErr = "";
        private StringBuilder _outErr = new StringBuilder();

        // Dynamic Micro-API: In-memory variables for IGC broadcast and CLI injection
        private readonly Dictionary<string, double> _dynamicValues
            = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _dynText
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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
            public int BgIndex = -1;        // first background sprite (one, or one per gradient band)
            public Color[] BgColors;        // their normal colours
            public Color AlertCol;          // colour they flash to while the red alert is on
            public bool AlertShown;
            public Vector2 LastSurfaceSize;
            public bool Initialized = false;
            public List<BgEntry> Bgs = new List<BgEntry>(); // backgrounds that change with a value or flash
        }

        // A background sprite whose colour follows a value (bg: color(...)) or flashes on a value (flash:).
        private class BgEntry
        {
            public int Index;
            public Color Base, Cur;
            public List<ColorRule> Rules, Flash;
            public string Bind, Field, FlashBind, FlashField;
            public bool IsScreen, Force;
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
            public string LastText;
            public float DisplayValue = -1f; // fluid damping lerp
        }

        private Dictionary<ScreenDef, ScreenBuffer> _screenBuffers
            = new Dictionary<ScreenDef, ScreenBuffer>();

        private int _screenCursor = 0;
        private double _rescanSec = 0;
        private double _time = 0;        // seconds since the script started (Runtime.TimeSinceLastRun)
        private int _idleRuns = 0;       // consecutive runs in which nothing was redrawn
        private Dictionary<ScreenDef, double> _bootT0 = new Dictionary<ScreenDef, double>();
        private Dictionary<ScreenDef, double> _drawAt = new Dictionary<ScreenDef, double>(); // refresh: earliest next draw
        private int _tickCounter = 0;
        private const string HELM_VERSION = "0.0.0"; // stamped from the VERSION file by Build-Release.ps1
        private IMyTextSurface _pbSurface;             // the Programmable Block's own screen (console)
        private bool _consoleOn = true;
        private double _consoleAt = 0;
        private string _consoleText = "";
        private string _lastScreen = "";
        private int _instrLast = 0, _instrPeak = 0; // instructions of the last completed run, and the highest so far
        private List<KeyValuePair<string, int>> _con = new List<KeyValuePair<string, int>>();
        private const double RESCAN_SECONDS = 120; // PDR §5: rescan blocks periodically, not per tick

        // Approximate line height in pixels per unit FontSize.
        // *** UNVERIFIED — a placeholder, not a researched constant. ***
        // The real value should come from MeasureStringInPixels() against an
        // actual surface in-game, per PDR §7's "file/API check tells you
        // presence, not exact rendered metrics" caution. Do not trust this
        // number until it's been checked on a real screen.
        private const float APPROX_LINE_HEIGHT_PER_FONTSIZE = 22f;

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10; // fast while booting; Main() drops to Update100 when idle
            try
            {
                _helmBroadcastListener = IGC.RegisterBroadcastListener("HELM");
                _helmBroadcastListener.SetMessageCallback("HELM_IGC");
            }
            catch {}
            ReloadConfig();
            StartBoot();
            InitConsole();
        }

        public void Main(string argument, UpdateType updateType)
        {
            double dt = Runtime.TimeSinceLastRun.TotalSeconds;
            _time += dt;
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
            else if (arg == "boot")
            {
                StartBoot();
            }
            else if (arg == "console on" || arg == "console off")
            {
                _consoleOn = arg == "console on";
                InitConsole();
                _consoleAt = 0;
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
            _rescanSec += dt;
            if (_rescanSec >= RESCAN_SECONDS)
            {
                _rescanSec = 0;
                ResolveBindsAndOutputs();
            }

            if (_config == null || _config.Screens.Count == 0)
            {
                UpdateConsole();
                EchoNotes();
                Echo("HELM: no valid @screen configured. Check Custom Data.");
                return;
            }

            // Round-robin: exactly one screen's full redraw per Main() call.
            // This is the FSD2-failure fix (PDR §1/§4) — never render every
            // screen in one pass.
            var screen = _config.Screens[_screenCursor % _config.Screens.Count];
            _screenCursor++;
            _lastScreen = screen.Name;

            // Screens that are booting draw their boot frame every run (cheap); the others take turns.
            bool active = _bootT0.Count > 0;
            if (active) RenderBoots();
            double due;
            if (!_bootT0.ContainsKey(screen) && !(_drawAt.TryGetValue(screen, out due) && _time < due))
            {
                if (RenderScreen(screen)) active = true;
                if (screen.RefreshTicks > 0) _drawAt[screen] = _time + screen.RefreshTicks / 60.0;
            }

            // Fast (Update10) while anything is booting, changing, easing or alerting; slow (Update100) once
            // every screen has been idle for a few runs. Refresh rates are to be revisited after more in-game
            // and stress testing (DEVTRACKER TASK-32.7).
            _idleRuns = active ? 0 : _idleRuns + 1;
            Runtime.UpdateFrequency = _idleRuns <= _config.Screens.Count + 1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;

            UpdateConsole();
            EchoNotes();

            _instrLast = Runtime.CurrentInstructionCount;
            if (_instrLast > _instrPeak) _instrPeak = _instrLast;
        }

        // Config errors and output-block problems are kept and echoed on every run: Echo text is replaced each run.
        private void EchoNotes()
        {
            if (_cfgErr.Length > 0) Echo(_cfgErr);
            if (_outErr.Length > 0) Echo(_outErr.ToString());
        }

        // Every block HELM may use: this construct only (same grid, or subgrids on rotors, pistons and hinges).
        // GetBlocks also returns blocks of ships docked on a connector; those are dropped and counted.
        // all = true also returns blocks of ships docked on a connector (a bind with scope: all).
        private List<IMyTerminalBlock> AllBlocks(bool all = false)
        {
            if (!_allLoaded)
            {
                _rawBlocks.Clear();
                GridTerminalSystem.GetBlocks(_rawBlocks);
                _allBlocks.Clear();
                _allBlocks.AddRange(_rawBlocks);
                _allBlocks.RemoveAll(b => !b.IsSameConstructAs(Me));
                _foreignBlocks = _rawBlocks.Count - _allBlocks.Count;
                _allLoaded = true;
            }
            return all ? _rawBlocks : _allBlocks;
        }

        // The first block whose name contains 'name', on this construct. GetBlockWithName returns the first match
        // on any connected ship, so a docked ship's block can win; search this construct only when it does.
        private IMyTerminalBlock FindBlock(string name, bool anyShip = false)
        {
            var b = GridTerminalSystem.GetBlockWithName(name);
            if (b == null || anyShip || b.IsSameConstructAs(Me)) return b;
            var all = AllBlocks();
            for (int i = 0; i < all.Count; i++)
                if (all[i].CustomName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0) return all[i];
            return null;
        }

        // Blocks of the first group named 'name' that has any on this construct, added to 'into'.
        private void CollectGroup(string name, List<IMyTerminalBlock> into, bool all = false)
        {
            var g = GridTerminalSystem.GetBlockGroupWithName(name);
            if (g == null) return;
            int before = into.Count;
            var tmp = new List<IMyTerminalBlock>();
            g.GetBlocks(tmp);
            AddMine(tmp, into, all);
            if (into.Count > before) return;
            var groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            foreach (var x in groups)
            {
                if (x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) < 0) continue;
                tmp.Clear();
                x.GetBlocks(tmp);
                AddMine(tmp, into, all);
                if (into.Count > before) return;
            }
        }

        private void AddMine(List<IMyTerminalBlock> from, List<IMyTerminalBlock> into, bool all = false)
        {
            foreach (var b in from)
                if ((all || b.IsSameConstructAs(Me)) && !into.Contains(b)) into.Add(b);
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
            _bootT0.Clear();
            _drawAt.Clear();
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
                        _dynText.Remove(key);
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
                        else
                        {
                            _dynText[key] = valStr;
                        }
                    }
                }
            }
        }

        // ---------------- console ----------------
        // One model, two views: the info pane in the terminal (Echo, plain text) and the Programmable Block's own
        // screen (grays and magenta). It is rebuilt once a second; Echo repeats the cached text every run because
        // the game clears it each run.

        private void ConLine(int style, string text)
        {
            _con.Add(new KeyValuePair<string, int>(text, style));
        }

        private static string Trunc(string s, int n)
        {
            return s.Length > n ? s.Substring(0, n) : s;
        }

        private void InitConsole()
        {
            var prov = Me as IMyTextSurfaceProvider;
            if (prov == null || prov.SurfaceCount < 1) return;
            _pbSurface = prov.GetSurface(0);
            _pbSurface.ContentType = _consoleOn ? ContentType.SCRIPT : ContentType.NONE;
            _pbSurface.Script = "";
        }

        private void UpdateConsole()
        {
            if (_time >= _consoleAt)
            {
                // Once a second while active; every 5 s when idle so the idle run stays cheap.
                _consoleAt = _time + (_idleRuns > _config.Screens.Count + 1 ? 5.0 : 1.0);
                BuildConsole();
                if (_consoleOn && _pbSurface != null) DrawConsole();
            }
            Echo(_consoleText);
        }

        // Styles: 0 header, 1 section, 2 body, 3 warning, 4 rule.
        private void BuildConsole()
        {
            _con.Clear();
            int bound = 0, blockCount = 0;
            foreach (var b in _config.Binds)
            {
                List<IMyTerminalBlock> l;
                if (b.Kind == BindKind.Dynamic) { bound++; continue; }
                if (_resolvedBinds.TryGetValue(b.Name, out l)) { blockCount += l.Count; if (l.Count > 0) bound++; }
            }
            int errs = _config.Errors.Count;
            ConLine(0, "HELM-[Kernel-(v" + HELM_VERSION + ")]");
            ConLine(4, "------------------------------");
            ConLine(1, "krn-cfg");
            ConLine(2, " screens " + _config.Screens.Count + "  binds " + bound + "/" + _config.Binds.Count + "  errors " + errs);
            ConLine(2, " alert " + (_globalRedAlert ? "RED" : "off") + "  blocks " + blockCount + "  docked-ignored " + _foreignBlocks);
            if (errs > 0) ConLine(3, " ! " + Trunc(_config.Errors[0], 40));
            ConLine(1, "cfg-dsp");
            int shown = 0;
            foreach (var s in _config.Screens)
            {
                if (++shown > 10) { ConLine(2, " +" + (_config.Screens.Count - 10) + " more"); break; }
                IMyTextSurface sf;
                string size = "--", state = "no block";
                if (_resolvedOutputs.TryGetValue(s, out sf))
                {
                    size = sf.SurfaceSize.X.ToString("0") + "x" + sf.SurfaceSize.Y.ToString("0");
                    state = _bootT0.ContainsKey(s) ? "boot" : "live";
                }
                ConLine(state == "no block" ? 3 : 2, " " + Trunc(s.Name, 14).PadRight(14) + " " + size.PadRight(9) + " " + state);
            }
            ConLine(1, "perf");
            ConLine(2, " tick " + _tickCounter + "  " + Runtime.LastRunTimeMs.ToString("0.000") + " ms");
            ConLine(2, " instr " + _instrLast + "  peak " + _instrPeak + "  max " + Runtime.MaxInstructionCount);
            ConLine(2, " rate " + Runtime.UpdateFrequency + "  idle " + _idleRuns + "  up " + (int)_time + "s");
            if (_lastScreen.Length > 0) ConLine(2, " last " + _lastScreen);

            var sb = new StringBuilder();
            foreach (var kv in _con) sb.AppendLine(kv.Key);
            _consoleText = sb.ToString();
        }

        private static Color ConColor(int style)
        {
            switch (style)
            {
                case 0: return new Color(232, 121, 249);
                case 1: return new Color(192, 38, 211);
                case 3: return new Color(255, 80, 255);
                case 4: return new Color(71, 85, 105);
            }
            return new Color(170, 180, 195);
        }

        private void DrawConsole()
        {
            Vector2 size = _pbSurface.SurfaceSize;
            Vector2 off = (_pbSurface.TextureSize - size) / 2f;
            int chars = 30;
            foreach (var kv in _con) chars = Math.Max(chars, kv.Key.Length);
            Vector2 cell = _pbSurface.MeasureStringInPixels(new StringBuilder("M"), "Monospace", 1f);
            float scale = Math.Min(1f, Math.Min(size.X * 0.94f / (chars * cell.X), size.Y * 0.94f / (_con.Count * cell.Y)));
            float x = off.X + size.X * 0.03f;
            float y = off.Y + size.Y * 0.03f;
            var sp = new List<MySprite>();
            sp.Add(new MySprite { Type = SpriteType.TEXTURE, Data = "SquareSimple", Position = off + size / 2f, Size = size, Color = new Color(12, 12, 16) });
            foreach (var kv in _con)
            {
                AddBootText(sp, kv.Key, ConColor(kv.Value), x, y, scale);
                y += cell.Y * scale;
            }
            using (var frame = _pbSurface.DrawFrame())
            {
                frame.AddRange(sp);
            }
        }

        // ---------------- boot sequence ----------------

        // Startup logo. Drawn line by line with the screen's boot gradient.
        private static readonly List<string> BootLogo = new List<string>
        {
            "        ZZ     ZZ",
            "     ZZ ZZ     ZZ ZZ",
            "   ZZVZ ZZ     ZZ ZVZZ",
            "  ZZZZZ ZZZZZZZZZ ZZZZZ",
            " ZVZ    ZZ     ZZ    ZVZ",
            " ZGZ    ZZ     ZZ    ZGZ",
            " ZVVZ   ZZ     ZZ   ZVVZ",
            "  ZVVZ ZZ       ZZ ZUVZ",
            "   ZZZ Z         Z ZZZ",
            "     ZZZZZZZ ZZZZZZZ",
            "        ZZZZ ZZZZ"
        };

        // Starts the boot sequence: screen i powers on i * boot_stagger ticks after the first. boot_duration: 0 skips it.
        private void StartBoot()
        {
            _bootT0.Clear();
            if (_config == null) return;
            for (int i = 0; i < _config.Screens.Count; i++)
            {
                var s = _config.Screens[i];
                if (s.BootDuration > 0f) _bootT0[s] = _time + (i / Math.Max(1, s.BootBatch)) * s.BootStaggerTicks / 60.0;
            }
        }

        private void RenderBoots()
        {
            // At most 4 boot frames are drawn per run, rotating, so many screens cannot exceed the instruction limit.
            List<ScreenDef> done = null;
            int groups = (_bootT0.Count + 3) / 4, k = 0;
            foreach (var kv in _bootT0)
            {
                double p = (_time - kv.Value) / kv.Key.BootDuration;
                if (p < 0) continue;
                if (p >= 1.0)
                {
                    if (done == null) done = new List<ScreenDef>();
                    done.Add(kv.Key);
                    continue;
                }
                if ((k++ + _tickCounter) % groups == 0) DrawBoot(kv.Key, p);
            }
            if (done == null) return;
            foreach (var s in done)
            {
                // Rebuild the normal screen from scratch so it draws on its next turn.
                _bootT0.Remove(s);
                _screenBuffers.Remove(s);
                _lastSignature.Remove(s);
            }
        }

        private static void AddBootText(List<MySprite> sp, string text, Color col, float x, float y, float scale)
        {
            var t = MySprite.CreateText(text, "Monospace", col, scale, TextAlignment.LEFT);
            t.Position = new Vector2(x, y);
            sp.Add(t);
        }

        // Draws ASCII art line by line with the screen's logo colours (boot_color, boot_gradient).
        private static void AddArt(List<MySprite> sp, List<string> art, ScreenDef screen, float x, float y, float scale, float lh)
        {
            string[] stops = screen.BootGradientColors != null && screen.BootGradientColors.Length > 0 ? screen.BootGradientColors : new[] { screen.BootColor };
            for (int i = 0; i < art.Count; i++)
            {
                HelmColor c = Layout.GradientLerp(stops, art.Count > 1 ? 100.0 * i / (art.Count - 1) : 0.0);
                AddBootText(sp, art[i], new Color(c.R, c.G, c.B), x, y + i * lh, scale);
            }
        }

        // A full-screen banner: the art is scaled to fill the surface and drawn once.
        private bool RenderBanner(ScreenDef screen, IMyTextSurface surface)
        {
            List<string> art;
            if (_lastSignature.ContainsKey(screen) || !_config.Banners.TryGetValue(screen.Banner, out art)) return false;
            _lastSignature[screen] = "banner";
            Vector2 size = surface.SurfaceSize;
            Vector2 off = (surface.TextureSize - size) / 2f;
            int chars = 1;
            foreach (var l in art) chars = Math.Max(chars, l.Length);
            Vector2 cell = surface.MeasureStringInPixels(new StringBuilder("M"), "Monospace", 1f);
            float scale = Math.Min(size.X * 0.94f / (chars * cell.X), size.Y * 0.94f / (art.Count * cell.Y));
            var sp = new List<MySprite>();
            sp.Add(new MySprite { Type = SpriteType.TEXTURE, Data = "SquareSimple", Position = off + size / 2f, Size = size, Color = string.IsNullOrEmpty(screen.Background) ? new Color(10, 12, 16) : ResolveColor(screen.Background) });
            AddArt(sp, art, screen, off.X + (size.X - chars * cell.X * scale) / 2f, off.Y + (size.Y - art.Count * cell.Y * scale) / 2f, scale, cell.Y * scale);
            using (var frame = surface.DrawFrame())
            {
                frame.AddRange(sp);
            }
            return true;
        }

        // One boot frame at progress p (0 to 1): logo, message and the diagnostic lines revealed so far.
        private void DrawBoot(ScreenDef screen, double p)
        {
            IMyTextSurface surface;
            if (!_resolvedOutputs.TryGetValue(screen, out surface)) return;

            Vector2 size = surface.SurfaceSize;
            Vector2 off = (surface.TextureSize - size) / 2f;
            var logs = screen.GetEffectiveBootLogs();
            string msg = screen.BootMessage ?? "HELM TELEMETRY KERNEL ONLINE";
            bool logo = size.Y >= 200f;
            List<string> art;
            if (screen.BootLogo == null || !_config.Banners.TryGetValue(screen.BootLogo, out art)) art = BootLogo;

            // Fit the block to the surface with the real font metrics.
            int chars = msg.Length + 3;
            foreach (var l in logs) chars = Math.Max(chars, l.Length + 5);
            if (logo) foreach (var l in art) chars = Math.Max(chars, l.Length);
            int rows = (logo ? art.Count + 1 : 0) + 2 + logs.Count;
            Vector2 cell = surface.MeasureStringInPixels(new StringBuilder("M"), "Monospace", 1f);
            float scale = Math.Min(1f, Math.Min(size.X * 0.94f / (chars * cell.X), size.Y * 0.94f / (rows * cell.Y)));
            float x0 = off.X + (size.X - chars * cell.X * scale) / 2f;
            float y = off.Y + (size.Y - rows * cell.Y * scale) / 2f;
            float lh = cell.Y * scale;

            var sp = new List<MySprite>();
            sp.Add(new MySprite { Type = SpriteType.TEXTURE, Data = "SquareSimple", Position = off + size / 2f, Size = size, Color = new Color(10, 12, 16) });

            if (logo)
            {
                AddArt(sp, art, screen, x0, y, scale, lh);
                y += (art.Count + 1) * lh;
            }

            AddBootText(sp, ">> " + msg, new Color(34, 197, 94), x0, y, scale);
            y += lh * 2f;

            int shown = Layout.BootLogCount(p, logs.Count);
            for (int i = 0; i < shown; i++)
            {
                bool ok = i == logs.Count - 1 && p >= 0.8;
                AddBootText(sp, ok ? "[OK]" : "[..]", ok ? new Color(34, 197, 94) : new Color(56, 189, 248), x0, y, scale);
                AddBootText(sp, logs[i], new Color(203, 213, 225), x0 + 5f * cell.X * scale, y, scale);
                y += lh;
            }

            using (var frame = surface.DrawFrame())
            {
                frame.AddRange(sp);
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
                        if (bind.DefaultText != null) _dynText[k] = bind.DefaultText;
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
                        var groupBlocks = new List<IMyTerminalBlock>();
                        CollectGroup(targetName, groupBlocks, bind.AllShips);
                        if (groupBlocks.Count > 0)
                        {
                            foreach (var gb in groupBlocks)
                            {
                                if (!blocks.Contains(gb)) blocks.Add(gb);
                            }
                        }
                        else
                        {
                            // Search all terminal blocks for wildcard or exact match
                            var allBlocks = AllBlocks(bind.AllShips);
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
                        var block = FindBlock(targetName, bind.AllShips);
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

                var outBlock = FindBlock(screen.OutputBlockName);
                if (outBlock == null)
                {
                    _outErr.AppendLine($"HELM: output block '{screen.OutputBlockName}' not found");
                    continue;
                }

                IMyTextSurface surface = null;
                var provider = outBlock as IMyTextSurfaceProvider;
                var directSurface = outBlock as IMyTextSurface;
                if (provider != null && screen.OutputSurfaceIndex < provider.SurfaceCount)
                {
                    surface = provider.GetSurface(screen.OutputSurfaceIndex);
                }
                else if (directSurface != null && screen.OutputSurfaceIndex == 0)
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
            _rawBlocks.Clear();
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
            _allLoaded = false;
            Echo($"Blocks: {AllBlocks().Count} on this construct; {_foreignBlocks} on docked ships (ignored)");
            _allBlocks.Clear();
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

                List<IMyTerminalBlock> blocks;
                if (_resolvedBinds.TryGetValue(bind.Name, out blocks))
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

            IMyTextSurface anySurface = null;
            foreach (var os in _resolvedOutputs.Values) { anySurface = os; break; }
            if (anySurface != null)
            {
                var spriteNames = new List<string>();
                anySurface.GetSprites(spriteNames);
                Echo($"--- SPRITES ({spriteNames.Count}): names usable in bg_texture ---");
                Echo(string.Join(", ", spriteNames.Take(60).ToArray()));
            }
            Echo("--- SURFACE OUTPUT AUDIT ---");
            foreach (var screen in _config.Screens)
            {
                IMyTextSurface surface;
                if (_resolvedOutputs.TryGetValue(screen, out surface))
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
                return tool.IsActivated ? 100.0 : (tool.Enabled ? 50.0 : 0.0);

            var gen = block as IMyGasGenerator;
            if (gen != null)
                return gen.AutoRefill ? 100.0 : (gen.Enabled ? 50.0 : 0.0);

            var turret = block as IMyLargeTurretBase;
            if (turret != null)
                return turret.IsShooting ? 100.0 : (turret.HasTarget ? 50.0 : (turret.Enabled ? 10.0 : 0.0));

            var proj = block as IMyProjector;
            if (proj != null)
                return proj.TotalBlocks > 0 ? ((double)(proj.TotalBlocks - proj.RemainingBlocks) / proj.TotalBlocks) * 100.0 : 0.0;

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

        // Amount of one item in all of a block's inventories. Field "kind:Subtype", for example ingot:Iron, ore:Ice,
        // component:SteelPlate, ammo:NATO_25x184mm. A reactor's fuel is ingot:Uranium.
        private static double ItemAmount(IMyTerminalBlock b, string field)
        {
            int c = field.IndexOf(':');
            string kind = field.Substring(0, c).ToLowerInvariant();
            var type = new MyItemType("MyObjectBuilder_" + (kind == "ammo" ? "AmmoMagazine" : char.ToUpper(kind[0]) + kind.Substring(1)), field.Substring(c + 1));
            double sum = 0;
            for (int i = 0; i < b.InventoryCount; i++) sum += (double)b.GetInventory(i).GetItemAmount(type);
            return sum;
        }

        private double ResolveBlockField(IMyTerminalBlock block, string field)
        {
            if (block == null) return 0.0;
            string f = field.ToLowerInvariant();
            if (f == "percent" || f == "pct") return ResolvePercent(block);
            if (f.IndexOf(':') > 0) return ItemAmount(block, field);

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
                return (double)door.OpenRatio * 100.0;
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
                if (f == "brake") return susp.Brake ? 100.0 : 0.0;
                if (f == "height") return susp.Height;
                if (f == "strength") return susp.Strength;
                if (f == "power") return susp.Power;
                if (f == "steer") return susp.SteeringOverride * 100.0;
                if (f == "propulsion") return susp.PropulsionOverride * 100.0;
                return susp.Friction;
            }

            var connector = block as IMyShipConnector;
            if (connector != null)
            {
                return connector.Status == MyShipConnectorStatus.Connected ? 100.0 : 0.0;
            }

            var prod = block as IMyProductionBlock;
            if (prod != null)
            {
                if (f == "producing" || f == "active" || f == "running" || f == "state") return prod.IsProducing ? 100.0 : 0.0;
                if (f == "queue") return prod.IsQueueEmpty ? 0.0 : 1.0;
            }

            var asm = block as IMyAssembler;
            if (asm != null)
            {
                if (f == "disassembling") return asm.Mode == MyAssemblerMode.Disassembly ? 100.0 : 0.0;
                if (f == "repeating" || f == "loop") return asm.Repeating ? 100.0 : 0.0;
            }

            var gen = block as IMyGasGenerator;
            if (gen != null)
            {
                if (f == "autorefill") return gen.AutoRefill ? 100.0 : 0.0;
                if (f == "active") return gen.Enabled ? 100.0 : 0.0;
            }

            var turret = block as IMyLargeTurretBase;
            if (turret != null)
            {
                if (f == "shooting") return turret.IsShooting ? 100.0 : 0.0;
                if (f == "target" || f == "hastarget") return turret.HasTarget ? 100.0 : 0.0;
                if (f == "range") return turret.Range;
            }

            var proj = block as IMyProjector;
            if (proj != null)
            {
                if (f == "built") return proj.TotalBlocks - proj.RemainingBlocks;
                if (f == "remaining") return proj.RemainingBlocks;
                if (f == "total") return proj.TotalBlocks;
                if (f == "projecting") return proj.IsProjecting ? 100.0 : 0.0;
                if (f == "armor") return proj.RemainingArmorBlocks;
                if (f == "buildable") return proj.BuildableBlocksCount;
            }

            var tool = block as IMyShipToolBase;
            if (tool != null)
            {
                if (f == "working" || f == "active" || f == "activated") return tool.IsActivated ? 100.0 : 0.0;
                return tool.Enabled ? 100.0 : 0.0;
            }

            var gyro = block as IMyGyro;
            if (gyro != null)
            {
                if (f == "override") return gyro.GyroOverride ? 100.0 : 0.0;
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
                if (f == "locked") return gear.IsLocked ? 100.0 : 0.0;
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
                if (f == "locked") return rotor.RotorLock ? 100.0 : 0.0;
            }

            var warhead = block as IMyWarhead;
            if (warhead != null)
            {
                if (f == "armed") return warhead.IsArmed ? 100.0 : 0.0;
                if (f == "countdown") return warhead.DetonationTime;
            }

            var sensor = block as IMySensorBlock;
            if (sensor != null)
            {
                if (f == "detected") return !sensor.LastDetectedEntity.IsEmpty() ? 100.0 : 0.0;
            }

            var camera = block as IMyCameraBlock;
            if (camera != null)
            {
                if (f == "range") return camera.AvailableScanRange;
                if (f == "active") return camera.IsActive ? 100.0 : 0.0;
            }

            var chute = block as IMyParachute;
            if (chute != null)
            {
                if (f == "open" || f == "openratio") return (double)chute.OpenRatio * 100.0;
            }

            var tcb = block as IMyTurretControlBlock;
            if (tcb != null)
            {
                if (f == "target" || f == "hastarget") return tcb.HasTarget ? 100.0 : 0.0;
                if (f == "azimuth") return tcb.AzimuthRotor != null ? tcb.AzimuthRotor.Angle * 180.0 / Math.PI : 0.0;
                if (f == "elevation") return tcb.ElevationRotor != null ? tcb.ElevationRotor.Angle * 180.0 / Math.PI : 0.0;
            }

            var func = block as IMyFunctionalBlock;
            if (func != null)
            {
                return func.Enabled ? 100.0 : 0.0;
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
                double dVal;
                if (_dynamicValues.TryGetValue(k, out dVal)) return dVal;
                return bindDef.DefaultValue;
            }
            double dynVal;
            if (_dynamicValues.TryGetValue(bindName, out dynVal)) return dynVal;

            List<IMyTerminalBlock> blocks;
            if (!_resolvedBinds.TryGetValue(bindName, out blocks) || blocks.Count == 0)
                throw new InvalidOperationException($"HELM: bind '{bindName}' resolved to zero blocks — check the name/typo");

            if (blocks.Count == 1)
                return ResolveBlockField(blocks[0], field);

            var values = new List<double>();
            foreach (var b in blocks) values.Add(ResolveBlockField(b, field));
            return Aggregator.Aggregate(bindDef != null ? bindDef.Aggregation : Agg.Avg, values);
        }

        private double ResolvePercentForBind(string bindName)
        {
            return ResolveFieldValue(bindName, "percent");
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

        // Returns true when the screen was redrawn (something changed).
        private bool RenderScreen(ScreenDef screen)
        {
            IMyTextSurface surface;
            if (!_resolvedOutputs.TryGetValue(screen, out surface))
                return false;

            if (screen.Banner != null) return RenderBanner(screen, surface);
            if (screen.Border.Equals("ascii", StringComparison.OrdinalIgnoreCase))
            {
                return RenderScreenAscii(screen, surface);
            }
            return RenderScreenGraphic(screen, surface);
        }

        // =========================================================================
        // Approach B: Retained Virtual Sprite Buffer (Geometric quads + in-place mutation)
        // =========================================================================

        // The colour at pct (0 to 100) along the stops, blended between the two neighbouring stops.
        private Color GradColor(string[] stops, double pct)
        {
            if (stops.Length == 1) return ResolveColor(stops[0]);
            double r = Math.Max(0.0, Math.Min(100.0, pct)) / 100.0 * (stops.Length - 1);
            int i = Math.Min((int)r, stops.Length - 2);
            float t = (float)(r - i);
            Color a = ResolveColor(stops[i]), b = ResolveColor(stops[i + 1]);
            return new Color((int)(a.R + (b.R - a.R) * t), (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t), (int)(a.A + (b.A - a.A) * t));
        }

        // A gradient as 16 bands inside a rectangle. Returns the band colours in order.
        private static Color[] AddBands(ScreenBuffer buf, string[] stops, string dir, float x, float y, float w, float h)
        {
            int bands = 16;
            bool horizontal = dir == "horizontal";
            var cols = new Color[bands];
            for (int i = 0; i < bands; i++)
            {
                HelmColor gc = Layout.GradientLerp(stops, 100.0 * i / (bands - 1));
                cols[i] = new Color(gc.R, gc.G, gc.B);
                float bw = w / bands, bh = h / bands;
                AddQuad(buf,
                    horizontal ? new Vector2(x + bw * (i + 0.5f), y + h / 2f) : new Vector2(x + w / 2f, y + bh * (i + 0.5f)),
                    horizontal ? new Vector2(bw + 1f, h) : new Vector2(w, bh + 1f), cols[i]);
            }
            return cols;
        }

        private static void AddQuad(ScreenBuffer buf, Vector2 pos, Vector2 size, Color col, float rot = 0f, string data = "SquareSimple")
        {
            buf.Sprites.Add(new MySprite { Type = SpriteType.TEXTURE, Data = data, Position = pos, Size = size, Color = col, RotationOrScale = rot });
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
            buf.BgIndex = -1;
            buf.AlertShown = false;
            buf.AlertCol = ResolveColor(screen.AlertBg ?? "#5b0f0f");
            buf.Bgs.Clear();
            if (screen.BgGradient != null && screen.BgGradient.Length > 1)
            {
                buf.BgIndex = buf.Sprites.Count;
                buf.BgColors = AddBands(buf, screen.BgGradient, screen.BgDir, viewportOffset.X + 2f, viewportOffset.Y + 2f, surfaceSize.X - 4f, surfaceSize.Y - 4f);
            }
            else if (drawBg || screen.BgRules != null || screen.FlashRules != null)
            {
                if (screen.BgRules != null) bgCol = ResolveColor(screen.BgRules.First(r => r.IsDefault).Color);
                buf.BgIndex = buf.Sprites.Count;
                buf.BgColors = new[] { bgCol };
                AddQuad(buf, viewportOffset + (surfaceSize / 2f), surfaceSize - new Vector2(4f, 4f), bgCol);
                if (screen.BgRules != null || screen.FlashRules != null)
                    buf.Bgs.Add(new BgEntry { Index = buf.BgIndex, Base = bgCol, Cur = bgCol, Rules = screen.BgRules, Bind = screen.BgBind, Field = screen.BgField, Flash = screen.FlashRules, FlashBind = screen.FlashBind, FlashField = screen.FlashField, IsScreen = true });
            }

            if (!string.IsNullOrEmpty(screen.BgTexture))
                AddQuad(buf, viewportOffset + (surfaceSize / 2f), surfaceSize - new Vector2(4f, 4f), ResolveColor(screen.BgTextureColor ?? "255,255,255,24"), 0f, screen.BgTexture);

            if (!screen.Border.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                Color borderColTop = screen.BorderGradientColors != null && screen.BorderGradientColors.Length > 0
                    ? ResolveColor(screen.BorderGradientColors[0])
                    : ResolveColor(screen.BorderColor ?? "40, 60, 85");
                Color borderColBot = screen.BorderGradientColors != null && screen.BorderGradientColors.Length > 1
                    ? ResolveColor(screen.BorderGradientColors[screen.BorderGradientColors.Length - 1])
                    : borderColTop;

                float borderX = viewportOffset.X + (surfaceSize.X - usableW) / 2f;
                if (screen.BorderGradientColors != null && screen.BorderGradientColors.Length > 1 && !screen.GradientSteps)
                {
                    // border_gradient blends across the frame: both lines run through all the stops, left to right.
                    AddBands(buf, screen.BorderGradientColors, "horizontal", borderX, viewportOffset.Y + paddingY * 0.5f - 1f, usableW, 2f);
                    AddBands(buf, screen.BorderGradientColors, "horizontal", borderX, viewportOffset.Y + surfaceSize.Y - (paddingY * 0.5f) - 1f, usableW, 2f);
                }
                else
                {
                    AddQuad(buf, new Vector2(viewportOffset.X + surfaceSize.X / 2f, viewportOffset.Y + paddingY * 0.5f), new Vector2(usableW, 2f), borderColTop);
                    AddQuad(buf, new Vector2(viewportOffset.X + surfaceSize.X / 2f, viewportOffset.Y + surfaceSize.Y - (paddingY * 0.5f)), new Vector2(usableW, 2f), borderColBot);
                }
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

                bool ruled = panel.BgRules != null || panel.FlashRules != null;
                if (panel.BgGradient != null && panel.BgGradient.Length > 1)
                {
                    AddBands(buf, panel.BgGradient, panel.BgDir != "vertical" ? panel.BgDir : screen.BgDir, pLeft, pTop, pWidth, pHeight);
                }
                else
                {
                    // A panel's own colour, its value-driven colour, or (on a multi-panel screen) the default card: a faint
                    // white overlay that lifts any background instead of painting a second colour. panel_bg: sets it or "none".
                    Color pc;
                    if (panel.BgRules != null) pc = ResolveColor(panel.BgRules.First(r => r.IsDefault).Color);
                    else if (!string.IsNullOrEmpty(panel.Background)) pc = ResolveColor(panel.Background);
                    else if (screen.GridCols > 1 || screen.GridRows > 1) pc = string.IsNullOrEmpty(screen.PanelBg) ? new Color(255, 255, 255, 6) : ResolveColor(screen.PanelBg);
                    else pc = Color.Transparent;
                    if (pc.A > 0 || ruled)
                    {
                        if (ruled) buf.Bgs.Add(new BgEntry { Index = buf.Sprites.Count, Base = pc, Cur = pc, Rules = panel.BgRules, Bind = panel.BgBind, Field = panel.BgField, Flash = panel.FlashRules, FlashBind = panel.FlashBind, FlashField = panel.FlashField });
                        AddQuad(buf, new Vector2(pLeft + pWidth / 2f, pTop + pHeight / 2f), new Vector2(pWidth, pHeight), pc);
                    }
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


        // Sets the text and colour of a line's value sprite, or the colour of its fill sprite, in the retained buffer.
        private static void SetVal(ScreenBuffer buf, LineSlot slot, string text, Color col)
        {
            var v = buf.Sprites[slot.ValSpriteIndex];
            v.Data = text;
            v.Color = col;
            buf.Sprites[slot.ValSpriteIndex] = v;
        }

        private static void SetFill(ScreenBuffer buf, LineSlot slot, Color col)
        {
            var f = buf.Sprites[slot.FillSpriteIndex];
            f.Color = col;
            buf.Sprites[slot.FillSpriteIndex] = f;
        }

        private bool RenderScreenGraphic(ScreenDef screen, IMyTextSurface surface)
        {
            ScreenBuffer buf;
            if (!_screenBuffers.TryGetValue(screen, out buf))
            {
                buf = new ScreenBuffer();
                _screenBuffers[screen] = buf;
            }

            if (!buf.Initialized || buf.LastSurfaceSize != surface.SurfaceSize)
            {
                BuildGraphicBuffer(screen, surface, buf);
            }

            bool dirty = false;
            bool alertPhase = (_time % screen.AlertPeriod) < screen.AlertPeriod / 2.0;

            // Red alert (run argument "alert red"): the background flashes to alert_bg. alert_bg: none turns it off.
            bool wantAlert = _globalRedAlert && alertPhase && buf.AlertCol.A > 0;
            if (buf.BgIndex >= 0 && wantAlert != buf.AlertShown)
            {
                buf.AlertShown = wantAlert;
                for (int i = 0; i < buf.BgColors.Length; i++)
                {
                    var bg = buf.Sprites[buf.BgIndex + i];
                    bg.Color = wantAlert ? buf.AlertCol : buf.BgColors[i];
                    buf.Sprites[buf.BgIndex + i] = bg;
                }
                dirty = true;
                foreach (var be in buf.Bgs) be.Force = true;
            }

            // Backgrounds that follow a value (bg: color(...)) or flash on one (flash:).
            foreach (var be in buf.Bgs)
            {
                if (be.IsScreen && buf.AlertShown) continue;
                Color bc = be.Base;
                try
                {
                    if (be.Rules != null) bc = ResolveColor(Layout.ResolveConditionalColor(be.Rules, ResolveFieldValue(be.Bind, be.Field)));
                    if (be.Flash != null && alertPhase && Layout.ResolveConditionalColor(be.Flash, ResolveFieldValue(be.FlashBind, be.FlashField)) != null) bc = buf.AlertCol;
                }
                catch { }
                if (bc != be.Cur || be.Force)
                {
                    be.Cur = bc;
                    be.Force = false;
                    var bs = buf.Sprites[be.Index];
                    bs.Color = bc;
                    buf.Sprites[be.Index] = bs;
                    dirty = true;
                }
            }

            for (int i = 0; i < buf.Slots.Count; i++)
            {
                var slot = buf.Slots[i];
                var line = slot.Line;
                double value;
                bool hasErr = false;

                double mx = line.Max;
                try
                {
                    value = ResolveFieldValue(line.BindName, line.Field);
                    if (line.MaxBind != null) mx = ResolveFieldValue(line.MaxBind, line.MaxField);
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

                // max(N): gauges work on the percentage of N; the text keeps the real amount.
                double raw = value;
                if (mx > 0) value = Math.Min(100.0, raw / mx * 100.0);

                string colorName = line.Gradient
                    ? Layout.GradientColorAt(line.GradientColors, value)
                    : line.FlatColor;
                Color barCol = ResolveColor(colorName);
                bool isRedAlert = (barCol == Color.Red); // decided by the nearest stop, so red segments still flash
                if (line.Gradient && !screen.GradientSteps && line.GradientColors.Length > 1) barCol = GradColor(line.GradientColors, value);

                if (isRedAlert && alertPhase)
                {
                    barCol = new Color(255, 75, 75);
                    dirty = true;
                }

                bool valueChanged = Math.Abs(raw - slot.LastValue) > 0.1;
                if (valueChanged) slot.LastValue = raw;

                if (line.Kind == LineKind.Bar)
                {
                    double fillVal = (line.HasMax || line.Field == "percent" || line.Field == "pct") ? value : ResolvePercentForBind(line.BindName);
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

                        SetVal(buf, slot, FormatDisplayValue(raw, line), barCol);
                    }
                }
                else if (line.Kind == LineKind.LedBar || line.Kind == LineKind.Hazard || line.Kind == LineKind.Chevron || line.Kind == LineKind.Dots)
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        double fillVal = (line.HasMax || line.Field == "percent" || line.Field == "pct") ? value : ResolvePercentForBind(line.BindName);
                        int segCount = slot.ExtraSpriteIndices.Count;
                        for (int s = 0; s < segCount; s++)
                        {
                            bool lit = fillVal >= ((s + 0.4) / (double)segCount * 100.0);
                            Color segCol = lit ? barCol : new Color(24, 28, 36);
                            var segSprite = buf.Sprites[slot.ExtraSpriteIndices[s]];
                            segSprite.Color = segCol;
                            buf.Sprites[slot.ExtraSpriteIndices[s]] = segSprite;
                        }

                        SetVal(buf, slot, FormatDisplayValue(raw, line), barCol);
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

                        SetVal(buf, slot, FormatDisplayValue(raw, line), barCol);
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

                        SetVal(buf, slot, FormatDisplayValue(raw, line), barCol);
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

                        SetVal(buf, slot, isOn ? "ON" : "OFF", knobCol);
                    }
                }
                else if (line.Kind == LineKind.Checkbox)
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        bool isChecked = value >= 50.0;
                        Color checkCol = isChecked ? (barCol != Color.White ? barCol : new Color(56, 189, 248)) : new Color(30, 36, 48);

                        SetFill(buf, slot, checkCol);

                        SetVal(buf, slot, isChecked ? "ACTIVE" : "READY", isChecked ? checkCol : new Color(148, 163, 184));
                    }
                }
                else if (line.Kind == LineKind.Status)
                {
                    if (valueChanged || isRedAlert)
                    {
                        dirty = true;
                        string statusLabel = value >= 75.0 ? "ONLINE" : (value >= 25.0 ? "STBY" : "OFFLINE");
                        Color statusCol = value >= 75.0 ? new Color(34, 197, 94) : (value >= 25.0 ? new Color(234, 179, 8) : new Color(239, 68, 68));

                        SetFill(buf, slot, statusCol);

                        SetVal(buf, slot, statusLabel, statusCol);
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

                        SetVal(buf, slot, FormatDisplayValue(raw, line), barCol);
                    }
                }
                else if (line.Kind == LineKind.Flow)
                {
                    dirty = true; // Rotates/updates dynamically
                    float rot;
                    Color flowCol;
                    string flowText;

                    bool isMachinery = (line.Field == "producing" || line.Field == "active" || line.Field == "running" || line.Field == "state" || (line.Field != "percent" && (value <= 1.0 || value == 100.0)));
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
                    fillSprite.RotationOrScale = rot;
                    fillSprite.Color = flowCol;
                    buf.Sprites[slot.FillSpriteIndex] = fillSprite;

                    SetVal(buf, slot, flowText, flowCol);
                }
                else // Text line
                {
                    string dt;
                    BindDef tb;
                    _bindMap.TryGetValue(line.BindName, out tb);
                    _dynText.TryGetValue(tb != null && tb.DynamicKey != null ? tb.DynamicKey : line.BindName, out dt);
                    if (valueChanged || isRedAlert || dt != slot.LastText)
                    {
                        slot.LastText = dt;
                        dirty = true;
                        Color txtCol = line.ColorRules != null
                            ? ResolveColor(Layout.ResolveConditionalColor(line.ColorRules, value))
                            : Color.White;

                        if (txtCol == Color.Red && alertPhase)
                            txtCol = new Color(255, 75, 75);

                        SetVal(buf, slot, dt ?? FormatDisplayValue(raw, line), txtCol);
                    }
                }

                if (slot.HasDynamicLabel && (valueChanged || isRedAlert))
                {
                    dirty = true;
                    var lblSprite = buf.Sprites[slot.LabelSpriteIndex];
                    string pctStr = GetPercentString(value);
                    string rawVal = FormatDisplayValue(raw, line);
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
        

            return dirty;
        }

        // =========================================================================
        // Legacy/Retro ASCII Bordered Window Rendering
        // =========================================================================

        private bool RenderScreenAscii(ScreenDef screen, IMyTextSurface surface)
        {
            surface.FontSize = screen.FontSize;

            var contentLines = new List<KeyValuePair<string, string>>();

            foreach (var line in screen.Lines)
            {
                double value = 0.0;
                string text;
                string color = null;

                double mx = line.Max;
                try
                {
                    value = ResolveFieldValue(line.BindName, line.Field);
                    if (line.MaxBind != null) mx = ResolveFieldValue(line.MaxBind, line.MaxField);
                }
                catch (Exception ex)
                {
                    contentLines.Add(new KeyValuePair<string, string>($"{line.Label}: [UNRESOLVED]", "red"));
                    continue;
                }

                double raw = value;
                if (mx > 0) value = Math.Min(100.0, raw / mx * 100.0);
                string valStr = FormatDisplayValue(raw, line);

                if (line.Kind == LineKind.Bar)
                {
                    double fillVal = (line.HasMax || line.Field == "percent" || line.Field == "pct") ? value : ResolvePercentForBind(line.BindName);
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
                    bool isMachinery = (line.Field == "producing" || line.Field == "active" || line.Field == "running" || line.Field == "state" || (line.Field != "percent" && (value <= 1.0 || value == 100.0)));
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

                contentLines.Add(new KeyValuePair<string, string>(text, color));
            }

            var borderedRaw = Layout.RenderBorderedWindow(
                new List<string>(contentLines.ConvertAll(t => t.Key)),
                width: screen.Width
            );

            // Dirty-flag check
            string signature = string.Join("\n", borderedRaw);
            string prevSig;
            if (_lastSignature.TryGetValue(screen, out prevSig) && prevSig == signature)
                return false;
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
                        colorName = contentLines[i - 1].Value;

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
        

            return true;
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
}
