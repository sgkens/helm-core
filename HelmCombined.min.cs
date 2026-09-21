hQ r;string h6=null;Dictionary<string,List<IMyTerminalBlock>>bK=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>fz=new Dictionary<string,bool>();
Dictionary<string,bk>d0=new Dictionary<string,bk>();Dictionary<string,Color>l4=new Dictionary<string,Color>();List<IMyTerminalBlock>co=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>cV=new List<IMyTerminalBlock>();bool fw;int h3;string d1="";StringBuilder d4=new StringBuilder();readonly Dictionary<string,double>aJ=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>cU=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener d2;bool cp=false;
Dictionary<z,IMyTextSurface>bL=new Dictionary<z,IMyTextSurface>();Dictionary<z,string>cq=new Dictionary<z,string>();
static readonly string[]lK=new string[101];static Program(){for(int gc=0;gc<=100;gc++){lK[gc]=gc+"%";}}static string ly(double uC){int ev=(int)Math.Round(uC);
if(ev<0)ev=0;else if(ev>100)ev=100;return lK[ev];}class aC{public List<MySprite>f=new List<MySprite>();public List<ch>dU=new List<ch>();
public int ca= -1;public Color[]e8;public Color e4;public bool e5;public List<hP>bF=new List<hP>();public Vector2 dP,hM;
public int dZ;public bool dN=false;public List<e9>dC=new List<e9>();}class hP{public string lJ;public int hU;public Vector2 lO,lS;
}class e9{public int fj;public Color hj,fd;public List<aT>fr,fg;public string hn,hx,hy,hA;public bool lE,hC;
}class ch{public bm fm;public int W= -1;public int aX= -1;public int cQ= -1;public bool lA=false;
public List<int>K=new List<int>();public float[]ft=new float[8];public float b9;public float bj;public float a3;
public float b8;public double fl= -99999.0;public string lG;public float E= -1f;public bool l_;public float lo=12f;}Dictionary<z,aC>bM=new Dictionary<z,aC>();
int l6=0;double h8=0;double d5=0;double _=0;int d3=0;Dictionary<z,double>bp=new Dictionary<z,double>();
Dictionary<z,double>h2=new Dictionary<z,double>();Dictionary<string,int>fC=new Dictionary<string,int>();List<string>cW=new List<string>();
Dictionary<string,int>fD=new Dictionary<string,int>();double ia;Dictionary<string,double>d7=new Dictionary<string,double>();List<MyInventoryItem>h5=new List<MyInventoryItem>();
int bN= -1,fB;double h9;Dictionary<z,double>fx=new Dictionary<z,double>();int d6=0;const string qw="0.11.0";
IMyTextSurface aY;bool h1=true;double fy=0;string l5="";string h7="";int fA=0,h4=0;List<KeyValuePair<string,int>>cT=new List<KeyValuePair<string,int>>();
const double qO=120;const float p1=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{d2=IGC.RegisterBroadcastListener("HELM");
d2.SetMessageCallback("HELM_IGC");}catch{}fq();hV();lD();}public void Main(string mj,UpdateType uy){
double nb=Runtime.TimeSinceLastRun.TotalSeconds;_+=nb;if((uy&UpdateType.IGC)!=0||mj=="HELM_IGC"){qM();}string oP=(mj ?? "").Trim();
string an=oP.ToLowerInvariant();if(an=="reload"){fq();}else if(an=="refresh"){hS();}else if(an=="boot"){hV();}else if(an=="probe"){
d5=_+20.0;ql();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(an=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=q9;
fq();hV();}else if(an=="console on"||an=="console off"){h1=an=="console on";lD();fy=0;}else if(an=="scan"||an=="diagnose"||an=="test"){
q6();return;}else if(an.StartsWith("set ")){string gJ=oP.Substring(4).Trim();int nf=gJ.IndexOf('=');int t8=gJ.IndexOf(' ');int kF=nf>0 ? nf : t8;
if(kF>0){string nW=gJ.Substring(0,kF).Trim();string pH=gJ.Substring(kF+1).Trim();lg($"{nW}={pH}");Echo($"[HELM] Set dynamic variable '{nW}' = {pH}");
}return;}else if(an.StartsWith("alert")){string e0=an.Length>5 ? an.Substring(5).Trim(): "toggle";if(e0=="toggle")cp= !cp;else cp=e0=="red"||e0=="on"||e0=="1"||e0=="true";
Echo($"[HELM] Red alert state: {cp}");return;}else if(Me.CustomData!=h6){fq();}d6++;h8+=nb;if(h8>=qO){
h8=0;hS();}if(d5>0){if(_<d5){h0();return;}d5=0;bM.Clear();cq.Clear();
fy=0;}if(r==null||r.T.Count==0){h0();lq();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cD=r.T[l6%r.T.Count];
l6++;h7=cD.a9;bool ie=bp.Count>0;if(ie)qV();double nc;if(!bp.ContainsKey(cD)&& !(h2.TryGetValue(cD,out nc)&&_<nc)){
if(q_(cD))ie=true;if(cD.hR>0)h2[cD]=_+cD.hR/60.0;}d3=ie ? 0 : d3+1;Runtime.UpdateFrequency=d3<=r.T.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
q8();q7();h0();lq();fA=Runtime.CurrentInstructionCount;if(fA>h4)h4=fA;}void lq(){if(d1.Length>0)Echo(d1);
if(d4.Length>0)Echo(d4.ToString());}List<IMyTerminalBlock>e6(bool rj=false){if(!fw){cV.Clear();GridTerminalSystem.GetBlocks(cV);
co.Clear();co.AddRange(cV);co.RemoveAll(rw=> !rw.IsSameConstructAs(Me));h3=cV.Count-co.Count;fw=true;
}return rj ? cV : co;}IMyTerminalBlock lt(string ok,bool rl=false){var ip=GridTerminalSystem.GetBlockWithName(ok);if(ip==null||rl||ip.IsSameConstructAs(Me))return ip;
var ih=e6();for(int gd=0;gd<ih.Count;gd++)if(ih[gd].CustomName.IndexOf(ok,StringComparison.OrdinalIgnoreCase)>=0)return ih[gd];return null;}void qc(string ol,List<IMyTerminalBlock>ex,bool mc=false){
var ny=GridTerminalSystem.GetBlockGroupWithName(ol);if(ny==null)return;int mu=ex.Count;var eX=new List<IMyTerminalBlock>();ny.GetBlocks(eX);le(eX,ex,mc);
if(ex.Count>mu)return;var nJ=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(nJ);foreach(var pW in nJ){if(pW.Name.IndexOf(ol,StringComparison.OrdinalIgnoreCase)<0)continue;
eX.Clear();pW.GetBlocks(eX);le(eX,ex,mc);if(ex.Count>mu)return;}}void le(List<IMyTerminalBlock>sq,List<IMyTerminalBlock>nU,bool rk=false){
foreach(var iq in sq)if((rk||iq.IsSameConstructAs(Me))&& !nU.Contains(iq))nU.Add(iq);}void fq(){h6=Me.CustomData;r=lC.qF(h6);
d1="";if(r.a6.Count>0){var o_=new StringBuilder("HELM config errors:\n");foreach(var r9 in r.a6)o_.AppendLine(" - "+r9);d1=o_.ToString();
}if(r.d_.Count>0){var pU=new StringBuilder("HELM notes:\n");foreach(var uH in r.d_)pU.AppendLine(" - "+uH);d1+=pU.ToString();}d0.Clear();
foreach(var ir in r.cb)if(!d0.ContainsKey(ir.ae))d0[ir.ae]=ir;bM.Clear();cq.Clear();bp.Clear();h2.Clear();
fx.Clear();qr();hS();}void qM(){try{while(d2!=null&&d2.HasPendingMessage){
var og=d2.AcceptMessage();if(og.Data is string){lg((string)og.Data);}}}catch{}}void lg(string oG){
if(string.IsNullOrEmpty(oG))return;var oD=oG.Split(';');for(int jm=0;jm<oD.Length;jm++){var eI=oD[jm].Trim();if(eI.Length==0)continue;int ei=eI.IndexOf('=');
if(ei<0)ei=eI.IndexOf(':');if(ei>0){string db=eI.Substring(0,ei).Trim();string bi=eI.Substring(ei+1).Trim();if(db.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cp=bi.Equals("red",StringComparison.OrdinalIgnoreCase)||bi=="1"||bi.Equals("true",StringComparison.OrdinalIgnoreCase)||bi.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double op;cU.Remove(db);if(double.TryParse(bi,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out op)){
aJ[db]=op;}else if(bi.Equals("true",StringComparison.OrdinalIgnoreCase)||bi.Equals("on",StringComparison.OrdinalIgnoreCase)){aJ[db]=100.0;
}else if(bi.Equals("false",StringComparison.OrdinalIgnoreCase)||bi.Equals("off",StringComparison.OrdinalIgnoreCase)){aJ[db]=0.0;}else{cU[db]=bi;
}}}}}const string q9="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void ql(){var ii=new List<IMyTextSurface>();if(aY!=null)ii.Add(aY);foreach(var tc in bL.Values)ii.Add(tc);foreach(var t3 in ii)qk(t3);
}static void bn(List<MySprite>t9,float r2,float r3,float uI,float sx,Color rR){t9.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(r2,r3),Size=new Vector2(uI,sx),Color=rR}
);}void qk(IMyTextSurface kG){Vector2 aO=kG.SurfaceSize,aG=kG.TextureSize,al=(aG-aO)/2f;var aF=new List<MySprite>();Color ga=new Color(150,150,150);
bn(aF,aG.X/2f,2f,aG.X,4f,ga);bn(aF,aG.X/2f,aG.Y-2f,aG.X,4f,ga);bn(aF,2f,aG.Y/2f,4f,aG.Y,ga);bn(aF,aG.X-2f,aG.Y/2f,4f,aG.Y,ga);
bn(aF,al.X+aO.X/2f,al.Y+aO.Y/2f,aO.X,aO.Y,new Color(20,40,90));float ak=32f;bn(aF,al.X+ak/2f,al.Y+ak/2f,ak,ak,new Color(255,0,0));bn(aF,al.X+aO.X-ak/2f,al.Y+ak/2f,ak,ak,new Color(0,255,0));
bn(aF,al.X+ak/2f,al.Y+aO.Y-ak/2f,ak,ak,new Color(255,255,0));bn(aF,al.X+aO.X-ak/2f,al.Y+aO.Y-ak/2f,ak,ak,new Color(255,0,255));float lc=al.Y+aO.Y/2f-30f;
a2(aF,"SURFACE "+aO.X.ToString("0")+"x"+aO.Y.ToString("0"),Color.White,al.X+44f,lc,0.9f);a2(aF,"TEXTURE "+aG.X.ToString("0")+"x"+aG.Y.ToString("0"),Color.White,al.X+44f,lc+24f,0.9f);
a2(aF,"OFFSET  "+al.X.ToString("0")+","+al.Y.ToString("0"),Color.White,al.X+44f,lc+48f,0.9f);using(var sj=kG.DrawFrame()){sj.AddRange(aF);}}static bool lF(string c6){
return c6.Length>3&&(c6.StartsWith("top")&&char.IsDigit(c6[3])||c6.Length>4&&c6.StartsWith("line")&&char.IsDigit(c6[4]));}void qr(){fC.Clear();cW.Clear();
fD.Clear();bN= -1;h9=ia=0;foreach(var kB in r.T){var jS=new List<List<bm>>();foreach(var ty in kB.ai)jS.Add(ty.am);
if(kB.ai.Count==0)jS.Add(kB.fn);foreach(var s7 in jS)foreach(var eB in s7){if(!lF(eB.I))continue;bool pC=eB.I[0]=='t';var m4=pC ? fC : fD;
int s8=int.Parse(eB.I.Substring(pC ? 3 : 4));int nN;m4.TryGetValue(eB.aS,out nN);m4[eB.aS]=Math.Max(nN,s8);}}cW.AddRange(fC.Keys);
}void q7(){if(fD.Count==0||_<ia)return;ia=_+2.0;foreach(var jJ in fD){List<IMyTerminalBlock>iz;var o8=bK.TryGetValue(jJ.Key,out iz)&&iz.Count>0 ? iz[0]as IMyTextSurface : null;
string[]od=o8==null ? new string[0]: o8.GetText().Split('\n');for(int eL=1;eL<=jJ.Value;eL++)cU[jJ.Key+".line"+eL]=eL<=od.Length ? od[eL-1].TrimEnd('\r'): "";}}
void q8(){if(cW.Count==0)return;if(bN<0){if(_<h9)return;bN=0;fB=0;d7.Clear();}while(bN<cW.Count&&Runtime.CurrentInstructionCount<20000){
string iC=cW[bN];List<IMyTerminalBlock>fN;bK.TryGetValue(iC,out fN);if(fN!=null&&fB<fN.Count){var ml=fN[fB++];
for(int jn=0;jn<ml.InventoryCount;jn++){h5.Clear();ml.GetInventory(jn).GetItems(h5);foreach(var gk in h5){string jF=gk.Type.TypeId.Replace("MyObjectBuilder_","");
string nX=jF=="Ore"||jF=="Ingot" ? gk.Type.SubtypeId+" "+jF.ToLowerInvariant(): gk.Type.SubtypeId;double nO;d7.TryGetValue(nX,out nO);d7[nX]=nO+(double)gk.Amount;
}}continue;}var gH=new List<KeyValuePair<string,double>>(d7);gH.Sort((uK,uP)=>uP.Value.CompareTo(uK.Value));for(int dk=1;dk<=fC[iC];dk++)cU[iC+".top"+dk]=dk<=gH.Count ? gH[dk-1].Key+" "+H.lx(gH[dk-1].Value,"auto"): "-";
d7.Clear();bN++;fB=0;}if(bN>=cW.Count){bN= -1;h9=_+15.0;}}void ah(int ui,string uq){cT.Add(new KeyValuePair<string,int>(uq,ui));
}static string h_(string ku,int oj){return ku.Length>oj ? ku.Substring(0,oj): ku;}void lD(){var kn=Me as IMyTextSurfaceProvider;if(kn==null||kn.SurfaceCount<1)return;
aY=kn.GetSurface(0);aY.ContentType=h1 ? ContentType.SCRIPT : ContentType.NONE;aY.Script="";aY.ScriptBackgroundColor=Color.Black;
}void h0(){if(_>=fy){fy=_+(d3>r.T.Count+1 ? 5.0 : 1.0);p9();if(h1&&aY!=null&&d5==0)qj();
}Echo(l5);}void p9(){cT.Clear();int iF=0,mB=0;foreach(var mm in r.cb){List<IMyTerminalBlock>jM;if(mm.bD==a4.dI){
iF++;continue;}if(bK.TryGetValue(mm.ae,out jM)){mB+=jM.Count;if(jM.Count>0)iF++;}}int ng=r.a6.Count;ah(0,"HELM-[Kernel-(v"+qw+")]");
ah(4,"------------------------------");ah(1,"krn-cfg");ah(2," screens "+r.T.Count+"  binds "+iF+"/"+r.cb.Count+"  errors "+ng);
ah(2," alert "+(cp ? "RED" : "off")+"  blocks "+mB+"  docked-ignored "+h3);if(ng>0)ah(3," ! "+h_(r.a6[0],40));
else if(r.d_.Count>0)ah(3," ? "+h_(r.d_[0],40));ah(1,"cfg-dsp");if(r.T.Count==0)ah(3," no @screen: run 'starter'");
int t5=0;foreach(var kv in r.T){if(++t5>10){ah(2," +"+(r.T.Count-10)+" more");break;}IMyTextSurface kH;string pa="--",kM="no block";
if(bL.TryGetValue(kv,out kH)){pa=kH.SurfaceSize.X.ToString("0")+"x"+kH.SurfaceSize.Y.ToString("0");kM=bp.ContainsKey(kv)? "boot" : "live";
}ah(kM=="no block" ? 3 : 2," "+h_(kv.a9,14).PadRight(14)+" "+pa.PadRight(9)+" "+kM);}ah(1,"perf");ah(2," tick "+d6+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ah(2," instr "+fA+"  peak "+h4+"  max "+Runtime.MaxInstructionCount);ah(2," rate "+Runtime.UpdateFrequency+"  idle "+d3+"  up "+(int)_+"s");
if(h7.Length>0)ah(2," last "+h7);var o0=new StringBuilder();foreach(var sN in cT)o0.AppendLine(sN.Key);l5=o0.ToString();}static Color qf(int uj){
switch(uj){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void qj(){Vector2 cE=aY.SurfaceSize;Vector2 jZ=(aY.TextureSize-cE)/2f;int iO=30;foreach(var sO in cT)iO=Math.Max(iO,sO.Key.Length);
Vector2 iN=aY.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float o1=Math.Min(1f,Math.Min(cE.X*0.94f/(iO*iN.X),cE.Y*0.94f/(cT.Count*iN.Y)));
float uL=jZ.X+cE.X*0.03f;float pZ=jZ.Y+cE.Y*0.03f;var kI=new List<MySprite>();kI.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jZ+cE/2f,Size=cE,Color=new Color(12,12,16)}
);foreach(var n1 in cT){a2(kI,n1.Key,qf(n1.Value),uL,pZ,o1);pZ+=iN.Y*o1;}using(var sk=aY.DrawFrame()){sk.AddRange(kI);}}static readonly List<string>p8=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void hV(){bp.Clear();if(r==null)return;for(int ge=0;ge<r.T.Count;ge++){var gP=r.T[ge];if(gP.hq>0f)bp[gP]=_+(ge/Math.Max(1,gP.li))*gP.ll/60.0;
}}void qV(){List<z>eh=null;int sw=(bp.Count+3)/4,sG=0;foreach(var go in bp){double j6=(_-go.Value)/go.Key.hq;if(j6<0)continue;
if(j6>=1.0){if(eh==null)eh=new List<z>();eh.Add(go.Key);continue;}if((sG++ +d6)%sw==0)qi(go.Key,j6);}if(eh==null)return;foreach(var eS in eh){
bp.Remove(eS);if(eS.hp>0f)fx[eS]=_;bM.Remove(eS);cq.Remove(eS);}}static void a2(List<MySprite>ua,string ur,Color rT,float uM,float uQ,float t0){
var pp=MySprite.CreateText(ur,"Monospace",rT,t0,TextAlignment.LEFT);pp.Position=new Vector2(uM,uQ);ua.Add(pp);}static void ld(List<MySprite>ub,List<string>fI,z gS,float uN,float uR,float t1,float sZ){
string[]ug=gS.fa!=null&&gS.fa.Length>0 ? gS.fa : new[]{gS.lj};for(int eq=0;eq<fI.Count;
eq++){o iH=H.lz(ug,fI.Count>1 ? 100.0*eq/(fI.Count-1): 0.0);a2(ub,fI[eq],new Color(iH.bG,iH.bC,iH.bw),uN,uR+eq*sZ,t1);}}bool qZ(z dm,IMyTextSurface kQ){
aC ax,aL;z o4=r.T.Find(pX=>pX!=dm&&pX.a9.Equals(dm.hI,StringComparison.OrdinalIgnoreCase));if(o4==null|| !bM.TryGetValue(o4,out ax)|| !ax.dN)return false;
if(!bM.TryGetValue(dm,out aL))bM[dm]=aL=new aC();Vector2 dq=kQ.SurfaceSize;if(aL.dN&&aL.dZ==ax.dZ&&aL.dP==dq)return false;
aL.dN=true;aL.dZ=ax.dZ;aL.dP=dq;aL.f.Clear();Vector2 g0=ax.dP,ou=ax.hM,os=(kQ.TextureSize-dq)/2f;aL.hM=os;
int nv=0,pB=ax.f.Count;if(dm.hJ!=null){int dj=ax.bF.FindIndex(uO=>uO.lJ.Equals(dm.hJ,StringComparison.OrdinalIgnoreCase));
if(dj<0)return false;g0=ax.bF[dj].lS;ou=ax.bF[dj].lO;nv=ax.bF[dj].hU;if(dj+1<ax.bF.Count)pB=ax.bF[dj+1].hU;}float gm=Math.Min(dq.X/g0.X,dq.Y/g0.Y);
Vector2 rt=os+(dq-g0*gm)/2f;for(int jo=nv;jo<pB;jo++){var br=ax.f[jo];if(br.Position.HasValue)br.Position=(br.Position.Value-ou)*gm+rt;if(br.Size.HasValue)br.Size=br.Size.Value*gm;
if(br.Type==SpriteType.TEXT)br.RotationOrScale*=gm;aL.f.Add(br);}using(var sl=kQ.DrawFrame())sl.AddRange(aL.f);return true;}bool qT(z dn,IMyTextSurface gZ){
List<string>d9;if(cq.ContainsKey(dn)|| !r.dz.TryGetValue(dn.b7,out d9))return false;cq[dn]="banner";Vector2 cF=gZ.SurfaceSize;
Vector2 j_=(gZ.TextureSize-cF)/2f;int fT=1;foreach(var sP in d9)fT=Math.Max(fT,sP.Length);Vector2 ef=gZ.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float gR=Math.Min(cF.X*0.94f/(fT*ef.X),cF.Y*0.94f/(d9.Count*ef.Y));var kJ=new List<MySprite>();kJ.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=j_+cF/2f,Size=cF,Color=string.IsNullOrEmpty(dn.bx)? new Color(10,12,16): F(dn.bx)}
);ld(kJ,d9,dn,j_.X+(cF.X-fT*ef.X*gR)/2f,j_.Y+(cF.Y-d9.Count*ef.Y*gR)/2f,gR,ef.Y*gR);using(var sm=gZ.DrawFrame()){sm.AddRange(kJ);
}return true;}void qi(z dp,double ox){IMyTextSurface eU;if(!bL.TryGetValue(dp,out eU))return;Vector2 b3=eU.SurfaceSize;
Vector2 j0=(eU.TextureSize-b3)/2f;var eC=dp.qs();string oh=dp.lk ?? "HELM TELEMETRY KERNEL ONLINE";bool jT=b3.Y>=200f;
List<string>cX;if(dp.cc==null|| !r.dz.TryGetValue(dp.cc,out cX))cX=p8;int c3=oh.Length+3;foreach(var sQ in eC)c3=Math.Max(c3,sQ.Length+5);
if(jT)foreach(var sR in cX)c3=Math.Max(c3,sR.Length);int oT=(jT ? cX.Count+1 : 0)+2+eC.Count;Vector2 c2=eU.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float b_=Math.Min(1f,Math.Min(b3.X*0.94f/(c3*c2.X),b3.Y*0.94f/(oT*c2.Y)));float hf=j0.X+(b3.X-c3*c2.X*b_)/2f;float cJ=j0.Y+(b3.Y-oT*c2.Y*b_)/2f;
float gs=c2.Y*b_;var dr=new List<MySprite>();dr.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=j0+b3/2f,Size=b3,Color=new Color(10,12,16)}
);if(jT){ld(dr,cX,dp,hf,cJ,b_,gs);cJ+=(cX.Count+1)*gs;}a2(dr,">> "+oh,new Color(34,197,94),hf,cJ,b_);cJ+=gs*2f;int t6=H.p7(ox,eC.Count);
for(int gf=0;gf<t6;gf++){bool ot=gf==eC.Count-1&&ox>=0.8;a2(dr,ot ? "[OK]" : "[..]",ot ? new Color(34,197,94): new Color(56,189,248),hf,cJ,b_);a2(dr,eC[gf],new Color(203,213,225),hf+5f*c2.X*b_,cJ,b_);
cJ+=gs;}using(var sn=eU.DrawFrame()){sn.AddRange(dr);}}void hS(){bK.Clear();bL.Clear();fz.Clear();
d4.Clear();fw=false;if(r==null)return;foreach(var ac in r.cb){var aZ=new List<IMyTerminalBlock>();if(ac.bD==a4.dI){
string jD= !string.IsNullOrEmpty(ac.bl)? ac.bl : ac.ae;if(!aJ.ContainsKey(jD)){aJ[jD]=ac.fe;if(ac.hu!=null)cU[jD]=ac.hu;
}bK[ac.ae]=aZ;continue;}var pt=ac.aW.Count>0 ? ac.aW : new List<string>{ac.lX};foreach(var cH in pt){
if(string.IsNullOrEmpty(cH))continue;bool nV=cH.Contains("*")||cH.Contains("?");if(ac.bD==a4.hE||nV){var jh=new List<IMyTerminalBlock>();
qc(cH,jh,ac.e7);if(jh.Count>0){foreach(var nF in jh){if(!aZ.Contains(nF))aZ.Add(nF);}}else{var me=e6(ac.e7);
for(int it=0;it<me.Count;it++){var g6=me[it];if(nV ? H.qB(cH,g6.CustomName): string.Equals(cH,g6.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aZ.Contains(g6))aZ.Add(g6);}}}}else{var iy=lt(cH,ac.e7);if(iy!=null&& !aZ.Contains(iy))aZ.Add(iy);}}if(aZ.Count==0){
q4(ac,pt,aZ);if(aZ.Count>0)fz[ac.ae]=true;}bK[ac.ae]=aZ;}foreach(var bt in r.T){
if(string.IsNullOrEmpty(bt.bE))continue;var j3=lt(bt.bE);if(j3==null){d4.AppendLine($"HELM: output block '{bt.bE}' not found");
continue;}IMyTextSurface cG=null;var ko=j3 as IMyTextSurfaceProvider;var m5=j3 as IMyTextSurface;if(ko!=null&&bt.ci<ko.SurfaceCount){
cG=ko.GetSurface(bt.ci);}else if(m5!=null&&bt.ci==0){cG=m5;}if(cG==null){d4.AppendLine($"HELM: output block '{bt.bE}' has no surface index {bt.ci}");
continue;}cG.ContentType=ContentType.SCRIPT;cG.Script="";cG.ScriptBackgroundColor=Color.Black;bL[bt]=cG;}co.Clear();
cV.Clear();}static readonly string[]ls={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool qq(int sH,IMyTerminalBlock v){switch(sH){case 0: return v is IMyBatteryBlock;case 1: return v is IMyReactor;case 2: return v is IMySolarPanel;
case 3: return v is IMyWindTurbine;case 4: return v is IMyGasTank;case 5: return v is IMyAirVent;case 6: return v is IMyCargoContainer;case 7: return v is IMyJumpDrive;
case 8: return v is IMyThrust;case 9: return v is IMyMotorSuspension;case 10: return v is IMyDoor;case 11: return v is IMyShipConnector;case 12: return v is IMyAssembler;
case 13: return v is IMyRefinery;case 14: return v is IMyShipDrill;case 15: return v is IMyShipWelder;case 16: return v is IMyShipGrinder;case 17: return v is IMyGasGenerator;
case 18: return v is IMyLargeTurretBase;case 19: return v is IMyProjector;case 20: return v is IMyGyro;case 21: return v is IMyGravityGeneratorBase;case 22: return v is IMyLandingGear;
case 23: return v is IMyRadioAntenna;case 24: return v is IMyBeacon;case 25: return v is IMyLightingBlock;case 26: return v is IMyPistonBase;case 27: return v is IMyMotorStator;
case 28: return v is IMyWarhead;case 29: return v is IMySensorBlock;case 30: return v is IMyCameraBlock;case 31: return v is IMyParachute;case 32: return v is IMyTurretControlBlock;
}return false;}void q4(bk rI,List<string>uo,List<IMyTerminalBlock>iA){string sB=(rI.ae+" "+string.Join(" ",uo)).ToLowerInvariant();
var jH=new List<int>();for(int gn=0;gn<ls.Length;gn++){foreach(var uJ in ls[gn].Split(','))if(sB.Contains(uJ)){jH.Add(gn);break;}}if(jH.Count==0)return;
var md=e6();var ov=new List<IMyTerminalBlock>();for(int jp=0;jp<md.Count;jp++){var iu=md[jp];foreach(int sI in jH){if(qq(sI,iu)){(iu.CubeGrid==Me.CubeGrid ? iA : ov).Add(iu);
break;}}}if(iA.Count==0)iA.AddRange(ov);}void q6(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(r==null||r.T.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {r.T.Count} screens, {r.cb.Count} binds");fw=false;
Echo($"Blocks: {e6().Count} on this construct; {h3} on docked ships (ignored)");co.Clear();Echo("--- BIND RESOLUTION ---");foreach(var ap in r.cb){
if(ap.bD==a4.dI){string jE= !string.IsNullOrEmpty(ap.bl)? ap.bl : ap.ae;double uz=aJ.ContainsKey(jE)? aJ[jE]: ap.fe;
Echo($"[OK] @bind {ap.ae} (dynamic variable '{jE}' = {uz})");continue;}List<IMyTerminalBlock>iB;if(bK.TryGetValue(ap.ae,out iB)){if(iB.Count>0){
string sf=fz.ContainsKey(ap.ae)&&fz[ap.ae]? " [fallback: typed match]" : "";Echo($"[OK] @bind {ap.ae} ({iB.Count} blocks matched{sf})");
}else{string up=string.Join(", ",ap.aW.Count>0 ? ap.aW : new List<string>{ap.lX});Echo($"[WARN] @bind {ap.ae} -> 0 blocks matched \"{up}\"");
}}else{Echo($"[ERR] @bind {ap.ae} -> unresolved");}}if(aJ.Count>0){Echo($"--- DYNAMIC VARIABLES ({aJ.Count}) ---");foreach(var n3 in aJ)Echo($"[VAR] {n3.Key} = {n3.Value}");
}IMyTextSurface ij=null;foreach(var tf in bL.Values){ij=tf;break;}if(ij!=null){var kK=new List<string>();ij.GetSprites(kK);
Echo($"--- SPRITES ({kK.Count}): names usable in bg_texture ---");Echo(string.Join(", ",kK.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bu in r.T){IMyTextSurface g_;if(bL.TryGetValue(bu,out g_)){var pb=g_.SurfaceSize;var ue=H.qe(bu);
Echo($"[OK] Screen \"{bu.a9}\" [{bu.cj}] -> {bu.bE} [#{bu.ci}] ({pb.X:0}x{pb.Y:0} px, texture {g_.TextureSize.X:0}x{g_.TextureSize.Y:0}, {ue.af} sprites)");
}else{Echo($"[ERR] Screen \"{bu.a9}\" -> Missing block \"{bu.bE}\" [#{bu.ci}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double q5(IMyTerminalBlock w){
if(w==null)return 0.0;var fL=w as IMyBatteryBlock;if(fL!=null)return fL.MaxStoredPower>0 ?(fL.CurrentStoredPower/fL.MaxStoredPower)*100.0 : 0.0;
var gl=w as IMyJumpDrive;if(gl!=null)return gl.MaxStoredPower>0 ?(gl.CurrentStoredPower/gl.MaxStoredPower)*100.0 : 0.0;var ps=w as IMyGasTank;
if(ps!=null)return ps.FilledRatio*100.0;var pQ=w as IMyAirVent;if(pQ!=null)return pQ.GetOxygenLevel()*100.0;var mK=w as IMyCargoContainer;if(mK!=null){
var gi=mK.GetInventory();if(gi!=null&&gi.MaxVolume.RawValue>0)return(double)gi.CurrentVolume.RawValue/gi.MaxVolume.RawValue*100.0;return 0.0;}var gI=w as IMyReactor;
if(gI!=null)return gI.MaxOutput>0 ?(gI.CurrentOutput/gI.MaxOutput)*100.0 : 0.0;var gT=w as IMySolarPanel;if(gT!=null)return gT.MaxOutput>0 ?(gT.CurrentOutput/gT.MaxOutput)*100.0 : 0.0;
var he=w as IMyWindTurbine;if(he!=null)return he.MaxOutput>0 ?(he.CurrentOutput/he.MaxOutput)*100.0 : 0.0;var m6=w as IMyDoor;if(m6!=null)return(double)m6.OpenRatio*100.0;
var g9=w as IMyThrust;if(g9!=null)return g9.MaxEffectiveThrust>0 ?(g9.CurrentThrust/g9.MaxEffectiveThrust)*100.0 : 0.0;var kV=w as IMyMotorSuspension;
if(kV!=null)return kV.Brake ? 100.0 :(kV.Friction*100.0);var iS=w as IMyShipConnector;if(iS!=null)return iS.Status==MyShipConnectorStatus.Connected ? 100.0 :(iS.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var oL=w as IMyProductionBlock;if(oL!=null)return oL.IsProducing ? 100.0 : 0.0;var na=w as IMyShipDrill;if(na!=null)return na.Enabled ? 100.0 : 0.0;
var k0=w as IMyShipToolBase;if(k0!=null)return k0.IsActivated ? 100.0 :(k0.Enabled ? 50.0 : 0.0);var je=w as IMyGasGenerator;if(je!=null)return je.AutoRefill ? 100.0 :(je.Enabled ? 50.0 : 0.0);
var ha=w as IMyLargeTurretBase;if(ha!=null)return ha.IsShooting ? 100.0 :(ha.HasTarget ? 50.0 :(ha.Enabled ? 10.0 : 0.0));var eK=w as IMyProjector;
if(eK!=null)return eK.TotalBlocks>0 ?((double)(eK.TotalBlocks-eK.RemainingBlocks)/eK.TotalBlocks)*100.0 : 0.0;var jk=w as IMyGyro;if(jk!=null)return jk.GyroOverride ? 100.0 :(jk.Enabled ? 50.0 : 0.0);
var nH=w as IMyGravityGeneratorBase;if(nH!=null)return Math.Min(100.0,Math.Abs(nH.GravityAcceleration)/9.81*100.0);var jd=w as IMyLandingGear;if(jd!=null)return jd.IsLocked ? 100.0 :(jd.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var mf=w as IMyRadioAntenna;if(mf!=null)return(mf.Radius/50000.0)*100.0;var ms=w as IMyBeacon;if(ms!=null)return(ms.Radius/50000.0)*100.0;
var ob=w as IMyLightingBlock;if(ob!=null)return ob.Intensity*10.0;var gE=w as IMyPistonBase;if(gE!=null)return gE.HighestPosition>0 ?(gE.CurrentPosition/gE.HighestPosition)*100.0 : 0.0;
var oR=w as IMyMotorStator;if(oR!=null)return Math.Min(100.0,Math.Abs(oR.Angle)/6.28318*100.0);var pT=w as IMyWarhead;if(pT!=null)return pT.IsArmed ? 100.0 : 0.0;
var kE=w as IMySensorBlock;if(kE!=null)return!kE.LastDetectedEntity.IsEmpty()? 100.0 :(kE.Enabled ? 25.0 : 0.0);var iK=w as IMyCameraBlock;
if(iK!=null)return iK.IsActive ? 100.0 :(iK.Enabled ? 50.0 : 0.0);var mQ=w as IMyParachute;if(mQ!=null)return(double)mQ.OpenRatio*100.0;
var g8=w as IMyTurretControlBlock;if(g8!=null)return g8.HasTarget ? 100.0 :(g8.IsUnderControl ? 50.0 :(g8.Enabled ? 10.0 : 0.0));var nw=w as IMyFunctionalBlock;
if(nw!=null)return nw.Enabled ? 100.0 : 0.0;return 0.0;}static double qy(IMyTerminalBlock mn,string i6){int mJ=i6.IndexOf(':');string jG=i6.Substring(0,mJ).ToLowerInvariant();
var uw=new MyItemType("MyObjectBuilder_"+(jG=="ammo" ? "AmmoMagazine" : char.ToUpper(jG[0])+jG.Substring(1)),i6.Substring(mJ+1));double pl=0;for(int jq=0;
jq<mn.InventoryCount;jq++)pl+=(double)mn.GetInventory(jq).GetItemAmount(uw);return pl;}double lP(IMyTerminalBlock x,string nm){if(x==null)return 0.0;
string b=nm.ToLowerInvariant();if(b=="percent"||b=="pct")return q5(x);if(b.IndexOf(':')>0)return qy(x,nm);var bQ=x as IMyBatteryBlock;
if(bQ!=null){if(b=="current"||b=="stored"||b=="val"||b=="value"||b=="raw")return bQ.CurrentStoredPower;if(b=="max"||b=="capacity")return bQ.MaxStoredPower;
if(b=="output")return bQ.CurrentOutput;if(b=="maxoutput")return bQ.MaxOutput;if(b=="input")return bQ.CurrentInput;if(b=="maxinput")return bQ.MaxInput;
return bQ.CurrentStoredPower;}var jC=x as IMyJumpDrive;if(jC!=null){if(b=="max"||b=="capacity")return jC.MaxStoredPower;return jC.CurrentStoredPower;
}var g3=x as IMyGasTank;if(g3!=null){if(b=="max"||b=="capacity")return g3.Capacity;return g3.FilledRatio*g3.Capacity;}var pR=x as IMyAirVent;
if(pR!=null){if(b=="max"||b=="capacity")return 100.0;return pR.GetOxygenLevel()*100.0;}var mL=x as IMyCargoContainer;if(mL!=null){var gj=mL.GetInventory();
if(gj!=null){if(b=="mass")return(double)gj.CurrentMass;if(b=="max"||b=="capacity")return(double)gj.MaxVolume;return(double)gj.CurrentVolume;}return 0.0;}var kp=x as IMyPowerProducer;
if(kp!=null){if(b=="max"||b=="capacity")return kp.MaxOutput;return kp.CurrentOutput;}var m7=x as IMyDoor;if(m7!=null){if(b=="max")return 1.0;return(double)m7.OpenRatio*100.0;
}var kZ=x as IMyThrust;if(kZ!=null){if(b=="max"||b=="capacity")return kZ.MaxEffectiveThrust;return kZ.CurrentThrust;}var b5=x as IMyMotorSuspension;
if(b5!=null){if(b=="brake")return b5.Brake ? 100.0 : 0.0;if(b=="height")return b5.Height;if(b=="strength")return b5.Strength;if(b=="power")return b5.Power;
if(b=="steer")return b5.SteeringOverride*100.0;if(b=="propulsion")return b5.PropulsionOverride*100.0;return b5.Friction;}var mX=x as IMyShipConnector;
if(mX!=null){return mX.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var km=x as IMyProductionBlock;if(km!=null){if(b=="producing"||b=="active"||b=="running"||b=="state")return km.IsProducing ? 100.0 : 0.0;
if(b=="queue")return km.IsQueueEmpty ? 0.0 : 1.0;}var io=x as IMyAssembler;if(io!=null){if(b=="disassembling")return io.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(b=="repeating"||b=="loop")return io.Repeating ? 100.0 : 0.0;}var jf=x as IMyGasGenerator;if(jf!=null){if(b=="autorefill")return jf.AutoRefill ? 100.0 : 0.0;
if(b=="active")return jf.Enabled ? 100.0 : 0.0;}var hb=x as IMyLargeTurretBase;if(hb!=null){if(b=="shooting")return hb.IsShooting ? 100.0 : 0.0;
if(b=="target"||b=="hastarget")return hb.HasTarget ? 100.0 : 0.0;if(b=="range")return hb.Range;}var bZ=x as IMyProjector;if(bZ!=null){if(b=="built")return bZ.TotalBlocks-bZ.RemainingBlocks;
if(b=="remaining")return bZ.RemainingBlocks;if(b=="total")return bZ.TotalBlocks;if(b=="projecting")return bZ.IsProjecting ? 100.0 : 0.0;if(b=="armor")return bZ.RemainingArmorBlocks;
if(b=="buildable")return bZ.BuildableBlocksCount;}var k1=x as IMyShipToolBase;if(k1!=null){if(b=="working"||b=="active"||b=="activated")return k1.IsActivated ? 100.0 : 0.0;
return k1.Enabled ? 100.0 : 0.0;}var c8=x as IMyGyro;if(c8!=null){if(b=="override")return c8.GyroOverride ? 100.0 : 0.0;if(b=="pitch")return c8.Pitch;
if(b=="yaw")return c8.Yaw;if(b=="roll")return c8.Roll;if(b=="power")return c8.GyroPower*100.0;}var nI=x as IMyGravityGeneratorBase;if(nI!=null){if(b=="g"||b=="accel")return nI.GravityAcceleration;
}var nG=x as IMyLandingGear;if(nG!=null){if(b=="locked")return nG.IsLocked ? 100.0 : 0.0;}var mg=x as IMyRadioAntenna;if(mg!=null){if(b=="radius"||b=="range")return mg.Radius;
}var mt=x as IMyBeacon;if(mt!=null){if(b=="radius"||b=="range")return mt.Radius;}var jP=x as IMyLightingBlock;if(jP!=null){if(b=="intensity")return jP.Intensity;
if(b=="radius")return jP.Radius;}var gF=x as IMyPistonBase;if(gF!=null){if(b=="pos"||b=="position"||b=="extension")return gF.CurrentPosition;
if(b=="velocity")return gF.Velocity;if(b=="max")return gF.HighestPosition;}var eR=x as IMyMotorStator;if(eR!=null){if(b=="angle"||b=="deg")return eR.Angle*180.0/Math.PI;
if(b=="rad")return eR.Angle;if(b=="velocity"||b=="rpm")return eR.TargetVelocityRPM;if(b=="locked")return eR.RotorLock ? 100.0 : 0.0;}var la=x as IMyWarhead;
if(la!=null){if(b=="armed")return la.IsArmed ? 100.0 : 0.0;if(b=="countdown")return la.DetonationTime;}var o7=x as IMySensorBlock;if(o7!=null){
if(b=="detected")return!o7.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var iL=x as IMyCameraBlock;if(iL!=null){if(b=="range")return iL.AvailableScanRange;
if(b=="active")return iL.IsActive ? 100.0 : 0.0;}var mR=x as IMyParachute;if(mR!=null){if(b=="open"||b=="openratio")return(double)mR.OpenRatio*100.0;
}var dv=x as IMyTurretControlBlock;if(dv!=null){if(b=="target"||b=="hastarget")return dv.HasTarget ? 100.0 : 0.0;if(b=="azimuth")return dv.AzimuthRotor!=null ? dv.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(b=="elevation")return dv.ElevationRotor!=null ? dv.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var nx=x as IMyFunctionalBlock;if(nx!=null){return nx.Enabled ? 100.0 : 0.0;
}return 0.0;}double bo(string ec,string nn){bk bR;d0.TryGetValue(ec,out bR);if(bR!=null&&bR.bD==a4.dI){
string sJ= !string.IsNullOrEmpty(bR.bl)? bR.bl : ec;double m_;if(aJ.TryGetValue(sJ,out m_))return m_;return bR.fe;
}double nd;if(aJ.TryGetValue(ec,out nd))return nd;List<IMyTerminalBlock>ed;if(!bK.TryGetValue(ec,out ed)||ed.Count==0)throw new InvalidOperationException($"HELM: bind '{ec}' resolved to zero blocks — check the name/typo");
if(ed.Count==1)return lP(ed[0],nn);var pP=new List<double>();foreach(var rx in ed)pP.Add(lP(rx,nn));return p3.p2(bR!=null ? bR.dy : aH.hi,pP);
}double hT(string rK){return bo(rK,"percent");}static string a7(double pL,bm de){if(de==null||(string.IsNullOrEmpty(de.dY)&&string.IsNullOrEmpty(de.dJ)&&de.I=="percent")){
return ly(pL);}return H.lx(pL,de.dY,de.dJ);}bool q_(z b0){IMyTextSurface eV;if(!bL.TryGetValue(b0,out eV))return false;
if(b0.b7!=null)return qT(b0,eV);if(b0.hI!=null)return qZ(b0,eV);if(b0.dD.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return q0(b0,eV);}return q1(b0,eV);}Color av(bm n4,Color r4){return n4.hZ!=null ? F(n4.hZ): r4;
}Color fh(string[]ds,double to){if(ds.Length==1)return F(ds[0]);double oM=Math.Max(0.0,Math.Min(100.0,to))/100.0*(ds.Length-1);int jr=Math.Min((int)oM,ds.Length-2);
float g1=(float)(oM-jr);Color bO=F(ds[jr]),fK=F(ds[jr+1]);return new Color((int)(bO.R+(fK.R-bO.R)*g1),(int)(bO.G+(fK.G-bO.G)*g1),(int)(bO.B+(fK.B-bO.B)*g1),(int)(bO.A+(fK.A-bO.A)*g1));
}static Color[]e3(aC rP,string[]uh,string r6,float pY,float p_,float k7,float jl){int ea=16;bool nP=r6=="horizontal";var iR=new Color[ea];
for(int cw=0;cw<ea;cw++){o jc=H.lz(uh,100.0*cw/(ea-1));iR[cw]=new Color(jc.bG,jc.bC,jc.bw);float mI=k7/ea,mA=jl/ea;u(rP,nP ? new Vector2(pY+mI*(cw+0.5f),p_+jl/2f): new Vector2(pY+k7/2f,p_+mA*(cw+0.5f)),nP ? new Vector2(mI+1f,jl): new Vector2(k7,mA+1f),iR[cw]);
}return iR;}static void u(aC rQ,Vector2 tA,Vector2 t7,Color rU,float tS=0f,string r5="SquareSimple"){rQ.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=r5,Position=tA,Size=t7,Color=rU,RotationOrScale=tS}
);}void qa(z k,IMyTextSurface kR,aC g){g.f.Clear();g.dU.Clear();g.bF.Clear();Vector2 O=kR.SurfaceSize;
Vector2 ut=kR.TextureSize;Vector2 ay=(ut-O)/2f;g.hM=ay;float gx=Math.Max(4f,O.X*0.02f);
float cA=Math.Max(4f,O.Y*0.02f);if(k.cj=="compact"||(k.cj=="auto"&&O.Y<=260f&&O.Y>150f)){gx=4f;cA=4f;
}else if(k.cj=="corner"||(k.cj=="auto"&&O.Y<=150f)){gx=2f;cA=2f;}float e_=O.X-(gx*2f);Color cZ=new Color(10,12,16);
bool m9=true;if(!string.IsNullOrEmpty(k.bx)){var eo=o.dR(k.bx);if(eo.fk)m9=false;else cZ=new Color(eo.bG,eo.bC,eo.bw,eo.hg);
}g.ca= -1;g.e5=false;g.e4=F(k.lf ?? "#5b0f0f");g.dC.Clear();if(k.bA!=null&&k.bA.Length>1){
g.ca=g.f.Count;g.e8=e3(g,k.bA,k.dB,ay.X+2f,ay.Y+2f,O.X-4f,O.Y-4f);
}else if(m9||k.aR!=null||k.cN!=null){if(k.aR!=null)cZ=F(k.aR.First(tF=>tF.cP).ce);g.ca=g.f.Count;
g.e8=new[]{cZ};u(g,ay+(O/2f),O-new Vector2(4f,4f),cZ);if(k.aR!=null||k.cN!=null)g.dC.Add(new e9{
fj=g.ca,hj=cZ,fd=cZ,fr=k.aR,hn=k.hk,hx=k.hl,fg=k.cN,hy=k.hz,hA=k.hB,lE=true}
);}if(!string.IsNullOrEmpty(k.hm))u(g,ay+(O/2f),O-new Vector2(4f,4f),F(k.lh ?? "255,255,255,24"),0f,k.hm);
if(!k.dD.Equals("none",StringComparison.OrdinalIgnoreCase)){Color mC=k.az!=null&&k.az.Length>0 ? F(k.az[0]): F(k.fc ?? "40, 60, 85");
Color rO=k.az!=null&&k.az.Length>1 ? F(k.az[k.az.Length-1]): mC;
float mD=ay.X+(O.X-e_)/2f;if(k.az!=null&&k.az.Length>1&& !k.hD){e3(g,k.az,"horizontal",mD,ay.Y+cA*0.5f-1f,e_,2f);
e3(g,k.az,"horizontal",mD,ay.Y+O.Y-(cA*0.5f)-1f,e_,2f);}else{u(g,new Vector2(ay.X+O.X/2f,ay.Y+cA*0.5f),new Vector2(e_,2f),mC);
u(g,new Vector2(ay.X+O.X/2f,ay.Y+O.Y-(cA*0.5f)),new Vector2(e_,2f),rO);}}var kb=k.ai;
if(kb.Count==0){kb=new List<aB>{new aB{cS="main",dF=0,dT=0,dV=k.dK,dW=k.dL,am=k.fn}};}float nK=8f;
foreach(var s in kb){var gw=H.qd(s.dF,s.dT,s.dV,s.dW,k.dK,k.dL,O.X,O.Y,gx,cA,nK,nK);
float aM=ay.X+gw.l2;float cz=ay.Y+gw.l3;float a0=gw.l1;float dh=gw.lB;g.bF.Add(new hP{lJ=s.cS,hU=g.f.Count,lO=new Vector2(aM,cz),lS=new Vector2(a0,dh)}
);bool oU=s.aR!=null||s.cN!=null;if(s.bA!=null&&s.bA.Length>1){e3(g,s.bA,s.dB!="vertical" ? s.dB : k.dB,aM,cz,a0,dh);
}else{Color bY;if(s.aR!=null)bY=F(s.aR.First(tG=>tG.cP).ce);else if(!string.IsNullOrEmpty(s.cL))bY=F(s.cL);
else if(!string.IsNullOrEmpty(k.hO))bY=F(k.hO);else bY=Color.Transparent;if(bY.A>0||oU){if(oU)g.dC.Add(new e9{fj=g.f.Count,hj=bY,fd=bY,fr=s.aR,hn=s.hk,hx=s.hl,fg=s.cN,hy=s.hz,hA=s.hB}
);u(g,new Vector2(aM+a0/2f,cz+dh/2f),new Vector2(a0,dh),bY);}}string f7=s.lu ?? k.lv ?? "Monospace";float rG=H.qt(k.cj,O.X,O.Y,k.cO);
float aK=s.lw ?? rG;float ep=0f;if(!string.IsNullOrEmpty(s.cm)){ep=20f*aK;var ig=TextAlignment.LEFT;float k_=aM+4f;
string l8=s.hX ?? k.lY ?? "left";if(l8.Equals("center",StringComparison.OrdinalIgnoreCase)){ig=TextAlignment.CENTER;k_=aM+(a0/2f);
}else if(l8.Equals("right",StringComparison.OrdinalIgnoreCase)){ig=TextAlignment.RIGHT;k_=aM+a0-4f;}string pv=s.hY ?? k.lZ;
Color uu= !string.IsNullOrEmpty(pv)? F(pv): F(s.hr ?? k.fc ?? "white");var pA=MySprite.CreateText(s.cm,f7,uu,aK*0.9f,ig);
pA.Position=new Vector2(k_,cz+2f);g.f.Add(pA);string[]iv=s.hs ?? k.az;Color r8=iv!=null&&iv.Length>0 ? F(iv[0]): F(s.hr ?? k.fc ?? "40, 60, 85");
u(g,new Vector2(aM+a0/2f,cz+ep-2f),new Vector2(a0-8f,1f),r8);}if(s.hN=="vertical"||s.am.Any(tw=>tw.h==e.fu)){
qb(g,kR,s,f7,aK,aM,cz,a0,dh,ep);continue;}int uT=Math.Max(1,s.am.Count);float tm=dh-ep-8f;
float pV=0f,ma=0f;foreach(var tx in s.am)pV+=1<<tx.fs;float ux=tm/Math.Max(1f,pV);float gh=6f;for(int js=0;js<s.am.Count;
js++){var p=s.am[js];var V=new ch{fm=p};float gN=ux*(1<<p.fs);float tT=cz+ep+4f+ma;ma+=gN;float M=tT+(gN/2f);
float px=M-(11f*aK);V.cQ=g.f.Count;V.lA=p.Z!=null&&(p.Z.Contains("{val}")||p.Z.Contains("{percent}"));
var n8=MySprite.CreateText(p.Z,f7,p.dO!=null ? F(p.dO): Color.White,aK,TextAlignment.LEFT);n8.Position=new Vector2(aM+gh,px);
g.f.Add(n8);float sX=Math.Max(24f*aK,(p.Z.Length+1)*12f*aK);float uG=(!string.IsNullOrEmpty(p.dY)|| !string.IsNullOrEmpty(p.dJ))? 64f*aK : 52f*aK;
if(p.h!=e.hW){float L=aM+gh+sX;float rF=aM+a0-gh-uG;float J=Math.Max(16f,rF-L);
float Q=Math.Max(p.h==e.ck ? 8f : 4f,Math.Min((p.h==e.ck ? 20f : 16f)*aK,gN*(p.h==e.ck ? 0.55f : 0.45f)));
if(p.fs>0)Q=gN*0.7f;Color pE=av(p,new Color(25,30,38));V.b9=L;V.bj=J;V.a3=Q;V.b8=M;
if(p.h==e.dA){u(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),pE);V.W=g.f.Count;
u(g,new Vector2(L,M),new Vector2(0f,Q),F(p.au));}else if(p.h==e.dQ){int kC=10;float nz=2f;
float kD=Math.Max(2f,(J-(nz*(kC-1)))/kC);for(int kw=0;kw<kC;kw++){V.K.Add(g.f.Count);u(g,new Vector2(L+kw*(kD+nz)+(kD/2f),M),new Vector2(kD,Q),av(p,new Color(22,26,32)));
}}else if(p.h==e.ck){u(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),av(p,new Color(20,24,30)));int kl=8;
float nA=2f;float iP=Math.Max(2f,(J-(nA*(kl-1)))/kl);for(int j7=0;j7<kl;j7++){V.K.Add(g.f.Count);u(g,new Vector2(L+j7*(iP+nA)+(iP/2f),M),new Vector2(iP,2f),F(p.au));
}}else if(p.h==e.dM){u(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),av(p,new Color(20,24,30)));int kN=10;
float nB=2f;float kO=Math.Max(2f,(J-(nB*(kN-1)))/kN);for(int kx=0;kx<kN;kx++){V.K.Add(g.f.Count);
u(g,new Vector2(L+kx*(kO+nB)+(kO/2f),M),new Vector2(kO,Q),av(p,new Color(24,28,36)),0.523599f);}}else if(p.h==e.dE){
int il=10;float nC=2f;float im=Math.Max(2f,(J-(nC*(il-1)))/il);for(int ky=0;ky<il;ky++){V.K.Add(g.f.Count);
u(g,new Vector2(L+ky*(im+nC)+(im/2f),M),new Vector2(im,Q),av(p,new Color(24,28,36)),1.570796f,"Triangle");}}else if(p.h==e.dH){
int iV=12;float nD=2f;float iX=Math.Max(2f,(J-(nD*(iV-1)))/iV);float m8=Math.Min(iX,Q);for(int kz=0;kz<iV;kz++){V.K.Add(g.f.Count);
u(g,new Vector2(L+kz*(iX+nD)+(iX/2f),M),new Vector2(m8,m8),av(p,new Color(24,28,36)),0f,"Circle");}}else if(p.h==e.cM){
u(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),pE);V.W=g.f.Count;u(g,new Vector2(L+(J/2f),M),new Vector2(0f,Q),F(p.au));
u(g,new Vector2(L+(J/2f),M),new Vector2(2f,Q+4f),new Color(80,110,150));}else if(p.h==e.cn){u(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),av(p,new Color(20,24,30)));
V.W=g.f.Count;float jI=Math.Max(4f,Q);u(g,new Vector2(L+jI*0.5f,M),new Vector2(jI,jI),new Color(40,50,65),0f,"Circle");
}else if(p.h==e.cd){float c_=Math.Max(6f,Math.Min(Q,14f*aK));u(g,new Vector2(L+c_*0.5f,M),new Vector2(c_,c_),new Color(28,34,46));
V.W=g.f.Count;u(g,new Vector2(L+c_*0.5f,M),new Vector2(c_*0.6f,c_*0.6f),new Color(40,50,65));}else if(p.h==e.cl){
u(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),av(p,new Color(18,22,30)));V.W=g.f.Count;float ki=Math.Max(4f,Q*0.7f);
u(g,new Vector2(L+ki+2f,M),new Vector2(ki,ki),new Color(34,197,94),0f,"Circle");}else if(p.h==e.cK){int kh=8;
float nE=2f;float kj=Math.Max(2f,(J-(nE*(kh-1)))/kh);for(int j8=0;j8<kh;j8++){V.K.Add(g.f.Count);u(g,new Vector2(L+j8*(kj+nE)+(kj/2f),M),new Vector2(kj,Q),av(p,new Color(24,28,36)));
}}else if(p.h==e.cf){u(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),av(p,new Color(20,24,30)));V.W=g.f.Count;
float nS=Math.Max(4f,Q);u(g,new Vector2(L+(J/2f),M),new Vector2(nS,nS),F(p.au),0f,"Triangle");}}var pJ=MySprite.CreateText("---",f7,Color.White,aK,TextAlignment.RIGHT);
pJ.Position=new Vector2(aM+a0-gh,px);V.aX=g.f.Count;g.f.Add(pJ);g.dU.Add(V);}}g.dP=O;
g.dN=true;}const string re="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bI=8;void qb(aC aa,IMyTextSurface uk,aB ka,string nu,float em,float tj,float j9,float tk,float oy,float sA){
int s9=Math.Max(1,ka.am.Count);float mU=(tk-12f)/s9;float hc=22f*em;float bh=j9+sA+4f+hc;float aP=Math.Max(20f,j9+oy-4f-hc-bh);
float r1=uk.MeasureStringInPixels(new StringBuilder("M"),nu,em*0.9f).X;for(int gg=0;gg<ka.am.Count;gg++){var cx=ka.am[gg];float bc=tj+6f+mU*(gg+0.5f);
float cI=mU*0.7f;var be=new ch{fm=cx,l_=true,b9=bc,bj=cI,a3=aP,b8=bh+aP/2f,lo=r1};be.cQ=aa.f.Count;
a2(aa.f,cx.Z,cx.dO!=null ? F(cx.dO): Color.White,bc,j9+oy-4f-hc,em);be.aX=aa.f.Count;
a2(aa.f,"---",Color.White,bc,bh-hc,em);var n5=aa.f[be.cQ];n5.Alignment=TextAlignment.CENTER;aa.f[be.cQ]=n5;
var pI=aa.f[be.aX];pI.Alignment=TextAlignment.CENTER;aa.f[be.aX]=pI;u(aa,new Vector2(bc,bh+aP/2f),new Vector2(cI,aP),av(cx,new Color(25,30,38)));
be.W=aa.f.Count;if(cx.h==e.fu){for(int mo=0;mo<bI;mo++){be.K.Add(aa.f.Count);u(aa,new Vector2(bc,bh+aP),new Vector2(cI-6f,0f),Color.White);
}for(int k8=0;k8<4;k8++){be.K.Add(aa.f.Count);var pq=MySprite.CreateText("",nu,k8==0 ? new Color(255,255,255,170): new Color(255,255,255,110),em*0.9f,TextAlignment.CENTER);
pq.Position=new Vector2(bc,bh+aP);aa.f.Add(pq);}Color iZ=new Color(90,110,135);u(aa,new Vector2(bc-cI/2f,bh+aP/2f),new Vector2(3f,aP),iZ);u(aa,new Vector2(bc+cI/2f,bh+aP/2f),new Vector2(3f,aP),iZ);
u(aa,new Vector2(bc,bh+aP),new Vector2(cI+3f,3f),iZ);}else{u(aa,new Vector2(bc,bh+aP),new Vector2(cI,0f),F(cx.au));}aa.dU.Add(be);
}}bool rd(aC ba,ch y,double e1,double oN,Color fU,bool pN){var a_=y.fm;float mv=y.E;if(y.E<0f)y.E=(float)e1;
y.E=Math.Abs(y.E-(float)e1)>0.05f ? y.E+((float)e1-y.E)*0.35f :(float)e1;float cy=Math.Max(0f,Math.Min(1f,y.E/100f))*y.a3;
float fR=y.b8+y.a3/2f;float fY=y.b9;if(a_.h!=e.fu){var f2=ba.f[y.W];f2.Size=new Vector2(y.bj,cy);
f2.Position=new Vector2(fY,fR-cy/2f);f2.Color=fU;ba.f[y.W]=f2;if(pN||y.E!=mv)aI(ba,y,a7(oN,a_),fU);
return pN||y.E!=mv;}float ix=y.a3/bI;string eM=a_.bB!=null ? H.dS(a_.bB,a_.dG!=null ? bo(a_.dG,a_.ht): e1): null;
Color s4=eM!=null ? F(eM): fU;string[]pi=eM!=null ?(eM.Contains("/")? eM.Split('/'): null): a_.cg&&a_.a8.Length>1 ? a_.a8 : null;
for(int cs=0;cs<bI;cs++){float pS=Math.Max(0f,Math.Min(ix,cy-cs*ix));var gG=ba.f[y.K[cs]];gG.Size=new Vector2(y.bj-6f,pS);
gG.Position=new Vector2(fY,fR-cs*ix-pS/2f);gG.Color=pi!=null ? fh(pi,100.0*(cs+0.5)/bI): s4;ba.f[y.K[cs]]=gG;}
int rS=Math.Max(1,(int)((y.bj-8f)/y.lo));bool t4=cy>6f;var lb=ba.f[y.K[bI]];lb.Data=t4 ? re.Substring((int)(_*5.0)%6,Math.Min(rS,100)): "";
lb.Position=new Vector2(fY,fR-cy-12f);ba.f[y.K[bI]]=lb;for(int bW=0;bW<3;bW++){var iG=ba.f[y.K[bI+1+bW]];
double ts=(_*(0.35+0.1*bW)+bW*0.37)%1.0;iG.Data=cy>24f ?(bW==1 ? "O" : "o"): "";iG.Position=new Vector2(fY+(bW-1)*y.bj*0.26f,fR-(float)ts*(cy-6f)-10f);
ba.f[y.K[bI+1+bW]]=iG;}aI(ba,y,a7(oN,a_),fU);return true;}static void aI(aC mF,ch pc,string us,Color rV){
var k5=mF.f[pc.aX];k5.Data=us;k5.Color=rV;mF.f[pc.aX]=k5;}static void lR(aC mG,ch pd,Color rW){
var nh=mG.f[pd.W];nh.Color=rW;mG.f[pd.W]=nh;}bool q1(z bv,IMyTextSurface kS){aC j;
if(!bM.TryGetValue(bv,out j)){j=new aC();bM[bv]=j;}bool oQ= !j.dN||j.dP!=kS.SurfaceSize;
if(oQ){qa(bv,kS,j);}bool N=oQ;bool fG=(_%bv.hh)<bv.hh/2.0;double cv= -1.0,mY;if(fx.TryGetValue(bv,out mY)){
cv=(_-mY)/bv.hp;if(cv>=1.0){fx.Remove(bv);cv=1.0;}}bool k9=cp&&fG&&j.e4.A>0;if(j.ca>=0&&k9!=j.e5){
j.e5=k9;for(int er=0;er<j.e8.Length;er++){var mw=j.f[j.ca+er];mw.Color=k9 ? j.e4 : j.e8[er];j.f[j.ca+er]=mw;
}N=true;foreach(var rH in j.dC)rH.hC=true;}foreach(var aj in j.dC){if(aj.lE&&j.e5)continue;Color eb=aj.hj;try{if(aj.fr!=null){
double mH=bo(aj.hn,aj.hx);string gL=H.dS(aj.fr,mH);eb=gL!=null&&gL.Contains("/")? fh(gL.Split('/'),mH): F(gL);
}if(aj.fg!=null&&fG&&H.dS(aj.fg,bo(aj.hy,aj.hA))!=null)eb=j.e4;}catch{}if(eb!=aj.fd||aj.hC){
aj.fd=eb;aj.hC=false;var mE=j.f[aj.fj];mE.Color=eb;j.f[aj.fj]=mE;N=true;}}for(int jt=0;jt<j.dU.Count;jt++){var d=j.dU[jt];
var i=d.fm;double D;bool nM=false;double jW=i.fo;try{D=bo(i.aS,i.I);if(i.cR!=null)jW=bo(i.cR,i.hH);
}catch{nM=true;D=0.0;}if(nM){if(d.fl!= -99999.0||(d.aX>=0&&j.f[d.aX].Data!="ERR")){N=true;d.fl= -99999.0;
if(d.aX>=0){var kA=j.f[d.aX];kA.Data="ERR";kA.Color=Color.Red;j.f[d.aX]=kA;}}continue;}double a1=D;
if(jW>0)D=Math.Min(100.0,a1/jW*100.0);bool pn=cv>=0.0&&i.h!=e.hW&&i.h!=e.cn&&i.h!=e.cd&&i.h!=e.cl&&i.h!=e.cf;
if(pn)D=cv<0.5 ? cv*200.0 : 100.0+(D-100.0)*(cv-0.5)*2.0;double fX=i.dG!=null ? bo(i.dG,i.ht): D;string fV=i.bB!=null ? H.dS(i.bB,fX): i.cg ? H.fi(i.a8,fX): i.au;
Color U=F(fV);bool bq=(U==Color.Red);if(fV!=null&&fV.Contains("/"))U=fh(fV.Split('/'),fX);if(i.bB==null&&i.cg&& !bv.hD&&i.a8.Length>1)U=fh(i.a8,fX);
if(bq&&fG){U=new Color(255,75,75);N=true;}bool aQ=pn||Math.Abs(a1-d.fl)>0.1;if(aQ)d.fl=a1;
if(d.l_){if(rd(j,d,D,a1,U,aQ))N=true;continue;}if(i.h==e.dA){double no=(i.hF||i.I=="percent"||i.I=="pct")? D : hT(i.aS);
if(d.E<0f)d.E=(float)no;float g4=(float)no;if(Math.Abs(d.E-g4)>0.05f){d.E+=(g4-d.E)*0.35f;
N=true;}else if(d.E!=g4){d.E=g4;N=true;}if(aQ)N=true;if(N){float tp=(float)Math.Max(0.0,Math.Min(1.0,d.E/100.0));
float nq=d.bj*tp;var f4=j.f[d.W];f4.Size=new Vector2(nq,d.a3);f4.Position=new Vector2(d.b9+(nq/2f),d.b8);
f4.Color=U;j.f[d.W]=f4;aI(j,d,a7(a1,i),U);}}else if(i.h==e.dQ||i.h==e.dM||i.h==e.dE||i.h==e.dH){
if(aQ||bq){N=true;double sh=(i.hF||i.I=="percent"||i.I=="pct")? D : hT(i.aS);int o5=d.K.Count;
for(int eT=0;eT<o5;eT++){bool s5=sh>=((eT+0.4)/(double)o5*100.0);Color t2=s5 ? U : av(i,new Color(24,28,36));var o6=j.f[d.K[eT]];
o6.Color=t2;j.f[d.K[eT]]=o6;}aI(j,d,a7(a1,i),U);}}else if(i.h==e.ck){
if(aQ||bq){N=true;for(int gb=0;gb<7;gb++)d.ft[gb]=d.ft[gb+1];d.ft[7]=(float)D;int tz=d.K.Count;
for(int eE=0;eE<tz;eE++){float sz=d.ft[eE];float sy=Math.Max(0.06f,Math.Min(1f,sz/100f));float mT=d.a3*sy;float rX=d.b8+(d.a3/2f)-(mT/2f);
var c4=j.f[d.K[eE]];c4.Size=new Vector2(c4.Size.Value.X,mT);c4.Position=new Vector2(c4.Position.Value.X,rX);
c4.Color=U;j.f[d.K[eE]]=c4;}aI(j,d,a7(a1,i),U);}}else if(i.h==e.cM){
if(d.E<0f)d.E=(float)D;float g5=(float)D;if(Math.Abs(d.E-g5)>0.05f){d.E+=(g5-d.E)*0.35f;
N=true;}else if(d.E!=g5){d.E=g5;N=true;}if(N||aQ){float nL=d.bj*0.5f;float mO=d.b9+nL;
float m3=(float)Math.Max(-1.0,Math.Min(1.0,(d.E-50.0)/50.0));float ja=Math.Abs(m3)*nL;float si=(m3>=0)?(mO+ja*0.5f):(mO-ja*0.5f);
var f5=j.f[d.W];f5.Size=new Vector2(ja,d.a3);f5.Position=new Vector2(si,d.b8);f5.Color=U;
j.f[d.W]=f5;aI(j,d,a7(a1,i),U);}}else if(i.h==e.cn){if(aQ||bq){
N=true;bool jB=D>=50.0;float n0=Math.Max(4f,d.a3);float sM=jB ?(d.b9+d.bj-n0*0.5f):(d.b9+n0*0.5f);
Color n_=jB ?(U!=Color.White ? U : new Color(34,197,94)): new Color(60,70,85);var i8=j.f[d.W];i8.Position=new Vector2(sM,d.b8);
i8.Color=n_;j.f[d.W]=i8;aI(j,d,jB ? "ON" : "OFF",n_);}}else if(i.h==e.cd){if(aQ||bq){
N=true;bool jA=D>=50.0;Color mP=jA ?(U!=Color.White ? U : new Color(56,189,248)): new Color(30,36,48);lR(j,d,mP);
aI(j,d,jA ? "ACTIVE" : "READY",jA ? mP : new Color(148,163,184));}}else if(i.h==e.cl){if(aQ||bq){
N=true;string uf=D>=75.0 ? "ONLINE" :(D>=25.0 ? "STBY" : "OFFLINE");Color ph=D>=75.0 ? new Color(34,197,94):(D>=25.0 ? new Color(234,179,8): new Color(239,68,68));
lR(j,d,ph);aI(j,d,uf,ph);}}else if(i.h==e.cK){if(aQ||bq){N=true;int oH=d.K.Count;
for(int eF=0;eF<oH;eF++){bool s6=D>=((eF+0.5)/(double)oH*100.0);Color tu=s6 ? U : av(i,new Color(24,28,36));var oI=j.f[d.K[eF]];
oI.Color=tu;j.f[d.K[eF]]=oI;}aI(j,d,a7(a1,i),U);}}else if(i.h==e.cf){
N=true;float gM;Color el;string f6;bool sE=(i.I=="producing"||i.I=="active"||i.I=="running"||i.I=="state"||(i.I!="percent"&&(D<=1.0||D==100.0)));
if(sE){if(D>0.0){d.E=(d.E+0.3927f)%6.28318f;gM=d.E;el=new Color(34,197,94);f6="RUN";}else{gM=0f;
el=new Color(148,163,184);f6="IDLE";}}else{gM=(D>55.0)? 0f :((D<45.0)? 3.14159f : 1.57079f);el=(D>55.0)? new Color(34,197,94):((D<45.0)? new Color(239,68,68): new Color(148,163,184));
f6=(D>55.0)? "CHG" :((D<45.0)? "DSC" : "STBY");}var i9=j.f[d.W];i9.RotationOrScale=gM;i9.Color=el;
j.f[d.W]=i9;aI(j,d,f6,el);}else{string f1;bk g7;d0.TryGetValue(i.aS,out g7);cU.TryGetValue(lF(i.I)? i.aS+"."+i.I :(g7!=null&&g7.bl!=null ? g7.bl : i.aS),out f1);
if(aQ||bq||f1!=d.lG){d.lG=f1;N=true;Color k2=U;if(k2==Color.Red&&fG)k2=new Color(255,75,75);aI(j,d,f1 ?? a7(a1,i),k2);
}}if(d.lA&&(aQ||bq)){N=true;var oa=j.f[d.cQ];string tq=ly(D);string tQ=a7(a1,i);
oa.Data=i.Z.Replace("{percent}",tq).Replace("{val}",tQ);j.f[d.cQ]=oa;}}if(N){j.dZ++;using(var so=kS.DrawFrame()){
so.AddRange(j.f);}}return N;}bool q0(z b1,IMyTextSurface kT){kT.FontSize=b1.cO;var fW=new List<KeyValuePair<string,string>>();
foreach(var m in b1.fn){double P=0.0;string bg;string bb=null;double jX=m.fo;try{P=bo(m.aS,m.I);if(m.cR!=null)jX=bo(m.cR,m.hH);
}catch(Exception uS){fW.Add(new KeyValuePair<string,string>($"{m.Z}: [UNRESOLVED]","red"));continue;}double oO=P;if(jX>0)P=Math.Min(100.0,oO/jX*100.0);
string pK=a7(oO,m);if(m.h==e.dA){double np=(m.hF||m.I=="percent"||m.I=="pct")? P : hT(m.aS);
bg=H.qU(m.Z,np,10,pK);bb=m.cg ? H.fi(m.a8,np): m.au;}else if(m.h==e.cM){
bg=H.qS(m.Z,P,10);bb=m.cg ? H.fi(m.a8,P): m.au;}else if(m.h==e.cn){
bg=H.q3(m.Z,P);bb=m.au ??(P>=50.0 ? "green" : "gray");}else if(m.h==e.cd){bg=H.qX(m.Z,P);
bb=m.au ??(P>=50.0 ? "cyan" : "gray");}else if(m.h==e.cl){bg=H.q2(m.Z,P);bb=m.au ??(P>=75.0 ? "lime" :(P>=25.0 ? "yellow" : "red"));
}else if(m.h==e.cK){bg=H.qR(m.Z,P,8);bb=m.cg ? H.fi(m.a8,P):(m.au ?? "yellow");
}else if(m.h==e.cf){bool sF=(m.I=="producing"||m.I=="active"||m.I=="running"||m.I=="state"||(m.I!="percent"&&(P<=1.0||P==100.0)));
if(sF){char[]ud=new[]{'|','/','-','\\'};char t_=ud[Math.Abs(d6)%4];bg=P>0 ? $"{m.Z} [{t_}] RUN" : $"{m.Z} [==] IDLE";
bb=P>0 ? "green" : "gray";}else{bg=H.qY(m.Z,P,d6);bb=m.au ??(P>55.0 ? "green" :(P<45.0 ? "red" : "cyan"));
}}else{bg=$"{m.Z}: {pK}";bb=m.bB!=null ? H.dS(m.bB,P): null;}fW.Add(new KeyValuePair<string,string>(bg,bb));
}var fQ=H.qW(new List<string>(fW.ConvertAll(un=>un.Key)),dx: b1.l0);string o9=string.Join("\n",fQ);
string oK;if(cq.TryGetValue(b1,out oK)&&oK==o9)return false;cq[b1]=o9;using(var sp=kT.DrawFrame()){
float p0=4f;float oc=p1*b1.cO;try{var of=kT.MeasureStringInPixels(new StringBuilder("A"),"Monospace",b1.cO);
if(of.Y>0)oc=of.Y;}catch{}for(int c9=0;c9<fQ.Count;c9++){string mW=null;if(c9>0&&c9<fQ.Count-1)mW=fW[c9-1].Value;
var pg=MySprite.CreateText(fQ[c9],"Monospace",F(mW),b1.cO,TextAlignment.LEFT);pg.Position=new Vector2(4f,p0);sp.Add(pg);
p0+=oc;}}return true;}Color F(string gu){if(string.IsNullOrEmpty(gu))return Color.White;Color ey;if(l4.TryGetValue(gu,out ey))return ey;
o ee=o.dR(gu);ey=ee.fk ? Color.Transparent : new Color(ee.bG,ee.bC,ee.bw,ee.hg);l4[gu]=ey;return ey;}public enum a4{p5,hE,dI}
public enum aH{qC,hi,lW,lI,lH}public class bk{public string ae;public a4 bD;public List<string>aW=new List<string>();public string lX{
get{return aW.Count>0 ? aW[0]: "";}set{if(aW.Count==0)aW.Add(value);else aW[0]=value;}}public aH dy=aH.qC;
public double fe=0.0;public string hu;public string bl;public bool e7;}public enum e{hW,dA,dQ,ck,dM,dE,dH,cM,cn,cd,cl,cK,cf,fu}
public class aT{public string hw;public string ce;public bool cP;}public class bm{public e h;public string Z;public string aS;
public string I;public bool cg;public string[]a8;public string au;public List<aT>bB;public string lp="right";
public string dY;public string dJ;public int fs;public string dG,ht;public string dO,hZ;public double fo;public string cR,hH;
public bool hF{get{return fo>0||cR!=null;}}}public struct o{public byte bG,bC,bw,hg;public bool fk;public o(byte tH,byte sr,byte ry,byte rg=255){
bG=tH;bC=sr;bw=ry;hg=rg;fk=false;}public static readonly o rc=new o(0,0,0,0){fk=true};public static readonly o fv=new o(255,255,255);
public static readonly o p4=new o(0,0,0);public static readonly o qP=new o(255,0,0);public static readonly o qv=new o(0,255,0);
public static readonly o qz=new o(50,205,50);public static readonly o rf=new o(255,255,0);public static readonly o qD=new o(255,140,0);
public static readonly o qg=new o(0,255,255);public static readonly o p6=new o(0,120,255);public static readonly o qA=new o(255,0,255);
public static readonly o qN=new o(128,0,128);public static readonly o qu=new o(128,128,128);public static readonly o qh=new o(60,60,60);
public static o dR(string bf){if(string.IsNullOrWhiteSpace(bf))return fv;bf=bf.Trim();if(bf.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bf.Equals("none",StringComparison.OrdinalIgnoreCase))return rc;
if(bf.StartsWith("#")){string bU=bf.Substring(1).Trim();if(bU.Length==6){byte tI=Convert.ToByte(bU.Substring(0,2),16);byte ss=Convert.ToByte(bU.Substring(2,2),16);
byte rz=Convert.ToByte(bU.Substring(4,2),16);return new o(tI,ss,rz);}if(bU.Length==3){byte tJ=Convert.ToByte(new string(bU[0],2),16);byte st=Convert.ToByte(new string(bU[1],2),16);
byte rA=Convert.ToByte(new string(bU[2],2),16);return new o(tJ,st,rA);}}if(bf.Contains(",")){var di=bf.Split(',');if(di.Length>=3){byte tK=(byte)Math.Max(0,Math.Min(255,int.Parse(di[0].Trim())));
byte su=(byte)Math.Max(0,Math.Min(255,int.Parse(di[1].Trim())));byte rB=(byte)Math.Max(0,Math.Min(255,int.Parse(di[2].Trim())));byte rh=di.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(di[3].Trim()))):(byte)255;
return new o(tK,su,rB,rh);}}switch(bf.ToLowerInvariant()){case "red": return qP;case "green": return qv;case "lime": return qz;case "yellow": return rf;
case "orange": return qD;case "cyan": return qg;case "blue": return p6;case "magenta": return qA;case "purple": return qN;case "white": return fv;
case "black": return p4;case "gray": case "grey": return qu;case "darkgray": case "darkgrey": return qh;default: return fv;}}}public class bz{
public string[]bA;public string dB="vertical";public List<aT>aR;public string hk,hl;public List<aT>cN;public string hz,hB;
}public class aB : bz{public string cS="default";public int dF=0;public int dT=0;public int dV=1;public int dW=1;public string cm;public string hX="left";
public string hY;public string lm;public string lu;public float? lw;public string cL;public string hr;public string[]hs;
public string hN="horizontal";public List<bm>am=new List<bm>();}public class z : bz{public string a9;public string cj="standard";
public string dD="graphic";public string fc;public string[]az;public string lY="left";public string lZ;public string lv="Monospace";
public string bx;public string lk;public int ll=10;public int li=1;public float hp=1.2f;public bool hD;
public string hO;public string hm;public string lh;public string lf;public string hJ;public string hI;public string b7;
public string cc;public float hq=2.5f;public string lj="cyan";public string[]fa;public List<string>fb=new List<string>();
public List<string>qs(){if(fb!=null&&fb.Count>0)return fb;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float cO=1.0f;public int hR=0;public float hh=1.6f;public int dK=1;public int dL=1;public int l0=30;public string bE;
public int ci=0;public List<aB>ai=new List<aB>();public List<bm>fn{get{if(ai.Count==0)ai.Add(new aB{cS="main"}
);return ai[0].am;}}}public class bJ{public string hK;public List<string>fp=new List<string>();public Dictionary<string,string>hv=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>ho=new List<string>();}public class hQ{public List<bk>cb=new List<bk>();public List<z>T=new List<z>();
public List<string>a6=new List<string>();public List<string>d_=new List<string>();public Dictionary<string,List<string>>dz=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class lC{static readonly Dictionary<string,bJ>ln=new Dictionary<string,bJ>(StringComparer.OrdinalIgnoreCase);
static lC(){qQ();}static void qQ(){bH("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bH("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bH("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bH("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bH("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bH("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bH("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bH("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bH(string om,string[]tn,string rL){var g2=new bJ{hK=om}
;foreach(var tg in tn){var jK=tg.Split(new[]{'='},2);string oz=hL(jK[0]);g2.fp.Add(oz);if(jK.Length>1)g2.hv[oz]=ab(jK[1].Trim());
}var s3=rL.Replace("\r\n","\n").Split('\n');foreach(var sS in s3){string oW=sS.Trim();if(oW.Length>0)g2.ho.Add(oW);}ln[om]=g2;}static string hL(string df){
df=df.Trim();if(df.StartsWith("$"))df=df.Substring(1);return df.ToLowerInvariant();}public static hQ qF(string r_){var aq=new hQ();z c=null;
aB t=null;var oJ=qL(r_,aq.a6,aq.dz);for(int es=0;es<oJ.Count;es++){string a=oJ[es].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){aq.cb.Add(qG(a));}else if(a.StartsWith("@screen")){c=new z{a9=a.Substring("@screen".Length).Trim()}
;aq.T.Add(c);t=null;}else if(a.StartsWith("profile:")){q(c,"profile:");c.cj=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){q(c,"border:");string mp=a.Substring("border:".Length).Trim();if(t!=null)t.lm=mp;
else c.dD=mp;}else if(a.StartsWith("border_color:")){q(c,"border_color:");string mr=a.Substring("border_color:".Length).Trim();
if(t!=null)t.hr=mr;else c.fc=mr;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
q(c,"border_gradient");string ct=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
ct=ct.Trim();if(ct.StartsWith("(")&&ct.Contains(")"))ct=aU(ct,0);string[]pj=aV(ct).Select(tU=>tU.Trim()).ToArray();if(t!=null)t.hs=pj;
else c.az=pj;}else if(a.StartsWith("title_align:")){q(c,"title_align:");string pr=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(t!=null)t.hX=pr;else c.lY=pr;}else if(a.StartsWith("title_color:")){q(c,"title_color:");string pu=a.Substring("title_color:".Length).Trim();
if(t!=null)t.hY=pu;else c.lZ=pu;}else if(a.StartsWith("font_size:")){q(c,"font_size:");float po=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(t!=null)t.lw=po;else c.cO=po;}else if(a.StartsWith("font:")){q(c,"font:");string ni=a.Substring("font:".Length).Trim();
if(t!=null)t.lu=ni;else c.lv=ni;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){q(c,"bg:");string cY=a.Substring(a.IndexOf(':')+1).Trim();
bz iw=t!=null ?(bz)t : c;if(cY.StartsWith("color(")&&cY.EndsWith(")")){string mz;iw.aR=lM(cY.Substring(6,cY.Length-7),out mz);
string mx,my;lT(mz,out mx,out my);iw.hk=mx;iw.hl=my;}else if(t!=null)t.cL=cY;
else c.bx=cY;}else if(a.StartsWith("layout:")){q(c,"layout:");qH(a.Substring("layout:".Length).Trim(),c);}else if(a.StartsWith("alert_period:")){
q(c,"alert_period:");c.hh=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){q(c,"refresh:");c.hR=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
q(c,"output:");var gA=aV(a.Substring("output:".Length).Trim());if(gA.Count==0)throw new FormatException("output: requires a target block name");
c.bE=ab(gA[0]);c.ci=gA.Count>1 ? int.Parse(gA[1]): 0;}else if(a.StartsWith("width:")){q(c,"width:");
c.l0=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){q(c,"boot_msg:");
c.lk=ab(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){q(c,"boot_stagger:");c.ll=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){q(c,"boot_calibrate:");string mZ=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
c.hp=mZ=="off" ? 0f : Math.Max(0f,float.Parse(mZ,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){q(c,"boot_batch:");
c.li=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){q(c,"boot_duration:");
string eg=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool nR=eg.EndsWith("s");if(nR)eg=eg.Substring(0,eg.Length-1).Trim();
float iY;if(float.TryParse(eg,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out iY)){c.hq=nR ? iY : iY/60f;
}}else if(a.StartsWith("boot_log:")){q(c,"boot_log:");c.fb.Add(ab(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("mirror_panel:")){
q(c,"mirror_panel:");c.hJ=ab(a.Substring("mirror_panel:".Length).Trim());}else if(a.StartsWith("mirror:")){q(c,"mirror:");
c.hI=ab(a.Substring("mirror:".Length).Trim());}else if(a.StartsWith("banner:")){q(c,"banner:");c.b7=ab(a.Substring("banner:".Length).Trim());
}else if(a.StartsWith("boot_logo:")){q(c,"boot_logo:");c.cc=ab(a.Substring("boot_logo:".Length).Trim());}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){
q(c,"boot_gradient");c.fa=lN(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){q(c,"bg_gradient");
(t!=null ?(bz)t : c).bA=lN(a);}else if(a.StartsWith("bg_gradient_dir:")){q(c,"bg_gradient_dir:");
(t!=null ?(bz)t : c).dB=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(a.StartsWith("flash:")){
q(c,"flash:");bz i5=t!=null ?(bz)t : c;string i4=a.Substring("flash:".Length).Trim();string nk=lV(i4);
string nj,nl;lT(i4.Substring(0,i4.Length-nk.Length),out nj,out nl);i5.hz=nj;i5.hB=nl;i5.cN=new List<aT>{
new aT{hw=nk,ce="on"}};}else if(a.StartsWith("bg_texture:")){q(c,"bg_texture:");c.hm=ab(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){q(c,"bg_texture_color:");c.lh=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gradient_mode:")){q(c,"gradient_mode:");c.hD=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){q(c,"screen_bg:");c.bx=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
q(c,"panel_bg:");c.hO=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){q(c,"alert_bg:");
c.lf=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){q(c,"boot_color:");
c.lj=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){q(c,"orientation:");string td=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(t!=null)t.hN=td;}else if(a.StartsWith("@panel")){q(c,"@panel");t=qJ(a);c.ai.Add(t);
}else if(a.StartsWith("title:")){q(c,"title:");if(t==null){t=new aB{cS="main",dF=0,dT=0,dV=c.dK,dW=c.dL}
;c.ai.Add(t);}t.cm=ab(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){q(c,"line:");
if(t==null){t=new aB{cS="main",dF=0,dT=0,dV=c.dK,dW=c.dL};c.ai.Add(t);}t.am.Add(qI(a.Substring("line:".Length).Trim()));
}else{aq.a6.Add($"Line {es + 1}: unrecognized directive: \"{a}\"");}}catch(Exception sb){aq.a6.Add($"Line {es + 1}: parse error ({sb.Message}) in \"{a}\"");
}}foreach(var oX in aq.T){if(string.IsNullOrEmpty(oX.bE))aq.a6.Add($"@screen {oX.a9}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var at in aq.T){int j5=at.ai.Count(kk=>kk.cL!=null||kk.aR!=null||kk.bA!=null);if(at.ai.Count>1&&j5>0&&j5<at.ai.Count)aq.d_.Add("screen '"+at.a9+"': only "+j5+" of "+at.ai.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(at.b7!=null&& !aq.dz.ContainsKey(at.b7))aq.a6.Add("Screen '"+at.a9+"': banner '"+at.b7+"' is not defined (@banner "+at.b7+" ... @end)");
if(at.cc!=null&& !aq.dz.ContainsKey(at.cc))aq.a6.Add("Screen '"+at.a9+"': boot_logo '"+at.cc+"' is not defined (@banner "+at.cc+" ... @end)");
}return aq;}static void qH(string iD,z o3){int kc=iD.IndexOf('(');if(kc<0)throw new FormatException("missing '(' in layout: specification");
string nZ=iD.Substring(0,kc).Trim().ToLowerInvariant();string ro=aU(iD,kc);var ik=aV(ro);if(nZ=="grid"){if(ik.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
o3.dK=Math.Max(1,int.Parse(ik[0]));o3.dL=Math.Max(1,int.Parse(ik[1]));}else{throw new FormatException($"unknown layout kind '{nZ}' (expected grid)");
}}static aB qJ(string s_){string eO=s_.Substring("@panel".Length).Trim();int fJ=eO.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(fJ<0)throw new FormatException("@panel requires an at(...) positioning clause");
string ta=eO.Substring(0,fJ).Trim();string mk=eO.Substring(fJ);int oF=mk.IndexOf('(');string mi=aU(mk,oF);
var cr=aV(mi);if(cr.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bs=new aB{cS=ta,dF=int.Parse(cr[0]),dT=int.Parse(cr[1]),dV=cr.Count>2 ? int.Parse(cr[2]): 1,dW=cr.Count>3 ? int.Parse(cr[3]): 1}
;int mb=fJ+oF+mi.Length+1;if(mb<eO.Length){string aN=eO.Substring(mb).Trim();int pz=aN.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(pz>=0){string eW=aN.Substring(pz+5).Trim();if(eW.StartsWith("(")&&eW.Contains(")"))bs.cm=ab(aU(eW,0));else if(eW.StartsWith(":")){
string cu=eW.Substring(1).Trim();if(cu.StartsWith("\"")){int ne=cu.IndexOf('"',1);bs.cm=ne>0 ? cu.Substring(1,ne-1): ab(cu);}else{int pe=cu.IndexOf(' ');
bs.cm=pe>=0 ? cu.Substring(0,pe): cu;}}}string oq=ff(aN,"orientation:");if(oq!=null)bs.hN=oq.ToLowerInvariant();string l9=ff(aN,"title_align:");
if(l9!=null)bs.hX=l9.ToLowerInvariant();string pw=ff(aN,"title_color:");if(pw!=null)bs.hY=pw;int fP=aN.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(fP>=0&& !aN.Substring(fP).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aN.Substring(fP).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bs.lm=ff(aN.Substring(fP),"border:");}int fM=aN.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(fM<0)fM=aN.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(fM>=0){string bS=aN.Substring(fM+"border_gradient".Length).Trim();if(bS.StartsWith(":"))bS=bS.Substring(1).Trim();if(bS.StartsWith("(")&&bS.Contains(")"))bS=aU(bS,0);
bs.hs=aV(bS).Select(tV=>tV.Trim()).ToArray();}}return bs;}static string ff(string oY,string nY){int nQ=oY.IndexOf(nY,StringComparison.OrdinalIgnoreCase);
if(nQ<0)return null;string k6=oY.Substring(nQ+nY.Length).Trim();int pf=k6.IndexOf(' ');return pf>=0 ? k6.Substring(0,pf): k6;}static string[]lN(string jQ){
int jg=jQ.IndexOfAny(new[]{':','('});string en=jQ.Substring(jQ[jg]==':' ? jg+1 : jg).Trim();if(en.StartsWith("(")&&en.Contains(")"))en=aU(en,0);return aV(en).Select(tW=>tW.Trim()).ToArray();
}static List<string>qL(string r0,List<string>i2,Dictionary<string,List<string>>rD){var pF=new Dictionary<string,bJ>(StringComparer.OrdinalIgnoreCase);
var dl=(r0 ?? "").Replace("\r\n","\n").Split('\n');var j4=new List<string>();int da=0;while(da<dl.Length){string kq=dl[da++];if(kq.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ao=new List<string>();while(da<dl.Length){string l7=dl[da++].Replace("\t","    ").TrimEnd();if(l7.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ao.Add(l7);}while(ao.Count>0&&ao[0].Length==0)ao.RemoveAt(0);while(ao.Count>0&&ao[ao.Count-1].Length==0)ao.RemoveAt(ao.Count-1);int jz=int.MaxValue;
foreach(var ib in ao)if(ib.Length>0)jz=Math.Min(jz,ib.Length-ib.TrimStart().Length);for(int ez=0;ez<ao.Count;ez++)if(ao[ez].Length>0)ao[ez]=ao[ez].Substring(jz);
rD[kq.TrimStart().Substring(8).Trim()]=ao;continue;}string bX=lU(kq).Trim();if(bX.Length==0)continue;if(bX.StartsWith("@template")){try{
var kX=qK(bX);while(da<dl.Length){string rN=dl[da++];string iE=lU(rN).Trim();if(iE.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(iE.Length>0)kX.ho.Add(iE);}pF[kX.hK]=kX;}catch(Exception sc){i2.Add($"Template parse error ({sc.Message}) in \"{bX}\"");}}
else if(bX.StartsWith("@use")){try{lr(bX,pF,j4,i2);}catch(Exception sd){i2.Add($"@use error ({sd.Message}) in \"{bX}\"");
}}else{j4.Add(bX);}}return j4;}static bJ qK(string s0){string kr=s0.Substring("@template".Length).Trim();int kd=kr.IndexOf('(');
if(kd<0)throw new FormatException("missing '(' in @template header");string tb=kr.Substring(0,kd).Trim();string rp=aU(kr,kd);var kY=new bJ{
hK=tb};foreach(var th in aV(rp)){var jL=th.Split(new[]{'='},2);string oA=hL(jL[0]);kY.fp.Add(oA);if(jL.Length>1)kY.hv[oA]=ab(jL[1].Trim());
}return kY;}static void lr(string s1,Dictionary<string,bJ>pG,List<string>ow,List<string>sa,int m2=0){string gK=s1.Substring("@use".Length).Trim();
int gy=gK.IndexOf('(');string jY=gy>=0 ? gK.Substring(0,gy).Trim(): gK.Trim();string rq=gy>=0 ? aU(gK,gy): "";bJ du=null;
if(!pG.TryGetValue(jY,out du)){if(!ln.TryGetValue(jY,out du))throw new FormatException($"unknown template '{jY}'");}var fS=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var n2 in du.hv)fS[n2.Key]=n2.Value;var mh=aV(rq);for(int d8=0;d8<mh.Count;d8++){string fH=mh[d8];int i0=fH.IndexOf('=');if(i0>=0){
string sK=hL(fH.Substring(0,i0));string uA=ab(fH.Substring(i0+1).Trim());fS[sK]=uA;}else{if(d8<du.fp.Count){string sL=du.fp[d8];
fS[sL]=ab(fH.Trim());}}}foreach(var rM in du.ho){string ej=rM;foreach(var gp in fS){ej=ej.Replace("$"+gp.Key,gp.Value).Replace("{"+gp.Key+"}",gp.Value);
}if(ej.StartsWith("@use")){if(m2>3)throw new FormatException("@use nested too deep");lr(ej,pG,ow,sa,m2+1);
}else ow.Add(ej);}}static void q(z rZ,string r7){if(rZ==null)throw new FormatException($"'{r7}' used before any @screen declared");
}static string lU(string bd){if(string.IsNullOrEmpty(bd))return bd;bool jy=false;for(int bV=0;bV<bd.Length;bV++){char iI=bd[bV];if(iI=='"'){jy= !jy;
}else if(!jy){if(iI=='/'&&bV+1<bd.Length&&bd[bV+1]=='/'){return bd.Substring(0,bV);}if(iI=='#'){if(qx(bd,bV)){continue;}return bd.Substring(0,bV);}}}return bd;
}static bool qx(string cB,int nT){int dd=0;for(int ju=nT+1;ju<cB.Length;ju++){char c0=cB[ju];if((c0>='0'&&c0<='9')||(c0>='a'&&c0<='f')||(c0>='A'&&c0<='F'))dd++;
else break;}if(dd==3||dd==4||dd==6||dd==8){int eD=nT+1+dd;if(eD>=cB.Length||char.IsWhiteSpace(cB[eD])||cB[eD]==')'||cB[eD]==','||cB[eD]==':')return true;
}return false;}static bk qG(string s2){string ks=s2.Substring("@bind".Length).Trim();int i1=ks.IndexOf('=');if(i1<0)throw new FormatException("missing '=' in @bind");
string oo=ks.Substring(0,i1).Trim();string i3=ks.Substring(i1+1).Trim();int ke=i3.IndexOf('(');if(ke<0)throw new FormatException("missing '(' in @bind target");
string f9=i3.Substring(0,ke).Trim().ToLowerInvariant();string rr=aU(i3,ke);var bP=aV(rr);if(bP.Count==0)throw new FormatException("@bind target has no arguments");
var ar=new bk{ae=oo};if(f9=="dynamic"||f9=="var"||f9=="mem"){ar.bD=a4.dI;ar.bl=bP.Count>0 ? ab(bP[0].Trim()): oo;
for(int ic=0;ic<bP.Count;ic++){var gq=bP[ic].Split(new[]{':'},2);if(gq.Length==2&&gq[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double m1;
if(double.TryParse(gq[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out m1))ar.fe=m1;else ar.hu=ab(gq[1].Trim());
}}return ar;}ar.bD=f9=="group" ? a4.hE : a4.p5;string fF=null;for(int fE=0;fE<bP.Count;fE++){var dc=bP[fE].Split(new[]{':'},2);if(dc.Length==2&&dc[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
fF=dc[1].Trim().ToLowerInvariant();}else if(dc.Length==2&&dc[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ar.e7=dc[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ar.aW.Add(ab(bP[fE].Trim()));}}if(ar.aW.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ar.bD==a4.hE){if(fF==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(fF){case "avg": ar.dy=aH.hi;break;case "sum": ar.dy=aH.lW;break;case "min": ar.dy=aH.lI;break;case "max": ar.dy=aH.lH;
break;default: throw new FormatException($"unknown agg '{fF}' (expected avg/sum/min/max)");}}return ar;}static bm qI(string fO){var n=new bm();
if(!fO.StartsWith("\""))throw new FormatException("line must start with a quoted label");int i_=fO.IndexOf('"',1);if(i_<0)throw new FormatException("unterminated quoted label");
n.Z=fO.Substring(1,i_-1);string eN=fO.Substring(i_+1).Trim();int gz=eN.IndexOf('(');if(gz<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string C=eN.Substring(0,gz).Trim().ToLowerInvariant();string rn=aU(eN,gz);string ag=eN.Substring(eN.IndexOf(')',gz)+1).Trim();
var iW=rn.Trim().Split('.');if(iW.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
n.aS=iW[0].Trim();n.I=iW[1].Trim();if(C=="bar")n.h=e.dA;else if(C=="led")n.h=e.dQ;else if(C=="spark"||C=="sparkline")n.h=e.ck;
else if(C=="hazard"||C=="stripes")n.h=e.dM;else if(C=="chevron"||C=="chevrons")n.h=e.dE;else if(C=="dots"||C=="dot")n.h=e.dH;
else if(C=="balance"||C=="horizon"||C=="centered")n.h=e.cM;else if(C=="toggle"||C=="switch")n.h=e.cn;else if(C=="checkbox"||C=="check")n.h=e.cd;
else if(C=="status"||C=="badge"||C=="state")n.h=e.cl;else if(C=="ammo"||C=="pips")n.h=e.cK;else if(C=="flow"||C=="rate"||C=="spinner")n.h=e.cf;
else if(C=="text")n.h=e.hW;else if(C=="tank"||C=="tube"||C=="bucket")n.h=e.fu;else throw new FormatException($"unknown display function '{C}'");
while(ag.Length>0){int iU=ag.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);int c7=ag.IndexOf('(');if(iU>=0&&(c7<0||iU<c7)){
string fZ=ag.Substring(iU+4).Trim();int gU=fZ.IndexOf(' ');n.lp=(gU>=0 ? fZ.Substring(0,gU): fZ).ToLowerInvariant();ag=(gU>=0 ? fZ.Substring(gU): "").Trim();
continue;}int k4=ag.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(k4>=0&&(c7<0||k4<c7)){string hd=ag.Substring(k4+5).Trim();
int gV=hd.IndexOf(' ');n.dY=(gV>=0 ? hd.Substring(0,gV): hd).Trim().Trim('"');ag=(gV>=0 ? hd.Substring(gV): "").Trim();continue;}int jb=ag.IndexOf("format:",StringComparison.OrdinalIgnoreCase);
if(jb>=0&&(c7<0||jb<c7)){string f3=ag.Substring(jb+7).Trim();int gW=f3.IndexOf(' ');n.dJ=(gW>=0 ? f3.Substring(0,gW): f3).Trim().Trim('"');
ag=(gW>=0 ? f3.Substring(gW): "").Trim();continue;}int gt=ag.IndexOf('(');if(gt<0)break;string ad=ag.Substring(0,gt).Trim().ToLowerInvariant();
int n9=ad.LastIndexOf(' ');if(n9>=0)ad=ad.Substring(n9+1).Trim();string aD=aU(ag,gt);ag=ag.Substring(ag.IndexOf(')',gt)+1).Trim();
if(ad=="gradient"){n.cg=true;n.a8=aV(aD).Select(tX=>tX.Trim()).ToArray();if(n.a8.Length<2)throw new FormatException("gradient() needs at least 2 colors");
}else if(ad=="size"){string kW=aD.Trim().ToLowerInvariant();n.fs=kW=="large"||kW=="fill" ? 2 : kW=="medium" ? 1 : 0;}else if(ad=="color_by"){var iM=aD.Trim().Split('.');
if(iM.Length!=2)throw new FormatException("color_by() takes bindname.field");n.dG=iM[0].Trim();n.ht=iM[1].Trim();}else if(ad=="label_color")n.dO=aD.Trim();
else if(ad=="track")n.hZ=aD.Trim();else if(ad=="flat"){n.au=aD.Trim();}else if(ad=="pattern"){string S=aD.Trim().ToLowerInvariant();
if(S=="hazard"||S=="stripes")n.h=e.dM;else if(S=="chevron"||S=="chevrons")n.h=e.dE;else if(S=="dots"||S=="dot")n.h=e.dH;
else if(S=="led")n.h=e.dQ;else if(S=="balance"||S=="horizon")n.h=e.cM;else if(S=="toggle"||S=="switch")n.h=e.cn;
else if(S=="checkbox"||S=="check")n.h=e.cd;else if(S=="status"||S=="badge"||S=="state")n.h=e.cl;else if(S=="ammo"||S=="pips")n.h=e.cK;
else if(S=="flow"||S=="rate"||S=="spinner")n.h=e.cf;}else if(ad=="color"){string sD;n.bB=lM(aD,out sD);
}else if(ad=="dir"||ad=="direction"){n.lp=aD.Trim().ToLowerInvariant();}else if(ad=="unit"){n.dY=aD.Trim().Trim('"');}else if(ad=="max"){
double oi;if(double.TryParse(aD.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out oi))n.fo=oi;else{var jV=aD.Trim().Split('.');
if(jV.Length!=2)throw new FormatException("max() takes a number or bindname.field");n.cR=jV[0].Trim();n.hH=jV[1].Trim();}}else if(ad=="format"||ad=="fmt"){
n.dJ=aD.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{ad}'");}}return n;}static string aU(string gQ,int j2){
int iT=0;for(int et=j2;et<gQ.Length;et++){if(gQ[et]=='(')iT++;else if(gQ[et]==')'){iT--;if(iT==0)return gQ.Substring(j2+1,et-j2-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aV(string rs){return rs.Split(',').Select(tY=>tY.Trim()).Where(tZ=>tZ.Length>0).ToList();
}static List<aT>lM(string rm,out string i7){var gO=new List<aT>();i7=null;foreach(var kf in aV(rm)){int iQ=kf.LastIndexOf(':');
if(iQ<0)throw new FormatException("color() entries need 'expr : color'");string gr=kf.Substring(0,iQ).Trim();string mV=kf.Substring(iQ+1).Trim();
if(gr.Equals("default",StringComparison.OrdinalIgnoreCase)){gO.Add(new aT{cP=true,ce=mV});}else{string pk=lV(gr);
if(i7==null)i7=gr.Substring(0,gr.Length-pk.Length).Trim();gO.Add(new aT{hw=pk,ce=mV});}}if(!gO.Any(tL=>tL.cP))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return gO;}static void lT(string tM,out string rJ,out string sg){var eG=(tM ?? "").Split('.');if(eG.Length!=2||eG[0].Trim().Length==0||eG[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
rJ=eG[0].Trim();sg=eG[1].Trim();}static string lV(string c5){c5=c5.Trim();foreach(var te in new[]{">=","<=","==",">","<"}){int jv=c5.IndexOf(te,StringComparison.Ordinal);
if(jv>0)return c5.Substring(jv).Trim();if(jv==0)return c5;}throw new FormatException($"color() rule has no recognizable comparison operator: '{c5}'");}
static string ab(string cC){if(cC.Length>=2&&cC[0]=='"'&&cC[cC.Length-1]=='"')return cC.Substring(1,cC.Length-2);return cC;}}public static class p3{public static double p2(aH ri,List<double>dw){
if(dw==null||dw.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(ri){case aH.hi: return dw.Average();case aH.lW: return dw.Sum();
case aH.lI: return dw.Min();case aH.lH: return dw.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class H{
public static string qU(string sU,double gB,int mq=10,string pO=null){gB=Math.Max(0,Math.Min(100,gB));int nr=(int)Math.Round(mq*(gB/100.0));
string rE=new string('#',nr)+new string('-',mq-nr);string uB=pO!=null ? pO : $"{gB:0}%";return $"{sU} [{rE}] {uB}";
}public static string lx(double b6,string k3=null,string f8=null){if(k3=="auto"){double id=Math.Abs(b6);if(id>=1e9)return(b6/1e9).ToString("F1")+"G";
if(id>=1e6)return(b6/1e6).ToString("F1")+"M";if(id>=1e3)return(b6/1e3).ToString("F1")+"k";string se= !string.IsNullOrEmpty(f8)? f8 : "F0";return b6.ToString(se);
}string nt= !string.IsNullOrEmpty(f8)? f8 :(Math.Abs(b6)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(k3)){return b6.ToString(nt);}return b6.ToString(nt)+k3;
}public static float qt(string tB,float pm,float kU,float f_=1.0f){string dg=(tB ?? "standard").ToLowerInvariant();
if(dg=="compact"||(dg=="auto"&&pm<=300&&kU<=300&&kU>160)){return f_*0.75f;}if(dg=="corner"||(dg=="auto"&&kU<=160)){
return f_*0.55f;}if(dg=="wide"||(dg=="auto"&&pm>=1000)){return f_*1.15f;}return f_;}public static string qS(string sV,double gC,int bT=5){
gC=Math.Max(0,Math.Min(100,gC));double f0=(gC-50.0)/50.0;int eA=0;int eQ=0;if(f0< -0.01)eA=(int)Math.Round(-f0*bT);
else if(f0>0.01)eQ=(int)Math.Round(f0*bT);if(eA>bT)eA=bT;if(eQ>bT)eQ=bT;string sY=new string('-',bT-eA)+new string('<',eA);
string tR=new string('>',eQ)+new string('-',bT-eQ);return $"{sV} [{sY}|{tR}] {gC:0}%";}public static string q3(string n6,double uD){
return uD>=50.0 ? $"{n6} [====O] ON" : $"{n6} [O====] OFF";}public static string qX(string n7,double uE){return uE>=50.0 ? $"{n7} [X] ACTIVE" : $"{n7} [ ] READY ";
}public static string q2(string jN,double pM){if(pM>=75.0)return $"{jN} [ ONLINE  ]";if(pM>=25.0)return $"{jN} [ STANDBY ]";return $"{jN} [ OFFLINE ]";
}public static string qR(string sW,double gD,int oe=8){gD=Math.Max(0,Math.Min(100,gD));int ns=(int)Math.Round(oe*(gD/100.0));
string tv=new string('|',ns)+new string('.',oe-ns);return $"{sW} [{tv}] {gD:0}%";}public static string qY(string jO,double eJ,int py=0){
char[]uc=new[]{'|','/','-','\\'};char oZ=uc[(py>=0 ? py : 0)%4];if(eJ>55.0)return $"{jO} {oZ} ▲ CHG {eJ:0}%";if(eJ<45.0)return $"{jO} {oZ} ▼ DSC {eJ:0}%";
return $"{jO} ● STBY {eJ:0}%";}public static bool qB(string aw,string ew){if(aw==null||ew==null)return false;if(aw=="*"||aw==ew)return true;
if(!aw.Contains("*")&& !aw.Contains("?"))return string.Equals(aw,ew,StringComparison.OrdinalIgnoreCase);int aE=0,eu=0,kL= -1,jU=0;while(eu<ew.Length){
if(aE<aw.Length&&(aw[aE]=='?'||char.ToLowerInvariant(aw[aE])==char.ToLowerInvariant(ew[eu]))){aE++;eu++;}else if(aE<aw.Length&&aw[aE]=='*'){
kL=aE;jU=eu;aE++;}else if(kL!= -1){aE=kL+1;jU++;eu=jU;}else{return false;}}while(aE<aw.Length&&aw[aE]=='*')aE++;return aE==aw.Length;
}public static int p7(double ti,int pD){return Math.Max(0,Math.Min(pD,1+(int)(Math.Max(0.0,ti)*pD)));}public static string fi(string[]dt,double kg){
if(dt==null||dt.Length==0)return "white";kg=Math.Max(0,Math.Min(100,kg));int jw=(int)Math.Floor(kg/100.0*dt.Length);if(jw>=dt.Length)jw=dt.Length-1;
return dt[jw];}public static o lz(string[]b4,double tr){if(b4==null||b4.Length==0)return o.fv;if(b4.Length==1)return o.dR(b4[0]);
double tP=Math.Max(0.0,Math.Min(1.0,tr/100.0));double o2=tP*(b4.Length-1);int jx=(int)Math.Floor(o2);int sC=Math.Min(b4.Length-1,jx+1);
double kP=o2-jx;o c1=o.dR(b4[jx]);o iJ=o.dR(b4[sC]);byte tN=(byte)Math.Round(c1.bG+(iJ.bG-c1.bG)*kP);
byte sv=(byte)Math.Round(c1.bC+(iJ.bC-c1.bC)*kP);byte rC=(byte)Math.Round(c1.bw+(iJ.bw-c1.bw)*kP);return new o(tN,sv,rC);}public static string dS(List<aT>oV,double uF){
foreach(var kt in oV){if(kt.cP)continue;if(qp(kt.hw,uF))return kt.ce;}var m0=oV.FirstOrDefault(tO=>tO.cP);return m0!=null ? m0.ce : null;
}static bool qp(string ek,double e2){ek=ek.Trim();foreach(var j1 in new[]{">=","<=","==",">","<"}){if(ek.StartsWith(j1)){double eP=double.Parse(ek.Substring(j1.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(j1){case ">=": return e2>=eP;case "<=": return e2<=eP;case "==": return Math.Abs(e2-eP)<1e-9;case ">": return e2>eP;case "<": return e2<eP;
}}}throw new FormatException($"unsupported comparison expression: '{ek}'");}public static List<string>qW(List<string>rY,int dx){
var gv=new List<string>();gv.Add("+"+new string('-',dx+2)+"+");foreach(var jR in rY){string uv=jR.Length>dx ? jR.Substring(0,dx): jR;
string tl=uv.PadRight(dx);gv.Add("| "+tl+" |");}gv.Add("+"+new string('-',dx+2)+"+");return gv;}public struct lL{public float l2,l3,l1,lB;
}public static lL qd(int mS,int oS,int gX,int gY,int eY,int eZ,float um,float ul,float oB,float oC,float ji,float jj){
eY=Math.Max(1,eY);eZ=Math.Max(1,eZ);gX=Math.Max(1,Math.Min(eY-mS,gX));gY=Math.Max(1,Math.Min(eZ-oS,gY));
float rv=um-(oB*2f);float ru=ul-(oC*2f);float mN=(rv-(ji*(eY-1)))/eY;float mM=(ru-(jj*(eZ-1)))/eZ;
float tD=oB+mS*(mN+ji);float tE=oC+oS*(mM+jj);float tC=gX*mN+((gX-1)*ji);float tt=gY*mM+((gY-1)*jj);return new lL{
l2=tD,l3=tE,l1=tC,lB=tt};}public struct lQ{public int qE;public int hG;public int af;public int aA;public int dX;
public int a5;public int ra;public int rb;public int qm;public int qn;
public double qo;}public static lQ qe(z b2){var l=new lQ();var oE=b2.ai!=null&&b2.ai.Count>0 ? b2.ai : new List<aB>{
new aB{am=b2.fn}};l.qE=oE.Count;l.hG=0;l.af=0;l.aA=0;if(!string.IsNullOrEmpty(b2.bx)&& !b2.bx.Equals("none",StringComparison.OrdinalIgnoreCase))l.af++;
if(!string.IsNullOrEmpty(b2.dD)&& !b2.dD.Equals("none",StringComparison.OrdinalIgnoreCase))l.af+=2;foreach(var eH in oE){
if(!string.IsNullOrEmpty(eH.cL)&& !eH.cL.Equals("none",StringComparison.OrdinalIgnoreCase))l.af++;if(!string.IsNullOrEmpty(eH.cm))l.af+=2;
l.hG+=eH.am.Count;foreach(var sT in eH.am){l.ra++;l.rb++;l.af+=2;switch(sT.h){case e.dA: l.dX++;
l.a5++;l.af+=2;l.aA++;break;case e.dQ: l.a5+=10;l.af+=10;
l.aA+=10;break;case e.dM: l.a5+=10;l.af+=11;l.aA+=10;break;case e.dE: l.a5+=10;
l.af+=10;l.aA+=10;break;case e.dH: l.a5+=12;l.af+=12;l.aA+=12;
break;case e.ck: l.dX++;l.a5+=8;l.af+=9;l.aA+=8;break;case e.cM: l.dX+=2;
l.a5++;l.af+=3;l.aA++;break;case e.cn: case e.cd: l.dX++;
l.a5++;l.af+=2;l.aA++;break;case e.cl: case e.cf: l.dX++;l.a5+=2;
l.af+=3;l.aA+=2;break;case e.cK: l.a5+=8;l.af+=8;l.aA+=8;
break;default: l.aA++;break;}}}l.qm=25+(l.hG*2);l.qn=180+(l.aA*4);
l.qo=0.0;return l;}}