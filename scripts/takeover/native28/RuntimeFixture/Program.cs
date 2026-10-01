using Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using CI=Mono.Cecil.Cil.Instruction;
using RO=System.Reflection.Emit.OpCodes;
public class InfrastructureFault:Exception {public InfrastructureFault(string s):base(s){}}
public static class Tr {
 public static List<string> Events=new();public static string Fail="";public static bool Fault;public static Action? OnItem,OnTruth;public static object? Made;
 public static void Reset(){Events.Clear();Fail="";Fault=false;OnItem=OnTruth=null;Made=null;}
 public static void Event(string s){if(Fault)throw new InfrastructureFault("Injected fixture invocation failure");Events.Add(s);if(Fail==s)throw new ApplicationException(s);}
}
// Explicit helper doubles. No Unity player or original helper body is executed.
public class Obj {
 public bool Alive=true;
 public static bool op_Implicit(Obj? o){bool result=o!=null&&o.Alive;var cb=Tr.OnTruth;Tr.OnTruth=null;cb?.Invoke();return result;}
 public static bool op_Equality(Obj? a,Obj? b)=>!op_Inequality(a,b);
 public static bool op_Inequality(Obj? a,Obj? b){bool an=a==null||!a.Alive,bn=b==null||!b.Alive;return an!=bn||(!an&&!ReferenceEquals(a,b));}
}
public class L<T>{public int _size;public List<T> Items=new();public T get_Item(int i){Tr.Event("item:"+i);var value=Items[i];Tr.OnItem?.Invoke();return value;}}
public class Attack:Obj{}
public class Skill {public int ID;}
public class Zombie:Obj{}
public class Plant:Obj {
 public bool skillOngoing;public Skill? skill;public RangeUI? attackRangeUIController;
 public List<Zombie> EnemySeeking_plural(int id,Attack? a){Tr.Event("seek:"+id+":"+(ReferenceEquals(a,RangeUI.Result)?"result":"other"));return Result;}
 public static List<Zombie> Result=new();
}
public class Device:Obj{}
public class Sprite:Obj{}
public class Supplies{}
public class PlantManager {public L<Plant>? plantPrefabList;}
public class DeviceManager {public L<Device>? devicePrefabList;}
public class BoardPreview {public L<Device>? devicePrefabList;}
public class RangeUI {
 public L<Attack>? attackRangeList;public static Attack? Result=new();
 public Attack? GetAttackRange(int id){Tr.Event("range:"+id);return Result;}
}
public class SuppliesInitial {public L<Supplies>? suppliesInfos;}
public class Zoom {public L<float>? modifiedCharScale;}
public class Board:Obj {public bool gameStart;}
public class BoardManager:Obj {public static BoardManager? Instance;public Board? board;}
public class Resource {public static L<Sprite>? itemSprites;}
public class DebugDouble {public static void Log(object? o){Tr.Event("log:"+o);}}
public class Grid {public Plant? plant_sheath,plant_common,plant_bottom;}
public class Dialogue{}
public class DialogueManager {
 public Board? board;public L<Dialogue>? dialogueList;public int dialogueIndex,dialogueTriggerType;
 public void LoadDialogue(Dialogue? d){Tr.Event("load:"+dialogueIndex);Tr.Made=d;}
}
public class ClipData:Obj {public void set_sequence(string? s){Tr.Event("sequence:"+(s??"<null>"));}}
public class Controller {public ClipData? _clip;public void GotoAndStop(int id){Tr.Event("stop:"+id);}}
public class PlayWait {public Controller? Subscribed;public PlayWait(Controller? c){Tr.Event("ctor:"+(c==null?"null":"controller"));Tr.Made=this;}public PlayWait Subscribe(Controller c){Tr.Event("subscribe");Subscribed=c;return null!;}}
static class Native28Fixture {
 static readonly Dictionary<string,Type> Types=new(){["System.Object"]=typeof(object),["System.Int32"]=typeof(int),["System.Single"]=typeof(float),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.String"]=typeof(string),["System.NullReferenceException"]=typeof(NullReferenceException),["UnityEngine.Object"]=typeof(Obj),["Plant"]=typeof(Plant),["Device"]=typeof(Device),["Zombie"]=typeof(Zombie),["UnityEngine.Sprite"]=typeof(Sprite),["SuppliesInfo"]=typeof(Supplies),["AttackRange"]=typeof(Attack),["Skill"]=typeof(Skill),["PlantManager"]=typeof(PlantManager),["DeviceManager"]=typeof(DeviceManager),["BoardPreview"]=typeof(BoardPreview),["AttackRangeUIController"]=typeof(RangeUI),["SuppliesInitialValue"]=typeof(SuppliesInitial),["TMPro.Examples.VertexZoom/<>c__DisplayClass10_0"]=typeof(Zoom),["Board"]=typeof(Board),["BoardManager"]=typeof(BoardManager),["ResourceManager"]=typeof(Resource),["UnityEngine.Debug"]=typeof(DebugDouble),["Grid"]=typeof(Grid),["BoardDialogue"]=typeof(Dialogue),["DialogueTriggerType"]=typeof(int),["DialogueManager_OnBoard"]=typeof(DialogueManager),["FTRuntime.SwfClip"]=typeof(ClipData),["FTRuntime.SwfClipController"]=typeof(Controller),["FTRuntime.Yields.SwfWaitPlayStopped"]=typeof(PlayWait)};
 static Type T(TypeReference r){if(r.FullName=="System.IntPtr")return typeof(nint);if(r.FullName=="System.Int64")return typeof(long);if(r is GenericInstanceType g&&g.ElementType.FullName=="System.Collections.Generic.List`1"){var item=T(g.GenericArguments[0]);return item==typeof(Zombie)?typeof(List<Zombie>):typeof(L<>).MakeGenericType(item);}return Types.TryGetValue(r.FullName,out var t)?t:throw new InfrastructureFault("Unmapped fixture type "+r.FullName);}
 // Contract strings are generated from the archived original-input extraction, never from the candidate.
 static readonly Dictionary<int,string> Contracts=new(){[0x060001F1]="Plant PlantManager::GetPlantPrefab(System.Int32)",[0x060005BD]="AttackRange AttackRangeUIController::GetAttackRange(System.Int32)",[0x0600080E]="System.Int32 TMPro.Examples.VertexZoom/<>c__DisplayClass10_0::<AnimateVertexColors>b__0(System.Int32,System.Int32)",[0x0600025D]="SuppliesInfo SuppliesInitialValue::GetSuppliesInfo(System.Int32)",[0x06000146]="System.Boolean GlobalStaticVars::IsOnBoard()",[0x0600022C]="UnityEngine.Sprite ResourceManager::Load_supplies_sprite(System.Int32)",[0x06000189]="Device DeviceManager::GetDevicePrefab(System.Int32)",[0x060005D8]="Device BoardPreview::GetDevicePrefab(System.Int32)",[0x0600043B]="Plant Zombie::FindPlant_Grid(Grid)",[0x060008B9]="FTRuntime.Yields.SwfWaitPlayStopped FTRuntime.Yields.SwfWaitExtensions::GotoAndStopAndWaitPlay(FTRuntime.SwfClipController,System.String,System.Int32)",[0x06000361]="System.Collections.Generic.List`1<Zombie> Plant::EnemySeeking_plural(System.Int32)",[0x0600019C]="System.Boolean DialogueManager_OnBoard::TriggerCheck(DialogueTriggerType)"};
 static DynamicMethod Emit(MethodDefinition m,string mutant="none"){
  if(m.FullName!=Contracts[m.MetadataToken.ToInt32()])throw new InfrastructureFault("Pinned signature mismatch");if(mutant=="fault_emit")throw new InfrastructureFault("Injected emitter failure");
  var args=(m.HasThis?new[]{T(m.DeclaringType)}:Array.Empty<Type>()).Concat(m.Parameters.Select(p=>T(p.ParameterType))).ToArray();var dm=new DynamicMethod(m.Name+mutant,T(m.ReturnType),args,typeof(Native28Fixture).Module,true){InitLocals=m.Body.InitLocals};var il=dm.GetILGenerator();var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());var ops=typeof(RO).GetFields(BindingFlags.Public|BindingFlags.Static).Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);bool inverted=false;
  void Default(){var rt=T(m.ReturnType);if(rt==typeof(bool)||rt==typeof(int))il.Emit(RO.Ldc_I4_0);else il.Emit(RO.Ldnull);}
  if(mutant=="illegal_stack")il.Emit(RO.Pop);
  foreach(var i in m.Body.Instructions){il.MarkLabel(labels[i]);var op=ops[i.OpCode.Value];var a=i.Operand;
   if(mutant=="invert_null"&&!inverted&&op==RO.Brtrue){var comparison=i.Previous?.Previous?.Previous;var producer=comparison?.Previous;while(producer?.OpCode.Code==Mono.Cecil.Cil.Code.Nop)producer=producer.Previous;if(comparison?.OpCode.Code==Mono.Cecil.Cil.Code.Ceq&&producer?.OpCode.Code==Mono.Cecil.Cil.Code.Ldnull){op=RO.Brfalse;inverted=true;}}
   if(mutant=="return_nre"&&op==RO.Throw){il.Emit(RO.Pop);Default();il.Emit(RO.Ret);continue;}
   if(a is MethodReference c){var owner=T(c.DeclaringType);var ps=c.Parameters.Select(p=>T(p.ParameterType)).ToArray();if(c.Name==".ctor")il.Emit(op,owner.GetConstructor(ps)??throw new InfrastructureFault("Ctor unmapped"));else il.Emit(op,owner.GetMethod(c.Name,ps)??throw new InfrastructureFault("Call unmapped "+c.FullName));continue;}
   if(a is FieldReference f){string name=f.Name=="<Instance>k__BackingField"?"Instance":f.Name;il.Emit(op,T(f.DeclaringType).GetField(name)??throw new InfrastructureFault("Field unmapped "+f.FullName));continue;}
   if(a is CI j){il.Emit(op,labels[j]);continue;}if(a is CI[] js){il.Emit(op,js.Select(j=>labels[j]).ToArray());continue;}
   if(a is Mono.Cecil.Cil.VariableDefinition v){int ix=mutant=="default_comparer"&&i.Offset==74?9:v.Index;il.Emit(op,locals[ix]);continue;}
   if(a is ParameterDefinition p){int ix=p.Index+(m.HasThis?1:0);if(mutant=="swap_comparer")ix=ix==1?2:1;il.Emit(op,(short)ix);continue;}
   if(a is int x){il.Emit(op,x);continue;}if(a is float f32){il.Emit(op,f32);continue;}if(a is sbyte b){il.Emit(op,b);continue;}if(a is string s){il.Emit(op,s);continue;}if(a==null){il.Emit(op);continue;}throw new InfrastructureFault("Operand unmapped "+a);
  }return dm;
 }
 static (object? Value,string? Error) Invoke(DynamicMethod dm,object?[] args){try{return(dm.Invoke(null,args),null);}catch(TargetInvocationException e)when(e.InnerException is NullReferenceException or ArgumentOutOfRangeException or ApplicationException or InvalidProgramException or System.Security.VerificationException){return(null,e.InnerException!.GetType().Name);}catch(InvalidProgramException e){return(null,e.GetType().Name);}catch(System.Security.VerificationException e){return(null,e.GetType().Name);}}
 record Case(string Name,bool Pass,string? Error,string[] Events,string[] Expected);
 static List<Case> Check(int token,DynamicMethod dm,bool fault=false){var rows=new List<Case>();
  void Test(string name,Func<object?[]> setup,object? expected,string? error,params string[] events){Tr.Reset();var args=setup();Tr.Fault=fault;var(v,e)=Invoke(dm,args);rows.Add(new(name,e==error&&(error!=null||Equals(v,expected))&&Tr.Events.SequenceEqual(events),e,Tr.Events.ToArray(),events));}
  static L<T> ListOf<T>(params T[] x)=>new(){Items=x.ToList(),_size=x.Length};
  int[] ids={int.MinValue,-1,0,1,2,3,int.MaxValue};
  if(token is 0x060001F1 or 0x06000189 or 0x060005D8){
   foreach(int id in ids)foreach(int itemState in new[]{0,1,2})foreach(bool nullList in new[]{false,true}){
    bool plant=token==0x060001F1;Obj? item=itemState==0?null:plant?new Plant{Alive=itemState==1}:new Device{Alive=itemState==1};bool accesses=plant||2>=id;string? err=nullList?"NullReferenceException":accesses&&(id<0||id>=2)?"ArgumentOutOfRangeException":null;object? expected=accesses&&itemState==1?item:null;
    Test($"prefab:{id}:{itemState}:{nullList}",()=>plant?new object?[]{new PlantManager{plantPrefabList=nullList?null:ListOf((Plant?)item!, (Plant?)item!)},id}:token==0x06000189?new object?[]{new DeviceManager{devicePrefabList=nullList?null:ListOf((Device?)item!, (Device?)item!)},id}:new object?[]{new BoardPreview{devicePrefabList=nullList?null:ListOf((Device?)item!,(Device?)item!)},id},expected,err,!nullList&&accesses?new[]{"item:"+id}:Array.Empty<string>());
   }Test("null-this",()=>new object?[]{null,0},null,"NullReferenceException");return rows;
  }
  if(token is 0x060005BD or 0x0600025D or 0x0600022C){
   foreach(int id in ids)foreach(bool nullThis in new[]{false,true})foreach(bool nullList in new[]{false,true}){
    bool supply=token==0x0600025D,resource=token==0x0600022C;object item=supply?new Supplies():resource?new Sprite():new Attack();bool skip=supply&&id<0;string? err=skip?null:(nullThis&&!resource)||nullList?"NullReferenceException":id<0?"ArgumentOutOfRangeException":null;object? expected=!skip&&id>=0&&id<2&&err==null?item:null;var events=skip||err=="NullReferenceException"?Array.Empty<string>():id<2?new[]{"item:"+id}:resource?new[]{"log:访问超出范围-材料贴图"}:Array.Empty<string>();
    Test($"range:{id}:{nullThis}:{nullList}",()=>{if(resource){Resource.itemSprites=nullList?null:ListOf((Sprite)item,(Sprite)item);return new object?[]{id};}return new object?[]{nullThis?null:supply?(object)new SuppliesInitial{suppliesInfos=nullList?null:ListOf((Supplies)item,(Supplies)item)}:new RangeUI{attackRangeList=nullList?null:ListOf((Attack)item,(Attack)item)},id};},expected,err,events);
   }return rows;
  }
  if(token==0x0600080E){
   // Oracle follows the inspected leaf native ordered/unordered float branches, not candidate calls.
   static int Order(float a,float b)=>a<b?-1:a>b?1:a==b?0:float.IsNaN(a)?float.IsNaN(b)?0:-1:1;
   float[] values={float.NaN,float.NegativeInfinity,-3,-0f,0,2,float.PositiveInfinity};
   foreach(float a in values)foreach(float b in values)Test($"float:{a}:{b}",()=>new object?[]{new Zoom{modifiedCharScale=ListOf(a,b)},0,1},Order(a,b),null,"item:0","item:1");
   foreach(int i in ids)foreach(int j in ids){bool bad=i<0||i>=2||j<0||j>=2;Test($"indices:{i}:{j}",()=>new object?[]{new Zoom{modifiedCharScale=ListOf(3f,-2f)},i,j},bad?null:i==j?0:i==0?1:-1,bad?"ArgumentOutOfRangeException":null,i<0||i>=2?new[]{"item:"+i}:new[]{"item:"+i,"item:"+j});}
   Test("null-this",()=>new object?[]{null,0,1},null,"NullReferenceException");Test("null-list",()=>new object?[]{new Zoom(),0,1},null,"NullReferenceException");
   Test("list-reload-null",()=>{var z=new Zoom{modifiedCharScale=ListOf(3f,-2f)};Tr.OnItem=()=>z.modifiedCharScale=null;return new object?[]{z,0,1};},null,"NullReferenceException","item:0");
   Test("list-reload-replaced",()=>{var z=new Zoom{modifiedCharScale=ListOf(3f,-2f)};Tr.OnItem=()=>z.modifiedCharScale=ListOf(-20f,4f);return new object?[]{z,0,1};},-1,null,"item:0","item:1");return rows;
  }
  if(token==0x06000146){foreach(int manager in new[]{0,1,2})foreach(int board in new[]{0,1,2}){Test($"onboard:{manager}:{board}",()=>{BoardManager.Instance=manager==0?null:new BoardManager{Alive=manager==1,board=board==0?null:new Board{Alive=board==1}};return Array.Empty<object?>();},manager==1&&board==1,null);}Test("static-reload-null",()=>{BoardManager.Instance=new BoardManager{board=new Board()};Tr.OnTruth=()=>BoardManager.Instance=null;return Array.Empty<object?>();},null,"NullReferenceException");return rows;}
  if(token==0x0600043B){foreach(int sh in new[]{0,1,2})foreach(int co in new[]{0,1,2})foreach(int bo in new[]{0,1,2}){Plant? s=sh==0?null:new(){Alive=sh==1},c=co==0?null:new(){Alive=co==1},b=bo==0?null:new(){Alive=bo==1};object? expected=sh==1?s:co==1?c:bo==1?b:null;Test($"grid:{sh}:{co}:{bo}",()=>new object?[]{null,new Grid{plant_sheath=s,plant_common=c,plant_bottom=b}},expected,null);}Test("grid-null",()=>new object?[]{new Zombie(),null},null,"NullReferenceException");return rows;}
  if(token==0x06000361){foreach(bool active in new[]{false,true})foreach(bool nullSkill in new[]{false,true})foreach(bool nullRange in new[]{false,true})foreach(int id in ids){bool bad=nullRange||(active&&nullSkill);Test($"seek:{active}:{nullSkill}:{nullRange}:{id}",()=>new object?[]{new Plant{skillOngoing=active,skill=nullSkill?null:new Skill{ID=id},attackRangeUIController=nullRange?null:new RangeUI()},17},bad?null:Plant.Result,bad?"NullReferenceException":null,bad?Array.Empty<string>():new[]{"range:"+(active?id:0),"seek:17:result"});}Test("null-this",()=>new object?[]{null,0},null,"NullReferenceException");return rows;}
  if(token==0x0600019C){foreach(int board in new[]{0,1,2})foreach(bool nullList in new[]{false,true})foreach(int idx in ids)foreach(int kind in new[]{0,1,2})foreach(int incoming in new[]{0,1,2}){
    var dialogue=new Dialogue();bool alive=board==1;bool access=alive&&!nullList&&idx<2&&kind!=0&&incoming==kind;string? err=alive&&nullList?"NullReferenceException":access&&idx<0?"ArgumentOutOfRangeException":null;Test($"trigger:{board}:{nullList}:{idx}:{kind}:{incoming}",()=>new object?[]{new DialogueManager{board=board==0?null:new Board{Alive=alive},dialogueList=nullList?null:ListOf(dialogue,dialogue),dialogueIndex=idx,dialogueTriggerType=kind},incoming},access&&err==null,err,access?err==null?new[]{"item:"+idx,"load:"+idx}:new[]{"item:"+idx}:Array.Empty<string>());
   }Test("null-this",()=>new object?[]{null,1},null,"NullReferenceException");return rows;}
  if(token==0x060008B9){foreach(int clipState in new[]{0,1,2})foreach(string? seq in new string?[]{null,"","run"})foreach(int id in ids){
    var c=new Controller{_clip=clipState==0?null:new ClipData{Alive=clipState==1}};Tr.Reset();var events=new List<string>();if(clipState==1)events.Add("sequence:"+(seq??"<null>"));events.Add("stop:"+id);events.Add("ctor:null");events.Add("subscribe");Tr.Fault=fault;var(v,e)=Invoke(dm,new object?[]{c,seq,id});rows.Add(new($"wrapper:{clipState}:{seq}:{id}",e==null&&ReferenceEquals(v,Tr.Made)&&v is PlayWait p&&ReferenceEquals(p.Subscribed,c)&&Tr.Events.SequenceEqual(events),e,Tr.Events.ToArray(),events.ToArray()));
   }Test("null-controller",()=>new object?[]{null,"run",0},null,"NullReferenceException");Test("clip-reload-null",()=>{var c=new Controller{_clip=new ClipData()};Tr.OnTruth=()=>c._clip=null;return new object?[]{c,"run",0};},null,"NullReferenceException");return rows;}
  throw new InfrastructureFault("Missing pinned oracle");
 }
 record Mutation(string Token,string Name,string Status,bool Detected,int Failed,int ClrRejected,string? ToolError);
 static Mutation Evaluate(MethodDefinition m,string name,bool fault=false){try{var rows=Check(m.MetadataToken.ToInt32(),Emit(m,name),fault);int clr=rows.Count(r=>r.Error is "InvalidProgramException" or "VerificationException"),failed=rows.Count(r=>!r.Pass);return new($"0x{m.MetadataToken.ToInt32():X8}",name,clr>0?"CLR_REJECTED":failed>0?"BEHAVIOR_MISMATCH":"MISSED",failed>0,failed,clr,null);}catch(Exception e){return new($"0x{m.MetadataToken.ToInt32():X8}",name,"TOOL_ERROR",false,0,0,e.ToString());}}
 static int Main(string[] args){var positives=new List<object>();var mutations=new List<Mutation>();try{using var asm=AssemblyDefinition.ReadAssembly(args[0]);bool fault=args.Length>2;foreach(var(token,signature)in Contracts){var m=(MethodDefinition)asm.MainModule.LookupToken(token);if(fault){mutations.Add(Evaluate(m,args[2],args[2]=="fault_invoke"));break;}positives.Add(new{token=$"0x{token:X8}",signature,cases=Check(token,Emit(m))});foreach(var name in new[]{"invert_null","return_nre"})mutations.Add(Evaluate(m,name));if(token==0x0600080E)foreach(var name in new[]{"default_comparer","swap_comparer"})mutations.Add(Evaluate(m,name));}if(!fault)foreach(int t in new[]{0x0600080E,0x060005BD})mutations.Add(Evaluate((MethodDefinition)asm.MainModule.LookupToken(t),"illegal_stack"));
    var json=JsonSerializer.SerializeToElement(positives);int cases=0,fails=0;foreach(var row in json.EnumerateArray())foreach(var c in row.GetProperty("cases").EnumerateArray()){cases++;if(!c.GetProperty("Pass").GetBoolean())fails++;}bool gate=!fault&&fails==0&&mutations.All(x=>x.Detected&&x.ToolError==null);var report=new{runtime=Environment.Version.ToString(),scope="Real emitted candidate CIL; pinned original PC outer contracts with helper doubles, not original Unity player/helper execution",methods=positives.Count,positive_cases=cases,positive_failures=fails,negative_controls=mutations.Count,tool_errors=mutations.Count(x=>x.ToolError!=null),gate_pass=gate,positives,mutations};File.WriteAllText(args[1],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(new{report.methods,report.positive_cases,report.positive_failures,report.negative_controls,report.tool_errors,report.gate_pass}));return gate?0:2;
   }catch(Exception e){Console.Error.WriteLine(e);return 2;}}
}
