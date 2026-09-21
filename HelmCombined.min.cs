gx v;string gJ=null;Dictionary<string,List<IMyTerminalBlock>>ci=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>eC=new Dictionary<string,bool>();
Dictionary<string,aR>df=new Dictionary<string,aR>();Dictionary<string,Color>kk=new Dictionary<string,Color>();List<IMyTerminalBlock>bM=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ch=new List<IMyTerminalBlock>();bool ez;int gH;string eA="";StringBuilder di=new StringBuilder();readonly Dictionary<string,double>au=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>eB=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener dg;bool bN=false;
Dictionary<C,IMyTextSurface>bO=new Dictionary<C,IMyTextSurface>();Dictionary<C,string>cg=new Dictionary<C,string>();
static readonly string[]jZ=new string[101];static Program(){for(int e5=0;e5<=100;e5++){jZ[e5]=e5+"%";}}static string jL(double rO){int dG=(int)Math.Round(rO);
if(dG<0)dG=0;else if(dG>100)dG=100;return jZ[dG];}class aU{public List<MySprite>k=new List<MySprite>();public List<c7>ew=new List<c7>();
public int bC= -1;public Color[]ef;public Color ea;public bool eb;public Vector2 jT;public bool jR=false;public List<eg>cY=new List<eg>();
}class eg{public int ep;public Color f5,ek;public List<aw>ev,en;public string f9,gh,gi,gk;public bool jS,gm;
}class c7{public bG jV;public int aa= -1;public int be= -1;public int gq= -1;public bool jN=false;
public List<int>ad=new List<int>();public float[]ex=new float[8];public float cW;public float ee;public float b3;
public float cV;public double er= -99999.0;public string jU;public float Q= -1f;}Dictionary<C,aU>eE=new Dictionary<C,aU>();
int km=0;double gL=0;double aW=0;int dh=0;Dictionary<C,double>aV=new Dictionary<C,double>();Dictionary<C,double>gG=new Dictionary<C,double>();
int dj=0;const string oa="0.11.0";IMyTextSurface bf;bool gF=true;double gE=0;string kl="";string gK="";
int eD=0,gI=0;List<KeyValuePair<string,int>>cf=new List<KeyValuePair<string,int>>();const double ou=120;const float nI=22f;
public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{dg=IGC.RegisterBroadcastListener("HELM");dg.SetMessageCallback("HELM_IGC");
}catch{}gz();j6();jQ();}public void Main(string ky,UpdateType rK){double li=Runtime.TimeSinceLastRun.TotalSeconds;aW+=li;
if((rK&UpdateType.IGC)!=0||ky=="HELM_IGC"){os();}string mI=(ky ?? "").Trim();string ao=mI.ToLowerInvariant();if(ao=="reload"){
gz();}else if(ao=="refresh"){gA();}else if(ao=="boot"){j6();}else if(ao=="console on"||ao=="console off"){gF=ao=="console on";
jQ();gE=0;}else if(ao=="scan"||ao=="diagnose"||ao=="test"){oM();return;}else if(ao.StartsWith("set ")){string fz=mI.Substring(4).Trim();
int lm=fz.IndexOf('=');int rn=fz.IndexOf(' ');int i0=lm>0 ? lm : rn;if(i0>0){string lZ=fz.Substring(0,i0).Trim();string nt=fz.Substring(i0+1).Trim();jv($"{lZ}={nt}");
Echo($"[HELM] Set dynamic variable '{lZ}' = {nt}");}return;}else if(ao.StartsWith("alert")){string d7=ao.Length>5 ? ao.Substring(5).Trim(): "toggle";if(d7=="toggle")bN= !bN;
else bN=d7=="red"||d7=="on"||d7=="1"||d7=="true";Echo($"[HELM] Red alert state: {bN}");return;}else if(Me.CustomData!=gJ){
gz();}dj++;gL+=li;if(gL>=ou){gL=0;gA();}if(v==null||v.V.Count==0){
kf();jE();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var bW=v.V[km%v.V.Count];
km++;gK=bW.a9;bool gP=aV.Count>0;if(gP)oB();double lj;if(!aV.ContainsKey(bW)&& !(gG.TryGetValue(bW,out lj)&&aW<lj)){
if(oF(bW))gP=true;if(bW.gy>0)gG[bW]=aW+bW.gy/60.0;}dh=gP ? 0 : dh+1;Runtime.UpdateFrequency=dh<=v.V.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
kf();jE();eD=Runtime.CurrentInstructionCount;if(eD>gI)gI=eD;}void jE(){if(eA.Length>0)Echo(eA);
if(di.Length>0)Echo(di.ToString());}List<IMyTerminalBlock>ec(bool oU=false){if(!ez){ch.Clear();GridTerminalSystem.GetBlocks(ch);
bM.Clear();bM.AddRange(ch);bM.RemoveAll(o5=> !o5.IsSameConstructAs(Me));gH=ch.Count-bM.Count;ez=true;
}return oU ? ch : bM;}IMyTerminalBlock jH(string mj,bool oW=false){var gX=GridTerminalSystem.GetBlockWithName(mj);if(gX==null||oW||gX.IsSameConstructAs(Me))return gX;
var gR=ec();for(int e6=0;e6<gR.Count;e6++)if(gR[e6].CustomName.IndexOf(mj,StringComparison.OrdinalIgnoreCase)>=0)return gR[e6];return null;}void nS(string mk,List<IMyTerminalBlock>dI,bool kr=false){
var lD=GridTerminalSystem.GetBlockGroupWithName(mk);if(lD==null)return;int kH=dI.Count;var d3=new List<IMyTerminalBlock>();lD.GetBlocks(d3);jt(d3,dI,kr);
if(dI.Count>kH)return;var lO=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(lO);foreach(var nD in lO){if(nD.Name.IndexOf(mk,StringComparison.OrdinalIgnoreCase)<0)continue;
d3.Clear();nD.GetBlocks(d3);jt(d3,dI,kr);if(dI.Count>kH)return;}}void jt(List<IMyTerminalBlock>pT,List<IMyTerminalBlock>lX,bool oV=false){
foreach(var gY in pT)if((oV||gY.IsSameConstructAs(Me))&& !lX.Contains(gY))lX.Add(gY);}void gz(){gJ=Me.CustomData;v=jP.oj(gJ);
eA="";if(v.aG.Count>0){var mS=new StringBuilder("HELM config errors:\n");foreach(var pE in v.aG)mS.AppendLine(" - "+pE);eA=mS.ToString();
}df.Clear();foreach(var gZ in v.bD)if(!df.ContainsKey(gZ.U))df[gZ.U]=gZ;eE.Clear();cg.Clear();aV.Clear();
gG.Clear();gA();}void os(){try{while(dg!=null&&dg.HasPendingMessage){var mf=dg.AcceptMessage();
if(mf.Data is string){jv((string)mf.Data);}}}catch{}}void jv(string mA){if(string.IsNullOrEmpty(mA))return;var mx=mA.Split(';');
for(int hO=0;hO<mx.Length;hO++){var dT=mx[hO].Trim();if(dT.Length==0)continue;int dv=dT.IndexOf('=');if(dv<0)dv=dT.IndexOf(':');if(dv>0){string cx=dT.Substring(0,dv).Trim();
string aQ=dT.Substring(dv+1).Trim();if(cx.Equals("alert",StringComparison.OrdinalIgnoreCase)){bN=aQ.Equals("red",StringComparison.OrdinalIgnoreCase)||aQ=="1"||aQ.Equals("true",StringComparison.OrdinalIgnoreCase)||aQ.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double mn;eB.Remove(cx);if(double.TryParse(aQ,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mn)){
au[cx]=mn;}else if(aQ.Equals("true",StringComparison.OrdinalIgnoreCase)||aQ.Equals("on",StringComparison.OrdinalIgnoreCase)){au[cx]=100.0;
}else if(aQ.Equals("false",StringComparison.OrdinalIgnoreCase)||aQ.Equals("off",StringComparison.OrdinalIgnoreCase)){au[cx]=0.0;}else{eB[cx]=aQ;
}}}}}void ah(int rw,string rD){cf.Add(new KeyValuePair<string,int>(rD,rw));}static string ke(string iQ,int mi){return iQ.Length>mi ? iQ.Substring(0,mi): iQ;
}void jQ(){var iI=Me as IMyTextSurfaceProvider;if(iI==null||iI.SurfaceCount<1)return;bf=iI.GetSurface(0);bf.ContentType=gF ? ContentType.SCRIPT : ContentType.NONE;
bf.Script="";}void kf(){if(aW>=gE){gE=aW+(dh>v.V.Count+1 ? 5.0 : 1.0);nQ();if(gF&&bf!=null)nZ();
}Echo(kl);}void nQ(){cf.Clear();int g8=0,kN=0;foreach(var kA in v.bD){List<IMyTerminalBlock>h8;if(kA.a8==aE.c3){
g8++;continue;}if(ci.TryGetValue(kA.U,out h8)){kN+=h8.Count;if(h8.Count>0)g8++;}}int ln=v.aG.Count;ah(0,"HELM-[Kernel-(v"+oa+")]");
ah(4,"------------------------------");ah(1,"krn-cfg");ah(2," screens "+v.V.Count+"  binds "+g8+"/"+v.bD.Count+"  errors "+ln);
ah(2," alert "+(bN ? "RED" : "off")+"  blocks "+kN+"  docked-ignored "+gH);if(ln>0)ah(3," ! "+ke(v.aG[0],40));
ah(1,"cfg-dsp");int rk=0;foreach(var iR in v.V){if(++rk>10){ah(2," +"+(v.V.Count-10)+" more");break;}IMyTextSurface i1;string m0="--",i6="no block";
if(bO.TryGetValue(iR,out i1)){m0=i1.SurfaceSize.X.ToString("0")+"x"+i1.SurfaceSize.Y.ToString("0");i6=aV.ContainsKey(iR)? "boot" : "live";
}ah(i6=="no block" ? 3 : 2," "+ke(iR.a9,14).PadRight(14)+" "+m0.PadRight(9)+" "+i6);}ah(1,"perf");ah(2," tick "+dj+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ah(2," instr "+eD+"  peak "+gI+"  max "+Runtime.MaxInstructionCount);ah(2," rate "+Runtime.UpdateFrequency+"  idle "+dh+"  up "+(int)aW+"s");
if(gK.Length>0)ah(2," last "+gK);var mT=new StringBuilder();foreach(var qd in cf)mT.AppendLine(qd.Key);kl=mT.ToString();}static Color nV(int rx){
switch(rx){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void nZ(){Vector2 bX=bf.SurfaceSize;Vector2 il=(bf.TextureSize-bX)/2f;int hf=30;foreach(var qe in cf)hf=Math.Max(hf,qe.Key.Length);
Vector2 he=bf.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float mU=Math.Min(1f,Math.Min(bX.X*0.94f/(hf*he.X),bX.Y*0.94f/(cf.Count*he.Y)));
float rU=il.X+bX.X*0.03f;float nF=il.Y+bX.Y*0.03f;var i2=new List<MySprite>();i2.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=il+bX/2f,Size=bX,Color=new Color(12,12,16)}
);foreach(var l3 in cf){cQ(i2,l3.Key,nV(l3.Value),rU,nF,mU);nF+=he.Y*mU;}using(var pO=bf.DrawFrame()){pO.AddRange(i2);}}static readonly List<string>nP=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void j6(){aV.Clear();if(v==null)return;for(int e7=0;e7<v.V.Count;e7++){var fD=v.V[e7];if(fD.gb>0f)aV[fD]=aW+(e7/Math.Max(1,fD.jx))*fD.jA/60.0;
}}void oB(){List<C>du=null;int pZ=(aV.Count+3)/4,p6=0;foreach(var ff in aV){double iu=(aW-ff.Value)/ff.Key.gb;if(iu<0)continue;
if(iu>=1.0){if(du==null)du=new List<C>();du.Add(ff.Key);continue;}if((p6++ +dj)%pZ==0)nY(ff.Key,iu);}if(du==null)return;foreach(var iS in du){
aV.Remove(iS);eE.Remove(iS);cg.Remove(iS);}}static void cQ(List<MySprite>ro,string rE,Color pq,float rV,float rX,float rh){
var nf=MySprite.CreateText(rE,"Monospace",pq,rh,TextAlignment.LEFT);nf.Position=new Vector2(rV,rX);ro.Add(nf);}static void js(List<MySprite>rp,List<string>eJ,C fG,float rW,float rY,float ri,float qp){
string[]ru=fG.eh!=null&&fG.eh.Length>0 ? fG.eh : new[]{fG.jy};for(int dB=0;dB<eJ.Count;
dB++){n g9=E.jM(ru,eJ.Count>1 ? 100.0*dB/(eJ.Count-1): 0.0);cQ(rp,eJ[dB],new Color(g9.bb,g9.a6,g9.a2),rW,rY+dB*qp,ri);}}bool oz(C cF,IMyTextSurface fN){
List<string>dl;if(cg.ContainsKey(cF)|| !v.cT.TryGetValue(cF.bA,out dl))return false;cg[cF]="banner";Vector2 bY=fN.SurfaceSize;
Vector2 im=(fN.TextureSize-bY)/2f;int eS=1;foreach(var qf in dl)eS=Math.Max(eS,qf.Length);Vector2 ds=fN.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float fF=Math.Min(bY.X*0.94f/(eS*ds.X),bY.Y*0.94f/(dl.Count*ds.Y));var i3=new List<MySprite>();i3.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=im+bY/2f,Size=bY,Color=string.IsNullOrEmpty(cF.bz)? new Color(10,12,16): D(cF.bz)}
);js(i3,dl,cF,im.X+(bY.X-eS*ds.X*fF)/2f,im.Y+(bY.Y-dl.Count*ds.Y*fF)/2f,fF,ds.Y*fF);using(var pP=fN.DrawFrame()){pP.AddRange(i3);
}return true;}void nY(C cG,double ms){IMyTextSurface d1;if(!bO.TryGetValue(cG,out d1))return;Vector2 bu=d1.SurfaceSize;
Vector2 io=(d1.TextureSize-bu)/2f;var dM=cG.n5();string mg=cG.jz ?? "HELM TELEMETRY KERNEL ONLINE";bool ie=bu.Y>=200f;
List<string>cj;if(cG.bE==null|| !v.cT.TryGetValue(cG.bE,out cj))cj=nP;int cq=mg.Length+3;foreach(var qg in dM)cq=Math.Max(cq,qg.Length+5);
if(ie)foreach(var qh in cj)cq=Math.Max(cq,qh.Length);int mL=(ie ? cj.Count+1 : 0)+2+dM.Count;Vector2 cp=d1.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float br=Math.Min(1f,Math.Min(bu.X*0.94f/(cq*cp.X),bu.Y*0.94f/(mL*cp.Y)));float f1=io.X+(bu.X-cq*cp.X*br)/2f;float b0=io.Y+(bu.Y-mL*cp.Y*br)/2f;
float fj=cp.Y*br;var cJ=new List<MySprite>();cJ.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=io+bu/2f,Size=bu,Color=new Color(10,12,16)}
);if(ie){js(cJ,cj,cG,f1,b0,br,fj);b0+=(cj.Count+1)*fj;}cQ(cJ,">> "+mg,new Color(34,197,94),f1,b0,br);b0+=fj*2f;int rl=E.nO(ms,dM.Count);
for(int e8=0;e8<rl;e8++){bool mp=e8==dM.Count-1&&ms>=0.8;cQ(cJ,mp ? "[OK]" : "[..]",mp ? new Color(34,197,94): new Color(56,189,248),f1,b0,br);cQ(cJ,dM[e8],new Color(203,213,225),f1+5f*cp.X*br,b0,br);
b0+=fj;}using(var pQ=d1.DrawFrame()){pQ.AddRange(cJ);}}void gA(){ci.Clear();bO.Clear();eC.Clear();
di.Clear();ez=false;if(v==null)return;foreach(var T in v.bD){var aA=new List<IMyTerminalBlock>();if(T.a8==aE.c3){
string h1= !string.IsNullOrEmpty(T.aS)? T.aS : T.U;if(!au.ContainsKey(h1)){au[h1]=T.el;if(T.ge!=null)eB[h1]=T.ge;
}ci[T.U]=aA;continue;}var ni=T.az.Count>0 ? T.az : new List<string>{T.ka};foreach(var b_ in ni){
if(string.IsNullOrEmpty(b_))continue;bool lY=b_.Contains("*")||b_.Contains("?");if(T.a8==aE.go||lY){var hJ=new List<IMyTerminalBlock>();
nS(b_,hJ,T.ed);if(hJ.Count>0){foreach(var lK in hJ){if(!aA.Contains(lK))aA.Add(lK);}}else{var kt=ec(T.ed);
for(int g_=0;g_<kt.Count;g_++){var fU=kt[g_];if(lY ? E.of(b_,fU.CustomName): string.Equals(b_,fU.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aA.Contains(fU))aA.Add(fU);}}}}else{var g3=jH(b_,T.ed);if(g3!=null&& !aA.Contains(g3))aA.Add(g3);}}if(aA.Count==0){
oK(T,ni,aA);if(aA.Count>0)eC[T.U]=true;}ci[T.U]=aA;}foreach(var a0 in v.V){
if(string.IsNullOrEmpty(a0.ba))continue;var ir=jH(a0.ba);if(ir==null){di.AppendLine($"HELM: output block '{a0.ba}' not found");
continue;}IMyTextSurface bZ=null;var iJ=ir as IMyTextSurfaceProvider;var lc=ir as IMyTextSurface;if(iJ!=null&&a0.bH<iJ.SurfaceCount){
bZ=iJ.GetSurface(a0.bH);}else if(lc!=null&&a0.bH==0){bZ=lc;}if(bZ==null){di.AppendLine($"HELM: output block '{a0.ba}' has no surface index {a0.bH}");
continue;}bZ.ContentType=ContentType.SCRIPT;bZ.Script="";bZ.ScriptBackgroundColor=Color.Black;bO[a0]=bZ;}bM.Clear();
ch.Clear();}static readonly string[]jG={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool n3(int p7,IMyTerminalBlock r){switch(p7){case 0: return r is IMyBatteryBlock;case 1: return r is IMyReactor;case 2: return r is IMySolarPanel;
case 3: return r is IMyWindTurbine;case 4: return r is IMyGasTank;case 5: return r is IMyAirVent;case 6: return r is IMyCargoContainer;case 7: return r is IMyJumpDrive;
case 8: return r is IMyThrust;case 9: return r is IMyMotorSuspension;case 10: return r is IMyDoor;case 11: return r is IMyShipConnector;case 12: return r is IMyAssembler;
case 13: return r is IMyRefinery;case 14: return r is IMyShipDrill;case 15: return r is IMyShipWelder;case 16: return r is IMyShipGrinder;case 17: return r is IMyGasGenerator;
case 18: return r is IMyLargeTurretBase;case 19: return r is IMyProjector;case 20: return r is IMyGyro;case 21: return r is IMyGravityGeneratorBase;case 22: return r is IMyLandingGear;
case 23: return r is IMyRadioAntenna;case 24: return r is IMyBeacon;case 25: return r is IMyLightingBlock;case 26: return r is IMyPistonBase;case 27: return r is IMyMotorStator;
case 28: return r is IMyWarhead;case 29: return r is IMySensorBlock;case 30: return r is IMyCameraBlock;case 31: return r is IMyParachute;case 32: return r is IMyTurretControlBlock;
}return false;}void oK(aR ph,List<string>rB,List<IMyTerminalBlock>g4){string p1=(ph.U+" "+string.Join(" ",rB)).ToLowerInvariant();
var h4=new List<int>();for(int fe=0;fe<jG.Length;fe++){foreach(var rT in jG[fe].Split(','))if(p1.Contains(rT)){h4.Add(fe);break;}}if(h4.Count==0)return;
var ks=ec();var mq=new List<IMyTerminalBlock>();for(int hP=0;hP<ks.Count;hP++){var g0=ks[hP];foreach(int p8 in h4){if(n3(p8,g0)){(g0.CubeGrid==Me.CubeGrid ? g4 : mq).Add(g0);
break;}}}if(g4.Count==0)g4.AddRange(mq);}void oM(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(v==null||v.V.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {v.V.Count} screens, {v.bD.Count} binds");ez=false;
Echo($"Blocks: {ec().Count} on this construct; {gH} on docked ships (ignored)");bM.Clear();Echo("--- BIND RESOLUTION ---");foreach(var af in v.bD){
if(af.a8==aE.c3){string h2= !string.IsNullOrEmpty(af.aS)? af.aS : af.U;double rL=au.ContainsKey(h2)? au[h2]: af.el;
Echo($"[OK] @bind {af.U} (dynamic variable '{h2}' = {rL})");continue;}List<IMyTerminalBlock>g5;if(ci.TryGetValue(af.U,out g5)){if(g5.Count>0){
string pK=eC.ContainsKey(af.U)&&eC[af.U]? " [fallback: typed match]" : "";Echo($"[OK] @bind {af.U} ({g5.Count} blocks matched{pK})");
}else{string rC=string.Join(", ",af.az.Count>0 ? af.az : new List<string>{af.ka});Echo($"[WARN] @bind {af.U} -> 0 blocks matched \"{rC}\"");
}}else{Echo($"[ERR] @bind {af.U} -> unresolved");}}if(au.Count>0){Echo($"--- DYNAMIC VARIABLES ({au.Count}) ---");foreach(var l5 in au)Echo($"[VAR] {l5.Key} = {l5.Value}");
}IMyTextSurface gS=null;foreach(var qC in bO.Values){gS=qC;break;}if(gS!=null){var i4=new List<string>();gS.GetSprites(i4);
Echo($"--- SPRITES ({i4.Count}): names usable in bg_texture ---");Echo(string.Join(", ",i4.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var a1 in v.V){IMyTextSurface nb;if(bO.TryGetValue(a1,out nb)){var m1=nb.SurfaceSize;var rs=E.nU(a1);
Echo($"[OK] Screen \"{a1.a9}\" [{a1.bI}] -> {a1.ba} [#{a1.bH}] ({m1.X:0}x{m1.Y:0} px, {rs.W} sprites)");
}else{Echo($"[ERR] Screen \"{a1.a9}\" -> Missing block \"{a1.ba}\" [#{a1.bH}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double oL(IMyTerminalBlock s){
if(s==null)return 0.0;var eM=s as IMyBatteryBlock;if(eM!=null)return eM.MaxStoredPower>0 ?(eM.CurrentStoredPower/eM.MaxStoredPower)*100.0 : 0.0;
var fd=s as IMyJumpDrive;if(fd!=null)return fd.MaxStoredPower>0 ?(fd.CurrentStoredPower/fd.MaxStoredPower)*100.0 : 0.0;var nh=s as IMyGasTank;
if(nh!=null)return nh.FilledRatio*100.0;var nA=s as IMyAirVent;if(nA!=null)return nA.GetOxygenLevel()*100.0;var kV=s as IMyCargoContainer;if(kV!=null){
var fb=kV.GetInventory();if(fb!=null&&fb.MaxVolume.RawValue>0)return(double)fb.CurrentVolume.RawValue/fb.MaxVolume.RawValue*100.0;return 0.0;}var fy=s as IMyReactor;
if(fy!=null)return fy.MaxOutput>0 ?(fy.CurrentOutput/fy.MaxOutput)*100.0 : 0.0;var fH=s as IMySolarPanel;if(fH!=null)return fH.MaxOutput>0 ?(fH.CurrentOutput/fH.MaxOutput)*100.0 : 0.0;
var f0=s as IMyWindTurbine;if(f0!=null)return f0.MaxOutput>0 ?(f0.CurrentOutput/f0.MaxOutput)*100.0 : 0.0;var ld=s as IMyDoor;if(ld!=null)return(double)ld.OpenRatio*100.0;
var fX=s as IMyThrust;if(fX!=null)return fX.MaxEffectiveThrust>0 ?(fX.CurrentThrust/fX.MaxEffectiveThrust)*100.0 : 0.0;var jd=s as IMyMotorSuspension;
if(jd!=null)return jd.Brake ? 100.0 :(jd.Friction*100.0);var hj=s as IMyShipConnector;if(hj!=null)return hj.Status==MyShipConnectorStatus.Connected ? 100.0 :(hj.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var mF=s as IMyProductionBlock;if(mF!=null)return mF.IsProducing ? 100.0 : 0.0;var lh=s as IMyShipDrill;if(lh!=null)return lh.Enabled ? 100.0 : 0.0;
var ji=s as IMyShipToolBase;if(ji!=null)return ji.IsActivated ? 100.0 :(ji.Enabled ? 50.0 : 0.0);var hG=s as IMyGasGenerator;if(hG!=null)return hG.AutoRefill ? 100.0 :(hG.Enabled ? 50.0 : 0.0);
var fY=s as IMyLargeTurretBase;if(fY!=null)return fY.IsShooting ? 100.0 :(fY.HasTarget ? 50.0 :(fY.Enabled ? 10.0 : 0.0));var dV=s as IMyProjector;
if(dV!=null)return dV.TotalBlocks>0 ?((double)(dV.TotalBlocks-dV.RemainingBlocks)/dV.TotalBlocks)*100.0 : 0.0;var hM=s as IMyGyro;if(hM!=null)return hM.GyroOverride ? 100.0 :(hM.Enabled ? 50.0 : 0.0);
var lM=s as IMyGravityGeneratorBase;if(lM!=null)return Math.Min(100.0,Math.Abs(lM.GravityAcceleration)/9.81*100.0);var hF=s as IMyLandingGear;if(hF!=null)return hF.IsLocked ? 100.0 :(hF.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var ku=s as IMyRadioAntenna;if(ku!=null)return(ku.Radius/50000.0)*100.0;var kF=s as IMyBeacon;if(kF!=null)return(kF.Radius/50000.0)*100.0;
var mb=s as IMyLightingBlock;if(mb!=null)return mb.Intensity*10.0;var fw=s as IMyPistonBase;if(fw!=null)return fw.HighestPosition>0 ?(fw.CurrentPosition/fw.HighestPosition)*100.0 : 0.0;
var mJ=s as IMyMotorStator;if(mJ!=null)return Math.Min(100.0,Math.Abs(mJ.Angle)/6.28318*100.0);var nC=s as IMyWarhead;if(nC!=null)return nC.IsArmed ? 100.0 : 0.0;
var i_=s as IMySensorBlock;if(i_!=null)return!i_.LastDetectedEntity.IsEmpty()? 100.0 :(i_.Enabled ? 25.0 : 0.0);var hc=s as IMyCameraBlock;
if(hc!=null)return hc.IsActive ? 100.0 :(hc.Enabled ? 50.0 : 0.0);var k0=s as IMyParachute;if(k0!=null)return(double)k0.OpenRatio*100.0;
var fW=s as IMyTurretControlBlock;if(fW!=null)return fW.HasTarget ? 100.0 :(fW.IsUnderControl ? 50.0 :(fW.Enabled ? 10.0 : 0.0));var lB=s as IMyFunctionalBlock;
if(lB!=null)return lB.Enabled ? 100.0 : 0.0;return 0.0;}static double oc(IMyTerminalBlock kB,string hx){int kU=hx.IndexOf(':');string h3=hx.Substring(0,kU).ToLowerInvariant();
var rJ=new MyItemType("MyObjectBuilder_"+(h3=="ammo" ? "AmmoMagazine" : char.ToUpper(h3[0])+h3.Substring(1)),hx.Substring(kU+1));double na=0;for(int hQ=0;
hQ<kB.InventoryCount;hQ++)na+=(double)kB.GetInventory(hQ).GetItemAmount(rJ);return na;}double j2(IMyTerminalBlock t,string lt){if(t==null)return 0.0;
string a=lt.ToLowerInvariant();if(a=="percent"||a=="pct")return oL(t);if(a.IndexOf(':')>0)return oc(t,lt);var bi=t as IMyBatteryBlock;
if(bi!=null){if(a=="current"||a=="stored"||a=="val"||a=="value"||a=="raw")return bi.CurrentStoredPower;if(a=="max"||a=="capacity")return bi.MaxStoredPower;
if(a=="output")return bi.CurrentOutput;if(a=="maxoutput")return bi.MaxOutput;if(a=="input")return bi.CurrentInput;if(a=="maxinput")return bi.MaxInput;
return bi.CurrentStoredPower;}var h0=t as IMyJumpDrive;if(h0!=null){if(a=="max"||a=="capacity")return h0.MaxStoredPower;return h0.CurrentStoredPower;
}var fR=t as IMyGasTank;if(fR!=null){if(a=="max"||a=="capacity")return fR.Capacity;return fR.FilledRatio*fR.Capacity;}var nB=t as IMyAirVent;
if(nB!=null){if(a=="max"||a=="capacity")return 100.0;return nB.GetOxygenLevel()*100.0;}var kW=t as IMyCargoContainer;if(kW!=null){var fc=kW.GetInventory();
if(fc!=null){if(a=="mass")return(double)fc.CurrentMass;if(a=="max"||a=="capacity")return(double)fc.MaxVolume;return(double)fc.CurrentVolume;}return 0.0;}var iK=t as IMyPowerProducer;
if(iK!=null){if(a=="max"||a=="capacity")return iK.MaxOutput;return iK.CurrentOutput;}var le=t as IMyDoor;if(le!=null){if(a=="max")return 1.0;return(double)le.OpenRatio*100.0;
}var jg=t as IMyThrust;if(jg!=null){if(a=="max"||a=="capacity")return jg.MaxEffectiveThrust;return jg.CurrentThrust;}var bw=t as IMyMotorSuspension;
if(bw!=null){if(a=="brake")return bw.Brake ? 100.0 : 0.0;if(a=="height")return bw.Height;if(a=="strength")return bw.Strength;if(a=="power")return bw.Power;
if(a=="steer")return bw.SteeringOverride*100.0;if(a=="propulsion")return bw.PropulsionOverride*100.0;return bw.Friction;}var k6=t as IMyShipConnector;
if(k6!=null){return k6.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var iH=t as IMyProductionBlock;if(iH!=null){if(a=="producing"||a=="active"||a=="running"||a=="state")return iH.IsProducing ? 100.0 : 0.0;
if(a=="queue")return iH.IsQueueEmpty ? 0.0 : 1.0;}var gW=t as IMyAssembler;if(gW!=null){if(a=="disassembling")return gW.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(a=="repeating"||a=="loop")return gW.Repeating ? 100.0 : 0.0;}var hH=t as IMyGasGenerator;if(hH!=null){if(a=="autorefill")return hH.AutoRefill ? 100.0 : 0.0;
if(a=="active")return hH.Enabled ? 100.0 : 0.0;}var fZ=t as IMyLargeTurretBase;if(fZ!=null){if(a=="shooting")return fZ.IsShooting ? 100.0 : 0.0;
if(a=="target"||a=="hastarget")return fZ.HasTarget ? 100.0 : 0.0;if(a=="range")return fZ.Range;}var bq=t as IMyProjector;if(bq!=null){if(a=="built")return bq.TotalBlocks-bq.RemainingBlocks;
if(a=="remaining")return bq.RemainingBlocks;if(a=="total")return bq.TotalBlocks;if(a=="projecting")return bq.IsProjecting ? 100.0 : 0.0;if(a=="armor")return bq.RemainingArmorBlocks;
if(a=="buildable")return bq.BuildableBlocksCount;}var jj=t as IMyShipToolBase;if(jj!=null){if(a=="working"||a=="active"||a=="activated")return jj.IsActivated ? 100.0 : 0.0;
return jj.Enabled ? 100.0 : 0.0;}var cu=t as IMyGyro;if(cu!=null){if(a=="override")return cu.GyroOverride ? 100.0 : 0.0;if(a=="pitch")return cu.Pitch;
if(a=="yaw")return cu.Yaw;if(a=="roll")return cu.Roll;if(a=="power")return cu.GyroPower*100.0;}var lN=t as IMyGravityGeneratorBase;if(lN!=null){if(a=="g"||a=="accel")return lN.GravityAcceleration;
}var lL=t as IMyLandingGear;if(lL!=null){if(a=="locked")return lL.IsLocked ? 100.0 : 0.0;}var kv=t as IMyRadioAntenna;if(kv!=null){if(a=="radius"||a=="range")return kv.Radius;
}var kG=t as IMyBeacon;if(kG!=null){if(a=="radius"||a=="range")return kG.Radius;}var ib=t as IMyLightingBlock;if(ib!=null){if(a=="intensity")return ib.Intensity;
if(a=="radius")return ib.Radius;}var fx=t as IMyPistonBase;if(fx!=null){if(a=="pos"||a=="position"||a=="extension")return fx.CurrentPosition;
if(a=="velocity")return fx.Velocity;if(a=="max")return fx.HighestPosition;}var d_=t as IMyMotorStator;if(d_!=null){if(a=="angle"||a=="deg")return d_.Angle*180.0/Math.PI;
if(a=="rad")return d_.Angle;if(a=="velocity"||a=="rpm")return d_.TargetVelocityRPM;if(a=="locked")return d_.RotorLock ? 100.0 : 0.0;}var jr=t as IMyWarhead;
if(jr!=null){if(a=="armed")return jr.IsArmed ? 100.0 : 0.0;if(a=="countdown")return jr.DetonationTime;}var mZ=t as IMySensorBlock;if(mZ!=null){
if(a=="detected")return!mZ.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var hd=t as IMyCameraBlock;if(hd!=null){if(a=="range")return hd.AvailableScanRange;
if(a=="active")return hd.IsActive ? 100.0 : 0.0;}var k1=t as IMyParachute;if(k1!=null){if(a=="open"||a=="openratio")return(double)k1.OpenRatio*100.0;
}var cN=t as IMyTurretControlBlock;if(cN!=null){if(a=="target"||a=="hastarget")return cN.HasTarget ? 100.0 : 0.0;if(a=="azimuth")return cN.AzimuthRotor!=null ? cN.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(a=="elevation")return cN.ElevationRotor!=null ? cN.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var lC=t as IMyFunctionalBlock;if(lC!=null){return lC.Enabled ? 100.0 : 0.0;
}return 0.0;}double bJ(string dp,string lu){aR bj;df.TryGetValue(dp,out bj);if(bj!=null&&bj.a8==aE.c3){
string p9= !string.IsNullOrEmpty(bj.aS)? bj.aS : dp;double k7;if(au.TryGetValue(p9,out k7))return k7;return bj.el;
}double lk;if(au.TryGetValue(dp,out lk))return lk;List<IMyTerminalBlock>dq;if(!ci.TryGetValue(dp,out dq)||dq.Count==0)throw new InvalidOperationException($"HELM: bind '{dp}' resolved to zero blocks — check the name/typo");
if(dq.Count==1)return j2(dq[0],lu);var nz=new List<double>();foreach(var o6 in dq)nz.Add(j2(o6,lu));return nK.nJ(bj!=null ? bj.cR : ar.f4,nz);
}double gB(string pj){return bJ(pj,"percent");}static string a5(double nw,bG cA){if(cA==null||(string.IsNullOrEmpty(cA.de)&&string.IsNullOrEmpty(cA.c4)&&cA.N=="percent")){
return jL(nw);}return E.n4(nw,cA.de,cA.c4);}bool oF(C cH){IMyTextSurface fO;if(!bO.TryGetValue(cH,out fO))return false;
if(cH.bA!=null)return oz(cH,fO);if(cH.cZ.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return oG(cH,fO);
}return oH(cH,fO);}Color n7(string[]cK,double qJ){if(cK.Length==1)return D(cK[0]);double mG=Math.Max(0.0,Math.Min(100.0,qJ))/100.0*(cK.Length-1);
int hR=Math.Min((int)mG,cK.Length-2);float fP=(float)(mG-hR);Color bg=D(cK[hR]),eL=D(cK[hR+1]);return new Color((int)(bg.R+(eL.R-bg.R)*fP),(int)(bg.G+(eL.G-bg.G)*fP),(int)(bg.B+(eL.B-bg.B)*fP),(int)(bg.A+(eL.A-bg.A)*fP));
}static Color[]d9(aU po,string[]rv,string pB,float nE,float nG,float jp,float hN){int dm=16;bool lS=pB=="horizontal";var hi=new Color[dm];
for(int bS=0;bS<dm;bS++){n hE=E.jM(rv,100.0*bS/(dm-1));hi[bS]=new Color(hE.bb,hE.a6,hE.a2);float kT=jp/dm,kM=hN/dm;x(po,lS ? new Vector2(nE+kT*(bS+0.5f),nG+hN/2f): new Vector2(nE+jp/2f,nG+kM*(bS+0.5f)),lS ? new Vector2(kT+1f,hN): new Vector2(jp,kM+1f),hi[bS]);
}return hi;}static void x(aU pp,Vector2 qR,Vector2 rm,Color pr,float q8=0f,string pA="SquareSimple"){pp.k.Add(new MySprite{Type=SpriteType.TEXTURE,Data=pA,Position=qR,Size=rm,Color=pr,RotationOrScale=q8}
);}void nR(C h,IMyTextSurface nc,aU f){f.k.Clear();f.ew.Clear();Vector2 J=nc.SurfaceSize;
Vector2 rG=nc.TextureSize;Vector2 aq=(rG-J)/2f;float fp=Math.Max(4f,J.X*0.02f);float bT=Math.Max(4f,J.Y*0.02f);
if(h.bI=="compact"||(h.bI=="auto"&&J.Y<=260f&&J.Y>150f)){fp=4f;bT=4f;}else if(h.bI=="corner"||(h.bI=="auto"&&J.Y<=150f)){
fp=2f;bT=2f;}float d6=J.X-(fp*2f);Color cl=new Color(10,12,16);bool lg=true;if(!string.IsNullOrEmpty(h.bz)){
var dA=n.c9(h.bz);if(dA.eq)lg=false;else cl=new Color(dA.bb,dA.a6,dA.a2,dA.f2);}f.bC= -1;f.eb=false;
f.ea=D(h.ju ?? "#5b0f0f");f.cY.Clear();if(h.bB!=null&&h.bB.Length>1){f.bC=f.k.Count;f.ef=d9(f,h.bB,h.cX,aq.X+2f,aq.Y+2f,J.X-4f,J.Y-4f);
}else if(lg||h.aD!=null||h.b5!=null){if(h.aD!=null)cl=D(h.aD.First(qW=>qW.cb).bF);f.bC=f.k.Count;
f.ef=new[]{cl};x(f,aq+(J/2f),J-new Vector2(4f,4f),cl);if(h.aD!=null||h.b5!=null)f.cY.Add(new eg{
ep=f.bC,f5=cl,ek=cl,ev=h.aD,f9=h.f6,gh=h.f7,en=h.b5,gi=h.gj,gk=h.gl,jS=true}
);}if(!string.IsNullOrEmpty(h.f8))x(f,aq+(J/2f),J-new Vector2(4f,4f),D(h.jw ?? "255,255,255,24"),0f,h.f8);
if(!h.cZ.Equals("none",StringComparison.OrdinalIgnoreCase)){Color kO=h.al!=null&&h.al.Length>0 ? D(h.al[0]): D(h.ej ?? "40, 60, 85");
Color pn=h.al!=null&&h.al.Length>1 ? D(h.al[h.al.Length-1]): kO;
float kP=aq.X+(J.X-d6)/2f;if(h.al!=null&&h.al.Length>1&& !h.gn){d9(f,h.al,"horizontal",kP,aq.Y+bT*0.5f-1f,d6,2f);
d9(f,h.al,"horizontal",kP,aq.Y+J.Y-(bT*0.5f)-1f,d6,2f);}else{x(f,new Vector2(aq.X+J.X/2f,aq.Y+bT*0.5f),new Vector2(d6,2f),kO);
x(f,new Vector2(aq.X+J.X/2f,aq.Y+J.Y-(bT*0.5f)),new Vector2(d6,2f),pn);}}var ix=h.aH;
if(ix.Count==0){ix=new List<at>{new at{c8="main",c0=0,da=0,db=h.b9,dc=h.ca,aT=h.gs}};}float lP=8f;
foreach(var u in ix){var fo=E.nT(u.c0,u.da,u.db,u.dc,h.b9,h.ca,J.X,J.Y,fp,bT,lP,lP);
float aK=aq.X+fo.ki;float dS=aq.Y+fo.kj;float aZ=fo.kh;float fn=fo.jO;bool mM=u.aD!=null||u.b5!=null;
if(u.bB!=null&&u.bB.Length>1){d9(f,u.bB,u.cX!="vertical" ? u.cX : h.cX,aK,dS,aZ,fn);
}else{Color bp;if(u.aD!=null)bp=D(u.aD.First(qX=>qX.cb).bF);else if(!string.IsNullOrEmpty(u.cS))bp=D(u.cS);
else if(h.b9>1||h.ca>1)bp=string.IsNullOrEmpty(h.gw)? new Color(255,255,255,6): D(h.gw);else bp=Color.Transparent;
if(bp.A>0||mM){if(mM)f.cY.Add(new eg{ep=f.k.Count,f5=bp,ek=bp,ev=u.aD,f9=u.f6,gh=u.f7,en=u.b5,gi=u.gj,gk=u.gl}
);x(f,new Vector2(aK+aZ/2f,dS+fn/2f),new Vector2(aZ,fn),bp);}}string hD=u.jI ?? h.jJ ?? "Monospace";float pf=E.n6(h.bI,J.X,J.Y,h.b7);
float aB=u.jK ?? pf;float e4=0f;if(!string.IsNullOrEmpty(u.bL)){e4=20f*aB;var gQ=TextAlignment.LEFT;float jh=aK+4f;
string ko=u.gC ?? h.kc ?? "left";if(ko.Equals("center",StringComparison.OrdinalIgnoreCase)){gQ=TextAlignment.CENTER;jh=aK+(aZ/2f);
}else if(ko.Equals("right",StringComparison.OrdinalIgnoreCase)){gQ=TextAlignment.RIGHT;jh=aK+aZ-4f;}string nk=u.gD ?? h.kd;
Color rH= !string.IsNullOrEmpty(nk)? D(nk): D(u.gc ?? h.ej ?? "white");var np=MySprite.CreateText(u.bL,hD,rH,aB*0.9f,gQ);
np.Position=new Vector2(jh,dS+2f);f.k.Add(np);string[]g1=u.gd ?? h.al;Color pD=g1!=null&&g1.Length>0 ? D(g1[0]): D(u.gc ?? h.ej ?? "40, 60, 85");
x(f,new Vector2(aK+aZ/2f,dS+e4-2f),new Vector2(aZ-8f,1f),pD);}int qu=Math.Max(1,u.aT.Count);float qH=fn-e4-8f;
float iO=qH/qu;float fa=6f;for(int e9=0;e9<u.aT.Count;e9++){var y=u.aT[e9];var P=new c7{jV=y};float q9=dS+e4+4f+(e9*iO);
float I=q9+(iO/2f);float nm=I-(11f*aB);P.gq=f.k.Count;P.jN=y.S!=null&&(y.S.Contains("{val}")||y.S.Contains("{percent}"));
var l8=MySprite.CreateText(y.S,hD,Color.White,aB,TextAlignment.LEFT);l8.Position=new Vector2(aK+fa,nm);f.k.Add(l8);
float qn=Math.Max(24f*aB,(y.S.Length+1)*12f*aB);float rS=(!string.IsNullOrEmpty(y.de)|| !string.IsNullOrEmpty(y.c4))? 64f*aB : 52f*aB;
if(y.g!=e.kb){float H=aK+fa+qn;float pe=aK+aZ-fa-rS;float F=Math.Max(16f,pe-H);
float O=Math.Max(y.g==e.bK ? 8f : 4f,Math.Min((y.g==e.bK ? 20f : 16f)*aB,iO*(y.g==e.bK ? 0.55f : 0.45f)));
P.cW=H;P.ee=F;P.b3=O;P.cV=I;if(y.g==e.cU){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(25,30,38));
P.aa=f.k.Count;x(f,new Vector2(H,I),new Vector2(0f,O),D(y.am));}else if(y.g==e.c6){
int iY=10;float lE=2f;float iZ=Math.Max(2f,(F-(lE*(iY-1)))/iY);for(int iT=0;iT<iY;iT++){P.ad.Add(f.k.Count);
x(f,new Vector2(H+iT*(iZ+lE)+(iZ/2f),I),new Vector2(iZ,O),new Color(22,26,32));}}else if(y.g==e.bK){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(20,24,30));
int iG=8;float lF=2f;float hg=Math.Max(2f,(F-(lF*(iG-1)))/iG);for(int iv=0;iv<iG;iv++){P.ad.Add(f.k.Count);
x(f,new Vector2(H+iv*(hg+lF)+(hg/2f),I),new Vector2(hg,2f),D(y.am));}}else if(y.g==e.c5){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(20,24,30));
int i7=10;float lG=2f;float i8=Math.Max(2f,(F-(lG*(i7-1)))/i7);for(int iU=0;iU<i7;iU++){P.ad.Add(f.k.Count);
x(f,new Vector2(H+iU*(i8+lG)+(i8/2f),I),new Vector2(i8,O),new Color(24,28,36),0.523599f);}}else if(y.g==e.c_){
int gU=10;float lH=2f;float gV=Math.Max(2f,(F-(lH*(gU-1)))/gU);for(int iV=0;iV<gU;iV++){P.ad.Add(f.k.Count);
x(f,new Vector2(H+iV*(gV+lH)+(gV/2f),I),new Vector2(gV,O),new Color(24,28,36),1.570796f,"Triangle");}}else if(y.g==e.c2){
int hm=12;float lI=2f;float ho=Math.Max(2f,(F-(lI*(hm-1)))/hm);float lf=Math.Min(ho,O);for(int iW=0;iW<hm;iW++){P.ad.Add(f.k.Count);
x(f,new Vector2(H+iW*(ho+lI)+(ho/2f),I),new Vector2(lf,lf),new Color(24,28,36),0f,"Circle");}}else if(y.g==e.b2){
x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(25,30,38));P.aa=f.k.Count;x(f,new Vector2(H+(F/2f),I),new Vector2(0f,O),D(y.am));
x(f,new Vector2(H+(F/2f),I),new Vector2(2f,O+4f),new Color(80,110,150));}else if(y.g==e.ce){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(20,24,30));
P.aa=f.k.Count;float h5=Math.Max(4f,O);x(f,new Vector2(H+h5*0.5f,I),new Vector2(h5,h5),new Color(40,50,65),0f,"Circle");
}else if(y.g==e.b4){float cm=Math.Max(6f,Math.Min(O,14f*aB));x(f,new Vector2(H+cm*0.5f,I),new Vector2(cm,cm),new Color(28,34,46));
P.aa=f.k.Count;x(f,new Vector2(H+cm*0.5f,I),new Vector2(cm*0.6f,cm*0.6f),new Color(40,50,65));}else if(y.g==e.cd){
x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(18,22,30));P.aa=f.k.Count;float iE=Math.Max(4f,O*0.7f);
x(f,new Vector2(H+iE+2f,I),new Vector2(iE,iE),new Color(34,197,94),0f,"Circle");}else if(y.g==e.b1){int iD=8;
float lJ=2f;float iF=Math.Max(2f,(F-(lJ*(iD-1)))/iD);for(int iw=0;iw<iD;iw++){P.ad.Add(f.k.Count);x(f,new Vector2(H+iw*(iF+lJ)+(iF/2f),I),new Vector2(iF,O),new Color(24,28,36));
}}else if(y.g==e.b6){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(20,24,30));P.aa=f.k.Count;
float lV=Math.Max(4f,O);x(f,new Vector2(H+(F/2f),I),new Vector2(lV,lV),D(y.am),0f,"Triangle");}}var nu=MySprite.CreateText("---",hD,Color.White,aB,TextAlignment.RIGHT);
nu.Position=new Vector2(aK+aZ-fa,nm);P.be=f.k.Count;f.k.Add(nu);f.ew.Add(P);}}f.jT=J;
f.jR=true;}static void aI(aU kR,c7 m2,string rF,Color ps){var jn=kR.k[m2.be];jn.Data=rF;jn.Color=ps;kR.k[m2.be]=jn;
}static void j4(aU kS,c7 m3,Color pt){var lo=kS.k[m3.aa];lo.Color=pt;kS.k[m3.aa]=lo;}bool oH(C cI,IMyTextSurface ja){
aU i;if(!eE.TryGetValue(cI,out i)){i=new aU();eE[cI]=i;}if(!i.jR||i.jT!=ja.SurfaceSize){
nR(cI,ja,i);}bool L=false;bool eH=(aW%cI.f3)<cI.f3/2.0;bool jq=bN&&eH&&i.ea.A>0;
if(i.bC>=0&&jq!=i.eb){i.eb=jq;for(int dC=0;dC<i.ef.Length;dC++){var kI=i.k[i.bC+dC];kI.Color=jq ? i.ea : i.ef[dC];
i.k[i.bC+dC]=kI;}L=true;foreach(var pg in i.cY)pg.gm=true;}foreach(var ac in i.cY){if(ac.jS&&i.eb)continue;Color dn=ac.f5;
try{if(ac.ev!=null)dn=D(E.eu(ac.ev,bJ(ac.f9,ac.gh)));if(ac.en!=null&&eH&&E.eu(ac.en,bJ(ac.gi,ac.gk))!=null)dn=i.ea;
}catch{}if(dn!=ac.ek||ac.gm){ac.ek=dn;ac.gm=false;var kQ=i.k[ac.ep];kQ.Color=dn;i.k[ac.ep]=kQ;L=true;}}for(int hS=0;hS<i.ew.Count;
hS++){var c=i.ew[hS];var l=c.jV;double w;bool lR=false;double ii=l.es;try{w=bJ(l.a4,l.N);if(l.cc!=null)ii=bJ(l.cc,l.gt);
}catch{lR=true;w=0.0;}if(lR){if(c.er!= -99999.0||(c.be>=0&&i.k[c.be].Data!="ERR")){L=true;c.er= -99999.0;
if(c.be>=0){var iX=i.k[c.be];iX.Data="ERR";iX.Color=Color.Red;i.k[c.be]=iX;}}continue;}double aL=w;
if(ii>0)w=Math.Min(100.0,aL/ii*100.0);string pv=l.b8 ? E.eo(l.a7,w): l.am;Color _=D(pv);
bool aX=(_==Color.Red);if(l.b8&& !cI.gn&&l.a7.Length>1)_=n7(l.a7,w);if(aX&&eH){
_=new Color(255,75,75);L=true;}bool aC=Math.Abs(aL-c.er)>0.1;if(aC)c.er=aL;if(l.g==e.cU){double lv=(l.gp||l.N=="percent"||l.N=="pct")? w : gB(l.a4);
if(c.Q<0f)c.Q=(float)lv;float fS=(float)lv;if(Math.Abs(c.Q-fS)>0.05f){c.Q+=(fS-c.Q)*0.35f;
L=true;}else if(c.Q!=fS){c.Q=fS;L=true;}if(aC)L=true;if(L){float qK=(float)Math.Max(0.0,Math.Min(1.0,c.Q/100.0));
float lx=c.ee*qK;var eZ=i.k[c.aa];eZ.Size=new Vector2(lx,c.b3);eZ.Position=new Vector2(c.cW+(lx/2f),c.cV);
eZ.Color=_;i.k[c.aa]=eZ;aI(i,c,a5(aL,l),_);}}else if(l.g==e.c6||l.g==e.c5||l.g==e.c_||l.g==e.c2){
if(aC||aX){L=true;double pM=(l.gp||l.N=="percent"||l.N=="pct")? w : gB(l.a4);int mX=c.ad.Count;
for(int d0=0;d0<mX;d0++){bool qw=pM>=((d0+0.4)/(double)mX*100.0);Color rj=qw ? _ : new Color(24,28,36);var mY=i.k[c.ad[d0]];
mY.Color=rj;i.k[c.ad[d0]]=mY;}aI(i,c,a5(aL,l),_);}}else if(l.g==e.bK){
if(aC||aX){L=true;for(int e3=0;e3<7;e3++)c.ex[e3]=c.ex[e3+1];c.ex[7]=(float)w;int qQ=c.ad.Count;
for(int dO=0;dO<qQ;dO++){float p0=c.ex[dO];float p_=Math.Max(0.06f,Math.Min(1f,p0/100f));float k3=c.b3*p_;float pu=c.cV+(c.b3/2f)-(k3/2f);
var cr=i.k[c.ad[dO]];cr.Size=new Vector2(cr.Size.Value.X,k3);cr.Position=new Vector2(cr.Position.Value.X,pu);
cr.Color=_;i.k[c.ad[dO]]=cr;}aI(i,c,a5(aL,l),_);}}else if(l.g==e.b2){
if(c.Q<0f)c.Q=(float)w;float fT=(float)w;if(Math.Abs(c.Q-fT)>0.05f){c.Q+=(fT-c.Q)*0.35f;
L=true;}else if(c.Q!=fT){c.Q=fT;L=true;}if(L||aC){float lQ=c.ee*0.5f;float kZ=c.cW+lQ;
float lb=(float)Math.Max(-1.0,Math.Min(1.0,(c.Q-50.0)/50.0));float hB=Math.Abs(lb)*lQ;float pN=(lb>=0)?(kZ+hB*0.5f):(kZ-hB*0.5f);
var e_=i.k[c.aa];e_.Size=new Vector2(hB,c.b3);e_.Position=new Vector2(pN,c.cV);e_.Color=_;
i.k[c.aa]=e_;aI(i,c,a5(aL,l),_);}}else if(l.g==e.ce){if(aC||aX){
L=true;bool h_=w>=50.0;float l2=Math.Max(4f,c.b3);float qc=h_ ?(c.cW+c.ee-l2*0.5f):(c.cW+l2*0.5f);
Color l1=h_ ?(_!=Color.White ? _ : new Color(34,197,94)): new Color(60,70,85);var hz=i.k[c.aa];hz.Position=new Vector2(qc,c.cV);
hz.Color=l1;i.k[c.aa]=hz;aI(i,c,h_ ? "ON" : "OFF",l1);}}else if(l.g==e.b4){if(aC||aX){
L=true;bool hZ=w>=50.0;Color k_=hZ ?(_!=Color.White ? _ : new Color(56,189,248)): new Color(30,36,48);j4(i,c,k_);
aI(i,c,hZ ? "ACTIVE" : "READY",hZ ? k_ : new Color(148,163,184));}}else if(l.g==e.cd){if(aC||aX){
L=true;string rt=w>=75.0 ? "ONLINE" :(w>=25.0 ? "STBY" : "OFFLINE");Color m7=w>=75.0 ? new Color(34,197,94):(w>=25.0 ? new Color(234,179,8): new Color(239,68,68));
j4(i,c,m7);aI(i,c,rt,m7);}}else if(l.g==e.b1){if(aC||aX){L=true;int mB=c.ad.Count;
for(int dP=0;dP<mB;dP++){bool qx=w>=((dP+0.5)/(double)mB*100.0);Color qO=qx ? _ : new Color(24,28,36);var mC=i.k[c.ad[dP]];
mC.Color=qO;i.k[c.ad[dP]]=mC;}aI(i,c,a5(aL,l),_);}}else if(l.g==e.b6){
L=true;float fB;Color dy;string e0;bool p4=(l.N=="producing"||l.N=="active"||l.N=="running"||l.N=="state"||(l.N!="percent"&&(w<=1.0||w==100.0)));
if(p4){if(w>0.0){c.Q=(c.Q+0.3927f)%6.28318f;fB=c.Q;dy=new Color(34,197,94);e0="RUN";}else{fB=0f;
dy=new Color(148,163,184);e0="IDLE";}}else{fB=(w>55.0)? 0f :((w<45.0)? 3.14159f : 1.57079f);dy=(w>55.0)? new Color(34,197,94):((w<45.0)? new Color(239,68,68): new Color(148,163,184));
e0=(w>55.0)? "CHG" :((w<45.0)? "DSC" : "STBY");}var hA=i.k[c.aa];hA.RotationOrScale=fB;hA.Color=dy;
i.k[c.aa]=hA;aI(i,c,e0,dy);}else{string eX;aR fV;df.TryGetValue(l.a4,out fV);eB.TryGetValue(fV!=null&&fV.aS!=null ? fV.aS : l.a4,out eX);
if(aC||aX||eX!=c.jU){c.jU=eX;L=true;Color jk=l.c1!=null ? D(E.eu(l.c1,w)): Color.White;
if(jk==Color.Red&&eH)jk=new Color(255,75,75);aI(i,c,eX ?? a5(aL,l),jk);}}if(c.jN&&(aC||aX)){
L=true;var ma=i.k[c.gq];string qL=jL(w);string q6=a5(aL,l);ma.Data=l.S.Replace("{percent}",qL).Replace("{val}",q6);
i.k[c.gq]=ma;}}if(L){using(var pR=ja.DrawFrame()){pR.AddRange(i.k);}}return L;}bool oG(C bs,IMyTextSurface jb){
jb.FontSize=bs.b7;var eT=new List<KeyValuePair<string,string>>();foreach(var m in bs.gs){double K=0.0;string aP;string aJ=null;
double ij=m.es;try{K=bJ(m.a4,m.N);if(m.cc!=null)ij=bJ(m.cc,m.gt);}catch(Exception rZ){
eT.Add(new KeyValuePair<string,string>($"{m.S}: [UNRESOLVED]","red"));continue;}double mH=K;if(ij>0)K=Math.Min(100.0,mH/ij*100.0);string nv=a5(mH,m);
if(m.g==e.cU){double lw=(m.gp||m.N=="percent"||m.N=="pct")? K : gB(m.a4);aP=E.oA(m.S,lw,10,nv);
aJ=m.b8 ? E.eo(m.a7,lw): m.am;}else if(m.g==e.b2){aP=E.oy(m.S,K,10);
aJ=m.b8 ? E.eo(m.a7,K): m.am;}else if(m.g==e.ce){aP=E.oJ(m.S,K);
aJ=m.am ??(K>=50.0 ? "green" : "gray");}else if(m.g==e.b4){aP=E.oD(m.S,K);aJ=m.am ??(K>=50.0 ? "cyan" : "gray");
}else if(m.g==e.cd){aP=E.oI(m.S,K);aJ=m.am ??(K>=75.0 ? "lime" :(K>=25.0 ? "yellow" : "red"));
}else if(m.g==e.b1){aP=E.ox(m.S,K,8);aJ=m.b8 ? E.eo(m.a7,K):(m.am ?? "yellow");
}else if(m.g==e.b6){bool p5=(m.N=="producing"||m.N=="active"||m.N=="running"||m.N=="state"||(m.N!="percent"&&(K<=1.0||K==100.0)));
if(p5){char[]rr=new[]{'|','/','-','\\'};char rg=rr[Math.Abs(dj)%4];aP=K>0 ? $"{m.S} [{rg}] RUN" : $"{m.S} [==] IDLE";
aJ=K>0 ? "green" : "gray";}else{aP=E.oE(m.S,K,dj);aJ=m.am ??(K>55.0 ? "green" :(K<45.0 ? "red" : "cyan"));
}}else{aP=$"{m.S}: {nv}";aJ=m.c1!=null ? E.eu(m.c1,K): null;}eT.Add(new KeyValuePair<string,string>(aP,aJ));
}var eQ=E.oC(new List<string>(eT.ConvertAll(rA=>rA.Key)),cP: bs.kg);string m_=string.Join("\n",eQ);
string mE;if(cg.TryGetValue(bs,out mE)&&mE==m_)return false;cg[bs]=m_;using(var pS=jb.DrawFrame()){
float nH=4f;float mc=nI*bs.b7;try{var me=jb.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bs.b7);
if(me.Y>0)mc=me.Y;}catch{}for(int cv=0;cv<eQ.Count;cv++){string k5=null;if(cv>0&&cv<eQ.Count-1)k5=eT[cv-1].Value;
var m6=MySprite.CreateText(eQ[cv],"Monospace",D(k5),bs.b7,TextAlignment.LEFT);m6.Position=new Vector2(4f,nH);pS.Add(m6);
nH+=mc;}}return true;}Color D(string fl){if(string.IsNullOrEmpty(fl))return Color.White;Color dJ;if(kk.TryGetValue(fl,out dJ))return dJ;
n dr=n.c9(fl);dJ=dr.eq ? Color.Transparent : new Color(dr.bb,dr.a6,dr.a2,dr.f2);kk[fl]=dJ;return dJ;}public enum aE{nM,go,c3}
public enum ar{og,f4,j9,jX,jW}public class aR{public string U;public aE a8;public List<string>az=new List<string>();public string ka{
get{return az.Count>0 ? az[0]: "";}set{if(az.Count==0)az.Add(value);else az[0]=value;}}public ar cR=ar.og;
public double el=0.0;public string ge;public string aS;public bool ed;}public enum e{kb,cU,c6,bK,c5,c_,c2,b2,ce,b4,cd,b1,b6}
public class aw{public string gg;public string bF;public bool cb;}public class bG{public e g;public string S;public string a4;
public string N;public bool b8;public string[]a7;public string am;public List<aw>c1;public string jD="right";
public string de;public string c4;public double es;public string cc,gt;public bool gp{get{return es>0||cc!=null;}}}public struct n{
public byte bb,a6,a2,f2;public bool eq;public n(byte qY,byte pU,byte o7,byte oR=255){bb=qY;a6=pU;a2=o7;f2=oR;eq=false;}public static readonly n oP=new n(0,0,0,0){
eq=true};public static readonly n ey=new n(255,255,255);public static readonly n nL=new n(0,0,0);public static readonly n ov=new n(255,0,0);
public static readonly n n9=new n(0,255,0);public static readonly n od=new n(50,205,50);public static readonly n oQ=new n(255,255,0);
public static readonly n oh=new n(255,140,0);public static readonly n nW=new n(0,255,255);public static readonly n nN=new n(0,120,255);
public static readonly n oe=new n(255,0,255);public static readonly n ot=new n(128,0,128);public static readonly n n8=new n(128,128,128);
public static readonly n nX=new n(60,60,60);public static n c9(string aO){if(string.IsNullOrWhiteSpace(aO))return ey;aO=aO.Trim();
if(aO.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aO.Equals("none",StringComparison.OrdinalIgnoreCase))return oP;if(aO.StartsWith("#")){
string bm=aO.Substring(1).Trim();if(bm.Length==6){byte qZ=Convert.ToByte(bm.Substring(0,2),16);byte pV=Convert.ToByte(bm.Substring(2,2),16);byte o8=Convert.ToByte(bm.Substring(4,2),16);
return new n(qZ,pV,o8);}if(bm.Length==3){byte q_=Convert.ToByte(new string(bm[0],2),16);byte pW=Convert.ToByte(new string(bm[1],2),16);byte o9=Convert.ToByte(new string(bm[2],2),16);
return new n(q_,pW,o9);}}if(aO.Contains(",")){var cD=aO.Split(',');if(cD.Length>=3){byte q0=(byte)Math.Max(0,Math.Min(255,int.Parse(cD[0].Trim())));
byte pX=(byte)Math.Max(0,Math.Min(255,int.Parse(cD[1].Trim())));byte pa=(byte)Math.Max(0,Math.Min(255,int.Parse(cD[2].Trim())));byte oS=cD.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(cD[3].Trim()))):(byte)255;
return new n(q0,pX,pa,oS);}}switch(aO.ToLowerInvariant()){case "red": return ov;case "green": return n9;case "lime": return od;case "yellow": return oQ;
case "orange": return oh;case "cyan": return nW;case "blue": return nN;case "magenta": return oe;case "purple": return ot;case "white": return ey;
case "black": return nL;case "gray": case "grey": return n8;case "darkgray": case "darkgrey": return nX;default: return ey;}}}public class a3{
public string[]bB;public string cX="vertical";public List<aw>aD;public string f6,f7;public List<aw>b5;public string gj,gl;
}public class at : a3{public string c8="default";public int c0=0;public int da=0;public int db=1;public int dc=1;public string bL;public string gC="left";
public string gD;public string jB;public string jI;public float? jK;public string cS;public string gc;public string[]gd;
public string jY="horizontal";public List<bG>aT=new List<bG>();}public class C : a3{public string a9;public string bI="standard";
public string cZ="graphic";public string ej;public string[]al;public string kc="left";public string kd;public string jJ="Monospace";
public string bz;public string jz;public int jA=10;public int jx=1;public bool gn;public string gw;public string f8;
public string jw;public string ju;public string bA;public string bE;public float gb=2.5f;public string jy="cyan";public string[]eh;
public List<string>ei=new List<string>();public List<string>n5(){if(ei!=null&&ei.Count>0)return ei;return new List<string>{
"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"};}public float b7=1.0f;
public int gy=0;public float f3=1.6f;public int b9=1;public int ca=1;public int kg=30;public string ba;public int bH=0;
public List<at>aH=new List<at>();public List<bG>gs{get{if(aH.Count==0)aH.Add(new at{c8="main"});return aH[0].aT;}
}}public class bd{public string gu;public List<string>et=new List<string>();public Dictionary<string,string>gf=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>ga=new List<string>();}public class gx{public List<aR>bD=new List<aR>();public List<C>V=new List<C>();
public List<string>aG=new List<string>();public Dictionary<string,List<string>>cT=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class jP{static readonly Dictionary<string,bd>jC=new Dictionary<string,bd>(StringComparer.OrdinalIgnoreCase);
static jP(){ow();}static void ow(){bc("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bc("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bc("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bc("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bc("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bc("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bc("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bc("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bc(string ml,string[]qI,string pk){var fQ=new bd{gu=ml}
;foreach(var qD in qI){var h6=qD.Split(new[]{'='},2);string mt=gv(h6[0]);fQ.et.Add(mt);if(h6.Length>1)fQ.gf[mt]=ab(h6[1].Trim());
}var qv=pk.Replace("\r\n","\n").Split('\n');foreach(var qi in qv){string mO=qi.Trim();if(mO.Length>0)fQ.ga.Add(mO);}jC[ml]=fQ;}static string gv(string cB){
cB=cB.Trim();if(cB.StartsWith("$"))cB=cB.Substring(1);return cB.ToLowerInvariant();}public static gx oj(string py){var ai=new gx();C d=null;
at p=null;var mD=oq(py,ai.aG,ai.cT);for(int dD=0;dD<mD.Count;dD++){string b=mD[dD].Trim();
if(b.Length==0)continue;try{if(b.StartsWith("@bind")){ai.bD.Add(ok(b));}else if(b.StartsWith("@screen")){d=new C{a9=b.Substring("@screen".Length).Trim()}
;ai.V.Add(d);p=null;}else if(b.StartsWith("profile:")){q(d,"profile:");d.bI=b.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(b.StartsWith("border:")){q(d,"border:");string kC=b.Substring("border:".Length).Trim();if(p!=null)p.jB=kC;
else d.cZ=kC;}else if(b.StartsWith("border_color:")){q(d,"border_color:");string kE=b.Substring("border_color:".Length).Trim();
if(p!=null)p.gc=kE;else d.ej=kE;}else if(b.StartsWith("border_gradient:")||b.StartsWith("border_gradient(")){
q(d,"border_gradient");string bQ=b.StartsWith("border_gradient:")? b.Substring("border_gradient:".Length): b.Substring("border_gradient".Length);
bQ=bQ.Trim();if(bQ.StartsWith("(")&&bQ.Contains(")"))bQ=ax(bQ,0);string[]m8=ay(bQ).Select(ra=>ra.Trim()).ToArray();if(p!=null)p.gd=m8;
else d.al=m8;}else if(b.StartsWith("title_align:")){q(d,"title_align:");string ng=b.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(p!=null)p.gC=ng;else d.kc=ng;}else if(b.StartsWith("title_color:")){q(d,"title_color:");string nj=b.Substring("title_color:".Length).Trim();
if(p!=null)p.gD=nj;else d.kd=nj;}else if(b.StartsWith("font_size:")){q(d,"font_size:");float ne=float.Parse(b.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(p!=null)p.jK=ne;else d.b7=ne;}else if(b.StartsWith("font:")){q(d,"font:");string lp=b.Substring("font:".Length).Trim();
if(p!=null)p.jI=lp;else d.jJ=lp;}else if(b.StartsWith("bg:")||b.StartsWith("background:")){q(d,"bg:");string ck=b.Substring(b.IndexOf(':')+1).Trim();
a3 g2=p!=null ?(a3)p : d;if(ck.StartsWith("color(")&&ck.EndsWith(")")){string kL;g2.aD=j0(ck.Substring(6,ck.Length-7),out kL);
string kJ,kK;j5(kL,out kJ,out kK);g2.f6=kJ;g2.f7=kK;}else if(p!=null)p.cS=ck;
else d.bz=ck;}else if(b.StartsWith("layout:")){q(d,"layout:");ol(b.Substring("layout:".Length).Trim(),d);}else if(b.StartsWith("alert_period:")){
q(d,"alert_period:");d.f3=Math.Max(0.2f,float.Parse(b.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(b.StartsWith("refresh:")){q(d,"refresh:");d.gy=int.Parse(b.Substring("refresh:".Length).Trim());}else if(b.StartsWith("output:")){
q(d,"output:");var fs=ay(b.Substring("output:".Length).Trim());if(fs.Count==0)throw new FormatException("output: requires a target block name");
d.ba=ab(fs[0]);d.bH=fs.Count>1 ? int.Parse(fs[1]): 0;}else if(b.StartsWith("width:")){q(d,"width:");
d.kg=int.Parse(b.Substring("width:".Length).Trim());}else if(b.StartsWith("boot_msg:")||b.StartsWith("boot:")){q(d,"boot_msg:");
d.jz=ab(b.Substring(b.IndexOf(':')+1).Trim());}else if(b.StartsWith("boot_stagger:")){q(d,"boot_stagger:");d.jA=int.Parse(b.Substring("boot_stagger:".Length).Trim());
}else if(b.StartsWith("boot_batch:")){q(d,"boot_batch:");d.jx=Math.Max(1,int.Parse(b.Substring("boot_batch:".Length).Trim()));
}else if(b.StartsWith("boot_duration:")){q(d,"boot_duration:");string dt=b.Substring("boot_duration:".Length).Trim().ToLowerInvariant();
bool lU=dt.EndsWith("s");if(lU)dt=dt.Substring(0,dt.Length-1).Trim();float hp;if(float.TryParse(dt,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out hp)){
d.gb=lU ? hp : hp/60f;}}else if(b.StartsWith("boot_log:")){q(d,"boot_log:");d.ei.Add(ab(b.Substring("boot_log:".Length).Trim()));
}else if(b.StartsWith("banner:")){q(d,"banner:");d.bA=ab(b.Substring("banner:".Length).Trim());}else if(b.StartsWith("boot_logo:")){
q(d,"boot_logo:");d.bE=ab(b.Substring("boot_logo:".Length).Trim());}else if(b.StartsWith("boot_gradient:")||b.StartsWith("boot_gradient(")||b.StartsWith("banner_gradient:")||b.StartsWith("banner_gradient(")){
q(d,"boot_gradient");d.eh=j1(b);}else if(b.StartsWith("bg_gradient:")||b.StartsWith("bg_gradient(")){q(d,"bg_gradient");
(p!=null ?(a3)p : d).bB=j1(b);}else if(b.StartsWith("bg_gradient_dir:")){q(d,"bg_gradient_dir:");
(p!=null ?(a3)p : d).cX=b.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(b.StartsWith("flash:")){
q(d,"flash:");a3 hw=p!=null ?(a3)p : d;string hv=b.Substring("flash:".Length).Trim();string lr=j8(hv);
string lq,ls;j5(hv.Substring(0,hv.Length-lr.Length),out lq,out ls);hw.gj=lq;hw.gl=ls;hw.b5=new List<aw>{
new aw{gg=lr,bF="on"}};}else if(b.StartsWith("bg_texture:")){q(d,"bg_texture:");d.f8=ab(b.Substring("bg_texture:".Length).Trim());
}else if(b.StartsWith("bg_texture_color:")){q(d,"bg_texture_color:");d.jw=b.Substring("bg_texture_color:".Length).Trim();
}else if(b.StartsWith("gradient_mode:")){q(d,"gradient_mode:");d.gn=b.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(b.StartsWith("panel_bg:")){q(d,"panel_bg:");d.gw=b.Substring("panel_bg:".Length).Trim();}else if(b.StartsWith("alert_bg:")){
q(d,"alert_bg:");d.ju=b.Substring("alert_bg:".Length).Trim();}else if(b.StartsWith("boot_color:")||b.StartsWith("banner_color:")){
q(d,"boot_color:");d.jy=b.Substring(b.IndexOf(':')+1).Trim();}else if(b.StartsWith("orientation:")){q(d,"orientation:");
string qA=b.Substring("orientation:".Length).Trim().ToLowerInvariant();if(p!=null)p.jY=qA;}else if(b.StartsWith("@panel")){q(d,"@panel");
p=oo(b);d.aH.Add(p);}else if(b.StartsWith("title:")){q(d,"title:");if(p==null){p=new at{
c8="main",c0=0,da=0,db=d.b9,dc=d.ca};d.aH.Add(p);}p.bL=ab(b.Substring("title:".Length).Trim());
}else if(b.StartsWith("line:")){q(d,"line:");if(p==null){p=new at{c8="main",c0=0,da=0,db=d.b9,dc=d.ca}
;d.aH.Add(p);}p.aT.Add(om(b.Substring("line:".Length).Trim()));}else{ai.aG.Add($"Line {dD + 1}: unrecognized directive: \"{b}\"");
}}catch(Exception pG){ai.aG.Add($"Line {dD + 1}: parse error ({pG.Message}) in \"{b}\"");}}foreach(var mP in ai.V){if(string.IsNullOrEmpty(mP.ba))ai.aG.Add($"@screen {mP.a9}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aN in ai.V){if(aN.bA!=null&& !ai.cT.ContainsKey(aN.bA))ai.aG.Add("Screen '"+aN.a9+"': banner '"+aN.bA+"' is not defined (@banner "+aN.bA+" ... @end)");
if(aN.bE!=null&& !ai.cT.ContainsKey(aN.bE))ai.aG.Add("Screen '"+aN.a9+"': boot_logo '"+aN.bE+"' is not defined (@banner "+aN.bE+" ... @end)");
}return ai;}static void ol(string g6,C mW){int iy=g6.IndexOf('(');if(iy<0)throw new FormatException("missing '(' in layout: specification");
string l0=g6.Substring(0,iy).Trim().ToLowerInvariant();string oZ=ax(g6,iy);var gT=ay(oZ);if(l0=="grid"){if(gT.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
mW.b9=Math.Max(1,int.Parse(gT[0]));mW.ca=Math.Max(1,int.Parse(gT[1]));}else{throw new FormatException($"unknown layout kind '{l0}' (expected grid)");
}}static at oo(string qq){string dX=qq.Substring("@panel".Length).Trim();int eK=dX.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(eK<0)throw new FormatException("@panel requires an at(...) positioning clause");
string qy=dX.Substring(0,eK).Trim();string kz=dX.Substring(eK);int mz=kz.IndexOf('(');string kx=ax(kz,mz);
var bP=ay(kx);if(bP.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var a_=new at{c8=qy,c0=int.Parse(bP[0]),da=int.Parse(bP[1]),db=bP.Count>2 ? int.Parse(bP[2]): 1,dc=bP.Count>3 ? int.Parse(bP[3]): 1}
;int kq=eK+mz+kx.Length+1;if(kq<dX.Length){string av=dX.Substring(kq).Trim();int no=av.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(no>=0){string d2=av.Substring(no+5).Trim();if(d2.StartsWith("(")&&d2.Contains(")"))a_.bL=ab(ax(d2,0));else if(d2.StartsWith(":")){
string bR=d2.Substring(1).Trim();if(bR.StartsWith("\"")){int ll=bR.IndexOf('"',1);a_.bL=ll>0 ? bR.Substring(1,ll-1): ab(bR);}else{int m4=bR.IndexOf(' ');
a_.bL=m4>=0 ? bR.Substring(0,m4): bR;}}}string mo=em(av,"orientation:");if(mo!=null)a_.jY=mo.ToLowerInvariant();string kp=em(av,"title_align:");
if(kp!=null)a_.gC=kp.ToLowerInvariant();string nl=em(av,"title_color:");if(nl!=null)a_.gD=nl;int eP=av.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(eP>=0&& !av.Substring(eP).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !av.Substring(eP).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
a_.jB=em(av.Substring(eP),"border:");}int eN=av.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(eN<0)eN=av.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(eN>=0){string bk=av.Substring(eN+"border_gradient".Length).Trim();if(bk.StartsWith(":"))bk=bk.Substring(1).Trim();if(bk.StartsWith("(")&&bk.Contains(")"))bk=ax(bk,0);
a_.gd=ay(bk).Select(rb=>rb.Trim()).ToArray();}}return a_;}static string em(string mQ,string l_){int lT=mQ.IndexOf(l_,StringComparison.OrdinalIgnoreCase);
if(lT<0)return null;string jo=mQ.Substring(lT+l_.Length).Trim();int m5=jo.IndexOf(' ');return m5>=0 ? jo.Substring(0,m5): jo;}static string[]j1(string ic){
int hI=ic.IndexOfAny(new[]{':','('});string dz=ic.Substring(ic[hI]==':' ? hI+1 : hI).Trim();if(dz.StartsWith("(")&&dz.Contains(")"))dz=ax(dz,0);return ay(dz).Select(rc=>rc.Trim()).ToArray();
}static List<string>oq(string pz,List<string>ht,Dictionary<string,List<string>>pc){var nr=new Dictionary<string,bd>(StringComparer.OrdinalIgnoreCase);
var cE=(pz ?? "").Replace("\r\n","\n").Split('\n');var it=new List<string>();int cw=0;while(cw<cE.Length){string iL=cE[cw++];if(iL.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ae=new List<string>();while(cw<cE.Length){string kn=cE[cw++].Replace("\t","    ").TrimEnd();if(kn.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ae.Add(kn);}while(ae.Count>0&&ae[0].Length==0)ae.RemoveAt(0);while(ae.Count>0&&ae[ae.Count-1].Length==0)ae.RemoveAt(ae.Count-1);int hY=int.MaxValue;
foreach(var gM in ae)if(gM.Length>0)hY=Math.Min(hY,gM.Length-gM.TrimStart().Length);for(int dK=0;dK<ae.Count;dK++)if(ae[dK].Length>0)ae[dK]=ae[dK].Substring(hY);
pc[iL.TrimStart().Substring(8).Trim()]=ae;continue;}string bo=j7(iL).Trim();if(bo.Length==0)continue;if(bo.StartsWith("@template")){try{
var je=op(bo);while(cw<cE.Length){string pm=cE[cw++];string g7=j7(pm).Trim();if(g7.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(g7.Length>0)je.ga.Add(g7);}nr[je.gu]=je;}catch(Exception pH){ht.Add($"Template parse error ({pH.Message}) in \"{bo}\"");}}
else if(bo.StartsWith("@use")){try{jF(bo,nr,it,ht);}catch(Exception pI){ht.Add($"@use error ({pI.Message}) in \"{bo}\"");
}}else{it.Add(bo);}}return it;}static bd op(string qr){string iM=qr.Substring("@template".Length).Trim();int iz=iM.IndexOf('(');
if(iz<0)throw new FormatException("missing '(' in @template header");string qz=iM.Substring(0,iz).Trim();string o_=ax(iM,iz);var jf=new bd{
gu=qz};foreach(var qE in ay(o_)){var h7=qE.Split(new[]{'='},2);string mu=gv(h7[0]);jf.et.Add(mu);if(h7.Length>1)jf.gf[mu]=ab(h7[1].Trim());
}return jf;}static void jF(string qs,Dictionary<string,bd>ns,List<string>mr,List<string>pF,int la=0){string fA=qs.Substring("@use".Length).Trim();
int fq=fA.IndexOf('(');string ik=fq>=0 ? fA.Substring(0,fq).Trim(): fA.Trim();string o0=fq>=0 ? ax(fA,fq): "";bd cM=null;
if(!ns.TryGetValue(ik,out cM)){if(!jC.TryGetValue(ik,out cM))throw new FormatException($"unknown template '{ik}'");}var eR=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var l4 in cM.gf)eR[l4.Key]=l4.Value;var kw=ay(o0);for(int dk=0;dk<kw.Count;dk++){string eI=kw[dk];int hr=eI.IndexOf('=');if(hr>=0){
string qa=gv(eI.Substring(0,hr));string rM=ab(eI.Substring(hr+1).Trim());eR[qa]=rM;}else{if(dk<cM.et.Count){string qb=cM.et[dk];
eR[qb]=ab(eI.Trim());}}}foreach(var pl in cM.ga){string dw=pl;foreach(var fg in eR){dw=dw.Replace("$"+fg.Key,fg.Value).Replace("{"+fg.Key+"}",fg.Value);
}if(dw.StartsWith("@use")){if(la>3)throw new FormatException("@use nested too deep");jF(dw,ns,mr,pF,la+1);
}else mr.Add(dw);}}static void q(C px,string pC){if(px==null)throw new FormatException($"'{pC}' used before any @screen declared");
}static string j7(string aM){if(string.IsNullOrEmpty(aM))return aM;bool hX=false;for(int bn=0;bn<aM.Length;bn++){char ha=aM[bn];if(ha=='"'){hX= !hX;
}else if(!hX){if(ha=='/'&&bn+1<aM.Length&&aM[bn+1]=='/'){return aM.Substring(0,bn);}if(ha=='#'){if(ob(aM,bn)){continue;}return aM.Substring(0,bn);}}}return aM;
}static bool ob(string bU,int lW){int cz=0;for(int hT=lW+1;hT<bU.Length;hT++){char cn=bU[hT];if((cn>='0'&&cn<='9')||(cn>='a'&&cn<='f')||(cn>='A'&&cn<='F'))cz++;
else break;}if(cz==3||cz==4||cz==6||cz==8){int dN=lW+1+cz;if(dN>=bU.Length||char.IsWhiteSpace(bU[dN])||bU[dN]==')'||bU[dN]==','||bU[dN]==':')return true;
}return false;}static aR ok(string qt){string iN=qt.Substring("@bind".Length).Trim();int hs=iN.IndexOf('=');if(hs<0)throw new FormatException("missing '=' in @bind");
string mm=iN.Substring(0,hs).Trim();string hu=iN.Substring(hs+1).Trim();int iA=hu.IndexOf('(');if(iA<0)throw new FormatException("missing '(' in @bind target");
string e2=hu.Substring(0,iA).Trim().ToLowerInvariant();string o1=ax(hu,iA);var bh=ay(o1);if(bh.Count==0)throw new FormatException("@bind target has no arguments");
var ag=new aR{U=mm};if(e2=="dynamic"||e2=="var"||e2=="mem"){ag.a8=aE.c3;ag.aS=bh.Count>0 ? ab(bh[0].Trim()): mm;
for(int gN=0;gN<bh.Count;gN++){var fh=bh[gN].Split(new[]{':'},2);if(fh.Length==2&&fh[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double k9;
if(double.TryParse(fh[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out k9))ag.el=k9;else ag.ge=ab(fh[1].Trim());
}}return ag;}ag.a8=e2=="group" ? aE.go : aE.nM;string eG=null;for(int eF=0;eF<bh.Count;eF++){var cy=bh[eF].Split(new[]{':'},2);if(cy.Length==2&&cy[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
eG=cy[1].Trim().ToLowerInvariant();}else if(cy.Length==2&&cy[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ag.ed=cy[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ag.az.Add(ab(bh[eF].Trim()));}}if(ag.az.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ag.a8==aE.go){if(eG==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(eG){case "avg": ag.cR=ar.f4;break;case "sum": ag.cR=ar.j9;break;case "min": ag.cR=ar.jX;break;case "max": ag.cR=ar.jW;
break;default: throw new FormatException($"unknown agg '{eG}' (expected avg/sum/min/max)");}}return ag;}static bG om(string eO){var o=new bG();
if(!eO.StartsWith("\""))throw new FormatException("line must start with a quoted label");int hq=eO.IndexOf('"',1);if(hq<0)throw new FormatException("unterminated quoted label");
o.S=eO.Substring(1,hq-1);string dW=eO.Substring(hq+1).Trim();int fr=dW.IndexOf('(');if(fr<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string z=dW.Substring(0,fr).Trim().ToLowerInvariant();string oY=ax(dW,fr);string Z=dW.Substring(dW.IndexOf(')',fr)+1).Trim();
var hn=oY.Trim().Split('.');if(hn.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.a4=hn[0].Trim();o.N=hn[1].Trim();if(z=="bar")o.g=e.cU;else if(z=="led")o.g=e.c6;else if(z=="spark"||z=="sparkline")o.g=e.bK;
else if(z=="hazard"||z=="stripes")o.g=e.c5;else if(z=="chevron"||z=="chevrons")o.g=e.c_;else if(z=="dots"||z=="dot")o.g=e.c2;
else if(z=="balance"||z=="horizon"||z=="centered")o.g=e.b2;else if(z=="toggle"||z=="switch")o.g=e.ce;else if(z=="checkbox"||z=="check")o.g=e.b4;
else if(z=="status"||z=="badge"||z=="state")o.g=e.cd;else if(z=="ammo"||z=="pips")o.g=e.b1;else if(z=="flow"||z=="rate"||z=="spinner")o.g=e.b6;
else if(z=="text")o.g=e.kb;else throw new FormatException($"unknown display function '{z}'");while(Z.Length>0){int hl=Z.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int ct=Z.IndexOf('(');if(hl>=0&&(ct<0||hl<ct)){string eU=Z.Substring(hl+4).Trim();int fI=eU.IndexOf(' ');o.jD=(fI>=0 ? eU.Substring(0,fI): eU).ToLowerInvariant();
Z=(fI>=0 ? eU.Substring(fI): "").Trim();continue;}int jm=Z.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(jm>=0&&(ct<0||jm<ct)){
string f_=Z.Substring(jm+5).Trim();int fJ=f_.IndexOf(' ');o.de=(fJ>=0 ? f_.Substring(0,fJ): f_).Trim().Trim('"');Z=(fJ>=0 ? f_.Substring(fJ): "").Trim();
continue;}int hC=Z.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(hC>=0&&(ct<0||hC<ct)){string eY=Z.Substring(hC+7).Trim();
int fK=eY.IndexOf(' ');o.c4=(fK>=0 ? eY.Substring(0,fK): eY).Trim().Trim('"');Z=(fK>=0 ? eY.Substring(fK): "").Trim();continue;}int fk=Z.IndexOf('(');
if(fk<0)break;string aj=Z.Substring(0,fk).Trim().ToLowerInvariant();int l9=aj.LastIndexOf(' ');if(l9>=0)aj=aj.Substring(l9+1).Trim();
string aY=ax(Z,fk);Z=Z.Substring(Z.IndexOf(')',fk)+1).Trim();if(aj=="gradient"){o.b8=true;o.a7=ay(aY).Select(rd=>rd.Trim()).ToArray();
if(o.a7.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(aj=="flat"){o.am=aY.Trim();}else if(aj=="pattern"){
string M=aY.Trim().ToLowerInvariant();if(M=="hazard"||M=="stripes")o.g=e.c5;else if(M=="chevron"||M=="chevrons")o.g=e.c_;
else if(M=="dots"||M=="dot")o.g=e.c2;else if(M=="led")o.g=e.c6;else if(M=="balance"||M=="horizon")o.g=e.b2;
else if(M=="toggle"||M=="switch")o.g=e.ce;else if(M=="checkbox"||M=="check")o.g=e.b4;else if(M=="status"||M=="badge"||M=="state")o.g=e.cd;
else if(M=="ammo"||M=="pips")o.g=e.b1;else if(M=="flow"||M=="rate"||M=="spinner")o.g=e.b6;}else if(aj=="color"){string p3;
o.c1=j0(aY,out p3);}else if(aj=="dir"||aj=="direction"){o.jD=aY.Trim().ToLowerInvariant();}else if(aj=="unit"){
o.de=aY.Trim().Trim('"');}else if(aj=="max"){double mh;if(double.TryParse(aY.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mh))o.es=mh;
else{var ih=aY.Trim().Split('.');if(ih.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.cc=ih[0].Trim();o.gt=ih[1].Trim();
}}else if(aj=="format"||aj=="fmt"){o.c4=aY.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{aj}'");}}return o;}static string ax(string fE,int iq){
int hk=0;for(int dE=iq;dE<fE.Length;dE++){if(fE[dE]=='(')hk++;else if(fE[dE]==')'){hk--;if(hk==0)return fE.Substring(iq+1,dE-iq-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>ay(string o2){return o2.Split(',').Select(re=>re.Trim()).Where(rf=>rf.Length>0).ToList();
}static List<aw>j0(string oX,out string hy){var fC=new List<aw>();hy=null;foreach(var iB in ay(oX)){int hh=iB.LastIndexOf(':');
if(hh<0)throw new FormatException("color() entries need 'expr : color'");string fi=iB.Substring(0,hh).Trim();string k4=iB.Substring(hh+1).Trim();
if(fi.Equals("default",StringComparison.OrdinalIgnoreCase)){fC.Add(new aw{cb=true,bF=k4});}else{string m9=j8(fi);
if(hy==null)hy=fi.Substring(0,fi.Length-m9.Length).Trim();fC.Add(new aw{gg=m9,bF=k4});}}if(!fC.Any(q1=>q1.cb))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return fC;}static void j5(string q2,out string pi,out string pL){var dQ=(q2 ?? "").Split('.');if(dQ.Length!=2||dQ[0].Trim().Length==0||dQ[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
pi=dQ[0].Trim();pL=dQ[1].Trim();}static string j8(string cs){cs=cs.Trim();foreach(var qB in new[]{">=","<=","==",">","<"}){int hU=cs.IndexOf(qB,StringComparison.Ordinal);
if(hU>0)return cs.Substring(hU).Trim();if(hU==0)return cs;}throw new FormatException($"color() rule has no recognizable comparison operator: '{cs}'");}
static string ab(string bV){if(bV.Length>=2&&bV[0]=='"'&&bV[bV.Length-1]=='"')return bV.Substring(1,bV.Length-2);return bV;}}public static class nK{public static double nJ(ar oT,List<double>cO){
if(cO==null||cO.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(oT){case ar.f4: return cO.Average();case ar.j9: return cO.Sum();
case ar.jX: return cO.Min();case ar.jW: return cO.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class E{
public static string oA(string qk,double ft,int kD=10,string ny=null){ft=Math.Max(0,Math.Min(100,ft));int ly=(int)Math.Round(kD*(ft/100.0));
string pd=new string('#',ly)+new string('-',kD-ly);string rN=ny!=null ? ny : $"{ft:0}%";return $"{qk} [{pd}] {rN}";
}public static string n4(double bx,string jl=null,string e1=null){if(jl=="auto"){double gO=Math.Abs(bx);if(gO>=1e9)return(bx/1e9).ToString("F1")+"G";
if(gO>=1e6)return(bx/1e6).ToString("F1")+"M";if(gO>=1e3)return(bx/1e3).ToString("F1")+"k";string pJ= !string.IsNullOrEmpty(e1)? e1 : "F0";return bx.ToString(pJ);
}string lA= !string.IsNullOrEmpty(e1)? e1 :(Math.Abs(bx)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(jl)){return bx.ToString(lA);}return bx.ToString(lA)+jl;
}public static float n6(string qS,float nd,float jc,float eV=1.0f){string cC=(qS ?? "standard").ToLowerInvariant();
if(cC=="compact"||(cC=="auto"&&nd<=300&&jc<=300&&jc>160)){return eV*0.75f;}if(cC=="corner"||(cC=="auto"&&jc<=160)){
return eV*0.55f;}if(cC=="wide"||(cC=="auto"&&nd>=1000)){return eV*1.15f;}return eV;}public static string oy(string ql,double fu,int bl=5){
fu=Math.Max(0,Math.Min(100,fu));double eW=(fu-50.0)/50.0;int dL=0;int dZ=0;if(eW< -0.01)dL=(int)Math.Round(-eW*bl);
else if(eW>0.01)dZ=(int)Math.Round(eW*bl);if(dL>bl)dL=bl;if(dZ>bl)dZ=bl;string qo=new string('-',bl-dL)+new string('<',dL);
string q7=new string('>',dZ)+new string('-',bl-dZ);return $"{ql} [{qo}|{q7}] {fu:0}%";}public static string oJ(string l6,double rP){
return rP>=50.0 ? $"{l6} [====O] ON" : $"{l6} [O====] OFF";}public static string oD(string l7,double rQ){return rQ>=50.0 ? $"{l7} [X] ACTIVE" : $"{l7} [ ] READY ";
}public static string oI(string h9,double nx){if(nx>=75.0)return $"{h9} [ ONLINE  ]";if(nx>=25.0)return $"{h9} [ STANDBY ]";return $"{h9} [ OFFLINE ]";
}public static string ox(string qm,double fv,int md=8){fv=Math.Max(0,Math.Min(100,fv));int lz=(int)Math.Round(md*(fv/100.0));
string qP=new string('|',lz)+new string('.',md-lz);return $"{qm} [{qP}] {fv:0}%";}public static string oE(string ia,double dU,int nn=0){
char[]rq=new[]{'|','/','-','\\'};char mR=rq[(nn>=0 ? nn : 0)%4];if(dU>55.0)return $"{ia} {mR} ▲ CHG {dU:0}%";if(dU<45.0)return $"{ia} {mR} ▼ DSC {dU:0}%";
return $"{ia} ● STBY {dU:0}%";}public static bool of(string ak,string dH){if(ak==null||dH==null)return false;if(ak=="*"||ak==dH)return true;
if(!ak.Contains("*")&& !ak.Contains("?"))return string.Equals(ak,dH,StringComparison.OrdinalIgnoreCase);int ap=0,dF=0,i5= -1,ig=0;while(dF<dH.Length){
if(ap<ak.Length&&(ak[ap]=='?'||char.ToLowerInvariant(ak[ap])==char.ToLowerInvariant(dH[dF]))){ap++;dF++;}else if(ap<ak.Length&&ak[ap]=='*'){
i5=ap;ig=dF;ap++;}else if(i5!= -1){ap=i5+1;ig++;dF=ig;}else{return false;}}while(ap<ak.Length&&ak[ap]=='*')ap++;return ap==ak.Length;
}public static int nO(double qF,int nq){return Math.Max(0,Math.Min(nq,1+(int)(Math.Max(0.0,qF)*nq)));}public static string eo(string[]cL,double iC){
if(cL==null||cL.Length==0)return "white";iC=Math.Max(0,Math.Min(100,iC));int hV=(int)Math.Floor(iC/100.0*cL.Length);if(hV>=cL.Length)hV=cL.Length-1;
return cL[hV];}public static n jM(string[]bv,double qM){if(bv==null||bv.Length==0)return n.ey;if(bv.Length==1)return n.c9(bv[0]);
double q5=Math.Max(0.0,Math.Min(1.0,qM/100.0));double mV=q5*(bv.Length-1);int hW=(int)Math.Floor(mV);int p2=Math.Min(bv.Length-1,hW+1);
double i9=mV-hW;n co=n.c9(bv[hW]);n hb=n.c9(bv[p2]);byte q3=(byte)Math.Round(co.bb+(hb.bb-co.bb)*i9);
byte pY=(byte)Math.Round(co.a6+(hb.a6-co.a6)*i9);byte pb=(byte)Math.Round(co.a2+(hb.a2-co.a2)*i9);return new n(q3,pY,pb);}public static string eu(List<aw>mN,double rR){
foreach(var iP in mN){if(iP.cb)continue;if(n2(iP.gg,rR))return iP.bF;}var k8=mN.FirstOrDefault(q4=>q4.cb);return k8!=null ? k8.bF : null;
}static bool n2(string dx,double d8){dx=dx.Trim();foreach(var ip in new[]{">=","<=","==",">","<"}){if(dx.StartsWith(ip)){double dY=double.Parse(dx.Substring(ip.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(ip){case ">=": return d8>=dY;case "<=": return d8<=dY;case "==": return Math.Abs(d8-dY)<1e-9;case ">": return d8>dY;case "<": return d8<dY;
}}}throw new FormatException($"unsupported comparison expression: '{dx}'");}public static List<string>oC(List<string>pw,int cP){
var fm=new List<string>();fm.Add("+"+new string('-',cP+2)+"+");foreach(var id in pw){string rI=id.Length>cP ? id.Substring(0,cP): id;
string qG=rI.PadRight(cP);fm.Add("| "+qG+" |");}fm.Add("+"+new string('-',cP+2)+"+");return fm;}public struct j_{public float ki,kj,kh,jO;
}public static j_ nT(int k2,int mK,int fL,int fM,int d4,int d5,float rz,float ry,float mv,float mw,float hK,float hL){
d4=Math.Max(1,d4);d5=Math.Max(1,d5);fL=Math.Max(1,Math.Min(d4-k2,fL));fM=Math.Max(1,Math.Min(d5-mK,fM));
float o4=rz-(mv*2f);float o3=ry-(mw*2f);float kY=(o4-(hK*(d4-1)))/d4;float kX=(o3-(hL*(d5-1)))/d5;
float qU=mv+k2*(kY+hK);float qV=mw+mK*(kX+hL);float qT=fL*kY+((fL-1)*hK);float qN=fM*kX+((fM-1)*hL);return new j_{
ki=qU,kj=qV,kh=qT,jO=qN};}public struct j3{public int oi;public int gr;public int W;public int an;public int dd;
public int aF;public int oN;public int oO;public int n_;public int n0;
public double n1;}public static j3 nU(C bt){var j=new j3();var my=bt.aH!=null&&bt.aH.Count>0 ? bt.aH : new List<at>{
new at{aT=bt.gs}};j.oi=my.Count;j.gr=0;j.W=0;j.an=0;if(!string.IsNullOrEmpty(bt.bz)&& !bt.bz.Equals("none",StringComparison.OrdinalIgnoreCase))j.W++;
if(!string.IsNullOrEmpty(bt.cZ)&& !bt.cZ.Equals("none",StringComparison.OrdinalIgnoreCase))j.W+=2;foreach(var dR in my){
if(!string.IsNullOrEmpty(dR.cS)&& !dR.cS.Equals("none",StringComparison.OrdinalIgnoreCase))j.W++;if(!string.IsNullOrEmpty(dR.bL))j.W+=2;
j.gr+=dR.aT.Count;foreach(var qj in dR.aT){j.oN++;j.oO++;j.W+=2;switch(qj.g){case e.cU: j.dd++;
j.aF++;j.W+=2;j.an++;break;case e.c6: j.aF+=10;j.W+=10;
j.an+=10;break;case e.c5: j.aF+=10;j.W+=11;j.an+=10;break;case e.c_: j.aF+=10;
j.W+=10;j.an+=10;break;case e.c2: j.aF+=12;j.W+=12;j.an+=12;
break;case e.bK: j.dd++;j.aF+=8;j.W+=9;j.an+=8;break;case e.b2: j.dd+=2;
j.aF++;j.W+=3;j.an++;break;case e.ce: case e.b4: j.dd++;
j.aF++;j.W+=2;j.an++;break;case e.cd: case e.b6: j.dd++;j.aF+=2;
j.W+=3;j.an+=2;break;case e.b1: j.aF+=8;j.W+=8;j.an+=8;
break;default: j.an++;break;}}}j.n_=25+(j.gr*2);j.n0=180+(j.an*4);
j.n1=0.0;return j;}}