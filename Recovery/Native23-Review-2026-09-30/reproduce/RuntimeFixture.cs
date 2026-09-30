using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using Mono.Cecil;
using ROp=System.Reflection.Emit.OpCodes;
public static class Events {public static List<string> Log=new();}
public class Board {public Grid grid;public Grid GetGrid(int x,int y){Events.Log.Add($"GetGrid:{x},{y}");return grid;}}
public class Grid {public bool allowed;public bool CanPlacing(Device d){Events.Log.Add("CanPlacing");return allowed;}}
public class Device {public Board board;public void Placing(int x,int y){Events.Log.Add($"Placing:{x},{y}");}public void PlacingNoop(int x,int y){}}
namespace UnityEngine {
 public class Animator {public string id;}
 public class Transform {public string id;}
 public class GameObject {public string id;public Animator animator;public Transform tr;public bool failGet,failTransform;
  public T GetComponent<T>() where T:class {Events.Log.Add("GetComponent:"+id);if(failGet)throw new ApplicationException("get");return animator as T;}
  public Transform transform {get {Events.Log.Add("Transform:"+id);if(failTransform)throw new ApplicationException("transform");return tr;}}
 }
}
public class Plant {
 public UnityEngine.GameObject animationGroup,ani_aniPart;public UnityEngine.Animator animator;public List<UnityEngine.GameObject> animationSprites;public bool failLoop;
 public void LoopAddAnimation(UnityEngine.Transform tr){Events.Log.Add("Loop:"+(tr?.id??"null")+":count="+animationSprites.Count);if(failLoop)throw new ApplicationException("loop");}
}
class RuntimeFixture {
 static Dictionary<string,Type> map=new(){["Device"]=typeof(Device),["Board"]=typeof(Board),["Grid"]=typeof(Grid),["Plant"]=typeof(Plant),["System.Int32"]=typeof(int),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.NullReferenceException"]=typeof(NullReferenceException),["UnityEngine.GameObject"]=typeof(UnityEngine.GameObject),["UnityEngine.Animator"]=typeof(UnityEngine.Animator),["UnityEngine.Transform"]=typeof(UnityEngine.Transform),["System.Collections.Generic.List`1<UnityEngine.GameObject>"]=typeof(List<UnityEngine.GameObject>)};
 static Type T(TypeReference r)=>map.TryGetValue(r.FullName,out var v)?v:throw new Exception("unsupported type "+r.FullName);
 static Delegate Emit(MethodDefinition m,bool mutant=false){
  if(m.Body.ExceptionHandlers.Count!=0)throw new Exception("EH unsupported");
  var args=(m.HasThis?new[]{T(m.DeclaringType)}:Array.Empty<Type>()).Concat(m.Parameters.Select(p=>T(p.ParameterType))).ToArray();
  var dm=new DynamicMethod(m.Name+(mutant?"_mutant":""),T(m.ReturnType),args,typeof(RuntimeFixture).Module,true);var il=dm.GetILGenerator();
  var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());
  var opmap=typeof(ROp).GetFields(BindingFlags.Public|BindingFlags.Static).Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)).ToDictionary(o=>o.Value);
  foreach(var i in m.Body.Instructions){il.MarkLabel(labels[i]);var op=opmap[i.OpCode.Value];var a=i.Operand;
   if(a==null){il.Emit(op);continue;}
   if(a is Mono.Cecil.Cil.Instruction target){il.Emit(op,labels[target]);continue;}
   if(a is Mono.Cecil.Cil.VariableDefinition vv){il.Emit(op,locals[vv.Index]);continue;}
   if(a is ParameterDefinition pp){il.Emit(op,(short)(pp.Index+(m.HasThis?1:0)));continue;}
   if(a is FieldReference fr){var name=fr.Name;if(mutant&&m.Name=="Awake"&&name=="animationGroup")name="ani_aniPart";var f=T(fr.DeclaringType).GetField(name);if(f==null||f.FieldType!=T(fr.FieldType))throw new Exception("unbound field "+fr);il.Emit(op,f);continue;}
   if(a is MethodReference mr){var owner=T(mr.DeclaringType);var aa=mr.Parameters.Select(p=>T(p.ParameterType)).ToArray();
    if(mr.Name==".ctor"){il.Emit(op,owner.GetConstructor(aa)??throw new Exception("unbound ctor"));continue;}
    var name=mutant&&m.Name=="TryPlacing"&&mr.Name=="Placing"?"PlacingNoop":mr.Name;
    MethodInfo mi;
    if(mr is GenericInstanceMethod gm){mi=owner.GetMethods().Single(x=>x.Name==name&&x.IsGenericMethodDefinition).MakeGenericMethod(gm.GenericArguments.Select(T).ToArray());}
    else mi=owner.GetMethod(name,aa)??throw new Exception("unbound method "+mr);
    il.Emit(op,mi);continue;
   }
   if(a is int n){il.Emit(op,n);continue;}if(a is sbyte sn){il.Emit(op,sn);continue;}if(a is byte bn){il.Emit(op,bn);continue;}
   throw new Exception("unsupported operand "+a);
  }
  return dm.CreateDelegate(m.Name=="Awake"?typeof(Action<Plant>):typeof(Func<Device,int,int,bool>));
 }
 static object DeviceTest(Func<Device,int,int,bool> f,int scenario,int x,int y){
  Events.Log.Clear();var d=new Device{board=scenario==0?null:new Board{grid=scenario==1?null:new Grid{allowed=scenario==3}}};bool? value=null;string exception=null;
  try{value=f(d,x,y);}catch(Exception e){exception=e.GetType().Name;}
  var expected=scenario==0?Array.Empty<string>():scenario==1?new[]{$"GetGrid:{x},{y}"}:scenario==2?new[]{$"GetGrid:{x},{y}","CanPlacing"}:new[]{$"GetGrid:{x},{y}","CanPlacing",$"Placing:{x},{y}"};
  var pass=Events.Log.SequenceEqual(expected)&&(scenario==0?exception=="NullReferenceException"&&value==null:exception==null&&value==(scenario==3));
  return new{scenario,x,y,value,exception,events=Events.Log.ToArray(),pass};
 }
 static object AwakeTest(Action<Plant> f,int scenario){
  Events.Log.Clear();var old=new UnityEngine.Animator{id="old"};var fresh=new UnityEngine.Animator{id="fresh"};
  var group=new UnityEngine.GameObject{id="group",animator=scenario==4?null:fresh,failGet=scenario==5};var part=new UnityEngine.GameObject{id="part",animator=new UnityEngine.Animator{id="wrong"},tr=new UnityEngine.Transform{id="part-transform"},failTransform=scenario==6};
  var p=new Plant{animationGroup=scenario==1?null:group,ani_aniPart=scenario==3?null:part,animationSprites=scenario==2?null:new List<UnityEngine.GameObject>{group,part},animator=old,failLoop=scenario==7};string ex=null;
  try{f(p);}catch(Exception e){ex=e.GetType().Name;}
  var expected=scenario==1?Array.Empty<string>():scenario==2||scenario==3||scenario==5?new[]{"GetComponent:group"}:scenario==6?new[]{"GetComponent:group","Transform:part"}:new[]{"GetComponent:group","Transform:part","Loop:part-transform:count=0"};
  string expectedEx=scenario>=1&&scenario<=3?"NullReferenceException":scenario>=5?"ApplicationException":null;
  var expectedAnimator=scenario==1||scenario==5?old:scenario==4?null:fresh;int? expectedCount=scenario==2?null:scenario==1||scenario==5?2:0;
  bool pass=Events.Log.SequenceEqual(expected)&&ex==expectedEx&&ReferenceEquals(p.animator,expectedAnimator)&&p.animationSprites?.Count==expectedCount;
  return new{scenario,exception=ex,events=Events.Log.ToArray(),animator=p.animator?.id,count=p.animationSprites?.Count,pass};
 }
 static bool Pass(object x)=> (bool)x.GetType().GetProperty("pass").GetValue(x);
 static void Main(string[] args){
  using var asm=AssemblyDefinition.ReadAssembly(args[0]);var dev=asm.MainModule.Types.Single(t=>t.Name=="Device").Methods.Single(m=>m.MetadataToken.ToUInt32()==0x06000339);var plant=asm.MainModule.Types.Single(t=>t.Name=="Plant").Methods.Single(m=>m.MetadataToken.ToUInt32()==0x06000348);
  var df=(Func<Device,int,int,bool>)Emit(dev);var af=(Action<Plant>)Emit(plant);var dm=(Func<Device,int,int,bool>)Emit(dev,true);var am=(Action<Plant>)Emit(plant,true);
  var dtests=new List<object>();foreach(var pair in new[]{(0,0),(1,2),(-3,4),(int.MinValue,int.MaxValue)})for(int s=0;s<4;s++)dtests.Add(DeviceTest(df,s,pair.Item1,pair.Item2));
  var atests=Enumerable.Range(0,8).Select(s=>AwakeTest(af,s)).ToArray();var mutants=new[]{DeviceTest(dm,3,1,2),AwakeTest(am,0)};
  var result=new{runtime=Environment.Version.ToString(),scope="Exact candidate CIL emitted with explicitly bound helper doubles; no Unity player execution",device=dtests,awake=atests,positive_pass=dtests.Concat(atests).All(Pass),positive_cases=dtests.Count+atests.Length,mutants,mutants_detected=mutants.All(x=>!Pass(x))};
  System.IO.File.WriteAllText(args[1],JsonSerializer.Serialize(result,new JsonSerializerOptions{WriteIndented=true}));if(!result.positive_pass||!result.mutants_detected)Environment.Exit(1);
 }
}
