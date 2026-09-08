using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF20Patch <input.dll> <output.dll>");
    return 2;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });

IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in All(t.NestedTypes)) yield return n;
    }
}
var types = All(module.Types).ToList();
TypeDefinition T(string n) => types.Single(t => t.Name == n || t.FullName == n);
FieldDefinition F(TypeDefinition t, string n) => t.Fields.Single(f => f.Name == n);
MethodDefinition M(TypeDefinition t, string n, int pc)
{
    var q = t.Methods.Where(m => m.Name == n && m.Parameters.Count == pc).ToList();
    return q.Count == 1 ? q[0] : throw new InvalidDataException($"Expected one {t.FullName}.{n}/{pc}, got {q.Count}");
}
var refs = types.SelectMany(t => t.Methods).Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>().ToList();
MethodReference R(string decl, string name, params string[] ps)
{
    var q = refs.Where(r => r.DeclaringType.FullName == decl && r.Name == name && r.Parameters.Count == ps.Length)
        .Where(r => r.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(ps)).ToList();
    if (q.Count == 0) throw new InvalidDataException($"Missing MethodRef {decl}::{name}({string.Join(',', ps)})");
    return q[0];
}
MethodReference MR(string name, TypeReference ret, TypeReference decl, params TypeReference[] ps)
{
    var r = new MethodReference(name, ret, decl) { HasThis = true };
    foreach (var p in ps) r.Parameters.Add(new ParameterDefinition(p));
    return r;
}
GenericInstanceMethod G(MethodReference m, TypeReference arg)
{
    var g = new GenericInstanceMethod(m); g.GenericArguments.Add(arg); return g;
}
void E(ILProcessor il, OpCode op, object? value = null)
{
    Instruction ins = value switch
    {
        null => Instruction.Create(op),
        int i => Instruction.Create(op, i),
        float f => Instruction.Create(op, f),
        string s => Instruction.Create(op, s),
        MethodReference mr => Instruction.Create(op, mr),
        FieldReference fr => Instruction.Create(op, fr),
        TypeReference tr => Instruction.Create(op, tr),
        VariableDefinition vd => Instruction.Create(op, vd),
        ParameterDefinition pd => Instruction.Create(op, pd),
        Instruction target => Instruction.Create(op, target),
        _ => throw new NotSupportedException(value.GetType().FullName)
    };
    il.Append(ins);
}
MethodBody B(MethodDefinition m, bool locals = true, int stack = 8)
{
    var b = new MethodBody(m) { InitLocals = locals, MaxStackSize = stack }; m.Body = b; return b;
}
void ThrowNre(ILProcessor il, MethodReference nreCtor) { E(il, OpCodes.Newobj, nreCtor); E(il, OpCodes.Throw); }

var attackRange = T("AttackRange");
var damage = T("Damage");
var element = T("Element");
var plant = T("Plant");
var projectile = T("Projectile");
var zombie = T("Zombie");
var device = T("Device");
var skill = T("Skill");

var newCircle3 = M(attackRange, "NewCircleRange", 3);
var newCircle4 = M(attackRange, "NewCircleRange", 4);
var addElement = M(damage, "AddElement", 1);
var elementCtor = M(element, ".ctor", 4);
var getAtk = M(plant, "GetATK", 0);
var getDamageRange = M(plant, "GetDamageRange", 4);
var getDamage3 = M(plant, "GetDamage", 3);

var expected = new Dictionary<MethodDefinition,uint>
{
    [newCircle3]=0x060000CF,
    [newCircle4]=0x060000D0,
    [addElement]=0x0600010B,
    [elementCtor]=0x0600011C,
    [getAtk]=0x06000367,
    [getDamage3]=0x0600036D,
    [getDamageRange]=0x06000371,
};
foreach (var kv in expected)
    if (kv.Key.MetadataToken.ToUInt32()!=kv.Value) throw new InvalidDataException($"HF20 token drift {kv.Key.FullName}: 0x{kv.Key.MetadataToken.ToUInt32():X8}");

var arTransform=F(attackRange,"transform"); var arPlant=F(attackRange,"plant"); var arZombie=F(attackRange,"zombie");
var arProjectile=F(attackRange,"projectile"); var arDevice=F(attackRange,"device"); var arRangeType=F(attackRange,"rangeType");
var arRadius=F(attackRange,"radius"); var arCenters=F(attackRange,"centers");
var dDamagePoint=F(damage,"damagePoint"); var dCamp=F(damage,"camp"); var dAttr=F(damage,"damageAttribute");
var dAcrossArmor2=F(damage,"acrossArmor2"); var dAntiAir=F(damage,"anti_air_able"); var dAntiSub=F(damage,"anti_submarine_able");
var dNoResidue=F(damage,"noResidue"); var dRange=F(damage,"attackRange"); var dPen=F(damage,"penetrationVigour");
var dElements=F(damage,"elements"); var dSourcePlant=F(damage,"sourcePlant"); var dBoard=F(damage,"board"); var dStiff=F(damage,"stiffnessTime");
var eDamage=F(element,"damage"); var eType=F(element,"type"); var ePoint=F(element,"point"); var eShuttle=F(element,"shuttle_able");
var pID=F(plant,"ID"); var pCamp=F(plant,"camp"); var pAttackPoint=F(plant,"attackPoint"); var pBuffManager=F(plant,"buffManager");
var pOrder=F(plant,"order"); var pElementLv=F(plant,"elementLv"); var pSkill=F(plant,"skill"); var pBoard=F(plant,"board");
var pRangeUI=F(plant,"attackRangeUIController"); var pTargetPosition=F(plant,"targetPosition");
var sID=F(skill,"ID");

var attackRangeCtor=M(attackRange,".ctor",0); var damageCtor=M(damage,".ctor",0);
var manager=T("BuffManager"); var getIncrement=M(manager,"GetIncrement",2);
var rangeUI=T("AttackRangeUIController"); var uiGetRange=M(rangeUI,"GetAttackRange",1);

var objectCtor=R("System.Object",".ctor");
var nreCtor=R("System.NullReferenceException",".ctor");
var getTransform=R("UnityEngine.Component","get_transform");
var getPosition=R("UnityEngine.Transform","get_position");
var objImplicit=R("UnityEngine.Object","op_Implicit","UnityEngine.Object");
var objEquality=R("UnityEngine.Object","op_Equality","UnityEngine.Object","UnityEngine.Object");
var objInequality=R("UnityEngine.Object","op_Inequality","UnityEngine.Object","UnityEngine.Object");
var vector3Type=getPosition.ReturnType; var vecSub=new MethodReference("op_Subtraction",vector3Type,vector3Type){HasThis=false}; vecSub.Parameters.Add(new ParameterDefinition(vector3Type)); vecSub.Parameters.Add(new ParameterDefinition(vector3Type));
var mathMax=R("System.Math","Max","System.Single","System.Single");
var mathFType=new TypeReference("System","MathF",module,module.TypeSystem.CoreLibrary,false);
var mathFRound=new MethodReference("Round",module.TypeSystem.Single,mathFType){HasThis=false}; mathFRound.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));

var radiusList=(GenericInstanceType)arRadius.FieldType; var centersList=(GenericInstanceType)arCenters.FieldType; var elementList=(GenericInstanceType)dElements.FieldType;
var addFloat=MR("Add",module.TypeSystem.Void,radiusList,module.TypeSystem.Single);
var vector3=centersList.GenericArguments[0];
var addVector=MR("Add",module.TypeSystem.Void,centersList,vector3);
var addElementList=MR("Add",module.TypeSystem.Void,elementList,element);
var getEnumElement=refs.FirstOrDefault(r=>r.Name=="GetEnumerator" && r.DeclaringType.FullName==elementList.FullName && r.Parameters.Count==0)
    ?? throw new InvalidDataException("HF20 missing List<Element>.GetEnumerator reference");
var enumElementType=getEnumElement.ReturnType;
var enumMoveNext=new MethodReference("MoveNext",module.TypeSystem.Boolean,enumElementType){HasThis=true};
var enumCurrent=new MethodReference("get_Current",element,enumElementType){HasThis=true};
var enumDispose=new MethodReference("Dispose",module.TypeSystem.Void,enumElementType){HasThis=true};

PatchNewCircle4();
PatchNewCircle3();
PatchElementCtor();
PatchAddElement();
PatchGetATK();
PatchGetDamageRange();
PatchGetDamage();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output,new WriterParameters{WriteSymbols=false});

using (var verify=ModuleDefinition.ReadModule(output,new ReaderParameters{InMemory=true,ReadSymbols=false}))
{
    var vt=All(verify.Types).ToList();
    MethodDefinition VM(string tn,string mn,int pc)=>vt.Single(t=>t.Name==tn).Methods.Single(m=>m.Name==mn&&m.Parameters.Count==pc);
    var checks=new[]{
        ("AttackRange.NewCircleRange/3",VM("AttackRange","NewCircleRange",3),8,0),
        ("AttackRange.NewCircleRange/4",VM("AttackRange","NewCircleRange",4),30,0),
        ("Element..ctor",VM("Element",".ctor",4),10,0),
        ("Damage.AddElement",VM("Damage","AddElement",1),30,1),
        ("Plant.GetATK",VM("Plant","GetATK",0),8,0),
        ("Plant.GetDamageRange",VM("Plant","GetDamageRange",4),70,0),
        ("Plant.GetDamage/3",VM("Plant","GetDamage",3),170,0),
    };
    foreach(var (name,m,min,eh) in checks)
    {
        if(!m.HasBody||m.Body.Instructions.Count<min) throw new InvalidDataException($"HF20 {name} body too small: {m.Body.Instructions.Count}");
        if(m.Body.ExceptionHandlers.Count!=eh) throw new InvalidDataException($"HF20 {name} EH mismatch {m.Body.ExceptionHandlers.Count}");
        if(m.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>().Any(r=>r.DeclaringType.FullName.Contains("Cpp2ILHelpers",StringComparison.Ordinal)))
            throw new InvalidDataException($"HF20 {name} still contains Cpp2IL helper");
        Console.WriteLine($"VERIFY {name}: token=0x{m.MetadataToken.ToUInt32():X8}, IL={m.Body.Instructions.Count}, bytes={m.Body.CodeSize}, EH={m.Body.ExceptionHandlers.Count}");
    }
    var vatk=VM("Plant","GetATK",0);
    if(vatk.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>().Count(r=>r.DeclaringType.FullName=="System.MathF"&&r.Name=="Round")!=1)
        throw new InvalidDataException("HF20 GetATK must contain exactly one MathF.Round");
    var var4=VM("AttackRange","NewCircleRange",4);
    if(var4.Body.Instructions.Count(i=>i.OpCode==OpCodes.Isinst)!=4) throw new InvalidDataException("HF20 NewCircleRange/4 must have four host type gates");
    var vae=VM("Damage","AddElement",1);
    if(vae.Body.ExceptionHandlers.Count!=1||vae.Body.ExceptionHandlers[0].HandlerType!=ExceptionHandlerType.Finally) throw new InvalidDataException("HF20 AddElement foreach-finally missing");
    var vgdr=VM("Plant","GetDamageRange",4);
    foreach(var c in new[]{80f,155f,160f,195f,225f}) if(!vgdr.Body.Instructions.Any(i=>i.OpCode==OpCodes.Ldc_R4&&i.Operand is float f&&f==c)) throw new InvalidDataException($"HF20 GetDamageRange missing radius {c}");
    var vgd=VM("Plant","GetDamage",3);
    foreach(var c in new[]{0.2f,8f,1.2f,2.8f,0.3f,1.15f,2.5f,4.5f,1.4f}) if(!vgd.Body.Instructions.Any(i=>i.OpCode==OpCodes.Ldc_R4&&i.Operand is float f&&f==c)) throw new InvalidDataException($"HF20 GetDamage missing constant {c}");
    var gdrCall=vgd.Body.Instructions.Select((ins,idx)=>(ins,idx)).Single(x=>x.ins.Operand is MethodReference r&&r.Name=="GetDamageRange"&&r.DeclaringType.Name=="Plant");
    if(!vgd.Body.Instructions.Skip(Math.Max(0,gdrCall.idx-5)).Take(5).Any(i=>i.OpCode==OpCodes.Ldarg_3)) throw new InvalidDataException("HF20 GetDamage must forward specialType into GetDamageRange");
}

Console.WriteLine("HF20 Plant damage pipeline restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchNewCircle3()
{
    var b=B(newCircle3,true,8); var il=b.GetILProcessor();
    var center=new VariableDefinition(vector3); b.Variables.Add(center);
    E(il,OpCodes.Ldloca,center); E(il,OpCodes.Initobj,vector3);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldloc,center);
    E(il,OpCodes.Call,G(newCircle4,newCircle3.GenericParameters[0])); E(il,OpCodes.Ret);
}

void PatchNewCircle4()
{
    var b=B(newCircle4,true,8); var il=b.GetILProcessor();
    var ar=new VariableDefinition(attackRange); b.Variables.Add(ar);
    var vz=new VariableDefinition(zombie); var vp=new VariableDefinition(plant); var vpr=new VariableDefinition(projectile); var vd=new VariableDefinition(device);
    b.Variables.Add(vz); b.Variables.Add(vp); b.Variables.Add(vpr); b.Variables.Add(vd);
    E(il,OpCodes.Newobj,attackRangeCtor); E(il,OpCodes.Stloc,ar);
    E(il,OpCodes.Ldloc,ar); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Stfld,arTransform);
    E(il,OpCodes.Ldloc,ar); E(il,OpCodes.Ldfld,arRadius); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Callvirt,addFloat);
    E(il,OpCodes.Ldloc,ar); E(il,OpCodes.Ldfld,arCenters); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Callvirt,addVector);
    E(il,OpCodes.Ldloc,ar); E(il,OpCodes.Ldc_I4_2); E(il,OpCodes.Stfld,arRangeType);
    void Gate(TypeReference target,VariableDefinition local,FieldReference field)
    {
        var next=Instruction.Create(OpCodes.Nop);
        E(il,OpCodes.Ldarg_0); E(il,OpCodes.Box,newCircle4.GenericParameters[0]); E(il,OpCodes.Isinst,target); E(il,OpCodes.Stloc,local);
        E(il,OpCodes.Ldloc,local); E(il,OpCodes.Brfalse,next);
        E(il,OpCodes.Ldloc,ar); E(il,OpCodes.Ldloc,local); E(il,OpCodes.Stfld,field); il.Append(next);
    }
    Gate(zombie,vz,arZombie); Gate(plant,vp,arPlant); Gate(projectile,vpr,arProjectile); Gate(device,vd,arDevice);
    E(il,OpCodes.Ldloc,ar); E(il,OpCodes.Ret);
}

void PatchElementCtor()
{
    var b=B(elementCtor,false,4); var il=b.GetILProcessor();
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Call,objectCtor);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Stfld,ePoint);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Stfld,eType);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Stfld,eDamage);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldarg,elementCtor.Parameters[3]); E(il,OpCodes.Stfld,eShuttle);
    E(il,OpCodes.Ret);
}

void PatchAddElement()
{
    var b=B(addElement,true,5); var il=b.GetILProcessor();
    var en=new VariableDefinition(enumElementType); var cur=new VariableDefinition(element); b.Variables.Add(en); b.Variables.Add(cur);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,dElements); E(il,OpCodes.Callvirt,getEnumElement); E(il,OpCodes.Stloc,en);
    var tryStart=Instruction.Create(OpCodes.Nop); il.Append(tryStart);
    var loopCheck=Instruction.Create(OpCodes.Ldloca,en); E(il,OpCodes.Br,loopCheck);
    var loopBody=Instruction.Create(OpCodes.Ldloca,en); il.Append(loopBody); E(il,OpCodes.Call,enumCurrent); E(il,OpCodes.Stloc,cur);
    var curOk=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldloc,cur); E(il,OpCodes.Brtrue,curOk); ThrowNre(il,nreCtor); il.Append(curOk);
    var argOk=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Brtrue,argOk); ThrowNre(il,nreCtor); il.Append(argOk);
    var noMatch=Instruction.Create(OpCodes.Nop);
    E(il,OpCodes.Ldloc,cur); E(il,OpCodes.Ldfld,eType); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Ldfld,eType); E(il,OpCodes.Bne_Un,noMatch);
    E(il,OpCodes.Ldloc,cur); E(il,OpCodes.Ldloc,cur); E(il,OpCodes.Ldfld,ePoint); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Ldfld,ePoint); E(il,OpCodes.Add); E(il,OpCodes.Stfld,ePoint);
    var ret=Instruction.Create(OpCodes.Ret); E(il,OpCodes.Leave,ret); il.Append(noMatch);
    il.Append(loopCheck); E(il,OpCodes.Call,enumMoveNext); E(il,OpCodes.Brtrue,loopBody);
    var afterFinally=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Leave,afterFinally);
    var finallyStart=Instruction.Create(OpCodes.Ldloca,en); il.Append(finallyStart); E(il,OpCodes.Call,enumDispose); E(il,OpCodes.Endfinally);
    il.Append(afterFinally); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,dElements); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Callvirt,addElementList); E(il,OpCodes.Ret);
    il.Append(ret);
    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=tryStart,TryEnd=finallyStart,HandlerStart=finallyStart,HandlerEnd=afterFinally});
}

void PatchGetATK()
{
    var b=B(getAtk,false,4); var il=b.GetILProcessor();
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pBuffManager); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pAttackPoint); E(il,OpCodes.Ldstr,"Atk"); E(il,OpCodes.Callvirt,getIncrement);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pAttackPoint); E(il,OpCodes.Add); E(il,OpCodes.Call,mathFRound); E(il,OpCodes.Ldc_R4,0f); E(il,OpCodes.Call,mathMax); E(il,OpCodes.Ret);
}

void PatchGetDamageRange()
{
    var b=B(getDamageRange,true,8); var il=b.GetILProcessor(); var ar=new VariableDefinition(attackRange); var sk=new VariableDefinition(skill); b.Variables.Add(ar); b.Variables.Add(sk);
    var ret=Instruction.Create(OpCodes.Ret); var assign=Instruction.Create(OpCodes.Ldarg_1);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Brfalse,ret);
    E(il,OpCodes.Ldnull); E(il,OpCodes.Stloc,ar);
    var id4=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_2); E(il,OpCodes.Bne_Un,id4);
    var id2Null=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldnull); E(il,OpCodes.Call,objInequality); E(il,OpCodes.Brfalse,id2Null);
    var id2not28=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4,28); E(il,OpCodes.Bne_Un,id2not28);
    EmitCircleProjectile(155f,G(newCircle3,projectile)); E(il,OpCodes.Br,assign); il.Append(id2not28);
    var id2not10=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4,10); E(il,OpCodes.Bne_Un,id2not10); EmitCircleProjectile(160f,G(newCircle3,projectile)); E(il,OpCodes.Br,assign); il.Append(id2not10);
    var id2done=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4,11); E(il,OpCodes.Bne_Un,id2done); EmitCircleProjectile(160f,G(newCircle3,projectile)); il.Append(id2done); E(il,OpCodes.Br,assign);
    il.Append(id2Null); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pRangeUI); E(il,OpCodes.Ldc_I4_0); E(il,OpCodes.Callvirt,uiGetRange); E(il,OpCodes.Stloc,ar); E(il,OpCodes.Br,assign);
    il.Append(id4); var id49=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_4); E(il,OpCodes.Bne_Un,id49);
    var id4self=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Call,objImplicit); E(il,OpCodes.Brfalse,id4self); EmitCircleProjectile(160f,G(newCircle3,projectile)); E(il,OpCodes.Br,assign);
    il.Append(id4self); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Call,getTransform); E(il,OpCodes.Ldc_R4,225f); E(il,OpCodes.Call,G(newCircle3,plant)); E(il,OpCodes.Stloc,ar); E(il,OpCodes.Br,assign);
    il.Append(id49); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4,49); E(il,OpCodes.Bne_Un,assign);
    var skillPath=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4_0); E(il,OpCodes.Blt,skillPath);
    E(il,OpCodes.Ldarg_2); E(il,OpCodes.Call,objImplicit); E(il,OpCodes.Brfalse,assign);
    var p21=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4,29); E(il,OpCodes.Bne_Un,p21); EmitCircleProjectile(225f,G(newCircle3,projectile)); E(il,OpCodes.Br,assign);
    il.Append(p21); var p30=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4,30); E(il,OpCodes.Bne_Un,p30); EmitCircleProjectile(225f,G(newCircle3,projectile)); E(il,OpCodes.Br,assign);
    il.Append(p30); var projectileDone=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4,21); E(il,OpCodes.Bne_Un,projectileDone);
    var special1=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg,getDamageRange.Parameters[3]); E(il,OpCodes.Brtrue,special1); EmitCircleProjectile(80f,G(newCircle3,projectile)); E(il,OpCodes.Br,assign);
    il.Append(special1); E(il,OpCodes.Ldarg,getDamageRange.Parameters[3]); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Bne_Un,assign); EmitCircleProjectile(195f,G(newCircle3,projectile)); E(il,OpCodes.Br,assign);
    il.Append(projectileDone); E(il,OpCodes.Br,assign);
    il.Append(skillPath); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pSkill); E(il,OpCodes.Stloc,sk);
    var skill2=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldloc,sk); E(il,OpCodes.Ldfld,sID); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Bne_Un,skill2);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Call,getTransform); E(il,OpCodes.Ldc_R4,225f); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pTargetPosition); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Call,getTransform); E(il,OpCodes.Callvirt,getPosition); E(il,OpCodes.Call,vecSub); E(il,OpCodes.Call,G(newCircle4,plant)); E(il,OpCodes.Stloc,ar); E(il,OpCodes.Br,assign);
    il.Append(skill2); var skill3=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldloc,sk); E(il,OpCodes.Ldfld,sID); E(il,OpCodes.Ldc_I4_3); E(il,OpCodes.Bne_Un,skill3); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pRangeUI); E(il,OpCodes.Ldc_I4_3); E(il,OpCodes.Callvirt,uiGetRange); E(il,OpCodes.Stloc,ar); E(il,OpCodes.Br,assign); il.Append(skill3); E(il,OpCodes.Br,assign);
    il.Append(assign); E(il,OpCodes.Ldloc,ar); E(il,OpCodes.Stfld,dRange); il.Append(ret);

    void EmitCircleProjectile(float r,MethodReference gm)
    {
        E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Call,getTransform); E(il,OpCodes.Ldc_R4,r); E(il,OpCodes.Call,gm); E(il,OpCodes.Stloc,ar);
    }
}

void PatchGetDamage()
{
    var b=B(getDamage3,true,10); var il=b.GetILProcessor(); var d=new VariableDefinition(damage); var atk=new VariableDefinition(module.TypeSystem.Single); var el=new VariableDefinition(element); var sk=new VariableDefinition(skill); b.Variables.Add(d); b.Variables.Add(atk); b.Variables.Add(el); b.Variables.Add(sk);
    E(il,OpCodes.Newobj,damageCtor); E(il,OpCodes.Stloc,d); E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pCamp); E(il,OpCodes.Stfld,dCamp);
    var id1=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Brtrue,id1);
    SetBool(dAcrossArmor2); SetBool(dAntiAir); var attrsDone=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Br,attrsDone); il.Append(id1);
    var id2=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_2); E(il,OpCodes.Bne_Un,id2); SetInt(dAttr,3); SetBool(dAntiSub); SetBool(dNoResidue); var id2NoAir=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,28); E(il,OpCodes.Beq,id2NoAir); SetBool(dAntiAir); il.Append(id2NoAir); E(il,OpCodes.Br,attrsDone); il.Append(id2);
    var id3=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_3); E(il,OpCodes.Bne_Un,id3); SetInt(dAttr,0); E(il,OpCodes.Br,attrsDone); il.Append(id3);
    var id4=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_4); E(il,OpCodes.Bne_Un,id4); SetInt(dAttr,2); SetBool(dAntiAir); SetBool(dAntiSub); SetBool(dNoResidue); E(il,OpCodes.Br,attrsDone); il.Append(id4);
    var id5=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_5); E(il,OpCodes.Bne_Un,id5); var id5p20=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,19); E(il,OpCodes.Bne_Un,id5p20); SetInt(dAttr,1); E(il,OpCodes.Br,attrsDone); il.Append(id5p20); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,20); E(il,OpCodes.Bne_Un,attrsDone); SetInt(dAttr,1); SetBool(dAntiAir); E(il,OpCodes.Br,attrsDone); il.Append(id5);
    var id7=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_7); E(il,OpCodes.Bne_Un,id7); SetInt(dAttr,1); SetBool(dAntiAir); E(il,OpCodes.Br,attrsDone); il.Append(id7);
    var not49=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4,49); E(il,OpCodes.Bne_Un,not49);
    var skillAttr=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4_0); E(il,OpCodes.Blt,skillAttr);
    var p23=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,21); E(il,OpCodes.Bne_Un,p23); SetInt(dAttr,2); var p21Done=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Bne_Un,p21Done); SetBool(dAntiAir); il.Append(p21Done); E(il,OpCodes.Br,attrsDone);
    il.Append(p23); var p2930=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,23); E(il,OpCodes.Bne_Un,p2930); SetInt(dAttr,0); E(il,OpCodes.Br,attrsDone); il.Append(p2930);
    var p30=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,29); E(il,OpCodes.Bne_Un,p30); SetInt(dAttr,2); SetBool(dAntiAir); SetBool(dAntiSub); E(il,OpCodes.Br,attrsDone); il.Append(p30);
    var pOther=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,30); E(il,OpCodes.Bne_Un,pOther); SetInt(dAttr,2); SetBool(dAntiAir); SetBool(dAntiSub); E(il,OpCodes.Br,attrsDone); il.Append(pOther); E(il,OpCodes.Br,attrsDone);
    il.Append(skillAttr); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pSkill); E(il,OpCodes.Stloc,sk);
    var s2=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldloc,sk); E(il,OpCodes.Ldfld,sID); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Bne_Un,s2); var s1special=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Beq,s1special); SetInt(dAttr,0); E(il,OpCodes.Br,attrsDone); il.Append(s1special); SetInt(dAttr,2); SetBool(dAntiAir); SetBool(dAntiSub); E(il,OpCodes.Br,attrsDone);
    il.Append(s2); var s3=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldloc,sk); E(il,OpCodes.Ldfld,sID); E(il,OpCodes.Ldc_I4_2); E(il,OpCodes.Bne_Un,s3); SetInt(dAttr,0); SetBool(dAntiAir); E(il,OpCodes.Br,attrsDone); il.Append(s3); var sdone=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldloc,sk); E(il,OpCodes.Ldfld,sID); E(il,OpCodes.Ldc_I4_3); E(il,OpCodes.Bne_Un,sdone); SetInt(dAttr,0); il.Append(sdone); E(il,OpCodes.Br,attrsDone);
    il.Append(not49); il.Append(attrsDone);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Call,getDamageRange);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Call,getAtk); E(il,OpCodes.Stloc,atk);
    var afterMult=Instruction.Create(OpCodes.Nop);
    var multId4=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_2); E(il,OpCodes.Bne_Un,multId4); var id2NoMul=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,28); E(il,OpCodes.Bne_Un,id2NoMul); MulAtk(0.2f); il.Append(id2NoMul); E(il,OpCodes.Br,afterMult); il.Append(multId4);
    var multId5=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_4); E(il,OpCodes.Bne_Un,multId5); var id4NoMul=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Ldnull); E(il,OpCodes.Call,objEquality); E(il,OpCodes.Brfalse,id4NoMul); MulAtk(8f); il.Append(id4NoMul); E(il,OpCodes.Br,afterMult); il.Append(multId5);
    var multId7=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_5); E(il,OpCodes.Bne_Un,multId7); var id5Done=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Call,objImplicit); E(il,OpCodes.Brfalse,id5Done); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,20); E(il,OpCodes.Bne_Un,id5Done); MulAtk(1.2f); SetFloat(dPen,15f); il.Append(id5Done); E(il,OpCodes.Br,afterMult); il.Append(multId7);
    var mult49=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_7); E(il,OpCodes.Bne_Un,mult49); var id7Done=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Call,objImplicit); E(il,OpCodes.Brfalse,id7Done); SetFloat(dPen,20f); il.Append(id7Done); E(il,OpCodes.Br,afterMult); il.Append(mult49);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4,49); E(il,OpCodes.Bne_Un,afterMult); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4_0); E(il,OpCodes.Blt,afterMult); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Call,objImplicit); E(il,OpCodes.Brfalse,afterMult);
    var p21mul=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,30); E(il,OpCodes.Bne_Un,p21mul); MulAtk(2.8f); E(il,OpCodes.Br,afterMult); il.Append(p21mul); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,21); E(il,OpCodes.Bne_Un,afterMult); var sp1=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Brtrue,sp1); MulAtk(0.3f); E(il,OpCodes.Br,afterMult); il.Append(sp1); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Bne_Un,afterMult); MulAtk(1.15f);
    il.Append(afterMult); E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldloc,atk); E(il,OpCodes.Stfld,dDamagePoint);
    var afterElement=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_5); E(il,OpCodes.Bne_Un,afterElement); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4_0); E(il,OpCodes.Blt,afterElement);
    E(il,OpCodes.Ldc_I4_0); E(il,OpCodes.Ldc_R4,0f); E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Newobj,elementCtor); E(il,OpCodes.Stloc,el);
    var ep19=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Bne_Un,ep19); E(il,OpCodes.Ldloc,el); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Call,getAtk); E(il,OpCodes.Ldc_R4,2.5f); E(il,OpCodes.Mul); E(il,OpCodes.Stfld,ePoint); var scaleElement=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Br,scaleElement); il.Append(ep19);
    var ep20=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,19); E(il,OpCodes.Bne_Un,ep20); E(il,OpCodes.Ldloc,el); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Call,getAtk); E(il,OpCodes.Stfld,ePoint); E(il,OpCodes.Br,scaleElement); il.Append(ep20);
    var eOther=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,20); E(il,OpCodes.Bne_Un,eOther); E(il,OpCodes.Ldloc,el); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Call,getAtk); E(il,OpCodes.Ldc_R4,4.5f); E(il,OpCodes.Mul); E(il,OpCodes.Stfld,ePoint); il.Append(eOther);
    il.Append(scaleElement); var addEl=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pOrder); E(il,OpCodes.Ldc_I4_0); E(il,OpCodes.Ble,addEl); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pElementLv); E(il,OpCodes.Ldc_I4_0); E(il,OpCodes.Ble,addEl); E(il,OpCodes.Ldloc,el); E(il,OpCodes.Ldloc,el); E(il,OpCodes.Ldfld,ePoint); E(il,OpCodes.Ldc_R4,1.4f); E(il,OpCodes.Mul); E(il,OpCodes.Stfld,ePoint); il.Append(addEl); E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldloc,el); E(il,OpCodes.Callvirt,addElement); il.Append(afterElement);
    var stiff49=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4_4); E(il,OpCodes.Bne_Un,stiff49); var afterStiff=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pOrder); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Blt,afterStiff); var stiffProjectile=Instruction.Create(OpCodes.Nop); E(il,OpCodes.Ldarg_1); E(il,OpCodes.Call,objImplicit); E(il,OpCodes.Brtrue,stiffProjectile); SetFloat(dStiff,5f); E(il,OpCodes.Br,afterStiff); il.Append(stiffProjectile); SetFloat(dStiff,1f); E(il,OpCodes.Br,afterStiff); il.Append(stiff49);
    E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pID); E(il,OpCodes.Ldc_I4,49); E(il,OpCodes.Bne_Un,afterStiff); E(il,OpCodes.Ldarg_2); E(il,OpCodes.Ldc_I4,21); E(il,OpCodes.Bne_Un,afterStiff); E(il,OpCodes.Ldarg_3); E(il,OpCodes.Brtrue,afterStiff); SetFloat(dStiff,0.5f); il.Append(afterStiff);
    E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Stfld,dSourcePlant); E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldarg_0); E(il,OpCodes.Ldfld,pBoard); E(il,OpCodes.Stfld,dBoard); E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ret);

    void SetBool(FieldReference f){E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldc_I4_1); E(il,OpCodes.Stfld,f);} void SetInt(FieldReference f,int v){E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldc_I4,v); E(il,OpCodes.Stfld,f);} void SetFloat(FieldReference f,float v){E(il,OpCodes.Ldloc,d); E(il,OpCodes.Ldc_R4,v); E(il,OpCodes.Stfld,f);} void MulAtk(float v){E(il,OpCodes.Ldloc,atk); E(il,OpCodes.Ldc_R4,v); E(il,OpCodes.Mul); E(il,OpCodes.Stloc,atk);}
}
