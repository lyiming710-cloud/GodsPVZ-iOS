using Mono.Cecil;
using System;
using System.IO;
using System.Linq;

if (args.Length != 1) { Console.Error.WriteLine("Usage: MethodIndexDump <Assembly-CSharp.dll>"); return 2; }
using var m = ModuleDefinition.ReadModule(Path.GetFullPath(args[0]), new ReaderParameters { InMemory = true, ReadSymbols = false });
var methods = m.Types.SelectMany(Flatten).SelectMany(t => t.Methods).OrderBy(x => x.MetadataToken.RID).ToList();
foreach (var method in methods)
{
    var rid = method.MetadataToken.RID;
    var index = rid - 1;
    var ps = string.Join(",", method.Parameters.Select(p => p.ParameterType.FullName));
    Console.WriteLine($"{index}\t{rid}\t{method.DeclaringType.FullName}\t{method.Name}\t{ps}");
}
return 0;

static System.Collections.Generic.IEnumerable<TypeDefinition> Flatten(TypeDefinition t)
{
    yield return t;
    foreach (var n in t.NestedTypes)
        foreach (var x in Flatten(n)) yield return x;
}
