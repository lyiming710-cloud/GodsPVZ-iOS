using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.DecompileType <assembly.dll> <TypeName> [TypeName ...]");
    return 2;
}

var assemblyPath = Path.GetFullPath(args[0]);
var resolver = new UniversalAssemblyResolver(assemblyPath, false, null);
var settings = new DecompilerSettings
{
    ThrowOnAssemblyResolveErrors = false,
    UseSdkStyleProjectFormat = true,
};
var decompiler = new CSharpDecompiler(assemblyPath, resolver, settings);

for (var i = 1; i < args.Length; i++)
{
    var typeName = args[i];
    Console.WriteLine($"// ===== TYPE {typeName} =====");
    try
    {
        Console.WriteLine(decompiler.DecompileTypeAsString(new FullTypeName(typeName)));
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to decompile {typeName}: {ex}");
        return 1;
    }
}

return 0;
