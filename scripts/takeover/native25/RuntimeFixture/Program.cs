using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using Mono.Cecil;
using CInstruction=Mono.Cecil.Cil.Instruction;
using ROp=System.Reflection.Emit.OpCodes;
public static class Trace {public static List<string> Events=new();}
public class Device {public Board? board;}
public class Board {public Grid? grid;public bool fail;public Action? hook;public Grid? GetGrid(int x,int y){Trace.Events.Add($"GetGrid:{x},{y}");if(fail)throw new ApplicationException("get");hook?.Invoke();return grid;}}
public class Grid {public bool allowed,fail;public Device? observed;public Action<Device>? hook;public bool CanPlacing(Device d){Trace.Events.Add("CanPlacing");observed=d;if(fail)throw new ApplicationException("can");hook?.Invoke(d);return allowed;}}
internal static class Native25Fixture {
 static readonly Dictionary<string,Type> Types=new(){["Device"]=typeof(Device),["Board"]=typeof(Board),["Grid"]=typeof(Grid),["System.Int32"]=typeof(int),["System.Boolean"]=typeof(bool),["System.NullReferenceException"]=typeof(NullReferenceException),["System.Void"]=typeof(void)};
 static Type T(TypeReference r)=>Types.TryGetValue(r.FullName,out var t)?t:throw new Exception("Unbound type "+r.FullName);
 static Func<Device,int,int,bool> Emit(MethodDefinition m,string mutation="none"){
  if(m.Body.ExceptionHandlers.Count!=0||m.Body.Variables.Count!=0)throw new Exception("Unexpected body specification");
  var dm=new DynamicMethod("TestPlacing_"+mutation,typeof(bool),new[]{typeof(Device),typeof(int),typeof(int)},typeof(Native25Fixture).Module,true){InitLocals=m.Body.InitLocals};var il=dm.GetILGenerator();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());var ops=typeof(ROp).GetFields(BindingFlags.Static|BindingFlags.Public).Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
  foreach(var i in m.Body.Instructions){
   il.MarkLabel(labels[i]);var op=ops[i.OpCode.Value];var a=i.Operand;
   if(mutation=="board_branch"&&i.Offset==7)op=ROp.Brfalse_S;
   if(mutation=="grid_branch"&&i.Offset==24)op=ROp.Brfalse_S;
   if(mutation=="null_grid_true"&&i.Offset==27)op=ROp.Ldc_I4_1;
   if(mutation=="swap_coordinates"&&i.Offset==16)op=ROp.Ldarg_2;
   if(mutation=="swap_coordinates"&&i.Offset==17)op=ROp.Ldarg_1;
   if(mutation=="wrong_device"&&i.Offset==29)op=ROp.Ldnull;
   if(mutation=="skip_can"&&i.Offset==30){il.Emit(ROp.Pop);il.Emit(ROp.Pop);il.Emit(ROp.Ldc_I4_0);continue;}
   if(a==null){il.Emit(op);continue;}
   if(a is CInstruction target){il.Emit(op,labels[target]);continue;}
   if(a is FieldReference f){var field=T(f.DeclaringType).GetField(f.Name)!;if(field==null||field.FieldType!=T(f.FieldType))throw new Exception("Field signature mismatch");il.Emit(op,field);continue;}
   if(a is MethodReference c){var owner=T(c.DeclaringType);var args=c.Parameters.Select(p=>T(p.ParameterType)).ToArray();if(c.Name==".ctor")il.Emit(op,owner.GetConstructor(args)!);else {var method=owner.GetMethod(c.Name,args)!;if(method.ReturnType!=T(c.ReturnType))throw new Exception("Call signature mismatch");il.Emit(op,method);}continue;}
   throw new Exception("Unsupported operand "+a);
  }
  return (Func<Device,int,int,bool>)dm.CreateDelegate(typeof(Func<Device,int,int,bool>));
 }
 static object Case(Func<Device,int,int,bool> f,int scenario,int x,int y){
  Trace.Events.Clear();var grid=new Grid{allowed=scenario is 3 or 6 or 7 or 8,fail=scenario==5};var start=new Board{grid=scenario==1?null:grid,fail=scenario==4};var replacement=new Board{grid=new Grid{allowed=false}};var d=new Device{board=scenario==0?null:start};
  if(scenario==6)start.hook=()=>d.board=replacement;
  if(scenario==7)start.hook=()=>start.grid=new Grid{allowed=true};
  if(scenario==8)grid.hook=who=>who.board=replacement;
  bool? result=null;string? exception=null;try{result=f(d,x,y);}catch(Exception e){exception=e.GetType().Name;}
  var expected=scenario==0?Array.Empty<string>():scenario is 1 or 4?new[]{$"GetGrid:{x},{y}"}:new[]{$"GetGrid:{x},{y}","CanPlacing"};var expectEx=scenario==0?"NullReferenceException":scenario is 4 or 5?"ApplicationException":null;bool? expectValue=expectEx!=null?null:scenario is 3 or 6 or 7 or 8;
  bool identity=scenario is 0 or 1 or 4||ReferenceEquals(start.grid?.observed,d);bool boardIdentity=ReferenceEquals(d.board,scenario==0?null:scenario is 6 or 8?replacement:start);var pass=Trace.Events.SequenceEqual(expected)&&exception==expectEx&&result==expectValue&&identity&&boardIdentity;
  return new {scenario,x,y,result,exception,events=Trace.Events.ToArray(),grid_received_original_device=identity,board_state_expected=boardIdentity,pass};
 }
 static bool Pass(object r)=>(bool)r.GetType().GetProperty("pass")!.GetValue(r)!;
 static void Main(string[] a){
  using var asm=AssemblyDefinition.ReadAssembly(a[0]);var m=asm.MainModule.Types.Single(t=>t.Name=="Device").Methods.Single(m=>m.MetadataToken.ToUInt32()==0x06000338);var f=Emit(m);var cases=new List<object>();
  foreach(var (x,y) in new[]{(0,0),(1,2),(-3,4),(int.MinValue,int.MaxValue),(int.MaxValue,int.MinValue)})for(int scenario=0;scenario<9;scenario++)cases.Add(Case(f,scenario,x,y));
  Trace.Events.Clear();string? ex=null;try{f(null!,1,2);}catch(Exception e){ex=e.GetType().Name;}var nullThisPass=ex=="NullReferenceException"&&Trace.Events.Count==0;
  var names=new[]{"board_branch","grid_branch","null_grid_true","swap_coordinates","wrong_device","skip_can"};var mutants=new List<object>();foreach(var name in names){var mutant=Emit(m,name);var results=Enumerable.Range(0,9).Select(s=>Case(mutant,s,1,2)).ToArray();mutants.Add(new {mutation=name,detected=results.Any(r=>!Pass(r)),results});}
  var data=new {runtime=Environment.Version.ToString(),scope="Actual candidate CIL executed with explicit helper doubles; native CFG/call contracts checked separately; no Unity Player or original helper execution",positive_cases=cases.Count+1,positive_pass=cases.All(Pass)&&nullThisPass,cases,null_this=new {exception=ex,pass=nullThisPass},mutants,mutants_detected=mutants.All(x=>(bool)x.GetType().GetProperty("detected")!.GetValue(x)!)};
  File.WriteAllText(a[1],JsonSerializer.Serialize(data,new JsonSerializerOptions{WriteIndented=true}));if(!data.positive_pass||!data.mutants_detected)Environment.Exit(1);
 }
}
