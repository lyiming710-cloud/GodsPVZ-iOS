using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF40Patch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e";
const uint TargetToken = 0x0600012B;
var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"FORMAL_INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256)
    throw new InvalidDataException($"HF40 formal input SHA mismatch: expected {ExpectedInputSha256}, got {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });
var method = module.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("HF40 target token missing");
if (method.DeclaringType.FullName != "ElementUIController" || method.Name != "Start" || method.Parameters.Count != 0)
    throw new InvalidDataException($"HF40 token drift: {method.FullName}");
if (!method.HasBody) throw new InvalidDataException("HF40 target has no body");

var type = method.DeclaringType;
FieldDefinition F(string name) => type.Fields.Single(f => f.Name == name);
var progress = F("progress");
var images = new[] { F("back1"), F("image1"), F("back2"), F("image2") };

var old = method.Body.Instructions;
var getGameObject = old.Select(i => i.Operand).OfType<MethodReference>()
    .FirstOrDefault(m => m.Name == "get_gameObject" && m.DeclaringType.FullName == "UnityEngine.Component")
    ?? throw new InvalidDataException("HF40 could not resolve Component.get_gameObject from formal input");
var setActive = old.Select(i => i.Operand).OfType<MethodReference>()
    .FirstOrDefault(m => m.Name == "SetActive" && m.DeclaringType.FullName == "UnityEngine.GameObject" && m.Parameters.Count == 1)
    ?? throw new InvalidDataException("HF40 could not resolve GameObject.SetActive from formal input");

var body = new MethodBody(method) { InitLocals = false, MaxStackSize = 2 };
method.Body = body;
var il = body.GetILProcessor();
var ret = Instruction.Create(OpCodes.Ret);

// PC native UCOMISS + JP/JNE: unordered (NaN) and nonzero return; only ordered +/-0 continues.
il.Append(Instruction.Create(OpCodes.Ldarg_0));
il.Append(Instruction.Create(OpCodes.Ldfld, progress));
il.Append(Instruction.Create(OpCodes.Ldc_R4, 0f));
il.Append(Instruction.Create(OpCodes.Bne_Un, ret));

foreach (var field in images)
{
    var next = Instruction.Create(OpCodes.Nop);
    il.Append(Instruction.Create(OpCodes.Ldarg_0));
    il.Append(Instruction.Create(OpCodes.Ldfld, field));
    il.Append(Instruction.Create(OpCodes.Brfalse, next));
    il.Append(Instruction.Create(OpCodes.Ldarg_0));
    il.Append(Instruction.Create(OpCodes.Ldfld, field));
    il.Append(Instruction.Create(OpCodes.Callvirt, getGameObject));
    il.Append(Instruction.Create(OpCodes.Ldc_I4_0));
    il.Append(Instruction.Create(OpCodes.Callvirt, setActive));
    il.Append(next);
}
il.Append(ret);

Console.WriteLine($"PATCH 0x{TargetToken:X8} {method.DeclaringType.FullName}.{method.Name}/0 il={method.Body.Instructions.Count} bytes={method.Body.CodeSize} eh={method.Body.ExceptionHandlers.Count}");
module.Write(output);

using (var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var m = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException("HF40 reopen target missing");
    if (!m.HasBody || m.Body.Instructions.Count < 30) throw new InvalidDataException("HF40 reopen body too small");
    if (m.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("HF40 unexpected EH");
    if (m.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal)))
        throw new InvalidDataException("HF40 Cpp2IL helper remained");
    if (m.Body.Instructions.Any(i => i.Operand is string s && (s.Contains("Not implemented", StringComparison.OrdinalIgnoreCase) || s.Contains("Warning", StringComparison.OrdinalIgnoreCase))))
        throw new InvalidDataException("HF40 issue marker string remained");
    Console.WriteLine($"REOPEN 0x{TargetToken:X8} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
}

var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");
return 0;
