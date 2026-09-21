hA q;string hP=null;Dictionary<string,List<IMyTerminalBlock>>bG=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>fp=new Dictionary<string,bool>();
Dictionary<string,bg>dT=new Dictionary<string,bg>();Dictionary<string,Color>lK=new Dictionary<string,Color>();List<IMyTerminalBlock>cl=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>cQ=new List<IMyTerminalBlock>();bool fm;int hM;string dU="";StringBuilder dX=new StringBuilder();readonly Dictionary<string,double>aG=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>cP=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener dV;bool cm=false;
Dictionary<z,IMyTextSurface>bH=new Dictionary<z,IMyTextSurface>();Dictionary<z,string>cn=new Dictionary<z,string>();
static readonly string[]lr=new string[101];static Program(){for(int f0=0;f0<=100;f0++){lr[f0]=f0+"%";}}static string le(double t8){int en=(int)Math.Round(t8);
if(en<0)en=0;else if(en>100)en=100;return lr[en];}class aA{public List<MySprite>f=new List<MySprite>();public List<cd>dM=new List<cd>();
public int b6= -1;public Color[]e0;public Color eX;public bool eY;public Vector2 dG,lq;public int dR;public bool dF=false;
public List<e1>dv=new List<e1>();}class e1{public int fa;public Color g7,e5;public List<aO>fi,e8;public string hb,hk,hl,hn;
public bool ll,hp;}class cd{public bA fd;public int V= -1;public int aS= -1;public int cM= -1;public bool lh=false;
public List<int>K=new List<int>();public float[]fj=new float[8];public float b5;public float bf;public float aZ;
public float b4;public double fc= -99999.0;public string ln;public float D= -1f;public bool lF;public float k4=12f;}Dictionary<z,aA>bI=new Dictionary<z,aA>();
int lM=0;double hR=0;double dY=0;double Z=0;int dW=0;Dictionary<z,double>bj=new Dictionary<z,double>();
Dictionary<z,double>hL=new Dictionary<z,double>();Dictionary<string,int>fs=new Dictionary<string,int>();List<string>cR=new List<string>();
Dictionary<string,int>ft=new Dictionary<string,int>();double hT;Dictionary<string,double>d_=new Dictionary<string,double>();List<MyInventoryItem>hO=new List<MyInventoryItem>();
int bJ= -1,fr;double hS;Dictionary<z,double>fn=new Dictionary<z,double>();int dZ=0;const string p1="0.11.0";
IMyTextSurface aT;bool hK=true;double fo=0;string lL="";string hQ="";int fq=0,hN=0;List<KeyValuePair<string,int>>cO=new List<KeyValuePair<string,int>>();
const double qj=120;const float px=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{dV=IGC.RegisterBroadcastListener("HELM");
dV.SetMessageCallback("HELM_IGC");}catch{}fh();hE();lk();}public void Main(string lY,UpdateType t4){
double mP=Runtime.TimeSinceLastRun.TotalSeconds;Z+=mP;if((t4&UpdateType.IGC)!=0||lY=="HELM_IGC"){qh();}string om=(lY ?? "").Trim();
string al=om.ToLowerInvariant();if(al=="reload"){fh();}else if(al=="refresh"){hC();}else if(al=="boot"){hE();}else if(al=="probe"){
dY=Z+20.0;pR();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(al=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=qF;
fh();hE();}else if(al=="console on"||al=="console off"){hK=al=="console on";lk();fo=0;}else if(al=="scan"||al=="diagnose"||al=="test"){
qC();return;}else if(al.StartsWith("set ")){string gz=om.Substring(4).Trim();int mT=gz.IndexOf('=');int tF=gz.IndexOf(' ');int kl=mT>0 ? mT : tF;
if(kl>0){string ny=gz.Substring(0,kl).Trim();string pd=gz.Substring(kl+1).Trim();kX($"{ny}={pd}");Echo($"[HELM] Set dynamic variable '{ny}' = {pd}");
}return;}else if(al.StartsWith("alert")){string eT=al.Length>5 ? al.Substring(5).Trim(): "toggle";if(eT=="toggle")cm= !cm;else cm=eT=="red"||eT=="on"||eT=="1"||eT=="true";
Echo($"[HELM] Red alert state: {cm}");return;}else if(Me.CustomData!=hP){fh();}dZ++;hR+=mP;if(hR>=qj){
hR=0;hC();}if(dY>0){if(Z<dY){hJ();return;}dY=0;bI.Clear();cn.Clear();
fo=0;}if(q==null||q.S.Count==0){hJ();k6();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cy=q.S[lM%q.S.Count];
lM++;hQ=cy.a4;bool hX=bj.Count>0;if(hX)qq();double mQ;if(!bj.ContainsKey(cy)&& !(hL.TryGetValue(cy,out mQ)&&Z<mQ)){
if(qv(cy))hX=true;if(cy.hB>0)hL[cy]=Z+cy.hB/60.0;}dW=hX ? 0 : dW+1;Runtime.UpdateFrequency=dW<=q.S.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
qE();qD();hJ();k6();fq=Runtime.CurrentInstructionCount;if(fq>hN)hN=fq;}void k6(){if(dU.Length>0)Echo(dU);
if(dX.Length>0)Echo(dX.ToString());}List<IMyTerminalBlock>eZ(bool qP=false){if(!fm){cQ.Clear();GridTerminalSystem.GetBlocks(cQ);
cl.Clear();cl.AddRange(cQ);cl.RemoveAll(q1=> !q1.IsSameConstructAs(Me));hM=cQ.Count-cl.Count;fm=true;
}return qP ? cQ : cl;}IMyTerminalBlock k9(string nW,bool qR=false){var h5=GridTerminalSystem.GetBlockWithName(nW);if(h5==null||qR||h5.IsSameConstructAs(Me))return h5;
var hZ=eZ();for(int f1=0;f1<hZ.Count;f1++)if(hZ[f1].CustomName.IndexOf(nW,StringComparison.OrdinalIgnoreCase)>=0)return hZ[f1];return null;}void pI(string nX,List<IMyTerminalBlock>ep,bool lR=false){
var na=GridTerminalSystem.GetBlockGroupWithName(nX);if(na==null)return;int l8=ep.Count;var eP=new List<IMyTerminalBlock>();na.GetBlocks(eP);kV(eP,ep,lR);
if(ep.Count>l8)return;var nl=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(nl);foreach(var pr in nl){if(pr.Name.IndexOf(nX,StringComparison.OrdinalIgnoreCase)<0)continue;
eP.Clear();pr.GetBlocks(eP);kV(eP,ep,lR);if(ep.Count>l8)return;}}void kV(List<IMyTerminalBlock>rW,List<IMyTerminalBlock>nw,bool qQ=false){
foreach(var h6 in rW)if((qQ||h6.IsSameConstructAs(Me))&& !nw.Contains(h6))nw.Add(h6);}void fh(){hP=Me.CustomData;q=lj.qa(hP);
dU="";if(q.a1.Count>0){var oz=new StringBuilder("HELM config errors:\n");foreach(var rF in q.a1)oz.AppendLine(" - "+rF);dU=oz.ToString();
}if(q.dS.Count>0){var pq=new StringBuilder("HELM notes:\n");foreach(var ud in q.dS)pq.AppendLine(" - "+ud);dU+=pq.ToString();}dT.Clear();
foreach(var h7 in q.b7)if(!dT.ContainsKey(h7.ac))dT[h7.ac]=h7;bI.Clear();cn.Clear();bj.Clear();hL.Clear();
fn.Clear();pX();hC();}void qh(){try{while(dV!=null&&dV.HasPendingMessage){
var nS=dV.AcceptMessage();if(nS.Data is string){kX((string)nS.Data);}}}catch{}}void kX(string od){
if(string.IsNullOrEmpty(od))return;var oa=od.Split(';');for(int i3=0;i3<oa.Length;i3++){var eB=oa[i3].Trim();if(eB.Length==0)continue;int ea=eB.IndexOf('=');
if(ea<0)ea=eB.IndexOf(':');if(ea>0){string c6=eB.Substring(0,ea).Trim();string be=eB.Substring(ea+1).Trim();if(c6.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cm=be.Equals("red",StringComparison.OrdinalIgnoreCase)||be=="1"||be.Equals("true",StringComparison.OrdinalIgnoreCase)||be.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double n_;cP.Remove(c6);if(double.TryParse(be,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out n_)){
aG[c6]=n_;}else if(be.Equals("true",StringComparison.OrdinalIgnoreCase)||be.Equals("on",StringComparison.OrdinalIgnoreCase)){aG[c6]=100.0;
}else if(be.Equals("false",StringComparison.OrdinalIgnoreCase)||be.Equals("off",StringComparison.OrdinalIgnoreCase)){aG[c6]=0.0;}else{cP[c6]=be;
}}}}}const string qF="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void pR(){var h_=new List<IMyTextSurface>();if(aT!=null)h_.Add(aT);foreach(var sJ in bH.Values)h_.Add(sJ);foreach(var tA in h_)pQ(tA);
}static void bi(List<MySprite>tG,float rz,float rA,float ue,float r2,Color rm){tG.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(rz,rA),Size=new Vector2(ue,r2),Color=rm}
);}void pQ(IMyTextSurface km){Vector2 aJ=km.SurfaceSize,aD=km.TextureSize,ak=(aD-aJ)/2f;var aC=new List<MySprite>();Color fZ=new Color(150,150,150);
bi(aC,aD.X/2f,2f,aD.X,4f,fZ);bi(aC,aD.X/2f,aD.Y-2f,aD.X,4f,fZ);bi(aC,2f,aD.Y/2f,4f,aD.Y,fZ);bi(aC,aD.X-2f,aD.Y/2f,4f,aD.Y,fZ);
bi(aC,ak.X+aJ.X/2f,ak.Y+aJ.Y/2f,aJ.X,aJ.Y,new Color(20,40,90));float aj=32f;bi(aC,ak.X+aj/2f,ak.Y+aj/2f,aj,aj,new Color(255,0,0));bi(aC,ak.X+aJ.X-aj/2f,ak.Y+aj/2f,aj,aj,new Color(0,255,0));
bi(aC,ak.X+aj/2f,ak.Y+aJ.Y-aj/2f,aj,aj,new Color(255,255,0));bi(aC,ak.X+aJ.X-aj/2f,ak.Y+aJ.Y-aj/2f,aj,aj,new Color(255,0,255));float kT=ak.Y+aJ.Y/2f-30f;
aY(aC,"SURFACE "+aJ.X.ToString("0")+"x"+aJ.Y.ToString("0"),Color.White,ak.X+44f,kT,0.9f);aY(aC,"TEXTURE "+aD.X.ToString("0")+"x"+aD.Y.ToString("0"),Color.White,ak.X+44f,kT+24f,0.9f);
aY(aC,"OFFSET  "+ak.X.ToString("0")+","+ak.Y.ToString("0"),Color.White,ak.X+44f,kT+48f,0.9f);using(var rP=km.DrawFrame()){rP.AddRange(aC);}}static bool lm(string c1){
return c1.Length>3&&(c1.StartsWith("top")&&char.IsDigit(c1[3])||c1.Length>4&&c1.StartsWith("line")&&char.IsDigit(c1[4]));}void pX(){fs.Clear();cR.Clear();
ft.Clear();bJ= -1;hS=hT=0;foreach(var kh in q.S){var jx=new List<List<bA>>();foreach(var s4 in kh.ah)jx.Add(s4.at);
if(kh.ah.Count==0)jx.Add(kh.fe);foreach(var sE in jx)foreach(var et in sE){if(!lm(et.I))continue;bool o9=et.I[0]=='t';var mI=o9 ? fs : ft;
int sF=int.Parse(et.I.Substring(o9 ? 3 : 4));int np;mI.TryGetValue(et.aN,out np);mI[et.aN]=Math.Max(np,sF);}}cR.AddRange(fs.Keys);
}void qD(){if(ft.Count==0||Z<hT)return;hT=Z+2.0;foreach(var jo in ft){List<IMyTerminalBlock>ie;var oI=bG.TryGetValue(jo.Key,out ie)&&ie.Count>0 ? ie[0]as IMyTextSurface : null;
string[]nP=oI==null ? new string[0]: oI.GetText().Split('\n');for(int eE=1;eE<=jo.Value;eE++)cP[jo.Key+".line"+eE]=eE<=nP.Length ? nP[eE-1].TrimEnd('\r'): "";}}
void qE(){if(cR.Count==0)return;if(bJ<0){if(Z<hS)return;bJ=0;fr=0;d_.Clear();}while(bJ<cR.Count&&Runtime.CurrentInstructionCount<20000){
string ii=cR[bJ];List<IMyTerminalBlock>fD;bG.TryGetValue(ii,out fD);if(fD!=null&&fr<fD.Count){var l_=fD[fr++];
for(int i4=0;i4<l_.InventoryCount;i4++){hO.Clear();l_.GetInventory(i4).GetItems(hO);foreach(var f9 in hO){string jk=f9.Type.TypeId.Replace("MyObjectBuilder_","");
string nz=jk=="Ore"||jk=="Ingot" ? f9.Type.SubtypeId+" "+jk.ToLowerInvariant(): f9.Type.SubtypeId;double nq;d_.TryGetValue(nz,out nq);d_[nz]=nq+(double)f9.Amount;
}}continue;}var gx=new List<KeyValuePair<string,double>>(d_);gx.Sort((ug,uk)=>uk.Value.CompareTo(ug.Value));for(int de=1;de<=fs[ii];de++)cP[ii+".top"+de]=de<=gx.Count ? gx[de-1].Key+" "+H.ld(gx[de-1].Value,"auto"): "-";
d_.Clear();bJ++;fr=0;}if(bJ>=cR.Count){bJ= -1;hS=Z+15.0;}}void ag(int tQ,string tY){cO.Add(new KeyValuePair<string,int>(tY,tQ));
}static string hI(string ka,int nV){return ka.Length>nV ? ka.Substring(0,nV): ka;}void lk(){var j2=Me as IMyTextSurfaceProvider;if(j2==null||j2.SurfaceCount<1)return;
aT=j2.GetSurface(0);aT.ContentType=hK ? ContentType.SCRIPT : ContentType.NONE;aT.Script="";aT.ScriptBackgroundColor=Color.Black;
}void hJ(){if(Z>=fo){fo=Z+(dW>q.S.Count+1 ? 5.0 : 1.0);pF();if(hK&&aT!=null&&dY==0)pP();
}Echo(lL);}void pF(){cO.Clear();int il=0,mf=0;foreach(var l0 in q.b7){List<IMyTerminalBlock>jr;if(l0.bz==a_.dA){
il++;continue;}if(bG.TryGetValue(l0.ac,out jr)){mf+=jr.Count;if(jr.Count>0)il++;}}int mU=q.a1.Count;ag(0,"HELM-[Kernel-(v"+p1+")]");
ag(4,"------------------------------");ag(1,"krn-cfg");ag(2," screens "+q.S.Count+"  binds "+il+"/"+q.b7.Count+"  errors "+mU);
ag(2," alert "+(cm ? "RED" : "off")+"  blocks "+mf+"  docked-ignored "+hM);if(mU>0)ag(3," ! "+hI(q.a1[0],40));
else if(q.dS.Count>0)ag(3," ? "+hI(q.dS[0],40));ag(1,"cfg-dsp");if(q.S.Count==0)ag(3," no @screen: run 'starter'");
int tC=0;foreach(var kb in q.S){if(++tC>10){ag(2," +"+(q.S.Count-10)+" more");break;}IMyTextSurface kn;string oK="--",ks="no block";
if(bH.TryGetValue(kb,out kn)){oK=kn.SurfaceSize.X.ToString("0")+"x"+kn.SurfaceSize.Y.ToString("0");ks=bj.ContainsKey(kb)? "boot" : "live";
}ag(ks=="no block" ? 3 : 2," "+hI(kb.a4,14).PadRight(14)+" "+oK.PadRight(9)+" "+ks);}ag(1,"perf");ag(2," tick "+dZ+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ag(2," instr "+fq+"  peak "+hN+"  max "+Runtime.MaxInstructionCount);ag(2," rate "+Runtime.UpdateFrequency+"  idle "+dW+"  up "+(int)Z+"s");
if(hQ.Length>0)ag(2," last "+hQ);var oA=new StringBuilder();foreach(var si in cO)oA.AppendLine(si.Key);lL=oA.ToString();}static Color pL(int tR){
switch(tR){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void pP(){Vector2 cz=aT.SurfaceSize;Vector2 jE=(aT.TextureSize-cz)/2f;int iv=30;foreach(var sj in cO)iv=Math.Max(iv,sj.Key.Length);
Vector2 iu=aT.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float oB=Math.Min(1f,Math.Min(cz.X*0.94f/(iv*iu.X),cz.Y*0.94f/(cO.Count*iu.Y)));
float uh=jE.X+cz.X*0.03f;float pu=jE.Y+cz.Y*0.03f;var ko=new List<MySprite>();ko.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jE+cz/2f,Size=cz,Color=new Color(12,12,16)}
);foreach(var nE in cO){aY(ko,nE.Key,pL(nE.Value),uh,pu,oB);pu+=iu.Y*oB;}using(var rQ=aT.DrawFrame()){rQ.AddRange(ko);}}static readonly List<string>pE=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void hE(){bj.Clear();if(q==null)return;for(int f2=0;f2<q.S.Count;f2++){var gD=q.S[f2];if(gD.he>0f)bj[gD]=Z+(f2/Math.Max(1,gD.kZ))*gD.k1/60.0;
}}void qq(){List<z>d9=null;int r1=(bj.Count+3)/4,sb=0;foreach(var gd in bj){double jM=(Z-gd.Value)/gd.Key.he;if(jM<0)continue;
if(jM>=1.0){if(d9==null)d9=new List<z>();d9.Add(gd.Key);continue;}if((sb++ +dZ)%r1==0)pO(gd.Key,jM);}if(d9==null)return;foreach(var eK in d9){
bj.Remove(eK);if(eK.hd>0f)fn[eK]=Z;bI.Remove(eK);cn.Remove(eK);}}static void aY(List<MySprite>tH,string tZ,Color ro,float ui,float ul,float tx){
var oY=MySprite.CreateText(tZ,"Monospace",ro,tx,TextAlignment.LEFT);oY.Position=new Vector2(ui,ul);tH.Add(oY);}static void kU(List<MySprite>tI,List<string>fy,z gG,float uj,float um,float ty,float su){
string[]tO=gG.e2!=null&&gG.e2.Length>0 ? gG.e2 : new[]{gG.k_};for(int ei=0;ei<fy.Count;
ei++){n io=H.lg(tO,fy.Count>1 ? 100.0*ei/(fy.Count-1): 0.0);aY(tI,fy[ei],new Color(io.bC,io.bx,io.bs),uj,um+ei*su,ty);}}bool qu(z gH,IMyTextSurface kw){
aA cB,aV;z oE=q.S.Find(ps=>ps!=gH&&ps.a4.Equals(gH.hv,StringComparison.OrdinalIgnoreCase));if(oE==null|| !bI.TryGetValue(oE,out cB)|| !cB.dF)return false;
if(!bI.TryGetValue(gH,out aV))bI[gH]=aV=new aA();Vector2 di=kw.SurfaceSize;if(aV.dF&&aV.dR==cB.dR&&aV.dG==di)return false;
aV.dF=true;aV.dR=cB.dR;aV.dG=di;aV.f.Clear();Vector2 kC=cB.dG,sL=(kw.TextureSize-di)/2f;float gb=Math.Min(di.X/kC.X,di.Y/kC.Y);
Vector2 qZ=sL+(di-kC*gb)/2f;foreach(var tJ in cB.f){var bm=tJ;if(bm.Position.HasValue)bm.Position=(bm.Position.Value-cB.lq)*gb+qZ;if(bm.Size.HasValue)bm.Size=bm.Size.Value*gb;
if(bm.Type==SpriteType.TEXT)bm.RotationOrScale*=gb;aV.f.Add(bm);}using(var rR=kw.DrawFrame())rR.AddRange(aV.f);return true;}bool qo(z dg,IMyTextSurface gO){
List<string>d1;if(cn.ContainsKey(dg)|| !q.ds.TryGetValue(dg.b3,out d1))return false;cn[dg]="banner";Vector2 cA=gO.SurfaceSize;
Vector2 jF=(gO.TextureSize-cA)/2f;int fJ=1;foreach(var sk in d1)fJ=Math.Max(fJ,sk.Length);Vector2 d7=gO.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float gF=Math.Min(cA.X*0.94f/(fJ*d7.X),cA.Y*0.94f/(d1.Count*d7.Y));var kp=new List<MySprite>();kp.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jF+cA/2f,Size=cA,Color=string.IsNullOrEmpty(dg.bt)? new Color(10,12,16): F(dg.bt)}
);kU(kp,d1,dg,jF.X+(cA.X-fJ*d7.X*gF)/2f,jF.Y+(cA.Y-d1.Count*d7.Y*gF)/2f,gF,d7.Y*gF);using(var rS=gO.DrawFrame()){rS.AddRange(kp);
}return true;}void pO(z dh,double n4){IMyTextSurface eM;if(!bH.TryGetValue(dh,out eM))return;Vector2 b_=eM.SurfaceSize;
Vector2 jG=(eM.TextureSize-b_)/2f;var eu=dh.pY();string nT=dh.k0 ?? "HELM TELEMETRY KERNEL ONLINE";bool jy=b_.Y>=200f;
List<string>cS;if(dh.b8==null|| !q.ds.TryGetValue(dh.b8,out cS))cS=pE;int cZ=nT.Length+3;foreach(var sl in eu)cZ=Math.Max(cZ,sl.Length+5);
if(jy)foreach(var sm in cS)cZ=Math.Max(cZ,sm.Length);int os=(jy ? cS.Count+1 : 0)+2+eu.Count;Vector2 cY=eM.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bW=Math.Min(1f,Math.Min(b_.X*0.94f/(cZ*cY.X),b_.Y*0.94f/(os*cY.Y)));float g3=jG.X+(b_.X-cZ*cY.X*bW)/2f;float cF=jG.Y+(b_.Y-os*cY.Y*bW)/2f;
float gh=cY.Y*bW;var dj=new List<MySprite>();dj.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jG+b_/2f,Size=b_,Color=new Color(10,12,16)}
);if(jy){kU(dj,cS,dh,g3,cF,bW,gh);cF+=(cS.Count+1)*gh;}aY(dj,">> "+nT,new Color(34,197,94),g3,cF,bW);cF+=gh*2f;int tD=H.pD(n4,eu.Count);
for(int f3=0;f3<tD;f3++){bool n1=f3==eu.Count-1&&n4>=0.8;aY(dj,n1 ? "[OK]" : "[..]",n1 ? new Color(34,197,94): new Color(56,189,248),g3,cF,bW);aY(dj,eu[f3],new Color(203,213,225),g3+5f*cY.X*bW,cF,bW);
cF+=gh;}using(var rT=eM.DrawFrame()){rT.AddRange(dj);}}void hC(){bG.Clear();bH.Clear();fp.Clear();
dX.Clear();fm=false;if(q==null)return;foreach(var ab in q.b7){var aU=new List<IMyTerminalBlock>();if(ab.bz==a_.dA){
string ji= !string.IsNullOrEmpty(ab.bh)? ab.bh : ab.ac;if(!aG.ContainsKey(ji)){aG[ji]=ab.e6;if(ab.hh!=null)cP[ji]=ab.hh;
}bG[ab.ac]=aU;continue;}var o1=ab.aR.Count>0 ? ab.aR : new List<string>{ab.lC};foreach(var cD in o1){
if(string.IsNullOrEmpty(cD))continue;bool nx=cD.Contains("*")||cD.Contains("?");if(ab.bz==a_.hr||nx){var iZ=new List<IMyTerminalBlock>();
pI(cD,iZ,ab.e_);if(iZ.Count>0){foreach(var nh in iZ){if(!aU.Contains(nh))aU.Add(nh);}}else{var lT=eZ(ab.e_);
for(int h8=0;h8<lT.Count;h8++){var gV=lT[h8];if(nx ? H.p6(cD,gV.CustomName): string.Equals(cD,gV.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aU.Contains(gV))aU.Add(gV);}}}}else{var id=k9(cD,ab.e_);if(id!=null&& !aU.Contains(id))aU.Add(id);}}if(aU.Count==0){
qA(ab,o1,aU);if(aU.Count>0)fp[ab.ac]=true;}bG[ab.ac]=aU;}foreach(var bp in q.S){
if(string.IsNullOrEmpty(bp.bB))continue;var jJ=k9(bp.bB);if(jJ==null){dX.AppendLine($"HELM: output block '{bp.bB}' not found");
continue;}IMyTextSurface cC=null;var j3=jJ as IMyTextSurfaceProvider;var mJ=jJ as IMyTextSurface;if(j3!=null&&bp.ce<j3.SurfaceCount){
cC=j3.GetSurface(bp.ce);}else if(mJ!=null&&bp.ce==0){cC=mJ;}if(cC==null){dX.AppendLine($"HELM: output block '{bp.bB}' has no surface index {bp.ce}");
continue;}cC.ContentType=ContentType.SCRIPT;cC.Script="";cC.ScriptBackgroundColor=Color.Black;bH[bp]=cC;}cl.Clear();
cQ.Clear();}static readonly string[]k8={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool pW(int sc,IMyTerminalBlock u){switch(sc){case 0: return u is IMyBatteryBlock;case 1: return u is IMyReactor;case 2: return u is IMySolarPanel;
case 3: return u is IMyWindTurbine;case 4: return u is IMyGasTank;case 5: return u is IMyAirVent;case 6: return u is IMyCargoContainer;case 7: return u is IMyJumpDrive;
case 8: return u is IMyThrust;case 9: return u is IMyMotorSuspension;case 10: return u is IMyDoor;case 11: return u is IMyShipConnector;case 12: return u is IMyAssembler;
case 13: return u is IMyRefinery;case 14: return u is IMyShipDrill;case 15: return u is IMyShipWelder;case 16: return u is IMyShipGrinder;case 17: return u is IMyGasGenerator;
case 18: return u is IMyLargeTurretBase;case 19: return u is IMyProjector;case 20: return u is IMyGyro;case 21: return u is IMyGravityGeneratorBase;case 22: return u is IMyLandingGear;
case 23: return u is IMyRadioAntenna;case 24: return u is IMyBeacon;case 25: return u is IMyLightingBlock;case 26: return u is IMyPistonBase;case 27: return u is IMyMotorStator;
case 28: return u is IMyWarhead;case 29: return u is IMySensorBlock;case 30: return u is IMyCameraBlock;case 31: return u is IMyParachute;case 32: return u is IMyTurretControlBlock;
}return false;}void qA(bg rd,List<string>tW,List<IMyTerminalBlock>ig){string r6=(rd.ac+" "+string.Join(" ",tW)).ToLowerInvariant();
var jm=new List<int>();for(int gc=0;gc<k8.Length;gc++){foreach(var uf in k8[gc].Split(','))if(r6.Contains(uf)){jm.Add(gc);break;}}if(jm.Count==0)return;
var lS=eZ();var n2=new List<IMyTerminalBlock>();for(int i5=0;i5<lS.Count;i5++){var h9=lS[i5];foreach(int sd in jm){if(pW(sd,h9)){(h9.CubeGrid==Me.CubeGrid ? ig : n2).Add(h9);
break;}}}if(ig.Count==0)ig.AddRange(n2);}void qC(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(q==null||q.S.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {q.S.Count} screens, {q.b7.Count} binds");fm=false;
Echo($"Blocks: {eZ().Count} on this construct; {hM} on docked ships (ignored)");cl.Clear();Echo("--- BIND RESOLUTION ---");foreach(var an in q.b7){
if(an.bz==a_.dA){string jj= !string.IsNullOrEmpty(an.bh)? an.bh : an.ac;double t5=aG.ContainsKey(jj)? aG[jj]: an.e6;
Echo($"[OK] @bind {an.ac} (dynamic variable '{jj}' = {t5})");continue;}List<IMyTerminalBlock>ih;if(bG.TryGetValue(an.ac,out ih)){if(ih.Count>0){
string rL=fp.ContainsKey(an.ac)&&fp[an.ac]? " [fallback: typed match]" : "";Echo($"[OK] @bind {an.ac} ({ih.Count} blocks matched{rL})");
}else{string tX=string.Join(", ",an.aR.Count>0 ? an.aR : new List<string>{an.lC});Echo($"[WARN] @bind {an.ac} -> 0 blocks matched \"{tX}\"");
}}else{Echo($"[ERR] @bind {an.ac} -> unresolved");}}if(aG.Count>0){Echo($"--- DYNAMIC VARIABLES ({aG.Count}) ---");foreach(var nG in aG)Echo($"[VAR] {nG.Key} = {nG.Value}");
}IMyTextSurface h0=null;foreach(var sN in bH.Values){h0=sN;break;}if(h0!=null){var kq=new List<string>();h0.GetSprites(kq);
Echo($"--- SPRITES ({kq.Count}): names usable in bg_texture ---");Echo(string.Join(", ",kq.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bq in q.S){IMyTextSurface gP;if(bH.TryGetValue(bq,out gP)){var oL=gP.SurfaceSize;var tM=H.pK(bq);
Echo($"[OK] Screen \"{bq.a4}\" [{bq.cf}] -> {bq.bB} [#{bq.ce}] ({oL.X:0}x{oL.Y:0} px, texture {gP.TextureSize.X:0}x{gP.TextureSize.Y:0}, {tM.ad} sprites)");
}else{Echo($"[ERR] Screen \"{bq.a4}\" -> Missing block \"{bq.bB}\" [#{bq.ce}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double qB(IMyTerminalBlock v){
if(v==null)return 0.0;var fB=v as IMyBatteryBlock;if(fB!=null)return fB.MaxStoredPower>0 ?(fB.CurrentStoredPower/fB.MaxStoredPower)*100.0 : 0.0;
var ga=v as IMyJumpDrive;if(ga!=null)return ga.MaxStoredPower>0 ?(ga.CurrentStoredPower/ga.MaxStoredPower)*100.0 : 0.0;var o0=v as IMyGasTank;
if(o0!=null)return o0.FilledRatio*100.0;var pm=v as IMyAirVent;if(pm!=null)return pm.GetOxygenLevel()*100.0;var mn=v as IMyCargoContainer;if(mn!=null){
var f7=mn.GetInventory();if(f7!=null&&f7.MaxVolume.RawValue>0)return(double)f7.CurrentVolume.RawValue/f7.MaxVolume.RawValue*100.0;return 0.0;}var gy=v as IMyReactor;
if(gy!=null)return gy.MaxOutput>0 ?(gy.CurrentOutput/gy.MaxOutput)*100.0 : 0.0;var gI=v as IMySolarPanel;if(gI!=null)return gI.MaxOutput>0 ?(gI.CurrentOutput/gI.MaxOutput)*100.0 : 0.0;
var g2=v as IMyWindTurbine;if(g2!=null)return g2.MaxOutput>0 ?(g2.CurrentOutput/g2.MaxOutput)*100.0 : 0.0;var mK=v as IMyDoor;if(mK!=null)return(double)mK.OpenRatio*100.0;
var gY=v as IMyThrust;if(gY!=null)return gY.MaxEffectiveThrust>0 ?(gY.CurrentThrust/gY.MaxEffectiveThrust)*100.0 : 0.0;var kB=v as IMyMotorSuspension;
if(kB!=null)return kB.Brake ? 100.0 :(kB.Friction*100.0);var iz=v as IMyShipConnector;if(iz!=null)return iz.Status==MyShipConnectorStatus.Connected ? 100.0 :(iz.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var oi=v as IMyProductionBlock;if(oi!=null)return oi.IsProducing ? 100.0 : 0.0;var mO=v as IMyShipDrill;if(mO!=null)return mO.Enabled ? 100.0 : 0.0;
var kH=v as IMyShipToolBase;if(kH!=null)return kH.IsActivated ? 100.0 :(kH.Enabled ? 50.0 : 0.0);var iW=v as IMyGasGenerator;if(iW!=null)return iW.AutoRefill ? 100.0 :(iW.Enabled ? 50.0 : 0.0);
var gZ=v as IMyLargeTurretBase;if(gZ!=null)return gZ.IsShooting ? 100.0 :(gZ.HasTarget ? 50.0 :(gZ.Enabled ? 10.0 : 0.0));var eD=v as IMyProjector;
if(eD!=null)return eD.TotalBlocks>0 ?((double)(eD.TotalBlocks-eD.RemainingBlocks)/eD.TotalBlocks)*100.0 : 0.0;var i1=v as IMyGyro;if(i1!=null)return i1.GyroOverride ? 100.0 :(i1.Enabled ? 50.0 : 0.0);
var nj=v as IMyGravityGeneratorBase;if(nj!=null)return Math.Min(100.0,Math.Abs(nj.GravityAcceleration)/9.81*100.0);var iV=v as IMyLandingGear;if(iV!=null)return iV.IsLocked ? 100.0 :(iV.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var lU=v as IMyRadioAntenna;if(lU!=null)return(lU.Radius/50000.0)*100.0;var l6=v as IMyBeacon;if(l6!=null)return(l6.Radius/50000.0)*100.0;
var nN=v as IMyLightingBlock;if(nN!=null)return nN.Intensity*10.0;var gu=v as IMyPistonBase;if(gu!=null)return gu.HighestPosition>0 ?(gu.CurrentPosition/gu.HighestPosition)*100.0 : 0.0;
var op=v as IMyMotorStator;if(op!=null)return Math.Min(100.0,Math.Abs(op.Angle)/6.28318*100.0);var pp=v as IMyWarhead;if(pp!=null)return pp.IsArmed ? 100.0 : 0.0;
var kk=v as IMySensorBlock;if(kk!=null)return!kk.LastDetectedEntity.IsEmpty()? 100.0 :(kk.Enabled ? 25.0 : 0.0);var ir=v as IMyCameraBlock;
if(ir!=null)return ir.IsActive ? 100.0 :(ir.Enabled ? 50.0 : 0.0);var mt=v as IMyParachute;if(mt!=null)return(double)mt.OpenRatio*100.0;
var gX=v as IMyTurretControlBlock;if(gX!=null)return gX.HasTarget ? 100.0 :(gX.IsUnderControl ? 50.0 :(gX.Enabled ? 10.0 : 0.0));var m8=v as IMyFunctionalBlock;
if(m8!=null)return m8.Enabled ? 100.0 : 0.0;return 0.0;}static double p3(IMyTerminalBlock l1,string iO){int mm=iO.IndexOf(':');string jl=iO.Substring(0,mm).ToLowerInvariant();
var t3=new MyItemType("MyObjectBuilder_"+(jl=="ammo" ? "AmmoMagazine" : char.ToUpper(jl[0])+jl.Substring(1)),iO.Substring(mm+1));double oU=0;for(int i6=0;
i6<l1.InventoryCount;i6++)oU+=(double)l1.GetInventory(i6).GetItemAmount(t3);return oU;}double lv(IMyTerminalBlock w,string m_){if(w==null)return 0.0;
string b=m_.ToLowerInvariant();if(b=="percent"||b=="pct")return qB(w);if(b.IndexOf(':')>0)return p3(w,m_);var bM=w as IMyBatteryBlock;
if(bM!=null){if(b=="current"||b=="stored"||b=="val"||b=="value"||b=="raw")return bM.CurrentStoredPower;if(b=="max"||b=="capacity")return bM.MaxStoredPower;
if(b=="output")return bM.CurrentOutput;if(b=="maxoutput")return bM.MaxOutput;if(b=="input")return bM.CurrentInput;if(b=="maxinput")return bM.MaxInput;
return bM.CurrentStoredPower;}var jh=w as IMyJumpDrive;if(jh!=null){if(b=="max"||b=="capacity")return jh.MaxStoredPower;return jh.CurrentStoredPower;
}var gS=w as IMyGasTank;if(gS!=null){if(b=="max"||b=="capacity")return gS.Capacity;return gS.FilledRatio*gS.Capacity;}var pn=w as IMyAirVent;
if(pn!=null){if(b=="max"||b=="capacity")return 100.0;return pn.GetOxygenLevel()*100.0;}var mo=w as IMyCargoContainer;if(mo!=null){var f8=mo.GetInventory();
if(f8!=null){if(b=="mass")return(double)f8.CurrentMass;if(b=="max"||b=="capacity")return(double)f8.MaxVolume;return(double)f8.CurrentVolume;}return 0.0;}var j4=w as IMyPowerProducer;
if(j4!=null){if(b=="max"||b=="capacity")return j4.MaxOutput;return j4.CurrentOutput;}var mL=w as IMyDoor;if(mL!=null){if(b=="max")return 1.0;return(double)mL.OpenRatio*100.0;
}var kF=w as IMyThrust;if(kF!=null){if(b=="max"||b=="capacity")return kF.MaxEffectiveThrust;return kF.CurrentThrust;}var b1=w as IMyMotorSuspension;
if(b1!=null){if(b=="brake")return b1.Brake ? 100.0 : 0.0;if(b=="height")return b1.Height;if(b=="strength")return b1.Strength;if(b=="power")return b1.Power;
if(b=="steer")return b1.SteeringOverride*100.0;if(b=="propulsion")return b1.PropulsionOverride*100.0;return b1.Friction;}var mA=w as IMyShipConnector;
if(mA!=null){return mA.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var j1=w as IMyProductionBlock;if(j1!=null){if(b=="producing"||b=="active"||b=="running"||b=="state")return j1.IsProducing ? 100.0 : 0.0;
if(b=="queue")return j1.IsQueueEmpty ? 0.0 : 1.0;}var h4=w as IMyAssembler;if(h4!=null){if(b=="disassembling")return h4.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(b=="repeating"||b=="loop")return h4.Repeating ? 100.0 : 0.0;}var iX=w as IMyGasGenerator;if(iX!=null){if(b=="autorefill")return iX.AutoRefill ? 100.0 : 0.0;
if(b=="active")return iX.Enabled ? 100.0 : 0.0;}var g_=w as IMyLargeTurretBase;if(g_!=null){if(b=="shooting")return g_.IsShooting ? 100.0 : 0.0;
if(b=="target"||b=="hastarget")return g_.HasTarget ? 100.0 : 0.0;if(b=="range")return g_.Range;}var bV=w as IMyProjector;if(bV!=null){if(b=="built")return bV.TotalBlocks-bV.RemainingBlocks;
if(b=="remaining")return bV.RemainingBlocks;if(b=="total")return bV.TotalBlocks;if(b=="projecting")return bV.IsProjecting ? 100.0 : 0.0;if(b=="armor")return bV.RemainingArmorBlocks;
if(b=="buildable")return bV.BuildableBlocksCount;}var kI=w as IMyShipToolBase;if(kI!=null){if(b=="working"||b=="active"||b=="activated")return kI.IsActivated ? 100.0 : 0.0;
return kI.Enabled ? 100.0 : 0.0;}var c3=w as IMyGyro;if(c3!=null){if(b=="override")return c3.GyroOverride ? 100.0 : 0.0;if(b=="pitch")return c3.Pitch;
if(b=="yaw")return c3.Yaw;if(b=="roll")return c3.Roll;if(b=="power")return c3.GyroPower*100.0;}var nk=w as IMyGravityGeneratorBase;if(nk!=null){if(b=="g"||b=="accel")return nk.GravityAcceleration;
}var ni=w as IMyLandingGear;if(ni!=null){if(b=="locked")return ni.IsLocked ? 100.0 : 0.0;}var lV=w as IMyRadioAntenna;if(lV!=null){if(b=="radius"||b=="range")return lV.Radius;
}var l7=w as IMyBeacon;if(l7!=null){if(b=="radius"||b=="range")return l7.Radius;}var ju=w as IMyLightingBlock;if(ju!=null){if(b=="intensity")return ju.Intensity;
if(b=="radius")return ju.Radius;}var gv=w as IMyPistonBase;if(gv!=null){if(b=="pos"||b=="position"||b=="extension")return gv.CurrentPosition;
if(b=="velocity")return gv.Velocity;if(b=="max")return gv.HighestPosition;}var eJ=w as IMyMotorStator;if(eJ!=null){if(b=="angle"||b=="deg")return eJ.Angle*180.0/Math.PI;
if(b=="rad")return eJ.Angle;if(b=="velocity"||b=="rpm")return eJ.TargetVelocityRPM;if(b=="locked")return eJ.RotorLock ? 100.0 : 0.0;}var kR=w as IMyWarhead;
if(kR!=null){if(b=="armed")return kR.IsArmed ? 100.0 : 0.0;if(b=="countdown")return kR.DetonationTime;}var oH=w as IMySensorBlock;if(oH!=null){
if(b=="detected")return!oH.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var it=w as IMyCameraBlock;if(it!=null){if(b=="range")return it.AvailableScanRange;
if(b=="active")return it.IsActive ? 100.0 : 0.0;}var mu=w as IMyParachute;if(mu!=null){if(b=="open"||b=="openratio")return(double)mu.OpenRatio*100.0;
}var dn=w as IMyTurretControlBlock;if(dn!=null){if(b=="target"||b=="hastarget")return dn.HasTarget ? 100.0 : 0.0;if(b=="azimuth")return dn.AzimuthRotor!=null ? dn.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(b=="elevation")return dn.ElevationRotor!=null ? dn.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var m9=w as IMyFunctionalBlock;if(m9!=null){return m9.Enabled ? 100.0 : 0.0;
}return 0.0;}double cg(string d4,string m0){bg bN;dT.TryGetValue(d4,out bN);if(bN!=null&&bN.bz==a_.dA){
string se= !string.IsNullOrEmpty(bN.bh)? bN.bh : d4;double mD;if(aG.TryGetValue(se,out mD))return mD;return bN.e6;
}double mR;if(aG.TryGetValue(d4,out mR))return mR;List<IMyTerminalBlock>d5;if(!bG.TryGetValue(d4,out d5)||d5.Count==0)throw new InvalidOperationException($"HELM: bind '{d4}' resolved to zero blocks — check the name/typo");
if(d5.Count==1)return lv(d5[0],m0);var pl=new List<double>();foreach(var q2 in d5)pl.Add(lv(q2,m0));return pz.py(bN!=null ? bN.dr : aE.g6,pl);
}double hD(string rf){return cg(rf,"percent");}static string a2(double ph,bA c9){if(c9==null||(string.IsNullOrEmpty(c9.dQ)&&string.IsNullOrEmpty(c9.dB)&&c9.I=="percent")){
return le(ph);}return H.ld(ph,c9.dQ,c9.dB);}bool qv(z bX){IMyTextSurface eN;if(!bH.TryGetValue(bX,out eN))return false;
if(bX.b3!=null)return qo(bX,eN);if(bX.hv!=null)return qu(bX,eN);if(bX.dw.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return qw(bX,eN);}return qx(bX,eN);}Color lf(string[]dk,double sW){if(dk.Length==1)return F(dk[0]);
double oj=Math.Max(0.0,Math.Min(100.0,sW))/100.0*(dk.Length-1);int i7=Math.Min((int)oj,dk.Length-2);float gQ=(float)(oj-i7);Color bK=F(dk[i7]),fA=F(dk[i7+1]);
return new Color((int)(bK.R+(fA.R-bK.R)*gQ),(int)(bK.G+(fA.G-bK.G)*gQ),(int)(bK.B+(fA.B-bK.B)*gQ),(int)(bK.A+(fA.A-bK.A)*gQ));}static Color[]eW(aA rk,string[]tP,string rC,float pt,float pv,float kO,float i2){
int d2=16;bool nr=rC=="horizontal";var iy=new Color[d2];for(int ct=0;ct<d2;ct++){n iU=H.lg(tP,100.0*ct/(d2-1));iy[ct]=new Color(iU.bC,iU.bx,iU.bs);
float ml=kO/d2,me=i2/d2;t(rk,nr ? new Vector2(pt+ml*(ct+0.5f),pv+i2/2f): new Vector2(pt+kO/2f,pv+me*(ct+0.5f)),nr ? new Vector2(ml+1f,i2): new Vector2(kO,me+1f),iy[ct]);
}return iy;}static void t(aA rl,Vector2 s6,Vector2 tE,Color rp,float to=0f,string rB="SquareSimple"){rl.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=rB,Position=s6,Size=tE,Color=rp,RotationOrScale=to}
);}void pG(z k,IMyTextSurface kx,aA g){g.f.Clear();g.dM.Clear();Vector2 O=kx.SurfaceSize;
Vector2 t0=kx.TextureSize;Vector2 aw=(t0-O)/2f;g.lq=aw;float gn=Math.Max(4f,O.X*0.02f);
float cv=Math.Max(4f,O.Y*0.02f);if(k.cf=="compact"||(k.cf=="auto"&&O.Y<=260f&&O.Y>150f)){gn=4f;cv=4f;
}else if(k.cf=="corner"||(k.cf=="auto"&&O.Y<=150f)){gn=2f;cv=2f;}float eS=O.X-(gn*2f);Color cU=new Color(10,12,16);
bool mN=true;if(!string.IsNullOrEmpty(k.bt)){var eg=n.dJ(k.bt);if(eg.fb)mN=false;else cU=new Color(eg.bC,eg.bx,eg.bs,eg.g4);
}g.b6= -1;g.eY=false;g.eX=F(k.kW ?? "#5b0f0f");g.dv.Clear();if(k.bv!=null&&k.bv.Length>1){
g.b6=g.f.Count;g.e0=eW(g,k.bv,k.du,aw.X+2f,aw.Y+2f,O.X-4f,O.Y-4f);
}else if(mN||k.aM!=null||k.cJ!=null){if(k.aM!=null)cU=F(k.aM.First(tb=>tb.cL).ca);g.b6=g.f.Count;
g.e0=new[]{cU};t(g,aw+(O/2f),O-new Vector2(4f,4f),cU);if(k.aM!=null||k.cJ!=null)g.dv.Add(new e1{
fa=g.b6,g7=cU,e5=cU,fi=k.aM,hb=k.g8,hk=k.g9,e8=k.cJ,hl=k.hm,hn=k.ho,ll=true}
);}if(!string.IsNullOrEmpty(k.ha))t(g,aw+(O/2f),O-new Vector2(4f,4f),F(k.kY ?? "255,255,255,24"),0f,k.ha);
if(!k.dw.Equals("none",StringComparison.OrdinalIgnoreCase)){Color mg=k.ax!=null&&k.ax.Length>0 ? F(k.ax[0]): F(k.e4 ?? "40, 60, 85");
Color rj=k.ax!=null&&k.ax.Length>1 ? F(k.ax[k.ax.Length-1]): mg;
float mh=aw.X+(O.X-eS)/2f;if(k.ax!=null&&k.ax.Length>1&& !k.hq){eW(g,k.ax,"horizontal",mh,aw.Y+cv*0.5f-1f,eS,2f);
eW(g,k.ax,"horizontal",mh,aw.Y+O.Y-(cv*0.5f)-1f,eS,2f);}else{t(g,new Vector2(aw.X+O.X/2f,aw.Y+cv*0.5f),new Vector2(eS,2f),mg);
t(g,new Vector2(aw.X+O.X/2f,aw.Y+O.Y-(cv*0.5f)),new Vector2(eS,2f),rj);}}var jR=k.ah;
if(jR.Count==0){jR=new List<az>{new az{dI="main",dy=0,dL=0,dN=k.dC,dO=k.dD,at=k.fe}};}float nm=8f;
foreach(var s in jR){var gm=H.pJ(s.dy,s.dL,s.dN,s.dO,k.dC,k.dD,O.X,O.Y,gn,cv,nm,nm);
float aW=aw.X+gm.lI;float dc=aw.Y+gm.lJ;float a8=gm.lH;float eA=gm.li;bool ot=s.aM!=null||s.cJ!=null;
if(s.bv!=null&&s.bv.Length>1){eW(g,s.bv,s.du!="vertical" ? s.du : k.du,aW,dc,a8,eA);
}else{Color bU;if(s.aM!=null)bU=F(s.aM.First(tc=>tc.cL).ca);else if(!string.IsNullOrEmpty(s.cH))bU=F(s.cH);
else if(!string.IsNullOrEmpty(k.hz))bU=F(k.hz);else bU=Color.Transparent;if(bU.A>0||ot){if(ot)g.dv.Add(new e1{fa=g.f.Count,g7=bU,e5=bU,fi=s.aM,hb=s.g8,hk=s.g9,e8=s.cJ,hl=s.hm,hn=s.ho}
);t(g,new Vector2(aW+a8/2f,dc+eA/2f),new Vector2(a8,eA),bU);}}string fW=s.la ?? k.lb ?? "Monospace";float rb=H.pZ(k.cf,O.X,O.Y,k.cK);
float aH=s.lc ?? rb;float eh=0f;if(!string.IsNullOrEmpty(s.cj)){eh=20f*aH;var hY=TextAlignment.LEFT;float kG=aW+4f;
string lO=s.hG ?? k.lD ?? "left";if(lO.Equals("center",StringComparison.OrdinalIgnoreCase)){hY=TextAlignment.CENTER;kG=aW+(a8/2f);
}else if(lO.Equals("right",StringComparison.OrdinalIgnoreCase)){hY=TextAlignment.RIGHT;kG=aW+a8-4f;}string o3=s.hH ?? k.lE;
Color t1= !string.IsNullOrEmpty(o3)? F(o3): F(s.hf ?? k.e4 ?? "white");var o8=MySprite.CreateText(s.cj,fW,t1,aH*0.9f,hY);
o8.Position=new Vector2(kG,dc+2f);g.f.Add(o8);string[]ia=s.hg ?? k.ax;Color rE=ia!=null&&ia.Length>0 ? F(ia[0]): F(s.hf ?? k.e4 ?? "40, 60, 85");
t(g,new Vector2(aW+a8/2f,dc+eh-2f),new Vector2(a8-8f,1f),rE);}if(s.hy=="vertical"||s.at.Any(s3=>s3.h==e.fk)){
pH(g,kx,s,fW,aH,aW,dc,a8,eA,eh);continue;}int sz=Math.Max(1,s.at.Count);float sU=eA-eh-8f;
float j8=sU/sz;float f6=6f;for(int f4=0;f4<s.at.Count;f4++){var E=s.at[f4];var U=new cd{fd=E};float tp=dc+eh+4f+(f4*j8);
float M=tp+(j8/2f);float o5=M-(11f*aH);U.cM=g.f.Count;U.lh=E.W!=null&&(E.W.Contains("{val}")||E.W.Contains("{percent}"));
var nK=MySprite.CreateText(E.W,fW,Color.White,aH,TextAlignment.LEFT);nK.Position=new Vector2(aW+f6,o5);g.f.Add(nK);
float ss=Math.Max(24f*aH,(E.W.Length+1)*12f*aH);float uc=(!string.IsNullOrEmpty(E.dQ)|| !string.IsNullOrEmpty(E.dB))? 64f*aH : 52f*aH;
if(E.h!=e.hF){float L=aW+f6+ss;float ra=aW+a8-f6-uc;float J=Math.Max(16f,ra-L);
float T=Math.Max(E.h==e.ch ? 8f : 4f,Math.Min((E.h==e.ch ? 20f : 16f)*aH,j8*(E.h==e.ch ? 0.55f : 0.45f)));
U.b5=L;U.bf=J;U.aZ=T;U.b4=M;if(E.h==e.dt){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(25,30,38));
U.V=g.f.Count;t(g,new Vector2(L,M),new Vector2(0f,T),F(E.ar));}else if(E.h==e.dH){
int ki=10;float nb=2f;float kj=Math.Max(2f,(J-(nb*(ki-1)))/ki);for(int kc=0;kc<ki;kc++){U.K.Add(g.f.Count);
t(g,new Vector2(L+kc*(kj+nb)+(kj/2f),M),new Vector2(kj,T),new Color(22,26,32));}}else if(E.h==e.ch){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(20,24,30));
int j0=8;float nc=2f;float iw=Math.Max(2f,(J-(nc*(j0-1)))/j0);for(int jN=0;jN<j0;jN++){U.K.Add(g.f.Count);
t(g,new Vector2(L+jN*(iw+nc)+(iw/2f),M),new Vector2(iw,2f),F(E.ar));}}else if(E.h==e.dE){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(20,24,30));
int kt=10;float nd=2f;float ku=Math.Max(2f,(J-(nd*(kt-1)))/kt);for(int kd=0;kd<kt;kd++){U.K.Add(g.f.Count);
t(g,new Vector2(L+kd*(ku+nd)+(ku/2f),M),new Vector2(ku,T),new Color(24,28,36),0.523599f);}}else if(E.h==e.dx){
int h2=10;float ne=2f;float h3=Math.Max(2f,(J-(ne*(h2-1)))/h2);for(int ke=0;ke<h2;ke++){U.K.Add(g.f.Count);
t(g,new Vector2(L+ke*(h3+ne)+(h3/2f),M),new Vector2(h3,T),new Color(24,28,36),1.570796f,"Triangle");}}else if(E.h==e.dz){
int iC=12;float nf=2f;float iE=Math.Max(2f,(J-(nf*(iC-1)))/iC);float mM=Math.Min(iE,T);for(int kf=0;kf<iC;kf++){U.K.Add(g.f.Count);
t(g,new Vector2(L+kf*(iE+nf)+(iE/2f),M),new Vector2(mM,mM),new Color(24,28,36),0f,"Circle");}}else if(E.h==e.cI){
t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(25,30,38));U.V=g.f.Count;t(g,new Vector2(L+(J/2f),M),new Vector2(0f,T),F(E.ar));
t(g,new Vector2(L+(J/2f),M),new Vector2(2f,T+4f),new Color(80,110,150));}else if(E.h==e.ck){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(20,24,30));
U.V=g.f.Count;float jn=Math.Max(4f,T);t(g,new Vector2(L+jn*0.5f,M),new Vector2(jn,jn),new Color(40,50,65),0f,"Circle");
}else if(E.h==e.b9){float cV=Math.Max(6f,Math.Min(T,14f*aH));t(g,new Vector2(L+cV*0.5f,M),new Vector2(cV,cV),new Color(28,34,46));
U.V=g.f.Count;t(g,new Vector2(L+cV*0.5f,M),new Vector2(cV*0.6f,cV*0.6f),new Color(40,50,65));}else if(E.h==e.ci){
t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(18,22,30));U.V=g.f.Count;float jY=Math.Max(4f,T*0.7f);
t(g,new Vector2(L+jY+2f,M),new Vector2(jY,jY),new Color(34,197,94),0f,"Circle");}else if(E.h==e.cG){int jX=8;
float ng=2f;float jZ=Math.Max(2f,(J-(ng*(jX-1)))/jX);for(int jO=0;jO<jX;jO++){U.K.Add(g.f.Count);t(g,new Vector2(L+jO*(jZ+ng)+(jZ/2f),M),new Vector2(jZ,T),new Color(24,28,36));
}}else if(E.h==e.cb){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(20,24,30));U.V=g.f.Count;
float nu=Math.Max(4f,T);t(g,new Vector2(L+(J/2f),M),new Vector2(nu,nu),F(E.ar),0f,"Triangle");}}var pf=MySprite.CreateText("---",fW,Color.White,aH,TextAlignment.RIGHT);
pf.Position=new Vector2(aW+a8-f6,o5);U.aS=g.f.Count;g.f.Add(pf);g.dM.Add(U);}}g.dG=O;
g.dF=true;}const string qK="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bE=8;void pH(aA _,IMyTextSurface tS,az jQ,string m7,float ee,float sR,float jP,float sS,float n5,float r5){
int sG=Math.Max(1,jQ.at.Count);float mx=(sS-12f)/sG;float g0=22f*ee;float bd=jP+r5+4f+g0;float aK=Math.Max(20f,jP+n5-4f-g0-bd);
float ry=tS.MeasureStringInPixels(new StringBuilder("M"),m7,ee*0.9f).X;for(int f5=0;f5<jQ.at.Count;f5++){var gi=jQ.at[f5];float a7=sR+6f+mx*(f5+0.5f);
float cE=mx*0.7f;var ba=new cd{fd=gi,lF=true,b5=a7,bf=cE,aZ=aK,b4=bd+aK/2f,k4=ry};ba.cM=_.f.Count;
aY(_.f,gi.W,Color.White,a7,jP+n5-4f-g0,ee);ba.aS=_.f.Count;aY(_.f,"---",Color.White,a7,bd-g0,ee);
var nH=_.f[ba.cM];nH.Alignment=TextAlignment.CENTER;_.f[ba.cM]=nH;var pe=_.f[ba.aS];pe.Alignment=TextAlignment.CENTER;
_.f[ba.aS]=pe;t(_,new Vector2(a7,bd+aK/2f),new Vector2(cE,aK),new Color(25,30,38));ba.V=_.f.Count;if(gi.h==e.fk){
for(int l2=0;l2<bE;l2++){ba.K.Add(_.f.Count);t(_,new Vector2(a7,bd+aK),new Vector2(cE-6f,0f),Color.White);}for(int kP=0;kP<4;
kP++){ba.K.Add(_.f.Count);var oZ=MySprite.CreateText("",m7,kP==0 ? new Color(255,255,255,170): new Color(255,255,255,110),ee*0.9f,TextAlignment.CENTER);
oZ.Position=new Vector2(a7,bd+aK);_.f.Add(oZ);}Color iG=new Color(90,110,135);t(_,new Vector2(a7-cE/2f,bd+aK/2f),new Vector2(3f,aK),iG);t(_,new Vector2(a7+cE/2f,bd+aK/2f),new Vector2(3f,aK),iG);
t(_,new Vector2(a7,bd+aK),new Vector2(cE+3f,3f),iG);}else{t(_,new Vector2(a7,bd+aK),new Vector2(cE,0f),F(gi.ar));}_.dM.Add(ba);
}}bool qJ(aA a5,cd y,double eU,double ok,Color fK,bool pj){var bl=y.fd;float l9=y.D;if(y.D<0f)y.D=(float)eU;
y.D=Math.Abs(y.D-(float)eU)>0.05f ? y.D+((float)eU-y.D)*0.35f :(float)eU;float cu=Math.Max(0f,Math.Min(1f,y.D/100f))*y.aZ;
float fH=y.b4+y.aZ/2f;float fM=y.b5;if(bl.h!=e.fk){var fR=a5.f[y.V];fR.Size=new Vector2(y.bf,cu);
fR.Position=new Vector2(fM,fH-cu/2f);fR.Color=fK;a5.f[y.V]=fR;if(pj||y.D!=l9)aF(a5,y,a2(ok,bl),fK);
return pj||y.D!=l9;}float ic=y.aZ/bE;Color sB=bl.bw!=null ? F(H.dK(bl.bw,eU)): fK;
for(int cp=0;cp<bE;cp++){float po=Math.Max(0f,Math.Min(ic,cu-cp*ic));var gw=a5.f[y.K[cp]];gw.Size=new Vector2(y.bf-6f,po);
gw.Position=new Vector2(fM,fH-cp*ic-po/2f);gw.Color=bl.bw==null&&bl.cc&&bl.a3.Length>1 ? lf(bl.a3,100.0*(cp+0.5)/bE): sB;
a5.f[y.K[cp]]=gw;}int rn=Math.Max(1,(int)((y.bf-8f)/y.k4));bool tB=cu>6f;var kS=a5.f[y.K[bE]];
kS.Data=tB ? qK.Substring((int)(Z*5.0)%6,Math.Min(rn,100)): "";kS.Position=new Vector2(fM,fH-cu-12f);a5.f[y.K[bE]]=kS;
for(int bS=0;bS<3;bS++){var im=a5.f[y.K[bE+1+bS]];double s_=(Z*(0.35+0.1*bS)+bS*0.37)%1.0;im.Data=cu>24f ?(bS==1 ? "O" : "o"): "";
im.Position=new Vector2(fM+(bS-1)*y.bf*0.26f,fH-(float)s_*(cu-6f)-10f);a5.f[y.K[bE+1+bS]]=im;}aF(a5,y,a2(ok,bl),fK);
return true;}static void aF(aA mj,cd oM,string t_,Color rq){var kM=mj.f[oM.aS];kM.Data=t_;kM.Color=rq;mj.f[oM.aS]=kM;
}static void lx(aA mk,cd oN,Color rr){var mV=mk.f[oN.V];mV.Color=rr;mk.f[oN.V]=mV;}bool qx(z br,IMyTextSurface ky){
aA i;if(!bI.TryGetValue(br,out i)){i=new aA();bI[br]=i;}bool oo= !i.dF||i.dG!=ky.SurfaceSize;
if(oo){pG(br,ky,i);}bool N=oo;bool fw=(Z%br.g5)<br.g5/2.0;double cs= -1.0,mB;if(fn.TryGetValue(br,out mB)){
cs=(Z-mB)/br.hd;if(cs>=1.0){fn.Remove(br);cs=1.0;}}bool kQ=cm&&fw&&i.eX.A>0;if(i.b6>=0&&kQ!=i.eY){
i.eY=kQ;for(int ej=0;ej<i.e0.Length;ej++){var ma=i.f[i.b6+ej];ma.Color=kQ ? i.eX : i.e0[ej];i.f[i.b6+ej]=ma;
}N=true;foreach(var rc in i.dv)rc.hp=true;}foreach(var ai in i.dv){if(ai.ll&&i.eY)continue;Color d3=ai.g7;try{if(ai.fi!=null)d3=F(H.dK(ai.fi,cg(ai.hb,ai.hk)));
if(ai.e8!=null&&fw&&H.dK(ai.e8,cg(ai.hl,ai.hn))!=null)d3=i.eX;}catch{}if(d3!=ai.e5||ai.hp){
ai.e5=d3;ai.hp=false;var mi=i.f[ai.fa];mi.Color=d3;i.f[ai.fa]=mi;N=true;}}for(int i8=0;i8<i.dM.Count;i8++){var c=i.dM[i8];
var j=c.fd;double x;bool no=false;double jB=j.ff;try{x=cg(j.aN,j.I);if(j.cN!=null)jB=cg(j.cN,j.hu);
}catch{no=true;x=0.0;}if(no){if(c.fc!= -99999.0||(c.aS>=0&&i.f[c.aS].Data!="ERR")){N=true;c.fc= -99999.0;
if(c.aS>=0){var kg=i.f[c.aS];kg.Data="ERR";kg.Color=Color.Red;i.f[c.aS]=kg;}}continue;}double aX=x;
if(jB>0)x=Math.Min(100.0,aX/jB*100.0);bool oW=cs>=0.0&&j.h!=e.hF&&j.h!=e.ck&&j.h!=e.b9&&j.h!=e.ci&&j.h!=e.cb;
if(oW)x=cs<0.5 ? cs*200.0 : 100.0+(x-100.0)*(cs-0.5)*2.0;string rt=j.cc ? H.e9(j.a3,x): j.ar;
Color aa=F(rt);bool bk=(aa==Color.Red);if(j.cc&& !br.hq&&j.a3.Length>1)aa=lf(j.a3,x);
if(bk&&fw){aa=new Color(255,75,75);N=true;}bool aL=oW||Math.Abs(aX-c.fc)>0.1;if(aL)c.fc=aX;
if(c.lF){if(qJ(i,c,x,aX,aa,aL))N=true;continue;}if(j.h==e.dt){double m1=(j.hs||j.I=="percent"||j.I=="pct")? x : hD(j.aN);
if(c.D<0f)c.D=(float)m1;float gT=(float)m1;if(Math.Abs(c.D-gT)>0.05f){c.D+=(gT-c.D)*0.35f;
N=true;}else if(c.D!=gT){c.D=gT;N=true;}if(aL)N=true;if(N){float sX=(float)Math.Max(0.0,Math.Min(1.0,c.D/100.0));
float m3=c.bf*sX;var fT=i.f[c.V];fT.Size=new Vector2(m3,c.aZ);fT.Position=new Vector2(c.b5+(m3/2f),c.b4);
fT.Color=aa;i.f[c.V]=fT;aF(i,c,a2(aX,j),aa);}}else if(j.h==e.dH||j.h==e.dE||j.h==e.dx||j.h==e.dz){
if(aL||bk){N=true;double rN=(j.hs||j.I=="percent"||j.I=="pct")? x : hD(j.aN);int oF=c.K.Count;
for(int eL=0;eL<oF;eL++){bool sC=rN>=((eL+0.4)/(double)oF*100.0);Color tz=sC ? aa : new Color(24,28,36);var oG=i.f[c.K[eL]];
oG.Color=tz;i.f[c.K[eL]]=oG;}aF(i,c,a2(aX,j),aa);}}else if(j.h==e.ch){
if(aL||bk){N=true;for(int f_=0;f_<7;f_++)c.fj[f_]=c.fj[f_+1];c.fj[7]=(float)x;int s5=c.K.Count;
for(int ew=0;ew<s5;ew++){float r4=c.fj[ew];float r3=Math.Max(0.06f,Math.Min(1f,r4/100f));float mw=c.aZ*r3;float rs=c.b4+(c.aZ/2f)-(mw/2f);
var c_=i.f[c.K[ew]];c_.Size=new Vector2(c_.Size.Value.X,mw);c_.Position=new Vector2(c_.Position.Value.X,rs);
c_.Color=aa;i.f[c.K[ew]]=c_;}aF(i,c,a2(aX,j),aa);}}else if(j.h==e.cI){
if(c.D<0f)c.D=(float)x;float gU=(float)x;if(Math.Abs(c.D-gU)>0.05f){c.D+=(gU-c.D)*0.35f;
N=true;}else if(c.D!=gU){c.D=gU;N=true;}if(N||aL){float nn=c.bf*0.5f;float mr=c.b5+nn;
float mH=(float)Math.Max(-1.0,Math.Min(1.0,(c.D-50.0)/50.0));float iS=Math.Abs(mH)*nn;float rO=(mH>=0)?(mr+iS*0.5f):(mr-iS*0.5f);
var fU=i.f[c.V];fU.Size=new Vector2(iS,c.aZ);fU.Position=new Vector2(rO,c.b4);fU.Color=aa;
i.f[c.V]=fU;aF(i,c,a2(aX,j),aa);}}else if(j.h==e.ck){if(aL||bk){
N=true;bool jg=x>=50.0;float nD=Math.Max(4f,c.aZ);float sh=jg ?(c.b5+c.bf-nD*0.5f):(c.b5+nD*0.5f);
Color nC=jg ?(aa!=Color.White ? aa : new Color(34,197,94)): new Color(60,70,85);var iQ=i.f[c.V];iQ.Position=new Vector2(sh,c.b4);
iQ.Color=nC;i.f[c.V]=iQ;aF(i,c,jg ? "ON" : "OFF",nC);}}else if(j.h==e.b9){if(aL||bk){
N=true;bool jf=x>=50.0;Color ms=jf ?(aa!=Color.White ? aa : new Color(56,189,248)): new Color(30,36,48);lx(i,c,ms);
aF(i,c,jf ? "ACTIVE" : "READY",jf ? ms : new Color(148,163,184));}}else if(j.h==e.ci){if(aL||bk){
N=true;string tN=x>=75.0 ? "ONLINE" :(x>=25.0 ? "STBY" : "OFFLINE");Color oR=x>=75.0 ? new Color(34,197,94):(x>=25.0 ? new Color(234,179,8): new Color(239,68,68));
lx(i,c,oR);aF(i,c,tN,oR);}}else if(j.h==e.cG){if(aL||bk){N=true;int oe=c.K.Count;
for(int ex=0;ex<oe;ex++){bool sD=x>=((ex+0.5)/(double)oe*100.0);Color s1=sD ? aa : new Color(24,28,36);var of=i.f[c.K[ex]];
of.Color=s1;i.f[c.K[ex]]=of;}aF(i,c,a2(aX,j),aa);}}else if(j.h==e.cb){
N=true;float gB;Color ed;string fV;bool r9=(j.I=="producing"||j.I=="active"||j.I=="running"||j.I=="state"||(j.I!="percent"&&(x<=1.0||x==100.0)));
if(r9){if(x>0.0){c.D=(c.D+0.3927f)%6.28318f;gB=c.D;ed=new Color(34,197,94);fV="RUN";}else{gB=0f;
ed=new Color(148,163,184);fV="IDLE";}}else{gB=(x>55.0)? 0f :((x<45.0)? 3.14159f : 1.57079f);ed=(x>55.0)? new Color(34,197,94):((x<45.0)? new Color(239,68,68): new Color(148,163,184));
fV=(x>55.0)? "CHG" :((x<45.0)? "DSC" : "STBY");}var iR=i.f[c.V];iR.RotationOrScale=gB;iR.Color=ed;
i.f[c.V]=iR;aF(i,c,fV,ed);}else{string fQ;bg gW;dT.TryGetValue(j.aN,out gW);cP.TryGetValue(lm(j.I)? j.aN+"."+j.I :(gW!=null&&gW.bh!=null ? gW.bh : j.aN),out fQ);
if(aL||bk||fQ!=c.ln){c.ln=fQ;N=true;Color kJ=j.bw!=null ? F(H.dK(j.bw,x)): Color.White;
if(kJ==Color.Red&&fw)kJ=new Color(255,75,75);aF(i,c,fQ ?? a2(aX,j),kJ);}}if(c.lh&&(aL||bk)){
N=true;var nM=i.f[c.cM];string sY=le(x);string tm=a2(aX,j);nM.Data=j.W.Replace("{percent}",sY).Replace("{val}",tm);
i.f[c.cM]=nM;}}if(N){i.dR++;using(var rU=ky.DrawFrame()){rU.AddRange(i.f);}}return N;}bool qw(z bY,IMyTextSurface kz){
kz.FontSize=bY.cK;var fL=new List<KeyValuePair<string,string>>();foreach(var m in bY.fe){double P=0.0;string bc;string a6=null;
double jC=m.ff;try{P=cg(m.aN,m.I);if(m.cN!=null)jC=cg(m.cN,m.hu);}catch(Exception un){
fL.Add(new KeyValuePair<string,string>($"{m.W}: [UNRESOLVED]","red"));continue;}double ol=P;if(jC>0)P=Math.Min(100.0,ol/jC*100.0);string pg=a2(ol,m);
if(m.h==e.dt){double m2=(m.hs||m.I=="percent"||m.I=="pct")? P : hD(m.aN);bc=H.qp(m.W,m2,10,pg);
a6=m.cc ? H.e9(m.a3,m2): m.ar;}else if(m.h==e.cI){bc=H.qn(m.W,P,10);
a6=m.cc ? H.e9(m.a3,P): m.ar;}else if(m.h==e.ck){bc=H.qz(m.W,P);
a6=m.ar ??(P>=50.0 ? "green" : "gray");}else if(m.h==e.b9){bc=H.qs(m.W,P);a6=m.ar ??(P>=50.0 ? "cyan" : "gray");
}else if(m.h==e.ci){bc=H.qy(m.W,P);a6=m.ar ??(P>=75.0 ? "lime" :(P>=25.0 ? "yellow" : "red"));
}else if(m.h==e.cG){bc=H.qm(m.W,P,8);a6=m.cc ? H.e9(m.a3,P):(m.ar ?? "yellow");
}else if(m.h==e.cb){bool sa=(m.I=="producing"||m.I=="active"||m.I=="running"||m.I=="state"||(m.I!="percent"&&(P<=1.0||P==100.0)));
if(sa){char[]tL=new[]{'|','/','-','\\'};char tw=tL[Math.Abs(dZ)%4];bc=P>0 ? $"{m.W} [{tw}] RUN" : $"{m.W} [==] IDLE";
a6=P>0 ? "green" : "gray";}else{bc=H.qt(m.W,P,dZ);a6=m.ar ??(P>55.0 ? "green" :(P<45.0 ? "red" : "cyan"));
}}else{bc=$"{m.W}: {pg}";a6=m.bw!=null ? H.dK(m.bw,P): null;}fL.Add(new KeyValuePair<string,string>(bc,a6));
}var fG=H.qr(new List<string>(fL.ConvertAll(tV=>tV.Key)),dq: bY.lG);string oJ=string.Join("\n",fG);
string oh;if(cn.TryGetValue(bY,out oh)&&oh==oJ)return false;cn[bY]=oJ;using(var rV=kz.DrawFrame()){
float pw=4f;float nO=px*bY.cK;try{var nR=kz.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bY.cK);
if(nR.Y>0)nO=nR.Y;}catch{}for(int c4=0;c4<fG.Count;c4++){string mz=null;if(c4>0&&c4<fG.Count-1)mz=fL[c4-1].Value;
var oQ=MySprite.CreateText(fG[c4],"Monospace",F(mz),bY.cK,TextAlignment.LEFT);oQ.Position=new Vector2(4f,pw);rV.Add(oQ);
pw+=nO;}}return true;}Color F(string gk){if(string.IsNullOrEmpty(gk))return Color.White;Color eq;if(lK.TryGetValue(gk,out eq))return eq;
n d6=n.dJ(gk);eq=d6.fb ? Color.Transparent : new Color(d6.bC,d6.bx,d6.bs,d6.g4);lK[gk]=eq;return eq;}public enum a_{pB,hr,dA}
public enum aE{p7,g6,lB,lp,lo}public class bg{public string ac;public a_ bz;public List<string>aR=new List<string>();public string lC{
get{return aR.Count>0 ? aR[0]: "";}set{if(aR.Count==0)aR.Add(value);else aR[0]=value;}}public aE dr=aE.p7;
public double e6=0.0;public string hh;public string bh;public bool e_;}public enum e{hF,dt,dH,ch,dE,dx,dz,cI,ck,b9,ci,cG,cb,fk}
public class aO{public string hj;public string ca;public bool cL;}public class bA{public e h;public string W;public string aN;
public string I;public bool cc;public string[]a3;public string ar;public List<aO>bw;public string k5="right";
public string dQ;public string dB;public double ff;public string cN,hu;public bool hs{get{return ff>0||cN!=null;}}}public struct n{
public byte bC,bx,bs,g4;public bool fb;public n(byte td,byte rX,byte q3,byte qM=255){bC=td;bx=rX;bs=q3;g4=qM;fb=false;}public static readonly n qI=new n(0,0,0,0){
fb=true};public static readonly n fl=new n(255,255,255);public static readonly n pA=new n(0,0,0);public static readonly n qk=new n(255,0,0);
public static readonly n p0=new n(0,255,0);public static readonly n p4=new n(50,205,50);public static readonly n qL=new n(255,255,0);
public static readonly n p8=new n(255,140,0);public static readonly n pM=new n(0,255,255);public static readonly n pC=new n(0,120,255);
public static readonly n p5=new n(255,0,255);public static readonly n qi=new n(128,0,128);public static readonly n p_=new n(128,128,128);
public static readonly n pN=new n(60,60,60);public static n dJ(string bb){if(string.IsNullOrWhiteSpace(bb))return fl;bb=bb.Trim();
if(bb.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bb.Equals("none",StringComparison.OrdinalIgnoreCase))return qI;if(bb.StartsWith("#")){
string bQ=bb.Substring(1).Trim();if(bQ.Length==6){byte te=Convert.ToByte(bQ.Substring(0,2),16);byte rY=Convert.ToByte(bQ.Substring(2,2),16);byte q4=Convert.ToByte(bQ.Substring(4,2),16);
return new n(te,rY,q4);}if(bQ.Length==3){byte tf=Convert.ToByte(new string(bQ[0],2),16);byte rZ=Convert.ToByte(new string(bQ[1],2),16);byte q5=Convert.ToByte(new string(bQ[2],2),16);
return new n(tf,rZ,q5);}}if(bb.Contains(",")){var dd=bb.Split(',');if(dd.Length>=3){byte tg=(byte)Math.Max(0,Math.Min(255,int.Parse(dd[0].Trim())));
byte r_=(byte)Math.Max(0,Math.Min(255,int.Parse(dd[1].Trim())));byte q6=(byte)Math.Max(0,Math.Min(255,int.Parse(dd[2].Trim())));byte qN=dd.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(dd[3].Trim()))):(byte)255;
return new n(tg,r_,q6,qN);}}switch(bb.ToLowerInvariant()){case "red": return qk;case "green": return p0;case "lime": return p4;case "yellow": return qL;
case "orange": return p8;case "cyan": return pM;case "blue": return pC;case "magenta": return p5;case "purple": return qi;case "white": return fl;
case "black": return pA;case "gray": case "grey": return p_;case "darkgray": case "darkgrey": return pN;default: return fl;}}}public class bu{
public string[]bv;public string du="vertical";public List<aO>aM;public string g8,g9;public List<aO>cJ;public string hm,ho;
}public class az : bu{public string dI="default";public int dy=0;public int dL=0;public int dN=1;public int dO=1;public string cj;public string hG="left";
public string hH;public string k2;public string la;public float? lc;public string cH;public string hf;public string[]hg;
public string hy="horizontal";public List<bA>at=new List<bA>();}public class z : bu{public string a4;public string cf="standard";
public string dw="graphic";public string e4;public string[]ax;public string lD="left";public string lE;public string lb="Monospace";
public string bt;public string k0;public int k1=10;public int kZ=1;public float hd=1.2f;public bool hq;
public string hz;public string ha;public string kY;public string kW;public string hv;public string b3;public string b8;
public float he=2.5f;public string k_="cyan";public string[]e2;public List<string>e3=new List<string>();public List<string>pY(){
if(e3!=null&&e3.Count>0)return e3;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float cK=1.0f;public int hB=0;public float g5=1.6f;public int dC=1;public int dD=1;public int lG=30;public string bB;
public int ce=0;public List<az>ah=new List<az>();public List<bA>fe{get{if(ah.Count==0)ah.Add(new az{dI="main"}
);return ah[0].at;}}}public class bF{public string hw;public List<string>fg=new List<string>();public Dictionary<string,string>hi=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>hc=new List<string>();}public class hA{public List<bg>b7=new List<bg>();public List<z>S=new List<z>();
public List<string>a1=new List<string>();public List<string>dS=new List<string>();public Dictionary<string,List<string>>ds=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class lj{static readonly Dictionary<string,bF>k3=new Dictionary<string,bF>(StringComparer.OrdinalIgnoreCase);
static lj(){ql();}static void ql(){bD("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bD("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bD("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bD("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bD("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bD("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bD("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bD("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bD(string nY,string[]sV,string rg){var gR=new bF{hw=nY}
;foreach(var sO in sV){var jp=sO.Split(new[]{'='},2);string n6=hx(jp[0]);gR.fg.Add(n6);if(jp.Length>1)gR.hi[n6]=ae(jp[1].Trim());
}var sA=rg.Replace("\r\n","\n").Split('\n');foreach(var sn in sA){string ov=sn.Trim();if(ov.Length>0)gR.hc.Add(ov);}k3[nY]=gR;}static string hx(string da){
da=da.Trim();if(da.StartsWith("$"))da=da.Substring(1);return da.ToLowerInvariant();}public static hA qa(string rw){var ao=new hA();z d=null;
az r=null;var og=qg(rw,ao.a1,ao.ds);for(int ek=0;ek<og.Count;ek++){string a=og[ek].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){ao.b7.Add(qb(a));}else if(a.StartsWith("@screen")){d=new z{a4=a.Substring("@screen".Length).Trim()}
;ao.S.Add(d);r=null;}else if(a.StartsWith("profile:")){p(d,"profile:");d.cf=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){p(d,"border:");string l3=a.Substring("border:".Length).Trim();if(r!=null)r.k2=l3;
else d.dw=l3;}else if(a.StartsWith("border_color:")){p(d,"border_color:");string l5=a.Substring("border_color:".Length).Trim();
if(r!=null)r.hf=l5;else d.e4=l5;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
p(d,"border_gradient");string cq=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
cq=cq.Trim();if(cq.StartsWith("(")&&cq.Contains(")"))cq=aP(cq,0);string[]oS=aQ(cq).Select(tq=>tq.Trim()).ToArray();if(r!=null)r.hg=oS;
else d.ax=oS;}else if(a.StartsWith("title_align:")){p(d,"title_align:");string o_=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(r!=null)r.hG=o_;else d.lD=o_;}else if(a.StartsWith("title_color:")){p(d,"title_color:");string o2=a.Substring("title_color:".Length).Trim();
if(r!=null)r.hH=o2;else d.lE=o2;}else if(a.StartsWith("font_size:")){p(d,"font_size:");float oX=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(r!=null)r.lc=oX;else d.cK=oX;}else if(a.StartsWith("font:")){p(d,"font:");string mW=a.Substring("font:".Length).Trim();
if(r!=null)r.la=mW;else d.lb=mW;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){p(d,"bg:");string cT=a.Substring(a.IndexOf(':')+1).Trim();
bu ib=r!=null ?(bu)r : d;if(cT.StartsWith("color(")&&cT.EndsWith(")")){string md;ib.aM=lt(cT.Substring(6,cT.Length-7),out md);
string mb,mc;ly(md,out mb,out mc);ib.g8=mb;ib.g9=mc;}else if(r!=null)r.cH=cT;
else d.bt=cT;}else if(a.StartsWith("layout:")){p(d,"layout:");qc(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
p(d,"alert_period:");d.g5=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){p(d,"refresh:");d.hB=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
p(d,"output:");var gq=aQ(a.Substring("output:".Length).Trim());if(gq.Count==0)throw new FormatException("output: requires a target block name");
d.bB=ae(gq[0]);d.ce=gq.Count>1 ? int.Parse(gq[1]): 0;}else if(a.StartsWith("width:")){p(d,"width:");
d.lG=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){p(d,"boot_msg:");
d.k0=ae(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){p(d,"boot_stagger:");d.k1=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){p(d,"boot_calibrate:");string mC=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.hd=mC=="off" ? 0f : Math.Max(0f,float.Parse(mC,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){p(d,"boot_batch:");
d.kZ=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){p(d,"boot_duration:");
string d8=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool nt=d8.EndsWith("s");if(nt)d8=d8.Substring(0,d8.Length-1).Trim();
float iF;if(float.TryParse(d8,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out iF)){d.he=nt ? iF : iF/60f;
}}else if(a.StartsWith("boot_log:")){p(d,"boot_log:");d.e3.Add(ae(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("mirror:")){
p(d,"mirror:");d.hv=ae(a.Substring("mirror:".Length).Trim());}else if(a.StartsWith("banner:")){p(d,"banner:");
d.b3=ae(a.Substring("banner:".Length).Trim());}else if(a.StartsWith("boot_logo:")){p(d,"boot_logo:");d.b8=ae(a.Substring("boot_logo:".Length).Trim());
}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){p(d,"boot_gradient");
d.e2=lu(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){p(d,"bg_gradient");(r!=null ?(bu)r : d).bv=lu(a);
}else if(a.StartsWith("bg_gradient_dir:")){p(d,"bg_gradient_dir:");(r!=null ?(bu)r : d).du=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("flash:")){p(d,"flash:");bu iN=r!=null ?(bu)r : d;string iM=a.Substring("flash:".Length).Trim();
string mY=lA(iM);string mX,mZ;ly(iM.Substring(0,iM.Length-mY.Length),out mX,out mZ);iN.hm=mX;iN.ho=mZ;
iN.cJ=new List<aO>{new aO{hj=mY,ca="on"}};}else if(a.StartsWith("bg_texture:")){p(d,"bg_texture:");d.ha=ae(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){p(d,"bg_texture_color:");d.kY=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gradient_mode:")){p(d,"gradient_mode:");d.hq=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){p(d,"screen_bg:");d.bt=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
p(d,"panel_bg:");d.hz=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){p(d,"alert_bg:");
d.kW=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){p(d,"boot_color:");
d.k_=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){p(d,"orientation:");string sK=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(r!=null)r.hy=sK;}else if(a.StartsWith("@panel")){p(d,"@panel");r=qe(a);d.ah.Add(r);
}else if(a.StartsWith("title:")){p(d,"title:");if(r==null){r=new az{dI="main",dy=0,dL=0,dN=d.dC,dO=d.dD}
;d.ah.Add(r);}r.cj=ae(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){p(d,"line:");
if(r==null){r=new az{dI="main",dy=0,dL=0,dN=d.dC,dO=d.dD};d.ah.Add(r);}r.at.Add(qd(a.Substring("line:".Length).Trim()));
}else{ao.a1.Add($"Line {ek + 1}: unrecognized directive: \"{a}\"");}}catch(Exception rH){ao.a1.Add($"Line {ek + 1}: parse error ({rH.Message}) in \"{a}\"");
}}foreach(var ow in ao.S){if(string.IsNullOrEmpty(ow.bB))ao.a1.Add($"@screen {ow.a4}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aq in ao.S){int jL=aq.ah.Count(j_=>j_.cH!=null||j_.aM!=null||j_.bv!=null);if(aq.ah.Count>1&&jL>0&&jL<aq.ah.Count)ao.dS.Add("screen '"+aq.a4+"': only "+jL+" of "+aq.ah.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(aq.b3!=null&& !ao.ds.ContainsKey(aq.b3))ao.a1.Add("Screen '"+aq.a4+"': banner '"+aq.b3+"' is not defined (@banner "+aq.b3+" ... @end)");
if(aq.b8!=null&& !ao.ds.ContainsKey(aq.b8))ao.a1.Add("Screen '"+aq.a4+"': boot_logo '"+aq.b8+"' is not defined (@banner "+aq.b8+" ... @end)");
}return ao;}static void qc(string ij,z oD){int jS=ij.IndexOf('(');if(jS<0)throw new FormatException("missing '(' in layout: specification");
string nB=ij.Substring(0,jS).Trim().ToLowerInvariant();string qU=aP(ij,jS);var h1=aQ(qU);if(nB=="grid"){if(h1.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
oD.dC=Math.Max(1,int.Parse(h1[0]));oD.dD=Math.Max(1,int.Parse(h1[1]));}else{throw new FormatException($"unknown layout kind '{nB}' (expected grid)");
}}static az qe(string sv){string eG=sv.Substring("@panel".Length).Trim();int fz=eG.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(fz<0)throw new FormatException("@panel requires an at(...) positioning clause");
string sH=eG.Substring(0,fz).Trim();string lZ=eG.Substring(fz);int oc=lZ.IndexOf('(');string lX=aP(lZ,oc);
var co=aQ(lX);if(co.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bo=new az{dI=sH,dy=int.Parse(co[0]),dL=int.Parse(co[1]),dN=co.Count>2 ? int.Parse(co[2]): 1,dO=co.Count>3 ? int.Parse(co[3]): 1}
;int lQ=fz+oc+lX.Length+1;if(lQ<eG.Length){string aI=eG.Substring(lQ).Trim();int o7=aI.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(o7>=0){string eO=aI.Substring(o7+5).Trim();if(eO.StartsWith("(")&&eO.Contains(")"))bo.cj=ae(aP(eO,0));else if(eO.StartsWith(":")){
string cr=eO.Substring(1).Trim();if(cr.StartsWith("\"")){int mS=cr.IndexOf('"',1);bo.cj=mS>0 ? cr.Substring(1,mS-1): ae(cr);}else{int oO=cr.IndexOf(' ');
bo.cj=oO>=0 ? cr.Substring(0,oO): cr;}}}string n0=e7(aI,"orientation:");if(n0!=null)bo.hy=n0.ToLowerInvariant();string lP=e7(aI,"title_align:");
if(lP!=null)bo.hG=lP.ToLowerInvariant();string o4=e7(aI,"title_color:");if(o4!=null)bo.hH=o4;int fF=aI.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(fF>=0&& !aI.Substring(fF).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aI.Substring(fF).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bo.k2=e7(aI.Substring(fF),"border:");}int fC=aI.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(fC<0)fC=aI.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(fC>=0){string bO=aI.Substring(fC+"border_gradient".Length).Trim();if(bO.StartsWith(":"))bO=bO.Substring(1).Trim();if(bO.StartsWith("(")&&bO.Contains(")"))bO=aP(bO,0);
bo.hg=aQ(bO).Select(tr=>tr.Trim()).ToArray();}}return bo;}static string e7(string ox,string nA){int ns=ox.IndexOf(nA,StringComparison.OrdinalIgnoreCase);
if(ns<0)return null;string kN=ox.Substring(ns+nA.Length).Trim();int oP=kN.IndexOf(' ');return oP>=0 ? kN.Substring(0,oP): kN;}static string[]lu(string jv){
int iY=jv.IndexOfAny(new[]{':','('});string ef=jv.Substring(jv[iY]==':' ? iY+1 : iY).Trim();if(ef.StartsWith("(")&&ef.Contains(")"))ef=aP(ef,0);return aQ(ef).Select(ts=>ts.Trim()).ToArray();
}static List<string>qg(string rx,List<string>iK,Dictionary<string,List<string>>q8){var pb=new Dictionary<string,bF>(StringComparer.OrdinalIgnoreCase);
var df=(rx ?? "").Replace("\r\n","\n").Split('\n');var jK=new List<string>();int c5=0;while(c5<df.Length){string j5=df[c5++];if(j5.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var am=new List<string>();while(c5<df.Length){string lN=df[c5++].Replace("\t","    ").TrimEnd();if(lN.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
am.Add(lN);}while(am.Count>0&&am[0].Length==0)am.RemoveAt(0);while(am.Count>0&&am[am.Count-1].Length==0)am.RemoveAt(am.Count-1);int je=int.MaxValue;
foreach(var hU in am)if(hU.Length>0)je=Math.Min(je,hU.Length-hU.TrimStart().Length);for(int er=0;er<am.Count;er++)if(am[er].Length>0)am[er]=am[er].Substring(je);
q8[j5.TrimStart().Substring(8).Trim()]=am;continue;}string bT=lz(j5).Trim();if(bT.Length==0)continue;if(bT.StartsWith("@template")){try{
var kD=qf(bT);while(c5<df.Length){string ri=df[c5++];string ik=lz(ri).Trim();if(ik.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(ik.Length>0)kD.hc.Add(ik);}pb[kD.hw]=kD;}catch(Exception rI){iK.Add($"Template parse error ({rI.Message}) in \"{bT}\"");}}
else if(bT.StartsWith("@use")){try{k7(bT,pb,jK,iK);}catch(Exception rJ){iK.Add($"@use error ({rJ.Message}) in \"{bT}\"");
}}else{jK.Add(bT);}}return jK;}static bF qf(string sw){string j6=sw.Substring("@template".Length).Trim();int jT=j6.IndexOf('(');
if(jT<0)throw new FormatException("missing '(' in @template header");string sI=j6.Substring(0,jT).Trim();string qV=aP(j6,jT);var kE=new bF{
hw=sI};foreach(var sP in aQ(qV)){var jq=sP.Split(new[]{'='},2);string n7=hx(jq[0]);kE.fg.Add(n7);if(jq.Length>1)kE.hi[n7]=ae(jq[1].Trim());
}return kE;}static void k7(string sx,Dictionary<string,bF>pc,List<string>n3,List<string>rG,int mG=0){string gA=sx.Substring("@use".Length).Trim();
int go=gA.IndexOf('(');string jD=go>=0 ? gA.Substring(0,go).Trim(): gA.Trim();string qW=go>=0 ? aP(gA,go): "";bF dm=null;
if(!pc.TryGetValue(jD,out dm)){if(!k3.TryGetValue(jD,out dm))throw new FormatException($"unknown template '{jD}'");}var fI=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var nF in dm.hi)fI[nF.Key]=nF.Value;var lW=aQ(qW);for(int d0=0;d0<lW.Count;d0++){string fx=lW[d0];int iI=fx.IndexOf('=');if(iI>=0){
string sf=hx(fx.Substring(0,iI));string t6=ae(fx.Substring(iI+1).Trim());fI[sf]=t6;}else{if(d0<dm.fg.Count){string sg=dm.fg[d0];
fI[sg]=ae(fx.Trim());}}}foreach(var rh in dm.hc){string eb=rh;foreach(var ge in fI){eb=eb.Replace("$"+ge.Key,ge.Value).Replace("{"+ge.Key+"}",ge.Value);
}if(eb.StartsWith("@use")){if(mG>3)throw new FormatException("@use nested too deep");k7(eb,pc,n3,rG,mG+1);
}else n3.Add(eb);}}static void p(z rv,string rD){if(rv==null)throw new FormatException($"'{rD}' used before any @screen declared");
}static string lz(string a9){if(string.IsNullOrEmpty(a9))return a9;bool jd=false;for(int bR=0;bR<a9.Length;bR++){char ip=a9[bR];if(ip=='"'){jd= !jd;
}else if(!jd){if(ip=='/'&&bR+1<a9.Length&&a9[bR+1]=='/'){return a9.Substring(0,bR);}if(ip=='#'){if(p2(a9,bR)){continue;}return a9.Substring(0,bR);}}}return a9;
}static bool p2(string cw,int nv){int c8=0;for(int i9=nv+1;i9<cw.Length;i9++){char cW=cw[i9];if((cW>='0'&&cW<='9')||(cW>='a'&&cW<='f')||(cW>='A'&&cW<='F'))c8++;
else break;}if(c8==3||c8==4||c8==6||c8==8){int ev=nv+1+c8;if(ev>=cw.Length||char.IsWhiteSpace(cw[ev])||cw[ev]==')'||cw[ev]==','||cw[ev]==':')return true;
}return false;}static bg qb(string sy){string j7=sy.Substring("@bind".Length).Trim();int iJ=j7.IndexOf('=');if(iJ<0)throw new FormatException("missing '=' in @bind");
string nZ=j7.Substring(0,iJ).Trim();string iL=j7.Substring(iJ+1).Trim();int jU=iL.IndexOf('(');if(jU<0)throw new FormatException("missing '(' in @bind target");
string fY=iL.Substring(0,jU).Trim().ToLowerInvariant();string qX=aP(iL,jU);var bL=aQ(qX);if(bL.Count==0)throw new FormatException("@bind target has no arguments");
var ap=new bg{ac=nZ};if(fY=="dynamic"||fY=="var"||fY=="mem"){ap.bz=a_.dA;ap.bh=bL.Count>0 ? ae(bL[0].Trim()): nZ;
for(int hV=0;hV<bL.Count;hV++){var gf=bL[hV].Split(new[]{':'},2);if(gf.Length==2&&gf[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double mF;
if(double.TryParse(gf[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mF))ap.e6=mF;else ap.hh=ae(gf[1].Trim());
}}return ap;}ap.bz=fY=="group" ? a_.hr : a_.pB;string fv=null;for(int fu=0;fu<bL.Count;fu++){var c7=bL[fu].Split(new[]{':'},2);if(c7.Length==2&&c7[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
fv=c7[1].Trim().ToLowerInvariant();}else if(c7.Length==2&&c7[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ap.e_=c7[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ap.aR.Add(ae(bL[fu].Trim()));}}if(ap.aR.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ap.bz==a_.hr){if(fv==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(fv){case "avg": ap.dr=aE.g6;break;case "sum": ap.dr=aE.lB;break;case "min": ap.dr=aE.lp;break;case "max": ap.dr=aE.lo;
break;default: throw new FormatException($"unknown agg '{fv}' (expected avg/sum/min/max)");}}return ap;}static bA qd(string fE){var o=new bA();
if(!fE.StartsWith("\""))throw new FormatException("line must start with a quoted label");int iH=fE.IndexOf('"',1);if(iH<0)throw new FormatException("unterminated quoted label");
o.W=fE.Substring(1,iH-1);string eF=fE.Substring(iH+1).Trim();int gp=eF.IndexOf('(');if(gp<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string C=eF.Substring(0,gp).Trim().ToLowerInvariant();string qT=aP(eF,gp);string af=eF.Substring(eF.IndexOf(')',gp)+1).Trim();
var iD=qT.Trim().Split('.');if(iD.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.aN=iD[0].Trim();o.I=iD[1].Trim();if(C=="bar")o.h=e.dt;else if(C=="led")o.h=e.dH;else if(C=="spark"||C=="sparkline")o.h=e.ch;
else if(C=="hazard"||C=="stripes")o.h=e.dE;else if(C=="chevron"||C=="chevrons")o.h=e.dx;else if(C=="dots"||C=="dot")o.h=e.dz;
else if(C=="balance"||C=="horizon"||C=="centered")o.h=e.cI;else if(C=="toggle"||C=="switch")o.h=e.ck;else if(C=="checkbox"||C=="check")o.h=e.b9;
else if(C=="status"||C=="badge"||C=="state")o.h=e.ci;else if(C=="ammo"||C=="pips")o.h=e.cG;else if(C=="flow"||C=="rate"||C=="spinner")o.h=e.cb;
else if(C=="text")o.h=e.hF;else if(C=="tank"||C=="tube"||C=="bucket")o.h=e.fk;else throw new FormatException($"unknown display function '{C}'");
while(af.Length>0){int iB=af.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);int c2=af.IndexOf('(');if(iB>=0&&(c2<0||iB<c2)){
string fN=af.Substring(iB+4).Trim();int gJ=fN.IndexOf(' ');o.k5=(gJ>=0 ? fN.Substring(0,gJ): fN).ToLowerInvariant();af=(gJ>=0 ? fN.Substring(gJ): "").Trim();
continue;}int kL=af.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(kL>=0&&(c2<0||kL<c2)){string g1=af.Substring(kL+5).Trim();
int gK=g1.IndexOf(' ');o.dQ=(gK>=0 ? g1.Substring(0,gK): g1).Trim().Trim('"');af=(gK>=0 ? g1.Substring(gK): "").Trim();continue;}int iT=af.IndexOf("format:",StringComparison.OrdinalIgnoreCase);
if(iT>=0&&(c2<0||iT<c2)){string fS=af.Substring(iT+7).Trim();int gL=fS.IndexOf(' ');o.dB=(gL>=0 ? fS.Substring(0,gL): fS).Trim().Trim('"');
af=(gL>=0 ? fS.Substring(gL): "").Trim();continue;}int gj=af.IndexOf('(');if(gj<0)break;string au=af.Substring(0,gj).Trim().ToLowerInvariant();
int nL=au.LastIndexOf(' ');if(nL>=0)au=au.Substring(nL+1).Trim();string bn=aP(af,gj);af=af.Substring(af.IndexOf(')',gj)+1).Trim();
if(au=="gradient"){o.cc=true;o.a3=aQ(bn).Select(tt=>tt.Trim()).ToArray();if(o.a3.Length<2)throw new FormatException("gradient() needs at least 2 colors");
}else if(au=="flat"){o.ar=bn.Trim();}else if(au=="pattern"){string Q=bn.Trim().ToLowerInvariant();if(Q=="hazard"||Q=="stripes")o.h=e.dE;
else if(Q=="chevron"||Q=="chevrons")o.h=e.dx;else if(Q=="dots"||Q=="dot")o.h=e.dz;else if(Q=="led")o.h=e.dH;
else if(Q=="balance"||Q=="horizon")o.h=e.cI;else if(Q=="toggle"||Q=="switch")o.h=e.ck;else if(Q=="checkbox"||Q=="check")o.h=e.b9;
else if(Q=="status"||Q=="badge"||Q=="state")o.h=e.ci;else if(Q=="ammo"||Q=="pips")o.h=e.cG;else if(Q=="flow"||Q=="rate"||Q=="spinner")o.h=e.cb;
}else if(au=="color"){string r8;o.bw=lt(bn,out r8);}else if(au=="dir"||au=="direction"){o.k5=bn.Trim().ToLowerInvariant();
}else if(au=="unit"){o.dQ=bn.Trim().Trim('"');}else if(au=="max"){double nU;if(double.TryParse(bn.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nU))o.ff=nU;
else{var jA=bn.Trim().Split('.');if(jA.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.cN=jA[0].Trim();o.hu=jA[1].Trim();
}}else if(au=="format"||au=="fmt"){o.dB=bn.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{au}'");}}return o;}static string aP(string gE,int jI){
int iA=0;for(int el=jI;el<gE.Length;el++){if(gE[el]=='(')iA++;else if(gE[el]==')'){iA--;if(iA==0)return gE.Substring(jI+1,el-jI-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aQ(string qY){return qY.Split(',').Select(tu=>tu.Trim()).Where(tv=>tv.Length>0).ToList();
}static List<aO>lt(string qS,out string iP){var gC=new List<aO>();iP=null;foreach(var jV in aQ(qS)){int ix=jV.LastIndexOf(':');
if(ix<0)throw new FormatException("color() entries need 'expr : color'");string gg=jV.Substring(0,ix).Trim();string my=jV.Substring(ix+1).Trim();
if(gg.Equals("default",StringComparison.OrdinalIgnoreCase)){gC.Add(new aO{cL=true,ca=my});}else{string oT=lA(gg);
if(iP==null)iP=gg.Substring(0,gg.Length-oT.Length).Trim();gC.Add(new aO{hj=oT,ca=my});}}if(!gC.Any(th=>th.cL))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return gC;}static void ly(string ti,out string re,out string rM){var ey=(ti ?? "").Split('.');if(ey.Length!=2||ey[0].Trim().Length==0||ey[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
re=ey[0].Trim();rM=ey[1].Trim();}static string lA(string c0){c0=c0.Trim();foreach(var sM in new[]{">=","<=","==",">","<"}){int ja=c0.IndexOf(sM,StringComparison.Ordinal);
if(ja>0)return c0.Substring(ja).Trim();if(ja==0)return c0;}throw new FormatException($"color() rule has no recognizable comparison operator: '{c0}'");}
static string ae(string cx){if(cx.Length>=2&&cx[0]=='"'&&cx[cx.Length-1]=='"')return cx.Substring(1,cx.Length-2);return cx;}}public static class pz{public static double py(aE qO,List<double>dp){
if(dp==null||dp.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(qO){case aE.g6: return dp.Average();case aE.lB: return dp.Sum();
case aE.lp: return dp.Min();case aE.lo: return dp.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class H{
public static string qp(string sp,double gr,int l4=10,string pk=null){gr=Math.Max(0,Math.Min(100,gr));int m4=(int)Math.Round(l4*(gr/100.0));
string q9=new string('#',m4)+new string('-',l4-m4);string t7=pk!=null ? pk : $"{gr:0}%";return $"{sp} [{q9}] {t7}";
}public static string ld(double b2,string kK=null,string fX=null){if(kK=="auto"){double hW=Math.Abs(b2);if(hW>=1e9)return(b2/1e9).ToString("F1")+"G";
if(hW>=1e6)return(b2/1e6).ToString("F1")+"M";if(hW>=1e3)return(b2/1e3).ToString("F1")+"k";string rK= !string.IsNullOrEmpty(fX)? fX : "F0";return b2.ToString(rK);
}string m6= !string.IsNullOrEmpty(fX)? fX :(Math.Abs(b2)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(kK)){return b2.ToString(m6);}return b2.ToString(m6)+kK;
}public static float pZ(string s7,float oV,float kA,float fO=1.0f){string db=(s7 ?? "standard").ToLowerInvariant();
if(db=="compact"||(db=="auto"&&oV<=300&&kA<=300&&kA>160)){return fO*0.75f;}if(db=="corner"||(db=="auto"&&kA<=160)){
return fO*0.55f;}if(db=="wide"||(db=="auto"&&oV>=1000)){return fO*1.15f;}return fO;}public static string qn(string sq,double gs,int bP=5){
gs=Math.Max(0,Math.Min(100,gs));double fP=(gs-50.0)/50.0;int es=0;int eI=0;if(fP< -0.01)es=(int)Math.Round(-fP*bP);
else if(fP>0.01)eI=(int)Math.Round(fP*bP);if(es>bP)es=bP;if(eI>bP)eI=bP;string st=new string('-',bP-es)+new string('<',es);
string tn=new string('>',eI)+new string('-',bP-eI);return $"{sq} [{st}|{tn}] {gs:0}%";}public static string qz(string nI,double t9){
return t9>=50.0 ? $"{nI} [====O] ON" : $"{nI} [O====] OFF";}public static string qs(string nJ,double ua){return ua>=50.0 ? $"{nJ} [X] ACTIVE" : $"{nJ} [ ] READY ";
}public static string qy(string js,double pi){if(pi>=75.0)return $"{js} [ ONLINE  ]";if(pi>=25.0)return $"{js} [ STANDBY ]";return $"{js} [ OFFLINE ]";
}public static string qm(string sr,double gt,int nQ=8){gt=Math.Max(0,Math.Min(100,gt));int m5=(int)Math.Round(nQ*(gt/100.0));
string s2=new string('|',m5)+new string('.',nQ-m5);return $"{sr} [{s2}] {gt:0}%";}public static string qt(string jt,double eC,int o6=0){
char[]tK=new[]{'|','/','-','\\'};char oy=tK[(o6>=0 ? o6 : 0)%4];if(eC>55.0)return $"{jt} {oy} ▲ CHG {eC:0}%";if(eC<45.0)return $"{jt} {oy} ▼ DSC {eC:0}%";
return $"{jt} ● STBY {eC:0}%";}public static bool p6(string av,string eo){if(av==null||eo==null)return false;if(av=="*"||av==eo)return true;
if(!av.Contains("*")&& !av.Contains("?"))return string.Equals(av,eo,StringComparison.OrdinalIgnoreCase);int aB=0,em=0,kr= -1,jz=0;while(em<eo.Length){
if(aB<av.Length&&(av[aB]=='?'||char.ToLowerInvariant(av[aB])==char.ToLowerInvariant(eo[em]))){aB++;em++;}else if(aB<av.Length&&av[aB]=='*'){
kr=aB;jz=em;aB++;}else if(kr!= -1){aB=kr+1;jz++;em=jz;}else{return false;}}while(aB<av.Length&&av[aB]=='*')aB++;return aB==av.Length;
}public static int pD(double sQ,int pa){return Math.Max(0,Math.Min(pa,1+(int)(Math.Max(0.0,sQ)*pa)));}public static string e9(string[]dl,double jW){
if(dl==null||dl.Length==0)return "white";jW=Math.Max(0,Math.Min(100,jW));int jb=(int)Math.Floor(jW/100.0*dl.Length);if(jb>=dl.Length)jb=dl.Length-1;
return dl[jb];}public static n lg(string[]b0,double sZ){if(b0==null||b0.Length==0)return n.fl;if(b0.Length==1)return n.dJ(b0[0]);
double tl=Math.Max(0.0,Math.Min(1.0,sZ/100.0));double oC=tl*(b0.Length-1);int jc=(int)Math.Floor(oC);int r7=Math.Min(b0.Length-1,jc+1);
double kv=oC-jc;n cX=n.dJ(b0[jc]);n iq=n.dJ(b0[r7]);byte tj=(byte)Math.Round(cX.bC+(iq.bC-cX.bC)*kv);
byte r0=(byte)Math.Round(cX.bx+(iq.bx-cX.bx)*kv);byte q7=(byte)Math.Round(cX.bs+(iq.bs-cX.bs)*kv);return new n(tj,r0,q7);}public static string dK(List<aO>ou,double ub){
foreach(var j9 in ou){if(j9.cL)continue;if(pV(j9.hj,ub))return j9.ca;}var mE=ou.FirstOrDefault(tk=>tk.cL);return mE!=null ? mE.ca : null;
}static bool pV(string ec,double eV){ec=ec.Trim();foreach(var jH in new[]{">=","<=","==",">","<"}){if(ec.StartsWith(jH)){double eH=double.Parse(ec.Substring(jH.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(jH){case ">=": return eV>=eH;case "<=": return eV<=eH;case "==": return Math.Abs(eV-eH)<1e-9;case ">": return eV>eH;case "<": return eV<eH;
}}}throw new FormatException($"unsupported comparison expression: '{ec}'");}public static List<string>qr(List<string>ru,int dq){
var gl=new List<string>();gl.Add("+"+new string('-',dq+2)+"+");foreach(var jw in ru){string t2=jw.Length>dq ? jw.Substring(0,dq): jw;
string sT=t2.PadRight(dq);gl.Add("| "+sT+" |");}gl.Add("+"+new string('-',dq+2)+"+");return gl;}public struct ls{public float lI,lJ,lH,li;
}public static ls pJ(int mv,int oq,int gM,int gN,int eQ,int eR,float tU,float tT,float n8,float n9,float i_,float i0){
eQ=Math.Max(1,eQ);eR=Math.Max(1,eR);gM=Math.Max(1,Math.Min(eQ-mv,gM));gN=Math.Max(1,Math.Min(eR-oq,gN));
float q0=tU-(n8*2f);float q_=tT-(n9*2f);float mq=(q0-(i_*(eQ-1)))/eQ;float mp=(q_-(i0*(eR-1)))/eR;
float s9=n8+mv*(mq+i_);float ta=n9+oq*(mp+i0);float s8=gM*mq+((gM-1)*i_);float s0=gN*mp+((gN-1)*i0);return new ls{
lI=s9,lJ=ta,lH=s8,li=s0};}public struct lw{public int p9;public int ht;public int ad;public int ay;public int dP;
public int a0;public int qG;public int qH;public int pS;public int pT;
public double pU;}public static lw pK(z bZ){var l=new lw();var ob=bZ.ah!=null&&bZ.ah.Count>0 ? bZ.ah : new List<az>{
new az{at=bZ.fe}};l.p9=ob.Count;l.ht=0;l.ad=0;l.ay=0;if(!string.IsNullOrEmpty(bZ.bt)&& !bZ.bt.Equals("none",StringComparison.OrdinalIgnoreCase))l.ad++;
if(!string.IsNullOrEmpty(bZ.dw)&& !bZ.dw.Equals("none",StringComparison.OrdinalIgnoreCase))l.ad+=2;foreach(var ez in ob){
if(!string.IsNullOrEmpty(ez.cH)&& !ez.cH.Equals("none",StringComparison.OrdinalIgnoreCase))l.ad++;if(!string.IsNullOrEmpty(ez.cj))l.ad+=2;
l.ht+=ez.at.Count;foreach(var so in ez.at){l.qG++;l.qH++;l.ad+=2;switch(so.h){case e.dt: l.dP++;
l.a0++;l.ad+=2;l.ay++;break;case e.dH: l.a0+=10;l.ad+=10;
l.ay+=10;break;case e.dE: l.a0+=10;l.ad+=11;l.ay+=10;break;case e.dx: l.a0+=10;
l.ad+=10;l.ay+=10;break;case e.dz: l.a0+=12;l.ad+=12;l.ay+=12;
break;case e.ch: l.dP++;l.a0+=8;l.ad+=9;l.ay+=8;break;case e.cI: l.dP+=2;
l.a0++;l.ad+=3;l.ay++;break;case e.ck: case e.b9: l.dP++;
l.a0++;l.ad+=2;l.ay++;break;case e.ci: case e.cb: l.dP++;l.a0+=2;
l.ad+=3;l.ay+=2;break;case e.cG: l.a0+=8;l.ad+=8;l.ay+=8;
break;default: l.ay++;break;}}}l.pS=25+(l.ht*2);l.pT=180+(l.ay*4);
l.pU=0.0;return l;}}