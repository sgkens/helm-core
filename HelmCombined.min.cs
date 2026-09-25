ix t;string iR=null;Dictionary<string,List<IMyTerminalBlock>>b1=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>f9=new Dictionary<string,bool>();
Dictionary<string,bu>ep=new Dictionary<string,bu>();Dictionary<string,Color>m_=new Dictionary<string,Color>();List<IMyTerminalBlock>cG=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>dd=new List<IMyTerminalBlock>();bool f6;int iO;string eq="";StringBuilder et=new StringBuilder();readonly Dictionary<string,double>aQ=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>bC=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener er;bool cH=false;
Dictionary<F,IMyTextSurface>b2=new Dictionary<F,IMyTextSurface>();Dictionary<F,string>cI=new Dictionary<F,string>();
static readonly string[]mG=new string[101];static Program(){for(int gQ=0;gQ<=100;gQ++){mG[gQ]=gQ+"%";}}static string mt(double v0){int eX=(int)Math.Round(v0);
if(eX<0)eX=0;else if(eX>100)eX=100;return mG[eX];}class aA{public List<MySprite>f=new List<MySprite>();public List<cz>ei=new List<cz>();
public int cu= -1;public Color[]fD;public Color fA;public bool fB;public List<iw>bX=new List<iw>();public Vector2 ed,iq;
public int en;public bool eb=false;public List<fE>dZ=new List<fE>();}class iw{public string mE;public int iC;public Vector2 mK,mP;
}class fE{public int fU;public Color hX,fK;public List<ay>f2,fQ;public string h0,h8,h9,ib;public bool mz,id;
}class cz{public bx ef;public int aa= -1;public int a4= -1;public int c8= -1;public bool mv=false;
public List<int>E=new List<int>();public float[]cC=new float[8];public float bb;public float aD;public float ba;
public float bt;public double fW= -99999.0;public string mB;public float D= -1f;public bool mV;public float mj=12f;}Dictionary<F,aA>b3=new Dictionary<F,aA>();
int m1=0;double iV=0;double eu=0;double T=0;int es=0;Dictionary<F,double>bB=new Dictionary<F,double>();
Dictionary<F,double>iN=new Dictionary<F,double>();Dictionary<string,int>gd=new Dictionary<string,int>();List<string>de=new List<string>();
Dictionary<string,int>ge=new Dictionary<string,int>();double iX,iU;HashSet<string>gb=new HashSet<string>();Dictionary<string,double[]>iT=new Dictionary<string,double[]>();
Dictionary<string,double>ew=new Dictionary<string,double>();List<MyInventoryItem>iQ=new List<MyInventoryItem>();int b4= -1,gc;double iW;
Dictionary<F,double>f7=new Dictionary<F,double>();int ev=0;const string rL="0.38.3";IMyTextSurface a5;bool iM=true;
double f8=0;string m0="";string iS="";int ga=0,iP=0;List<KeyValuePair<string,int>>dc=new List<KeyValuePair<string,int>>();
const double r2=120;const float rf=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{er=IGC.RegisterBroadcastListener("HELM");
er.SetMessageCallback("HELM_IGC");}catch{}f1();iD();my();}public void Main(string ne,UpdateType vX){
double n6=Runtime.TimeSinceLastRun.TotalSeconds;T+=n6;if((vX&UpdateType.IGC)!=0||ne=="HELM_IGC"){r0();}string pW=(ne ?? "").Trim();
string am=pW.ToLowerInvariant();if(am=="reload"){f1();}else if(am=="refresh"){iz();}else if(am=="boot"){iD();}else if(am=="probe"){
eu=T+20.0;rz();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(am=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=sp;
f1();iD();}else if(am=="console on"||am=="console off"){iM=am=="console on";my();f8=0;}else if(am=="names"){if(Me.CustomData.Contains("# --- names")){
Echo("HELM: the names are already at the end of Custom Data. Delete that block to list them again.");return;}var e4=new List<string>();foreach(var uu in dV())e4.Add(uu.CustomName);
var pp=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(pp);foreach(var uv in pp)e4.Add("[group] "+uv.Name);e4.Sort();Me.CustomData+="\n# --- names (delete when done) ---\n# "+string.Join("\n# ",e4);
Echo("HELM: "+e4.Count+" names written to the end of Custom Data.");return;}else if(am=="scan"||am=="diagnose"||am=="test"){sl();return;}else if(am.StartsWith("set ")){
string hm=pW.Substring(4).Trim();int oa=hm.IndexOf('=');int vv=hm.IndexOf(' ');int lv=oa>0 ? oa : vv;if(lv>0){string oV=hm.Substring(0,lv).Trim();
string qV=hm.Substring(lv+1).Trim();ma($"{oV}={qV}");Echo($"[HELM] Set dynamic variable '{oV}' = {qV}");}return;}else if(am.StartsWith("alert")){
string fu=am.Length>5 ? am.Substring(5).Trim(): "toggle";if(fu=="toggle")cH= !cH;else cH=fu=="red"||fu=="on"||fu=="1"||fu=="true";
Echo($"[HELM] Red alert state: {cH}");return;}else if(Me.CustomData!=iR){f1();}ev++;iV+=n6;if(iV>=r2){
iV=0;iz();}if(eu>0){if(T<eu){iL();return;}eu=0;b3.Clear();cI.Clear();
f8=0;}if(t==null||t.Z.Count==0){iL();mk();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cT=t.Z[m1%t.Z.Count];
m1++;iS=cT.bg;bool i0=bB.Count>0;if(i0)r9();double n7;if(!bB.ContainsKey(cT)&& !(iN.TryGetValue(cT,out n7)&&T<n7)){
if(se(cT))i0=true;if(cT.iy>0)iN[cT]=T+cT.iy/60.0;}es=i0 ? 0 : es+1;Runtime.UpdateFrequency=es<=t.Z.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
so();sn();sm();iL();mk();ga=Runtime.CurrentInstructionCount;if(ga>iP)iP=ga;}void mk(){
if(eq.Length>0)Echo(eq);if(et.Length>0)Echo(et.ToString());}List<IMyTerminalBlock>dV(bool sz=false){if(!f6){dd.Clear();
GridTerminalSystem.GetBlocks(dd);cG.Clear();cG.AddRange(dd);cG.RemoveAll(sM=> !sM.IsSameConstructAs(Me));iO=dd.Count-cG.Count;
f6=true;}return sz ? dd : cG;}IMyTerminalBlock mn(string pl,bool sB=false){var ja=GridTerminalSystem.GetBlockWithName(pl);
if(ja==null||sB||ja.IsSameConstructAs(Me))return ja;var i2=dV();for(int gR=0;gR<i2.Count;gR++)if(i2[gR].CustomName.IndexOf(pl,StringComparison.OrdinalIgnoreCase)>=0)return i2[gR];
return null;}void rq(string pm,List<IMyTerminalBlock>e_,bool m7=false){var ow=GridTerminalSystem.GetBlockGroupWithName(pm);if(ow==null)return;int np=e_.Count;
var fr=new List<IMyTerminalBlock>();ow.GetBlocks(fr);l8(fr,e_,m7);if(e_.Count>np)return;var oH=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(oH);
foreach(var q9 in oH){if(q9.Name.IndexOf(pm,StringComparison.OrdinalIgnoreCase)<0)continue;fr.Clear();q9.GetBlocks(fr);l8(fr,e_,m7);if(e_.Count>np)return;
}}void l8(List<IMyTerminalBlock>tG,List<IMyTerminalBlock>oS,bool sA=false){foreach(var jb in tG)if((sA||jb.IsSameConstructAs(Me))&& !oS.Contains(jb))oS.Add(jb);
}void f1(){iR=Me.CustomData;t=mx.rU(iR);eq="";if(t.be.Count>0){var p9=new StringBuilder("HELM config errors:\n");
foreach(var tp in t.be)p9.AppendLine(" - "+tp);eq=p9.ToString();}if(t.eo.Count>0){var q7=new StringBuilder("HELM notes:\n");foreach(var v5 in t.eo)q7.AppendLine(" - "+v5);
eq+=q7.ToString();}ep.Clear();foreach(var jc in t.cv)if(!ep.ContainsKey(jc.ag))ep[jc.ag]=jc;b3.Clear();cI.Clear();
bB.Clear();iN.Clear();f7.Clear();rF();iz();}void r0(){try{while(er!=null&&er.HasPendingMessage){
var pg=er.AcceptMessage();if(pg.Data is string){ma((string)pg.Data);}}}catch{}}void ma(string pJ){
if(string.IsNullOrEmpty(pJ))return;var pF=pJ.Split(';');for(int kc=0;kc<pF.Length;kc++){var fb=pF[kc].Trim();if(fb.Length==0)continue;int eJ=fb.IndexOf('=');
if(eJ<0)eJ=fb.IndexOf(':');if(eJ>0){string dx=fb.Substring(0,eJ).Trim();string br=fb.Substring(eJ+1).Trim();if(dx.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cH=br.Equals("red",StringComparison.OrdinalIgnoreCase)||br=="1"||br.Equals("true",StringComparison.OrdinalIgnoreCase)||br.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double pr;bC.Remove(dx);if(double.TryParse(br,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out pr)){
aQ[dx]=pr;}else if(br.Equals("true",StringComparison.OrdinalIgnoreCase)||br.Equals("on",StringComparison.OrdinalIgnoreCase)){aQ[dx]=100.0;
}else if(br.Equals("false",StringComparison.OrdinalIgnoreCase)||br.Equals("off",StringComparison.OrdinalIgnoreCase)){aQ[dx]=0.0;}else{bC[dx]=br;
}}}}}const string sp="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void rz(){var i3=new List<IMyTextSurface>();if(a5!=null)i3.Add(a5);foreach(var uw in b2.Values)i3.Add(uw);foreach(var vq in i3)ry(vq);
}static void bz(List<MySprite>vw,float ti,float tj,float v6,float tN,Color s5){vw.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(ti,tj),Size=new Vector2(v6,tN),Color=s5}
);}void ry(IMyTextSurface lw){Vector2 aX=lw.SurfaceSize,aK=lw.TextureSize,ap=(aK-aX)/2f;var aJ=new List<MySprite>();Color gN=new Color(150,150,150);
bz(aJ,aK.X/2f,2f,aK.X,4f,gN);bz(aJ,aK.X/2f,aK.Y-2f,aK.X,4f,gN);bz(aJ,2f,aK.Y/2f,4f,aK.Y,gN);bz(aJ,aK.X-2f,aK.Y/2f,4f,aK.Y,gN);
bz(aJ,ap.X+aX.X/2f,ap.Y+aX.Y/2f,aX.X,aX.Y,new Color(20,40,90));float ao=32f;bz(aJ,ap.X+ao/2f,ap.Y+ao/2f,ao,ao,new Color(255,0,0));bz(aJ,ap.X+aX.X-ao/2f,ap.Y+ao/2f,ao,ao,new Color(0,255,0));
bz(aJ,ap.X+ao/2f,ap.Y+aX.Y-ao/2f,ao,ao,new Color(255,255,0));bz(aJ,ap.X+aX.X-ao/2f,ap.Y+aX.Y-ao/2f,ao,ao,new Color(255,0,255));float l6=ap.Y+aX.Y/2f-30f;
a9(aJ,"SURFACE "+aX.X.ToString("0")+"x"+aX.Y.ToString("0"),Color.White,ap.X+44f,l6,0.9f);a9(aJ,"TEXTURE "+aK.X.ToString("0")+"x"+aK.Y.ToString("0"),Color.White,ap.X+44f,l6+24f,0.9f);
a9(aJ,"OFFSET  "+ap.X.ToString("0")+","+ap.Y.ToString("0"),Color.White,ap.X+44f,l6+48f,0.9f);using(var tz=lw.DrawFrame()){tz.AddRange(aJ);}}static bool mA(string bE){
return bE=="rate"||bE=="eta"||bE=="trend"||bE.Length>3&&(bE.StartsWith("top")&&char.IsDigit(bE[3])||bE.Length>4&&bE.StartsWith("line")&&char.IsDigit(bE[4]));}void rF(){
gd.Clear();de.Clear();ge.Clear();gb.Clear();iT.Clear();b4= -1;iW=iX=iU=0;foreach(var lr in t.Z){var kH=new List<List<bx>>();
foreach(var uT in lr.ak)kH.Add(uT.aq);if(lr.ak.Count==0)kH.Add(lr.fX);foreach(var uo in kH)foreach(var bF in uo){if(!mA(bF.J))continue;
if(bF.J=="rate"||bF.J=="eta"||bF.J=="trend"){gb.Add(bF.aM);continue;}bool qO=bF.J[0]=='t';var n_=qO ? gd : ge;int uq=int.Parse(bF.J.Substring(qO ? 3 : 4));
int oL;n_.TryGetValue(bF.aM,out oL);n_[bF.aM]=Math.Max(oL,uq);}}de.AddRange(gd.Keys);}void sn(){if(ge.Count==0||T<iX)return;
iX=T+2.0;foreach(var ky in ge){List<IMyTerminalBlock>jk;var qi=b1.TryGetValue(ky.Key,out jk)&&jk.Count>0 ? jk[0]as IMyTextSurface : null;
string[]pc=qi==null ? new string[0]: qi.GetText().Split('\n');for(int ff=1;ff<=ky.Value;ff++)bC[ky.Key+".line"+ff]=ff<=pc.Length ? pc[ff-1].TrimEnd('\r'): "";}}
void sm(){if(gb.Count==0||T<iU)return;iU=T+5.0;foreach(var di in gb){double hR=a2(di,"percent"),ck=0.0;double[]gO;
if(iT.TryGetValue(di,out gO))ck=gO[2]*0.6+(hR-gO[0])/Math.Max(0.1,T-gO[1])*0.4;iT[di]=new[]{hR,T,ck};double fd=ck*60.0;int cY=(int)Math.Min(359999.0,(ck<0 ? hR/ -ck : ck>0 ?(100.0-hR)/ck : 0.0)+0.5);
bC[di+".rate"]=(fd>=0 ? "+" : "")+fd.ToString("0.0")+"%/min";bC[di+".trend"]=fd>0.05 ? "^" : fd< -0.05 ? "v" : "=";bC[di+".eta"]=Math.Abs(fd)<0.05 ? "--" :(ck<0 ? "empty " : "full ")+(cY>=3600 ? cY/3600+"h "+cY%3600/60+"m" : cY>=60 ? cY/60+"m "+cY%60+"s" : cY+"s");
}}void so(){if(de.Count==0)return;if(b4<0){if(T<iW)return;b4=0;gc=0;ew.Clear();}while(b4<de.Count&&Runtime.CurrentInstructionCount<20000){
string jn=de[b4];List<IMyTerminalBlock>gn;b1.TryGetValue(jn,out gn);if(gn!=null&&gc<gn.Count){var ng=gn[gc++];
for(int kd=0;kd<ng.InventoryCount;kd++){iQ.Clear();ng.GetInventory(kd).GetItems(iQ);foreach(var gY in iQ){string ku=gY.Type.TypeId.Replace("MyObjectBuilder_","");
string oW=ku=="Ore"||ku=="Ingot" ? gY.Type.SubtypeId+" "+ku.ToLowerInvariant(): gY.Type.SubtypeId;double oM;ew.TryGetValue(oW,out oM);ew[oW]=oM+(double)gY.Amount;
}}continue;}var hk=new List<KeyValuePair<string,double>>(ew);hk.Sort((v8,we)=>we.Value.CompareTo(v8.Value));for(int dF=1;dF<=gd[jn];dF++)bC[jn+".top"+dF]=dF<=hk.Count ? hk[dF-1].Key+" "+z.mr(hk[dF-1].Value,"auto"): "-";
ew.Clear();b4++;gc=0;}if(b4>=de.Count){b4= -1;iW=T+15.0;}}void aj(int vF,string vP){dc.Add(new KeyValuePair<string,int>(vP,vF));
}static string iK(string lk,int pk){return lk.Length>pk ? lk.Substring(0,pk): lk;}void my(){var ld=Me as IMyTextSurfaceProvider;if(ld==null||ld.SurfaceCount<1)return;
a5=ld.GetSurface(0);a5.ContentType=iM ? ContentType.SCRIPT : ContentType.NONE;a5.Script="";a5.ScriptBackgroundColor=Color.Black;
}void iL(){if(T>=f8){f8=T+(es>t.Z.Count+1 ? 5.0 : 1.0);rn();if(iM&&a5!=null&&eu==0)rx();
}Echo(m0);}void rn(){dc.Clear();int jq=0,nw=0;foreach(var nh in t.cv){List<IMyTerminalBlock>kB;if(nh.bV==bc.d5){
jq++;continue;}if(b1.TryGetValue(nh.ag,out kB)){nw+=kB.Count;if(kB.Count>0)jq++;}}int ob=t.be.Count;aj(0,"HELM-[Kernel-(v"+rL+")]");
aj(4,"------------------------------");aj(1,"krn-cfg");aj(2," screens "+t.Z.Count+"  binds "+jq+"/"+t.cv.Count+"  errors "+ob);
aj(2," alert "+(cH ? "RED" : "off")+"  blocks "+nw+"  docked-ignored "+iO);if(ob>0)aj(3," ! "+iK(t.be[0],40));
else if(t.eo.Count>0)aj(3," ? "+iK(t.eo[0],40));aj(1,"cfg-dsp");if(t.Z.Count==0)aj(3," no @screen: run 'starter'");
int vs=0;foreach(var ll in t.Z){if(++vs>10){aj(2," +"+(t.Z.Count-10)+" more");break;}IMyTextSurface lx;string qn="--",lD="no block";
if(b2.TryGetValue(ll,out lx)){qn=lx.SurfaceSize.X.ToString("0")+"x"+lx.SurfaceSize.Y.ToString("0");lD=bB.ContainsKey(ll)? "boot" : "live";
}aj(lD=="no block" ? 3 : 2," "+iK(ll.bg,14).PadRight(14)+" "+qn.PadRight(9)+" "+lD);}aj(1,"perf");aj(2," tick "+ev+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
aj(2," instr "+ga+"  peak "+iP+"  max "+Runtime.MaxInstructionCount);aj(2," rate "+Runtime.UpdateFrequency+"  idle "+es+"  up "+(int)T+"s");
if(iS.Length>0)aj(2," last "+iS);var qa=new StringBuilder();foreach(var t3 in dc)qa.AppendLine(t3.Key);m0=qa.ToString();}static Color rt(int vG){
switch(vG){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void rx(){Vector2 cU=a5.SurfaceSize;Vector2 kP=(a5.TextureSize-cU)/2f;int jB=30;foreach(var t4 in dc)jB=Math.Max(jB,t4.Key.Length);
Vector2 jA=a5.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float qb=Math.Min(1f,Math.Min(cU.X*0.94f/(jB*jA.X),cU.Y*0.94f/(dc.Count*jA.Y)));
float v9=kP.X+cU.X*0.03f;float rc=kP.Y+cU.Y*0.03f;var lz=new List<MySprite>();lz.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kP+cU/2f,Size=cU,Color=new Color(12,12,16)}
);foreach(var o0 in dc){a9(lz,o0.Key,rt(o0.Value),v9,rc,qb);rc+=jA.Y*qb;}using(var tA=a5.DrawFrame()){tA.AddRange(lz);}}static readonly List<string>rm=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void iD(){bB.Clear();if(t==null)return;for(int gS=0;gS<t.Z.Count;gS++){var hr=t.Z[gS];if(hr.h4>0f)bB[hr]=T+(gS/Math.Max(1,hr.me))*hr.mh/60.0;
}}void r9(){List<F>eI=null;int tM=(bB.Count+3)/4,tX=0;foreach(var g1 in bB){double kX=(T-g1.Value)/g1.Key.h4;if(kX<0)continue;
if(kX>=1.0){if(eI==null)eI=new List<F>();eI.Add(g1.Key);continue;}if((tX++ +ev)%tM==0)rw(g1.Key,kX);}if(eI==null)return;foreach(var fm in eI){
bB.Remove(fm);if(fm.h3>0f)f7[fm]=T;b3.Remove(fm);cI.Remove(fm);}}static void a9(List<MySprite>vx,string vQ,Color s8,float wa,float wf,float vn){
var qB=MySprite.CreateText(vQ,"Monospace",s8,vn,TextAlignment.LEFT);qB.Position=new Vector2(wa,wf);vx.Add(qB);}static void l7(List<MySprite>vy,List<string>gi,F hu,float wb,float wg,float vo,float uf){
string[]vD=hu.fF!=null&&hu.fF.Length>0 ? hu.fF : new[]{hu.mf};for(int eR=0;eR<gi.Count;
eR++){p jt=z.mu(vD,gi.Count>1 ? 100.0*eR/(gi.Count-1): 0.0);a9(vy,gi[eR],new Color(jt.bY,jt.bT,jt.bO),wb,wg+eR*uf,vo);}}bool sd(F dJ,IMyTextSurface lI){
aA aC,aS;F qe=t.Z.Find(ra=>ra!=dJ&&ra.bg.Equals(dJ.ik,StringComparison.OrdinalIgnoreCase));if(qe==null|| !b3.TryGetValue(qe,out aC)|| !aC.eb)return false;
if(!b3.TryGetValue(dJ,out aS))b3[dJ]=aS=new aA();Vector2 dM=lI.SurfaceSize;if(aS.eb&&aS.en==aC.en&&aS.ed==dM)return false;
aS.eb=true;aS.en=aC.en;aS.ed=dM;aS.f.Clear();Vector2 hD=aC.ed,pv=aC.iq,pt=(lI.TextureSize-dM)/2f;aS.iq=pt;
int ot=0,qN=aC.f.Count;if(dJ.il!=null){int dE=aC.bX.FindIndex(wc=>wc.mE.Equals(dJ.il,StringComparison.OrdinalIgnoreCase));
if(dE<0)return false;hD=aC.bX[dE].mP;pv=aC.bX[dE].mK;ot=aC.bX[dE].iC;if(dE+1<aC.bX.Count)qN=aC.bX[dE+1].iC;}float g_=Math.Min(dM.X/hD.X,dM.Y/hD.Y);
Vector2 sJ=pt+(dM-hD*g_)/2f;for(int ke=ot;ke<qN;ke++){var bG=aC.f[ke];if(bG.Position.HasValue)bG.Position=(bG.Position.Value-pv)*g_+sJ;if(bG.Size.HasValue)bG.Size=bG.Size.Value*g_;
if(bG.Type==SpriteType.TEXT)bG.RotationOrScale*=g_;aS.f.Add(bG);}using(var tB=lI.DrawFrame())tB.AddRange(aS.f);return true;}bool r7(F dK,IMyTextSurface hB){
List<string>ez;if(cI.ContainsKey(dK)|| !t.dW.TryGetValue(dK.ct,out ez))return false;cI[dK]="banner";Vector2 cV=hB.SurfaceSize;
Vector2 kQ=(hB.TextureSize-cV)/2f;int gt=1;foreach(var t5 in ez)gt=Math.Max(gt,t5.Length);Vector2 eF=hB.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float ht=Math.Min(cV.X*0.94f/(gt*eF.X),cV.Y*0.94f/(ez.Count*eF.Y));var lA=new List<MySprite>();lA.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kQ+cV/2f,Size=cV,Color=string.IsNullOrEmpty(dK.bP)? new Color(10,12,16): K(dK.bP)}
);l7(lA,ez,dK,kQ.X+(cV.X-gt*eF.X*ht)/2f,kQ.Y+(cV.Y-ez.Count*eF.Y*ht)/2f,ht,eF.Y*ht);using(var tC=hB.DrawFrame()){tC.AddRange(lA);
}return true;}void rw(F dL,double py){IMyTextSurface fo;if(!b2.TryGetValue(dL,out fo))return;Vector2 cp=fo.SurfaceSize;
Vector2 kR=(fo.TextureSize-cp)/2f;var e3=dL.rG();string ph=dL.mg ?? "HELM TELEMETRY KERNEL ONLINE";bool kI=cp.Y>=200f;
List<string>df;if(dL.cw==null|| !t.dW.TryGetValue(dL.cw,out df))df=rm;int dn=ph.Length+3;foreach(var t6 in e3)dn=Math.Max(dn,t6.Length+5);
if(kI)foreach(var t7 in df)dn=Math.Max(dn,t7.Length);int p2=(kI ? df.Count+1 : 0)+2+e3.Count;Vector2 dm=fo.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float cl=Math.Min(1f,Math.Min(cp.X*0.94f/(dn*dm.X),cp.Y*0.94f/(p2*dm.Y)));float hT=kR.X+(cp.X-dn*dm.X*cl)/2f;float c0=kR.Y+(cp.Y-p2*dm.Y*cl)/2f;
float g5=dm.Y*cl;var dN=new List<MySprite>();dN.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kR+cp/2f,Size=cp,Color=new Color(10,12,16)}
);if(kI){l7(dN,df,dL,hT,c0,cl,g5);c0+=(df.Count+1)*g5;}a9(dN,">> "+ph,new Color(34,197,94),hT,c0,cl);c0+=g5*2f;int vt=z.rl(py,e3.Count);
for(int gT=0;gT<vt;gT++){bool pu=gT==e3.Count-1&&py>=0.8;a9(dN,pu ? "[OK]" : "[..]",pu ? new Color(34,197,94): new Color(56,189,248),hT,c0,cl);a9(dN,e3[gT],new Color(203,213,225),hT+5f*dm.X*cl,c0,cl);
c0+=g5;}using(var tD=fo.DrawFrame()){tD.AddRange(dN);}}void iz(){b1.Clear();b2.Clear();f9.Clear();
et.Clear();f6=false;if(t==null)return;foreach(var af in t.cv){var a6=new List<IMyTerminalBlock>();if(af.bV==bc.d5){
string ks= !string.IsNullOrEmpty(af.bw)? af.bw : af.ag;if(!aQ.ContainsKey(ks)){aQ[ks]=af.fL;if(af.h6!=null)bC[ks]=af.h6;
}b1[af.ag]=a6;continue;}var qF=af.a3.Count>0 ? af.a3 : new List<string>{af.mS};foreach(var cZ in qF){
if(string.IsNullOrEmpty(cZ))continue;bool oU=cZ.Contains("*")||cZ.Contains("?");if(af.bV==bc.ig||oU){var j6=new List<IMyTerminalBlock>();
rq(cZ,j6,af.fC);if(j6.Count>0){foreach(var oD in j6){if(!a6.Contains(oD))a6.Add(oD);}}else{var m9=dV(af.fC);
for(int jd=0;jd<m9.Count;jd++){var hJ=m9[jd];if(oU ? z.rQ(cZ,hJ.CustomName): string.Equals(cZ,hJ.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!a6.Contains(hJ))a6.Add(hJ);}}}}else{var jj=mn(cZ,af.fC);if(jj!=null&& !a6.Contains(jj))a6.Add(jj);}}if(a6.Count==0){
sj(af,qF,a6);if(a6.Count>0)f9[af.ag]=true;}b1[af.ag]=a6;}foreach(var bL in t.Z){
if(string.IsNullOrEmpty(bL.bW))continue;var kU=mn(bL.bW);if(kU==null){et.AppendLine($"HELM: output block '{bL.bW}' not found");
continue;}IMyTextSurface cX=null;var le=kU as IMyTextSurfaceProvider;var n0=kU as IMyTextSurface;if(le!=null&&bL.cA<le.SurfaceCount){
cX=le.GetSurface(bL.cA);}else if(n0!=null&&bL.cA==0){cX=n0;}if(cX==null){et.AppendLine($"HELM: output block '{bL.bW}' has no surface index {bL.cA}");
continue;}cX.ContentType=ContentType.SCRIPT;cX.Script="";cX.ScriptBackgroundColor=Color.Black;b2[bL]=cX;}cG.Clear();
dd.Clear();}static readonly string[]mm={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool rE(int tY,IMyTerminalBlock v){switch(tY){case 0: return v is IMyBatteryBlock;case 1: return v is IMyReactor;case 2: return v is IMySolarPanel;
case 3: return v is IMyWindTurbine;case 4: return v is IMyGasTank;case 5: return v is IMyAirVent;case 6: return v is IMyCargoContainer;case 7: return v is IMyJumpDrive;
case 8: return v is IMyThrust;case 9: return v is IMyMotorSuspension;case 10: return v is IMyDoor;case 11: return v is IMyShipConnector;case 12: return v is IMyAssembler;
case 13: return v is IMyRefinery;case 14: return v is IMyShipDrill;case 15: return v is IMyShipWelder;case 16: return v is IMyShipGrinder;case 17: return v is IMyGasGenerator;
case 18: return v is IMyLargeTurretBase;case 19: return v is IMyProjector;case 20: return v is IMyGyro;case 21: return v is IMyGravityGeneratorBase;case 22: return v is IMyLandingGear;
case 23: return v is IMyRadioAntenna;case 24: return v is IMyBeacon;case 25: return v is IMyLightingBlock;case 26: return v is IMyPistonBase;case 27: return v is IMyMotorStator;
case 28: return v is IMyWarhead;case 29: return v is IMySensorBlock;case 30: return v is IMyCameraBlock;case 31: return v is IMyParachute;case 32: return v is IMyTurretControlBlock;
}return false;}void sj(bu sY,List<string>vN,List<IMyTerminalBlock>jl){string tS=(sY.ag+" "+string.Join(" ",vN)).ToLowerInvariant();
var kw=new List<int>();for(int g0=0;g0<mm.Length;g0++){foreach(var v7 in mm[g0].Split(','))if(tS.Contains(v7)){kw.Add(g0);break;}}if(kw.Count==0)return;
var m8=dV();var pw=new List<IMyTerminalBlock>();for(int kf=0;kf<m8.Count;kf++){var je=m8[kf];foreach(int tZ in kw){if(rE(tZ,je)){(je.CubeGrid==Me.CubeGrid ? jl : pw).Add(je);
break;}}}if(jl.Count==0)jl.AddRange(pw);}void sl(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(t==null||t.Z.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {t.Z.Count} screens, {t.cv.Count} binds");f6=false;
Echo($"Blocks: {dV().Count} on this construct; {iO} on docked ships (ignored)");cG.Clear();Echo("--- BIND RESOLUTION ---");foreach(var at in t.cv){
if(at.bV==bc.d5){string kt= !string.IsNullOrEmpty(at.bw)? at.bw : at.ag;double vY=aQ.ContainsKey(kt)? aQ[kt]: at.fL;
Echo($"[OK] @bind {at.ag} (dynamic variable '{kt}' = {vY})");continue;}List<IMyTerminalBlock>jm;if(b1.TryGetValue(at.ag,out jm)){if(jm.Count>0){
string tv=f9.ContainsKey(at.ag)&&f9[at.ag]? " [fallback: typed match]" : "";Echo($"[OK] @bind {at.ag} ({jm.Count} blocks matched{tv})");
}else{string vO=string.Join(", ",at.a3.Count>0 ? at.a3 : new List<string>{at.mS});Echo($"[WARN] @bind {at.ag} -> 0 blocks matched \"{vO}\"");
}}else{Echo($"[ERR] @bind {at.ag} -> unresolved");}}if(aQ.Count>0){Echo($"--- DYNAMIC VARIABLES ({aQ.Count}) ---");foreach(var o2 in aQ)Echo($"[VAR] {o2.Key} = {o2.Value}");
}IMyTextSurface i5=null;foreach(var uz in b2.Values){i5=uz;break;}if(i5!=null){var lB=new List<string>();i5.GetSprites(lB);
Echo($"--- SPRITES ({lB.Count}): names usable in bg_texture ---");Echo(string.Join(", ",lB.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bM in t.Z){IMyTextSurface hC;if(b2.TryGetValue(bM,out hC)){var qo=hC.SurfaceSize;var vB=z.rs(bM);
Echo($"[OK] Screen \"{bM.bg}\" [{bM.f0}] -> {bM.bW} [#{bM.cA}] ({qo.X:0}x{qo.Y:0} px, texture {hC.TextureSize.X:0}x{hC.TextureSize.Y:0}, {vB.ah} sprites)");
}else{Echo($"[ERR] Screen \"{bM.bg}\" -> Missing block \"{bM.bW}\" [#{bM.cA}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double sk(IMyTerminalBlock w){
if(w==null)return 0.0;var gl=w as IMyBatteryBlock;if(gl!=null)return gl.MaxStoredPower>0 ?(gl.CurrentStoredPower/gl.MaxStoredPower)*100.0 : 0.0;
var gZ=w as IMyJumpDrive;if(gZ!=null)return gZ.MaxStoredPower>0 ?(gZ.CurrentStoredPower/gZ.MaxStoredPower)*100.0 : 0.0;var qE=w as IMyGasTank;
if(qE!=null)return qE.FilledRatio*100.0;var q3=w as IMyAirVent;if(q3!=null)return q3.GetOxygenLevel()*100.0;var nE=w as IMyCargoContainer;if(nE!=null){
var gW=nE.GetInventory();if(gW!=null&&gW.MaxVolume.RawValue>0)return(double)gW.CurrentVolume.RawValue/gW.MaxVolume.RawValue*100.0;return 0.0;}var hl=w as IMyReactor;
if(hl!=null)return hl.MaxOutput>0 ?(hl.CurrentOutput/hl.MaxOutput)*100.0 : 0.0;var hv=w as IMySolarPanel;if(hv!=null)return hv.MaxOutput>0 ?(hv.CurrentOutput/hv.MaxOutput)*100.0 : 0.0;
var hS=w as IMyWindTurbine;if(hS!=null)return hS.MaxOutput>0 ?(hS.CurrentOutput/hS.MaxOutput)*100.0 : 0.0;var n1=w as IMyDoor;if(n1!=null)return(double)n1.OpenRatio*100.0;
var hM=w as IMyThrust;if(hM!=null)return hM.MaxEffectiveThrust>0 ?(hM.CurrentThrust/hM.MaxEffectiveThrust)*100.0 : 0.0;var lN=w as IMyMotorSuspension;
if(lN!=null)return lN.Brake ? 100.0 :(lN.Friction*100.0);var jF=w as IMyShipConnector;if(jF!=null)return jF.Status==MyShipConnectorStatus.Connected ? 100.0 :(jF.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var pP=w as IMyProductionBlock;if(pP!=null)return pP.IsProducing ? 100.0 : 0.0;var n5=w as IMyShipDrill;if(n5!=null)return n5.Enabled ? 100.0 : 0.0;
var lV=w as IMyShipToolBase;if(lV!=null)return lV.IsActivated ? 100.0 :(lV.Enabled ? 50.0 : 0.0);var j3=w as IMyGasGenerator;if(j3!=null)return j3.AutoRefill ? 100.0 :(j3.Enabled ? 50.0 : 0.0);
var hN=w as IMyLargeTurretBase;if(hN!=null)return hN.IsShooting ? 100.0 :(hN.HasTarget ? 50.0 :(hN.Enabled ? 10.0 : 0.0));var fe=w as IMyProjector;
if(fe!=null)return fe.TotalBlocks>0 ?((double)(fe.TotalBlocks-fe.RemainingBlocks)/fe.TotalBlocks)*100.0 : 0.0;var j9=w as IMyGyro;if(j9!=null)return j9.GyroOverride ? 100.0 :(j9.Enabled ? 50.0 : 0.0);
var oF=w as IMyGravityGeneratorBase;if(oF!=null)return Math.Min(100.0,Math.Abs(oF.GravityAcceleration)/9.81*100.0);var j2=w as IMyLandingGear;if(j2!=null)return j2.IsLocked ? 100.0 :(j2.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var na=w as IMyRadioAntenna;if(na!=null)return(na.Radius/50000.0)*100.0;var nn=w as IMyBeacon;if(nn!=null)return(nn.Radius/50000.0)*100.0;
var pa=w as IMyLightingBlock;if(pa!=null)return pa.Intensity*10.0;var hg=w as IMyPistonBase;if(hg!=null)return hg.HighestPosition>0 ?(hg.CurrentPosition/hg.HighestPosition)*100.0 : 0.0;
var p_=w as IMyMotorStator;if(p_!=null)return Math.Min(100.0,Math.Abs(p_.Angle)/6.28318*100.0);var q6=w as IMyWarhead;if(q6!=null)return q6.IsArmed ? 100.0 : 0.0;
var lu=w as IMySensorBlock;if(lu!=null)return!lu.LastDetectedEntity.IsEmpty()? 100.0 :(lu.Enabled ? 25.0 : 0.0);var jw=w as IMyCameraBlock;
if(jw!=null)return jw.IsActive ? 100.0 :(jw.Enabled ? 50.0 : 0.0);var nK=w as IMyParachute;if(nK!=null)return(double)nK.OpenRatio*100.0;
var hL=w as IMyTurretControlBlock;if(hL!=null)return hL.HasTarget ? 100.0 :(hL.IsUnderControl ? 50.0 :(hL.Enabled ? 10.0 : 0.0));var ou=w as IMyFunctionalBlock;
if(ou!=null)return ou.Enabled ? 100.0 : 0.0;return 0.0;}static double rN(IMyTerminalBlock jf,string ds){bool qk=ds.StartsWith("share:",StringComparison.OrdinalIgnoreCase);
if(qk)ds=ds.Substring(6);int nD=ds.IndexOf(':');string kv=ds.Substring(0,nD).ToLowerInvariant();var qR=new MyItemType("MyObjectBuilder_"+(kv=="ammo" ? "AmmoMagazine" : char.ToUpper(kv[0])+kv.Substring(1)),ds.Substring(nD+1));
double lH=0,jy=0;for(int gU=0;gU<jf.InventoryCount;gU++){lH+=(double)jf.GetInventory(gU).GetItemAmount(qR);jy+=(double)jf.GetInventory(gU).MaxVolume;}return qk ?(jy>0 ? lH*qR.GetItemInfo().Volume/jy*100.0 : 0.0): lH;
}double mL(IMyTerminalBlock x,string oh){if(x==null)return 0.0;string c=oh.ToLowerInvariant();if(c=="percent"||c=="pct")return sk(x);
if(c.IndexOf(':')>0)return rN(x,oh);var b7=x as IMyBatteryBlock;if(b7!=null){if(c=="current"||c=="stored"||c=="val"||c=="value"||c=="raw")return b7.CurrentStoredPower;
if(c=="max"||c=="capacity")return b7.MaxStoredPower;if(c=="output")return b7.CurrentOutput;if(c=="maxoutput")return b7.MaxOutput;if(c=="input")return b7.CurrentInput;
if(c=="maxinput")return b7.MaxInput;return b7.CurrentStoredPower;}var kr=x as IMyJumpDrive;if(kr!=null){if(c=="max"||c=="capacity")return kr.MaxStoredPower;
return kr.CurrentStoredPower;}var hG=x as IMyGasTank;if(hG!=null){if(c=="max"||c=="capacity")return hG.Capacity;return hG.FilledRatio*hG.Capacity;
}var q4=x as IMyAirVent;if(q4!=null){if(c=="max"||c=="capacity")return 100.0;return q4.GetOxygenLevel()*100.0;}var nF=x as IMyCargoContainer;if(nF!=null){
var gX=nF.GetInventory();if(gX!=null){if(c=="mass")return(double)gX.CurrentMass;if(c=="max"||c=="capacity")return(double)gX.MaxVolume;return(double)gX.CurrentVolume;
}return 0.0;}var lf=x as IMyPowerProducer;if(lf!=null){if(c=="max"||c=="capacity")return lf.MaxOutput;return lf.CurrentOutput;}var n2=x as IMyDoor;
if(n2!=null){if(c=="max")return 1.0;return(double)n2.OpenRatio*100.0;}var lS=x as IMyThrust;if(lS!=null){if(c=="max"||c=="capacity")return lS.MaxEffectiveThrust;
return lS.CurrentThrust;}var cr=x as IMyMotorSuspension;if(cr!=null){if(c=="brake")return cr.Brake ? 100.0 : 0.0;if(c=="height")return cr.Height;
if(c=="strength")return cr.Strength;if(c=="power")return cr.Power;if(c=="steer")return cr.SteeringOverride*100.0;if(c=="propulsion")return cr.PropulsionOverride*100.0;
return cr.Friction;}var nS=x as IMyShipConnector;if(nS!=null){return nS.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var lc=x as IMyProductionBlock;
if(lc!=null){if(c=="producing"||c=="active"||c=="running"||c=="state")return lc.IsProducing ? 100.0 : 0.0;if(c=="queue")return lc.IsQueueEmpty ? 0.0 : 1.0;
}var i9=x as IMyAssembler;if(i9!=null){if(c=="disassembling")return i9.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;if(c=="repeating"||c=="loop")return i9.Repeating ? 100.0 : 0.0;
}var j4=x as IMyGasGenerator;if(j4!=null){if(c=="autorefill")return j4.AutoRefill ? 100.0 : 0.0;if(c=="active")return j4.Enabled ? 100.0 : 0.0;}var hO=x as IMyLargeTurretBase;
if(hO!=null){if(c=="shooting")return hO.IsShooting ? 100.0 : 0.0;if(c=="target"||c=="hastarget")return hO.HasTarget ? 100.0 : 0.0;if(c=="range")return hO.Range;
}var cj=x as IMyProjector;if(cj!=null){if(c=="built")return cj.TotalBlocks-cj.RemainingBlocks;if(c=="remaining")return cj.RemainingBlocks;if(c=="total")return cj.TotalBlocks;
if(c=="projecting")return cj.IsProjecting ? 100.0 : 0.0;if(c=="armor")return cj.RemainingArmorBlocks;if(c=="buildable")return cj.BuildableBlocksCount;}var lW=x as IMyShipToolBase;
if(lW!=null){if(c=="working"||c=="active"||c=="activated")return lW.IsActivated ? 100.0 : 0.0;return lW.Enabled ? 100.0 : 0.0;}var du=x as IMyGyro;if(du!=null){
if(c=="override")return du.GyroOverride ? 100.0 : 0.0;if(c=="pitch")return du.Pitch;if(c=="yaw")return du.Yaw;if(c=="roll")return du.Roll;if(c=="power")return du.GyroPower*100.0;
}var oG=x as IMyGravityGeneratorBase;if(oG!=null){if(c=="g"||c=="accel")return oG.GravityAcceleration;}var oE=x as IMyLandingGear;if(oE!=null){
if(c=="locked")return oE.IsLocked ? 100.0 : 0.0;}var nb=x as IMyRadioAntenna;if(nb!=null){if(c=="radius"||c=="range")return nb.Radius;}var no=x as IMyBeacon;
if(no!=null){if(c=="radius"||c=="range")return no.Radius;}var kE=x as IMyLightingBlock;if(kE!=null){if(c=="intensity")return kE.Intensity;if(c=="radius")return kE.Radius;
}var hh=x as IMyPistonBase;if(hh!=null){if(c=="pos"||c=="position"||c=="extension")return hh.CurrentPosition;if(c=="velocity")return hh.Velocity;
if(c=="max")return hh.HighestPosition;}var fl=x as IMyMotorStator;if(fl!=null){if(c=="angle"||c=="deg")return fl.Angle*180.0/Math.PI;if(c=="rad")return fl.Angle;
if(c=="velocity"||c=="rpm")return fl.TargetVelocityRPM;if(c=="locked")return fl.RotorLock ? 100.0 : 0.0;}var l4=x as IMyWarhead;if(l4!=null){
if(c=="armed")return l4.IsArmed ? 100.0 : 0.0;if(c=="countdown")return l4.DetonationTime;}var qh=x as IMySensorBlock;if(qh!=null){if(c=="detected")return!qh.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;
}var jx=x as IMyCameraBlock;if(jx!=null){if(c=="range")return jx.AvailableScanRange;if(c=="active")return jx.IsActive ? 100.0 : 0.0;}var nL=x as IMyParachute;
if(nL!=null){if(c=="open"||c=="openratio")return(double)nL.OpenRatio*100.0;}var dR=x as IMyTurretControlBlock;if(dR!=null){if(c=="target"||c=="hastarget")return dR.HasTarget ? 100.0 : 0.0;
if(c=="azimuth")return dR.AzimuthRotor!=null ? dR.AzimuthRotor.Angle*180.0/Math.PI : 0.0;if(c=="elevation")return dR.ElevationRotor!=null ? dR.ElevationRotor.Angle*180.0/Math.PI : 0.0;
}var ov=x as IMyFunctionalBlock;if(ov!=null){return ov.Enabled ? 100.0 : 0.0;}return 0.0;}double a2(string eC,string oi){bu b8;
ep.TryGetValue(eC,out b8);if(b8!=null&&b8.bV==bc.d5){string t_= !string.IsNullOrEmpty(b8.bw)? b8.bw : eC;
double nV;if(aQ.TryGetValue(t_,out nV))return nV;return b8.fL;}double n8;if(aQ.TryGetValue(eC,out n8))return n8;
List<IMyTerminalBlock>eD;if(!b1.TryGetValue(eC,out eD)||eD.Count==0)throw new InvalidOperationException($"HELM: bind '{eC}' resolved to zero blocks — check the name/typo");
if(eD.Count==1)return mL(eD[0],oi);var q2=new List<double>();foreach(var sN in eD)q2.Add(mL(sN,oi));return rh.rg(b8!=null ? b8.dU : aL.hW,q2);
}double iA(string s_){return a2(s_,"percent");}static string bf(double qZ,bx dA){if(dA==null||(string.IsNullOrEmpty(dA.em)&&string.IsNullOrEmpty(dA.d6)&&dA.J=="percent")){
return mt(qZ);}return z.mr(qZ,dA.em,dA.d6);}bool se(F cm){IMyTextSurface fp;if(!b2.TryGetValue(cm,out fp))return false;
if(cm.ct!=null)return r7(cm,fp);if(cm.ik!=null)return sd(cm,fp);if(cm.c4.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return sf(cm,fp);}return sg(cm,fp);}Color al(bx o3,Color tk){return o3.iJ!=null ? K(o3.iJ): tk;
}Color d7(string[]dO,double uK){if(dO.Length==1)return K(dO[0]);double pT=Math.Max(0.0,Math.Min(100.0,uK))/100.0*(dO.Length-1);int kg=Math.Min((int)pT,dO.Length-2);
float hE=(float)(pT-kg);Color b5=K(dO[kg]),gk=K(dO[kg+1]);return new Color((int)(b5.R+(gk.R-b5.R)*hE),(int)(b5.G+(gk.G-b5.G)*hE),(int)(b5.B+(gk.B-b5.B)*hE),(int)(b5.A+(gk.A-b5.A)*hE));
}static Color[]fy(aA s3,string[]vE,string tm,float rb,float rd,float l1,float ka){int eA=16;bool oN=tm=="horizontal";var jE=new Color[eA];
for(int cO=0;cO<eA;cO++){p j1=z.mu(vE,100.0*cO/(eA-1));jE[cO]=new Color(j1.bY,j1.bT,j1.bO);float nC=l1/eA,nv=ka/eA;q(s3,oN ? new Vector2(rb+nC*(cO+0.5f),rd+ka/2f): new Vector2(rb+l1/2f,rd+nv*(cO+0.5f)),oN ? new Vector2(nC+1f,ka): new Vector2(l1,nv+1f),jE[cO]);
}return jE;}static void q(aA s4,Vector2 uV,Vector2 vu,Color s9,float vf=0f,string tl="SquareSimple"){s4.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=tl,Position=uV,Size=vu,Color=s9,RotationOrScale=vf}
);}void fz(aA b9,string cW,string[]aR,string nO,bool qq,float aH,float aY,float bK,float bh){cW=cW.ToLowerInvariant();float V=cW=="heavy" ? 4f : 2f,fx=bK-aH,kb=bh-aY;
Color bD=K(aR!=null&&aR.Length>0 ? aR[0]: nO ?? "40, 60, 85");Color ca=aR!=null&&aR.Length>1 ? K(aR[aR.Length-1]): bD;Action<float,float,float,float,Color>aV=(wd,wh,pS,pR,s6)=>q(b9,new Vector2(wd+pS/2f,wh+pR/2f),new Vector2(pS,pR),s6);
if(cW=="corners"){float aT=Math.Max(10f,Math.Min(fx,kb)*0.15f);aV(aH,aY,aT,V,bD);aV(aH,aY,V,aT,bD);aV(bK-aT,aY,aT,V,ca);aV(bK-V,aY,V,aT,ca);aV(aH,bh-V,aT,V,bD);aV(aH,bh-aT,V,aT,bD);
aV(bK-aT,bh-V,aT,V,ca);aV(bK-V,bh-aT,V,aT,ca);return;}if(aR!=null&&aR.Length>1&&qq){fy(b9,aR,"horizontal",aH,aY,fx,V);fy(b9,aR,"horizontal",aH,bh-V,fx,V);
}else{aV(aH,aY,fx,V,bD);aV(aH,bh-V,fx,V,ca);}if("|frame|single|heavy|double|rounded|".Contains("|"+cW+"|")){aV(aH,aY,V,kb,bD);aV(bK-V,aY,V,kb,ca);}if(cW=="rounded"){
float cb=V*3f;q(b9,new Vector2(aH,aY),new Vector2(cb,cb),bD,0f,"Circle");q(b9,new Vector2(bK,aY),new Vector2(cb,cb),ca,0f,"Circle");q(b9,new Vector2(aH,bh),new Vector2(cb,cb),bD,0f,"Circle");
q(b9,new Vector2(bK,bh),new Vector2(cb,cb),ca,0f,"Circle");}if(cW=="double")fz(b9,"single",aR,nO,qq,aH+5f,aY+5f,bK-5f,bh-5f);}void ro(F n,IMyTextSurface lJ,aA g){
g.f.Clear();g.ei.Clear();g.bX.Clear();Vector2 ad=lJ.SurfaceSize;Vector2 vS=lJ.TextureSize;Vector2 bs=(vS-ad)/2f;
g.iq=bs;float fa=Math.Max(4f,ad.X*0.02f);float dC=Math.Max(4f,ad.Y*0.02f);string pZ=z.mM(n.f0,ad.X,ad.Y);
if(pZ=="compact"){fa=4f;dC=4f;}else if(pZ=="corner"){fa=2f;dC=2f;}if(n.iu>=0f)fa=dC=n.iu;
float qS=ad.X-(fa*2f);Color dh=new Color(10,12,16);bool n4=true;if(!string.IsNullOrEmpty(n.bP)){var eP=p.eg(n.bP);
if(eP.fV)n4=false;else dh=new Color(eP.bY,eP.bT,eP.bO,eP.hU);}g.cu= -1;g.fB=false;g.fA=K(n.l9 ?? "#5b0f0f");
g.dZ.Clear();if(n.bR!=null&&n.bR.Length>1){g.cu=g.f.Count;g.fD=fy(g,n.bR,n.dY,bs.X+2f,bs.Y+2f,ad.X-4f,ad.Y-4f);
}else if(n4||n.a0!=null||n.c5!=null){if(n.a0!=null)dh=K(n.a0.First(u1=>u1.c7).bS);g.cu=g.f.Count;
g.fD=new[]{dh};q(g,bs+(ad/2f),ad-new Vector2(4f,4f),dh);if(n.a0!=null||n.c5!=null)g.dZ.Add(new fE{
fU=g.cu,hX=dh,fK=dh,f2=n.a0,h0=n.hY,h8=n.hZ,fQ=n.c5,h9=n.ia,ib=n.ic,mz=true}
);}if(!string.IsNullOrEmpty(n.h_))q(g,bs+(ad/2f),ad-new Vector2(4f,4f),K(n.mb ?? "255,255,255,24"),0f,n.h_);
if(!n.c4.Equals("none",StringComparison.OrdinalIgnoreCase)){float nx=bs.X+(ad.X-qS)/2f;fz(g,n.c4,n.fJ,n.d0,!n.fS,nx-2f,bs.Y+dC*0.5f-1f,nx+qS+2f,bs.Y+ad.Y-dC*0.5f+1f);
}var k2=n.ak;if(k2.Count==0){k2=new List<aF>{new aF{da="main",d2=0,eh=0,ej=n.d8,ek=n.d9,aq=n.fX}
};}float oI=n.ie>=0f ? n.ie : 8f;foreach(var r in k2){var g9=z.rr(r.d2,r.eh,r.ej,r.ek,n.d8,n.d9,ad.X,ad.Y,fa,dC,oI,oI);
float aw=bs.X+g9.mY;float bI=bs.Y+g9.mZ;float aU=g9.mX;float bH=g9.mw;g.bX.Add(new iw{mE=r.da,iC=g.f.Count,mK=new Vector2(aw,bI),mP=new Vector2(aU,bH)}
);bool p3=r.a0!=null||r.c5!=null;if(r.bR!=null&&r.bR.Length>1){fy(g,r.bR,r.dY!="vertical" ? r.dY : n.dY,aw,bI,aU,bH);
}else{Color ci;if(r.a0!=null)ci=K(r.a0.First(u2=>u2.c7).bS);else if(!string.IsNullOrEmpty(r.c2))ci=K(r.c2);
else if(!string.IsNullOrEmpty(n.iv))ci=K(n.iv);else ci=Color.Transparent;if(ci.A>0||p3){if(p3)g.dZ.Add(new fE{fU=g.f.Count,hX=ci,fK=ci,f2=r.a0,h0=r.hY,h8=r.hZ,fQ=r.c5,h9=r.ia,ib=r.ic}
);q(g,new Vector2(aw+aU/2f,bI+bH/2f),new Vector2(aU,bH),ci);}}if(r.d_!=null&& !r.d_.Equals("none",StringComparison.OrdinalIgnoreCase))fz(g,r.d_,r.fI ?? n.fJ,r.fH ?? n.d0,!n.fS,aw+1f,bI+1f,aw+aU-1f,bI+bH-1f);
string gK=r.mo ?? n.mp ?? "Monospace";float sW=z.rI(n.f0,ad.X,ad.Y,n.c6);float aG=r.mq ?? sW;
float eQ=0f;if(!string.IsNullOrEmpty(r.cE)){eQ=z.ms(bH,aG);float vU=z.rH(bH,aG);
var i1=TextAlignment.LEFT;float lT=aw+4f;string m3=r.iH ?? n.mT ?? "left";if(m3.Equals("center",StringComparison.OrdinalIgnoreCase)){
i1=TextAlignment.CENTER;lT=aw+(aU/2f);}else if(m3.Equals("right",StringComparison.OrdinalIgnoreCase)){i1=TextAlignment.RIGHT;lT=aw+aU-4f;
}string qH=r.iI ?? n.mU;Color vT= !string.IsNullOrEmpty(qH)? K(qH): K(r.fH ?? n.d0 ?? "white");
var qM=MySprite.CreateText(r.cE,gK,vT,vU,i1);qM.Position=new Vector2(lT,bI+2f);g.f.Add(qM);
string[]jg=r.fI ?? n.fJ;Color to=jg!=null&&jg.Length>0 ? K(jg[0]): K(r.fH ?? n.d0 ?? "40, 60, 85");
q(g,new Vector2(aw+aU/2f,bI+Math.Max(1f,eQ-2f)),new Vector2(Math.Max(0f,aU-8f),1f),to);}if(r.ir=="vertical"||r.aq.Any(uR=>uR.i==e.f4)){
rp(g,lJ,r,gK,aG,aw,bI,aU,bH,eQ);continue;}int wj=Math.Max(1,r.aq.Count);float uI=Math.Max(1f,bH-eQ-8f);
float q8=0f,m5=0f;foreach(var uS in r.aq)q8+=1<<uS.f3;float vW=uI/Math.Max(1f,q8);float eY=6f;for(int kh=0;kh<r.aq.Count;
kh++){var j=r.aq[kh];var O=new cz{ef=j};float dI=vW*(1<<j.f3);float p1=bI+eQ+4f+m5;m5+=dI;float M=p1+(dI/2f);
float qJ=M-(11f*aG);O.c8=g.f.Count;O.mv=j.ab!=null&&(j.ab.Contains("{val}")||j.ab.Contains("{percent}"));
float eW=j.fT!=null ? 18f*aG : 0f;var o7=MySprite.CreateText(j.ab,gK,j.ec!=null ? K(j.ec): Color.White,aG,TextAlignment.LEFT);
o7.Position=new Vector2(aw+eY+eW,qJ);g.f.Add(o7);if(j.fT!=null)q(g,new Vector2(aw+eY+eW/2f,M),new Vector2(eW-2f,eW-2f),Color.White,0f,j.fT);
float ud=Math.Max(24f*aG,(j.ab.Length+1)*12f*aG);float v4=j.io ? 0f :(!string.IsNullOrEmpty(j.em)|| !string.IsNullOrEmpty(j.d6))? 64f*aG : 52f*aG;
if(j.i!=e.iF){float L=aw+eY+eW+ud;float sV=aw+aU-eY-v4;float I=Math.Max(16f,sV-L);
float Q=Math.Max(j.i==e.cB ? 8f : 4f,Math.Min((j.i==e.cB ? 20f : 16f)*aG,dI*(j.i==e.cB ? 0.55f : 0.45f)));
if(j.f3>0)Q=dI*0.7f;Color qQ=al(j,new Color(25,30,38));O.bb=L;O.aD=I;O.ba=Q;O.bt=M;
if(j.i==e.dX){q(g,new Vector2(L+(I/2f),M),new Vector2(I,Q),qQ);O.aa=g.f.Count;
q(g,new Vector2(L,M),new Vector2(0f,Q),K(j.az));bool vd=j.fN=="left";for(int lU=1;lU<j.iG;lU++)q(g,new Vector2(L+I*lU/j.iG,M),new Vector2(1f,Q),new Color(0,0,0,150));
if(j.fY!=null)foreach(double pf in j.fY)q(g,new Vector2(L+I*(float)(vd ? 1.0-pf/100.0 : pf/100.0),M),new Vector2(2f,Q+6f),Color.White);
if(j.it!=null)fz(g,"single",null,j.it,true,L,M-Q/2f,L+I,M+Q/2f);}else if(j.i==e.fM){
float dG=Math.Max(8f,Math.Min(I/2f,dI*0.85f));float gA=L+I/2f,gB=p1+dI-4f;int jJ=j.bA>0 ? j.bA : 16;O.bb=gA;
O.bt=gB;O.aD=dG;for(int jG=0;jG<jJ;jG++){float lR=(jG+0.5f)/jJ*3.14159f;O.E.Add(g.f.Count);q(g,new Vector2(gA-(float)Math.Cos(lR)*dG*0.8f,gB-(float)Math.Sin(lR)*dG*0.8f),new Vector2(dG*3.14159f/jJ*0.85f,Math.Max(3f,dG*0.16f)),al(j,new Color(24,28,36)),lR-1.5708f);
}O.E.Add(g.f.Count);q(g,new Vector2(gA,gB),new Vector2(3f,dG*0.9f),Color.White);q(g,new Vector2(gA,gB),new Vector2(8f,8f),Color.White,0f,"Circle");
}else if(j.i==e.ee){int ls=j.bA>0 ? j.bA : 10;float ox=2f;float lt=Math.Max(2f,(I-(ox*(ls-1)))/ls);for(int lm=0;
lm<ls;lm++){O.E.Add(g.f.Count);q(g,new Vector2(L+lm*(lt+ox)+(lt/2f),M),new Vector2(lt,Q),al(j,new Color(22,26,32)));
}}else if(j.i==e.cB){q(g,new Vector2(L+(I/2f),M),new Vector2(I,Q),al(j,new Color(20,24,30)));int hi=j.bA>0 ? j.bA : 8;
O.cC=new float[hi];float oy=2f;float jC=Math.Max(2f,(I-(oy*(hi-1)))/hi);for(int kY=0;kY<hi;kY++){O.E.Add(g.f.Count);
q(g,new Vector2(L+kY*(jC+oy)+(jC/2f),M),new Vector2(jC,2f),K(j.az));}}else if(j.i==e.ea){q(g,new Vector2(L+(I/2f),M),new Vector2(I,Q),al(j,new Color(20,24,30)));
int lE=10;float oz=2f;float lF=Math.Max(2f,(I-(oz*(lE-1)))/lE);for(int ln=0;ln<lE;ln++){O.E.Add(g.f.Count);
q(g,new Vector2(L+ln*(lF+oz)+(lF/2f),M),new Vector2(lF,Q),al(j,new Color(24,28,36)),0.523599f);}}else if(j.i==e.d1){
int i7=10;float oA=2f;float i8=Math.Max(2f,(I-(oA*(i7-1)))/i7);for(int lo=0;lo<i7;lo++){O.E.Add(g.f.Count);
q(g,new Vector2(L+lo*(i8+oA)+(i8/2f),M),new Vector2(i8,Q),al(j,new Color(24,28,36)),1.570796f,"Triangle");}}else if(j.i==e.d4){
int jK=j.bA>0 ? j.bA : 12;float oB=2f;float jM=Math.Max(2f,(I-(oB*(jK-1)))/jK);float n3=Math.Min(jM,Q);for(int lp=0;
lp<jK;lp++){O.E.Add(g.f.Count);q(g,new Vector2(L+lp*(jM+oB)+(jM/2f),M),new Vector2(n3,n3),al(j,new Color(24,28,36)),0f,"Circle");
}}else if(j.i==e.c3){q(g,new Vector2(L+(I/2f),M),new Vector2(I,Q),qQ);O.aa=g.f.Count;
q(g,new Vector2(L+(I/2f),M),new Vector2(0f,Q),K(j.az));q(g,new Vector2(L+(I/2f),M),new Vector2(2f,Q+4f),new Color(80,110,150));
}else if(j.i==e.cF){q(g,new Vector2(L+(I/2f),M),new Vector2(I,Q),al(j,new Color(20,24,30)));O.aa=g.f.Count;
float kx=Math.Max(4f,Q);q(g,new Vector2(L+kx*0.5f,M),new Vector2(kx,kx),new Color(40,50,65),0f,"Circle");}else if(j.i==e.cx){
float dj=Math.Max(6f,Math.Min(Q,14f*aG));q(g,new Vector2(L+dj*0.5f,M),new Vector2(dj,dj),new Color(28,34,46));
O.aa=g.f.Count;q(g,new Vector2(L+dj*0.5f,M),new Vector2(dj*0.6f,dj*0.6f),new Color(40,50,65));}else if(j.i==e.cD){
q(g,new Vector2(L+(I/2f),M),new Vector2(I,Q),al(j,new Color(18,22,30)));O.aa=g.f.Count;float k9=Math.Max(4f,Q*0.7f);
q(g,new Vector2(L+k9+2f,M),new Vector2(k9,k9),new Color(34,197,94),0f,"Circle");}else if(j.i==e.c1){int k8=8;
float oC=2f;float la=Math.Max(2f,(I-(oC*(k8-1)))/k8);for(int kZ=0;kZ<k8;kZ++){O.E.Add(g.f.Count);q(g,new Vector2(L+kZ*(la+oC)+(la/2f),M),new Vector2(la,Q),al(j,new Color(24,28,36)));
}}else if(j.i==e.cy){q(g,new Vector2(L+(I/2f),M),new Vector2(I,Q),al(j,new Color(20,24,30)));O.aa=g.f.Count;
float oQ=Math.Max(4f,Q);q(g,new Vector2(L+(I/2f),M),new Vector2(oQ,oQ),K(j.az),0f,"Triangle");}}var qX=MySprite.CreateText("---",gK,Color.White,aG,TextAlignment.RIGHT);
qX.Position=new Vector2(aw+aU-eY,qJ);O.a4=g.f.Count;g.f.Add(qX);g.ei.Add(O);}}g.ed=ad;
g.eb=true;}const string su="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int b_=8;void rp(aA ac,IMyTextSurface vH,aF k1,string op,float eN,float uE,float k0,float uF,float pA,float tQ){
int ur=Math.Max(1,k1.aq.Count);float nP=(uF-12f)/ur;float hP=22f*eN;float bq=k0+tQ+4f+hP;float aZ=Math.Max(20f,k0+pA-4f-hP-bq);
float th=vH.MeasureStringInPixels(new StringBuilder("M"),op,eN*0.9f).X;for(int gV=0;gV<k1.aq.Count;gV++){var cP=k1.aq[gV];float bk=uE+6f+nP*(gV+0.5f);
float c_=nP*0.7f;var bn=new cz{ef=cP,mV=true,bb=bk,aD=c_,ba=aZ,bt=bq+aZ/2f,mj=th};bn.c8=ac.f.Count;
a9(ac.f,cP.ab,cP.ec!=null ? K(cP.ec): Color.White,bk,k0+pA-4f-hP,eN);bn.a4=ac.f.Count;
a9(ac.f,"---",Color.White,bk,bq-hP,eN);var o4=ac.f[bn.c8];o4.Alignment=TextAlignment.CENTER;ac.f[bn.c8]=o4;
var qW=ac.f[bn.a4];qW.Alignment=TextAlignment.CENTER;ac.f[bn.a4]=qW;q(ac,new Vector2(bk,bq+aZ/2f),new Vector2(c_,aZ),al(cP,new Color(25,30,38)));
bn.aa=ac.f.Count;if(cP.i==e.f4){for(int ni=0;ni<b_;ni++){bn.E.Add(ac.f.Count);q(ac,new Vector2(bk,bq+aZ),new Vector2(c_-6f,0f),Color.White);
}for(int l2=0;l2<4;l2++){bn.E.Add(ac.f.Count);var qC=MySprite.CreateText("",op,l2==0 ? new Color(255,255,255,170): new Color(255,255,255,110),eN*0.9f,TextAlignment.CENTER);
qC.Position=new Vector2(bk,bq+aZ);ac.f.Add(qC);}Color jO=new Color(90,110,135);q(ac,new Vector2(bk-c_/2f,bq+aZ/2f),new Vector2(3f,aZ),jO);q(ac,new Vector2(bk+c_/2f,bq+aZ/2f),new Vector2(3f,aZ),jO);
q(ac,new Vector2(bk,bq+aZ),new Vector2(c_+3f,3f),jO);}else{q(ac,new Vector2(bk,bq+aZ),new Vector2(c_,0f),K(cP.az));}ac.ei.Add(bn);
}}bool st(aA bi,cz C,double fv,double pU,Color gu,bool q0){var a7=C.ef;float nq=C.D;if(C.D<0f)C.D=(float)fv;
C.D=Math.Abs(C.D-(float)fv)>0.05f ? C.D+((float)fv-C.D)*0.35f :(float)fv;float cQ=Math.Max(0f,Math.Min(1f,C.D/100f))*C.ba;
float gr=C.bt+C.ba/2f;float gy=C.bb;if(a7.i!=e.f4){var gF=bi.f[C.aa];gF.Size=new Vector2(C.aD,cQ);
gF.Position=new Vector2(gy,gr-cQ/2f);gF.Color=gu;bi.f[C.aa]=gF;if(q0||C.D!=nq)aO(bi,C,bf(pU,a7),gu);
return q0||C.D!=nq;}float ji=C.ba/b_;string fg=a7.bv!=null ? z.db(a7.bv,a7.d3!=null ? a2(a7.d3,a7.h5): fv): null;
Color ul=fg!=null ? K(fg): gu;string[]qv=fg!=null ?(fg.Contains("/")? fg.Split('/'): null): a7.bU&&a7.aN.Length>1 ? a7.aN : null;
for(int cK=0;cK<b_;cK++){float q5=Math.Max(0f,Math.Min(ji,cQ-cK*ji));var hj=bi.f[C.E[cK]];hj.Size=new Vector2(C.aD-6f,q5);
hj.Position=new Vector2(gy,gr-cK*ji-q5/2f);hj.Color=qv!=null ? d7(qv,100.0*(cK+0.5)/b_): ul;bi.f[C.E[cK]]=hj;}
int s7=Math.Max(1,(int)((C.aD-8f)/C.mj));bool vr=cQ>6f;var l5=bi.f[C.E[b_]];l5.Data=vr ? su.Substring((int)(T*5.0)%6,Math.Min(s7,100)): "";
l5.Position=new Vector2(gy,gr-cQ-12f);bi.f[C.E[b_]]=l5;for(int cg=0;cg<3;cg++){var jr=bi.f[C.E[b_+1+cg]];
double uN=(T*(0.35+0.1*cg)+cg*0.37)%1.0;jr.Data=cQ>24f ?(cg==1 ? "O" : "o"): "";jr.Position=new Vector2(gy+(cg-1)*C.aD*0.26f,gr-(float)uN*(cQ-6f)-10f);
bi.f[C.E[b_+1+cg]]=jr;}aO(bi,C,bf(pU,a7),gu);return true;}static void aO(aA nz,cz ly,string vR,Color ta){
var l_=nz.f[ly.a4];l_.Data=ly.ef.io ? "" : vR;l_.Color=ta;nz.f[ly.a4]=l_;}static void mO(aA nA,cz qp,Color tb){
var oc=nA.f[qp.aa];oc.Color=tb;nA.f[qp.aa]=oc;}bool sg(F bN,IMyTextSurface lK){aA k;
if(!b3.TryGetValue(bN,out k)){k=new aA();b3[bN]=k;}bool pY= !k.eb||k.ed!=lK.SurfaceSize;
if(pY){ro(bN,lK,k);}bool S=pY;bool ey=(T%bN.hV)<bN.hV/2.0;double cN= -1.0,nT;if(f7.TryGetValue(bN,out nT)){
cN=(T-nT)/bN.h3;if(cN>=1.0){f7.Remove(bN);cN=1.0;}}bool l3=cH&&ey&&k.fA.A>0;if(k.cu>=0&&l3!=k.fB){
k.fB=l3;for(int eS=0;eS<k.fD.Length;eS++){var nr=k.f[k.cu+eS];nr.Color=l3 ? k.fA : k.fD[eS];k.f[k.cu+eS]=nr;
}S=true;foreach(var sX in k.dZ)sX.id=true;}foreach(var an in k.dZ){if(an.mz&&k.fB)continue;Color eB=an.hX;try{if(an.f2!=null){
double nB=a2(an.h0,an.h8);string ho=z.db(an.f2,nB);eB=ho!=null&&ho.Contains("/")? d7(ho.Split('/'),nB): K(ho);
}if(an.fQ!=null&&ey&&z.db(an.fQ,a2(an.h9,an.ib))!=null)eB=k.fA;}catch{}if(eB!=an.fK||an.id){
an.fK=eB;an.id=false;var ny=k.f[an.fU];ny.Color=eB;k.f[an.fU]=ny;S=true;}}for(int ki=0;ki<k.ei.Count;ki++){var b=k.ei[ki];
var h=b.ef;double H;bool oK=false;double kL=h.fZ;try{H=a2(h.aM,h.J);if(h.c9!=null)kL=a2(h.c9,h.ij);
}catch{oK=true;H=0.0;}if(oK){if(b.fW!= -99999.0||(b.a4>=0&&k.f[b.a4].Data!="ERR")){S=true;b.fW= -99999.0;
if(b.a4>=0){var lq=k.f[b.a4];lq.Data="ERR";lq.Color=Color.Red;k.f[b.a4]=lq;}}continue;}double a8=H;
if(kL>0)H=Math.Min(100.0,a8/kL*100.0);bool qz=cN>=0.0&&h.i!=e.iF&&h.i!=e.cF&&h.i!=e.cx&&h.i!=e.cD&&h.i!=e.cy;
if(qz)H=cN<0.5 ? cN*200.0 : 100.0+(H-100.0)*(cN-0.5)*2.0;double gx=h.d3!=null ? a2(h.d3,h.h5): H;string gv=h.bv!=null ? z.db(h.bv,gx): h.bU ? z.fR(h.aN,gx): h.az;
Color P=K(gv);bool bl=(P==Color.Red);if(gv!=null&&gv.Contains("/"))P=d7(gv.Split('/'),gx);if(h.bv==null&&h.bU&& !bN.fS&&h.aN.Length>1)P=d7(h.aN,gx);
if(bl&&ey){P=new Color(255,75,75);S=true;}if(h.h1!=null&&z.db(h.h1,a2(h.mc,h.md))!=null){
bl=S=true;if(ey)P=new Color(P.R/3,P.G/3,P.B/3);}bool a_=qz||Math.Abs(a8-b.fW)>0.1;if(a_)b.fW=a8;
if(b.mV){if(st(k,b,H,a8,P,a_))S=true;continue;}if(h.i==e.dX||h.i==e.fM){double oj=(h.ih||h.J=="percent"||h.J=="pct")? H : iA(h.aM);
if(b.D<0f)b.D=(float)oj;float hH=(float)oj;if(Math.Abs(b.D-hH)>0.05f){b.D+=(hH-b.D)*0.35f;
S=true;}else if(b.D!=hH){b.D=hH;S=true;}if(a_)S=true;if(S){float pK=(float)Math.Max(0.0,Math.Min(1.0,b.D/100.0));
float jZ=b.aD*pK;if(h.i==e.fM){int eH=b.E.Count-1;for(int dq=0;dq<eH;dq++){var qj=k.f[b.E[dq]];
qj.Color=b.D>=(dq+0.5)/eH*100.0 ?(h.bv==null&&h.bU&&h.aN.Length>1 ? d7(h.aN,(dq+0.5)/eH*100.0): P): al(h,new Color(24,28,36));
k.f[b.E[dq]]=qj;}float i4=pK*3.14159f;var kO=k.f[b.E[eH]];kO.Position=new Vector2(b.bb-(float)Math.Cos(i4)*b.aD*0.45f,b.bt-(float)Math.Sin(i4)*b.aD*0.45f);
kO.RotationOrScale=i4-1.5708f;k.f[b.E[eH]]=kO;}else{var gH=k.f[b.aa];gH.Size=new Vector2(jZ,b.ba);
gH.Position=new Vector2(h.fN=="left" ? b.bb+b.aD-(jZ/2f): b.bb+(jZ/2f),b.bt);gH.Color=P;
k.f[b.aa]=gH;}aO(k,b,bf(a8,h),P);}}else if(h.i==e.ee||h.i==e.ea||h.i==e.d1||h.i==e.d4){
if(a_||bl){S=true;double tx=(h.ih||h.J=="percent"||h.J=="pct")? H : iA(h.aM);int qf=b.E.Count;
for(int fn=0;fn<qf;fn++){bool um=tx>=((fn+0.4)/(double)qf*100.0);Color vp=um ? P : al(h,new Color(24,28,36));var qg=k.f[b.E[fn]];
qg.Color=vp;k.f[b.E[fn]]=qg;}aO(k,b,bf(a8,h),P);}}else if(h.i==e.cB){
if(a_||bl){S=true;for(int gP=0;gP<b.cC.Length-1;gP++)b.cC[gP]=b.cC[gP+1];b.cC[b.cC.Length-1]=(float)H;
int uU=b.E.Count;for(int e6=0;e6<uU;e6++){float tP=b.cC[e6];float tO=Math.Max(0.06f,Math.Min(1f,tP/100f));float nN=b.ba*tO;
float tc=b.bt+(b.ba/2f)-(nN/2f);var dp=k.f[b.E[e6]];dp.Size=new Vector2(dp.Size.Value.X,nN);
dp.Position=new Vector2(dp.Position.Value.X,tc);dp.Color=P;k.f[b.E[e6]]=dp;}aO(k,b,bf(a8,h),P);
}}else if(h.i==e.c3){if(b.D<0f)b.D=(float)H;float hI=(float)H;if(Math.Abs(b.D-hI)>0.05f){
b.D+=(hI-b.D)*0.35f;S=true;}else if(b.D!=hI){b.D=hI;S=true;}if(S||a_){
float oJ=b.aD*0.5f;float nI=b.bb+oJ;float nZ=(float)Math.Max(-1.0,Math.Min(1.0,(b.D-50.0)/50.0));float j_=Math.Abs(nZ)*oJ;
float ty=(nZ>=0)?(nI+j_*0.5f):(nI-j_*0.5f);var gI=k.f[b.aa];gI.Size=new Vector2(j_,b.ba);
gI.Position=new Vector2(ty,b.bt);gI.Color=P;k.f[b.aa]=gI;aO(k,b,bf(a8,h),P);
}}else if(h.i==e.cF){if(a_||bl){S=true;bool kq=H>=50.0;float o_=Math.Max(4f,b.ba);float t2=kq ?(b.bb+b.aD-o_*0.5f):(b.bb+o_*0.5f);
Color oZ=kq ?(P!=Color.White ? P : new Color(34,197,94)): new Color(60,70,85);var jX=k.f[b.aa];jX.Position=new Vector2(t2,b.bt);
jX.Color=oZ;k.f[b.aa]=jX;aO(k,b,kq ? "ON" : "OFF",oZ);}}else if(h.i==e.cx){if(a_||bl){
S=true;bool kp=H>=50.0;Color nJ=kp ?(P!=Color.White ? P : new Color(56,189,248)): new Color(30,36,48);mO(k,b,nJ);
aO(k,b,kp ? "ACTIVE" : "READY",kp ? nJ : new Color(148,163,184));}}else if(h.i==e.cD){if(a_||bl){
S=true;string vC=H>=75.0 ? "ONLINE" :(H>=25.0 ? "STBY" : "OFFLINE");Color qu=H>=75.0 ? new Color(34,197,94):(H>=25.0 ? new Color(234,179,8): new Color(239,68,68));
mO(k,b,qu);aO(k,b,vC,qu);}}else if(h.i==e.c1){if(a_||bl){S=true;int pL=b.E.Count;
for(int e7=0;e7<pL;e7++){bool un=H>=((e7+0.5)/(double)pL*100.0);Color uP=un ? P : al(h,new Color(24,28,36));var pM=k.f[b.E[e7]];
pM.Color=uP;k.f[b.E[e7]]=pM;}aO(k,b,bf(a8,h),P);}}else if(h.i==e.cy){
S=true;float hp;Color eM;string gJ;bool tV=(h.J=="producing"||h.J=="active"||h.J=="running"||h.J=="state"||(h.J!="percent"&&(H<=1.0||H==100.0)));
if(tV){if(H>0.0){b.D=(b.D+0.3927f)%6.28318f;hp=b.D;eM=new Color(34,197,94);gJ="RUN";}else{hp=0f;
eM=new Color(148,163,184);gJ="IDLE";}}else{hp=(H>55.0)? 0f :((H<45.0)? 3.14159f : 1.57079f);eM=(H>55.0)? new Color(34,197,94):((H<45.0)? new Color(239,68,68): new Color(148,163,184));
gJ=(H>55.0)? "CHG" :((H<45.0)? "DSC" : "STBY");}var jY=k.f[b.aa];jY.RotationOrScale=hp;jY.Color=eM;
k.f[b.aa]=jY;aO(k,b,gJ,eM);}else{string gE;bu hK;ep.TryGetValue(h.aM,out hK);bC.TryGetValue(mA(h.J)? h.aM+"."+h.J :(hK!=null&&hK.bw!=null ? hK.bw : h.aM),out gE);
if(a_||bl||gE!=b.mB){b.mB=gE;S=true;Color lX=P;if(lX==Color.Red&&ey)lX=new Color(255,75,75);aO(k,b,gE ?? bf(a8,h),lX);
}}if(b.mv&&(a_||bl)){S=true;var o9=k.f[b.c8];string uL=mt(H);string vc=bf(a8,h);
o9.Data=h.ab.Replace("{percent}",uL).Replace("{val}",vc);k.f[b.c8]=o9;}}if(S){k.en++;using(var tE=lK.DrawFrame()){
tE.AddRange(k.f);}}return S;}bool sf(F cn,IMyTextSurface lL){lL.FontSize=cn.c6;var gw=new List<KeyValuePair<string,string>>();
foreach(var o in cn.fX){double U=0.0;string bp;string bj=null;double kM=o.fZ;try{U=a2(o.aM,o.J);if(o.c9!=null)kM=a2(o.c9,o.ij);
}catch(Exception wi){gw.Add(new KeyValuePair<string,string>($"{o.ab}: [UNRESOLVED]","red"));continue;}double pV=U;if(kM>0)U=Math.Min(100.0,pV/kM*100.0);
string qY=bf(pV,o);if(o.i==e.dX){double ok=(o.ih||o.J=="percent"||o.J=="pct")? U : iA(o.aM);
bp=z.r8(o.ab,ok,10,qY);bj=o.bU ? z.fR(o.aN,ok): o.az;}else if(o.i==e.c3){
bp=z.r6(o.ab,U,10);bj=o.bU ? z.fR(o.aN,U): o.az;}else if(o.i==e.cF){
bp=z.si(o.ab,U);bj=o.az ??(U>=50.0 ? "green" : "gray");}else if(o.i==e.cx){bp=z.sb(o.ab,U);
bj=o.az ??(U>=50.0 ? "cyan" : "gray");}else if(o.i==e.cD){bp=z.sh(o.ab,U);bj=o.az ??(U>=75.0 ? "lime" :(U>=25.0 ? "yellow" : "red"));
}else if(o.i==e.c1){bp=z.r5(o.ab,U,8);bj=o.bU ? z.fR(o.aN,U):(o.az ?? "yellow");
}else if(o.i==e.cy){bool tW=(o.J=="producing"||o.J=="active"||o.J=="running"||o.J=="state"||(o.J!="percent"&&(U<=1.0||U==100.0)));
if(tW){char[]vA=new[]{'|','/','-','\\'};char vm=vA[Math.Abs(ev)%4];bp=U>0 ? $"{o.ab} [{vm}] RUN" : $"{o.ab} [==] IDLE";
bj=U>0 ? "green" : "gray";}else{bp=z.sc(o.ab,U,ev);bj=o.az ??(U>55.0 ? "green" :(U<45.0 ? "red" : "cyan"));
}}else{bp=$"{o.ab}: {qY}";bj=o.bv!=null ? z.db(o.bv,U): null;}gw.Add(new KeyValuePair<string,string>(bp,bj));
}var gq=z.sa(new List<string>(gw.ConvertAll(vM=>vM.Key)),dT: cn.mW);string qm=string.Join("\n",gq);
string pO;if(cI.TryGetValue(cn,out pO)&&pO==qm)return false;cI[cn]=qm;using(var tF=lL.DrawFrame()){
float re=4f;float pb=rf*cn.c6;try{var pe=lL.MeasureStringInPixels(new StringBuilder("A"),"Monospace",cn.c6);
if(pe.Y>0)pb=pe.Y;}catch{}for(int dv=0;dv<gq.Count;dv++){string nR=null;if(dv>0&&dv<gq.Count-1)nR=gw[dv-1].Value;
var qt=MySprite.CreateText(gq[dv],"Monospace",K(nR),cn.c6,TextAlignment.LEFT);qt.Position=new Vector2(4f,re);tF.Add(qt);
re+=pb;}}return true;}Color K(string g7){if(string.IsNullOrEmpty(g7))return Color.White;Color e0;if(m_.TryGetValue(g7,out e0))return e0;
p eE=p.eg(g7);e0=eE.fV ? Color.Transparent : new Color(eE.bY,eE.bT,eE.bO,eE.hU);m_[g7]=e0;return e0;}public enum bc{rj,ig,d5}
public enum aL{rR,hW,mR,mD,mC}public class bu{public string ag;public bc bV;public List<string>a3=new List<string>();public string mS{
get{return a3.Count>0 ? a3[0]: "";}set{if(a3.Count==0)a3.Add(value);else a3[0]=value;}}public aL dU=aL.rR;
public double fL=0.0;public string h6;public string bw;public bool fC;}public enum e{iF,dX,ee,cB,ea,d1,d4,c3,cF,cx,cD,c1,cy,f4,fM}
public class ay{public string fO;public string bS;public bool c7;}public class bx{public e i;public string ab;public string aM;
public string J;public bool bU;public string[]aN;public string az;public List<ay>bv;public string fN="right";
public string em;public string d6;public List<double>fY;public string fT;public List<ay>h1;public string mc,md;public bool io;
public int bA;public int iG;public string it;public int f3;public string d3,h5;public string ec,iJ;public double fZ;public string c9,ij;
public bool ih{get{return fZ>0||c9!=null;}}}public struct p{public byte bY,bT,bO,hU;public bool fV;public p(byte u3,byte tH,byte sO,byte sw=255){
bY=u3;bT=tH;bO=sO;hU=sw;fV=false;}public static readonly p ss=new p(0,0,0,0){fV=true};public static readonly p f5=new p(255,255,255);
public static readonly p ri=new p(0,0,0);public static readonly p r3=new p(255,0,0);public static readonly p rK=new p(0,255,0);
public static readonly p rO=new p(50,205,50);public static readonly p sv=new p(255,255,0);public static readonly p rS=new p(255,140,0);
public static readonly p ru=new p(0,255,255);public static readonly p rk=new p(0,120,255);public static readonly p rP=new p(255,0,255);
public static readonly p r1=new p(128,0,128);public static readonly p rJ=new p(128,128,128);public static readonly p rv=new p(60,60,60);
public static p eg(string bo){if(string.IsNullOrWhiteSpace(bo))return f5;bo=bo.Trim();if(bo.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bo.Equals("none",StringComparison.OrdinalIgnoreCase))return ss;
if(bo.StartsWith("#")){string ce=bo.Substring(1).Trim();if(ce.Length==6){byte u4=Convert.ToByte(ce.Substring(0,2),16);byte tI=Convert.ToByte(ce.Substring(2,2),16);
byte sP=Convert.ToByte(ce.Substring(4,2),16);return new p(u4,tI,sP);}if(ce.Length==3){byte u5=Convert.ToByte(new string(ce[0],2),16);byte tJ=Convert.ToByte(new string(ce[1],2),16);
byte sQ=Convert.ToByte(new string(ce[2],2),16);return new p(u5,tJ,sQ);}}if(bo.Contains(",")){var dD=bo.Split(',');if(dD.Length>=3){byte u6=(byte)Math.Max(0,Math.Min(255,int.Parse(dD[0].Trim())));
byte tK=(byte)Math.Max(0,Math.Min(255,int.Parse(dD[1].Trim())));byte sR=(byte)Math.Max(0,Math.Min(255,int.Parse(dD[2].Trim())));byte sx=dD.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(dD[3].Trim()))):(byte)255;
return new p(u6,tK,sR,sx);}}switch(bo.ToLowerInvariant()){case "red": return r3;case "green": return rK;case "lime": return rO;case "yellow": return sv;
case "orange": return rS;case "cyan": return ru;case "blue": return rk;case "magenta": return rP;case "purple": return r1;case "white": return f5;
case "black": return ri;case "gray": case "grey": return rJ;case "darkgray": case "darkgrey": return rv;default: return f5;}}}public class bQ{
public string[]bR;public string dY="vertical";public List<ay>a0;public string hY,hZ;public List<ay>c5;public string ia,ic;
}public class aF : bQ{public string da="default";public int d2=0;public int eh=0;public int ej=1;public int ek=1;public string cE;public string iH="left";
public string iI;public string d_;public string mo;public float? mq;public string c2;public string fH;public string[]fI;
public string ir="horizontal";public List<bx>aq=new List<bx>();}public class F : bQ{public string bg;public string f0="standard";
public string c4="graphic";public string d0;public string[]fJ;public string mT="left";public string mU;public string mp="Monospace";
public string bP;public string mg;public int mh=10;public int me=1;public float h3=1.2f;public float ie= -1f,iu= -1f;
public bool fS;public string iv;public string h_;public string mb;public string l9;public string il;public string ik;
public string ct;public string cw;public float h4=2.5f;public string mf="cyan";public string[]fF;public List<string>fG=new List<string>();
public List<string>rG(){if(fG!=null&&fG.Count>0)return fG;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float c6=1.0f;public int iy=0;public float hV=1.6f;public int d8=1;public int d9=1;public int mW=30;public string bW;
public int cA=0;public List<aF>ak=new List<aF>();public List<bx>fX{get{if(ak.Count==0)ak.Add(new aF{da="main"}
);return ak[0].aq;}}}public class b0{public string im;public List<string>f_=new List<string>();public Dictionary<string,string>h7=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>h2=new List<string>();}public class ix{public List<bu>cv=new List<bu>();public List<F>Z=new List<F>();
public List<string>be=new List<string>();public List<string>eo=new List<string>();public Dictionary<string,List<string>>dW=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class mx{static readonly Dictionary<string,b0>mi=new Dictionary<string,b0>(StringComparer.OrdinalIgnoreCase);
static mx(){r4();}static void r4(){bZ("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bZ("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bZ("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bZ("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bZ("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bZ("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bZ("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bZ("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bZ(string pn,string[]uJ,string s0){var hF=new b0{im=pn}
;foreach(var uA in uJ){var kz=uA.Split(new[]{'='},2);string pB=ip(kz[0]);hF.f_.Add(pB);if(kz.Length>1)hF.h7[pB]=ae(kz[1].Trim());
}var uk=s0.Replace("\r\n","\n").Split('\n');foreach(var t8 in uk){string p5=t8.Trim();if(p5.Length>0)hF.h2.Add(p5);}mi[pn]=hF;}static string ip(string dB){
dB=dB.Trim();if(dB.StartsWith("$"))dB=dB.Substring(1);return dB.ToLowerInvariant();}public static ix rU(string tf){var au=new ix();F d=null;
aF u=null;var pN=r_(tf,au.be,au.dW);for(int eT=0;eT<pN.Count;eT++){string a=pN[eT].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){au.cv.Add(rV(a));}else if(a.StartsWith("@screen")){d=new F{bg=a.Substring("@screen".Length).Trim()}
;au.Z.Add(d);u=null;}else if(a.StartsWith("profile:")){s(d,"profile:");string pX=a.Substring("profile:".Length).Trim();
string pq=z.mF(pX);if(pq==null)throw new FormatException("unknown profile '"+pX+"' (expected auto/standard/compact/wide/corner or a hardware profile name)");
d.f0=pq;}else if(a.StartsWith("border:")){s(d,"border:");string nj=a.Substring("border:".Length).Trim();if(u!=null)u.d_=nj;
else d.c4=nj;}else if(a.StartsWith("border_color:")){s(d,"border_color:");string nl=a.Substring("border_color:".Length).Trim();
if(u!=null)u.fH=nl;else d.d0=nl;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
s(d,"border_gradient");string cL=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
cL=cL.Trim();if(cL.StartsWith("(")&&cL.Contains(")"))cL=a1(cL,0);string[]qw=aP(cL).Select(vg=>vg.Trim()).ToArray();if(u!=null)u.fI=qw;
else d.fJ=qw;}else if(a.StartsWith("title_align:")){s(d,"title_align:");string qD=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(u!=null)u.iH=qD;else d.mT=qD;}else if(a.StartsWith("title_color:")){s(d,"title_color:");string qG=a.Substring("title_color:".Length).Trim();
if(u!=null)u.iI=qG;else d.mU=qG;}else if(a.StartsWith("font_size:")){s(d,"font_size:");float qA=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(u!=null)u.mq=qA;else d.c6=qA;}else if(a.StartsWith("font:")){s(d,"font:");string od=a.Substring("font:".Length).Trim();
if(u!=null)u.mo=od;else d.mp=od;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){s(d,"bg:");string dg=a.Substring(a.IndexOf(':')+1).Trim();
bQ jh=u!=null ?(bQ)u : d;if(dg.StartsWith("color(")&&dg.EndsWith(")")){string nu;jh.a0=mI(dg.Substring(6,dg.Length-7),out nu);
string ns,nt;iB(nu,out ns,out nt);jh.hY=ns;jh.hZ=nt;}else if(u!=null)u.c2=dg;
else d.bP=dg;}else if(a.StartsWith("layout:")){s(d,"layout:");rW(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
s(d,"alert_period:");d.hV=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){s(d,"refresh:");d.iy=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
s(d,"output:");var hc=aP(a.Substring("output:".Length).Trim());if(hc.Count==0)throw new FormatException("output: requires a target block name");
d.bW=ae(hc[0]);d.cA=hc.Count>1 ? int.Parse(hc[1]): 0;}else if(a.StartsWith("width:")){s(d,"width:");
d.mW=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){s(d,"boot_msg:");
d.mg=ae(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){s(d,"boot_stagger:");d.mh=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){s(d,"boot_calibrate:");string nU=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.h3=nU=="off" ? 0f : Math.Max(0f,float.Parse(nU,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){s(d,"boot_batch:");
d.me=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){s(d,"boot_duration:");
string eG=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool oP=eG.EndsWith("s");if(oP)eG=eG.Substring(0,eG.Length-1).Trim();
float jN;if(float.TryParse(eG,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out jN)){d.h4=oP ? jN : jN/60f;
}}else if(a.StartsWith("boot_log:")){s(d,"boot_log:");d.fG.Add(ae(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("mirror_panel:")){
s(d,"mirror_panel:");d.il=ae(a.Substring("mirror_panel:".Length).Trim());}else if(a.StartsWith("mirror:")){s(d,"mirror:");
d.ik=ae(a.Substring("mirror:".Length).Trim());}else if(a.StartsWith("banner:")){s(d,"banner:");d.ct=ae(a.Substring("banner:".Length).Trim());
}else if(a.StartsWith("boot_logo:")){s(d,"boot_logo:");d.cw=ae(a.Substring("boot_logo:".Length).Trim());}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){
s(d,"boot_gradient");d.fF=mJ(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){s(d,"bg_gradient");
(u!=null ?(bQ)u : d).bR=mJ(a);}else if(a.StartsWith("bg_gradient_dir:")){s(d,"bg_gradient_dir:");
(u!=null ?(bQ)u : d).dY=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(a.StartsWith("flash:")){
s(d,"flash:");bQ jV=u!=null ?(bQ)u : d;string jU=a.Substring("flash:".Length).Trim();string of=iE(jU);
string oe,og;iB(jU.Substring(0,jU.Length-of.Length),out oe,out og);jV.ia=oe;jV.ic=og;jV.c5=new List<ay>{
new ay{fO=of,bS="on"}};}else if(a.StartsWith("bg_texture:")){s(d,"bg_texture:");d.h_=ae(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){s(d,"bg_texture_color:");d.mb=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gap:")||a.StartsWith("padding:")){bool oT=a.StartsWith("gap:");s(d,oT ? "gap:" : "padding:");float pQ=float.Parse(a.Substring(a.IndexOf(':')+1).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(oT)d.ie=pQ;else d.iu=pQ;}else if(a.StartsWith("gradient_mode:")){s(d,"gradient_mode:");d.fS=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){s(d,"screen_bg:");d.bP=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
s(d,"panel_bg:");d.iv=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){s(d,"alert_bg:");
d.l9=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){s(d,"boot_color:");
d.mf=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){s(d,"orientation:");string ux=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(u!=null)u.ir=ux;}else if(a.StartsWith("@panel")){s(d,"@panel");u=rY(a);d.ak.Add(u);
}else if(a.StartsWith("title:")){s(d,"title:");if(u==null){u=new aF{da="main",d2=0,eh=0,ej=d.d8,ek=d.d9}
;d.ak.Add(u);}u.cE=ae(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){s(d,"line:");
if(u==null){u=new aF{da="main",d2=0,eh=0,ej=d.d8,ek=d.d9};d.ak.Add(u);}u.aq.Add(rX(a.Substring("line:".Length).Trim()));
}else{au.be.Add($"Line {eT + 1}: unrecognized directive: \"{a}\"");}}catch(Exception tr){au.be.Add($"Line {eT + 1}: parse error ({tr.Message}) in \"{a}\"");
}}foreach(var p6 in au.Z){if(string.IsNullOrEmpty(p6.bW))au.be.Add($"@screen {p6.bg}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var ax in au.Z){int kW=ax.ak.Count(lb=>lb.c2!=null||lb.a0!=null||lb.bR!=null);if(ax.ak.Count>1&&kW>0&&kW<ax.ak.Count)au.eo.Add("screen '"+ax.bg+"': only "+kW+" of "+ax.ak.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(ax.ct!=null&& !au.dW.ContainsKey(ax.ct))au.be.Add("Screen '"+ax.bg+"': banner '"+ax.ct+"' is not defined (@banner "+ax.ct+" ... @end)");
if(ax.cw!=null&& !au.dW.ContainsKey(ax.cw))au.be.Add("Screen '"+ax.bg+"': boot_logo '"+ax.cw+"' is not defined (@banner "+ax.cw+" ... @end)");
}return au;}static void rW(string jo,F qd){int k3=jo.IndexOf('(');if(k3<0)throw new FormatException("missing '(' in layout: specification");
string oY=jo.Substring(0,k3).Trim().ToLowerInvariant();string sE=a1(jo,k3);var i6=aP(sE);if(oY=="grid"){if(i6.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
qd.d8=Math.Max(1,int.Parse(i6[0]));qd.d9=Math.Max(1,int.Parse(i6[1]));}else{throw new FormatException($"unknown layout kind '{oY}' (expected grid)");
}}static aF rY(string ug){string fi=ug.Substring("@panel".Length).Trim();int gj=fi.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(gj<0)throw new FormatException("@panel requires an at(...) positioning clause");
string us=fi.Substring(0,gj).Trim();string nf=fi.Substring(gj);int pI=nf.IndexOf('(');string nd=a1(nf,pI);
var cJ=aP(nd);if(cJ.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bJ=new aF{da=us,d2=int.Parse(cJ[0]),eh=int.Parse(cJ[1]),ej=cJ.Count>2 ? int.Parse(cJ[2]): 1,ek=cJ.Count>3 ? int.Parse(cJ[3]): 1}
;int m6=gj+pI+nd.Length+1;if(m6<fi.Length){string aW=fi.Substring(m6).Trim();int qL=aW.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(qL>=0){string fq=aW.Substring(qL+5).Trim();if(fq.StartsWith("(")&&fq.Contains(")"))bJ.cE=ae(a1(fq,0));else if(fq.StartsWith(":")){
string cM=fq.Substring(1).Trim();if(cM.StartsWith("\"")){int n9=cM.IndexOf('"',1);bJ.cE=n9>0 ? cM.Substring(1,n9-1): ae(cM);}else{int qr=cM.IndexOf(' ');
bJ.cE=qr>=0 ? cM.Substring(0,qr): cM;}}}string ps=fP(aW,"orientation:");if(ps!=null)bJ.ir=ps.ToLowerInvariant();string m4=fP(aW,"title_align:");
if(m4!=null)bJ.iH=m4.ToLowerInvariant();string qI=fP(aW,"title_color:");if(qI!=null)bJ.iI=qI;int gp=aW.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(gp>=0&& !aW.Substring(gp).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aW.Substring(gp).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bJ.d_=fP(aW.Substring(gp),"border:");}int gm=aW.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(gm<0)gm=aW.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(gm>=0){string cc=aW.Substring(gm+"border_gradient".Length).Trim();if(cc.StartsWith(":"))cc=cc.Substring(1).Trim();if(cc.StartsWith("(")&&cc.Contains(")"))cc=a1(cc,0);
bJ.fI=aP(cc).Select(vh=>vh.Trim()).ToArray();}}return bJ;}static string fP(string p7,string oX){int oO=p7.IndexOf(oX,StringComparison.OrdinalIgnoreCase);
if(oO<0)return null;string l0=p7.Substring(oO+oX.Length).Trim();int qs=l0.IndexOf(' ');return qs>=0 ? l0.Substring(0,qs): l0;}static string[]mJ(string kF){
int j5=kF.IndexOfAny(new[]{':','('});string eO=kF.Substring(kF[j5]==':' ? j5+1 : j5).Trim();if(eO.StartsWith("(")&&eO.Contains(")"))eO=a1(eO,0);return aP(eO).Select(vi=>vi.Trim()).ToArray();
}static List<string>r_(string tg,List<string>jS,Dictionary<string,List<string>>sT){var qT=new Dictionary<string,b0>(StringComparer.OrdinalIgnoreCase);
var dH=(tg ?? "").Replace("\r\n","\n").Split('\n');var kV=new List<string>();int dw=0;while(dw<dH.Length){string lg=dH[dw++];if(lg.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ar=new List<string>();while(dw<dH.Length){string m2=dH[dw++].Replace("\t","    ").TrimEnd();if(m2.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ar.Add(m2);}while(ar.Count>0&&ar[0].Length==0)ar.RemoveAt(0);while(ar.Count>0&&ar[ar.Count-1].Length==0)ar.RemoveAt(ar.Count-1);int ko=int.MaxValue;
foreach(var iY in ar)if(iY.Length>0)ko=Math.Min(ko,iY.Length-iY.TrimStart().Length);for(int e1=0;e1<ar.Count;e1++)if(ar[e1].Length>0)ar[e1]=ar[e1].Substring(ko);
sT[lg.TrimStart().Substring(8).Trim()]=ar;continue;}string ch=mQ(lg).Trim();if(ch.Length==0)continue;if(ch.StartsWith("@template")){try{
var lP=rZ(ch);while(dw<dH.Length){string s2=dH[dw++];string jp=mQ(s2).Trim();if(jp.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(jp.Length>0)lP.h2.Add(jp);}qT[lP.im]=lP;}catch(Exception ts){jS.Add($"Template parse error ({ts.Message}) in \"{ch}\"");}}
else if(ch.StartsWith("@use")){try{ml(ch,qT,kV,jS);}catch(Exception tt){jS.Add($"@use error ({tt.Message}) in \"{ch}\"");
}}else{kV.Add(ch);}}return kV;}static b0 rZ(string uh){string lh=uh.Substring("@template".Length).Trim();int k4=lh.IndexOf('(');
if(k4<0)throw new FormatException("missing '(' in @template header");string ut=lh.Substring(0,k4).Trim();string sF=a1(lh,k4);var lQ=new b0{
im=ut};foreach(var uB in aP(sF)){var kA=uB.Split(new[]{'='},2);string pC=ip(kA[0]);lQ.f_.Add(pC);if(kA.Length>1)lQ.h7[pC]=ae(kA[1].Trim());
}return lQ;}static void ml(string ui,Dictionary<string,b0>qU,List<string>px,List<string>tq,int nY=0){string hn=ui.Substring("@use".Length).Trim();
int ha=hn.IndexOf('(');string kN=ha>=0 ? hn.Substring(0,ha).Trim(): hn.Trim();string sG=ha>=0 ? a1(hn,ha): "";b0 dQ=null;
if(!qU.TryGetValue(kN,out dQ)){if(!mi.TryGetValue(kN,out dQ))throw new FormatException($"unknown template '{kN}'");}var gs=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var o1 in dQ.h7)gs[o1.Key]=o1.Value;var nc=aP(sG);for(int ex=0;ex<nc.Count;ex++){string gh=nc[ex];int jQ=gh.IndexOf('=');if(jQ>=0){
string t0=ip(gh.Substring(0,jQ));string vZ=ae(gh.Substring(jQ+1).Trim());gs[t0]=vZ;}else{if(ex<dQ.f_.Count){string t1=dQ.f_[ex];
gs[t1]=ae(gh.Trim());}}}foreach(var s1 in dQ.h2){string eK=s1;foreach(var g2 in gs){eK=eK.Replace("$"+g2.Key,g2.Value).Replace("{"+g2.Key+"}",g2.Value);
}if(eK.StartsWith("@use")){if(nY>3)throw new FormatException("@use nested too deep");ml(eK,qU,px,tq,nY+1);
}else px.Add(eK);}}static void s(F te,string tn){if(te==null)throw new FormatException($"'{tn}' used before any @screen declared");
}static string mQ(string bm){if(string.IsNullOrEmpty(bm))return bm;bool kn=false;for(int cf=0;cf<bm.Length;cf++){char ju=bm[cf];if(ju=='"'){kn= !kn;
}else if(!kn){if(ju=='/'&&cf+1<bm.Length&&bm[cf+1]=='/'){return bm.Substring(0,cf);}if(ju=='#'){if(rM(bm,cf)){continue;}return bm.Substring(0,cf);}}}return bm;
}static bool rM(string cR,int oR){int dz=0;for(int kj=oR+1;kj<cR.Length;kj++){char dk=cR[kj];if((dk>='0'&&dk<='9')||(dk>='a'&&dk<='f')||(dk>='A'&&dk<='F'))dz++;
else break;}if(dz==3||dz==4||dz==6||dz==8){int e5=oR+1+dz;if(e5>=cR.Length||char.IsWhiteSpace(cR[e5])||cR[e5]==')'||cR[e5]==','||cR[e5]==':')return true;
}return false;}static bu rV(string uj){string li=uj.Substring("@bind".Length).Trim();int jR=li.IndexOf('=');if(jR<0)throw new FormatException("missing '=' in @bind");
string po=li.Substring(0,jR).Trim();string jT=li.Substring(jR+1).Trim();int k5=jT.IndexOf('(');if(k5<0)throw new FormatException("missing '(' in @bind target");
string gM=jT.Substring(0,k5).Trim().ToLowerInvariant();string sH=a1(jT,k5);var b6=aP(sH);if(b6.Count==0)throw new FormatException("@bind target has no arguments");
var av=new bu{ag=po};if(gM=="dynamic"||gM=="var"||gM=="mem"){av.bV=bc.d5;av.bw=b6.Count>0 ? ae(b6[0].Trim()): po;
for(int iZ=0;iZ<b6.Count;iZ++){var g3=b6[iZ].Split(new[]{':'},2);if(g3.Length==2&&g3[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double nX;
if(double.TryParse(g3[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nX))av.fL=nX;else av.h6=ae(g3[1].Trim());
}}return av;}av.bV=gM=="group" ? bc.ig : bc.rj;string gg=null;for(int gf=0;gf<b6.Count;gf++){var dy=b6[gf].Split(new[]{':'},2);if(dy.Length==2&&dy[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
gg=dy[1].Trim().ToLowerInvariant();}else if(dy.Length==2&&dy[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){av.fC=dy[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{av.a3.Add(ae(b6[gf].Trim()));}}if(av.a3.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(av.bV==bc.ig){if(gg==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(gg){case "avg": av.dU=aL.hW;break;case "sum": av.dU=aL.mR;break;case "min": av.dU=aL.mD;break;case "max": av.dU=aL.mC;
break;default: throw new FormatException($"unknown agg '{gg}' (expected avg/sum/min/max)");}}return av;}static bx rX(string go){var l=new bx();
if(!go.StartsWith("\""))throw new FormatException("line must start with a quoted label");int jP=go.IndexOf('"',1);if(jP<0)throw new FormatException("unterminated quoted label");
l.ab=go.Substring(1,jP-1);string fh=go.Substring(jP+1).Trim();int hb=fh.IndexOf('(');if(hb<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string y=fh.Substring(0,hb).Trim().ToLowerInvariant();string sD=a1(fh,hb);string ai=fh.Substring(fh.IndexOf(')',hb)+1).Trim();
var jL=sD.Trim().Split('.');if(jL.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
l.aM=jL[0].Trim();l.J=jL[1].Trim();if(y=="bar")l.i=e.dX;else if(y=="led")l.i=e.ee;else if(y=="spark"||y=="sparkline")l.i=e.cB;
else if(y=="hazard"||y=="stripes")l.i=e.ea;else if(y=="chevron"||y=="chevrons")l.i=e.d1;else if(y=="dots"||y=="dot")l.i=e.d4;
else if(y=="balance"||y=="horizon"||y=="centered")l.i=e.c3;else if(y=="toggle"||y=="switch")l.i=e.cF;else if(y=="checkbox"||y=="check")l.i=e.cx;
else if(y=="status"||y=="badge"||y=="state")l.i=e.cD;else if(y=="ammo"||y=="pips")l.i=e.c1;else if(y=="flow"||y=="rate"||y=="spinner")l.i=e.cy;
else if(y=="text")l.i=e.iF;else if(y=="tank"||y=="tube"||y=="bucket")l.i=e.f4;else if(y=="dial"||y=="gauge"||y=="arc")l.i=e.fM;
else throw new FormatException($"unknown display function '{y}'");while(ai.Length>0){int jI=ai.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int dt=ai.IndexOf('(');if(jI>=0&&(dt<0||jI<dt)){string gz=ai.Substring(jI+4).Trim();int hw=gz.IndexOf(' ');l.fN=(hw>=0 ? gz.Substring(0,hw): gz).ToLowerInvariant();
ai=(hw>=0 ? gz.Substring(hw): "").Trim();continue;}int lZ=ai.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(lZ>=0&&(dt<0||lZ<dt)){
string hQ=ai.Substring(lZ+5).Trim();int hx=hQ.IndexOf(' ');l.em=(hx>=0 ? hQ.Substring(0,hx): hQ).Trim().Trim('"');ai=(hx>=0 ? hQ.Substring(hx): "").Trim();
continue;}int j0=ai.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(j0>=0&&(dt<0||j0<dt)){string gG=ai.Substring(j0+7).Trim();
int hy=gG.IndexOf(' ');l.d6=(hy>=0 ? gG.Substring(0,hy): gG).Trim().Trim('"');ai=(hy>=0 ? gG.Substring(hy): "").Trim();continue;}int g6=ai.IndexOf('(');
if(g6<0)break;string N=ai.Substring(0,g6).Trim().ToLowerInvariant();int o8=N.LastIndexOf(' ');if(o8>=0)N=N.Substring(o8+1).Trim();
string _=a1(ai,g6);ai=ai.Substring(ai.IndexOf(')',g6)+1).Trim();if(N=="gradient"){l.bU=true;l.aN=aP(_).Select(vj=>vj.Trim()).ToArray();
if(l.aN.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(N=="size"){string lO=_.Trim().ToLowerInvariant();
l.f3=lO=="large"||lO=="fill" ? 2 : lO=="medium" ? 1 : 0;}else if(N=="marker"){l.fY=new List<double>();foreach(var up in aP(_)){double pj;
if(!double.TryParse(up,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out pj))throw new FormatException("marker() takes numbers from 0 to 100");
l.fY.Add(pj);}}else if(N=="icon")l.fT=_.Trim();else if(N=="blink"){string js=_.Trim(),nm=iE(js);iB(js.Substring(0,js.Length-nm.Length),out l.mc,out l.md);
l.h1=new List<ay>{new ay{fO=nm,bS="on"}};}else if(N=="value")l.io=_.Trim().Equals("off",StringComparison.OrdinalIgnoreCase);
else if(N=="ticks")l.iG=int.Parse(_.Trim());else if(N=="segments"||N=="points")l.bA=Math.Max(2,Math.Min(40,int.Parse(_.Trim())));else if(N=="outline")l.it=_.Trim();
else if(N=="color_by"){var jz=_.Trim().Split('.');if(jz.Length!=2)throw new FormatException("color_by() takes bindname.field");l.d3=jz[0].Trim();
l.h5=jz[1].Trim();}else if(N=="label_color")l.ec=_.Trim();else if(N=="track")l.iJ=_.Trim();else if(N=="flat"){l.az=_.Trim();
}else if(N=="pattern"){string W=_.Trim().ToLowerInvariant();if(W=="hazard"||W=="stripes")l.i=e.ea;else if(W=="chevron"||W=="chevrons")l.i=e.d1;
else if(W=="dots"||W=="dot")l.i=e.d4;else if(W=="led")l.i=e.ee;else if(W=="balance"||W=="horizon")l.i=e.c3;
else if(W=="toggle"||W=="switch")l.i=e.cF;else if(W=="checkbox"||W=="check")l.i=e.cx;else if(W=="status"||W=="badge"||W=="state")l.i=e.cD;
else if(W=="ammo"||W=="pips")l.i=e.c1;else if(W=="flow"||W=="rate"||W=="spinner")l.i=e.cy;}else if(N=="color"){string tU;
l.bv=mI(_,out tU);}else if(N=="dir"||N=="direction"){l.fN=_.Trim().ToLowerInvariant();}else if(N=="unit"){
l.em=_.Trim().Trim('"');}else if(N=="max"){double pi;if(double.TryParse(_.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out pi))l.fZ=pi;
else{var kK=_.Trim().Split('.');if(kK.Length!=2)throw new FormatException("max() takes a number or bindname.field");l.c9=kK[0].Trim();l.ij=kK[1].Trim();
}}else if(N=="format"||N=="fmt"){l.d6=_.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{N}'");}}return l;}static string a1(string hs,int kT){
int jH=0;for(int eU=kT;eU<hs.Length;eU++){if(hs[eU]=='(')jH++;else if(hs[eU]==')'){jH--;if(jH==0)return hs.Substring(kT+1,eU-kT-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aP(string sI){return sI.Split(',').Select(vk=>vk.Trim()).Where(vl=>vl.Length>0).ToList();
}static List<ay>mI(string sC,out string jW){var hq=new List<ay>();jW=null;foreach(var k6 in aP(sC)){int jD=k6.LastIndexOf(':');
if(jD<0)throw new FormatException("color() entries need 'expr : color'");string g4=k6.Substring(0,jD).Trim();string nQ=k6.Substring(jD+1).Trim();
if(g4.Equals("default",StringComparison.OrdinalIgnoreCase)){hq.Add(new ay{c7=true,bS=nQ});}else{string qx=iE(g4);
if(jW==null)jW=g4.Substring(0,g4.Length-qx.Length).Trim();hq.Add(new ay{fO=qx,bS=nQ});}}if(!hq.Any(u7=>u7.c7))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return hq;}static void iB(string u8,out string sZ,out string tw){var e8=(u8 ?? "").Split('.');if(e8.Length!=2||e8[0].Trim().Length==0||e8[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
sZ=e8[0].Trim();tw=e8[1].Trim();}static string iE(string dr){dr=dr.Trim();foreach(var uy in new[]{">=","<=","==",">","<"}){int kk=dr.IndexOf(uy,StringComparison.Ordinal);
if(kk>0)return dr.Substring(kk).Trim();if(kk==0)return dr;}throw new FormatException($"color() rule has no recognizable comparison operator: '{dr}'");}
static string ae(string cS){if(cS.Length>=2&&cS[0]=='"'&&cS[cS.Length-1]=='"')return cS.Substring(1,cS.Length-2);return cS;}}public static class rh{public static double rg(aL sy,List<double>dS){
if(dS==null||dS.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(sy){case aL.hW: return dS.Average();case aL.mR: return dS.Sum();
case aL.mD: return dS.Min();case aL.mC: return dS.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class z{
public static string r8(string ua,double hd,int nk=10,string q1=null){hd=Math.Max(0,Math.Min(100,hd));int ol=(int)Math.Round(nk*(hd/100.0));
string sU=new string('#',ol)+new string('-',nk-ol);string v_=q1!=null ? q1 : $"{hd:0}%";return $"{ua} [{sU}] {v_}";
}public static string mr(double cs,string lY=null,string gL=null){if(lY=="auto"){double i_=Math.Abs(cs);if(i_>=1e9)return(cs/1e9).ToString("F1")+"G";
if(i_>=1e6)return(cs/1e6).ToString("F1")+"M";if(i_>=1e3)return(cs/1e3).ToString("F1")+"k";string tu= !string.IsNullOrEmpty(gL)? gL : "F0";return cs.ToString(tu);
}string oo= !string.IsNullOrEmpty(gL)? gL :(Math.Abs(cs)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(lY)){return cs.ToString(oo);}return cs.ToString(oo)+lY;
}public static string mF(string uW){string uC=(uW ?? "").Trim().ToLowerInvariant().Replace("_","").Replace("-","").Replace(" ","");switch(uC){
case "auto": return "auto";case "standard": case "textpanel": return "standard";case "compact": case "cockpitsmall": return "compact";case "wide": case "widelcd": return "wide";
case "corner": case "cornerlcd": case "cockpitcorner": return "corner";default: return null;}}public static string mM(string uX,float lM,float qy){
string pz=mF(uX)?? "standard";if(pz!="auto")return pz;if(lM<=0f||qy<=0f)return "standard";float ql=Math.Min(lM,qy);
if(ql<=160f)return "corner";if(ql<=300f)return "compact";if(lM>=1000f)return "wide";return "standard";}public static float rI(string uY,float vL,float vJ,float gC=1.0f){
string k_=mM(uY,vL,vJ);if(k_=="compact")return gC*0.75f;if(k_=="corner")return gC*0.55f;if(k_=="wide")return gC*1.15f;
return gC;}public static float ms(float pG,float oq){if(pG<=0f||oq<=0f)return 0f;return Math.Min(20f*oq,pG*0.18f);
}public static float rH(float uH,float os){float tR=ms(uH,os);return Math.Min(os*0.9f,tR*0.045f);
}public static string r6(string ub,double he,int cd=5){he=Math.Max(0,Math.Min(100,he));double gD=(he-50.0)/50.0;int e2=0;
int fk=0;if(gD< -0.01)e2=(int)Math.Round(-gD*cd);else if(gD>0.01)fk=(int)Math.Round(gD*cd);if(e2>cd)e2=cd;
if(fk>cd)fk=cd;string ue=new string('-',cd-e2)+new string('<',e2);string ve=new string('>',fk)+new string('-',cd-fk);
return $"{ub} [{ue}|{ve}] {he:0}%";}public static string si(string o5,double v1){return v1>=50.0 ? $"{o5} [====O] ON" : $"{o5} [O====] OFF";
}public static string sb(string o6,double v2){return v2>=50.0 ? $"{o6} [X] ACTIVE" : $"{o6} [ ] READY ";}public static string sh(string kC,double q_){
if(q_>=75.0)return $"{kC} [ ONLINE  ]";if(q_>=25.0)return $"{kC} [ STANDBY ]";return $"{kC} [ OFFLINE ]";}public static string r5(string uc,double hf,int pd=8){
hf=Math.Max(0,Math.Min(100,hf));int om=(int)Math.Round(pd*(hf/100.0));string uQ=new string('|',om)+new string('.',pd-om);
return $"{uc} [{uQ}] {hf:0}%";}public static string sc(string kD,double fc,int qK=0){char[]vz=new[]{'|','/','-','\\'};char p8=vz[(qK>=0 ? qK : 0)%4];
if(fc>55.0)return $"{kD} {p8} ▲ CHG {fc:0}%";if(fc<45.0)return $"{kD} {p8} ▼ DSC {fc:0}%";return $"{kD} ● STBY {fc:0}%";}public static bool rQ(string aB,string eZ){
if(aB==null||eZ==null)return false;if(aB=="*"||aB==eZ)return true;if(!aB.Contains("*")&& !aB.Contains("?"))return string.Equals(aB,eZ,StringComparison.OrdinalIgnoreCase);
int aI=0,eV=0,lC= -1,kJ=0;while(eV<eZ.Length){if(aI<aB.Length&&(aB[aI]=='?'||char.ToLowerInvariant(aB[aI])==char.ToLowerInvariant(eZ[eV]))){
aI++;eV++;}else if(aI<aB.Length&&aB[aI]=='*'){lC=aI;kJ=eV;aI++;}else if(lC!= -1){aI=lC+1;kJ++;eV=kJ;}else{return false;}}while(aI<aB.Length&&aB[aI]=='*')aI++;
return aI==aB.Length;}public static int rl(double uD,int qP){return Math.Max(0,Math.Min(qP,1+(int)(Math.Max(0.0,uD)*qP)));}public static string fR(string[]dP,double k7){
if(dP==null||dP.Length==0)return "white";k7=Math.Max(0,Math.Min(100,k7));int kl=(int)Math.Floor(k7/100.0*dP.Length);if(kl>=dP.Length)kl=dP.Length-1;
return dP[kl];}public static p mu(string[]cq,double uM){if(cq==null||cq.Length==0)return p.f5;if(cq.Length==1)return p.eg(cq[0]);
double vb=Math.Max(0.0,Math.Min(1.0,uM/100.0));double qc=vb*(cq.Length-1);int km=(int)Math.Floor(qc);int tT=Math.Min(cq.Length-1,km+1);
double lG=qc-km;p dl=p.eg(cq[km]);p jv=p.eg(cq[tT]);byte u9=(byte)Math.Round(dl.bY+(jv.bY-dl.bY)*lG);
byte tL=(byte)Math.Round(dl.bT+(jv.bT-dl.bT)*lG);byte sS=(byte)Math.Round(dl.bO+(jv.bO-dl.bO)*lG);return new p(u9,tL,sS);}public static string db(List<ay>p4,double v3){
foreach(var lj in p4){if(lj.c7)continue;if(rD(lj.fO,v3))return lj.bS;}var nW=p4.FirstOrDefault(va=>va.c7);return nW!=null ? nW.bS : null;
}static bool rD(string eL,double fw){eL=eL.Trim();foreach(var kS in new[]{">=","<=","==",">","<"}){if(eL.StartsWith(kS)){double fj=double.Parse(eL.Substring(kS.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(kS){case ">=": return fw>=fj;case "<=": return fw<=fj;case "==": return Math.Abs(fw-fj)<1e-9;case ">": return fw>fj;case "<": return fw<fj;
}}}throw new FormatException($"unsupported comparison expression: '{eL}'");}public static List<string>sa(List<string>td,int dT){
var g8=new List<string>();g8.Add("+"+new string('-',dT+2)+"+");foreach(var kG in td){string vV=kG.Length>dT ? kG.Substring(0,dT): kG;
string uG=vV.PadRight(dT);g8.Add("| "+uG+" |");}g8.Add("+"+new string('-',dT+2)+"+");return g8;}public struct mH{public float mY,mZ,mX,mw;
}public static mH rr(int nM,int p0,int hz,int hA,int fs,int ft,float vK,float vI,float pD,float pE,float j7,float j8){
fs=Math.Max(1,fs);ft=Math.Max(1,ft);hz=Math.Max(1,Math.Min(fs-nM,hz));hA=Math.Max(1,Math.Min(ft-p0,hA));
float sL=vK-(pD*2f);float sK=vI-(pE*2f);float nH=(sL-(j7*(fs-1)))/fs;float nG=(sK-(j8*(ft-1)))/ft;
float u_=pD+nM*(nH+j7);float u0=pE+p0*(nG+j8);float uZ=hz*nH+((hz-1)*j7);float uO=hA*nG+((hA-1)*j8);return new mH{
mY=u_,mZ=u0,mX=uZ,mw=uO};}public struct mN{public int rT;public int ii;public int ah;public int aE;public int el;
public int bd;public int sq;public int sr;public int rA;public int rB;
public double rC;}public static mN rs(F co){var m=new mN();var pH=co.ak!=null&&co.ak.Count>0 ? co.ak : new List<aF>{
new aF{aq=co.fX}};m.rT=pH.Count;m.ii=0;m.ah=0;m.aE=0;if(!string.IsNullOrEmpty(co.bP)&& !co.bP.Equals("none",StringComparison.OrdinalIgnoreCase))m.ah++;
if(!string.IsNullOrEmpty(co.c4)&& !co.c4.Equals("none",StringComparison.OrdinalIgnoreCase))m.ah+=2;foreach(var e9 in pH){
if(!string.IsNullOrEmpty(e9.c2)&& !e9.c2.Equals("none",StringComparison.OrdinalIgnoreCase))m.ah++;if(!string.IsNullOrEmpty(e9.cE))m.ah+=2;
m.ii+=e9.aq.Count;foreach(var t9 in e9.aq){m.sq++;m.sr++;m.ah+=2;switch(t9.i){case e.dX: m.el++;
m.bd++;m.ah+=2;m.aE++;break;case e.ee: m.bd+=10;m.ah+=10;
m.aE+=10;break;case e.ea: m.bd+=10;m.ah+=11;m.aE+=10;break;case e.d1: m.bd+=10;
m.ah+=10;m.aE+=10;break;case e.d4: m.bd+=12;m.ah+=12;m.aE+=12;
break;case e.cB: m.el++;m.bd+=8;m.ah+=9;m.aE+=8;break;case e.c3: m.el+=2;
m.bd++;m.ah+=3;m.aE++;break;case e.cF: case e.cx: m.el++;
m.bd++;m.ah+=2;m.aE++;break;case e.cD: case e.cy: m.el++;m.bd+=2;
m.ah+=3;m.aE+=2;break;case e.c1: m.bd+=8;m.ah+=8;m.aE+=8;
break;default: m.aE++;break;}}}m.rA=25+(m.ii*2);m.rB=180+(m.aE*4);
m.rC=0.0;return m;}}