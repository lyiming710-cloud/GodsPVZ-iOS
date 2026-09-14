using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: Stage9PlantCtorProbe <candidate.dll>");
    return 2;
}

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static string Sha256(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();

var path = Path.GetFullPath(args[0]);
Console.WriteLine($"INPUT_SHA256 {Sha256(path)}");
using var module = ModuleDefinition.ReadModule(path, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var plant = AllTypes(module.Types).Single(t => t.FullName == "Plant");
var ctor = plant.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0);
var zeroRefs = ctor.Body.Instructions
    .Where(i => i.OpCode == OpCodes.Ldsfld && i.Operand is FieldReference fr && fr.DeclaringType.FullName == "UnityEngine.Vector3" && fr.Name == "zeroVector")
    .ToList();
Console.WriteLine($"PLANT_CTOR token=0x{ctor.MetadataToken.ToUInt32():X8} rid={ctor.MetadataToken.RID} rva=0x{ctor.RVA:X} code_size={ctor.Body.CodeSize} locals={ctor.Body.Variables.Count} zeroVector_ldsfld={zeroRefs.Count}");
foreach (var i in zeroRefs)
    Console.WriteLine($"ZERO_REF offset=0x{i.Offset:X4} operand={i.Operand}");
var fieldNames = new[] { "plantName", "characteristicText", "talentNames", "talents", "level", "healthPoint", "maxHealthPoint", "attackPoint", "attackable", "active", "camp", "updateRate", "skill", "elementManager", "dithering", "dithering_anim", "animationSprites", "UISprites1", "UISprites2", "UISprites3", "UISprites4", "elementUIControllers", "UI_Characteristic", "produce_Brightness", "flash_Brightness", "parameter_ints", "buffManager" };
foreach (var n in fieldNames)
{
    var f = plant.Fields.SingleOrDefault(x => x.Name == n);
    if (f != null) Console.WriteLine($"FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName}");
}
return 0;
