gn v;string gz=null;Dictionary<string,List<IMyTerminalBlock>>cc=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>ev=new Dictionary<string,bool>();
Dictionary<string,aN>c8=new Dictionary<string,aN>();Dictionary<string,Color>ka=new Dictionary<string,Color>();List<IMyTerminalBlock>bH=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>cb=new List<IMyTerminalBlock>();bool es;int gx;string et="";StringBuilder db=new StringBuilder();readonly Dictionary<string,double>ao=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>eu=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener c9;bool bI=false;
Dictionary<B,IMyTextSurface>bJ=new Dictionary<B,IMyTextSurface>();Dictionary<B,string>ca=new Dictionary<B,string>();
static readonly string[]jP=new string[101];static Program(){for(int eY=0;eY<=100;eY++){jP[eY]=eY+"%";}}static string jB(double rB){int dz=(int)Math.Round(rB);
if(dz<0)dz=0;else if(dz>100)dz=100;return jP[dz];}class aR{public List<MySprite>k=new List<MySprite>();public List<c0>ep=new List<c0>();
public int bw= -1;public Color[]d8;public Color d3;public bool d4;public Vector2 jJ;public bool jH=false;public List<d9>cQ=new List<d9>();
}class d9{public int ei;public Color fX,ed;public List<aq>eo,eg;public string f0,f8,f9,gb;public bool jI,gd;
}class c0{public bB jL;public int V= -1;public int ba= -1;public int gg= -1;public bool jD=false;
public List<int>aa=new List<int>();public float[]eq=new float[8];public float cO;public float d7;public float bY;
public float cN;public double ek= -99999.0;public string jK;public float O= -1f;}Dictionary<B,aR>ex=new Dictionary<B,aR>();
int kc=0;double gB=0;double aT=0;int da=0;Dictionary<B,double>aS=new Dictionary<B,double>();Dictionary<B,double>gw=new Dictionary<B,double>();
int dc=0;const string nZ="0.11.0";IMyTextSurface bb;bool gv=true;double gu=0;string kb="";string gA="";
int ew=0,gy=0;List<KeyValuePair<string,int>>b9=new List<KeyValuePair<string,int>>();const double og=120;const float nx=22f;
public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{c9=IGC.RegisterBroadcastListener("HELM");c9.SetMessageCallback("HELM_IGC");
}catch{}gp();jX();jG();}public void Main(string ko,UpdateType rx){double k7=Runtime.TimeSinceLastRun.TotalSeconds;aT+=k7;
if((rx&UpdateType.IGC)!=0||ko=="HELM_IGC"){oe();}string mw=(ko ?? "").Trim();string ak=mw.ToLowerInvariant();if(ak=="reload"){
gp();}else if(ak=="refresh"){gq();}else if(ak=="boot"){jX();}else if(ak=="console on"||ak=="console off"){gv=ak=="console on";
jG();gu=0;}else if(ak=="scan"||ak=="diagnose"||ak=="test"){oA();return;}else if(ak.StartsWith("set ")){string fr=mw.Substring(4).Trim();
int lb=fr.IndexOf('=');int ra=fr.IndexOf(' ');int iQ=lb>0 ? lb : ra;if(iQ>0){string lO=fr.Substring(0,iQ).Trim();string ni=fr.Substring(iQ+1).Trim();jl($"{lO}={ni}");
Echo($"[HELM] Set dynamic variable '{lO}' = {ni}");}return;}else if(ak.StartsWith("alert")){string d1=ak.Length>5 ? ak.Substring(5).Trim(): "toggle";if(d1=="toggle")bI= !bI;
else bI=d1=="red"||d1=="on"||d1=="1"||d1=="true";Echo($"[HELM] Red alert state: {bI}");return;}else if(Me.CustomData!=gz){
gp();}dc++;gB+=k7;if(gB>=og){gB=0;gq();}if(v==null||v.S.Count==0){
j5();ju();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var bQ=v.S[kc%v.S.Count];
kc++;gA=bQ.a5;bool gF=aS.Count>0;if(gF)oo();double k8;if(!aS.ContainsKey(bQ)&& !(gw.TryGetValue(bQ,out k8)&&aT<k8)){
if(ot(bQ))gF=true;if(bQ.go>0)gw[bQ]=aT+bQ.go/60.0;}da=gF ? 0 : da+1;Runtime.UpdateFrequency=da<=v.S.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
j5();ju();ew=Runtime.CurrentInstructionCount;if(ew>gy)gy=ew;}void ju(){if(et.Length>0)Echo(et);
if(db.Length>0)Echo(db.ToString());}List<IMyTerminalBlock>d5(bool oI=false){if(!es){cb.Clear();GridTerminalSystem.GetBlocks(cb);
bH.Clear();bH.AddRange(cb);bH.RemoveAll(oU=> !oU.IsSameConstructAs(Me));gx=cb.Count-bH.Count;es=true;
}return oI ? cb : bH;}IMyTerminalBlock jx(string l8,bool oK=false){var gN=GridTerminalSystem.GetBlockWithName(l8);if(gN==null||oK||gN.IsSameConstructAs(Me))return gN;
var gH=d5();for(int eZ=0;eZ<gH.Count;eZ++)if(gH[eZ].CustomName.IndexOf(l8,StringComparison.OrdinalIgnoreCase)>=0)return gH[eZ];return null;}void nH(string l9,List<IMyTerminalBlock>dB,bool kh=false){
var ls=GridTerminalSystem.GetBlockGroupWithName(l9);if(ls==null)return;int kx=dB.Count;var dZ=new List<IMyTerminalBlock>();ls.GetBlocks(dZ);jj(dZ,dB,kh);
if(dB.Count>kx)return;var lD=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(lD);foreach(var ns in lD){if(ns.Name.IndexOf(l9,StringComparison.OrdinalIgnoreCase)<0)continue;
dZ.Clear();ns.GetBlocks(dZ);jj(dZ,dB,kh);if(dB.Count>kx)return;}}void jj(List<IMyTerminalBlock>pH,List<IMyTerminalBlock>lM,bool oJ=false){
foreach(var gO in pH)if((oJ||gO.IsSameConstructAs(Me))&& !lM.Contains(gO))lM.Add(gO);}void gp(){gz=Me.CustomData;v=jF.n7(gz);
et="";if(v.aB.Count>0){var mG=new StringBuilder("HELM config errors:\n");foreach(var ps in v.aB)mG.AppendLine(" - "+ps);et=mG.ToString();
}c8.Clear();foreach(var gP in v.bx)if(!c8.ContainsKey(gP.R))c8[gP.R]=gP;ex.Clear();ca.Clear();aS.Clear();
gw.Clear();gq();}void oe(){try{while(c9!=null&&c9.HasPendingMessage){var l4=c9.AcceptMessage();
if(l4.Data is string){jl((string)l4.Data);}}}catch{}}void jl(string mp){if(string.IsNullOrEmpty(mp))return;var mm=mp.Split(';');
for(int hE=0;hE<mm.Length;hE++){var dN=mm[hE].Trim();if(dN.Length==0)continue;int dn=dN.IndexOf('=');if(dn<0)dn=dN.IndexOf(':');if(dn>0){string cr=dN.Substring(0,dn).Trim();
string aL=dN.Substring(dn+1).Trim();if(cr.Equals("alert",StringComparison.OrdinalIgnoreCase)){bI=aL.Equals("red",StringComparison.OrdinalIgnoreCase)||aL=="1"||aL.Equals("true",StringComparison.OrdinalIgnoreCase)||aL.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double mc;eu.Remove(cr);if(double.TryParse(aL,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mc)){
ao[cr]=mc;}else if(aL.Equals("true",StringComparison.OrdinalIgnoreCase)||aL.Equals("on",StringComparison.OrdinalIgnoreCase)){ao[cr]=100.0;
}else if(aL.Equals("false",StringComparison.OrdinalIgnoreCase)||aL.Equals("off",StringComparison.OrdinalIgnoreCase)){ao[cr]=0.0;}else{eu[cr]=aL;
}}}}}void ae(int rj,string rq){b9.Add(new KeyValuePair<string,int>(rq,rj));}static string j4(string iF,int l7){return iF.Length>l7 ? iF.Substring(0,l7): iF;
}void jG(){var ix=Me as IMyTextSurfaceProvider;if(ix==null||ix.SurfaceCount<1)return;bb=ix.GetSurface(0);bb.ContentType=gv ? ContentType.SCRIPT : ContentType.NONE;
bb.Script="";}void j5(){if(aT>=gu){gu=aT+(da>v.S.Count+1 ? 5.0 : 1.0);nF();if(gv&&bb!=null)nO();
}Echo(kb);}void nF(){b9.Clear();int gZ=0,kD=0;foreach(var kq in v.bx){List<IMyTerminalBlock>hY;if(kq.a4==az.cW){
gZ++;continue;}if(cc.TryGetValue(kq.R,out hY)){kD+=hY.Count;if(hY.Count>0)gZ++;}}int lc=v.aB.Count;ae(0,"HELM-[Kernel-(v"+nZ+")]");
ae(4,"------------------------------");ae(1,"krn-cfg");ae(2," screens "+v.S.Count+"  binds "+gZ+"/"+v.bx.Count+"  errors "+lc);
ae(2," alert "+(bI ? "RED" : "off")+"  blocks "+kD+"  docked-ignored "+gx);if(lc>0)ae(3," ! "+j4(v.aB[0],40));
ae(1,"cfg-dsp");int q7=0;foreach(var iG in v.S){if(++q7>10){ae(2," +"+(v.S.Count-10)+" more");break;}IMyTextSurface iR;string mP="--",iW="no block";
if(bJ.TryGetValue(iG,out iR)){mP=iR.SurfaceSize.X.ToString("0")+"x"+iR.SurfaceSize.Y.ToString("0");iW=aS.ContainsKey(iG)? "boot" : "live";
}ae(iW=="no block" ? 3 : 2," "+j4(iG.a5,14).PadRight(14)+" "+mP.PadRight(9)+" "+iW);}ae(1,"perf");ae(2," tick "+dc+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ae(2," instr "+ew+"  peak "+gy+"  max "+Runtime.MaxInstructionCount);ae(2," rate "+Runtime.UpdateFrequency+"  idle "+da+"  up "+(int)aT+"s");
if(gA.Length>0)ae(2," last "+gA);var mH=new StringBuilder();foreach(var p1 in b9)mH.AppendLine(p1.Key);kb=mH.ToString();}static Color nK(int rk){
switch(rk){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void nO(){Vector2 bR=bb.SurfaceSize;Vector2 h9=(bb.TextureSize-bR)/2f;int g5=30;foreach(var p2 in b9)g5=Math.Max(g5,p2.Key.Length);
Vector2 g4=bb.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float mI=Math.Min(1f,Math.Min(bR.X*0.94f/(g5*g4.X),bR.Y*0.94f/(b9.Count*g4.Y)));
float rH=h9.X+bR.X*0.03f;float nu=h9.Y+bR.Y*0.03f;var iS=new List<MySprite>();iS.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=h9+bR/2f,Size=bR,Color=new Color(12,12,16)}
);foreach(var lT in b9){cI(iS,lT.Key,nK(lT.Value),rH,nu,mI);nu+=g4.Y*mI;}using(var pC=bb.DrawFrame()){pC.AddRange(iS);}}static readonly List<string>nE=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void jX(){aS.Clear();if(v==null)return;for(int e_=0;e_<v.S.Count;e_++){var fv=v.S[e_];if(fv.f2>0f)aS[fv]=aT+(e_/Math.Max(1,fv.jn))*fv.jq/60.0;
}}void oo(){List<B>dm=null;int pN=(aS.Count+3)/4,pV=0;foreach(var e7 in aS){double ih=(aT-e7.Value)/e7.Key.f2;if(ih<0)continue;
if(ih>=1.0){if(dm==null)dm=new List<B>();dm.Add(e7.Key);continue;}if((pV++ +dc)%pN==0)nN(e7.Key,ih);}if(dm==null)return;foreach(var iH in dm){
aS.Remove(iH);ex.Remove(iH);ca.Remove(iH);}}static void cI(List<MySprite>rb,string rr,Color pe,float rI,float rK,float q4){
var m3=MySprite.CreateText(rr,"Monospace",pe,q4,TextAlignment.LEFT);m3.Position=new Vector2(rI,rK);rb.Add(m3);}static void jh(List<MySprite>rc,List<string>eC,B fy,float rJ,float rL,float q5,float qd){
string[]rh=fy.ea!=null&&fy.ea.Length>0 ? fy.ea : new[]{fy.jo};for(int du=0;du<eC.Count;
du++){m g_=C.jC(rh,eC.Count>1 ? 100.0*du/(eC.Count-1): 0.0);cI(rc,eC[du],new Color(g_.a7,g_.a3,g_.a_),rJ,rL+du*qd,q5);}}bool ol(B cz,IMyTextSurface fF){
List<string>de;if(ca.ContainsKey(cz)|| !v.cL.TryGetValue(cz.bu,out de))return false;ca[cz]="banner";Vector2 bS=fF.SurfaceSize;
Vector2 ia=(fF.TextureSize-bS)/2f;int eK=1;foreach(var p3 in de)eK=Math.Max(eK,p3.Length);Vector2 dk=fF.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float fx=Math.Min(bS.X*0.94f/(eK*dk.X),bS.Y*0.94f/(de.Count*dk.Y));var iT=new List<MySprite>();iT.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=ia+bS/2f,Size=bS,Color=string.IsNullOrEmpty(cz.bt)? new Color(10,12,16): H(cz.bt)}
);jh(iT,de,cz,ia.X+(bS.X-eK*dk.X*fx)/2f,ia.Y+(bS.Y-de.Count*dk.Y*fx)/2f,fx,dk.Y*fx);using(var pD=fF.DrawFrame()){pD.AddRange(iT);
}return true;}void nN(B cA,double mh){IMyTextSurface dX;if(!bJ.TryGetValue(cA,out dX))return;Vector2 bp=dX.SurfaceSize;
Vector2 ib=(dX.TextureSize-bp)/2f;var dF=cA.nV();string l5=cA.jp ?? "HELM TELEMETRY KERNEL ONLINE";bool h3=bp.Y>=200f;
List<string>cd;if(cA.bz==null|| !v.cL.TryGetValue(cA.bz,out cd))cd=nE;int ck=l5.Length+3;foreach(var p4 in dF)ck=Math.Max(ck,p4.Length+5);
if(h3)foreach(var p5 in cd)ck=Math.Max(ck,p5.Length);int mz=(h3 ? cd.Count+1 : 0)+2+dF.Count;Vector2 cj=dX.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bm=Math.Min(1f,Math.Min(bp.X*0.94f/(ck*cj.X),bp.Y*0.94f/(mz*cj.Y)));float fT=ib.X+(bp.X-ck*cj.X*bm)/2f;float bV=ib.Y+(bp.Y-mz*cj.Y*bm)/2f;
float fb=cj.Y*bm;var cC=new List<MySprite>();cC.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=ib+bp/2f,Size=bp,Color=new Color(10,12,16)}
);if(h3){jh(cC,cd,cA,fT,bV,bm,fb);bV+=(cd.Count+1)*fb;}cI(cC,">> "+l5,new Color(34,197,94),fT,bV,bm);bV+=fb*2f;int q8=C.nD(mh,dF.Count);
for(int e0=0;e0<q8;e0++){bool me=e0==dF.Count-1&&mh>=0.8;cI(cC,me ? "[OK]" : "[..]",me ? new Color(34,197,94): new Color(56,189,248),fT,bV,bm);cI(cC,dF[e0],new Color(203,213,225),fT+5f*cj.X*bm,bV,bm);
bV+=fb;}using(var pE=dX.DrawFrame()){pE.AddRange(cC);}}void gq(){cc.Clear();bJ.Clear();ev.Clear();
db.Clear();es=false;if(v==null)return;foreach(var Q in v.bx){var av=new List<IMyTerminalBlock>();if(Q.a4==az.cW){
string hR= !string.IsNullOrEmpty(Q.aP)? Q.aP : Q.R;if(!ao.ContainsKey(hR)){ao[hR]=Q.ee;if(Q.f5!=null)eu[hR]=Q.f5;
}cc[Q.R]=av;continue;}var m6=Q.au.Count>0 ? Q.au : new List<string>{Q.j0};foreach(var bU in m6){
if(string.IsNullOrEmpty(bU))continue;bool lN=bU.Contains("*")||bU.Contains("?");if(Q.a4==az.ge||lN){var hz=new List<IMyTerminalBlock>();
nH(bU,hz,Q.d6);if(hz.Count>0){foreach(var lz in hz){if(!av.Contains(lz))av.Add(lz);}}else{var kj=d5(Q.d6);
for(int gQ=0;gQ<kj.Count;gQ++){var fL=kj[gQ];if(lN ? C.n3(bU,fL.CustomName): string.Equals(bU,fL.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!av.Contains(fL))av.Add(fL);}}}}else{var gU=jx(bU,Q.d6);if(gU!=null&& !av.Contains(gU))av.Add(gU);}}if(av.Count==0){
oy(Q,m6,av);if(av.Count>0)ev[Q.R]=true;}cc[Q.R]=av;}foreach(var aY in v.S){
if(string.IsNullOrEmpty(aY.a6))continue;var ie=jx(aY.a6);if(ie==null){db.AppendLine($"HELM: output block '{aY.a6}' not found");
continue;}IMyTextSurface bT=null;var iy=ie as IMyTextSurfaceProvider;var k1=ie as IMyTextSurface;if(iy!=null&&aY.bC<iy.SurfaceCount){
bT=iy.GetSurface(aY.bC);}else if(k1!=null&&aY.bC==0){bT=k1;}if(bT==null){db.AppendLine($"HELM: output block '{aY.a6}' has no surface index {aY.bC}");
continue;}bT.ContentType=ContentType.SCRIPT;bT.Script="";bT.ScriptBackgroundColor=Color.Black;bJ[aY]=bT;}bH.Clear();
cb.Clear();}static readonly string[]jw={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool nT(int pW,IMyTerminalBlock r){switch(pW){case 0: return r is IMyBatteryBlock;case 1: return r is IMyReactor;case 2: return r is IMySolarPanel;
case 3: return r is IMyWindTurbine;case 4: return r is IMyGasTank;case 5: return r is IMyAirVent;case 6: return r is IMyCargoContainer;case 7: return r is IMyJumpDrive;
case 8: return r is IMyThrust;case 9: return r is IMyMotorSuspension;case 10: return r is IMyDoor;case 11: return r is IMyShipConnector;case 12: return r is IMyAssembler;
case 13: return r is IMyRefinery;case 14: return r is IMyShipDrill;case 15: return r is IMyShipWelder;case 16: return r is IMyShipGrinder;case 17: return r is IMyGasGenerator;
case 18: return r is IMyLargeTurretBase;case 19: return r is IMyProjector;case 20: return r is IMyGyro;case 21: return r is IMyGravityGeneratorBase;case 22: return r is IMyLandingGear;
case 23: return r is IMyRadioAntenna;case 24: return r is IMyBeacon;case 25: return r is IMyLightingBlock;case 26: return r is IMyPistonBase;case 27: return r is IMyMotorStator;
case 28: return r is IMyWarhead;case 29: return r is IMySensorBlock;case 30: return r is IMyCameraBlock;case 31: return r is IMyParachute;case 32: return r is IMyTurretControlBlock;
}return false;}void oy(aN o5,List<string>ro,List<IMyTerminalBlock>gV){string pQ=(o5.R+" "+string.Join(" ",ro)).ToLowerInvariant();
var hU=new List<int>();for(int e6=0;e6<jw.Length;e6++){foreach(var rG in jw[e6].Split(','))if(pQ.Contains(rG)){hU.Add(e6);break;}}if(hU.Count==0)return;
var ki=d5();var mf=new List<IMyTerminalBlock>();for(int hF=0;hF<ki.Count;hF++){var gR=ki[hF];foreach(int pX in hU){if(nT(pX,gR)){(gR.CubeGrid==Me.CubeGrid ? gV : mf).Add(gR);
break;}}}if(gV.Count==0)gV.AddRange(mf);}void oA(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(v==null||v.S.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {v.S.Count} screens, {v.bx.Count} binds");es=false;
Echo($"Blocks: {d5().Count} on this construct; {gx} on docked ships (ignored)");bH.Clear();Echo("--- BIND RESOLUTION ---");foreach(var ac in v.bx){
if(ac.a4==az.cW){string hS= !string.IsNullOrEmpty(ac.aP)? ac.aP : ac.R;double ry=ao.ContainsKey(hS)? ao[hS]: ac.ee;
Echo($"[OK] @bind {ac.R} (dynamic variable '{hS}' = {ry})");continue;}List<IMyTerminalBlock>gW;if(cc.TryGetValue(ac.R,out gW)){if(gW.Count>0){
string py=ev.ContainsKey(ac.R)&&ev[ac.R]? " [fallback: typed match]" : "";Echo($"[OK] @bind {ac.R} ({gW.Count} blocks matched{py})");
}else{string rp=string.Join(", ",ac.au.Count>0 ? ac.au : new List<string>{ac.j0});Echo($"[WARN] @bind {ac.R} -> 0 blocks matched \"{rp}\"");
}}else{Echo($"[ERR] @bind {ac.R} -> unresolved");}}if(ao.Count>0){Echo($"--- DYNAMIC VARIABLES ({ao.Count}) ---");foreach(var lV in ao)Echo($"[VAR] {lV.Key} = {lV.Value}");
}IMyTextSurface gI=null;foreach(var qq in bJ.Values){gI=qq;break;}if(gI!=null){var iU=new List<string>();gI.GetSprites(iU);
Echo($"--- SPRITES ({iU.Count}): names usable in bg_texture ---");Echo(string.Join(", ",iU.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var aZ in v.S){IMyTextSurface m_;if(bJ.TryGetValue(aZ,out m_)){var mQ=m_.SurfaceSize;var rf=C.nJ(aZ);
Echo($"[OK] Screen \"{aZ.a5}\" [{aZ.bD}] -> {aZ.a6} [#{aZ.bC}] ({mQ.X:0}x{mQ.Y:0} px, {rf.T} sprites)");
}else{Echo($"[ERR] Screen \"{aZ.a5}\" -> Missing block \"{aZ.a6}\" [#{aZ.bC}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double oz(IMyTerminalBlock s){
if(s==null)return 0.0;var eE=s as IMyBatteryBlock;if(eE!=null)return eE.MaxStoredPower>0 ?(eE.CurrentStoredPower/eE.MaxStoredPower)*100.0 : 0.0;
var e5=s as IMyJumpDrive;if(e5!=null)return e5.MaxStoredPower>0 ?(e5.CurrentStoredPower/e5.MaxStoredPower)*100.0 : 0.0;var m5=s as IMyGasTank;
if(m5!=null)return m5.FilledRatio*100.0;var np=s as IMyAirVent;if(np!=null)return np.GetOxygenLevel()*100.0;var kK=s as IMyCargoContainer;if(kK!=null){
var e3=kK.GetInventory();if(e3!=null&&e3.MaxVolume.RawValue>0)return(double)e3.CurrentVolume.RawValue/e3.MaxVolume.RawValue*100.0;return 0.0;}var fq=s as IMyReactor;
if(fq!=null)return fq.MaxOutput>0 ?(fq.CurrentOutput/fq.MaxOutput)*100.0 : 0.0;var fz=s as IMySolarPanel;if(fz!=null)return fz.MaxOutput>0 ?(fz.CurrentOutput/fz.MaxOutput)*100.0 : 0.0;
var fS=s as IMyWindTurbine;if(fS!=null)return fS.MaxOutput>0 ?(fS.CurrentOutput/fS.MaxOutput)*100.0 : 0.0;var k2=s as IMyDoor;if(k2!=null)return(double)k2.OpenRatio*100.0;
var fO=s as IMyThrust;if(fO!=null)return fO.MaxEffectiveThrust>0 ?(fO.CurrentThrust/fO.MaxEffectiveThrust)*100.0 : 0.0;var i2=s as IMyMotorSuspension;
if(i2!=null)return i2.Brake ? 100.0 :(i2.Friction*100.0);var g9=s as IMyShipConnector;if(g9!=null)return g9.Status==MyShipConnectorStatus.Connected ? 100.0 :(g9.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var mu=s as IMyProductionBlock;if(mu!=null)return mu.IsProducing ? 100.0 : 0.0;var k6=s as IMyShipDrill;if(k6!=null)return k6.Enabled ? 100.0 : 0.0;
var i7=s as IMyShipToolBase;if(i7!=null)return i7.IsActivated ? 100.0 :(i7.Enabled ? 50.0 : 0.0);var hw=s as IMyGasGenerator;if(hw!=null)return hw.AutoRefill ? 100.0 :(hw.Enabled ? 50.0 : 0.0);
var fP=s as IMyLargeTurretBase;if(fP!=null)return fP.IsShooting ? 100.0 :(fP.HasTarget ? 50.0 :(fP.Enabled ? 10.0 : 0.0));var dP=s as IMyProjector;
if(dP!=null)return dP.TotalBlocks>0 ?((double)(dP.TotalBlocks-dP.RemainingBlocks)/dP.TotalBlocks)*100.0 : 0.0;var hC=s as IMyGyro;if(hC!=null)return hC.GyroOverride ? 100.0 :(hC.Enabled ? 50.0 : 0.0);
var lB=s as IMyGravityGeneratorBase;if(lB!=null)return Math.Min(100.0,Math.Abs(lB.GravityAcceleration)/9.81*100.0);var hv=s as IMyLandingGear;if(hv!=null)return hv.IsLocked ? 100.0 :(hv.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var kk=s as IMyRadioAntenna;if(kk!=null)return(kk.Radius/50000.0)*100.0;var kv=s as IMyBeacon;if(kv!=null)return(kv.Radius/50000.0)*100.0;
var l0=s as IMyLightingBlock;if(l0!=null)return l0.Intensity*10.0;var fo=s as IMyPistonBase;if(fo!=null)return fo.HighestPosition>0 ?(fo.CurrentPosition/fo.HighestPosition)*100.0 : 0.0;
var mx=s as IMyMotorStator;if(mx!=null)return Math.Min(100.0,Math.Abs(mx.Angle)/6.28318*100.0);var nr=s as IMyWarhead;if(nr!=null)return nr.IsArmed ? 100.0 : 0.0;
var iP=s as IMySensorBlock;if(iP!=null)return!iP.LastDetectedEntity.IsEmpty()? 100.0 :(iP.Enabled ? 25.0 : 0.0);var g2=s as IMyCameraBlock;
if(g2!=null)return g2.IsActive ? 100.0 :(g2.Enabled ? 50.0 : 0.0);var kQ=s as IMyParachute;if(kQ!=null)return(double)kQ.OpenRatio*100.0;
var fN=s as IMyTurretControlBlock;if(fN!=null)return fN.HasTarget ? 100.0 :(fN.IsUnderControl ? 50.0 :(fN.Enabled ? 10.0 : 0.0));var lq=s as IMyFunctionalBlock;
if(lq!=null)return lq.Enabled ? 100.0 : 0.0;return 0.0;}static double n0(IMyTerminalBlock kr,string hn){int kJ=hn.IndexOf(':');string hT=hn.Substring(0,kJ).ToLowerInvariant();
var rw=new MyItemType("MyObjectBuilder_"+(hT=="ammo" ? "AmmoMagazine" : char.ToUpper(hT[0])+hT.Substring(1)),hn.Substring(kJ+1));double mZ=0;for(int hG=0;
hG<kr.InventoryCount;hG++)mZ+=(double)kr.GetInventory(hG).GetItemAmount(rw);return mZ;}double jT(IMyTerminalBlock t,string li){if(t==null)return 0.0;
string a=li.ToLowerInvariant();if(a=="percent"||a=="pct")return oz(t);if(a.IndexOf(':')>0)return n0(t,li);var bd=t as IMyBatteryBlock;
if(bd!=null){if(a=="current"||a=="stored"||a=="val"||a=="value"||a=="raw")return bd.CurrentStoredPower;if(a=="max"||a=="capacity")return bd.MaxStoredPower;
if(a=="output")return bd.CurrentOutput;if(a=="maxoutput")return bd.MaxOutput;if(a=="input")return bd.CurrentInput;if(a=="maxinput")return bd.MaxInput;
return bd.CurrentStoredPower;}var hQ=t as IMyJumpDrive;if(hQ!=null){if(a=="max"||a=="capacity")return hQ.MaxStoredPower;return hQ.CurrentStoredPower;
}var fI=t as IMyGasTank;if(fI!=null){if(a=="max"||a=="capacity")return fI.Capacity;return fI.FilledRatio*fI.Capacity;}var nq=t as IMyAirVent;
if(nq!=null){if(a=="max"||a=="capacity")return 100.0;return nq.GetOxygenLevel()*100.0;}var kL=t as IMyCargoContainer;if(kL!=null){var e4=kL.GetInventory();
if(e4!=null){if(a=="mass")return(double)e4.CurrentMass;if(a=="max"||a=="capacity")return(double)e4.MaxVolume;return(double)e4.CurrentVolume;}return 0.0;}var iz=t as IMyPowerProducer;
if(iz!=null){if(a=="max"||a=="capacity")return iz.MaxOutput;return iz.CurrentOutput;}var k3=t as IMyDoor;if(k3!=null){if(a=="max")return 1.0;return(double)k3.OpenRatio*100.0;
}var i5=t as IMyThrust;if(i5!=null){if(a=="max"||a=="capacity")return i5.MaxEffectiveThrust;return i5.CurrentThrust;}var br=t as IMyMotorSuspension;
if(br!=null){if(a=="brake")return br.Brake ? 100.0 : 0.0;if(a=="height")return br.Height;if(a=="strength")return br.Strength;if(a=="power")return br.Power;
if(a=="steer")return br.SteeringOverride*100.0;if(a=="propulsion")return br.PropulsionOverride*100.0;return br.Friction;}var kW=t as IMyShipConnector;
if(kW!=null){return kW.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var iw=t as IMyProductionBlock;if(iw!=null){if(a=="producing"||a=="active"||a=="running"||a=="state")return iw.IsProducing ? 100.0 : 0.0;
if(a=="queue")return iw.IsQueueEmpty ? 0.0 : 1.0;}var gM=t as IMyAssembler;if(gM!=null){if(a=="disassembling")return gM.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(a=="repeating"||a=="loop")return gM.Repeating ? 100.0 : 0.0;}var hx=t as IMyGasGenerator;if(hx!=null){if(a=="autorefill")return hx.AutoRefill ? 100.0 : 0.0;
if(a=="active")return hx.Enabled ? 100.0 : 0.0;}var fQ=t as IMyLargeTurretBase;if(fQ!=null){if(a=="shooting")return fQ.IsShooting ? 100.0 : 0.0;
if(a=="target"||a=="hastarget")return fQ.HasTarget ? 100.0 : 0.0;if(a=="range")return fQ.Range;}var bl=t as IMyProjector;if(bl!=null){if(a=="built")return bl.TotalBlocks-bl.RemainingBlocks;
if(a=="remaining")return bl.RemainingBlocks;if(a=="total")return bl.TotalBlocks;if(a=="projecting")return bl.IsProjecting ? 100.0 : 0.0;if(a=="armor")return bl.RemainingArmorBlocks;
if(a=="buildable")return bl.BuildableBlocksCount;}var i8=t as IMyShipToolBase;if(i8!=null){if(a=="working"||a=="active"||a=="activated")return i8.IsActivated ? 100.0 : 0.0;
return i8.Enabled ? 100.0 : 0.0;}var co=t as IMyGyro;if(co!=null){if(a=="override")return co.GyroOverride ? 100.0 : 0.0;if(a=="pitch")return co.Pitch;
if(a=="yaw")return co.Yaw;if(a=="roll")return co.Roll;if(a=="power")return co.GyroPower*100.0;}var lC=t as IMyGravityGeneratorBase;if(lC!=null){if(a=="g"||a=="accel")return lC.GravityAcceleration;
}var lA=t as IMyLandingGear;if(lA!=null){if(a=="locked")return lA.IsLocked ? 100.0 : 0.0;}var kl=t as IMyRadioAntenna;if(kl!=null){if(a=="radius"||a=="range")return kl.Radius;
}var kw=t as IMyBeacon;if(kw!=null){if(a=="radius"||a=="range")return kw.Radius;}var h0=t as IMyLightingBlock;if(h0!=null){if(a=="intensity")return h0.Intensity;
if(a=="radius")return h0.Radius;}var fp=t as IMyPistonBase;if(fp!=null){if(a=="pos"||a=="position"||a=="extension")return fp.CurrentPosition;
if(a=="velocity")return fp.Velocity;if(a=="max")return fp.HighestPosition;}var dU=t as IMyMotorStator;if(dU!=null){if(a=="angle"||a=="deg")return dU.Angle*180.0/Math.PI;
if(a=="rad")return dU.Angle;if(a=="velocity"||a=="rpm")return dU.TargetVelocityRPM;if(a=="locked")return dU.RotorLock ? 100.0 : 0.0;}var jg=t as IMyWarhead;
if(jg!=null){if(a=="armed")return jg.IsArmed ? 100.0 : 0.0;if(a=="countdown")return jg.DetonationTime;}var mN=t as IMySensorBlock;if(mN!=null){
if(a=="detected")return!mN.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var g3=t as IMyCameraBlock;if(g3!=null){if(a=="range")return g3.AvailableScanRange;
if(a=="active")return g3.IsActive ? 100.0 : 0.0;}var kR=t as IMyParachute;if(kR!=null){if(a=="open"||a=="openratio")return(double)kR.OpenRatio*100.0;
}var cF=t as IMyTurretControlBlock;if(cF!=null){if(a=="target"||a=="hastarget")return cF.HasTarget ? 100.0 : 0.0;if(a=="azimuth")return cF.AzimuthRotor!=null ? cF.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(a=="elevation")return cF.ElevationRotor!=null ? cF.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var lr=t as IMyFunctionalBlock;if(lr!=null){return lr.Enabled ? 100.0 : 0.0;
}return 0.0;}double bE(string dh,string lj){aN be;c8.TryGetValue(dh,out be);if(be!=null&&be.a4==az.cW){
string pY= !string.IsNullOrEmpty(be.aP)? be.aP : dh;double kX;if(ao.TryGetValue(pY,out kX))return kX;return be.ee;
}double k9;if(ao.TryGetValue(dh,out k9))return k9;List<IMyTerminalBlock>di;if(!cc.TryGetValue(dh,out di)||di.Count==0)throw new InvalidOperationException($"HELM: bind '{dh}' resolved to zero blocks — check the name/typo");
if(di.Count==1)return jT(di[0],lj);var no=new List<double>();foreach(var oV in di)no.Add(jT(oV,lj));return nz.ny(be!=null ? be.cJ : am.fW,no);
}double gr(string o7){return bE(o7,"percent");}static string a2(double nl,bB cu){if(cu==null||(string.IsNullOrEmpty(cu.c7)&&string.IsNullOrEmpty(cu.cX)&&cu.L=="percent")){
return jB(nl);}return C.nU(nl,cu.c7,cu.cX);}bool ot(B cB){IMyTextSurface fG;if(!bJ.TryGetValue(cB,out fG))return false;
if(cB.bu!=null)return ol(cB,fG);if(cB.cR.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return ou(cB,fG);
}return ov(cB,fG);}static Color[]ji(aR pc,string[]ri,string pp,float nt,float nv,float je,float hD){int df=16;bool lH=pp=="horizontal";
var g8=new Color[df];for(int bN=0;bN<df;bN++){m hu=C.jC(ri,100.0*bN/(df-1));g8[bN]=new Color(hu.a7,hu.a3,hu.a_);float kI=je/df,kC=hD/df;
w(pc,lH ? new Vector2(nt+kI*(bN+0.5f),nv+hD/2f): new Vector2(nt+je/2f,nv+kC*(bN+0.5f)),lH ? new Vector2(kI+1f,hD): new Vector2(je,kC+1f),g8[bN]);}
return g8;}static void w(aR pd,Vector2 qE,Vector2 q9,Color pf,float qW=0f,string po="SquareSimple"){pd.k.Add(new MySprite{Type=SpriteType.TEXTURE,Data=po,Position=qE,Size=q9,Color=pf,RotationOrScale=qW}
);}void nG(B i,IMyTextSurface m0,aR f){f.k.Clear();f.ep.Clear();Vector2 K=m0.SurfaceSize;
Vector2 rt=m0.TextureSize;Vector2 aM=(rt-K)/2f;float fh=Math.Max(4f,K.X*0.02f);float dM=Math.Max(4f,K.Y*0.02f);
if(i.bD=="compact"||(i.bD=="auto"&&K.Y<=260f&&K.Y>150f)){fh=4f;dM=4f;}else if(i.bD=="corner"||(i.bD=="auto"&&K.Y<=150f)){
fh=2f;dM=2f;}float nf=K.X-(fh*2f);Color cf=new Color(10,12,16);bool k5=true;if(!string.IsNullOrEmpty(i.bt)){
var dt=m.c2(i.bt);if(dt.ej)k5=false;else cf=new Color(dt.a7,dt.a3,dt.a_,dt.fU);}f.bw= -1;f.d4=false;
f.d3=H(i.jk ?? "#5b0f0f");f.cQ.Clear();if(i.bv!=null&&i.bv.Length>1){f.bw=f.k.Count;f.d8=ji(f,i.bv,i.cP,aM.X+2f,aM.Y+2f,K.X-4f,K.Y-4f);
}else if(k5||i.ay!=null||i.b_!=null){if(i.ay!=null)cf=H(i.ay.First(qJ=>qJ.b5).bA);f.bw=f.k.Count;
f.d8=new[]{cf};w(f,aM+(K/2f),K-new Vector2(4f,4f),cf);if(i.ay!=null||i.b_!=null)f.cQ.Add(new d9{
ei=f.bw,fX=cf,ed=cf,eo=i.ay,f0=i.fY,f8=i.fZ,eg=i.b_,f9=i.ga,gb=i.gc,jI=true}
);}if(!string.IsNullOrEmpty(i.f_))w(f,aM+(K/2f),K-new Vector2(4f,4f),H(i.jm ?? "255,255,255,24"),0f,i.f_);
if(!i.cR.Equals("none",StringComparison.OrdinalIgnoreCase)){Color kE=i.aO!=null&&i.aO.Length>0 ? H(i.aO[0]): H(i.ec ?? "40, 60, 85");
Color pb=i.aO!=null&&i.aO.Length>1 ? H(i.aO[i.aO.Length-1]): kE;
w(f,new Vector2(aM.X+K.X/2f,aM.Y+dM*0.5f),new Vector2(nf,2f),kE);w(f,new Vector2(aM.X+K.X/2f,aM.Y+K.Y-(dM*0.5f)),new Vector2(nf,2f),pb);
}var ik=i.aC;if(ik.Count==0){ik=new List<an>{new an{c1="main",cT=0,c3=0,c4=i.b3,c5=i.b4,aQ=i.gi}
};}float lE=8f;foreach(var u in ik){var fg=C.nI(u.cT,u.c3,u.c4,u.c5,i.b3,i.b4,K.X,K.Y,fh,dM,lE,lE);
float aF=aM.X+fg.j8;float dL=aM.Y+fg.j9;float aW=fg.j7;float ff=fg.jE;bool mA=u.ay!=null||u.b_!=null;
if(u.bv!=null&&u.bv.Length>1){ji(f,u.bv,u.cP!="vertical" ? u.cP : i.cP,aF,dL,aW,ff);
}else{Color bk;if(u.ay!=null)bk=H(u.ay.First(qK=>qK.b5).bA);else if(!string.IsNullOrEmpty(u.cK))bk=H(u.cK);
else if(i.b3>1||i.b4>1)bk=string.IsNullOrEmpty(i.gm)? new Color(255,255,255,6): H(i.gm);else bk=Color.Transparent;
if(bk.A>0||mA){if(mA)f.cQ.Add(new d9{ei=f.k.Count,fX=bk,ed=bk,eo=u.ay,f0=u.fY,f8=u.fZ,eg=u.b_,f9=u.ga,gb=u.gc}
);w(f,new Vector2(aF+aW/2f,dL+ff/2f),new Vector2(aW,ff),bk);}}string ht=u.jy ?? i.jz ?? "Monospace";float o3=C.nW(i.bD,K.X,K.Y,i.b1);
float aw=u.jA ?? o3;float eX=0f;if(!string.IsNullOrEmpty(u.bG)){eX=20f*aw;var gG=TextAlignment.LEFT;float i6=aF+4f;
string ke=u.gs ?? i.j2 ?? "left";if(ke.Equals("center",StringComparison.OrdinalIgnoreCase)){gG=TextAlignment.CENTER;i6=aF+(aW/2f);
}else if(ke.Equals("right",StringComparison.OrdinalIgnoreCase)){gG=TextAlignment.RIGHT;i6=aF+aW-4f;}string m8=u.gt ?? i.j3;
Color ru= !string.IsNullOrEmpty(m8)? H(m8): H(u.f3 ?? i.ec ?? "white");var nd=MySprite.CreateText(u.bG,ht,ru,aw*0.9f,gG);
nd.Position=new Vector2(i6,dL+2f);f.k.Add(nd);string[]gS=u.f4 ?? i.aO;Color pr=gS!=null&&gS.Length>0 ? H(gS[0]): H(u.f3 ?? i.ec ?? "40, 60, 85");
w(f,new Vector2(aF+aW/2f,dL+eX-2f),new Vector2(aW-8f,1f),pr);}int qi=Math.Max(1,u.aQ.Count);float qv=ff-eX-8f;
float iD=qv/qi;float e2=6f;for(int e1=0;e1<u.aQ.Count;e1++){var x=u.aQ[e1];var N=new c0{jL=x};float qX=dL+eX+4f+(e1*iD);
float F=qX+(iD/2f);float na=F-(11f*aw);N.gg=f.k.Count;N.jD=x.P!=null&&(x.P.Contains("{val}")||x.P.Contains("{percent}"));
var lY=MySprite.CreateText(x.P,ht,Color.White,aw,TextAlignment.LEFT);lY.Position=new Vector2(aF+e2,na);f.k.Add(lY);
float qb=Math.Max(24f*aw,(x.P.Length+1)*12f*aw);float rF=(!string.IsNullOrEmpty(x.c7)|| !string.IsNullOrEmpty(x.cX))? 64f*aw : 52f*aw;
if(x.g!=e.j1){float E=aF+e2+qb;float o2=aF+aW-e2-rF;float D=Math.Max(16f,o2-E);
float M=Math.Max(x.g==e.bF ? 8f : 4f,Math.Min((x.g==e.bF ? 20f : 16f)*aw,iD*(x.g==e.bF ? 0.55f : 0.45f)));
N.cO=E;N.d7=D;N.bY=M;N.cN=F;if(x.g==e.cM){w(f,new Vector2(E+(D/2f),F),new Vector2(D,M),new Color(25,30,38));
N.V=f.k.Count;w(f,new Vector2(E,F),new Vector2(0f,M),H(x.ai));}else if(x.g==e.c_){
int iN=10;float lt=2f;float iO=Math.Max(2f,(D-(lt*(iN-1)))/iN);for(int iI=0;iI<iN;iI++){N.aa.Add(f.k.Count);
w(f,new Vector2(E+iI*(iO+lt)+(iO/2f),F),new Vector2(iO,M),new Color(22,26,32));}}else if(x.g==e.bF){w(f,new Vector2(E+(D/2f),F),new Vector2(D,M),new Color(20,24,30));
int iv=8;float lu=2f;float g6=Math.Max(2f,(D-(lu*(iv-1)))/iv);for(int ii=0;ii<iv;ii++){N.aa.Add(f.k.Count);
w(f,new Vector2(E+ii*(g6+lu)+(g6/2f),F),new Vector2(g6,2f),H(x.ai));}}else if(x.g==e.cZ){w(f,new Vector2(E+(D/2f),F),new Vector2(D,M),new Color(20,24,30));
int iX=10;float lv=2f;float iY=Math.Max(2f,(D-(lv*(iX-1)))/iX);for(int iJ=0;iJ<iX;iJ++){N.aa.Add(f.k.Count);
w(f,new Vector2(E+iJ*(iY+lv)+(iY/2f),F),new Vector2(iY,M),new Color(24,28,36),0.523599f);}}else if(x.g==e.cS){
int gK=10;float lw=2f;float gL=Math.Max(2f,(D-(lw*(gK-1)))/gK);for(int iK=0;iK<gK;iK++){N.aa.Add(f.k.Count);
w(f,new Vector2(E+iK*(gL+lw)+(gL/2f),F),new Vector2(gL,M),new Color(24,28,36),1.570796f,"Triangle");}}else if(x.g==e.cV){
int hc=12;float lx=2f;float he=Math.Max(2f,(D-(lx*(hc-1)))/hc);float k4=Math.Min(he,M);for(int iL=0;iL<hc;iL++){N.aa.Add(f.k.Count);
w(f,new Vector2(E+iL*(he+lx)+(he/2f),F),new Vector2(k4,k4),new Color(24,28,36),0f,"Circle");}}else if(x.g==e.bX){
w(f,new Vector2(E+(D/2f),F),new Vector2(D,M),new Color(25,30,38));N.V=f.k.Count;w(f,new Vector2(E+(D/2f),F),new Vector2(0f,M),H(x.ai));
w(f,new Vector2(E+(D/2f),F),new Vector2(2f,M+4f),new Color(80,110,150));}else if(x.g==e.b8){w(f,new Vector2(E+(D/2f),F),new Vector2(D,M),new Color(20,24,30));
N.V=f.k.Count;float hV=Math.Max(4f,M);w(f,new Vector2(E+hV*0.5f,F),new Vector2(hV,hV),new Color(40,50,65),0f,"Circle");
}else if(x.g==e.bZ){float cg=Math.Max(6f,Math.Min(M,14f*aw));w(f,new Vector2(E+cg*0.5f,F),new Vector2(cg,cg),new Color(28,34,46));
N.V=f.k.Count;w(f,new Vector2(E+cg*0.5f,F),new Vector2(cg*0.6f,cg*0.6f),new Color(40,50,65));}else if(x.g==e.b7){
w(f,new Vector2(E+(D/2f),F),new Vector2(D,M),new Color(18,22,30));N.V=f.k.Count;float it=Math.Max(4f,M*0.7f);
w(f,new Vector2(E+it+2f,F),new Vector2(it,it),new Color(34,197,94),0f,"Circle");}else if(x.g==e.bW){int ir=8;
float ly=2f;float iu=Math.Max(2f,(D-(ly*(ir-1)))/ir);for(int ij=0;ij<ir;ij++){N.aa.Add(f.k.Count);w(f,new Vector2(E+ij*(iu+ly)+(iu/2f),F),new Vector2(iu,M),new Color(24,28,36));
}}else if(x.g==e.b0){w(f,new Vector2(E+(D/2f),F),new Vector2(D,M),new Color(20,24,30));N.V=f.k.Count;
float lK=Math.Max(4f,M);w(f,new Vector2(E+(D/2f),F),new Vector2(lK,lK),H(x.ai),0f,"Triangle");}}var nj=MySprite.CreateText("---",ht,Color.White,aw,TextAlignment.RIGHT);
nj.Position=new Vector2(aF+aW-e2,na);N.ba=f.k.Count;f.k.Add(nj);f.ep.Add(N);}}f.jJ=K;
f.jH=true;}static void aD(aR kG,c0 mR,string rs,Color pg){var jc=kG.k[mR.ba];jc.Data=rs;jc.Color=pg;kG.k[mR.ba]=jc;
}static void jV(aR kH,c0 mS,Color ph){var ld=kH.k[mS.V];ld.Color=ph;kH.k[mS.V]=ld;}bool ov(B dW,IMyTextSurface i_){
aR h;if(!ex.TryGetValue(dW,out h)){h=new aR();ex[dW]=h;}if(!h.jH||h.jJ!=i_.SurfaceSize){
nG(dW,i_,h);}bool I=false;bool eA=(aT%dW.fV)<dW.fV/2.0;bool jf=bI&&eA&&h.d3.A>0;
if(h.bw>=0&&jf!=h.d4){h.d4=jf;for(int dv=0;dv<h.d8.Length;dv++){var ky=h.k[h.bw+dv];ky.Color=jf ? h.d3 : h.d8[dv];
h.k[h.bw+dv]=ky;}I=true;foreach(var o4 in h.cQ)o4.gd=true;}foreach(var _ in h.cQ){if(_.jI&&h.d4)continue;Color dg=_.fX;
try{if(_.eo!=null)dg=H(C.en(_.eo,bE(_.f0,_.f8)));if(_.eg!=null&&eA&&C.en(_.eg,bE(_.f9,_.gb))!=null)dg=h.d3;
}catch{}if(dg!=_.ed||_.gd){_.ed=dg;_.gd=false;var kF=h.k[_.ei];kF.Color=dg;h.k[_.ei]=kF;I=true;}}for(int hH=0;hH<h.ep.Count;
hH++){var c=h.ep[hH];var n=c.jL;double y;bool lG=false;double h6=n.el;try{y=bE(n.a1,n.L);if(n.b6!=null)h6=bE(n.b6,n.gj);
}catch{lG=true;y=0.0;}if(lG){if(c.ek!= -99999.0||(c.ba>=0&&h.k[c.ba].Data!="ERR")){I=true;c.ek= -99999.0;
if(c.ba>=0){var iM=h.k[c.ba];iM.Data="ERR";iM.Color=Color.Red;h.k[c.ba]=iM;}}continue;}double aG=y;
if(h6>0)y=Math.Min(100.0,aG/h6*100.0);string pj=n.cY ? C.eh(n.b2,y): n.ai;Color Z=H(pj);
bool aU=(Z==Color.Red);if(aU&&eA){Z=new Color(255,75,75);I=true;}bool ax=Math.Abs(aG-c.ek)>0.1;if(ax)c.ek=aG;
if(n.g==e.cM){double lk=(n.gf||n.L=="percent"||n.L=="pct")? y : gr(n.a1);if(c.O<0f)c.O=(float)lk;
float fJ=(float)lk;if(Math.Abs(c.O-fJ)>0.05f){c.O+=(fJ-c.O)*0.35f;I=true;}else if(c.O!=fJ){
c.O=fJ;I=true;}if(ax)I=true;if(I){float qx=(float)Math.Max(0.0,Math.Min(1.0,c.O/100.0));float lm=c.d7*qx;
var eR=h.k[c.V];eR.Size=new Vector2(lm,c.bY);eR.Position=new Vector2(c.cO+(lm/2f),c.cN);
eR.Color=Z;h.k[c.V]=eR;aD(h,c,a2(aG,n),Z);}}else if(n.g==e.c_||n.g==e.cZ||n.g==e.cS||n.g==e.cV){
if(ax||aU){I=true;double pA=(n.gf||n.L=="percent"||n.L=="pct")? y : gr(n.a1);int mL=c.aa.Count;
for(int dV=0;dV<mL;dV++){bool qk=pA>=((dV+0.4)/(double)mL*100.0);Color q6=qk ? Z : new Color(24,28,36);var mM=h.k[c.aa[dV]];
mM.Color=q6;h.k[c.aa[dV]]=mM;}aD(h,c,a2(aG,n),Z);}}else if(n.g==e.bF){
if(ax||aU){I=true;for(int eW=0;eW<7;eW++)c.eq[eW]=c.eq[eW+1];c.eq[7]=(float)y;int qD=c.aa.Count;
for(int dH=0;dH<qD;dH++){float pP=c.eq[dH];float pO=Math.Max(0.06f,Math.Min(1f,pP/100f));float kT=c.bY*pO;float pi=c.cN+(c.bY/2f)-(kT/2f);
var cl=h.k[c.aa[dH]];cl.Size=new Vector2(cl.Size.Value.X,kT);cl.Position=new Vector2(cl.Position.Value.X,pi);
cl.Color=Z;h.k[c.aa[dH]]=cl;}aD(h,c,a2(aG,n),Z);}}else if(n.g==e.bX){
if(c.O<0f)c.O=(float)y;float fK=(float)y;if(Math.Abs(c.O-fK)>0.05f){c.O+=(fK-c.O)*0.35f;
I=true;}else if(c.O!=fK){c.O=fK;I=true;}if(I||ax){float lF=c.d7*0.5f;float kO=c.cO+lF;
float k0=(float)Math.Max(-1.0,Math.Min(1.0,(c.O-50.0)/50.0));float hr=Math.Abs(k0)*lF;float pB=(k0>=0)?(kO+hr*0.5f):(kO-hr*0.5f);
var eS=h.k[c.V];eS.Size=new Vector2(hr,c.bY);eS.Position=new Vector2(pB,c.cN);eS.Color=Z;
h.k[c.V]=eS;aD(h,c,a2(aG,n),Z);}}else if(n.g==e.b8){if(ax||aU){
I=true;bool hP=y>=50.0;float lS=Math.Max(4f,c.bY);float p0=hP ?(c.cO+c.d7-lS*0.5f):(c.cO+lS*0.5f);
Color lR=hP ?(Z!=Color.White ? Z : new Color(34,197,94)): new Color(60,70,85);var hp=h.k[c.V];hp.Position=new Vector2(p0,c.cN);
hp.Color=lR;h.k[c.V]=hp;aD(h,c,hP ? "ON" : "OFF",lR);}}else if(n.g==e.bZ){if(ax||aU){
I=true;bool hO=y>=50.0;Color kP=hO ?(Z!=Color.White ? Z : new Color(56,189,248)): new Color(30,36,48);jV(h,c,kP);
aD(h,c,hO ? "ACTIVE" : "READY",hO ? kP : new Color(148,163,184));}}else if(n.g==e.b7){if(ax||aU){
I=true;string rg=y>=75.0 ? "ONLINE" :(y>=25.0 ? "STBY" : "OFFLINE");Color mW=y>=75.0 ? new Color(34,197,94):(y>=25.0 ? new Color(234,179,8): new Color(239,68,68));
jV(h,c,mW);aD(h,c,rg,mW);}}else if(n.g==e.bW){if(ax||aU){I=true;int mq=c.aa.Count;
for(int dI=0;dI<mq;dI++){bool ql=y>=((dI+0.5)/(double)mq*100.0);Color qB=ql ? Z : new Color(24,28,36);var mr=h.k[c.aa[dI]];
mr.Color=qB;h.k[c.aa[dI]]=mr;}aD(h,c,a2(aG,n),Z);}}else if(n.g==e.b0){
I=true;float ft;Color dr;string eT;bool pT=(n.L=="producing"||n.L=="active"||n.L=="running"||n.L=="state"||(n.L!="percent"&&(y<=1.0||y==100.0)));
if(pT){if(y>0.0){c.O=(c.O+0.3927f)%6.28318f;ft=c.O;dr=new Color(34,197,94);eT="RUN";}else{ft=0f;
dr=new Color(148,163,184);eT="IDLE";}}else{ft=(y>55.0)? 0f :((y<45.0)? 3.14159f : 1.57079f);dr=(y>55.0)? new Color(34,197,94):((y<45.0)? new Color(239,68,68): new Color(148,163,184));
eT=(y>55.0)? "CHG" :((y<45.0)? "DSC" : "STBY");}var hq=h.k[c.V];hq.RotationOrScale=ft;hq.Color=dr;
h.k[c.V]=hq;aD(h,c,eT,dr);}else{string eP;aN fM;c8.TryGetValue(n.a1,out fM);eu.TryGetValue(fM!=null&&fM.aP!=null ? fM.aP : n.a1,out eP);
if(ax||aU||eP!=c.jK){c.jK=eP;I=true;Color i9=n.cU!=null ? H(C.en(n.cU,y)): Color.White;
if(i9==Color.Red&&eA)i9=new Color(255,75,75);aD(h,c,eP ?? a2(aG,n),i9);}}if(c.jD&&(ax||aU)){
I=true;var l_=h.k[c.gg];string qy=jB(y);string qU=a2(aG,n);l_.Data=n.P.Replace("{percent}",qy).Replace("{val}",qU);
h.k[c.gg]=l_;}}if(I){using(var pF=i_.DrawFrame()){pF.AddRange(h.k);}}return I;}bool ou(B bn,IMyTextSurface i0){
i0.FontSize=bn.b1;var eL=new List<KeyValuePair<string,string>>();foreach(var l in bn.gi){double G=0.0;string aK;string aE=null;
double h7=l.el;try{G=bE(l.a1,l.L);if(l.b6!=null)h7=bE(l.b6,l.gj);}catch(Exception rM){
eL.Add(new KeyValuePair<string,string>($"{l.P}: [UNRESOLVED]","red"));continue;}double mv=G;if(h7>0)G=Math.Min(100.0,mv/h7*100.0);string nk=a2(mv,l);
if(l.g==e.cM){double ll=(l.gf||l.L=="percent"||l.L=="pct")? G : gr(l.a1);aK=C.om(l.P,ll,10,nk);
aE=l.cY ? C.eh(l.b2,ll): l.ai;}else if(l.g==e.bX){aK=C.ok(l.P,G,10);
aE=l.cY ? C.eh(l.b2,G): l.ai;}else if(l.g==e.b8){aK=C.ox(l.P,G);
aE=l.ai ??(G>=50.0 ? "green" : "gray");}else if(l.g==e.bZ){aK=C.oq(l.P,G);aE=l.ai ??(G>=50.0 ? "cyan" : "gray");
}else if(l.g==e.b7){aK=C.ow(l.P,G);aE=l.ai ??(G>=75.0 ? "lime" :(G>=25.0 ? "yellow" : "red"));
}else if(l.g==e.bW){aK=C.oj(l.P,G,8);aE=l.cY ? C.eh(l.b2,G):(l.ai ?? "yellow");
}else if(l.g==e.b0){bool pU=(l.L=="producing"||l.L=="active"||l.L=="running"||l.L=="state"||(l.L!="percent"&&(G<=1.0||G==100.0)));
if(pU){char[]re=new[]{'|','/','-','\\'};char q3=re[Math.Abs(dc)%4];aK=G>0 ? $"{l.P} [{q3}] RUN" : $"{l.P} [==] IDLE";
aE=G>0 ? "green" : "gray";}else{aK=C.os(l.P,G,dc);aE=l.ai ??(G>55.0 ? "green" :(G<45.0 ? "red" : "cyan"));
}}else{aK=$"{l.P}: {nk}";aE=l.cU!=null ? C.en(l.cU,G): null;}eL.Add(new KeyValuePair<string,string>(aK,aE));
}var eI=C.op(new List<string>(eL.ConvertAll(rn=>rn.Key)),cH: bn.j6);string mO=string.Join("\n",eI);
string mt;if(ca.TryGetValue(bn,out mt)&&mt==mO)return false;ca[bn]=mO;using(var pG=i0.DrawFrame()){
float nw=4f;float l1=nx*bn.b1;try{var l3=i0.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bn.b1);
if(l3.Y>0)l1=l3.Y;}catch{}for(int cp=0;cp<eI.Count;cp++){string kV=null;if(cp>0&&cp<eI.Count-1)kV=eL[cp-1].Value;
var mV=MySprite.CreateText(eI[cp],"Monospace",H(kV),bn.b1,TextAlignment.LEFT);mV.Position=new Vector2(4f,nw);pG.Add(mV);
nw+=l1;}}return true;}Color H(string fd){if(string.IsNullOrEmpty(fd))return Color.White;Color dC;if(ka.TryGetValue(fd,out dC))return dC;
m dj=m.c2(fd);dC=dj.ej ? Color.Transparent : new Color(dj.a7,dj.a3,dj.a_,dj.fU);ka[fd]=dC;return dC;}public enum az{nB,ge,cW}
public enum am{n4,fW,j_,jN,jM}public class aN{public string R;public az a4;public List<string>au=new List<string>();public string j0{
get{return au.Count>0 ? au[0]: "";}set{if(au.Count==0)au.Add(value);else au[0]=value;}}public am cJ=am.n4;
public double ee=0.0;public string f5;public string aP;public bool d6;}public enum e{j1,cM,c_,bF,cZ,cS,cV,bX,b8,bZ,b7,bW,b0}
public class aq{public string f7;public string bA;public bool b5;}public class bB{public e g;public string P;public string a1;
public string L;public bool cY;public string[]b2;public string ai;public List<aq>cU;public string jt="right";
public string c7;public string cX;public double el;public string b6,gj;public bool gf{get{return el>0||b6!=null;}}}public struct m{
public byte a7,a3,a_,fU;public bool ej;public m(byte qL,byte pI,byte oW,byte oF=255){a7=qL;a3=pI;a_=oW;fU=oF;ej=false;}public static readonly m oD=new m(0,0,0,0){
ej=true};public static readonly m er=new m(255,255,255);public static readonly m nA=new m(0,0,0);public static readonly m oh=new m(255,0,0);
public static readonly m nY=new m(0,255,0);public static readonly m n1=new m(50,205,50);public static readonly m oE=new m(255,255,0);
public static readonly m n5=new m(255,140,0);public static readonly m nL=new m(0,255,255);public static readonly m nC=new m(0,120,255);
public static readonly m n2=new m(255,0,255);public static readonly m of=new m(128,0,128);public static readonly m nX=new m(128,128,128);
public static readonly m nM=new m(60,60,60);public static m c2(string aJ){if(string.IsNullOrWhiteSpace(aJ))return er;aJ=aJ.Trim();
if(aJ.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aJ.Equals("none",StringComparison.OrdinalIgnoreCase))return oD;if(aJ.StartsWith("#")){
string bh=aJ.Substring(1).Trim();if(bh.Length==6){byte qM=Convert.ToByte(bh.Substring(0,2),16);byte pJ=Convert.ToByte(bh.Substring(2,2),16);byte oX=Convert.ToByte(bh.Substring(4,2),16);
return new m(qM,pJ,oX);}if(bh.Length==3){byte qN=Convert.ToByte(new string(bh[0],2),16);byte pK=Convert.ToByte(new string(bh[1],2),16);byte oY=Convert.ToByte(new string(bh[2],2),16);
return new m(qN,pK,oY);}}if(aJ.Contains(",")){var cx=aJ.Split(',');if(cx.Length>=3){byte qO=(byte)Math.Max(0,Math.Min(255,int.Parse(cx[0].Trim())));
byte pL=(byte)Math.Max(0,Math.Min(255,int.Parse(cx[1].Trim())));byte oZ=(byte)Math.Max(0,Math.Min(255,int.Parse(cx[2].Trim())));byte oG=cx.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(cx[3].Trim()))):(byte)255;
return new m(qO,pL,oZ,oG);}}switch(aJ.ToLowerInvariant()){case "red": return oh;case "green": return nY;case "lime": return n1;case "yellow": return oE;
case "orange": return n5;case "cyan": return nL;case "blue": return nC;case "magenta": return n2;case "purple": return of;case "white": return er;
case "black": return nA;case "gray": case "grey": return nX;case "darkgray": case "darkgrey": return nM;default: return er;}}}public class a0{
public string[]bv;public string cP="vertical";public List<aq>ay;public string fY,fZ;public List<aq>b_;public string ga,gc;
}public class an : a0{public string c1="default";public int cT=0;public int c3=0;public int c4=1;public int c5=1;public string bG;public string gs="left";
public string gt;public string jr;public string jy;public float? jA;public string cK;public string f3;public string[]f4;
public string jO="horizontal";public List<bB>aQ=new List<bB>();}public class B : a0{public string a5;public string bD="standard";
public string cR="graphic";public string ec;public string[]aO;public string j2="left";public string j3;public string jz="Monospace";
public string bt;public string jp;public int jq=10;public int jn=1;public string gm;public string f_;public string jm;
public string jk;public string bu;public string bz;public float f2=2.5f;public string jo="cyan";public string[]ea;
public List<string>eb=new List<string>();public List<string>nV(){if(eb!=null&&eb.Count>0)return eb;return new List<string>{
"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"};}public float b1=1.0f;
public int go=0;public float fV=1.6f;public int b3=1;public int b4=1;public int j6=30;public string a6;public int bC=0;
public List<an>aC=new List<an>();public List<bB>gi{get{if(aC.Count==0)aC.Add(new an{c1="main"});return aC[0].aQ;}
}}public class a9{public string gk;public List<string>em=new List<string>();public Dictionary<string,string>f6=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>f1=new List<string>();}public class gn{public List<aN>bx=new List<aN>();public List<B>S=new List<B>();
public List<string>aB=new List<string>();public Dictionary<string,List<string>>cL=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class jF{static readonly Dictionary<string,a9>js=new Dictionary<string,a9>(StringComparer.OrdinalIgnoreCase);
static jF(){oi();}static void oi(){a8("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");a8("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");a8("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");a8("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");a8("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");a8("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");a8("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");a8("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void a8(string ma,string[]qw,string o8){var fH=new a9{gk=ma}
;foreach(var qr in qw){var hW=qr.Split(new[]{'='},2);string mi=gl(hW[0]);fH.em.Add(mi);if(hW.Length>1)fH.f6[mi]=W(hW[1].Trim());
}var qj=o8.Replace("\r\n","\n").Split('\n');foreach(var p6 in qj){string mC=p6.Trim();if(mC.Length>0)fH.f1.Add(mC);}js[ma]=fH;}static string gl(string cv){
cv=cv.Trim();if(cv.StartsWith("$"))cv=cv.Substring(1);return cv.ToLowerInvariant();}public static gn n7(string pm){var af=new gn();B d=null;
an p=null;var ms=od(pm,af.aB,af.cL);for(int dw=0;dw<ms.Count;dw++){string b=ms[dw].Trim();
if(b.Length==0)continue;try{if(b.StartsWith("@bind")){af.bx.Add(n8(b));}else if(b.StartsWith("@screen")){d=new B{a5=b.Substring("@screen".Length).Trim()}
;af.S.Add(d);p=null;}else if(b.StartsWith("profile:")){q(d,"profile:");d.bD=b.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(b.StartsWith("border:")){q(d,"border:");string ks=b.Substring("border:".Length).Trim();if(p!=null)p.jr=ks;
else d.cR=ks;}else if(b.StartsWith("border_color:")){q(d,"border_color:");string ku=b.Substring("border_color:".Length).Trim();
if(p!=null)p.f3=ku;else d.ec=ku;}else if(b.StartsWith("border_gradient:")||b.StartsWith("border_gradient(")){
q(d,"border_gradient");string bL=b.StartsWith("border_gradient:")? b.Substring("border_gradient:".Length): b.Substring("border_gradient".Length);
bL=bL.Trim();if(bL.StartsWith("(")&&bL.Contains(")"))bL=ar(bL,0);string[]mX=at(bL).Select(qY=>qY.Trim()).ToArray();if(p!=null)p.f4=mX;
else d.aO=mX;}else if(b.StartsWith("title_align:")){q(d,"title_align:");string m4=b.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(p!=null)p.gs=m4;else d.j2=m4;}else if(b.StartsWith("title_color:")){q(d,"title_color:");string m7=b.Substring("title_color:".Length).Trim();
if(p!=null)p.gt=m7;else d.j3=m7;}else if(b.StartsWith("font_size:")){q(d,"font_size:");float m2=float.Parse(b.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(p!=null)p.jA=m2;else d.b1=m2;}else if(b.StartsWith("font:")){q(d,"font:");string le=b.Substring("font:".Length).Trim();
if(p!=null)p.jy=le;else d.jz=le;}else if(b.StartsWith("bg:")||b.StartsWith("background:")){q(d,"bg:");string ce=b.Substring(b.IndexOf(':')+1).Trim();
a0 gT=p!=null ?(a0)p : d;if(ce.StartsWith("color(")&&ce.EndsWith(")")){string kB;gT.ay=jR(ce.Substring(6,ce.Length-7),out kB);
string kz,kA;jW(kB,out kz,out kA);gT.fY=kz;gT.fZ=kA;}else if(p!=null)p.cK=ce;
else d.bt=ce;}else if(b.StartsWith("layout:")){q(d,"layout:");n9(b.Substring("layout:".Length).Trim(),d);}else if(b.StartsWith("alert_period:")){
q(d,"alert_period:");d.fV=Math.Max(0.2f,float.Parse(b.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(b.StartsWith("refresh:")){q(d,"refresh:");d.go=int.Parse(b.Substring("refresh:".Length).Trim());}else if(b.StartsWith("output:")){
q(d,"output:");var fk=at(b.Substring("output:".Length).Trim());if(fk.Count==0)throw new FormatException("output: requires a target block name");
d.a6=W(fk[0]);d.bC=fk.Count>1 ? int.Parse(fk[1]): 0;}else if(b.StartsWith("width:")){q(d,"width:");
d.j6=int.Parse(b.Substring("width:".Length).Trim());}else if(b.StartsWith("boot_msg:")||b.StartsWith("boot:")){q(d,"boot_msg:");
d.jp=W(b.Substring(b.IndexOf(':')+1).Trim());}else if(b.StartsWith("boot_stagger:")){q(d,"boot_stagger:");d.jq=int.Parse(b.Substring("boot_stagger:".Length).Trim());
}else if(b.StartsWith("boot_batch:")){q(d,"boot_batch:");d.jn=Math.Max(1,int.Parse(b.Substring("boot_batch:".Length).Trim()));
}else if(b.StartsWith("boot_duration:")){q(d,"boot_duration:");string dl=b.Substring("boot_duration:".Length).Trim().ToLowerInvariant();
bool lJ=dl.EndsWith("s");if(lJ)dl=dl.Substring(0,dl.Length-1).Trim();float hf;if(float.TryParse(dl,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out hf)){
d.f2=lJ ? hf : hf/60f;}}else if(b.StartsWith("boot_log:")){q(d,"boot_log:");d.eb.Add(W(b.Substring("boot_log:".Length).Trim()));
}else if(b.StartsWith("banner:")){q(d,"banner:");d.bu=W(b.Substring("banner:".Length).Trim());}else if(b.StartsWith("boot_logo:")){
q(d,"boot_logo:");d.bz=W(b.Substring("boot_logo:".Length).Trim());}else if(b.StartsWith("boot_gradient:")||b.StartsWith("boot_gradient(")||b.StartsWith("banner_gradient:")||b.StartsWith("banner_gradient(")){
q(d,"boot_gradient");d.ea=jS(b);}else if(b.StartsWith("bg_gradient:")||b.StartsWith("bg_gradient(")){q(d,"bg_gradient");
(p!=null ?(a0)p : d).bv=jS(b);}else if(b.StartsWith("bg_gradient_dir:")){q(d,"bg_gradient_dir:");
(p!=null ?(a0)p : d).cP=b.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(b.StartsWith("flash:")){
q(d,"flash:");a0 hm=p!=null ?(a0)p : d;string hl=b.Substring("flash:".Length).Trim();string lg=jZ(hl);
string lf,lh;jW(hl.Substring(0,hl.Length-lg.Length),out lf,out lh);hm.ga=lf;hm.gc=lh;hm.b_=new List<aq>{
new aq{f7=lg,bA="on"}};}else if(b.StartsWith("bg_texture:")){q(d,"bg_texture:");d.f_=W(b.Substring("bg_texture:".Length).Trim());
}else if(b.StartsWith("bg_texture_color:")){q(d,"bg_texture_color:");d.jm=b.Substring("bg_texture_color:".Length).Trim();
}else if(b.StartsWith("panel_bg:")){q(d,"panel_bg:");d.gm=b.Substring("panel_bg:".Length).Trim();}else if(b.StartsWith("alert_bg:")){
q(d,"alert_bg:");d.jk=b.Substring("alert_bg:".Length).Trim();}else if(b.StartsWith("boot_color:")||b.StartsWith("banner_color:")){
q(d,"boot_color:");d.jo=b.Substring(b.IndexOf(':')+1).Trim();}else if(b.StartsWith("orientation:")){q(d,"orientation:");
string qo=b.Substring("orientation:".Length).Trim().ToLowerInvariant();if(p!=null)p.jO=qo;}else if(b.StartsWith("@panel")){q(d,"@panel");
p=ob(b);d.aC.Add(p);}else if(b.StartsWith("title:")){q(d,"title:");if(p==null){p=new an{
c1="main",cT=0,c3=0,c4=d.b3,c5=d.b4};d.aC.Add(p);}p.bG=W(b.Substring("title:".Length).Trim());
}else if(b.StartsWith("line:")){q(d,"line:");if(p==null){p=new an{c1="main",cT=0,c3=0,c4=d.b3,c5=d.b4}
;d.aC.Add(p);}p.aQ.Add(oa(b.Substring("line:".Length).Trim()));}else{af.aB.Add($"Line {dw + 1}: unrecognized directive: \"{b}\"");
}}catch(Exception pu){af.aB.Add($"Line {dw + 1}: parse error ({pu.Message}) in \"{b}\"");}}foreach(var mD in af.S){if(string.IsNullOrEmpty(mD.a6))af.aB.Add($"@screen {mD.a5}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aI in af.S){if(aI.bu!=null&& !af.cL.ContainsKey(aI.bu))af.aB.Add("Screen '"+aI.a5+"': banner '"+aI.bu+"' is not defined (@banner "+aI.bu+" ... @end)");
if(aI.bz!=null&& !af.cL.ContainsKey(aI.bz))af.aB.Add("Screen '"+aI.a5+"': boot_logo '"+aI.bz+"' is not defined (@banner "+aI.bz+" ... @end)");
}return af;}static void n9(string gX,B mK){int il=gX.IndexOf('(');if(il<0)throw new FormatException("missing '(' in layout: specification");
string lQ=gX.Substring(0,il).Trim().ToLowerInvariant();string oN=ar(gX,il);var gJ=at(oN);if(lQ=="grid"){if(gJ.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
mK.b3=Math.Max(1,int.Parse(gJ[0]));mK.b4=Math.Max(1,int.Parse(gJ[1]));}else{throw new FormatException($"unknown layout kind '{lQ}' (expected grid)");
}}static an ob(string qe){string dR=qe.Substring("@panel".Length).Trim();int eD=dR.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(eD<0)throw new FormatException("@panel requires an at(...) positioning clause");
string qm=dR.Substring(0,eD).Trim();string kp=dR.Substring(eD);int mo=kp.IndexOf('(');string kn=ar(kp,mo);
var bK=at(kn);if(bK.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var aX=new an{c1=qm,cT=int.Parse(bK[0]),c3=int.Parse(bK[1]),c4=bK.Count>2 ? int.Parse(bK[2]): 1,c5=bK.Count>3 ? int.Parse(bK[3]): 1}
;int kg=eD+mo+kn.Length+1;if(kg<dR.Length){string ap=dR.Substring(kg).Trim();int nc=ap.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(nc>=0){string dY=ap.Substring(nc+5).Trim();if(dY.StartsWith("(")&&dY.Contains(")"))aX.bG=W(ar(dY,0));else if(dY.StartsWith(":")){
string bM=dY.Substring(1).Trim();if(bM.StartsWith("\"")){int la=bM.IndexOf('"',1);aX.bG=la>0 ? bM.Substring(1,la-1): W(bM);}else{int mT=bM.IndexOf(' ');
aX.bG=mT>=0 ? bM.Substring(0,mT): bM;}}}string md=ef(ap,"orientation:");if(md!=null)aX.jO=md.ToLowerInvariant();string kf=ef(ap,"title_align:");
if(kf!=null)aX.gs=kf.ToLowerInvariant();string m9=ef(ap,"title_color:");if(m9!=null)aX.gt=m9;int eH=ap.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(eH>=0&& !ap.Substring(eH).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !ap.Substring(eH).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
aX.jr=ef(ap.Substring(eH),"border:");}int eF=ap.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(eF<0)eF=ap.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(eF>=0){string bf=ap.Substring(eF+"border_gradient".Length).Trim();if(bf.StartsWith(":"))bf=bf.Substring(1).Trim();if(bf.StartsWith("(")&&bf.Contains(")"))bf=ar(bf,0);
aX.f4=at(bf).Select(qZ=>qZ.Trim()).ToArray();}}return aX;}static string ef(string mE,string lP){int lI=mE.IndexOf(lP,StringComparison.OrdinalIgnoreCase);
if(lI<0)return null;string jd=mE.Substring(lI+lP.Length).Trim();int mU=jd.IndexOf(' ');return mU>=0 ? jd.Substring(0,mU): jd;}static string[]jS(string h1){
int hy=h1.IndexOfAny(new[]{':','('});string ds=h1.Substring(h1[hy]==':' ? hy+1 : hy).Trim();if(ds.StartsWith("(")&&ds.Contains(")"))ds=ar(ds,0);return at(ds).Select(q_=>q_.Trim()).ToArray();
}static List<string>od(string pn,List<string>hj,Dictionary<string,List<string>>o0){var ng=new Dictionary<string,a9>(StringComparer.OrdinalIgnoreCase);
var cy=(pn ?? "").Replace("\r\n","\n").Split('\n');var ig=new List<string>();int cq=0;while(cq<cy.Length){string iA=cy[cq++];if(iA.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ab=new List<string>();while(cq<cy.Length){string kd=cy[cq++].Replace("\t","    ").TrimEnd();if(kd.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ab.Add(kd);}while(ab.Count>0&&ab[0].Length==0)ab.RemoveAt(0);while(ab.Count>0&&ab[ab.Count-1].Length==0)ab.RemoveAt(ab.Count-1);int hN=int.MaxValue;
foreach(var gC in ab)if(gC.Length>0)hN=Math.Min(hN,gC.Length-gC.TrimStart().Length);for(int dD=0;dD<ab.Count;dD++)if(ab[dD].Length>0)ab[dD]=ab[dD].Substring(hN);
o0[iA.TrimStart().Substring(8).Trim()]=ab;continue;}string bj=jY(iA).Trim();if(bj.Length==0)continue;if(bj.StartsWith("@template")){try{
var i3=oc(bj);while(cq<cy.Length){string pa=cy[cq++];string gY=jY(pa).Trim();if(gY.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(gY.Length>0)i3.f1.Add(gY);}ng[i3.gk]=i3;}catch(Exception pv){hj.Add($"Template parse error ({pv.Message}) in \"{bj}\"");}}
else if(bj.StartsWith("@use")){try{jv(bj,ng,ig,hj);}catch(Exception pw){hj.Add($"@use error ({pw.Message}) in \"{bj}\"");
}}else{ig.Add(bj);}}return ig;}static a9 oc(string qf){string iB=qf.Substring("@template".Length).Trim();int im=iB.IndexOf('(');
if(im<0)throw new FormatException("missing '(' in @template header");string qn=iB.Substring(0,im).Trim();string oO=ar(iB,im);var i4=new a9{
gk=qn};foreach(var qs in at(oO)){var hX=qs.Split(new[]{'='},2);string mj=gl(hX[0]);i4.em.Add(mj);if(hX.Length>1)i4.f6[mj]=W(hX[1].Trim());
}return i4;}static void jv(string qg,Dictionary<string,a9>nh,List<string>mg,List<string>pt,int k_=0){string fs=qg.Substring("@use".Length).Trim();
int fi=fs.IndexOf('(');string h8=fi>=0 ? fs.Substring(0,fi).Trim(): fs.Trim();string oP=fi>=0 ? ar(fs,fi): "";a9 cE=null;
if(!nh.TryGetValue(h8,out cE)){if(!js.TryGetValue(h8,out cE))throw new FormatException($"unknown template '{h8}'");}var eJ=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var lU in cE.f6)eJ[lU.Key]=lU.Value;var km=at(oP);for(int dd=0;dd<km.Count;dd++){string eB=km[dd];int hh=eB.IndexOf('=');if(hh>=0){
string pZ=gl(eB.Substring(0,hh));string rz=W(eB.Substring(hh+1).Trim());eJ[pZ]=rz;}else{if(dd<cE.em.Count){string p_=cE.em[dd];
eJ[p_]=W(eB.Trim());}}}foreach(var o9 in cE.f1){string dp=o9;foreach(var e8 in eJ){dp=dp.Replace("$"+e8.Key,e8.Value).Replace("{"+e8.Key+"}",e8.Value);
}if(dp.StartsWith("@use")){if(k_>3)throw new FormatException("@use nested too deep");jv(dp,nh,mg,pt,k_+1);
}else mg.Add(dp);}}static void q(B pl,string pq){if(pl==null)throw new FormatException($"'{pq}' used before any @screen declared");
}static string jY(string aH){if(string.IsNullOrEmpty(aH))return aH;bool hM=false;for(int bi=0;bi<aH.Length;bi++){char g0=aH[bi];if(g0=='"'){hM= !hM;
}else if(!hM){if(g0=='/'&&bi+1<aH.Length&&aH[bi+1]=='/'){return aH.Substring(0,bi);}if(g0=='#'){if(n_(aH,bi)){continue;}return aH.Substring(0,bi);}}}return aH;
}static bool n_(string bO,int lL){int ct=0;for(int hI=lL+1;hI<bO.Length;hI++){char ch=bO[hI];if((ch>='0'&&ch<='9')||(ch>='a'&&ch<='f')||(ch>='A'&&ch<='F'))ct++;
else break;}if(ct==3||ct==4||ct==6||ct==8){int dG=lL+1+ct;if(dG>=bO.Length||char.IsWhiteSpace(bO[dG])||bO[dG]==')'||bO[dG]==','||bO[dG]==':')return true;
}return false;}static aN n8(string qh){string iC=qh.Substring("@bind".Length).Trim();int hi=iC.IndexOf('=');if(hi<0)throw new FormatException("missing '=' in @bind");
string mb=iC.Substring(0,hi).Trim();string hk=iC.Substring(hi+1).Trim();int io=hk.IndexOf('(');if(io<0)throw new FormatException("missing '(' in @bind target");
string eV=hk.Substring(0,io).Trim().ToLowerInvariant();string oQ=ar(hk,io);var bc=at(oQ);if(bc.Count==0)throw new FormatException("@bind target has no arguments");
var ad=new aN{R=mb};if(eV=="dynamic"||eV=="var"||eV=="mem"){ad.a4=az.cW;ad.aP=bc.Count>0 ? W(bc[0].Trim()): mb;
for(int gD=0;gD<bc.Count;gD++){var e9=bc[gD].Split(new[]{':'},2);if(e9.Length==2&&e9[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double kZ;
if(double.TryParse(e9[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out kZ))ad.ee=kZ;else ad.f5=W(e9[1].Trim());
}}return ad;}ad.a4=eV=="group" ? az.ge : az.nB;string ez=null;for(int ey=0;ey<bc.Count;ey++){var cs=bc[ey].Split(new[]{':'},2);if(cs.Length==2&&cs[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
ez=cs[1].Trim().ToLowerInvariant();}else if(cs.Length==2&&cs[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ad.d6=cs[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ad.au.Add(W(bc[ey].Trim()));}}if(ad.au.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ad.a4==az.ge){if(ez==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(ez){case "avg": ad.cJ=am.fW;break;case "sum": ad.cJ=am.j_;break;case "min": ad.cJ=am.jN;break;case "max": ad.cJ=am.jM;
break;default: throw new FormatException($"unknown agg '{ez}' (expected avg/sum/min/max)");}}return ad;}static bB oa(string eG){var o=new bB();
if(!eG.StartsWith("\""))throw new FormatException("line must start with a quoted label");int hg=eG.IndexOf('"',1);if(hg<0)throw new FormatException("unterminated quoted label");
o.P=eG.Substring(1,hg-1);string dQ=eG.Substring(hg+1).Trim();int fj=dQ.IndexOf('(');if(fj<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string z=dQ.Substring(0,fj).Trim().ToLowerInvariant();string oM=ar(dQ,fj);string U=dQ.Substring(dQ.IndexOf(')',fj)+1).Trim();
var hd=oM.Trim().Split('.');if(hd.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.a1=hd[0].Trim();o.L=hd[1].Trim();if(z=="bar")o.g=e.cM;else if(z=="led")o.g=e.c_;else if(z=="spark"||z=="sparkline")o.g=e.bF;
else if(z=="hazard"||z=="stripes")o.g=e.cZ;else if(z=="chevron"||z=="chevrons")o.g=e.cS;else if(z=="dots"||z=="dot")o.g=e.cV;
else if(z=="balance"||z=="horizon"||z=="centered")o.g=e.bX;else if(z=="toggle"||z=="switch")o.g=e.b8;else if(z=="checkbox"||z=="check")o.g=e.bZ;
else if(z=="status"||z=="badge"||z=="state")o.g=e.b7;else if(z=="ammo"||z=="pips")o.g=e.bW;else if(z=="flow"||z=="rate"||z=="spinner")o.g=e.b0;
else if(z=="text")o.g=e.j1;else throw new FormatException($"unknown display function '{z}'");while(U.Length>0){int hb=U.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int cn=U.IndexOf('(');if(hb>=0&&(cn<0||hb<cn)){string eM=U.Substring(hb+4).Trim();int fA=eM.IndexOf(' ');o.jt=(fA>=0 ? eM.Substring(0,fA): eM).ToLowerInvariant();
U=(fA>=0 ? eM.Substring(fA): "").Trim();continue;}int jb=U.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(jb>=0&&(cn<0||jb<cn)){
string fR=U.Substring(jb+5).Trim();int fB=fR.IndexOf(' ');o.c7=(fB>=0 ? fR.Substring(0,fB): fR).Trim().Trim('"');U=(fB>=0 ? fR.Substring(fB): "").Trim();
continue;}int hs=U.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(hs>=0&&(cn<0||hs<cn)){string eQ=U.Substring(hs+7).Trim();
int fC=eQ.IndexOf(' ');o.cX=(fC>=0 ? eQ.Substring(0,fC): eQ).Trim().Trim('"');U=(fC>=0 ? eQ.Substring(fC): "").Trim();continue;}int fc=U.IndexOf('(');
if(fc<0)break;string ag=U.Substring(0,fc).Trim().ToLowerInvariant();int lZ=ag.LastIndexOf(' ');if(lZ>=0)ag=ag.Substring(lZ+1).Trim();
string aV=ar(U,fc);U=U.Substring(U.IndexOf(')',fc)+1).Trim();if(ag=="gradient"){o.cY=true;o.b2=at(aV).Select(q0=>q0.Trim()).ToArray();
if(o.b2.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(ag=="flat"){o.ai=aV.Trim();}else if(ag=="pattern"){
string J=aV.Trim().ToLowerInvariant();if(J=="hazard"||J=="stripes")o.g=e.cZ;else if(J=="chevron"||J=="chevrons")o.g=e.cS;
else if(J=="dots"||J=="dot")o.g=e.cV;else if(J=="led")o.g=e.c_;else if(J=="balance"||J=="horizon")o.g=e.bX;
else if(J=="toggle"||J=="switch")o.g=e.b8;else if(J=="checkbox"||J=="check")o.g=e.bZ;else if(J=="status"||J=="badge"||J=="state")o.g=e.b7;
else if(J=="ammo"||J=="pips")o.g=e.bW;else if(J=="flow"||J=="rate"||J=="spinner")o.g=e.b0;}else if(ag=="color"){string pS;
o.cU=jR(aV,out pS);}else if(ag=="dir"||ag=="direction"){o.jt=aV.Trim().ToLowerInvariant();}else if(ag=="unit"){
o.c7=aV.Trim().Trim('"');}else if(ag=="max"){double l6;if(double.TryParse(aV.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out l6))o.el=l6;
else{var h5=aV.Trim().Split('.');if(h5.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.b6=h5[0].Trim();o.gj=h5[1].Trim();
}}else if(ag=="format"||ag=="fmt"){o.cX=aV.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{ag}'");}}return o;}static string ar(string fw,int id){
int ha=0;for(int dx=id;dx<fw.Length;dx++){if(fw[dx]=='(')ha++;else if(fw[dx]==')'){ha--;if(ha==0)return fw.Substring(id+1,dx-id-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>at(string oR){return oR.Split(',').Select(q1=>q1.Trim()).Where(q2=>q2.Length>0).ToList();
}static List<aq>jR(string oL,out string ho){var fu=new List<aq>();ho=null;foreach(var ip in at(oL)){int g7=ip.LastIndexOf(':');
if(g7<0)throw new FormatException("color() entries need 'expr : color'");string fa=ip.Substring(0,g7).Trim();string kU=ip.Substring(g7+1).Trim();
if(fa.Equals("default",StringComparison.OrdinalIgnoreCase)){fu.Add(new aq{b5=true,bA=kU});}else{string mY=jZ(fa);
if(ho==null)ho=fa.Substring(0,fa.Length-mY.Length).Trim();fu.Add(new aq{f7=mY,bA=kU});}}if(!fu.Any(qP=>qP.b5))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return fu;}static void jW(string qQ,out string o6,out string pz){var dJ=(qQ ?? "").Split('.');if(dJ.Length!=2||dJ[0].Trim().Length==0||dJ[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
o6=dJ[0].Trim();pz=dJ[1].Trim();}static string jZ(string cm){cm=cm.Trim();foreach(var qp in new[]{">=","<=","==",">","<"}){int hJ=cm.IndexOf(qp,StringComparison.Ordinal);
if(hJ>0)return cm.Substring(hJ).Trim();if(hJ==0)return cm;}throw new FormatException($"color() rule has no recognizable comparison operator: '{cm}'");}
static string W(string bP){if(bP.Length>=2&&bP[0]=='"'&&bP[bP.Length-1]=='"')return bP.Substring(1,bP.Length-2);return bP;}}public static class nz{public static double ny(am oH,List<double>cG){
if(cG==null||cG.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(oH){case am.fW: return cG.Average();case am.j_: return cG.Sum();
case am.jN: return cG.Min();case am.jM: return cG.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class C{
public static string om(string p8,double fl,int kt=10,string nn=null){fl=Math.Max(0,Math.Min(100,fl));int ln=(int)Math.Round(kt*(fl/100.0));
string o1=new string('#',ln)+new string('-',kt-ln);string rA=nn!=null ? nn : $"{fl:0}%";return $"{p8} [{o1}] {rA}";
}public static string nU(double bs,string ja=null,string eU=null){if(ja=="auto"){double gE=Math.Abs(bs);if(gE>=1e9)return(bs/1e9).ToString("F1")+"G";
if(gE>=1e6)return(bs/1e6).ToString("F1")+"M";if(gE>=1e3)return(bs/1e3).ToString("F1")+"k";string px= !string.IsNullOrEmpty(eU)? eU : "F0";return bs.ToString(px);
}string lp= !string.IsNullOrEmpty(eU)? eU :(Math.Abs(bs)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(ja)){return bs.ToString(lp);}return bs.ToString(lp)+ja;
}public static float nW(string qF,float m1,float i1,float eN=1.0f){string cw=(qF ?? "standard").ToLowerInvariant();
if(cw=="compact"||(cw=="auto"&&m1<=300&&i1<=300&&i1>160)){return eN*0.75f;}if(cw=="corner"||(cw=="auto"&&i1<=160)){
return eN*0.55f;}if(cw=="wide"||(cw=="auto"&&m1>=1000)){return eN*1.15f;}return eN;}public static string ok(string p9,double fm,int bg=5){
fm=Math.Max(0,Math.Min(100,fm));double eO=(fm-50.0)/50.0;int dE=0;int dT=0;if(eO< -0.01)dE=(int)Math.Round(-eO*bg);
else if(eO>0.01)dT=(int)Math.Round(eO*bg);if(dE>bg)dE=bg;if(dT>bg)dT=bg;string qc=new string('-',bg-dE)+new string('<',dE);
string qV=new string('>',dT)+new string('-',bg-dT);return $"{p9} [{qc}|{qV}] {fm:0}%";}public static string ox(string lW,double rC){
return rC>=50.0 ? $"{lW} [====O] ON" : $"{lW} [O====] OFF";}public static string oq(string lX,double rD){return rD>=50.0 ? $"{lX} [X] ACTIVE" : $"{lX} [ ] READY ";
}public static string ow(string hZ,double nm){if(nm>=75.0)return $"{hZ} [ ONLINE  ]";if(nm>=25.0)return $"{hZ} [ STANDBY ]";return $"{hZ} [ OFFLINE ]";
}public static string oj(string qa,double fn,int l2=8){fn=Math.Max(0,Math.Min(100,fn));int lo=(int)Math.Round(l2*(fn/100.0));
string qC=new string('|',lo)+new string('.',l2-lo);return $"{qa} [{qC}] {fn:0}%";}public static string os(string h_,double dO,int nb=0){
char[]rd=new[]{'|','/','-','\\'};char mF=rd[(nb>=0 ? nb : 0)%4];if(dO>55.0)return $"{h_} {mF} ▲ CHG {dO:0}%";if(dO<45.0)return $"{h_} {mF} ▼ DSC {dO:0}%";
return $"{h_} ● STBY {dO:0}%";}public static bool n3(string ah,string dA){if(ah==null||dA==null)return false;if(ah=="*"||ah==dA)return true;
if(!ah.Contains("*")&& !ah.Contains("?"))return string.Equals(ah,dA,StringComparison.OrdinalIgnoreCase);int al=0,dy=0,iV= -1,h4=0;while(dy<dA.Length){
if(al<ah.Length&&(ah[al]=='?'||char.ToLowerInvariant(ah[al])==char.ToLowerInvariant(dA[dy]))){al++;dy++;}else if(al<ah.Length&&ah[al]=='*'){
iV=al;h4=dy;al++;}else if(iV!= -1){al=iV+1;h4++;dy=h4;}else{return false;}}while(al<ah.Length&&ah[al]=='*')al++;return al==ah.Length;
}public static int nD(double qt,int ne){return Math.Max(0,Math.Min(ne,1+(int)(Math.Max(0.0,qt)*ne)));}public static string eh(string[]cD,double iq){
if(cD==null||cD.Length==0)return "white";iq=Math.Max(0,Math.Min(100,iq));int hK=(int)Math.Floor(iq/100.0*cD.Length);if(hK>=cD.Length)hK=cD.Length-1;
return cD[hK];}public static m jC(string[]bq,double qz){if(bq==null||bq.Length==0)return m.er;if(bq.Length==1)return m.c2(bq[0]);
double qT=Math.Max(0.0,Math.Min(1.0,qz/100.0));double mJ=qT*(bq.Length-1);int hL=(int)Math.Floor(mJ);int pR=Math.Min(bq.Length-1,hL+1);
double iZ=mJ-hL;m ci=m.c2(bq[hL]);m g1=m.c2(bq[pR]);byte qR=(byte)Math.Round(ci.a7+(g1.a7-ci.a7)*iZ);
byte pM=(byte)Math.Round(ci.a3+(g1.a3-ci.a3)*iZ);byte o_=(byte)Math.Round(ci.a_+(g1.a_-ci.a_)*iZ);return new m(qR,pM,o_);}public static string en(List<aq>mB,double rE){
foreach(var iE in mB){if(iE.b5)continue;if(nS(iE.f7,rE))return iE.bA;}var kY=mB.FirstOrDefault(qS=>qS.b5);return kY!=null ? kY.bA : null;
}static bool nS(string dq,double d2){dq=dq.Trim();foreach(var ic in new[]{">=","<=","==",">","<"}){if(dq.StartsWith(ic)){double dS=double.Parse(dq.Substring(ic.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(ic){case ">=": return d2>=dS;case "<=": return d2<=dS;case "==": return Math.Abs(d2-dS)<1e-9;case ">": return d2>dS;case "<": return d2<dS;
}}}throw new FormatException($"unsupported comparison expression: '{dq}'");}public static List<string>op(List<string>pk,int cH){
var fe=new List<string>();fe.Add("+"+new string('-',cH+2)+"+");foreach(var h2 in pk){string rv=h2.Length>cH ? h2.Substring(0,cH): h2;
string qu=rv.PadRight(cH);fe.Add("| "+qu+" |");}fe.Add("+"+new string('-',cH+2)+"+");return fe;}public struct jQ{public float j8,j9,j7,jE;
}public static jQ nI(int kS,int my,int fD,int fE,int d_,int d0,float rm,float rl,float mk,float ml,float hA,float hB){
d_=Math.Max(1,d_);d0=Math.Max(1,d0);fD=Math.Max(1,Math.Min(d_-kS,fD));fE=Math.Max(1,Math.Min(d0-my,fE));
float oT=rm-(mk*2f);float oS=rl-(ml*2f);float kN=(oT-(hA*(d_-1)))/d_;float kM=(oS-(hB*(d0-1)))/d0;
float qH=mk+kS*(kN+hA);float qI=ml+my*(kM+hB);float qG=fD*kN+((fD-1)*hA);float qA=fE*kM+((fE-1)*hB);return new jQ{
j8=qH,j9=qI,j7=qG,jE=qA};}public struct jU{public int n6;public int gh;public int T;public int aj;public int c6;
public int aA;public int oB;public int oC;public int nP;public int nQ;
public double nR;}public static jU nJ(B bo){var j=new jU();var mn=bo.aC!=null&&bo.aC.Count>0 ? bo.aC : new List<an>{
new an{aQ=bo.gi}};j.n6=mn.Count;j.gh=0;j.T=0;j.aj=0;if(!string.IsNullOrEmpty(bo.bt)&& !bo.bt.Equals("none",StringComparison.OrdinalIgnoreCase))j.T++;
if(!string.IsNullOrEmpty(bo.cR)&& !bo.cR.Equals("none",StringComparison.OrdinalIgnoreCase))j.T+=2;foreach(var dK in mn){
if(!string.IsNullOrEmpty(dK.cK)&& !dK.cK.Equals("none",StringComparison.OrdinalIgnoreCase))j.T++;if(!string.IsNullOrEmpty(dK.bG))j.T+=2;
j.gh+=dK.aQ.Count;foreach(var p7 in dK.aQ){j.oB++;j.oC++;j.T+=2;switch(p7.g){case e.cM: j.c6++;
j.aA++;j.T+=2;j.aj++;break;case e.c_: j.aA+=10;j.T+=10;
j.aj+=10;break;case e.cZ: j.aA+=10;j.T+=11;j.aj+=10;break;case e.cS: j.aA+=10;
j.T+=10;j.aj+=10;break;case e.cV: j.aA+=12;j.T+=12;j.aj+=12;
break;case e.bF: j.c6++;j.aA+=8;j.T+=9;j.aj+=8;break;case e.bX: j.c6+=2;
j.aA++;j.T+=3;j.aj++;break;case e.b8: case e.bZ: j.c6++;
j.aA++;j.T+=2;j.aj++;break;case e.b7: case e.b0: j.c6++;j.aA+=2;
j.T+=3;j.aj+=2;break;case e.bW: j.aA+=8;j.T+=8;j.aj+=8;
break;default: j.aj++;break;}}}j.nP=25+(j.gh*2);j.nQ=180+(j.aj*4);
j.nR=0.0;return j;}}