using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine{
 public class Object {public static implicit operator bool(Object o)=>o!=null;public static bool operator !=(Object a,Object b)=>!ReferenceEquals(a,b);public static bool operator ==(Object a,Object b)=>ReferenceEquals(a,b);public override bool Equals(object o)=>ReferenceEquals(this,o);public override int GetHashCode()=>base.GetHashCode();}
 public struct Vector3 {public float x,y,z;public Vector3(float a,float b,float c){x=a;y=b;z=c;}}
 public class Component:Object{public Transform transform{get;set;}public GameObject gameObject{get;set;}public T GetComponent<T>() where T:class=>this is Transform t?t.Renderer as T:null;}
 public class SpriteRenderer:Component{}
 public class Transform:Component{
  public Vector3 position{get;set;}public SpriteRenderer Renderer;public List<Transform> Children=new();public int Enumerated,Disposed;public bool FailMove;
  public IEnumerator GetEnumerator(){Enumerated++;return new Traversal(this);}
  class Traversal:IEnumerator,IDisposable{Transform root;int index=-1;public Traversal(Transform t){root=t;}public object Current=>root.Children[index];public bool MoveNext(){if(root.FailMove)throw new InvalidOperationException("fixture traversal failure");return ++index<root.Children.Count;}public void Reset()=>throw new NotSupportedException();public void Dispose(){root.Disposed++;}}
 }
 public class GameObject:Object{public Transform transform{get;set;}}
 public static class Time {public static float deltaTime{get;set;}}
}
public class Project{
 public List<GameObject> animationSprites=new();
 // Seed the exact method references consumed by the shared patch emitter.
 public void LoopAddAnimation(Transform parent){if(parent.GetComponent<SpriteRenderer>())animationSprites.Add(parent.gameObject);var en=parent.GetEnumerator();try{while(en.MoveNext())LoopAddAnimation((Transform)en.Current);}finally{(en as IDisposable)?.Dispose();}throw new NotImplementedException();}
}
public class Damage{public float damagePoint;}
public class DamageText {public static Vector3 Last;public static float Point;public static DamageText CreatDamageText(float p,int type,Vector3 pos){Point=p;Last=pos;return new DamageText();}}
public class HPUIController_Zombie:UnityEngine.Object {public int Count;public void Ashe(){Count++;}}
public class BoardConfig{}
public class Board:UnityEngine.Object{public Map map=new();public BoardConfig boardConfig=new();}
public class Map{public List<Row> rows=new();}
public class Row{public List<Grid> grids=new();}
public class Grid{public bool isHome;}
public class EnemyPath{
 public int x,y,Cost;public float Weight;public BoardConfig Config;
 public EnemyPath(int x,int y,float weight,BoardConfig config){this.x=x;this.y=y;Weight=weight;Config=config;}
 public static int DistanceStatistics(List<EnemyPath> path,int x,int y)=>path[0].Cost;
}
public class Zombie:Component {
 public float fX,fY,fZ;public bool isDied,dieOnTest,nutZombie_hotNut;public Vector3 rSpeed,previousPosition,direction;public GameObject animationGroup,UI_HP,UI_Elements,UI_Buff;public Transform shadow;public HPUIController_Zombie hpUIController;
 public int deaths,nuts,testCalls;public bool deathResidue,deathAshe;
 public int ID,gridX,gridY;public Board board;public List<EnemyPath> prePath=new(),path=new();public Func<EnemyPath,bool> PathTest;
 public bool Path_Finding()=>PathTest(prePath[0]);
 public void CreateStartPrePath(){prePath.Add(null);throw new NotImplementedException();}
 public Vector3 GetMoveDirection()=>direction;public void SetrSpeed(Vector3 speed){rSpeed=speed;}public void TestPosition(float x,float y){testCalls++;if(dieOnTest)isDied=true;}
 public void Start_PreviousPosition()=>throw new NotImplementedException();public void Update_Move()=>throw new NotImplementedException();public void Update_PreviousPosition()=>throw new NotImplementedException();public void Ashe(Damage d)=>throw new NotImplementedException();
 public void Die(bool noResidue,bool ashe){deaths++;deathResidue=noResidue;deathAshe=ashe;}public void ZC_NutBoom(){nuts++;}
}
public static class Program{
 static int checks;static void Check(bool ok,string s){if(!ok)throw new Exception(s);checks++;}
 static void Vec(Vector3 p,float x,float y,float z,string s){Check(p.x==x&&p.y==y&&p.z==z,s+" got="+p.x+","+p.y+","+p.z);}
 static Transform T(float x,float y,float z){var t=new Transform{position=new Vector3(x,y,z)};t.transform=t;return t;}
 static GameObject G(float x,float y,float z)=>new GameObject{transform=T(x,y,z)};
 static Zombie Z()=>new Zombie{fX=10,fY=20,fZ=5,transform=T(10,20,7),animationGroup=G(10,25,7),shadow=T(10,25,7)};
 static void Throws(Action f,string name){try{f();}catch(NullReferenceException){checks++;return;}throw new Exception(name);}
 public static void Main(){
  var z=Z();z.Start_PreviousPosition();Vec(z.previousPosition,10,20,5,"initial height");z.animationGroup=null;Throws(()=>z.Start_PreviousPosition(),"animation null must throw");
  z=Z();Time.deltaTime=.5f;z.direction=new Vector3(4,6,0);z.Update_Move();Vec(z.transform.position,12,23,7,"root move");Vec(z.animationGroup.transform.position,12,28,7,"animation fZ");Check(z.testCalls==1,"TestPosition called");
  z=Z();z.dieOnTest=true;z.direction=new Vector3(4,6,0);z.Update_Move();Vec(z.transform.position,10,20,7,"dead skips root");Check(z.fX==12&&z.fY==23,"logical position before TestPosition");
  z=Z();z.animationGroup=null;Throws(()=>z.Update_Move(),"move animation null throws");
  z=Z();z.previousPosition=new Vector3(10,20,5);z.transform.position=new Vector3(12,23,7);z.shadow.position=new Vector3(12,29,7);z.UI_HP=G(13,30,0);z.UI_Elements=G(15,40,1);z.UI_Buff=G(17,50,2);z.Update_PreviousPosition();Vec(z.UI_HP.transform.position,15,34,7,"hp follows deltas");Vec(z.UI_Elements.transform.position,17,44,7,"element follows deltas");Vec(z.UI_Buff.transform.position,19,54,7,"buff follows deltas");Vec(z.previousPosition,12,23,6,"updated height history");
  z.Update_PreviousPosition();Vec(z.UI_HP.transform.position,15,34,7,"stationary no drift");z.UI_HP=z.UI_Elements=z.UI_Buff=null;z.Update_PreviousPosition();checks++;z.shadow=null;Throws(()=>z.Update_PreviousPosition(),"shadow null still throws without UI");
  z=Z();z.hpUIController=new HPUIController_Zombie();z.Ashe(new Damage{damagePoint=42});Vec(DamageText.Last,10,159,7,"ash text fY+fZ+134");Check(DamageText.Point==42&&z.hpUIController.Count==1,"hp and damage preserved");Check(z.deaths==1&&z.deathResidue&&z.deathAshe&&z.nuts==0,"ash death flags");
  z=Z();z.nutZombie_hotNut=true;z.Ashe(new Damage());Check(z.nuts==1&&z.deaths==0,"hot nut route");
  var root=T(0,0,0);root.gameObject=G(0,0,0);root.Renderer=new SpriteRenderer();
  var middle=T(0,0,0);middle.gameObject=G(0,0,0);var leaf=T(0,0,0);leaf.gameObject=G(0,0,0);leaf.Renderer=new SpriteRenderer();root.Children.Add(middle);middle.Children.Add(leaf);
  var p=new Project();p.LoopAddAnimation(root);Check(p.animationSprites.Count==2&&p.animationSprites[0]==root.gameObject&&p.animationSprites[1]==leaf.gameObject,"preorder collection through non-renderer parent");
  Check(root.Disposed==1&&middle.Disposed==1&&leaf.Disposed==1,"all enumerators disposed on success");
  leaf.FailMove=true;try{p.LoopAddAnimation(root);throw new Exception("failure swallowed");}catch(InvalidOperationException){checks++;}
  Check(root.Disposed==2&&middle.Disposed==2&&leaf.Disposed==2,"nested exceptional unwind disposes every enumerator");
  Throws(()=>p.LoopAddAnimation(null),"null parent must fault");
  z=Z();z.board=new Board();z.board.map.rows.Add(new Row{grids=new(){new Grid{isHome=true},new Grid(),new Grid{isHome=true}}});z.board.map.rows.Add(new Row{grids=new(){new Grid{isHome=true}}});
  var old=new EnemyPath(-1,-1,0,null);z.prePath.Add(old);z.prePath.Add(old);int attempts=0;z.PathTest=e=>{Check(z.prePath.Count==1,"candidate list cleared between searches");Check(e.Config==z.board.boardConfig&&e.Weight==9999,"constructor config and weight");attempts++;e.Cost=e.y==0?(e.x==0?8:3):3;z.path=new(){e};return true;};
  z.CreateStartPrePath();Check(attempts==3&&z.prePath.Count==1&&z.prePath[0].x==2&&z.prePath[0].y==0,"minimum with first tie retained");
  var backing=(EnemyPath[])typeof(List<EnemyPath>).GetField("_items",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(z.prePath);Check(backing[1]==null,"clear removes stale backing reference");
  z.PathTest=e=>false;z.CreateStartPrePath();Check(z.path==null&&z.prePath.Count==0,"failed search clears path");
  z.ID=13;z.prePath.Add(old);z.CreateStartPrePath();Check(z.prePath[0]==old,"ID13 leaves path untouched");
  z.ID=0;z.board=null;z.CreateStartPrePath();Check(z.prePath[0]==old,"missing board leaves path untouched");
  z.board=new Board();z.prePath=null;Throws(()=>z.CreateStartPrePath(),"mandatory prePath null must fault");
  Console.WriteLine("NATIVE19_CORRECTION_CLR_PASS assertions="+checks);
 }
}
