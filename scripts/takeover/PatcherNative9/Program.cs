using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x060004A3u;
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static void Main(string[] args)
    {
        if (args.Length != 2) throw new ArgumentException("usage: PatcherNative9 <native8.dll> <native9.dll>");
        var input=Path.GetFullPath(args[0]); var output=Path.GetFullPath(args[1]);
        if(!File.Exists(input)) throw new FileNotFoundException(input);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);
        using var asm=AssemblyDefinition.ReadAssembly(input);
        var mod=asm.MainModule; var mvid=mod.Mvid;
        CheckIdentity(mod,"input"); var target=Method(mod,TargetToken);
        if(target.Name!="ZC_PoleTestJump" || !target.HasBody) throw new InvalidOperationException("target identity mismatch");
        var before=Snapshot(mod);
        Patch(mod,target);
        CheckIdentity(mod,"memory"); CheckNonTargets(mod,before,"memory");
        asm.Write(output);
        using var reopened=AssemblyDefinition.ReadAssembly(output);
        if(reopened.MainModule.Mvid!=mvid) throw new InvalidOperationException($"MVID changed {mvid}->{reopened.MainModule.Mvid}");
        CheckIdentity(reopened.MainModule,"reopened"); CheckNonTargets(reopened.MainModule,before,"reopened");
        var t=Method(reopened.MainModule,TargetToken);
        Console.WriteLine($"NATIVE9_PATCH_PASS input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
        Console.WriteLine($"TARGET token=0x{TargetToken:X8} {t.FullName} code_size={t.Body.CodeSize} il={t.Body.Instructions.Count} locals={t.Body.Variables.Count} eh={t.Body.ExceptionHandlers.Count}");
    }

    static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static MethodDefinition Method(ModuleDefinition m,uint token)=>Types(m).SelectMany(t=>t.Methods).Single(x=>x.MetadataToken.ToUInt32()==token);
    static void CheckIdentity(ModuleDefinition m,string stage){var ts=Types(m).ToArray();var mc=ts.Sum(t=>t.Methods.Count);var fc=ts.Sum(t=>t.Fields.Count);if(ts.Length!=ExpectedTypes||mc!=ExpectedMethods||fc!=ExpectedFields)throw new InvalidOperationException($"{stage}: counts {ts.Length}/{mc}/{fc}");}

    static Dictionary<uint,string> Snapshot(ModuleDefinition m)=>Types(m).SelectMany(t=>t.Methods).Where(x=>x.HasBody&&x.MetadataToken.ToUInt32()!=TargetToken).ToDictionary(x=>x.MetadataToken.ToUInt32(),Fingerprint);
    static void CheckNonTargets(ModuleDefinition m,Dictionary<uint,string> before,string stage){var now=Snapshot(m);if(now.Count!=before.Count)throw new InvalidOperationException($"{stage}: non-target count changed");foreach(var kv in before)if(!now.TryGetValue(kv.Key,out var v)||v!=kv.Value)throw new InvalidOperationException($"{stage}: non-target changed 0x{kv.Key:X8}");Console.WriteLine($"NON_TARGET_ISOLATION_PASS stage={stage} methods={before.Count}");}
    static string StableOp(Instruction i){var n=i.OpCode.Code.ToString();if(i.Operand is Instruction||i.Operand is Instruction[])return n.EndsWith("_S",StringComparison.Ordinal)?n[..^2]:n;return n;}
    static int Idx(MethodDefinition m,Instruction i)=>i==null?-1:m.Body.Instructions.IndexOf(i);
    static string Fingerprint(MethodDefinition m)
    {
        var b=new StringBuilder().Append(m.Body.InitLocals).Append('|').Append(m.Body.MaxStackSize).Append('|');
        foreach(var v in m.Body.Variables)b.Append("V:").Append(v.VariableType.FullName).Append(';');
        foreach(var i in m.Body.Instructions){b.Append(StableOp(i)).Append(':');switch(i.Operand){case null:break;case Instruction x:b.Append('@').Append(Idx(m,x));break;case Instruction[] xs:foreach(var x in xs)b.Append('@').Append(Idx(m,x)).Append(',');break;case VariableDefinition v:b.Append('V').Append(v.Index).Append(':').Append(v.VariableType.FullName);break;case ParameterDefinition p:b.Append('P').Append(p.Index).Append(':').Append(p.ParameterType.FullName);break;case MemberReference mr:b.Append('M').Append(mr.FullName).Append('@').Append(mr.DeclaringType?.Scope?.Name);break;default:b.Append(i.Operand);break;}b.Append(';');}
        foreach(var e in m.Body.ExceptionHandlers)b.Append("EH:").Append(e.HandlerType).Append(':').Append(Idx(m,e.TryStart)).Append(':').Append(Idx(m,e.TryEnd)).Append(':').Append(Idx(m,e.HandlerStart)).Append(':').Append(Idx(m,e.HandlerEnd)).Append(':').Append(Idx(m,e.FilterStart)).Append(':').Append(e.CatchType?.FullName).Append(';');
        return b.ToString();
    }

    static void Patch(ModuleDefinition mod,MethodDefinition m)
    {
        var z=m.DeclaringType;
        var fPole=z.Fields.Single(x=>x.Name=="poleZombie_pole");
        var fJump=z.Fields.Single(x=>x.Name=="poleZombie_jump");
        var fStant=z.Fields.Single(x=>x.Name=="isStant");
        var fBoard=z.Fields.Single(x=>x.Name=="board");
        var fX=z.Fields.Single(x=>x.Name=="fX");
        var fY=z.Fields.Single(x=>x.Name=="fY");
        var fPass=z.Fields.Single(x=>x.Name=="passablePoint");
        var fDir=z.Fields.Single(x=>x.Name=="rDirection");
        var fVX=mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(x=>x.DeclaringType.Name=="Vector3"&&x.Name=="x")??new FieldReference("x",mod.TypeSystem.Single,fDir.FieldType);
        var fVY=mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(x=>x.DeclaringType.Name=="Vector3"&&x.Name=="y")??new FieldReference("y",mod.TypeSystem.Single,fDir.FieldType);
        var isDisabled=Method(mod,0x06000464u);
        var getGX=Method(mod,0x06000159u); var getGY=Method(mod,0x0600015Au); var getGrid=Method(mod,0x060002C4u); var getPass=Method(mod,0x06000295u);
        var boardType=(TypeDefinition)fBoard.FieldType.Resolve(); var fBoardConfig=boardType.Fields.Single(x=>x.Name=="boardConfig");
        var gridType=(TypeDefinition)getGrid.ReturnType.Resolve();
        var fBottom=gridType.Fields.Single(x=>x.Name=="plant_bottom"); var fCommon=gridType.Fields.Single(x=>x.Name=="plant_common"); var fSheath=gridType.Fields.Single(x=>x.Name=="plant_sheath");
        var plantType=fSheath.FieldType;

        // Bind UnityEngine.Object operators with explicit Boolean return types. The damaged
        // recovery contains historical MemberRefs whose signatures are not trusted as semantic
        // authority, so only their declaring-type scope is reused here.
        var existingObjImplicit=mod.GetMemberReferences().OfType<MethodReference>().First(x=>x.DeclaringType.FullName=="UnityEngine.Object"&&x.Name=="op_Implicit"&&x.Parameters.Count==1);
        var unityObjectType=existingObjImplicit.DeclaringType;
        var unityObjectImplicit=new MethodReference("op_Implicit",mod.TypeSystem.Boolean,unityObjectType){HasThis=false};
        unityObjectImplicit.Parameters.Add(new ParameterDefinition(unityObjectType));
        var unityObjectInequality=new MethodReference("op_Inequality",mod.TypeSystem.Boolean,unityObjectType){HasThis=false};
        unityObjectInequality.Parameters.Add(new ParameterDefinition(unityObjectType));
        unityObjectInequality.Parameters.Add(new ParameterDefinition(unityObjectType));

        m.Body.Instructions.Clear();m.Body.Variables.Clear();m.Body.ExceptionHandlers.Clear();m.Body.InitLocals=true;m.Body.MaxStackSize=6;
        var vX=new VariableDefinition(mod.TypeSystem.Single);var vY=new VariableDefinition(mod.TypeSystem.Single);var vGX=new VariableDefinition(mod.TypeSystem.Int32);var vGY=new VariableDefinition(mod.TypeSystem.Int32);var vGrid=new VariableDefinition(getGrid.ReturnType);var vGridPass=new VariableDefinition(mod.TypeSystem.Int32);var vPlant=new VariableDefinition(plantType);var vResult=new VariableDefinition(mod.TypeSystem.Boolean);
        foreach(var v in new[]{vX,vY,vGX,vGY,vGrid,vGridPass,vPlant,vResult})m.Body.Variables.Add(v);
        var il=m.Body.GetILProcessor();
        var finalReturn=il.Create(OpCodes.Ldloc,vResult);
        var plantChecks=il.Create(OpCodes.Ldloc,vGrid);
        var chooseCommon=il.Create(OpCodes.Ldloc,vGrid);
        var chooseBottom=il.Create(OpCodes.Ldloc,vGrid);
        var finishPlant=il.Create(OpCodes.Ldloc,vPlant);

        // One explicit Boolean result local gives every control-flow edge the same empty-stack
        // contract and avoids branch-to-stack-value return blocks that Unity 2022 IL2CPP rejects.
        il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Stloc,vResult);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fPole);il.Emit(OpCodes.Brfalse,finalReturn);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fJump);il.Emit(OpCodes.Brtrue,finalReturn);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fStant);il.Emit(OpCodes.Brtrue,finalReturn);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Call,isDisabled);il.Emit(OpCodes.Brtrue,finalReturn);

        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fX);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,fDir);il.Emit(OpCodes.Ldfld,fVX);il.Emit(OpCodes.Ldc_R4,134f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,vX);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fY);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,fDir);il.Emit(OpCodes.Ldfld,fVY);il.Emit(OpCodes.Ldc_R4,134f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,vY);

        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fBoard);il.Emit(OpCodes.Ldfld,fBoardConfig);il.Emit(OpCodes.Ldloc,vX);il.Emit(OpCodes.Ldloc,vY);il.Emit(OpCodes.Callvirt,getGX);il.Emit(OpCodes.Stloc,vGX);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fBoard);il.Emit(OpCodes.Ldfld,fBoardConfig);il.Emit(OpCodes.Ldloc,vX);il.Emit(OpCodes.Ldloc,vY);il.Emit(OpCodes.Callvirt,getGY);il.Emit(OpCodes.Stloc,vGY);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fBoard);il.Emit(OpCodes.Ldloc,vGX);il.Emit(OpCodes.Ldloc,vGY);il.Emit(OpCodes.Callvirt,getGrid);il.Emit(OpCodes.Stloc,vGrid);
        il.Emit(OpCodes.Ldloc,vGrid);il.Emit(OpCodes.Brfalse,finalReturn);
        il.Emit(OpCodes.Ldloc,vGrid);il.Emit(OpCodes.Callvirt,getPass);il.Emit(OpCodes.Stloc,vGridPass);
        il.Emit(OpCodes.Ldloc,vGridPass);il.Emit(OpCodes.Conv_R4);il.Emit(OpCodes.Ldc_R4,100f);il.Emit(OpCodes.Ble,plantChecks);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fPass);il.Emit(OpCodes.Conv_R4);il.Emit(OpCodes.Ldloc,vGridPass);il.Emit(OpCodes.Conv_R4);il.Emit(OpCodes.Blt,plantChecks);
        il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Stloc,vResult);il.Emit(OpCodes.Br,finalReturn);

        // PC native tests Unity object lifetime in sheath -> common -> bottom priority.
        il.Append(plantChecks);il.Emit(OpCodes.Ldfld,fSheath);il.Emit(OpCodes.Stloc,vPlant);
        il.Emit(OpCodes.Ldloc,vPlant);il.Emit(OpCodes.Call,unityObjectImplicit);il.Emit(OpCodes.Brtrue,finishPlant);
        il.Append(chooseCommon);il.Emit(OpCodes.Ldfld,fCommon);il.Emit(OpCodes.Stloc,vPlant);
        il.Emit(OpCodes.Ldloc,vPlant);il.Emit(OpCodes.Call,unityObjectImplicit);il.Emit(OpCodes.Brtrue,finishPlant);
        il.Append(chooseBottom);il.Emit(OpCodes.Ldfld,fBottom);il.Emit(OpCodes.Stloc,vPlant);
        il.Emit(OpCodes.Ldloc,vPlant);il.Emit(OpCodes.Call,unityObjectImplicit);il.Emit(OpCodes.Brtrue,finishPlant);
        il.Emit(OpCodes.Ldnull);il.Emit(OpCodes.Stloc,vPlant);il.Emit(OpCodes.Br,finishPlant);

        il.Append(finishPlant);il.Emit(OpCodes.Ldnull);il.Emit(OpCodes.Call,unityObjectInequality);il.Emit(OpCodes.Stloc,vResult);il.Emit(OpCodes.Br,finalReturn);
        il.Append(finalReturn);il.Emit(OpCodes.Ret);
    }
}
