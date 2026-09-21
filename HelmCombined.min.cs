hx q;string hM=null;Dictionary<string,List<IMyTerminalBlock>>cn=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>fn=new Dictionary<string,bool>();
Dictionary<string,bg>dR=new Dictionary<string,bg>();Dictionary<string,Color>lF=new Dictionary<string,Color>();List<IMyTerminalBlock>ck=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>cP=new List<IMyTerminalBlock>();bool fk;int hJ;string dS="";StringBuilder dW=new StringBuilder();readonly Dictionary<string,double>aG=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>dT=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener dU;bool cl=false;
Dictionary<z,IMyTextSurface>bG=new Dictionary<z,IMyTextSurface>();Dictionary<z,string>cm=new Dictionary<z,string>();
static readonly string[]lm=new string[101];static Program(){for(int fX=0;fX<=100;fX++){lm[fX]=fX+"%";}}static string k9(double tZ){int en=(int)Math.Round(tZ);
if(en<0)en=0;else if(en>100)en=100;return lm[en];}class aA{public List<MySprite>f=new List<MySprite>();public List<cc>dK=new List<cc>();
public int b5= -1;public Color[]eZ;public Color eV;public bool eW;public Vector2 dE,ll;public int dP;public bool dD=false;
public List<e_>dt=new List<e_>();}class e_{public int e8;public Color g4,e3;public List<aO>fg,e6;public string g8,hh,hi,hk;
public bool lg,hm;}class cc{public bA fb;public int V= -1;public int aS= -1;public int cM= -1;public bool lc=false;
public List<int>K=new List<int>();public float[]fh=new float[8];public float b4;public float bf;public float aZ;
public float b3;public double fa= -99999.0;public string li;public float D= -1f;public bool lA;public float k_=12f;}Dictionary<z,aA>bH=new Dictionary<z,aA>();
int lH=0;double hO=0;double dX=0;double ae=0;int dV=0;Dictionary<z,double>bj=new Dictionary<z,double>();
Dictionary<z,double>hI=new Dictionary<z,double>();Dictionary<string,int>d_=new Dictionary<string,int>();List<string>cQ=new List<string>();
Dictionary<string,double>dZ=new Dictionary<string,double>();List<MyInventoryItem>hL=new List<MyInventoryItem>();int bI= -1,fp;double hP;
Dictionary<z,double>fl=new Dictionary<z,double>();int dY=0;const string pT="0.11.0";IMyTextSurface aT;bool hH=true;
double fm=0;string lG="";string hN="";int fo=0,hK=0;List<KeyValuePair<string,int>>cO=new List<KeyValuePair<string,int>>();
const double qa=120;const float po=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{dU=IGC.RegisterBroadcastListener("HELM");
dU.SetMessageCallback("HELM_IGC");}catch{}ff();hB();lf();}public void Main(string lT,UpdateType tV){
double mJ=Runtime.TimeSinceLastRun.TotalSeconds;ae+=mJ;if((tV&UpdateType.IGC)!=0||lT=="HELM_IGC"){p8();}string of=(lT ?? "").Trim();
string al=of.ToLowerInvariant();if(al=="reload"){ff();}else if(al=="refresh"){hz();}else if(al=="boot"){hB();}else if(al=="probe"){
dX=ae+20.0;pI();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(al=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=qv;
ff();hB();}else if(al=="console on"||al=="console off"){hH=al=="console on";lf();fm=0;}else if(al=="scan"||al=="diagnose"||al=="test"){
qt();return;}else if(al.StartsWith("set ")){string gw=of.Substring(4).Trim();int mN=gw.IndexOf('=');int tv=gw.IndexOf(' ');int kg=mN>0 ? mN : tv;
if(kg>0){string ns=gw.Substring(0,kg).Trim();string o4=gw.Substring(kg+1).Trim();kS($"{ns}={o4}");Echo($"[HELM] Set dynamic variable '{ns}' = {o4}");
}return;}else if(al.StartsWith("alert")){string eR=al.Length>5 ? al.Substring(5).Trim(): "toggle";if(eR=="toggle")cl= !cl;else cl=eR=="red"||eR=="on"||eR=="1"||eR=="true";
Echo($"[HELM] Red alert state: {cl}");return;}else if(Me.CustomData!=hM){ff();}dY++;hO+=mJ;if(hO>=qa){
hO=0;hz();}if(dX>0){if(ae<dX){hG();return;}dX=0;bH.Clear();cm.Clear();
fm=0;}if(q==null||q.S.Count==0){hG();k1();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cy=q.S[lH%q.S.Count];
lH++;hN=cy.a4;bool hT=bj.Count>0;if(hT)qh();double mK;if(!bj.ContainsKey(cy)&& !(hI.TryGetValue(cy,out mK)&&ae<mK)){
if(qm(cy))hT=true;if(cy.hy>0)hI[cy]=ae+cy.hy/60.0;}dV=hT ? 0 : dV+1;Runtime.UpdateFrequency=dV<=q.S.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
qu();hG();k1();fo=Runtime.CurrentInstructionCount;if(fo>hK)hK=fo;}void k1(){if(dS.Length>0)Echo(dS);
if(dW.Length>0)Echo(dW.ToString());}List<IMyTerminalBlock>eX(bool qF=false){if(!fk){cP.Clear();GridTerminalSystem.GetBlocks(cP);
ck.Clear();ck.AddRange(cP);ck.RemoveAll(qS=> !qS.IsSameConstructAs(Me));hJ=cP.Count-ck.Count;fk=true;
}return qF ? cP : ck;}IMyTerminalBlock k4(string nP,bool qH=false){var h1=GridTerminalSystem.GetBlockWithName(nP);if(h1==null||qH||h1.IsSameConstructAs(Me))return h1;
var hV=eX();for(int fY=0;fY<hV.Count;fY++)if(hV[fY].CustomName.IndexOf(nP,StringComparison.OrdinalIgnoreCase)>=0)return hV[fY];return null;}void pz(string nQ,List<IMyTerminalBlock>ep,bool lM=false){
var m4=GridTerminalSystem.GetBlockGroupWithName(nQ);if(m4==null)return;int l3=ep.Count;var eN=new List<IMyTerminalBlock>();m4.GetBlocks(eN);kQ(eN,ep,lM);
if(ep.Count>l3)return;var nf=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(nf);foreach(var pi in nf){if(pi.Name.IndexOf(nQ,StringComparison.OrdinalIgnoreCase)<0)continue;
eN.Clear();pi.GetBlocks(eN);kQ(eN,ep,lM);if(ep.Count>l3)return;}}void kQ(List<IMyTerminalBlock>rM,List<IMyTerminalBlock>nq,bool qG=false){
foreach(var h2 in rM)if((qG||h2.IsSameConstructAs(Me))&& !nq.Contains(h2))nq.Add(h2);}void ff(){hM=Me.CustomData;q=le.p1(hM);
dS="";if(q.a1.Count>0){var os=new StringBuilder("HELM config errors:\n");foreach(var rv in q.a1)os.AppendLine(" - "+rv);dS=os.ToString();
}if(q.dQ.Count>0){var ph=new StringBuilder("HELM notes:\n");foreach(var t3 in q.dQ)ph.AppendLine(" - "+t3);dS+=ph.ToString();}dR.Clear();
foreach(var h3 in q.b6)if(!dR.ContainsKey(h3.ab))dR[h3.ab]=h3;bH.Clear();cm.Clear();bj.Clear();hI.Clear();
fl.Clear();pO();hz();}void p8(){try{while(dU!=null&&dU.HasPendingMessage){
var nL=dU.AcceptMessage();if(nL.Data is string){kS((string)nL.Data);}}}catch{}}void kS(string n6){
if(string.IsNullOrEmpty(n6))return;var n3=n6.Split(';');for(int i_=0;i_<n3.Length;i_++){var eA=n3[i_].Trim();if(eA.Length==0)continue;int ea=eA.IndexOf('=');
if(ea<0)ea=eA.IndexOf(':');if(ea>0){string c4=eA.Substring(0,ea).Trim();string be=eA.Substring(ea+1).Trim();if(c4.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cl=be.Equals("red",StringComparison.OrdinalIgnoreCase)||be=="1"||be.Equals("true",StringComparison.OrdinalIgnoreCase)||be.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double nT;dT.Remove(c4);if(double.TryParse(be,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nT)){
aG[c4]=nT;}else if(be.Equals("true",StringComparison.OrdinalIgnoreCase)||be.Equals("on",StringComparison.OrdinalIgnoreCase)){aG[c4]=100.0;
}else if(be.Equals("false",StringComparison.OrdinalIgnoreCase)||be.Equals("off",StringComparison.OrdinalIgnoreCase)){aG[c4]=0.0;}else{dT[c4]=be;
}}}}}const string qv="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void pI(){var hW=new List<IMyTextSurface>();if(aT!=null)hW.Add(aT);foreach(var sz in bG.Values)hW.Add(sz);foreach(var tq in hW)pH(tq);
}static void bi(List<MySprite>tw,float rp,float rq,float t4,float rT,Color rc){tw.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(rp,rq),Size=new Vector2(t4,rT),Color=rc}
);}void pH(IMyTextSurface kh){Vector2 aJ=kh.SurfaceSize,aD=kh.TextureSize,ak=(aD-aJ)/2f;var aC=new List<MySprite>();Color fV=new Color(150,150,150);
bi(aC,aD.X/2f,2f,aD.X,4f,fV);bi(aC,aD.X/2f,aD.Y-2f,aD.X,4f,fV);bi(aC,2f,aD.Y/2f,4f,aD.Y,fV);bi(aC,aD.X-2f,aD.Y/2f,4f,aD.Y,fV);
bi(aC,ak.X+aJ.X/2f,ak.Y+aJ.Y/2f,aJ.X,aJ.Y,new Color(20,40,90));float aj=32f;bi(aC,ak.X+aj/2f,ak.Y+aj/2f,aj,aj,new Color(255,0,0));bi(aC,ak.X+aJ.X-aj/2f,ak.Y+aj/2f,aj,aj,new Color(0,255,0));
bi(aC,ak.X+aj/2f,ak.Y+aJ.Y-aj/2f,aj,aj,new Color(255,255,0));bi(aC,ak.X+aJ.X-aj/2f,ak.Y+aJ.Y-aj/2f,aj,aj,new Color(255,0,255));float kO=ak.Y+aJ.Y/2f-30f;
aY(aC,"SURFACE "+aJ.X.ToString("0")+"x"+aJ.Y.ToString("0"),Color.White,ak.X+44f,kO,0.9f);aY(aC,"TEXTURE "+aD.X.ToString("0")+"x"+aD.Y.ToString("0"),Color.White,ak.X+44f,kO+24f,0.9f);
aY(aC,"OFFSET  "+ak.X.ToString("0")+","+ak.Y.ToString("0"),Color.White,ak.X+44f,kO+48f,0.9f);using(var rF=kh.DrawFrame()){rF.AddRange(aC);}}static bool lh(string iH){
return iH.Length>3&&iH.StartsWith("top")&&char.IsDigit(iH[3]);}void pO(){d_.Clear();cQ.Clear();bI= -1;hP=0;foreach(var kc in q.S){
var js=new List<List<bA>>();foreach(var sV in kc.ah)js.Add(sV.at);if(kc.ah.Count==0)js.Add(kc.fc);foreach(var su in js)foreach(var gf in su){
if(!lh(gf.I))continue;int sv=int.Parse(gf.I.Substring(3));int nj;d_.TryGetValue(gf.aN,out nj);d_[gf.aN]=Math.Max(nj,sv);}}cQ.AddRange(d_.Keys);
}void qu(){if(cQ.Count==0)return;if(bI<0){if(ae<hP)return;bI=0;fp=0;dZ.Clear();}while(bI<cQ.Count&&Runtime.CurrentInstructionCount<20000){
string ic=cQ[bI];List<IMyTerminalBlock>fz;cn.TryGetValue(ic,out fz);if(fz!=null&&fp<fz.Count){var lV=fz[fp++];
for(int i0=0;i0<lV.InventoryCount;i0++){hL.Clear();lV.GetInventory(i0).GetItems(hL);foreach(var f5 in hL){string jg=f5.Type.TypeId.Replace("MyObjectBuilder_","");
string nt=jg=="Ore"||jg=="Ingot" ? f5.Type.SubtypeId+" "+jg.ToLowerInvariant(): f5.Type.SubtypeId;double nk;dZ.TryGetValue(nt,out nk);dZ[nt]=nk+(double)f5.Amount;
}}continue;}var gu=new List<KeyValuePair<string,double>>(dZ);gu.Sort((t6,ua)=>ua.Value.CompareTo(t6.Value));for(int dc=1;dc<=d_[ic];dc++)dT[ic+".top"+dc]=dc<=gu.Count ? gu[dc-1].Key+" "+H.k8(gu[dc-1].Value,"auto"): "-";
dZ.Clear();bI++;fp=0;}if(bI>=cQ.Count){bI= -1;hP=ae+15.0;}}void ag(int tG,string tO){cO.Add(new KeyValuePair<string,int>(tO,tG));
}static string hF(string j5,int nO){return j5.Length>nO ? j5.Substring(0,nO): j5;}void lf(){var jY=Me as IMyTextSurfaceProvider;if(jY==null||jY.SurfaceCount<1)return;
aT=jY.GetSurface(0);aT.ContentType=hH ? ContentType.SCRIPT : ContentType.NONE;aT.Script="";aT.ScriptBackgroundColor=Color.Black;
}void hG(){if(ae>=fm){fm=ae+(dV>q.S.Count+1 ? 5.0 : 1.0);pw();if(hH&&aT!=null&&dX==0)pG();
}Echo(lG);}void pw(){cO.Clear();int ig=0,ma=0;foreach(var lW in q.b6){List<IMyTerminalBlock>jm;if(lW.bz==a_.dy){
ig++;continue;}if(cn.TryGetValue(lW.ab,out jm)){ma+=jm.Count;if(jm.Count>0)ig++;}}int mO=q.a1.Count;ag(0,"HELM-[Kernel-(v"+pT+")]");
ag(4,"------------------------------");ag(1,"krn-cfg");ag(2," screens "+q.S.Count+"  binds "+ig+"/"+q.b6.Count+"  errors "+mO);
ag(2," alert "+(cl ? "RED" : "off")+"  blocks "+ma+"  docked-ignored "+hJ);if(mO>0)ag(3," ! "+hF(q.a1[0],40));
else if(q.dQ.Count>0)ag(3," ? "+hF(q.dQ[0],40));ag(1,"cfg-dsp");if(q.S.Count==0)ag(3," no @screen: run 'starter'");
int ts=0;foreach(var j6 in q.S){if(++ts>10){ag(2," +"+(q.S.Count-10)+" more");break;}IMyTextSurface ki;string oC="--",kn="no block";
if(bG.TryGetValue(j6,out ki)){oC=ki.SurfaceSize.X.ToString("0")+"x"+ki.SurfaceSize.Y.ToString("0");kn=bj.ContainsKey(j6)? "boot" : "live";
}ag(kn=="no block" ? 3 : 2," "+hF(j6.a4,14).PadRight(14)+" "+oC.PadRight(9)+" "+kn);}ag(1,"perf");ag(2," tick "+dY+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ag(2," instr "+fo+"  peak "+hK+"  max "+Runtime.MaxInstructionCount);ag(2," rate "+Runtime.UpdateFrequency+"  idle "+dV+"  up "+(int)ae+"s");
if(hN.Length>0)ag(2," last "+hN);var ot=new StringBuilder();foreach(var r8 in cO)ot.AppendLine(r8.Key);lG=ot.ToString();}static Color pC(int tH){
switch(tH){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void pG(){Vector2 cz=aT.SurfaceSize;Vector2 jz=(aT.TextureSize-cz)/2f;int ip=30;foreach(var r9 in cO)ip=Math.Max(ip,r9.Key.Length);
Vector2 io=aT.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float ou=Math.Min(1f,Math.Min(cz.X*0.94f/(ip*io.X),cz.Y*0.94f/(cO.Count*io.Y)));
float t7=jz.X+cz.X*0.03f;float pl=jz.Y+cz.Y*0.03f;var kj=new List<MySprite>();kj.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jz+cz/2f,Size=cz,Color=new Color(12,12,16)}
);foreach(var ny in cO){aY(kj,ny.Key,pC(ny.Value),t7,pl,ou);pl+=io.Y*ou;}using(var rG=aT.DrawFrame()){rG.AddRange(kj);}}static readonly List<string>pv=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void hB(){bj.Clear();if(q==null)return;for(int fZ=0;fZ<q.S.Count;fZ++){var gA=q.S[fZ];if(gA.hb>0f)bj[gA]=ae+(fZ/Math.Max(1,gA.kU))*gA.kX/60.0;
}}void qh(){List<z>d9=null;int rS=(bj.Count+3)/4,r1=0;foreach(var f9 in bj){double jH=(ae-f9.Value)/f9.Key.hb;if(jH<0)continue;
if(jH>=1.0){if(d9==null)d9=new List<z>();d9.Add(f9.Key);continue;}if((r1++ +dY)%rS==0)pF(f9.Key,jH);}if(d9==null)return;foreach(var eI in d9){
bj.Remove(eI);if(eI.ha>0f)fl[eI]=ae;bH.Remove(eI);cm.Remove(eI);}}static void aY(List<MySprite>tx,string tP,Color re,float t8,float ub,float tn){
var oQ=MySprite.CreateText(tP,"Monospace",re,tn,TextAlignment.LEFT);oQ.Position=new Vector2(t8,ub);tx.Add(oQ);}static void kP(List<MySprite>ty,List<string>fu,z gD,float t9,float uc,float to,float sk){
string[]tE=gD.e0!=null&&gD.e0.Length>0 ? gD.e0 : new[]{gD.kV};for(int ei=0;ei<fu.Count;
ei++){n ii=H.lb(tE,fu.Count>1 ? 100.0*ei/(fu.Count-1): 0.0);aY(ty,fu[ei],new Color(ii.bC,ii.bx,ii.bs),t9,uc+ei*sk,to);}}bool ql(z gE,IMyTextSurface kr){
aA cB,aV;z ox=q.S.Find(pj=>pj!=gE&&pj.a4.Equals(gE.hs,StringComparison.OrdinalIgnoreCase));if(ox==null|| !bH.TryGetValue(ox,out cB)|| !cB.dD)return false;
if(!bH.TryGetValue(gE,out aV))bH[gE]=aV=new aA();Vector2 dg=kr.SurfaceSize;if(aV.dD&&aV.dP==cB.dP&&aV.dE==dg)return false;
aV.dD=true;aV.dP=cB.dP;aV.dE=dg;aV.f.Clear();Vector2 kx=cB.dE,sB=(kr.TextureSize-dg)/2f;float f7=Math.Min(dg.X/kx.X,dg.Y/kx.Y);
Vector2 qP=sB+(dg-kx*f7)/2f;foreach(var tz in cB.f){var bm=tz;if(bm.Position.HasValue)bm.Position=(bm.Position.Value-cB.ll)*f7+qP;if(bm.Size.HasValue)bm.Size=bm.Size.Value*f7;
if(bm.Type==SpriteType.TEXT)bm.RotationOrScale*=f7;aV.f.Add(bm);}using(var rH=kr.DrawFrame())rH.AddRange(aV.f);return true;}bool qf(z de,IMyTextSurface gL){
List<string>d1;if(cm.ContainsKey(de)|| !q.dq.TryGetValue(de.b2,out d1))return false;cm[de]="banner";Vector2 cA=gL.SurfaceSize;
Vector2 jA=(gL.TextureSize-cA)/2f;int fF=1;foreach(var sa in d1)fF=Math.Max(fF,sa.Length);Vector2 d7=gL.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float gC=Math.Min(cA.X*0.94f/(fF*d7.X),cA.Y*0.94f/(d1.Count*d7.Y));var kk=new List<MySprite>();kk.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jA+cA/2f,Size=cA,Color=string.IsNullOrEmpty(de.bt)? new Color(10,12,16): F(de.bt)}
);kP(kk,d1,de,jA.X+(cA.X-fF*d7.X*gC)/2f,jA.Y+(cA.Y-d1.Count*d7.Y*gC)/2f,gC,d7.Y*gC);using(var rI=gL.DrawFrame()){rI.AddRange(kk);
}return true;}void pF(z df,double nY){IMyTextSurface eK;if(!bG.TryGetValue(df,out eK))return;Vector2 bZ=eK.SurfaceSize;
Vector2 jB=(eK.TextureSize-bZ)/2f;var et=df.pP();string nM=df.kW ?? "HELM TELEMETRY KERNEL ONLINE";bool jt=bZ.Y>=200f;
List<string>cR;if(df.b7==null|| !q.dq.TryGetValue(df.b7,out cR))cR=pv;int cY=nM.Length+3;foreach(var sb in et)cY=Math.Max(cY,sb.Length+5);
if(jt)foreach(var sc in cR)cY=Math.Max(cY,sc.Length);int oj=(jt ? cR.Count+1 : 0)+2+et.Count;Vector2 cX=eK.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bV=Math.Min(1f,Math.Min(bZ.X*0.94f/(cY*cX.X),bZ.Y*0.94f/(oj*cX.Y)));float g0=jB.X+(bZ.X-cY*cX.X*bV)/2f;float cF=jB.Y+(bZ.Y-oj*cX.Y*bV)/2f;
float gd=cX.Y*bV;var dh=new List<MySprite>();dh.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jB+bZ/2f,Size=bZ,Color=new Color(10,12,16)}
);if(jt){kP(dh,cR,df,g0,cF,bV,gd);cF+=(cR.Count+1)*gd;}aY(dh,">> "+nM,new Color(34,197,94),g0,cF,bV);cF+=gd*2f;int tt=H.pu(nY,et.Count);
for(int f_=0;f_<tt;f_++){bool nV=f_==et.Count-1&&nY>=0.8;aY(dh,nV ? "[OK]" : "[..]",nV ? new Color(34,197,94): new Color(56,189,248),g0,cF,bV);aY(dh,et[f_],new Color(203,213,225),g0+5f*cX.X*bV,cF,bV);
cF+=gd;}using(var rJ=eK.DrawFrame()){rJ.AddRange(dh);}}void hz(){cn.Clear();bG.Clear();fn.Clear();
dW.Clear();fk=false;if(q==null)return;foreach(var aa in q.b6){var aU=new List<IMyTerminalBlock>();if(aa.bz==a_.dy){
string je= !string.IsNullOrEmpty(aa.bh)? aa.bh : aa.ab;if(!aG.ContainsKey(je)){aG[je]=aa.e4;if(aa.he!=null)dT[je]=aa.he;
}cn[aa.ab]=aU;continue;}var oU=aa.aR.Count>0 ? aa.aR : new List<string>{aa.lx};foreach(var cD in oU){
if(string.IsNullOrEmpty(cD))continue;bool nr=cD.Contains("*")||cD.Contains("?");if(aa.bz==a_.ho||nr){var iV=new List<IMyTerminalBlock>();
pz(cD,iV,aa.eY);if(iV.Count>0){foreach(var nb in iV){if(!aU.Contains(nb))aU.Add(nb);}}else{var lO=eX(aa.eY);
for(int h4=0;h4<lO.Count;h4++){var gS=lO[h4];if(nr ? H.pY(cD,gS.CustomName): string.Equals(cD,gS.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aU.Contains(gS))aU.Add(gS);}}}}else{var h9=k4(cD,aa.eY);if(h9!=null&& !aU.Contains(h9))aU.Add(h9);}}if(aU.Count==0){
qr(aa,oU,aU);if(aU.Count>0)fn[aa.ab]=true;}cn[aa.ab]=aU;}foreach(var bp in q.S){
if(string.IsNullOrEmpty(bp.bB))continue;var jE=k4(bp.bB);if(jE==null){dW.AppendLine($"HELM: output block '{bp.bB}' not found");
continue;}IMyTextSurface cC=null;var jZ=jE as IMyTextSurfaceProvider;var mD=jE as IMyTextSurface;if(jZ!=null&&bp.cd<jZ.SurfaceCount){
cC=jZ.GetSurface(bp.cd);}else if(mD!=null&&bp.cd==0){cC=mD;}if(cC==null){dW.AppendLine($"HELM: output block '{bp.bB}' has no surface index {bp.cd}");
continue;}cC.ContentType=ContentType.SCRIPT;cC.Script="";cC.ScriptBackgroundColor=Color.Black;bG[bp]=cC;}ck.Clear();
cP.Clear();}static readonly string[]k3={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool pN(int r2,IMyTerminalBlock u){switch(r2){case 0: return u is IMyBatteryBlock;case 1: return u is IMyReactor;case 2: return u is IMySolarPanel;
case 3: return u is IMyWindTurbine;case 4: return u is IMyGasTank;case 5: return u is IMyAirVent;case 6: return u is IMyCargoContainer;case 7: return u is IMyJumpDrive;
case 8: return u is IMyThrust;case 9: return u is IMyMotorSuspension;case 10: return u is IMyDoor;case 11: return u is IMyShipConnector;case 12: return u is IMyAssembler;
case 13: return u is IMyRefinery;case 14: return u is IMyShipDrill;case 15: return u is IMyShipWelder;case 16: return u is IMyShipGrinder;case 17: return u is IMyGasGenerator;
case 18: return u is IMyLargeTurretBase;case 19: return u is IMyProjector;case 20: return u is IMyGyro;case 21: return u is IMyGravityGeneratorBase;case 22: return u is IMyLandingGear;
case 23: return u is IMyRadioAntenna;case 24: return u is IMyBeacon;case 25: return u is IMyLightingBlock;case 26: return u is IMyPistonBase;case 27: return u is IMyMotorStator;
case 28: return u is IMyWarhead;case 29: return u is IMySensorBlock;case 30: return u is IMyCameraBlock;case 31: return u is IMyParachute;case 32: return u is IMyTurretControlBlock;
}return false;}void qr(bg q3,List<string>tM,List<IMyTerminalBlock>ia){string rX=(q3.ab+" "+string.Join(" ",tM)).ToLowerInvariant();
var ji=new List<int>();for(int f8=0;f8<k3.Length;f8++){foreach(var t5 in k3[f8].Split(','))if(rX.Contains(t5)){ji.Add(f8);break;}}if(ji.Count==0)return;
var lN=eX();var nW=new List<IMyTerminalBlock>();for(int i1=0;i1<lN.Count;i1++){var h5=lN[i1];foreach(int r3 in ji){if(pN(r3,h5)){(h5.CubeGrid==Me.CubeGrid ? ia : nW).Add(h5);
break;}}}if(ia.Count==0)ia.AddRange(nW);}void qt(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(q==null||q.S.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {q.S.Count} screens, {q.b6.Count} binds");fk=false;
Echo($"Blocks: {eX().Count} on this construct; {hJ} on docked ships (ignored)");ck.Clear();Echo("--- BIND RESOLUTION ---");foreach(var an in q.b6){
if(an.bz==a_.dy){string jf= !string.IsNullOrEmpty(an.bh)? an.bh : an.ab;double tW=aG.ContainsKey(jf)? aG[jf]: an.e4;
Echo($"[OK] @bind {an.ab} (dynamic variable '{jf}' = {tW})");continue;}List<IMyTerminalBlock>ib;if(cn.TryGetValue(an.ab,out ib)){if(ib.Count>0){
string rB=fn.ContainsKey(an.ab)&&fn[an.ab]? " [fallback: typed match]" : "";Echo($"[OK] @bind {an.ab} ({ib.Count} blocks matched{rB})");
}else{string tN=string.Join(", ",an.aR.Count>0 ? an.aR : new List<string>{an.lx});Echo($"[WARN] @bind {an.ab} -> 0 blocks matched \"{tN}\"");
}}else{Echo($"[ERR] @bind {an.ab} -> unresolved");}}if(aG.Count>0){Echo($"--- DYNAMIC VARIABLES ({aG.Count}) ---");foreach(var nA in aG)Echo($"[VAR] {nA.Key} = {nA.Value}");
}IMyTextSurface hX=null;foreach(var sD in bG.Values){hX=sD;break;}if(hX!=null){var kl=new List<string>();hX.GetSprites(kl);
Echo($"--- SPRITES ({kl.Count}): names usable in bg_texture ---");Echo(string.Join(", ",kl.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bq in q.S){IMyTextSurface gM;if(bG.TryGetValue(bq,out gM)){var oD=gM.SurfaceSize;var tC=H.pB(bq);
Echo($"[OK] Screen \"{bq.a4}\" [{bq.ce}] -> {bq.bB} [#{bq.cd}] ({oD.X:0}x{oD.Y:0} px, texture {gM.TextureSize.X:0}x{gM.TextureSize.Y:0}, {tC.ac} sprites)");
}else{Echo($"[ERR] Screen \"{bq.a4}\" -> Missing block \"{bq.bB}\" [#{bq.cd}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double qs(IMyTerminalBlock v){
if(v==null)return 0.0;var fx=v as IMyBatteryBlock;if(fx!=null)return fx.MaxStoredPower>0 ?(fx.CurrentStoredPower/fx.MaxStoredPower)*100.0 : 0.0;
var f6=v as IMyJumpDrive;if(f6!=null)return f6.MaxStoredPower>0 ?(f6.CurrentStoredPower/f6.MaxStoredPower)*100.0 : 0.0;var oT=v as IMyGasTank;
if(oT!=null)return oT.FilledRatio*100.0;var pd=v as IMyAirVent;if(pd!=null)return pd.GetOxygenLevel()*100.0;var mi=v as IMyCargoContainer;if(mi!=null){
var f3=mi.GetInventory();if(f3!=null&&f3.MaxVolume.RawValue>0)return(double)f3.CurrentVolume.RawValue/f3.MaxVolume.RawValue*100.0;return 0.0;}var gv=v as IMyReactor;
if(gv!=null)return gv.MaxOutput>0 ?(gv.CurrentOutput/gv.MaxOutput)*100.0 : 0.0;var gF=v as IMySolarPanel;if(gF!=null)return gF.MaxOutput>0 ?(gF.CurrentOutput/gF.MaxOutput)*100.0 : 0.0;
var g_=v as IMyWindTurbine;if(g_!=null)return g_.MaxOutput>0 ?(g_.CurrentOutput/g_.MaxOutput)*100.0 : 0.0;var mE=v as IMyDoor;if(mE!=null)return(double)mE.OpenRatio*100.0;
var gV=v as IMyThrust;if(gV!=null)return gV.MaxEffectiveThrust>0 ?(gV.CurrentThrust/gV.MaxEffectiveThrust)*100.0 : 0.0;var kw=v as IMyMotorSuspension;
if(kw!=null)return kw.Brake ? 100.0 :(kw.Friction*100.0);var iu=v as IMyShipConnector;if(iu!=null)return iu.Status==MyShipConnectorStatus.Connected ? 100.0 :(iu.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var ob=v as IMyProductionBlock;if(ob!=null)return ob.IsProducing ? 100.0 : 0.0;var mI=v as IMyShipDrill;if(mI!=null)return mI.Enabled ? 100.0 : 0.0;
var kC=v as IMyShipToolBase;if(kC!=null)return kC.IsActivated ? 100.0 :(kC.Enabled ? 50.0 : 0.0);var iS=v as IMyGasGenerator;if(iS!=null)return iS.AutoRefill ? 100.0 :(iS.Enabled ? 50.0 : 0.0);
var gW=v as IMyLargeTurretBase;if(gW!=null)return gW.IsShooting ? 100.0 :(gW.HasTarget ? 50.0 :(gW.Enabled ? 10.0 : 0.0));var eC=v as IMyProjector;
if(eC!=null)return eC.TotalBlocks>0 ?((double)(eC.TotalBlocks-eC.RemainingBlocks)/eC.TotalBlocks)*100.0 : 0.0;var iY=v as IMyGyro;if(iY!=null)return iY.GyroOverride ? 100.0 :(iY.Enabled ? 50.0 : 0.0);
var nd=v as IMyGravityGeneratorBase;if(nd!=null)return Math.Min(100.0,Math.Abs(nd.GravityAcceleration)/9.81*100.0);var iR=v as IMyLandingGear;if(iR!=null)return iR.IsLocked ? 100.0 :(iR.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var lP=v as IMyRadioAntenna;if(lP!=null)return(lP.Radius/50000.0)*100.0;var l1=v as IMyBeacon;if(l1!=null)return(l1.Radius/50000.0)*100.0;
var nH=v as IMyLightingBlock;if(nH!=null)return nH.Intensity*10.0;var gr=v as IMyPistonBase;if(gr!=null)return gr.HighestPosition>0 ?(gr.CurrentPosition/gr.HighestPosition)*100.0 : 0.0;
var oh=v as IMyMotorStator;if(oh!=null)return Math.Min(100.0,Math.Abs(oh.Angle)/6.28318*100.0);var pg=v as IMyWarhead;if(pg!=null)return pg.IsArmed ? 100.0 : 0.0;
var kf=v as IMySensorBlock;if(kf!=null)return!kf.LastDetectedEntity.IsEmpty()? 100.0 :(kf.Enabled ? 25.0 : 0.0);var il=v as IMyCameraBlock;
if(il!=null)return il.IsActive ? 100.0 :(il.Enabled ? 50.0 : 0.0);var mo=v as IMyParachute;if(mo!=null)return(double)mo.OpenRatio*100.0;
var gU=v as IMyTurretControlBlock;if(gU!=null)return gU.HasTarget ? 100.0 :(gU.IsUnderControl ? 50.0 :(gU.Enabled ? 10.0 : 0.0));var m2=v as IMyFunctionalBlock;
if(m2!=null)return m2.Enabled ? 100.0 : 0.0;return 0.0;}static double pV(IMyTerminalBlock lX,string iK){int mh=iK.IndexOf(':');string jh=iK.Substring(0,mh).ToLowerInvariant();
var tU=new MyItemType("MyObjectBuilder_"+(jh=="ammo" ? "AmmoMagazine" : char.ToUpper(jh[0])+jh.Substring(1)),iK.Substring(mh+1));double oM=0;for(int i2=0;
i2<lX.InventoryCount;i2++)oM+=(double)lX.GetInventory(i2).GetItemAmount(tU);return oM;}double lq(IMyTerminalBlock w,string mU){if(w==null)return 0.0;
string b=mU.ToLowerInvariant();if(b=="percent"||b=="pct")return qs(w);if(b.IndexOf(':')>0)return pV(w,mU);var bL=w as IMyBatteryBlock;
if(bL!=null){if(b=="current"||b=="stored"||b=="val"||b=="value"||b=="raw")return bL.CurrentStoredPower;if(b=="max"||b=="capacity")return bL.MaxStoredPower;
if(b=="output")return bL.CurrentOutput;if(b=="maxoutput")return bL.MaxOutput;if(b=="input")return bL.CurrentInput;if(b=="maxinput")return bL.MaxInput;
return bL.CurrentStoredPower;}var jd=w as IMyJumpDrive;if(jd!=null){if(b=="max"||b=="capacity")return jd.MaxStoredPower;return jd.CurrentStoredPower;
}var gP=w as IMyGasTank;if(gP!=null){if(b=="max"||b=="capacity")return gP.Capacity;return gP.FilledRatio*gP.Capacity;}var pe=w as IMyAirVent;
if(pe!=null){if(b=="max"||b=="capacity")return 100.0;return pe.GetOxygenLevel()*100.0;}var mj=w as IMyCargoContainer;if(mj!=null){var f4=mj.GetInventory();
if(f4!=null){if(b=="mass")return(double)f4.CurrentMass;if(b=="max"||b=="capacity")return(double)f4.MaxVolume;return(double)f4.CurrentVolume;}return 0.0;}var j_=w as IMyPowerProducer;
if(j_!=null){if(b=="max"||b=="capacity")return j_.MaxOutput;return j_.CurrentOutput;}var mF=w as IMyDoor;if(mF!=null){if(b=="max")return 1.0;return(double)mF.OpenRatio*100.0;
}var kA=w as IMyThrust;if(kA!=null){if(b=="max"||b=="capacity")return kA.MaxEffectiveThrust;return kA.CurrentThrust;}var b0=w as IMyMotorSuspension;
if(b0!=null){if(b=="brake")return b0.Brake ? 100.0 : 0.0;if(b=="height")return b0.Height;if(b=="strength")return b0.Strength;if(b=="power")return b0.Power;
if(b=="steer")return b0.SteeringOverride*100.0;if(b=="propulsion")return b0.PropulsionOverride*100.0;return b0.Friction;}var mv=w as IMyShipConnector;
if(mv!=null){return mv.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var jX=w as IMyProductionBlock;if(jX!=null){if(b=="producing"||b=="active"||b=="running"||b=="state")return jX.IsProducing ? 100.0 : 0.0;
if(b=="queue")return jX.IsQueueEmpty ? 0.0 : 1.0;}var h0=w as IMyAssembler;if(h0!=null){if(b=="disassembling")return h0.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(b=="repeating"||b=="loop")return h0.Repeating ? 100.0 : 0.0;}var iT=w as IMyGasGenerator;if(iT!=null){if(b=="autorefill")return iT.AutoRefill ? 100.0 : 0.0;
if(b=="active")return iT.Enabled ? 100.0 : 0.0;}var gX=w as IMyLargeTurretBase;if(gX!=null){if(b=="shooting")return gX.IsShooting ? 100.0 : 0.0;
if(b=="target"||b=="hastarget")return gX.HasTarget ? 100.0 : 0.0;if(b=="range")return gX.Range;}var bU=w as IMyProjector;if(bU!=null){if(b=="built")return bU.TotalBlocks-bU.RemainingBlocks;
if(b=="remaining")return bU.RemainingBlocks;if(b=="total")return bU.TotalBlocks;if(b=="projecting")return bU.IsProjecting ? 100.0 : 0.0;if(b=="armor")return bU.RemainingArmorBlocks;
if(b=="buildable")return bU.BuildableBlocksCount;}var kD=w as IMyShipToolBase;if(kD!=null){if(b=="working"||b=="active"||b=="activated")return kD.IsActivated ? 100.0 : 0.0;
return kD.Enabled ? 100.0 : 0.0;}var c1=w as IMyGyro;if(c1!=null){if(b=="override")return c1.GyroOverride ? 100.0 : 0.0;if(b=="pitch")return c1.Pitch;
if(b=="yaw")return c1.Yaw;if(b=="roll")return c1.Roll;if(b=="power")return c1.GyroPower*100.0;}var ne=w as IMyGravityGeneratorBase;if(ne!=null){if(b=="g"||b=="accel")return ne.GravityAcceleration;
}var nc=w as IMyLandingGear;if(nc!=null){if(b=="locked")return nc.IsLocked ? 100.0 : 0.0;}var lQ=w as IMyRadioAntenna;if(lQ!=null){if(b=="radius"||b=="range")return lQ.Radius;
}var l2=w as IMyBeacon;if(l2!=null){if(b=="radius"||b=="range")return l2.Radius;}var jp=w as IMyLightingBlock;if(jp!=null){if(b=="intensity")return jp.Intensity;
if(b=="radius")return jp.Radius;}var gs=w as IMyPistonBase;if(gs!=null){if(b=="pos"||b=="position"||b=="extension")return gs.CurrentPosition;
if(b=="velocity")return gs.Velocity;if(b=="max")return gs.HighestPosition;}var eH=w as IMyMotorStator;if(eH!=null){if(b=="angle"||b=="deg")return eH.Angle*180.0/Math.PI;
if(b=="rad")return eH.Angle;if(b=="velocity"||b=="rpm")return eH.TargetVelocityRPM;if(b=="locked")return eH.RotorLock ? 100.0 : 0.0;}var kM=w as IMyWarhead;
if(kM!=null){if(b=="armed")return kM.IsArmed ? 100.0 : 0.0;if(b=="countdown")return kM.DetonationTime;}var oA=w as IMySensorBlock;if(oA!=null){
if(b=="detected")return!oA.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var im=w as IMyCameraBlock;if(im!=null){if(b=="range")return im.AvailableScanRange;
if(b=="active")return im.IsActive ? 100.0 : 0.0;}var mp=w as IMyParachute;if(mp!=null){if(b=="open"||b=="openratio")return(double)mp.OpenRatio*100.0;
}var dl=w as IMyTurretControlBlock;if(dl!=null){if(b=="target"||b=="hastarget")return dl.HasTarget ? 100.0 : 0.0;if(b=="azimuth")return dl.AzimuthRotor!=null ? dl.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(b=="elevation")return dl.ElevationRotor!=null ? dl.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var m3=w as IMyFunctionalBlock;if(m3!=null){return m3.Enabled ? 100.0 : 0.0;
}return 0.0;}double cf(string d4,string mV){bg bM;dR.TryGetValue(d4,out bM);if(bM!=null&&bM.bz==a_.dy){
string r4= !string.IsNullOrEmpty(bM.bh)? bM.bh : d4;double my;if(aG.TryGetValue(r4,out my))return my;return bM.e4;
}double mL;if(aG.TryGetValue(d4,out mL))return mL;List<IMyTerminalBlock>d5;if(!cn.TryGetValue(d4,out d5)||d5.Count==0)throw new InvalidOperationException($"HELM: bind '{d4}' resolved to zero blocks — check the name/typo");
if(d5.Count==1)return lq(d5[0],mV);var pc=new List<double>();foreach(var qT in d5)pc.Add(lq(qT,mV));return pq.pp(bM!=null ? bM.dp : aE.g3,pc);
}double hA(string q5){return cf(q5,"percent");}static string a2(double o8,bA c7){if(c7==null||(string.IsNullOrEmpty(c7.dO)&&string.IsNullOrEmpty(c7.dz)&&c7.I=="percent")){
return k9(o8);}return H.k8(o8,c7.dO,c7.dz);}bool qm(z bW){IMyTextSurface eL;if(!bG.TryGetValue(bW,out eL))return false;
if(bW.b2!=null)return qf(bW,eL);if(bW.hs!=null)return ql(bW,eL);if(bW.du.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return qn(bW,eL);}return qo(bW,eL);}Color la(string[]di,double sM){if(di.Length==1)return F(di[0]);
double oc=Math.Max(0.0,Math.Min(100.0,sM))/100.0*(di.Length-1);int i3=Math.Min((int)oc,di.Length-2);float gN=(float)(oc-i3);Color bJ=F(di[i3]),fw=F(di[i3+1]);
return new Color((int)(bJ.R+(fw.R-bJ.R)*gN),(int)(bJ.G+(fw.G-bJ.G)*gN),(int)(bJ.B+(fw.B-bJ.B)*gN),(int)(bJ.A+(fw.A-bJ.A)*gN));}static Color[]eU(aA ra,string[]tF,string rs,float pk,float pm,float kJ,float iZ){
int d2=16;bool nl=rs=="horizontal";var it=new Color[d2];for(int ct=0;ct<d2;ct++){n iQ=H.lb(tF,100.0*ct/(d2-1));it[ct]=new Color(iQ.bC,iQ.bx,iQ.bs);
float mg=kJ/d2,l9=iZ/d2;t(ra,nl ? new Vector2(pk+mg*(ct+0.5f),pm+iZ/2f): new Vector2(pk+kJ/2f,pm+l9*(ct+0.5f)),nl ? new Vector2(mg+1f,iZ): new Vector2(kJ,l9+1f),it[ct]);
}return it;}static void t(aA rb,Vector2 sX,Vector2 tu,Color rf,float te=0f,string rr="SquareSimple"){rb.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=rr,Position=sX,Size=tu,Color=rf,RotationOrScale=te}
);}void px(z k,IMyTextSurface ks,aA g){g.f.Clear();g.dK.Clear();Vector2 O=ks.SurfaceSize;
Vector2 tR=ks.TextureSize;Vector2 aw=(tR-O)/2f;g.ll=aw;float gk=Math.Max(4f,O.X*0.02f);
float cv=Math.Max(4f,O.Y*0.02f);if(k.ce=="compact"||(k.ce=="auto"&&O.Y<=260f&&O.Y>150f)){gk=4f;cv=4f;
}else if(k.ce=="corner"||(k.ce=="auto"&&O.Y<=150f)){gk=2f;cv=2f;}float eQ=O.X-(gk*2f);Color cT=new Color(10,12,16);
bool mH=true;if(!string.IsNullOrEmpty(k.bt)){var eg=n.dH(k.bt);if(eg.e9)mH=false;else cT=new Color(eg.bC,eg.bx,eg.bs,eg.g1);
}g.b5= -1;g.eW=false;g.eV=F(k.kR ?? "#5b0f0f");g.dt.Clear();if(k.bv!=null&&k.bv.Length>1){
g.b5=g.f.Count;g.eZ=eU(g,k.bv,k.ds,aw.X+2f,aw.Y+2f,O.X-4f,O.Y-4f);
}else if(mH||k.aM!=null||k.cJ!=null){if(k.aM!=null)cT=F(k.aM.First(s1=>s1.cL).b9);g.b5=g.f.Count;
g.eZ=new[]{cT};t(g,aw+(O/2f),O-new Vector2(4f,4f),cT);if(k.aM!=null||k.cJ!=null)g.dt.Add(new e_{
e8=g.b5,g4=cT,e3=cT,fg=k.aM,g8=k.g5,hh=k.g6,e6=k.cJ,hi=k.hj,hk=k.hl,lg=true}
);}if(!string.IsNullOrEmpty(k.g7))t(g,aw+(O/2f),O-new Vector2(4f,4f),F(k.kT ?? "255,255,255,24"),0f,k.g7);
if(!k.du.Equals("none",StringComparison.OrdinalIgnoreCase)){Color mb=k.ax!=null&&k.ax.Length>0 ? F(k.ax[0]): F(k.e2 ?? "40, 60, 85");
Color q9=k.ax!=null&&k.ax.Length>1 ? F(k.ax[k.ax.Length-1]): mb;
float mc=aw.X+(O.X-eQ)/2f;if(k.ax!=null&&k.ax.Length>1&& !k.hn){eU(g,k.ax,"horizontal",mc,aw.Y+cv*0.5f-1f,eQ,2f);
eU(g,k.ax,"horizontal",mc,aw.Y+O.Y-(cv*0.5f)-1f,eQ,2f);}else{t(g,new Vector2(aw.X+O.X/2f,aw.Y+cv*0.5f),new Vector2(eQ,2f),mb);
t(g,new Vector2(aw.X+O.X/2f,aw.Y+O.Y-(cv*0.5f)),new Vector2(eQ,2f),q9);}}var jM=k.ah;
if(jM.Count==0){jM=new List<az>{new az{dG="main",dw=0,dJ=0,dL=k.dA,dM=k.dB,at=k.fc}};}float ng=8f;
foreach(var s in jM){var gj=H.pA(s.dw,s.dJ,s.dL,s.dM,k.dA,k.dB,O.X,O.Y,gk,cv,ng,ng);
float aW=aw.X+gj.lD;float da=aw.Y+gj.lE;float a8=gj.lC;float ez=gj.ld;bool ok=s.aM!=null||s.cJ!=null;
if(s.bv!=null&&s.bv.Length>1){eU(g,s.bv,s.ds!="vertical" ? s.ds : k.ds,aW,da,a8,ez);
}else{Color bT;if(s.aM!=null)bT=F(s.aM.First(s2=>s2.cL).b9);else if(!string.IsNullOrEmpty(s.cH))bT=F(s.cH);
else if(!string.IsNullOrEmpty(k.hw))bT=F(k.hw);else bT=Color.Transparent;if(bT.A>0||ok){if(ok)g.dt.Add(new e_{e8=g.f.Count,g4=bT,e3=bT,fg=s.aM,g8=s.g5,hh=s.g6,e6=s.cJ,hi=s.hj,hk=s.hl}
);t(g,new Vector2(aW+a8/2f,da+ez/2f),new Vector2(a8,ez),bT);}}string fS=s.k5 ?? k.k6 ?? "Monospace";float q1=H.pQ(k.ce,O.X,O.Y,k.cK);
float aH=s.k7 ?? q1;float eh=0f;if(!string.IsNullOrEmpty(s.ci)){eh=20f*aH;var hU=TextAlignment.LEFT;float kB=aW+4f;
string lJ=s.hD ?? k.ly ?? "left";if(lJ.Equals("center",StringComparison.OrdinalIgnoreCase)){hU=TextAlignment.CENTER;kB=aW+(a8/2f);
}else if(lJ.Equals("right",StringComparison.OrdinalIgnoreCase)){hU=TextAlignment.RIGHT;kB=aW+a8-4f;}string oW=s.hE ?? k.lz;
Color tS= !string.IsNullOrEmpty(oW)? F(oW): F(s.hc ?? k.e2 ?? "white");var o0=MySprite.CreateText(s.ci,fS,tS,aH*0.9f,hU);
o0.Position=new Vector2(kB,da+2f);g.f.Add(o0);string[]h6=s.hd ?? k.ax;Color ru=h6!=null&&h6.Length>0 ? F(h6[0]): F(s.hc ?? k.e2 ?? "40, 60, 85");
t(g,new Vector2(aW+a8/2f,da+eh-2f),new Vector2(a8-8f,1f),ru);}if(s.hv=="vertical"||s.at.Any(sU=>sU.h==e.fi)){
py(g,ks,s,fS,aH,aW,da,a8,ez,eh);continue;}int sp=Math.Max(1,s.at.Count);float sK=ez-eh-8f;
float j3=sK/sp;float f2=6f;for(int f0=0;f0<s.at.Count;f0++){var E=s.at[f0];var U=new cc{fb=E};float tf=da+eh+4f+(f0*j3);
float M=tf+(j3/2f);float oY=M-(11f*aH);U.cM=g.f.Count;U.lc=E.W!=null&&(E.W.Contains("{val}")||E.W.Contains("{percent}"));
var nE=MySprite.CreateText(E.W,fS,Color.White,aH,TextAlignment.LEFT);nE.Position=new Vector2(aW+f2,oY);g.f.Add(nE);
float si=Math.Max(24f*aH,(E.W.Length+1)*12f*aH);float t2=(!string.IsNullOrEmpty(E.dO)|| !string.IsNullOrEmpty(E.dz))? 64f*aH : 52f*aH;
if(E.h!=e.hC){float L=aW+f2+si;float q0=aW+a8-f2-t2;float J=Math.Max(16f,q0-L);
float T=Math.Max(E.h==e.cg ? 8f : 4f,Math.Min((E.h==e.cg ? 20f : 16f)*aH,j3*(E.h==e.cg ? 0.55f : 0.45f)));
U.b4=L;U.bf=J;U.aZ=T;U.b3=M;if(E.h==e.dr){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(25,30,38));
U.V=g.f.Count;t(g,new Vector2(L,M),new Vector2(0f,T),F(E.ar));}else if(E.h==e.dF){
int kd=10;float m5=2f;float ke=Math.Max(2f,(J-(m5*(kd-1)))/kd);for(int j7=0;j7<kd;j7++){U.K.Add(g.f.Count);
t(g,new Vector2(L+j7*(ke+m5)+(ke/2f),M),new Vector2(ke,T),new Color(22,26,32));}}else if(E.h==e.cg){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(20,24,30));
int jW=8;float m6=2f;float iq=Math.Max(2f,(J-(m6*(jW-1)))/jW);for(int jI=0;jI<jW;jI++){U.K.Add(g.f.Count);
t(g,new Vector2(L+jI*(iq+m6)+(iq/2f),M),new Vector2(iq,2f),F(E.ar));}}else if(E.h==e.dC){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(20,24,30));
int ko=10;float m7=2f;float kp=Math.Max(2f,(J-(m7*(ko-1)))/ko);for(int j8=0;j8<ko;j8++){U.K.Add(g.f.Count);
t(g,new Vector2(L+j8*(kp+m7)+(kp/2f),M),new Vector2(kp,T),new Color(24,28,36),0.523599f);}}else if(E.h==e.dv){
int hZ=10;float m8=2f;float h_=Math.Max(2f,(J-(m8*(hZ-1)))/hZ);for(int j9=0;j9<hZ;j9++){U.K.Add(g.f.Count);
t(g,new Vector2(L+j9*(h_+m8)+(h_/2f),M),new Vector2(h_,T),new Color(24,28,36),1.570796f,"Triangle");}}else if(E.h==e.dx){
int ix=12;float m9=2f;float iz=Math.Max(2f,(J-(m9*(ix-1)))/ix);float mG=Math.Min(iz,T);for(int ka=0;ka<ix;ka++){U.K.Add(g.f.Count);
t(g,new Vector2(L+ka*(iz+m9)+(iz/2f),M),new Vector2(mG,mG),new Color(24,28,36),0f,"Circle");}}else if(E.h==e.cI){
t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(25,30,38));U.V=g.f.Count;t(g,new Vector2(L+(J/2f),M),new Vector2(0f,T),F(E.ar));
t(g,new Vector2(L+(J/2f),M),new Vector2(2f,T+4f),new Color(80,110,150));}else if(E.h==e.cj){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(20,24,30));
U.V=g.f.Count;float jj=Math.Max(4f,T);t(g,new Vector2(L+jj*0.5f,M),new Vector2(jj,jj),new Color(40,50,65),0f,"Circle");
}else if(E.h==e.b8){float cU=Math.Max(6f,Math.Min(T,14f*aH));t(g,new Vector2(L+cU*0.5f,M),new Vector2(cU,cU),new Color(28,34,46));
U.V=g.f.Count;t(g,new Vector2(L+cU*0.5f,M),new Vector2(cU*0.6f,cU*0.6f),new Color(40,50,65));}else if(E.h==e.ch){
t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(18,22,30));U.V=g.f.Count;float jT=Math.Max(4f,T*0.7f);
t(g,new Vector2(L+jT+2f,M),new Vector2(jT,jT),new Color(34,197,94),0f,"Circle");}else if(E.h==e.cG){int jS=8;
float na=2f;float jU=Math.Max(2f,(J-(na*(jS-1)))/jS);for(int jJ=0;jJ<jS;jJ++){U.K.Add(g.f.Count);t(g,new Vector2(L+jJ*(jU+na)+(jU/2f),M),new Vector2(jU,T),new Color(24,28,36));
}}else if(E.h==e.ca){t(g,new Vector2(L+(J/2f),M),new Vector2(J,T),new Color(20,24,30));U.V=g.f.Count;
float no=Math.Max(4f,T);t(g,new Vector2(L+(J/2f),M),new Vector2(no,no),F(E.ar),0f,"Triangle");}}var o6=MySprite.CreateText("---",fS,Color.White,aH,TextAlignment.RIGHT);
o6.Position=new Vector2(aW+a8-f2,oY);U.aS=g.f.Count;g.f.Add(o6);g.dK.Add(U);}}g.dE=O;
g.dD=true;}const string qA="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bE=8;void py(aA Z,IMyTextSurface tI,az jL,string m1,float ee,float sH,float jK,float sI,float nZ,float rW){
int sw=Math.Max(1,jL.at.Count);float ms=(sI-12f)/sw;float gY=22f*ee;float bd=jK+rW+4f+gY;float aK=Math.Max(20f,jK+nZ-4f-gY-bd);
float ro=tI.MeasureStringInPixels(new StringBuilder("M"),m1,ee*0.9f).X;for(int f1=0;f1<jL.at.Count;f1++){var ge=jL.at[f1];float a7=sH+6f+ms*(f1+0.5f);
float cE=ms*0.7f;var ba=new cc{fb=ge,lA=true,b4=a7,bf=cE,aZ=aK,b3=bd+aK/2f,k_=ro};ba.cM=Z.f.Count;
aY(Z.f,ge.W,Color.White,a7,jK+nZ-4f-gY,ee);ba.aS=Z.f.Count;aY(Z.f,"---",Color.White,a7,bd-gY,ee);
var nB=Z.f[ba.cM];nB.Alignment=TextAlignment.CENTER;Z.f[ba.cM]=nB;var o5=Z.f[ba.aS];o5.Alignment=TextAlignment.CENTER;
Z.f[ba.aS]=o5;t(Z,new Vector2(a7,bd+aK/2f),new Vector2(cE,aK),new Color(25,30,38));ba.V=Z.f.Count;if(ge.h==e.fi){
for(int lY=0;lY<bE;lY++){ba.K.Add(Z.f.Count);t(Z,new Vector2(a7,bd+aK),new Vector2(cE-6f,0f),Color.White);}for(int kK=0;kK<4;
kK++){ba.K.Add(Z.f.Count);var oR=MySprite.CreateText("",m1,kK==0 ? new Color(255,255,255,170): new Color(255,255,255,110),ee*0.9f,TextAlignment.CENTER);
oR.Position=new Vector2(a7,bd+aK);Z.f.Add(oR);}Color iB=new Color(90,110,135);t(Z,new Vector2(a7-cE/2f,bd+aK/2f),new Vector2(3f,aK),iB);t(Z,new Vector2(a7+cE/2f,bd+aK/2f),new Vector2(3f,aK),iB);
t(Z,new Vector2(a7,bd+aK),new Vector2(cE+3f,3f),iB);}else{t(Z,new Vector2(a7,bd+aK),new Vector2(cE,0f),F(ge.ar));}Z.dK.Add(ba);
}}bool qz(aA a5,cc y,double eS,double od,Color fG,bool pa){var bl=y.fb;float l4=y.D;if(y.D<0f)y.D=(float)eS;
y.D=Math.Abs(y.D-(float)eS)>0.05f ? y.D+((float)eS-y.D)*0.35f :(float)eS;float cu=Math.Max(0f,Math.Min(1f,y.D/100f))*y.aZ;
float fD=y.b3+y.aZ/2f;float fI=y.b4;if(bl.h!=e.fi){var fN=a5.f[y.V];fN.Size=new Vector2(y.bf,cu);
fN.Position=new Vector2(fI,fD-cu/2f);fN.Color=fG;a5.f[y.V]=fN;if(pa||y.D!=l4)aF(a5,y,a2(od,bl),fG);
return pa||y.D!=l4;}float h8=y.aZ/bE;Color sr=bl.bw!=null ? F(H.dI(bl.bw,eS)): fG;
for(int cp=0;cp<bE;cp++){float pf=Math.Max(0f,Math.Min(h8,cu-cp*h8));var gt=a5.f[y.K[cp]];gt.Size=new Vector2(y.bf-6f,pf);
gt.Position=new Vector2(fI,fD-cp*h8-pf/2f);gt.Color=bl.bw==null&&bl.cb&&bl.a3.Length>1 ? la(bl.a3,100.0*(cp+0.5)/bE): sr;
a5.f[y.K[cp]]=gt;}int rd=Math.Max(1,(int)((y.bf-8f)/y.k_));bool tr=cu>6f;var kN=a5.f[y.K[bE]];
kN.Data=tr ? qA.Substring((int)(ae*5.0)%6,Math.Min(rd,100)): "";kN.Position=new Vector2(fI,fD-cu-12f);a5.f[y.K[bE]]=kN;
for(int bR=0;bR<3;bR++){var ih=a5.f[y.K[bE+1+bR]];double sQ=(ae*(0.35+0.1*bR)+bR*0.37)%1.0;ih.Data=cu>24f ?(bR==1 ? "O" : "o"): "";
ih.Position=new Vector2(fI+(bR-1)*y.bf*0.26f,fD-(float)sQ*(cu-6f)-10f);a5.f[y.K[bE+1+bR]]=ih;}aF(a5,y,a2(od,bl),fG);
return true;}static void aF(aA me,cc oE,string tQ,Color rg){var kH=me.f[oE.aS];kH.Data=tQ;kH.Color=rg;me.f[oE.aS]=kH;
}static void ls(aA mf,cc oF,Color rh){var mP=mf.f[oF.V];mP.Color=rh;mf.f[oF.V]=mP;}bool qo(z br,IMyTextSurface kt){
aA i;if(!bH.TryGetValue(br,out i)){i=new aA();bH[br]=i;}bool og= !i.dD||i.dE!=kt.SurfaceSize;
if(og){px(br,kt,i);}bool N=og;bool fs=(ae%br.g2)<br.g2/2.0;double cs= -1.0,mw;if(fl.TryGetValue(br,out mw)){
cs=(ae-mw)/br.ha;if(cs>=1.0){fl.Remove(br);cs=1.0;}}bool kL=cl&&fs&&i.eV.A>0;if(i.b5>=0&&kL!=i.eW){
i.eW=kL;for(int ej=0;ej<i.eZ.Length;ej++){var l5=i.f[i.b5+ej];l5.Color=kL ? i.eV : i.eZ[ej];i.f[i.b5+ej]=l5;
}N=true;foreach(var q2 in i.dt)q2.hm=true;}foreach(var ai in i.dt){if(ai.lg&&i.eW)continue;Color d3=ai.g4;try{if(ai.fg!=null)d3=F(H.dI(ai.fg,cf(ai.g8,ai.hh)));
if(ai.e6!=null&&fs&&H.dI(ai.e6,cf(ai.hi,ai.hk))!=null)d3=i.eV;}catch{}if(d3!=ai.e3||ai.hm){
ai.e3=d3;ai.hm=false;var md=i.f[ai.e8];md.Color=d3;i.f[ai.e8]=md;N=true;}}for(int i4=0;i4<i.dK.Count;i4++){var c=i.dK[i4];
var j=c.fb;double x;bool ni=false;double jw=j.fd;try{x=cf(j.aN,j.I);if(j.cN!=null)jw=cf(j.cN,j.hr);
}catch{ni=true;x=0.0;}if(ni){if(c.fa!= -99999.0||(c.aS>=0&&i.f[c.aS].Data!="ERR")){N=true;c.fa= -99999.0;
if(c.aS>=0){var kb=i.f[c.aS];kb.Data="ERR";kb.Color=Color.Red;i.f[c.aS]=kb;}}continue;}double aX=x;
if(jw>0)x=Math.Min(100.0,aX/jw*100.0);bool oO=cs>=0.0&&j.h!=e.hC&&j.h!=e.cj&&j.h!=e.b8&&j.h!=e.ch&&j.h!=e.ca;
if(oO)x=cs<0.5 ? cs*200.0 : 100.0+(x-100.0)*(cs-0.5)*2.0;string rj=j.cb ? H.e7(j.a3,x): j.ar;
Color _=F(rj);bool bk=(_==Color.Red);if(j.cb&& !br.hn&&j.a3.Length>1)_=la(j.a3,x);
if(bk&&fs){_=new Color(255,75,75);N=true;}bool aL=oO||Math.Abs(aX-c.fa)>0.1;if(aL)c.fa=aX;
if(c.lA){if(qz(i,c,x,aX,_,aL))N=true;continue;}if(j.h==e.dr){double mW=(j.hp||j.I=="percent"||j.I=="pct")? x : hA(j.aN);
if(c.D<0f)c.D=(float)mW;float gQ=(float)mW;if(Math.Abs(c.D-gQ)>0.05f){c.D+=(gQ-c.D)*0.35f;
N=true;}else if(c.D!=gQ){c.D=gQ;N=true;}if(aL)N=true;if(N){float sN=(float)Math.Max(0.0,Math.Min(1.0,c.D/100.0));
float mY=c.bf*sN;var fP=i.f[c.V];fP.Size=new Vector2(mY,c.aZ);fP.Position=new Vector2(c.b4+(mY/2f),c.b3);
fP.Color=_;i.f[c.V]=fP;aF(i,c,a2(aX,j),_);}}else if(j.h==e.dF||j.h==e.dC||j.h==e.dv||j.h==e.dx){
if(aL||bk){N=true;double rD=(j.hp||j.I=="percent"||j.I=="pct")? x : hA(j.aN);int oy=c.K.Count;
for(int eJ=0;eJ<oy;eJ++){bool ss=rD>=((eJ+0.4)/(double)oy*100.0);Color tp=ss ? _ : new Color(24,28,36);var oz=i.f[c.K[eJ]];
oz.Color=tp;i.f[c.K[eJ]]=oz;}aF(i,c,a2(aX,j),_);}}else if(j.h==e.cg){
if(aL||bk){N=true;for(int fW=0;fW<7;fW++)c.fh[fW]=c.fh[fW+1];c.fh[7]=(float)x;int sW=c.K.Count;
for(int ev=0;ev<sW;ev++){float rV=c.fh[ev];float rU=Math.Max(0.06f,Math.Min(1f,rV/100f));float mr=c.aZ*rU;float ri=c.b3+(c.aZ/2f)-(mr/2f);
var cZ=i.f[c.K[ev]];cZ.Size=new Vector2(cZ.Size.Value.X,mr);cZ.Position=new Vector2(cZ.Position.Value.X,ri);
cZ.Color=_;i.f[c.K[ev]]=cZ;}aF(i,c,a2(aX,j),_);}}else if(j.h==e.cI){
if(c.D<0f)c.D=(float)x;float gR=(float)x;if(Math.Abs(c.D-gR)>0.05f){c.D+=(gR-c.D)*0.35f;
N=true;}else if(c.D!=gR){c.D=gR;N=true;}if(N||aL){float nh=c.bf*0.5f;float mm=c.b4+nh;
float mC=(float)Math.Max(-1.0,Math.Min(1.0,(c.D-50.0)/50.0));float iO=Math.Abs(mC)*nh;float rE=(mC>=0)?(mm+iO*0.5f):(mm-iO*0.5f);
var fQ=i.f[c.V];fQ.Size=new Vector2(iO,c.aZ);fQ.Position=new Vector2(rE,c.b3);fQ.Color=_;
i.f[c.V]=fQ;aF(i,c,a2(aX,j),_);}}else if(j.h==e.cj){if(aL||bk){
N=true;bool jc=x>=50.0;float nx=Math.Max(4f,c.aZ);float r7=jc ?(c.b4+c.bf-nx*0.5f):(c.b4+nx*0.5f);
Color nw=jc ?(_!=Color.White ? _ : new Color(34,197,94)): new Color(60,70,85);var iM=i.f[c.V];iM.Position=new Vector2(r7,c.b3);
iM.Color=nw;i.f[c.V]=iM;aF(i,c,jc ? "ON" : "OFF",nw);}}else if(j.h==e.b8){if(aL||bk){
N=true;bool jb=x>=50.0;Color mn=jb ?(_!=Color.White ? _ : new Color(56,189,248)): new Color(30,36,48);ls(i,c,mn);
aF(i,c,jb ? "ACTIVE" : "READY",jb ? mn : new Color(148,163,184));}}else if(j.h==e.ch){if(aL||bk){
N=true;string tD=x>=75.0 ? "ONLINE" :(x>=25.0 ? "STBY" : "OFFLINE");Color oJ=x>=75.0 ? new Color(34,197,94):(x>=25.0 ? new Color(234,179,8): new Color(239,68,68));
ls(i,c,oJ);aF(i,c,tD,oJ);}}else if(j.h==e.cG){if(aL||bk){N=true;int n7=c.K.Count;
for(int ew=0;ew<n7;ew++){bool st=x>=((ew+0.5)/(double)n7*100.0);Color sS=st ? _ : new Color(24,28,36);var n8=i.f[c.K[ew]];
n8.Color=sS;i.f[c.K[ew]]=n8;}aF(i,c,a2(aX,j),_);}}else if(j.h==e.ca){
N=true;float gy;Color ed;string fR;bool r_=(j.I=="producing"||j.I=="active"||j.I=="running"||j.I=="state"||(j.I!="percent"&&(x<=1.0||x==100.0)));
if(r_){if(x>0.0){c.D=(c.D+0.3927f)%6.28318f;gy=c.D;ed=new Color(34,197,94);fR="RUN";}else{gy=0f;
ed=new Color(148,163,184);fR="IDLE";}}else{gy=(x>55.0)? 0f :((x<45.0)? 3.14159f : 1.57079f);ed=(x>55.0)? new Color(34,197,94):((x<45.0)? new Color(239,68,68): new Color(148,163,184));
fR=(x>55.0)? "CHG" :((x<45.0)? "DSC" : "STBY");}var iN=i.f[c.V];iN.RotationOrScale=gy;iN.Color=ed;
i.f[c.V]=iN;aF(i,c,fR,ed);}else{string fM;bg gT;dR.TryGetValue(j.aN,out gT);dT.TryGetValue(lh(j.I)? j.aN+"."+j.I :(gT!=null&&gT.bh!=null ? gT.bh : j.aN),out fM);
if(aL||bk||fM!=c.li){c.li=fM;N=true;Color kE=j.bw!=null ? F(H.dI(j.bw,x)): Color.White;
if(kE==Color.Red&&fs)kE=new Color(255,75,75);aF(i,c,fM ?? a2(aX,j),kE);}}if(c.lc&&(aL||bk)){
N=true;var nG=i.f[c.cM];string sO=k9(x);string tc=a2(aX,j);nG.Data=j.W.Replace("{percent}",sO).Replace("{val}",tc);
i.f[c.cM]=nG;}}if(N){i.dP++;using(var rK=kt.DrawFrame()){rK.AddRange(i.f);}}return N;}bool qn(z bX,IMyTextSurface ku){
ku.FontSize=bX.cK;var fH=new List<KeyValuePair<string,string>>();foreach(var m in bX.fc){double P=0.0;string bc;string a6=null;
double jx=m.fd;try{P=cf(m.aN,m.I);if(m.cN!=null)jx=cf(m.cN,m.hr);}catch(Exception ud){
fH.Add(new KeyValuePair<string,string>($"{m.W}: [UNRESOLVED]","red"));continue;}double oe=P;if(jx>0)P=Math.Min(100.0,oe/jx*100.0);string o7=a2(oe,m);
if(m.h==e.dr){double mX=(m.hp||m.I=="percent"||m.I=="pct")? P : hA(m.aN);bc=H.qg(m.W,mX,10,o7);
a6=m.cb ? H.e7(m.a3,mX): m.ar;}else if(m.h==e.cI){bc=H.qe(m.W,P,10);
a6=m.cb ? H.e7(m.a3,P): m.ar;}else if(m.h==e.cj){bc=H.qq(m.W,P);
a6=m.ar ??(P>=50.0 ? "green" : "gray");}else if(m.h==e.b8){bc=H.qj(m.W,P);a6=m.ar ??(P>=50.0 ? "cyan" : "gray");
}else if(m.h==e.ch){bc=H.qp(m.W,P);a6=m.ar ??(P>=75.0 ? "lime" :(P>=25.0 ? "yellow" : "red"));
}else if(m.h==e.cG){bc=H.qd(m.W,P,8);a6=m.cb ? H.e7(m.a3,P):(m.ar ?? "yellow");
}else if(m.h==e.ca){bool r0=(m.I=="producing"||m.I=="active"||m.I=="running"||m.I=="state"||(m.I!="percent"&&(P<=1.0||P==100.0)));
if(r0){char[]tB=new[]{'|','/','-','\\'};char tm=tB[Math.Abs(dY)%4];bc=P>0 ? $"{m.W} [{tm}] RUN" : $"{m.W} [==] IDLE";
a6=P>0 ? "green" : "gray";}else{bc=H.qk(m.W,P,dY);a6=m.ar ??(P>55.0 ? "green" :(P<45.0 ? "red" : "cyan"));
}}else{bc=$"{m.W}: {o7}";a6=m.bw!=null ? H.dI(m.bw,P): null;}fH.Add(new KeyValuePair<string,string>(bc,a6));
}var fC=H.qi(new List<string>(fH.ConvertAll(tL=>tL.Key)),dn: bX.lB);string oB=string.Join("\n",fC);
string oa;if(cm.TryGetValue(bX,out oa)&&oa==oB)return false;cm[bX]=oB;using(var rL=ku.DrawFrame()){
float pn=4f;float nI=po*bX.cK;try{var nK=ku.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bX.cK);
if(nK.Y>0)nI=nK.Y;}catch{}for(int c2=0;c2<fC.Count;c2++){string mu=null;if(c2>0&&c2<fC.Count-1)mu=fH[c2-1].Value;
var oI=MySprite.CreateText(fC[c2],"Monospace",F(mu),bX.cK,TextAlignment.LEFT);oI.Position=new Vector2(4f,pn);rL.Add(oI);
pn+=nI;}}return true;}Color F(string gh){if(string.IsNullOrEmpty(gh))return Color.White;Color eq;if(lF.TryGetValue(gh,out eq))return eq;
n d6=n.dH(gh);eq=d6.e9 ? Color.Transparent : new Color(d6.bC,d6.bx,d6.bs,d6.g1);lF[gh]=eq;return eq;}public enum a_{ps,ho,dy}
public enum aE{pZ,g3,lw,lk,lj}public class bg{public string ab;public a_ bz;public List<string>aR=new List<string>();public string lx{
get{return aR.Count>0 ? aR[0]: "";}set{if(aR.Count==0)aR.Add(value);else aR[0]=value;}}public aE dp=aE.pZ;
public double e4=0.0;public string he;public string bh;public bool eY;}public enum e{hC,dr,dF,cg,dC,dv,dx,cI,cj,b8,ch,cG,ca,fi}
public class aO{public string hg;public string b9;public bool cL;}public class bA{public e h;public string W;public string aN;
public string I;public bool cb;public string[]a3;public string ar;public List<aO>bw;public string k0="right";
public string dO;public string dz;public double fd;public string cN,hr;public bool hp{get{return fd>0||cN!=null;}}}public struct n{
public byte bC,bx,bs,g1;public bool e9;public n(byte s3,byte rN,byte qU,byte qC=255){bC=s3;bx=rN;bs=qU;g1=qC;e9=false;}public static readonly n qy=new n(0,0,0,0){
e9=true};public static readonly n fj=new n(255,255,255);public static readonly n pr=new n(0,0,0);public static readonly n qb=new n(255,0,0);
public static readonly n pS=new n(0,255,0);public static readonly n pW=new n(50,205,50);public static readonly n qB=new n(255,255,0);
public static readonly n p_=new n(255,140,0);public static readonly n pD=new n(0,255,255);public static readonly n pt=new n(0,120,255);
public static readonly n pX=new n(255,0,255);public static readonly n p9=new n(128,0,128);public static readonly n pR=new n(128,128,128);
public static readonly n pE=new n(60,60,60);public static n dH(string bb){if(string.IsNullOrWhiteSpace(bb))return fj;bb=bb.Trim();
if(bb.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bb.Equals("none",StringComparison.OrdinalIgnoreCase))return qy;if(bb.StartsWith("#")){
string bP=bb.Substring(1).Trim();if(bP.Length==6){byte s4=Convert.ToByte(bP.Substring(0,2),16);byte rO=Convert.ToByte(bP.Substring(2,2),16);byte qV=Convert.ToByte(bP.Substring(4,2),16);
return new n(s4,rO,qV);}if(bP.Length==3){byte s5=Convert.ToByte(new string(bP[0],2),16);byte rP=Convert.ToByte(new string(bP[1],2),16);byte qW=Convert.ToByte(new string(bP[2],2),16);
return new n(s5,rP,qW);}}if(bb.Contains(",")){var db=bb.Split(',');if(db.Length>=3){byte s6=(byte)Math.Max(0,Math.Min(255,int.Parse(db[0].Trim())));
byte rQ=(byte)Math.Max(0,Math.Min(255,int.Parse(db[1].Trim())));byte qX=(byte)Math.Max(0,Math.Min(255,int.Parse(db[2].Trim())));byte qD=db.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(db[3].Trim()))):(byte)255;
return new n(s6,rQ,qX,qD);}}switch(bb.ToLowerInvariant()){case "red": return qb;case "green": return pS;case "lime": return pW;case "yellow": return qB;
case "orange": return p_;case "cyan": return pD;case "blue": return pt;case "magenta": return pX;case "purple": return p9;case "white": return fj;
case "black": return pr;case "gray": case "grey": return pR;case "darkgray": case "darkgrey": return pE;default: return fj;}}}public class bu{
public string[]bv;public string ds="vertical";public List<aO>aM;public string g5,g6;public List<aO>cJ;public string hj,hl;
}public class az : bu{public string dG="default";public int dw=0;public int dJ=0;public int dL=1;public int dM=1;public string ci;public string hD="left";
public string hE;public string kY;public string k5;public float? k7;public string cH;public string hc;public string[]hd;
public string hv="horizontal";public List<bA>at=new List<bA>();}public class z : bu{public string a4;public string ce="standard";
public string du="graphic";public string e2;public string[]ax;public string ly="left";public string lz;public string k6="Monospace";
public string bt;public string kW;public int kX=10;public int kU=1;public float ha=1.2f;public bool hn;
public string hw;public string g7;public string kT;public string kR;public string hs;public string b2;public string b7;
public float hb=2.5f;public string kV="cyan";public string[]e0;public List<string>e1=new List<string>();public List<string>pP(){
if(e1!=null&&e1.Count>0)return e1;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float cK=1.0f;public int hy=0;public float g2=1.6f;public int dA=1;public int dB=1;public int lB=30;public string bB;
public int cd=0;public List<az>ah=new List<az>();public List<bA>fc{get{if(ah.Count==0)ah.Add(new az{dG="main"}
);return ah[0].at;}}}public class bF{public string ht;public List<string>fe=new List<string>();public Dictionary<string,string>hf=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>g9=new List<string>();}public class hx{public List<bg>b6=new List<bg>();public List<z>S=new List<z>();
public List<string>a1=new List<string>();public List<string>dQ=new List<string>();public Dictionary<string,List<string>>dq=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class le{static readonly Dictionary<string,bF>kZ=new Dictionary<string,bF>(StringComparer.OrdinalIgnoreCase);
static le(){qc();}static void qc(){bD("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bD(string nR,string[]sL,string q6){var gO=new bF{ht=nR}
;foreach(var sE in sL){var jk=sE.Split(new[]{'='},2);string n_=hu(jk[0]);gO.fe.Add(n_);if(jk.Length>1)gO.hf[n_]=ad(jk[1].Trim());
}var sq=q6.Replace("\r\n","\n").Split('\n');foreach(var sd in sq){string om=sd.Trim();if(om.Length>0)gO.g9.Add(om);}kZ[nR]=gO;}static string hu(string c8){
c8=c8.Trim();if(c8.StartsWith("$"))c8=c8.Substring(1);return c8.ToLowerInvariant();}public static hx p1(string rm){var ao=new hx();z d=null;
az r=null;var n9=p7(rm,ao.a1,ao.dq);for(int ek=0;ek<n9.Count;ek++){string a=n9[ek].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){ao.b6.Add(p2(a));}else if(a.StartsWith("@screen")){d=new z{a4=a.Substring("@screen".Length).Trim()}
;ao.S.Add(d);r=null;}else if(a.StartsWith("profile:")){p(d,"profile:");d.ce=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){p(d,"border:");string lZ=a.Substring("border:".Length).Trim();if(r!=null)r.kY=lZ;
else d.du=lZ;}else if(a.StartsWith("border_color:")){p(d,"border_color:");string l0=a.Substring("border_color:".Length).Trim();
if(r!=null)r.hc=l0;else d.e2=l0;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
p(d,"border_gradient");string cq=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
cq=cq.Trim();if(cq.StartsWith("(")&&cq.Contains(")"))cq=aP(cq,0);string[]oK=aQ(cq).Select(tg=>tg.Trim()).ToArray();if(r!=null)r.hd=oK;
else d.ax=oK;}else if(a.StartsWith("title_align:")){p(d,"title_align:");string oS=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(r!=null)r.hD=oS;else d.ly=oS;}else if(a.StartsWith("title_color:")){p(d,"title_color:");string oV=a.Substring("title_color:".Length).Trim();
if(r!=null)r.hE=oV;else d.lz=oV;}else if(a.StartsWith("font_size:")){p(d,"font_size:");float oP=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(r!=null)r.k7=oP;else d.cK=oP;}else if(a.StartsWith("font:")){p(d,"font:");string mQ=a.Substring("font:".Length).Trim();
if(r!=null)r.k5=mQ;else d.k6=mQ;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){p(d,"bg:");string cS=a.Substring(a.IndexOf(':')+1).Trim();
bu h7=r!=null ?(bu)r : d;if(cS.StartsWith("color(")&&cS.EndsWith(")")){string l8;h7.aM=lo(cS.Substring(6,cS.Length-7),out l8);
string l6,l7;lt(l8,out l6,out l7);h7.g5=l6;h7.g6=l7;}else if(r!=null)r.cH=cS;
else d.bt=cS;}else if(a.StartsWith("layout:")){p(d,"layout:");p3(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
p(d,"alert_period:");d.g2=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){p(d,"refresh:");d.hy=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
p(d,"output:");var gn=aQ(a.Substring("output:".Length).Trim());if(gn.Count==0)throw new FormatException("output: requires a target block name");
d.bB=ad(gn[0]);d.cd=gn.Count>1 ? int.Parse(gn[1]): 0;}else if(a.StartsWith("width:")){p(d,"width:");
d.lB=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){p(d,"boot_msg:");
d.kW=ad(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){p(d,"boot_stagger:");d.kX=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){p(d,"boot_calibrate:");string mx=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.ha=mx=="off" ? 0f : Math.Max(0f,float.Parse(mx,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){p(d,"boot_batch:");
d.kU=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){p(d,"boot_duration:");
string d8=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool nn=d8.EndsWith("s");if(nn)d8=d8.Substring(0,d8.Length-1).Trim();
float iA;if(float.TryParse(d8,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out iA)){d.hb=nn ? iA : iA/60f;
}}else if(a.StartsWith("boot_log:")){p(d,"boot_log:");d.e1.Add(ad(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("mirror:")){
p(d,"mirror:");d.hs=ad(a.Substring("mirror:".Length).Trim());}else if(a.StartsWith("banner:")){p(d,"banner:");
d.b2=ad(a.Substring("banner:".Length).Trim());}else if(a.StartsWith("boot_logo:")){p(d,"boot_logo:");d.b7=ad(a.Substring("boot_logo:".Length).Trim());
}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){p(d,"boot_gradient");
d.e0=lp(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){p(d,"bg_gradient");(r!=null ?(bu)r : d).bv=lp(a);
}else if(a.StartsWith("bg_gradient_dir:")){p(d,"bg_gradient_dir:");(r!=null ?(bu)r : d).ds=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("flash:")){p(d,"flash:");bu iJ=r!=null ?(bu)r : d;string iI=a.Substring("flash:".Length).Trim();
string mS=lv(iI);string mR,mT;lt(iI.Substring(0,iI.Length-mS.Length),out mR,out mT);iJ.hj=mR;iJ.hl=mT;
iJ.cJ=new List<aO>{new aO{hg=mS,b9="on"}};}else if(a.StartsWith("bg_texture:")){p(d,"bg_texture:");d.g7=ad(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){p(d,"bg_texture_color:");d.kT=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gradient_mode:")){p(d,"gradient_mode:");d.hn=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){p(d,"screen_bg:");d.bt=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
p(d,"panel_bg:");d.hw=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){p(d,"alert_bg:");
d.kR=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){p(d,"boot_color:");
d.kV=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){p(d,"orientation:");string sA=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(r!=null)r.hv=sA;}else if(a.StartsWith("@panel")){p(d,"@panel");r=p5(a);d.ah.Add(r);
}else if(a.StartsWith("title:")){p(d,"title:");if(r==null){r=new az{dG="main",dw=0,dJ=0,dL=d.dA,dM=d.dB}
;d.ah.Add(r);}r.ci=ad(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){p(d,"line:");
if(r==null){r=new az{dG="main",dw=0,dJ=0,dL=d.dA,dM=d.dB};d.ah.Add(r);}r.at.Add(p4(a.Substring("line:".Length).Trim()));
}else{ao.a1.Add($"Line {ek + 1}: unrecognized directive: \"{a}\"");}}catch(Exception rx){ao.a1.Add($"Line {ek + 1}: parse error ({rx.Message}) in \"{a}\"");
}}foreach(var oo in ao.S){if(string.IsNullOrEmpty(oo.bB))ao.a1.Add($"@screen {oo.a4}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aq in ao.S){int jG=aq.ah.Count(jV=>jV.cH!=null||jV.aM!=null||jV.bv!=null);if(aq.ah.Count>1&&jG>0&&jG<aq.ah.Count)ao.dQ.Add("screen '"+aq.a4+"': only "+jG+" of "+aq.ah.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(aq.b2!=null&& !ao.dq.ContainsKey(aq.b2))ao.a1.Add("Screen '"+aq.a4+"': banner '"+aq.b2+"' is not defined (@banner "+aq.b2+" ... @end)");
if(aq.b7!=null&& !ao.dq.ContainsKey(aq.b7))ao.a1.Add("Screen '"+aq.a4+"': boot_logo '"+aq.b7+"' is not defined (@banner "+aq.b7+" ... @end)");
}return ao;}static void p3(string id,z ow){int jN=id.IndexOf('(');if(jN<0)throw new FormatException("missing '(' in layout: specification");
string nv=id.Substring(0,jN).Trim().ToLowerInvariant();string qK=aP(id,jN);var hY=aQ(qK);if(nv=="grid"){if(hY.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
ow.dA=Math.Max(1,int.Parse(hY[0]));ow.dB=Math.Max(1,int.Parse(hY[1]));}else{throw new FormatException($"unknown layout kind '{nv}' (expected grid)");
}}static az p5(string sl){string eE=sl.Substring("@panel".Length).Trim();int fv=eE.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(fv<0)throw new FormatException("@panel requires an at(...) positioning clause");
string sx=eE.Substring(0,fv).Trim();string lU=eE.Substring(fv);int n5=lU.IndexOf('(');string lS=aP(lU,n5);
var co=aQ(lS);if(co.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bo=new az{dG=sx,dw=int.Parse(co[0]),dJ=int.Parse(co[1]),dL=co.Count>2 ? int.Parse(co[2]): 1,dM=co.Count>3 ? int.Parse(co[3]): 1}
;int lL=fv+n5+lS.Length+1;if(lL<eE.Length){string aI=eE.Substring(lL).Trim();int o_=aI.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(o_>=0){string eM=aI.Substring(o_+5).Trim();if(eM.StartsWith("(")&&eM.Contains(")"))bo.ci=ad(aP(eM,0));else if(eM.StartsWith(":")){
string cr=eM.Substring(1).Trim();if(cr.StartsWith("\"")){int mM=cr.IndexOf('"',1);bo.ci=mM>0 ? cr.Substring(1,mM-1): ad(cr);}else{int oG=cr.IndexOf(' ');
bo.ci=oG>=0 ? cr.Substring(0,oG): cr;}}}string nU=e5(aI,"orientation:");if(nU!=null)bo.hv=nU.ToLowerInvariant();string lK=e5(aI,"title_align:");
if(lK!=null)bo.hD=lK.ToLowerInvariant();string oX=e5(aI,"title_color:");if(oX!=null)bo.hE=oX;int fB=aI.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(fB>=0&& !aI.Substring(fB).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aI.Substring(fB).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bo.kY=e5(aI.Substring(fB),"border:");}int fy=aI.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(fy<0)fy=aI.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(fy>=0){string bN=aI.Substring(fy+"border_gradient".Length).Trim();if(bN.StartsWith(":"))bN=bN.Substring(1).Trim();if(bN.StartsWith("(")&&bN.Contains(")"))bN=aP(bN,0);
bo.hd=aQ(bN).Select(th=>th.Trim()).ToArray();}}return bo;}static string e5(string op,string nu){int nm=op.IndexOf(nu,StringComparison.OrdinalIgnoreCase);
if(nm<0)return null;string kI=op.Substring(nm+nu.Length).Trim();int oH=kI.IndexOf(' ');return oH>=0 ? kI.Substring(0,oH): kI;}static string[]lp(string jq){
int iU=jq.IndexOfAny(new[]{':','('});string ef=jq.Substring(jq[iU]==':' ? iU+1 : iU).Trim();if(ef.StartsWith("(")&&ef.Contains(")"))ef=aP(ef,0);return aQ(ef).Select(ti=>ti.Trim()).ToArray();
}static List<string>p7(string rn,List<string>iF,Dictionary<string,List<string>>qZ){var o2=new Dictionary<string,bF>(StringComparer.OrdinalIgnoreCase);
var dd=(rn ?? "").Replace("\r\n","\n").Split('\n');var jF=new List<string>();int c3=0;while(c3<dd.Length){string j0=dd[c3++];if(j0.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var am=new List<string>();while(c3<dd.Length){string lI=dd[c3++].Replace("\t","    ").TrimEnd();if(lI.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
am.Add(lI);}while(am.Count>0&&am[0].Length==0)am.RemoveAt(0);while(am.Count>0&&am[am.Count-1].Length==0)am.RemoveAt(am.Count-1);int ja=int.MaxValue;
foreach(var hQ in am)if(hQ.Length>0)ja=Math.Min(ja,hQ.Length-hQ.TrimStart().Length);for(int er=0;er<am.Count;er++)if(am[er].Length>0)am[er]=am[er].Substring(ja);
qZ[j0.TrimStart().Substring(8).Trim()]=am;continue;}string bS=lu(j0).Trim();if(bS.Length==0)continue;if(bS.StartsWith("@template")){try{
var ky=p6(bS);while(c3<dd.Length){string q8=dd[c3++];string ie=lu(q8).Trim();if(ie.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(ie.Length>0)ky.g9.Add(ie);}o2[ky.ht]=ky;}catch(Exception ry){iF.Add($"Template parse error ({ry.Message}) in \"{bS}\"");}}
else if(bS.StartsWith("@use")){try{k2(bS,o2,jF,iF);}catch(Exception rz){iF.Add($"@use error ({rz.Message}) in \"{bS}\"");
}}else{jF.Add(bS);}}return jF;}static bF p6(string sm){string j1=sm.Substring("@template".Length).Trim();int jO=j1.IndexOf('(');
if(jO<0)throw new FormatException("missing '(' in @template header");string sy=j1.Substring(0,jO).Trim();string qL=aP(j1,jO);var kz=new bF{
ht=sy};foreach(var sF in aQ(qL)){var jl=sF.Split(new[]{'='},2);string n0=hu(jl[0]);kz.fe.Add(n0);if(jl.Length>1)kz.hf[n0]=ad(jl[1].Trim());
}return kz;}static void k2(string sn,Dictionary<string,bF>o3,List<string>nX,List<string>rw,int mB=0){string gx=sn.Substring("@use".Length).Trim();
int gl=gx.IndexOf('(');string jy=gl>=0 ? gx.Substring(0,gl).Trim(): gx.Trim();string qM=gl>=0 ? aP(gx,gl): "";bF dk=null;
if(!o3.TryGetValue(jy,out dk)){if(!kZ.TryGetValue(jy,out dk))throw new FormatException($"unknown template '{jy}'");}var fE=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var nz in dk.hf)fE[nz.Key]=nz.Value;var lR=aQ(qM);for(int d0=0;d0<lR.Count;d0++){string ft=lR[d0];int iD=ft.IndexOf('=');if(iD>=0){
string r5=hu(ft.Substring(0,iD));string tX=ad(ft.Substring(iD+1).Trim());fE[r5]=tX;}else{if(d0<dk.fe.Count){string r6=dk.fe[d0];
fE[r6]=ad(ft.Trim());}}}foreach(var q7 in dk.g9){string eb=q7;foreach(var ga in fE){eb=eb.Replace("$"+ga.Key,ga.Value).Replace("{"+ga.Key+"}",ga.Value);
}if(eb.StartsWith("@use")){if(mB>3)throw new FormatException("@use nested too deep");k2(eb,o3,nX,rw,mB+1);
}else nX.Add(eb);}}static void p(z rl,string rt){if(rl==null)throw new FormatException($"'{rt}' used before any @screen declared");
}static string lu(string a9){if(string.IsNullOrEmpty(a9))return a9;bool i9=false;for(int bQ=0;bQ<a9.Length;bQ++){char ij=a9[bQ];if(ij=='"'){i9= !i9;
}else if(!i9){if(ij=='/'&&bQ+1<a9.Length&&a9[bQ+1]=='/'){return a9.Substring(0,bQ);}if(ij=='#'){if(pU(a9,bQ)){continue;}return a9.Substring(0,bQ);}}}return a9;
}static bool pU(string cw,int np){int c6=0;for(int i5=np+1;i5<cw.Length;i5++){char cV=cw[i5];if((cV>='0'&&cV<='9')||(cV>='a'&&cV<='f')||(cV>='A'&&cV<='F'))c6++;
else break;}if(c6==3||c6==4||c6==6||c6==8){int eu=np+1+c6;if(eu>=cw.Length||char.IsWhiteSpace(cw[eu])||cw[eu]==')'||cw[eu]==','||cw[eu]==':')return true;
}return false;}static bg p2(string so){string j2=so.Substring("@bind".Length).Trim();int iE=j2.IndexOf('=');if(iE<0)throw new FormatException("missing '=' in @bind");
string nS=j2.Substring(0,iE).Trim();string iG=j2.Substring(iE+1).Trim();int jP=iG.IndexOf('(');if(jP<0)throw new FormatException("missing '(' in @bind target");
string fU=iG.Substring(0,jP).Trim().ToLowerInvariant();string qN=aP(iG,jP);var bK=aQ(qN);if(bK.Count==0)throw new FormatException("@bind target has no arguments");
var ap=new bg{ab=nS};if(fU=="dynamic"||fU=="var"||fU=="mem"){ap.bz=a_.dy;ap.bh=bK.Count>0 ? ad(bK[0].Trim()): nS;
for(int hR=0;hR<bK.Count;hR++){var gb=bK[hR].Split(new[]{':'},2);if(gb.Length==2&&gb[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double mA;
if(double.TryParse(gb[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mA))ap.e4=mA;else ap.he=ad(gb[1].Trim());
}}return ap;}ap.bz=fU=="group" ? a_.ho : a_.ps;string fr=null;for(int fq=0;fq<bK.Count;fq++){var c5=bK[fq].Split(new[]{':'},2);if(c5.Length==2&&c5[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
fr=c5[1].Trim().ToLowerInvariant();}else if(c5.Length==2&&c5[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ap.eY=c5[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ap.aR.Add(ad(bK[fq].Trim()));}}if(ap.aR.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ap.bz==a_.ho){if(fr==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(fr){case "avg": ap.dp=aE.g3;break;case "sum": ap.dp=aE.lw;break;case "min": ap.dp=aE.lk;break;case "max": ap.dp=aE.lj;
break;default: throw new FormatException($"unknown agg '{fr}' (expected avg/sum/min/max)");}}return ap;}static bA p4(string fA){var o=new bA();
if(!fA.StartsWith("\""))throw new FormatException("line must start with a quoted label");int iC=fA.IndexOf('"',1);if(iC<0)throw new FormatException("unterminated quoted label");
o.W=fA.Substring(1,iC-1);string eD=fA.Substring(iC+1).Trim();int gm=eD.IndexOf('(');if(gm<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string C=eD.Substring(0,gm).Trim().ToLowerInvariant();string qJ=aP(eD,gm);string af=eD.Substring(eD.IndexOf(')',gm)+1).Trim();
var iy=qJ.Trim().Split('.');if(iy.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.aN=iy[0].Trim();o.I=iy[1].Trim();if(C=="bar")o.h=e.dr;else if(C=="led")o.h=e.dF;else if(C=="spark"||C=="sparkline")o.h=e.cg;
else if(C=="hazard"||C=="stripes")o.h=e.dC;else if(C=="chevron"||C=="chevrons")o.h=e.dv;else if(C=="dots"||C=="dot")o.h=e.dx;
else if(C=="balance"||C=="horizon"||C=="centered")o.h=e.cI;else if(C=="toggle"||C=="switch")o.h=e.cj;else if(C=="checkbox"||C=="check")o.h=e.b8;
else if(C=="status"||C=="badge"||C=="state")o.h=e.ch;else if(C=="ammo"||C=="pips")o.h=e.cG;else if(C=="flow"||C=="rate"||C=="spinner")o.h=e.ca;
else if(C=="text")o.h=e.hC;else if(C=="tank"||C=="tube"||C=="bucket")o.h=e.fi;else throw new FormatException($"unknown display function '{C}'");
while(af.Length>0){int iw=af.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);int c0=af.IndexOf('(');if(iw>=0&&(c0<0||iw<c0)){
string fJ=af.Substring(iw+4).Trim();int gG=fJ.IndexOf(' ');o.k0=(gG>=0 ? fJ.Substring(0,gG): fJ).ToLowerInvariant();af=(gG>=0 ? fJ.Substring(gG): "").Trim();
continue;}int kG=af.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(kG>=0&&(c0<0||kG<c0)){string gZ=af.Substring(kG+5).Trim();
int gH=gZ.IndexOf(' ');o.dO=(gH>=0 ? gZ.Substring(0,gH): gZ).Trim().Trim('"');af=(gH>=0 ? gZ.Substring(gH): "").Trim();continue;}int iP=af.IndexOf("format:",StringComparison.OrdinalIgnoreCase);
if(iP>=0&&(c0<0||iP<c0)){string fO=af.Substring(iP+7).Trim();int gI=fO.IndexOf(' ');o.dz=(gI>=0 ? fO.Substring(0,gI): fO).Trim().Trim('"');
af=(gI>=0 ? fO.Substring(gI): "").Trim();continue;}int gg=af.IndexOf('(');if(gg<0)break;string au=af.Substring(0,gg).Trim().ToLowerInvariant();
int nF=au.LastIndexOf(' ');if(nF>=0)au=au.Substring(nF+1).Trim();string bn=aP(af,gg);af=af.Substring(af.IndexOf(')',gg)+1).Trim();
if(au=="gradient"){o.cb=true;o.a3=aQ(bn).Select(tj=>tj.Trim()).ToArray();if(o.a3.Length<2)throw new FormatException("gradient() needs at least 2 colors");
}else if(au=="flat"){o.ar=bn.Trim();}else if(au=="pattern"){string Q=bn.Trim().ToLowerInvariant();if(Q=="hazard"||Q=="stripes")o.h=e.dC;
else if(Q=="chevron"||Q=="chevrons")o.h=e.dv;else if(Q=="dots"||Q=="dot")o.h=e.dx;else if(Q=="led")o.h=e.dF;
else if(Q=="balance"||Q=="horizon")o.h=e.cI;else if(Q=="toggle"||Q=="switch")o.h=e.cj;else if(Q=="checkbox"||Q=="check")o.h=e.b8;
else if(Q=="status"||Q=="badge"||Q=="state")o.h=e.ch;else if(Q=="ammo"||Q=="pips")o.h=e.cG;else if(Q=="flow"||Q=="rate"||Q=="spinner")o.h=e.ca;
}else if(au=="color"){string rZ;o.bw=lo(bn,out rZ);}else if(au=="dir"||au=="direction"){o.k0=bn.Trim().ToLowerInvariant();
}else if(au=="unit"){o.dO=bn.Trim().Trim('"');}else if(au=="max"){double nN;if(double.TryParse(bn.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nN))o.fd=nN;
else{var jv=bn.Trim().Split('.');if(jv.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.cN=jv[0].Trim();o.hr=jv[1].Trim();
}}else if(au=="format"||au=="fmt"){o.dz=bn.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{au}'");}}return o;}static string aP(string gB,int jD){
int iv=0;for(int el=jD;el<gB.Length;el++){if(gB[el]=='(')iv++;else if(gB[el]==')'){iv--;if(iv==0)return gB.Substring(jD+1,el-jD-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aQ(string qO){return qO.Split(',').Select(tk=>tk.Trim()).Where(tl=>tl.Length>0).ToList();
}static List<aO>lo(string qI,out string iL){var gz=new List<aO>();iL=null;foreach(var jQ in aQ(qI)){int ir=jQ.LastIndexOf(':');
if(ir<0)throw new FormatException("color() entries need 'expr : color'");string gc=jQ.Substring(0,ir).Trim();string mt=jQ.Substring(ir+1).Trim();
if(gc.Equals("default",StringComparison.OrdinalIgnoreCase)){gz.Add(new aO{cL=true,b9=mt});}else{string oL=lv(gc);
if(iL==null)iL=gc.Substring(0,gc.Length-oL.Length).Trim();gz.Add(new aO{hg=oL,b9=mt});}}if(!gz.Any(s7=>s7.cL))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return gz;}static void lt(string s8,out string q4,out string rC){var ex=(s8 ?? "").Split('.');if(ex.Length!=2||ex[0].Trim().Length==0||ex[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
q4=ex[0].Trim();rC=ex[1].Trim();}static string lv(string c_){c_=c_.Trim();foreach(var sC in new[]{">=","<=","==",">","<"}){int i6=c_.IndexOf(sC,StringComparison.Ordinal);
if(i6>0)return c_.Substring(i6).Trim();if(i6==0)return c_;}throw new FormatException($"color() rule has no recognizable comparison operator: '{c_}'");}
static string ad(string cx){if(cx.Length>=2&&cx[0]=='"'&&cx[cx.Length-1]=='"')return cx.Substring(1,cx.Length-2);return cx;}}public static class pq{public static double pp(aE qE,List<double>dm){
if(dm==null||dm.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(qE){case aE.g3: return dm.Average();case aE.lw: return dm.Sum();
case aE.lk: return dm.Min();case aE.lj: return dm.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class H{
public static string qg(string sf,double go,int l_=10,string pb=null){go=Math.Max(0,Math.Min(100,go));int mZ=(int)Math.Round(l_*(go/100.0));
string q_=new string('#',mZ)+new string('-',l_-mZ);string tY=pb!=null ? pb : $"{go:0}%";return $"{sf} [{q_}] {tY}";
}public static string k8(double b1,string kF=null,string fT=null){if(kF=="auto"){double hS=Math.Abs(b1);if(hS>=1e9)return(b1/1e9).ToString("F1")+"G";
if(hS>=1e6)return(b1/1e6).ToString("F1")+"M";if(hS>=1e3)return(b1/1e3).ToString("F1")+"k";string rA= !string.IsNullOrEmpty(fT)? fT : "F0";return b1.ToString(rA);
}string m0= !string.IsNullOrEmpty(fT)? fT :(Math.Abs(b1)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(kF)){return b1.ToString(m0);}return b1.ToString(m0)+kF;
}public static float pQ(string sY,float oN,float kv,float fK=1.0f){string c9=(sY ?? "standard").ToLowerInvariant();
if(c9=="compact"||(c9=="auto"&&oN<=300&&kv<=300&&kv>160)){return fK*0.75f;}if(c9=="corner"||(c9=="auto"&&kv<=160)){
return fK*0.55f;}if(c9=="wide"||(c9=="auto"&&oN>=1000)){return fK*1.15f;}return fK;}public static string qe(string sg,double gp,int bO=5){
gp=Math.Max(0,Math.Min(100,gp));double fL=(gp-50.0)/50.0;int es=0;int eG=0;if(fL< -0.01)es=(int)Math.Round(-fL*bO);
else if(fL>0.01)eG=(int)Math.Round(fL*bO);if(es>bO)es=bO;if(eG>bO)eG=bO;string sj=new string('-',bO-es)+new string('<',es);
string td=new string('>',eG)+new string('-',bO-eG);return $"{sg} [{sj}|{td}] {gp:0}%";}public static string qq(string nC,double t_){
return t_>=50.0 ? $"{nC} [====O] ON" : $"{nC} [O====] OFF";}public static string qj(string nD,double t0){return t0>=50.0 ? $"{nD} [X] ACTIVE" : $"{nD} [ ] READY ";
}public static string qp(string jn,double o9){if(o9>=75.0)return $"{jn} [ ONLINE  ]";if(o9>=25.0)return $"{jn} [ STANDBY ]";return $"{jn} [ OFFLINE ]";
}public static string qd(string sh,double gq,int nJ=8){gq=Math.Max(0,Math.Min(100,gq));int m_=(int)Math.Round(nJ*(gq/100.0));
string sT=new string('|',m_)+new string('.',nJ-m_);return $"{sh} [{sT}] {gq:0}%";}public static string qk(string jo,double eB,int oZ=0){
char[]tA=new[]{'|','/','-','\\'};char oq=tA[(oZ>=0 ? oZ : 0)%4];if(eB>55.0)return $"{jo} {oq} ▲ CHG {eB:0}%";if(eB<45.0)return $"{jo} {oq} ▼ DSC {eB:0}%";
return $"{jo} ● STBY {eB:0}%";}public static bool pY(string av,string eo){if(av==null||eo==null)return false;if(av=="*"||av==eo)return true;
if(!av.Contains("*")&& !av.Contains("?"))return string.Equals(av,eo,StringComparison.OrdinalIgnoreCase);int aB=0,em=0,km= -1,ju=0;while(em<eo.Length){
if(aB<av.Length&&(av[aB]=='?'||char.ToLowerInvariant(av[aB])==char.ToLowerInvariant(eo[em]))){aB++;em++;}else if(aB<av.Length&&av[aB]=='*'){
km=aB;ju=em;aB++;}else if(km!= -1){aB=km+1;ju++;em=ju;}else{return false;}}while(aB<av.Length&&av[aB]=='*')aB++;return aB==av.Length;
}public static int pu(double sG,int o1){return Math.Max(0,Math.Min(o1,1+(int)(Math.Max(0.0,sG)*o1)));}public static string e7(string[]dj,double jR){
if(dj==null||dj.Length==0)return "white";jR=Math.Max(0,Math.Min(100,jR));int i7=(int)Math.Floor(jR/100.0*dj.Length);if(i7>=dj.Length)i7=dj.Length-1;
return dj[i7];}public static n lb(string[]b_,double sP){if(b_==null||b_.Length==0)return n.fj;if(b_.Length==1)return n.dH(b_[0]);
double tb=Math.Max(0.0,Math.Min(1.0,sP/100.0));double ov=tb*(b_.Length-1);int i8=(int)Math.Floor(ov);int rY=Math.Min(b_.Length-1,i8+1);
double kq=ov-i8;n cW=n.dH(b_[i8]);n ik=n.dH(b_[rY]);byte s9=(byte)Math.Round(cW.bC+(ik.bC-cW.bC)*kq);
byte rR=(byte)Math.Round(cW.bx+(ik.bx-cW.bx)*kq);byte qY=(byte)Math.Round(cW.bs+(ik.bs-cW.bs)*kq);return new n(s9,rR,qY);}public static string dI(List<aO>ol,double t1){
foreach(var j4 in ol){if(j4.cL)continue;if(pM(j4.hg,t1))return j4.b9;}var mz=ol.FirstOrDefault(ta=>ta.cL);return mz!=null ? mz.b9 : null;
}static bool pM(string ec,double eT){ec=ec.Trim();foreach(var jC in new[]{">=","<=","==",">","<"}){if(ec.StartsWith(jC)){double eF=double.Parse(ec.Substring(jC.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(jC){case ">=": return eT>=eF;case "<=": return eT<=eF;case "==": return Math.Abs(eT-eF)<1e-9;case ">": return eT>eF;case "<": return eT<eF;
}}}throw new FormatException($"unsupported comparison expression: '{ec}'");}public static List<string>qi(List<string>rk,int dn){
var gi=new List<string>();gi.Add("+"+new string('-',dn+2)+"+");foreach(var jr in rk){string tT=jr.Length>dn ? jr.Substring(0,dn): jr;
string sJ=tT.PadRight(dn);gi.Add("| "+sJ+" |");}gi.Add("+"+new string('-',dn+2)+"+");return gi;}public struct ln{public float lD,lE,lC,ld;
}public static ln pA(int mq,int oi,int gJ,int gK,int eO,int eP,float tK,float tJ,float n1,float n2,float iW,float iX){
eO=Math.Max(1,eO);eP=Math.Max(1,eP);gJ=Math.Max(1,Math.Min(eO-mq,gJ));gK=Math.Max(1,Math.Min(eP-oi,gK));
float qR=tK-(n1*2f);float qQ=tJ-(n2*2f);float ml=(qR-(iW*(eO-1)))/eO;float mk=(qQ-(iX*(eP-1)))/eP;
float s_=n1+mq*(ml+iW);float s0=n2+oi*(mk+iX);float sZ=gJ*ml+((gJ-1)*iW);float sR=gK*mk+((gK-1)*iX);return new ln{
lD=s_,lE=s0,lC=sZ,ld=sR};}public struct lr{public int p0;public int hq;public int ac;public int ay;public int dN;
public int a0;public int qw;public int qx;public int pJ;public int pK;
public double pL;}public static lr pB(z bY){var l=new lr();var n4=bY.ah!=null&&bY.ah.Count>0 ? bY.ah : new List<az>{
new az{at=bY.fc}};l.p0=n4.Count;l.hq=0;l.ac=0;l.ay=0;if(!string.IsNullOrEmpty(bY.bt)&& !bY.bt.Equals("none",StringComparison.OrdinalIgnoreCase))l.ac++;
if(!string.IsNullOrEmpty(bY.du)&& !bY.du.Equals("none",StringComparison.OrdinalIgnoreCase))l.ac+=2;foreach(var ey in n4){
if(!string.IsNullOrEmpty(ey.cH)&& !ey.cH.Equals("none",StringComparison.OrdinalIgnoreCase))l.ac++;if(!string.IsNullOrEmpty(ey.ci))l.ac+=2;
l.hq+=ey.at.Count;foreach(var se in ey.at){l.qw++;l.qx++;l.ac+=2;switch(se.h){case e.dr: l.dN++;
l.a0++;l.ac+=2;l.ay++;break;case e.dF: l.a0+=10;l.ac+=10;
l.ay+=10;break;case e.dC: l.a0+=10;l.ac+=11;l.ay+=10;break;case e.dv: l.a0+=10;
l.ac+=10;l.ay+=10;break;case e.dx: l.a0+=12;l.ac+=12;l.ay+=12;
break;case e.cg: l.dN++;l.a0+=8;l.ac+=9;l.ay+=8;break;case e.cI: l.dN+=2;
l.a0++;l.ac+=3;l.ay++;break;case e.cj: case e.b8: l.dN++;
l.a0++;l.ac+=2;l.ay++;break;case e.ch: case e.ca: l.dN++;l.a0+=2;
l.ac+=3;l.ay+=2;break;case e.cG: l.a0+=8;l.ac+=8;l.ay+=8;
break;default: l.ay++;break;}}}l.pJ=25+(l.hq*2);l.pK=180+(l.ay*4);
l.pL=0.0;return l;}}