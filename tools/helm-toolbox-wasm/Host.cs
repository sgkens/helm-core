// Host.cs: browser entry point for the HELM Toolbox. Renders a layout with the real script and returns the sprite list as JSON.
//   Render(dsl, width, height, values, seconds, screenIndex) ->
//     {"errors":[..],"warnings":[..],"screens":[..],"binds":[{"name":..,"kind":..}],"surface":[w,h],"texture":[m,m],"sprites":[..]}
// values is "bind=50;other=12"; every value is on the script's 0-100 scale. seconds drives the script's clock (waves, easing).
// The sprite list is in TEXTURE-space coordinates (surface + the bezel-side viewport offset already added),
// the same convention docs/assets/frames/*.json uses: subtract (texture - surface) / 2 before drawing, as
// docs/assets/js/sprite-svg.mjs and se-lcd-bezel.mjs's #content-slot expect.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
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
    public string GetText() { return ""; }
    public void GetSprites(List<string> sprites) { sprites.Add("Grid"); sprites.Add("Circle"); sprites.Add("Danger"); }
    public Vector2 MeasureStringInPixels(StringBuilder t, string font, float scale) { return new Vector2(t.Length * 13f * scale, 22f * scale); }
    public MySpriteDrawFrame DrawFrame() { return new CapturingFrame(this); }
    public void Dispose() { }
}

class CapturingFrame : MySpriteDrawFrame
{
    readonly MockSurface _s;
    public CapturingFrame(MockSurface s) { _s = s; }
    public override void Dispose() { _s.Last = new List<MySprite>(Sprites); }
}

public static partial class Host
{
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

    static void WriteSprite(StringBuilder sb, MySprite s)
    {
        sb.Append("{\"t\":").Append(s.Type == SpriteType.TEXT ? "\"text\"" : s.Type == SpriteType.CLIP_RECT ? "\"clip\"" : "\"tex\"").Append(",\"d\":").Append(Esc(s.Data));
        if (s.Position.HasValue) sb.Append(",\"x\":").Append(F(s.Position.Value.X)).Append(",\"y\":").Append(F(s.Position.Value.Y));
        if (s.Size.HasValue) sb.Append(",\"w\":").Append(F(s.Size.Value.X)).Append(",\"h\":").Append(F(s.Size.Value.Y));
        if (s.Color.HasValue) { var c = s.Color.Value; sb.Append(",\"c\":[").Append(c.R).Append(',').Append(c.G).Append(',').Append(c.B).Append(',').Append(c.A).Append(']'); }
        if (s.Type == SpriteType.TEXT) sb.Append(",\"f\":").Append(Esc(s.FontId)).Append(",\"a\":\"").Append(s.Alignment.ToString().ToLowerInvariant()).Append("\",\"s\":").Append(F(s.RotationOrScale));
        else if (s.RotationOrScale != 0f) sb.Append(",\"r\":").Append(F(s.RotationOrScale));
        sb.Append('}');
    }

    static object Get(object o, string field)
    {
        var f = o.GetType().GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (f == null) throw new InvalidOperationException("Program has no field " + field);
        return f.GetValue(o);
    }

    static void Strings(StringBuilder sb, IEnumerable<string> items)
    {
        sb.Append('[');
        bool first = true;
        foreach (var e in items) { if (!first) sb.Append(','); first = false; sb.Append(Esc(e)); }
        sb.Append(']');
    }

    public static void Main() { }

    static bool _templatesLoaded;

    // Loads layouts/templates.txt into HelmParser's template table exactly as helmv.ps1 and
    // tools/spritedump/Dump.cs do (both call the same HELM_TOOLS-only HelmParser.LoadTemplateFile),
    // so @use resolves the same set of templates here as it does in those tools. A browser has no
    // filesystem, so the file is embedded at build time (see HelmToolboxWasm.csproj) instead of read from
    // disk. Calling LoadTemplateFile also triggers HelmParser's static constructor on first use, which
    // registers the 8 built-in templates before this adds templates.txt's on top.
    static void EnsureTemplatesLoaded()
    {
        if (_templatesLoaded) return;
        _templatesLoaded = true;
        try
        {
            using (var stream = typeof(Host).Assembly.GetManifestResourceStream("templates.txt"))
            {
                if (stream == null) return;
                using (var reader = new System.IO.StreamReader(stream))
                    HelmParser.LoadTemplateFile(reader.ReadToEnd());
            }
        }
        catch { /* best effort: Render/ListTemplates still work with just the built-in templates */ }
    }

    [JSExport]
    public static string ListTemplates()
    {
        EnsureTemplatesLoaded();
        var field = typeof(HelmParser).GetField("BuiltinTemplates", BindingFlags.NonPublic | BindingFlags.Static);
        var templates = (Dictionary<string, TemplateDef>)field.GetValue(null);
        var sb = new StringBuilder("[");
        bool first = true;
        foreach (var kv in templates)
        {
            var t = kv.Value;
            if (!first) sb.Append(',');
            first = false;
            sb.Append("{\"name\":").Append(Esc(t.Name)).Append(",\"params\":[");
            bool firstParam = true;
            foreach (var p in t.ParamNames)
            {
                if (!firstParam) sb.Append(',');
                firstParam = false;
                string def;
                t.Defaults.TryGetValue(p, out def);
                sb.Append("{\"name\":").Append(Esc(p)).Append(",\"default\":").Append(Esc(def)).Append('}');
            }
            sb.Append("]}");
        }
        sb.Append(']');
        return sb.ToString();
    }

    [JSExport]
    public static string Render(string dsl, int width, int height, string values, double seconds, int screenIndex)
    {
        var sb = new StringBuilder();
        try
        {
            EnsureTemplatesLoaded();
            MyGridProgram.BootstrapMe = new MockMe { CustomData = dsl };
            var prog = new Program();
            var config = (ParsedConfig)Get(prog, "_config");
            sb.Append("{\"errors\":"); Strings(sb, config != null ? config.Errors : new List<string> { "no config" });
            sb.Append(",\"warnings\":"); Strings(sb, config != null ? config.Warnings : new List<string>());
            var names = new List<string>();
            if (config != null) foreach (var s in config.Screens) names.Add(s.Name);
            sb.Append(",\"screens\":"); Strings(sb, names);
            sb.Append(",\"binds\":[");
            if (config != null)
            {
                bool firstBind = true;
                foreach (var b in config.Binds)
                {
                    if (!firstBind) sb.Append(',');
                    firstBind = false;
                    sb.Append("{\"name\":").Append(Esc(b.Name)).Append(",\"kind\":").Append(Esc(b.Kind.ToString().ToLowerInvariant()))
                      .Append(",\"target\":").Append(Esc(b.TargetName)).Append(",\"isText\":").Append(b.DynamicKey != null && b.DefaultText != null ? "true" : "false").Append('}');
                }
            }
            sb.Append(']');
            sb.Append(",\"surface\":[").Append(width).Append(',').Append(height).Append("],\"texture\":[").Append(Math.Max(width, height)).Append(',').Append(Math.Max(width, height)).Append(']');
            sb.Append(",\"sprites\":[");
            if (config != null && config.Screens.Count > 0)
            {
                var dyn = (Dictionary<string, double>)Get(prog, "_dynamicValues");
                var given = new Dictionary<string, double>();
                bool wantAlert = false;
                foreach (var kv in (values ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var p = kv.Split('=');
                    if (p.Length != 2) continue;
                    string key = p[0].Trim();
                    // "__alert" is not a @bind name (those come only from config.Binds below); it lets the
                    // caller preview the "alert red" run-argument command (Program.cs's _globalRedAlert),
                    // which no @bind maps to, without changing Render's own parameter list.
                    if (key == "__alert") { wantAlert = p[1].Trim() == "1"; continue; }
                    double v;
                    if (double.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out v)) given[key] = v;
                }
                var alertField = typeof(Program).GetField("_globalRedAlert", BindingFlags.Instance | BindingFlags.NonPublic);
                if (alertField != null) alertField.SetValue(prog, wantAlert);
                foreach (var b in config.Binds)
                {
                    double v;
                    if (!given.TryGetValue(b.Name, out v)) v = 50;
                    dyn[b.Name] = v;
                    if (!string.IsNullOrEmpty(b.DynamicKey)) dyn[b.DynamicKey] = v;
                }
                var screen = config.Screens[Math.Max(0, Math.Min(screenIndex, config.Screens.Count - 1))];
                // The screen picker previews each rotation page independently of the game's clock.
                screen.RotationPages = null;
                var surface = new MockSurface(width, height);
                var resolvedOutputs = (Dictionary<ScreenDef, IMyTextSurface>)Get(prog, "_resolvedOutputs");
                resolvedOutputs[screen] = surface;
                // A mirror screen (RenderScreen -> RenderMirror) reads its source's cached _screenBuffers
                // entry, which only exists once that source has itself been through RenderScreen. The game
                // fills it in naturally over earlier round-robin ticks; here only the requested screen would
                // ever render, so a mirror screen alone previewed as blank (0 sprites, no error - RenderMirror
                // fails silently when the source has no buffer yet). Render the source alongside it instead.
                ScreenDef mirrorSource = screen.Mirror != null
                    ? config.Screens.Find(s => s != screen && s.Name.Equals(screen.Mirror, StringComparison.OrdinalIgnoreCase))
                    : null;
                if (mirrorSource != null)
                {
                    mirrorSource.RotationPages = null;
                    resolvedOutputs[mirrorSource] = new MockSurface(width, height);
                }
                var render = typeof(Program).GetMethod("RenderScreen", BindingFlags.Instance | BindingFlags.NonPublic);
                var timeField = typeof(Program).GetField("_time", BindingFlags.Instance | BindingFlags.NonPublic);
                var tickField = typeof(Program).GetField("_tickCounter", BindingFlags.Instance | BindingFlags.NonPublic);
                // Several passes let the eased values settle, as the game's repeated runs do.
                for (int i = 0; i < 40; i++)
                {
                    timeField.SetValue(prog, seconds + i * 0.1);
                    tickField.SetValue(prog, i);
                    if (mirrorSource != null) render.Invoke(prog, new object[] { mirrorSource });
                    render.Invoke(prog, new object[] { screen });
                }
                bool first = true;
                foreach (var sp in surface.Last) { if (!first) sb.Append(','); first = false; WriteSprite(sb, sp); }
            }
            sb.Append("]}");
        }
        catch (Exception e0)
        {
            var e = e0 is TargetInvocationException && e0.InnerException != null ? e0.InnerException : e0;
            return "{\"errors\":[" + "\"host: " + e.GetType().Name + ": " + e.Message.Replace("\"", "'").Replace("\n", " ") + " @ " + (e.StackTrace ?? "").Split('\n')[0].Trim().Replace("\"", "'") + "\"],\"warnings\":[],\"screens\":[],\"sprites\":[]}";
        }
        return sb.ToString();
    }
}
