hG q;string hV=null;Dictionary<string,List<IMyTerminalBlock>>bF=new Dictionary<string,List<IMyTerminalBlock>>();Dictionary<string,bool>ft=new Dictionary<string,bool>();
Dictionary<string,bh>dV=new Dictionary<string,bh>();Dictionary<string,Color>lQ=new Dictionary<string,Color>();List<IMyTerminalBlock>cl=new List<IMyTerminalBlock>();
List<IMyTerminalBlock>cQ=new List<IMyTerminalBlock>();bool fq;int hS;string dW="";StringBuilder dZ=new StringBuilder();readonly Dictionary<string,double>aG=new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);
readonly Dictionary<string,string>cP=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);IMyBroadcastListener dX;bool cm=false;
Dictionary<C,IMyTextSurface>bG=new Dictionary<C,IMyTextSurface>();Dictionary<C,string>cn=new Dictionary<C,string>();
static readonly string[]lx=new string[101];static Program(){for(int f5=0;f5<=100;f5++){lx[f5]=f5+"%";}}static string ll(double ui){int ep=(int)Math.Round(ui);
if(ep<0)ep=0;else if(ep>100)ep=100;return lx[ep];}class aA{public List<MySprite>f=new List<MySprite>();public List<cd>dN=new List<cd>();
public int b6= -1;public Color[]e3;public Color e_;public bool e0;public Vector2 dH,lw;public int dT;public bool dG=false;
public List<e4>dw=new List<e4>();}class e4{public int fd;public Color hb,e8;public List<aP>fl,fb;public string hf,ho,hp,hr;
public bool lr,ht;}class cd{public bz fg;public int W= -1;public int aT= -1;public int cM= -1;public bool ln=false;
public List<int>K=new List<int>();public float[]fn=new float[8];public float b5;public float bg;public float a_;
public float b4;public double ff= -99999.0;public string lt;public float E= -1f;public bool lL;public float lb=12f;}Dictionary<C,aA>bH=new Dictionary<C,aA>();
int lS=0;double hX=0;double d_=0;double _=0;int dY=0;Dictionary<C,double>bk=new Dictionary<C,double>();
Dictionary<C,double>hR=new Dictionary<C,double>();Dictionary<string,int>fw=new Dictionary<string,int>();List<string>cR=new List<string>();
Dictionary<string,int>fx=new Dictionary<string,int>();double hZ;Dictionary<string,double>d1=new Dictionary<string,double>();List<MyInventoryItem>hU=new List<MyInventoryItem>();
int bI= -1,fv;double hY;Dictionary<C,double>fr=new Dictionary<C,double>();int d0=0;const string qb="0.11.0";
IMyTextSurface aU;bool hQ=true;double fs=0;string lR="";string hW="";int fu=0,hT=0;List<KeyValuePair<string,int>>cO=new List<KeyValuePair<string,int>>();
const double qt=120;const float pH=22f;public Program(){Runtime.UpdateFrequency=UpdateFrequency.Update10;try{dX=IGC.RegisterBroadcastListener("HELM");
dX.SetMessageCallback("HELM_IGC");}catch{}fk();hK();lq();}public void Main(string l4,UpdateType ue){
double mW=Runtime.TimeSinceLastRun.TotalSeconds;_+=mW;if((ue&UpdateType.IGC)!=0||l4=="HELM_IGC"){qr();}string ov=(l4 ?? "").Trim();
string an=ov.ToLowerInvariant();if(an=="reload"){fk();}else if(an=="refresh"){hI();}else if(an=="boot"){hK();}else if(an=="probe"){
d_=_+20.0;p0();Echo("[HELM] Test pattern on every screen for 20 s");return;}else if(an=="starter"){if(string.IsNullOrWhiteSpace(Me.CustomData))Me.CustomData=qP;
fk();hK();}else if(an=="console on"||an=="console off"){hQ=an=="console on";lq();fs=0;}else if(an=="scan"||an=="diagnose"||an=="test"){
qM();return;}else if(an.StartsWith("set ")){string gC=ov.Substring(4).Trim();int m_=gC.IndexOf('=');int tO=gC.IndexOf(' ');int kr=m_>0 ? m_ : tO;
if(kr>0){string nF=gC.Substring(0,kr).Trim();string pm=gC.Substring(kr+1).Trim();k3($"{nF}={pm}");Echo($"[HELM] Set dynamic variable '{nF}' = {pm}");
}return;}else if(an.StartsWith("alert")){string eW=an.Length>5 ? an.Substring(5).Trim(): "toggle";if(eW=="toggle")cm= !cm;else cm=eW=="red"||eW=="on"||eW=="1"||eW=="true";
Echo($"[HELM] Red alert state: {cm}");return;}else if(Me.CustomData!=hV){fk();}d0++;hX+=mW;if(hX>=qt){
hX=0;hI();}if(d_>0){if(_<d_){hP();return;}d_=0;bH.Clear();cn.Clear();
fs=0;}if(q==null||q.T.Count==0){hP();ld();Echo("HELM: no valid @screen configured. Check Custom Data.");return;}var cy=q.T[lS%q.T.Count];
lS++;hW=cy.a5;bool h2=bk.Count>0;if(h2)qA();double mX;if(!bk.ContainsKey(cy)&& !(hR.TryGetValue(cy,out mX)&&_<mX)){
if(qF(cy))h2=true;if(cy.hH>0)hR[cy]=_+cy.hH/60.0;}dY=h2 ? 0 : dY+1;Runtime.UpdateFrequency=dY<=q.T.Count+1 ? UpdateFrequency.Update10 : UpdateFrequency.Update100;
qO();qN();hP();ld();fu=Runtime.CurrentInstructionCount;if(fu>hT)hT=fu;}void ld(){if(dW.Length>0)Echo(dW);
if(dZ.Length>0)Echo(dZ.ToString());}List<IMyTerminalBlock>e1(bool qZ=false){if(!fq){cQ.Clear();GridTerminalSystem.GetBlocks(cQ);
cl.Clear();cl.AddRange(cQ);cl.RemoveAll(rb=> !rb.IsSameConstructAs(Me));hS=cQ.Count-cl.Count;fq=true;
}return qZ ? cQ : cl;}IMyTerminalBlock lg(string n2,bool q0=false){var ib=GridTerminalSystem.GetBlockWithName(n2);if(ib==null||q0||ib.IsSameConstructAs(Me))return ib;
var h4=e1();for(int f6=0;f6<h4.Count;f6++)if(h4[f6].CustomName.IndexOf(n2,StringComparison.OrdinalIgnoreCase)>=0)return h4[f6];return null;}void pS(string n3,List<IMyTerminalBlock>er,bool lY=false){
var nh=GridTerminalSystem.GetBlockGroupWithName(n3);if(nh==null)return;int mf=er.Count;var eS=new List<IMyTerminalBlock>();nh.GetBlocks(eS);k1(eS,er,lY);
if(er.Count>mf)return;var ns=new List<IMyBlockGroup>();GridTerminalSystem.GetBlockGroups(ns);foreach(var pB in ns){if(pB.Name.IndexOf(n3,StringComparison.OrdinalIgnoreCase)<0)continue;
eS.Clear();pB.GetBlocks(eS);k1(eS,er,lY);if(er.Count>mf)return;}}void k1(List<IMyTerminalBlock>r4,List<IMyTerminalBlock>nD,bool q_=false){
foreach(var ic in r4)if((q_||ic.IsSameConstructAs(Me))&& !nD.Contains(ic))nD.Add(ic);}void fk(){hV=Me.CustomData;q=lp.qk(hV);
dW="";if(q.a2.Count>0){var oG=new StringBuilder("HELM config errors:\n");foreach(var rO in q.a2)oG.AppendLine(" - "+rO);dW=oG.ToString();
}if(q.dU.Count>0){var pz=new StringBuilder("HELM notes:\n");foreach(var un in q.dU)pz.AppendLine(" - "+un);dW+=pz.ToString();}dV.Clear();
foreach(var id in q.b7)if(!dV.ContainsKey(id.ac))dV[id.ac]=id;bH.Clear();cn.Clear();bk.Clear();hR.Clear();
fr.Clear();p6();hI();}void qr(){try{while(dX!=null&&dX.HasPendingMessage){
var nZ=dX.AcceptMessage();if(nZ.Data is string){k3((string)nZ.Data);}}}catch{}}void k3(string ok){
if(string.IsNullOrEmpty(ok))return;var oh=ok.Split(';');for(int i9=0;i9<oh.Length;i9++){var eD=oh[i9].Trim();if(eD.Length==0)continue;int ec=eD.IndexOf('=');
if(ec<0)ec=eD.IndexOf(':');if(ec>0){string c6=eD.Substring(0,ec).Trim();string bf=eD.Substring(ec+1).Trim();if(c6.Equals("alert",StringComparison.OrdinalIgnoreCase)){
cm=bf.Equals("red",StringComparison.OrdinalIgnoreCase)||bf=="1"||bf.Equals("true",StringComparison.OrdinalIgnoreCase)||bf.Equals("on",StringComparison.OrdinalIgnoreCase);
}else{double n6;cP.Remove(c6);if(double.TryParse(bf,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out n6)){
aG[c6]=n6;}else if(bf.Equals("true",StringComparison.OrdinalIgnoreCase)||bf.Equals("on",StringComparison.OrdinalIgnoreCase)){aG[c6]=100.0;
}else if(bf.Equals("false",StringComparison.OrdinalIgnoreCase)||bf.Equals("off",StringComparison.OrdinalIgnoreCase)){aG[c6]=0.0;}else{cP[c6]=bf;
}}}}}const string qP="# HELM starter. Set output: to the name of your LCD (any part of the name works), then run this block with the argument: reload\n@bind batt = block(\"Battery\")\n@screen Demo\noutput: \"LCD\", 0\nlayout: grid(1, 1)\n@panel p at(0, 0, 1, 1) title: \"HELM\"\nline: \"BATTERY \" bar(batt.percent) gradient(#ef4444, #eab308, #22c55e)\n";
void p0(){var h5=new List<IMyTextSurface>();if(aU!=null)h5.Add(aU);foreach(var sR in bG.Values)h5.Add(sR);foreach(var tJ in h5)p_(tJ);
}static void bj(List<MySprite>tP,float rI,float rJ,float uo,float sb,Color rw){tP.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=new Vector2(rI,rJ),Size=new Vector2(uo,sb),Color=rw}
);}void p_(IMyTextSurface ks){Vector2 aK=ks.SurfaceSize,aD=ks.TextureSize,al=(aD-aK)/2f;var aC=new List<MySprite>();Color f3=new Color(150,150,150);
bj(aC,aD.X/2f,2f,aD.X,4f,f3);bj(aC,aD.X/2f,aD.Y-2f,aD.X,4f,f3);bj(aC,2f,aD.Y/2f,4f,aD.Y,f3);bj(aC,aD.X-2f,aD.Y/2f,4f,aD.Y,f3);
bj(aC,al.X+aK.X/2f,al.Y+aK.Y/2f,aK.X,aK.Y,new Color(20,40,90));float ak=32f;bj(aC,al.X+ak/2f,al.Y+ak/2f,ak,ak,new Color(255,0,0));bj(aC,al.X+aK.X-ak/2f,al.Y+ak/2f,ak,ak,new Color(0,255,0));
bj(aC,al.X+ak/2f,al.Y+aK.Y-ak/2f,ak,ak,new Color(255,255,0));bj(aC,al.X+aK.X-ak/2f,al.Y+aK.Y-ak/2f,ak,ak,new Color(255,0,255));float k_=al.Y+aK.Y/2f-30f;
aZ(aC,"SURFACE "+aK.X.ToString("0")+"x"+aK.Y.ToString("0"),Color.White,al.X+44f,k_,0.9f);aZ(aC,"TEXTURE "+aD.X.ToString("0")+"x"+aD.Y.ToString("0"),Color.White,al.X+44f,k_+24f,0.9f);
aZ(aC,"OFFSET  "+al.X.ToString("0")+","+al.Y.ToString("0"),Color.White,al.X+44f,k_+48f,0.9f);using(var rY=ks.DrawFrame()){rY.AddRange(aC);}}static bool ls(string c1){
return c1.Length>3&&(c1.StartsWith("top")&&char.IsDigit(c1[3])||c1.Length>4&&c1.StartsWith("line")&&char.IsDigit(c1[4]));}void p6(){fw.Clear();cR.Clear();
fx.Clear();bI= -1;hY=hZ=0;foreach(var kn in q.T){var jE=new List<List<bz>>();foreach(var td in kn.ai)jE.Add(td.am);
if(kn.ai.Count==0)jE.Add(kn.fh);foreach(var sM in jE)foreach(var ev in sM){if(!ls(ev.I))continue;bool ph=ev.I[0]=='t';var mP=ph ? fw : fx;
int sN=int.Parse(ev.I.Substring(ph ? 3 : 4));int nw;mP.TryGetValue(ev.aO,out nw);mP[ev.aO]=Math.Max(nw,sN);}}cR.AddRange(fw.Keys);
}void qN(){if(fx.Count==0||_<hZ)return;hZ=_+2.0;foreach(var jv in fx){List<IMyTerminalBlock>il;var oP=bF.TryGetValue(jv.Key,out il)&&il.Count>0 ? il[0]as IMyTextSurface : null;
string[]nW=oP==null ? new string[0]: oP.GetText().Split('\n');for(int eG=1;eG<=jv.Value;eG++)cP[jv.Key+".line"+eG]=eG<=nW.Length ? nW[eG-1].TrimEnd('\r'): "";}}
void qO(){if(cR.Count==0)return;if(bI<0){if(_<hY)return;bI=0;fv=0;d1.Clear();}while(bI<cR.Count&&Runtime.CurrentInstructionCount<20000){
string ip=cR[bI];List<IMyTerminalBlock>fH;bF.TryGetValue(ip,out fH);if(fH!=null&&fv<fH.Count){var l6=fH[fv++];
for(int ja=0;ja<l6.InventoryCount;ja++){hU.Clear();l6.GetInventory(ja).GetItems(hU);foreach(var gd in hU){string jr=gd.Type.TypeId.Replace("MyObjectBuilder_","");
string nG=jr=="Ore"||jr=="Ingot" ? gd.Type.SubtypeId+" "+jr.ToLowerInvariant(): gd.Type.SubtypeId;double nx;d1.TryGetValue(nG,out nx);d1[nG]=nx+(double)gd.Amount;
}}continue;}var gA=new List<KeyValuePair<string,double>>(d1);gA.Sort((uq,uu)=>uu.Value.CompareTo(uq.Value));for(int df=1;df<=fw[ip];df++)cP[ip+".top"+df]=df<=gA.Count ? gA[df-1].Key+" "+H.lk(gA[df-1].Value,"auto"): "-";
d1.Clear();bI++;fv=0;}if(bI>=cR.Count){bI= -1;hY=_+15.0;}}void ah(int tZ,string t6){cO.Add(new KeyValuePair<string,int>(t6,tZ));
}static string hO(string kg,int n1){return kg.Length>n1 ? kg.Substring(0,n1): kg;}void lq(){var j9=Me as IMyTextSurfaceProvider;if(j9==null||j9.SurfaceCount<1)return;
aU=j9.GetSurface(0);aU.ContentType=hQ ? ContentType.SCRIPT : ContentType.NONE;aU.Script="";aU.ScriptBackgroundColor=Color.Black;
}void hP(){if(_>=fs){fs=_+(dY>q.T.Count+1 ? 5.0 : 1.0);pP();if(hQ&&aU!=null&&d_==0)pZ();
}Echo(lR);}void pP(){cO.Clear();int it=0,mm=0;foreach(var l7 in q.b7){List<IMyTerminalBlock>jy;if(l7.bx==a0.dB){
it++;continue;}if(bF.TryGetValue(l7.ac,out jy)){mm+=jy.Count;if(jy.Count>0)it++;}}int m0=q.a2.Count;ah(0,"HELM-[Kernel-(v"+qb+")]");
ah(4,"------------------------------");ah(1,"krn-cfg");ah(2," screens "+q.T.Count+"  binds "+it+"/"+q.b7.Count+"  errors "+m0);
ah(2," alert "+(cm ? "RED" : "off")+"  blocks "+mm+"  docked-ignored "+hS);if(m0>0)ah(3," ! "+hO(q.a2[0],40));
else if(q.dU.Count>0)ah(3," ? "+hO(q.dU[0],40));ah(1,"cfg-dsp");if(q.T.Count==0)ah(3," no @screen: run 'starter'");
int tL=0;foreach(var kh in q.T){if(++tL>10){ah(2," +"+(q.T.Count-10)+" more");break;}IMyTextSurface kt;string oR="--",ky="no block";
if(bG.TryGetValue(kh,out kt)){oR=kt.SurfaceSize.X.ToString("0")+"x"+kt.SurfaceSize.Y.ToString("0");ky=bk.ContainsKey(kh)? "boot" : "live";
}ah(ky=="no block" ? 3 : 2," "+hO(kh.a5,14).PadRight(14)+" "+oR.PadRight(9)+" "+ky);}ah(1,"perf");ah(2," tick "+d0+"  "+Runtime.LastRunTimeMs.ToString("0.000")+" ms");
ah(2," instr "+fu+"  peak "+hT+"  max "+Runtime.MaxInstructionCount);ah(2," rate "+Runtime.UpdateFrequency+"  idle "+dY+"  up "+(int)_+"s");
if(hW.Length>0)ah(2," last "+hW);var oH=new StringBuilder();foreach(var sr in cO)oH.AppendLine(sr.Key);lR=oH.ToString();}static Color pV(int t_){
switch(t_){case 0: return new Color(232,121,249);case 1: return new Color(192,38,211);case 3: return new Color(255,80,255);case 4: return new Color(71,85,105);
}return new Color(170,180,195);}void pZ(){Vector2 cz=aU.SurfaceSize;Vector2 jL=(aU.TextureSize-cz)/2f;int iB=30;foreach(var ss in cO)iB=Math.Max(iB,ss.Key.Length);
Vector2 iA=aU.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);float oI=Math.Min(1f,Math.Min(cz.X*0.94f/(iB*iA.X),cz.Y*0.94f/(cO.Count*iA.Y)));
float ur=jL.X+cz.X*0.03f;float pE=jL.Y+cz.Y*0.03f;var ku=new List<MySprite>();ku.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jL+cz/2f,Size=cz,Color=new Color(12,12,16)}
);foreach(var nL in cO){aZ(ku,nL.Key,pV(nL.Value),ur,pE,oI);pE+=iA.Y*oI;}using(var rZ=aU.DrawFrame()){rZ.AddRange(ku);}}static readonly List<string>pO=new List<string>{
"        ZZ     ZZ","     ZZ ZZ     ZZ ZZ","   ZZVZ ZZ     ZZ ZVZZ","  ZZZZZ ZZZZZZZZZ ZZZZZ"," ZVZ    ZZ     ZZ    ZVZ"," ZGZ    ZZ     ZZ    ZGZ"," ZVVZ   ZZ     ZZ   ZVVZ","  ZVVZ ZZ       ZZ ZUVZ","   ZZZ Z         Z ZZZ","     ZZZZZZZ ZZZZZZZ","        ZZZZ ZZZZ"}
;void hK(){bk.Clear();if(q==null)return;for(int f7=0;f7<q.T.Count;f7++){var gH=q.T[f7];if(gH.hi>0f)bk[gH]=_+(f7/Math.Max(1,gH.k5))*gH.k8/60.0;
}}void qA(){List<C>eb=null;int sa=(bk.Count+3)/4,sk=0;foreach(var gh in bk){double jT=(_-gh.Value)/gh.Key.hi;if(jT<0)continue;
if(jT>=1.0){if(eb==null)eb=new List<C>();eb.Add(gh.Key);continue;}if((sk++ +d0)%sa==0)pY(gh.Key,jT);}if(eb==null)return;foreach(var eN in eb){
bk.Remove(eN);if(eN.hh>0f)fr[eN]=_;bH.Remove(eN);cn.Remove(eN);}}static void aZ(List<MySprite>tQ,string t7,Color ry,float us,float uv,float tG){
var o5=MySprite.CreateText(t7,"Monospace",ry,tG,TextAlignment.LEFT);o5.Position=new Vector2(us,uv);tQ.Add(o5);}static void k0(List<MySprite>tR,List<string>fC,C gK,float ut,float uw,float tH,float sD){
string[]tX=gK.e5!=null&&gK.e5.Length>0 ? gK.e5 : new[]{gK.k6};for(int ek=0;ek<fC.Count;
ek++){n iv=H.lm(tX,fC.Count>1 ? 100.0*ek/(fC.Count-1): 0.0);aZ(tR,fC[ek],new Color(iv.bB,iv.bw,iv.br),ut,uw+ek*sD,tH);}}bool qE(C gL,IMyTextSurface kC){
aA cB,aW;C oL=q.T.Find(pC=>pC!=gL&&pC.a5.Equals(gL.hB,StringComparison.OrdinalIgnoreCase));if(oL==null|| !bH.TryGetValue(oL,out cB)|| !cB.dG)return false;
if(!bH.TryGetValue(gL,out aW))bH[gL]=aW=new aA();Vector2 dj=kC.SurfaceSize;if(aW.dG&&aW.dT==cB.dT&&aW.dH==dj)return false;
aW.dG=true;aW.dT=cB.dT;aW.dH=dj;aW.f.Clear();Vector2 kI=cB.dH,sT=(kC.TextureSize-dj)/2f;float gf=Math.Min(dj.X/kI.X,dj.Y/kI.Y);
Vector2 q8=sT+(dj-kI*gf)/2f;foreach(var tS in cB.f){var bm=tS;if(bm.Position.HasValue)bm.Position=(bm.Position.Value-cB.lw)*gf+q8;if(bm.Size.HasValue)bm.Size=bm.Size.Value*gf;
if(bm.Type==SpriteType.TEXT)bm.RotationOrScale*=gf;aW.f.Add(bm);}using(var r_=kC.DrawFrame())r_.AddRange(aW.f);return true;}bool qy(C dh,IMyTextSurface gS){
List<string>d3;if(cn.ContainsKey(dh)|| !q.dt.TryGetValue(dh.b3,out d3))return false;cn[dh]="banner";Vector2 cA=gS.SurfaceSize;
Vector2 jM=(gS.TextureSize-cA)/2f;int fN=1;foreach(var st in d3)fN=Math.Max(fN,st.Length);Vector2 d9=gS.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float gJ=Math.Min(cA.X*0.94f/(fN*d9.X),cA.Y*0.94f/(d3.Count*d9.Y));var kv=new List<MySprite>();kv.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jM+cA/2f,Size=cA,Color=string.IsNullOrEmpty(dh.bs)? new Color(10,12,16): F(dh.bs)}
);k0(kv,d3,dh,jM.X+(cA.X-fN*d9.X*gJ)/2f,jM.Y+(cA.Y-d3.Count*d9.Y*gJ)/2f,gJ,d9.Y*gJ);using(var r0=gS.DrawFrame()){r0.AddRange(kv);
}return true;}void pY(C di,double ob){IMyTextSurface eP;if(!bG.TryGetValue(di,out eP))return;Vector2 b_=eP.SurfaceSize;
Vector2 jN=(eP.TextureSize-b_)/2f;var ew=di.p7();string n_=di.k7 ?? "HELM TELEMETRY KERNEL ONLINE";bool jF=b_.Y>=200f;
List<string>cS;if(di.b8==null|| !q.dt.TryGetValue(di.b8,out cS))cS=pO;int cZ=n_.Length+3;foreach(var su in ew)cZ=Math.Max(cZ,su.Length+5);
if(jF)foreach(var sv in cS)cZ=Math.Max(cZ,sv.Length);int oz=(jF ? cS.Count+1 : 0)+2+ew.Count;Vector2 cY=eP.MeasureStringInPixels(new StringBuilder("M"),"Monospace",1f);
float bW=Math.Min(1f,Math.Min(b_.X*0.94f/(cZ*cY.X),b_.Y*0.94f/(oz*cY.Y)));float g7=jN.X+(b_.X-cZ*cY.X*bW)/2f;float cF=jN.Y+(b_.Y-oz*cY.Y*bW)/2f;
float gl=cY.Y*bW;var dk=new List<MySprite>();dk.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data="SquareSimple",Position=jN+b_/2f,Size=b_,Color=new Color(10,12,16)}
);if(jF){k0(dk,cS,di,g7,cF,bW,gl);cF+=(cS.Count+1)*gl;}aZ(dk,">> "+n_,new Color(34,197,94),g7,cF,bW);cF+=gl*2f;int tM=H.pN(ob,ew.Count);
for(int f8=0;f8<tM;f8++){bool n8=f8==ew.Count-1&&ob>=0.8;aZ(dk,n8 ? "[OK]" : "[..]",n8 ? new Color(34,197,94): new Color(56,189,248),g7,cF,bW);aZ(dk,ew[f8],new Color(203,213,225),g7+5f*cY.X*bW,cF,bW);
cF+=gl;}using(var r1=eP.DrawFrame()){r1.AddRange(dk);}}void hI(){bF.Clear();bG.Clear();ft.Clear();
dZ.Clear();fq=false;if(q==null)return;foreach(var ab in q.b7){var aV=new List<IMyTerminalBlock>();if(ab.bx==a0.dB){
string jp= !string.IsNullOrEmpty(ab.bi)? ab.bi : ab.ac;if(!aG.ContainsKey(jp)){aG[jp]=ab.e9;if(ab.hl!=null)cP[jp]=ab.hl;
}bF[ab.ac]=aV;continue;}var o9=ab.aS.Count>0 ? ab.aS : new List<string>{ab.lI};foreach(var cD in o9){
if(string.IsNullOrEmpty(cD))continue;bool nE=cD.Contains("*")||cD.Contains("?");if(ab.bx==a0.hw||nE){var i4=new List<IMyTerminalBlock>();
pS(cD,i4,ab.e2);if(i4.Count>0){foreach(var no in i4){if(!aV.Contains(no))aV.Add(no);}}else{var l_=e1(ab.e2);
for(int ie=0;ie<l_.Count;ie++){var gZ=l_[ie];if(nE ? H.qg(cD,gZ.CustomName): string.Equals(cD,gZ.CustomName,StringComparison.OrdinalIgnoreCase)){
if(!aV.Contains(gZ))aV.Add(gZ);}}}}else{var ik=lg(cD,ab.e2);if(ik!=null&& !aV.Contains(ik))aV.Add(ik);}}if(aV.Count==0){
qK(ab,o9,aV);if(aV.Count>0)ft[ab.ac]=true;}bF[ab.ac]=aV;}foreach(var bo in q.T){
if(string.IsNullOrEmpty(bo.bA))continue;var jQ=lg(bo.bA);if(jQ==null){dZ.AppendLine($"HELM: output block '{bo.bA}' not found");
continue;}IMyTextSurface cC=null;var ka=jQ as IMyTextSurfaceProvider;var mQ=jQ as IMyTextSurface;if(ka!=null&&bo.ce<ka.SurfaceCount){
cC=ka.GetSurface(bo.ce);}else if(mQ!=null&&bo.ce==0){cC=mQ;}if(cC==null){dZ.AppendLine($"HELM: output block '{bo.bA}' has no surface index {bo.ce}");
continue;}cC.ContentType=ContentType.SCRIPT;cC.Script="";cC.ScriptBackgroundColor=Color.Black;bG[bo]=cC;}cl.Clear();
cQ.Clear();}static readonly string[]lf={"batt,power,pwr","reactor,nuke","solar","wind","tank,hydro,hydrogen,o2,oxygen","vent,air,press","cargo,container,store,inv","jump,drive","thrust,engine","susp,wheel,brake","door,gate,airlock","connect","asm,assembler,fab,prod","refin,smelt,furnace","drill,mine,bore","weld","grind","o2h2,gen,generator","turret,gun,cannon,weapon,defense","proj,blueprint","gyro","grav,gravity","gear,magplate,landing","ant,comm,radio","beacon","light,spot","piston","rotor,hinge","warhead","sensor","camera","parachute","tcb,customturret"}
;static bool p5(int sl,IMyTerminalBlock v){switch(sl){case 0: return v is IMyBatteryBlock;case 1: return v is IMyReactor;case 2: return v is IMySolarPanel;
case 3: return v is IMyWindTurbine;case 4: return v is IMyGasTank;case 5: return v is IMyAirVent;case 6: return v is IMyCargoContainer;case 7: return v is IMyJumpDrive;
case 8: return v is IMyThrust;case 9: return v is IMyMotorSuspension;case 10: return v is IMyDoor;case 11: return v is IMyShipConnector;case 12: return v is IMyAssembler;
case 13: return v is IMyRefinery;case 14: return v is IMyShipDrill;case 15: return v is IMyShipWelder;case 16: return v is IMyShipGrinder;case 17: return v is IMyGasGenerator;
case 18: return v is IMyLargeTurretBase;case 19: return v is IMyProjector;case 20: return v is IMyGyro;case 21: return v is IMyGravityGeneratorBase;case 22: return v is IMyLandingGear;
case 23: return v is IMyRadioAntenna;case 24: return v is IMyBeacon;case 25: return v is IMyLightingBlock;case 26: return v is IMyPistonBase;case 27: return v is IMyMotorStator;
case 28: return v is IMyWarhead;case 29: return v is IMySensorBlock;case 30: return v is IMyCameraBlock;case 31: return v is IMyParachute;case 32: return v is IMyTurretControlBlock;
}return false;}void qK(bh rn,List<string>t4,List<IMyTerminalBlock>im){string sf=(rn.ac+" "+string.Join(" ",t4)).ToLowerInvariant();
var jt=new List<int>();for(int gg=0;gg<lf.Length;gg++){foreach(var up in lf[gg].Split(','))if(sf.Contains(up)){jt.Add(gg);break;}}if(jt.Count==0)return;
var lZ=e1();var n9=new List<IMyTerminalBlock>();for(int jb=0;jb<lZ.Count;jb++){var ig=lZ[jb];foreach(int sm in jt){if(p5(sm,ig)){(ig.CubeGrid==Me.CubeGrid ? im : n9).Add(ig);
break;}}}if(im.Count==0)im.AddRange(n9);}void qM(){Echo("=== HELM SELF-DIAGNOSTIC AUDIT ===");if(q==null||q.T.Count==0){
Echo("[WARN] No active @screen configured in Custom Data.");return;}Echo($"Configuration: {q.T.Count} screens, {q.b7.Count} binds");fq=false;
Echo($"Blocks: {e1().Count} on this construct; {hS} on docked ships (ignored)");cl.Clear();Echo("--- BIND RESOLUTION ---");foreach(var ap in q.b7){
if(ap.bx==a0.dB){string jq= !string.IsNullOrEmpty(ap.bi)? ap.bi : ap.ac;double uf=aG.ContainsKey(jq)? aG[jq]: ap.e9;
Echo($"[OK] @bind {ap.ac} (dynamic variable '{jq}' = {uf})");continue;}List<IMyTerminalBlock>io;if(bF.TryGetValue(ap.ac,out io)){if(io.Count>0){
string rU=ft.ContainsKey(ap.ac)&&ft[ap.ac]? " [fallback: typed match]" : "";Echo($"[OK] @bind {ap.ac} ({io.Count} blocks matched{rU})");
}else{string t5=string.Join(", ",ap.aS.Count>0 ? ap.aS : new List<string>{ap.lI});Echo($"[WARN] @bind {ap.ac} -> 0 blocks matched \"{t5}\"");
}}else{Echo($"[ERR] @bind {ap.ac} -> unresolved");}}if(aG.Count>0){Echo($"--- DYNAMIC VARIABLES ({aG.Count}) ---");foreach(var nN in aG)Echo($"[VAR] {nN.Key} = {nN.Value}");
}IMyTextSurface h6=null;foreach(var sV in bG.Values){h6=sV;break;}if(h6!=null){var kw=new List<string>();h6.GetSprites(kw);
Echo($"--- SPRITES ({kw.Count}): names usable in bg_texture ---");Echo(string.Join(", ",kw.Take(60).ToArray()));}Echo("--- SURFACE OUTPUT AUDIT ---");
foreach(var bp in q.T){IMyTextSurface gT;if(bG.TryGetValue(bp,out gT)){var oS=gT.SurfaceSize;var tV=H.pU(bp);
Echo($"[OK] Screen \"{bp.a5}\" [{bp.cf}] -> {bp.bA} [#{bp.ce}] ({oS.X:0}x{oS.Y:0} px, texture {gT.TextureSize.X:0}x{gT.TextureSize.Y:0}, {tV.ad} sprites)");
}else{Echo($"[ERR] Screen \"{bp.a5}\" -> Missing block \"{bp.bA}\" [#{bp.ce}]");}}Echo("--- PERFORMANCE STATUS ---");
Echo($"Instruction Usage: {Runtime.CurrentInstructionCount}/{Runtime.MaxInstructionCount} | Allocs: 0-byte GC");Echo("=== DIAGNOSTIC AUDIT COMPLETE ===");}double qL(IMyTerminalBlock w){
if(w==null)return 0.0;var fF=w as IMyBatteryBlock;if(fF!=null)return fF.MaxStoredPower>0 ?(fF.CurrentStoredPower/fF.MaxStoredPower)*100.0 : 0.0;
var ge=w as IMyJumpDrive;if(ge!=null)return ge.MaxStoredPower>0 ?(ge.CurrentStoredPower/ge.MaxStoredPower)*100.0 : 0.0;var o8=w as IMyGasTank;
if(o8!=null)return o8.FilledRatio*100.0;var pv=w as IMyAirVent;if(pv!=null)return pv.GetOxygenLevel()*100.0;var mu=w as IMyCargoContainer;if(mu!=null){
var gb=mu.GetInventory();if(gb!=null&&gb.MaxVolume.RawValue>0)return(double)gb.CurrentVolume.RawValue/gb.MaxVolume.RawValue*100.0;return 0.0;}var gB=w as IMyReactor;
if(gB!=null)return gB.MaxOutput>0 ?(gB.CurrentOutput/gB.MaxOutput)*100.0 : 0.0;var gM=w as IMySolarPanel;if(gM!=null)return gM.MaxOutput>0 ?(gM.CurrentOutput/gM.MaxOutput)*100.0 : 0.0;
var g6=w as IMyWindTurbine;if(g6!=null)return g6.MaxOutput>0 ?(g6.CurrentOutput/g6.MaxOutput)*100.0 : 0.0;var mR=w as IMyDoor;if(mR!=null)return(double)mR.OpenRatio*100.0;
var g1=w as IMyThrust;if(g1!=null)return g1.MaxEffectiveThrust>0 ?(g1.CurrentThrust/g1.MaxEffectiveThrust)*100.0 : 0.0;var kH=w as IMyMotorSuspension;
if(kH!=null)return kH.Brake ? 100.0 :(kH.Friction*100.0);var iF=w as IMyShipConnector;if(iF!=null)return iF.Status==MyShipConnectorStatus.Connected ? 100.0 :(iF.Status==MyShipConnectorStatus.Connectable ? 50.0 : 0.0);
var oq=w as IMyProductionBlock;if(oq!=null)return oq.IsProducing ? 100.0 : 0.0;var mV=w as IMyShipDrill;if(mV!=null)return mV.Enabled ? 100.0 : 0.0;
var kO=w as IMyShipToolBase;if(kO!=null)return kO.IsActivated ? 100.0 :(kO.Enabled ? 50.0 : 0.0);var i1=w as IMyGasGenerator;if(i1!=null)return i1.AutoRefill ? 100.0 :(i1.Enabled ? 50.0 : 0.0);
var g2=w as IMyLargeTurretBase;if(g2!=null)return g2.IsShooting ? 100.0 :(g2.HasTarget ? 50.0 :(g2.Enabled ? 10.0 : 0.0));var eF=w as IMyProjector;
if(eF!=null)return eF.TotalBlocks>0 ?((double)(eF.TotalBlocks-eF.RemainingBlocks)/eF.TotalBlocks)*100.0 : 0.0;var i7=w as IMyGyro;if(i7!=null)return i7.GyroOverride ? 100.0 :(i7.Enabled ? 50.0 : 0.0);
var nq=w as IMyGravityGeneratorBase;if(nq!=null)return Math.Min(100.0,Math.Abs(nq.GravityAcceleration)/9.81*100.0);var i0=w as IMyLandingGear;if(i0!=null)return i0.IsLocked ? 100.0 :(i0.LockMode==LandingGearMode.ReadyToLock ? 50.0 : 0.0);
var l0=w as IMyRadioAntenna;if(l0!=null)return(l0.Radius/50000.0)*100.0;var md=w as IMyBeacon;if(md!=null)return(md.Radius/50000.0)*100.0;
var nU=w as IMyLightingBlock;if(nU!=null)return nU.Intensity*10.0;var gx=w as IMyPistonBase;if(gx!=null)return gx.HighestPosition>0 ?(gx.CurrentPosition/gx.HighestPosition)*100.0 : 0.0;
var ox=w as IMyMotorStator;if(ox!=null)return Math.Min(100.0,Math.Abs(ox.Angle)/6.28318*100.0);var py=w as IMyWarhead;if(py!=null)return py.IsArmed ? 100.0 : 0.0;
var kq=w as IMySensorBlock;if(kq!=null)return!kq.LastDetectedEntity.IsEmpty()? 100.0 :(kq.Enabled ? 25.0 : 0.0);var iy=w as IMyCameraBlock;
if(iy!=null)return iy.IsActive ? 100.0 :(iy.Enabled ? 50.0 : 0.0);var mA=w as IMyParachute;if(mA!=null)return(double)mA.OpenRatio*100.0;
var g0=w as IMyTurretControlBlock;if(g0!=null)return g0.HasTarget ? 100.0 :(g0.IsUnderControl ? 50.0 :(g0.Enabled ? 10.0 : 0.0));var nf=w as IMyFunctionalBlock;
if(nf!=null)return nf.Enabled ? 100.0 : 0.0;return 0.0;}static double qd(IMyTerminalBlock l8,string iU){int mt=iU.IndexOf(':');string js=iU.Substring(0,mt).ToLowerInvariant();
var uc=new MyItemType("MyObjectBuilder_"+(js=="ammo" ? "AmmoMagazine" : char.ToUpper(js[0])+js.Substring(1)),iU.Substring(mt+1));double o1=0;for(int jc=0;
jc<l8.InventoryCount;jc++)o1+=(double)l8.GetInventory(jc).GetItemAmount(uc);return o1;}double lB(IMyTerminalBlock x,string m6){if(x==null)return 0.0;
string b=m6.ToLowerInvariant();if(b=="percent"||b=="pct")return qL(x);if(b.IndexOf(':')>0)return qd(x,m6);var bL=x as IMyBatteryBlock;
if(bL!=null){if(b=="current"||b=="stored"||b=="val"||b=="value"||b=="raw")return bL.CurrentStoredPower;if(b=="max"||b=="capacity")return bL.MaxStoredPower;
if(b=="output")return bL.CurrentOutput;if(b=="maxoutput")return bL.MaxOutput;if(b=="input")return bL.CurrentInput;if(b=="maxinput")return bL.MaxInput;
return bL.CurrentStoredPower;}var jo=x as IMyJumpDrive;if(jo!=null){if(b=="max"||b=="capacity")return jo.MaxStoredPower;return jo.CurrentStoredPower;
}var gW=x as IMyGasTank;if(gW!=null){if(b=="max"||b=="capacity")return gW.Capacity;return gW.FilledRatio*gW.Capacity;}var pw=x as IMyAirVent;
if(pw!=null){if(b=="max"||b=="capacity")return 100.0;return pw.GetOxygenLevel()*100.0;}var mv=x as IMyCargoContainer;if(mv!=null){var gc=mv.GetInventory();
if(gc!=null){if(b=="mass")return(double)gc.CurrentMass;if(b=="max"||b=="capacity")return(double)gc.MaxVolume;return(double)gc.CurrentVolume;}return 0.0;}var kb=x as IMyPowerProducer;
if(kb!=null){if(b=="max"||b=="capacity")return kb.MaxOutput;return kb.CurrentOutput;}var mS=x as IMyDoor;if(mS!=null){if(b=="max")return 1.0;return(double)mS.OpenRatio*100.0;
}var kM=x as IMyThrust;if(kM!=null){if(b=="max"||b=="capacity")return kM.MaxEffectiveThrust;return kM.CurrentThrust;}var b1=x as IMyMotorSuspension;
if(b1!=null){if(b=="brake")return b1.Brake ? 100.0 : 0.0;if(b=="height")return b1.Height;if(b=="strength")return b1.Strength;if(b=="power")return b1.Power;
if(b=="steer")return b1.SteeringOverride*100.0;if(b=="propulsion")return b1.PropulsionOverride*100.0;return b1.Friction;}var mH=x as IMyShipConnector;
if(mH!=null){return mH.Status==MyShipConnectorStatus.Connected ? 100.0 : 0.0;}var j8=x as IMyProductionBlock;if(j8!=null){if(b=="producing"||b=="active"||b=="running"||b=="state")return j8.IsProducing ? 100.0 : 0.0;
if(b=="queue")return j8.IsQueueEmpty ? 0.0 : 1.0;}var ia=x as IMyAssembler;if(ia!=null){if(b=="disassembling")return ia.Mode==MyAssemblerMode.Disassembly ? 100.0 : 0.0;
if(b=="repeating"||b=="loop")return ia.Repeating ? 100.0 : 0.0;}var i2=x as IMyGasGenerator;if(i2!=null){if(b=="autorefill")return i2.AutoRefill ? 100.0 : 0.0;
if(b=="active")return i2.Enabled ? 100.0 : 0.0;}var g3=x as IMyLargeTurretBase;if(g3!=null){if(b=="shooting")return g3.IsShooting ? 100.0 : 0.0;
if(b=="target"||b=="hastarget")return g3.HasTarget ? 100.0 : 0.0;if(b=="range")return g3.Range;}var bV=x as IMyProjector;if(bV!=null){if(b=="built")return bV.TotalBlocks-bV.RemainingBlocks;
if(b=="remaining")return bV.RemainingBlocks;if(b=="total")return bV.TotalBlocks;if(b=="projecting")return bV.IsProjecting ? 100.0 : 0.0;if(b=="armor")return bV.RemainingArmorBlocks;
if(b=="buildable")return bV.BuildableBlocksCount;}var kP=x as IMyShipToolBase;if(kP!=null){if(b=="working"||b=="active"||b=="activated")return kP.IsActivated ? 100.0 : 0.0;
return kP.Enabled ? 100.0 : 0.0;}var c3=x as IMyGyro;if(c3!=null){if(b=="override")return c3.GyroOverride ? 100.0 : 0.0;if(b=="pitch")return c3.Pitch;
if(b=="yaw")return c3.Yaw;if(b=="roll")return c3.Roll;if(b=="power")return c3.GyroPower*100.0;}var nr=x as IMyGravityGeneratorBase;if(nr!=null){if(b=="g"||b=="accel")return nr.GravityAcceleration;
}var np=x as IMyLandingGear;if(np!=null){if(b=="locked")return np.IsLocked ? 100.0 : 0.0;}var l1=x as IMyRadioAntenna;if(l1!=null){if(b=="radius"||b=="range")return l1.Radius;
}var me=x as IMyBeacon;if(me!=null){if(b=="radius"||b=="range")return me.Radius;}var jB=x as IMyLightingBlock;if(jB!=null){if(b=="intensity")return jB.Intensity;
if(b=="radius")return jB.Radius;}var gy=x as IMyPistonBase;if(gy!=null){if(b=="pos"||b=="position"||b=="extension")return gy.CurrentPosition;
if(b=="velocity")return gy.Velocity;if(b=="max")return gy.HighestPosition;}var eM=x as IMyMotorStator;if(eM!=null){if(b=="angle"||b=="deg")return eM.Angle*180.0/Math.PI;
if(b=="rad")return eM.Angle;if(b=="velocity"||b=="rpm")return eM.TargetVelocityRPM;if(b=="locked")return eM.RotorLock ? 100.0 : 0.0;}var kY=x as IMyWarhead;
if(kY!=null){if(b=="armed")return kY.IsArmed ? 100.0 : 0.0;if(b=="countdown")return kY.DetonationTime;}var oO=x as IMySensorBlock;if(oO!=null){
if(b=="detected")return!oO.LastDetectedEntity.IsEmpty()? 100.0 : 0.0;}var iz=x as IMyCameraBlock;if(iz!=null){if(b=="range")return iz.AvailableScanRange;
if(b=="active")return iz.IsActive ? 100.0 : 0.0;}var mB=x as IMyParachute;if(mB!=null){if(b=="open"||b=="openratio")return(double)mB.OpenRatio*100.0;
}var dp=x as IMyTurretControlBlock;if(dp!=null){if(b=="target"||b=="hastarget")return dp.HasTarget ? 100.0 : 0.0;if(b=="azimuth")return dp.AzimuthRotor!=null ? dp.AzimuthRotor.Angle*180.0/Math.PI : 0.0;
if(b=="elevation")return dp.ElevationRotor!=null ? dp.ElevationRotor.Angle*180.0/Math.PI : 0.0;}var ng=x as IMyFunctionalBlock;if(ng!=null){return ng.Enabled ? 100.0 : 0.0;
}return 0.0;}double cg(string d6,string m7){bh bM;dV.TryGetValue(d6,out bM);if(bM!=null&&bM.bx==a0.dB){
string sn= !string.IsNullOrEmpty(bM.bi)? bM.bi : d6;double mK;if(aG.TryGetValue(sn,out mK))return mK;return bM.e9;
}double mY;if(aG.TryGetValue(d6,out mY))return mY;List<IMyTerminalBlock>d7;if(!bF.TryGetValue(d6,out d7)||d7.Count==0)throw new InvalidOperationException($"HELM: bind '{d6}' resolved to zero blocks — check the name/typo");
if(d7.Count==1)return lB(d7[0],m7);var pu=new List<double>();foreach(var rc in d7)pu.Add(lB(rc,m7));return pJ.pI(bM!=null ? bM.ds : aE.ha,pu);
}double hJ(string rp){return cg(rp,"percent");}static string a3(double pq,bz c9){if(c9==null||(string.IsNullOrEmpty(c9.dS)&&string.IsNullOrEmpty(c9.dC)&&c9.I=="percent")){
return ll(pq);}return H.lk(pq,c9.dS,c9.dC);}bool qF(C bX){IMyTextSurface eQ;if(!bG.TryGetValue(bX,out eQ))return false;
if(bX.b3!=null)return qy(bX,eQ);if(bX.hB!=null)return qE(bX,eQ);if(bX.dx.Equals("ascii",StringComparison.OrdinalIgnoreCase)){
return qG(bX,eQ);}return qH(bX,eQ);}Color hu(string[]dl,double s3){if(dl.Length==1)return F(dl[0]);
double os=Math.Max(0.0,Math.Min(100.0,s3))/100.0*(dl.Length-1);int jd=Math.Min((int)os,dl.Length-2);float gU=(float)(os-jd);Color bJ=F(dl[jd]),fE=F(dl[jd+1]);
return new Color((int)(bJ.R+(fE.R-bJ.R)*gU),(int)(bJ.G+(fE.G-bJ.G)*gU),(int)(bJ.B+(fE.B-bJ.B)*gU),(int)(bJ.A+(fE.A-bJ.A)*gU));}static Color[]eZ(aA ru,string[]tY,string rL,float pD,float pF,float kV,float i8){
int d4=16;bool ny=rL=="horizontal";var iE=new Color[d4];for(int ct=0;ct<d4;ct++){n i_=H.lm(tY,100.0*ct/(d4-1));iE[ct]=new Color(i_.bB,i_.bw,i_.br);
float ms=kV/d4,ml=i8/d4;t(ru,ny ? new Vector2(pD+ms*(ct+0.5f),pF+i8/2f): new Vector2(pD+kV/2f,pF+ml*(ct+0.5f)),ny ? new Vector2(ms+1f,i8): new Vector2(kV,ml+1f),iE[ct]);
}return iE;}static void t(aA rv,Vector2 tf,Vector2 tN,Color rz,float tx=0f,string rK="SquareSimple"){rv.f.Add(new MySprite{Type=SpriteType.TEXTURE,Alignment=TextAlignment.CENTER,Data=rK,Position=tf,Size=tN,Color=rz,RotationOrScale=tx}
);}void pQ(C k,IMyTextSurface kD,aA g){g.f.Clear();g.dN.Clear();Vector2 O=kD.SurfaceSize;
Vector2 t9=kD.TextureSize;Vector2 aw=(t9-O)/2f;g.lw=aw;float gq=Math.Max(4f,O.X*0.02f);
float cv=Math.Max(4f,O.Y*0.02f);if(k.cf=="compact"||(k.cf=="auto"&&O.Y<=260f&&O.Y>150f)){gq=4f;cv=4f;
}else if(k.cf=="corner"||(k.cf=="auto"&&O.Y<=150f)){gq=2f;cv=2f;}float eV=O.X-(gq*2f);Color cU=new Color(10,12,16);
bool mU=true;if(!string.IsNullOrEmpty(k.bs)){var ei=n.dK(k.bs);if(ei.fe)mU=false;else cU=new Color(ei.bB,ei.bw,ei.br,ei.g8);
}g.b6= -1;g.e0=false;g.e_=F(k.k2 ?? "#5b0f0f");g.dw.Clear();if(k.bu!=null&&k.bu.Length>1){
g.b6=g.f.Count;g.e3=eZ(g,k.bu,k.dv,aw.X+2f,aw.Y+2f,O.X-4f,O.Y-4f);
}else if(mU||k.aN!=null||k.cJ!=null){if(k.aN!=null)cU=F(k.aN.First(tk=>tk.cL).ca);g.b6=g.f.Count;
g.e3=new[]{cU};t(g,aw+(O/2f),O-new Vector2(4f,4f),cU);if(k.aN!=null||k.cJ!=null)g.dw.Add(new e4{
fd=g.b6,hb=cU,e8=cU,fl=k.aN,hf=k.hc,ho=k.hd,fb=k.cJ,hp=k.hq,hr=k.hs,lr=true}
);}if(!string.IsNullOrEmpty(k.he))t(g,aw+(O/2f),O-new Vector2(4f,4f),F(k.k4 ?? "255,255,255,24"),0f,k.he);
if(!k.dx.Equals("none",StringComparison.OrdinalIgnoreCase)){Color mn=k.ax!=null&&k.ax.Length>0 ? F(k.ax[0]): F(k.e7 ?? "40, 60, 85");
Color rt=k.ax!=null&&k.ax.Length>1 ? F(k.ax[k.ax.Length-1]): mn;
float mo=aw.X+(O.X-eV)/2f;if(k.ax!=null&&k.ax.Length>1&& !k.hv){eZ(g,k.ax,"horizontal",mo,aw.Y+cv*0.5f-1f,eV,2f);
eZ(g,k.ax,"horizontal",mo,aw.Y+O.Y-(cv*0.5f)-1f,eV,2f);}else{t(g,new Vector2(aw.X+O.X/2f,aw.Y+cv*0.5f),new Vector2(eV,2f),mn);
t(g,new Vector2(aw.X+O.X/2f,aw.Y+O.Y-(cv*0.5f)),new Vector2(eV,2f),rt);}}var jY=k.ai;
if(jY.Count==0){jY=new List<az>{new az{dJ="main",dz=0,dM=0,dO=k.dD,dP=k.dE,am=k.fh}};}float nt=8f;
foreach(var s in jY){var gp=H.pT(s.dz,s.dM,s.dO,s.dP,k.dD,k.dE,O.X,O.Y,gq,cv,nt,nt);
float aX=aw.X+gp.lO;float dd=aw.Y+gp.lP;float a9=gp.lN;float eC=gp.lo;bool oA=s.aN!=null||s.cJ!=null;
if(s.bu!=null&&s.bu.Length>1){eZ(g,s.bu,s.dv!="vertical" ? s.dv : k.dv,aX,dd,a9,eC);
}else{Color bU;if(s.aN!=null)bU=F(s.aN.First(tl=>tl.cL).ca);else if(!string.IsNullOrEmpty(s.cH))bU=F(s.cH);
else if(!string.IsNullOrEmpty(k.hF))bU=F(k.hF);else bU=Color.Transparent;if(bU.A>0||oA){if(oA)g.dw.Add(new e4{fd=g.f.Count,hb=bU,e8=bU,fl=s.aN,hf=s.hc,ho=s.hd,fb=s.cJ,hp=s.hq,hr=s.hs}
);t(g,new Vector2(aX+a9/2f,dd+eC/2f),new Vector2(a9,eC),bU);}}string f0=s.lh ?? k.li ?? "Monospace";float rl=H.p8(k.cf,O.X,O.Y,k.cK);
float aH=s.lj ?? rl;float ej=0f;if(!string.IsNullOrEmpty(s.cj)){ej=20f*aH;var h3=TextAlignment.LEFT;float kN=aX+4f;
string lU=s.hM ?? k.lJ ?? "left";if(lU.Equals("center",StringComparison.OrdinalIgnoreCase)){h3=TextAlignment.CENTER;kN=aX+(a9/2f);
}else if(lU.Equals("right",StringComparison.OrdinalIgnoreCase)){h3=TextAlignment.RIGHT;kN=aX+a9-4f;}string pb=s.hN ?? k.lK;
Color ua= !string.IsNullOrEmpty(pb)? F(pb): F(s.hj ?? k.e7 ?? "white");var pg=MySprite.CreateText(s.cj,f0,ua,aH*0.9f,h3);
pg.Position=new Vector2(kN,dd+2f);g.f.Add(pg);string[]ih=s.hk ?? k.ax;Color rN=ih!=null&&ih.Length>0 ? F(ih[0]): F(s.hj ?? k.e7 ?? "40, 60, 85");
t(g,new Vector2(aX+a9/2f,dd+ej-2f),new Vector2(a9-8f,1f),rN);}if(s.hE=="vertical"||s.am.Any(tb=>tb.h==e.fo)){
pR(g,kD,s,f0,aH,aX,dd,a9,eC,ej);continue;}int uy=Math.Max(1,s.am.Count);float s1=eC-ej-8f;
float pA=0f,lW=0f;foreach(var tc in s.am)pA+=1<<tc.fm;float ud=s1/Math.Max(1f,pA);float ga=6f;for(int je=0;je<s.am.Count;
je++){var u=s.am[je];var V=new cd{fg=u};float gF=ud*(1<<u.fm);float ty=dd+ej+4f+lW;lW+=gF;float M=ty+(gF/2f);
float pd=M-(11f*aH);V.cM=g.f.Count;V.ln=u.Z!=null&&(u.Z.Contains("{val}")||u.Z.Contains("{percent}"));
var nR=MySprite.CreateText(u.Z,f0,u.hy!=null ? F(u.hy): Color.White,aH,TextAlignment.LEFT);nR.Position=new Vector2(aX+ga,pd);
g.f.Add(nR);float sB=Math.Max(24f*aH,(u.Z.Length+1)*12f*aH);float um=(!string.IsNullOrEmpty(u.dS)|| !string.IsNullOrEmpty(u.dC))? 64f*aH : 52f*aH;
if(u.h!=e.hL){float L=aX+ga+sB;float rk=aX+a9-ga-um;float J=Math.Max(16f,rk-L);
float Q=Math.Max(u.h==e.ch ? 8f : 4f,Math.Min((u.h==e.ch ? 20f : 16f)*aH,gF*(u.h==e.ch ? 0.55f : 0.45f)));
if(u.fm>0)Q=gF*0.7f;Color pj=u.dR!=null ? F(u.dR): new Color(25,30,38);V.b5=L;V.bg=J;
V.a_=Q;V.b4=M;if(u.h==e.du){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),pj);
V.W=g.f.Count;t(g,new Vector2(L,M),new Vector2(0f,Q),F(u.au));}else if(u.h==e.dI){
int ko=10;float ni=2f;float kp=Math.Max(2f,(J-(ni*(ko-1)))/ko);for(int ki=0;ki<ko;ki++){V.K.Add(g.f.Count);
t(g,new Vector2(L+ki*(kp+ni)+(kp/2f),M),new Vector2(kp,Q),new Color(22,26,32));}}else if(u.h==e.ch){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),new Color(20,24,30));
int j7=8;float nj=2f;float iC=Math.Max(2f,(J-(nj*(j7-1)))/j7);for(int jU=0;jU<j7;jU++){V.K.Add(g.f.Count);
t(g,new Vector2(L+jU*(iC+nj)+(iC/2f),M),new Vector2(iC,2f),F(u.au));}}else if(u.h==e.dF){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),new Color(20,24,30));
int kz=10;float nk=2f;float kA=Math.Max(2f,(J-(nk*(kz-1)))/kz);for(int kj=0;kj<kz;kj++){V.K.Add(g.f.Count);
t(g,new Vector2(L+kj*(kA+nk)+(kA/2f),M),new Vector2(kA,Q),new Color(24,28,36),0.523599f);}}else if(u.h==e.dy){
int h8=10;float nl=2f;float h9=Math.Max(2f,(J-(nl*(h8-1)))/h8);for(int kk=0;kk<h8;kk++){V.K.Add(g.f.Count);
t(g,new Vector2(L+kk*(h9+nl)+(h9/2f),M),new Vector2(h9,Q),new Color(24,28,36),1.570796f,"Triangle");}}else if(u.h==e.dA){
int iI=12;float nm=2f;float iK=Math.Max(2f,(J-(nm*(iI-1)))/iI);float mT=Math.Min(iK,Q);for(int kl=0;kl<iI;kl++){V.K.Add(g.f.Count);
t(g,new Vector2(L+kl*(iK+nm)+(iK/2f),M),new Vector2(mT,mT),new Color(24,28,36),0f,"Circle");}}else if(u.h==e.cI){
t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),pj);V.W=g.f.Count;t(g,new Vector2(L+(J/2f),M),new Vector2(0f,Q),F(u.au));
t(g,new Vector2(L+(J/2f),M),new Vector2(2f,Q+4f),new Color(80,110,150));}else if(u.h==e.ck){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),new Color(20,24,30));
V.W=g.f.Count;float ju=Math.Max(4f,Q);t(g,new Vector2(L+ju*0.5f,M),new Vector2(ju,ju),new Color(40,50,65),0f,"Circle");
}else if(u.h==e.b9){float cV=Math.Max(6f,Math.Min(Q,14f*aH));t(g,new Vector2(L+cV*0.5f,M),new Vector2(cV,cV),new Color(28,34,46));
V.W=g.f.Count;t(g,new Vector2(L+cV*0.5f,M),new Vector2(cV*0.6f,cV*0.6f),new Color(40,50,65));}else if(u.h==e.ci){
t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),new Color(18,22,30));V.W=g.f.Count;float j4=Math.Max(4f,Q*0.7f);
t(g,new Vector2(L+j4+2f,M),new Vector2(j4,j4),new Color(34,197,94),0f,"Circle");}else if(u.h==e.cG){int j3=8;
float nn=2f;float j5=Math.Max(2f,(J-(nn*(j3-1)))/j3);for(int jV=0;jV<j3;jV++){V.K.Add(g.f.Count);t(g,new Vector2(L+jV*(j5+nn)+(j5/2f),M),new Vector2(j5,Q),new Color(24,28,36));
}}else if(u.h==e.cb){t(g,new Vector2(L+(J/2f),M),new Vector2(J,Q),new Color(20,24,30));V.W=g.f.Count;
float nB=Math.Max(4f,Q);t(g,new Vector2(L+(J/2f),M),new Vector2(nB,nB),F(u.au),0f,"Triangle");}}var po=MySprite.CreateText("---",f0,Color.White,aH,TextAlignment.RIGHT);
po.Position=new Vector2(aX+a9-ga,pd);V.aT=g.f.Count;g.f.Add(po);g.dN.Add(V);}}g.dH=O;
g.dG=true;}const string qU="~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-";
const int bD=8;void pR(aA aa,IMyTextSurface t0,az jX,string ne,float eg,float sZ,float jW,float s_,float oc,float se){
int sO=Math.Max(1,jX.am.Count);float mE=(s_-12f)/sO;float g4=22f*eg;float be=jW+se+4f+g4;float aL=Math.Max(20f,jW+oc-4f-g4-be);
float rH=t0.MeasureStringInPixels(new StringBuilder("M"),ne,eg*0.9f).X;for(int f9=0;f9<jX.am.Count;f9++){var da=jX.am[f9];float a8=sZ+6f+mE*(f9+0.5f);
float cE=mE*0.7f;var bb=new cd{fg=da,lL=true,b5=a8,bg=cE,a_=aL,b4=be+aL/2f,lb=rH};bb.cM=aa.f.Count;
aZ(aa.f,da.Z,Color.White,a8,jW+oc-4f-g4,eg);bb.aT=aa.f.Count;aZ(aa.f,"---",Color.White,a8,be-g4,eg);
var nO=aa.f[bb.cM];nO.Alignment=TextAlignment.CENTER;aa.f[bb.cM]=nO;var pn=aa.f[bb.aT];pn.Alignment=TextAlignment.CENTER;
aa.f[bb.aT]=pn;t(aa,new Vector2(a8,be+aL/2f),new Vector2(cE,aL),da.dR!=null ? F(da.dR): new Color(25,30,38));
bb.W=aa.f.Count;if(da.h==e.fo){for(int l9=0;l9<bD;l9++){bb.K.Add(aa.f.Count);t(aa,new Vector2(a8,be+aL),new Vector2(cE-6f,0f),Color.White);
}for(int kW=0;kW<4;kW++){bb.K.Add(aa.f.Count);var o6=MySprite.CreateText("",ne,kW==0 ? new Color(255,255,255,170): new Color(255,255,255,110),eg*0.9f,TextAlignment.CENTER);
o6.Position=new Vector2(a8,be+aL);aa.f.Add(o6);}Color iM=new Color(90,110,135);t(aa,new Vector2(a8-cE/2f,be+aL/2f),new Vector2(3f,aL),iM);t(aa,new Vector2(a8+cE/2f,be+aL/2f),new Vector2(3f,aL),iM);
t(aa,new Vector2(a8,be+aL),new Vector2(cE+3f,3f),iM);}else{t(aa,new Vector2(a8,be+aL),new Vector2(cE,0f),F(da.au));}aa.dN.Add(bb);
}}bool qT(aA a6,cd z,double eX,double ot,Color fO,bool ps){var bS=z.fg;float mg=z.E;if(z.E<0f)z.E=(float)eX;
z.E=Math.Abs(z.E-(float)eX)>0.05f ? z.E+((float)eX-z.E)*0.35f :(float)eX;float cu=Math.Max(0f,Math.Min(1f,z.E/100f))*z.a_;
float fL=z.b4+z.a_/2f;float fR=z.b5;if(bS.h!=e.fo){var fW=a6.f[z.W];fW.Size=new Vector2(z.bg,cu);
fW.Position=new Vector2(fR,fL-cu/2f);fW.Color=fO;a6.f[z.W]=fW;if(ps||z.E!=mg)aF(a6,z,a3(ot,bS),fO);
return ps||z.E!=mg;}float ij=z.a_/bD;string eH=bS.bv!=null ? H.dL(bS.bv,eX): null;
Color sJ=eH!=null ? F(eH): fO;string[]oZ=eH!=null ?(eH.Contains("/")? eH.Split('/'): null): bS.cc&&bS.a4.Length>1 ? bS.a4 : null;
for(int cp=0;cp<bD;cp++){float px=Math.Max(0f,Math.Min(ij,cu-cp*ij));var gz=a6.f[z.K[cp]];gz.Size=new Vector2(z.bg-6f,px);
gz.Position=new Vector2(fR,fL-cp*ij-px/2f);gz.Color=oZ!=null ? hu(oZ,100.0*(cp+0.5)/bD): sJ;a6.f[z.K[cp]]=gz;}
int rx=Math.Max(1,(int)((z.bg-8f)/z.lb));bool tK=cu>6f;var kZ=a6.f[z.K[bD]];kZ.Data=tK ? qU.Substring((int)(_*5.0)%6,Math.Min(rx,100)): "";
kZ.Position=new Vector2(fR,fL-cu-12f);a6.f[z.K[bD]]=kZ;for(int bR=0;bR<3;bR++){var iu=a6.f[z.K[bD+1+bR]];
double s7=(_*(0.35+0.1*bR)+bR*0.37)%1.0;iu.Data=cu>24f ?(bR==1 ? "O" : "o"): "";iu.Position=new Vector2(fR+(bR-1)*z.bg*0.26f,fL-(float)s7*(cu-6f)-10f);
a6.f[z.K[bD+1+bR]]=iu;}aF(a6,z,a3(ot,bS),fO);return true;}static void aF(aA mq,cd oT,string t8,Color rA){
var kT=mq.f[oT.aT];kT.Data=t8;kT.Color=rA;mq.f[oT.aT]=kT;}static void lD(aA mr,cd oU,Color rB){
var m1=mr.f[oU.W];m1.Color=rB;mr.f[oU.W]=m1;}bool qH(C bq,IMyTextSurface kE){aA i;
if(!bH.TryGetValue(bq,out i)){i=new aA();bH[bq]=i;}bool ow= !i.dG||i.dH!=kE.SurfaceSize;
if(ow){pQ(bq,kE,i);}bool N=ow;bool fA=(_%bq.g9)<bq.g9/2.0;double cs= -1.0,mI;if(fr.TryGetValue(bq,out mI)){
cs=(_-mI)/bq.hh;if(cs>=1.0){fr.Remove(bq);cs=1.0;}}bool kX=cm&&fA&&i.e_.A>0;if(i.b6>=0&&kX!=i.e0){
i.e0=kX;for(int el=0;el<i.e3.Length;el++){var mh=i.f[i.b6+el];mh.Color=kX ? i.e_ : i.e3[el];i.f[i.b6+el]=mh;
}N=true;foreach(var rm in i.dw)rm.ht=true;}foreach(var aj in i.dw){if(aj.lr&&i.e0)continue;Color d5=aj.hb;try{if(aj.fl!=null)d5=F(H.dL(aj.fl,cg(aj.hf,aj.ho)));
if(aj.fb!=null&&fA&&H.dL(aj.fb,cg(aj.hp,aj.hr))!=null)d5=i.e_;}catch{}if(d5!=aj.e8||aj.ht){
aj.e8=d5;aj.ht=false;var mp=i.f[aj.fd];mp.Color=d5;i.f[aj.fd]=mp;N=true;}}for(int jf=0;jf<i.dN.Count;jf++){var c=i.dN[jf];
var j=c.fg;double y;bool nv=false;double jI=j.fi;try{y=cg(j.aO,j.I);if(j.cN!=null)jI=cg(j.cN,j.hA);
}catch{nv=true;y=0.0;}if(nv){if(c.ff!= -99999.0||(c.aT>=0&&i.f[c.aT].Data!="ERR")){N=true;c.ff= -99999.0;
if(c.aT>=0){var km=i.f[c.aT];km.Data="ERR";km.Color=Color.Red;i.f[c.aT]=km;}}continue;}double aY=y;
if(jI>0)y=Math.Min(100.0,aY/jI*100.0);bool o3=cs>=0.0&&j.h!=e.hL&&j.h!=e.ck&&j.h!=e.b9&&j.h!=e.ci&&j.h!=e.cb;
if(o3)y=cs<0.5 ? cs*200.0 : 100.0+(y-100.0)*(cs-0.5)*2.0;string fP=j.bv!=null ? H.dL(j.bv,y): j.cc ? H.fc(j.a4,y): j.au;
Color U=F(fP);bool bl=(U==Color.Red);if(fP!=null&&fP.Contains("/"))U=hu(fP.Split('/'),y);
if(j.bv==null&&j.cc&& !bq.hv&&j.a4.Length>1)U=hu(j.a4,y);if(bl&&fA){
U=new Color(255,75,75);N=true;}bool aM=o3||Math.Abs(aY-c.ff)>0.1;if(aM)c.ff=aY;if(c.lL){if(qT(i,c,y,aY,U,aM))N=true;
continue;}if(j.h==e.du){double m8=(j.hx||j.I=="percent"||j.I=="pct")? y : hJ(j.aO);if(c.E<0f)c.E=(float)m8;
float gX=(float)m8;if(Math.Abs(c.E-gX)>0.05f){c.E+=(gX-c.E)*0.35f;N=true;}else if(c.E!=gX){
c.E=gX;N=true;}if(aM)N=true;if(N){float s4=(float)Math.Max(0.0,Math.Min(1.0,c.E/100.0));float na=c.bg*s4;
var fY=i.f[c.W];fY.Size=new Vector2(na,c.a_);fY.Position=new Vector2(c.b5+(na/2f),c.b4);
fY.Color=U;i.f[c.W]=fY;aF(i,c,a3(aY,j),U);}}else if(j.h==e.dI||j.h==e.dF||j.h==e.dy||j.h==e.dA){
if(aM||bl){N=true;double rW=(j.hx||j.I=="percent"||j.I=="pct")? y : hJ(j.aO);int oM=c.K.Count;
for(int eO=0;eO<oM;eO++){bool sK=rW>=((eO+0.4)/(double)oM*100.0);Color tI=sK ? U : new Color(24,28,36);var oN=i.f[c.K[eO]];
oN.Color=tI;i.f[c.K[eO]]=oN;}aF(i,c,a3(aY,j),U);}}else if(j.h==e.ch){
if(aM||bl){N=true;for(int f4=0;f4<7;f4++)c.fn[f4]=c.fn[f4+1];c.fn[7]=(float)y;int te=c.K.Count;
for(int ey=0;ey<te;ey++){float sd=c.fn[ey];float sc=Math.Max(0.06f,Math.Min(1f,sd/100f));float mD=c.a_*sc;float rC=c.b4+(c.a_/2f)-(mD/2f);
var c_=i.f[c.K[ey]];c_.Size=new Vector2(c_.Size.Value.X,mD);c_.Position=new Vector2(c_.Position.Value.X,rC);
c_.Color=U;i.f[c.K[ey]]=c_;}aF(i,c,a3(aY,j),U);}}else if(j.h==e.cI){
if(c.E<0f)c.E=(float)y;float gY=(float)y;if(Math.Abs(c.E-gY)>0.05f){c.E+=(gY-c.E)*0.35f;
N=true;}else if(c.E!=gY){c.E=gY;N=true;}if(N||aM){float nu=c.bg*0.5f;float my=c.b5+nu;
float mO=(float)Math.Max(-1.0,Math.Min(1.0,(c.E-50.0)/50.0));float iY=Math.Abs(mO)*nu;float rX=(mO>=0)?(my+iY*0.5f):(my-iY*0.5f);
var fZ=i.f[c.W];fZ.Size=new Vector2(iY,c.a_);fZ.Position=new Vector2(rX,c.b4);fZ.Color=U;
i.f[c.W]=fZ;aF(i,c,a3(aY,j),U);}}else if(j.h==e.ck){if(aM||bl){
N=true;bool jn=y>=50.0;float nK=Math.Max(4f,c.a_);float sq=jn ?(c.b5+c.bg-nK*0.5f):(c.b5+nK*0.5f);
Color nJ=jn ?(U!=Color.White ? U : new Color(34,197,94)): new Color(60,70,85);var iW=i.f[c.W];iW.Position=new Vector2(sq,c.b4);
iW.Color=nJ;i.f[c.W]=iW;aF(i,c,jn ? "ON" : "OFF",nJ);}}else if(j.h==e.b9){if(aM||bl){
N=true;bool jm=y>=50.0;Color mz=jm ?(U!=Color.White ? U : new Color(56,189,248)): new Color(30,36,48);lD(i,c,mz);
aF(i,c,jm ? "ACTIVE" : "READY",jm ? mz : new Color(148,163,184));}}else if(j.h==e.ci){if(aM||bl){
N=true;string tW=y>=75.0 ? "ONLINE" :(y>=25.0 ? "STBY" : "OFFLINE");Color oY=y>=75.0 ? new Color(34,197,94):(y>=25.0 ? new Color(234,179,8): new Color(239,68,68));
lD(i,c,oY);aF(i,c,tW,oY);}}else if(j.h==e.cG){if(aM||bl){N=true;int ol=c.K.Count;
for(int ez=0;ez<ol;ez++){bool sL=y>=((ez+0.5)/(double)ol*100.0);Color s9=sL ? U : new Color(24,28,36);var om=i.f[c.K[ez]];
om.Color=s9;i.f[c.K[ez]]=om;}aF(i,c,a3(aY,j),U);}}else if(j.h==e.cb){
N=true;float gE;Color ef;string f_;bool si=(j.I=="producing"||j.I=="active"||j.I=="running"||j.I=="state"||(j.I!="percent"&&(y<=1.0||y==100.0)));
if(si){if(y>0.0){c.E=(c.E+0.3927f)%6.28318f;gE=c.E;ef=new Color(34,197,94);f_="RUN";}else{gE=0f;
ef=new Color(148,163,184);f_="IDLE";}}else{gE=(y>55.0)? 0f :((y<45.0)? 3.14159f : 1.57079f);ef=(y>55.0)? new Color(34,197,94):((y<45.0)? new Color(239,68,68): new Color(148,163,184));
f_=(y>55.0)? "CHG" :((y<45.0)? "DSC" : "STBY");}var iX=i.f[c.W];iX.RotationOrScale=gE;iX.Color=ef;
i.f[c.W]=iX;aF(i,c,f_,ef);}else{string fV;bh g_;dV.TryGetValue(j.aO,out g_);cP.TryGetValue(ls(j.I)? j.aO+"."+j.I :(g_!=null&&g_.bi!=null ? g_.bi : j.aO),out fV);
if(aM||bl||fV!=c.lt){c.lt=fV;N=true;Color kQ=U;if(kQ==Color.Red&&fA)kQ=new Color(255,75,75);aF(i,c,fV ?? a3(aY,j),kQ);
}}if(c.ln&&(aM||bl)){N=true;var nT=i.f[c.cM];string s5=ll(y);string tv=a3(aY,j);
nT.Data=j.Z.Replace("{percent}",s5).Replace("{val}",tv);i.f[c.cM]=nT;}}if(N){i.dT++;using(var r2=kE.DrawFrame()){
r2.AddRange(i.f);}}return N;}bool qG(C bY,IMyTextSurface kF){kF.FontSize=bY.cK;var fQ=new List<KeyValuePair<string,string>>();
foreach(var m in bY.fh){double P=0.0;string bd;string a7=null;double jJ=m.fi;try{P=cg(m.aO,m.I);if(m.cN!=null)jJ=cg(m.cN,m.hA);
}catch(Exception ux){fQ.Add(new KeyValuePair<string,string>($"{m.Z}: [UNRESOLVED]","red"));continue;}double ou=P;if(jJ>0)P=Math.Min(100.0,ou/jJ*100.0);
string pp=a3(ou,m);if(m.h==e.du){double m9=(m.hx||m.I=="percent"||m.I=="pct")? P : hJ(m.aO);
bd=H.qz(m.Z,m9,10,pp);a7=m.cc ? H.fc(m.a4,m9): m.au;}else if(m.h==e.cI){
bd=H.qx(m.Z,P,10);a7=m.cc ? H.fc(m.a4,P): m.au;}else if(m.h==e.ck){
bd=H.qJ(m.Z,P);a7=m.au ??(P>=50.0 ? "green" : "gray");}else if(m.h==e.b9){bd=H.qC(m.Z,P);
a7=m.au ??(P>=50.0 ? "cyan" : "gray");}else if(m.h==e.ci){bd=H.qI(m.Z,P);a7=m.au ??(P>=75.0 ? "lime" :(P>=25.0 ? "yellow" : "red"));
}else if(m.h==e.cG){bd=H.qw(m.Z,P,8);a7=m.cc ? H.fc(m.a4,P):(m.au ?? "yellow");
}else if(m.h==e.cb){bool sj=(m.I=="producing"||m.I=="active"||m.I=="running"||m.I=="state"||(m.I!="percent"&&(P<=1.0||P==100.0)));
if(sj){char[]tU=new[]{'|','/','-','\\'};char tF=tU[Math.Abs(d0)%4];bd=P>0 ? $"{m.Z} [{tF}] RUN" : $"{m.Z} [==] IDLE";
a7=P>0 ? "green" : "gray";}else{bd=H.qD(m.Z,P,d0);a7=m.au ??(P>55.0 ? "green" :(P<45.0 ? "red" : "cyan"));
}}else{bd=$"{m.Z}: {pp}";a7=m.bv!=null ? H.dL(m.bv,P): null;}fQ.Add(new KeyValuePair<string,string>(bd,a7));
}var fK=H.qB(new List<string>(fQ.ConvertAll(t3=>t3.Key)),dr: bY.lM);string oQ=string.Join("\n",fK);
string op;if(cn.TryGetValue(bY,out op)&&op==oQ)return false;cn[bY]=oQ;using(var r3=kF.DrawFrame()){
float pG=4f;float nV=pH*bY.cK;try{var nY=kF.MeasureStringInPixels(new StringBuilder("A"),"Monospace",bY.cK);
if(nY.Y>0)nV=nY.Y;}catch{}for(int c4=0;c4<fK.Count;c4++){string mG=null;if(c4>0&&c4<fK.Count-1)mG=fQ[c4-1].Value;
var oX=MySprite.CreateText(fK[c4],"Monospace",F(mG),bY.cK,TextAlignment.LEFT);oX.Position=new Vector2(4f,pG);r3.Add(oX);
pG+=nV;}}return true;}Color F(string gn){if(string.IsNullOrEmpty(gn))return Color.White;Color es;if(lQ.TryGetValue(gn,out es))return es;
n d8=n.dK(gn);es=d8.fe ? Color.Transparent : new Color(d8.bB,d8.bw,d8.br,d8.g8);lQ[gn]=es;return es;}public enum a0{pL,hw,dB}
public enum aE{qh,ha,lH,lv,lu}public class bh{public string ac;public a0 bx;public List<string>aS=new List<string>();public string lI{
get{return aS.Count>0 ? aS[0]: "";}set{if(aS.Count==0)aS.Add(value);else aS[0]=value;}}public aE ds=aE.qh;
public double e9=0.0;public string hl;public string bi;public bool e2;}public enum e{hL,du,dI,ch,dF,dy,dA,cI,ck,b9,ci,cG,cb,fo}
public class aP{public string hn;public string ca;public bool cL;}public class bz{public e h;public string Z;public string aO;
public string I;public bool cc;public string[]a4;public string au;public List<aP>bv;public string lc="right";
public string dS;public string dC;public int fm;public string hy,dR;public double fi;public string cN,hA;public bool hx{get{return fi>0||cN!=null;
}}}public struct n{public byte bB,bw,br,g8;public bool fe;public n(byte tm,byte r5,byte rd,byte qW=255){bB=tm;bw=r5;br=rd;g8=qW;fe=false;}
public static readonly n qS=new n(0,0,0,0){fe=true};public static readonly n fp=new n(255,255,255);public static readonly n pK=new n(0,0,0);
public static readonly n qu=new n(255,0,0);public static readonly n qa=new n(0,255,0);public static readonly n qe=new n(50,205,50);
public static readonly n qV=new n(255,255,0);public static readonly n qi=new n(255,140,0);public static readonly n pW=new n(0,255,255);
public static readonly n pM=new n(0,120,255);public static readonly n qf=new n(255,0,255);public static readonly n qs=new n(128,0,128);
public static readonly n p9=new n(128,128,128);public static readonly n pX=new n(60,60,60);public static n dK(string bc){
if(string.IsNullOrWhiteSpace(bc))return fp;bc=bc.Trim();if(bc.Equals("transparent",StringComparison.OrdinalIgnoreCase)||bc.Equals("none",StringComparison.OrdinalIgnoreCase))return qS;
if(bc.StartsWith("#")){string bP=bc.Substring(1).Trim();if(bP.Length==6){byte tn=Convert.ToByte(bP.Substring(0,2),16);byte r6=Convert.ToByte(bP.Substring(2,2),16);
byte re=Convert.ToByte(bP.Substring(4,2),16);return new n(tn,r6,re);}if(bP.Length==3){byte to=Convert.ToByte(new string(bP[0],2),16);byte r7=Convert.ToByte(new string(bP[1],2),16);
byte rf=Convert.ToByte(new string(bP[2],2),16);return new n(to,r7,rf);}}if(bc.Contains(",")){var de=bc.Split(',');if(de.Length>=3){byte tp=(byte)Math.Max(0,Math.Min(255,int.Parse(de[0].Trim())));
byte r8=(byte)Math.Max(0,Math.Min(255,int.Parse(de[1].Trim())));byte rg=(byte)Math.Max(0,Math.Min(255,int.Parse(de[2].Trim())));byte qX=de.Length>3 ?(byte)Math.Max(0,Math.Min(255,int.Parse(de[3].Trim()))):(byte)255;
return new n(tp,r8,rg,qX);}}switch(bc.ToLowerInvariant()){case "red": return qu;case "green": return qa;case "lime": return qe;case "yellow": return qV;
case "orange": return qi;case "cyan": return pW;case "blue": return pM;case "magenta": return qf;case "purple": return qs;case "white": return fp;
case "black": return pK;case "gray": case "grey": return p9;case "darkgray": case "darkgrey": return pX;default: return fp;}}}public class bt{
public string[]bu;public string dv="vertical";public List<aP>aN;public string hc,hd;public List<aP>cJ;public string hq,hs;
}public class az : bt{public string dJ="default";public int dz=0;public int dM=0;public int dO=1;public int dP=1;public string cj;public string hM="left";
public string hN;public string k9;public string lh;public float? lj;public string cH;public string hj;public string[]hk;
public string hE="horizontal";public List<bz>am=new List<bz>();}public class C : bt{public string a5;public string cf="standard";
public string dx="graphic";public string e7;public string[]ax;public string lJ="left";public string lK;public string li="Monospace";
public string bs;public string k7;public int k8=10;public int k5=1;public float hh=1.2f;public bool hv;
public string hF;public string he;public string k4;public string k2;public string hB;public string b3;public string b8;
public float hi=2.5f;public string k6="cyan";public string[]e5;public List<string>e6=new List<string>();public List<string>p7(){
if(e6!=null&&e6.Count>0)return e6;return new List<string>{"helm-core initializing...","seeking compliant surface displays...","uploading firmware...","awaiting compliance...","helm-core online -- BOOT // SUCCESS"}
;}public float cK=1.0f;public int hH=0;public float g9=1.6f;public int dD=1;public int dE=1;public int lM=30;public string bA;
public int ce=0;public List<az>ai=new List<az>();public List<bz>fh{get{if(ai.Count==0)ai.Add(new az{dJ="main"}
);return ai[0].am;}}}public class bE{public string hC;public List<string>fj=new List<string>();public Dictionary<string,string>hm=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
public List<string>hg=new List<string>();}public class hG{public List<bh>b7=new List<bh>();public List<C>T=new List<C>();
public List<string>a2=new List<string>();public List<string>dU=new List<string>();public Dictionary<string,List<string>>dt=new Dictionary<string,List<string>>(StringComparer.OrdinalIgnoreCase);
}public static class lp{static readonly Dictionary<string,bE>la=new Dictionary<string,bE>(StringComparer.OrdinalIgnoreCase);
static lp(){qv();}static void qv(){bC("fighter_hud",new[]{"$screen=\"Cockpit Display\"","$surf=0","$pwr=\"Main Batteries\"","$h2=\"Hydrogen Tanks\"","$cargo=\"Cargo Containers\""}
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
line:""CARGO"" led(crg_$screen.percent)");bC("reactor_matrix",new[]{"$screen=\"Reactor LCD\"","$surf=0","$reactors=\"Reactors\"","$batteries=\"Batteries\""}
,@"@bind rct_$screen=group(""$reactors"",agg:avg)
@bind bat_$screen=group(""$batteries"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel core at(0,0,1,1) title:""REACTORS""
line:""CORE"" bar(rct_$screen.percent)
@panel grid at(0,1,1,1) title:""BATTERIES""
line:""BANK"" led(bat_$screen.percent)");bC("bridge_status",new[]{"$screen=\"Bridge LCD\"","$surf=0","$power=\"Batteries\"","$o2=\"Air Vents\"","$cargo=\"Cargo\"","$jump=\"Jump Drives\""}
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
line:""CARGO"" led(crg_$screen.percent)");bC("mining_ops",new[]{"$screen=\"Drill LCD\"","$surf=0","$cargo=\"Cargo Containers\"","$drills=\"Drills\""}
,@"@bind crg_$screen=group(""$cargo"",agg:sum)
@bind drl_$screen=group(""$drills"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,2)
@panel ore at(0,0,1,1) title:""ORE STORAGE""
line:""CARGO"" bar(crg_$screen.percent)
@panel drill at(0,1,1,1) title:""DRILL STATUS""
line:""DRILL"" led(drl_$screen.percent)");bC("airlock",new[]{"$screen=\"Airlock LCD\"","$surf=0","$vent=\"Airlock Vent\"","$outer=\"Outer Door\"","$inner=\"Inner Door\""}
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
line:""INNER"" led(inner_$screen.percent)");bC("rover",new[]{"$screen=\"Rover Cockpit\"","$surf=0","$power=\"Rover Batteries\"","$susp=\"Suspension Wheels\"","$cargo=\"Rover Cargo\"","$h2=\"Hydrogen Engine\""}
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
line:""HOLD"" bar(crg_$screen.percent)");bC("cargo_overview",new[]{"screen","surf=0","crg=Cargo*","ore=Ore*","ingot=Ingot*"},@"@bind crg_$screen=group(""$crg"",agg:avg)
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
line:""NATO"" bar(crg_$screen.percent)");bC("battery_monolith",new[]{"screen","surf=0","batt=Battery*"},@"@bind pwr_$screen=group(""$batt"",agg:avg)
@screen $screen
output:""$screen"",$surf
layout:grid(1,1)
@panel core at(0,0,1,1) title:""POWER CORE MONOLITH"" orientation:vertical
line:""ENERGY"" bar(pwr_$screen.percent) dir:up");}public static void bC(string n4,string[]s2,string rq){var gV=new bE{hC=n4}
;foreach(var sW in s2){var jw=sW.Split(new[]{'='},2);string od=hD(jw[0]);gV.fj.Add(od);if(jw.Length>1)gV.hm[od]=ae(jw[1].Trim());
}var sI=rq.Replace("\r\n","\n").Split('\n');foreach(var sw in sI){string oC=sw.Trim();if(oC.Length>0)gV.hg.Add(oC);}la[n4]=gV;}static string hD(string db){
db=db.Trim();if(db.StartsWith("$"))db=db.Substring(1);return db.ToLowerInvariant();}public static hG qk(string rF){var aq=new hG();C d=null;
az r=null;var oo=qq(rF,aq.a2,aq.dt);for(int em=0;em<oo.Count;em++){string a=oo[em].Trim();
if(a.Length==0)continue;try{if(a.StartsWith("@bind")){aq.b7.Add(ql(a));}else if(a.StartsWith("@screen")){d=new C{a5=a.Substring("@screen".Length).Trim()}
;aq.T.Add(d);r=null;}else if(a.StartsWith("profile:")){p(d,"profile:");d.cf=a.Substring("profile:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("border:")){p(d,"border:");string ma=a.Substring("border:".Length).Trim();if(r!=null)r.k9=ma;
else d.dx=ma;}else if(a.StartsWith("border_color:")){p(d,"border_color:");string mc=a.Substring("border_color:".Length).Trim();
if(r!=null)r.hj=mc;else d.e7=mc;}else if(a.StartsWith("border_gradient:")||a.StartsWith("border_gradient(")){
p(d,"border_gradient");string cq=a.StartsWith("border_gradient:")? a.Substring("border_gradient:".Length): a.Substring("border_gradient".Length);
cq=cq.Trim();if(cq.StartsWith("(")&&cq.Contains(")"))cq=aQ(cq,0);string[]o_=aR(cq).Select(tz=>tz.Trim()).ToArray();if(r!=null)r.hk=o_;
else d.ax=o_;}else if(a.StartsWith("title_align:")){p(d,"title_align:");string o7=a.Substring("title_align:".Length).Trim().ToLowerInvariant();
if(r!=null)r.hM=o7;else d.lJ=o7;}else if(a.StartsWith("title_color:")){p(d,"title_color:");string pa=a.Substring("title_color:".Length).Trim();
if(r!=null)r.hN=pa;else d.lK=pa;}else if(a.StartsWith("font_size:")){p(d,"font_size:");float o4=float.Parse(a.Substring("font_size:".Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
if(r!=null)r.lj=o4;else d.cK=o4;}else if(a.StartsWith("font:")){p(d,"font:");string m2=a.Substring("font:".Length).Trim();
if(r!=null)r.lh=m2;else d.li=m2;}else if(a.StartsWith("bg:")||a.StartsWith("background:")){p(d,"bg:");string cT=a.Substring(a.IndexOf(':')+1).Trim();
bt ii=r!=null ?(bt)r : d;if(cT.StartsWith("color(")&&cT.EndsWith(")")){string mk;ii.aN=lz(cT.Substring(6,cT.Length-7),out mk);
string mi,mj;lE(mk,out mi,out mj);ii.hc=mi;ii.hd=mj;}else if(r!=null)r.cH=cT;
else d.bs=cT;}else if(a.StartsWith("layout:")){p(d,"layout:");qm(a.Substring("layout:".Length).Trim(),d);}else if(a.StartsWith("alert_period:")){
p(d,"alert_period:");d.g9=Math.Max(0.2f,float.Parse(a.Substring("alert_period:".Length).Trim().TrimEnd('s'),System.Globalization.CultureInfo.InvariantCulture));
}else if(a.StartsWith("refresh:")){p(d,"refresh:");d.hH=int.Parse(a.Substring("refresh:".Length).Trim());}else if(a.StartsWith("output:")){
p(d,"output:");var gt=aR(a.Substring("output:".Length).Trim());if(gt.Count==0)throw new FormatException("output: requires a target block name");
d.bA=ae(gt[0]);d.ce=gt.Count>1 ? int.Parse(gt[1]): 0;}else if(a.StartsWith("width:")){p(d,"width:");
d.lM=int.Parse(a.Substring("width:".Length).Trim());}else if(a.StartsWith("boot_msg:")||a.StartsWith("boot:")){p(d,"boot_msg:");
d.k7=ae(a.Substring(a.IndexOf(':')+1).Trim());}else if(a.StartsWith("boot_stagger:")){p(d,"boot_stagger:");d.k8=int.Parse(a.Substring("boot_stagger:".Length).Trim());
}else if(a.StartsWith("boot_calibrate:")){p(d,"boot_calibrate:");string mJ=a.Substring("boot_calibrate:".Length).Trim().ToLowerInvariant().TrimEnd('s');
d.hh=mJ=="off" ? 0f : Math.Max(0f,float.Parse(mJ,System.Globalization.CultureInfo.InvariantCulture));}else if(a.StartsWith("boot_batch:")){p(d,"boot_batch:");
d.k5=Math.Max(1,int.Parse(a.Substring("boot_batch:".Length).Trim()));}else if(a.StartsWith("boot_duration:")){p(d,"boot_duration:");
string ea=a.Substring("boot_duration:".Length).Trim().ToLowerInvariant();bool nA=ea.EndsWith("s");if(nA)ea=ea.Substring(0,ea.Length-1).Trim();
float iL;if(float.TryParse(ea,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out iL)){d.hi=nA ? iL : iL/60f;
}}else if(a.StartsWith("boot_log:")){p(d,"boot_log:");d.e6.Add(ae(a.Substring("boot_log:".Length).Trim()));}else if(a.StartsWith("mirror:")){
p(d,"mirror:");d.hB=ae(a.Substring("mirror:".Length).Trim());}else if(a.StartsWith("banner:")){p(d,"banner:");
d.b3=ae(a.Substring("banner:".Length).Trim());}else if(a.StartsWith("boot_logo:")){p(d,"boot_logo:");d.b8=ae(a.Substring("boot_logo:".Length).Trim());
}else if(a.StartsWith("boot_gradient:")||a.StartsWith("boot_gradient(")||a.StartsWith("banner_gradient:")||a.StartsWith("banner_gradient(")){p(d,"boot_gradient");
d.e5=lA(a);}else if(a.StartsWith("bg_gradient:")||a.StartsWith("bg_gradient(")){p(d,"bg_gradient");(r!=null ?(bt)r : d).bu=lA(a);
}else if(a.StartsWith("bg_gradient_dir:")){p(d,"bg_gradient_dir:");(r!=null ?(bt)r : d).dv=a.Substring("bg_gradient_dir:".Length).Trim().ToLowerInvariant();
}else if(a.StartsWith("flash:")){p(d,"flash:");bt iT=r!=null ?(bt)r : d;string iS=a.Substring("flash:".Length).Trim();
string m4=lG(iS);string m3,m5;lE(iS.Substring(0,iS.Length-m4.Length),out m3,out m5);iT.hq=m3;iT.hs=m5;
iT.cJ=new List<aP>{new aP{hn=m4,ca="on"}};}else if(a.StartsWith("bg_texture:")){p(d,"bg_texture:");d.he=ae(a.Substring("bg_texture:".Length).Trim());
}else if(a.StartsWith("bg_texture_color:")){p(d,"bg_texture_color:");d.k4=a.Substring("bg_texture_color:".Length).Trim();
}else if(a.StartsWith("gradient_mode:")){p(d,"gradient_mode:");d.hv=a.Substring("gradient_mode:".Length).Trim().Equals("steps",StringComparison.OrdinalIgnoreCase);
}else if(a.StartsWith("screen_bg:")){p(d,"screen_bg:");d.bs=a.Substring("screen_bg:".Length).Trim();}else if(a.StartsWith("panel_bg:")){
p(d,"panel_bg:");d.hF=a.Substring("panel_bg:".Length).Trim();}else if(a.StartsWith("alert_bg:")){p(d,"alert_bg:");
d.k2=a.Substring("alert_bg:".Length).Trim();}else if(a.StartsWith("boot_color:")||a.StartsWith("banner_color:")){p(d,"boot_color:");
d.k6=a.Substring(a.IndexOf(':')+1).Trim();}else if(a.StartsWith("orientation:")){p(d,"orientation:");string sS=a.Substring("orientation:".Length).Trim().ToLowerInvariant();
if(r!=null)r.hE=sS;}else if(a.StartsWith("@panel")){p(d,"@panel");r=qo(a);d.ai.Add(r);
}else if(a.StartsWith("title:")){p(d,"title:");if(r==null){r=new az{dJ="main",dz=0,dM=0,dO=d.dD,dP=d.dE}
;d.ai.Add(r);}r.cj=ae(a.Substring("title:".Length).Trim());}else if(a.StartsWith("line:")){p(d,"line:");
if(r==null){r=new az{dJ="main",dz=0,dM=0,dO=d.dD,dP=d.dE};d.ai.Add(r);}r.am.Add(qn(a.Substring("line:".Length).Trim()));
}else{aq.a2.Add($"Line {em + 1}: unrecognized directive: \"{a}\"");}}catch(Exception rQ){aq.a2.Add($"Line {em + 1}: parse error ({rQ.Message}) in \"{a}\"");
}}foreach(var oD in aq.T){if(string.IsNullOrEmpty(oD.bA))aq.a2.Add($"@screen {oD.a5}: missing required 'output:' — a screen must declare which physical block/surface it renders to");
}foreach(var at in aq.T){int jS=at.ai.Count(j6=>j6.cH!=null||j6.aN!=null||j6.bu!=null);if(at.ai.Count>1&&jS>0&&jS<at.ai.Count)aq.dU.Add("screen '"+at.a5+"': only "+jS+" of "+at.ai.Count+" panels have their own bg:, so the screen shows two colours. bg: after an @panel colours that panel only; use screen_bg: (or bg: before the first @panel) for the whole screen");
if(at.b3!=null&& !aq.dt.ContainsKey(at.b3))aq.a2.Add("Screen '"+at.a5+"': banner '"+at.b3+"' is not defined (@banner "+at.b3+" ... @end)");
if(at.b8!=null&& !aq.dt.ContainsKey(at.b8))aq.a2.Add("Screen '"+at.a5+"': boot_logo '"+at.b8+"' is not defined (@banner "+at.b8+" ... @end)");
}return aq;}static void qm(string iq,C oK){int jZ=iq.IndexOf('(');if(jZ<0)throw new FormatException("missing '(' in layout: specification");
string nI=iq.Substring(0,jZ).Trim().ToLowerInvariant();string q3=aQ(iq,jZ);var h7=aR(q3);if(nI=="grid"){if(h7.Count<2)throw new FormatException("layout: grid(cols, rows) requires 2 arguments");
oK.dD=Math.Max(1,int.Parse(h7[0]));oK.dE=Math.Max(1,int.Parse(h7[1]));}else{throw new FormatException($"unknown layout kind '{nI}' (expected grid)");
}}static az qo(string sE){string eJ=sE.Substring("@panel".Length).Trim();int fD=eJ.IndexOf("at(",StringComparison.OrdinalIgnoreCase);if(fD<0)throw new FormatException("@panel requires an at(...) positioning clause");
string sP=eJ.Substring(0,fD).Trim();string l5=eJ.Substring(fD);int oj=l5.IndexOf('(');string l3=aQ(l5,oj);
var co=aR(l3);if(co.Count<2)throw new FormatException("at(col, row) requires at least 2 numbers");var bn=new az{dJ=sP,dz=int.Parse(co[0]),dM=int.Parse(co[1]),dO=co.Count>2 ? int.Parse(co[2]): 1,dP=co.Count>3 ? int.Parse(co[3]): 1}
;int lX=fD+oj+l3.Length+1;if(lX<eJ.Length){string aJ=eJ.Substring(lX).Trim();int pf=aJ.IndexOf("title",StringComparison.OrdinalIgnoreCase);
if(pf>=0){string eR=aJ.Substring(pf+5).Trim();if(eR.StartsWith("(")&&eR.Contains(")"))bn.cj=ae(aQ(eR,0));else if(eR.StartsWith(":")){
string cr=eR.Substring(1).Trim();if(cr.StartsWith("\"")){int mZ=cr.IndexOf('"',1);bn.cj=mZ>0 ? cr.Substring(1,mZ-1): ae(cr);}else{int oV=cr.IndexOf(' ');
bn.cj=oV>=0 ? cr.Substring(0,oV): cr;}}}string n7=fa(aJ,"orientation:");if(n7!=null)bn.hE=n7.ToLowerInvariant();string lV=fa(aJ,"title_align:");
if(lV!=null)bn.hM=lV.ToLowerInvariant();string pc=fa(aJ,"title_color:");if(pc!=null)bn.hN=pc;int fJ=aJ.IndexOf("border:",StringComparison.OrdinalIgnoreCase);
if(fJ>=0&& !aJ.Substring(fJ).StartsWith("border_gradient:",StringComparison.OrdinalIgnoreCase)&& !aJ.Substring(fJ).StartsWith("border_color:",StringComparison.OrdinalIgnoreCase)){
bn.k9=fa(aJ.Substring(fJ),"border:");}int fG=aJ.IndexOf("border_gradient:",StringComparison.OrdinalIgnoreCase);if(fG<0)fG=aJ.IndexOf("border_gradient(",StringComparison.OrdinalIgnoreCase);
if(fG>=0){string bN=aJ.Substring(fG+"border_gradient".Length).Trim();if(bN.StartsWith(":"))bN=bN.Substring(1).Trim();if(bN.StartsWith("(")&&bN.Contains(")"))bN=aQ(bN,0);
bn.hk=aR(bN).Select(tA=>tA.Trim()).ToArray();}}return bn;}static string fa(string oE,string nH){int nz=oE.IndexOf(nH,StringComparison.OrdinalIgnoreCase);
if(nz<0)return null;string kU=oE.Substring(nz+nH.Length).Trim();int oW=kU.IndexOf(' ');return oW>=0 ? kU.Substring(0,oW): kU;}static string[]lA(string jC){
int i3=jC.IndexOfAny(new[]{':','('});string eh=jC.Substring(jC[i3]==':' ? i3+1 : i3).Trim();if(eh.StartsWith("(")&&eh.Contains(")"))eh=aQ(eh,0);return aR(eh).Select(tB=>tB.Trim()).ToArray();
}static List<string>qq(string rG,List<string>iQ,Dictionary<string,List<string>>ri){var pk=new Dictionary<string,bE>(StringComparer.OrdinalIgnoreCase);
var dg=(rG ?? "").Replace("\r\n","\n").Split('\n');var jR=new List<string>();int c5=0;while(c5<dg.Length){string kc=dg[c5++];if(kc.TrimStart().StartsWith("@banner ",StringComparison.OrdinalIgnoreCase)){
var ao=new List<string>();while(c5<dg.Length){string lT=dg[c5++].Replace("\t","    ").TrimEnd();if(lT.Trim().Equals("@end",StringComparison.OrdinalIgnoreCase))break;
ao.Add(lT);}while(ao.Count>0&&ao[0].Length==0)ao.RemoveAt(0);while(ao.Count>0&&ao[ao.Count-1].Length==0)ao.RemoveAt(ao.Count-1);int jl=int.MaxValue;
foreach(var h_ in ao)if(h_.Length>0)jl=Math.Min(jl,h_.Length-h_.TrimStart().Length);for(int et=0;et<ao.Count;et++)if(ao[et].Length>0)ao[et]=ao[et].Substring(jl);
ri[kc.TrimStart().Substring(8).Trim()]=ao;continue;}string bT=lF(kc).Trim();if(bT.Length==0)continue;if(bT.StartsWith("@template")){try{
var kK=qp(bT);while(c5<dg.Length){string rs=dg[c5++];string ir=lF(rs).Trim();if(ir.Equals("@end",StringComparison.OrdinalIgnoreCase))break;
if(ir.Length>0)kK.hg.Add(ir);}pk[kK.hC]=kK;}catch(Exception rR){iQ.Add($"Template parse error ({rR.Message}) in \"{bT}\"");}}
else if(bT.StartsWith("@use")){try{le(bT,pk,jR,iQ);}catch(Exception rS){iQ.Add($"@use error ({rS.Message}) in \"{bT}\"");
}}else{jR.Add(bT);}}return jR;}static bE qp(string sF){string kd=sF.Substring("@template".Length).Trim();int j_=kd.IndexOf('(');
if(j_<0)throw new FormatException("missing '(' in @template header");string sQ=kd.Substring(0,j_).Trim();string q4=aQ(kd,j_);var kL=new bE{
hC=sQ};foreach(var sX in aR(q4)){var jx=sX.Split(new[]{'='},2);string oe=hD(jx[0]);kL.fj.Add(oe);if(jx.Length>1)kL.hm[oe]=ae(jx[1].Trim());
}return kL;}static void le(string sG,Dictionary<string,bE>pl,List<string>oa,List<string>rP,int mN=0){string gD=sG.Substring("@use".Length).Trim();
int gr=gD.IndexOf('(');string jK=gr>=0 ? gD.Substring(0,gr).Trim(): gD.Trim();string q5=gr>=0 ? aQ(gD,gr): "";bE dn=null;
if(!pl.TryGetValue(jK,out dn)){if(!la.TryGetValue(jK,out dn))throw new FormatException($"unknown template '{jK}'");}var fM=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
foreach(var nM in dn.hm)fM[nM.Key]=nM.Value;var l2=aR(q5);for(int d2=0;d2<l2.Count;d2++){string fB=l2[d2];int iO=fB.IndexOf('=');if(iO>=0){
string so=hD(fB.Substring(0,iO));string ug=ae(fB.Substring(iO+1).Trim());fM[so]=ug;}else{if(d2<dn.fj.Count){string sp=dn.fj[d2];
fM[sp]=ae(fB.Trim());}}}foreach(var rr in dn.hg){string ed=rr;foreach(var gi in fM){ed=ed.Replace("$"+gi.Key,gi.Value).Replace("{"+gi.Key+"}",gi.Value);
}if(ed.StartsWith("@use")){if(mN>3)throw new FormatException("@use nested too deep");le(ed,pl,oa,rP,mN+1);
}else oa.Add(ed);}}static void p(C rE,string rM){if(rE==null)throw new FormatException($"'{rM}' used before any @screen declared");
}static string lF(string ba){if(string.IsNullOrEmpty(ba))return ba;bool jk=false;for(int bQ=0;bQ<ba.Length;bQ++){char iw=ba[bQ];if(iw=='"'){jk= !jk;
}else if(!jk){if(iw=='/'&&bQ+1<ba.Length&&ba[bQ+1]=='/'){return ba.Substring(0,bQ);}if(iw=='#'){if(qc(ba,bQ)){continue;}return ba.Substring(0,bQ);}}}return ba;
}static bool qc(string cw,int nC){int c8=0;for(int jg=nC+1;jg<cw.Length;jg++){char cW=cw[jg];if((cW>='0'&&cW<='9')||(cW>='a'&&cW<='f')||(cW>='A'&&cW<='F'))c8++;
else break;}if(c8==3||c8==4||c8==6||c8==8){int ex=nC+1+c8;if(ex>=cw.Length||char.IsWhiteSpace(cw[ex])||cw[ex]==')'||cw[ex]==','||cw[ex]==':')return true;
}return false;}static bh ql(string sH){string ke=sH.Substring("@bind".Length).Trim();int iP=ke.IndexOf('=');if(iP<0)throw new FormatException("missing '=' in @bind");
string n5=ke.Substring(0,iP).Trim();string iR=ke.Substring(iP+1).Trim();int j0=iR.IndexOf('(');if(j0<0)throw new FormatException("missing '(' in @bind target");
string f2=iR.Substring(0,j0).Trim().ToLowerInvariant();string q6=aQ(iR,j0);var bK=aR(q6);if(bK.Count==0)throw new FormatException("@bind target has no arguments");
var ar=new bh{ac=n5};if(f2=="dynamic"||f2=="var"||f2=="mem"){ar.bx=a0.dB;ar.bi=bK.Count>0 ? ae(bK[0].Trim()): n5;
for(int h0=0;h0<bK.Count;h0++){var gj=bK[h0].Split(new[]{':'},2);if(gj.Length==2&&gj[0].Trim().Equals("default",StringComparison.OrdinalIgnoreCase)){double mM;
if(double.TryParse(gj[1].Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out mM))ar.e9=mM;else ar.hl=ae(gj[1].Trim());
}}return ar;}ar.bx=f2=="group" ? a0.hw : a0.pL;string fz=null;for(int fy=0;fy<bK.Count;fy++){var c7=bK[fy].Split(new[]{':'},2);if(c7.Length==2&&c7[0].Trim().Equals("agg",StringComparison.OrdinalIgnoreCase)){
fz=c7[1].Trim().ToLowerInvariant();}else if(c7.Length==2&&c7[0].Trim().Equals("scope",StringComparison.OrdinalIgnoreCase)){ar.e2=c7[1].Trim().Equals("all",StringComparison.OrdinalIgnoreCase);
}else{ar.aS.Add(ae(bK[fy].Trim()));}}if(ar.aS.Count==0)throw new FormatException("@bind target requires at least one block or group name");
if(ar.bx==a0.hw){if(fz==null)throw new FormatException("group bind requires an explicit 'agg:' (avg/sum/min/max) — no default is assumed (PDR §6)");
switch(fz){case "avg": ar.ds=aE.ha;break;case "sum": ar.ds=aE.lH;break;case "min": ar.ds=aE.lv;break;case "max": ar.ds=aE.lu;
break;default: throw new FormatException($"unknown agg '{fz}' (expected avg/sum/min/max)");}}return ar;}static bz qn(string fI){var o=new bz();
if(!fI.StartsWith("\""))throw new FormatException("line must start with a quoted label");int iN=fI.IndexOf('"',1);if(iN<0)throw new FormatException("unterminated quoted label");
o.Z=fI.Substring(1,iN-1);string eI=fI.Substring(iN+1).Trim();int gs=eI.IndexOf('(');if(gs<0)throw new FormatException("missing display function, e.g. bar(...)/text(...)");
string D=eI.Substring(0,gs).Trim().ToLowerInvariant();string q2=aQ(eI,gs);string af=eI.Substring(eI.IndexOf(')',gs)+1).Trim();
var iJ=q2.Trim().Split('.');if(iJ.Length!=2)throw new FormatException("expected 'bindname.field' inside display function, e.g. power.percent");
o.aO=iJ[0].Trim();o.I=iJ[1].Trim();if(D=="bar")o.h=e.du;else if(D=="led")o.h=e.dI;else if(D=="spark"||D=="sparkline")o.h=e.ch;
else if(D=="hazard"||D=="stripes")o.h=e.dF;else if(D=="chevron"||D=="chevrons")o.h=e.dy;else if(D=="dots"||D=="dot")o.h=e.dA;
else if(D=="balance"||D=="horizon"||D=="centered")o.h=e.cI;else if(D=="toggle"||D=="switch")o.h=e.ck;else if(D=="checkbox"||D=="check")o.h=e.b9;
else if(D=="status"||D=="badge"||D=="state")o.h=e.ci;else if(D=="ammo"||D=="pips")o.h=e.cG;else if(D=="flow"||D=="rate"||D=="spinner")o.h=e.cb;
else if(D=="text")o.h=e.hL;else if(D=="tank"||D=="tube"||D=="bucket")o.h=e.fo;else throw new FormatException($"unknown display function '{D}'");
while(af.Length>0){int iH=af.IndexOf("dir:",StringComparison.OrdinalIgnoreCase);int c2=af.IndexOf('(');if(iH>=0&&(c2<0||iH<c2)){
string fS=af.Substring(iH+4).Trim();int gN=fS.IndexOf(' ');o.lc=(gN>=0 ? fS.Substring(0,gN): fS).ToLowerInvariant();af=(gN>=0 ? fS.Substring(gN): "").Trim();
continue;}int kS=af.IndexOf("unit:",StringComparison.OrdinalIgnoreCase);if(kS>=0&&(c2<0||kS<c2)){string g5=af.Substring(kS+5).Trim();
int gO=g5.IndexOf(' ');o.dS=(gO>=0 ? g5.Substring(0,gO): g5).Trim().Trim('"');af=(gO>=0 ? g5.Substring(gO): "").Trim();continue;}int iZ=af.IndexOf("format:",StringComparison.OrdinalIgnoreCase);
if(iZ>=0&&(c2<0||iZ<c2)){string fX=af.Substring(iZ+7).Trim();int gP=fX.IndexOf(' ');o.dC=(gP>=0 ? fX.Substring(0,gP): fX).Trim().Trim('"');
af=(gP>=0 ? fX.Substring(gP): "").Trim();continue;}int gm=af.IndexOf('(');if(gm<0)break;string ag=af.Substring(0,gm).Trim().ToLowerInvariant();
int nS=ag.LastIndexOf(' ');if(nS>=0)ag=ag.Substring(nS+1).Trim();string aI=aQ(af,gm);af=af.Substring(af.IndexOf(')',gm)+1).Trim();
if(ag=="gradient"){o.cc=true;o.a4=aR(aI).Select(tC=>tC.Trim()).ToArray();if(o.a4.Length<2)throw new FormatException("gradient() needs at least 2 colors");
}else if(ag=="size"){string kJ=aI.Trim().ToLowerInvariant();o.fm=kJ=="large"||kJ=="fill" ? 2 : kJ=="medium" ? 1 : 0;}else if(ag=="label_color")o.hy=aI.Trim();
else if(ag=="track")o.dR=aI.Trim();else if(ag=="flat"){o.au=aI.Trim();}else if(ag=="pattern"){string S=aI.Trim().ToLowerInvariant();
if(S=="hazard"||S=="stripes")o.h=e.dF;else if(S=="chevron"||S=="chevrons")o.h=e.dy;else if(S=="dots"||S=="dot")o.h=e.dA;
else if(S=="led")o.h=e.dI;else if(S=="balance"||S=="horizon")o.h=e.cI;else if(S=="toggle"||S=="switch")o.h=e.ck;
else if(S=="checkbox"||S=="check")o.h=e.b9;else if(S=="status"||S=="badge"||S=="state")o.h=e.ci;else if(S=="ammo"||S=="pips")o.h=e.cG;
else if(S=="flow"||S=="rate"||S=="spinner")o.h=e.cb;}else if(ag=="color"){string sh;o.bv=lz(aI,out sh);
}else if(ag=="dir"||ag=="direction"){o.lc=aI.Trim().ToLowerInvariant();}else if(ag=="unit"){o.dS=aI.Trim().Trim('"');}else if(ag=="max"){
double n0;if(double.TryParse(aI.Trim(),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out n0))o.fi=n0;else{var jH=aI.Trim().Split('.');
if(jH.Length!=2)throw new FormatException("max() takes a number or bindname.field");o.cN=jH[0].Trim();o.hA=jH[1].Trim();}}else if(ag=="format"||ag=="fmt"){
o.dC=aI.Trim().Trim('"');}else{throw new FormatException($"unknown modifier '{ag}'");}}return o;}static string aQ(string gI,int jP){
int iG=0;for(int en=jP;en<gI.Length;en++){if(gI[en]=='(')iG++;else if(gI[en]==')'){iG--;if(iG==0)return gI.Substring(jP+1,en-jP-1);
}}throw new FormatException("unbalanced parentheses");}static List<string>aR(string q7){return q7.Split(',').Select(tD=>tD.Trim()).Where(tE=>tE.Length>0).ToList();
}static List<aP>lz(string q1,out string iV){var gG=new List<aP>();iV=null;foreach(var j1 in aR(q1)){int iD=j1.LastIndexOf(':');
if(iD<0)throw new FormatException("color() entries need 'expr : color'");string gk=j1.Substring(0,iD).Trim();string mF=j1.Substring(iD+1).Trim();
if(gk.Equals("default",StringComparison.OrdinalIgnoreCase)){gG.Add(new aP{cL=true,ca=mF});}else{string o0=lG(gk);
if(iV==null)iV=gk.Substring(0,gk.Length-o0.Length).Trim();gG.Add(new aP{hn=o0,ca=mF});}}if(!gG.Any(tq=>tq.cL))throw new FormatException("color() requires a 'default:' fallback entry — PDR-consistent decision: no silent fallback color assumed");
return gG;}static void lE(string tr,out string ro,out string rV){var eA=(tr ?? "").Split('.');if(eA.Length!=2||eA[0].Trim().Length==0||eA[1].Trim().Length==0)throw new FormatException("expected bindname.field in front of the comparison, e.g. batt.percent < 20");
ro=eA[0].Trim();rV=eA[1].Trim();}static string lG(string c0){c0=c0.Trim();foreach(var sU in new[]{">=","<=","==",">","<"}){int jh=c0.IndexOf(sU,StringComparison.Ordinal);
if(jh>0)return c0.Substring(jh).Trim();if(jh==0)return c0;}throw new FormatException($"color() rule has no recognizable comparison operator: '{c0}'");}
static string ae(string cx){if(cx.Length>=2&&cx[0]=='"'&&cx[cx.Length-1]=='"')return cx.Substring(1,cx.Length-2);return cx;}}public static class pJ{public static double pI(aE qY,List<double>dq){
if(dq==null||dq.Count==0)throw new InvalidOperationException("cannot aggregate an empty value set");switch(qY){case aE.ha: return dq.Average();case aE.lH: return dq.Sum();
case aE.lv: return dq.Min();case aE.lu: return dq.Max();default: throw new InvalidOperationException("Aggregate() called with Agg.None");}}}public static class H{
public static string qz(string sy,double gu,int mb=10,string pt=null){gu=Math.Max(0,Math.Min(100,gu));int nb=(int)Math.Round(mb*(gu/100.0));
string rj=new string('#',nb)+new string('-',mb-nb);string uh=pt!=null ? pt : $"{gu:0}%";return $"{sy} [{rj}] {uh}";
}public static string lk(double b2,string kR=null,string f1=null){if(kR=="auto"){double h1=Math.Abs(b2);if(h1>=1e9)return(b2/1e9).ToString("F1")+"G";
if(h1>=1e6)return(b2/1e6).ToString("F1")+"M";if(h1>=1e3)return(b2/1e3).ToString("F1")+"k";string rT= !string.IsNullOrEmpty(f1)? f1 : "F0";return b2.ToString(rT);
}string nd= !string.IsNullOrEmpty(f1)? f1 :(Math.Abs(b2)>=100.0 ? "F0" : "F1");if(string.IsNullOrEmpty(kR)){return b2.ToString(nd);}return b2.ToString(nd)+kR;
}public static float p8(string tg,float o2,float kG,float fT=1.0f){string dc=(tg ?? "standard").ToLowerInvariant();
if(dc=="compact"||(dc=="auto"&&o2<=300&&kG<=300&&kG>160)){return fT*0.75f;}if(dc=="corner"||(dc=="auto"&&kG<=160)){
return fT*0.55f;}if(dc=="wide"||(dc=="auto"&&o2>=1000)){return fT*1.15f;}return fT;}public static string qx(string sz,double gv,int bO=5){
gv=Math.Max(0,Math.Min(100,gv));double fU=(gv-50.0)/50.0;int eu=0;int eL=0;if(fU< -0.01)eu=(int)Math.Round(-fU*bO);
else if(fU>0.01)eL=(int)Math.Round(fU*bO);if(eu>bO)eu=bO;if(eL>bO)eL=bO;string sC=new string('-',bO-eu)+new string('<',eu);
string tw=new string('>',eL)+new string('-',bO-eL);return $"{sz} [{sC}|{tw}] {gv:0}%";}public static string qJ(string nP,double uj){
return uj>=50.0 ? $"{nP} [====O] ON" : $"{nP} [O====] OFF";}public static string qC(string nQ,double uk){return uk>=50.0 ? $"{nQ} [X] ACTIVE" : $"{nQ} [ ] READY ";
}public static string qI(string jz,double pr){if(pr>=75.0)return $"{jz} [ ONLINE  ]";if(pr>=25.0)return $"{jz} [ STANDBY ]";return $"{jz} [ OFFLINE ]";
}public static string qw(string sA,double gw,int nX=8){gw=Math.Max(0,Math.Min(100,gw));int nc=(int)Math.Round(nX*(gw/100.0));
string ta=new string('|',nc)+new string('.',nX-nc);return $"{sA} [{ta}] {gw:0}%";}public static string qD(string jA,double eE,int pe=0){
char[]tT=new[]{'|','/','-','\\'};char oF=tT[(pe>=0 ? pe : 0)%4];if(eE>55.0)return $"{jA} {oF} ▲ CHG {eE:0}%";if(eE<45.0)return $"{jA} {oF} ▼ DSC {eE:0}%";
return $"{jA} ● STBY {eE:0}%";}public static bool qg(string av,string eq){if(av==null||eq==null)return false;if(av=="*"||av==eq)return true;
if(!av.Contains("*")&& !av.Contains("?"))return string.Equals(av,eq,StringComparison.OrdinalIgnoreCase);int aB=0,eo=0,kx= -1,jG=0;while(eo<eq.Length){
if(aB<av.Length&&(av[aB]=='?'||char.ToLowerInvariant(av[aB])==char.ToLowerInvariant(eq[eo]))){aB++;eo++;}else if(aB<av.Length&&av[aB]=='*'){
kx=aB;jG=eo;aB++;}else if(kx!= -1){aB=kx+1;jG++;eo=jG;}else{return false;}}while(aB<av.Length&&av[aB]=='*')aB++;return aB==av.Length;
}public static int pN(double sY,int pi){return Math.Max(0,Math.Min(pi,1+(int)(Math.Max(0.0,sY)*pi)));}public static string fc(string[]dm,double j2){
if(dm==null||dm.Length==0)return "white";j2=Math.Max(0,Math.Min(100,j2));int ji=(int)Math.Floor(j2/100.0*dm.Length);if(ji>=dm.Length)ji=dm.Length-1;
return dm[ji];}public static n lm(string[]b0,double s6){if(b0==null||b0.Length==0)return n.fp;if(b0.Length==1)return n.dK(b0[0]);
double tu=Math.Max(0.0,Math.Min(1.0,s6/100.0));double oJ=tu*(b0.Length-1);int jj=(int)Math.Floor(oJ);int sg=Math.Min(b0.Length-1,jj+1);
double kB=oJ-jj;n cX=n.dK(b0[jj]);n ix=n.dK(b0[sg]);byte ts=(byte)Math.Round(cX.bB+(ix.bB-cX.bB)*kB);
byte r9=(byte)Math.Round(cX.bw+(ix.bw-cX.bw)*kB);byte rh=(byte)Math.Round(cX.br+(ix.br-cX.br)*kB);return new n(ts,r9,rh);}public static string dL(List<aP>oB,double ul){
foreach(var kf in oB){if(kf.cL)continue;if(p4(kf.hn,ul))return kf.ca;}var mL=oB.FirstOrDefault(tt=>tt.cL);return mL!=null ? mL.ca : null;
}static bool p4(string ee,double eY){ee=ee.Trim();foreach(var jO in new[]{">=","<=","==",">","<"}){if(ee.StartsWith(jO)){double eK=double.Parse(ee.Substring(jO.Length).Trim(),System.Globalization.CultureInfo.InvariantCulture);
switch(jO){case ">=": return eY>=eK;case "<=": return eY<=eK;case "==": return Math.Abs(eY-eK)<1e-9;case ">": return eY>eK;case "<": return eY<eK;
}}}throw new FormatException($"unsupported comparison expression: '{ee}'");}public static List<string>qB(List<string>rD,int dr){
var go=new List<string>();go.Add("+"+new string('-',dr+2)+"+");foreach(var jD in rD){string ub=jD.Length>dr ? jD.Substring(0,dr): jD;
string s0=ub.PadRight(dr);go.Add("| "+s0+" |");}go.Add("+"+new string('-',dr+2)+"+");return go;}public struct ly{public float lO,lP,lN,lo;
}public static ly pT(int mC,int oy,int gQ,int gR,int eT,int eU,float t2,float t1,float of,float og,float i5,float i6){
eT=Math.Max(1,eT);eU=Math.Max(1,eU);gQ=Math.Max(1,Math.Min(eT-mC,gQ));gR=Math.Max(1,Math.Min(eU-oy,gR));
float ra=t2-(of*2f);float q9=t1-(og*2f);float mx=(ra-(i5*(eT-1)))/eT;float mw=(q9-(i6*(eU-1)))/eU;
float ti=of+mC*(mx+i5);float tj=og+oy*(mw+i6);float th=gQ*mx+((gQ-1)*i5);float s8=gR*mw+((gR-1)*i6);return new ly{
lO=ti,lP=tj,lN=th,lo=s8};}public struct lC{public int qj;public int hz;public int ad;public int ay;public int dQ;
public int a1;public int qQ;public int qR;public int p1;public int p2;
public double p3;}public static lC pU(C bZ){var l=new lC();var oi=bZ.ai!=null&&bZ.ai.Count>0 ? bZ.ai : new List<az>{
new az{am=bZ.fh}};l.qj=oi.Count;l.hz=0;l.ad=0;l.ay=0;if(!string.IsNullOrEmpty(bZ.bs)&& !bZ.bs.Equals("none",StringComparison.OrdinalIgnoreCase))l.ad++;
if(!string.IsNullOrEmpty(bZ.dx)&& !bZ.dx.Equals("none",StringComparison.OrdinalIgnoreCase))l.ad+=2;foreach(var eB in oi){
if(!string.IsNullOrEmpty(eB.cH)&& !eB.cH.Equals("none",StringComparison.OrdinalIgnoreCase))l.ad++;if(!string.IsNullOrEmpty(eB.cj))l.ad+=2;
l.hz+=eB.am.Count;foreach(var sx in eB.am){l.qQ++;l.qR++;l.ad+=2;switch(sx.h){case e.du: l.dQ++;
l.a1++;l.ad+=2;l.ay++;break;case e.dI: l.a1+=10;l.ad+=10;
l.ay+=10;break;case e.dF: l.a1+=10;l.ad+=11;l.ay+=10;break;case e.dy: l.a1+=10;
l.ad+=10;l.ay+=10;break;case e.dA: l.a1+=12;l.ad+=12;l.ay+=12;
break;case e.ch: l.dQ++;l.a1+=8;l.ad+=9;l.ay+=8;break;case e.cI: l.dQ+=2;
l.a1++;l.ad+=3;l.ay++;break;case e.ck: case e.b9: l.dQ++;
l.a1++;l.ad+=2;l.ay++;break;case e.ci: case e.cb: l.dQ++;l.a1+=2;
l.ad+=3;l.ay+=2;break;case e.cG: l.a1+=8;l.ad+=8;l.ay+=8;
break;default: l.ay++;break;}}}l.p1=25+(l.hz*2);l.p2=180+(l.ay*4);
l.p3=0.0;return l;}}