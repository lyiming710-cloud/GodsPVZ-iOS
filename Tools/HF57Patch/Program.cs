using Mono.Cecil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF57Patch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "2308c08296cfb83a72321325fd28362d59c25d6bb1a9cc6635a7b4624af202b0";

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"FORMAL_INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256)
    throw new InvalidDataException($"HF57 formal HF56 input SHA mismatch: expected {ExpectedInputSha256}, got {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true });

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

var allTypes = AllTypes(module.Types).ToList();
var methodCount = allTypes.SelectMany(t => t.Methods).Count();
if (methodCount != 2317)
    throw new InvalidDataException($"HF57 input MethodDef count drifted: {methodCount}");

var gameStart = allTypes.SingleOrDefault(t => t.FullName == "GameStart")
    ?? throw new InvalidDataException("HF57 GameStart type missing");
var field = gameStart.Fields.SingleOrDefault(f => f.Name == "mainMenuPrefab")
    ?? throw new InvalidDataException("HF57 GameStart.mainMenuPrefab field missing");

if (!field.IsPrivate || field.IsStatic || field.FieldType.FullName != "UnityEngine.GameObject")
    throw new InvalidDataException($"HF57 field signature drift: attrs={field.Attributes} type={field.FieldType.FullName}");

var existing = field.CustomAttributes.Count(a => a.AttributeType.FullName == "UnityEngine.SerializeField");
if (existing != 0)
    throw new InvalidDataException($"HF57 expected missing SerializeField before patch, found {existing}");

var coreRef = module.AssemblyReferences.SingleOrDefault(a => a.Name == "UnityEngine.CoreModule")
    ?? throw new InvalidDataException("HF57 UnityEngine.CoreModule AssemblyRef missing");
var serializeFieldType = new TypeReference("UnityEngine", "SerializeField", module, coreRef, false);
var ctor = new MethodReference(".ctor", module.TypeSystem.Void, serializeFieldType)
{
    HasThis = true,
    ExplicitThis = false,
    CallingConvention = MethodCallingConvention.Default
};
field.CustomAttributes.Add(new CustomAttribute(ctor));

Console.WriteLine("PATCH GameStart.mainMenuPrefab: add [UnityEngine.SerializeField] custom attribute");
module.Write(output);

var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using (var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true }))
{
    var types = AllTypes(reopen.Types).ToList();
    if (types.SelectMany(t => t.Methods).Count() != 2317)
        throw new InvalidDataException("HF57 output MethodDef count drifted from 2317");

    var rt = types.Single(t => t.FullName == "GameStart");
    var rf = rt.Fields.Single(f => f.Name == "mainMenuPrefab");
    if (!rf.IsPrivate || rf.IsStatic || rf.FieldType.FullName != "UnityEngine.GameObject")
        throw new InvalidDataException("HF57 reopen field signature drift");

    var attrs = rf.CustomAttributes.Where(a => a.AttributeType.FullName == "UnityEngine.SerializeField").ToList();
    if (attrs.Count != 1 || attrs[0].Constructor.Name != ".ctor" || attrs[0].Constructor.Parameters.Count != 0)
        throw new InvalidDataException($"HF57 reopen SerializeField validation failed count={attrs.Count}");

    Console.WriteLine($"REOPEN GameStart.mainMenuPrefab attrs={rf.CustomAttributes.Count} serializeField={attrs.Count}");
}

return 0;
