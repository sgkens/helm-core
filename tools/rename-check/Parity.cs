// Parity: drives IngameScript.Program through the public API only (constructor, Main and run arguments) on fake blocks,
// and writes everything observable: every sprite drawn on every screen at several moments, and every echoed line.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IngameScript;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRageMath;

class Surf : IMyTextSurface
{
    readonly Vector2 _size, _tex;
    public List<MySprite> Last = new List<MySprite>();
    public Surf(int w, int h) { _size = new Vector2(w, h); int m = Math.Max(w, h); _tex = new Vector2(m, m); }
    public Vector2 SurfaceSize { get { return _size; } }
    public Vector2 TextureSize { get { return _tex; } }
    public ContentType ContentType { get; set; }
    public string Script { get; set; }
    public Color ScriptBackgroundColor { get; set; }
    public Color ScriptForegroundColor { get; set; }
    public string Font { get; set; }
    public float FontSize { get; set; }
    public Color FontColor { get; set; }
    public Color BackgroundColor { get; set; }
    public bool WriteText(string v, bool append = false) { return true; }
    public string GetText() { return ""; }
    public void GetSprites(List<string> sprites) { sprites.Add("Grid"); sprites.Add("Circle"); sprites.Add("Danger"); }
    public Vector2 MeasureStringInPixels(StringBuilder t, string font, float scale) { return new Vector2(t.Length * 13f * scale, 22f * scale); }
    public MySpriteDrawFrame DrawFrame() { return new Cap(this); }
    public void Dispose() { }
    class Cap : MySpriteDrawFrame
    {
        readonly Surf _s; public Cap(Surf s) { _s = s; }
        public override void Dispose() { _s.Last = new List<MySprite>(Sprites); }
    }
}

class Blk : IMyTerminalBlock, IMyTextSurfaceProvider
{
    public Surf[] Surfs = new Surf[6];
    public Blk(string name) { CustomName = name; for (int i = 0; i < Surfs.Length; i++) Surfs[i] = new Surf(512, 512); }
    public string CustomName { get; set; }
    public string CustomData { get; set; }
    public IMyCubeGrid CubeGrid { get { return null; } }
    public dynamic IsSameConstructAs(params object[] a) { return true; }
    public dynamic InventoryCount { get { return 0; } set { } }
    public dynamic GetInventory(params object[] a) { return null; }
    public int SurfaceCount { get { return Surfs.Length; } }
    public IMyTextSurface GetSurface(int i) { return Surfs[i]; }
}

class Gts : IMyGridTerminalSystem
{
    public List<IMyTerminalBlock> All = new List<IMyTerminalBlock>();
    public dynamic GetBlocks(params object[] a) { ((List<IMyTerminalBlock>)a[0]).AddRange(All); return null; }
    public dynamic GetBlockWithName(params object[] a) { return All.FirstOrDefault(b => b.CustomName.Contains((string)a[0])); }
    public dynamic GetBlockGroupWithName(params object[] a) { return null; }
    public dynamic GetBlockGroups(params object[] a) { return null; }
}

class Clock : IMyGridProgramRuntimeInfo
{
    public UpdateFrequency UpdateFrequency { get; set; }
    public double LastRunTimeMs { get { return 0.125; } }
    public TimeSpan TimeSinceLastRun { get; set; }
    public int CurrentInstructionCount { get { return 4321; } }
    public int MaxInstructionCount { get { return 50000; } }
    public int CurrentCallChainDepth { get { return 0; } }
    public int MaxCallChainDepth { get { return 1000; } }
}

static class Parity
{
    static string F(float? v) { return v.HasValue ? v.Value.ToString("0.###", CultureInfo.InvariantCulture) : "-"; }
    static string V(Vector2? v) { return v.HasValue ? F(v.Value.X) + "," + F(v.Value.Y) : "-"; }
    static string C(Color? c) { return c.HasValue ? c.Value.R + "." + c.Value.G + "." + c.Value.B + "." + c.Value.A : "-"; }

    static void Dump(StringBuilder o, string when, Blk lcd, string name)
    {
        for (int i = 0; i < lcd.Surfs.Length; i++)
        {
            var s = lcd.Surfs[i];
            o.AppendLine("== " + when + " " + name + "[" + i + "] sprites=" + s.Last.Count);
            foreach (var sp in s.Last)
                o.AppendLine(sp.Type + "|" + sp.Data + "|" + V(sp.Position) + "|" + V(sp.Size) + "|" + C(sp.Color) + "|" + F(sp.RotationOrScale) + "|" + sp.FontId + "|" + sp.Alignment);
        }
    }

    static double Value(string name)
    {
        uint h = 2166136261;
        foreach (char c in name.ToLowerInvariant()) { h ^= c; h *= 16777619; }
        return 22 + (h % 74);
    }

    static int Main(string[] args)
    {
        string dir = args[0], outDir = args[1];
        string templates = File.ReadAllText(Path.Combine(dir, "templates.txt"));
        Directory.CreateDirectory(outDir);
        int n = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(dir, "default"), "*.txt").OrderBy(f => f, StringComparer.Ordinal))
        {
            string name = Path.GetFileNameWithoutExtension(file);
            string dsl = File.ReadAllText(file);
            var o = new StringBuilder();
            var gts = new Gts();
            var me = new Blk("Programmable Block") { CustomData = templates + "\n" + dsl };
            var lcds = new List<KeyValuePair<string, Blk>>();
            foreach (Match m in Regex.Matches(dsl, "output:\\s*\"([^\"]+)\"").Cast<Match>().GroupBy(x => x.Groups[1].Value).Select(g => g.First()))
            {
                var b = new Blk(m.Groups[1].Value);
                lcds.Add(new KeyValuePair<string, Blk>(b.CustomName, b));
                gts.All.Add(b);
            }
            gts.All.Add(me);
            MyGridProgram.BootstrapMe = me;
            MyGridProgram.BootstrapGts = gts;
            var clock = new Clock();
            var prog = new Program();
            prog.Runtime = clock;
            prog.Echo = t => o.Append("echo: ").AppendLine(Regex.Replace(t, @"\(v[0-9A-Za-z.+\-]+\)", "(vX)").Replace("\r", "").Replace("\n", " / "));
            foreach (Match b in Regex.Matches(dsl, @"@bind\s+(\w+)"))
                prog.Main("set " + b.Groups[1].Value + "=" + Value(b.Groups[1].Value).ToString(CultureInfo.InvariantCulture), UpdateType.Terminal);

            double now = 0;
            for (int run = 1; run <= 90; run++)
            {
                double dt = clock.UpdateFrequency == UpdateFrequency.Update100 ? 100 / 60.0 : 10 / 60.0;
                now += dt;
                clock.TimeSinceLastRun = TimeSpan.FromSeconds(dt);
                // values change once, and an alert is switched on and off, so easing, redraws and the alert path are exercised
                if (run == 40) foreach (Match b in Regex.Matches(dsl, @"@bind\s+(\w+)"))
                    prog.Main("set " + b.Groups[1].Value + "=" + (100 - Value(b.Groups[1].Value)).ToString(CultureInfo.InvariantCulture), UpdateType.Terminal);
                if (run == 60) prog.Main("alert red", UpdateType.Terminal);
                if (run == 75) prog.Main("alert off", UpdateType.Terminal);
                prog.Main("", UpdateType.Update10);
                o.AppendLine("run " + run + " rate=" + clock.UpdateFrequency);
                if (run == 3 || run == 12 || run == 45 || run == 90) foreach (var l in lcds) Dump(o, "run" + run, l.Value, l.Key);
            }
            prog.Main("scan", UpdateType.Terminal);
            prog.Main("boot", UpdateType.Terminal);
            for (int run = 91; run <= 96; run++) { clock.TimeSinceLastRun = TimeSpan.FromSeconds(0.2); prog.Main("", UpdateType.Update10); }
            foreach (var l in lcds) Dump(o, "afterboot", l.Value, l.Key);
            File.WriteAllText(Path.Combine(outDir, name + ".txt"), o.ToString());
            n++;
        }
        Console.WriteLine("parity: wrote " + n + " layouts to " + outDir);
        return 0;
    }
}
