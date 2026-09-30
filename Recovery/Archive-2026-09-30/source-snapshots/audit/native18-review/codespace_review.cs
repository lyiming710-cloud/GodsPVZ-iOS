using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using System.Linq;
using System;
using System.IO;
class Review {
static void Main(){
var dir="/workspaces/GodsPVZ-native14-check/.validation/native14/replay-edye4slp/";
using var p17=new PEReader(File.OpenRead(dir+"native17-unlinked.dll"));
using var p18=new PEReader(File.OpenRead(dir+"native18-unlinked.dll"));
var a=p17.GetMetadataReader();var b=p18.GetMetadataReader();
var targetNames=new[]{"PreviousPosition","Start_PreviousPosition","Update_Move","Update_PreviousPosition","ArmBroken","Ashe","CheckZombieWin","CreateStartPrePath","CreatParticles","DestroyZombie","Die"};
int count=0,changes=0,targets=0;var targetTokens=new System.Collections.Generic.List<string>();
foreach(var h in a.MethodDefinitions){var m=a.GetMethodDefinition(h);var n=b.GetMethodDefinition(h);if(m.RelativeVirtualAddress==0)continue;
var name=a.GetString(m.Name);var type=a.GetString(a.GetTypeDefinition(m.GetDeclaringType()).Name);
if(type=="Zombie"&&targetNames.Contains(name)){targets++;targetTokens.Add(name+"=0x"+System.Reflection.Metadata.Ecma335.MetadataTokens.GetToken(h).ToString("X8"));continue;}
count++;if(!p17.GetMethodBody(m.RelativeVirtualAddress).GetILBytes()!.SequenceEqual(p18.GetMethodBody(n.RelativeVirtualAddress).GetILBytes()!))changes++;
}
Console.WriteLine(JsonSerializer.Serialize(new{check="Independent_SystemReflectionMetadata_raw_CIL",nonTargets=count,changed=changes,targets,targetTokens}));
var dm=new DynamicMethod("NativeIntPlusInt64",typeof(long),Type.EmptyTypes);var il=dm.GetILGenerator();il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Conv_I);il.Emit(OpCodes.Ldc_I8,6442450944L);il.Emit(OpCodes.Add);il.Emit(OpCodes.Ret);
try{var value=((Func<long>)dm.CreateDelegate(typeof(Func<long>)))();Console.WriteLine("CLR_MIXED_ADD_ACCEPTED value="+value+" (runtime tolerance is not ECMA verification)");}catch(Exception ex){Console.WriteLine("CLR_MIXED_ADD_REJECTED "+ex.GetType().FullName);}
var root="/workspaces/GodsPVZ-native14-check/";foreach(var f in new[]{"scripts/package_unsigned_ipa.mjs","scripts/validate_ipa.mjs"})Console.WriteLine("WORKFLOW_SCRIPT_EXISTS "+f+"="+File.Exists(root+f));
using var module=Mono.Cecil.ModuleDefinition.CreateModule("IndependentNegativeControl",Mono.Cecil.ModuleKind.Dll);
var vtype=new Mono.Cecil.TypeDefinition("Test","Vector3",Mono.Cecil.TypeAttributes.Public|Mono.Cecil.TypeAttributes.Sealed|Mono.Cecil.TypeAttributes.SequentialLayout,module.ImportReference(typeof(ValueType)));module.Types.Add(vtype);
vtype.Fields.Add(new Mono.Cecil.FieldDefinition("x",Mono.Cecil.FieldAttributes.Public,module.TypeSystem.Single));
var callee=new Mono.Cecil.MethodDefinition("ConsumeValue",Mono.Cecil.MethodAttributes.Static|Mono.Cecil.MethodAttributes.Public,module.TypeSystem.Void);callee.Parameters.Add(new Mono.Cecil.ParameterDefinition(vtype));vtype.Methods.Add(callee);
var bad=new Mono.Cecil.MethodDefinition("WrongByRefArgument",Mono.Cecil.MethodAttributes.Static|Mono.Cecil.MethodAttributes.Public,module.TypeSystem.Void);vtype.Methods.Add(bad);var local=new Mono.Cecil.Cil.VariableDefinition(vtype);bad.Body.Variables.Add(local);var cil=bad.Body.GetILProcessor();cil.Emit(Mono.Cecil.Cil.OpCodes.Ldloca,local);cil.Emit(Mono.Cecil.Cil.OpCodes.Call,callee);cil.Emit(Mono.Cecil.Cil.OpCodes.Ret);
var check=typeof(Program).GetMethod("StackCheck",BindingFlags.NonPublic|BindingFlags.Static)!;
try{check.Invoke(null,new object[]{bad});Console.WriteLine("PATCHER_STACKCHECK_TYPE_NEGATIVE_CONTROL_ACCEPTED: Vector3& supplied to Vector3 value parameter");}catch(TargetInvocationException ex){Console.WriteLine("PATCHER_STACKCHECK_TYPE_NEGATIVE_CONTROL_REJECTED: "+ex.InnerException!.Message);}
}
}
