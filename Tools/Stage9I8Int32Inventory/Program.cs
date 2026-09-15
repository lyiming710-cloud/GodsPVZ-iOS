using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
if(args.Length!=1){Console.Error.WriteLine("usage: inventory <dll>");return 2;}
var p=Path.GetFullPath(args[0]); Console.WriteLine($"INPUT_SHA256 {Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant()}");
using var m=ModuleDefinition.ReadModule(p,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate});
IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> xs){foreach(var t in xs){yield return t;foreach(var n in All(t.NestedTypes))yield return n;}}
var hits=new List<(MethodDefinition m,FieldReference f,int off)>();
foreach(var md in All(m.Types).SelectMany(t=>t.Methods).Where(x=>x.HasBody)){
 var ins=md.Body.Instructions;
 for(int i=0;i+1<ins.Count;i++) if(ins[i].OpCode==OpCodes.Ldc_I8 && ins[i].Operand is long n && n==4294967295L && ins[i+1].OpCode==OpCodes.Stfld && ins[i+1].Operand is FieldReference f && f.FieldType.FullName=="System.Int32") hits.Add((md,f,ins[i].Offset));
}
foreach(var h in hits.OrderBy(x=>x.m.MetadataToken.RID).ThenBy(x=>x.off)) Console.WriteLine($"I8_INT32_HIT method_token=0x{h.m.MetadataToken.ToInt32():X8} rid={h.m.MetadataToken.RID} method={h.m.FullName} il=0x{h.off:X4} field_token=0x{h.f.MetadataToken.ToInt32():X8} field={h.f.FullName}");
Console.WriteLine($"I8_INT32_HIT_COUNT {hits.Count}");
Console.WriteLine("READONLY_I8_INT32_INVENTORY_PASS mutation=0");
return 0;
