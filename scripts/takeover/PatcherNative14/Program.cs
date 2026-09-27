using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x06000126u;
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static void Main(string[] args)
    {
        if(args.Length < 2 || args.Length > 3) throw new ArgumentException("usage: PatcherNative14 <native13.dll> <native14.dll> [linked]");
        bool linked=args.Length==3 && args[2]=="linked";
        var input=Path.GetFullPath(args[0]); var output=Path.GetFullPath(args[1]);
        if(!File.Exists(input)) throw new FileNotFoundException(input);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);
        using var asm=AssemblyDefinition.ReadAssembly(input); var mod=asm.MainModule; var mvid=mod.Mvid;
        if(!linked) CheckIdentity(mod,"input");
        var target=FindTarget(mod,linked); var before=Snapshot(mod,target);
        Patch(mod,target);
        if(!linked) CheckIdentity(mod,"memory"); CheckNonTargets(mod,target,before,"memory");
        asm.Write(output);
        using var reopened=AssemblyDefinition.ReadAssembly(output);
        if(reopened.MainModule.Mvid!=mvid) throw new InvalidOperationException($"MVID changed {mvid}->{reopened.MainModule.Mvid}");
        if(!linked) CheckIdentity(reopened.MainModule,"reopened");
        var rt=FindTarget(reopened.MainModule,linked); CheckNonTargets(reopened.MainModule,rt,before,"reopened");
        if(rt.Body.ExceptionHandlers.Count!=1 || rt.Body.ExceptionHandlers[0].HandlerType!=ExceptionHandlerType.Finally) throw new InvalidOperationException("finally cleanup missing after reopen");
        Console.WriteLine($"NATIVE14_PATCH_PASS linked={linked} input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
        Console.WriteLine($"TARGET token=0x{rt.MetadataToken.ToUInt32():X8} {rt.FullName} code_size={rt.Body.CodeSize} il={rt.Body.Instructions.Count} locals={rt.Body.Variables.Count} eh={rt.Body.ExceptionHandlers.Count} gp={rt.GenericParameters.Count}");
    }

    static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static MethodDefinition FindTarget(ModuleDefinition m,bool linked)
    {
        var t=Types(m).Single(x=>x.Name=="ElementManager"&&x.Namespace=="");
        var md=t.Methods.Single(x=>x.Name=="CreateNewElements"&&x.GenericParameters.Count==1&&x.Parameters.Count==1);
        if(!linked && md.MetadataToken.ToUInt32()!=TargetToken)throw new InvalidOperationException($"target token mismatch 0x{md.MetadataToken.ToUInt32():X8}");
        if(!md.HasBody)throw new InvalidOperationException("target body missing");
        if(md.IsStatic)throw new InvalidOperationException("CreateNewElements unexpectedly static");
        return md;
    }
    static void CheckIdentity(ModuleDefinition m,string stage){var ts=Types(m).ToArray();int mc=ts.Sum(t=>t.Methods.Count),fc=ts.Sum(t=>t.Fields.Count);if(ts.Length!=ExpectedTypes||mc!=ExpectedMethods||fc!=ExpectedFields)throw new InvalidOperationException($"{stage}: counts {ts.Length}/{mc}/{fc}");}
    static Dictionary<uint,string> Snapshot(ModuleDefinition m,MethodDefinition target)=>Types(m).SelectMany(t=>t.Methods).Where(x=>x.HasBody&&x!=target).ToDictionary(x=>x.MetadataToken.ToUInt32(),Fingerprint);
    static void CheckNonTargets(ModuleDefinition m,MethodDefinition target,Dictionary<uint,string> before,string stage){var now=Snapshot(m,target);if(now.Count!=before.Count)throw new InvalidOperationException($"{stage}: non-target count {before.Count}->{now.Count}");foreach(var kv in before)if(!now.TryGetValue(kv.Key,out var v)||v!=kv.Value)throw new InvalidOperationException($"{stage}: non-target changed 0x{kv.Key:X8}");Console.WriteLine($"NON_TARGET_ISOLATION_PASS stage={stage} methods={before.Count}");}
    static string StableOp(Instruction i){var n=i.OpCode.Code.ToString();if((i.Operand is Instruction||i.Operand is Instruction[])&&n.EndsWith("_S",StringComparison.Ordinal))return n[..^2];return n;}
    static int Ix(MethodDefinition m,Instruction i)=>i==null?-1:m.Body.Instructions.IndexOf(i);
    static string Fingerprint(MethodDefinition m)
    {
        var b=new StringBuilder().Append(m.Body.InitLocals).Append('|').Append(m.Body.MaxStackSize).Append('|');
        foreach(var v in m.Body.Variables)b.Append("V:").Append(v.VariableType.FullName).Append(';');
        foreach(var i in m.Body.Instructions){b.Append(StableOp(i)).Append(':');switch(i.Operand){case null:break;case Instruction x:b.Append('@').Append(Ix(m,x));break;case Instruction[] xs:foreach(var x in xs)b.Append('@').Append(Ix(m,x)).Append(',');break;case VariableDefinition v:b.Append('V').Append(v.Index).Append(':').Append(v.VariableType.FullName);break;case ParameterDefinition p:b.Append('P').Append(p.Index).Append(':').Append(p.ParameterType.FullName);break;case MemberReference mr:b.Append('M').Append(mr.FullName).Append('@').Append(mr.DeclaringType?.Scope?.Name);break;default:b.Append(i.Operand);break;}b.Append(';');}
        foreach(var e in m.Body.ExceptionHandlers)b.Append("EH:").Append(e.HandlerType).Append(':').Append(Ix(m,e.TryStart)).Append(':').Append(Ix(m,e.TryEnd)).Append(':').Append(Ix(m,e.HandlerStart)).Append(':').Append(Ix(m,e.HandlerEnd)).Append(':').Append(Ix(m,e.FilterStart)).Append(':').Append(e.CatchType?.FullName).Append(';');
        return b.ToString();
    }
    static MethodReference MR(ModuleDefinition m,string decl,string name,int argc,string ret=null)
    {
        var q=m.GetMemberReferences().OfType<MethodReference>().Where(x=>x.DeclaringType.FullName==decl&&x.Name==name&&x.Parameters.Count==argc);
        if(ret!=null)q=q.Where(x=>x.ReturnType.FullName==ret);
        return q.First();
    }

    static void Patch(ModuleDefinition mod, MethodDefinition m)
    {
        if(m.GenericParameters.Count!=1) throw new InvalidOperationException("expected one generic parameter");
        var gp=m.GenericParameters[0];
        var manager=m.DeclaringType;
        var zombie=Types(mod).Single(t=>t.Name=="Zombie"&&t.Namespace=="");
        var plant=Types(mod).Single(t=>t.Name=="Plant"&&t.Namespace=="");
        var element=Types(mod).Single(t=>t.Name=="Element"&&t.Namespace=="");
        var elementType=Types(mod).Single(t=>t.Name=="ElementType"&&t.Namespace=="");
        var fZombie=manager.Fields.Single(f=>f.Name=="zombie");
        var fPlant=manager.Fields.Single(f=>f.Name=="plant");
        var fElements=manager.Fields.Single(f=>f.Name=="elements");
        var fDamage=element.Fields.Single(f=>f.Name=="damage");
        var fManager=element.Fields.Single(f=>f.Name=="manager");
        var ctor=element.Methods.Single(x=>x.IsConstructor&&!x.IsStatic&&x.Parameters.Count==4&&x.Parameters[0].ParameterType.FullName==elementType.FullName&&x.Parameters[1].ParameterType.FullName=="System.Single"&&x.Parameters[2].ParameterType.FullName=="Damage"&&x.Parameters[3].ParameterType.FullName=="System.Boolean");
        var typeFromHandle=MR(mod,"System.Type","GetTypeFromHandle",1,"System.Type");
        var enumValues=MR(mod,"System.Enum","GetValues",1,"System.Array");
        var arrayEnumerator=MR(mod,"System.Array","GetEnumerator",0,"System.Collections.IEnumerator");
        var moveNext=MR(mod,"System.Collections.IEnumerator","MoveNext",0,"System.Boolean");
        var current=MR(mod,"System.Collections.IEnumerator","get_Current",0,"System.Object");
        var dispose=MR(mod,"System.IDisposable","Dispose",0,"System.Void");
        var listAdd=mod.GetMemberReferences().OfType<MethodReference>().First(x=>x.Name=="Add"&&x.DeclaringType.FullName==fElements.FieldType.FullName&&x.Parameters.Count==1);
        var ienum=moveNext.DeclaringType;
        var idisposable=dispose.DeclaringType;

        m.Body.Instructions.Clear(); m.Body.Variables.Clear(); m.Body.ExceptionHandlers.Clear();
        m.Body.InitLocals=true; m.Body.MaxStackSize=5;
        var vEnum=new VariableDefinition(ienum); var vType=new VariableDefinition(elementType); var vElement=new VariableDefinition(element); var vDisp=new VariableDefinition(idisposable);
        m.Body.Variables.Add(vEnum); m.Body.Variables.Add(vType); m.Body.Variables.Add(vElement); m.Body.Variables.Add(vDisp);
        var il=m.Body.GetILProcessor();

        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldarg_1); il.Emit(OpCodes.Box,gp); il.Emit(OpCodes.Isinst,zombie); il.Emit(OpCodes.Stfld,fZombie);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldarg_1); il.Emit(OpCodes.Box,gp); il.Emit(OpCodes.Isinst,plant); il.Emit(OpCodes.Stfld,fPlant);

        il.Emit(OpCodes.Ldtoken,elementType); il.Emit(OpCodes.Call,typeFromHandle); il.Emit(OpCodes.Call,enumValues); il.Emit(OpCodes.Callvirt,arrayEnumerator); il.Emit(OpCodes.Stloc,vEnum);

        var tryStart=il.Create(OpCodes.Ldloc,vEnum);
        var loopBody=il.Create(OpCodes.Ldloc,vEnum);
        var handlerStart=il.Create(OpCodes.Ldloc,vEnum);
        var endFinally=il.Create(OpCodes.Endfinally);
        var afterFinally=il.Create(OpCodes.Ret);
        var leaveTry=il.Create(OpCodes.Leave,afterFinally);

        il.Append(tryStart); il.Emit(OpCodes.Callvirt,moveNext); il.Emit(OpCodes.Brtrue,loopBody); il.Append(leaveTry);
        il.Append(loopBody); il.Emit(OpCodes.Callvirt,current); il.Emit(OpCodes.Unbox_Any,elementType); il.Emit(OpCodes.Stloc,vType);
        il.Emit(OpCodes.Ldloc,vType); il.Emit(OpCodes.Ldc_R4,0f); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ldc_I4_1); il.Emit(OpCodes.Newobj,ctor); il.Emit(OpCodes.Stloc,vElement);
        il.Emit(OpCodes.Ldloc,vElement); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Stfld,fDamage);
        il.Emit(OpCodes.Ldloc,vElement); il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Stfld,fManager);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld,fElements); il.Emit(OpCodes.Ldloc,vElement); il.Emit(OpCodes.Callvirt,listAdd);
        il.Emit(OpCodes.Br,tryStart);

        il.Append(handlerStart); il.Emit(OpCodes.Isinst,idisposable); il.Emit(OpCodes.Stloc,vDisp); il.Emit(OpCodes.Ldloc,vDisp); il.Emit(OpCodes.Brfalse,endFinally); il.Emit(OpCodes.Ldloc,vDisp); il.Emit(OpCodes.Callvirt,dispose); il.Append(endFinally);
        il.Append(afterFinally);
        m.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=tryStart,TryEnd=handlerStart,HandlerStart=handlerStart,HandlerEnd=afterFinally});
    }
}
