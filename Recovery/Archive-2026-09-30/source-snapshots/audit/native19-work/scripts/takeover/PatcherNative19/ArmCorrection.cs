using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
internal static partial class Program
{
    // Native 0x18035D86D-0x18035D91D: List<string>.GetEnumerator,
    // MoveNext, Current, HideSprite and Dispose in a finally clause.
    // The native Dispose pointer is shared and its nearest symbol is not its identity.
    static void CorrectArmEnumerator(ModuleDefinition mod,MethodDefinition m)
    {
        var ins=m.Body.Instructions;var entry=ins.Single(i=>i.Offset==0x4dc);
        if(entry.OpCode!=OpCodes.Ldstr||(string)entry.Operand!="Method not found @1808197F0")throw new Exception("Arm enumerator evidence shape changed");
        var oldReturn=ins.Single(i=>i.Offset==0x510);var oldLoop=ins.Single(i=>i.Offset==0x5b0);
        var enumType=(GenericInstanceType)((MethodReference)ins.Single(i=>i.Offset==0x506).Operand).DeclaringType;
        if(enumType.FullName!="System.Collections.Generic.List`1/Enumerator<System.String>")throw new Exception("Arm enumerator identity changed");
        var list=m.Body.Variables[8];var add=(MethodReference)ins.First(i=>i.Operand is MethodReference mr&&mr.Name=="Add"&&mr.DeclaringType.FullName==list.VariableType.FullName).Operand;
        var generic=add.Parameters[0].ParameterType;
        var enumReturn=new GenericInstanceType(enumType.ElementType);enumReturn.GenericArguments.Add(generic);
        var getEnumerator=new MethodReference("GetEnumerator",enumReturn,list.VariableType){HasThis=true};
        var move=new MethodReference("MoveNext",mod.TypeSystem.Boolean,enumType){HasThis=true};
        var current=new MethodReference("get_Current",generic,enumType){HasThis=true};
        var dispose=new MethodReference("Dispose",mod.TypeSystem.Void,enumType){HasThis=true};
        var remove=ins.Where(i=>i.Offset>0x4dc&&i.Offset<0x510||i.Offset>=0x5b0).ToArray();
        // Only the damaged loop refers to these removed instruction identities.
        foreach(var i in ins.Except(remove).Where(i=>i!=entry)){
            if(i.Operand is Instruction target&&remove.Contains(target))throw new Exception("unexpected external Arm loop edge");
            if(i.Operand is Instruction[] targets&&targets.Any(remove.Contains))throw new Exception("unexpected Arm switch edge");
        }
        foreach(var i in remove)ins.Remove(i);
        var e=Local(m,enumType);var il=m.Body.GetILProcessor();var start=Instruction.Create(OpCodes.Ldloc,list);var loop=Instruction.Create(OpCodes.Ldloca,e);var body=Instruction.Create(OpCodes.Ldarg_0);var cleanup=Instruction.Create(OpCodes.Ldloca,e);var done=Instruction.Create(OpCodes.Ret);
        entry.OpCode=OpCodes.Br;entry.Operand=start;
        il.Append(start);il.Emit(OpCodes.Callvirt,getEnumerator);il.Emit(OpCodes.Stloc,e);
        var begin=Instruction.Create(OpCodes.Br,loop);il.Append(begin);il.Append(body);il.Emit(OpCodes.Ldfld,m.DeclaringType.Fields.Single(f=>f.Name=="animationSprites"));il.Emit(OpCodes.Ldloca,e);il.Emit(OpCodes.Call,current);il.Emit(OpCodes.Call,MR(mod,"GlobalStaticVars","HideSprite",2));
        il.Append(loop);il.Emit(OpCodes.Call,move);il.Emit(OpCodes.Brtrue,body);il.Emit(OpCodes.Leave,done);
        il.Append(cleanup);il.Emit(OpCodes.Call,dispose);il.Emit(OpCodes.Endfinally);il.Append(done);
        m.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=begin,TryEnd=cleanup,HandlerStart=cleanup,HandlerEnd=done});
    }
}
