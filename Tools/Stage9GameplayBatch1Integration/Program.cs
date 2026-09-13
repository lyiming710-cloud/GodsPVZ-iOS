using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9GameplayBatch1Integration <map-candidate.dll> <base-1c62.dll> <output.dll>");
    return 2;
}

const string ExpectedMapSha = "6fb26689d20362531c5ccabbe89a8b90f8b80af9381b4581e95263b2692e0305";
const string ExpectedBaseSha = "1c62f969409ed16c9d7c7855f7b52e6b593701524e2b2d75ec0affbc3d05b209";
const int ExpectedMethodCount = 2317;
const int ExpectedFieldCount = 2802;
const uint GameStartToken = 0x0600067E;
const uint GetterToken = 0x06000169;
static readonly HashSet<uint> MapTokens = new() { 0x06000285, 0x06000289, 0x0600028A };
static readonly HashSet<uint> FinalTokens = new() { GameStartToken, 0x06000285, 0x06000289, 0x0600028A };

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha256(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static string ScopeName(TypeReference t) => t.Scope switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => t.Scope?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{ScopeName(t)}";
static string OperandSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return $"M:{mr.FullName}@{ScopeName(mr.DeclaringType)}";
    if (o is FieldReference fr) return $"F:{fr.FullName}@{ScopeName(fr.DeclaringType)}";
    if (o is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (o is VariableDefinition vr) return $"V:{vr.Index}:{TypeSig(vr.VariableType)}";
    if (o is ParameterDefinition pr) return $"P:{pr.Index}:{TypeSig(pr.ParameterType)}";
    if (o is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (o is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (o is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(o, CultureInfo.InvariantCulture);
}
static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSig(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':')
          .Append(h.CatchType is null ? "" : TypeSig(h.CatchType)).Append(';');
    }
    return sb.ToString();
}
static string FieldSemantic(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static int CountCalls(MethodDefinition m, Func<MethodReference, bool> pred) =>
    m.Body.Instructions.Count(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference mr && pred(mr));
static int PrivateListFieldRefs(MethodDefinition m) => m.HasBody
    ? m.Body.Instructions.Count(i => i.Operand is FieldReference fr &&
        fr.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal) &&
        (fr.Name == "_size" || fr.Name == "_version" || fr.Name == "_items"))
    : 0;

var mapInput = Path.GetFullPath(args[0]);
var baseInput = Path.GetFullPath(args[1]);
var output = Path.GetFullPath(args[2]);
var mapSha = Sha256(mapInput); var baseSha = Sha256(baseInput);
Console.WriteLine($"MAP_INPUT_SHA256 {mapSha}");
Console.WriteLine($"BASE_INPUT_SHA256 {baseSha}");
if (mapSha != ExpectedMapSha) throw new InvalidDataException("map candidate SHA mismatch");
if (baseSha != ExpectedBaseSha) throw new InvalidDataException("base SHA mismatch");

var baseM = new Dictionary<uint, string>();
var baseF = new Dictionary<uint, string>();
using (var b = ModuleDefinition.ReadModule(baseInput, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(b.Types).ToList(); var methods = types.SelectMany(t => t.Methods).ToList(); var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodCount || fields.Count != ExpectedFieldCount) throw new InvalidDataException("base metadata count drift");
    foreach (var m in methods) baseM[Raw(m)] = MethodSemantic(m);
    foreach (var f in fields) baseF[Raw(f)] = FieldSemantic(f);
}

using (var module = ModuleDefinition.ReadModule(mapInput, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList(); var methods = types.SelectMany(t => t.Methods).ToList(); var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodCount || fields.Count != ExpectedFieldCount) throw new InvalidDataException("map metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("map input unexpectedly has System.Private.CoreLib");

    var mapChanged = methods.Where(m => !baseM.TryGetValue(Raw(m), out var old) || old != MethodSemantic(m)).Select(Raw).ToHashSet();
    if (!mapChanged.SetEquals(MapTokens)) throw new InvalidDataException("map candidate isolation mismatch: " + string.Join(',', mapChanged.Select(x => $"0x{x:X8}")));
    foreach (var f in fields) if (!baseF.TryGetValue(Raw(f), out var old) || old != FieldSemantic(f)) throw new InvalidDataException($"map candidate field drift 0x{Raw(f):X8}");
    Console.WriteLine("PREINTEGRATION_MAP_ISOLATION_PASS changed_methods=3 tokens=0x06000285,0x06000289,0x0600028A fields_unchanged=2802");

    var p = types.Single(t => t.FullName == "PrepareUIController");
    var target = p.Methods.Single(m => m.Name == "GameStart" && m.Parameters.Count == 0 && !m.IsStatic);
    if (Raw(target) != GameStartToken || target.Body.CodeSize != 255 || target.Body.Variables.Count != 11)
        throw new InvalidDataException($"GameStart fingerprint mismatch token=0x{Raw(target):X8} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    var bm = types.Single(t => t.FullName == "BoardManager");
    var getter = bm.Methods.Single(m => m.Name == "get_Instance" && m.IsStatic && m.Parameters.Count == 0);
    if (Raw(getter) != GetterToken) throw new InvalidDataException($"getter token drift 0x{Raw(getter):X8}");
    var backing = bm.Fields.Single(f => f.Name == "<Instance>k__BackingField");
    var bad = target.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldsfld && i.Operand is FieldReference fr && fr.Name == backing.Name && fr.DeclaringType.FullName == "BoardManager").ToList();
    if (bad.Count != 1) throw new InvalidDataException($"expected one GameStart backing-field read, got {bad.Count}");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "BoardManager" && mr.Name == "get_Instance") != 0) throw new InvalidDataException("GameStart getter already present");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "BoardManager" && mr.Name == "LoadBoard") != 1) throw new InvalidDataException("GameStart LoadBoard fingerprint drift");
    bad[0].OpCode = OpCodes.Call; bad[0].Operand = getter;
    Console.WriteLine("INTEGRATE_GAMESTART_PASS target_token=0x0600067E replacement=private_backing_read_to_get_Instance loadboard_calls=1");
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha256(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList(); var methods = types.SelectMany(t => t.Methods).ToList(); var fields = types.SelectMany(t => t.Fields).ToList();
    var changed = methods.Where(m => !baseM.TryGetValue(Raw(m), out var old) || old != MethodSemantic(m)).Select(Raw).ToHashSet();
    if (!changed.SetEquals(FinalTokens)) throw new InvalidDataException("final integration isolation mismatch: " + string.Join(',', changed.Select(x => $"0x{x:X8}")));
    foreach (var f in fields) if (!baseF.TryGetValue(Raw(f), out var old) || old != FieldSemantic(f)) throw new InvalidDataException($"final field drift 0x{Raw(f):X8}");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib introduced");

    var gs = types.Single(t => t.FullName == "PrepareUIController").Methods.Single(m => Raw(m) == GameStartToken);
    var backingRefs = gs.Body.Instructions.Count(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName == "BoardManager" && fr.Name == "<Instance>k__BackingField");
    var getterCalls = CountCalls(gs, mr => mr.DeclaringType.FullName == "BoardManager" && mr.Name == "get_Instance");
    var loadCalls = CountCalls(gs, mr => mr.DeclaringType.FullName == "BoardManager" && mr.Name == "LoadBoard");
    if (backingRefs != 0 || getterCalls != 1 || loadCalls != 1) throw new InvalidDataException("GameStart post-integration semantics mismatch");

    var map = types.Single(t => t.FullName == "Map");
    foreach (var tok in MapTokens)
    {
        var m = map.Methods.Single(x => Raw(x) == tok);
        if (PrivateListFieldRefs(m) != 0) throw new InvalidDataException($"Map private List field refs remain in 0x{tok:X8}");
    }
    var copy = map.Methods.Single(x => Raw(x) == 0x06000285);
    if (CountCalls(copy, mr => mr.DeclaringType.FullName == "Map" && mr.Name == "GetMapX") != 1) throw new InvalidDataException("Map.Copy GetMapX call drift");
    if (CountCalls(copy, mr => mr.DeclaringType.FullName == "Grid" && mr.Name == "Copy") != 1) throw new InvalidDataException("Map.Copy Grid.Copy call drift");

    Console.WriteLine("GAMEPLAY_BATCH1_REOPEN_PASS changed_methods=4 tokens=0x0600067E,0x06000285,0x06000289,0x0600028A fields_unchanged=2802 corelib_refs=0");
    Console.WriteLine("GAMEPLAY_BATCH1_SEMANTICS_PASS gamestart_getter_calls=1 gamestart_loadboard_calls=1 map_private_list_refs=0 map_copy_getx_calls=1 map_copy_grid_copy_calls=1");
}
return 0;
