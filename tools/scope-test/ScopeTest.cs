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
    var fLcd = new FLcd(dock, "Cockpit"); var mLcd = new FLcd(mine, "Cockpit"); var aLcd = new FLcd(mine, "AmmoLcd"); var gLcd = new FLcd(mine, "GradLcd"); var nLcd = new FLcd(mine, "NoneLcd"); var rLcd = new FLcd(mine, "RefLcd"); var cLcd = new FLcd(mine, "CkLcd"); var vLcd = new FLcd(mine, "ValLcd"); var tLcd = new FLcd(mine, "TexLcd"); var qLcd = new FLcd(mine, "StepLcd"); var bLcd = new FLcd(mine, "BorderLcd"); var pLcd = new FLcd(mine, "TopLcd"); var conn = new FConn { Ship = mine, CustomName = "Dock Connector", Status = MyShipConnectorStatus.Connected }; var hdoor = new FDoor { Ship = mine, CustomName = "Hangar Door", OpenRatio = 0.5f };
    mBat.Invs = new[] { new FInv { Iron = 100, Ice = 5 }, new FInv { Iron = 50 } };
    mTank.Invs = new[] { new FInv { Iron = 25 } };
    fBat.Invs = new[] { new FInv { Iron = 9999 } };
    var gts = new FGts();
    gts.All.AddRange(new IMyTerminalBlock[] { fBat, fTank, fLcd, mBat, mTank, mLcd, aLcd, gLcd, nLcd, rLcd, cLcd, vLcd, tLcd, qLcd, bLcd, pLcd, conn, hdoor, me });
    var gDock = new FGroup { Name = "Fleet" }; gDock.Blocks.Add(fBat); gDock.Blocks.Add(fTank);
    var gMine = new FGroup { Name = "Fleet" }; gMine.Blocks.Add(mBat); gMine.Blocks.Add(mTank);
    gts.Groups.Add(gDock); gts.Groups.Add(gMine);
    var gOnlyDock = new FGroup { Name = "DockOnly" }; gOnlyDock.Blocks.Add(fBat); gts.Groups.Add(gOnlyDock);
    me.CustomData = "@bind bat = block(\"Main Battery\")\n@bind tank = block(\"H2 Tank\")\n@bind fleet = group(\"Fleet\", agg: avg)\n@bind wild = group(\"*Tank*\", agg: avg)\n@bind lost = group(\"DockOnly\", agg: avg)\n@bind none = block(\"Nothing Here\")\n@bind fleetsum = group(\"Fleet\", agg: sum)\n@bind conn = block(\"Dock Connector\")\n@bind hdoor = block(\"Hangar Door\")\n@bind dock = group(\"Fleet\", agg: sum, scope: all)\n@bind dockwild = group(\"*Tank*\", agg: sum, scope: all)\n@screen S\noutput: \"Cockpit\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"T\"\nline: \"B\" bar(bat.percent) flat(cyan)\n@screen A2\noutput: \"AmmoLcd\", 0\nlayout: grid(1, 1)\n@panel q at(0, 0, 1, 1) title: \"T\"\nline: \"HALF\" bar(bat.ingot:Iron) max(300) flat(cyan)\nline: \"OVER\" bar(bat.ingot:Iron) max(100) flat(cyan)\nline: \"DYN \" bar(bat.ingot:Iron) max(fleetsum.ingot:Iron) flat(cyan)\n@screen G\noutput: \"GradLcd\", 0\nlayout: grid(2, 1)\nbg_gradient(#000000, #ffffff)\npanel_bg: #112233\n@panel a at(0, 0, 1, 1) title: \"A\"\nline: \"X\" bar(bat.percent) flat(cyan)\n@panel b at(1, 0, 1, 1) title: \"B\"\nline: \"Y\" bar(bat.percent) flat(cyan)\n@screen N\noutput: \"NoneLcd\", 0\nlayout: grid(2, 1)\nbg: #102030\npanel_bg: none\nalert_bg: #ff0000\n@panel a at(0, 0, 1, 1) title: \"A\"\nline: \"X\" bar(bat.percent) flat(cyan)\n@panel b at(1, 0, 1, 1) title: \"B\"\nline: \"Y\" bar(bat.percent) flat(cyan)\n@screen C\noutput: \"CkLcd\", 0\nlayout: grid(1, 1)\n@panel c at(0, 0, 1, 1) title: \"C\"\nline: \"LINK\" checkbox(conn.connected) flat(cyan)\nline: \"DOOR\" bar(hdoor.open) flat(cyan)\n@screen V\noutput: \"ValLcd\", 0\nlayout: grid(1, 1)\n@panel v at(0, 0, 1, 1) title: \"V\"\nbg: color(bat.ingot:Iron > 100 : #ff0000, default: #00ff00)\nflash: bat.ingot:Iron > 100\nline: \"X\" bar(bat.percent) flat(cyan)\n@screen T\noutput: \"TexLcd\", 0\nlayout: grid(1, 1)\nbg_gradient(#000000, #ffffff)\nbg_gradient_dir: horizontal\nbg_texture: Grid\n@panel t at(0, 0, 1, 1) title: \"T\"\nline: \"X\" bar(bat.percent) flat(cyan)\n@screen Q\noutput: \"StepLcd\", 0\nlayout: grid(1, 1)\ngradient_mode: steps\n@panel q at(0, 0, 1, 1) title: \"Q\"\nline: \"GRAD\" bar(bat.ingot:Iron) max(300) gradient(#000000, #ffffff)\n@screen B\noutput: \"BorderLcd\", 0\nlayout: grid(1, 1)\nborder_gradient(#ff0000, #0000ff)\n@panel b at(0, 0, 1, 1) title: \"B\"\nline: \"GRAD\" bar(bat.ingot:Iron) max(300) gradient(#000000, #ffffff)\n@screen P\noutput: \"TopLcd\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"P\"\nline: \"1\" text(bat.top1) flat(cyan)\nline: \"2\" text(bat.top2) flat(cyan)\nline: \"3\" text(bat.top3) flat(cyan)\nline: \"4\" text(bat.top4) flat(cyan)\n@screen R\noutput: \"RefLcd\", 0\nlayout: grid(1, 1)\nrefresh: 600\n@panel r at(0, 0, 1, 1) title: \"R\"\nline: \"X\" bar(bat.percent) flat(cyan)\n";
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
    Check(text.Contains("screens " + cfg0.Screens.Count) && text.Contains("binds 9/11"), "console: all screens listed; 9 of 11 binds matched a block");
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

    Console.WriteLine(fails == 0 ? "ALL PASS" : fails + " FAILED");
    return fails;
  }
}
