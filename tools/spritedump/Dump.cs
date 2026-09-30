// Dump.cs — headless frame dump for the HELM docs site.
//
// Runs the real IngameScript.Program renderer (Program.cs, compiled against the
// stand-in API in SeShim*.cs) for each layout x hardware screen profile and writes
// the resulting sprite buffer as JSON. The docs site draws that data; it does not
// re-implement layout. Telemetry is deterministic mock data injected through the
// script's dynamic-value table.
//
// A layout with the comment line "# spritedump: unset" is rendered without injected values (unresolved binds).
//
// Usage:
//   dotnet run --project tools/spritedump -- --out <dir> [--ticks N] [--set name=value ...] [--payload "k=v;k=v"] <layout.txt | dir> ...
//
// With --ticks N (N > 0) each frame also carries an animation: the mock telemetry follows a looping
// timeline for N ticks (6 ticks per second, Update10) and later ticks are exported as deltas against
// the previous tick (changed sprite fields only, or a full replacement when the sprite count changes).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using HelmCore;
using IngameScript;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
    public class StubGts : IMyGridTerminalSystem
    {
        public dynamic GetBlockGroupWithName(params object[] a) { return null; }
        public dynamic GetBlocks(params object[] a) { return null; }
        public dynamic GetBlockGroups(params object[] a) { return null; }
        public dynamic GetBlockWithName(params object[] a) { return null; }
    }
}

public static class Dump
{
    // --payload "key=value;key=value": applied through the script's own ApplyDynamicPayload after mock values are injected.
    static string Payload;
    // --boot: export each screen's boot frame (progress 0.95, all diagnostic lines shown) instead of the live screen.
    static bool BootMode;

    // Hardware profiles from the Epic 23 table. Texture size is ASSUMED square (larger side);
    // it only affects the viewport offset, which is exported so the viewer can crop.
    static readonly (string Name, int W, int H)[] Profiles =
    {
        ("TextPanel", 512, 512), ("WideLCD", 1024, 512), ("CockpitSmall", 512, 256),
        ("CornerLCD", 512, 128), ("CockpitCorner", 256, 128),
    };

    public class MockMe : IMyTerminalBlock
    {
        public string CustomName { get; set; } = "Programmable Block";
        public string CustomData { get; set; }
        public IMyCubeGrid CubeGrid { get { return null; } }
        public dynamic IsSameConstructAs(params object[] a) { return true; }
        public dynamic InventoryCount { get { return 0; } set { } }
        public dynamic GetInventory(params object[] a) { return null; }
    }

    public class MockSurface : IMyTextSurface
    {
        readonly Vector2 _size, _tex;
        public List<MySprite> Last = new List<MySprite>();
        public MockSurface(int w, int h) { _size = new Vector2(w, h); int m = Math.Max(w, h); _tex = new Vector2(m, m); }
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
        public string Text = "";
        public string GetText() { return Text; }
        public void GetSprites(List<string> sprites) { sprites.Add("Grid"); sprites.Add("Circle"); sprites.Add("Danger"); }
        // Placeholder metrics (see DEVTRACKER Epic 26 / TASK-26.5): monospace, 0.6 em wide.
        public Vector2 MeasureStringInPixels(StringBuilder t, string font, float scale)
        { return new Vector2(t.Length * 13f * scale, 22f * scale); }
        public MySpriteDrawFrame DrawFrame() { return new CapturingFrame(this); }
        public void Dispose() { }
    }

    class CapturingFrame : MySpriteDrawFrame
    {
        readonly MockSurface _s;
        public CapturingFrame(MockSurface s) { _s = s; }
        public override void Dispose() { _s.Last = new List<MySprite>(Sprites); }
    }

    static double MockValue(string name)
    {
        uint h = 2166136261;
        foreach (char c in name.ToLowerInvariant()) { h ^= c; h *= 16777619; }
        return 22 + (h % 74); // 22..95
    }

    static string F(float v) { return v.ToString("0.###", CultureInfo.InvariantCulture); }

    static string Esc(string s)
    {
        if (s == null) return "null";
        var sb = new StringBuilder("\"");
        foreach (char c in s)
        {
            if (c == '"') sb.Append("\\\""); else if (c == '\\') sb.Append("\\\\");
            else if (c == '\n') sb.Append("\\n"); else if (c == '\r') sb.Append("\\r"); else if (c == '\t') sb.Append("\\t");
            else if (c < 32) sb.Append("\\u").Append(((int)c).ToString("x4"));
            else sb.Append(c);
        }
        return sb.Append('"').ToString();
    }

    // Serialized sprite fields as (key, json value); the same list drives full and delta output.
    static List<KeyValuePair<string, string>> Fields(MySprite s)
    {
        var f = new List<KeyValuePair<string, string>>();
        Action<string, string> add = (k, v) => f.Add(new KeyValuePair<string, string>(k, v));
        add("t", s.Type == SpriteType.TEXT ? "\"text\"" : s.Type == SpriteType.CLIP_RECT ? "\"clip\"" : "\"tex\"");
        add("d", Esc(s.Data));
        if (s.Position.HasValue) { add("x", F(s.Position.Value.X)); add("y", F(s.Position.Value.Y)); }
        if (s.Size.HasValue) { add("w", F(s.Size.Value.X)); add("h", F(s.Size.Value.Y)); }
        if (s.Color.HasValue) { var c = s.Color.Value; add("c", "[" + c.R + "," + c.G + "," + c.B + "," + c.A + "]"); }
        if (s.Type == SpriteType.TEXT)
        {
            add("f", Esc(s.FontId));
            add("a", "\"" + s.Alignment.ToString().ToLowerInvariant() + "\"");
            add("s", F(s.RotationOrScale));
        }
        else if (s.RotationOrScale != 0f) add("r", F(s.RotationOrScale));
        return f;
    }

    static void WriteSprite(StringBuilder sb, MySprite s)
    {
        sb.Append('{');
        var f = Fields(s);
        for (int i = 0; i < f.Count; i++) { if (i > 0) sb.Append(','); sb.Append('"').Append(f[i].Key).Append("\":").Append(f[i].Value); }
        sb.Append('}');
    }

    static void WriteSpriteList(StringBuilder sb, List<MySprite> list)
    {
        sb.Append('[');
        for (int i = 0; i < list.Count; i++) { if (i > 0) sb.Append(','); WriteSprite(sb, list[i]); }
        sb.Append(']');
    }

    // Delta of one tick against the previous one: [{"i":index, changed fields...}, ...], or {"full":[...]} when the
    // sprite count changed (buffer rebuilt). Fields that disappear are not expected within a retained buffer.
    static void WriteDelta(StringBuilder sb, List<MySprite> prev, List<MySprite> cur)
    {
        if (prev.Count != cur.Count) { sb.Append("{\"full\":"); WriteSpriteList(sb, cur); sb.Append('}'); return; }
        sb.Append('[');
        bool firstChange = true;
        for (int i = 0; i < cur.Count; i++)
        {
            var a = Fields(prev[i]); var b = Fields(cur[i]);
            var d = new StringBuilder();
            var prevMap = new Dictionary<string, string>();
            foreach (var kv in a) prevMap[kv.Key] = kv.Value;
            foreach (var kv in b) { string old; if (!prevMap.TryGetValue(kv.Key, out old) || old != kv.Value) d.Append(',').Append('"').Append(kv.Key).Append("\":").Append(kv.Value); }
            if (d.Length == 0) continue;
            if (!firstChange) sb.Append(',');
            firstChange = false;
            sb.Append("{\"i\":").Append(i).Append(d).Append('}');
        }
        sb.Append(']');
    }

    // Looping telemetry timeline: each bind oscillates around its mock value with a per-bind phase.
    static double Timeline(string name, int tick, int ticks, double baseValue)
    {
        uint h = 2166136261;
        foreach (char c in name.ToLowerInvariant()) { h ^= c; h *= 16777619; }
        double phase = (h % 628) / 100.0;
        double v = baseValue + 22.0 * Math.Sin(2.0 * Math.PI * tick / ticks + phase);
        return Math.Max(0.0, Math.Min(100.0, v));
    }

    public static object Get(object o, string field)
    {
        var f = o.GetType().GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (f == null) throw new InvalidOperationException("Program has no field " + field);
        return f.GetValue(o);
    }

    // Renders every screen of one layout on one hardware profile. Returns JSON objects (one per screen).
    static void RenderLayout(string dsl, string layoutName, (string Name, int W, int H) prof,
                             Dictionary<string, double> overrides, int ticks, StringBuilder sb, ref bool first, List<string> errors)
    {
        MyGridProgram.BootstrapMe = new MockMe { CustomData = dsl };
        var prog = new Program();
        var config = (ParsedConfig)Get(prog, "_config");
        if (config != null && first) foreach (var e in config.Errors) errors.Add(layoutName + " [parser]: " + e);
        if (config == null || config.Screens.Count == 0)
        {
            foreach (var e in config != null ? config.Errors : new List<string> { "no config" }) errors.Add(layoutName + ": " + e);
            return;
        }

        var dyn = (Dictionary<string, double>)Get(prog, "_dynamicValues");
        // A layout containing the comment line "# spritedump: unset" is rendered with no injected values, so its binds
        // resolve to zero blocks the way they do in game when a name matches nothing.
        bool unset = dsl.Contains("# spritedump: unset");
        foreach (var b in config.Binds)
        {
            if (unset) break;
            double v;
            if (!overrides.TryGetValue(b.Name, out v)) v = MockValue(b.Name);
            dyn[b.Name] = v;
            if (!string.IsNullOrEmpty(b.DynamicKey)) dyn[b.DynamicKey] = v;
        }

        if (Payload != null)
            typeof(Program).GetMethod("ApplyDynamicPayload", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(prog, new object[] { Payload });

        var outputs = (Dictionary<ScreenDef, IMyTextSurface>)Get(prog, "_resolvedOutputs");
        var render = typeof(Program).GetMethod("RenderScreen", BindingFlags.Instance | BindingFlags.NonPublic);
        var tick = typeof(Program).GetField("_tickCounter", BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var screen in config.Screens)
        {
            // Export each page independently; in-game rotation belongs to the live scheduler.
            screen.RotationPages = null;
            var surface = new MockSurface(prof.W, prof.H);
            outputs[screen] = surface;
            var frames = new List<List<MySprite>>();
            if (BootMode)
            {
                var drawBoot = typeof(Program).GetMethod("DrawBoot", BindingFlags.Instance | BindingFlags.NonPublic);
                drawBoot.Invoke(prog, new object[] { screen, 0.95 });
                frames.Add(new List<MySprite>(surface.Last));
            }
            else if (ticks <= 0)
            {
                // Several passes: fluid damping converges toward the mock value; keep the last drawn frame.
                for (int i = 0; i < 40; i++) { tick.SetValue(prog, i); render.Invoke(prog, new object[] { screen }); }
                frames.Add(new List<MySprite>(surface.Last));
            }
            else
            {
                for (int t = 0; t < ticks; t++)
                {
                    foreach (var b in config.Binds)
                    {
                        if (unset) break;
                        double v;
                        double baseValue = overrides.TryGetValue(b.Name, out v) ? v : MockValue(b.Name);
                        double tv = Timeline(b.Name, t, ticks, baseValue);
                        dyn[b.Name] = tv;
                        if (!string.IsNullOrEmpty(b.DynamicKey)) dyn[b.DynamicKey] = tv;
                    }
                    tick.SetValue(prog, t);
                    render.Invoke(prog, new object[] { screen });
                    frames.Add(new List<MySprite>(surface.Last));
                }
            }

            if (frames.Count == 0 || frames[frames.Count - 1].Count == 0)
                errors.Add(layoutName + " [" + prof.Name + "] screen " + screen.Name + ": rendered no sprites");

            if (!first) sb.Append(",\n");
            first = false;
            sb.Append("    {\"layout\":").Append(Esc(layoutName)).Append(",\"screen\":").Append(Esc(screen.Name))
              .Append(",\"profile\":").Append(Esc(prof.Name))
              .Append(",\"mode\":").Append(Esc(screen.Border.Equals("ascii", StringComparison.OrdinalIgnoreCase) ? "ascii" : "graphic"))
              .Append(",\"surface\":[").Append(prof.W).Append(',').Append(prof.H).Append(']')
              .Append(",\"texture\":[").Append(Math.Max(prof.W, prof.H)).Append(',').Append(Math.Max(prof.W, prof.H)).Append(']')
              .Append(",\"font\":").Append(Esc(screen.Font))
              .Append(",\"sprites\":[");
            for (int i = 0; i < frames[0].Count; i++) { if (i > 0) sb.Append(','); WriteSprite(sb, frames[0][i]); }
            sb.Append("]");
            if (ticks > 0)
            {
                sb.Append(",\"fps\":6,\"ticks\":").Append(ticks).Append(",\"deltas\":[");
                for (int t = 1; t < frames.Count; t++) { if (t > 1) sb.Append(','); WriteDelta(sb, frames[t - 1], frames[t]); }
                // Closing delta returns to frame 0 so the loop is seamless.
                if (frames.Count > 1) sb.Append(','); 
                WriteDelta(sb, frames[frames.Count - 1], frames[0]);
                sb.Append(']');
            }
            sb.Append("}");
        }
    }

    static int Main(string[] args)
    {
        string outDir = null;
        int ticks = 0;
        var inputs = new List<string>();
        var overrides = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--out" && i + 1 < args.Length) outDir = args[++i];
            else if (args[i] == "--ticks" && i + 1 < args.Length) ticks = int.Parse(args[++i], CultureInfo.InvariantCulture);
            else if (args[i] == "--payload" && i + 1 < args.Length) Payload = args[++i];
            else if (args[i] == "--boot") BootMode = true;
            else if (args[i] == "--set" && i + 1 < args.Length)
            {
                var kv = args[++i].Split('=');
                overrides[kv[0]] = double.Parse(kv[1], CultureInfo.InvariantCulture);
            }
            else inputs.Add(args[i]);
        }
        if (outDir == null || inputs.Count == 0)
        {
            Console.Error.WriteLine("usage: spritedump --out <dir> [--ticks N] [--set bind=value] [--payload \"k=v;k=v\"] [--boot] <layout.txt|dir> ...");
            return 2;
        }

        // layouts/templates.txt sits next to the layouts/<set>/ directories; look relative to each input first.
        var tplCandidates = new List<string>();
        foreach (var inp in inputs)
        {
            string dir = Directory.Exists(inp) ? inp : Path.GetDirectoryName(Path.GetFullPath(inp));
            tplCandidates.Add(Path.Combine(dir, "..", "templates.txt"));
        }
        tplCandidates.Add("layouts/templates.txt");
        foreach (var cand in tplCandidates)
            if (File.Exists(cand)) { HelmParser.LoadTemplateFile(File.ReadAllText(cand)); break; }

        var files = new List<string>();
        foreach (var inp in inputs)
        {
            if (Directory.Exists(inp)) files.AddRange(Directory.GetFiles(inp, "*.txt"));
            else files.Add(inp);
        }
        files.Sort(StringComparer.Ordinal);

        Directory.CreateDirectory(outDir);
        var errors = new List<string>();
        int written = 0;
        foreach (var file in files)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            var sb = new StringBuilder("{\n  \"layout\":").Append(Esc(name)).Append(",\n  \"frames\":[\n");
            bool first = true;
            foreach (var prof in Profiles)
            {
                try { RenderLayout(File.ReadAllText(file), name, prof, overrides, ticks, sb, ref first, errors); }
                catch (Exception ex) { errors.Add(name + " [" + prof.Name + "]: " + (ex.InnerException ?? ex).Message); }
            }
            sb.Append("\n  ]\n}\n");
            File.WriteAllText(Path.Combine(outDir, name + ".json"), sb.ToString());
            written++;
        }
        Console.WriteLine("spritedump: wrote " + written + " layout(s) to " + outDir);
        foreach (var e in errors) Console.Error.WriteLine("  ERROR " + e);
        return errors.Count == 0 ? 0 : 1;
    }
}
