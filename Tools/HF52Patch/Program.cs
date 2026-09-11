using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF52Patch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "89e1961cefec6f8c532a0ca7cda4bd4afff2152c14f3856842ee44e5d7d7d90e";
const uint TargetToken = 0x060003CEu;
var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"FORMAL_INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256)
    throw new InvalidDataException($"HF52 formal input SHA mismatch: expected {ExpectedInputSha256}, got {inputSha}");

using var target = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true });

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

var types = AllTypes(target.Types).ToList();
var methods = types.SelectMany(t => t.Methods).ToList();
if (methods.Count != 2317)
    throw new InvalidDataException($"HF52 input MethodDef count {methods.Count}, expected 2317");

var m = target.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException($"HF52 target missing 0x{TargetToken:X8}");
if (m.DeclaringType.FullName != "Project" || m.Name != "SunSet" || m.Parameters.Count != 1 || m.GenericParameters.Count != 0 || m.Parameters[0].ParameterType.FullName != "System.Int32")
    throw new InvalidDataException($"HF52 token/signature drift: {m.FullName}");
if (!m.HasBody)
    throw new InvalidDataException("HF52 target has no managed body");

var project = m.DeclaringType;
var valueField = project.Fields.SingleOrDefault(f => f.Name == "value" && f.FieldType.FullName == "System.Int32")
    ?? throw new InvalidDataException("HF52 missing Project.value:int");
var sizeField = project.Fields.SingleOrDefault(f => f.Name == "size" && f.FieldType.FullName == "System.Single")
    ?? throw new InvalidDataException("HF52 missing Project.size:float");

var powRefs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
    .Where(x => x.DeclaringType.FullName == "System.Math" && x.Name == "Pow" && x.Parameters.Count == 2
        && x.Parameters[0].ParameterType.FullName == "System.Double"
        && x.Parameters[1].ParameterType.FullName == "System.Double"
        && x.ReturnType.FullName == "System.Double")
    .ToList();
if (powRefs.Count != 1)
    throw new InvalidDataException($"HF52 expected exactly one existing System.Math.Pow(double,double) reference, got {powRefs.Count}");
var pow = powRefs[0];

var body = new Mono.Cecil.Cil.MethodBody(m) { InitLocals = false, MaxStackSize = 4 };
m.Body = body;
var il = body.GetILProcessor();
il.Append(Instruction.Create(OpCodes.Ldarg_0));
il.Append(Instruction.Create(OpCodes.Ldarg_1));
il.Append(Instruction.Create(OpCodes.Stfld, valueField));
il.Append(Instruction.Create(OpCodes.Ldarg_0));
il.Append(Instruction.Create(OpCodes.Ldarg_1));
il.Append(Instruction.Create(OpCodes.Conv_R4));
il.Append(Instruction.Create(OpCodes.Ldc_R4, 50f));
il.Append(Instruction.Create(OpCodes.Div));
il.Append(Instruction.Create(OpCodes.Conv_R8));
il.Append(Instruction.Create(OpCodes.Ldc_R8, 0.5));
il.Append(Instruction.Create(OpCodes.Call, pow));
il.Append(Instruction.Create(OpCodes.Conv_R4));
il.Append(Instruction.Create(OpCodes.Stfld, sizeField));
il.Append(Instruction.Create(OpCodes.Ret));

Console.WriteLine($"PATCH 0x{TargetToken:X8} Project.SunSet/1 il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
target.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true });
var reopenedTypes = AllTypes(reopen.Types).ToList();
if (reopenedTypes.SelectMany(t => t.Methods).Count() != 2317)
    throw new InvalidDataException("HF52 output MethodDef count drifted from 2317");
var r = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("HF52 reopen target missing");
if (r.DeclaringType.FullName != "Project" || r.Name != "SunSet" || r.Parameters.Count != 1 || r.GenericParameters.Count != 0 || !r.HasBody || r.Body.Instructions.Count != 14)
    throw new InvalidDataException($"HF52 reopen invalid: {r.FullName} il={r.Body.Instructions.Count}");
if (r.Body.ExceptionHandlers.Count != 0)
    throw new InvalidDataException("HF52 unexpected exception handler in SunSet");
if (r.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal)))
    throw new InvalidDataException("HF52 Cpp2IL helper remained in target");
var rp = r.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().SingleOrDefault(x => x.DeclaringType.FullName == "System.Math" && x.Name == "Pow");
if (rp == null)
    throw new InvalidDataException("HF52 reopened SunSet lost Math.Pow call");
Console.WriteLine($"REOPEN 0x{TargetToken:X8} il={r.Body.Instructions.Count} bytes={r.Body.CodeSize} eh={r.Body.ExceptionHandlers.Count} gp={r.GenericParameters.Count}");
return 0;
