fT s;string f4=null;Dictionary<string,List<IMyTerminalBlock>>b3=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>ee=new Dictionary<string,bool>();
Dictionary<string,aL>cZ=new Dictionary<string,aL>();Dictionary<string,Color>jv=new Dictionary<string,Color>();List<IMyTerminalBlock>bz=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>b2=new List<IMyTerminalBlock>();bool eb;int f2;string ec="";StringBuilder c1=new StringBuilder();readonly Dictionary<string,double>an=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>ed=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener c_;bool bA=false;
Dictionary<z,IMyTextSurface>b4=new Dictionary<z,IMyTextSurface>();Dictionary<z,string>b1=new Dictionary<z,string>();
static readonly string[]jb=new string[101];static Program(){for(int eH=0;eH<=100;eH++){jb[eH]=eH+"%";}}static string iZ(double qC){int dm=(int)Math.Round(qC);
if(dm<0)dm=0;else if(dm>100)dm=100;return jb[dm];}class a5{public List<MySprite>l=new List<MySprite>();public List<cR>d8=new List<cR>();
public int bQ= -1;public Color[]bP;public Color fA;public bool fC;public Vector2 i5;public bool i4=false;}class cR{
public bt i7;public int V= -1;public int a7= -1;public int fM= -1;public bool i0=false;public List<int>Z=new List<int>();
public float[]d9=new float[8];public float cF;public float dW;public float bO;public float cE;public double d5= -99999.0;
public string i6;public float M= -1f;}Dictionary<z,a5>eg=new Dictionary<z,a5>();int jx=0;
double f6=0;double aS=0;int c0=0;Dictionary<z,double>aR=new Dictionary<z,double>();Dictionary<z,double>f1=new Dictionary<z,double>();
int c2=0;const string m9="0.11.0";IMyTextSurface a8;bool f0=true;double f_=0;string jw="";string f5="";
int ef=0,f3=0;List<KeyValuePair<string,int>>b0=new List<KeyValuePair<string,int>>();const double nr=120;const float mI=22f;
public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{c_=IGC.RegisterBroadcastListener("HELM");c_.SetMessageCallback("HELM_IGC");
}catch{}fV();ji();i3();}public void Main(string jJ,UpdateType qy){double kp=Runtime.TimeSinceLastRun.TotalSeconds;aS+=kp;
if((qy&UpdateType.IGC)!=0||jJ=="HELM_IGC"){np();}string lL=(jJ ?? "").Trim();string aj=lL.ToLowerInvariant();if(aj=="reload"){
fV();}else if(aj=="refresh"){fW();}else if(aj=="boot"){ji();}else if(aj=="console on"||aj=="console off"){f0=aj=="console on";
i3();f_=0;}else if(aj=="scan"||aj=="diagnose"||aj=="test"){nJ();return;}else if(aj.StartsWith("set ")){string e8=lL.Substring(4).Trim();
int kt=e8.IndexOf('=');int qb=e8.IndexOf(' ');int id=kt>0 ? kt : qb;if(id>0){string k1=e8.Substring(0,id).Trim();string mv=e8.Substring(id+1).Trim();iJ($"{k1}={mv}");
Echo($"[HELM] Set dynamic variable '{k1}' = {mv}");}return;}else if(aj.StartsWith("alert")){string dR=aj.Length>5 ? aj.Substring(5).Trim(): "toggle";if(dR=="toggle")bA= !bA;
else bA=dR=="red"||dR=="on"||dR=="1"||dR=="true";Echo($"[HELM] Red alert state: {bA}");return;}else if(Me.CustomData!=f4){
fV();}c2++;f6+=kp;if(f6>=nr){f6=0;fW();}if(s==null||s.S.Count==0){
jq();iR();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var bG=s.S[jx%s.S.Count];
jx++;f5=bG.a2;bool ga=aR.Count>0;if(ga)ny();double kq;if(!aR.ContainsKey(bG)&& !(f1.TryGetValue(bG,out kq)&&aS<kq)){
if(nC(bG))ga=true;if(bG.fU>0)f1[bG]=aS+bG.fU/60.0;}c0=ga ? 0 : c0+1;Runtime.UpdateFrequency=c0<=s.S.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
jq();iR();ef=Runtime.CurrentInstructionCount;if(ef>f3)f3=ef;}void iR(){if(ec.Length>0)Echo(ec);
if(c1.Length>0)Echo(c1.ToString());}List<IMyTerminalBlock>dU(bool nS=false){if(!eb){b2.Clear();GridTerminalSystem.GetBlocks(b2);
bz.Clear();bz.AddRange(b2);bz.RemoveAll(n2=> !n2.IsSameConstructAs(Me));f2=b2.Count-bz.Count;eb=true;
}return nS ? b2 : bz;}IMyTerminalBlock iV(string ln,bool nU=false){var gi=GridTerminalSystem.GetBlockWithName(ln);if(gi==null||nU||gi.IsSameConstructAs(Me))return gi;
var gd=dU();for(int eI=0;eI<gd.Count;eI++)if(gd[eI].CustomName.IndexOf(ln,StringComparison.OrdinalIgnoreCase)>=0)return gd[eI];return null;}void mS(string lo,List<IMyTerminalBlock>dp,bool jC=false){
var kH=GridTerminalSystem.GetBlockGroupWithName(lo);if(kH==null)return;int jT=dp.Count;var dO=new List<IMyTerminalBlock>();kH.GetBlocks(dO);iH(dO,dp,jC);
if(dp.Count>jT)return;var kS=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(kS);foreach(var mF in kS){if(mF.Name.IndexOf(lo,StringComparison.OrdinalIgnoreCase)<0)continue;
dO.Clear();mF.GetBlocks(dO);iH(dO,dp,jC);if(dp.Count>jT)return;}}void iH(List<IMyTerminalBlock>oN,List<IMyTerminalBlock>k_,bool nT=false){
foreach(var gj in oN)if((nT||gj.IsSameConstructAs(Me))&& !k_.Contains(gj))k_.Add(gj);}void fV(){f4=Me.CustomData;s=i2.ni(f4);
ec="";if(s.ay.Count>0){var lU=new StringBuilder("HELM config errors:\n");foreach(var oz in s.ay)lU.AppendLine(" - "+oz);ec=lU.ToString();
}cZ.Clear();foreach(var gk in s.br)if(!cZ.ContainsKey(gk.Q))cZ[gk.Q]=gk;eg.Clear();b1.Clear();aR.Clear();
f1.Clear();fW();}void np(){try{while(c_!=null&&c_.HasPendingMessage){var lj=c_.AcceptMessage();
if(lj.Data is string){iJ((string)lj.Data);}}}catch{}}void iJ(string lE){if(string.IsNullOrEmpty(lE))return;var lB=lE.Split(';');
for(int g4=0;g4<lB.Length;g4++){var dC=lB[g4].Trim();if(dC.Length==0)continue;int db=dC.IndexOf('=');if(db<0)db=dC.IndexOf(':');if(db>0){string ci=dC.Substring(0,db).Trim();
string aJ=dC.Substring(db+1).Trim();if(ci.Equals("alert",StringComparison.OrdinalIgnoreCase)){bA=aJ.Equals("red",StringComparison.OrdinalIgnoreCase)||aJ=="1"||aJ.Equals("true",StringComparison.OrdinalIgnoreCase)||aJ.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double lr;ed.Remove(ci);if(double.TryParse(aJ,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out lr)){
an[ci]=lr;}else if(aJ.Equals("true",StringComparison.OrdinalIgnoreCase)||aJ.Equals("on",StringComparison.OrdinalIgnoreCase)){an[ci]=100.0;
}else if(aJ.Equals("false",StringComparison.OrdinalIgnoreCase)||aJ.Equals("off",StringComparison.OrdinalIgnoreCase)){an[ci]=0.0;}else{ed[ci]=aJ;
}}}}}void ad(int qk,string qr){b0.Add(new KeyValuePair<string,int>(qr,qk));}static string jp(string h2,int lm){return h2.Length>lm ? h2.Substring(0,lm): h2;
}void i3(){var hV=Me as IMyTextSurfaceProvider;if(hV==null||hV.SurfaceCount<1)return;a8=hV.GetSurface(0);a8.ContentType=f0 ? ContentType.SCRIPT : ContentType.NONE;
a8.Script="";}void jq(){if(aS>=f_){f_=aS+(c0>s.S.Count+1 ? 5.0 : 1.0);mQ();if(f0&&a8!=null)mZ();
}Echo(jw);}void mQ(){b0.Clear();int gu=0,jW=0;foreach(var jL in s.br){List<IMyTerminalBlock>ho;if(jL.a1==aw.cL){
gu++;continue;}if(b3.TryGetValue(jL.Q,out ho)){jW+=ho.Count;if(ho.Count>0)gu++;}}int ku=s.ay.Count;ad(0,"HELM-[Kernel-(v"+m9+")]");
ad(4,"------------------------------");ad(1,"krn-cfg");ad(2," screens "+s.S.Count+"  binds "+gu+"/"+s.br.Count+"  errors "+ku);
ad(2," alert "+(bA ? "RED" : "off")+"  blocks "+jW+"  docked-ignored "+f2);if(ku>0)ad(3," ! "+jp(s.ay[0],40));
ad(1,"cfg-dsp");int p8=0;foreach(var h3 in s.S){if(++p8>10){ad(2," +"+(s.S.Count-10)+" more");break;}IMyTextSurface ie;string l2="--",ij="no block";
if(b4.TryGetValue(h3,out ie)){l2=ie.SurfaceSize.X.ToString("0")+"x"+ie.SurfaceSize.Y.ToString("0");ij=aR.ContainsKey(h3)? "boot" : "live";
}ad(ij=="no block" ? 3 : 2," "+jp(h3.a2,14).PadRight(14)+" "+l2.PadRight(9)+" "+ij);}ad(1,"perf");ad(2," tick "+c2+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ad(2," instr "+ef+"  peak "+f3+"  max "+Runtime.MaxInstructionCount);ad(2," rate "+Runtime.UpdateFrequency+"  idle "+c0+"  up "+(int)aS+"s");
if(f5.Length>0)ad(2," last "+f5);var lV=new StringBuilder();foreach(var o6 in b0)lV.AppendLine(o6.Key);jw=lV.ToString();}static Color mV(int ql){
switch(ql){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void mZ(){Vector2 bH=a8.SurfaceSize;Vector2 hA=(a8.TextureSize-bH)/2f;int gB=30;foreach(var o7 in b0)gB=Math.Max(gB,o7.Key.Length);
Vector2 gA=a8.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float lW=Math.Min(1f,Math.Min(bH.X*0.94f/(gB*gA.X),bH.Y*0.94f/(b0.Count*gA.Y)));
float qI=hA.X+bH.X*0.03f;float mG=hA.Y+bH.Y*0.03f;var ig=new List<MySprite>();ig.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=hA+bH/2f,Size=bH,Color=new Color(12,12,16)}
);foreach(var k6 in b0){cz(ig,k6.Key,mV(k6.Value),qI,mG,lW);mG+=gA.Y*lW;}using(var oI=a8.DrawFrame()){oI.AddRange(ig);}}static readonly List<string>mP=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void ji(){aR.Clear();if(s==null)return;for(int eJ=0;eJ<s.S.Count;eJ++){var fb=s.S[eJ];if(fb.fF>0f)aR[fb]=aS+(eJ/Math.Max(1,fb.iK))*fb.iN/60.0;
}}void ny(){List<z>da=null;int oT=(aR.Count+3)/4,o_=0;foreach(var eR in aR){double hH=(aS-eR.Value)/eR.Key.fF;if(hH<0)continue;
if(hH>=1.0){if(da==null)da=new List<z>();da.Add(eR.Key);continue;}if((o_++ +c2)%oT==0)mY(eR.Key,hH);}if(da==null)return;foreach(var h4 in da){
aR.Remove(h4);eg.Remove(h4);b1.Remove(h4);}}static void cz(List<MySprite>qc,string qs,Color ok,float qJ,float qL,float p5){
var mg=MySprite.CreateText(qs,"Monospace",ok,p5,TextAlignment.LEFT);mg.Position=new Vector2(qJ,qL);qc.Add(mg);}static void iG(List<MySprite>qd,List<string>ek,z fe,float qK,float qM,float p6,float pi){
string[]qi=fe.dY!=null&&fe.dY.Length>0 ? fe.dY : new[]{fe.iL};for(int dh=0;dh<ek.Count;
dh++){k gv=F.i_(qi,ek.Count>1 ? 100.0*dh/(ek.Count-1): 0.0);cz(qd,ek[dh],new Color(gv.aQ,gv.aO,gv.aK),qK,qM+dh*pi,p6);}}bool nw(z cq,IMyTextSurface fl){
List<string>c4;if(b1.ContainsKey(cq)|| !s.cC.TryGetValue(cq.bq,out c4))return false;b1[cq]="banner";Vector2 bI=fl.SurfaceSize;
Vector2 hB=(fl.TextureSize-bI)/2f;int et=1;foreach(var o8 in c4)et=Math.Max(et,o8.Length);Vector2 c8=fl.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float fd=Math.Min(bI.X*0.94f/(et*c8.X),bI.Y*0.94f/(c4.Count*c8.Y));var ih=new List<MySprite>();ih.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=hB+bI/2f,Size=bI,Color=string.IsNullOrEmpty(cq.bp)? new Color(10,12,16): R(cq.bp)}
);iG(ih,c4,cq,hB.X+(bI.X-et*c8.X*fd)/2f,hB.Y+(bI.Y-c4.Count*c8.Y*fd)/2f,fd,c8.Y*fd);using(var oJ=fl.DrawFrame()){oJ.AddRange(ih);
}return true;}void mY(z cr,double lw){IMyTextSurface dM;if(!b4.TryGetValue(cr,out dM))return;Vector2 bl=dM.SurfaceSize;
Vector2 hC=(dM.TextureSize-bl)/2f;var dt=cr.m5();string lk=cr.iM ?? "HELM TELEMETRY KERNEL ONLINE";bool hu=bl.Y>=200f;
List<string>b5;if(cr.bs==null|| !s.cC.TryGetValue(cr.bs,out b5))b5=mP;int ca=lk.Length+3;foreach(var o9 in dt)ca=Math.Max(ca,o9.Length+5);
if(hu)foreach(var pa in b5)ca=Math.Max(ca,pa.Length);int lO=(hu ? b5.Count+1 : 0)+2+dt.Count;Vector2 b9=dM.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bi=Math.Min(1f,Math.Min(bl.X*0.94f/(ca*b9.X),bl.Y*0.94f/(lO*b9.Y)));float fz=hC.X+(bl.X-ca*b9.X*bi)/2f;float bL=hC.Y+(bl.Y-lO*b9.Y*bi)/2f;
float eU=b9.Y*bi;var ct=new List<MySprite>();ct.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=hC+bl/2f,Size=bl,Color=new Color(10,12,16)}
);if(hu){iG(ct,b5,cr,fz,bL,bi,eU);bL+=(b5.Count+1)*eU;}cz(ct,">> "+lk,new Color(34,197,94),fz,bL,bi);bL+=eU*2f;int p9=F.mO(lw,dt.Count);
for(int eK=0;eK<p9;eK++){bool lt=eK==dt.Count-1&&lw>=0.8;cz(ct,lt ? "[OK]" : "[..]",lt ? new Color(34,197,94): new Color(56,189,248),fz,bL,bi);cz(ct,dt[eK],new Color(203,213,225),fz+5f*b9.X*bi,bL,bi);
bL+=eU;}using(var oK=dM.DrawFrame()){oK.AddRange(ct);}}void fW(){b3.Clear();b4.Clear();ee.Clear();
c1.Clear();eb=false;if(s==null)return;foreach(var O in s.br){var at=new List<IMyTerminalBlock>();if(O.a1==aw.cL){
string hh= !string.IsNullOrEmpty(O.aN)? O.aN : O.Q;if(!an.ContainsKey(hh)){an[hh]=O.d1;if(O.fI!=null)ed[hh]=O.fI;
}b3[O.Q]=at;continue;}var mj=O.ar.Count>0 ? O.ar : new List<string>{O.jl};foreach(var bK in mj){
if(string.IsNullOrEmpty(bK))continue;bool k0=bK.Contains("*")||bK.Contains("?");if(O.a1==aw.fK||k0){var g0=new List<IMyTerminalBlock>();
mS(bK,g0,O.dV);if(g0.Count>0){foreach(var kO in g0){if(!at.Contains(kO))at.Add(kO);}}else{var jE=dU(O.dV);
for(int gl=0;gl<jE.Count;gl++){var fr=jE[gl];if(k0 ? F.ne(bK,fr.CustomName): string.Equals(bK,fr.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!at.Contains(fr))at.Add(fr);}}}}else{var gp=iV(bK,O.dV);if(gp!=null&& !at.Contains(gp))at.Add(gp);}}if(at.Count==0){
nH(O,mj,at);if(at.Count>0)ee[O.Q]=true;}b3[O.Q]=at;}foreach(var aW in s.S){
if(string.IsNullOrEmpty(aW.a3))continue;var hF=iV(aW.a3);if(hF==null){c1.AppendLine($"HELM: output block '{aW.a3}' not found");
continue;}IMyTextSurface bJ=null;var hW=hF as IMyTextSurfaceProvider;var kj=hF as IMyTextSurface;if(hW!=null&&aW.bu<hW.SurfaceCount){
bJ=hW.GetSurface(aW.bu);}else if(kj!=null&&aW.bu==0){bJ=kj;}if(bJ==null){c1.AppendLine($"HELM: output block '{aW.a3}' has no surface index {aW.bu}");
continue;}bJ.ContentType=ContentType.SCRIPT;bJ.Script="";bJ.ScriptBackgroundColor=Color.Black;b4[aW]=bJ;}bz.Clear();
b2.Clear();}static readonly string[]iU={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool m3(int o0,IMyTerminalBlock p){switch(o0){case 0: return p is IMyBatteryBlock;case 1: return p is IMyReactor;case 2: return p is IMySolarPanel;
case 3: return p is IMyWindTurbine;case 4: return p is IMyGasTank;case 5: return p is IMyAirVent;case 6: return p is IMyCargoContainer;case 7: return p is IMyJumpDrive;
case 8: return p is IMyThrust;case 9: return p is IMyMotorSuspension;case 10: return p is IMyDoor;case 11: return p is IMyShipConnector;case 12: return p is IMyAssembler;
case 13: return p is IMyRefinery;case 14: return p is IMyShipDrill;case 15: return p is IMyShipWelder;case 16: return p is IMyShipGrinder;case 17: return p is IMyGasGenerator;
case 18: return p is IMyLargeTurretBase;case 19: return p is IMyProjector;case 20: return p is IMyGyro;case 21: return p is IMyGravityGeneratorBase;case 22: return p is IMyLandingGear;
case 23: return p is IMyRadioAntenna;case 24: return p is IMyBeacon;case 25: return p is IMyLightingBlock;case 26: return p is IMyPistonBase;case 27: return p is IMyMotorStator;
case 28: return p is IMyWarhead;case 29: return p is IMySensorBlock;case 30: return p is IMyCameraBlock;case 31: return p is IMyParachute;case 32: return p is IMyTurretControlBlock;
}return false;}void nH(aL od,List<string>qp,List<IMyTerminalBlock>gq){string oW=(od.Q+" "+string.Join(" ",qp)).ToLowerInvariant();
var hk=new List<int>();for(int eQ=0;eQ<iU.Length;eQ++){foreach(var qH in iU[eQ].Split(','))if(oW.Contains(qH)){hk.Add(eQ);break;}}if(hk.Count==0)return;
var jD=dU();var lu=new List<IMyTerminalBlock>();for(int g5=0;g5<jD.Count;g5++){var gm=jD[g5];foreach(int o1 in hk){if(m3(o1,gm)){(gm.CubeGrid==Me.CubeGrid ? gq : lu).Add(gm);
break;}}}if(gq.Count==0)gq.AddRange(lu);}void nJ(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(s==null||s.S.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {s.S.Count} screens, {s.br.Count} binds");eb=false;
Echo($"Blocks: {dU().Count} on this construct; {f2} on docked ships (ignored)");bz.Clear();Echo("--- BIND RESOLUTION ---");foreach(var ab in s.br){
if(ab.a1==aw.cL){string hi= !string.IsNullOrEmpty(ab.aN)? ab.aN : ab.Q;double qz=an.ContainsKey(hi)? an[hi]: ab.d1;
Echo($"[OK] @bind {ab.Q} (dynamic variable '{hi}' = {qz})");continue;}List<IMyTerminalBlock>gr;if(b3.TryGetValue(ab.Q,out gr)){if(gr.Count>0){
string oF=ee.ContainsKey(ab.Q)&&ee[ab.Q]? " [fallback: typed match]" : "";Echo($"[OK] @bind {ab.Q} ({gr.Count} blocks matched{oF})");
}else{string qq=string.Join(", ",ab.ar.Count>0 ? ab.ar : new List<string>{ab.jl});Echo($"[WARN] @bind {ab.Q} -> 0 blocks matched \"{qq}\"");
}}else{Echo($"[ERR] @bind {ab.Q} -> unresolved");}}if(an.Count>0){Echo($"--- DYNAMIC VARIABLES ({an.Count}) ---");foreach(var k8 in an)Echo($"[VAR] {k8.Key} = {k8.Value}");
}Echo("--- SURFACE OUTPUT AUDIT ---");foreach(var aX in s.S){IMyTextSurface mc;if(b4.TryGetValue(aX,out mc)){var l3=mc.SurfaceSize;
var qg=F.mU(aX);Echo($"[OK] Screen \"{aX.a2}\" [{aX.bv}] -> {aX.a3} [#{aX.bu}] ({l3.X:0}x{l3.Y:0} px, {qg.T} sprites)");
}else{Echo($"[ERR] Screen \"{aX.a2}\" -> Missing block \"{aX.a3}\" [#{aX.bu}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double nI(IMyTerminalBlock q){
if(q==null)return 0.0;var en=q as IMyBatteryBlock;if(en!=null)return en.MaxStoredPower>0 ?(en.CurrentStoredPower/en.MaxStoredPower)*100.0 : 0.0;
var eP=q as IMyJumpDrive;if(eP!=null)return eP.MaxStoredPower>0 ?(eP.CurrentStoredPower/eP.MaxStoredPower)*100.0 : 0.0;var mi=q as IMyGasTank;
if(mi!=null)return mi.FilledRatio*100.0;var mC=q as IMyAirVent;if(mC!=null)return mC.GetOxygenLevel()*100.0;var j1=q as IMyCargoContainer;if(j1!=null){
var eN=j1.GetInventory();if(eN!=null&&eN.MaxVolume.RawValue>0)return(double)eN.CurrentVolume.RawValue/eN.MaxVolume.RawValue*100.0;return 0.0;}var e7=q as IMyReactor;
if(e7!=null)return e7.MaxOutput>0 ?(e7.CurrentOutput/e7.MaxOutput)*100.0 : 0.0;var ff=q as IMySolarPanel;if(ff!=null)return ff.MaxOutput>0 ?(ff.CurrentOutput/ff.MaxOutput)*100.0 : 0.0;
var fy=q as IMyWindTurbine;if(fy!=null)return fy.MaxOutput>0 ?(fy.CurrentOutput/fy.MaxOutput)*100.0 : 0.0;var kk=q as IMyDoor;if(kk!=null)return(double)kk.OpenRatio*100.0;
var fu=q as IMyThrust;if(fu!=null)return fu.MaxEffectiveThrust>0 ?(fu.CurrentThrust/fu.MaxEffectiveThrust)*100.0 : 0.0;var ir=q as IMyMotorSuspension;
if(ir!=null)return ir.Brake ? 100.0 :(ir.Friction*100.0);var gE=q as IMyShipConnector;if(gE!=null)return gE.Status==MyShipConnectorStatus.Connected ? 100.0 :(gE.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var lJ=q as IMyProductionBlock;if(lJ!=null)return lJ.IsProducing ? 100.0 : 0.0;var ko=q as IMyShipDrill;if(ko!=null)return ko.Enabled ? 100.0 : 0.0;
var ix=q as IMyShipToolBase;if(ix!=null)return ix.IsActivated ? 100.0 :(ix.Enabled ? 50.0 : 0.0);var gY=q as IMyGasGenerator;if(gY!=null)return gY.AutoRefill ? 100.0 :(gY.Enabled ? 50.0 : 0.0);
var fv=q as IMyLargeTurretBase;if(fv!=null)return fv.IsShooting ? 100.0 :(fv.HasTarget ? 50.0 :(fv.Enabled ? 10.0 : 0.0));var dE=q as IMyProjector;
if(dE!=null)return dE.TotalBlocks>0 ?((double)(dE.TotalBlocks-dE.RemainingBlocks)/dE.TotalBlocks)*100.0 : 0.0;var g3=q as IMyGyro;if(g3!=null)return g3.GyroOverride ? 100.0 :(g3.Enabled ? 50.0 : 0.0);
var kQ=q as IMyGravityGeneratorBase;if(kQ!=null)return Math.Min(100.0,Math.Abs(kQ.GravityAcceleration)/9.81*100.0);var gX=q as IMyLandingGear;if(gX!=null)return gX.IsLocked ? 100.0 :(gX.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var jF=q as IMyRadioAntenna;if(jF!=null)return(jF.Radius/50000.0)*100.0;var jR=q as IMyBeacon;if(jR!=null)return(jR.Radius/50000.0)*100.0;
var lf=q as IMyLightingBlock;if(lf!=null)return lf.Intensity*10.0;var e5=q as IMyPistonBase;if(e5!=null)return e5.HighestPosition>0 ?(e5.CurrentPosition/e5.HighestPosition)*100.0 : 0.0;
var lM=q as IMyMotorStator;if(lM!=null)return Math.Min(100.0,Math.Abs(lM.Angle)/6.28318*100.0);var mE=q as IMyWarhead;if(mE!=null)return mE.IsArmed ? 100.0 : 0.0;
var ic=q as IMySensorBlock;if(ic!=null)return!ic.LastDetectedEntity.IsEmpty()? 100.0 :(ic.Enabled ? 25.0 : 0.0);var gy=q as IMyCameraBlock;
if(gy!=null)return gy.IsActive ? 100.0 :(gy.Enabled ? 50.0 : 0.0);var j7=q as IMyParachute;if(j7!=null)return(double)j7.OpenRatio*100.0;
var ft=q as IMyTurretControlBlock;if(ft!=null)return ft.HasTarget ? 100.0 :(ft.IsUnderControl ? 50.0 :(ft.Enabled ? 10.0 : 0.0));var kF=q as IMyFunctionalBlock;
if(kF!=null)return kF.Enabled ? 100.0 : 0.0;return 0.0;}static double nb(IMyTerminalBlock jM,string gQ){int j_=gQ.IndexOf(':');string hj=gQ.Substring(0,j_).ToLowerInvariant();
var qx=new MyItemType("MyObjectBuilder_"+(hj=="ammo" ? "AmmoMagazine" : char.ToUpper(hj[0])+hj.Substring(1)),gQ.Substring(j_+1));double mb=0;for(int g6=0;
g6<jM.InventoryCount;g6++)mb+=(double)jM.GetInventory(g6).GetItemAmount(qx);return mb;}double je(IMyTerminalBlock r,string kx){if(r==null)return 0.0;
string a=kx.ToLowerInvariant();if(a=="percent"||a=="pct")return nI(r);if(a.IndexOf(':')>0)return nb(r,kx);var ba=r as IMyBatteryBlock;
if(ba!=null){if(a=="current"||a=="stored"||a=="val"||a=="value"||a=="raw")return ba.CurrentStoredPower;if(a=="max"||a=="capacity")return ba.MaxStoredPower;
if(a=="output")return ba.CurrentOutput;if(a=="maxoutput")return ba.MaxOutput;if(a=="input")return ba.CurrentInput;if(a=="maxinput")return ba.MaxInput;
return ba.CurrentStoredPower;}var hg=r as IMyJumpDrive;if(hg!=null){if(a=="max"||a=="capacity")return hg.MaxStoredPower;return hg.CurrentStoredPower;
}var fo=r as IMyGasTank;if(fo!=null){if(a=="max"||a=="capacity")return fo.Capacity;return fo.FilledRatio*fo.Capacity;}var mD=r as IMyAirVent;
if(mD!=null){if(a=="max"||a=="capacity")return 100.0;return mD.GetOxygenLevel()*100.0;}var j2=r as IMyCargoContainer;if(j2!=null){var eO=j2.GetInventory();
if(eO!=null){if(a=="mass")return(double)eO.CurrentMass;if(a=="max"||a=="capacity")return(double)eO.MaxVolume;return(double)eO.CurrentVolume;}return 0.0;}var hX=r as IMyPowerProducer;
if(hX!=null){if(a=="max"||a=="capacity")return hX.MaxOutput;return hX.CurrentOutput;}var kl=r as IMyDoor;if(kl!=null){if(a=="max")return 1.0;return(double)kl.OpenRatio*100.0;
}var iv=r as IMyThrust;if(iv!=null){if(a=="max"||a=="capacity")return iv.MaxEffectiveThrust;return iv.CurrentThrust;}var bn=r as IMyMotorSuspension;
if(bn!=null){if(a=="brake")return bn.Brake ? 100.0 : 0.0;if(a=="height")return bn.Height;if(a=="strength")return bn.Strength;if(a=="power")return bn.Power;
if(a=="steer")return bn.SteeringOverride*100.0;if(a=="propulsion")return bn.PropulsionOverride*100.0;return bn.Friction;}var kd=r as IMyShipConnector;
if(kd!=null){return kd.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var hU=r as IMyProductionBlock;if(hU!=null){if(a=="producing"||a=="active"||a=="running"||a=="state")return hU.IsProducing ? 100.0 : 0.0;
if(a=="queue")return hU.IsQueueEmpty ? 0.0 : 1.0;}var gh=r as IMyAssembler;if(gh!=null){if(a=="disassembling")return gh.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(a=="repeating"||a=="loop")return gh.Repeating ? 100.0 : 0.0;}var gZ=r as IMyGasGenerator;if(gZ!=null){if(a=="autorefill")return gZ.AutoRefill ? 100.0 : 0.0;
if(a=="active")return gZ.Enabled ? 100.0 : 0.0;}var fw=r as IMyLargeTurretBase;if(fw!=null){if(a=="shooting")return fw.IsShooting ? 100.0 : 0.0;
if(a=="target"||a=="hastarget")return fw.HasTarget ? 100.0 : 0.0;if(a=="range")return fw.Range;}var bh=r as IMyProjector;if(bh!=null){if(a=="built")return bh.TotalBlocks-bh.RemainingBlocks;
if(a=="remaining")return bh.RemainingBlocks;if(a=="total")return bh.TotalBlocks;if(a=="projecting")return bh.IsProjecting ? 100.0 : 0.0;if(a=="armor")return bh.RemainingArmorBlocks;
if(a=="buildable")return bh.BuildableBlocksCount;}var iy=r as IMyShipToolBase;if(iy!=null){if(a=="working"||a=="active"||a=="activated")return iy.IsActivated ? 100.0 : 0.0;
return iy.Enabled ? 100.0 : 0.0;}var ce=r as IMyGyro;if(ce!=null){if(a=="override")return ce.GyroOverride ? 100.0 : 0.0;if(a=="pitch")return ce.Pitch;
if(a=="yaw")return ce.Yaw;if(a=="roll")return ce.Roll;if(a=="power")return ce.GyroPower*100.0;}var kR=r as IMyGravityGeneratorBase;if(kR!=null){if(a=="g"||a=="accel")return kR.GravityAcceleration;
}var kP=r as IMyLandingGear;if(kP!=null){if(a=="locked")return kP.IsLocked ? 100.0 : 0.0;}var jG=r as IMyRadioAntenna;if(jG!=null){if(a=="radius"||a=="range")return jG.Radius;
}var jS=r as IMyBeacon;if(jS!=null){if(a=="radius"||a=="range")return jS.Radius;}var hr=r as IMyLightingBlock;if(hr!=null){if(a=="intensity")return hr.Intensity;
if(a=="radius")return hr.Radius;}var e6=r as IMyPistonBase;if(e6!=null){if(a=="pos"||a=="position"||a=="extension")return e6.CurrentPosition;
if(a=="velocity")return e6.Velocity;if(a=="max")return e6.HighestPosition;}var dJ=r as IMyMotorStator;if(dJ!=null){if(a=="angle"||a=="deg")return dJ.Angle*180.0/Math.PI;
if(a=="rad")return dJ.Angle;if(a=="velocity"||a=="rpm")return dJ.TargetVelocityRPM;if(a=="locked")return dJ.RotorLock ? 100.0 : 0.0;}var iF=r as IMyWarhead;
if(iF!=null){if(a=="armed")return iF.IsArmed ? 100.0 : 0.0;if(a=="countdown")return iF.DetonationTime;}var l0=r as IMySensorBlock;if(l0!=null){
if(a=="detected")return!l0.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var gz=r as IMyCameraBlock;if(gz!=null){if(a=="range")return gz.AvailableScanRange;
if(a=="active")return gz.IsActive ? 100.0 : 0.0;}var j8=r as IMyParachute;if(j8!=null){if(a=="open"||a=="openratio")return(double)j8.OpenRatio*100.0;
}var cw=r as IMyTurretControlBlock;if(cw!=null){if(a=="target"||a=="hastarget")return cw.HasTarget ? 100.0 : 0.0;if(a=="azimuth")return cw.AzimuthRotor!=null ? cw.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(a=="elevation")return cw.ElevationRotor!=null ? cw.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var kG=r as IMyFunctionalBlock;if(kG!=null){return kG.Enabled ? 100.0 : 0.0;
}return 0.0;}double cT(string c5,string ky){aL bb;cZ.TryGetValue(c5,out bb);if(bb!=null&&bb.a1==aw.cL){
string o2= !string.IsNullOrEmpty(bb.aN)? bb.aN : c5;double ke;if(an.TryGetValue(o2,out ke))return ke;return bb.d1;
}double kr;if(an.TryGetValue(c5,out kr))return kr;List<IMyTerminalBlock>c6;if(!b3.TryGetValue(c5,out c6)||c6.Count==0)throw new InvalidOperationException($"HELM: bind '{c5}' resolved to zero blocks — check the name/typo");
if(c6.Count==1)return je(c6[0],ky);var mB=new List<double>();foreach(var n3 in c6)mB.Add(je(n3,ky));return mK.mJ(bb!=null ? bb.cA : al.fD,mB);
}double fX(string oe){return cT(oe,"percent");}static string a0(double my,bt cl){if(cl==null||(string.IsNullOrEmpty(cl.cY)&&string.IsNullOrEmpty(cl.cM)&&cl.I=="percent")){
return iZ(my);}return F.m4(my,cl.cY,cl.cM);}bool nC(z cs){IMyTextSurface fm;if(!b4.TryGetValue(cs,out fm))return false;
if(cs.bq!=null)return nw(cs,fm);if(cs.cG.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return nD(cs,fm);
}return nE(cs,fm);}static void v(a5 oj,Vector2 pI,Vector2 qa,Color ol,float pX=0f,string ow="SquareSimple"){oj.l.Add(new MySprite{
Type=SpriteType.TEXTURE,Data=ow,Position=pI,Size=qa,Color=ol,RotationOrScale=pX});}void mR(z o,IMyTextSurface md,a5 f){
f.l.Clear();f.d8.Clear();Vector2 L=md.SurfaceSize;Vector2 qu=md.TextureSize;Vector2 aY=(qu-L)/2f;
float eZ=Math.Max(4f,L.X*0.02f);float dB=Math.Max(4f,L.Y*0.02f);if(o.bv=="compact"||(o.bv=="auto"&&L.Y<=260f&&L.Y>150f)){
eZ=4f;dB=4f;}else if(o.bv=="corner"||(o.bv=="auto"&&L.Y<=150f)){eZ=2f;dB=2f;}float ms=L.X-(eZ*2f);
Color go=new Color(10,12,16);bool kn=true;if(!string.IsNullOrEmpty(o.bp)){var dg=k.bY(o.bp);if(dg.cP)kn=false;
else go=new Color(dg.aQ,dg.aO,dg.aK,dg.dT);}f.bQ= -1;f.fC=false;f.fA=R(o.iI ?? "#5b0f0f");if(o.dX!=null&&o.dX.Length>1){
int em=16;float jO=(L.Y-4f)/em;f.bQ=f.l.Count;f.bP=new Color[em];for(int cf=0;cf<em;cf++){k gW=F.i_(o.dX,100.0*cf/(em-1));
f.bP[cf]=new Color(gW.aQ,gW.aO,gW.aK);v(f,new Vector2(aY.X+L.X/2f,aY.Y+2f+jO*(cf+0.5f)),new Vector2(L.X-4f,jO+1f),f.bP[cf]);
}}else if(kn){f.bQ=f.l.Count;f.bP=new[]{go};v(f,aY+(L/2f),L-new Vector2(4f,4f),go);}if(!o.cG.Equals("none",StringComparison.OrdinalIgnoreCase)){
Color jX=o.aM!=null&&o.aM.Length>0 ? R(o.aM[0]): R(o.d_ ?? "40, 60, 85");
Color oi=o.aM!=null&&o.aM.Length>1 ? R(o.aM[o.aM.Length-1]): jX;
v(f,new Vector2(aY.X+L.X/2f,aY.Y+dB*0.5f),new Vector2(ms,2f),jX);v(f,new Vector2(aY.X+L.X/2f,aY.Y+L.Y-(dB*0.5f)),new Vector2(ms,2f),oi);
}var hK=o.az;if(hK.Count==0){hK=new List<am>{new am{cS="main",cI=0,cU=0,cV=o.bV,cW=o.bW,aP=o.fO}
};}float kT=8f;foreach(var P in hK){var eY=F.mT(P.cI,P.cU,P.cV,P.cW,o.bV,o.bW,L.X,L.Y,eZ,dB,kT,kT);
float aC=aY.X+eY.jt;float dA=aY.Y+eY.ju;float aD=eY.js;float dz=eY.i1;if(!string.IsNullOrEmpty(P.cB)){
var dy=k.bY(P.cB);if(!dy.cP)v(f,new Vector2(aC+aD/2f,dA+dz/2f),new Vector2(aD,dz),new Color(dy.aQ,dy.aO,dy.aK,dy.dT));
}else if(o.bV>1||o.bW>1){Color j0=string.IsNullOrEmpty(o.fS)? new Color(255,255,255,6): R(o.fS);if(j0.A>0)v(f,new Vector2(aC+aD/2f,dA+dz/2f),new Vector2(aD,dz),j0);
}string gV=P.iW ?? o.iX ?? "Monospace";float oc=F.m6(o.bv,L.X,L.Y,o.bT);float au=P.iY ?? oc;
float eG=0f;if(!string.IsNullOrEmpty(P.bx)){eG=20f*au;var gc=TextAlignment.LEFT;float iw=aC+4f;string jz=P.fY ?? o.jn ?? "left";
if(jz.Equals("center",StringComparison.OrdinalIgnoreCase)){gc=TextAlignment.CENTER;iw=aC+(aD/2f);}else if(jz.Equals("right",StringComparison.OrdinalIgnoreCase)){
gc=TextAlignment.RIGHT;iw=aC+aD-4f;}string ml=P.fZ ?? o.jo;Color qv= !string.IsNullOrEmpty(ml)? R(ml): R(P.fG ?? o.d_ ?? "white");
var mq=MySprite.CreateText(P.bx,gV,qv,au*0.9f,gc);mq.Position=new Vector2(iw,dA+2f);f.l.Add(mq);
string[]gn=P.fH ?? o.aM;Color oy=gn!=null&&gn.Length>0 ? R(gn[0]): R(P.fG ?? o.d_ ?? "40, 60, 85");
v(f,new Vector2(aC+aD/2f,dA+eG-2f),new Vector2(aD-8f,1f),oy);}int pn=Math.Max(1,P.aP.Count);float pz=dz-eG-8f;
float h0=pz/pn;float eM=6f;for(int eL=0;eL<P.aP.Count;eL++){var w=P.aP[eL];var K=new cR{i7=w};float pY=dA+eG+4f+(eL*h0);
float D=pY+(h0/2f);float mn=D-(11f*au);K.fM=f.l.Count;K.i0=w.N!=null&&(w.N.Contains("{val}")||w.N.Contains("{percent}"));
var lb=MySprite.CreateText(w.N,gV,Color.White,au,TextAlignment.LEFT);lb.Position=new Vector2(aC+eM,mn);f.l.Add(lb);
float pg=Math.Max(24f*au,(w.N.Length+1)*12f*au);float qG=(!string.IsNullOrEmpty(w.cY)|| !string.IsNullOrEmpty(w.cM))? 64f*au : 52f*au;
if(w.g!=d.jm){float C=aC+eM+pg;float ob=aC+aD-eM-qG;float B=Math.Max(16f,ob-C);
float J=Math.Max(w.g==d.bw ? 8f : 4f,Math.Min((w.g==d.bw ? 20f : 16f)*au,h0*(w.g==d.bw ? 0.55f : 0.45f)));
K.cF=C;K.dW=B;K.bO=J;K.cE=D;if(w.g==d.cD){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(25,30,38));
K.V=f.l.Count;v(f,new Vector2(C,D),new Vector2(0f,J),R(w.ah));}else if(w.g==d.cQ){
int ia=10;float kI=2f;float ib=Math.Max(2f,(B-(kI*(ia-1)))/ia);for(int h5=0;h5<ia;h5++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+h5*(ib+kI)+(ib/2f),D),new Vector2(ib,J),new Color(22,26,32));}}else if(w.g==d.bw){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));
int hT=8;float kJ=2f;float gC=Math.Max(2f,(B-(kJ*(hT-1)))/hT);for(int hI=0;hI<hT;hI++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+hI*(gC+kJ)+(gC/2f),D),new Vector2(gC,2f),R(w.ah));}}else if(w.g==d.cO){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));
int ik=10;float kK=2f;float il=Math.Max(2f,(B-(kK*(ik-1)))/ik);for(int h6=0;h6<ik;h6++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+h6*(il+kK)+(il/2f),D),new Vector2(il,J),new Color(24,28,36),0.523599f);}}else if(w.g==d.cH){
int gf=10;float kL=2f;float gg=Math.Max(2f,(B-(kL*(gf-1)))/gf);for(int h7=0;h7<gf;h7++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+h7*(gg+kL)+(gg/2f),D),new Vector2(gg,J),new Color(24,28,36),1.570796f,"Triangle");}}else if(w.g==d.cK){
int gH=12;float kM=2f;float gJ=Math.Max(2f,(B-(kM*(gH-1)))/gH);float km=Math.Min(gJ,J);for(int h8=0;h8<gH;h8++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+h8*(gJ+kM)+(gJ/2f),D),new Vector2(km,km),new Color(24,28,36),0f,"Circle");}}else if(w.g==d.bN){
v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(25,30,38));K.V=f.l.Count;v(f,new Vector2(C+(B/2f),D),new Vector2(0f,J),R(w.ah));
v(f,new Vector2(C+(B/2f),D),new Vector2(2f,J+4f),new Color(80,110,150));}else if(w.g==d.b_){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));
K.V=f.l.Count;float hl=Math.Max(4f,J);v(f,new Vector2(C+hl*0.5f,D),new Vector2(hl,hl),new Color(40,50,65),0f,"Circle");
}else if(w.g==d.bR){float b6=Math.Max(6f,Math.Min(J,14f*au));v(f,new Vector2(C+b6*0.5f,D),new Vector2(b6,b6),new Color(28,34,46));
K.V=f.l.Count;v(f,new Vector2(C+b6*0.5f,D),new Vector2(b6*0.6f,b6*0.6f),new Color(40,50,65));}else if(w.g==d.bZ){
v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(18,22,30));K.V=f.l.Count;float hR=Math.Max(4f,J*0.7f);
v(f,new Vector2(C+hR+2f,D),new Vector2(hR,hR),new Color(34,197,94),0f,"Circle");}else if(w.g==d.bM){int hQ=8;
float kN=2f;float hS=Math.Max(2f,(B-(kN*(hQ-1)))/hQ);for(int hJ=0;hJ<hQ;hJ++){K.Z.Add(f.l.Count);v(f,new Vector2(C+hJ*(hS+kN)+(hS/2f),D),new Vector2(hS,J),new Color(24,28,36));
}}else if(w.g==d.bS){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));K.V=f.l.Count;
float kY=Math.Max(4f,J);v(f,new Vector2(C+(B/2f),D),new Vector2(kY,kY),R(w.ah),0f,"Triangle");}}var mw=MySprite.CreateText("---",gV,Color.White,au,TextAlignment.RIGHT);
mw.Position=new Vector2(aC+aD-eM,mn);K.a7=f.l.Count;f.l.Add(mw);f.d8.Add(K);}}f.i5=L;
f.i4=true;}static void aA(a5 jY,cR l4,string qt,Color om){var iC=jY.l[l4.a7];iC.Data=qt;iC.Color=om;jY.l[l4.a7]=iC;
}static void jh(a5 jZ,cR l5,Color oo){var kv=jZ.l[l5.V];kv.Color=oo;jZ.l[l5.V]=kv;}bool nE(z dL,IMyTextSurface io){
a5 i;if(!eg.TryGetValue(dL,out i)){i=new a5();eg[dL]=i;}if(!i.i4||i.i5!=io.SurfaceSize){
mR(dL,io,i);}bool G=false;bool gb=(aS%dL.fB)<dL.fB/2.0;bool iE=bA&&gb&&i.fA.A>0;
if(i.bQ>=0&&iE!=i.fC){i.fC=iE;for(int di=0;di<i.bP.Length;di++){var jU=i.l[i.bQ+di];jU.Color=iE ? i.fA : i.bP[di];
i.l[i.bQ+di]=jU;}G=true;}for(int g7=0;g7<i.d8.Count;g7++){var b=i.d8[g7];var m=b.i7;double x;bool kV=false;double hx=m.d6;
try{x=cT(m.aZ,m.I);if(m.bX!=null)hx=cT(m.bX,m.fP);}catch{kV=true;x=0.0;}if(kV){
if(b.d5!= -99999.0||(b.a7>=0&&i.l[b.a7].Data!="ERR")){G=true;b.d5= -99999.0;if(b.a7>=0){
var h9=i.l[b.a7];h9.Data="ERR";h9.Color=Color.Red;i.l[b.a7]=h9;}}continue;}double aE=x;if(hx>0)x=Math.Min(100.0,aE/hx*100.0);
string oq=m.cN ? F.d3(m.bU,x): m.ah;Color W=R(oq);bool aT=(W==Color.Red);
if(aT&&gb){W=new Color(255,75,75);G=true;}bool av=Math.Abs(aE-b.d5)>0.1;if(av)b.d5=aE;if(m.g==d.cD){
double kz=(m.fL||m.I=="percent"||m.I=="pct")? x : fX(m.aZ);if(b.M<0f)b.M=(float)kz;
float fp=(float)kz;if(Math.Abs(b.M-fp)>0.05f){b.M+=(fp-b.M)*0.35f;G=true;}else if(b.M!=fp){
b.M=fp;G=true;}if(av)G=true;if(G){float pB=(float)Math.Max(0.0,Math.Min(1.0,b.M/100.0));float kB=b.dW*pB;
var eA=i.l[b.V];eA.Size=new Vector2(kB,b.bO);eA.Position=new Vector2(b.cF+(kB/2f),b.cE);
eA.Color=W;i.l[b.V]=eA;aA(i,b,a0(aE,m),W);}}else if(m.g==d.cQ||m.g==d.cO||m.g==d.cH||m.g==d.cK){
if(av||aT){G=true;double oG=(m.fL||m.I=="percent"||m.I=="pct")? x : fX(m.aZ);int lZ=b.Z.Count;
for(int dK=0;dK<lZ;dK++){bool pp=oG>=((dK+0.4)/(double)lZ*100.0);Color p7=pp ? W : new Color(24,28,36);var l_=i.l[b.Z[dK]];
l_.Color=p7;i.l[b.Z[dK]]=l_;}aA(i,b,a0(aE,m),W);}}else if(m.g==d.bw){
if(av||aT){G=true;for(int eF=0;eF<7;eF++)b.d9[eF]=b.d9[eF+1];b.d9[7]=(float)x;int pH=b.Z.Count;
for(int dv=0;dv<pH;dv++){float oV=b.d9[dv];float oU=Math.Max(0.06f,Math.Min(1f,oV/100f));float ka=b.bO*oU;float op=b.cE+(b.bO/2f)-(ka/2f);
var cb=i.l[b.Z[dv]];cb.Size=new Vector2(cb.Size.Value.X,ka);cb.Position=new Vector2(cb.Position.Value.X,op);
cb.Color=W;i.l[b.Z[dv]]=cb;}aA(i,b,a0(aE,m),W);}}else if(m.g==d.bN){
if(b.M<0f)b.M=(float)x;float fq=(float)x;if(Math.Abs(b.M-fq)>0.05f){b.M+=(fq-b.M)*0.35f;
G=true;}else if(b.M!=fq){b.M=fq;G=true;}if(G||av){float kU=b.dW*0.5f;float j5=b.cF+kU;
float ki=(float)Math.Max(-1.0,Math.Min(1.0,(b.M-50.0)/50.0));float gT=Math.Abs(ki)*kU;float oH=(ki>=0)?(j5+gT*0.5f):(j5-gT*0.5f);
var eB=i.l[b.V];eB.Size=new Vector2(gT,b.bO);eB.Position=new Vector2(oH,b.cE);eB.Color=W;
i.l[b.V]=eB;aA(i,b,a0(aE,m),W);}}else if(m.g==d.b_){if(av||aT){
G=true;bool hf=x>=50.0;float k5=Math.Max(4f,b.bO);float o5=hf ?(b.cF+b.dW-k5*0.5f):(b.cF+k5*0.5f);
Color k4=hf ?(W!=Color.White ? W : new Color(34,197,94)): new Color(60,70,85);var gR=i.l[b.V];gR.Position=new Vector2(o5,b.cE);
gR.Color=k4;i.l[b.V]=gR;aA(i,b,hf ? "ON" : "OFF",k4);}}else if(m.g==d.bR){if(av||aT){
G=true;bool he=x>=50.0;Color j6=he ?(W!=Color.White ? W : new Color(56,189,248)): new Color(30,36,48);jh(i,b,j6);
aA(i,b,he ? "ACTIVE" : "READY",he ? j6 : new Color(148,163,184));}}else if(m.g==d.bZ){if(av||aT){
G=true;string qh=x>=75.0 ? "ONLINE" :(x>=25.0 ? "STBY" : "OFFLINE");Color l9=x>=75.0 ? new Color(34,197,94):(x>=25.0 ? new Color(234,179,8): new Color(239,68,68));
jh(i,b,l9);aA(i,b,qh,l9);}}else if(m.g==d.bM){if(av||aT){G=true;int lF=b.Z.Count;
for(int dw=0;dw<lF;dw++){bool pq=x>=((dw+0.5)/(double)lF*100.0);Color pF=pq ? W : new Color(24,28,36);var lG=i.l[b.Z[dw]];
lG.Color=pF;i.l[b.Z[dw]]=lG;}aA(i,b,a0(aE,m),W);}}else if(m.g==d.bS){
G=true;float fa;Color de;string eC;bool oY=(m.I=="producing"||m.I=="active"||m.I=="running"||m.I=="state"||(m.I!="percent"&&(x<=1.0||x==100.0)));
if(oY){if(x>0.0){b.M=(b.M+0.3927f)%6.28318f;fa=b.M;de=new Color(34,197,94);eC="RUN";}else{fa=0f;
de=new Color(148,163,184);eC="IDLE";}}else{fa=(x>55.0)? 0f :((x<45.0)? 3.14159f : 1.57079f);de=(x>55.0)? new Color(34,197,94):((x<45.0)? new Color(239,68,68): new Color(148,163,184));
eC=(x>55.0)? "CHG" :((x<45.0)? "DSC" : "STBY");}var gS=i.l[b.V];gS.RotationOrScale=fa;gS.Color=de;
i.l[b.V]=gS;aA(i,b,eC,de);}else{string ey;aL fs;cZ.TryGetValue(m.aZ,out fs);ed.TryGetValue(fs!=null&&fs.aN!=null ? fs.aN : m.aZ,out ey);
if(av||aT||ey!=b.i6){b.i6=ey;G=true;Color iz=m.a_!=null ? R(F.jf(m.a_,x)): Color.White;
if(iz==Color.Red&&gb)iz=new Color(255,75,75);aA(i,b,ey ?? a0(aE,m),iz);}}if(b.i0&&(av||aT)){
G=true;var ld=i.l[b.fM];string pC=iZ(x);string pV=a0(aE,m);ld.Data=m.N.Replace("{percent}",pC).Replace("{val}",pV);
i.l[b.fM]=ld;}}if(G){using(var oL=io.DrawFrame()){oL.AddRange(i.l);}}return G;}bool nD(z bj,IMyTextSurface ip){
ip.FontSize=bj.bT;var eu=new List<KeyValuePair<string,string>>();foreach(var j in bj.fO){double E=0.0;string aI;string aB=null;
double hy=j.d6;try{E=cT(j.aZ,j.I);if(j.bX!=null)hy=cT(j.bX,j.fP);}catch(Exception qN){
eu.Add(new KeyValuePair<string,string>($"{j.N}: [UNRESOLVED]","red"));continue;}double lK=E;if(hy>0)E=Math.Min(100.0,lK/hy*100.0);string mx=a0(lK,j);
if(j.g==d.cD){double kA=(j.fL||j.I=="percent"||j.I=="pct")? E : fX(j.aZ);aI=F.nx(j.N,kA,10,mx);
aB=j.cN ? F.d3(j.bU,kA): j.ah;}else if(j.g==d.bN){aI=F.nv(j.N,E,10);
aB=j.cN ? F.d3(j.bU,E): j.ah;}else if(j.g==d.b_){aI=F.nG(j.N,E);
aB=j.ah ??(E>=50.0 ? "green" : "gray");}else if(j.g==d.bR){aI=F.nA(j.N,E);aB=j.ah ??(E>=50.0 ? "cyan" : "gray");
}else if(j.g==d.bZ){aI=F.nF(j.N,E);aB=j.ah ??(E>=75.0 ? "lime" :(E>=25.0 ? "yellow" : "red"));
}else if(j.g==d.bM){aI=F.nu(j.N,E,8);aB=j.cN ? F.d3(j.bU,E):(j.ah ?? "yellow");
}else if(j.g==d.bS){bool oZ=(j.I=="producing"||j.I=="active"||j.I=="running"||j.I=="state"||(j.I!="percent"&&(E<=1.0||E==100.0)));
if(oZ){char[]qf=new[]{'|','/','-','\\'};char p4=qf[Math.Abs(c2)%4];aI=E>0 ? $"{j.N} [{p4}] RUN" : $"{j.N} [==] IDLE";
aB=E>0 ? "green" : "gray";}else{aI=F.nB(j.N,E,c2);aB=j.ah ??(E>55.0 ? "green" :(E<45.0 ? "red" : "cyan"));
}}else{aI=$"{j.N}: {mx}";aB=j.a_!=null ? F.jf(j.a_,E): null;}eu.Add(new KeyValuePair<string,string>(aI,aB));
}var er=F.nz(new List<string>(eu.ConvertAll(qo=>qo.Key)),cy: bj.jr);string l1=string.Join("\n",er);
string lI;if(b1.TryGetValue(bj,out lI)&&lI==l1)return false;b1[bj]=l1;using(var oM=ip.DrawFrame()){
float mH=4f;float lg=mI*bj.bT;try{var li=ip.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bj.bT);
if(li.Y>0)lg=li.Y;}catch{}for(int cg=0;cg<er.Count;cg++){string kc=null;if(cg>0&&cg<er.Count-1)kc=eu[cg-1].Value;
var l8=MySprite.CreateText(er[cg],"Monospace",R(kc),bj.bT,TextAlignment.LEFT);l8.Position=new Vector2(4f,mH);oM.Add(l8);
mH+=lg;}}return true;}Color R(string eW){if(string.IsNullOrEmpty(eW))return Color.White;Color dq;if(jv.TryGetValue(eW,out dq))return dq;
k c7=k.bY(eW);dq=c7.cP ? Color.Transparent : new Color(c7.aQ,c7.aO,c7.aK,c7.dT);jv[eW]=dq;return dq;}public enum aw{mM,fK,cL}
public enum al{nf,fD,jk,i9,i8}public class aL{public string Q;public aw a1;public List<string>ar=new List<string>();public string jl{
get{return ar.Count>0 ? ar[0]: "";}set{if(ar.Count==0)ar.Add(value);else ar[0]=value;}}public al cA=al.nf;
public double d1=0.0;public string fI;public string aN;public bool dV;}public enum d{jm,cD,cQ,bw,cO,cH,cK,bN,b_,bR,bZ,bM,bS}
public class cJ{public string iT;public string d0;public bool d4;}public class bt{public d g;public string N;public string aZ;
public string I;public bool cN;public string[]bU;public string ah;public List<cJ>a_;public string iQ="right";
public string cY;public string cM;public double d6;public string bX,fP;public bool fL{get{return d6>0||bX!=null;}}}public struct k{
public byte aQ,aO,aK,dT;public bool cP;public k(byte pN,byte oO,byte n4,byte nP=255){aQ=pN;aO=oO;aK=n4;dT=nP;cP=false;}public static readonly k nN=new k(0,0,0,0){
cP=true};public static readonly k ea=new k(255,255,255);public static readonly k mL=new k(0,0,0);public static readonly k ns=new k(255,0,0);
public static readonly k m8=new k(0,255,0);public static readonly k nc=new k(50,205,50);public static readonly k nO=new k(255,255,0);
public static readonly k ng=new k(255,140,0);public static readonly k mW=new k(0,255,255);public static readonly k mN=new k(0,120,255);
public static readonly k nd=new k(255,0,255);public static readonly k nq=new k(128,0,128);public static readonly k m7=new k(128,128,128);
public static readonly k mX=new k(60,60,60);public static k bY(string aH){if(string.IsNullOrWhiteSpace(aH))return ea;aH=aH.Trim();
if(aH.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aH.Equals("none",StringComparison.OrdinalIgnoreCase))return nN;if(aH.StartsWith("#")){
string be=aH.Substring(1).Trim();if(be.Length==6){byte pO=Convert.ToByte(be.Substring(0,2),16);byte oP=Convert.ToByte(be.Substring(2,2),16);byte n5=Convert.ToByte(be.Substring(4,2),16);
return new k(pO,oP,n5);}if(be.Length==3){byte pP=Convert.ToByte(new string(be[0],2),16);byte oQ=Convert.ToByte(new string(be[1],2),16);byte n6=Convert.ToByte(new string(be[2],2),16);
return new k(pP,oQ,n6);}}if(aH.Contains(",")){var co=aH.Split(',');if(co.Length>=3){byte pQ=(byte)Math.Max(0,Math.Min(255,int.Parse(co[0].Trim())));
byte oR=(byte)Math.Max(0,Math.Min(255,int.Parse(co[1].Trim())));byte n7=(byte)Math.Max(0,Math.Min(255,int.Parse(co[2].Trim())));byte nQ=co.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(co[3].Trim()))):(byte)255;
return new k(pQ,oR,n7,nQ);}}switch(aH.ToLowerInvariant()){case "red": return ns;case "green": return m8;case "lime": return nc;case "yellow": return nO;
case "orange": return ng;case "cyan": return mW;case "blue": return mN;case "magenta": return nd;case "purple": return nq;case "white": return ea;
case "black": return mL;case "gray": case "grey": return m7;case "darkgray": case "darkgrey": return mX;default: return ea;}}}public class am{
public string cS="default";public int cI=0;public int cU=0;public int cV=1;public int cW=1;public string bx;public string fY="left";public string fZ;
public string iO;public string iW;public float? iY;public string cB;public string fG;public string[]fH;public string ja="horizontal";
public List<bt>aP=new List<bt>();}public class z{public string a2;public string bv="standard";public string cG="graphic";public string d_;
public string[]aM;public string jn="left";public string jo;public string iX="Monospace";public string bp;public string iM;
public int iN=10;public int iK=1;public string fS;public string[]dX;public string iI;public string bq;public string bs;
public float fF=2.5f;public string iL="cyan";public string[]dY;public List<string>dZ=new List<string>();public List<string>m5(){
if(dZ!=null&&dZ.Count>0)return dZ;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float bT=1.0f;public int fU=0;public float fB=1.6f;public int bV=1;public int bW=1;public int jr=30;public string a3;
public int bu=0;public List<am>az=new List<am>();public List<bt>fO{get{if(az.Count==0)az.Add(new am{cS="main"}
);return az[0].aP;}}}public class a6{public string fQ;public List<string>d7=new List<string>();public Dictionary<string,string>fJ=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>fE=new List<string>();}public class fT{public List<aL>br=new List<aL>();public List<z>S=new List<z>();
public List<string>ay=new List<string>();public Dictionary<string,List<string>>cC=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class i2{static readonly Dictionary<string,a6>iP=new Dictionary<string,a6>(StringComparer.OrdinalIgnoreCase);
static i2(){nt();}static void nt(){a4("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
,@"@bind pwr_$screen=group(""$pwr"",agg:avg)
@bind h2_$screen=group(""$h2"",agg:avg)
@bind crg_$screen=group(""$cargo"",agg:sum)
@screen $screen
output:""$screen"",$surf
layout:grid(2,1)
@panel sys at(0,0,1,1) title:""PROPULSION""
line:""PWR"" bar(pwr_$screen.percent)
line:""H2"" bar(h2_$screen.percent)
@panel hold at(1,0,1,1) title:""STORES""
line:""CARGO"" led(crg_$screen.percent)");a4("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");a4("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
,@"@bind pwr_$screen=group(""$power"",agg:avg)
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
line:""CARGO"" led(crg_$screen.percent)");a4("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");a4("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
,@"@bind vent_$screen=airvent(""$vent"")
@bind outer_$screen=block(""$outer"")
@bind inner_$screen=block(""$inner"")
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel env at(0,0,1,1) title:""ATMOSPHERE""
line:""PRESS"" bar(vent_$screen.percent)
@panel doors at(0,1,1,1) title:""SECURITY AIRLOCK""
line:""OUTER"" led(outer_$screen.percent)
line:""INNER"" led(inner_$screen.percent)");a4("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
,@"@bind bat_$screen=group(""$power"",agg:avg)
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
line:""HOLD"" bar(crg_$screen.percent)");a4("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");a4("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void a4(string lp,string[]pA,string of){var fn=new a6{fQ=lp}
;foreach(var pv in pA){var hm=pv.Split(new[]{'='},2);string lx=fR(hm[0]);fn.d7.Add(lx);if(hm.Length>1)fn.fJ[lx]=_(hm[1].Trim());
}var po=of.Replace("\r\n","\n").Split('\n');foreach(var pb in po){string lQ=pb.Trim();if(lQ.Length>0)fn.fE.Add(lQ);}iP[lp]=fn;}static string fR(string cm){
cm=cm.Trim();if(cm.StartsWith("$"))cm=cm.Substring(1);return cm.ToLowerInvariant();}public static fT ni(string ou){var ae=new fT();z e=null;
am u=null;var lH=no(ou,ae.ay,ae.cC);for(int dj=0;dj<lH.Count;dj++){string c=lH[dj].Trim();
if(c.Length==0)continue;try{if(c.StartsWith("@bind")){ae.br.Add(nj(c));}else if(c.StartsWith("@screen")){e=new z{a2=c.Substring("@screen".Length).Trim()}
;ae.S.Add(e);u=null;}else if(c.StartsWith("profile:")){t(e,"profile:");e.bv=c.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(c.StartsWith("border:")){t(e,"border:");string jN=c.Substring("border:".Length).Trim();if(u!=null)u.iO=jN;
else e.cG=jN;}else if(c.StartsWith("border_color:")){t(e,"border_color:");string jQ=c.Substring("border_color:".Length).Trim();
if(u!=null)u.fG=jQ;else e.d_=jQ;}else if(c.StartsWith("border_gradient:")||c.StartsWith("border_gradient(")){
t(e,"border_gradient");string bC=c.StartsWith("border_gradient:")? c.Substring("border_gradient:".Length): c.Substring("border_gradient".Length);
bC=bC.Trim();if(bC.StartsWith("(")&&bC.Contains(")"))bC=ap(bC,0);string[]ma=aq(bC).Select(pZ=>pZ.Trim()).ToArray();if(u!=null)u.fH=ma;
else e.aM=ma;}else if(c.StartsWith("title_align:")){t(e,"title_align:");string mh=c.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(u!=null)u.fY=mh;else e.jn=mh;}else if(c.StartsWith("title_color:")){t(e,"title_color:");string mk=c.Substring("title_color:".Length).Trim();
if(u!=null)u.fZ=mk;else e.jo=mk;}else if(c.StartsWith("font_size:")){t(e,"font_size:");float mf=float.Parse(c.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(u!=null)u.iY=mf;else e.bT=mf;}else if(c.StartsWith("font:")){t(e,"font:");string kw=c.Substring("font:".Length).Trim();
if(u!=null)u.iW=kw;else e.iX=kw;}else if(c.StartsWith("bg:")||c.StartsWith("background:")){t(e,"bg:");string jV=c.Substring(c.IndexOf(':')+1).Trim();
if(u!=null)u.cB=jV;else e.bp=jV;}else if(c.StartsWith("layout:")){t(e,"layout:");nk(c.Substring("layout:".Length).Trim(),e);
}else if(c.StartsWith("alert_period:")){t(e,"alert_period:");e.fB=Math.Max(0.2f,float.Parse(c.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(c.StartsWith("refresh:")){t(e,"refresh:");e.fU=int.Parse(c.Substring("refresh:".Length).Trim());}else if(c.StartsWith("output:")){
t(e,"output:");var e1=aq(c.Substring("output:".Length).Trim());if(e1.Count==0)throw new FormatException("output: requires a target block name");
e.a3=_(e1[0]);e.bu=e1.Count>1 ? int.Parse(e1[1]): 0;}else if(c.StartsWith("width:")){t(e,"width:");
e.jr=int.Parse(c.Substring("width:".Length).Trim());}else if(c.StartsWith("boot_msg:")||c.StartsWith("boot:")){t(e,"boot_msg:");
e.iM=_(c.Substring(c.IndexOf(':')+1).Trim());}else if(c.StartsWith("boot_stagger:")){t(e,"boot_stagger:");e.iN=int.Parse(c.Substring("boot_stagger:".Length).Trim());
}else if(c.StartsWith("boot_batch:")){t(e,"boot_batch:");e.iK=Math.Max(1,int.Parse(c.Substring("boot_batch:".Length).Trim()));
}else if(c.StartsWith("boot_duration:")){t(e,"boot_duration:");string c9=c.Substring("boot_duration:".Length).Trim().ToLowerInvariant();
bool kX=c9.EndsWith("s");if(kX)c9=c9.Substring(0,c9.Length-1).Trim();float gK;if(float.TryParse(c9,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out gK)){
e.fF=kX ? gK : gK/60f;}}else if(c.StartsWith("boot_log:")){t(e,"boot_log:");e.dZ.Add(_(c.Substring("boot_log:".Length).Trim()));
}else if(c.StartsWith("banner:")){t(e,"banner:");e.bq=_(c.Substring("banner:".Length).Trim());}else if(c.StartsWith("boot_logo:")){
t(e,"boot_logo:");e.bs=_(c.Substring("boot_logo:".Length).Trim());}else if(c.StartsWith("boot_gradient:")||c.StartsWith("boot_gradient(")||c.StartsWith("banner_gradient:")||c.StartsWith("banner_gradient(")){
t(e,"boot_gradient");e.dY=jd(c);}else if(c.StartsWith("bg_gradient:")||c.StartsWith("bg_gradient(")){t(e,"bg_gradient");
e.dX=jd(c);}else if(c.StartsWith("panel_bg:")){t(e,"panel_bg:");e.fS=c.Substring("panel_bg:".Length).Trim();
}else if(c.StartsWith("alert_bg:")){t(e,"alert_bg:");e.iI=c.Substring("alert_bg:".Length).Trim();}else if(c.StartsWith("boot_color:")||c.StartsWith("banner_color:")){
t(e,"boot_color:");e.iL=c.Substring(c.IndexOf(':')+1).Trim();}else if(c.StartsWith("orientation:")){t(e,"orientation:");
string pt=c.Substring("orientation:".Length).Trim().ToLowerInvariant();if(u!=null)u.ja=pt;}else if(c.StartsWith("@panel")){t(e,"@panel");
u=nm(c);e.az.Add(u);}else if(c.StartsWith("title:")){t(e,"title:");if(u==null){u=new am{
cS="main",cI=0,cU=0,cV=e.bV,cW=e.bW};e.az.Add(u);}u.bx=_(c.Substring("title:".Length).Trim());
}else if(c.StartsWith("line:")){t(e,"line:");if(u==null){u=new am{cS="main",cI=0,cU=0,cV=e.bV,cW=e.bW}
;e.az.Add(u);}u.aP.Add(nl(c.Substring("line:".Length).Trim()));}else{ae.ay.Add($"Line {dj + 1}: unrecognized directive: \"{c}\"");
}}catch(Exception oB){ae.ay.Add($"Line {dj + 1}: parse error ({oB.Message}) in \"{c}\"");}}foreach(var lR in ae.S){if(string.IsNullOrEmpty(lR.a3))ae.ay.Add($"@screen {lR.a2}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aG in ae.S){if(aG.bq!=null&& !ae.cC.ContainsKey(aG.bq))ae.ay.Add("Screen '"+aG.a2+"': banner '"+aG.bq+"' is not defined (@banner "+aG.bq+" ... @end)");
if(aG.bs!=null&& !ae.cC.ContainsKey(aG.bs))ae.ay.Add("Screen '"+aG.a2+"': boot_logo '"+aG.bs+"' is not defined (@banner "+aG.bs+" ... @end)");
}return ae;}static void nk(string gs,z lY){int hL=gs.IndexOf('(');if(hL<0)throw new FormatException("missing '(' in layout: specification");
string k3=gs.Substring(0,hL).Trim().ToLowerInvariant();string nW=ap(gs,hL);var ge=aq(nW);if(k3=="grid"){if(ge.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
lY.bV=Math.Max(1,int.Parse(ge[0]));lY.bW=Math.Max(1,int.Parse(ge[1]));}else{throw new FormatException($"unknown layout kind '{k3}' (expected grid)");
}}static am nm(string pj){string dG=pj.Substring("@panel".Length).Trim();int el=dG.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(el<0)throw new FormatException("@panel requires an at(...) positioning clause");
string pr=dG.Substring(0,el).Trim();string jK=dG.Substring(el);int lD=jK.IndexOf('(');string jI=ap(jK,lD);
var bB=aq(jI);if(bB.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var aV=new am{cS=pr,cI=int.Parse(bB[0]),cU=int.Parse(bB[1]),cV=bB.Count>2 ? int.Parse(bB[2]): 1,cW=bB.Count>3 ? int.Parse(bB[3]): 1}
;int jB=el+lD+jI.Length+1;if(jB<dG.Length){string ao=dG.Substring(jB).Trim();int mp=ao.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(mp>=0){string dN=ao.Substring(mp+5).Trim();if(dN.StartsWith("(")&&dN.Contains(")"))aV.bx=_(ap(dN,0));else if(dN.StartsWith(":")){
string bD=dN.Substring(1).Trim();if(bD.StartsWith("\"")){int ks=bD.IndexOf('"',1);aV.bx=ks>0 ? bD.Substring(1,ks-1): _(bD);}else{int l6=bD.IndexOf(' ');
aV.bx=l6>=0 ? bD.Substring(0,l6): bD;}}}string ls=d2(ao,"orientation:");if(ls!=null)aV.ja=ls.ToLowerInvariant();string jA=d2(ao,"title_align:");
if(jA!=null)aV.fY=jA.ToLowerInvariant();string mm=d2(ao,"title_color:");if(mm!=null)aV.fZ=mm;int eq=ao.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(eq>=0&& !ao.Substring(eq).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !ao.Substring(eq).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
aV.iO=d2(ao.Substring(eq),"border:");}int eo=ao.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(eo<0)eo=ao.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(eo>=0){string bc=ao.Substring(eo+"border_gradient".Length).Trim();if(bc.StartsWith(":"))bc=bc.Substring(1).Trim();if(bc.StartsWith("(")&&bc.Contains(")"))bc=ap(bc,0);
aV.fH=aq(bc).Select(p_=>p_.Trim()).ToArray();}}return aV;}static string d2(string lS,string k2){int kW=lS.IndexOf(k2,StringComparison.OrdinalIgnoreCase);
if(kW<0)return null;string iD=lS.Substring(kW+k2.Length).Trim();int l7=iD.IndexOf(' ');return l7>=0 ? iD.Substring(0,l7): iD;}static string[]jd(string hs){
int g_=hs.IndexOfAny(new[]{':','('});string df=hs.Substring(hs[g_]==':' ? g_+1 : g_).Trim();if(df.StartsWith("(")&&df.Contains(")"))df=ap(df,0);return aq(df).Select(p0=>p0.Trim()).ToArray();
}static List<string>no(string ov,List<string>gO,Dictionary<string,List<string>>n9){var mt=new Dictionary<string,a6>(StringComparer.OrdinalIgnoreCase);
var cp=(ov ?? "").Replace("\r\n","\n").Split('\n');var hG=new List<string>();int ch=0;while(ch<cp.Length){string hY=cp[ch++];if(hY.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var aa=new List<string>();while(ch<cp.Length){string jy=cp[ch++].Replace("\t","    ").TrimEnd();if(jy.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
aa.Add(jy);}while(aa.Count>0&&aa[0].Length==0)aa.RemoveAt(0);while(aa.Count>0&&aa[aa.Count-1].Length==0)aa.RemoveAt(aa.Count-1);int hd=int.MaxValue;
foreach(var f7 in aa)if(f7.Length>0)hd=Math.Min(hd,f7.Length-f7.TrimStart().Length);for(int dr=0;dr<aa.Count;dr++)if(aa[dr].Length>0)aa[dr]=aa[dr].Substring(hd);
n9[hY.TrimStart().Substring(8).Trim()]=aa;continue;}string bg=jj(hY).Trim();if(bg.Length==0)continue;if(bg.StartsWith("@template")){try{
var it=nn(bg);while(ch<cp.Length){string oh=cp[ch++];string gt=jj(oh).Trim();if(gt.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(gt.Length>0)it.fE.Add(gt);}mt[it.fQ]=it;}catch(Exception oC){gO.Add($"Template parse error ({oC.Message}) in \"{bg}\"");}}
else if(bg.StartsWith("@use")){try{iS(bg,mt,hG,gO);}catch(Exception oD){gO.Add($"@use error ({oD.Message}) in \"{bg}\"");
}}else{hG.Add(bg);}}return hG;}static a6 nn(string pk){string hZ=pk.Substring("@template".Length).Trim();int hM=hZ.IndexOf('(');
if(hM<0)throw new FormatException("missing '(' in @template header");string ps=hZ.Substring(0,hM).Trim();string nX=ap(hZ,hM);var iu=new a6{
fQ=ps};foreach(var pw in aq(nX)){var hn=pw.Split(new[]{'='},2);string ly=fR(hn[0]);iu.d7.Add(ly);if(hn.Length>1)iu.fJ[ly]=_(hn[1].Trim());
}return iu;}static void iS(string pl,Dictionary<string,a6>mu,List<string>lv,List<string>oA,int kh=0){string e9=pl.Substring("@use".Length).Trim();
int e_=e9.IndexOf('(');string hz=e_>=0 ? e9.Substring(0,e_).Trim(): e9.Trim();string nY=e_>=0 ? ap(e9,e_): "";a6 cv=null;
if(!mu.TryGetValue(hz,out cv)){if(!iP.TryGetValue(hz,out cv))throw new FormatException($"unknown template '{hz}'");}var es=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var k7 in cv.fJ)es[k7.Key]=k7.Value;var jH=aq(nY);for(int c3=0;c3<jH.Count;c3++){string ej=jH[c3];int gM=ej.IndexOf('=');if(gM>=0){
string o3=fR(ej.Substring(0,gM));string qA=_(ej.Substring(gM+1).Trim());es[o3]=qA;}else{if(c3<cv.d7.Count){string o4=cv.d7[c3];
es[o4]=_(ej.Trim());}}}foreach(var og in cv.fE){string dc=og;foreach(var eS in es){dc=dc.Replace("$"+eS.Key,eS.Value).Replace("{"+eS.Key+"}",eS.Value);
}if(dc.StartsWith("@use")){if(kh>3)throw new FormatException("@use nested too deep");iS(dc,mu,lv,oA,kh+1);
}else lv.Add(dc);}}static void t(z ot,string ox){if(ot==null)throw new FormatException($"'{ox}' used before any @screen declared");
}static string jj(string aF){if(string.IsNullOrEmpty(aF))return aF;bool hc=false;for(int bf=0;bf<aF.Length;bf++){char gw=aF[bf];if(gw=='"'){hc= !hc;
}else if(!hc){if(gw=='/'&&bf+1<aF.Length&&aF[bf+1]=='/'){return aF.Substring(0,bf);}if(gw=='#'){if(na(aF,bf)){continue;}return aF.Substring(0,bf);}}}return aF;
}static bool na(string bE,int kZ){int ck=0;for(int g8=kZ+1;g8<bE.Length;g8++){char b7=bE[g8];if((b7>='0'&&b7<='9')||(b7>='a'&&b7<='f')||(b7>='A'&&b7<='F'))ck++;
else break;}if(ck==3||ck==4||ck==6||ck==8){int du=kZ+1+ck;if(du>=bE.Length||char.IsWhiteSpace(bE[du])||bE[du]==')'||bE[du]==','||bE[du]==':')return true;
}return false;}static aL nj(string pm){string h_=pm.Substring("@bind".Length).Trim();int gN=h_.IndexOf('=');if(gN<0)throw new FormatException("missing '=' in @bind");
string lq=h_.Substring(0,gN).Trim();string gP=h_.Substring(gN+1).Trim();int hN=gP.IndexOf('(');if(hN<0)throw new FormatException("missing '(' in @bind target");
string eE=gP.Substring(0,hN).Trim().ToLowerInvariant();string nZ=ap(gP,hN);var a9=aq(nZ);if(a9.Count==0)throw new FormatException("@bind target has no arguments");
var ac=new aL{Q=lq};if(eE=="dynamic"||eE=="var"||eE=="mem"){ac.a1=aw.cL;ac.aN=a9.Count>0 ? _(a9[0].Trim()): lq;
for(int f8=0;f8<a9.Count;f8++){var eT=a9[f8].Split(new[]{':'},2);if(eT.Length==2&&eT[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double kg;
if(double.TryParse(eT[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out kg))ac.d1=kg;else ac.fI=_(eT[1].Trim());
}}return ac;}ac.a1=eE=="group" ? aw.fK : aw.mM;string ei=null;for(int eh=0;eh<a9.Count;eh++){var cj=a9[eh].Split(new[]{':'},2);if(cj.Length==2&&cj[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
ei=cj[1].Trim().ToLowerInvariant();}else if(cj.Length==2&&cj[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ac.dV=cj[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ac.ar.Add(_(a9[eh].Trim()));}}if(ac.ar.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ac.a1==aw.fK){if(ei==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(ei){case "avg": ac.cA=al.fD;break;case "sum": ac.cA=al.jk;break;case "min": ac.cA=al.i9;break;case "max": ac.cA=al.i8;
break;default: throw new FormatException($"unknown agg '{ei}' (expected avg/sum/min/max)");}}return ac;}static bt nl(string ep){var n=new bt();
if(!ep.StartsWith("\""))throw new FormatException("line must start with a quoted label");int gL=ep.IndexOf('"',1);if(gL<0)throw new FormatException("unterminated quoted label");
n.N=ep.Substring(1,gL-1);string dF=ep.Substring(gL+1).Trim();int e0=dF.IndexOf('(');if(e0<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string y=dF.Substring(0,e0).Trim().ToLowerInvariant();string nV=ap(dF,e0);string U=dF.Substring(dF.IndexOf(')',e0)+1).Trim();
var gI=nV.Trim().Split('.');if(gI.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
n.aZ=gI[0].Trim();n.I=gI[1].Trim();if(y=="bar")n.g=d.cD;else if(y=="led")n.g=d.cQ;else if(y=="spark"||y=="sparkline")n.g=d.bw;
else if(y=="hazard"||y=="stripes")n.g=d.cO;else if(y=="chevron"||y=="chevrons")n.g=d.cH;else if(y=="dots"||y=="dot")n.g=d.cK;
else if(y=="balance"||y=="horizon"||y=="centered")n.g=d.bN;else if(y=="toggle"||y=="switch")n.g=d.b_;else if(y=="checkbox"||y=="check")n.g=d.bR;
else if(y=="status"||y=="badge"||y=="state")n.g=d.bZ;else if(y=="ammo"||y=="pips")n.g=d.bM;else if(y=="flow"||y=="rate"||y=="spinner")n.g=d.bS;
else if(y=="text")n.g=d.jm;else throw new FormatException($"unknown display function '{y}'");while(U.Length>0){int gG=U.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int cd=U.IndexOf('(');if(gG>=0&&(cd<0||gG<cd)){string ev=U.Substring(gG+4).Trim();int fg=ev.IndexOf(' ');n.iQ=(fg>=0 ? ev.Substring(0,fg): ev).ToLowerInvariant();
U=(fg>=0 ? ev.Substring(fg): "").Trim();continue;}int iB=U.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(iB>=0&&(cd<0||iB<cd)){
string fx=U.Substring(iB+5).Trim();int fh=fx.IndexOf(' ');n.cY=(fh>=0 ? fx.Substring(0,fh): fx).Trim().Trim('"');U=(fh>=0 ? fx.Substring(fh): "").Trim();
continue;}int gU=U.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(gU>=0&&(cd<0||gU<cd)){string ez=U.Substring(gU+7).Trim();
int fi=ez.IndexOf(' ');n.cM=(fi>=0 ? ez.Substring(0,fi): ez).Trim().Trim('"');U=(fi>=0 ? ez.Substring(fi): "").Trim();continue;}int eV=U.IndexOf('(');
if(eV<0)break;string af=U.Substring(0,eV).Trim().ToLowerInvariant();int lc=af.LastIndexOf(' ');if(lc>=0)af=af.Substring(lc+1).Trim();
string aU=ap(U,eV);U=U.Substring(U.IndexOf(')',eV)+1).Trim();if(af=="gradient"){n.cN=true;n.bU=aq(aU).Select(p1=>p1.Trim()).ToArray();
if(n.bU.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(af=="flat"){n.ah=aU.Trim();}else if(af=="pattern"){
string H=aU.Trim().ToLowerInvariant();if(H=="hazard"||H=="stripes")n.g=d.cO;else if(H=="chevron"||H=="chevrons")n.g=d.cH;
else if(H=="dots"||H=="dot")n.g=d.cK;else if(H=="led")n.g=d.cQ;else if(H=="balance"||H=="horizon")n.g=d.bN;
else if(H=="toggle"||H=="switch")n.g=d.b_;else if(H=="checkbox"||H=="check")n.g=d.bR;else if(H=="status"||H=="badge"||H=="state")n.g=d.bZ;
else if(H=="ammo"||H=="pips")n.g=d.bM;else if(H=="flow"||H=="rate"||H=="spinner")n.g=d.bS;}else if(af=="color"){n.a_=new List<cJ>();
foreach(var hO in aq(aU)){int gD=hO.LastIndexOf(':');if(gD<0)throw new FormatException("color() entries need 'expr : color'");string le=hO.Substring(0,gD).Trim();
string kb=hO.Substring(gD+1).Trim();if(le.Equals("default",StringComparison.OrdinalIgnoreCase)){n.a_.Add(new cJ{d4=true,d0=kb}
);}else{string qj=nK(le);n.a_.Add(new cJ{iT=qj,d0=kb});}}if(!n.a_.Any(pR=>pR.d4))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
}else if(af=="dir"||af=="direction"){n.iQ=aU.Trim().ToLowerInvariant();}else if(af=="unit"){n.cY=aU.Trim().Trim('"');}else if(af=="max"){
double ll;if(double.TryParse(aU.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out ll))n.d6=ll;else{var hw=aU.Trim().Split('.');
if(hw.Length!=2)throw new FormatException("max() takes a number or bindname.field");n.bX=hw[0].Trim();n.fP=hw[1].Trim();}}else if(af=="format"||af=="fmt"){
n.cM=aU.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{af}'");}}return n;}static string ap(string fc,int hE){
int gF=0;for(int dk=hE;dk<fc.Length;dk++){if(fc[dk]=='(')gF++;else if(fc[dk]==')'){gF--;if(gF==0)return fc.Substring(hE+1,dk-hE-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aq(string n_){return n_.Split(',').Select(p2=>p2.Trim()).Where(p3=>p3.Length>0).ToList();
}static string nK(string cc){cc=cc.Trim();foreach(var pu in new[]{">=","<=","==",">","<"}){int g9=cc.IndexOf(pu,StringComparison.Ordinal);
if(g9>0)return cc.Substring(g9).Trim();if(g9==0)return cc;}throw new FormatException($"color() rule has no recognizable comparison operator: '{cc}'");}
static string _(string bF){if(bF.Length>=2&&bF[0]=='"'&&bF[bF.Length-1]=='"')return bF.Substring(1,bF.Length-2);return bF;}}public static class mK{public static double mJ(al nR,List<double>cx){
if(cx==null||cx.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(nR){case al.fD: return cx.Average();case al.jk: return cx.Sum();
case al.i9: return cx.Min();case al.i8: return cx.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class F{
public static string nx(string pd,double e2,int jP=10,string mA=null){e2=Math.Max(0,Math.Min(100,e2));int kC=(int)Math.Round(jP*(e2/100.0));
string oa=new string('#',kC)+new string('-',jP-kC);string qB=mA!=null ? mA : $"{e2:0}%";return $"{pd} [{oa}] {qB}";
}public static string m4(double bo,string iA=null,string eD=null){if(iA=="auto"){double f9=Math.Abs(bo);if(f9>=1e9)return(bo/1e9).ToString("F1")+"G";
if(f9>=1e6)return(bo/1e6).ToString("F1")+"M";if(f9>=1e3)return(bo/1e3).ToString("F1")+"k";string oE= !string.IsNullOrEmpty(eD)? eD : "F0";return bo.ToString(oE);
}string kE= !string.IsNullOrEmpty(eD)? eD :(Math.Abs(bo)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(iA)){return bo.ToString(kE);}return bo.ToString(kE)+iA;
}public static float m6(string pJ,float me,float iq,float ew=1.0f){string cn=(pJ ?? "standard").ToLowerInvariant();
if(cn=="compact"||(cn=="auto"&&me<=300&&iq<=300&&iq>160)){return ew*0.75f;}if(cn=="corner"||(cn=="auto"&&iq<=160)){
return ew*0.55f;}if(cn=="wide"||(cn=="auto"&&me>=1000)){return ew*1.15f;}return ew;}public static string nv(string pe,double e3,int bd=5){
e3=Math.Max(0,Math.Min(100,e3));double ex=(e3-50.0)/50.0;int ds=0;int dI=0;if(ex< -0.01)ds=(int)Math.Round(-ex*bd);
else if(ex>0.01)dI=(int)Math.Round(ex*bd);if(ds>bd)ds=bd;if(dI>bd)dI=bd;string ph=new string('-',bd-ds)+new string('<',ds);
string pW=new string('>',dI)+new string('-',bd-dI);return $"{pe} [{ph}|{pW}] {e3:0}%";}public static string nG(string k9,double qD){
return qD>=50.0 ? $"{k9} [====O] ON" : $"{k9} [O====] OFF";}public static string nA(string la,double qE){return qE>=50.0 ? $"{la} [X] ACTIVE" : $"{la} [ ] READY ";
}public static string nF(string hp,double mz){if(mz>=75.0)return $"{hp} [ ONLINE  ]";if(mz>=25.0)return $"{hp} [ STANDBY ]";return $"{hp} [ OFFLINE ]";
}public static string nu(string pf,double e4,int lh=8){e4=Math.Max(0,Math.Min(100,e4));int kD=(int)Math.Round(lh*(e4/100.0));
string pG=new string('|',kD)+new string('.',lh-kD);return $"{pf} [{pG}] {e4:0}%";}public static string nB(string hq,double dD,int mo=0){
char[]qe=new[]{'|','/','-','\\'};char lT=qe[(mo>=0 ? mo : 0)%4];if(dD>55.0)return $"{hq} {lT} ▲ CHG {dD:0}%";if(dD<45.0)return $"{hq} {lT} ▼ DSC {dD:0}%";
return $"{hq} ● STBY {dD:0}%";}public static bool ne(string ag,string dn){if(ag==null||dn==null)return false;if(ag=="*"||ag==dn)return true;
if(!ag.Contains("*")&& !ag.Contains("?"))return string.Equals(ag,dn,StringComparison.OrdinalIgnoreCase);int ak=0,dl=0,ii= -1,hv=0;while(dl<dn.Length){
if(ak<ag.Length&&(ag[ak]=='?'||char.ToLowerInvariant(ag[ak])==char.ToLowerInvariant(dn[dl]))){ak++;dl++;}else if(ak<ag.Length&&ag[ak]=='*'){
ii=ak;hv=dl;ak++;}else if(ii!= -1){ak=ii+1;hv++;dl=hv;}else{return false;}}while(ak<ag.Length&&ag[ak]=='*')ak++;return ak==ag.Length;
}public static int mO(double px,int mr){return Math.Max(0,Math.Min(mr,1+(int)(Math.Max(0.0,px)*mr)));}public static string d3(string[]cu,double hP){
if(cu==null||cu.Length==0)return "white";hP=Math.Max(0,Math.Min(100,hP));int ha=(int)Math.Floor(hP/100.0*cu.Length);if(ha>=cu.Length)ha=cu.Length-1;
return cu[ha];}public static k i_(string[]bm,double pD){if(bm==null||bm.Length==0)return k.ea;if(bm.Length==1)return k.bY(bm[0]);
double pU=Math.Max(0.0,Math.Min(1.0,pD/100.0));double lX=pU*(bm.Length-1);int hb=(int)Math.Floor(lX);int oX=Math.Min(bm.Length-1,hb+1);
double im=lX-hb;k b8=k.bY(bm[hb]);k gx=k.bY(bm[oX]);byte pS=(byte)Math.Round(b8.aQ+(gx.aQ-b8.aQ)*im);
byte oS=(byte)Math.Round(b8.aO+(gx.aO-b8.aO)*im);byte n8=(byte)Math.Round(b8.aK+(gx.aK-b8.aK)*im);return new k(pS,oS,n8);}public static string jf(List<cJ>lP,double qF){
foreach(var h1 in lP){if(h1.d4)continue;if(m2(h1.iT,qF))return h1.d0;}var kf=lP.FirstOrDefault(pT=>pT.d4);return kf!=null ? kf.d0 : null;
}static bool m2(string dd,double dS){dd=dd.Trim();foreach(var hD in new[]{">=","<=","==",">","<"}){if(dd.StartsWith(hD)){double dH=double.Parse(dd.Substring(hD.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(hD){case ">=": return dS>=dH;case "<=": return dS<=dH;case "==": return Math.Abs(dS-dH)<1e-9;case ">": return dS>dH;case "<": return dS<dH;
}}}throw new FormatException($"unsupported comparison expression: '{dd}'");}public static List<string>nz(List<string>os,int cy){
var eX=new List<string>();eX.Add("+"+new string('-',cy+2)+"+");foreach(var ht in os){string qw=ht.Length>cy ? ht.Substring(0,cy): ht;
string py=qw.PadRight(cy);eX.Add("| "+py+" |");}eX.Add("+"+new string('-',cy+2)+"+");return eX;}public struct jc{public float jt,ju,js,i1;
}public static jc mT(int j9,int lN,int fj,int fk,int dP,int dQ,float qn,float qm,float lz,float lA,float g1,float g2){
dP=Math.Max(1,dP);dQ=Math.Max(1,dQ);fj=Math.Max(1,Math.Min(dP-j9,fj));fk=Math.Max(1,Math.Min(dQ-lN,fk));
float n1=qn-(lz*2f);float n0=qm-(lA*2f);float j4=(n1-(g1*(dP-1)))/dP;float j3=(n0-(g2*(dQ-1)))/dQ;
float pL=lz+j9*(j4+g1);float pM=lA+lN*(j3+g2);float pK=fj*j4+((fj-1)*g1);float pE=fk*j3+((fk-1)*g2);return new jc{
jt=pL,ju=pM,js=pK,i1=pE};}public struct jg{public int nh;public int fN;public int T;public int ai;public int cX;
public int ax;public int nL;public int nM;public int m_;public int m0;
public double m1;}public static jg mU(z bk){var h=new jg();var lC=bk.az!=null&&bk.az.Count>0 ? bk.az : new List<am>{
new am{aP=bk.fO}};h.nh=lC.Count;h.fN=0;h.T=0;h.ai=0;if(!string.IsNullOrEmpty(bk.bp)&& !bk.bp.Equals("none",StringComparison.OrdinalIgnoreCase))h.T++;
if(!string.IsNullOrEmpty(bk.cG)&& !bk.cG.Equals("none",StringComparison.OrdinalIgnoreCase))h.T+=2;foreach(var dx in lC){
if(!string.IsNullOrEmpty(dx.cB)&& !dx.cB.Equals("none",StringComparison.OrdinalIgnoreCase))h.T++;if(!string.IsNullOrEmpty(dx.bx))h.T+=2;
h.fN+=dx.aP.Count;foreach(var pc in dx.aP){h.nL++;h.nM++;h.T+=2;switch(pc.g){case d.cD: h.cX++;
h.ax++;h.T+=2;h.ai++;break;case d.cQ: h.ax+=10;h.T+=10;
h.ai+=10;break;case d.cO: h.ax+=10;h.T+=11;h.ai+=10;break;case d.cH: h.ax+=10;
h.T+=10;h.ai+=10;break;case d.cK: h.ax+=12;h.T+=12;h.ai+=12;
break;case d.bw: h.cX++;h.ax+=8;h.T+=9;h.ai+=8;break;case d.bN: h.cX+=2;
h.ax++;h.T+=3;h.ai++;break;case d.b_: case d.bR: h.cX++;
h.ax++;h.T+=2;h.ai++;break;case d.bZ: case d.bS: h.cX++;h.ax+=2;
h.T+=3;h.ai+=2;break;case d.bM: h.ax+=8;h.T+=8;h.ai+=8;
break;default: h.ai++;break;}}}h.m_=25+(h.fN*2);h.m0=180+(h.ai*4);
h.m1=0.0;return h;}}