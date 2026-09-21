fI s;string fT=null;Dictionary<string,List<IMyTerminalBlock>>bZ=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>d6=new Dictionary<string,bool>();
Dictionary<string,aL>cV=new Dictionary<string,aL>();Dictionary<string,Color>ji=new Dictionary<string,Color>();List<IMyTerminalBlock>a7=new List<IMyTerminalBlock>();
bool d3;int fR;string d4="";StringBuilder cY=new StringBuilder();readonly Dictionary<string,double>an=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>d5=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener cW;bool bx=false;
Dictionary<z,IMyTextSurface>b_=new Dictionary<z,IMyTextSurface>();Dictionary<z,string>bY=new Dictionary<z,string>();
static readonly string[]iZ=new string[101];static Program(){for(int ez=0;ez<=100;ez++){iZ[ez]=ez+"%";}}static string iM(double qm){int dh=(int)Math.Round(qm);
if(dh<0)dh=0;else if(dh>100)dh=100;return iZ[dh];}class a4{public List<MySprite>k=new List<MySprite>();public List<cO>d0=new List<cO>();
public int bN= -1;public Color[]bM;public Color fs;public bool ft;public Vector2 iT;public bool iS=false;}class cO{
public bs iV;public int U= -1;public int a6= -1;public int fC= -1;public bool iO=false;public List<int>W=new List<int>();
public float[]d1=new float[8];public float cC;public float dP;public float bL;public float cB;public double dZ= -99999.0;
public string iU;public float M= -1f;}Dictionary<z,a4>d8=new Dictionary<z,a4>();int jk=0;
double fV=0;double b1=0;int cX=0;Dictionary<z,double>aR=new Dictionary<z,double>();int b0=0;const string mV="0.11.0";
IMyTextSurface a8;bool fQ=true;double fP=0;string jj="";string fU="";int d7=0,fS=0;List<KeyValuePair<string,int>>bX=new List<KeyValuePair<string,int>>();
const double nc=120;const float mt=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{cW=IGC.RegisterBroadcastListener("HELM");
cW.SetMessageCallback("HELM_IGC");}catch{}fJ();i5();iR();}public void Main(string jv,UpdateType qi){
double kb=Runtime.TimeSinceLastRun.TotalSeconds;b1+=kb;if((qi&UpdateType.IGC)!=0||jv=="HELM_IGC"){na();}string lv=(jv ?? "").Trim();
string aj=lv.ToLowerInvariant();if(aj=="reload"){fJ();}else if(aj=="refresh"){fK();}else if(aj=="boot"){i5();}else if(aj=="console on"||aj=="console off"){
fQ=aj=="console on";iR();fP=0;}else if(aj=="scan"||aj=="diagnose"||aj=="test"){nv();return;}else if(aj.StartsWith("set ")){
string e0=lv.Substring(4).Trim();int ke=e0.IndexOf('=');int pW=e0.IndexOf(' ');int h1=ke>0 ? ke : pW;if(h1>0){string kN=e0.Substring(0,h1).Trim();
string mg=e0.Substring(h1+1).Trim();iw($"{kN}={mg}");Echo($"[HELM] Set dynamic variable '{kN}' = {mg}");}return;}else if(aj.StartsWith("alert")){
string dL=aj.Length>5 ? aj.Substring(5).Trim(): "toggle";if(dL=="toggle")bx= !bx;else bx=dL=="red"||dL=="on"||dL=="1"||dL=="true";
Echo($"[HELM] Red alert state: {bx}");return;}else if(Me.CustomData!=fT){fJ();}b0++;fV+=kb;if(fV>=nc){
fV=0;fK();}if(s==null||s.R.Count==0){jd();iE();Echo("HELM: no valid @screen configured. Check Custom Data.");
return;}var hX=s.R[jk%s.R.Count];jk++;fU=hX.a1;bool fZ=aR.Count>0;if(fZ)nk();
if(!aR.ContainsKey(hX)&&no(hX))fZ=true;cX=fZ ? 0 : cX+1;Runtime.UpdateFrequency=cX<=s.R.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
jd();iE();d7=Runtime.CurrentInstructionCount;if(d7>fS)fS=d7;}void iE(){if(d4.Length>0)Echo(d4);
if(cY.Length>0)Echo(cY.ToString());}List<IMyTerminalBlock>dO(){if(!d3){a7.Clear();GridTerminalSystem.GetBlocks(a7);int pF=a7.Count;
a7.RemoveAll(nM=> !nM.IsSameConstructAs(Me));fR=pF-a7.Count;d3=true;}return a7;}IMyTerminalBlock iI(string k7){
var f6=GridTerminalSystem.GetBlockWithName(k7);if(f6==null||f6.IsSameConstructAs(Me))return f6;var f1=dO();for(int eA=0;eA<f1.Count;eA++)if(f1[eA].CustomName.IndexOf(k7,StringComparison.OrdinalIgnoreCase)>=0)return f1[eA];
return null;}void mD(string k8,List<IMyTerminalBlock>dj){var ks=GridTerminalSystem.GetBlockGroupWithName(k8);if(ks==null)return;int jF=dj.Count;
var dI=new List<IMyTerminalBlock>();ks.GetBlocks(dI);iu(dI,dj);if(dj.Count>jF)return;var kD=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(kD);
foreach(var mq in kD){if(mq.Name.IndexOf(k8,StringComparison.OrdinalIgnoreCase)<0)continue;dI.Clear();mq.GetBlocks(dI);iu(dI,dj);if(dj.Count>jF)return;
}}void iu(List<IMyTerminalBlock>ow,List<IMyTerminalBlock>kL){foreach(var f7 in ow)if(f7.IsSameConstructAs(Me)&& !kL.Contains(f7))kL.Add(f7);}void fJ(){
fT=Me.CustomData;s=iQ.m3(fT);d4="";if(s.ay.Count>0){var lE=new StringBuilder("HELM config errors:\n");
foreach(var og in s.ay)lE.AppendLine(" - "+og);d4=lE.ToString();}cV.Clear();foreach(var f8 in s.bq)if(!cV.ContainsKey(f8.P))cV[f8.P]=f8;
d8.Clear();bY.Clear();aR.Clear();fK();}void na(){try{while(cW!=null&&cW.HasPendingMessage){
var k4=cW.AcceptMessage();if(k4.Data is string){iw((string)k4.Data);}}}catch{}}void iw(string lo){
if(string.IsNullOrEmpty(lo))return;var ll=lo.Split(';');for(int gT=0;gT<ll.Length;gT++){var dx=ll[gT].Trim();if(dx.Length==0)continue;int c6=dx.IndexOf('=');
if(c6<0)c6=dx.IndexOf(':');if(c6>0){string cf=dx.Substring(0,c6).Trim();string aJ=dx.Substring(c6+1).Trim();if(cf.Equals("alert",StringComparison.OrdinalIgnoreCase)){
bx=aJ.Equals("red",StringComparison.OrdinalIgnoreCase)||aJ=="1"||aJ.Equals("true",StringComparison.OrdinalIgnoreCase)||aJ.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double lb;d5.Remove(cf);if(double.TryParse(aJ,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out lb)){
an[cf]=lb;}else if(aJ.Equals("true",StringComparison.OrdinalIgnoreCase)||aJ.Equals("on",StringComparison.OrdinalIgnoreCase)){an[cf]=100.0;
}else if(aJ.Equals("false",StringComparison.OrdinalIgnoreCase)||aJ.Equals("off",StringComparison.OrdinalIgnoreCase)){an[cf]=0.0;}else{d5[cf]=aJ;
}}}}}void ac(int p4,string qb){bX.Add(new KeyValuePair<string,int>(qb,p4));}static string jc(string hP,int k6){return hP.Length>k6 ? hP.Substring(0,k6): hP;
}void iR(){var hH=Me as IMyTextSurfaceProvider;if(hH==null||hH.SurfaceCount<1)return;a8=hH.GetSurface(0);a8.ContentType=fQ ? ContentType.SCRIPT : ContentType.NONE;
a8.Script="";}void jd(){if(b1>=fP){fP=b1+(cX>s.R.Count+1 ? 5.0 : 1.0);mB();if(fQ&&a8!=null)mK();
}Echo(jj);}void mB(){bX.Clear();int gi=0,jI=0;foreach(var jx in s.bq){List<IMyTerminalBlock>hd;if(jx.a_==aw.cI){
gi++;continue;}if(bZ.TryGetValue(jx.P,out hd)){jI+=hd.Count;if(hd.Count>0)gi++;}}int kf=s.ay.Count;ac(0,"HELM-[Kernel-(v"+mV+")]");
ac(4,"------------------------------");ac(1,"krn-cfg");ac(2," screens "+s.R.Count+"  binds "+gi+"/"+s.bq.Count+"  errors "+kf);
ac(2," alert "+(bx ? "RED" : "off")+"  blocks "+jI+"  docked-ignored "+fR);if(kf>0)ac(3," ! "+jc(s.ay[0],40));
ac(1,"cfg-dsp");int pT=0;foreach(var hQ in s.R){if(++pT>10){ac(2," +"+(s.R.Count-10)+" more");break;}IMyTextSurface h2;string lN="--",h6="no block";
if(b_.TryGetValue(hQ,out h2)){lN=h2.SurfaceSize.X.ToString("0")+"x"+h2.SurfaceSize.Y.ToString("0");h6=aR.ContainsKey(hQ)? "boot" : "live";
}ac(h6=="no block" ? 3 : 2," "+jc(hQ.a1,14).PadRight(14)+" "+lN.PadRight(9)+" "+h6);}ac(1,"perf");ac(2," tick "+b0+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ac(2," instr "+d7+"  peak "+fS+"  max "+Runtime.MaxInstructionCount);ac(2," rate "+Runtime.UpdateFrequency+"  idle "+cX+"  up "+(int)b1+"s");
if(fU.Length>0)ac(2," last "+fU);var lF=new StringBuilder();foreach(var oQ in bX)lF.AppendLine(oQ.Key);jj=lF.ToString();}static Color mG(int p5){
switch(p5){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void mK(){Vector2 bE=a8.SurfaceSize;Vector2 hm=(a8.TextureSize-bE)/2f;int gp=30;foreach(var oR in bX)gp=Math.Max(gp,oR.Key.Length);
Vector2 go=a8.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float lG=Math.Min(1f,Math.Min(bE.X*0.94f/(gp*go.X),bE.Y*0.94f/(bX.Count*go.Y)));
float qs=hm.X+bE.X*0.03f;float mr=hm.Y+bE.Y*0.03f;var h3=new List<MySprite>();h3.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=hm+bE/2f,Size=bE,Color=new Color(12,12,16)}
);foreach(var kS in bX){cw(h3,kS.Key,mG(kS.Value),qs,mr,lG);mr+=go.Y*lG;}using(var oq=a8.DrawFrame()){oq.AddRange(h3);}}static readonly List<string>mA=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void i5(){aR.Clear();if(s==null)return;for(int eB=0;eB<s.R.Count;eB++){var e3=s.R[eB];if(e3.fw>0f)aR[e3]=b1+(eB/Math.Max(1,e3.ix))*e3.iA/60.0;
}}void nk(){List<z>c5=null;int oC=(aR.Count+3)/4,oJ=0;foreach(var eJ in aR){double ht=(b1-eJ.Value)/eJ.Key.fw;if(ht<0)continue;
if(ht>=1.0){if(c5==null)c5=new List<z>();c5.Add(eJ.Key);continue;}if((oJ++ +b0)%oC==0)mJ(eJ.Key,ht);}if(c5==null)return;foreach(var hR in c5){
aR.Remove(hR);d8.Remove(hR);bY.Remove(hR);}}static void cw(List<MySprite>pX,string qc,Color n3,float qt,float qv,float pQ){
var l0=MySprite.CreateText(qc,"Monospace",n3,pQ,TextAlignment.LEFT);l0.Position=new Vector2(qt,qv);pX.Add(l0);}static void it(List<MySprite>pY,List<string>ec,z e6,float qu,float qw,float pR,float o1){
string[]p2=e6.dR!=null&&e6.dR.Length>0 ? e6.dR : new[]{e6.iy};for(int dc=0;dc<ec.Count;
dc++){j gj=E.iN(p2,ec.Count>1 ? 100.0*dc/(ec.Count-1): 0.0);cw(pY,ec[dc],new Color(gj.aQ,gj.aO,gj.aK),qu,qw+dc*o1,pR);}}bool ni(z cn,IMyTextSurface fd){
List<string>c_;if(bY.ContainsKey(cn)|| !s.cz.TryGetValue(cn.bp,out c_))return false;bY[cn]="banner";Vector2 bF=fd.SurfaceSize;
Vector2 hn=(fd.TextureSize-bF)/2f;int el=1;foreach(var oS in c_)el=Math.Max(el,oS.Length);Vector2 c3=fd.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float e5=Math.Min(bF.X*0.94f/(el*c3.X),bF.Y*0.94f/(c_.Count*c3.Y));var h4=new List<MySprite>();h4.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=hn+bF/2f,Size=bF,Color=string.IsNullOrEmpty(cn.bo)? new Color(10,12,16): Q(cn.bo)}
);it(h4,c_,cn,hn.X+(bF.X-el*c3.X*e5)/2f,hn.Y+(bF.Y-c_.Count*c3.Y*e5)/2f,e5,c3.Y*e5);using(var os=fd.DrawFrame()){os.AddRange(h4);
}return true;}void mJ(z co,double lg){IMyTextSurface dG;if(!b_.TryGetValue(co,out dG))return;Vector2 bl=dG.SurfaceSize;
Vector2 ho=(dG.TextureSize-bl)/2f;var dn=co.mR();string k5=co.iz ?? "HELM TELEMETRY KERNEL ONLINE";bool hj=bl.Y>=200f;
List<string>b2;if(co.br==null|| !s.cz.TryGetValue(co.br,out b2))b2=mA;int b7=k5.Length+3;foreach(var oT in dn)b7=Math.Max(b7,oT.Length+5);
if(hj)foreach(var oU in b2)b7=Math.Max(b7,oU.Length);int ly=(hj ? b2.Count+1 : 0)+2+dn.Count;Vector2 b6=dG.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bi=Math.Min(1f,Math.Min(bl.X*0.94f/(b7*b6.X),bl.Y*0.94f/(ly*b6.Y)));float fr=ho.X+(bl.X-b7*b6.X*bi)/2f;float bI=ho.Y+(bl.Y-ly*b6.Y*bi)/2f;
float eM=b6.Y*bi;var cq=new List<MySprite>();cq.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=ho+bl/2f,Size=bl,Color=new Color(10,12,16)}
);if(hj){it(cq,b2,co,fr,bI,bi,eM);bI+=(b2.Count+1)*eM;}cw(cq,">> "+k5,new Color(34,197,94),fr,bI,bi);bI+=eM*2f;int pU=E.mz(lg,dn.Count);
for(int eC=0;eC<pU;eC++){bool ld=eC==dn.Count-1&&lg>=0.8;cw(cq,ld ? "[OK]" : "[..]",ld ? new Color(34,197,94): new Color(56,189,248),fr,bI,bi);cw(cq,dn[eC],new Color(203,213,225),fr+5f*b6.X*bi,bI,bi);
bI+=eM;}using(var ot=dG.DrawFrame()){ot.AddRange(cq);}}void fK(){bZ.Clear();b_.Clear();d6.Clear();
cY.Clear();d3=false;if(s==null)return;foreach(var aa in s.bq){var at=new List<IMyTerminalBlock>();if(aa.a_==aw.cI){
string g5= !string.IsNullOrEmpty(aa.aN)? aa.aN : aa.P;if(!an.ContainsKey(g5)){an[g5]=aa.dV;if(aa.fz!=null)d5[g5]=aa.fz;
}bZ[aa.P]=at;continue;}var l3=aa.ar.Count>0 ? aa.ar : new List<string>{aa.i8};foreach(var bH in l3){
if(string.IsNullOrEmpty(bH))continue;bool kM=bH.Contains("*")||bH.Contains("?");if(aa.a_==aw.fB||kM){var gP=new List<IMyTerminalBlock>();
mD(bH,gP);if(gP.Count>0){foreach(var kz in gP){if(!at.Contains(kz))at.Add(kz);}}else{var jq=dO();
for(int f9=0;f9<jq.Count;f9++){var fj=jq[f9];if(kM ? E.m_(bH,fj.CustomName): string.Equals(bH,fj.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!at.Contains(fj))at.Add(fj);}}}}else{var gd=iI(bH);if(gd!=null&& !at.Contains(gd))at.Add(gd);}}if(at.Count==0){
nt(aa,l3,at);if(at.Count>0)d6[aa.P]=true;}bZ[aa.P]=at;}foreach(var aU in s.R){
if(string.IsNullOrEmpty(aU.a2))continue;var hr=iI(aU.a2);if(hr==null){cY.AppendLine($"HELM: output block '{aU.a2}' not found");
continue;}IMyTextSurface bG=null;var hI=hr as IMyTextSurfaceProvider;var j5=hr as IMyTextSurface;if(hI!=null&&aU.bt<hI.SurfaceCount){
bG=hI.GetSurface(aU.bt);}else if(j5!=null&&aU.bt==0){bG=j5;}if(bG==null){cY.AppendLine($"HELM: output block '{aU.a2}' has no surface index {aU.bt}");
continue;}bG.ContentType=ContentType.SCRIPT;bG.Script="";bG.ScriptBackgroundColor=Color.Black;b_[aU]=bG;}a7.Clear();
}static readonly string[]iH={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool mP(int oK,IMyTerminalBlock p){switch(oK){case 0: return p is IMyBatteryBlock;case 1: return p is IMyReactor;case 2: return p is IMySolarPanel;
case 3: return p is IMyWindTurbine;case 4: return p is IMyGasTank;case 5: return p is IMyAirVent;case 6: return p is IMyCargoContainer;case 7: return p is IMyJumpDrive;
case 8: return p is IMyThrust;case 9: return p is IMyMotorSuspension;case 10: return p is IMyDoor;case 11: return p is IMyShipConnector;case 12: return p is IMyAssembler;
case 13: return p is IMyRefinery;case 14: return p is IMyShipDrill;case 15: return p is IMyShipWelder;case 16: return p is IMyShipGrinder;case 17: return p is IMyGasGenerator;
case 18: return p is IMyLargeTurretBase;case 19: return p is IMyProjector;case 20: return p is IMyGyro;case 21: return p is IMyGravityGeneratorBase;case 22: return p is IMyLandingGear;
case 23: return p is IMyRadioAntenna;case 24: return p is IMyBeacon;case 25: return p is IMyLightingBlock;case 26: return p is IMyPistonBase;case 27: return p is IMyMotorStator;
case 28: return p is IMyWarhead;case 29: return p is IMySensorBlock;case 30: return p is IMyCameraBlock;case 31: return p is IMyParachute;case 32: return p is IMyTurretControlBlock;
}return false;}void nt(aL nX,List<string>p9,List<IMyTerminalBlock>ge){string oF=(nX.P+" "+string.Join(" ",p9)).ToLowerInvariant();
var g8=new List<int>();for(int eI=0;eI<iH.Length;eI++){foreach(var qr in iH[eI].Split(','))if(oF.Contains(qr)){g8.Add(eI);break;}}if(g8.Count==0)return;
var jp=dO();var le=new List<IMyTerminalBlock>();for(int gU=0;gU<jp.Count;gU++){var ga=jp[gU];foreach(int oL in g8){if(mP(oL,ga)){(ga.CubeGrid==Me.CubeGrid ? ge : le).Add(ga);
break;}}}if(ge.Count==0)ge.AddRange(le);}void nv(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(s==null||s.R.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {s.R.Count} screens, {s.bq.Count} binds");d3=false;
Echo($"Blocks: {dO().Count} on this construct; {fR} on docked ships (ignored)");a7.Clear();Echo("--- BIND RESOLUTION ---");foreach(var ab in s.bq){
if(ab.a_==aw.cI){string g6= !string.IsNullOrEmpty(ab.aN)? ab.aN : ab.P;double qj=an.ContainsKey(g6)? an[g6]: ab.dV;
Echo($"[OK] @bind {ab.P} (dynamic variable '{g6}' = {qj})");continue;}List<IMyTerminalBlock>gf;if(bZ.TryGetValue(ab.P,out gf)){if(gf.Count>0){
string om=d6.ContainsKey(ab.P)&&d6[ab.P]? " [fallback: typed match]" : "";Echo($"[OK] @bind {ab.P} ({gf.Count} blocks matched{om})");
}else{string qa=string.Join(", ",ab.ar.Count>0 ? ab.ar : new List<string>{ab.i8});Echo($"[WARN] @bind {ab.P} -> 0 blocks matched \"{qa}\"");
}}else{Echo($"[ERR] @bind {ab.P} -> unresolved");}}if(an.Count>0){Echo($"--- DYNAMIC VARIABLES ({an.Count}) ---");foreach(var kU in an)Echo($"[VAR] {kU.Key} = {kU.Value}");
}Echo("--- SURFACE OUTPUT AUDIT ---");foreach(var aV in s.R){IMyTextSurface lX;if(b_.TryGetValue(aV,out lX)){var lO=lX.SurfaceSize;
var p0=E.mF(aV);Echo($"[OK] Screen \"{aV.a1}\" [{aV.bu}] -> {aV.a2} [#{aV.bt}] ({lO.X:0}x{lO.Y:0} px, {p0.S} sprites)");
}else{Echo($"[ERR] Screen \"{aV.a1}\" -> Missing block \"{aV.a2}\" [#{aV.bt}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double nu(IMyTerminalBlock q){
if(q==null)return 0.0;var ef=q as IMyBatteryBlock;if(ef!=null)return ef.MaxStoredPower>0 ?(ef.CurrentStoredPower/ef.MaxStoredPower)*100.0 : 0.0;
var eH=q as IMyJumpDrive;if(eH!=null)return eH.MaxStoredPower>0 ?(eH.CurrentStoredPower/eH.MaxStoredPower)*100.0 : 0.0;var l2=q as IMyGasTank;
if(l2!=null)return l2.FilledRatio*100.0;var mn=q as IMyAirVent;if(mn!=null)return mn.GetOxygenLevel()*100.0;var jO=q as IMyCargoContainer;if(jO!=null){
var eF=jO.GetInventory();if(eF!=null&&eF.MaxVolume.RawValue>0)return(double)eF.CurrentVolume.RawValue/eF.MaxVolume.RawValue*100.0;return 0.0;}var e_=q as IMyReactor;
if(e_!=null)return e_.MaxOutput>0 ?(e_.CurrentOutput/e_.MaxOutput)*100.0 : 0.0;var e7=q as IMySolarPanel;if(e7!=null)return e7.MaxOutput>0 ?(e7.CurrentOutput/e7.MaxOutput)*100.0 : 0.0;
var fq=q as IMyWindTurbine;if(fq!=null)return fq.MaxOutput>0 ?(fq.CurrentOutput/fq.MaxOutput)*100.0 : 0.0;var j6=q as IMyDoor;if(j6!=null)return(double)j6.OpenRatio*100.0;
var fm=q as IMyThrust;if(fm!=null)return fm.MaxEffectiveThrust>0 ?(fm.CurrentThrust/fm.MaxEffectiveThrust)*100.0 : 0.0;var id=q as IMyMotorSuspension;
if(id!=null)return id.Brake ? 100.0 :(id.Friction*100.0);var gs=q as IMyShipConnector;if(gs!=null)return gs.Status==MyShipConnectorStatus.Connected ? 100.0 :(gs.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var lt=q as IMyProductionBlock;if(lt!=null)return lt.IsProducing ? 100.0 : 0.0;var ka=q as IMyShipDrill;if(ka!=null)return ka.Enabled ? 100.0 : 0.0;
var mb=q as IMyShipToolBase;if(mb!=null)return mb.Enabled ? 100.0 : 0.0;var gM=q as IMyGasGenerator;if(gM!=null)return gM.AutoRefill ? 100.0 :(gM.Enabled ? 50.0 : 0.0);
var fn=q as IMyLargeTurretBase;if(fn!=null)return fn.IsShooting ? 100.0 :(fn.HasTarget ? 50.0 :(fn.Enabled ? 10.0 : 0.0));var dz=q as IMyProjector;
if(dz!=null)return dz.TotalBlocks>0 ?((double)(dz.TotalBlocks-dz.RemainingBlocks)/dz.TotalBlocks)*100.0 : 0.0;var gS=q as IMyGyro;if(gS!=null)return gS.GyroOverride ? 100.0 :(gS.Enabled ? 50.0 : 0.0);
var kB=q as IMyGravityGeneratorBase;if(kB!=null)return Math.Min(100.0,Math.Abs(kB.GravityAcceleration)/9.81*100.0);var gL=q as IMyLandingGear;if(gL!=null)return gL.IsLocked ? 100.0 :(gL.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var jr=q as IMyRadioAntenna;if(jr!=null)return(jr.Radius/50000.0)*100.0;var jD=q as IMyBeacon;if(jD!=null)return(jD.Radius/50000.0)*100.0;
var k0=q as IMyLightingBlock;if(k0!=null)return k0.Intensity*10.0;var eY=q as IMyPistonBase;if(eY!=null)return eY.HighestPosition>0 ?(eY.CurrentPosition/eY.HighestPosition)*100.0 : 0.0;
var lw=q as IMyMotorStator;if(lw!=null)return Math.Min(100.0,Math.Abs(lw.Angle)/6.28318*100.0);var mp=q as IMyWarhead;if(mp!=null)return mp.IsArmed ? 100.0 : 0.0;
var h0=q as IMySensorBlock;if(h0!=null)return!h0.LastDetectedEntity.IsEmpty()? 100.0 :(h0.Enabled ? 25.0 : 0.0);var gm=q as IMyCameraBlock;
if(gm!=null)return gm.IsActive ? 100.0 :(gm.Enabled ? 50.0 : 0.0);var jU=q as IMyParachute;if(jU!=null)return(double)jU.OpenRatio*100.0;
var fl=q as IMyTurretControlBlock;if(fl!=null)return fl.HasTarget ? 100.0 :(fl.IsUnderControl ? 50.0 :(fl.Enabled ? 10.0 : 0.0));var kq=q as IMyFunctionalBlock;
if(kq!=null)return kq.Enabled ? 100.0 : 0.0;return 0.0;}static double mX(IMyTerminalBlock jy,string gE){int jM=gE.IndexOf(':');string g7=gE.Substring(0,jM).ToLowerInvariant();
var qh=new MyItemType("MyObjectBuilder_"+(g7=="ammo" ? "AmmoMagazine" : char.ToUpper(g7[0])+g7.Substring(1)),gE.Substring(jM+1));double lW=0;for(int gV=0;
gV<jy.InventoryCount;gV++)lW+=(double)jy.GetInventory(gV).GetItemAmount(qh);return lW;}double i1(IMyTerminalBlock r,string ki){if(r==null)return 0.0;
string a=ki.ToLowerInvariant();if(a=="percent"||a=="pct")return nu(r);if(a.IndexOf(':')>0)return mX(r,ki);var ba=r as IMyBatteryBlock;
if(ba!=null){if(a=="current"||a=="stored"||a=="val"||a=="value"||a=="raw")return ba.CurrentStoredPower;if(a=="max"||a=="capacity")return ba.MaxStoredPower;
if(a=="output")return ba.CurrentOutput;if(a=="maxoutput")return ba.MaxOutput;if(a=="input")return ba.CurrentInput;if(a=="maxinput")return ba.MaxInput;
return ba.CurrentStoredPower;}var g4=r as IMyJumpDrive;if(g4!=null){if(a=="max"||a=="capacity")return g4.MaxStoredPower;return g4.CurrentStoredPower;
}var fg=r as IMyGasTank;if(fg!=null){if(a=="max"||a=="capacity")return fg.Capacity;return fg.FilledRatio*fg.Capacity;}var mo=r as IMyAirVent;
if(mo!=null){if(a=="max"||a=="capacity")return 100.0;return mo.GetOxygenLevel()*100.0;}var jP=r as IMyCargoContainer;if(jP!=null){var eG=jP.GetInventory();
if(eG!=null){if(a=="mass")return(double)eG.CurrentMass;if(a=="max"||a=="capacity")return(double)eG.MaxVolume;return(double)eG.CurrentVolume;}return 0.0;}var hJ=r as IMyPowerProducer;
if(hJ!=null){if(a=="max"||a=="capacity")return hJ.MaxOutput;return hJ.CurrentOutput;}var j7=r as IMyDoor;if(j7!=null){if(a=="max")return 1.0;return(double)j7.OpenRatio;
}var ii=r as IMyThrust;if(ii!=null){if(a=="max"||a=="capacity")return ii.MaxEffectiveThrust;return ii.CurrentThrust;}var ie=r as IMyMotorSuspension;
if(ie!=null){if(a=="brake")return ie.Brake ? 1.0 : 0.0;return ie.Friction*100.0;}var j_=r as IMyShipConnector;if(j_!=null){return j_.Status==MyShipConnectorStatus.Connected ? 1.0 : 0.0;
}var hG=r as IMyProductionBlock;if(hG!=null){if(a=="producing"||a=="active"||a=="running"||a=="state")return hG.IsProducing ? 1.0 : 0.0;if(a=="queue")return hG.IsQueueEmpty ? 0.0 : 1.0;
}var f5=r as IMyAssembler;if(f5!=null){if(a=="disassembling")return f5.Mode==MyAssemblerMode.Disassembly ? 1.0 : 0.0;if(a=="repeating"||a=="loop")return f5.Repeating ? 1.0 : 0.0;
}var gN=r as IMyGasGenerator;if(gN!=null){if(a=="autorefill")return gN.AutoRefill ? 1.0 : 0.0;if(a=="active")return gN.Enabled ? 1.0 : 0.0;}var fo=r as IMyLargeTurretBase;
if(fo!=null){if(a=="shooting")return fo.IsShooting ? 1.0 : 0.0;if(a=="target"||a=="hastarget")return fo.HasTarget ? 1.0 : 0.0;if(a=="range")return fo.Range;
}var cl=r as IMyProjector;if(cl!=null){if(a=="built")return cl.TotalBlocks-cl.RemainingBlocks;if(a=="remaining")return cl.RemainingBlocks;if(a=="total")return cl.TotalBlocks;
if(a=="projecting")return cl.IsProjecting ? 1.0 : 0.0;}var cb=r as IMyGyro;if(cb!=null){if(a=="override")return cb.GyroOverride ? 1.0 : 0.0;if(a=="pitch")return cb.Pitch;
if(a=="yaw")return cb.Yaw;if(a=="roll")return cb.Roll;if(a=="power")return cb.GyroPower*100.0;}var kC=r as IMyGravityGeneratorBase;if(kC!=null){if(a=="g"||a=="accel")return kC.GravityAcceleration;
}var kA=r as IMyLandingGear;if(kA!=null){if(a=="locked")return kA.IsLocked ? 1.0 : 0.0;}var js=r as IMyRadioAntenna;if(js!=null){if(a=="radius"||a=="range")return js.Radius;
}var jE=r as IMyBeacon;if(jE!=null){if(a=="radius"||a=="range")return jE.Radius;}var hg=r as IMyLightingBlock;if(hg!=null){if(a=="intensity")return hg.Intensity;
if(a=="radius")return hg.Radius;}var eZ=r as IMyPistonBase;if(eZ!=null){if(a=="pos"||a=="position"||a=="extension")return eZ.CurrentPosition;
if(a=="velocity")return eZ.Velocity;if(a=="max")return eZ.HighestPosition;}var dE=r as IMyMotorStator;if(dE!=null){if(a=="angle"||a=="deg")return dE.Angle*180.0/Math.PI;
if(a=="rad")return dE.Angle;if(a=="velocity"||a=="rpm")return dE.TargetVelocityRPM;if(a=="locked")return dE.RotorLock ? 1.0 : 0.0;}var ir=r as IMyWarhead;
if(ir!=null){if(a=="armed")return ir.IsArmed ? 1.0 : 0.0;if(a=="countdown")return ir.DetonationTime;}var lL=r as IMySensorBlock;if(lL!=null){
if(a=="detected")return!lL.LastDetectedEntity.IsEmpty()? 1.0 : 0.0;}var gn=r as IMyCameraBlock;if(gn!=null){if(a=="range")return gn.AvailableScanRange;
if(a=="active")return gn.IsActive ? 1.0 : 0.0;}var jV=r as IMyParachute;if(jV!=null){if(a=="open"||a=="openratio")return(double)jV.OpenRatio;}var ct=r as IMyTurretControlBlock;
if(ct!=null){if(a=="target"||a=="hastarget")return ct.HasTarget ? 1.0 : 0.0;if(a=="azimuth")return ct.AzimuthRotor!=null ? ct.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(a=="elevation")return ct.ElevationRotor!=null ? ct.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var kr=r as IMyFunctionalBlock;if(kr!=null){return kr.Enabled ? 1.0 : 0.0;
}return 0.0;}double fL(string c0,string kj){aL bb;cV.TryGetValue(c0,out bb);if(bb!=null&&bb.a_==aw.cI){
string oM= !string.IsNullOrEmpty(bb.aN)? bb.aN : c0;double j0;if(an.TryGetValue(oM,out j0))return j0;return bb.dV;
}double kc;if(an.TryGetValue(c0,out kc))return kc;List<IMyTerminalBlock>c1;if(!bZ.TryGetValue(c0,out c1)||c1.Count==0)throw new InvalidOperationException($"HELM: bind '{c0}' resolved to zero blocks — check the name/typo");
if(c1.Count==1)return i1(c1[0],kj);var mm=new List<double>();foreach(var nN in c1)mm.Add(i1(nN,kj));return mv.mu(bb!=null ? bb.cx : al.fu,mm);
}double fM(string nY){return fL(nY,"percent");}static string aZ(double mj,bs ch){if(ch==null||(string.IsNullOrEmpty(ch.cU)&&string.IsNullOrEmpty(ch.cJ)&&ch.I=="percent")){
return iM(mj);}return E.mQ(mj,ch.cU,ch.cJ);}bool no(z cp){IMyTextSurface fe;if(!b_.TryGetValue(cp,out fe))return false;
if(cp.bp!=null)return ni(cp,fe);if(cp.cD.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return np(cp,fe);
}return nq(cp,fe);}static void v(a4 n2,Vector2 pr,Vector2 pV,Color n4,float pH=0f,string od="SquareSimple"){n2.k.Add(new MySprite{
Type=SpriteType.TEXTURE,Data=od,Position=pr,Size=pV,Color=n4,RotationOrScale=pH});}void mC(z o,IMyTextSurface lY,a4 f){
f.k.Clear();f.d0.Clear();Vector2 L=lY.SurfaceSize;Vector2 qe=lY.TextureSize;Vector2 aW=(qe-L)/2f;
float eR=Math.Max(4f,L.X*0.02f);float dw=Math.Max(4f,L.Y*0.02f);if(o.bu=="compact"||(o.bu=="auto"&&L.Y<=260f&&L.Y>150f)){
eR=4f;dw=4f;}else if(o.bu=="corner"||(o.bu=="auto"&&L.Y<=150f)){eR=2f;dw=2f;}float md=L.X-(eR*2f);
Color gc=new Color(10,12,16);bool j9=true;if(!string.IsNullOrEmpty(o.bo)){var db=j.bU(o.bo);if(db.cM)j9=false;
else gc=new Color(db.aQ,db.aO,db.aK,db.dN);}f.bN= -1;f.ft=false;f.fs=Q(o.iv ?? "#5b0f0f");if(o.dQ!=null&&o.dQ.Length>1){
int ee=16;float jA=(L.Y-4f)/ee;f.bN=f.k.Count;f.bM=new Color[ee];for(int cc=0;cc<ee;cc++){j gK=E.iN(o.dQ,100.0*cc/(ee-1));
f.bM[cc]=new Color(gK.aQ,gK.aO,gK.aK);v(f,new Vector2(aW.X+L.X/2f,aW.Y+2f+jA*(cc+0.5f)),new Vector2(L.X-4f,jA+1f),f.bM[cc]);
}}else if(j9){f.bN=f.k.Count;f.bM=new[]{gc};v(f,aW+(L/2f),L-new Vector2(4f,4f),gc);}if(!o.cD.Equals("none",StringComparison.OrdinalIgnoreCase)){
Color jJ=o.aM!=null&&o.aM.Length>0 ? Q(o.aM[0]): Q(o.dT ?? "40, 60, 85");
Color n1=o.aM!=null&&o.aM.Length>1 ? Q(o.aM[o.aM.Length-1]): jJ;
v(f,new Vector2(aW.X+L.X/2f,aW.Y+dw*0.5f),new Vector2(md,2f),jJ);v(f,new Vector2(aW.X+L.X/2f,aW.Y+L.Y-(dw*0.5f)),new Vector2(md,2f),n1);
}var hw=o.az;if(hw.Count==0){hw=new List<am>{new am{cP="main",cF=0,cQ=0,cR=o.bS,cS=o.bT,aP=o.fE}
};}float kE=8f;foreach(var O in hw){var eQ=E.mE(O.cF,O.cQ,O.cR,O.cS,o.bS,o.bT,L.X,L.Y,eR,dw,kE,kE);
float aC=aW.X+eQ.jg;float dv=aW.Y+eQ.jh;float aD=eQ.jf;float du=eQ.iP;if(!string.IsNullOrEmpty(O.cy)){
var dt=j.bU(O.cy);if(!dt.cM)v(f,new Vector2(aC+aD/2f,dv+du/2f),new Vector2(aD,du),new Color(dt.aQ,dt.aO,dt.aK,dt.dN));
}else if(o.bS>1||o.bT>1){Color jN=string.IsNullOrEmpty(o.fH)? new Color(255,255,255,6): Q(o.fH);if(jN.A>0)v(f,new Vector2(aC+aD/2f,dv+du/2f),new Vector2(aD,du),jN);
}string gJ=O.iJ ?? o.iK ?? "Monospace";float nW=E.mS(o.bu,L.X,L.Y,o.bQ);float au=O.iL ?? nW;
float ey=0f;if(!string.IsNullOrEmpty(O.bw)){ey=20f*au;var f0=TextAlignment.LEFT;float ij=aC+4f;string jm=O.fN ?? o.ja ?? "left";
if(jm.Equals("center",StringComparison.OrdinalIgnoreCase)){f0=TextAlignment.CENTER;ij=aC+(aD/2f);}else if(jm.Equals("right",StringComparison.OrdinalIgnoreCase)){
f0=TextAlignment.RIGHT;ij=aC+aD-4f;}string l5=O.fO ?? o.jb;Color qf= !string.IsNullOrEmpty(l5)? Q(l5): Q(O.fx ?? o.dT ?? "white");
var ma=MySprite.CreateText(O.bw,gJ,qf,au*0.9f,f0);ma.Position=new Vector2(ij,dv+2f);f.k.Add(ma);
string[]gb=O.fy ?? o.aM;Color of=gb!=null&&gb.Length>0 ? Q(gb[0]): Q(O.fx ?? o.dT ?? "40, 60, 85");
v(f,new Vector2(aC+aD/2f,dv+ey-2f),new Vector2(aD-8f,1f),of);}int o6=Math.Max(1,O.aP.Count);float pi=du-ey-8f;
float hN=pi/o6;float eE=6f;for(int eD=0;eD<O.aP.Count;eD++){var w=O.aP[eD];var K=new cO{iV=w};float pI=dv+ey+4f+(eD*hN);
float D=pI+(hN/2f);float l7=D-(11f*au);K.fC=f.k.Count;K.iO=w.N!=null&&(w.N.Contains("{val}")||w.N.Contains("{percent}"));
var kX=MySprite.CreateText(w.N,gJ,Color.White,au,TextAlignment.LEFT);kX.Position=new Vector2(aC+eE,l7);f.k.Add(kX);
float o_=Math.Max(24f*au,(w.N.Length+1)*12f*au);float qq=(!string.IsNullOrEmpty(w.cU)|| !string.IsNullOrEmpty(w.cJ))? 64f*au : 52f*au;
if(w.g!=d.i9){float C=aC+eE+o_;float nV=aC+aD-eE-qq;float B=Math.Max(16f,nV-C);
float J=Math.Max(w.g==d.bv ? 8f : 4f,Math.Min((w.g==d.bv ? 20f : 16f)*au,hN*(w.g==d.bv ? 0.55f : 0.45f)));
K.cC=C;K.dP=B;K.bL=J;K.cB=D;if(w.g==d.cA){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(25,30,38));
K.U=f.k.Count;v(f,new Vector2(C,D),new Vector2(0f,J),Q(w.ah));}else if(w.g==d.cN){
int hZ=10;float kt=2f;float h_=Math.Max(2f,(B-(kt*(hZ-1)))/hZ);for(int hS=0;hS<hZ;hS++){K.W.Add(f.k.Count);
v(f,new Vector2(C+hS*(h_+kt)+(h_/2f),D),new Vector2(h_,J),new Color(22,26,32));}}else if(w.g==d.bv){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));
int hF=8;float ku=2f;float gq=Math.Max(2f,(B-(ku*(hF-1)))/hF);for(int hu=0;hu<hF;hu++){K.W.Add(f.k.Count);
v(f,new Vector2(C+hu*(gq+ku)+(gq/2f),D),new Vector2(gq,2f),Q(w.ah));}}else if(w.g==d.cL){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));
int h7=10;float kv=2f;float h8=Math.Max(2f,(B-(kv*(h7-1)))/h7);for(int hT=0;hT<h7;hT++){K.W.Add(f.k.Count);
v(f,new Vector2(C+hT*(h8+kv)+(h8/2f),D),new Vector2(h8,J),new Color(24,28,36),0.523599f);}}else if(w.g==d.cE){
int f3=10;float kw=2f;float f4=Math.Max(2f,(B-(kw*(f3-1)))/f3);for(int hU=0;hU<f3;hU++){K.W.Add(f.k.Count);
v(f,new Vector2(C+hU*(f4+kw)+(f4/2f),D),new Vector2(f4,J),new Color(24,28,36),1.570796f,"Triangle");}}else if(w.g==d.cH){
int gv=12;float kx=2f;float gx=Math.Max(2f,(B-(kx*(gv-1)))/gv);float j8=Math.Min(gx,J);for(int hV=0;hV<gv;hV++){K.W.Add(f.k.Count);
v(f,new Vector2(C+hV*(gx+kx)+(gx/2f),D),new Vector2(j8,j8),new Color(24,28,36),0f,"Circle");}}else if(w.g==d.bK){
v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(25,30,38));K.U=f.k.Count;v(f,new Vector2(C+(B/2f),D),new Vector2(0f,J),Q(w.ah));
v(f,new Vector2(C+(B/2f),D),new Vector2(2f,J+4f),new Color(80,110,150));}else if(w.g==d.bW){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));
K.U=f.k.Count;float g9=Math.Max(4f,J);v(f,new Vector2(C+g9*0.5f,D),new Vector2(g9,g9),new Color(40,50,65),0f,"Circle");
}else if(w.g==d.bO){float b3=Math.Max(6f,Math.Min(J,14f*au));v(f,new Vector2(C+b3*0.5f,D),new Vector2(b3,b3),new Color(28,34,46));
K.U=f.k.Count;v(f,new Vector2(C+b3*0.5f,D),new Vector2(b3*0.6f,b3*0.6f),new Color(40,50,65));}else if(w.g==d.bV){
v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(18,22,30));K.U=f.k.Count;float hD=Math.Max(4f,J*0.7f);
v(f,new Vector2(C+hD+2f,D),new Vector2(hD,hD),new Color(34,197,94),0f,"Circle");}else if(w.g==d.bJ){int hC=8;
float ky=2f;float hE=Math.Max(2f,(B-(ky*(hC-1)))/hC);for(int hv=0;hv<hC;hv++){K.W.Add(f.k.Count);v(f,new Vector2(C+hv*(hE+ky)+(hE/2f),D),new Vector2(hE,J),new Color(24,28,36));
}}else if(w.g==d.bP){v(f,new Vector2(C+(B/2f),D),new Vector2(B,J),new Color(20,24,30));K.U=f.k.Count;
float kJ=Math.Max(4f,J);v(f,new Vector2(C+(B/2f),D),new Vector2(kJ,kJ),Q(w.ah),0f,"Triangle");}}var mh=MySprite.CreateText("---",gJ,Color.White,au,TextAlignment.RIGHT);
mh.Position=new Vector2(aC+aD-eE,l7);K.a6=f.k.Count;f.k.Add(mh);f.d0.Add(K);}}f.iT=L;
f.iS=true;}static void aA(a4 jK,cO lP,string qd,Color n5){var io=jK.k[lP.a6];io.Data=qd;io.Color=n5;jK.k[lP.a6]=io;
}static void i4(a4 jL,cO lQ,Color n6){var kg=jL.k[lQ.U];kg.Color=n6;jL.k[lQ.U]=kg;}bool nq(z hY,IMyTextSurface ia){
a4 i;if(!d8.TryGetValue(hY,out i)){i=new a4();d8[hY]=i;}if(!i.iS||i.iT!=ia.SurfaceSize){
mC(hY,ia,i);}bool G=false;bool f_=(b0%16)<8;bool iq=bx&&f_&&i.fs.A>0;if(i.bN>=0&&iq!=i.ft){
i.ft=iq;for(int dd=0;dd<i.bM.Length;dd++){var jG=i.k[i.bN+dd];jG.Color=iq ? i.fs : i.bM[dd];i.k[i.bN+dd]=jG;
}G=true;}for(int gW=0;gW<i.d0.Count;gW++){var b=i.d0[gW];var m=b.iV;double y;bool kG=false;try{y=fL(m.aX,m.I);
}catch{kG=true;y=0.0;}if(kG){if(b.dZ!= -99999.0||(b.a6>=0&&i.k[b.a6].Data!="ERR")){G=true;b.dZ= -99999.0;
if(b.a6>=0){var hW=i.k[b.a6];hW.Data="ERR";hW.Color=Color.Red;i.k[b.a6]=hW;}}continue;}double aE=y;
if(m.a0>0)y=Math.Min(100.0,aE/m.a0*100.0);string n8=m.cK ? E.dX(m.bR,y): m.ah;Color V=Q(n8);
bool aS=(V==Color.Red);if(aS&&f_){V=new Color(255,75,75);G=true;}bool av=Math.Abs(aE-b.dZ)>0.1;if(av)b.dZ=aE;
if(m.g==d.cA){double kk=(m.a0>0||m.I=="percent"||m.I=="pct")? y : fM(m.aX);if(b.M<0f)b.M=(float)kk;
float fh=(float)kk;if(Math.Abs(b.M-fh)>0.05f){b.M+=(fh-b.M)*0.35f;G=true;}else if(b.M!=fh){
b.M=fh;G=true;}if(av)G=true;if(G){float pk=(float)Math.Max(0.0,Math.Min(1.0,b.M/100.0));float km=b.dP*pk;
var es=i.k[b.U];es.Size=new Vector2(km,b.bL);es.Position=new Vector2(b.cC+(km/2f),b.cB);
es.Color=V;i.k[b.U]=es;aA(i,b,aZ(aE,m),V);}}else if(m.g==d.cN||m.g==d.cL||m.g==d.cE||m.g==d.cH){
if(av||aS){G=true;double oo=(m.a0>0||m.I=="percent"||m.I=="pct")? y : fM(m.aX);int lJ=b.W.Count;
for(int dF=0;dF<lJ;dF++){bool o8=oo>=((dF+0.4)/(double)lJ*100.0);Color pS=o8 ? V : new Color(24,28,36);var lK=i.k[b.W[dF]];
lK.Color=pS;i.k[b.W[dF]]=lK;}aA(i,b,aZ(aE,m),V);}}else if(m.g==d.bv){
if(av||aS){G=true;for(int ex=0;ex<7;ex++)b.d1[ex]=b.d1[ex+1];b.d1[7]=(float)y;int pq=b.W.Count;
for(int dq=0;dq<pq;dq++){float oE=b.d1[dq];float oD=Math.Max(0.06f,Math.Min(1f,oE/100f));float jX=b.bL*oD;float n7=b.cB+(b.bL/2f)-(jX/2f);
var b8=i.k[b.W[dq]];b8.Size=new Vector2(b8.Size.Value.X,jX);b8.Position=new Vector2(b8.Position.Value.X,n7);
b8.Color=V;i.k[b.W[dq]]=b8;}aA(i,b,aZ(aE,m),V);}}else if(m.g==d.bK){
if(b.M<0f)b.M=(float)y;float fi=(float)y;if(Math.Abs(b.M-fi)>0.05f){b.M+=(fi-b.M)*0.35f;
G=true;}else if(b.M!=fi){b.M=fi;G=true;}if(G||av){float kF=b.dP*0.5f;float jS=b.cC+kF;
float j4=(float)Math.Max(-1.0,Math.Min(1.0,(b.M-50.0)/50.0));float gH=Math.Abs(j4)*kF;float op=(j4>=0)?(jS+gH*0.5f):(jS-gH*0.5f);
var et=i.k[b.U];et.Size=new Vector2(gH,b.bL);et.Position=new Vector2(op,b.cB);et.Color=V;
i.k[b.U]=et;aA(i,b,aZ(aE,m),V);}}else if(m.g==d.bW){if(av||aS){
G=true;bool g3=y>=50.0;float kR=Math.Max(4f,b.bL);float oP=g3 ?(b.cC+b.dP-kR*0.5f):(b.cC+kR*0.5f);
Color kQ=g3 ?(V!=Color.White ? V : new Color(34,197,94)): new Color(60,70,85);var gF=i.k[b.U];gF.Position=new Vector2(oP,b.cB);
gF.Color=kQ;i.k[b.U]=gF;aA(i,b,g3 ? "ON" : "OFF",kQ);}}else if(m.g==d.bO){if(av||aS){
G=true;bool g2=y>=50.0;Color jT=g2 ?(V!=Color.White ? V : new Color(56,189,248)): new Color(30,36,48);i4(i,b,jT);
aA(i,b,g2 ? "ACTIVE" : "READY",g2 ? jT : new Color(148,163,184));}}else if(m.g==d.bV){if(av||aS){
G=true;string p1=y>=75.0 ? "ONLINE" :(y>=25.0 ? "STBY" : "OFFLINE");Color lU=y>=75.0 ? new Color(34,197,94):(y>=25.0 ? new Color(234,179,8): new Color(239,68,68));
i4(i,b,lU);aA(i,b,p1,lU);}}else if(m.g==d.bJ){if(av||aS){G=true;int lp=b.W.Count;
for(int dr=0;dr<lp;dr++){bool o9=y>=((dr+0.5)/(double)lp*100.0);Color po=o9 ? V : new Color(24,28,36);var lq=i.k[b.W[dr]];
lq.Color=po;i.k[b.W[dr]]=lq;}aA(i,b,aZ(aE,m),V);}}else if(m.g==d.bP){
G=true;float e2;Color c9;string eu;bool oH=(m.I=="producing"||m.I=="active"||m.I=="running"||m.I=="state"||(y<=1.0&&m.I!="percent"));
if(oH){if(y>0.0){b.M=(b.M+0.3927f)%6.28318f;e2=b.M;c9=new Color(34,197,94);eu="RUN";}else{e2=0f;
c9=new Color(148,163,184);eu="IDLE";}}else{e2=(y>55.0)? 0f :((y<45.0)? 3.14159f : 1.57079f);c9=(y>55.0)? new Color(34,197,94):((y<45.0)? new Color(239,68,68): new Color(148,163,184));
eu=(y>55.0)? "CHG" :((y<45.0)? "DSC" : "STBY");}var gG=i.k[b.U];gG.RotationOrScale=e2;gG.Color=c9;
i.k[b.U]=gG;aA(i,b,eu,c9);}else{string eq;aL fk;cV.TryGetValue(m.aX,out fk);d5.TryGetValue(fk!=null&&fk.aN!=null ? fk.aN : m.aX,out eq);
if(av||aS||eq!=b.iU){b.iU=eq;G=true;Color ik=m.aY!=null ? Q(E.i2(m.aY,y)): Color.White;
if(ik==Color.Red&&f_)ik=new Color(255,75,75);aA(i,b,eq ?? aZ(aE,m),ik);}}if(b.iO&&(av||aS)){
G=true;var kZ=i.k[b.fC];string pl=iM(y);string pE=aZ(aE,m);kZ.Data=m.N.Replace("{percent}",pl).Replace("{val}",pE);
i.k[b.fC]=kZ;}}if(G){using(var ou=ia.DrawFrame()){ou.AddRange(i.k);}}return G;}bool np(z bj,IMyTextSurface ib){
ib.FontSize=bj.bQ;var em=new List<KeyValuePair<string,string>>();foreach(var l in bj.fE){double F=0.0;string aI;string aB=null;
try{F=fL(l.aX,l.I);}catch(Exception qx){em.Add(new KeyValuePair<string,string>($"{l.N}: [UNRESOLVED]","red"));
continue;}double lu=F;if(l.a0>0)F=Math.Min(100.0,lu/l.a0*100.0);string mi=aZ(lu,l);if(l.g==d.cA){double kl=(l.a0>0||l.I=="percent"||l.I=="pct")? F : fM(l.aX);
aI=E.nj(l.N,kl,10,mi);aB=l.cK ? E.dX(l.bR,kl): l.ah;}else if(l.g==d.bK){
aI=E.nh(l.N,F,10);aB=l.cK ? E.dX(l.bR,F): l.ah;}else if(l.g==d.bW){
aI=E.ns(l.N,F);aB=l.ah ??(F>=50.0 ? "green" : "gray");}else if(l.g==d.bO){aI=E.nm(l.N,F);
aB=l.ah ??(F>=50.0 ? "cyan" : "gray");}else if(l.g==d.bV){aI=E.nr(l.N,F);aB=l.ah ??(F>=75.0 ? "lime" :(F>=25.0 ? "yellow" : "red"));
}else if(l.g==d.bJ){aI=E.ng(l.N,F,8);aB=l.cK ? E.dX(l.bR,F):(l.ah ?? "yellow");
}else if(l.g==d.bP){bool oI=(l.I=="producing"||l.I=="active"||l.I=="running"||l.I=="state"||(F<=1.0&&l.I!="percent"));
if(oI){char[]p_=new[]{'|','/','-','\\'};char pP=p_[Math.Abs(b0)%4];aI=F>0 ? $"{l.N} [{pP}] RUN" : $"{l.N} [==] IDLE";
aB=F>0 ? "green" : "gray";}else{aI=E.nn(l.N,F,b0);aB=l.ah ??(F>55.0 ? "green" :(F<45.0 ? "red" : "cyan"));
}}else{aI=$"{l.N}: {mi}";aB=l.aY!=null ? E.i2(l.aY,F): null;}em.Add(new KeyValuePair<string,string>(aI,aB));
}var ej=E.nl(new List<string>(em.ConvertAll(p8=>p8.Key)),cv: bj.je);string lM=string.Join("\n",ej);
string ls;if(bY.TryGetValue(bj,out ls)&&ls==lM)return false;bY[bj]=lM;using(var ov=ib.DrawFrame()){
float ms=4f;float k1=mt*bj.bQ;try{var k3=ib.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bj.bQ);
if(k3.Y>0)k1=k3.Y;}catch{}for(int cd=0;cd<ej.Count;cd++){string jZ=null;if(cd>0&&cd<ej.Count-1)jZ=em[cd-1].Value;
var lT=MySprite.CreateText(ej[cd],"Monospace",Q(jZ),bj.bQ,TextAlignment.LEFT);lT.Position=new Vector2(4f,ms);ov.Add(lT);
ms+=k1;}}return true;}Color Q(string eO){if(string.IsNullOrEmpty(eO))return Color.White;Color dk;if(ji.TryGetValue(eO,out dk))return dk;
j c2=j.bU(eO);dk=c2.cM ? Color.Transparent : new Color(c2.aQ,c2.aO,c2.aK,c2.dN);ji[eO]=dk;return dk;}public enum aw{mx,fB,cI}
public enum al{m0,fu,i7,iX,iW}public class aL{public string P;public aw a_;public List<string>ar=new List<string>();public string i8{
get{return ar.Count>0 ? ar[0]: "";}set{if(ar.Count==0)ar.Add(value);else ar[0]=value;}}public al cx=al.m0;
public double dV=0.0;public string fz;public string aN;}public enum d{i9,cA,cN,bv,cL,cE,cH,bK,bW,bO,bV,bJ,bP}
public class cG{public string iG;public string dU;public bool dY;}public class bs{public d g;public string N;public string aX;
public string I;public bool cK;public string[]bR;public string ah;public List<cG>aY;public string iD="right";
public string cU;public string cJ;public double a0;}public struct j{public byte aQ,aO,aK,dN;public bool cM;public j(byte pw,byte ox,byte nO,byte nB=255){
aQ=pw;aO=ox;aK=nO;dN=nB;cM=false;}public static readonly j nz=new j(0,0,0,0){cM=true};public static readonly j d2=new j(255,255,255);
public static readonly j mw=new j(0,0,0);public static readonly j nd=new j(255,0,0);public static readonly j mU=new j(0,255,0);
public static readonly j mY=new j(50,205,50);public static readonly j nA=new j(255,255,0);public static readonly j m1=new j(255,140,0);
public static readonly j mH=new j(0,255,255);public static readonly j my=new j(0,120,255);public static readonly j mZ=new j(255,0,255);
public static readonly j nb=new j(128,0,128);public static readonly j mT=new j(128,128,128);public static readonly j mI=new j(60,60,60);
public static j bU(string aH){if(string.IsNullOrWhiteSpace(aH))return d2;aH=aH.Trim();if(aH.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aH.Equals("none",StringComparison.OrdinalIgnoreCase))return nz;
if(aH.StartsWith("#")){string be=aH.Substring(1).Trim();if(be.Length==6){byte px=Convert.ToByte(be.Substring(0,2),16);byte oy=Convert.ToByte(be.Substring(2,2),16);
byte nP=Convert.ToByte(be.Substring(4,2),16);return new j(px,oy,nP);}if(be.Length==3){byte py=Convert.ToByte(new string(be[0],2),16);byte oz=Convert.ToByte(new string(be[1],2),16);
byte nQ=Convert.ToByte(new string(be[2],2),16);return new j(py,oz,nQ);}}if(aH.Contains(",")){var ck=aH.Split(',');if(ck.Length>=3){byte pz=(byte)Math.Max(0,Math.Min(255,int.Parse(ck[0].Trim())));
byte oA=(byte)Math.Max(0,Math.Min(255,int.Parse(ck[1].Trim())));byte nR=(byte)Math.Max(0,Math.Min(255,int.Parse(ck[2].Trim())));byte nC=ck.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(ck[3].Trim()))):(byte)255;
return new j(pz,oA,nR,nC);}}switch(aH.ToLowerInvariant()){case "red": return nd;case "green": return mU;case "lime": return mY;case "yellow": return nA;
case "orange": return m1;case "cyan": return mH;case "blue": return my;case "magenta": return mZ;case "purple": return nb;case "white": return d2;
case "black": return mw;case "gray": case "grey": return mT;case "darkgray": case "darkgrey": return mI;default: return d2;}}}public class am{
public string cP="default";public int cF=0;public int cQ=0;public int cR=1;public int cS=1;public string bw;public string fN="left";public string fO;
public string iB;public string iJ;public float? iL;public string cy;public string fx;public string[]fy;public string iY="horizontal";
public List<bs>aP=new List<bs>();}public class z{public string a1;public string bu="standard";public string cD="graphic";public string dT;
public string[]aM;public string ja="left";public string jb;public string iK="Monospace";public string bo;public string iz;
public int iA=10;public int ix=1;public string fH;public string[]dQ;public string iv;public string bp;public string br;
public float fw=2.5f;public string iy="cyan";public string[]dR;public List<string>dS=new List<string>();public List<string>mR(){
if(dS!=null&&dS.Count>0)return dS;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float bQ=1.0f;public int ne=100;public int bS=1;public int bT=1;public int je=30;public string a2;public int bt=0;
public List<am>az=new List<am>();public List<bs>fE{get{if(az.Count==0)az.Add(new am{cP="main"});return az[0].aP;}
}}public class a5{public string fF;public List<string>d_=new List<string>();public Dictionary<string,string>fA=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>fv=new List<string>();}public class fI{public List<aL>bq=new List<aL>();public List<z>R=new List<z>();
public List<string>ay=new List<string>();public Dictionary<string,List<string>>cz=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class iQ{static readonly Dictionary<string,a5>iC=new Dictionary<string,a5>(StringComparer.OrdinalIgnoreCase);
static iQ(){nf();}static void nf(){a3("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");a3("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");a3("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");a3("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");a3("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");a3("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");a3("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");a3("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void a3(string k9,string[]pj,string nZ){var ff=new a5{fF=k9}
;foreach(var pe in pj){var ha=pe.Split(new[]{'='},2);string lh=fG(ha[0]);ff.d_.Add(lh);if(ha.Length>1)ff.fA[lh]=Z(ha[1].Trim());
}var o7=nZ.Replace("\r\n","\n").Split('\n');foreach(var oV in o7){string lA=oV.Trim();if(lA.Length>0)ff.fv.Add(lA);}iC[k9]=ff;}static string fG(string ci){
ci=ci.Trim();if(ci.StartsWith("$"))ci=ci.Substring(1);return ci.ToLowerInvariant();}public static fI m3(string ob){var ad=new fI();z e=null;
am u=null;var lr=m9(ob,ad.ay,ad.cz);for(int de=0;de<lr.Count;de++){string c=lr[de].Trim();
if(c.Length==0)continue;try{if(c.StartsWith("@bind")){ad.bq.Add(m4(c));}else if(c.StartsWith("@screen")){e=new z{a1=c.Substring("@screen".Length).Trim()}
;ad.R.Add(e);u=null;}else if(c.StartsWith("profile:")){t(e,"profile:");e.bu=c.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(c.StartsWith("border:")){t(e,"border:");string jz=c.Substring("border:".Length).Trim();if(u!=null)u.iB=jz;
else e.cD=jz;}else if(c.StartsWith("border_color:")){t(e,"border_color:");string jC=c.Substring("border_color:".Length).Trim();
if(u!=null)u.fx=jC;else e.dT=jC;}else if(c.StartsWith("border_gradient:")||c.StartsWith("border_gradient(")){
t(e,"border_gradient");string bA=c.StartsWith("border_gradient:")? c.Substring("border_gradient:".Length): c.Substring("border_gradient".Length);
bA=bA.Trim();if(bA.StartsWith("(")&&bA.Contains(")"))bA=ap(bA,0);string[]lV=aq(bA).Select(pJ=>pJ.Trim()).ToArray();if(u!=null)u.fy=lV;
else e.aM=lV;}else if(c.StartsWith("title_align:")){t(e,"title_align:");string l1=c.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(u!=null)u.fN=l1;else e.ja=l1;}else if(c.StartsWith("title_color:")){t(e,"title_color:");string l4=c.Substring("title_color:".Length).Trim();
if(u!=null)u.fO=l4;else e.jb=l4;}else if(c.StartsWith("font_size:")){t(e,"font_size:");float l_=float.Parse(c.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(u!=null)u.iL=l_;else e.bQ=l_;}else if(c.StartsWith("font:")){t(e,"font:");string kh=c.Substring("font:".Length).Trim();
if(u!=null)u.iJ=kh;else e.iK=kh;}else if(c.StartsWith("bg:")||c.StartsWith("background:")){t(e,"bg:");string jH=c.Substring(c.IndexOf(':')+1).Trim();
if(u!=null)u.cy=jH;else e.bo=jH;}else if(c.StartsWith("layout:")){t(e,"layout:");m5(c.Substring("layout:".Length).Trim(),e);
}else if(c.StartsWith("refresh:")){t(e,"refresh:");e.ne=int.Parse(c.Substring("refresh:".Length).Trim());}else if(c.StartsWith("output:")){
t(e,"output:");var eU=aq(c.Substring("output:".Length).Trim());if(eU.Count==0)throw new FormatException("output: requires a target block name");
e.a2=Z(eU[0]);e.bt=eU.Count>1 ? int.Parse(eU[1]): 0;}else if(c.StartsWith("width:")){t(e,"width:");
e.je=int.Parse(c.Substring("width:".Length).Trim());}else if(c.StartsWith("boot_msg:")||c.StartsWith("boot:")){t(e,"boot_msg:");
e.iz=Z(c.Substring(c.IndexOf(':')+1).Trim());}else if(c.StartsWith("boot_stagger:")){t(e,"boot_stagger:");e.iA=int.Parse(c.Substring("boot_stagger:".Length).Trim());
}else if(c.StartsWith("boot_batch:")){t(e,"boot_batch:");e.ix=Math.Max(1,int.Parse(c.Substring("boot_batch:".Length).Trim()));
}else if(c.StartsWith("boot_duration:")){t(e,"boot_duration:");string c4=c.Substring("boot_duration:".Length).Trim().ToLowerInvariant();
bool kI=c4.EndsWith("s");if(kI)c4=c4.Substring(0,c4.Length-1).Trim();float gy;if(float.TryParse(c4,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out gy)){
e.fw=kI ? gy : gy/60f;}}else if(c.StartsWith("boot_log:")){t(e,"boot_log:");e.dS.Add(Z(c.Substring("boot_log:".Length).Trim()));
}else if(c.StartsWith("banner:")){t(e,"banner:");e.bp=Z(c.Substring("banner:".Length).Trim());}else if(c.StartsWith("boot_logo:")){
t(e,"boot_logo:");e.br=Z(c.Substring("boot_logo:".Length).Trim());}else if(c.StartsWith("boot_gradient:")||c.StartsWith("boot_gradient(")||c.StartsWith("banner_gradient:")||c.StartsWith("banner_gradient(")){
t(e,"boot_gradient");e.dR=i0(c);}else if(c.StartsWith("bg_gradient:")||c.StartsWith("bg_gradient(")){t(e,"bg_gradient");
e.dQ=i0(c);}else if(c.StartsWith("panel_bg:")){t(e,"panel_bg:");e.fH=c.Substring("panel_bg:".Length).Trim();
}else if(c.StartsWith("alert_bg:")){t(e,"alert_bg:");e.iv=c.Substring("alert_bg:".Length).Trim();}else if(c.StartsWith("boot_color:")||c.StartsWith("banner_color:")){
t(e,"boot_color:");e.iy=c.Substring(c.IndexOf(':')+1).Trim();}else if(c.StartsWith("orientation:")){t(e,"orientation:");
string pc=c.Substring("orientation:".Length).Trim().ToLowerInvariant();if(u!=null)u.iY=pc;}else if(c.StartsWith("@panel")){t(e,"@panel");
u=m7(c);e.az.Add(u);}else if(c.StartsWith("title:")){t(e,"title:");if(u==null){u=new am{
cP="main",cF=0,cQ=0,cR=e.bS,cS=e.bT};e.az.Add(u);}u.bw=Z(c.Substring("title:".Length).Trim());
}else if(c.StartsWith("line:")){t(e,"line:");if(u==null){u=new am{cP="main",cF=0,cQ=0,cR=e.bS,cS=e.bT}
;e.az.Add(u);}u.aP.Add(m6(c.Substring("line:".Length).Trim()));}else{ad.ay.Add($"Line {de + 1}: unrecognized directive: \"{c}\"");
}}catch(Exception oi){ad.ay.Add($"Line {de + 1}: parse error ({oi.Message}) in \"{c}\"");}}foreach(var lB in ad.R){if(string.IsNullOrEmpty(lB.a2))ad.ay.Add($"@screen {lB.a1}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aG in ad.R){if(aG.bp!=null&& !ad.cz.ContainsKey(aG.bp))ad.ay.Add("Screen '"+aG.a1+"': banner '"+aG.bp+"' is not defined (@banner "+aG.bp+" ... @end)");
if(aG.br!=null&& !ad.cz.ContainsKey(aG.br))ad.ay.Add("Screen '"+aG.a1+"': boot_logo '"+aG.br+"' is not defined (@banner "+aG.br+" ... @end)");
}return ad;}static void m5(string gg,z lI){int hx=gg.IndexOf('(');if(hx<0)throw new FormatException("missing '(' in layout: specification");
string kP=gg.Substring(0,hx).Trim().ToLowerInvariant();string nF=ap(gg,hx);var f2=aq(nF);if(kP=="grid"){if(f2.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
lI.bS=Math.Max(1,int.Parse(f2[0]));lI.bT=Math.Max(1,int.Parse(f2[1]));}else{throw new FormatException($"unknown layout kind '{kP}' (expected grid)");
}}static am m7(string o2){string dB=o2.Substring("@panel".Length).Trim();int ed=dB.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(ed<0)throw new FormatException("@panel requires an at(...) positioning clause");
string pa=dB.Substring(0,ed).Trim();string jw=dB.Substring(ed);int ln=jw.IndexOf('(');string ju=ap(jw,ln);
var bz=aq(ju);if(bz.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var aT=new am{cP=pa,cF=int.Parse(bz[0]),cQ=int.Parse(bz[1]),cR=bz.Count>2 ? int.Parse(bz[2]): 1,cS=bz.Count>3 ? int.Parse(bz[3]): 1}
;int jo=ed+ln+ju.Length+1;if(jo<dB.Length){string ao=dB.Substring(jo).Trim();int l9=ao.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(l9>=0){string dH=ao.Substring(l9+5).Trim();if(dH.StartsWith("(")&&dH.Contains(")"))aT.bw=Z(ap(dH,0));else if(dH.StartsWith(":")){
string bB=dH.Substring(1).Trim();if(bB.StartsWith("\"")){int kd=bB.IndexOf('"',1);aT.bw=kd>0 ? bB.Substring(1,kd-1): Z(bB);}else{int lR=bB.IndexOf(' ');
aT.bw=lR>=0 ? bB.Substring(0,lR): bB;}}}string lc=dW(ao,"orientation:");if(lc!=null)aT.iY=lc.ToLowerInvariant();string jn=dW(ao,"title_align:");
if(jn!=null)aT.fN=jn.ToLowerInvariant();string l6=dW(ao,"title_color:");if(l6!=null)aT.fO=l6;int ei=ao.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(ei>=0&& !ao.Substring(ei).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !ao.Substring(ei).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
aT.iB=dW(ao.Substring(ei),"border:");}int eg=ao.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(eg<0)eg=ao.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(eg>=0){string bc=ao.Substring(eg+"border_gradient".Length).Trim();if(bc.StartsWith(":"))bc=bc.Substring(1).Trim();if(bc.StartsWith("(")&&bc.Contains(")"))bc=ap(bc,0);
aT.fy=aq(bc).Select(pK=>pK.Trim()).ToArray();}}return aT;}static string dW(string lC,string kO){int kH=lC.IndexOf(kO,StringComparison.OrdinalIgnoreCase);
if(kH<0)return null;string ip=lC.Substring(kH+kO.Length).Trim();int lS=ip.IndexOf(' ');return lS>=0 ? ip.Substring(0,lS): ip;}static string[]i0(string hh){
int gO=hh.IndexOfAny(new[]{':','('});string da=hh.Substring(hh[gO]==':' ? gO+1 : gO).Trim();if(da.StartsWith("(")&&da.Contains(")"))da=ap(da,0);return aq(da).Select(pL=>pL.Trim()).ToArray();
}static List<string>m9(string oc,List<string>gC,Dictionary<string,List<string>>nT){var me=new Dictionary<string,a5>(StringComparer.OrdinalIgnoreCase);
var cm=(oc ?? "").Replace("\r\n","\n").Split('\n');var hs=new List<string>();int ce=0;while(ce<cm.Length){string hK=cm[ce++];if(hK.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var _=new List<string>();while(ce<cm.Length){string jl=cm[ce++].Replace("\t","    ").TrimEnd();if(jl.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
_.Add(jl);}while(_.Count>0&&_[0].Length==0)_.RemoveAt(0);while(_.Count>0&&_[_.Count-1].Length==0)_.RemoveAt(_.Count-1);int g1=int.MaxValue;
foreach(var fW in _)if(fW.Length>0)g1=Math.Min(g1,fW.Length-fW.TrimStart().Length);for(int dl=0;dl<_.Count;dl++)if(_[dl].Length>0)_[dl]=_[dl].Substring(g1);
nT[hK.TrimStart().Substring(8).Trim()]=_;continue;}string bg=i6(hK).Trim();if(bg.Length==0)continue;if(bg.StartsWith("@template")){try{
var ig=m8(bg);while(ce<cm.Length){string n0=cm[ce++];string gh=i6(n0).Trim();if(gh.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(gh.Length>0)ig.fv.Add(gh);}me[ig.fF]=ig;}catch(Exception oj){gC.Add($"Template parse error ({oj.Message}) in \"{bg}\"");}}
else if(bg.StartsWith("@use")){try{iF(bg,me,hs,gC);}catch(Exception ok){gC.Add($"@use error ({ok.Message}) in \"{bg}\"");
}}else{hs.Add(bg);}}return hs;}static a5 m8(string o3){string hL=o3.Substring("@template".Length).Trim();int hy=hL.IndexOf('(');
if(hy<0)throw new FormatException("missing '(' in @template header");string pb=hL.Substring(0,hy).Trim();string nG=ap(hL,hy);var ih=new a5{
fF=pb};foreach(var pf in aq(nG)){var hb=pf.Split(new[]{'='},2);string li=fG(hb[0]);ih.d_.Add(li);if(hb.Length>1)ih.fA[li]=Z(hb[1].Trim());
}return ih;}static void iF(string o4,Dictionary<string,a5>mf,List<string>lf,List<string>oh,int j3=0){string e1=o4.Substring("@use".Length).Trim();
int eS=e1.IndexOf('(');string hl=eS>=0 ? e1.Substring(0,eS).Trim(): e1.Trim();string nH=eS>=0 ? ap(e1,eS): "";a5 cs=null;
if(!mf.TryGetValue(hl,out cs)){if(!iC.TryGetValue(hl,out cs))throw new FormatException($"unknown template '{hl}'");}var ek=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var kT in cs.fA)ek[kT.Key]=kT.Value;var jt=aq(nH);for(int cZ=0;cZ<jt.Count;cZ++){string eb=jt[cZ];int gA=eb.IndexOf('=');if(gA>=0){
string oN=fG(eb.Substring(0,gA));string qk=Z(eb.Substring(gA+1).Trim());ek[oN]=qk;}else{if(cZ<cs.d_.Count){string oO=cs.d_[cZ];
ek[oO]=Z(eb.Trim());}}}foreach(var n_ in cs.fv){string c7=n_;foreach(var eK in ek){c7=c7.Replace("$"+eK.Key,eK.Value).Replace("{"+eK.Key+"}",eK.Value);
}if(c7.StartsWith("@use")){if(j3>3)throw new FormatException("@use nested too deep");iF(c7,mf,lf,oh,j3+1);
}else lf.Add(c7);}}static void t(z oa,string oe){if(oa==null)throw new FormatException($"'{oe}' used before any @screen declared");
}static string i6(string aF){if(string.IsNullOrEmpty(aF))return aF;bool g0=false;for(int bf=0;bf<aF.Length;bf++){char gk=aF[bf];if(gk=='"'){g0= !g0;
}else if(!g0){if(gk=='/'&&bf+1<aF.Length&&aF[bf+1]=='/'){return aF.Substring(0,bf);}if(gk=='#'){if(mW(aF,bf)){continue;}return aF.Substring(0,bf);}}}return aF;
}static bool mW(string bC,int kK){int cg=0;for(int gX=kK+1;gX<bC.Length;gX++){char b4=bC[gX];if((b4>='0'&&b4<='9')||(b4>='a'&&b4<='f')||(b4>='A'&&b4<='F'))cg++;
else break;}if(cg==3||cg==4||cg==6||cg==8){int dp=kK+1+cg;if(dp>=bC.Length||char.IsWhiteSpace(bC[dp])||bC[dp]==')'||bC[dp]==','||bC[dp]==':')return true;
}return false;}static aL m4(string o5){string hM=o5.Substring("@bind".Length).Trim();int gB=hM.IndexOf('=');if(gB<0)throw new FormatException("missing '=' in @bind");
string la=hM.Substring(0,gB).Trim();string gD=hM.Substring(gB+1).Trim();int hz=gD.IndexOf('(');if(hz<0)throw new FormatException("missing '(' in @bind target");
string ew=gD.Substring(0,hz).Trim().ToLowerInvariant();string nI=ap(gD,hz);var a9=aq(nI);if(a9.Count==0)throw new FormatException("@bind target has no arguments");
var ae=new aL{P=la};if(ew=="dynamic"||ew=="var"||ew=="mem"){ae.a_=aw.cI;ae.aN=a9.Count>0 ? Z(a9[0].Trim()): la;
for(int fX=0;fX<a9.Count;fX++){var eL=a9[fX].Split(new[]{':'},2);if(eL.Length==2&&eL[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double j2;
if(double.TryParse(eL[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out j2))ae.dV=j2;else ae.fz=Z(eL[1].Trim());
}}return ae;}ae.a_=ew=="group" ? aw.fB : aw.mx;string ea=null;for(int d9=0;d9<a9.Count;d9++){var hc=a9[d9].Split(new[]{':'},2);if(hc.Length==2&&hc[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
ea=hc[1].Trim().ToLowerInvariant();}else{ae.ar.Add(Z(a9[d9].Trim()));}}if(ae.ar.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ae.a_==aw.fB){if(ea==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(ea){case "avg": ae.cx=al.fu;break;case "sum": ae.cx=al.i7;break;case "min": ae.cx=al.iX;break;case "max": ae.cx=al.iW;
break;default: throw new FormatException($"unknown agg '{ea}' (expected avg/sum/min/max)");}}return ae;}static bs m6(string eh){var n=new bs();
if(!eh.StartsWith("\""))throw new FormatException("line must start with a quoted label");int gz=eh.IndexOf('"',1);if(gz<0)throw new FormatException("unterminated quoted label");
n.N=eh.Substring(1,gz-1);string dA=eh.Substring(gz+1).Trim();int eT=dA.IndexOf('(');if(eT<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string x=dA.Substring(0,eT).Trim().ToLowerInvariant();string nE=ap(dA,eT);string T=dA.Substring(dA.IndexOf(')',eT)+1).Trim();
var gw=nE.Trim().Split('.');if(gw.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
n.aX=gw[0].Trim();n.I=gw[1].Trim();if(x=="bar")n.g=d.cA;else if(x=="led")n.g=d.cN;else if(x=="spark"||x=="sparkline")n.g=d.bv;
else if(x=="hazard"||x=="stripes")n.g=d.cL;else if(x=="chevron"||x=="chevrons")n.g=d.cE;else if(x=="dots"||x=="dot")n.g=d.cH;
else if(x=="balance"||x=="horizon"||x=="centered")n.g=d.bK;else if(x=="toggle"||x=="switch")n.g=d.bW;else if(x=="checkbox"||x=="check")n.g=d.bO;
else if(x=="status"||x=="badge"||x=="state")n.g=d.bV;else if(x=="ammo"||x=="pips")n.g=d.bJ;else if(x=="flow"||x=="rate"||x=="spinner")n.g=d.bP;
else if(x=="text")n.g=d.i9;else throw new FormatException($"unknown display function '{x}'");while(T.Length>0){int gu=T.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int ca=T.IndexOf('(');if(gu>=0&&(ca<0||gu<ca)){string en=T.Substring(gu+4).Trim();int e8=en.IndexOf(' ');n.iD=(e8>=0 ? en.Substring(0,e8): en).ToLowerInvariant();
T=(e8>=0 ? en.Substring(e8): "").Trim();continue;}int im=T.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(im>=0&&(ca<0||im<ca)){
string fp=T.Substring(im+5).Trim();int e9=fp.IndexOf(' ');n.cU=(e9>=0 ? fp.Substring(0,e9): fp).Trim().Trim('"');T=(e9>=0 ? fp.Substring(e9): "").Trim();
continue;}int gI=T.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(gI>=0&&(ca<0||gI<ca)){string er=T.Substring(gI+7).Trim();
int fa=er.IndexOf(' ');n.cJ=(fa>=0 ? er.Substring(0,fa): er).Trim().Trim('"');T=(fa>=0 ? er.Substring(fa): "").Trim();continue;}int eN=T.IndexOf('(');
if(eN<0)break;string af=T.Substring(0,eN).Trim().ToLowerInvariant();int kY=af.LastIndexOf(' ');if(kY>=0)af=af.Substring(kY+1).Trim();
string bh=ap(T,eN);T=T.Substring(T.IndexOf(')',eN)+1).Trim();if(af=="gradient"){n.cK=true;n.bR=aq(bh).Select(pM=>pM.Trim()).ToArray();
if(n.bR.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(af=="flat"){n.ah=bh.Trim();}else if(af=="pattern"){
string H=bh.Trim().ToLowerInvariant();if(H=="hazard"||H=="stripes")n.g=d.cL;else if(H=="chevron"||H=="chevrons")n.g=d.cE;
else if(H=="dots"||H=="dot")n.g=d.cH;else if(H=="led")n.g=d.cN;else if(H=="balance"||H=="horizon")n.g=d.bK;
else if(H=="toggle"||H=="switch")n.g=d.bW;else if(H=="checkbox"||H=="check")n.g=d.bO;else if(H=="status"||H=="badge"||H=="state")n.g=d.bV;
else if(H=="ammo"||H=="pips")n.g=d.bJ;else if(H=="flow"||H=="rate"||H=="spinner")n.g=d.bP;}else if(af=="color"){n.aY=new List<cG>();
foreach(var hA in aq(bh)){int gr=hA.LastIndexOf(':');if(gr<0)throw new FormatException("color() entries need 'expr : color'");string k_=hA.Substring(0,gr).Trim();
string jY=hA.Substring(gr+1).Trim();if(k_.Equals("default",StringComparison.OrdinalIgnoreCase)){n.aY.Add(new cG{dY=true,dU=jY}
);}else{string p3=nw(k_);n.aY.Add(new cG{iG=p3,dU=jY});}}if(!n.aY.Any(pA=>pA.dY))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
}else if(af=="dir"||af=="direction"){n.iD=bh.Trim().ToLowerInvariant();}else if(af=="unit"){n.cU=bh.Trim().Trim('"');}else if(af=="max"){
n.a0=double.Parse(bh.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture);}else if(af=="format"||af=="fmt"){
n.cJ=bh.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{af}'");}}return n;}static string ap(string e4,int hq){
int gt=0;for(int df=hq;df<e4.Length;df++){if(e4[df]=='(')gt++;else if(e4[df]==')'){gt--;if(gt==0)return e4.Substring(hq+1,df-hq-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aq(string nJ){return nJ.Split(',').Select(pN=>pN.Trim()).Where(pO=>pO.Length>0).ToList();
}static string nw(string b9){b9=b9.Trim();foreach(var pd in new[]{">=","<=","==",">","<"}){int gY=b9.IndexOf(pd,StringComparison.Ordinal);
if(gY>0)return b9.Substring(gY).Trim();if(gY==0)return b9;}throw new FormatException($"color() rule has no recognizable comparison operator: '{b9}'");}
static string Z(string bD){if(bD.Length>=2&&bD[0]=='"'&&bD[bD.Length-1]=='"')return bD.Substring(1,bD.Length-2);return bD;}}public static class mv{public static double mu(al nD,List<double>cu){
if(cu==null||cu.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(nD){case al.fu: return cu.Average();case al.i7: return cu.Sum();
case al.iX: return cu.Min();case al.iW: return cu.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class E{
public static string nj(string oX,double eV,int jB=10,string ml=null){eV=Math.Max(0,Math.Min(100,eV));int kn=(int)Math.Round(jB*(eV/100.0));
string nU=new string('#',kn)+new string('-',jB-kn);string ql=ml!=null ? ml : $"{eV:0}%";return $"{oX} [{nU}] {ql}";
}public static string mQ(double bn,string il=null,string ev=null){if(il=="auto"){double fY=Math.Abs(bn);if(fY>=1e9)return(bn/1e9).ToString("F1")+"G";
if(fY>=1e6)return(bn/1e6).ToString("F1")+"M";if(fY>=1e3)return(bn/1e3).ToString("F1")+"k";string ol= !string.IsNullOrEmpty(ev)? ev : "F0";return bn.ToString(ol);
}string kp= !string.IsNullOrEmpty(ev)? ev :(Math.Abs(bn)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(il)){return bn.ToString(kp);}return bn.ToString(kp)+il;
}public static float mS(string ps,float lZ,float ic,float eo=1.0f){string cj=(ps ?? "standard").ToLowerInvariant();
if(cj=="compact"||(cj=="auto"&&lZ<=300&&ic<=300&&ic>160)){return eo*0.75f;}if(cj=="corner"||(cj=="auto"&&ic<=160)){
return eo*0.55f;}if(cj=="wide"||(cj=="auto"&&lZ>=1000)){return eo*1.15f;}return eo;}public static string nh(string oY,double eW,int bd=5){
eW=Math.Max(0,Math.Min(100,eW));double ep=(eW-50.0)/50.0;int dm=0;int dD=0;if(ep< -0.01)dm=(int)Math.Round(-ep*bd);
else if(ep>0.01)dD=(int)Math.Round(ep*bd);if(dm>bd)dm=bd;if(dD>bd)dD=bd;string o0=new string('-',bd-dm)+new string('<',dm);
string pG=new string('>',dD)+new string('-',bd-dD);return $"{oY} [{o0}|{pG}] {eW:0}%";}public static string ns(string kV,double qn){
return qn>=50.0 ? $"{kV} [====O] ON" : $"{kV} [O====] OFF";}public static string nm(string kW,double qo){return qo>=50.0 ? $"{kW} [X] ACTIVE" : $"{kW} [ ] READY ";
}public static string nr(string he,double mk){if(mk>=75.0)return $"{he} [ ONLINE  ]";if(mk>=25.0)return $"{he} [ STANDBY ]";return $"{he} [ OFFLINE ]";
}public static string ng(string oZ,double eX,int k2=8){eX=Math.Max(0,Math.Min(100,eX));int ko=(int)Math.Round(k2*(eX/100.0));
string pp=new string('|',ko)+new string('.',k2-ko);return $"{oZ} [{pp}] {eX:0}%";}public static string nn(string hf,double dy,int l8=0){
char[]pZ=new[]{'|','/','-','\\'};char lD=pZ[(l8>=0 ? l8 : 0)%4];if(dy>55.0)return $"{hf} {lD} ▲ CHG {dy:0}%";if(dy<45.0)return $"{hf} {lD} ▼ DSC {dy:0}%";
return $"{hf} ● STBY {dy:0}%";}public static bool m_(string ag,string di){if(ag==null||di==null)return false;if(ag=="*"||ag==di)return true;
if(!ag.Contains("*")&& !ag.Contains("?"))return string.Equals(ag,di,StringComparison.OrdinalIgnoreCase);int ak=0,dg=0,h5= -1,hk=0;while(dg<di.Length){
if(ak<ag.Length&&(ag[ak]=='?'||char.ToLowerInvariant(ag[ak])==char.ToLowerInvariant(di[dg]))){ak++;dg++;}else if(ak<ag.Length&&ag[ak]=='*'){
h5=ak;hk=dg;ak++;}else if(h5!= -1){ak=h5+1;hk++;dg=hk;}else{return false;}}while(ak<ag.Length&&ag[ak]=='*')ak++;return ak==ag.Length;
}public static int mz(double pg,int mc){return Math.Max(0,Math.Min(mc,1+(int)(Math.Max(0.0,pg)*mc)));}public static string dX(string[]cr,double hB){
if(cr==null||cr.Length==0)return "white";hB=Math.Max(0,Math.Min(100,hB));int gZ=(int)Math.Floor(hB/100.0*cr.Length);if(gZ>=cr.Length)gZ=cr.Length-1;
return cr[gZ];}public static j iN(string[]bm,double pm){if(bm==null||bm.Length==0)return j.d2;if(bm.Length==1)return j.bU(bm[0]);
double pD=Math.Max(0.0,Math.Min(1.0,pm/100.0));double lH=pD*(bm.Length-1);int g_=(int)Math.Floor(lH);int oG=Math.Min(bm.Length-1,g_+1);
double h9=lH-g_;j b5=j.bU(bm[g_]);j gl=j.bU(bm[oG]);byte pB=(byte)Math.Round(b5.aQ+(gl.aQ-b5.aQ)*h9);
byte oB=(byte)Math.Round(b5.aO+(gl.aO-b5.aO)*h9);byte nS=(byte)Math.Round(b5.aK+(gl.aK-b5.aK)*h9);return new j(pB,oB,nS);}public static string i2(List<cG>lz,double qp){
foreach(var hO in lz){if(hO.dY)continue;if(mO(hO.iG,qp))return hO.dU;}var j1=lz.FirstOrDefault(pC=>pC.dY);return j1!=null ? j1.dU : null;
}static bool mO(string c8,double dM){c8=c8.Trim();foreach(var hp in new[]{">=","<=","==",">","<"}){if(c8.StartsWith(hp)){double dC=double.Parse(c8.Substring(hp.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(hp){case ">=": return dM>=dC;case "<=": return dM<=dC;case "==": return Math.Abs(dM-dC)<1e-9;case ">": return dM>dC;case "<": return dM<dC;
}}}throw new FormatException($"unsupported comparison expression: '{c8}'");}public static List<string>nl(List<string>n9,int cv){
var eP=new List<string>();eP.Add("+"+new string('-',cv+2)+"+");foreach(var hi in n9){string qg=hi.Length>cv ? hi.Substring(0,cv): hi;
string ph=qg.PadRight(cv);eP.Add("| "+ph+" |");}eP.Add("+"+new string('-',cv+2)+"+");return eP;}public struct i_{public float jg,jh,jf,iP;
}public static i_ mE(int jW,int lx,int fb,int fc,int dJ,int dK,float p7,float p6,float lj,float lk,float gQ,float gR){
dJ=Math.Max(1,dJ);dK=Math.Max(1,dK);fb=Math.Max(1,Math.Min(dJ-jW,fb));fc=Math.Max(1,Math.Min(dK-lx,fc));
float nL=p7-(lj*2f);float nK=p6-(lk*2f);float jR=(nL-(gQ*(dJ-1)))/dJ;float jQ=(nK-(gR*(dK-1)))/dK;
float pu=lj+jW*(jR+gQ);float pv=lk+lx*(jQ+gR);float pt=fb*jR+((fb-1)*gQ);float pn=fc*jQ+((fc-1)*gR);return new i_{
jg=pu,jh=pv,jf=pt,iP=pn};}public struct i3{public int m2;public int fD;public int S;public int ai;public int cT;
public int ax;public int nx;public int ny;public int mL;public int mM;
public double mN;}public static i3 mF(z bk){var h=new i3();var lm=bk.az!=null&&bk.az.Count>0 ? bk.az : new List<am>{
new am{aP=bk.fE}};h.m2=lm.Count;h.fD=0;h.S=0;h.ai=0;if(!string.IsNullOrEmpty(bk.bo)&& !bk.bo.Equals("none",StringComparison.OrdinalIgnoreCase))h.S++;
if(!string.IsNullOrEmpty(bk.cD)&& !bk.cD.Equals("none",StringComparison.OrdinalIgnoreCase))h.S+=2;foreach(var ds in lm){
if(!string.IsNullOrEmpty(ds.cy)&& !ds.cy.Equals("none",StringComparison.OrdinalIgnoreCase))h.S++;if(!string.IsNullOrEmpty(ds.bw))h.S+=2;
h.fD+=ds.aP.Count;foreach(var oW in ds.aP){h.nx++;h.ny++;h.S+=2;switch(oW.g){case d.cA: h.cT++;
h.ax++;h.S+=2;h.ai++;break;case d.cN: h.ax+=10;h.S+=10;
h.ai+=10;break;case d.cL: h.ax+=10;h.S+=11;h.ai+=10;break;case d.cE: h.ax+=10;
h.S+=10;h.ai+=10;break;case d.cH: h.ax+=12;h.S+=12;h.ai+=12;
break;case d.bv: h.cT++;h.ax+=8;h.S+=9;h.ai+=8;break;case d.bK: h.cT+=2;
h.ax++;h.S+=3;h.ai++;break;case d.bW: case d.bO: h.cT++;
h.ax++;h.S+=2;h.ai++;break;case d.bV: case d.bP: h.cT++;h.ax+=2;
h.S+=3;h.ai+=2;break;case d.bJ: h.ax+=8;h.S+=8;h.ai+=8;
break;default: h.ai++;break;}}}h.mL=25+(h.fD*2);h.mM=180+(h.ai*4);
h.mN=0.0;return h;}}