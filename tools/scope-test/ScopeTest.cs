// ScopeTest: Program.cs must use only blocks on its own construct (same grid, or subgrids on rotors and pistons),
// never a ship docked on a connector. The game's GetBlocks, GetBlockWithName and GetBlockGroupWithName all reach
// docked ships and return the FIRST match, so a docked ship's block can win. The fake grid below lists the docked
// ship's blocks first to reproduce that. Run: dotnet run --project tools/scope-test (exit code 1 on any failure)
using System; using System.Collections.Generic; using System.Linq; using System.Reflection;
using HelmCore; using IngameScript; using Sandbox.ModAPI.Ingame;
class Ship { public string Name; public Ship(string n) { Name = n; } }
class FBlock : IMyTerminalBlock {
  public Ship Ship; public string CustomName { get; set; } public string CustomData { get; set; }
  public IMyCubeGrid CubeGrid { get { return null; } }
  public dynamic IsSameConstructAs(params object[] a) { return Ship == ((FBlock)a[0]).Ship; }
  public override string ToString() { return Ship.Name + ":" + CustomName; }
}
class FLcd : Dump.MockSurface, IMyTerminalBlock {
  public Ship Ship; public FLcd(Ship s, string n) : base(512, 512) { Ship = s; CustomName = n; }
  public string CustomName { get; set; } public string CustomData { get; set; }
  public IMyCubeGrid CubeGrid { get { return null; } }
  public dynamic IsSameConstructAs(params object[] a) { return Ship == ((FBlock)a[0]).Ship; }
  public override string ToString() { return Ship.Name + ":" + CustomName; }
}
class FGroup : IMyBlockGroup { public string Name { get; set; } public List<IMyTerminalBlock> Blocks = new List<IMyTerminalBlock>();
  public void GetBlocks(List<IMyTerminalBlock> l, Func<IMyTerminalBlock, bool> c = null) { l.AddRange(Blocks); } }
class FGts : IMyGridTerminalSystem {
  public List<IMyTerminalBlock> All = new List<IMyTerminalBlock>(); public List<FGroup> Groups = new List<FGroup>();
  public dynamic GetBlocks(params object[] a) { ((List<IMyTerminalBlock>)a[0]).AddRange(All); return null; }
  public dynamic GetBlockWithName(params object[] a) { return All.FirstOrDefault(b => b.CustomName.Contains((string)a[0])); }
  public dynamic GetBlockGroupWithName(params object[] a) { return Groups.FirstOrDefault(g => g.Name.Contains((string)a[0])); }
  public dynamic GetBlockGroups(params object[] a) { ((List<IMyBlockGroup>)a[0]).AddRange(Groups); return null; }
}
static class ScopeTest {
  static int fails = 0;
  static void Check(bool ok, string what) { Console.WriteLine((ok ? "PASS  " : "FAIL  ") + what); if (!ok) fails++; }
  static int Main() {
    var mine = new Ship("mine"); var sub = mine; var dock = new Ship("docked");
    var me = new FBlock { Ship = mine, CustomName = "Programmable Block" };
    // the docked ship's blocks come FIRST, which is how GetBlockWithName finds the wrong one
    var fBat = new FBlock { Ship = dock, CustomName = "Main Battery" }; var mBat = new FBlock { Ship = mine, CustomName = "Main Battery" };
    var fTank = new FBlock { Ship = dock, CustomName = "H2 Tank Alpha" }; var mTank = new FBlock { Ship = sub, CustomName = "H2 Tank Alpha" };
    var fLcd = new FLcd(dock, "Cockpit"); var mLcd = new FLcd(mine, "Cockpit");
    var gts = new FGts();
    gts.All.AddRange(new IMyTerminalBlock[] { fBat, fTank, fLcd, mBat, mTank, mLcd, me });
    var gDock = new FGroup { Name = "Fleet" }; gDock.Blocks.Add(fBat); gDock.Blocks.Add(fTank);
    var gMine = new FGroup { Name = "Fleet" }; gMine.Blocks.Add(mBat); gMine.Blocks.Add(mTank);
    gts.Groups.Add(gDock); gts.Groups.Add(gMine);
    var gOnlyDock = new FGroup { Name = "DockOnly" }; gOnlyDock.Blocks.Add(fBat); gts.Groups.Add(gOnlyDock);
    me.CustomData = "@bind bat = block(\"Main Battery\")\n@bind tank = block(\"H2 Tank\")\n@bind fleet = group(\"Fleet\", agg: avg)\n@bind wild = group(\"*Tank*\", agg: avg)\n@bind lost = group(\"DockOnly\", agg: avg)\n@bind none = block(\"Nothing Here\")\n@screen S\noutput: \"Cockpit\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"T\"\nline: \"B\" bar(bat.percent) flat(cyan)\n";
    MyGridProgram.BootstrapMe = me; MyGridProgram.BootstrapGts = gts;
    var prog = new Program();
    var binds = (Dictionary<string, List<IMyTerminalBlock>>)Dump.Get(prog, "_resolvedBinds");
    var outs = (Dictionary<ScreenDef, IMyTextSurface>)Dump.Get(prog, "_resolvedOutputs");
    Check(((string)Dump.Get(prog, "_cfgErr")).Length == 0, "test layout parses without errors");
    Func<string, string> show = k => binds.ContainsKey(k) ? string.Join(", ", binds[k].Select(b => b.ToString())) : "(none)";
    foreach (var k in new[] { "bat", "tank", "fleet", "wild", "lost", "none" }) Console.WriteLine("  bind {0,-6} -> {1}", k, show(k));
    Check(binds["bat"].Count == 1 && binds["bat"][0] == mBat, "single block: docked ship's 'Main Battery' listed first, this ship's is used");
    Check(binds["tank"].Count == 1 && binds["tank"][0] == mTank, "single block on a rotor subgrid is used, docked ship's is not");
    Check(binds["fleet"].Count == 2 && binds["fleet"].All(b => ((FBlock)b).Ship == mine), "group: first 'Fleet' group is the docked ship's, this ship's group is used");
    Check(binds["wild"].Count == 1 && binds["wild"][0] == mTank, "wildcard '*Tank*' matches this construct only");
    Check(binds["lost"].Count == 0, "group that exists only on the docked ship resolves to nothing");
    Check(binds["none"].Count == 0, "unknown name resolves to nothing");
    Check(outs.Count == 1 && outs.Values.First() == (IMyTextSurface)mLcd, "output 'Cockpit': this ship's LCD is drawn on, the docked ship's LCD is not touched");
    // Console: the same model is echoed to the info pane and drawn on the Programmable Block's own screen.
    var echo = new System.Text.StringBuilder();
    prog.Echo = t => echo.Append(t);
    var pb = new Dump.MockSurface(512, 320);
    typeof(Program).GetField("_pbSurface", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(prog, pb);
    prog.Main("", UpdateType.Update10);
    string text = echo.ToString();
    Console.WriteLine("--- info pane ---\n" + text + "-----------------");
    Check(text.Contains("HELM-[Kernel-(v0.0.0)]"), "console: header with the version");
    Check(text.Contains("krn-cfg") && text.Contains("cfg-dsp") && text.Contains("perf"), "console: krn-cfg, cfg-dsp and perf sections");
    Check(text.Contains("screens 1") && text.Contains("binds 4/6"), "console: 1 screen; 4 of 6 binds matched a block");
    Check(text.Contains("docked-ignored 3"), "console: three blocks on the docked ship were ignored");
    Check(text.Contains("512x512"), "console: display list shows the surface size");
    Check(text.Contains(" instr ") && text.Contains(" peak "), "console: perf shows the last run's instructions and the peak");
    Check(pb.Last.Count > 0, "console: drawn on the Programmable Block's screen");
    bool fits = pb.Last.Count > 0;
    foreach (var sp in pb.Last)
    {
        if (sp.Data == null || sp.Data == "SquareSimple") continue;
        // Positions include the offset of the (square) texture around the visible surface.
        float sc = sp.RotationOrScale;
        float offX = (pb.TextureSize.X - pb.SurfaceSize.X) / 2f, offY = (pb.TextureSize.Y - pb.SurfaceSize.Y) / 2f;
        float right = sp.Position.Value.X - offX + sp.Data.Length * 13f * sc, bottom = sp.Position.Value.Y - offY + 22f * sc;
        if (right > pb.SurfaceSize.X || bottom > pb.SurfaceSize.Y) { fits = false; Console.WriteLine("  overflow: '" + sp.Data + "' right " + right + " bottom " + bottom); }
    }
    Check(fits, "console: every line fits inside the block's 512x320 screen");
    Console.WriteLine(fails == 0 ? "ALL PASS" : fails + " FAILED");
    return fails;
  }
}
