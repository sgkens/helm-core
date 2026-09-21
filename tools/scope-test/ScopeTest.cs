// ScopeTest: Program.cs must use only blocks on its own construct (same grid, or subgrids on rotors and pistons),
// never a ship docked on a connector. The game's GetBlocks, GetBlockWithName and GetBlockGroupWithName all reach
// docked ships and return the FIRST match, so a docked ship's block can win. The fake grid below lists the docked
// ship's blocks first to reproduce that. Run: dotnet run --project tools/scope-test (exit code 1 on any failure)
using System; using System.Collections.Generic; using System.Linq; using System.Reflection;
using HelmCore; using IngameScript; using Sandbox.ModAPI.Ingame;
class Ship { public string Name; public Ship(string n) { Name = n; } }
class FInv { public double Iron, Ice; public List<VRage.Game.ModAPI.Ingame.MyInventoryItem> Items = new List<VRage.Game.ModAPI.Ingame.MyInventoryItem>(); public dynamic GetItems(params object[] a) { ((List<VRage.Game.ModAPI.Ingame.MyInventoryItem>)a[0]).AddRange(Items); return null; } public dynamic GetItemAmount(params object[] a) { var t = (VRage.Game.ModAPI.Ingame.MyItemType)a[0]; return t.TypeId == "MyObjectBuilder_Ingot" && t.SubtypeId == "Iron" ? Iron : t.TypeId == "MyObjectBuilder_Ore" && t.SubtypeId == "Ice" ? Ice : 0.0; } }
class FBlock : IMyTerminalBlock {
  public FInv[] Invs = new FInv[0]; public dynamic InventoryCount { get { return Invs.Length; } set { } } public dynamic GetInventory(params object[] a) { return Invs[(int)a[0]]; }
  public Ship Ship; public string CustomName { get; set; } public string CustomData { get; set; }
  public IMyCubeGrid CubeGrid { get { return null; } }
  public dynamic IsSameConstructAs(params object[] a) { return Ship == ((FBlock)a[0]).Ship; }
  public override string ToString() { return Ship.Name + ":" + CustomName; }
}
class FConn : FBlock, IMyShipConnector { public dynamic Status { get; set; } public dynamic Enabled { get; set; } }
class FDoor : FBlock, IMyDoor { public dynamic OpenRatio { get; set; } public dynamic Enabled { get; set; } }
class FLcd : Dump.MockSurface, IMyTerminalBlock {
  public dynamic InventoryCount { get { return 0; } set { } } public dynamic GetInventory(params object[] a) { return null; }
  public Ship Ship; public FLcd(Ship s, string n, int w = 512, int h = 512) : base(w, h) { Ship = s; CustomName = n; }
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
  public static double barInfo2(System.Collections.IList slots, List<VRage.Game.GUI.TextPanel.MySprite> sprites, int n) {
    var slot = slots[n]; var t = slot.GetType();
    float max = (float)t.GetField("BarMaxWidth").GetValue(slot);
    return sprites[(int)t.GetField("FillSpriteIndex").GetValue(slot)].Size.Value.X / max;
  }
  static int fails = 0;
  static void Check(bool ok, string what) { Console.WriteLine((ok ? "PASS  " : "FAIL  ") + what); if (!ok) fails++; }
  static int Main() {
    var mine = new Ship("mine"); var sub = mine; var dock = new Ship("docked");
    var me = new FBlock { Ship = mine, CustomName = "Programmable Block" };
    // the docked ship's blocks come FIRST, which is how GetBlockWithName finds the wrong one
    var fBat = new FBlock { Ship = dock, CustomName = "Main Battery" }; var mBat = new FBlock { Ship = mine, CustomName = "Main Battery" };
    var fTank = new FBlock { Ship = dock, CustomName = "H2 Tank Alpha" }; var mTank = new FBlock { Ship = sub, CustomName = "H2 Tank Alpha" };
    var fLcd = new FLcd(dock, "Cockpit"); var mLcd = new FLcd(mine, "Cockpit"); var aLcd = new FLcd(mine, "AmmoLcd"); var gLcd = new FLcd(mine, "GradLcd"); var nLcd = new FLcd(mine, "NoneLcd"); var rLcd = new FLcd(mine, "RefLcd"); var cLcd = new FLcd(mine, "CkLcd"); var vLcd = new FLcd(mine, "ValLcd"); var tLcd = new FLcd(mine, "TexLcd"); var qLcd = new FLcd(mine, "StepLcd"); var bLcd = new FLcd(mine, "BorderLcd"); var pLcd = new FLcd(mine, "TopLcd"); var eLcd = new FLcd(mine, "EmptyLcd"); var kLcd = new FLcd(mine, "GaugeLcd"); var frLcd = new FLcd(mine, "FrameLcd"); var szLcd = new FLcd(mine, "SizeLcd"); var mpLcd = new FLcd(mine, "PanLcd", 256, 256); var miLcd = new FLcd(mine, "MirrorLcd", 256, 256); var noteSrc = new FLcd(mine, "Notes") { Text = "alpha\r\nbeta\ngamma" }; var noteLcd = new FLcd(mine, "NoteLcd"); var conn = new FConn { Ship = mine, CustomName = "Dock Connector", Status = MyShipConnectorStatus.Connected }; var hdoor = new FDoor { Ship = mine, CustomName = "Hangar Door", OpenRatio = 0.5f };
    mBat.Invs = new[] { new FInv { Iron = 100, Ice = 5 }, new FInv { Iron = 50 } };
    mTank.Invs = new[] { new FInv { Iron = 25 } };
    fBat.Invs = new[] { new FInv { Iron = 9999 } };
    var gts = new FGts();
    gts.All.AddRange(new IMyTerminalBlock[] { fBat, fTank, fLcd, mBat, mTank, mLcd, frLcd, szLcd, mpLcd, miLcd, noteSrc, noteLcd, aLcd, gLcd, nLcd, rLcd, cLcd, vLcd, tLcd, qLcd, bLcd, pLcd, eLcd, kLcd, conn, hdoor, me });
    var gDock = new FGroup { Name = "Fleet" }; gDock.Blocks.Add(fBat); gDock.Blocks.Add(fTank);
    var gMine = new FGroup { Name = "Fleet" }; gMine.Blocks.Add(mBat); gMine.Blocks.Add(mTank);
    gts.Groups.Add(gDock); gts.Groups.Add(gMine);
    var gOnlyDock = new FGroup { Name = "DockOnly" }; gOnlyDock.Blocks.Add(fBat); gts.Groups.Add(gOnlyDock);
    me.CustomData = "@bind bat = block(\"Main Battery\")\n@bind tank = block(\"H2 Tank\")\n@bind fleet = group(\"Fleet\", agg: avg)\n@bind wild = group(\"*Tank*\", agg: avg)\n@bind lost = group(\"DockOnly\", agg: avg)\n@bind none = block(\"Nothing Here\")\n@bind note = block(\"Notes\")\n@bind fleetsum = group(\"Fleet\", agg: sum)\n@bind conn = block(\"Dock Connector\")\n@bind hdoor = block(\"Hangar Door\")\n@bind dock = group(\"Fleet\", agg: sum, scope: all)\n@bind dockwild = group(\"*Tank*\", agg: sum, scope: all)\n@screen S\noutput: \"Cockpit\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"T\"\nline: \"B\" bar(bat.percent) flat(cyan)\n@screen A2\noutput: \"AmmoLcd\", 0\nlayout: grid(1, 1)\n@panel q at(0, 0, 1, 1) title: \"T\"\nline: \"HALF\" bar(bat.ingot:Iron) max(300) flat(cyan)\nline: \"OVER\" bar(bat.ingot:Iron) max(100) flat(cyan)\nline: \"DYN \" bar(bat.ingot:Iron) max(fleetsum.ingot:Iron) flat(cyan)\n@screen G\noutput: \"GradLcd\", 0\nlayout: grid(2, 1)\nbg_gradient(#000000, #ffffff)\npanel_bg: #112233\n@panel a at(0, 0, 1, 1) title: \"A\"\nline: \"X\" bar(bat.percent) flat(cyan)\n@panel b at(1, 0, 1, 1) title: \"B\"\nline: \"Y\" bar(bat.percent) flat(cyan)\n@screen N\noutput: \"NoneLcd\", 0\nlayout: grid(2, 1)\nbg: #102030\npanel_bg: none\nalert_bg: #ff0000\n@panel a at(0, 0, 1, 1) title: \"A\"\nline: \"X\" bar(bat.percent) flat(cyan)\n@panel b at(1, 0, 1, 1) title: \"B\"\nline: \"Y\" bar(bat.percent) flat(cyan)\n@screen C\noutput: \"CkLcd\", 0\nlayout: grid(1, 1)\n@panel c at(0, 0, 1, 1) title: \"C\"\nline: \"LINK\" checkbox(conn.connected) flat(cyan)\nline: \"DOOR\" bar(hdoor.open) flat(cyan)\n@screen V\noutput: \"ValLcd\", 0\nlayout: grid(1, 1)\n@panel v at(0, 0, 1, 1) title: \"V\"\nbg: color(bat.ingot:Iron > 100 : #ff0000, default: #00ff00)\nflash: bat.ingot:Iron > 100\nline: \"X\" bar(bat.percent) flat(cyan)\n@screen T\noutput: \"TexLcd\", 0\nlayout: grid(1, 1)\nbg_gradient(#000000, #ffffff)\nbg_gradient_dir: horizontal\nbg_texture: Grid\n@panel t at(0, 0, 1, 1) title: \"T\"\nline: \"X\" bar(bat.percent) flat(cyan)\n@screen Q\noutput: \"StepLcd\", 0\nlayout: grid(1, 1)\ngradient_mode: steps\n@panel q at(0, 0, 1, 1) title: \"Q\"\nline: \"GRAD\" bar(bat.ingot:Iron) max(300) gradient(#000000, #ffffff)\n@screen B\noutput: \"BorderLcd\", 0\nlayout: grid(1, 1)\nborder_gradient(#ff0000, #0000ff)\n@panel b at(0, 0, 1, 1) title: \"B\"\nline: \"GRAD\" bar(bat.ingot:Iron) max(300) gradient(#000000, #ffffff)\n@screen Tk\noutput: \"GaugeLcd\", 0\nlayout: grid(1, 1)\n@panel t at(0, 0, 1, 1) title: \"T\"\nline: \"HALF\" tank(bat.ingot:Iron) max(300) flat(cyan)\nline: \"FULL\" tank(bat.ingot:Iron) max(150) gradient(#ff0000, #00ff00)\nline: \"VBAR\" bar(bat.ingot:Iron) max(300) flat(cyan)\n@screen Nt\noutput: \"NoteLcd\", 0\nlayout: grid(1, 1)\n@panel n at(0, 0, 1, 1) title: \"N\"\nline: \"1\" text(note.line1) flat(cyan)\nline: \"2\" text(note.line2) flat(cyan)\nline: \"3\" text(note.line3) flat(cyan)\nline: \"4\" text(note.line4) flat(cyan)\nline: \"5\" bar(bat.ingot:Iron) max(300) color(bat.ingot:Iron > 40 : #ff0000, default: #00ff00)\nline: \"6\" text(note.line1) gradient(#ff0000, #00ff00)\n@screen Sz\noutput: \"SizeLcd\", 0\nlayout: grid(1, 1)\n@panel s at(0, 0, 1, 1) title: \"S\"\nline: \"BIG\" bar(bat.ingot:Iron) max(300) size(large) label_color(#00ff00) track(#123456) color(bat.ingot:Iron > 40 : red/blue, default: green)\nline: \"MID\" bar(bat.ingot:Iron) max(300) size(medium)\nline: \"NRM\" bar(bat.ingot:Iron) max(300)\nline: \"PLN\" bar(bat.ingot:Iron) max(300) color(bat.percent < 10 : red, default: green)\nline: \"CLR\" bar(bat.ingot:Iron) max(300) color_by(bat.percent) color(bat.percent < 10 : red, default: green)\nline: \"LED\" led(bat.ingot:Iron) max(300) track(#123456)\nline: \"SEG\" led(bat.ingot:Iron) max(300) segments(20)\nline: \"PTS\" spark(bat.ingot:Iron) max(300) points(24)\nline: \"DRS\" bar(bat.ingot:Iron) max(300) ticks(4) marker(25, 80) outline(#ff00ff) dir: left\n@screen Fr\noutput: \"FrameLcd\", 0\nlayout: grid(1, 1)\nborder: double\nborder_color: #ff0000\n@panel f at(0, 0, 1, 1) title: \"F\"\nborder: rounded\nline: \"X\" bar(bat.percent) flat(cyan)\n@screen Mp\noutput: \"PanLcd\", 0\nmirror: \"tk\"\nmirror_panel: \"t\"\n@screen Mi\noutput: \"MirrorLcd\", 0\nmirror: \"tk\"\n@screen E\noutput: \"EmptyLcd\", 0\nlayout: grid(1, 1)\n@screen P\noutput: \"TopLcd\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"P\"\nline: \"1\" text(bat.top1) flat(cyan)\nline: \"2\" text(bat.top2) flat(cyan)\nline: \"3\" text(bat.top3) flat(cyan)\nline: \"4\" text(bat.top4) flat(cyan)\n@screen R\noutput: \"RefLcd\", 0\nlayout: grid(1, 1)\nrefresh: 600\n@panel r at(0, 0, 1, 1) title: \"R\"\nline: \"X\" bar(bat.percent) flat(cyan)\n";
    MyGridProgram.BootstrapMe = me; MyGridProgram.BootstrapGts = gts;
    var prog = new Program();
    var binds = (Dictionary<string, List<IMyTerminalBlock>>)Dump.Get(prog, "_resolvedBinds");
    var outs = (Dictionary<ScreenDef, IMyTextSurface>)Dump.Get(prog, "_resolvedOutputs");
    var cfg0 = (ParsedConfig)Dump.Get(prog, "_config");
    Check(((string)Dump.Get(prog, "_cfgErr")).Length == 0, "test layout parses without errors");
    Func<string, string> show = k => binds.ContainsKey(k) ? string.Join(", ", binds[k].Select(b => b.ToString())) : "(none)";
    foreach (var k in new[] { "bat", "tank", "fleet", "wild", "lost", "none" }) Console.WriteLine("  bind {0,-6} -> {1}", k, show(k));
    Check(binds["bat"].Count == 1 && binds["bat"][0] == mBat, "single block: docked ship's 'Main Battery' listed first, this ship's is used");
    Check(binds["tank"].Count == 1 && binds["tank"][0] == mTank, "single block on a rotor subgrid is used, docked ship's is not");
    Check(binds["fleet"].Count == 2 && binds["fleet"].All(b => ((FBlock)b).Ship == mine), "group: first 'Fleet' group is the docked ship's, this ship's group is used");
    Check(binds["wild"].Count == 1 && binds["wild"][0] == mTank, "wildcard '*Tank*' matches this construct only");
    Check(binds["lost"].Count == 0, "group that exists only on the docked ship resolves to nothing");
    Check(binds["none"].Count == 0, "unknown name resolves to nothing");
    Check(outs.Count == cfg0.Screens.Count && outs[cfg0.Screens[0]] == (IMyTextSurface)mLcd, "output 'Cockpit': this ship's LCD is drawn on, the docked ship's LCD is not touched");
    // Item amounts: "kind:Subtype" reads the amount of one item across all of a block's inventories.
    var rfv = typeof(Program).GetMethod("ResolveFieldValue", BindingFlags.Instance | BindingFlags.NonPublic);
    Func<string, string, double> field = (b, f) => (double)rfv.Invoke(prog, new object[] { b, f });
    Check(field("bat", "ingot:Iron") == 150, "items: 'ingot:Iron' adds both inventories of one block (100 + 50), not the docked ship's 9999");
    Check(field("bat", "ore:Ice") == 5, "items: 'ore:Ice' reads another item type");
    Check(field("bat", "component:SteelPlate") == 0, "items: an item the block does not hold is 0");
    Check(field("fleetsum", "ingot:Iron") == 175, "items: a group with agg: sum adds its blocks (150 + 25)");
    Check(field("fleet", "ingot:Iron") == 87.5, "items: a group with agg: avg averages them");

    // max(N): the bar fills to amount/N (capped at 100%) while the text keeps the real amount.
    var render = typeof(Program).GetMethod("RenderScreen", BindingFlags.Instance | BindingFlags.NonPublic);
    var ammo = cfg0.Screens[1];
    for (int pass = 0; pass < 40; pass++) render.Invoke(prog, new object[] { ammo });
    var bufs = Dump.Get(prog, "_screenBuffers");
    var buf = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { ammo });
    var slots = (System.Collections.IList)buf.GetType().GetField("Slots").GetValue(buf);
    var sprites = (List<VRage.Game.GUI.TextPanel.MySprite>)buf.GetType().GetField("Sprites").GetValue(buf);
    Func<int, string, double> barInfo = (n, what) => {
        var slot = slots[n]; var t = slot.GetType();
        float max = (float)t.GetField("BarMaxWidth").GetValue(slot);
        var fill = sprites[(int)t.GetField("FillSpriteIndex").GetValue(slot)];
        return what == "fill" ? fill.Size.Value.X / max : double.Parse(sprites[(int)t.GetField("ValSpriteIndex").GetValue(slot)].Data);
    };
    Check(Math.Abs(barInfo(0, "fill") - 0.5) < 0.01, "max: 150 of max(300) fills the bar to 50%: " + barInfo(0, "fill").ToString("0.000"));
    Check(barInfo(0, "text") == 150, "max: the text keeps the real amount (150)");
    Check(Math.Abs(barInfo(1, "fill") - 1.0) < 0.01, "max: 150 of max(100) is capped at a full bar: " + barInfo(1, "fill").ToString("0.000"));
    Check(barInfo(1, "text") == 150, "max: the capped bar still prints 150");

    // scope: all reads a docked ship on purpose; without it the docked ship is ignored.
    Check(binds["dock"].Count == 2 && binds["dock"].All(b => ((FBlock)b).Ship == dock), "scope: all: group 'Fleet' resolves to the docked ship's blocks when asked for");
    Check(binds["fleet"].All(b => ((FBlock)b).Ship == mine), "scope: without it the same group still resolves to this ship");
    Check(binds["dockwild"].Count == 2 && binds["dockwild"].Contains(fTank) && binds["dockwild"].Contains(mTank), "scope: all: a wildcard matches both ships");
    // max(bind.field): the maximum is read from another bind: 150 of 175 fills 85.7%.
    Check(Math.Abs(barInfo(2, "fill") - 150.0 / 175.0) < 0.01, "max(bind.field): 150 of fleetsum's 175 fills the bar to 85.7%: " + barInfo(2, "fill").ToString("0.000"));
    Check(barInfo(2, "text") == 150, "max(bind.field): the text keeps the real amount");

    // Boolean and ratio fields are on the same 0 to 100 scale as percent, so checkbox, toggle, status and bar read them.
    Check(field("conn", "connected") == 100, "fields: a connected connector reads 100, not 1");
    Check(field("hdoor", "open") == 50, "fields: a door half open reads 50, not 0.5: " + field("hdoor", "open"));
    var ck = cfg0.Screens[4];
    for (int pass = 0; pass < 40; pass++) render.Invoke(prog, new object[] { ck });
    var ckBuf = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { ck });
    var ckSlots = (System.Collections.IList)ckBuf.GetType().GetField("Slots").GetValue(ckBuf);
    var ckSprites = (List<VRage.Game.GUI.TextPanel.MySprite>)ckBuf.GetType().GetField("Sprites").GetValue(ckBuf);
    Func<int, string> valText = n => ckSprites[(int)ckSlots[n].GetType().GetField("ValSpriteIndex").GetValue(ckSlots[n])].Data;
    Check(valText(0) == "ACTIVE", "fields: checkbox(conn.connected) reads ACTIVE when connected: " + valText(0));
    conn.Status = MyShipConnectorStatus.Unconnected;
    for (int pass = 0; pass < 40; pass++) render.Invoke(prog, new object[] { ck });
    Check(valText(0) == "READY" && field("conn", "connected") == 0, "fields: and READY once it is disconnected: " + valText(0));
    Check(Math.Abs(ScopeTest.barInfo2(ckSlots, ckSprites, 1) - 0.5) < 0.01, "fields: bar(hdoor.open) fills to 50% for a half open door: " + ScopeTest.barInfo2(ckSlots, ckSprites, 1).ToString("0.000"));
    conn.Status = MyShipConnectorStatus.Connected;

    // Backgrounds: a gradient is drawn as 16 bands; the default card is a faint translucent overlay; panel_bg: none removes
    // the card; and the red alert flashes the background to alert_bg and back.
    Func<ScreenDef, List<VRage.Game.GUI.TextPanel.MySprite>> spritesOf = sc => {
        for (int pass = 0; pass < 3; pass++) render.Invoke(prog, new object[] { sc });
        var bb = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { sc });
        return (List<VRage.Game.GUI.TextPanel.MySprite>)bb.GetType().GetField("Sprites").GetValue(bb);
    };
    Func<VRage.Game.GUI.TextPanel.MySprite, string> col = sp => sp.Color.Value.R + "." + sp.Color.Value.G + "." + sp.Color.Value.B + "." + sp.Color.Value.A;
    var grad = spritesOf(cfg0.Screens[2]);
    Check(grad[0].Color.Value.R < 20 && grad[15].Color.Value.R > 235 && grad[7].Color.Value.R > 100 && grad[7].Color.Value.R < 150, "background: bg_gradient(#000000, #ffffff) runs dark to light over 16 bands");
    Check(grad.Count(sp => col(sp) == "17.34.51.255") == 2, "background: panel_bg: #112233 draws a card behind each of the two panels");
    var flat = spritesOf(cfg0.Screens[3]);
    Check(col(flat[0]) == "16.32.48.255", "background: bg #102030 is one quad: " + col(flat[0]));
    Check(!flat.Any(sp => col(sp) == "255.255.255.6" || sp.Size.HasValue && sp.Size.Value.X > 200f && sp.Size.Value.X < 300f && sp.Size.Value.Y > 400f), "background: without panel_bg the panels draw no card, so the screen is one colour");
    var tick = typeof(Program).GetField("_time", BindingFlags.Instance | BindingFlags.NonPublic);
    prog.Main("alert red", UpdateType.Terminal);
    tick.SetValue(prog, 0.0); flat = spritesOf(cfg0.Screens[3]);
    Check(col(flat[0]) == "255.0.0.255", "background: red alert flashes the background to alert_bg (phase on): " + col(flat[0]));
    tick.SetValue(prog, 1.0); flat = spritesOf(cfg0.Screens[3]);
    Check(col(flat[0]) == "16.32.48.255", "background: and back to bg (phase off): " + col(flat[0]));
    prog.Main("alert off", UpdateType.Terminal);
    tick.SetValue(prog, 0.0); flat = spritesOf(cfg0.Screens[3]);
    Check(col(flat[0]) == "16.32.48.255", "background: no flashing once the alert is off: " + col(flat[0]));

    // Value-driven backgrounds: a panel colour follows a value, and a panel flashes to alert_bg on a threshold.
    var timeF = typeof(Program).GetField("_time", BindingFlags.Instance | BindingFlags.NonPublic);
    Func<ScreenDef, KeyValuePair<List<VRage.Game.GUI.TextPanel.MySprite>, System.Collections.IList>> bgOf = sc => {
        for (int pass = 0; pass < 3; pass++) render.Invoke(prog, new object[] { sc });
        var bb = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { sc });
        return new KeyValuePair<List<VRage.Game.GUI.TextPanel.MySprite>, System.Collections.IList>(
            (List<VRage.Game.GUI.TextPanel.MySprite>)bb.GetType().GetField("Sprites").GetValue(bb), (System.Collections.IList)bb.GetType().GetField("Bgs").GetValue(bb));
    };
    Func<KeyValuePair<List<VRage.Game.GUI.TextPanel.MySprite>, System.Collections.IList>, string> panelBg = kv => {
        var e = kv.Value[kv.Value.Count - 1]; return col(kv.Key[(int)e.GetType().GetField("Index").GetValue(e)]); };
    var valScreen = cfg0.Screens[5];
    timeF.SetValue(prog, 1.0); // flash phase off (period 1.6 s)
    Check(panelBg(bgOf(valScreen)) == "255.0.0.255", "background: iron 150 > 100 turns the panel red (bg: color rule): " + panelBg(bgOf(valScreen)));
    timeF.SetValue(prog, 0.0); // flash phase on
    Check(panelBg(bgOf(valScreen)) == "91.15.15.255", "background: flash: iron > 100 flashes it to alert_bg on the on phase: " + panelBg(bgOf(valScreen)));
    mBat.Invs = new[] { new FInv { Iron = 5 } };
    timeF.SetValue(prog, 1.0);
    Check(panelBg(bgOf(valScreen)) == "0.255.0.255", "background: iron 5 falls to the default rule and turns it green: " + panelBg(bgOf(valScreen)));
    mBat.Invs = new[] { new FInv { Iron = 100 }, new FInv { Iron = 50 } };
    timeF.SetValue(prog, 0.0);
    // Horizontal gradient and a texture.
    var texScreen = cfg0.Screens[6];
    var tex = bgOf(texScreen).Key;
    Check(tex[15].Position.Value.X > tex[0].Position.Value.X + 300 && Math.Abs(tex[15].Position.Value.Y - tex[0].Position.Value.Y) < 1 && tex[0].Color.Value.R < 20 && tex[15].Color.Value.R > 235, "background: bg_gradient_dir: horizontal lays the 16 bands left to right, dark to light");
    Check(tex.Any(sp => sp.Data == "Grid" && col(sp) == "255.255.255.24"), "background: bg_texture: Grid draws the sprite over the background, white at 24/255");

    // Smooth gradients: 50% on black to white is mid grey, or pure white with gradient_mode: steps. border_gradient blends
    // across both border lines.
    Func<ScreenDef, string> fillColour = sc => {
        for (int pass = 0; pass < 40; pass++) render.Invoke(prog, new object[] { sc });
        var gb = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { sc });
        var gs = (System.Collections.IList)gb.GetType().GetField("Slots").GetValue(gb);
        var gsp = (List<VRage.Game.GUI.TextPanel.MySprite>)gb.GetType().GetField("Sprites").GetValue(gb);
        return col(gsp[(int)gs[0].GetType().GetField("FillSpriteIndex").GetValue(gs[0])]);
    };
    string smooth = fillColour(cfg0.Screens[8]);
    Check(smooth.StartsWith("127.127.127") || smooth.StartsWith("128.128.128") || smooth.StartsWith("126.126.126"), "gradient: 50% on black to white blends to mid grey: " + smooth);
    Check(fillColour(cfg0.Screens[7]).StartsWith("255.255.255"), "gradient: gradient_mode: steps picks the nearest stop (white): " + fillColour(cfg0.Screens[7]));
    var bsp = bgOf(cfg0.Screens[8]).Key.Where(sp => sp.Size.HasValue && sp.Size.Value.Y == 2f && sp.Size.Value.X < 100f).OrderBy(sp => sp.Position.Value.X).ToList();
    Check(bsp.Count == 32 && bsp[0].Color.Value.R > 230 && bsp[0].Color.Value.B < 30 && bsp[bsp.Count - 1].Color.Value.B > 230 && bsp[bsp.Count - 1].Color.Value.R < 30, "gradient: border_gradient(red, blue) is 16 strips on each border line running red to blue: " + bsp.Count);

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
    Check(text.Contains("screens " + cfg0.Screens.Count) && text.Contains("binds 10/12"), "console: all screens listed; 10 of 12 binds matched a block");
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
    // refresh: 600 draws a screen at most every 10 s. After it is drawn its next draw is due 10 s later, and it is not
    // touched (or re-armed) before then.
    var refreshScreen = cfg0.Screens[cfg0.Screens.Count - 1];
    var timeField = typeof(Program).GetField("_time", BindingFlags.Instance | BindingFlags.NonPublic);
    var drawAt = (Dictionary<ScreenDef, double>)Dump.Get(prog, "_drawAt");
    timeField.SetValue(prog, 100.0);
    for (int k = 0; k < cfg0.Screens.Count; k++) prog.Main("", UpdateType.Update10);
    Check(drawAt.ContainsKey(refreshScreen) && Math.Abs(drawAt[refreshScreen] - 110.0) < 0.5, "refresh: 600 arms the next draw 10 s later: " + (drawAt.ContainsKey(refreshScreen) ? drawAt[refreshScreen].ToString("0.0") : "none"));
    timeField.SetValue(prog, 105.0);
    for (int k = 0; k < cfg0.Screens.Count; k++) prog.Main("", UpdateType.Update10);
    Check(Math.Abs(drawAt[refreshScreen] - 110.0) < 0.5, "refresh: a visit before the 10 s are up is skipped: " + drawAt[refreshScreen].ToString("0.0"));
    timeField.SetValue(prog, 111.0);
    for (int k = 0; k < cfg0.Screens.Count; k++) prog.Main("", UpdateType.Update10);
    Check(Math.Abs(drawAt[refreshScreen] - 121.0) < 0.5, "refresh: after 10 s it is drawn again and re-armed: " + drawAt[refreshScreen].ToString("0.0"));
    timeField.SetValue(prog, 0.0);

    // Calibration sweep after the boot: a gauge reading 50% runs 0 to 100 (peak at the midpoint) and settles on 50%.
    var calib = (Dictionary<ScreenDef, double>)Dump.Get(prog, "_calibT0");
    var a2 = cfg0.Screens[1];
    Func<double, double> sweepFill = t => {
        timeField.SetValue(prog, t); calib[a2] = 0.0;
        for (int pass = 0; pass < 40; pass++) render.Invoke(prog, new object[] { a2 });
        var sb = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { a2 });
        return ScopeTest.barInfo2((System.Collections.IList)sb.GetType().GetField("Slots").GetValue(sb), (List<VRage.Game.GUI.TextPanel.MySprite>)sb.GetType().GetField("Sprites").GetValue(sb), 0);
    };
    Check(Math.Abs(sweepFill(0.3) - 0.50) < 0.02, "calibration: a quarter of the way the gauge is at 50% (0 to 100 in the first half): " + sweepFill(0.3).ToString("0.000"));
    Check(Math.Abs(sweepFill(0.6) - 1.00) < 0.02, "calibration: at the midpoint the gauge is at 100%: " + sweepFill(0.6).ToString("0.000"));
    Check(Math.Abs(sweepFill(0.9) - 0.75) < 0.02, "calibration: three quarters of the way it is coming back, at 75%: " + sweepFill(0.9).ToString("0.000"));
    Check(Math.Abs(sweepFill(1.3) - 0.50) < 0.02 && !calib.ContainsKey(a2), "calibration: after 1.2 s it settles on the live 50% and the sweep ends: " + sweepFill(1.3).ToString("0.000"));
    timeField.SetValue(prog, 0.0);

    // Contents ranking: text(bat.top1) shows the largest item summed over all of the bind's inventories.
    Func<string, string, double, VRage.Game.ModAPI.Ingame.MyInventoryItem> item = (t, sub, amount) => new VRage.Game.ModAPI.Ingame.MyInventoryItem { Type = new VRage.Game.ModAPI.Ingame.MyItemType(t, sub), Amount = amount };
    var savedInvs = mBat.Invs;
    mBat.Invs = new[] { new FInv(), new FInv() };
    mBat.Invs[0].Items.AddRange(new[] { item("MyObjectBuilder_Ingot", "Iron", 100), item("MyObjectBuilder_Component", "SteelPlate", 30), item("MyObjectBuilder_Ore", "Ice", 5) });
    mBat.Invs[1].Items.Add(item("MyObjectBuilder_Ingot", "Iron", 50));
    timeField.SetValue(prog, 1000.0);
    for (int k = 0; k < 12; k++) prog.Main("", UpdateType.Update10);
    var topScreen = cfg0.Screens[cfg0.Screens.Count - 2];
    for (int pass = 0; pass < 3; pass++) render.Invoke(prog, new object[] { topScreen });
    var tb = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { topScreen });
    var tslots = (System.Collections.IList)tb.GetType().GetField("Slots").GetValue(tb);
    var tsprites = (List<VRage.Game.GUI.TextPanel.MySprite>)tb.GetType().GetField("Sprites").GetValue(tb);
    Func<int, string> rank = n => tsprites[(int)tslots[n].GetType().GetField("ValSpriteIndex").GetValue(tslots[n])].Data;
    Check(rank(0) == "Iron ingot 150", "top items: rank 1 is the iron ingots summed over both inventories (100 + 50): " + rank(0));
    Check(rank(1) == "SteelPlate 30" && rank(2) == "Ice ore 5", "top items: ranks 2 and 3 follow, ordered by amount: " + rank(1) + " / " + rank(2));
    Check(rank(3) == "-", "top items: a rank with no item shows a dash: " + rank(3));
    mBat.Invs = savedInvs;
    timeField.SetValue(prog, 0.0);

    // probe: a test pattern on the block's own screen and on every screen, with the corner squares exactly at the viewport corners.
    prog.Main("probe", UpdateType.Terminal);
    var pbOff = new VRageMath.Vector2((pb.TextureSize.X - pb.SurfaceSize.X) / 2f, (pb.TextureSize.Y - pb.SurfaceSize.Y) / 2f);
    Func<VRageMath.Vector2, bool> squareAt = p => pb.Last.Any(sp => sp.Data == "SquareSimple" && sp.Size.HasValue && sp.Size.Value.X == 32f && Math.Abs(sp.Position.Value.X - p.X) < 0.5f && Math.Abs(sp.Position.Value.Y - p.Y) < 0.5f);
    Check(squareAt(new VRageMath.Vector2(pbOff.X + 16f, pbOff.Y + 16f)) && squareAt(new VRageMath.Vector2(pbOff.X + pb.SurfaceSize.X - 16f, pbOff.Y + pb.SurfaceSize.Y - 16f)), "probe: coloured corner squares sit exactly at the viewport corners of the block's screen");
    Check(pb.Last.Any(sp => sp.Data == "SURFACE 512x320") && pb.Last.Any(sp => sp.Data != null && sp.Data.StartsWith("TEXTURE ")), "probe: the surface and texture sizes are printed");
    Check(pb.Last.Count(sp => sp.Data == "SquareSimple" && sp.Color.Value.R == 150) == 4, "probe: a gray frame marks the texture edges");
    // starter: fills an empty Custom Data with a layout that parses, and never overwrites one that has content.
    var me2 = new FBlock { Ship = mine, CustomName = "Programmable Block", CustomData = "" };
    var gts2 = new FGts(); gts2.All.Add(me2);
    MyGridProgram.BootstrapMe = me2; MyGridProgram.BootstrapGts = gts2;
    var prog2 = new Program();
    prog2.Main("starter", UpdateType.Terminal);
    Check(me2.CustomData.Contains("@screen Demo") && me2.CustomData.Contains("@bind batt"), "starter: an empty Custom Data receives the sample layout");
    Check(((ParsedConfig)Dump.Get(prog2, "_config")).Screens.Count == 1 && ((ParsedConfig)Dump.Get(prog2, "_config")).Errors.Count == 0, "starter: the sample layout parses with one screen and no errors");
    me2.CustomData = "# keep me\n";
    prog2.Main("starter", UpdateType.Terminal);
    Check(me2.CustomData == "# keep me\n", "starter: a Custom Data that has content is never overwritten");

    // Sprite alignment: the game documents Alignment as "for the text and sprites", and the default (LEFT) anchors a texture by
    // its left edge. Every texture sprite must be CENTER so Position is its centre; otherwise the background covers half the screen.
    var emptyScreen = cfg0.Screens[cfg0.Screens.Count - 3];
    for (int pass = 0; pass < 2; pass++) render.Invoke(prog, new object[] { emptyScreen });
    Check(eLcd.Last.Count > 0, "first frame: a screen with no gauge lines is drawn once after it is built, not left showing the boot frame: " + eLcd.Last.Count);
    Func<IEnumerable<VRage.Game.GUI.TextPanel.MySprite>, int> notCentred = list => list.Count(sp => sp.Type == VRage.Game.GUI.TextPanel.SpriteType.TEXTURE && sp.Alignment != VRage.Game.GUI.TextPanel.TextAlignment.CENTER);
    Check(notCentred(eLcd.Last) == 0 && notCentred(tex) == 0 && notCentred(grad) == 0, "alignment: every texture sprite of the screens is CENTER");
    prog.Main("probe", UpdateType.Terminal);
    Check(notCentred(pb.Last) == 0 && pb.Last.Count > 0, "alignment: every texture sprite of the probe is CENTER");
    Check(pb.ScriptBackgroundColor.R == 0 && pb.ScriptBackgroundColor.G == 0 && pb.ScriptBackgroundColor.B == 0, "block screen: the game's own script background is black, so nothing uncovered shows a default colour");

    // Tanks and vertical bars: three columns (half tank, full gradient tank, vertical bar) in one panel.
    var tkScreen = cfg0.Screens.First(x => x.Name == "Tk");
    timeField.SetValue(prog, 3.0);
    for (int pass = 0; pass < 40; pass++) render.Invoke(prog, new object[] { tkScreen });
    var tkb = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { tkScreen });
    var tks = (System.Collections.IList)tkb.GetType().GetField("Slots").GetValue(tkb);
    var tksp = (List<VRage.Game.GUI.TextPanel.MySprite>)tkb.GetType().GetField("Sprites").GetValue(tkb);
    Func<int, string, object> slotField = (n, f) => tks[n].GetType().GetField(f).GetValue(tks[n]);
    Func<int, List<int>> extras = n => (List<int>)slotField(n, "ExtraSpriteIndices");
    Func<int, double> liquidHeight = n => extras(n).Take(8).Sum(ix => (double)tksp[ix].Size.Value.Y);
    double tubeH = (float)slotField(0, "BarHeight");
    Check(Math.Abs(liquidHeight(0) / tubeH - 0.5) < 0.01, "tank: 150 of max(300) holds half a tube of liquid: " + (liquidHeight(0) / tubeH).ToString("0.000"));
    Check(Math.Abs(liquidHeight(1) / tubeH - 1.0) < 0.01, "tank: 150 of max(150) is a full tube: " + (liquidHeight(1) / tubeH).ToString("0.000"));
    Check(tksp[extras(0)[8]].Data.Contains("~") && tksp[extras(0)[8]].Data.Length > 5, "tank: the wave line of characters is drawn on the surface: '" + tksp[extras(0)[8]].Data + "'");
    Check(new[] { 9, 10, 11 }.All(k => tksp[extras(0)[k]].Data == "o" || tksp[extras(0)[k]].Data == "O"), "tank: three bubbles are drawn inside the liquid");
    var bandLow = tksp[extras(1)[0]].Color.Value; var bandHigh = tksp[extras(1)[7]].Color.Value;
    Check(bandLow.R > 200 && bandLow.G < 60 && bandHigh.G > 200 && bandHigh.R < 60, "tank: a gradient runs through the liquid, red at the bottom to green at the top");
    var vf = tksp[(int)slotField(2, "FillSpriteIndex")];
    float vBottom = (float)slotField(2, "BarCenterY") + (float)slotField(2, "BarHeight") / 2f;
    Check(Math.Abs(vf.Size.Value.Y / tubeH - 0.5) < 0.01 && Math.Abs((vf.Position.Value.Y + vf.Size.Value.Y / 2f) - vBottom) < 1f, "vertical bar: fills half the height and sits on the bottom");
    var cols = Enumerable.Range(0, 3).Select(n => (float)slotField(n, "BarLeft")).ToList();
    Check(cols[0] < cols[1] && cols[1] < cols[2], "vertical panel: the three lines are columns, left to right");

    // Sizes and conditional gradients: one large, one medium and one normal bar share a panel by 4 : 2 : 1.
    var szScreen = cfg0.Screens.First(x => x.Name == "Sz");
    for (int pass = 0; pass < 40; pass++) render.Invoke(prog, new object[] { szScreen });
    var szb = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { szScreen });
    var szs = (System.Collections.IList)szb.GetType().GetField("Slots").GetValue(szb);
    var szsp = (List<VRage.Game.GUI.TextPanel.MySprite>)szb.GetType().GetField("Sprites").GetValue(szb);
    Func<int, float> barH = n => (float)szs[n].GetType().GetField("BarHeight").GetValue(szs[n]);
    Func<int, float> midY = n => (float)szs[n].GetType().GetField("BarCenterY").GetValue(szs[n]);
    Check(barH(0) > barH(1) * 1.5f && barH(1) > barH(2), "size: the large bar is taller than the medium one, which is taller than the normal one: " + barH(0) + " " + barH(1) + " " + barH(2));
    Check(Math.Abs((midY(1) - midY(0)) / (midY(2) - midY(1)) - 2f) < 0.3f, "size: rows take 4, 2 and 1 shares (centres " + midY(0) + ", " + midY(1) + ", " + midY(2) + ")");
    var bigFill = szsp[(int)szs[0].GetType().GetField("FillSpriteIndex").GetValue(szs[0])].Color.Value;
    Check(bigFill.R > 100 && bigFill.B > 100 && bigFill.G < 100, "conditional gradient: 150 of 300 is over 40, so the red/blue gradient at 50% is used: " + bigFill.R + "," + bigFill.G + "," + bigFill.B);
    var bigLabel = szsp[(int)szs[0].GetType().GetField("LabelSpriteIndex").GetValue(szs[0])].Color.Value;
    Check(bigLabel.G > 200 && bigLabel.R < 50, "label_color: the label is green: " + bigLabel);
    var trackQ = szsp[(int)szs[0].GetType().GetField("FillSpriteIndex").GetValue(szs[0]) - 1].Color.Value;
    Check(trackQ.R == 0x12 && trackQ.B == 0x56, "track: the empty part of the bar is #123456: " + trackQ);

    var plain = szsp[(int)szs[3].GetType().GetField("FillSpriteIndex").GetValue(szs[3])].Color.Value;
    var byOther = szsp[(int)szs[4].GetType().GetField("FillSpriteIndex").GetValue(szs[4])].Color.Value;
    Check(plain.G > 150 && plain.R < 100 && byOther.R > 200 && byOther.G < 120, "color_by: the same bar turns red when bat.percent is under 10, while the plain one follows its own value (50) and stays green: " + plain.R + "," + plain.G + " / " + byOther.R + "," + byOther.G);
    var ledOff = szsp[(int)((System.Collections.Generic.List<int>)szs[5].GetType().GetField("ExtraSpriteIndices").GetValue(szs[5]))[9]].Color.Value;
    Check(ledOff.R == 0x12 && ledOff.B == 0x56, "track: an unlit LED segment takes the track colour: " + ledOff.R + "," + ledOff.G + "," + ledOff.B);

    // Frames: a screen with border: double and a panel with border: rounded.
    var frScreen = cfg0.Screens.First(x => x.Name == "Fr");
    for (int pass = 0; pass < 3; pass++) render.Invoke(prog, new object[] { frScreen });
    var frb = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { frScreen });
    var frsp = (List<VRage.Game.GUI.TextPanel.MySprite>)frb.GetType().GetField("Sprites").GetValue(frb);
    int sides = frsp.Count(m => m.Data == "SquareSimple" && m.Size.HasValue && m.Size.Value.X <= 2.01f && m.Size.Value.Y > 200f);
    Check(sides >= 4, "border: double draws the sides of two frames (" + sides + " tall lines)");
    Check(frsp.Count(m => m.Data == "Circle" && m.Size.HasValue && m.Size.Value.X < 10f) == 4, "panel border: rounded draws four corner circles");
    Check(frsp.Count(m => m.Data == "SquareSimple" && m.Size.HasValue && m.Size.Value.X <= 2.01f && m.Size.Value.Y > 200f) >= 6, "the panel frame adds its own sides");

    Check(((System.Collections.Generic.List<int>)szs[6].GetType().GetField("ExtraSpriteIndices").GetValue(szs[6])).Count == 20, "segments(20): a led bar of twenty segments");
    Check(((System.Collections.Generic.List<int>)szs[7].GetType().GetField("ExtraSpriteIndices").GetValue(szs[7])).Count == 24 && ((float[])szs[7].GetType().GetField("SparkHistory").GetValue(szs[7])).Length == 24, "points(24): a sparkline of 24 columns with 24 samples of history");
    var drsL = (float)szs[8].GetType().GetField("BarLeft").GetValue(szs[8]); var drsW = (float)szs[8].GetType().GetField("BarMaxWidth").GetValue(szs[8]); var drsH = (float)szs[8].GetType().GetField("BarHeight").GetValue(szs[8]);
    var drsFill = szsp[(int)szs[8].GetType().GetField("FillSpriteIndex").GetValue(szs[8])];
    Check(Math.Abs(drsFill.Position.Value.X + drsFill.Size.Value.X / 2f - (drsL + drsW)) < 0.5f && Math.Abs(drsFill.Size.Value.X - drsW / 2f) < 1f, "dir: left: a half bar fills from the right end");
    Check(szsp.Count(m => m.Size.HasValue && m.Size.Value.X == 1f && Math.Abs(m.Size.Value.Y - drsH) < 0.01f) == 3, "ticks(4): three division lines");
    var marks = szsp.Where(m => m.Size.HasValue && m.Size.Value.X == 2f && Math.Abs(m.Size.Value.Y - (drsH + 6f)) < 0.01f).Select(m => m.Position.Value.X).OrderBy(x => x).ToList();
    Check(marks.Count == 2 && Math.Abs(marks[0] - (drsL + drsW * 0.2f)) < 0.5f && Math.Abs(marks[1] - (drsL + drsW * 0.75f)) < 0.5f, "marker(25, 80) on a bar that runs left: ticks at 75% and 20% from the left: " + string.Join(",", marks));
    Check(szsp.Any(m => m.Color.HasValue && m.Color.Value.R == 255 && m.Color.Value.B == 255 && m.Color.Value.G == 0), "outline(#ff00ff) draws a frame around the bar");

    // Mirror: a 256x256 screen copies the 512x512 tank screen at half scale.
    var miScreen = cfg0.Screens.First(x => x.Name == "Mi");
    Check(miScreen.Mirror == "tk", "mirror: parsed");
    render.Invoke(prog, new object[] { miScreen });
    Check(miLcd.Last.Count == tksp.Count && tksp.Count > 10, "mirror: every sprite of the source is copied: " + miLcd.Last.Count + " of " + tksp.Count);
    var srcBg = tksp[(int)tkb.GetType().GetField("BgIndex").GetValue(tkb)]; var mirBg = miLcd.Last[(int)tkb.GetType().GetField("BgIndex").GetValue(tkb)];
    Check(Math.Abs(mirBg.Size.Value.X - srcBg.Size.Value.X / 2f) < 0.5f && Math.Abs(mirBg.Position.Value.X - srcBg.Position.Value.X / 2f) < 0.5f, "mirror: sizes and positions are halved: " + mirBg.Size + " from " + srcBg.Size);
    Check(miLcd.Last.Where(m => m.Data != null).All(m => m.RotationOrScale <= tksp.Where(t => t.Data != null).Max(t => t.RotationOrScale) / 2f + 0.001f), "mirror: text is scaled down with the rest");
    var mpScreen = cfg0.Screens.First(x => x.Name == "Mp");
    Check(mpScreen.MirrorPanel == "t", "mirror_panel: parsed");
    render.Invoke(prog, new object[] { mpScreen });
    Check(mpLcd.Last.Count > 5 && mpLcd.Last.Count < tksp.Count, "mirror_panel: only the panel's sprites are copied: " + mpLcd.Last.Count + " of " + tksp.Count);
    miLcd.Last.Clear(); render.Invoke(prog, new object[] { miScreen });
    Check(miLcd.Last.Count == 0, "mirror: nothing is redrawn while the source has not changed");
    // Text of another LCD: text(note.lineN) shows line N of the text panel the bind names.
    var ntScreen = cfg0.Screens.First(x => x.Name == "Nt");
    timeField.SetValue(prog, 2000.0);
    for (int k = 0; k < 4; k++) prog.Main("", UpdateType.Update10);
    for (int pass = 0; pass < 3; pass++) render.Invoke(prog, new object[] { ntScreen });
    var ntb = bufs.GetType().GetProperty("Item").GetValue(bufs, new object[] { ntScreen });
    var nts = (System.Collections.IList)ntb.GetType().GetField("Slots").GetValue(ntb);
    var ntsp = (List<VRage.Game.GUI.TextPanel.MySprite>)ntb.GetType().GetField("Sprites").GetValue(ntb);
    Func<int, string> noteLine = n => ntsp[(int)nts[n].GetType().GetField("ValSpriteIndex").GetValue(nts[n])].Data;
    Check(noteLine(0) == "alpha" && noteLine(1) == "beta" && noteLine(2) == "gamma", "note lines: " + noteLine(0) + " | " + noteLine(1) + " | " + noteLine(2));
    Check(noteLine(3) == "", "a line the text panel does not have is empty: '" + noteLine(3) + "'");
    var cyanTxt = ntsp[(int)nts[0].GetType().GetField("ValSpriteIndex").GetValue(nts[0])].Color.Value;
    Check(cyanTxt.B > 200 && cyanTxt.R < 80, "a text line takes its flat colour: " + cyanTxt);
    var ruleBar = ntsp[(int)nts[4].GetType().GetField("FillSpriteIndex").GetValue(nts[4])].Color.Value;
    Check(ruleBar.R > 200 && ruleBar.G < 60, "a bar takes the colour of its color(...) rules: " + ruleBar);
    noteSrc.Text = "changed";
    timeField.SetValue(prog, 2010.0);
    for (int k = 0; k < 4; k++) prog.Main("", UpdateType.Update10);
    for (int pass = 0; pass < 3; pass++) render.Invoke(prog, new object[] { ntScreen });
    Check(noteLine(0) == "changed", "the note follows the text panel: " + noteLine(0));

    timeField.SetValue(prog, 0.0);

    Console.WriteLine(fails == 0 ? "ALL PASS" : fails + " FAILED");
    return fails;
  }
}
