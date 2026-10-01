// HelmCore.cs
// Zero-dependency logic: DSL parsing, aggregation math, and text/border layout.
// Per PDR §9: this is the part that should be unit-testable without SE at all.
// Nothing in this file touches IMyTextSurface, MySprite, or GridTerminalSystem.

using System;
using System.Collections.Generic;
using System.Linq; // NOTE: LINQ used here deliberately — this file only runs at
                    // parse-time (on Custom Data change), never per-tick. The
                    // PDR's "no LINQ" rule (§5) is a per-tick-path rule; this
                    // isn't one. Flagging explicitly so it isn't miscopied into
                    // the per-tick renderer path later.

namespace HelmCore
{
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
        public double DynamicTtl = 10; // ttl: seconds a received dynamic value remains fresh
        public bool AllShips;   // scope: all - also use blocks on ships docked on a connector
    }

    public enum LineKind { Text, Bar, LedBar, Spark, Hazard, Chevron, Dots, Balance, Toggle, Checkbox, Status, Ammo, Flow, Tank, Dial }

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
        public List<double> Marks;    // marker(N, ...): threshold ticks on a bar, in percent of the bar
        public string Icon;           // icon(Name): a built-in sprite drawn before the label
        public List<ColorRule> BlinkRules; // blink(bind.field > N): the gauge flashes while the comparison is true
        public string BlinkBind, BlinkField;
        public bool NoValue;          // value(off): no value text, and the gauge uses its space
        public int Segs;              // segments(N) for led and dots, points(N) for spark (2 to 40; 0 = the default)
        public int Ticks;             // ticks(N): a bar divided into N parts
        public string Outline;        // outline(color): a frame around a bar
        public int Size;              // size(medium) = 1, size(large) = 2: the row takes 2 or 4 shares of its panel and the bar grows with it
        public string ColorBind, ColorField; // color_by(bind.field): the colour follows another value than the one the gauge shows
        public string LabelColor, Track; // label_color(...) and track(...): the label text and the empty part of a bar
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

    // Background settings shared by a screen and a panel.
    public class BgBase
    {
        public string[] BgGradient;               // bg_gradient(...) stops
        public string BgDir = "vertical";         // bg_gradient_dir: vertical or horizontal
        public List<ColorRule> BgRules;           // bg: color(bind.field < 20 : #450a0a, default: #0e1218)
        public string BgBind, BgField;
        public List<ColorRule> FlashRules;        // flash: bind.field > 90  (flashes to alert_bg)
        public string FlashBind, FlashField;
    }

    public class PanelDef : BgBase
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

    public class ScreenDef : BgBase
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
        public float BootCalibrate = 1.2f; // boot_calibrate: seconds the gauges sweep 0 to 100 to the live value after the boot (0 = off)
        public float Gap = -1f, Padding = -1f; // gap: pixels between panels; padding: pixels around the screen (-1 = the default)
        public bool GradientSteps; // gradient_mode: steps - gradients pick the nearest stop instead of blending
        public string PanelBg;    // colour of the default card behind panels of a multi-panel screen; "none" for no card (default: a faint white overlay)
        public string BgTexture;  // bg_texture: name of a built-in sprite drawn over the background
        public string BgTextureColor; // bg_texture_color: its tint (default white at low opacity)
        public string AlertBg;    // background colour flashed while the red alert is on; "none" to disable
        public string MirrorPanel; // mirror_panel: copy only this panel of the mirrored screen
        public string Mirror;     // name of another screen whose drawing is copied here, scaled
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
        public double RotateSeconds;     // rotate: seconds per page; pages share the same output and interval
        public List<ScreenDef> RotationPages;
        public ScreenDef RotationPage(double seconds)
        {
            return RotationPages == null ? this : RotationPages[(int)((Math.Max(0, seconds) / RotateSeconds) % RotationPages.Count)];
        }
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
        public List<string> Warnings = new List<string>(); // probable mistakes that are not errors
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

#if HELM_TOOLS // Not shipped in the Programmable Block script (Build-Release.ps1 removes these blocks).
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
#endif

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
                        string rawProfile = line.Substring("profile:".Length).Trim();
                        string normalizedProfile = Layout.NormalizeProfile(rawProfile);
                        if (normalizedProfile == null)
                            throw new FormatException("unknown profile '" + rawProfile + "' (expected auto/standard/compact/wide/corner or a hardware profile name)");
                        current.Profile = normalizedProfile;
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
                        BgBase bgTarget = currentPanel != null ? (BgBase)currentPanel : current;
                        if (bg.StartsWith("color(") && bg.EndsWith(")"))
                        {
                            string bgRef;
                            bgTarget.BgRules = ParseColorRules(bg.Substring(6, bg.Length - 7), out bgRef);
                            string bgBind, bgField;
                            SplitRef(bgRef, out bgBind, out bgField);
                            bgTarget.BgBind = bgBind;
                            bgTarget.BgField = bgField;
                        }
                        else if (currentPanel != null) currentPanel.Background = bg;
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
                    else if (line.StartsWith("rotate:"))
                    {
                        RequireScreen(current, "rotate:");
                        double seconds = double.Parse(line.Substring(7).Trim().TrimEnd('s'), System.Globalization.CultureInfo.InvariantCulture);
                        if (double.IsNaN(seconds) || double.IsInfinity(seconds) || seconds < 1)
                            throw new FormatException("rotate: requires at least 1 second");
                        current.RotateSeconds = seconds;
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
                    else if (line.StartsWith("boot_calibrate:"))
                    {
                        RequireScreen(current, "boot_calibrate:");
                        string cv = line.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
                        current.BootCalibrate = cv == "off" ? 0f : Math.Max(0f, float.Parse(cv, System.Globalization.CultureInfo.InvariantCulture));
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
                    else if (line.StartsWith("mirror_panel:"))
                    {
                        RequireScreen(current, "mirror_panel:");
                        current.MirrorPanel = Unquote(line.Substring("mirror_panel:".Length).Trim());
                    }
                    else if (line.StartsWith("mirror:"))
                    {
                        RequireScreen(current, "mirror:");
                        current.Mirror = Unquote(line.Substring("mirror:".Length).Trim());
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
                        (currentPanel != null ? (BgBase)currentPanel : current).BgGradient = ParseStops(line);
                    }
                    else if (line.StartsWith("bg_gradient_dir:"))
                    {
                        RequireScreen(current, "bg_gradient_dir:");
                        (currentPanel != null ? (BgBase)currentPanel : current).BgDir = line.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();
                    }
                    else if (line.StartsWith("flash:"))
                    {
                        RequireScreen(current, "flash:");
                        BgBase fTarget = currentPanel != null ? (BgBase)currentPanel : current;
                        string fExpr = line.Substring("flash:".Length).Trim();
                        string fCmp = StripLeadingFieldRef(fExpr);
                        string fBind, fField;
                        SplitRef(fExpr.Substring(0, fExpr.Length - fCmp.Length), out fBind, out fField);
                        fTarget.FlashBind = fBind;
                        fTarget.FlashField = fField;
                        fTarget.FlashRules = new List<ColorRule> { new ColorRule { Expr = fCmp, Color = "on" } };
                    }
                    else if (line.StartsWith("bg_texture:"))
                    {
                        RequireScreen(current, "bg_texture:");
                        current.BgTexture = Unquote(line.Substring("bg_texture:".Length).Trim());
                    }
                    else if (line.StartsWith("bg_texture_color:"))
                    {
                        RequireScreen(current, "bg_texture_color:");
                        current.BgTextureColor = line.Substring("bg_texture_color:".Length).Trim();
                    }
                    else if (line.StartsWith("gap:") || line.StartsWith("padding:"))
                    {
                        bool isGap = line.StartsWith("gap:");
                        RequireScreen(current, isGap ? "gap:" : "padding:");
                        float pv = float.Parse(line.Substring(line.IndexOf(':') + 1).Trim(), System.Globalization.CultureInfo.InvariantCulture);
                        if (isGap) current.Gap = pv; else current.Padding = pv;
                    }
                    else if (line.StartsWith("gradient_mode:"))
                    {
                        RequireScreen(current, "gradient_mode:");
                        current.GradientSteps = line.Substring("gradient_mode:".Length).Trim().Equals("steps", StringComparison.OrdinalIgnoreCase);
                    }
                    else if (line.StartsWith("screen_bg:"))
                    {
                        RequireScreen(current, "screen_bg:");
                        current.Background = line.Substring("screen_bg:".Length).Trim();
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
                if (sc.RotateSeconds > 0 && sc.RotationPages == null)
                {
                    var pages = cfg.Screens.FindAll(s => s.OutputSurfaceIndex == sc.OutputSurfaceIndex &&
                        string.Equals(s.OutputBlockName, sc.OutputBlockName, StringComparison.OrdinalIgnoreCase));
                    foreach (var p in pages) p.RotationPages = pages;
                    if (pages.Any(p => p.RotateSeconds != sc.RotateSeconds))
                    {
                        cfg.Errors.Add("output '" + sc.OutputBlockName + "': every rotating page must use the same rotate: interval");
                        foreach (var p in pages) { p.RotationPages = null; p.RotateSeconds = 0; }
                    }
                }
                // bg: after an @panel line colours that panel only. A screen where only some panels have one shows two colours.
                int own = sc.Panels.Count(pn => pn.Background != null || pn.BgRules != null || pn.BgGradient != null);
                if (sc.Panels.Count > 1 && own > 0 && own < sc.Panels.Count)
                    cfg.Warnings.Add("screen '" + sc.Name + "': only " + own + " of " + sc.Panels.Count + " panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
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
                    else if (kv.Length == 2 && kv[0].Trim().Equals("ttl", StringComparison.OrdinalIgnoreCase))
                    {
                        double ttl = double.Parse(kv[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                        if (ttl <= 0 || double.IsNaN(ttl) || double.IsInfinity(ttl)) throw new FormatException("ttl: requires positive seconds");
                        def.DynamicTtl = ttl;
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
            else if (func == "tank" || func == "tube" || func == "bucket") def.Kind = LineKind.Tank;
            else if (func == "dial" || func == "gauge" || func == "arc") def.Kind = LineKind.Dial;
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
                else if (mFunc == "size")
                {
                    string sz = mArgs.Trim().ToLowerInvariant();
                    def.Size = sz == "large" || sz == "fill" ? 2 : sz == "medium" ? 1 : 0;
                }
                else if (mFunc == "marker")
                {
                    def.Marks = new List<double>();
                    foreach (var mk in SplitArgs(mArgs))
                    {
                        double mv2;
                        if (!double.TryParse(mk, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out mv2)) throw new FormatException("marker() takes numbers from 0 to 100");
                        def.Marks.Add(mv2);
                    }
                }
                else if (mFunc == "icon") def.Icon = mArgs.Trim();
                else if (mFunc == "blink")
                {
                    string bx = mArgs.Trim(), bc = StripLeadingFieldRef(bx);
                    SplitRef(bx.Substring(0, bx.Length - bc.Length), out def.BlinkBind, out def.BlinkField);
                    def.BlinkRules = new List<ColorRule> { new ColorRule { Expr = bc, Color = "on" } };
                }
                else if (mFunc == "value") def.NoValue = mArgs.Trim().Equals("off", StringComparison.OrdinalIgnoreCase);
                else if (mFunc == "ticks") def.Ticks = int.Parse(mArgs.Trim());
                else if (mFunc == "segments" || mFunc == "points") def.Segs = Math.Max(2, Math.Min(40, int.Parse(mArgs.Trim())));
                else if (mFunc == "outline") def.Outline = mArgs.Trim();
                else if (mFunc == "color_by")
                {
                    var cb = mArgs.Trim().Split('.');
                    if (cb.Length != 2) throw new FormatException("color_by() takes bindname.field");
                    def.ColorBind = cb[0].Trim();
                    def.ColorField = cb[1].Trim();
                }
                else if (mFunc == "label_color") def.LabelColor = mArgs.Trim();
                else if (mFunc == "track") def.Track = mArgs.Trim();
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
                    string ignored;
                    def.ColorRules = ParseColorRules(mArgs, out ignored);
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

        // "expr : color, expr : color, default : color". fieldRef gets the "bind.field" written in front of the first
        // comparison (the caller of a text() line already knows its own bind and field, a panel background does not).
        private static List<ColorRule> ParseColorRules(string args, out string fieldRef)
        {
            var rules = new List<ColorRule>();
            fieldRef = null;
            foreach (var part in SplitArgs(args))
            {
                int colon = part.LastIndexOf(':');
                if (colon < 0) throw new FormatException("color() entries need 'expr : color'");
                string left = part.Substring(0, colon).Trim();
                string color = part.Substring(colon + 1).Trim();
                if (left.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    rules.Add(new ColorRule { IsDefault = true, Color = color });
                }
                else
                {
                    string strippedExpr = StripLeadingFieldRef(left);
                    if (fieldRef == null) fieldRef = left.Substring(0, left.Length - strippedExpr.Length).Trim();
                    rules.Add(new ColorRule { Expr = strippedExpr, Color = color });
                }
            }
            if (!rules.Any(r => r.IsDefault))
                throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
            return rules;
        }

        // "bind.field" -> bind and field.
        private static void SplitRef(string r, out string bind, out string field)
        {
            var p = (r ?? "").Split('.');
            if (p.Length != 2 || p[0].Trim().Length == 0 || p[1].Trim().Length == 0)
                throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
            bind = p[0].Trim();
            field = p[1].Trim();
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

#if HELM_TOOLS
    public class RenderedLine
    {
        public string Text;
        public string Color; // color name/string as given in the DSL; SE-side maps to a real Color
    }
#endif

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

        // Maps user-facing hardware names and DSL aliases onto the five layout profiles used by the renderer.
        // Null means the value is not a supported profile; callers parsing user input should report that as an error.
        public static string NormalizeProfile(string profile)
        {
            string p = (profile ?? "").Trim().ToLowerInvariant().Replace("_", "").Replace("-", "").Replace(" ", "");
            switch (p)
            {
                case "auto": return "auto";
                case "standard":
                case "textpanel": return "standard";
                case "compact":
                case "cockpitsmall": return "compact";
                case "wide":
                case "widelcd": return "wide";
                case "corner":
                case "cornerlcd":
                case "cockpitcorner": return "corner";
                default: return null;
            }
        }

        // Resolves auto from the actual visible drawing surface. The shortest side is what constrains
        // readable sprite text, and also handles a modded or rotated surface without a special alias.
        public static string ResolveProfile(string profile, float surfaceWidth, float surfaceHeight)
        {
            string p = NormalizeProfile(profile) ?? "standard";
            if (p != "auto") return p;
            if (surfaceWidth <= 0f || surfaceHeight <= 0f) return "standard";

            float shortSide = Math.Min(surfaceWidth, surfaceHeight);
            if (shortSide <= 160f) return "corner";
            if (shortSide <= 300f) return "compact";
            if (surfaceWidth >= 1000f) return "wide";
            return "standard";
        }

        // Computes responsive font scaling according to viewport profile and screen size.
        public static float GetProfileFontSize(string profile, float surfaceWidth, float surfaceHeight, float defaultFontSize = 1.0f)
        {
            string p = ResolveProfile(profile, surfaceWidth, surfaceHeight);
            if (p == "compact") return defaultFontSize * 0.75f;
            if (p == "corner") return defaultFontSize * 0.55f;
            if (p == "wide") return defaultFontSize * 1.15f;
            return defaultFontSize;
        }

        // A title used to reserve 20 * fontSize pixels regardless of the panel height. On a short panel
        // (especially a multi-row 128 px surface) that could consume most of the content area. Keep the
        // normal header on roomy panels, but never let it claim more than 18% of its panel.
        public static float GetPanelTitleHeaderHeight(float panelHeight, float fontSize)
        {
            if (panelHeight <= 0f || fontSize <= 0f) return 0f;
            return Math.Min(20f * fontSize, panelHeight * 0.18f);
        }

        public static float GetPanelTitleFontSize(float panelHeight, float fontSize)
        {
            float headerHeight = GetPanelTitleHeaderHeight(panelHeight, fontSize);
            return Math.Min(fontSize * 0.9f, headerHeight * 0.045f);
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
}
