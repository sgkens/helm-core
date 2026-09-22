iy t;string iS=null;Dictionary<string,List<IMyTerminalBlock>>b0=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>ga=new Dictionary<string,bool>();
Dictionary<string,bu>er=new Dictionary<string,bu>();Dictionary<string,Color>mX=new Dictionary<string,Color>();List<IMyTerminalBlock>cG=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>de=new List<IMyTerminalBlock>();bool f7;int iP;string es="";StringBuilder ev=new StringBuilder();readonly Dictionary<string,double>aQ=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>bC=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener et;bool cH=false;
Dictionary<E,IMyTextSurface>b1=new Dictionary<E,IMyTextSurface>();Dictionary<E,string>cI=new Dictionary<E,string>();
static readonly string[]mE=new string[101];static Program(){for(int gR=0;gR<=100;gR++){mE[gR]=gR+"%";}}static string ms(double vH){int eZ=(int)Math.Round(vH);
if(eZ<0)eZ=0;else if(eZ>100)eZ=100;return mE[eZ];}class aA{public List<MySprite>f=new List<MySprite>();public List<cy>ek=new List<cy>();
public int ct= -1;public Color[]fF;public Color fC;public bool fD;public List<ix>bW=new List<ix>();public Vector2 ef,ir;
public int ep;public bool ed=false;public List<fG>d0=new List<fG>();}class ix{public string mD;public int iD;public Vector2 mI,mM;
}class fG{public int fW;public Color hY,fM;public List<ay>f3,fS;public string h1,h9,ia,ic;public bool my,ie;
}class cy{public bx eh;public int ab= -1;public int a4= -1;public int c9= -1;public bool mu=false;
public List<int>D=new List<int>();public float[]cC=new float[8];public float bb;public float aD;public float ba;
public float bt;public double fY= -99999.0;public string mA;public float C= -1f;public bool mS;public float mj=12f;}Dictionary<E,aA>b2=new Dictionary<E,aA>();
int mZ=0;double iW=0;double ew=0;double T=0;int eu=0;Dictionary<E,double>bB=new Dictionary<E,double>();
Dictionary<E,double>iO=new Dictionary<E,double>();Dictionary<string,int>ge=new Dictionary<string,int>();List<string>df=new List<string>();
Dictionary<string,int>gf=new Dictionary<string,int>();double iY,iV;HashSet<string>gc=new HashSet<string>();Dictionary<string,double[]>iU=new Dictionary<string,double[]>();
Dictionary<string,double>ey=new Dictionary<string,double>();List<MyInventoryItem>iR=new List<MyInventoryItem>();int b3= -1,gd;double iX;
Dictionary<E,double>f8=new Dictionary<E,double>();int ex=0;const string rz="0.38.3";IMyTextSurface a5;bool iN=true;
double f9=0;string mY="";string iT="";int gb=0,iQ=0;List<KeyValuePair<string,int>>dd=new List<KeyValuePair<string,int>>();
const double rR=120;const float q4=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{et=IGC.RegisterBroadcastListener("HELM");
et.SetMessageCallback("HELM_IGC");}catch{}f2();iE();mx();}public void Main(string nb,UpdateType vD){
double n3=Runtime.TimeSinceLastRun.TotalSeconds;T+=n3;if((vD&UpdateType.IGC)!=0||nb=="HELM_IGC"){rP();}string pO=(nb ?? "").Trim();
string am=pO.ToLowerInvariant();if(am=="reload"){f2();}else if(am=="refresh"){iA();}else if(am=="boot"){iE();}else if(am=="probe"){
ew=T+20.0;ro();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(am=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=sd;
f2();iE();}else if(am=="console on"||am=="console off"){iN=am=="console on";mx();f9=0;}else if(am=="names"){if(Me.CustomData.Contains("# --- names")){
Echo("HELM: the names are already at the end of Custom Data. Delete that block to list them again.");return;}var e6=new List<string>();foreach(var uh in dX())e6.Add(uh.CustomName);
var pk=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(pk);foreach(var ui in pk)e6.Add("[group] "+ui.Name);e6.Sort();Me.CustomData+="\n# --- names (delete when done) ---\n# "+string.Join("\n# ",e6);
Echo("HELM: "+e6.Count+" names written to the end of Custom Data.");return;}else if(am=="scan"||am=="diagnose"||am=="test"){r9();return;}else if(am.StartsWith("set ")){
string hn=pO.Substring(4).Trim();int n7=hn.IndexOf('=');int ve=hn.IndexOf(' ');int lv=n7>0 ? n7 : ve;if(lv>0){string oQ=hn.Substring(0,lv).Trim();
string qK=hn.Substring(lv+1).Trim();ma($"{oQ}={qK}");Echo($"[HELM] Set dynamic variable '{oQ}' = {qK}");}return;}else if(am.StartsWith("alert")){
string fw=am.Length>5 ? am.Substring(5).Trim(): "toggle";if(fw=="toggle")cH= !cH;else cH=fw=="red"||fw=="on"||fw=="1"||fw=="true";
Echo($"[HELM] Red alert state: {cH}");return;}else if(Me.CustomData!=iS){f2();}ex++;iW+=n3;if(iW>=rR){
iW=0;iA();}if(ew>0){if(T<ew){iM();return;}ew=0;b2.Clear();cI.Clear();
f9=0;}if(t==null||t.Z.Count==0){iM();mk();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cU=t.Z[mZ%t.Z.Count];
mZ++;iT=cU.bg;bool i1=bB.Count>0;if(i1)rY();double n4;if(!bB.ContainsKey(cU)&& !(iO.TryGetValue(cU,out n4)&&T<n4)){
if(r2(cU))i1=true;if(cU.iz>0)iO[cU]=T+cU.iz/60.0;}eu=i1 ? 0 : eu+1;Runtime.UpdateFrequency=eu<=t.Z.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
sc();sb();sa();iM();mk();gb=Runtime.CurrentInstructionCount;if(gb>iQ)iQ=gb;}void mk(){
if(es.Length>0)Echo(es);if(ev.Length>0)Echo(ev.ToString());}List<IMyTerminalBlock>dX(bool sn=false){if(!f7){de.Clear();
GridTerminalSystem.GetBlocks(de);cG.Clear();cG.AddRange(de);cG.RemoveAll(sA=> !sA.IsSameConstructAs(Me));iP=de.Count-cG.Count;
f7=true;}return sn ? de : cG;}IMyTerminalBlock mn(string pg,bool sp=false){var jb=GridTerminalSystem.GetBlockWithName(pg);
if(jb==null||sp||jb.IsSameConstructAs(Me))return jb;var i3=dX();for(int gS=0;gS<i3.Count;gS++)if(i3[gS].CustomName.IndexOf(pg,StringComparison.OrdinalIgnoreCase)>=0)return i3[gS];
return null;}void rf(string ph,List<IMyTerminalBlock>e1,bool m4=false){var oq=GridTerminalSystem.GetBlockGroupWithName(ph);if(oq==null)return;int nm=e1.Count;
var ft=new List<IMyTerminalBlock>();oq.GetBlocks(ft);l8(ft,e1,m4);if(e1.Count>nm)return;var oC=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(oC);
foreach(var qZ in oC){if(qZ.Name.IndexOf(ph,StringComparison.OrdinalIgnoreCase)<0)continue;ft.Clear();qZ.GetBlocks(ft);l8(ft,e1,m4);if(e1.Count>nm)return;
}}void l8(List<IMyTerminalBlock>tu,List<IMyTerminalBlock>oN,bool so=false){foreach(var jc in tu)if((so||jc.IsSameConstructAs(Me))&& !oN.Contains(jc))oN.Add(jc);
}void f2(){iS=Me.CustomData;t=mw.rI(iS);es="";if(t.be.Count>0){var p_=new StringBuilder("HELM config errors:\n");
foreach(var td in t.be)p_.AppendLine(" - "+td);es=p_.ToString();}if(t.eq.Count>0){var qX=new StringBuilder("HELM notes:\n");foreach(var vM in t.eq)qX.AppendLine(" - "+vM);
es+=qX.ToString();}er.Clear();foreach(var jd in t.cu)if(!er.ContainsKey(jd.ag))er[jd.ag]=jd;b2.Clear();cI.Clear();
bB.Clear();iO.Clear();f8.Clear();ru();iA();}void rP(){try{while(et!=null&&et.HasPendingMessage){
var pb=et.AcceptMessage();if(pb.Data is string){ma((string)pb.Data);}}}catch{}}void ma(string pB){
if(string.IsNullOrEmpty(pB))return;var py=pB.Split(';');for(int kd=0;kd<py.Length;kd++){var fd=py[kd].Trim();if(fd.Length==0)continue;int eL=fd.IndexOf('=');
if(eL<0)eL=fd.IndexOf(':');if(eL>0){string dy=fd.Substring(0,eL).Trim();string br=fd.Substring(eL+1).Trim();if(dy.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cH=br.Equals("red",StringComparison.OrdinalIgnoreCase)||br=="1"||br.Equals("true",StringComparison.OrdinalIgnoreCase)||br.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double pl;bC.Remove(dy);if(double.TryParse(br,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out pl)){
aQ[dy]=pl;}else if(br.Equals("true",StringComparison.OrdinalIgnoreCase)||br.Equals("on",StringComparison.OrdinalIgnoreCase)){aQ[dy]=100.0;
}else if(br.Equals("false",StringComparison.OrdinalIgnoreCase)||br.Equals("off",StringComparison.OrdinalIgnoreCase)){aQ[dy]=0.0;}else{bC[dy]=br;
}}}}}const string sd="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void ro(){var i4=new List<IMyTextSurface>();if(a5!=null)i4.Add(a5);foreach(var uj in b1.Values)i4.Add(uj);foreach(var u9 in i4)rn(u9);
}static void bz(List<MySprite>vf,float s6,float s7,float vN,float tB,Color sU){vf.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(s6,s7),Size=new Vector2(vN,tB),Color=sU}
);}void rn(IMyTextSurface lw){Vector2 aX=lw.SurfaceSize,aK=lw.TextureSize,ap=(aK-aX)/2f;var aJ=new List<MySprite>();Color gO=new Color(150,150,150);
bz(aJ,aK.X/2f,2f,aK.X,4f,gO);bz(aJ,aK.X/2f,aK.Y-2f,aK.X,4f,gO);bz(aJ,2f,aK.Y/2f,4f,aK.Y,gO);bz(aJ,aK.X-2f,aK.Y/2f,4f,aK.Y,gO);
bz(aJ,ap.X+aX.X/2f,ap.Y+aX.Y/2f,aX.X,aX.Y,new Color(20,40,90));float ao=32f;bz(aJ,ap.X+ao/2f,ap.Y+ao/2f,ao,ao,new Color(255,0,0));bz(aJ,ap.X+aX.X-ao/2f,ap.Y+ao/2f,ao,ao,new Color(0,255,0));
bz(aJ,ap.X+ao/2f,ap.Y+aX.Y-ao/2f,ao,ao,new Color(255,255,0));bz(aJ,ap.X+aX.X-ao/2f,ap.Y+aX.Y-ao/2f,ao,ao,new Color(255,0,255));float l6=ap.Y+aX.Y/2f-30f;
a9(aJ,"SURFACE "+aX.X.ToString("0")+"x"+aX.Y.ToString("0"),Color.White,ap.X+44f,l6,0.9f);a9(aJ,"TEXTURE "+aK.X.ToString("0")+"x"+aK.Y.ToString("0"),Color.White,ap.X+44f,l6+24f,0.9f);
a9(aJ,"OFFSET  "+ap.X.ToString("0")+","+ap.Y.ToString("0"),Color.White,ap.X+44f,l6+48f,0.9f);using(var tn=lw.DrawFrame()){tn.AddRange(aJ);}}static bool mz(string bE){
return bE=="rate"||bE=="eta"||bE=="trend"||bE.Length>3&&(bE.StartsWith("top")&&char.IsDigit(bE[3])||bE.Length>4&&bE.StartsWith("line")&&char.IsDigit(bE[4]));}void ru(){
ge.Clear();df.Clear();gf.Clear();gc.Clear();iU.Clear();b3= -1;iX=iY=iV=0;foreach(var lr in t.Z){var kI=new List<List<bx>>();
foreach(var uE in lr.ak)kI.Add(uE.aq);if(lr.ak.Count==0)kI.Add(lr.fZ);foreach(var ub in kI)foreach(var bF in ub){if(!mz(bF.I))continue;
if(bF.I=="rate"||bF.I=="eta"||bF.I=="trend"){gc.Add(bF.aM);continue;}bool qD=bF.I[0]=='t';var nX=qD ? ge : gf;int ud=int.Parse(bF.I.Substring(qD ? 3 : 4));
int oG;nX.TryGetValue(bF.aM,out oG);nX[bF.aM]=Math.Max(oG,ud);}}df.AddRange(ge.Keys);}void sb(){if(gf.Count==0||T<iY)return;
iY=T+2.0;foreach(var kz in gf){List<IMyTerminalBlock>jl;var p8=b0.TryGetValue(kz.Key,out jl)&&jl.Count>0 ? jl[0]as IMyTextSurface : null;
string[]o7=p8==null ? new string[0]: p8.GetText().Split('\n');for(int fh=1;fh<=kz.Value;fh++)bC[kz.Key+".line"+fh]=fh<=o7.Length ? o7[fh-1].TrimEnd('\r'): "";}}
void sa(){if(gc.Count==0||T<iV)return;iV=T+5.0;foreach(var dj in gc){double hS=a2(dj,"percent"),cj=0.0;double[]gP;
if(iU.TryGetValue(dj,out gP))cj=gP[2]*0.6+(hS-gP[0])/Math.Max(0.1,T-gP[1])*0.4;iU[dj]=new[]{hS,T,cj};double ff=cj*60.0;int cZ=(int)Math.Min(359999.0,(cj<0 ? hS/ -cj : cj>0 ?(100.0-hS)/cj : 0.0)+0.5);
bC[dj+".rate"]=(ff>=0 ? "+" : "")+ff.ToString("0.0")+"%/min";bC[dj+".trend"]=ff>0.05 ? "^" : ff< -0.05 ? "v" : "=";bC[dj+".eta"]=Math.Abs(ff)<0.05 ? "--" :(cj<0 ? "empty " : "full ")+(cZ>=3600 ? cZ/3600+"h "+cZ%3600/60+"m" : cZ>=60 ? cZ/60+"m "+cZ%60+"s" : cZ+"s");
}}void sc(){if(df.Count==0)return;if(b3<0){if(T<iX)return;b3=0;gd=0;ey.Clear();}while(b3<df.Count&&Runtime.CurrentInstructionCount<20000){
string jo=df[b3];List<IMyTerminalBlock>go;b0.TryGetValue(jo,out go);if(go!=null&&gd<go.Count){var nd=go[gd++];
for(int ke=0;ke<nd.InventoryCount;ke++){iR.Clear();nd.GetInventory(ke).GetItems(iR);foreach(var gZ in iR){string kv=gZ.Type.TypeId.Replace("MyObjectBuilder_","");
string oR=kv=="Ore"||kv=="Ingot" ? gZ.Type.SubtypeId+" "+kv.ToLowerInvariant(): gZ.Type.SubtypeId;double oH;ey.TryGetValue(oR,out oH);ey[oR]=oH+(double)gZ.Amount;
}}continue;}var hl=new List<KeyValuePair<string,double>>(ey);hl.Sort((vP,vV)=>vV.Value.CompareTo(vP.Value));for(int dH=1;dH<=ge[jo];dH++)bC[jo+".top"+dH]=dH<=hl.Count ? hl[dH-1].Key+" "+M.mr(hl[dH-1].Value,"auto"): "-";
ey.Clear();b3++;gd=0;}if(b3>=df.Count){b3= -1;iX=T+15.0;}}void aj(int vo,string vw){dd.Add(new KeyValuePair<string,int>(vw,vo));
}static string iL(string lk,int pf){return lk.Length>pf ? lk.Substring(0,pf): lk;}void mx(){var ld=Me as IMyTextSurfaceProvider;if(ld==null||ld.SurfaceCount<1)return;
a5=ld.GetSurface(0);a5.ContentType=iN ? ContentType.SCRIPT : ContentType.NONE;a5.Script="";a5.ScriptBackgroundColor=Color.Black;
}void iM(){if(T>=f9){f9=T+(eu>t.Z.Count+1 ? 5.0 : 1.0);rc();if(iN&&a5!=null&&ew==0)rm();
}Echo(mY);}void rc(){dd.Clear();int jr=0,nt=0;foreach(var ne in t.cu){List<IMyTerminalBlock>kC;if(ne.bU==bc.d7){
jr++;continue;}if(b0.TryGetValue(ne.ag,out kC)){nt+=kC.Count;if(kC.Count>0)jr++;}}int n8=t.be.Count;aj(0,"HELM-[Kernel-(v"+rz+")]");
aj(4,"------------------------------");aj(1,"krn-cfg");aj(2," screens "+t.Z.Count+"  binds "+jr+"/"+t.cu.Count+"  errors "+n8);
aj(2," alert "+(cH ? "RED" : "off")+"  blocks "+nt+"  docked-ignored "+iP);if(n8>0)aj(3," ! "+iL(t.be[0],40));
else if(t.eq.Count>0)aj(3," ? "+iL(t.eq[0],40));aj(1,"cfg-dsp");if(t.Z.Count==0)aj(3," no @screen: run 'starter'");
int vb=0;foreach(var ll in t.Z){if(++vb>10){aj(2," +"+(t.Z.Count-10)+" more");break;}IMyTextSurface lx;string qc="--",lD="no block";
if(b1.TryGetValue(ll,out lx)){qc=lx.SurfaceSize.X.ToString("0")+"x"+lx.SurfaceSize.Y.ToString("0");lD=bB.ContainsKey(ll)? "boot" : "live";
}aj(lD=="no block" ? 3 : 2," "+iL(ll.bg,14).PadRight(14)+" "+qc.PadRight(9)+" "+lD);}aj(1,"perf");aj(2," tick "+ex+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
aj(2," instr "+gb+"  peak "+iQ+"  max "+Runtime.MaxInstructionCount);aj(2," rate "+Runtime.UpdateFrequency+"  idle "+eu+"  up "+(int)T+"s");
if(iT.Length>0)aj(2," last "+iT);var p0=new StringBuilder();foreach(var tR in dd)p0.AppendLine(tR.Key);mY=p0.ToString();}static Color ri(int vp){
switch(vp){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void rm(){Vector2 cV=a5.SurfaceSize;Vector2 kQ=(a5.TextureSize-cV)/2f;int jC=30;foreach(var tS in dd)jC=Math.Max(jC,tS.Key.Length);
Vector2 jB=a5.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float p1=Math.Min(1f,Math.Min(cV.X*0.94f/(jC*jB.X),cV.Y*0.94f/(dd.Count*jB.Y)));
float vQ=kQ.X+cV.X*0.03f;float q1=kQ.Y+cV.Y*0.03f;var lz=new List<MySprite>();lz.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kQ+cV/2f,Size=cV,Color=new Color(12,12,16)}
);foreach(var oW in dd){a9(lz,oW.Key,ri(oW.Value),vQ,q1,p1);q1+=jB.Y*p1;}using(var to=a5.DrawFrame()){to.AddRange(lz);}}static readonly List<string>rb=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void iE(){bB.Clear();if(t==null)return;for(int gT=0;gT<t.Z.Count;gT++){var hs=t.Z[gT];if(hs.h5>0f)bB[hs]=T+(gT/Math.Max(1,hs.me))*hs.mh/60.0;
}}void rY(){List<E>eK=null;int tA=(bB.Count+3)/4,tK=0;foreach(var g2 in bB){double kY=(T-g2.Value)/g2.Key.h5;if(kY<0)continue;
if(kY>=1.0){if(eK==null)eK=new List<E>();eK.Add(g2.Key);continue;}if((tK++ +ex)%tA==0)rl(g2.Key,kY);}if(eK==null)return;foreach(var fo in eK){
bB.Remove(fo);if(fo.h4>0f)f8[fo]=T;b2.Remove(fo);cI.Remove(fo);}}static void a9(List<MySprite>vg,string vx,Color sX,float vR,float vW,float u6){
var qq=MySprite.CreateText(vx,"Monospace",sX,u6,TextAlignment.LEFT);qq.Position=new Vector2(vR,vW);vg.Add(qq);}static void l7(List<MySprite>vh,List<string>gj,E hv,float vS,float vX,float u7,float t2){
string[]vm=hv.fH!=null&&hv.fH.Length>0 ? hv.fH : new[]{hv.mf};for(int eT=0;eT<gj.Count;
eT++){p ju=M.mt(vm,gj.Count>1 ? 100.0*eT/(gj.Count-1): 0.0);a9(vh,gj[eT],new Color(ju.bX,ju.bS,ju.bN),vS,vX+eT*t2,u7);}}bool r1(E dL,IMyTextSurface lI){
aA aC,aS;E p4=t.Z.Find(q_=>q_!=dL&&q_.bg.Equals(dL.il,StringComparison.OrdinalIgnoreCase));if(p4==null|| !b2.TryGetValue(p4,out aC)|| !aC.ed)return false;
if(!b2.TryGetValue(dL,out aS))b2[dL]=aS=new aA();Vector2 dO=lI.SurfaceSize;if(aS.ed&&aS.ep==aC.ep&&aS.ef==dO)return false;
aS.ed=true;aS.ep=aC.ep;aS.ef=dO;aS.f.Clear();Vector2 hE=aC.ef,pp=aC.ir,pn=(lI.TextureSize-dO)/2f;aS.ir=pn;
int om=0,qC=aC.f.Count;if(dL.im!=null){int dG=aC.bW.FindIndex(vT=>vT.mD.Equals(dL.im,StringComparison.OrdinalIgnoreCase));
if(dG<0)return false;hE=aC.bW[dG].mM;pp=aC.bW[dG].mI;om=aC.bW[dG].iD;if(dG+1<aC.bW.Count)qC=aC.bW[dG+1].iD;}float g0=Math.Min(dO.X/hE.X,dO.Y/hE.Y);
Vector2 sx=pn+(dO-hE*g0)/2f;for(int kf=om;kf<qC;kf++){var bG=aC.f[kf];if(bG.Position.HasValue)bG.Position=(bG.Position.Value-pp)*g0+sx;if(bG.Size.HasValue)bG.Size=bG.Size.Value*g0;
if(bG.Type==SpriteType.TEXT)bG.RotationOrScale*=g0;aS.f.Add(bG);}using(var tp=lI.DrawFrame())tp.AddRange(aS.f);return true;}bool rW(E dM,IMyTextSurface hC){
List<string>eB;if(cI.ContainsKey(dM)|| !t.dY.TryGetValue(dM.cs,out eB))return false;cI[dM]="banner";Vector2 cW=hC.SurfaceSize;
Vector2 kR=(hC.TextureSize-cW)/2f;int gu=1;foreach(var tT in eB)gu=Math.Max(gu,tT.Length);Vector2 eH=hC.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float hu=Math.Min(cW.X*0.94f/(gu*eH.X),cW.Y*0.94f/(eB.Count*eH.Y));var lA=new List<MySprite>();lA.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kR+cW/2f,Size=cW,Color=string.IsNullOrEmpty(dM.bO)? new Color(10,12,16): J(dM.bO)}
);l7(lA,eB,dM,kR.X+(cW.X-gu*eH.X*hu)/2f,kR.Y+(cW.Y-eB.Count*eH.Y*hu)/2f,hu,eH.Y*hu);using(var tq=hC.DrawFrame()){tq.AddRange(lA);
}return true;}void rl(E dN,double ps){IMyTextSurface fq;if(!b1.TryGetValue(dN,out fq))return;Vector2 co=fq.SurfaceSize;
Vector2 kS=(fq.TextureSize-co)/2f;var e5=dN.rv();string pc=dN.mg ?? "HELM TELEMETRY KERNEL ONLINE";bool kJ=co.Y>=200f;
List<string>dg;if(dN.cv==null|| !t.dY.TryGetValue(dN.cv,out dg))dg=rb;int dp=pc.Length+3;foreach(var tU in e5)dp=Math.Max(dp,tU.Length+5);
if(kJ)foreach(var tV in dg)dp=Math.Max(dp,tV.Length);int pT=(kJ ? dg.Count+1 : 0)+2+e5.Count;Vector2 dn=fq.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float ck=Math.Min(1f,Math.Min(co.X*0.94f/(dp*dn.X),co.Y*0.94f/(pT*dn.Y)));float hU=kS.X+(co.X-dp*dn.X*ck)/2f;float c1=kS.Y+(co.Y-pT*dn.Y*ck)/2f;
float g6=dn.Y*ck;var dP=new List<MySprite>();dP.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kS+co/2f,Size=co,Color=new Color(10,12,16)}
);if(kJ){l7(dP,dg,dN,hU,c1,ck,g6);c1+=(dg.Count+1)*g6;}a9(dP,">> "+pc,new Color(34,197,94),hU,c1,ck);c1+=g6*2f;int vc=M.ra(ps,e5.Count);
for(int gU=0;gU<vc;gU++){bool po=gU==e5.Count-1&&ps>=0.8;a9(dP,po ? "[OK]" : "[..]",po ? new Color(34,197,94): new Color(56,189,248),hU,c1,ck);a9(dP,e5[gU],new Color(203,213,225),hU+5f*dn.X*ck,c1,ck);
c1+=g6;}using(var tr=fq.DrawFrame()){tr.AddRange(dP);}}void iA(){b0.Clear();b1.Clear();ga.Clear();
ev.Clear();f7=false;if(t==null)return;foreach(var af in t.cu){var a6=new List<IMyTerminalBlock>();if(af.bU==bc.d7){
string kt= !string.IsNullOrEmpty(af.bw)? af.bw : af.ag;if(!aQ.ContainsKey(kt)){aQ[kt]=af.fN;if(af.h7!=null)bC[kt]=af.h7;
}b0[af.ag]=a6;continue;}var qu=af.a3.Count>0 ? af.a3 : new List<string>{af.mP};foreach(var c_ in qu){
if(string.IsNullOrEmpty(c_))continue;bool oP=c_.Contains("*")||c_.Contains("?");if(af.bU==bc.ih||oP){var j7=new List<IMyTerminalBlock>();
rf(c_,j7,af.fE);if(j7.Count>0){foreach(var oy in j7){if(!a6.Contains(oy))a6.Add(oy);}}else{var m6=dX(af.fE);
for(int je=0;je<m6.Count;je++){var hK=m6[je];if(oP ? M.rE(c_,hK.CustomName): string.Equals(c_,hK.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!a6.Contains(hK))a6.Add(hK);}}}}else{var jk=mn(c_,af.fE);if(jk!=null&& !a6.Contains(jk))a6.Add(jk);}}if(a6.Count==0){
r7(af,qu,a6);if(a6.Count>0)ga[af.ag]=true;}b0[af.ag]=a6;}foreach(var bK in t.Z){
if(string.IsNullOrEmpty(bK.bV))continue;var kV=mn(bK.bV);if(kV==null){ev.AppendLine($"HELM: output block '{bK.bV}' not found");
continue;}IMyTextSurface cY=null;var le=kV as IMyTextSurfaceProvider;var nY=kV as IMyTextSurface;if(le!=null&&bK.cz<le.SurfaceCount){
cY=le.GetSurface(bK.cz);}else if(nY!=null&&bK.cz==0){cY=nY;}if(cY==null){ev.AppendLine($"HELM: output block '{bK.bV}' has no surface index {bK.cz}");
continue;}cY.ContentType=ContentType.SCRIPT;cY.Script="";cY.ScriptBackgroundColor=Color.Black;b1[bK]=cY;}cG.Clear();
de.Clear();}static readonly string[]mm={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool rt(int tL,IMyTerminalBlock v){switch(tL){case 0: return v is IMyBatteryBlock;case 1: return v is IMyReactor;case 2: return v is IMySolarPanel;
case 3: return v is IMyWindTurbine;case 4: return v is IMyGasTank;case 5: return v is IMyAirVent;case 6: return v is IMyCargoContainer;case 7: return v is IMyJumpDrive;
case 8: return v is IMyThrust;case 9: return v is IMyMotorSuspension;case 10: return v is IMyDoor;case 11: return v is IMyShipConnector;case 12: return v is IMyAssembler;
case 13: return v is IMyRefinery;case 14: return v is IMyShipDrill;case 15: return v is IMyShipWelder;case 16: return v is IMyShipGrinder;case 17: return v is IMyGasGenerator;
case 18: return v is IMyLargeTurretBase;case 19: return v is IMyProjector;case 20: return v is IMyGyro;case 21: return v is IMyGravityGeneratorBase;case 22: return v is IMyLandingGear;
case 23: return v is IMyRadioAntenna;case 24: return v is IMyBeacon;case 25: return v is IMyLightingBlock;case 26: return v is IMyPistonBase;case 27: return v is IMyMotorStator;
case 28: return v is IMyWarhead;case 29: return v is IMySensorBlock;case 30: return v is IMyCameraBlock;case 31: return v is IMyParachute;case 32: return v is IMyTurretControlBlock;
}return false;}void r7(bu sM,List<string>vu,List<IMyTerminalBlock>jm){string tF=(sM.ag+" "+string.Join(" ",vu)).ToLowerInvariant();
var kx=new List<int>();for(int g1=0;g1<mm.Length;g1++){foreach(var vO in mm[g1].Split(','))if(tF.Contains(vO)){kx.Add(g1);break;}}if(kx.Count==0)return;
var m5=dX();var pq=new List<IMyTerminalBlock>();for(int kg=0;kg<m5.Count;kg++){var jf=m5[kg];foreach(int tM in kx){if(rt(tM,jf)){(jf.CubeGrid==Me.CubeGrid ? jm : pq).Add(jf);
break;}}}if(jm.Count==0)jm.AddRange(pq);}void r9(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(t==null||t.Z.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {t.Z.Count} screens, {t.cu.Count} binds");f7=false;
Echo($"Blocks: {dX().Count} on this construct; {iP} on docked ships (ignored)");cG.Clear();Echo("--- BIND RESOLUTION ---");foreach(var at in t.cu){
if(at.bU==bc.d7){string ku= !string.IsNullOrEmpty(at.bw)? at.bw : at.ag;double vE=aQ.ContainsKey(ku)? aQ[ku]: at.fN;
Echo($"[OK] @bind {at.ag} (dynamic variable '{ku}' = {vE})");continue;}List<IMyTerminalBlock>jn;if(b0.TryGetValue(at.ag,out jn)){if(jn.Count>0){
string tj=ga.ContainsKey(at.ag)&&ga[at.ag]? " [fallback: typed match]" : "";Echo($"[OK] @bind {at.ag} ({jn.Count} blocks matched{tj})");
}else{string vv=string.Join(", ",at.a3.Count>0 ? at.a3 : new List<string>{at.mP});Echo($"[WARN] @bind {at.ag} -> 0 blocks matched \"{vv}\"");
}}else{Echo($"[ERR] @bind {at.ag} -> unresolved");}}if(aQ.Count>0){Echo($"--- DYNAMIC VARIABLES ({aQ.Count}) ---");foreach(var oY in aQ)Echo($"[VAR] {oY.Key} = {oY.Value}");
}IMyTextSurface i6=null;foreach(var um in b1.Values){i6=um;break;}if(i6!=null){var lB=new List<string>();i6.GetSprites(lB);
Echo($"--- SPRITES ({lB.Count}): names usable in bg_texture ---");Echo(string.Join(", ",lB.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bL in t.Z){IMyTextSurface hD;if(b1.TryGetValue(bL,out hD)){var qd=hD.SurfaceSize;var vk=M.rh(bL);
Echo($"[OK] Screen \"{bL.bg}\" [{bL.cA}] -> {bL.bV} [#{bL.cz}] ({qd.X:0}x{qd.Y:0} px, texture {hD.TextureSize.X:0}x{hD.TextureSize.Y:0}, {vk.ah} sprites)");
}else{Echo($"[ERR] Screen \"{bL.bg}\" -> Missing block \"{bL.bV}\" [#{bL.cz}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double r8(IMyTerminalBlock w){
if(w==null)return 0.0;var gm=w as IMyBatteryBlock;if(gm!=null)return gm.MaxStoredPower>0 ?(gm.CurrentStoredPower/gm.MaxStoredPower)*100.0 : 0.0;
var g_=w as IMyJumpDrive;if(g_!=null)return g_.MaxStoredPower>0 ?(g_.CurrentStoredPower/g_.MaxStoredPower)*100.0 : 0.0;var qt=w as IMyGasTank;
if(qt!=null)return qt.FilledRatio*100.0;var qT=w as IMyAirVent;if(qT!=null)return qT.GetOxygenLevel()*100.0;var nB=w as IMyCargoContainer;if(nB!=null){
var gX=nB.GetInventory();if(gX!=null&&gX.MaxVolume.RawValue>0)return(double)gX.CurrentVolume.RawValue/gX.MaxVolume.RawValue*100.0;return 0.0;}var hm=w as IMyReactor;
if(hm!=null)return hm.MaxOutput>0 ?(hm.CurrentOutput/hm.MaxOutput)*100.0 : 0.0;var hw=w as IMySolarPanel;if(hw!=null)return hw.MaxOutput>0 ?(hw.CurrentOutput/hw.MaxOutput)*100.0 : 0.0;
var hT=w as IMyWindTurbine;if(hT!=null)return hT.MaxOutput>0 ?(hT.CurrentOutput/hT.MaxOutput)*100.0 : 0.0;var nZ=w as IMyDoor;if(nZ!=null)return(double)nZ.OpenRatio*100.0;
var hN=w as IMyThrust;if(hN!=null)return hN.MaxEffectiveThrust>0 ?(hN.CurrentThrust/hN.MaxEffectiveThrust)*100.0 : 0.0;var lN=w as IMyMotorSuspension;
if(lN!=null)return lN.Brake ? 100.0 :(lN.Friction*100.0);var jG=w as IMyShipConnector;if(jG!=null)return jG.Status==MyShipConnectorStatus.Connected ? 100.0 :(jG.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var pH=w as IMyProductionBlock;if(pH!=null)return pH.IsProducing ? 100.0 : 0.0;var n2=w as IMyShipDrill;if(n2!=null)return n2.Enabled ? 100.0 : 0.0;
var lV=w as IMyShipToolBase;if(lV!=null)return lV.IsActivated ? 100.0 :(lV.Enabled ? 50.0 : 0.0);var j4=w as IMyGasGenerator;if(j4!=null)return j4.AutoRefill ? 100.0 :(j4.Enabled ? 50.0 : 0.0);
var hO=w as IMyLargeTurretBase;if(hO!=null)return hO.IsShooting ? 100.0 :(hO.HasTarget ? 50.0 :(hO.Enabled ? 10.0 : 0.0));var fg=w as IMyProjector;
if(fg!=null)return fg.TotalBlocks>0 ?((double)(fg.TotalBlocks-fg.RemainingBlocks)/fg.TotalBlocks)*100.0 : 0.0;var ka=w as IMyGyro;if(ka!=null)return ka.GyroOverride ? 100.0 :(ka.Enabled ? 50.0 : 0.0);
var oA=w as IMyGravityGeneratorBase;if(oA!=null)return Math.Min(100.0,Math.Abs(oA.GravityAcceleration)/9.81*100.0);var j3=w as IMyLandingGear;if(j3!=null)return j3.IsLocked ? 100.0 :(j3.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var m7=w as IMyRadioAntenna;if(m7!=null)return(m7.Radius/50000.0)*100.0;var nk=w as IMyBeacon;if(nk!=null)return(nk.Radius/50000.0)*100.0;
var o5=w as IMyLightingBlock;if(o5!=null)return o5.Intensity*10.0;var hh=w as IMyPistonBase;if(hh!=null)return hh.HighestPosition>0 ?(hh.CurrentPosition/hh.HighestPosition)*100.0 : 0.0;
var pQ=w as IMyMotorStator;if(pQ!=null)return Math.Min(100.0,Math.Abs(pQ.Angle)/6.28318*100.0);var qW=w as IMyWarhead;if(qW!=null)return qW.IsArmed ? 100.0 : 0.0;
var lu=w as IMySensorBlock;if(lu!=null)return!lu.LastDetectedEntity.IsEmpty()? 100.0 :(lu.Enabled ? 25.0 : 0.0);var jx=w as IMyCameraBlock;
if(jx!=null)return jx.IsActive ? 100.0 :(jx.Enabled ? 50.0 : 0.0);var nH=w as IMyParachute;if(nH!=null)return(double)nH.OpenRatio*100.0;
var hM=w as IMyTurretControlBlock;if(hM!=null)return hM.HasTarget ? 100.0 :(hM.IsUnderControl ? 50.0 :(hM.Enabled ? 10.0 : 0.0));var oo=w as IMyFunctionalBlock;
if(oo!=null)return oo.Enabled ? 100.0 : 0.0;return 0.0;}static double rB(IMyTerminalBlock jg,string dt){bool qa=dt.StartsWith("share:",StringComparison.OrdinalIgnoreCase);
if(qa)dt=dt.Substring(6);int nA=dt.IndexOf(':');string kw=dt.Substring(0,nA).ToLowerInvariant();var qG=new MyItemType("MyObjectBuilder_"+(kw=="ammo" ? "AmmoMagazine" : char.ToUpper(kw[0])+kw.Substring(1)),dt.Substring(nA+1));
double lH=0,jz=0;for(int gV=0;gV<jg.InventoryCount;gV++){lH+=(double)jg.GetInventory(gV).GetItemAmount(qG);jz+=(double)jg.GetInventory(gV).MaxVolume;}return qa ?(jz>0 ? lH*qG.GetItemInfo().Volume/jz*100.0 : 0.0): lH;
}double mJ(IMyTerminalBlock x,string oe){if(x==null)return 0.0;string c=oe.ToLowerInvariant();if(c=="percent"||c=="pct")return r8(x);
if(c.IndexOf(':')>0)return rB(x,oe);var b6=x as IMyBatteryBlock;if(b6!=null){if(c=="current"||c=="stored"||c=="val"||c=="value"||c=="raw")return b6.CurrentStoredPower;
if(c=="max"||c=="capacity")return b6.MaxStoredPower;if(c=="output")return b6.CurrentOutput;if(c=="maxoutput")return b6.MaxOutput;if(c=="input")return b6.CurrentInput;
if(c=="maxinput")return b6.MaxInput;return b6.CurrentStoredPower;}var ks=x as IMyJumpDrive;if(ks!=null){if(c=="max"||c=="capacity")return ks.MaxStoredPower;
return ks.CurrentStoredPower;}var hH=x as IMyGasTank;if(hH!=null){if(c=="max"||c=="capacity")return hH.Capacity;return hH.FilledRatio*hH.Capacity;
}var qU=x as IMyAirVent;if(qU!=null){if(c=="max"||c=="capacity")return 100.0;return qU.GetOxygenLevel()*100.0;}var nC=x as IMyCargoContainer;if(nC!=null){
var gY=nC.GetInventory();if(gY!=null){if(c=="mass")return(double)gY.CurrentMass;if(c=="max"||c=="capacity")return(double)gY.MaxVolume;return(double)gY.CurrentVolume;
}return 0.0;}var lf=x as IMyPowerProducer;if(lf!=null){if(c=="max"||c=="capacity")return lf.MaxOutput;return lf.CurrentOutput;}var n_=x as IMyDoor;
if(n_!=null){if(c=="max")return 1.0;return(double)n_.OpenRatio*100.0;}var lS=x as IMyThrust;if(lS!=null){if(c=="max"||c=="capacity")return lS.MaxEffectiveThrust;
return lS.CurrentThrust;}var cq=x as IMyMotorSuspension;if(cq!=null){if(c=="brake")return cq.Brake ? 100.0 : 0.0;if(c=="height")return cq.Height;
if(c=="strength")return cq.Strength;if(c=="power")return cq.Power;if(c=="steer")return cq.SteeringOverride*100.0;if(c=="propulsion")return cq.PropulsionOverride*100.0;
return cq.Friction;}var nP=x as IMyShipConnector;if(nP!=null){return nP.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var lc=x as IMyProductionBlock;
if(lc!=null){if(c=="producing"||c=="active"||c=="running"||c=="state")return lc.IsProducing ? 100.0 : 0.0;if(c=="queue")return lc.IsQueueEmpty ? 0.0 : 1.0;
}var ja=x as IMyAssembler;if(ja!=null){if(c=="disassembling")return ja.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;if(c=="repeating"||c=="loop")return ja.Repeating ? 100.0 : 0.0;
}var j5=x as IMyGasGenerator;if(j5!=null){if(c=="autorefill")return j5.AutoRefill ? 100.0 : 0.0;if(c=="active")return j5.Enabled ? 100.0 : 0.0;}var hP=x as IMyLargeTurretBase;
if(hP!=null){if(c=="shooting")return hP.IsShooting ? 100.0 : 0.0;if(c=="target"||c=="hastarget")return hP.HasTarget ? 100.0 : 0.0;if(c=="range")return hP.Range;
}var ci=x as IMyProjector;if(ci!=null){if(c=="built")return ci.TotalBlocks-ci.RemainingBlocks;if(c=="remaining")return ci.RemainingBlocks;if(c=="total")return ci.TotalBlocks;
if(c=="projecting")return ci.IsProjecting ? 100.0 : 0.0;if(c=="armor")return ci.RemainingArmorBlocks;if(c=="buildable")return ci.BuildableBlocksCount;}var lW=x as IMyShipToolBase;
if(lW!=null){if(c=="working"||c=="active"||c=="activated")return lW.IsActivated ? 100.0 : 0.0;return lW.Enabled ? 100.0 : 0.0;}var dv=x as IMyGyro;if(dv!=null){
if(c=="override")return dv.GyroOverride ? 100.0 : 0.0;if(c=="pitch")return dv.Pitch;if(c=="yaw")return dv.Yaw;if(c=="roll")return dv.Roll;if(c=="power")return dv.GyroPower*100.0;
}var oB=x as IMyGravityGeneratorBase;if(oB!=null){if(c=="g"||c=="accel")return oB.GravityAcceleration;}var oz=x as IMyLandingGear;if(oz!=null){
if(c=="locked")return oz.IsLocked ? 100.0 : 0.0;}var m8=x as IMyRadioAntenna;if(m8!=null){if(c=="radius"||c=="range")return m8.Radius;}var nl=x as IMyBeacon;
if(nl!=null){if(c=="radius"||c=="range")return nl.Radius;}var kF=x as IMyLightingBlock;if(kF!=null){if(c=="intensity")return kF.Intensity;if(c=="radius")return kF.Radius;
}var hi=x as IMyPistonBase;if(hi!=null){if(c=="pos"||c=="position"||c=="extension")return hi.CurrentPosition;if(c=="velocity")return hi.Velocity;
if(c=="max")return hi.HighestPosition;}var fn=x as IMyMotorStator;if(fn!=null){if(c=="angle"||c=="deg")return fn.Angle*180.0/Math.PI;if(c=="rad")return fn.Angle;
if(c=="velocity"||c=="rpm")return fn.TargetVelocityRPM;if(c=="locked")return fn.RotorLock ? 100.0 : 0.0;}var l4=x as IMyWarhead;if(l4!=null){
if(c=="armed")return l4.IsArmed ? 100.0 : 0.0;if(c=="countdown")return l4.DetonationTime;}var p7=x as IMySensorBlock;if(p7!=null){if(c=="detected")return!p7.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;
}var jy=x as IMyCameraBlock;if(jy!=null){if(c=="range")return jy.AvailableScanRange;if(c=="active")return jy.IsActive ? 100.0 : 0.0;}var nI=x as IMyParachute;
if(nI!=null){if(c=="open"||c=="openratio")return(double)nI.OpenRatio*100.0;}var dT=x as IMyTurretControlBlock;if(dT!=null){if(c=="target"||c=="hastarget")return dT.HasTarget ? 100.0 : 0.0;
if(c=="azimuth")return dT.AzimuthRotor!=null ? dT.AzimuthRotor.Angle*180.0/Math.PI : 0.0;if(c=="elevation")return dT.ElevationRotor!=null ? dT.ElevationRotor.Angle*180.0/Math.PI : 0.0;
}var op=x as IMyFunctionalBlock;if(op!=null){return op.Enabled ? 100.0 : 0.0;}return 0.0;}double a2(string eE,string of){bu b7;
er.TryGetValue(eE,out b7);if(b7!=null&&b7.bU==bc.d7){string tN= !string.IsNullOrEmpty(b7.bw)? b7.bw : eE;
double nS;if(aQ.TryGetValue(tN,out nS))return nS;return b7.fN;}double n5;if(aQ.TryGetValue(eE,out n5))return n5;
List<IMyTerminalBlock>eF;if(!b0.TryGetValue(eE,out eF)||eF.Count==0)throw new InvalidOperationException($"HELM: bind '{eE}' resolved to zero blocks — check the name/typo");
if(eF.Count==1)return mJ(eF[0],of);var qS=new List<double>();foreach(var sB in eF)qS.Add(mJ(sB,of));return q6.q5(b7!=null ? b7.dW : aL.hX,qS);
}double iB(string sO){return a2(sO,"percent");}static string bf(double qO,bx dB){if(dB==null||(string.IsNullOrEmpty(dB.eo)&&string.IsNullOrEmpty(dB.d8)&&dB.I=="percent")){
return ms(qO);}return M.mr(qO,dB.eo,dB.d8);}bool r2(E cl){IMyTextSurface fr;if(!b1.TryGetValue(cl,out fr))return false;
if(cl.cs!=null)return rW(cl,fr);if(cl.il!=null)return r1(cl,fr);if(cl.c5.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return r3(cl,fr);}return r4(cl,fr);}Color al(bx oZ,Color s8){return oZ.iK!=null ? J(oZ.iK): s8;
}Color d9(string[]dQ,double uv){if(dQ.Length==1)return J(dQ[0]);double pL=Math.Max(0.0,Math.Min(100.0,uv))/100.0*(dQ.Length-1);int kh=Math.Min((int)pL,dQ.Length-2);
float hF=(float)(pL-kh);Color b4=J(dQ[kh]),gl=J(dQ[kh+1]);return new Color((int)(b4.R+(gl.R-b4.R)*hF),(int)(b4.G+(gl.G-b4.G)*hF),(int)(b4.B+(gl.B-b4.B)*hF),(int)(b4.A+(gl.A-b4.A)*hF));
}static Color[]fA(aA sS,string[]vn,string ta,float q0,float q2,float l1,float kb){int eC=16;bool oI=ta=="horizontal";var jF=new Color[eC];
for(int cO=0;cO<eC;cO++){p j2=M.mt(vn,100.0*cO/(eC-1));jF[cO]=new Color(j2.bX,j2.bS,j2.bN);float nz=l1/eC,ns=kb/eC;q(sS,oI ? new Vector2(q0+nz*(cO+0.5f),q2+kb/2f): new Vector2(q0+l1/2f,q2+ns*(cO+0.5f)),oI ? new Vector2(nz+1f,kb): new Vector2(l1,ns+1f),jF[cO]);
}return jF;}static void q(aA sT,Vector2 uG,Vector2 vd,Color sY,float uZ=0f,string s9="SquareSimple"){sT.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=s9,Position=uG,Size=vd,Color=sY,RotationOrScale=uZ}
);}void fB(aA b8,string cX,string[]aR,string nL,bool qf,float aH,float aY,float bJ,float bh){cX=cX.ToLowerInvariant();float V=cX=="heavy" ? 4f : 2f,fz=bJ-aH,kc=bh-aY;
Color bD=J(aR!=null&&aR.Length>0 ? aR[0]: nL ?? "40, 60, 85");Color b9=aR!=null&&aR.Length>1 ? J(aR[aR.Length-1]): bD;Action<float,float,float,float,Color>aV=(vU,vY,pK,pJ,sV)=>q(b8,new Vector2(vU+pK/2f,vY+pJ/2f),new Vector2(pK,pJ),sV);
if(cX=="corners"){float aT=Math.Max(10f,Math.Min(fz,kc)*0.15f);aV(aH,aY,aT,V,bD);aV(aH,aY,V,aT,bD);aV(bJ-aT,aY,aT,V,b9);aV(bJ-V,aY,V,aT,b9);aV(aH,bh-V,aT,V,bD);aV(aH,bh-aT,V,aT,bD);
aV(bJ-aT,bh-V,aT,V,b9);aV(bJ-V,bh-aT,V,aT,b9);return;}if(aR!=null&&aR.Length>1&&qf){fA(b8,aR,"horizontal",aH,aY,fz,V);fA(b8,aR,"horizontal",aH,bh-V,fz,V);
}else{aV(aH,aY,fz,V,bD);aV(aH,bh-V,fz,V,b9);}if("|frame|single|heavy|double|rounded|".Contains("|"+cX+"|")){aV(aH,aY,V,kc,bD);aV(bJ-V,aY,V,kc,b9);}if(cX=="rounded"){
float ca=V*3f;q(b8,new Vector2(aH,aY),new Vector2(ca,ca),bD,0f,"Circle");q(b8,new Vector2(bJ,aY),new Vector2(ca,ca),b9,0f,"Circle");q(b8,new Vector2(aH,bh),new Vector2(ca,ca),bD,0f,"Circle");
q(b8,new Vector2(bJ,bh),new Vector2(ca,ca),b9,0f,"Circle");}if(cX=="double")fB(b8,"single",aR,nL,qf,aH+5f,aY+5f,bJ-5f,bh-5f);}void rd(E m,IMyTextSurface lJ,aA g){
g.f.Clear();g.ek.Clear();g.bW.Clear();Vector2 aa=lJ.SurfaceSize;Vector2 vz=lJ.TextureSize;Vector2 bs=(vz-aa)/2f;
g.ir=bs;float fc=Math.Max(4f,aa.X*0.02f);float dE=Math.Max(4f,aa.Y*0.02f);if(m.cA=="compact"||(m.cA=="auto"&&aa.Y<=260f&&aa.Y>150f)){
fc=4f;dE=4f;}else if(m.cA=="corner"||(m.cA=="auto"&&aa.Y<=150f)){fc=2f;dE=2f;}if(m.iv>=0f)fc=dE=m.iv;
float qH=aa.X-(fc*2f);Color di=new Color(10,12,16);bool n1=true;if(!string.IsNullOrEmpty(m.bO)){var eR=p.ei(m.bO);
if(eR.fX)n1=false;else di=new Color(eR.bX,eR.bS,eR.bN,eR.hV);}g.ct= -1;g.fD=false;g.fC=J(m.l9 ?? "#5b0f0f");
g.d0.Clear();if(m.bQ!=null&&m.bQ.Length>1){g.ct=g.f.Count;g.fF=fA(g,m.bQ,m.d_,bs.X+2f,bs.Y+2f,aa.X-4f,aa.Y-4f);
}else if(n1||m.a0!=null||m.c6!=null){if(m.a0!=null)di=J(m.a0.First(uL=>uL.c8).bR);g.ct=g.f.Count;
g.fF=new[]{di};q(g,bs+(aa/2f),aa-new Vector2(4f,4f),di);if(m.a0!=null||m.c6!=null)g.d0.Add(new fG{
fW=g.ct,hY=di,fM=di,f3=m.a0,h1=m.hZ,h9=m.h_,fS=m.c6,ia=m.ib,ic=m.id,my=true}
);}if(!string.IsNullOrEmpty(m.h0))q(g,bs+(aa/2f),aa-new Vector2(4f,4f),J(m.mb ?? "255,255,255,24"),0f,m.h0);
if(!m.c5.Equals("none",StringComparison.OrdinalIgnoreCase)){float nu=bs.X+(aa.X-qH)/2f;fB(g,m.c5,m.fL,m.d2,!m.fU,nu-2f,bs.Y+dE*0.5f-1f,nu+qH+2f,bs.Y+aa.Y-dE*0.5f+1f);
}var k2=m.ak;if(k2.Count==0){k2=new List<aF>{new aF{db="main",d4=0,ej=0,el=m.ea,em=m.eb,aq=m.fZ}
};}float oD=m.ig>=0f ? m.ig : 8f;foreach(var r in k2){var ha=M.rg(r.d4,r.ej,r.el,r.em,m.ea,m.eb,aa.X,aa.Y,fc,dE,oD,oD);
float aw=bs.X+ha.mV;float bH=bs.Y+ha.mW;float aU=ha.mU;float cR=ha.mv;g.bW.Add(new ix{mD=r.db,iD=g.f.Count,mI=new Vector2(aw,bH),mM=new Vector2(aU,cR)}
);bool pU=r.a0!=null||r.c6!=null;if(r.bQ!=null&&r.bQ.Length>1){fA(g,r.bQ,r.d_!="vertical" ? r.d_ : m.d_,aw,bH,aU,cR);
}else{Color ch;if(r.a0!=null)ch=J(r.a0.First(uM=>uM.c8).bR);else if(!string.IsNullOrEmpty(r.c3))ch=J(r.c3);
else if(!string.IsNullOrEmpty(m.iw))ch=J(m.iw);else ch=Color.Transparent;if(ch.A>0||pU){if(pU)g.d0.Add(new fG{fW=g.f.Count,hY=ch,fM=ch,f3=r.a0,h1=r.hZ,h9=r.h_,fS=r.c6,ia=r.ib,ic=r.id}
);q(g,new Vector2(aw+aU/2f,bH+cR/2f),new Vector2(aU,cR),ch);}}if(r.d1!=null&& !r.d1.Equals("none",StringComparison.OrdinalIgnoreCase))fB(g,r.d1,r.fK ?? m.fL,r.fJ ?? m.d2,!m.fU,aw+1f,bH+1f,aw+aU-1f,bH+cR-1f);
string gL=r.mo ?? m.mp ?? "Monospace";float sK=M.rw(m.cA,aa.X,aa.Y,m.c7);float aG=r.mq ?? sK;
float eS=0f;if(!string.IsNullOrEmpty(r.cE)){eS=20f*aG;var i2=TextAlignment.LEFT;float lT=aw+4f;string m0=r.iI ?? m.mQ ?? "left";
if(m0.Equals("center",StringComparison.OrdinalIgnoreCase)){i2=TextAlignment.CENTER;lT=aw+(aU/2f);}else if(m0.Equals("right",StringComparison.OrdinalIgnoreCase)){
i2=TextAlignment.RIGHT;lT=aw+aU-4f;}string qw=r.iJ ?? m.mR;Color vA= !string.IsNullOrEmpty(qw)? J(qw): J(r.fJ ?? m.d2 ?? "white");
var qB=MySprite.CreateText(r.cE,gL,vA,aG*0.9f,i2);qB.Position=new Vector2(lT,bH+2f);g.f.Add(qB);
string[]jh=r.fK ?? m.fL;Color tc=jh!=null&&jh.Length>0 ? J(jh[0]): J(r.fJ ?? m.d2 ?? "40, 60, 85");
q(g,new Vector2(aw+aU/2f,bH+eS-2f),new Vector2(aU-8f,1f),tc);}if(r.it=="vertical"||r.aq.Any(uC=>uC.i==e.f5)){
re(g,lJ,r,gL,aG,aw,bH,aU,cR,eS);continue;}int v_=Math.Max(1,r.aq.Count);float ut=cR-eS-8f;
float qY=0f,m2=0f;foreach(var uD in r.aq)qY+=1<<uD.f4;float vC=ut/Math.Max(1f,qY);float e_=6f;for(int ki=0;ki<r.aq.Count;
ki++){var j=r.aq[ki];var O=new cy{eh=j};float dK=vC*(1<<j.f4);float pS=bH+eS+4f+m2;m2+=dK;float L=pS+(dK/2f);
float qy=L-(11f*aG);O.c9=g.f.Count;O.mu=j.ac!=null&&(j.ac.Contains("{val}")||j.ac.Contains("{percent}"));
float eY=j.fV!=null ? 18f*aG : 0f;var o2=MySprite.CreateText(j.ac,gL,j.ee!=null ? J(j.ee): Color.White,aG,TextAlignment.LEFT);
o2.Position=new Vector2(aw+e_+eY,qy);g.f.Add(o2);if(j.fV!=null)q(g,new Vector2(aw+e_+eY/2f,L),new Vector2(eY-2f,eY-2f),Color.White,0f,j.fV);
float t0=Math.Max(24f*aG,(j.ac.Length+1)*12f*aG);float vL=j.ip ? 0f :(!string.IsNullOrEmpty(j.eo)|| !string.IsNullOrEmpty(j.d8))? 64f*aG : 52f*aG;
if(j.i!=e.iG){float K=aw+e_+eY+t0;float sJ=aw+aU-e_-vL;float H=Math.Max(16f,sJ-K);
float Q=Math.Max(j.i==e.cB ? 8f : 4f,Math.Min((j.i==e.cB ? 20f : 16f)*aG,dK*(j.i==e.cB ? 0.55f : 0.45f)));
if(j.f4>0)Q=dK*0.7f;Color qF=al(j,new Color(25,30,38));O.bb=K;O.aD=H;O.ba=Q;O.bt=L;
if(j.i==e.dZ){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),qF);O.ab=g.f.Count;
q(g,new Vector2(K,L),new Vector2(0f,Q),J(j.az));bool uX=j.fP=="left";for(int lU=1;lU<j.iH;lU++)q(g,new Vector2(K+H*lU/j.iH,L),new Vector2(1f,Q),new Color(0,0,0,150));
if(j.f_!=null)foreach(double pa in j.f_)q(g,new Vector2(K+H*(float)(uX ? 1.0-pa/100.0 : pa/100.0),L),new Vector2(2f,Q+6f),Color.White);
if(j.iu!=null)fB(g,"single",null,j.iu,true,K,L-Q/2f,K+H,L+Q/2f);}else if(j.i==e.fO){
float dI=Math.Max(8f,Math.Min(H/2f,dK*0.85f));float gB=K+H/2f,gC=pS+dK-4f;int jK=j.bA>0 ? j.bA : 16;O.bb=gB;
O.bt=gC;O.aD=dI;for(int jH=0;jH<jK;jH++){float lR=(jH+0.5f)/jK*3.14159f;O.D.Add(g.f.Count);q(g,new Vector2(gB-(float)Math.Cos(lR)*dI*0.8f,gC-(float)Math.Sin(lR)*dI*0.8f),new Vector2(dI*3.14159f/jK*0.85f,Math.Max(3f,dI*0.16f)),al(j,new Color(24,28,36)),lR-1.5708f);
}O.D.Add(g.f.Count);q(g,new Vector2(gB,gC),new Vector2(3f,dI*0.9f),Color.White);q(g,new Vector2(gB,gC),new Vector2(8f,8f),Color.White,0f,"Circle");
}else if(j.i==e.eg){int ls=j.bA>0 ? j.bA : 10;float os=2f;float lt=Math.Max(2f,(H-(os*(ls-1)))/ls);for(int lm=0;
lm<ls;lm++){O.D.Add(g.f.Count);q(g,new Vector2(K+lm*(lt+os)+(lt/2f),L),new Vector2(lt,Q),al(j,new Color(22,26,32)));
}}else if(j.i==e.cB){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(20,24,30)));int hj=j.bA>0 ? j.bA : 8;
O.cC=new float[hj];float ot=2f;float jD=Math.Max(2f,(H-(ot*(hj-1)))/hj);for(int kZ=0;kZ<hj;kZ++){O.D.Add(g.f.Count);
q(g,new Vector2(K+kZ*(jD+ot)+(jD/2f),L),new Vector2(jD,2f),J(j.az));}}else if(j.i==e.ec){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(20,24,30)));
int lE=10;float ou=2f;float lF=Math.Max(2f,(H-(ou*(lE-1)))/lE);for(int ln=0;ln<lE;ln++){O.D.Add(g.f.Count);
q(g,new Vector2(K+ln*(lF+ou)+(lF/2f),L),new Vector2(lF,Q),al(j,new Color(24,28,36)),0.523599f);}}else if(j.i==e.d3){
int i8=10;float ov=2f;float i9=Math.Max(2f,(H-(ov*(i8-1)))/i8);for(int lo=0;lo<i8;lo++){O.D.Add(g.f.Count);
q(g,new Vector2(K+lo*(i9+ov)+(i9/2f),L),new Vector2(i9,Q),al(j,new Color(24,28,36)),1.570796f,"Triangle");}}else if(j.i==e.d6){
int jL=j.bA>0 ? j.bA : 12;float ow=2f;float jN=Math.Max(2f,(H-(ow*(jL-1)))/jL);float n0=Math.Min(jN,Q);for(int lp=0;
lp<jL;lp++){O.D.Add(g.f.Count);q(g,new Vector2(K+lp*(jN+ow)+(jN/2f),L),new Vector2(n0,n0),al(j,new Color(24,28,36)),0f,"Circle");
}}else if(j.i==e.c4){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),qF);O.ab=g.f.Count;
q(g,new Vector2(K+(H/2f),L),new Vector2(0f,Q),J(j.az));q(g,new Vector2(K+(H/2f),L),new Vector2(2f,Q+4f),new Color(80,110,150));
}else if(j.i==e.cF){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(20,24,30)));O.ab=g.f.Count;
float ky=Math.Max(4f,Q);q(g,new Vector2(K+ky*0.5f,L),new Vector2(ky,ky),new Color(40,50,65),0f,"Circle");}else if(j.i==e.cw){
float dk=Math.Max(6f,Math.Min(Q,14f*aG));q(g,new Vector2(K+dk*0.5f,L),new Vector2(dk,dk),new Color(28,34,46));
O.ab=g.f.Count;q(g,new Vector2(K+dk*0.5f,L),new Vector2(dk*0.6f,dk*0.6f),new Color(40,50,65));}else if(j.i==e.cD){
q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(18,22,30)));O.ab=g.f.Count;float k9=Math.Max(4f,Q*0.7f);
q(g,new Vector2(K+k9+2f,L),new Vector2(k9,k9),new Color(34,197,94),0f,"Circle");}else if(j.i==e.c2){int k8=8;
float ox=2f;float la=Math.Max(2f,(H-(ox*(k8-1)))/k8);for(int k_=0;k_<k8;k_++){O.D.Add(g.f.Count);q(g,new Vector2(K+k_*(la+ox)+(la/2f),L),new Vector2(la,Q),al(j,new Color(24,28,36)));
}}else if(j.i==e.cx){q(g,new Vector2(K+(H/2f),L),new Vector2(H,Q),al(j,new Color(20,24,30)));O.ab=g.f.Count;
float oL=Math.Max(4f,Q);q(g,new Vector2(K+(H/2f),L),new Vector2(oL,oL),J(j.az),0f,"Triangle");}}var qM=MySprite.CreateText("---",gL,Color.White,aG,TextAlignment.RIGHT);
qM.Position=new Vector2(aw+aU-e_,qy);O.a4=g.f.Count;g.f.Add(qM);g.ek.Add(O);}}g.ef=aa;
g.ed=true;}const string si="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bZ=8;void re(aA ad,IMyTextSurface vq,aF k1,string ol,float eP,float uq,float k0,float ur,float pt,float tE){
int ue=Math.Max(1,k1.aq.Count);float nM=(ur-12f)/ue;float hQ=22f*eP;float bq=k0+tE+4f+hQ;float aZ=Math.Max(20f,k0+pt-4f-hQ-bq);
float s5=vq.MeasureStringInPixels(new StringBuilder("M"),ol,eP*0.9f).X;for(int gW=0;gW<k1.aq.Count;gW++){var cP=k1.aq[gW];float bk=uq+6f+nM*(gW+0.5f);
float c0=nM*0.7f;var bn=new cy{eh=cP,mS=true,bb=bk,aD=c0,ba=aZ,bt=bq+aZ/2f,mj=s5};bn.c9=ad.f.Count;
a9(ad.f,cP.ac,cP.ee!=null ? J(cP.ee): Color.White,bk,k0+pt-4f-hQ,eP);bn.a4=ad.f.Count;
a9(ad.f,"---",Color.White,bk,bq-hQ,eP);var o_=ad.f[bn.c9];o_.Alignment=TextAlignment.CENTER;ad.f[bn.c9]=o_;
var qL=ad.f[bn.a4];qL.Alignment=TextAlignment.CENTER;ad.f[bn.a4]=qL;q(ad,new Vector2(bk,bq+aZ/2f),new Vector2(c0,aZ),al(cP,new Color(25,30,38)));
bn.ab=ad.f.Count;if(cP.i==e.f5){for(int nf=0;nf<bZ;nf++){bn.D.Add(ad.f.Count);q(ad,new Vector2(bk,bq+aZ),new Vector2(c0-6f,0f),Color.White);
}for(int l2=0;l2<4;l2++){bn.D.Add(ad.f.Count);var qr=MySprite.CreateText("",ol,l2==0 ? new Color(255,255,255,170): new Color(255,255,255,110),eP*0.9f,TextAlignment.CENTER);
qr.Position=new Vector2(bk,bq+aZ);ad.f.Add(qr);}Color jP=new Color(90,110,135);q(ad,new Vector2(bk-c0/2f,bq+aZ/2f),new Vector2(3f,aZ),jP);q(ad,new Vector2(bk+c0/2f,bq+aZ/2f),new Vector2(3f,aZ),jP);
q(ad,new Vector2(bk,bq+aZ),new Vector2(c0+3f,3f),jP);}else{q(ad,new Vector2(bk,bq+aZ),new Vector2(c0,0f),J(cP.az));}ad.ek.Add(bn);
}}bool sh(aA bi,cy z,double fx,double pM,Color gv,bool qQ){var a7=z.eh;float nn=z.C;if(z.C<0f)z.C=(float)fx;
z.C=Math.Abs(z.C-(float)fx)>0.05f ? z.C+((float)fx-z.C)*0.35f :(float)fx;float cQ=Math.Max(0f,Math.Min(1f,z.C/100f))*z.ba;
float gs=z.bt+z.ba/2f;float gz=z.bb;if(a7.i!=e.f5){var gG=bi.f[z.ab];gG.Size=new Vector2(z.aD,cQ);
gG.Position=new Vector2(gz,gs-cQ/2f);gG.Color=gv;bi.f[z.ab]=gG;if(qQ||z.C!=nn)aO(bi,z,bf(pM,a7),gv);
return qQ||z.C!=nn;}float jj=z.ba/bZ;string fi=a7.bv!=null ? M.dc(a7.bv,a7.d5!=null ? a2(a7.d5,a7.h6): fx): null;
Color t8=fi!=null ? J(fi): gv;string[]qk=fi!=null ?(fi.Contains("/")? fi.Split('/'): null): a7.bT&&a7.aN.Length>1 ? a7.aN : null;
for(int cK=0;cK<bZ;cK++){float qV=Math.Max(0f,Math.Min(jj,cQ-cK*jj));var hk=bi.f[z.D[cK]];hk.Size=new Vector2(z.aD-6f,qV);
hk.Position=new Vector2(gz,gs-cK*jj-qV/2f);hk.Color=qk!=null ? d9(qk,100.0*(cK+0.5)/bZ): t8;bi.f[z.D[cK]]=hk;}
int sW=Math.Max(1,(int)((z.aD-8f)/z.mj));bool va=cQ>6f;var l5=bi.f[z.D[bZ]];l5.Data=va ? si.Substring((int)(T*5.0)%6,Math.Min(sW,100)): "";
l5.Position=new Vector2(gz,gs-cQ-12f);bi.f[z.D[bZ]]=l5;for(int cf=0;cf<3;cf++){var js=bi.f[z.D[bZ+1+cf]];
double uy=(T*(0.35+0.1*cf)+cf*0.37)%1.0;js.Data=cQ>24f ?(cf==1 ? "O" : "o"): "";js.Position=new Vector2(gz+(cf-1)*z.aD*0.26f,gs-(float)uy*(cQ-6f)-10f);
bi.f[z.D[bZ+1+cf]]=js;}aO(bi,z,bf(pM,a7),gv);return true;}static void aO(aA nw,cy ly,string vy,Color sZ){
var l_=nw.f[ly.a4];l_.Data=ly.eh.ip ? "" : vy;l_.Color=sZ;nw.f[ly.a4]=l_;}static void mL(aA nx,cy qe,Color s_){
var n9=nx.f[qe.ab];n9.Color=s_;nx.f[qe.ab]=n9;}bool r4(E bM,IMyTextSurface lK){aA k;
if(!b2.TryGetValue(bM,out k)){k=new aA();b2[bM]=k;}bool pP= !k.ed||k.ef!=lK.SurfaceSize;
if(pP){rd(bM,lK,k);}bool S=pP;bool eA=(T%bM.hW)<bM.hW/2.0;double cN= -1.0,nQ;if(f8.TryGetValue(bM,out nQ)){
cN=(T-nQ)/bM.h4;if(cN>=1.0){f8.Remove(bM);cN=1.0;}}bool l3=cH&&eA&&k.fC.A>0;if(k.ct>=0&&l3!=k.fD){
k.fD=l3;for(int eU=0;eU<k.fF.Length;eU++){var no=k.f[k.ct+eU];no.Color=l3 ? k.fC : k.fF[eU];k.f[k.ct+eU]=no;
}S=true;foreach(var sL in k.d0)sL.ie=true;}foreach(var an in k.d0){if(an.my&&k.fD)continue;Color eD=an.hY;try{if(an.f3!=null){
double ny=a2(an.h1,an.h9);string hp=M.dc(an.f3,ny);eD=hp!=null&&hp.Contains("/")? d9(hp.Split('/'),ny): J(hp);
}if(an.fS!=null&&eA&&M.dc(an.fS,a2(an.ia,an.ic))!=null)eD=k.fC;}catch{}if(eD!=an.fM||an.ie){
an.fM=eD;an.ie=false;var nv=k.f[an.fW];nv.Color=eD;k.f[an.fW]=nv;S=true;}}for(int kj=0;kj<k.ek.Count;kj++){var b=k.ek[kj];
var h=b.eh;double F;bool oF=false;double kM=h.f0;try{F=a2(h.aM,h.I);if(h.da!=null)kM=a2(h.da,h.ik);
}catch{oF=true;F=0.0;}if(oF){if(b.fY!= -99999.0||(b.a4>=0&&k.f[b.a4].Data!="ERR")){S=true;b.fY= -99999.0;
if(b.a4>=0){var lq=k.f[b.a4];lq.Data="ERR";lq.Color=Color.Red;k.f[b.a4]=lq;}}continue;}double a8=F;
if(kM>0)F=Math.Min(100.0,a8/kM*100.0);bool qo=cN>=0.0&&h.i!=e.iG&&h.i!=e.cF&&h.i!=e.cw&&h.i!=e.cD&&h.i!=e.cx;
if(qo)F=cN<0.5 ? cN*200.0 : 100.0+(F-100.0)*(cN-0.5)*2.0;double gy=h.d5!=null ? a2(h.d5,h.h6): F;string gw=h.bv!=null ? M.dc(h.bv,gy): h.bT ? M.fT(h.aN,gy): h.az;
Color P=J(gw);bool bl=(P==Color.Red);if(gw!=null&&gw.Contains("/"))P=d9(gw.Split('/'),gy);if(h.bv==null&&h.bT&& !bM.fU&&h.aN.Length>1)P=d9(h.aN,gy);
if(bl&&eA){P=new Color(255,75,75);S=true;}if(h.h2!=null&&M.dc(h.h2,a2(h.mc,h.md))!=null){
bl=S=true;if(eA)P=new Color(P.R/3,P.G/3,P.B/3);}bool a_=qo||Math.Abs(a8-b.fY)>0.1;if(a_)b.fY=a8;
if(b.mS){if(sh(k,b,F,a8,P,a_))S=true;continue;}if(h.i==e.dZ||h.i==e.fO){double og=(h.ii||h.I=="percent"||h.I=="pct")? F : iB(h.aM);
if(b.C<0f)b.C=(float)og;float hI=(float)og;if(Math.Abs(b.C-hI)>0.05f){b.C+=(hI-b.C)*0.35f;
S=true;}else if(b.C!=hI){b.C=hI;S=true;}if(a_)S=true;if(S){float pC=(float)Math.Max(0.0,Math.Min(1.0,b.C/100.0));
float j_=b.aD*pC;if(h.i==e.fO){int eJ=b.D.Count-1;for(int dr=0;dr<eJ;dr++){var p9=k.f[b.D[dr]];
p9.Color=b.C>=(dr+0.5)/eJ*100.0 ?(h.bv==null&&h.bT&&h.aN.Length>1 ? d9(h.aN,(dr+0.5)/eJ*100.0): P): al(h,new Color(24,28,36));
k.f[b.D[dr]]=p9;}float i5=pC*3.14159f;var kP=k.f[b.D[eJ]];kP.Position=new Vector2(b.bb-(float)Math.Cos(i5)*b.aD*0.45f,b.bt-(float)Math.Sin(i5)*b.aD*0.45f);
kP.RotationOrScale=i5-1.5708f;k.f[b.D[eJ]]=kP;}else{var gI=k.f[b.ab];gI.Size=new Vector2(j_,b.ba);
gI.Position=new Vector2(h.fP=="left" ? b.bb+b.aD-(j_/2f): b.bb+(j_/2f),b.bt);gI.Color=P;
k.f[b.ab]=gI;}aO(k,b,bf(a8,h),P);}}else if(h.i==e.eg||h.i==e.ec||h.i==e.d3||h.i==e.d6){
if(a_||bl){S=true;double tl=(h.ii||h.I=="percent"||h.I=="pct")? F : iB(h.aM);int p5=b.D.Count;
for(int fp=0;fp<p5;fp++){bool t9=tl>=((fp+0.4)/(double)p5*100.0);Color u8=t9 ? P : al(h,new Color(24,28,36));var p6=k.f[b.D[fp]];
p6.Color=u8;k.f[b.D[fp]]=p6;}aO(k,b,bf(a8,h),P);}}else if(h.i==e.cB){
if(a_||bl){S=true;for(int gQ=0;gQ<b.cC.Length-1;gQ++)b.cC[gQ]=b.cC[gQ+1];b.cC[b.cC.Length-1]=(float)F;
int uF=b.D.Count;for(int e8=0;e8<uF;e8++){float tD=b.cC[e8];float tC=Math.Max(0.06f,Math.Min(1f,tD/100f));float nK=b.ba*tC;
float s0=b.bt+(b.ba/2f)-(nK/2f);var dq=k.f[b.D[e8]];dq.Size=new Vector2(dq.Size.Value.X,nK);
dq.Position=new Vector2(dq.Position.Value.X,s0);dq.Color=P;k.f[b.D[e8]]=dq;}aO(k,b,bf(a8,h),P);
}}else if(h.i==e.c4){if(b.C<0f)b.C=(float)F;float hJ=(float)F;if(Math.Abs(b.C-hJ)>0.05f){
b.C+=(hJ-b.C)*0.35f;S=true;}else if(b.C!=hJ){b.C=hJ;S=true;}if(S||a_){
float oE=b.aD*0.5f;float nF=b.bb+oE;float nW=(float)Math.Max(-1.0,Math.Min(1.0,(b.C-50.0)/50.0));float j0=Math.Abs(nW)*oE;
float tm=(nW>=0)?(nF+j0*0.5f):(nF-j0*0.5f);var gJ=k.f[b.ab];gJ.Size=new Vector2(j0,b.ba);
gJ.Position=new Vector2(tm,b.bt);gJ.Color=P;k.f[b.ab]=gJ;aO(k,b,bf(a8,h),P);
}}else if(h.i==e.cF){if(a_||bl){S=true;bool kr=F>=50.0;float oV=Math.Max(4f,b.ba);float tQ=kr ?(b.bb+b.aD-oV*0.5f):(b.bb+oV*0.5f);
Color oU=kr ?(P!=Color.White ? P : new Color(34,197,94)): new Color(60,70,85);var jY=k.f[b.ab];jY.Position=new Vector2(tQ,b.bt);
jY.Color=oU;k.f[b.ab]=jY;aO(k,b,kr ? "ON" : "OFF",oU);}}else if(h.i==e.cw){if(a_||bl){
S=true;bool kq=F>=50.0;Color nG=kq ?(P!=Color.White ? P : new Color(56,189,248)): new Color(30,36,48);mL(k,b,nG);
aO(k,b,kq ? "ACTIVE" : "READY",kq ? nG : new Color(148,163,184));}}else if(h.i==e.cD){if(a_||bl){
S=true;string vl=F>=75.0 ? "ONLINE" :(F>=25.0 ? "STBY" : "OFFLINE");Color qj=F>=75.0 ? new Color(34,197,94):(F>=25.0 ? new Color(234,179,8): new Color(239,68,68));
mL(k,b,qj);aO(k,b,vl,qj);}}else if(h.i==e.c2){if(a_||bl){S=true;int pD=b.D.Count;
for(int e9=0;e9<pD;e9++){bool ua=F>=((e9+0.5)/(double)pD*100.0);Color uA=ua ? P : al(h,new Color(24,28,36));var pE=k.f[b.D[e9]];
pE.Color=uA;k.f[b.D[e9]]=pE;}aO(k,b,bf(a8,h),P);}}else if(h.i==e.cx){
S=true;float hq;Color eO;string gK;bool tI=(h.I=="producing"||h.I=="active"||h.I=="running"||h.I=="state"||(h.I!="percent"&&(F<=1.0||F==100.0)));
if(tI){if(F>0.0){b.C=(b.C+0.3927f)%6.28318f;hq=b.C;eO=new Color(34,197,94);gK="RUN";}else{hq=0f;
eO=new Color(148,163,184);gK="IDLE";}}else{hq=(F>55.0)? 0f :((F<45.0)? 3.14159f : 1.57079f);eO=(F>55.0)? new Color(34,197,94):((F<45.0)? new Color(239,68,68): new Color(148,163,184));
gK=(F>55.0)? "CHG" :((F<45.0)? "DSC" : "STBY");}var jZ=k.f[b.ab];jZ.RotationOrScale=hq;jZ.Color=eO;
k.f[b.ab]=jZ;aO(k,b,gK,eO);}else{string gF;bu hL;er.TryGetValue(h.aM,out hL);bC.TryGetValue(mz(h.I)? h.aM+"."+h.I :(hL!=null&&hL.bw!=null ? hL.bw : h.aM),out gF);
if(a_||bl||gF!=b.mA){b.mA=gF;S=true;Color lX=P;if(lX==Color.Red&&eA)lX=new Color(255,75,75);aO(k,b,gF ?? bf(a8,h),lX);
}}if(b.mu&&(a_||bl)){S=true;var o4=k.f[b.c9];string uw=ms(F);string uW=bf(a8,h);
o4.Data=h.ac.Replace("{percent}",uw).Replace("{val}",uW);k.f[b.c9]=o4;}}if(S){k.ep++;using(var ts=lK.DrawFrame()){
ts.AddRange(k.f);}}return S;}bool r3(E cm,IMyTextSurface lL){lL.FontSize=cm.c7;var gx=new List<KeyValuePair<string,string>>();
foreach(var o in cm.fZ){double U=0.0;string bp;string bj=null;double kN=o.f0;try{U=a2(o.aM,o.I);if(o.da!=null)kN=a2(o.da,o.ik);
}catch(Exception vZ){gx.Add(new KeyValuePair<string,string>($"{o.ac}: [UNRESOLVED]","red"));continue;}double pN=U;if(kN>0)U=Math.Min(100.0,pN/kN*100.0);
string qN=bf(pN,o);if(o.i==e.dZ){double oh=(o.ii||o.I=="percent"||o.I=="pct")? U : iB(o.aM);
bp=M.rX(o.ac,oh,10,qN);bj=o.bT ? M.fT(o.aN,oh): o.az;}else if(o.i==e.c4){
bp=M.rV(o.ac,U,10);bj=o.bT ? M.fT(o.aN,U): o.az;}else if(o.i==e.cF){
bp=M.r6(o.ac,U);bj=o.az ??(U>=50.0 ? "green" : "gray");}else if(o.i==e.cw){bp=M.r_(o.ac,U);
bj=o.az ??(U>=50.0 ? "cyan" : "gray");}else if(o.i==e.cD){bp=M.r5(o.ac,U);bj=o.az ??(U>=75.0 ? "lime" :(U>=25.0 ? "yellow" : "red"));
}else if(o.i==e.c2){bp=M.rU(o.ac,U,8);bj=o.bT ? M.fT(o.aN,U):(o.az ?? "yellow");
}else if(o.i==e.cx){bool tJ=(o.I=="producing"||o.I=="active"||o.I=="running"||o.I=="state"||(o.I!="percent"&&(U<=1.0||U==100.0)));
if(tJ){char[]vj=new[]{'|','/','-','\\'};char u5=vj[Math.Abs(ex)%4];bp=U>0 ? $"{o.ac} [{u5}] RUN" : $"{o.ac} [==] IDLE";
bj=U>0 ? "green" : "gray";}else{bp=M.r0(o.ac,U,ex);bj=o.az ??(U>55.0 ? "green" :(U<45.0 ? "red" : "cyan"));
}}else{bp=$"{o.ac}: {qN}";bj=o.bv!=null ? M.dc(o.bv,U): null;}gx.Add(new KeyValuePair<string,string>(bp,bj));
}var gr=M.rZ(new List<string>(gx.ConvertAll(vt=>vt.Key)),dV: cm.mT);string qb=string.Join("\n",gr);
string pG;if(cI.TryGetValue(cm,out pG)&&pG==qb)return false;cI[cm]=qb;using(var tt=lL.DrawFrame()){
float q3=4f;float o6=q4*cm.c7;try{var o9=lL.MeasureStringInPixels(new StringBuilder("A"),"Monospace",cm.c7);
if(o9.Y>0)o6=o9.Y;}catch{}for(int dw=0;dw<gr.Count;dw++){string nO=null;if(dw>0&&dw<gr.Count-1)nO=gx[dw-1].Value;
var qi=MySprite.CreateText(gr[dw],"Monospace",J(nO),cm.c7,TextAlignment.LEFT);qi.Position=new Vector2(4f,q3);tt.Add(qi);
q3+=o6;}}return true;}Color J(string g8){if(string.IsNullOrEmpty(g8))return Color.White;Color e2;if(mX.TryGetValue(g8,out e2))return e2;
p eG=p.ei(g8);e2=eG.fX ? Color.Transparent : new Color(eG.bX,eG.bS,eG.bN,eG.hV);mX[g8]=e2;return e2;}public enum bc{q8,ih,d7}
public enum aL{rF,hX,mO,mC,mB}public class bu{public string ag;public bc bU;public List<string>a3=new List<string>();public string mP{
get{return a3.Count>0 ? a3[0]: "";}set{if(a3.Count==0)a3.Add(value);else a3[0]=value;}}public aL dW=aL.rF;
public double fN=0.0;public string h7;public string bw;public bool fE;}public enum e{iG,dZ,eg,cB,ec,d3,d6,c4,cF,cw,cD,c2,cx,f5,fO}
public class ay{public string fQ;public string bR;public bool c8;}public class bx{public e i;public string ac;public string aM;
public string I;public bool bT;public string[]aN;public string az;public List<ay>bv;public string fP="right";
public string eo;public string d8;public List<double>f_;public string fV;public List<ay>h2;public string mc,md;public bool ip;
public int bA;public int iH;public string iu;public int f4;public string d5,h6;public string ee,iK;public double f0;public string da,ik;
public bool ii{get{return f0>0||da!=null;}}}public struct p{public byte bX,bS,bN,hV;public bool fX;public p(byte uN,byte tv,byte sC,byte sk=255){
bX=uN;bS=tv;bN=sC;hV=sk;fX=false;}public static readonly p sg=new p(0,0,0,0){fX=true};public static readonly p f6=new p(255,255,255);
public static readonly p q7=new p(0,0,0);public static readonly p rS=new p(255,0,0);public static readonly p ry=new p(0,255,0);
public static readonly p rC=new p(50,205,50);public static readonly p sj=new p(255,255,0);public static readonly p rG=new p(255,140,0);
public static readonly p rj=new p(0,255,255);public static readonly p q9=new p(0,120,255);public static readonly p rD=new p(255,0,255);
public static readonly p rQ=new p(128,0,128);public static readonly p rx=new p(128,128,128);public static readonly p rk=new p(60,60,60);
public static p ei(string bo){if(string.IsNullOrWhiteSpace(bo))return f6;bo=bo.Trim();if(bo.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bo.Equals("none",StringComparison.OrdinalIgnoreCase))return sg;
if(bo.StartsWith("#")){string cd=bo.Substring(1).Trim();if(cd.Length==6){byte uO=Convert.ToByte(cd.Substring(0,2),16);byte tw=Convert.ToByte(cd.Substring(2,2),16);
byte sD=Convert.ToByte(cd.Substring(4,2),16);return new p(uO,tw,sD);}if(cd.Length==3){byte uP=Convert.ToByte(new string(cd[0],2),16);byte tx=Convert.ToByte(new string(cd[1],2),16);
byte sE=Convert.ToByte(new string(cd[2],2),16);return new p(uP,tx,sE);}}if(bo.Contains(",")){var dF=bo.Split(',');if(dF.Length>=3){byte uQ=(byte)Math.Max(0,Math.Min(255,int.Parse(dF[0].Trim())));
byte ty=(byte)Math.Max(0,Math.Min(255,int.Parse(dF[1].Trim())));byte sF=(byte)Math.Max(0,Math.Min(255,int.Parse(dF[2].Trim())));byte sl=dF.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(dF[3].Trim()))):(byte)255;
return new p(uQ,ty,sF,sl);}}switch(bo.ToLowerInvariant()){case "red": return rS;case "green": return ry;case "lime": return rC;case "yellow": return sj;
case "orange": return rG;case "cyan": return rj;case "blue": return q9;case "magenta": return rD;case "purple": return rQ;case "white": return f6;
case "black": return q7;case "gray": case "grey": return rx;case "darkgray": case "darkgrey": return rk;default: return f6;}}}public class bP{
public string[]bQ;public string d_="vertical";public List<ay>a0;public string hZ,h_;public List<ay>c6;public string ib,id;
}public class aF : bP{public string db="default";public int d4=0;public int ej=0;public int el=1;public int em=1;public string cE;public string iI="left";
public string iJ;public string d1;public string mo;public float? mq;public string c3;public string fJ;public string[]fK;
public string it="horizontal";public List<bx>aq=new List<bx>();}public class E : bP{public string bg;public string cA="standard";
public string c5="graphic";public string d2;public string[]fL;public string mQ="left";public string mR;public string mp="Monospace";
public string bO;public string mg;public int mh=10;public int me=1;public float h4=1.2f;public float ig= -1f,iv= -1f;
public bool fU;public string iw;public string h0;public string mb;public string l9;public string im;public string il;
public string cs;public string cv;public float h5=2.5f;public string mf="cyan";public string[]fH;public List<string>fI=new List<string>();
public List<string>rv(){if(fI!=null&&fI.Count>0)return fI;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float c7=1.0f;public int iz=0;public float hW=1.6f;public int ea=1;public int eb=1;public int mT=30;public string bV;
public int cz=0;public List<aF>ak=new List<aF>();public List<bx>fZ{get{if(ak.Count==0)ak.Add(new aF{db="main"}
);return ak[0].aq;}}}public class b_{public string io;public List<string>f1=new List<string>();public Dictionary<string,string>h8=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>h3=new List<string>();}public class iy{public List<bu>cu=new List<bu>();public List<E>Z=new List<E>();
public List<string>be=new List<string>();public List<string>eq=new List<string>();public Dictionary<string,List<string>>dY=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class mw{static readonly Dictionary<string,b_>mi=new Dictionary<string,b_>(StringComparer.OrdinalIgnoreCase);
static mw(){rT();}static void rT(){bY("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bY(string pi,string[]uu,string sP){var hG=new b_{io=pi}
;foreach(var un in uu){var kA=un.Split(new[]{'='},2);string pu=iq(kA[0]);hG.f1.Add(pu);if(kA.Length>1)hG.h8[pu]=ae(kA[1].Trim());
}var t7=sP.Replace("\r\n","\n").Split('\n');foreach(var tW in t7){string pW=tW.Trim();if(pW.Length>0)hG.h3.Add(pW);}mi[pi]=hG;}static string iq(string dC){
dC=dC.Trim();if(dC.StartsWith("$"))dC=dC.Substring(1);return dC.ToLowerInvariant();}public static iy rI(string s3){var au=new iy();E d=null;
aF u=null;var pF=rO(s3,au.be,au.dY);for(int eV=0;eV<pF.Count;eV++){string a=pF[eV].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){au.cu.Add(rJ(a));}else if(a.StartsWith("@screen")){d=new E{bg=a.Substring("@screen".Length).Trim()}
;au.Z.Add(d);u=null;}else if(a.StartsWith("profile:")){s(d,"profile:");d.cA=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){s(d,"border:");string ng=a.Substring("border:".Length).Trim();if(u!=null)u.d1=ng;
else d.c5=ng;}else if(a.StartsWith("border_color:")){s(d,"border_color:");string ni=a.Substring("border_color:".Length).Trim();
if(u!=null)u.fJ=ni;else d.d2=ni;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
s(d,"border_gradient");string cL=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
cL=cL.Trim();if(cL.StartsWith("(")&&cL.Contains(")"))cL=a1(cL,0);string[]ql=aP(cL).Select(u_=>u_.Trim()).ToArray();if(u!=null)u.fK=ql;
else d.fL=ql;}else if(a.StartsWith("title_align:")){s(d,"title_align:");string qs=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(u!=null)u.iI=qs;else d.mQ=qs;}else if(a.StartsWith("title_color:")){s(d,"title_color:");string qv=a.Substring("title_color:".Length).Trim();
if(u!=null)u.iJ=qv;else d.mR=qv;}else if(a.StartsWith("font_size:")){s(d,"font_size:");float qp=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(u!=null)u.mq=qp;else d.c7=qp;}else if(a.StartsWith("font:")){s(d,"font:");string oa=a.Substring("font:".Length).Trim();
if(u!=null)u.mo=oa;else d.mp=oa;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){s(d,"bg:");string dh=a.Substring(a.IndexOf(':')+1).Trim();
bP ji=u!=null ?(bP)u : d;if(dh.StartsWith("color(")&&dh.EndsWith(")")){string nr;ji.a0=mG(dh.Substring(6,dh.Length-7),out nr);
string np,nq;iC(nr,out np,out nq);ji.hZ=np;ji.h_=nq;}else if(u!=null)u.c3=dh;
else d.bO=dh;}else if(a.StartsWith("layout:")){s(d,"layout:");rK(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
s(d,"alert_period:");d.hW=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){s(d,"refresh:");d.iz=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
s(d,"output:");var hd=aP(a.Substring("output:".Length).Trim());if(hd.Count==0)throw new FormatException("output: requires a target block name");
d.bV=ae(hd[0]);d.cz=hd.Count>1 ? int.Parse(hd[1]): 0;}else if(a.StartsWith("width:")){s(d,"width:");
d.mT=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){s(d,"boot_msg:");
d.mg=ae(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){s(d,"boot_stagger:");d.mh=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){s(d,"boot_calibrate:");string nR=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.h4=nR=="off" ? 0f : Math.Max(0f,float.Parse(nR,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){s(d,"boot_batch:");
d.me=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){s(d,"boot_duration:");
string eI=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool oK=eI.EndsWith("s");if(oK)eI=eI.Substring(0,eI.Length-1).Trim();
float jO;if(float.TryParse(eI,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out jO)){d.h5=oK ? jO : jO/60f;
}}else if(a.StartsWith("boot_log:")){s(d,"boot_log:");d.fI.Add(ae(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("mirror_panel:")){
s(d,"mirror_panel:");d.im=ae(a.Substring("mirror_panel:".Length).Trim());}else if(a.StartsWith("mirror:")){s(d,"mirror:");
d.il=ae(a.Substring("mirror:".Length).Trim());}else if(a.StartsWith("banner:")){s(d,"banner:");d.cs=ae(a.Substring("banner:".Length).Trim());
}else if(a.StartsWith("boot_logo:")){s(d,"boot_logo:");d.cv=ae(a.Substring("boot_logo:".Length).Trim());}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){
s(d,"boot_gradient");d.fH=mH(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){s(d,"bg_gradient");
(u!=null ?(bP)u : d).bQ=mH(a);}else if(a.StartsWith("bg_gradient_dir:")){s(d,"bg_gradient_dir:");
(u!=null ?(bP)u : d).d_=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(a.StartsWith("flash:")){
s(d,"flash:");bP jW=u!=null ?(bP)u : d;string jV=a.Substring("flash:".Length).Trim();string oc=iF(jV);
string ob,od;iC(jV.Substring(0,jV.Length-oc.Length),out ob,out od);jW.ib=ob;jW.id=od;jW.c6=new List<ay>{
new ay{fQ=oc,bR="on"}};}else if(a.StartsWith("bg_texture:")){s(d,"bg_texture:");d.h0=ae(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){s(d,"bg_texture_color:");d.mb=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gap:")||a.StartsWith("padding:")){bool oO=a.StartsWith("gap:");s(d,oO ? "gap:" : "padding:");float pI=float.Parse(a.Substring(a.IndexOf(':')+1).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(oO)d.ig=pI;else d.iv=pI;}else if(a.StartsWith("gradient_mode:")){s(d,"gradient_mode:");d.fU=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){s(d,"screen_bg:");d.bO=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
s(d,"panel_bg:");d.iw=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){s(d,"alert_bg:");
d.l9=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){s(d,"boot_color:");
d.mf=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){s(d,"orientation:");string uk=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(u!=null)u.it=uk;}else if(a.StartsWith("@panel")){s(d,"@panel");u=rM(a);d.ak.Add(u);
}else if(a.StartsWith("title:")){s(d,"title:");if(u==null){u=new aF{db="main",d4=0,ej=0,el=d.ea,em=d.eb}
;d.ak.Add(u);}u.cE=ae(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){s(d,"line:");
if(u==null){u=new aF{db="main",d4=0,ej=0,el=d.ea,em=d.eb};d.ak.Add(u);}u.aq.Add(rL(a.Substring("line:".Length).Trim()));
}else{au.be.Add($"Line {eV + 1}: unrecognized directive: \"{a}\"");}}catch(Exception tf){au.be.Add($"Line {eV + 1}: parse error ({tf.Message}) in \"{a}\"");
}}foreach(var pX in au.Z){if(string.IsNullOrEmpty(pX.bV))au.be.Add($"@screen {pX.bg}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var ax in au.Z){int kX=ax.ak.Count(lb=>lb.c3!=null||lb.a0!=null||lb.bQ!=null);if(ax.ak.Count>1&&kX>0&&kX<ax.ak.Count)au.eq.Add("screen '"+ax.bg+"': only "+kX+" of "+ax.ak.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(ax.cs!=null&& !au.dY.ContainsKey(ax.cs))au.be.Add("Screen '"+ax.bg+"': banner '"+ax.cs+"' is not defined (@banner "+ax.cs+" ... @end)");
if(ax.cv!=null&& !au.dY.ContainsKey(ax.cv))au.be.Add("Screen '"+ax.bg+"': boot_logo '"+ax.cv+"' is not defined (@banner "+ax.cv+" ... @end)");
}return au;}static void rK(string jp,E p3){int k3=jp.IndexOf('(');if(k3<0)throw new FormatException("missing '(' in layout: specification");
string oT=jp.Substring(0,k3).Trim().ToLowerInvariant();string ss=a1(jp,k3);var i7=aP(ss);if(oT=="grid"){if(i7.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
p3.ea=Math.Max(1,int.Parse(i7[0]));p3.eb=Math.Max(1,int.Parse(i7[1]));}else{throw new FormatException($"unknown layout kind '{oT}' (expected grid)");
}}static aF rM(string t3){string fk=t3.Substring("@panel".Length).Trim();int gk=fk.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(gk<0)throw new FormatException("@panel requires an at(...) positioning clause");
string uf=fk.Substring(0,gk).Trim();string nc=fk.Substring(gk);int pA=nc.IndexOf('(');string na=a1(nc,pA);
var cJ=aP(na);if(cJ.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bI=new aF{db=uf,d4=int.Parse(cJ[0]),ej=int.Parse(cJ[1]),el=cJ.Count>2 ? int.Parse(cJ[2]): 1,em=cJ.Count>3 ? int.Parse(cJ[3]): 1}
;int m3=gk+pA+na.Length+1;if(m3<fk.Length){string aW=fk.Substring(m3).Trim();int qA=aW.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(qA>=0){string fs=aW.Substring(qA+5).Trim();if(fs.StartsWith("(")&&fs.Contains(")"))bI.cE=ae(a1(fs,0));else if(fs.StartsWith(":")){
string cM=fs.Substring(1).Trim();if(cM.StartsWith("\"")){int n6=cM.IndexOf('"',1);bI.cE=n6>0 ? cM.Substring(1,n6-1): ae(cM);}else{int qg=cM.IndexOf(' ');
bI.cE=qg>=0 ? cM.Substring(0,qg): cM;}}}string pm=fR(aW,"orientation:");if(pm!=null)bI.it=pm.ToLowerInvariant();string m1=fR(aW,"title_align:");
if(m1!=null)bI.iI=m1.ToLowerInvariant();string qx=fR(aW,"title_color:");if(qx!=null)bI.iJ=qx;int gq=aW.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(gq>=0&& !aW.Substring(gq).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aW.Substring(gq).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bI.d1=fR(aW.Substring(gq),"border:");}int gn=aW.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(gn<0)gn=aW.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(gn>=0){string cb=aW.Substring(gn+"border_gradient".Length).Trim();if(cb.StartsWith(":"))cb=cb.Substring(1).Trim();if(cb.StartsWith("(")&&cb.Contains(")"))cb=a1(cb,0);
bI.fK=aP(cb).Select(u0=>u0.Trim()).ToArray();}}return bI;}static string fR(string pY,string oS){int oJ=pY.IndexOf(oS,StringComparison.OrdinalIgnoreCase);
if(oJ<0)return null;string l0=pY.Substring(oJ+oS.Length).Trim();int qh=l0.IndexOf(' ');return qh>=0 ? l0.Substring(0,qh): l0;}static string[]mH(string kG){
int j6=kG.IndexOfAny(new[]{':','('});string eQ=kG.Substring(kG[j6]==':' ? j6+1 : j6).Trim();if(eQ.StartsWith("(")&&eQ.Contains(")"))eQ=a1(eQ,0);return aP(eQ).Select(u1=>u1.Trim()).ToArray();
}static List<string>rO(string s4,List<string>jT,Dictionary<string,List<string>>sH){var qI=new Dictionary<string,b_>(StringComparer.OrdinalIgnoreCase);
var dJ=(s4 ?? "").Replace("\r\n","\n").Split('\n');var kW=new List<string>();int dx=0;while(dx<dJ.Length){string lg=dJ[dx++];if(lg.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ar=new List<string>();while(dx<dJ.Length){string m_=dJ[dx++].Replace("\t","    ").TrimEnd();if(m_.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ar.Add(m_);}while(ar.Count>0&&ar[0].Length==0)ar.RemoveAt(0);while(ar.Count>0&&ar[ar.Count-1].Length==0)ar.RemoveAt(ar.Count-1);int kp=int.MaxValue;
foreach(var iZ in ar)if(iZ.Length>0)kp=Math.Min(kp,iZ.Length-iZ.TrimStart().Length);for(int e3=0;e3<ar.Count;e3++)if(ar[e3].Length>0)ar[e3]=ar[e3].Substring(kp);
sH[lg.TrimStart().Substring(8).Trim()]=ar;continue;}string cg=mN(lg).Trim();if(cg.Length==0)continue;if(cg.StartsWith("@template")){try{
var lP=rN(cg);while(dx<dJ.Length){string sR=dJ[dx++];string jq=mN(sR).Trim();if(jq.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(jq.Length>0)lP.h3.Add(jq);}qI[lP.io]=lP;}catch(Exception tg){jT.Add($"Template parse error ({tg.Message}) in \"{cg}\"");}}
else if(cg.StartsWith("@use")){try{ml(cg,qI,kW,jT);}catch(Exception th){jT.Add($"@use error ({th.Message}) in \"{cg}\"");
}}else{kW.Add(cg);}}return kW;}static b_ rN(string t4){string lh=t4.Substring("@template".Length).Trim();int k4=lh.IndexOf('(');
if(k4<0)throw new FormatException("missing '(' in @template header");string ug=lh.Substring(0,k4).Trim();string st=a1(lh,k4);var lQ=new b_{
io=ug};foreach(var uo in aP(st)){var kB=uo.Split(new[]{'='},2);string pv=iq(kB[0]);lQ.f1.Add(pv);if(kB.Length>1)lQ.h8[pv]=ae(kB[1].Trim());
}return lQ;}static void ml(string t5,Dictionary<string,b_>qJ,List<string>pr,List<string>te,int nV=0){string ho=t5.Substring("@use".Length).Trim();
int hb=ho.IndexOf('(');string kO=hb>=0 ? ho.Substring(0,hb).Trim(): ho.Trim();string su=hb>=0 ? a1(ho,hb): "";b_ dS=null;
if(!qJ.TryGetValue(kO,out dS)){if(!mi.TryGetValue(kO,out dS))throw new FormatException($"unknown template '{kO}'");}var gt=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var oX in dS.h8)gt[oX.Key]=oX.Value;var m9=aP(su);for(int ez=0;ez<m9.Count;ez++){string gi=m9[ez];int jR=gi.IndexOf('=');if(jR>=0){
string tO=iq(gi.Substring(0,jR));string vF=ae(gi.Substring(jR+1).Trim());gt[tO]=vF;}else{if(ez<dS.f1.Count){string tP=dS.f1[ez];
gt[tP]=ae(gi.Trim());}}}foreach(var sQ in dS.h3){string eM=sQ;foreach(var g3 in gt){eM=eM.Replace("$"+g3.Key,g3.Value).Replace("{"+g3.Key+"}",g3.Value);
}if(eM.StartsWith("@use")){if(nV>3)throw new FormatException("@use nested too deep");ml(eM,qJ,pr,te,nV+1);
}else pr.Add(eM);}}static void s(E s2,string tb){if(s2==null)throw new FormatException($"'{tb}' used before any @screen declared");
}static string mN(string bm){if(string.IsNullOrEmpty(bm))return bm;bool ko=false;for(int ce=0;ce<bm.Length;ce++){char jv=bm[ce];if(jv=='"'){ko= !ko;
}else if(!ko){if(jv=='/'&&ce+1<bm.Length&&bm[ce+1]=='/'){return bm.Substring(0,ce);}if(jv=='#'){if(rA(bm,ce)){continue;}return bm.Substring(0,ce);}}}return bm;
}static bool rA(string cS,int oM){int dA=0;for(int kk=oM+1;kk<cS.Length;kk++){char dl=cS[kk];if((dl>='0'&&dl<='9')||(dl>='a'&&dl<='f')||(dl>='A'&&dl<='F'))dA++;
else break;}if(dA==3||dA==4||dA==6||dA==8){int e7=oM+1+dA;if(e7>=cS.Length||char.IsWhiteSpace(cS[e7])||cS[e7]==')'||cS[e7]==','||cS[e7]==':')return true;
}return false;}static bu rJ(string t6){string li=t6.Substring("@bind".Length).Trim();int jS=li.IndexOf('=');if(jS<0)throw new FormatException("missing '=' in @bind");
string pj=li.Substring(0,jS).Trim();string jU=li.Substring(jS+1).Trim();int k5=jU.IndexOf('(');if(k5<0)throw new FormatException("missing '(' in @bind target");
string gN=jU.Substring(0,k5).Trim().ToLowerInvariant();string sv=a1(jU,k5);var b5=aP(sv);if(b5.Count==0)throw new FormatException("@bind target has no arguments");
var av=new bu{ag=pj};if(gN=="dynamic"||gN=="var"||gN=="mem"){av.bU=bc.d7;av.bw=b5.Count>0 ? ae(b5[0].Trim()): pj;
for(int i_=0;i_<b5.Count;i_++){var g4=b5[i_].Split(new[]{':'},2);if(g4.Length==2&&g4[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double nU;
if(double.TryParse(g4[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nU))av.fN=nU;else av.h7=ae(g4[1].Trim());
}}return av;}av.bU=gN=="group" ? bc.ih : bc.q8;string gh=null;for(int gg=0;gg<b5.Count;gg++){var dz=b5[gg].Split(new[]{':'},2);if(dz.Length==2&&dz[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
gh=dz[1].Trim().ToLowerInvariant();}else if(dz.Length==2&&dz[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){av.fE=dz[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{av.a3.Add(ae(b5[gg].Trim()));}}if(av.a3.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(av.bU==bc.ih){if(gh==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(gh){case "avg": av.dW=aL.hX;break;case "sum": av.dW=aL.mO;break;case "min": av.dW=aL.mC;break;case "max": av.dW=aL.mB;
break;default: throw new FormatException($"unknown agg '{gh}' (expected avg/sum/min/max)");}}return av;}static bx rL(string gp){var l=new bx();
if(!gp.StartsWith("\""))throw new FormatException("line must start with a quoted label");int jQ=gp.IndexOf('"',1);if(jQ<0)throw new FormatException("unterminated quoted label");
l.ac=gp.Substring(1,jQ-1);string fj=gp.Substring(jQ+1).Trim();int hc=fj.IndexOf('(');if(hc<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string y=fj.Substring(0,hc).Trim().ToLowerInvariant();string sr=a1(fj,hc);string ai=fj.Substring(fj.IndexOf(')',hc)+1).Trim();
var jM=sr.Trim().Split('.');if(jM.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
l.aM=jM[0].Trim();l.I=jM[1].Trim();if(y=="bar")l.i=e.dZ;else if(y=="led")l.i=e.eg;else if(y=="spark"||y=="sparkline")l.i=e.cB;
else if(y=="hazard"||y=="stripes")l.i=e.ec;else if(y=="chevron"||y=="chevrons")l.i=e.d3;else if(y=="dots"||y=="dot")l.i=e.d6;
else if(y=="balance"||y=="horizon"||y=="centered")l.i=e.c4;else if(y=="toggle"||y=="switch")l.i=e.cF;else if(y=="checkbox"||y=="check")l.i=e.cw;
else if(y=="status"||y=="badge"||y=="state")l.i=e.cD;else if(y=="ammo"||y=="pips")l.i=e.c2;else if(y=="flow"||y=="rate"||y=="spinner")l.i=e.cx;
else if(y=="text")l.i=e.iG;else if(y=="tank"||y=="tube"||y=="bucket")l.i=e.f5;else if(y=="dial"||y=="gauge"||y=="arc")l.i=e.fO;
else throw new FormatException($"unknown display function '{y}'");while(ai.Length>0){int jJ=ai.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int du=ai.IndexOf('(');if(jJ>=0&&(du<0||jJ<du)){string gA=ai.Substring(jJ+4).Trim();int hx=gA.IndexOf(' ');l.fP=(hx>=0 ? gA.Substring(0,hx): gA).ToLowerInvariant();
ai=(hx>=0 ? gA.Substring(hx): "").Trim();continue;}int lZ=ai.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(lZ>=0&&(du<0||lZ<du)){
string hR=ai.Substring(lZ+5).Trim();int hy=hR.IndexOf(' ');l.eo=(hy>=0 ? hR.Substring(0,hy): hR).Trim().Trim('"');ai=(hy>=0 ? hR.Substring(hy): "").Trim();
continue;}int j1=ai.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(j1>=0&&(du<0||j1<du)){string gH=ai.Substring(j1+7).Trim();
int hz=gH.IndexOf(' ');l.d8=(hz>=0 ? gH.Substring(0,hz): gH).Trim().Trim('"');ai=(hz>=0 ? gH.Substring(hz): "").Trim();continue;}int g7=ai.IndexOf('(');
if(g7<0)break;string N=ai.Substring(0,g7).Trim().ToLowerInvariant();int o3=N.LastIndexOf(' ');if(o3>=0)N=N.Substring(o3+1).Trim();
string _=a1(ai,g7);ai=ai.Substring(ai.IndexOf(')',g7)+1).Trim();if(N=="gradient"){l.bT=true;l.aN=aP(_).Select(u2=>u2.Trim()).ToArray();
if(l.aN.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(N=="size"){string lO=_.Trim().ToLowerInvariant();
l.f4=lO=="large"||lO=="fill" ? 2 : lO=="medium" ? 1 : 0;}else if(N=="marker"){l.f_=new List<double>();foreach(var uc in aP(_)){double pe;
if(!double.TryParse(uc,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out pe))throw new FormatException("marker() takes numbers from 0 to 100");
l.f_.Add(pe);}}else if(N=="icon")l.fV=_.Trim();else if(N=="blink"){string jt=_.Trim(),nj=iF(jt);iC(jt.Substring(0,jt.Length-nj.Length),out l.mc,out l.md);
l.h2=new List<ay>{new ay{fQ=nj,bR="on"}};}else if(N=="value")l.ip=_.Trim().Equals("off",StringComparison.OrdinalIgnoreCase);
else if(N=="ticks")l.iH=int.Parse(_.Trim());else if(N=="segments"||N=="points")l.bA=Math.Max(2,Math.Min(40,int.Parse(_.Trim())));else if(N=="outline")l.iu=_.Trim();
else if(N=="color_by"){var jA=_.Trim().Split('.');if(jA.Length!=2)throw new FormatException("color_by() takes bindname.field");l.d5=jA[0].Trim();
l.h6=jA[1].Trim();}else if(N=="label_color")l.ee=_.Trim();else if(N=="track")l.iK=_.Trim();else if(N=="flat"){l.az=_.Trim();
}else if(N=="pattern"){string W=_.Trim().ToLowerInvariant();if(W=="hazard"||W=="stripes")l.i=e.ec;else if(W=="chevron"||W=="chevrons")l.i=e.d3;
else if(W=="dots"||W=="dot")l.i=e.d6;else if(W=="led")l.i=e.eg;else if(W=="balance"||W=="horizon")l.i=e.c4;
else if(W=="toggle"||W=="switch")l.i=e.cF;else if(W=="checkbox"||W=="check")l.i=e.cw;else if(W=="status"||W=="badge"||W=="state")l.i=e.cD;
else if(W=="ammo"||W=="pips")l.i=e.c2;else if(W=="flow"||W=="rate"||W=="spinner")l.i=e.cx;}else if(N=="color"){string tH;
l.bv=mG(_,out tH);}else if(N=="dir"||N=="direction"){l.fP=_.Trim().ToLowerInvariant();}else if(N=="unit"){
l.eo=_.Trim().Trim('"');}else if(N=="max"){double pd;if(double.TryParse(_.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out pd))l.f0=pd;
else{var kL=_.Trim().Split('.');if(kL.Length!=2)throw new FormatException("max() takes a number or bindname.field");l.da=kL[0].Trim();l.ik=kL[1].Trim();
}}else if(N=="format"||N=="fmt"){l.d8=_.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{N}'");}}return l;}static string a1(string ht,int kU){
int jI=0;for(int eW=kU;eW<ht.Length;eW++){if(ht[eW]=='(')jI++;else if(ht[eW]==')'){jI--;if(jI==0)return ht.Substring(kU+1,eW-kU-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aP(string sw){return sw.Split(',').Select(u3=>u3.Trim()).Where(u4=>u4.Length>0).ToList();
}static List<ay>mG(string sq,out string jX){var hr=new List<ay>();jX=null;foreach(var k6 in aP(sq)){int jE=k6.LastIndexOf(':');
if(jE<0)throw new FormatException("color() entries need 'expr : color'");string g5=k6.Substring(0,jE).Trim();string nN=k6.Substring(jE+1).Trim();
if(g5.Equals("default",StringComparison.OrdinalIgnoreCase)){hr.Add(new ay{c8=true,bR=nN});}else{string qm=iF(g5);
if(jX==null)jX=g5.Substring(0,g5.Length-qm.Length).Trim();hr.Add(new ay{fQ=qm,bR=nN});}}if(!hr.Any(uR=>uR.c8))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return hr;}static void iC(string uS,out string sN,out string tk){var fa=(uS ?? "").Split('.');if(fa.Length!=2||fa[0].Trim().Length==0||fa[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
sN=fa[0].Trim();tk=fa[1].Trim();}static string iF(string ds){ds=ds.Trim();foreach(var ul in new[]{">=","<=","==",">","<"}){int kl=ds.IndexOf(ul,StringComparison.Ordinal);
if(kl>0)return ds.Substring(kl).Trim();if(kl==0)return ds;}throw new FormatException($"color() rule has no recognizable comparison operator: '{ds}'");}
static string ae(string cT){if(cT.Length>=2&&cT[0]=='"'&&cT[cT.Length-1]=='"')return cT.Substring(1,cT.Length-2);return cT;}}public static class q6{public static double q5(aL sm,List<double>dU){
if(dU==null||dU.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(sm){case aL.hX: return dU.Average();case aL.mO: return dU.Sum();
case aL.mC: return dU.Min();case aL.mB: return dU.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class M{
public static string rX(string tY,double he,int nh=10,string qR=null){he=Math.Max(0,Math.Min(100,he));int oi=(int)Math.Round(nh*(he/100.0));
string sI=new string('#',oi)+new string('-',nh-oi);string vG=qR!=null ? qR : $"{he:0}%";return $"{tY} [{sI}] {vG}";
}public static string mr(double cr,string lY=null,string gM=null){if(lY=="auto"){double i0=Math.Abs(cr);if(i0>=1e9)return(cr/1e9).ToString("F1")+"G";
if(i0>=1e6)return(cr/1e6).ToString("F1")+"M";if(i0>=1e3)return(cr/1e3).ToString("F1")+"k";string ti= !string.IsNullOrEmpty(gM)? gM : "F0";return cr.ToString(ti);
}string ok= !string.IsNullOrEmpty(gM)? gM :(Math.Abs(cr)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(lY)){return cr.ToString(ok);}return cr.ToString(ok)+lY;
}public static float rw(string uH,float qn,float lM,float gD=1.0f){string dD=(uH ?? "standard").ToLowerInvariant();
if(dD=="compact"||(dD=="auto"&&qn<=300&&lM<=300&&lM>160)){return gD*0.75f;}if(dD=="corner"||(dD=="auto"&&lM<=160)){
return gD*0.55f;}if(dD=="wide"||(dD=="auto"&&qn>=1000)){return gD*1.15f;}return gD;}public static string rV(string tZ,double hf,int cc=5){
hf=Math.Max(0,Math.Min(100,hf));double gE=(hf-50.0)/50.0;int e4=0;int fm=0;if(gE< -0.01)e4=(int)Math.Round(-gE*cc);
else if(gE>0.01)fm=(int)Math.Round(gE*cc);if(e4>cc)e4=cc;if(fm>cc)fm=cc;string t1=new string('-',cc-e4)+new string('<',e4);
string uY=new string('>',fm)+new string('-',cc-fm);return $"{tZ} [{t1}|{uY}] {hf:0}%";}public static string r6(string o0,double vI){
return vI>=50.0 ? $"{o0} [====O] ON" : $"{o0} [O====] OFF";}public static string r_(string o1,double vJ){return vJ>=50.0 ? $"{o1} [X] ACTIVE" : $"{o1} [ ] READY ";
}public static string r5(string kD,double qP){if(qP>=75.0)return $"{kD} [ ONLINE  ]";if(qP>=25.0)return $"{kD} [ STANDBY ]";return $"{kD} [ OFFLINE ]";
}public static string rU(string t_,double hg,int o8=8){hg=Math.Max(0,Math.Min(100,hg));int oj=(int)Math.Round(o8*(hg/100.0));
string uB=new string('|',oj)+new string('.',o8-oj);return $"{t_} [{uB}] {hg:0}%";}public static string r0(string kE,double fe,int qz=0){
char[]vi=new[]{'|','/','-','\\'};char pZ=vi[(qz>=0 ? qz : 0)%4];if(fe>55.0)return $"{kE} {pZ} ▲ CHG {fe:0}%";if(fe<45.0)return $"{kE} {pZ} ▼ DSC {fe:0}%";
return $"{kE} ● STBY {fe:0}%";}public static bool rE(string aB,string e0){if(aB==null||e0==null)return false;if(aB=="*"||aB==e0)return true;
if(!aB.Contains("*")&& !aB.Contains("?"))return string.Equals(aB,e0,StringComparison.OrdinalIgnoreCase);int aI=0,eX=0,lC= -1,kK=0;while(eX<e0.Length){
if(aI<aB.Length&&(aB[aI]=='?'||char.ToLowerInvariant(aB[aI])==char.ToLowerInvariant(e0[eX]))){aI++;eX++;}else if(aI<aB.Length&&aB[aI]=='*'){
lC=aI;kK=eX;aI++;}else if(lC!= -1){aI=lC+1;kK++;eX=kK;}else{return false;}}while(aI<aB.Length&&aB[aI]=='*')aI++;return aI==aB.Length;
}public static int ra(double up,int qE){return Math.Max(0,Math.Min(qE,1+(int)(Math.Max(0.0,up)*qE)));}public static string fT(string[]dR,double k7){
if(dR==null||dR.Length==0)return "white";k7=Math.Max(0,Math.Min(100,k7));int km=(int)Math.Floor(k7/100.0*dR.Length);if(km>=dR.Length)km=dR.Length-1;
return dR[km];}public static p mt(string[]cp,double ux){if(cp==null||cp.Length==0)return p.f6;if(cp.Length==1)return p.ei(cp[0]);
double uV=Math.Max(0.0,Math.Min(1.0,ux/100.0));double p2=uV*(cp.Length-1);int kn=(int)Math.Floor(p2);int tG=Math.Min(cp.Length-1,kn+1);
double lG=p2-kn;p dm=p.ei(cp[kn]);p jw=p.ei(cp[tG]);byte uT=(byte)Math.Round(dm.bX+(jw.bX-dm.bX)*lG);
byte tz=(byte)Math.Round(dm.bS+(jw.bS-dm.bS)*lG);byte sG=(byte)Math.Round(dm.bN+(jw.bN-dm.bN)*lG);return new p(uT,tz,sG);}public static string dc(List<ay>pV,double vK){
foreach(var lj in pV){if(lj.c8)continue;if(rs(lj.fQ,vK))return lj.bR;}var nT=pV.FirstOrDefault(uU=>uU.c8);return nT!=null ? nT.bR : null;
}static bool rs(string eN,double fy){eN=eN.Trim();foreach(var kT in new[]{">=","<=","==",">","<"}){if(eN.StartsWith(kT)){double fl=double.Parse(eN.Substring(kT.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(kT){case ">=": return fy>=fl;case "<=": return fy<=fl;case "==": return Math.Abs(fy-fl)<1e-9;case ">": return fy>fl;case "<": return fy<fl;
}}}throw new FormatException($"unsupported comparison expression: '{eN}'");}public static List<string>rZ(List<string>s1,int dV){
var g9=new List<string>();g9.Add("+"+new string('-',dV+2)+"+");foreach(var kH in s1){string vB=kH.Length>dV ? kH.Substring(0,dV): kH;
string us=vB.PadRight(dV);g9.Add("| "+us+" |");}g9.Add("+"+new string('-',dV+2)+"+");return g9;}public struct mF{public float mV,mW,mU,mv;
}public static mF rg(int nJ,int pR,int hA,int hB,int fu,int fv,float vs,float vr,float pw,float px,float j8,float j9){
fu=Math.Max(1,fu);fv=Math.Max(1,fv);hA=Math.Max(1,Math.Min(fu-nJ,hA));hB=Math.Max(1,Math.Min(fv-pR,hB));
float sz=vs-(pw*2f);float sy=vr-(px*2f);float nE=(sz-(j8*(fu-1)))/fu;float nD=(sy-(j9*(fv-1)))/fv;
float uJ=pw+nJ*(nE+j8);float uK=px+pR*(nD+j9);float uI=hA*nE+((hA-1)*j8);float uz=hB*nD+((hB-1)*j9);return new mF{
mV=uJ,mW=uK,mU=uI,mv=uz};}public struct mK{public int rH;public int ij;public int ah;public int aE;public int en;
public int bd;public int se;public int sf;public int rp;public int rq;
public double rr;}public static mK rh(E cn){var n=new mK();var pz=cn.ak!=null&&cn.ak.Count>0 ? cn.ak : new List<aF>{
new aF{aq=cn.fZ}};n.rH=pz.Count;n.ij=0;n.ah=0;n.aE=0;if(!string.IsNullOrEmpty(cn.bO)&& !cn.bO.Equals("none",StringComparison.OrdinalIgnoreCase))n.ah++;
if(!string.IsNullOrEmpty(cn.c5)&& !cn.c5.Equals("none",StringComparison.OrdinalIgnoreCase))n.ah+=2;foreach(var fb in pz){
if(!string.IsNullOrEmpty(fb.c3)&& !fb.c3.Equals("none",StringComparison.OrdinalIgnoreCase))n.ah++;if(!string.IsNullOrEmpty(fb.cE))n.ah+=2;
n.ij+=fb.aq.Count;foreach(var tX in fb.aq){n.se++;n.sf++;n.ah+=2;switch(tX.i){case e.dZ: n.en++;
n.bd++;n.ah+=2;n.aE++;break;case e.eg: n.bd+=10;n.ah+=10;
n.aE+=10;break;case e.ec: n.bd+=10;n.ah+=11;n.aE+=10;break;case e.d3: n.bd+=10;
n.ah+=10;n.aE+=10;break;case e.d6: n.bd+=12;n.ah+=12;n.aE+=12;
break;case e.cB: n.en++;n.bd+=8;n.ah+=9;n.aE+=8;break;case e.c4: n.en+=2;
n.bd++;n.ah+=3;n.aE++;break;case e.cF: case e.cw: n.en++;
n.bd++;n.ah+=2;n.aE++;break;case e.cD: case e.cx: n.en++;n.bd+=2;
n.ah+=3;n.aE+=2;break;case e.c2: n.bd+=8;n.ah+=8;n.aE+=8;
break;default: n.aE++;break;}}}n.rp=25+(n.ij*2);n.rq=180+(n.aE*4);
n.rr=0.0;return n;}}