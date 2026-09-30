using System;
using System.Linq;
using System.Collections.Generic;
using FTRuntime.Internal;
public static class Trace { public static List<string> Events=new(); public static void Add(string s)=>Events.Add(s); }
namespace UnityEngine {
 public struct Vector3 {public float x,y,z;}
 public class Transform {}
 public class Component {public Transform transform {get {Trace.Add("transform");return new();}}}
 public class Renderer {public string Name;public string sortingLayerName {set=>Trace.Add(Name+":layer:"+value);}public int sortingOrder {set=>Trace.Add(Name+":order:"+value);}}
 public class SpriteRenderer:Renderer {}
 public class GameObject {public string Name;public int Reads;public bool NullRenderer;public T GetComponent<T>() where T:class {Trace.Add(Name+":get:"+(++Reads));return NullRenderer?null:new SpriteRenderer{Name=Name+Reads} as T;}}
}
public enum ParticleState { Other=0, Target=30 }
public class BoardConfig {public UnityEngine.Vector3 GetGridPosition(int x,int y){Trace.Add("position:"+x+","+y);return new(){y=y+0.25f};}}
public class Board {public BoardConfig boardConfig=new();}
public class Plant {public Board board=new();public int GridX=3,GridY=7;}
public static class GlobalStaticVars {public static bool NullLower,NullRenderer;public static UnityEngine.GameObject GetAnimationSprite_Name(List<UnityEngine.GameObject> list,string name){Trace.Add("lookup:"+name);return name=="Lower"&&NullLower?null:new(){Name=name,NullRenderer=NullRenderer};}}
public class VFXAnimationEvent:UnityEngine.Component {
 public void LoopAddAnimation(UnityEngine.Transform t,List<UnityEngine.GameObject> list){Trace.Add("loop");}
 public void SetSorting<T>(ParticleState state,T host){new UnityEngine.GameObject().GetComponent<UnityEngine.SpriteRenderer>().sortingLayerName="stub";new UnityEngine.SpriteRenderer().sortingOrder=0;throw new Exception("unpatched");}
}
namespace FTRuntime.Internal {
 public class SwfList<T> {public T[] _data;public int _size;public SwfList(params T[] xs){_data=(T[])xs.Clone();_size=xs.Length;}public T UnorderedRemoveAt(int i)=>throw new IndexOutOfRangeException();}
 public class SwfAssocList<T> {public SwfList<T> _list;public Dictionary<T,int> _dict=new();public IEqualityComparer<T> _comp=EqualityComparer<T>.Default;public SwfAssocList(params T[] xs){_list=new(xs);for(int i=0;i<xs.Length;i++)_dict[xs[i]]=i;}public void Remove(T item)=>throw new Exception("unpatched");}
}
public class RecordingComparer<T>:IEqualityComparer<T> {public List<(T,T)> Calls=new();public bool Fail;public Action Before;public bool Equals(T a,T b){Calls.Add((a,b));Before?.Invoke();if(Fail)throw new ApplicationException();return EqualityComparer<T>.Default.Equals(a,b);}public int GetHashCode(T a)=>a.GetHashCode();}
public static class Program {
 static int checks;
 static void Check(bool ok,string name){if(!ok)throw new Exception("FAIL "+name);checks++;}
 static void Throws<T>(Action a,string name) where T:Exception {try{a();}catch(T){checks++;return;}throw new Exception("no throw "+name);}
 static void Clear(){Trace.Events.Clear();GlobalStaticVars.NullLower=false;GlobalStaticVars.NullRenderer=false;}
 public static void Main(){
  var s=new SwfList<string>("a","b","c");Check(s.UnorderedRemoveAt(0)=="c"&&s._size==2&&s._data.SequenceEqual(new[]{"c","b",null}),"reference first move and clear");
  Check(s.UnorderedRemoveAt(1)=="b"&&s._size==1&&s._data[1]==null,"remove last returns original");
  var v=new SwfList<int>(5,7,9);Check(v.UnorderedRemoveAt(1)==9&&v._size==2&&v._data.SequenceEqual(new[]{5,9,0}),"value middle default");
  var pair=new SwfList<(int,string)>((1,"a"),(2,"b"));Check(pair.UnorderedRemoveAt(0)==(2,"b")&&pair._data[1]==default,"structured value default");
  Throws<IndexOutOfRangeException>(()=>v.UnorderedRemoveAt(-1),"unsigned negative");Throws<IndexOutOfRangeException>(()=>v.UnorderedRemoveAt(v._size),"index equals size");Check(v._size==2,"invalid unchanged");
  var empty=new SwfList<int>();Throws<IndexOutOfRangeException>(()=>empty.UnorderedRemoveAt(0),"empty");
  var corrupt=new SwfList<int>(1);corrupt._size=2;Throws<IndexOutOfRangeException>(()=>corrupt.UnorderedRemoveAt(0),"bad last index");Check(corrupt._size==2,"bad read before size decrement");
  var nil=new SwfList<int>(1);nil._data=null;Throws<NullReferenceException>(()=>nil.UnorderedRemoveAt(0),"null data");Check(nil._size==1,"null read before decrement");
  var a=new SwfAssocList<string>("a","b","c");var cmp=new RecordingComparer<string>();a._comp=cmp;a.Remove("a");Check(a._dict.Count==2&&!a._dict.ContainsKey("a")&&a._dict["c"]==0&&a._list._size==2,"assoc first");Check(cmp.Calls.Single()==("c","a"),"comparison moved first");
  a.Remove("b");Check(!a._dict.ContainsKey("b")&&a._dict["c"]==0&&cmp.Calls.Last()==("b","b"),"assoc last compares but no reinsertion");
  int n=cmp.Calls.Count;a.Remove("missing");Check(cmp.Calls.Count==n&&a._list._size==1,"missing early return");
  var ai=new SwfAssocList<int>(2,4,6);ai.Remove(4);Check(ai._dict[6]==1&&ai._list._data[2]==0,"assoc value generic");
  Throws<ArgumentNullException>(()=>a.Remove(null),"dictionary null key");
  var fail=new SwfAssocList<string>("a","b");var throwing=new RecordingComparer<string>{Fail=true};fail._comp=throwing;Throws<ApplicationException>(()=>fail.Remove("a"),"comparer throws");Check(!fail._dict.ContainsKey("a")&&fail._dict["b"]==1&&fail._list._size==1&&fail._list._data[0]=="b","mutations before comparer");
  var broken=new SwfAssocList<int>(1);broken._dict[1]=5;Throws<IndexOutOfRangeException>(()=>broken.Remove(1),"bad dict index");Check(broken._dict.Count==0&&broken._list._size==1,"dict removal precedes list failure");
  var noList=new SwfAssocList<int>(1);noList._list=null;Throws<NullReferenceException>(()=>noList.Remove(1),"null list");Check(noList._dict.Count==0,"dict precedes null list");
  var noComp=new SwfAssocList<int>(1,2);noComp._comp=null;Throws<NullReferenceException>(()=>noComp.Remove(1),"null comparer");Check(noComp._list._size==1&&noComp._dict[2]==1,"list before null comparer");
  var fx=new VFXAnimationEvent();var plant=new Plant();Clear();fx.SetSorting(ParticleState.Other,plant);Check(Trace.Events.SequenceEqual(new[]{"transform","loop"}),"loop before state");
  Clear();fx.SetSorting<object>(ParticleState.Target,null);Check(Trace.Events.SequenceEqual(new[]{"transform","loop"}),"loop before null host");
  Clear();fx.SetSorting(ParticleState.Target,123);Check(Trace.Events.SequenceEqual(new[]{"transform","loop"}),"value host ignored");
  Clear();fx.SetSorting(ParticleState.Target,"wrong");Check(Trace.Events.Count==2,"wrong reference ignored");
  Clear();fx.SetSorting(ParticleState.Target,plant);Check(Trace.Events.SequenceEqual(new[]{"transform","loop","lookup:Lower","position:3,8","Lower:get:1","Lower1:layer:Entity","Lower:get:2","Lower2:order:-92","lookup:Upper","position:3,5","Upper:get:1","Upper1:layer:Entity","Upper:get:2","Upper2:order:-42"}),"sorting order math and four independent components");
  Clear();plant.board=null;Throws<NullReferenceException>(()=>fx.SetSorting(ParticleState.Target,plant),"null board");Check(Trace.Events.Last()=="lookup:Lower","lookup before board failure");plant.board=new();
  Clear();GlobalStaticVars.NullLower=true;Throws<NullReferenceException>(()=>fx.SetSorting(ParticleState.Target,plant),"missing lower");Check(Trace.Events.Last()=="position:3,8","position before missing sprite failure");
  Clear();GlobalStaticVars.NullRenderer=true;Throws<NullReferenceException>(()=>fx.SetSorting(ParticleState.Target,plant),"missing renderer");Check(Trace.Events.Last()=="Lower:get:1","renderer failure before second lookup");
  Console.WriteLine("NATIVE16_CLR_FIXTURE_PASS assertions="+checks);
 }
}
