using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9RowCtorPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "51407d61b6850d73fe56cbe8f4bb3a264b5b05ac1211a7e60a712f34454988c4";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x06000292;

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

var target = methods.SingleOrDefault(m => m.DeclaringType.FullName == "Row" && m.Name == ".ctor" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.FullName == "System.Int32" && m.Parameters[1].ParameterType.FullName == "System.Int32")
    ?? throw new InvalidDataException("Row::.ctor(int,int) target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (!target.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1") && (f.Name == "_size" || f.Name == "_version" || f.Name == "_items")))
    throw new InvalidDataException("expected corrupt private List<T> access missing");

var allOperands = methods.Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).ToList();
var objectCtor = allOperands.OfType<MethodReference>().FirstOrDefault(m => m.FullName == "System.Void System.Object::.ctor()")
    ?? throw new InvalidDataException("System.Object::.ctor MethodRef missing");
var listCtor = allOperands.OfType<MethodReference>().FirstOrDefault(m => m.Name == ".ctor" && m.Parameters.Count == 0 && m.DeclaringType.FullName == "System.Collections.Generic.List`1<Grid>")
    ?? throw new InvalidDataException("List<Grid> ctor MethodRef missing");
var getCount = allOperands.OfType<MethodReference>().FirstOrDefault(m => m.Name == "get_Count" && m.Parameters.Count == 0 && m.DeclaringType.FullName == "System.Collections.Generic.List`1<Grid>" && m.ReturnType.FullName == "System.Int32")
    ?? throw new InvalidDataException("List<Grid>.get_Count MethodRef missing");
var add = allOperands.OfType<MethodReference>().FirstOrDefault(m => m.Name == "Add" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<Grid>" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType is GenericParameter gp && gp.Type == GenericParameterType.Type && gp.Position == 0)
    ?? throw new InvalidDataException("existing valid List<Grid>.Add(!0) MethodRef missing");
var gridCtor = allOperands.OfType<MethodReference>().FirstOrDefault(m => m.FullName == "System.Void Grid::.ctor(System.Int32,System.Int32)")
    ?? throw new InvalidDataException("Grid(int,int) ctor MethodRef missing");

var rowType = target.DeclaringType;
var gridType = module.Types.SingleOrDefault(t => t.FullName == "Grid") ?? throw new InvalidDataException("Grid type missing");
FieldDefinition RF(TypeDefinition t, string n) => t.Fields.SingleOrDefault(f => f.Name == n) ?? throw new InvalidDataException($"field {t.FullName}.{n} missing");
var rowGridY = RF(rowType, "gridY");
var rowGrids = RF(rowType, "grids");
var rowEnemyType = RF(rowType, "EnemyType");
var gridState = RF(gridType, "gridState");
var plantBottom = RF(gridType, "plant_bottom");
var plantCommon = RF(gridType, "plant_common");
var plantSheath = RF(gridType, "plant_sheath");
var plantTop = RF(gridType, "plant_top");
var gridX = RF(gridType, "gridX");
var gridY = RF(gridType, "gridY");

var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var body = target.Body;
body.Instructions.Clear();
body.ExceptionHandlers.Clear();
body.Variables.Clear();
body.InitLocals = true;
body.MaxStackSize = 3;
var idx = new VariableDefinition(module.TypeSystem.Int32);
var grid = new VariableDefinition(gridType);
body.Variables.Add(idx);
body.Variables.Add(grid);
var il = body.GetILProcessor();

// PC native 0x18033DEB0: base ctor; gridY=arg; grids=new List<Grid>(); while Count<mapX create/configure Grid and append; EnemyType=0.
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Call, objectCtor));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldarg_2));
il.Append(il.Create(OpCodes.Stfld, rowGridY));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Newobj, listCtor));
il.Append(il.Create(OpCodes.Stfld, rowGrids));

var check = il.Create(OpCodes.Ldarg_0);
var done = il.Create(OpCodes.Ldarg_0);
il.Append(check);
il.Append(il.Create(OpCodes.Ldfld, rowGrids));
il.Append(il.Create(OpCodes.Callvirt, getCount));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Bge, done));

// idx = grids.Count; grid = new Grid(0, idx)
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, rowGrids));
il.Append(il.Create(OpCodes.Callvirt, getCount));
il.Append(il.Create(OpCodes.Stloc, idx));
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Ldloc, idx));
il.Append(il.Create(OpCodes.Newobj, gridCtor));
il.Append(il.Create(OpCodes.Stloc, grid));

// Native explicitly initializes these Grid fields before insertion.
il.Append(il.Create(OpCodes.Ldloc, grid)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Stfld, gridState));
il.Append(il.Create(OpCodes.Ldloc, grid)); il.Append(il.Create(OpCodes.Ldnull)); il.Append(il.Create(OpCodes.Stfld, plantBottom));
il.Append(il.Create(OpCodes.Ldloc, grid)); il.Append(il.Create(OpCodes.Ldnull)); il.Append(il.Create(OpCodes.Stfld, plantCommon));
il.Append(il.Create(OpCodes.Ldloc, grid)); il.Append(il.Create(OpCodes.Ldnull)); il.Append(il.Create(OpCodes.Stfld, plantSheath));
il.Append(il.Create(OpCodes.Ldloc, grid)); il.Append(il.Create(OpCodes.Ldnull)); il.Append(il.Create(OpCodes.Stfld, plantTop));
il.Append(il.Create(OpCodes.Ldloc, grid)); il.Append(il.Create(OpCodes.Ldloc, idx)); il.Append(il.Create(OpCodes.Stfld, gridX));
il.Append(il.Create(OpCodes.Ldloc, grid)); il.Append(il.Create(OpCodes.Ldarg_2)); il.Append(il.Create(OpCodes.Stfld, gridY));

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, rowGrids));
il.Append(il.Create(OpCodes.Ldloc, grid));
il.Append(il.Create(OpCodes.Callvirt, add));
il.Append(il.Create(OpCodes.Br, check));

il.Append(done);
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Stfld, rowEnemyType));
il.Append(il.Create(OpCodes.Ret));

Console.WriteLine("NATIVE_AUTHORITY token=0x06000292 address=0x000000018033DEB0 function_end=0x000000018033E075 gameassembly_sha256=9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d metadata_sha256=ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9");
Console.WriteLine("PATCH_ROW_CTOR gridY=arg; grids=new List<Grid>(); while(Count<mapX){idx=Count; Grid(0,idx); state=Null; plants=null; gridX=idx; gridY=arg; Add(!0);} EnemyType=0 private_list_fields=0");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
if (rt.Body.Variables.Count != 2 || rt.Body.Variables[0].VariableType.FullName != "System.Int32" || rt.Body.Variables[1].VariableType.FullName != "Grid") throw new InvalidDataException("Row ctor repaired locals drift");
if (rt.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1") && (f.Name == "_size" || f.Name == "_version" || f.Name == "_items"))) throw new InvalidDataException("private List<T> field access survived repair");
var addCalls = rt.Body.Instructions.Where(i => i.OpCode.Code == Code.Callvirt && i.Operand is MethodReference m && m.Name == "Add").Select(i => (MethodReference)i.Operand).ToList();
if (addCalls.Count != 1 || addCalls[0].Parameters.Count != 1 || addCalls[0].Parameters[0].ParameterType is not GenericParameter agp || agp.Type != GenericParameterType.Type || agp.Position != 0) throw new InvalidDataException("Row ctor Add call is not List<Grid>.Add(!0)");
if (rt.Body.Instructions.Count(i => i.OpCode.Code == Code.Callvirt && i.Operand is MethodReference m && m.Name == "get_Count") != 2) throw new InvalidDataException("expected two List<Grid>.Count calls");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Newobj && i.Operand is MethodReference m && m.FullName == "System.Void Grid::.ctor(System.Int32,System.Int32)")) throw new InvalidDataException("Grid ctor missing after repair");
foreach (var fn in new[] { "gridState", "plant_bottom", "plant_common", "plant_sheath", "plant_top", "gridX", "gridY" })
    if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Stfld && i.Operand is FieldReference f && f.DeclaringType.FullName == "Grid" && f.Name == fn)) throw new InvalidDataException($"Grid.{fn} store missing");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Stfld && i.Operand is FieldReference f && f.DeclaringType.FullName == "Row" && f.Name == "EnemyType")) throw new InvalidDataException("Row.EnemyType store missing");
Console.WriteLine($"REOPEN_ROW_CTOR_PASS locals=2 count_calls=2 add_signature=!0 grid_ctor=1 private_list_fields=0 token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside Row::.ctor: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
