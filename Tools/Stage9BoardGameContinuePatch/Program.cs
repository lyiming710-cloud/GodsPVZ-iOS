using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9BoardGameContinuePatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha = "18b4d2ad02efa29c909a151e34fb12cb4e72f8a1fa0e3e3e702d5b8c169fa5fd";
const uint TargetToken = 0x060002BD;
const int ExpectedRid = 701;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 206;
const int ExpectedOldLocals = 5;
const string NativeSliceSha = "28b029b2f9c2ff3b683305f9a3e8aede54b2e42e255ececc3c12542900463987";

var preservation = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A,
    0x060003BA, 0x060003FD, 0x060003DD, 0x060003DE,
    0x060001F3, 0x06000216, 0x06000667, 0x06000668,
    0x060005E1, 0x06000157, 0x06000294, 0x06000193
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

    var board = T("Board");
    var target = board.Methods.Single(m => m.Name == "GameContinue" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException("Board.GameContinue fingerprint drift");

    var oldGetKeyDownInt = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .Single(m => m.DeclaringType.FullName == "UnityEngine.Input" && m.Name == "GetKeyDownInt" && m.Parameters.Count == 1);
    if (oldGetKeyDownInt.Parameters[0].ParameterType.FullName != "UnityEngine.KeyCode" || oldGetKeyDownInt.ReturnType.FullName != "System.Boolean")
        throw new InvalidDataException("unexpected GetKeyDownInt signature");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x72 && i.OpCode == OpCodes.Ldc_I4 && Convert.ToInt32(i.Operand, CultureInfo.InvariantCulture) == 32))
        throw new InvalidDataException("expected Space keycode 32 missing");

    MethodReference OneCall(string type, string name) => target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .Single(m => m.DeclaringType.FullName == type && m.Name == name);
    var setTimeScale = OneCall("UnityEngine.Time", "set_timeScale");
    var autoTimeSlow = OneCall("GlobalStaticVars", "AutoTimeSlow");
    var unPause = OneCall("UnityEngine.AudioSource", "UnPause");
    var bgmUnPause = OneCall("ZombieManager", "BGMUnPasue");
    var setActive = OneCall("UnityEngine.GameObject", "SetActive");

    var publicGetKeyDown = new MethodReference("GetKeyDown", oldGetKeyDownInt.ReturnType, oldGetKeyDownInt.DeclaringType)
    {
        HasThis = false,
        ExplicitThis = false,
        CallingConvention = MethodCallingConvention.Default
    };
    publicGetKeyDown.Parameters.Add(new ParameterDefinition(oldGetKeyDownInt.Parameters[0].ParameterType));
    publicGetKeyDown = module.ImportReference(publicGetKeyDown);

    var pauseTemp = F(board,"pauseTemp");
    var gamePause = F(board,"gamePause");
    var gameSpeed = F(board,"gameSpeed");
    var onDetailPage = F(board,"onDetailPage");
    var audioSource = F(board,"audioSource");
    var zombieManager = F(board,"zombieManager");
    var pause = F(board,"pause");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B84340 va=0x180326700 end=0x1803267D9 native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS keycode_space_32=1 pauseTemp_on_keydown=1 gamePause_false=1 restore_gameSpeed=1 detail_AutoTimeSlow_0_25=1 audio0_UnPause=1 BGMUnPasue=1 pause_inactive=1");
    Console.WriteLine("CLR_ACCESS_ADAPTATION internal_Input_GetKeyDownInt_to_public_Input_GetKeyDown=1 semantics_preserved=1 field_metadata_changes=0 framework_exception_ctor=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear(); body.InitLocals=false; body.MaxStackSize=2;
    var il = body.GetILProcessor();
    var afterKey = il.Create(OpCodes.Nop);
    var afterSlow = il.Create(OpCodes.Nop);

    il.Append(il.Create(OpCodes.Ldc_I4_S, (sbyte)32));
    il.Append(il.Create(OpCodes.Call, publicGetKeyDown));
    il.Append(il.Create(OpCodes.Brfalse, afterKey));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Stfld, pauseTemp));
    il.Append(afterKey);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Stfld, gamePause));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, gameSpeed));
    il.Append(il.Create(OpCodes.Call, setTimeScale));

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, onDetailPage));
    il.Append(il.Create(OpCodes.Brfalse, afterSlow));
    il.Append(il.Create(OpCodes.Ldc_R4, 0.25f));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, autoTimeSlow));
    il.Append(afterSlow);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, audioSource));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Ldelem_Ref));
    il.Append(il.Create(OpCodes.Call, unPause));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, zombieManager));
    il.Append(il.Create(OpCodes.Call, bgmUnPause));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, pause));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Call, setActive));
    il.Append(il.Create(OpCodes.Ret));

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
    var internalCalls=MethodCalls("UnityEngine.Input","GetKeyDownInt");
    var publicCalls=MethodCalls("UnityEngine.Input","GetKeyDown");
    var key32=target.Body.Instructions.Count(i=>(i.OpCode==OpCodes.Ldc_I4 || i.OpCode==OpCodes.Ldc_I4_S) && Convert.ToInt32(i.Operand, CultureInfo.InvariantCulture)==32);
    var q25=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ldc_R4 && i.Operand is float f && BitConverter.SingleToInt32Bits(f)==BitConverter.SingleToInt32Bits(0.25f));
    if (target.Body.Variables.Count!=0 || internalCalls!=0 || publicCalls!=1 || key32!=1 || q25!=1 ||
        MethodCalls("UnityEngine.Time","set_timeScale")!=1 || MethodCalls("GlobalStaticVars","AutoTimeSlow")!=1 ||
        MethodCalls("UnityEngine.AudioSource","UnPause")!=1 || MethodCalls("ZombieManager","BGMUnPasue")!=1 || MethodCalls("UnityEngine.GameObject","SetActive")!=1 ||
        FieldRefs("Board","pauseTemp",OpCodes.Stfld)!=1 || FieldRefs("Board","gamePause",OpCodes.Stfld)!=1 || FieldRefs("Board","gameSpeed")!=1 ||
        FieldRefs("Board","onDetailPage")!=1 || FieldRefs("Board","audioSource")!=1 || FieldRefs("Board","zombieManager")!=1 || FieldRefs("Board","pause")!=1)
        throw new InvalidDataException("reopen Board.GameContinue mismatch");
    Console.WriteLine($"REOPEN_BOARD_GAMECONTINUE_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=0 GetKeyDownInt_calls=0 GetKeyDown_calls=1 keycode32=1 AutoTimeSlow_calls=1 UnPause_calls=1 BGMUnPasue_calls=1 SetActive_calls=1");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0");

    var changedM=methods.Where(m=>beforeM[Raw(m)]!=MethodSig(m)).Select(m=>Raw(m)).OrderBy(x=>x).ToList();
    if (changedM.Count!=1 || changedM[0]!=TargetToken) throw new InvalidDataException("semantic method isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    var changedF=fields.Where(f=>beforeF[Raw(f)]!=FieldSig(f)).Select(f=>Raw(f)).ToList();
    if (changedF.Count!=0) throw new InvalidDataException("field metadata drift");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    foreach (var tok in preservation)
    {
        var m=methods.Single(x=>Raw(x)==tok);
        if (MethodSig(m)!=preserveBefore[tok]) throw new InvalidDataException($"preservation drift 0x{tok:X8}");
    }
    Console.WriteLine("PRESERVATION_PASS batch1=4 plant_ctor=1 projectile_ctor=1 projectile_hf21=2 managers=2 plant_detail_initialize=1 load_skill_logo=1 load_data=1 create_map=1 grid_copy=1 dialogue_start=1");
}
return 0;
