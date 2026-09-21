hn q;string hC=null;Dictionary<string,List<IMyTerminalBlock>>cj=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>ff=new Dictionary<string,bool>();
Dictionary<string,be>dJ=new Dictionary<string,be>();Dictionary<string,Color>lu=new Dictionary<string,Color>();List<IMyTerminalBlock>cg=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>cK=new List<IMyTerminalBlock>();bool fc;int hz;string dK="";StringBuilder dO=new StringBuilder();readonly Dictionary<string,double>aF=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>dL=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener dM;bool ch=false;
Dictionary<D,IMyTextSurface>bE=new Dictionary<D,IMyTextSurface>();Dictionary<D,string>ci=new Dictionary<D,string>();
static readonly string[]lb=new string[101];static Program(){for(int fP=0;fP<=100;fP++){lb[fP]=fP+"%";}}static string kY(double tH){int eg=(int)Math.Round(tH);
if(eg<0)eg=0;else if(eg>100)eg=100;return lb[eg];}class aP{public List<MySprite>f=new List<MySprite>();public List<b8>dD=new List<b8>();
public int b1= -1;public Color[]eR;public Color eN;public bool eO;public Vector2 k7;public bool k4=false;public List<eS>dn=new List<eS>();
}class eS{public int e0;public Color gW,eW;public List<aN>e8,eZ;public string g_,g8,g9,hb;public bool k5,hd;
}class b8{public bx e3;public int U= -1;public int aS= -1;public int cH= -1;public bool k0=false;
public List<int>K=new List<int>();public float[]e9=new float[8];public float b0;public float bd;public float aY;
public float b_;public double e2= -99999.0;public string k8;public float C= -1f;public bool lp;public float kO=12f;}Dictionary<D,aP>dQ=new Dictionary<D,aP>();
int lw=0;double hE=0;double dP=0;double ad=0;int dN=0;Dictionary<D,double>bi=new Dictionary<D,double>();
Dictionary<D,double>hy=new Dictionary<D,double>();Dictionary<string,int>dT=new Dictionary<string,int>();List<string>cL=new List<string>();
Dictionary<string,double>dS=new Dictionary<string,double>();List<MyInventoryItem>hB=new List<MyInventoryItem>();int bF= -1,fh;double hF;
Dictionary<D,double>fd=new Dictionary<D,double>();int dR=0;const string pG="0.11.0";IMyTextSurface aT;bool hx=true;
double fe=0;string lv="";string hD="";int fg=0,hA=0;List<KeyValuePair<string,int>>cJ=new List<KeyValuePair<string,int>>();
const double pY=120;const float pb=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{dM=IGC.RegisterBroadcastListener("HELM");
dM.SetMessageCallback("HELM_IGC");}catch{}e7();hr();k3();}public void Main(string lI,UpdateType tD){
double my=Runtime.TimeSinceLastRun.TotalSeconds;ad+=my;if((tD&UpdateType.IGC)!=0||lI=="HELM_IGC"){pW();}string n4=(lI ?? "").Trim();
string al=n4.ToLowerInvariant();if(al=="reload"){e7();}else if(al=="refresh"){hp();}else if(al=="boot"){hr();}else if(al=="probe"){
dP=ad+20.0;pv();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(al=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=qh;
e7();hr();}else if(al=="console on"||al=="console off"){hx=al=="console on";k3();fe=0;}else if(al=="scan"||al=="diagnose"||al=="test"){
qf();return;}else if(al.StartsWith("set ")){string gn=n4.Substring(4).Trim();int mC=gn.IndexOf('=');int te=gn.IndexOf(' ');int j6=mC>0 ? mC : te;
if(j6>0){string nh=gn.Substring(0,j6).Trim();string oT=gn.Substring(j6+1).Trim();kG($"{nh}={oT}");Echo($"[HELM] Set dynamic variable '{nh}' = {oT}");
}return;}else if(al.StartsWith("alert")){string eJ=al.Length>5 ? al.Substring(5).Trim(): "toggle";if(eJ=="toggle")ch= !ch;else ch=eJ=="red"||eJ=="on"||eJ=="1"||eJ=="true";
Echo($"[HELM] Red alert state: {ch}");return;}else if(Me.CustomData!=hC){e7();}dR++;hE+=my;if(hE>=pY){
hE=0;hp();}if(dP>0){if(ad<dP){hw();return;}dP=0;dQ.Clear();ci.Clear();
fe=0;}if(q==null||q.W.Count==0){hw();kQ();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cu=q.W[lw%q.W.Count];
lw++;hD=cu.bg;bool hJ=bi.Count>0;if(hJ)p4();double mz;if(!bi.ContainsKey(cu)&& !(hy.TryGetValue(cu,out mz)&&ad<mz)){
if(p8(cu))hJ=true;if(cu.ho>0)hy[cu]=ad+cu.ho/60.0;}dN=hJ ? 0 : dN+1;Runtime.UpdateFrequency=dN<=q.W.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
qg();hw();kQ();fg=Runtime.CurrentInstructionCount;if(fg>hA)hA=fg;}void kQ(){if(dK.Length>0)Echo(dK);
if(dO.Length>0)Echo(dO.ToString());}List<IMyTerminalBlock>eP(bool qr=false){if(!fc){cK.Clear();GridTerminalSystem.GetBlocks(cK);
cg.Clear();cg.AddRange(cK);cg.RemoveAll(qD=> !qD.IsSameConstructAs(Me));hz=cK.Count-cg.Count;fc=true;
}return qr ? cK : cg;}IMyTerminalBlock kT(string nE,bool qt=false){var hS=GridTerminalSystem.GetBlockWithName(nE);if(hS==null||qt||hS.IsSameConstructAs(Me))return hS;
var hL=eP();for(int fQ=0;fQ<hL.Count;fQ++)if(hL[fQ].CustomName.IndexOf(nE,StringComparison.OrdinalIgnoreCase)>=0)return hL[fQ];return null;}void pm(string nF,List<IMyTerminalBlock>ei,bool lB=false){
var mU=GridTerminalSystem.GetBlockGroupWithName(nF);if(mU==null)return;int lT=ei.Count;var eF=new List<IMyTerminalBlock>();mU.GetBlocks(eF);kE(eF,ei,lB);
if(ei.Count>lT)return;var m4=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(m4);foreach(var o6 in m4){if(o6.Name.IndexOf(nF,StringComparison.OrdinalIgnoreCase)<0)continue;
eF.Clear();o6.GetBlocks(eF);kE(eF,ei,lB);if(ei.Count>lT)return;}}void kE(List<IMyTerminalBlock>rw,List<IMyTerminalBlock>nf,bool qs=false){
foreach(var hT in rw)if((qs||hT.IsSameConstructAs(Me))&& !nf.Contains(hT))nf.Add(hT);}void e7(){hC=Me.CustomData;q=k2.pP(hC);
dK="";if(q.a0.Count>0){var of=new StringBuilder("HELM config errors:\n");foreach(var rg in q.a0)of.AppendLine(" - "+rg);dK=of.ToString();
}if(q.dI.Count>0){var o5=new StringBuilder("HELM notes:\n");foreach(var tM in q.dI)o5.AppendLine(" - "+tM);dK+=o5.ToString();}dJ.Clear();
foreach(var hU in q.b2)if(!dJ.ContainsKey(hU.ab))dJ[hU.ab]=hU;dQ.Clear();ci.Clear();bi.Clear();hy.Clear();
fd.Clear();pB();hp();}void pW(){try{while(dM!=null&&dM.HasPendingMessage){
var nA=dM.AcceptMessage();if(nA.Data is string){kG((string)nA.Data);}}}catch{}}void kG(string nW){
if(string.IsNullOrEmpty(nW))return;var nT=nW.Split(';');for(int iQ=0;iQ<nT.Length;iQ++){var et=nT[iQ].Trim();if(et.Length==0)continue;int d3=et.IndexOf('=');
if(d3<0)d3=et.IndexOf(':');if(d3>0){string c_=et.Substring(0,d3).Trim();string bc=et.Substring(d3+1).Trim();if(c_.Equals("alert",StringComparison.OrdinalIgnoreCase)){
ch=bc.Equals("red",StringComparison.OrdinalIgnoreCase)||bc=="1"||bc.Equals("true",StringComparison.OrdinalIgnoreCase)||bc.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double nI;dL.Remove(c_);if(double.TryParse(bc,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nI)){
aF[c_]=nI;}else if(bc.Equals("true",StringComparison.OrdinalIgnoreCase)||bc.Equals("on",StringComparison.OrdinalIgnoreCase)){aF[c_]=100.0;
}else if(bc.Equals("false",StringComparison.OrdinalIgnoreCase)||bc.Equals("off",StringComparison.OrdinalIgnoreCase)){aF[c_]=0.0;}else{dL[c_]=bc;
}}}}}const string qh="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void pv(){var hM=new List<IMyTextSurface>();if(aT!=null)hM.Add(aT);foreach(var sj in bE.Values)hM.Add(sj);foreach(var s9 in hM)pu(s9);
}static void bh(List<MySprite>tf,float ra,float rb,float tN,float rD,Color qY){tf.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(ra,rb),Size=new Vector2(tN,rD),Color=qY}
);}void pu(IMyTextSurface j7){Vector2 aI=j7.SurfaceSize,aB=j7.TextureSize,ak=(aB-aI)/2f;var aA=new List<MySprite>();Color fN=new Color(150,150,150);
bh(aA,aB.X/2f,2f,aB.X,4f,fN);bh(aA,aB.X/2f,aB.Y-2f,aB.X,4f,fN);bh(aA,2f,aB.Y/2f,4f,aB.Y,fN);bh(aA,aB.X-2f,aB.Y/2f,4f,aB.Y,fN);
bh(aA,ak.X+aI.X/2f,ak.Y+aI.Y/2f,aI.X,aI.Y,new Color(20,40,90));float aj=32f;bh(aA,ak.X+aj/2f,ak.Y+aj/2f,aj,aj,new Color(255,0,0));bh(aA,ak.X+aI.X-aj/2f,ak.Y+aj/2f,aj,aj,new Color(0,255,0));
bh(aA,ak.X+aj/2f,ak.Y+aI.Y-aj/2f,aj,aj,new Color(255,255,0));bh(aA,ak.X+aI.X-aj/2f,ak.Y+aI.Y-aj/2f,aj,aj,new Color(255,0,255));float kC=ak.Y+aI.Y/2f-30f;
aX(aA,"SURFACE "+aI.X.ToString("0")+"x"+aI.Y.ToString("0"),Color.White,ak.X+44f,kC,0.9f);aX(aA,"TEXTURE "+aB.X.ToString("0")+"x"+aB.Y.ToString("0"),Color.White,ak.X+44f,kC+24f,0.9f);
aX(aA,"OFFSET  "+ak.X.ToString("0")+","+ak.Y.ToString("0"),Color.White,ak.X+44f,kC+48f,0.9f);using(var rq=j7.DrawFrame()){rq.AddRange(aA);}}static bool k6(string ix){
return ix.Length>3&&ix.StartsWith("top")&&char.IsDigit(ix[3]);}void pB(){dT.Clear();cL.Clear();bF= -1;hF=0;foreach(var j2 in q.W){
var ji=new List<List<bx>>();foreach(var sE in j2.ag)ji.Add(sE.at);if(j2.ag.Count==0)ji.Add(j2.e4);foreach(var se in ji)foreach(var f6 in se){
if(!k6(f6.I))continue;int sf=int.Parse(f6.I.Substring(3));int m8;dT.TryGetValue(f6.aM,out m8);dT[f6.aM]=Math.Max(m8,sf);}}cL.AddRange(dT.Keys);
}void qg(){if(cL.Count==0)return;if(bF<0){if(ad<hF)return;bF=0;fh=0;dS.Clear();}while(bF<cL.Count&&Runtime.CurrentInstructionCount<20000){
string h2=cL[bF];List<IMyTerminalBlock>fr;cj.TryGetValue(h2,out fr);if(fr!=null&&fh<fr.Count){var lK=fr[fh++];
for(int iR=0;iR<lK.InventoryCount;iR++){hB.Clear();lK.GetInventory(iR).GetItems(hB);foreach(var fY in hB){string i6=fY.Type.TypeId.Replace("MyObjectBuilder_","");
string ni=i6=="Ore"||i6=="Ingot" ? fY.Type.SubtypeId+" "+i6.ToLowerInvariant(): fY.Type.SubtypeId;double m9;dS.TryGetValue(ni,out m9);dS[ni]=m9+(double)fY.Amount;
}}continue;}var gl=new List<KeyValuePair<string,double>>(dS);gl.Sort((tP,tT)=>tT.Value.CompareTo(tP.Value));for(int c7=1;c7<=dT[h2];c7++)dL[h2+".top"+c7]=c7<=gl.Count ? gl[c7-1].Key+" "+H.kX(gl[c7-1].Value,"auto"): "-";
dS.Clear();bF++;fh=0;}if(bF>=cL.Count){bF= -1;hF=ad+15.0;}}void af(int to,string tw){cJ.Add(new KeyValuePair<string,int>(tw,to));
}static string hv(string jW,int nD){return jW.Length>nD ? jW.Substring(0,nD): jW;}void k3(){var jO=Me as IMyTextSurfaceProvider;if(jO==null||jO.SurfaceCount<1)return;
aT=jO.GetSurface(0);aT.ContentType=hx ? ContentType.SCRIPT : ContentType.NONE;aT.Script="";aT.ScriptBackgroundColor=Color.Black;
}void hw(){if(ad>=fe){fe=ad+(dN>q.W.Count+1 ? 5.0 : 1.0);pj();if(hx&&aT!=null&&dP==0)pt();
}Echo(lv);}void pj(){cJ.Clear();int h5=0,l_=0;foreach(var lL in q.b2){List<IMyTerminalBlock>jc;if(lL.bw==aZ.dt){
h5++;continue;}if(cj.TryGetValue(lL.ab,out jc)){l_+=jc.Count;if(jc.Count>0)h5++;}}int mD=q.a0.Count;af(0,"HELM-[Kernel-(v"+pG+")]");
af(4,"------------------------------");af(1,"krn-cfg");af(2," screens "+q.W.Count+"  binds "+h5+"/"+q.b2.Count+"  errors "+mD);
af(2," alert "+(ch ? "RED" : "off")+"  blocks "+l_+"  docked-ignored "+hz);if(mD>0)af(3," ! "+hv(q.a0[0],40));
else if(q.dI.Count>0)af(3," ? "+hv(q.dI[0],40));af(1,"cfg-dsp");if(q.W.Count==0)af(3," no @screen: run 'starter'");
int tb=0;foreach(var jX in q.W){if(++tb>10){af(2," +"+(q.W.Count-10)+" more");break;}IMyTextSurface j8;string op="--",kd="no block";
if(bE.TryGetValue(jX,out j8)){op=j8.SurfaceSize.X.ToString("0")+"x"+j8.SurfaceSize.Y.ToString("0");kd=bi.ContainsKey(jX)? "boot" : "live";
}af(kd=="no block" ? 3 : 2," "+hv(jX.bg,14).PadRight(14)+" "+op.PadRight(9)+" "+kd);}af(1,"perf");af(2," tick "+dR+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
af(2," instr "+fg+"  peak "+hA+"  max "+Runtime.MaxInstructionCount);af(2," rate "+Runtime.UpdateFrequency+"  idle "+dN+"  up "+(int)ad+"s");
if(hD.Length>0)af(2," last "+hD);var og=new StringBuilder();foreach(var rT in cJ)og.AppendLine(rT.Key);lv=og.ToString();}static Color pp(int tp){
switch(tp){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void pt(){Vector2 cv=aT.SurfaceSize;Vector2 jp=(aT.TextureSize-cv)/2f;int id=30;foreach(var rU in cJ)id=Math.Max(id,rU.Key.Length);
Vector2 ic=aT.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float oh=Math.Min(1f,Math.Min(cv.X*0.94f/(id*ic.X),cv.Y*0.94f/(cJ.Count*ic.Y)));
float tQ=jp.X+cv.X*0.03f;float o8=jp.Y+cv.Y*0.03f;var j9=new List<MySprite>();j9.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jp+cv/2f,Size=cv,Color=new Color(12,12,16)}
);foreach(var nn in cJ){aX(j9,nn.Key,pp(nn.Value),tQ,o8,oh);o8+=ic.Y*oh;}using(var rr=aT.DrawFrame()){rr.AddRange(j9);}}static readonly List<string>pi=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void hr(){bi.Clear();if(q==null)return;for(int fR=0;fR<q.W.Count;fR++){var gr=q.W[fR];if(gr.g2>0f)bi[gr]=ad+(fR/Math.Max(1,gr.kI))*gr.kL/60.0;
}}void p4(){List<D>d2=null;int rC=(bi.Count+3)/4,rM=0;foreach(var f0 in bi){double jx=(ad-f0.Value)/f0.Key.g2;if(jx<0)continue;
if(jx>=1.0){if(d2==null)d2=new List<D>();d2.Add(f0.Key);continue;}if((rM++ +dR)%rC==0)ps(f0.Key,jx);}if(d2==null)return;foreach(var eB in d2){
bi.Remove(eB);if(eB.g1>0f)fd[eB]=ad;dQ.Remove(eB);ci.Remove(eB);}}static void aX(List<MySprite>tg,string tx,Color q_,float tR,float tU,float s6){
var oE=MySprite.CreateText(tx,"Monospace",q_,s6,TextAlignment.LEFT);oE.Position=new Vector2(tR,tU);tg.Add(oE);}static void kD(List<MySprite>th,List<string>fm,D gu,float tS,float tV,float s7,float r4){
string[]tm=gu.eT!=null&&gu.eT.Length>0 ? gu.eT : new[]{gu.kJ};for(int eb=0;eb<fm.Count;
eb++){n h7=H.k_(tm,fm.Count>1 ? 100.0*eb/(fm.Count-1): 0.0);aX(th,fm[eb],new Color(h7.bA,h7.bv,h7.bq),tS,tV+eb*r4,s7);}}bool p2(D c9,IMyTextSurface gB){
List<string>dV;if(ci.ContainsKey(c9)|| !q.dk.TryGetValue(c9.bZ,out dV))return false;ci[c9]="banner";Vector2 cw=gB.SurfaceSize;
Vector2 jq=(gB.TextureSize-cw)/2f;int fx=1;foreach(var rV in dV)fx=Math.Max(fx,rV.Length);Vector2 d0=gB.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float gt=Math.Min(cw.X*0.94f/(fx*d0.X),cw.Y*0.94f/(dV.Count*d0.Y));var ka=new List<MySprite>();ka.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jq+cw/2f,Size=cw,Color=string.IsNullOrEmpty(c9.br)? new Color(10,12,16): F(c9.br)}
);kD(ka,dV,c9,jq.X+(cw.X-fx*d0.X*gt)/2f,jq.Y+(cw.Y-dV.Count*d0.Y*gt)/2f,gt,d0.Y*gt);using(var rs=gB.DrawFrame()){rs.AddRange(ka);
}return true;}void ps(D da,double nN){IMyTextSurface eD;if(!bE.TryGetValue(da,out eD))return;Vector2 bV=eD.SurfaceSize;
Vector2 jr=(eD.TextureSize-bV)/2f;var em=da.pC();string nB=da.kK ?? "HELM TELEMETRY KERNEL ONLINE";bool jj=bV.Y>=200f;
List<string>cM;if(da.b3==null|| !q.dk.TryGetValue(da.b3,out cM))cM=pi;int cT=nB.Length+3;foreach(var rW in em)cT=Math.Max(cT,rW.Length+5);
if(jj)foreach(var rX in cM)cT=Math.Max(cT,rX.Length);int n8=(jj ? cM.Count+1 : 0)+2+em.Count;Vector2 cS=eD.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bS=Math.Min(1f,Math.Min(bV.X*0.94f/(cT*cS.X),bV.Y*0.94f/(n8*cS.Y)));float gS=jr.X+(bV.X-cT*cS.X*bS)/2f;float cA=jr.Y+(bV.Y-n8*cS.Y*bS)/2f;
float f4=cS.Y*bS;var dc=new List<MySprite>();dc.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jr+bV/2f,Size=bV,Color=new Color(10,12,16)}
);if(jj){kD(dc,cM,da,gS,cA,bS,f4);cA+=(cM.Count+1)*f4;}aX(dc,">> "+nB,new Color(34,197,94),gS,cA,bS);cA+=f4*2f;int tc=H.ph(nN,em.Count);
for(int fS=0;fS<tc;fS++){bool nK=fS==em.Count-1&&nN>=0.8;aX(dc,nK ? "[OK]" : "[..]",nK ? new Color(34,197,94): new Color(56,189,248),gS,cA,bS);aX(dc,em[fS],new Color(203,213,225),gS+5f*cS.X*bS,cA,bS);
cA+=f4;}using(var rt=eD.DrawFrame()){rt.AddRange(dc);}}void hp(){cj.Clear();bE.Clear();ff.Clear();
dO.Clear();fc=false;if(q==null)return;foreach(var aa in q.b2){var aU=new List<IMyTerminalBlock>();if(aa.bw==aZ.dt){
string i4= !string.IsNullOrEmpty(aa.bf)? aa.bf : aa.ab;if(!aF.ContainsKey(i4)){aF[i4]=aa.eX;if(aa.g5!=null)dL[i4]=aa.g5;
}cj[aa.ab]=aU;continue;}var oI=aa.aR.Count>0 ? aa.aR : new List<string>{aa.lm};foreach(var cy in oI){
if(string.IsNullOrEmpty(cy))continue;bool ng=cy.Contains("*")||cy.Contains("?");if(aa.bw==aZ.hf||ng){var iL=new List<IMyTerminalBlock>();
pm(cy,iL,aa.eQ);if(iL.Count>0){foreach(var m0 in iL){if(!aU.Contains(m0))aU.Add(m0);}}else{var lD=eP(aa.eQ);
for(int hV=0;hV<lD.Count;hV++){var gJ=lD[hV];if(ng ? H.pL(cy,gJ.CustomName): string.Equals(cy,gJ.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aU.Contains(gJ))aU.Add(gJ);}}}}else{var h_=kT(cy,aa.eQ);if(h_!=null&& !aU.Contains(h_))aU.Add(h_);}}if(aU.Count==0){
qd(aa,oI,aU);if(aU.Count>0)ff[aa.ab]=true;}cj[aa.ab]=aU;}foreach(var bn in q.W){
if(string.IsNullOrEmpty(bn.bz))continue;var ju=kT(bn.bz);if(ju==null){dO.AppendLine($"HELM: output block '{bn.bz}' not found");
continue;}IMyTextSurface cx=null;var jP=ju as IMyTextSurfaceProvider;var ms=ju as IMyTextSurface;if(jP!=null&&bn.b9<jP.SurfaceCount){
cx=jP.GetSurface(bn.b9);}else if(ms!=null&&bn.b9==0){cx=ms;}if(cx==null){dO.AppendLine($"HELM: output block '{bn.bz}' has no surface index {bn.b9}");
continue;}cx.ContentType=ContentType.SCRIPT;cx.Script="";cx.ScriptBackgroundColor=Color.Black;bE[bn]=cx;}cg.Clear();
cK.Clear();}static readonly string[]kS={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool pA(int rN,IMyTerminalBlock u){switch(rN){case 0: return u is IMyBatteryBlock;case 1: return u is IMyReactor;case 2: return u is IMySolarPanel;
case 3: return u is IMyWindTurbine;case 4: return u is IMyGasTank;case 5: return u is IMyAirVent;case 6: return u is IMyCargoContainer;case 7: return u is IMyJumpDrive;
case 8: return u is IMyThrust;case 9: return u is IMyMotorSuspension;case 10: return u is IMyDoor;case 11: return u is IMyShipConnector;case 12: return u is IMyAssembler;
case 13: return u is IMyRefinery;case 14: return u is IMyShipDrill;case 15: return u is IMyShipWelder;case 16: return u is IMyShipGrinder;case 17: return u is IMyGasGenerator;
case 18: return u is IMyLargeTurretBase;case 19: return u is IMyProjector;case 20: return u is IMyGyro;case 21: return u is IMyGravityGeneratorBase;case 22: return u is IMyLandingGear;
case 23: return u is IMyRadioAntenna;case 24: return u is IMyBeacon;case 25: return u is IMyLightingBlock;case 26: return u is IMyPistonBase;case 27: return u is IMyMotorStator;
case 28: return u is IMyWarhead;case 29: return u is IMySensorBlock;case 30: return u is IMyCameraBlock;case 31: return u is IMyParachute;case 32: return u is IMyTurretControlBlock;
}return false;}void qd(be qP,List<string>tu,List<IMyTerminalBlock>h0){string rH=(qP.ab+" "+string.Join(" ",tu)).ToLowerInvariant();
var i8=new List<int>();for(int f_=0;f_<kS.Length;f_++){foreach(var tO in kS[f_].Split(','))if(rH.Contains(tO)){i8.Add(f_);break;}}if(i8.Count==0)return;
var lC=eP();var nL=new List<IMyTerminalBlock>();for(int iS=0;iS<lC.Count;iS++){var hW=lC[iS];foreach(int rO in i8){if(pA(rO,hW)){(hW.CubeGrid==Me.CubeGrid ? h0 : nL).Add(hW);
break;}}}if(h0.Count==0)h0.AddRange(nL);}void qf(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(q==null||q.W.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {q.W.Count} screens, {q.b2.Count} binds");fc=false;
Echo($"Blocks: {eP().Count} on this construct; {hz} on docked ships (ignored)");cg.Clear();Echo("--- BIND RESOLUTION ---");foreach(var an in q.b2){
if(an.bw==aZ.dt){string i5= !string.IsNullOrEmpty(an.bf)? an.bf : an.ab;double tE=aF.ContainsKey(i5)? aF[i5]: an.eX;
Echo($"[OK] @bind {an.ab} (dynamic variable '{i5}' = {tE})");continue;}List<IMyTerminalBlock>h1;if(cj.TryGetValue(an.ab,out h1)){if(h1.Count>0){
string rm=ff.ContainsKey(an.ab)&&ff[an.ab]? " [fallback: typed match]" : "";Echo($"[OK] @bind {an.ab} ({h1.Count} blocks matched{rm})");
}else{string tv=string.Join(", ",an.aR.Count>0 ? an.aR : new List<string>{an.lm});Echo($"[WARN] @bind {an.ab} -> 0 blocks matched \"{tv}\"");
}}else{Echo($"[ERR] @bind {an.ab} -> unresolved");}}if(aF.Count>0){Echo($"--- DYNAMIC VARIABLES ({aF.Count}) ---");foreach(var np in aF)Echo($"[VAR] {np.Key} = {np.Value}");
}IMyTextSurface hN=null;foreach(var sm in bE.Values){hN=sm;break;}if(hN!=null){var kb=new List<string>();hN.GetSprites(kb);
Echo($"--- SPRITES ({kb.Count}): names usable in bg_texture ---");Echo(string.Join(", ",kb.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bo in q.W){IMyTextSurface gC;if(bE.TryGetValue(bo,out gC)){var oq=gC.SurfaceSize;var tk=H.po(bo);
Echo($"[OK] Screen \"{bo.bg}\" [{bo.ca}] -> {bo.bz} [#{bo.b9}] ({oq.X:0}x{oq.Y:0} px, texture {gC.TextureSize.X:0}x{gC.TextureSize.Y:0}, {tk.ac} sprites)");
}else{Echo($"[ERR] Screen \"{bo.bg}\" -> Missing block \"{bo.bz}\" [#{bo.b9}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double qe(IMyTerminalBlock v){
if(v==null)return 0.0;var fp=v as IMyBatteryBlock;if(fp!=null)return fp.MaxStoredPower>0 ?(fp.CurrentStoredPower/fp.MaxStoredPower)*100.0 : 0.0;
var fZ=v as IMyJumpDrive;if(fZ!=null)return fZ.MaxStoredPower>0 ?(fZ.CurrentStoredPower/fZ.MaxStoredPower)*100.0 : 0.0;var oH=v as IMyGasTank;
if(oH!=null)return oH.FilledRatio*100.0;var o1=v as IMyAirVent;if(o1!=null)return o1.GetOxygenLevel()*100.0;var l7=v as IMyCargoContainer;if(l7!=null){
var fW=l7.GetInventory();if(fW!=null&&fW.MaxVolume.RawValue>0)return(double)fW.CurrentVolume.RawValue/fW.MaxVolume.RawValue*100.0;return 0.0;}var gm=v as IMyReactor;
if(gm!=null)return gm.MaxOutput>0 ?(gm.CurrentOutput/gm.MaxOutput)*100.0 : 0.0;var gv=v as IMySolarPanel;if(gv!=null)return gv.MaxOutput>0 ?(gv.CurrentOutput/gv.MaxOutput)*100.0 : 0.0;
var gR=v as IMyWindTurbine;if(gR!=null)return gR.MaxOutput>0 ?(gR.CurrentOutput/gR.MaxOutput)*100.0 : 0.0;var mt=v as IMyDoor;if(mt!=null)return(double)mt.OpenRatio*100.0;
var gM=v as IMyThrust;if(gM!=null)return gM.MaxEffectiveThrust>0 ?(gM.CurrentThrust/gM.MaxEffectiveThrust)*100.0 : 0.0;var kl=v as IMyMotorSuspension;
if(kl!=null)return kl.Brake ? 100.0 :(kl.Friction*100.0);var ii=v as IMyShipConnector;if(ii!=null)return ii.Status==MyShipConnectorStatus.Connected ? 100.0 :(ii.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var n0=v as IMyProductionBlock;if(n0!=null)return n0.IsProducing ? 100.0 : 0.0;var mx=v as IMyShipDrill;if(mx!=null)return mx.Enabled ? 100.0 : 0.0;
var kq=v as IMyShipToolBase;if(kq!=null)return kq.IsActivated ? 100.0 :(kq.Enabled ? 50.0 : 0.0);var iI=v as IMyGasGenerator;if(iI!=null)return iI.AutoRefill ? 100.0 :(iI.Enabled ? 50.0 : 0.0);
var gN=v as IMyLargeTurretBase;if(gN!=null)return gN.IsShooting ? 100.0 :(gN.HasTarget ? 50.0 :(gN.Enabled ? 10.0 : 0.0));var ev=v as IMyProjector;
if(ev!=null)return ev.TotalBlocks>0 ?((double)(ev.TotalBlocks-ev.RemainingBlocks)/ev.TotalBlocks)*100.0 : 0.0;var iO=v as IMyGyro;if(iO!=null)return iO.GyroOverride ? 100.0 :(iO.Enabled ? 50.0 : 0.0);
var m2=v as IMyGravityGeneratorBase;if(m2!=null)return Math.Min(100.0,Math.Abs(m2.GravityAcceleration)/9.81*100.0);var iH=v as IMyLandingGear;if(iH!=null)return iH.IsLocked ? 100.0 :(iH.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var lE=v as IMyRadioAntenna;if(lE!=null)return(lE.Radius/50000.0)*100.0;var lR=v as IMyBeacon;if(lR!=null)return(lR.Radius/50000.0)*100.0;
var nw=v as IMyLightingBlock;if(nw!=null)return nw.Intensity*10.0;var gi=v as IMyPistonBase;if(gi!=null)return gi.HighestPosition>0 ?(gi.CurrentPosition/gi.HighestPosition)*100.0 : 0.0;
var n6=v as IMyMotorStator;if(n6!=null)return Math.Min(100.0,Math.Abs(n6.Angle)/6.28318*100.0);var o4=v as IMyWarhead;if(o4!=null)return o4.IsArmed ? 100.0 : 0.0;
var j5=v as IMySensorBlock;if(j5!=null)return!j5.LastDetectedEntity.IsEmpty()? 100.0 :(j5.Enabled ? 25.0 : 0.0);var ia=v as IMyCameraBlock;
if(ia!=null)return ia.IsActive ? 100.0 :(ia.Enabled ? 50.0 : 0.0);var md=v as IMyParachute;if(md!=null)return(double)md.OpenRatio*100.0;
var gL=v as IMyTurretControlBlock;if(gL!=null)return gL.HasTarget ? 100.0 :(gL.IsUnderControl ? 50.0 :(gL.Enabled ? 10.0 : 0.0));var mS=v as IMyFunctionalBlock;
if(mS!=null)return mS.Enabled ? 100.0 : 0.0;return 0.0;}static double pI(IMyTerminalBlock lM,string iA){int l6=iA.IndexOf(':');string i7=iA.Substring(0,l6).ToLowerInvariant();
var tC=new MyItemType("MyObjectBuilder_"+(i7=="ammo" ? "AmmoMagazine" : char.ToUpper(i7[0])+i7.Substring(1)),iA.Substring(l6+1));double oA=0;for(int iT=0;
iT<lM.InventoryCount;iT++)oA+=(double)lM.GetInventory(iT).GetItemAmount(tC);return oA;}double lf(IMyTerminalBlock w,string mJ){if(w==null)return 0.0;
string b=mJ.ToLowerInvariant();if(b=="percent"||b=="pct")return qe(w);if(b.IndexOf(':')>0)return pI(w,mJ);var bI=w as IMyBatteryBlock;
if(bI!=null){if(b=="current"||b=="stored"||b=="val"||b=="value"||b=="raw")return bI.CurrentStoredPower;if(b=="max"||b=="capacity")return bI.MaxStoredPower;
if(b=="output")return bI.CurrentOutput;if(b=="maxoutput")return bI.MaxOutput;if(b=="input")return bI.CurrentInput;if(b=="maxinput")return bI.MaxInput;
return bI.CurrentStoredPower;}var i3=w as IMyJumpDrive;if(i3!=null){if(b=="max"||b=="capacity")return i3.MaxStoredPower;return i3.CurrentStoredPower;
}var gG=w as IMyGasTank;if(gG!=null){if(b=="max"||b=="capacity")return gG.Capacity;return gG.FilledRatio*gG.Capacity;}var o2=w as IMyAirVent;
if(o2!=null){if(b=="max"||b=="capacity")return 100.0;return o2.GetOxygenLevel()*100.0;}var l8=w as IMyCargoContainer;if(l8!=null){var fX=l8.GetInventory();
if(fX!=null){if(b=="mass")return(double)fX.CurrentMass;if(b=="max"||b=="capacity")return(double)fX.MaxVolume;return(double)fX.CurrentVolume;}return 0.0;}var jQ=w as IMyPowerProducer;
if(jQ!=null){if(b=="max"||b=="capacity")return jQ.MaxOutput;return jQ.CurrentOutput;}var mu=w as IMyDoor;if(mu!=null){if(b=="max")return 1.0;return(double)mu.OpenRatio*100.0;
}var ko=w as IMyThrust;if(ko!=null){if(b=="max"||b=="capacity")return ko.MaxEffectiveThrust;return ko.CurrentThrust;}var bX=w as IMyMotorSuspension;
if(bX!=null){if(b=="brake")return bX.Brake ? 100.0 : 0.0;if(b=="height")return bX.Height;if(b=="strength")return bX.Strength;if(b=="power")return bX.Power;
if(b=="steer")return bX.SteeringOverride*100.0;if(b=="propulsion")return bX.PropulsionOverride*100.0;return bX.Friction;}var mk=w as IMyShipConnector;
if(mk!=null){return mk.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var jN=w as IMyProductionBlock;if(jN!=null){if(b=="producing"||b=="active"||b=="running"||b=="state")return jN.IsProducing ? 100.0 : 0.0;
if(b=="queue")return jN.IsQueueEmpty ? 0.0 : 1.0;}var hR=w as IMyAssembler;if(hR!=null){if(b=="disassembling")return hR.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(b=="repeating"||b=="loop")return hR.Repeating ? 100.0 : 0.0;}var iJ=w as IMyGasGenerator;if(iJ!=null){if(b=="autorefill")return iJ.AutoRefill ? 100.0 : 0.0;
if(b=="active")return iJ.Enabled ? 100.0 : 0.0;}var gO=w as IMyLargeTurretBase;if(gO!=null){if(b=="shooting")return gO.IsShooting ? 100.0 : 0.0;
if(b=="target"||b=="hastarget")return gO.HasTarget ? 100.0 : 0.0;if(b=="range")return gO.Range;}var bR=w as IMyProjector;if(bR!=null){if(b=="built")return bR.TotalBlocks-bR.RemainingBlocks;
if(b=="remaining")return bR.RemainingBlocks;if(b=="total")return bR.TotalBlocks;if(b=="projecting")return bR.IsProjecting ? 100.0 : 0.0;if(b=="armor")return bR.RemainingArmorBlocks;
if(b=="buildable")return bR.BuildableBlocksCount;}var kr=w as IMyShipToolBase;if(kr!=null){if(b=="working"||b=="active"||b=="activated")return kr.IsActivated ? 100.0 : 0.0;
return kr.Enabled ? 100.0 : 0.0;}var cX=w as IMyGyro;if(cX!=null){if(b=="override")return cX.GyroOverride ? 100.0 : 0.0;if(b=="pitch")return cX.Pitch;
if(b=="yaw")return cX.Yaw;if(b=="roll")return cX.Roll;if(b=="power")return cX.GyroPower*100.0;}var m3=w as IMyGravityGeneratorBase;if(m3!=null){if(b=="g"||b=="accel")return m3.GravityAcceleration;
}var m1=w as IMyLandingGear;if(m1!=null){if(b=="locked")return m1.IsLocked ? 100.0 : 0.0;}var lF=w as IMyRadioAntenna;if(lF!=null){if(b=="radius"||b=="range")return lF.Radius;
}var lS=w as IMyBeacon;if(lS!=null){if(b=="radius"||b=="range")return lS.Radius;}var jf=w as IMyLightingBlock;if(jf!=null){if(b=="intensity")return jf.Intensity;
if(b=="radius")return jf.Radius;}var gj=w as IMyPistonBase;if(gj!=null){if(b=="pos"||b=="position"||b=="extension")return gj.CurrentPosition;
if(b=="velocity")return gj.Velocity;if(b=="max")return gj.HighestPosition;}var eA=w as IMyMotorStator;if(eA!=null){if(b=="angle"||b=="deg")return eA.Angle*180.0/Math.PI;
if(b=="rad")return eA.Angle;if(b=="velocity"||b=="rpm")return eA.TargetVelocityRPM;if(b=="locked")return eA.RotorLock ? 100.0 : 0.0;}var kA=w as IMyWarhead;
if(kA!=null){if(b=="armed")return kA.IsArmed ? 100.0 : 0.0;if(b=="countdown")return kA.DetonationTime;}var om=w as IMySensorBlock;if(om!=null){
if(b=="detected")return!om.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var ib=w as IMyCameraBlock;if(ib!=null){if(b=="range")return ib.AvailableScanRange;
if(b=="active")return ib.IsActive ? 100.0 : 0.0;}var me=w as IMyParachute;if(me!=null){if(b=="open"||b=="openratio")return(double)me.OpenRatio*100.0;
}var dg=w as IMyTurretControlBlock;if(dg!=null){if(b=="target"||b=="hastarget")return dg.HasTarget ? 100.0 : 0.0;if(b=="azimuth")return dg.AzimuthRotor!=null ? dg.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(b=="elevation")return dg.ElevationRotor!=null ? dg.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var mT=w as IMyFunctionalBlock;if(mT!=null){return mT.Enabled ? 100.0 : 0.0;
}return 0.0;}double cb(string dY,string mK){be bJ;dJ.TryGetValue(dY,out bJ);if(bJ!=null&&bJ.bw==aZ.dt){
string rP= !string.IsNullOrEmpty(bJ.bf)? bJ.bf : dY;double mn;if(aF.TryGetValue(rP,out mn))return mn;return bJ.eX;
}double mA;if(aF.TryGetValue(dY,out mA))return mA;List<IMyTerminalBlock>dZ;if(!cj.TryGetValue(dY,out dZ)||dZ.Count==0)throw new InvalidOperationException($"HELM: bind '{dY}' resolved to zero blocks — check the name/typo");
if(dZ.Count==1)return lf(dZ[0],mK);var o0=new List<double>();foreach(var qE in dZ)o0.Add(lf(qE,mK));return pd.pc(bJ!=null ? bJ.dj : aD.gV,o0);
}double hq(string qR){return cb(qR,"percent");}static string a1(double oX,bx c2){if(c2==null||(string.IsNullOrEmpty(c2.dH)&&string.IsNullOrEmpty(c2.du)&&c2.I=="percent")){
return kY(oX);}return H.kX(oX,c2.dH,c2.du);}bool p8(D db){IMyTextSurface gD;if(!bE.TryGetValue(db,out gD))return false;
if(db.bZ!=null)return p2(db,gD);if(db.dp.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return p9(db,gD);
}return qa(db,gD);}Color kZ(string[]dd,double sv){if(dd.Length==1)return F(dd[0]);double n1=Math.Max(0.0,Math.Min(100.0,sv))/100.0*(dd.Length-1);
int iU=Math.Min((int)n1,dd.Length-2);float gE=(float)(n1-iU);Color bG=F(dd[iU]),fo=F(dd[iU+1]);return new Color((int)(bG.R+(fo.R-bG.R)*gE),(int)(bG.G+(fo.G-bG.G)*gE),(int)(bG.B+(fo.B-bG.B)*gE),(int)(bG.A+(fo.A-bG.A)*gE));
}static Color[]eM(aP qW,string[]tn,string rd,float o7,float o9,float kx,float iP){int dW=16;bool na=rd=="horizontal";var ih=new Color[dW];
for(int cp=0;cp<dW;cp++){n iG=H.k_(tn,100.0*cp/(dW-1));ih[cp]=new Color(iG.bA,iG.bv,iG.bq);float l5=kx/dW,lZ=iP/dW;t(qW,na ? new Vector2(o7+l5*(cp+0.5f),o9+iP/2f): new Vector2(o7+kx/2f,o9+lZ*(cp+0.5f)),na ? new Vector2(l5+1f,iP): new Vector2(kx,lZ+1f),ih[cp]);
}return ih;}static void t(aP qX,Vector2 sG,Vector2 td,Color q0,float sY=0f,string rc="SquareSimple"){qX.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=rc,Position=sG,Size=td,Color=q0,RotationOrScale=sY}
);}void pk(D k,IMyTextSurface kh,aP h){h.f.Clear();h.dD.Clear();Vector2 O=kh.SurfaceSize;
Vector2 tz=kh.TextureSize;Vector2 aC=(tz-O)/2f;float gb=Math.Max(4f,O.X*0.02f);float cr=Math.Max(4f,O.Y*0.02f);
if(k.ca=="compact"||(k.ca=="auto"&&O.Y<=260f&&O.Y>150f)){gb=4f;cr=4f;}else if(k.ca=="corner"||(k.ca=="auto"&&O.Y<=150f)){
gb=2f;cr=2f;}float eI=O.X-(gb*2f);Color cO=new Color(10,12,16);bool mw=true;if(!string.IsNullOrEmpty(k.br)){
var d9=n.dA(k.br);if(d9.e1)mw=false;else cO=new Color(d9.bA,d9.bv,d9.bq,d9.gT);}h.b1= -1;h.eO=false;
h.eN=F(k.kF ?? "#5b0f0f");h.dn.Clear();if(k.bt!=null&&k.bt.Length>1){h.b1=h.f.Count;h.eR=eM(h,k.bt,k.dm,aC.X+2f,aC.Y+2f,O.X-4f,O.Y-4f);
}else if(mw||k.aL!=null||k.cE!=null){if(k.aL!=null)cO=F(k.aL.First(sL=>sL.cG).b5);h.b1=h.f.Count;
h.eR=new[]{cO};t(h,aC+(O/2f),O-new Vector2(4f,4f),cO);if(k.aL!=null||k.cE!=null)h.dn.Add(new eS{
e0=h.b1,gW=cO,eW=cO,e8=k.aL,g_=k.gX,g8=k.gY,eZ=k.cE,g9=k.ha,hb=k.hc,k5=true}
);}if(!string.IsNullOrEmpty(k.gZ))t(h,aC+(O/2f),O-new Vector2(4f,4f),F(k.kH ?? "255,255,255,24"),0f,k.gZ);
if(!k.dp.Equals("none",StringComparison.OrdinalIgnoreCase)){Color l0=k.aw!=null&&k.aw.Length>0 ? F(k.aw[0]): F(k.eV ?? "40, 60, 85");
Color qV=k.aw!=null&&k.aw.Length>1 ? F(k.aw[k.aw.Length-1]): l0;
float l1=aC.X+(O.X-eI)/2f;if(k.aw!=null&&k.aw.Length>1&& !k.he){eM(h,k.aw,"horizontal",l1,aC.Y+cr*0.5f-1f,eI,2f);
eM(h,k.aw,"horizontal",l1,aC.Y+O.Y-(cr*0.5f)-1f,eI,2f);}else{t(h,new Vector2(aC.X+O.X/2f,aC.Y+cr*0.5f),new Vector2(eI,2f),l0);
t(h,new Vector2(aC.X+O.X/2f,aC.Y+O.Y-(cr*0.5f)),new Vector2(eI,2f),qV);}}var jC=k.ag;
if(jC.Count==0){jC=new List<ay>{new ay{dz="main",dr=0,dC=0,dE=k.dv,dF=k.dw,at=k.e4}};}float m5=8f;
foreach(var s in jC){var ga=H.pn(s.dr,s.dC,s.dE,s.dF,k.dv,k.dw,O.X,O.Y,gb,cr,m5,m5);
float aV=aC.X+ga.ls;float c5=aC.Y+ga.lt;float a6=ga.lr;float es=ga.k1;bool n9=s.aL!=null||s.cE!=null;
if(s.bt!=null&&s.bt.Length>1){eM(h,s.bt,s.dm!="vertical" ? s.dm : k.dm,aV,c5,a6,es);
}else{Color bQ;if(s.aL!=null)bQ=F(s.aL.First(sM=>sM.cG).b5);else if(!string.IsNullOrEmpty(s.cC))bQ=F(s.cC);
else if(!string.IsNullOrEmpty(k.hm))bQ=F(k.hm);else bQ=Color.Transparent;if(bQ.A>0||n9){if(n9)h.dn.Add(new eS{e0=h.f.Count,gW=bQ,eW=bQ,e8=s.aL,g_=s.gX,g8=s.gY,eZ=s.cE,g9=s.ha,hb=s.hc}
);t(h,new Vector2(aV+a6/2f,c5+es/2f),new Vector2(a6,es),bQ);}}string fK=s.kU ?? k.kV ?? "Monospace";float qN=H.pD(k.ca,O.X,O.Y,k.cF);
float aG=s.kW ?? qN;float ea=0f;if(!string.IsNullOrEmpty(s.ce)){ea=20f*aG;var hK=TextAlignment.LEFT;float kp=aV+4f;
string ly=s.ht ?? k.ln ?? "left";if(ly.Equals("center",StringComparison.OrdinalIgnoreCase)){hK=TextAlignment.CENTER;kp=aV+(a6/2f);
}else if(ly.Equals("right",StringComparison.OrdinalIgnoreCase)){hK=TextAlignment.RIGHT;kp=aV+a6-4f;}string oK=s.hu ?? k.lo;
Color tA= !string.IsNullOrEmpty(oK)? F(oK): F(s.g3 ?? k.eV ?? "white");var oP=MySprite.CreateText(s.ce,fK,tA,aG*0.9f,hK);
oP.Position=new Vector2(kp,c5+2f);h.f.Add(oP);string[]hX=s.g4 ?? k.aw;Color rf=hX!=null&&hX.Length>0 ? F(hX[0]): F(s.g3 ?? k.eV ?? "40, 60, 85");
t(h,new Vector2(aV+a6/2f,c5+ea-2f),new Vector2(a6-8f,1f),rf);}if(s.hl=="vertical"||s.at.Any(sD=>sD.g==e.fa)){
pl(h,kh,s,fK,aG,aV,c5,a6,es,ea);continue;}int r9=Math.Max(1,s.at.Count);float st=es-ea-8f;
float jU=st/r9;float fV=6f;for(int fT=0;fT<s.at.Count;fT++){var E=s.at[fT];var T=new b8{e3=E};float sZ=c5+ea+4f+(fT*jU);
float M=sZ+(jU/2f);float oM=M-(11f*aG);T.cH=h.f.Count;T.k0=E.V!=null&&(E.V.Contains("{val}")||E.V.Contains("{percent}"));
var nt=MySprite.CreateText(E.V,fK,Color.White,aG,TextAlignment.LEFT);nt.Position=new Vector2(aV+fV,oM);h.f.Add(nt);
float r2=Math.Max(24f*aG,(E.V.Length+1)*12f*aG);float tL=(!string.IsNullOrEmpty(E.dH)|| !string.IsNullOrEmpty(E.du))? 64f*aG : 52f*aG;
if(E.g!=e.hs){float L=aV+fV+r2;float qM=aV+a6-fV-tL;float J=Math.Max(16f,qM-L);
float S=Math.Max(E.g==e.cc ? 8f : 4f,Math.Min((E.g==e.cc ? 20f : 16f)*aG,jU*(E.g==e.cc ? 0.55f : 0.45f)));
T.b0=L;T.bd=J;T.aY=S;T.b_=M;if(E.g==e.dl){t(h,new Vector2(L+(J/2f),M),new Vector2(J,S),new Color(25,30,38));
T.U=h.f.Count;t(h,new Vector2(L,M),new Vector2(0f,S),F(E.ar));}else if(E.g==e.dy){
int j3=10;float mV=2f;float j4=Math.Max(2f,(J-(mV*(j3-1)))/j3);for(int jY=0;jY<j3;jY++){T.K.Add(h.f.Count);
t(h,new Vector2(L+jY*(j4+mV)+(j4/2f),M),new Vector2(j4,S),new Color(22,26,32));}}else if(E.g==e.cc){t(h,new Vector2(L+(J/2f),M),new Vector2(J,S),new Color(20,24,30));
int jM=8;float mW=2f;float ie=Math.Max(2f,(J-(mW*(jM-1)))/jM);for(int jy=0;jy<jM;jy++){T.K.Add(h.f.Count);
t(h,new Vector2(L+jy*(ie+mW)+(ie/2f),M),new Vector2(ie,2f),F(E.ar));}}else if(E.g==e.dx){t(h,new Vector2(L+(J/2f),M),new Vector2(J,S),new Color(20,24,30));
int ke=10;float mX=2f;float kf=Math.Max(2f,(J-(mX*(ke-1)))/ke);for(int jZ=0;jZ<ke;jZ++){T.K.Add(h.f.Count);
t(h,new Vector2(L+jZ*(kf+mX)+(kf/2f),M),new Vector2(kf,S),new Color(24,28,36),0.523599f);}}else if(E.g==e.dq){
int hP=10;float mY=2f;float hQ=Math.Max(2f,(J-(mY*(hP-1)))/hP);for(int j_=0;j_<hP;j_++){T.K.Add(h.f.Count);
t(h,new Vector2(L+j_*(hQ+mY)+(hQ/2f),M),new Vector2(hQ,S),new Color(24,28,36),1.570796f,"Triangle");}}else if(E.g==e.ds){
int il=12;float mZ=2f;float io=Math.Max(2f,(J-(mZ*(il-1)))/il);float mv=Math.Min(io,S);for(int j0=0;j0<il;j0++){T.K.Add(h.f.Count);
t(h,new Vector2(L+j0*(io+mZ)+(io/2f),M),new Vector2(mv,mv),new Color(24,28,36),0f,"Circle");}}else if(E.g==e.cD){
t(h,new Vector2(L+(J/2f),M),new Vector2(J,S),new Color(25,30,38));T.U=h.f.Count;t(h,new Vector2(L+(J/2f),M),new Vector2(0f,S),F(E.ar));
t(h,new Vector2(L+(J/2f),M),new Vector2(2f,S+4f),new Color(80,110,150));}else if(E.g==e.cf){t(h,new Vector2(L+(J/2f),M),new Vector2(J,S),new Color(20,24,30));
T.U=h.f.Count;float i9=Math.Max(4f,S);t(h,new Vector2(L+i9*0.5f,M),new Vector2(i9,i9),new Color(40,50,65),0f,"Circle");
}else if(E.g==e.b4){float cP=Math.Max(6f,Math.Min(S,14f*aG));t(h,new Vector2(L+cP*0.5f,M),new Vector2(cP,cP),new Color(28,34,46));
T.U=h.f.Count;t(h,new Vector2(L+cP*0.5f,M),new Vector2(cP*0.6f,cP*0.6f),new Color(40,50,65));}else if(E.g==e.cd){
t(h,new Vector2(L+(J/2f),M),new Vector2(J,S),new Color(18,22,30));T.U=h.f.Count;float jJ=Math.Max(4f,S*0.7f);
t(h,new Vector2(L+jJ+2f,M),new Vector2(jJ,jJ),new Color(34,197,94),0f,"Circle");}else if(E.g==e.cB){int jI=8;
float m_=2f;float jK=Math.Max(2f,(J-(m_*(jI-1)))/jI);for(int jz=0;jz<jI;jz++){T.K.Add(h.f.Count);t(h,new Vector2(L+jz*(jK+m_)+(jK/2f),M),new Vector2(jK,S),new Color(24,28,36));
}}else if(E.g==e.b6){t(h,new Vector2(L+(J/2f),M),new Vector2(J,S),new Color(20,24,30));T.U=h.f.Count;
float nd=Math.Max(4f,S);t(h,new Vector2(L+(J/2f),M),new Vector2(nd,nd),F(E.ar),0f,"Triangle");}}var oV=MySprite.CreateText("---",fK,Color.White,aG,TextAlignment.RIGHT);
oV.Position=new Vector2(aV+a6-fV,oM);T.aS=h.f.Count;h.f.Add(oV);h.dD.Add(T);}}h.k7=O;
h.k4=true;}const string qm="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bC=8;void pl(aP Z,IMyTextSurface tq,ay jB,string mR,float d7,float sq,float jA,float sr,float nO,float rG){
int sg=Math.Max(1,jB.at.Count);float mh=(sr-12f)/sg;float gP=22f*d7;float bb=jA+rG+4f+gP;float aJ=Math.Max(20f,jA+nO-4f-gP-bb);
float q9=tq.MeasureStringInPixels(new StringBuilder("M"),mR,d7*0.9f).X;for(int fU=0;fU<jB.at.Count;fU++){var f5=jB.at[fU];float a5=sq+6f+mh*(fU+0.5f);
float cz=mh*0.7f;var a8=new b8{e3=f5,lp=true,b0=a5,bd=cz,aY=aJ,b_=bb+aJ/2f,kO=q9};a8.cH=Z.f.Count;
aX(Z.f,f5.V,Color.White,a5,jA+nO-4f-gP,d7);a8.aS=Z.f.Count;aX(Z.f,"---",Color.White,a5,bb-gP,d7);
var nq=Z.f[a8.cH];nq.Alignment=TextAlignment.CENTER;Z.f[a8.cH]=nq;var oU=Z.f[a8.aS];oU.Alignment=TextAlignment.CENTER;
Z.f[a8.aS]=oU;t(Z,new Vector2(a5,bb+aJ/2f),new Vector2(cz,aJ),new Color(25,30,38));a8.U=Z.f.Count;if(f5.g==e.fa){
for(int lN=0;lN<bC;lN++){a8.K.Add(Z.f.Count);t(Z,new Vector2(a5,bb+aJ),new Vector2(cz-6f,0f),Color.White);}for(int ky=0;ky<4;
ky++){a8.K.Add(Z.f.Count);var oF=MySprite.CreateText("",mR,ky==0 ? new Color(255,255,255,170): new Color(255,255,255,110),d7*0.9f,TextAlignment.CENTER);
oF.Position=new Vector2(a5,bb+aJ);Z.f.Add(oF);}Color iq=new Color(90,110,135);t(Z,new Vector2(a5-cz/2f,bb+aJ/2f),new Vector2(3f,aJ),iq);t(Z,new Vector2(a5+cz/2f,bb+aJ/2f),new Vector2(3f,aJ),iq);
t(Z,new Vector2(a5,bb+aJ),new Vector2(cz+3f,3f),iq);}else{t(Z,new Vector2(a5,bb+aJ),new Vector2(cz,0f),F(f5.ar));}Z.dD.Add(a8);
}}bool ql(aP a3,b8 y,double eK,double n2,Color fy,bool oZ){var bk=y.e3;float lU=y.C;if(y.C<0f)y.C=(float)eK;
y.C=Math.Abs(y.C-(float)eK)>0.05f ? y.C+((float)eK-y.C)*0.35f :(float)eK;float cq=Math.Max(0f,Math.Min(1f,y.C/100f))*y.aY;
float fv=y.b_+y.aY/2f;float fA=y.b0;if(bk.g!=e.fa){var fF=a3.f[y.U];fF.Size=new Vector2(y.bd,cq);
fF.Position=new Vector2(fA,fv-cq/2f);fF.Color=fy;a3.f[y.U]=fF;if(oZ||y.C!=lU)aE(a3,y,a1(n2,bk),fy);
return oZ||y.C!=lU;}float hZ=y.aY/bC;Color sb=bk.bu!=null ? F(H.dB(bk.bu,eK)): fy;
for(int cl=0;cl<bC;cl++){float o3=Math.Max(0f,Math.Min(hZ,cq-cl*hZ));var gk=a3.f[y.K[cl]];gk.Size=new Vector2(y.bd-6f,o3);
gk.Position=new Vector2(fA,fv-cl*hZ-o3/2f);gk.Color=bk.bu==null&&bk.b7&&bk.a2.Length>1 ? kZ(bk.a2,100.0*(cl+0.5)/bC): sb;
a3.f[y.K[cl]]=gk;}int qZ=Math.Max(1,(int)((y.bd-8f)/y.kO));bool ta=cq>6f;var kB=a3.f[y.K[bC]];
kB.Data=ta ? qm.Substring((int)(ad*5.0)%6,Math.Min(qZ,100)): "";kB.Position=new Vector2(fA,fv-cq-12f);a3.f[y.K[bC]]=kB;
for(int bO=0;bO<3;bO++){var h6=a3.f[y.K[bC+1+bO]];double sz=(ad*(0.35+0.1*bO)+bO*0.37)%1.0;h6.Data=cq>24f ?(bO==1 ? "O" : "o"): "";
h6.Position=new Vector2(fA+(bO-1)*y.bd*0.26f,fv-(float)sz*(cq-6f)-10f);a3.f[y.K[bC+1+bO]]=h6;}aE(a3,y,a1(n2,bk),fy);
return true;}static void aE(aP l3,b8 os,string ty,Color q1){var kv=l3.f[os.aS];kv.Data=ty;kv.Color=q1;l3.f[os.aS]=kv;
}static void lh(aP l4,b8 ot,Color q2){var mE=l4.f[ot.U];mE.Color=q2;l4.f[ot.U]=mE;}bool qa(D bp,IMyTextSurface ki){
aP i;if(!dQ.TryGetValue(bp,out i)){i=new aP();dQ[bp]=i;}bool n5= !i.k4||i.k7!=ki.SurfaceSize;
if(n5){pk(bp,ki,i);}bool N=n5;bool fk=(ad%bp.gU)<bp.gU/2.0;double co= -1.0,ml;if(fd.TryGetValue(bp,out ml)){
co=(ad-ml)/bp.g1;if(co>=1.0){fd.Remove(bp);co=1.0;}}bool kz=ch&&fk&&i.eN.A>0;if(i.b1>=0&&kz!=i.eO){
i.eO=kz;for(int ec=0;ec<i.eR.Length;ec++){var lV=i.f[i.b1+ec];lV.Color=kz ? i.eN : i.eR[ec];i.f[i.b1+ec]=lV;
}N=true;foreach(var qO in i.dn)qO.hd=true;}foreach(var ai in i.dn){if(ai.k5&&i.eO)continue;Color dX=ai.gW;try{if(ai.e8!=null)dX=F(H.dB(ai.e8,cb(ai.g_,ai.g8)));
if(ai.eZ!=null&&fk&&H.dB(ai.eZ,cb(ai.g9,ai.hb))!=null)dX=i.eN;}catch{}if(dX!=ai.eW||ai.hd){
ai.eW=dX;ai.hd=false;var l2=i.f[ai.e0];l2.Color=dX;i.f[ai.e0]=l2;N=true;}}for(int iV=0;iV<i.dD.Count;iV++){var c=i.dD[iV];
var j=c.e3;double x;bool m7=false;double jm=j.e5;try{x=cb(j.aM,j.I);if(j.cI!=null)jm=cb(j.cI,j.hi);
}catch{m7=true;x=0.0;}if(m7){if(c.e2!= -99999.0||(c.aS>=0&&i.f[c.aS].Data!="ERR")){N=true;c.e2= -99999.0;
if(c.aS>=0){var j1=i.f[c.aS];j1.Data="ERR";j1.Color=Color.Red;i.f[c.aS]=j1;}}continue;}double aW=x;
if(jm>0)x=Math.Min(100.0,aW/jm*100.0);bool oC=co>=0.0&&j.g!=e.hs&&j.g!=e.cf&&j.g!=e.b4&&j.g!=e.cd&&j.g!=e.b6;
if(oC)x=co<0.5 ? co*200.0 : 100.0+(x-100.0)*(co-0.5)*2.0;string q4=j.b7 ? H.e_(j.a2,x): j.ar;
Color _=F(q4);bool bj=(_==Color.Red);if(j.b7&& !bp.he&&j.a2.Length>1)_=kZ(j.a2,x);
if(bj&&fk){_=new Color(255,75,75);N=true;}bool aK=oC||Math.Abs(aW-c.e2)>0.1;if(aK)c.e2=aW;
if(c.lp){if(ql(i,c,x,aW,_,aK))N=true;continue;}if(j.g==e.dl){double mL=(j.hg||j.I=="percent"||j.I=="pct")? x : hq(j.aM);
if(c.C<0f)c.C=(float)mL;float gH=(float)mL;if(Math.Abs(c.C-gH)>0.05f){c.C+=(gH-c.C)*0.35f;
N=true;}else if(c.C!=gH){c.C=gH;N=true;}if(aK)N=true;if(N){float sw=(float)Math.Max(0.0,Math.Min(1.0,c.C/100.0));
float mN=c.bd*sw;var fH=i.f[c.U];fH.Size=new Vector2(mN,c.aY);fH.Position=new Vector2(c.b0+(mN/2f),c.b_);
fH.Color=_;i.f[c.U]=fH;aE(i,c,a1(aW,j),_);}}else if(j.g==e.dy||j.g==e.dx||j.g==e.dq||j.g==e.ds){
if(aK||bj){N=true;double ro=(j.hg||j.I=="percent"||j.I=="pct")? x : hq(j.aM);int ok=c.K.Count;
for(int eC=0;eC<ok;eC++){bool sc=ro>=((eC+0.4)/(double)ok*100.0);Color s8=sc ? _ : new Color(24,28,36);var ol=i.f[c.K[eC]];
ol.Color=s8;i.f[c.K[eC]]=ol;}aE(i,c,a1(aW,j),_);}}else if(j.g==e.cc){
if(aK||bj){N=true;for(int fO=0;fO<7;fO++)c.e9[fO]=c.e9[fO+1];c.e9[7]=(float)x;int sF=c.K.Count;
for(int eo=0;eo<sF;eo++){float rF=c.e9[eo];float rE=Math.Max(0.06f,Math.Min(1f,rF/100f));float mg=c.aY*rE;float q3=c.b_+(c.aY/2f)-(mg/2f);
var cU=i.f[c.K[eo]];cU.Size=new Vector2(cU.Size.Value.X,mg);cU.Position=new Vector2(cU.Position.Value.X,q3);
cU.Color=_;i.f[c.K[eo]]=cU;}aE(i,c,a1(aW,j),_);}}else if(j.g==e.cD){
if(c.C<0f)c.C=(float)x;float gI=(float)x;if(Math.Abs(c.C-gI)>0.05f){c.C+=(gI-c.C)*0.35f;
N=true;}else if(c.C!=gI){c.C=gI;N=true;}if(N||aK){float m6=c.bd*0.5f;float mb=c.b0+m6;
float mr=(float)Math.Max(-1.0,Math.Min(1.0,(c.C-50.0)/50.0));float iE=Math.Abs(mr)*m6;float rp=(mr>=0)?(mb+iE*0.5f):(mb-iE*0.5f);
var fI=i.f[c.U];fI.Size=new Vector2(iE,c.aY);fI.Position=new Vector2(rp,c.b_);fI.Color=_;
i.f[c.U]=fI;aE(i,c,a1(aW,j),_);}}else if(j.g==e.cf){if(aK||bj){
N=true;bool i2=x>=50.0;float nm=Math.Max(4f,c.aY);float rS=i2 ?(c.b0+c.bd-nm*0.5f):(c.b0+nm*0.5f);
Color nl=i2 ?(_!=Color.White ? _ : new Color(34,197,94)): new Color(60,70,85);var iC=i.f[c.U];iC.Position=new Vector2(rS,c.b_);
iC.Color=nl;i.f[c.U]=iC;aE(i,c,i2 ? "ON" : "OFF",nl);}}else if(j.g==e.b4){if(aK||bj){
N=true;bool i1=x>=50.0;Color mc=i1 ?(_!=Color.White ? _ : new Color(56,189,248)): new Color(30,36,48);lh(i,c,mc);
aE(i,c,i1 ? "ACTIVE" : "READY",i1 ? mc : new Color(148,163,184));}}else if(j.g==e.cd){if(aK||bj){
N=true;string tl=x>=75.0 ? "ONLINE" :(x>=25.0 ? "STBY" : "OFFLINE");Color ox=x>=75.0 ? new Color(34,197,94):(x>=25.0 ? new Color(234,179,8): new Color(239,68,68));
lh(i,c,ox);aE(i,c,tl,ox);}}else if(j.g==e.cB){if(aK||bj){N=true;int nX=c.K.Count;
for(int ep=0;ep<nX;ep++){bool sd=x>=((ep+0.5)/(double)nX*100.0);Color sB=sd ? _ : new Color(24,28,36);var nY=i.f[c.K[ep]];
nY.Color=sB;i.f[c.K[ep]]=nY;}aE(i,c,a1(aW,j),_);}}else if(j.g==e.b6){
N=true;float gp;Color d6;string fJ;bool rK=(j.I=="producing"||j.I=="active"||j.I=="running"||j.I=="state"||(j.I!="percent"&&(x<=1.0||x==100.0)));
if(rK){if(x>0.0){c.C=(c.C+0.3927f)%6.28318f;gp=c.C;d6=new Color(34,197,94);fJ="RUN";}else{gp=0f;
d6=new Color(148,163,184);fJ="IDLE";}}else{gp=(x>55.0)? 0f :((x<45.0)? 3.14159f : 1.57079f);d6=(x>55.0)? new Color(34,197,94):((x<45.0)? new Color(239,68,68): new Color(148,163,184));
fJ=(x>55.0)? "CHG" :((x<45.0)? "DSC" : "STBY");}var iD=i.f[c.U];iD.RotationOrScale=gp;iD.Color=d6;
i.f[c.U]=iD;aE(i,c,fJ,d6);}else{string fE;be gK;dJ.TryGetValue(j.aM,out gK);dL.TryGetValue(k6(j.I)? j.aM+"."+j.I :(gK!=null&&gK.bf!=null ? gK.bf : j.aM),out fE);
if(aK||bj||fE!=c.k8){c.k8=fE;N=true;Color ks=j.bu!=null ? F(H.dB(j.bu,x)): Color.White;
if(ks==Color.Red&&fk)ks=new Color(255,75,75);aE(i,c,fE ?? a1(aW,j),ks);}}if(c.k0&&(aK||bj)){
N=true;var nv=i.f[c.cH];string sx=kY(x);string sW=a1(aW,j);nv.Data=j.V.Replace("{percent}",sx).Replace("{val}",sW);
i.f[c.cH]=nv;}}if(N){using(var ru=ki.DrawFrame()){ru.AddRange(i.f);}}return N;}bool p9(D bT,IMyTextSurface kj){
kj.FontSize=bT.cF;var fz=new List<KeyValuePair<string,string>>();foreach(var m in bT.e4){double P=0.0;string ba;string a4=null;
double jn=m.e5;try{P=cb(m.aM,m.I);if(m.cI!=null)jn=cb(m.cI,m.hi);}catch(Exception tW){
fz.Add(new KeyValuePair<string,string>($"{m.V}: [UNRESOLVED]","red"));continue;}double n3=P;if(jn>0)P=Math.Min(100.0,n3/jn*100.0);string oW=a1(n3,m);
if(m.g==e.dl){double mM=(m.hg||m.I=="percent"||m.I=="pct")? P : hq(m.aM);ba=H.p3(m.V,mM,10,oW);
a4=m.b7 ? H.e_(m.a2,mM): m.ar;}else if(m.g==e.cD){ba=H.p1(m.V,P,10);
a4=m.b7 ? H.e_(m.a2,P): m.ar;}else if(m.g==e.cf){ba=H.qc(m.V,P);
a4=m.ar ??(P>=50.0 ? "green" : "gray");}else if(m.g==e.b4){ba=H.p6(m.V,P);a4=m.ar ??(P>=50.0 ? "cyan" : "gray");
}else if(m.g==e.cd){ba=H.qb(m.V,P);a4=m.ar ??(P>=75.0 ? "lime" :(P>=25.0 ? "yellow" : "red"));
}else if(m.g==e.cB){ba=H.p0(m.V,P,8);a4=m.b7 ? H.e_(m.a2,P):(m.ar ?? "yellow");
}else if(m.g==e.b6){bool rL=(m.I=="producing"||m.I=="active"||m.I=="running"||m.I=="state"||(m.I!="percent"&&(P<=1.0||P==100.0)));
if(rL){char[]tj=new[]{'|','/','-','\\'};char s5=tj[Math.Abs(dR)%4];ba=P>0 ? $"{m.V} [{s5}] RUN" : $"{m.V} [==] IDLE";
a4=P>0 ? "green" : "gray";}else{ba=H.p7(m.V,P,dR);a4=m.ar ??(P>55.0 ? "green" :(P<45.0 ? "red" : "cyan"));
}}else{ba=$"{m.V}: {oW}";a4=m.bu!=null ? H.dB(m.bu,P): null;}fz.Add(new KeyValuePair<string,string>(ba,a4));
}var fu=H.p5(new List<string>(fz.ConvertAll(tt=>tt.Key)),di: bT.lq);string oo=string.Join("\n",fu);
string n_;if(ci.TryGetValue(bT,out n_)&&n_==oo)return false;ci[bT]=oo;using(var rv=kj.DrawFrame()){
float pa=4f;float nx=pb*bT.cF;try{var nz=kj.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bT.cF);
if(nz.Y>0)nx=nz.Y;}catch{}for(int cY=0;cY<fu.Count;cY++){string mj=null;if(cY>0&&cY<fu.Count-1)mj=fz[cY-1].Value;
var ow=MySprite.CreateText(fu[cY],"Monospace",F(mj),bT.cF,TextAlignment.LEFT);ow.Position=new Vector2(4f,pa);rv.Add(ow);
pa+=nx;}}return true;}Color F(string f8){if(string.IsNullOrEmpty(f8))return Color.White;Color ej;if(lu.TryGetValue(f8,out ej))return ej;
n d_=n.dA(f8);ej=d_.e1 ? Color.Transparent : new Color(d_.bA,d_.bv,d_.bq,d_.gT);lu[f8]=ej;return ej;}public enum aZ{pf,hf,dt}
public enum aD{pM,gV,ll,la,k9}public class be{public string ab;public aZ bw;public List<string>aR=new List<string>();public string lm{
get{return aR.Count>0 ? aR[0]: "";}set{if(aR.Count==0)aR.Add(value);else aR[0]=value;}}public aD dj=aD.pM;
public double eX=0.0;public string g5;public string bf;public bool eQ;}public enum e{hs,dl,dy,cc,dx,dq,ds,cD,cf,b4,cd,cB,b6,fa}
public class aN{public string g7;public string b5;public bool cG;}public class bx{public e g;public string V;public string aM;
public string I;public bool b7;public string[]a2;public string ar;public List<aN>bu;public string kP="right";
public string dH;public string du;public double e5;public string cI,hi;public bool hg{get{return e5>0||cI!=null;}}}public struct n{
public byte bA,bv,bq,gT;public bool e1;public n(byte sN,byte rx,byte qF,byte qo=255){bA=sN;bv=rx;bq=qF;gT=qo;e1=false;}public static readonly n qk=new n(0,0,0,0){
e1=true};public static readonly n fb=new n(255,255,255);public static readonly n pe=new n(0,0,0);public static readonly n pZ=new n(255,0,0);
public static readonly n pF=new n(0,255,0);public static readonly n pJ=new n(50,205,50);public static readonly n qn=new n(255,255,0);
public static readonly n pN=new n(255,140,0);public static readonly n pq=new n(0,255,255);public static readonly n pg=new n(0,120,255);
public static readonly n pK=new n(255,0,255);public static readonly n pX=new n(128,0,128);public static readonly n pE=new n(128,128,128);
public static readonly n pr=new n(60,60,60);public static n dA(string a9){if(string.IsNullOrWhiteSpace(a9))return fb;a9=a9.Trim();
if(a9.Equals("transparent",StringComparison.OrdinalIgnoreCase)||a9.Equals("none",StringComparison.OrdinalIgnoreCase))return qk;if(a9.StartsWith("#")){
string bM=a9.Substring(1).Trim();if(bM.Length==6){byte sO=Convert.ToByte(bM.Substring(0,2),16);byte ry=Convert.ToByte(bM.Substring(2,2),16);byte qG=Convert.ToByte(bM.Substring(4,2),16);
return new n(sO,ry,qG);}if(bM.Length==3){byte sP=Convert.ToByte(new string(bM[0],2),16);byte rz=Convert.ToByte(new string(bM[1],2),16);byte qH=Convert.ToByte(new string(bM[2],2),16);
return new n(sP,rz,qH);}}if(a9.Contains(",")){var c6=a9.Split(',');if(c6.Length>=3){byte sQ=(byte)Math.Max(0,Math.Min(255,int.Parse(c6[0].Trim())));
byte rA=(byte)Math.Max(0,Math.Min(255,int.Parse(c6[1].Trim())));byte qI=(byte)Math.Max(0,Math.Min(255,int.Parse(c6[2].Trim())));byte qp=c6.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(c6[3].Trim()))):(byte)255;
return new n(sQ,rA,qI,qp);}}switch(a9.ToLowerInvariant()){case "red": return pZ;case "green": return pF;case "lime": return pJ;case "yellow": return qn;
case "orange": return pN;case "cyan": return pq;case "blue": return pg;case "magenta": return pK;case "purple": return pX;case "white": return fb;
case "black": return pe;case "gray": case "grey": return pE;case "darkgray": case "darkgrey": return pr;default: return fb;}}}public class bs{
public string[]bt;public string dm="vertical";public List<aN>aL;public string gX,gY;public List<aN>cE;public string ha,hc;
}public class ay : bs{public string dz="default";public int dr=0;public int dC=0;public int dE=1;public int dF=1;public string ce;public string ht="left";
public string hu;public string kM;public string kU;public float? kW;public string cC;public string g3;public string[]g4;
public string hl="horizontal";public List<bx>at=new List<bx>();}public class D : bs{public string bg;public string ca="standard";
public string dp="graphic";public string eV;public string[]aw;public string ln="left";public string lo;public string kV="Monospace";
public string br;public string kK;public int kL=10;public int kI=1;public float g1=1.2f;public bool he;
public string hm;public string gZ;public string kH;public string kF;public string bZ;public string b3;public float g2=2.5f;
public string kJ="cyan";public string[]eT;public List<string>eU=new List<string>();public List<string>pC(){if(eU!=null&&eU.Count>0)return eU;
return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float cF=1.0f;public int ho=0;public float gU=1.6f;public int dv=1;public int dw=1;public int lq=30;public string bz;
public int b9=0;public List<ay>ag=new List<ay>();public List<bx>e4{get{if(ag.Count==0)ag.Add(new ay{dz="main"}
);return ag[0].at;}}}public class bD{public string hj;public List<string>e6=new List<string>();public Dictionary<string,string>g6=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>g0=new List<string>();}public class hn{public List<be>b2=new List<be>();public List<D>W=new List<D>();
public List<string>a0=new List<string>();public List<string>dI=new List<string>();public Dictionary<string,List<string>>dk=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class k2{static readonly Dictionary<string,bD>kN=new Dictionary<string,bD>(StringComparer.OrdinalIgnoreCase);
static k2(){p_();}static void p_(){bB("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bB("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bB("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bB("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bB("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bB("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bB("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bB("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bB(string nG,string[]su,string qS){var gF=new bD{hj=nG}
;foreach(var sn in su){var ja=sn.Split(new[]{'='},2);string nP=hk(ja[0]);gF.e6.Add(nP);if(ja.Length>1)gF.g6[nP]=ah(ja[1].Trim());
}var sa=qS.Replace("\r\n","\n").Split('\n');foreach(var rY in sa){string ob=rY.Trim();if(ob.Length>0)gF.g0.Add(ob);}kN[nG]=gF;}static string hk(string c3){
c3=c3.Trim();if(c3.StartsWith("$"))c3=c3.Substring(1);return c3.ToLowerInvariant();}public static hn pP(string q7){var ao=new hn();D d=null;
ay r=null;var nZ=pV(q7,ao.a0,ao.dk);for(int ed=0;ed<nZ.Count;ed++){string a=nZ[ed].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){ao.b2.Add(pQ(a));}else if(a.StartsWith("@screen")){d=new D{bg=a.Substring("@screen".Length).Trim()}
;ao.W.Add(d);r=null;}else if(a.StartsWith("profile:")){p(d,"profile:");d.ca=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){p(d,"border:");string lO=a.Substring("border:".Length).Trim();if(r!=null)r.kM=lO;
else d.dp=lO;}else if(a.StartsWith("border_color:")){p(d,"border_color:");string lQ=a.Substring("border_color:".Length).Trim();
if(r!=null)r.g3=lQ;else d.eV=lQ;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
p(d,"border_gradient");string cm=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
cm=cm.Trim();if(cm.StartsWith("(")&&cm.Contains(")"))cm=aO(cm,0);string[]oy=aQ(cm).Select(s_=>s_.Trim()).ToArray();if(r!=null)r.g4=oy;
else d.aw=oy;}else if(a.StartsWith("title_align:")){p(d,"title_align:");string oG=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(r!=null)r.ht=oG;else d.ln=oG;}else if(a.StartsWith("title_color:")){p(d,"title_color:");string oJ=a.Substring("title_color:".Length).Trim();
if(r!=null)r.hu=oJ;else d.lo=oJ;}else if(a.StartsWith("font_size:")){p(d,"font_size:");float oD=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(r!=null)r.kW=oD;else d.cF=oD;}else if(a.StartsWith("font:")){p(d,"font:");string mF=a.Substring("font:".Length).Trim();
if(r!=null)r.kU=mF;else d.kV=mF;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){p(d,"bg:");string cN=a.Substring(a.IndexOf(':')+1).Trim();
bs hY=r!=null ?(bs)r : d;if(cN.StartsWith("color(")&&cN.EndsWith(")")){string lY;hY.aL=ld(cN.Substring(6,cN.Length-7),out lY);
string lW,lX;li(lY,out lW,out lX);hY.gX=lW;hY.gY=lX;}else if(r!=null)r.cC=cN;
else d.br=cN;}else if(a.StartsWith("layout:")){p(d,"layout:");pR(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
p(d,"alert_period:");d.gU=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){p(d,"refresh:");d.ho=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
p(d,"output:");var ge=aQ(a.Substring("output:".Length).Trim());if(ge.Count==0)throw new FormatException("output: requires a target block name");
d.bz=ah(ge[0]);d.b9=ge.Count>1 ? int.Parse(ge[1]): 0;}else if(a.StartsWith("width:")){p(d,"width:");
d.lq=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){p(d,"boot_msg:");
d.kK=ah(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){p(d,"boot_stagger:");d.kL=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){p(d,"boot_calibrate:");string mm=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.g1=mm=="off" ? 0f : Math.Max(0f,float.Parse(mm,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){p(d,"boot_batch:");
d.kI=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){p(d,"boot_duration:");
string d1=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool nc=d1.EndsWith("s");if(nc)d1=d1.Substring(0,d1.Length-1).Trim();
float ip;if(float.TryParse(d1,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out ip)){d.g2=nc ? ip : ip/60f;
}}else if(a.StartsWith("boot_log:")){p(d,"boot_log:");d.eU.Add(ah(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("banner:")){
p(d,"banner:");d.bZ=ah(a.Substring("banner:".Length).Trim());}else if(a.StartsWith("boot_logo:")){p(d,"boot_logo:");
d.b3=ah(a.Substring("boot_logo:".Length).Trim());}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){
p(d,"boot_gradient");d.eT=le(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){p(d,"bg_gradient");
(r!=null ?(bs)r : d).bt=le(a);}else if(a.StartsWith("bg_gradient_dir:")){p(d,"bg_gradient_dir:");
(r!=null ?(bs)r : d).dm=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(a.StartsWith("flash:")){
p(d,"flash:");bs iz=r!=null ?(bs)r : d;string iy=a.Substring("flash:".Length).Trim();string mH=lk(iy);
string mG,mI;li(iy.Substring(0,iy.Length-mH.Length),out mG,out mI);iz.ha=mG;iz.hc=mI;iz.cE=new List<aN>{
new aN{g7=mH,b5="on"}};}else if(a.StartsWith("bg_texture:")){p(d,"bg_texture:");d.gZ=ah(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){p(d,"bg_texture_color:");d.kH=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gradient_mode:")){p(d,"gradient_mode:");d.he=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){p(d,"screen_bg:");d.br=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
p(d,"panel_bg:");d.hm=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){p(d,"alert_bg:");
d.kF=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){p(d,"boot_color:");
d.kJ=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){p(d,"orientation:");string sk=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(r!=null)r.hl=sk;}else if(a.StartsWith("@panel")){p(d,"@panel");r=pT(a);d.ag.Add(r);
}else if(a.StartsWith("title:")){p(d,"title:");if(r==null){r=new ay{dz="main",dr=0,dC=0,dE=d.dv,dF=d.dw}
;d.ag.Add(r);}r.ce=ah(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){p(d,"line:");
if(r==null){r=new ay{dz="main",dr=0,dC=0,dE=d.dv,dF=d.dw};d.ag.Add(r);}r.at.Add(pS(a.Substring("line:".Length).Trim()));
}else{ao.a0.Add($"Line {ed + 1}: unrecognized directive: \"{a}\"");}}catch(Exception ri){ao.a0.Add($"Line {ed + 1}: parse error ({ri.Message}) in \"{a}\"");
}}foreach(var oc in ao.W){if(string.IsNullOrEmpty(oc.bz))ao.a0.Add($"@screen {oc.bg}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var aq in ao.W){int jw=aq.ag.Count(jL=>jL.cC!=null||jL.aL!=null||jL.bt!=null);if(aq.ag.Count>1&&jw>0&&jw<aq.ag.Count)ao.dI.Add("screen '"+aq.bg+"': only "+jw+" of "+aq.ag.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(aq.bZ!=null&& !ao.dk.ContainsKey(aq.bZ))ao.a0.Add("Screen '"+aq.bg+"': banner '"+aq.bZ+"' is not defined (@banner "+aq.bZ+" ... @end)");
if(aq.b3!=null&& !ao.dk.ContainsKey(aq.b3))ao.a0.Add("Screen '"+aq.bg+"': boot_logo '"+aq.b3+"' is not defined (@banner "+aq.b3+" ... @end)");
}return ao;}static void pR(string h3,D oj){int jD=h3.IndexOf('(');if(jD<0)throw new FormatException("missing '(' in layout: specification");
string nk=h3.Substring(0,jD).Trim().ToLowerInvariant();string qw=aO(h3,jD);var hO=aQ(qw);if(nk=="grid"){if(hO.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
oj.dv=Math.Max(1,int.Parse(hO[0]));oj.dw=Math.Max(1,int.Parse(hO[1]));}else{throw new FormatException($"unknown layout kind '{nk}' (expected grid)");
}}static ay pT(string r5){string ex=r5.Substring("@panel".Length).Trim();int fn=ex.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(fn<0)throw new FormatException("@panel requires an at(...) positioning clause");
string sh=ex.Substring(0,fn).Trim();string lJ=ex.Substring(fn);int nV=lJ.IndexOf('(');string lH=aO(lJ,nV);
var ck=aQ(lH);if(ck.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bm=new ay{dz=sh,dr=int.Parse(ck[0]),dC=int.Parse(ck[1]),dE=ck.Count>2 ? int.Parse(ck[2]): 1,dF=ck.Count>3 ? int.Parse(ck[3]): 1}
;int lA=fn+nV+lH.Length+1;if(lA<ex.Length){string aH=ex.Substring(lA).Trim();int oO=aH.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(oO>=0){string eE=aH.Substring(oO+5).Trim();if(eE.StartsWith("(")&&eE.Contains(")"))bm.ce=ah(aO(eE,0));else if(eE.StartsWith(":")){
string cn=eE.Substring(1).Trim();if(cn.StartsWith("\"")){int mB=cn.IndexOf('"',1);bm.ce=mB>0 ? cn.Substring(1,mB-1): ah(cn);}else{int ou=cn.IndexOf(' ');
bm.ce=ou>=0 ? cn.Substring(0,ou): cn;}}}string nJ=eY(aH,"orientation:");if(nJ!=null)bm.hl=nJ.ToLowerInvariant();string lz=eY(aH,"title_align:");
if(lz!=null)bm.ht=lz.ToLowerInvariant();string oL=eY(aH,"title_color:");if(oL!=null)bm.hu=oL;int ft=aH.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(ft>=0&& !aH.Substring(ft).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aH.Substring(ft).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bm.kM=eY(aH.Substring(ft),"border:");}int fq=aH.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(fq<0)fq=aH.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(fq>=0){string bK=aH.Substring(fq+"border_gradient".Length).Trim();if(bK.StartsWith(":"))bK=bK.Substring(1).Trim();if(bK.StartsWith("(")&&bK.Contains(")"))bK=aO(bK,0);
bm.g4=aQ(bK).Select(s0=>s0.Trim()).ToArray();}}return bm;}static string eY(string od,string nj){int nb=od.IndexOf(nj,StringComparison.OrdinalIgnoreCase);
if(nb<0)return null;string kw=od.Substring(nb+nj.Length).Trim();int ov=kw.IndexOf(' ');return ov>=0 ? kw.Substring(0,ov): kw;}static string[]le(string jg){
int iK=jg.IndexOfAny(new[]{':','('});string d8=jg.Substring(jg[iK]==':' ? iK+1 : iK).Trim();if(d8.StartsWith("(")&&d8.Contains(")"))d8=aO(d8,0);return aQ(d8).Select(s1=>s1.Trim()).ToArray();
}static List<string>pV(string q8,List<string>iv,Dictionary<string,List<string>>qK){var oR=new Dictionary<string,bD>(StringComparer.OrdinalIgnoreCase);
var c8=(q8 ?? "").Replace("\r\n","\n").Split('\n');var jv=new List<string>();int cZ=0;while(cZ<c8.Length){string jR=c8[cZ++];if(jR.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var am=new List<string>();while(cZ<c8.Length){string lx=c8[cZ++].Replace("\t","    ").TrimEnd();if(lx.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
am.Add(lx);}while(am.Count>0&&am[0].Length==0)am.RemoveAt(0);while(am.Count>0&&am[am.Count-1].Length==0)am.RemoveAt(am.Count-1);int i0=int.MaxValue;
foreach(var hG in am)if(hG.Length>0)i0=Math.Min(i0,hG.Length-hG.TrimStart().Length);for(int ek=0;ek<am.Count;ek++)if(am[ek].Length>0)am[ek]=am[ek].Substring(i0);
qK[jR.TrimStart().Substring(8).Trim()]=am;continue;}string bP=lj(jR).Trim();if(bP.Length==0)continue;if(bP.StartsWith("@template")){try{
var km=pU(bP);while(cZ<c8.Length){string qU=c8[cZ++];string h4=lj(qU).Trim();if(h4.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(h4.Length>0)km.g0.Add(h4);}oR[km.hj]=km;}catch(Exception rj){iv.Add($"Template parse error ({rj.Message}) in \"{bP}\"");}}
else if(bP.StartsWith("@use")){try{kR(bP,oR,jv,iv);}catch(Exception rk){iv.Add($"@use error ({rk.Message}) in \"{bP}\"");
}}else{jv.Add(bP);}}return jv;}static bD pU(string r6){string jS=r6.Substring("@template".Length).Trim();int jE=jS.IndexOf('(');
if(jE<0)throw new FormatException("missing '(' in @template header");string si=jS.Substring(0,jE).Trim();string qx=aO(jS,jE);var kn=new bD{
hj=si};foreach(var so in aQ(qx)){var jb=so.Split(new[]{'='},2);string nQ=hk(jb[0]);kn.e6.Add(nQ);if(jb.Length>1)kn.g6[nQ]=ah(jb[1].Trim());
}return kn;}static void kR(string r7,Dictionary<string,bD>oS,List<string>nM,List<string>rh,int mq=0){string go=r7.Substring("@use".Length).Trim();
int gc=go.IndexOf('(');string jo=gc>=0 ? go.Substring(0,gc).Trim(): go.Trim();string qy=gc>=0 ? aO(go,gc): "";bD df=null;
if(!oS.TryGetValue(jo,out df)){if(!kN.TryGetValue(jo,out df))throw new FormatException($"unknown template '{jo}'");}var fw=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var no in df.g6)fw[no.Key]=no.Value;var lG=aQ(qy);for(int dU=0;dU<lG.Count;dU++){string fl=lG[dU];int it=fl.IndexOf('=');if(it>=0){
string rQ=hk(fl.Substring(0,it));string tF=ah(fl.Substring(it+1).Trim());fw[rQ]=tF;}else{if(dU<df.e6.Count){string rR=df.e6[dU];
fw[rR]=ah(fl.Trim());}}}foreach(var qT in df.g0){string d4=qT;foreach(var f1 in fw){d4=d4.Replace("$"+f1.Key,f1.Value).Replace("{"+f1.Key+"}",f1.Value);
}if(d4.StartsWith("@use")){if(mq>3)throw new FormatException("@use nested too deep");kR(d4,oS,nM,rh,mq+1);
}else nM.Add(d4);}}static void p(D q6,string re){if(q6==null)throw new FormatException($"'{re}' used before any @screen declared");
}static string lj(string a7){if(string.IsNullOrEmpty(a7))return a7;bool i_=false;for(int bN=0;bN<a7.Length;bN++){char h8=a7[bN];if(h8=='"'){i_= !i_;
}else if(!i_){if(h8=='/'&&bN+1<a7.Length&&a7[bN+1]=='/'){return a7.Substring(0,bN);}if(h8=='#'){if(pH(a7,bN)){continue;}return a7.Substring(0,bN);}}}return a7;
}static bool pH(string cs,int ne){int c1=0;for(int iW=ne+1;iW<cs.Length;iW++){char cQ=cs[iW];if((cQ>='0'&&cQ<='9')||(cQ>='a'&&cQ<='f')||(cQ>='A'&&cQ<='F'))c1++;
else break;}if(c1==3||c1==4||c1==6||c1==8){int en=ne+1+c1;if(en>=cs.Length||char.IsWhiteSpace(cs[en])||cs[en]==')'||cs[en]==','||cs[en]==':')return true;
}return false;}static be pQ(string r8){string jT=r8.Substring("@bind".Length).Trim();int iu=jT.IndexOf('=');if(iu<0)throw new FormatException("missing '=' in @bind");
string nH=jT.Substring(0,iu).Trim();string iw=jT.Substring(iu+1).Trim();int jF=iw.IndexOf('(');if(jF<0)throw new FormatException("missing '(' in @bind target");
string fM=iw.Substring(0,jF).Trim().ToLowerInvariant();string qz=aO(iw,jF);var bH=aQ(qz);if(bH.Count==0)throw new FormatException("@bind target has no arguments");
var ap=new be{ab=nH};if(fM=="dynamic"||fM=="var"||fM=="mem"){ap.bw=aZ.dt;ap.bf=bH.Count>0 ? ah(bH[0].Trim()): nH;
for(int hH=0;hH<bH.Count;hH++){var f2=bH[hH].Split(new[]{':'},2);if(f2.Length==2&&f2[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double mp;
if(double.TryParse(f2[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mp))ap.eX=mp;else ap.g5=ah(f2[1].Trim());
}}return ap;}ap.bw=fM=="group" ? aZ.hf : aZ.pf;string fj=null;for(int fi=0;fi<bH.Count;fi++){var c0=bH[fi].Split(new[]{':'},2);if(c0.Length==2&&c0[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
fj=c0[1].Trim().ToLowerInvariant();}else if(c0.Length==2&&c0[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ap.eQ=c0[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ap.aR.Add(ah(bH[fi].Trim()));}}if(ap.aR.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ap.bw==aZ.hf){if(fj==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(fj){case "avg": ap.dj=aD.gV;break;case "sum": ap.dj=aD.ll;break;case "min": ap.dj=aD.la;break;case "max": ap.dj=aD.k9;
break;default: throw new FormatException($"unknown agg '{fj}' (expected avg/sum/min/max)");}}return ap;}static bx pS(string fs){var o=new bx();
if(!fs.StartsWith("\""))throw new FormatException("line must start with a quoted label");int ir=fs.IndexOf('"',1);if(ir<0)throw new FormatException("unterminated quoted label");
o.V=fs.Substring(1,ir-1);string ew=fs.Substring(ir+1).Trim();int gd=ew.IndexOf('(');if(gd<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string z=ew.Substring(0,gd).Trim().ToLowerInvariant();string qv=aO(ew,gd);string ae=ew.Substring(ew.IndexOf(')',gd)+1).Trim();
var im=qv.Trim().Split('.');if(im.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.aM=im[0].Trim();o.I=im[1].Trim();if(z=="bar")o.g=e.dl;else if(z=="led")o.g=e.dy;else if(z=="spark"||z=="sparkline")o.g=e.cc;
else if(z=="hazard"||z=="stripes")o.g=e.dx;else if(z=="chevron"||z=="chevrons")o.g=e.dq;else if(z=="dots"||z=="dot")o.g=e.ds;
else if(z=="balance"||z=="horizon"||z=="centered")o.g=e.cD;else if(z=="toggle"||z=="switch")o.g=e.cf;else if(z=="checkbox"||z=="check")o.g=e.b4;
else if(z=="status"||z=="badge"||z=="state")o.g=e.cd;else if(z=="ammo"||z=="pips")o.g=e.cB;else if(z=="flow"||z=="rate"||z=="spinner")o.g=e.b6;
else if(z=="text")o.g=e.hs;else if(z=="tank"||z=="tube"||z=="bucket")o.g=e.fa;else throw new FormatException($"unknown display function '{z}'");
while(ae.Length>0){int ik=ae.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);int cW=ae.IndexOf('(');if(ik>=0&&(cW<0||ik<cW)){
string fB=ae.Substring(ik+4).Trim();int gw=fB.IndexOf(' ');o.kP=(gw>=0 ? fB.Substring(0,gw): fB).ToLowerInvariant();ae=(gw>=0 ? fB.Substring(gw): "").Trim();
continue;}int ku=ae.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(ku>=0&&(cW<0||ku<cW)){string gQ=ae.Substring(ku+5).Trim();
int gx=gQ.IndexOf(' ');o.dH=(gx>=0 ? gQ.Substring(0,gx): gQ).Trim().Trim('"');ae=(gx>=0 ? gQ.Substring(gx): "").Trim();continue;}int iF=ae.IndexOf("format:",StringComparison.OrdinalIgnoreCase);
if(iF>=0&&(cW<0||iF<cW)){string fG=ae.Substring(iF+7).Trim();int gy=fG.IndexOf(' ');o.du=(gy>=0 ? fG.Substring(0,gy): fG).Trim().Trim('"');
ae=(gy>=0 ? fG.Substring(gy): "").Trim();continue;}int f7=ae.IndexOf('(');if(f7<0)break;string au=ae.Substring(0,f7).Trim().ToLowerInvariant();
int nu=au.LastIndexOf(' ');if(nu>=0)au=au.Substring(nu+1).Trim();string bl=aO(ae,f7);ae=ae.Substring(ae.IndexOf(')',f7)+1).Trim();
if(au=="gradient"){o.b7=true;o.a2=aQ(bl).Select(s2=>s2.Trim()).ToArray();if(o.a2.Length<2)throw new FormatException("gradient() needs at least 2 colors");
}else if(au=="flat"){o.ar=bl.Trim();}else if(au=="pattern"){string Q=bl.Trim().ToLowerInvariant();if(Q=="hazard"||Q=="stripes")o.g=e.dx;
else if(Q=="chevron"||Q=="chevrons")o.g=e.dq;else if(Q=="dots"||Q=="dot")o.g=e.ds;else if(Q=="led")o.g=e.dy;
else if(Q=="balance"||Q=="horizon")o.g=e.cD;else if(Q=="toggle"||Q=="switch")o.g=e.cf;else if(Q=="checkbox"||Q=="check")o.g=e.b4;
else if(Q=="status"||Q=="badge"||Q=="state")o.g=e.cd;else if(Q=="ammo"||Q=="pips")o.g=e.cB;else if(Q=="flow"||Q=="rate"||Q=="spinner")o.g=e.b6;
}else if(au=="color"){string rJ;o.bu=ld(bl,out rJ);}else if(au=="dir"||au=="direction"){o.kP=bl.Trim().ToLowerInvariant();
}else if(au=="unit"){o.dH=bl.Trim().Trim('"');}else if(au=="max"){double nC;if(double.TryParse(bl.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out nC))o.e5=nC;
else{var jl=bl.Trim().Split('.');if(jl.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.cI=jl[0].Trim();o.hi=jl[1].Trim();
}}else if(au=="format"||au=="fmt"){o.du=bl.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{au}'");}}return o;}static string aO(string gs,int jt){
int ij=0;for(int ee=jt;ee<gs.Length;ee++){if(gs[ee]=='(')ij++;else if(gs[ee]==')'){ij--;if(ij==0)return gs.Substring(jt+1,ee-jt-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aQ(string qA){return qA.Split(',').Select(s3=>s3.Trim()).Where(s4=>s4.Length>0).ToList();
}static List<aN>ld(string qu,out string iB){var gq=new List<aN>();iB=null;foreach(var jG in aQ(qu)){int ig=jG.LastIndexOf(':');
if(ig<0)throw new FormatException("color() entries need 'expr : color'");string f3=jG.Substring(0,ig).Trim();string mi=jG.Substring(ig+1).Trim();
if(f3.Equals("default",StringComparison.OrdinalIgnoreCase)){gq.Add(new aN{cG=true,b5=mi});}else{string oz=lk(f3);
if(iB==null)iB=f3.Substring(0,f3.Length-oz.Length).Trim();gq.Add(new aN{g7=oz,b5=mi});}}if(!gq.Any(sR=>sR.cG))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return gq;}static void li(string sS,out string qQ,out string rn){var eq=(sS ?? "").Split('.');if(eq.Length!=2||eq[0].Trim().Length==0||eq[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
qQ=eq[0].Trim();rn=eq[1].Trim();}static string lk(string cV){cV=cV.Trim();foreach(var sl in new[]{">=","<=","==",">","<"}){int iX=cV.IndexOf(sl,StringComparison.Ordinal);
if(iX>0)return cV.Substring(iX).Trim();if(iX==0)return cV;}throw new FormatException($"color() rule has no recognizable comparison operator: '{cV}'");}
static string ah(string ct){if(ct.Length>=2&&ct[0]=='"'&&ct[ct.Length-1]=='"')return ct.Substring(1,ct.Length-2);return ct;}}public static class pd{public static double pc(aD qq,List<double>dh){
if(dh==null||dh.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(qq){case aD.gV: return dh.Average();case aD.ll: return dh.Sum();
case aD.la: return dh.Min();case aD.k9: return dh.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class H{
public static string p3(string r_,double gf,int lP=10,string o_=null){gf=Math.Max(0,Math.Min(100,gf));int mO=(int)Math.Round(lP*(gf/100.0));
string qL=new string('#',mO)+new string('-',lP-mO);string tG=o_!=null ? o_ : $"{gf:0}%";return $"{r_} [{qL}] {tG}";
}public static string kX(double bY,string kt=null,string fL=null){if(kt=="auto"){double hI=Math.Abs(bY);if(hI>=1e9)return(bY/1e9).ToString("F1")+"G";
if(hI>=1e6)return(bY/1e6).ToString("F1")+"M";if(hI>=1e3)return(bY/1e3).ToString("F1")+"k";string rl= !string.IsNullOrEmpty(fL)? fL : "F0";return bY.ToString(rl);
}string mQ= !string.IsNullOrEmpty(fL)? fL :(Math.Abs(bY)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(kt)){return bY.ToString(mQ);}return bY.ToString(mQ)+kt;
}public static float pD(string sH,float oB,float kk,float fC=1.0f){string c4=(sH ?? "standard").ToLowerInvariant();
if(c4=="compact"||(c4=="auto"&&oB<=300&&kk<=300&&kk>160)){return fC*0.75f;}if(c4=="corner"||(c4=="auto"&&kk<=160)){
return fC*0.55f;}if(c4=="wide"||(c4=="auto"&&oB>=1000)){return fC*1.15f;}return fC;}public static string p1(string r0,double gg,int bL=5){
gg=Math.Max(0,Math.Min(100,gg));double fD=(gg-50.0)/50.0;int el=0;int ez=0;if(fD< -0.01)el=(int)Math.Round(-fD*bL);
else if(fD>0.01)ez=(int)Math.Round(fD*bL);if(el>bL)el=bL;if(ez>bL)ez=bL;string r3=new string('-',bL-el)+new string('<',el);
string sX=new string('>',ez)+new string('-',bL-ez);return $"{r0} [{r3}|{sX}] {gg:0}%";}public static string qc(string nr,double tI){
return tI>=50.0 ? $"{nr} [====O] ON" : $"{nr} [O====] OFF";}public static string p6(string ns,double tJ){return tJ>=50.0 ? $"{ns} [X] ACTIVE" : $"{ns} [ ] READY ";
}public static string qb(string jd,double oY){if(oY>=75.0)return $"{jd} [ ONLINE  ]";if(oY>=25.0)return $"{jd} [ STANDBY ]";return $"{jd} [ OFFLINE ]";
}public static string p0(string r1,double gh,int ny=8){gh=Math.Max(0,Math.Min(100,gh));int mP=(int)Math.Round(ny*(gh/100.0));
string sC=new string('|',mP)+new string('.',ny-mP);return $"{r1} [{sC}] {gh:0}%";}public static string p7(string je,double eu,int oN=0){
char[]ti=new[]{'|','/','-','\\'};char oe=ti[(oN>=0 ? oN : 0)%4];if(eu>55.0)return $"{je} {oe} ▲ CHG {eu:0}%";if(eu<45.0)return $"{je} {oe} ▼ DSC {eu:0}%";
return $"{je} ● STBY {eu:0}%";}public static bool pL(string av,string eh){if(av==null||eh==null)return false;if(av=="*"||av==eh)return true;
if(!av.Contains("*")&& !av.Contains("?"))return string.Equals(av,eh,StringComparison.OrdinalIgnoreCase);int az=0,ef=0,kc= -1,jk=0;while(ef<eh.Length){
if(az<av.Length&&(av[az]=='?'||char.ToLowerInvariant(av[az])==char.ToLowerInvariant(eh[ef]))){az++;ef++;}else if(az<av.Length&&av[az]=='*'){
kc=az;jk=ef;az++;}else if(kc!= -1){az=kc+1;jk++;ef=jk;}else{return false;}}while(az<av.Length&&av[az]=='*')az++;return az==av.Length;
}public static int ph(double sp,int oQ){return Math.Max(0,Math.Min(oQ,1+(int)(Math.Max(0.0,sp)*oQ)));}public static string e_(string[]de,double jH){
if(de==null||de.Length==0)return "white";jH=Math.Max(0,Math.Min(100,jH));int iY=(int)Math.Floor(jH/100.0*de.Length);if(iY>=de.Length)iY=de.Length-1;
return de[iY];}public static n k_(string[]bW,double sy){if(bW==null||bW.Length==0)return n.fb;if(bW.Length==1)return n.dA(bW[0]);
double sV=Math.Max(0.0,Math.Min(1.0,sy/100.0));double oi=sV*(bW.Length-1);int iZ=(int)Math.Floor(oi);int rI=Math.Min(bW.Length-1,iZ+1);
double kg=oi-iZ;n cR=n.dA(bW[iZ]);n h9=n.dA(bW[rI]);byte sT=(byte)Math.Round(cR.bA+(h9.bA-cR.bA)*kg);
byte rB=(byte)Math.Round(cR.bv+(h9.bv-cR.bv)*kg);byte qJ=(byte)Math.Round(cR.bq+(h9.bq-cR.bq)*kg);return new n(sT,rB,qJ);}public static string dB(List<aN>oa,double tK){
foreach(var jV in oa){if(jV.cG)continue;if(pz(jV.g7,tK))return jV.b5;}var mo=oa.FirstOrDefault(sU=>sU.cG);return mo!=null ? mo.b5 : null;
}static bool pz(string d5,double eL){d5=d5.Trim();foreach(var js in new[]{">=","<=","==",">","<"}){if(d5.StartsWith(js)){double ey=double.Parse(d5.Substring(js.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(js){case ">=": return eL>=ey;case "<=": return eL<=ey;case "==": return Math.Abs(eL-ey)<1e-9;case ">": return eL>ey;case "<": return eL<ey;
}}}throw new FormatException($"unsupported comparison expression: '{d5}'");}public static List<string>p5(List<string>q5,int di){
var f9=new List<string>();f9.Add("+"+new string('-',di+2)+"+");foreach(var jh in q5){string tB=jh.Length>di ? jh.Substring(0,di): jh;
string ss=tB.PadRight(di);f9.Add("| "+ss+" |");}f9.Add("+"+new string('-',di+2)+"+");return f9;}public struct lc{public float ls,lt,lr,k1;
}public static lc pn(int mf,int n7,int gz,int gA,int eG,int eH,float ts,float tr,float nR,float nS,float iM,float iN){
eG=Math.Max(1,eG);eH=Math.Max(1,eH);gz=Math.Max(1,Math.Min(eG-mf,gz));gA=Math.Max(1,Math.Min(eH-n7,gA));
float qC=ts-(nR*2f);float qB=tr-(nS*2f);float ma=(qC-(iM*(eG-1)))/eG;float l9=(qB-(iN*(eH-1)))/eH;
float sJ=nR+mf*(ma+iM);float sK=nS+n7*(l9+iN);float sI=gz*ma+((gz-1)*iM);float sA=gA*l9+((gA-1)*iN);return new lc{
ls=sJ,lt=sK,lr=sI,k1=sA};}public struct lg{public int pO;public int hh;public int ac;public int ax;public int dG;
public int a_;public int qi;public int qj;public int pw;public int px;
public double py;}public static lg po(D bU){var l=new lg();var nU=bU.ag!=null&&bU.ag.Count>0 ? bU.ag : new List<ay>{
new ay{at=bU.e4}};l.pO=nU.Count;l.hh=0;l.ac=0;l.ax=0;if(!string.IsNullOrEmpty(bU.br)&& !bU.br.Equals("none",StringComparison.OrdinalIgnoreCase))l.ac++;
if(!string.IsNullOrEmpty(bU.dp)&& !bU.dp.Equals("none",StringComparison.OrdinalIgnoreCase))l.ac+=2;foreach(var er in nU){
if(!string.IsNullOrEmpty(er.cC)&& !er.cC.Equals("none",StringComparison.OrdinalIgnoreCase))l.ac++;if(!string.IsNullOrEmpty(er.ce))l.ac+=2;
l.hh+=er.at.Count;foreach(var rZ in er.at){l.qi++;l.qj++;l.ac+=2;switch(rZ.g){case e.dl: l.dG++;
l.a_++;l.ac+=2;l.ax++;break;case e.dy: l.a_+=10;l.ac+=10;
l.ax+=10;break;case e.dx: l.a_+=10;l.ac+=11;l.ax+=10;break;case e.dq: l.a_+=10;
l.ac+=10;l.ax+=10;break;case e.ds: l.a_+=12;l.ac+=12;l.ax+=12;
break;case e.cc: l.dG++;l.a_+=8;l.ac+=9;l.ax+=8;break;case e.cD: l.dG+=2;
l.a_++;l.ac+=3;l.ax++;break;case e.cf: case e.b4: l.dG++;
l.a_++;l.ac+=2;l.ax++;break;case e.cd: case e.b6: l.dG++;l.a_+=2;
l.ac+=3;l.ax+=2;break;case e.cB: l.a_+=8;l.ac+=8;l.ax+=8;
break;default: l.ax++;break;}}}l.pw=25+(l.hh*2);l.px=180+(l.ax*4);
l.py=0.0;return l;}}