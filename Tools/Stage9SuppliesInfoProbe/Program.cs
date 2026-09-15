using Mono.Cecil;
using System.Security.Cryptography;

if (args.Length != 2) { Console.Error.WriteLine("usage: probe <dll> <expected-sha256>"); return 2; }
static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots){ foreach(var t in roots){ yield return t; foreach(var n in AllTypes(t.NestedTypes)) yield return n; } }
static uint Raw(IMetadataTokenProvider p)=>p.MetadataToken.ToUInt32();
static string Sha(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
var p=Path.GetFullPath(args[0]); var expected=args[1].ToLowerInvariant();
if(Sha(p)!=expected) throw new InvalidDataException("input sha mismatch");
using var m=ModuleDefinition.ReadModule(p,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate});
var types=AllTypes(m.Types).ToList();
var t=types.Single(x=>x.FullName=="SuppliesInfo");
var ctor=t.Methods.Single(x=>x.IsConstructor&&!x.IsStatic&&x.Parameters.Count==1&&x.Parameters[0].ParameterType.FullName=="System.Int32");
var name=t.Fields.Single(x=>x.Name=="name"&&x.FieldType.FullName=="System.String");
var id=t.Fields.Single(x=>x.Name=="ID"&&x.FieldType.FullName=="System.Int32");
Console.WriteLine($"INPUT_SHA256 {Sha(p)}");
Console.WriteLine($"TYPE token=0x{Raw(t):X8} rid={t.MetadataToken.RID} name={t.FullName}");
Console.WriteLine($"CTOR token=0x{Raw(ctor):X8} rid={ctor.MetadataToken.RID} rva=0x{ctor.RVA:X} code_size={ctor.Body.CodeSize} locals={ctor.Body.Variables.Count} initlocals={ctor.Body.InitLocals}");
Console.WriteLine($"FIELD name token=0x{Raw(name):X8} rid={name.MetadataToken.RID} type={name.FieldType.FullName}");
Console.WriteLine($"FIELD ID token=0x{Raw(id):X8} rid={id.MetadataToken.RID} type={id.FieldType.FullName}");
for(int i=0;i<ctor.Body.Instructions.Count;i++){var ins=ctor.Body.Instructions[i]; Console.WriteLine($"IL index={i} offset=0x{ins.Offset:X4} op={ins.OpCode} operand={ins.Operand}");}
Console.WriteLine("READONLY_SUPPLIESINFO_PROBE_PASS mutation=0");
return 0;
