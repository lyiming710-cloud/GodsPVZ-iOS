using System;
using System.Collections.Generic;
namespace UnityEngine {
 public class Object { public bool destroyed; public static bool operator ==(Object a,Object b){bool na=ReferenceEquals(a,null)||(!ReferenceEquals(a,null)&&a.destroyed);bool nb=ReferenceEquals(b,null)||(!ReferenceEquals(b,null)&&b.destroyed);return na&&nb||!na&&!nb&&ReferenceEquals(a,b);}public static bool operator !=(Object a,Object b){return !(a==b);}public static implicit operator bool(Object a){return a!=null;}public override bool Equals(object b){return ReferenceEquals(this,b);}public override int GetHashCode(){return base.GetHashCode();} }
 public class GameObject:Object {public bool active=true;public bool activeSelf {get{return active;}} public void SetActive(bool value){Trace.calls.Add("active");active=value;}}
 public class Component:Object {public GameObject go=new GameObject(); public GameObject gameObject{get{return go;}}}
 public class Behaviour:Component{} public class MonoBehaviour:Behaviour{}
 public class Animator:Behaviour {public string floatName,triggerName;public float floatValue;public Action onFloat,onTrigger;public void SetFloat(string name,float value){Trace.calls.Add("float");floatName=name;floatValue=value;if(onFloat!=null)onFloat();}public void SetTrigger(string name){Trace.calls.Add("trigger");triggerName=name;if(onTrigger!=null)onTrigger();}}
 public struct Vector3 {public float x,y,z;}
}
public enum ChallengeType {None=-1,Adventure=0,Rescue=1,HardAdventure=2,Branch=3}
public enum BoardEntryType {Zero=0,Point=1}
public class BoardEntry {public bool must,select;public BoardEntryType boardEntryType;public float value;public float GetValue(){return value;}}
public class BoardConfig {public List<BoardEntry> boardEntries;public Map map;}
public enum SenarioState {Daytime=0,Night=1,Snowfield=8,Twilight=9,DarkSnowfield=10}
public class Map {public SenarioState senarioState;}
public class SunManager {public float sunFallTimeMax;public void SetMaxSunFallTime(BoardConfig config,Map map){throw new Exception("UNPATCHED");}}
public static class Trace {public static List<string> calls=new List<string>();}
public class AttackRangeUIController:UnityEngine.MonoBehaviour {public void CollapseView(){Trace.calls.Add("collapse");}}
public class Plant:UnityEngine.MonoBehaviour {public AttackRangeUIController attackRangeUIController;public UnityEngine.GameObject shadow;public UnityEngine.Animator animator;public int ID,state;public bool attackable;public float updateRate;public void SetAnimationState_Planting(){throw new Exception("UNPATCHED");}}
public class Save {public int adventureLevel;public int[] rescuePassNum,adventureHardMaxStarNum;public bool[] adventureHardPassed;}
public class SavesManager {public Save playerSave;public void PassLevel(ChallengeType t,int level,BoardConfig config){throw new Exception("UNPATCHED");}}
public class Zombie:UnityEngine.MonoBehaviour {public float fX,fY,fW=2,fD=2,fH=2;public bool attackable=true;public int attackCalls;public bool CanAttacked(){attackCalls++;return attackable;}}
public class ZombieManager {public List<Zombie> zombieList;}
public class Board {public ZombieManager zombieManager;}
public class BoardPreview:UnityEngine.MonoBehaviour {public List<Zombie> enemyPreviews;}
public class PrepareUIController {public BoardPreview boardPreview;public static PrepareUIController instance;public static PrepareUIController Instance {get{return instance;}}}
public class MouseManager {public bool onBoard;public Board board;public UnityEngine.Vector3 mouseWorldPosition;public Zombie GetZombieUnderMouse(int plantID){throw new Exception("UNPATCHED");}}
public class ZombieInfo {public int id,enemyPoint,minWave;public float enemyWeight;public bool isElite,isBoss;public bool[] enemyRequirements;}
public class ZombieSelect {public int id,point,minFlag,minWave;public float weight;public bool exist,elite;public bool[] requirements;public ZombieSelect(ZombieInfo info){throw new Exception("UNPATCHED");}}
public class EnemySelecter {public List<ZombieSelect> zombieSelects=new List<ZombieSelect>();public int enemyPoint_Base;public Board board;public EnemyManager enemyManager;}
public class ResourceManager {public static List<ZombieInfo> infos;public static List<ZombieInfo> Load_zombieInfo_all(){return infos;}}
public class EnemyManager {public Board board;public int enemyPoint_Base;public List<BoardEntry> boardEntries;public List<int> zombieTypes;public EnemySelecter CreateEnemySelecter(){throw new Exception("UNPATCHED");}}
class Harness {
 static int n;
 static void A(bool c,string s){if(!c)throw new Exception("FAIL "+s);n++;Console.WriteLine("PASS "+s);}
 static void Throws<T>(Action a,string s) where T:Exception {try{a();}catch(T){A(true,s);return;}throw new Exception("did not throw "+s);}
 static ZombieInfo Info(int n){return new ZombieInfo{id=n,enemyPoint=10+n,enemyWeight=.5f+n,minWave=3+n,enemyRequirements=new bool[]{true,false,true,false,true,false,true,false,true,false}};}
 public static void Main(){
  var info=Info(2);info.isBoss=true;var zs=new ZombieSelect(info);A(zs.id==2&&zs.point==12&&zs.weight==2.5f&&zs.minWave==5&&zs.minFlag==0&&!zs.exist&&zs.elite,"constructor scalar fields");A(zs.requirements.Length==10&&!ReferenceEquals(zs.requirements,info.enemyRequirements)&&zs.requirements[8]&&!zs.requirements[9],"constructor copies ten bools");info.enemyRequirements[0]=false;A(zs.requirements[0],"constructor does not alias array");Throws<NullReferenceException>(()=>new ZombieSelect(null),"constructor null info throws");var shortInfo=Info(1);shortInfo.enemyRequirements=new bool[9];Throws<IndexOutOfRangeException>(()=>new ZombieSelect(shortInfo),"constructor short array throws");
  var sm=new SavesManager{playerSave=new Save{adventureLevel=180,rescuePassNum=new int[3],adventureHardMaxStarNum=new int[3],adventureHardPassed=new bool[3]}};sm.PassLevel(ChallengeType.Adventure,-99,null);A(sm.playerSave.adventureLevel==181,"adventure increments 180 to 181");sm.PassLevel(ChallengeType.Adventure,0,null);A(sm.playerSave.adventureLevel==181,"adventure stops above 180");sm.PassLevel(ChallengeType.Rescue,2,null);A(sm.playerSave.rescuePassNum[2]==1,"rescue increments indexed counter");
  var config=new BoardConfig{boardEntries=new List<BoardEntry>{new BoardEntry{must=true,select=true},new BoardEntry{select=true},new BoardEntry{select=false},new BoardEntry{select=true}}};sm.PassLevel(ChallengeType.HardAdventure,1,config);A(sm.playerSave.adventureHardMaxStarNum[1]==2&&sm.playerSave.adventureHardPassed[1],"hard counts only optional selected entries");sm.playerSave.adventureHardMaxStarNum[1]=5;sm.PassLevel(ChallengeType.HardAdventure,1,config);A(sm.playerSave.adventureHardMaxStarNum[1]==5,"hard maximum never decreases");sm.playerSave=null;sm.PassLevel((ChallengeType)77,-1,null);A(true,"unhandled challenge has no dereferences");Throws<NullReferenceException>(()=>sm.PassLevel(ChallengeType.Adventure,0,null),"adventure null save throws");sm.playerSave=new Save{rescuePassNum=new int[1]};Throws<IndexOutOfRangeException>(()=>sm.PassLevel(ChallengeType.Rescue,-1,null),"rescue negative index throws");Throws<NullReferenceException>(()=>sm.PassLevel(ChallengeType.HardAdventure,0,null),"hard null config throws");
  var z1=new Zombie();var z2=new Zombie();var inactive=new Zombie();inactive.go.active=false;var destroyed=new Zombie{destroyed=true};var mouse=new MouseManager{onBoard=true,board=new Board{zombieManager=new ZombieManager{zombieList=new List<Zombie>{null,destroyed,inactive,z1,z2}}}};A(ReferenceEquals(mouse.GetZombieUnderMouse(0),z2),"mouse returns LAST eligible overlap");z2.attackable=false;A(ReferenceEquals(mouse.GetZombieUnderMouse(0),z1),"plantID zero applies CanAttacked");A(ReferenceEquals(mouse.GetZombieUnderMouse(1),z2),"nonzero plantID bypasses CanAttacked");A(inactive.attackCalls==0&&destroyed.attackCalls==0,"inactive and destroyed skipped before attack filter");mouse.mouseWorldPosition.x=1;A(ReferenceEquals(mouse.GetZombieUnderMouse(1),z2),"right boundary inclusive");mouse.mouseWorldPosition.x=1.001f;A(mouse.GetZombieUnderMouse(1)==null,"outside right boundary rejected");mouse.mouseWorldPosition.x=float.NaN;A(ReferenceEquals(mouse.GetZombieUnderMouse(1),z2),"NaN follows native ordered comparisons");mouse.board=null;Throws<NullReferenceException>(()=>mouse.GetZombieUnderMouse(1),"board null preserves exception");
  mouse.onBoard=false;mouse.mouseWorldPosition.x=0;PrepareUIController.instance=new PrepareUIController();A(mouse.GetZombieUnderMouse(0)==null,"missing preview returns null through Unity bool");PrepareUIController.instance.boardPreview=new BoardPreview{enemyPreviews=new List<Zombie>{z2}};int calls=z2.attackCalls;A(ReferenceEquals(mouse.GetZombieUnderMouse(0),z2)&&z2.attackCalls==calls,"preview skips CanAttacked even plantID zero");PrepareUIController.instance=null;Throws<NullReferenceException>(()=>mouse.GetZombieUnderMouse(1),"preview null singleton throws");
  ResourceManager.infos=new List<ZombieInfo>{Info(0),Info(1),Info(2)};var manager=new EnemyManager{board=new Board(),enemyPoint_Base=101,boardEntries=new List<BoardEntry>{new BoardEntry{boardEntryType=BoardEntryType.Point,value=.5f},new BoardEntry{boardEntryType=BoardEntryType.Zero,value=99}},zombieTypes=new List<int>{2,0}};var es=manager.CreateEnemySelecter();A(ReferenceEquals(es.board,manager.board)&&ReferenceEquals(es.enemyManager,manager),"selector owner links");A(es.enemyPoint_Base==151,"selector base times additive multiplier truncates");A(es.zombieSelects.Count==2&&es.zombieSelects[0].id==0&&es.zombieSelects[1].id==2&&es.zombieSelects.TrueForAll(z=>z.exist),"selector flags and filters preserves source order");manager.zombieTypes=new List<int>();A(manager.CreateEnemySelecter().zombieSelects.Count==0,"empty selection removes all");manager.zombieTypes=new List<int>{99};Throws<ArgumentOutOfRangeException>(()=>manager.CreateEnemySelecter(),"invalid selection index throws");manager.zombieTypes=new List<int>{0};manager.boardEntries=null;Throws<NullReferenceException>(()=>manager.CreateEnemySelecter(),"null boardEntries throws");
  TestNewMethods();
  Console.WriteLine("NATIVE4_CLR_HARNESS_PASS assertions="+n);
 }
 static Plant P(int id){return new Plant{ID=id,state=7,attackable=false,updateRate=1.75f,attackRangeUIController=new AttackRangeUIController(),shadow=new UnityEngine.GameObject{active=false},animator=new UnityEngine.Animator()};}
 static BoardConfig C(int scene){return new BoardConfig{map=new Map{senarioState=(SenarioState)scene},boardEntries=new List<BoardEntry>()};}
 static BoardEntry E(int type,bool must,bool select){return new BoardEntry{boardEntryType=(BoardEntryType)type,must=must,select=select};}
 static void TestNewMethods(){
  foreach(int id in new[]{4,49,2,0,99}){
   var p=P(id);Trace.calls.Clear();p.animator.onTrigger=()=>A(p.state==1&&p.attackable,"plant state committed before trigger");p.SetAnimationState_Planting();
   A(p.attackable==(id!=4&&id!=49)&&p.state==(id==2?1:7),"plant ID branch "+id);
   A(string.Join(",",Trace.calls)==(id==2?"collapse,active,float,trigger":"collapse,active,float")&&p.shadow.active&&p.animator.floatName=="speed"&&p.animator.floatValue==1.75f&&p.animator.triggerName==(id==2?"ExplodeTrigger":null),"plant call order and literals "+id);
  }
  var p0=P(2);p0.attackRangeUIController=null;Trace.calls.Clear();Throws<NullReferenceException>(()=>p0.SetAnimationState_Planting(),"plant null range controller throws");A(Trace.calls.Count==0&&p0.state==7&&!p0.attackable,"plant first failure preserves later state");
  p0=P(2);p0.shadow=null;Trace.calls.Clear();Throws<NullReferenceException>(()=>p0.SetAnimationState_Planting(),"plant null shadow throws");A(string.Join(",",Trace.calls)=="collapse"&&p0.state==7,"plant shadow failure follows collapse only");
  p0=P(2);p0.animator=null;Trace.calls.Clear();Throws<NullReferenceException>(()=>p0.SetAnimationState_Planting(),"plant null animator throws");A(string.Join(",",Trace.calls)=="collapse,active"&&p0.state==7,"plant animator failure follows shadow activation");
  p0=P(2);var p2=p0;p0.animator.onFloat=()=>p2.animator=null;Throws<NullReferenceException>(()=>p2.SetAnimationState_Planting(),"plant second animator dereference throws");A(p2.state==1&&p2.attackable,"plant state committed before second animator failure");
  var sun=new SunManager{sunFallTimeMax=77};sun.SetMaxSunFallTime(null,null);A(sun.sunFallTimeMax==77,"sun null config returns unchanged");
  foreach(int scene in new[]{-1,0,1,2,3,4,5,6,7,8,9,10,11}){float expected=scene==0?8:scene==8?16:scene==9?12:scene==10?24:0;sun.SetMaxSunFallTime(C(scene),new Map{senarioState=(SenarioState)10});A(sun.sunFallTimeMax==expected,"sun scene "+scene+" comes from config map");}
  var c=C(0);c.boardEntries.Add(E(2,false,false));sun.SetMaxSunFallTime(c,null);A(sun.sunFallTimeMax==8,"sun unselected optional modifier ignored");
  c.boardEntries=new List<BoardEntry>{E(2,true,false),E(2,false,true),E(2,true,true)};sun.SetMaxSunFallTime(c,null);A(sun.sunFallTimeMax==64,"sun mandatory or selected doubles once per entry");
  c.boardEntries=new List<BoardEntry>{E(2,true,false),E(3,false,true),E(2,true,false)};sun.SetMaxSunFallTime(c,null);A(sun.sunFallTimeMax==0,"sun disable then doubling stays zero");
  c.boardEntries=new List<BoardEntry>{E(3,false,false),E(1,true,true),E(77,true,true)};sun.SetMaxSunFallTime(c,null);A(sun.sunFallTimeMax==8,"sun unrelated and inactive disable entries ignored");
  sun.sunFallTimeMax=77;c.map=null;Throws<NullReferenceException>(()=>sun.SetMaxSunFallTime(c,new Map()),"sun null config map throws despite map argument");A(sun.sunFallTimeMax==77,"sun map failure preserves output");
  c=C(0);c.boardEntries=null;Throws<NullReferenceException>(()=>sun.SetMaxSunFallTime(c,null),"sun null entry list throws");A(sun.sunFallTimeMax==77,"sun list failure preserves output");
  c.boardEntries=new List<BoardEntry>{E(2,true,false),null};Throws<NullReferenceException>(()=>sun.SetMaxSunFallTime(c,null),"sun null entry throws after pending modifier");A(sun.sunFallTimeMax==77,"sun exception leaves output uncommitted");
 }
}
