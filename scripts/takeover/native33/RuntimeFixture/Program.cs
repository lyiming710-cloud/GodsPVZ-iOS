using Mono.Cecil;
using CI=Mono.Cecil.Cil.Instruction;
using RO=System.Reflection.Emit.OpCodes;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using System.Security.Cryptography;
public class Grid {}
public class Row {public List<Grid>? grids;}
public class Map {public List<Row>? rows;}
public class BoardConfig {public Map? map;}
public struct Vector3 {public float x,y,z;}
static class Fixture {
 static Dictionary<string,Type> Map=new(){["BoardConfig"]=typeof(BoardConfig),["Map"]=typeof(Map),["Row"]=typeof(Row),["Grid"]=typeof(Grid),["System.Collections.Generic.List`1<Row>"]=typeof(List<Row>),["System.Collections.Generic.List`1<Grid>"]=typeof(List<Grid>),["UnityEngine.Vector3"]=typeof(Vector3),["System.Int32"]=typeof(int),["System.UInt32"]=typeof(uint),["System.Single"]=typeof(float),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.Object"]=typeof(object),["System.IntPtr"]=typeof(IntPtr),["System.NullReferenceException"]=typeof(NullReferenceException)};
 static Dictionary<int,DynamicMethod> Nested=new();
 static Type T(TypeReference t)=>t.IsByReference?T(((ByReferenceType)t).ElementType).MakeByRefType():Map.TryGetValue(t.FullName,out var type)?type:throw new Exception("Unmapped type "+t.FullName);
 delegate void U(uint p,uint q,ref float r,ref float g,ref float b,ref float a);
 static DynamicMethod Emit(MethodDefinition m,string mutation){
  var dm=new DynamicMethod(m.Name+mutation,T(m.ReturnType),(m.HasThis?new[]{m.DeclaringType.IsValueType?T(m.DeclaringType).MakeByRefType():T(m.DeclaringType)}:Array.Empty<Type>()).Concat(m.Parameters.Select(p=>T(p.ParameterType))).ToArray(),typeof(Fixture).Module,true){InitLocals=m.Body.InitLocals};
  var il=dm.GetILGenerator();var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());
  var ops=typeof(RO).GetFields().Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
  if(mutation=="illegal_stack")il.Emit(RO.Pop);
  bool changed=mutation=="illegal_stack";
  foreach(var i in m.Body.Instructions){
   il.MarkLabel(labels[i]);var op=ops[i.OpCode.Value];var a=i.Operand;
   if(mutation=="wrong_return"&&op==RO.Ret&&m.ReturnType.FullName!="System.Void"){
    il.Emit(RO.Pop);if(T(m.ReturnType)==typeof(Vector3)){var zero=il.DeclareLocal(typeof(Vector3));il.Emit(RO.Ldloca,zero);il.Emit(RO.Initobj,typeof(Vector3));il.Emit(RO.Ldloc,zero);}else il.Emit(RO.Ldc_I4,0);changed=true;
   }
   if(mutation=="wrong_spacing"&&op==RO.Ldc_R4&&Equals(a,114f)){a=115f;changed=true;}
   if(mutation=="wrong_center_y"&&op==RO.Ldc_R4&&Equals(a,67f)){a=0f;changed=true;}
   if(mutation=="unsigned_grid_div"&&op==RO.Div){op=RO.Div_Un;changed=true;}
   if(a is CI target){il.Emit(op,labels[target]);continue;}
   if(a is CI[] ts){il.Emit(op,ts.Select(t=>labels[t]).ToArray());continue;}
   if(a is Mono.Cecil.Cil.VariableDefinition v){il.Emit(op,locals[v.Index]);continue;}
   if(a is ParameterDefinition p){il.Emit(op,(short)(p.Index+(m.HasThis?1:0)));continue;}
   if(a is MethodReference mr){
    if(mr.DeclaringType.FullName=="Map"||mr.DeclaringType.FullName=="BoardConfig"){
     var token=mr.Resolve().MetadataToken.ToInt32();if(!Nested.TryGetValue(token,out var nested))throw new Exception("Unbuilt real CIL dependency "+mr.FullName);
     // DynamicMethods are static with explicit this. Every nested original body
     // dereferences this, preserving null rejection; no C# helper implements it.
     il.Emit(RO.Call,nested);continue;
    }
    if(mr.Name==".ctor"&&mr.DeclaringType.FullName=="System.NullReferenceException"){il.Emit(op,typeof(NullReferenceException).GetConstructor(Type.EmptyTypes)!);continue;}
    var owner=T(mr.DeclaringType);var mi=owner.GetMethod(mr.Name,mr.Parameters.Select(p=>T(p.ParameterType)).ToArray())??throw new Exception("Missing runtime method "+mr);
    il.Emit(op,mi);continue;
   }
   if(a is TypeReference tr){il.Emit(op,T(tr));continue;}
   if(a is FieldReference field){il.Emit(op,T(field.DeclaringType).GetField(field.Name)!);continue;}
   if(a is int n){il.Emit(op,n);continue;}if(a is float f){il.Emit(op,f);continue;}if(a is sbyte b){il.Emit(op,b);continue;}
   if(a==null){il.Emit(op);continue;}throw new Exception("Unmapped operand "+a);
  }
  if(mutation!="none"&&!changed)throw new Exception("Mutation not applied "+mutation);
  return dm;
 }
 static float F(long n)=>BitConverter.Int32BitsToSingle(unchecked((int)n));
 static uint B(float f)=>unchecked((uint)BitConverter.SingleToInt32Bits(f));
 static bool Same(float a,long b)=>B(a)==(uint)b||(float.IsNaN(a)&&float.IsNaN(F(b)));
 static Map MakeMap(long mode,int width,int height){
  var m=new Map();if(mode==0)return m;m.rows=new List<Row>();
  for(int i=0;i<height;i++){var row=new Row{grids=new List<Grid>()};int n=i==0?width:(width+7)%101;for(int k=0;k<n;k++)row.grids.Add(new Grid());m.rows.Add(row);}return m;
 }
 static List<string> Cases(MethodDefinition m,DynamicMethod dm,List<long[]> rows,bool stop=false){
  var failures=new List<string>();int tok=m.MetadataToken.ToInt32(),ix=0;
  foreach(var r in rows){var map=MakeMap(r[0],(int)r[1],(int)r[2]);object?[] args=tok is 0x06000289 or 0x0600028A?new object?[]{map}:new object?[]{new BoardConfig{map=map},(int)r[3],(int)r[4]};
   var result=dm.Invoke(null,args);bool match=result is Vector3 v?Same(v.x,r[^3])&&Same(v.y,r[^2])&&B(v.z)==(uint)r[^1]:Convert.ToInt64(result)==r[^1];
   if(!match){failures.Add("row:"+ix+" expected:"+string.Join(",",r.TakeLast(3))+" actual:"+result);if(stop)break;}ix++;
  }
  if(tok is 0x0600015B or 0x0600015C){
   foreach(var a in new[]{new object?[]{null,0,0},new object?[]{new BoardConfig(),0,0},new object?[]{new BoardConfig{map=new Map{rows=new List<Row>{null!}}},0,0},new object?[]{new BoardConfig{map=new Map{rows=new List<Row>{new Row()}}},0,0}}){
    try{dm.Invoke(null,a);failures.Add("null-chain-not-rejected");}catch(TargetInvocationException e)when(e.InnerException is NullReferenceException){}
   }
  }else if(tok==0x06000289){
   foreach(var a in new object?[]{null,new Map{rows=new List<Row>{null!}},new Map{rows=new List<Row>{new Row()}}}){try{dm.Invoke(null,new[]{a});failures.Add("null-row-chain-not-rejected");}catch(TargetInvocationException e)when(e.InnerException is NullReferenceException){}}
  }else if(tok==0x0600028A){try{dm.Invoke(null,new object?[]{null});failures.Add("null-map-not-rejected");}catch(TargetInvocationException e)when(e.InnerException is NullReferenceException){}}
  return failures;
 }
 static int Main(string[] args){try{
  using var a=AssemblyDefinition.ReadAssembly(args[0]);var all=File.ReadLines(args[1]).Select(l=>l.Split(' ')).GroupBy(l=>l[0]).ToDictionary(g=>g.Key,g=>g.Select(l=>l.Skip(1).Select(long.Parse).ToArray()).ToList());
  if(all.Count!=4)throw new Exception("Expected four original-native method oracles");
  var expected=new Dictionary<int,string>{[0x0600015B]="UnityEngine.Vector3 BoardConfig::GetGridPosition(System.Int32,System.Int32)",[0x0600015C]="UnityEngine.Vector3 BoardConfig::GetGridCenterPosition(System.Int32,System.Int32)",[0x06000289]="System.Int32 Map::GetMapX()",[0x0600028A]="System.Int32 Map::GetMapY()"};
  foreach(int token in new[]{0x06000289,0x0600028A,0x0600015B}){var dep=(MethodDefinition)a.MainModule.LookupToken(token);if(dep.FullName!=expected[token])throw new Exception("Dependency signature mismatch");Nested[token]=Emit(dep,"none");}
  var positives=new List<object>();var controls=new List<object>();int bad=0,total=0,errors=0;bool detected=true;
  foreach(var (key,rows) in all){int token=Convert.ToInt32(key,16);var m=(MethodDefinition)a.MainModule.LookupToken(token);if(m.FullName!=expected[token])throw new Exception("Signature pin failed "+key);
   var failures=Cases(m,Emit(m,"none"),rows);bad+=failures.Count;int count=rows.Count+(token is 0x0600015B or 0x0600015C?4:token==0x06000289?3:1);total+=count;positives.Add(new{token=key,signature=m.FullName,cases=count,failures});
   foreach(string mut in (args.Length>3?Array.Empty<string>():new[]{"wrong_return","illegal_stack"}.Concat(token==0x0600015B?new[]{"wrong_spacing"}:token==0x0600015C?new[]{"wrong_center_y"}:Array.Empty<string>()))){
    bool reject=false;string? error=null;try{reject=Cases(m,Emit(m,mut),rows,true).Count>0;}catch(TargetInvocationException e)when(e.InnerException is InvalidProgramException){reject=true;}catch(InvalidProgramException){reject=true;}catch(Exception e){error=e.ToString();errors++;}
    detected&=reject&&error==null;controls.Add(new{token=key,mutation=mut,detected=reject,tool_error=error});
   }
  }
  bool pass=bad==0&&errors==0&&detected;var report=new{candidate_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[0]))).ToLowerInvariant(),oracle_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[1]))).ToLowerInvariant(),methods=all.Count,positive_cases=total,positive_failures=bad,negative_controls=controls.Count,tool_errors=errors,gate_pass=pass,scope="Real CLR candidate bodies and unchanged Map helper bodies versus executed original PC coordinate closure, including original native generic List.get_Item. Valid controlled List layouts, initialized Map metadata flags. Signed int32 overflow edges, finite domains. Dynamic internal calls use explicit-this static adapters; all dependency bodies dereference this. All Vector3 channel bits compared exactly; finite signed int32 edge samples and valid List layouts, not Unity/iOS/device acceptance",positives,controls};
  File.WriteAllText(args[2],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(new{report.methods,report.positive_cases,report.positive_failures,report.negative_controls,report.tool_errors,report.gate_pass}));return pass?0:2;
 }catch(Exception e){Console.Error.WriteLine(e);return 3;}}
}
