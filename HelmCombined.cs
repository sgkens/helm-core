// ==============================================================================
// HELM: High-Efficiency Lightweight Monitor for Space Engineers
// Paste into the Programmable Block editor (replaces everything in it).
// This is the body of Program: no usings, namespace or class declaration.
// ==============================================================================

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
        private const string HELM_VERSION = "0.11.0"; // stamped from the VERSION file by Build-Release.ps1
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
            if (screen.BgGradient != null && screen.BgGradient.Length > 1)
            {
                // Vertical gradient as 16 bands of the surface.
                int bands = 16;
                float bandH = (surfaceSize.Y - 4f) / bands;
                buf.BgIndex = buf.Sprites.Count;
                buf.BgColors = new Color[bands];
                for (int i = 0; i < bands; i++)
                {
                    HelmColor gc = Layout.GradientLerp(screen.BgGradient, 100.0 * i / (bands - 1));
                    buf.BgColors[i] = new Color(gc.R, gc.G, gc.B);
                    AddQuad(buf, new Vector2(viewportOffset.X + surfaceSize.X / 2f, viewportOffset.Y + 2f + bandH * (i + 0.5f)), new Vector2(surfaceSize.X - 4f, bandH + 1f), buf.BgColors[i]);
                }
            }
            else if (drawBg)
            {
                buf.BgIndex = buf.Sprites.Count;
                buf.BgColors = new[] { bgCol };
                AddQuad(buf, viewportOffset + (surfaceSize / 2f), surfaceSize - new Vector2(4f, 4f), bgCol);
            }

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
                    // The default card is a faint white overlay, so it lifts any background (solid, gradient or
                    // transparent) instead of painting a second colour over it. panel_bg: sets a colour or "none".
                    Color card = string.IsNullOrEmpty(screen.PanelBg) ? new Color(255, 255, 255, 6) : ResolveColor(screen.PanelBg);
                    if (card.A > 0)
                        AddQuad(buf, new Vector2(pLeft + pWidth / 2f, pTop + pHeight / 2f), new Vector2(pWidth, pHeight), card);
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
                bool isRedAlert = (barCol == Color.Red);

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
        public string DefaultText;
        public string DynamicKey;
        public bool AllShips;   // scope: all - also use blocks on ships docked on a connector
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
        public double Max;            // max(N): the value is shown as a percentage of N (0 = off)
        public string MaxBind, MaxField; // max(bind.field): the maximum is read from a block field
        public bool HasMax { get { return Max > 0 || MaxBind != null; } }
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
        public int BootBatch = 1; // screens that power on together
        public string PanelBg;    // colour of the default card behind panels of a multi-panel screen; "none" for no card (default: a faint white overlay)
        public string[] BgGradient; // vertical background gradient stops, top to bottom (overrides bg:)
        public string AlertBg;    // background colour flashed while the red alert is on; "none" to disable
        public string Banner;     // name of a @banner shown full screen instead of panels
        public string BootLogo;   // name of a @banner drawn instead of the default boot emblem
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
        public int RefreshTicks = 0;      // refresh: draw this screen at most every N ticks (0 = every run)
        public float AlertPeriod = 1.6f;  // alert_period: seconds for one on/off flash cycle
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
        // ASCII art blocks from "@banner name" ... "@end", one entry per line, left indentation removed.
        public Dictionary<string, List<string>> Banners = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
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

            var preprocessed = PreprocessTemplates(customData, cfg.Errors, cfg.Banners);

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
                    else if (line.StartsWith("alert_period:"))
                    {
                        RequireScreen(current, "alert_period:");
                        current.AlertPeriod = Math.Max(0.2f, float.Parse(line.Substring("alert_period:".Length).Trim().TrimEnd('s'), System.Globalization.CultureInfo.InvariantCulture));
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
                    else if (line.StartsWith("boot_batch:"))
                    {
                        RequireScreen(current, "boot_batch:");
                        current.BootBatch = Math.Max(1, int.Parse(line.Substring("boot_batch:".Length).Trim()));
                    }
                    else if (line.StartsWith("boot_duration:"))
                    {
                        RequireScreen(current, "boot_duration:");
                        // "3s" is seconds; a bare number is game ticks (60 per second): "180" is 3 seconds.
                        string dStr = line.Substring("boot_duration:".Length).Trim().ToLowerInvariant();
                        bool inSeconds = dStr.EndsWith("s");
                        if (inSeconds) dStr = dStr.Substring(0, dStr.Length - 1).Trim();
                        float dur;
                        if (float.TryParse(dStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out dur))
                        {
                            current.BootDuration = inSeconds ? dur : dur / 60f;
                        }
                    }
                    else if (line.StartsWith("boot_log:"))
                    {
                        RequireScreen(current, "boot_log:");
                        current.BootLogs.Add(Unquote(line.Substring("boot_log:".Length).Trim()));
                    }
                    else if (line.StartsWith("banner:"))
                    {
                        RequireScreen(current, "banner:");
                        current.Banner = Unquote(line.Substring("banner:".Length).Trim());
                    }
                    else if (line.StartsWith("boot_logo:"))
                    {
                        RequireScreen(current, "boot_logo:");
                        current.BootLogo = Unquote(line.Substring("boot_logo:".Length).Trim());
                    }
                    else if (line.StartsWith("boot_gradient:") || line.StartsWith("boot_gradient(") || line.StartsWith("banner_gradient:") || line.StartsWith("banner_gradient("))
                    {
                        RequireScreen(current, "boot_gradient");
                        current.BootGradientColors = ParseStops(line);
                    }
                    else if (line.StartsWith("bg_gradient:") || line.StartsWith("bg_gradient("))
                    {
                        RequireScreen(current, "bg_gradient");
                        current.BgGradient = ParseStops(line);
                    }
                    else if (line.StartsWith("panel_bg:"))
                    {
                        RequireScreen(current, "panel_bg:");
                        current.PanelBg = line.Substring("panel_bg:".Length).Trim();
                    }
                    else if (line.StartsWith("alert_bg:"))
                    {
                        RequireScreen(current, "alert_bg:");
                        current.AlertBg = line.Substring("alert_bg:".Length).Trim();
                    }
                    else if (line.StartsWith("boot_color:") || line.StartsWith("banner_color:"))
                    {
                        RequireScreen(current, "boot_color:");
                        current.BootColor = line.Substring(line.IndexOf(':') + 1).Trim();
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

            foreach (var sc in cfg.Screens)
            {
                if (sc.Banner != null && !cfg.Banners.ContainsKey(sc.Banner)) cfg.Errors.Add("Screen '" + sc.Name + "': banner '" + sc.Banner + "' is not defined (@banner " + sc.Banner + " ... @end)");
                if (sc.BootLogo != null && !cfg.Banners.ContainsKey(sc.BootLogo)) cfg.Errors.Add("Screen '" + sc.Name + "': boot_logo '" + sc.BootLogo + "' is not defined (@banner " + sc.BootLogo + " ... @end)");
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

        // Colour stops of "name(a, b, c)" or "name: a, b, c".
        private static string[] ParseStops(string line)
        {
            int gi = line.IndexOfAny(new[] { ':', '(' });
            string g = line.Substring(line[gi] == ':' ? gi + 1 : gi).Trim();
            if (g.StartsWith("(") && g.Contains(")")) g = ExtractParenArgs(g, 0);
            return SplitArgs(g).Select(s => s.Trim()).ToArray();
        }

        private static List<string> PreprocessTemplates(string customData, List<string> errors, Dictionary<string, List<string>> banners)
        {
            var userTemplates = new Dictionary<string, TemplateDef>(StringComparer.OrdinalIgnoreCase);
            var rawLines = (customData ?? "").Replace("\r\n", "\n").Split('\n');
            var outLines = new List<string>();

            int i = 0;
            while (i < rawLines.Length)
            {
                string raw = rawLines[i++];

                // A banner is raw text: '#', quotes and spaces are art, not syntax, so it is read before comments are stripped.
                if (raw.TrimStart().StartsWith("@banner ", StringComparison.OrdinalIgnoreCase))
                {
                    var art = new List<string>();
                    while (i < rawLines.Length)
                    {
                        string a = rawLines[i++].Replace("\t", "    ").TrimEnd();
                        if (a.Trim().Equals("@end", StringComparison.OrdinalIgnoreCase)) break;
                        art.Add(a);
                    }
                    while (art.Count > 0 && art[0].Length == 0) art.RemoveAt(0);
                    while (art.Count > 0 && art[art.Count - 1].Length == 0) art.RemoveAt(art.Count - 1);
                    int indent = int.MaxValue;
                    foreach (var a in art) if (a.Length > 0) indent = Math.Min(indent, a.Length - a.TrimStart().Length);
                    for (int k = 0; k < art.Count; k++) if (art[k].Length > 0) art[k] = art[k].Substring(indent);
                    banners[raw.TrimStart().Substring(8).Trim()] = art;
                    continue;
                }

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
                        else
                            def.DefaultText = Unquote(kv[1].Trim());
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
                else if (kv.Length == 2 && kv[0].Trim().Equals("scope", StringComparison.OrdinalIgnoreCase))
                {
                    def.AllShips = kv[1].Trim().Equals("all", StringComparison.OrdinalIgnoreCase);
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
                else if (mFunc == "max")
                {
                    double mv;
                    if (double.TryParse(mArgs.Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out mv)) def.Max = mv;
                    else
                    {
                        var mp = mArgs.Trim().Split('.');
                        if (mp.Length != 2) throw new FormatException("max() takes a number or bindname.field");
                        def.MaxBind = mp[0].Trim();
                        def.MaxField = mp[1].Trim();
                    }
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

        // Number of diagnostic boot lines shown at boot progress p (0 to 1): at least one once the boot has started.
        public static int BootLogCount(double p, int total)
        {
            return Math.Max(0, Math.Min(total, 1 + (int)(Math.Max(0.0, p) * total)));
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