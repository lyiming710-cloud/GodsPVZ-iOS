using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static partial class Program
{
    static void CorrectArmorComparisons(MethodDefinition m,int expected)
    {
        int changed=0;var ins=m.Body.Instructions;
        for(int n=1;n+1<ins.Count;n++)if(ins[n-1].Operand is FieldReference f&&f.DeclaringType.FullName=="Zombie"&&new[]{"armor1Point","armor2Point"}.Contains(f.Name)&&f.FieldType.FullName=="System.Single"&&ins[n].OpCode==OpCodes.Ldc_I4&&(int)ins[n].Operand==0&&ins[n+1].OpCode==OpCodes.Cgt){ins[n].OpCode=OpCodes.Ldc_R4;ins[n].Operand=0f;changed++;}
        if(changed!=expected)throw new Exception("armor comparison shape drift "+m.FullName+" count="+changed);
    }
    // PC 0x180369580: the third previousPosition component is an animation y offset.
    static void CorrectStartPrevious(ModuleDefinition mod, MethodDefinition m)
    {
        Reset(m); var il=m.Body.GetILProcessor();
        var prev=m.DeclaringType.Fields.Single(f=>f.Name=="previousPosition");
        var x=FR(mod,"UnityEngine.Vector3","x");var y=FR(mod,"UnityEngine.Vector3","y");var z=FR(mod,"UnityEngine.Vector3","z");
        void Dest(FieldReference component){il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,prev);}
        foreach(var pair in new[]{("fX",x),("fY",y)}){Dest(pair.Item2);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,m.DeclaringType.Fields.Single(f=>f.Name==pair.Item1));il.Emit(OpCodes.Stfld,pair.Item2);}
        Dest(z);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,m.DeclaringType.Fields.Single(f=>f.Name=="animationGroup"));
        il.Emit(OpCodes.Callvirt,MR(mod,"UnityEngine.GameObject","get_transform",0));il.Emit(OpCodes.Callvirt,MR(mod,"UnityEngine.Transform","get_position",0));il.Emit(OpCodes.Ldfld,y);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,prev);il.Emit(OpCodes.Ldfld,y);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stfld,z);il.Emit(OpCodes.Ret);
    }

    // PC 0x18036C4B0: distinct root and animation positions; no defensive null skip.
    static void CorrectMove(ModuleDefinition mod, MethodDefinition m)
    {
        Reset(m);var il=m.Body.GetILProcessor();var t=m.DeclaringType;
        FieldDefinition F(string n)=>t.Fields.Single(f=>f.Name==n);
        var vx=FR(mod,"UnityEngine.Vector3","x");var vy=FR(mod,"UnityEngine.Vector3","y");
        var pos=Local(m,F("previousPosition").FieldType);
        var getTransform=MR(mod,"UnityEngine.Component","get_transform",0);var getPosition=MR(mod,"UnityEngine.Transform","get_position",0);var setPosition=MR(mod,"UnityEngine.Transform","set_position",1);
        var ret=Instruction.Create(OpCodes.Ret);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Call,t.Methods.Single(x=>x.Name=="GetMoveDirection"));il.Emit(OpCodes.Call,t.Methods.Single(x=>x.Name=="SetrSpeed"));
        foreach(var pair in new[]{("fX",vx),("fY",vy)}){
            il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F(pair.Item1));il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,F("rSpeed"));il.Emit(OpCodes.Ldfld,pair.Item2);
            il.Emit(OpCodes.Call,MR(mod,"UnityEngine.Time","get_deltaTime",0));il.Emit(OpCodes.Mul);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stfld,F(pair.Item1));
        }
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("fX"));il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("fY"));il.Emit(OpCodes.Call,t.Methods.Single(x=>x.Name=="TestPosition"&&x.Parameters.Count==2));
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("isDied"));il.Emit(OpCodes.Brtrue,ret);
        void ReadPosition(){il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Call,getTransform);il.Emit(OpCodes.Callvirt,getPosition);il.Emit(OpCodes.Stloc,pos);}
        ReadPosition();
        foreach(var pair in new[]{("fX",vx),("fY",vy)}){il.Emit(OpCodes.Ldloca,pos);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F(pair.Item1));il.Emit(OpCodes.Stfld,pair.Item2);}
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Call,getTransform);il.Emit(OpCodes.Ldloc,pos);il.Emit(OpCodes.Callvirt,setPosition);
        ReadPosition();
        il.Emit(OpCodes.Ldloca,pos);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("fX"));il.Emit(OpCodes.Stfld,vx);
        il.Emit(OpCodes.Ldloca,pos);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("fZ"));il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("fY"));il.Emit(OpCodes.Add);il.Emit(OpCodes.Stfld,vy);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("animationGroup"));il.Emit(OpCodes.Callvirt,MR(mod,"UnityEngine.GameObject","get_transform",0));il.Emit(OpCodes.Ldloc,pos);il.Emit(OpCodes.Callvirt,setPosition);il.Append(ret);
    }

    // PC 0x18036C720: preserve UI offsets and propagate root/shadow height deltas.
    static void CorrectPreviousUpdate(ModuleDefinition mod, MethodDefinition m)
    {
        Reset(m);var il=m.Body.GetILProcessor();var t=m.DeclaringType;
        FieldDefinition F(string n)=>t.Fields.Single(f=>f.Name==n);
        var prev=F("previousPosition");var x=FR(mod,"UnityEngine.Vector3","x");var y=FR(mod,"UnityEngine.Vector3","y");var z=FR(mod,"UnityEngine.Vector3","z");
        var dx=Local(m,mod.TypeSystem.Single);var dy=Local(m,mod.TypeSystem.Single);var dh=Local(m,mod.TypeSystem.Single);var ui=Local(m,prev.FieldType);
        var getTransform=MR(mod,"UnityEngine.Component","get_transform",0);var getPosition=MR(mod,"UnityEngine.Transform","get_position",0);var getGoTransform=MR(mod,"UnityEngine.GameObject","get_transform",0);
        void SelfComponent(FieldReference component){il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Call,getTransform);il.Emit(OpCodes.Callvirt,getPosition);il.Emit(OpCodes.Ldfld,component);}
        void PrevComponent(FieldReference component){il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,prev);il.Emit(OpCodes.Ldfld,component);}
        void Height(){il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("shadow"));il.Emit(OpCodes.Callvirt,getTransform);il.Emit(OpCodes.Callvirt,getPosition);il.Emit(OpCodes.Ldfld,y);SelfComponent(y);il.Emit(OpCodes.Sub);}
        SelfComponent(x);PrevComponent(x);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,dx);
        SelfComponent(y);PrevComponent(y);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,dy);
        Height();PrevComponent(z);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,dh);
        foreach(var name in new[]{"UI_HP","UI_Elements","UI_Buff"}){
            var field=F(name);var skip=Instruction.Create(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,field);il.Emit(OpCodes.Ldnull);il.Emit(OpCodes.Call,MR(mod,"UnityEngine.Object","op_Inequality",2));il.Emit(OpCodes.Brfalse,skip);
            il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,field);il.Emit(OpCodes.Callvirt,getGoTransform);il.Emit(OpCodes.Callvirt,getPosition);il.Emit(OpCodes.Stloc,ui);
            il.Emit(OpCodes.Ldloca,ui);il.Emit(OpCodes.Ldloc,ui);il.Emit(OpCodes.Ldfld,x);il.Emit(OpCodes.Ldloc,dx);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stfld,x);
            il.Emit(OpCodes.Ldloca,ui);il.Emit(OpCodes.Ldloc,ui);il.Emit(OpCodes.Ldfld,y);il.Emit(OpCodes.Ldloc,dy);il.Emit(OpCodes.Add);il.Emit(OpCodes.Ldloc,dh);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stfld,y);
            il.Emit(OpCodes.Ldloca,ui);SelfComponent(z);il.Emit(OpCodes.Stfld,z);
            il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,field);il.Emit(OpCodes.Callvirt,getGoTransform);il.Emit(OpCodes.Ldloc,ui);il.Emit(OpCodes.Callvirt,MR(mod,"UnityEngine.Transform","set_position",1));il.Append(skip);
        }
        foreach(var component in new[]{x,y,z}){il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,prev);if(component==z)Height();else SelfComponent(component);il.Emit(OpCodes.Stfld,component);}
        il.Emit(OpCodes.Ret);
    }

    static void CorrectAshe(ModuleDefinition mod, MethodDefinition m)
    {
        Reset(m);var il=m.Body.GetILProcessor();var t=m.DeclaringType;FieldDefinition F(string n)=>t.Fields.Single(f=>f.Name==n);
        var v=Local(m,F("previousPosition").FieldType);var skipHp=Instruction.Create(OpCodes.Nop);var nut=Instruction.Create(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("hpUIController"));il.Emit(OpCodes.Call,MR(mod,"UnityEngine.Object","op_Implicit",1));il.Emit(OpCodes.Brfalse,skipHp);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("hpUIController"));il.Emit(OpCodes.Callvirt,MR(mod,"HPUIController_Zombie","Ashe",0));il.Append(skipHp);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Call,MR(mod,"UnityEngine.Component","get_transform",0));il.Emit(OpCodes.Callvirt,MR(mod,"UnityEngine.Transform","get_position",0));il.Emit(OpCodes.Stloc,v);
        il.Emit(OpCodes.Ldloca,v);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("fZ"));il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("fY"));il.Emit(OpCodes.Add);il.Emit(OpCodes.Ldc_R4,134f);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stfld,FR(mod,"UnityEngine.Vector3","y"));
        il.Emit(OpCodes.Ldarg,m.Parameters[0]);il.Emit(OpCodes.Ldfld,FR(mod,"Damage","damagePoint"));il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Ldloc,v);il.Emit(OpCodes.Call,MR(mod,"DamageText","CreatDamageText",3));il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,F("nutZombie_hotNut"));il.Emit(OpCodes.Brtrue,nut);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Call,t.Methods.Single(x=>x.Name=="Die"&&x.Parameters.Count==2));il.Emit(OpCodes.Ret);
        il.Append(nut);il.Emit(OpCodes.Call,t.Methods.Single(x=>x.Name=="ZC_NutBoom"));il.Emit(OpCodes.Ret);
    }

    static void CorrectDieDispatch(ModuleDefinition mod, MethodDefinition m)
    {
        var instructions=m.Body.Instructions;var id=m.DeclaringType.Fields.Single(f=>f.Name=="ID");
        // Match both damaged PC dispatch blocks by their exact variable identities.
        foreach(var spec in new[]{(31,29,16),(32,30,19)}){
            int start=instructions.Select((i,n)=>(i,n)).Single(x=>x.i.OpCode==OpCodes.Stloc&&x.i.Operand==m.Body.Variables[spec.Item1]).n-1;
            if(instructions[start].OpCode!=OpCodes.Ldc_I8||(long)instructions[start].Operand!=0x180000000L)throw new Exception("Die dispatch base changed");
            int finish=instructions.Select((i,n)=>(i,n)).Single(x=>x.i.OpCode==OpCodes.Stloc&&x.i.Operand==m.Body.Variables[spec.Item2]).n+3;
            var fallback=instructions[finish];
            var guardStore=instructions.First(i=>i.OpCode==OpCodes.Stloc&&i.Operand==m.Body.Variables[spec.Item3]);
            int gi=instructions.IndexOf(guardStore);var guard=instructions[gi-4];
            if(guard.OpCode!=OpCodes.Ldarg_0||!(instructions[gi-3].Operand is FieldReference f)||f.Name!="isDied")throw new Exception("Die guard shape changed");
            var block=instructions.Skip(start).Take(finish-start).ToArray();
            foreach(var i in block){i.OpCode=OpCodes.Nop;i.Operand=null;}
            block[0].OpCode=OpCodes.Ldarg_0;block[1].OpCode=OpCodes.Ldfld;block[1].Operand=id;
            block[2].OpCode=OpCodes.Switch;block[2].Operand=new[]{guard,fallback,guard,fallback,guard,guard};
            // Preserve all instruction identities, including any branch/EH targets.
        }
        // Native comparisons are unsigned (ja/jbe), including negative IDs.
        for(int i=0;i<instructions.Count;i++)if(instructions[i].OpCode==OpCodes.Cgt&&i>0&&instructions[i-1].OpCode==OpCodes.Ldc_I4&&new[]{1,5}.Contains((int)instructions[i-1].Operand))instructions[i].OpCode=OpCodes.Cgt_Un;
    }

    // PC 0x18035ECB5-0x18035ECEA: ID17 particle is (fX, fZ+fY, 0).
    static void CorrectDieParticlePosition(ModuleDefinition mod,MethodDefinition m)
    {
        var setter=m.Body.Instructions.Single(i=>i.Operand is MethodReference mr&&mr.Name=="set_position"&&i.Previous?.Operand is MethodReference before&&before.Name=="get_position");
        var transform=setter.Previous.Previous;var position=setter.Previous;
        if(!(transform.Operand is MethodReference getter)||getter.Name!="get_transform"||transform.Previous.OpCode!=OpCodes.Ldarg_0)throw new Exception("Die ID17 position shape drift");
        FieldDefinition F(string name)=>m.DeclaringType.Fields.Single(f=>f.Name==name);
        transform.OpCode=OpCodes.Ldfld;transform.Operand=F("fX");position.OpCode=OpCodes.Ldarg_0;position.Operand=null;
        var il=m.Body.GetILProcessor();foreach(var i in new[]{Instruction.Create(OpCodes.Ldfld,F("fZ")),Instruction.Create(OpCodes.Ldarg_0),Instruction.Create(OpCodes.Ldfld,F("fY")),Instruction.Create(OpCodes.Add),Instruction.Create(OpCodes.Ldc_R4,0f),Instruction.Create(OpCodes.Newobj,MR(mod,"UnityEngine.Vector3",".ctor",3))})il.InsertBefore(setter,i);
        // Native 0x18035EBF2 writes adjacent isDied/ashes bytes together.
        // A CIL bool field store cannot stand in for this two-byte native store.
        var packed=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldc_I4&&(int)i.Operand==257);
        if(!(packed.Next.Operand is FieldReference flag)||flag.Name!="isDied")throw new Exception("Die packed flags drift");
        packed.Operand=1;var after=packed.Next;
        foreach(var i in new[]{Instruction.Create(OpCodes.Ldarg_0),Instruction.Create(OpCodes.Ldc_I4_1),Instruction.Create(OpCodes.Stfld,F("ashes"))}){il.InsertAfter(after,i);after=i;}
    }
}
