fy r;string fJ=null;Dictionary<string,List<IMyTerminalBlock>>bX=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>d0=new Dictionary<string,bool>();
Dictionary<string,aJ>cS=new Dictionary<string,aJ>();Dictionary<string,Color>i0=new Dictionary<string,Color>();List<IMyTerminalBlock>a5=new List<IMyTerminalBlock>();
bool dY;int fH;string dZ="";StringBuilder cV=new StringBuilder();readonly Dictionary<string,double>am=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>d_=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener cT;bool bV=false;
Dictionary<z,IMyTextSurface>bY=new Dictionary<z,IMyTextSurface>();Dictionary<z,string>bW=new Dictionary<z,string>();
static readonly string[]iI=new string[101];static Program(){for(int es=0;es<=100;es++){iI[es]=es+"%";}}static string iw(double p4){int dc=(int)Math.Round(p4);
if(dc<0)dc=0;else if(dc>100)dc=100;return iI[dc];}class a2{public List<MySprite>l=new List<MySprite>();public List<cL>dV=new List<cL>();
public Vector2 iC;public bool iB=false;}class cL{public bq iE;public int S= -1;public int a4= -1;public int ft= -1;
public bool ix=false;public List<int>U=new List<int>();public float[]dW=new float[8];public float cz;public float dK;
public float bK;public float cy;public double dT= -99999.0;public string iD;public float K= -1f;}Dictionary<z,a2>d2=new Dictionary<z,a2>();
int i2=0;double fL=0;double b_=0;int cU=0;Dictionary<z,double>aN=new Dictionary<z,double>();int bZ=0;
const string mD="0.11.0";IMyTextSurface a6;bool fG=true;double fF=0;string i1="";string fK="";int d1=0,fI=0;
List<KeyValuePair<string,int>>bU=new List<KeyValuePair<string,int>>();const double mV=120;const float ma=22f;public Program(){
Runtime.UpdateFrequency=UpdateFrequency.Update10;try{cT=IGC.RegisterBroadcastListener("HELM");cT.SetMessageCallback("HELM_IGC");
}catch{}fz();iO();iA();}public void Main(string je,UpdateType p0){double jT=Runtime.TimeSinceLastRun.TotalSeconds;b_+=jT;
if((p0&UpdateType.IGC)!=0||je=="HELM_IGC"){mT();}string lc=(je ?? "").Trim();string ai=lc.ToLowerInvariant();if(ai=="reload"){
fz();}else if(ai=="refresh"){fA();}else if(ai=="boot"){iO();}else if(ai=="console on"||ai=="console off"){fG=ai=="console on";
iA();fF=0;}else if(ai=="scan"||ai=="diagnose"||ai=="test"){nd();return;}else if(ai.StartsWith("set ")){string eU=lc.Substring(4).Trim();
int jW=eU.IndexOf('=');int pE=eU.IndexOf(' ');int hO=jW>0 ? jW : pE;if(hO>0){string ku=eU.Substring(0,hO).Trim();string lY=eU.Substring(hO+1).Trim();id($"{ku}={lY}");
Echo($"[HELM] Set dynamic variable '{ku}' = {lY}");}return;}else if(ai.StartsWith("alert")){string dG=ai.Length>5 ? ai.Substring(5).Trim(): "toggle";if(dG=="toggle")bV= !bV;
else bV=dG=="red"||dG=="on"||dG=="1"||dG=="true";Echo($"[HELM] Red alert state: {bV}");return;}else if(Me.CustomData!=fJ){
fz();}bZ++;fL+=jT;if(fL>=mV){fL=0;fA();}if(r==null||r.O.Count==0){
iW();im();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var hJ=r.O[i2%r.O.Count];
i2++;fK=hJ.aZ;bool fP=aN.Count>0;if(fP)m2();if(!aN.ContainsKey(hJ)&&m6(hJ))fP=true;cU=fP ? 0 : cU+1;
Runtime.UpdateFrequency=cU<=r.O.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;iW();im();d1=Runtime.CurrentInstructionCount;
if(d1>fI)fI=d1;}void im(){if(dZ.Length>0)Echo(dZ);if(cV.Length>0)Echo(cV.ToString());}List<IMyTerminalBlock>dJ(){
if(!dY){a5.Clear();GridTerminalSystem.GetBlocks(a5);int pn=a5.Count;a5.RemoveAll(nu=> !nu.IsSameConstructAs(Me));fH=pn-a5.Count;
dY=true;}return a5;}IMyTerminalBlock ir(string kP){var fW=GridTerminalSystem.GetBlockWithName(kP);if(fW==null||fW.IsSameConstructAs(Me))return fW;
var fR=dJ();for(int et=0;et<fR.Count;et++)if(fR[et].CustomName.IndexOf(kP,StringComparison.OrdinalIgnoreCase)>=0)return fR[et];return null;}void mk(string kQ,List<IMyTerminalBlock>de){
var j9=GridTerminalSystem.GetBlockGroupWithName(kQ);if(j9==null)return;int jn=de.Count;var dD=new List<IMyTerminalBlock>();j9.GetBlocks(dD);ic(dD,de);
if(de.Count>jn)return;var kk=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(kk);foreach(var l7 in kk){if(l7.Name.IndexOf(kQ,StringComparison.OrdinalIgnoreCase)<0)continue;
dD.Clear();l7.GetBlocks(dD);ic(dD,de);if(de.Count>jn)return;}}void ic(List<IMyTerminalBlock>oc,List<IMyTerminalBlock>ks){foreach(var fX in oc)if(fX.IsSameConstructAs(Me)&& !ks.Contains(fX))ks.Add(fX);
}void fz(){fJ=Me.CustomData;r=iz.mM(fJ);dZ="";if(r.ax.Count>0){var ll=new StringBuilder("HELM config errors:\n");
foreach(var nZ in r.ax)ll.AppendLine(" - "+nZ);dZ=ll.ToString();}cS.Clear();foreach(var fY in r.bo)if(!cS.ContainsKey(fY.N))cS[fY.N]=fY;
d2.Clear();bW.Clear();aN.Clear();fA();}void mT(){try{while(cT!=null&&cT.HasPendingMessage){
var kM=cT.AcceptMessage();if(kM.Data is string){id((string)kM.Data);}}}catch{}}void id(string k5){
if(string.IsNullOrEmpty(k5))return;var k2=k5.Split(';');for(int gG=0;gG<k2.Length;gG++){var ds=k2[gG].Trim();if(ds.Length==0)continue;int c3=ds.IndexOf('=');
if(c3<0)c3=ds.IndexOf(':');if(c3>0){string cc=ds.Substring(0,c3).Trim();string aI=ds.Substring(c3+1).Trim();if(cc.Equals("alert",StringComparison.OrdinalIgnoreCase)){
bV=aI.Equals("red",StringComparison.OrdinalIgnoreCase)||aI=="1"||aI.Equals("true",StringComparison.OrdinalIgnoreCase)||aI.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double kT;d_.Remove(cc);if(double.TryParse(aI,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out kT)){
am[cc]=kT;}else if(aI.Equals("true",StringComparison.OrdinalIgnoreCase)||aI.Equals("on",StringComparison.OrdinalIgnoreCase)){am[cc]=100.0;
}else if(aI.Equals("false",StringComparison.OrdinalIgnoreCase)||aI.Equals("off",StringComparison.OrdinalIgnoreCase)){am[cc]=0.0;}else{d_[cc]=aI;
}}}}}void ab(int pN,string pU){bU.Add(new KeyValuePair<string,int>(pU,pN));}static string iV(string hB,int kO){return hB.Length>kO ? hB.Substring(0,kO): hB;
}void iA(){var ht=Me as IMyTextSurfaceProvider;if(ht==null||ht.SurfaceCount<1)return;a6=ht.GetSurface(0);a6.ContentType=fG ? ContentType.SCRIPT : ContentType.NONE;
a6.Script="";}void iW(){if(b_>=fF){fF=b_+(cU>r.O.Count+1 ? 5.0 : 1.0);mi();if(fG&&a6!=null)mr();
}Echo(i1);}void mi(){bU.Clear();int f6=0,jq=0;foreach(var jg in r.bo){List<IMyTerminalBlock>g0;if(jg.aX==av.cF){
f6++;continue;}if(bX.TryGetValue(jg.N,out g0)){jq+=g0.Count;if(g0.Count>0)f6++;}}int jX=r.ax.Count;ab(0,"HELM-[Kernel-(v"+mD+")]");
ab(4,"------------------------------");ab(1,"krn-cfg");ab(2," screens "+r.O.Count+"  binds "+f6+"/"+r.bo.Count+"  errors "+jX);
ab(2," alert "+(bV ? "RED" : "off")+"  blocks "+jq+"  docked-ignored "+fH);if(jX>0)ab(3," ! "+iV(r.ax[0],40));
ab(1,"cfg-dsp");int pB=0;foreach(var hC in r.O){if(++pB>10){ab(2," +"+(r.O.Count-10)+" more");break;}IMyTextSurface hP;string lu="--",hT="no block";
if(bY.TryGetValue(hC,out hP)){lu=hP.SurfaceSize.X.ToString("0")+"x"+hP.SurfaceSize.Y.ToString("0");hT=aN.ContainsKey(hC)? "boot" : "live";
}ab(hT=="no block" ? 3 : 2," "+iV(hC.aZ,14).PadRight(14)+" "+lu.PadRight(9)+" "+hT);}ab(1,"perf");ab(2," tick "+bZ+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ab(2," instr "+d1+"  peak "+fI+"  max "+Runtime.MaxInstructionCount);ab(2," rate "+Runtime.UpdateFrequency+"  idle "+cU+"  up "+(int)b_+"s");
if(fK.Length>0)ab(2," last "+fK);var lm=new StringBuilder();foreach(var oy in bU)lm.AppendLine(oy.Key);i1=lm.ToString();}static Color mn(int pO){
switch(pO){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void mr(){Vector2 bC=a6.SurfaceSize;Vector2 g8=(a6.TextureSize-bC)/2f;int gd=30;foreach(var oz in bU)gd=Math.Max(gd,oz.Key.Length);
Vector2 gc=a6.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float ln=Math.Min(1f,Math.Min(bC.X*0.94f/(gd*gc.X),bC.Y*0.94f/(bU.Count*gc.Y)));
float qa=g8.X+bC.X*0.03f;float l8=g8.Y+bC.Y*0.03f;var hQ=new List<MySprite>();hQ.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=g8+bC/2f,Size=bC,Color=new Color(12,12,16)}
);foreach(var kz in bU){ct(hQ,kz.Key,mn(kz.Value),qa,l8,ln);l8+=gc.Y*ln;}using(var n7=a6.DrawFrame()){n7.AddRange(hQ);}}static readonly List<string>mh=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void iO(){aN.Clear();if(r==null)return;for(int eu=0;eu<r.O.Count;eu++){var eX=r.O[eu];if(eX.fn>0f)aN[eX]=b_+(eu/Math.Max(1,eX.ie))*eX.ii/60.0;
}}void m2(){List<z>c2=null;int oi=(aN.Count+3)/4,oq=0;foreach(var eC in aN){double hf=(b_-eC.Value)/eC.Key.fn;if(hf<0)continue;
if(hf>=1.0){if(c2==null)c2=new List<z>();c2.Add(eC.Key);continue;}if((oq++ +bZ)%oi==0)mq(eC.Key,hf);}if(c2==null)return;foreach(var hD in c2){
aN.Remove(hD);d2.Remove(hD);bW.Remove(hD);}}static void ct(List<MySprite>pF,string pV,Color nM,float qb,float qd,float py){
var lI=MySprite.CreateText(pV,"Monospace",nM,py,TextAlignment.LEFT);lI.Position=new Vector2(qb,qd);pF.Add(lI);}static void ib(List<MySprite>pG,List<string>d6,z e_,float qc,float qe,float pz,float oK){
string[]pL=e_.dL!=null&&e_.dL.Length>0 ? e_.dL : new[]{e_.ig};for(int c8=0;c8<d6.Count;
c8++){j f7=E.mA(pL,d6.Count>1 ? 100.0*c8/(d6.Count-1): 0.0);ct(pG,d6[c8],new Color(f7.a0,f7.aW,f7.aS),qc,qe+c8*oK,pz);}}bool m0(z ck,IMyTextSurface e6){
List<string>cX;if(bW.ContainsKey(ck)|| !r.cw.TryGetValue(ck.bn,out cX))return false;bW[ck]="banner";Vector2 bD=e6.SurfaceSize;
Vector2 g9=(e6.TextureSize-bD)/2f;int ee=1;foreach(var oA in cX)ee=Math.Max(ee,oA.Length);Vector2 c0=e6.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float eZ=Math.Min(bD.X*0.94f/(ee*c0.X),bD.Y*0.94f/(cX.Count*c0.Y));var hR=new List<MySprite>();hR.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=g9+bD/2f,Size=bD,Color=string.IsNullOrEmpty(ck.bm)? new Color(10,12,16): V(ck.bm)}
);ib(hR,cX,ck,g9.X+(bD.X-ee*c0.X*eZ)/2f,g9.Y+(bD.Y-cX.Count*c0.Y*eZ)/2f,eZ,c0.Y*eZ);using(var n8=e6.DrawFrame()){n8.AddRange(hR);
}return true;}void mq(z cl,double kY){IMyTextSurface dB;if(!bY.TryGetValue(cl,out dB))return;Vector2 bj=dB.SurfaceSize;
Vector2 ha=(dB.TextureSize-bj)/2f;var di=cl.my();string kN=cl.ih ?? "HELM TELEMETRY KERNEL ONLINE";bool g5=bj.Y>=200f;
List<string>b0;if(cl.bp==null|| !r.cw.TryGetValue(cl.bp,out b0))b0=mh;int b5=kN.Length+3;foreach(var oB in di)b5=Math.Max(b5,oB.Length+5);
if(g5)foreach(var oC in b0)b5=Math.Max(b5,oC.Length);int lf=(g5 ? b0.Count+1 : 0)+2+di.Count;Vector2 b4=dB.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bg=Math.Min(1f,Math.Min(bj.X*0.94f/(b5*b4.X),bj.Y*0.94f/(lf*b4.Y)));float fk=ha.X+(bj.X-b5*b4.X*bg)/2f;float bH=ha.Y+(bj.Y-lf*b4.Y*bg)/2f;
float eF=b4.Y*bg;var cn=new List<MySprite>();cn.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=ha+bj/2f,Size=bj,Color=new Color(10,12,16)}
);if(g5){ib(cn,b0,cl,fk,bH,bg,eF);bH+=(b0.Count+1)*eF;}ct(cn,">> "+kN,new Color(34,197,94),fk,bH,bg);bH+=eF*2f;int pC=E.mg(kY,di.Count);
for(int ev=0;ev<pC;ev++){bool kV=ev==di.Count-1&&kY>=0.8;ct(cn,kV ? "[OK]" : "[..]",kV ? new Color(34,197,94): new Color(56,189,248),fk,bH,bg);ct(cn,di[ev],new Color(203,213,225),fk+5f*b4.X*bg,bH,bg);
bH+=eF;}using(var n9=dB.DrawFrame()){n9.AddRange(cn);}}void fA(){bX.Clear();bY.Clear();d0.Clear();
cV.Clear();dY=false;if(r==null)return;foreach(var _ in r.bo){var ar=new List<IMyTerminalBlock>();if(_.aX==av.cF){
string gT= !string.IsNullOrEmpty(_.aL)? _.aL : _.N;if(!am.ContainsKey(gT)){am[gT]=_.dP;if(_.fq!=null)d_[gT]=_.fq;
}bX[_.N]=ar;continue;}var lL=_.aq.Count>0 ? _.aq : new List<string>{_.iR};foreach(var bF in lL){
if(string.IsNullOrEmpty(bF))continue;bool kt=bF.Contains("*")||bF.Contains("?");if(_.aX==av.fs||kt){var gC=new List<IMyTerminalBlock>();
mk(bF,gC);if(gC.Count>0){foreach(var kg in gC){if(!ar.Contains(kg))ar.Add(kg);}}else{var i9=dJ();
for(int fZ=0;fZ<i9.Count;fZ++){var fc=i9[fZ];if(kt ? E.mI(bF,fc.CustomName): string.Equals(bF,fc.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!ar.Contains(fc))ar.Add(fc);}}}}else{var f1=ir(bF);if(f1!=null&& !ar.Contains(f1))ar.Add(f1);}}if(ar.Count==0){
nb(_,lL,ar);if(ar.Count>0)d0[_.N]=true;}bX[_.N]=ar;}foreach(var aQ in r.O){
if(string.IsNullOrEmpty(aQ.a_))continue;var hd=ir(aQ.a_);if(hd==null){cV.AppendLine($"HELM: output block '{aQ.a_}' not found");
continue;}IMyTextSurface bE=null;var hu=hd as IMyTextSurfaceProvider;var jN=hd as IMyTextSurface;if(hu!=null&&aQ.br<hu.SurfaceCount){
bE=hu.GetSurface(aQ.br);}else if(jN!=null&&aQ.br==0){bE=jN;}if(bE==null){cV.AppendLine($"HELM: output block '{aQ.a_}' has no surface index {aQ.br}");
continue;}bE.ContentType=ContentType.SCRIPT;bE.Script="";bE.ScriptBackgroundColor=Color.Black;bY[aQ]=bE;}a5.Clear();
}static readonly string[]iq={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool mw(int os,IMyTerminalBlock o){switch(os){case 0: return o is IMyBatteryBlock;case 1: return o is IMyReactor;case 2: return o is IMySolarPanel;
case 3: return o is IMyWindTurbine;case 4: return o is IMyGasTank;case 5: return o is IMyAirVent;case 6: return o is IMyCargoContainer;case 7: return o is IMyJumpDrive;
case 8: return o is IMyThrust;case 9: return o is IMyMotorSuspension;case 10: return o is IMyDoor;case 11: return o is IMyShipConnector;case 12: return o is IMyAssembler;
case 13: return o is IMyRefinery;case 14: return o is IMyShipDrill;case 15: return o is IMyShipWelder;case 16: return o is IMyShipGrinder;case 17: return o is IMyGasGenerator;
case 18: return o is IMyLargeTurretBase;case 19: return o is IMyProjector;case 20: return o is IMyGyro;case 21: return o is IMyGravityGeneratorBase;case 22: return o is IMyLandingGear;
case 23: return o is IMyRadioAntenna;case 24: return o is IMyBeacon;case 25: return o is IMyLightingBlock;case 26: return o is IMyPistonBase;case 27: return o is IMyMotorStator;
case 28: return o is IMyWarhead;case 29: return o is IMySensorBlock;case 30: return o is IMyCameraBlock;case 31: return o is IMyParachute;case 32: return o is IMyTurretControlBlock;
}return false;}void nb(aJ nF,List<string>pS,List<IMyTerminalBlock>f2){string ol=(nF.N+" "+string.Join(" ",pS)).ToLowerInvariant();
var gW=new List<int>();for(int eB=0;eB<iq.Length;eB++){foreach(var p9 in iq[eB].Split(','))if(ol.Contains(p9)){gW.Add(eB);break;}}if(gW.Count==0)return;
var i8=dJ();var kW=new List<IMyTerminalBlock>();for(int gH=0;gH<i8.Count;gH++){var f_=i8[gH];foreach(int ot in gW){if(mw(ot,f_)){(f_.CubeGrid==Me.CubeGrid ? f2 : kW).Add(f_);
break;}}}if(f2.Count==0)f2.AddRange(kW);}void nd(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(r==null||r.O.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {r.O.Count} screens, {r.bo.Count} binds");dY=false;
Echo($"Blocks: {dJ().Count} on this construct; {fH} on docked ships (ignored)");a5.Clear();Echo("--- BIND RESOLUTION ---");foreach(var aa in r.bo){
if(aa.aX==av.cF){string gU= !string.IsNullOrEmpty(aa.aL)? aa.aL : aa.N;double p1=am.ContainsKey(gU)? am[gU]: aa.dP;
Echo($"[OK] @bind {aa.N} (dynamic variable '{gU}' = {p1})");continue;}List<IMyTerminalBlock>f3;if(bX.TryGetValue(aa.N,out f3)){if(f3.Count>0){
string n4=d0.ContainsKey(aa.N)&&d0[aa.N]? " [fallback: typed match]" : "";Echo($"[OK] @bind {aa.N} ({f3.Count} blocks matched{n4})");
}else{string pT=string.Join(", ",aa.aq.Count>0 ? aa.aq : new List<string>{aa.iR});Echo($"[WARN] @bind {aa.N} -> 0 blocks matched \"{pT}\"");
}}else{Echo($"[ERR] @bind {aa.N} -> unresolved");}}if(am.Count>0){Echo($"--- DYNAMIC VARIABLES ({am.Count}) ---");foreach(var kB in am)Echo($"[VAR] {kB.Key} = {kB.Value}");
}Echo("--- SURFACE OUTPUT AUDIT ---");foreach(var aR in r.O){IMyTextSurface lE;if(bY.TryGetValue(aR,out lE)){var lv=lE.SurfaceSize;
var pJ=E.mm(aR);Echo($"[OK] Screen \"{aR.aZ}\" [{aR.bs}] -> {aR.a_} [#{aR.br}] ({lv.X:0}x{lv.Y:0} px, {pJ.P} sprites)");
}else{Echo($"[ERR] Screen \"{aR.aZ}\" -> Missing block \"{aR.a_}\" [#{aR.br}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double nc(IMyTerminalBlock p){
if(p==null)return 0.0;var d8=p as IMyBatteryBlock;if(d8!=null)return d8.MaxStoredPower>0 ?(d8.CurrentStoredPower/d8.MaxStoredPower)*100.0 : 0.0;
var eA=p as IMyJumpDrive;if(eA!=null)return eA.MaxStoredPower>0 ?(eA.CurrentStoredPower/eA.MaxStoredPower)*100.0 : 0.0;var lK=p as IMyGasTank;
if(lK!=null)return lK.FilledRatio*100.0;var l4=p as IMyAirVent;if(l4!=null)return l4.GetOxygenLevel()*100.0;var jv=p as IMyCargoContainer;if(jv!=null){
var ey=jv.GetInventory();if(ey!=null&&ey.MaxVolume.RawValue>0)return(double)ey.CurrentVolume.RawValue/ey.MaxVolume.RawValue*100.0;return 0.0;}var eT=p as IMyReactor;
if(eT!=null)return eT.MaxOutput>0 ?(eT.CurrentOutput/eT.MaxOutput)*100.0 : 0.0;var e0=p as IMySolarPanel;if(e0!=null)return e0.MaxOutput>0 ?(e0.CurrentOutput/e0.MaxOutput)*100.0 : 0.0;
var fj=p as IMyWindTurbine;if(fj!=null)return fj.MaxOutput>0 ?(fj.CurrentOutput/fj.MaxOutput)*100.0 : 0.0;var jO=p as IMyDoor;if(jO!=null)return(double)jO.OpenRatio*100.0;
var ff=p as IMyThrust;if(ff!=null)return ff.MaxEffectiveThrust>0 ?(ff.CurrentThrust/ff.MaxEffectiveThrust)*100.0 : 0.0;var h_=p as IMyMotorSuspension;
if(h_!=null)return h_.Brake ? 100.0 :(h_.Friction*100.0);var gg=p as IMyShipConnector;if(gg!=null)return gg.Status==MyShipConnectorStatus.Connected ? 100.0 :(gg.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var la=p as IMyProductionBlock;if(la!=null)return la.IsProducing ? 100.0 : 0.0;var jS=p as IMyShipDrill;if(jS!=null)return jS.Enabled ? 100.0 : 0.0;
var lT=p as IMyShipToolBase;if(lT!=null)return lT.Enabled ? 100.0 : 0.0;var gz=p as IMyGasGenerator;if(gz!=null)return gz.AutoRefill ? 100.0 :(gz.Enabled ? 50.0 : 0.0);
var fg=p as IMyLargeTurretBase;if(fg!=null)return fg.IsShooting ? 100.0 :(fg.HasTarget ? 50.0 :(fg.Enabled ? 10.0 : 0.0));var du=p as IMyProjector;
if(du!=null)return du.TotalBlocks>0 ?((double)(du.TotalBlocks-du.RemainingBlocks)/du.TotalBlocks)*100.0 : 0.0;var gF=p as IMyGyro;if(gF!=null)return gF.GyroOverride ? 100.0 :(gF.Enabled ? 50.0 : 0.0);
var ki=p as IMyGravityGeneratorBase;if(ki!=null)return Math.Min(100.0,Math.Abs(ki.GravityAcceleration)/9.81*100.0);var gy=p as IMyLandingGear;if(gy!=null)return gy.IsLocked ? 100.0 :(gy.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var ja=p as IMyRadioAntenna;if(ja!=null)return(ja.Radius/50000.0)*100.0;var jl=p as IMyBeacon;if(jl!=null)return(jl.Radius/50000.0)*100.0;
var kI=p as IMyLightingBlock;if(kI!=null)return kI.Intensity*10.0;var eR=p as IMyPistonBase;if(eR!=null)return eR.HighestPosition>0 ?(eR.CurrentPosition/eR.HighestPosition)*100.0 : 0.0;
var ld=p as IMyMotorStator;if(ld!=null)return Math.Min(100.0,Math.Abs(ld.Angle)/6.28318*100.0);var l6=p as IMyWarhead;if(l6!=null)return l6.IsArmed ? 100.0 : 0.0;
var hN=p as IMySensorBlock;if(hN!=null)return!hN.LastDetectedEntity.IsEmpty()? 100.0 :(hN.Enabled ? 25.0 : 0.0);var ga=p as IMyCameraBlock;
if(ga!=null)return ga.IsActive ? 100.0 :(ga.Enabled ? 50.0 : 0.0);var jB=p as IMyParachute;if(jB!=null)return(double)jB.OpenRatio*100.0;
var fe=p as IMyTurretControlBlock;if(fe!=null)return fe.HasTarget ? 100.0 :(fe.IsUnderControl ? 50.0 :(fe.Enabled ? 10.0 : 0.0));var j7=p as IMyFunctionalBlock;
if(j7!=null)return j7.Enabled ? 100.0 : 0.0;return 0.0;}static double mF(IMyTerminalBlock jh,string gs){int ju=gs.IndexOf(':');string gV=gs.Substring(0,ju).ToLowerInvariant();
var p_=new MyItemType("MyObjectBuilder_"+(gV=="ammo" ? "AmmoMagazine" : char.ToUpper(gV[0])+gV.Substring(1)),gs.Substring(ju+1));double lD=0;for(int gI=0;
gI<jh.InventoryCount;gI++)lD+=(double)jh.GetInventory(gI).GetItemAmount(p_);return lD;}double iK(IMyTerminalBlock q,string j_){if(q==null)return 0.0;
string a=j_.ToLowerInvariant();if(a=="percent"||a=="pct")return nc(q);if(a.IndexOf(':')>0)return mF(q,j_);var a8=q as IMyBatteryBlock;
if(a8!=null){if(a=="current"||a=="stored"||a=="val"||a=="value"||a=="raw")return a8.CurrentStoredPower;if(a=="max"||a=="capacity")return a8.MaxStoredPower;
if(a=="output")return a8.CurrentOutput;if(a=="maxoutput")return a8.MaxOutput;if(a=="input")return a8.CurrentInput;if(a=="maxinput")return a8.MaxInput;
return a8.CurrentStoredPower;}var gS=q as IMyJumpDrive;if(gS!=null){if(a=="max"||a=="capacity")return gS.MaxStoredPower;return gS.CurrentStoredPower;
}var e9=q as IMyGasTank;if(e9!=null){if(a=="max"||a=="capacity")return e9.Capacity;return e9.FilledRatio*e9.Capacity;}var l5=q as IMyAirVent;
if(l5!=null){if(a=="max"||a=="capacity")return 100.0;return l5.GetOxygenLevel()*100.0;}var jw=q as IMyCargoContainer;if(jw!=null){var ez=jw.GetInventory();
if(ez!=null){if(a=="mass")return(double)ez.CurrentMass;if(a=="max"||a=="capacity")return(double)ez.MaxVolume;return(double)ez.CurrentVolume;}return 0.0;}var hv=q as IMyPowerProducer;
if(hv!=null){if(a=="max"||a=="capacity")return hv.MaxOutput;return hv.CurrentOutput;}var jP=q as IMyDoor;if(jP!=null){if(a=="max")return 1.0;return(double)jP.OpenRatio;
}var h3=q as IMyThrust;if(h3!=null){if(a=="max"||a=="capacity")return h3.MaxEffectiveThrust;return h3.CurrentThrust;}var h0=q as IMyMotorSuspension;
if(h0!=null){if(a=="brake")return h0.Brake ? 1.0 : 0.0;return h0.Friction*100.0;}var jH=q as IMyShipConnector;if(jH!=null){return jH.Status==MyShipConnectorStatus.Connected ? 1.0 : 0.0;
}var hs=q as IMyProductionBlock;if(hs!=null){if(a=="producing"||a=="active"||a=="running"||a=="state")return hs.IsProducing ? 1.0 : 0.0;if(a=="queue")return hs.IsQueueEmpty ? 0.0 : 1.0;
}var fV=q as IMyAssembler;if(fV!=null){if(a=="disassembling")return fV.Mode==MyAssemblerMode.Disassembly ? 1.0 : 0.0;if(a=="repeating"||a=="loop")return fV.Repeating ? 1.0 : 0.0;
}var gA=q as IMyGasGenerator;if(gA!=null){if(a=="autorefill")return gA.AutoRefill ? 1.0 : 0.0;if(a=="active")return gA.Enabled ? 1.0 : 0.0;}var fh=q as IMyLargeTurretBase;
if(fh!=null){if(a=="shooting")return fh.IsShooting ? 1.0 : 0.0;if(a=="target"||a=="hastarget")return fh.HasTarget ? 1.0 : 0.0;if(a=="range")return fh.Range;
}var ci=q as IMyProjector;if(ci!=null){if(a=="built")return ci.TotalBlocks-ci.RemainingBlocks;if(a=="remaining")return ci.RemainingBlocks;if(a=="total")return ci.TotalBlocks;
if(a=="projecting")return ci.IsProjecting ? 1.0 : 0.0;}var b9=q as IMyGyro;if(b9!=null){if(a=="override")return b9.GyroOverride ? 1.0 : 0.0;if(a=="pitch")return b9.Pitch;
if(a=="yaw")return b9.Yaw;if(a=="roll")return b9.Roll;if(a=="power")return b9.GyroPower*100.0;}var kj=q as IMyGravityGeneratorBase;if(kj!=null){if(a=="g"||a=="accel")return kj.GravityAcceleration;
}var kh=q as IMyLandingGear;if(kh!=null){if(a=="locked")return kh.IsLocked ? 1.0 : 0.0;}var jb=q as IMyRadioAntenna;if(jb!=null){if(a=="radius"||a=="range")return jb.Radius;
}var jm=q as IMyBeacon;if(jm!=null){if(a=="radius"||a=="range")return jm.Radius;}var g3=q as IMyLightingBlock;if(g3!=null){if(a=="intensity")return g3.Intensity;
if(a=="radius")return g3.Radius;}var eS=q as IMyPistonBase;if(eS!=null){if(a=="pos"||a=="position"||a=="extension")return eS.CurrentPosition;
if(a=="velocity")return eS.Velocity;if(a=="max")return eS.HighestPosition;}var dz=q as IMyMotorStator;if(dz!=null){if(a=="angle"||a=="deg")return dz.Angle*180.0/Math.PI;
if(a=="rad")return dz.Angle;if(a=="velocity"||a=="rpm")return dz.TargetVelocityRPM;if(a=="locked")return dz.RotorLock ? 1.0 : 0.0;}var ia=q as IMyWarhead;
if(ia!=null){if(a=="armed")return ia.IsArmed ? 1.0 : 0.0;if(a=="countdown")return ia.DetonationTime;}var ls=q as IMySensorBlock;if(ls!=null){
if(a=="detected")return!ls.LastDetectedEntity.IsEmpty()? 1.0 : 0.0;}var gb=q as IMyCameraBlock;if(gb!=null){if(a=="range")return gb.AvailableScanRange;
if(a=="active")return gb.IsActive ? 1.0 : 0.0;}var jC=q as IMyParachute;if(jC!=null){if(a=="open"||a=="openratio")return(double)jC.OpenRatio;}var cq=q as IMyTurretControlBlock;
if(cq!=null){if(a=="target"||a=="hastarget")return cq.HasTarget ? 1.0 : 0.0;if(a=="azimuth")return cq.AzimuthRotor!=null ? cq.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(a=="elevation")return cq.ElevationRotor!=null ? cq.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var j8=q as IMyFunctionalBlock;if(j8!=null){return j8.Enabled ? 1.0 : 0.0;
}return 0.0;}double fB(string cY,string j0){aJ a9;cS.TryGetValue(cY,out a9);if(a9!=null&&a9.aX==av.cF){
string ou= !string.IsNullOrEmpty(a9.aL)? a9.aL : cY;double jI;if(am.TryGetValue(ou,out jI))return jI;return a9.dP;
}double jU;if(am.TryGetValue(cY,out jU))return jU;List<IMyTerminalBlock>cZ;if(!bX.TryGetValue(cY,out cZ)||cZ.Count==0)throw new InvalidOperationException($"HELM: bind '{cY}' resolved to zero blocks — check the name/typo");
if(cZ.Count==1)return iK(cZ[0],j0);var l3=new List<double>();foreach(var nv in cZ)l3.Add(iK(nv,j0));return mc.mb(a9!=null ? a9.cu : ak.fl,l3);
}double fC(string nG){return fB(nG,"percent");}static string aV(double l0,bq ce){if(ce==null||(string.IsNullOrEmpty(ce.cR)&&string.IsNullOrEmpty(ce.cG)&&ce.G=="percent")){
return iw(l0);}return E.mx(l0,ce.cR,ce.cG);}bool m6(z cm){IMyTextSurface e7;if(!bY.TryGetValue(cm,out e7))return false;
if(cm.bn!=null)return m0(cm,e7);if(cm.cA.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return m7(cm,e7);
}return m8(cm,e7);}static void v(a2 nL,Vector2 o9,Vector2 pD,Color nN,float pp=0f,string nW="SquareSimple"){nL.l.Add(new MySprite{
Type=SpriteType.TEXTURE,Data=nW,Position=o9,Size=pD,Color=nN,RotationOrScale=pp});}void mj(z s,IMyTextSurface lF,a2 h){
h.l.Clear();h.dV.Clear();Vector2 R=lF.SurfaceSize;Vector2 pX=lF.TextureSize;Vector2 bG=(pX-R)/2f;
float eK=Math.Max(4f,R.X*0.02f);float dr=Math.Max(4f,R.Y*0.02f);if(s.bs=="compact"||(s.bs=="auto"&&R.Y<=260f&&R.Y>150f)){
eK=4f;dr=4f;}else if(s.bs=="corner"||(s.bs=="auto"&&R.Y<=150f)){eK=2f;dr=2f;}float lV=R.X-(eK*2f);
Color jp=new Color(10,12,16);bool jR=true;if(!string.IsNullOrEmpty(s.bm)){var c7=j.bR(s.bm);if(c7.cJ)jR=false;
else jp=new Color(c7.a0,c7.aW,c7.aS,c7.dI);}if(jR)v(h,bG+(R/2f),R-new Vector2(4f,4f),jp);if(!s.cA.Equals("none",StringComparison.OrdinalIgnoreCase)){
Color jr=s.aK!=null&&s.aK.Length>0 ? V(s.aK[0]): V(s.dN ?? "40, 60, 85");
Color nK=s.aK!=null&&s.aK.Length>1 ? V(s.aK[s.aK.Length-1]): jr;
v(h,new Vector2(bG.X+R.X/2f,bG.Y+dr*0.5f),new Vector2(lV,2f),jr);v(h,new Vector2(bG.X+R.X/2f,bG.Y+R.Y-(dr*0.5f)),new Vector2(lV,2f),nK);
}var hi=s.ay;if(hi.Count==0){hi=new List<al>{new al{cM="main",cC=0,cN=0,cO=s.bP,cP=s.bQ,aM=s.fv}
};}float kl=8f;foreach(var M in hi){var eJ=E.ml(M.cC,M.cN,M.cO,M.cP,s.bP,s.bQ,R.X,R.Y,eK,dr,kl,kl);
float aB=bG.X+eJ.iZ;float dq=bG.Y+eJ.i_;float aC=eJ.iY;float dp=eJ.iy;if(!string.IsNullOrEmpty(M.cv)){
var dn=j.bR(M.cv);if(!dn.cJ)v(h,new Vector2(aB+aC/2f,dq+dp/2f),new Vector2(aC,dp),new Color(dn.a0,dn.aW,dn.aS,dn.dI));
}else if(s.bP>1||s.bQ>1){v(h,new Vector2(aB+aC/2f,dq+dp/2f),new Vector2(aC,dp),new Color(14,18,24));}string gx=M.it ?? s.iu ?? "Monospace";
float nE=E.mz(s.bs,R.X,R.Y,s.bN);float at=M.iv ?? nE;float er=0f;
if(!string.IsNullOrEmpty(M.bu)){er=20f*at;var fQ=TextAlignment.LEFT;float h4=aB+4f;string i4=M.fD ?? s.iT ?? "left";
if(i4.Equals("center",StringComparison.OrdinalIgnoreCase)){fQ=TextAlignment.CENTER;h4=aB+(aC/2f);}else if(i4.Equals("right",StringComparison.OrdinalIgnoreCase)){
fQ=TextAlignment.RIGHT;h4=aB+aC-4f;}string lN=M.fE ?? s.iU;Color pY= !string.IsNullOrEmpty(lN)? V(lN): V(M.fo ?? s.dN ?? "white");
var lS=MySprite.CreateText(M.bu,gx,pY,at*0.9f,fQ);lS.Position=new Vector2(h4,dq+2f);h.l.Add(lS);
string[]f0=M.fp ?? s.aK;Color nY=f0!=null&&f0.Length>0 ? V(f0[0]): V(M.fo ?? s.dN ?? "40, 60, 85");
v(h,new Vector2(aB+aC/2f,dq+er-2f),new Vector2(aC-8f,1f),nY);}int oP=Math.Max(1,M.aM.Count);float o0=dp-er-8f;
float hz=o0/oP;float ex=6f;for(int ew=0;ew<M.aM.Count;ew++){var u=M.aM[ew];var J=new cL{iE=u};float pq=dq+er+4f+(ew*hz);
float C=pq+(hz/2f);float lP=C-(11f*at);J.ft=h.l.Count;J.ix=u.L!=null&&(u.L.Contains("{val}")||u.L.Contains("{percent}"));
var kE=MySprite.CreateText(u.L,gx,Color.White,at,TextAlignment.LEFT);kE.Position=new Vector2(aB+ex,lP);h.l.Add(kE);
float oI=Math.Max(24f*at,(u.L.Length+1)*12f*at);float p8=(!string.IsNullOrEmpty(u.cR)|| !string.IsNullOrEmpty(u.cG))? 64f*at : 52f*at;
if(u.e!=d.iS){float B=aB+ex+oI;float nD=aB+aC-ex-p8;float A=Math.Max(16f,nD-B);
float H=Math.Max(u.e==d.bt ? 8f : 4f,Math.Min((u.e==d.bt ? 20f : 16f)*at,hz*(u.e==d.bt ? 0.55f : 0.45f)));
J.cz=B;J.dK=A;J.bK=H;J.cy=C;if(u.e==d.cx){v(h,new Vector2(B+(A/2f),C),new Vector2(A,H),new Color(25,30,38));
J.S=h.l.Count;v(h,new Vector2(B,C),new Vector2(0f,H),V(u.ag));}else if(u.e==d.cK){
int hL=10;float ka=2f;float hM=Math.Max(2f,(A-(ka*(hL-1)))/hL);for(int hE=0;hE<hL;hE++){J.U.Add(h.l.Count);
v(h,new Vector2(B+hE*(hM+ka)+(hM/2f),C),new Vector2(hM,H),new Color(22,26,32));}}else if(u.e==d.bt){v(h,new Vector2(B+(A/2f),C),new Vector2(A,H),new Color(20,24,30));
int hr=8;float kb=2f;float ge=Math.Max(2f,(A-(kb*(hr-1)))/hr);for(int hg=0;hg<hr;hg++){J.U.Add(h.l.Count);
v(h,new Vector2(B+hg*(ge+kb)+(ge/2f),C),new Vector2(ge,2f),V(u.ag));}}else if(u.e==d.cI){v(h,new Vector2(B+(A/2f),C),new Vector2(A,H),new Color(20,24,30));
int hU=10;float kc=2f;float hV=Math.Max(2f,(A-(kc*(hU-1)))/hU);for(int hF=0;hF<hU;hF++){J.U.Add(h.l.Count);
v(h,new Vector2(B+hF*(hV+kc)+(hV/2f),C),new Vector2(hV,H),new Color(24,28,36),0.523599f);}}else if(u.e==d.cB){
int fT=10;float kd=2f;float fU=Math.Max(2f,(A-(kd*(fT-1)))/fT);for(int hG=0;hG<fT;hG++){J.U.Add(h.l.Count);
v(h,new Vector2(B+hG*(fU+kd)+(fU/2f),C),new Vector2(fU,H),new Color(24,28,36),1.570796f,"Triangle");}}else if(u.e==d.cE){
int gj=12;float ke=2f;float gl=Math.Max(2f,(A-(ke*(gj-1)))/gj);float jQ=Math.Min(gl,H);for(int hH=0;hH<gj;hH++){J.U.Add(h.l.Count);
v(h,new Vector2(B+hH*(gl+ke)+(gl/2f),C),new Vector2(jQ,jQ),new Color(24,28,36),0f,"Circle");}}else if(u.e==d.bJ){
v(h,new Vector2(B+(A/2f),C),new Vector2(A,H),new Color(25,30,38));J.S=h.l.Count;v(h,new Vector2(B+(A/2f),C),new Vector2(0f,H),V(u.ag));
v(h,new Vector2(B+(A/2f),C),new Vector2(2f,H+4f),new Color(80,110,150));}else if(u.e==d.bT){v(h,new Vector2(B+(A/2f),C),new Vector2(A,H),new Color(20,24,30));
J.S=h.l.Count;float gX=Math.Max(4f,H);v(h,new Vector2(B+gX*0.5f,C),new Vector2(gX,gX),new Color(40,50,65),0f,"Circle");
}else if(u.e==d.bL){float b1=Math.Max(6f,Math.Min(H,14f*at));v(h,new Vector2(B+b1*0.5f,C),new Vector2(b1,b1),new Color(28,34,46));
J.S=h.l.Count;v(h,new Vector2(B+b1*0.5f,C),new Vector2(b1*0.6f,b1*0.6f),new Color(40,50,65));}else if(u.e==d.bS){
v(h,new Vector2(B+(A/2f),C),new Vector2(A,H),new Color(18,22,30));J.S=h.l.Count;float hp=Math.Max(4f,H*0.7f);
v(h,new Vector2(B+hp+2f,C),new Vector2(hp,hp),new Color(34,197,94),0f,"Circle");}else if(u.e==d.bI){int ho=8;
float kf=2f;float hq=Math.Max(2f,(A-(kf*(ho-1)))/ho);for(int hh=0;hh<ho;hh++){J.U.Add(h.l.Count);v(h,new Vector2(B+hh*(hq+kf)+(hq/2f),C),new Vector2(hq,H),new Color(24,28,36));
}}else if(u.e==d.bM){v(h,new Vector2(B+(A/2f),C),new Vector2(A,H),new Color(20,24,30));J.S=h.l.Count;
float kq=Math.Max(4f,H);v(h,new Vector2(B+(A/2f),C),new Vector2(kq,kq),V(u.ag),0f,"Triangle");}}var lZ=MySprite.CreateText("---",gx,Color.White,at,TextAlignment.RIGHT);
lZ.Position=new Vector2(aB+aC-ex,lP);J.a4=h.l.Count;h.l.Add(lZ);h.dV.Add(J);}}h.iC=R;
h.iB=true;}static void az(a2 js,cL lw,string pW,Color nO){var h8=js.l[lw.a4];h8.Data=pW;h8.Color=nO;js.l[lw.a4]=h8;
}static void iN(a2 jt,cL lx,Color nP){var jY=jt.l[lx.S];jY.Color=nP;jt.l[lx.S]=jY;}bool m8(z hK,IMyTextSurface hX){
a2 n;if(!d2.TryGetValue(hK,out n)){n=new a2();d2[hK]=n;}if(!n.iB||n.iC!=hX.SurfaceSize){
mj(hK,hX,n);}bool I=false;bool i7=(bZ%16)<8;for(int gJ=0;gJ<n.dV.Count;gJ++){var b=n.dV[gJ];var k=b.iE;
double x;bool kn=false;try{x=fB(k.aT,k.G);}catch{kn=true;x=0.0;}if(kn){if(b.dT!= -99999.0||(b.a4>=0&&n.l[b.a4].Data!="ERR")){
I=true;b.dT= -99999.0;if(b.a4>=0){var hI=n.l[b.a4];hI.Data="ERR";hI.Color=Color.Red;n.l[b.a4]=hI;
}}continue;}double aD=x;if(k.aY>0)x=Math.Min(100.0,aD/k.aY*100.0);string nR=k.cH ? E.dR(k.bO,x): k.ag;
Color T=V(nR);bool aO=(T==Color.Red);if(aO&&i7){T=new Color(255,75,75);I=true;}bool au=Math.Abs(aD-b.dT)>0.1;
if(au)b.dT=aD;if(k.e==d.cx){double j1=(k.aY>0||k.G=="percent"||k.G=="pct")? x : fC(k.aT);
if(b.K<0f)b.K=(float)j1;float fa=(float)j1;if(Math.Abs(b.K-fa)>0.05f){b.K+=(fa-b.K)*0.35f;
I=true;}else if(b.K!=fa){b.K=fa;I=true;}if(au)I=true;if(I){float o2=(float)Math.Max(0.0,Math.Min(1.0,b.K/100.0));
float j3=b.dK*o2;var el=n.l[b.S];el.Size=new Vector2(j3,b.bK);el.Position=new Vector2(b.cz+(j3/2f),b.cy);
el.Color=T;n.l[b.S]=el;az(n,b,aV(aD,k),T);}}else if(k.e==d.cK||k.e==d.cI||k.e==d.cB||k.e==d.cE){
if(au||aO){I=true;double n5=(k.aY>0||k.G=="percent"||k.G=="pct")? x : fC(k.aT);int lq=b.U.Count;
for(int dA=0;dA<lq;dA++){bool oR=n5>=((dA+0.4)/(double)lq*100.0);Color pA=oR ? T : new Color(24,28,36);var lr=n.l[b.U[dA]];
lr.Color=pA;n.l[b.U[dA]]=lr;}az(n,b,aV(aD,k),T);}}else if(k.e==d.bt){
if(au||aO){I=true;for(int eq=0;eq<7;eq++)b.dW[eq]=b.dW[eq+1];b.dW[7]=(float)x;int o8=b.U.Count;
for(int dk=0;dk<o8;dk++){float ok=b.dW[dk];float oj=Math.Max(0.06f,Math.Min(1f,ok/100f));float jE=b.bK*oj;float nQ=b.cy+(b.bK/2f)-(jE/2f);
var b6=n.l[b.U[dk]];b6.Size=new Vector2(b6.Size.Value.X,jE);b6.Position=new Vector2(b6.Position.Value.X,nQ);
b6.Color=T;n.l[b.U[dk]]=b6;}az(n,b,aV(aD,k),T);}}else if(k.e==d.bJ){
if(b.K<0f)b.K=(float)x;float fb=(float)x;if(Math.Abs(b.K-fb)>0.05f){b.K+=(fb-b.K)*0.35f;
I=true;}else if(b.K!=fb){b.K=fb;I=true;}if(I||au){float km=b.dK*0.5f;float jz=b.cz+km;
float jM=(float)Math.Max(-1.0,Math.Min(1.0,(b.K-50.0)/50.0));float gv=Math.Abs(jM)*km;float n6=(jM>=0)?(jz+gv*0.5f):(jz-gv*0.5f);
var em=n.l[b.S];em.Size=new Vector2(gv,b.bK);em.Position=new Vector2(n6,b.cy);em.Color=T;
n.l[b.S]=em;az(n,b,aV(aD,k),T);}}else if(k.e==d.bT){if(au||aO){
I=true;bool gR=x>=50.0;float ky=Math.Max(4f,b.bK);float ox=gR ?(b.cz+b.dK-ky*0.5f):(b.cz+ky*0.5f);
Color kx=gR ?(T!=Color.White ? T : new Color(34,197,94)): new Color(60,70,85);var gt=n.l[b.S];gt.Position=new Vector2(ox,b.cy);
gt.Color=kx;n.l[b.S]=gt;az(n,b,gR ? "ON" : "OFF",kx);}}else if(k.e==d.bL){if(au||aO){
I=true;bool gQ=x>=50.0;Color jA=gQ ?(T!=Color.White ? T : new Color(56,189,248)): new Color(30,36,48);iN(n,b,jA);
az(n,b,gQ ? "ACTIVE" : "READY",gQ ? jA : new Color(148,163,184));}}else if(k.e==d.bS){if(au||aO){
I=true;string pK=x>=75.0 ? "ONLINE" :(x>=25.0 ? "STBY" : "OFFLINE");Color lB=x>=75.0 ? new Color(34,197,94):(x>=25.0 ? new Color(234,179,8): new Color(239,68,68));
iN(n,b,lB);az(n,b,pK,lB);}}else if(k.e==d.bI){if(au||aO){I=true;int k6=b.U.Count;
for(int dl=0;dl<k6;dl++){bool oS=x>=((dl+0.5)/(double)k6*100.0);Color o6=oS ? T : new Color(24,28,36);var k7=n.l[b.U[dl]];
k7.Color=o6;n.l[b.U[dl]]=k7;}az(n,b,aV(aD,k),T);}}else if(k.e==d.bM){
I=true;float eW;Color c6;string en;bool oo=(k.G=="producing"||k.G=="active"||k.G=="running"||k.G=="state"||(x<=1.0&&k.G!="percent"));
if(oo){if(x>0.0){b.K=(b.K+0.3927f)%6.28318f;eW=b.K;c6=new Color(34,197,94);en="RUN";}else{eW=0f;
c6=new Color(148,163,184);en="IDLE";}}else{eW=(x>55.0)? 0f :((x<45.0)? 3.14159f : 1.57079f);c6=(x>55.0)? new Color(34,197,94):((x<45.0)? new Color(239,68,68): new Color(148,163,184));
en=(x>55.0)? "CHG" :((x<45.0)? "DSC" : "STBY");}var gu=n.l[b.S];gu.RotationOrScale=eW;gu.Color=c6;
n.l[b.S]=gu;az(n,b,en,c6);}else{string ej;aJ fd;cS.TryGetValue(k.aT,out fd);d_.TryGetValue(fd!=null&&fd.aL!=null ? fd.aL : k.aT,out ej);
if(au||aO||ej!=b.iD){b.iD=ej;I=true;Color h5=k.aU!=null ? V(E.iL(k.aU,x)): Color.White;
if(h5==Color.Red&&i7)h5=new Color(255,75,75);az(n,b,ej ?? aV(aD,k),h5);}}if(b.ix&&(au||aO)){
I=true;var kG=n.l[b.ft];string o3=iw(x);string pm=aV(aD,k);kG.Data=k.L.Replace("{percent}",o3).Replace("{val}",pm);
n.l[b.ft]=kG;}}if(I){using(var oa=hX.DrawFrame()){oa.AddRange(n.l);}}return I;}bool m7(z bh,IMyTextSurface hY){
hY.FontSize=bh.bN;var ef=new List<KeyValuePair<string,string>>();foreach(var i in bh.fv){double D=0.0;string aH;string aA=null;
try{D=fB(i.aT,i.G);}catch(Exception qf){ef.Add(new KeyValuePair<string,string>($"{i.L}: [UNRESOLVED]","red"));
continue;}double lb=D;if(i.aY>0)D=Math.Min(100.0,lb/i.aY*100.0);string l_=aV(lb,i);if(i.e==d.cx){double j2=(i.aY>0||i.G=="percent"||i.G=="pct")? D : fC(i.aT);
aH=E.m1(i.L,j2,10,l_);aA=i.cH ? E.dR(i.bO,j2): i.ag;}else if(i.e==d.bJ){
aH=E.m_(i.L,D,10);aA=i.cH ? E.dR(i.bO,D): i.ag;}else if(i.e==d.bT){
aH=E.na(i.L,D);aA=i.ag ??(D>=50.0 ? "green" : "gray");}else if(i.e==d.bL){aH=E.m4(i.L,D);
aA=i.ag ??(D>=50.0 ? "cyan" : "gray");}else if(i.e==d.bS){aH=E.m9(i.L,D);aA=i.ag ??(D>=75.0 ? "lime" :(D>=25.0 ? "yellow" : "red"));
}else if(i.e==d.bI){aH=E.mZ(i.L,D,8);aA=i.cH ? E.dR(i.bO,D):(i.ag ?? "yellow");
}else if(i.e==d.bM){bool op=(i.G=="producing"||i.G=="active"||i.G=="running"||i.G=="state"||(D<=1.0&&i.G!="percent"));
if(op){char[]pI=new[]{'|','/','-','\\'};char px=pI[Math.Abs(bZ)%4];aH=D>0 ? $"{i.L} [{px}] RUN" : $"{i.L} [==] IDLE";
aA=D>0 ? "green" : "gray";}else{aH=E.m5(i.L,D,bZ);aA=i.ag ??(D>55.0 ? "green" :(D<45.0 ? "red" : "cyan"));
}}else{aH=$"{i.L}: {l_}";aA=i.aU!=null ? E.iL(i.aU,D): null;}ef.Add(new KeyValuePair<string,string>(aH,aA));
}var ec=E.m3(new List<string>(ef.ConvertAll(pR=>pR.Key)),cs: bh.iX);string lt=string.Join("\n",ec);
string k9;if(bW.TryGetValue(bh,out k9)&&k9==lt)return false;bW[bh]=lt;using(var ob=hY.DrawFrame()){
float l9=4f;float kJ=ma*bh.bN;try{var kL=hY.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bh.bN);
if(kL.Y>0)kJ=kL.Y;}catch{}for(int ca=0;ca<ec.Count;ca++){string jG=null;if(ca>0&&ca<ec.Count-1)jG=ef[ca-1].Value;
var lA=MySprite.CreateText(ec[ca],"Monospace",V(jG),bh.bN,TextAlignment.LEFT);lA.Position=new Vector2(4f,l9);ob.Add(lA);
l9+=kJ;}}return true;}Color V(string eH){if(string.IsNullOrEmpty(eH))return Color.White;Color df;if(i0.TryGetValue(eH,out df))return df;
j c_=j.bR(eH);df=c_.cJ ? Color.Transparent : new Color(c_.a0,c_.aW,c_.aS,c_.dI);i0[eH]=df;return df;}public enum av{me,fs,cF}
public enum ak{mJ,fl,iQ,iG,iF}public class aJ{public string N;public av aX;public List<string>aq=new List<string>();public string iR{
get{return aq.Count>0 ? aq[0]: "";}set{if(aq.Count==0)aq.Add(value);else aq[0]=value;}}public ak cu=ak.mJ;
public double dP=0.0;public string fq;public string aL;}public enum d{iS,cx,cK,bt,cI,cB,cE,bJ,bT,bL,bS,bI,bM}
public class cD{public string ip;public string dO;public bool dS;}public class bq{public d e;public string L;public string aT;
public string G;public bool cH;public string[]bO;public string ag;public List<cD>aU;public string il="right";
public string cR;public string cG;public double aY;}public struct j{public byte a0,aW,aS,dI;public bool cJ;public j(byte pe,byte od,byte nw,byte nj=255){
a0=pe;aW=od;aS=nw;dI=nj;cJ=false;}public static readonly j nh=new j(0,0,0,0){cJ=true};public static readonly j dX=new j(255,255,255);
public static readonly j md=new j(0,0,0);public static readonly j mW=new j(255,0,0);public static readonly j mC=new j(0,255,0);
public static readonly j mG=new j(50,205,50);public static readonly j ni=new j(255,255,0);public static readonly j mK=new j(255,140,0);
public static readonly j mo=new j(0,255,255);public static readonly j mf=new j(0,120,255);public static readonly j mH=new j(255,0,255);
public static readonly j mU=new j(128,0,128);public static readonly j mB=new j(128,128,128);public static readonly j mp=new j(60,60,60);
public static j bR(string aG){if(string.IsNullOrWhiteSpace(aG))return dX;aG=aG.Trim();if(aG.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aG.Equals("none",StringComparison.OrdinalIgnoreCase))return nh;
if(aG.StartsWith("#")){string bc=aG.Substring(1).Trim();if(bc.Length==6){byte pf=Convert.ToByte(bc.Substring(0,2),16);byte oe=Convert.ToByte(bc.Substring(2,2),16);
byte nx=Convert.ToByte(bc.Substring(4,2),16);return new j(pf,oe,nx);}if(bc.Length==3){byte pg=Convert.ToByte(new string(bc[0],2),16);byte of=Convert.ToByte(new string(bc[1],2),16);
byte ny=Convert.ToByte(new string(bc[2],2),16);return new j(pg,of,ny);}}if(aG.Contains(",")){var ch=aG.Split(',');if(ch.Length>=3){byte ph=(byte)Math.Max(0,Math.Min(255,int.Parse(ch[0].Trim())));
byte og=(byte)Math.Max(0,Math.Min(255,int.Parse(ch[1].Trim())));byte nz=(byte)Math.Max(0,Math.Min(255,int.Parse(ch[2].Trim())));byte nk=ch.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(ch[3].Trim()))):(byte)255;
return new j(ph,og,nz,nk);}}switch(aG.ToLowerInvariant()){case "red": return mW;case "green": return mC;case "lime": return mG;case "yellow": return ni;
case "orange": return mK;case "cyan": return mo;case "blue": return mf;case "magenta": return mH;case "purple": return mU;case "white": return dX;
case "black": return md;case "gray": case "grey": return mB;case "darkgray": case "darkgrey": return mp;default: return dX;}}}public class al{
public string cM="default";public int cC=0;public int cN=0;public int cO=1;public int cP=1;public string bu;public string fD="left";public string fE;
public string ij;public string it;public float? iv;public string cv;public string fo;public string[]fp;public string iH="horizontal";
public List<bq>aM=new List<bq>();}public class z{public string aZ;public string bs="standard";public string cA="graphic";public string dN;
public string[]aK;public string iT="left";public string iU;public string iu="Monospace";public string bm;public string ih;
public int ii=10;public int ie=1;public string bn;public string bp;public float fn=2.5f;public string ig="cyan";
public string[]dL;public List<string>dM=new List<string>();public List<string>my(){if(dM!=null&&dM.Count>0)return dM;
return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float bN=1.0f;public int mX=100;public int bP=1;public int bQ=1;public int iX=30;public string a_;public int br=0;
public List<al>ay=new List<al>();public List<bq>fv{get{if(ay.Count==0)ay.Add(new al{cM="main"});return ay[0].aM;}
}}public class a3{public string fw;public List<string>dU=new List<string>();public Dictionary<string,string>fr=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>fm=new List<string>();}public class fy{public List<aJ>bo=new List<aJ>();public List<z>O=new List<z>();
public List<string>ax=new List<string>();public Dictionary<string,List<string>>cw=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class iz{static readonly Dictionary<string,a3>ik=new Dictionary<string,a3>(StringComparer.OrdinalIgnoreCase);
static iz(){mY();}static void mY(){a1("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");a1("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");a1("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");a1("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");a1("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");a1("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");a1("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");a1("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void a1(string kR,string[]o1,string nH){var e8=new a3{fw=kR}
;foreach(var oX in o1){var gY=oX.Split(new[]{'='},2);string kZ=fx(gY[0]);e8.dU.Add(kZ);if(gY.Length>1)e8.fr[kZ]=W(gY[1].Trim());
}var oQ=nH.Replace("\r\n","\n").Split('\n');foreach(var oD in oQ){string lh=oD.Trim();if(lh.Length>0)e8.fm.Add(lh);}ik[kR]=e8;}static string fx(string cf){
cf=cf.Trim();if(cf.StartsWith("$"))cf=cf.Substring(1);return cf.ToLowerInvariant();}public static fy mM(string nU){var ac=new fy();z f=null;
al t=null;var k8=mS(nU,ac.ax,ac.cw);for(int c9=0;c9<k8.Count;c9++){string c=k8[c9].Trim();
if(c.Length==0)continue;try{if(c.StartsWith("@bind")){ac.bo.Add(mN(c));}else if(c.StartsWith("@screen")){f=new z{aZ=c.Substring("@screen".Length).Trim()}
;ac.O.Add(f);t=null;}else if(c.StartsWith("profile:")){y(f,"profile:");f.bs=c.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(c.StartsWith("border:")){y(f,"border:");string ji=c.Substring("border:".Length).Trim();if(t!=null)t.ij=ji;
else f.cA=ji;}else if(c.StartsWith("border_color:")){y(f,"border_color:");string jk=c.Substring("border_color:".Length).Trim();
if(t!=null)t.fo=jk;else f.dN=jk;}else if(c.StartsWith("border_gradient:")||c.StartsWith("border_gradient(")){
y(f,"border_gradient");string bw=c.StartsWith("border_gradient:")? c.Substring("border_gradient:".Length): c.Substring("border_gradient".Length);
bw=bw.Trim();if(bw.StartsWith("(")&&bw.Contains(")"))bw=ao(bw,0);string[]lC=ap(bw).Select(pr=>pr.Trim()).ToArray();if(t!=null)t.fp=lC;
else f.aK=lC;}else if(c.StartsWith("title_align:")){y(f,"title_align:");string lJ=c.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(t!=null)t.fD=lJ;else f.iT=lJ;}else if(c.StartsWith("title_color:")){y(f,"title_color:");string lM=c.Substring("title_color:".Length).Trim();
if(t!=null)t.fE=lM;else f.iU=lM;}else if(c.StartsWith("font_size:")){y(f,"font_size:");float lH=float.Parse(c.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(t!=null)t.iv=lH;else f.bN=lH;}else if(c.StartsWith("font:")){y(f,"font:");string jZ=c.Substring("font:".Length).Trim();
if(t!=null)t.it=jZ;else f.iu=jZ;}else if(c.StartsWith("bg:")||c.StartsWith("background:")){y(f,"bg:");string jo=c.Substring(c.IndexOf(':')+1).Trim();
if(t!=null)t.cv=jo;else f.bm=jo;}else if(c.StartsWith("layout:")){y(f,"layout:");mO(c.Substring("layout:".Length).Trim(),f);
}else if(c.StartsWith("refresh:")){y(f,"refresh:");f.mX=int.Parse(c.Substring("refresh:".Length).Trim());}else if(c.StartsWith("output:")){
y(f,"output:");var eN=ap(c.Substring("output:".Length).Trim());if(eN.Count==0)throw new FormatException("output: requires a target block name");
f.a_=W(eN[0]);f.br=eN.Count>1 ? int.Parse(eN[1]): 0;}else if(c.StartsWith("width:")){y(f,"width:");
f.iX=int.Parse(c.Substring("width:".Length).Trim());}else if(c.StartsWith("boot_msg:")||c.StartsWith("boot:")){y(f,"boot_msg:");
f.ih=W(c.Substring(c.IndexOf(':')+1).Trim());}else if(c.StartsWith("boot_stagger:")){y(f,"boot_stagger:");f.ii=int.Parse(c.Substring("boot_stagger:".Length).Trim());
}else if(c.StartsWith("boot_batch:")){y(f,"boot_batch:");f.ie=Math.Max(1,int.Parse(c.Substring("boot_batch:".Length).Trim()));
}else if(c.StartsWith("boot_duration:")){y(f,"boot_duration:");string c1=c.Substring("boot_duration:".Length).Trim().ToLowerInvariant();
bool kp=c1.EndsWith("s");if(kp)c1=c1.Substring(0,c1.Length-1).Trim();float gm;if(float.TryParse(c1,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out gm)){
f.fn=kp ? gm : gm/60f;}}else if(c.StartsWith("boot_log:")){y(f,"boot_log:");f.dM.Add(W(c.Substring("boot_log:".Length).Trim()));
}else if(c.StartsWith("banner:")){y(f,"banner:");f.bn=W(c.Substring("banner:".Length).Trim());}else if(c.StartsWith("boot_logo:")){
y(f,"boot_logo:");f.bp=W(c.Substring("boot_logo:".Length).Trim());}else if(c.StartsWith("boot_gradient:")||c.StartsWith("boot_gradient(")||c.StartsWith("banner_gradient:")||c.StartsWith("banner_gradient(")){
y(f,"boot_gradient");int gB=c.IndexOfAny(new[]{':','('});string bz=c.Substring(c[gB]==':' ? gB+1 : gB);bz=bz.Trim();if(bz.StartsWith("(")&&bz.Contains(")"))bz=ao(bz,0);
f.dL=ap(bz).Select(ps=>ps.Trim()).ToArray();}else if(c.StartsWith("boot_color:")||c.StartsWith("banner_color:")){y(f,"boot_color:");
f.ig=c.Substring(c.IndexOf(':')+1).Trim();}else if(c.StartsWith("orientation:")){y(f,"orientation:");string oV=c.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(t!=null)t.iH=oV;}else if(c.StartsWith("@panel")){y(f,"@panel");t=mQ(c);f.ay.Add(t);
}else if(c.StartsWith("title:")){y(f,"title:");if(t==null){t=new al{cM="main",cC=0,cN=0,cO=f.bP,cP=f.bQ}
;f.ay.Add(t);}t.bu=W(c.Substring("title:".Length).Trim());}else if(c.StartsWith("line:")){y(f,"line:");
if(t==null){t=new al{cM="main",cC=0,cN=0,cO=f.bP,cP=f.bQ};f.ay.Add(t);}t.aM.Add(mP(c.Substring("line:".Length).Trim()));
}else{ac.ax.Add($"Line {c9 + 1}: unrecognized directive: \"{c}\"");}}catch(Exception n0){ac.ax.Add($"Line {c9 + 1}: parse error ({n0.Message}) in \"{c}\"");
}}foreach(var li in ac.O){if(string.IsNullOrEmpty(li.a_))ac.ax.Add($"@screen {li.aZ}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aF in ac.O){if(aF.bn!=null&& !ac.cw.ContainsKey(aF.bn))ac.ax.Add("Screen '"+aF.aZ+"': banner '"+aF.bn+"' is not defined (@banner "+aF.bn+" ... @end)");
if(aF.bp!=null&& !ac.cw.ContainsKey(aF.bp))ac.ax.Add("Screen '"+aF.aZ+"': boot_logo '"+aF.bp+"' is not defined (@banner "+aF.bp+" ... @end)");
}return ac;}static void mO(string f4,z lp){int hj=f4.IndexOf('(');if(hj<0)throw new FormatException("missing '(' in layout: specification");
string kw=f4.Substring(0,hj).Trim().ToLowerInvariant();string nn=ao(f4,hj);var fS=ap(nn);if(kw=="grid"){if(fS.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
lp.bP=Math.Max(1,int.Parse(fS[0]));lp.bQ=Math.Max(1,int.Parse(fS[1]));}else{throw new FormatException($"unknown layout kind '{kw}' (expected grid)");
}}static al mQ(string oL){string dw=oL.Substring("@panel".Length).Trim();int d7=dw.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(d7<0)throw new FormatException("@panel requires an at(...) positioning clause");
string oT=dw.Substring(0,d7).Trim();string jf=dw.Substring(d7);int k4=jf.IndexOf('(');string jd=ao(jf,k4);
var bv=ap(jd);if(bv.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var aP=new al{cM=oT,cC=int.Parse(bv[0]),cN=int.Parse(bv[1]),cO=bv.Count>2 ? int.Parse(bv[2]): 1,cP=bv.Count>3 ? int.Parse(bv[3]): 1}
;int i6=d7+k4+jd.Length+1;if(i6<dw.Length){string an=dw.Substring(i6).Trim();int lR=an.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(lR>=0){string dC=an.Substring(lR+5).Trim();if(dC.StartsWith("(")&&dC.Contains(")"))aP.bu=W(ao(dC,0));else if(dC.StartsWith(":")){
string bx=dC.Substring(1).Trim();if(bx.StartsWith("\"")){int jV=bx.IndexOf('"',1);aP.bu=jV>0 ? bx.Substring(1,jV-1): W(bx);}else{int ly=bx.IndexOf(' ');
aP.bu=ly>=0 ? bx.Substring(0,ly): bx;}}}string kU=dQ(an,"orientation:");if(kU!=null)aP.iH=kU.ToLowerInvariant();string i5=dQ(an,"title_align:");
if(i5!=null)aP.fD=i5.ToLowerInvariant();string lO=dQ(an,"title_color:");if(lO!=null)aP.fE=lO;int eb=an.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(eb>=0&& !an.Substring(eb).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !an.Substring(eb).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
aP.ij=dQ(an.Substring(eb),"border:");}int d9=an.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(d9<0)d9=an.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(d9>=0){string ba=an.Substring(d9+"border_gradient".Length).Trim();if(ba.StartsWith(":"))ba=ba.Substring(1).Trim();if(ba.StartsWith("(")&&ba.Contains(")"))ba=ao(ba,0);
aP.fp=ap(ba).Select(pt=>pt.Trim()).ToArray();}}return aP;}static string dQ(string lj,string kv){int ko=lj.IndexOf(kv,StringComparison.OrdinalIgnoreCase);
if(ko<0)return null;string h9=lj.Substring(ko+kv.Length).Trim();int lz=h9.IndexOf(' ');return lz>=0 ? h9.Substring(0,lz): h9;}static List<string>mS(string nV,List<string>gq,Dictionary<string,List<string>>nB){
var lW=new Dictionary<string,a3>(StringComparer.OrdinalIgnoreCase);var cj=(nV ?? "").Replace("\r\n","\n").Split('\n');var he=new List<string>();
int cb=0;while(cb<cj.Length){string hw=cj[cb++];if(hw.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){var Z=new List<string>();
while(cb<cj.Length){string i3=cj[cb++].Replace("\t","    ").TrimEnd();if(i3.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;Z.Add(i3);}while(Z.Count>0&&Z[0].Length==0)Z.RemoveAt(0);
while(Z.Count>0&&Z[Z.Count-1].Length==0)Z.RemoveAt(Z.Count-1);int gP=int.MaxValue;foreach(var fM in Z)if(fM.Length>0)gP=Math.Min(gP,fM.Length-fM.TrimStart().Length);
for(int dg=0;dg<Z.Count;dg++)if(Z[dg].Length>0)Z[dg]=Z[dg].Substring(gP);nB[hw.TrimStart().Substring(8).Trim()]=Z;continue;}string be=iP(hw).Trim();
if(be.Length==0)continue;if(be.StartsWith("@template")){try{var h1=mR(be);while(cb<cj.Length){string nJ=cj[cb++];string f5=iP(nJ).Trim();
if(f5.Equals("@end",StringComparison.OrdinalIgnoreCase))break;if(f5.Length>0)h1.fm.Add(f5);}lW[h1.fw]=h1;}catch(Exception n1){
gq.Add($"Template parse error ({n1.Message}) in \"{be}\"");}}else if(be.StartsWith("@use")){try{io(be,lW,he,gq);}
catch(Exception n2){gq.Add($"@use error ({n2.Message}) in \"{be}\"");}}else{he.Add(be);}}return he;}static a3 mR(string oM){
string hx=oM.Substring("@template".Length).Trim();int hk=hx.IndexOf('(');if(hk<0)throw new FormatException("missing '(' in @template header");string oU=hx.Substring(0,hk).Trim();
string no=ao(hx,hk);var h2=new a3{fw=oU};foreach(var oY in ap(no)){var gZ=oY.Split(new[]{'='},2);string k_=fx(gZ[0]);
h2.dU.Add(k_);if(gZ.Length>1)h2.fr[k_]=W(gZ[1].Trim());}return h2;}static void io(string oN,Dictionary<string,a3>lX,List<string>kX,List<string>n_,int jL=0){
string eV=oN.Substring("@use".Length).Trim();int eL=eV.IndexOf('(');string g7=eL>=0 ? eV.Substring(0,eL).Trim(): eV.Trim();string np=eL>=0 ? ao(eV,eL): "";
a3 cp=null;if(!lX.TryGetValue(g7,out cp)){if(!ik.TryGetValue(g7,out cp))throw new FormatException($"unknown template '{g7}'");
}var ed=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);foreach(var kA in cp.fr)ed[kA.Key]=kA.Value;var jc=ap(np);for(int cW=0;
cW<jc.Count;cW++){string d5=jc[cW];int go=d5.IndexOf('=');if(go>=0){string ov=fx(d5.Substring(0,go));string p2=W(d5.Substring(go+1).Trim());
ed[ov]=p2;}else{if(cW<cp.dU.Count){string ow=cp.dU[cW];ed[ow]=W(d5.Trim());}}}foreach(var nI in cp.fm){string c4=nI;
foreach(var eD in ed){c4=c4.Replace("$"+eD.Key,eD.Value).Replace("{"+eD.Key+"}",eD.Value);}if(c4.StartsWith("@use")){if(jL>3)throw new FormatException("@use nested too deep");
io(c4,lX,kX,n_,jL+1);}else kX.Add(c4);}}static void y(z nT,string nX){
if(nT==null)throw new FormatException($"'{nX}' used before any @screen declared");}static string iP(string aE){if(string.IsNullOrEmpty(aE))return aE;
bool gO=false;for(int bd=0;bd<aE.Length;bd++){char f8=aE[bd];if(f8=='"'){gO= !gO;}else if(!gO){if(f8=='/'&&bd+1<aE.Length&&aE[bd+1]=='/'){return aE.Substring(0,bd);
}if(f8=='#'){if(mE(aE,bd)){continue;}return aE.Substring(0,bd);}}}return aE;}static bool mE(string bA,int kr){int cd=0;for(int gK=kr+1;gK<bA.Length;
gK++){char b2=bA[gK];if((b2>='0'&&b2<='9')||(b2>='a'&&b2<='f')||(b2>='A'&&b2<='F'))cd++;else break;}if(cd==3||cd==4||cd==6||cd==8){int dj=kr+1+cd;if(dj>=bA.Length||char.IsWhiteSpace(bA[dj])||bA[dj]==')'||bA[dj]==','||bA[dj]==':')return true;
}return false;}static aJ mN(string oO){string hy=oO.Substring("@bind".Length).Trim();int gp=hy.IndexOf('=');if(gp<0)throw new FormatException("missing '=' in @bind");
string kS=hy.Substring(0,gp).Trim();string gr=hy.Substring(gp+1).Trim();int hl=gr.IndexOf('(');if(hl<0)throw new FormatException("missing '(' in @bind target");
string ep=gr.Substring(0,hl).Trim().ToLowerInvariant();string nq=ao(gr,hl);var a7=ap(nq);if(a7.Count==0)throw new FormatException("@bind target has no arguments");
var ad=new aJ{N=kS};if(ep=="dynamic"||ep=="var"||ep=="mem"){ad.aX=av.cF;ad.aL=a7.Count>0 ? W(a7[0].Trim()): kS;
for(int fN=0;fN<a7.Count;fN++){var eE=a7[fN].Split(new[]{':'},2);if(eE.Length==2&&eE[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double jK;
if(double.TryParse(eE[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out jK))ad.dP=jK;else ad.fq=W(eE[1].Trim());
}}return ad;}ad.aX=ep=="group" ? av.fs : av.me;string d4=null;for(int d3=0;d3<a7.Count;d3++){var g_=a7[d3].Split(new[]{':'},2);if(g_.Length==2&&g_[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
d4=g_[1].Trim().ToLowerInvariant();}else{ad.aq.Add(W(a7[d3].Trim()));}}if(ad.aq.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ad.aX==av.fs){if(d4==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(d4){case "avg": ad.cu=ak.fl;break;case "sum": ad.cu=ak.iQ;break;case "min": ad.cu=ak.iG;break;case "max": ad.cu=ak.iF;
break;default: throw new FormatException($"unknown agg '{d4}' (expected avg/sum/min/max)");}}return ad;}static bq mP(string ea){var m=new bq();
if(!ea.StartsWith("\""))throw new FormatException("line must start with a quoted label");int gn=ea.IndexOf('"',1);if(gn<0)throw new FormatException("unterminated quoted label");
m.L=ea.Substring(1,gn-1);string dv=ea.Substring(gn+1).Trim();int eM=dv.IndexOf('(');if(eM<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string w=dv.Substring(0,eM).Trim().ToLowerInvariant();string nm=ao(dv,eM);string Q=dv.Substring(dv.IndexOf(')',eM)+1).Trim();
var gk=nm.Trim().Split('.');if(gk.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
m.aT=gk[0].Trim();m.G=gk[1].Trim();if(w=="bar")m.e=d.cx;else if(w=="led")m.e=d.cK;else if(w=="spark"||w=="sparkline")m.e=d.bt;
else if(w=="hazard"||w=="stripes")m.e=d.cI;else if(w=="chevron"||w=="chevrons")m.e=d.cB;else if(w=="dots"||w=="dot")m.e=d.cE;
else if(w=="balance"||w=="horizon"||w=="centered")m.e=d.bJ;else if(w=="toggle"||w=="switch")m.e=d.bT;else if(w=="checkbox"||w=="check")m.e=d.bL;
else if(w=="status"||w=="badge"||w=="state")m.e=d.bS;else if(w=="ammo"||w=="pips")m.e=d.bI;else if(w=="flow"||w=="rate"||w=="spinner")m.e=d.bM;
else if(w=="text")m.e=d.iS;else throw new FormatException($"unknown display function '{w}'");while(Q.Length>0){int gi=Q.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int b8=Q.IndexOf('(');if(gi>=0&&(b8<0||gi<b8)){string eg=Q.Substring(gi+4).Trim();int e1=eg.IndexOf(' ');m.il=(e1>=0 ? eg.Substring(0,e1): eg).ToLowerInvariant();
Q=(e1>=0 ? eg.Substring(e1): "").Trim();continue;}int h7=Q.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(h7>=0&&(b8<0||h7<b8)){
string fi=Q.Substring(h7+5).Trim();int e2=fi.IndexOf(' ');m.cR=(e2>=0 ? fi.Substring(0,e2): fi).Trim().Trim('"');Q=(e2>=0 ? fi.Substring(e2): "").Trim();
continue;}int gw=Q.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(gw>=0&&(b8<0||gw<b8)){string ek=Q.Substring(gw+7).Trim();
int e3=ek.IndexOf(' ');m.cG=(e3>=0 ? ek.Substring(0,e3): ek).Trim().Trim('"');Q=(e3>=0 ? ek.Substring(e3): "").Trim();continue;}int eG=Q.IndexOf('(');
if(eG<0)break;string ae=Q.Substring(0,eG).Trim().ToLowerInvariant();int kF=ae.LastIndexOf(' ');if(kF>=0)ae=ae.Substring(kF+1).Trim();
string bf=ao(Q,eG);Q=Q.Substring(Q.IndexOf(')',eG)+1).Trim();if(ae=="gradient"){m.cH=true;m.bO=ap(bf).Select(pu=>pu.Trim()).ToArray();
if(m.bO.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(ae=="flat"){m.ag=bf.Trim();}else if(ae=="pattern"){
string F=bf.Trim().ToLowerInvariant();if(F=="hazard"||F=="stripes")m.e=d.cI;else if(F=="chevron"||F=="chevrons")m.e=d.cB;
else if(F=="dots"||F=="dot")m.e=d.cE;else if(F=="led")m.e=d.cK;else if(F=="balance"||F=="horizon")m.e=d.bJ;
else if(F=="toggle"||F=="switch")m.e=d.bT;else if(F=="checkbox"||F=="check")m.e=d.bL;else if(F=="status"||F=="badge"||F=="state")m.e=d.bS;
else if(F=="ammo"||F=="pips")m.e=d.bI;else if(F=="flow"||F=="rate"||F=="spinner")m.e=d.bM;}else if(ae=="color"){m.aU=new List<cD>();
foreach(var hm in ap(bf)){int gf=hm.LastIndexOf(':');if(gf<0)throw new FormatException("color() entries need 'expr : color'");string kH=hm.Substring(0,gf).Trim();
string jF=hm.Substring(gf+1).Trim();if(kH.Equals("default",StringComparison.OrdinalIgnoreCase)){m.aU.Add(new cD{dS=true,dO=jF}
);}else{string pM=ne(kH);m.aU.Add(new cD{ip=pM,dO=jF});}}if(!m.aU.Any(pi=>pi.dS))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
}else if(ae=="dir"||ae=="direction"){m.il=bf.Trim().ToLowerInvariant();}else if(ae=="unit"){m.cR=bf.Trim().Trim('"');}else if(ae=="max"){
m.aY=double.Parse(bf.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture);}else if(ae=="format"||ae=="fmt"){
m.cG=bf.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{ae}'");}}return m;}static string ao(string eY,int hc){
int gh=0;for(int da=hc;da<eY.Length;da++){if(eY[da]=='(')gh++;else if(eY[da]==')'){gh--;if(gh==0)return eY.Substring(hc+1,da-hc-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>ap(string nr){return nr.Split(',').Select(pv=>pv.Trim()).Where(pw=>pw.Length>0).ToList();
}static string ne(string b7){b7=b7.Trim();foreach(var oW in new[]{">=","<=","==",">","<"}){int gL=b7.IndexOf(oW,StringComparison.Ordinal);
if(gL>0)return b7.Substring(gL).Trim();if(gL==0)return b7;}throw new FormatException($"color() rule has no recognizable comparison operator: '{b7}'");}
static string W(string bB){if(bB.Length>=2&&bB[0]=='"'&&bB[bB.Length-1]=='"')return bB.Substring(1,bB.Length-2);return bB;}}public static class mc{public static double mb(ak nl,List<double>cr){
if(cr==null||cr.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(nl){case ak.fl: return cr.Average();case ak.iQ: return cr.Sum();
case ak.iG: return cr.Min();case ak.iF: return cr.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class E{
public static string m1(string oF,double eO,int jj=10,string l2=null){eO=Math.Max(0,Math.Min(100,eO));int j4=(int)Math.Round(jj*(eO/100.0));
string nC=new string('#',j4)+new string('-',jj-j4);string p3=l2!=null ? l2 : $"{eO:0}%";return $"{oF} [{nC}] {p3}";
}public static string mx(double bl,string h6=null,string eo=null){if(h6=="auto"){double fO=Math.Abs(bl);if(fO>=1e9)return(bl/1e9).ToString("F1")+"G";
if(fO>=1e6)return(bl/1e6).ToString("F1")+"M";if(fO>=1e3)return(bl/1e3).ToString("F1")+"k";string n3= !string.IsNullOrEmpty(eo)? eo : "F0";return bl.ToString(n3);
}string j6= !string.IsNullOrEmpty(eo)? eo :(Math.Abs(bl)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(h6)){return bl.ToString(j6);}return bl.ToString(j6)+h6;
}public static float mz(string pa,float lG,float hZ,float eh=1.0f){string cg=(pa ?? "standard").ToLowerInvariant();
if(cg=="compact"||(cg=="auto"&&lG<=300&&hZ<=300&&hZ>160)){return eh*0.75f;}if(cg=="corner"||(cg=="auto"&&hZ<=160)){
return eh*0.55f;}if(cg=="wide"||(cg=="auto"&&lG>=1000)){return eh*1.15f;}return eh;}public static string m_(string oG,double eP,int bb=5){
eP=Math.Max(0,Math.Min(100,eP));double ei=(eP-50.0)/50.0;int dh=0;int dy=0;if(ei< -0.01)dh=(int)Math.Round(-ei*bb);
else if(ei>0.01)dy=(int)Math.Round(ei*bb);if(dh>bb)dh=bb;if(dy>bb)dy=bb;string oJ=new string('-',bb-dh)+new string('<',dh);
string po=new string('>',dy)+new string('-',bb-dy);return $"{oG} [{oJ}|{po}] {eP:0}%";}public static string na(string kC,double p5){
return p5>=50.0 ? $"{kC} [====O] ON" : $"{kC} [O====] OFF";}public static string m4(string kD,double p6){return p6>=50.0 ? $"{kD} [X] ACTIVE" : $"{kD} [ ] READY ";
}public static string m9(string g1,double l1){if(l1>=75.0)return $"{g1} [ ONLINE  ]";if(l1>=25.0)return $"{g1} [ STANDBY ]";return $"{g1} [ OFFLINE ]";
}public static string mZ(string oH,double eQ,int kK=8){eQ=Math.Max(0,Math.Min(100,eQ));int j5=(int)Math.Round(kK*(eQ/100.0));
string o7=new string('|',j5)+new string('.',kK-j5);return $"{oH} [{o7}] {eQ:0}%";}public static string m5(string g2,double dt,int lQ=0){
char[]pH=new[]{'|','/','-','\\'};char lk=pH[(lQ>=0 ? lQ : 0)%4];if(dt>55.0)return $"{g2} {lk} ▲ CHG {dt:0}%";if(dt<45.0)return $"{g2} {lk} ▼ DSC {dt:0}%";
return $"{g2} ● STBY {dt:0}%";}public static bool mI(string af,string dd){if(af==null||dd==null)return false;if(af=="*"||af==dd)return true;
if(!af.Contains("*")&& !af.Contains("?"))return string.Equals(af,dd,StringComparison.OrdinalIgnoreCase);int aj=0,db=0,hS= -1,g6=0;while(db<dd.Length){
if(aj<af.Length&&(af[aj]=='?'||char.ToLowerInvariant(af[aj])==char.ToLowerInvariant(dd[db]))){aj++;db++;}else if(aj<af.Length&&af[aj]=='*'){
hS=aj;g6=db;aj++;}else if(hS!= -1){aj=hS+1;g6++;db=g6;}else{return false;}}while(aj<af.Length&&af[aj]=='*')aj++;return aj==af.Length;
}public static int mg(double oZ,int lU){return Math.Max(0,Math.Min(lU,1+(int)(Math.Max(0.0,oZ)*lU)));}public static string dR(string[]co,double hn){
if(co==null||co.Length==0)return "white";hn=Math.Max(0,Math.Min(100,hn));int gM=(int)Math.Floor(hn/100.0*co.Length);if(gM>=co.Length)gM=co.Length-1;
return co[gM];}public static j mA(string[]bk,double o4){if(bk==null||bk.Length==0)return j.dX;if(bk.Length==1)return j.bR(bk[0]);
double pl=Math.Max(0.0,Math.Min(1.0,o4/100.0));double lo=pl*(bk.Length-1);int gN=(int)Math.Floor(lo);int om=Math.Min(bk.Length-1,gN+1);
double hW=lo-gN;j b3=j.bR(bk[gN]);j f9=j.bR(bk[om]);byte pj=(byte)Math.Round(b3.a0+(f9.a0-b3.a0)*hW);
byte oh=(byte)Math.Round(b3.aW+(f9.aW-b3.aW)*hW);byte nA=(byte)Math.Round(b3.aS+(f9.aS-b3.aS)*hW);return new j(pj,oh,nA);}public static string iL(List<cD>lg,double p7){
foreach(var hA in lg){if(hA.dS)continue;if(mv(hA.ip,p7))return hA.dO;}var jJ=lg.FirstOrDefault(pk=>pk.dS);return jJ!=null ? jJ.dO : null;
}static bool mv(string c5,double dH){c5=c5.Trim();foreach(var hb in new[]{">=","<=","==",">","<"}){if(c5.StartsWith(hb)){double dx=double.Parse(c5.Substring(hb.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(hb){case ">=": return dH>=dx;case "<=": return dH<=dx;case "==": return Math.Abs(dH-dx)<1e-9;case ">": return dH>dx;case "<": return dH<dx;
}}}throw new FormatException($"unsupported comparison expression: '{c5}'");}public static List<string>m3(List<string>nS,int cs){
var eI=new List<string>();eI.Add("+"+new string('-',cs+2)+"+");foreach(var g4 in nS){string pZ=g4.Length>cs ? g4.Substring(0,cs): g4;
string o_=pZ.PadRight(cs);eI.Add("| "+o_+" |");}eI.Add("+"+new string('-',cs+2)+"+");return eI;}public struct iJ{public float iZ,i_,iY,iy;
}public static iJ ml(int jD,int le,int e4,int e5,int dE,int dF,float pQ,float pP,float k0,float k1,float gD,float gE){
dE=Math.Max(1,dE);dF=Math.Max(1,dF);e4=Math.Max(1,Math.Min(dE-jD,e4));e5=Math.Max(1,Math.Min(dF-le,e5));
float nt=pQ-(k0*2f);float ns=pP-(k1*2f);float jy=(nt-(gD*(dE-1)))/dE;float jx=(ns-(gE*(dF-1)))/dF;
float pc=k0+jD*(jy+gD);float pd=k1+le*(jx+gE);float pb=e4*jy+((e4-1)*gD);float o5=e5*jx+((e5-1)*gE);return new iJ{
iZ=pc,i_=pd,iY=pb,iy=o5};}public struct iM{public int mL;public int fu;public int P;public int ah;public int cQ;
public int aw;public int nf;public int ng;public int ms;public int mt;
public double mu;}public static iM mm(z bi){var g=new iM();var k3=bi.ay!=null&&bi.ay.Count>0 ? bi.ay : new List<al>{
new al{aM=bi.fv}};g.mL=k3.Count;g.fu=0;g.P=0;g.ah=0;if(!string.IsNullOrEmpty(bi.bm)&& !bi.bm.Equals("none",StringComparison.OrdinalIgnoreCase))g.P++;
if(!string.IsNullOrEmpty(bi.cA)&& !bi.cA.Equals("none",StringComparison.OrdinalIgnoreCase))g.P+=2;foreach(var dm in k3){
if(!string.IsNullOrEmpty(dm.cv)&& !dm.cv.Equals("none",StringComparison.OrdinalIgnoreCase))g.P++;if(!string.IsNullOrEmpty(dm.bu))g.P+=2;
g.fu+=dm.aM.Count;foreach(var oE in dm.aM){g.nf++;g.ng++;g.P+=2;switch(oE.e){case d.cx: g.cQ++;
g.aw++;g.P+=2;g.ah++;break;case d.cK: g.aw+=10;g.P+=10;
g.ah+=10;break;case d.cI: g.aw+=10;g.P+=11;g.ah+=10;break;case d.cB: g.aw+=10;
g.P+=10;g.ah+=10;break;case d.cE: g.aw+=12;g.P+=12;g.ah+=12;
break;case d.bt: g.cQ++;g.aw+=8;g.P+=9;g.ah+=8;break;case d.bJ: g.cQ+=2;
g.aw++;g.P+=3;g.ah++;break;case d.bT: case d.bL: g.cQ++;
g.aw++;g.P+=2;g.ah++;break;case d.bS: case d.bM: g.cQ++;g.aw+=2;
g.P+=3;g.ah+=2;break;case d.bI: g.aw+=8;g.P+=8;g.ah+=8;
break;default: g.ah++;break;}}}g.ms=25+(g.fu*2);g.mt=180+(g.ah*4);
g.mu=0.0;return g;}}