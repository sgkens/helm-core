using System;
using System.Collections.Generic;
using HelmCore;

class Tests
{
    static int pass = 0, fail = 0;

    static void Check(bool cond, string label)
    {
        if (cond) { pass++; }
        else { fail++; Console.WriteLine("FAIL: " + label); }
    }

    static void Main()
    {
        string sample = @"
@bind power = battery(""Main Battery Group"")
@bind o2    = airvent(""O2 Vent"")
@bind cargo = cargo(""Cargo Containers"")
@bind fleet_power = group(""Main Batteries"", agg: avg)

@screen main
output: ""Cockpit Display"", 0
border: ascii
font_size: 1.0
width: 25
line: ""PWR"" bar(power.percent) gradient(green,yellow,red)
line: ""O2""  bar(o2.percent) flat(cyan)
line: ""CARGO"" text(cargo.percent) color(cargo.percent > 90 : red, default : white)
line: ""FLEET"" bar(fleet_power.percent) flat(green)
refresh: 100
";
        var cfg = HelmParser.Parse(sample);

        Check(cfg.Errors.Count == 0, "sample config parses with zero errors: " + string.Join(" | ", cfg.Errors));
        Check(cfg.Binds.Count == 4, "4 binds parsed, got " + cfg.Binds.Count);
        Check(cfg.Binds[0].Name == "power" && cfg.Binds[0].Kind == BindKind.Block, "first bind = power/block");
        Check(cfg.Binds[3].Kind == BindKind.Group && cfg.Binds[3].Aggregation == Agg.Avg, "group bind has agg=avg");
        Check(cfg.Screens.Count == 1, "1 screen parsed");

        var scr = cfg.Screens[0];
        Check(scr.Name == "main", "screen name = main");
        Check(scr.OutputBlockName == "Cockpit Display" && scr.OutputSurfaceIndex == 0, "output: parsed block name + surface index");
        Check(scr.Width == 25, "width parsed = 25");

        // --- missing output: is now a required-field error, not silent ---
        string noOutput = "@screen s\nline: \"X\" bar(x.percent) flat(red)\n";
        var cfgNoOut = HelmParser.Parse(noOutput);
        Check(cfgNoOut.Errors.Count == 1 && cfgNoOut.Errors[0].Contains("output"), "screen without output: produces exactly one clear error");
        Check(Math.Abs(scr.FontSize - 1.0f) < 0.001, "font_size parsed = 1.0");
        Check(scr.RefreshTicks == 100, "refresh = 100");
        Check(scr.Lines.Count == 4, "4 lines parsed, got " + scr.Lines.Count);

        var pwr = scr.Lines[0];
        Check(pwr.Kind == LineKind.Bar && pwr.Label == "PWR" && pwr.BindName == "power" && pwr.Field == "percent", "PWR line fields correct");
        Check(pwr.Gradient && pwr.GradientColors.Length == 3 && pwr.GradientColors[2] == "red", "PWR gradient stops correct");

        var o2 = scr.Lines[1];
        Check(o2.FlatColor == "cyan", "O2 flat color = cyan");

        var cargoLine = scr.Lines[2];
        Check(cargoLine.Kind == LineKind.Text && cargoLine.ColorRules.Count == 2, "CARGO line has 2 color rules");
        Check(cargoLine.ColorRules[0].Expr == "> 90" && cargoLine.ColorRules[0].Color == "red", "CARGO rule[0] = > 90 : red");
        Check(cargoLine.ColorRules[1].IsDefault && cargoLine.ColorRules[1].Color == "white", "CARGO default rule = white");

        // --- required-agg enforcement (PDR §6: no silent default) ---
        string missingAgg = "@bind x = group(\"Some Group\")\n@screen s\noutput: \"Panel\"\nline: \"X\" bar(x.percent) flat(red)\n";
        var cfg2 = HelmParser.Parse(missingAgg);
        Check(cfg2.Errors.Count == 1 && cfg2.Errors[0].Contains("agg"), "group bind WITHOUT agg produces an error, not a silent default");

        // --- color() requires default: (PDR-consistent decision) ---
        string missingDefault = "@screen s\noutput: \"Panel\"\nline: \"X\" text(x.percent) color(x.percent > 5 : red)\n";
        var cfg3 = HelmParser.Parse(missingDefault);
        Check(cfg3.Errors.Count == 1 && cfg3.Errors[0].ToLower().Contains("default"), "color() WITHOUT default: produces an error");

        // --- malformed line doesn't crash the whole parse, gets collected ---
        string malformed = "@screen s\noutput: \"Panel\"\nline: not a valid line\nline: \"OK\" bar(x.percent) flat(red)\n";
        var cfg4 = HelmParser.Parse(malformed);
        Check(cfg4.Errors.Count == 1, "one malformed line produces exactly one error, got " + cfg4.Errors.Count);
        Check(cfg4.Screens[0].Lines.Count == 1, "the OTHER, valid line still parses despite the bad one");

        // --- led and spark display functions ---
        string advancedConfig = "@screen adv\noutput: \"Screen\"\nline: \"PWR\" led(p.percent) gradient(green,red)\nline: \"HIST\" spark(p.percent) flat(cyan)\n";
        var cfgAdv = HelmParser.Parse(advancedConfig);
        Check(cfgAdv.Errors.Count == 0, "led and spark config parses cleanly");
        Check(cfgAdv.Screens[0].Lines[0].Kind == LineKind.LedBar, "first line parsed as LedBar");
        Check(cfgAdv.Screens[0].Lines[1].Kind == LineKind.Spark, "second line parsed as Spark");

        // --- aggregation math ---
        var vals = new List<double> { 10, 20, 30, 40 };
        Check(Aggregator.Aggregate(Agg.Avg, vals) == 25, "avg([10,20,30,40]) == 25");
        Check(Aggregator.Aggregate(Agg.Sum, vals) == 100, "sum == 100");
        Check(Aggregator.Aggregate(Agg.Min, vals) == 10, "min == 10");
        Check(Aggregator.Aggregate(Agg.Max, vals) == 40, "max == 40");
        bool threw = false;
        try { Aggregator.Aggregate(Agg.Avg, new List<double>()); } catch (InvalidOperationException) { threw = true; }
        Check(threw, "aggregating an empty set throws instead of silently returning 0 (avoids a fake reading)");

        // --- layout: bar rendering ---
        Check(Layout.RenderBarText("PWR", 50, 10) == "PWR [#####-----] 50%", "50% bar renders half-filled");
        Check(Layout.RenderBarText("PWR", 0, 10) == "PWR [----------] 0%", "0% bar renders empty");
        Check(Layout.RenderBarText("PWR", 100, 10) == "PWR [##########] 100%", "100% bar renders full");
        Check(Layout.RenderBarText("PWR", 137, 10) == "PWR [##########] 100%", "out-of-range percent (137) clamps to 100, doesn't overflow the bar");
        Check(Layout.RenderBarText("PWR", -5, 10) == "PWR [----------] 0%", "negative percent clamps to 0");

        // --- gradient stop selection ---
        Check(Layout.GradientColorAt(new[] { "green", "yellow", "red" }, 10) == "green", "10% -> green stop");
        Check(Layout.GradientColorAt(new[] { "green", "yellow", "red" }, 50) == "yellow", "50% -> yellow stop");
        Check(Layout.GradientColorAt(new[] { "green", "yellow", "red" }, 99) == "red", "99% -> red stop");
        Check(Layout.GradientColorAt(new[] { "green", "yellow", "red" }, 100) == "red", "100% (edge case) doesn't index out of bounds");

        // --- conditional color resolution ---
        var rules = cargoLine.ColorRules;
        Check(Layout.ResolveConditionalColor(rules, 95) == "red", "95 > 90 -> red");
        Check(Layout.ResolveConditionalColor(rules, 50) == "white", "50 falls through to default -> white");

        // --- bordered window ---
        var border = Layout.RenderBorderedWindow(new List<string> { "PWR [#####-----] 50%", "O2  [##########] 100%" }, 20);
        Check(border.Count == 4, "border produces top + 2 content + bottom = 4 lines");
        Check(border[0] == "+----------------------+", "top border width matches content width + padding");
        Check(border[1].StartsWith("| PWR") && border[1].EndsWith(" |"), "content line wrapped correctly");
        var longLine = Layout.RenderBorderedWindow(new List<string> { "this line is way longer than the configured width" }, 10);
        Check(longLine[1].Length == 14, "an over-length content line gets truncated to width, not left to overflow the border");

        // --- Epic 2: Hex & RGB Color Science ---
        var cHex6 = HelmColor.Parse("#FF8000");
        Check(cHex6.R == 255 && cHex6.G == 128 && cHex6.B == 0 && !cHex6.IsTransparent, "parse #FF8000 (hex 6-digit)");
        var cHex3 = HelmColor.Parse("#0FC");
        Check(cHex3.R == 0 && cHex3.G == 255 && cHex3.B == 204, "parse #0FC (hex 3-digit)");
        var cRgb = HelmColor.Parse("12, 34, 56");
        Check(cRgb.R == 12 && cRgb.G == 34 && cRgb.B == 56, "parse RGB comma triples");
        var cTrans = HelmColor.Parse("transparent");
        Check(cTrans.IsTransparent, "parse transparent");
        var cNone = HelmColor.Parse("none");
        Check(cNone.IsTransparent, "parse none");

        // --- Epic 2 & 3: Visual QoL & Grid Panel Layout ---
        string gridSample = @"
@bind pwr = battery(""Main Battery"")
@screen cockpit
output: ""Cockpit LCD"", 0
font: Debug
bg: #0a0d14
border_color: #3b82f6
layout: grid(2, 2)

@panel sys at(0, 0, 1, 1) title: ""SYSTEMS""
line: ""PWR"" bar(pwr.percent) flat(#22c55e)

@panel nav at(1, 0, 1, 2) title(""NAVIGATION"")
font: Monospace
bg: #111827
line: ""NAV"" text(pwr.percent) color(pwr.percent > 50 : #38bdf8, default : #94a3b8)
";
        var cfgGrid = HelmParser.Parse(gridSample);
        Check(cfgGrid.Errors.Count == 0, "grid layout config parses without errors: " + string.Join(" | ", cfgGrid.Errors));
        var scrCockpit = cfgGrid.Screens[0];
        Check(scrCockpit.Font == "Debug", "screen font parsed = Debug");
        Check(scrCockpit.Background == "#0a0d14", "screen bg parsed = #0a0d14");
        Check(scrCockpit.BorderColor == "#3b82f6", "screen border_color parsed = #3b82f6");
        Check(scrCockpit.GridCols == 2 && scrCockpit.GridRows == 2, "layout: grid(2, 2) parsed cols and rows");
        Check(scrCockpit.Panels.Count == 2, "screen has 2 panels");

        var p0 = scrCockpit.Panels[0];
        Check(p0.Name == "sys" && p0.Col == 0 && p0.Row == 0 && p0.SpanX == 1 && p0.SpanY == 1, "panel 0 positioning (0,0,1,1)");
        Check(p0.Title == "SYSTEMS", "panel 0 title = SYSTEMS");
        Check(p0.Lines.Count == 1 && p0.Lines[0].FlatColor == "#22c55e", "panel 0 line and hex flat color");

        var p1 = scrCockpit.Panels[1];
        Check(p1.Name == "nav" && p1.Col == 1 && p1.Row == 0 && p1.SpanX == 1 && p1.SpanY == 2, "panel 1 positioning (1,0,1,2)");
        Check(p1.Title == "NAVIGATION", "panel 1 title = NAVIGATION");
        Check(p1.Font == "Monospace", "panel 1 font override = Monospace");
        Check(p1.Background == "#111827", "panel 1 bg override = #111827");

        // --- Epic 3: Grid Math Calculations ---
        // 500x500 surface, 10px pad, 10px gutter, 2x2 grid
        var rect00 = Layout.ComputePanelRect(0, 0, 1, 1, 2, 2, 500, 500, 10, 10, 10, 10);
        Check(Math.Abs(rect00.X - 10f) < 0.1f && Math.Abs(rect00.Y - 10f) < 0.1f, "rect (0,0) position at (10, 10)");
        Check(Math.Abs(rect00.Width - 235f) < 0.1f && Math.Abs(rect00.Height - 235f) < 0.1f, "rect (0,0) size 235x235");

        // Span full height on right column: (1, 0, 1, 2)
        var rectRightSpan = Layout.ComputePanelRect(1, 0, 1, 2, 2, 2, 500, 500, 10, 10, 10, 10);
        Check(Math.Abs(rectRightSpan.X - 255f) < 0.1f && Math.Abs(rectRightSpan.Y - 10f) < 0.1f, "right span rect X=255, Y=10");
        Check(Math.Abs(rectRightSpan.Width - 235f) < 0.1f && Math.Abs(rectRightSpan.Height - 480f) < 0.1f, "right span rect height spans full height (480)");

        // --- Epic 1: Custom Template Definition & Instantiation ---
        string templateSample = @"
@template pod_monitor($screen, $tank_group, $pwr_group)
@bind pwr_$screen = group(""$pwr_group"", agg: avg)
@bind o2_$screen  = group(""$tank_group"", agg: avg)
@screen $screen
output: ""$screen LCD"", 0
line: ""PWR"" bar(pwr_$screen.percent) flat(green)
line: ""O2""  bar(o2_$screen.percent) flat(cyan)
@end

@use pod_monitor(screen=""Pod Alpha"", tank_group=""Alpha Tanks"", pwr_group=""Alpha Batts"")
@use pod_monitor(screen=""Pod Beta"", tank_group=""Beta Tanks"", pwr_group=""Beta Batts"")
";
        var cfgTmpl = HelmParser.Parse(templateSample);
        Check(cfgTmpl.Errors.Count == 0, "template definition and multiple @use parse with zero errors: " + string.Join(" | ", cfgTmpl.Errors));
        Check(cfgTmpl.Binds.Count == 4, "4 binds generated from 2 template instances");
        Check(cfgTmpl.Screens.Count == 2, "2 screens generated from 2 template instances");
        Check(cfgTmpl.Screens[0].Name == "Pod Alpha", "screen 0 name = Pod Alpha");
        Check(cfgTmpl.Screens[0].OutputBlockName == "Pod Alpha LCD", "screen 0 output = Pod Alpha LCD");
        Check(cfgTmpl.Screens[1].Name == "Pod Beta", "screen 1 name = Pod Beta");
        Check(cfgTmpl.Screens[1].OutputBlockName == "Pod Beta LCD", "screen 1 output = Pod Beta LCD");

        // --- boot_duration units: "s" is seconds, a bare number is ticks, 0 disables ---
        Check(Math.Abs(HelmParser.Parse("@screen a\nboot_duration: 180").Screens[0].BootDuration - 3.0f) < 0.01f, "boot_duration: 180 is 180 ticks = 3 s");
        Check(Math.Abs(HelmParser.Parse("@screen a\nboot_duration: 2s").Screens[0].BootDuration - 2.0f) < 0.01f, "boot_duration: 2s is 2 s");
        Check(HelmParser.Parse("@screen a\nboot_duration: 0").Screens[0].BootDuration == 0f, "boot_duration: 0 disables the boot");

        Check(HelmParser.Parse("@screen a\nboot_batch: 4").Screens[0].BootBatch == 4, "boot_batch: 4 parsed");
        Check(HelmParser.Parse("@screen a").Screens[0].BootBatch == 1, "boot_batch defaults to 1");
        Check(HelmParser.Parse("@screen a\nboot_batch: 0").Screens[0].BootBatch == 1, "boot_batch below 1 is clamped to 1");

        // --- gradient_mode ---
        Check(!HelmParser.Parse("@screen a").Screens[0].GradientSteps, "gradients blend by default");
        Check(HelmParser.Parse("@screen a\ngradient_mode: steps").Screens[0].GradientSteps && !HelmParser.Parse("@screen a\ngradient_mode: smooth").Screens[0].GradientSteps, "gradient_mode: steps selects the nearest stop, smooth blends");

        // --- Value-driven backgrounds, flash, horizontal gradients and textures ---
        var cfgVb = HelmParser.Parse("@screen a\noutput: \"x\", 0\nbg: color(b.percent < 20 : #450a0a, default: #0e1218)\nflash: b.percent < 5\nbg_texture: Grid\nbg_texture_color: #38bdf8\nbg_gradient_dir: horizontal\n@panel p at(0, 0, 1, 1) title: \"T\"\nbg: color(c.output > 90 : red, c.output > 50 : yellow, default: green)\nbg_gradient(#000, #fff)\nbg_gradient_dir: vertical\nflash: c.percent > 90\n");
        Check(cfgVb.Errors.Count == 0, "value backgrounds parse without errors: " + string.Join(" | ", cfgVb.Errors));
        var scVb = cfgVb.Screens[0]; var pnVb = scVb.Panels[0];
        Check(scVb.BgRules.Count == 2 && scVb.BgBind == "b" && scVb.BgField == "percent" && scVb.BgRules[0].Expr == "< 20" && scVb.BgRules[1].IsDefault, "screen bg: color(...) keeps its rules and the bind.field written before the comparison");
        Check(scVb.FlashBind == "b" && scVb.FlashField == "percent" && scVb.FlashRules[0].Expr == "< 5", "screen flash: b.percent < 5 parsed");
        Check(scVb.BgTexture == "Grid" && scVb.BgTextureColor == "#38bdf8" && scVb.BgDir == "horizontal", "bg_texture, bg_texture_color and bg_gradient_dir parsed on the screen");
        Check(pnVb.BgRules.Count == 3 && pnVb.BgBind == "c" && pnVb.BgField == "output" && pnVb.BgGradient.Length == 2 && pnVb.BgDir == "vertical", "after @panel the same directives apply to the panel, not the screen");
        Check(pnVb.FlashBind == "c" && scVb.Background == null && pnVb.Background == null, "panel flash parsed; bg: color(...) does not set a solid colour");
        Check(HelmParser.Parse("@screen a\noutput: \"x\", 0\nflash: percent < 5\n").Errors.Count == 1, "flash: without a bind.field is reported as an error");

        // --- scope: all and max(bind.field) ---
        var cfgScope = HelmParser.Parse("@bind a = group(\"G\", agg: sum, scope: all)\n@bind b = group(\"G\", agg: sum)\n@bind c = block(\"B\", scope: all)\n@screen s\noutput: \"x\", 0\n@panel p at(0, 0, 1, 1) title: \"T\"\nline: \"A\" bar(a.ammo:X) max(b.ammo:X) flat(cyan)\nline: \"B\" bar(a.percent) max(1.5) flat(cyan)\n");
        Check(cfgScope.Errors.Count == 0, "scope and max(bind.field) parse without errors: " + string.Join(" | ", cfgScope.Errors));
        Check(cfgScope.Binds[0].AllShips && !cfgScope.Binds[1].AllShips && cfgScope.Binds[2].AllShips, "scope: all sets AllShips on a group and a block bind, and is off by default");
        var linesM = cfgScope.Screens[0].Panels[0].Lines;
        Check(linesM[0].MaxBind == "b" && linesM[0].MaxField == "ammo:X" && linesM[0].Max == 0 && linesM[0].HasMax, "max(b.ammo:X) reads the maximum from a block field");
        Check(linesM[1].Max == 1.5 && linesM[1].MaxBind == null && linesM[1].HasMax, "max(1.5) is still a number");
        Check(!HelmParser.Parse("@screen s\noutput: \"x\", 0\n@panel p at(0, 0, 1, 1) title: \"T\"\nline: \"A\" bar(a.b) max(a.b.c) flat(cyan)\n").Errors.Count.Equals(0), "max(a.b.c) is reported as an error");

        // --- refresh: and alert_period: ---
        Check(HelmParser.Parse("@screen a").Screens[0].RefreshTicks == 0, "refresh defaults to 0 (draw every run)");
        Check(HelmParser.Parse("@screen a\nrefresh: 600").Screens[0].RefreshTicks == 600, "refresh: 600 parsed");
        Check(Math.Abs(HelmParser.Parse("@screen a").Screens[0].AlertPeriod - 1.6f) < 0.001f, "alert_period defaults to 1.6 s");
        Check(Math.Abs(HelmParser.Parse("@screen a\nalert_period: 0.8s").Screens[0].AlertPeriod - 0.8f) < 0.001f, "alert_period: 0.8s parsed");

        // --- Backgrounds ---
        var cfgBg = HelmParser.Parse("@screen a\noutput: \"x\", 0\nbg_gradient(#000000, #ffffff)\npanel_bg: none\nalert_bg: #7f1d1d\n@panel p at(0, 0, 1, 1) title: \"T\"\nbg: #123456\n");
        Check(cfgBg.Errors.Count == 0, "background directives parse without errors: " + string.Join(" | ", cfgBg.Errors));
        Check(cfgBg.Screens[0].BgGradient != null && cfgBg.Screens[0].BgGradient.Length == 2 && cfgBg.Screens[0].BgGradient[1] == "#ffffff", "bg_gradient(...) parsed into two stops");
        Check(cfgBg.Screens[0].PanelBg == "none" && cfgBg.Screens[0].AlertBg == "#7f1d1d", "panel_bg: and alert_bg: parsed");
        Check(cfgBg.Screens[0].Panels[0].Background == "#123456" && cfgBg.Screens[0].Background == null, "bg: after @panel sets the panel background, not the screen's");
        Check(HelmParser.Parse("@screen a\nbg_gradient: red, blue, green\n").Screens[0].BgGradient.Length == 3, "bg_gradient: with a colon parses three stops");

        // --- max(N): item amounts as a percentage ---
        var cfgMax = HelmParser.Parse("@bind g = group(\"Guns\", agg: sum)\n@screen a\noutput: \"x\", 0\n@panel p at(0, 0, 1, 1) title: \"T\"\nline: \"AMMO\" bar(g.ammo:NATO_25x184mm) max(1200) flat(cyan)\nline: \"PLAIN\" bar(g.percent) flat(cyan)\n");
        Check(cfgMax.Errors.Count == 0, "max(N) parses without errors: " + string.Join(" | ", cfgMax.Errors));
        Check(cfgMax.Screens[0].Panels[0].Lines[0].Max == 1200.0 && cfgMax.Screens[0].Panels[0].Lines[0].Field == "ammo:NATO_25x184mm", "max(1200) and the item field are parsed");
        Check(cfgMax.Screens[0].Panels[0].Lines[1].Max == 0.0, "a line without max() has no maximum");

        // --- Banners: raw ASCII art blocks ---
        string bannerDsl = "@banner logo\n\n        ZZ   # \"q\"\n     ZZ ZZ\n\n@end\n@screen a\noutput: \"x\", 0\nbanner: logo\nboot_logo: logo\nbanner_color: red\n";
        var cfgBan = HelmParser.Parse(bannerDsl);
        Check(cfgBan.Errors.Count == 0, "banner parses without errors: " + string.Join(" | ", cfgBan.Errors));
        Check(cfgBan.Banners.ContainsKey("logo") && cfgBan.Banners["logo"].Count == 2, "banner keeps 2 lines and drops the blank lines around it");
        Check(cfgBan.Banners["logo"][0] == "   ZZ   # \"q\"", "banner keeps '#', quotes and inner spaces, and removes the common left indent: '" + cfgBan.Banners["logo"][0] + "'");
        Check(cfgBan.Banners["logo"][1] == "ZZ ZZ", "banner second line: '" + cfgBan.Banners["logo"][1] + "'");
        Check(cfgBan.Screens[0].Banner == "logo" && cfgBan.Screens[0].BootLogo == "logo", "banner: and boot_logo: parsed");
        Check(cfgBan.Screens[0].BootColor == "red", "banner_color: sets the logo colour");
        var cfgBan2 = HelmParser.Parse("@screen a\noutput: \"x\", 0\nbanner: nope\nboot_logo: nada\nbanner_gradient(cyan, blue)\n");
        Check(cfgBan2.Errors.Count == 2 && cfgBan2.Errors[0].Contains("'nope' is not defined"), "undefined banner and boot_logo are reported: " + string.Join(" | ", cfgBan2.Errors));
        Check(cfgBan2.Screens[0].BootGradientColors != null && cfgBan2.Screens[0].BootGradientColors.Length == 2, "banner_gradient(...) sets the logo gradient");

        // --- Boot log reveal ---
        Check(Layout.BootLogCount(0.0, 5) == 1, "boot: first log shown as soon as the boot starts");
        Check(Layout.BootLogCount(0.5, 5) == 3, "boot: half way shows 3 of 5 logs");
        Check(Layout.BootLogCount(0.999, 5) == 5, "boot: all logs shown at the end");
        Check(Layout.BootLogCount(2.0, 5) == 5, "boot: never more logs than exist");
        Check(Layout.BootLogCount(-1.0, 5) == 1, "boot: negative progress is clamped");
        Check(Layout.BootLogCount(0.5, 0) == 0, "boot: no logs, none shown");

        // --- Nested templates: a template body may call another template ---
        string nestedSample = @"
@template leaf($screen)
@screen $screen
output: ""$screen LCD"", 0
@end
@template pair($side)
@use leaf(screen=""$side Fore"")
@use leaf(screen=""$side Aft"")
@end
@use pair(side=""Port"")
@use pair(side=""Starboard"")
";
        var cfgNested = HelmParser.Parse(nestedSample);
        Check(cfgNested.Errors.Count == 0, "nested @use parses with zero errors: " + string.Join(" | ", cfgNested.Errors));
        Check(cfgNested.Screens.Count == 4, "nested @use: 2 pairs x 2 leaves = 4 screens");
        Check(cfgNested.Screens.Count == 4 && cfgNested.Screens[3].Name == "Starboard Aft", "nested @use: last screen = Starboard Aft");
        var cfgLoop = HelmParser.Parse("@template a($x)\n@use a(x=\"$x\")\n@end\n@use a(x=\"1\")\n");
        Check(cfgLoop.Errors.Count == 1 && cfgLoop.Errors[0].Contains("nested too deep"), "self-calling template stops with one 'nested too deep' error");

        // --- Epic 1: Predefined Built-In Template Gallery ---
        string gallerySample = @"
@use fighter_hud(screen=""Fighter Cockpit"", pwr=""Fighter Batteries"", h2=""Hydrogen Tanks"")
";
        var cfgGallery = HelmParser.Parse(gallerySample);
        Check(cfgGallery.Errors.Count == 0, "predefined gallery template fighter_hud parses cleanly: " + string.Join(" | ", cfgGallery.Errors));
        Check(cfgGallery.Screens.Count == 1, "1 screen created from fighter_hud");
        var fighterScreen = cfgGallery.Screens[0];
        Check(fighterScreen.Name == "Fighter Cockpit", "screen name = Fighter Cockpit");
        Check(fighterScreen.Panels.Count == 2, "fighter_hud generated 2 panels (sys and hold)");
        Check(fighterScreen.Panels[0].Title == "PROPULSION", "panel 0 title = PROPULSION");
        Check(fighterScreen.Panels[1].Title == "STORES", "panel 1 title = STORES");

        // --- Epic 4 & 5: Patterns, Gauges, Multi-target Group Binds ---
        string advPatterns = @"
@bind power_grid = group(""Main Batteries"", ""Aux Reactors"", agg: avg)
@screen patterns_demo
output: ""Bridge LCD"", 0
line: ""HAZ""  hazard(power_grid.percent) flat(yellow)
line: ""CHEV"" chevron(power_grid.percent) flat(cyan)
line: ""DOTS"" dots(power_grid.percent) flat(lime)
line: ""MOD""  bar(power_grid.percent) pattern(hazard) gradient(green, red)
";
        var cfgPat = HelmParser.Parse(advPatterns);
        Check(cfgPat.Errors.Count == 0, "patterns demo parses without errors: " + string.Join(" | ", cfgPat.Errors));
        Check(cfgPat.Binds.Count == 1, "1 multi-target bind parsed");
        Check(cfgPat.Binds[0].TargetNames.Count == 2, "bind has 2 targets: Batteries and Reactors");
        Check(cfgPat.Binds[0].TargetNames[0] == "Main Batteries" && cfgPat.Binds[0].TargetNames[1] == "Aux Reactors", "bind targets correct");
        Check(cfgPat.Binds[0].Aggregation == Agg.Avg, "bind agg is Avg");

        var scrPat = cfgPat.Screens[0];
        Check(scrPat.Lines.Count == 4, "4 pattern lines parsed");
        Check(scrPat.Lines[0].Kind == LineKind.Hazard, "first line is Hazard");
        Check(scrPat.Lines[1].Kind == LineKind.Chevron, "second line is Chevron");
        Check(scrPat.Lines[2].Kind == LineKind.Dots, "third line is Dots");
        Check(scrPat.Lines[3].Kind == LineKind.Hazard && scrPat.Lines[3].Gradient, "fourth line is bar with pattern(hazard) and gradient");

        // --- Epic 7: Airlock & Rover Templates ---
        string newTemplates = @"
@use airlock(screen=""Airlock Status"", vent=""Air Vent 1"", outer=""Hangar Door Outer"", inner=""Hangar Door Inner"")
@use rover(screen=""Explorer Rover"", power=""Rover Batts"", susp=""Wheel Suspensions"", cargo=""Ore Container"")
";
        var cfgNewTmpl = HelmParser.Parse(newTemplates);
        Check(cfgNewTmpl.Errors.Count == 0, "airlock and rover templates parse cleanly: " + string.Join(" | ", cfgNewTmpl.Errors));
        Check(cfgNewTmpl.Screens.Count == 2, "2 screens generated for airlock and rover");
        Check(cfgNewTmpl.Screens[0].Name == "Airlock Status", "screen 0 is Airlock Status");
        Check(cfgNewTmpl.Screens[0].Panels.Count == 2, "airlock has 2 panels (env, doors)");
        Check(cfgNewTmpl.Screens[1].Name == "Explorer Rover", "screen 1 is Explorer Rover");
        Check(cfgNewTmpl.Screens[1].Panels.Count == 3, "rover has 3 panels (pwr, susp, cargo)");

        // --- Epic 9, 10, 11: Monoliths, Directional Fill, Boot Sequence & Cargo Overview ---
        string sprint6Dsl = @"
@use cargo_overview(screen=""Station Cargo"", crg=""Main Stores"", ore=""Ore Silo"", ingot=""Ingot Vault"")
@use battery_monolith(screen=""Power Core LCD"", batt=""Main Power Array"")

@screen custom_boot_test
output: ""Cockpit Display"", 0
boot_msg: ""HELM AEROSPACE BOOT V1""
boot_stagger: 15
@panel vert_panel at(0, 0, 1, 1) title: ""VERTICAL CORE"" orientation: vertical
line: ""FLUID"" bar(power_grid.percent) dir: up
";
        var cfgSprint6 = HelmParser.Parse(sprint6Dsl);
        Check(cfgSprint6.Errors.Count == 0, "sprint 6 templates parse cleanly: " + string.Join(" | ", cfgSprint6.Errors));
        Check(cfgSprint6.Screens.Count == 3, "3 screens generated (cargo, battery_monolith, custom_boot)");
        Check(cfgSprint6.Screens[0].Name == "Station Cargo", "screen 0 is Station Cargo");
        Check(cfgSprint6.Screens[0].Panels.Count == 4, "cargo_overview has 4 panels");
        Check(cfgSprint6.Screens[1].Name == "Power Core LCD", "screen 1 is Power Core LCD");
        Check(cfgSprint6.Screens[1].Panels[0].Orientation == "vertical", "battery monolith panel is vertical");
        Check(cfgSprint6.Screens[1].Panels[0].Lines[0].Direction == "up", "monolith energy bar has dir: up");

        Check(cfgSprint6.Screens[2].BootMessage == "HELM AEROSPACE BOOT V1", "custom boot_msg parsed");
        Check(cfgSprint6.Screens[2].BootStaggerTicks == 15, "custom boot_stagger parsed (15 ticks)");
        Check(cfgSprint6.Screens[2].Panels[0].Orientation == "vertical", "vert_panel orientation is vertical");
        Check(cfgSprint6.Screens[2].Panels[0].Lines[0].Direction == "up", "vert_panel line direction is up");

        // Verify default boot logs on screens without custom logs
        var defLogs = cfgSprint6.Screens[0].GetEffectiveBootLogs();
        Check(defLogs.Count == 5, "default boot logs count = 5");
        Check(defLogs[0] == "helm-core initializing...", "default boot log 1");
        Check(defLogs[1] == "seeking compliant surface displays...", "default boot log 2");
        Check(defLogs[2] == "uploading firmware...", "default boot log 3");
        Check(defLogs[3] == "awaiting compliance...", "default boot log 4");
        Check(defLogs[4] == "helm-core online -- BOOT // SUCCESS", "default boot log 5");

        // Custom boot duration, gradient, and custom trailing logs
        string customBootDsl = @"
@screen military_bridge
output: ""Command LCD"", 0
boot_duration: 3.5s
boot_stagger: 20
boot_color: lime
boot_gradient(cyan, blue, magenta)
boot_log: ""[CLASSIFIED] initializing secure firmware bus...""
boot_log: ""handshake with main reactor telemetry...""
boot_log: ""MIL-SPEC HUD ONLINE""
line: ""MAIN"" bar(power_grid.percent) flat(cyan)
";
        var cfgMil = HelmParser.Parse(customBootDsl);
        Check(cfgMil.Errors.Count == 0, "custom boot dsl parsed cleanly: " + string.Join(" | ", cfgMil.Errors));
        var milScreen = cfgMil.Screens[0];
        Check(Math.Abs(milScreen.BootDuration - 3.5f) < 0.01f, "boot_duration parsed = 3.5s");
        Check(milScreen.BootStaggerTicks == 20, "boot_stagger parsed = 20");
        Check(milScreen.BootColor == "lime", "boot_color parsed = lime");
        Check(milScreen.BootGradientColors != null && milScreen.BootGradientColors.Length == 3, "boot_gradient parsed with 3 colors");
        Check(milScreen.BootGradientColors[0] == "cyan" && milScreen.BootGradientColors[2] == "magenta", "boot_gradient colors match");
        Check(milScreen.BootLogs.Count == 3, "3 custom boot logs parsed");
        Check(milScreen.BootLogs[0] == "[CLASSIFIED] initializing secure firmware bus...", "custom boot log 1");

        // ScreenStats & ComputeStats test
        var statsMil = Layout.ComputeStats(milScreen);
        Check(statsMil.LineCount == 1, "stats LineCount == 1");
        Check(statsMil.PanelCount == 1, "stats PanelCount == 1");
        Check(statsMil.EstimatedGcAllocBytes == 0.0, "stats 0-byte GC alloc");
        Check(statsMil.TotalAllocatedSprites > 0, "stats TotalAllocatedSprites > 0");
        Check(statsMil.MutatedPerTick == 1, "stats MutatedPerTick == 1 for single bar");
        Check(statsMil.EstInstructionCountNormal > 0 && statsMil.EstInstructionCountNormal < 100, "stats normal instruction count realistic (< 100)");
        Check(statsMil.EstInstructionCountRedraw > 0 && statsMil.EstInstructionCountRedraw < 500, "stats redraw instruction count realistic (< 500)");

        // GradientLerp continuous RGB interpolation tests
        string[] testStops = new[] { "red", "yellow", "green" };
        var c0 = Layout.GradientLerp(testStops, 0.0);
        Check(c0.R == 255 && c0.G == 0 && c0.B == 0, "GradientLerp 0% is pure red");
        var c50 = Layout.GradientLerp(testStops, 50.0);
        Check(c50.R == 255 && c50.G == 255 && c50.B == 0, "GradientLerp 50% is pure yellow");
        var c100 = Layout.GradientLerp(testStops, 100.0);
        Check(c100.R == 0 && c100.G == 255 && c100.B == 0, "GradientLerp 100% is pure green");
        var c25 = Layout.GradientLerp(testStops, 25.0);
        Check(c25.R == 255 && c25.G == 128 && c25.B == 0, "GradientLerp 25% is intermediate orange (RGB lerp)");

        // LineKind.Balance and RenderBalanceText tests
        string balanceDsl = @"
@screen gyro_balance
output: ""Cockpit LCD"", 0
line: ""ROLL"" balance(gyro.roll) flat(cyan)
line: ""PITCH"" bar(gyro.pitch) pattern(balance) flat(yellow)
";
        var cfgBal = HelmParser.Parse(balanceDsl);
        Check(cfgBal.Errors.Count == 0, "balance dsl parsed cleanly");
        Check(cfgBal.Screens[0].Lines[0].Kind == LineKind.Balance, "line 1 Kind == LineKind.Balance (balance func)");
        Check(cfgBal.Screens[0].Lines[1].Kind == LineKind.Balance, "line 2 Kind == LineKind.Balance (pattern balance)");

        string balZero = Layout.RenderBalanceText("ROLL", 50.0, 5);
        Check(balZero == "ROLL [-----|-----] 50%", "RenderBalanceText at 50% is dead center: " + balZero);
        string balLeft = Layout.RenderBalanceText("ROLL", 20.0, 5);
        Check(balLeft == "ROLL [--<<<|-----] 20%", "RenderBalanceText at 20% fills left: " + balLeft);
        string balRight = Layout.RenderBalanceText("ROLL", 80.0, 5);
        Check(balRight == "ROLL [-----|>>>--] 80%", "RenderBalanceText at 80% fills right: " + balRight);

        var statsBal = Layout.ComputeStats(cfgBal.Screens[0]);
        Check(statsBal.LineCount == 2, "balance stats LineCount == 2");
        Check(statsBal.StaticTrackSprites >= 4, "balance stats has static track + center dividers");
        Check(statsBal.DynamicFillSprites >= 2, "balance stats has dynamic fill quads");

        // Advanced Visual Controls: Toggle, Checkbox, Status, Ammo, Flow
        string advancedControlsDsl = @"
@screen bridge_controls
output: ""Bridge LCD"", 0
line: ""RADAR"" toggle(radar.enabled) flat(cyan)
line: ""DAMP "" checkbox(damp.enabled) flat(green)
line: ""BEAC "" status(beacon.status) flat(lime)
line: ""GATL "" ammo(turret.ammo) flat(yellow)
line: ""PWRFL"" flow(reactor.output) flat(orange)
";
        var cfgCtrl = HelmParser.Parse(advancedControlsDsl);
        Check(cfgCtrl.Errors.Count == 0, "advanced controls dsl parsed cleanly: " + string.Join(" | ", cfgCtrl.Errors));
        var ctrlLines = cfgCtrl.Screens[0].Lines;
        Check(ctrlLines[0].Kind == LineKind.Toggle, "line 0 Kind == LineKind.Toggle");
        Check(ctrlLines[1].Kind == LineKind.Checkbox, "line 1 Kind == LineKind.Checkbox");
        Check(ctrlLines[2].Kind == LineKind.Status, "line 2 Kind == LineKind.Status");
        Check(ctrlLines[3].Kind == LineKind.Ammo, "line 3 Kind == LineKind.Ammo");
        Check(ctrlLines[4].Kind == LineKind.Flow, "line 4 Kind == LineKind.Flow");

        // Test ASCII renderers
        string togOn = Layout.RenderToggleText("RADAR", 100.0);
        Check(togOn.Contains("[====O] ON"), "RenderToggleText ON: " + togOn);
        string togOff = Layout.RenderToggleText("RADAR", 0.0);
        Check(togOff.Contains("[O====] OFF"), "RenderToggleText OFF: " + togOff);

        string chkActive = Layout.RenderCheckboxText("DAMP", 100.0);
        Check(chkActive.Contains("[X] ACTIVE"), "RenderCheckboxText ACTIVE: " + chkActive);
        string chkReady = Layout.RenderCheckboxText("DAMP", 0.0);
        Check(chkReady.Contains("[ ] READY"), "RenderCheckboxText READY: " + chkReady);

        string stOnline = Layout.RenderStatusText("BEAC", 90.0);
        Check(stOnline.Contains("[ ONLINE  ]"), "RenderStatusText ONLINE: " + stOnline);
        string stStby = Layout.RenderStatusText("BEAC", 50.0);
        Check(stStby.Contains("[ STANDBY ]"), "RenderStatusText STANDBY: " + stStby);
        string stOff = Layout.RenderStatusText("BEAC", 10.0);
        Check(stOff.Contains("[ OFFLINE ]"), "RenderStatusText OFFLINE: " + stOff);

        string ammoText = Layout.RenderAmmoText("GATL", 50.0, 8);
        Check(ammoText.Contains("[||||....] 50%"), "RenderAmmoText 50%: " + ammoText);

        string flowChg = Layout.RenderFlowText("PWRFL", 80.0, 0);
        Check(flowChg.Contains("▲ CHG 80%"), "RenderFlowText charging: " + flowChg);
        string flowDsc = Layout.RenderFlowText("PWRFL", 20.0, 1);
        Check(flowDsc.Contains("▼ DSC 20%"), "RenderFlowText discharging: " + flowDsc);
        string flowStby = Layout.RenderFlowText("PWRFL", 50.0, 2);
        Check(flowStby.Contains("● STBY 50%"), "RenderFlowText standby: " + flowStby);

        var statsCtrl = Layout.ComputeStats(cfgCtrl.Screens[0]);
        Check(statsCtrl.LineCount == 5, "advanced controls line count == 5");
        Check(statsCtrl.TotalAllocatedSprites >= 18, "advanced controls allocated sprites >= 18");
        Check(statsCtrl.MutatedPerTick >= 14, "advanced controls mutated per tick >= 14");

        // Test MatchWildcard
        Check(Layout.MatchWildcard("Battery*", "Battery Large 1"), "MatchWildcard prefix Battery*");
        Check(Layout.MatchWildcard("*Hydrogen*", "Fighter Hydrogen Tank"), "MatchWildcard substring *Hydrogen*");
        Check(Layout.MatchWildcard("Tank ?", "Tank 1"), "MatchWildcard single char Tank ?");
        Check(!Layout.MatchWildcard("Tank ?", "Tank 10"), "MatchWildcard single char failure Tank 10");
        Check(Layout.MatchWildcard("Reactor", "reactor"), "MatchWildcard case insensitive exact match");
        Check(!Layout.MatchWildcard("Battery*", "Large Reactor"), "MatchWildcard negative match");

        // Epic 16: Engineering Units and Formatting
        string dslUnits = @"
@screen telemetry
profile: compact
line: ""POWER"" bar(batt.current) unit(""MW"") format(""F2"")
line: ""HYDRO"" bar(tank.raw) unit(""kL"")
line: ""MASS "" text(cargo.val) unit: ""t"" format: ""F1""
line: ""SPEED"" text(ship.speed) unit(""auto"")
";
        var cfgUnits = HelmParser.Parse(dslUnits);
        Check(cfgUnits.Screens[0].Profile == "compact", "screen profile parsed as compact");
        var unitLines = cfgUnits.Screens[0].Lines;
        Check(unitLines[0].Unit == "MW", "line 0 unit is MW");
        Check(unitLines[0].Format == "F2", "line 0 format is F2");
        Check(unitLines[1].Unit == "kL", "line 1 unit is kL");
        Check(unitLines[2].Unit == "t", "line 2 unit is t");
        Check(unitLines[2].Format == "F1", "line 2 format is F1");
        Check(unitLines[3].Unit == "auto", "line 3 unit is auto");

        // Layout.FormatDisplayValue tests
        Check(Layout.FormatDisplayValue(12.3456, "MW", "F2") == "12.35MW", "FormatDisplayValue 12.35MW");
        Check(Layout.FormatDisplayValue(450.0, "kL", "F0") == "450kL", "FormatDisplayValue 450kL");
        Check(Layout.FormatDisplayValue(1500000.0, "auto") == "1.5M", "FormatDisplayValue auto 1.5M");
        Check(Layout.FormatDisplayValue(2400.0, "auto") == "2.4k", "FormatDisplayValue auto 2.4k");

        // RenderBarText with custom suffix
        string barSuffix = Layout.RenderBarText("PWR", 75.0, 10, "12.5MW");
        Check(barSuffix.Contains("12.5MW"), "RenderBarText includes custom suffix: " + barSuffix);

        // Epic 17: Responsive Viewport Profiles
        float fontCompact = Layout.GetProfileFontSize("compact", 256f, 256f, 1.0f);
        Check(fontCompact == 0.75f, "GetProfileFontSize compact scales to 0.75");
        float fontCorner = Layout.GetProfileFontSize("corner", 256f, 128f, 1.0f);
        Check(fontCorner == 0.55f, "GetProfileFontSize corner scales to 0.55");
        float fontWide = Layout.GetProfileFontSize("wide", 1024f, 512f, 1.0f);
        Check(fontWide == 1.15f, "GetProfileFontSize wide scales to 1.15");

        // Epic 21: Dynamic Telemetry & Micro-API Binds
        string dslDyn = @"
@bind target_dist = dynamic(""target.dist"", default: 1500.5)
@bind alert_mode  = var(""alert.status"", default: 0)
@bind ship_speed  = mem(""flight.speed"")
@screen nav
line: ""DIST"" bar(target_dist.val) unit(""m"")
line: ""SPD "" text(ship_speed.val) unit(""m/s"")
";
        var cfgDyn = HelmParser.Parse(dslDyn);
        Check(cfgDyn.Binds.Count == 3, "dynamic binds count == 3");
        Check(cfgDyn.Binds[0].Kind == BindKind.Dynamic, "bind 0 is Dynamic");
        Check(cfgDyn.Binds[0].DynamicKey == "target.dist", "bind 0 key is target.dist");
        Check(Math.Abs(cfgDyn.Binds[0].DefaultValue - 1500.5) < 0.01, "bind 0 default value is 1500.5");
        Check(cfgDyn.Binds[1].Kind == BindKind.Dynamic, "bind 1 is Dynamic");
        Check(cfgDyn.Binds[1].DynamicKey == "alert.status", "bind 1 key is alert.status");
        Check(cfgDyn.Binds[2].Kind == BindKind.Dynamic, "bind 2 is Dynamic");
        Check(cfgDyn.Binds[2].DynamicKey == "flight.speed", "bind 2 key is flight.speed");

        // Epic 22: Advanced Border Engine, Positional Titles & Title Colors
        string dslEpic22 = @"
@screen test_e22
output: ""Bridge LCD"", 0
border: heavy
border_gradient: #38bdf8, #1e40af
title_align: center
title_color: #fef08a
@panel p1 at(0, 0, 1, 1) title: ""COMBAT CORE"" border: double title_align: right title_color: #ef4444 border_gradient: #ef4444, #7f1d1d
line: ""TEST"" bar(x.percent) flat(cyan)
";
        var cfgE22 = HelmParser.Parse(dslEpic22);
        Check(cfgE22.Errors.Count == 0, "Epic 22 DSL parses with zero errors: " + string.Join(" | ", cfgE22.Errors));
        var scrE22 = cfgE22.Screens[0];
        Check(scrE22.Border == "heavy", "screen border parsed as heavy");
        Check(scrE22.BorderGradientColors != null && scrE22.BorderGradientColors.Length == 2, "screen border_gradient parsed 2 stops");
        Check(scrE22.TitleAlign == "center", "screen title_align parsed as center");
        Check(scrE22.TitleColor == "#fef08a", "screen title_color parsed");
        var panE22 = scrE22.Panels[0];
        Check(panE22.Title == "COMBAT CORE", "panel title parsed");
        Check(panE22.Border == "double", "panel border parsed as double");
        Check(panE22.TitleAlign == "right", "panel title_align parsed as right");
        Check(panE22.TitleColor == "#ef4444", "panel title_color parsed as #ef4444");
        Check(panE22.BorderGradientColors != null && panE22.BorderGradientColors.Length == 2, "panel border_gradient parsed 2 stops");

        Console.WriteLine($"\n{pass} passed, {fail} failed.");
        Environment.Exit(fail == 0 ? 0 : 1);
    }
}


