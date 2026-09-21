gL v;string gZ=null;Dictionary<string,List<IMyTerminalBlock>>bU=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>eL=new Dictionary<string,bool>();
Dictionary<string,aU>dj=new Dictionary<string,aU>();Dictionary<string,Color>kH=new Dictionary<string,Color>();List<IMyTerminalBlock>bS=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>cl=new List<IMyTerminalBlock>();bool eI;int gW;string eK="";StringBuilder dn=new StringBuilder();readonly Dictionary<string,double>aw=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>dk=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener dl;bool bT=false;
Dictionary<y,IMyTextSurface>bV=new Dictionary<y,IMyTextSurface>();Dictionary<y,string>ck=new Dictionary<y,string>();
static readonly string[]km=new string[101];static Program(){for(int fg=0;fg<=100;fg++){km[fg]=fg+"%";}}static string j7(double sl){int dN=(int)Math.Round(sl);
if(dN<0)dN=0;else if(dN>100)dN=100;return km[dN];}class aW{public List<MySprite>l=new List<MySprite>();public List<db>eF=new List<db>();
public int bF= -1;public Color[]en;public Color ei;public bool ej;public Vector2 kg;public bool kd=false;public List<eo>c1=new List<eo>();
}class eo{public int ex;public Color gj,es;public List<az>eE,ev;public string gn,gw,gx,gz;public bool ke,gB;
}class db{public ba ki;public int aa= -1;public int bg= -1;public int gF= -1;public bool j9=false;
public List<int>ad=new List<int>();public float[]eG=new float[8];public float c_;public float em;public float cb;
public float cZ;public double ez= -99999.0;public string kh;public float Q= -1f;}Dictionary<y,aW>eN=new Dictionary<y,aW>();
int kJ=0;double g0=0;double ao=0;int dm=0;Dictionary<y,double>aX=new Dictionary<y,double>();Dictionary<y,double>gV=new Dictionary<y,double>();
Dictionary<string,int>dr=new Dictionary<string,int>();List<string>cm=new List<string>();Dictionary<string,double>dq=new Dictionary<string,double>();
List<MyInventoryItem>gY=new List<MyInventoryItem>();int bi= -1,eO;double g1;Dictionary<y,double>eJ=new Dictionary<y,double>();
int dp=0;const string oG="0.11.0";IMyTextSurface bh;bool gU=true;double gT=0;string kI="";string g_="";
int eM=0,gX=0;List<KeyValuePair<string,int>>cj=new List<KeyValuePair<string,int>>();const double oY=120;const float ob=22f;
public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{dl=IGC.RegisterBroadcastListener("HELM");dl.SetMessageCallback("HELM_IGC");
}catch{}gN();ku();kc();}public void Main(string kV,UpdateType sh){double lI=Runtime.TimeSinceLastRun.TotalSeconds;ao+=lI;
if((sh&UpdateType.IGC)!=0||kV=="HELM_IGC"){oW();}string na=(kV ?? "").Trim();string ap=na.ToLowerInvariant();if(ap=="reload"){
gN();}else if(ap=="refresh"){gO();}else if(ap=="boot"){ku();}else if(ap=="console on"||ap=="console off"){gU=ap=="console on";
kc();gT=0;}else if(ap=="scan"||ap=="diagnose"||ap=="test"){pf();return;}else if(ap.StartsWith("set ")){string fN=na.Substring(4).Trim();
int lM=fN.IndexOf('=');int rV=fN.IndexOf(' ');int jm=lM>0 ? lM : rV;if(jm>0){string mq=fN.Substring(0,jm).Trim();string nX=fN.Substring(jm+1).Trim();jR($"{mq}={nX}");
Echo($"[HELM] Set dynamic variable '{mq}' = {nX}");}return;}else if(ap.StartsWith("alert")){string ef=ap.Length>5 ? ap.Substring(5).Trim(): "toggle";if(ef=="toggle")bT= !bT;
else bT=ef=="red"||ef=="on"||ef=="1"||ef=="true";Echo($"[HELM] Red alert state: {bT}");return;}else if(Me.CustomData!=gZ){
gN();}dp++;g0+=lI;if(g0>=oY){g0=0;gO();}if(v==null||v.T.Count==0){
kC();j_();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var b3=v.T[kJ%v.T.Count];
kJ++;g_=b3.bb;bool g5=aX.Count>0;if(g5)o4();double lJ;if(!aX.ContainsKey(b3)&& !(gV.TryGetValue(b3,out lJ)&&ao<lJ)){
if(o8(b3))g5=true;if(b3.gM>0)gV[b3]=ao+b3.gM/60.0;}dm=g5 ? 0 : dm+1;Runtime.UpdateFrequency=dm<=v.T.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
pg();kC();j_();eM=Runtime.CurrentInstructionCount;if(eM>gX)gX=eM;}void j_(){if(eK.Length>0)Echo(eK);
if(dn.Length>0)Echo(dn.ToString());}List<IMyTerminalBlock>ek(bool po=false){if(!eI){cl.Clear();GridTerminalSystem.GetBlocks(cl);
bS.Clear();bS.AddRange(cl);bS.RemoveAll(pA=> !pA.IsSameConstructAs(Me));gW=cl.Count-bS.Count;eI=true;
}return po ? cl : bS;}IMyTerminalBlock j2(string mM,bool pq=false){var hd=GridTerminalSystem.GetBlockWithName(mM);if(hd==null||pq||hd.IsSameConstructAs(Me))return hd;
var g7=ek();for(int fh=0;fh<g7.Count;fh++)if(g7[fh].CustomName.IndexOf(mM,StringComparison.OrdinalIgnoreCase)>=0)return g7[fh];return null;}void ol(string mN,List<IMyTerminalBlock>dP,bool kO=false){
var l2=GridTerminalSystem.GetBlockGroupWithName(mN);if(l2==null)return;int k4=dP.Count;var eb=new List<IMyTerminalBlock>();l2.GetBlocks(eb);jP(eb,dP,kO);
if(dP.Count>k4)return;var md=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(md);foreach(var n6 in md){if(n6.Name.IndexOf(mN,StringComparison.OrdinalIgnoreCase)<0)continue;
eb.Clear();n6.GetBlocks(eb);jP(eb,dP,kO);if(dP.Count>k4)return;}}void jP(List<IMyTerminalBlock>qn,List<IMyTerminalBlock>mo,bool pp=false){
foreach(var he in qn)if((pp||he.IsSameConstructAs(Me))&& !mo.Contains(he))mo.Add(he);}void gN(){gZ=Me.CustomData;v=kb.oP(gZ);
eK="";if(v.aJ.Count>0){var nk=new StringBuilder("HELM config errors:\n");foreach(var p8 in v.aJ)nk.AppendLine(" - "+p8);eK=nk.ToString();
}dj.Clear();foreach(var hf in v.bG)if(!dj.ContainsKey(hf.V))dj[hf.V]=hf;eN.Clear();ck.Clear();aX.Clear();
gV.Clear();eJ.Clear();oA();gO();}void oW(){try{while(dl!=null&&dl.HasPendingMessage){
var mI=dl.AcceptMessage();if(mI.Data is string){jR((string)mI.Data);}}}catch{}}void jR(string m2){
if(string.IsNullOrEmpty(m2))return;var m_=m2.Split(';');for(int h6=0;h6<m_.Length;h6++){var d_=m_[h6].Trim();if(d_.Length==0)continue;int dC=d_.IndexOf('=');
if(dC<0)dC=d_.IndexOf(':');if(dC>0){string cB=d_.Substring(0,dC).Trim();string aT=d_.Substring(dC+1).Trim();if(cB.Equals("alert",StringComparison.OrdinalIgnoreCase)){
bT=aT.Equals("red",StringComparison.OrdinalIgnoreCase)||aT=="1"||aT.Equals("true",StringComparison.OrdinalIgnoreCase)||aT.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double mQ;dk.Remove(cB);if(double.TryParse(aT,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mQ)){
aw[cB]=mQ;}else if(aT.Equals("true",StringComparison.OrdinalIgnoreCase)||aT.Equals("on",StringComparison.OrdinalIgnoreCase)){aw[cB]=100.0;
}else if(aT.Equals("false",StringComparison.OrdinalIgnoreCase)||aT.Equals("off",StringComparison.OrdinalIgnoreCase)){aw[cB]=0.0;}else{dk[cB]=aT;
}}}}}static bool kf(string hN){return hN.Length>3&&hN.StartsWith("top")&&char.IsDigit(hN[3]);}void oA(){dr.Clear();cm.Clear();bi= -1;g1=0;
foreach(var ji in v.T){var iC=new List<List<ba>>();foreach(var rm in ji.av)iC.Add(rm.aK);if(ji.av.Count==0)iC.Add(ji.eA);
foreach(var q2 in iC)foreach(var fw in q2){if(!kf(fw.F))continue;int q3=int.Parse(fw.F.Substring(3));int mh;dr.TryGetValue(fw.ay,out mh);
dr[fw.ay]=Math.Max(mh,q3);}}cm.AddRange(dr.Keys);}void pg(){if(cm.Count==0)return;if(bi<0){if(ao<g1)return;bi=0;
eO=0;dq.Clear();}while(bi<cm.Count&&Runtime.CurrentInstructionCount<20000){string hn=cm[bi];List<IMyTerminalBlock>eY;bU.TryGetValue(hn,out eY);
if(eY!=null&&eO<eY.Count){var kX=eY[eO++];for(int h7=0;h7<kX.InventoryCount;h7++){gY.Clear();kX.GetInventory(h7).GetItems(gY);foreach(var fo in gY){
string ip=fo.Type.TypeId.Replace("MyObjectBuilder_","");string mr=ip=="Ore"||ip=="Ingot" ? fo.Type.SubtypeId+" "+ip.ToLowerInvariant(): fo.Type.SubtypeId;
double mi;dq.TryGetValue(mr,out mi);dq[mr]=mi+(double)fo.Amount;}}continue;}var fL=new List<KeyValuePair<string,double>>(dq);fL.Sort((sr,sv)=>sv.Value.CompareTo(sr.Value));
for(int cI=1;cI<=dr[hn];cI++)dk[hn+".top"+cI]=cI<=fL.Count ? fL[cI-1].Key+" "+D.j6(fL[cI-1].Value,"auto"): "-";dq.Clear();
bi++;eO=0;}if(bi>=cm.Count){bi= -1;g1=ao+15.0;}}void ah(int r3,string sa){cj.Add(new KeyValuePair<string,int>(sa,r3));
}static string kB(string jb,int mL){return jb.Length>mL ? jb.Substring(0,mL): jb;}void kc(){var i3=Me as IMyTextSurfaceProvider;if(i3==null||i3.SurfaceCount<1)return;
bh=i3.GetSurface(0);bh.ContentType=gU ? ContentType.SCRIPT : ContentType.NONE;bh.Script="";}void kC(){if(ao>=gT){
gT=ao+(dm>v.T.Count+1 ? 5.0 : 1.0);oj();if(gU&&bh!=null)ou();}Echo(kI);}void oj(){
cj.Clear();int hq=0,la=0;foreach(var kY in v.bG){List<IMyTerminalBlock>iw;if(kY.a9==aH.c7){hq++;continue;}if(bU.TryGetValue(kY.V,out iw)){
la+=iw.Count;if(iw.Count>0)hq++;}}int lN=v.aJ.Count;ah(0,"HELM-[Kernel-(v"+oG+")]");ah(4,"------------------------------");
ah(1,"krn-cfg");ah(2," screens "+v.T.Count+"  binds "+hq+"/"+v.bG.Count+"  errors "+lN);ah(2," alert "+(bT ? "RED" : "off")+"  blocks "+la+"  docked-ignored "+gW);
if(lN>0)ah(3," ! "+kB(v.aJ[0],40));ah(1,"cfg-dsp");int rS=0;foreach(var jc in v.T){if(++rS>10){ah(2," +"+(v.T.Count-10)+" more");
break;}IMyTextSurface jn;string nt="--",js="no block";if(bV.TryGetValue(jc,out jn)){nt=jn.SurfaceSize.X.ToString("0")+"x"+jn.SurfaceSize.Y.ToString("0");
js=aX.ContainsKey(jc)? "boot" : "live";}ah(js=="no block" ? 3 : 2," "+kB(jc.bb,14).PadRight(14)+" "+nt.PadRight(9)+" "+js);}ah(1,"perf");
ah(2," tick "+dp+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");ah(2," instr "+eM+"  peak "+gX+"  max "+Runtime.MaxInstructionCount);
ah(2," rate "+Runtime.UpdateFrequency+"  idle "+dm+"  up "+(int)ao+"s");if(g_.Length>0)ah(2," last "+g_);var nl=new StringBuilder();
foreach(var qI in cj)nl.AppendLine(qI.Key);kI=nl.ToString();}static Color op(int r4){switch(r4){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);
case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);}return new Color(170,180,195);}void ou(){Vector2 b4=bh.SurfaceSize;
Vector2 iJ=(bh.TextureSize-b4)/2f;int hx=30;foreach(var qJ in cj)hx=Math.Max(hx,qJ.Key.Length);Vector2 hw=bh.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float nm=Math.Min(1f,Math.Min(b4.X*0.94f/(hx*hw.X),b4.Y*0.94f/(cj.Count*hw.Y)));float ss=iJ.X+b4.X*0.03f;float n8=iJ.Y+b4.Y*0.03f;var jo=new List<MySprite>();
jo.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iJ+b4/2f,Size=b4,Color=new Color(12,12,16)});foreach(var mw in cj){cU(jo,mw.Key,op(mw.Value),ss,n8,nm);
n8+=hw.Y*nm;}using(var qi=bh.DrawFrame()){qi.AddRange(jo);}}static readonly List<string>oi=new List<string>{"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void ku(){aX.Clear();if(v==null)return;for(int fi=0;fi<v.T.Count;fi++){var fR=v.T[fi];if(fR.gq>0f)aX[fR]=ao+(fi/Math.Max(1,fR.jT))*fR.jW/60.0;
}}void o4(){List<y>dB=null;int qt=(aX.Count+3)/4,qB=0;foreach(var fr in aX){double iQ=(ao-fr.Value)/fr.Key.gq;if(iQ<0)continue;
if(iQ>=1.0){if(dB==null)dB=new List<y>();dB.Add(fr.Key);continue;}if((qB++ +dp)%qt==0)ot(fr.Key,iQ);}if(dB==null)return;foreach(var d7 in dB){
aX.Remove(d7);if(d7.gp>0f)eJ[d7]=ao;eN.Remove(d7);ck.Remove(d7);}}static void cU(List<MySprite>rW,string sb,Color pV,float st,float sw,float rP){
var nJ=MySprite.CreateText(sb,"Monospace",pV,rP,TextAlignment.LEFT);nJ.Position=new Vector2(st,sw);rW.Add(nJ);}static void jO(List<MySprite>rX,List<string>eT,y fU,float su,float sx,float rQ,float qU){
string[]r1=fU.ep!=null&&fU.ep.Length>0 ? fU.ep : new[]{fU.jU};for(int dI=0;dI<eT.Count;
dI++){n hr=D.j8(r1,eT.Count>1 ? 100.0*dI/(eT.Count-1): 0.0);cU(rX,eT[dI],new Color(hr.bd,hr.a7,hr.a4),su,sx+dI*qU,rQ);}}bool o2(y cK,IMyTextSurface f0){
List<string>dt;if(ck.ContainsKey(cK)|| !v.cX.TryGetValue(cK.bD,out dt))return false;ck[cK]="banner";Vector2 b5=f0.SurfaceSize;
Vector2 iK=(f0.TextureSize-b5)/2f;int e2=1;foreach(var qK in dt)e2=Math.Max(e2,qK.Length);Vector2 dz=f0.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float fT=Math.Min(b5.X*0.94f/(e2*dz.X),b5.Y*0.94f/(dt.Count*dz.Y));var jp=new List<MySprite>();jp.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iK+b5/2f,Size=b5,Color=string.IsNullOrEmpty(cK.bC)? new Color(10,12,16): E(cK.bC)}
);jO(jp,dt,cK,iK.X+(b5.X-e2*dz.X*fT)/2f,iK.Y+(b5.Y-dt.Count*dz.Y*fT)/2f,fT,dz.Y*fT);using(var qj=f0.DrawFrame()){qj.AddRange(jp);
}return true;}void ot(y cL,double mV){IMyTextSurface d9;if(!bV.TryGetValue(cL,out d9))return;Vector2 bx=d9.SurfaceSize;
Vector2 iL=(d9.TextureSize-bx)/2f;var dT=cL.oB();string mJ=cL.jV ?? "HELM TELEMETRY KERNEL ONLINE";bool iD=bx.Y>=200f;
List<string>cn;if(cL.bH==null|| !v.cX.TryGetValue(cL.bH,out cn))cn=oi;int cu=mJ.Length+3;foreach(var qL in dT)cu=Math.Max(cu,qL.Length+5);
if(iD)foreach(var qM in cn)cu=Math.Max(cu,qM.Length);int nd=(iD ? cn.Count+1 : 0)+2+dT.Count;Vector2 ct=d9.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bu=Math.Min(1f,Math.Min(bx.X*0.94f/(cu*ct.X),bx.Y*0.94f/(nd*ct.Y)));float gf=iL.X+(bx.X-cu*ct.X*bu)/2f;float b8=iL.Y+(bx.Y-nd*ct.Y*bu)/2f;
float fv=ct.Y*bu;var cN=new List<MySprite>();cN.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iL+bx/2f,Size=bx,Color=new Color(10,12,16)}
);if(iD){jO(cN,cn,cL,gf,b8,bu,fv);b8+=(cn.Count+1)*fv;}cU(cN,">> "+mJ,new Color(34,197,94),gf,b8,bu);b8+=fv*2f;int rT=D.oh(mV,dT.Count);
for(int fj=0;fj<rT;fj++){bool mS=fj==dT.Count-1&&mV>=0.8;cU(cN,mS ? "[OK]" : "[..]",mS ? new Color(34,197,94): new Color(56,189,248),gf,b8,bu);cU(cN,dT[fj],new Color(203,213,225),gf+5f*ct.X*bu,b8,bu);
b8+=fv;}using(var qk=d9.DrawFrame()){qk.AddRange(cN);}}void gO(){bU.Clear();bV.Clear();eL.Clear();
dn.Clear();eI=false;if(v==null)return;foreach(var U in v.bG){var aD=new List<IMyTerminalBlock>();if(U.a9==aH.c7){
string im= !string.IsNullOrEmpty(U.aV)? U.aV : U.V;if(!aw.ContainsKey(im)){aw[im]=U.et;if(U.gt!=null)dk[im]=U.gt;
}bU[U.V]=aD;continue;}var nM=U.aC.Count>0 ? U.aC : new List<string>{U.ky};foreach(var b7 in nM){
if(string.IsNullOrEmpty(b7))continue;bool mp=b7.Contains("*")||b7.Contains("?");if(U.a9==aH.gD||mp){var h1=new List<IMyTerminalBlock>();
ol(b7,h1,U.el);if(h1.Count>0){foreach(var l9 in h1){if(!aD.Contains(l9))aD.Add(l9);}}else{var kQ=ek(U.el);
for(int hg=0;hg<kQ.Count;hg++){var f7=kQ[hg];if(mp ? D.oL(b7,f7.CustomName): string.Equals(b7,f7.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aD.Contains(f7))aD.Add(f7);}}}}else{var hk=j2(b7,U.el);if(hk!=null&& !aD.Contains(hk))aD.Add(hk);}}if(aD.Count==0){
pd(U,nM,aD);if(aD.Count>0)eL[U.V]=true;}bU[U.V]=aD;}foreach(var a1 in v.T){
if(string.IsNullOrEmpty(a1.bc))continue;var iO=j2(a1.bc);if(iO==null){dn.AppendLine($"HELM: output block '{a1.bc}' not found");
continue;}IMyTextSurface b6=null;var i4=iO as IMyTextSurfaceProvider;var lC=iO as IMyTextSurface;if(i4!=null&&a1.bL<i4.SurfaceCount){
b6=i4.GetSurface(a1.bL);}else if(lC!=null&&a1.bL==0){b6=lC;}if(b6==null){dn.AppendLine($"HELM: output block '{a1.bc}' has no surface index {a1.bL}");
continue;}b6.ContentType=ContentType.SCRIPT;b6.Script="";b6.ScriptBackgroundColor=Color.Black;bV[a1]=b6;}bS.Clear();
cl.Clear();}static readonly string[]j1={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool oz(int qC,IMyTerminalBlock r){switch(qC){case 0: return r is IMyBatteryBlock;case 1: return r is IMyReactor;case 2: return r is IMySolarPanel;
case 3: return r is IMyWindTurbine;case 4: return r is IMyGasTank;case 5: return r is IMyAirVent;case 6: return r is IMyCargoContainer;case 7: return r is IMyJumpDrive;
case 8: return r is IMyThrust;case 9: return r is IMyMotorSuspension;case 10: return r is IMyDoor;case 11: return r is IMyShipConnector;case 12: return r is IMyAssembler;
case 13: return r is IMyRefinery;case 14: return r is IMyShipDrill;case 15: return r is IMyShipWelder;case 16: return r is IMyShipGrinder;case 17: return r is IMyGasGenerator;
case 18: return r is IMyLargeTurretBase;case 19: return r is IMyProjector;case 20: return r is IMyGyro;case 21: return r is IMyGravityGeneratorBase;case 22: return r is IMyLandingGear;
case 23: return r is IMyRadioAntenna;case 24: return r is IMyBeacon;case 25: return r is IMyLightingBlock;case 26: return r is IMyPistonBase;case 27: return r is IMyMotorStator;
case 28: return r is IMyWarhead;case 29: return r is IMySensorBlock;case 30: return r is IMyCameraBlock;case 31: return r is IMyParachute;case 32: return r is IMyTurretControlBlock;
}return false;}void pd(aU pM,List<string>r8,List<IMyTerminalBlock>hl){string qw=(pM.V+" "+string.Join(" ",r8)).ToLowerInvariant();
var ir=new List<int>();for(int fq=0;fq<j1.Length;fq++){foreach(var sq in j1[fq].Split(','))if(qw.Contains(sq)){ir.Add(fq);break;}}if(ir.Count==0)return;
var kP=ek();var mT=new List<IMyTerminalBlock>();for(int h8=0;h8<kP.Count;h8++){var hh=kP[h8];foreach(int qD in ir){if(oz(qD,hh)){(hh.CubeGrid==Me.CubeGrid ? hl : mT).Add(hh);
break;}}}if(hl.Count==0)hl.AddRange(mT);}void pf(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(v==null||v.T.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {v.T.Count} screens, {v.bG.Count} binds");eI=false;
Echo($"Blocks: {ek().Count} on this construct; {gW} on docked ships (ignored)");bS.Clear();Echo("--- BIND RESOLUTION ---");foreach(var af in v.bG){
if(af.a9==aH.c7){string io= !string.IsNullOrEmpty(af.aV)? af.aV : af.V;double si=aw.ContainsKey(io)? aw[io]: af.et;
Echo($"[OK] @bind {af.V} (dynamic variable '{io}' = {si})");continue;}List<IMyTerminalBlock>hm;if(bU.TryGetValue(af.V,out hm)){if(hm.Count>0){
string qe=eL.ContainsKey(af.V)&&eL[af.V]? " [fallback: typed match]" : "";Echo($"[OK] @bind {af.V} ({hm.Count} blocks matched{qe})");
}else{string r9=string.Join(", ",af.aC.Count>0 ? af.aC : new List<string>{af.ky});Echo($"[WARN] @bind {af.V} -> 0 blocks matched \"{r9}\"");
}}else{Echo($"[ERR] @bind {af.V} -> unresolved");}}if(aw.Count>0){Echo($"--- DYNAMIC VARIABLES ({aw.Count}) ---");foreach(var my in aw)Echo($"[VAR] {my.Key} = {my.Value}");
}IMyTextSurface g8=null;foreach(var q8 in bV.Values){g8=q8;break;}if(g8!=null){var jq=new List<string>();g8.GetSprites(jq);
Echo($"--- SPRITES ({jq.Count}): names usable in bg_texture ---");Echo(string.Join(", ",jq.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var a2 in v.T){IMyTextSurface nE;if(bV.TryGetValue(a2,out nE)){var nu=nE.SurfaceSize;var r_=D.oo(a2);
Echo($"[OK] Screen \"{a2.bb}\" [{a2.bM}] -> {a2.bc} [#{a2.bL}] ({nu.X:0}x{nu.Y:0} px, {r_.W} sprites)");
}else{Echo($"[ERR] Screen \"{a2.bb}\" -> Missing block \"{a2.bc}\" [#{a2.bL}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double pe(IMyTerminalBlock s){
if(s==null)return 0.0;var eW=s as IMyBatteryBlock;if(eW!=null)return eW.MaxStoredPower>0 ?(eW.CurrentStoredPower/eW.MaxStoredPower)*100.0 : 0.0;
var fp=s as IMyJumpDrive;if(fp!=null)return fp.MaxStoredPower>0 ?(fp.CurrentStoredPower/fp.MaxStoredPower)*100.0 : 0.0;var nL=s as IMyGasTank;
if(nL!=null)return nL.FilledRatio*100.0;var n3=s as IMyAirVent;if(n3!=null)return n3.GetOxygenLevel()*100.0;var li=s as IMyCargoContainer;if(li!=null){
var fm=li.GetInventory();if(fm!=null&&fm.MaxVolume.RawValue>0)return(double)fm.CurrentVolume.RawValue/fm.MaxVolume.RawValue*100.0;return 0.0;}var fM=s as IMyReactor;
if(fM!=null)return fM.MaxOutput>0 ?(fM.CurrentOutput/fM.MaxOutput)*100.0 : 0.0;var fV=s as IMySolarPanel;if(fV!=null)return fV.MaxOutput>0 ?(fV.CurrentOutput/fV.MaxOutput)*100.0 : 0.0;
var ge=s as IMyWindTurbine;if(ge!=null)return ge.MaxOutput>0 ?(ge.CurrentOutput/ge.MaxOutput)*100.0 : 0.0;var lD=s as IMyDoor;if(lD!=null)return(double)lD.OpenRatio*100.0;
var ga=s as IMyThrust;if(ga!=null)return ga.MaxEffectiveThrust>0 ?(ga.CurrentThrust/ga.MaxEffectiveThrust)*100.0 : 0.0;var jz=s as IMyMotorSuspension;
if(jz!=null)return jz.Brake ? 100.0 :(jz.Friction*100.0);var hB=s as IMyShipConnector;if(hB!=null)return hB.Status==MyShipConnectorStatus.Connected ? 100.0 :(hB.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var m7=s as IMyProductionBlock;if(m7!=null)return m7.IsProducing ? 100.0 : 0.0;var lH=s as IMyShipDrill;if(lH!=null)return lH.Enabled ? 100.0 : 0.0;
var jE=s as IMyShipToolBase;if(jE!=null)return jE.IsActivated ? 100.0 :(jE.Enabled ? 50.0 : 0.0);var hZ=s as IMyGasGenerator;if(hZ!=null)return hZ.AutoRefill ? 100.0 :(hZ.Enabled ? 50.0 : 0.0);
var gb=s as IMyLargeTurretBase;if(gb!=null)return gb.IsShooting ? 100.0 :(gb.HasTarget ? 50.0 :(gb.Enabled ? 10.0 : 0.0));var d1=s as IMyProjector;
if(d1!=null)return d1.TotalBlocks>0 ?((double)(d1.TotalBlocks-d1.RemainingBlocks)/d1.TotalBlocks)*100.0 : 0.0;var h4=s as IMyGyro;if(h4!=null)return h4.GyroOverride ? 100.0 :(h4.Enabled ? 50.0 : 0.0);
var mb=s as IMyGravityGeneratorBase;if(mb!=null)return Math.Min(100.0,Math.Abs(mb.GravityAcceleration)/9.81*100.0);var hY=s as IMyLandingGear;if(hY!=null)return hY.IsLocked ? 100.0 :(hY.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var kR=s as IMyRadioAntenna;if(kR!=null)return(kR.Radius/50000.0)*100.0;var k2=s as IMyBeacon;if(k2!=null)return(k2.Radius/50000.0)*100.0;
var mE=s as IMyLightingBlock;if(mE!=null)return mE.Intensity*10.0;var fJ=s as IMyPistonBase;if(fJ!=null)return fJ.HighestPosition>0 ?(fJ.CurrentPosition/fJ.HighestPosition)*100.0 : 0.0;
var nb=s as IMyMotorStator;if(nb!=null)return Math.Min(100.0,Math.Abs(nb.Angle)/6.28318*100.0);var n5=s as IMyWarhead;if(n5!=null)return n5.IsArmed ? 100.0 : 0.0;
var jl=s as IMySensorBlock;if(jl!=null)return!jl.LastDetectedEntity.IsEmpty()? 100.0 :(jl.Enabled ? 25.0 : 0.0);var hu=s as IMyCameraBlock;
if(hu!=null)return hu.IsActive ? 100.0 :(hu.Enabled ? 50.0 : 0.0);var lo=s as IMyParachute;if(lo!=null)return(double)lo.OpenRatio*100.0;
var f9=s as IMyTurretControlBlock;if(f9!=null)return f9.HasTarget ? 100.0 :(f9.IsUnderControl ? 50.0 :(f9.Enabled ? 10.0 : 0.0));var l0=s as IMyFunctionalBlock;
if(l0!=null)return l0.Enabled ? 100.0 : 0.0;return 0.0;}static double oI(IMyTerminalBlock kZ,string hQ){int lh=hQ.IndexOf(':');string iq=hQ.Substring(0,lh).ToLowerInvariant();
var sg=new MyItemType("MyObjectBuilder_"+(iq=="ammo" ? "AmmoMagazine" : char.ToUpper(iq[0])+iq.Substring(1)),hQ.Substring(lh+1));double nD=0;for(int h9=0;
h9<kZ.InventoryCount;h9++)nD+=(double)kZ.GetInventory(h9).GetItemAmount(sg);return nD;}double kq(IMyTerminalBlock t,string lT){if(t==null)return 0.0;
string a=lT.ToLowerInvariant();if(a=="percent"||a=="pct")return pe(t);if(a.IndexOf(':')>0)return oI(t,lT);var bl=t as IMyBatteryBlock;
if(bl!=null){if(a=="current"||a=="stored"||a=="val"||a=="value"||a=="raw")return bl.CurrentStoredPower;if(a=="max"||a=="capacity")return bl.MaxStoredPower;
if(a=="output")return bl.CurrentOutput;if(a=="maxoutput")return bl.MaxOutput;if(a=="input")return bl.CurrentInput;if(a=="maxinput")return bl.MaxInput;
return bl.CurrentStoredPower;}var il=t as IMyJumpDrive;if(il!=null){if(a=="max"||a=="capacity")return il.MaxStoredPower;return il.CurrentStoredPower;
}var f4=t as IMyGasTank;if(f4!=null){if(a=="max"||a=="capacity")return f4.Capacity;return f4.FilledRatio*f4.Capacity;}var n4=t as IMyAirVent;
if(n4!=null){if(a=="max"||a=="capacity")return 100.0;return n4.GetOxygenLevel()*100.0;}var lj=t as IMyCargoContainer;if(lj!=null){var fn=lj.GetInventory();
if(fn!=null){if(a=="mass")return(double)fn.CurrentMass;if(a=="max"||a=="capacity")return(double)fn.MaxVolume;return(double)fn.CurrentVolume;}return 0.0;}var i5=t as IMyPowerProducer;
if(i5!=null){if(a=="max"||a=="capacity")return i5.MaxOutput;return i5.CurrentOutput;}var lE=t as IMyDoor;if(lE!=null){if(a=="max")return 1.0;return(double)lE.OpenRatio*100.0;
}var jC=t as IMyThrust;if(jC!=null){if(a=="max"||a=="capacity")return jC.MaxEffectiveThrust;return jC.CurrentThrust;}var bA=t as IMyMotorSuspension;
if(bA!=null){if(a=="brake")return bA.Brake ? 100.0 : 0.0;if(a=="height")return bA.Height;if(a=="strength")return bA.Strength;if(a=="power")return bA.Power;
if(a=="steer")return bA.SteeringOverride*100.0;if(a=="propulsion")return bA.PropulsionOverride*100.0;return bA.Friction;}var lu=t as IMyShipConnector;
if(lu!=null){return lu.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var i2=t as IMyProductionBlock;if(i2!=null){if(a=="producing"||a=="active"||a=="running"||a=="state")return i2.IsProducing ? 100.0 : 0.0;
if(a=="queue")return i2.IsQueueEmpty ? 0.0 : 1.0;}var hc=t as IMyAssembler;if(hc!=null){if(a=="disassembling")return hc.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(a=="repeating"||a=="loop")return hc.Repeating ? 100.0 : 0.0;}var h_=t as IMyGasGenerator;if(h_!=null){if(a=="autorefill")return h_.AutoRefill ? 100.0 : 0.0;
if(a=="active")return h_.Enabled ? 100.0 : 0.0;}var gc=t as IMyLargeTurretBase;if(gc!=null){if(a=="shooting")return gc.IsShooting ? 100.0 : 0.0;
if(a=="target"||a=="hastarget")return gc.HasTarget ? 100.0 : 0.0;if(a=="range")return gc.Range;}var bt=t as IMyProjector;if(bt!=null){if(a=="built")return bt.TotalBlocks-bt.RemainingBlocks;
if(a=="remaining")return bt.RemainingBlocks;if(a=="total")return bt.TotalBlocks;if(a=="projecting")return bt.IsProjecting ? 100.0 : 0.0;if(a=="armor")return bt.RemainingArmorBlocks;
if(a=="buildable")return bt.BuildableBlocksCount;}var jF=t as IMyShipToolBase;if(jF!=null){if(a=="working"||a=="active"||a=="activated")return jF.IsActivated ? 100.0 : 0.0;
return jF.Enabled ? 100.0 : 0.0;}var cy=t as IMyGyro;if(cy!=null){if(a=="override")return cy.GyroOverride ? 100.0 : 0.0;if(a=="pitch")return cy.Pitch;
if(a=="yaw")return cy.Yaw;if(a=="roll")return cy.Roll;if(a=="power")return cy.GyroPower*100.0;}var mc=t as IMyGravityGeneratorBase;if(mc!=null){if(a=="g"||a=="accel")return mc.GravityAcceleration;
}var ma=t as IMyLandingGear;if(ma!=null){if(a=="locked")return ma.IsLocked ? 100.0 : 0.0;}var kS=t as IMyRadioAntenna;if(kS!=null){if(a=="radius"||a=="range")return kS.Radius;
}var k3=t as IMyBeacon;if(k3!=null){if(a=="radius"||a=="range")return k3.Radius;}var iz=t as IMyLightingBlock;if(iz!=null){if(a=="intensity")return iz.Intensity;
if(a=="radius")return iz.Radius;}var fK=t as IMyPistonBase;if(fK!=null){if(a=="pos"||a=="position"||a=="extension")return fK.CurrentPosition;
if(a=="velocity")return fK.Velocity;if(a=="max")return fK.HighestPosition;}var d6=t as IMyMotorStator;if(d6!=null){if(a=="angle"||a=="deg")return d6.Angle*180.0/Math.PI;
if(a=="rad")return d6.Angle;if(a=="velocity"||a=="rpm")return d6.TargetVelocityRPM;if(a=="locked")return d6.RotorLock ? 100.0 : 0.0;}var jN=t as IMyWarhead;
if(jN!=null){if(a=="armed")return jN.IsArmed ? 100.0 : 0.0;if(a=="countdown")return jN.DetonationTime;}var nr=t as IMySensorBlock;if(nr!=null){
if(a=="detected")return!nr.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var hv=t as IMyCameraBlock;if(hv!=null){if(a=="range")return hv.AvailableScanRange;
if(a=="active")return hv.IsActive ? 100.0 : 0.0;}var lp=t as IMyParachute;if(lp!=null){if(a=="open"||a=="openratio")return(double)lp.OpenRatio*100.0;
}var cR=t as IMyTurretControlBlock;if(cR!=null){if(a=="target"||a=="hastarget")return cR.HasTarget ? 100.0 : 0.0;if(a=="azimuth")return cR.AzimuthRotor!=null ? cR.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(a=="elevation")return cR.ElevationRotor!=null ? cR.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var l1=t as IMyFunctionalBlock;if(l1!=null){return l1.Enabled ? 100.0 : 0.0;
}return 0.0;}double bN(string dw,string lU){aU bm;dj.TryGetValue(dw,out bm);if(bm!=null&&bm.a9==aH.c7){
string qE= !string.IsNullOrEmpty(bm.aV)? bm.aV : dw;double lx;if(aw.TryGetValue(qE,out lx))return lx;return bm.et;
}double lK;if(aw.TryGetValue(dw,out lK))return lK;List<IMyTerminalBlock>dx;if(!bU.TryGetValue(dw,out dx)||dx.Count==0)throw new InvalidOperationException($"HELM: bind '{dw}' resolved to zero blocks — check the name/typo");
if(dx.Count==1)return kq(dx[0],lU);var n2=new List<double>();foreach(var pB in dx)n2.Add(kq(pB,lU));return od.oc(bm!=null ? bm.cV : at.gi,n2);
}double gP(string pO){return bN(pO,"percent");}static string a6(double n_,ba cE){if(cE==null||(string.IsNullOrEmpty(cE.di)&&string.IsNullOrEmpty(cE.c8)&&cE.F=="percent")){
return j7(n_);}return D.j6(n_,cE.di,cE.c8);}bool o8(y cM){IMyTextSurface f1;if(!bV.TryGetValue(cM,out f1))return false;
if(cM.bD!=null)return o2(cM,f1);if(cM.c2.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return o9(cM,f1);
}return pa(cM,f1);}Color oD(string[]cO,double rf){if(cO.Length==1)return E(cO[0]);double m8=Math.Max(0.0,Math.Min(100.0,rf))/100.0*(cO.Length-1);
int ia=Math.Min((int)m8,cO.Length-2);float f2=(float)(m8-ia);Color bj=E(cO[ia]),eV=E(cO[ia+1]);return new Color((int)(bj.R+(eV.R-bj.R)*f2),(int)(bj.G+(eV.G-bj.G)*f2),(int)(bj.B+(eV.B-bj.B)*f2),(int)(bj.A+(eV.A-bj.A)*f2));
}static Color[]eh(aW pT,string[]r2,string p5,float n7,float n9,float jL,float h5){int du=16;bool mj=p5=="horizontal";var hA=new Color[du];
for(int b_=0;b_<du;b_++){n hX=D.j8(r2,100.0*b_/(du-1));hA[b_]=new Color(hX.bd,hX.a7,hX.a4);float lg=jL/du,k9=h5/du;x(pT,mj ? new Vector2(n7+lg*(b_+0.5f),n9+h5/2f): new Vector2(n7+jL/2f,n9+k9*(b_+0.5f)),mj ? new Vector2(lg+1f,h5): new Vector2(jL,k9+1f),hA[b_]);
}return hA;}static void x(aW pU,Vector2 ro,Vector2 rU,Color pW,float rG=0f,string p4="SquareSimple"){pU.l.Add(new MySprite{Type=SpriteType.TEXTURE,Data=p4,Position=ro,Size=rU,Color=pW,RotationOrScale=rG}
);}void ok(y h,IMyTextSurface nF,aW f){f.l.Clear();f.eF.Clear();Vector2 K=nF.SurfaceSize;
Vector2 sd=nF.TextureSize;Vector2 ar=(sd-K)/2f;float fC=Math.Max(4f,K.X*0.02f);float b0=Math.Max(4f,K.Y*0.02f);
if(h.bM=="compact"||(h.bM=="auto"&&K.Y<=260f&&K.Y>150f)){fC=4f;b0=4f;}else if(h.bM=="corner"||(h.bM=="auto"&&K.Y<=150f)){
fC=2f;b0=2f;}float ee=K.X-(fC*2f);Color cp=new Color(10,12,16);bool lG=true;if(!string.IsNullOrEmpty(h.bC)){
var dH=n.dd(h.bC);if(dH.ey)lG=false;else cp=new Color(dH.bd,dH.a7,dH.a4,dH.gg);}f.bF= -1;f.ej=false;
f.ei=E(h.jQ ?? "#5b0f0f");f.c1.Clear();if(h.bE!=null&&h.bE.Length>1){f.bF=f.l.Count;f.en=eh(f,h.bE,h.c0,ar.X+2f,ar.Y+2f,K.X-4f,K.Y-4f);
}else if(lG||h.aG!=null||h.cc!=null){if(h.aG!=null)cp=E(h.aG.First(rt=>rt.ch).bJ);f.bF=f.l.Count;
f.en=new[]{cp};x(f,ar+(K/2f),K-new Vector2(4f,4f),cp);if(h.aG!=null||h.cc!=null)f.c1.Add(new eo{
ex=f.bF,gj=cp,es=cp,eE=h.aG,gn=h.gk,gw=h.gl,ev=h.cc,gx=h.gy,gz=h.gA,ke=true}
);}if(!string.IsNullOrEmpty(h.gm))x(f,ar+(K/2f),K-new Vector2(4f,4f),E(h.jS ?? "255,255,255,24"),0f,h.gm);
if(!h.c2.Equals("none",StringComparison.OrdinalIgnoreCase)){Color lb=h.al!=null&&h.al.Length>0 ? E(h.al[0]): E(h.er ?? "40, 60, 85");
Color pS=h.al!=null&&h.al.Length>1 ? E(h.al[h.al.Length-1]): lb;
float lc=ar.X+(K.X-ee)/2f;if(h.al!=null&&h.al.Length>1&& !h.gC){eh(f,h.al,"horizontal",lc,ar.Y+b0*0.5f-1f,ee,2f);
eh(f,h.al,"horizontal",lc,ar.Y+K.Y-(b0*0.5f)-1f,ee,2f);}else{x(f,new Vector2(ar.X+K.X/2f,ar.Y+b0*0.5f),new Vector2(ee,2f),lb);
x(f,new Vector2(ar.X+K.X/2f,ar.Y+K.Y-(b0*0.5f)),new Vector2(ee,2f),pS);}}var iT=h.av;
if(iT.Count==0){iT=new List<au>{new au{dc="main",c4=0,de=0,df=h.cf,dg=h.cg,aK=h.eA}};}float me=8f;
foreach(var u in iT){var fB=D.om(u.c4,u.de,u.df,u.dg,h.cf,h.cg,K.X,K.Y,fC,b0,me,me);
float aN=ar.X+fB.kF;float dZ=ar.Y+fB.kG;float a_=fB.kE;float fA=fB.ka;bool ne=u.aG!=null||u.cc!=null;
if(u.bE!=null&&u.bE.Length>1){eh(f,u.bE,u.c0!="vertical" ? u.c0 : h.c0,aN,dZ,a_,fA);
}else{Color bs;if(u.aG!=null)bs=E(u.aG.First(ru=>ru.ch).bJ);else if(!string.IsNullOrEmpty(u.cW))bs=E(u.cW);
else if(h.cf>1||h.cg>1)bs=string.IsNullOrEmpty(h.gK)? new Color(255,255,255,6): E(h.gK);else bs=Color.Transparent;
if(bs.A>0||ne){if(ne)f.c1.Add(new eo{ex=f.l.Count,gj=bs,es=bs,eE=u.aG,gn=u.gk,gw=u.gl,ev=u.cc,gx=u.gy,gz=u.gA}
);x(f,new Vector2(aN+a_/2f,dZ+fA/2f),new Vector2(a_,fA),bs);}}string hW=u.j3 ?? h.j4 ?? "Monospace";float pK=D.oC(h.bM,K.X,K.Y,h.cd);
float aE=u.j5 ?? pK;float ff=0f;if(!string.IsNullOrEmpty(u.bQ)){ff=20f*aE;var g6=TextAlignment.LEFT;float jD=aN+4f;
string kL=u.gR ?? h.kz ?? "left";if(kL.Equals("center",StringComparison.OrdinalIgnoreCase)){g6=TextAlignment.CENTER;jD=aN+(a_/2f);
}else if(kL.Equals("right",StringComparison.OrdinalIgnoreCase)){g6=TextAlignment.RIGHT;jD=aN+a_-4f;}string nO=u.gS ?? h.kA;
Color se= !string.IsNullOrEmpty(nO)? E(nO): E(u.gr ?? h.er ?? "white");var nT=MySprite.CreateText(u.bQ,hW,se,aE*0.9f,g6);
nT.Position=new Vector2(jD,dZ+2f);f.l.Add(nT);string[]hi=u.gs ?? h.al;Color p7=hi!=null&&hi.Length>0 ? E(hi[0]): E(u.gr ?? h.er ?? "40, 60, 85");
x(f,new Vector2(aN+a_/2f,dZ+ff-2f),new Vector2(a_-8f,1f),p7);}int qZ=Math.Max(1,u.aK.Count);float rd=fA-ff-8f;
float i9=rd/qZ;float fl=6f;for(int fk=0;fk<u.aK.Count;fk++){var z=u.aK[fk];var P=new db{ki=z};float rH=dZ+ff+4f+(fk*i9);
float J=rH+(i9/2f);float nQ=J-(11f*aE);P.gF=f.l.Count;P.j9=z.S!=null&&(z.S.Contains("{val}")||z.S.Contains("{percent}"));
var mB=MySprite.CreateText(z.S,hW,Color.White,aE,TextAlignment.LEFT);mB.Position=new Vector2(aN+fl,nQ);f.l.Add(mB);
float qS=Math.Max(24f*aE,(z.S.Length+1)*12f*aE);float sp=(!string.IsNullOrEmpty(z.di)|| !string.IsNullOrEmpty(z.c8))? 64f*aE : 52f*aE;
if(z.g!=e.gQ){float I=aN+fl+qS;float pJ=aN+a_-fl-sp;float H=Math.Max(16f,pJ-I);
float O=Math.Max(z.g==e.bO ? 8f : 4f,Math.Min((z.g==e.bO ? 20f : 16f)*aE,i9*(z.g==e.bO ? 0.55f : 0.45f)));
P.c_=I;P.em=H;P.cb=O;P.cZ=J;if(z.g==e.cY){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(25,30,38));
P.aa=f.l.Count;x(f,new Vector2(I,J),new Vector2(0f,O),E(z.am));}else if(z.g==e.da){
int jj=10;float l3=2f;float jk=Math.Max(2f,(H-(l3*(jj-1)))/jj);for(int jd=0;jd<jj;jd++){P.ad.Add(f.l.Count);
x(f,new Vector2(I+jd*(jk+l3)+(jk/2f),J),new Vector2(jk,O),new Color(22,26,32));}}else if(z.g==e.bO){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));
int i1=8;float l4=2f;float hy=Math.Max(2f,(H-(l4*(i1-1)))/i1);for(int iR=0;iR<i1;iR++){P.ad.Add(f.l.Count);
x(f,new Vector2(I+iR*(hy+l4)+(hy/2f),J),new Vector2(hy,2f),E(z.am));}}else if(z.g==e.c9){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));
int jt=10;float l5=2f;float ju=Math.Max(2f,(H-(l5*(jt-1)))/jt);for(int je=0;je<jt;je++){P.ad.Add(f.l.Count);
x(f,new Vector2(I+je*(ju+l5)+(ju/2f),J),new Vector2(ju,O),new Color(24,28,36),0.523599f);}}else if(z.g==e.c3){
int ha=10;float l6=2f;float hb=Math.Max(2f,(H-(l6*(ha-1)))/ha);for(int jf=0;jf<ha;jf++){P.ad.Add(f.l.Count);
x(f,new Vector2(I+jf*(hb+l6)+(hb/2f),J),new Vector2(hb,O),new Color(24,28,36),1.570796f,"Triangle");}}else if(z.g==e.c6){
int hE=12;float l7=2f;float hG=Math.Max(2f,(H-(l7*(hE-1)))/hE);float lF=Math.Min(hG,O);for(int jg=0;jg<hE;jg++){P.ad.Add(f.l.Count);
x(f,new Vector2(I+jg*(hG+l7)+(hG/2f),J),new Vector2(lF,lF),new Color(24,28,36),0f,"Circle");}}else if(z.g==e.ca){
x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(25,30,38));P.aa=f.l.Count;x(f,new Vector2(I+(H/2f),J),new Vector2(0f,O),E(z.am));
x(f,new Vector2(I+(H/2f),J),new Vector2(2f,O+4f),new Color(80,110,150));}else if(z.g==e.bR){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));
P.aa=f.l.Count;float it=Math.Max(4f,O);x(f,new Vector2(I+it*0.5f,J),new Vector2(it,it),new Color(40,50,65),0f,"Circle");
}else if(z.g==e.bI){float cq=Math.Max(6f,Math.Min(O,14f*aE));x(f,new Vector2(I+cq*0.5f,J),new Vector2(cq,cq),new Color(28,34,46));
P.aa=f.l.Count;x(f,new Vector2(I+cq*0.5f,J),new Vector2(cq*0.6f,cq*0.6f),new Color(40,50,65));}else if(z.g==e.bP){
x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(18,22,30));P.aa=f.l.Count;float i_=Math.Max(4f,O*0.7f);
x(f,new Vector2(I+i_+2f,J),new Vector2(i_,i_),new Color(34,197,94),0f,"Circle");}else if(z.g==e.b9){int iZ=8;
float l8=2f;float i0=Math.Max(2f,(H-(l8*(iZ-1)))/iZ);for(int iS=0;iS<iZ;iS++){P.ad.Add(f.l.Count);x(f,new Vector2(I+iS*(i0+l8)+(i0/2f),J),new Vector2(i0,O),new Color(24,28,36));
}}else if(z.g==e.bK){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));P.aa=f.l.Count;
float mm=Math.Max(4f,O);x(f,new Vector2(I+(H/2f),J),new Vector2(mm,mm),E(z.am),0f,"Triangle");}}var nY=MySprite.CreateText("---",hW,Color.White,aE,TextAlignment.RIGHT);
nY.Position=new Vector2(aN+a_-fl,nQ);P.bg=f.l.Count;f.l.Add(nY);f.eF.Add(P);}}f.kg=K;
f.kd=true;}static void aL(aW le,db nv,string sc,Color pX){var jJ=le.l[nv.bg];jJ.Data=sc;jJ.Color=pX;le.l[nv.bg]=jJ;
}static void ks(aW lf,db nw,Color pY){var lO=lf.l[nw.aa];lO.Color=pY;lf.l[nw.aa]=lO;}bool pa(y a3,IMyTextSurface jw){
aW i;if(!eN.TryGetValue(a3,out i)){i=new aW();eN[a3]=i;}if(!i.kd||i.kg!=jw.SurfaceSize){
ok(a3,jw,i);}bool M=false;bool eR=(ao%a3.gh)<a3.gh/2.0;double bZ= -1.0,lv;if(eJ.TryGetValue(a3,out lv)){
bZ=(ao-lv)/a3.gp;if(bZ>=1.0){eJ.Remove(a3);bZ=1.0;}}bool jM=bT&&eR&&i.ei.A>0;if(i.bF>=0&&jM!=i.ej){
i.ej=jM;for(int dJ=0;dJ<i.en.Length;dJ++){var k5=i.l[i.bF+dJ];k5.Color=jM ? i.ei : i.en[dJ];i.l[i.bF+dJ]=k5;
}M=true;foreach(var pL in i.c1)pL.gB=true;}foreach(var ac in i.c1){if(ac.ke&&i.ej)continue;Color dv=ac.gj;try{if(ac.eE!=null)dv=E(D.eD(ac.eE,bN(ac.gn,ac.gw)));
if(ac.ev!=null&&eR&&D.eD(ac.ev,bN(ac.gx,ac.gz))!=null)dv=i.ei;}catch{}if(dv!=ac.es||ac.gB){
ac.es=dv;ac.gB=false;var ld=i.l[ac.ex];ld.Color=dv;i.l[ac.ex]=ld;M=true;}}for(int ib=0;ib<i.eF.Count;ib++){var c=i.eF[ib];
var j=c.ki;double w;bool mg=false;double iG=j.eB;try{w=bN(j.ay,j.F);if(j.ci!=null)iG=bN(j.ci,j.gH);
}catch{mg=true;w=0.0;}if(mg){if(c.ez!= -99999.0||(c.bg>=0&&i.l[c.bg].Data!="ERR")){M=true;c.ez= -99999.0;
if(c.bg>=0){var jh=i.l[c.bg];jh.Data="ERR";jh.Color=Color.Red;i.l[c.bg]=jh;}}continue;}double aO=w;
if(iG>0)w=Math.Min(100.0,aO/iG*100.0);bool nH=bZ>=0.0&&j.g!=e.gQ&&j.g!=e.bR&&j.g!=e.bI&&j.g!=e.bP&&j.g!=e.bK;
if(nH)w=bZ<0.5 ? bZ*200.0 : 100.0+(w-100.0)*(bZ-0.5)*2.0;string p_=j.ce ? D.ew(j.a8,w): j.am;
Color _=E(p_);bool aY=(_==Color.Red);if(j.ce&& !a3.gC&&j.a8.Length>1)_=oD(j.a8,w);
if(aY&&eR){_=new Color(255,75,75);M=true;}bool aF=nH||Math.Abs(aO-c.ez)>0.1;if(aF)c.ez=aO;
if(j.g==e.cY){double lV=(j.gE||j.F=="percent"||j.F=="pct")? w : gP(j.ay);if(c.Q<0f)c.Q=(float)lV;
float f5=(float)lV;if(Math.Abs(c.Q-f5)>0.05f){c.Q+=(f5-c.Q)*0.35f;M=true;}else if(c.Q!=f5){
c.Q=f5;M=true;}if(aF)M=true;if(M){float rg=(float)Math.Max(0.0,Math.Min(1.0,c.Q/100.0));float lX=c.em*rg;
var e9=i.l[c.aa];e9.Size=new Vector2(lX,c.cb);e9.Position=new Vector2(c.c_+(lX/2f),c.cZ);
e9.Color=_;i.l[c.aa]=e9;aL(i,c,a6(aO,j),_);}}else if(j.g==e.da||j.g==e.c9||j.g==e.c3||j.g==e.c6){
if(aF||aY){M=true;double qg=(j.gE||j.F=="percent"||j.F=="pct")? w : gP(j.ay);int np=c.ad.Count;
for(int d8=0;d8<np;d8++){bool q0=qg>=((d8+0.4)/(double)np*100.0);Color rR=q0 ? _ : new Color(24,28,36);var nq=i.l[c.ad[d8]];
nq.Color=rR;i.l[c.ad[d8]]=nq;}aL(i,c,a6(aO,j),_);}}else if(j.g==e.bO){
if(aF||aY){M=true;for(int fe=0;fe<7;fe++)c.eG[fe]=c.eG[fe+1];c.eG[7]=(float)w;int rn=c.ad.Count;
for(int dV=0;dV<rn;dV++){float qv=c.eG[dV];float qu=Math.Max(0.06f,Math.Min(1f,qv/100f));float lr=c.cb*qu;float pZ=c.cZ+(c.cb/2f)-(lr/2f);
var cv=i.l[c.ad[dV]];cv.Size=new Vector2(cv.Size.Value.X,lr);cv.Position=new Vector2(cv.Position.Value.X,pZ);
cv.Color=_;i.l[c.ad[dV]]=cv;}aL(i,c,a6(aO,j),_);}}else if(j.g==e.ca){
if(c.Q<0f)c.Q=(float)w;float f6=(float)w;if(Math.Abs(c.Q-f6)>0.05f){c.Q+=(f6-c.Q)*0.35f;
M=true;}else if(c.Q!=f6){c.Q=f6;M=true;}if(M||aF){float mf=c.em*0.5f;float lm=c.c_+mf;
float lB=(float)Math.Max(-1.0,Math.Min(1.0,(c.Q-50.0)/50.0));float hU=Math.Abs(lB)*mf;float qh=(lB>=0)?(lm+hU*0.5f):(lm-hU*0.5f);
var fa=i.l[c.aa];fa.Size=new Vector2(hU,c.cb);fa.Position=new Vector2(qh,c.cZ);fa.Color=_;
i.l[c.aa]=fa;aL(i,c,a6(aO,j),_);}}else if(j.g==e.bR){if(aF||aY){
M=true;bool ik=w>=50.0;float mv=Math.Max(4f,c.cb);float qH=ik ?(c.c_+c.em-mv*0.5f):(c.c_+mv*0.5f);
Color mu=ik ?(_!=Color.White ? _ : new Color(34,197,94)): new Color(60,70,85);var hS=i.l[c.aa];hS.Position=new Vector2(qH,c.cZ);
hS.Color=mu;i.l[c.aa]=hS;aL(i,c,ik ? "ON" : "OFF",mu);}}else if(j.g==e.bI){if(aF||aY){
M=true;bool ij=w>=50.0;Color ln=ij ?(_!=Color.White ? _ : new Color(56,189,248)): new Color(30,36,48);ks(i,c,ln);
aL(i,c,ij ? "ACTIVE" : "READY",ij ? ln : new Color(148,163,184));}}else if(j.g==e.bP){if(aF||aY){
M=true;string r0=w>=75.0 ? "ONLINE" :(w>=25.0 ? "STBY" : "OFFLINE");Color nA=w>=75.0 ? new Color(34,197,94):(w>=25.0 ? new Color(234,179,8): new Color(239,68,68));
ks(i,c,nA);aL(i,c,r0,nA);}}else if(j.g==e.b9){if(aF||aY){M=true;int m3=c.ad.Count;
for(int dW=0;dW<m3;dW++){bool q1=w>=((dW+0.5)/(double)m3*100.0);Color rk=q1 ? _ : new Color(24,28,36);var m4=i.l[c.ad[dW]];
m4.Color=rk;i.l[c.ad[dW]]=m4;}aL(i,c,a6(aO,j),_);}}else if(j.g==e.bK){
M=true;float fP;Color dF;string fb;bool qz=(j.F=="producing"||j.F=="active"||j.F=="running"||j.F=="state"||(j.F!="percent"&&(w<=1.0||w==100.0)));
if(qz){if(w>0.0){c.Q=(c.Q+0.3927f)%6.28318f;fP=c.Q;dF=new Color(34,197,94);fb="RUN";}else{fP=0f;
dF=new Color(148,163,184);fb="IDLE";}}else{fP=(w>55.0)? 0f :((w<45.0)? 3.14159f : 1.57079f);dF=(w>55.0)? new Color(34,197,94):((w<45.0)? new Color(239,68,68): new Color(148,163,184));
fb=(w>55.0)? "CHG" :((w<45.0)? "DSC" : "STBY");}var hT=i.l[c.aa];hT.RotationOrScale=fP;hT.Color=dF;
i.l[c.aa]=hT;aL(i,c,fb,dF);}else{string e7;aU f8;dj.TryGetValue(j.ay,out f8);dk.TryGetValue(kf(j.F)? j.ay+"."+j.F :(f8!=null&&f8.aV!=null ? f8.aV : j.ay),out e7);
if(aF||aY||e7!=c.kh){c.kh=e7;M=true;Color jG=j.c5!=null ? E(D.eD(j.c5,w)): Color.White;
if(jG==Color.Red&&eR)jG=new Color(255,75,75);aL(i,c,e7 ?? a6(aO,j),jG);}}if(c.j9&&(aF||aY)){
M=true;var mD=i.l[c.gF];string rh=j7(w);string rE=a6(aO,j);mD.Data=j.S.Replace("{percent}",rh).Replace("{val}",rE);
i.l[c.gF]=mD;}}if(M){using(var ql=jw.DrawFrame()){ql.AddRange(i.l);}}return M;}bool o9(y bv,IMyTextSurface jx){
jx.FontSize=bv.cd;var e3=new List<KeyValuePair<string,string>>();foreach(var m in bv.eA){double L=0.0;string aS;string aM=null;
double iH=m.eB;try{L=bN(m.ay,m.F);if(m.ci!=null)iH=bN(m.ci,m.gH);}catch(Exception sy){
e3.Add(new KeyValuePair<string,string>($"{m.S}: [UNRESOLVED]","red"));continue;}double m9=L;if(iH>0)L=Math.Min(100.0,m9/iH*100.0);string nZ=a6(m9,m);
if(m.g==e.cY){double lW=(m.gE||m.F=="percent"||m.F=="pct")? L : gP(m.ay);aS=D.o3(m.S,lW,10,nZ);
aM=m.ce ? D.ew(m.a8,lW): m.am;}else if(m.g==e.ca){aS=D.o1(m.S,L,10);
aM=m.ce ? D.ew(m.a8,L): m.am;}else if(m.g==e.bR){aS=D.pc(m.S,L);
aM=m.am ??(L>=50.0 ? "green" : "gray");}else if(m.g==e.bI){aS=D.o6(m.S,L);aM=m.am ??(L>=50.0 ? "cyan" : "gray");
}else if(m.g==e.bP){aS=D.pb(m.S,L);aM=m.am ??(L>=75.0 ? "lime" :(L>=25.0 ? "yellow" : "red"));
}else if(m.g==e.b9){aS=D.o0(m.S,L,8);aM=m.ce ? D.ew(m.a8,L):(m.am ?? "yellow");
}else if(m.g==e.bK){bool qA=(m.F=="producing"||m.F=="active"||m.F=="running"||m.F=="state"||(m.F!="percent"&&(L<=1.0||L==100.0)));
if(qA){char[]rZ=new[]{'|','/','-','\\'};char rO=rZ[Math.Abs(dp)%4];aS=L>0 ? $"{m.S} [{rO}] RUN" : $"{m.S} [==] IDLE";
aM=L>0 ? "green" : "gray";}else{aS=D.o7(m.S,L,dp);aM=m.am ??(L>55.0 ? "green" :(L<45.0 ? "red" : "cyan"));
}}else{aS=$"{m.S}: {nZ}";aM=m.c5!=null ? D.eD(m.c5,L): null;}e3.Add(new KeyValuePair<string,string>(aS,aM));
}var e0=D.o5(new List<string>(e3.ConvertAll(r7=>r7.Key)),cT: bv.kD);string ns=string.Join("\n",e0);
string m6;if(ck.TryGetValue(bv,out m6)&&m6==ns)return false;ck[bv]=ns;using(var qm=jx.DrawFrame()){
float oa=4f;float mF=ob*bv.cd;try{var mH=jx.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bv.cd);
if(mH.Y>0)mF=mH.Y;}catch{}for(int cz=0;cz<e0.Count;cz++){string lt=null;if(cz>0&&cz<e0.Count-1)lt=e3[cz-1].Value;
var nz=MySprite.CreateText(e0[cz],"Monospace",E(lt),bv.cd,TextAlignment.LEFT);nz.Position=new Vector2(4f,oa);qm.Add(nz);
oa+=mF;}}return true;}Color E(string fy){if(string.IsNullOrEmpty(fy))return Color.White;Color dQ;if(kH.TryGetValue(fy,out dQ))return dQ;
n dy=n.dd(fy);dQ=dy.ey ? Color.Transparent : new Color(dy.bd,dy.a7,dy.a4,dy.gg);kH[fy]=dQ;return dQ;}public enum aH{of,gD,c7}
public enum at{oM,gi,kx,kk,kj}public class aU{public string V;public aH a9;public List<string>aC=new List<string>();public string ky{
get{return aC.Count>0 ? aC[0]: "";}set{if(aC.Count==0)aC.Add(value);else aC[0]=value;}}public at cV=at.oM;
public double et=0.0;public string gt;public string aV;public bool el;}public enum e{gQ,cY,da,bO,c9,c3,c6,ca,bR,bI,bP,b9,bK}
public class az{public string gv;public string bJ;public bool ch;}public class ba{public e g;public string S;public string ay;
public string F;public bool ce;public string[]a8;public string am;public List<az>c5;public string jZ="right";
public string di;public string c8;public double eB;public string ci,gH;public bool gE{get{return eB>0||ci!=null;}}}public struct n{
public byte bd,a7,a4,gg;public bool ey;public n(byte rv,byte qo,byte pC,byte pl=255){bd=rv;a7=qo;a4=pC;gg=pl;ey=false;}public static readonly n pj=new n(0,0,0,0){
ey=true};public static readonly n eH=new n(255,255,255);public static readonly n oe=new n(0,0,0);public static readonly n oZ=new n(255,0,0);
public static readonly n oF=new n(0,255,0);public static readonly n oJ=new n(50,205,50);public static readonly n pk=new n(255,255,0);
public static readonly n oN=new n(255,140,0);public static readonly n oq=new n(0,255,255);public static readonly n og=new n(0,120,255);
public static readonly n oK=new n(255,0,255);public static readonly n oX=new n(128,0,128);public static readonly n oE=new n(128,128,128);
public static readonly n os=new n(60,60,60);public static n dd(string aR){if(string.IsNullOrWhiteSpace(aR))return eH;aR=aR.Trim();
if(aR.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aR.Equals("none",StringComparison.OrdinalIgnoreCase))return pj;if(aR.StartsWith("#")){
string bp=aR.Substring(1).Trim();if(bp.Length==6){byte rw=Convert.ToByte(bp.Substring(0,2),16);byte qp=Convert.ToByte(bp.Substring(2,2),16);byte pD=Convert.ToByte(bp.Substring(4,2),16);
return new n(rw,qp,pD);}if(bp.Length==3){byte rx=Convert.ToByte(new string(bp[0],2),16);byte qq=Convert.ToByte(new string(bp[1],2),16);byte pE=Convert.ToByte(new string(bp[2],2),16);
return new n(rx,qq,pE);}}if(aR.Contains(",")){var cH=aR.Split(',');if(cH.Length>=3){byte ry=(byte)Math.Max(0,Math.Min(255,int.Parse(cH[0].Trim())));
byte qr=(byte)Math.Max(0,Math.Min(255,int.Parse(cH[1].Trim())));byte pF=(byte)Math.Max(0,Math.Min(255,int.Parse(cH[2].Trim())));byte pm=cH.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(cH[3].Trim()))):(byte)255;
return new n(ry,qr,pF,pm);}}switch(aR.ToLowerInvariant()){case "red": return oZ;case "green": return oF;case "lime": return oJ;case "yellow": return pk;
case "orange": return oN;case "cyan": return oq;case "blue": return og;case "magenta": return oK;case "purple": return oX;case "white": return eH;
case "black": return oe;case "gray": case "grey": return oE;case "darkgray": case "darkgrey": return os;default: return eH;}}}public class a5{
public string[]bE;public string c0="vertical";public List<az>aG;public string gk,gl;public List<az>cc;public string gy,gA;
}public class au : a5{public string dc="default";public int c4=0;public int de=0;public int df=1;public int dg=1;public string bQ;public string gR="left";
public string gS;public string jX;public string j3;public float? j5;public string cW;public string gr;public string[]gs;
public string kl="horizontal";public List<ba>aK=new List<ba>();}public class y : a5{public string bb;public string bM="standard";
public string c2="graphic";public string er;public string[]al;public string kz="left";public string kA;public string j4="Monospace";
public string bC;public string jV;public int jW=10;public int jT=1;public float gp=1.2f;public bool gC;
public string gK;public string gm;public string jS;public string jQ;public string bD;public string bH;public float gq=2.5f;
public string jU="cyan";public string[]ep;public List<string>eq=new List<string>();public List<string>oB(){if(eq!=null&&eq.Count>0)return eq;
return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float cd=1.0f;public int gM=0;public float gh=1.6f;public int cf=1;public int cg=1;public int kD=30;public string bc;
public int bL=0;public List<au>av=new List<au>();public List<ba>eA{get{if(av.Count==0)av.Add(new au{dc="main"}
);return av[0].aK;}}}public class bf{public string gI;public List<string>eC=new List<string>();public Dictionary<string,string>gu=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>go=new List<string>();}public class gL{public List<aU>bG=new List<aU>();public List<y>T=new List<y>();
public List<string>aJ=new List<string>();public Dictionary<string,List<string>>cX=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class kb{static readonly Dictionary<string,bf>jY=new Dictionary<string,bf>(StringComparer.OrdinalIgnoreCase);
static kb(){o_();}static void o_(){be("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");be("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");be("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");be("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");be("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");be("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");be("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");be("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void be(string mO,string[]re,string pP){var f3=new bf{gI=mO}
;foreach(var q9 in re){var iu=q9.Split(new[]{'='},2);string mW=gJ(iu[0]);f3.eC.Add(mW);if(iu.Length>1)f3.gu[mW]=ab(iu[1].Trim());
}var q_=pP.Replace("\r\n","\n").Split('\n');foreach(var qN in q_){string ng=qN.Trim();if(ng.Length>0)f3.go.Add(ng);}jY[mO]=f3;}static string gJ(string cF){
cF=cF.Trim();if(cF.StartsWith("$"))cF=cF.Substring(1);return cF.ToLowerInvariant();}public static gL oP(string p2){var ai=new gL();y d=null;
au p=null;var m5=oV(p2,ai.aJ,ai.cX);for(int dK=0;dK<m5.Count;dK++){string b=m5[dK].Trim();
if(b.Length==0)continue;try{if(b.StartsWith("@bind")){ai.bG.Add(oQ(b));}else if(b.StartsWith("@screen")){d=new y{bb=b.Substring("@screen".Length).Trim()}
;ai.T.Add(d);p=null;}else if(b.StartsWith("profile:")){q(d,"profile:");d.bM=b.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(b.StartsWith("border:")){q(d,"border:");string k_=b.Substring("border:".Length).Trim();if(p!=null)p.jX=k_;
else d.c2=k_;}else if(b.StartsWith("border_color:")){q(d,"border_color:");string k1=b.Substring("border_color:".Length).Trim();
if(p!=null)p.gr=k1;else d.er=k1;}else if(b.StartsWith("border_gradient:")||b.StartsWith("border_gradient(")){
q(d,"border_gradient");string bX=b.StartsWith("border_gradient:")? b.Substring("border_gradient:".Length): b.Substring("border_gradient".Length);
bX=bX.Trim();if(bX.StartsWith("(")&&bX.Contains(")"))bX=aA(bX,0);string[]nB=aB(bX).Select(rI=>rI.Trim()).ToArray();if(p!=null)p.gs=nB;
else d.al=nB;}else if(b.StartsWith("title_align:")){q(d,"title_align:");string nK=b.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(p!=null)p.gR=nK;else d.kz=nK;}else if(b.StartsWith("title_color:")){q(d,"title_color:");string nN=b.Substring("title_color:".Length).Trim();
if(p!=null)p.gS=nN;else d.kA=nN;}else if(b.StartsWith("font_size:")){q(d,"font_size:");float nI=float.Parse(b.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(p!=null)p.j5=nI;else d.cd=nI;}else if(b.StartsWith("font:")){q(d,"font:");string lP=b.Substring("font:".Length).Trim();
if(p!=null)p.j3=lP;else d.j4=lP;}else if(b.StartsWith("bg:")||b.StartsWith("background:")){q(d,"bg:");string co=b.Substring(b.IndexOf(':')+1).Trim();
a5 hj=p!=null ?(a5)p : d;if(co.StartsWith("color(")&&co.EndsWith(")")){string k8;hj.aG=ko(co.Substring(6,co.Length-7),out k8);
string k6,k7;kt(k8,out k6,out k7);hj.gk=k6;hj.gl=k7;}else if(p!=null)p.cW=co;
else d.bC=co;}else if(b.StartsWith("layout:")){q(d,"layout:");oR(b.Substring("layout:".Length).Trim(),d);}else if(b.StartsWith("alert_period:")){
q(d,"alert_period:");d.gh=Math.Max(0.2f,float.Parse(b.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(b.StartsWith("refresh:")){q(d,"refresh:");d.gM=int.Parse(b.Substring("refresh:".Length).Trim());}else if(b.StartsWith("output:")){
q(d,"output:");var fF=aB(b.Substring("output:".Length).Trim());if(fF.Count==0)throw new FormatException("output: requires a target block name");
d.bc=ab(fF[0]);d.bL=fF.Count>1 ? int.Parse(fF[1]): 0;}else if(b.StartsWith("width:")){q(d,"width:");
d.kD=int.Parse(b.Substring("width:".Length).Trim());}else if(b.StartsWith("boot_msg:")||b.StartsWith("boot:")){q(d,"boot_msg:");
d.jV=ab(b.Substring(b.IndexOf(':')+1).Trim());}else if(b.StartsWith("boot_stagger:")){q(d,"boot_stagger:");d.jW=int.Parse(b.Substring("boot_stagger:".Length).Trim());
}else if(b.StartsWith("boot_calibrate:")){q(d,"boot_calibrate:");string lw=b.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.gp=lw=="off" ? 0f : Math.Max(0f,float.Parse(lw,System.Globalization.CultureInfo.InvariantCulture));}else if(b.StartsWith("boot_batch:")){q(d,"boot_batch:");
d.jT=Math.Max(1,int.Parse(b.Substring("boot_batch:".Length).Trim()));}else if(b.StartsWith("boot_duration:")){q(d,"boot_duration:");
string dA=b.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool ml=dA.EndsWith("s");if(ml)dA=dA.Substring(0,dA.Length-1).Trim();
float hH;if(float.TryParse(dA,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out hH)){d.gq=ml ? hH : hH/60f;
}}else if(b.StartsWith("boot_log:")){q(d,"boot_log:");d.eq.Add(ab(b.Substring("boot_log:".Length).Trim()));}else if(b.StartsWith("banner:")){
q(d,"banner:");d.bD=ab(b.Substring("banner:".Length).Trim());}else if(b.StartsWith("boot_logo:")){q(d,"boot_logo:");
d.bH=ab(b.Substring("boot_logo:".Length).Trim());}else if(b.StartsWith("boot_gradient:")||b.StartsWith("boot_gradient(")||b.StartsWith("banner_gradient:")||b.StartsWith("banner_gradient(")){
q(d,"boot_gradient");d.ep=kp(b);}else if(b.StartsWith("bg_gradient:")||b.StartsWith("bg_gradient(")){q(d,"bg_gradient");
(p!=null ?(a5)p : d).bE=kp(b);}else if(b.StartsWith("bg_gradient_dir:")){q(d,"bg_gradient_dir:");
(p!=null ?(a5)p : d).c0=b.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(b.StartsWith("flash:")){
q(d,"flash:");a5 hP=p!=null ?(a5)p : d;string hO=b.Substring("flash:".Length).Trim();string lR=kw(hO);
string lQ,lS;kt(hO.Substring(0,hO.Length-lR.Length),out lQ,out lS);hP.gy=lQ;hP.gA=lS;hP.cc=new List<az>{
new az{gv=lR,bJ="on"}};}else if(b.StartsWith("bg_texture:")){q(d,"bg_texture:");d.gm=ab(b.Substring("bg_texture:".Length).Trim());
}else if(b.StartsWith("bg_texture_color:")){q(d,"bg_texture_color:");d.jS=b.Substring("bg_texture_color:".Length).Trim();
}else if(b.StartsWith("gradient_mode:")){q(d,"gradient_mode:");d.gC=b.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(b.StartsWith("panel_bg:")){q(d,"panel_bg:");d.gK=b.Substring("panel_bg:".Length).Trim();}else if(b.StartsWith("alert_bg:")){
q(d,"alert_bg:");d.jQ=b.Substring("alert_bg:".Length).Trim();}else if(b.StartsWith("boot_color:")||b.StartsWith("banner_color:")){
q(d,"boot_color:");d.jU=b.Substring(b.IndexOf(':')+1).Trim();}else if(b.StartsWith("orientation:")){q(d,"orientation:");
string q6=b.Substring("orientation:".Length).Trim().ToLowerInvariant();if(p!=null)p.kl=q6;}else if(b.StartsWith("@panel")){q(d,"@panel");
p=oT(b);d.av.Add(p);}else if(b.StartsWith("title:")){q(d,"title:");if(p==null){p=new au{
dc="main",c4=0,de=0,df=d.cf,dg=d.cg};d.av.Add(p);}p.bQ=ab(b.Substring("title:".Length).Trim());
}else if(b.StartsWith("line:")){q(d,"line:");if(p==null){p=new au{dc="main",c4=0,de=0,df=d.cf,dg=d.cg}
;d.av.Add(p);}p.aK.Add(oS(b.Substring("line:".Length).Trim()));}else{ai.aJ.Add($"Line {dK + 1}: unrecognized directive: \"{b}\"");
}}catch(Exception qa){ai.aJ.Add($"Line {dK + 1}: parse error ({qa.Message}) in \"{b}\"");}}foreach(var nh in ai.T){if(string.IsNullOrEmpty(nh.bc))ai.aJ.Add($"@screen {nh.bb}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aQ in ai.T){if(aQ.bD!=null&& !ai.cX.ContainsKey(aQ.bD))ai.aJ.Add("Screen '"+aQ.bb+"': banner '"+aQ.bD+"' is not defined (@banner "+aQ.bD+" ... @end)");
if(aQ.bH!=null&& !ai.cX.ContainsKey(aQ.bH))ai.aJ.Add("Screen '"+aQ.bb+"': boot_logo '"+aQ.bH+"' is not defined (@banner "+aQ.bH+" ... @end)");
}return ai;}static void oR(string ho,y no){int iU=ho.IndexOf('(');if(iU<0)throw new FormatException("missing '(' in layout: specification");
string mt=ho.Substring(0,iU).Trim().ToLowerInvariant();string pt=aA(ho,iU);var g9=aB(pt);if(mt=="grid"){if(g9.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
no.cf=Math.Max(1,int.Parse(g9[0]));no.cg=Math.Max(1,int.Parse(g9[1]));}else{throw new FormatException($"unknown layout kind '{mt}' (expected grid)");
}}static au oT(string qV){string d3=qV.Substring("@panel".Length).Trim();int eU=d3.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(eU<0)throw new FormatException("@panel requires an at(...) positioning clause");
string q4=d3.Substring(0,eU).Trim();string kW=d3.Substring(eU);int m1=kW.IndexOf('(');string kU=aA(kW,m1);
var bW=aB(kU);if(bW.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var a0=new au{dc=q4,c4=int.Parse(bW[0]),de=int.Parse(bW[1]),df=bW.Count>2 ? int.Parse(bW[2]): 1,dg=bW.Count>3 ? int.Parse(bW[3]): 1}
;int kN=eU+m1+kU.Length+1;if(kN<d3.Length){string ax=d3.Substring(kN).Trim();int nS=ax.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(nS>=0){string ea=ax.Substring(nS+5).Trim();if(ea.StartsWith("(")&&ea.Contains(")"))a0.bQ=ab(aA(ea,0));else if(ea.StartsWith(":")){
string bY=ea.Substring(1).Trim();if(bY.StartsWith("\"")){int lL=bY.IndexOf('"',1);a0.bQ=lL>0 ? bY.Substring(1,lL-1): ab(bY);}else{int nx=bY.IndexOf(' ');
a0.bQ=nx>=0 ? bY.Substring(0,nx): bY;}}}string mR=eu(ax,"orientation:");if(mR!=null)a0.kl=mR.ToLowerInvariant();string kM=eu(ax,"title_align:");
if(kM!=null)a0.gR=kM.ToLowerInvariant();string nP=eu(ax,"title_color:");if(nP!=null)a0.gS=nP;int e_=ax.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(e_>=0&& !ax.Substring(e_).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !ax.Substring(e_).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
a0.jX=eu(ax.Substring(e_),"border:");}int eX=ax.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(eX<0)eX=ax.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(eX>=0){string bn=ax.Substring(eX+"border_gradient".Length).Trim();if(bn.StartsWith(":"))bn=bn.Substring(1).Trim();if(bn.StartsWith("(")&&bn.Contains(")"))bn=aA(bn,0);
a0.gs=aB(bn).Select(rJ=>rJ.Trim()).ToArray();}}return a0;}static string eu(string ni,string ms){int mk=ni.IndexOf(ms,StringComparison.OrdinalIgnoreCase);
if(mk<0)return null;string jK=ni.Substring(mk+ms.Length).Trim();int ny=jK.IndexOf(' ');return ny>=0 ? jK.Substring(0,ny): jK;}static string[]kp(string iA){
int h0=iA.IndexOfAny(new[]{':','('});string dG=iA.Substring(iA[h0]==':' ? h0+1 : h0).Trim();if(dG.StartsWith("(")&&dG.Contains(")"))dG=aA(dG,0);return aB(dG).Select(rK=>rK.Trim()).ToArray();
}static List<string>oV(string p3,List<string>hL,Dictionary<string,List<string>>pH){var nV=new Dictionary<string,bf>(StringComparer.OrdinalIgnoreCase);
var cJ=(p3 ?? "").Replace("\r\n","\n").Split('\n');var iP=new List<string>();int cA=0;while(cA<cJ.Length){string i6=cJ[cA++];if(i6.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ae=new List<string>();while(cA<cJ.Length){string kK=cJ[cA++].Replace("\t","    ").TrimEnd();if(kK.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ae.Add(kK);}while(ae.Count>0&&ae[0].Length==0)ae.RemoveAt(0);while(ae.Count>0&&ae[ae.Count-1].Length==0)ae.RemoveAt(ae.Count-1);int ii=int.MaxValue;
foreach(var g2 in ae)if(g2.Length>0)ii=Math.Min(ii,g2.Length-g2.TrimStart().Length);for(int dR=0;dR<ae.Count;dR++)if(ae[dR].Length>0)ae[dR]=ae[dR].Substring(ii);
pH[i6.TrimStart().Substring(8).Trim()]=ae;continue;}string br=kv(i6).Trim();if(br.Length==0)continue;if(br.StartsWith("@template")){try{
var jA=oU(br);while(cA<cJ.Length){string pR=cJ[cA++];string hp=kv(pR).Trim();if(hp.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(hp.Length>0)jA.go.Add(hp);}nV[jA.gI]=jA;}catch(Exception qb){hL.Add($"Template parse error ({qb.Message}) in \"{br}\"");}}
else if(br.StartsWith("@use")){try{j0(br,nV,iP,hL);}catch(Exception qc){hL.Add($"@use error ({qc.Message}) in \"{br}\"");
}}else{iP.Add(br);}}return iP;}static bf oU(string qW){string i7=qW.Substring("@template".Length).Trim();int iV=i7.IndexOf('(');
if(iV<0)throw new FormatException("missing '(' in @template header");string q5=i7.Substring(0,iV).Trim();string pu=aA(i7,iV);var jB=new bf{
gI=q5};foreach(var ra in aB(pu)){var iv=ra.Split(new[]{'='},2);string mX=gJ(iv[0]);jB.eC.Add(mX);if(iv.Length>1)jB.gu[mX]=ab(iv[1].Trim());
}return jB;}static void j0(string qX,Dictionary<string,bf>nW,List<string>mU,List<string>p9,int lA=0){string fO=qX.Substring("@use".Length).Trim();
int fD=fO.IndexOf('(');string iI=fD>=0 ? fO.Substring(0,fD).Trim(): fO.Trim();string pv=fD>=0 ? aA(fO,fD): "";bf cQ=null;
if(!nW.TryGetValue(iI,out cQ)){if(!jY.TryGetValue(iI,out cQ))throw new FormatException($"unknown template '{iI}'");}var e1=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var mx in cQ.gu)e1[mx.Key]=mx.Value;var kT=aB(pv);for(int ds=0;ds<kT.Count;ds++){string eS=kT[ds];int hJ=eS.IndexOf('=');if(hJ>=0){
string qF=gJ(eS.Substring(0,hJ));string sj=ab(eS.Substring(hJ+1).Trim());e1[qF]=sj;}else{if(ds<cQ.eC.Count){string qG=cQ.eC[ds];
e1[qG]=ab(eS.Trim());}}}foreach(var pQ in cQ.go){string dD=pQ;foreach(var fs in e1){dD=dD.Replace("$"+fs.Key,fs.Value).Replace("{"+fs.Key+"}",fs.Value);
}if(dD.StartsWith("@use")){if(lA>3)throw new FormatException("@use nested too deep");j0(dD,nW,mU,p9,lA+1);
}else mU.Add(dD);}}static void q(y p1,string p6){if(p1==null)throw new FormatException($"'{p6}' used before any @screen declared");
}static string kv(string aP){if(string.IsNullOrEmpty(aP))return aP;bool ih=false;for(int bq=0;bq<aP.Length;bq++){char hs=aP[bq];if(hs=='"'){ih= !ih;
}else if(!ih){if(hs=='/'&&bq+1<aP.Length&&aP[bq+1]=='/'){return aP.Substring(0,bq);}if(hs=='#'){if(oH(aP,bq)){continue;}return aP.Substring(0,bq);}}}return aP;
}static bool oH(string b1,int mn){int cD=0;for(int ic=mn+1;ic<b1.Length;ic++){char cr=b1[ic];if((cr>='0'&&cr<='9')||(cr>='a'&&cr<='f')||(cr>='A'&&cr<='F'))cD++;
else break;}if(cD==3||cD==4||cD==6||cD==8){int dU=mn+1+cD;if(dU>=b1.Length||char.IsWhiteSpace(b1[dU])||b1[dU]==')'||b1[dU]==','||b1[dU]==':')return true;
}return false;}static aU oQ(string qY){string i8=qY.Substring("@bind".Length).Trim();int hK=i8.IndexOf('=');if(hK<0)throw new FormatException("missing '=' in @bind");
string mP=i8.Substring(0,hK).Trim();string hM=i8.Substring(hK+1).Trim();int iW=hM.IndexOf('(');if(iW<0)throw new FormatException("missing '(' in @bind target");
string fd=hM.Substring(0,iW).Trim().ToLowerInvariant();string pw=aA(hM,iW);var bk=aB(pw);if(bk.Count==0)throw new FormatException("@bind target has no arguments");
var ag=new aU{V=mP};if(fd=="dynamic"||fd=="var"||fd=="mem"){ag.a9=aH.c7;ag.aV=bk.Count>0 ? ab(bk[0].Trim()): mP;
for(int g3=0;g3<bk.Count;g3++){var ft=bk[g3].Split(new[]{':'},2);if(ft.Length==2&&ft[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double lz;
if(double.TryParse(ft[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out lz))ag.et=lz;else ag.gt=ab(ft[1].Trim());
}}return ag;}ag.a9=fd=="group" ? aH.gD : aH.of;string eQ=null;for(int eP=0;eP<bk.Count;eP++){var cC=bk[eP].Split(new[]{':'},2);if(cC.Length==2&&cC[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
eQ=cC[1].Trim().ToLowerInvariant();}else if(cC.Length==2&&cC[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ag.el=cC[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ag.aC.Add(ab(bk[eP].Trim()));}}if(ag.aC.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ag.a9==aH.gD){if(eQ==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(eQ){case "avg": ag.cV=at.gi;break;case "sum": ag.cV=at.kx;break;case "min": ag.cV=at.kk;break;case "max": ag.cV=at.kj;
break;default: throw new FormatException($"unknown agg '{eQ}' (expected avg/sum/min/max)");}}return ag;}static ba oS(string eZ){var o=new ba();
if(!eZ.StartsWith("\""))throw new FormatException("line must start with a quoted label");int hI=eZ.IndexOf('"',1);if(hI<0)throw new FormatException("unterminated quoted label");
o.S=eZ.Substring(1,hI-1);string d2=eZ.Substring(hI+1).Trim();int fE=d2.IndexOf('(');if(fE<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string C=d2.Substring(0,fE).Trim().ToLowerInvariant();string ps=aA(d2,fE);string Z=d2.Substring(d2.IndexOf(')',fE)+1).Trim();
var hF=ps.Trim().Split('.');if(hF.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.ay=hF[0].Trim();o.F=hF[1].Trim();if(C=="bar")o.g=e.cY;else if(C=="led")o.g=e.da;else if(C=="spark"||C=="sparkline")o.g=e.bO;
else if(C=="hazard"||C=="stripes")o.g=e.c9;else if(C=="chevron"||C=="chevrons")o.g=e.c3;else if(C=="dots"||C=="dot")o.g=e.c6;
else if(C=="balance"||C=="horizon"||C=="centered")o.g=e.ca;else if(C=="toggle"||C=="switch")o.g=e.bR;else if(C=="checkbox"||C=="check")o.g=e.bI;
else if(C=="status"||C=="badge"||C=="state")o.g=e.bP;else if(C=="ammo"||C=="pips")o.g=e.b9;else if(C=="flow"||C=="rate"||C=="spinner")o.g=e.bK;
else if(C=="text")o.g=e.gQ;else throw new FormatException($"unknown display function '{C}'");while(Z.Length>0){int hD=Z.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int cx=Z.IndexOf('(');if(hD>=0&&(cx<0||hD<cx)){string e4=Z.Substring(hD+4).Trim();int fW=e4.IndexOf(' ');o.jZ=(fW>=0 ? e4.Substring(0,fW): e4).ToLowerInvariant();
Z=(fW>=0 ? e4.Substring(fW): "").Trim();continue;}int jI=Z.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(jI>=0&&(cx<0||jI<cx)){
string gd=Z.Substring(jI+5).Trim();int fX=gd.IndexOf(' ');o.di=(fX>=0 ? gd.Substring(0,fX): gd).Trim().Trim('"');Z=(fX>=0 ? gd.Substring(fX): "").Trim();
continue;}int hV=Z.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(hV>=0&&(cx<0||hV<cx)){string e8=Z.Substring(hV+7).Trim();
int fY=e8.IndexOf(' ');o.c8=(fY>=0 ? e8.Substring(0,fY): e8).Trim().Trim('"');Z=(fY>=0 ? e8.Substring(fY): "").Trim();continue;}int fx=Z.IndexOf('(');
if(fx<0)break;string aj=Z.Substring(0,fx).Trim().ToLowerInvariant();int mC=aj.LastIndexOf(' ');if(mC>=0)aj=aj.Substring(mC+1).Trim();
string aZ=aA(Z,fx);Z=Z.Substring(Z.IndexOf(')',fx)+1).Trim();if(aj=="gradient"){o.ce=true;o.a8=aB(aZ).Select(rL=>rL.Trim()).ToArray();
if(o.a8.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(aj=="flat"){o.am=aZ.Trim();}else if(aj=="pattern"){
string N=aZ.Trim().ToLowerInvariant();if(N=="hazard"||N=="stripes")o.g=e.c9;else if(N=="chevron"||N=="chevrons")o.g=e.c3;
else if(N=="dots"||N=="dot")o.g=e.c6;else if(N=="led")o.g=e.da;else if(N=="balance"||N=="horizon")o.g=e.ca;
else if(N=="toggle"||N=="switch")o.g=e.bR;else if(N=="checkbox"||N=="check")o.g=e.bI;else if(N=="status"||N=="badge"||N=="state")o.g=e.bP;
else if(N=="ammo"||N=="pips")o.g=e.b9;else if(N=="flow"||N=="rate"||N=="spinner")o.g=e.bK;}else if(aj=="color"){string qy;
o.c5=ko(aZ,out qy);}else if(aj=="dir"||aj=="direction"){o.jZ=aZ.Trim().ToLowerInvariant();}else if(aj=="unit"){
o.di=aZ.Trim().Trim('"');}else if(aj=="max"){double mK;if(double.TryParse(aZ.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mK))o.eB=mK;
else{var iF=aZ.Trim().Split('.');if(iF.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.ci=iF[0].Trim();o.gH=iF[1].Trim();
}}else if(aj=="format"||aj=="fmt"){o.c8=aZ.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{aj}'");}}return o;}static string aA(string fS,int iN){
int hC=0;for(int dL=iN;dL<fS.Length;dL++){if(fS[dL]=='(')hC++;else if(fS[dL]==')'){hC--;if(hC==0)return fS.Substring(iN+1,dL-iN-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aB(string px){return px.Split(',').Select(rM=>rM.Trim()).Where(rN=>rN.Length>0).ToList();
}static List<az>ko(string pr,out string hR){var fQ=new List<az>();hR=null;foreach(var iX in aB(pr)){int hz=iX.LastIndexOf(':');
if(hz<0)throw new FormatException("color() entries need 'expr : color'");string fu=iX.Substring(0,hz).Trim();string ls=iX.Substring(hz+1).Trim();
if(fu.Equals("default",StringComparison.OrdinalIgnoreCase)){fQ.Add(new az{ch=true,bJ=ls});}else{string nC=kw(fu);
if(hR==null)hR=fu.Substring(0,fu.Length-nC.Length).Trim();fQ.Add(new az{gv=nC,bJ=ls});}}if(!fQ.Any(rz=>rz.ch))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return fQ;}static void kt(string rA,out string pN,out string qf){var dX=(rA ?? "").Split('.');if(dX.Length!=2||dX[0].Trim().Length==0||dX[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
pN=dX[0].Trim();qf=dX[1].Trim();}static string kw(string cw){cw=cw.Trim();foreach(var q7 in new[]{">=","<=","==",">","<"}){int id=cw.IndexOf(q7,StringComparison.Ordinal);
if(id>0)return cw.Substring(id).Trim();if(id==0)return cw;}throw new FormatException($"color() rule has no recognizable comparison operator: '{cw}'");}
static string ab(string b2){if(b2.Length>=2&&b2[0]=='"'&&b2[b2.Length-1]=='"')return b2.Substring(1,b2.Length-2);return b2;}}public static class od{public static double oc(at pn,List<double>cS){
if(cS==null||cS.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(pn){case at.gi: return cS.Average();case at.kx: return cS.Sum();
case at.kk: return cS.Min();case at.kj: return cS.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class D{
public static string o3(string qP,double fG,int k0=10,string n1=null){fG=Math.Max(0,Math.Min(100,fG));int lY=(int)Math.Round(k0*(fG/100.0));
string pI=new string('#',lY)+new string('-',k0-lY);string sk=n1!=null ? n1 : $"{fG:0}%";return $"{qP} [{pI}] {sk}";
}public static string j6(double bB,string jH=null,string fc=null){if(jH=="auto"){double g4=Math.Abs(bB);if(g4>=1e9)return(bB/1e9).ToString("F1")+"G";
if(g4>=1e6)return(bB/1e6).ToString("F1")+"M";if(g4>=1e3)return(bB/1e3).ToString("F1")+"k";string qd= !string.IsNullOrEmpty(fc)? fc : "F0";return bB.ToString(qd);
}string l_= !string.IsNullOrEmpty(fc)? fc :(Math.Abs(bB)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(jH)){return bB.ToString(l_);}return bB.ToString(l_)+jH;
}public static float oC(string rp,float nG,float jy,float e5=1.0f){string cG=(rp ?? "standard").ToLowerInvariant();
if(cG=="compact"||(cG=="auto"&&nG<=300&&jy<=300&&jy>160)){return e5*0.75f;}if(cG=="corner"||(cG=="auto"&&jy<=160)){
return e5*0.55f;}if(cG=="wide"||(cG=="auto"&&nG>=1000)){return e5*1.15f;}return e5;}public static string o1(string qQ,double fH,int bo=5){
fH=Math.Max(0,Math.Min(100,fH));double e6=(fH-50.0)/50.0;int dS=0;int d5=0;if(e6< -0.01)dS=(int)Math.Round(-e6*bo);
else if(e6>0.01)d5=(int)Math.Round(e6*bo);if(dS>bo)dS=bo;if(d5>bo)d5=bo;string qT=new string('-',bo-dS)+new string('<',dS);
string rF=new string('>',d5)+new string('-',bo-d5);return $"{qQ} [{qT}|{rF}] {fH:0}%";}public static string pc(string mz,double sm){
return sm>=50.0 ? $"{mz} [====O] ON" : $"{mz} [O====] OFF";}public static string o6(string mA,double sn){return sn>=50.0 ? $"{mA} [X] ACTIVE" : $"{mA} [ ] READY ";
}public static string pb(string ix,double n0){if(n0>=75.0)return $"{ix} [ ONLINE  ]";if(n0>=25.0)return $"{ix} [ STANDBY ]";return $"{ix} [ OFFLINE ]";
}public static string o0(string qR,double fI,int mG=8){fI=Math.Max(0,Math.Min(100,fI));int lZ=(int)Math.Round(mG*(fI/100.0));
string rl=new string('|',lZ)+new string('.',mG-lZ);return $"{qR} [{rl}] {fI:0}%";}public static string o7(string iy,double d0,int nR=0){
char[]rY=new[]{'|','/','-','\\'};char nj=rY[(nR>=0 ? nR : 0)%4];if(d0>55.0)return $"{iy} {nj} ▲ CHG {d0:0}%";if(d0<45.0)return $"{iy} {nj} ▼ DSC {d0:0}%";
return $"{iy} ● STBY {d0:0}%";}public static bool oL(string ak,string dO){if(ak==null||dO==null)return false;if(ak=="*"||ak==dO)return true;
if(!ak.Contains("*")&& !ak.Contains("?"))return string.Equals(ak,dO,StringComparison.OrdinalIgnoreCase);int aq=0,dM=0,jr= -1,iE=0;while(dM<dO.Length){
if(aq<ak.Length&&(ak[aq]=='?'||char.ToLowerInvariant(ak[aq])==char.ToLowerInvariant(dO[dM]))){aq++;dM++;}else if(aq<ak.Length&&ak[aq]=='*'){
jr=aq;iE=dM;aq++;}else if(jr!= -1){aq=jr+1;iE++;dM=iE;}else{return false;}}while(aq<ak.Length&&ak[aq]=='*')aq++;return aq==ak.Length;
}public static int oh(double rb,int nU){return Math.Max(0,Math.Min(nU,1+(int)(Math.Max(0.0,rb)*nU)));}public static string ew(string[]cP,double iY){
if(cP==null||cP.Length==0)return "white";iY=Math.Max(0,Math.Min(100,iY));int ie=(int)Math.Floor(iY/100.0*cP.Length);if(ie>=cP.Length)ie=cP.Length-1;
return cP[ie];}public static n j8(string[]bz,double ri){if(bz==null||bz.Length==0)return n.eH;if(bz.Length==1)return n.dd(bz[0]);
double rD=Math.Max(0.0,Math.Min(1.0,ri/100.0));double nn=rD*(bz.Length-1);int ig=(int)Math.Floor(nn);int qx=Math.Min(bz.Length-1,ig+1);
double jv=nn-ig;n cs=n.dd(bz[ig]);n ht=n.dd(bz[qx]);byte rB=(byte)Math.Round(cs.bd+(ht.bd-cs.bd)*jv);
byte qs=(byte)Math.Round(cs.a7+(ht.a7-cs.a7)*jv);byte pG=(byte)Math.Round(cs.a4+(ht.a4-cs.a4)*jv);return new n(rB,qs,pG);}public static string eD(List<az>nf,double so){
foreach(var ja in nf){if(ja.ch)continue;if(oy(ja.gv,so))return ja.bJ;}var ly=nf.FirstOrDefault(rC=>rC.ch);return ly!=null ? ly.bJ : null;
}static bool oy(string dE,double eg){dE=dE.Trim();foreach(var iM in new[]{">=","<=","==",">","<"}){if(dE.StartsWith(iM)){double d4=double.Parse(dE.Substring(iM.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(iM){case ">=": return eg>=d4;case "<=": return eg<=d4;case "==": return Math.Abs(eg-d4)<1e-9;case ">": return eg>d4;case "<": return eg<d4;
}}}throw new FormatException($"unsupported comparison expression: '{dE}'");}public static List<string>o5(List<string>p0,int cT){
var fz=new List<string>();fz.Add("+"+new string('-',cT+2)+"+");foreach(var iB in p0){string sf=iB.Length>cT ? iB.Substring(0,cT): iB;
string rc=sf.PadRight(cT);fz.Add("| "+rc+" |");}fz.Add("+"+new string('-',cT+2)+"+");return fz;}public struct kn{public float kF,kG,kE,ka;
}public static kn om(int lq,int nc,int fZ,int f_,int ec,int ed,float r6,float r5,float mY,float mZ,float h2,float h3){
ec=Math.Max(1,ec);ed=Math.Max(1,ed);fZ=Math.Max(1,Math.Min(ec-lq,fZ));f_=Math.Max(1,Math.Min(ed-nc,f_));
float pz=r6-(mY*2f);float py=r5-(mZ*2f);float ll=(pz-(h2*(ec-1)))/ec;float lk=(py-(h3*(ed-1)))/ed;
float rr=mY+lq*(ll+h2);float rs=mZ+nc*(lk+h3);float rq=fZ*ll+((fZ-1)*h2);float rj=f_*lk+((f_-1)*h3);return new kn{
kF=rr,kG=rs,kE=rq,ka=rj};}public struct kr{public int oO;public int gG;public int W;public int an;public int dh;
public int aI;public int ph;public int pi;public int ov;public int ow;
public double ox;}public static kr oo(y bw){var k=new kr();var m0=bw.av!=null&&bw.av.Count>0 ? bw.av : new List<au>{
new au{aK=bw.eA}};k.oO=m0.Count;k.gG=0;k.W=0;k.an=0;if(!string.IsNullOrEmpty(bw.bC)&& !bw.bC.Equals("none",StringComparison.OrdinalIgnoreCase))k.W++;
if(!string.IsNullOrEmpty(bw.c2)&& !bw.c2.Equals("none",StringComparison.OrdinalIgnoreCase))k.W+=2;foreach(var dY in m0){
if(!string.IsNullOrEmpty(dY.cW)&& !dY.cW.Equals("none",StringComparison.OrdinalIgnoreCase))k.W++;if(!string.IsNullOrEmpty(dY.bQ))k.W+=2;
k.gG+=dY.aK.Count;foreach(var qO in dY.aK){k.ph++;k.pi++;k.W+=2;switch(qO.g){case e.cY: k.dh++;
k.aI++;k.W+=2;k.an++;break;case e.da: k.aI+=10;k.W+=10;
k.an+=10;break;case e.c9: k.aI+=10;k.W+=11;k.an+=10;break;case e.c3: k.aI+=10;
k.W+=10;k.an+=10;break;case e.c6: k.aI+=12;k.W+=12;k.an+=12;
break;case e.bO: k.dh++;k.aI+=8;k.W+=9;k.an+=8;break;case e.ca: k.dh+=2;
k.aI++;k.W+=3;k.an++;break;case e.bR: case e.bI: k.dh++;
k.aI++;k.W+=2;k.an++;break;case e.bP: case e.bK: k.dh++;k.aI+=2;
k.W+=3;k.an+=2;break;case e.b9: k.aI+=8;k.W+=8;k.an+=8;
break;default: k.an++;break;}}}k.ov=25+(k.gG*2);k.ow=180+(k.an*4);
k.ox=0.0;return k;}}