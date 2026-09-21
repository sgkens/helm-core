gX q;string hb=null;Dictionary<string,List<IMyTerminalBlock>>b2=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>eW=new Dictionary<string,bool>();
Dictionary<string,a_>dr=new Dictionary<string,a_>();Dictionary<string,Color>kW=new Dictionary<string,Color>();List<IMyTerminalBlock>b_=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>cr=new List<IMyTerminalBlock>();bool eT;int g8;string ds="";StringBuilder dw=new StringBuilder();readonly Dictionary<string,double>aB=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>dt=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener du;bool b0=false;
Dictionary<y,IMyTextSurface>br=new Dictionary<y,IMyTextSurface>();Dictionary<y,string>b1=new Dictionary<y,string>();
static readonly string[]kE=new string[101];static Program(){for(int fr=0;fr<=100;fr++){kE[fr]=fr+"%";}}static string kp(double sL){int dX=(int)Math.Round(sL);
if(dX<0)dX=0;else if(dX>100)dX=100;return kE[dX];}class a3{public List<MySprite>l=new List<MySprite>();public List<dh>eQ=new List<dh>();
public int bN= -1;public Color[]ex;public Color es;public bool et;public Vector2 ky;public bool kv=false;public List<ey>c5=new List<ey>();
}class ey{public int eH;public Color gv,eC;public List<aG>eP,eF;public string gz,gI,gJ,gL;public bool kw,gN;
}class dh{public bl kA;public int ab= -1;public int bq= -1;public int gR= -1;public bool kr=false;
public List<int>ah=new List<int>();public float[]eR=new float[8];public float c3;public float ew;public float ck;
public float c2;public double eJ= -99999.0;public string kz;public float Q= -1f;}Dictionary<y,a3>dy=new Dictionary<y,a3>();
int kY=0;double hd=0;double dx=0;double ai=0;int dv=0;Dictionary<y,double>a4=new Dictionary<y,double>();
Dictionary<y,double>g7=new Dictionary<y,double>();Dictionary<string,int>dB=new Dictionary<string,int>();List<string>cs=new List<string>();
Dictionary<string,double>dA=new Dictionary<string,double>();List<MyInventoryItem>ha=new List<MyInventoryItem>();int bs= -1,eY;double he;
Dictionary<y,double>eU=new Dictionary<y,double>();int dz=0;const string oX="0.11.0";IMyTextSurface aS;bool g6=true;
double eV=0;string kX="";string hc="";int eX=0,g9=0;List<KeyValuePair<string,int>>cq=new List<KeyValuePair<string,int>>();
const double pe=120;const float os=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{du=IGC.RegisterBroadcastListener("HELM");
du.SetMessageCallback("HELM_IGC");}catch{}eN();g0();ku();}public void Main(string k9,UpdateType sH){
double lX=Runtime.TimeSinceLastRun.TotalSeconds;ai+=lX;if((sH&UpdateType.IGC)!=0||k9=="HELM_IGC"){pc();}string np=(k9 ?? "").Trim();
string aj=np.ToLowerInvariant();if(aj=="reload"){eN();}else if(aj=="refresh"){gZ();}else if(aj=="boot"){g0();}else if(aj=="probe"){
dx=ai+20.0;oL();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(aj=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=py;
eN();g0();}else if(aj=="console on"||aj=="console off"){g6=aj=="console on";ku();eV=0;}else if(aj=="scan"||aj=="diagnose"||aj=="test"){
pw();return;}else if(aj.StartsWith("set ")){string fY=np.Substring(4).Trim();int l0=fY.IndexOf('=');int sj=fY.IndexOf(' ');int jC=l0>0 ? l0 : sj;
if(jC>0){string mF=fY.Substring(0,jC).Trim();string oa=fY.Substring(jC+1).Trim();j8($"{mF}={oa}");Echo($"[HELM] Set dynamic variable '{mF}' = {oa}");
}return;}else if(aj.StartsWith("alert")){string ep=aj.Length>5 ? aj.Substring(5).Trim(): "toggle";if(ep=="toggle")b0= !b0;else b0=ep=="red"||ep=="on"||ep=="1"||ep=="true";
Echo($"[HELM] Red alert state: {b0}");return;}else if(Me.CustomData!=hb){eN();}dz++;hd+=lX;if(hd>=pe){
hd=0;gZ();}if(dx>0){if(ai<dx){g5();return;}dx=0;dy.Clear();b1.Clear();
eV=0;}if(q==null||q.S.Count==0){g5();kh();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cb=q.S[kY%q.S.Count];
kY++;hc=cb.a1;bool hi=a4.Count>0;if(hi)pl();double lY;if(!a4.ContainsKey(cb)&& !(g7.TryGetValue(cb,out lY)&&ai<lY)){
if(pp(cb))hi=true;if(cb.gY>0)g7[cb]=ai+cb.gY/60.0;}dv=hi ? 0 : dv+1;Runtime.UpdateFrequency=dv<=q.S.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
px();g5();kh();eX=Runtime.CurrentInstructionCount;if(eX>g9)g9=eX;}void kh(){if(ds.Length>0)Echo(ds);
if(dw.Length>0)Echo(dw.ToString());}List<IMyTerminalBlock>eu(bool pG=false){if(!eT){cr.Clear();GridTerminalSystem.GetBlocks(cr);
b_.Clear();b_.AddRange(cr);b_.RemoveAll(pS=> !pS.IsSameConstructAs(Me));g8=cr.Count-b_.Count;eT=true;
}return pG ? cr : b_;}IMyTerminalBlock kk(string m0,bool pI=false){var hr=GridTerminalSystem.GetBlockWithName(m0);if(hr==null||pI||hr.IsSameConstructAs(Me))return hr;
var hk=eu();for(int fs=0;fs<hk.Count;fs++)if(hk[fs].CustomName.IndexOf(m0,StringComparison.OrdinalIgnoreCase)>=0)return hk[fs];return null;}void oC(string m1,List<IMyTerminalBlock>dZ,bool k2=false){
var mh=GridTerminalSystem.GetBlockGroupWithName(m1);if(mh==null)return;int lj=dZ.Count;var el=new List<IMyTerminalBlock>();mh.GetBlocks(el);j6(el,dZ,k2);
if(dZ.Count>lj)return;var ms=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(ms);foreach(var ol in ms){if(ol.Name.IndexOf(m1,StringComparison.OrdinalIgnoreCase)<0)continue;
el.Clear();ol.GetBlocks(el);j6(el,dZ,k2);if(dZ.Count>lj)return;}}void j6(List<IMyTerminalBlock>qJ,List<IMyTerminalBlock>mD,bool pH=false){
foreach(var hs in qJ)if((pH||hs.IsSameConstructAs(Me))&& !mD.Contains(hs))mD.Add(hs);}void eN(){hb=Me.CustomData;q=kt.o5(hb);
ds="";if(q.aP.Count>0){var nz=new StringBuilder("HELM config errors:\n");foreach(var qt in q.aP)nz.AppendLine(" - "+qt);ds=nz.ToString();
}if(q.dq.Count>0){var ok=new StringBuilder("HELM notes:\n");foreach(var sQ in q.dq)ok.AppendLine(" - "+sQ);ds+=ok.ToString();}dr.Clear();
foreach(var ht in q.bO)if(!dr.ContainsKey(ht.V))dr[ht.V]=ht;dy.Clear();b1.Clear();a4.Clear();g7.Clear();
eU.Clear();oR();gZ();}void pc(){try{while(du!=null&&du.HasPendingMessage){
var mX=du.AcceptMessage();if(mX.Data is string){j8((string)mX.Data);}}}catch{}}void j8(string nh){
if(string.IsNullOrEmpty(nh))return;var ne=nh.Split(';');for(int il=0;il<ne.Length;il++){var d9=ne[il].Trim();if(d9.Length==0)continue;int dM=d9.IndexOf('=');
if(dM<0)dM=d9.IndexOf(':');if(dM>0){string cH=d9.Substring(0,dM).Trim();string aZ=d9.Substring(dM+1).Trim();if(cH.Equals("alert",StringComparison.OrdinalIgnoreCase)){
b0=aZ.Equals("red",StringComparison.OrdinalIgnoreCase)||aZ=="1"||aZ.Equals("true",StringComparison.OrdinalIgnoreCase)||aZ.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double m4;dt.Remove(cH);if(double.TryParse(aZ,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out m4)){
aB[cH]=m4;}else if(aZ.Equals("true",StringComparison.OrdinalIgnoreCase)||aZ.Equals("on",StringComparison.OrdinalIgnoreCase)){aB[cH]=100.0;
}else if(aZ.Equals("false",StringComparison.OrdinalIgnoreCase)||aZ.Equals("off",StringComparison.OrdinalIgnoreCase)){aB[cH]=0.0;}else{dt[cH]=aZ;
}}}}}const string py="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void oL(){var hl=new List<IMyTextSurface>();if(aS!=null)hl.Add(aS);foreach(var rt in br.Values)hl.Add(rt);foreach(var sf in hl)oK(sf);
}static void a2(List<MySprite>sk,float qn,float qo,float sR,float qQ,Color qc){sk.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=new Vector2(qn,qo),Size=new Vector2(sR,qQ),Color=qc}
);}void oK(IMyTextSurface jD){Vector2 aD=jD.SurfaceSize,ax=jD.TextureSize,ag=(ax-aD)/2f;var aw=new List<MySprite>();Color fo=new Color(150,150,150);
a2(aw,ax.X/2f,2f,ax.X,4f,fo);a2(aw,ax.X/2f,ax.Y-2f,ax.X,4f,fo);a2(aw,2f,ax.Y/2f,4f,ax.Y,fo);a2(aw,ax.X-2f,ax.Y/2f,4f,ax.Y,fo);
a2(aw,ag.X+aD.X/2f,ag.Y+aD.Y/2f,aD.X,aD.Y,new Color(20,40,90));float af=32f;a2(aw,ag.X+af/2f,ag.Y+af/2f,af,af,new Color(255,0,0));a2(aw,ag.X+aD.X-af/2f,ag.Y+af/2f,af,af,new Color(0,255,0));
a2(aw,ag.X+af/2f,ag.Y+aD.Y-af/2f,af,af,new Color(255,255,0));a2(aw,ag.X+aD.X-af/2f,ag.Y+aD.Y-af/2f,af,af,new Color(255,0,255));float j4=ag.Y+aD.Y/2f-30f;
bc(aw,"SURFACE "+aD.X.ToString("0")+"x"+aD.Y.ToString("0"),Color.White,ag.X+44f,j4,0.9f);bc(aw,"TEXTURE "+ax.X.ToString("0")+"x"+ax.Y.ToString("0"),Color.White,ag.X+44f,j4+24f,0.9f);
bc(aw,"OFFSET  "+ag.X.ToString("0")+","+ag.Y.ToString("0"),Color.White,ag.X+44f,j4+48f,0.9f);using(var qD=jD.DrawFrame()){qD.AddRange(aw);}}static bool kx(string h0){
return h0.Length>3&&h0.StartsWith("top")&&char.IsDigit(h0[3]);}void oR(){dB.Clear();cs.Clear();bs= -1;he=0;foreach(var jy in q.S){
var iQ=new List<List<bl>>();foreach(var rK in jy.ac)iQ.Add(rK.aQ);if(jy.ac.Count==0)iQ.Add(jy.eK);foreach(var rp in iQ)foreach(var fH in rp){
if(!kx(fH.F))continue;int rq=int.Parse(fH.F.Substring(3));int mw;dB.TryGetValue(fH.aF,out mw);dB[fH.aF]=Math.Max(mw,rq);}}cs.AddRange(dB.Keys);
}void px(){if(cs.Count==0)return;if(bs<0){if(ai<he)return;bs=0;eY=0;dA.Clear();}while(bs<cs.Count&&Runtime.CurrentInstructionCount<20000){
string hB=cs[bs];List<IMyTerminalBlock>e7;b2.TryGetValue(hB,out e7);if(e7!=null&&eY<e7.Count){var lb=e7[eY++];
for(int im=0;im<lb.InventoryCount;im++){ha.Clear();lb.GetInventory(im).GetItems(ha);foreach(var fz in ha){string iE=fz.Type.TypeId.Replace("MyObjectBuilder_","");
string mG=iE=="Ore"||iE=="Ingot" ? fz.Type.SubtypeId+" "+iE.ToLowerInvariant(): fz.Type.SubtypeId;double mx;dA.TryGetValue(mG,out mx);dA[mG]=mx+(double)fz.Amount;
}}continue;}var fW=new List<KeyValuePair<string,double>>(dA);fW.Sort((sT,sX)=>sX.Value.CompareTo(sT.Value));for(int cO=1;cO<=dB[hB];cO++)dt[hB+".top"+cO]=cO<=fW.Count ? fW[cO-1].Key+" "+D.ko(fW[cO-1].Value,"auto"): "-";
dA.Clear();bs++;eY=0;}if(bs>=cs.Count){bs= -1;he=ai+15.0;}}void aa(int st,string sA){cq.Add(new KeyValuePair<string,int>(sA,st));
}static string g4(string jr,int m_){return jr.Length>m_ ? jr.Substring(0,m_): jr;}void ku(){var jj=Me as IMyTextSurfaceProvider;if(jj==null||jj.SurfaceCount<1)return;
aS=jj.GetSurface(0);aS.ContentType=g6 ? ContentType.SCRIPT : ContentType.NONE;aS.Script="";}void g5(){if(ai>=eV){
eV=ai+(dv>q.S.Count+1 ? 5.0 : 1.0);oA();if(g6&&aS!=null&&dx==0)oJ();}Echo(kX);
}void oA(){cq.Clear();int hE=0,lp=0;foreach(var lc in q.bO){List<IMyTerminalBlock>iK;if(lc.bk==aN.db){hE++;continue;
}if(b2.TryGetValue(lc.V,out iK)){lp+=iK.Count;if(iK.Count>0)hE++;}}int l1=q.aP.Count;aa(0,"HELM-[Kernel-(v"+oX+")]");
aa(4,"------------------------------");aa(1,"krn-cfg");aa(2," screens "+q.S.Count+"  binds "+hE+"/"+q.bO.Count+"  errors "+l1);
aa(2," alert "+(b0 ? "RED" : "off")+"  blocks "+lp+"  docked-ignored "+g8);if(l1>0)aa(3," ! "+g4(q.aP[0],40));
else if(q.dq.Count>0)aa(3," ? "+g4(q.dq[0],40));aa(1,"cfg-dsp");if(q.S.Count==0)aa(3," no @screen: run 'starter'");
int sg=0;foreach(var js in q.S){if(++sg>10){aa(2," +"+(q.S.Count-10)+" more");break;}IMyTextSurface jE;string nI="--",jJ="no block";
if(br.TryGetValue(js,out jE)){nI=jE.SurfaceSize.X.ToString("0")+"x"+jE.SurfaceSize.Y.ToString("0");jJ=a4.ContainsKey(js)? "boot" : "live";
}aa(jJ=="no block" ? 3 : 2," "+g4(js.a1,14).PadRight(14)+" "+nI.PadRight(9)+" "+jJ);}aa(1,"perf");aa(2," tick "+dz+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
aa(2," instr "+eX+"  peak "+g9+"  max "+Runtime.MaxInstructionCount);aa(2," rate "+Runtime.UpdateFrequency+"  idle "+dv+"  up "+(int)ai+"s");
if(hc.Length>0)aa(2," last "+hc);var nA=new StringBuilder();foreach(var q4 in cq)nA.AppendLine(q4.Key);kX=nA.ToString();}static Color oF(int su){
switch(su){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void oJ(){Vector2 cc=aS.SurfaceSize;Vector2 iX=(aS.TextureSize-cc)/2f;int hL=30;foreach(var q5 in cq)hL=Math.Max(hL,q5.Key.Length);
Vector2 hK=aS.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float nB=Math.Min(1f,Math.Min(cc.X*0.94f/(hL*hK.X),cc.Y*0.94f/(cq.Count*hK.Y)));
float sU=iX.X+cc.X*0.03f;float oo=iX.Y+cc.Y*0.03f;var jF=new List<MySprite>();jF.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iX+cc/2f,Size=cc,Color=new Color(12,12,16)}
);foreach(var mL in cq){bc(jF,mL.Key,oF(mL.Value),sU,oo,nB);oo+=hK.Y*nB;}using(var qE=aS.DrawFrame()){qE.AddRange(jF);}}static readonly List<string>oz=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void g0(){a4.Clear();if(q==null)return;for(int ft=0;ft<q.S.Count;ft++){var f1=q.S[ft];if(f1.gC>0f)a4[f1]=ai+(ft/Math.Max(1,f1.ka))*f1.kd/60.0;
}}void pl(){List<y>dL=null;int qP=(a4.Count+3)/4,qY=0;foreach(var fC in a4){double i4=(ai-fC.Value)/fC.Key.gC;if(i4<0)continue;
if(i4>=1.0){if(dL==null)dL=new List<y>();dL.Add(fC.Key);continue;}if((qY++ +dz)%qP==0)oI(fC.Key,i4);}if(dL==null)return;foreach(var eh in dL){
a4.Remove(eh);if(eh.gB>0f)eU[eh]=ai;dy.Remove(eh);b1.Remove(eh);}}static void bc(List<MySprite>sl,string sB,Color qd,float sV,float sY,float sc){
var nX=MySprite.CreateText(sB,"Monospace",qd,sc,TextAlignment.LEFT);nX.Position=new Vector2(sV,sY);sl.Add(nX);}static void j5(List<MySprite>sm,List<string>e2,y f4,float sW,float sZ,float sd,float rg){
string[]sr=f4.ez!=null&&f4.ez.Length>0 ? f4.ez : new[]{f4.kb};for(int dS=0;dS<e2.Count;
dS++){n hF=D.kq(sr,e2.Count>1 ? 100.0*dS/(e2.Count-1): 0.0);bc(sm,e2[dS],new Color(hF.bn,hF.bi,hF.bd),sW,sZ+dS*rg,sd);}}bool pj(y cQ,IMyTextSurface gb){
List<string>dD;if(b1.ContainsKey(cQ)|| !q.c0.TryGetValue(cQ.bM,out dD))return false;b1[cQ]="banner";Vector2 cd=gb.SurfaceSize;
Vector2 iY=(gb.TextureSize-cd)/2f;int fc=1;foreach(var q6 in dD)fc=Math.Max(fc,q6.Length);Vector2 dJ=gb.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float f3=Math.Min(cd.X*0.94f/(fc*dJ.X),cd.Y*0.94f/(dD.Count*dJ.Y));var jG=new List<MySprite>();jG.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iY+cd/2f,Size=cd,Color=string.IsNullOrEmpty(cQ.be)? new Color(10,12,16): E(cQ.be)}
);j5(jG,dD,cQ,iY.X+(cd.X-fc*dJ.X*f3)/2f,iY.Y+(cd.Y-dD.Count*dJ.Y*f3)/2f,f3,dJ.Y*f3);using(var qF=gb.DrawFrame()){qF.AddRange(jG);
}return true;}void oI(y cR,double m9){IMyTextSurface ej;if(!br.TryGetValue(cR,out ej))return;Vector2 bI=ej.SurfaceSize;
Vector2 iZ=(ej.TextureSize-bI)/2f;var d2=cR.oS();string mY=cR.kc ?? "HELM TELEMETRY KERNEL ONLINE";bool iR=bI.Y>=200f;
List<string>ct;if(cR.bP==null|| !q.c0.TryGetValue(cR.bP,out ct))ct=oz;int cA=mY.Length+3;foreach(var q7 in d2)cA=Math.Max(cA,q7.Length+5);
if(iR)foreach(var q8 in ct)cA=Math.Max(cA,q8.Length);int ns=(iR ? ct.Count+1 : 0)+2+d2.Count;Vector2 cz=ej.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bF=Math.Min(1f,Math.Min(bI.X*0.94f/(cA*cz.X),bI.Y*0.94f/(ns*cz.Y)));float gr=iZ.X+(bI.X-cA*cz.X*bF)/2f;float cg=iZ.Y+(bI.Y-ns*cz.Y*bF)/2f;
float fG=cz.Y*bF;var cT=new List<MySprite>();cT.Add(new MySprite{Type=SpriteType.TEXTURE,Data="SquareSimple",Position=iZ+bI/2f,Size=bI,Color=new Color(10,12,16)}
);if(iR){j5(cT,ct,cR,gr,cg,bF,fG);cg+=(ct.Count+1)*fG;}bc(cT,">> "+mY,new Color(34,197,94),gr,cg,bF);cg+=fG*2f;int sh=D.oy(m9,d2.Count);
for(int fu=0;fu<sh;fu++){bool m6=fu==d2.Count-1&&m9>=0.8;bc(cT,m6 ? "[OK]" : "[..]",m6 ? new Color(34,197,94): new Color(56,189,248),gr,cg,bF);bc(cT,d2[fu],new Color(203,213,225),gr+5f*cz.X*bF,cg,bF);
cg+=fG;}using(var qG=ej.DrawFrame()){qG.AddRange(cT);}}void gZ(){b2.Clear();br.Clear();eW.Clear();
dw.Clear();eT=false;if(q==null)return;foreach(var U in q.bO){var aK=new List<IMyTerminalBlock>();if(U.bk==aN.db){
string iC= !string.IsNullOrEmpty(U.a0)? U.a0 : U.V;if(!aB.ContainsKey(iC)){aB[iC]=U.eD;if(U.gF!=null)dt[iC]=U.gF;
}b2[U.V]=aK;continue;}var n_=U.aJ.Count>0 ? U.aJ : new List<string>{U.kP};foreach(var cf in n_){
if(string.IsNullOrEmpty(cf))continue;bool mE=cf.Contains("*")||cf.Contains("?");if(U.bk==aN.gP||mE){var ig=new List<IMyTerminalBlock>();
oC(cf,ig,U.ev);if(ig.Count>0){foreach(var mo in ig){if(!aK.Contains(mo))aK.Add(mo);}}else{var k4=eu(U.ev);
for(int hu=0;hu<k4.Count;hu++){var gj=k4[hu];if(mE ? D.o1(cf,gj.CustomName): string.Equals(cf,gj.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aK.Contains(gj))aK.Add(gj);}}}}else{var hy=kk(cf,U.ev);if(hy!=null&& !aK.Contains(hy))aK.Add(hy);}}if(aK.Count==0){
pu(U,n_,aK);if(aK.Count>0)eW[U.V]=true;}b2[U.V]=aK;}foreach(var a9 in q.S){
if(string.IsNullOrEmpty(a9.bm))continue;var i1=kk(a9.bm);if(i1==null){dw.AppendLine($"HELM: output block '{a9.bm}' not found");
continue;}IMyTextSurface ce=null;var jk=i1 as IMyTextSurfaceProvider;var lR=i1 as IMyTextSurface;if(jk!=null&&a9.bT<jk.SurfaceCount){
ce=jk.GetSurface(a9.bT);}else if(lR!=null&&a9.bT==0){ce=lR;}if(ce==null){dw.AppendLine($"HELM: output block '{a9.bm}' has no surface index {a9.bT}");
continue;}ce.ContentType=ContentType.SCRIPT;ce.Script="";ce.ScriptBackgroundColor=Color.Black;br[a9]=ce;}b_.Clear();
cr.Clear();}static readonly string[]kj={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool oQ(int qZ,IMyTerminalBlock s){switch(qZ){case 0: return s is IMyBatteryBlock;case 1: return s is IMyReactor;case 2: return s is IMySolarPanel;
case 3: return s is IMyWindTurbine;case 4: return s is IMyGasTank;case 5: return s is IMyAirVent;case 6: return s is IMyCargoContainer;case 7: return s is IMyJumpDrive;
case 8: return s is IMyThrust;case 9: return s is IMyMotorSuspension;case 10: return s is IMyDoor;case 11: return s is IMyShipConnector;case 12: return s is IMyAssembler;
case 13: return s is IMyRefinery;case 14: return s is IMyShipDrill;case 15: return s is IMyShipWelder;case 16: return s is IMyShipGrinder;case 17: return s is IMyGasGenerator;
case 18: return s is IMyLargeTurretBase;case 19: return s is IMyProjector;case 20: return s is IMyGyro;case 21: return s is IMyGravityGeneratorBase;case 22: return s is IMyLandingGear;
case 23: return s is IMyRadioAntenna;case 24: return s is IMyBeacon;case 25: return s is IMyLightingBlock;case 26: return s is IMyPistonBase;case 27: return s is IMyMotorStator;
case 28: return s is IMyWarhead;case 29: return s is IMySensorBlock;case 30: return s is IMyCameraBlock;case 31: return s is IMyParachute;case 32: return s is IMyTurretControlBlock;
}return false;}void pu(a_ p3,List<string>sy,List<IMyTerminalBlock>hz){string qT=(p3.V+" "+string.Join(" ",sy)).ToLowerInvariant();
var iG=new List<int>();for(int fB=0;fB<kj.Length;fB++){foreach(var sS in kj[fB].Split(','))if(qT.Contains(sS)){iG.Add(fB);break;}}if(iG.Count==0)return;
var k3=eu();var m7=new List<IMyTerminalBlock>();for(int io=0;io<k3.Count;io++){var hv=k3[io];foreach(int q_ in iG){if(oQ(q_,hv)){(hv.CubeGrid==Me.CubeGrid ? hz : m7).Add(hv);
break;}}}if(hz.Count==0)hz.AddRange(m7);}void pw(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(q==null||q.S.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {q.S.Count} screens, {q.bO.Count} binds");eT=false;
Echo($"Blocks: {eu().Count} on this construct; {g8} on docked ships (ignored)");b_.Clear();Echo("--- BIND RESOLUTION ---");foreach(var al in q.bO){
if(al.bk==aN.db){string iD= !string.IsNullOrEmpty(al.a0)? al.a0 : al.V;double sI=aB.ContainsKey(iD)? aB[iD]: al.eD;
Echo($"[OK] @bind {al.V} (dynamic variable '{iD}' = {sI})");continue;}List<IMyTerminalBlock>hA;if(b2.TryGetValue(al.V,out hA)){if(hA.Count>0){
string qz=eW.ContainsKey(al.V)&&eW[al.V]? " [fallback: typed match]" : "";Echo($"[OK] @bind {al.V} ({hA.Count} blocks matched{qz})");
}else{string sz=string.Join(", ",al.aJ.Count>0 ? al.aJ : new List<string>{al.kP});Echo($"[WARN] @bind {al.V} -> 0 blocks matched \"{sz}\"");
}}else{Echo($"[ERR] @bind {al.V} -> unresolved");}}if(aB.Count>0){Echo($"--- DYNAMIC VARIABLES ({aB.Count}) ---");foreach(var mN in aB)Echo($"[VAR] {mN.Key} = {mN.Value}");
}IMyTextSurface hm=null;foreach(var rw in br.Values){hm=rw;break;}if(hm!=null){var jH=new List<string>();hm.GetSprites(jH);
Echo($"--- SPRITES ({jH.Count}): names usable in bg_texture ---");Echo(string.Join(", ",jH.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var ba in q.S){IMyTextSurface gc;if(br.TryGetValue(ba,out gc)){var nJ=gc.SurfaceSize;var sp=D.oE(ba);
Echo($"[OK] Screen \"{ba.a1}\" [{ba.bU}] -> {ba.bm} [#{ba.bT}] ({nJ.X:0}x{nJ.Y:0} px, texture {gc.TextureSize.X:0}x{gc.TextureSize.Y:0}, {sp.W} sprites)");
}else{Echo($"[ERR] Screen \"{ba.a1}\" -> Missing block \"{ba.bm}\" [#{ba.bT}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double pv(IMyTerminalBlock t){
if(t==null)return 0.0;var e5=t as IMyBatteryBlock;if(e5!=null)return e5.MaxStoredPower>0 ?(e5.CurrentStoredPower/e5.MaxStoredPower)*100.0 : 0.0;
var fA=t as IMyJumpDrive;if(fA!=null)return fA.MaxStoredPower>0 ?(fA.CurrentStoredPower/fA.MaxStoredPower)*100.0 : 0.0;var nZ=t as IMyGasTank;
if(nZ!=null)return nZ.FilledRatio*100.0;var oh=t as IMyAirVent;if(oh!=null)return oh.GetOxygenLevel()*100.0;var lx=t as IMyCargoContainer;if(lx!=null){
var fx=lx.GetInventory();if(fx!=null&&fx.MaxVolume.RawValue>0)return(double)fx.CurrentVolume.RawValue/fx.MaxVolume.RawValue*100.0;return 0.0;}var fX=t as IMyReactor;
if(fX!=null)return fX.MaxOutput>0 ?(fX.CurrentOutput/fX.MaxOutput)*100.0 : 0.0;var f5=t as IMySolarPanel;if(f5!=null)return f5.MaxOutput>0 ?(f5.CurrentOutput/f5.MaxOutput)*100.0 : 0.0;
var gq=t as IMyWindTurbine;if(gq!=null)return gq.MaxOutput>0 ?(gq.CurrentOutput/gq.MaxOutput)*100.0 : 0.0;var lS=t as IMyDoor;if(lS!=null)return(double)lS.OpenRatio*100.0;
var gm=t as IMyThrust;if(gm!=null)return gm.MaxEffectiveThrust>0 ?(gm.CurrentThrust/gm.MaxEffectiveThrust)*100.0 : 0.0;var jQ=t as IMyMotorSuspension;
if(jQ!=null)return jQ.Brake ? 100.0 :(jQ.Friction*100.0);var hP=t as IMyShipConnector;if(hP!=null)return hP.Status==MyShipConnectorStatus.Connected ? 100.0 :(hP.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var nm=t as IMyProductionBlock;if(nm!=null)return nm.IsProducing ? 100.0 : 0.0;var lW=t as IMyShipDrill;if(lW!=null)return lW.Enabled ? 100.0 : 0.0;
var jV=t as IMyShipToolBase;if(jV!=null)return jV.IsActivated ? 100.0 :(jV.Enabled ? 50.0 : 0.0);var ic=t as IMyGasGenerator;if(ic!=null)return ic.AutoRefill ? 100.0 :(ic.Enabled ? 50.0 : 0.0);
var gn=t as IMyLargeTurretBase;if(gn!=null)return gn.IsShooting ? 100.0 :(gn.HasTarget ? 50.0 :(gn.Enabled ? 10.0 : 0.0));var eb=t as IMyProjector;
if(eb!=null)return eb.TotalBlocks>0 ?((double)(eb.TotalBlocks-eb.RemainingBlocks)/eb.TotalBlocks)*100.0 : 0.0;var ij=t as IMyGyro;if(ij!=null)return ij.GyroOverride ? 100.0 :(ij.Enabled ? 50.0 : 0.0);
var mq=t as IMyGravityGeneratorBase;if(mq!=null)return Math.Min(100.0,Math.Abs(mq.GravityAcceleration)/9.81*100.0);var ib=t as IMyLandingGear;if(ib!=null)return ib.IsLocked ? 100.0 :(ib.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var k5=t as IMyRadioAntenna;if(k5!=null)return(k5.Radius/50000.0)*100.0;var lh=t as IMyBeacon;if(lh!=null)return(lh.Radius/50000.0)*100.0;
var mT=t as IMyLightingBlock;if(mT!=null)return mT.Intensity*10.0;var fU=t as IMyPistonBase;if(fU!=null)return fU.HighestPosition>0 ?(fU.CurrentPosition/fU.HighestPosition)*100.0 : 0.0;
var nq=t as IMyMotorStator;if(nq!=null)return Math.Min(100.0,Math.Abs(nq.Angle)/6.28318*100.0);var oj=t as IMyWarhead;if(oj!=null)return oj.IsArmed ? 100.0 : 0.0;
var jB=t as IMySensorBlock;if(jB!=null)return!jB.LastDetectedEntity.IsEmpty()? 100.0 :(jB.Enabled ? 25.0 : 0.0);var hI=t as IMyCameraBlock;
if(hI!=null)return hI.IsActive ? 100.0 :(hI.Enabled ? 50.0 : 0.0);var lD=t as IMyParachute;if(lD!=null)return(double)lD.OpenRatio*100.0;
var gl=t as IMyTurretControlBlock;if(gl!=null)return gl.HasTarget ? 100.0 :(gl.IsUnderControl ? 50.0 :(gl.Enabled ? 10.0 : 0.0));var mf=t as IMyFunctionalBlock;
if(mf!=null)return mf.Enabled ? 100.0 : 0.0;return 0.0;}static double oZ(IMyTerminalBlock ld,string h3){int lw=h3.IndexOf(':');string iF=h3.Substring(0,lw).ToLowerInvariant();
var sG=new MyItemType("MyObjectBuilder_"+(iF=="ammo" ? "AmmoMagazine" : char.ToUpper(iF[0])+iF.Substring(1)),h3.Substring(lw+1));double nS=0;for(int ip=0;
ip<ld.InventoryCount;ip++)nS+=(double)ld.GetInventory(ip).GetItemAmount(sG);return nS;}double kI(IMyTerminalBlock u,string l7){if(u==null)return 0.0;
string b=l7.ToLowerInvariant();if(b=="percent"||b=="pct")return pv(u);if(b.IndexOf(':')>0)return oZ(u,l7);var bv=u as IMyBatteryBlock;
if(bv!=null){if(b=="current"||b=="stored"||b=="val"||b=="value"||b=="raw")return bv.CurrentStoredPower;if(b=="max"||b=="capacity")return bv.MaxStoredPower;
if(b=="output")return bv.CurrentOutput;if(b=="maxoutput")return bv.MaxOutput;if(b=="input")return bv.CurrentInput;if(b=="maxinput")return bv.MaxInput;
return bv.CurrentStoredPower;}var iB=u as IMyJumpDrive;if(iB!=null){if(b=="max"||b=="capacity")return iB.MaxStoredPower;return iB.CurrentStoredPower;
}var gg=u as IMyGasTank;if(gg!=null){if(b=="max"||b=="capacity")return gg.Capacity;return gg.FilledRatio*gg.Capacity;}var oi=u as IMyAirVent;
if(oi!=null){if(b=="max"||b=="capacity")return 100.0;return oi.GetOxygenLevel()*100.0;}var ly=u as IMyCargoContainer;if(ly!=null){var fy=ly.GetInventory();
if(fy!=null){if(b=="mass")return(double)fy.CurrentMass;if(b=="max"||b=="capacity")return(double)fy.MaxVolume;return(double)fy.CurrentVolume;}return 0.0;}var jl=u as IMyPowerProducer;
if(jl!=null){if(b=="max"||b=="capacity")return jl.MaxOutput;return jl.CurrentOutput;}var lT=u as IMyDoor;if(lT!=null){if(b=="max")return 1.0;return(double)lT.OpenRatio*100.0;
}var jT=u as IMyThrust;if(jT!=null){if(b=="max"||b=="capacity")return jT.MaxEffectiveThrust;return jT.CurrentThrust;}var bK=u as IMyMotorSuspension;
if(bK!=null){if(b=="brake")return bK.Brake ? 100.0 : 0.0;if(b=="height")return bK.Height;if(b=="strength")return bK.Strength;if(b=="power")return bK.Power;
if(b=="steer")return bK.SteeringOverride*100.0;if(b=="propulsion")return bK.PropulsionOverride*100.0;return bK.Friction;}var lJ=u as IMyShipConnector;
if(lJ!=null){return lJ.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var ji=u as IMyProductionBlock;if(ji!=null){if(b=="producing"||b=="active"||b=="running"||b=="state")return ji.IsProducing ? 100.0 : 0.0;
if(b=="queue")return ji.IsQueueEmpty ? 0.0 : 1.0;}var hq=u as IMyAssembler;if(hq!=null){if(b=="disassembling")return hq.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(b=="repeating"||b=="loop")return hq.Repeating ? 100.0 : 0.0;}var id=u as IMyGasGenerator;if(id!=null){if(b=="autorefill")return id.AutoRefill ? 100.0 : 0.0;
if(b=="active")return id.Enabled ? 100.0 : 0.0;}var go=u as IMyLargeTurretBase;if(go!=null){if(b=="shooting")return go.IsShooting ? 100.0 : 0.0;
if(b=="target"||b=="hastarget")return go.HasTarget ? 100.0 : 0.0;if(b=="range")return go.Range;}var bE=u as IMyProjector;if(bE!=null){if(b=="built")return bE.TotalBlocks-bE.RemainingBlocks;
if(b=="remaining")return bE.RemainingBlocks;if(b=="total")return bE.TotalBlocks;if(b=="projecting")return bE.IsProjecting ? 100.0 : 0.0;if(b=="armor")return bE.RemainingArmorBlocks;
if(b=="buildable")return bE.BuildableBlocksCount;}var jW=u as IMyShipToolBase;if(jW!=null){if(b=="working"||b=="active"||b=="activated")return jW.IsActivated ? 100.0 : 0.0;
return jW.Enabled ? 100.0 : 0.0;}var cE=u as IMyGyro;if(cE!=null){if(b=="override")return cE.GyroOverride ? 100.0 : 0.0;if(b=="pitch")return cE.Pitch;
if(b=="yaw")return cE.Yaw;if(b=="roll")return cE.Roll;if(b=="power")return cE.GyroPower*100.0;}var mr=u as IMyGravityGeneratorBase;if(mr!=null){if(b=="g"||b=="accel")return mr.GravityAcceleration;
}var mp=u as IMyLandingGear;if(mp!=null){if(b=="locked")return mp.IsLocked ? 100.0 : 0.0;}var k6=u as IMyRadioAntenna;if(k6!=null){if(b=="radius"||b=="range")return k6.Radius;
}var li=u as IMyBeacon;if(li!=null){if(b=="radius"||b=="range")return li.Radius;}var iN=u as IMyLightingBlock;if(iN!=null){if(b=="intensity")return iN.Intensity;
if(b=="radius")return iN.Radius;}var fV=u as IMyPistonBase;if(fV!=null){if(b=="pos"||b=="position"||b=="extension")return fV.CurrentPosition;
if(b=="velocity")return fV.Velocity;if(b=="max")return fV.HighestPosition;}var eg=u as IMyMotorStator;if(eg!=null){if(b=="angle"||b=="deg")return eg.Angle*180.0/Math.PI;
if(b=="rad")return eg.Angle;if(b=="velocity"||b=="rpm")return eg.TargetVelocityRPM;if(b=="locked")return eg.RotorLock ? 100.0 : 0.0;}var j3=u as IMyWarhead;
if(j3!=null){if(b=="armed")return j3.IsArmed ? 100.0 : 0.0;if(b=="countdown")return j3.DetonationTime;}var nG=u as IMySensorBlock;if(nG!=null){
if(b=="detected")return!nG.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var hJ=u as IMyCameraBlock;if(hJ!=null){if(b=="range")return hJ.AvailableScanRange;
if(b=="active")return hJ.IsActive ? 100.0 : 0.0;}var lE=u as IMyParachute;if(lE!=null){if(b=="open"||b=="openratio")return(double)lE.OpenRatio*100.0;
}var cX=u as IMyTurretControlBlock;if(cX!=null){if(b=="target"||b=="hastarget")return cX.HasTarget ? 100.0 : 0.0;if(b=="azimuth")return cX.AzimuthRotor!=null ? cX.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(b=="elevation")return cX.ElevationRotor!=null ? cX.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var mg=u as IMyFunctionalBlock;if(mg!=null){return mg.Enabled ? 100.0 : 0.0;
}return 0.0;}double bV(string dG,string l8){a_ bw;dr.TryGetValue(dG,out bw);if(bw!=null&&bw.bk==aN.db){
string q0= !string.IsNullOrEmpty(bw.a0)? bw.a0 : dG;double lM;if(aB.TryGetValue(q0,out lM))return lM;return bw.eD;
}double lZ;if(aB.TryGetValue(dG,out lZ))return lZ;List<IMyTerminalBlock>dH;if(!b2.TryGetValue(dG,out dH)||dH.Count==0)throw new InvalidOperationException($"HELM: bind '{dG}' resolved to zero blocks — check the name/typo");
if(dH.Count==1)return kI(dH[0],l8);var og=new List<double>();foreach(var pT in dH)og.Add(kI(pT,l8));return ou.ot(bw!=null ? bw.c_ : az.gu,og);
}double g_(string p5){return bV(p5,"percent");}static string bh(double od,bl cK){if(cK==null||(string.IsNullOrEmpty(cK.dp)&&string.IsNullOrEmpty(cK.dc)&&cK.F=="percent")){
return kp(od);}return D.ko(od,cK.dp,cK.dc);}bool pp(y cS){IMyTextSurface gd;if(!br.TryGetValue(cS,out gd))return false;
if(cS.bM!=null)return pj(cS,gd);if(cS.c6.Equals("ascii",StringComparison.OrdinalIgnoreCase)){return pq(cS,gd);
}return pr(cS,gd);}Color oU(string[]cU,double rD){if(cU.Length==1)return E(cU[0]);double nn=Math.Max(0.0,Math.Min(100.0,rD))/100.0*(cU.Length-1);
int iq=Math.Min((int)nn,cU.Length-2);float ge=(float)(nn-iq);Color bt=E(cU[iq]),e4=E(cU[iq+1]);return new Color((int)(bt.R+(e4.R-bt.R)*ge),(int)(bt.G+(e4.G-bt.G)*ge),(int)(bt.B+(e4.B-bt.B)*ge),(int)(bt.A+(e4.A-bt.A)*ge));
}static Color[]er(a3 qa,string[]ss,string qq,float om,float op,float j1,float ik){int dE=16;bool my=qq=="horizontal";var hO=new Color[dE];
for(int b7=0;b7<dE;b7++){n ia=D.kq(ss,100.0*b7/(dE-1));hO[b7]=new Color(ia.bn,ia.bi,ia.bd);float lv=j1/dE,lo=ik/dE;x(qa,my ? new Vector2(om+lv*(b7+0.5f),op+ik/2f): new Vector2(om+j1/2f,op+lo*(b7+0.5f)),my ? new Vector2(lv+1f,ik): new Vector2(j1,lo+1f),hO[b7]);
}return hO;}static void x(a3 qb,Vector2 rM,Vector2 si,Color qe,float r3=0f,string qp="SquareSimple"){qb.l.Add(new MySprite{Type=SpriteType.TEXTURE,Data=qp,Position=rM,Size=si,Color=qe,RotationOrScale=r3}
);}void oB(y j,IMyTextSurface nT,a3 f){f.l.Clear();f.eQ.Clear();Vector2 K=nT.SurfaceSize;
Vector2 sD=nT.TextureSize;Vector2 ay=(sD-K)/2f;float fN=Math.Max(4f,K.X*0.02f);float b8=Math.Max(4f,K.Y*0.02f);
if(j.bU=="compact"||(j.bU=="auto"&&K.Y<=260f&&K.Y>150f)){fN=4f;b8=4f;}else if(j.bU=="corner"||(j.bU=="auto"&&K.Y<=150f)){
fN=2f;b8=2f;}float eo=K.X-(fN*2f);Color cv=new Color(10,12,16);bool lV=true;if(!string.IsNullOrEmpty(j.be)){
var dR=n.dj(j.be);if(dR.eI)lV=false;else cv=new Color(dR.bn,dR.bi,dR.bd,dR.gs);}f.bN= -1;f.et=false;
f.es=E(j.j7 ?? "#5b0f0f");f.c5.Clear();if(j.bg!=null&&j.bg.Length>1){f.bN=f.l.Count;f.ex=er(f,j.bg,j.c4,ay.X+2f,ay.Y+2f,K.X-4f,K.Y-4f);
}else if(lV||j.aE!=null||j.cl!=null){if(j.aE!=null)cv=E(j.aE.First(rR=>rR.co).bR);f.bN=f.l.Count;
f.ex=new[]{cv};x(f,ay+(K/2f),K-new Vector2(4f,4f),cv);if(j.aE!=null||j.cl!=null)f.c5.Add(new ey{
eH=f.bN,gv=cv,eC=cv,eP=j.aE,gz=j.gw,gI=j.gx,eF=j.cl,gJ=j.gK,gL=j.gM,kw=true}
);}if(!string.IsNullOrEmpty(j.gy))x(f,ay+(K/2f),K-new Vector2(4f,4f),E(j.j9 ?? "255,255,255,24"),0f,j.gy);
if(!j.c6.Equals("none",StringComparison.OrdinalIgnoreCase)){Color lq=j.ar!=null&&j.ar.Length>0 ? E(j.ar[0]): E(j.eB ?? "40, 60, 85");
Color p9=j.ar!=null&&j.ar.Length>1 ? E(j.ar[j.ar.Length-1]): lq;
float lr=ay.X+(K.X-eo)/2f;if(j.ar!=null&&j.ar.Length>1&& !j.gO){er(f,j.ar,"horizontal",lr,ay.Y+b8*0.5f-1f,eo,2f);
er(f,j.ar,"horizontal",lr,ay.Y+K.Y-(b8*0.5f)-1f,eo,2f);}else{x(f,new Vector2(ay.X+K.X/2f,ay.Y+b8*0.5f),new Vector2(eo,2f),lq);
x(f,new Vector2(ay.X+K.X/2f,ay.Y+K.Y-(b8*0.5f)),new Vector2(eo,2f),p9);}}var i7=j.ac;
if(i7.Count==0){i7=new List<aA>{new aA{di="main",c8=0,dk=0,dl=j.dd,dm=j.de,aQ=j.eK}};}float mt=8f;
foreach(var v in i7){var fM=D.oD(v.c8,v.dk,v.dl,v.dm,j.dd,j.de,K.X,K.Y,fN,b8,mt,mt);
float aU=ay.X+fM.kU;float d8=ay.Y+fM.kV;float a7=fM.kT;float fL=fM.ks;bool nt=v.aE!=null||v.cl!=null;
if(v.bg!=null&&v.bg.Length>1){er(f,v.bg,v.c4!="vertical" ? v.c4 : j.c4,aU,d8,a7,fL);
}else{Color bD;if(v.aE!=null)bD=E(v.aE.First(rS=>rS.co).bR);else if(!string.IsNullOrEmpty(v.ci))bD=E(v.ci);
else if(!string.IsNullOrEmpty(j.gW))bD=E(j.gW);else bD=Color.Transparent;if(bD.A>0||nt){if(nt)f.c5.Add(new ey{eH=f.l.Count,gv=bD,eC=bD,eP=v.aE,gz=v.gw,gI=v.gx,eF=v.cl,gJ=v.gK,gL=v.gM}
);x(f,new Vector2(aU+a7/2f,d8+fL/2f),new Vector2(a7,fL),bD);}}string h9=v.kl ?? j.km ?? "Monospace";float p1=D.oT(j.bU,K.X,K.Y,j.cm);
float aL=v.kn ?? p1;float fq=0f;if(!string.IsNullOrEmpty(v.bY)){fq=20f*aL;var hj=TextAlignment.LEFT;float jU=aU+4f;
string k_=v.g2 ?? j.kQ ?? "left";if(k_.Equals("center",StringComparison.OrdinalIgnoreCase)){hj=TextAlignment.CENTER;jU=aU+(a7/2f);
}else if(k_.Equals("right",StringComparison.OrdinalIgnoreCase)){hj=TextAlignment.RIGHT;jU=aU+a7-4f;}string n1=v.g3 ?? j.kR;
Color sE= !string.IsNullOrEmpty(n1)? E(n1): E(v.gD ?? j.eB ?? "white");var n6=MySprite.CreateText(v.bY,h9,sE,aL*0.9f,hj);
n6.Position=new Vector2(jU,d8+2f);f.l.Add(n6);string[]hw=v.gE ?? j.ar;Color qs=hw!=null&&hw.Length>0 ? E(hw[0]): E(v.gD ?? j.eB ?? "40, 60, 85");
x(f,new Vector2(aU+a7/2f,d8+fq-2f),new Vector2(a7-8f,1f),qs);}int rl=Math.Max(1,v.aQ.Count);float rB=fL-fq-8f;
float jp=rB/rl;float fw=6f;for(int fv=0;fv<v.aQ.Count;fv++){var z=v.aQ[fv];var P=new dh{kA=z};float r4=d8+fq+4f+(fv*jp);
float J=r4+(jp/2f);float n3=J-(11f*aL);P.gR=f.l.Count;P.kr=z.T!=null&&(z.T.Contains("{val}")||z.T.Contains("{percent}"));
var mQ=MySprite.CreateText(z.T,h9,Color.White,aL,TextAlignment.LEFT);mQ.Position=new Vector2(aU+fw,n3);f.l.Add(mQ);
float re=Math.Max(24f*aL,(z.T.Length+1)*12f*aL);float sP=(!string.IsNullOrEmpty(z.dp)|| !string.IsNullOrEmpty(z.dc))? 64f*aL : 52f*aL;
if(z.g!=e.g1){float I=aU+fw+re;float p0=aU+a7-fw-sP;float H=Math.Max(16f,p0-I);
float O=Math.Max(z.g==e.bW ? 8f : 4f,Math.Min((z.g==e.bW ? 20f : 16f)*aL,jp*(z.g==e.bW ? 0.55f : 0.45f)));
P.c3=I;P.ew=H;P.ck=O;P.c2=J;if(z.g==e.c1){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(25,30,38));
P.ab=f.l.Count;x(f,new Vector2(I,J),new Vector2(0f,O),E(z.at));}else if(z.g==e.dg){
int jz=10;float mi=2f;float jA=Math.Max(2f,(H-(mi*(jz-1)))/jz);for(int jt=0;jt<jz;jt++){P.ah.Add(f.l.Count);
x(f,new Vector2(I+jt*(jA+mi)+(jA/2f),J),new Vector2(jA,O),new Color(22,26,32));}}else if(z.g==e.bW){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));
int jh=8;float mj=2f;float hM=Math.Max(2f,(H-(mj*(jh-1)))/jh);for(int i5=0;i5<jh;i5++){P.ah.Add(f.l.Count);
x(f,new Vector2(I+i5*(hM+mj)+(hM/2f),J),new Vector2(hM,2f),E(z.at));}}else if(z.g==e.df){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));
int jK=10;float mk=2f;float jL=Math.Max(2f,(H-(mk*(jK-1)))/jK);for(int ju=0;ju<jK;ju++){P.ah.Add(f.l.Count);
x(f,new Vector2(I+ju*(jL+mk)+(jL/2f),J),new Vector2(jL,O),new Color(24,28,36),0.523599f);}}else if(z.g==e.c7){
int ho=10;float ml=2f;float hp=Math.Max(2f,(H-(ml*(ho-1)))/ho);for(int jv=0;jv<ho;jv++){P.ah.Add(f.l.Count);
x(f,new Vector2(I+jv*(hp+ml)+(hp/2f),J),new Vector2(hp,O),new Color(24,28,36),1.570796f,"Triangle");}}else if(z.g==e.da){
int hS=12;float mm=2f;float hU=Math.Max(2f,(H-(mm*(hS-1)))/hS);float lU=Math.Min(hU,O);for(int jw=0;jw<hS;jw++){P.ah.Add(f.l.Count);
x(f,new Vector2(I+jw*(hU+mm)+(hU/2f),J),new Vector2(lU,lU),new Color(24,28,36),0f,"Circle");}}else if(z.g==e.cj){
x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(25,30,38));P.ab=f.l.Count;x(f,new Vector2(I+(H/2f),J),new Vector2(0f,O),E(z.at));
x(f,new Vector2(I+(H/2f),J),new Vector2(2f,O+4f),new Color(80,110,150));}else if(z.g==e.bZ){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));
P.ab=f.l.Count;float iH=Math.Max(4f,O);x(f,new Vector2(I+iH*0.5f,J),new Vector2(iH,iH),new Color(40,50,65),0f,"Circle");
}else if(z.g==e.bQ){float cw=Math.Max(6f,Math.Min(O,14f*aL));x(f,new Vector2(I+cw*0.5f,J),new Vector2(cw,cw),new Color(28,34,46));
P.ab=f.l.Count;x(f,new Vector2(I+cw*0.5f,J),new Vector2(cw*0.6f,cw*0.6f),new Color(40,50,65));}else if(z.g==e.bX){
x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(18,22,30));P.ab=f.l.Count;float je=Math.Max(4f,O*0.7f);
x(f,new Vector2(I+je+2f,J),new Vector2(je,je),new Color(34,197,94),0f,"Circle");}else if(z.g==e.ch){int jd=8;
float mn=2f;float jf=Math.Max(2f,(H-(mn*(jd-1)))/jd);for(int i6=0;i6<jd;i6++){P.ah.Add(f.l.Count);x(f,new Vector2(I+i6*(jf+mn)+(jf/2f),J),new Vector2(jf,O),new Color(24,28,36));
}}else if(z.g==e.bS){x(f,new Vector2(I+(H/2f),J),new Vector2(H,O),new Color(20,24,30));P.ab=f.l.Count;
float mB=Math.Max(4f,O);x(f,new Vector2(I+(H/2f),J),new Vector2(mB,mB),E(z.at),0f,"Triangle");}}var ob=MySprite.CreateText("---",h9,Color.White,aL,TextAlignment.RIGHT);
ob.Position=new Vector2(aU+a7-fw,n3);P.bq=f.l.Count;f.l.Add(ob);f.eQ.Add(P);}}f.ky=K;
f.kv=true;}static void aR(a3 lt,dh nK,string sC,Color qf){var j_=lt.l[nK.bq];j_.Data=sC;j_.Color=qf;lt.l[nK.bq]=j_;
}static void kK(a3 lu,dh nL,Color qg){var l2=lu.l[nL.ab];l2.Color=qg;lu.l[nL.ab]=l2;}bool pr(y bb,IMyTextSurface jN){
a3 h;if(!dy.TryGetValue(bb,out h)){h=new a3();dy[bb]=h;}if(!h.kv||h.ky!=jN.SurfaceSize){
oB(bb,jN,h);}bool M=false;bool e0=(ai%bb.gt)<bb.gt/2.0;double b6= -1.0,lK;if(eU.TryGetValue(bb,out lK)){
b6=(ai-lK)/bb.gB;if(b6>=1.0){eU.Remove(bb);b6=1.0;}}bool j2=b0&&e0&&h.es.A>0;if(h.bN>=0&&j2!=h.et){
h.et=j2;for(int dT=0;dT<h.ex.Length;dT++){var lk=h.l[h.bN+dT];lk.Color=j2 ? h.es : h.ex[dT];h.l[h.bN+dT]=lk;
}M=true;foreach(var p2 in h.c5)p2.gN=true;}foreach(var ae in h.c5){if(ae.kw&&h.et)continue;Color dF=ae.gv;try{if(ae.eP!=null)dF=E(D.eO(ae.eP,bV(ae.gz,ae.gI)));
if(ae.eF!=null&&e0&&D.eO(ae.eF,bV(ae.gJ,ae.gL))!=null)dF=h.es;}catch{}if(dF!=ae.eC||ae.gN){
ae.eC=dF;ae.gN=false;var ls=h.l[ae.eH];ls.Color=dF;h.l[ae.eH]=ls;M=true;}}for(int ir=0;ir<h.eQ.Count;ir++){var c=h.eQ[ir];
var i=c.kA;double w;bool mv=false;double iU=i.eL;try{w=bV(i.aF,i.F);if(i.cp!=null)iU=bV(i.cp,i.gT);
}catch{mv=true;w=0.0;}if(mv){if(c.eJ!= -99999.0||(c.bq>=0&&h.l[c.bq].Data!="ERR")){M=true;c.eJ= -99999.0;
if(c.bq>=0){var jx=h.l[c.bq];jx.Data="ERR";jx.Color=Color.Red;h.l[c.bq]=jx;}}continue;}double aV=w;
if(iU>0)w=Math.Min(100.0,aV/iU*100.0);bool nV=b6>=0.0&&i.g!=e.g1&&i.g!=e.bZ&&i.g!=e.bQ&&i.g!=e.bX&&i.g!=e.bS;
if(nV)w=b6<0.5 ? b6*200.0 : 100.0+(w-100.0)*(b6-0.5)*2.0;string qi=i.cn ? D.eG(i.bj,w): i.at;
Color _=E(qi);bool a5=(_==Color.Red);if(i.cn&& !bb.gO&&i.bj.Length>1)_=oU(i.bj,w);
if(a5&&e0){_=new Color(255,75,75);M=true;}bool aM=nV||Math.Abs(aV-c.eJ)>0.1;if(aM)c.eJ=aV;
if(i.g==e.c1){double l9=(i.gQ||i.F=="percent"||i.F=="pct")? w : g_(i.aF);if(c.Q<0f)c.Q=(float)l9;
float gh=(float)l9;if(Math.Abs(c.Q-gh)>0.05f){c.Q+=(gh-c.Q)*0.35f;M=true;}else if(c.Q!=gh){
c.Q=gh;M=true;}if(aM)M=true;if(M){float rE=(float)Math.Max(0.0,Math.Min(1.0,c.Q/100.0));float mb=c.ew*rE;
var fj=h.l[c.ab];fj.Size=new Vector2(mb,c.ck);fj.Position=new Vector2(c.c3+(mb/2f),c.c2);
fj.Color=_;h.l[c.ab]=fj;aR(h,c,bh(aV,i),_);}}else if(i.g==e.dg||i.g==e.df||i.g==e.c7||i.g==e.da){
if(aM||a5){M=true;double qB=(i.gQ||i.F=="percent"||i.F=="pct")? w : g_(i.aF);int nE=c.ah.Count;
for(int ei=0;ei<nE;ei++){bool rn=qB>=((ei+0.4)/(double)nE*100.0);Color se=rn ? _ : new Color(24,28,36);var nF=h.l[c.ah[ei]];
nF.Color=se;h.l[c.ah[ei]]=nF;}aR(h,c,bh(aV,i),_);}}else if(i.g==e.bW){
if(aM||a5){M=true;for(int fp=0;fp<7;fp++)c.eR[fp]=c.eR[fp+1];c.eR[7]=(float)w;int rL=c.ah.Count;
for(int d4=0;d4<rL;d4++){float qS=c.eR[d4];float qR=Math.Max(0.06f,Math.Min(1f,qS/100f));float lG=c.ck*qR;float qh=c.c2+(c.ck/2f)-(lG/2f);
var cB=h.l[c.ah[d4]];cB.Size=new Vector2(cB.Size.Value.X,lG);cB.Position=new Vector2(cB.Position.Value.X,qh);
cB.Color=_;h.l[c.ah[d4]]=cB;}aR(h,c,bh(aV,i),_);}}else if(i.g==e.cj){
if(c.Q<0f)c.Q=(float)w;float gi=(float)w;if(Math.Abs(c.Q-gi)>0.05f){c.Q+=(gi-c.Q)*0.35f;
M=true;}else if(c.Q!=gi){c.Q=gi;M=true;}if(M||aM){float mu=c.ew*0.5f;float lB=c.c3+mu;
float lQ=(float)Math.Max(-1.0,Math.Min(1.0,(c.Q-50.0)/50.0));float h7=Math.Abs(lQ)*mu;float qC=(lQ>=0)?(lB+h7*0.5f):(lB-h7*0.5f);
var fk=h.l[c.ab];fk.Size=new Vector2(h7,c.ck);fk.Position=new Vector2(qC,c.c2);fk.Color=_;
h.l[c.ab]=fk;aR(h,c,bh(aV,i),_);}}else if(i.g==e.bZ){if(aM||a5){
M=true;bool iA=w>=50.0;float mK=Math.Max(4f,c.ck);float q3=iA ?(c.c3+c.ew-mK*0.5f):(c.c3+mK*0.5f);
Color mJ=iA ?(_!=Color.White ? _ : new Color(34,197,94)): new Color(60,70,85);var h5=h.l[c.ab];h5.Position=new Vector2(q3,c.c2);
h5.Color=mJ;h.l[c.ab]=h5;aR(h,c,iA ? "ON" : "OFF",mJ);}}else if(i.g==e.bQ){if(aM||a5){
M=true;bool iz=w>=50.0;Color lC=iz ?(_!=Color.White ? _ : new Color(56,189,248)): new Color(30,36,48);kK(h,c,lC);
aR(h,c,iz ? "ACTIVE" : "READY",iz ? lC : new Color(148,163,184));}}else if(i.g==e.bX){if(aM||a5){
M=true;string sq=w>=75.0 ? "ONLINE" :(w>=25.0 ? "STBY" : "OFFLINE");Color nP=w>=75.0 ? new Color(34,197,94):(w>=25.0 ? new Color(234,179,8): new Color(239,68,68));
kK(h,c,nP);aR(h,c,sq,nP);}}else if(i.g==e.ch){if(aM||a5){M=true;int ni=c.ah.Count;
for(int d5=0;d5<ni;d5++){bool ro=w>=((d5+0.5)/(double)ni*100.0);Color rI=ro ? _ : new Color(24,28,36);var nj=h.l[c.ah[d5]];
nj.Color=rI;h.l[c.ah[d5]]=nj;}aR(h,c,bh(aV,i),_);}}else if(i.g==e.bS){
M=true;float f_;Color dP;string fl;bool qW=(i.F=="producing"||i.F=="active"||i.F=="running"||i.F=="state"||(i.F!="percent"&&(w<=1.0||w==100.0)));
if(qW){if(w>0.0){c.Q=(c.Q+0.3927f)%6.28318f;f_=c.Q;dP=new Color(34,197,94);fl="RUN";}else{f_=0f;
dP=new Color(148,163,184);fl="IDLE";}}else{f_=(w>55.0)? 0f :((w<45.0)? 3.14159f : 1.57079f);dP=(w>55.0)? new Color(34,197,94):((w<45.0)? new Color(239,68,68): new Color(148,163,184));
fl=(w>55.0)? "CHG" :((w<45.0)? "DSC" : "STBY");}var h6=h.l[c.ab];h6.RotationOrScale=f_;h6.Color=dP;
h.l[c.ab]=h6;aR(h,c,fl,dP);}else{string fh;a_ gk;dr.TryGetValue(i.aF,out gk);dt.TryGetValue(kx(i.F)? i.aF+"."+i.F :(gk!=null&&gk.a0!=null ? gk.a0 : i.aF),out fh);
if(aM||a5||fh!=c.kz){c.kz=fh;M=true;Color jX=i.c9!=null ? E(D.eO(i.c9,w)): Color.White;
if(jX==Color.Red&&e0)jX=new Color(255,75,75);aR(h,c,fh ?? bh(aV,i),jX);}}if(c.kr&&(aM||a5)){
M=true;var mS=h.l[c.gR];string rF=kp(w);string r1=bh(aV,i);mS.Data=i.T.Replace("{percent}",rF).Replace("{val}",r1);
h.l[c.gR]=mS;}}if(M){using(var qH=jN.DrawFrame()){qH.AddRange(h.l);}}return M;}bool pq(y bG,IMyTextSurface jO){
jO.FontSize=bG.cm;var fd=new List<KeyValuePair<string,string>>();foreach(var m in bG.eK){double L=0.0;string aY;string aT=null;
double iV=m.eL;try{L=bV(m.aF,m.F);if(m.cp!=null)iV=bV(m.cp,m.gT);}catch(Exception s_){
fd.Add(new KeyValuePair<string,string>($"{m.T}: [UNRESOLVED]","red"));continue;}double no=L;if(iV>0)L=Math.Min(100.0,no/iV*100.0);string oc=bh(no,m);
if(m.g==e.c1){double ma=(m.gQ||m.F=="percent"||m.F=="pct")? L : g_(m.aF);aY=D.pk(m.T,ma,10,oc);
aT=m.cn ? D.eG(m.bj,ma): m.at;}else if(m.g==e.cj){aY=D.pi(m.T,L,10);
aT=m.cn ? D.eG(m.bj,L): m.at;}else if(m.g==e.bZ){aY=D.pt(m.T,L);
aT=m.at ??(L>=50.0 ? "green" : "gray");}else if(m.g==e.bQ){aY=D.pn(m.T,L);aT=m.at ??(L>=50.0 ? "cyan" : "gray");
}else if(m.g==e.bX){aY=D.ps(m.T,L);aT=m.at ??(L>=75.0 ? "lime" :(L>=25.0 ? "yellow" : "red"));
}else if(m.g==e.ch){aY=D.ph(m.T,L,8);aT=m.cn ? D.eG(m.bj,L):(m.at ?? "yellow");
}else if(m.g==e.bS){bool qX=(m.F=="producing"||m.F=="active"||m.F=="running"||m.F=="state"||(m.F!="percent"&&(L<=1.0||L==100.0)));
if(qX){char[]so=new[]{'|','/','-','\\'};char sb=so[Math.Abs(dz)%4];aY=L>0 ? $"{m.T} [{sb}] RUN" : $"{m.T} [==] IDLE";
aT=L>0 ? "green" : "gray";}else{aY=D.po(m.T,L,dz);aT=m.at ??(L>55.0 ? "green" :(L<45.0 ? "red" : "cyan"));
}}else{aY=$"{m.T}: {oc}";aT=m.c9!=null ? D.eO(m.c9,L): null;}fd.Add(new KeyValuePair<string,string>(aY,aT));
}var fa=D.pm(new List<string>(fd.ConvertAll(sx=>sx.Key)),cZ: bG.kS);string nH=string.Join("\n",fa);
string nl;if(b1.TryGetValue(bG,out nl)&&nl==nH)return false;b1[bG]=nH;using(var qI=jO.DrawFrame()){
float oq=4f;float mU=os*bG.cm;try{var mW=jO.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bG.cm);
if(mW.Y>0)mU=mW.Y;}catch{}for(int cF=0;cF<fa.Count;cF++){string lI=null;if(cF>0&&cF<fa.Count-1)lI=fd[cF-1].Value;
var nO=MySprite.CreateText(fa[cF],"Monospace",E(lI),bG.cm,TextAlignment.LEFT);nO.Position=new Vector2(4f,oq);qI.Add(nO);
oq+=mU;}}return true;}Color E(string fJ){if(string.IsNullOrEmpty(fJ))return Color.White;Color d_;if(kW.TryGetValue(fJ,out d_))return d_;
n dI=n.dj(fJ);d_=dI.eI ? Color.Transparent : new Color(dI.bn,dI.bi,dI.bd,dI.gs);kW[fJ]=d_;return d_;}public enum aN{ow,gP,db}
public enum az{o2,gu,kO,kC,kB}public class a_{public string V;public aN bk;public List<string>aJ=new List<string>();public string kP{
get{return aJ.Count>0 ? aJ[0]: "";}set{if(aJ.Count==0)aJ.Add(value);else aJ[0]=value;}}public az c_=az.o2;
public double eD=0.0;public string gF;public string a0;public bool ev;}public enum e{g1,c1,dg,bW,df,c7,da,cj,bZ,bQ,bX,ch,bS}
public class aG{public string gH;public string bR;public bool co;}public class bl{public e g;public string T;public string aF;
public string F;public bool cn;public string[]bj;public string at;public List<aG>c9;public string kg="right";
public string dp;public string dc;public double eL;public string cp,gT;public bool gQ{get{return eL>0||cp!=null;}}}public struct n{
public byte bn,bi,bd,gs;public bool eI;public n(byte rT,byte qK,byte pU,byte pD=255){bn=rT;bi=qK;bd=pU;gs=pD;eI=false;}public static readonly n pB=new n(0,0,0,0){
eI=true};public static readonly n eS=new n(255,255,255);public static readonly n ov=new n(0,0,0);public static readonly n pf=new n(255,0,0);
public static readonly n oW=new n(0,255,0);public static readonly n o_=new n(50,205,50);public static readonly n pC=new n(255,255,0);
public static readonly n o3=new n(255,140,0);public static readonly n oG=new n(0,255,255);public static readonly n ox=new n(0,120,255);
public static readonly n o0=new n(255,0,255);public static readonly n pd=new n(128,0,128);public static readonly n oV=new n(128,128,128);
public static readonly n oH=new n(60,60,60);public static n dj(string aX){if(string.IsNullOrWhiteSpace(aX))return eS;aX=aX.Trim();
if(aX.Equals("transparent",StringComparison.OrdinalIgnoreCase)||aX.Equals("none",StringComparison.OrdinalIgnoreCase))return pB;if(aX.StartsWith("#")){
string bA=aX.Substring(1).Trim();if(bA.Length==6){byte rU=Convert.ToByte(bA.Substring(0,2),16);byte qL=Convert.ToByte(bA.Substring(2,2),16);byte pV=Convert.ToByte(bA.Substring(4,2),16);
return new n(rU,qL,pV);}if(bA.Length==3){byte rV=Convert.ToByte(new string(bA[0],2),16);byte qM=Convert.ToByte(new string(bA[1],2),16);byte pW=Convert.ToByte(new string(bA[2],2),16);
return new n(rV,qM,pW);}}if(aX.Contains(",")){var cN=aX.Split(',');if(cN.Length>=3){byte rW=(byte)Math.Max(0,Math.Min(255,int.Parse(cN[0].Trim())));
byte qN=(byte)Math.Max(0,Math.Min(255,int.Parse(cN[1].Trim())));byte pX=(byte)Math.Max(0,Math.Min(255,int.Parse(cN[2].Trim())));byte pE=cN.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(cN[3].Trim()))):(byte)255;
return new n(rW,qN,pX,pE);}}switch(aX.ToLowerInvariant()){case "red": return pf;case "green": return oW;case "lime": return o_;case "yellow": return pC;
case "orange": return o3;case "cyan": return oG;case "blue": return ox;case "magenta": return o0;case "purple": return pd;case "white": return eS;
case "black": return ov;case "gray": case "grey": return oV;case "darkgray": case "darkgrey": return oH;default: return eS;}}}public class bf{
public string[]bg;public string c4="vertical";public List<aG>aE;public string gw,gx;public List<aG>cl;public string gK,gM;
}public class aA : bf{public string di="default";public int c8=0;public int dk=0;public int dl=1;public int dm=1;public string bY;public string g2="left";
public string g3;public string ke;public string kl;public float? kn;public string ci;public string gD;public string[]gE;
public string kD="horizontal";public List<bl>aQ=new List<bl>();}public class y : bf{public string a1;public string bU="standard";
public string c6="graphic";public string eB;public string[]ar;public string kQ="left";public string kR;public string km="Monospace";
public string be;public string kc;public int kd=10;public int ka=1;public float gB=1.2f;public bool gO;
public string gW;public string gy;public string j9;public string j7;public string bM;public string bP;public float gC=2.5f;
public string kb="cyan";public string[]ez;public List<string>eA=new List<string>();public List<string>oS(){if(eA!=null&&eA.Count>0)return eA;
return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float cm=1.0f;public int gY=0;public float gt=1.6f;public int dd=1;public int de=1;public int kS=30;public string bm;
public int bT=0;public List<aA>ac=new List<aA>();public List<bl>eK{get{if(ac.Count==0)ac.Add(new aA{di="main"}
);return ac[0].aQ;}}}public class bp{public string gU;public List<string>eM=new List<string>();public Dictionary<string,string>gG=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>gA=new List<string>();}public class gX{public List<a_>bO=new List<a_>();public List<y>S=new List<y>();
public List<string>aP=new List<string>();public List<string>dq=new List<string>();public Dictionary<string,List<string>>c0=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class kt{static readonly Dictionary<string,bp>kf=new Dictionary<string,bp>(StringComparer.OrdinalIgnoreCase);
static kt(){pg();}static void pg(){bo("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bo("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bo("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bo("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bo("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bo("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bo("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bo("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bo(string m2,string[]rC,string p6){var gf=new bp{gU=m2}
;foreach(var rx in rC){var iI=rx.Split(new[]{'='},2);string na=gV(iI[0]);gf.eM.Add(na);if(iI.Length>1)gf.gG[na]=ad(iI[1].Trim());
}var rm=p6.Replace("\r\n","\n").Split('\n');foreach(var q9 in rm){string nv=q9.Trim();if(nv.Length>0)gf.gA.Add(nv);}kf[m2]=gf;}static string gV(string cL){
cL=cL.Trim();if(cL.StartsWith("$"))cL=cL.Substring(1);return cL.ToLowerInvariant();}public static gX o5(string ql){var am=new gX();y d=null;
aA r=null;var nk=pb(ql,am.aP,am.c0);for(int dU=0;dU<nk.Count;dU++){string a=nk[dU].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){am.bO.Add(o6(a));}else if(a.StartsWith("@screen")){d=new y{a1=a.Substring("@screen".Length).Trim()}
;am.S.Add(d);r=null;}else if(a.StartsWith("profile:")){p(d,"profile:");d.bU=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){p(d,"border:");string le=a.Substring("border:".Length).Trim();if(r!=null)r.ke=le;
else d.c6=le;}else if(a.StartsWith("border_color:")){p(d,"border_color:");string lg=a.Substring("border_color:".Length).Trim();
if(r!=null)r.gD=lg;else d.eB=lg;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
p(d,"border_gradient");string b4=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
b4=b4.Trim();if(b4.StartsWith("(")&&b4.Contains(")"))b4=aH(b4,0);string[]nQ=aI(b4).Select(r5=>r5.Trim()).ToArray();if(r!=null)r.gE=nQ;
else d.ar=nQ;}else if(a.StartsWith("title_align:")){p(d,"title_align:");string nY=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(r!=null)r.g2=nY;else d.kQ=nY;}else if(a.StartsWith("title_color:")){p(d,"title_color:");string n0=a.Substring("title_color:".Length).Trim();
if(r!=null)r.g3=n0;else d.kR=n0;}else if(a.StartsWith("font_size:")){p(d,"font_size:");float nW=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(r!=null)r.kn=nW;else d.cm=nW;}else if(a.StartsWith("font:")){p(d,"font:");string l3=a.Substring("font:".Length).Trim();
if(r!=null)r.kl=l3;else d.km=l3;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){p(d,"bg:");string cu=a.Substring(a.IndexOf(':')+1).Trim();
bf hx=r!=null ?(bf)r : d;if(cu.StartsWith("color(")&&cu.EndsWith(")")){string ln;hx.aE=kG(cu.Substring(6,cu.Length-7),out ln);
string ll,lm;kL(ln,out ll,out lm);hx.gw=ll;hx.gx=lm;}else if(r!=null)r.ci=cu;
else d.be=cu;}else if(a.StartsWith("layout:")){p(d,"layout:");o7(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
p(d,"alert_period:");d.gt=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){p(d,"refresh:");d.gY=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
p(d,"output:");var fQ=aI(a.Substring("output:".Length).Trim());if(fQ.Count==0)throw new FormatException("output: requires a target block name");
d.bm=ad(fQ[0]);d.bT=fQ.Count>1 ? int.Parse(fQ[1]): 0;}else if(a.StartsWith("width:")){p(d,"width:");
d.kS=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){p(d,"boot_msg:");
d.kc=ad(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){p(d,"boot_stagger:");d.kd=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){p(d,"boot_calibrate:");string lL=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.gB=lL=="off" ? 0f : Math.Max(0f,float.Parse(lL,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){p(d,"boot_batch:");
d.ka=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){p(d,"boot_duration:");
string dK=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool mA=dK.EndsWith("s");if(mA)dK=dK.Substring(0,dK.Length-1).Trim();
float hV;if(float.TryParse(dK,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out hV)){d.gC=mA ? hV : hV/60f;
}}else if(a.StartsWith("boot_log:")){p(d,"boot_log:");d.eA.Add(ad(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("banner:")){
p(d,"banner:");d.bM=ad(a.Substring("banner:".Length).Trim());}else if(a.StartsWith("boot_logo:")){p(d,"boot_logo:");
d.bP=ad(a.Substring("boot_logo:".Length).Trim());}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){
p(d,"boot_gradient");d.ez=kH(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){p(d,"bg_gradient");
(r!=null ?(bf)r : d).bg=kH(a);}else if(a.StartsWith("bg_gradient_dir:")){p(d,"bg_gradient_dir:");
(r!=null ?(bf)r : d).c4=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();}else if(a.StartsWith("flash:")){
p(d,"flash:");bf h2=r!=null ?(bf)r : d;string h1=a.Substring("flash:".Length).Trim();string l5=kN(h1);
string l4,l6;kL(h1.Substring(0,h1.Length-l5.Length),out l4,out l6);h2.gK=l4;h2.gM=l6;h2.cl=new List<aG>{
new aG{gH=l5,bR="on"}};}else if(a.StartsWith("bg_texture:")){p(d,"bg_texture:");d.gy=ad(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){p(d,"bg_texture_color:");d.j9=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gradient_mode:")){p(d,"gradient_mode:");d.gO=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){p(d,"screen_bg:");d.be=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
p(d,"panel_bg:");d.gW=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){p(d,"alert_bg:");
d.j7=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){p(d,"boot_color:");
d.kb=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){p(d,"orientation:");string ru=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(r!=null)r.kD=ru;}else if(a.StartsWith("@panel")){p(d,"@panel");r=o9(a);d.ac.Add(r);
}else if(a.StartsWith("title:")){p(d,"title:");if(r==null){r=new aA{di="main",c8=0,dk=0,dl=d.dd,dm=d.de}
;d.ac.Add(r);}r.bY=ad(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){p(d,"line:");
if(r==null){r=new aA{di="main",c8=0,dk=0,dl=d.dd,dm=d.de};d.ac.Add(r);}r.aQ.Add(o8(a.Substring("line:".Length).Trim()));
}else{am.aP.Add($"Line {dU + 1}: unrecognized directive: \"{a}\"");}}catch(Exception qv){am.aP.Add($"Line {dU + 1}: parse error ({qv.Message}) in \"{a}\"");
}}foreach(var nw in am.S){if(string.IsNullOrEmpty(nw.bm))am.aP.Add($"@screen {nw.a1}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var ao in am.S){int i3=ao.ac.Count(jg=>jg.ci!=null||jg.aE!=null||jg.bg!=null);if(ao.ac.Count>1&&i3>0&&i3<ao.ac.Count)am.dq.Add("screen '"+ao.a1+"': only "+i3+" of "+ao.ac.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(ao.bM!=null&& !am.c0.ContainsKey(ao.bM))am.aP.Add("Screen '"+ao.a1+"': banner '"+ao.bM+"' is not defined (@banner "+ao.bM+" ... @end)");
if(ao.bP!=null&& !am.c0.ContainsKey(ao.bP))am.aP.Add("Screen '"+ao.a1+"': boot_logo '"+ao.bP+"' is not defined (@banner "+ao.bP+" ... @end)");
}return am;}static void o7(string hC,y nD){int i8=hC.IndexOf('(');if(i8<0)throw new FormatException("missing '(' in layout: specification");
string mI=hC.Substring(0,i8).Trim().ToLowerInvariant();string pL=aH(hC,i8);var hn=aI(pL);if(mI=="grid"){if(hn.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
nD.dd=Math.Max(1,int.Parse(hn[0]));nD.de=Math.Max(1,int.Parse(hn[1]));}else{throw new FormatException($"unknown layout kind '{mI}' (expected grid)");
}}static aA o9(string rh){string ed=rh.Substring("@panel".Length).Trim();int e3=ed.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(e3<0)throw new FormatException("@panel requires an at(...) positioning clause");
string rr=ed.Substring(0,e3).Trim();string la=ed.Substring(e3);int ng=la.IndexOf('(');string k8=aH(la,ng);
var b3=aI(k8);if(b3.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var a8=new aA{di=rr,c8=int.Parse(b3[0]),dk=int.Parse(b3[1]),dl=b3.Count>2 ? int.Parse(b3[2]): 1,dm=b3.Count>3 ? int.Parse(b3[3]): 1}
;int k1=e3+ng+k8.Length+1;if(k1<ed.Length){string aC=ed.Substring(k1).Trim();int n5=aC.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(n5>=0){string ek=aC.Substring(n5+5).Trim();if(ek.StartsWith("(")&&ek.Contains(")"))a8.bY=ad(aH(ek,0));else if(ek.StartsWith(":")){
string b5=ek.Substring(1).Trim();if(b5.StartsWith("\"")){int l_=b5.IndexOf('"',1);a8.bY=l_>0 ? b5.Substring(1,l_-1): ad(b5);}else{int nM=b5.IndexOf(' ');
a8.bY=nM>=0 ? b5.Substring(0,nM): b5;}}}string m5=eE(aC,"orientation:");if(m5!=null)a8.kD=m5.ToLowerInvariant();string k0=eE(aC,"title_align:");
if(k0!=null)a8.g2=k0.ToLowerInvariant();string n2=eE(aC,"title_color:");if(n2!=null)a8.g3=n2;int e9=aC.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(e9>=0&& !aC.Substring(e9).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aC.Substring(e9).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
a8.ke=eE(aC.Substring(e9),"border:");}int e6=aC.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(e6<0)e6=aC.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(e6>=0){string bx=aC.Substring(e6+"border_gradient".Length).Trim();if(bx.StartsWith(":"))bx=bx.Substring(1).Trim();if(bx.StartsWith("(")&&bx.Contains(")"))bx=aH(bx,0);
a8.gE=aI(bx).Select(r6=>r6.Trim()).ToArray();}}return a8;}static string eE(string nx,string mH){int mz=nx.IndexOf(mH,StringComparison.OrdinalIgnoreCase);
if(mz<0)return null;string j0=nx.Substring(mz+mH.Length).Trim();int nN=j0.IndexOf(' ');return nN>=0 ? j0.Substring(0,nN): j0;}static string[]kH(string iO){
int ie=iO.IndexOfAny(new[]{':','('});string dQ=iO.Substring(iO[ie]==':' ? ie+1 : ie).Trim();if(dQ.StartsWith("(")&&dQ.Contains(")"))dQ=aH(dQ,0);return aI(dQ).Select(r7=>r7.Trim()).ToArray();
}static List<string>pb(string qm,List<string>hZ,Dictionary<string,List<string>>pZ){var n8=new Dictionary<string,bp>(StringComparer.OrdinalIgnoreCase);
var cP=(qm ?? "").Replace("\r\n","\n").Split('\n');var i2=new List<string>();int cG=0;while(cG<cP.Length){string jm=cP[cG++];if(jm.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ak=new List<string>();while(cG<cP.Length){string kZ=cP[cG++].Replace("\t","    ").TrimEnd();if(kZ.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ak.Add(kZ);}while(ak.Count>0&&ak[0].Length==0)ak.RemoveAt(0);while(ak.Count>0&&ak[ak.Count-1].Length==0)ak.RemoveAt(ak.Count-1);int iy=int.MaxValue;
foreach(var hf in ak)if(hf.Length>0)iy=Math.Min(iy,hf.Length-hf.TrimStart().Length);for(int d0=0;d0<ak.Count;d0++)if(ak[d0].Length>0)ak[d0]=ak[d0].Substring(iy);
pZ[jm.TrimStart().Substring(8).Trim()]=ak;continue;}string bC=kM(jm).Trim();if(bC.Length==0)continue;if(bC.StartsWith("@template")){try{
var jR=pa(bC);while(cG<cP.Length){string p8=cP[cG++];string hD=kM(p8).Trim();if(hD.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(hD.Length>0)jR.gA.Add(hD);}n8[jR.gU]=jR;}catch(Exception qw){hZ.Add($"Template parse error ({qw.Message}) in \"{bC}\"");}}
else if(bC.StartsWith("@use")){try{ki(bC,n8,i2,hZ);}catch(Exception qx){hZ.Add($"@use error ({qx.Message}) in \"{bC}\"");
}}else{i2.Add(bC);}}return i2;}static bp pa(string ri){string jn=ri.Substring("@template".Length).Trim();int i9=jn.IndexOf('(');
if(i9<0)throw new FormatException("missing '(' in @template header");string rs=jn.Substring(0,i9).Trim();string pM=aH(jn,i9);var jS=new bp{
gU=rs};foreach(var ry in aI(pM)){var iJ=ry.Split(new[]{'='},2);string nb=gV(iJ[0]);jS.eM.Add(nb);if(iJ.Length>1)jS.gG[nb]=ad(iJ[1].Trim());
}return jS;}static void ki(string rj,Dictionary<string,bp>n9,List<string>m8,List<string>qu,int lP=0){string fZ=rj.Substring("@use".Length).Trim();
int fO=fZ.IndexOf('(');string iW=fO>=0 ? fZ.Substring(0,fO).Trim(): fZ.Trim();string pN=fO>=0 ? aH(fZ,fO): "";bp cW=null;
if(!n9.TryGetValue(iW,out cW)){if(!kf.TryGetValue(iW,out cW))throw new FormatException($"unknown template '{iW}'");}var fb=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var mM in cW.gG)fb[mM.Key]=mM.Value;var k7=aI(pN);for(int dC=0;dC<k7.Count;dC++){string e1=k7[dC];int hX=e1.IndexOf('=');if(hX>=0){
string q1=gV(e1.Substring(0,hX));string sJ=ad(e1.Substring(hX+1).Trim());fb[q1]=sJ;}else{if(dC<cW.eM.Count){string q2=cW.eM[dC];
fb[q2]=ad(e1.Trim());}}}foreach(var p7 in cW.gA){string dN=p7;foreach(var fD in fb){dN=dN.Replace("$"+fD.Key,fD.Value).Replace("{"+fD.Key+"}",fD.Value);
}if(dN.StartsWith("@use")){if(lP>3)throw new FormatException("@use nested too deep");ki(dN,n9,m8,qu,lP+1);
}else m8.Add(dN);}}static void p(y qk,string qr){if(qk==null)throw new FormatException($"'{qr}' used before any @screen declared");
}static string kM(string aW){if(string.IsNullOrEmpty(aW))return aW;bool ix=false;for(int bB=0;bB<aW.Length;bB++){char hG=aW[bB];if(hG=='"'){ix= !ix;
}else if(!ix){if(hG=='/'&&bB+1<aW.Length&&aW[bB+1]=='/'){return aW.Substring(0,bB);}if(hG=='#'){if(oY(aW,bB)){continue;}return aW.Substring(0,bB);}}}return aW;
}static bool oY(string b9,int mC){int cJ=0;for(int it=mC+1;it<b9.Length;it++){char cx=b9[it];if((cx>='0'&&cx<='9')||(cx>='a'&&cx<='f')||(cx>='A'&&cx<='F'))cJ++;
else break;}if(cJ==3||cJ==4||cJ==6||cJ==8){int d3=mC+1+cJ;if(d3>=b9.Length||char.IsWhiteSpace(b9[d3])||b9[d3]==')'||b9[d3]==','||b9[d3]==':')return true;
}return false;}static a_ o6(string rk){string jo=rk.Substring("@bind".Length).Trim();int hY=jo.IndexOf('=');if(hY<0)throw new FormatException("missing '=' in @bind");
string m3=jo.Substring(0,hY).Trim();string h_=jo.Substring(hY+1).Trim();int ja=h_.IndexOf('(');if(ja<0)throw new FormatException("missing '(' in @bind target");
string fn=h_.Substring(0,ja).Trim().ToLowerInvariant();string pO=aH(h_,ja);var bu=aI(pO);if(bu.Count==0)throw new FormatException("@bind target has no arguments");
var an=new a_{V=m3};if(fn=="dynamic"||fn=="var"||fn=="mem"){an.bk=aN.db;an.a0=bu.Count>0 ? ad(bu[0].Trim()): m3;
for(int hg=0;hg<bu.Count;hg++){var fE=bu[hg].Split(new[]{':'},2);if(fE.Length==2&&fE[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double lO;
if(double.TryParse(fE[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out lO))an.eD=lO;else an.gF=ad(fE[1].Trim());
}}return an;}an.bk=fn=="group" ? aN.gP : aN.ow;string e_=null;for(int eZ=0;eZ<bu.Count;eZ++){var cI=bu[eZ].Split(new[]{':'},2);if(cI.Length==2&&cI[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
e_=cI[1].Trim().ToLowerInvariant();}else if(cI.Length==2&&cI[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){an.ev=cI[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{an.aJ.Add(ad(bu[eZ].Trim()));}}if(an.aJ.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(an.bk==aN.gP){if(e_==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(e_){case "avg": an.c_=az.gu;break;case "sum": an.c_=az.kO;break;case "min": an.c_=az.kC;break;case "max": an.c_=az.kB;
break;default: throw new FormatException($"unknown agg '{e_}' (expected avg/sum/min/max)");}}return an;}static bl o8(string e8){var o=new bl();
if(!e8.StartsWith("\""))throw new FormatException("line must start with a quoted label");int hW=e8.IndexOf('"',1);if(hW<0)throw new FormatException("unterminated quoted label");
o.T=e8.Substring(1,hW-1);string ec=e8.Substring(hW+1).Trim();int fP=ec.IndexOf('(');if(fP<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string C=ec.Substring(0,fP).Trim().ToLowerInvariant();string pK=aH(ec,fP);string Z=ec.Substring(ec.IndexOf(')',fP)+1).Trim();
var hT=pK.Trim().Split('.');if(hT.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.aF=hT[0].Trim();o.F=hT[1].Trim();if(C=="bar")o.g=e.c1;else if(C=="led")o.g=e.dg;else if(C=="spark"||C=="sparkline")o.g=e.bW;
else if(C=="hazard"||C=="stripes")o.g=e.df;else if(C=="chevron"||C=="chevrons")o.g=e.c7;else if(C=="dots"||C=="dot")o.g=e.da;
else if(C=="balance"||C=="horizon"||C=="centered")o.g=e.cj;else if(C=="toggle"||C=="switch")o.g=e.bZ;else if(C=="checkbox"||C=="check")o.g=e.bQ;
else if(C=="status"||C=="badge"||C=="state")o.g=e.bX;else if(C=="ammo"||C=="pips")o.g=e.ch;else if(C=="flow"||C=="rate"||C=="spinner")o.g=e.bS;
else if(C=="text")o.g=e.g1;else throw new FormatException($"unknown display function '{C}'");while(Z.Length>0){int hR=Z.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);
int cD=Z.IndexOf('(');if(hR>=0&&(cD<0||hR<cD)){string fe=Z.Substring(hR+4).Trim();int f6=fe.IndexOf(' ');o.kg=(f6>=0 ? fe.Substring(0,f6): fe).ToLowerInvariant();
Z=(f6>=0 ? fe.Substring(f6): "").Trim();continue;}int jZ=Z.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(jZ>=0&&(cD<0||jZ<cD)){
string gp=Z.Substring(jZ+5).Trim();int f7=gp.IndexOf(' ');o.dp=(f7>=0 ? gp.Substring(0,f7): gp).Trim().Trim('"');Z=(f7>=0 ? gp.Substring(f7): "").Trim();
continue;}int h8=Z.IndexOf("format:",StringComparison.OrdinalIgnoreCase);if(h8>=0&&(cD<0||h8<cD)){string fi=Z.Substring(h8+7).Trim();
int f8=fi.IndexOf(' ');o.dc=(f8>=0 ? fi.Substring(0,f8): fi).Trim().Trim('"');Z=(f8>=0 ? fi.Substring(f8): "").Trim();continue;}int fI=Z.IndexOf('(');
if(fI<0)break;string ap=Z.Substring(0,fI).Trim().ToLowerInvariant();int mR=ap.LastIndexOf(' ');if(mR>=0)ap=ap.Substring(mR+1).Trim();
string a6=aH(Z,fI);Z=Z.Substring(Z.IndexOf(')',fI)+1).Trim();if(ap=="gradient"){o.cn=true;o.bj=aI(a6).Select(r8=>r8.Trim()).ToArray();
if(o.bj.Length<2)throw new FormatException("gradient() needs at least 2 colors");}else if(ap=="flat"){o.at=a6.Trim();}else if(ap=="pattern"){
string N=a6.Trim().ToLowerInvariant();if(N=="hazard"||N=="stripes")o.g=e.df;else if(N=="chevron"||N=="chevrons")o.g=e.c7;
else if(N=="dots"||N=="dot")o.g=e.da;else if(N=="led")o.g=e.dg;else if(N=="balance"||N=="horizon")o.g=e.cj;
else if(N=="toggle"||N=="switch")o.g=e.bZ;else if(N=="checkbox"||N=="check")o.g=e.bQ;else if(N=="status"||N=="badge"||N=="state")o.g=e.bX;
else if(N=="ammo"||N=="pips")o.g=e.ch;else if(N=="flow"||N=="rate"||N=="spinner")o.g=e.bS;}else if(ap=="color"){string qV;
o.c9=kG(a6,out qV);}else if(ap=="dir"||ap=="direction"){o.kg=a6.Trim().ToLowerInvariant();}else if(ap=="unit"){
o.dp=a6.Trim().Trim('"');}else if(ap=="max"){double mZ;if(double.TryParse(a6.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mZ))o.eL=mZ;
else{var iT=a6.Trim().Split('.');if(iT.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.cp=iT[0].Trim();o.gT=iT[1].Trim();
}}else if(ap=="format"||ap=="fmt"){o.dc=a6.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{ap}'");}}return o;}static string aH(string f2,int i0){
int hQ=0;for(int dV=i0;dV<f2.Length;dV++){if(f2[dV]=='(')hQ++;else if(f2[dV]==')'){hQ--;if(hQ==0)return f2.Substring(i0+1,dV-i0-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aI(string pP){return pP.Split(',').Select(r9=>r9.Trim()).Where(sa=>sa.Length>0).ToList();
}static List<aG>kG(string pJ,out string h4){var f0=new List<aG>();h4=null;foreach(var jb in aI(pJ)){int hN=jb.LastIndexOf(':');
if(hN<0)throw new FormatException("color() entries need 'expr : color'");string fF=jb.Substring(0,hN).Trim();string lH=jb.Substring(hN+1).Trim();
if(fF.Equals("default",StringComparison.OrdinalIgnoreCase)){f0.Add(new aG{co=true,bR=lH});}else{string nR=kN(fF);
if(h4==null)h4=fF.Substring(0,fF.Length-nR.Length).Trim();f0.Add(new aG{gH=nR,bR=lH});}}if(!f0.Any(rX=>rX.co))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return f0;}static void kL(string rY,out string p4,out string qA){var d6=(rY ?? "").Split('.');if(d6.Length!=2||d6[0].Trim().Length==0||d6[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
p4=d6[0].Trim();qA=d6[1].Trim();}static string kN(string cC){cC=cC.Trim();foreach(var rv in new[]{">=","<=","==",">","<"}){int iu=cC.IndexOf(rv,StringComparison.Ordinal);
if(iu>0)return cC.Substring(iu).Trim();if(iu==0)return cC;}throw new FormatException($"color() rule has no recognizable comparison operator: '{cC}'");}
static string ad(string ca){if(ca.Length>=2&&ca[0]=='"'&&ca[ca.Length-1]=='"')return ca.Substring(1,ca.Length-2);return ca;}}public static class ou{public static double ot(az pF,List<double>cY){
if(cY==null||cY.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(pF){case az.gu: return cY.Average();case az.kO: return cY.Sum();
case az.kC: return cY.Min();case az.kB: return cY.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class D{
public static string pk(string rb,double fR,int lf=10,string of=null){fR=Math.Max(0,Math.Min(100,fR));int mc=(int)Math.Round(lf*(fR/100.0));
string p_=new string('#',mc)+new string('-',lf-mc);string sK=of!=null ? of : $"{fR:0}%";return $"{rb} [{p_}] {sK}";
}public static string ko(double bL,string jY=null,string fm=null){if(jY=="auto"){double hh=Math.Abs(bL);if(hh>=1e9)return(bL/1e9).ToString("F1")+"G";
if(hh>=1e6)return(bL/1e6).ToString("F1")+"M";if(hh>=1e3)return(bL/1e3).ToString("F1")+"k";string qy= !string.IsNullOrEmpty(fm)? fm : "F0";return bL.ToString(qy);
}string me= !string.IsNullOrEmpty(fm)? fm :(Math.Abs(bL)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(jY)){return bL.ToString(me);}return bL.ToString(me)+jY;
}public static float oT(string rN,float nU,float jP,float ff=1.0f){string cM=(rN ?? "standard").ToLowerInvariant();
if(cM=="compact"||(cM=="auto"&&nU<=300&&jP<=300&&jP>160)){return ff*0.75f;}if(cM=="corner"||(cM=="auto"&&jP<=160)){
return ff*0.55f;}if(cM=="wide"||(cM=="auto"&&nU>=1000)){return ff*1.15f;}return ff;}public static string pi(string rc,double fS,int bz=5){
fS=Math.Max(0,Math.Min(100,fS));double fg=(fS-50.0)/50.0;int d1=0;int ef=0;if(fg< -0.01)d1=(int)Math.Round(-fg*bz);
else if(fg>0.01)ef=(int)Math.Round(fg*bz);if(d1>bz)d1=bz;if(ef>bz)ef=bz;string rf=new string('-',bz-d1)+new string('<',d1);
string r2=new string('>',ef)+new string('-',bz-ef);return $"{rc} [{rf}|{r2}] {fS:0}%";}public static string pt(string mO,double sM){
return sM>=50.0 ? $"{mO} [====O] ON" : $"{mO} [O====] OFF";}public static string pn(string mP,double sN){return sN>=50.0 ? $"{mP} [X] ACTIVE" : $"{mP} [ ] READY ";
}public static string ps(string iL,double oe){if(oe>=75.0)return $"{iL} [ ONLINE  ]";if(oe>=25.0)return $"{iL} [ STANDBY ]";return $"{iL} [ OFFLINE ]";
}public static string ph(string rd,double fT,int mV=8){fT=Math.Max(0,Math.Min(100,fT));int md=(int)Math.Round(mV*(fT/100.0));
string rJ=new string('|',md)+new string('.',mV-md);return $"{rd} [{rJ}] {fT:0}%";}public static string po(string iM,double ea,int n4=0){
char[]sn=new[]{'|','/','-','\\'};char ny=sn[(n4>=0 ? n4 : 0)%4];if(ea>55.0)return $"{iM} {ny} ▲ CHG {ea:0}%";if(ea<45.0)return $"{iM} {ny} ▼ DSC {ea:0}%";
return $"{iM} ● STBY {ea:0}%";}public static bool o1(string aq,string dY){if(aq==null||dY==null)return false;if(aq=="*"||aq==dY)return true;
if(!aq.Contains("*")&& !aq.Contains("?"))return string.Equals(aq,dY,StringComparison.OrdinalIgnoreCase);int av=0,dW=0,jI= -1,iS=0;while(dW<dY.Length){
if(av<aq.Length&&(aq[av]=='?'||char.ToLowerInvariant(aq[av])==char.ToLowerInvariant(dY[dW]))){av++;dW++;}else if(av<aq.Length&&aq[av]=='*'){
jI=av;iS=dW;av++;}else if(jI!= -1){av=jI+1;iS++;dW=iS;}else{return false;}}while(av<aq.Length&&aq[av]=='*')av++;return av==aq.Length;
}public static int oy(double rz,int n7){return Math.Max(0,Math.Min(n7,1+(int)(Math.Max(0.0,rz)*n7)));}public static string eG(string[]cV,double jc){
if(cV==null||cV.Length==0)return "white";jc=Math.Max(0,Math.Min(100,jc));int iv=(int)Math.Floor(jc/100.0*cV.Length);if(iv>=cV.Length)iv=cV.Length-1;
return cV[iv];}public static n kq(string[]bJ,double rG){if(bJ==null||bJ.Length==0)return n.eS;if(bJ.Length==1)return n.dj(bJ[0]);
double r0=Math.Max(0.0,Math.Min(1.0,rG/100.0));double nC=r0*(bJ.Length-1);int iw=(int)Math.Floor(nC);int qU=Math.Min(bJ.Length-1,iw+1);
double jM=nC-iw;n cy=n.dj(bJ[iw]);n hH=n.dj(bJ[qU]);byte rZ=(byte)Math.Round(cy.bn+(hH.bn-cy.bn)*jM);
byte qO=(byte)Math.Round(cy.bi+(hH.bi-cy.bi)*jM);byte pY=(byte)Math.Round(cy.bd+(hH.bd-cy.bd)*jM);return new n(rZ,qO,pY);}public static string eO(List<aG>nu,double sO){
foreach(var jq in nu){if(jq.co)continue;if(oP(jq.gH,sO))return jq.bR;}var lN=nu.FirstOrDefault(r_=>r_.co);return lN!=null ? lN.bR : null;
}static bool oP(string dO,double eq){dO=dO.Trim();foreach(var i_ in new[]{">=","<=","==",">","<"}){if(dO.StartsWith(i_)){double ee=double.Parse(dO.Substring(i_.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(i_){case ">=": return eq>=ee;case "<=": return eq<=ee;case "==": return Math.Abs(eq-ee)<1e-9;case ">": return eq>ee;case "<": return eq<ee;
}}}throw new FormatException($"unsupported comparison expression: '{dO}'");}public static List<string>pm(List<string>qj,int cZ){
var fK=new List<string>();fK.Add("+"+new string('-',cZ+2)+"+");foreach(var iP in qj){string sF=iP.Length>cZ ? iP.Substring(0,cZ): iP;
string rA=sF.PadRight(cZ);fK.Add("| "+rA+" |");}fK.Add("+"+new string('-',cZ+2)+"+");return fK;}public struct kF{public float kU,kV,kT,ks;
}public static kF oD(int lF,int nr,int f9,int ga,int em,int en,float sw,float sv,float nc,float nd,float ih,float ii){
em=Math.Max(1,em);en=Math.Max(1,en);f9=Math.Max(1,Math.Min(em-lF,f9));ga=Math.Max(1,Math.Min(en-nr,ga));
float pR=sw-(nc*2f);float pQ=sv-(nd*2f);float lA=(pR-(ih*(em-1)))/em;float lz=(pQ-(ii*(en-1)))/en;
float rP=nc+lF*(lA+ih);float rQ=nd+nr*(lz+ii);float rO=f9*lA+((f9-1)*ih);float rH=ga*lz+((ga-1)*ii);return new kF{
kU=rP,kV=rQ,kT=rO,ks=rH};}public struct kJ{public int o4;public int gS;public int W;public int au;public int dn;
public int aO;public int pz;public int pA;public int oM;public int oN;
public double oO;}public static kJ oE(y bH){var k=new kJ();var nf=bH.ac!=null&&bH.ac.Count>0 ? bH.ac : new List<aA>{
new aA{aQ=bH.eK}};k.o4=nf.Count;k.gS=0;k.W=0;k.au=0;if(!string.IsNullOrEmpty(bH.be)&& !bH.be.Equals("none",StringComparison.OrdinalIgnoreCase))k.W++;
if(!string.IsNullOrEmpty(bH.c6)&& !bH.c6.Equals("none",StringComparison.OrdinalIgnoreCase))k.W+=2;foreach(var d7 in nf){
if(!string.IsNullOrEmpty(d7.ci)&& !d7.ci.Equals("none",StringComparison.OrdinalIgnoreCase))k.W++;if(!string.IsNullOrEmpty(d7.bY))k.W+=2;
k.gS+=d7.aQ.Count;foreach(var ra in d7.aQ){k.pz++;k.pA++;k.W+=2;switch(ra.g){case e.c1: k.dn++;
k.aO++;k.W+=2;k.au++;break;case e.dg: k.aO+=10;k.W+=10;
k.au+=10;break;case e.df: k.aO+=10;k.W+=11;k.au+=10;break;case e.c7: k.aO+=10;
k.W+=10;k.au+=10;break;case e.da: k.aO+=12;k.W+=12;k.au+=12;
break;case e.bW: k.dn++;k.aO+=8;k.W+=9;k.au+=8;break;case e.cj: k.dn+=2;
k.aO++;k.W+=3;k.au++;break;case e.bZ: case e.bQ: k.dn++;
k.aO++;k.W+=2;k.au++;break;case e.bX: case e.bS: k.dn++;k.aO+=2;
k.W+=3;k.au+=2;break;case e.ch: k.aO+=8;k.W+=8;k.au+=8;
break;default: k.au++;break;}}}k.oM=25+(k.gS*2);k.oN=180+(k.au*4);
k.oO=0.0;return k;}}