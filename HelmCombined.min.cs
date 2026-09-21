gN r;string g1=null;Dictionary<string,List<IMyTerminalBlock>>bU=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>eM=new Dictionary<string,bool>();
Dictionary<string,aU>dk=new Dictionary<string,aU>();Dictionary<string,Color>kL=new Dictionary<string,Color>();List<IMyTerminalBlock>bS=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>ck=new List<IMyTerminalBlock>();bool eK;int gZ;string dl="";StringBuilder dq=new StringBuilder();readonly Dictionary<string,double>ax=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>dm=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener dn;bool bT=false;
Dictionary<y,IMyTextSurface>bV=new Dictionary<y,IMyTextSurface>();Dictionary<y,string>cj=new Dictionary<y,string>();
static readonly string[]kr=new string[101];static Program(){for(int fh=0;fh<=100;fh++){kr[fh]=fh+"%";}}static string kc(double sp){int dP=(int)Math.Round(sp);
if(dP<0)dP=0;else if(dP>100)dP=100;return kr[dP];}class aX{public List<MySprite>l=new List<MySprite>();public List<db>eH=new List<db>();
public int bF= -1;public Color[]ep;public Color ek;public bool el;public Vector2 kl;public bool ki=false;public List<eq>c_=new List<eq>();
}class eq{public int ez;public Color gl,eu;public List<aB>eG,ex;public string gp,gy,gz,gB;public bool kj,gD;
}class db{public bd kn;public int aa= -1;public int bi= -1;public int gH= -1;public bool ke=false;
public List<int>af=new List<int>();public float[]eI=new float[8];public float cY;public float eo;public float cc;
public float cX;public double eB= -99999.0;public string km;public float Q= -1f;}Dictionary<y,aX>eO=new Dictionary<y,aX>();
int kN=0;double g3=0;double aq=0;int dp=0;Dictionary<y,double>aY=new Dictionary<y,double>();Dictionary<y,double>gY=new Dictionary<y,double>();
Dictionary<string,int>dt=new Dictionary<string,int>();List<string>cl=new List<string>();Dictionary<string,double>ds=new Dictionary<string,double>();
List<MyInventoryItem>g0=new List<MyInventoryItem>();int bk= -1,eP;double g4;Dictionary<y,double>eL=new Dictionary<y,double>();
int dr=0;const string oK="0.11.0";IMyTextSurface bj;bool gX=true;double gW=0;string kM="";string g2="";
int eN=0,g_=0;List<KeyValuePair<string,int>>ci=new List<KeyValuePair<string,int>>();const double o1=120;const float of=22f;
public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{dn=IGC.RegisterBroadcastListener("HELM");dn.SetMessageCallback("HELM_IGC");
}catch{}gP();kz();kh();}public void Main(string kZ,UpdateType sl){double lM=Runtime.TimeSinceLastRun.TotalSeconds;aq+=lM;
if((sl&UpdateType.IGC)!=0||kZ=="HELM_IGC"){o_();}string ne=(kZ ?? "").Trim();string ar=ne.ToLowerInvariant();if(ar=="reload"){
gP();}else if(ar=="refresh"){gQ();}else if(ar=="boot"){kz();}else if(ar=="console on"||ar=="console off"){gX=ar=="console on";
kh();gW=0;}else if(ar=="scan"||ar=="diagnose"||ar=="test"){pj();return;}else if(ar.StartsWith("set ")){string fO=ne.Substring(4).Trim();
int lQ=fO.IndexOf('=');int rZ=fO.IndexOf(' ');int jr=lQ>0 ? lQ : rZ;if(jr>0){string mu=fO.Substring(0,jr).Trim();string n_=fO.Substring(jr+1).Trim();jW($"{mu}={n_}");
Echo($"[HELM] Set dynamic variable '{mu}' = {n_}");}return;}else if(ar.StartsWith("alert")){string eh=ar.Length>5 ? ar.Substring(5).Trim(): "toggle";if(eh=="toggle")bT= !bT;
else bT=eh=="red"||eh=="on"||eh=="1"||eh=="true";Echo($"[HELM] Red alert state: {bT}");return;}else if(Me.CustomData!=g1){
gP();}dr++;g3+=lM;if(g3>=o1){g3=0;gQ();}if(r==null||r.T.Count==0){
kG();j4();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var b3=r.T[kN%r.T.Count];
kN++;g2=b3.aW;bool g8=aY.Count>0;if(g8)o8();double lN;if(!aY.ContainsKey(b3)&& !(gY.TryGetValue(b3,out lN)&&aq<lN)){
if(pc(b3))g8=true;if(b3.gO>0)gY[b3]=aq+b3.gO/60.0;}dp=g8 ? 0 : dp+1;Runtime.UpdateFrequency=dp<=r.T.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
pk();kG();j4();eN=Runtime.CurrentInstructionCount;if(eN>g_)g_=eN;}void j4(){if(dl.Length>0)Echo(dl);
if(dq.Length>0)Echo(dq.ToString());}List<IMyTerminalBlock>em(bool ps=false){if(!eK){ck.Clear();GridTerminalSystem.GetBlocks(ck);
bS.Clear();bS.AddRange(ck);bS.RemoveAll(pE=> !pE.IsSameConstructAs(Me));gZ=ck.Count-bS.Count;eK=true;
}return ps ? ck : bS;}IMyTerminalBlock j7(string mQ,bool pu=false){var hg=GridTerminalSystem.GetBlockWithName(mQ);if(hg==null||pu||hg.IsSameConstructAs(Me))return hg;
var ha=em();for(int fi=0;fi<ha.Count;fi++)if(ha[fi].CustomName.IndexOf(mQ,StringComparison.OrdinalIgnoreCase)>=0)return ha[fi];return null;}void oq(string mR,List<IMyTerminalBlock>dR,bool kS=false){
var l6=GridTerminalSystem.GetBlockGroupWithName(mR);if(l6==null)return;int k8=dR.Count;var ed=new List<IMyTerminalBlock>();l6.GetBlocks(ed);jU(ed,dR,kS);
if(dR.Count>k8)return;var mh=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(mh);foreach(var oa in mh){if(oa.Name.IndexOf(mR,StringComparison.OrdinalIgnoreCase)<0)continue;
ed.Clear();oa.GetBlocks(ed);jU(ed,dR,kS);if(dR.Count>k8)return;}}void jU(List<IMyTerminalBlock>qr,List<IMyTerminalBlock>ms,bool pt=false){
foreach(var hh in qr)if((pt||hh.IsSameConstructAs(Me))&& !ms.Contains(hh))ms.Add(hh);}void gP(){g1=Me.CustomData;r=kg.oT(g1);
dl="";if(r.aK.Count>0){var no=new StringBuilder("HELM config errors:\n");foreach(var qc in r.aK)no.AppendLine(" - "+qc);dl=no.ToString();
}if(r.dj.Count>0){var n9=new StringBuilder("HELM notes:\n");foreach(var su in r.dj)n9.AppendLine(" - "+su);dl+=n9.ToString();}dk.Clear();
foreach(var hi in r.bG)if(!dk.ContainsKey(hi.V))dk[hi.V]=hi;eO.Clear();cj.Clear();aY.Clear();gY.Clear();
eL.Clear();oE();gQ();}void o_(){try{while(dn!=null&&dn.HasPendingMessage){
var mM=dn.AcceptMessage();if(mM.Data is string){jW((string)mM.Data);}}}catch{}}void jW(string m6){
if(string.IsNullOrEmpty(m6))return;var m3=m6.Split(';');for(int h9=0;h9<m3.Length;h9++){var d1=m3[h9].Trim();if(d1.Length==0)continue;int dE=d1.IndexOf('=');
if(dE<0)dE=d1.IndexOf(':');if(dE>0){string cA=d1.Substring(0,dE).Trim();string aT=d1.Substring(dE+1).Trim();if(cA.Equals("alert",StringComparison.OrdinalIgnoreCase)){
bT=aT.Equals("red",StringComparison.OrdinalIgnoreCase)||aT=="1"||aT.Equals("true",StringComparison.OrdinalIgnoreCase)||aT.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double mU;dm.Remove(cA);if(double.TryParse(aT,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mU)){
ax[cA]=mU;}else if(aT.Equals("true",StringComparison.OrdinalIgnoreCase)||aT.Equals("on",StringComparison.OrdinalIgnoreCase)){ax[cA]=100.0;
}else if(aT.Equals("false",StringComparison.OrdinalIgnoreCase)||aT.Equals("off",StringComparison.OrdinalIgnoreCase)){ax[cA]=0.0;}else{dm[cA]=aT;
}}}}}static bool kk(string hQ){return hQ.Length>3&&hQ.StartsWith("top")&&char.IsDigit(hQ[3]);}void oE(){dt.Clear();cl.Clear();bk= -1;g4=0;
foreach(var jn in r.T){var iF=new List<List<bd>>();foreach(var rq in jn.ab)iF.Add(rq.aL);if(jn.ab.Count==0)iF.Add(jn.eC);
foreach(var q6 in iF)foreach(var fx in q6){if(!kk(fx.F))continue;int q7=int.Parse(fx.F.Substring(3));int ml;dt.TryGetValue(fx.aA,out ml);
dt[fx.aA]=Math.Max(ml,q7);}}cl.AddRange(dt.Keys);}void pk(){if(cl.Count==0)return;if(bk<0){if(aq<g4)return;bk=0;
eP=0;ds.Clear();}while(bk<cl.Count&&Runtime.CurrentInstructionCount<20000){string hq=cl[bk];List<IMyTerminalBlock>eZ;bU.TryGetValue(hq,out eZ);
if(eZ!=null&&eP<eZ.Count){var k0=eZ[eP++];for(int ia=0;ia<k0.InventoryCount;ia++){g0.Clear();k0.GetInventory(ia).GetItems(g0);foreach(var fp in g0){
string it=fp.Type.TypeId.Replace("MyObjectBuilder_","");string mv=it=="Ore"||it=="Ingot" ? fp.Type.SubtypeId+" "+it.ToLowerInvariant(): fp.Type.SubtypeId;
double mm;ds.TryGetValue(mv,out mm);ds[mv]=mm+(double)fp.Amount;}}continue;}var fM=new List<KeyValuePair<string,double>>(ds);fM.Sort((sw,sA)=>sA.Value.CompareTo(sw.Value));
for(int cH=1;cH<=dt[hq];cH++)dm[hq+".top"+cH]=cH<=fM.Count ? fM[cH-1].Key+" "+D.kb(fM[cH-1].Value,"auto"): "-";ds.Clear();
bk++;eP=0;}if(bk>=cl.Count){bk= -1;g4=aq+15.0;}}void ae(int r7,string se){ci.Add(new KeyValuePair<string,int>(se,r7));
}static string gV(string jg,int mP){return jg.Length>mP ? jg.Substring(0,mP): jg;}void kh(){var i8=Me as IMyTextSurfaceProvider;if(i8==null||i8.SurfaceCount<1)return;
bj=i8.GetSurface(0);bj.ContentType=gX ? ContentType.SCRIPT : ContentType.NONE;bj.Script="";}void kG(){if(aq>=gW){
gW=aq+(dp>r.T.Count+1 ? 5.0 : 1.0);oo();if(gX&&bj!=null)oy();}Echo(kM);}void oo(){
ci.Clear();int ht=0,le=0;foreach(var k1 in r.bG){List<IMyTerminalBlock>iz;if(k1.bc==aI.c5){ht++;continue;}if(bU.TryGetValue(k1.V,out iz)){
le+=iz.Count;if(iz.Count>0)ht++;}}int lR=r.aK.Count;ae(0,"HELM-[Kernel-(v"+oK+")]");ae(4,"------------------------------");
ae(1,"krn-cfg");ae(2," screens "+r.T.Count+"  binds "+ht+"/"+r.bG.Count+"  errors "+lR);ae(2," alert "+(bT ? "RED" : "off")+"  blocks "+le+"  docked-ignored "+gZ);
if(lR>0)ae(3," ! "+gV(r.aK[0],40));else if(r.dj.Count>0)ae(3," ? "+gV(r.dj[0],40));ae(1,"cfg-dsp");int rW=0;
foreach(var jh in r.T){if(++rW>10){ae(2," +"+(r.T.Count-10)+" more");break;}IMyTextSurface js;string nx="--",jx="no block";if(bV.TryGetValue(jh,out js)){
nx=js.SurfaceSize.X.ToString("0")+"x"+js.SurfaceSize.Y.ToString("0");jx=aY.ContainsKey(jh)? "boot" : "live";}ae(jx=="no block" ? 3 : 2," "+gV(jh.aW,14).PadRight(14)+" "+nx.PadRight(9)+" "+jx);
}ae(1,"perf");ae(2," tick "+dr+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");ae(2," instr "+eN+"  peak "+g_+"  max "+Runtime.MaxInstructionCount);
ae(2," rate "+Runtime.UpdateFrequency+"  idle "+dp+"  up "+(int)aq+"s");if(g2.Length>0)ae(2," last "+g2);var np=new StringBuilder();
foreach(var qM in ci)np.AppendLine(qM.Key);kM=np.ToString();}static Color ou(int r8){switch(r8){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);
case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);}return new Color(170,180,195);}void oy(){Vector2 b4=bj.SurfaceSize;
Vector2 iM=(bj.TextureSize-b4)/2f;int hA=30;foreach(var qN in ci)hA=Math.Max(hA,qN.Key.Length);Vector2 hz=bj.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float nq=Math.Min(1f,Math.Min(b4.X*0.94f/(hA*hz.X),b4.Y*0.94f/(ci.Count*hz.Y)));float sx=iM.X+b4.X*0.03f;float oc=iM.Y+b4.Y*0.03f;var jt=new List<MySprite>();
jt.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iM+b4/2f,Size=b4,Color=new Color(12,12,16)});foreach(var mA in ci){cT(jt,mA.Key,ou(mA.Value),sx,oc,nq);
oc+=hz.Y*nq;}using(var qm=bj.DrawFrame()){qm.AddRange(jt);}}static readonly List<string>om=new List<string>{"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void kz(){aY.Clear();if(r==null)return;for(int fj=0;fj<r.T.Count;fj++){var fS=r.T[fj];if(fS.gs>0f)aY[fS]=aq+(fj/Math.Max(1,fS.jY))*fS.j0/60.0;
}}void o8(){List<y>dD=null;int qx=(aY.Count+3)/4,qF=0;foreach(var fs in aY){double iU=(aq-fs.Value)/fs.Key.gs;if(iU<0)continue;
if(iU>=1.0){if(dD==null)dD=new List<y>();dD.Add(fs.Key);continue;}if((qF++ +dr)%qx==0)ox(fs.Key,iU);}if(dD==null)return;foreach(var d9 in dD){
aY.Remove(d9);if(d9.gr>0f)eL[d9]=aq;eO.Remove(d9);cj.Remove(d9);}}static void cT(List<MySprite>r_,string sf,Color pZ,float sy,float sB,float rT){
var nM=MySprite.CreateText(sf,"Monospace",pZ,rT,TextAlignment.LEFT);nM.Position=new Vector2(sy,sB);r_.Add(nM);}static void jT(List<MySprite>r0,List<string>eU,y fV,float sz,float sC,float rU,float qY){
string[]r5=fV.er!=null&&fV.er.Length>0 ? fV.er : new[]{fV.jZ};for(int dK=0;dK<eU.Count;
dK++){n hu=D.kd(r5,eU.Count>1 ? 100.0*dK/(eU.Count-1): 0.0);cT(r0,eU[dK],new Color(hu.bf,hu.ba,hu.a5),sz,sC+dK*qY,rU);}}bool o6(y cJ,IMyTextSurface f1){
List<string>dv;if(cj.ContainsKey(cJ)|| !r.cV.TryGetValue(cJ.bE,out dv))return false;cj[cJ]="banner";Vector2 b5=f1.SurfaceSize;
Vector2 iN=(f1.TextureSize-b5)/2f;int e3=1;foreach(var qO in dv)e3=Math.Max(e3,qO.Length);Vector2 dB=f1.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float fU=Math.Min(b5.X*0.94f/(e3*dB.X),b5.Y*0.94f/(dv.Count*dB.Y));var ju=new List<MySprite>();ju.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iN+b5/2f,Size=b5,Color=string.IsNullOrEmpty(cJ.a6)? new Color(10,12,16): E(cJ.a6)}
);jT(ju,dv,cJ,iN.X+(b5.X-e3*dB.X*fU)/2f,iN.Y+(b5.Y-dv.Count*dB.Y*fU)/2f,fU,dB.Y*fU);using(var qn=f1.DrawFrame()){qn.AddRange(ju);
}return true;}void ox(y cK,double mZ){IMyTextSurface eb;if(!bV.TryGetValue(cK,out eb))return;Vector2 bA=eb.SurfaceSize;
Vector2 iO=(eb.TextureSize-bA)/2f;var dV=cK.oF();string mN=cK.j_ ?? "HELM TELEMETRY KERNEL ONLINE";bool iG=bA.Y>=200f;
List<string>cm;if(cK.bH==null|| !r.cV.TryGetValue(cK.bH,out cm))cm=om;int ct=mN.Length+3;foreach(var qP in dV)ct=Math.Max(ct,qP.Length+5);
if(iG)foreach(var qQ in cm)ct=Math.Max(ct,qQ.Length);int nh=(iG ? cm.Count+1 : 0)+2+dV.Count;Vector2 cs=eb.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bw=Math.Min(1f,Math.Min(bA.X*0.94f/(ct*cs.X),bA.Y*0.94f/(nh*cs.Y)));float gh=iO.X+(bA.X-ct*cs.X*bw)/2f;float b8=iO.Y+(bA.Y-nh*cs.Y*bw)/2f;
float fw=cs.Y*bw;var cM=new List<MySprite>();cM.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iO+bA/2f,Size=bA,Color=new Color(10,12,16)}
);if(iG){jT(cM,cm,cK,gh,b8,bw,fw);b8+=(cm.Count+1)*fw;}cT(cM,">> "+mN,new Color(34,197,94),gh,b8,bw);b8+=fw*2f;int rX=D.ol(mZ,dV.Count);
for(int fk=0;fk<rX;fk++){bool mW=fk==dV.Count-1&&mZ>=0.8;cT(cM,mW ? "[OK]" : "[..]",mW ? new Color(34,197,94): new Color(56,189,248),gh,b8,bw);cT(cM,dV[fk],new Color(203,213,225),gh+5f*cs.X*bw,b8,bw);
b8+=fw;}using(var qo=eb.DrawFrame()){qo.AddRange(cM);}}void gQ(){bU.Clear();bV.Clear();eM.Clear();
dq.Clear();eK=false;if(r==null)return;foreach(var U in r.bG){var aF=new List<IMyTerminalBlock>();if(U.bc==aI.c5){
string iq= !string.IsNullOrEmpty(U.aV)? U.aV : U.V;if(!ax.ContainsKey(iq)){ax[iq]=U.ev;if(U.gv!=null)dm[iq]=U.gv;
}bU[U.V]=aF;continue;}var nP=U.aE.Count>0 ? U.aE : new List<string>{U.kD};foreach(var b7 in nP){
if(string.IsNullOrEmpty(b7))continue;bool mt=b7.Contains("*")||b7.Contains("?");if(U.bc==aI.gF||mt){var h4=new List<IMyTerminalBlock>();
oq(b7,h4,U.en);if(h4.Count>0){foreach(var md in h4){if(!aF.Contains(md))aF.Add(md);}}else{var kU=em(U.en);
for(int hj=0;hj<kU.Count;hj++){var f9=kU[hj];if(mt ? D.oP(b7,f9.CustomName): string.Equals(b7,f9.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aF.Contains(f9))aF.Add(f9);}}}}else{var hn=j7(b7,U.en);if(hn!=null&& !aF.Contains(hn))aF.Add(hn);}}if(aF.Count==0){
ph(U,nP,aF);if(aF.Count>0)eM[U.V]=true;}bU[U.V]=aF;}foreach(var a2 in r.T){
if(string.IsNullOrEmpty(a2.be))continue;var iR=j7(a2.be);if(iR==null){dq.AppendLine($"HELM: output block '{a2.be}' not found");
continue;}IMyTextSurface b6=null;var i9=iR as IMyTextSurfaceProvider;var lG=iR as IMyTextSurface;if(i9!=null&&a2.bL<i9.SurfaceCount){
b6=i9.GetSurface(a2.bL);}else if(lG!=null&&a2.bL==0){b6=lG;}if(b6==null){dq.AppendLine($"HELM: output block '{a2.be}' has no surface index {a2.bL}");
continue;}b6.ContentType=ContentType.SCRIPT;b6.Script="";b6.ScriptBackgroundColor=Color.Black;bV[a2]=b6;}bS.Clear();
ck.Clear();}static readonly string[]j6={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool oD(int qG,IMyTerminalBlock s){switch(qG){case 0: return s is IMyBatteryBlock;case 1: return s is IMyReactor;case 2: return s is IMySolarPanel;
case 3: return s is IMyWindTurbine;case 4: return s is IMyGasTank;case 5: return s is IMyAirVent;case 6: return s is IMyCargoContainer;case 7: return s is IMyJumpDrive;
case 8: return s is IMyThrust;case 9: return s is IMyMotorSuspension;case 10: return s is IMyDoor;case 11: return s is IMyShipConnector;case 12: return s is IMyAssembler;
case 13: return s is IMyRefinery;case 14: return s is IMyShipDrill;case 15: return s is IMyShipWelder;case 16: return s is IMyShipGrinder;case 17: return s is IMyGasGenerator;
case 18: return s is IMyLargeTurretBase;case 19: return s is IMyProjector;case 20: return s is IMyGyro;case 21: return s is IMyGravityGeneratorBase;case 22: return s is IMyLandingGear;
case 23: return s is IMyRadioAntenna;case 24: return s is IMyBeacon;case 25: return s is IMyLightingBlock;case 26: return s is IMyPistonBase;case 27: return s is IMyMotorStator;
case 28: return s is IMyWarhead;case 29: return s is IMySensorBlock;case 30: return s is IMyCameraBlock;case 31: return s is IMyParachute;case 32: return s is IMyTurretControlBlock;
}return false;}void ph(aU pQ,List<string>sc,List<IMyTerminalBlock>ho){string qA=(pQ.V+" "+string.Join(" ",sc)).ToLowerInvariant();
var iv=new List<int>();for(int fr=0;fr<j6.Length;fr++){foreach(var sv in j6[fr].Split(','))if(qA.Contains(sv)){iv.Add(fr);break;}}if(iv.Count==0)return;
var kT=em();var mX=new List<IMyTerminalBlock>();for(int ib=0;ib<kT.Count;ib++){var hk=kT[ib];foreach(int qH in iv){if(oD(qH,hk)){(hk.CubeGrid==Me.CubeGrid ? ho : mX).Add(hk);
break;}}}if(ho.Count==0)ho.AddRange(mX);}void pj(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(r==null||r.T.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {r.T.Count} screens, {r.bG.Count} binds");eK=false;
Echo($"Blocks: {em().Count} on this construct; {gZ} on docked ships (ignored)");bS.Clear();Echo("--- BIND RESOLUTION ---");foreach(var ah in r.bG){
if(ah.bc==aI.c5){string ir= !string.IsNullOrEmpty(ah.aV)? ah.aV : ah.V;double sm=ax.ContainsKey(ir)? ax[ir]: ah.ev;
Echo($"[OK] @bind {ah.V} (dynamic variable '{ir}' = {sm})");continue;}List<IMyTerminalBlock>hp;if(bU.TryGetValue(ah.V,out hp)){if(hp.Count>0){
string qi=eM.ContainsKey(ah.V)&&eM[ah.V]? " [fallback: typed match]" : "";Echo($"[OK] @bind {ah.V} ({hp.Count} blocks matched{qi})");
}else{string sd=string.Join(", ",ah.aE.Count>0 ? ah.aE : new List<string>{ah.kD});Echo($"[WARN] @bind {ah.V} -> 0 blocks matched \"{sd}\"");
}}else{Echo($"[ERR] @bind {ah.V} -> unresolved");}}if(ax.Count>0){Echo($"--- DYNAMIC VARIABLES ({ax.Count}) ---");foreach(var mC in ax)Echo($"[VAR] {mC.Key} = {mC.Value}");
}IMyTextSurface hb=null;foreach(var rc in bV.Values){hb=rc;break;}if(hb!=null){var jv=new List<string>();hb.GetSprites(jv);
Echo($"--- SPRITES ({jv.Count}): names usable in bg_texture ---");Echo(string.Join(", ",jv.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var a3 in r.T){IMyTextSurface f2;if(bV.TryGetValue(a3,out f2)){var ny=f2.SurfaceSize;var r3=D.ot(a3);
Echo($"[OK] Screen \"{a3.aW}\" [{a3.bM}] -> {a3.be} [#{a3.bL}] ({ny.X:0}x{ny.Y:0} px, texture {f2.TextureSize.X:0}x{f2.TextureSize.Y:0}, {r3.W} sprites)");
}else{Echo($"[ERR] Screen \"{a3.aW}\" -> Missing block \"{a3.be}\" [#{a3.bL}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double pi(IMyTerminalBlock t){
if(t==null)return 0.0;var eX=t as IMyBatteryBlock;if(eX!=null)return eX.MaxStoredPower>0 ?(eX.CurrentStoredPower/eX.MaxStoredPower)*100.0 : 0.0;
var fq=t as IMyJumpDrive;if(fq!=null)return fq.MaxStoredPower>0 ?(fq.CurrentStoredPower/fq.MaxStoredPower)*100.0 : 0.0;var nO=t as IMyGasTank;
if(nO!=null)return nO.FilledRatio*100.0;var n6=t as IMyAirVent;if(n6!=null)return n6.GetOxygenLevel()*100.0;var lm=t as IMyCargoContainer;if(lm!=null){
var fn=lm.GetInventory();if(fn!=null&&fn.MaxVolume.RawValue>0)return(double)fn.CurrentVolume.RawValue/fn.MaxVolume.RawValue*100.0;return 0.0;}var fN=t as IMyReactor;
if(fN!=null)return fN.MaxOutput>0 ?(fN.CurrentOutput/fN.MaxOutput)*100.0 : 0.0;var fW=t as IMySolarPanel;if(fW!=null)return fW.MaxOutput>0 ?(fW.CurrentOutput/fW.MaxOutput)*100.0 : 0.0;
var gg=t as IMyWindTurbine;if(gg!=null)return gg.MaxOutput>0 ?(gg.CurrentOutput/gg.MaxOutput)*100.0 : 0.0;var lH=t as IMyDoor;if(lH!=null)return(double)lH.OpenRatio*100.0;
var gc=t as IMyThrust;if(gc!=null)return gc.MaxEffectiveThrust>0 ?(gc.CurrentThrust/gc.MaxEffectiveThrust)*100.0 : 0.0;var jE=t as IMyMotorSuspension;
if(jE!=null)return jE.Brake ? 100.0 :(jE.Friction*100.0);var hE=t as IMyShipConnector;if(hE!=null)return hE.Status==MyShipConnectorStatus.Connected ? 100.0 :(hE.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var nb=t as IMyProductionBlock;if(nb!=null)return nb.IsProducing ? 100.0 : 0.0;var lL=t as IMyShipDrill;if(lL!=null)return lL.Enabled ? 100.0 : 0.0;
var jJ=t as IMyShipToolBase;if(jJ!=null)return jJ.IsActivated ? 100.0 :(jJ.Enabled ? 50.0 : 0.0);var h1=t as IMyGasGenerator;if(h1!=null)return h1.AutoRefill ? 100.0 :(h1.Enabled ? 50.0 : 0.0);
var gd=t as IMyLargeTurretBase;if(gd!=null)return gd.IsShooting ? 100.0 :(gd.HasTarget ? 50.0 :(gd.Enabled ? 10.0 : 0.0));var d3=t as IMyProjector;
if(d3!=null)return d3.TotalBlocks>0 ?((double)(d3.TotalBlocks-d3.RemainingBlocks)/d3.TotalBlocks)*100.0 : 0.0;var h7=t as IMyGyro;if(h7!=null)return h7.GyroOverride ? 100.0 :(h7.Enabled ? 50.0 : 0.0);
var mf=t as IMyGravityGeneratorBase;if(mf!=null)return Math.Min(100.0,Math.Abs(mf.GravityAcceleration)/9.81*100.0);var h0=t as IMyLandingGear;if(h0!=null)return h0.IsLocked ? 100.0 :(h0.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var kV=t as IMyRadioAntenna;if(kV!=null)return(kV.Radius/50000.0)*100.0;var k6=t as IMyBeacon;if(k6!=null)return(k6.Radius/50000.0)*100.0;
var mI=t as IMyLightingBlock;if(mI!=null)return mI.Intensity*10.0;var fK=t as IMyPistonBase;if(fK!=null)return fK.HighestPosition>0 ?(fK.CurrentPosition/fK.HighestPosition)*100.0 : 0.0;
var nf=t as IMyMotorStator;if(nf!=null)return Math.Min(100.0,Math.Abs(nf.Angle)/6.28318*100.0);var n8=t as IMyWarhead;if(n8!=null)return n8.IsArmed ? 100.0 : 0.0;
var jq=t as IMySensorBlock;if(jq!=null)return!jq.LastDetectedEntity.IsEmpty()? 100.0 :(jq.Enabled ? 25.0 : 0.0);var hx=t as IMyCameraBlock;
if(hx!=null)return hx.IsActive ? 100.0 :(hx.Enabled ? 50.0 : 0.0);var ls=t as IMyParachute;if(ls!=null)return(double)ls.OpenRatio*100.0;
var gb=t as IMyTurretControlBlock;if(gb!=null)return gb.HasTarget ? 100.0 :(gb.IsUnderControl ? 50.0 :(gb.Enabled ? 10.0 : 0.0));var l4=t as IMyFunctionalBlock;
if(l4!=null)return l4.Enabled ? 100.0 : 0.0;return 0.0;}static double oM(IMyTerminalBlock k2,string hT){int ll=hT.IndexOf(':');string iu=hT.Substring(0,ll).ToLowerInvariant();
var sk=new MyItemType("MyObjectBuilder_"+(iu=="ammo" ? "AmmoMagazine" : char.ToUpper(iu[0])+iu.Substring(1)),hT.Substring(ll+1));double nH=0;for(int ic=0;
ic<k2.InventoryCount;ic++)nH+=(double)k2.GetInventory(ic).GetItemAmount(sk);return nH;}double kv(IMyTerminalBlock u,string lX){if(u==null)return 0.0;
string b=lX.ToLowerInvariant();if(b=="percent"||b=="pct")return pi(u);if(b.IndexOf(':')>0)return oM(u,lX);var bn=u as IMyBatteryBlock;
if(bn!=null){if(b=="current"||b=="stored"||b=="val"||b=="value"||b=="raw")return bn.CurrentStoredPower;if(b=="max"||b=="capacity")return bn.MaxStoredPower;
if(b=="output")return bn.CurrentOutput;if(b=="maxoutput")return bn.MaxOutput;if(b=="input")return bn.CurrentInput;if(b=="maxinput")return bn.MaxInput;
return bn.CurrentStoredPower;}var ip=u as IMyJumpDrive;if(ip!=null){if(b=="max"||b=="capacity")return ip.MaxStoredPower;return ip.CurrentStoredPower;
}var f6=u as IMyGasTank;if(f6!=null){if(b=="max"||b=="capacity")return f6.Capacity;return f6.FilledRatio*f6.Capacity;}var n7=u as IMyAirVent;
if(n7!=null){if(b=="max"||b=="capacity")return 100.0;return n7.GetOxygenLevel()*100.0;}var ln=u as IMyCargoContainer;if(ln!=null){var fo=ln.GetInventory();
if(fo!=null){if(b=="mass")return(double)fo.CurrentMass;if(b=="max"||b=="capacity")return(double)fo.MaxVolume;return(double)fo.CurrentVolume;}return 0.0;}var ja=u as IMyPowerProducer;
if(ja!=null){if(b=="max"||b=="capacity")return ja.MaxOutput;return ja.CurrentOutput;}var lI=u as IMyDoor;if(lI!=null){if(b=="max")return 1.0;return(double)lI.OpenRatio*100.0;
}var jH=u as IMyThrust;if(jH!=null){if(b=="max"||b=="capacity")return jH.MaxEffectiveThrust;return jH.CurrentThrust;}var bC=u as IMyMotorSuspension;
if(bC!=null){if(b=="brake")return bC.Brake ? 100.0 : 0.0;if(b=="height")return bC.Height;if(b=="strength")return bC.Strength;if(b=="power")return bC.Power;
if(b=="steer")return bC.SteeringOverride*100.0;if(b=="propulsion")return bC.PropulsionOverride*100.0;return bC.Friction;}var ly=u as IMyShipConnector;
if(ly!=null){return ly.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var i7=u as IMyProductionBlock;if(i7!=null){if(b=="producing"||b=="active"||b=="running"||b=="state")return i7.IsProducing ? 100.0 : 0.0;
if(b=="queue")return i7.IsQueueEmpty ? 0.0 : 1.0;}var hf=u as IMyAssembler;if(hf!=null){if(b=="disassembling")return hf.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(b=="repeating"||b=="loop")return hf.Repeating ? 100.0 : 0.0;}var h2=u as IMyGasGenerator;if(h2!=null){if(b=="autorefill")return h2.AutoRefill ? 100.0 : 0.0;
if(b=="active")return h2.Enabled ? 100.0 : 0.0;}var ge=u as IMyLargeTurretBase;if(ge!=null){if(b=="shooting")return ge.IsShooting ? 100.0 : 0.0;
if(b=="target"||b=="hastarget")return ge.HasTarget ? 100.0 : 0.0;if(b=="range")return ge.Range;}var bv=u as IMyProjector;if(bv!=null){if(b=="built")return bv.TotalBlocks-bv.RemainingBlocks;
if(b=="remaining")return bv.RemainingBlocks;if(b=="total")return bv.TotalBlocks;if(b=="projecting")return bv.IsProjecting ? 100.0 : 0.0;if(b=="armor")return bv.RemainingArmorBlocks;
if(b=="buildable")return bv.BuildableBlocksCount;}var jK=u as IMyShipToolBase;if(jK!=null){if(b=="working"||b=="active"||b=="activated")return jK.IsActivated ? 100.0 : 0.0;
return jK.Enabled ? 100.0 : 0.0;}var cx=u as IMyGyro;if(cx!=null){if(b=="override")return cx.GyroOverride ? 100.0 : 0.0;if(b=="pitch")return cx.Pitch;
if(b=="yaw")return cx.Yaw;if(b=="roll")return cx.Roll;if(b=="power")return cx.GyroPower*100.0;}var mg=u as IMyGravityGeneratorBase;if(mg!=null){if(b=="g"||b=="accel")return mg.GravityAcceleration;
}var me=u as IMyLandingGear;if(me!=null){if(b=="locked")return me.IsLocked ? 100.0 : 0.0;}var kW=u as IMyRadioAntenna;if(kW!=null){if(b=="radius"||b=="range")return kW.Radius;
}var k7=u as IMyBeacon;if(k7!=null){if(b=="radius"||b=="range")return k7.Radius;}var iC=u as IMyLightingBlock;if(iC!=null){if(b=="intensity")return iC.Intensity;
if(b=="radius")return iC.Radius;}var fL=u as IMyPistonBase;if(fL!=null){if(b=="pos"||b=="position"||b=="extension")return fL.CurrentPosition;
if(b=="velocity")return fL.Velocity;if(b=="max")return fL.HighestPosition;}var d8=u as IMyMotorStator;if(d8!=null){if(b=="angle"||b=="deg")return d8.Angle*180.0/Math.PI;
if(b=="rad")return d8.Angle;if(b=="velocity"||b=="rpm")return d8.TargetVelocityRPM;if(b=="locked")return d8.RotorLock ? 100.0 : 0.0;}var jS=u as IMyWarhead;
if(jS!=null){if(b=="armed")return jS.IsArmed ? 100.0 : 0.0;if(b=="countdown")return jS.DetonationTime;}var nv=u as IMySensorBlock;if(nv!=null){
if(b=="detected")return!nv.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var hy=u as IMyCameraBlock;if(hy!=null){if(b=="range")return hy.AvailableScanRange;
if(b=="active")return hy.IsActive ? 100.0 : 0.0;}var lt=u as IMyParachute;if(lt!=null){if(b=="open"||b=="openratio")return(double)lt.OpenRatio*100.0;
}var cQ=u as IMyTurretControlBlock;if(cQ!=null){if(b=="target"||b=="hastarget")return cQ.HasTarget ? 100.0 : 0.0;if(b=="azimuth")return cQ.AzimuthRotor!=null ? cQ.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(b=="elevation")return cQ.ElevationRotor!=null ? cQ.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var l5=u as IMyFunctionalBlock;if(l5!=null){return l5.Enabled ? 100.0 : 0.0;
}return 0.0;}double bN(string dy,string lY){aU bo;dk.TryGetValue(dy,out bo);if(bo!=null&&bo.bc==aI.c5){
string qI= !string.IsNullOrEmpty(bo.aV)? bo.aV : dy;double lB;if(ax.TryGetValue(qI,out lB))return lB;return bo.ev;
}double lO;if(ax.TryGetValue(dy,out lO))return lO;List<IMyTerminalBlock>dz;if(!bU.TryGetValue(dy,out dz)||dz.Count==0)throw new InvalidOperationException($"HELM: bind '{dy}' resolved to zero blocks — check the name/typo");
if(dz.Count==1)return kv(dz[0],lY);var n5=new List<double>();foreach(var pF in dz)n5.Add(kv(pF,lY));return oh.og(bo!=null ? bo.cU : av.gk,n5);
}double gR(string pS){return bN(pS,"percent");}static string a9(double n2,bd cD){if(cD==null||(string.IsNullOrEmpty(cD.di)&&string.IsNullOrEmpty(cD.c6)&&cD.F=="percent")){
return kc(n2);}return D.kb(n2,cD.di,cD.c6);}bool pc(y cL){IMyTextSurface f3;if(!bV.TryGetValue(cL,out f3))return false;
if(cL.bE!=null)return o6(cL,f3);if(cL.c0.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return pd(cL,f3);
}return pe(cL,f3);}Color oH(string[]cN,double rj){if(cN.Length==1)return E(cN[0]);double nc=Math.Max(0.0,Math.Min(100.0,rj))/100.0*(cN.Length-1);
int id=Math.Min((int)nc,cN.Length-2);float f4=(float)(nc-id);Color bl=E(cN[id]),eW=E(cN[id+1]);return new Color((int)(bl.R+(eW.R-bl.R)*f4),(int)(bl.G+(eW.G-bl.G)*f4),(int)(bl.B+(eW.B-bl.B)*f4),(int)(bl.A+(eW.A-bl.A)*f4));
}static Color[]ej(aX pX,string[]r6,string p9,float ob,float od,float jQ,float h8){int dw=16;bool mn=p9=="horizontal";var hD=new Color[dw];
for(int b_=0;b_<dw;b_++){n h_=D.kd(r6,100.0*b_/(dw-1));hD[b_]=new Color(h_.bf,h_.ba,h_.a5);float lk=jQ/dw,ld=h8/dw;x(pX,mn ? new Vector2(ob+lk*(b_+0.5f),od+h8/2f): new Vector2(ob+jQ/2f,od+ld*(b_+0.5f)),mn ? new Vector2(lk+1f,h8): new Vector2(jQ,ld+1f),hD[b_]);
}return hD;}static void x(aX pY,Vector2 rs,Vector2 rY,Color p_,float rK=0f,string p8="SquareSimple"){pY.l.Add(new MySprite{Type=SpriteType.TEXTURE,Data=p8,Position=rs,Size=rY,Color=p_,RotationOrScale=rK}
);}void op(y j,IMyTextSurface nI,aX f){f.l.Clear();f.eH.Clear();Vector2 K=nI.SurfaceSize;
Vector2 sh=nI.TextureSize;Vector2 au=(sh-K)/2f;float fD=Math.Max(4f,K.X*0.02f);float b0=Math.Max(4f,K.Y*0.02f);
if(j.bM=="compact"||(j.bM=="auto"&&K.Y<=260f&&K.Y>150f)){fD=4f;b0=4f;}else if(j.bM=="corner"||(j.bM=="auto"&&K.Y<=150f)){
fD=2f;b0=2f;}float eg=K.X-(fD*2f);Color co=new Color(10,12,16);bool lK=true;if(!string.IsNullOrEmpty(j.a6)){
var dJ=n.dd(j.a6);if(dJ.eA)lK=false;else co=new Color(dJ.bf,dJ.ba,dJ.a5,dJ.gi);}f.bF= -1;f.el=false;
f.ek=E(j.jV ?? "#5b0f0f");f.c_.Clear();if(j.a8!=null&&j.a8.Length>1){f.bF=f.l.Count;f.ep=ej(f,j.a8,j.cZ,au.X+2f,au.Y+2f,K.X-4f,K.Y-4f);
}else if(lK||j.az!=null||j.cd!=null){if(j.az!=null)co=E(j.az.First(rx=>rx.cg).bJ);f.bF=f.l.Count;
f.ep=new[]{co};x(f,au+(K/2f),K-new Vector2(4f,4f),co);if(j.az!=null||j.cd!=null)f.c_.Add(new eq{
ez=f.bF,gl=co,eu=co,eG=j.az,gp=j.gm,gy=j.gn,ex=j.cd,gz=j.gA,gB=j.gC,kj=true}
);}if(!string.IsNullOrEmpty(j.go))x(f,au+(K/2f),K-new Vector2(4f,4f),E(j.jX ?? "255,255,255,24"),0f,j.go);
if(!j.c0.Equals("none",StringComparison.OrdinalIgnoreCase)){Color lf=j.an!=null&&j.an.Length>0 ? E(j.an[0]): E(j.et ?? "40, 60, 85");
Color pW=j.an!=null&&j.an.Length>1 ? E(j.an[j.an.Length-1]): lf;
float lg=au.X+(K.X-eg)/2f;if(j.an!=null&&j.an.Length>1&& !j.gE){ej(f,j.an,"horizontal",lg,au.Y+b0*0.5f-1f,eg,2f);
ej(f,j.an,"horizontal",lg,au.Y+K.Y-(b0*0.5f)-1f,eg,2f);}else{x(f,new Vector2(au.X+K.X/2f,au.Y+b0*0.5f),new Vector2(eg,2f),lf);
x(f,new Vector2(au.X+K.X/2f,au.Y+K.Y-(b0*0.5f)),new Vector2(eg,2f),pW);}}var iX=j.ab;
if(iX.Count==0){iX=new List<aw>{new aw{dc="main",c2=0,de=0,df=j.c7,dg=j.c8,aL=j.eC}};}float mi=8f;
foreach(var v in iX){var fC=D.os(v.c2,v.de,v.df,v.dg,j.c7,j.c8,K.X,K.Y,fD,b0,mi,mi);
float aO=au.X+fC.kJ;float d0=au.Y+fC.kK;float a0=fC.kI;float fB=fC.kf;bool ni=v.az!=null||v.cd!=null;
if(v.a8!=null&&v.a8.Length>1){ej(f,v.a8,v.cZ!="vertical" ? v.cZ : j.cZ,aO,d0,a0,fB);
}else{Color bu;if(v.az!=null)bu=E(v.az.First(ry=>ry.cg).bJ);else if(!string.IsNullOrEmpty(v.ca))bu=E(v.ca);
else if(!string.IsNullOrEmpty(j.gM))bu=E(j.gM);else bu=Color.Transparent;if(bu.A>0||ni){if(ni)f.c_.Add(new eq{ez=f.l.Count,gl=bu,eu=bu,eG=v.az,gp=v.gm,gy=v.gn,ex=v.cd,gz=v.gA,gB=v.gC}
);x(f,new Vector2(aO+a0/2f,d0+fB/2f),new Vector2(a0,fB),bu);}}string hZ=v.j8 ?? j.j9 ?? "Monospace";float pO=D.oG(j.bM,K.X,K.Y,j.ce);
float aG=v.ka ?? pO;float fg=0f;if(!string.IsNullOrEmpty(v.bQ)){fg=20f*aG;var g9=TextAlignment.LEFT;float jI=aO+4f;
string kP=v.gT ?? j.kE ?? "left";if(kP.Equals("center",StringComparison.OrdinalIgnoreCase)){g9=TextAlignment.CENTER;jI=aO+(a0/2f);
}else if(kP.Equals("right",StringComparison.OrdinalIgnoreCase)){g9=TextAlignment.RIGHT;jI=aO+a0-4f;}string nR=v.gU ?? j.kF;
Color si= !string.IsNullOrEmpty(nR)? E(nR): E(v.gt ?? j.et ?? "white");var nW=MySprite.CreateText(v.bQ,hZ,si,aG*0.9f,g9);
nW.Position=new Vector2(jI,d0+2f);f.l.Add(nW);string[]hl=v.gu ?? j.an;Color qb=hl!=null&&hl.Length>0 ? E(hl[0]): E(v.gt ?? j.et ?? "40, 60, 85");
x(f,new Vector2(aO+a0/2f,d0+fg-2f),new Vector2(a0-8f,1f),qb);}int q2=Math.Max(1,v.aL.Count);float rh=fB-fg-8f;
float je=rh/q2;float fm=6f;for(int fl=0;fl<v.aL.Count;fl++){var z=v.aL[fl];var P=new db{kn=z};float rL=d0+fg+4f+(fl*je);
float J=rL+(je/2f);float nT=J-(11f*aG);P.gH=f.l.Count;P.ke=z.S!=null&&(z.S.Contains("{val}")||z.S.Contains("{percent}"));
var mF=MySprite.CreateText(z.S,hZ,Color.White,aG,TextAlignment.LEFT);mF.Position=new Vector2(aO+fm,nT);f.l.Add(mF);
float qW=Math.Max(24f*aG,(z.S.Length+1)*12f*aG);float st=(!string.IsNullOrEmpty(z.di)|| !string.IsNullOrEmpty(z.c6))? 64f*aG : 52f*aG;
if(z.g!=e.gS){float I=aO+fm+qW;float pN=aO+a0-fm-st;float H=Math.Max(16f,pN-I);
float O=Math.Max(z.g==e.bO ? 8f : 4f,Math.Min((z.g==e.bO ? 20f : 16f)*aG,je*(z.g==e.bO ? 0.55f : 0.45f)));
P.cY=I;P.eo=H;P.cc=O;P.cX=J;if(z.g==e.cW){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(25,30,38));
P.aa=f.l.Count;x(f,new Vector2(I,J),new Vector2(0f,O),E(z.ao));}else if(z.g==e.da){
int jo=10;float l7=2f;float jp=Math.Max(2f,(H-(l7*(jo-1)))/jo);for(int ji=0;ji<jo;ji++){P.af.Add(f.l.Count);
x(f,new Vector2(I+ji*(jp+l7)+(jp/2f),J),new Vector2(jp,O),new Color(22,26,32));}}else if(z.g==e.bO){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));
int i6=8;float l8=2f;float hB=Math.Max(2f,(H-(l8*(i6-1)))/i6);for(int iV=0;iV<i6;iV++){P.af.Add(f.l.Count);
x(f,new Vector2(I+iV*(hB+l8)+(hB/2f),J),new Vector2(hB,2f),E(z.ao));}}else if(z.g==e.c9){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));
int jy=10;float l9=2f;float jz=Math.Max(2f,(H-(l9*(jy-1)))/jy);for(int jj=0;jj<jy;jj++){P.af.Add(f.l.Count);
x(f,new Vector2(I+jj*(jz+l9)+(jz/2f),J),new Vector2(jz,O),new Color(24,28,36),0.523599f);}}else if(z.g==e.c1){
int hd=10;float ma=2f;float he=Math.Max(2f,(H-(ma*(hd-1)))/hd);for(int jk=0;jk<hd;jk++){P.af.Add(f.l.Count);
x(f,new Vector2(I+jk*(he+ma)+(he/2f),J),new Vector2(he,O),new Color(24,28,36),1.570796f,"Triangle");}}else if(z.g==e.c4){
int hH=12;float mb=2f;float hJ=Math.Max(2f,(H-(mb*(hH-1)))/hH);float lJ=Math.Min(hJ,O);for(int jl=0;jl<hH;jl++){P.af.Add(f.l.Count);
x(f,new Vector2(I+jl*(hJ+mb)+(hJ/2f),J),new Vector2(lJ,lJ),new Color(24,28,36),0f,"Circle");}}else if(z.g==e.cb){
x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(25,30,38));P.aa=f.l.Count;x(f,new Vector2(I+(H/2f),J),new Vector2(0f,O),E(z.ao));
x(f,new Vector2(I+(H/2f),J),new Vector2(2f,O+4f),new Color(80,110,150));}else if(z.g==e.bR){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));
P.aa=f.l.Count;float iw=Math.Max(4f,O);x(f,new Vector2(I+iw*0.5f,J),new Vector2(iw,iw),new Color(40,50,65),0f,"Circle");
}else if(z.g==e.bI){float cp=Math.Max(6f,Math.Min(O,14f*aG));x(f,new Vector2(I+cp*0.5f,J),new Vector2(cp,cp),new Color(28,34,46));
P.aa=f.l.Count;x(f,new Vector2(I+cp*0.5f,J),new Vector2(cp*0.6f,cp*0.6f),new Color(40,50,65));}else if(z.g==e.bP){
x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(18,22,30));P.aa=f.l.Count;float i3=Math.Max(4f,O*0.7f);
x(f,new Vector2(I+i3+2f,J),new Vector2(i3,i3),new Color(34,197,94),0f,"Circle");}else if(z.g==e.b9){int i2=8;
float mc=2f;float i4=Math.Max(2f,(H-(mc*(i2-1)))/i2);for(int iW=0;iW<i2;iW++){P.af.Add(f.l.Count);x(f,new Vector2(I+iW*(i4+mc)+(i4/2f),J),new Vector2(i4,O),new Color(24,28,36));
}}else if(z.g==e.bK){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));P.aa=f.l.Count;
float mq=Math.Max(4f,O);x(f,new Vector2(I+(H/2f),J),new Vector2(mq,mq),E(z.ao),0f,"Triangle");}}var n0=MySprite.CreateText("---",hZ,Color.White,aG,TextAlignment.RIGHT);
n0.Position=new Vector2(aO+a0-fm,nT);P.bi=f.l.Count;f.l.Add(n0);f.eH.Add(P);}}f.kl=K;
f.ki=true;}static void aM(aX li,db nz,string sg,Color p0){var jO=li.l[nz.bi];jO.Data=sg;jO.Color=p0;li.l[nz.bi]=jO;
}static void kx(aX lj,db nA,Color p1){var lS=lj.l[nA.aa];lS.Color=p1;lj.l[nA.aa]=lS;}bool pe(y a4,IMyTextSurface jB){
aX h;if(!eO.TryGetValue(a4,out h)){h=new aX();eO[a4]=h;}if(!h.ki||h.kl!=jB.SurfaceSize){
op(a4,jB,h);}bool M=false;bool eS=(aq%a4.gj)<a4.gj/2.0;double bZ= -1.0,lz;if(eL.TryGetValue(a4,out lz)){
bZ=(aq-lz)/a4.gr;if(bZ>=1.0){eL.Remove(a4);bZ=1.0;}}bool jR=bT&&eS&&h.ek.A>0;if(h.bF>=0&&jR!=h.el){
h.el=jR;for(int dL=0;dL<h.ep.Length;dL++){var k9=h.l[h.bF+dL];k9.Color=jR ? h.ek : h.ep[dL];h.l[h.bF+dL]=k9;
}M=true;foreach(var pP in h.c_)pP.gD=true;}foreach(var ad in h.c_){if(ad.kj&&h.el)continue;Color dx=ad.gl;try{if(ad.eG!=null)dx=E(D.eF(ad.eG,bN(ad.gp,ad.gy)));
if(ad.ex!=null&&eS&&D.eF(ad.ex,bN(ad.gz,ad.gB))!=null)dx=h.ek;}catch{}if(dx!=ad.eu||ad.gD){
ad.eu=dx;ad.gD=false;var lh=h.l[ad.ez];lh.Color=dx;h.l[ad.ez]=lh;M=true;}}for(int ie=0;ie<h.eH.Count;ie++){var c=h.eH[ie];
var i=c.kn;double w;bool mk=false;double iJ=i.eD;try{w=bN(i.aA,i.F);if(i.ch!=null)iJ=bN(i.ch,i.gJ);
}catch{mk=true;w=0.0;}if(mk){if(c.eB!= -99999.0||(c.bi>=0&&h.l[c.bi].Data!="ERR")){M=true;c.eB= -99999.0;
if(c.bi>=0){var jm=h.l[c.bi];jm.Data="ERR";jm.Color=Color.Red;h.l[c.bi]=jm;}}continue;}double aP=w;
if(iJ>0)w=Math.Min(100.0,aP/iJ*100.0);bool nK=bZ>=0.0&&i.g!=e.gS&&i.g!=e.bR&&i.g!=e.bI&&i.g!=e.bP&&i.g!=e.bK;
if(nK)w=bZ<0.5 ? bZ*200.0 : 100.0+(w-100.0)*(bZ-0.5)*2.0;string p3=i.cf ? D.ey(i.bb,w): i.ao;
Color _=E(p3);bool aZ=(_==Color.Red);if(i.cf&& !a4.gE&&i.bb.Length>1)_=oH(i.bb,w);
if(aZ&&eS){_=new Color(255,75,75);M=true;}bool aH=nK||Math.Abs(aP-c.eB)>0.1;if(aH)c.eB=aP;
if(i.g==e.cW){double lZ=(i.gG||i.F=="percent"||i.F=="pct")? w : gR(i.aA);if(c.Q<0f)c.Q=(float)lZ;
float f7=(float)lZ;if(Math.Abs(c.Q-f7)>0.05f){c.Q+=(f7-c.Q)*0.35f;M=true;}else if(c.Q!=f7){
c.Q=f7;M=true;}if(aH)M=true;if(M){float rk=(float)Math.Max(0.0,Math.Min(1.0,c.Q/100.0));float l0=c.eo*rk;
var fa=h.l[c.aa];fa.Size=new Vector2(l0,c.cc);fa.Position=new Vector2(c.cY+(l0/2f),c.cX);
fa.Color=_;h.l[c.aa]=fa;aM(h,c,a9(aP,i),_);}}else if(i.g==e.da||i.g==e.c9||i.g==e.c1||i.g==e.c4){
if(aH||aZ){M=true;double qk=(i.gG||i.F=="percent"||i.F=="pct")? w : gR(i.aA);int nt=c.af.Count;
for(int ea=0;ea<nt;ea++){bool q4=qk>=((ea+0.4)/(double)nt*100.0);Color rV=q4 ? _ : new Color(24,28,36);var nu=h.l[c.af[ea]];
nu.Color=rV;h.l[c.af[ea]]=nu;}aM(h,c,a9(aP,i),_);}}else if(i.g==e.bO){
if(aH||aZ){M=true;for(int ff=0;ff<7;ff++)c.eI[ff]=c.eI[ff+1];c.eI[7]=(float)w;int rr=c.af.Count;
for(int dX=0;dX<rr;dX++){float qz=c.eI[dX];float qy=Math.Max(0.06f,Math.Min(1f,qz/100f));float lv=c.cc*qy;float p2=c.cX+(c.cc/2f)-(lv/2f);
var cu=h.l[c.af[dX]];cu.Size=new Vector2(cu.Size.Value.X,lv);cu.Position=new Vector2(cu.Position.Value.X,p2);
cu.Color=_;h.l[c.af[dX]]=cu;}aM(h,c,a9(aP,i),_);}}else if(i.g==e.cb){
if(c.Q<0f)c.Q=(float)w;float f8=(float)w;if(Math.Abs(c.Q-f8)>0.05f){c.Q+=(f8-c.Q)*0.35f;
M=true;}else if(c.Q!=f8){c.Q=f8;M=true;}if(M||aH){float mj=c.eo*0.5f;float lq=c.cY+mj;
float lF=(float)Math.Max(-1.0,Math.Min(1.0,(c.Q-50.0)/50.0));float hX=Math.Abs(lF)*mj;float ql=(lF>=0)?(lq+hX*0.5f):(lq-hX*0.5f);
var fb=h.l[c.aa];fb.Size=new Vector2(hX,c.cc);fb.Position=new Vector2(ql,c.cX);fb.Color=_;
h.l[c.aa]=fb;aM(h,c,a9(aP,i),_);}}else if(i.g==e.bR){if(aH||aZ){
M=true;bool io=w>=50.0;float mz=Math.Max(4f,c.cc);float qL=io ?(c.cY+c.eo-mz*0.5f):(c.cY+mz*0.5f);
Color my=io ?(_!=Color.White ? _ : new Color(34,197,94)): new Color(60,70,85);var hV=h.l[c.aa];hV.Position=new Vector2(qL,c.cX);
hV.Color=my;h.l[c.aa]=hV;aM(h,c,io ? "ON" : "OFF",my);}}else if(i.g==e.bI){if(aH||aZ){
M=true;bool im=w>=50.0;Color lr=im ?(_!=Color.White ? _ : new Color(56,189,248)): new Color(30,36,48);kx(h,c,lr);
aM(h,c,im ? "ACTIVE" : "READY",im ? lr : new Color(148,163,184));}}else if(i.g==e.bP){if(aH||aZ){
M=true;string r4=w>=75.0 ? "ONLINE" :(w>=25.0 ? "STBY" : "OFFLINE");Color nE=w>=75.0 ? new Color(34,197,94):(w>=25.0 ? new Color(234,179,8): new Color(239,68,68));
kx(h,c,nE);aM(h,c,r4,nE);}}else if(i.g==e.b9){if(aH||aZ){M=true;int m7=c.af.Count;
for(int dY=0;dY<m7;dY++){bool q5=w>=((dY+0.5)/(double)m7*100.0);Color ro=q5 ? _ : new Color(24,28,36);var m8=h.l[c.af[dY]];
m8.Color=ro;h.l[c.af[dY]]=m8;}aM(h,c,a9(aP,i),_);}}else if(i.g==e.bK){
M=true;float fQ;Color dH;string fc;bool qD=(i.F=="producing"||i.F=="active"||i.F=="running"||i.F=="state"||(i.F!="percent"&&(w<=1.0||w==100.0)));
if(qD){if(w>0.0){c.Q=(c.Q+0.3927f)%6.28318f;fQ=c.Q;dH=new Color(34,197,94);fc="RUN";}else{fQ=0f;
dH=new Color(148,163,184);fc="IDLE";}}else{fQ=(w>55.0)? 0f :((w<45.0)? 3.14159f : 1.57079f);dH=(w>55.0)? new Color(34,197,94):((w<45.0)? new Color(239,68,68): new Color(148,163,184));
fc=(w>55.0)? "CHG" :((w<45.0)? "DSC" : "STBY");}var hW=h.l[c.aa];hW.RotationOrScale=fQ;hW.Color=dH;
h.l[c.aa]=hW;aM(h,c,fc,dH);}else{string e8;aU ga;dk.TryGetValue(i.aA,out ga);dm.TryGetValue(kk(i.F)? i.aA+"."+i.F :(ga!=null&&ga.aV!=null ? ga.aV : i.aA),out e8);
if(aH||aZ||e8!=c.km){c.km=e8;M=true;Color jL=i.c3!=null ? E(D.eF(i.c3,w)): Color.White;
if(jL==Color.Red&&eS)jL=new Color(255,75,75);aM(h,c,e8 ?? a9(aP,i),jL);}}if(c.ke&&(aH||aZ)){
M=true;var mH=h.l[c.gH];string rl=kc(w);string rI=a9(aP,i);mH.Data=i.S.Replace("{percent}",rl).Replace("{val}",rI);
h.l[c.gH]=mH;}}if(M){using(var qp=jB.DrawFrame()){qp.AddRange(h.l);}}return M;}bool pd(y bx,IMyTextSurface jC){
jC.FontSize=bx.ce;var e4=new List<KeyValuePair<string,string>>();foreach(var m in bx.eC){double L=0.0;string aS;string aN=null;
double iK=m.eD;try{L=bN(m.aA,m.F);if(m.ch!=null)iK=bN(m.ch,m.gJ);}catch(Exception sD){
e4.Add(new KeyValuePair<string,string>($"{m.S}: [UNRESOLVED]","red"));continue;}double nd=L;if(iK>0)L=Math.Min(100.0,nd/iK*100.0);string n1=a9(nd,m);
if(m.g==e.cW){double l_=(m.gG||m.F=="percent"||m.F=="pct")? L : gR(m.aA);aS=D.o7(m.S,l_,10,n1);
aN=m.cf ? D.ey(m.bb,l_): m.ao;}else if(m.g==e.cb){aS=D.o5(m.S,L,10);
aN=m.cf ? D.ey(m.bb,L): m.ao;}else if(m.g==e.bR){aS=D.pg(m.S,L);
aN=m.ao ??(L>=50.0 ? "green" : "gray");}else if(m.g==e.bI){aS=D.pa(m.S,L);aN=m.ao ??(L>=50.0 ? "cyan" : "gray");
}else if(m.g==e.bP){aS=D.pf(m.S,L);aN=m.ao ??(L>=75.0 ? "lime" :(L>=25.0 ? "yellow" : "red"));
}else if(m.g==e.b9){aS=D.o4(m.S,L,8);aN=m.cf ? D.ey(m.bb,L):(m.ao ?? "yellow");
}else if(m.g==e.bK){bool qE=(m.F=="producing"||m.F=="active"||m.F=="running"||m.F=="state"||(m.F!="percent"&&(L<=1.0||L==100.0)));
if(qE){char[]r2=new[]{'|','/','-','\\'};char rS=r2[Math.Abs(dr)%4];aS=L>0 ? $"{m.S} [{rS}] RUN" : $"{m.S} [==] IDLE";
aN=L>0 ? "green" : "gray";}else{aS=D.pb(m.S,L,dr);aN=m.ao ??(L>55.0 ? "green" :(L<45.0 ? "red" : "cyan"));
}}else{aS=$"{m.S}: {n1}";aN=m.c3!=null ? D.eF(m.c3,L): null;}e4.Add(new KeyValuePair<string,string>(aS,aN));
}var e1=D.o9(new List<string>(e4.ConvertAll(sb=>sb.Key)),cS: bx.kH);string nw=string.Join("\n",e1);
string na;if(cj.TryGetValue(bx,out na)&&na==nw)return false;cj[bx]=nw;using(var qq=jC.DrawFrame()){
float oe=4f;float mJ=of*bx.ce;try{var mL=jC.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bx.ce);
if(mL.Y>0)mJ=mL.Y;}catch{}for(int cy=0;cy<e1.Count;cy++){string lx=null;if(cy>0&&cy<e1.Count-1)lx=e4[cy-1].Value;
var nD=MySprite.CreateText(e1[cy],"Monospace",E(lx),bx.ce,TextAlignment.LEFT);nD.Position=new Vector2(4f,oe);qq.Add(nD);
oe+=mJ;}}return true;}Color E(string fz){if(string.IsNullOrEmpty(fz))return Color.White;Color dS;if(kL.TryGetValue(fz,out dS))return dS;
n dA=n.dd(fz);dS=dA.eA ? Color.Transparent : new Color(dA.bf,dA.ba,dA.a5,dA.gi);kL[fz]=dS;return dS;}public enum aI{oj,gF,c5}
public enum av{oQ,gk,kC,kp,ko}public class aU{public string V;public aI bc;public List<string>aE=new List<string>();public string kD{
get{return aE.Count>0 ? aE[0]: "";}set{if(aE.Count==0)aE.Add(value);else aE[0]=value;}}public av cU=av.oQ;
public double ev=0.0;public string gv;public string aV;public bool en;}public enum e{gS,cW,da,bO,c9,c1,c4,cb,bR,bI,bP,b9,bK}
public class aB{public string gx;public string bJ;public bool cg;}public class bd{public e g;public string S;public string aA;
public string F;public bool cf;public string[]bb;public string ao;public List<aB>c3;public string j3="right";
public string di;public string c6;public double eD;public string ch,gJ;public bool gG{get{return eD>0||ch!=null;}}}public struct n{
public byte bf,ba,a5,gi;public bool eA;public n(byte rz,byte qs,byte pG,byte pp=255){bf=rz;ba=qs;a5=pG;gi=pp;eA=false;}public static readonly n pn=new n(0,0,0,0){
eA=true};public static readonly n eJ=new n(255,255,255);public static readonly n oi=new n(0,0,0);public static readonly n o2=new n(255,0,0);
public static readonly n oJ=new n(0,255,0);public static readonly n oN=new n(50,205,50);public static readonly n po=new n(255,255,0);
public static readonly n oR=new n(255,140,0);public static readonly n ov=new n(0,255,255);public static readonly n ok=new n(0,120,255);
public static readonly n oO=new n(255,0,255);public static readonly n o0=new n(128,0,128);public static readonly n oI=new n(128,128,128);
public static readonly n ow=new n(60,60,60);public static n dd(string aR){if(string.IsNullOrWhiteSpace(aR))return eJ;aR=aR.Trim();
if(aR.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aR.Equals("none",StringComparison.OrdinalIgnoreCase))return pn;if(aR.StartsWith("#")){
string br=aR.Substring(1).Trim();if(br.Length==6){byte rA=Convert.ToByte(br.Substring(0,2),16);byte qt=Convert.ToByte(br.Substring(2,2),16);byte pH=Convert.ToByte(br.Substring(4,2),16);
return new n(rA,qt,pH);}if(br.Length==3){byte rB=Convert.ToByte(new string(br[0],2),16);byte qu=Convert.ToByte(new string(br[1],2),16);byte pI=Convert.ToByte(new string(br[2],2),16);
return new n(rB,qu,pI);}}if(aR.Contains(",")){var cG=aR.Split(',');if(cG.Length>=3){byte rC=(byte)Math.Max(0,Math.Min(255,int.Parse(cG[0].Trim())));
byte qv=(byte)Math.Max(0,Math.Min(255,int.Parse(cG[1].Trim())));byte pJ=(byte)Math.Max(0,Math.Min(255,int.Parse(cG[2].Trim())));byte pq=cG.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(cG[3].Trim()))):(byte)255;
return new n(rC,qv,pJ,pq);}}switch(aR.ToLowerInvariant()){case "red": return o2;case "green": return oJ;case "lime": return oN;case "yellow": return po;
case "orange": return oR;case "cyan": return ov;case "blue": return ok;case "magenta": return oO;case "purple": return o0;case "white": return eJ;
case "black": return oi;case "gray": case "grey": return oI;case "darkgray": case "darkgrey": return ow;default: return eJ;}}}public class a7{
public string[]a8;public string cZ="vertical";public List<aB>az;public string gm,gn;public List<aB>cd;public string gA,gC;
}public class aw : a7{public string dc="default";public int c2=0;public int de=0;public int df=1;public int dg=1;public string bQ;public string gT="left";
public string gU;public string j1;public string j8;public float? ka;public string ca;public string gt;public string[]gu;
public string kq="horizontal";public List<bd>aL=new List<bd>();}public class y : a7{public string aW;public string bM="standard";
public string c0="graphic";public string et;public string[]an;public string kE="left";public string kF;public string j9="Monospace";
public string a6;public string j_;public int j0=10;public int jY=1;public float gr=1.2f;public bool gE;
public string gM;public string go;public string jX;public string jV;public string bE;public string bH;public float gs=2.5f;
public string jZ="cyan";public string[]er;public List<string>es=new List<string>();public List<string>oF(){if(es!=null&&es.Count>0)return es;
return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float ce=1.0f;public int gO=0;public float gj=1.6f;public int c7=1;public int c8=1;public int kH=30;public string be;
public int bL=0;public List<aw>ab=new List<aw>();public List<bd>eC{get{if(ab.Count==0)ab.Add(new aw{dc="main"}
);return ab[0].aL;}}}public class bh{public string gK;public List<string>eE=new List<string>();public Dictionary<string,string>gw=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>gq=new List<string>();}public class gN{public List<aU>bG=new List<aU>();public List<y>T=new List<y>();
public List<string>aK=new List<string>();public List<string>dj=new List<string>();public Dictionary<string,List<string>>cV=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class kg{static readonly Dictionary<string,bh>j2=new Dictionary<string,bh>(StringComparer.OrdinalIgnoreCase);
static kg(){o3();}static void o3(){bg("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bg("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bg("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bg("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bg("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bg("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bg("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bg("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bg(string mS,string[]ri,string pT){var f5=new bh{gK=mS}
;foreach(var rd in ri){var ix=rd.Split(new[]{'='},2);string m_=gL(ix[0]);f5.eE.Add(m_);if(ix.Length>1)f5.gw[m_]=ac(ix[1].Trim());
}var q3=pT.Replace("\r\n","\n").Split('\n');foreach(var qR in q3){string nk=qR.Trim();if(nk.Length>0)f5.gq.Add(nk);}j2[mS]=f5;}static string gL(string cE){
cE=cE.Trim();if(cE.StartsWith("$"))cE=cE.Substring(1);return cE.ToLowerInvariant();}public static gN oT(string p6){var ai=new gN();y d=null;
aw q=null;var m9=oZ(p6,ai.aK,ai.cV);for(int dM=0;dM<m9.Count;dM++){string a=m9[dM].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){ai.bG.Add(oU(a));}else if(a.StartsWith("@screen")){d=new y{aW=a.Substring("@screen".Length).Trim()}
;ai.T.Add(d);q=null;}else if(a.StartsWith("profile:")){p(d,"profile:");d.bM=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){p(d,"border:");string k3=a.Substring("border:".Length).Trim();if(q!=null)q.j1=k3;
else d.c0=k3;}else if(a.StartsWith("border_color:")){p(d,"border_color:");string k5=a.Substring("border_color:".Length).Trim();
if(q!=null)q.gt=k5;else d.et=k5;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
p(d,"border_gradient");string bX=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
bX=bX.Trim();if(bX.StartsWith("(")&&bX.Contains(")"))bX=aC(bX,0);string[]nF=aD(bX).Select(rM=>rM.Trim()).ToArray();if(q!=null)q.gu=nF;
else d.an=nF;}else if(a.StartsWith("title_align:")){p(d,"title_align:");string nN=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(q!=null)q.gT=nN;else d.kE=nN;}else if(a.StartsWith("title_color:")){p(d,"title_color:");string nQ=a.Substring("title_color:".Length).Trim();
if(q!=null)q.gU=nQ;else d.kF=nQ;}else if(a.StartsWith("font_size:")){p(d,"font_size:");float nL=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(q!=null)q.ka=nL;else d.ce=nL;}else if(a.StartsWith("font:")){p(d,"font:");string lT=a.Substring("font:".Length).Trim();
if(q!=null)q.j8=lT;else d.j9=lT;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){p(d,"bg:");string cn=a.Substring(a.IndexOf(':')+1).Trim();
a7 hm=q!=null ?(a7)q : d;if(cn.StartsWith("color(")&&cn.EndsWith(")")){string lc;hm.az=kt(cn.Substring(6,cn.Length-7),out lc);
string la,lb;ky(lc,out la,out lb);hm.gm=la;hm.gn=lb;}else if(q!=null)q.ca=cn;
else d.a6=cn;}else if(a.StartsWith("layout:")){p(d,"layout:");oV(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
p(d,"alert_period:");d.gj=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){p(d,"refresh:");d.gO=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
p(d,"output:");var fG=aD(a.Substring("output:".Length).Trim());if(fG.Count==0)throw new FormatException("output: requires a target block name");
d.be=ac(fG[0]);d.bL=fG.Count>1 ? int.Parse(fG[1]): 0;}else if(a.StartsWith("width:")){p(d,"width:");
d.kH=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){p(d,"boot_msg:");
d.j_=ac(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){p(d,"boot_stagger:");d.j0=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){p(d,"boot_calibrate:");string lA=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.gr=lA=="off" ? 0f : Math.Max(0f,float.Parse(lA,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){p(d,"boot_batch:");
d.jY=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){p(d,"boot_duration:");
string dC=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool mp=dC.EndsWith("s");if(mp)dC=dC.Substring(0,dC.Length-1).Trim();
float hK;if(float.TryParse(dC,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out hK)){d.gs=mp ? hK : hK/60f;
}}else if(a.StartsWith("boot_log:")){p(d,"boot_log:");d.es.Add(ac(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("banner:")){
p(d,"banner:");d.bE=ac(a.Substring("banner:".Length).Trim());}else if(a.StartsWith("boot_logo:")){p(d,"boot_logo:");
d.bH=ac(a.Substring("boot_logo:".Length).Trim());}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){
p(d,"boot_gradient");d.er=ku(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){p(d,"bg_gradient");
(q!=null ?(a7)q : d).a8=ku(a);}else if(a.StartsWith("bg_gradient_dir:")){p(d,"bg_gradient_dir:");
(q!=null ?(a7)q : d).cZ=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(a.StartsWith("flash:")){
p(d,"flash:");a7 hS=q!=null ?(a7)q : d;string hR=a.Substring("flash:".Length).Trim();string lV=kB(hR);
string lU,lW;ky(hR.Substring(0,hR.Length-lV.Length),out lU,out lW);hS.gA=lU;hS.gC=lW;hS.cd=new List<aB>{
new aB{gx=lV,bJ="on"}};}else if(a.StartsWith("bg_texture:")){p(d,"bg_texture:");d.go=ac(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){p(d,"bg_texture_color:");d.jX=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gradient_mode:")){p(d,"gradient_mode:");d.gE=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){p(d,"screen_bg:");d.a6=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
p(d,"panel_bg:");d.gM=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){p(d,"alert_bg:");
d.jV=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){p(d,"boot_color:");
d.jZ=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){p(d,"orientation:");string ra=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(q!=null)q.kq=ra;}else if(a.StartsWith("@panel")){p(d,"@panel");q=oX(a);d.ab.Add(q);
}else if(a.StartsWith("title:")){p(d,"title:");if(q==null){q=new aw{dc="main",c2=0,de=0,df=d.c7,dg=d.c8}
;d.ab.Add(q);}q.bQ=ac(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){p(d,"line:");
if(q==null){q=new aw{dc="main",c2=0,de=0,df=d.c7,dg=d.c8};d.ab.Add(q);}q.aL.Add(oW(a.Substring("line:".Length).Trim()));
}else{ai.aK.Add($"Line {dM + 1}: unrecognized directive: \"{a}\"");}}catch(Exception qe){ai.aK.Add($"Line {dM + 1}: parse error ({qe.Message}) in \"{a}\"");
}}foreach(var nl in ai.T){if(string.IsNullOrEmpty(nl.be))ai.aK.Add($"@screen {nl.aW}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var ak in ai.T){int iT=ak.ab.Count(i5=>i5.ca!=null||i5.az!=null||i5.a8!=null);if(ak.ab.Count>1&&iT>0&&iT<ak.ab.Count)ai.dj.Add("screen '"+ak.aW+"': only "+iT+" of "+ak.ab.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(ak.bE!=null&& !ai.cV.ContainsKey(ak.bE))ai.aK.Add("Screen '"+ak.aW+"': banner '"+ak.bE+"' is not defined (@banner "+ak.bE+" ... @end)");
if(ak.bH!=null&& !ai.cV.ContainsKey(ak.bH))ai.aK.Add("Screen '"+ak.aW+"': boot_logo '"+ak.bH+"' is not defined (@banner "+ak.bH+" ... @end)");
}return ai;}static void oV(string hr,y ns){int iY=hr.IndexOf('(');if(iY<0)throw new FormatException("missing '(' in layout: specification");
string mx=hr.Substring(0,iY).Trim().ToLowerInvariant();string px=aC(hr,iY);var hc=aD(px);if(mx=="grid"){if(hc.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
ns.c7=Math.Max(1,int.Parse(hc[0]));ns.c8=Math.Max(1,int.Parse(hc[1]));}else{throw new FormatException($"unknown layout kind '{mx}' (expected grid)");
}}static aw oX(string qZ){string d5=qZ.Substring("@panel".Length).Trim();int eV=d5.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(eV<0)throw new FormatException("@panel requires an at(...) positioning clause");
string q8=d5.Substring(0,eV).Trim();string k_=d5.Substring(eV);int m5=k_.IndexOf('(');string kY=aC(k_,m5);
var bW=aD(kY);if(bW.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var a1=new aw{dc=q8,c2=int.Parse(bW[0]),de=int.Parse(bW[1]),df=bW.Count>2 ? int.Parse(bW[2]): 1,dg=bW.Count>3 ? int.Parse(bW[3]): 1}
;int kR=eV+m5+kY.Length+1;if(kR<d5.Length){string ay=d5.Substring(kR).Trim();int nV=ay.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(nV>=0){string ec=ay.Substring(nV+5).Trim();if(ec.StartsWith("(")&&ec.Contains(")"))a1.bQ=ac(aC(ec,0));else if(ec.StartsWith(":")){
string bY=ec.Substring(1).Trim();if(bY.StartsWith("\"")){int lP=bY.IndexOf('"',1);a1.bQ=lP>0 ? bY.Substring(1,lP-1): ac(bY);}else{int nB=bY.IndexOf(' ');
a1.bQ=nB>=0 ? bY.Substring(0,nB): bY;}}}string mV=ew(ay,"orientation:");if(mV!=null)a1.kq=mV.ToLowerInvariant();string kQ=ew(ay,"title_align:");
if(kQ!=null)a1.gT=kQ.ToLowerInvariant();string nS=ew(ay,"title_color:");if(nS!=null)a1.gU=nS;int e0=ay.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(e0>=0&& !ay.Substring(e0).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !ay.Substring(e0).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
a1.j1=ew(ay.Substring(e0),"border:");}int eY=ay.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(eY<0)eY=ay.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(eY>=0){string bp=ay.Substring(eY+"border_gradient".Length).Trim();if(bp.StartsWith(":"))bp=bp.Substring(1).Trim();if(bp.StartsWith("(")&&bp.Contains(")"))bp=aC(bp,0);
a1.gu=aD(bp).Select(rN=>rN.Trim()).ToArray();}}return a1;}static string ew(string nm,string mw){int mo=nm.IndexOf(mw,StringComparison.OrdinalIgnoreCase);
if(mo<0)return null;string jP=nm.Substring(mo+mw.Length).Trim();int nC=jP.IndexOf(' ');return nC>=0 ? jP.Substring(0,nC): jP;}static string[]ku(string iD){
int h3=iD.IndexOfAny(new[]{':','('});string dI=iD.Substring(iD[h3]==':' ? h3+1 : h3).Trim();if(dI.StartsWith("(")&&dI.Contains(")"))dI=aC(dI,0);return aD(dI).Select(rO=>rO.Trim()).ToArray();
}static List<string>oZ(string p7,List<string>hO,Dictionary<string,List<string>>pL){var nY=new Dictionary<string,bh>(StringComparer.OrdinalIgnoreCase);
var cI=(p7 ?? "").Replace("\r\n","\n").Split('\n');var iS=new List<string>();int cz=0;while(cz<cI.Length){string jb=cI[cz++];if(jb.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ag=new List<string>();while(cz<cI.Length){string kO=cI[cz++].Replace("\t","    ").TrimEnd();if(kO.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ag.Add(kO);}while(ag.Count>0&&ag[0].Length==0)ag.RemoveAt(0);while(ag.Count>0&&ag[ag.Count-1].Length==0)ag.RemoveAt(ag.Count-1);int il=int.MaxValue;
foreach(var g5 in ag)if(g5.Length>0)il=Math.Min(il,g5.Length-g5.TrimStart().Length);for(int dT=0;dT<ag.Count;dT++)if(ag[dT].Length>0)ag[dT]=ag[dT].Substring(il);
pL[jb.TrimStart().Substring(8).Trim()]=ag;continue;}string bt=kA(jb).Trim();if(bt.Length==0)continue;if(bt.StartsWith("@template")){try{
var jF=oY(bt);while(cz<cI.Length){string pV=cI[cz++];string hs=kA(pV).Trim();if(hs.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(hs.Length>0)jF.gq.Add(hs);}nY[jF.gK]=jF;}catch(Exception qf){hO.Add($"Template parse error ({qf.Message}) in \"{bt}\"");}}
else if(bt.StartsWith("@use")){try{j5(bt,nY,iS,hO);}catch(Exception qg){hO.Add($"@use error ({qg.Message}) in \"{bt}\"");
}}else{iS.Add(bt);}}return iS;}static bh oY(string q_){string jc=q_.Substring("@template".Length).Trim();int iZ=jc.IndexOf('(');
if(iZ<0)throw new FormatException("missing '(' in @template header");string q9=jc.Substring(0,iZ).Trim();string py=aC(jc,iZ);var jG=new bh{
gK=q9};foreach(var re in aD(py)){var iy=re.Split(new[]{'='},2);string m0=gL(iy[0]);jG.eE.Add(m0);if(iy.Length>1)jG.gw[m0]=ac(iy[1].Trim());
}return jG;}static void j5(string q0,Dictionary<string,bh>nZ,List<string>mY,List<string>qd,int lE=0){string fP=q0.Substring("@use".Length).Trim();
int fE=fP.IndexOf('(');string iL=fE>=0 ? fP.Substring(0,fE).Trim(): fP.Trim();string pz=fE>=0 ? aC(fP,fE): "";bh cP=null;
if(!nZ.TryGetValue(iL,out cP)){if(!j2.TryGetValue(iL,out cP))throw new FormatException($"unknown template '{iL}'");}var e2=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var mB in cP.gw)e2[mB.Key]=mB.Value;var kX=aD(pz);for(int du=0;du<kX.Count;du++){string eT=kX[du];int hM=eT.IndexOf('=');if(hM>=0){
string qJ=gL(eT.Substring(0,hM));string sn=ac(eT.Substring(hM+1).Trim());e2[qJ]=sn;}else{if(du<cP.eE.Count){string qK=cP.eE[du];
e2[qK]=ac(eT.Trim());}}}foreach(var pU in cP.gq){string dF=pU;foreach(var ft in e2){dF=dF.Replace("$"+ft.Key,ft.Value).Replace("{"+ft.Key+"}",ft.Value);
}if(dF.StartsWith("@use")){if(lE>3)throw new FormatException("@use nested too deep");j5(dF,nZ,mY,qd,lE+1);
}else mY.Add(dF);}}static void p(y p5,string qa){if(p5==null)throw new FormatException($"'{qa}' used before any @screen declared");
}static string kA(string aQ){if(string.IsNullOrEmpty(aQ))return aQ;bool ik=false;for(int bs=0;bs<aQ.Length;bs++){char hv=aQ[bs];if(hv=='"'){ik= !ik;
}else if(!ik){if(hv=='/'&&bs+1<aQ.Length&&aQ[bs+1]=='/'){return aQ.Substring(0,bs);}if(hv=='#'){if(oL(aQ,bs)){continue;}return aQ.Substring(0,bs);}}}return aQ;
}static bool oL(string b1,int mr){int cC=0;for(int ig=mr+1;ig<b1.Length;ig++){char cq=b1[ig];if((cq>='0'&&cq<='9')||(cq>='a'&&cq<='f')||(cq>='A'&&cq<='F'))cC++;
else break;}if(cC==3||cC==4||cC==6||cC==8){int dW=mr+1+cC;if(dW>=b1.Length||char.IsWhiteSpace(b1[dW])||b1[dW]==')'||b1[dW]==','||b1[dW]==':')return true;
}return false;}static aU oU(string q1){string jd=q1.Substring("@bind".Length).Trim();int hN=jd.IndexOf('=');if(hN<0)throw new FormatException("missing '=' in @bind");
string mT=jd.Substring(0,hN).Trim();string hP=jd.Substring(hN+1).Trim();int i_=hP.IndexOf('(');if(i_<0)throw new FormatException("missing '(' in @bind target");
string fe=hP.Substring(0,i_).Trim().ToLowerInvariant();string pA=aC(hP,i_);var bm=aD(pA);if(bm.Count==0)throw new FormatException("@bind target has no arguments");
var aj=new aU{V=mT};if(fe=="dynamic"||fe=="var"||fe=="mem"){aj.bc=aI.c5;aj.aV=bm.Count>0 ? ac(bm[0].Trim()): mT;
for(int g6=0;g6<bm.Count;g6++){var fu=bm[g6].Split(new[]{':'},2);if(fu.Length==2&&fu[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double lD;
if(double.TryParse(fu[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out lD))aj.ev=lD;else aj.gv=ac(fu[1].Trim());
}}return aj;}aj.bc=fe=="group" ? aI.gF : aI.oj;string eR=null;for(int eQ=0;eQ<bm.Count;eQ++){var cB=bm[eQ].Split(new[]{':'},2);if(cB.Length==2&&cB[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
eR=cB[1].Trim().ToLowerInvariant();}else if(cB.Length==2&&cB[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){aj.en=cB[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{aj.aE.Add(ac(bm[eQ].Trim()));}}if(aj.aE.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(aj.bc==aI.gF){if(eR==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(eR){case "avg": aj.cU=av.gk;break;case "sum": aj.cU=av.kC;break;case "min": aj.cU=av.kp;break;case "max": aj.cU=av.ko;
break;default: throw new FormatException($"unknown agg '{eR}' (expected avg/sum/min/max)");}}return aj;}static bd oW(string e_){var o=new bd();
if(!e_.StartsWith("\""))throw new FormatException("line must start with a quoted label");int hL=e_.IndexOf('"',1);if(hL<0)throw new FormatException("unterminated quoted label");
o.S=e_.Substring(1,hL-1);string d4=e_.Substring(hL+1).Trim();int fF=d4.IndexOf('(');if(fF<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string C=d4.Substring(0,fF).Trim().ToLowerInvariant();string pw=aC(d4,fF);string Z=d4.Substring(d4.IndexOf(')',fF)+1).Trim();
var hI=pw.Trim().Split('.');if(hI.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.aA=hI[0].Trim();o.F=hI[1].Trim();if(C=="bar")o.g=e.cW;else if(C=="led")o.g=e.da;else if(C=="spark"||C=="sparkline")o.g=e.bO;
else if(C=="hazard"||C=="stripes")o.g=e.c9;else if(C=="chevron"||C=="chevrons")o.g=e.c1;else if(C=="dots"||C=="dot")o.g=e.c4;
else if(C=="balance"||C=="horizon"||C=="centered")o.g=e.cb;else if(C=="toggle"||C=="switch")o.g=e.bR;else if(C=="checkbox"||C=="check")o.g=e.bI;
else if(C=="status"||C=="badge"||C=="state")o.g=e.bP;else if(C=="ammo"||C=="pips")o.g=e.b9;else if(C=="flow"||C=="rate"||C=="spinner")o.g=e.bK;
else if(C=="text")o.g=e.gS;else throw new FormatException($"unknown display function '{C}'");while(Z.Length>0){int hG=Z.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int cw=Z.IndexOf('(');if(hG>=0&&(cw<0||hG<cw)){string e5=Z.Substring(hG+4).Trim();int fX=e5.IndexOf(' ');o.j3=(fX>=0 ? e5.Substring(0,fX): e5).ToLowerInvariant();
Z=(fX>=0 ? e5.Substring(fX): "").Trim();continue;}int jN=Z.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(jN>=0&&(cw<0||jN<cw)){
string gf=Z.Substring(jN+5).Trim();int fY=gf.IndexOf(' ');o.di=(fY>=0 ? gf.Substring(0,fY): gf).Trim().Trim('"');Z=(fY>=0 ? gf.Substring(fY): "").Trim();
continue;}int hY=Z.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(hY>=0&&(cw<0||hY<cw)){string e9=Z.Substring(hY+7).Trim();
int fZ=e9.IndexOf(' ');o.c6=(fZ>=0 ? e9.Substring(0,fZ): e9).Trim().Trim('"');Z=(fZ>=0 ? e9.Substring(fZ): "").Trim();continue;}int fy=Z.IndexOf('(');
if(fy<0)break;string al=Z.Substring(0,fy).Trim().ToLowerInvariant();int mG=al.LastIndexOf(' ');if(mG>=0)al=al.Substring(mG+1).Trim();
string a_=aC(Z,fy);Z=Z.Substring(Z.IndexOf(')',fy)+1).Trim();if(al=="gradient"){o.cf=true;o.bb=aD(a_).Select(rP=>rP.Trim()).ToArray();
if(o.bb.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(al=="flat"){o.ao=a_.Trim();}else if(al=="pattern"){
string N=a_.Trim().ToLowerInvariant();if(N=="hazard"||N=="stripes")o.g=e.c9;else if(N=="chevron"||N=="chevrons")o.g=e.c1;
else if(N=="dots"||N=="dot")o.g=e.c4;else if(N=="led")o.g=e.da;else if(N=="balance"||N=="horizon")o.g=e.cb;
else if(N=="toggle"||N=="switch")o.g=e.bR;else if(N=="checkbox"||N=="check")o.g=e.bI;else if(N=="status"||N=="badge"||N=="state")o.g=e.bP;
else if(N=="ammo"||N=="pips")o.g=e.b9;else if(N=="flow"||N=="rate"||N=="spinner")o.g=e.bK;}else if(al=="color"){string qC;
o.c3=kt(a_,out qC);}else if(al=="dir"||al=="direction"){o.j3=a_.Trim().ToLowerInvariant();}else if(al=="unit"){
o.di=a_.Trim().Trim('"');}else if(al=="max"){double mO;if(double.TryParse(a_.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mO))o.eD=mO;
else{var iI=a_.Trim().Split('.');if(iI.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.ch=iI[0].Trim();o.gJ=iI[1].Trim();
}}else if(al=="format"||al=="fmt"){o.c6=a_.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{al}'");}}return o;}static string aC(string fT,int iQ){
int hF=0;for(int dN=iQ;dN<fT.Length;dN++){if(fT[dN]=='(')hF++;else if(fT[dN]==')'){hF--;if(hF==0)return fT.Substring(iQ+1,dN-iQ-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aD(string pB){return pB.Split(',').Select(rQ=>rQ.Trim()).Where(rR=>rR.Length>0).ToList();
}static List<aB>kt(string pv,out string hU){var fR=new List<aB>();hU=null;foreach(var i0 in aD(pv)){int hC=i0.LastIndexOf(':');
if(hC<0)throw new FormatException("color() entries need 'expr : color'");string fv=i0.Substring(0,hC).Trim();string lw=i0.Substring(hC+1).Trim();
if(fv.Equals("default",StringComparison.OrdinalIgnoreCase)){fR.Add(new aB{cg=true,bJ=lw});}else{string nG=kB(fv);
if(hU==null)hU=fv.Substring(0,fv.Length-nG.Length).Trim();fR.Add(new aB{gx=nG,bJ=lw});}}if(!fR.Any(rD=>rD.cg))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return fR;}static void ky(string rE,out string pR,out string qj){var dZ=(rE ?? "").Split('.');if(dZ.Length!=2||dZ[0].Trim().Length==0||dZ[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
pR=dZ[0].Trim();qj=dZ[1].Trim();}static string kB(string cv){cv=cv.Trim();foreach(var rb in new[]{">=","<=","==",">","<"}){int ih=cv.IndexOf(rb,StringComparison.Ordinal);
if(ih>0)return cv.Substring(ih).Trim();if(ih==0)return cv;}throw new FormatException($"color() rule has no recognizable comparison operator: '{cv}'");}
static string ac(string b2){if(b2.Length>=2&&b2[0]=='"'&&b2[b2.Length-1]=='"')return b2.Substring(1,b2.Length-2);return b2;}}public static class oh{public static double og(av pr,List<double>cR){
if(cR==null||cR.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(pr){case av.gk: return cR.Average();case av.kC: return cR.Sum();
case av.kp: return cR.Min();case av.ko: return cR.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class D{
public static string o7(string qT,double fH,int k4=10,string n4=null){fH=Math.Max(0,Math.Min(100,fH));int l1=(int)Math.Round(k4*(fH/100.0));
string pM=new string('#',l1)+new string('-',k4-l1);string so=n4!=null ? n4 : $"{fH:0}%";return $"{qT} [{pM}] {so}";
}public static string kb(double bD,string jM=null,string fd=null){if(jM=="auto"){double g7=Math.Abs(bD);if(g7>=1e9)return(bD/1e9).ToString("F1")+"G";
if(g7>=1e6)return(bD/1e6).ToString("F1")+"M";if(g7>=1e3)return(bD/1e3).ToString("F1")+"k";string qh= !string.IsNullOrEmpty(fd)? fd : "F0";return bD.ToString(qh);
}string l3= !string.IsNullOrEmpty(fd)? fd :(Math.Abs(bD)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(jM)){return bD.ToString(l3);}return bD.ToString(l3)+jM;
}public static float oG(string rt,float nJ,float jD,float e6=1.0f){string cF=(rt ?? "standard").ToLowerInvariant();
if(cF=="compact"||(cF=="auto"&&nJ<=300&&jD<=300&&jD>160)){return e6*0.75f;}if(cF=="corner"||(cF=="auto"&&jD<=160)){
return e6*0.55f;}if(cF=="wide"||(cF=="auto"&&nJ>=1000)){return e6*1.15f;}return e6;}public static string o5(string qU,double fI,int bq=5){
fI=Math.Max(0,Math.Min(100,fI));double e7=(fI-50.0)/50.0;int dU=0;int d7=0;if(e7< -0.01)dU=(int)Math.Round(-e7*bq);
else if(e7>0.01)d7=(int)Math.Round(e7*bq);if(dU>bq)dU=bq;if(d7>bq)d7=bq;string qX=new string('-',bq-dU)+new string('<',dU);
string rJ=new string('>',d7)+new string('-',bq-d7);return $"{qU} [{qX}|{rJ}] {fI:0}%";}public static string pg(string mD,double sq){
return sq>=50.0 ? $"{mD} [====O] ON" : $"{mD} [O====] OFF";}public static string pa(string mE,double sr){return sr>=50.0 ? $"{mE} [X] ACTIVE" : $"{mE} [ ] READY ";
}public static string pf(string iA,double n3){if(n3>=75.0)return $"{iA} [ ONLINE  ]";if(n3>=25.0)return $"{iA} [ STANDBY ]";return $"{iA} [ OFFLINE ]";
}public static string o4(string qV,double fJ,int mK=8){fJ=Math.Max(0,Math.Min(100,fJ));int l2=(int)Math.Round(mK*(fJ/100.0));
string rp=new string('|',l2)+new string('.',mK-l2);return $"{qV} [{rp}] {fJ:0}%";}public static string pb(string iB,double d2,int nU=0){
char[]r1=new[]{'|','/','-','\\'};char nn=r1[(nU>=0 ? nU : 0)%4];if(d2>55.0)return $"{iB} {nn} ▲ CHG {d2:0}%";if(d2<45.0)return $"{iB} {nn} ▼ DSC {d2:0}%";
return $"{iB} ● STBY {d2:0}%";}public static bool oP(string am,string dQ){if(am==null||dQ==null)return false;if(am=="*"||am==dQ)return true;
if(!am.Contains("*")&& !am.Contains("?"))return string.Equals(am,dQ,StringComparison.OrdinalIgnoreCase);int at=0,dO=0,jw= -1,iH=0;while(dO<dQ.Length){
if(at<am.Length&&(am[at]=='?'||char.ToLowerInvariant(am[at])==char.ToLowerInvariant(dQ[dO]))){at++;dO++;}else if(at<am.Length&&am[at]=='*'){
jw=at;iH=dO;at++;}else if(jw!= -1){at=jw+1;iH++;dO=iH;}else{return false;}}while(at<am.Length&&am[at]=='*')at++;return at==am.Length;
}public static int ol(double rf,int nX){return Math.Max(0,Math.Min(nX,1+(int)(Math.Max(0.0,rf)*nX)));}public static string ey(string[]cO,double i1){
if(cO==null||cO.Length==0)return "white";i1=Math.Max(0,Math.Min(100,i1));int ii=(int)Math.Floor(i1/100.0*cO.Length);if(ii>=cO.Length)ii=cO.Length-1;
return cO[ii];}public static n kd(string[]bB,double rm){if(bB==null||bB.Length==0)return n.eJ;if(bB.Length==1)return n.dd(bB[0]);
double rH=Math.Max(0.0,Math.Min(1.0,rm/100.0));double nr=rH*(bB.Length-1);int ij=(int)Math.Floor(nr);int qB=Math.Min(bB.Length-1,ij+1);
double jA=nr-ij;n cr=n.dd(bB[ij]);n hw=n.dd(bB[qB]);byte rF=(byte)Math.Round(cr.bf+(hw.bf-cr.bf)*jA);
byte qw=(byte)Math.Round(cr.ba+(hw.ba-cr.ba)*jA);byte pK=(byte)Math.Round(cr.a5+(hw.a5-cr.a5)*jA);return new n(rF,qw,pK);}public static string eF(List<aB>nj,double ss){
foreach(var jf in nj){if(jf.cg)continue;if(oC(jf.gx,ss))return jf.bJ;}var lC=nj.FirstOrDefault(rG=>rG.cg);return lC!=null ? lC.bJ : null;
}static bool oC(string dG,double ei){dG=dG.Trim();foreach(var iP in new[]{">=","<=","==",">","<"}){if(dG.StartsWith(iP)){double d6=double.Parse(dG.Substring(iP.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(iP){case ">=": return ei>=d6;case "<=": return ei<=d6;case "==": return Math.Abs(ei-d6)<1e-9;case ">": return ei>d6;case "<": return ei<d6;
}}}throw new FormatException($"unsupported comparison expression: '{dG}'");}public static List<string>o9(List<string>p4,int cS){
var fA=new List<string>();fA.Add("+"+new string('-',cS+2)+"+");foreach(var iE in p4){string sj=iE.Length>cS ? iE.Substring(0,cS): iE;
string rg=sj.PadRight(cS);fA.Add("| "+rg+" |");}fA.Add("+"+new string('-',cS+2)+"+");return fA;}public struct ks{public float kJ,kK,kI,kf;
}public static ks os(int lu,int ng,int f_,int f0,int ee,int ef,float sa,float r9,float m1,float m2,float h5,float h6){
ee=Math.Max(1,ee);ef=Math.Max(1,ef);f_=Math.Max(1,Math.Min(ee-lu,f_));f0=Math.Max(1,Math.Min(ef-ng,f0));
float pD=sa-(m1*2f);float pC=r9-(m2*2f);float lp=(pD-(h5*(ee-1)))/ee;float lo=(pC-(h6*(ef-1)))/ef;
float rv=m1+lu*(lp+h5);float rw=m2+ng*(lo+h6);float ru=f_*lp+((f_-1)*h5);float rn=f0*lo+((f0-1)*h6);return new ks{
kJ=rv,kK=rw,kI=ru,kf=rn};}public struct kw{public int oS;public int gI;public int W;public int ap;public int dh;
public int aJ;public int pl;public int pm;public int oz;public int oA;
public double oB;}public static kw ot(y bz){var k=new kw();var m4=bz.ab!=null&&bz.ab.Count>0 ? bz.ab : new List<aw>{
new aw{aL=bz.eC}};k.oS=m4.Count;k.gI=0;k.W=0;k.ap=0;if(!string.IsNullOrEmpty(bz.a6)&& !bz.a6.Equals("none",StringComparison.OrdinalIgnoreCase))k.W++;
if(!string.IsNullOrEmpty(bz.c0)&& !bz.c0.Equals("none",StringComparison.OrdinalIgnoreCase))k.W+=2;foreach(var d_ in m4){
if(!string.IsNullOrEmpty(d_.ca)&& !d_.ca.Equals("none",StringComparison.OrdinalIgnoreCase))k.W++;if(!string.IsNullOrEmpty(d_.bQ))k.W+=2;
k.gI+=d_.aL.Count;foreach(var qS in d_.aL){k.pl++;k.pm++;k.W+=2;switch(qS.g){case e.cW: k.dh++;
k.aJ++;k.W+=2;k.ap++;break;case e.da: k.aJ+=10;k.W+=10;
k.ap+=10;break;case e.c9: k.aJ+=10;k.W+=11;k.ap+=10;break;case e.c1: k.aJ+=10;
k.W+=10;k.ap+=10;break;case e.c4: k.aJ+=12;k.W+=12;k.ap+=12;
break;case e.bO: k.dh++;k.aJ+=8;k.W+=9;k.ap+=8;break;case e.cb: k.dh+=2;
k.aJ++;k.W+=3;k.ap++;break;case e.bR: case e.bI: k.dh++;
k.aJ++;k.W+=2;k.ap++;break;case e.bP: case e.bK: k.dh++;k.aJ+=2;
k.W+=3;k.ap+=2;break;case e.b9: k.aJ+=8;k.W+=8;k.ap+=8;
break;default: k.ap++;break;}}}k.oz=25+(k.gI*2);k.oA=180+(k.ap*4);
k.oB=0.0;return k;}}