using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
internal static partial class Program
{
    static IEnumerable<MethodDefinition> AdditionalTargets(ModuleDefinition mod)
    {
        foreach(var spec in new[]{("Device/ICEUIController","Update"),("Device/ICEUIController","Broken"),("Project","Start"),("Project","LoopAddAnimation")})
            yield return Types(mod).Single(t=>t.FullName==spec.Item1).Methods.Single(m=>m.Name==spec.Item2);
    }
    static void PatchAdditionalTargets(ModuleDefinition mod)
    {
        foreach(var m in AdditionalTargets(mod)){
            if(m.DeclaringType.FullName=="Device/ICEUIController")PatchIce(mod,m);
            else if(m.Name=="Start")PatchProjectStart(mod,m);else PatchProjectLoop(mod,m);
        }
    }
    // Original PC 0x18034D610/0x18034D1B0: particle origin is the controller
    // GameObject position; audio origin is Camera.main.transform.position.
    static void PatchIce(ModuleDefinition mod,MethodDefinition m)
    {
        int[] positions=m.Name=="Update"?new[]{25,33}:new[]{4};int[] audio=m.Name=="Update"?new[]{47,46}:new[]{15};int pi=0,ai=0;
        var ins=m.Body.Instructions;
        for(int n=0;n<ins.Count;n++)if(ins[n].Operand is MethodReference mr&&ins[n].OpCode==OpCodes.Call){
            int index=-1,at=-1;
            if(mr.DeclaringType.FullName=="UnityEngine.Transform"&&mr.Name=="set_position"){index=positions[pi++];at=n-1;}
            if(mr.DeclaringType.FullName=="GlobalStaticVars"&&mr.Name=="CreateAudioAtPoint"&&mr.Parameters.Count==3){index=audio[ai++];at=n-2;}
            if(at>=0){var v=m.Body.Variables[index];if(v.VariableType.FullName!="UnityEngine.Vector3"||ins[at].OpCode!=OpCodes.Ldloca)throw new Exception("ICE value argument shape drift");ins[at].OpCode=OpCodes.Ldloc;ins[at].Operand=v;}
        }
        if(pi!=positions.Length||ai!=audio.Length)throw new Exception("ICE expected call count changed");
        RemoveTrailingStackWarning(m);
    }
    // PC 0x1803784B0: Vector3.one * size was entirely missing from the damaged IL.
    static void PatchProjectStart(ModuleDefinition mod,MethodDefinition m)
    {
        var ins=m.Body.Instructions;var v=m.Body.Variables.Single(x=>x.Index==16&&x.VariableType.FullName=="UnityEngine.Vector3");
        var setter=ins.Single(i=>i.Operand is MethodReference mr&&mr.Name=="set_localScale");
        if(setter.Previous.OpCode!=OpCodes.Ldloca||setter.Previous.Operand!=v)throw new Exception("Project scale argument drift");
        setter.Previous.OpCode=OpCodes.Ldloc;
        var anchor=ins.Single(i=>i.Offset==0x1f2);if(anchor.OpCode!=OpCodes.Nop)throw new Exception("Project scale block drift");
        anchor.OpCode=OpCodes.Call;anchor.Operand=MR(mod,"UnityEngine.Vector3","get_one",0);
        var mul=mod.GetMemberReferences().OfType<MethodReference>().First(x=>x.DeclaringType.FullName=="UnityEngine.Vector3"&&x.Name=="op_Multiply"&&x.Parameters.Count==2&&x.Parameters[0].ParameterType.FullName=="UnityEngine.Vector3"&&x.Parameters[1].ParameterType.FullName=="System.Single");
        var extra=new[]{Instruction.Create(OpCodes.Ldarg_0),Instruction.Create(OpCodes.Ldfld,m.DeclaringType.Fields.Single(f=>f.Name=="size")),Instruction.Create(OpCodes.Call,mul),Instruction.Create(OpCodes.Stloc,v)};
        var il=m.Body.GetILProcessor();foreach(var i in extra){il.InsertAfter(anchor,i);anchor=i;}
        var hint=ins.Single(i=>i.OpCode==OpCodes.Ldstr&&((string)i.Operand).StartsWith("Not implemented instruction:"));
        if(hint.Next.OpCode!=OpCodes.Pop)throw new Exception("Project jp hint shape drift");hint.Next.OpCode=OpCodes.Nop;hint.OpCode=OpCodes.Nop;hint.Operand=null;
    }
    // PC 0x180377630: pre-order SpriteRenderer collection, recursive Transform
    // IEnumerator traversal, IDisposable finally on normal and exceptional exit.
    static void PatchProjectLoop(ModuleDefinition mod,MethodDefinition m)
    {
        var getComponent=m.Body.Instructions.Select(i=>i.Operand).OfType<GenericInstanceMethod>().Single(x=>x.Name=="GetComponent"&&x.GenericArguments.Single().FullName=="UnityEngine.SpriteRenderer");
        var getEnumerator=MR(mod,"UnityEngine.Transform","GetEnumerator",0);var move=MR(mod,"System.Collections.IEnumerator","MoveNext",0);var current=MR(mod,"System.Collections.IEnumerator","get_Current",0);var dispose=MR(mod,"System.IDisposable","Dispose",0);
        var field=m.DeclaringType.Fields.Single(f=>f.Name=="animationSprites");
        var template=mod.GetMemberReferences().OfType<MethodReference>().First(x=>x.Name=="Add"&&x.DeclaringType is GenericInstanceType g&&g.ElementType.FullName=="System.Collections.Generic.List`1"&&x.Parameters.Count==1);
        var add=new MethodReference("Add",mod.TypeSystem.Void,field.FieldType){HasThis=true};add.Parameters.Add(new ParameterDefinition(template.Parameters[0].ParameterType));
        Reset(m);var il=m.Body.GetILProcessor();var en=Local(m,getEnumerator.ReturnType);var disposable=Local(m,dispose.DeclaringType);
        var skipAdd=Instruction.Create(OpCodes.Nop);var loop=Instruction.Create(OpCodes.Ldloc,en);var body=Instruction.Create(OpCodes.Ldarg_0);var done=Instruction.Create(OpCodes.Ret);var cleanup=Instruction.Create(OpCodes.Ldloc,en);var endFinally=Instruction.Create(OpCodes.Endfinally);
        il.Emit(OpCodes.Ldarg,m.Parameters[0]);il.Emit(OpCodes.Callvirt,getComponent);il.Emit(OpCodes.Call,MR(mod,"UnityEngine.Object","op_Implicit",1));il.Emit(OpCodes.Brfalse,skipAdd);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,field);il.Emit(OpCodes.Ldarg,m.Parameters[0]);il.Emit(OpCodes.Callvirt,MR(mod,"UnityEngine.Component","get_gameObject",0));il.Emit(OpCodes.Callvirt,add);il.Append(skipAdd);
        il.Emit(OpCodes.Ldarg,m.Parameters[0]);il.Emit(OpCodes.Callvirt,getEnumerator);il.Emit(OpCodes.Stloc,en);
        var begin=Instruction.Create(OpCodes.Br,loop);il.Append(begin);il.Append(body);il.Emit(OpCodes.Ldloc,en);il.Emit(OpCodes.Callvirt,current);il.Emit(OpCodes.Castclass,m.Parameters[0].ParameterType);il.Emit(OpCodes.Call,m);
        il.Append(loop);il.Emit(OpCodes.Callvirt,move);il.Emit(OpCodes.Brtrue,body);il.Emit(OpCodes.Leave,done);
        il.Append(cleanup);il.Emit(OpCodes.Isinst,dispose.DeclaringType);il.Emit(OpCodes.Stloc,disposable);il.Emit(OpCodes.Ldloc,disposable);il.Emit(OpCodes.Brfalse,endFinally);il.Emit(OpCodes.Ldloc,disposable);il.Emit(OpCodes.Callvirt,dispose);il.Append(endFinally);il.Append(done);
        m.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=begin,TryEnd=cleanup,HandlerStart=cleanup,HandlerEnd=done});
    }
}
