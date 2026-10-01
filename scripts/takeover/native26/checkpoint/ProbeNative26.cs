using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using System.Buffers.Binary;
using System.Security.Cryptography;
using AssemblyDefinition=Mono.Cecil.AssemblyDefinition;
using MethodDefinition=Mono.Cecil.MethodDefinition;
var bytes=File.ReadAllBytes(args[0]);using var asm=AssemblyDefinition.ReadAssembly(args[0]);using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);var md=pe.GetMetadataReader();
var a=asm.MainModule.Types.Single(t=>t.Name=="AttackRange");var d=asm.MainModule.Types.Single(t=>t.Name=="Device");
object Describe(MethodDefinition m){var block=pe.GetMethodBody(m.RVA);var sig=block.LocalSignature;var tok=System.Reflection.Metadata.Ecma335.MetadataTokens.GetToken(sig);var blob=md.GetBlobBytes(md.GetStandaloneSignature(sig).Signature);return new {name=m.FullName,token=$"0x{m.MetadataToken.ToUInt32():X8}",m.RVA,m.Body.CodeSize,m.Body.MaxStackSize,localSigToken=$"0x{tok:X8}",localSigBlob=Convert.ToHexString(blob),closed=m.Body.Variables.All(v=>!v.VariableType.ContainsGenericParameter),locals=m.Body.Variables.Select(v=>new{type=v.VariableType.FullName,v.VariableType.IsValueType,v.VariableType.ContainsGenericParameter}),calls=m.Body.Instructions.Where(i=>i.Operand is MethodReference).Select(i=>new{ i.Offset,token=$"0x{((MethodReference)i.Operand).MetadataToken.ToUInt32():X8}",name=((MethodReference)i.Operand).FullName}),fields=m.Body.Instructions.Where(i=>i.Operand is FieldReference).Select(i=>new{i.Offset,token=$"0x{((FieldReference)i.Operand).MetadataToken.ToUInt32():X8}",name=((FieldReference)i.Operand).FullName})};}
var result=new{sha256=Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant(),methods=a.Methods.Where(m=>m.MetadataToken.ToUInt32() is 0x060000D8 or 0x060000D9).Select(Describe),fields=new[]{a,d}.Select(t=>new{type=t.FullName,fields=t.Fields.Select(f=>new{f.Name,type=f.FieldType.FullName,token=$"0x{f.MetadataToken.ToUInt32():X8}"})})};
File.WriteAllText(args[1],JsonSerializer.Serialize(result,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(result));
if(args.Length>2){using var unity=AssemblyDefinition.ReadAssembly(args[2]);var ctors=unity.MainModule.Types.Where(t=>t.FullName is "UnityEngine.Rect" or "UnityEngine.Vector3").SelectMany(t=>t.Methods.Where(m=>m.Name==".ctor"&&m.Parameters.All(p=>p.ParameterType.FullName=="System.Single")&&(m.Parameters.Count==4||m.Parameters.Count==3))).Select(m=>new{name=m.FullName,token=$"0x{m.MetadataToken.ToUInt32():X8}",instructions=m.Body.Instructions.Select(i=>new{i.Offset,opcode=i.OpCode.Name,operand=i.Operand?.ToString()})});File.WriteAllText(args[1]+".unity.json",JsonSerializer.Serialize(new{sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[2]))).ToLowerInvariant(),constructors=ctors},new JsonSerializerOptions{WriteIndented=true}));}
