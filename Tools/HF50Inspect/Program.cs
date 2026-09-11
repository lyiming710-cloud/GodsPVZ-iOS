using System.Text.Json;
using Mono.Cecil;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF50Inspect <Assembly-CSharp.dll>");
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

using var module = ModuleDefinition.ReadModule(Path.GetFullPath(args[0]), new ReaderParameters { InMemory = true, ReadSymbols = false });
var rows = AllTypes(module.Types)
    .SelectMany(t => t.Methods)
    .OrderBy(m => m.MetadataToken.RID)
    .Select(m => new
    {
        RID = m.MetadataToken.RID,
        Token = $"0x{m.MetadataToken.ToUInt32():X8}",
        RVA = $"0x{m.RVA:X8}",
        Type = m.DeclaringType.FullName,
        Name = m.Name,
        Parameters = m.Parameters.Count,
        GenericParameters = m.GenericParameters.Count,
        HasBody = m.HasBody,
        IL = m.HasBody ? m.Body.Instructions.Count : 0,
        CodeSize = m.HasBody ? m.Body.CodeSize : 0,
        EH = m.HasBody ? m.Body.ExceptionHandlers.Count : 0,
        Attributes = m.Attributes.ToString(),
        ImplAttributes = m.ImplAttributes.ToString()
    }).ToList();

if (rows.Count != 2317)
    throw new InvalidDataException($"MethodDef count {rows.Count}, expected 2317");

Console.WriteLine(JsonSerializer.Serialize(new { count = rows.Count, rows }, new JsonSerializerOptions { WriteIndented = true }));
return 0;
