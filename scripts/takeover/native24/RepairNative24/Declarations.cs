using Mono.Cecil;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Cryptography;
internal static class Declarations
{
    static IEnumerable<TypeDefinition> Types(TypeDefinition t) { yield return t; foreach(var n in t.NestedTypes) foreach(var x in Types(n)) yield return x; }
    static string Type(TypeReference? t) => t == null ? "" : t.FullName + " @" + (t.Scope is ModuleDefinition m ? m.Assembly.Name.FullName : t.Scope?.ToString());
    static object? Value(object? value) => value switch {
        null => null, TypeReference t => new { type=Type(t) }, CustomAttributeArgument a => Arg(a), CustomAttributeArgument[] aa => aa.Select(Arg).ToArray(), byte[] b => Convert.ToHexString(b), _ => value
    };
    static object Arg(CustomAttributeArgument a) => new { type=Type(a.Type), value=Value(a.Value) };
    static object Attrs(ICustomAttributeProvider p) => p.CustomAttributes.Select(a => new { constructor=a.Constructor.FullName, scope=Type(a.Constructor.DeclaringType), args=a.ConstructorArguments.Select(Arg).ToArray(), fields=a.Fields.Select(x=>new {x.Name,value=Arg(x.Argument)}).ToArray(), properties=a.Properties.Select(x=>new {x.Name,value=Arg(x.Argument)}).ToArray() }).ToArray();
    static object? Marshal(MarshalInfo? m) => m==null ? null : new { kind=m.GetType().FullName, fields=m.GetType().GetProperties().Where(p=>p.CanRead && p.GetIndexParameters().Length==0).ToDictionary(p=>p.Name,p=>Value(p.GetValue(m))) };
    static object Generic(GenericParameter g) => new {g.Name,g.Position,kind=g.Type.ToString(),attributes=(int)g.Attributes,attrs=Attrs(g),constraints=g.Constraints.Select(x=>new {type=Type(x.ConstraintType),attrs=Attrs(x)}).ToArray()};
    static object Security(ISecurityDeclarationProvider p) => p.SecurityDeclarations.Select(x=>new { action=x.Action.ToString(),blob=Convert.ToHexString(x.GetBlob()) }).ToArray();
    static object Parameter(ParameterDefinition p) => new {p.Name,p.Index,attributes=(int)p.Attributes,type=Type(p.ParameterType),constant=p.HasConstant?Value(p.Constant):null,marshal=Marshal(p.HasMarshalInfo?p.MarshalInfo:null),attrs=Attrs(p)};
    static string? Method(MethodDefinition? m) => m?.FullName;
    public static void Write(string input,string output,string support)
    {
        var resolver=new DefaultAssemblyResolver();foreach(var dir in resolver.GetSearchDirectories())resolver.RemoveSearchDirectory(dir);resolver.AddSearchDirectory(support);
        using var a=AssemblyDefinition.ReadAssembly(input,new ReaderParameters {AssemblyResolver=resolver});var m=a.MainModule;
        var declarations=new {
            assembly=a.Name.FullName,assemblyAttributes=(uint)a.Name.Attributes,assemblyCustomAttributes=Attrs(a),assemblySecurity=Security(a),
            module=new {m.Name,m.Mvid,m.RuntimeVersion,kind=m.Kind.ToString(),architecture=m.Architecture.ToString(),attributes=(int)m.Attributes,attrs=Attrs(m),entryPoint=Method(m.EntryPoint)},
            assemblyReferences=m.AssemblyReferences.Select(x=>x.FullName).ToArray(),moduleReferences=m.ModuleReferences.Select(x=>x.Name).ToArray(),
            resources=m.Resources.Select(r=>new {r.Name,attributes=(int)r.Attributes,kind=r.ResourceType.ToString(),embeddedHash=r is EmbeddedResource er?Convert.ToHexString(SHA256.HashData(er.GetResourceData())):null,linked=r is LinkedResource lr?lr.File:null,assembly=r is AssemblyLinkedResource ar?ar.Assembly.FullName:null}).ToArray(),
            exportedTypes=m.ExportedTypes.Select(t=>new {t.FullName,attributes=(int)t.Attributes,t.Identifier,scope=t.Scope?.ToString()}).ToArray(),
            types=m.Types.SelectMany(Types).Select(t=>new {
                token=t.MetadataToken.ToUInt32(),t.FullName,attributes=(int)t.Attributes,baseType=Type(t.BaseType),t.PackingSize,t.ClassSize,attrs=Attrs(t),security=Security(t),generic=t.GenericParameters.Select(Generic).ToArray(),
                interfaces=t.Interfaces.Select(x=>new {type=Type(x.InterfaceType),attrs=Attrs(x)}).ToArray(),
                fields=t.Fields.Select(f=>new {token=f.MetadataToken.ToUInt32(),f.Name,attributes=(int)f.Attributes,type=Type(f.FieldType),f.Offset,constant=f.HasConstant?Value(f.Constant):null,initial=Convert.ToHexString(f.InitialValue),marshal=Marshal(f.HasMarshalInfo?f.MarshalInfo:null),attrs=Attrs(f)}).ToArray(),
                methods=t.Methods.Select(f=>new {token=f.MetadataToken.ToUInt32(),f.FullName,attributes=(int)f.Attributes,impl=(int)f.ImplAttributes,f.HasThis,f.ExplicitThis,convention=f.CallingConvention.ToString(),result=new {type=Type(f.ReturnType),attributes=(int)f.MethodReturnType.Attributes,attrs=Attrs(f.MethodReturnType),constant=f.MethodReturnType.HasConstant?Value(f.MethodReturnType.Constant):null,marshal=Marshal(f.MethodReturnType.HasMarshalInfo?f.MethodReturnType.MarshalInfo:null)},parameters=f.Parameters.Select(Parameter).ToArray(),generic=f.GenericParameters.Select(Generic).ToArray(),attrs=Attrs(f),security=Security(f),overrides=f.Overrides.Select(x=>x.FullName+" @"+Type(x.DeclaringType)).ToArray(),pinvoke=f.HasPInvokeInfo?new {f.PInvokeInfo.EntryPoint,attributes=(int)f.PInvokeInfo.Attributes,module=f.PInvokeInfo.Module.Name}:null}).ToArray(),
                properties=t.Properties.Select(f=>new {f.Name,attributes=(int)f.Attributes,type=Type(f.PropertyType),constant=f.HasConstant?Value(f.Constant):null,parameters=f.Parameters.Select(Parameter).ToArray(),attrs=Attrs(f),get=Method(f.GetMethod),set=Method(f.SetMethod),others=f.OtherMethods.Select(Method).ToArray()}).ToArray(),
                events=t.Events.Select(f=>new {f.Name,attributes=(int)f.Attributes,type=Type(f.EventType),attrs=Attrs(f),add=Method(f.AddMethod),remove=Method(f.RemoveMethod),invoke=Method(f.InvokeMethod),others=f.OtherMethods.Select(Method).ToArray()}).ToArray()
            }).ToArray()
        };
        File.WriteAllText(output,JsonSerializer.Serialize(declarations,new JsonSerializerOptions {WriteIndented=true,NumberHandling=JsonNumberHandling.AllowNamedFloatingPointLiterals}));
    }
}
