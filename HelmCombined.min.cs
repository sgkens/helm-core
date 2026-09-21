gB v;string gO=null;Dictionary<string,List<IMyTerminalBlock>>ck=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>eF=new Dictionary<string,bool>();
Dictionary<string,aS>dg=new Dictionary<string,aS>();Dictionary<string,Color>kn=new Dictionary<string,Color>();List<IMyTerminalBlock>bR=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>cj=new List<IMyTerminalBlock>();bool eB;int gM;string eD="";StringBuilder dj=new StringBuilder();readonly Dictionary<string,double>au=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>eE=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener dh;bool bS=false;
Dictionary<y,IMyTextSurface>bT=new Dictionary<y,IMyTextSurface>();Dictionary<y,string>ci=new Dictionary<y,string>();
static readonly string[]j2=new string[101];static Program(){for(int e8=0;e8<=100;e8++){j2[e8]=e8+"%";}}static string jP(double rU){int dH=(int)Math.Round(rU);
if(dH<0)dH=0;else if(dH>100)dH=100;return j2[dH];}class aV{public List<MySprite>l=new List<MySprite>();public List<c8>ey=new List<c8>();
public int bD= -1;public Color[]eh;public Color ec;public bool ed;public Vector2 jX;public bool jV=false;public List<ei>cZ=new List<ei>();
}class ei{public int er;public Color f8,em;public List<aw>ex,ep;public string gc,gl,gm,go;public bool jW,gq;
}class c8{public bJ jZ;public int aa= -1;public int bf= -1;public int gu= -1;public bool jR=false;
public List<int>ad=new List<int>();public float[]ez=new float[8];public float cX;public float eg;public float b9;
public float cW;public double et= -99999.0;public string jY;public float Q= -1f;}Dictionary<y,aV>eH=new Dictionary<y,aV>();
int kp=0;double gQ=0;double aA=0;int di=0;Dictionary<y,double>aW=new Dictionary<y,double>();Dictionary<y,double>gL=new Dictionary<y,double>();
Dictionary<y,double>eC=new Dictionary<y,double>();int dk=0;const string og="0.11.0";IMyTextSurface bg;bool gK=true;
double gJ=0;string ko="";string gP="";int eG=0,gN=0;List<KeyValuePair<string,int>>ch=new List<KeyValuePair<string,int>>();
const double oA=120;const float nO=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{dh=IGC.RegisterBroadcastListener("HELM");
dh.SetMessageCallback("HELM_IGC");}catch{}gD();ka();jU();}public void Main(string kB,UpdateType rQ){
double ln=Runtime.TimeSinceLastRun.TotalSeconds;aA+=ln;if((rQ&UpdateType.IGC)!=0||kB=="HELM_IGC"){oy();}string mN=(kB ?? "").Trim();
string ao=mN.ToLowerInvariant();if(ao=="reload"){gD();}else if(ao=="refresh"){gE();}else if(ao=="boot"){ka();}else if(ao=="console on"||ao=="console off"){
gK=ao=="console on";jU();gJ=0;}else if(ao=="scan"||ao=="diagnose"||ao=="test"){oS();return;}else if(ao.StartsWith("set ")){
string fC=mN.Substring(4).Trim();int lr=fC.IndexOf('=');int rt=fC.IndexOf(' ');int i4=lr>0 ? lr : rt;if(i4>0){string l3=fC.Substring(0,i4).Trim();
string nz=fC.Substring(i4+1).Trim();jz($"{l3}={nz}");Echo($"[HELM] Set dynamic variable '{l3}' = {nz}");}return;}else if(ao.StartsWith("alert")){
string d9=ao.Length>5 ? ao.Substring(5).Trim(): "toggle";if(d9=="toggle")bS= !bS;else bS=d9=="red"||d9=="on"||d9=="1"||d9=="true";
Echo($"[HELM] Red alert state: {bS}");return;}else if(Me.CustomData!=gO){gD();}dk++;gQ+=ln;if(gQ>=oA){
gQ=0;gE();}if(v==null||v.V.Count==0){ki();jI();Echo("HELM: no valid @screen configured. Check Custom Data.");
return;}var b1=v.V[kp%v.V.Count];kp++;gP=b1.ba;bool gU=aW.Count>0;if(gU)oH();
double lo;if(!aW.ContainsKey(b1)&& !(gL.TryGetValue(b1,out lo)&&aA<lo)){if(oL(b1))gU=true;if(b1.gC>0)gL[b1]=aA+b1.gC/60.0;
}di=gU ? 0 : di+1;Runtime.UpdateFrequency=di<=v.V.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;ki();
jI();eG=Runtime.CurrentInstructionCount;if(eG>gN)gN=eG;}void jI(){if(eD.Length>0)Echo(eD);if(dj.Length>0)Echo(dj.ToString());
}List<IMyTerminalBlock>ee(bool o_=false){if(!eB){cj.Clear();GridTerminalSystem.GetBlocks(cj);bR.Clear();bR.AddRange(cj);
bR.RemoveAll(pb=> !pb.IsSameConstructAs(Me));gM=cj.Count-bR.Count;eB=true;}return o_ ? cj : bR;}IMyTerminalBlock jL(string mo,bool o1=false){
var g1=GridTerminalSystem.GetBlockWithName(mo);if(g1==null||o1||g1.IsSameConstructAs(Me))return g1;var gW=ee();for(int e9=0;e9<gW.Count;e9++)if(gW[e9].CustomName.IndexOf(mo,StringComparison.OrdinalIgnoreCase)>=0)return gW[e9];
return null;}void nY(string mp,List<IMyTerminalBlock>dJ,bool ku=false){var lI=GridTerminalSystem.GetBlockGroupWithName(mp);if(lI==null)return;int kK=dJ.Count;
var d5=new List<IMyTerminalBlock>();lI.GetBlocks(d5);jx(d5,dJ,ku);if(dJ.Count>kK)return;var lT=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(lT);
foreach(var nJ in lT){if(nJ.Name.IndexOf(mp,StringComparison.OrdinalIgnoreCase)<0)continue;d5.Clear();nJ.GetBlocks(d5);jx(d5,dJ,ku);if(dJ.Count>kK)return;
}}void jx(List<IMyTerminalBlock>pZ,List<IMyTerminalBlock>l1,bool o0=false){foreach(var g2 in pZ)if((o0||g2.IsSameConstructAs(Me))&& !l1.Contains(g2))l1.Add(g2);
}void gD(){gO=Me.CustomData;v=jT.oq(gO);eD="";if(v.aH.Count>0){var mX=new StringBuilder("HELM config errors:\n");
foreach(var pK in v.aH)mX.AppendLine(" - "+pK);eD=mX.ToString();}dg.Clear();foreach(var g3 in v.bE)if(!dg.ContainsKey(g3.U))dg[g3.U]=g3;
eH.Clear();ci.Clear();aW.Clear();gL.Clear();eC.Clear();gE();}void oy(){try{while(dh!=null&&dh.HasPendingMessage){
var mk=dh.AcceptMessage();if(mk.Data is string){jz((string)mk.Data);}}}catch{}}void jz(string mF){
if(string.IsNullOrEmpty(mF))return;var mC=mF.Split(';');for(int hT=0;hT<mC.Length;hT++){var dU=mC[hT].Trim();if(dU.Length==0)continue;int dw=dU.IndexOf('=');
if(dw<0)dw=dU.IndexOf(':');if(dw>0){string cz=dU.Substring(0,dw).Trim();string aR=dU.Substring(dw+1).Trim();if(cz.Equals("alert",StringComparison.OrdinalIgnoreCase)){
bS=aR.Equals("red",StringComparison.OrdinalIgnoreCase)||aR=="1"||aR.Equals("true",StringComparison.OrdinalIgnoreCase)||aR.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double ms;eE.Remove(cz);if(double.TryParse(aR,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out ms)){
au[cz]=ms;}else if(aR.Equals("true",StringComparison.OrdinalIgnoreCase)||aR.Equals("on",StringComparison.OrdinalIgnoreCase)){au[cz]=100.0;
}else if(aR.Equals("false",StringComparison.OrdinalIgnoreCase)||aR.Equals("off",StringComparison.OrdinalIgnoreCase)){au[cz]=0.0;}else{eE[cz]=aR;
}}}}}void ah(int rC,string rJ){ch.Add(new KeyValuePair<string,int>(rJ,rC));}static string kh(string iV,int mn){return iV.Length>mn ? iV.Substring(0,mn): iV;
}void jU(){var iN=Me as IMyTextSurfaceProvider;if(iN==null||iN.SurfaceCount<1)return;bg=iN.GetSurface(0);bg.ContentType=gK ? ContentType.SCRIPT : ContentType.NONE;
bg.Script="";}void ki(){if(aA>=gJ){gJ=aA+(di>v.V.Count+1 ? 5.0 : 1.0);nW();if(gK&&bg!=null)n4();
}Echo(ko);}void nW(){ch.Clear();int hd=0,kQ=0;foreach(var kD in v.bE){List<IMyTerminalBlock>id;if(kD.a9==aF.c4){
hd++;continue;}if(ck.TryGetValue(kD.U,out id)){kQ+=id.Count;if(id.Count>0)hd++;}}int ls=v.aH.Count;ah(0,"HELM-[Kernel-(v"+og+")]");
ah(4,"------------------------------");ah(1,"krn-cfg");ah(2," screens "+v.V.Count+"  binds "+hd+"/"+v.bE.Count+"  errors "+ls);
ah(2," alert "+(bS ? "RED" : "off")+"  blocks "+kQ+"  docked-ignored "+gM);if(ls>0)ah(3," ! "+kh(v.aH[0],40));
ah(1,"cfg-dsp");int rq=0;foreach(var iW in v.V){if(++rq>10){ah(2," +"+(v.V.Count-10)+" more");break;}IMyTextSurface i5;string m5="--",ja="no block";
if(bT.TryGetValue(iW,out i5)){m5=i5.SurfaceSize.X.ToString("0")+"x"+i5.SurfaceSize.Y.ToString("0");ja=aW.ContainsKey(iW)? "boot" : "live";
}ah(ja=="no block" ? 3 : 2," "+kh(iW.ba,14).PadRight(14)+" "+m5.PadRight(9)+" "+ja);}ah(1,"perf");ah(2," tick "+dk+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ah(2," instr "+eG+"  peak "+gN+"  max "+Runtime.MaxInstructionCount);ah(2," rate "+Runtime.UpdateFrequency+"  idle "+di+"  up "+(int)aA+"s");
if(gP.Length>0)ah(2," last "+gP);var mY=new StringBuilder();foreach(var qj in ch)mY.AppendLine(qj.Key);ko=mY.ToString();}static Color n0(int rD){
switch(rD){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void n4(){Vector2 b2=bg.SurfaceSize;Vector2 ir=(bg.TextureSize-b2)/2f;int hk=30;foreach(var qk in ch)hk=Math.Max(hk,qk.Key.Length);
Vector2 hj=bg.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float mZ=Math.Min(1f,Math.Min(b2.X*0.94f/(hk*hj.X),b2.Y*0.94f/(ch.Count*hj.Y)));
float r_=ir.X+b2.X*0.03f;float nL=ir.Y+b2.Y*0.03f;var i6=new List<MySprite>();i6.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=ir+b2/2f,Size=b2,Color=new Color(12,12,16)}
);foreach(var l8 in ch){cR(i6,l8.Key,n0(l8.Value),r_,nL,mZ);nL+=hj.Y*mZ;}using(var pU=bg.DrawFrame()){pU.AddRange(i6);}}static readonly List<string>nV=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void ka(){aW.Clear();if(v==null)return;for(int fa=0;fa<v.V.Count;fa++){var fG=v.V[fa];if(fG.gf>0f)aW[fG]=aA+(fa/Math.Max(1,fG.jB))*fG.jE/60.0;
}}void oH(){List<y>dv=null;int p4=(aW.Count+3)/4,qc=0;foreach(var fi in aW){double iz=(aA-fi.Value)/fi.Key.gf;if(iz<0)continue;
if(iz>=1.0){if(dv==null)dv=new List<y>();dv.Add(fi.Key);continue;}if((qc++ +dk)%p4==0)n3(fi.Key,iz);}if(dv==null)return;foreach(var d1 in dv){
aW.Remove(d1);if(d1.ge>0f)eC[d1]=aA;eH.Remove(d1);ci.Remove(d1);}}static void cR(List<MySprite>ru,string rK,Color pw,float r0,float r2,float rn){
var nl=MySprite.CreateText(rK,"Monospace",pw,rn,TextAlignment.LEFT);nl.Position=new Vector2(r0,r2);ru.Add(nl);}static void jw(List<MySprite>rv,List<string>eM,y fJ,float r1,float r3,float ro,float qv){
string[]rA=fJ.ej!=null&&fJ.ej.Length>0 ? fJ.ej : new[]{fJ.jC};for(int dC=0;dC<eM.Count;
dC++){n he=E.jQ(rA,eM.Count>1 ? 100.0*dC/(eM.Count-1): 0.0);cR(rv,eM[dC],new Color(he.bc,he.a7,he.a3),r1,r3+dC*qv,ro);}}bool oF(y cH,IMyTextSurface fQ){
List<string>dm;if(ci.ContainsKey(cH)|| !v.cU.TryGetValue(cH.bB,out dm))return false;ci[cH]="banner";Vector2 b3=fQ.SurfaceSize;
Vector2 it=(fQ.TextureSize-b3)/2f;int eV=1;foreach(var ql in dm)eV=Math.Max(eV,ql.Length);Vector2 dt=fQ.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float fI=Math.Min(b3.X*0.94f/(eV*dt.X),b3.Y*0.94f/(dm.Count*dt.Y));var i7=new List<MySprite>();i7.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=it+b3/2f,Size=b3,Color=string.IsNullOrEmpty(cH.bA)? new Color(10,12,16): D(cH.bA)}
);jw(i7,dm,cH,it.X+(b3.X-eV*dt.X*fI)/2f,it.Y+(b3.Y-dm.Count*dt.Y*fI)/2f,fI,dt.Y*fI);using(var pV=fQ.DrawFrame()){pV.AddRange(i7);
}return true;}void n3(y cI,double mx){IMyTextSurface d3;if(!bT.TryGetValue(cI,out d3))return;Vector2 bv=d3.SurfaceSize;
Vector2 iu=(d3.TextureSize-bv)/2f;var dN=cI.ob();string ml=cI.jD ?? "HELM TELEMETRY KERNEL ONLINE";bool ik=bv.Y>=200f;
List<string>cl;if(cI.bF==null|| !v.cU.TryGetValue(cI.bF,out cl))cl=nV;int cs=ml.Length+3;foreach(var qm in dN)cs=Math.Max(cs,qm.Length+5);
if(ik)foreach(var qn in cl)cs=Math.Max(cs,qn.Length);int mQ=(ik ? cl.Count+1 : 0)+2+dN.Count;Vector2 cr=d3.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bs=Math.Min(1f,Math.Min(bv.X*0.94f/(cs*cr.X),bv.Y*0.94f/(mQ*cr.Y)));float f4=iu.X+(bv.X-cs*cr.X*bs)/2f;float b6=iu.Y+(bv.Y-mQ*cr.Y*bs)/2f;
float fm=cr.Y*bs;var cK=new List<MySprite>();cK.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iu+bv/2f,Size=bv,Color=new Color(10,12,16)}
);if(ik){jw(cK,cl,cI,f4,b6,bs,fm);b6+=(cl.Count+1)*fm;}cR(cK,">> "+ml,new Color(34,197,94),f4,b6,bs);b6+=fm*2f;int rr=E.nU(mx,dN.Count);
for(int fb=0;fb<rr;fb++){bool mu=fb==dN.Count-1&&mx>=0.8;cR(cK,mu ? "[OK]" : "[..]",mu ? new Color(34,197,94): new Color(56,189,248),f4,b6,bs);cR(cK,dN[fb],new Color(203,213,225),f4+5f*cr.X*bs,b6,bs);
b6+=fm;}using(var pW=d3.DrawFrame()){pW.AddRange(cK);}}void gE(){ck.Clear();bT.Clear();eF.Clear();
dj.Clear();eB=false;if(v==null)return;foreach(var T in v.bE){var aB=new List<IMyTerminalBlock>();if(T.a9==aF.c4){
string h6= !string.IsNullOrEmpty(T.aT)? T.aT : T.U;if(!au.ContainsKey(h6)){au[h6]=T.en;if(T.gi!=null)eE[h6]=T.gi;
}ck[T.U]=aB;continue;}var no=T.az.Count>0 ? T.az : new List<string>{T.ke};foreach(var b5 in no){
if(string.IsNullOrEmpty(b5))continue;bool l2=b5.Contains("*")||b5.Contains("?");if(T.a9==aF.gs||l2){var hO=new List<IMyTerminalBlock>();
nY(b5,hO,T.ef);if(hO.Count>0){foreach(var lP in hO){if(!aB.Contains(lP))aB.Add(lP);}}else{var kw=ee(T.ef);
for(int g4=0;g4<kw.Count;g4++){var fX=kw[g4];if(l2 ? E.ol(b5,fX.CustomName): string.Equals(b5,fX.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aB.Contains(fX))aB.Add(fX);}}}}else{var g8=jL(b5,T.ef);if(g8!=null&& !aB.Contains(g8))aB.Add(g8);}}if(aB.Count==0){
oQ(T,no,aB);if(aB.Count>0)eF[T.U]=true;}ck[T.U]=aB;}foreach(var a0 in v.V){
if(string.IsNullOrEmpty(a0.bb))continue;var ix=jL(a0.bb);if(ix==null){dj.AppendLine($"HELM: output block '{a0.bb}' not found");
continue;}IMyTextSurface b4=null;var iO=ix as IMyTextSurfaceProvider;var lh=ix as IMyTextSurface;if(iO!=null&&a0.bK<iO.SurfaceCount){
b4=iO.GetSurface(a0.bK);}else if(lh!=null&&a0.bK==0){b4=lh;}if(b4==null){dj.AppendLine($"HELM: output block '{a0.bb}' has no surface index {a0.bK}");
continue;}b4.ContentType=ContentType.SCRIPT;b4.Script="";b4.ScriptBackgroundColor=Color.Black;bT[a0]=b4;}bR.Clear();
cj.Clear();}static readonly string[]jK={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool n9(int qd,IMyTerminalBlock r){switch(qd){case 0: return r is IMyBatteryBlock;case 1: return r is IMyReactor;case 2: return r is IMySolarPanel;
case 3: return r is IMyWindTurbine;case 4: return r is IMyGasTank;case 5: return r is IMyAirVent;case 6: return r is IMyCargoContainer;case 7: return r is IMyJumpDrive;
case 8: return r is IMyThrust;case 9: return r is IMyMotorSuspension;case 10: return r is IMyDoor;case 11: return r is IMyShipConnector;case 12: return r is IMyAssembler;
case 13: return r is IMyRefinery;case 14: return r is IMyShipDrill;case 15: return r is IMyShipWelder;case 16: return r is IMyShipGrinder;case 17: return r is IMyGasGenerator;
case 18: return r is IMyLargeTurretBase;case 19: return r is IMyProjector;case 20: return r is IMyGyro;case 21: return r is IMyGravityGeneratorBase;case 22: return r is IMyLandingGear;
case 23: return r is IMyRadioAntenna;case 24: return r is IMyBeacon;case 25: return r is IMyLightingBlock;case 26: return r is IMyPistonBase;case 27: return r is IMyMotorStator;
case 28: return r is IMyWarhead;case 29: return r is IMySensorBlock;case 30: return r is IMyCameraBlock;case 31: return r is IMyParachute;case 32: return r is IMyTurretControlBlock;
}return false;}void oQ(aS pn,List<string>rH,List<IMyTerminalBlock>g9){string p7=(pn.U+" "+string.Join(" ",rH)).ToLowerInvariant();
var h9=new List<int>();for(int fh=0;fh<jK.Length;fh++){foreach(var rZ in jK[fh].Split(','))if(p7.Contains(rZ)){h9.Add(fh);break;}}if(h9.Count==0)return;
var kv=ee();var mv=new List<IMyTerminalBlock>();for(int hU=0;hU<kv.Count;hU++){var g5=kv[hU];foreach(int qe in h9){if(n9(qe,g5)){(g5.CubeGrid==Me.CubeGrid ? g9 : mv).Add(g5);
break;}}}if(g9.Count==0)g9.AddRange(mv);}void oS(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(v==null||v.V.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {v.V.Count} screens, {v.bE.Count} binds");eB=false;
Echo($"Blocks: {ee().Count} on this construct; {gM} on docked ships (ignored)");bR.Clear();Echo("--- BIND RESOLUTION ---");foreach(var af in v.bE){
if(af.a9==aF.c4){string h7= !string.IsNullOrEmpty(af.aT)? af.aT : af.U;double rR=au.ContainsKey(h7)? au[h7]: af.en;
Echo($"[OK] @bind {af.U} (dynamic variable '{h7}' = {rR})");continue;}List<IMyTerminalBlock>ha;if(ck.TryGetValue(af.U,out ha)){if(ha.Count>0){
string pQ=eF.ContainsKey(af.U)&&eF[af.U]? " [fallback: typed match]" : "";Echo($"[OK] @bind {af.U} ({ha.Count} blocks matched{pQ})");
}else{string rI=string.Join(", ",af.az.Count>0 ? af.az : new List<string>{af.ke});Echo($"[WARN] @bind {af.U} -> 0 blocks matched \"{rI}\"");
}}else{Echo($"[ERR] @bind {af.U} -> unresolved");}}if(au.Count>0){Echo($"--- DYNAMIC VARIABLES ({au.Count}) ---");foreach(var ma in au)Echo($"[VAR] {ma.Key} = {ma.Value}");
}IMyTextSurface gX=null;foreach(var qI in bT.Values){gX=qI;break;}if(gX!=null){var i8=new List<string>();gX.GetSprites(i8);
Echo($"--- SPRITES ({i8.Count}): names usable in bg_texture ---");Echo(string.Join(", ",i8.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var a1 in v.V){IMyTextSurface ng;if(bT.TryGetValue(a1,out ng)){var m6=ng.SurfaceSize;var ry=E.n_(a1);
Echo($"[OK] Screen \"{a1.ba}\" [{a1.bL}] -> {a1.bb} [#{a1.bK}] ({m6.X:0}x{m6.Y:0} px, {ry.W} sprites)");
}else{Echo($"[ERR] Screen \"{a1.ba}\" -> Missing block \"{a1.bb}\" [#{a1.bK}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double oR(IMyTerminalBlock s){
if(s==null)return 0.0;var eP=s as IMyBatteryBlock;if(eP!=null)return eP.MaxStoredPower>0 ?(eP.CurrentStoredPower/eP.MaxStoredPower)*100.0 : 0.0;
var fg=s as IMyJumpDrive;if(fg!=null)return fg.MaxStoredPower>0 ?(fg.CurrentStoredPower/fg.MaxStoredPower)*100.0 : 0.0;var nn=s as IMyGasTank;
if(nn!=null)return nn.FilledRatio*100.0;var nG=s as IMyAirVent;if(nG!=null)return nG.GetOxygenLevel()*100.0;var kY=s as IMyCargoContainer;if(kY!=null){
var fe=kY.GetInventory();if(fe!=null&&fe.MaxVolume.RawValue>0)return(double)fe.CurrentVolume.RawValue/fe.MaxVolume.RawValue*100.0;return 0.0;}var fB=s as IMyReactor;
if(fB!=null)return fB.MaxOutput>0 ?(fB.CurrentOutput/fB.MaxOutput)*100.0 : 0.0;var fK=s as IMySolarPanel;if(fK!=null)return fK.MaxOutput>0 ?(fK.CurrentOutput/fK.MaxOutput)*100.0 : 0.0;
var f3=s as IMyWindTurbine;if(f3!=null)return f3.MaxOutput>0 ?(f3.CurrentOutput/f3.MaxOutput)*100.0 : 0.0;var li=s as IMyDoor;if(li!=null)return(double)li.OpenRatio*100.0;
var f_=s as IMyThrust;if(f_!=null)return f_.MaxEffectiveThrust>0 ?(f_.CurrentThrust/f_.MaxEffectiveThrust)*100.0 : 0.0;var jh=s as IMyMotorSuspension;
if(jh!=null)return jh.Brake ? 100.0 :(jh.Friction*100.0);var ho=s as IMyShipConnector;if(ho!=null)return ho.Status==MyShipConnectorStatus.Connected ? 100.0 :(ho.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var mK=s as IMyProductionBlock;if(mK!=null)return mK.IsProducing ? 100.0 : 0.0;var lm=s as IMyShipDrill;if(lm!=null)return lm.Enabled ? 100.0 : 0.0;
var jm=s as IMyShipToolBase;if(jm!=null)return jm.IsActivated ? 100.0 :(jm.Enabled ? 50.0 : 0.0);var hL=s as IMyGasGenerator;if(hL!=null)return hL.AutoRefill ? 100.0 :(hL.Enabled ? 50.0 : 0.0);
var f0=s as IMyLargeTurretBase;if(f0!=null)return f0.IsShooting ? 100.0 :(f0.HasTarget ? 50.0 :(f0.Enabled ? 10.0 : 0.0));var dW=s as IMyProjector;
if(dW!=null)return dW.TotalBlocks>0 ?((double)(dW.TotalBlocks-dW.RemainingBlocks)/dW.TotalBlocks)*100.0 : 0.0;var hR=s as IMyGyro;if(hR!=null)return hR.GyroOverride ? 100.0 :(hR.Enabled ? 50.0 : 0.0);
var lR=s as IMyGravityGeneratorBase;if(lR!=null)return Math.Min(100.0,Math.Abs(lR.GravityAcceleration)/9.81*100.0);var hK=s as IMyLandingGear;if(hK!=null)return hK.IsLocked ? 100.0 :(hK.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var kx=s as IMyRadioAntenna;if(kx!=null)return(kx.Radius/50000.0)*100.0;var kI=s as IMyBeacon;if(kI!=null)return(kI.Radius/50000.0)*100.0;
var mg=s as IMyLightingBlock;if(mg!=null)return mg.Intensity*10.0;var fz=s as IMyPistonBase;if(fz!=null)return fz.HighestPosition>0 ?(fz.CurrentPosition/fz.HighestPosition)*100.0 : 0.0;
var mO=s as IMyMotorStator;if(mO!=null)return Math.Min(100.0,Math.Abs(mO.Angle)/6.28318*100.0);var nI=s as IMyWarhead;if(nI!=null)return nI.IsArmed ? 100.0 : 0.0;
var i3=s as IMySensorBlock;if(i3!=null)return!i3.LastDetectedEntity.IsEmpty()? 100.0 :(i3.Enabled ? 25.0 : 0.0);var hh=s as IMyCameraBlock;
if(hh!=null)return hh.IsActive ? 100.0 :(hh.Enabled ? 50.0 : 0.0);var k3=s as IMyParachute;if(k3!=null)return(double)k3.OpenRatio*100.0;
var fZ=s as IMyTurretControlBlock;if(fZ!=null)return fZ.HasTarget ? 100.0 :(fZ.IsUnderControl ? 50.0 :(fZ.Enabled ? 10.0 : 0.0));var lG=s as IMyFunctionalBlock;
if(lG!=null)return lG.Enabled ? 100.0 : 0.0;return 0.0;}static double oi(IMyTerminalBlock kE,string hC){int kX=hC.IndexOf(':');string h8=hC.Substring(0,kX).ToLowerInvariant();
var rP=new MyItemType("MyObjectBuilder_"+(h8=="ammo" ? "AmmoMagazine" : char.ToUpper(h8[0])+h8.Substring(1)),hC.Substring(kX+1));double nf=0;for(int hV=0;
hV<kE.InventoryCount;hV++)nf+=(double)kE.GetInventory(hV).GetItemAmount(rP);return nf;}double j6(IMyTerminalBlock t,string ly){if(t==null)return 0.0;
string a=ly.ToLowerInvariant();if(a=="percent"||a=="pct")return oR(t);if(a.IndexOf(':')>0)return oi(t,ly);var bj=t as IMyBatteryBlock;
if(bj!=null){if(a=="current"||a=="stored"||a=="val"||a=="value"||a=="raw")return bj.CurrentStoredPower;if(a=="max"||a=="capacity")return bj.MaxStoredPower;
if(a=="output")return bj.CurrentOutput;if(a=="maxoutput")return bj.MaxOutput;if(a=="input")return bj.CurrentInput;if(a=="maxinput")return bj.MaxInput;
return bj.CurrentStoredPower;}var h5=t as IMyJumpDrive;if(h5!=null){if(a=="max"||a=="capacity")return h5.MaxStoredPower;return h5.CurrentStoredPower;
}var fU=t as IMyGasTank;if(fU!=null){if(a=="max"||a=="capacity")return fU.Capacity;return fU.FilledRatio*fU.Capacity;}var nH=t as IMyAirVent;
if(nH!=null){if(a=="max"||a=="capacity")return 100.0;return nH.GetOxygenLevel()*100.0;}var kZ=t as IMyCargoContainer;if(kZ!=null){var ff=kZ.GetInventory();
if(ff!=null){if(a=="mass")return(double)ff.CurrentMass;if(a=="max"||a=="capacity")return(double)ff.MaxVolume;return(double)ff.CurrentVolume;}return 0.0;}var iP=t as IMyPowerProducer;
if(iP!=null){if(a=="max"||a=="capacity")return iP.MaxOutput;return iP.CurrentOutput;}var lj=t as IMyDoor;if(lj!=null){if(a=="max")return 1.0;return(double)lj.OpenRatio*100.0;
}var jk=t as IMyThrust;if(jk!=null){if(a=="max"||a=="capacity")return jk.MaxEffectiveThrust;return jk.CurrentThrust;}var bx=t as IMyMotorSuspension;
if(bx!=null){if(a=="brake")return bx.Brake ? 100.0 : 0.0;if(a=="height")return bx.Height;if(a=="strength")return bx.Strength;if(a=="power")return bx.Power;
if(a=="steer")return bx.SteeringOverride*100.0;if(a=="propulsion")return bx.PropulsionOverride*100.0;return bx.Friction;}var k9=t as IMyShipConnector;
if(k9!=null){return k9.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var iM=t as IMyProductionBlock;if(iM!=null){if(a=="producing"||a=="active"||a=="running"||a=="state")return iM.IsProducing ? 100.0 : 0.0;
if(a=="queue")return iM.IsQueueEmpty ? 0.0 : 1.0;}var g0=t as IMyAssembler;if(g0!=null){if(a=="disassembling")return g0.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(a=="repeating"||a=="loop")return g0.Repeating ? 100.0 : 0.0;}var hM=t as IMyGasGenerator;if(hM!=null){if(a=="autorefill")return hM.AutoRefill ? 100.0 : 0.0;
if(a=="active")return hM.Enabled ? 100.0 : 0.0;}var f1=t as IMyLargeTurretBase;if(f1!=null){if(a=="shooting")return f1.IsShooting ? 100.0 : 0.0;
if(a=="target"||a=="hastarget")return f1.HasTarget ? 100.0 : 0.0;if(a=="range")return f1.Range;}var br=t as IMyProjector;if(br!=null){if(a=="built")return br.TotalBlocks-br.RemainingBlocks;
if(a=="remaining")return br.RemainingBlocks;if(a=="total")return br.TotalBlocks;if(a=="projecting")return br.IsProjecting ? 100.0 : 0.0;if(a=="armor")return br.RemainingArmorBlocks;
if(a=="buildable")return br.BuildableBlocksCount;}var jn=t as IMyShipToolBase;if(jn!=null){if(a=="working"||a=="active"||a=="activated")return jn.IsActivated ? 100.0 : 0.0;
return jn.Enabled ? 100.0 : 0.0;}var cw=t as IMyGyro;if(cw!=null){if(a=="override")return cw.GyroOverride ? 100.0 : 0.0;if(a=="pitch")return cw.Pitch;
if(a=="yaw")return cw.Yaw;if(a=="roll")return cw.Roll;if(a=="power")return cw.GyroPower*100.0;}var lS=t as IMyGravityGeneratorBase;if(lS!=null){if(a=="g"||a=="accel")return lS.GravityAcceleration;
}var lQ=t as IMyLandingGear;if(lQ!=null){if(a=="locked")return lQ.IsLocked ? 100.0 : 0.0;}var ky=t as IMyRadioAntenna;if(ky!=null){if(a=="radius"||a=="range")return ky.Radius;
}var kJ=t as IMyBeacon;if(kJ!=null){if(a=="radius"||a=="range")return kJ.Radius;}var ih=t as IMyLightingBlock;if(ih!=null){if(a=="intensity")return ih.Intensity;
if(a=="radius")return ih.Radius;}var fA=t as IMyPistonBase;if(fA!=null){if(a=="pos"||a=="position"||a=="extension")return fA.CurrentPosition;
if(a=="velocity")return fA.Velocity;if(a=="max")return fA.HighestPosition;}var d0=t as IMyMotorStator;if(d0!=null){if(a=="angle"||a=="deg")return d0.Angle*180.0/Math.PI;
if(a=="rad")return d0.Angle;if(a=="velocity"||a=="rpm")return d0.TargetVelocityRPM;if(a=="locked")return d0.RotorLock ? 100.0 : 0.0;}var jv=t as IMyWarhead;
if(jv!=null){if(a=="armed")return jv.IsArmed ? 100.0 : 0.0;if(a=="countdown")return jv.DetonationTime;}var m3=t as IMySensorBlock;if(m3!=null){
if(a=="detected")return!m3.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var hi=t as IMyCameraBlock;if(hi!=null){if(a=="range")return hi.AvailableScanRange;
if(a=="active")return hi.IsActive ? 100.0 : 0.0;}var k4=t as IMyParachute;if(k4!=null){if(a=="open"||a=="openratio")return(double)k4.OpenRatio*100.0;
}var cO=t as IMyTurretControlBlock;if(cO!=null){if(a=="target"||a=="hastarget")return cO.HasTarget ? 100.0 : 0.0;if(a=="azimuth")return cO.AzimuthRotor!=null ? cO.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(a=="elevation")return cO.ElevationRotor!=null ? cO.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var lH=t as IMyFunctionalBlock;if(lH!=null){return lH.Enabled ? 100.0 : 0.0;
}return 0.0;}double bM(string dq,string lz){aS bk;dg.TryGetValue(dq,out bk);if(bk!=null&&bk.a9==aF.c4){
string qf= !string.IsNullOrEmpty(bk.aT)? bk.aT : dq;double lc;if(au.TryGetValue(qf,out lc))return lc;return bk.en;
}double lp;if(au.TryGetValue(dq,out lp))return lp;List<IMyTerminalBlock>dr;if(!ck.TryGetValue(dq,out dr)||dr.Count==0)throw new InvalidOperationException($"HELM: bind '{dq}' resolved to zero blocks — check the name/typo");
if(dr.Count==1)return j6(dr[0],lz);var nF=new List<double>();foreach(var pc in dr)nF.Add(j6(pc,lz));return nQ.nP(bk!=null ? bk.cS : ar.f7,nF);
}double gF(string pp){return bM(pp,"percent");}static string a6(double nC,bJ cC){if(cC==null||(string.IsNullOrEmpty(cC.df)&&string.IsNullOrEmpty(cC.c5)&&cC.N=="percent")){
return jP(nC);}return E.oa(nC,cC.df,cC.c5);}bool oL(y cJ){IMyTextSurface fR;if(!bT.TryGetValue(cJ,out fR))return false;
if(cJ.bB!=null)return oF(cJ,fR);if(cJ.c_.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return oM(cJ,fR);
}return oN(cJ,fR);}Color od(string[]cL,double qP){if(cL.Length==1)return D(cL[0]);double mL=Math.Max(0.0,Math.Min(100.0,qP))/100.0*(cL.Length-1);
int hW=Math.Min((int)mL,cL.Length-2);float fS=(float)(mL-hW);Color bh=D(cL[hW]),eO=D(cL[hW+1]);return new Color((int)(bh.R+(eO.R-bh.R)*fS),(int)(bh.G+(eO.G-bh.G)*fS),(int)(bh.B+(eO.B-bh.B)*fS),(int)(bh.A+(eO.A-bh.A)*fS));
}static Color[]eb(aV pu,string[]rB,string pH,float nK,float nM,float jt,float hS){int dn=16;bool lX=pH=="horizontal";var hn=new Color[dn];
for(int bY=0;bY<dn;bY++){n hJ=E.jQ(rB,100.0*bY/(dn-1));hn[bY]=new Color(hJ.bc,hJ.a7,hJ.a3);float kW=jt/dn,kP=hS/dn;x(pu,lX ? new Vector2(nK+kW*(bY+0.5f),nM+hS/2f): new Vector2(nK+jt/2f,nM+kP*(bY+0.5f)),lX ? new Vector2(kW+1f,hS): new Vector2(jt,kP+1f),hn[bY]);
}return hn;}static void x(aV pv,Vector2 qX,Vector2 rs,Color px,float re=0f,string pG="SquareSimple"){pv.l.Add(new MySprite{Type=SpriteType.TEXTURE,Data=pG,Position=qX,Size=rs,Color=px,RotationOrScale=re}
);}void nX(y h,IMyTextSurface nh,aV f){f.l.Clear();f.ey.Clear();Vector2 J=nh.SurfaceSize;
Vector2 rM=nh.TextureSize;Vector2 aq=(rM-J)/2f;float fs=Math.Max(4f,J.X*0.02f);float bZ=Math.Max(4f,J.Y*0.02f);
if(h.bL=="compact"||(h.bL=="auto"&&J.Y<=260f&&J.Y>150f)){fs=4f;bZ=4f;}else if(h.bL=="corner"||(h.bL=="auto"&&J.Y<=150f)){
fs=2f;bZ=2f;}float d8=J.X-(fs*2f);Color cn=new Color(10,12,16);bool ll=true;if(!string.IsNullOrEmpty(h.bA)){
var dB=n.da(h.bA);if(dB.es)ll=false;else cn=new Color(dB.bc,dB.a7,dB.a3,dB.f5);}f.bD= -1;f.ed=false;
f.ec=D(h.jy ?? "#5b0f0f");f.cZ.Clear();if(h.bC!=null&&h.bC.Length>1){f.bD=f.l.Count;f.eh=eb(f,h.bC,h.cY,aq.X+2f,aq.Y+2f,J.X-4f,J.Y-4f);
}else if(ll||h.aE!=null||h.ca!=null){if(h.aE!=null)cn=D(h.aE.First(q1=>q1.cf).bH);f.bD=f.l.Count;
f.eh=new[]{cn};x(f,aq+(J/2f),J-new Vector2(4f,4f),cn);if(h.aE!=null||h.ca!=null)f.cZ.Add(new ei{
er=f.bD,f8=cn,em=cn,ex=h.aE,gc=h.f9,gl=h.ga,ep=h.ca,gm=h.gn,go=h.gp,jW=true}
);}if(!string.IsNullOrEmpty(h.gb))x(f,aq+(J/2f),J-new Vector2(4f,4f),D(h.jA ?? "255,255,255,24"),0f,h.gb);
if(!h.c_.Equals("none",StringComparison.OrdinalIgnoreCase)){Color kR=h.al!=null&&h.al.Length>0 ? D(h.al[0]): D(h.el ?? "40, 60, 85");
Color pt=h.al!=null&&h.al.Length>1 ? D(h.al[h.al.Length-1]): kR;
float kS=aq.X+(J.X-d8)/2f;if(h.al!=null&&h.al.Length>1&& !h.gr){eb(f,h.al,"horizontal",kS,aq.Y+bZ*0.5f-1f,d8,2f);
eb(f,h.al,"horizontal",kS,aq.Y+J.Y-(bZ*0.5f)-1f,d8,2f);}else{x(f,new Vector2(aq.X+J.X/2f,aq.Y+bZ*0.5f),new Vector2(d8,2f),kR);
x(f,new Vector2(aq.X+J.X/2f,aq.Y+J.Y-(bZ*0.5f)),new Vector2(d8,2f),pt);}}var iC=h.aI;
if(iC.Count==0){iC=new List<at>{new at{c9="main",c1=0,db=0,dc=h.cd,dd=h.ce,aU=h.gw}};}float lU=8f;
foreach(var u in iC){var fr=E.nZ(u.c1,u.db,u.dc,u.dd,h.cd,h.ce,J.X,J.Y,fs,bZ,lU,lU);
float aL=aq.X+fr.kl;float dT=aq.Y+fr.km;float aZ=fr.kk;float fq=fr.jS;bool mR=u.aE!=null||u.ca!=null;
if(u.bC!=null&&u.bC.Length>1){eb(f,u.bC,u.cY!="vertical" ? u.cY : h.cY,aL,dT,aZ,fq);
}else{Color bq;if(u.aE!=null)bq=D(u.aE.First(q2=>q2.cf).bH);else if(!string.IsNullOrEmpty(u.cT))bq=D(u.cT);
else if(h.cd>1||h.ce>1)bq=string.IsNullOrEmpty(h.gA)? new Color(255,255,255,6): D(h.gA);else bq=Color.Transparent;
if(bq.A>0||mR){if(mR)f.cZ.Add(new ei{er=f.l.Count,f8=bq,em=bq,ex=u.aE,gc=u.f9,gl=u.ga,ep=u.ca,gm=u.gn,go=u.gp}
);x(f,new Vector2(aL+aZ/2f,dT+fq/2f),new Vector2(aZ,fq),bq);}}string hI=u.jM ?? h.jN ?? "Monospace";float pl=E.oc(h.bL,J.X,J.Y,h.cb);
float aC=u.jO ?? pl;float e7=0f;if(!string.IsNullOrEmpty(u.bP)){e7=20f*aC;var gV=TextAlignment.LEFT;float jl=aL+4f;
string kr=u.gH ?? h.kf ?? "left";if(kr.Equals("center",StringComparison.OrdinalIgnoreCase)){gV=TextAlignment.CENTER;jl=aL+(aZ/2f);
}else if(kr.Equals("right",StringComparison.OrdinalIgnoreCase)){gV=TextAlignment.RIGHT;jl=aL+aZ-4f;}string nq=u.gI ?? h.kg;
Color rN= !string.IsNullOrEmpty(nq)? D(nq): D(u.gg ?? h.el ?? "white");var nv=MySprite.CreateText(u.bP,hI,rN,aC*0.9f,gV);
nv.Position=new Vector2(jl,dT+2f);f.l.Add(nv);string[]g6=u.gh ?? h.al;Color pJ=g6!=null&&g6.Length>0 ? D(g6[0]): D(u.gg ?? h.el ?? "40, 60, 85");
x(f,new Vector2(aL+aZ/2f,dT+e7-2f),new Vector2(aZ-8f,1f),pJ);}int qA=Math.Max(1,u.aU.Count);float qN=fq-e7-8f;
float iT=qN/qA;float fd=6f;for(int fc=0;fc<u.aU.Count;fc++){var z=u.aU[fc];var P=new c8{jZ=z};float rf=dT+e7+4f+(fc*iT);
float I=rf+(iT/2f);float ns=I-(11f*aC);P.gu=f.l.Count;P.jR=z.S!=null&&(z.S.Contains("{val}")||z.S.Contains("{percent}"));
var md=MySprite.CreateText(z.S,hI,Color.White,aC,TextAlignment.LEFT);md.Position=new Vector2(aL+fd,ns);f.l.Add(md);
float qt=Math.Max(24f*aC,(z.S.Length+1)*12f*aC);float rY=(!string.IsNullOrEmpty(z.df)|| !string.IsNullOrEmpty(z.c5))? 64f*aC : 52f*aC;
if(z.g!=e.gG){float H=aL+fd+qt;float pk=aL+aZ-fd-rY;float F=Math.Max(16f,pk-H);
float O=Math.Max(z.g==e.bN ? 8f : 4f,Math.Min((z.g==e.bN ? 20f : 16f)*aC,iT*(z.g==e.bN ? 0.55f : 0.45f)));
P.cX=H;P.eg=F;P.b9=O;P.cW=I;if(z.g==e.cV){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(25,30,38));
P.aa=f.l.Count;x(f,new Vector2(H,I),new Vector2(0f,O),D(z.am));}else if(z.g==e.c7){
int i1=10;float lJ=2f;float i2=Math.Max(2f,(F-(lJ*(i1-1)))/i1);for(int iX=0;iX<i1;iX++){P.ad.Add(f.l.Count);
x(f,new Vector2(H+iX*(i2+lJ)+(i2/2f),I),new Vector2(i2,O),new Color(22,26,32));}}else if(z.g==e.bN){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(20,24,30));
int iL=8;float lK=2f;float hl=Math.Max(2f,(F-(lK*(iL-1)))/iL);for(int iA=0;iA<iL;iA++){P.ad.Add(f.l.Count);
x(f,new Vector2(H+iA*(hl+lK)+(hl/2f),I),new Vector2(hl,2f),D(z.am));}}else if(z.g==e.c6){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(20,24,30));
int jb=10;float lL=2f;float jc=Math.Max(2f,(F-(lL*(jb-1)))/jb);for(int iY=0;iY<jb;iY++){P.ad.Add(f.l.Count);
x(f,new Vector2(H+iY*(jc+lL)+(jc/2f),I),new Vector2(jc,O),new Color(24,28,36),0.523599f);}}else if(z.g==e.c0){
int gZ=10;float lM=2f;float g_=Math.Max(2f,(F-(lM*(gZ-1)))/gZ);for(int iZ=0;iZ<gZ;iZ++){P.ad.Add(f.l.Count);
x(f,new Vector2(H+iZ*(g_+lM)+(g_/2f),I),new Vector2(g_,O),new Color(24,28,36),1.570796f,"Triangle");}}else if(z.g==e.c3){
int hr=12;float lN=2f;float ht=Math.Max(2f,(F-(lN*(hr-1)))/hr);float lk=Math.Min(ht,O);for(int i_=0;i_<hr;i_++){P.ad.Add(f.l.Count);
x(f,new Vector2(H+i_*(ht+lN)+(ht/2f),I),new Vector2(lk,lk),new Color(24,28,36),0f,"Circle");}}else if(z.g==e.b8){
x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(25,30,38));P.aa=f.l.Count;x(f,new Vector2(H+(F/2f),I),new Vector2(0f,O),D(z.am));
x(f,new Vector2(H+(F/2f),I),new Vector2(2f,O+4f),new Color(80,110,150));}else if(z.g==e.bQ){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(20,24,30));
P.aa=f.l.Count;float ia=Math.Max(4f,O);x(f,new Vector2(H+ia*0.5f,I),new Vector2(ia,ia),new Color(40,50,65),0f,"Circle");
}else if(z.g==e.bG){float co=Math.Max(6f,Math.Min(O,14f*aC));x(f,new Vector2(H+co*0.5f,I),new Vector2(co,co),new Color(28,34,46));
P.aa=f.l.Count;x(f,new Vector2(H+co*0.5f,I),new Vector2(co*0.6f,co*0.6f),new Color(40,50,65));}else if(z.g==e.bO){
x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(18,22,30));P.aa=f.l.Count;float iJ=Math.Max(4f,O*0.7f);
x(f,new Vector2(H+iJ+2f,I),new Vector2(iJ,iJ),new Color(34,197,94),0f,"Circle");}else if(z.g==e.b7){int iI=8;
float lO=2f;float iK=Math.Max(2f,(F-(lO*(iI-1)))/iI);for(int iB=0;iB<iI;iB++){P.ad.Add(f.l.Count);x(f,new Vector2(H+iB*(iK+lO)+(iK/2f),I),new Vector2(iK,O),new Color(24,28,36));
}}else if(z.g==e.bI){x(f,new Vector2(H+(F/2f),I),new Vector2(F,O),new Color(20,24,30));P.aa=f.l.Count;
float l_=Math.Max(4f,O);x(f,new Vector2(H+(F/2f),I),new Vector2(l_,l_),D(z.am),0f,"Triangle");}}var nA=MySprite.CreateText("---",hI,Color.White,aC,TextAlignment.RIGHT);
nA.Position=new Vector2(aL+aZ-fd,ns);P.bf=f.l.Count;f.l.Add(nA);f.ey.Add(P);}}f.jX=J;
f.jV=true;}static void aJ(aV kU,c8 m7,string rL,Color py){var jr=kU.l[m7.bf];jr.Data=rL;jr.Color=py;kU.l[m7.bf]=jr;
}static void j8(aV kV,c8 m8,Color pz){var lt=kV.l[m8.aa];lt.Color=pz;kV.l[m8.aa]=lt;}bool oN(y a2,IMyTextSurface je){
aV i;if(!eH.TryGetValue(a2,out i)){i=new aV();eH[a2]=i;}if(!i.jV||i.jX!=je.SurfaceSize){
nX(a2,je,i);}bool L=false;bool eK=(aA%a2.f6)<a2.f6/2.0;double bX= -1.0,la;if(eC.TryGetValue(a2,out la)){
bX=(aA-la)/a2.ge;if(bX>=1.0){eC.Remove(a2);bX=1.0;}}bool ju=bS&&eK&&i.ec.A>0;if(i.bD>=0&&ju!=i.ed){
i.ed=ju;for(int dD=0;dD<i.eh.Length;dD++){var kL=i.l[i.bD+dD];kL.Color=ju ? i.ec : i.eh[dD];i.l[i.bD+dD]=kL;
}L=true;foreach(var pm in i.cZ)pm.gq=true;}foreach(var ac in i.cZ){if(ac.jW&&i.ed)continue;Color dp=ac.f8;try{if(ac.ex!=null)dp=D(E.ew(ac.ex,bM(ac.gc,ac.gl)));
if(ac.ep!=null&&eK&&E.ew(ac.ep,bM(ac.gm,ac.go))!=null)dp=i.ec;}catch{}if(dp!=ac.em||ac.gq){
ac.em=dp;ac.gq=false;var kT=i.l[ac.er];kT.Color=dp;i.l[ac.er]=kT;L=true;}}for(int hX=0;hX<i.ey.Count;hX++){var c=i.ey[hX];
var j=c.jZ;double w;bool lW=false;double io=j.eu;try{w=bM(j.a5,j.N);if(j.cg!=null)io=bM(j.cg,j.gx);
}catch{lW=true;w=0.0;}if(lW){if(c.et!= -99999.0||(c.bf>=0&&i.l[c.bf].Data!="ERR")){L=true;c.et= -99999.0;
if(c.bf>=0){var i0=i.l[c.bf];i0.Data="ERR";i0.Color=Color.Red;i.l[c.bf]=i0;}}continue;}double aM=w;
if(io>0)w=Math.Min(100.0,aM/io*100.0);bool nj=bX>=0.0&&j.g!=e.gG&&j.g!=e.bQ&&j.g!=e.bG&&j.g!=e.bO&&j.g!=e.bI;
if(nj)w=bX<0.5 ? bX*200.0 : 100.0+(w-100.0)*(bX-0.5)*2.0;string pB=j.cc ? E.eq(j.a8,w): j.am;
Color _=D(pB);bool aX=(_==Color.Red);if(j.cc&& !a2.gr&&j.a8.Length>1)_=od(j.a8,w);
if(aX&&eK){_=new Color(255,75,75);L=true;}bool aD=nj||Math.Abs(aM-c.et)>0.1;if(aD)c.et=aM;
if(j.g==e.cV){double lA=(j.gt||j.N=="percent"||j.N=="pct")? w : gF(j.a5);if(c.Q<0f)c.Q=(float)lA;
float fV=(float)lA;if(Math.Abs(c.Q-fV)>0.05f){c.Q+=(fV-c.Q)*0.35f;L=true;}else if(c.Q!=fV){
c.Q=fV;L=true;}if(aD)L=true;if(L){float qQ=(float)Math.Max(0.0,Math.Min(1.0,c.Q/100.0));float lC=c.eg*qQ;
var e1=i.l[c.aa];e1.Size=new Vector2(lC,c.b9);e1.Position=new Vector2(c.cX+(lC/2f),c.cW);
e1.Color=_;i.l[c.aa]=e1;aJ(i,c,a6(aM,j),_);}}else if(j.g==e.c7||j.g==e.c6||j.g==e.c0||j.g==e.c3){
if(aD||aX){L=true;double pS=(j.gt||j.N=="percent"||j.N=="pct")? w : gF(j.a5);int m1=c.ad.Count;
for(int d2=0;d2<m1;d2++){bool qC=pS>=((d2+0.4)/(double)m1*100.0);Color rp=qC ? _ : new Color(24,28,36);var m2=i.l[c.ad[d2]];
m2.Color=rp;i.l[c.ad[d2]]=m2;}aJ(i,c,a6(aM,j),_);}}else if(j.g==e.bN){
if(aD||aX){L=true;for(int e6=0;e6<7;e6++)c.ez[e6]=c.ez[e6+1];c.ez[7]=(float)w;int qW=c.ad.Count;
for(int dP=0;dP<qW;dP++){float p6=c.ez[dP];float p5=Math.Max(0.06f,Math.Min(1f,p6/100f));float k6=c.b9*p5;float pA=c.cW+(c.b9/2f)-(k6/2f);
var ct=i.l[c.ad[dP]];ct.Size=new Vector2(ct.Size.Value.X,k6);ct.Position=new Vector2(ct.Position.Value.X,pA);
ct.Color=_;i.l[c.ad[dP]]=ct;}aJ(i,c,a6(aM,j),_);}}else if(j.g==e.b8){
if(c.Q<0f)c.Q=(float)w;float fW=(float)w;if(Math.Abs(c.Q-fW)>0.05f){c.Q+=(fW-c.Q)*0.35f;
L=true;}else if(c.Q!=fW){c.Q=fW;L=true;}if(L||aD){float lV=c.eg*0.5f;float k1=c.cX+lV;
float lg=(float)Math.Max(-1.0,Math.Min(1.0,(c.Q-50.0)/50.0));float hG=Math.Abs(lg)*lV;float pT=(lg>=0)?(k1+hG*0.5f):(k1-hG*0.5f);
var e2=i.l[c.aa];e2.Size=new Vector2(hG,c.b9);e2.Position=new Vector2(pT,c.cW);e2.Color=_;
i.l[c.aa]=e2;aJ(i,c,a6(aM,j),_);}}else if(j.g==e.bQ){if(aD||aX){
L=true;bool h4=w>=50.0;float l7=Math.Max(4f,c.b9);float qi=h4 ?(c.cX+c.eg-l7*0.5f):(c.cX+l7*0.5f);
Color l6=h4 ?(_!=Color.White ? _ : new Color(34,197,94)): new Color(60,70,85);var hE=i.l[c.aa];hE.Position=new Vector2(qi,c.cW);
hE.Color=l6;i.l[c.aa]=hE;aJ(i,c,h4 ? "ON" : "OFF",l6);}}else if(j.g==e.bG){if(aD||aX){
L=true;bool h3=w>=50.0;Color k2=h3 ?(_!=Color.White ? _ : new Color(56,189,248)): new Color(30,36,48);j8(i,c,k2);
aJ(i,c,h3 ? "ACTIVE" : "READY",h3 ? k2 : new Color(148,163,184));}}else if(j.g==e.bO){if(aD||aX){
L=true;string rz=w>=75.0 ? "ONLINE" :(w>=25.0 ? "STBY" : "OFFLINE");Color nc=w>=75.0 ? new Color(34,197,94):(w>=25.0 ? new Color(234,179,8): new Color(239,68,68));
j8(i,c,nc);aJ(i,c,rz,nc);}}else if(j.g==e.b7){if(aD||aX){L=true;int mG=c.ad.Count;
for(int dQ=0;dQ<mG;dQ++){bool qD=w>=((dQ+0.5)/(double)mG*100.0);Color qU=qD ? _ : new Color(24,28,36);var mH=i.l[c.ad[dQ]];
mH.Color=qU;i.l[c.ad[dQ]]=mH;}aJ(i,c,a6(aM,j),_);}}else if(j.g==e.bI){
L=true;float fE;Color dz;string e3;bool qa=(j.N=="producing"||j.N=="active"||j.N=="running"||j.N=="state"||(j.N!="percent"&&(w<=1.0||w==100.0)));
if(qa){if(w>0.0){c.Q=(c.Q+0.3927f)%6.28318f;fE=c.Q;dz=new Color(34,197,94);e3="RUN";}else{fE=0f;
dz=new Color(148,163,184);e3="IDLE";}}else{fE=(w>55.0)? 0f :((w<45.0)? 3.14159f : 1.57079f);dz=(w>55.0)? new Color(34,197,94):((w<45.0)? new Color(239,68,68): new Color(148,163,184));
e3=(w>55.0)? "CHG" :((w<45.0)? "DSC" : "STBY");}var hF=i.l[c.aa];hF.RotationOrScale=fE;hF.Color=dz;
i.l[c.aa]=hF;aJ(i,c,e3,dz);}else{string e_;aS fY;dg.TryGetValue(j.a5,out fY);eE.TryGetValue(fY!=null&&fY.aT!=null ? fY.aT : j.a5,out e_);
if(aD||aX||e_!=c.jY){c.jY=e_;L=true;Color jo=j.c2!=null ? D(E.ew(j.c2,w)): Color.White;
if(jo==Color.Red&&eK)jo=new Color(255,75,75);aJ(i,c,e_ ?? a6(aM,j),jo);}}if(c.jR&&(aD||aX)){
L=true;var mf=i.l[c.gu];string qR=jP(w);string rc=a6(aM,j);mf.Data=j.S.Replace("{percent}",qR).Replace("{val}",rc);
i.l[c.gu]=mf;}}if(L){using(var pX=je.DrawFrame()){pX.AddRange(i.l);}}return L;}bool oM(y bt,IMyTextSurface jf){
jf.FontSize=bt.cb;var eW=new List<KeyValuePair<string,string>>();foreach(var m in bt.gw){double K=0.0;string aQ;string aK=null;
double ip=m.eu;try{K=bM(m.a5,m.N);if(m.cg!=null)ip=bM(m.cg,m.gx);}catch(Exception r4){
eW.Add(new KeyValuePair<string,string>($"{m.S}: [UNRESOLVED]","red"));continue;}double mM=K;if(ip>0)K=Math.Min(100.0,mM/ip*100.0);string nB=a6(mM,m);
if(m.g==e.cV){double lB=(m.gt||m.N=="percent"||m.N=="pct")? K : gF(m.a5);aQ=E.oG(m.S,lB,10,nB);
aK=m.cc ? E.eq(m.a8,lB): m.am;}else if(m.g==e.b8){aQ=E.oE(m.S,K,10);
aK=m.cc ? E.eq(m.a8,K): m.am;}else if(m.g==e.bQ){aQ=E.oP(m.S,K);
aK=m.am ??(K>=50.0 ? "green" : "gray");}else if(m.g==e.bG){aQ=E.oJ(m.S,K);aK=m.am ??(K>=50.0 ? "cyan" : "gray");
}else if(m.g==e.bO){aQ=E.oO(m.S,K);aK=m.am ??(K>=75.0 ? "lime" :(K>=25.0 ? "yellow" : "red"));
}else if(m.g==e.b7){aQ=E.oD(m.S,K,8);aK=m.cc ? E.eq(m.a8,K):(m.am ?? "yellow");
}else if(m.g==e.bI){bool qb=(m.N=="producing"||m.N=="active"||m.N=="running"||m.N=="state"||(m.N!="percent"&&(K<=1.0||K==100.0)));
if(qb){char[]rx=new[]{'|','/','-','\\'};char rm=rx[Math.Abs(dk)%4];aQ=K>0 ? $"{m.S} [{rm}] RUN" : $"{m.S} [==] IDLE";
aK=K>0 ? "green" : "gray";}else{aQ=E.oK(m.S,K,dk);aK=m.am ??(K>55.0 ? "green" :(K<45.0 ? "red" : "cyan"));
}}else{aQ=$"{m.S}: {nB}";aK=m.c2!=null ? E.ew(m.c2,K): null;}eW.Add(new KeyValuePair<string,string>(aQ,aK));
}var eT=E.oI(new List<string>(eW.ConvertAll(rG=>rG.Key)),cQ: bt.kj);string m4=string.Join("\n",eT);
string mJ;if(ci.TryGetValue(bt,out mJ)&&mJ==m4)return false;ci[bt]=m4;using(var pY=jf.DrawFrame()){
float nN=4f;float mh=nO*bt.cb;try{var mj=jf.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bt.cb);
if(mj.Y>0)mh=mj.Y;}catch{}for(int cx=0;cx<eT.Count;cx++){string k8=null;if(cx>0&&cx<eT.Count-1)k8=eW[cx-1].Value;
var nb=MySprite.CreateText(eT[cx],"Monospace",D(k8),bt.cb,TextAlignment.LEFT);nb.Position=new Vector2(4f,nN);pY.Add(nb);
nN+=mh;}}return true;}Color D(string fo){if(string.IsNullOrEmpty(fo))return Color.White;Color dK;if(kn.TryGetValue(fo,out dK))return dK;
n ds=n.da(fo);dK=ds.es ? Color.Transparent : new Color(ds.bc,ds.a7,ds.a3,ds.f5);kn[fo]=dK;return dK;}public enum aF{nS,gs,c4}
public enum ar{om,f7,kd,j0,j_}public class aS{public string U;public aF a9;public List<string>az=new List<string>();public string ke{
get{return az.Count>0 ? az[0]: "";}set{if(az.Count==0)az.Add(value);else az[0]=value;}}public ar cS=ar.om;
public double en=0.0;public string gi;public string aT;public bool ef;}public enum e{gG,cV,c7,bN,c6,c0,c3,b8,bQ,bG,bO,b7,bI}
public class aw{public string gk;public string bH;public bool cf;}public class bJ{public e g;public string S;public string a5;
public string N;public bool cc;public string[]a8;public string am;public List<aw>c2;public string jH="right";
public string df;public string c5;public double eu;public string cg,gx;public bool gt{get{return eu>0||cg!=null;}}}public struct n{
public byte bc,a7,a3,f5;public bool es;public n(byte q3,byte p_,byte pd,byte oX=255){bc=q3;a7=p_;a3=pd;f5=oX;es=false;}public static readonly n oV=new n(0,0,0,0){
es=true};public static readonly n eA=new n(255,255,255);public static readonly n nR=new n(0,0,0);public static readonly n oB=new n(255,0,0);
public static readonly n of=new n(0,255,0);public static readonly n oj=new n(50,205,50);public static readonly n oW=new n(255,255,0);
public static readonly n oo=new n(255,140,0);public static readonly n n1=new n(0,255,255);public static readonly n nT=new n(0,120,255);
public static readonly n ok=new n(255,0,255);public static readonly n oz=new n(128,0,128);public static readonly n oe=new n(128,128,128);
public static readonly n n2=new n(60,60,60);public static n da(string aP){if(string.IsNullOrWhiteSpace(aP))return eA;aP=aP.Trim();
if(aP.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aP.Equals("none",StringComparison.OrdinalIgnoreCase))return oV;if(aP.StartsWith("#")){
string bn=aP.Substring(1).Trim();if(bn.Length==6){byte q4=Convert.ToByte(bn.Substring(0,2),16);byte p0=Convert.ToByte(bn.Substring(2,2),16);byte pe=Convert.ToByte(bn.Substring(4,2),16);
return new n(q4,p0,pe);}if(bn.Length==3){byte q5=Convert.ToByte(new string(bn[0],2),16);byte p1=Convert.ToByte(new string(bn[1],2),16);byte pf=Convert.ToByte(new string(bn[2],2),16);
return new n(q5,p1,pf);}}if(aP.Contains(",")){var cF=aP.Split(',');if(cF.Length>=3){byte q6=(byte)Math.Max(0,Math.Min(255,int.Parse(cF[0].Trim())));
byte p2=(byte)Math.Max(0,Math.Min(255,int.Parse(cF[1].Trim())));byte pg=(byte)Math.Max(0,Math.Min(255,int.Parse(cF[2].Trim())));byte oY=cF.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(cF[3].Trim()))):(byte)255;
return new n(q6,p2,pg,oY);}}switch(aP.ToLowerInvariant()){case "red": return oB;case "green": return of;case "lime": return oj;case "yellow": return oW;
case "orange": return oo;case "cyan": return n1;case "blue": return nT;case "magenta": return ok;case "purple": return oz;case "white": return eA;
case "black": return nR;case "gray": case "grey": return oe;case "darkgray": case "darkgrey": return n2;default: return eA;}}}public class a4{
public string[]bC;public string cY="vertical";public List<aw>aE;public string f9,ga;public List<aw>ca;public string gn,gp;
}public class at : a4{public string c9="default";public int c1=0;public int db=0;public int dc=1;public int dd=1;public string bP;public string gH="left";
public string gI;public string jF;public string jM;public float? jO;public string cT;public string gg;public string[]gh;
public string j1="horizontal";public List<bJ>aU=new List<bJ>();}public class y : a4{public string ba;public string bL="standard";
public string c_="graphic";public string el;public string[]al;public string kf="left";public string kg;public string jN="Monospace";
public string bA;public string jD;public int jE=10;public int jB=1;public float ge=1.2f;public bool gr;
public string gA;public string gb;public string jA;public string jy;public string bB;public string bF;public float gf=2.5f;
public string jC="cyan";public string[]ej;public List<string>ek=new List<string>();public List<string>ob(){if(ek!=null&&ek.Count>0)return ek;
return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float cb=1.0f;public int gC=0;public float f6=1.6f;public int cd=1;public int ce=1;public int kj=30;public string bb;
public int bK=0;public List<at>aI=new List<at>();public List<bJ>gw{get{if(aI.Count==0)aI.Add(new at{c9="main"}
);return aI[0].aU;}}}public class be{public string gy;public List<string>ev=new List<string>();public Dictionary<string,string>gj=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>gd=new List<string>();}public class gB{public List<aS>bE=new List<aS>();public List<y>V=new List<y>();
public List<string>aH=new List<string>();public Dictionary<string,List<string>>cU=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class jT{static readonly Dictionary<string,be>jG=new Dictionary<string,be>(StringComparer.OrdinalIgnoreCase);
static jT(){oC();}static void oC(){bd("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bd("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bd("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bd("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bd("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bd("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bd("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bd("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bd(string mq,string[]qO,string pq){var fT=new be{gy=mq}
;foreach(var qJ in qO){var ib=qJ.Split(new[]{'='},2);string my=gz(ib[0]);fT.ev.Add(my);if(ib.Length>1)fT.gj[my]=ab(ib[1].Trim());
}var qB=pq.Replace("\r\n","\n").Split('\n');foreach(var qo in qB){string mT=qo.Trim();if(mT.Length>0)fT.gd.Add(mT);}jG[mq]=fT;}static string gz(string cD){
cD=cD.Trim();if(cD.StartsWith("$"))cD=cD.Substring(1);return cD.ToLowerInvariant();}public static gB oq(string pE){var ai=new gB();y d=null;
at p=null;var mI=ox(pE,ai.aH,ai.cU);for(int dE=0;dE<mI.Count;dE++){string b=mI[dE].Trim();
if(b.Length==0)continue;try{if(b.StartsWith("@bind")){ai.bE.Add(os(b));}else if(b.StartsWith("@screen")){d=new y{ba=b.Substring("@screen".Length).Trim()}
;ai.V.Add(d);p=null;}else if(b.StartsWith("profile:")){q(d,"profile:");d.bL=b.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(b.StartsWith("border:")){q(d,"border:");string kF=b.Substring("border:".Length).Trim();if(p!=null)p.jF=kF;
else d.c_=kF;}else if(b.StartsWith("border_color:")){q(d,"border_color:");string kH=b.Substring("border_color:".Length).Trim();
if(p!=null)p.gg=kH;else d.el=kH;}else if(b.StartsWith("border_gradient:")||b.StartsWith("border_gradient(")){
q(d,"border_gradient");string bV=b.StartsWith("border_gradient:")? b.Substring("border_gradient:".Length): b.Substring("border_gradient".Length);
bV=bV.Trim();if(bV.StartsWith("(")&&bV.Contains(")"))bV=ax(bV,0);string[]nd=ay(bV).Select(rg=>rg.Trim()).ToArray();if(p!=null)p.gh=nd;
else d.al=nd;}else if(b.StartsWith("title_align:")){q(d,"title_align:");string nm=b.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(p!=null)p.gH=nm;else d.kf=nm;}else if(b.StartsWith("title_color:")){q(d,"title_color:");string np=b.Substring("title_color:".Length).Trim();
if(p!=null)p.gI=np;else d.kg=np;}else if(b.StartsWith("font_size:")){q(d,"font_size:");float nk=float.Parse(b.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(p!=null)p.jO=nk;else d.cb=nk;}else if(b.StartsWith("font:")){q(d,"font:");string lu=b.Substring("font:".Length).Trim();
if(p!=null)p.jM=lu;else d.jN=lu;}else if(b.StartsWith("bg:")||b.StartsWith("background:")){q(d,"bg:");string cm=b.Substring(b.IndexOf(':')+1).Trim();
a4 g7=p!=null ?(a4)p : d;if(cm.StartsWith("color(")&&cm.EndsWith(")")){string kO;g7.aE=j4(cm.Substring(6,cm.Length-7),out kO);
string kM,kN;j9(kO,out kM,out kN);g7.f9=kM;g7.ga=kN;}else if(p!=null)p.cT=cm;
else d.bA=cm;}else if(b.StartsWith("layout:")){q(d,"layout:");ot(b.Substring("layout:".Length).Trim(),d);}else if(b.StartsWith("alert_period:")){
q(d,"alert_period:");d.f6=Math.Max(0.2f,float.Parse(b.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(b.StartsWith("refresh:")){q(d,"refresh:");d.gC=int.Parse(b.Substring("refresh:".Length).Trim());}else if(b.StartsWith("output:")){
q(d,"output:");var fv=ay(b.Substring("output:".Length).Trim());if(fv.Count==0)throw new FormatException("output: requires a target block name");
d.bb=ab(fv[0]);d.bK=fv.Count>1 ? int.Parse(fv[1]): 0;}else if(b.StartsWith("width:")){q(d,"width:");
d.kj=int.Parse(b.Substring("width:".Length).Trim());}else if(b.StartsWith("boot_msg:")||b.StartsWith("boot:")){q(d,"boot_msg:");
d.jD=ab(b.Substring(b.IndexOf(':')+1).Trim());}else if(b.StartsWith("boot_stagger:")){q(d,"boot_stagger:");d.jE=int.Parse(b.Substring("boot_stagger:".Length).Trim());
}else if(b.StartsWith("boot_calibrate:")){q(d,"boot_calibrate:");string lb=b.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.ge=lb=="off" ? 0f : Math.Max(0f,float.Parse(lb,System.Globalization.CultureInfo.InvariantCulture));}else if(b.StartsWith("boot_batch:")){q(d,"boot_batch:");
d.jB=Math.Max(1,int.Parse(b.Substring("boot_batch:".Length).Trim()));}else if(b.StartsWith("boot_duration:")){q(d,"boot_duration:");
string du=b.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool lZ=du.EndsWith("s");if(lZ)du=du.Substring(0,du.Length-1).Trim();
float hu;if(float.TryParse(du,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out hu)){d.gf=lZ ? hu : hu/60f;
}}else if(b.StartsWith("boot_log:")){q(d,"boot_log:");d.ek.Add(ab(b.Substring("boot_log:".Length).Trim()));}else if(b.StartsWith("banner:")){
q(d,"banner:");d.bB=ab(b.Substring("banner:".Length).Trim());}else if(b.StartsWith("boot_logo:")){q(d,"boot_logo:");
d.bF=ab(b.Substring("boot_logo:".Length).Trim());}else if(b.StartsWith("boot_gradient:")||b.StartsWith("boot_gradient(")||b.StartsWith("banner_gradient:")||b.StartsWith("banner_gradient(")){
q(d,"boot_gradient");d.ej=j5(b);}else if(b.StartsWith("bg_gradient:")||b.StartsWith("bg_gradient(")){q(d,"bg_gradient");
(p!=null ?(a4)p : d).bC=j5(b);}else if(b.StartsWith("bg_gradient_dir:")){q(d,"bg_gradient_dir:");
(p!=null ?(a4)p : d).cY=b.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(b.StartsWith("flash:")){
q(d,"flash:");a4 hB=p!=null ?(a4)p : d;string hA=b.Substring("flash:".Length).Trim();string lw=kc(hA);
string lv,lx;j9(hA.Substring(0,hA.Length-lw.Length),out lv,out lx);hB.gn=lv;hB.gp=lx;hB.ca=new List<aw>{
new aw{gk=lw,bH="on"}};}else if(b.StartsWith("bg_texture:")){q(d,"bg_texture:");d.gb=ab(b.Substring("bg_texture:".Length).Trim());
}else if(b.StartsWith("bg_texture_color:")){q(d,"bg_texture_color:");d.jA=b.Substring("bg_texture_color:".Length).Trim();
}else if(b.StartsWith("gradient_mode:")){q(d,"gradient_mode:");d.gr=b.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(b.StartsWith("panel_bg:")){q(d,"panel_bg:");d.gA=b.Substring("panel_bg:".Length).Trim();}else if(b.StartsWith("alert_bg:")){
q(d,"alert_bg:");d.jy=b.Substring("alert_bg:".Length).Trim();}else if(b.StartsWith("boot_color:")||b.StartsWith("banner_color:")){
q(d,"boot_color:");d.jC=b.Substring(b.IndexOf(':')+1).Trim();}else if(b.StartsWith("orientation:")){q(d,"orientation:");
string qG=b.Substring("orientation:".Length).Trim().ToLowerInvariant();if(p!=null)p.j1=qG;}else if(b.StartsWith("@panel")){q(d,"@panel");
p=ov(b);d.aI.Add(p);}else if(b.StartsWith("title:")){q(d,"title:");if(p==null){p=new at{
c9="main",c1=0,db=0,dc=d.cd,dd=d.ce};d.aI.Add(p);}p.bP=ab(b.Substring("title:".Length).Trim());
}else if(b.StartsWith("line:")){q(d,"line:");if(p==null){p=new at{c9="main",c1=0,db=0,dc=d.cd,dd=d.ce}
;d.aI.Add(p);}p.aU.Add(ou(b.Substring("line:".Length).Trim()));}else{ai.aH.Add($"Line {dE + 1}: unrecognized directive: \"{b}\"");
}}catch(Exception pM){ai.aH.Add($"Line {dE + 1}: parse error ({pM.Message}) in \"{b}\"");}}foreach(var mU in ai.V){if(string.IsNullOrEmpty(mU.bb))ai.aH.Add($"@screen {mU.ba}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aO in ai.V){if(aO.bB!=null&& !ai.cU.ContainsKey(aO.bB))ai.aH.Add("Screen '"+aO.ba+"': banner '"+aO.bB+"' is not defined (@banner "+aO.bB+" ... @end)");
if(aO.bF!=null&& !ai.cU.ContainsKey(aO.bF))ai.aH.Add("Screen '"+aO.ba+"': boot_logo '"+aO.bF+"' is not defined (@banner "+aO.bF+" ... @end)");
}return ai;}static void ot(string hb,y m0){int iD=hb.IndexOf('(');if(iD<0)throw new FormatException("missing '(' in layout: specification");
string l5=hb.Substring(0,iD).Trim().ToLowerInvariant();string o4=ax(hb,iD);var gY=ay(o4);if(l5=="grid"){if(gY.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
m0.cd=Math.Max(1,int.Parse(gY[0]));m0.ce=Math.Max(1,int.Parse(gY[1]));}else{throw new FormatException($"unknown layout kind '{l5}' (expected grid)");
}}static at ov(string qw){string dY=qw.Substring("@panel".Length).Trim();int eN=dY.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(eN<0)throw new FormatException("@panel requires an at(...) positioning clause");
string qE=dY.Substring(0,eN).Trim();string kC=dY.Substring(eN);int mE=kC.IndexOf('(');string kA=ax(kC,mE);
var bU=ay(kA);if(bU.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var a_=new at{c9=qE,c1=int.Parse(bU[0]),db=int.Parse(bU[1]),dc=bU.Count>2 ? int.Parse(bU[2]): 1,dd=bU.Count>3 ? int.Parse(bU[3]): 1}
;int kt=eN+mE+kA.Length+1;if(kt<dY.Length){string av=dY.Substring(kt).Trim();int nu=av.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(nu>=0){string d4=av.Substring(nu+5).Trim();if(d4.StartsWith("(")&&d4.Contains(")"))a_.bP=ab(ax(d4,0));else if(d4.StartsWith(":")){
string bW=d4.Substring(1).Trim();if(bW.StartsWith("\"")){int lq=bW.IndexOf('"',1);a_.bP=lq>0 ? bW.Substring(1,lq-1): ab(bW);}else{int m9=bW.IndexOf(' ');
a_.bP=m9>=0 ? bW.Substring(0,m9): bW;}}}string mt=eo(av,"orientation:");if(mt!=null)a_.j1=mt.ToLowerInvariant();string ks=eo(av,"title_align:");
if(ks!=null)a_.gH=ks.ToLowerInvariant();string nr=eo(av,"title_color:");if(nr!=null)a_.gI=nr;int eS=av.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(eS>=0&& !av.Substring(eS).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !av.Substring(eS).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
a_.jF=eo(av.Substring(eS),"border:");}int eQ=av.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(eQ<0)eQ=av.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(eQ>=0){string bl=av.Substring(eQ+"border_gradient".Length).Trim();if(bl.StartsWith(":"))bl=bl.Substring(1).Trim();if(bl.StartsWith("(")&&bl.Contains(")"))bl=ax(bl,0);
a_.gh=ay(bl).Select(rh=>rh.Trim()).ToArray();}}return a_;}static string eo(string mV,string l4){int lY=mV.IndexOf(l4,StringComparison.OrdinalIgnoreCase);
if(lY<0)return null;string js=mV.Substring(lY+l4.Length).Trim();int na=js.IndexOf(' ');return na>=0 ? js.Substring(0,na): js;}static string[]j5(string ii){
int hN=ii.IndexOfAny(new[]{':','('});string dA=ii.Substring(ii[hN]==':' ? hN+1 : hN).Trim();if(dA.StartsWith("(")&&dA.Contains(")"))dA=ax(dA,0);return ay(dA).Select(ri=>ri.Trim()).ToArray();
}static List<string>ox(string pF,List<string>hy,Dictionary<string,List<string>>pi){var nx=new Dictionary<string,be>(StringComparer.OrdinalIgnoreCase);
var cG=(pF ?? "").Replace("\r\n","\n").Split('\n');var iy=new List<string>();int cy=0;while(cy<cG.Length){string iQ=cG[cy++];if(iQ.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ae=new List<string>();while(cy<cG.Length){string kq=cG[cy++].Replace("\t","    ").TrimEnd();if(kq.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ae.Add(kq);}while(ae.Count>0&&ae[0].Length==0)ae.RemoveAt(0);while(ae.Count>0&&ae[ae.Count-1].Length==0)ae.RemoveAt(ae.Count-1);int h2=int.MaxValue;
foreach(var gR in ae)if(gR.Length>0)h2=Math.Min(h2,gR.Length-gR.TrimStart().Length);for(int dL=0;dL<ae.Count;dL++)if(ae[dL].Length>0)ae[dL]=ae[dL].Substring(h2);
pi[iQ.TrimStart().Substring(8).Trim()]=ae;continue;}string bp=kb(iQ).Trim();if(bp.Length==0)continue;if(bp.StartsWith("@template")){try{
var ji=ow(bp);while(cy<cG.Length){string ps=cG[cy++];string hc=kb(ps).Trim();if(hc.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(hc.Length>0)ji.gd.Add(hc);}nx[ji.gy]=ji;}catch(Exception pN){hy.Add($"Template parse error ({pN.Message}) in \"{bp}\"");}}
else if(bp.StartsWith("@use")){try{jJ(bp,nx,iy,hy);}catch(Exception pO){hy.Add($"@use error ({pO.Message}) in \"{bp}\"");
}}else{iy.Add(bp);}}return iy;}static be ow(string qx){string iR=qx.Substring("@template".Length).Trim();int iE=iR.IndexOf('(');
if(iE<0)throw new FormatException("missing '(' in @template header");string qF=iR.Substring(0,iE).Trim();string o5=ax(iR,iE);var jj=new be{
gy=qF};foreach(var qK in ay(o5)){var ic=qK.Split(new[]{'='},2);string mz=gz(ic[0]);jj.ev.Add(mz);if(ic.Length>1)jj.gj[mz]=ab(ic[1].Trim());
}return jj;}static void jJ(string qy,Dictionary<string,be>ny,List<string>mw,List<string>pL,int lf=0){string fD=qy.Substring("@use".Length).Trim();
int ft=fD.IndexOf('(');string iq=ft>=0 ? fD.Substring(0,ft).Trim(): fD.Trim();string o6=ft>=0 ? ax(fD,ft): "";be cN=null;
if(!ny.TryGetValue(iq,out cN)){if(!jG.TryGetValue(iq,out cN))throw new FormatException($"unknown template '{iq}'");}var eU=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var l9 in cN.gj)eU[l9.Key]=l9.Value;var kz=ay(o6);for(int dl=0;dl<kz.Count;dl++){string eL=kz[dl];int hw=eL.IndexOf('=');if(hw>=0){
string qg=gz(eL.Substring(0,hw));string rS=ab(eL.Substring(hw+1).Trim());eU[qg]=rS;}else{if(dl<cN.ev.Count){string qh=cN.ev[dl];
eU[qh]=ab(eL.Trim());}}}foreach(var pr in cN.gd){string dx=pr;foreach(var fj in eU){dx=dx.Replace("$"+fj.Key,fj.Value).Replace("{"+fj.Key+"}",fj.Value);
}if(dx.StartsWith("@use")){if(lf>3)throw new FormatException("@use nested too deep");jJ(dx,ny,mw,pL,lf+1);
}else mw.Add(dx);}}static void q(y pD,string pI){if(pD==null)throw new FormatException($"'{pI}' used before any @screen declared");
}static string kb(string aN){if(string.IsNullOrEmpty(aN))return aN;bool h1=false;for(int bo=0;bo<aN.Length;bo++){char hf=aN[bo];if(hf=='"'){h1= !h1;
}else if(!h1){if(hf=='/'&&bo+1<aN.Length&&aN[bo+1]=='/'){return aN.Substring(0,bo);}if(hf=='#'){if(oh(aN,bo)){continue;}return aN.Substring(0,bo);}}}return aN;
}static bool oh(string b_,int l0){int cB=0;for(int hY=l0+1;hY<b_.Length;hY++){char cp=b_[hY];if((cp>='0'&&cp<='9')||(cp>='a'&&cp<='f')||(cp>='A'&&cp<='F'))cB++;
else break;}if(cB==3||cB==4||cB==6||cB==8){int dO=l0+1+cB;if(dO>=b_.Length||char.IsWhiteSpace(b_[dO])||b_[dO]==')'||b_[dO]==','||b_[dO]==':')return true;
}return false;}static aS os(string qz){string iS=qz.Substring("@bind".Length).Trim();int hx=iS.IndexOf('=');if(hx<0)throw new FormatException("missing '=' in @bind");
string mr=iS.Substring(0,hx).Trim();string hz=iS.Substring(hx+1).Trim();int iF=hz.IndexOf('(');if(iF<0)throw new FormatException("missing '(' in @bind target");
string e5=hz.Substring(0,iF).Trim().ToLowerInvariant();string o7=ax(hz,iF);var bi=ay(o7);if(bi.Count==0)throw new FormatException("@bind target has no arguments");
var ag=new aS{U=mr};if(e5=="dynamic"||e5=="var"||e5=="mem"){ag.a9=aF.c4;ag.aT=bi.Count>0 ? ab(bi[0].Trim()): mr;
for(int gS=0;gS<bi.Count;gS++){var fk=bi[gS].Split(new[]{':'},2);if(fk.Length==2&&fk[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double le;
if(double.TryParse(fk[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out le))ag.en=le;else ag.gi=ab(fk[1].Trim());
}}return ag;}ag.a9=e5=="group" ? aF.gs : aF.nS;string eJ=null;for(int eI=0;eI<bi.Count;eI++){var cA=bi[eI].Split(new[]{':'},2);if(cA.Length==2&&cA[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
eJ=cA[1].Trim().ToLowerInvariant();}else if(cA.Length==2&&cA[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ag.ef=cA[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ag.az.Add(ab(bi[eI].Trim()));}}if(ag.az.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ag.a9==aF.gs){if(eJ==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(eJ){case "avg": ag.cS=ar.f7;break;case "sum": ag.cS=ar.kd;break;case "min": ag.cS=ar.j0;break;case "max": ag.cS=ar.j_;
break;default: throw new FormatException($"unknown agg '{eJ}' (expected avg/sum/min/max)");}}return ag;}static bJ ou(string eR){var o=new bJ();
if(!eR.StartsWith("\""))throw new FormatException("line must start with a quoted label");int hv=eR.IndexOf('"',1);if(hv<0)throw new FormatException("unterminated quoted label");
o.S=eR.Substring(1,hv-1);string dX=eR.Substring(hv+1).Trim();int fu=dX.IndexOf('(');if(fu<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string C=dX.Substring(0,fu).Trim().ToLowerInvariant();string o3=ax(dX,fu);string Z=dX.Substring(dX.IndexOf(')',fu)+1).Trim();
var hs=o3.Trim().Split('.');if(hs.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.a5=hs[0].Trim();o.N=hs[1].Trim();if(C=="bar")o.g=e.cV;else if(C=="led")o.g=e.c7;else if(C=="spark"||C=="sparkline")o.g=e.bN;
else if(C=="hazard"||C=="stripes")o.g=e.c6;else if(C=="chevron"||C=="chevrons")o.g=e.c0;else if(C=="dots"||C=="dot")o.g=e.c3;
else if(C=="balance"||C=="horizon"||C=="centered")o.g=e.b8;else if(C=="toggle"||C=="switch")o.g=e.bQ;else if(C=="checkbox"||C=="check")o.g=e.bG;
else if(C=="status"||C=="badge"||C=="state")o.g=e.bO;else if(C=="ammo"||C=="pips")o.g=e.b7;else if(C=="flow"||C=="rate"||C=="spinner")o.g=e.bI;
else if(C=="text")o.g=e.gG;else throw new FormatException($"unknown display function '{C}'");while(Z.Length>0){int hq=Z.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int cv=Z.IndexOf('(');if(hq>=0&&(cv<0||hq<cv)){string eX=Z.Substring(hq+4).Trim();int fL=eX.IndexOf(' ');o.jH=(fL>=0 ? eX.Substring(0,fL): eX).ToLowerInvariant();
Z=(fL>=0 ? eX.Substring(fL): "").Trim();continue;}int jq=Z.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(jq>=0&&(cv<0||jq<cv)){
string f2=Z.Substring(jq+5).Trim();int fM=f2.IndexOf(' ');o.df=(fM>=0 ? f2.Substring(0,fM): f2).Trim().Trim('"');Z=(fM>=0 ? f2.Substring(fM): "").Trim();
continue;}int hH=Z.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(hH>=0&&(cv<0||hH<cv)){string e0=Z.Substring(hH+7).Trim();
int fN=e0.IndexOf(' ');o.c5=(fN>=0 ? e0.Substring(0,fN): e0).Trim().Trim('"');Z=(fN>=0 ? e0.Substring(fN): "").Trim();continue;}int fn=Z.IndexOf('(');
if(fn<0)break;string aj=Z.Substring(0,fn).Trim().ToLowerInvariant();int me=aj.LastIndexOf(' ');if(me>=0)aj=aj.Substring(me+1).Trim();
string aY=ax(Z,fn);Z=Z.Substring(Z.IndexOf(')',fn)+1).Trim();if(aj=="gradient"){o.cc=true;o.a8=ay(aY).Select(rj=>rj.Trim()).ToArray();
if(o.a8.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(aj=="flat"){o.am=aY.Trim();}else if(aj=="pattern"){
string M=aY.Trim().ToLowerInvariant();if(M=="hazard"||M=="stripes")o.g=e.c6;else if(M=="chevron"||M=="chevrons")o.g=e.c0;
else if(M=="dots"||M=="dot")o.g=e.c3;else if(M=="led")o.g=e.c7;else if(M=="balance"||M=="horizon")o.g=e.b8;
else if(M=="toggle"||M=="switch")o.g=e.bQ;else if(M=="checkbox"||M=="check")o.g=e.bG;else if(M=="status"||M=="badge"||M=="state")o.g=e.bO;
else if(M=="ammo"||M=="pips")o.g=e.b7;else if(M=="flow"||M=="rate"||M=="spinner")o.g=e.bI;}else if(aj=="color"){string p9;
o.c2=j4(aY,out p9);}else if(aj=="dir"||aj=="direction"){o.jH=aY.Trim().ToLowerInvariant();}else if(aj=="unit"){
o.df=aY.Trim().Trim('"');}else if(aj=="max"){double mm;if(double.TryParse(aY.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mm))o.eu=mm;
else{var im=aY.Trim().Split('.');if(im.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.cg=im[0].Trim();o.gx=im[1].Trim();
}}else if(aj=="format"||aj=="fmt"){o.c5=aY.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{aj}'");}}return o;}static string ax(string fH,int iw){
int hp=0;for(int dF=iw;dF<fH.Length;dF++){if(fH[dF]=='(')hp++;else if(fH[dF]==')'){hp--;if(hp==0)return fH.Substring(iw+1,dF-iw-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>ay(string o8){return o8.Split(',').Select(rk=>rk.Trim()).Where(rl=>rl.Length>0).ToList();
}static List<aw>j4(string o2,out string hD){var fF=new List<aw>();hD=null;foreach(var iG in ay(o2)){int hm=iG.LastIndexOf(':');
if(hm<0)throw new FormatException("color() entries need 'expr : color'");string fl=iG.Substring(0,hm).Trim();string k7=iG.Substring(hm+1).Trim();
if(fl.Equals("default",StringComparison.OrdinalIgnoreCase)){fF.Add(new aw{cf=true,bH=k7});}else{string ne=kc(fl);
if(hD==null)hD=fl.Substring(0,fl.Length-ne.Length).Trim();fF.Add(new aw{gk=ne,bH=k7});}}if(!fF.Any(q7=>q7.cf))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return fF;}static void j9(string q8,out string po,out string pR){var dR=(q8 ?? "").Split('.');if(dR.Length!=2||dR[0].Trim().Length==0||dR[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
po=dR[0].Trim();pR=dR[1].Trim();}static string kc(string cu){cu=cu.Trim();foreach(var qH in new[]{">=","<=","==",">","<"}){int hZ=cu.IndexOf(qH,StringComparison.Ordinal);
if(hZ>0)return cu.Substring(hZ).Trim();if(hZ==0)return cu;}throw new FormatException($"color() rule has no recognizable comparison operator: '{cu}'");}
static string ab(string b0){if(b0.Length>=2&&b0[0]=='"'&&b0[b0.Length-1]=='"')return b0.Substring(1,b0.Length-2);return b0;}}public static class nQ{public static double nP(ar oZ,List<double>cP){
if(cP==null||cP.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(oZ){case ar.f7: return cP.Average();case ar.kd: return cP.Sum();
case ar.j0: return cP.Min();case ar.j_: return cP.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class E{
public static string oG(string qq,double fw,int kG=10,string nE=null){fw=Math.Max(0,Math.Min(100,fw));int lD=(int)Math.Round(kG*(fw/100.0));
string pj=new string('#',lD)+new string('-',kG-lD);string rT=nE!=null ? nE : $"{fw:0}%";return $"{qq} [{pj}] {rT}";
}public static string oa(double bz,string jp=null,string e4=null){if(jp=="auto"){double gT=Math.Abs(bz);if(gT>=1e9)return(bz/1e9).ToString("F1")+"G";
if(gT>=1e6)return(bz/1e6).ToString("F1")+"M";if(gT>=1e3)return(bz/1e3).ToString("F1")+"k";string pP= !string.IsNullOrEmpty(e4)? e4 : "F0";return bz.ToString(pP);
}string lF= !string.IsNullOrEmpty(e4)? e4 :(Math.Abs(bz)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(jp)){return bz.ToString(lF);}return bz.ToString(lF)+jp;
}public static float oc(string qY,float ni,float jg,float eY=1.0f){string cE=(qY ?? "standard").ToLowerInvariant();
if(cE=="compact"||(cE=="auto"&&ni<=300&&jg<=300&&jg>160)){return eY*0.75f;}if(cE=="corner"||(cE=="auto"&&jg<=160)){
return eY*0.55f;}if(cE=="wide"||(cE=="auto"&&ni>=1000)){return eY*1.15f;}return eY;}public static string oE(string qr,double fx,int bm=5){
fx=Math.Max(0,Math.Min(100,fx));double eZ=(fx-50.0)/50.0;int dM=0;int d_=0;if(eZ< -0.01)dM=(int)Math.Round(-eZ*bm);
else if(eZ>0.01)d_=(int)Math.Round(eZ*bm);if(dM>bm)dM=bm;if(d_>bm)d_=bm;string qu=new string('-',bm-dM)+new string('<',dM);
string rd=new string('>',d_)+new string('-',bm-d_);return $"{qr} [{qu}|{rd}] {fx:0}%";}public static string oP(string mb,double rV){
return rV>=50.0 ? $"{mb} [====O] ON" : $"{mb} [O====] OFF";}public static string oJ(string mc,double rW){return rW>=50.0 ? $"{mc} [X] ACTIVE" : $"{mc} [ ] READY ";
}public static string oO(string ie,double nD){if(nD>=75.0)return $"{ie} [ ONLINE  ]";if(nD>=25.0)return $"{ie} [ STANDBY ]";return $"{ie} [ OFFLINE ]";
}public static string oD(string qs,double fy,int mi=8){fy=Math.Max(0,Math.Min(100,fy));int lE=(int)Math.Round(mi*(fy/100.0));
string qV=new string('|',lE)+new string('.',mi-lE);return $"{qs} [{qV}] {fy:0}%";}public static string oK(string ig,double dV,int nt=0){
char[]rw=new[]{'|','/','-','\\'};char mW=rw[(nt>=0 ? nt : 0)%4];if(dV>55.0)return $"{ig} {mW} ▲ CHG {dV:0}%";if(dV<45.0)return $"{ig} {mW} ▼ DSC {dV:0}%";
return $"{ig} ● STBY {dV:0}%";}public static bool ol(string ak,string dI){if(ak==null||dI==null)return false;if(ak=="*"||ak==dI)return true;
if(!ak.Contains("*")&& !ak.Contains("?"))return string.Equals(ak,dI,StringComparison.OrdinalIgnoreCase);int ap=0,dG=0,i9= -1,il=0;while(dG<dI.Length){
if(ap<ak.Length&&(ak[ap]=='?'||char.ToLowerInvariant(ak[ap])==char.ToLowerInvariant(dI[dG]))){ap++;dG++;}else if(ap<ak.Length&&ak[ap]=='*'){
i9=ap;il=dG;ap++;}else if(i9!= -1){ap=i9+1;il++;dG=il;}else{return false;}}while(ap<ak.Length&&ak[ap]=='*')ap++;return ap==ak.Length;
}public static int nU(double qL,int nw){return Math.Max(0,Math.Min(nw,1+(int)(Math.Max(0.0,qL)*nw)));}public static string eq(string[]cM,double iH){
if(cM==null||cM.Length==0)return "white";iH=Math.Max(0,Math.Min(100,iH));int h_=(int)Math.Floor(iH/100.0*cM.Length);if(h_>=cM.Length)h_=cM.Length-1;
return cM[h_];}public static n jQ(string[]bw,double qS){if(bw==null||bw.Length==0)return n.eA;if(bw.Length==1)return n.da(bw[0]);
double rb=Math.Max(0.0,Math.Min(1.0,qS/100.0));double m_=rb*(bw.Length-1);int h0=(int)Math.Floor(m_);int p8=Math.Min(bw.Length-1,h0+1);
double jd=m_-h0;n cq=n.da(bw[h0]);n hg=n.da(bw[p8]);byte q9=(byte)Math.Round(cq.bc+(hg.bc-cq.bc)*jd);
byte p3=(byte)Math.Round(cq.a7+(hg.a7-cq.a7)*jd);byte ph=(byte)Math.Round(cq.a3+(hg.a3-cq.a3)*jd);return new n(q9,p3,ph);}public static string ew(List<aw>mS,double rX){
foreach(var iU in mS){if(iU.cf)continue;if(n8(iU.gk,rX))return iU.bH;}var ld=mS.FirstOrDefault(ra=>ra.cf);return ld!=null ? ld.bH : null;
}static bool n8(string dy,double ea){dy=dy.Trim();foreach(var iv in new[]{">=","<=","==",">","<"}){if(dy.StartsWith(iv)){double dZ=double.Parse(dy.Substring(iv.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(iv){case ">=": return ea>=dZ;case "<=": return ea<=dZ;case "==": return Math.Abs(ea-dZ)<1e-9;case ">": return ea>dZ;case "<": return ea<dZ;
}}}throw new FormatException($"unsupported comparison expression: '{dy}'");}public static List<string>oI(List<string>pC,int cQ){
var fp=new List<string>();fp.Add("+"+new string('-',cQ+2)+"+");foreach(var ij in pC){string rO=ij.Length>cQ ? ij.Substring(0,cQ): ij;
string qM=rO.PadRight(cQ);fp.Add("| "+qM+" |");}fp.Add("+"+new string('-',cQ+2)+"+");return fp;}public struct j3{public float kl,km,kk,jS;
}public static j3 nZ(int k5,int mP,int fO,int fP,int d6,int d7,float rF,float rE,float mA,float mB,float hP,float hQ){
d6=Math.Max(1,d6);d7=Math.Max(1,d7);fO=Math.Max(1,Math.Min(d6-k5,fO));fP=Math.Max(1,Math.Min(d7-mP,fP));
float pa=rF-(mA*2f);float o9=rE-(mB*2f);float k0=(pa-(hP*(d6-1)))/d6;float k_=(o9-(hQ*(d7-1)))/d7;
float q_=mA+k5*(k0+hP);float q0=mB+mP*(k_+hQ);float qZ=fO*k0+((fO-1)*hP);float qT=fP*k_+((fP-1)*hQ);return new j3{
kl=q_,km=q0,kk=qZ,jS=qT};}public struct j7{public int op;public int gv;public int W;public int an;public int de;
public int aG;public int oT;public int oU;public int n5;public int n6;
public double n7;}public static j7 n_(y bu){var k=new j7();var mD=bu.aI!=null&&bu.aI.Count>0 ? bu.aI : new List<at>{
new at{aU=bu.gw}};k.op=mD.Count;k.gv=0;k.W=0;k.an=0;if(!string.IsNullOrEmpty(bu.bA)&& !bu.bA.Equals("none",StringComparison.OrdinalIgnoreCase))k.W++;
if(!string.IsNullOrEmpty(bu.c_)&& !bu.c_.Equals("none",StringComparison.OrdinalIgnoreCase))k.W+=2;foreach(var dS in mD){
if(!string.IsNullOrEmpty(dS.cT)&& !dS.cT.Equals("none",StringComparison.OrdinalIgnoreCase))k.W++;if(!string.IsNullOrEmpty(dS.bP))k.W+=2;
k.gv+=dS.aU.Count;foreach(var qp in dS.aU){k.oT++;k.oU++;k.W+=2;switch(qp.g){case e.cV: k.de++;
k.aG++;k.W+=2;k.an++;break;case e.c7: k.aG+=10;k.W+=10;
k.an+=10;break;case e.c6: k.aG+=10;k.W+=11;k.an+=10;break;case e.c0: k.aG+=10;
k.W+=10;k.an+=10;break;case e.c3: k.aG+=12;k.W+=12;k.an+=12;
break;case e.bN: k.de++;k.aG+=8;k.W+=9;k.an+=8;break;case e.b8: k.de+=2;
k.aG++;k.W+=3;k.an++;break;case e.bQ: case e.bG: k.de++;
k.aG++;k.W+=2;k.an++;break;case e.bO: case e.bI: k.de++;k.aG+=2;
k.W+=3;k.an+=2;break;case e.b7: k.aG+=8;k.W+=8;k.an+=8;
break;default: k.an++;break;}}}k.n5=25+(k.gv*2);k.n6=180+(k.an*4);
k.n7=0.0;return k;}}