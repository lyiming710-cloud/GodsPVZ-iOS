using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9MapCtorPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "d5f010f3eb617f03534d9b56394dc10f14bd5e2cbbdd17baef8afc59c0cdd63b";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x06000281;

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction bi) return $"I#{owner.Body.Instructions.IndexOf(bi)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(x => owner.Body.Instructions.IndexOf(x))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}";
    if (operand is TypeReference tr) return $"T:{tr.FullName}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{vr.VariableType.FullName}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{pr.ParameterType.FullName}";
    if (operand is string s) return $"S:{Convert.ToBase64String(Encoding.UTF8.GetBytes(s))}";
    return $"C:{Convert.ToString(operand, System.Globalization.CultureInfo.InvariantCulture)}";
}
static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
    return sb.ToString();
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");

var target = methods.SingleOrDefault(m => m.DeclaringType.FullName == "Map" && m.Name == ".ctor" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.FullName == "System.Int32" && m.Parameters[1].ParameterType.FullName == "System.Int32")
    ?? throw new InvalidDataException("Map::.ctor(int,int) target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (target.Body.Variables.Count != 15) throw new InvalidDataException($"Map ctor local count drift: {target.Body.Variables.Count}");
if (!target.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldfld && i.Operand is FieldReference f && f.FullName.Contains("System.Collections.Generic.List`1<Row>::_size"))) throw new InvalidDataException("expected corrupt List<Row>._size access missing");
if (!target.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldstr && Convert.ToString(i.Operand) == "Method not found @180302170")) throw new InvalidDataException("expected Cpp2IL base-ctor placeholder missing");

var allOperands = methods.Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).ToList();
var objectCtor = allOperands.OfType<MethodReference>().FirstOrDefault(m => m.FullName == "System.Void System.Object::.ctor()")
    ?? throw new InvalidDataException("System.Object::.ctor MethodRef missing");
var listCtor = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().FirstOrDefault(m => m.FullName == "System.Void System.Collections.Generic.List`1<Row>::.ctor()")
    ?? throw new InvalidDataException("List<Row> ctor MethodRef missing");
var rowCtor = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().FirstOrDefault(m => m.FullName == "System.Void Row::.ctor(System.Int32,System.Int32)")
    ?? throw new InvalidDataException("Row(int,int) ctor MethodRef missing");
var rowsField = target.DeclaringType.Fields.SingleOrDefault(f => f.Name == "rows" && f.FieldType.FullName == "System.Collections.Generic.List`1<Row>")
    ?? throw new InvalidDataException("Map.rows field missing");
var rowType = module.Types.SingleOrDefault(t => t.FullName == "Row") ?? throw new InvalidDataException("Row type missing");
var listRowType = listCtor.DeclaringType;
var getCount = new MethodReference("get_Count", module.TypeSystem.Int32, listRowType) { HasThis = true };
var add = new MethodReference("Add", module.TypeSystem.Void, listRowType) { HasThis = true };
add.Parameters.Add(new ParameterDefinition(rowType));

var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var body = target.Body;
body.Instructions.Clear();
body.ExceptionHandlers.Clear();
body.Variables.Clear();
body.InitLocals = false;
body.MaxStackSize = 4;
var il = body.GetILProcessor();

// Native 0x18032D290: base .ctor, allocate List<Row>, assign rows, then append Row(mapX, rows.Count) until rows.Count >= mapY.
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Call, objectCtor));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Newobj, listCtor));
il.Append(il.Create(OpCodes.Stfld, rowsField));

var check = il.Create(OpCodes.Ldarg_0);
var done = il.Create(OpCodes.Ret);
il.Append(check);
il.Append(il.Create(OpCodes.Ldfld, rowsField));
il.Append(il.Create(OpCodes.Callvirt, getCount));
il.Append(il.Create(OpCodes.Ldarg_2));
il.Append(il.Create(OpCodes.Bge, done));

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, rowsField));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, rowsField));
il.Append(il.Create(OpCodes.Callvirt, getCount));
il.Append(il.Create(OpCodes.Newobj, rowCtor));
il.Append(il.Create(OpCodes.Callvirt, add));
il.Append(il.Create(OpCodes.Br, check));
il.Append(done);

Console.WriteLine("NATIVE_AUTHORITY token=0x06000281 address=0x000000018032D290 function_end=0x000000018032D3FB gameassembly_sha256=9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d metadata_sha256=ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9");
Console.WriteLine("PATCH_MAP_CTOR rows=new List<Row>(); while(rows.Count<mapY) rows.Add(new Row(mapX,rows.Count)); private_list_fields=0");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
if (rt.Body.Variables.Count != 0) throw new InvalidDataException("Map ctor repaired locals drift");
if (rt.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1") && (f.Name == "_size" || f.Name == "_version" || f.Name == "_items"))) throw new InvalidDataException("private List<T> field access survived repair");
if (rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldstr && Convert.ToString(i.Operand)!.StartsWith("Method not found @"))) throw new InvalidDataException("Cpp2IL method placeholder survived repair");
if (rt.Body.Instructions.Count(i => i.OpCode.Code == Code.Callvirt && i.Operand is MethodReference m && m.Name == "get_Count") != 2) throw new InvalidDataException("expected two List<Row>.Count calls");
if (rt.Body.Instructions.Count(i => i.OpCode.Code == Code.Callvirt && i.Operand is MethodReference m && m.Name == "Add") != 1) throw new InvalidDataException("expected one List<Row>.Add call");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Newobj && i.Operand is MethodReference m && m.FullName == "System.Void Row::.ctor(System.Int32,System.Int32)")) throw new InvalidDataException("Row ctor missing after repair");
Console.WriteLine($"REOPEN_MAP_CTOR_PASS locals=0 count_calls=2 add_calls=1 private_list_fields=0 token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside Map::.ctor: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
