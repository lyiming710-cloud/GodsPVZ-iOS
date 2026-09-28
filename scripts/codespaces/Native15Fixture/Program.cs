using System;
using System.Linq;
using System.Collections.Generic;
namespace UnityEngine {
 public class GameObject { }
 public class Component {public int reads; public GameObject value=new(); public GameObject gameObject {get{reads++;return value;}}}
 public static class Debug {public static List<string> logs=new();public static void Log(object x)=>logs.Add((string)x);}
 public static class Random {public static Queue<int> sequence=new();public static int calls;public static int Range(int a,int b){calls++;int v=sequence.Dequeue();if(a!=0||v<0||v>=b)throw new Exception("range mismatch");return v;}}
}
public class Plant {public List<UnityEngine.GameObject> particleSystems=new();}
public class Device { }
public class VFXAnimationEvent:UnityEngine.Component {
 public string type;public float duration;public Plant plant;public bool original=true;
 public void Binding<T>(T host,bool bind,float seconds,string text)=>throw new Exception("UNPATCHED Binding");
}
public class Grid {public int gridX,gridY;public bool allowed=true;public Action onCheck;public int checks; public bool CanPlacing(Device d){checks++;onCheck?.Invoke();return allowed;}}
public class Row {public List<Grid> grids=new();}
public class Map {
 public List<Row> rows=new();public int width=3,widthCalls;
 public int GetMapX(){widthCalls++;return width;}
 public List<Grid> RandomGet_Grid_TestPlace<T>(int n,T host,int begin,int end)=>throw new Exception("UNPATCHED Map");
}
static class Program {
 static int assertions;
 static void Check(bool value,string label){if(!value)throw new Exception(label);assertions++;}
 static void Throws<T>(Action a,string label)where T:Exception {try{a();}catch(T){assertions++;return;}throw new Exception(label);}
 static Map Make(){UnityEngine.Debug.logs.Clear();UnityEngine.Random.calls=0;UnityEngine.Random.sequence.Clear();return new Map{rows=new(){new Row{grids=new(){new Grid{gridX=0,gridY=0},new Grid{gridX=1,gridY=0},new Grid{gridX=2,gridY=0}}},new Row{grids=new(){new Grid{gridX=0,gridY=1},new Grid{gridX=1,gridY=1},new Grid{gridX=2,gridY=1}}}}};}
 // Keep exact external member signatures available for the same Cecil emitter.
 static void References(){var x=new HashSet<int>();x.Add(1);Console.WriteLine(string.Concat(1.ToString(),",",2.ToString()));}
 static void Main(){
  var old=new Plant();var p=new Plant();var v=new VFXAnimationEvent{plant=old};
  v.Binding(p,false,3.5f,"off");Check(v.type=="off"&&v.duration==3.5f&&v.plant==old&&p.particleSystems.Count==0&&v.reads==0,"flag false writes only metadata");
  v.Binding<Plant>(null,true,4,"null");Check(v.type=="null"&&v.plant==old&&v.reads==0,"null preserves existing binding");
  v.Binding(42,true,5,"value");Check(v.plant==old&&v.reads==0,"unconstrained value boxes and fails cast");
  v.Binding(new object(),true,6,"other");Check(v.plant==old,"wrong reference preserves plant");
  v.Binding(p,true,7,"plant");Check(v.plant==p&&p.particleSystems.Single()==v.value&&v.reads==1&&v.original,"plant list add and original preserved");
  p.particleSystems=null;Throws<NullReferenceException>(()=>v.Binding(p,true,8,"throw"),"null list must throw");Check(v.reads==2&&v.type=="throw"&&v.duration==8,"gameObject evaluated before null list failure");
  var d=new Device();var m=Make();var a=m.RandomGet_Grid_TestPlace(99,d,1,99);Check(a.Count==4&&a[0]==m.rows[0].grids[1]&&a[3]==m.rows[1].grids[2],"row-major eligible and width clamp");Check(m.widthCalls==6&&UnityEngine.Debug.logs.SequenceEqual(new[]{"1,0","2,0","1,1","2,1"})&&UnityEngine.Random.calls==0,"width re-evaluation and logs");
  m=Make();m.rows[0].grids[1].allowed=false;a=m.RandomGet_Grid_TestPlace(99,d,0,2);Check(a.Count==5&&m.rows.SelectMany(x=>x.grids).All(x=>x.checks==1),"CanPlacing filtering");
  m=Make();a=m.RandomGet_Grid_TestPlace(2,new object(),0,2);Check(a.Count==0&&UnityEngine.Debug.logs.Count==6&&m.rows[0].grids[0].checks==0,"wrong host still logs");
  m=Make();a=m.RandomGet_Grid_TestPlace<Device>(1,null,0,2);Check(a.Count==0&&UnityEngine.Debug.logs.Count==6,"null host still traverses");
  m=Make();a=m.RandomGet_Grid_TestPlace(1,17,0,2);Check(a.Count==0,"value host remains legal");
  m=Make();a=m.RandomGet_Grid_TestPlace(0,d,0,2);Check(a.Count==0&&UnityEngine.Debug.logs.Count==6&&UnityEngine.Random.calls==0,"zero request after traversal");
  m=Make();a=m.RandomGet_Grid_TestPlace(-2,d,0,2);Check(a.Count==0&&UnityEngine.Random.calls==0,"negative count preserves empty result");
  m=Make();UnityEngine.Random.sequence=new(new[]{4,4,1});a=m.RandomGet_Grid_TestPlace(2,d,0,2);Check(a.SequenceEqual(new[]{m.rows[1].grids[1],m.rows[0].grids[1]})&&UnityEngine.Random.calls==3,"duplicate random draws and hashset order");
  m=Make();UnityEngine.Random.sequence=new(new[]{5,4,3,2,1,0});a=m.RandomGet_Grid_TestPlace(6,d,0,2);Check(a.Count==6&&a[0]==m.rows[1].grids[2]&&UnityEngine.Random.calls==6,"equal count still samples");
  m=Make();a=m.RandomGet_Grid_TestPlace(3,d,0,int.MaxValue);Check(a.Count==0&&UnityEngine.Debug.logs.Count==0&&m.widthCalls==2,"unchecked upper-bound overflow");
  m=Make();m.rows=null;Throws<NullReferenceException>(()=>m.RandomGet_Grid_TestPlace(1,d,0,2),"null rows");
  m=Make();m.rows[0]=null;Throws<NullReferenceException>(()=>m.RandomGet_Grid_TestPlace(1,d,0,2),"null row");
  m=Make();m.rows[0].grids=null;Throws<NullReferenceException>(()=>m.RandomGet_Grid_TestPlace(1,d,0,2),"null grids");
  m=Make();m.rows[0].grids[0]=null;Throws<NullReferenceException>(()=>m.RandomGet_Grid_TestPlace(1,d,0,2),"null grid");
  m=Make();Throws<ArgumentOutOfRangeException>(()=>m.RandomGet_Grid_TestPlace(1,d,-1,2),"negative column");
  m=Make();m.rows[0].grids[0].onCheck=()=>m.rows.Add(new Row());Throws<InvalidOperationException>(()=>m.RandomGet_Grid_TestPlace(1,d,0,2),"row list mutation invalidates enumerator");
  Console.WriteLine("NATIVE15_CLR_FIXTURE_PASS assertions="+assertions);
 }
}
