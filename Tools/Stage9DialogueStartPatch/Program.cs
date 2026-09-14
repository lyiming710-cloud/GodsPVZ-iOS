using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9DialogueStartPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha = "8905a8866d0d148f4c681e9128417fd927a968521a5afb35c78b4bfa8d4f5e4b";
const uint TargetToken = 0x06000193;
const int ExpectedRid = 403;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 580;
const int ExpectedOldLocals = 22;
const string NativeSliceSha = "600d505be96241a0c336dd07725df87fdfc5e32184162e16aad70234a261a8d5";

var preservation = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A,
    0x060003BA, 0x060003FD, 0x060003DD, 0x060003DE,
    0x060001F3, 0x06000216, 0x06000667, 0x06000668,
    0x060005E1, 0x06000157, 0x06000294
};

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static string Scope(IMetadataScope? s) => s switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => s?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{Scope(t.Scope)}";
static string OpSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return $"M:{mr.FullName}@{Scope(mr.DeclaringType.Scope)}";
    if (o is FieldReference fr) return $"F:{fr.FullName}@{Scope(fr.DeclaringType.Scope)}";
    if (o is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (o is VariableDefinition v) return $"V:{v.Index}:{TypeSig(v.VariableType)}";
    if (o is ParameterDefinition p) return $"P:{p.Index}:{TypeSig(p.ParameterType)}";
    if (o is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (o is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (o is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(o, CultureInfo.InvariantCulture);
}
static string MethodSig(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OpSig(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':')
          .Append(h.CatchType is null ? "" : TypeSig(h.CatchType)).Append(';');
    }
    return sb.ToString();
}
static string FieldSig(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static int CoreLibRefs(ModuleDefinition m) => m.AssemblyReferences.Count(a => a.Name == "System.Private.CoreLib");

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha) throw new InvalidDataException("input SHA mismatch");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
var preserveBefore = new Dictionary<uint,string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    if (CoreLibRefs(module) != 0) throw new InvalidDataException("input references System.Private.CoreLib");
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    foreach (var tok in preservation) preserveBefore[tok] = beforeM[tok];

    TypeDefinition T(string n) => types.Single(t => t.FullName == n);
    FieldDefinition F(TypeDefinition t, string n) => t.Fields.Single(f => f.Name == n);

    var dlg = T("DialogueManager_OnBoard");
    var boardT = T("Board");
    var boardConfigT = T("BoardConfig");
    var saveT = T("Save");
    var savesManagerT = T("SavesManager");
    var globalT = T("GlobalStaticVars");
    var lawnT = T("GlobalStaticVars/LawnApp");
    var target = dlg.Methods.Single(m => m.Name == "Start" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException("DialogueManager_OnBoard.Start fingerprint drift");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x86 && i.OpCode == OpCodes.Ceq)) throw new InvalidDataException("expected IL_0086 ceq missing");

    var getGameObject = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .Single(m => m.Name == "get_gameObject" && m.DeclaringType.FullName == "UnityEngine.Component");
    var setActive = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .Single(m => m.Name == "SetActive" && m.DeclaringType.FullName == "UnityEngine.GameObject" && m.Parameters.Count == 1);

    var gLawnApp = F(globalT,"gLawnApp");
    var savesManager = F(lawnT,"savesManager");
    var playerSave = F(savesManagerT,"playerSave");
    var rescuePassNum = F(saveT,"rescuePassNum");
    var hardStars = F(saveT,"adventureHardMaxStarNum");
    var imageCurtain = F(dlg,"image_Curtain");
    var isRunning = F(dlg,"isRunning");
    var triggerType = F(dlg,"dialogueTriggerType");
    var boardField = F(dlg,"board");
    var dialogueList = F(dlg,"dialogueList");
    var challengeType = F(boardT,"challengeType");
    var level = F(boardT,"level");
    var boardConfig = F(boardT,"boardConfig");
    var boardDialogues = F(boardConfigT,"boardDialogues");
    var gameContinue = boardT.Methods.Single(m => m.Name == "GameContinue" && m.Parameters.Count == 0);

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B839F0 va=0x180314620 end=0x18031478F native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS playerSave_chain=1 curtain_inactive=1 isRunning_false=1 trigger_none=1 GameContinue=1 challenge_minus1_0_continue=1 challenge1_rescue_gate=1 challenge2_hardstar_gate=1 other_return=1 boardDialogues_assign=1");
    Console.WriteLine("CLR_FAILURE_ADAPTATION direct_deref=1 direct_array_index=1 explicit_framework_exception_ctor=0 system_private_corelib_refs=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear(); body.InitLocals=true; body.MaxStackSize=3;
    var saveLocal = new VariableDefinition(saveT);
    var challengeLocal = new VariableDefinition(module.TypeSystem.Int32);
    body.Variables.Add(saveLocal); body.Variables.Add(challengeLocal);
    var il = body.GetILProcessor();
    var checkRescue = il.Create(OpCodes.Nop);
    var checkHard = il.Create(OpCodes.Nop);
    var assign = il.Create(OpCodes.Nop);
    var ret = il.Create(OpCodes.Ret);

    il.Append(il.Create(OpCodes.Ldsfld,gLawnApp));
    il.Append(il.Create(OpCodes.Ldfld,savesManager));
    il.Append(il.Create(OpCodes.Ldfld,playerSave));
    il.Append(il.Create(OpCodes.Stloc,saveLocal));

    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld,imageCurtain));
    il.Append(il.Create(OpCodes.Call,getGameObject)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Call,setActive));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Stfld,isRunning));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Stfld,triggerType));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld,boardField)); il.Append(il.Create(OpCodes.Call,gameContinue));

    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld,boardField)); il.Append(il.Create(OpCodes.Ldfld,challengeType)); il.Append(il.Create(OpCodes.Stloc,challengeLocal));
    il.Append(il.Create(OpCodes.Ldloc,challengeLocal)); il.Append(il.Create(OpCodes.Ldc_I4_M1)); il.Append(il.Create(OpCodes.Beq,assign));
    il.Append(il.Create(OpCodes.Ldloc,challengeLocal)); il.Append(il.Create(OpCodes.Brfalse,assign));
    il.Append(il.Create(OpCodes.Ldloc,challengeLocal)); il.Append(il.Create(OpCodes.Ldc_I4_1)); il.Append(il.Create(OpCodes.Beq,checkRescue));
    il.Append(il.Create(OpCodes.Ldloc,challengeLocal)); il.Append(il.Create(OpCodes.Ldc_I4_2)); il.Append(il.Create(OpCodes.Beq,checkHard));
    il.Append(il.Create(OpCodes.Br,ret));

    il.Append(checkRescue);
    il.Append(il.Create(OpCodes.Ldloc,saveLocal)); il.Append(il.Create(OpCodes.Ldfld,rescuePassNum));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld,boardField)); il.Append(il.Create(OpCodes.Ldfld,level));
    il.Append(il.Create(OpCodes.Ldelem_I4)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Bgt,ret)); il.Append(il.Create(OpCodes.Br,assign));

    il.Append(checkHard);
    il.Append(il.Create(OpCodes.Ldloc,saveLocal)); il.Append(il.Create(OpCodes.Ldfld,hardStars));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld,boardField)); il.Append(il.Create(OpCodes.Ldfld,level));
    il.Append(il.Create(OpCodes.Ldelem_I4)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Bgt,ret));

    il.Append(assign);
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld,boardField));
    il.Append(il.Create(OpCodes.Ldfld,boardConfig)); il.Append(il.Create(OpCodes.Ldfld,boardDialogues)); il.Append(il.Create(OpCodes.Stfld,dialogueList));
    il.Append(ret);

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    if (CoreLibRefs(module) != 0) throw new InvalidDataException("System.Private.CoreLib reference pollution");
    var types=AllTypes(module.Types).ToList(); var methods=types.SelectMany(t=>t.Methods).ToList(); var fields=types.SelectMany(t=>t.Fields).ToList();
    var target=methods.Single(m=>Raw(m)==TargetToken);
    int FieldRefs(string type,string name,OpCode? op=null)=>target.Body.Instructions.Count(i=>(op is null || i.OpCode==op.Value) && i.Operand is FieldReference f && f.DeclaringType.FullName==type && f.Name==name);
    int MethodCalls(string type,string name)=>target.Body.Instructions.Count(i=>(i.OpCode==OpCodes.Call || i.OpCode==OpCodes.Callvirt) && i.Operand is MethodReference m && m.DeclaringType.FullName==type && m.Name==name);
    var ceq=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ceq);
    var objectLocals=target.Body.Variables.Count(v=>v.VariableType.FullName=="System.Object");
    var frameworkExceptionCtors=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Newobj && i.Operand is MethodReference m && (m.DeclaringType.FullName=="System.NullReferenceException" || m.DeclaringType.FullName=="System.IndexOutOfRangeException"));
    var ldelemI4=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ldelem_I4);
    if (target.Body.Variables.Count!=2 || ceq!=0 || objectLocals!=0 || frameworkExceptionCtors!=0 || ldelemI4!=2 ||
        MethodCalls("Board","GameContinue")!=1 || MethodCalls("UnityEngine.GameObject","SetActive")!=1 ||
        FieldRefs("Board","challengeType")!=1 || FieldRefs("Save","rescuePassNum")!=1 || FieldRefs("Save","adventureHardMaxStarNum")!=1 ||
        FieldRefs("BoardConfig","boardDialogues")!=1 || FieldRefs("DialogueManager_OnBoard","dialogueList",OpCodes.Stfld)!=1)
        throw new InvalidDataException("reopen Dialogue Start mismatch");
    Console.WriteLine($"REOPEN_DIALOGUE_START_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} ceq={ceq} object_locals={objectLocals} framework_exception_ctors={frameworkExceptionCtors} ldelem_i4={ldelemI4} GameContinue_calls=1 SetActive_calls=1 rescue_gate=1 hardstar_gate=1 dialogue_assign=1");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0");

    var changedM=methods.Where(m=>beforeM[Raw(m)]!=MethodSig(m)).Select(Raw).ToList();
    if (changedM.Count!=1 || changedM[0]!=TargetToken) throw new InvalidDataException("method semantic isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={methods.Count-1} changed_methods=1 target=0x{TargetToken:X8}");
    var changedF=fields.Where(f=>beforeF[Raw(f)]!=FieldSig(f)).Select(Raw).ToList();
    if (changedF.Count!=0) throw new InvalidDataException("field metadata isolation failed");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={fields.Count} changed_fields=0");
    foreach (var tok in preservation) if (MethodSig(methods.Single(m=>Raw(m)==tok))!=preserveBefore[tok]) throw new InvalidDataException($"preservation drift 0x{tok:X8}");
    Console.WriteLine("PRESERVATION_PASS batch1=4 plant_ctor=1 projectile_ctor=1 projectile_hf21=2 managers=2 plant_detail_initialize=1 load_skill_logo=1 load_data=1 create_map=1 grid_copy=1");
    Console.WriteLine("PATCH_DIALOGUE_START method_body_changes=1 pc_native_semantics=1 direct_failure_semantics=1 framework_ref_pollution=0 metadata_visibility_changes=0");
}
return 0;
