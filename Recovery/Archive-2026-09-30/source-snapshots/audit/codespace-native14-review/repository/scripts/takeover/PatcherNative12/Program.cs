using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x06000111u;
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static void Main(string[] args)
    {
        if(args.Length < 2 || args.Length > 3) throw new ArgumentException("usage: PatcherNative12 <native11.dll> <native12.dll> [linked]");
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
        Console.WriteLine($"NATIVE12_PATCH_PASS linked={linked} input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
        Console.WriteLine($"TARGET token=0x{rt.MetadataToken.ToUInt32():X8} {rt.FullName} code_size={rt.Body.CodeSize} il={rt.Body.Instructions.Count} locals={rt.Body.Variables.Count} eh={rt.Body.ExceptionHandlers.Count} gp={rt.GenericParameters.Count}");
    }

    static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static MethodDefinition FindTarget(ModuleDefinition m,bool linked)
    {
        var t=Types(m).Single(x=>x.Name=="Damage"&&x.Namespace=="");
        var md=t.Methods.Single(x=>x.Name=="CalculateAD"&&x.GenericParameters.Count==1&&x.Parameters.Count==4);
        if(!linked && md.MetadataToken.ToUInt32()!=TargetToken)throw new InvalidOperationException($"target token mismatch 0x{md.MetadataToken.ToUInt32():X8}");
        if(!md.HasBody)throw new InvalidOperationException("target body missing");
        if(!md.IsStatic)throw new InvalidOperationException("CalculateAD unexpectedly became instance method");
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

    static void Patch(ModuleDefinition mod, MethodDefinition m)
    {
        if(m.GenericParameters.Count!=1) throw new InvalidOperationException("expected one generic parameter");
        var gp=m.GenericParameters[0];
        var zombie=Types(mod).Single(t=>t.Name=="Zombie"&&t.Namespace=="");
        m.Body.Instructions.Clear();m.Body.Variables.Clear();m.Body.ExceptionHandlers.Clear();m.Body.InitLocals=false;m.Body.MaxStackSize=3;
        var il=m.Body.GetILProcessor();
        var hostChecks=il.Create(OpCodes.Nop);
        var typeCheck=il.Create(OpCodes.Nop);
        var armorFormula=il.Create(OpCodes.Nop);
        var lowDamage=il.Create(OpCodes.Nop);

        // DamageAttribute.real == 4 bypasses armor entirely.
        il.Emit(OpCodes.Ldarg_3);il.Emit(OpCodes.Ldc_I4_4);il.Emit(OpCodes.Bne_Un,hostChecks);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ret);

        // Unconstrained T uses the same observable generic-sharing semantics as the PC bodies:
        // box T, null-test, then runtime type-test against Zombie.
        il.Append(hostChecks);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Box,gp);il.Emit(OpCodes.Brtrue,typeCheck);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ret);
        il.Append(typeCheck);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Box,gp);il.Emit(OpCodes.Isinst,zombie);il.Emit(OpCodes.Brtrue,armorFormula);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ret);

        // PC x64 uses COMISS defense,attack + JAE. Ordered defense>=attack takes
        // the 10% branch; unordered falls through to the subtract/add branch.
        il.Append(armorFormula);il.Emit(OpCodes.Ldarg_2);il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Bge,lowDamage);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ldarg_2);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Ldarg_2);il.Emit(OpCodes.Ldc_R4,0.1f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Add);il.Emit(OpCodes.Ret);
        il.Append(lowDamage);il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ldc_R4,0.1f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Ret);
    }
}
