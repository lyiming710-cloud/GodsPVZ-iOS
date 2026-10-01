using Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using CI=Mono.Cecil.Cil.Instruction;
using VariableDefinition=Mono.Cecil.Cil.VariableDefinition;
using RO=System.Reflection.Emit.OpCodes;
public enum RangeType{None,Rect,Circle,Both,All}
public struct Rect {public float x,y,width,height;public Rect(float a,float b,float c,float d){x=a;y=b;width=c;height=d;}}
public struct Vector3 {public float x,y,z;public Vector3(float a,float b,float c){x=a;y=b;z=c;}}
public class Device{public float fX,fY,fW,fD;}
public class Plant{}public class Projectile{}public class Zombie{}
public class AttackRange{
 public RangeType rangeType;public bool rect,circle,rectFail,circleFail;public Action? hook;public List<string> events=new();public Rect? observedRect;public Vector3? observedCircle;
 public bool TestInRects_Rect(Rect r){events.Add("rect");observedRect=r;if(rectFail)throw new ApplicationException("rect");hook?.Invoke();return rect;}
 public bool TestInCircles_Position(Vector3 v){events.Add("circle");observedCircle=v;if(circleFail)throw new ApplicationException("circle");return circle;}
}
static class Native26Fixture{
 static readonly Dictionary<string,Type> types=new(){["Device"]=typeof(Device),["Plant"]=typeof(Plant),["Projectile"]=typeof(Projectile),["Zombie"]=typeof(Zombie),["AttackRange"]=typeof(AttackRange),["RangeType"]=typeof(RangeType),["UnityEngine.Rect"]=typeof(Rect),["UnityEngine.Vector3"]=typeof(Vector3),["System.Single"]=typeof(float),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.NullReferenceException"]=typeof(NullReferenceException)};
 static Type T(TypeReference r)=>types[r.FullName];
 static Func<AttackRange,Device,bool> Emit(MethodDefinition m,string mutation="none"){
  var dm=new DynamicMethod("range_"+mutation,typeof(bool),new[]{typeof(AttackRange),typeof(Device)},typeof(Native26Fixture).Module,true){InitLocals=m.Body.InitLocals};var il=dm.GetILGenerator();var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());var ops=typeof(RO).GetFields(BindingFlags.Public|BindingFlags.Static).Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
  foreach(var i in m.Body.Instructions){il.MarkLabel(labels[i]);var op=ops[i.OpCode.Value];var a=i.Operand;
   if(mutation=="short_circuit"&&i.OpCode.Code==Mono.Cecil.Cil.Code.Brtrue&&i.Offset!=1)op=RO.Brfalse;
   if(mutation=="center_z"&&a is float fl&&fl==0f)a=1f;
   if(a==null){il.Emit(op);continue;}
   if(a is CI jump){il.Emit(op,labels[jump]);continue;}
   if(a is CI[] table){var copy=table.ToArray();if(mutation=="all_false")copy[4]=copy[0];il.Emit(op,copy.Select(t=>labels[t]).ToArray());continue;}
   if(a is VariableDefinition v){if(mutation=="center_swap"&&op==RO.Ldloc_S&&v.Index==11){il.Emit(op,locals[12]);continue;}if(mutation=="center_swap"&&op==RO.Ldloc_S&&v.Index==12){il.Emit(op,locals[11]);continue;}il.Emit(op,locals[v.Index]);continue;}
   if(a is float f){il.Emit(op,f);continue;}
   if(a is FieldReference field){var name=mutation=="depth_width"&&field.Name=="fD"?"fW":field.Name;il.Emit(op,T(field.DeclaringType).GetField(name)!);continue;}
   if(a is MethodReference c){var owner=T(c.DeclaringType);var args=c.Parameters.Select(p=>T(p.ParameterType)).ToArray();if(c.Name==".ctor")il.Emit(op,owner.GetConstructor(args)!);else if(mutation=="skip_rect"&&c.Name=="TestInRects_Rect"){il.Emit(RO.Pop);il.Emit(RO.Pop);il.Emit(RO.Ldc_I4_0);}else il.Emit(op,owner.GetMethod(c.Name,args)!);continue;}
   throw new Exception("Unsupported "+a);
  }
  return (Func<AttackRange,Device,bool>)dm.CreateDelegate(typeof(Func<AttackRange,Device,bool>));
 }
 static int Bits(float f)=>BitConverter.SingleToInt32Bits(f);
 static bool Same(float a,float b)=>float.IsNaN(a)&&float.IsNaN(b)||Bits(a)==Bits(b);
 static object Case(Func<AttackRange,Device,bool> f,int range,int scenario,float x,float y,float w,float d){
  var device=new Device{fX=x,fY=y,fW=w,fD=d};var a=new AttackRange{rangeType=(RangeType)range,rect=scenario is 1 or 3,circle=scenario is 2 or 3,rectFail=scenario==4,circleFail=scenario==5};if(scenario==6)a.hook=()=>{device.fX=999;device.fY=-999;device.fW=222;device.fD=333;a.rangeType=RangeType.All;};
  var events=new List<string>();bool? expected=null;string? error=null;
  if(range==1||range==3){events.Add("rect");if(a.rectFail)error="ApplicationException";else if(range==1||a.rect)expected=a.rect;}
  if(error==null&&expected==null){if(range==2||range==3){events.Add("circle");if(a.circleFail)error="ApplicationException";else expected=a.circle;}else expected=range==4;}
  bool? result=null;string? ex=null;try{result=f(a,device);}catch(Exception e){ex=e.GetType().Name;}
  var r=a.observedRect;var v=a.observedCircle;bool shapes=(!events.Contains("rect")||r.HasValue&&Same(r.Value.x,x-w*.5f)&&Same(r.Value.y,y-d*.5f)&&Same(r.Value.width,w)&&Same(r.Value.height,d))&&(!events.Contains("circle")||v.HasValue&&Same(v.Value.x,x)&&Same(v.Value.y,y)&&Same(v.Value.z,0f));
  bool state=range==3&&scenario==6?device.fX==999&&a.rangeType==RangeType.All:Same(device.fX,x)&&a.rangeType==(RangeType)range;
  // Range==1 also invokes the mutating Rect helper, even though there is no later Circle.
  if(range==1&&scenario==6)state=device.fX==999&&a.rangeType==RangeType.All;
  bool pass=a.events.SequenceEqual(events)&&result==expected&&ex==error&&shapes&&state;return new{range,scenario,result,exception=ex,events=a.events.ToArray(),shape_snapshot_preserved=shapes,state_expected=state,pass};
 }
 static bool Pass(object x)=>(bool)x.GetType().GetProperty("pass")!.GetValue(x)!;
 static void Main(string[] args){using var asm=AssemblyDefinition.ReadAssembly(args[0]);var m=(MethodDefinition)asm.MainModule.LookupToken(0x060000D9);var f=Emit(m);var cases=new List<object>();var ranges=new[]{int.MinValue,-1,0,1,2,3,4,5,int.MaxValue};var shapes=new[]{(1.25f,-2.5f,3.75f,6.25f),(0f,-0f,-4f,8f),(float.MaxValue,float.MinValue,float.MaxValue,float.MaxValue),(float.NaN,float.PositiveInfinity,1f,float.NegativeInfinity)};foreach(var range in ranges)for(int s=0;s<7;s++)foreach(var (x,y,w,d) in shapes)cases.Add(Case(f,range,s,x,y,w,d));
  var nulls=new List<object>();foreach(var pair in new[]{(true,false),(false,true),(true,true)}){string? e=null;var a=new AttackRange{rangeType=RangeType.All};try{f(pair.Item1?null!:a,pair.Item2?null!:new Device());}catch(Exception ex){e=ex.GetType().Name;}nulls.Add(new{null_this=pair.Item1,null_device=pair.Item2,exception=e,pass=e=="NullReferenceException"&&a.events.Count==0});}
  var mutants=new List<object>();foreach(var name in new[]{"short_circuit","center_z","center_swap","depth_width","all_false","skip_rect"}){var mf=Emit(m,name);var rows=ranges.SelectMany(t=>Enumerable.Range(0,7).Select(s=>Case(mf,t,s,1.25f,-2.5f,3.75f,6.25f))).ToArray();mutants.Add(new{mutation=name,detected=rows.Any(r=>!Pass(r)),failing_cases=rows.Count(r=>!Pass(r))});}
  var report=new{runtime=Environment.Version.ToString(),scope="Actual candidate CIL with explicit Rect/Vector3 value shapes and helper doubles; original AttackRange helper bodies not executed",positive_cases=cases.Count+nulls.Count,positive_pass=cases.All(Pass)&&nulls.All(Pass),cases,nulls,mutants,mutants_detected=mutants.All(PassMutant)};File.WriteAllText(args[1],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));if(!report.positive_pass||!report.mutants_detected)Environment.Exit(1);
 }
 static bool PassMutant(object x)=>(bool)x.GetType().GetProperty("detected")!.GetValue(x)!;
}
