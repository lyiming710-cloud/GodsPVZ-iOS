using System;
using System.Linq;
using System.Collections.Generic;
using FTRuntime.Internal;
namespace FTRuntime.Internal { public class SwfList<T> {public T[] _data;public int _size;public SwfList(params T[] data){_data=data;_size=data.Length;}public void AssignTo(List<T> target)=>throw new IndexOutOfRangeException();public void AssignTo(SwfList<T> target)=>throw new NotSupportedException("untouched overload");} }
public static class Program {
 static int checks;
 static void Check(bool ok,string name){if(!ok)throw new Exception(name);checks++;}
 static void Throws<T>(Action f,string name) where T:Exception {try{f();}catch(T){checks++;return;}throw new Exception("missing throw "+name);}
 public static void Main(){
  var s=new SwfList<string>("a",null,"c");var l=new List<string>{"old"};s.AssignTo(l);Check(l.SequenceEqual(s._data),"ordered copy includes null");Check(l.Capacity==4,"adequate capacity retained");Check(s._size==3&&s._data[0]=="a","source unchanged");
  var small=new List<string>(1){"old"};s.AssignTo(small);Check(small.Capacity==6&&small.SequenceEqual(s._data),"growth exactly twice size");
  var same=new List<string>(3);s.AssignTo(same);Check(same.Capacity==3,"equal capacity not grown");
  var values=new SwfList<int>(4,2,0);var ints=new List<int>(1){9};values.AssignTo(ints);Check(ints.SequenceEqual(new[]{4,2,0})&&ints.Capacity==6,"value copy");
  var pairs=new SwfList<(int,string)>((2,"x"),(3,null));var pairList=new List<(int,string)>();pairs.AssignTo(pairList);Check(pairList.SequenceEqual(pairs._data),"struct copy");
  var empty=new SwfList<int>();var target=new List<int>(10){1,2};var en=target.GetEnumerator();empty.AssignTo(target);Check(target.Count==0&&target.Capacity==10,"empty clears retains capacity");Throws<InvalidOperationException>(()=>en.MoveNext(),"clear invalidates enumerator");
  Throws<NullReferenceException>(()=>values.AssignTo((List<int>)null),"null destination");
  var nil=new SwfList<int>(1);nil._data=null;target.Add(9);Throws<NullReferenceException>(()=>nil.AssignTo(target),"null source array");Check(target.Count==0,"destination cleared before source failure");
  var bad=new SwfList<int>(10,20);bad._size=3;var partial=new List<int>();Throws<IndexOutOfRangeException>(()=>bad.AssignTo(partial),"array short");Check(partial.SequenceEqual(new[]{10,20})&&partial.Capacity==6,"partial copy and capacity preserved on error");
  var negative=new SwfList<int>();negative._size=-1;target.Add(8);negative.AssignTo(target);Check(target.Count==0,"negative count clears no loop");
  var overflow=new SwfList<int>();overflow._size=int.MaxValue;target.Add(8);Throws<ArgumentOutOfRangeException>(()=>overflow.AssignTo(target),"capacity unchecked overflow");Check(target.Count==0,"clear before capacity overflow");
  var only=new SwfList<int>(1,2,3);only._size=1;only.AssignTo(target);Check(target.SequenceEqual(new[]{1}),"copy logical size only");
  Throws<NotSupportedException>(()=>only.AssignTo(new SwfList<int>()),"other overload unchanged");
  Console.WriteLine("NATIVE17_CLR_FIXTURE_PASS assertions="+checks);
 }
}
