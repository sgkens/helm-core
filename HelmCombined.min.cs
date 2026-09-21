fS s;string f3=null;Dictionary<string,List<IMyTerminalBlock>>b1=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>ed=new Dictionary<string,bool>();
Dictionary<string,aL>cY=new Dictionary<string,aL>();Dictionary<string,Color>jt=new Dictionary<string,Color>();List<IMyTerminalBlock>bw=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>b0=new List<IMyTerminalBlock>();bool ea;int f1;string eb="";StringBuilder c0=new StringBuilder();readonly Dictionary<string,double>an=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>ec=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener cZ;bool bx=false;
Dictionary<z,IMyTextSurface>b2=new Dictionary<z,IMyTextSurface>();Dictionary<z,string>b_=new Dictionary<z,string>();
static readonly string[]i9=new string[101];static Program(){for(int eG=0;eG<=100;eG++){i9[eG]=eG+"%";}}static string iX(double qB){int dl=(int)Math.Round(qB);
if(dl<0)dl=0;else if(dl>100)dl=100;return i9[dl];}class a5{public List<MySprite>l=new List<MySprite>();public List<cQ>d7=new List<cQ>();
public int bO= -1;public Color[]bN;public Color fz;public bool fB;public Vector2 i3;public bool i2=false;}class cQ{
public br i5;public int V= -1;public int a7= -1;public int fL= -1;public bool iZ=false;public List<int>Z=new List<int>();
public float[]d8=new float[8];public float cE;public float dV;public float bM;public float cD;public double d4= -99999.0;
public string i4;public float M= -1f;}Dictionary<z,a5>ef=new Dictionary<z,a5>();int jv=0;
double f5=0;double aS=0;int c_=0;Dictionary<z,double>aR=new Dictionary<z,double>();Dictionary<z,double>f0=new Dictionary<z,double>();
int c1=0;const string m8="0.11.0";IMyTextSurface a8;bool f_=true;double fZ=0;string ju="";string f4="";
int ee=0,f2=0;List<KeyValuePair<string,int>>bZ=new List<KeyValuePair<string,int>>();const double nq=120;const float mH=22f;
public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{cZ=IGC.RegisterBroadcastListener("HELM");cZ.SetMessageCallback("HELM_IGC");
}catch{}fU();jg();i1();}public void Main(string jH,UpdateType qx){double kn=Runtime.TimeSinceLastRun.TotalSeconds;aS+=kn;
if((qx&UpdateType.IGC)!=0||jH=="HELM_IGC"){no();}string lJ=(jH ?? "").Trim();string aj=lJ.ToLowerInvariant();if(aj=="reload"){
fU();}else if(aj=="refresh"){fV();}else if(aj=="boot"){jg();}else if(aj=="console on"||aj=="console off"){f_=aj=="console on";
i1();fZ=0;}else if(aj=="scan"||aj=="diagnose"||aj=="test"){nI();return;}else if(aj.StartsWith("set ")){string e7=lJ.Substring(4).Trim();
int kr=e7.IndexOf('=');int qa=e7.IndexOf(' ');int ic=kr>0 ? kr : qa;if(ic>0){string k_=e7.Substring(0,ic).Trim();string mu=e7.Substring(ic+1).Trim();iH($"{k_}={mu}");
Echo($"[HELM] Set dynamic variable '{k_}' = {mu}");}return;}else if(aj.StartsWith("alert")){string dQ=aj.Length>5 ? aj.Substring(5).Trim(): "toggle";if(dQ=="toggle")bx= !bx;
else bx=dQ=="red"||dQ=="on"||dQ=="1"||dQ=="true";Echo($"[HELM] Red alert state: {bx}");return;}else if(Me.CustomData!=f3){
fU();}c1++;f5+=kn;if(f5>=nq){f5=0;fV();}if(s==null||s.S.Count==0){
jo();iP();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var bE=s.S[jv%s.S.Count];
jv++;f4=bE.a2;bool f9=aR.Count>0;if(f9)nx();double ko;if(!aR.ContainsKey(bE)&& !(f0.TryGetValue(bE,out ko)&&aS<ko)){
if(nB(bE))f9=true;if(bE.fT>0)f0[bE]=aS+bE.fT/60.0;}c_=f9 ? 0 : c_+1;Runtime.UpdateFrequency=c_<=s.S.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
jo();iP();ee=Runtime.CurrentInstructionCount;if(ee>f2)f2=ee;}void iP(){if(eb.Length>0)Echo(eb);
if(c0.Length>0)Echo(c0.ToString());}List<IMyTerminalBlock>dT(bool nR=false){if(!ea){b0.Clear();GridTerminalSystem.GetBlocks(b0);
bw.Clear();bw.AddRange(b0);bw.RemoveAll(n1=> !n1.IsSameConstructAs(Me));f1=b0.Count-bw.Count;ea=true;
}return nR ? b0 : bw;}IMyTerminalBlock iT(string ll,bool nT=false){var gh=GridTerminalSystem.GetBlockWithName(ll);if(gh==null||nT||gh.IsSameConstructAs(Me))return gh;
var gc=dT();for(int eH=0;eH<gc.Count;eH++)if(gc[eH].CustomName.IndexOf(ll,StringComparison.OrdinalIgnoreCase)>=0)return gc[eH];return null;}void mR(string lm,List<IMyTerminalBlock>dn,bool jA=false){
var kF=GridTerminalSystem.GetBlockGroupWithName(lm);if(kF==null)return;int jR=dn.Count;var dN=new List<IMyTerminalBlock>();kF.GetBlocks(dN);iF(dN,dn,jA);
if(dn.Count>jR)return;var kQ=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(kQ);foreach(var mE in kQ){if(mE.Name.IndexOf(lm,StringComparison.OrdinalIgnoreCase)<0)continue;
dN.Clear();mE.GetBlocks(dN);iF(dN,dn,jA);if(dn.Count>jR)return;}}void iF(List<IMyTerminalBlock>oM,List<IMyTerminalBlock>kY,bool nS=false){
foreach(var gi in oM)if((nS||gi.IsSameConstructAs(Me))&& !kY.Contains(gi))kY.Add(gi);}void fU(){f3=Me.CustomData;s=i0.nh(f3);
eb="";if(s.ay.Count>0){var lS=new StringBuilder("HELM config errors:\n");foreach(var oy in s.ay)lS.AppendLine(" - "+oy);eb=lS.ToString();
}cY.Clear();foreach(var gj in s.bp)if(!cY.ContainsKey(gj.Q))cY[gj.Q]=gj;ef.Clear();b_.Clear();aR.Clear();
f0.Clear();fV();}void no(){try{while(cZ!=null&&cZ.HasPendingMessage){var lh=cZ.AcceptMessage();
if(lh.Data is string){iH((string)lh.Data);}}}catch{}}void iH(string lC){if(string.IsNullOrEmpty(lC))return;var lz=lC.Split(';');
for(int g3=0;g3<lz.Length;g3++){var dB=lz[g3].Trim();if(dB.Length==0)continue;int da=dB.IndexOf('=');if(da<0)da=dB.IndexOf(':');if(da>0){string cg=dB.Substring(0,da).Trim();
string aJ=dB.Substring(da+1).Trim();if(cg.Equals("alert",StringComparison.OrdinalIgnoreCase)){bx=aJ.Equals("red",StringComparison.OrdinalIgnoreCase)||aJ=="1"||aJ.Equals("true",StringComparison.OrdinalIgnoreCase)||aJ.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double lp;ec.Remove(cg);if(double.TryParse(aJ,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out lp)){
an[cg]=lp;}else if(aJ.Equals("true",StringComparison.OrdinalIgnoreCase)||aJ.Equals("on",StringComparison.OrdinalIgnoreCase)){an[cg]=100.0;
}else if(aJ.Equals("false",StringComparison.OrdinalIgnoreCase)||aJ.Equals("off",StringComparison.OrdinalIgnoreCase)){an[cg]=0.0;}else{ec[cg]=aJ;
}}}}}void ad(int qj,string qq){bZ.Add(new KeyValuePair<string,int>(qq,qj));}static string jn(string h1,int lk){return h1.Length>lk ? h1.Substring(0,lk): h1;
}void i1(){var hU=Me as IMyTextSurfaceProvider;if(hU==null||hU.SurfaceCount<1)return;a8=hU.GetSurface(0);a8.ContentType=f_ ? ContentType.SCRIPT : ContentType.NONE;
a8.Script="";}void jo(){if(aS>=fZ){fZ=aS+(c_>s.S.Count+1 ? 5.0 : 1.0);mP();if(f_&&a8!=null)mY();
}Echo(ju);}void mP(){bZ.Clear();int gt=0,jU=0;foreach(var jJ in s.bp){List<IMyTerminalBlock>hn;if(jJ.a1==aw.cK){
gt++;continue;}if(b1.TryGetValue(jJ.Q,out hn)){jU+=hn.Count;if(hn.Count>0)gt++;}}int ks=s.ay.Count;ad(0,"HELM-[Kernel-(v"+m8+")]");
ad(4,"------------------------------");ad(1,"krn-cfg");ad(2," screens "+s.S.Count+"  binds "+gt+"/"+s.bp.Count+"  errors "+ks);
ad(2," alert "+(bx ? "RED" : "off")+"  blocks "+jU+"  docked-ignored "+f1);if(ks>0)ad(3," ! "+jn(s.ay[0],40));
ad(1,"cfg-dsp");int p7=0;foreach(var h2 in s.S){if(++p7>10){ad(2," +"+(s.S.Count-10)+" more");break;}IMyTextSurface id;string l0="--",ii="no block";
if(b2.TryGetValue(h2,out id)){l0=id.SurfaceSize.X.ToString("0")+"x"+id.SurfaceSize.Y.ToString("0");ii=aR.ContainsKey(h2)? "boot" : "live";
}ad(ii=="no block" ? 3 : 2," "+jn(h2.a2,14).PadRight(14)+" "+l0.PadRight(9)+" "+ii);}ad(1,"perf");ad(2," tick "+c1+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ad(2," instr "+ee+"  peak "+f2+"  max "+Runtime.MaxInstructionCount);ad(2," rate "+Runtime.UpdateFrequency+"  idle "+c_+"  up "+(int)aS+"s");
if(f4.Length>0)ad(2," last "+f4);var lT=new StringBuilder();foreach(var o5 in bZ)lT.AppendLine(o5.Key);ju=lT.ToString();}static Color mU(int qk){
switch(qk){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void mY(){Vector2 bF=a8.SurfaceSize;Vector2 hz=(a8.TextureSize-bF)/2f;int gA=30;foreach(var o6 in bZ)gA=Math.Max(gA,o6.Key.Length);
Vector2 gz=a8.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float lU=Math.Min(1f,Math.Min(bF.X*0.94f/(gA*gz.X),bF.Y*0.94f/(bZ.Count*gz.Y)));
float qH=hz.X+bF.X*0.03f;float mF=hz.Y+bF.Y*0.03f;var ie=new List<MySprite>();ie.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=hz+bF/2f,Size=bF,Color=new Color(12,12,16)}
);foreach(var k4 in bZ){cy(ie,k4.Key,mU(k4.Value),qH,mF,lU);mF+=gz.Y*lU;}using(var oH=a8.DrawFrame()){oH.AddRange(ie);}}static readonly List<string>mO=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void jg(){aR.Clear();if(s==null)return;for(int eI=0;eI<s.S.Count;eI++){var fa=s.S[eI];if(fa.fE>0f)aR[fa]=aS+(eI/Math.Max(1,fa.iI))*fa.iL/60.0;
}}void nx(){List<z>c9=null;int oS=(aR.Count+3)/4,oZ=0;foreach(var eQ in aR){double hG=(aS-eQ.Value)/eQ.Key.fE;if(hG<0)continue;
if(hG>=1.0){if(c9==null)c9=new List<z>();c9.Add(eQ.Key);continue;}if((oZ++ +c1)%oS==0)mX(eQ.Key,hG);}if(c9==null)return;foreach(var h3 in c9){
aR.Remove(h3);ef.Remove(h3);b_.Remove(h3);}}static void cy(List<MySprite>qb,string qr,Color oj,float qI,float qK,float p4){
var me=MySprite.CreateText(qr,"Monospace",oj,p4,TextAlignment.LEFT);me.Position=new Vector2(qI,qK);qb.Add(me);}static void iE(List<MySprite>qc,List<string>ej,z fd,float qJ,float qL,float p5,float ph){
string[]qh=fd.dX!=null&&fd.dX.Length>0 ? fd.dX : new[]{fd.iJ};for(int dg=0;dg<ej.Count;
dg++){k gu=E.iY(qh,ej.Count>1 ? 100.0*dg/(ej.Count-1): 0.0);cy(qc,ej[dg],new Color(gu.aQ,gu.aO,gu.aK),qJ,qL+dg*ph,p5);}}bool nv(z cp,IMyTextSurface fk){
List<string>c3;if(b_.ContainsKey(cp)|| !s.cB.TryGetValue(cp.bo,out c3))return false;b_[cp]="banner";Vector2 bG=fk.SurfaceSize;
Vector2 hA=(fk.TextureSize-bG)/2f;int es=1;foreach(var o7 in c3)es=Math.Max(es,o7.Length);Vector2 c7=fk.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float fc=Math.Min(bG.X*0.94f/(es*c7.X),bG.Y*0.94f/(c3.Count*c7.Y));var ig=new List<MySprite>();ig.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=hA+bG/2f,Size=bG,Color=string.IsNullOrEmpty(cp.bn)? new Color(10,12,16): R(cp.bn)}
);iE(ig,c3,cp,hA.X+(bG.X-es*c7.X*fc)/2f,hA.Y+(bG.Y-c3.Count*c7.Y*fc)/2f,fc,c7.Y*fc);using(var oI=fk.DrawFrame()){oI.AddRange(ig);
}return true;}void mX(z cq,double lu){IMyTextSurface dL;if(!b2.TryGetValue(cq,out dL))return;Vector2 bk=dL.SurfaceSize;
Vector2 hB=(dL.TextureSize-bk)/2f;var ds=cq.m4();string li=cq.iK ?? "HELM TELEMETRY KERNEL ONLINE";bool ht=bk.Y>=200f;
List<string>b3;if(cq.bq==null|| !s.cB.TryGetValue(cq.bq,out b3))b3=mO;int b8=li.Length+3;foreach(var o8 in ds)b8=Math.Max(b8,o8.Length+5);
if(ht)foreach(var o9 in b3)b8=Math.Max(b8,o9.Length);int lM=(ht ? b3.Count+1 : 0)+2+ds.Count;Vector2 b7=dL.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bh=Math.Min(1f,Math.Min(bk.X*0.94f/(b8*b7.X),bk.Y*0.94f/(lM*b7.Y)));float fy=hB.X+(bk.X-b8*b7.X*bh)/2f;float bJ=hB.Y+(bk.Y-lM*b7.Y*bh)/2f;
float eT=b7.Y*bh;var cs=new List<MySprite>();cs.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=hB+bk/2f,Size=bk,Color=new Color(10,12,16)}
);if(ht){iE(cs,b3,cq,fy,bJ,bh,eT);bJ+=(b3.Count+1)*eT;}cy(cs,">> "+li,new Color(34,197,94),fy,bJ,bh);bJ+=eT*2f;int p8=E.mN(lu,ds.Count);
for(int eJ=0;eJ<p8;eJ++){bool lr=eJ==ds.Count-1&&lu>=0.8;cy(cs,lr ? "[OK]" : "[..]",lr ? new Color(34,197,94): new Color(56,189,248),fy,bJ,bh);cy(cs,ds[eJ],new Color(203,213,225),fy+5f*b7.X*bh,bJ,bh);
bJ+=eT;}using(var oJ=dL.DrawFrame()){oJ.AddRange(cs);}}void fV(){b1.Clear();b2.Clear();ed.Clear();
c0.Clear();ea=false;if(s==null)return;foreach(var O in s.bp){var at=new List<IMyTerminalBlock>();if(O.a1==aw.cK){
string hg= !string.IsNullOrEmpty(O.aN)? O.aN : O.Q;if(!an.ContainsKey(hg)){an[hg]=O.d0;if(O.fH!=null)ec[hg]=O.fH;
}b1[O.Q]=at;continue;}var mh=O.ar.Count>0 ? O.ar : new List<string>{O.jj};foreach(var bI in mh){
if(string.IsNullOrEmpty(bI))continue;bool kZ=bI.Contains("*")||bI.Contains("?");if(O.a1==aw.fJ||kZ){var g_=new List<IMyTerminalBlock>();
mR(bI,g_,O.dU);if(g_.Count>0){foreach(var kM in g_){if(!at.Contains(kM))at.Add(kM);}}else{var jC=dT(O.dU);
for(int gk=0;gk<jC.Count;gk++){var fq=jC[gk];if(kZ ? E.nd(bI,fq.CustomName): string.Equals(bI,fq.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!at.Contains(fq))at.Add(fq);}}}}else{var go=iT(bI,O.dU);if(go!=null&& !at.Contains(go))at.Add(go);}}if(at.Count==0){
nG(O,mh,at);if(at.Count>0)ed[O.Q]=true;}b1[O.Q]=at;}foreach(var aW in s.S){
if(string.IsNullOrEmpty(aW.a3))continue;var hE=iT(aW.a3);if(hE==null){c0.AppendLine($"HELM: output block '{aW.a3}' not found");
continue;}IMyTextSurface bH=null;var hV=hE as IMyTextSurfaceProvider;var kh=hE as IMyTextSurface;if(hV!=null&&aW.bs<hV.SurfaceCount){
bH=hV.GetSurface(aW.bs);}else if(kh!=null&&aW.bs==0){bH=kh;}if(bH==null){c0.AppendLine($"HELM: output block '{aW.a3}' has no surface index {aW.bs}");
continue;}bH.ContentType=ContentType.SCRIPT;bH.Script="";bH.ScriptBackgroundColor=Color.Black;b2[aW]=bH;}bw.Clear();
b0.Clear();}static readonly string[]iS={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool m2(int o_,IMyTerminalBlock p){switch(o_){case 0: return p is IMyBatteryBlock;case 1: return p is IMyReactor;case 2: return p is IMySolarPanel;
case 3: return p is IMyWindTurbine;case 4: return p is IMyGasTank;case 5: return p is IMyAirVent;case 6: return p is IMyCargoContainer;case 7: return p is IMyJumpDrive;
case 8: return p is IMyThrust;case 9: return p is IMyMotorSuspension;case 10: return p is IMyDoor;case 11: return p is IMyShipConnector;case 12: return p is IMyAssembler;
case 13: return p is IMyRefinery;case 14: return p is IMyShipDrill;case 15: return p is IMyShipWelder;case 16: return p is IMyShipGrinder;case 17: return p is IMyGasGenerator;
case 18: return p is IMyLargeTurretBase;case 19: return p is IMyProjector;case 20: return p is IMyGyro;case 21: return p is IMyGravityGeneratorBase;case 22: return p is IMyLandingGear;
case 23: return p is IMyRadioAntenna;case 24: return p is IMyBeacon;case 25: return p is IMyLightingBlock;case 26: return p is IMyPistonBase;case 27: return p is IMyMotorStator;
case 28: return p is IMyWarhead;case 29: return p is IMySensorBlock;case 30: return p is IMyCameraBlock;case 31: return p is IMyParachute;case 32: return p is IMyTurretControlBlock;
}return false;}void nG(aL oc,List<string>qo,List<IMyTerminalBlock>gp){string oV=(oc.Q+" "+string.Join(" ",qo)).ToLowerInvariant();
var hj=new List<int>();for(int eP=0;eP<iS.Length;eP++){foreach(var qG in iS[eP].Split(','))if(oV.Contains(qG)){hj.Add(eP);break;}}if(hj.Count==0)return;
var jB=dT();var ls=new List<IMyTerminalBlock>();for(int g4=0;g4<jB.Count;g4++){var gl=jB[g4];foreach(int o0 in hj){if(m2(o0,gl)){(gl.CubeGrid==Me.CubeGrid ? gp : ls).Add(gl);
break;}}}if(gp.Count==0)gp.AddRange(ls);}void nI(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(s==null||s.S.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {s.S.Count} screens, {s.bp.Count} binds");ea=false;
Echo($"Blocks: {dT().Count} on this construct; {f1} on docked ships (ignored)");bw.Clear();Echo("--- BIND RESOLUTION ---");foreach(var ab in s.bp){
if(ab.a1==aw.cK){string hh= !string.IsNullOrEmpty(ab.aN)? ab.aN : ab.Q;double qy=an.ContainsKey(hh)? an[hh]: ab.d0;
Echo($"[OK] @bind {ab.Q} (dynamic variable '{hh}' = {qy})");continue;}List<IMyTerminalBlock>gq;if(b1.TryGetValue(ab.Q,out gq)){if(gq.Count>0){
string oE=ed.ContainsKey(ab.Q)&&ed[ab.Q]? " [fallback: typed match]" : "";Echo($"[OK] @bind {ab.Q} ({gq.Count} blocks matched{oE})");
}else{string qp=string.Join(", ",ab.ar.Count>0 ? ab.ar : new List<string>{ab.jj});Echo($"[WARN] @bind {ab.Q} -> 0 blocks matched \"{qp}\"");
}}else{Echo($"[ERR] @bind {ab.Q} -> unresolved");}}if(an.Count>0){Echo($"--- DYNAMIC VARIABLES ({an.Count}) ---");foreach(var k6 in an)Echo($"[VAR] {k6.Key} = {k6.Value}");
}Echo("--- SURFACE OUTPUT AUDIT ---");foreach(var aX in s.S){IMyTextSurface ma;if(b2.TryGetValue(aX,out ma)){var l1=ma.SurfaceSize;
var qf=E.mT(aX);Echo($"[OK] Screen \"{aX.a2}\" [{aX.bt}] -> {aX.a3} [#{aX.bs}] ({l1.X:0}x{l1.Y:0} px, {qf.T} sprites)");
}else{Echo($"[ERR] Screen \"{aX.a2}\" -> Missing block \"{aX.a3}\" [#{aX.bs}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double nH(IMyTerminalBlock q){
if(q==null)return 0.0;var em=q as IMyBatteryBlock;if(em!=null)return em.MaxStoredPower>0 ?(em.CurrentStoredPower/em.MaxStoredPower)*100.0 : 0.0;
var eO=q as IMyJumpDrive;if(eO!=null)return eO.MaxStoredPower>0 ?(eO.CurrentStoredPower/eO.MaxStoredPower)*100.0 : 0.0;var mg=q as IMyGasTank;
if(mg!=null)return mg.FilledRatio*100.0;var mB=q as IMyAirVent;if(mB!=null)return mB.GetOxygenLevel()*100.0;var j_=q as IMyCargoContainer;if(j_!=null){
var eM=j_.GetInventory();if(eM!=null&&eM.MaxVolume.RawValue>0)return(double)eM.CurrentVolume.RawValue/eM.MaxVolume.RawValue*100.0;return 0.0;}var e6=q as IMyReactor;
if(e6!=null)return e6.MaxOutput>0 ?(e6.CurrentOutput/e6.MaxOutput)*100.0 : 0.0;var fe=q as IMySolarPanel;if(fe!=null)return fe.MaxOutput>0 ?(fe.CurrentOutput/fe.MaxOutput)*100.0 : 0.0;
var fx=q as IMyWindTurbine;if(fx!=null)return fx.MaxOutput>0 ?(fx.CurrentOutput/fx.MaxOutput)*100.0 : 0.0;var ki=q as IMyDoor;if(ki!=null)return(double)ki.OpenRatio*100.0;
var ft=q as IMyThrust;if(ft!=null)return ft.MaxEffectiveThrust>0 ?(ft.CurrentThrust/ft.MaxEffectiveThrust)*100.0 : 0.0;var iq=q as IMyMotorSuspension;
if(iq!=null)return iq.Brake ? 100.0 :(iq.Friction*100.0);var gD=q as IMyShipConnector;if(gD!=null)return gD.Status==MyShipConnectorStatus.Connected ? 100.0 :(gD.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var lH=q as IMyProductionBlock;if(lH!=null)return lH.IsProducing ? 100.0 : 0.0;var km=q as IMyShipDrill;if(km!=null)return km.Enabled ? 100.0 : 0.0;
var mp=q as IMyShipToolBase;if(mp!=null)return mp.Enabled ? 100.0 : 0.0;var gX=q as IMyGasGenerator;if(gX!=null)return gX.AutoRefill ? 100.0 :(gX.Enabled ? 50.0 : 0.0);
var fu=q as IMyLargeTurretBase;if(fu!=null)return fu.IsShooting ? 100.0 :(fu.HasTarget ? 50.0 :(fu.Enabled ? 10.0 : 0.0));var dD=q as IMyProjector;
if(dD!=null)return dD.TotalBlocks>0 ?((double)(dD.TotalBlocks-dD.RemainingBlocks)/dD.TotalBlocks)*100.0 : 0.0;var g2=q as IMyGyro;if(g2!=null)return g2.GyroOverride ? 100.0 :(g2.Enabled ? 50.0 : 0.0);
var kO=q as IMyGravityGeneratorBase;if(kO!=null)return Math.Min(100.0,Math.Abs(kO.GravityAcceleration)/9.81*100.0);var gW=q as IMyLandingGear;if(gW!=null)return gW.IsLocked ? 100.0 :(gW.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var jD=q as IMyRadioAntenna;if(jD!=null)return(jD.Radius/50000.0)*100.0;var jP=q as IMyBeacon;if(jP!=null)return(jP.Radius/50000.0)*100.0;
var ld=q as IMyLightingBlock;if(ld!=null)return ld.Intensity*10.0;var e4=q as IMyPistonBase;if(e4!=null)return e4.HighestPosition>0 ?(e4.CurrentPosition/e4.HighestPosition)*100.0 : 0.0;
var lK=q as IMyMotorStator;if(lK!=null)return Math.Min(100.0,Math.Abs(lK.Angle)/6.28318*100.0);var mD=q as IMyWarhead;if(mD!=null)return mD.IsArmed ? 100.0 : 0.0;
var ib=q as IMySensorBlock;if(ib!=null)return!ib.LastDetectedEntity.IsEmpty()? 100.0 :(ib.Enabled ? 25.0 : 0.0);var gx=q as IMyCameraBlock;
if(gx!=null)return gx.IsActive ? 100.0 :(gx.Enabled ? 50.0 : 0.0);var j5=q as IMyParachute;if(j5!=null)return(double)j5.OpenRatio*100.0;
var fs=q as IMyTurretControlBlock;if(fs!=null)return fs.HasTarget ? 100.0 :(fs.IsUnderControl ? 50.0 :(fs.Enabled ? 10.0 : 0.0));var kD=q as IMyFunctionalBlock;
if(kD!=null)return kD.Enabled ? 100.0 : 0.0;return 0.0;}static double na(IMyTerminalBlock jK,string gP){int jY=gP.IndexOf(':');string hi=gP.Substring(0,jY).ToLowerInvariant();
var qw=new MyItemType("MyObjectBuilder_"+(hi=="ammo" ? "AmmoMagazine" : char.ToUpper(hi[0])+hi.Substring(1)),gP.Substring(jY+1));double l9=0;for(int g5=0;
g5<jK.InventoryCount;g5++)l9+=(double)jK.GetInventory(g5).GetItemAmount(qw);return l9;}double jc(IMyTerminalBlock r,string kv){if(r==null)return 0.0;
string a=kv.ToLowerInvariant();if(a=="percent"||a=="pct")return nH(r);if(a.IndexOf(':')>0)return na(r,kv);var ba=r as IMyBatteryBlock;
if(ba!=null){if(a=="current"||a=="stored"||a=="val"||a=="value"||a=="raw")return ba.CurrentStoredPower;if(a=="max"||a=="capacity")return ba.MaxStoredPower;
if(a=="output")return ba.CurrentOutput;if(a=="maxoutput")return ba.MaxOutput;if(a=="input")return ba.CurrentInput;if(a=="maxinput")return ba.MaxInput;
return ba.CurrentStoredPower;}var hf=r as IMyJumpDrive;if(hf!=null){if(a=="max"||a=="capacity")return hf.MaxStoredPower;return hf.CurrentStoredPower;
}var fn=r as IMyGasTank;if(fn!=null){if(a=="max"||a=="capacity")return fn.Capacity;return fn.FilledRatio*fn.Capacity;}var mC=r as IMyAirVent;
if(mC!=null){if(a=="max"||a=="capacity")return 100.0;return mC.GetOxygenLevel()*100.0;}var j0=r as IMyCargoContainer;if(j0!=null){var eN=j0.GetInventory();
if(eN!=null){if(a=="mass")return(double)eN.CurrentMass;if(a=="max"||a=="capacity")return(double)eN.MaxVolume;return(double)eN.CurrentVolume;}return 0.0;}var hW=r as IMyPowerProducer;
if(hW!=null){if(a=="max"||a=="capacity")return hW.MaxOutput;return hW.CurrentOutput;}var kj=r as IMyDoor;if(kj!=null){if(a=="max")return 1.0;return(double)kj.OpenRatio;
}var iv=r as IMyThrust;if(iv!=null){if(a=="max"||a=="capacity")return iv.MaxEffectiveThrust;return iv.CurrentThrust;}var ir=r as IMyMotorSuspension;
if(ir!=null){if(a=="brake")return ir.Brake ? 1.0 : 0.0;return ir.Friction*100.0;}var kb=r as IMyShipConnector;if(kb!=null){return kb.Status==MyShipConnectorStatus.Connected ? 1.0 : 0.0;
}var hT=r as IMyProductionBlock;if(hT!=null){if(a=="producing"||a=="active"||a=="running"||a=="state")return hT.IsProducing ? 1.0 : 0.0;if(a=="queue")return hT.IsQueueEmpty ? 0.0 : 1.0;
}var gg=r as IMyAssembler;if(gg!=null){if(a=="disassembling")return gg.Mode==MyAssemblerMode.Disassembly ? 1.0 : 0.0;if(a=="repeating"||a=="loop")return gg.Repeating ? 1.0 : 0.0;
}var gY=r as IMyGasGenerator;if(gY!=null){if(a=="autorefill")return gY.AutoRefill ? 1.0 : 0.0;if(a=="active")return gY.Enabled ? 1.0 : 0.0;}var fv=r as IMyLargeTurretBase;
if(fv!=null){if(a=="shooting")return fv.IsShooting ? 1.0 : 0.0;if(a=="target"||a=="hastarget")return fv.HasTarget ? 1.0 : 0.0;if(a=="range")return fv.Range;
}var cn=r as IMyProjector;if(cn!=null){if(a=="built")return cn.TotalBlocks-cn.RemainingBlocks;if(a=="remaining")return cn.RemainingBlocks;if(a=="total")return cn.TotalBlocks;
if(a=="projecting")return cn.IsProjecting ? 1.0 : 0.0;}var cc=r as IMyGyro;if(cc!=null){if(a=="override")return cc.GyroOverride ? 1.0 : 0.0;if(a=="pitch")return cc.Pitch;
if(a=="yaw")return cc.Yaw;if(a=="roll")return cc.Roll;if(a=="power")return cc.GyroPower*100.0;}var kP=r as IMyGravityGeneratorBase;if(kP!=null){if(a=="g"||a=="accel")return kP.GravityAcceleration;
}var kN=r as IMyLandingGear;if(kN!=null){if(a=="locked")return kN.IsLocked ? 1.0 : 0.0;}var jE=r as IMyRadioAntenna;if(jE!=null){if(a=="radius"||a=="range")return jE.Radius;
}var jQ=r as IMyBeacon;if(jQ!=null){if(a=="radius"||a=="range")return jQ.Radius;}var hq=r as IMyLightingBlock;if(hq!=null){if(a=="intensity")return hq.Intensity;
if(a=="radius")return hq.Radius;}var e5=r as IMyPistonBase;if(e5!=null){if(a=="pos"||a=="position"||a=="extension")return e5.CurrentPosition;
if(a=="velocity")return e5.Velocity;if(a=="max")return e5.HighestPosition;}var dI=r as IMyMotorStator;if(dI!=null){if(a=="angle"||a=="deg")return dI.Angle*180.0/Math.PI;
if(a=="rad")return dI.Angle;if(a=="velocity"||a=="rpm")return dI.TargetVelocityRPM;if(a=="locked")return dI.RotorLock ? 1.0 : 0.0;}var iD=r as IMyWarhead;
if(iD!=null){if(a=="armed")return iD.IsArmed ? 1.0 : 0.0;if(a=="countdown")return iD.DetonationTime;}var lZ=r as IMySensorBlock;if(lZ!=null){
if(a=="detected")return!lZ.LastDetectedEntity.IsEmpty()? 1.0 : 0.0;}var gy=r as IMyCameraBlock;if(gy!=null){if(a=="range")return gy.AvailableScanRange;
if(a=="active")return gy.IsActive ? 1.0 : 0.0;}var j6=r as IMyParachute;if(j6!=null){if(a=="open"||a=="openratio")return(double)j6.OpenRatio;}var cv=r as IMyTurretControlBlock;
if(cv!=null){if(a=="target"||a=="hastarget")return cv.HasTarget ? 1.0 : 0.0;if(a=="azimuth")return cv.AzimuthRotor!=null ? cv.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(a=="elevation")return cv.ElevationRotor!=null ? cv.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var kE=r as IMyFunctionalBlock;if(kE!=null){return kE.Enabled ? 1.0 : 0.0;
}return 0.0;}double cS(string c4,string kw){aL bb;cY.TryGetValue(c4,out bb);if(bb!=null&&bb.a1==aw.cK){
string o1= !string.IsNullOrEmpty(bb.aN)? bb.aN : c4;double kc;if(an.TryGetValue(o1,out kc))return kc;return bb.d0;
}double kp;if(an.TryGetValue(c4,out kp))return kp;List<IMyTerminalBlock>c5;if(!b1.TryGetValue(c4,out c5)||c5.Count==0)throw new InvalidOperationException($"HELM: bind '{c4}' resolved to zero blocks — check the name/typo");
if(c5.Count==1)return jc(c5[0],kw);var mA=new List<double>();foreach(var n2 in c5)mA.Add(jc(n2,kw));return mJ.mI(bb!=null ? bb.cz : al.fC,mA);
}double fW(string od){return cS(od,"percent");}static string a0(double mx,br cj){if(cj==null||(string.IsNullOrEmpty(cj.cX)&&string.IsNullOrEmpty(cj.cL)&&cj.I=="percent")){
return iX(mx);}return E.m3(mx,cj.cX,cj.cL);}bool nB(z cr){IMyTextSurface fl;if(!b2.TryGetValue(cr,out fl))return false;
if(cr.bo!=null)return nv(cr,fl);if(cr.cF.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return nC(cr,fl);
}return nD(cr,fl);}static void v(a5 oi,Vector2 pH,Vector2 p9,Color ok,float pW=0f,string ov="SquareSimple"){oi.l.Add(new MySprite{
Type=SpriteType.TEXTURE,Data=ov,Position=pH,Size=p9,Color=ok,RotationOrScale=pW});}void mQ(z o,IMyTextSurface mb,a5 f){
f.l.Clear();f.d7.Clear();Vector2 L=mb.SurfaceSize;Vector2 qt=mb.TextureSize;Vector2 aY=(qt-L)/2f;
float eY=Math.Max(4f,L.X*0.02f);float dA=Math.Max(4f,L.Y*0.02f);if(o.bt=="compact"||(o.bt=="auto"&&L.Y<=260f&&L.Y>150f)){
eY=4f;dA=4f;}else if(o.bt=="corner"||(o.bt=="auto"&&L.Y<=150f)){eY=2f;dA=2f;}float mr=L.X-(eY*2f);
Color gn=new Color(10,12,16);bool kl=true;if(!string.IsNullOrEmpty(o.bn)){var df=k.bW(o.bn);if(df.cO)kl=false;
else gn=new Color(df.aQ,df.aO,df.aK,df.dS);}f.bO= -1;f.fB=false;f.fz=R(o.iG ?? "#5b0f0f");if(o.dW!=null&&o.dW.Length>1){
int el=16;float jM=(L.Y-4f)/el;f.bO=f.l.Count;f.bN=new Color[el];for(int cd=0;cd<el;cd++){k gV=E.iY(o.dW,100.0*cd/(el-1));
f.bN[cd]=new Color(gV.aQ,gV.aO,gV.aK);v(f,new Vector2(aY.X+L.X/2f,aY.Y+2f+jM*(cd+0.5f)),new Vector2(L.X-4f,jM+1f),f.bN[cd]);
}}else if(kl){f.bO=f.l.Count;f.bN=new[]{gn};v(f,aY+(L/2f),L-new Vector2(4f,4f),gn);}if(!o.cF.Equals("none",StringComparison.OrdinalIgnoreCase)){
Color jV=o.aM!=null&&o.aM.Length>0 ? R(o.aM[0]): R(o.dZ ?? "40, 60, 85");
Color oh=o.aM!=null&&o.aM.Length>1 ? R(o.aM[o.aM.Length-1]): jV;
v(f,new Vector2(aY.X+L.X/2f,aY.Y+dA*0.5f),new Vector2(mr,2f),jV);v(f,new Vector2(aY.X+L.X/2f,aY.Y+L.Y-(dA*0.5f)),new Vector2(mr,2f),oh);
}var hJ=o.az;if(hJ.Count==0){hJ=new List<am>{new am{cR="main",cH=0,cT=0,cU=o.bT,cV=o.bU,aP=o.fN}
};}float kR=8f;foreach(var P in hJ){var eX=E.mS(P.cH,P.cT,P.cU,P.cV,o.bT,o.bU,L.X,L.Y,eY,dA,kR,kR);
float aC=aY.X+eX.jr;float dz=aY.Y+eX.js;float aD=eX.jq;float dy=eX.i_;if(!string.IsNullOrEmpty(P.cA)){
var dx=k.bW(P.cA);if(!dx.cO)v(f,new Vector2(aC+aD/2f,dz+dy/2f),new Vector2(aD,dy),new Color(dx.aQ,dx.aO,dx.aK,dx.dS));
}else if(o.bT>1||o.bU>1){Color jZ=string.IsNullOrEmpty(o.fR)? new Color(255,255,255,6): R(o.fR);if(jZ.A>0)v(f,new Vector2(aC+aD/2f,dz+dy/2f),new Vector2(aD,dy),jZ);
}string gU=P.iU ?? o.iV ?? "Monospace";float ob=E.m5(o.bt,L.X,L.Y,o.bR);float au=P.iW ?? ob;
float eF=0f;if(!string.IsNullOrEmpty(P.bv)){eF=20f*au;var gb=TextAlignment.LEFT;float iw=aC+4f;string jx=P.fX ?? o.jl ?? "left";
if(jx.Equals("center",StringComparison.OrdinalIgnoreCase)){gb=TextAlignment.CENTER;iw=aC+(aD/2f);}else if(jx.Equals("right",StringComparison.OrdinalIgnoreCase)){
gb=TextAlignment.RIGHT;iw=aC+aD-4f;}string mj=P.fY ?? o.jm;Color qu= !string.IsNullOrEmpty(mj)? R(mj): R(P.fF ?? o.dZ ?? "white");
var mo=MySprite.CreateText(P.bv,gU,qu,au*0.9f,gb);mo.Position=new Vector2(iw,dz+2f);f.l.Add(mo);
string[]gm=P.fG ?? o.aM;Color ox=gm!=null&&gm.Length>0 ? R(gm[0]): R(P.fF ?? o.dZ ?? "40, 60, 85");
v(f,new Vector2(aC+aD/2f,dz+eF-2f),new Vector2(aD-8f,1f),ox);}int pm=Math.Max(1,P.aP.Count);float py=dy-eF-8f;
float h_=py/pm;float eL=6f;for(int eK=0;eK<P.aP.Count;eK++){var w=P.aP[eK];var K=new cQ{i5=w};float pX=dz+eF+4f+(eK*h_);
float D=pX+(h_/2f);float ml=D-(11f*au);K.fL=f.l.Count;K.iZ=w.N!=null&&(w.N.Contains("{val}")||w.N.Contains("{percent}"));
var k9=MySprite.CreateText(w.N,gU,Color.White,au,TextAlignment.LEFT);k9.Position=new Vector2(aC+eL,ml);f.l.Add(k9);
float pf=Math.Max(24f*au,(w.N.Length+1)*12f*au);float qF=(!string.IsNullOrEmpty(w.cX)|| !string.IsNullOrEmpty(w.cL))? 64f*au : 52f*au;
if(w.g!=d.jk){float C=aC+eL+pf;float oa=aC+aD-eL-qF;float B=Math.Max(16f,oa-C);
float J=Math.Max(w.g==d.bu ? 8f : 4f,Math.Min((w.g==d.bu ? 20f : 16f)*au,h_*(w.g==d.bu ? 0.55f : 0.45f)));
K.cE=C;K.dV=B;K.bM=J;K.cD=D;if(w.g==d.cC){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(25,30,38));
K.V=f.l.Count;v(f,new Vector2(C,D),new Vector2(0f,J),R(w.ah));}else if(w.g==d.cP){
int h9=10;float kG=2f;float ia=Math.Max(2f,(B-(kG*(h9-1)))/h9);for(int h4=0;h4<h9;h4++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+h4*(ia+kG)+(ia/2f),D),new Vector2(ia,J),new Color(22,26,32));}}else if(w.g==d.bu){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));
int hS=8;float kH=2f;float gB=Math.Max(2f,(B-(kH*(hS-1)))/hS);for(int hH=0;hH<hS;hH++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+hH*(gB+kH)+(gB/2f),D),new Vector2(gB,2f),R(w.ah));}}else if(w.g==d.cN){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));
int ij=10;float kI=2f;float ik=Math.Max(2f,(B-(kI*(ij-1)))/ij);for(int h5=0;h5<ij;h5++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+h5*(ik+kI)+(ik/2f),D),new Vector2(ik,J),new Color(24,28,36),0.523599f);}}else if(w.g==d.cG){
int ge=10;float kJ=2f;float gf=Math.Max(2f,(B-(kJ*(ge-1)))/ge);for(int h6=0;h6<ge;h6++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+h6*(gf+kJ)+(gf/2f),D),new Vector2(gf,J),new Color(24,28,36),1.570796f,"Triangle");}}else if(w.g==d.cJ){
int gG=12;float kK=2f;float gI=Math.Max(2f,(B-(kK*(gG-1)))/gG);float kk=Math.Min(gI,J);for(int h7=0;h7<gG;h7++){K.Z.Add(f.l.Count);
v(f,new Vector2(C+h7*(gI+kK)+(gI/2f),D),new Vector2(kk,kk),new Color(24,28,36),0f,"Circle");}}else if(w.g==d.bL){
v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(25,30,38));K.V=f.l.Count;v(f,new Vector2(C+(B/2f),D),new Vector2(0f,J),R(w.ah));
v(f,new Vector2(C+(B/2f),D),new Vector2(2f,J+4f),new Color(80,110,150));}else if(w.g==d.bY){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));
K.V=f.l.Count;float hk=Math.Max(4f,J);v(f,new Vector2(C+hk*0.5f,D),new Vector2(hk,hk),new Color(40,50,65),0f,"Circle");
}else if(w.g==d.bP){float b4=Math.Max(6f,Math.Min(J,14f*au));v(f,new Vector2(C+b4*0.5f,D),new Vector2(b4,b4),new Color(28,34,46));
K.V=f.l.Count;v(f,new Vector2(C+b4*0.5f,D),new Vector2(b4*0.6f,b4*0.6f),new Color(40,50,65));}else if(w.g==d.bX){
v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(18,22,30));K.V=f.l.Count;float hQ=Math.Max(4f,J*0.7f);
v(f,new Vector2(C+hQ+2f,D),new Vector2(hQ,hQ),new Color(34,197,94),0f,"Circle");}else if(w.g==d.bK){int hP=8;
float kL=2f;float hR=Math.Max(2f,(B-(kL*(hP-1)))/hP);for(int hI=0;hI<hP;hI++){K.Z.Add(f.l.Count);v(f,new Vector2(C+hI*(hR+kL)+(hR/2f),D),new Vector2(hR,J),new Color(24,28,36));
}}else if(w.g==d.bQ){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));K.V=f.l.Count;
float kW=Math.Max(4f,J);v(f,new Vector2(C+(B/2f),D),new Vector2(kW,kW),R(w.ah),0f,"Triangle");}}var mv=MySprite.CreateText("---",gU,Color.White,au,TextAlignment.RIGHT);
mv.Position=new Vector2(aC+aD-eL,ml);K.a7=f.l.Count;f.l.Add(mv);f.d7.Add(K);}}f.i3=L;
f.i2=true;}static void aA(a5 jW,cQ l2,string qs,Color ol){var iA=jW.l[l2.a7];iA.Data=qs;iA.Color=ol;jW.l[l2.a7]=iA;
}static void jf(a5 jX,cQ l3,Color om){var kt=jX.l[l3.V];kt.Color=om;jX.l[l3.V]=kt;}bool nD(z dK,IMyTextSurface im){
a5 i;if(!ef.TryGetValue(dK,out i)){i=new a5();ef[dK]=i;}if(!i.i2||i.i3!=im.SurfaceSize){
mQ(dK,im,i);}bool G=false;bool ga=(aS%dK.fA)<dK.fA/2.0;bool iC=bx&&ga&&i.fz.A>0;
if(i.bO>=0&&iC!=i.fB){i.fB=iC;for(int dh=0;dh<i.bN.Length;dh++){var jS=i.l[i.bO+dh];jS.Color=iC ? i.fz : i.bN[dh];
i.l[i.bO+dh]=jS;}G=true;}for(int g6=0;g6<i.d7.Count;g6++){var b=i.d7[g6];var m=b.i5;double y;bool kT=false;double hw=m.d5;
try{y=cS(m.aZ,m.I);if(m.bV!=null)hw=cS(m.bV,m.fO);}catch{kT=true;y=0.0;}if(kT){
if(b.d4!= -99999.0||(b.a7>=0&&i.l[b.a7].Data!="ERR")){G=true;b.d4= -99999.0;if(b.a7>=0){
var h8=i.l[b.a7];h8.Data="ERR";h8.Color=Color.Red;i.l[b.a7]=h8;}}continue;}double aE=y;if(hw>0)y=Math.Min(100.0,aE/hw*100.0);
string op=m.cM ? E.d2(m.bS,y): m.ah;Color W=R(op);bool aT=(W==Color.Red);
if(aT&&ga){W=new Color(255,75,75);G=true;}bool av=Math.Abs(aE-b.d4)>0.1;if(av)b.d4=aE;if(m.g==d.cC){
double kx=(m.fK||m.I=="percent"||m.I=="pct")? y : fW(m.aZ);if(b.M<0f)b.M=(float)kx;
float fo=(float)kx;if(Math.Abs(b.M-fo)>0.05f){b.M+=(fo-b.M)*0.35f;G=true;}else if(b.M!=fo){
b.M=fo;G=true;}if(av)G=true;if(G){float pA=(float)Math.Max(0.0,Math.Min(1.0,b.M/100.0));float kz=b.dV*pA;
var ez=i.l[b.V];ez.Size=new Vector2(kz,b.bM);ez.Position=new Vector2(b.cE+(kz/2f),b.cD);
ez.Color=W;i.l[b.V]=ez;aA(i,b,a0(aE,m),W);}}else if(m.g==d.cP||m.g==d.cN||m.g==d.cG||m.g==d.cJ){
if(av||aT){G=true;double oF=(m.fK||m.I=="percent"||m.I=="pct")? y : fW(m.aZ);int lX=b.Z.Count;
for(int dJ=0;dJ<lX;dJ++){bool po=oF>=((dJ+0.4)/(double)lX*100.0);Color p6=po ? W : new Color(24,28,36);var lY=i.l[b.Z[dJ]];
lY.Color=p6;i.l[b.Z[dJ]]=lY;}aA(i,b,a0(aE,m),W);}}else if(m.g==d.bu){
if(av||aT){G=true;for(int eE=0;eE<7;eE++)b.d8[eE]=b.d8[eE+1];b.d8[7]=(float)y;int pG=b.Z.Count;
for(int du=0;du<pG;du++){float oU=b.d8[du];float oT=Math.Max(0.06f,Math.Min(1f,oU/100f));float j8=b.bM*oT;float oo=b.cD+(b.bM/2f)-(j8/2f);
var b9=i.l[b.Z[du]];b9.Size=new Vector2(b9.Size.Value.X,j8);b9.Position=new Vector2(b9.Position.Value.X,oo);
b9.Color=W;i.l[b.Z[du]]=b9;}aA(i,b,a0(aE,m),W);}}else if(m.g==d.bL){
if(b.M<0f)b.M=(float)y;float fp=(float)y;if(Math.Abs(b.M-fp)>0.05f){b.M+=(fp-b.M)*0.35f;
G=true;}else if(b.M!=fp){b.M=fp;G=true;}if(G||av){float kS=b.dV*0.5f;float j3=b.cE+kS;
float kg=(float)Math.Max(-1.0,Math.Min(1.0,(b.M-50.0)/50.0));float gS=Math.Abs(kg)*kS;float oG=(kg>=0)?(j3+gS*0.5f):(j3-gS*0.5f);
var eA=i.l[b.V];eA.Size=new Vector2(gS,b.bM);eA.Position=new Vector2(oG,b.cD);eA.Color=W;
i.l[b.V]=eA;aA(i,b,a0(aE,m),W);}}else if(m.g==d.bY){if(av||aT){
G=true;bool he=y>=50.0;float k3=Math.Max(4f,b.bM);float o4=he ?(b.cE+b.dV-k3*0.5f):(b.cE+k3*0.5f);
Color k2=he ?(W!=Color.White ? W : new Color(34,197,94)): new Color(60,70,85);var gQ=i.l[b.V];gQ.Position=new Vector2(o4,b.cD);
gQ.Color=k2;i.l[b.V]=gQ;aA(i,b,he ? "ON" : "OFF",k2);}}else if(m.g==d.bP){if(av||aT){
G=true;bool hd=y>=50.0;Color j4=hd ?(W!=Color.White ? W : new Color(56,189,248)): new Color(30,36,48);jf(i,b,j4);
aA(i,b,hd ? "ACTIVE" : "READY",hd ? j4 : new Color(148,163,184));}}else if(m.g==d.bX){if(av||aT){
G=true;string qg=y>=75.0 ? "ONLINE" :(y>=25.0 ? "STBY" : "OFFLINE");Color l7=y>=75.0 ? new Color(34,197,94):(y>=25.0 ? new Color(234,179,8): new Color(239,68,68));
jf(i,b,l7);aA(i,b,qg,l7);}}else if(m.g==d.bK){if(av||aT){G=true;int lD=b.Z.Count;
for(int dv=0;dv<lD;dv++){bool pp=y>=((dv+0.5)/(double)lD*100.0);Color pE=pp ? W : new Color(24,28,36);var lE=i.l[b.Z[dv]];
lE.Color=pE;i.l[b.Z[dv]]=lE;}aA(i,b,a0(aE,m),W);}}else if(m.g==d.bQ){
G=true;float e9;Color dd;string eB;bool oX=(m.I=="producing"||m.I=="active"||m.I=="running"||m.I=="state"||(y<=1.0&&m.I!="percent"));
if(oX){if(y>0.0){b.M=(b.M+0.3927f)%6.28318f;e9=b.M;dd=new Color(34,197,94);eB="RUN";}else{e9=0f;
dd=new Color(148,163,184);eB="IDLE";}}else{e9=(y>55.0)? 0f :((y<45.0)? 3.14159f : 1.57079f);dd=(y>55.0)? new Color(34,197,94):((y<45.0)? new Color(239,68,68): new Color(148,163,184));
eB=(y>55.0)? "CHG" :((y<45.0)? "DSC" : "STBY");}var gR=i.l[b.V];gR.RotationOrScale=e9;gR.Color=dd;
i.l[b.V]=gR;aA(i,b,eB,dd);}else{string ex;aL fr;cY.TryGetValue(m.aZ,out fr);ec.TryGetValue(fr!=null&&fr.aN!=null ? fr.aN : m.aZ,out ex);
if(av||aT||ex!=b.i4){b.i4=ex;G=true;Color ix=m.a_!=null ? R(E.jd(m.a_,y)): Color.White;
if(ix==Color.Red&&ga)ix=new Color(255,75,75);aA(i,b,ex ?? a0(aE,m),ix);}}if(b.iZ&&(av||aT)){
G=true;var lb=i.l[b.fL];string pB=iX(y);string pU=a0(aE,m);lb.Data=m.N.Replace("{percent}",pB).Replace("{val}",pU);
i.l[b.fL]=lb;}}if(G){using(var oK=im.DrawFrame()){oK.AddRange(i.l);}}return G;}bool nC(z bi,IMyTextSurface io){
io.FontSize=bi.bR;var et=new List<KeyValuePair<string,string>>();foreach(var j in bi.fN){double F=0.0;string aI;string aB=null;
double hx=j.d5;try{F=cS(j.aZ,j.I);if(j.bV!=null)hx=cS(j.bV,j.fO);}catch(Exception qM){
et.Add(new KeyValuePair<string,string>($"{j.N}: [UNRESOLVED]","red"));continue;}double lI=F;if(hx>0)F=Math.Min(100.0,lI/hx*100.0);string mw=a0(lI,j);
if(j.g==d.cC){double ky=(j.fK||j.I=="percent"||j.I=="pct")? F : fW(j.aZ);aI=E.nw(j.N,ky,10,mw);
aB=j.cM ? E.d2(j.bS,ky): j.ah;}else if(j.g==d.bL){aI=E.nu(j.N,F,10);
aB=j.cM ? E.d2(j.bS,F): j.ah;}else if(j.g==d.bY){aI=E.nF(j.N,F);
aB=j.ah ??(F>=50.0 ? "green" : "gray");}else if(j.g==d.bP){aI=E.nz(j.N,F);aB=j.ah ??(F>=50.0 ? "cyan" : "gray");
}else if(j.g==d.bX){aI=E.nE(j.N,F);aB=j.ah ??(F>=75.0 ? "lime" :(F>=25.0 ? "yellow" : "red"));
}else if(j.g==d.bK){aI=E.nt(j.N,F,8);aB=j.cM ? E.d2(j.bS,F):(j.ah ?? "yellow");
}else if(j.g==d.bQ){bool oY=(j.I=="producing"||j.I=="active"||j.I=="running"||j.I=="state"||(F<=1.0&&j.I!="percent"));
if(oY){char[]qe=new[]{'|','/','-','\\'};char p3=qe[Math.Abs(c1)%4];aI=F>0 ? $"{j.N} [{p3}] RUN" : $"{j.N} [==] IDLE";
aB=F>0 ? "green" : "gray";}else{aI=E.nA(j.N,F,c1);aB=j.ah ??(F>55.0 ? "green" :(F<45.0 ? "red" : "cyan"));
}}else{aI=$"{j.N}: {mw}";aB=j.a_!=null ? E.jd(j.a_,F): null;}et.Add(new KeyValuePair<string,string>(aI,aB));
}var eq=E.ny(new List<string>(et.ConvertAll(qn=>qn.Key)),cx: bi.jp);string l_=string.Join("\n",eq);
string lG;if(b_.TryGetValue(bi,out lG)&&lG==l_)return false;b_[bi]=l_;using(var oL=io.DrawFrame()){
float mG=4f;float le=mH*bi.bR;try{var lg=io.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bi.bR);
if(lg.Y>0)le=lg.Y;}catch{}for(int ce=0;ce<eq.Count;ce++){string ka=null;if(ce>0&&ce<eq.Count-1)ka=et[ce-1].Value;
var l6=MySprite.CreateText(eq[ce],"Monospace",R(ka),bi.bR,TextAlignment.LEFT);l6.Position=new Vector2(4f,mG);oL.Add(l6);
mG+=le;}}return true;}Color R(string eV){if(string.IsNullOrEmpty(eV))return Color.White;Color dp;if(jt.TryGetValue(eV,out dp))return dp;
k c6=k.bW(eV);dp=c6.cO ? Color.Transparent : new Color(c6.aQ,c6.aO,c6.aK,c6.dS);jt[eV]=dp;return dp;}public enum aw{mL,fJ,cK}
public enum al{ne,fC,ji,i7,i6}public class aL{public string Q;public aw a1;public List<string>ar=new List<string>();public string jj{
get{return ar.Count>0 ? ar[0]: "";}set{if(ar.Count==0)ar.Add(value);else ar[0]=value;}}public al cz=al.ne;
public double d0=0.0;public string fH;public string aN;public bool dU;}public enum d{jk,cC,cP,bu,cN,cG,cJ,bL,bY,bP,bX,bK,bQ}
public class cI{public string iR;public string d_;public bool d3;}public class br{public d g;public string N;public string aZ;
public string I;public bool cM;public string[]bS;public string ah;public List<cI>a_;public string iO="right";
public string cX;public string cL;public double d5;public string bV,fO;public bool fK{get{return d5>0||bV!=null;}}}public struct k{
public byte aQ,aO,aK,dS;public bool cO;public k(byte pM,byte oN,byte n3,byte nO=255){aQ=pM;aO=oN;aK=n3;dS=nO;cO=false;}public static readonly k nM=new k(0,0,0,0){
cO=true};public static readonly k d9=new k(255,255,255);public static readonly k mK=new k(0,0,0);public static readonly k nr=new k(255,0,0);
public static readonly k m7=new k(0,255,0);public static readonly k nb=new k(50,205,50);public static readonly k nN=new k(255,255,0);
public static readonly k nf=new k(255,140,0);public static readonly k mV=new k(0,255,255);public static readonly k mM=new k(0,120,255);
public static readonly k nc=new k(255,0,255);public static readonly k np=new k(128,0,128);public static readonly k m6=new k(128,128,128);
public static readonly k mW=new k(60,60,60);public static k bW(string aH){if(string.IsNullOrWhiteSpace(aH))return d9;aH=aH.Trim();
if(aH.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aH.Equals("none",StringComparison.OrdinalIgnoreCase))return nM;if(aH.StartsWith("#")){
string be=aH.Substring(1).Trim();if(be.Length==6){byte pN=Convert.ToByte(be.Substring(0,2),16);byte oO=Convert.ToByte(be.Substring(2,2),16);byte n4=Convert.ToByte(be.Substring(4,2),16);
return new k(pN,oO,n4);}if(be.Length==3){byte pO=Convert.ToByte(new string(be[0],2),16);byte oP=Convert.ToByte(new string(be[1],2),16);byte n5=Convert.ToByte(new string(be[2],2),16);
return new k(pO,oP,n5);}}if(aH.Contains(",")){var cm=aH.Split(',');if(cm.Length>=3){byte pP=(byte)Math.Max(0,Math.Min(255,int.Parse(cm[0].Trim())));
byte oQ=(byte)Math.Max(0,Math.Min(255,int.Parse(cm[1].Trim())));byte n6=(byte)Math.Max(0,Math.Min(255,int.Parse(cm[2].Trim())));byte nP=cm.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(cm[3].Trim()))):(byte)255;
return new k(pP,oQ,n6,nP);}}switch(aH.ToLowerInvariant()){case "red": return nr;case "green": return m7;case "lime": return nb;case "yellow": return nN;
case "orange": return nf;case "cyan": return mV;case "blue": return mM;case "magenta": return nc;case "purple": return np;case "white": return d9;
case "black": return mK;case "gray": case "grey": return m6;case "darkgray": case "darkgrey": return mW;default: return d9;}}}public class am{
public string cR="default";public int cH=0;public int cT=0;public int cU=1;public int cV=1;public string bv;public string fX="left";public string fY;
public string iM;public string iU;public float? iW;public string cA;public string fF;public string[]fG;public string i8="horizontal";
public List<br>aP=new List<br>();}public class z{public string a2;public string bt="standard";public string cF="graphic";public string dZ;
public string[]aM;public string jl="left";public string jm;public string iV="Monospace";public string bn;public string iK;
public int iL=10;public int iI=1;public string fR;public string[]dW;public string iG;public string bo;public string bq;
public float fE=2.5f;public string iJ="cyan";public string[]dX;public List<string>dY=new List<string>();public List<string>m4(){
if(dY!=null&&dY.Count>0)return dY;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float bR=1.0f;public int fT=0;public float fA=1.6f;public int bT=1;public int bU=1;public int jp=30;public string a3;
public int bs=0;public List<am>az=new List<am>();public List<br>fN{get{if(az.Count==0)az.Add(new am{cR="main"}
);return az[0].aP;}}}public class a6{public string fP;public List<string>d6=new List<string>();public Dictionary<string,string>fI=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>fD=new List<string>();}public class fS{public List<aL>bp=new List<aL>();public List<z>S=new List<z>();
public List<string>ay=new List<string>();public Dictionary<string,List<string>>cB=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class i0{static readonly Dictionary<string,a6>iN=new Dictionary<string,a6>(StringComparer.OrdinalIgnoreCase);
static i0(){ns();}static void ns(){a4("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void a4(string ln,string[]pz,string oe){var fm=new a6{fP=ln}
;foreach(var pu in pz){var hl=pu.Split(new[]{'='},2);string lv=fQ(hl[0]);fm.d6.Add(lv);if(hl.Length>1)fm.fI[lv]=_(hl[1].Trim());
}var pn=oe.Replace("\r\n","\n").Split('\n');foreach(var pa in pn){string lO=pa.Trim();if(lO.Length>0)fm.fD.Add(lO);}iN[ln]=fm;}static string fQ(string ck){
ck=ck.Trim();if(ck.StartsWith("$"))ck=ck.Substring(1);return ck.ToLowerInvariant();}public static fS nh(string ot){var ae=new fS();z e=null;
am u=null;var lF=nn(ot,ae.ay,ae.cB);for(int di=0;di<lF.Count;di++){string c=lF[di].Trim();
if(c.Length==0)continue;try{if(c.StartsWith("@bind")){ae.bp.Add(ni(c));}else if(c.StartsWith("@screen")){e=new z{a2=c.Substring("@screen".Length).Trim()}
;ae.S.Add(e);u=null;}else if(c.StartsWith("profile:")){t(e,"profile:");e.bt=c.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(c.StartsWith("border:")){t(e,"border:");string jL=c.Substring("border:".Length).Trim();if(u!=null)u.iM=jL;
else e.cF=jL;}else if(c.StartsWith("border_color:")){t(e,"border_color:");string jO=c.Substring("border_color:".Length).Trim();
if(u!=null)u.fF=jO;else e.dZ=jO;}else if(c.StartsWith("border_gradient:")||c.StartsWith("border_gradient(")){
t(e,"border_gradient");string bA=c.StartsWith("border_gradient:")? c.Substring("border_gradient:".Length): c.Substring("border_gradient".Length);
bA=bA.Trim();if(bA.StartsWith("(")&&bA.Contains(")"))bA=ap(bA,0);string[]l8=aq(bA).Select(pY=>pY.Trim()).ToArray();if(u!=null)u.fG=l8;
else e.aM=l8;}else if(c.StartsWith("title_align:")){t(e,"title_align:");string mf=c.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(u!=null)u.fX=mf;else e.jl=mf;}else if(c.StartsWith("title_color:")){t(e,"title_color:");string mi=c.Substring("title_color:".Length).Trim();
if(u!=null)u.fY=mi;else e.jm=mi;}else if(c.StartsWith("font_size:")){t(e,"font_size:");float md=float.Parse(c.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(u!=null)u.iW=md;else e.bR=md;}else if(c.StartsWith("font:")){t(e,"font:");string ku=c.Substring("font:".Length).Trim();
if(u!=null)u.iU=ku;else e.iV=ku;}else if(c.StartsWith("bg:")||c.StartsWith("background:")){t(e,"bg:");string jT=c.Substring(c.IndexOf(':')+1).Trim();
if(u!=null)u.cA=jT;else e.bn=jT;}else if(c.StartsWith("layout:")){t(e,"layout:");nj(c.Substring("layout:".Length).Trim(),e);
}else if(c.StartsWith("alert_period:")){t(e,"alert_period:");e.fA=Math.Max(0.2f,float.Parse(c.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(c.StartsWith("refresh:")){t(e,"refresh:");e.fT=int.Parse(c.Substring("refresh:".Length).Trim());}else if(c.StartsWith("output:")){
t(e,"output:");var e0=aq(c.Substring("output:".Length).Trim());if(e0.Count==0)throw new FormatException("output: requires a target block name");
e.a3=_(e0[0]);e.bs=e0.Count>1 ? int.Parse(e0[1]): 0;}else if(c.StartsWith("width:")){t(e,"width:");
e.jp=int.Parse(c.Substring("width:".Length).Trim());}else if(c.StartsWith("boot_msg:")||c.StartsWith("boot:")){t(e,"boot_msg:");
e.iK=_(c.Substring(c.IndexOf(':')+1).Trim());}else if(c.StartsWith("boot_stagger:")){t(e,"boot_stagger:");e.iL=int.Parse(c.Substring("boot_stagger:".Length).Trim());
}else if(c.StartsWith("boot_batch:")){t(e,"boot_batch:");e.iI=Math.Max(1,int.Parse(c.Substring("boot_batch:".Length).Trim()));
}else if(c.StartsWith("boot_duration:")){t(e,"boot_duration:");string c8=c.Substring("boot_duration:".Length).Trim().ToLowerInvariant();
bool kV=c8.EndsWith("s");if(kV)c8=c8.Substring(0,c8.Length-1).Trim();float gJ;if(float.TryParse(c8,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out gJ)){
e.fE=kV ? gJ : gJ/60f;}}else if(c.StartsWith("boot_log:")){t(e,"boot_log:");e.dY.Add(_(c.Substring("boot_log:".Length).Trim()));
}else if(c.StartsWith("banner:")){t(e,"banner:");e.bo=_(c.Substring("banner:".Length).Trim());}else if(c.StartsWith("boot_logo:")){
t(e,"boot_logo:");e.bq=_(c.Substring("boot_logo:".Length).Trim());}else if(c.StartsWith("boot_gradient:")||c.StartsWith("boot_gradient(")||c.StartsWith("banner_gradient:")||c.StartsWith("banner_gradient(")){
t(e,"boot_gradient");e.dX=jb(c);}else if(c.StartsWith("bg_gradient:")||c.StartsWith("bg_gradient(")){t(e,"bg_gradient");
e.dW=jb(c);}else if(c.StartsWith("panel_bg:")){t(e,"panel_bg:");e.fR=c.Substring("panel_bg:".Length).Trim();
}else if(c.StartsWith("alert_bg:")){t(e,"alert_bg:");e.iG=c.Substring("alert_bg:".Length).Trim();}else if(c.StartsWith("boot_color:")||c.StartsWith("banner_color:")){
t(e,"boot_color:");e.iJ=c.Substring(c.IndexOf(':')+1).Trim();}else if(c.StartsWith("orientation:")){t(e,"orientation:");
string ps=c.Substring("orientation:".Length).Trim().ToLowerInvariant();if(u!=null)u.i8=ps;}else if(c.StartsWith("@panel")){t(e,"@panel");
u=nl(c);e.az.Add(u);}else if(c.StartsWith("title:")){t(e,"title:");if(u==null){u=new am{
cR="main",cH=0,cT=0,cU=e.bT,cV=e.bU};e.az.Add(u);}u.bv=_(c.Substring("title:".Length).Trim());
}else if(c.StartsWith("line:")){t(e,"line:");if(u==null){u=new am{cR="main",cH=0,cT=0,cU=e.bT,cV=e.bU}
;e.az.Add(u);}u.aP.Add(nk(c.Substring("line:".Length).Trim()));}else{ae.ay.Add($"Line {di + 1}: unrecognized directive: \"{c}\"");
}}catch(Exception oA){ae.ay.Add($"Line {di + 1}: parse error ({oA.Message}) in \"{c}\"");}}foreach(var lP in ae.S){if(string.IsNullOrEmpty(lP.a3))ae.ay.Add($"@screen {lP.a2}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aG in ae.S){if(aG.bo!=null&& !ae.cB.ContainsKey(aG.bo))ae.ay.Add("Screen '"+aG.a2+"': banner '"+aG.bo+"' is not defined (@banner "+aG.bo+" ... @end)");
if(aG.bq!=null&& !ae.cB.ContainsKey(aG.bq))ae.ay.Add("Screen '"+aG.a2+"': boot_logo '"+aG.bq+"' is not defined (@banner "+aG.bq+" ... @end)");
}return ae;}static void nj(string gr,z lW){int hK=gr.IndexOf('(');if(hK<0)throw new FormatException("missing '(' in layout: specification");
string k1=gr.Substring(0,hK).Trim().ToLowerInvariant();string nV=ap(gr,hK);var gd=aq(nV);if(k1=="grid"){if(gd.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
lW.bT=Math.Max(1,int.Parse(gd[0]));lW.bU=Math.Max(1,int.Parse(gd[1]));}else{throw new FormatException($"unknown layout kind '{k1}' (expected grid)");
}}static am nl(string pi){string dF=pi.Substring("@panel".Length).Trim();int ek=dF.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(ek<0)throw new FormatException("@panel requires an at(...) positioning clause");
string pq=dF.Substring(0,ek).Trim();string jI=dF.Substring(ek);int lB=jI.IndexOf('(');string jG=ap(jI,lB);
var bz=aq(jG);if(bz.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var aV=new am{cR=pq,cH=int.Parse(bz[0]),cT=int.Parse(bz[1]),cU=bz.Count>2 ? int.Parse(bz[2]): 1,cV=bz.Count>3 ? int.Parse(bz[3]): 1}
;int jz=ek+lB+jG.Length+1;if(jz<dF.Length){string ao=dF.Substring(jz).Trim();int mn=ao.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(mn>=0){string dM=ao.Substring(mn+5).Trim();if(dM.StartsWith("(")&&dM.Contains(")"))aV.bv=_(ap(dM,0));else if(dM.StartsWith(":")){
string bB=dM.Substring(1).Trim();if(bB.StartsWith("\"")){int kq=bB.IndexOf('"',1);aV.bv=kq>0 ? bB.Substring(1,kq-1): _(bB);}else{int l4=bB.IndexOf(' ');
aV.bv=l4>=0 ? bB.Substring(0,l4): bB;}}}string lq=d1(ao,"orientation:");if(lq!=null)aV.i8=lq.ToLowerInvariant();string jy=d1(ao,"title_align:");
if(jy!=null)aV.fX=jy.ToLowerInvariant();string mk=d1(ao,"title_color:");if(mk!=null)aV.fY=mk;int ep=ao.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(ep>=0&& !ao.Substring(ep).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !ao.Substring(ep).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
aV.iM=d1(ao.Substring(ep),"border:");}int en=ao.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(en<0)en=ao.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(en>=0){string bc=ao.Substring(en+"border_gradient".Length).Trim();if(bc.StartsWith(":"))bc=bc.Substring(1).Trim();if(bc.StartsWith("(")&&bc.Contains(")"))bc=ap(bc,0);
aV.fG=aq(bc).Select(pZ=>pZ.Trim()).ToArray();}}return aV;}static string d1(string lQ,string k0){int kU=lQ.IndexOf(k0,StringComparison.OrdinalIgnoreCase);
if(kU<0)return null;string iB=lQ.Substring(kU+k0.Length).Trim();int l5=iB.IndexOf(' ');return l5>=0 ? iB.Substring(0,l5): iB;}static string[]jb(string hr){
int gZ=hr.IndexOfAny(new[]{':','('});string de=hr.Substring(hr[gZ]==':' ? gZ+1 : gZ).Trim();if(de.StartsWith("(")&&de.Contains(")"))de=ap(de,0);return aq(de).Select(p_=>p_.Trim()).ToArray();
}static List<string>nn(string ou,List<string>gN,Dictionary<string,List<string>>n8){var ms=new Dictionary<string,a6>(StringComparer.OrdinalIgnoreCase);
var co=(ou ?? "").Replace("\r\n","\n").Split('\n');var hF=new List<string>();int cf=0;while(cf<co.Length){string hX=co[cf++];if(hX.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var aa=new List<string>();while(cf<co.Length){string jw=co[cf++].Replace("\t","    ").TrimEnd();if(jw.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
aa.Add(jw);}while(aa.Count>0&&aa[0].Length==0)aa.RemoveAt(0);while(aa.Count>0&&aa[aa.Count-1].Length==0)aa.RemoveAt(aa.Count-1);int hc=int.MaxValue;
foreach(var f6 in aa)if(f6.Length>0)hc=Math.Min(hc,f6.Length-f6.TrimStart().Length);for(int dq=0;dq<aa.Count;dq++)if(aa[dq].Length>0)aa[dq]=aa[dq].Substring(hc);
n8[hX.TrimStart().Substring(8).Trim()]=aa;continue;}string bg=jh(hX).Trim();if(bg.Length==0)continue;if(bg.StartsWith("@template")){try{
var it=nm(bg);while(cf<co.Length){string og=co[cf++];string gs=jh(og).Trim();if(gs.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(gs.Length>0)it.fD.Add(gs);}ms[it.fP]=it;}catch(Exception oB){gN.Add($"Template parse error ({oB.Message}) in \"{bg}\"");}}
else if(bg.StartsWith("@use")){try{iQ(bg,ms,hF,gN);}catch(Exception oC){gN.Add($"@use error ({oC.Message}) in \"{bg}\"");
}}else{hF.Add(bg);}}return hF;}static a6 nm(string pj){string hY=pj.Substring("@template".Length).Trim();int hL=hY.IndexOf('(');
if(hL<0)throw new FormatException("missing '(' in @template header");string pr=hY.Substring(0,hL).Trim();string nW=ap(hY,hL);var iu=new a6{
fP=pr};foreach(var pv in aq(nW)){var hm=pv.Split(new[]{'='},2);string lw=fQ(hm[0]);iu.d6.Add(lw);if(hm.Length>1)iu.fI[lw]=_(hm[1].Trim());
}return iu;}static void iQ(string pk,Dictionary<string,a6>mt,List<string>lt,List<string>oz,int kf=0){string e8=pk.Substring("@use".Length).Trim();
int eZ=e8.IndexOf('(');string hy=eZ>=0 ? e8.Substring(0,eZ).Trim(): e8.Trim();string nX=eZ>=0 ? ap(e8,eZ): "";a6 cu=null;
if(!mt.TryGetValue(hy,out cu)){if(!iN.TryGetValue(hy,out cu))throw new FormatException($"unknown template '{hy}'");}var er=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var k5 in cu.fI)er[k5.Key]=k5.Value;var jF=aq(nX);for(int c2=0;c2<jF.Count;c2++){string ei=jF[c2];int gL=ei.IndexOf('=');if(gL>=0){
string o2=fQ(ei.Substring(0,gL));string qz=_(ei.Substring(gL+1).Trim());er[o2]=qz;}else{if(c2<cu.d6.Count){string o3=cu.d6[c2];
er[o3]=_(ei.Trim());}}}foreach(var of in cu.fD){string db=of;foreach(var eR in er){db=db.Replace("$"+eR.Key,eR.Value).Replace("{"+eR.Key+"}",eR.Value);
}if(db.StartsWith("@use")){if(kf>3)throw new FormatException("@use nested too deep");iQ(db,mt,lt,oz,kf+1);
}else lt.Add(db);}}static void t(z os,string ow){if(os==null)throw new FormatException($"'{ow}' used before any @screen declared");
}static string jh(string aF){if(string.IsNullOrEmpty(aF))return aF;bool hb=false;for(int bf=0;bf<aF.Length;bf++){char gv=aF[bf];if(gv=='"'){hb= !hb;
}else if(!hb){if(gv=='/'&&bf+1<aF.Length&&aF[bf+1]=='/'){return aF.Substring(0,bf);}if(gv=='#'){if(m9(aF,bf)){continue;}return aF.Substring(0,bf);}}}return aF;
}static bool m9(string bC,int kX){int ci=0;for(int g7=kX+1;g7<bC.Length;g7++){char b5=bC[g7];if((b5>='0'&&b5<='9')||(b5>='a'&&b5<='f')||(b5>='A'&&b5<='F'))ci++;
else break;}if(ci==3||ci==4||ci==6||ci==8){int dt=kX+1+ci;if(dt>=bC.Length||char.IsWhiteSpace(bC[dt])||bC[dt]==')'||bC[dt]==','||bC[dt]==':')return true;
}return false;}static aL ni(string pl){string hZ=pl.Substring("@bind".Length).Trim();int gM=hZ.IndexOf('=');if(gM<0)throw new FormatException("missing '=' in @bind");
string lo=hZ.Substring(0,gM).Trim();string gO=hZ.Substring(gM+1).Trim();int hM=gO.IndexOf('(');if(hM<0)throw new FormatException("missing '(' in @bind target");
string eD=gO.Substring(0,hM).Trim().ToLowerInvariant();string nY=ap(gO,hM);var a9=aq(nY);if(a9.Count==0)throw new FormatException("@bind target has no arguments");
var ac=new aL{Q=lo};if(eD=="dynamic"||eD=="var"||eD=="mem"){ac.a1=aw.cK;ac.aN=a9.Count>0 ? _(a9[0].Trim()): lo;
for(int f7=0;f7<a9.Count;f7++){var eS=a9[f7].Split(new[]{':'},2);if(eS.Length==2&&eS[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double ke;
if(double.TryParse(eS[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out ke))ac.d0=ke;else ac.fH=_(eS[1].Trim());
}}return ac;}ac.a1=eD=="group" ? aw.fJ : aw.mL;string eh=null;for(int eg=0;eg<a9.Count;eg++){var ch=a9[eg].Split(new[]{':'},2);if(ch.Length==2&&ch[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
eh=ch[1].Trim().ToLowerInvariant();}else if(ch.Length==2&&ch[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ac.dU=ch[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ac.ar.Add(_(a9[eg].Trim()));}}if(ac.ar.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ac.a1==aw.fJ){if(eh==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(eh){case "avg": ac.cz=al.fC;break;case "sum": ac.cz=al.ji;break;case "min": ac.cz=al.i7;break;case "max": ac.cz=al.i6;
break;default: throw new FormatException($"unknown agg '{eh}' (expected avg/sum/min/max)");}}return ac;}static br nk(string eo){var n=new br();
if(!eo.StartsWith("\""))throw new FormatException("line must start with a quoted label");int gK=eo.IndexOf('"',1);if(gK<0)throw new FormatException("unterminated quoted label");
n.N=eo.Substring(1,gK-1);string dE=eo.Substring(gK+1).Trim();int e_=dE.IndexOf('(');if(e_<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string x=dE.Substring(0,e_).Trim().ToLowerInvariant();string nU=ap(dE,e_);string U=dE.Substring(dE.IndexOf(')',e_)+1).Trim();
var gH=nU.Trim().Split('.');if(gH.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
n.aZ=gH[0].Trim();n.I=gH[1].Trim();if(x=="bar")n.g=d.cC;else if(x=="led")n.g=d.cP;else if(x=="spark"||x=="sparkline")n.g=d.bu;
else if(x=="hazard"||x=="stripes")n.g=d.cN;else if(x=="chevron"||x=="chevrons")n.g=d.cG;else if(x=="dots"||x=="dot")n.g=d.cJ;
else if(x=="balance"||x=="horizon"||x=="centered")n.g=d.bL;else if(x=="toggle"||x=="switch")n.g=d.bY;else if(x=="checkbox"||x=="check")n.g=d.bP;
else if(x=="status"||x=="badge"||x=="state")n.g=d.bX;else if(x=="ammo"||x=="pips")n.g=d.bK;else if(x=="flow"||x=="rate"||x=="spinner")n.g=d.bQ;
else if(x=="text")n.g=d.jk;else throw new FormatException($"unknown display function '{x}'");while(U.Length>0){int gF=U.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int cb=U.IndexOf('(');if(gF>=0&&(cb<0||gF<cb)){string eu=U.Substring(gF+4).Trim();int ff=eu.IndexOf(' ');n.iO=(ff>=0 ? eu.Substring(0,ff): eu).ToLowerInvariant();
U=(ff>=0 ? eu.Substring(ff): "").Trim();continue;}int iz=U.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(iz>=0&&(cb<0||iz<cb)){
string fw=U.Substring(iz+5).Trim();int fg=fw.IndexOf(' ');n.cX=(fg>=0 ? fw.Substring(0,fg): fw).Trim().Trim('"');U=(fg>=0 ? fw.Substring(fg): "").Trim();
continue;}int gT=U.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(gT>=0&&(cb<0||gT<cb)){string ey=U.Substring(gT+7).Trim();
int fh=ey.IndexOf(' ');n.cL=(fh>=0 ? ey.Substring(0,fh): ey).Trim().Trim('"');U=(fh>=0 ? ey.Substring(fh): "").Trim();continue;}int eU=U.IndexOf('(');
if(eU<0)break;string af=U.Substring(0,eU).Trim().ToLowerInvariant();int la=af.LastIndexOf(' ');if(la>=0)af=af.Substring(la+1).Trim();
string aU=ap(U,eU);U=U.Substring(U.IndexOf(')',eU)+1).Trim();if(af=="gradient"){n.cM=true;n.bS=aq(aU).Select(p0=>p0.Trim()).ToArray();
if(n.bS.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(af=="flat"){n.ah=aU.Trim();}else if(af=="pattern"){
string H=aU.Trim().ToLowerInvariant();if(H=="hazard"||H=="stripes")n.g=d.cN;else if(H=="chevron"||H=="chevrons")n.g=d.cG;
else if(H=="dots"||H=="dot")n.g=d.cJ;else if(H=="led")n.g=d.cP;else if(H=="balance"||H=="horizon")n.g=d.bL;
else if(H=="toggle"||H=="switch")n.g=d.bY;else if(H=="checkbox"||H=="check")n.g=d.bP;else if(H=="status"||H=="badge"||H=="state")n.g=d.bX;
else if(H=="ammo"||H=="pips")n.g=d.bK;else if(H=="flow"||H=="rate"||H=="spinner")n.g=d.bQ;}else if(af=="color"){n.a_=new List<cI>();
foreach(var hN in aq(aU)){int gC=hN.LastIndexOf(':');if(gC<0)throw new FormatException("color() entries need 'expr : color'");string lc=hN.Substring(0,gC).Trim();
string j9=hN.Substring(gC+1).Trim();if(lc.Equals("default",StringComparison.OrdinalIgnoreCase)){n.a_.Add(new cI{d3=true,d_=j9}
);}else{string qi=nJ(lc);n.a_.Add(new cI{iR=qi,d_=j9});}}if(!n.a_.Any(pQ=>pQ.d3))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
}else if(af=="dir"||af=="direction"){n.iO=aU.Trim().ToLowerInvariant();}else if(af=="unit"){n.cX=aU.Trim().Trim('"');}else if(af=="max"){
double lj;if(double.TryParse(aU.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out lj))n.d5=lj;else{var hv=aU.Trim().Split('.');
if(hv.Length!=2)throw new FormatException("max() takes a number or bindname.field");n.bV=hv[0].Trim();n.fO=hv[1].Trim();}}else if(af=="format"||af=="fmt"){
n.cL=aU.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{af}'");}}return n;}static string ap(string fb,int hD){
int gE=0;for(int dj=hD;dj<fb.Length;dj++){if(fb[dj]=='(')gE++;else if(fb[dj]==')'){gE--;if(gE==0)return fb.Substring(hD+1,dj-hD-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aq(string nZ){return nZ.Split(',').Select(p1=>p1.Trim()).Where(p2=>p2.Length>0).ToList();
}static string nJ(string ca){ca=ca.Trim();foreach(var pt in new[]{">=","<=","==",">","<"}){int g8=ca.IndexOf(pt,StringComparison.Ordinal);
if(g8>0)return ca.Substring(g8).Trim();if(g8==0)return ca;}throw new FormatException($"color() rule has no recognizable comparison operator: '{ca}'");}
static string _(string bD){if(bD.Length>=2&&bD[0]=='"'&&bD[bD.Length-1]=='"')return bD.Substring(1,bD.Length-2);return bD;}}public static class mJ{public static double mI(al nQ,List<double>cw){
if(cw==null||cw.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(nQ){case al.fC: return cw.Average();case al.ji: return cw.Sum();
case al.i7: return cw.Min();case al.i6: return cw.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class E{
public static string nw(string pc,double e1,int jN=10,string mz=null){e1=Math.Max(0,Math.Min(100,e1));int kA=(int)Math.Round(jN*(e1/100.0));
string n9=new string('#',kA)+new string('-',jN-kA);string qA=mz!=null ? mz : $"{e1:0}%";return $"{pc} [{n9}] {qA}";
}public static string m3(double bm,string iy=null,string eC=null){if(iy=="auto"){double f8=Math.Abs(bm);if(f8>=1e9)return(bm/1e9).ToString("F1")+"G";
if(f8>=1e6)return(bm/1e6).ToString("F1")+"M";if(f8>=1e3)return(bm/1e3).ToString("F1")+"k";string oD= !string.IsNullOrEmpty(eC)? eC : "F0";return bm.ToString(oD);
}string kC= !string.IsNullOrEmpty(eC)? eC :(Math.Abs(bm)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(iy)){return bm.ToString(kC);}return bm.ToString(kC)+iy;
}public static float m5(string pI,float mc,float ip,float ev=1.0f){string cl=(pI ?? "standard").ToLowerInvariant();
if(cl=="compact"||(cl=="auto"&&mc<=300&&ip<=300&&ip>160)){return ev*0.75f;}if(cl=="corner"||(cl=="auto"&&ip<=160)){
return ev*0.55f;}if(cl=="wide"||(cl=="auto"&&mc>=1000)){return ev*1.15f;}return ev;}public static string nu(string pd,double e2,int bd=5){
e2=Math.Max(0,Math.Min(100,e2));double ew=(e2-50.0)/50.0;int dr=0;int dH=0;if(ew< -0.01)dr=(int)Math.Round(-ew*bd);
else if(ew>0.01)dH=(int)Math.Round(ew*bd);if(dr>bd)dr=bd;if(dH>bd)dH=bd;string pg=new string('-',bd-dr)+new string('<',dr);
string pV=new string('>',dH)+new string('-',bd-dH);return $"{pd} [{pg}|{pV}] {e2:0}%";}public static string nF(string k7,double qC){
return qC>=50.0 ? $"{k7} [====O] ON" : $"{k7} [O====] OFF";}public static string nz(string k8,double qD){return qD>=50.0 ? $"{k8} [X] ACTIVE" : $"{k8} [ ] READY ";
}public static string nE(string ho,double my){if(my>=75.0)return $"{ho} [ ONLINE  ]";if(my>=25.0)return $"{ho} [ STANDBY ]";return $"{ho} [ OFFLINE ]";
}public static string nt(string pe,double e3,int lf=8){e3=Math.Max(0,Math.Min(100,e3));int kB=(int)Math.Round(lf*(e3/100.0));
string pF=new string('|',kB)+new string('.',lf-kB);return $"{pe} [{pF}] {e3:0}%";}public static string nA(string hp,double dC,int mm=0){
char[]qd=new[]{'|','/','-','\\'};char lR=qd[(mm>=0 ? mm : 0)%4];if(dC>55.0)return $"{hp} {lR} ▲ CHG {dC:0}%";if(dC<45.0)return $"{hp} {lR} ▼ DSC {dC:0}%";
return $"{hp} ● STBY {dC:0}%";}public static bool nd(string ag,string dm){if(ag==null||dm==null)return false;if(ag=="*"||ag==dm)return true;
if(!ag.Contains("*")&& !ag.Contains("?"))return string.Equals(ag,dm,StringComparison.OrdinalIgnoreCase);int ak=0,dk=0,ih= -1,hu=0;while(dk<dm.Length){
if(ak<ag.Length&&(ag[ak]=='?'||char.ToLowerInvariant(ag[ak])==char.ToLowerInvariant(dm[dk]))){ak++;dk++;}else if(ak<ag.Length&&ag[ak]=='*'){
ih=ak;hu=dk;ak++;}else if(ih!= -1){ak=ih+1;hu++;dk=hu;}else{return false;}}while(ak<ag.Length&&ag[ak]=='*')ak++;return ak==ag.Length;
}public static int mN(double pw,int mq){return Math.Max(0,Math.Min(mq,1+(int)(Math.Max(0.0,pw)*mq)));}public static string d2(string[]ct,double hO){
if(ct==null||ct.Length==0)return "white";hO=Math.Max(0,Math.Min(100,hO));int g9=(int)Math.Floor(hO/100.0*ct.Length);if(g9>=ct.Length)g9=ct.Length-1;
return ct[g9];}public static k iY(string[]bl,double pC){if(bl==null||bl.Length==0)return k.d9;if(bl.Length==1)return k.bW(bl[0]);
double pT=Math.Max(0.0,Math.Min(1.0,pC/100.0));double lV=pT*(bl.Length-1);int ha=(int)Math.Floor(lV);int oW=Math.Min(bl.Length-1,ha+1);
double il=lV-ha;k b6=k.bW(bl[ha]);k gw=k.bW(bl[oW]);byte pR=(byte)Math.Round(b6.aQ+(gw.aQ-b6.aQ)*il);
byte oR=(byte)Math.Round(b6.aO+(gw.aO-b6.aO)*il);byte n7=(byte)Math.Round(b6.aK+(gw.aK-b6.aK)*il);return new k(pR,oR,n7);}public static string jd(List<cI>lN,double qE){
foreach(var h0 in lN){if(h0.d3)continue;if(m1(h0.iR,qE))return h0.d_;}var kd=lN.FirstOrDefault(pS=>pS.d3);return kd!=null ? kd.d_ : null;
}static bool m1(string dc,double dR){dc=dc.Trim();foreach(var hC in new[]{">=","<=","==",">","<"}){if(dc.StartsWith(hC)){double dG=double.Parse(dc.Substring(hC.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(hC){case ">=": return dR>=dG;case "<=": return dR<=dG;case "==": return Math.Abs(dR-dG)<1e-9;case ">": return dR>dG;case "<": return dR<dG;
}}}throw new FormatException($"unsupported comparison expression: '{dc}'");}public static List<string>ny(List<string>oq,int cx){
var eW=new List<string>();eW.Add("+"+new string('-',cx+2)+"+");foreach(var hs in oq){string qv=hs.Length>cx ? hs.Substring(0,cx): hs;
string px=qv.PadRight(cx);eW.Add("| "+px+" |");}eW.Add("+"+new string('-',cx+2)+"+");return eW;}public struct ja{public float jr,js,jq,i_;
}public static ja mS(int j7,int lL,int fi,int fj,int dO,int dP,float qm,float ql,float lx,float ly,float g0,float g1){
dO=Math.Max(1,dO);dP=Math.Max(1,dP);fi=Math.Max(1,Math.Min(dO-j7,fi));fj=Math.Max(1,Math.Min(dP-lL,fj));
float n0=qm-(lx*2f);float n_=ql-(ly*2f);float j2=(n0-(g0*(dO-1)))/dO;float j1=(n_-(g1*(dP-1)))/dP;
float pK=lx+j7*(j2+g0);float pL=ly+lL*(j1+g1);float pJ=fi*j2+((fi-1)*g0);float pD=fj*j1+((fj-1)*g1);return new ja{
jr=pK,js=pL,jq=pJ,i_=pD};}public struct je{public int ng;public int fM;public int T;public int ai;public int cW;
public int ax;public int nK;public int nL;public int mZ;public int m_;
public double m0;}public static je mT(z bj){var h=new je();var lA=bj.az!=null&&bj.az.Count>0 ? bj.az : new List<am>{
new am{aP=bj.fN}};h.ng=lA.Count;h.fM=0;h.T=0;h.ai=0;if(!string.IsNullOrEmpty(bj.bn)&& !bj.bn.Equals("none",StringComparison.OrdinalIgnoreCase))h.T++;
if(!string.IsNullOrEmpty(bj.cF)&& !bj.cF.Equals("none",StringComparison.OrdinalIgnoreCase))h.T+=2;foreach(var dw in lA){
if(!string.IsNullOrEmpty(dw.cA)&& !dw.cA.Equals("none",StringComparison.OrdinalIgnoreCase))h.T++;if(!string.IsNullOrEmpty(dw.bv))h.T+=2;
h.fM+=dw.aP.Count;foreach(var pb in dw.aP){h.nK++;h.nL++;h.T+=2;switch(pb.g){case d.cC: h.cW++;
h.ax++;h.T+=2;h.ai++;break;case d.cP: h.ax+=10;h.T+=10;
h.ai+=10;break;case d.cN: h.ax+=10;h.T+=11;h.ai+=10;break;case d.cG: h.ax+=10;
h.T+=10;h.ai+=10;break;case d.cJ: h.ax+=12;h.T+=12;h.ai+=12;
break;case d.bu: h.cW++;h.ax+=8;h.T+=9;h.ai+=8;break;case d.bL: h.cW+=2;
h.ax++;h.T+=3;h.ai++;break;case d.bY: case d.bP: h.cW++;
h.ax++;h.T+=2;h.ai++;break;case d.bX: case d.bQ: h.cW++;h.ax+=2;
h.T+=3;h.ai+=2;break;case d.bK: h.ax+=8;h.T+=8;h.ai+=8;
break;default: h.ai++;break;}}}h.mZ=25+(h.fM*2);h.m_=180+(h.ai*4);
h.m0=0.0;return h;}}