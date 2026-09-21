h9 t;string iu=null;Dictionary<string,List<IMyTerminalBlock>>bU=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>fU=new Dictionary<string,bool>();
Dictionary<string,bq>ef=new Dictionary<string,bq>();Dictionary<string,Color>mp=new Dictionary<string,Color>();List<IMyTerminalBlock>cC=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>c9=new List<IMyTerminalBlock>();bool fR;int iq;string eg="";StringBuilder ej=new StringBuilder();readonly Dictionary<string,double>aL=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>c8=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener eh;bool cD=false;
Dictionary<z,IMyTextSurface>bV=new Dictionary<z,IMyTextSurface>();Dictionary<z,string>cE=new Dictionary<z,string>();
static readonly string[]l4=new string[101];static Program(){for(int gx=0;gx<=100;gx++){l4[gx]=gx+"%";}}static string lT(double u3){int eK=(int)Math.Round(u3);
if(eK<0)eK=0;else if(eK>100)eK=100;return l4[eK];}class ax{public List<MySprite>f=new List<MySprite>();public List<ct>d8=new List<ct>();
public int cm= -1;public Color[]fp;public Color fl;public bool fm;public List<h8>bP=new List<h8>();public Vector2 d3,h4;
public int ed;public bool d1=false;public List<fq>dQ=new List<fq>();}class h8{public string l3;public int id;public Vector2 l8,mc;
}class fq{public int fE;public Color hF,fw;public List<aZ>fN,fA;public string hJ,hR,hS,hU;public bool lZ,hW;
}class ct{public bs fH;public int _= -1;public int a1= -1;public int c4= -1;public bool lV=false;
public List<int>N=new List<int>();public float[]cy=new float[8];public float bI;public float a8;public float a7;
public float cl;public double fG= -99999.0;public string l0;public float E= -1f;public bool mk;public float lK=12f;}Dictionary<z,ax>bW=new Dictionary<z,ax>();
int mr=0;double iw=0;double ek=0;double ab=0;int ei=0;Dictionary<z,double>bv=new Dictionary<z,double>();
Dictionary<z,double>ip=new Dictionary<z,double>();Dictionary<string,int>fX=new Dictionary<string,int>();List<string>da=new List<string>();
Dictionary<string,int>fY=new Dictionary<string,int>();double iy;Dictionary<string,double>em=new Dictionary<string,double>();List<MyInventoryItem>it=new List<MyInventoryItem>();
int bX= -1,fW;double ix;Dictionary<z,double>fS=new Dictionary<z,double>();int el=0;const string qW="0.11.0";
IMyTextSurface a2;bool io=true;double fT=0;string mq="";string iv="";int fV=0,ir=0;List<KeyValuePair<string,int>>c7=new List<KeyValuePair<string,int>>();
const double rd=120;const float qr=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{eh=IGC.RegisterBroadcastListener("HELM");
eh.SetMessageCallback("HELM_IGC");}catch{}fM();ie();lY();}public void Main(string mE,UpdateType u_){
double nw=Runtime.TimeSinceLastRun.TotalSeconds;ab+=nw;if((u_&UpdateType.IGC)!=0||mE=="HELM_IGC"){rb();}string pc=(mE ?? "").Trim();
string ap=pc.ToLowerInvariant();if(ap=="reload"){fM();}else if(ap=="refresh"){ib();}else if(ap=="boot"){ie();}else if(ap=="probe"){
ek=ab+20.0;qL();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(ap=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=rz;
fM();ie();}else if(ap=="console on"||ap=="console off"){io=ap=="console on";lY();fT=0;}else if(ap=="scan"||ap=="diagnose"||ap=="test"){
rw();return;}else if(ap.StartsWith("set ")){string g4=pc.Substring(4).Trim();int nA=g4.IndexOf('=');int uA=g4.IndexOf(' ');int k0=nA>0 ? nA : uA;
if(k0>0){string of=g4.Substring(0,k0).Trim();string p6=g4.Substring(k0+1).Trim();lD($"{of}={p6}");Echo($"[HELM] Set dynamic variable '{of}' = {p6}");
}return;}else if(ap.StartsWith("alert")){string ff=ap.Length>5 ? ap.Substring(5).Trim(): "toggle";if(ff=="toggle")cD= !cD;else cD=ff=="red"||ff=="on"||ff=="1"||ff=="true";
Echo($"[HELM] Red alert state: {cD}");return;}else if(Me.CustomData!=iu){fM();}el++;iw+=nw;if(iw>=rd){
iw=0;ib();}if(ek>0){if(ab<ek){im();return;}ek=0;bW.Clear();cE.Clear();
fT=0;}if(t==null||t.V.Count==0){im();lL();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cQ=t.V[mr%t.V.Count];
mr++;iv=cQ.be;bool iC=bv.Count>0;if(iC)rk();double nx;if(!bv.ContainsKey(cQ)&& !(ip.TryGetValue(cQ,out nx)&&ab<nx)){
if(rp(cQ))iC=true;if(cQ.ia>0)ip[cQ]=ab+cQ.ia/60.0;}ei=iC ? 0 : ei+1;Runtime.UpdateFrequency=ei<=t.V.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
ry();rx();im();lL();fV=Runtime.CurrentInstructionCount;if(fV>ir)ir=fV;}void lL(){if(eg.Length>0)Echo(eg);
if(ej.Length>0)Echo(ej.ToString());}List<IMyTerminalBlock>fn(bool rJ=false){if(!fR){c9.Clear();GridTerminalSystem.GetBlocks(c9);
cC.Clear();cC.AddRange(c9);cC.RemoveAll(rW=> !rW.IsSameConstructAs(Me));iq=c9.Count-cC.Count;fR=true;
}return rJ ? c9 : cC;}IMyTerminalBlock lO(string oI,bool rL=false){var iL=GridTerminalSystem.GetBlockWithName(oI);if(iL==null||rL||iL.IsSameConstructAs(Me))return iL;
var iE=fn();for(int gy=0;gy<iE.Count;gy++)if(iE[gy].CustomName.IndexOf(oI,StringComparison.OrdinalIgnoreCase)>=0)return iE[gy];return null;}void qC(string oJ,List<IMyTerminalBlock>eM,bool mx=false){
var nS=GridTerminalSystem.GetBlockGroupWithName(oJ);if(nS==null)return;int mP=eM.Count;var fc=new List<IMyTerminalBlock>();nS.GetBlocks(fc);lB(fc,eM,mx);
if(eM.Count>mP)return;var n2=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(n2);foreach(var ql in n2){if(ql.Name.IndexOf(oJ,StringComparison.OrdinalIgnoreCase)<0)continue;
fc.Clear();ql.GetBlocks(fc);lB(fc,eM,mx);if(eM.Count>mP)return;}}void lB(List<IMyTerminalBlock>sQ,List<IMyTerminalBlock>od,bool rK=false){
foreach(var iM in sQ)if((rK||iM.IsSameConstructAs(Me))&& !od.Contains(iM))od.Add(iM);}void fM(){iu=Me.CustomData;t=lX.q4(iu);
eg="";if(t.bb.Count>0){var pn=new StringBuilder("HELM config errors:\n");foreach(var sz in t.bb)pn.AppendLine(" - "+sz);eg=pn.ToString();
}if(t.ee.Count>0){var qj=new StringBuilder("HELM notes:\n");foreach(var u8 in t.ee)qj.AppendLine(" - "+u8);eg+=qj.ToString();}ef.Clear();
foreach(var iN in t.cn)if(!ef.ContainsKey(iN.af))ef[iN.af]=iN;bW.Clear();cE.Clear();bv.Clear();ip.Clear();
fS.Clear();qR();ib();}void rb(){try{while(eh!=null&&eh.HasPendingMessage){
var oD=eh.AcceptMessage();if(oD.Data is string){lD((string)oD.Data);}}}catch{}}void lD(string o1){
if(string.IsNullOrEmpty(o1))return;var oZ=o1.Split(';');for(int jJ=0;jJ<oZ.Length;jJ++){var eY=oZ[jJ].Trim();if(eY.Length==0)continue;int ex=eY.IndexOf('=');
if(ex<0)ex=eY.IndexOf(':');if(ex>0){string dr=eY.Substring(0,ex).Trim();string bo=eY.Substring(ex+1).Trim();if(dr.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cD=bo.Equals("red",StringComparison.OrdinalIgnoreCase)||bo=="1"||bo.Equals("true",StringComparison.OrdinalIgnoreCase)||bo.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double oM;c8.Remove(dr);if(double.TryParse(bo,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out oM)){
aL[dr]=oM;}else if(bo.Equals("true",StringComparison.OrdinalIgnoreCase)||bo.Equals("on",StringComparison.OrdinalIgnoreCase)){aL[dr]=100.0;
}else if(bo.Equals("false",StringComparison.OrdinalIgnoreCase)||bo.Equals("off",StringComparison.OrdinalIgnoreCase)){aL[dr]=0.0;}else{c8[dr]=bo;
}}}}}const string rz="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void qL(){var iF=new List<IMyTextSurface>();if(a2!=null)iF.Add(a2);foreach(var tD in bV.Values)iF.Add(tD);foreach(var uv in iF)qK(uv);
}static void bt(List<MySprite>uB,float ss,float st,float u9,float sX,Color sf){uB.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(ss,st),Size=new Vector2(u9,sX),Color=sf}
);}void qK(IMyTextSurface k1){Vector2 aT=k1.SurfaceSize,aH=k1.TextureSize,an=(aH-aT)/2f;var aG=new List<MySprite>();Color gv=new Color(150,150,150);
bt(aG,aH.X/2f,2f,aH.X,4f,gv);bt(aG,aH.X/2f,aH.Y-2f,aH.X,4f,gv);bt(aG,2f,aH.Y/2f,4f,aH.Y,gv);bt(aG,aH.X-2f,aH.Y/2f,4f,aH.Y,gv);
bt(aG,an.X+aT.X/2f,an.Y+aT.Y/2f,aT.X,aT.Y,new Color(20,40,90));float am=32f;bt(aG,an.X+am/2f,an.Y+am/2f,am,am,new Color(255,0,0));bt(aG,an.X+aT.X-am/2f,an.Y+am/2f,am,am,new Color(0,255,0));
bt(aG,an.X+am/2f,an.Y+aT.Y-am/2f,am,am,new Color(255,255,0));bt(aG,an.X+aT.X-am/2f,an.Y+aT.Y-am/2f,am,am,new Color(255,0,255));float lz=an.Y+aT.Y/2f-30f;
a6(aG,"SURFACE "+aT.X.ToString("0")+"x"+aT.Y.ToString("0"),Color.White,an.X+44f,lz,0.9f);a6(aG,"TEXTURE "+aH.X.ToString("0")+"x"+aH.Y.ToString("0"),Color.White,an.X+44f,lz+24f,0.9f);
a6(aG,"OFFSET  "+an.X.ToString("0")+","+an.Y.ToString("0"),Color.White,an.X+44f,lz+48f,0.9f);using(var sJ=k1.DrawFrame()){sJ.AddRange(aG);}}static bool l_(string dl){
return dl.Length>3&&(dl.StartsWith("top")&&char.IsDigit(dl[3])||dl.Length>4&&dl.StartsWith("line")&&char.IsDigit(dl[4]));}void qR(){fX.Clear();da.Clear();
fY.Clear();bX= -1;ix=iy=0;foreach(var kX in t.V){var ke=new List<List<bs>>();foreach(var tZ in kX.ak)ke.Add(tZ.ao);
if(kX.ak.Count==0)ke.Add(kX.fI);foreach(var tx in ke)foreach(var eQ in tx){if(!l_(eQ.L))continue;bool p0=eQ.L[0]=='t';var np=p0 ? fX : fY;
int tz=int.Parse(eQ.L.Substring(p0 ? 3 : 4));int n6;np.TryGetValue(eQ.aY,out n6);np[eQ.aY]=Math.Max(n6,tz);}}da.AddRange(fX.Keys);
}void rx(){if(fY.Count==0||ab<iy)return;iy=ab+2.0;foreach(var j5 in fY){List<IMyTerminalBlock>iU;var pw=bU.TryGetValue(j5.Key,out iU)&&iU.Count>0 ? iU[0]as IMyTextSurface : null;
string[]oz=pw==null ? new string[0]: pw.GetText().Split('\n');for(int e0=1;e0<=j5.Value;e0++)c8[j5.Key+".line"+e0]=e0<=oz.Length ? oz[e0-1].TrimEnd('\r'): "";}}
void ry(){if(da.Count==0)return;if(bX<0){if(ab<ix)return;bX=0;fW=0;em.Clear();}while(bX<da.Count&&Runtime.CurrentInstructionCount<20000){
string iX=da[bX];List<IMyTerminalBlock>f7;bU.TryGetValue(iX,out f7);if(f7!=null&&fW<f7.Count){var mG=f7[fW++];
for(int jK=0;jK<mG.InventoryCount;jK++){it.Clear();mG.GetInventory(jK).GetItems(it);foreach(var gF in it){string j1=gF.Type.TypeId.Replace("MyObjectBuilder_","");
string og=j1=="Ore"||j1=="Ingot" ? gF.Type.SubtypeId+" "+j1.ToLowerInvariant(): gF.Type.SubtypeId;double n7;em.TryGetValue(og,out n7);em[og]=n7+(double)gF.Amount;
}}continue;}var g2=new List<KeyValuePair<string,double>>(em);g2.Sort((vb,vh)=>vh.Value.CompareTo(vb.Value));for(int dz=1;dz<=fX[iX];dz++)c8[iX+".top"+dz]=dz<=g2.Count ? g2[dz-1].Key+" "+K.lS(g2[dz-1].Value,"auto"): "-";
em.Clear();bX++;fW=0;}if(bX>=da.Count){bX= -1;ix=ab+15.0;}}void aj(int uK,string uS){c7.Add(new KeyValuePair<string,int>(uS,uK));
}static string il(string kQ,int oH){return kQ.Length>oH ? kQ.Substring(0,oH): kQ;}void lY(){var kJ=Me as IMyTextSurfaceProvider;if(kJ==null||kJ.SurfaceCount<1)return;
a2=kJ.GetSurface(0);a2.ContentType=io ? ContentType.SCRIPT : ContentType.NONE;a2.Script="";a2.ScriptBackgroundColor=Color.Black;
}void im(){if(ab>=fT){fT=ab+(ei>t.V.Count+1 ? 5.0 : 1.0);qz();if(io&&a2!=null&&ek==0)qJ();
}Echo(mq);}void qz(){c7.Clear();int i_=0,mW=0;foreach(var mH in t.cn){List<IMyTerminalBlock>j8;if(mH.bN==a9.dX){
i_++;continue;}if(bU.TryGetValue(mH.af,out j8)){mW+=j8.Count;if(j8.Count>0)i_++;}}int nB=t.bb.Count;aj(0,"HELM-[Kernel-(v"+qW+")]");
aj(4,"------------------------------");aj(1,"krn-cfg");aj(2," screens "+t.V.Count+"  binds "+i_+"/"+t.cn.Count+"  errors "+nB);
aj(2," alert "+(cD ? "RED" : "off")+"  blocks "+mW+"  docked-ignored "+iq);if(nB>0)aj(3," ! "+il(t.bb[0],40));
else if(t.ee.Count>0)aj(3," ? "+il(t.ee[0],40));aj(1,"cfg-dsp");if(t.V.Count==0)aj(3," no @screen: run 'starter'");
int ux=0;foreach(var kR in t.V){if(++ux>10){aj(2," +"+(t.V.Count-10)+" more");break;}IMyTextSurface k2;string py="--",k7="no block";
if(bV.TryGetValue(kR,out k2)){py=k2.SurfaceSize.X.ToString("0")+"x"+k2.SurfaceSize.Y.ToString("0");k7=bv.ContainsKey(kR)? "boot" : "live";
}aj(k7=="no block" ? 3 : 2," "+il(kR.be,14).PadRight(14)+" "+py.PadRight(9)+" "+k7);}aj(1,"perf");aj(2," tick "+el+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
aj(2," instr "+fV+"  peak "+ir+"  max "+Runtime.MaxInstructionCount);aj(2," rate "+Runtime.UpdateFrequency+"  idle "+ei+"  up "+(int)ab+"s");
if(iv.Length>0)aj(2," last "+iv);var po=new StringBuilder();foreach(var tc in c7)po.AppendLine(tc.Key);mq=po.ToString();}static Color qF(int uL){
switch(uL){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void qJ(){Vector2 cR=a2.SurfaceSize;Vector2 kl=(a2.TextureSize-cR)/2f;int i8=30;foreach(var td in c7)i8=Math.Max(i8,td.Key.Length);
Vector2 i7=a2.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float pp=Math.Min(1f,Math.Min(cR.X*0.94f/(i8*i7.X),cR.Y*0.94f/(c7.Count*i7.Y)));
float vc=kl.X+cR.X*0.03f;float qo=kl.Y+cR.Y*0.03f;var k3=new List<MySprite>();k3.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kl+cR/2f,Size=cR,Color=new Color(12,12,16)}
);foreach(var ol in c7){a6(k3,ol.Key,qF(ol.Value),vc,qo,pp);qo+=i7.Y*pp;}using(var sK=a2.DrawFrame()){sK.AddRange(k3);}}static readonly List<string>qy=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void ie(){bv.Clear();if(t==null)return;for(int gz=0;gz<t.V.Count;gz++){var ha=t.V[gz];if(ha.hM>0f)bv[ha]=ab+(gz/Math.Max(1,ha.lF))*ha.lI/60.0;
}}void rk(){List<z>ew=null;int sW=(bv.Count+3)/4,s5=0;foreach(var gJ in bv){double kt=(ab-gJ.Value)/gJ.Key.hM;if(kt<0)continue;
if(kt>=1.0){if(ew==null)ew=new List<z>();ew.Add(gJ.Key);continue;}if((s5++ +el)%sW==0)qI(gJ.Key,kt);}if(ew==null)return;foreach(var e7 in ew){
bv.Remove(e7);if(e7.hL>0f)fS[e7]=ab;bW.Remove(e7);cE.Remove(e7);}}static void a6(List<MySprite>uC,string uT,Color si,float vd,float vi,float us){
var pO=MySprite.CreateText(uT,"Monospace",si,us,TextAlignment.LEFT);pO.Position=new Vector2(vd,vi);uC.Add(pO);}static void lA(List<MySprite>uD,List<string>f2,z hd,float ve,float vj,float ut,float to){
string[]uI=hd.fr!=null&&hd.fr.Length>0 ? hd.fr : new[]{hd.lG};for(int eF=0;eF<f2.Count;
eF++){p i1=K.lU(uI,f2.Count>1 ? 100.0*eF/(f2.Count-1): 0.0);a6(uD,f2[eF],new Color(i1.bQ,i1.bM,i1.bG),ve,vj+eF*to,ut);}}bool ro(z dB,IMyTextSurface lb){
ax aB,aO;z ps=t.V.Find(qm=>qm!=dB&&qm.be.Equals(dB.h0,StringComparison.OrdinalIgnoreCase));if(ps==null|| !bW.TryGetValue(ps,out aB)|| !aB.d1)return false;
if(!bW.TryGetValue(dB,out aO))bW[dB]=aO=new ax();Vector2 dE=lb.SurfaceSize;if(aO.d1&&aO.ed==aB.ed&&aO.d3==dE)return false;
aO.d1=true;aO.ed=aB.ed;aO.d3=dE;aO.f.Clear();Vector2 hm=aB.d3,oQ=aB.h4,oO=(lb.TextureSize-dE)/2f;aO.h4=oO;
int nP=0,p_=aB.f.Count;if(dB.h1!=null){int dy=aB.bP.FindIndex(vf=>vf.l3.Equals(dB.h1,StringComparison.OrdinalIgnoreCase));
if(dy<0)return false;hm=aB.bP[dy].mc;oQ=aB.bP[dy].l8;nP=aB.bP[dy].id;if(dy+1<aB.bP.Count)p_=aB.bP[dy+1].id;}float gH=Math.Min(dE.X/hm.X,dE.Y/hm.Y);
Vector2 rT=oO+(dE-hm*gH)/2f;for(int jL=nP;jL<p_;jL++){var bz=aB.f[jL];if(bz.Position.HasValue)bz.Position=(bz.Position.Value-oQ)*gH+rT;if(bz.Size.HasValue)bz.Size=bz.Size.Value*gH;
if(bz.Type==SpriteType.TEXT)bz.RotationOrScale*=gH;aO.f.Add(bz);}using(var sL=lb.DrawFrame())sL.AddRange(aO.f);return true;}bool ri(z dC,IMyTextSurface hk){
List<string>eo;if(cE.ContainsKey(dC)|| !t.dN.TryGetValue(dC.ck,out eo))return false;cE[dC]="banner";Vector2 cS=hk.SurfaceSize;
Vector2 km=(hk.TextureSize-cS)/2f;int gd=1;foreach(var te in eo)gd=Math.Max(gd,te.Length);Vector2 eu=hk.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float hc=Math.Min(cS.X*0.94f/(gd*eu.X),cS.Y*0.94f/(eo.Count*eu.Y));var k4=new List<MySprite>();k4.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=km+cS/2f,Size=cS,Color=string.IsNullOrEmpty(dC.bH)? new Color(10,12,16): F(dC.bH)}
);lA(k4,eo,dC,km.X+(cS.X-gd*eu.X*hc)/2f,km.Y+(cS.Y-eo.Count*eu.Y*hc)/2f,hc,eu.Y*hc);using(var sM=hk.DrawFrame()){sM.AddRange(k4);
}return true;}void qI(z dD,double oT){IMyTextSurface e9;if(!bV.TryGetValue(dD,out e9))return;Vector2 cg=e9.SurfaceSize;
Vector2 kn=(e9.TextureSize-cg)/2f;var eR=dD.qS();string oE=dD.lH ?? "HELM TELEMETRY KERNEL ONLINE";bool kf=cg.Y>=200f;
List<string>db;if(dD.co==null|| !t.dN.TryGetValue(dD.co,out db))db=qy;int di=oE.Length+3;foreach(var tf in eR)di=Math.Max(di,tf.Length+5);
if(kf)foreach(var tg in db)di=Math.Max(di,tg.Length);int pg=(kf ? db.Count+1 : 0)+2+eR.Count;Vector2 dh=e9.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float cc=Math.Min(1f,Math.Min(cg.X*0.94f/(di*dh.X),cg.Y*0.94f/(pg*dh.Y)));float hB=kn.X+(cg.X-di*dh.X*cc)/2f;float cX=kn.Y+(cg.Y-pg*dh.Y*cc)/2f;
float gN=dh.Y*cc;var dF=new List<MySprite>();dF.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kn+cg/2f,Size=cg,Color=new Color(10,12,16)}
);if(kf){lA(dF,db,dD,hB,cX,cc,gN);cX+=(db.Count+1)*gN;}a6(dF,">> "+oE,new Color(34,197,94),hB,cX,cc);cX+=gN*2f;int uy=K.qx(oT,eR.Count);
for(int gA=0;gA<uy;gA++){bool oP=gA==eR.Count-1&&oT>=0.8;a6(dF,oP ? "[OK]" : "[..]",oP ? new Color(34,197,94): new Color(56,189,248),hB,cX,cc);a6(dF,eR[gA],new Color(203,213,225),hB+5f*dh.X*cc,cX,cc);
cX+=gN;}using(var sN=e9.DrawFrame()){sN.AddRange(dF);}}void ib(){bU.Clear();bV.Clear();fU.Clear();
ej.Clear();fR=false;if(t==null)return;foreach(var ae in t.cn){var a3=new List<IMyTerminalBlock>();if(ae.bN==a9.dX){
string j_= !string.IsNullOrEmpty(ae.br)? ae.br : ae.af;if(!aL.ContainsKey(j_)){aL[j_]=ae.fx;if(ae.hO!=null)c8[j_]=ae.hO;
}bU[ae.af]=a3;continue;}var pS=ae.a0.Count>0 ? ae.a0 : new List<string>{ae.mh};foreach(var cV in pS){
if(string.IsNullOrEmpty(cV))continue;bool oe=cV.Contains("*")||cV.Contains("?");if(ae.bN==a9.hX||oe){var jD=new List<IMyTerminalBlock>();
qC(cV,jD,ae.fo);if(jD.Count>0){foreach(var nZ in jD){if(!a3.Contains(nZ))a3.Add(nZ);}}else{var mz=fn(ae.fo);
for(int iO=0;iO<mz.Count;iO++){var hs=mz[iO];if(oe ? K.q0(cV,hs.CustomName): string.Equals(cV,hs.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!a3.Contains(hs))a3.Add(hs);}}}}else{var iT=lO(cV,ae.fo);if(iT!=null&& !a3.Contains(iT))a3.Add(iT);}}if(a3.Count==0){
ru(ae,pS,a3);if(a3.Count>0)fU[ae.af]=true;}bU[ae.af]=a3;}foreach(var bD in t.V){
if(string.IsNullOrEmpty(bD.bO))continue;var kq=lO(bD.bO);if(kq==null){ej.AppendLine($"HELM: output block '{bD.bO}' not found");
continue;}IMyTextSurface cU=null;var kK=kq as IMyTextSurfaceProvider;var nq=kq as IMyTextSurface;if(kK!=null&&bD.cu<kK.SurfaceCount){
cU=kK.GetSurface(bD.cu);}else if(nq!=null&&bD.cu==0){cU=nq;}if(cU==null){ej.AppendLine($"HELM: output block '{bD.bO}' has no surface index {bD.cu}");
continue;}cU.ContentType=ContentType.SCRIPT;cU.Script="";cU.ScriptBackgroundColor=Color.Black;bV[bD]=cU;}cC.Clear();
c9.Clear();}static readonly string[]lN={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool qQ(int s6,IMyTerminalBlock v){switch(s6){case 0: return v is IMyBatteryBlock;case 1: return v is IMyReactor;case 2: return v is IMySolarPanel;
case 3: return v is IMyWindTurbine;case 4: return v is IMyGasTank;case 5: return v is IMyAirVent;case 6: return v is IMyCargoContainer;case 7: return v is IMyJumpDrive;
case 8: return v is IMyThrust;case 9: return v is IMyMotorSuspension;case 10: return v is IMyDoor;case 11: return v is IMyShipConnector;case 12: return v is IMyAssembler;
case 13: return v is IMyRefinery;case 14: return v is IMyShipDrill;case 15: return v is IMyShipWelder;case 16: return v is IMyShipGrinder;case 17: return v is IMyGasGenerator;
case 18: return v is IMyLargeTurretBase;case 19: return v is IMyProjector;case 20: return v is IMyGyro;case 21: return v is IMyGravityGeneratorBase;case 22: return v is IMyLandingGear;
case 23: return v is IMyRadioAntenna;case 24: return v is IMyBeacon;case 25: return v is IMyLightingBlock;case 26: return v is IMyPistonBase;case 27: return v is IMyMotorStator;
case 28: return v is IMyWarhead;case 29: return v is IMySensorBlock;case 30: return v is IMyCameraBlock;case 31: return v is IMyParachute;case 32: return v is IMyTurretControlBlock;
}return false;}void ru(bq r7,List<string>uQ,List<IMyTerminalBlock>iV){string s0=(r7.af+" "+string.Join(" ",uQ)).ToLowerInvariant();
var j3=new List<int>();for(int gI=0;gI<lN.Length;gI++){foreach(var va in lN[gI].Split(','))if(s0.Contains(va)){j3.Add(gI);break;}}if(j3.Count==0)return;
var my=fn();var oR=new List<IMyTerminalBlock>();for(int jM=0;jM<my.Count;jM++){var iP=my[jM];foreach(int s7 in j3){if(qQ(s7,iP)){(iP.CubeGrid==Me.CubeGrid ? iV : oR).Add(iP);
break;}}}if(iV.Count==0)iV.AddRange(oR);}void rw(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(t==null||t.V.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {t.V.Count} screens, {t.cn.Count} binds");fR=false;
Echo($"Blocks: {fn().Count} on this construct; {iq} on docked ships (ignored)");cC.Clear();Echo("--- BIND RESOLUTION ---");foreach(var ar in t.cn){
if(ar.bN==a9.dX){string j0= !string.IsNullOrEmpty(ar.br)? ar.br : ar.af;double u0=aL.ContainsKey(j0)? aL[j0]: ar.fx;
Echo($"[OK] @bind {ar.af} (dynamic variable '{j0}' = {u0})");continue;}List<IMyTerminalBlock>iW;if(bU.TryGetValue(ar.af,out iW)){if(iW.Count>0){
string sF=fU.ContainsKey(ar.af)&&fU[ar.af]? " [fallback: typed match]" : "";Echo($"[OK] @bind {ar.af} ({iW.Count} blocks matched{sF})");
}else{string uR=string.Join(", ",ar.a0.Count>0 ? ar.a0 : new List<string>{ar.mh});Echo($"[WARN] @bind {ar.af} -> 0 blocks matched \"{uR}\"");
}}else{Echo($"[ERR] @bind {ar.af} -> unresolved");}}if(aL.Count>0){Echo($"--- DYNAMIC VARIABLES ({aL.Count}) ---");foreach(var oo in aL)Echo($"[VAR] {oo.Key} = {oo.Value}");
}IMyTextSurface iG=null;foreach(var tG in bV.Values){iG=tG;break;}if(iG!=null){var k5=new List<string>();iG.GetSprites(k5);
Echo($"--- SPRITES ({k5.Count}): names usable in bg_texture ---");Echo(string.Join(", ",k5.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bE in t.V){IMyTextSurface hl;if(bV.TryGetValue(bE,out hl)){var pz=hl.SurfaceSize;var uG=K.qE(bE);
Echo($"[OK] Screen \"{bE.be}\" [{bE.cv}] -> {bE.bO} [#{bE.cu}] ({pz.X:0}x{pz.Y:0} px, texture {hl.TextureSize.X:0}x{hl.TextureSize.Y:0}, {uG.ag} sprites)");
}else{Echo($"[ERR] Screen \"{bE.be}\" -> Missing block \"{bE.bO}\" [#{bE.cu}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double rv(IMyTerminalBlock w){
if(w==null)return 0.0;var f5=w as IMyBatteryBlock;if(f5!=null)return f5.MaxStoredPower>0 ?(f5.CurrentStoredPower/f5.MaxStoredPower)*100.0 : 0.0;
var gG=w as IMyJumpDrive;if(gG!=null)return gG.MaxStoredPower>0 ?(gG.CurrentStoredPower/gG.MaxStoredPower)*100.0 : 0.0;var pR=w as IMyGasTank;
if(pR!=null)return pR.FilledRatio*100.0;var qf=w as IMyAirVent;if(qf!=null)return qf.GetOxygenLevel()*100.0;var m3=w as IMyCargoContainer;if(m3!=null){
var gD=m3.GetInventory();if(gD!=null&&gD.MaxVolume.RawValue>0)return(double)gD.CurrentVolume.RawValue/gD.MaxVolume.RawValue*100.0;return 0.0;}var g3=w as IMyReactor;
if(g3!=null)return g3.MaxOutput>0 ?(g3.CurrentOutput/g3.MaxOutput)*100.0 : 0.0;var he=w as IMySolarPanel;if(he!=null)return he.MaxOutput>0 ?(he.CurrentOutput/he.MaxOutput)*100.0 : 0.0;
var hA=w as IMyWindTurbine;if(hA!=null)return hA.MaxOutput>0 ?(hA.CurrentOutput/hA.MaxOutput)*100.0 : 0.0;var nr=w as IMyDoor;if(nr!=null)return(double)nr.OpenRatio*100.0;
var hv=w as IMyThrust;if(hv!=null)return hv.MaxEffectiveThrust>0 ?(hv.CurrentThrust/hv.MaxEffectiveThrust)*100.0 : 0.0;var lg=w as IMyMotorSuspension;
if(lg!=null)return lg.Brake ? 100.0 :(lg.Friction*100.0);var jc=w as IMyShipConnector;if(jc!=null)return jc.Status==MyShipConnectorStatus.Connected ? 100.0 :(jc.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var o6=w as IMyProductionBlock;if(o6!=null)return o6.IsProducing ? 100.0 : 0.0;var nv=w as IMyShipDrill;if(nv!=null)return nv.Enabled ? 100.0 : 0.0;
var ln=w as IMyShipToolBase;if(ln!=null)return ln.IsActivated ? 100.0 :(ln.Enabled ? 50.0 : 0.0);var jA=w as IMyGasGenerator;if(jA!=null)return jA.AutoRefill ? 100.0 :(jA.Enabled ? 50.0 : 0.0);
var hw=w as IMyLargeTurretBase;if(hw!=null)return hw.IsShooting ? 100.0 :(hw.HasTarget ? 50.0 :(hw.Enabled ? 10.0 : 0.0));var e_=w as IMyProjector;
if(e_!=null)return e_.TotalBlocks>0 ?((double)(e_.TotalBlocks-e_.RemainingBlocks)/e_.TotalBlocks)*100.0 : 0.0;var jG=w as IMyGyro;if(jG!=null)return jG.GyroOverride ? 100.0 :(jG.Enabled ? 50.0 : 0.0);
var n0=w as IMyGravityGeneratorBase;if(n0!=null)return Math.Min(100.0,Math.Abs(n0.GravityAcceleration)/9.81*100.0);var jz=w as IMyLandingGear;if(jz!=null)return jz.IsLocked ? 100.0 :(jz.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var mA=w as IMyRadioAntenna;if(mA!=null)return(mA.Radius/50000.0)*100.0;var mN=w as IMyBeacon;if(mN!=null)return(mN.Radius/50000.0)*100.0;
var ox=w as IMyLightingBlock;if(ox!=null)return ox.Intensity*10.0;var gZ=w as IMyPistonBase;if(gZ!=null)return gZ.HighestPosition>0 ?(gZ.CurrentPosition/gZ.HighestPosition)*100.0 : 0.0;
var pe=w as IMyMotorStator;if(pe!=null)return Math.Min(100.0,Math.Abs(pe.Angle)/6.28318*100.0);var qi=w as IMyWarhead;if(qi!=null)return qi.IsArmed ? 100.0 : 0.0;
var k_=w as IMySensorBlock;if(k_!=null)return!k_.LastDetectedEntity.IsEmpty()? 100.0 :(k_.Enabled ? 25.0 : 0.0);var i4=w as IMyCameraBlock;
if(i4!=null)return i4.IsActive ? 100.0 :(i4.Enabled ? 50.0 : 0.0);var m9=w as IMyParachute;if(m9!=null)return(double)m9.OpenRatio*100.0;
var hu=w as IMyTurretControlBlock;if(hu!=null)return hu.HasTarget ? 100.0 :(hu.IsUnderControl ? 50.0 :(hu.Enabled ? 10.0 : 0.0));var nQ=w as IMyFunctionalBlock;
if(nQ!=null)return nQ.Enabled ? 100.0 : 0.0;return 0.0;}static double qY(IMyTerminalBlock mI,string jr){int m2=jr.IndexOf(':');string j2=jr.Substring(0,m2).ToLowerInvariant();
var uY=new MyItemType("MyObjectBuilder_"+(j2=="ammo" ? "AmmoMagazine" : char.ToUpper(j2[0])+j2.Substring(1)),jr.Substring(m2+1));double pK=0;for(int jN=0;
jN<mI.InventoryCount;jN++)pK+=(double)mI.GetInventory(jN).GetItemAmount(uY);return pK;}double l9(IMyTerminalBlock x,string nH){if(x==null)return 0.0;
string b=nH.ToLowerInvariant();if(b=="percent"||b=="pct")return rv(x);if(b.IndexOf(':')>0)return qY(x,nH);var b_=x as IMyBatteryBlock;
if(b_!=null){if(b=="current"||b=="stored"||b=="val"||b=="value"||b=="raw")return b_.CurrentStoredPower;if(b=="max"||b=="capacity")return b_.MaxStoredPower;
if(b=="output")return b_.CurrentOutput;if(b=="maxoutput")return b_.MaxOutput;if(b=="input")return b_.CurrentInput;if(b=="maxinput")return b_.MaxInput;
return b_.CurrentStoredPower;}var jZ=x as IMyJumpDrive;if(jZ!=null){if(b=="max"||b=="capacity")return jZ.MaxStoredPower;return jZ.CurrentStoredPower;
}var hp=x as IMyGasTank;if(hp!=null){if(b=="max"||b=="capacity")return hp.Capacity;return hp.FilledRatio*hp.Capacity;}var qg=x as IMyAirVent;
if(qg!=null){if(b=="max"||b=="capacity")return 100.0;return qg.GetOxygenLevel()*100.0;}var m4=x as IMyCargoContainer;if(m4!=null){var gE=m4.GetInventory();
if(gE!=null){if(b=="mass")return(double)gE.CurrentMass;if(b=="max"||b=="capacity")return(double)gE.MaxVolume;return(double)gE.CurrentVolume;}return 0.0;}var kL=x as IMyPowerProducer;
if(kL!=null){if(b=="max"||b=="capacity")return kL.MaxOutput;return kL.CurrentOutput;}var ns=x as IMyDoor;if(ns!=null){if(b=="max")return 1.0;return(double)ns.OpenRatio*100.0;
}var lk=x as IMyThrust;if(lk!=null){if(b=="max"||b=="capacity")return lk.MaxEffectiveThrust;return lk.CurrentThrust;}var ci=x as IMyMotorSuspension;
if(ci!=null){if(b=="brake")return ci.Brake ? 100.0 : 0.0;if(b=="height")return ci.Height;if(b=="strength")return ci.Strength;if(b=="power")return ci.Power;
if(b=="steer")return ci.SteeringOverride*100.0;if(b=="propulsion")return ci.PropulsionOverride*100.0;return ci.Friction;}var nh=x as IMyShipConnector;
if(nh!=null){return nh.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var kI=x as IMyProductionBlock;if(kI!=null){if(b=="producing"||b=="active"||b=="running"||b=="state")return kI.IsProducing ? 100.0 : 0.0;
if(b=="queue")return kI.IsQueueEmpty ? 0.0 : 1.0;}var iK=x as IMyAssembler;if(iK!=null){if(b=="disassembling")return iK.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(b=="repeating"||b=="loop")return iK.Repeating ? 100.0 : 0.0;}var jB=x as IMyGasGenerator;if(jB!=null){if(b=="autorefill")return jB.AutoRefill ? 100.0 : 0.0;
if(b=="active")return jB.Enabled ? 100.0 : 0.0;}var hx=x as IMyLargeTurretBase;if(hx!=null){if(b=="shooting")return hx.IsShooting ? 100.0 : 0.0;
if(b=="target"||b=="hastarget")return hx.HasTarget ? 100.0 : 0.0;if(b=="range")return hx.Range;}var cb=x as IMyProjector;if(cb!=null){if(b=="built")return cb.TotalBlocks-cb.RemainingBlocks;
if(b=="remaining")return cb.RemainingBlocks;if(b=="total")return cb.TotalBlocks;if(b=="projecting")return cb.IsProjecting ? 100.0 : 0.0;if(b=="armor")return cb.RemainingArmorBlocks;
if(b=="buildable")return cb.BuildableBlocksCount;}var lo=x as IMyShipToolBase;if(lo!=null){if(b=="working"||b=="active"||b=="activated")return lo.IsActivated ? 100.0 : 0.0;
return lo.Enabled ? 100.0 : 0.0;}var dn=x as IMyGyro;if(dn!=null){if(b=="override")return dn.GyroOverride ? 100.0 : 0.0;if(b=="pitch")return dn.Pitch;
if(b=="yaw")return dn.Yaw;if(b=="roll")return dn.Roll;if(b=="power")return dn.GyroPower*100.0;}var n1=x as IMyGravityGeneratorBase;if(n1!=null){if(b=="g"||b=="accel")return n1.GravityAcceleration;
}var n_=x as IMyLandingGear;if(n_!=null){if(b=="locked")return n_.IsLocked ? 100.0 : 0.0;}var mB=x as IMyRadioAntenna;if(mB!=null){if(b=="radius"||b=="range")return mB.Radius;
}var mO=x as IMyBeacon;if(mO!=null){if(b=="radius"||b=="range")return mO.Radius;}var kb=x as IMyLightingBlock;if(kb!=null){if(b=="intensity")return kb.Intensity;
if(b=="radius")return kb.Radius;}var g_=x as IMyPistonBase;if(g_!=null){if(b=="pos"||b=="position"||b=="extension")return g_.CurrentPosition;
if(b=="velocity")return g_.Velocity;if(b=="max")return g_.HighestPosition;}var e6=x as IMyMotorStator;if(e6!=null){if(b=="angle"||b=="deg")return e6.Angle*180.0/Math.PI;
if(b=="rad")return e6.Angle;if(b=="velocity"||b=="rpm")return e6.TargetVelocityRPM;if(b=="locked")return e6.RotorLock ? 100.0 : 0.0;}var lx=x as IMyWarhead;
if(lx!=null){if(b=="armed")return lx.IsArmed ? 100.0 : 0.0;if(b=="countdown")return lx.DetonationTime;}var pv=x as IMySensorBlock;if(pv!=null){
if(b=="detected")return!pv.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var i5=x as IMyCameraBlock;if(i5!=null){if(b=="range")return i5.AvailableScanRange;
if(b=="active")return i5.IsActive ? 100.0 : 0.0;}var na=x as IMyParachute;if(na!=null){if(b=="open"||b=="openratio")return(double)na.OpenRatio*100.0;
}var dJ=x as IMyTurretControlBlock;if(dJ!=null){if(b=="target"||b=="hastarget")return dJ.HasTarget ? 100.0 : 0.0;if(b=="azimuth")return dJ.AzimuthRotor!=null ? dJ.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(b=="elevation")return dJ.ElevationRotor!=null ? dJ.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var nR=x as IMyFunctionalBlock;if(nR!=null){return nR.Enabled ? 100.0 : 0.0;
}return 0.0;}double bu(string er,string nI){bq b0;ef.TryGetValue(er,out b0);if(b0!=null&&b0.bN==a9.dX){
string s8= !string.IsNullOrEmpty(b0.br)? b0.br : er;double nk;if(aL.TryGetValue(s8,out nk))return nk;return b0.fx;
}double ny;if(aL.TryGetValue(er,out ny))return ny;List<IMyTerminalBlock>es;if(!bU.TryGetValue(er,out es)||es.Count==0)throw new InvalidOperationException($"HELM: bind '{er}' resolved to zero blocks — check the name/typo");
if(es.Count==1)return l9(es[0],nI);var qe=new List<double>();foreach(var rX in es)qe.Add(l9(rX,nI));return qt.qs(b0!=null ? b0.dM : aI.hE,qe);
}double ic(string r9){return bu(r9,"percent");}static string bc(double qa,bs du){if(du==null||(string.IsNullOrEmpty(du.ec)&&string.IsNullOrEmpty(du.dY)&&du.L=="percent")){
return lT(qa);}return K.lS(qa,du.ec,du.dY);}bool rp(z cd){IMyTextSurface fa;if(!bV.TryGetValue(cd,out fa))return false;
if(cd.ck!=null)return ri(cd,fa);if(cd.h0!=null)return ro(cd,fa);if(cd.c0.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return rq(cd,fa);}return rr(cd,fa);}Color ay(bs op,Color su){return op.ik!=null ? F(op.ik): su;
}Color fB(string[]dG,double tP){if(dG.Length==1)return F(dG[0]);double o9=Math.Max(0.0,Math.Min(100.0,tP))/100.0*(dG.Length-1);int jO=Math.Min((int)o9,dG.Length-2);
float hn=(float)(o9-jO);Color bY=F(dG[jO]),f4=F(dG[jO+1]);return new Color((int)(bY.R+(f4.R-bY.R)*hn),(int)(bY.G+(f4.G-bY.G)*hn),(int)(bY.B+(f4.B-bY.B)*hn),(int)(bY.A+(f4.A-bY.A)*hn));
}static Color[]fj(ax sd,string[]uJ,string sw,float qn,float qp,float lu,float jH){int ep=16;bool n8=sw=="horizontal";var jb=new Color[ep];
for(int cK=0;cK<ep;cK++){p jy=K.lU(uJ,100.0*cK/(ep-1));jb[cK]=new Color(jy.bQ,jy.bM,jy.bG);float m1=lu/ep,mV=jH/ep;r(sd,n8 ? new Vector2(qn+m1*(cK+0.5f),qp+jH/2f): new Vector2(qn+lu/2f,qp+mV*(cK+0.5f)),n8 ? new Vector2(m1+1f,jH): new Vector2(lu,mV+1f),jb[cK]);
}return jb;}static void r(ax se,Vector2 t0,Vector2 uz,Color sj,float uj=0f,string sv="SquareSimple"){se.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=sv,Position=t0,Size=uz,Color=sj,RotationOrScale=uj}
);}void fk(ax b1,string cT,string[]aN,string nd,bool pC,float aE,float aU,float bC,float bf){cT=cT.ToLowerInvariant();float S=cT=="heavy" ? 4f : 2f,fi=bC-aE,jI=bf-aU;
Color bw=F(aN!=null&&aN.Length>0 ? aN[0]: nd ?? "40, 60, 85");Color b2=aN!=null&&aN.Length>1 ? F(aN[aN.Length-1]): bw;Action<float,float,float,float,Color>aR=(vg,vk,o8,o7,sg)=>r(b1,new Vector2(vg+o8/2f,vk+o7/2f),new Vector2(o8,o7),sg);
if(cT=="corners"){float aP=Math.Max(10f,Math.Min(fi,jI)*0.15f);aR(aE,aU,aP,S,bw);aR(aE,aU,S,aP,bw);aR(bC-aP,aU,aP,S,b2);aR(bC-S,aU,S,aP,b2);aR(aE,bf-S,aP,S,bw);aR(aE,bf-aP,S,aP,bw);
aR(bC-aP,bf-S,aP,S,b2);aR(bC-S,bf-aP,S,aP,b2);return;}if(aN!=null&&aN.Length>1&&pC){fj(b1,aN,"horizontal",aE,aU,fi,S);fj(b1,aN,"horizontal",aE,bf-S,fi,S);
}else{aR(aE,aU,fi,S,bw);aR(aE,bf-S,fi,S,b2);}if("|frame|single|heavy|double|rounded|".Contains("|"+cT+"|")){aR(aE,aU,S,jI,bw);aR(bC-S,aU,S,jI,b2);}if(cT=="rounded"){
float b3=S*3f;r(b1,new Vector2(aE,aU),new Vector2(b3,b3),bw,0f,"Circle");r(b1,new Vector2(bC,aU),new Vector2(b3,b3),b2,0f,"Circle");r(b1,new Vector2(aE,bf),new Vector2(b3,b3),bw,0f,"Circle");
r(b1,new Vector2(bC,bf),new Vector2(b3,b3),b2,0f,"Circle");}if(cT=="double")fk(b1,"single",aN,nd,pC,aE+5f,aU+5f,bC-5f,bf-5f);}void qA(z n,IMyTextSurface lc,ax g){
g.f.Clear();g.d8.Clear();g.bP.Clear();Vector2 Z=lc.SurfaceSize;Vector2 uV=lc.TextureSize;Vector2 bp=(uV-Z)/2f;
g.h4=bp;float gS=Math.Max(4f,Z.X*0.02f);float eX=Math.Max(4f,Z.Y*0.02f);if(n.cv=="compact"||(n.cv=="auto"&&Z.Y<=260f&&Z.Y>150f)){
gS=4f;eX=4f;}else if(n.cv=="corner"||(n.cv=="auto"&&Z.Y<=150f)){gS=2f;eX=2f;}float p3=Z.X-(gS*2f);
Color dd=new Color(10,12,16);bool nu=true;if(!string.IsNullOrEmpty(n.bH)){var eD=p.d5(n.bH);if(eD.fF)nu=false;
else dd=new Color(eD.bQ,eD.bM,eD.bG,eD.hC);}g.cm= -1;g.fm=false;g.fl=F(n.lC ?? "#5b0f0f");g.dQ.Clear();if(n.bK!=null&&n.bK.Length>1){
g.cm=g.f.Count;g.fp=fj(g,n.bK,n.dP,bp.X+2f,bp.Y+2f,Z.X-4f,Z.Y-4f);
}else if(nu||n.aX!=null||n.c1!=null){if(n.aX!=null)dd=F(n.aX.First(t5=>t5.c3).cq);g.cm=g.f.Count;
g.fp=new[]{dd};r(g,bp+(Z/2f),Z-new Vector2(4f,4f),dd);if(n.aX!=null||n.c1!=null)g.dQ.Add(new fq{
fE=g.cm,hF=dd,fw=dd,fN=n.aX,hJ=n.hG,hR=n.hH,fA=n.c1,hS=n.hT,hU=n.hV,lZ=true}
);}if(!string.IsNullOrEmpty(n.hI))r(g,bp+(Z/2f),Z-new Vector2(4f,4f),F(n.lE ?? "255,255,255,24"),0f,n.hI);
if(!n.c0.Equals("none",StringComparison.OrdinalIgnoreCase)){float mX=bp.X+(Z.X-p3)/2f;fk(g,n.c0,n.fv,n.dS,!n.fD,mX-2f,bp.Y+eX*0.5f-1f,mX+p3+2f,bp.Y+Z.Y-eX*0.5f+1f);
}var ky=n.ak;if(ky.Count==0){ky=new List<aD>{new aD{c6="main",dU=0,d7=0,d9=n.dZ,ea=n.d_,ao=n.fI}
};}float n3=8f;foreach(var q in ky){var gR=K.qD(q.dU,q.d7,q.d9,q.ea,n.dZ,n.d_,Z.X,Z.Y,gS,eX,n3,n3);
float az=bp.X+gR.mn;float bA=bp.Y+gR.mo;float aQ=gR.mm;float cN=gR.lW;g.bP.Add(new h8{l3=q.c6,id=g.f.Count,l8=new Vector2(az,bA),mc=new Vector2(aQ,cN)}
);bool ph=q.aX!=null||q.c1!=null;if(q.bK!=null&&q.bK.Length>1){fj(g,q.bK,q.dP!="vertical" ? q.dP : n.dP,az,bA,aQ,cN);
}else{Color ca;if(q.aX!=null)ca=F(q.aX.First(t6=>t6.c3).cq);else if(!string.IsNullOrEmpty(q.cZ))ca=F(q.cZ);
else if(!string.IsNullOrEmpty(n.h7))ca=F(n.h7);else ca=Color.Transparent;if(ca.A>0||ph){if(ph)g.dQ.Add(new fq{fE=g.f.Count,hF=ca,fw=ca,fN=q.aX,hJ=q.hG,hR=q.hH,fA=q.c1,hS=q.hT,hU=q.hV}
);r(g,new Vector2(az+aQ/2f,bA+cN/2f),new Vector2(aQ,cN),ca);}}if(q.dR!=null&& !q.dR.Equals("none",StringComparison.OrdinalIgnoreCase))fk(g,q.dR,q.fu ?? n.fv,q.ft ?? n.dS,!n.fD,az+1f,bA+1f,az+aQ-1f,bA+cN-1f);
string gs=q.lP ?? n.lQ ?? "Monospace";float r5=K.qT(n.cv,Z.X,Z.Y,n.c2);float aM=q.lR ?? r5;
float eE=0f;if(!string.IsNullOrEmpty(q.cA)){eE=20f*aM;var iD=TextAlignment.LEFT;float ll=az+4f;string mt=q.ii ?? n.mi ?? "left";
if(mt.Equals("center",StringComparison.OrdinalIgnoreCase)){iD=TextAlignment.CENTER;ll=az+(aQ/2f);}else if(mt.Equals("right",StringComparison.OrdinalIgnoreCase)){
iD=TextAlignment.RIGHT;ll=az+aQ-4f;}string pU=q.ij ?? n.mj;Color uW= !string.IsNullOrEmpty(pU)? F(pU): F(q.ft ?? n.dS ?? "white");
var pZ=MySprite.CreateText(q.cA,gs,uW,aM*0.9f,iD);pZ.Position=new Vector2(ll,bA+2f);g.f.Add(pZ);
string[]iQ=q.fu ?? n.fv;Color sy=iQ!=null&&iQ.Length>0 ? F(iQ[0]): F(q.ft ?? n.dS ?? "40, 60, 85");
r(g,new Vector2(az+aQ/2f,bA+eE-2f),new Vector2(aQ-8f,1f),sy);}if(q.h5=="vertical"||q.ao.Any(tX=>tX.h==e.fP)){
qB(g,lc,q,gs,aM,az,bA,aQ,cN,eE);continue;}int vm=Math.Max(1,q.ao.Count);float tN=cN-eE-8f;
float qk=0f,mv=0f;foreach(var tY in q.ao)qk+=1<<tY.fO;float uZ=tN/Math.Max(1f,qk);float gC=6f;for(int jP=0;jP<q.ao.Count;
jP++){var k=q.ao[jP];var U=new ct{fH=k};float g8=uZ*(1<<k.fO);float uk=bA+eE+4f+mv;mv+=g8;float J=uk+(g8/2f);
float pW=J-(11f*aM);U.c4=g.f.Count;U.lV=k.aa!=null&&(k.aa.Contains("{val}")||k.aa.Contains("{percent}"));
var ou=MySprite.CreateText(k.aa,gs,k.d2!=null ? F(k.d2): Color.White,aM,TextAlignment.LEFT);ou.Position=new Vector2(az+gC,pW);
g.f.Add(ou);float tm=Math.Max(24f*aM,(k.aa.Length+1)*12f*aM);float u7=(!string.IsNullOrEmpty(k.ec)|| !string.IsNullOrEmpty(k.dY))? 64f*aM : 52f*aM;
if(k.h!=e.ig){float H=az+gC+tm;float r4=az+aQ-gC-u7;float I=Math.Max(16f,r4-H);
float M=Math.Max(k.h==e.cx ? 8f : 4f,Math.Min((k.h==e.cx ? 20f : 16f)*aM,g8*(k.h==e.cx ? 0.55f : 0.45f)));
if(k.fO>0)M=g8*0.7f;Color p2=ay(k,new Color(25,30,38));U.bI=H;U.a8=I;U.a7=M;U.cl=J;
if(k.h==e.dO){r(g,new Vector2(H+(I/2f),J),new Vector2(I,M),p2);U._=g.f.Count;
r(g,new Vector2(H,J),new Vector2(0f,M),F(k.aw));bool uh=k.fy=="left";for(int lm=1;lm<k.ih;lm++)r(g,new Vector2(H+I*lm/k.ih,J),new Vector2(1f,M),new Color(0,0,0,150));
if(k.fJ!=null)foreach(double oC in k.fJ)r(g,new Vector2(H+I*(float)(uh ? 1.0-oC/100.0 : oC/100.0),J),new Vector2(2f,M+6f),Color.White);
if(k.h6!=null)fk(g,"single",null,k.h6,true,H,J-M/2f,H+I,J+M/2f);}else if(k.h==e.d4){
int kY=k.cw>0 ? k.cw : 10;float nT=2f;float kZ=Math.Max(2f,(I-(nT*(kY-1)))/kY);for(int kS=0;kS<kY;kS++){U.N.Add(g.f.Count);
r(g,new Vector2(H+kS*(kZ+nT)+(kZ/2f),J),new Vector2(kZ,M),ay(k,new Color(22,26,32)));}}else if(k.h==e.cx){
r(g,new Vector2(H+(I/2f),J),new Vector2(I,M),ay(k,new Color(20,24,30)));int g0=k.cw>0 ? k.cw : 8;
U.cy=new float[g0];float nU=2f;float i9=Math.Max(2f,(I-(nU*(g0-1)))/g0);for(int ku=0;ku<g0;ku++){U.N.Add(g.f.Count);
r(g,new Vector2(H+ku*(i9+nU)+(i9/2f),J),new Vector2(i9,2f),F(k.aw));}}else if(k.h==e.d0){r(g,new Vector2(H+(I/2f),J),new Vector2(I,M),ay(k,new Color(20,24,30)));
int k8=10;float nV=2f;float k9=Math.Max(2f,(I-(nV*(k8-1)))/k8);for(int kT=0;kT<k8;kT++){U.N.Add(g.f.Count);
r(g,new Vector2(H+kT*(k9+nV)+(k9/2f),J),new Vector2(k9,M),ay(k,new Color(24,28,36)),0.523599f);}}else if(k.h==e.dT){
int iI=10;float nW=2f;float iJ=Math.Max(2f,(I-(nW*(iI-1)))/iI);for(int kU=0;kU<iI;kU++){U.N.Add(g.f.Count);
r(g,new Vector2(H+kU*(iJ+nW)+(iJ/2f),J),new Vector2(iJ,M),ay(k,new Color(24,28,36)),1.570796f,"Triangle");}}else if(k.h==e.dW){
int jf=k.cw>0 ? k.cw : 12;float nX=2f;float jh=Math.Max(2f,(I-(nX*(jf-1)))/jf);float nt=Math.Min(jh,M);for(int kV=0;
kV<jf;kV++){U.N.Add(g.f.Count);r(g,new Vector2(H+kV*(jh+nX)+(jh/2f),J),new Vector2(nt,nt),ay(k,new Color(24,28,36)),0f,"Circle");
}}else if(k.h==e.c_){r(g,new Vector2(H+(I/2f),J),new Vector2(I,M),p2);U._=g.f.Count;
r(g,new Vector2(H+(I/2f),J),new Vector2(0f,M),F(k.aw));r(g,new Vector2(H+(I/2f),J),new Vector2(2f,M+4f),new Color(80,110,150));
}else if(k.h==e.cB){r(g,new Vector2(H+(I/2f),J),new Vector2(I,M),ay(k,new Color(20,24,30)));U._=g.f.Count;
float j4=Math.Max(4f,M);r(g,new Vector2(H+j4*0.5f,J),new Vector2(j4,j4),new Color(40,50,65),0f,"Circle");}else if(k.h==e.cp){
float de=Math.Max(6f,Math.Min(M,14f*aM));r(g,new Vector2(H+de*0.5f,J),new Vector2(de,de),new Color(28,34,46));
U._=g.f.Count;r(g,new Vector2(H+de*0.5f,J),new Vector2(de*0.6f,de*0.6f),new Color(40,50,65));}else if(k.h==e.cz){
r(g,new Vector2(H+(I/2f),J),new Vector2(I,M),ay(k,new Color(18,22,30)));U._=g.f.Count;float kF=Math.Max(4f,M*0.7f);
r(g,new Vector2(H+kF+2f,J),new Vector2(kF,kF),new Color(34,197,94),0f,"Circle");}else if(k.h==e.cY){int kE=8;
float nY=2f;float kG=Math.Max(2f,(I-(nY*(kE-1)))/kE);for(int kv=0;kv<kE;kv++){U.N.Add(g.f.Count);r(g,new Vector2(H+kv*(kG+nY)+(kG/2f),J),new Vector2(kG,M),ay(k,new Color(24,28,36)));
}}else if(k.h==e.cr){r(g,new Vector2(H+(I/2f),J),new Vector2(I,M),ay(k,new Color(20,24,30)));U._=g.f.Count;
float ob=Math.Max(4f,M);r(g,new Vector2(H+(I/2f),J),new Vector2(ob,ob),F(k.aw),0f,"Triangle");}}var p8=MySprite.CreateText("---",gs,Color.White,aM,TextAlignment.RIGHT);
p8.Position=new Vector2(az+aQ-gC,pW);U.a1=g.f.Count;g.f.Add(p8);g.d8.Add(U);}}g.d3=Z;
g.d1=true;}const string rE="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bS=8;void qB(ax ac,IMyTextSurface uM,aD kx,string nO,float eB,float tK,float kw,float tL,float oU,float s_){
int tA=Math.Max(1,kx.ao.Count);float ne=(tL-12f)/tA;float hy=22f*eB;float bn=kw+s_+4f+hy;float aV=Math.Max(20f,kw+oU-4f-hy-bn);
float sr=uM.MeasureStringInPixels(new StringBuilder("M"),nO,eB*0.9f).X;for(int gB=0;gB<kx.ao.Count;gB++){var cL=kx.ao[gB];float bi=tK+6f+ne*(gB+0.5f);
float cW=ne*0.7f;var bk=new ct{fH=cL,mk=true,bI=bi,a8=cW,a7=aV,cl=bn+aV/2f,lK=sr};bk.c4=ac.f.Count;
a6(ac.f,cL.aa,cL.d2!=null ? F(cL.d2): Color.White,bi,kw+oU-4f-hy,eB);bk.a1=ac.f.Count;
a6(ac.f,"---",Color.White,bi,bn-hy,eB);var oq=ac.f[bk.c4];oq.Alignment=TextAlignment.CENTER;ac.f[bk.c4]=oq;
var p7=ac.f[bk.a1];p7.Alignment=TextAlignment.CENTER;ac.f[bk.a1]=p7;r(ac,new Vector2(bi,bn+aV/2f),new Vector2(cW,aV),ay(cL,new Color(25,30,38)));
bk._=ac.f.Count;if(cL.h==e.fP){for(int mJ=0;mJ<bS;mJ++){bk.N.Add(ac.f.Count);r(ac,new Vector2(bi,bn+aV),new Vector2(cW-6f,0f),Color.White);
}for(int lv=0;lv<4;lv++){bk.N.Add(ac.f.Count);var pP=MySprite.CreateText("",nO,lv==0 ? new Color(255,255,255,170): new Color(255,255,255,110),eB*0.9f,TextAlignment.CENTER);
pP.Position=new Vector2(bi,bn+aV);ac.f.Add(pP);}Color jj=new Color(90,110,135);r(ac,new Vector2(bi-cW/2f,bn+aV/2f),new Vector2(3f,aV),jj);r(ac,new Vector2(bi+cW/2f,bn+aV/2f),new Vector2(3f,aV),jj);
r(ac,new Vector2(bi,bn+aV),new Vector2(cW+3f,3f),jj);}else{r(ac,new Vector2(bi,bn+aV),new Vector2(cW,0f),F(cL.aw));}ac.d8.Add(bk);
}}bool rD(ax bg,ct y,double fg,double pa,Color ge,bool qc){var a4=y.fH;float mQ=y.E;if(y.E<0f)y.E=(float)fg;
y.E=Math.Abs(y.E-(float)fg)>0.05f ? y.E+((float)fg-y.E)*0.35f :(float)fg;float cM=Math.Max(0f,Math.Min(1f,y.E/100f))*y.a7;
float gb=y.cl+y.a7/2f;float gi=y.bI;if(a4.h!=e.fP){var gn=bg.f[y._];gn.Size=new Vector2(y.a8,cM);
gn.Position=new Vector2(gi,gb-cM/2f);gn.Color=ge;bg.f[y._]=gn;if(qc||y.E!=mQ)aJ(bg,y,bc(pa,a4),ge);
return qc||y.E!=mQ;}float iS=y.a7/bS;string e1=a4.bL!=null ? K.d6(a4.bL,a4.dV!=null ? bu(a4.dV,a4.hN): fg): null;
Color tu=e1!=null ? F(e1): ge;string[]pH=e1!=null ?(e1.Contains("/")? e1.Split('/'): null): a4.cs&&a4.bd.Length>1 ? a4.bd : null;
for(int cG=0;cG<bS;cG++){float qh=Math.Max(0f,Math.Min(iS,cM-cG*iS));var g1=bg.f[y.N[cG]];g1.Size=new Vector2(y.a8-6f,qh);
g1.Position=new Vector2(gi,gb-cG*iS-qh/2f);g1.Color=pH!=null ? fB(pH,100.0*(cG+0.5)/bS): tu;bg.f[y.N[cG]]=g1;}
int sh=Math.Max(1,(int)((y.a8-8f)/y.lK));bool uw=cM>6f;var ly=bg.f[y.N[bS]];ly.Data=uw ? rE.Substring((int)(ab*5.0)%6,Math.Min(sh,100)): "";
ly.Position=new Vector2(gi,gb-cM-12f);bg.f[y.N[bS]]=ly;for(int b8=0;b8<3;b8++){var i0=bg.f[y.N[bS+1+b8]];
double tT=(ab*(0.35+0.1*b8)+b8*0.37)%1.0;i0.Data=cM>24f ?(b8==1 ? "O" : "o"): "";i0.Position=new Vector2(gi+(b8-1)*y.a8*0.26f,gb-(float)tT*(cM-6f)-10f);
bg.f[y.N[bS+1+b8]]=i0;}aJ(bg,y,bc(pa,a4),ge);return true;}static void aJ(ax mZ,ct pA,string uU,Color sk){
var ls=mZ.f[pA.a1];ls.Data=uU;ls.Color=sk;mZ.f[pA.a1]=ls;}static void mb(ax m_,ct pB,Color sl){
var nC=m_.f[pB._];nC.Color=sl;m_.f[pB._]=nC;}bool rr(z bF,IMyTextSurface ld){ax j;
if(!bW.TryGetValue(bF,out j)){j=new ax();bW[bF]=j;}bool pd= !j.d1||j.d3!=ld.SurfaceSize;
if(pd){qA(bF,ld,j);}bool O=pd;bool f0=(ab%bF.hD)<bF.hD/2.0;double cJ= -1.0,ni;if(fS.TryGetValue(bF,out ni)){
cJ=(ab-ni)/bF.hL;if(cJ>=1.0){fS.Remove(bF);cJ=1.0;}}bool lw=cD&&f0&&j.fl.A>0;if(j.cm>=0&&lw!=j.fm){
j.fm=lw;for(int eG=0;eG<j.fp.Length;eG++){var mR=j.f[j.cm+eG];mR.Color=lw ? j.fl : j.fp[eG];j.f[j.cm+eG]=mR;
}O=true;foreach(var r6 in j.dQ)r6.hW=true;}foreach(var al in j.dQ){if(al.lZ&&j.fm)continue;Color eq=al.hF;try{if(al.fN!=null){
double m0=bu(al.hJ,al.hR);string g6=K.d6(al.fN,m0);eq=g6!=null&&g6.Contains("/")? fB(g6.Split('/'),m0): F(g6);
}if(al.fA!=null&&f0&&K.d6(al.fA,bu(al.hS,al.hU))!=null)eq=j.fl;}catch{}if(eq!=al.fw||al.hW){
al.fw=eq;al.hW=false;var mY=j.f[al.fE];mY.Color=eq;j.f[al.fE]=mY;O=true;}}for(int jQ=0;jQ<j.d8.Count;jQ++){var c=j.d8[jQ];
var i=c.fH;double D;bool n5=false;double ki=i.fK;try{D=bu(i.aY,i.L);if(i.c5!=null)ki=bu(i.c5,i.h_);
}catch{n5=true;D=0.0;}if(n5){if(c.fG!= -99999.0||(c.a1>=0&&j.f[c.a1].Data!="ERR")){O=true;c.fG= -99999.0;
if(c.a1>=0){var kW=j.f[c.a1];kW.Data="ERR";kW.Color=Color.Red;j.f[c.a1]=kW;}}continue;}double a5=D;
if(ki>0)D=Math.Min(100.0,a5/ki*100.0);bool pM=cJ>=0.0&&i.h!=e.ig&&i.h!=e.cB&&i.h!=e.cp&&i.h!=e.cz&&i.h!=e.cr;
if(pM)D=cJ<0.5 ? cJ*200.0 : 100.0+(D-100.0)*(cJ-0.5)*2.0;double gh=i.dV!=null ? bu(i.dV,i.hN): D;string gf=i.bL!=null ? K.d6(i.bL,gh): i.cs ? K.fC(i.bd,gh): i.aw;
Color W=F(gf);bool bx=(W==Color.Red);if(gf!=null&&gf.Contains("/"))W=fB(gf.Split('/'),gh);if(i.bL==null&&i.cs&& !bF.fD&&i.bd.Length>1)W=fB(i.bd,gh);
if(bx&&f0){W=new Color(255,75,75);O=true;}bool aW=pM||Math.Abs(a5-c.fG)>0.1;if(aW)c.fG=a5;
if(c.mk){if(rD(j,c,D,a5,W,aW))O=true;continue;}if(i.h==e.dO){double nJ=(i.hY||i.L=="percent"||i.L=="pct")? D : ic(i.aY);
if(c.E<0f)c.E=(float)nJ;float hq=(float)nJ;if(Math.Abs(c.E-hq)>0.05f){c.E+=(hq-c.E)*0.35f;
O=true;}else if(c.E!=hq){c.E=hq;O=true;}if(aW)O=true;if(O){float tQ=(float)Math.Max(0.0,Math.Min(1.0,c.E/100.0));
float jv=c.a8*tQ;var gp=j.f[c._];gp.Size=new Vector2(jv,c.a7);gp.Position=new Vector2(i.fy=="left" ? c.bI+c.a8-(jv/2f): c.bI+(jv/2f),c.cl);
gp.Color=W;j.f[c._]=gp;aJ(j,c,bc(a5,i),W);}}else if(i.h==e.d4||i.h==e.d0||i.h==e.dT||i.h==e.dW){
if(aW||bx){O=true;double sH=(i.hY||i.L=="percent"||i.L=="pct")? D : ic(i.aY);int pt=c.N.Count;
for(int e8=0;e8<pt;e8++){bool tv=sH>=((e8+0.4)/(double)pt*100.0);Color uu=tv ? W : ay(i,new Color(24,28,36));var pu=j.f[c.N[e8]];
pu.Color=uu;j.f[c.N[e8]]=pu;}aJ(j,c,bc(a5,i),W);}}else if(i.h==e.cx){
if(aW||bx){O=true;for(int gw=0;gw<c.cy.Length-1;gw++)c.cy[gw]=c.cy[gw+1];c.cy[c.cy.Length-1]=(float)D;
int t_=c.N.Count;for(int eT=0;eT<t_;eT++){float sZ=c.cy[eT];float sY=Math.Max(0.06f,Math.Min(1f,sZ/100f));float nc=c.a7*sY;
float sm=c.cl+(c.a7/2f)-(nc/2f);var dj=j.f[c.N[eT]];dj.Size=new Vector2(dj.Size.Value.X,nc);
dj.Position=new Vector2(dj.Position.Value.X,sm);dj.Color=W;j.f[c.N[eT]]=dj;}aJ(j,c,bc(a5,i),W);
}}else if(i.h==e.c_){if(c.E<0f)c.E=(float)D;float hr=(float)D;if(Math.Abs(c.E-hr)>0.05f){
c.E+=(hr-c.E)*0.35f;O=true;}else if(c.E!=hr){c.E=hr;O=true;}if(O||aW){
float n4=c.a8*0.5f;float m7=c.bI+n4;float no=(float)Math.Max(-1.0,Math.Min(1.0,(c.E-50.0)/50.0));float jw=Math.Abs(no)*n4;
float sI=(no>=0)?(m7+jw*0.5f):(m7-jw*0.5f);var gq=j.f[c._];gq.Size=new Vector2(jw,c.a7);
gq.Position=new Vector2(sI,c.cl);gq.Color=W;j.f[c._]=gq;aJ(j,c,bc(a5,i),W);
}}else if(i.h==e.cB){if(aW||bx){O=true;bool jY=D>=50.0;float ok=Math.Max(4f,c.a7);float tb=jY ?(c.bI+c.a8-ok*0.5f):(c.bI+ok*0.5f);
Color oj=jY ?(W!=Color.White ? W : new Color(34,197,94)): new Color(60,70,85);var jt=j.f[c._];jt.Position=new Vector2(tb,c.cl);
jt.Color=oj;j.f[c._]=jt;aJ(j,c,jY ? "ON" : "OFF",oj);}}else if(i.h==e.cp){if(aW||bx){
O=true;bool jX=D>=50.0;Color m8=jX ?(W!=Color.White ? W : new Color(56,189,248)): new Color(30,36,48);mb(j,c,m8);
aJ(j,c,jX ? "ACTIVE" : "READY",jX ? m8 : new Color(148,163,184));}}else if(i.h==e.cz){if(aW||bx){
O=true;string uH=D>=75.0 ? "ONLINE" :(D>=25.0 ? "STBY" : "OFFLINE");Color pG=D>=75.0 ? new Color(34,197,94):(D>=25.0 ? new Color(234,179,8): new Color(239,68,68));
mb(j,c,pG);aJ(j,c,uH,pG);}}else if(i.h==e.cY){if(aW||bx){O=true;int o2=c.N.Count;
for(int eU=0;eU<o2;eU++){bool tw=D>=((eU+0.5)/(double)o2*100.0);Color tV=tw ? W : ay(i,new Color(24,28,36));var o3=j.f[c.N[eU]];
o3.Color=tV;j.f[c.N[eU]]=o3;}aJ(j,c,bc(a5,i),W);}}else if(i.h==e.cr){
O=true;float g7;Color eA;string gr;bool s3=(i.L=="producing"||i.L=="active"||i.L=="running"||i.L=="state"||(i.L!="percent"&&(D<=1.0||D==100.0)));
if(s3){if(D>0.0){c.E=(c.E+0.3927f)%6.28318f;g7=c.E;eA=new Color(34,197,94);gr="RUN";}else{g7=0f;
eA=new Color(148,163,184);gr="IDLE";}}else{g7=(D>55.0)? 0f :((D<45.0)? 3.14159f : 1.57079f);eA=(D>55.0)? new Color(34,197,94):((D<45.0)? new Color(239,68,68): new Color(148,163,184));
gr=(D>55.0)? "CHG" :((D<45.0)? "DSC" : "STBY");}var ju=j.f[c._];ju.RotationOrScale=g7;ju.Color=eA;
j.f[c._]=ju;aJ(j,c,gr,eA);}else{string gm;bq ht;ef.TryGetValue(i.aY,out ht);c8.TryGetValue(l_(i.L)? i.aY+"."+i.L :(ht!=null&&ht.br!=null ? ht.br : i.aY),out gm);
if(aW||bx||gm!=c.l0){c.l0=gm;O=true;Color lp=W;if(lp==Color.Red&&f0)lp=new Color(255,75,75);aJ(j,c,gm ?? bc(a5,i),lp);
}}if(c.lV&&(aW||bx)){O=true;var ow=j.f[c.c4];string tR=lT(D);string ug=bc(a5,i);
ow.Data=i.aa.Replace("{percent}",tR).Replace("{val}",ug);j.f[c.c4]=ow;}}if(O){j.ed++;using(var sO=ld.DrawFrame()){
sO.AddRange(j.f);}}return O;}bool rq(z ce,IMyTextSurface le){le.FontSize=ce.c2;var gg=new List<KeyValuePair<string,string>>();
foreach(var o in ce.fI){double Q=0.0;string bm;string bh=null;double kj=o.fK;try{Q=bu(o.aY,o.L);if(o.c5!=null)kj=bu(o.c5,o.h_);
}catch(Exception vl){gg.Add(new KeyValuePair<string,string>($"{o.aa}: [UNRESOLVED]","red"));continue;}double pb=Q;if(kj>0)Q=Math.Min(100.0,pb/kj*100.0);
string p9=bc(pb,o);if(o.h==e.dO){double nK=(o.hY||o.L=="percent"||o.L=="pct")? Q : ic(o.aY);
bm=K.rj(o.aa,nK,10,p9);bh=o.cs ? K.fC(o.bd,nK): o.aw;}else if(o.h==e.c_){
bm=K.rh(o.aa,Q,10);bh=o.cs ? K.fC(o.bd,Q): o.aw;}else if(o.h==e.cB){
bm=K.rt(o.aa,Q);bh=o.aw ??(Q>=50.0 ? "green" : "gray");}else if(o.h==e.cp){bm=K.rm(o.aa,Q);
bh=o.aw ??(Q>=50.0 ? "cyan" : "gray");}else if(o.h==e.cz){bm=K.rs(o.aa,Q);bh=o.aw ??(Q>=75.0 ? "lime" :(Q>=25.0 ? "yellow" : "red"));
}else if(o.h==e.cY){bm=K.rg(o.aa,Q,8);bh=o.cs ? K.fC(o.bd,Q):(o.aw ?? "yellow");
}else if(o.h==e.cr){bool s4=(o.L=="producing"||o.L=="active"||o.L=="running"||o.L=="state"||(o.L!="percent"&&(Q<=1.0||Q==100.0)));
if(s4){char[]uF=new[]{'|','/','-','\\'};char ur=uF[Math.Abs(el)%4];bm=Q>0 ? $"{o.aa} [{ur}] RUN" : $"{o.aa} [==] IDLE";
bh=Q>0 ? "green" : "gray";}else{bm=K.rn(o.aa,Q,el);bh=o.aw ??(Q>55.0 ? "green" :(Q<45.0 ? "red" : "cyan"));
}}else{bm=$"{o.aa}: {p9}";bh=o.bL!=null ? K.d6(o.bL,Q): null;}gg.Add(new KeyValuePair<string,string>(bm,bh));
}var ga=K.rl(new List<string>(gg.ConvertAll(uP=>uP.Key)),dL: ce.ml);string px=string.Join("\n",ga);
string o5;if(cE.TryGetValue(ce,out o5)&&o5==px)return false;cE[ce]=px;using(var sP=le.DrawFrame()){
float qq=4f;float oy=qr*ce.c2;try{var oB=le.MeasureStringInPixels(new StringBuilder("A"),"Monospace",ce.c2);
if(oB.Y>0)oy=oB.Y;}catch{}for(int dp=0;dp<ga.Count;dp++){string ng=null;if(dp>0&&dp<ga.Count-1)ng=gg[dp-1].Value;
var pF=MySprite.CreateText(ga[dp],"Monospace",F(ng),ce.c2,TextAlignment.LEFT);pF.Position=new Vector2(4f,qq);sP.Add(pF);
qq+=oy;}}return true;}Color F(string gP){if(string.IsNullOrEmpty(gP))return Color.White;Color eN;if(mp.TryGetValue(gP,out eN))return eN;
p et=p.d5(gP);eN=et.fF ? Color.Transparent : new Color(et.bQ,et.bM,et.bG,et.hC);mp[gP]=eN;return eN;}public enum a9{qv,hX,dX}
public enum aI{q1,hE,mg,l2,l1}public class bq{public string af;public a9 bN;public List<string>a0=new List<string>();public string mh{
get{return a0.Count>0 ? a0[0]: "";}set{if(a0.Count==0)a0.Add(value);else a0[0]=value;}}public aI dM=aI.q1;
public double fx=0.0;public string hO;public string br;public bool fo;}public enum e{ig,dO,d4,cx,d0,dT,dW,c_,cB,cp,cz,cY,cr,fP}
public class aZ{public string hQ;public string cq;public bool c3;}public class bs{public e h;public string aa;public string aY;
public string L;public bool cs;public string[]bd;public string aw;public List<aZ>bL;public string fy="right";
public string ec;public string dY;public List<double>fJ;public int cw;public int ih;public string h6;public int fO;public string dV,hN;
public string d2,ik;public double fK;public string c5,h_;public bool hY{get{return fK>0||c5!=null;}}}public struct p{public byte bQ,bM,bG,hC;
public bool fF;public p(byte t7,byte sR,byte rY,byte rG=255){bQ=t7;bM=sR;bG=rY;hC=rG;fF=false;}public static readonly p rC=new p(0,0,0,0){
fF=true};public static readonly p fQ=new p(255,255,255);public static readonly p qu=new p(0,0,0);public static readonly p re=new p(255,0,0);
public static readonly p qV=new p(0,255,0);public static readonly p qZ=new p(50,205,50);public static readonly p rF=new p(255,255,0);
public static readonly p q2=new p(255,140,0);public static readonly p qG=new p(0,255,255);public static readonly p qw=new p(0,120,255);
public static readonly p q_=new p(255,0,255);public static readonly p rc=new p(128,0,128);public static readonly p qU=new p(128,128,128);
public static readonly p qH=new p(60,60,60);public static p d5(string bl){if(string.IsNullOrWhiteSpace(bl))return fQ;bl=bl.Trim();
if(bl.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bl.Equals("none",StringComparison.OrdinalIgnoreCase))return rC;if(bl.StartsWith("#")){
string b6=bl.Substring(1).Trim();if(b6.Length==6){byte t8=Convert.ToByte(b6.Substring(0,2),16);byte sS=Convert.ToByte(b6.Substring(2,2),16);byte rZ=Convert.ToByte(b6.Substring(4,2),16);
return new p(t8,sS,rZ);}if(b6.Length==3){byte t9=Convert.ToByte(new string(b6[0],2),16);byte sT=Convert.ToByte(new string(b6[1],2),16);byte r_=Convert.ToByte(new string(b6[2],2),16);
return new p(t9,sT,r_);}}if(bl.Contains(",")){var dx=bl.Split(',');if(dx.Length>=3){byte ua=(byte)Math.Max(0,Math.Min(255,int.Parse(dx[0].Trim())));
byte sU=(byte)Math.Max(0,Math.Min(255,int.Parse(dx[1].Trim())));byte r0=(byte)Math.Max(0,Math.Min(255,int.Parse(dx[2].Trim())));byte rH=dx.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(dx[3].Trim()))):(byte)255;
return new p(ua,sU,r0,rH);}}switch(bl.ToLowerInvariant()){case "red": return re;case "green": return qV;case "lime": return qZ;case "yellow": return rF;
case "orange": return q2;case "cyan": return qG;case "blue": return qw;case "magenta": return q_;case "purple": return rc;case "white": return fQ;
case "black": return qu;case "gray": case "grey": return qU;case "darkgray": case "darkgrey": return qH;default: return fQ;}}}public class bJ{
public string[]bK;public string dP="vertical";public List<aZ>aX;public string hG,hH;public List<aZ>c1;public string hT,hV;
}public class aD : bJ{public string c6="default";public int dU=0;public int d7=0;public int d9=1;public int ea=1;public string cA;public string ii="left";
public string ij;public string dR;public string lP;public float? lR;public string cZ;public string ft;public string[]fu;
public string h5="horizontal";public List<bs>ao=new List<bs>();}public class z : bJ{public string be;public string cv="standard";
public string c0="graphic";public string dS;public string[]fv;public string mi="left";public string mj;public string lQ="Monospace";
public string bH;public string lH;public int lI=10;public int lF=1;public float hL=1.2f;public bool fD;
public string h7;public string hI;public string lE;public string lC;public string h1;public string h0;public string ck;
public string co;public float hM=2.5f;public string lG="cyan";public string[]fr;public List<string>fs=new List<string>();
public List<string>qS(){if(fs!=null&&fs.Count>0)return fs;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float c2=1.0f;public int ia=0;public float hD=1.6f;public int dZ=1;public int d_=1;public int ml=30;public string bO;
public int cu=0;public List<aD>ak=new List<aD>();public List<bs>fI{get{if(ak.Count==0)ak.Add(new aD{c6="main"}
);return ak[0].ao;}}}public class bT{public string h2;public List<string>fL=new List<string>();public Dictionary<string,string>hP=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>hK=new List<string>();}public class h9{public List<bq>cn=new List<bq>();public List<z>V=new List<z>();
public List<string>bb=new List<string>();public List<string>ee=new List<string>();public Dictionary<string,List<string>>dN=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class lX{static readonly Dictionary<string,bT>lJ=new Dictionary<string,bT>(StringComparer.OrdinalIgnoreCase);
static lX(){rf();}static void rf(){bR("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bR("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bR("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bR("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bR("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bR("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bR("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bR("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bR(string oK,string[]tO,string sa){var ho=new bT{h2=oK}
;foreach(var tH in tO){var j6=tH.Split(new[]{'='},2);string oV=h3(j6[0]);ho.fL.Add(oV);if(j6.Length>1)ho.hP[oV]=ad(j6[1].Trim());
}var tt=sa.Replace("\r\n","\n").Split('\n');foreach(var th in tt){string pj=th.Trim();if(pj.Length>0)ho.hK.Add(pj);}lJ[oK]=ho;}static string h3(string dv){
dv=dv.Trim();if(dv.StartsWith("$"))dv=dv.Substring(1);return dv.ToLowerInvariant();}public static h9 q4(string sp){var at=new h9();z d=null;
aD u=null;var o4=ra(sp,at.bb,at.dN);for(int eH=0;eH<o4.Count;eH++){string a=o4[eH].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){at.cn.Add(q5(a));}else if(a.StartsWith("@screen")){d=new z{be=a.Substring("@screen".Length).Trim()}
;at.V.Add(d);u=null;}else if(a.StartsWith("profile:")){s(d,"profile:");d.cv=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){s(d,"border:");string mK=a.Substring("border:".Length).Trim();if(u!=null)u.dR=mK;
else d.c0=mK;}else if(a.StartsWith("border_color:")){s(d,"border_color:");string mM=a.Substring("border_color:".Length).Trim();
if(u!=null)u.ft=mM;else d.dS=mM;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
s(d,"border_gradient");string cH=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
cH=cH.Trim();if(cH.StartsWith("(")&&cH.Contains(")"))cH=a_(cH,0);string[]pI=aK(cH).Select(ul=>ul.Trim()).ToArray();if(u!=null)u.fu=pI;
else d.fv=pI;}else if(a.StartsWith("title_align:")){s(d,"title_align:");string pQ=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(u!=null)u.ii=pQ;else d.mi=pQ;}else if(a.StartsWith("title_color:")){s(d,"title_color:");string pT=a.Substring("title_color:".Length).Trim();
if(u!=null)u.ij=pT;else d.mj=pT;}else if(a.StartsWith("font_size:")){s(d,"font_size:");float pN=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(u!=null)u.lR=pN;else d.c2=pN;}else if(a.StartsWith("font:")){s(d,"font:");string nD=a.Substring("font:".Length).Trim();
if(u!=null)u.lP=nD;else d.lQ=nD;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){s(d,"bg:");string dc=a.Substring(a.IndexOf(':')+1).Trim();
bJ iR=u!=null ?(bJ)u : d;if(dc.StartsWith("color(")&&dc.EndsWith(")")){string mU;iR.aX=l6(dc.Substring(6,dc.Length-7),out mU);
string mS,mT;md(mU,out mS,out mT);iR.hG=mS;iR.hH=mT;}else if(u!=null)u.cZ=dc;
else d.bH=dc;}else if(a.StartsWith("layout:")){s(d,"layout:");q6(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
s(d,"alert_period:");d.hD=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){s(d,"refresh:");d.ia=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
s(d,"output:");var gV=aK(a.Substring("output:".Length).Trim());if(gV.Count==0)throw new FormatException("output: requires a target block name");
d.bO=ad(gV[0]);d.cu=gV.Count>1 ? int.Parse(gV[1]): 0;}else if(a.StartsWith("width:")){s(d,"width:");
d.ml=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){s(d,"boot_msg:");
d.lH=ad(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){s(d,"boot_stagger:");d.lI=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){s(d,"boot_calibrate:");string nj=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.hL=nj=="off" ? 0f : Math.Max(0f,float.Parse(nj,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){s(d,"boot_batch:");
d.lF=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){s(d,"boot_duration:");
string ev=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool oa=ev.EndsWith("s");if(oa)ev=ev.Substring(0,ev.Length-1).Trim();
float ji;if(float.TryParse(ev,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out ji)){d.hM=oa ? ji : ji/60f;
}}else if(a.StartsWith("boot_log:")){s(d,"boot_log:");d.fs.Add(ad(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("mirror_panel:")){
s(d,"mirror_panel:");d.h1=ad(a.Substring("mirror_panel:".Length).Trim());}else if(a.StartsWith("mirror:")){s(d,"mirror:");
d.h0=ad(a.Substring("mirror:".Length).Trim());}else if(a.StartsWith("banner:")){s(d,"banner:");d.ck=ad(a.Substring("banner:".Length).Trim());
}else if(a.StartsWith("boot_logo:")){s(d,"boot_logo:");d.co=ad(a.Substring("boot_logo:".Length).Trim());}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){
s(d,"boot_gradient");d.fr=l7(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){s(d,"bg_gradient");
(u!=null ?(bJ)u : d).bK=l7(a);}else if(a.StartsWith("bg_gradient_dir:")){s(d,"bg_gradient_dir:");
(u!=null ?(bJ)u : d).dP=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(a.StartsWith("flash:")){
s(d,"flash:");bJ jq=u!=null ?(bJ)u : d;string jp=a.Substring("flash:".Length).Trim();string nF=mf(jp);
string nE,nG;md(jp.Substring(0,jp.Length-nF.Length),out nE,out nG);jq.hT=nE;jq.hV=nG;jq.c1=new List<aZ>{
new aZ{hQ=nF,cq="on"}};}else if(a.StartsWith("bg_texture:")){s(d,"bg_texture:");d.hI=ad(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){s(d,"bg_texture_color:");d.lE=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gradient_mode:")){s(d,"gradient_mode:");d.fD=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){s(d,"screen_bg:");d.bH=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
s(d,"panel_bg:");d.h7=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){s(d,"alert_bg:");
d.lC=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){s(d,"boot_color:");
d.lG=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){s(d,"orientation:");string tE=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(u!=null)u.h5=tE;}else if(a.StartsWith("@panel")){s(d,"@panel");u=q8(a);d.ak.Add(u);
}else if(a.StartsWith("title:")){s(d,"title:");if(u==null){u=new aD{c6="main",dU=0,d7=0,d9=d.dZ,ea=d.d_}
;d.ak.Add(u);}u.cA=ad(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){s(d,"line:");
if(u==null){u=new aD{c6="main",dU=0,d7=0,d9=d.dZ,ea=d.d_};d.ak.Add(u);}u.ao.Add(q7(a.Substring("line:".Length).Trim()));
}else{at.bb.Add($"Line {eH + 1}: unrecognized directive: \"{a}\"");}}catch(Exception sB){at.bb.Add($"Line {eH + 1}: parse error ({sB.Message}) in \"{a}\"");
}}foreach(var pk in at.V){if(string.IsNullOrEmpty(pk.bO))at.bb.Add($"@screen {pk.be}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var av in at.V){int ks=av.ak.Count(kH=>kH.cZ!=null||kH.aX!=null||kH.bK!=null);if(av.ak.Count>1&&ks>0&&ks<av.ak.Count)at.ee.Add("screen '"+av.be+"': only "+ks+" of "+av.ak.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(av.ck!=null&& !at.dN.ContainsKey(av.ck))at.bb.Add("Screen '"+av.be+"': banner '"+av.ck+"' is not defined (@banner "+av.ck+" ... @end)");
if(av.co!=null&& !at.dN.ContainsKey(av.co))at.bb.Add("Screen '"+av.be+"': boot_logo '"+av.co+"' is not defined (@banner "+av.co+" ... @end)");
}return at;}static void q6(string iY,z pr){int kz=iY.IndexOf('(');if(kz<0)throw new FormatException("missing '(' in layout: specification");
string oi=iY.Substring(0,kz).Trim().ToLowerInvariant();string rO=a_(iY,kz);var iH=aK(rO);if(oi=="grid"){if(iH.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
pr.dZ=Math.Max(1,int.Parse(iH[0]));pr.d_=Math.Max(1,int.Parse(iH[1]));}else{throw new FormatException($"unknown layout kind '{oi}' (expected grid)");
}}static aD q8(string tp){string e3=tp.Substring("@panel".Length).Trim();int f3=e3.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(f3<0)throw new FormatException("@panel requires an at(...) positioning clause");
string tB=e3.Substring(0,f3).Trim();string mF=e3.Substring(f3);int o0=mF.IndexOf('(');string mD=a_(mF,o0);
var cF=aK(mD);if(cF.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bB=new aD{c6=tB,dU=int.Parse(cF[0]),d7=int.Parse(cF[1]),d9=cF.Count>2 ? int.Parse(cF[2]): 1,ea=cF.Count>3 ? int.Parse(cF[3]): 1}
;int mw=f3+o0+mD.Length+1;if(mw<e3.Length){string aS=e3.Substring(mw).Trim();int pY=aS.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(pY>=0){string fb=aS.Substring(pY+5).Trim();if(fb.StartsWith("(")&&fb.Contains(")"))bB.cA=ad(a_(fb,0));else if(fb.StartsWith(":")){
string cI=fb.Substring(1).Trim();if(cI.StartsWith("\"")){int nz=cI.IndexOf('"',1);bB.cA=nz>0 ? cI.Substring(1,nz-1): ad(cI);}else{int pD=cI.IndexOf(' ');
bB.cA=pD>=0 ? cI.Substring(0,pD): cI;}}}string oN=fz(aS,"orientation:");if(oN!=null)bB.h5=oN.ToLowerInvariant();string mu=fz(aS,"title_align:");
if(mu!=null)bB.ii=mu.ToLowerInvariant();string pV=fz(aS,"title_color:");if(pV!=null)bB.ij=pV;int f9=aS.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(f9>=0&& !aS.Substring(f9).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aS.Substring(f9).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bB.dR=fz(aS.Substring(f9),"border:");}int f6=aS.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(f6<0)f6=aS.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(f6>=0){string b4=aS.Substring(f6+"border_gradient".Length).Trim();if(b4.StartsWith(":"))b4=b4.Substring(1).Trim();if(b4.StartsWith("(")&&b4.Contains(")"))b4=a_(b4,0);
bB.fu=aK(b4).Select(um=>um.Trim()).ToArray();}}return bB;}static string fz(string pl,string oh){int n9=pl.IndexOf(oh,StringComparison.OrdinalIgnoreCase);
if(n9<0)return null;string lt=pl.Substring(n9+oh.Length).Trim();int pE=lt.IndexOf(' ');return pE>=0 ? lt.Substring(0,pE): lt;}static string[]l7(string kc){
int jC=kc.IndexOfAny(new[]{':','('});string eC=kc.Substring(kc[jC]==':' ? jC+1 : jC).Trim();if(eC.StartsWith("(")&&eC.Contains(")"))eC=a_(eC,0);return aK(eC).Select(un=>un.Trim()).ToArray();
}static List<string>ra(string sq,List<string>jn,Dictionary<string,List<string>>r2){var p4=new Dictionary<string,bT>(StringComparer.OrdinalIgnoreCase);
var dA=(sq ?? "").Replace("\r\n","\n").Split('\n');var kr=new List<string>();int dq=0;while(dq<dA.Length){string kM=dA[dq++];if(kM.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var aq=new List<string>();while(dq<dA.Length){string ms=dA[dq++].Replace("\t","    ").TrimEnd();if(ms.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
aq.Add(ms);}while(aq.Count>0&&aq[0].Length==0)aq.RemoveAt(0);while(aq.Count>0&&aq[aq.Count-1].Length==0)aq.RemoveAt(aq.Count-1);int jW=int.MaxValue;
foreach(var iz in aq)if(iz.Length>0)jW=Math.Min(jW,iz.Length-iz.TrimStart().Length);for(int eO=0;eO<aq.Count;eO++)if(aq[eO].Length>0)aq[eO]=aq[eO].Substring(jW);
r2[kM.TrimStart().Substring(8).Trim()]=aq;continue;}string b9=me(kM).Trim();if(b9.Length==0)continue;if(b9.StartsWith("@template")){try{
var li=q9(b9);while(dq<dA.Length){string sc=dA[dq++];string iZ=me(sc).Trim();if(iZ.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(iZ.Length>0)li.hK.Add(iZ);}p4[li.h2]=li;}catch(Exception sC){jn.Add($"Template parse error ({sC.Message}) in \"{b9}\"");}}
else if(b9.StartsWith("@use")){try{lM(b9,p4,kr,jn);}catch(Exception sD){jn.Add($"@use error ({sD.Message}) in \"{b9}\"");
}}else{kr.Add(b9);}}return kr;}static bT q9(string tq){string kN=tq.Substring("@template".Length).Trim();int kA=kN.IndexOf('(');
if(kA<0)throw new FormatException("missing '(' in @template header");string tC=kN.Substring(0,kA).Trim();string rP=a_(kN,kA);var lj=new bT{
h2=tC};foreach(var tI in aK(rP)){var j7=tI.Split(new[]{'='},2);string oW=h3(j7[0]);lj.fL.Add(oW);if(j7.Length>1)lj.hP[oW]=ad(j7[1].Trim());
}return lj;}static void lM(string tr,Dictionary<string,bT>p5,List<string>oS,List<string>sA,int nn=0){string g5=tr.Substring("@use".Length).Trim();
int gT=g5.IndexOf('(');string kk=gT>=0 ? g5.Substring(0,gT).Trim(): g5.Trim();string rQ=gT>=0 ? a_(g5,gT): "";bT dI=null;
if(!p5.TryGetValue(kk,out dI)){if(!lJ.TryGetValue(kk,out dI))throw new FormatException($"unknown template '{kk}'");}var gc=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var om in dI.hP)gc[om.Key]=om.Value;var mC=aK(rQ);for(int en=0;en<mC.Count;en++){string f1=mC[en];int jl=f1.IndexOf('=');if(jl>=0){
string s9=h3(f1.Substring(0,jl));string u1=ad(f1.Substring(jl+1).Trim());gc[s9]=u1;}else{if(en<dI.fL.Count){string ta=dI.fL[en];
gc[ta]=ad(f1.Trim());}}}foreach(var sb in dI.hK){string ey=sb;foreach(var gK in gc){ey=ey.Replace("$"+gK.Key,gK.Value).Replace("{"+gK.Key+"}",gK.Value);
}if(ey.StartsWith("@use")){if(nn>3)throw new FormatException("@use nested too deep");lM(ey,p5,oS,sA,nn+1);
}else oS.Add(ey);}}static void s(z so,string sx){if(so==null)throw new FormatException($"'{sx}' used before any @screen declared");
}static string me(string bj){if(string.IsNullOrEmpty(bj))return bj;bool jV=false;for(int b7=0;b7<bj.Length;b7++){char i2=bj[b7];if(i2=='"'){jV= !jV;
}else if(!jV){if(i2=='/'&&b7+1<bj.Length&&bj[b7+1]=='/'){return bj.Substring(0,b7);}if(i2=='#'){if(qX(bj,b7)){continue;}return bj.Substring(0,b7);}}}return bj;
}static bool qX(string cO,int oc){int dt=0;for(int jR=oc+1;jR<cO.Length;jR++){char df=cO[jR];if((df>='0'&&df<='9')||(df>='a'&&df<='f')||(df>='A'&&df<='F'))dt++;
else break;}if(dt==3||dt==4||dt==6||dt==8){int eS=oc+1+dt;if(eS>=cO.Length||char.IsWhiteSpace(cO[eS])||cO[eS]==')'||cO[eS]==','||cO[eS]==':')return true;
}return false;}static bq q5(string ts){string kO=ts.Substring("@bind".Length).Trim();int jm=kO.IndexOf('=');if(jm<0)throw new FormatException("missing '=' in @bind");
string oL=kO.Substring(0,jm).Trim();string jo=kO.Substring(jm+1).Trim();int kB=jo.IndexOf('(');if(kB<0)throw new FormatException("missing '(' in @bind target");
string gu=jo.Substring(0,kB).Trim().ToLowerInvariant();string rR=a_(jo,kB);var bZ=aK(rR);if(bZ.Count==0)throw new FormatException("@bind target has no arguments");
var au=new bq{af=oL};if(gu=="dynamic"||gu=="var"||gu=="mem"){au.bN=a9.dX;au.br=bZ.Count>0 ? ad(bZ[0].Trim()): oL;
for(int iA=0;iA<bZ.Count;iA++){var gL=bZ[iA].Split(new[]{':'},2);if(gL.Length==2&&gL[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double nm;
if(double.TryParse(gL[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nm))au.fx=nm;else au.hO=ad(gL[1].Trim());
}}return au;}au.bN=gu=="group" ? a9.hX : a9.qv;string f_=null;for(int fZ=0;fZ<bZ.Count;fZ++){var ds=bZ[fZ].Split(new[]{':'},2);if(ds.Length==2&&ds[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
f_=ds[1].Trim().ToLowerInvariant();}else if(ds.Length==2&&ds[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){au.fo=ds[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{au.a0.Add(ad(bZ[fZ].Trim()));}}if(au.a0.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(au.bN==a9.hX){if(f_==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(f_){case "avg": au.dM=aI.hE;break;case "sum": au.dM=aI.mg;break;case "min": au.dM=aI.l2;break;case "max": au.dM=aI.l1;
break;default: throw new FormatException($"unknown agg '{f_}' (expected avg/sum/min/max)");}}return au;}static bs q7(string f8){var m=new bs();
if(!f8.StartsWith("\""))throw new FormatException("line must start with a quoted label");int jk=f8.IndexOf('"',1);if(jk<0)throw new FormatException("unterminated quoted label");
m.aa=f8.Substring(1,jk-1);string e2=f8.Substring(jk+1).Trim();int gU=e2.IndexOf('(');if(gU<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string C=e2.Substring(0,gU).Trim().ToLowerInvariant();string rN=a_(e2,gU);string ah=e2.Substring(e2.IndexOf(')',gU)+1).Trim();
var jg=rN.Trim().Split('.');if(jg.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
m.aY=jg[0].Trim();m.L=jg[1].Trim();if(C=="bar")m.h=e.dO;else if(C=="led")m.h=e.d4;else if(C=="spark"||C=="sparkline")m.h=e.cx;
else if(C=="hazard"||C=="stripes")m.h=e.d0;else if(C=="chevron"||C=="chevrons")m.h=e.dT;else if(C=="dots"||C=="dot")m.h=e.dW;
else if(C=="balance"||C=="horizon"||C=="centered")m.h=e.c_;else if(C=="toggle"||C=="switch")m.h=e.cB;else if(C=="checkbox"||C=="check")m.h=e.cp;
else if(C=="status"||C=="badge"||C=="state")m.h=e.cz;else if(C=="ammo"||C=="pips")m.h=e.cY;else if(C=="flow"||C=="rate"||C=="spinner")m.h=e.cr;
else if(C=="text")m.h=e.ig;else if(C=="tank"||C=="tube"||C=="bucket")m.h=e.fP;else throw new FormatException($"unknown display function '{C}'");
while(ah.Length>0){int je=ah.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);int dm=ah.IndexOf('(');if(je>=0&&(dm<0||je<dm)){
string gj=ah.Substring(je+4).Trim();int hf=gj.IndexOf(' ');m.fy=(hf>=0 ? gj.Substring(0,hf): gj).ToLowerInvariant();ah=(hf>=0 ? gj.Substring(hf): "").Trim();
continue;}int lr=ah.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(lr>=0&&(dm<0||lr<dm)){string hz=ah.Substring(lr+5).Trim();
int hg=hz.IndexOf(' ');m.ec=(hg>=0 ? hz.Substring(0,hg): hz).Trim().Trim('"');ah=(hg>=0 ? hz.Substring(hg): "").Trim();continue;}int jx=ah.IndexOf("format:",StringComparison.OrdinalIgnoreCase);
if(jx>=0&&(dm<0||jx<dm)){string go=ah.Substring(jx+7).Trim();int hh=go.IndexOf(' ');m.dY=(hh>=0 ? go.Substring(0,hh): go).Trim().Trim('"');
ah=(hh>=0 ? go.Substring(hh): "").Trim();continue;}int gO=ah.IndexOf('(');if(gO<0)break;string P=ah.Substring(0,gO).Trim().ToLowerInvariant();
int ov=P.LastIndexOf(' ');if(ov>=0)P=P.Substring(ov+1).Trim();string ai=a_(ah,gO);ah=ah.Substring(ah.IndexOf(')',gO)+1).Trim();
if(P=="gradient"){m.cs=true;m.bd=aK(ai).Select(uo=>uo.Trim()).ToArray();if(m.bd.Length<2)throw new FormatException("gradient() needs at least 2 colors");
}else if(P=="size"){string lh=ai.Trim().ToLowerInvariant();m.fO=lh=="large"||lh=="fill" ? 2 : lh=="medium" ? 1 : 0;}else if(P=="marker"){m.fJ=new List<double>();
foreach(var ty in aK(ai)){double oG;if(!double.TryParse(ty,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out oG))throw new FormatException("marker() takes numbers from 0 to 100");
m.fJ.Add(oG);}}else if(P=="ticks")m.ih=int.Parse(ai.Trim());else if(P=="segments"||P=="points")m.cw=Math.Max(2,Math.Min(40,int.Parse(ai.Trim())));
else if(P=="outline")m.h6=ai.Trim();else if(P=="color_by"){var i6=ai.Trim().Split('.');if(i6.Length!=2)throw new FormatException("color_by() takes bindname.field");
m.dV=i6[0].Trim();m.hN=i6[1].Trim();}else if(P=="label_color")m.d2=ai.Trim();else if(P=="track")m.ik=ai.Trim();else if(P=="flat"){
m.aw=ai.Trim();}else if(P=="pattern"){string T=ai.Trim().ToLowerInvariant();if(T=="hazard"||T=="stripes")m.h=e.d0;else if(T=="chevron"||T=="chevrons")m.h=e.dT;
else if(T=="dots"||T=="dot")m.h=e.dW;else if(T=="led")m.h=e.d4;else if(T=="balance"||T=="horizon")m.h=e.c_;
else if(T=="toggle"||T=="switch")m.h=e.cB;else if(T=="checkbox"||T=="check")m.h=e.cp;else if(T=="status"||T=="badge"||T=="state")m.h=e.cz;
else if(T=="ammo"||T=="pips")m.h=e.cY;else if(T=="flow"||T=="rate"||T=="spinner")m.h=e.cr;}else if(P=="color"){string s2;
m.bL=l6(ai,out s2);}else if(P=="dir"||P=="direction"){m.fy=ai.Trim().ToLowerInvariant();}else if(P=="unit"){
m.ec=ai.Trim().Trim('"');}else if(P=="max"){double oF;if(double.TryParse(ai.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out oF))m.fK=oF;
else{var kh=ai.Trim().Split('.');if(kh.Length!=2)throw new FormatException("max() takes a number or bindname.field");m.c5=kh[0].Trim();m.h_=kh[1].Trim();
}}else if(P=="format"||P=="fmt"){m.dY=ai.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{P}'");}}return m;}static string a_(string hb,int kp){
int jd=0;for(int eI=kp;eI<hb.Length;eI++){if(hb[eI]=='(')jd++;else if(hb[eI]==')'){jd--;if(jd==0)return hb.Substring(kp+1,eI-kp-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aK(string rS){return rS.Split(',').Select(up=>up.Trim()).Where(uq=>uq.Length>0).ToList();
}static List<aZ>l6(string rM,out string js){var g9=new List<aZ>();js=null;foreach(var kC in aK(rM)){int ja=kC.LastIndexOf(':');
if(ja<0)throw new FormatException("color() entries need 'expr : color'");string gM=kC.Substring(0,ja).Trim();string nf=kC.Substring(ja+1).Trim();
if(gM.Equals("default",StringComparison.OrdinalIgnoreCase)){g9.Add(new aZ{c3=true,cq=nf});}else{string pJ=mf(gM);
if(js==null)js=gM.Substring(0,gM.Length-pJ.Length).Trim();g9.Add(new aZ{hQ=pJ,cq=nf});}}if(!g9.Any(ub=>ub.c3))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return g9;}static void md(string uc,out string r8,out string sG){var eV=(uc ?? "").Split('.');if(eV.Length!=2||eV[0].Trim().Length==0||eV[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
r8=eV[0].Trim();sG=eV[1].Trim();}static string mf(string dk){dk=dk.Trim();foreach(var tF in new[]{">=","<=","==",">","<"}){int jS=dk.IndexOf(tF,StringComparison.Ordinal);
if(jS>0)return dk.Substring(jS).Trim();if(jS==0)return dk;}throw new FormatException($"color() rule has no recognizable comparison operator: '{dk}'");}
static string ad(string cP){if(cP.Length>=2&&cP[0]=='"'&&cP[cP.Length-1]=='"')return cP.Substring(1,cP.Length-2);return cP;}}public static class qt{public static double qs(aI rI,List<double>dK){
if(dK==null||dK.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(rI){case aI.hE: return dK.Average();case aI.mg: return dK.Sum();
case aI.l2: return dK.Min();case aI.l1: return dK.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class K{
public static string rj(string tj,double gW,int mL=10,string qd=null){gW=Math.Max(0,Math.Min(100,gW));int nL=(int)Math.Round(mL*(gW/100.0));
string r3=new string('#',nL)+new string('-',mL-nL);string u2=qd!=null ? qd : $"{gW:0}%";return $"{tj} [{r3}] {u2}";
}public static string lS(double cj,string lq=null,string gt=null){if(lq=="auto"){double iB=Math.Abs(cj);if(iB>=1e9)return(cj/1e9).ToString("F1")+"G";
if(iB>=1e6)return(cj/1e6).ToString("F1")+"M";if(iB>=1e3)return(cj/1e3).ToString("F1")+"k";string sE= !string.IsNullOrEmpty(gt)? gt : "F0";return cj.ToString(sE);
}string nN= !string.IsNullOrEmpty(gt)? gt :(Math.Abs(cj)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(lq)){return cj.ToString(nN);}return cj.ToString(nN)+lq;
}public static float qT(string t1,float pL,float lf,float gk=1.0f){string dw=(t1 ?? "standard").ToLowerInvariant();
if(dw=="compact"||(dw=="auto"&&pL<=300&&lf<=300&&lf>160)){return gk*0.75f;}if(dw=="corner"||(dw=="auto"&&lf<=160)){
return gk*0.55f;}if(dw=="wide"||(dw=="auto"&&pL>=1000)){return gk*1.15f;}return gk;}public static string rh(string tk,double gX,int b5=5){
gX=Math.Max(0,Math.Min(100,gX));double gl=(gX-50.0)/50.0;int eP=0;int e5=0;if(gl< -0.01)eP=(int)Math.Round(-gl*b5);
else if(gl>0.01)e5=(int)Math.Round(gl*b5);if(eP>b5)eP=b5;if(e5>b5)e5=b5;string tn=new string('-',b5-eP)+new string('<',eP);
string ui=new string('>',e5)+new string('-',b5-e5);return $"{tk} [{tn}|{ui}] {gX:0}%";}public static string rt(string os,double u4){
return u4>=50.0 ? $"{os} [====O] ON" : $"{os} [O====] OFF";}public static string rm(string ot,double u5){return u5>=50.0 ? $"{ot} [X] ACTIVE" : $"{ot} [ ] READY ";
}public static string rs(string j9,double qb){if(qb>=75.0)return $"{j9} [ ONLINE  ]";if(qb>=25.0)return $"{j9} [ STANDBY ]";return $"{j9} [ OFFLINE ]";
}public static string rg(string tl,double gY,int oA=8){gY=Math.Max(0,Math.Min(100,gY));int nM=(int)Math.Round(oA*(gY/100.0));
string tW=new string('|',nM)+new string('.',oA-nM);return $"{tl} [{tW}] {gY:0}%";}public static string rn(string ka,double eZ,int pX=0){
char[]uE=new[]{'|','/','-','\\'};char pm=uE[(pX>=0 ? pX : 0)%4];if(eZ>55.0)return $"{ka} {pm} ▲ CHG {eZ:0}%";if(eZ<45.0)return $"{ka} {pm} ▼ DSC {eZ:0}%";
return $"{ka} ● STBY {eZ:0}%";}public static bool q0(string aA,string eL){if(aA==null||eL==null)return false;if(aA=="*"||aA==eL)return true;
if(!aA.Contains("*")&& !aA.Contains("?"))return string.Equals(aA,eL,StringComparison.OrdinalIgnoreCase);int aF=0,eJ=0,k6= -1,kg=0;while(eJ<eL.Length){
if(aF<aA.Length&&(aA[aF]=='?'||char.ToLowerInvariant(aA[aF])==char.ToLowerInvariant(eL[eJ]))){aF++;eJ++;}else if(aF<aA.Length&&aA[aF]=='*'){
k6=aF;kg=eJ;aF++;}else if(k6!= -1){aF=k6+1;kg++;eJ=kg;}else{return false;}}while(aF<aA.Length&&aA[aF]=='*')aF++;return aF==aA.Length;
}public static int qx(double tJ,int p1){return Math.Max(0,Math.Min(p1,1+(int)(Math.Max(0.0,tJ)*p1)));}public static string fC(string[]dH,double kD){
if(dH==null||dH.Length==0)return "white";kD=Math.Max(0,Math.Min(100,kD));int jT=(int)Math.Floor(kD/100.0*dH.Length);if(jT>=dH.Length)jT=dH.Length-1;
return dH[jT];}public static p lU(string[]ch,double tS){if(ch==null||ch.Length==0)return p.fQ;if(ch.Length==1)return p.d5(ch[0]);
double uf=Math.Max(0.0,Math.Min(1.0,tS/100.0));double pq=uf*(ch.Length-1);int jU=(int)Math.Floor(pq);int s1=Math.Min(ch.Length-1,jU+1);
double la=pq-jU;p dg=p.d5(ch[jU]);p i3=p.d5(ch[s1]);byte ud=(byte)Math.Round(dg.bQ+(i3.bQ-dg.bQ)*la);
byte sV=(byte)Math.Round(dg.bM+(i3.bM-dg.bM)*la);byte r1=(byte)Math.Round(dg.bG+(i3.bG-dg.bG)*la);return new p(ud,sV,r1);}public static string d6(List<aZ>pi,double u6){
foreach(var kP in pi){if(kP.c3)continue;if(qP(kP.hQ,u6))return kP.cq;}var nl=pi.FirstOrDefault(ue=>ue.c3);return nl!=null ? nl.cq : null;
}static bool qP(string ez,double fh){ez=ez.Trim();foreach(var ko in new[]{">=","<=","==",">","<"}){if(ez.StartsWith(ko)){double e4=double.Parse(ez.Substring(ko.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(ko){case ">=": return fh>=e4;case "<=": return fh<=e4;case "==": return Math.Abs(fh-e4)<1e-9;case ">": return fh>e4;case "<": return fh<e4;
}}}throw new FormatException($"unsupported comparison expression: '{ez}'");}public static List<string>rl(List<string>sn,int dL){
var gQ=new List<string>();gQ.Add("+"+new string('-',dL+2)+"+");foreach(var kd in sn){string uX=kd.Length>dL ? kd.Substring(0,dL): kd;
string tM=uX.PadRight(dL);gQ.Add("| "+tM+" |");}gQ.Add("+"+new string('-',dL+2)+"+");return gQ;}public struct l5{public float mn,mo,mm,lW;
}public static l5 qD(int nb,int pf,int hi,int hj,int fd,int fe,float uO,float uN,float oX,float oY,float jE,float jF){
fd=Math.Max(1,fd);fe=Math.Max(1,fe);hi=Math.Max(1,Math.Min(fd-nb,hi));hj=Math.Max(1,Math.Min(fe-pf,hj));
float rV=uO-(oX*2f);float rU=uN-(oY*2f);float m6=(rV-(jE*(fd-1)))/fd;float m5=(rU-(jF*(fe-1)))/fe;
float t3=oX+nb*(m6+jE);float t4=oY+pf*(m5+jF);float t2=hi*m6+((hi-1)*jE);float tU=hj*m5+((hj-1)*jF);return new l5{
mn=t3,mo=t4,mm=t2,lW=tU};}public struct ma{public int q3;public int hZ;public int ag;public int aC;public int eb;
public int ba;public int rA;public int rB;public int qM;public int qN;
public double qO;}public static ma qE(z cf){var l=new ma();var o_=cf.ak!=null&&cf.ak.Count>0 ? cf.ak : new List<aD>{
new aD{ao=cf.fI}};l.q3=o_.Count;l.hZ=0;l.ag=0;l.aC=0;if(!string.IsNullOrEmpty(cf.bH)&& !cf.bH.Equals("none",StringComparison.OrdinalIgnoreCase))l.ag++;
if(!string.IsNullOrEmpty(cf.c0)&& !cf.c0.Equals("none",StringComparison.OrdinalIgnoreCase))l.ag+=2;foreach(var eW in o_){
if(!string.IsNullOrEmpty(eW.cZ)&& !eW.cZ.Equals("none",StringComparison.OrdinalIgnoreCase))l.ag++;if(!string.IsNullOrEmpty(eW.cA))l.ag+=2;
l.hZ+=eW.ao.Count;foreach(var ti in eW.ao){l.rA++;l.rB++;l.ag+=2;switch(ti.h){case e.dO: l.eb++;
l.ba++;l.ag+=2;l.aC++;break;case e.d4: l.ba+=10;l.ag+=10;
l.aC+=10;break;case e.d0: l.ba+=10;l.ag+=11;l.aC+=10;break;case e.dT: l.ba+=10;
l.ag+=10;l.aC+=10;break;case e.dW: l.ba+=12;l.ag+=12;l.aC+=12;
break;case e.cx: l.eb++;l.ba+=8;l.ag+=9;l.aC+=8;break;case e.c_: l.eb+=2;
l.ba++;l.ag+=3;l.aC++;break;case e.cB: case e.cp: l.eb++;
l.ba++;l.ag+=2;l.aC++;break;case e.cz: case e.cr: l.eb++;l.ba+=2;
l.ag+=3;l.aC+=2;break;case e.cY: l.ba+=8;l.ag+=8;l.aC+=8;
break;default: l.aC++;break;}}}l.qM=25+(l.hZ*2);l.qN=180+(l.aC*4);
l.qO=0.0;return l;}}