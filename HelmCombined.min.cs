io t;string iH=null;Dictionary<string,List<IMyTerminalBlock>>b_=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>f2=new Dictionary<string,bool>();
Dictionary<string,bt>en=new Dictionary<string,bt>();Dictionary<string,Color>mJ=new Dictionary<string,Color>();List<IMyTerminalBlock>cG=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>dd=new List<IMyTerminalBlock>();bool f_;int iE;string eo="";StringBuilder er=new StringBuilder();readonly Dictionary<string,double>aO=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>bB=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener ep;bool cH=false;
Dictionary<E,IMyTextSurface>b0=new Dictionary<E,IMyTextSurface>();Dictionary<E,string>cI=new Dictionary<E,string>();
static readonly string[]mo=new string[101];static Program(){for(int gK=0;gK<=100;gK++){mo[gK]=gK+"%";}}static string mc(double vp){int eT=(int)Math.Round(vp);
if(eT<0)eT=0;else if(eT>100)eT=100;return mo[eT];}class ay{public List<MySprite>f=new List<MySprite>();public List<cy>eg=new List<cy>();
public int cs= -1;public Color[]fy;public Color fu;public bool fv;public List<im>bV=new List<im>();public Vector2 eb,ii;
public int el;public bool d9=false;public List<fz>dX=new List<fz>();}class im{public string mn;public int it;public Vector2 ms,mw;
}class fz{public int fN;public Color hS,fF;public List<a0>fW,fK;public string hW,h3,h4,h6;public bool mi,h8;
}class cy{public bw fQ;public int aa= -1;public int a3= -1;public int c9= -1;public bool me=false;
public List<int>D=new List<int>();public float[]cC=new float[8];public float ba;public float aC;public float a9;
public float bs;public double fP= -99999.0;public string mk;public float C= -1f;public bool mE;public float l3=12f;}Dictionary<E,ay>b1=new Dictionary<E,ay>();
int mL=0;double iL=0;double es=0;double P=0;int eq=0;Dictionary<E,double>bA=new Dictionary<E,double>();
Dictionary<E,double>iD=new Dictionary<E,double>();Dictionary<string,int>f6=new Dictionary<string,int>();List<string>de=new List<string>();
Dictionary<string,int>f7=new Dictionary<string,int>();double iN,iK;HashSet<string>f4=new HashSet<string>();Dictionary<string,double[]>iJ=new Dictionary<string,double[]>();
Dictionary<string,double>eu=new Dictionary<string,double>();List<MyInventoryItem>iG=new List<MyInventoryItem>();int b2= -1,f5;double iM;
Dictionary<E,double>f0=new Dictionary<E,double>();int et=0;const string ri="0.11.0";IMyTextSurface a4;bool iC=true;
double f1=0;string mK="";string iI="";int f3=0,iF=0;List<KeyValuePair<string,int>>dc=new List<KeyValuePair<string,int>>();
const double rA=120;const float qO=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{ep=IGC.RegisterBroadcastListener("HELM");
ep.SetMessageCallback("HELM_IGC");}catch{}fV();iu();mh();}public void Main(string mY,UpdateType vl){
double nQ=Runtime.TimeSinceLastRun.TotalSeconds;P+=nQ;if((vl&UpdateType.IGC)!=0||mY=="HELM_IGC"){ry();}string px=(mY ?? "").Trim();
string aq=px.ToLowerInvariant();if(aq=="reload"){fV();}else if(aq=="refresh"){iq();}else if(aq=="boot"){iu();}else if(aq=="probe"){
es=P+20.0;q7();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(aq=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=rX;
fV();iu();}else if(aq=="console on"||aq=="console off"){iC=aq=="console on";mh();f1=0;}else if(aq=="scan"||aq=="diagnose"||aq=="test"){
rT();return;}else if(aq.StartsWith("set ")){string hh=px.Substring(4).Trim();int nU=hh.IndexOf('=');int uW=hh.IndexOf(' ');int lj=nU>0 ? nU : uW;
if(lj>0){string oB=hh.Substring(0,lj).Trim();string qt=hh.Substring(lj+1).Trim();lX($"{oB}={qt}");Echo($"[HELM] Set dynamic variable '{oB}' = {qt}");
}return;}else if(aq.StartsWith("alert")){string fo=aq.Length>5 ? aq.Substring(5).Trim(): "toggle";if(fo=="toggle")cH= !cH;else cH=fo=="red"||fo=="on"||fo=="1"||fo=="true";
Echo($"[HELM] Red alert state: {cH}");return;}else if(Me.CustomData!=iH){fV();}et++;iL+=nQ;if(iL>=rA){
iL=0;iq();}if(es>0){if(P<es){iB();return;}es=0;b1.Clear();cI.Clear();
f1=0;}if(t==null||t.Z.Count==0){iB();l4();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cU=t.Z[mL%t.Z.Count];
mL++;iI=cU.bf;bool iR=bA.Count>0;if(iR)rH();double nR;if(!bA.ContainsKey(cU)&& !(iD.TryGetValue(cU,out nR)&&P<nR)){
if(rM(cU))iR=true;if(cU.ip>0)iD[cU]=P+cU.ip/60.0;}eq=iR ? 0 : eq+1;Runtime.UpdateFrequency=eq<=t.Z.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
rW();rV();rU();iB();l4();f3=Runtime.CurrentInstructionCount;if(f3>iF)iF=f3;}void l4(){
if(eo.Length>0)Echo(eo);if(er.Length>0)Echo(er.ToString());}List<IMyTerminalBlock>fw(bool r6=false){if(!f_){dd.Clear();
GridTerminalSystem.GetBlocks(dd);cG.Clear();cG.AddRange(dd);cG.RemoveAll(sj=> !sj.IsSameConstructAs(Me));iE=dd.Count-cG.Count;
f_=true;}return r6 ? dd : cG;}IMyTerminalBlock l7(string o1,bool r8=false){var i0=GridTerminalSystem.GetBlockWithName(o1);
if(i0==null||r8||i0.IsSameConstructAs(Me))return i0;var iT=fw();for(int gL=0;gL<iT.Count;gL++)if(iT[gL].CustomName.IndexOf(o1,StringComparison.OrdinalIgnoreCase)>=0)return iT[gL];
return null;}void qZ(string o2,List<IMyTerminalBlock>eV,bool mR=false){var ob=GridTerminalSystem.GetBlockGroupWithName(o2);if(ob==null)return;int m8=eV.Count;
var fl=new List<IMyTerminalBlock>();ob.GetBlocks(fl);lV(fl,eV,mR);if(eV.Count>m8)return;var om=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(om);
foreach(var qI in om){if(qI.Name.IndexOf(o2,StringComparison.OrdinalIgnoreCase)<0)continue;fl.Clear();qI.GetBlocks(fl);lV(fl,eV,mR);if(eV.Count>m8)return;
}}void lV(List<IMyTerminalBlock>td,List<IMyTerminalBlock>oz,bool r7=false){foreach(var i1 in td)if((r7||i1.IsSameConstructAs(Me))&& !oz.Contains(i1))oz.Add(i1);
}void fV(){iH=Me.CustomData;t=mg.rr(iH);eo="";if(t.bd.Count>0){var pJ=new StringBuilder("HELM config errors:\n");
foreach(var sX in t.bd)pJ.AppendLine(" - "+sX);eo=pJ.ToString();}if(t.em.Count>0){var qG=new StringBuilder("HELM notes:\n");foreach(var vu in t.em)qG.AppendLine(" - "+vu);
eo+=qG.ToString();}en.Clear();foreach(var i2 in t.ct)if(!en.ContainsKey(i2.af))en[i2.af]=i2;b1.Clear();cI.Clear();
bA.Clear();iD.Clear();f0.Clear();rd();iq();}void ry(){try{while(ep!=null&&ep.HasPendingMessage){
var oX=ep.AcceptMessage();if(oX.Data is string){lX((string)oX.Data);}}}catch{}}void lX(string pl){
if(string.IsNullOrEmpty(pl))return;var pi=pl.Split(';');for(int j0=0;j0<pi.Length;j0++){var e5=pi[j0].Trim();if(e5.Length==0)continue;int eG=e5.IndexOf('=');
if(eG<0)eG=e5.IndexOf(':');if(eG>0){string dw=e5.Substring(0,eG).Trim();string bq=e5.Substring(eG+1).Trim();if(dw.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cH=bq.Equals("red",StringComparison.OrdinalIgnoreCase)||bq=="1"||bq.Equals("true",StringComparison.OrdinalIgnoreCase)||bq.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double o5;bB.Remove(dw);if(double.TryParse(bq,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out o5)){
aO[dw]=o5;}else if(bq.Equals("true",StringComparison.OrdinalIgnoreCase)||bq.Equals("on",StringComparison.OrdinalIgnoreCase)){aO[dw]=100.0;
}else if(bq.Equals("false",StringComparison.OrdinalIgnoreCase)||bq.Equals("off",StringComparison.OrdinalIgnoreCase)){aO[dw]=0.0;}else{bB[dw]=bq;
}}}}}const string rX="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void q7(){var iU=new List<IMyTextSurface>();if(a4!=null)iU.Add(a4);foreach(var t0 in b0.Values)iU.Add(t0);foreach(var uR in iU)q6(uR);
}static void bx(List<MySprite>uX,float sQ,float sR,float vv,float tk,Color sD){uX.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(sQ,sR),Size=new Vector2(vv,tk),Color=sD}
);}void q6(IMyTextSurface lk){Vector2 aW=lk.SurfaceSize,aI=lk.TextureSize,ao=(aI-aW)/2f;var aH=new List<MySprite>();Color gH=new Color(150,150,150);
bx(aH,aI.X/2f,2f,aI.X,4f,gH);bx(aH,aI.X/2f,aI.Y-2f,aI.X,4f,gH);bx(aH,2f,aI.Y/2f,4f,aI.Y,gH);bx(aH,aI.X-2f,aI.Y/2f,4f,aI.Y,gH);
bx(aH,ao.X+aW.X/2f,ao.Y+aW.Y/2f,aW.X,aW.Y,new Color(20,40,90));float an=32f;bx(aH,ao.X+an/2f,ao.Y+an/2f,an,an,new Color(255,0,0));bx(aH,ao.X+aW.X-an/2f,ao.Y+an/2f,an,an,new Color(0,255,0));
bx(aH,ao.X+an/2f,ao.Y+aW.Y-an/2f,an,an,new Color(255,255,0));bx(aH,ao.X+aW.X-an/2f,ao.Y+aW.Y-an/2f,an,an,new Color(255,0,255));float lT=ao.Y+aW.Y/2f-30f;
a8(aH,"SURFACE "+aW.X.ToString("0")+"x"+aW.Y.ToString("0"),Color.White,ao.X+44f,lT,0.9f);a8(aH,"TEXTURE "+aI.X.ToString("0")+"x"+aI.Y.ToString("0"),Color.White,ao.X+44f,lT+24f,0.9f);
a8(aH,"OFFSET  "+ao.X.ToString("0")+","+ao.Y.ToString("0"),Color.White,ao.X+44f,lT+48f,0.9f);using(var s6=lk.DrawFrame()){s6.AddRange(aH);}}static bool mj(string bD){
return bD=="rate"||bD=="eta"||bD=="trend"||bD.Length>3&&(bD.StartsWith("top")&&char.IsDigit(bD[3])||bD.Length>4&&bD.StartsWith("line")&&char.IsDigit(bD[4]));}void rd(){
f6.Clear();de.Clear();f7.Clear();f4.Clear();iJ.Clear();b2= -1;iM=iN=iK=0;foreach(var lf in t.Z){var kw=new List<List<bw>>();
foreach(var ul in lf.ak)kw.Add(ul.ap);if(lf.ak.Count==0)kw.Add(lf.fR);foreach(var tV in kw)foreach(var bF in tV){if(!mj(bF.I))continue;
if(bF.I=="rate"||bF.I=="eta"||bF.I=="trend"){f4.Add(bF.aK);continue;}bool qn=bF.I[0]=='t';var nJ=qn ? f6 : f7;int tX=int.Parse(bF.I.Substring(qn ? 3 : 4));
int os;nJ.TryGetValue(bF.aK,out os);nJ[bF.aK]=Math.Max(os,tX);}}de.AddRange(f6.Keys);}void rV(){if(f7.Count==0||P<iN)return;
iN=P+2.0;foreach(var kn in f7){List<IMyTerminalBlock>i9;var pS=b_.TryGetValue(kn.Key,out i9)&&i9.Count>0 ? i9[0]as IMyTextSurface : null;
string[]oT=pS==null ? new string[0]: pS.GetText().Split('\n');for(int e9=1;e9<=kn.Value;e9++)bB[kn.Key+".line"+e9]=e9<=oT.Length ? oT[e9-1].TrimEnd('\r'): "";}}
void rU(){if(f4.Count==0||P<iK)return;iK=P+5.0;foreach(var di in f4){double hM=bg(di,"percent"),ci=0.0;double[]gI;
if(iJ.TryGetValue(di,out gI))ci=gI[2]*0.6+(hM-gI[0])/Math.Max(0.1,P-gI[1])*0.4;iJ[di]=new[]{hM,P,ci};double e7=ci*60.0;int cZ=(int)Math.Min(359999.0,(ci<0 ? hM/ -ci : ci>0 ?(100.0-hM)/ci : 0.0)+0.5);
bB[di+".rate"]=(e7>=0 ? "+" : "")+e7.ToString("0.0")+"%/min";bB[di+".trend"]=e7>0.05 ? "^" : e7< -0.05 ? "v" : "=";bB[di+".eta"]=Math.Abs(e7)<0.05 ? "--" :(ci<0 ? "empty " : "full ")+(cZ>=3600 ? cZ/3600+"h "+cZ%3600/60+"m" : cZ>=60 ? cZ/60+"m "+cZ%60+"s" : cZ+"s");
}}void rW(){if(de.Count==0)return;if(b2<0){if(P<iM)return;b2=0;f5=0;eu.Clear();}while(b2<de.Count&&Runtime.CurrentInstructionCount<20000){
string jc=de[b2];List<IMyTerminalBlock>gh;b_.TryGetValue(jc,out gh);if(gh!=null&&f5<gh.Count){var m_=gh[f5++];
for(int j1=0;j1<m_.InventoryCount;j1++){iG.Clear();m_.GetInventory(j1).GetItems(iG);foreach(var gS in iG){string kj=gS.Type.TypeId.Replace("MyObjectBuilder_","");
string oC=kj=="Ore"||kj=="Ingot" ? gS.Type.SubtypeId+" "+kj.ToLowerInvariant(): gS.Type.SubtypeId;double ot;eu.TryGetValue(oC,out ot);eu[oC]=ot+(double)gS.Amount;
}}continue;}var hf=new List<KeyValuePair<string,double>>(eu);hf.Sort((vx,vD)=>vD.Value.CompareTo(vx.Value));for(int dE=1;dE<=f6[jc];dE++)bB[jc+".top"+dE]=dE<=hf.Count ? hf[dE-1].Key+" "+M.mb(hf[dE-1].Value,"auto"): "-";
eu.Clear();b2++;f5=0;}if(b2>=de.Count){b2= -1;iM=P+15.0;}}void aj(int u5,string vd){dc.Add(new KeyValuePair<string,int>(vd,u5));
}static string iA(string k8,int o0){return k8.Length>o0 ? k8.Substring(0,o0): k8;}void mh(){var k1=Me as IMyTextSurfaceProvider;if(k1==null||k1.SurfaceCount<1)return;
a4=k1.GetSurface(0);a4.ContentType=iC ? ContentType.SCRIPT : ContentType.NONE;a4.Script="";a4.ScriptBackgroundColor=Color.Black;
}void iB(){if(P>=f1){f1=P+(eq>t.Z.Count+1 ? 5.0 : 1.0);qW();if(iC&&a4!=null&&es==0)q5();
}Echo(mK);}void qW(){dc.Clear();int jf=0,nf=0;foreach(var m0 in t.ct){List<IMyTerminalBlock>kq;if(m0.bT==bb.d3){
jf++;continue;}if(b_.TryGetValue(m0.af,out kq)){nf+=kq.Count;if(kq.Count>0)jf++;}}int nV=t.bd.Count;aj(0,"HELM-[Kernel-(v"+ri+")]");
aj(4,"------------------------------");aj(1,"krn-cfg");aj(2," screens "+t.Z.Count+"  binds "+jf+"/"+t.ct.Count+"  errors "+nV);
aj(2," alert "+(cH ? "RED" : "off")+"  blocks "+nf+"  docked-ignored "+iE);if(nV>0)aj(3," ! "+iA(t.bd[0],40));
else if(t.em.Count>0)aj(3," ? "+iA(t.em[0],40));aj(1,"cfg-dsp");if(t.Z.Count==0)aj(3," no @screen: run 'starter'");
int uT=0;foreach(var k9 in t.Z){if(++uT>10){aj(2," +"+(t.Z.Count-10)+" more");break;}IMyTextSurface ll;string pV="--",lq="no block";
if(b0.TryGetValue(k9,out ll)){pV=ll.SurfaceSize.X.ToString("0")+"x"+ll.SurfaceSize.Y.ToString("0");lq=bA.ContainsKey(k9)? "boot" : "live";
}aj(lq=="no block" ? 3 : 2," "+iA(k9.bf,14).PadRight(14)+" "+pV.PadRight(9)+" "+lq);}aj(1,"perf");aj(2," tick "+et+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
aj(2," instr "+f3+"  peak "+iF+"  max "+Runtime.MaxInstructionCount);aj(2," rate "+Runtime.UpdateFrequency+"  idle "+eq+"  up "+(int)P+"s");
if(iI.Length>0)aj(2," last "+iI);var pK=new StringBuilder();foreach(var tA in dc)pK.AppendLine(tA.Key);mK=pK.ToString();}static Color q1(int u6){
switch(u6){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void q5(){Vector2 cV=a4.SurfaceSize;Vector2 kE=(a4.TextureSize-cV)/2f;int jo=30;foreach(var tB in dc)jo=Math.Max(jo,tB.Key.Length);
Vector2 jn=a4.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float pL=Math.Min(1f,Math.Min(cV.X*0.94f/(jo*jn.X),cV.Y*0.94f/(dc.Count*jn.Y)));
float vy=kE.X+cV.X*0.03f;float qL=kE.Y+cV.Y*0.03f;var lm=new List<MySprite>();lm.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kE+cV/2f,Size=cV,Color=new Color(12,12,16)}
);foreach(var oH in dc){a8(lm,oH.Key,q1(oH.Value),vy,qL,pL);qL+=jn.Y*pL;}using(var s7=a4.DrawFrame()){s7.AddRange(lm);}}static readonly List<string>qV=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void iu(){bA.Clear();if(t==null)return;for(int gM=0;gM<t.Z.Count;gM++){var hm=t.Z[gM];if(hm.hZ>0f)bA[hm]=P+(gM/Math.Max(1,hm.lZ))*hm.l1/60.0;
}}void rH(){List<E>eF=null;int tj=(bA.Count+3)/4,tt=0;foreach(var gW in bA){double kM=(P-gW.Value)/gW.Key.hZ;if(kM<0)continue;
if(kM>=1.0){if(eF==null)eF=new List<E>();eF.Add(gW.Key);continue;}if((tt++ +et)%tj==0)q4(gW.Key,kM);}if(eF==null)return;foreach(var fg in eF){
bA.Remove(fg);if(fg.hY>0f)f0[fg]=P;b1.Remove(fg);cI.Remove(fg);}}static void a8(List<MySprite>uY,string ve,Color sG,float vz,float vE,float uO){
var qa=MySprite.CreateText(ve,"Monospace",sG,uO,TextAlignment.LEFT);qa.Position=new Vector2(vz,vE);uY.Add(qa);}static void lU(List<MySprite>uZ,List<string>gc,E hp,float vA,float vF,float uP,float tM){
string[]u3=hp.fA!=null&&hp.fA.Length>0 ? hp.fA : new[]{hp.l_};for(int eO=0;eO<gc.Count;
eO++){p jh=M.md(u3,gc.Count>1 ? 100.0*eO/(gc.Count-1): 0.0);a8(uZ,gc[eO],new Color(jh.bW,jh.bR,jh.bN),vA,vF+eO*tM,uP);}}bool rL(E dI,IMyTextSurface lu){
ay aB,aR;E pO=t.Z.Find(qJ=>qJ!=dI&&qJ.bf.Equals(dI.id,StringComparison.OrdinalIgnoreCase));if(pO==null|| !b1.TryGetValue(pO,out aB)|| !aB.d9)return false;
if(!b1.TryGetValue(dI,out aR))b1[dI]=aR=new ay();Vector2 dL=lu.SurfaceSize;if(aR.d9&&aR.el==aB.el&&aR.eb==dL)return false;
aR.d9=true;aR.el=aB.el;aR.eb=dL;aR.f.Clear();Vector2 hy=aB.eb,o9=aB.ii,o7=(lu.TextureSize-dL)/2f;aR.ii=o7;
int n8=0,qm=aB.f.Count;if(dI.ie!=null){int dD=aB.bV.FindIndex(vB=>vB.mn.Equals(dI.ie,StringComparison.OrdinalIgnoreCase));
if(dD<0)return false;hy=aB.bV[dD].mw;o9=aB.bV[dD].ms;n8=aB.bV[dD].it;if(dD+1<aB.bV.Count)qm=aB.bV[dD+1].it;}float gU=Math.Min(dL.X/hy.X,dL.Y/hy.Y);
Vector2 sg=o7+(dL-hy*gU)/2f;for(int j2=n8;j2<qm;j2++){var bG=aB.f[j2];if(bG.Position.HasValue)bG.Position=(bG.Position.Value-o9)*gU+sg;if(bG.Size.HasValue)bG.Size=bG.Size.Value*gU;
if(bG.Type==SpriteType.TEXT)bG.RotationOrScale*=gU;aR.f.Add(bG);}using(var s8=lu.DrawFrame())s8.AddRange(aR.f);return true;}bool rF(E dJ,IMyTextSurface hw){
List<string>ew;if(cI.ContainsKey(dJ)|| !t.dU.TryGetValue(dJ.cr,out ew))return false;cI[dJ]="banner";Vector2 cW=hw.SurfaceSize;
Vector2 kF=(hw.TextureSize-cW)/2f;int gn=1;foreach(var tC in ew)gn=Math.Max(gn,tC.Length);Vector2 eC=hw.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float ho=Math.Min(cW.X*0.94f/(gn*eC.X),cW.Y*0.94f/(ew.Count*eC.Y));var ln=new List<MySprite>();ln.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kF+cW/2f,Size=cW,Color=string.IsNullOrEmpty(dJ.bO)? new Color(10,12,16): J(dJ.bO)}
);lU(ln,ew,dJ,kF.X+(cW.X-gn*eC.X*ho)/2f,kF.Y+(cW.Y-ew.Count*eC.Y*ho)/2f,ho,eC.Y*ho);using(var s9=hw.DrawFrame()){s9.AddRange(ln);
}return true;}void q4(E dK,double pc){IMyTextSurface fi;if(!b0.TryGetValue(dK,out fi))return;Vector2 cn=fi.SurfaceSize;
Vector2 kG=(fi.TextureSize-cn)/2f;var eZ=dK.re();string oY=dK.l0 ?? "HELM TELEMETRY KERNEL ONLINE";bool kx=cn.Y>=200f;
List<string>df;if(dK.cu==null|| !t.dU.TryGetValue(dK.cu,out df))df=qV;int dn=oY.Length+3;foreach(var tD in eZ)dn=Math.Max(dn,tD.Length+5);
if(kx)foreach(var tE in df)dn=Math.Max(dn,tE.Length);int pC=(kx ? df.Count+1 : 0)+2+eZ.Count;Vector2 dm=fi.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float cj=Math.Min(1f,Math.Min(cn.X*0.94f/(dn*dm.X),cn.Y*0.94f/(pC*dm.Y)));float hO=kG.X+(cn.X-dn*dm.X*cj)/2f;float c1=kG.Y+(cn.Y-pC*dm.Y*cj)/2f;
float g_=dm.Y*cj;var dM=new List<MySprite>();dM.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=kG+cn/2f,Size=cn,Color=new Color(10,12,16)}
);if(kx){lU(dM,df,dK,hO,c1,cj,g_);c1+=(df.Count+1)*g_;}a8(dM,">> "+oY,new Color(34,197,94),hO,c1,cj);c1+=g_*2f;int uU=M.qU(pc,eZ.Count);
for(int gN=0;gN<uU;gN++){bool o8=gN==eZ.Count-1&&pc>=0.8;a8(dM,o8 ? "[OK]" : "[..]",o8 ? new Color(34,197,94): new Color(56,189,248),hO,c1,cj);a8(dM,eZ[gN],new Color(203,213,225),hO+5f*dm.X*cj,c1,cj);
c1+=g_;}using(var ta=fi.DrawFrame()){ta.AddRange(dM);}}void iq(){b_.Clear();b0.Clear();f2.Clear();
er.Clear();f_=false;if(t==null)return;foreach(var ae in t.ct){var a5=new List<IMyTerminalBlock>();if(ae.bT==bb.d3){
string kh= !string.IsNullOrEmpty(ae.bv)? ae.bv : ae.af;if(!aO.ContainsKey(kh)){aO[kh]=ae.fG;if(ae.h0!=null)bB[kh]=ae.h0;
}b_[ae.af]=a5;continue;}var qe=ae.a2.Count>0 ? ae.a2 : new List<string>{ae.mB};foreach(var c_ in qe){
if(string.IsNullOrEmpty(c_))continue;bool oA=c_.Contains("*")||c_.Contains("?");if(ae.bT==bb.h9||oA){var jV=new List<IMyTerminalBlock>();
qZ(c_,jV,ae.fx);if(jV.Count>0){foreach(var oi in jV){if(!a5.Contains(oi))a5.Add(oi);}}else{var mT=fw(ae.fx);
for(int i3=0;i3<mT.Count;i3++){var hE=mT[i3];if(oA ? M.rn(c_,hE.CustomName): string.Equals(c_,hE.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!a5.Contains(hE))a5.Add(hE);}}}}else{var i8=l7(c_,ae.fx);if(i8!=null&& !a5.Contains(i8))a5.Add(i8);}}if(a5.Count==0){
rR(ae,qe,a5);if(a5.Count>0)f2[ae.af]=true;}b_[ae.af]=a5;}foreach(var bK in t.Z){
if(string.IsNullOrEmpty(bK.bU))continue;var kJ=l7(bK.bU);if(kJ==null){er.AppendLine($"HELM: output block '{bK.bU}' not found");
continue;}IMyTextSurface cY=null;var k2=kJ as IMyTextSurfaceProvider;var nK=kJ as IMyTextSurface;if(k2!=null&&bK.cz<k2.SurfaceCount){
cY=k2.GetSurface(bK.cz);}else if(nK!=null&&bK.cz==0){cY=nK;}if(cY==null){er.AppendLine($"HELM: output block '{bK.bU}' has no surface index {bK.cz}");
continue;}cY.ContentType=ContentType.SCRIPT;cY.Script="";cY.ScriptBackgroundColor=Color.Black;b0[bK]=cY;}cG.Clear();
dd.Clear();}static readonly string[]l6={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool rc(int tu,IMyTerminalBlock v){switch(tu){case 0: return v is IMyBatteryBlock;case 1: return v is IMyReactor;case 2: return v is IMySolarPanel;
case 3: return v is IMyWindTurbine;case 4: return v is IMyGasTank;case 5: return v is IMyAirVent;case 6: return v is IMyCargoContainer;case 7: return v is IMyJumpDrive;
case 8: return v is IMyThrust;case 9: return v is IMyMotorSuspension;case 10: return v is IMyDoor;case 11: return v is IMyShipConnector;case 12: return v is IMyAssembler;
case 13: return v is IMyRefinery;case 14: return v is IMyShipDrill;case 15: return v is IMyShipWelder;case 16: return v is IMyShipGrinder;case 17: return v is IMyGasGenerator;
case 18: return v is IMyLargeTurretBase;case 19: return v is IMyProjector;case 20: return v is IMyGyro;case 21: return v is IMyGravityGeneratorBase;case 22: return v is IMyLandingGear;
case 23: return v is IMyRadioAntenna;case 24: return v is IMyBeacon;case 25: return v is IMyLightingBlock;case 26: return v is IMyPistonBase;case 27: return v is IMyMotorStator;
case 28: return v is IMyWarhead;case 29: return v is IMySensorBlock;case 30: return v is IMyCameraBlock;case 31: return v is IMyParachute;case 32: return v is IMyTurretControlBlock;
}return false;}void rR(bt sv,List<string>vb,List<IMyTerminalBlock>ja){string to=(sv.af+" "+string.Join(" ",vb)).ToLowerInvariant();
var kl=new List<int>();for(int gV=0;gV<l6.Length;gV++){foreach(var vw in l6[gV].Split(','))if(to.Contains(vw)){kl.Add(gV);break;}}if(kl.Count==0)return;
var mS=fw();var pa=new List<IMyTerminalBlock>();for(int j3=0;j3<mS.Count;j3++){var i4=mS[j3];foreach(int tv in kl){if(rc(tv,i4)){(i4.CubeGrid==Me.CubeGrid ? ja : pa).Add(i4);
break;}}}if(ja.Count==0)ja.AddRange(pa);}void rT(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(t==null||t.Z.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {t.Z.Count} screens, {t.ct.Count} binds");f_=false;
Echo($"Blocks: {fw().Count} on this construct; {iE} on docked ships (ignored)");cG.Clear();Echo("--- BIND RESOLUTION ---");foreach(var at in t.ct){
if(at.bT==bb.d3){string ki= !string.IsNullOrEmpty(at.bv)? at.bv : at.af;double vm=aO.ContainsKey(ki)? aO[ki]: at.fG;
Echo($"[OK] @bind {at.af} (dynamic variable '{ki}' = {vm})");continue;}List<IMyTerminalBlock>jb;if(b_.TryGetValue(at.af,out jb)){if(jb.Count>0){
string s2=f2.ContainsKey(at.af)&&f2[at.af]? " [fallback: typed match]" : "";Echo($"[OK] @bind {at.af} ({jb.Count} blocks matched{s2})");
}else{string vc=string.Join(", ",at.a2.Count>0 ? at.a2 : new List<string>{at.mB});Echo($"[WARN] @bind {at.af} -> 0 blocks matched \"{vc}\"");
}}else{Echo($"[ERR] @bind {at.af} -> unresolved");}}if(aO.Count>0){Echo($"--- DYNAMIC VARIABLES ({aO.Count}) ---");foreach(var oJ in aO)Echo($"[VAR] {oJ.Key} = {oJ.Value}");
}IMyTextSurface iW=null;foreach(var t3 in b0.Values){iW=t3;break;}if(iW!=null){var lo=new List<string>();iW.GetSprites(lo);
Echo($"--- SPRITES ({lo.Count}): names usable in bg_texture ---");Echo(string.Join(", ",lo.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bL in t.Z){IMyTextSurface hx;if(b0.TryGetValue(bL,out hx)){var pW=hx.SurfaceSize;var u1=M.q0(bL);
Echo($"[OK] Screen \"{bL.bf}\" [{bL.cA}] -> {bL.bU} [#{bL.cz}] ({pW.X:0}x{pW.Y:0} px, texture {hx.TextureSize.X:0}x{hx.TextureSize.Y:0}, {u1.ag} sprites)");
}else{Echo($"[ERR] Screen \"{bL.bf}\" -> Missing block \"{bL.bU}\" [#{bL.cz}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double rS(IMyTerminalBlock w){
if(w==null)return 0.0;var gf=w as IMyBatteryBlock;if(gf!=null)return gf.MaxStoredPower>0 ?(gf.CurrentStoredPower/gf.MaxStoredPower)*100.0 : 0.0;
var gT=w as IMyJumpDrive;if(gT!=null)return gT.MaxStoredPower>0 ?(gT.CurrentStoredPower/gT.MaxStoredPower)*100.0 : 0.0;var qd=w as IMyGasTank;
if(qd!=null)return qd.FilledRatio*100.0;var qC=w as IMyAirVent;if(qC!=null)return qC.GetOxygenLevel()*100.0;var nn=w as IMyCargoContainer;if(nn!=null){
var gQ=nn.GetInventory();if(gQ!=null&&gQ.MaxVolume.RawValue>0)return(double)gQ.CurrentVolume.RawValue/gQ.MaxVolume.RawValue*100.0;return 0.0;}var hg=w as IMyReactor;
if(hg!=null)return hg.MaxOutput>0 ?(hg.CurrentOutput/hg.MaxOutput)*100.0 : 0.0;var hq=w as IMySolarPanel;if(hq!=null)return hq.MaxOutput>0 ?(hq.CurrentOutput/hq.MaxOutput)*100.0 : 0.0;
var hN=w as IMyWindTurbine;if(hN!=null)return hN.MaxOutput>0 ?(hN.CurrentOutput/hN.MaxOutput)*100.0 : 0.0;var nL=w as IMyDoor;if(nL!=null)return(double)nL.OpenRatio*100.0;
var hH=w as IMyThrust;if(hH!=null)return hH.MaxEffectiveThrust>0 ?(hH.CurrentThrust/hH.MaxEffectiveThrust)*100.0 : 0.0;var lz=w as IMyMotorSuspension;
if(lz!=null)return lz.Brake ? 100.0 :(lz.Friction*100.0);var js=w as IMyShipConnector;if(js!=null)return js.Status==MyShipConnectorStatus.Connected ? 100.0 :(js.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var pr=w as IMyProductionBlock;if(pr!=null)return pr.IsProducing ? 100.0 : 0.0;var nP=w as IMyShipDrill;if(nP!=null)return nP.Enabled ? 100.0 : 0.0;
var lH=w as IMyShipToolBase;if(lH!=null)return lH.IsActivated ? 100.0 :(lH.Enabled ? 50.0 : 0.0);var jS=w as IMyGasGenerator;if(jS!=null)return jS.AutoRefill ? 100.0 :(jS.Enabled ? 50.0 : 0.0);
var hI=w as IMyLargeTurretBase;if(hI!=null)return hI.IsShooting ? 100.0 :(hI.HasTarget ? 50.0 :(hI.Enabled ? 10.0 : 0.0));var e8=w as IMyProjector;
if(e8!=null)return e8.TotalBlocks>0 ?((double)(e8.TotalBlocks-e8.RemainingBlocks)/e8.TotalBlocks)*100.0 : 0.0;var jY=w as IMyGyro;if(jY!=null)return jY.GyroOverride ? 100.0 :(jY.Enabled ? 50.0 : 0.0);
var ok=w as IMyGravityGeneratorBase;if(ok!=null)return Math.Min(100.0,Math.Abs(ok.GravityAcceleration)/9.81*100.0);var jR=w as IMyLandingGear;if(jR!=null)return jR.IsLocked ? 100.0 :(jR.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var mU=w as IMyRadioAntenna;if(mU!=null)return(mU.Radius/50000.0)*100.0;var m6=w as IMyBeacon;if(m6!=null)return(m6.Radius/50000.0)*100.0;
var oR=w as IMyLightingBlock;if(oR!=null)return oR.Intensity*10.0;var hb=w as IMyPistonBase;if(hb!=null)return hb.HighestPosition>0 ?(hb.CurrentPosition/hb.HighestPosition)*100.0 : 0.0;
var pz=w as IMyMotorStator;if(pz!=null)return Math.Min(100.0,Math.Abs(pz.Angle)/6.28318*100.0);var qF=w as IMyWarhead;if(qF!=null)return qF.IsArmed ? 100.0 : 0.0;
var li=w as IMySensorBlock;if(li!=null)return!li.LastDetectedEntity.IsEmpty()? 100.0 :(li.Enabled ? 25.0 : 0.0);var jk=w as IMyCameraBlock;
if(jk!=null)return jk.IsActive ? 100.0 :(jk.Enabled ? 50.0 : 0.0);var nt=w as IMyParachute;if(nt!=null)return(double)nt.OpenRatio*100.0;
var hG=w as IMyTurretControlBlock;if(hG!=null)return hG.HasTarget ? 100.0 :(hG.IsUnderControl ? 50.0 :(hG.Enabled ? 10.0 : 0.0));var n9=w as IMyFunctionalBlock;
if(n9!=null)return n9.Enabled ? 100.0 : 0.0;return 0.0;}static double rk(IMyTerminalBlock m1,string jJ){int nm=jJ.IndexOf(':');string kk=jJ.Substring(0,nm).ToLowerInvariant();
var vj=new MyItemType("MyObjectBuilder_"+(kk=="ammo" ? "AmmoMagazine" : char.ToUpper(kk[0])+kk.Substring(1)),jJ.Substring(nm+1));double p6=0;for(int j4=0;
j4<m1.InventoryCount;j4++)p6+=(double)m1.GetInventory(j4).GetItemAmount(vj);return p6;}double mt(IMyTerminalBlock x,string n0){if(x==null)return 0.0;
string c=n0.ToLowerInvariant();if(c=="percent"||c=="pct")return rS(x);if(c.IndexOf(':')>0)return rk(x,n0);var b5=x as IMyBatteryBlock;
if(b5!=null){if(c=="current"||c=="stored"||c=="val"||c=="value"||c=="raw")return b5.CurrentStoredPower;if(c=="max"||c=="capacity")return b5.MaxStoredPower;
if(c=="output")return b5.CurrentOutput;if(c=="maxoutput")return b5.MaxOutput;if(c=="input")return b5.CurrentInput;if(c=="maxinput")return b5.MaxInput;
return b5.CurrentStoredPower;}var kg=x as IMyJumpDrive;if(kg!=null){if(c=="max"||c=="capacity")return kg.MaxStoredPower;return kg.CurrentStoredPower;
}var hB=x as IMyGasTank;if(hB!=null){if(c=="max"||c=="capacity")return hB.Capacity;return hB.FilledRatio*hB.Capacity;}var qD=x as IMyAirVent;
if(qD!=null){if(c=="max"||c=="capacity")return 100.0;return qD.GetOxygenLevel()*100.0;}var no=x as IMyCargoContainer;if(no!=null){var gR=no.GetInventory();
if(gR!=null){if(c=="mass")return(double)gR.CurrentMass;if(c=="max"||c=="capacity")return(double)gR.MaxVolume;return(double)gR.CurrentVolume;}return 0.0;}var k3=x as IMyPowerProducer;
if(k3!=null){if(c=="max"||c=="capacity")return k3.MaxOutput;return k3.CurrentOutput;}var nM=x as IMyDoor;if(nM!=null){if(c=="max")return 1.0;return(double)nM.OpenRatio*100.0;
}var lE=x as IMyThrust;if(lE!=null){if(c=="max"||c=="capacity")return lE.MaxEffectiveThrust;return lE.CurrentThrust;}var cp=x as IMyMotorSuspension;
if(cp!=null){if(c=="brake")return cp.Brake ? 100.0 : 0.0;if(c=="height")return cp.Height;if(c=="strength")return cp.Strength;if(c=="power")return cp.Power;
if(c=="steer")return cp.SteeringOverride*100.0;if(c=="propulsion")return cp.PropulsionOverride*100.0;return cp.Friction;}var nB=x as IMyShipConnector;
if(nB!=null){return nB.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var k0=x as IMyProductionBlock;if(k0!=null){if(c=="producing"||c=="active"||c=="running"||c=="state")return k0.IsProducing ? 100.0 : 0.0;
if(c=="queue")return k0.IsQueueEmpty ? 0.0 : 1.0;}var i_=x as IMyAssembler;if(i_!=null){if(c=="disassembling")return i_.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(c=="repeating"||c=="loop")return i_.Repeating ? 100.0 : 0.0;}var jT=x as IMyGasGenerator;if(jT!=null){if(c=="autorefill")return jT.AutoRefill ? 100.0 : 0.0;
if(c=="active")return jT.Enabled ? 100.0 : 0.0;}var hJ=x as IMyLargeTurretBase;if(hJ!=null){if(c=="shooting")return hJ.IsShooting ? 100.0 : 0.0;
if(c=="target"||c=="hastarget")return hJ.HasTarget ? 100.0 : 0.0;if(c=="range")return hJ.Range;}var ch=x as IMyProjector;if(ch!=null){if(c=="built")return ch.TotalBlocks-ch.RemainingBlocks;
if(c=="remaining")return ch.RemainingBlocks;if(c=="total")return ch.TotalBlocks;if(c=="projecting")return ch.IsProjecting ? 100.0 : 0.0;if(c=="armor")return ch.RemainingArmorBlocks;
if(c=="buildable")return ch.BuildableBlocksCount;}var lI=x as IMyShipToolBase;if(lI!=null){if(c=="working"||c=="active"||c=="activated")return lI.IsActivated ? 100.0 : 0.0;
return lI.Enabled ? 100.0 : 0.0;}var dt=x as IMyGyro;if(dt!=null){if(c=="override")return dt.GyroOverride ? 100.0 : 0.0;if(c=="pitch")return dt.Pitch;
if(c=="yaw")return dt.Yaw;if(c=="roll")return dt.Roll;if(c=="power")return dt.GyroPower*100.0;}var ol=x as IMyGravityGeneratorBase;if(ol!=null){if(c=="g"||c=="accel")return ol.GravityAcceleration;
}var oj=x as IMyLandingGear;if(oj!=null){if(c=="locked")return oj.IsLocked ? 100.0 : 0.0;}var mV=x as IMyRadioAntenna;if(mV!=null){if(c=="radius"||c=="range")return mV.Radius;
}var m7=x as IMyBeacon;if(m7!=null){if(c=="radius"||c=="range")return m7.Radius;}var kt=x as IMyLightingBlock;if(kt!=null){if(c=="intensity")return kt.Intensity;
if(c=="radius")return kt.Radius;}var hc=x as IMyPistonBase;if(hc!=null){if(c=="pos"||c=="position"||c=="extension")return hc.CurrentPosition;
if(c=="velocity")return hc.Velocity;if(c=="max")return hc.HighestPosition;}var ff=x as IMyMotorStator;if(ff!=null){if(c=="angle"||c=="deg")return ff.Angle*180.0/Math.PI;
if(c=="rad")return ff.Angle;if(c=="velocity"||c=="rpm")return ff.TargetVelocityRPM;if(c=="locked")return ff.RotorLock ? 100.0 : 0.0;}var lR=x as IMyWarhead;
if(lR!=null){if(c=="armed")return lR.IsArmed ? 100.0 : 0.0;if(c=="countdown")return lR.DetonationTime;}var pR=x as IMySensorBlock;if(pR!=null){
if(c=="detected")return!pR.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var jl=x as IMyCameraBlock;if(jl!=null){if(c=="range")return jl.AvailableScanRange;
if(c=="active")return jl.IsActive ? 100.0 : 0.0;}var nu=x as IMyParachute;if(nu!=null){if(c=="open"||c=="openratio")return(double)nu.OpenRatio*100.0;
}var dQ=x as IMyTurretControlBlock;if(dQ!=null){if(c=="target"||c=="hastarget")return dQ.HasTarget ? 100.0 : 0.0;if(c=="azimuth")return dQ.AzimuthRotor!=null ? dQ.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(c=="elevation")return dQ.ElevationRotor!=null ? dQ.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var oa=x as IMyFunctionalBlock;if(oa!=null){return oa.Enabled ? 100.0 : 0.0;
}return 0.0;}double bg(string ez,string n1){bt b6;en.TryGetValue(ez,out b6);if(b6!=null&&b6.bT==bb.d3){
string tw= !string.IsNullOrEmpty(b6.bv)? b6.bv : ez;double nE;if(aO.TryGetValue(tw,out nE))return nE;return b6.fG;
}double nS;if(aO.TryGetValue(ez,out nS))return nS;List<IMyTerminalBlock>eA;if(!b_.TryGetValue(ez,out eA)||eA.Count==0)throw new InvalidOperationException($"HELM: bind '{ez}' resolved to zero blocks — check the name/typo");
if(eA.Count==1)return mt(eA[0],n1);var qB=new List<double>();foreach(var sk in eA)qB.Add(mt(sk,n1));return qQ.qP(b6!=null ? b6.dT : aJ.hR,qB);
}double ir(string sx){return bg(sx,"percent");}static string be(double qx,bw dz){if(dz==null||(string.IsNullOrEmpty(dz.ek)&&string.IsNullOrEmpty(dz.d4)&&dz.I=="percent")){
return mc(qx);}return M.mb(qx,dz.ek,dz.d4);}bool rM(E ck){IMyTextSurface fj;if(!b0.TryGetValue(ck,out fj))return false;
if(ck.cr!=null)return rF(ck,fj);if(ck.id!=null)return rL(ck,fj);if(ck.c5.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return rN(ck,fj);}return rO(ck,fj);}Color al(bw oK,Color sS){return oK.iz!=null ? J(oK.iz): sS;
}Color d5(string[]dN,double uc){if(dN.Length==1)return J(dN[0]);double pu=Math.Max(0.0,Math.Min(100.0,uc))/100.0*(dN.Length-1);int j5=Math.Min((int)pu,dN.Length-2);
float hz=(float)(pu-j5);Color b3=J(dN[j5]),ge=J(dN[j5+1]);return new Color((int)(b3.R+(ge.R-b3.R)*hz),(int)(b3.G+(ge.G-b3.G)*hz),(int)(b3.B+(ge.B-b3.B)*hz),(int)(b3.A+(ge.A-b3.A)*hz));
}static Color[]fs(ay sB,string[]u4,string sU,float qK,float qM,float lO,float jZ){int ex=16;bool ou=sU=="horizontal";var jr=new Color[ex];
for(int cO=0;cO<ex;cO++){p jQ=M.md(u4,100.0*cO/(ex-1));jr[cO]=new Color(jQ.bW,jQ.bR,jQ.bN);float nl=lO/ex,ne=jZ/ex;r(sB,ou ? new Vector2(qK+nl*(cO+0.5f),qM+jZ/2f): new Vector2(qK+lO/2f,qM+ne*(cO+0.5f)),ou ? new Vector2(nl+1f,jZ): new Vector2(lO,ne+1f),jr[cO]);
}return jr;}static void r(ay sC,Vector2 un,Vector2 uV,Color sH,float uG=0f,string sT="SquareSimple"){sC.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=sT,Position=un,Size=uV,Color=sH,RotationOrScale=uG}
);}void ft(ay b7,string cX,string[]aQ,string nx,bool pZ,float aF,float aX,float bJ,float bh){cX=cX.ToLowerInvariant();float U=cX=="heavy" ? 4f : 2f,fr=bJ-aF,j_=bh-aX;
Color bC=J(aQ!=null&&aQ.Length>0 ? aQ[0]: nx ?? "40, 60, 85");Color b8=aQ!=null&&aQ.Length>1 ? J(aQ[aQ.Length-1]): bC;Action<float,float,float,float,Color>aU=(vC,vG,pt,ps,sE)=>r(b7,new Vector2(vC+pt/2f,vG+ps/2f),new Vector2(pt,ps),sE);
if(cX=="corners"){float aS=Math.Max(10f,Math.Min(fr,j_)*0.15f);aU(aF,aX,aS,U,bC);aU(aF,aX,U,aS,bC);aU(bJ-aS,aX,aS,U,b8);aU(bJ-U,aX,U,aS,b8);aU(aF,bh-U,aS,U,bC);aU(aF,bh-aS,U,aS,bC);
aU(bJ-aS,bh-U,aS,U,b8);aU(bJ-U,bh-aS,U,aS,b8);return;}if(aQ!=null&&aQ.Length>1&&pZ){fs(b7,aQ,"horizontal",aF,aX,fr,U);fs(b7,aQ,"horizontal",aF,bh-U,fr,U);
}else{aU(aF,aX,fr,U,bC);aU(aF,bh-U,fr,U,b8);}if("|frame|single|heavy|double|rounded|".Contains("|"+cX+"|")){aU(aF,aX,U,j_,bC);aU(bJ-U,aX,U,j_,b8);}if(cX=="rounded"){
float b9=U*3f;r(b7,new Vector2(aF,aX),new Vector2(b9,b9),bC,0f,"Circle");r(b7,new Vector2(bJ,aX),new Vector2(b9,b9),b8,0f,"Circle");r(b7,new Vector2(aF,bh),new Vector2(b9,b9),bC,0f,"Circle");
r(b7,new Vector2(bJ,bh),new Vector2(b9,b9),b8,0f,"Circle");}if(cX=="double")ft(b7,"single",aQ,nx,pZ,aF+5f,aX+5f,bJ-5f,bh-5f);}void qX(E n,IMyTextSurface lv,ay g){
g.f.Clear();g.eg.Clear();g.bV.Clear();Vector2 _=lv.SurfaceSize;Vector2 vg=lv.TextureSize;Vector2 br=(vg-_)/2f;
g.ii=br;float g4=Math.Max(4f,_.X*0.02f);float e4=Math.Max(4f,_.Y*0.02f);if(n.cA=="compact"||(n.cA=="auto"&&_.Y<=260f&&_.Y>150f)){
g4=4f;e4=4f;}else if(n.cA=="corner"||(n.cA=="auto"&&_.Y<=150f)){g4=2f;e4=2f;}float qq=_.X-(g4*2f);
Color dh=new Color(10,12,16);bool nO=true;if(!string.IsNullOrEmpty(n.bO)){var eM=p.ed(n.bO);if(eM.fO)nO=false;
else dh=new Color(eM.bW,eM.bR,eM.bN,eM.hP);}g.cs= -1;g.fv=false;g.fu=J(n.lW ?? "#5b0f0f");g.dX.Clear();if(n.bQ!=null&&n.bQ.Length>1){
g.cs=g.f.Count;g.fy=fs(g,n.bQ,n.dW,br.X+2f,br.Y+2f,_.X-4f,_.Y-4f);
}else if(nO||n.a_!=null||n.c6!=null){if(n.a_!=null)dh=J(n.a_.First(us=>us.c8).cw);g.cs=g.f.Count;
g.fy=new[]{dh};r(g,br+(_/2f),_-new Vector2(4f,4f),dh);if(n.a_!=null||n.c6!=null)g.dX.Add(new fz{
fN=g.cs,hS=dh,fF=dh,fW=n.a_,hW=n.hT,h3=n.hU,fK=n.c6,h4=n.h5,h6=n.h7,mi=true}
);}if(!string.IsNullOrEmpty(n.hV))r(g,br+(_/2f),_-new Vector2(4f,4f),J(n.lY ?? "255,255,255,24"),0f,n.hV);
if(!n.c5.Equals("none",StringComparison.OrdinalIgnoreCase)){float ng=br.X+(_.X-qq)/2f;ft(g,n.c5,n.fE,n.dZ,!n.fM,ng-2f,br.Y+e4*0.5f-1f,ng+qq+2f,br.Y+_.Y-e4*0.5f+1f);
}var kR=n.ak;if(kR.Count==0){kR=new List<aE>{new aE{db="main",d0=0,ef=0,eh=n.d6,ei=n.d7,ap=n.fR}
};}float oo=8f;foreach(var q in kR){var g3=M.q_(q.d0,q.ef,q.eh,q.ei,n.d6,n.d7,_.X,_.Y,g4,e4,oo,oo);
float az=br.X+g3.mH;float bH=br.Y+g3.mI;float aT=g3.mG;float cR=g3.mf;g.bV.Add(new im{mn=q.db,it=g.f.Count,ms=new Vector2(az,bH),mw=new Vector2(aT,cR)}
);bool pD=q.a_!=null||q.c6!=null;if(q.bQ!=null&&q.bQ.Length>1){fs(g,q.bQ,q.dW!="vertical" ? q.dW : n.dW,az,bH,aT,cR);
}else{Color cg;if(q.a_!=null)cg=J(q.a_.First(ut=>ut.c8).cw);else if(!string.IsNullOrEmpty(q.c3))cg=J(q.c3);
else if(!string.IsNullOrEmpty(n.il))cg=J(n.il);else cg=Color.Transparent;if(cg.A>0||pD){if(pD)g.dX.Add(new fz{fN=g.f.Count,hS=cg,fF=cg,fW=q.a_,hW=q.hT,h3=q.hU,fK=q.c6,h4=q.h5,h6=q.h7}
);r(g,new Vector2(az+aT/2f,bH+cR/2f),new Vector2(aT,cR),cg);}}if(q.dY!=null&& !q.dY.Equals("none",StringComparison.OrdinalIgnoreCase))ft(g,q.dY,q.fD ?? n.fE,q.fC ?? n.dZ,!n.fM,az+1f,bH+1f,az+aT-1f,bH+cR-1f);
string gE=q.l8 ?? n.l9 ?? "Monospace";float st=M.rf(n.cA,_.X,_.Y,n.c7);float aP=q.ma ?? st;
float eN=0f;if(!string.IsNullOrEmpty(q.cE)){eN=20f*aP;var iS=TextAlignment.LEFT;float lF=az+4f;string mN=q.ix ?? n.mC ?? "left";
if(mN.Equals("center",StringComparison.OrdinalIgnoreCase)){iS=TextAlignment.CENTER;lF=az+(aT/2f);}else if(mN.Equals("right",StringComparison.OrdinalIgnoreCase)){
iS=TextAlignment.RIGHT;lF=az+aT-4f;}string qg=q.iy ?? n.mD;Color vh= !string.IsNullOrEmpty(qg)? J(qg): J(q.fC ?? n.dZ ?? "white");
var ql=MySprite.CreateText(q.cE,gE,vh,aP*0.9f,iS);ql.Position=new Vector2(lF,bH+2f);g.f.Add(ql);
string[]i5=q.fD ?? n.fE;Color sW=i5!=null&&i5.Length>0 ? J(i5[0]): J(q.fC ?? n.dZ ?? "40, 60, 85");
r(g,new Vector2(az+aT/2f,bH+eN-2f),new Vector2(aT-8f,1f),sW);}if(q.ij=="vertical"||q.ap.Any(uj=>uj.h==e.fY)){
qY(g,lv,q,gE,aP,az,bH,aT,cR,eN);continue;}int vI=Math.Max(1,q.ap.Count);float ua=cR-eN-8f;
float qH=0f,mP=0f;foreach(var uk in q.ap)qH+=1<<uk.fX;float vk=ua/Math.Max(1f,qH);float gP=6f;for(int j6=0;j6<q.ap.Count;
j6++){var k=q.ap[j6];var N=new cy{fQ=k};float dH=vk*(1<<k.fX);float pB=bH+eN+4f+mP;mP+=dH;float L=pB+(dH/2f);
float qi=L-(11f*aP);N.c9=g.f.Count;N.me=k.ab!=null&&(k.ab.Contains("{val}")||k.ab.Contains("{percent}"));
var oO=MySprite.CreateText(k.ab,gE,k.ea!=null ? J(k.ea): Color.White,aP,TextAlignment.LEFT);oO.Position=new Vector2(az+gP,qi);
g.f.Add(oO);float tK=Math.Max(24f*aP,(k.ab.Length+1)*12f*aP);float vt=(!string.IsNullOrEmpty(k.ek)|| !string.IsNullOrEmpty(k.d4))? 64f*aP : 52f*aP;
if(k.h!=e.iv){float K=az+gP+tK;float ss=az+aT-gP-vt;float H=Math.Max(16f,ss-K);
float O=Math.Max(k.h==e.cB ? 8f : 4f,Math.Min((k.h==e.cB ? 20f : 16f)*aP,dH*(k.h==e.cB ? 0.55f : 0.45f)));
if(k.fX>0)O=dH*0.7f;Color qp=al(k,new Color(25,30,38));N.ba=K;N.aC=H;N.a9=O;N.bs=L;
if(k.h==e.dV){r(g,new Vector2(K+(H/2f),L),new Vector2(H,O),qp);N.aa=g.f.Count;
r(g,new Vector2(K,L),new Vector2(0f,O),J(k.ax));bool uE=k.fI=="left";for(int lG=1;lG<k.iw;lG++)r(g,new Vector2(K+H*lG/k.iw,L),new Vector2(1f,O),new Color(0,0,0,150));
if(k.fS!=null)foreach(double oW in k.fS)r(g,new Vector2(K+H*(float)(uE ? 1.0-oW/100.0 : oW/100.0),L),new Vector2(2f,O+6f),Color.White);
if(k.ik!=null)ft(g,"single",null,k.ik,true,K,L-O/2f,K+H,L+O/2f);}else if(k.h==e.fH){
float dF=Math.Max(8f,Math.Min(H/2f,dH*0.85f));float gu=K+H/2f,gv=pB+dH-4f;int jw=k.bz>0 ? k.bz : 16;N.ba=gu;
N.bs=gv;N.aC=dF;for(int jt=0;jt<jw;jt++){float lD=(jt+0.5f)/jw*3.14159f;N.D.Add(g.f.Count);r(g,new Vector2(gu-(float)Math.Cos(lD)*dF*0.8f,gv-(float)Math.Sin(lD)*dF*0.8f),new Vector2(dF*3.14159f/jw*0.85f,Math.Max(3f,dF*0.16f)),al(k,new Color(24,28,36)),lD-1.5708f);
}N.D.Add(g.f.Count);r(g,new Vector2(gu,gv),new Vector2(3f,dF*0.9f),Color.White);r(g,new Vector2(gu,gv),new Vector2(8f,8f),Color.White,0f,"Circle");
}else if(k.h==e.ec){int lg=k.bz>0 ? k.bz : 10;float oc=2f;float lh=Math.Max(2f,(H-(oc*(lg-1)))/lg);for(int la=0;
la<lg;la++){N.D.Add(g.f.Count);r(g,new Vector2(K+la*(lh+oc)+(lh/2f),L),new Vector2(lh,O),al(k,new Color(22,26,32)));
}}else if(k.h==e.cB){r(g,new Vector2(K+(H/2f),L),new Vector2(H,O),al(k,new Color(20,24,30)));int hd=k.bz>0 ? k.bz : 8;
N.cC=new float[hd];float od=2f;float jp=Math.Max(2f,(H-(od*(hd-1)))/hd);for(int kN=0;kN<hd;kN++){N.D.Add(g.f.Count);
r(g,new Vector2(K+kN*(jp+od)+(jp/2f),L),new Vector2(jp,2f),J(k.ax));}}else if(k.h==e.d8){r(g,new Vector2(K+(H/2f),L),new Vector2(H,O),al(k,new Color(20,24,30)));
int lr=10;float oe=2f;float ls=Math.Max(2f,(H-(oe*(lr-1)))/lr);for(int lb=0;lb<lr;lb++){N.D.Add(g.f.Count);
r(g,new Vector2(K+lb*(ls+oe)+(ls/2f),L),new Vector2(ls,O),al(k,new Color(24,28,36)),0.523599f);}}else if(k.h==e.d_){
int iY=10;float of=2f;float iZ=Math.Max(2f,(H-(of*(iY-1)))/iY);for(int lc=0;lc<iY;lc++){N.D.Add(g.f.Count);
r(g,new Vector2(K+lc*(iZ+of)+(iZ/2f),L),new Vector2(iZ,O),al(k,new Color(24,28,36)),1.570796f,"Triangle");}}else if(k.h==e.d2){
int jx=k.bz>0 ? k.bz : 12;float og=2f;float jz=Math.Max(2f,(H-(og*(jx-1)))/jx);float nN=Math.Min(jz,O);for(int ld=0;
ld<jx;ld++){N.D.Add(g.f.Count);r(g,new Vector2(K+ld*(jz+og)+(jz/2f),L),new Vector2(nN,nN),al(k,new Color(24,28,36)),0f,"Circle");
}}else if(k.h==e.c4){r(g,new Vector2(K+(H/2f),L),new Vector2(H,O),qp);N.aa=g.f.Count;
r(g,new Vector2(K+(H/2f),L),new Vector2(0f,O),J(k.ax));r(g,new Vector2(K+(H/2f),L),new Vector2(2f,O+4f),new Color(80,110,150));
}else if(k.h==e.cF){r(g,new Vector2(K+(H/2f),L),new Vector2(H,O),al(k,new Color(20,24,30)));N.aa=g.f.Count;
float km=Math.Max(4f,O);r(g,new Vector2(K+km*0.5f,L),new Vector2(km,km),new Color(40,50,65),0f,"Circle");}else if(k.h==e.cv){
float dj=Math.Max(6f,Math.Min(O,14f*aP));r(g,new Vector2(K+dj*0.5f,L),new Vector2(dj,dj),new Color(28,34,46));
N.aa=g.f.Count;r(g,new Vector2(K+dj*0.5f,L),new Vector2(dj*0.6f,dj*0.6f),new Color(40,50,65));}else if(k.h==e.cD){
r(g,new Vector2(K+(H/2f),L),new Vector2(H,O),al(k,new Color(18,22,30)));N.aa=g.f.Count;float kY=Math.Max(4f,O*0.7f);
r(g,new Vector2(K+kY+2f,L),new Vector2(kY,kY),new Color(34,197,94),0f,"Circle");}else if(k.h==e.c2){int kX=8;
float oh=2f;float kZ=Math.Max(2f,(H-(oh*(kX-1)))/kX);for(int kO=0;kO<kX;kO++){N.D.Add(g.f.Count);r(g,new Vector2(K+kO*(kZ+oh)+(kZ/2f),L),new Vector2(kZ,O),al(k,new Color(24,28,36)));
}}else if(k.h==e.cx){r(g,new Vector2(K+(H/2f),L),new Vector2(H,O),al(k,new Color(20,24,30)));N.aa=g.f.Count;
float ox=Math.Max(4f,O);r(g,new Vector2(K+(H/2f),L),new Vector2(ox,ox),J(k.ax),0f,"Triangle");}}var qv=MySprite.CreateText("---",gE,Color.White,aP,TextAlignment.RIGHT);
qv.Position=new Vector2(az+aT-gP,qi);N.a3=g.f.Count;g.f.Add(qv);g.eg.Add(N);}}g.eb=_;
g.d9=true;}const string r1="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bY=8;void qY(ay ac,IMyTextSurface u7,aE kQ,string n7,float eK,float t7,float kP,float t8,float pd,float tn){
int tY=Math.Max(1,kQ.ap.Count);float ny=(t8-12f)/tY;float hK=22f*eK;float bp=kP+tn+4f+hK;float aY=Math.Max(20f,kP+pd-4f-hK-bp);
float sP=u7.MeasureStringInPixels(new StringBuilder("M"),n7,eK*0.9f).X;for(int gO=0;gO<kQ.ap.Count;gO++){var cP=kQ.ap[gO];float bk=t7+6f+ny*(gO+0.5f);
float c0=ny*0.7f;var bm=new cy{fQ=cP,mE=true,ba=bk,aC=c0,a9=aY,bs=bp+aY/2f,l3=sP};bm.c9=ac.f.Count;
a8(ac.f,cP.ab,cP.ea!=null ? J(cP.ea): Color.White,bk,kP+pd-4f-hK,eK);bm.a3=ac.f.Count;
a8(ac.f,"---",Color.White,bk,bp-hK,eK);var oL=ac.f[bm.c9];oL.Alignment=TextAlignment.CENTER;ac.f[bm.c9]=oL;
var qu=ac.f[bm.a3];qu.Alignment=TextAlignment.CENTER;ac.f[bm.a3]=qu;r(ac,new Vector2(bk,bp+aY/2f),new Vector2(c0,aY),al(cP,new Color(25,30,38)));
bm.aa=ac.f.Count;if(cP.h==e.fY){for(int m2=0;m2<bY;m2++){bm.D.Add(ac.f.Count);r(ac,new Vector2(bk,bp+aY),new Vector2(c0-6f,0f),Color.White);
}for(int lP=0;lP<4;lP++){bm.D.Add(ac.f.Count);var qb=MySprite.CreateText("",n7,lP==0 ? new Color(255,255,255,170): new Color(255,255,255,110),eK*0.9f,TextAlignment.CENTER);
qb.Position=new Vector2(bk,bp+aY);ac.f.Add(qb);}Color jB=new Color(90,110,135);r(ac,new Vector2(bk-c0/2f,bp+aY/2f),new Vector2(3f,aY),jB);r(ac,new Vector2(bk+c0/2f,bp+aY/2f),new Vector2(3f,aY),jB);
r(ac,new Vector2(bk,bp+aY),new Vector2(c0+3f,3f),jB);}else{r(ac,new Vector2(bk,bp+aY),new Vector2(c0,0f),J(cP.ax));}ac.eg.Add(bm);
}}bool r0(ay bi,cy z,double fp,double pv,Color go,bool qz){var a6=z.fQ;float m9=z.C;if(z.C<0f)z.C=(float)fp;
z.C=Math.Abs(z.C-(float)fp)>0.05f ? z.C+((float)fp-z.C)*0.35f :(float)fp;float cQ=Math.Max(0f,Math.Min(1f,z.C/100f))*z.a9;
float gl=z.bs+z.a9/2f;float gs=z.ba;if(a6.h!=e.fY){var gz=bi.f[z.aa];gz.Size=new Vector2(z.aC,cQ);
gz.Position=new Vector2(gs,gl-cQ/2f);gz.Color=go;bi.f[z.aa]=gz;if(qz||z.C!=m9)aM(bi,z,be(pv,a6),go);
return qz||z.C!=m9;}float i7=z.a9/bY;string fa=a6.bu!=null ? M.ee(a6.bu,a6.d1!=null ? bg(a6.d1,a6.h_): fp): null;
Color tS=fa!=null ? J(fa): go;string[]p3=fa!=null ?(fa.Contains("/")? fa.Split('/'): null): a6.bS&&a6.aL.Length>1 ? a6.aL : null;
for(int cK=0;cK<bY;cK++){float qE=Math.Max(0f,Math.Min(i7,cQ-cK*i7));var he=bi.f[z.D[cK]];he.Size=new Vector2(z.aC-6f,qE);
he.Position=new Vector2(gs,gl-cK*i7-qE/2f);he.Color=p3!=null ? d5(p3,100.0*(cK+0.5)/bY): tS;bi.f[z.D[cK]]=he;}
int sF=Math.Max(1,(int)((z.aC-8f)/z.l3));bool uS=cQ>6f;var lS=bi.f[z.D[bY]];lS.Data=uS ? r1.Substring((int)(P*5.0)%6,Math.Min(sF,100)): "";
lS.Position=new Vector2(gs,gl-cQ-12f);bi.f[z.D[bY]]=lS;for(int ce=0;ce<3;ce++){var jg=bi.f[z.D[bY+1+ce]];
double uf=(P*(0.35+0.1*ce)+ce*0.37)%1.0;jg.Data=cQ>24f ?(ce==1 ? "O" : "o"): "";jg.Position=new Vector2(gs+(ce-1)*z.aC*0.26f,gl-(float)uf*(cQ-6f)-10f);
bi.f[z.D[bY+1+ce]]=jg;}aM(bi,z,be(pv,a6),go);return true;}static void aM(ay ni,cy pX,string vf,Color sI){
var lM=ni.f[pX.a3];lM.Data=vf;lM.Color=sI;ni.f[pX.a3]=lM;}static void mv(ay nj,cy pY,Color sJ){
var nW=nj.f[pY.aa];nW.Color=sJ;nj.f[pY.aa]=nW;}bool rO(E bM,IMyTextSurface lw){ay j;
if(!b1.TryGetValue(bM,out j)){j=new ay();b1[bM]=j;}bool py= !j.d9||j.eb!=lw.SurfaceSize;
if(py){qX(bM,lw,j);}bool Q=py;bool ga=(P%bM.hQ)<bM.hQ/2.0;double cN= -1.0,nC;if(f0.TryGetValue(bM,out nC)){
cN=(P-nC)/bM.hY;if(cN>=1.0){f0.Remove(bM);cN=1.0;}}bool lQ=cH&&ga&&j.fu.A>0;if(j.cs>=0&&lQ!=j.fv){
j.fv=lQ;for(int eP=0;eP<j.fy.Length;eP++){var na=j.f[j.cs+eP];na.Color=lQ ? j.fu : j.fy[eP];j.f[j.cs+eP]=na;
}Q=true;foreach(var su in j.dX)su.h8=true;}foreach(var am in j.dX){if(am.mi&&j.fv)continue;Color ey=am.hS;try{if(am.fW!=null){
double nk=bg(am.hW,am.h3);string hj=M.ee(am.fW,nk);ey=hj!=null&&hj.Contains("/")? d5(hj.Split('/'),nk): J(hj);
}if(am.fK!=null&&ga&&M.ee(am.fK,bg(am.h4,am.h6))!=null)ey=j.fu;}catch{}if(ey!=am.fF||am.h8){
am.fF=ey;am.h8=false;var nh=j.f[am.fN];nh.Color=ey;j.f[am.fN]=nh;Q=true;}}for(int j7=0;j7<j.eg.Count;j7++){var a=j.eg[j7];
var i=a.fQ;double F;bool oq=false;double kA=i.fT;try{F=bg(i.aK,i.I);if(i.da!=null)kA=bg(i.da,i.ic);
}catch{oq=true;F=0.0;}if(oq){if(a.fP!= -99999.0||(a.a3>=0&&j.f[a.a3].Data!="ERR")){Q=true;a.fP= -99999.0;
if(a.a3>=0){var le=j.f[a.a3];le.Data="ERR";le.Color=Color.Red;j.f[a.a3]=le;}}continue;}double a7=F;
if(kA>0)F=Math.Min(100.0,a7/kA*100.0);bool p8=cN>=0.0&&i.h!=e.iv&&i.h!=e.cF&&i.h!=e.cv&&i.h!=e.cD&&i.h!=e.cx;
if(p8)F=cN<0.5 ? cN*200.0 : 100.0+(F-100.0)*(cN-0.5)*2.0;double gr=i.d1!=null ? bg(i.d1,i.h_): F;string gp=i.bu!=null ? M.ee(i.bu,gr): i.bS ? M.fL(i.aL,gr): i.ax;
Color V=J(gp);bool bE=(V==Color.Red);if(gp!=null&&gp.Contains("/"))V=d5(gp.Split('/'),gr);if(i.bu==null&&i.bS&& !bM.fM&&i.aL.Length>1)V=d5(i.aL,gr);
if(bE&&ga){V=new Color(255,75,75);Q=true;}bool aZ=p8||Math.Abs(a7-a.fP)>0.1;if(aZ)a.fP=a7;
if(a.mE){if(r0(j,a,F,a7,V,aZ))Q=true;continue;}if(i.h==e.dV||i.h==e.fH){double n2=(i.ia||i.I=="percent"||i.I=="pct")? F : ir(i.aK);
if(a.C<0f)a.C=(float)n2;float hC=(float)n2;if(Math.Abs(a.C-hC)>0.05f){a.C+=(hC-a.C)*0.35f;
Q=true;}else if(a.C!=hC){a.C=hC;Q=true;}if(aZ)Q=true;if(Q){float pm=(float)Math.Max(0.0,Math.Min(1.0,a.C/100.0));
float jN=a.aC*pm;if(i.h==e.fH){int eE=a.D.Count-1;for(int dq=0;dq<eE;dq++){var pT=j.f[a.D[dq]];
pT.Color=a.C>=(dq+0.5)/eE*100.0 ?(i.bu==null&&i.bS&&i.aL.Length>1 ? d5(i.aL,(dq+0.5)/eE*100.0): V): al(i,new Color(24,28,36));
j.f[a.D[dq]]=pT;}float iV=pm*3.14159f;var kD=j.f[a.D[eE]];kD.Position=new Vector2(a.ba-(float)Math.Cos(iV)*a.aC*0.45f,a.bs-(float)Math.Sin(iV)*a.aC*0.45f);
kD.RotationOrScale=iV-1.5708f;j.f[a.D[eE]]=kD;}else{var gB=j.f[a.aa];gB.Size=new Vector2(jN,a.a9);
gB.Position=new Vector2(i.fI=="left" ? a.ba+a.aC-(jN/2f): a.ba+(jN/2f),a.bs);gB.Color=V;
j.f[a.aa]=gB;}aM(j,a,be(a7,i),V);}}else if(i.h==e.ec||i.h==e.d8||i.h==e.d_||i.h==e.d2){
if(aZ||bE){Q=true;double s4=(i.ia||i.I=="percent"||i.I=="pct")? F : ir(i.aK);int pP=a.D.Count;
for(int fh=0;fh<pP;fh++){bool tT=s4>=((fh+0.4)/(double)pP*100.0);Color uQ=tT ? V : al(i,new Color(24,28,36));var pQ=j.f[a.D[fh]];
pQ.Color=uQ;j.f[a.D[fh]]=pQ;}aM(j,a,be(a7,i),V);}}else if(i.h==e.cB){
if(aZ||bE){Q=true;for(int gJ=0;gJ<a.cC.Length-1;gJ++)a.cC[gJ]=a.cC[gJ+1];a.cC[a.cC.Length-1]=(float)F;
int um=a.D.Count;for(int e0=0;e0<um;e0++){float tm=a.cC[e0];float tl=Math.Max(0.06f,Math.Min(1f,tm/100f));float nw=a.a9*tl;
float sK=a.bs+(a.a9/2f)-(nw/2f);var dp=j.f[a.D[e0]];dp.Size=new Vector2(dp.Size.Value.X,nw);
dp.Position=new Vector2(dp.Position.Value.X,sK);dp.Color=V;j.f[a.D[e0]]=dp;}aM(j,a,be(a7,i),V);
}}else if(i.h==e.c4){if(a.C<0f)a.C=(float)F;float hD=(float)F;if(Math.Abs(a.C-hD)>0.05f){
a.C+=(hD-a.C)*0.35f;Q=true;}else if(a.C!=hD){a.C=hD;Q=true;}if(Q||aZ){
float op=a.aC*0.5f;float nr=a.ba+op;float nI=(float)Math.Max(-1.0,Math.Min(1.0,(a.C-50.0)/50.0));float jO=Math.Abs(nI)*op;
float s5=(nI>=0)?(nr+jO*0.5f):(nr-jO*0.5f);var gC=j.f[a.aa];gC.Size=new Vector2(jO,a.a9);
gC.Position=new Vector2(s5,a.bs);gC.Color=V;j.f[a.aa]=gC;aM(j,a,be(a7,i),V);
}}else if(i.h==e.cF){if(aZ||bE){Q=true;bool kf=F>=50.0;float oG=Math.Max(4f,a.a9);float tz=kf ?(a.ba+a.aC-oG*0.5f):(a.ba+oG*0.5f);
Color oF=kf ?(V!=Color.White ? V : new Color(34,197,94)): new Color(60,70,85);var jL=j.f[a.aa];jL.Position=new Vector2(tz,a.bs);
jL.Color=oF;j.f[a.aa]=jL;aM(j,a,kf ? "ON" : "OFF",oF);}}else if(i.h==e.cv){if(aZ||bE){
Q=true;bool ke=F>=50.0;Color ns=ke ?(V!=Color.White ? V : new Color(56,189,248)): new Color(30,36,48);mv(j,a,ns);
aM(j,a,ke ? "ACTIVE" : "READY",ke ? ns : new Color(148,163,184));}}else if(i.h==e.cD){if(aZ||bE){
Q=true;string u2=F>=75.0 ? "ONLINE" :(F>=25.0 ? "STBY" : "OFFLINE");Color p2=F>=75.0 ? new Color(34,197,94):(F>=25.0 ? new Color(234,179,8): new Color(239,68,68));
mv(j,a,p2);aM(j,a,u2,p2);}}else if(i.h==e.c2){if(aZ||bE){Q=true;int pn=a.D.Count;
for(int e1=0;e1<pn;e1++){bool tU=F>=((e1+0.5)/(double)pn*100.0);Color uh=tU ? V : al(i,new Color(24,28,36));var po=j.f[a.D[e1]];
po.Color=uh;j.f[a.D[e1]]=po;}aM(j,a,be(a7,i),V);}}else if(i.h==e.cx){
Q=true;float hk;Color eJ;string gD;bool tr=(i.I=="producing"||i.I=="active"||i.I=="running"||i.I=="state"||(i.I!="percent"&&(F<=1.0||F==100.0)));
if(tr){if(F>0.0){a.C=(a.C+0.3927f)%6.28318f;hk=a.C;eJ=new Color(34,197,94);gD="RUN";}else{hk=0f;
eJ=new Color(148,163,184);gD="IDLE";}}else{hk=(F>55.0)? 0f :((F<45.0)? 3.14159f : 1.57079f);eJ=(F>55.0)? new Color(34,197,94):((F<45.0)? new Color(239,68,68): new Color(148,163,184));
gD=(F>55.0)? "CHG" :((F<45.0)? "DSC" : "STBY");}var jM=j.f[a.aa];jM.RotationOrScale=hk;jM.Color=eJ;
j.f[a.aa]=jM;aM(j,a,gD,eJ);}else{string gy;bt hF;en.TryGetValue(i.aK,out hF);bB.TryGetValue(mj(i.I)? i.aK+"."+i.I :(hF!=null&&hF.bv!=null ? hF.bv : i.aK),out gy);
if(aZ||bE||gy!=a.mk){a.mk=gy;Q=true;Color lJ=V;if(lJ==Color.Red&&ga)lJ=new Color(255,75,75);aM(j,a,gy ?? be(a7,i),lJ);
}}if(a.me&&(aZ||bE)){Q=true;var oQ=j.f[a.c9];string ud=mc(F);string uD=be(a7,i);
oQ.Data=i.ab.Replace("{percent}",ud).Replace("{val}",uD);j.f[a.c9]=oQ;}}if(Q){j.el++;using(var tb=lw.DrawFrame()){
tb.AddRange(j.f);}}return Q;}bool rN(E cl,IMyTextSurface lx){lx.FontSize=cl.c7;var gq=new List<KeyValuePair<string,string>>();
foreach(var o in cl.fR){double T=0.0;string bo;string bj=null;double kB=o.fT;try{T=bg(o.aK,o.I);if(o.da!=null)kB=bg(o.da,o.ic);
}catch(Exception vH){gq.Add(new KeyValuePair<string,string>($"{o.ab}: [UNRESOLVED]","red"));continue;}double pw=T;if(kB>0)T=Math.Min(100.0,pw/kB*100.0);
string qw=be(pw,o);if(o.h==e.dV){double n3=(o.ia||o.I=="percent"||o.I=="pct")? T : ir(o.aK);
bo=M.rG(o.ab,n3,10,qw);bj=o.bS ? M.fL(o.aL,n3): o.ax;}else if(o.h==e.c4){
bo=M.rE(o.ab,T,10);bj=o.bS ? M.fL(o.aL,T): o.ax;}else if(o.h==e.cF){
bo=M.rQ(o.ab,T);bj=o.ax ??(T>=50.0 ? "green" : "gray");}else if(o.h==e.cv){bo=M.rJ(o.ab,T);
bj=o.ax ??(T>=50.0 ? "cyan" : "gray");}else if(o.h==e.cD){bo=M.rP(o.ab,T);bj=o.ax ??(T>=75.0 ? "lime" :(T>=25.0 ? "yellow" : "red"));
}else if(o.h==e.c2){bo=M.rD(o.ab,T,8);bj=o.bS ? M.fL(o.aL,T):(o.ax ?? "yellow");
}else if(o.h==e.cx){bool ts=(o.I=="producing"||o.I=="active"||o.I=="running"||o.I=="state"||(o.I!="percent"&&(T<=1.0||T==100.0)));
if(ts){char[]u0=new[]{'|','/','-','\\'};char uN=u0[Math.Abs(et)%4];bo=T>0 ? $"{o.ab} [{uN}] RUN" : $"{o.ab} [==] IDLE";
bj=T>0 ? "green" : "gray";}else{bo=M.rK(o.ab,T,et);bj=o.ax ??(T>55.0 ? "green" :(T<45.0 ? "red" : "cyan"));
}}else{bo=$"{o.ab}: {qw}";bj=o.bu!=null ? M.ee(o.bu,T): null;}gq.Add(new KeyValuePair<string,string>(bo,bj));
}var gk=M.rI(new List<string>(gq.ConvertAll(va=>va.Key)),dS: cl.mF);string pU=string.Join("\n",gk);
string pq;if(cI.TryGetValue(cl,out pq)&&pq==pU)return false;cI[cl]=pU;using(var tc=lx.DrawFrame()){
float qN=4f;float oS=qO*cl.c7;try{var oV=lx.MeasureStringInPixels(new StringBuilder("A"),"Monospace",cl.c7);
if(oV.Y>0)oS=oV.Y;}catch{}for(int du=0;du<gk.Count;du++){string nA=null;if(du>0&&du<gk.Count-1)nA=gq[du-1].Value;
var p1=MySprite.CreateText(gk[du],"Monospace",J(nA),cl.c7,TextAlignment.LEFT);p1.Position=new Vector2(4f,qN);tc.Add(p1);
qN+=oS;}}return true;}Color J(string g1){if(string.IsNullOrEmpty(g1))return Color.White;Color eW;if(mJ.TryGetValue(g1,out eW))return eW;
p eB=p.ed(g1);eW=eB.fO ? Color.Transparent : new Color(eB.bW,eB.bR,eB.bN,eB.hP);mJ[g1]=eW;return eW;}public enum bb{qS,h9,d3}
public enum aJ{ro,hR,mA,mm,ml}public class bt{public string af;public bb bT;public List<string>a2=new List<string>();public string mB{
get{return a2.Count>0 ? a2[0]: "";}set{if(a2.Count==0)a2.Add(value);else a2[0]=value;}}public aJ dT=aJ.ro;
public double fG=0.0;public string h0;public string bv;public bool fx;}public enum e{iv,dV,ec,cB,d8,d_,d2,c4,cF,cv,cD,c2,cx,fY,fH}
public class a0{public string h2;public string cw;public bool c8;}public class bw{public e h;public string ab;public string aK;
public string I;public bool bS;public string[]aL;public string ax;public List<a0>bu;public string fI="right";
public string ek;public string d4;public List<double>fS;public int bz;public int iw;public string ik;public int fX;public string d1,h_;
public string ea,iz;public double fT;public string da,ic;public bool ia{get{return fT>0||da!=null;}}}public struct p{public byte bW,bR,bN,hP;
public bool fO;public p(byte uu,byte te,byte sl,byte r3=255){bW=uu;bR=te;bN=sl;hP=r3;fO=false;}public static readonly p r_=new p(0,0,0,0){
fO=true};public static readonly p fZ=new p(255,255,255);public static readonly p qR=new p(0,0,0);public static readonly p rB=new p(255,0,0);
public static readonly p rh=new p(0,255,0);public static readonly p rl=new p(50,205,50);public static readonly p r2=new p(255,255,0);
public static readonly p rp=new p(255,140,0);public static readonly p q2=new p(0,255,255);public static readonly p qT=new p(0,120,255);
public static readonly p rm=new p(255,0,255);public static readonly p rz=new p(128,0,128);public static readonly p rg=new p(128,128,128);
public static readonly p q3=new p(60,60,60);public static p ed(string bn){if(string.IsNullOrWhiteSpace(bn))return fZ;bn=bn.Trim();
if(bn.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bn.Equals("none",StringComparison.OrdinalIgnoreCase))return r_;if(bn.StartsWith("#")){
string cc=bn.Substring(1).Trim();if(cc.Length==6){byte uv=Convert.ToByte(cc.Substring(0,2),16);byte tf=Convert.ToByte(cc.Substring(2,2),16);byte sm=Convert.ToByte(cc.Substring(4,2),16);
return new p(uv,tf,sm);}if(cc.Length==3){byte uw=Convert.ToByte(new string(cc[0],2),16);byte tg=Convert.ToByte(new string(cc[1],2),16);byte sn=Convert.ToByte(new string(cc[2],2),16);
return new p(uw,tg,sn);}}if(bn.Contains(",")){var dC=bn.Split(',');if(dC.Length>=3){byte ux=(byte)Math.Max(0,Math.Min(255,int.Parse(dC[0].Trim())));
byte th=(byte)Math.Max(0,Math.Min(255,int.Parse(dC[1].Trim())));byte so=(byte)Math.Max(0,Math.Min(255,int.Parse(dC[2].Trim())));byte r4=dC.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(dC[3].Trim()))):(byte)255;
return new p(ux,th,so,r4);}}switch(bn.ToLowerInvariant()){case "red": return rB;case "green": return rh;case "lime": return rl;case "yellow": return r2;
case "orange": return rp;case "cyan": return q2;case "blue": return qT;case "magenta": return rm;case "purple": return rz;case "white": return fZ;
case "black": return qR;case "gray": case "grey": return rg;case "darkgray": case "darkgrey": return q3;default: return fZ;}}}public class bP{
public string[]bQ;public string dW="vertical";public List<a0>a_;public string hT,hU;public List<a0>c6;public string h5,h7;
}public class aE : bP{public string db="default";public int d0=0;public int ef=0;public int eh=1;public int ei=1;public string cE;public string ix="left";
public string iy;public string dY;public string l8;public float? ma;public string c3;public string fC;public string[]fD;
public string ij="horizontal";public List<bw>ap=new List<bw>();}public class E : bP{public string bf;public string cA="standard";
public string c5="graphic";public string dZ;public string[]fE;public string mC="left";public string mD;public string l9="Monospace";
public string bO;public string l0;public int l1=10;public int lZ=1;public float hY=1.2f;public bool fM;
public string il;public string hV;public string lY;public string lW;public string ie;public string id;public string cr;
public string cu;public float hZ=2.5f;public string l_="cyan";public string[]fA;public List<string>fB=new List<string>();
public List<string>re(){if(fB!=null&&fB.Count>0)return fB;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float c7=1.0f;public int ip=0;public float hQ=1.6f;public int d6=1;public int d7=1;public int mF=30;public string bU;
public int cz=0;public List<aE>ak=new List<aE>();public List<bw>fR{get{if(ak.Count==0)ak.Add(new aE{db="main"}
);return ak[0].ap;}}}public class bZ{public string ig;public List<string>fU=new List<string>();public Dictionary<string,string>h1=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>hX=new List<string>();}public class io{public List<bt>ct=new List<bt>();public List<E>Z=new List<E>();
public List<string>bd=new List<string>();public List<string>em=new List<string>();public Dictionary<string,List<string>>dU=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class mg{static readonly Dictionary<string,bZ>l2=new Dictionary<string,bZ>(StringComparer.OrdinalIgnoreCase);
static mg(){rC();}static void rC(){bX("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bX("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bX("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bX("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bX("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bX("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bX("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bX("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bX(string o3,string[]ub,string sy){var hA=new bZ{ig=o3}
;foreach(var t4 in ub){var ko=t4.Split(new[]{'='},2);string pe=ih(ko[0]);hA.fU.Add(pe);if(ko.Length>1)hA.h1[pe]=ad(ko[1].Trim());
}var tR=sy.Replace("\r\n","\n").Split('\n');foreach(var tF in tR){string pF=tF.Trim();if(pF.Length>0)hA.hX.Add(pF);}l2[o3]=hA;}static string ih(string dA){
dA=dA.Trim();if(dA.StartsWith("$"))dA=dA.Substring(1);return dA.ToLowerInvariant();}public static io rr(string sN){var au=new io();E d=null;
aE u=null;var pp=rx(sN,au.bd,au.dU);for(int eQ=0;eQ<pp.Count;eQ++){string b=pp[eQ].Trim();
if(b.Length==0)continue;try{if(b.StartsWith("@bind")){au.ct.Add(rs(b));}else if(b.StartsWith("@screen")){d=new E{bf=b.Substring("@screen".Length).Trim()}
;au.Z.Add(d);u=null;}else if(b.StartsWith("profile:")){s(d,"profile:");d.cA=b.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(b.StartsWith("border:")){s(d,"border:");string m3=b.Substring("border:".Length).Trim();if(u!=null)u.dY=m3;
else d.c5=m3;}else if(b.StartsWith("border_color:")){s(d,"border_color:");string m5=b.Substring("border_color:".Length).Trim();
if(u!=null)u.fC=m5;else d.dZ=m5;}else if(b.StartsWith("border_gradient:")||b.StartsWith("border_gradient(")){
s(d,"border_gradient");string cL=b.StartsWith("border_gradient:")? b.Substring("border_gradient:".Length): b.Substring("border_gradient".Length);
cL=cL.Trim();if(cL.StartsWith("(")&&cL.Contains(")"))cL=a1(cL,0);string[]p4=aN(cL).Select(uH=>uH.Trim()).ToArray();if(u!=null)u.fD=p4;
else d.fE=p4;}else if(b.StartsWith("title_align:")){s(d,"title_align:");string qc=b.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(u!=null)u.ix=qc;else d.mC=qc;}else if(b.StartsWith("title_color:")){s(d,"title_color:");string qf=b.Substring("title_color:".Length).Trim();
if(u!=null)u.iy=qf;else d.mD=qf;}else if(b.StartsWith("font_size:")){s(d,"font_size:");float p9=float.Parse(b.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(u!=null)u.ma=p9;else d.c7=p9;}else if(b.StartsWith("font:")){s(d,"font:");string nX=b.Substring("font:".Length).Trim();
if(u!=null)u.l8=nX;else d.l9=nX;}else if(b.StartsWith("bg:")||b.StartsWith("background:")){s(d,"bg:");string dg=b.Substring(b.IndexOf(':')+1).Trim();
bP i6=u!=null ?(bP)u : d;if(dg.StartsWith("color(")&&dg.EndsWith(")")){string nd;i6.a_=mq(dg.Substring(6,dg.Length-7),out nd);
string nb,nc;mx(nd,out nb,out nc);i6.hT=nb;i6.hU=nc;}else if(u!=null)u.c3=dg;
else d.bO=dg;}else if(b.StartsWith("layout:")){s(d,"layout:");rt(b.Substring("layout:".Length).Trim(),d);}else if(b.StartsWith("alert_period:")){
s(d,"alert_period:");d.hQ=Math.Max(0.2f,float.Parse(b.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(b.StartsWith("refresh:")){s(d,"refresh:");d.ip=int.Parse(b.Substring("refresh:".Length).Trim());}else if(b.StartsWith("output:")){
s(d,"output:");var g7=aN(b.Substring("output:".Length).Trim());if(g7.Count==0)throw new FormatException("output: requires a target block name");
d.bU=ad(g7[0]);d.cz=g7.Count>1 ? int.Parse(g7[1]): 0;}else if(b.StartsWith("width:")){s(d,"width:");
d.mF=int.Parse(b.Substring("width:".Length).Trim());}else if(b.StartsWith("boot_msg:")||b.StartsWith("boot:")){s(d,"boot_msg:");
d.l0=ad(b.Substring(b.IndexOf(':')+1).Trim());}else if(b.StartsWith("boot_stagger:")){s(d,"boot_stagger:");d.l1=int.Parse(b.Substring("boot_stagger:".Length).Trim());
}else if(b.StartsWith("boot_calibrate:")){s(d,"boot_calibrate:");string nD=b.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.hY=nD=="off" ? 0f : Math.Max(0f,float.Parse(nD,System.Globalization.CultureInfo.InvariantCulture));}else if(b.StartsWith("boot_batch:")){s(d,"boot_batch:");
d.lZ=Math.Max(1,int.Parse(b.Substring("boot_batch:".Length).Trim()));}else if(b.StartsWith("boot_duration:")){s(d,"boot_duration:");
string eD=b.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool ow=eD.EndsWith("s");if(ow)eD=eD.Substring(0,eD.Length-1).Trim();
float jA;if(float.TryParse(eD,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out jA)){d.hZ=ow ? jA : jA/60f;
}}else if(b.StartsWith("boot_log:")){s(d,"boot_log:");d.fB.Add(ad(b.Substring("boot_log:".Length).Trim()));}else if(b.StartsWith("mirror_panel:")){
s(d,"mirror_panel:");d.ie=ad(b.Substring("mirror_panel:".Length).Trim());}else if(b.StartsWith("mirror:")){s(d,"mirror:");
d.id=ad(b.Substring("mirror:".Length).Trim());}else if(b.StartsWith("banner:")){s(d,"banner:");d.cr=ad(b.Substring("banner:".Length).Trim());
}else if(b.StartsWith("boot_logo:")){s(d,"boot_logo:");d.cu=ad(b.Substring("boot_logo:".Length).Trim());}else if(b.StartsWith("boot_gradient:")||b.StartsWith("boot_gradient(")||b.StartsWith("banner_gradient:")||b.StartsWith("banner_gradient(")){
s(d,"boot_gradient");d.fA=mr(b);}else if(b.StartsWith("bg_gradient:")||b.StartsWith("bg_gradient(")){s(d,"bg_gradient");
(u!=null ?(bP)u : d).bQ=mr(b);}else if(b.StartsWith("bg_gradient_dir:")){s(d,"bg_gradient_dir:");
(u!=null ?(bP)u : d).dW=b.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(b.StartsWith("flash:")){
s(d,"flash:");bP jI=u!=null ?(bP)u : d;string jH=b.Substring("flash:".Length).Trim();string nZ=mz(jH);
string nY,n_;mx(jH.Substring(0,jH.Length-nZ.Length),out nY,out n_);jI.h5=nY;jI.h7=n_;jI.c6=new List<a0>{
new a0{h2=nZ,cw="on"}};}else if(b.StartsWith("bg_texture:")){s(d,"bg_texture:");d.hV=ad(b.Substring("bg_texture:".Length).Trim());
}else if(b.StartsWith("bg_texture_color:")){s(d,"bg_texture_color:");d.lY=b.Substring("bg_texture_color:".Length).Trim();
}else if(b.StartsWith("gradient_mode:")){s(d,"gradient_mode:");d.fM=b.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(b.StartsWith("screen_bg:")){s(d,"screen_bg:");d.bO=b.Substring("screen_bg:".Length).Trim();}else if(b.StartsWith("panel_bg:")){
s(d,"panel_bg:");d.il=b.Substring("panel_bg:".Length).Trim();}else if(b.StartsWith("alert_bg:")){s(d,"alert_bg:");
d.lW=b.Substring("alert_bg:".Length).Trim();}else if(b.StartsWith("boot_color:")||b.StartsWith("banner_color:")){s(d,"boot_color:");
d.l_=b.Substring(b.IndexOf(':')+1).Trim();}else if(b.StartsWith("orientation:")){s(d,"orientation:");string t1=b.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(u!=null)u.ij=t1;}else if(b.StartsWith("@panel")){s(d,"@panel");u=rv(b);d.ak.Add(u);
}else if(b.StartsWith("title:")){s(d,"title:");if(u==null){u=new aE{db="main",d0=0,ef=0,eh=d.d6,ei=d.d7}
;d.ak.Add(u);}u.cE=ad(b.Substring("title:".Length).Trim());}else if(b.StartsWith("line:")){s(d,"line:");
if(u==null){u=new aE{db="main",d0=0,ef=0,eh=d.d6,ei=d.d7};d.ak.Add(u);}u.ap.Add(ru(b.Substring("line:".Length).Trim()));
}else{au.bd.Add($"Line {eQ + 1}: unrecognized directive: \"{b}\"");}}catch(Exception sZ){au.bd.Add($"Line {eQ + 1}: parse error ({sZ.Message}) in \"{b}\"");
}}foreach(var pG in au.Z){if(string.IsNullOrEmpty(pG.bU))au.bd.Add($"@screen {pG.bf}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aw in au.Z){int kL=aw.ak.Count(k_=>k_.c3!=null||k_.a_!=null||k_.bQ!=null);if(aw.ak.Count>1&&kL>0&&kL<aw.ak.Count)au.em.Add("screen '"+aw.bf+"': only "+kL+" of "+aw.ak.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(aw.cr!=null&& !au.dU.ContainsKey(aw.cr))au.bd.Add("Screen '"+aw.bf+"': banner '"+aw.cr+"' is not defined (@banner "+aw.cr+" ... @end)");
if(aw.cu!=null&& !au.dU.ContainsKey(aw.cu))au.bd.Add("Screen '"+aw.bf+"': boot_logo '"+aw.cu+"' is not defined (@banner "+aw.cu+" ... @end)");
}return au;}static void rt(string jd,E pN){int kS=jd.IndexOf('(');if(kS<0)throw new FormatException("missing '(' in layout: specification");
string oE=jd.Substring(0,kS).Trim().ToLowerInvariant();string sb=a1(jd,kS);var iX=aN(sb);if(oE=="grid"){if(iX.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
pN.d6=Math.Max(1,int.Parse(iX[0]));pN.d7=Math.Max(1,int.Parse(iX[1]));}else{throw new FormatException($"unknown layout kind '{oE}' (expected grid)");
}}static aE rv(string tN){string fc=tN.Substring("@panel".Length).Trim();int gd=fc.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(gd<0)throw new FormatException("@panel requires an at(...) positioning clause");
string tZ=fc.Substring(0,gd).Trim();string mZ=fc.Substring(gd);int pk=mZ.IndexOf('(');string mX=a1(mZ,pk);
var cJ=aN(mX);if(cJ.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bI=new aE{db=tZ,d0=int.Parse(cJ[0]),ef=int.Parse(cJ[1]),eh=cJ.Count>2 ? int.Parse(cJ[2]): 1,ei=cJ.Count>3 ? int.Parse(cJ[3]): 1}
;int mQ=gd+pk+mX.Length+1;if(mQ<fc.Length){string aV=fc.Substring(mQ).Trim();int qk=aV.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(qk>=0){string fk=aV.Substring(qk+5).Trim();if(fk.StartsWith("(")&&fk.Contains(")"))bI.cE=ad(a1(fk,0));else if(fk.StartsWith(":")){
string cM=fk.Substring(1).Trim();if(cM.StartsWith("\"")){int nT=cM.IndexOf('"',1);bI.cE=nT>0 ? cM.Substring(1,nT-1): ad(cM);}else{int p_=cM.IndexOf(' ');
bI.cE=p_>=0 ? cM.Substring(0,p_): cM;}}}string o6=fJ(aV,"orientation:");if(o6!=null)bI.ij=o6.ToLowerInvariant();string mO=fJ(aV,"title_align:");
if(mO!=null)bI.ix=mO.ToLowerInvariant();string qh=fJ(aV,"title_color:");if(qh!=null)bI.iy=qh;int gj=aV.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(gj>=0&& !aV.Substring(gj).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aV.Substring(gj).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bI.dY=fJ(aV.Substring(gj),"border:");}int gg=aV.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(gg<0)gg=aV.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(gg>=0){string ca=aV.Substring(gg+"border_gradient".Length).Trim();if(ca.StartsWith(":"))ca=ca.Substring(1).Trim();if(ca.StartsWith("(")&&ca.Contains(")"))ca=a1(ca,0);
bI.fD=aN(ca).Select(uI=>uI.Trim()).ToArray();}}return bI;}static string fJ(string pH,string oD){int ov=pH.IndexOf(oD,StringComparison.OrdinalIgnoreCase);
if(ov<0)return null;string lN=pH.Substring(ov+oD.Length).Trim();int p0=lN.IndexOf(' ');return p0>=0 ? lN.Substring(0,p0): lN;}static string[]mr(string ku){
int jU=ku.IndexOfAny(new[]{':','('});string eL=ku.Substring(ku[jU]==':' ? jU+1 : jU).Trim();if(eL.StartsWith("(")&&eL.Contains(")"))eL=a1(eL,0);return aN(eL).Select(uJ=>uJ.Trim()).ToArray();
}static List<string>rx(string sO,List<string>jF,Dictionary<string,List<string>>sq){var qr=new Dictionary<string,bZ>(StringComparer.OrdinalIgnoreCase);
var dG=(sO ?? "").Replace("\r\n","\n").Split('\n');var kK=new List<string>();int dv=0;while(dv<dG.Length){string k4=dG[dv++];if(k4.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ar=new List<string>();while(dv<dG.Length){string mM=dG[dv++].Replace("\t","    ").TrimEnd();if(mM.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ar.Add(mM);}while(ar.Count>0&&ar[0].Length==0)ar.RemoveAt(0);while(ar.Count>0&&ar[ar.Count-1].Length==0)ar.RemoveAt(ar.Count-1);int kd=int.MaxValue;
foreach(var iO in ar)if(iO.Length>0)kd=Math.Min(kd,iO.Length-iO.TrimStart().Length);for(int eX=0;eX<ar.Count;eX++)if(ar[eX].Length>0)ar[eX]=ar[eX].Substring(kd);
sq[k4.TrimStart().Substring(8).Trim()]=ar;continue;}string cf=my(k4).Trim();if(cf.Length==0)continue;if(cf.StartsWith("@template")){try{
var lB=rw(cf);while(dv<dG.Length){string sA=dG[dv++];string je=my(sA).Trim();if(je.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(je.Length>0)lB.hX.Add(je);}qr[lB.ig]=lB;}catch(Exception s_){jF.Add($"Template parse error ({s_.Message}) in \"{cf}\"");}}
else if(cf.StartsWith("@use")){try{l5(cf,qr,kK,jF);}catch(Exception s0){jF.Add($"@use error ({s0.Message}) in \"{cf}\"");
}}else{kK.Add(cf);}}return kK;}static bZ rw(string tO){string k5=tO.Substring("@template".Length).Trim();int kT=k5.IndexOf('(');
if(kT<0)throw new FormatException("missing '(' in @template header");string t_=k5.Substring(0,kT).Trim();string sc=a1(k5,kT);var lC=new bZ{
ig=t_};foreach(var t5 in aN(sc)){var kp=t5.Split(new[]{'='},2);string pf=ih(kp[0]);lC.fU.Add(pf);if(kp.Length>1)lC.h1[pf]=ad(kp[1].Trim());
}return lC;}static void l5(string tP,Dictionary<string,bZ>qs,List<string>pb,List<string>sY,int nH=0){string hi=tP.Substring("@use".Length).Trim();
int g5=hi.IndexOf('(');string kC=g5>=0 ? hi.Substring(0,g5).Trim(): hi.Trim();string sd=g5>=0 ? a1(hi,g5): "";bZ dP=null;
if(!qs.TryGetValue(kC,out dP)){if(!l2.TryGetValue(kC,out dP))throw new FormatException($"unknown template '{kC}'");}var gm=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var oI in dP.h1)gm[oI.Key]=oI.Value;var mW=aN(sd);for(int ev=0;ev<mW.Count;ev++){string gb=mW[ev];int jD=gb.IndexOf('=');if(jD>=0){
string tx=ih(gb.Substring(0,jD));string vn=ad(gb.Substring(jD+1).Trim());gm[tx]=vn;}else{if(ev<dP.fU.Count){string ty=dP.fU[ev];
gm[ty]=ad(gb.Trim());}}}foreach(var sz in dP.hX){string eH=sz;foreach(var gX in gm){eH=eH.Replace("$"+gX.Key,gX.Value).Replace("{"+gX.Key+"}",gX.Value);
}if(eH.StartsWith("@use")){if(nH>3)throw new FormatException("@use nested too deep");l5(eH,qs,pb,sY,nH+1);
}else pb.Add(eH);}}static void s(E sM,string sV){if(sM==null)throw new FormatException($"'{sV}' used before any @screen declared");
}static string my(string bl){if(string.IsNullOrEmpty(bl))return bl;bool kc=false;for(int cd=0;cd<bl.Length;cd++){char ji=bl[cd];if(ji=='"'){kc= !kc;
}else if(!kc){if(ji=='/'&&cd+1<bl.Length&&bl[cd+1]=='/'){return bl.Substring(0,cd);}if(ji=='#'){if(rj(bl,cd)){continue;}return bl.Substring(0,cd);}}}return bl;
}static bool rj(string cS,int oy){int dy=0;for(int j8=oy+1;j8<cS.Length;j8++){char dk=cS[j8];if((dk>='0'&&dk<='9')||(dk>='a'&&dk<='f')||(dk>='A'&&dk<='F'))dy++;
else break;}if(dy==3||dy==4||dy==6||dy==8){int e_=oy+1+dy;if(e_>=cS.Length||char.IsWhiteSpace(cS[e_])||cS[e_]==')'||cS[e_]==','||cS[e_]==':')return true;
}return false;}static bt rs(string tQ){string k6=tQ.Substring("@bind".Length).Trim();int jE=k6.IndexOf('=');if(jE<0)throw new FormatException("missing '=' in @bind");
string o4=k6.Substring(0,jE).Trim();string jG=k6.Substring(jE+1).Trim();int kU=jG.IndexOf('(');if(kU<0)throw new FormatException("missing '(' in @bind target");
string gG=jG.Substring(0,kU).Trim().ToLowerInvariant();string se=a1(jG,kU);var b4=aN(se);if(b4.Count==0)throw new FormatException("@bind target has no arguments");
var av=new bt{af=o4};if(gG=="dynamic"||gG=="var"||gG=="mem"){av.bT=bb.d3;av.bv=b4.Count>0 ? ad(b4[0].Trim()): o4;
for(int iP=0;iP<b4.Count;iP++){var gY=b4[iP].Split(new[]{':'},2);if(gY.Length==2&&gY[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double nG;
if(double.TryParse(gY[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nG))av.fG=nG;else av.h0=ad(gY[1].Trim());
}}return av;}av.bT=gG=="group" ? bb.h9 : bb.qS;string f9=null;for(int f8=0;f8<b4.Count;f8++){var dx=b4[f8].Split(new[]{':'},2);if(dx.Length==2&&dx[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
f9=dx[1].Trim().ToLowerInvariant();}else if(dx.Length==2&&dx[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){av.fx=dx[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{av.a2.Add(ad(b4[f8].Trim()));}}if(av.a2.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(av.bT==bb.h9){if(f9==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(f9){case "avg": av.dT=aJ.hR;break;case "sum": av.dT=aJ.mA;break;case "min": av.dT=aJ.mm;break;case "max": av.dT=aJ.ml;
break;default: throw new FormatException($"unknown agg '{f9}' (expected avg/sum/min/max)");}}return av;}static bw ru(string gi){var m=new bw();
if(!gi.StartsWith("\""))throw new FormatException("line must start with a quoted label");int jC=gi.IndexOf('"',1);if(jC<0)throw new FormatException("unterminated quoted label");
m.ab=gi.Substring(1,jC-1);string fb=gi.Substring(jC+1).Trim();int g6=fb.IndexOf('(');if(g6<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string y=fb.Substring(0,g6).Trim().ToLowerInvariant();string sa=a1(fb,g6);string ah=fb.Substring(fb.IndexOf(')',g6)+1).Trim();
var jy=sa.Trim().Split('.');if(jy.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
m.aK=jy[0].Trim();m.I=jy[1].Trim();if(y=="bar")m.h=e.dV;else if(y=="led")m.h=e.ec;else if(y=="spark"||y=="sparkline")m.h=e.cB;
else if(y=="hazard"||y=="stripes")m.h=e.d8;else if(y=="chevron"||y=="chevrons")m.h=e.d_;else if(y=="dots"||y=="dot")m.h=e.d2;
else if(y=="balance"||y=="horizon"||y=="centered")m.h=e.c4;else if(y=="toggle"||y=="switch")m.h=e.cF;else if(y=="checkbox"||y=="check")m.h=e.cv;
else if(y=="status"||y=="badge"||y=="state")m.h=e.cD;else if(y=="ammo"||y=="pips")m.h=e.c2;else if(y=="flow"||y=="rate"||y=="spinner")m.h=e.cx;
else if(y=="text")m.h=e.iv;else if(y=="tank"||y=="tube"||y=="bucket")m.h=e.fY;else if(y=="dial"||y=="gauge"||y=="arc")m.h=e.fH;
else throw new FormatException($"unknown display function '{y}'");while(ah.Length>0){int jv=ah.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int ds=ah.IndexOf('(');if(jv>=0&&(ds<0||jv<ds)){string gt=ah.Substring(jv+4).Trim();int hr=gt.IndexOf(' ');m.fI=(hr>=0 ? gt.Substring(0,hr): gt).ToLowerInvariant();
ah=(hr>=0 ? gt.Substring(hr): "").Trim();continue;}int lL=ah.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(lL>=0&&(ds<0||lL<ds)){
string hL=ah.Substring(lL+5).Trim();int hs=hL.IndexOf(' ');m.ek=(hs>=0 ? hL.Substring(0,hs): hL).Trim().Trim('"');ah=(hs>=0 ? hL.Substring(hs): "").Trim();
continue;}int jP=ah.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(jP>=0&&(ds<0||jP<ds)){string gA=ah.Substring(jP+7).Trim();
int ht=gA.IndexOf(' ');m.d4=(ht>=0 ? gA.Substring(0,ht): gA).Trim().Trim('"');ah=(ht>=0 ? gA.Substring(ht): "").Trim();continue;}int g0=ah.IndexOf('(');
if(g0<0)break;string S=ah.Substring(0,g0).Trim().ToLowerInvariant();int oP=S.LastIndexOf(' ');if(oP>=0)S=S.Substring(oP+1).Trim();
string ai=a1(ah,g0);ah=ah.Substring(ah.IndexOf(')',g0)+1).Trim();if(S=="gradient"){m.bS=true;m.aL=aN(ai).Select(uK=>uK.Trim()).ToArray();
if(m.aL.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(S=="size"){string lA=ai.Trim().ToLowerInvariant();
m.fX=lA=="large"||lA=="fill" ? 2 : lA=="medium" ? 1 : 0;}else if(S=="marker"){m.fS=new List<double>();foreach(var tW in aN(ai)){double o_;
if(!double.TryParse(tW,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out o_))throw new FormatException("marker() takes numbers from 0 to 100");
m.fS.Add(o_);}}else if(S=="ticks")m.iw=int.Parse(ai.Trim());else if(S=="segments"||S=="points")m.bz=Math.Max(2,Math.Min(40,int.Parse(ai.Trim())));
else if(S=="outline")m.ik=ai.Trim();else if(S=="color_by"){var jm=ai.Trim().Split('.');if(jm.Length!=2)throw new FormatException("color_by() takes bindname.field");
m.d1=jm[0].Trim();m.h_=jm[1].Trim();}else if(S=="label_color")m.ea=ai.Trim();else if(S=="track")m.iz=ai.Trim();else if(S=="flat"){
m.ax=ai.Trim();}else if(S=="pattern"){string W=ai.Trim().ToLowerInvariant();if(W=="hazard"||W=="stripes")m.h=e.d8;else if(W=="chevron"||W=="chevrons")m.h=e.d_;
else if(W=="dots"||W=="dot")m.h=e.d2;else if(W=="led")m.h=e.ec;else if(W=="balance"||W=="horizon")m.h=e.c4;
else if(W=="toggle"||W=="switch")m.h=e.cF;else if(W=="checkbox"||W=="check")m.h=e.cv;else if(W=="status"||W=="badge"||W=="state")m.h=e.cD;
else if(W=="ammo"||W=="pips")m.h=e.c2;else if(W=="flow"||W=="rate"||W=="spinner")m.h=e.cx;}else if(S=="color"){string tq;
m.bu=mq(ai,out tq);}else if(S=="dir"||S=="direction"){m.fI=ai.Trim().ToLowerInvariant();}else if(S=="unit"){
m.ek=ai.Trim().Trim('"');}else if(S=="max"){double oZ;if(double.TryParse(ai.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out oZ))m.fT=oZ;
else{var kz=ai.Trim().Split('.');if(kz.Length!=2)throw new FormatException("max() takes a number or bindname.field");m.da=kz[0].Trim();m.ic=kz[1].Trim();
}}else if(S=="format"||S=="fmt"){m.d4=ai.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{S}'");}}return m;}static string a1(string hn,int kI){
int ju=0;for(int eR=kI;eR<hn.Length;eR++){if(hn[eR]=='(')ju++;else if(hn[eR]==')'){ju--;if(ju==0)return hn.Substring(kI+1,eR-kI-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aN(string sf){return sf.Split(',').Select(uL=>uL.Trim()).Where(uM=>uM.Length>0).ToList();
}static List<a0>mq(string r9,out string jK){var hl=new List<a0>();jK=null;foreach(var kV in aN(r9)){int jq=kV.LastIndexOf(':');
if(jq<0)throw new FormatException("color() entries need 'expr : color'");string gZ=kV.Substring(0,jq).Trim();string nz=kV.Substring(jq+1).Trim();
if(gZ.Equals("default",StringComparison.OrdinalIgnoreCase)){hl.Add(new a0{c8=true,cw=nz});}else{string p5=mz(gZ);
if(jK==null)jK=gZ.Substring(0,gZ.Length-p5.Length).Trim();hl.Add(new a0{h2=p5,cw=nz});}}if(!hl.Any(uy=>uy.c8))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return hl;}static void mx(string uz,out string sw,out string s3){var e2=(uz ?? "").Split('.');if(e2.Length!=2||e2[0].Trim().Length==0||e2[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
sw=e2[0].Trim();s3=e2[1].Trim();}static string mz(string dr){dr=dr.Trim();foreach(var t2 in new[]{">=","<=","==",">","<"}){int j9=dr.IndexOf(t2,StringComparison.Ordinal);
if(j9>0)return dr.Substring(j9).Trim();if(j9==0)return dr;}throw new FormatException($"color() rule has no recognizable comparison operator: '{dr}'");}
static string ad(string cT){if(cT.Length>=2&&cT[0]=='"'&&cT[cT.Length-1]=='"')return cT.Substring(1,cT.Length-2);return cT;}}public static class qQ{public static double qP(aJ r5,List<double>dR){
if(dR==null||dR.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(r5){case aJ.hR: return dR.Average();case aJ.mA: return dR.Sum();
case aJ.mm: return dR.Min();case aJ.ml: return dR.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class M{
public static string rG(string tH,double g8,int m4=10,string qA=null){g8=Math.Max(0,Math.Min(100,g8));int n4=(int)Math.Round(m4*(g8/100.0));
string sr=new string('#',n4)+new string('-',m4-n4);string vo=qA!=null ? qA : $"{g8:0}%";return $"{tH} [{sr}] {vo}";
}public static string mb(double cq,string lK=null,string gF=null){if(lK=="auto"){double iQ=Math.Abs(cq);if(iQ>=1e9)return(cq/1e9).ToString("F1")+"G";
if(iQ>=1e6)return(cq/1e6).ToString("F1")+"M";if(iQ>=1e3)return(cq/1e3).ToString("F1")+"k";string s1= !string.IsNullOrEmpty(gF)? gF : "F0";return cq.ToString(s1);
}string n6= !string.IsNullOrEmpty(gF)? gF :(Math.Abs(cq)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(lK)){return cq.ToString(n6);}return cq.ToString(n6)+lK;
}public static float rf(string uo,float p7,float ly,float gw=1.0f){string dB=(uo ?? "standard").ToLowerInvariant();
if(dB=="compact"||(dB=="auto"&&p7<=300&&ly<=300&&ly>160)){return gw*0.75f;}if(dB=="corner"||(dB=="auto"&&ly<=160)){
return gw*0.55f;}if(dB=="wide"||(dB=="auto"&&p7>=1000)){return gw*1.15f;}return gw;}public static string rE(string tI,double g9,int cb=5){
g9=Math.Max(0,Math.Min(100,g9));double gx=(g9-50.0)/50.0;int eY=0;int fe=0;if(gx< -0.01)eY=(int)Math.Round(-gx*cb);
else if(gx>0.01)fe=(int)Math.Round(gx*cb);if(eY>cb)eY=cb;if(fe>cb)fe=cb;string tL=new string('-',cb-eY)+new string('<',eY);
string uF=new string('>',fe)+new string('-',cb-fe);return $"{tI} [{tL}|{uF}] {g9:0}%";}public static string rQ(string oM,double vq){
return vq>=50.0 ? $"{oM} [====O] ON" : $"{oM} [O====] OFF";}public static string rJ(string oN,double vr){return vr>=50.0 ? $"{oN} [X] ACTIVE" : $"{oN} [ ] READY ";
}public static string rP(string kr,double qy){if(qy>=75.0)return $"{kr} [ ONLINE  ]";if(qy>=25.0)return $"{kr} [ STANDBY ]";return $"{kr} [ OFFLINE ]";
}public static string rD(string tJ,double ha,int oU=8){ha=Math.Max(0,Math.Min(100,ha));int n5=(int)Math.Round(oU*(ha/100.0));
string ui=new string('|',n5)+new string('.',oU-n5);return $"{tJ} [{ui}] {ha:0}%";}public static string rK(string ks,double e6,int qj=0){
char[]u_=new[]{'|','/','-','\\'};char pI=u_[(qj>=0 ? qj : 0)%4];if(e6>55.0)return $"{ks} {pI} ▲ CHG {e6:0}%";if(e6<45.0)return $"{ks} {pI} ▼ DSC {e6:0}%";
return $"{ks} ● STBY {e6:0}%";}public static bool rn(string aA,string eU){if(aA==null||eU==null)return false;if(aA=="*"||aA==eU)return true;
if(!aA.Contains("*")&& !aA.Contains("?"))return string.Equals(aA,eU,StringComparison.OrdinalIgnoreCase);int aG=0,eS=0,lp= -1,ky=0;while(eS<eU.Length){
if(aG<aA.Length&&(aA[aG]=='?'||char.ToLowerInvariant(aA[aG])==char.ToLowerInvariant(eU[eS]))){aG++;eS++;}else if(aG<aA.Length&&aA[aG]=='*'){
lp=aG;ky=eS;aG++;}else if(lp!= -1){aG=lp+1;ky++;eS=ky;}else{return false;}}while(aG<aA.Length&&aA[aG]=='*')aG++;return aG==aA.Length;
}public static int qU(double t6,int qo){return Math.Max(0,Math.Min(qo,1+(int)(Math.Max(0.0,t6)*qo)));}public static string fL(string[]dO,double kW){
if(dO==null||dO.Length==0)return "white";kW=Math.Max(0,Math.Min(100,kW));int ka=(int)Math.Floor(kW/100.0*dO.Length);if(ka>=dO.Length)ka=dO.Length-1;
return dO[ka];}public static p md(string[]co,double ue){if(co==null||co.Length==0)return p.fZ;if(co.Length==1)return p.ed(co[0]);
double uC=Math.Max(0.0,Math.Min(1.0,ue/100.0));double pM=uC*(co.Length-1);int kb=(int)Math.Floor(pM);int tp=Math.Min(co.Length-1,kb+1);
double lt=pM-kb;p dl=p.ed(co[kb]);p jj=p.ed(co[tp]);byte uA=(byte)Math.Round(dl.bW+(jj.bW-dl.bW)*lt);
byte ti=(byte)Math.Round(dl.bR+(jj.bR-dl.bR)*lt);byte sp=(byte)Math.Round(dl.bN+(jj.bN-dl.bN)*lt);return new p(uA,ti,sp);}public static string ee(List<a0>pE,double vs){
foreach(var k7 in pE){if(k7.c8)continue;if(rb(k7.h2,vs))return k7.cw;}var nF=pE.FirstOrDefault(uB=>uB.c8);return nF!=null ? nF.cw : null;
}static bool rb(string eI,double fq){eI=eI.Trim();foreach(var kH in new[]{">=","<=","==",">","<"}){if(eI.StartsWith(kH)){double fd=double.Parse(eI.Substring(kH.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(kH){case ">=": return fq>=fd;case "<=": return fq<=fd;case "==": return Math.Abs(fq-fd)<1e-9;case ">": return fq>fd;case "<": return fq<fd;
}}}throw new FormatException($"unsupported comparison expression: '{eI}'");}public static List<string>rI(List<string>sL,int dS){
var g2=new List<string>();g2.Add("+"+new string('-',dS+2)+"+");foreach(var kv in sL){string vi=kv.Length>dS ? kv.Substring(0,dS): kv;
string t9=vi.PadRight(dS);g2.Add("| "+t9+" |");}g2.Add("+"+new string('-',dS+2)+"+");return g2;}public struct mp{public float mH,mI,mG,mf;
}public static mp q_(int nv,int pA,int hu,int hv,int fm,int fn,float u9,float u8,float pg,float ph,float jW,float jX){
fm=Math.Max(1,fm);fn=Math.Max(1,fn);hu=Math.Max(1,Math.Min(fm-nv,hu));hv=Math.Max(1,Math.Min(fn-pA,hv));
float si=u9-(pg*2f);float sh=u8-(ph*2f);float nq=(si-(jW*(fm-1)))/fm;float np=(sh-(jX*(fn-1)))/fn;
float uq=pg+nv*(nq+jW);float ur=ph+pA*(np+jX);float up=hu*nq+((hu-1)*jW);float ug=hv*np+((hv-1)*jX);return new mp{
mH=uq,mI=ur,mG=up,mf=ug};}public struct mu{public int rq;public int ib;public int ag;public int aD;public int ej;
public int bc;public int rY;public int rZ;public int q8;public int q9;
public double ra;}public static mu q0(E cm){var l=new mu();var pj=cm.ak!=null&&cm.ak.Count>0 ? cm.ak : new List<aE>{
new aE{ap=cm.fR}};l.rq=pj.Count;l.ib=0;l.ag=0;l.aD=0;if(!string.IsNullOrEmpty(cm.bO)&& !cm.bO.Equals("none",StringComparison.OrdinalIgnoreCase))l.ag++;
if(!string.IsNullOrEmpty(cm.c5)&& !cm.c5.Equals("none",StringComparison.OrdinalIgnoreCase))l.ag+=2;foreach(var e3 in pj){
if(!string.IsNullOrEmpty(e3.c3)&& !e3.c3.Equals("none",StringComparison.OrdinalIgnoreCase))l.ag++;if(!string.IsNullOrEmpty(e3.cE))l.ag+=2;
l.ib+=e3.ap.Count;foreach(var tG in e3.ap){l.rY++;l.rZ++;l.ag+=2;switch(tG.h){case e.dV: l.ej++;
l.bc++;l.ag+=2;l.aD++;break;case e.ec: l.bc+=10;l.ag+=10;
l.aD+=10;break;case e.d8: l.bc+=10;l.ag+=11;l.aD+=10;break;case e.d_: l.bc+=10;
l.ag+=10;l.aD+=10;break;case e.d2: l.bc+=12;l.ag+=12;l.aD+=12;
break;case e.cB: l.ej++;l.bc+=8;l.ag+=9;l.aD+=8;break;case e.c4: l.ej+=2;
l.bc++;l.ag+=3;l.aD++;break;case e.cF: case e.cv: l.ej++;
l.bc++;l.ag+=2;l.aD++;break;case e.cD: case e.cx: l.ej++;l.bc+=2;
l.ag+=3;l.aD+=2;break;case e.c2: l.bc+=8;l.ag+=8;l.aD+=8;
break;default: l.aD++;break;}}}l.q8=25+(l.ib*2);l.q9=180+(l.aD*4);
l.ra=0.0;return l;}}