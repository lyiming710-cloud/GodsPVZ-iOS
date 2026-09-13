using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Text;

if (args.Length != 4)
{
    Console.Error.WriteLine("Usage: Stage9FourMethodIntegrationVerify <base-1c62.dll> <gamestart-green.dll> <map-green.dll> <integrated.dll>");
    return 2;
}

const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const uint GameStartToken = 0x0600067E;
const uint MapCopyToken = 0x06000285;
const uint MapGetXToken = 0x06000289;
const uint MapGetYToken = 0x0600028A;
var targets = new HashSet<uint> { GameStartToken, MapCopyToken, MapGetXToken, MapGetYToken };

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
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
static Dictionary<uint, MethodDefinition> Methods(ModuleDefinition m) => AllTypes(m.Types).SelectMany(t => t.Methods).ToDictionary(Raw);
static Dictionary<uint, FieldDefinition> Fields(ModuleDefinition m) => AllTypes(m.Types).SelectMany(t => t.Fields).ToDictionary(Raw);
static int CountCalls(MethodDefinition m, Func<MethodReference, bool> p) => m.Body.Instructions.Count(i =>
    (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj) && i.Operand is MethodReference mr && p(mr));
static int CountListPrivate(MethodDefinition m) => m.Body.Instructions.Count(i =>
    i.Operand is FieldReference fr && fr.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal) &&
    (fr.Name == "_size" || fr.Name == "_version" || fr.Name == "_items"));

using var baseline = ModuleDefinition.ReadModule(Path.GetFullPath(args[0]), new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
using var game = ModuleDefinition.ReadModule(Path.GetFullPath(args[1]), new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
using var map = ModuleDefinition.ReadModule(Path.GetFullPath(args[2]), new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
using var integrated = ModuleDefinition.ReadModule(Path.GetFullPath(args[3]), new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });

var bm = Methods(baseline); var gm = Methods(game); var mm = Methods(map); var im = Methods(integrated);
var bf = Fields(baseline); var inf = Fields(integrated);
foreach (var x in new[] { bm.Count, gm.Count, mm.Count, im.Count }) if (x != ExpectedMethods) throw new InvalidDataException($"MethodDef count drift: {x}");
foreach (var x in new[] { bf.Count, inf.Count }) if (x != ExpectedFields) throw new InvalidDataException($"FieldDef count drift: {x}");
if (integrated.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib introduced");

var changed = new List<uint>();
foreach (var kv in bm)
{
    if (!im.TryGetValue(kv.Key, out var after)) throw new InvalidDataException($"missing integrated method 0x{kv.Key:X8}");
    if (MethodSemantic(kv.Value) != MethodSemantic(after)) changed.Add(kv.Key);
}
changed.Sort();
var expected = targets.OrderBy(x => x).ToArray();
if (!changed.SequenceEqual(expected)) throw new InvalidDataException("integrated method diff mismatch: " + string.Join(',', changed.Select(x => $"0x{x:X8}")));

if (MethodSemantic(im[GameStartToken]) != MethodSemantic(gm[GameStartToken])) throw new InvalidDataException("GameStart integrated body differs from standalone green candidate");
foreach (var tok in new[] { MapCopyToken, MapGetXToken, MapGetYToken })
    if (MethodSemantic(im[tok]) != MethodSemantic(mm[tok])) throw new InvalidDataException($"Map integrated body differs from standalone green candidate: 0x{tok:X8}");

foreach (var kv in bf)
{
    if (!inf.TryGetValue(kv.Key, out var after) || FieldSemantic(kv.Value) != FieldSemantic(after))
        throw new InvalidDataException($"field drift: 0x{kv.Key:X8} {kv.Value.FullName}");
}

var gs = im[GameStartToken];
if (CountCalls(gs, mr => mr.DeclaringType.FullName == "BoardManager" && mr.Name == "get_Instance") != 1 ||
    CountCalls(gs, mr => mr.DeclaringType.FullName == "BoardManager" && mr.Name == "LoadBoard") != 1)
    throw new InvalidDataException("GameStart call shape drift");
if (gs.Body.Instructions.Any(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName == "BoardManager" && fr.Name == "<Instance>k__BackingField"))
    throw new InvalidDataException("GameStart private backing field read survived");

foreach (var tok in new[] { MapCopyToken, MapGetXToken, MapGetYToken })
    if (CountListPrivate(im[tok]) != 0) throw new InvalidDataException($"List private field ref survived in 0x{tok:X8}");
if (CountCalls(im[MapCopyToken], mr => mr.DeclaringType.FullName == "Map" && mr.Name == "GetMapX") != 1 ||
    CountCalls(im[MapCopyToken], mr => mr.DeclaringType.FullName == "Grid" && mr.Name == "Copy") != 1)
    throw new InvalidDataException("Map.Copy call shape drift");

Console.WriteLine("FOUR_METHOD_INTEGRATION_PASS changed_methods=4 untouched_methods=2313 tokens=0x06000285,0x06000289,0x0600028A,0x0600067E standalone_semantics_exact=1");
Console.WriteLine("FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0");
Console.WriteLine("INTEGRATION_SEMANTICS_PASS gamestart_getter_calls=1 gamestart_loadboard_calls=1 map_private_list_refs=0 corelib_refs=0");
return 0;
