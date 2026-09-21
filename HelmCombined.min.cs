h4 s;string im=null;Dictionary<string,List<IMyTerminalBlock>>bT=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>fQ=new Dictionary<string,bool>();
Dictionary<string,bq>ed=new Dictionary<string,bq>();Dictionary<string,Color>mj=new Dictionary<string,Color>();List<IMyTerminalBlock>cA=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>c7=new List<IMyTerminalBlock>();bool fN;int ij;string ee="";StringBuilder eh=new StringBuilder();readonly Dictionary<string,double>aK=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>c6=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener ef;bool cB=false;
Dictionary<z,IMyTextSurface>bU=new Dictionary<z,IMyTextSurface>();Dictionary<z,string>cC=new Dictionary<z,string>();
static readonly string[]lZ=new string[101];static Program(){for(int gt=0;gt<=100;gt++){lZ[gt]=gt+"%";}}static string lN(double uV){int eI=(int)Math.Round(uV);
if(eI<0)eI=0;else if(eI>100)eI=100;return lZ[eI];}class aw{public List<MySprite>f=new List<MySprite>();public List<ct>d6=new List<ct>();
public int cm= -1;public Color[]fm;public Color fi;public bool fj;public List<h3>bO=new List<h3>();public Vector2 d1,h0;
public int eb;public bool d_=false;public List<fn>dO=new List<fn>();}class h3{public string lY;public int h8;public Vector2 l2,l6;
}class fn{public int fA;public Color hB,ft;public List<aY>fI,fw;public string hF,hN,hO,hQ;public bool lT,hS;
}class ct{public bs fD;public int Z= -1;public int a1= -1;public int c2= -1;public bool lP=false;
public List<int>K=new List<int>();public float[]fK=new float[8];public float cl;public float bp;public float a7;
public float ck;public double fC= -99999.0;public string lV;public float E= -1f;public bool me;public float lD=12f;}Dictionary<z,aw>bV=new Dictionary<z,aw>();
int ml=0;double ip=0;double ei=0;double aa=0;int eg=0;Dictionary<z,double>bv=new Dictionary<z,double>();
Dictionary<z,double>ii=new Dictionary<z,double>();Dictionary<string,int>fT=new Dictionary<string,int>();List<string>c8=new List<string>();
Dictionary<string,int>fU=new Dictionary<string,int>();double ir;Dictionary<string,double>ek=new Dictionary<string,double>();List<MyInventoryItem>il=new List<MyInventoryItem>();
int bW= -1,fS;double iq;Dictionary<z,double>fO=new Dictionary<z,double>();int ej=0;const string qP="0.11.0";
IMyTextSurface a2;bool ih=true;double fP=0;string mk="";string io="";int fR=0,ik=0;List<KeyValuePair<string,int>>c5=new List<KeyValuePair<string,int>>();
const double q6=120;const float qk=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{ef=IGC.RegisterBroadcastListener("HELM");
ef.SetMessageCallback("HELM_IGC");}catch{}fH();h9();lS();}public void Main(string my,UpdateType uR){
double nq=Runtime.TimeSinceLastRun.TotalSeconds;aa+=nq;if((uR&UpdateType.IGC)!=0||my=="HELM_IGC"){q4();}string o5=(my ?? "").Trim();
string ao=o5.ToLowerInvariant();if(ao=="reload"){fH();}else if(ao=="refresh"){h6();}else if(ao=="boot"){h9();}else if(ao=="probe"){
ei=aa+20.0;qE();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(ao=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=rs;
fH();h9();}else if(ao=="console on"||ao=="console off"){ih=ao=="console on";lS();fP=0;}else if(ao=="scan"||ao=="diagnose"||ao=="test"){
rp();return;}else if(ao.StartsWith("set ")){string g_=o5.Substring(4).Trim();int nu=g_.IndexOf('=');int ur=g_.IndexOf(' ');int kV=nu>0 ? nu : ur;
if(kV>0){string oa=g_.Substring(0,kV).Trim();string p_=g_.Substring(kV+1).Trim();lw($"{oa}={p_}");Echo($"[HELM] Set dynamic variable '{oa}' = {p_}");
}return;}else if(ao.StartsWith("alert")){string fd=ao.Length>5 ? ao.Substring(5).Trim(): "toggle";if(fd=="toggle")cB= !cB;else cB=fd=="red"||fd=="on"||fd=="1"||fd=="true";
Echo($"[HELM] Red alert state: {cB}");return;}else if(Me.CustomData!=im){fH();}ej++;ip+=nq;if(ip>=q6){
ip=0;h6();}if(ei>0){if(aa<ei){ig();return;}ei=0;bV.Clear();cC.Clear();
fP=0;}if(s==null||s.T.Count==0){ig();lF();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cO=s.T[ml%s.T.Count];
ml++;io=cO.bd;bool iw=bv.Count>0;if(iw)rd();double nr;if(!bv.ContainsKey(cO)&& !(ii.TryGetValue(cO,out nr)&&aa<nr)){
if(ri(cO))iw=true;if(cO.h5>0)ii[cO]=aa+cO.h5/60.0;}eg=iw ? 0 : eg+1;Runtime.UpdateFrequency=eg<=s.T.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
rr();rq();ig();lF();fR=Runtime.CurrentInstructionCount;if(fR>ik)ik=fR;}void lF(){if(ee.Length>0)Echo(ee);
if(eh.Length>0)Echo(eh.ToString());}List<IMyTerminalBlock>fk(bool rC=false){if(!fN){c7.Clear();GridTerminalSystem.GetBlocks(c7);
cA.Clear();cA.AddRange(c7);cA.RemoveAll(rP=> !rP.IsSameConstructAs(Me));ij=c7.Count-cA.Count;fN=true;
}return rC ? c7 : cA;}IMyTerminalBlock lI(string oB,bool rE=false){var iF=GridTerminalSystem.GetBlockWithName(oB);if(iF==null||rE||iF.IsSameConstructAs(Me))return iF;
var iy=fk();for(int gu=0;gu<iy.Count;gu++)if(iy[gu].CustomName.IndexOf(oB,StringComparison.OrdinalIgnoreCase)>=0)return iy[gu];return null;}void qv(string oC,List<IMyTerminalBlock>eK,bool mr=false){
var nN=GridTerminalSystem.GetBlockGroupWithName(oC);if(nN==null)return;int mJ=eK.Count;var fa=new List<IMyTerminalBlock>();nN.GetBlocks(fa);lu(fa,eK,mr);
if(eK.Count>mJ)return;var nY=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(nY);foreach(var qe in nY){if(qe.Name.IndexOf(oC,StringComparison.OrdinalIgnoreCase)<0)continue;
fa.Clear();qe.GetBlocks(fa);lu(fa,eK,mr);if(eK.Count>mJ)return;}}void lu(List<IMyTerminalBlock>sJ,List<IMyTerminalBlock>n8,bool rD=false){
foreach(var iG in sJ)if((rD||iG.IsSameConstructAs(Me))&& !n8.Contains(iG))n8.Add(iG);}void fH(){im=Me.CustomData;s=lR.qY(im);
ee="";if(s.ba.Count>0){var pg=new StringBuilder("HELM config errors:\n");foreach(var ss in s.ba)pg.AppendLine(" - "+ss);ee=pg.ToString();
}if(s.ec.Count>0){var qc=new StringBuilder("HELM notes:\n");foreach(var u_ in s.ec)qc.AppendLine(" - "+u_);ee+=qc.ToString();}ed.Clear();
foreach(var iH in s.cn)if(!ed.ContainsKey(iH.af))ed[iH.af]=iH;bV.Clear();cC.Clear();bv.Clear();ii.Clear();
fO.Clear();qK();h6();}void q4(){try{while(ef!=null&&ef.HasPendingMessage){
var ox=ef.AcceptMessage();if(ox.Data is string){lw((string)ox.Data);}}}catch{}}void lw(string oV){
if(string.IsNullOrEmpty(oV))return;var oS=oV.Split(';');for(int jC=0;jC<oS.Length;jC++){var eW=oS[jC].Trim();if(eW.Length==0)continue;int ev=eW.IndexOf('=');
if(ev<0)ev=eW.IndexOf(':');if(ev>0){string dp=eW.Substring(0,ev).Trim();string bn=eW.Substring(ev+1).Trim();if(dp.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cB=bn.Equals("red",StringComparison.OrdinalIgnoreCase)||bn=="1"||bn.Equals("true",StringComparison.OrdinalIgnoreCase)||bn.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double oF;c6.Remove(dp);if(double.TryParse(bn,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out oF)){
aK[dp]=oF;}else if(bn.Equals("true",StringComparison.OrdinalIgnoreCase)||bn.Equals("on",StringComparison.OrdinalIgnoreCase)){aK[dp]=100.0;
}else if(bn.Equals("false",StringComparison.OrdinalIgnoreCase)||bn.Equals("off",StringComparison.OrdinalIgnoreCase)){aK[dp]=0.0;}else{c6[dp]=bn;
}}}}}const string rs="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void qE(){var iz=new List<IMyTextSurface>();if(a2!=null)iz.Add(a2);foreach(var tv in bU.Values)iz.Add(tv);foreach(var um in iz)qD(um);
}static void bt(List<MySprite>us,float sl,float sm,float u0,float sQ,Color r8){us.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(sl,sm),Size=new Vector2(u0,sQ),Color=r8}
);}void qD(IMyTextSurface kW){Vector2 aS=kW.SurfaceSize,aH=kW.TextureSize,am=(aH-aS)/2f;var aG=new List<MySprite>();Color gr=new Color(150,150,150);
bt(aG,aH.X/2f,2f,aH.X,4f,gr);bt(aG,aH.X/2f,aH.Y-2f,aH.X,4f,gr);bt(aG,2f,aH.Y/2f,4f,aH.Y,gr);bt(aG,aH.X-2f,aH.Y/2f,4f,aH.Y,gr);
bt(aG,am.X+aS.X/2f,am.Y+aS.Y/2f,aS.X,aS.Y,new Color(20,40,90));float al=32f;bt(aG,am.X+al/2f,am.Y+al/2f,al,al,new Color(255,0,0));bt(aG,am.X+aS.X-al/2f,am.Y+al/2f,al,al,new Color(0,255,0));
bt(aG,am.X+al/2f,am.Y+aS.Y-al/2f,al,al,new Color(255,255,0));bt(aG,am.X+aS.X-al/2f,am.Y+aS.Y-al/2f,al,al,new Color(255,0,255));float ls=am.Y+aS.Y/2f-30f;
a6(aG,"SURFACE "+aS.X.ToString("0")+"x"+aS.Y.ToString("0"),Color.White,am.X+44f,ls,0.9f);a6(aG,"TEXTURE "+aH.X.ToString("0")+"x"+aH.Y.ToString("0"),Color.White,am.X+44f,ls+24f,0.9f);
a6(aG,"OFFSET  "+am.X.ToString("0")+","+am.Y.ToString("0"),Color.White,am.X+44f,ls+48f,0.9f);using(var sC=kW.DrawFrame()){sC.AddRange(aG);}}static bool lU(string dj){
return dj.Length>3&&(dj.StartsWith("top")&&char.IsDigit(dj[3])||dj.Length>4&&dj.StartsWith("line")&&char.IsDigit(dj[4]));}void qK(){fT.Clear();c8.Clear();
fU.Clear();bW= -1;iq=ir=0;foreach(var kR in s.T){var j7=new List<List<bs>>();foreach(var tR in kR.aj)j7.Add(tR.an);
if(kR.aj.Count==0)j7.Add(kR.fE);foreach(var tq in j7)foreach(var eO in tq){if(!lU(eO.I))continue;bool pU=eO.I[0]=='t';var nj=pU ? fT : fU;
int tr=int.Parse(eO.I.Substring(pU ? 3 : 4));int n1;nj.TryGetValue(eO.aX,out n1);nj[eO.aX]=Math.Max(n1,tr);}}c8.AddRange(fT.Keys);
}void rq(){if(fU.Count==0||aa<ir)return;ir=aa+2.0;foreach(var jZ in fU){List<IMyTerminalBlock>iO;var pp=bT.TryGetValue(jZ.Key,out iO)&&iO.Count>0 ? iO[0]as IMyTextSurface : null;
string[]ou=pp==null ? new string[0]: pp.GetText().Split('\n');for(int eZ=1;eZ<=jZ.Value;eZ++)c6[jZ.Key+".line"+eZ]=eZ<=ou.Length ? ou[eZ-1].TrimEnd('\r'): "";}}
void rr(){if(c8.Count==0)return;if(bW<0){if(aa<iq)return;bW=0;fS=0;ek.Clear();}while(bW<c8.Count&&Runtime.CurrentInstructionCount<20000){
string iR=c8[bW];List<IMyTerminalBlock>f3;bT.TryGetValue(iR,out f3);if(f3!=null&&fS<f3.Count){var mA=f3[fS++];
for(int jD=0;jD<mA.InventoryCount;jD++){il.Clear();mA.GetInventory(jD).GetItems(il);foreach(var gB in il){string jV=gB.Type.TypeId.Replace("MyObjectBuilder_","");
string ob=jV=="Ore"||jV=="Ingot" ? gB.Type.SubtypeId+" "+jV.ToLowerInvariant(): gB.Type.SubtypeId;double n2;ek.TryGetValue(ob,out n2);ek[ob]=n2+(double)gB.Amount;
}}continue;}var gY=new List<KeyValuePair<string,double>>(ek);gY.Sort((u2,u8)=>u8.Value.CompareTo(u2.Value));for(int dx=1;dx<=fT[iR];dx++)c6[iR+".top"+dx]=dx<=gY.Count ? gY[dx-1].Key+" "+H.lM(gY[dx-1].Value,"auto"): "-";
ek.Clear();bW++;fS=0;}if(bW>=c8.Count){bW= -1;iq=aa+15.0;}}void ai(int uB,string uJ){c5.Add(new KeyValuePair<string,int>(uJ,uB));
}static string ie(string kK,int oA){return kK.Length>oA ? kK.Substring(0,oA): kK;}void lS(){var kD=Me as IMyTextSurfaceProvider;if(kD==null||kD.SurfaceCount<1)return;
a2=kD.GetSurface(0);a2.ContentType=ih ? ContentType.SCRIPT : ContentType.NONE;a2.Script="";a2.ScriptBackgroundColor=Color.Black;
}void ig(){if(aa>=fP){fP=aa+(eg>s.T.Count+1 ? 5.0 : 1.0);qs();if(ih&&a2!=null&&ei==0)qC();
}Echo(mk);}void qs(){c5.Clear();int iU=0,mQ=0;foreach(var mB in s.cn){List<IMyTerminalBlock>j1;if(mB.bM==a8.dV){
iU++;continue;}if(bT.TryGetValue(mB.af,out j1)){mQ+=j1.Count;if(j1.Count>0)iU++;}}int nv=s.ba.Count;ai(0,"HELM-[Kernel-(v"+qP+")]");
ai(4,"------------------------------");ai(1,"krn-cfg");ai(2," screens "+s.T.Count+"  binds "+iU+"/"+s.cn.Count+"  errors "+nv);
ai(2," alert "+(cB ? "RED" : "off")+"  blocks "+mQ+"  docked-ignored "+ij);if(nv>0)ai(3," ! "+ie(s.ba[0],40));
else if(s.ec.Count>0)ai(3," ? "+ie(s.ec[0],40));ai(1,"cfg-dsp");if(s.T.Count==0)ai(3," no @screen: run 'starter'");
int uo=0;foreach(var kL in s.T){if(++uo>10){ai(2," +"+(s.T.Count-10)+" more");break;}IMyTextSurface kX;string pr="--",k1="no block";
if(bU.TryGetValue(kL,out kX)){pr=kX.SurfaceSize.X.ToString("0")+"x"+kX.SurfaceSize.Y.ToString("0");k1=bv.ContainsKey(kL)? "boot" : "live";
}ai(k1=="no block" ? 3 : 2," "+ie(kL.bd,14).PadRight(14)+" "+pr.PadRight(9)+" "+k1);}ai(1,"perf");ai(2," tick "+ej+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ai(2," instr "+fR+"  peak "+ik+"  max "+Runtime.MaxInstructionCount);ai(2," rate "+Runtime.UpdateFrequency+"  idle "+eg+"  up "+(int)aa+"s");
if(io.Length>0)ai(2," last "+io);var ph=new StringBuilder();foreach(var s5 in c5)ph.AppendLine(s5.Key);mk=ph.ToString();}static Color qy(int uC){
switch(uC){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void qC(){Vector2 cP=a2.SurfaceSize;Vector2 ke=(a2.TextureSize-cP)/2f;int i2=30;foreach(var s6 in c5)i2=Math.Max(i2,s6.Key.Length);
Vector2 i1=a2.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float pi=Math.Min(1f,Math.Min(cP.X*0.94f/(i2*i1.X),cP.Y*0.94f/(c5.Count*i1.Y)));
float u3=ke.X+cP.X*0.03f;float qh=ke.Y+cP.Y*0.03f;var kY=new List<MySprite>();kY.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=ke+cP/2f,Size=cP,Color=new Color(12,12,16)}
);foreach(var og in c5){a6(kY,og.Key,qy(og.Value),u3,qh,pi);qh+=i1.Y*pi;}using(var sD=a2.DrawFrame()){sD.AddRange(kY);}}static readonly List<string>qr=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void h9(){bv.Clear();if(s==null)return;for(int gv=0;gv<s.T.Count;gv++){var g5=s.T[gv];if(g5.hI>0f)bv[g5]=aa+(gv/Math.Max(1,g5.ly))*g5.lB/60.0;
}}void rd(){List<z>eu=null;int sP=(bv.Count+3)/4,sZ=0;foreach(var gF in bv){double km=(aa-gF.Value)/gF.Key.hI;if(km<0)continue;
if(km>=1.0){if(eu==null)eu=new List<z>();eu.Add(gF.Key);continue;}if((sZ++ +ej)%sP==0)qB(gF.Key,km);}if(eu==null)return;foreach(var e5 in eu){
bv.Remove(e5);if(e5.hH>0f)fO[e5]=aa;bV.Remove(e5);cC.Remove(e5);}}static void a6(List<MySprite>ut,string uK,Color sb,float u4,float u9,float uj){
var pH=MySprite.CreateText(uK,"Monospace",sb,uj,TextAlignment.LEFT);pH.Position=new Vector2(u4,u9);ut.Add(pH);}static void lt(List<MySprite>uu,List<string>fZ,z g8,float u5,float va,float uk,float th){
string[]uz=g8.fo!=null&&g8.fo.Length>0 ? g8.fo : new[]{g8.lz};for(int eD=0;eD<fZ.Count;
eD++){o iW=H.lO(uz,fZ.Count>1 ? 100.0*eD/(fZ.Count-1): 0.0);a6(uu,fZ[eD],new Color(iW.bP,iW.bL,iW.bG),u5,va+eD*th,uk);}}bool rh(z dz,IMyTextSurface k5){
aw aA,aN;z pl=s.T.Find(qf=>qf!=dz&&qf.bd.Equals(dz.hX,StringComparison.OrdinalIgnoreCase));if(pl==null|| !bV.TryGetValue(pl,out aA)|| !aA.d_)return false;
if(!bV.TryGetValue(dz,out aN))bV[dz]=aN=new aw();Vector2 dC=k5.SurfaceSize;if(aN.d_&&aN.eb==aA.eb&&aN.d1==dC)return false;
aN.d_=true;aN.eb=aA.eb;aN.d1=dC;aN.f.Clear();Vector2 hh=aA.d1,oJ=aA.h0,oH=(k5.TextureSize-dC)/2f;aN.h0=oH;
int nK=0,pT=aA.f.Count;if(dz.hY!=null){int dw=aA.bO.FindIndex(u6=>u6.lY.Equals(dz.hY,StringComparison.OrdinalIgnoreCase));
if(dw<0)return false;hh=aA.bO[dw].l6;oJ=aA.bO[dw].l2;nK=aA.bO[dw].h8;if(dw+1<aA.bO.Count)pT=aA.bO[dw+1].h8;}float gD=Math.Min(dC.X/hh.X,dC.Y/hh.Y);
Vector2 rM=oH+(dC-hh*gD)/2f;for(int jE=nK;jE<pT;jE++){var bz=aA.f[jE];if(bz.Position.HasValue)bz.Position=(bz.Position.Value-oJ)*gD+rM;if(bz.Size.HasValue)bz.Size=bz.Size.Value*gD;
if(bz.Type==SpriteType.TEXT)bz.RotationOrScale*=gD;aN.f.Add(bz);}using(var sE=k5.DrawFrame())sE.AddRange(aN.f);return true;}bool rb(z dA,IMyTextSurface hf){
List<string>em;if(cC.ContainsKey(dA)|| !s.dL.TryGetValue(dA.cj,out em))return false;cC[dA]="banner";Vector2 cQ=hf.SurfaceSize;
Vector2 kf=(hf.TextureSize-cQ)/2f;int f9=1;foreach(var s7 in em)f9=Math.Max(f9,s7.Length);Vector2 es=hf.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float g7=Math.Min(cQ.X*0.94f/(f9*es.X),cQ.Y*0.94f/(em.Count*es.Y));var kZ=new List<MySprite>();kZ.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kf+cQ/2f,Size=cQ,Color=string.IsNullOrEmpty(dA.bH)? new Color(10,12,16): F(dA.bH)}
);lt(kZ,em,dA,kf.X+(cQ.X-f9*es.X*g7)/2f,kf.Y+(cQ.Y-em.Count*es.Y*g7)/2f,g7,es.Y*g7);using(var sF=hf.DrawFrame()){sF.AddRange(kZ);
}return true;}void qB(z dB,double oM){IMyTextSurface e7;if(!bU.TryGetValue(dB,out e7))return;Vector2 cf=e7.SurfaceSize;
Vector2 kg=(e7.TextureSize-cf)/2f;var eP=dB.qL();string oy=dB.lA ?? "HELM TELEMETRY KERNEL ONLINE";bool j8=cf.Y>=200f;
List<string>c9;if(dB.co==null|| !s.dL.TryGetValue(dB.co,out c9))c9=qr;int dg=oy.Length+3;foreach(var s8 in eP)dg=Math.Max(dg,s8.Length+5);
if(j8)foreach(var s9 in c9)dg=Math.Max(dg,s9.Length);int o9=(j8 ? c9.Count+1 : 0)+2+eP.Count;Vector2 df=e7.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float cb=Math.Min(1f,Math.Min(cf.X*0.94f/(dg*df.X),cf.Y*0.94f/(o9*df.Y)));float hw=kg.X+(cf.X-dg*df.X*cb)/2f;float cV=kg.Y+(cf.Y-o9*df.Y*cb)/2f;
float gJ=df.Y*cb;var dD=new List<MySprite>();dD.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kg+cf/2f,Size=cf,Color=new Color(10,12,16)}
);if(j8){lt(dD,c9,dB,hw,cV,cb,gJ);cV+=(c9.Count+1)*gJ;}a6(dD,">> "+oy,new Color(34,197,94),hw,cV,cb);cV+=gJ*2f;int up=H.qq(oM,eP.Count);
for(int gw=0;gw<up;gw++){bool oI=gw==eP.Count-1&&oM>=0.8;a6(dD,oI ? "[OK]" : "[..]",oI ? new Color(34,197,94): new Color(56,189,248),hw,cV,cb);a6(dD,eP[gw],new Color(203,213,225),hw+5f*df.X*cb,cV,cb);
cV+=gJ;}using(var sG=e7.DrawFrame()){sG.AddRange(dD);}}void h6(){bT.Clear();bU.Clear();fQ.Clear();
eh.Clear();fN=false;if(s==null)return;foreach(var ad in s.cn){var a3=new List<IMyTerminalBlock>();if(ad.bM==a8.dV){
string jT= !string.IsNullOrEmpty(ad.br)? ad.br : ad.af;if(!aK.ContainsKey(jT)){aK[jT]=ad.fu;if(ad.hK!=null)c6[jT]=ad.hK;
}bT[ad.af]=a3;continue;}var pL=ad.a0.Count>0 ? ad.a0 : new List<string>{ad.mb};foreach(var cT in pL){
if(string.IsNullOrEmpty(cT))continue;bool n9=cT.Contains("*")||cT.Contains("?");if(ad.bM==a8.hT||n9){var jw=new List<IMyTerminalBlock>();
qv(cT,jw,ad.fl);if(jw.Count>0){foreach(var nU in jw){if(!a3.Contains(nU))a3.Add(nU);}}else{var mt=fk(ad.fl);
for(int iI=0;iI<mt.Count;iI++){var hn=mt[iI];if(n9 ? H.qU(cT,hn.CustomName): string.Equals(cT,hn.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!a3.Contains(hn))a3.Add(hn);}}}}else{var iN=lI(cT,ad.fl);if(iN!=null&& !a3.Contains(iN))a3.Add(iN);}}if(a3.Count==0){
rn(ad,pL,a3);if(a3.Count>0)fQ[ad.af]=true;}bT[ad.af]=a3;}foreach(var bD in s.T){
if(string.IsNullOrEmpty(bD.bN))continue;var kj=lI(bD.bN);if(kj==null){eh.AppendLine($"HELM: output block '{bD.bN}' not found");
continue;}IMyTextSurface cS=null;var kE=kj as IMyTextSurfaceProvider;var nk=kj as IMyTextSurface;if(kE!=null&&bD.cu<kE.SurfaceCount){
cS=kE.GetSurface(bD.cu);}else if(nk!=null&&bD.cu==0){cS=nk;}if(cS==null){eh.AppendLine($"HELM: output block '{bD.bN}' has no surface index {bD.cu}");
continue;}cS.ContentType=ContentType.SCRIPT;cS.Script="";cS.ScriptBackgroundColor=Color.Black;bU[bD]=cS;}cA.Clear();
c7.Clear();}static readonly string[]lH={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool qJ(int s_,IMyTerminalBlock v){switch(s_){case 0: return v is IMyBatteryBlock;case 1: return v is IMyReactor;case 2: return v is IMySolarPanel;
case 3: return v is IMyWindTurbine;case 4: return v is IMyGasTank;case 5: return v is IMyAirVent;case 6: return v is IMyCargoContainer;case 7: return v is IMyJumpDrive;
case 8: return v is IMyThrust;case 9: return v is IMyMotorSuspension;case 10: return v is IMyDoor;case 11: return v is IMyShipConnector;case 12: return v is IMyAssembler;
case 13: return v is IMyRefinery;case 14: return v is IMyShipDrill;case 15: return v is IMyShipWelder;case 16: return v is IMyShipGrinder;case 17: return v is IMyGasGenerator;
case 18: return v is IMyLargeTurretBase;case 19: return v is IMyProjector;case 20: return v is IMyGyro;case 21: return v is IMyGravityGeneratorBase;case 22: return v is IMyLandingGear;
case 23: return v is IMyRadioAntenna;case 24: return v is IMyBeacon;case 25: return v is IMyLightingBlock;case 26: return v is IMyPistonBase;case 27: return v is IMyMotorStator;
case 28: return v is IMyWarhead;case 29: return v is IMySensorBlock;case 30: return v is IMyCameraBlock;case 31: return v is IMyParachute;case 32: return v is IMyTurretControlBlock;
}return false;}void rn(bq r0,List<string>uH,List<IMyTerminalBlock>iP){string sU=(r0.af+" "+string.Join(" ",uH)).ToLowerInvariant();
var jX=new List<int>();for(int gE=0;gE<lH.Length;gE++){foreach(var u1 in lH[gE].Split(','))if(sU.Contains(u1)){jX.Add(gE);break;}}if(jX.Count==0)return;
var ms=fk();var oK=new List<IMyTerminalBlock>();for(int jF=0;jF<ms.Count;jF++){var iJ=ms[jF];foreach(int s0 in jX){if(qJ(s0,iJ)){(iJ.CubeGrid==Me.CubeGrid ? iP : oK).Add(iJ);
break;}}}if(iP.Count==0)iP.AddRange(oK);}void rp(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(s==null||s.T.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {s.T.Count} screens, {s.cn.Count} binds");fN=false;
Echo($"Blocks: {fk().Count} on this construct; {ij} on docked ships (ignored)");cA.Clear();Echo("--- BIND RESOLUTION ---");foreach(var aq in s.cn){
if(aq.bM==a8.dV){string jU= !string.IsNullOrEmpty(aq.br)? aq.br : aq.af;double uS=aK.ContainsKey(jU)? aK[jU]: aq.fu;
Echo($"[OK] @bind {aq.af} (dynamic variable '{jU}' = {uS})");continue;}List<IMyTerminalBlock>iQ;if(bT.TryGetValue(aq.af,out iQ)){if(iQ.Count>0){
string sy=fQ.ContainsKey(aq.af)&&fQ[aq.af]? " [fallback: typed match]" : "";Echo($"[OK] @bind {aq.af} ({iQ.Count} blocks matched{sy})");
}else{string uI=string.Join(", ",aq.a0.Count>0 ? aq.a0 : new List<string>{aq.mb});Echo($"[WARN] @bind {aq.af} -> 0 blocks matched \"{uI}\"");
}}else{Echo($"[ERR] @bind {aq.af} -> unresolved");}}if(aK.Count>0){Echo($"--- DYNAMIC VARIABLES ({aK.Count}) ---");foreach(var oi in aK)Echo($"[VAR] {oi.Key} = {oi.Value}");
}IMyTextSurface iA=null;foreach(var ty in bU.Values){iA=ty;break;}if(iA!=null){var k_=new List<string>();iA.GetSprites(k_);
Echo($"--- SPRITES ({k_.Count}): names usable in bg_texture ---");Echo(string.Join(", ",k_.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bE in s.T){IMyTextSurface hg;if(bU.TryGetValue(bE,out hg)){var ps=hg.SurfaceSize;var ux=H.qx(bE);
Echo($"[OK] Screen \"{bE.bd}\" [{bE.cv}] -> {bE.bN} [#{bE.cu}] ({ps.X:0}x{ps.Y:0} px, texture {hg.TextureSize.X:0}x{hg.TextureSize.Y:0}, {ux.ag} sprites)");
}else{Echo($"[ERR] Screen \"{bE.bd}\" -> Missing block \"{bE.bN}\" [#{bE.cu}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double ro(IMyTerminalBlock w){
if(w==null)return 0.0;var f1=w as IMyBatteryBlock;if(f1!=null)return f1.MaxStoredPower>0 ?(f1.CurrentStoredPower/f1.MaxStoredPower)*100.0 : 0.0;
var gC=w as IMyJumpDrive;if(gC!=null)return gC.MaxStoredPower>0 ?(gC.CurrentStoredPower/gC.MaxStoredPower)*100.0 : 0.0;var pK=w as IMyGasTank;
if(pK!=null)return pK.FilledRatio*100.0;var p8=w as IMyAirVent;if(p8!=null)return p8.GetOxygenLevel()*100.0;var mY=w as IMyCargoContainer;if(mY!=null){
var gz=mY.GetInventory();if(gz!=null&&gz.MaxVolume.RawValue>0)return(double)gz.CurrentVolume.RawValue/gz.MaxVolume.RawValue*100.0;return 0.0;}var gZ=w as IMyReactor;
if(gZ!=null)return gZ.MaxOutput>0 ?(gZ.CurrentOutput/gZ.MaxOutput)*100.0 : 0.0;var g9=w as IMySolarPanel;if(g9!=null)return g9.MaxOutput>0 ?(g9.CurrentOutput/g9.MaxOutput)*100.0 : 0.0;
var hv=w as IMyWindTurbine;if(hv!=null)return hv.MaxOutput>0 ?(hv.CurrentOutput/hv.MaxOutput)*100.0 : 0.0;var nl=w as IMyDoor;if(nl!=null)return(double)nl.OpenRatio*100.0;
var hq=w as IMyThrust;if(hq!=null)return hq.MaxEffectiveThrust>0 ?(hq.CurrentThrust/hq.MaxEffectiveThrust)*100.0 : 0.0;var la=w as IMyMotorSuspension;
if(la!=null)return la.Brake ? 100.0 :(la.Friction*100.0);var i6=w as IMyShipConnector;if(i6!=null)return i6.Status==MyShipConnectorStatus.Connected ? 100.0 :(i6.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var o_=w as IMyProductionBlock;if(o_!=null)return o_.IsProducing ? 100.0 : 0.0;var np=w as IMyShipDrill;if(np!=null)return np.Enabled ? 100.0 : 0.0;
var lg=w as IMyShipToolBase;if(lg!=null)return lg.IsActivated ? 100.0 :(lg.Enabled ? 50.0 : 0.0);var jt=w as IMyGasGenerator;if(jt!=null)return jt.AutoRefill ? 100.0 :(jt.Enabled ? 50.0 : 0.0);
var hr=w as IMyLargeTurretBase;if(hr!=null)return hr.IsShooting ? 100.0 :(hr.HasTarget ? 50.0 :(hr.Enabled ? 10.0 : 0.0));var eY=w as IMyProjector;
if(eY!=null)return eY.TotalBlocks>0 ?((double)(eY.TotalBlocks-eY.RemainingBlocks)/eY.TotalBlocks)*100.0 : 0.0;var jz=w as IMyGyro;if(jz!=null)return jz.GyroOverride ? 100.0 :(jz.Enabled ? 50.0 : 0.0);
var nW=w as IMyGravityGeneratorBase;if(nW!=null)return Math.Min(100.0,Math.Abs(nW.GravityAcceleration)/9.81*100.0);var js=w as IMyLandingGear;if(js!=null)return js.IsLocked ? 100.0 :(js.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var mu=w as IMyRadioAntenna;if(mu!=null)return(mu.Radius/50000.0)*100.0;var mH=w as IMyBeacon;if(mH!=null)return(mH.Radius/50000.0)*100.0;
var os=w as IMyLightingBlock;if(os!=null)return os.Intensity*10.0;var gV=w as IMyPistonBase;if(gV!=null)return gV.HighestPosition>0 ?(gV.CurrentPosition/gV.HighestPosition)*100.0 : 0.0;
var o7=w as IMyMotorStator;if(o7!=null)return Math.Min(100.0,Math.Abs(o7.Angle)/6.28318*100.0);var qb=w as IMyWarhead;if(qb!=null)return qb.IsArmed ? 100.0 : 0.0;
var kU=w as IMySensorBlock;if(kU!=null)return!kU.LastDetectedEntity.IsEmpty()? 100.0 :(kU.Enabled ? 25.0 : 0.0);var iZ=w as IMyCameraBlock;
if(iZ!=null)return iZ.IsActive ? 100.0 :(iZ.Enabled ? 50.0 : 0.0);var m3=w as IMyParachute;if(m3!=null)return(double)m3.OpenRatio*100.0;
var hp=w as IMyTurretControlBlock;if(hp!=null)return hp.HasTarget ? 100.0 :(hp.IsUnderControl ? 50.0 :(hp.Enabled ? 10.0 : 0.0));var nL=w as IMyFunctionalBlock;
if(nL!=null)return nL.Enabled ? 100.0 : 0.0;return 0.0;}static double qR(IMyTerminalBlock mC,string jl){int mX=jl.IndexOf(':');string jW=jl.Substring(0,mX).ToLowerInvariant();
var uP=new MyItemType("MyObjectBuilder_"+(jW=="ammo" ? "AmmoMagazine" : char.ToUpper(jW[0])+jW.Substring(1)),jl.Substring(mX+1));double pD=0;for(int jG=0;
jG<mC.InventoryCount;jG++)pD+=(double)mC.GetInventory(jG).GetItemAmount(uP);return pD;}double l3(IMyTerminalBlock x,string nB){if(x==null)return 0.0;
string b=nB.ToLowerInvariant();if(b=="percent"||b=="pct")return ro(x);if(b.IndexOf(':')>0)return qR(x,nB);var bZ=x as IMyBatteryBlock;
if(bZ!=null){if(b=="current"||b=="stored"||b=="val"||b=="value"||b=="raw")return bZ.CurrentStoredPower;if(b=="max"||b=="capacity")return bZ.MaxStoredPower;
if(b=="output")return bZ.CurrentOutput;if(b=="maxoutput")return bZ.MaxOutput;if(b=="input")return bZ.CurrentInput;if(b=="maxinput")return bZ.MaxInput;
return bZ.CurrentStoredPower;}var jS=x as IMyJumpDrive;if(jS!=null){if(b=="max"||b=="capacity")return jS.MaxStoredPower;return jS.CurrentStoredPower;
}var hk=x as IMyGasTank;if(hk!=null){if(b=="max"||b=="capacity")return hk.Capacity;return hk.FilledRatio*hk.Capacity;}var p9=x as IMyAirVent;
if(p9!=null){if(b=="max"||b=="capacity")return 100.0;return p9.GetOxygenLevel()*100.0;}var mZ=x as IMyCargoContainer;if(mZ!=null){var gA=mZ.GetInventory();
if(gA!=null){if(b=="mass")return(double)gA.CurrentMass;if(b=="max"||b=="capacity")return(double)gA.MaxVolume;return(double)gA.CurrentVolume;}return 0.0;}var kF=x as IMyPowerProducer;
if(kF!=null){if(b=="max"||b=="capacity")return kF.MaxOutput;return kF.CurrentOutput;}var nm=x as IMyDoor;if(nm!=null){if(b=="max")return 1.0;return(double)nm.OpenRatio*100.0;
}var le=x as IMyThrust;if(le!=null){if(b=="max"||b=="capacity")return le.MaxEffectiveThrust;return le.CurrentThrust;}var ch=x as IMyMotorSuspension;
if(ch!=null){if(b=="brake")return ch.Brake ? 100.0 : 0.0;if(b=="height")return ch.Height;if(b=="strength")return ch.Strength;if(b=="power")return ch.Power;
if(b=="steer")return ch.SteeringOverride*100.0;if(b=="propulsion")return ch.PropulsionOverride*100.0;return ch.Friction;}var nb=x as IMyShipConnector;
if(nb!=null){return nb.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var kC=x as IMyProductionBlock;if(kC!=null){if(b=="producing"||b=="active"||b=="running"||b=="state")return kC.IsProducing ? 100.0 : 0.0;
if(b=="queue")return kC.IsQueueEmpty ? 0.0 : 1.0;}var iE=x as IMyAssembler;if(iE!=null){if(b=="disassembling")return iE.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(b=="repeating"||b=="loop")return iE.Repeating ? 100.0 : 0.0;}var ju=x as IMyGasGenerator;if(ju!=null){if(b=="autorefill")return ju.AutoRefill ? 100.0 : 0.0;
if(b=="active")return ju.Enabled ? 100.0 : 0.0;}var hs=x as IMyLargeTurretBase;if(hs!=null){if(b=="shooting")return hs.IsShooting ? 100.0 : 0.0;
if(b=="target"||b=="hastarget")return hs.HasTarget ? 100.0 : 0.0;if(b=="range")return hs.Range;}var ca=x as IMyProjector;if(ca!=null){if(b=="built")return ca.TotalBlocks-ca.RemainingBlocks;
if(b=="remaining")return ca.RemainingBlocks;if(b=="total")return ca.TotalBlocks;if(b=="projecting")return ca.IsProjecting ? 100.0 : 0.0;if(b=="armor")return ca.RemainingArmorBlocks;
if(b=="buildable")return ca.BuildableBlocksCount;}var lh=x as IMyShipToolBase;if(lh!=null){if(b=="working"||b=="active"||b=="activated")return lh.IsActivated ? 100.0 : 0.0;
return lh.Enabled ? 100.0 : 0.0;}var dl=x as IMyGyro;if(dl!=null){if(b=="override")return dl.GyroOverride ? 100.0 : 0.0;if(b=="pitch")return dl.Pitch;
if(b=="yaw")return dl.Yaw;if(b=="roll")return dl.Roll;if(b=="power")return dl.GyroPower*100.0;}var nX=x as IMyGravityGeneratorBase;if(nX!=null){if(b=="g"||b=="accel")return nX.GravityAcceleration;
}var nV=x as IMyLandingGear;if(nV!=null){if(b=="locked")return nV.IsLocked ? 100.0 : 0.0;}var mv=x as IMyRadioAntenna;if(mv!=null){if(b=="radius"||b=="range")return mv.Radius;
}var mI=x as IMyBeacon;if(mI!=null){if(b=="radius"||b=="range")return mI.Radius;}var j4=x as IMyLightingBlock;if(j4!=null){if(b=="intensity")return j4.Intensity;
if(b=="radius")return j4.Radius;}var gW=x as IMyPistonBase;if(gW!=null){if(b=="pos"||b=="position"||b=="extension")return gW.CurrentPosition;
if(b=="velocity")return gW.Velocity;if(b=="max")return gW.HighestPosition;}var e4=x as IMyMotorStator;if(e4!=null){if(b=="angle"||b=="deg")return e4.Angle*180.0/Math.PI;
if(b=="rad")return e4.Angle;if(b=="velocity"||b=="rpm")return e4.TargetVelocityRPM;if(b=="locked")return e4.RotorLock ? 100.0 : 0.0;}var lq=x as IMyWarhead;
if(lq!=null){if(b=="armed")return lq.IsArmed ? 100.0 : 0.0;if(b=="countdown")return lq.DetonationTime;}var po=x as IMySensorBlock;if(po!=null){
if(b=="detected")return!po.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var i_=x as IMyCameraBlock;if(i_!=null){if(b=="range")return i_.AvailableScanRange;
if(b=="active")return i_.IsActive ? 100.0 : 0.0;}var m4=x as IMyParachute;if(m4!=null){if(b=="open"||b=="openratio")return(double)m4.OpenRatio*100.0;
}var dH=x as IMyTurretControlBlock;if(dH!=null){if(b=="target"||b=="hastarget")return dH.HasTarget ? 100.0 : 0.0;if(b=="azimuth")return dH.AzimuthRotor!=null ? dH.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(b=="elevation")return dH.ElevationRotor!=null ? dH.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var nM=x as IMyFunctionalBlock;if(nM!=null){return nM.Enabled ? 100.0 : 0.0;
}return 0.0;}double bu(string ep,string nC){bq b_;ed.TryGetValue(ep,out b_);if(b_!=null&&b_.bM==a8.dV){
string s1= !string.IsNullOrEmpty(b_.br)? b_.br : ep;double ne;if(aK.TryGetValue(s1,out ne))return ne;return b_.fu;
}double ns;if(aK.TryGetValue(ep,out ns))return ns;List<IMyTerminalBlock>eq;if(!bT.TryGetValue(ep,out eq)||eq.Count==0)throw new InvalidOperationException($"HELM: bind '{ep}' resolved to zero blocks — check the name/typo");
if(eq.Count==1)return l3(eq[0],nC);var p7=new List<double>();foreach(var rQ in eq)p7.Add(l3(rQ,nC));return qm.ql(b_!=null ? b_.dK : aI.hA,p7);
}double h7(string r2){return bu(r2,"percent");}static string bb(double p3,bs ds){if(ds==null||(string.IsNullOrEmpty(ds.ea)&&string.IsNullOrEmpty(ds.dW)&&ds.I=="percent")){
return lN(p3);}return H.lM(p3,ds.ea,ds.dW);}bool ri(z cc){IMyTextSurface e8;if(!bU.TryGetValue(cc,out e8))return false;
if(cc.cj!=null)return rb(cc,e8);if(cc.hX!=null)return rh(cc,e8);if(cc.cZ.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return rj(cc,e8);}return rk(cc,e8);}Color ax(bs oj,Color sn){return oj.id!=null ? F(oj.id): sn;
}Color fx(string[]dE,double tH){if(dE.Length==1)return F(dE[0]);double o2=Math.Max(0.0,Math.Min(100.0,tH))/100.0*(dE.Length-1);int jH=Math.Min((int)o2,dE.Length-2);
float hi=(float)(o2-jH);Color bX=F(dE[jH]),f0=F(dE[jH+1]);return new Color((int)(bX.R+(f0.R-bX.R)*hi),(int)(bX.G+(f0.G-bX.G)*hi),(int)(bX.B+(f0.B-bX.B)*hi),(int)(bX.A+(f0.A-bX.A)*hi));
}static Color[]fh(aw r6,string[]uA,string sp,float qg,float qi,float ln,float jA){int en=16;bool n3=sp=="horizontal";var i5=new Color[en];
for(int cI=0;cI<en;cI++){o jr=H.lO(uA,100.0*cI/(en-1));i5[cI]=new Color(jr.bP,jr.bL,jr.bG);float mW=ln/en,mP=jA/en;t(r6,n3 ? new Vector2(qg+mW*(cI+0.5f),qi+jA/2f): new Vector2(qg+ln/2f,qi+mP*(cI+0.5f)),n3 ? new Vector2(mW+1f,jA): new Vector2(ln,mP+1f),i5[cI]);
}return i5;}static void t(aw r7,Vector2 tT,Vector2 uq,Color sc,float ua=0f,string so="SquareSimple"){r7.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=so,Position=tT,Size=uq,Color=sc,RotationOrScale=ua}
);}void hy(aw b0,string cR,string[]aM,string m7,bool pv,float aD,float aT,float bC,float be){cR=cR.ToLowerInvariant();float P=cR=="heavy" ? 4f : 2f,fg=bC-aD,jB=be-aT;
Color bw=F(aM!=null&&aM.Length>0 ? aM[0]: m7 ?? "40, 60, 85");Color b1=aM!=null&&aM.Length>1 ? F(aM[aM.Length-1]): bw;Action<float,float,float,float,Color>aQ=(u7,vb,o1,o0,r9)=>t(b0,new Vector2(u7+o1/2f,vb+o0/2f),new Vector2(o1,o0),r9);
if(cR=="corners"){float aO=Math.Max(10f,Math.Min(fg,jB)*0.15f);aQ(aD,aT,aO,P,bw);aQ(aD,aT,P,aO,bw);aQ(bC-aO,aT,aO,P,b1);aQ(bC-P,aT,P,aO,b1);aQ(aD,be-P,aO,P,bw);aQ(aD,be-aO,P,aO,bw);
aQ(bC-aO,be-P,aO,P,b1);aQ(bC-P,be-aO,P,aO,b1);return;}if(aM!=null&&aM.Length>1&&pv){fh(b0,aM,"horizontal",aD,aT,fg,P);fh(b0,aM,"horizontal",aD,be-P,fg,P);
}else{aQ(aD,aT,fg,P,bw);aQ(aD,be-P,fg,P,b1);}if("|frame|single|heavy|double|rounded|".Contains("|"+cR+"|")){aQ(aD,aT,P,jB,bw);aQ(bC-P,aT,P,jB,b1);}if(cR=="rounded"){
float b2=P*3f;t(b0,new Vector2(aD,aT),new Vector2(b2,b2),bw,0f,"Circle");t(b0,new Vector2(bC,aT),new Vector2(b2,b2),b1,0f,"Circle");t(b0,new Vector2(aD,be),new Vector2(b2,b2),bw,0f,"Circle");
t(b0,new Vector2(bC,be),new Vector2(b2,b2),b1,0f,"Circle");}if(cR=="double")hy(b0,"single",aM,m7,pv,aD+5f,aT+5f,bC-5f,be-5f);}void qt(z l,IMyTextSurface k6,aw g){
g.f.Clear();g.d6.Clear();g.bO.Clear();Vector2 W=k6.SurfaceSize;Vector2 uM=k6.TextureSize;Vector2 bo=(uM-W)/2f;
g.h0=bo;float gO=Math.Max(4f,W.X*0.02f);float eV=Math.Max(4f,W.Y*0.02f);if(l.cv=="compact"||(l.cv=="auto"&&W.Y<=260f&&W.Y>150f)){
gO=4f;eV=4f;}else if(l.cv=="corner"||(l.cv=="auto"&&W.Y<=150f)){gO=2f;eV=2f;}float pX=W.X-(gO*2f);
Color db=new Color(10,12,16);bool no=true;if(!string.IsNullOrEmpty(l.bH)){var eB=o.d3(l.bH);if(eB.fB)no=false;
else db=new Color(eB.bP,eB.bL,eB.bG,eB.hx);}g.cm= -1;g.fj=false;g.fi=F(l.lv ?? "#5b0f0f");g.dO.Clear();if(l.bJ!=null&&l.bJ.Length>1){
g.cm=g.f.Count;g.fm=fh(g,l.bJ,l.dN,bo.X+2f,bo.Y+2f,W.X-4f,W.Y-4f);
}else if(no||l.aW!=null||l.c_!=null){if(l.aW!=null)db=F(l.aW.First(tY=>tY.c1).cq);g.cm=g.f.Count;
g.fm=new[]{db};t(g,bo+(W/2f),W-new Vector2(4f,4f),db);if(l.aW!=null||l.c_!=null)g.dO.Add(new fn{
fA=g.cm,hB=db,ft=db,fI=l.aW,hF=l.hC,hN=l.hD,fw=l.c_,hO=l.hP,hQ=l.hR,lT=true}
);}if(!string.IsNullOrEmpty(l.hE))t(g,bo+(W/2f),W-new Vector2(4f,4f),F(l.lx ?? "255,255,255,24"),0f,l.hE);
if(!l.cZ.Equals("none",StringComparison.OrdinalIgnoreCase)){float mR=bo.X+(W.X-pX)/2f;hy(g,l.cZ,l.fs,l.dQ,!l.fz,mR-2f,bo.Y+eV*0.5f-1f,mR+pX+2f,bo.Y+W.Y-eV*0.5f+1f);
}var kr=l.aj;if(kr.Count==0){kr=new List<aC>{new aC{c4="main",dS=0,d5=0,d7=l.dX,d8=l.dY,an=l.fE}
};}float nZ=8f;foreach(var q in kr){var gN=H.qw(q.dS,q.d5,q.d7,q.d8,l.dX,l.dY,W.X,W.Y,gO,eV,nZ,nZ);
float ay=bo.X+gN.mh;float bA=bo.Y+gN.mi;float aP=gN.mg;float cL=gN.lQ;g.bO.Add(new h3{lY=q.c4,h8=g.f.Count,l2=new Vector2(ay,bA),l6=new Vector2(aP,cL)}
);bool pa=q.aW!=null||q.c_!=null;if(q.bJ!=null&&q.bJ.Length>1){fh(g,q.bJ,q.dN!="vertical" ? q.dN : l.dN,ay,bA,aP,cL);
}else{Color b9;if(q.aW!=null)b9=F(q.aW.First(tZ=>tZ.c1).cq);else if(!string.IsNullOrEmpty(q.cX))b9=F(q.cX);
else if(!string.IsNullOrEmpty(l.h2))b9=F(l.h2);else b9=Color.Transparent;if(b9.A>0||pa){if(pa)g.dO.Add(new fn{fA=g.f.Count,hB=b9,ft=b9,fI=q.aW,hF=q.hC,hN=q.hD,fw=q.c_,hO=q.hP,hQ=q.hR}
);t(g,new Vector2(ay+aP/2f,bA+cL/2f),new Vector2(aP,cL),b9);}}if(q.dP!=null&& !q.dP.Equals("none",StringComparison.OrdinalIgnoreCase))hy(g,q.dP,q.fr ?? l.fs,q.fq ?? l.dQ,!l.fz,ay+1f,bA+1f,ay+aP-1f,bA+cL-1f);
string go=q.lJ ?? l.lK ?? "Monospace";float rZ=H.qM(l.cv,W.X,W.Y,l.c0);float aL=q.lL ?? rZ;
float eC=0f;if(!string.IsNullOrEmpty(q.cy)){eC=20f*aL;var ix=TextAlignment.LEFT;float lf=ay+4f;string mn=q.ib ?? l.mc ?? "left";
if(mn.Equals("center",StringComparison.OrdinalIgnoreCase)){ix=TextAlignment.CENTER;lf=ay+(aP/2f);}else if(mn.Equals("right",StringComparison.OrdinalIgnoreCase)){
ix=TextAlignment.RIGHT;lf=ay+aP-4f;}string pN=q.ic ?? l.md;Color uN= !string.IsNullOrEmpty(pN)? F(pN): F(q.fq ?? l.dQ ?? "white");
var pS=MySprite.CreateText(q.cy,go,uN,aL*0.9f,ix);pS.Position=new Vector2(lf,bA+2f);g.f.Add(pS);
string[]iK=q.fr ?? l.fs;Color sr=iK!=null&&iK.Length>0 ? F(iK[0]): F(q.fq ?? l.dQ ?? "40, 60, 85");
t(g,new Vector2(ay+aP/2f,bA+eC-2f),new Vector2(aP-8f,1f),sr);}if(q.h1=="vertical"||q.an.Any(tP=>tP.h==e.fL)){
qu(g,k6,q,go,aL,ay,bA,aP,cL,eC);continue;}int vd=Math.Max(1,q.an.Count);float tF=cL-eC-8f;
float qd=0f,mp=0f;foreach(var tQ in q.an)qd+=1<<tQ.fJ;float uQ=tF/Math.Max(1f,qd);float gy=6f;for(int jI=0;jI<q.an.Count;
jI++){var p=q.an[jI];var V=new ct{fD=p};float g3=uQ*(1<<p.fJ);float ub=bA+eC+4f+mp;mp+=g3;float M=ub+(g3/2f);
float pP=M-(11f*aL);V.c2=g.f.Count;V.lP=p._!=null&&(p._.Contains("{val}")||p._.Contains("{percent}"));
var oo=MySprite.CreateText(p._,go,p.d0!=null ? F(p.d0): Color.White,aL,TextAlignment.LEFT);oo.Position=new Vector2(ay+gy,pP);
g.f.Add(oo);float tf=Math.Max(24f*aL,(p._.Length+1)*12f*aL);float uZ=(!string.IsNullOrEmpty(p.ea)|| !string.IsNullOrEmpty(p.dW))? 64f*aL : 52f*aL;
if(p.h!=e.ia){float L=ay+gy+tf;float rY=ay+aP-gy-uZ;float J=Math.Max(16f,rY-L);
float Q=Math.Max(p.h==e.cw ? 8f : 4f,Math.Min((p.h==e.cw ? 20f : 16f)*aL,g3*(p.h==e.cw ? 0.55f : 0.45f)));
if(p.fJ>0)Q=g3*0.7f;Color pW=ax(p,new Color(25,30,38));V.cl=L;V.bp=J;V.a7=Q;V.ck=M;
if(p.h==e.dM){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),pW);V.Z=g.f.Count;
t(g,new Vector2(L,M),new Vector2(0f,Q),F(p.av));}else if(p.h==e.d2){int kS=10;float nO=2f;
float kT=Math.Max(2f,(J-(nO*(kS-1)))/kS);for(int kM=0;kM<kS;kM++){V.K.Add(g.f.Count);t(g,new Vector2(L+kM*(kT+nO)+(kT/2f),M),new Vector2(kT,Q),ax(p,new Color(22,26,32)));
}}else if(p.h==e.cw){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),ax(p,new Color(20,24,30)));int kB=8;
float nP=2f;float i3=Math.Max(2f,(J-(nP*(kB-1)))/kB);for(int kn=0;kn<kB;kn++){V.K.Add(g.f.Count);t(g,new Vector2(L+kn*(i3+nP)+(i3/2f),M),new Vector2(i3,2f),F(p.av));
}}else if(p.h==e.dZ){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),ax(p,new Color(20,24,30)));int k2=10;
float nQ=2f;float k3=Math.Max(2f,(J-(nQ*(k2-1)))/k2);for(int kN=0;kN<k2;kN++){V.K.Add(g.f.Count);
t(g,new Vector2(L+kN*(k3+nQ)+(k3/2f),M),new Vector2(k3,Q),ax(p,new Color(24,28,36)),0.523599f);}}else if(p.h==e.dR){
int iC=10;float nR=2f;float iD=Math.Max(2f,(J-(nR*(iC-1)))/iC);for(int kO=0;kO<iC;kO++){V.K.Add(g.f.Count);
t(g,new Vector2(L+kO*(iD+nR)+(iD/2f),M),new Vector2(iD,Q),ax(p,new Color(24,28,36)),1.570796f,"Triangle");}}else if(p.h==e.dU){
int i9=12;float nS=2f;float jb=Math.Max(2f,(J-(nS*(i9-1)))/i9);float nn=Math.Min(jb,Q);for(int kP=0;kP<i9;kP++){V.K.Add(g.f.Count);
t(g,new Vector2(L+kP*(jb+nS)+(jb/2f),M),new Vector2(nn,nn),ax(p,new Color(24,28,36)),0f,"Circle");}}else if(p.h==e.cY){
t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),pW);V.Z=g.f.Count;t(g,new Vector2(L+(J/2f),M),new Vector2(0f,Q),F(p.av));
t(g,new Vector2(L+(J/2f),M),new Vector2(2f,Q+4f),new Color(80,110,150));}else if(p.h==e.cz){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),ax(p,new Color(20,24,30)));
V.Z=g.f.Count;float jY=Math.Max(4f,Q);t(g,new Vector2(L+jY*0.5f,M),new Vector2(jY,jY),new Color(40,50,65),0f,"Circle");
}else if(p.h==e.cp){float dc=Math.Max(6f,Math.Min(Q,14f*aL));t(g,new Vector2(L+dc*0.5f,M),new Vector2(dc,dc),new Color(28,34,46));
V.Z=g.f.Count;t(g,new Vector2(L+dc*0.5f,M),new Vector2(dc*0.6f,dc*0.6f),new Color(40,50,65));}else if(p.h==e.cx){
t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),ax(p,new Color(18,22,30)));V.Z=g.f.Count;float ky=Math.Max(4f,Q*0.7f);
t(g,new Vector2(L+ky+2f,M),new Vector2(ky,ky),new Color(34,197,94),0f,"Circle");}else if(p.h==e.cW){int kx=8;
float nT=2f;float kz=Math.Max(2f,(J-(nT*(kx-1)))/kx);for(int ko=0;ko<kx;ko++){V.K.Add(g.f.Count);t(g,new Vector2(L+ko*(kz+nT)+(kz/2f),M),new Vector2(kz,Q),ax(p,new Color(24,28,36)));
}}else if(p.h==e.cr){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),ax(p,new Color(20,24,30)));V.Z=g.f.Count;
float n6=Math.Max(4f,Q);t(g,new Vector2(L+(J/2f),M),new Vector2(n6,n6),F(p.av),0f,"Triangle");}}var p1=MySprite.CreateText("---",go,Color.White,aL,TextAlignment.RIGHT);
p1.Position=new Vector2(ay+aP-gy,pP);V.a1=g.f.Count;g.f.Add(p1);g.d6.Add(V);}}g.d1=W;
g.d_=true;}const string rx="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bR=8;void qu(aw ab,IMyTextSurface uD,aC kq,string nJ,float ez,float tC,float kp,float tD,float oN,float sT){
int ts=Math.Max(1,kq.an.Count);float m8=(tD-12f)/ts;float ht=22f*ez;float bm=kp+sT+4f+ht;float aU=Math.Max(20f,kp+oN-4f-ht-bm);
float sk=uD.MeasureStringInPixels(new StringBuilder("M"),nJ,ez*0.9f).X;for(int gx=0;gx<kq.an.Count;gx++){var cJ=kq.an[gx];float bh=tC+6f+m8*(gx+0.5f);
float cU=m8*0.7f;var bj=new ct{fD=cJ,me=true,cl=bh,bp=cU,a7=aU,ck=bm+aU/2f,lD=sk};bj.c2=ab.f.Count;
a6(ab.f,cJ._,cJ.d0!=null ? F(cJ.d0): Color.White,bh,kp+oN-4f-ht,ez);bj.a1=ab.f.Count;
a6(ab.f,"---",Color.White,bh,bm-ht,ez);var ok=ab.f[bj.c2];ok.Alignment=TextAlignment.CENTER;ab.f[bj.c2]=ok;
var p0=ab.f[bj.a1];p0.Alignment=TextAlignment.CENTER;ab.f[bj.a1]=p0;t(ab,new Vector2(bh,bm+aU/2f),new Vector2(cU,aU),ax(cJ,new Color(25,30,38)));
bj.Z=ab.f.Count;if(cJ.h==e.fL){for(int mD=0;mD<bR;mD++){bj.K.Add(ab.f.Count);t(ab,new Vector2(bh,bm+aU),new Vector2(cU-6f,0f),Color.White);
}for(int lo=0;lo<4;lo++){bj.K.Add(ab.f.Count);var pI=MySprite.CreateText("",nJ,lo==0 ? new Color(255,255,255,170): new Color(255,255,255,110),ez*0.9f,TextAlignment.CENTER);
pI.Position=new Vector2(bh,bm+aU);ab.f.Add(pI);}Color jd=new Color(90,110,135);t(ab,new Vector2(bh-cU/2f,bm+aU/2f),new Vector2(3f,aU),jd);t(ab,new Vector2(bh+cU/2f,bm+aU/2f),new Vector2(3f,aU),jd);
t(ab,new Vector2(bh,bm+aU),new Vector2(cU+3f,3f),jd);}else{t(ab,new Vector2(bh,bm+aU),new Vector2(cU,0f),F(cJ.av));}ab.d6.Add(bj);
}}bool rw(aw bf,ct y,double fe,double o3,Color ga,bool p5){var a4=y.fD;float mK=y.E;if(y.E<0f)y.E=(float)fe;
y.E=Math.Abs(y.E-(float)fe)>0.05f ? y.E+((float)fe-y.E)*0.35f :(float)fe;float cK=Math.Max(0f,Math.Min(1f,y.E/100f))*y.a7;
float f7=y.ck+y.a7/2f;float ge=y.cl;if(a4.h!=e.fL){var gj=bf.f[y.Z];gj.Size=new Vector2(y.bp,cK);
gj.Position=new Vector2(ge,f7-cK/2f);gj.Color=ga;bf.f[y.Z]=gj;if(p5||y.E!=mK)aJ(bf,y,bb(o3,a4),ga);
return p5||y.E!=mK;}float iM=y.a7/bR;string e_=a4.bK!=null ? H.d4(a4.bK,a4.dT!=null ? bu(a4.dT,a4.hJ): fe): null;
Color tn=e_!=null ? F(e_): ga;string[]pA=e_!=null ?(e_.Contains("/")? e_.Split('/'): null): a4.cs&&a4.bc.Length>1 ? a4.bc : null;
for(int cE=0;cE<bR;cE++){float qa=Math.Max(0f,Math.Min(iM,cK-cE*iM));var gX=bf.f[y.K[cE]];gX.Size=new Vector2(y.bp-6f,qa);
gX.Position=new Vector2(ge,f7-cE*iM-qa/2f);gX.Color=pA!=null ? fx(pA,100.0*(cE+0.5)/bR): tn;bf.f[y.K[cE]]=gX;}
int sa=Math.Max(1,(int)((y.bp-8f)/y.lD));bool un=cK>6f;var lr=bf.f[y.K[bR]];lr.Data=un ? rx.Substring((int)(aa*5.0)%6,Math.Min(sa,100)): "";
lr.Position=new Vector2(ge,f7-cK-12f);bf.f[y.K[bR]]=lr;for(int b7=0;b7<3;b7++){var iV=bf.f[y.K[bR+1+b7]];
double tL=(aa*(0.35+0.1*b7)+b7*0.37)%1.0;iV.Data=cK>24f ?(b7==1 ? "O" : "o"): "";iV.Position=new Vector2(ge+(b7-1)*y.bp*0.26f,f7-(float)tL*(cK-6f)-10f);
bf.f[y.K[bR+1+b7]]=iV;}aJ(bf,y,bb(o3,a4),ga);return true;}static void aJ(aw mT,ct pt,string uL,Color sd){
var ll=mT.f[pt.a1];ll.Data=uL;ll.Color=sd;mT.f[pt.a1]=ll;}static void l5(aw mU,ct pu,Color se){
var nw=mU.f[pu.Z];nw.Color=se;mU.f[pu.Z]=nw;}bool rk(z bF,IMyTextSurface k7){aw j;
if(!bV.TryGetValue(bF,out j)){j=new aw();bV[bF]=j;}bool o6= !j.d_||j.d1!=k7.SurfaceSize;
if(o6){qt(bF,k7,j);}bool N=o6;bool fX=(aa%bF.hz)<bF.hz/2.0;double cH= -1.0,nc;if(fO.TryGetValue(bF,out nc)){
cH=(aa-nc)/bF.hH;if(cH>=1.0){fO.Remove(bF);cH=1.0;}}bool lp=cB&&fX&&j.fi.A>0;if(j.cm>=0&&lp!=j.fj){
j.fj=lp;for(int eE=0;eE<j.fm.Length;eE++){var mL=j.f[j.cm+eE];mL.Color=lp ? j.fi : j.fm[eE];j.f[j.cm+eE]=mL;
}N=true;foreach(var r_ in j.dO)r_.hS=true;}foreach(var ak in j.dO){if(ak.lT&&j.fj)continue;Color eo=ak.hB;try{if(ak.fI!=null){
double mV=bu(ak.hF,ak.hN);string g1=H.d4(ak.fI,mV);eo=g1!=null&&g1.Contains("/")? fx(g1.Split('/'),mV): F(g1);
}if(ak.fw!=null&&fX&&H.d4(ak.fw,bu(ak.hO,ak.hQ))!=null)eo=j.fi;}catch{}if(eo!=ak.ft||ak.hS){
ak.ft=eo;ak.hS=false;var mS=j.f[ak.fA];mS.Color=eo;j.f[ak.fA]=mS;N=true;}}for(int jJ=0;jJ<j.d6.Count;jJ++){var d=j.d6[jJ];
var i=d.fD;double D;bool n0=false;double kb=i.fF;try{D=bu(i.aX,i.I);if(i.c3!=null)kb=bu(i.c3,i.hW);
}catch{n0=true;D=0.0;}if(n0){if(d.fC!= -99999.0||(d.a1>=0&&j.f[d.a1].Data!="ERR")){N=true;d.fC= -99999.0;
if(d.a1>=0){var kQ=j.f[d.a1];kQ.Data="ERR";kQ.Color=Color.Red;j.f[d.a1]=kQ;}}continue;}double a5=D;
if(kb>0)D=Math.Min(100.0,a5/kb*100.0);bool pF=cH>=0.0&&i.h!=e.ia&&i.h!=e.cz&&i.h!=e.cp&&i.h!=e.cx&&i.h!=e.cr;
if(pF)D=cH<0.5 ? cH*200.0 : 100.0+(D-100.0)*(cH-0.5)*2.0;double gd=i.dT!=null ? bu(i.dT,i.hJ): D;string gb=i.bK!=null ? H.d4(i.bK,gd): i.cs ? H.fy(i.bc,gd): i.av;
Color U=F(gb);bool bx=(U==Color.Red);if(gb!=null&&gb.Contains("/"))U=fx(gb.Split('/'),gd);if(i.bK==null&&i.cs&& !bF.fz&&i.bc.Length>1)U=fx(i.bc,gd);
if(bx&&fX){U=new Color(255,75,75);N=true;}bool aV=pF||Math.Abs(a5-d.fC)>0.1;if(aV)d.fC=a5;
if(d.me){if(rw(j,d,D,a5,U,aV))N=true;continue;}if(i.h==e.dM){double nD=(i.hU||i.I=="percent"||i.I=="pct")? D : h7(i.aX);
if(d.E<0f)d.E=(float)nD;float hl=(float)nD;if(Math.Abs(d.E-hl)>0.05f){d.E+=(hl-d.E)*0.35f;
N=true;}else if(d.E!=hl){d.E=hl;N=true;}if(aV)N=true;if(N){float tI=(float)Math.Max(0.0,Math.Min(1.0,d.E/100.0));
float nF=d.bp*tI;var gl=j.f[d.Z];gl.Size=new Vector2(nF,d.a7);gl.Position=new Vector2(d.cl+(nF/2f),d.ck);
gl.Color=U;j.f[d.Z]=gl;aJ(j,d,bb(a5,i),U);}}else if(i.h==e.d2||i.h==e.dZ||i.h==e.dR||i.h==e.dU){
if(aV||bx){N=true;double sA=(i.hU||i.I=="percent"||i.I=="pct")? D : h7(i.aX);int pm=d.K.Count;
for(int e6=0;e6<pm;e6++){bool to=sA>=((e6+0.4)/(double)pm*100.0);Color ul=to ? U : ax(i,new Color(24,28,36));var pn=j.f[d.K[e6]];
pn.Color=ul;j.f[d.K[e6]]=pn;}aJ(j,d,bb(a5,i),U);}}else if(i.h==e.cw){
if(aV||bx){N=true;for(int gs=0;gs<7;gs++)d.fK[gs]=d.fK[gs+1];d.fK[7]=(float)D;int tS=d.K.Count;
for(int eR=0;eR<tS;eR++){float sS=d.fK[eR];float sR=Math.Max(0.06f,Math.Min(1f,sS/100f));float m6=d.a7*sR;float sf=d.ck+(d.a7/2f)-(m6/2f);
var dh=j.f[d.K[eR]];dh.Size=new Vector2(dh.Size.Value.X,m6);dh.Position=new Vector2(dh.Position.Value.X,sf);
dh.Color=U;j.f[d.K[eR]]=dh;}aJ(j,d,bb(a5,i),U);}}else if(i.h==e.cY){
if(d.E<0f)d.E=(float)D;float hm=(float)D;if(Math.Abs(d.E-hm)>0.05f){d.E+=(hm-d.E)*0.35f;
N=true;}else if(d.E!=hm){d.E=hm;N=true;}if(N||aV){float n_=d.bp*0.5f;float m1=d.cl+n_;
float ni=(float)Math.Max(-1.0,Math.Min(1.0,(d.E-50.0)/50.0));float jp=Math.Abs(ni)*n_;float sB=(ni>=0)?(m1+jp*0.5f):(m1-jp*0.5f);
var gm=j.f[d.Z];gm.Size=new Vector2(jp,d.a7);gm.Position=new Vector2(sB,d.ck);gm.Color=U;
j.f[d.Z]=gm;aJ(j,d,bb(a5,i),U);}}else if(i.h==e.cz){if(aV||bx){
N=true;bool jR=D>=50.0;float of=Math.Max(4f,d.a7);float s4=jR ?(d.cl+d.bp-of*0.5f):(d.cl+of*0.5f);
Color oe=jR ?(U!=Color.White ? U : new Color(34,197,94)): new Color(60,70,85);var jn=j.f[d.Z];jn.Position=new Vector2(s4,d.ck);
jn.Color=oe;j.f[d.Z]=jn;aJ(j,d,jR ? "ON" : "OFF",oe);}}else if(i.h==e.cp){if(aV||bx){
N=true;bool jQ=D>=50.0;Color m2=jQ ?(U!=Color.White ? U : new Color(56,189,248)): new Color(30,36,48);l5(j,d,m2);
aJ(j,d,jQ ? "ACTIVE" : "READY",jQ ? m2 : new Color(148,163,184));}}else if(i.h==e.cx){if(aV||bx){
N=true;string uy=D>=75.0 ? "ONLINE" :(D>=25.0 ? "STBY" : "OFFLINE");Color pz=D>=75.0 ? new Color(34,197,94):(D>=25.0 ? new Color(234,179,8): new Color(239,68,68));
l5(j,d,pz);aJ(j,d,uy,pz);}}else if(i.h==e.cW){if(aV||bx){N=true;int oW=d.K.Count;
for(int eS=0;eS<oW;eS++){bool tp=D>=((eS+0.5)/(double)oW*100.0);Color tN=tp ? U : ax(i,new Color(24,28,36));var oX=j.f[d.K[eS]];
oX.Color=tN;j.f[d.K[eS]]=oX;}aJ(j,d,bb(a5,i),U);}}else if(i.h==e.cr){
N=true;float g2;Color ey;string gn;bool sX=(i.I=="producing"||i.I=="active"||i.I=="running"||i.I=="state"||(i.I!="percent"&&(D<=1.0||D==100.0)));
if(sX){if(D>0.0){d.E=(d.E+0.3927f)%6.28318f;g2=d.E;ey=new Color(34,197,94);gn="RUN";}else{g2=0f;
ey=new Color(148,163,184);gn="IDLE";}}else{g2=(D>55.0)? 0f :((D<45.0)? 3.14159f : 1.57079f);ey=(D>55.0)? new Color(34,197,94):((D<45.0)? new Color(239,68,68): new Color(148,163,184));
gn=(D>55.0)? "CHG" :((D<45.0)? "DSC" : "STBY");}var jo=j.f[d.Z];jo.RotationOrScale=g2;jo.Color=ey;
j.f[d.Z]=jo;aJ(j,d,gn,ey);}else{string gi;bq ho;ed.TryGetValue(i.aX,out ho);c6.TryGetValue(lU(i.I)? i.aX+"."+i.I :(ho!=null&&ho.br!=null ? ho.br : i.aX),out gi);
if(aV||bx||gi!=d.lV){d.lV=gi;N=true;Color li=U;if(li==Color.Red&&fX)li=new Color(255,75,75);aJ(j,d,gi ?? bb(a5,i),li);
}}if(d.lP&&(aV||bx)){N=true;var oq=j.f[d.c2];string tJ=lN(D);string t8=bb(a5,i);
oq.Data=i._.Replace("{percent}",tJ).Replace("{val}",t8);j.f[d.c2]=oq;}}if(N){j.eb++;using(var sH=k7.DrawFrame()){
sH.AddRange(j.f);}}return N;}bool rj(z cd,IMyTextSurface k8){k8.FontSize=cd.c0;var gc=new List<KeyValuePair<string,string>>();
foreach(var m in cd.fE){double O=0.0;string bl;string bg=null;double kc=m.fF;try{O=bu(m.aX,m.I);if(m.c3!=null)kc=bu(m.c3,m.hW);
}catch(Exception vc){gc.Add(new KeyValuePair<string,string>($"{m._}: [UNRESOLVED]","red"));continue;}double o4=O;if(kc>0)O=Math.Min(100.0,o4/kc*100.0);
string p2=bb(o4,m);if(m.h==e.dM){double nE=(m.hU||m.I=="percent"||m.I=="pct")? O : h7(m.aX);
bl=H.rc(m._,nE,10,p2);bg=m.cs ? H.fy(m.bc,nE): m.av;}else if(m.h==e.cY){
bl=H.ra(m._,O,10);bg=m.cs ? H.fy(m.bc,O): m.av;}else if(m.h==e.cz){
bl=H.rm(m._,O);bg=m.av ??(O>=50.0 ? "green" : "gray");}else if(m.h==e.cp){bl=H.rf(m._,O);
bg=m.av ??(O>=50.0 ? "cyan" : "gray");}else if(m.h==e.cx){bl=H.rl(m._,O);bg=m.av ??(O>=75.0 ? "lime" :(O>=25.0 ? "yellow" : "red"));
}else if(m.h==e.cW){bl=H.q9(m._,O,8);bg=m.cs ? H.fy(m.bc,O):(m.av ?? "yellow");
}else if(m.h==e.cr){bool sY=(m.I=="producing"||m.I=="active"||m.I=="running"||m.I=="state"||(m.I!="percent"&&(O<=1.0||O==100.0)));
if(sY){char[]uw=new[]{'|','/','-','\\'};char ui=uw[Math.Abs(ej)%4];bl=O>0 ? $"{m._} [{ui}] RUN" : $"{m._} [==] IDLE";
bg=O>0 ? "green" : "gray";}else{bl=H.rg(m._,O,ej);bg=m.av ??(O>55.0 ? "green" :(O<45.0 ? "red" : "cyan"));
}}else{bl=$"{m._}: {p2}";bg=m.bK!=null ? H.d4(m.bK,O): null;}gc.Add(new KeyValuePair<string,string>(bl,bg));
}var f6=H.re(new List<string>(gc.ConvertAll(uG=>uG.Key)),dJ: cd.mf);string pq=string.Join("\n",f6);
string oZ;if(cC.TryGetValue(cd,out oZ)&&oZ==pq)return false;cC[cd]=pq;using(var sI=k8.DrawFrame()){
float qj=4f;float ot=qk*cd.c0;try{var ow=k8.MeasureStringInPixels(new StringBuilder("A"),"Monospace",cd.c0);
if(ow.Y>0)ot=ow.Y;}catch{}for(int dm=0;dm<f6.Count;dm++){string na=null;if(dm>0&&dm<f6.Count-1)na=gc[dm-1].Value;
var py=MySprite.CreateText(f6[dm],"Monospace",F(na),cd.c0,TextAlignment.LEFT);py.Position=new Vector2(4f,qj);sI.Add(py);
qj+=ot;}}return true;}Color F(string gL){if(string.IsNullOrEmpty(gL))return Color.White;Color eL;if(mj.TryGetValue(gL,out eL))return eL;
o er=o.d3(gL);eL=er.fB ? Color.Transparent : new Color(er.bP,er.bL,er.bG,er.hx);mj[gL]=eL;return eL;}public enum a8{qo,hT,dV}
public enum aI{qV,hA,ma,lX,lW}public class bq{public string af;public a8 bM;public List<string>a0=new List<string>();public string mb{
get{return a0.Count>0 ? a0[0]: "";}set{if(a0.Count==0)a0.Add(value);else a0[0]=value;}}public aI dK=aI.qV;
public double fu=0.0;public string hK;public string br;public bool fl;}public enum e{ia,dM,d2,cw,dZ,dR,dU,cY,cz,cp,cx,cW,cr,fL}
public class aY{public string hM;public string cq;public bool c1;}public class bs{public e h;public string _;public string aX;
public string I;public bool cs;public string[]bc;public string av;public List<aY>bK;public string lE="right";
public string ea;public string dW;public int fJ;public string dT,hJ;public string d0,id;public double fF;public string c3,hW;
public bool hU{get{return fF>0||c3!=null;}}}public struct o{public byte bP,bL,bG,hx;public bool fB;public o(byte t_,byte sK,byte rR,byte rz=255){
bP=t_;bL=sK;bG=rR;hx=rz;fB=false;}public static readonly o rv=new o(0,0,0,0){fB=true};public static readonly o fM=new o(255,255,255);
public static readonly o qn=new o(0,0,0);public static readonly o q7=new o(255,0,0);public static readonly o qO=new o(0,255,0);
public static readonly o qS=new o(50,205,50);public static readonly o ry=new o(255,255,0);public static readonly o qW=new o(255,140,0);
public static readonly o qz=new o(0,255,255);public static readonly o qp=new o(0,120,255);public static readonly o qT=new o(255,0,255);
public static readonly o q5=new o(128,0,128);public static readonly o qN=new o(128,128,128);public static readonly o qA=new o(60,60,60);
public static o d3(string bk){if(string.IsNullOrWhiteSpace(bk))return fM;bk=bk.Trim();if(bk.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bk.Equals("none",StringComparison.OrdinalIgnoreCase))return rv;
if(bk.StartsWith("#")){string b5=bk.Substring(1).Trim();if(b5.Length==6){byte t0=Convert.ToByte(b5.Substring(0,2),16);byte sL=Convert.ToByte(b5.Substring(2,2),16);
byte rS=Convert.ToByte(b5.Substring(4,2),16);return new o(t0,sL,rS);}if(b5.Length==3){byte t1=Convert.ToByte(new string(b5[0],2),16);byte sM=Convert.ToByte(new string(b5[1],2),16);
byte rT=Convert.ToByte(new string(b5[2],2),16);return new o(t1,sM,rT);}}if(bk.Contains(",")){var dv=bk.Split(',');if(dv.Length>=3){byte t2=(byte)Math.Max(0,Math.Min(255,int.Parse(dv[0].Trim())));
byte sN=(byte)Math.Max(0,Math.Min(255,int.Parse(dv[1].Trim())));byte rU=(byte)Math.Max(0,Math.Min(255,int.Parse(dv[2].Trim())));byte rA=dv.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(dv[3].Trim()))):(byte)255;
return new o(t2,sN,rU,rA);}}switch(bk.ToLowerInvariant()){case "red": return q7;case "green": return qO;case "lime": return qS;case "yellow": return ry;
case "orange": return qW;case "cyan": return qz;case "blue": return qp;case "magenta": return qT;case "purple": return q5;case "white": return fM;
case "black": return qn;case "gray": case "grey": return qN;case "darkgray": case "darkgrey": return qA;default: return fM;}}}public class bI{
public string[]bJ;public string dN="vertical";public List<aY>aW;public string hC,hD;public List<aY>c_;public string hP,hR;
}public class aC : bI{public string c4="default";public int dS=0;public int d5=0;public int d7=1;public int d8=1;public string cy;public string ib="left";
public string ic;public string dP;public string lJ;public float? lL;public string cX;public string fq;public string[]fr;
public string h1="horizontal";public List<bs>an=new List<bs>();}public class z : bI{public string bd;public string cv="standard";
public string cZ="graphic";public string dQ;public string[]fs;public string mc="left";public string md;public string lK="Monospace";
public string bH;public string lA;public int lB=10;public int ly=1;public float hH=1.2f;public bool fz;
public string h2;public string hE;public string lx;public string lv;public string hY;public string hX;public string cj;
public string co;public float hI=2.5f;public string lz="cyan";public string[]fo;public List<string>fp=new List<string>();
public List<string>qL(){if(fp!=null&&fp.Count>0)return fp;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float c0=1.0f;public int h5=0;public float hz=1.6f;public int dX=1;public int dY=1;public int mf=30;public string bN;
public int cu=0;public List<aC>aj=new List<aC>();public List<bs>fE{get{if(aj.Count==0)aj.Add(new aC{c4="main"}
);return aj[0].an;}}}public class bS{public string hZ;public List<string>fG=new List<string>();public Dictionary<string,string>hL=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>hG=new List<string>();}public class h4{public List<bq>cn=new List<bq>();public List<z>T=new List<z>();
public List<string>ba=new List<string>();public List<string>ec=new List<string>();public Dictionary<string,List<string>>dL=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class lR{static readonly Dictionary<string,bS>lC=new Dictionary<string,bS>(StringComparer.OrdinalIgnoreCase);
static lR(){q8();}static void q8(){bQ("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bQ("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bQ("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bQ("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bQ("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bQ("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bQ("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bQ("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bQ(string oD,string[]tG,string r3){var hj=new bS{hZ=oD}
;foreach(var tz in tG){var j_=tz.Split(new[]{'='},2);string oO=h_(j_[0]);hj.fG.Add(oO);if(j_.Length>1)hj.hL[oO]=ac(j_[1].Trim());
}var tm=r3.Replace("\r\n","\n").Split('\n');foreach(var ta in tm){string pc=ta.Trim();if(pc.Length>0)hj.hG.Add(pc);}lC[oD]=hj;}static string h_(string dt){
dt=dt.Trim();if(dt.StartsWith("$"))dt=dt.Substring(1);return dt.ToLowerInvariant();}public static h4 qY(string si){var ar=new h4();z c=null;
aC u=null;var oY=q3(si,ar.ba,ar.dL);for(int eF=0;eF<oY.Count;eF++){string a=oY[eF].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){ar.cn.Add(qZ(a));}else if(a.StartsWith("@screen")){c=new z{bd=a.Substring("@screen".Length).Trim()}
;ar.T.Add(c);u=null;}else if(a.StartsWith("profile:")){r(c,"profile:");c.cv=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){r(c,"border:");string mE=a.Substring("border:".Length).Trim();if(u!=null)u.dP=mE;
else c.cZ=mE;}else if(a.StartsWith("border_color:")){r(c,"border_color:");string mG=a.Substring("border_color:".Length).Trim();
if(u!=null)u.fq=mG;else c.dQ=mG;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
r(c,"border_gradient");string cF=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
cF=cF.Trim();if(cF.StartsWith("(")&&cF.Contains(")"))cF=aZ(cF,0);string[]pB=a_(cF).Select(uc=>uc.Trim()).ToArray();if(u!=null)u.fr=pB;
else c.fs=pB;}else if(a.StartsWith("title_align:")){r(c,"title_align:");string pJ=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(u!=null)u.ib=pJ;else c.mc=pJ;}else if(a.StartsWith("title_color:")){r(c,"title_color:");string pM=a.Substring("title_color:".Length).Trim();
if(u!=null)u.ic=pM;else c.md=pM;}else if(a.StartsWith("font_size:")){r(c,"font_size:");float pG=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(u!=null)u.lL=pG;else c.c0=pG;}else if(a.StartsWith("font:")){r(c,"font:");string nx=a.Substring("font:".Length).Trim();
if(u!=null)u.lJ=nx;else c.lK=nx;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){r(c,"bg:");string da=a.Substring(a.IndexOf(':')+1).Trim();
bI iL=u!=null ?(bI)u : c;if(da.StartsWith("color(")&&da.EndsWith(")")){string mO;iL.aW=l0(da.Substring(6,da.Length-7),out mO);
string mM,mN;l7(mO,out mM,out mN);iL.hC=mM;iL.hD=mN;}else if(u!=null)u.cX=da;
else c.bH=da;}else if(a.StartsWith("layout:")){r(c,"layout:");q_(a.Substring("layout:".Length).Trim(),c);}else if(a.StartsWith("alert_period:")){
r(c,"alert_period:");c.hz=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){r(c,"refresh:");c.h5=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
r(c,"output:");var gR=a_(a.Substring("output:".Length).Trim());if(gR.Count==0)throw new FormatException("output: requires a target block name");
c.bN=ac(gR[0]);c.cu=gR.Count>1 ? int.Parse(gR[1]): 0;}else if(a.StartsWith("width:")){r(c,"width:");
c.mf=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){r(c,"boot_msg:");
c.lA=ac(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){r(c,"boot_stagger:");c.lB=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){r(c,"boot_calibrate:");string nd=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
c.hH=nd=="off" ? 0f : Math.Max(0f,float.Parse(nd,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){r(c,"boot_batch:");
c.ly=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){r(c,"boot_duration:");
string et=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool n5=et.EndsWith("s");if(n5)et=et.Substring(0,et.Length-1).Trim();
float jc;if(float.TryParse(et,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out jc)){c.hI=n5 ? jc : jc/60f;
}}else if(a.StartsWith("boot_log:")){r(c,"boot_log:");c.fp.Add(ac(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("mirror_panel:")){
r(c,"mirror_panel:");c.hY=ac(a.Substring("mirror_panel:".Length).Trim());}else if(a.StartsWith("mirror:")){r(c,"mirror:");
c.hX=ac(a.Substring("mirror:".Length).Trim());}else if(a.StartsWith("banner:")){r(c,"banner:");c.cj=ac(a.Substring("banner:".Length).Trim());
}else if(a.StartsWith("boot_logo:")){r(c,"boot_logo:");c.co=ac(a.Substring("boot_logo:".Length).Trim());}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){
r(c,"boot_gradient");c.fo=l1(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){r(c,"bg_gradient");
(u!=null ?(bI)u : c).bJ=l1(a);}else if(a.StartsWith("bg_gradient_dir:")){r(c,"bg_gradient_dir:");
(u!=null ?(bI)u : c).dN=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(a.StartsWith("flash:")){
r(c,"flash:");bI jk=u!=null ?(bI)u : c;string jj=a.Substring("flash:".Length).Trim();string nz=l9(jj);
string ny,nA;l7(jj.Substring(0,jj.Length-nz.Length),out ny,out nA);jk.hP=ny;jk.hR=nA;jk.c_=new List<aY>{
new aY{hM=nz,cq="on"}};}else if(a.StartsWith("bg_texture:")){r(c,"bg_texture:");c.hE=ac(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){r(c,"bg_texture_color:");c.lx=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gradient_mode:")){r(c,"gradient_mode:");c.fz=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){r(c,"screen_bg:");c.bH=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
r(c,"panel_bg:");c.h2=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){r(c,"alert_bg:");
c.lv=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){r(c,"boot_color:");
c.lz=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){r(c,"orientation:");string tw=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(u!=null)u.h1=tw;}else if(a.StartsWith("@panel")){r(c,"@panel");u=q1(a);c.aj.Add(u);
}else if(a.StartsWith("title:")){r(c,"title:");if(u==null){u=new aC{c4="main",dS=0,d5=0,d7=c.dX,d8=c.dY}
;c.aj.Add(u);}u.cy=ac(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){r(c,"line:");
if(u==null){u=new aC{c4="main",dS=0,d5=0,d7=c.dX,d8=c.dY};c.aj.Add(u);}u.an.Add(q0(a.Substring("line:".Length).Trim()));
}else{ar.ba.Add($"Line {eF + 1}: unrecognized directive: \"{a}\"");}}catch(Exception su){ar.ba.Add($"Line {eF + 1}: parse error ({su.Message}) in \"{a}\"");
}}foreach(var pd in ar.T){if(string.IsNullOrEmpty(pd.bN))ar.ba.Add($"@screen {pd.bd}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var au in ar.T){int kl=au.aj.Count(kA=>kA.cX!=null||kA.aW!=null||kA.bJ!=null);if(au.aj.Count>1&&kl>0&&kl<au.aj.Count)ar.ec.Add("screen '"+au.bd+"': only "+kl+" of "+au.aj.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(au.cj!=null&& !ar.dL.ContainsKey(au.cj))ar.ba.Add("Screen '"+au.bd+"': banner '"+au.cj+"' is not defined (@banner "+au.cj+" ... @end)");
if(au.co!=null&& !ar.dL.ContainsKey(au.co))ar.ba.Add("Screen '"+au.bd+"': boot_logo '"+au.co+"' is not defined (@banner "+au.co+" ... @end)");
}return ar;}static void q_(string iS,z pk){int ks=iS.IndexOf('(');if(ks<0)throw new FormatException("missing '(' in layout: specification");
string od=iS.Substring(0,ks).Trim().ToLowerInvariant();string rH=aZ(iS,ks);var iB=a_(rH);if(od=="grid"){if(iB.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
pk.dX=Math.Max(1,int.Parse(iB[0]));pk.dY=Math.Max(1,int.Parse(iB[1]));}else{throw new FormatException($"unknown layout kind '{od}' (expected grid)");
}}static aC q1(string ti){string e1=ti.Substring("@panel".Length).Trim();int f_=e1.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(f_<0)throw new FormatException("@panel requires an at(...) positioning clause");
string tt=e1.Substring(0,f_).Trim();string mz=e1.Substring(f_);int oU=mz.IndexOf('(');string mx=aZ(mz,oU);
var cD=a_(mx);if(cD.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bB=new aC{c4=tt,dS=int.Parse(cD[0]),d5=int.Parse(cD[1]),d7=cD.Count>2 ? int.Parse(cD[2]): 1,d8=cD.Count>3 ? int.Parse(cD[3]): 1}
;int mq=f_+oU+mx.Length+1;if(mq<e1.Length){string aR=e1.Substring(mq).Trim();int pR=aR.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(pR>=0){string e9=aR.Substring(pR+5).Trim();if(e9.StartsWith("(")&&e9.Contains(")"))bB.cy=ac(aZ(e9,0));else if(e9.StartsWith(":")){
string cG=e9.Substring(1).Trim();if(cG.StartsWith("\"")){int nt=cG.IndexOf('"',1);bB.cy=nt>0 ? cG.Substring(1,nt-1): ac(cG);}else{int pw=cG.IndexOf(' ');
bB.cy=pw>=0 ? cG.Substring(0,pw): cG;}}}string oG=fv(aR,"orientation:");if(oG!=null)bB.h1=oG.ToLowerInvariant();string mo=fv(aR,"title_align:");
if(mo!=null)bB.ib=mo.ToLowerInvariant();string pO=fv(aR,"title_color:");if(pO!=null)bB.ic=pO;int f5=aR.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(f5>=0&& !aR.Substring(f5).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aR.Substring(f5).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bB.dP=fv(aR.Substring(f5),"border:");}int f2=aR.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(f2<0)f2=aR.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(f2>=0){string b3=aR.Substring(f2+"border_gradient".Length).Trim();if(b3.StartsWith(":"))b3=b3.Substring(1).Trim();if(b3.StartsWith("(")&&b3.Contains(")"))b3=aZ(b3,0);
bB.fr=a_(b3).Select(ud=>ud.Trim()).ToArray();}}return bB;}static string fv(string pe,string oc){int n4=pe.IndexOf(oc,StringComparison.OrdinalIgnoreCase);
if(n4<0)return null;string lm=pe.Substring(n4+oc.Length).Trim();int px=lm.IndexOf(' ');return px>=0 ? lm.Substring(0,px): lm;}static string[]l1(string j5){
int jv=j5.IndexOfAny(new[]{':','('});string eA=j5.Substring(j5[jv]==':' ? jv+1 : jv).Trim();if(eA.StartsWith("(")&&eA.Contains(")"))eA=aZ(eA,0);return a_(eA).Select(ue=>ue.Trim()).ToArray();
}static List<string>q3(string sj,List<string>jh,Dictionary<string,List<string>>rW){var pY=new Dictionary<string,bS>(StringComparer.OrdinalIgnoreCase);
var dy=(sj ?? "").Replace("\r\n","\n").Split('\n');var kk=new List<string>();int dn=0;while(dn<dy.Length){string kG=dy[dn++];if(kG.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ap=new List<string>();while(dn<dy.Length){string mm=dy[dn++].Replace("\t","    ").TrimEnd();if(mm.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ap.Add(mm);}while(ap.Count>0&&ap[0].Length==0)ap.RemoveAt(0);while(ap.Count>0&&ap[ap.Count-1].Length==0)ap.RemoveAt(ap.Count-1);int jP=int.MaxValue;
foreach(var it in ap)if(it.Length>0)jP=Math.Min(jP,it.Length-it.TrimStart().Length);for(int eM=0;eM<ap.Count;eM++)if(ap[eM].Length>0)ap[eM]=ap[eM].Substring(jP);
rW[kG.TrimStart().Substring(8).Trim()]=ap;continue;}string b8=l8(kG).Trim();if(b8.Length==0)continue;if(b8.StartsWith("@template")){try{
var lc=q2(b8);while(dn<dy.Length){string r5=dy[dn++];string iT=l8(r5).Trim();if(iT.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(iT.Length>0)lc.hG.Add(iT);}pY[lc.hZ]=lc;}catch(Exception sv){jh.Add($"Template parse error ({sv.Message}) in \"{b8}\"");}}
else if(b8.StartsWith("@use")){try{lG(b8,pY,kk,jh);}catch(Exception sw){jh.Add($"@use error ({sw.Message}) in \"{b8}\"");
}}else{kk.Add(b8);}}return kk;}static bS q2(string tj){string kH=tj.Substring("@template".Length).Trim();int kt=kH.IndexOf('(');
if(kt<0)throw new FormatException("missing '(' in @template header");string tu=kH.Substring(0,kt).Trim();string rI=aZ(kH,kt);var ld=new bS{
hZ=tu};foreach(var tA in a_(rI)){var j0=tA.Split(new[]{'='},2);string oP=h_(j0[0]);ld.fG.Add(oP);if(j0.Length>1)ld.hL[oP]=ac(j0[1].Trim());
}return ld;}static void lG(string tk,Dictionary<string,bS>pZ,List<string>oL,List<string>st,int nh=0){string g0=tk.Substring("@use".Length).Trim();
int gP=g0.IndexOf('(');string kd=gP>=0 ? g0.Substring(0,gP).Trim(): g0.Trim();string rJ=gP>=0 ? aZ(g0,gP): "";bS dG=null;
if(!pZ.TryGetValue(kd,out dG)){if(!lC.TryGetValue(kd,out dG))throw new FormatException($"unknown template '{kd}'");}var f8=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var oh in dG.hL)f8[oh.Key]=oh.Value;var mw=a_(rJ);for(int el=0;el<mw.Count;el++){string fY=mw[el];int jf=fY.IndexOf('=');if(jf>=0){
string s2=h_(fY.Substring(0,jf));string uT=ac(fY.Substring(jf+1).Trim());f8[s2]=uT;}else{if(el<dG.fG.Count){string s3=dG.fG[el];
f8[s3]=ac(fY.Trim());}}}foreach(var r4 in dG.hG){string ew=r4;foreach(var gG in f8){ew=ew.Replace("$"+gG.Key,gG.Value).Replace("{"+gG.Key+"}",gG.Value);
}if(ew.StartsWith("@use")){if(nh>3)throw new FormatException("@use nested too deep");lG(ew,pZ,oL,st,nh+1);
}else oL.Add(ew);}}static void r(z sh,string sq){if(sh==null)throw new FormatException($"'{sq}' used before any @screen declared");
}static string l8(string bi){if(string.IsNullOrEmpty(bi))return bi;bool jO=false;for(int b6=0;b6<bi.Length;b6++){char iX=bi[b6];if(iX=='"'){jO= !jO;
}else if(!jO){if(iX=='/'&&b6+1<bi.Length&&bi[b6+1]=='/'){return bi.Substring(0,b6);}if(iX=='#'){if(qQ(bi,b6)){continue;}return bi.Substring(0,b6);}}}return bi;
}static bool qQ(string cM,int n7){int dr=0;for(int jK=n7+1;jK<cM.Length;jK++){char dd=cM[jK];if((dd>='0'&&dd<='9')||(dd>='a'&&dd<='f')||(dd>='A'&&dd<='F'))dr++;
else break;}if(dr==3||dr==4||dr==6||dr==8){int eQ=n7+1+dr;if(eQ>=cM.Length||char.IsWhiteSpace(cM[eQ])||cM[eQ]==')'||cM[eQ]==','||cM[eQ]==':')return true;
}return false;}static bq qZ(string tl){string kI=tl.Substring("@bind".Length).Trim();int jg=kI.IndexOf('=');if(jg<0)throw new FormatException("missing '=' in @bind");
string oE=kI.Substring(0,jg).Trim();string ji=kI.Substring(jg+1).Trim();int ku=ji.IndexOf('(');if(ku<0)throw new FormatException("missing '(' in @bind target");
string gq=ji.Substring(0,ku).Trim().ToLowerInvariant();string rK=aZ(ji,ku);var bY=a_(rK);if(bY.Count==0)throw new FormatException("@bind target has no arguments");
var at=new bq{af=oE};if(gq=="dynamic"||gq=="var"||gq=="mem"){at.bM=a8.dV;at.br=bY.Count>0 ? ac(bY[0].Trim()): oE;
for(int iu=0;iu<bY.Count;iu++){var gH=bY[iu].Split(new[]{':'},2);if(gH.Length==2&&gH[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double ng;
if(double.TryParse(gH[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out ng))at.fu=ng;else at.hK=ac(gH[1].Trim());
}}return at;}at.bM=gq=="group" ? a8.hT : a8.qo;string fW=null;for(int fV=0;fV<bY.Count;fV++){var dq=bY[fV].Split(new[]{':'},2);if(dq.Length==2&&dq[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
fW=dq[1].Trim().ToLowerInvariant();}else if(dq.Length==2&&dq[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){at.fl=dq[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{at.a0.Add(ac(bY[fV].Trim()));}}if(at.a0.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(at.bM==a8.hT){if(fW==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(fW){case "avg": at.dK=aI.hA;break;case "sum": at.dK=aI.ma;break;case "min": at.dK=aI.lX;break;case "max": at.dK=aI.lW;
break;default: throw new FormatException($"unknown agg '{fW}' (expected avg/sum/min/max)");}}return at;}static bs q0(string f4){var n=new bs();
if(!f4.StartsWith("\""))throw new FormatException("line must start with a quoted label");int je=f4.IndexOf('"',1);if(je<0)throw new FormatException("unterminated quoted label");
n._=f4.Substring(1,je-1);string e0=f4.Substring(je+1).Trim();int gQ=e0.IndexOf('(');if(gQ<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string C=e0.Substring(0,gQ).Trim().ToLowerInvariant();string rG=aZ(e0,gQ);string ah=e0.Substring(e0.IndexOf(')',gQ)+1).Trim();
var ja=rG.Trim().Split('.');if(ja.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
n.aX=ja[0].Trim();n.I=ja[1].Trim();if(C=="bar")n.h=e.dM;else if(C=="led")n.h=e.d2;else if(C=="spark"||C=="sparkline")n.h=e.cw;
else if(C=="hazard"||C=="stripes")n.h=e.dZ;else if(C=="chevron"||C=="chevrons")n.h=e.dR;else if(C=="dots"||C=="dot")n.h=e.dU;
else if(C=="balance"||C=="horizon"||C=="centered")n.h=e.cY;else if(C=="toggle"||C=="switch")n.h=e.cz;else if(C=="checkbox"||C=="check")n.h=e.cp;
else if(C=="status"||C=="badge"||C=="state")n.h=e.cx;else if(C=="ammo"||C=="pips")n.h=e.cW;else if(C=="flow"||C=="rate"||C=="spinner")n.h=e.cr;
else if(C=="text")n.h=e.ia;else if(C=="tank"||C=="tube"||C=="bucket")n.h=e.fL;else throw new FormatException($"unknown display function '{C}'");
while(ah.Length>0){int i8=ah.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);int dk=ah.IndexOf('(');if(i8>=0&&(dk<0||i8<dk)){
string gf=ah.Substring(i8+4).Trim();int ha=gf.IndexOf(' ');n.lE=(ha>=0 ? gf.Substring(0,ha): gf).ToLowerInvariant();ah=(ha>=0 ? gf.Substring(ha): "").Trim();
continue;}int lk=ah.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(lk>=0&&(dk<0||lk<dk)){string hu=ah.Substring(lk+5).Trim();
int hb=hu.IndexOf(' ');n.ea=(hb>=0 ? hu.Substring(0,hb): hu).Trim().Trim('"');ah=(hb>=0 ? hu.Substring(hb): "").Trim();continue;}int jq=ah.IndexOf("format:",StringComparison.OrdinalIgnoreCase);
if(jq>=0&&(dk<0||jq<dk)){string gk=ah.Substring(jq+7).Trim();int hc=gk.IndexOf(' ');n.dW=(hc>=0 ? gk.Substring(0,hc): gk).Trim().Trim('"');
ah=(hc>=0 ? gk.Substring(hc): "").Trim();continue;}int gK=ah.IndexOf('(');if(gK<0)break;string ae=ah.Substring(0,gK).Trim().ToLowerInvariant();
int op=ae.LastIndexOf(' ');if(op>=0)ae=ae.Substring(op+1).Trim();string aE=aZ(ah,gK);ah=ah.Substring(ah.IndexOf(')',gK)+1).Trim();
if(ae=="gradient"){n.cs=true;n.bc=a_(aE).Select(uf=>uf.Trim()).ToArray();if(n.bc.Length<2)throw new FormatException("gradient() needs at least 2 colors");
}else if(ae=="size"){string lb=aE.Trim().ToLowerInvariant();n.fJ=lb=="large"||lb=="fill" ? 2 : lb=="medium" ? 1 : 0;}else if(ae=="color_by"){var i0=aE.Trim().Split('.');
if(i0.Length!=2)throw new FormatException("color_by() takes bindname.field");n.dT=i0[0].Trim();n.hJ=i0[1].Trim();}else if(ae=="label_color")n.d0=aE.Trim();
else if(ae=="track")n.id=aE.Trim();else if(ae=="flat"){n.av=aE.Trim();}else if(ae=="pattern"){string S=aE.Trim().ToLowerInvariant();
if(S=="hazard"||S=="stripes")n.h=e.dZ;else if(S=="chevron"||S=="chevrons")n.h=e.dR;else if(S=="dots"||S=="dot")n.h=e.dU;
else if(S=="led")n.h=e.d2;else if(S=="balance"||S=="horizon")n.h=e.cY;else if(S=="toggle"||S=="switch")n.h=e.cz;
else if(S=="checkbox"||S=="check")n.h=e.cp;else if(S=="status"||S=="badge"||S=="state")n.h=e.cx;else if(S=="ammo"||S=="pips")n.h=e.cW;
else if(S=="flow"||S=="rate"||S=="spinner")n.h=e.cr;}else if(ae=="color"){string sW;n.bK=l0(aE,out sW);
}else if(ae=="dir"||ae=="direction"){n.lE=aE.Trim().ToLowerInvariant();}else if(ae=="unit"){n.ea=aE.Trim().Trim('"');}else if(ae=="max"){
double oz;if(double.TryParse(aE.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out oz))n.fF=oz;else{var ka=aE.Trim().Split('.');
if(ka.Length!=2)throw new FormatException("max() takes a number or bindname.field");n.c3=ka[0].Trim();n.hW=ka[1].Trim();}}else if(ae=="format"||ae=="fmt"){
n.dW=aE.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{ae}'");}}return n;}static string aZ(string g6,int ki){
int i7=0;for(int eG=ki;eG<g6.Length;eG++){if(g6[eG]=='(')i7++;else if(g6[eG]==')'){i7--;if(i7==0)return g6.Substring(ki+1,eG-ki-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>a_(string rL){return rL.Split(',').Select(ug=>ug.Trim()).Where(uh=>uh.Length>0).ToList();
}static List<aY>l0(string rF,out string jm){var g4=new List<aY>();jm=null;foreach(var kv in a_(rF)){int i4=kv.LastIndexOf(':');
if(i4<0)throw new FormatException("color() entries need 'expr : color'");string gI=kv.Substring(0,i4).Trim();string m9=kv.Substring(i4+1).Trim();
if(gI.Equals("default",StringComparison.OrdinalIgnoreCase)){g4.Add(new aY{c1=true,cq=m9});}else{string pC=l9(gI);
if(jm==null)jm=gI.Substring(0,gI.Length-pC.Length).Trim();g4.Add(new aY{hM=pC,cq=m9});}}if(!g4.Any(t3=>t3.c1))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return g4;}static void l7(string t4,out string r1,out string sz){var eT=(t4 ?? "").Split('.');if(eT.Length!=2||eT[0].Trim().Length==0||eT[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
r1=eT[0].Trim();sz=eT[1].Trim();}static string l9(string di){di=di.Trim();foreach(var tx in new[]{">=","<=","==",">","<"}){int jL=di.IndexOf(tx,StringComparison.Ordinal);
if(jL>0)return di.Substring(jL).Trim();if(jL==0)return di;}throw new FormatException($"color() rule has no recognizable comparison operator: '{di}'");}
static string ac(string cN){if(cN.Length>=2&&cN[0]=='"'&&cN[cN.Length-1]=='"')return cN.Substring(1,cN.Length-2);return cN;}}public static class qm{public static double ql(aI rB,List<double>dI){
if(dI==null||dI.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(rB){case aI.hA: return dI.Average();case aI.ma: return dI.Sum();
case aI.lX: return dI.Min();case aI.lW: return dI.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class H{
public static string rc(string tc,double gS,int mF=10,string p6=null){gS=Math.Max(0,Math.Min(100,gS));int nG=(int)Math.Round(mF*(gS/100.0));
string rX=new string('#',nG)+new string('-',mF-nG);string uU=p6!=null ? p6 : $"{gS:0}%";return $"{tc} [{rX}] {uU}";
}public static string lM(double ci,string lj=null,string gp=null){if(lj=="auto"){double iv=Math.Abs(ci);if(iv>=1e9)return(ci/1e9).ToString("F1")+"G";
if(iv>=1e6)return(ci/1e6).ToString("F1")+"M";if(iv>=1e3)return(ci/1e3).ToString("F1")+"k";string sx= !string.IsNullOrEmpty(gp)? gp : "F0";return ci.ToString(sx);
}string nI= !string.IsNullOrEmpty(gp)? gp :(Math.Abs(ci)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(lj)){return ci.ToString(nI);}return ci.ToString(nI)+lj;
}public static float qM(string tU,float pE,float k9,float gg=1.0f){string du=(tU ?? "standard").ToLowerInvariant();
if(du=="compact"||(du=="auto"&&pE<=300&&k9<=300&&k9>160)){return gg*0.75f;}if(du=="corner"||(du=="auto"&&k9<=160)){
return gg*0.55f;}if(du=="wide"||(du=="auto"&&pE>=1000)){return gg*1.15f;}return gg;}public static string ra(string td,double gT,int b4=5){
gT=Math.Max(0,Math.Min(100,gT));double gh=(gT-50.0)/50.0;int eN=0;int e3=0;if(gh< -0.01)eN=(int)Math.Round(-gh*b4);
else if(gh>0.01)e3=(int)Math.Round(gh*b4);if(eN>b4)eN=b4;if(e3>b4)e3=b4;string tg=new string('-',b4-eN)+new string('<',eN);
string t9=new string('>',e3)+new string('-',b4-e3);return $"{td} [{tg}|{t9}] {gT:0}%";}public static string rm(string ol,double uW){
return uW>=50.0 ? $"{ol} [====O] ON" : $"{ol} [O====] OFF";}public static string rf(string om,double uX){return uX>=50.0 ? $"{om} [X] ACTIVE" : $"{om} [ ] READY ";
}public static string rl(string j2,double p4){if(p4>=75.0)return $"{j2} [ ONLINE  ]";if(p4>=25.0)return $"{j2} [ STANDBY ]";return $"{j2} [ OFFLINE ]";
}public static string q9(string te,double gU,int ov=8){gU=Math.Max(0,Math.Min(100,gU));int nH=(int)Math.Round(ov*(gU/100.0));
string tO=new string('|',nH)+new string('.',ov-nH);return $"{te} [{tO}] {gU:0}%";}public static string rg(string j3,double eX,int pQ=0){
char[]uv=new[]{'|','/','-','\\'};char pf=uv[(pQ>=0 ? pQ : 0)%4];if(eX>55.0)return $"{j3} {pf} ▲ CHG {eX:0}%";if(eX<45.0)return $"{j3} {pf} ▼ DSC {eX:0}%";
return $"{j3} ● STBY {eX:0}%";}public static bool qU(string az,string eJ){if(az==null||eJ==null)return false;if(az=="*"||az==eJ)return true;
if(!az.Contains("*")&& !az.Contains("?"))return string.Equals(az,eJ,StringComparison.OrdinalIgnoreCase);int aF=0,eH=0,k0= -1,j9=0;while(eH<eJ.Length){
if(aF<az.Length&&(az[aF]=='?'||char.ToLowerInvariant(az[aF])==char.ToLowerInvariant(eJ[eH]))){aF++;eH++;}else if(aF<az.Length&&az[aF]=='*'){
k0=aF;j9=eH;aF++;}else if(k0!= -1){aF=k0+1;j9++;eH=j9;}else{return false;}}while(aF<az.Length&&az[aF]=='*')aF++;return aF==az.Length;
}public static int qq(double tB,int pV){return Math.Max(0,Math.Min(pV,1+(int)(Math.Max(0.0,tB)*pV)));}public static string fy(string[]dF,double kw){
if(dF==null||dF.Length==0)return "white";kw=Math.Max(0,Math.Min(100,kw));int jM=(int)Math.Floor(kw/100.0*dF.Length);if(jM>=dF.Length)jM=dF.Length-1;
return dF[jM];}public static o lO(string[]cg,double tK){if(cg==null||cg.Length==0)return o.fM;if(cg.Length==1)return o.d3(cg[0]);
double t7=Math.Max(0.0,Math.Min(1.0,tK/100.0));double pj=t7*(cg.Length-1);int jN=(int)Math.Floor(pj);int sV=Math.Min(cg.Length-1,jN+1);
double k4=pj-jN;o de=o.d3(cg[jN]);o iY=o.d3(cg[sV]);byte t5=(byte)Math.Round(de.bP+(iY.bP-de.bP)*k4);
byte sO=(byte)Math.Round(de.bL+(iY.bL-de.bL)*k4);byte rV=(byte)Math.Round(de.bG+(iY.bG-de.bG)*k4);return new o(t5,sO,rV);}public static string d4(List<aY>pb,double uY){
foreach(var kJ in pb){if(kJ.c1)continue;if(qI(kJ.hM,uY))return kJ.cq;}var nf=pb.FirstOrDefault(t6=>t6.c1);return nf!=null ? nf.cq : null;
}static bool qI(string ex,double ff){ex=ex.Trim();foreach(var kh in new[]{">=","<=","==",">","<"}){if(ex.StartsWith(kh)){double e2=double.Parse(ex.Substring(kh.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(kh){case ">=": return ff>=e2;case "<=": return ff<=e2;case "==": return Math.Abs(ff-e2)<1e-9;case ">": return ff>e2;case "<": return ff<e2;
}}}throw new FormatException($"unsupported comparison expression: '{ex}'");}public static List<string>re(List<string>sg,int dJ){
var gM=new List<string>();gM.Add("+"+new string('-',dJ+2)+"+");foreach(var j6 in sg){string uO=j6.Length>dJ ? j6.Substring(0,dJ): j6;
string tE=uO.PadRight(dJ);gM.Add("| "+tE+" |");}gM.Add("+"+new string('-',dJ+2)+"+");return gM;}public struct l_{public float mh,mi,mg,lQ;
}public static l_ qw(int m5,int o8,int hd,int he,int fb,int fc,float uF,float uE,float oQ,float oR,float jx,float jy){
fb=Math.Max(1,fb);fc=Math.Max(1,fc);hd=Math.Max(1,Math.Min(fb-m5,hd));he=Math.Max(1,Math.Min(fc-o8,he));
float rO=uF-(oQ*2f);float rN=uE-(oR*2f);float m0=(rO-(jx*(fb-1)))/fb;float m_=(rN-(jy*(fc-1)))/fc;
float tW=oQ+m5*(m0+jx);float tX=oR+o8*(m_+jy);float tV=hd*m0+((hd-1)*jx);float tM=he*m_+((he-1)*jy);return new l_{
mh=tW,mi=tX,mg=tV,lQ=tM};}public struct l4{public int qX;public int hV;public int ag;public int aB;public int d9;
public int a9;public int rt;public int ru;public int qF;public int qG;
public double qH;}public static l4 qx(z ce){var k=new l4();var oT=ce.aj!=null&&ce.aj.Count>0 ? ce.aj : new List<aC>{
new aC{an=ce.fE}};k.qX=oT.Count;k.hV=0;k.ag=0;k.aB=0;if(!string.IsNullOrEmpty(ce.bH)&& !ce.bH.Equals("none",StringComparison.OrdinalIgnoreCase))k.ag++;
if(!string.IsNullOrEmpty(ce.cZ)&& !ce.cZ.Equals("none",StringComparison.OrdinalIgnoreCase))k.ag+=2;foreach(var eU in oT){
if(!string.IsNullOrEmpty(eU.cX)&& !eU.cX.Equals("none",StringComparison.OrdinalIgnoreCase))k.ag++;if(!string.IsNullOrEmpty(eU.cy))k.ag+=2;
k.hV+=eU.an.Count;foreach(var tb in eU.an){k.rt++;k.ru++;k.ag+=2;switch(tb.h){case e.dM: k.d9++;
k.a9++;k.ag+=2;k.aB++;break;case e.d2: k.a9+=10;k.ag+=10;
k.aB+=10;break;case e.dZ: k.a9+=10;k.ag+=11;k.aB+=10;break;case e.dR: k.a9+=10;
k.ag+=10;k.aB+=10;break;case e.dU: k.a9+=12;k.ag+=12;k.aB+=12;
break;case e.cw: k.d9++;k.a9+=8;k.ag+=9;k.aB+=8;break;case e.cY: k.d9+=2;
k.a9++;k.ag+=3;k.aB++;break;case e.cz: case e.cp: k.d9++;
k.a9++;k.ag+=2;k.aB++;break;case e.cx: case e.cr: k.d9++;k.a9+=2;
k.ag+=3;k.aB+=2;break;case e.cW: k.a9+=8;k.ag+=8;k.aB+=8;
break;default: k.aB++;break;}}}k.qF=25+(k.hV*2);k.qG=180+(k.aB*4);
k.qH=0.0;return k;}}