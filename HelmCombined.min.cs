iv t;string iP=null;Dictionary<string,List<IMyTerminalBlock>>b0=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>f8=new Dictionary<string,bool>();
Dictionary<string,bu>ep=new Dictionary<string,bu>();Dictionary<string,Color>mT=new Dictionary<string,Color>();List<IMyTerminalBlock>cG=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>de=new List<IMyTerminalBlock>();bool f5;int iM;string eq="";StringBuilder et=new StringBuilder();readonly Dictionary<string,double>aQ=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>bC=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener er;bool cH=false;
Dictionary<E,IMyTextSurface>b1=new Dictionary<E,IMyTextSurface>();Dictionary<E,string>cI=new Dictionary<E,string>();
static readonly string[]mA=new string[101];static Program(){for(int gP=0;gP<=100;gP++){mA[gP]=gP+"%";}}static string mo(double vB){int eX=(int)Math.Round(vB);
if(eX<0)eX=0;else if(eX>100)eX=100;return mA[eX];}class aA{public List<MySprite>f=new List<MySprite>();public List<cy>ei=new List<cy>();
public int ct= -1;public Color[]fD;public Color fz;public bool fA;public List<iu>bW=new List<iu>();public Vector2 ed,io;
public int en;public bool eb=false;public List<fE>dZ=new List<fE>();}class iu{public string mz;public int iA;public Vector2 mE,mI;
}class fE{public int fU;public Color hV,fK;public List<ay>f1,fQ;public string hZ,h6,h7,h9;public bool mu,ib;
}class cy{public bx ef;public int ab= -1;public int a4= -1;public int c9= -1;public bool mq=false;
public List<int>D=new List<int>();public float[]cC=new float[8];public float bb;public float aD;public float ba;
public float bt;public double fW= -99999.0;public string mw;public float C= -1f;public bool mO;public float mf=12f;}Dictionary<E,aA>b2=new Dictionary<E,aA>();
int mV=0;double iT=0;double eu=0;double T=0;int es=0;Dictionary<E,double>bB=new Dictionary<E,double>();
Dictionary<E,double>iL=new Dictionary<E,double>();Dictionary<string,int>gc=new Dictionary<string,int>();List<string>df=new List<string>();
Dictionary<string,int>gd=new Dictionary<string,int>();double iV,iS;HashSet<string>ga=new HashSet<string>();Dictionary<string,double[]>iR=new Dictionary<string,double[]>();
Dictionary<string,double>ew=new Dictionary<string,double>();List<MyInventoryItem>iO=new List<MyInventoryItem>();int b3= -1,gb;double iU;
Dictionary<E,double>f6=new Dictionary<E,double>();int ev=0;const string ru="0.11.0";IMyTextSurface a5;bool iK=true;
double f7=0;string mU="";string iQ="";int f9=0,iN=0;List<KeyValuePair<string,int>>dd=new List<KeyValuePair<string,int>>();
const double rM=120;const float q_=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{er=IGC.RegisterBroadcastListener("HELM");
er.SetMessageCallback("HELM_IGC");}catch{}f0();iB();mt();}public void Main(string m7,UpdateType vx){
double n0=Runtime.TimeSinceLastRun.TotalSeconds;T+=n0;if((vx&UpdateType.IGC)!=0||m7=="HELM_IGC"){rK();}string pK=(m7 ?? "").Trim();
string aq=pK.ToLowerInvariant();if(aq=="reload"){f0();}else if(aq=="refresh"){ix();}else if(aq=="boot"){iB();}else if(aq=="probe"){
eu=T+20.0;rj();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(aq=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=r8;
f0();iB();}else if(aq=="console on"||aq=="console off"){iK=aq=="console on";mt();f7=0;}else if(aq=="scan"||aq=="diagnose"||aq=="test"){
r4();return;}else if(aq.StartsWith("set ")){string hk=pK.Substring(4).Trim();int n4=hk.IndexOf('=');int u7=hk.IndexOf(' ');int ls=n4>0 ? n4 : u7;
if(ls>0){string oN=hk.Substring(0,ls).Trim();string qF=hk.Substring(ls+1).Trim();l6($"{oN}={qF}");Echo($"[HELM] Set dynamic variable '{oN}' = {qF}");
}return;}else if(aq.StartsWith("alert")){string ft=aq.Length>5 ? aq.Substring(5).Trim(): "toggle";if(ft=="toggle")cH= !cH;else cH=ft=="red"||ft=="on"||ft=="1"||ft=="true";
Echo($"[HELM] Red alert state: {cH}");return;}else if(Me.CustomData!=iP){f0();}ev++;iT+=n0;if(iT>=rM){
iT=0;ix();}if(eu>0){if(T<eu){iJ();return;}eu=0;b2.Clear();cI.Clear();
f7=0;}if(t==null||t.Z.Count==0){iJ();mg();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cU=t.Z[mV%t.Z.Count];
mV++;iQ=cU.bg;bool iZ=bB.Count>0;if(iZ)rT();double n1;if(!bB.ContainsKey(cU)&& !(iL.TryGetValue(cU,out n1)&&T<n1)){
if(rY(cU))iZ=true;if(cU.iw>0)iL[cU]=T+cU.iw/60.0;}es=iZ ? 0 : es+1;Runtime.UpdateFrequency=es<=t.Z.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
r7();r6();r5();iJ();mg();f9=Runtime.CurrentInstructionCount;if(f9>iN)iN=f9;}void mg(){
if(eq.Length>0)Echo(eq);if(et.Length>0)Echo(et.ToString());}List<IMyTerminalBlock>fB(bool si=false){if(!f5){de.Clear();
GridTerminalSystem.GetBlocks(de);cG.Clear();cG.AddRange(de);cG.RemoveAll(sv=> !sv.IsSameConstructAs(Me));iM=de.Count-cG.Count;
f5=true;}return si ? de : cG;}IMyTerminalBlock mj(string pd,bool sk=false){var i8=GridTerminalSystem.GetBlockWithName(pd);
if(i8==null||sk||i8.IsSameConstructAs(Me))return i8;var i0=fB();for(int gQ=0;gQ<i0.Count;gQ++)if(i0[gQ].CustomName.IndexOf(pd,StringComparison.OrdinalIgnoreCase)>=0)return i0[gQ];
return null;}void ra(string pe,List<IMyTerminalBlock>e_,bool m0=false){var om=GridTerminalSystem.GetBlockGroupWithName(pe);if(om==null)return;int nj=e_.Count;
var fq=new List<IMyTerminalBlock>();om.GetBlocks(fq);l4(fq,e_,m0);if(e_.Count>nj)return;var oz=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(oz);
foreach(var qU in oz){if(qU.Name.IndexOf(pe,StringComparison.OrdinalIgnoreCase)<0)continue;fq.Clear();qU.GetBlocks(fq);l4(fq,e_,m0);if(e_.Count>nj)return;
}}void l4(List<IMyTerminalBlock>tp,List<IMyTerminalBlock>oK,bool sj=false){foreach(var i9 in tp)if((sj||i9.IsSameConstructAs(Me))&& !oK.Contains(i9))oK.Add(i9);
}void f0(){iP=Me.CustomData;t=ms.rD(iP);eq="";if(t.be.Count>0){var pW=new StringBuilder("HELM config errors:\n");
foreach(var s8 in t.be)pW.AppendLine(" - "+s8);eq=pW.ToString();}if(t.eo.Count>0){var qS=new StringBuilder("HELM notes:\n");foreach(var vG in t.eo)qS.AppendLine(" - "+vG);
eq+=qS.ToString();}ep.Clear();foreach(var ja in t.cu)if(!ep.ContainsKey(ja.ag))ep[ja.ag]=ja;b2.Clear();cI.Clear();
bB.Clear();iL.Clear();f6.Clear();rp();ix();}void rK(){try{while(er!=null&&er.HasPendingMessage){
var o8=er.AcceptMessage();if(o8.Data is string){l6((string)o8.Data);}}}catch{}}void l6(string px){
if(string.IsNullOrEmpty(px))return;var pu=px.Split(';');for(int j9=0;j9<pu.Length;j9++){var fa=pu[j9].Trim();if(fa.Length==0)continue;int eJ=fa.IndexOf('=');
if(eJ<0)eJ=fa.IndexOf(':');if(eJ>0){string dx=fa.Substring(0,eJ).Trim();string br=fa.Substring(eJ+1).Trim();if(dx.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cH=br.Equals("red",StringComparison.OrdinalIgnoreCase)||br=="1"||br.Equals("true",StringComparison.OrdinalIgnoreCase)||br.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double ph;bC.Remove(dx);if(double.TryParse(br,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out ph)){
aQ[dx]=ph;}else if(br.Equals("true",StringComparison.OrdinalIgnoreCase)||br.Equals("on",StringComparison.OrdinalIgnoreCase)){aQ[dx]=100.0;
}else if(br.Equals("false",StringComparison.OrdinalIgnoreCase)||br.Equals("off",StringComparison.OrdinalIgnoreCase)){aQ[dx]=0.0;}else{bC[dx]=br;
}}}}}const string r8="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void rj(){var i1=new List<IMyTextSurface>();if(a5!=null)i1.Add(a5);foreach(var uc in b1.Values)i1.Add(uc);foreach(var u2 in i1)ri(u2);
}static void bz(List<MySprite>u8,float s1,float s2,float vH,float tw,Color sP){u8.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(s1,s2),Size=new Vector2(vH,tw),Color=sP}
);}void ri(IMyTextSurface lt){Vector2 aX=lt.SurfaceSize,aK=lt.TextureSize,ao=(aK-aX)/2f;var aJ=new List<MySprite>();Color gM=new Color(150,150,150);
bz(aJ,aK.X/2f,2f,aK.X,4f,gM);bz(aJ,aK.X/2f,aK.Y-2f,aK.X,4f,gM);bz(aJ,2f,aK.Y/2f,4f,aK.Y,gM);bz(aJ,aK.X-2f,aK.Y/2f,4f,aK.Y,gM);
bz(aJ,ao.X+aX.X/2f,ao.Y+aX.Y/2f,aX.X,aX.Y,new Color(20,40,90));float an=32f;bz(aJ,ao.X+an/2f,ao.Y+an/2f,an,an,new Color(255,0,0));bz(aJ,ao.X+aX.X-an/2f,ao.Y+an/2f,an,an,new Color(0,255,0));
bz(aJ,ao.X+an/2f,ao.Y+aX.Y-an/2f,an,an,new Color(255,255,0));bz(aJ,ao.X+aX.X-an/2f,ao.Y+aX.Y-an/2f,an,an,new Color(255,0,255));float l2=ao.Y+aX.Y/2f-30f;
a9(aJ,"SURFACE "+aX.X.ToString("0")+"x"+aX.Y.ToString("0"),Color.White,ao.X+44f,l2,0.9f);a9(aJ,"TEXTURE "+aK.X.ToString("0")+"x"+aK.Y.ToString("0"),Color.White,ao.X+44f,l2+24f,0.9f);
a9(aJ,"OFFSET  "+ao.X.ToString("0")+","+ao.Y.ToString("0"),Color.White,ao.X+44f,l2+48f,0.9f);using(var ti=lt.DrawFrame()){ti.AddRange(aJ);}}static bool mv(string bE){
return bE=="rate"||bE=="eta"||bE=="trend"||bE.Length>3&&(bE.StartsWith("top")&&char.IsDigit(bE[3])||bE.Length>4&&bE.StartsWith("line")&&char.IsDigit(bE[4]));}void rp(){
gc.Clear();df.Clear();gd.Clear();ga.Clear();iR.Clear();b3= -1;iU=iV=iS=0;foreach(var lo in t.Z){var kF=new List<List<bx>>();
foreach(var ux in lo.ak)kF.Add(ux.ap);if(lo.ak.Count==0)kF.Add(lo.fX);foreach(var t6 in kF)foreach(var bF in t6){if(!mv(bF.I))continue;
if(bF.I=="rate"||bF.I=="eta"||bF.I=="trend"){ga.Add(bF.aM);continue;}bool qz=bF.I[0]=='t';var nU=qz ? gc : gd;int t8=int.Parse(bF.I.Substring(qz ? 3 : 4));
int oD;nU.TryGetValue(bF.aM,out oD);nU[bF.aM]=Math.Max(oD,t8);}}df.AddRange(gc.Keys);}void r6(){if(gd.Count==0||T<iV)return;
iV=T+2.0;foreach(var kw in gd){List<IMyTerminalBlock>jh;var p4=b0.TryGetValue(kw.Key,out jh)&&jh.Count>0 ? jh[0]as IMyTextSurface : null;
string[]o4=p4==null ? new string[0]: p4.GetText().Split('\n');for(int fe=1;fe<=kw.Value;fe++)bC[kw.Key+".line"+fe]=fe<=o4.Length ? o4[fe-1].TrimEnd('\r'): "";}}
void r5(){if(ga.Count==0||T<iS)return;iS=T+5.0;foreach(var dj in ga){double hP=a2(dj,"percent"),cj=0.0;double[]gN;
if(iR.TryGetValue(dj,out gN))cj=gN[2]*0.6+(hP-gN[0])/Math.Max(0.1,T-gN[1])*0.4;iR[dj]=new[]{hP,T,cj};double fc=cj*60.0;int cZ=(int)Math.Min(359999.0,(cj<0 ? hP/ -cj : cj>0 ?(100.0-hP)/cj : 0.0)+0.5);
bC[dj+".rate"]=(fc>=0 ? "+" : "")+fc.ToString("0.0")+"%/min";bC[dj+".trend"]=fc>0.05 ? "^" : fc< -0.05 ? "v" : "=";bC[dj+".eta"]=Math.Abs(fc)<0.05 ? "--" :(cj<0 ? "empty " : "full ")+(cZ>=3600 ? cZ/3600+"h "+cZ%3600/60+"m" : cZ>=60 ? cZ/60+"m "+cZ%60+"s" : cZ+"s");
}}void r7(){if(df.Count==0)return;if(b3<0){if(T<iU)return;b3=0;gb=0;ew.Clear();}while(b3<df.Count&&Runtime.CurrentInstructionCount<20000){
string jk=df[b3];List<IMyTerminalBlock>gm;b0.TryGetValue(jk,out gm);if(gm!=null&&gb<gm.Count){var m9=gm[gb++];
for(int ka=0;ka<m9.InventoryCount;ka++){iO.Clear();m9.GetInventory(ka).GetItems(iO);foreach(var gW in iO){string ks=gW.Type.TypeId.Replace("MyObjectBuilder_","");
string oO=ks=="Ore"||ks=="Ingot" ? gW.Type.SubtypeId+" "+ks.ToLowerInvariant(): gW.Type.SubtypeId;double oE;ew.TryGetValue(oO,out oE);ew[oO]=oE+(double)gW.Amount;
}}continue;}var hi=new List<KeyValuePair<string,double>>(ew);hi.Sort((vJ,vP)=>vP.Value.CompareTo(vJ.Value));for(int dG=1;dG<=gc[jk];dG++)bC[jk+".top"+dG]=dG<=hi.Count ? hi[dG-1].Key+" "+M.mn(hi[dG-1].Value,"auto"): "-";
ew.Clear();b3++;gb=0;}if(b3>=df.Count){b3= -1;iU=T+15.0;}}void aj(int vh,string vp){dd.Add(new KeyValuePair<string,int>(vp,vh));
}static string iI(string lh,int pc){return lh.Length>pc ? lh.Substring(0,pc): lh;}void mt(){var la=Me as IMyTextSurfaceProvider;if(la==null||la.SurfaceCount<1)return;
a5=la.GetSurface(0);a5.ContentType=iK ? ContentType.SCRIPT : ContentType.NONE;a5.Script="";a5.ScriptBackgroundColor=Color.Black;
}void iJ(){if(T>=f7){f7=T+(es>t.Z.Count+1 ? 5.0 : 1.0);q7();if(iK&&a5!=null&&eu==0)rh();
}Echo(mU);}void q7(){dd.Clear();int jn=0,nq=0;foreach(var na in t.cu){List<IMyTerminalBlock>kz;if(na.bU==bc.d5){
jn++;continue;}if(b0.TryGetValue(na.ag,out kz)){nq+=kz.Count;if(kz.Count>0)jn++;}}int n5=t.be.Count;aj(0,"HELM-[Kernel-(v"+ru+")]");
aj(4,"------------------------------");aj(1,"krn-cfg");aj(2," screens "+t.Z.Count+"  binds "+jn+"/"+t.cu.Count+"  errors "+n5);
aj(2," alert "+(cH ? "RED" : "off")+"  blocks "+nq+"  docked-ignored "+iM);if(n5>0)aj(3," ! "+iI(t.be[0],40));
else if(t.eo.Count>0)aj(3," ? "+iI(t.eo[0],40));aj(1,"cfg-dsp");if(t.Z.Count==0)aj(3," no @screen: run 'starter'");
int u4=0;foreach(var li in t.Z){if(++u4>10){aj(2," +"+(t.Z.Count-10)+" more");break;}IMyTextSurface lu;string p7="--",lA="no block";
if(b1.TryGetValue(li,out lu)){p7=lu.SurfaceSize.X.ToString("0")+"x"+lu.SurfaceSize.Y.ToString("0");lA=bB.ContainsKey(li)? "boot" : "live";
}aj(lA=="no block" ? 3 : 2," "+iI(li.bg,14).PadRight(14)+" "+p7.PadRight(9)+" "+lA);}aj(1,"perf");aj(2," tick "+ev+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
aj(2," instr "+f9+"  peak "+iN+"  max "+Runtime.MaxInstructionCount);aj(2," rate "+Runtime.UpdateFrequency+"  idle "+es+"  up "+(int)T+"s");
if(iQ.Length>0)aj(2," last "+iQ);var pX=new StringBuilder();foreach(var tM in dd)pX.AppendLine(tM.Key);mU=pX.ToString();}static Color rd(int vi){
switch(vi){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void rh(){Vector2 cV=a5.SurfaceSize;Vector2 kN=(a5.TextureSize-cV)/2f;int jx=30;foreach(var tN in dd)jx=Math.Max(jx,tN.Key.Length);
Vector2 jw=a5.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float pY=Math.Min(1f,Math.Min(cV.X*0.94f/(jx*jw.X),cV.Y*0.94f/(dd.Count*jw.Y)));
float vK=kN.X+cV.X*0.03f;float qX=kN.Y+cV.Y*0.03f;var lw=new List<MySprite>();lw.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kN+cV/2f,Size=cV,Color=new Color(12,12,16)}
);foreach(var oT in dd){a9(lw,oT.Key,rd(oT.Value),vK,qX,pY);qX+=jw.Y*pY;}using(var tj=a5.DrawFrame()){tj.AddRange(lw);}}static readonly List<string>q6=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void iB(){bB.Clear();if(t==null)return;for(int gR=0;gR<t.Z.Count;gR++){var hp=t.Z[gR];if(hp.h2>0f)bB[hp]=T+(gR/Math.Max(1,hp.ma))*hp.md/60.0;
}}void rT(){List<E>eI=null;int tv=(bB.Count+3)/4,tF=0;foreach(var g_ in bB){double kV=(T-g_.Value)/g_.Key.h2;if(kV<0)continue;
if(kV>=1.0){if(eI==null)eI=new List<E>();eI.Add(g_.Key);continue;}if((tF++ +ev)%tv==0)rg(g_.Key,kV);}if(eI==null)return;foreach(var fl in eI){
bB.Remove(fl);if(fl.h1>0f)f6[fl]=T;b2.Remove(fl);cI.Remove(fl);}}static void a9(List<MySprite>u9,string vq,Color sS,float vL,float vQ,float u_){
var qm=MySprite.CreateText(vq,"Monospace",sS,u_,TextAlignment.LEFT);qm.Position=new Vector2(vL,vQ);u9.Add(qm);}static void l3(List<MySprite>va,List<string>gh,E hs,float vM,float vR,float u0,float tY){
string[]vf=hs.fF!=null&&hs.fF.Length>0 ? hs.fF : new[]{hs.mb};for(int eR=0;eR<gh.Count;
eR++){p jq=M.mp(vf,gh.Count>1 ? 100.0*eR/(gh.Count-1): 0.0);a9(va,gh[eR],new Color(jq.bX,jq.bS,jq.bN),vM,vR+eR*tY,u0);}}bool rX(E dK,IMyTextSurface lE){
aA aC,aS;E p0=t.Z.Find(qV=>qV!=dK&&qV.bg.Equals(dK.ii,StringComparison.OrdinalIgnoreCase));if(p0==null|| !b2.TryGetValue(p0,out aC)|| !aC.eb)return false;
if(!b2.TryGetValue(dK,out aS))b2[dK]=aS=new aA();Vector2 dN=lE.SurfaceSize;if(aS.eb&&aS.en==aC.en&&aS.ed==dN)return false;
aS.eb=true;aS.en=aC.en;aS.ed=dN;aS.f.Clear();Vector2 hB=aC.ed,pl=aC.io,pj=(lE.TextureSize-dN)/2f;aS.io=pj;
int oj=0,qy=aC.f.Count;if(dK.ij!=null){int dF=aC.bW.FindIndex(vN=>vN.mz.Equals(dK.ij,StringComparison.OrdinalIgnoreCase));
if(dF<0)return false;hB=aC.bW[dF].mI;pl=aC.bW[dF].mE;oj=aC.bW[dF].iA;if(dF+1<aC.bW.Count)qy=aC.bW[dF+1].iA;}float gY=Math.Min(dN.X/hB.X,dN.Y/hB.Y);
Vector2 ss=pj+(dN-hB*gY)/2f;for(int kb=oj;kb<qy;kb++){var bG=aC.f[kb];if(bG.Position.HasValue)bG.Position=(bG.Position.Value-pl)*gY+ss;if(bG.Size.HasValue)bG.Size=bG.Size.Value*gY;
if(bG.Type==SpriteType.TEXT)bG.RotationOrScale*=gY;aS.f.Add(bG);}using(var tk=lE.DrawFrame())tk.AddRange(aS.f);return true;}bool rR(E dL,IMyTextSurface hz){
List<string>ez;if(cI.ContainsKey(dL)|| !t.dW.TryGetValue(dL.cs,out ez))return false;cI[dL]="banner";Vector2 cW=hz.SurfaceSize;
Vector2 kO=(hz.TextureSize-cW)/2f;int gs=1;foreach(var tO in ez)gs=Math.Max(gs,tO.Length);Vector2 eF=hz.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float hr=Math.Min(cW.X*0.94f/(gs*eF.X),cW.Y*0.94f/(ez.Count*eF.Y));var lx=new List<MySprite>();lx.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kO+cW/2f,Size=cW,Color=string.IsNullOrEmpty(dL.bO)? new Color(10,12,16): J(dL.bO)}
);l3(lx,ez,dL,kO.X+(cW.X-gs*eF.X*hr)/2f,kO.Y+(cW.Y-ez.Count*eF.Y*hr)/2f,hr,eF.Y*hr);using(var tl=hz.DrawFrame()){tl.AddRange(lx);
}return true;}void rg(E dM,double po){IMyTextSurface fn;if(!b1.TryGetValue(dM,out fn))return;Vector2 co=fn.SurfaceSize;
Vector2 kP=(fn.TextureSize-co)/2f;var e3=dM.rq();string o9=dM.mc ?? "HELM TELEMETRY KERNEL ONLINE";bool kG=co.Y>=200f;
List<string>dg;if(dM.cv==null|| !t.dW.TryGetValue(dM.cv,out dg))dg=q6;int dp=o9.Length+3;foreach(var tP in e3)dp=Math.Max(dp,tP.Length+5);
if(kG)foreach(var tQ in dg)dp=Math.Max(dp,tQ.Length);int pP=(kG ? dg.Count+1 : 0)+2+e3.Count;Vector2 dn=fn.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float ck=Math.Min(1f,Math.Min(co.X*0.94f/(dp*dn.X),co.Y*0.94f/(pP*dn.Y)));float hR=kP.X+(co.X-dp*dn.X*ck)/2f;float c1=kP.Y+(co.Y-pP*dn.Y*ck)/2f;
float g3=dn.Y*ck;var dO=new List<MySprite>();dO.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kP+co/2f,Size=co,Color=new Color(10,12,16)}
);if(kG){l3(dO,dg,dM,hR,c1,ck,g3);c1+=(dg.Count+1)*g3;}a9(dO,">> "+o9,new Color(34,197,94),hR,c1,ck);c1+=g3*2f;int u5=M.q5(po,e3.Count);
for(int gS=0;gS<u5;gS++){bool pk=gS==e3.Count-1&&po>=0.8;a9(dO,pk ? "[OK]" : "[..]",pk ? new Color(34,197,94): new Color(56,189,248),hR,c1,ck);a9(dO,e3[gS],new Color(203,213,225),hR+5f*dn.X*ck,c1,ck);
c1+=g3;}using(var tm=fn.DrawFrame()){tm.AddRange(dO);}}void ix(){b0.Clear();b1.Clear();f8.Clear();
et.Clear();f5=false;if(t==null)return;foreach(var af in t.cu){var a6=new List<IMyTerminalBlock>();if(af.bU==bc.d5){
string kq= !string.IsNullOrEmpty(af.bw)? af.bw : af.ag;if(!aQ.ContainsKey(kq)){aQ[kq]=af.fL;if(af.h4!=null)bC[kq]=af.h4;
}b0[af.ag]=a6;continue;}var qq=af.a3.Count>0 ? af.a3 : new List<string>{af.mL};foreach(var c_ in qq){
if(string.IsNullOrEmpty(c_))continue;bool oM=c_.Contains("*")||c_.Contains("?");if(af.bU==bc.id||oM){var j3=new List<IMyTerminalBlock>();
ra(c_,j3,af.fC);if(j3.Count>0){foreach(var ov in j3){if(!a6.Contains(ov))a6.Add(ov);}}else{var m2=fB(af.fC);
for(int jb=0;jb<m2.Count;jb++){var hH=m2[jb];if(oM ? M.rz(c_,hH.CustomName): string.Equals(c_,hH.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!a6.Contains(hH))a6.Add(hH);}}}}else{var jg=mj(c_,af.fC);if(jg!=null&& !a6.Contains(jg))a6.Add(jg);}}if(a6.Count==0){
r2(af,qq,a6);if(a6.Count>0)f8[af.ag]=true;}b0[af.ag]=a6;}foreach(var bK in t.Z){
if(string.IsNullOrEmpty(bK.bV))continue;var kS=mj(bK.bV);if(kS==null){et.AppendLine($"HELM: output block '{bK.bV}' not found");
continue;}IMyTextSurface cY=null;var lb=kS as IMyTextSurfaceProvider;var nV=kS as IMyTextSurface;if(lb!=null&&bK.cz<lb.SurfaceCount){
cY=lb.GetSurface(bK.cz);}else if(nV!=null&&bK.cz==0){cY=nV;}if(cY==null){et.AppendLine($"HELM: output block '{bK.bV}' has no surface index {bK.cz}");
continue;}cY.ContentType=ContentType.SCRIPT;cY.Script="";cY.ScriptBackgroundColor=Color.Black;b1[bK]=cY;}cG.Clear();
de.Clear();}static readonly string[]mi={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool ro(int tG,IMyTerminalBlock v){switch(tG){case 0: return v is IMyBatteryBlock;case 1: return v is IMyReactor;case 2: return v is IMySolarPanel;
case 3: return v is IMyWindTurbine;case 4: return v is IMyGasTank;case 5: return v is IMyAirVent;case 6: return v is IMyCargoContainer;case 7: return v is IMyJumpDrive;
case 8: return v is IMyThrust;case 9: return v is IMyMotorSuspension;case 10: return v is IMyDoor;case 11: return v is IMyShipConnector;case 12: return v is IMyAssembler;
case 13: return v is IMyRefinery;case 14: return v is IMyShipDrill;case 15: return v is IMyShipWelder;case 16: return v is IMyShipGrinder;case 17: return v is IMyGasGenerator;
case 18: return v is IMyLargeTurretBase;case 19: return v is IMyProjector;case 20: return v is IMyGyro;case 21: return v is IMyGravityGeneratorBase;case 22: return v is IMyLandingGear;
case 23: return v is IMyRadioAntenna;case 24: return v is IMyBeacon;case 25: return v is IMyLightingBlock;case 26: return v is IMyPistonBase;case 27: return v is IMyMotorStator;
case 28: return v is IMyWarhead;case 29: return v is IMySensorBlock;case 30: return v is IMyCameraBlock;case 31: return v is IMyParachute;case 32: return v is IMyTurretControlBlock;
}return false;}void r2(bu sH,List<string>vn,List<IMyTerminalBlock>ji){string tA=(sH.ag+" "+string.Join(" ",vn)).ToLowerInvariant();
var ku=new List<int>();for(int gZ=0;gZ<mi.Length;gZ++){foreach(var vI in mi[gZ].Split(','))if(tA.Contains(vI)){ku.Add(gZ);break;}}if(ku.Count==0)return;
var m1=fB();var pm=new List<IMyTerminalBlock>();for(int kc=0;kc<m1.Count;kc++){var jc=m1[kc];foreach(int tH in ku){if(ro(tH,jc)){(jc.CubeGrid==Me.CubeGrid ? ji : pm).Add(jc);
break;}}}if(ji.Count==0)ji.AddRange(pm);}void r4(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(t==null||t.Z.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {t.Z.Count} screens, {t.cu.Count} binds");f5=false;
Echo($"Blocks: {fB().Count} on this construct; {iM} on docked ships (ignored)");cG.Clear();Echo("--- BIND RESOLUTION ---");foreach(var at in t.cu){
if(at.bU==bc.d5){string kr= !string.IsNullOrEmpty(at.bw)? at.bw : at.ag;double vy=aQ.ContainsKey(kr)? aQ[kr]: at.fL;
Echo($"[OK] @bind {at.ag} (dynamic variable '{kr}' = {vy})");continue;}List<IMyTerminalBlock>jj;if(b0.TryGetValue(at.ag,out jj)){if(jj.Count>0){
string te=f8.ContainsKey(at.ag)&&f8[at.ag]? " [fallback: typed match]" : "";Echo($"[OK] @bind {at.ag} ({jj.Count} blocks matched{te})");
}else{string vo=string.Join(", ",at.a3.Count>0 ? at.a3 : new List<string>{at.mL});Echo($"[WARN] @bind {at.ag} -> 0 blocks matched \"{vo}\"");
}}else{Echo($"[ERR] @bind {at.ag} -> unresolved");}}if(aQ.Count>0){Echo($"--- DYNAMIC VARIABLES ({aQ.Count}) ---");foreach(var oV in aQ)Echo($"[VAR] {oV.Key} = {oV.Value}");
}IMyTextSurface i3=null;foreach(var uf in b1.Values){i3=uf;break;}if(i3!=null){var ly=new List<string>();i3.GetSprites(ly);
Echo($"--- SPRITES ({ly.Count}): names usable in bg_texture ---");Echo(string.Join(", ",ly.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bL in t.Z){IMyTextSurface hA;if(b1.TryGetValue(bL,out hA)){var p8=hA.SurfaceSize;var vd=M.rc(bL);
Echo($"[OK] Screen \"{bL.bg}\" [{bL.cA}] -> {bL.bV} [#{bL.cz}] ({p8.X:0}x{p8.Y:0} px, texture {hA.TextureSize.X:0}x{hA.TextureSize.Y:0}, {vd.ah} sprites)");
}else{Echo($"[ERR] Screen \"{bL.bg}\" -> Missing block \"{bL.bV}\" [#{bL.cz}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double r3(IMyTerminalBlock w){
if(w==null)return 0.0;var gk=w as IMyBatteryBlock;if(gk!=null)return gk.MaxStoredPower>0 ?(gk.CurrentStoredPower/gk.MaxStoredPower)*100.0 : 0.0;
var gX=w as IMyJumpDrive;if(gX!=null)return gX.MaxStoredPower>0 ?(gX.CurrentStoredPower/gX.MaxStoredPower)*100.0 : 0.0;var qp=w as IMyGasTank;
if(qp!=null)return qp.FilledRatio*100.0;var qO=w as IMyAirVent;if(qO!=null)return qO.GetOxygenLevel()*100.0;var ny=w as IMyCargoContainer;if(ny!=null){
var gU=ny.GetInventory();if(gU!=null&&gU.MaxVolume.RawValue>0)return(double)gU.CurrentVolume.RawValue/gU.MaxVolume.RawValue*100.0;return 0.0;}var hj=w as IMyReactor;
if(hj!=null)return hj.MaxOutput>0 ?(hj.CurrentOutput/hj.MaxOutput)*100.0 : 0.0;var ht=w as IMySolarPanel;if(ht!=null)return ht.MaxOutput>0 ?(ht.CurrentOutput/ht.MaxOutput)*100.0 : 0.0;
var hQ=w as IMyWindTurbine;if(hQ!=null)return hQ.MaxOutput>0 ?(hQ.CurrentOutput/hQ.MaxOutput)*100.0 : 0.0;var nW=w as IMyDoor;if(nW!=null)return(double)nW.OpenRatio*100.0;
var hK=w as IMyThrust;if(hK!=null)return hK.MaxEffectiveThrust>0 ?(hK.CurrentThrust/hK.MaxEffectiveThrust)*100.0 : 0.0;var lJ=w as IMyMotorSuspension;
if(lJ!=null)return lJ.Brake ? 100.0 :(lJ.Friction*100.0);var jB=w as IMyShipConnector;if(jB!=null)return jB.Status==MyShipConnectorStatus.Connected ? 100.0 :(jB.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var pD=w as IMyProductionBlock;if(pD!=null)return pD.IsProducing ? 100.0 : 0.0;var n_=w as IMyShipDrill;if(n_!=null)return n_.Enabled ? 100.0 : 0.0;
var lR=w as IMyShipToolBase;if(lR!=null)return lR.IsActivated ? 100.0 :(lR.Enabled ? 50.0 : 0.0);var j0=w as IMyGasGenerator;if(j0!=null)return j0.AutoRefill ? 100.0 :(j0.Enabled ? 50.0 : 0.0);
var hL=w as IMyLargeTurretBase;if(hL!=null)return hL.IsShooting ? 100.0 :(hL.HasTarget ? 50.0 :(hL.Enabled ? 10.0 : 0.0));var fd=w as IMyProjector;
if(fd!=null)return fd.TotalBlocks>0 ?((double)(fd.TotalBlocks-fd.RemainingBlocks)/fd.TotalBlocks)*100.0 : 0.0;var j6=w as IMyGyro;if(j6!=null)return j6.GyroOverride ? 100.0 :(j6.Enabled ? 50.0 : 0.0);
var ox=w as IMyGravityGeneratorBase;if(ox!=null)return Math.Min(100.0,Math.Abs(ox.GravityAcceleration)/9.81*100.0);var j_=w as IMyLandingGear;if(j_!=null)return j_.IsLocked ? 100.0 :(j_.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var m3=w as IMyRadioAntenna;if(m3!=null)return(m3.Radius/50000.0)*100.0;var nh=w as IMyBeacon;if(nh!=null)return(nh.Radius/50000.0)*100.0;
var o2=w as IMyLightingBlock;if(o2!=null)return o2.Intensity*10.0;var he=w as IMyPistonBase;if(he!=null)return he.HighestPosition>0 ?(he.CurrentPosition/he.HighestPosition)*100.0 : 0.0;
var pM=w as IMyMotorStator;if(pM!=null)return Math.Min(100.0,Math.Abs(pM.Angle)/6.28318*100.0);var qR=w as IMyWarhead;if(qR!=null)return qR.IsArmed ? 100.0 : 0.0;
var lr=w as IMySensorBlock;if(lr!=null)return!lr.LastDetectedEntity.IsEmpty()? 100.0 :(lr.Enabled ? 25.0 : 0.0);var jt=w as IMyCameraBlock;
if(jt!=null)return jt.IsActive ? 100.0 :(jt.Enabled ? 50.0 : 0.0);var nE=w as IMyParachute;if(nE!=null)return(double)nE.OpenRatio*100.0;
var hJ=w as IMyTurretControlBlock;if(hJ!=null)return hJ.HasTarget ? 100.0 :(hJ.IsUnderControl ? 50.0 :(hJ.Enabled ? 10.0 : 0.0));var ok=w as IMyFunctionalBlock;
if(ok!=null)return ok.Enabled ? 100.0 : 0.0;return 0.0;}static double rw(IMyTerminalBlock nb,string jS){int nx=jS.IndexOf(':');string kt=jS.Substring(0,nx).ToLowerInvariant();
var vv=new MyItemType("MyObjectBuilder_"+(kt=="ammo" ? "AmmoMagazine" : char.ToUpper(kt[0])+kt.Substring(1)),jS.Substring(nx+1));double qi=0;for(int kd=0;
kd<nb.InventoryCount;kd++)qi+=(double)nb.GetInventory(kd).GetItemAmount(vv);return qi;}double mF(IMyTerminalBlock x,string ob){if(x==null)return 0.0;
string c=ob.ToLowerInvariant();if(c=="percent"||c=="pct")return r3(x);if(c.IndexOf(':')>0)return rw(x,ob);var b6=x as IMyBatteryBlock;
if(b6!=null){if(c=="current"||c=="stored"||c=="val"||c=="value"||c=="raw")return b6.CurrentStoredPower;if(c=="max"||c=="capacity")return b6.MaxStoredPower;
if(c=="output")return b6.CurrentOutput;if(c=="maxoutput")return b6.MaxOutput;if(c=="input")return b6.CurrentInput;if(c=="maxinput")return b6.MaxInput;
return b6.CurrentStoredPower;}var kp=x as IMyJumpDrive;if(kp!=null){if(c=="max"||c=="capacity")return kp.MaxStoredPower;return kp.CurrentStoredPower;
}var hE=x as IMyGasTank;if(hE!=null){if(c=="max"||c=="capacity")return hE.Capacity;return hE.FilledRatio*hE.Capacity;}var qP=x as IMyAirVent;
if(qP!=null){if(c=="max"||c=="capacity")return 100.0;return qP.GetOxygenLevel()*100.0;}var nz=x as IMyCargoContainer;if(nz!=null){var gV=nz.GetInventory();
if(gV!=null){if(c=="mass")return(double)gV.CurrentMass;if(c=="max"||c=="capacity")return(double)gV.MaxVolume;return(double)gV.CurrentVolume;}return 0.0;}var lc=x as IMyPowerProducer;
if(lc!=null){if(c=="max"||c=="capacity")return lc.MaxOutput;return lc.CurrentOutput;}var nX=x as IMyDoor;if(nX!=null){if(c=="max")return 1.0;return(double)nX.OpenRatio*100.0;
}var lO=x as IMyThrust;if(lO!=null){if(c=="max"||c=="capacity")return lO.MaxEffectiveThrust;return lO.CurrentThrust;}var cq=x as IMyMotorSuspension;
if(cq!=null){if(c=="brake")return cq.Brake ? 100.0 : 0.0;if(c=="height")return cq.Height;if(c=="strength")return cq.Strength;if(c=="power")return cq.Power;
if(c=="steer")return cq.SteeringOverride*100.0;if(c=="propulsion")return cq.PropulsionOverride*100.0;return cq.Friction;}var nM=x as IMyShipConnector;
if(nM!=null){return nM.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var k9=x as IMyProductionBlock;if(k9!=null){if(c=="producing"||c=="active"||c=="running"||c=="state")return k9.IsProducing ? 100.0 : 0.0;
if(c=="queue")return k9.IsQueueEmpty ? 0.0 : 1.0;}var i7=x as IMyAssembler;if(i7!=null){if(c=="disassembling")return i7.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(c=="repeating"||c=="loop")return i7.Repeating ? 100.0 : 0.0;}var j1=x as IMyGasGenerator;if(j1!=null){if(c=="autorefill")return j1.AutoRefill ? 100.0 : 0.0;
if(c=="active")return j1.Enabled ? 100.0 : 0.0;}var hM=x as IMyLargeTurretBase;if(hM!=null){if(c=="shooting")return hM.IsShooting ? 100.0 : 0.0;
if(c=="target"||c=="hastarget")return hM.HasTarget ? 100.0 : 0.0;if(c=="range")return hM.Range;}var ci=x as IMyProjector;if(ci!=null){if(c=="built")return ci.TotalBlocks-ci.RemainingBlocks;
if(c=="remaining")return ci.RemainingBlocks;if(c=="total")return ci.TotalBlocks;if(c=="projecting")return ci.IsProjecting ? 100.0 : 0.0;if(c=="armor")return ci.RemainingArmorBlocks;
if(c=="buildable")return ci.BuildableBlocksCount;}var lS=x as IMyShipToolBase;if(lS!=null){if(c=="working"||c=="active"||c=="activated")return lS.IsActivated ? 100.0 : 0.0;
return lS.Enabled ? 100.0 : 0.0;}var du=x as IMyGyro;if(du!=null){if(c=="override")return du.GyroOverride ? 100.0 : 0.0;if(c=="pitch")return du.Pitch;
if(c=="yaw")return du.Yaw;if(c=="roll")return du.Roll;if(c=="power")return du.GyroPower*100.0;}var oy=x as IMyGravityGeneratorBase;if(oy!=null){if(c=="g"||c=="accel")return oy.GravityAcceleration;
}var ow=x as IMyLandingGear;if(ow!=null){if(c=="locked")return ow.IsLocked ? 100.0 : 0.0;}var m4=x as IMyRadioAntenna;if(m4!=null){if(c=="radius"||c=="range")return m4.Radius;
}var ni=x as IMyBeacon;if(ni!=null){if(c=="radius"||c=="range")return ni.Radius;}var kC=x as IMyLightingBlock;if(kC!=null){if(c=="intensity")return kC.Intensity;
if(c=="radius")return kC.Radius;}var hf=x as IMyPistonBase;if(hf!=null){if(c=="pos"||c=="position"||c=="extension")return hf.CurrentPosition;
if(c=="velocity")return hf.Velocity;if(c=="max")return hf.HighestPosition;}var fk=x as IMyMotorStator;if(fk!=null){if(c=="angle"||c=="deg")return fk.Angle*180.0/Math.PI;
if(c=="rad")return fk.Angle;if(c=="velocity"||c=="rpm")return fk.TargetVelocityRPM;if(c=="locked")return fk.RotorLock ? 100.0 : 0.0;}var l0=x as IMyWarhead;
if(l0!=null){if(c=="armed")return l0.IsArmed ? 100.0 : 0.0;if(c=="countdown")return l0.DetonationTime;}var p3=x as IMySensorBlock;if(p3!=null){
if(c=="detected")return!p3.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var ju=x as IMyCameraBlock;if(ju!=null){if(c=="range")return ju.AvailableScanRange;
if(c=="active")return ju.IsActive ? 100.0 : 0.0;}var nF=x as IMyParachute;if(nF!=null){if(c=="open"||c=="openratio")return(double)nF.OpenRatio*100.0;
}var dS=x as IMyTurretControlBlock;if(dS!=null){if(c=="target"||c=="hastarget")return dS.HasTarget ? 100.0 : 0.0;if(c=="azimuth")return dS.AzimuthRotor!=null ? dS.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(c=="elevation")return dS.ElevationRotor!=null ? dS.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var ol=x as IMyFunctionalBlock;if(ol!=null){return ol.Enabled ? 100.0 : 0.0;
}return 0.0;}double a2(string eC,string oc){bu b7;ep.TryGetValue(eC,out b7);if(b7!=null&&b7.bU==bc.d5){
string tI= !string.IsNullOrEmpty(b7.bw)? b7.bw : eC;double nP;if(aQ.TryGetValue(tI,out nP))return nP;return b7.fL;
}double n2;if(aQ.TryGetValue(eC,out n2))return n2;List<IMyTerminalBlock>eD;if(!b0.TryGetValue(eC,out eD)||eD.Count==0)throw new InvalidOperationException($"HELM: bind '{eC}' resolved to zero blocks — check the name/typo");
if(eD.Count==1)return mF(eD[0],oc);var qN=new List<double>();foreach(var sw in eD)qN.Add(mF(sw,oc));return q1.q0(b7!=null ? b7.dV : aL.hU,qN);
}double iy(string sJ){return a2(sJ,"percent");}static string bf(double qJ,bx dA){if(dA==null||(string.IsNullOrEmpty(dA.em)&&string.IsNullOrEmpty(dA.d6)&&dA.I=="percent")){
return mo(qJ);}return M.mn(qJ,dA.em,dA.d6);}bool rY(E cl){IMyTextSurface fo;if(!b1.TryGetValue(cl,out fo))return false;
if(cl.cs!=null)return rR(cl,fo);if(cl.ii!=null)return rX(cl,fo);if(cl.c5.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return rZ(cl,fo);}return r_(cl,fo);}Color al(bx oW,Color s3){return oW.iH!=null ? J(oW.iH): s3;
}Color d7(string[]dP,double uo){if(dP.Length==1)return J(dP[0]);double pH=Math.Max(0.0,Math.Min(100.0,uo))/100.0*(dP.Length-1);int ke=Math.Min((int)pH,dP.Length-2);
float hC=(float)(pH-ke);Color b4=J(dP[ke]),gj=J(dP[ke+1]);return new Color((int)(b4.R+(gj.R-b4.R)*hC),(int)(b4.G+(gj.G-b4.G)*hC),(int)(b4.B+(gj.B-b4.B)*hC),(int)(b4.A+(gj.A-b4.A)*hC));
}static Color[]fx(aA sN,string[]vg,string s5,float qW,float qY,float lY,float j7){int eA=16;bool oF=s5=="horizontal";var jA=new Color[eA];
for(int cO=0;cO<eA;cO++){p jZ=M.mp(vg,100.0*cO/(eA-1));jA[cO]=new Color(jZ.bX,jZ.bS,jZ.bN);float nw=lY/eA,np=j7/eA;q(sN,oF ? new Vector2(qW+nw*(cO+0.5f),qY+j7/2f): new Vector2(qW+lY/2f,qY+np*(cO+0.5f)),oF ? new Vector2(nw+1f,j7): new Vector2(lY,np+1f),jA[cO]);
}return jA;}static void q(aA sO,Vector2 uz,Vector2 u6,Color sT,float uS=0f,string s4="SquareSimple"){sO.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=s4,Position=uz,Size=u6,Color=sT,RotationOrScale=uS}
);}void fy(aA b8,string cX,string[]aR,string nI,bool qa,float aH,float aY,float bJ,float bh){cX=cX.ToLowerInvariant();float V=cX=="heavy" ? 4f : 2f,fw=bJ-aH,j8=bh-aY;
Color bD=J(aR!=null&&aR.Length>0 ? aR[0]: nI ?? "40, 60, 85");Color b9=aR!=null&&aR.Length>1 ? J(aR[aR.Length-1]): bD;Action<float,float,float,float,Color>aV=(vO,vS,pG,pF,sQ)=>q(b8,new Vector2(vO+pG/2f,vS+pF/2f),new Vector2(pG,pF),sQ);
if(cX=="corners"){float aT=Math.Max(10f,Math.Min(fw,j8)*0.15f);aV(aH,aY,aT,V,bD);aV(aH,aY,V,aT,bD);aV(bJ-aT,aY,aT,V,b9);aV(bJ-V,aY,V,aT,b9);aV(aH,bh-V,aT,V,bD);aV(aH,bh-aT,V,aT,bD);
aV(bJ-aT,bh-V,aT,V,b9);aV(bJ-V,bh-aT,V,aT,b9);return;}if(aR!=null&&aR.Length>1&&qa){fx(b8,aR,"horizontal",aH,aY,fw,V);fx(b8,aR,"horizontal",aH,bh-V,fw,V);
}else{aV(aH,aY,fw,V,bD);aV(aH,bh-V,fw,V,b9);}if("|frame|single|heavy|double|rounded|".Contains("|"+cX+"|")){aV(aH,aY,V,j8,bD);aV(bJ-V,aY,V,j8,b9);}if(cX=="rounded"){
float ca=V*3f;q(b8,new Vector2(aH,aY),new Vector2(ca,ca),bD,0f,"Circle");q(b8,new Vector2(bJ,aY),new Vector2(ca,ca),b9,0f,"Circle");q(b8,new Vector2(aH,bh),new Vector2(ca,ca),bD,0f,"Circle");
q(b8,new Vector2(bJ,bh),new Vector2(ca,ca),b9,0f,"Circle");}if(cX=="double")fy(b8,"single",aR,nI,qa,aH+5f,aY+5f,bJ-5f,bh-5f);}void q8(E m,IMyTextSurface lF,aA g){
g.f.Clear();g.ei.Clear();g.bW.Clear();Vector2 aa=lF.SurfaceSize;Vector2 vs=lF.TextureSize;Vector2 bs=(vs-aa)/2f;
g.io=bs;float e9=Math.Max(4f,aa.X*0.02f);float dD=Math.Max(4f,aa.Y*0.02f);if(m.cA=="compact"||(m.cA=="auto"&&aa.Y<=260f&&aa.Y>150f)){
e9=4f;dD=4f;}else if(m.cA=="corner"||(m.cA=="auto"&&aa.Y<=150f)){e9=2f;dD=2f;}if(m.ir>=0f)e9=dD=m.ir;
float qC=aa.X-(e9*2f);Color di=new Color(10,12,16);bool nZ=true;if(!string.IsNullOrEmpty(m.bO)){var eP=p.eg(m.bO);
if(eP.fV)nZ=false;else di=new Color(eP.bX,eP.bS,eP.bN,eP.hS);}g.ct= -1;g.fA=false;g.fz=J(m.l5 ?? "#5b0f0f");
g.dZ.Clear();if(m.bQ!=null&&m.bQ.Length>1){g.ct=g.f.Count;g.fD=fx(g,m.bQ,m.dY,bs.X+2f,bs.Y+2f,aa.X-4f,aa.Y-4f);
}else if(nZ||m.a0!=null||m.c6!=null){if(m.a0!=null)di=J(m.a0.First(uE=>uE.c8).bR);g.ct=g.f.Count;
g.fD=new[]{di};q(g,bs+(aa/2f),aa-new Vector2(4f,4f),di);if(m.a0!=null||m.c6!=null)g.dZ.Add(new fE{
fU=g.ct,hV=di,fK=di,f1=m.a0,hZ=m.hW,h6=m.hX,fQ=m.c6,h7=m.h8,h9=m.ia,mu=true}
);}if(!string.IsNullOrEmpty(m.hY))q(g,bs+(aa/2f),aa-new Vector2(4f,4f),J(m.l7 ?? "255,255,255,24"),0f,m.hY);
if(!m.c5.Equals("none",StringComparison.OrdinalIgnoreCase)){float nr=bs.X+(aa.X-qC)/2f;fy(g,m.c5,m.fJ,m.d0,!m.fS,nr-2f,bs.Y+dD*0.5f-1f,nr+qC+2f,bs.Y+aa.Y-dD*0.5f+1f);
}var k_=m.ak;if(k_.Count==0){k_=new List<aF>{new aF{db="main",d2=0,eh=0,ej=m.d8,ek=m.d9,ap=m.fX}
};}float oA=m.ic>=0f ? m.ic : 8f;foreach(var r in k_){var g7=M.rb(r.d2,r.eh,r.ej,r.ek,m.d8,m.d9,aa.X,aa.Y,e9,dD,oA,oA);
float aw=bs.X+g7.mR;float bH=bs.Y+g7.mS;float aU=g7.mQ;float cR=g7.mr;g.bW.Add(new iu{mz=r.db,iA=g.f.Count,mE=new Vector2(aw,bH),mI=new Vector2(aU,cR)}
);bool pQ=r.a0!=null||r.c6!=null;if(r.bQ!=null&&r.bQ.Length>1){fx(g,r.bQ,r.dY!="vertical" ? r.dY : m.dY,aw,bH,aU,cR);
}else{Color ch;if(r.a0!=null)ch=J(r.a0.First(uF=>uF.c8).bR);else if(!string.IsNullOrEmpty(r.c3))ch=J(r.c3);
else if(!string.IsNullOrEmpty(m.it))ch=J(m.it);else ch=Color.Transparent;if(ch.A>0||pQ){if(pQ)g.dZ.Add(new fE{fU=g.f.Count,hV=ch,fK=ch,f1=r.a0,hZ=r.hW,h6=r.hX,fQ=r.c6,h7=r.h8,h9=r.ia}
);q(g,new Vector2(aw+aU/2f,bH+cR/2f),new Vector2(aU,cR),ch);}}if(r.d_!=null&& !r.d_.Equals("none",StringComparison.OrdinalIgnoreCase))fy(g,r.d_,r.fI ?? m.fJ,r.fH ?? m.d0,!m.fS,aw+1f,bH+1f,aw+aU-1f,bH+cR-1f);
string gJ=r.mk ?? m.ml ?? "Monospace";float sF=M.rr(m.cA,aa.X,aa.Y,m.c7);float aG=r.mm ?? sF;
float eQ=0f;if(!string.IsNullOrEmpty(r.cE)){eQ=20f*aG;var i_=TextAlignment.LEFT;float lP=aw+4f;string mX=r.iF ?? m.mM ?? "left";
if(mX.Equals("center",StringComparison.OrdinalIgnoreCase)){i_=TextAlignment.CENTER;lP=aw+(aU/2f);}else if(mX.Equals("right",StringComparison.OrdinalIgnoreCase)){
i_=TextAlignment.RIGHT;lP=aw+aU-4f;}string qs=r.iG ?? m.mN;Color vt= !string.IsNullOrEmpty(qs)? J(qs): J(r.fH ?? m.d0 ?? "white");
var qx=MySprite.CreateText(r.cE,gJ,vt,aG*0.9f,i_);qx.Position=new Vector2(lP,bH+2f);g.f.Add(qx);
string[]jd=r.fI ?? m.fJ;Color s7=jd!=null&&jd.Length>0 ? J(jd[0]): J(r.fH ?? m.d0 ?? "40, 60, 85");
q(g,new Vector2(aw+aU/2f,bH+eQ-2f),new Vector2(aU-8f,1f),s7);}if(r.ip=="vertical"||r.ap.Any(uv=>uv.i==e.f3)){
q9(g,lF,r,gJ,aG,aw,bH,aU,cR,eQ);continue;}int vU=Math.Max(1,r.ap.Count);float um=cR-eQ-8f;
float qT=0f,mZ=0f;foreach(var uw in r.ap)qT+=1<<uw.f2;float vw=um/Math.Max(1f,qT);float eY=6f;for(int kf=0;kf<r.ap.Count;
kf++){var j=r.ap[kf];var O=new cy{ef=j};float dJ=vw*(1<<j.f2);float pO=bH+eQ+4f+mZ;mZ+=dJ;float L=pO+(dJ/2f);
float qu=L-(11f*aG);O.c9=g.f.Count;O.mq=j.ac!=null&&(j.ac.Contains("{val}")||j.ac.Contains("{percent}"));
float eW=j.fT!=null ? 18f*aG : 0f;var o_=MySprite.CreateText(j.ac,gJ,j.ec!=null ? J(j.ec): Color.White,aG,TextAlignment.LEFT);
o_.Position=new Vector2(aw+eY+eW,qu);g.f.Add(o_);if(j.fT!=null)q(g,new Vector2(aw+eY+eW/2f,L),new Vector2(eW-2f,eW-2f),Color.White,0f,j.fT);
float tW=Math.Max(24f*aG,(j.ac.Length+1)*12f*aG);float vF=j.il ? 0f :(!string.IsNullOrEmpty(j.em)|| !string.IsNullOrEmpty(j.d6))? 64f*aG : 52f*aG;
if(j.i!=e.iD){float K=aw+eY+eW+tW;float sE=aw+aU-eY-vF;float H=Math.Max(16f,sE-K);
float Q=Math.Max(j.i==e.cB ? 8f : 4f,Math.Min((j.i==e.cB ? 20f : 16f)*aG,dJ*(j.i==e.cB ? 0.55f : 0.45f)));
if(j.f2>0)Q=dJ*0.7f;Color qB=al(j,new Color(25,30,38));O.bb=K;O.aD=H;O.ba=Q;O.bt=L;
if(j.i==e.dX){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),qB);O.ab=g.f.Count;
q(g,new Vector2(K,L),new Vector2(0f,Q),J(j.az));bool uQ=j.fN=="left";for(int lQ=1;lQ<j.iE;lQ++)q(g,new Vector2(K+H*lQ/j.iE,L),new Vector2(1f,Q),new Color(0,0,0,150));
if(j.fY!=null)foreach(double o7 in j.fY)q(g,new Vector2(K+H*(float)(uQ ? 1.0-o7/100.0 : o7/100.0),L),new Vector2(2f,Q+6f),Color.White);
if(j.iq!=null)fy(g,"single",null,j.iq,true,K,L-Q/2f,K+H,L+Q/2f);}else if(j.i==e.fM){
float dH=Math.Max(8f,Math.Min(H/2f,dJ*0.85f));float gz=K+H/2f,gA=pO+dJ-4f;int jF=j.bA>0 ? j.bA : 16;O.bb=gz;
O.bt=gA;O.aD=dH;for(int jC=0;jC<jF;jC++){float lN=(jC+0.5f)/jF*3.14159f;O.D.Add(g.f.Count);q(g,new Vector2(gz-(float)Math.Cos(lN)*dH*0.8f,gA-(float)Math.Sin(lN)*dH*0.8f),new Vector2(dH*3.14159f/jF*0.85f,Math.Max(3f,dH*0.16f)),al(j,new Color(24,28,36)),lN-1.5708f);
}O.D.Add(g.f.Count);q(g,new Vector2(gz,gA),new Vector2(3f,dH*0.9f),Color.White);q(g,new Vector2(gz,gA),new Vector2(8f,8f),Color.White,0f,"Circle");
}else if(j.i==e.ee){int lp=j.bA>0 ? j.bA : 10;float oo=2f;float lq=Math.Max(2f,(H-(oo*(lp-1)))/lp);for(int lj=0;
lj<lp;lj++){O.D.Add(g.f.Count);q(g,new Vector2(K+lj*(lq+oo)+(lq/2f),L),new Vector2(lq,Q),al(j,new Color(22,26,32)));
}}else if(j.i==e.cB){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(20,24,30)));int hg=j.bA>0 ? j.bA : 8;
O.cC=new float[hg];float op=2f;float jy=Math.Max(2f,(H-(op*(hg-1)))/hg);for(int kW=0;kW<hg;kW++){O.D.Add(g.f.Count);
q(g,new Vector2(K+kW*(jy+op)+(jy/2f),L),new Vector2(jy,2f),J(j.az));}}else if(j.i==e.ea){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(20,24,30)));
int lB=10;float oq=2f;float lC=Math.Max(2f,(H-(oq*(lB-1)))/lB);for(int lk=0;lk<lB;lk++){O.D.Add(g.f.Count);
q(g,new Vector2(K+lk*(lC+oq)+(lC/2f),L),new Vector2(lC,Q),al(j,new Color(24,28,36)),0.523599f);}}else if(j.i==e.d1){
int i5=10;float os=2f;float i6=Math.Max(2f,(H-(os*(i5-1)))/i5);for(int ll=0;ll<i5;ll++){O.D.Add(g.f.Count);
q(g,new Vector2(K+ll*(i6+os)+(i6/2f),L),new Vector2(i6,Q),al(j,new Color(24,28,36)),1.570796f,"Triangle");}}else if(j.i==e.d4){
int jG=j.bA>0 ? j.bA : 12;float ot=2f;float jI=Math.Max(2f,(H-(ot*(jG-1)))/jG);float nY=Math.Min(jI,Q);for(int lm=0;
lm<jG;lm++){O.D.Add(g.f.Count);q(g,new Vector2(K+lm*(jI+ot)+(jI/2f),L),new Vector2(nY,nY),al(j,new Color(24,28,36)),0f,"Circle");
}}else if(j.i==e.c4){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),qB);O.ab=g.f.Count;
q(g,new Vector2(K+(H/2f),L),new Vector2(0f,Q),J(j.az));q(g,new Vector2(K+(H/2f),L),new Vector2(2f,Q+4f),new Color(80,110,150));
}else if(j.i==e.cF){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(20,24,30)));O.ab=g.f.Count;
float kv=Math.Max(4f,Q);q(g,new Vector2(K+kv*0.5f,L),new Vector2(kv,kv),new Color(40,50,65),0f,"Circle");}else if(j.i==e.cw){
float dk=Math.Max(6f,Math.Min(Q,14f*aG));q(g,new Vector2(K+dk*0.5f,L),new Vector2(dk,dk),new Color(28,34,46));
O.ab=g.f.Count;q(g,new Vector2(K+dk*0.5f,L),new Vector2(dk*0.6f,dk*0.6f),new Color(40,50,65));}else if(j.i==e.cD){
q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(18,22,30)));O.ab=g.f.Count;float k6=Math.Max(4f,Q*0.7f);
q(g,new Vector2(K+k6+2f,L),new Vector2(k6,k6),new Color(34,197,94),0f,"Circle");}else if(j.i==e.c2){int k5=8;
float ou=2f;float k7=Math.Max(2f,(H-(ou*(k5-1)))/k5);for(int kX=0;kX<k5;kX++){O.D.Add(g.f.Count);q(g,new Vector2(K+kX*(k7+ou)+(k7/2f),L),new Vector2(k7,Q),al(j,new Color(24,28,36)));
}}else if(j.i==e.cx){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(20,24,30)));O.ab=g.f.Count;
float oI=Math.Max(4f,Q);q(g,new Vector2(K+(H/2f),L),new Vector2(oI,oI),J(j.az),0f,"Triangle");}}var qH=MySprite.CreateText("---",gJ,Color.White,aG,TextAlignment.RIGHT);
qH.Position=new Vector2(aw+aU-eY,qu);O.a4=g.f.Count;g.f.Add(qH);g.ei.Add(O);}}g.ed=aa;
g.eb=true;}const string sd="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bZ=8;void q9(aA ad,IMyTextSurface vj,aF kZ,string oi,float eN,float uj,float kY,float uk,float pp,float tz){
int t9=Math.Max(1,kZ.ap.Count);float nJ=(uk-12f)/t9;float hN=22f*eN;float bq=kY+tz+4f+hN;float aZ=Math.Max(20f,kY+pp-4f-hN-bq);
float s0=vj.MeasureStringInPixels(new StringBuilder("M"),oi,eN*0.9f).X;for(int gT=0;gT<kZ.ap.Count;gT++){var cP=kZ.ap[gT];float bk=uj+6f+nJ*(gT+0.5f);
float c0=nJ*0.7f;var bn=new cy{ef=cP,mO=true,bb=bk,aD=c0,ba=aZ,bt=bq+aZ/2f,mf=s0};bn.c9=ad.f.Count;
a9(ad.f,cP.ac,cP.ec!=null ? J(cP.ec): Color.White,bk,kY+pp-4f-hN,eN);bn.a4=ad.f.Count;
a9(ad.f,"---",Color.White,bk,bq-hN,eN);var oX=ad.f[bn.c9];oX.Alignment=TextAlignment.CENTER;ad.f[bn.c9]=oX;
var qG=ad.f[bn.a4];qG.Alignment=TextAlignment.CENTER;ad.f[bn.a4]=qG;q(ad,new Vector2(bk,bq+aZ/2f),new Vector2(c0,aZ),al(cP,new Color(25,30,38)));
bn.ab=ad.f.Count;if(cP.i==e.f3){for(int nc=0;nc<bZ;nc++){bn.D.Add(ad.f.Count);q(ad,new Vector2(bk,bq+aZ),new Vector2(c0-6f,0f),Color.White);
}for(int lZ=0;lZ<4;lZ++){bn.D.Add(ad.f.Count);var qn=MySprite.CreateText("",oi,lZ==0 ? new Color(255,255,255,170): new Color(255,255,255,110),eN*0.9f,TextAlignment.CENTER);
qn.Position=new Vector2(bk,bq+aZ);ad.f.Add(qn);}Color jK=new Color(90,110,135);q(ad,new Vector2(bk-c0/2f,bq+aZ/2f),new Vector2(3f,aZ),jK);q(ad,new Vector2(bk+c0/2f,bq+aZ/2f),new Vector2(3f,aZ),jK);
q(ad,new Vector2(bk,bq+aZ),new Vector2(c0+3f,3f),jK);}else{q(ad,new Vector2(bk,bq+aZ),new Vector2(c0,0f),J(cP.az));}ad.ei.Add(bn);
}}bool sc(aA bi,cy z,double fu,double pI,Color gt,bool qL){var a7=z.ef;float nk=z.C;if(z.C<0f)z.C=(float)fu;
z.C=Math.Abs(z.C-(float)fu)>0.05f ? z.C+((float)fu-z.C)*0.35f :(float)fu;float cQ=Math.Max(0f,Math.Min(1f,z.C/100f))*z.ba;
float gq=z.bt+z.ba/2f;float gx=z.bb;if(a7.i!=e.f3){var gE=bi.f[z.ab];gE.Size=new Vector2(z.aD,cQ);
gE.Position=new Vector2(gx,gq-cQ/2f);gE.Color=gt;bi.f[z.ab]=gE;if(qL||z.C!=nk)aO(bi,z,bf(pI,a7),gt);
return qL||z.C!=nk;}float jf=z.ba/bZ;string ff=a7.bv!=null ? M.dc(a7.bv,a7.d3!=null ? a2(a7.d3,a7.h3): fu): null;
Color t3=ff!=null ? J(ff): gt;string[]qf=ff!=null ?(ff.Contains("/")? ff.Split('/'): null): a7.bT&&a7.aN.Length>1 ? a7.aN : null;
for(int cK=0;cK<bZ;cK++){float qQ=Math.Max(0f,Math.Min(jf,cQ-cK*jf));var hh=bi.f[z.D[cK]];hh.Size=new Vector2(z.aD-6f,qQ);
hh.Position=new Vector2(gx,gq-cK*jf-qQ/2f);hh.Color=qf!=null ? d7(qf,100.0*(cK+0.5)/bZ): t3;bi.f[z.D[cK]]=hh;}
int sR=Math.Max(1,(int)((z.aD-8f)/z.mf));bool u3=cQ>6f;var l1=bi.f[z.D[bZ]];l1.Data=u3 ? sd.Substring((int)(T*5.0)%6,Math.Min(sR,100)): "";
l1.Position=new Vector2(gx,gq-cQ-12f);bi.f[z.D[bZ]]=l1;for(int cf=0;cf<3;cf++){var jo=bi.f[z.D[bZ+1+cf]];
double ur=(T*(0.35+0.1*cf)+cf*0.37)%1.0;jo.Data=cQ>24f ?(cf==1 ? "O" : "o"): "";jo.Position=new Vector2(gx+(cf-1)*z.aD*0.26f,gq-(float)ur*(cQ-6f)-10f);
bi.f[z.D[bZ+1+cf]]=jo;}aO(bi,z,bf(pI,a7),gt);return true;}static void aO(aA nt,cy lv,string vr,Color sU){
var lW=nt.f[lv.a4];lW.Data=lv.ef.il ? "" : vr;lW.Color=sU;nt.f[lv.a4]=lW;}static void mH(aA nu,cy p9,Color sV){
var n6=nu.f[p9.ab];n6.Color=sV;nu.f[p9.ab]=n6;}bool r_(E bM,IMyTextSurface lG){aA k;
if(!b2.TryGetValue(bM,out k)){k=new aA();b2[bM]=k;}bool pL= !k.eb||k.ed!=lG.SurfaceSize;
if(pL){q8(bM,lG,k);}bool S=pL;bool ey=(T%bM.hT)<bM.hT/2.0;double cN= -1.0,nN;if(f6.TryGetValue(bM,out nN)){
cN=(T-nN)/bM.h1;if(cN>=1.0){f6.Remove(bM);cN=1.0;}}bool l_=cH&&ey&&k.fz.A>0;if(k.ct>=0&&l_!=k.fA){
k.fA=l_;for(int eS=0;eS<k.fD.Length;eS++){var nl=k.f[k.ct+eS];nl.Color=l_ ? k.fz : k.fD[eS];k.f[k.ct+eS]=nl;
}S=true;foreach(var sG in k.dZ)sG.ib=true;}foreach(var am in k.dZ){if(am.mu&&k.fA)continue;Color eB=am.hV;try{if(am.f1!=null){
double nv=a2(am.hZ,am.h6);string hm=M.dc(am.f1,nv);eB=hm!=null&&hm.Contains("/")? d7(hm.Split('/'),nv): J(hm);
}if(am.fQ!=null&&ey&&M.dc(am.fQ,a2(am.h7,am.h9))!=null)eB=k.fz;}catch{}if(eB!=am.fK||am.ib){
am.fK=eB;am.ib=false;var ns=k.f[am.fU];ns.Color=eB;k.f[am.fU]=ns;S=true;}}for(int kg=0;kg<k.ei.Count;kg++){var b=k.ei[kg];
var h=b.ef;double F;bool oC=false;double kJ=h.fZ;try{F=a2(h.aM,h.I);if(h.da!=null)kJ=a2(h.da,h.ih);
}catch{oC=true;F=0.0;}if(oC){if(b.fW!= -99999.0||(b.a4>=0&&k.f[b.a4].Data!="ERR")){S=true;b.fW= -99999.0;
if(b.a4>=0){var ln=k.f[b.a4];ln.Data="ERR";ln.Color=Color.Red;k.f[b.a4]=ln;}}continue;}double a8=F;
if(kJ>0)F=Math.Min(100.0,a8/kJ*100.0);bool qk=cN>=0.0&&h.i!=e.iD&&h.i!=e.cF&&h.i!=e.cw&&h.i!=e.cD&&h.i!=e.cx;
if(qk)F=cN<0.5 ? cN*200.0 : 100.0+(F-100.0)*(cN-0.5)*2.0;double gw=h.d3!=null ? a2(h.d3,h.h3): F;string gu=h.bv!=null ? M.dc(h.bv,gw): h.bT ? M.fR(h.aN,gw): h.az;
Color P=J(gu);bool bl=(P==Color.Red);if(gu!=null&&gu.Contains("/"))P=d7(gu.Split('/'),gw);if(h.bv==null&&h.bT&& !bM.fS&&h.aN.Length>1)P=d7(h.aN,gw);
if(bl&&ey){P=new Color(255,75,75);S=true;}if(h.h_!=null&&M.dc(h.h_,a2(h.l8,h.l9))!=null){
bl=S=true;if(ey)P=new Color(P.R/3,P.G/3,P.B/3);}bool a_=qk||Math.Abs(a8-b.fW)>0.1;if(a_)b.fW=a8;
if(b.mO){if(sc(k,b,F,a8,P,a_))S=true;continue;}if(h.i==e.dX||h.i==e.fM){double od=(h.ie||h.I=="percent"||h.I=="pct")? F : iy(h.aM);
if(b.C<0f)b.C=(float)od;float hF=(float)od;if(Math.Abs(b.C-hF)>0.05f){b.C+=(hF-b.C)*0.35f;
S=true;}else if(b.C!=hF){b.C=hF;S=true;}if(a_)S=true;if(S){float py=(float)Math.Max(0.0,Math.Min(1.0,b.C/100.0));
float jW=b.aD*py;if(h.i==e.fM){int eH=b.D.Count-1;for(int dr=0;dr<eH;dr++){var p5=k.f[b.D[dr]];
p5.Color=b.C>=(dr+0.5)/eH*100.0 ?(h.bv==null&&h.bT&&h.aN.Length>1 ? d7(h.aN,(dr+0.5)/eH*100.0): P): al(h,new Color(24,28,36));
k.f[b.D[dr]]=p5;}float i2=py*3.14159f;var kM=k.f[b.D[eH]];kM.Position=new Vector2(b.bb-(float)Math.Cos(i2)*b.aD*0.45f,b.bt-(float)Math.Sin(i2)*b.aD*0.45f);
kM.RotationOrScale=i2-1.5708f;k.f[b.D[eH]]=kM;}else{var gG=k.f[b.ab];gG.Size=new Vector2(jW,b.ba);
gG.Position=new Vector2(h.fN=="left" ? b.bb+b.aD-(jW/2f): b.bb+(jW/2f),b.bt);gG.Color=P;
k.f[b.ab]=gG;}aO(k,b,bf(a8,h),P);}}else if(h.i==e.ee||h.i==e.ea||h.i==e.d1||h.i==e.d4){
if(a_||bl){S=true;double tg=(h.ie||h.I=="percent"||h.I=="pct")? F : iy(h.aM);int p1=b.D.Count;
for(int fm=0;fm<p1;fm++){bool t4=tg>=((fm+0.4)/(double)p1*100.0);Color u1=t4 ? P : al(h,new Color(24,28,36));var p2=k.f[b.D[fm]];
p2.Color=u1;k.f[b.D[fm]]=p2;}aO(k,b,bf(a8,h),P);}}else if(h.i==e.cB){
if(a_||bl){S=true;for(int gO=0;gO<b.cC.Length-1;gO++)b.cC[gO]=b.cC[gO+1];b.cC[b.cC.Length-1]=(float)F;
int uy=b.D.Count;for(int e5=0;e5<uy;e5++){float ty=b.cC[e5];float tx=Math.Max(0.06f,Math.Min(1f,ty/100f));float nH=b.ba*tx;
float sW=b.bt+(b.ba/2f)-(nH/2f);var dq=k.f[b.D[e5]];dq.Size=new Vector2(dq.Size.Value.X,nH);
dq.Position=new Vector2(dq.Position.Value.X,sW);dq.Color=P;k.f[b.D[e5]]=dq;}aO(k,b,bf(a8,h),P);
}}else if(h.i==e.c4){if(b.C<0f)b.C=(float)F;float hG=(float)F;if(Math.Abs(b.C-hG)>0.05f){
b.C+=(hG-b.C)*0.35f;S=true;}else if(b.C!=hG){b.C=hG;S=true;}if(S||a_){
float oB=b.aD*0.5f;float nC=b.bb+oB;float nT=(float)Math.Max(-1.0,Math.Min(1.0,(b.C-50.0)/50.0));float jX=Math.Abs(nT)*oB;
float th=(nT>=0)?(nC+jX*0.5f):(nC-jX*0.5f);var gH=k.f[b.ab];gH.Size=new Vector2(jX,b.ba);
gH.Position=new Vector2(th,b.bt);gH.Color=P;k.f[b.ab]=gH;aO(k,b,bf(a8,h),P);
}}else if(h.i==e.cF){if(a_||bl){S=true;bool ko=F>=50.0;float oS=Math.Max(4f,b.ba);float tL=ko ?(b.bb+b.aD-oS*0.5f):(b.bb+oS*0.5f);
Color oR=ko ?(P!=Color.White ? P : new Color(34,197,94)): new Color(60,70,85);var jU=k.f[b.ab];jU.Position=new Vector2(tL,b.bt);
jU.Color=oR;k.f[b.ab]=jU;aO(k,b,ko ? "ON" : "OFF",oR);}}else if(h.i==e.cw){if(a_||bl){
S=true;bool kn=F>=50.0;Color nD=kn ?(P!=Color.White ? P : new Color(56,189,248)): new Color(30,36,48);mH(k,b,nD);
aO(k,b,kn ? "ACTIVE" : "READY",kn ? nD : new Color(148,163,184));}}else if(h.i==e.cD){if(a_||bl){
S=true;string ve=F>=75.0 ? "ONLINE" :(F>=25.0 ? "STBY" : "OFFLINE");Color qe=F>=75.0 ? new Color(34,197,94):(F>=25.0 ? new Color(234,179,8): new Color(239,68,68));
mH(k,b,qe);aO(k,b,ve,qe);}}else if(h.i==e.c2){if(a_||bl){S=true;int pz=b.D.Count;
for(int e6=0;e6<pz;e6++){bool t5=F>=((e6+0.5)/(double)pz*100.0);Color ut=t5 ? P : al(h,new Color(24,28,36));var pA=k.f[b.D[e6]];
pA.Color=ut;k.f[b.D[e6]]=pA;}aO(k,b,bf(a8,h),P);}}else if(h.i==e.cx){
S=true;float hn;Color eM;string gI;bool tD=(h.I=="producing"||h.I=="active"||h.I=="running"||h.I=="state"||(h.I!="percent"&&(F<=1.0||F==100.0)));
if(tD){if(F>0.0){b.C=(b.C+0.3927f)%6.28318f;hn=b.C;eM=new Color(34,197,94);gI="RUN";}else{hn=0f;
eM=new Color(148,163,184);gI="IDLE";}}else{hn=(F>55.0)? 0f :((F<45.0)? 3.14159f : 1.57079f);eM=(F>55.0)? new Color(34,197,94):((F<45.0)? new Color(239,68,68): new Color(148,163,184));
gI=(F>55.0)? "CHG" :((F<45.0)? "DSC" : "STBY");}var jV=k.f[b.ab];jV.RotationOrScale=hn;jV.Color=eM;
k.f[b.ab]=jV;aO(k,b,gI,eM);}else{string gD;bu hI;ep.TryGetValue(h.aM,out hI);bC.TryGetValue(mv(h.I)? h.aM+"."+h.I :(hI!=null&&hI.bw!=null ? hI.bw : h.aM),out gD);
if(a_||bl||gD!=b.mw){b.mw=gD;S=true;Color lT=P;if(lT==Color.Red&&ey)lT=new Color(255,75,75);aO(k,b,gD ?? bf(a8,h),lT);
}}if(b.mq&&(a_||bl)){S=true;var o1=k.f[b.c9];string up=mo(F);string uP=bf(a8,h);
o1.Data=h.ac.Replace("{percent}",up).Replace("{val}",uP);k.f[b.c9]=o1;}}if(S){k.en++;using(var tn=lG.DrawFrame()){
tn.AddRange(k.f);}}return S;}bool rZ(E cm,IMyTextSurface lH){lH.FontSize=cm.c7;var gv=new List<KeyValuePair<string,string>>();
foreach(var o in cm.fX){double U=0.0;string bp;string bj=null;double kK=o.fZ;try{U=a2(o.aM,o.I);if(o.da!=null)kK=a2(o.da,o.ih);
}catch(Exception vT){gv.Add(new KeyValuePair<string,string>($"{o.ac}: [UNRESOLVED]","red"));continue;}double pJ=U;if(kK>0)U=Math.Min(100.0,pJ/kK*100.0);
string qI=bf(pJ,o);if(o.i==e.dX){double oe=(o.ie||o.I=="percent"||o.I=="pct")? U : iy(o.aM);
bp=M.rS(o.ac,oe,10,qI);bj=o.bT ? M.fR(o.aN,oe): o.az;}else if(o.i==e.c4){
bp=M.rQ(o.ac,U,10);bj=o.bT ? M.fR(o.aN,U): o.az;}else if(o.i==e.cF){
bp=M.r1(o.ac,U);bj=o.az ??(U>=50.0 ? "green" : "gray");}else if(o.i==e.cw){bp=M.rV(o.ac,U);
bj=o.az ??(U>=50.0 ? "cyan" : "gray");}else if(o.i==e.cD){bp=M.r0(o.ac,U);bj=o.az ??(U>=75.0 ? "lime" :(U>=25.0 ? "yellow" : "red"));
}else if(o.i==e.c2){bp=M.rP(o.ac,U,8);bj=o.bT ? M.fR(o.aN,U):(o.az ?? "yellow");
}else if(o.i==e.cx){bool tE=(o.I=="producing"||o.I=="active"||o.I=="running"||o.I=="state"||(o.I!="percent"&&(U<=1.0||U==100.0)));
if(tE){char[]vc=new[]{'|','/','-','\\'};char uZ=vc[Math.Abs(ev)%4];bp=U>0 ? $"{o.ac} [{uZ}] RUN" : $"{o.ac} [==] IDLE";
bj=U>0 ? "green" : "gray";}else{bp=M.rW(o.ac,U,ev);bj=o.az ??(U>55.0 ? "green" :(U<45.0 ? "red" : "cyan"));
}}else{bp=$"{o.ac}: {qI}";bj=o.bv!=null ? M.dc(o.bv,U): null;}gv.Add(new KeyValuePair<string,string>(bp,bj));
}var gp=M.rU(new List<string>(gv.ConvertAll(vm=>vm.Key)),dU: cm.mP);string p6=string.Join("\n",gp);
string pC;if(cI.TryGetValue(cm,out pC)&&pC==p6)return false;cI[cm]=p6;using(var to=lH.DrawFrame()){
float qZ=4f;float o3=q_*cm.c7;try{var o6=lH.MeasureStringInPixels(new StringBuilder("A"),"Monospace",cm.c7);
if(o6.Y>0)o3=o6.Y;}catch{}for(int dv=0;dv<gp.Count;dv++){string nL=null;if(dv>0&&dv<gp.Count-1)nL=gv[dv-1].Value;
var qd=MySprite.CreateText(gp[dv],"Monospace",J(nL),cm.c7,TextAlignment.LEFT);qd.Position=new Vector2(4f,qZ);to.Add(qd);
qZ+=o3;}}return true;}Color J(string g5){if(string.IsNullOrEmpty(g5))return Color.White;Color e0;if(mT.TryGetValue(g5,out e0))return e0;
p eE=p.eg(g5);e0=eE.fV ? Color.Transparent : new Color(eE.bX,eE.bS,eE.bN,eE.hS);mT[g5]=e0;return e0;}public enum bc{q3,id,d5}
public enum aL{rA,hU,mK,my,mx}public class bu{public string ag;public bc bU;public List<string>a3=new List<string>();public string mL{
get{return a3.Count>0 ? a3[0]: "";}set{if(a3.Count==0)a3.Add(value);else a3[0]=value;}}public aL dV=aL.rA;
public double fL=0.0;public string h4;public string bw;public bool fC;}public enum e{iD,dX,ee,cB,ea,d1,d4,c4,cF,cw,cD,c2,cx,f3,fM}
public class ay{public string fO;public string bR;public bool c8;}public class bx{public e i;public string ac;public string aM;
public string I;public bool bT;public string[]aN;public string az;public List<ay>bv;public string fN="right";
public string em;public string d6;public List<double>fY;public string fT;public List<ay>h_;public string l8,l9;public bool il;
public int bA;public int iE;public string iq;public int f2;public string d3,h3;public string ec,iH;public double fZ;public string da,ih;
public bool ie{get{return fZ>0||da!=null;}}}public struct p{public byte bX,bS,bN,hS;public bool fV;public p(byte uG,byte tq,byte sx,byte sf=255){
bX=uG;bS=tq;bN=sx;hS=sf;fV=false;}public static readonly p sb=new p(0,0,0,0){fV=true};public static readonly p f4=new p(255,255,255);
public static readonly p q2=new p(0,0,0);public static readonly p rN=new p(255,0,0);public static readonly p rt=new p(0,255,0);
public static readonly p rx=new p(50,205,50);public static readonly p se=new p(255,255,0);public static readonly p rB=new p(255,140,0);
public static readonly p re=new p(0,255,255);public static readonly p q4=new p(0,120,255);public static readonly p ry=new p(255,0,255);
public static readonly p rL=new p(128,0,128);public static readonly p rs=new p(128,128,128);public static readonly p rf=new p(60,60,60);
public static p eg(string bo){if(string.IsNullOrWhiteSpace(bo))return f4;bo=bo.Trim();if(bo.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bo.Equals("none",StringComparison.OrdinalIgnoreCase))return sb;
if(bo.StartsWith("#")){string cd=bo.Substring(1).Trim();if(cd.Length==6){byte uH=Convert.ToByte(cd.Substring(0,2),16);byte tr=Convert.ToByte(cd.Substring(2,2),16);
byte sy=Convert.ToByte(cd.Substring(4,2),16);return new p(uH,tr,sy);}if(cd.Length==3){byte uI=Convert.ToByte(new string(cd[0],2),16);byte ts=Convert.ToByte(new string(cd[1],2),16);
byte sz=Convert.ToByte(new string(cd[2],2),16);return new p(uI,ts,sz);}}if(bo.Contains(",")){var dE=bo.Split(',');if(dE.Length>=3){byte uJ=(byte)Math.Max(0,Math.Min(255,int.Parse(dE[0].Trim())));
byte tt=(byte)Math.Max(0,Math.Min(255,int.Parse(dE[1].Trim())));byte sA=(byte)Math.Max(0,Math.Min(255,int.Parse(dE[2].Trim())));byte sg=dE.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(dE[3].Trim()))):(byte)255;
return new p(uJ,tt,sA,sg);}}switch(bo.ToLowerInvariant()){case "red": return rN;case "green": return rt;case "lime": return rx;case "yellow": return se;
case "orange": return rB;case "cyan": return re;case "blue": return q4;case "magenta": return ry;case "purple": return rL;case "white": return f4;
case "black": return q2;case "gray": case "grey": return rs;case "darkgray": case "darkgrey": return rf;default: return f4;}}}public class bP{
public string[]bQ;public string dY="vertical";public List<ay>a0;public string hW,hX;public List<ay>c6;public string h8,ia;
}public class aF : bP{public string db="default";public int d2=0;public int eh=0;public int ej=1;public int ek=1;public string cE;public string iF="left";
public string iG;public string d_;public string mk;public float? mm;public string c3;public string fH;public string[]fI;
public string ip="horizontal";public List<bx>ap=new List<bx>();}public class E : bP{public string bg;public string cA="standard";
public string c5="graphic";public string d0;public string[]fJ;public string mM="left";public string mN;public string ml="Monospace";
public string bO;public string mc;public int md=10;public int ma=1;public float h1=1.2f;public float ic= -1f,ir= -1f;
public bool fS;public string it;public string hY;public string l7;public string l5;public string ij;public string ii;
public string cs;public string cv;public float h2=2.5f;public string mb="cyan";public string[]fF;public List<string>fG=new List<string>();
public List<string>rq(){if(fG!=null&&fG.Count>0)return fG;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float c7=1.0f;public int iw=0;public float hT=1.6f;public int d8=1;public int d9=1;public int mP=30;public string bV;
public int cz=0;public List<aF>ak=new List<aF>();public List<bx>fX{get{if(ak.Count==0)ak.Add(new aF{db="main"}
);return ak[0].ap;}}}public class b_{public string ik;public List<string>f_=new List<string>();public Dictionary<string,string>h5=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>h0=new List<string>();}public class iv{public List<bu>cu=new List<bu>();public List<E>Z=new List<E>();
public List<string>be=new List<string>();public List<string>eo=new List<string>();public Dictionary<string,List<string>>dW=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class ms{static readonly Dictionary<string,b_>me=new Dictionary<string,b_>(StringComparer.OrdinalIgnoreCase);
static ms(){rO();}static void rO(){bY("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bY("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bY("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bY("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bY("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bY("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bY("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bY("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bY(string pf,string[]un,string sK){var hD=new b_{ik=pf}
;foreach(var ug in un){var kx=ug.Split(new[]{'='},2);string pq=im(kx[0]);hD.f_.Add(pq);if(kx.Length>1)hD.h5[pq]=ae(kx[1].Trim());
}var t2=sK.Replace("\r\n","\n").Split('\n');foreach(var tR in t2){string pS=tR.Trim();if(pS.Length>0)hD.h0.Add(pS);}me[pf]=hD;}static string im(string dB){
dB=dB.Trim();if(dB.StartsWith("$"))dB=dB.Substring(1);return dB.ToLowerInvariant();}public static iv rD(string sZ){var au=new iv();E d=null;
aF u=null;var pB=rJ(sZ,au.be,au.dW);for(int eT=0;eT<pB.Count;eT++){string a=pB[eT].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){au.cu.Add(rE(a));}else if(a.StartsWith("@screen")){d=new E{bg=a.Substring("@screen".Length).Trim()}
;au.Z.Add(d);u=null;}else if(a.StartsWith("profile:")){s(d,"profile:");d.cA=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){s(d,"border:");string nd=a.Substring("border:".Length).Trim();if(u!=null)u.d_=nd;
else d.c5=nd;}else if(a.StartsWith("border_color:")){s(d,"border_color:");string nf=a.Substring("border_color:".Length).Trim();
if(u!=null)u.fH=nf;else d.d0=nf;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
s(d,"border_gradient");string cL=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
cL=cL.Trim();if(cL.StartsWith("(")&&cL.Contains(")"))cL=a1(cL,0);string[]qg=aP(cL).Select(uT=>uT.Trim()).ToArray();if(u!=null)u.fI=qg;
else d.fJ=qg;}else if(a.StartsWith("title_align:")){s(d,"title_align:");string qo=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(u!=null)u.iF=qo;else d.mM=qo;}else if(a.StartsWith("title_color:")){s(d,"title_color:");string qr=a.Substring("title_color:".Length).Trim();
if(u!=null)u.iG=qr;else d.mN=qr;}else if(a.StartsWith("font_size:")){s(d,"font_size:");float ql=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(u!=null)u.mm=ql;else d.c7=ql;}else if(a.StartsWith("font:")){s(d,"font:");string n7=a.Substring("font:".Length).Trim();
if(u!=null)u.mk=n7;else d.ml=n7;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){s(d,"bg:");string dh=a.Substring(a.IndexOf(':')+1).Trim();
bP je=u!=null ?(bP)u : d;if(dh.StartsWith("color(")&&dh.EndsWith(")")){string no;je.a0=mC(dh.Substring(6,dh.Length-7),out no);
string nm,nn;iz(no,out nm,out nn);je.hW=nm;je.hX=nn;}else if(u!=null)u.c3=dh;
else d.bO=dh;}else if(a.StartsWith("layout:")){s(d,"layout:");rF(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
s(d,"alert_period:");d.hT=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){s(d,"refresh:");d.iw=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
s(d,"output:");var ha=aP(a.Substring("output:".Length).Trim());if(ha.Count==0)throw new FormatException("output: requires a target block name");
d.bV=ae(ha[0]);d.cz=ha.Count>1 ? int.Parse(ha[1]): 0;}else if(a.StartsWith("width:")){s(d,"width:");
d.mP=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){s(d,"boot_msg:");
d.mc=ae(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){s(d,"boot_stagger:");d.md=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){s(d,"boot_calibrate:");string nO=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.h1=nO=="off" ? 0f : Math.Max(0f,float.Parse(nO,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){s(d,"boot_batch:");
d.ma=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){s(d,"boot_duration:");
string eG=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool oH=eG.EndsWith("s");if(oH)eG=eG.Substring(0,eG.Length-1).Trim();
float jJ;if(float.TryParse(eG,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out jJ)){d.h2=oH ? jJ : jJ/60f;
}}else if(a.StartsWith("boot_log:")){s(d,"boot_log:");d.fG.Add(ae(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("mirror_panel:")){
s(d,"mirror_panel:");d.ij=ae(a.Substring("mirror_panel:".Length).Trim());}else if(a.StartsWith("mirror:")){s(d,"mirror:");
d.ii=ae(a.Substring("mirror:".Length).Trim());}else if(a.StartsWith("banner:")){s(d,"banner:");d.cs=ae(a.Substring("banner:".Length).Trim());
}else if(a.StartsWith("boot_logo:")){s(d,"boot_logo:");d.cv=ae(a.Substring("boot_logo:".Length).Trim());}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){
s(d,"boot_gradient");d.fF=mD(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){s(d,"bg_gradient");
(u!=null ?(bP)u : d).bQ=mD(a);}else if(a.StartsWith("bg_gradient_dir:")){s(d,"bg_gradient_dir:");
(u!=null ?(bP)u : d).dY=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(a.StartsWith("flash:")){
s(d,"flash:");bP jR=u!=null ?(bP)u : d;string jQ=a.Substring("flash:".Length).Trim();string n9=iC(jQ);
string n8,oa;iz(jQ.Substring(0,jQ.Length-n9.Length),out n8,out oa);jR.h8=n8;jR.ia=oa;jR.c6=new List<ay>{
new ay{fO=n9,bR="on"}};}else if(a.StartsWith("bg_texture:")){s(d,"bg_texture:");d.hY=ae(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){s(d,"bg_texture_color:");d.l7=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gap:")||a.StartsWith("padding:")){bool oL=a.StartsWith("gap:");s(d,oL ? "gap:" : "padding:");float pE=float.Parse(a.Substring(a.IndexOf(':')+1).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(oL)d.ic=pE;else d.ir=pE;}else if(a.StartsWith("gradient_mode:")){s(d,"gradient_mode:");d.fS=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){s(d,"screen_bg:");d.bO=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
s(d,"panel_bg:");d.it=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){s(d,"alert_bg:");
d.l5=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){s(d,"boot_color:");
d.mb=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){s(d,"orientation:");string ud=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(u!=null)u.ip=ud;}else if(a.StartsWith("@panel")){s(d,"@panel");u=rH(a);d.ak.Add(u);
}else if(a.StartsWith("title:")){s(d,"title:");if(u==null){u=new aF{db="main",d2=0,eh=0,ej=d.d8,ek=d.d9}
;d.ak.Add(u);}u.cE=ae(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){s(d,"line:");
if(u==null){u=new aF{db="main",d2=0,eh=0,ej=d.d8,ek=d.d9};d.ak.Add(u);}u.ap.Add(rG(a.Substring("line:".Length).Trim()));
}else{au.be.Add($"Line {eT + 1}: unrecognized directive: \"{a}\"");}}catch(Exception ta){au.be.Add($"Line {eT + 1}: parse error ({ta.Message}) in \"{a}\"");
}}foreach(var pT in au.Z){if(string.IsNullOrEmpty(pT.bV))au.be.Add($"@screen {pT.bg}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var ax in au.Z){int kU=ax.ak.Count(k8=>k8.c3!=null||k8.a0!=null||k8.bQ!=null);if(ax.ak.Count>1&&kU>0&&kU<ax.ak.Count)au.eo.Add("screen '"+ax.bg+"': only "+kU+" of "+ax.ak.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(ax.cs!=null&& !au.dW.ContainsKey(ax.cs))au.be.Add("Screen '"+ax.bg+"': banner '"+ax.cs+"' is not defined (@banner "+ax.cs+" ... @end)");
if(ax.cv!=null&& !au.dW.ContainsKey(ax.cv))au.be.Add("Screen '"+ax.bg+"': boot_logo '"+ax.cv+"' is not defined (@banner "+ax.cv+" ... @end)");
}return au;}static void rF(string jl,E p_){int k0=jl.IndexOf('(');if(k0<0)throw new FormatException("missing '(' in layout: specification");
string oQ=jl.Substring(0,k0).Trim().ToLowerInvariant();string sn=a1(jl,k0);var i4=aP(sn);if(oQ=="grid"){if(i4.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
p_.d8=Math.Max(1,int.Parse(i4[0]));p_.d9=Math.Max(1,int.Parse(i4[1]));}else{throw new FormatException($"unknown layout kind '{oQ}' (expected grid)");
}}static aF rH(string tZ){string fh=tZ.Substring("@panel".Length).Trim();int gi=fh.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(gi<0)throw new FormatException("@panel requires an at(...) positioning clause");
string ua=fh.Substring(0,gi).Trim();string m8=fh.Substring(gi);int pw=m8.IndexOf('(');string m6=a1(m8,pw);
var cJ=aP(m6);if(cJ.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bI=new aF{db=ua,d2=int.Parse(cJ[0]),eh=int.Parse(cJ[1]),ej=cJ.Count>2 ? int.Parse(cJ[2]): 1,ek=cJ.Count>3 ? int.Parse(cJ[3]): 1}
;int m_=gi+pw+m6.Length+1;if(m_<fh.Length){string aW=fh.Substring(m_).Trim();int qw=aW.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(qw>=0){string fp=aW.Substring(qw+5).Trim();if(fp.StartsWith("(")&&fp.Contains(")"))bI.cE=ae(a1(fp,0));else if(fp.StartsWith(":")){
string cM=fp.Substring(1).Trim();if(cM.StartsWith("\"")){int n3=cM.IndexOf('"',1);bI.cE=n3>0 ? cM.Substring(1,n3-1): ae(cM);}else{int qb=cM.IndexOf(' ');
bI.cE=qb>=0 ? cM.Substring(0,qb): cM;}}}string pi=fP(aW,"orientation:");if(pi!=null)bI.ip=pi.ToLowerInvariant();string mY=fP(aW,"title_align:");
if(mY!=null)bI.iF=mY.ToLowerInvariant();string qt=fP(aW,"title_color:");if(qt!=null)bI.iG=qt;int go=aW.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(go>=0&& !aW.Substring(go).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aW.Substring(go).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bI.d_=fP(aW.Substring(go),"border:");}int gl=aW.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(gl<0)gl=aW.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(gl>=0){string cb=aW.Substring(gl+"border_gradient".Length).Trim();if(cb.StartsWith(":"))cb=cb.Substring(1).Trim();if(cb.StartsWith("(")&&cb.Contains(")"))cb=a1(cb,0);
bI.fI=aP(cb).Select(uU=>uU.Trim()).ToArray();}}return bI;}static string fP(string pU,string oP){int oG=pU.IndexOf(oP,StringComparison.OrdinalIgnoreCase);
if(oG<0)return null;string lX=pU.Substring(oG+oP.Length).Trim();int qc=lX.IndexOf(' ');return qc>=0 ? lX.Substring(0,qc): lX;}static string[]mD(string kD){
int j2=kD.IndexOfAny(new[]{':','('});string eO=kD.Substring(kD[j2]==':' ? j2+1 : j2).Trim();if(eO.StartsWith("(")&&eO.Contains(")"))eO=a1(eO,0);return aP(eO).Select(uV=>uV.Trim()).ToArray();
}static List<string>rJ(string s_,List<string>jO,Dictionary<string,List<string>>sC){var qD=new Dictionary<string,b_>(StringComparer.OrdinalIgnoreCase);
var dI=(s_ ?? "").Replace("\r\n","\n").Split('\n');var kT=new List<string>();int dw=0;while(dw<dI.Length){string ld=dI[dw++];if(ld.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ar=new List<string>();while(dw<dI.Length){string mW=dI[dw++].Replace("\t","    ").TrimEnd();if(mW.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ar.Add(mW);}while(ar.Count>0&&ar[0].Length==0)ar.RemoveAt(0);while(ar.Count>0&&ar[ar.Count-1].Length==0)ar.RemoveAt(ar.Count-1);int km=int.MaxValue;
foreach(var iW in ar)if(iW.Length>0)km=Math.Min(km,iW.Length-iW.TrimStart().Length);for(int e1=0;e1<ar.Count;e1++)if(ar[e1].Length>0)ar[e1]=ar[e1].Substring(km);
sC[ld.TrimStart().Substring(8).Trim()]=ar;continue;}string cg=mJ(ld).Trim();if(cg.Length==0)continue;if(cg.StartsWith("@template")){try{
var lL=rI(cg);while(dw<dI.Length){string sM=dI[dw++];string jm=mJ(sM).Trim();if(jm.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(jm.Length>0)lL.h0.Add(jm);}qD[lL.ik]=lL;}catch(Exception tb){jO.Add($"Template parse error ({tb.Message}) in \"{cg}\"");}}
else if(cg.StartsWith("@use")){try{mh(cg,qD,kT,jO);}catch(Exception tc){jO.Add($"@use error ({tc.Message}) in \"{cg}\"");
}}else{kT.Add(cg);}}return kT;}static b_ rI(string t_){string le=t_.Substring("@template".Length).Trim();int k1=le.IndexOf('(');
if(k1<0)throw new FormatException("missing '(' in @template header");string ub=le.Substring(0,k1).Trim();string so=a1(le,k1);var lM=new b_{
ik=ub};foreach(var uh in aP(so)){var ky=uh.Split(new[]{'='},2);string pr=im(ky[0]);lM.f_.Add(pr);if(ky.Length>1)lM.h5[pr]=ae(ky[1].Trim());
}return lM;}static void mh(string t0,Dictionary<string,b_>qE,List<string>pn,List<string>s9,int nS=0){string hl=t0.Substring("@use".Length).Trim();
int g8=hl.IndexOf('(');string kL=g8>=0 ? hl.Substring(0,g8).Trim(): hl.Trim();string sp=g8>=0 ? a1(hl,g8): "";b_ dR=null;
if(!qE.TryGetValue(kL,out dR)){if(!me.TryGetValue(kL,out dR))throw new FormatException($"unknown template '{kL}'");}var gr=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var oU in dR.h5)gr[oU.Key]=oU.Value;var m5=aP(sp);for(int ex=0;ex<m5.Count;ex++){string gg=m5[ex];int jM=gg.IndexOf('=');if(jM>=0){
string tJ=im(gg.Substring(0,jM));string vz=ae(gg.Substring(jM+1).Trim());gr[tJ]=vz;}else{if(ex<dR.f_.Count){string tK=dR.f_[ex];
gr[tK]=ae(gg.Trim());}}}foreach(var sL in dR.h0){string eK=sL;foreach(var g0 in gr){eK=eK.Replace("$"+g0.Key,g0.Value).Replace("{"+g0.Key+"}",g0.Value);
}if(eK.StartsWith("@use")){if(nS>3)throw new FormatException("@use nested too deep");mh(eK,qE,pn,s9,nS+1);
}else pn.Add(eK);}}static void s(E sY,string s6){if(sY==null)throw new FormatException($"'{s6}' used before any @screen declared");
}static string mJ(string bm){if(string.IsNullOrEmpty(bm))return bm;bool kl=false;for(int ce=0;ce<bm.Length;ce++){char jr=bm[ce];if(jr=='"'){kl= !kl;
}else if(!kl){if(jr=='/'&&ce+1<bm.Length&&bm[ce+1]=='/'){return bm.Substring(0,ce);}if(jr=='#'){if(rv(bm,ce)){continue;}return bm.Substring(0,ce);}}}return bm;
}static bool rv(string cS,int oJ){int dz=0;for(int kh=oJ+1;kh<cS.Length;kh++){char dl=cS[kh];if((dl>='0'&&dl<='9')||(dl>='a'&&dl<='f')||(dl>='A'&&dl<='F'))dz++;
else break;}if(dz==3||dz==4||dz==6||dz==8){int e4=oJ+1+dz;if(e4>=cS.Length||char.IsWhiteSpace(cS[e4])||cS[e4]==')'||cS[e4]==','||cS[e4]==':')return true;
}return false;}static bu rE(string t1){string lf=t1.Substring("@bind".Length).Trim();int jN=lf.IndexOf('=');if(jN<0)throw new FormatException("missing '=' in @bind");
string pg=lf.Substring(0,jN).Trim();string jP=lf.Substring(jN+1).Trim();int k2=jP.IndexOf('(');if(k2<0)throw new FormatException("missing '(' in @bind target");
string gL=jP.Substring(0,k2).Trim().ToLowerInvariant();string sq=a1(jP,k2);var b5=aP(sq);if(b5.Count==0)throw new FormatException("@bind target has no arguments");
var av=new bu{ag=pg};if(gL=="dynamic"||gL=="var"||gL=="mem"){av.bU=bc.d5;av.bw=b5.Count>0 ? ae(b5[0].Trim()): pg;
for(int iX=0;iX<b5.Count;iX++){var g1=b5[iX].Split(new[]{':'},2);if(g1.Length==2&&g1[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double nR;
if(double.TryParse(g1[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nR))av.fL=nR;else av.h4=ae(g1[1].Trim());
}}return av;}av.bU=gL=="group" ? bc.id : bc.q3;string gf=null;for(int ge=0;ge<b5.Count;ge++){var dy=b5[ge].Split(new[]{':'},2);if(dy.Length==2&&dy[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
gf=dy[1].Trim().ToLowerInvariant();}else if(dy.Length==2&&dy[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){av.fC=dy[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{av.a3.Add(ae(b5[ge].Trim()));}}if(av.a3.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(av.bU==bc.id){if(gf==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(gf){case "avg": av.dV=aL.hU;break;case "sum": av.dV=aL.mK;break;case "min": av.dV=aL.my;break;case "max": av.dV=aL.mx;
break;default: throw new FormatException($"unknown agg '{gf}' (expected avg/sum/min/max)");}}return av;}static bx rG(string gn){var l=new bx();
if(!gn.StartsWith("\""))throw new FormatException("line must start with a quoted label");int jL=gn.IndexOf('"',1);if(jL<0)throw new FormatException("unterminated quoted label");
l.ac=gn.Substring(1,jL-1);string fg=gn.Substring(jL+1).Trim();int g9=fg.IndexOf('(');if(g9<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string y=fg.Substring(0,g9).Trim().ToLowerInvariant();string sm=a1(fg,g9);string ai=fg.Substring(fg.IndexOf(')',g9)+1).Trim();
var jH=sm.Trim().Split('.');if(jH.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
l.aM=jH[0].Trim();l.I=jH[1].Trim();if(y=="bar")l.i=e.dX;else if(y=="led")l.i=e.ee;else if(y=="spark"||y=="sparkline")l.i=e.cB;
else if(y=="hazard"||y=="stripes")l.i=e.ea;else if(y=="chevron"||y=="chevrons")l.i=e.d1;else if(y=="dots"||y=="dot")l.i=e.d4;
else if(y=="balance"||y=="horizon"||y=="centered")l.i=e.c4;else if(y=="toggle"||y=="switch")l.i=e.cF;else if(y=="checkbox"||y=="check")l.i=e.cw;
else if(y=="status"||y=="badge"||y=="state")l.i=e.cD;else if(y=="ammo"||y=="pips")l.i=e.c2;else if(y=="flow"||y=="rate"||y=="spinner")l.i=e.cx;
else if(y=="text")l.i=e.iD;else if(y=="tank"||y=="tube"||y=="bucket")l.i=e.f3;else if(y=="dial"||y=="gauge"||y=="arc")l.i=e.fM;
else throw new FormatException($"unknown display function '{y}'");while(ai.Length>0){int jE=ai.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int dt=ai.IndexOf('(');if(jE>=0&&(dt<0||jE<dt)){string gy=ai.Substring(jE+4).Trim();int hu=gy.IndexOf(' ');l.fN=(hu>=0 ? gy.Substring(0,hu): gy).ToLowerInvariant();
ai=(hu>=0 ? gy.Substring(hu): "").Trim();continue;}int lV=ai.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(lV>=0&&(dt<0||lV<dt)){
string hO=ai.Substring(lV+5).Trim();int hv=hO.IndexOf(' ');l.em=(hv>=0 ? hO.Substring(0,hv): hO).Trim().Trim('"');ai=(hv>=0 ? hO.Substring(hv): "").Trim();
continue;}int jY=ai.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(jY>=0&&(dt<0||jY<dt)){string gF=ai.Substring(jY+7).Trim();
int hw=gF.IndexOf(' ');l.d6=(hw>=0 ? gF.Substring(0,hw): gF).Trim().Trim('"');ai=(hw>=0 ? gF.Substring(hw): "").Trim();continue;}int g4=ai.IndexOf('(');
if(g4<0)break;string N=ai.Substring(0,g4).Trim().ToLowerInvariant();int o0=N.LastIndexOf(' ');if(o0>=0)N=N.Substring(o0+1).Trim();
string _=a1(ai,g4);ai=ai.Substring(ai.IndexOf(')',g4)+1).Trim();if(N=="gradient"){l.bT=true;l.aN=aP(_).Select(uW=>uW.Trim()).ToArray();
if(l.aN.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(N=="size"){string lK=_.Trim().ToLowerInvariant();
l.f2=lK=="large"||lK=="fill" ? 2 : lK=="medium" ? 1 : 0;}else if(N=="marker"){l.fY=new List<double>();foreach(var t7 in aP(_)){double pb;
if(!double.TryParse(t7,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out pb))throw new FormatException("marker() takes numbers from 0 to 100");
l.fY.Add(pb);}}else if(N=="icon")l.fT=_.Trim();else if(N=="blink"){string jp=_.Trim(),ng=iC(jp);iz(jp.Substring(0,jp.Length-ng.Length),out l.l8,out l.l9);
l.h_=new List<ay>{new ay{fO=ng,bR="on"}};}else if(N=="value")l.il=_.Trim().Equals("off",StringComparison.OrdinalIgnoreCase);
else if(N=="ticks")l.iE=int.Parse(_.Trim());else if(N=="segments"||N=="points")l.bA=Math.Max(2,Math.Min(40,int.Parse(_.Trim())));else if(N=="outline")l.iq=_.Trim();
else if(N=="color_by"){var jv=_.Trim().Split('.');if(jv.Length!=2)throw new FormatException("color_by() takes bindname.field");l.d3=jv[0].Trim();
l.h3=jv[1].Trim();}else if(N=="label_color")l.ec=_.Trim();else if(N=="track")l.iH=_.Trim();else if(N=="flat"){l.az=_.Trim();
}else if(N=="pattern"){string W=_.Trim().ToLowerInvariant();if(W=="hazard"||W=="stripes")l.i=e.ea;else if(W=="chevron"||W=="chevrons")l.i=e.d1;
else if(W=="dots"||W=="dot")l.i=e.d4;else if(W=="led")l.i=e.ee;else if(W=="balance"||W=="horizon")l.i=e.c4;
else if(W=="toggle"||W=="switch")l.i=e.cF;else if(W=="checkbox"||W=="check")l.i=e.cw;else if(W=="status"||W=="badge"||W=="state")l.i=e.cD;
else if(W=="ammo"||W=="pips")l.i=e.c2;else if(W=="flow"||W=="rate"||W=="spinner")l.i=e.cx;}else if(N=="color"){string tC;
l.bv=mC(_,out tC);}else if(N=="dir"||N=="direction"){l.fN=_.Trim().ToLowerInvariant();}else if(N=="unit"){
l.em=_.Trim().Trim('"');}else if(N=="max"){double pa;if(double.TryParse(_.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out pa))l.fZ=pa;
else{var kI=_.Trim().Split('.');if(kI.Length!=2)throw new FormatException("max() takes a number or bindname.field");l.da=kI[0].Trim();l.ih=kI[1].Trim();
}}else if(N=="format"||N=="fmt"){l.d6=_.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{N}'");}}return l;}static string a1(string hq,int kR){
int jD=0;for(int eU=kR;eU<hq.Length;eU++){if(hq[eU]=='(')jD++;else if(hq[eU]==')'){jD--;if(jD==0)return hq.Substring(kR+1,eU-kR-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aP(string sr){return sr.Split(',').Select(uX=>uX.Trim()).Where(uY=>uY.Length>0).ToList();
}static List<ay>mC(string sl,out string jT){var ho=new List<ay>();jT=null;foreach(var k3 in aP(sl)){int jz=k3.LastIndexOf(':');
if(jz<0)throw new FormatException("color() entries need 'expr : color'");string g2=k3.Substring(0,jz).Trim();string nK=k3.Substring(jz+1).Trim();
if(g2.Equals("default",StringComparison.OrdinalIgnoreCase)){ho.Add(new ay{c8=true,bR=nK});}else{string qh=iC(g2);
if(jT==null)jT=g2.Substring(0,g2.Length-qh.Length).Trim();ho.Add(new ay{fO=qh,bR=nK});}}if(!ho.Any(uK=>uK.c8))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return ho;}static void iz(string uL,out string sI,out string tf){var e7=(uL ?? "").Split('.');if(e7.Length!=2||e7[0].Trim().Length==0||e7[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
sI=e7[0].Trim();tf=e7[1].Trim();}static string iC(string ds){ds=ds.Trim();foreach(var ue in new[]{">=","<=","==",">","<"}){int ki=ds.IndexOf(ue,StringComparison.Ordinal);
if(ki>0)return ds.Substring(ki).Trim();if(ki==0)return ds;}throw new FormatException($"color() rule has no recognizable comparison operator: '{ds}'");}
static string ae(string cT){if(cT.Length>=2&&cT[0]=='"'&&cT[cT.Length-1]=='"')return cT.Substring(1,cT.Length-2);return cT;}}public static class q1{public static double q0(aL sh,List<double>dT){
if(dT==null||dT.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(sh){case aL.hU: return dT.Average();case aL.mK: return dT.Sum();
case aL.my: return dT.Min();case aL.mx: return dT.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class M{
public static string rS(string tT,double hb,int ne=10,string qM=null){hb=Math.Max(0,Math.Min(100,hb));int of=(int)Math.Round(ne*(hb/100.0));
string sD=new string('#',of)+new string('-',ne-of);string vA=qM!=null ? qM : $"{hb:0}%";return $"{tT} [{sD}] {vA}";
}public static string mn(double cr,string lU=null,string gK=null){if(lU=="auto"){double iY=Math.Abs(cr);if(iY>=1e9)return(cr/1e9).ToString("F1")+"G";
if(iY>=1e6)return(cr/1e6).ToString("F1")+"M";if(iY>=1e3)return(cr/1e3).ToString("F1")+"k";string td= !string.IsNullOrEmpty(gK)? gK : "F0";return cr.ToString(td);
}string oh= !string.IsNullOrEmpty(gK)? gK :(Math.Abs(cr)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(lU)){return cr.ToString(oh);}return cr.ToString(oh)+lU;
}public static float rr(string uA,float qj,float lI,float gB=1.0f){string dC=(uA ?? "standard").ToLowerInvariant();
if(dC=="compact"||(dC=="auto"&&qj<=300&&lI<=300&&lI>160)){return gB*0.75f;}if(dC=="corner"||(dC=="auto"&&lI<=160)){
return gB*0.55f;}if(dC=="wide"||(dC=="auto"&&qj>=1000)){return gB*1.15f;}return gB;}public static string rQ(string tU,double hc,int cc=5){
hc=Math.Max(0,Math.Min(100,hc));double gC=(hc-50.0)/50.0;int e2=0;int fj=0;if(gC< -0.01)e2=(int)Math.Round(-gC*cc);
else if(gC>0.01)fj=(int)Math.Round(gC*cc);if(e2>cc)e2=cc;if(fj>cc)fj=cc;string tX=new string('-',cc-e2)+new string('<',e2);
string uR=new string('>',fj)+new string('-',cc-fj);return $"{tU} [{tX}|{uR}] {hc:0}%";}public static string r1(string oY,double vC){
return vC>=50.0 ? $"{oY} [====O] ON" : $"{oY} [O====] OFF";}public static string rV(string oZ,double vD){return vD>=50.0 ? $"{oZ} [X] ACTIVE" : $"{oZ} [ ] READY ";
}public static string r0(string kA,double qK){if(qK>=75.0)return $"{kA} [ ONLINE  ]";if(qK>=25.0)return $"{kA} [ STANDBY ]";return $"{kA} [ OFFLINE ]";
}public static string rP(string tV,double hd,int o5=8){hd=Math.Max(0,Math.Min(100,hd));int og=(int)Math.Round(o5*(hd/100.0));
string uu=new string('|',og)+new string('.',o5-og);return $"{tV} [{uu}] {hd:0}%";}public static string rW(string kB,double fb,int qv=0){
char[]vb=new[]{'|','/','-','\\'};char pV=vb[(qv>=0 ? qv : 0)%4];if(fb>55.0)return $"{kB} {pV} ▲ CHG {fb:0}%";if(fb<45.0)return $"{kB} {pV} ▼ DSC {fb:0}%";
return $"{kB} ● STBY {fb:0}%";}public static bool rz(string aB,string eZ){if(aB==null||eZ==null)return false;if(aB=="*"||aB==eZ)return true;
if(!aB.Contains("*")&& !aB.Contains("?"))return string.Equals(aB,eZ,StringComparison.OrdinalIgnoreCase);int aI=0,eV=0,lz= -1,kH=0;while(eV<eZ.Length){
if(aI<aB.Length&&(aB[aI]=='?'||char.ToLowerInvariant(aB[aI])==char.ToLowerInvariant(eZ[eV]))){aI++;eV++;}else if(aI<aB.Length&&aB[aI]=='*'){
lz=aI;kH=eV;aI++;}else if(lz!= -1){aI=lz+1;kH++;eV=kH;}else{return false;}}while(aI<aB.Length&&aB[aI]=='*')aI++;return aI==aB.Length;
}public static int q5(double ui,int qA){return Math.Max(0,Math.Min(qA,1+(int)(Math.Max(0.0,ui)*qA)));}public static string fR(string[]dQ,double k4){
if(dQ==null||dQ.Length==0)return "white";k4=Math.Max(0,Math.Min(100,k4));int kj=(int)Math.Floor(k4/100.0*dQ.Length);if(kj>=dQ.Length)kj=dQ.Length-1;
return dQ[kj];}public static p mp(string[]cp,double uq){if(cp==null||cp.Length==0)return p.f4;if(cp.Length==1)return p.eg(cp[0]);
double uO=Math.Max(0.0,Math.Min(1.0,uq/100.0));double pZ=uO*(cp.Length-1);int kk=(int)Math.Floor(pZ);int tB=Math.Min(cp.Length-1,kk+1);
double lD=pZ-kk;p dm=p.eg(cp[kk]);p js=p.eg(cp[tB]);byte uM=(byte)Math.Round(dm.bX+(js.bX-dm.bX)*lD);
byte tu=(byte)Math.Round(dm.bS+(js.bS-dm.bS)*lD);byte sB=(byte)Math.Round(dm.bN+(js.bN-dm.bN)*lD);return new p(uM,tu,sB);}public static string dc(List<ay>pR,double vE){
foreach(var lg in pR){if(lg.c8)continue;if(rn(lg.fO,vE))return lg.bR;}var nQ=pR.FirstOrDefault(uN=>uN.c8);return nQ!=null ? nQ.bR : null;
}static bool rn(string eL,double fv){eL=eL.Trim();foreach(var kQ in new[]{">=","<=","==",">","<"}){if(eL.StartsWith(kQ)){double fi=double.Parse(eL.Substring(kQ.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(kQ){case ">=": return fv>=fi;case "<=": return fv<=fi;case "==": return Math.Abs(fv-fi)<1e-9;case ">": return fv>fi;case "<": return fv<fi;
}}}throw new FormatException($"unsupported comparison expression: '{eL}'");}public static List<string>rU(List<string>sX,int dU){
var g6=new List<string>();g6.Add("+"+new string('-',dU+2)+"+");foreach(var kE in sX){string vu=kE.Length>dU ? kE.Substring(0,dU): kE;
string ul=vu.PadRight(dU);g6.Add("| "+ul+" |");}g6.Add("+"+new string('-',dU+2)+"+");return g6;}public struct mB{public float mR,mS,mQ,mr;
}public static mB rb(int nG,int pN,int hx,int hy,int fr,int fs,float vl,float vk,float ps,float pt,float j4,float j5){
fr=Math.Max(1,fr);fs=Math.Max(1,fs);hx=Math.Max(1,Math.Min(fr-nG,hx));hy=Math.Max(1,Math.Min(fs-pN,hy));
float su=vl-(ps*2f);float st=vk-(pt*2f);float nB=(su-(j4*(fr-1)))/fr;float nA=(st-(j5*(fs-1)))/fs;
float uC=ps+nG*(nB+j4);float uD=pt+pN*(nA+j5);float uB=hx*nB+((hx-1)*j4);float us=hy*nA+((hy-1)*j5);return new mB{
mR=uC,mS=uD,mQ=uB,mr=us};}public struct mG{public int rC;public int ig;public int ah;public int aE;public int el;
public int bd;public int r9;public int sa;public int rk;public int rl;
public double rm;}public static mG rc(E cn){var n=new mG();var pv=cn.ak!=null&&cn.ak.Count>0 ? cn.ak : new List<aF>{
new aF{ap=cn.fX}};n.rC=pv.Count;n.ig=0;n.ah=0;n.aE=0;if(!string.IsNullOrEmpty(cn.bO)&& !cn.bO.Equals("none",StringComparison.OrdinalIgnoreCase))n.ah++;
if(!string.IsNullOrEmpty(cn.c5)&& !cn.c5.Equals("none",StringComparison.OrdinalIgnoreCase))n.ah+=2;foreach(var e8 in pv){
if(!string.IsNullOrEmpty(e8.c3)&& !e8.c3.Equals("none",StringComparison.OrdinalIgnoreCase))n.ah++;if(!string.IsNullOrEmpty(e8.cE))n.ah+=2;
n.ig+=e8.ap.Count;foreach(var tS in e8.ap){n.r9++;n.sa++;n.ah+=2;switch(tS.i){case e.dX: n.el++;
n.bd++;n.ah+=2;n.aE++;break;case e.ee: n.bd+=10;n.ah+=10;
n.aE+=10;break;case e.ea: n.bd+=10;n.ah+=11;n.aE+=10;break;case e.d1: n.bd+=10;
n.ah+=10;n.aE+=10;break;case e.d4: n.bd+=12;n.ah+=12;n.aE+=12;
break;case e.cB: n.el++;n.bd+=8;n.ah+=9;n.aE+=8;break;case e.c4: n.el+=2;
n.bd++;n.ah+=3;n.aE++;break;case e.cF: case e.cw: n.el++;
n.bd++;n.ah+=2;n.aE++;break;case e.cD: case e.cx: n.el++;n.bd+=2;
n.ah+=3;n.aE+=2;break;case e.c2: n.bd+=8;n.ah+=8;n.aE+=8;
break;default: n.aE++;break;}}}n.rk=25+(n.ig*2);n.rl=180+(n.aE*4);
n.rm=0.0;return n;}}