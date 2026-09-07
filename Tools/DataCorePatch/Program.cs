using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.DataCorePatch <input.dll> <output.dll>");
    return 2;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });

var save = T("Save");
var plantSave = T("PlantSave");
var propSave = T("PropSave");
var supplies = T("Supplies");
var skill = T("Skill");
var savesManager = T("SavesManager");
var skillManager = T("SkillManager");
var resourceManager = T("ResourceManager");

var fPlantSaves = F(save, "plantSaves");
var fPropSaves = F(save, "propSaves");
var fSupplies = F(save, "supplies");
var fPlantId = F(plantSave, "ID");
var fPropId = F(propSave, "ID");
var fSupplyId = F(supplies, "ID");
var fSupplyQty = F(supplies, "quantity");
var fSkillId = F(skill, "ID");
var fSkillUnlock = F(skill, "unlock");
var fSkill1 = F(plantSave, "skill1");
var fSkill2 = F(plantSave, "skill2");
var fSkill3 = F(plantSave, "skill3");
var fPsId = F(plantSave, "ID");

PatchGetPlantSave(M(save, "GetPlantSave", 1));
PatchGetPropSave(M(save, "GetPropSave", 1));
PatchGetSupplies(M(save, "GetSupplies", 1));
PatchGetSkillId(M(plantSave, "GetSkill_ID", 1));
PatchSkillCount(M(plantSave, "GetSkillNum", 0), unlockedOnly: false);
PatchSkillCount(M(plantSave, "GetSkillNum_Unlocked", 0), unlockedOnly: true);
PatchGetSkillData(M(savesManager, "GetSkillData", 3));
PatchGetSkillDataAll(M(savesManager, "GetSkillData_All", 2));

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });
using var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false });
foreach (var spec in new[]
{
    ("Save","GetPlantSave",1), ("Save","GetPropSave",1), ("Save","GetSupplies",1),
    ("PlantSave","GetSkill_ID",1), ("PlantSave","GetSkillNum",0), ("PlantSave","GetSkillNum_Unlocked",0),
    ("SavesManager","GetSkillData",3), ("SavesManager","GetSkillData_All",2)
})
{
    var mm = FindType(verify, spec.Item1).Methods.Single(x => x.Name == spec.Item2 && x.Parameters.Count == spec.Item3);
    if (!mm.HasBody || mm.Body.Instructions.Count < 3) throw new InvalidOperationException($"trivial body after write: {spec}");
    Console.WriteLine($"VERIFY {spec.Item1}.{spec.Item2}: {mm.Body.Instructions.Count} IL instructions");
}
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchGetPlantSave(MethodDefinition m)
{
    var listType = (GenericInstanceType)fPlantSaves.FieldType;
    var count = ListMethod(listType, "get_Count", module.TypeSystem.Int32);
    var item = ListMethod(listType, "get_Item", plantSave, module.TypeSystem.Int32);
    var add = ListMethod(listType, "Add", module.TypeSystem.Void, plantSave);
    var insert = ListMethod(listType, "Insert", module.TypeSystem.Void, module.TypeSystem.Int32, plantSave);
    var body = Fresh(m, true); var il = body.GetILProcessor();
    var i = new VariableDefinition(module.TypeSystem.Int32); body.Variables.Add(i);
    var cur = new VariableDefinition(plantSave); body.Variables.Add(cur);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var next = Instruction.Create(OpCodes.Nop);
    var addEnd = Instruction.Create(OpCodes.Nop);
    var done = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Brfalse, done);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlantSaves); E(il, OpCodes.Brfalse, done);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlantSaves); E(il, OpCodes.Callvirt, count); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Bgt, loopCheck);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlantSaves); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Callvirt, add); E(il, OpCodes.Br, done);
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stloc, i);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlantSaves); E(il, OpCodes.Callvirt, count); E(il, OpCodes.Bge, addEnd);
    il.Append(loopBody);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlantSaves); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Callvirt, item); E(il, OpCodes.Stloc, cur);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Brfalse, next);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Ldfld, fPlantId); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, fPlantId); E(il, OpCodes.Beq, done);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Ldfld, fPlantId); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, fPlantId); E(il, OpCodes.Ble, next);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlantSaves); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Callvirt, insert); E(il, OpCodes.Br, done);
    il.Append(next);
    E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Add); E(il, OpCodes.Stloc, i); E(il, OpCodes.Br, loopCheck);
    il.Append(addEnd);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlantSaves); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Callvirt, add);
    il.Append(done);
    Console.WriteLine("PATCH Save.GetPlantSave: sorted unique managed insertion");
}

void PatchGetPropSave(MethodDefinition m)
{
    var listType = (GenericInstanceType)fPropSaves.FieldType;
    var count = ListMethod(listType, "get_Count", module.TypeSystem.Int32);
    var item = ListMethod(listType, "get_Item", propSave, module.TypeSystem.Int32);
    var body = Fresh(m, true); var il = body.GetILProcessor();
    var i = new VariableDefinition(module.TypeSystem.Int32); body.Variables.Add(i);
    var cur = new VariableDefinition(propSave); body.Variables.Add(cur);
    var loopCheck = Instruction.Create(OpCodes.Nop); var loopBody = Instruction.Create(OpCodes.Nop); var next = Instruction.Create(OpCodes.Nop); var notFound = Instruction.Create(OpCodes.Ldnull);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPropSaves); E(il, OpCodes.Brfalse, notFound);
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stloc, i); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPropSaves); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Callvirt, item); E(il, OpCodes.Stloc, cur);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Brfalse, next);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Ldfld, fPropId); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Bne_Un, next);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Ret);
    il.Append(next); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Add); E(il, OpCodes.Stloc, i);
    il.Append(loopCheck); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPropSaves); E(il, OpCodes.Callvirt, count); E(il, OpCodes.Blt, loopBody);
    il.Append(notFound); E(il, OpCodes.Ret);
    Console.WriteLine("PATCH Save.GetPropSave: managed indexed lookup");
}

void PatchGetSupplies(MethodDefinition m)
{
    var listType = (GenericInstanceType)fSupplies.FieldType;
    var count = ListMethod(listType, "get_Count", module.TypeSystem.Int32);
    var item = ListMethod(listType, "get_Item", supplies, module.TypeSystem.Int32);
    var create = M(resourceManager, "CreatNewSupplies", 1);
    var body = Fresh(m, true); var il = body.GetILProcessor();
    var i = new VariableDefinition(module.TypeSystem.Int32); body.Variables.Add(i);
    var cur = new VariableDefinition(supplies); body.Variables.Add(cur);
    var created = new VariableDefinition(supplies); body.Variables.Add(created);
    var createNew = Instruction.Create(OpCodes.Nop); var loopCheck = Instruction.Create(OpCodes.Nop); var loopBody = Instruction.Create(OpCodes.Nop); var next = Instruction.Create(OpCodes.Nop); var retNull = Instruction.Create(OpCodes.Ldnull);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSupplies); E(il, OpCodes.Brfalse, createNew);
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stloc, i); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSupplies); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Callvirt, item); E(il, OpCodes.Stloc, cur);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Brfalse, next);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Ldfld, fSupplyId); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Bne_Un, next);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Ret);
    il.Append(next); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Add); E(il, OpCodes.Stloc, i);
    il.Append(loopCheck); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSupplies); E(il, OpCodes.Callvirt, count); E(il, OpCodes.Blt, loopBody);
    il.Append(createNew);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Call, create); E(il, OpCodes.Stloc, created);
    E(il, OpCodes.Ldloc, created); E(il, OpCodes.Brfalse, retNull);
    E(il, OpCodes.Ldloc, created); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, fSupplyQty); E(il, OpCodes.Ldloc, created); E(il, OpCodes.Ret);
    il.Append(retNull); E(il, OpCodes.Ret);
    Console.WriteLine("PATCH Save.GetSupplies: managed lookup + zero-quantity fallback");
}

void PatchGetSkillId(MethodDefinition m)
{
    var body = Fresh(m); var il = body.GetILProcessor();
    var check2 = Instruction.Create(OpCodes.Nop); var check3 = Instruction.Create(OpCodes.Nop); var retNull = Instruction.Create(OpCodes.Ldnull);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill1); E(il, OpCodes.Brfalse, check2);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill1); E(il, OpCodes.Ldfld, fSkillId); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Bne_Un, check2);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill1); E(il, OpCodes.Ret);
    il.Append(check2);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill2); E(il, OpCodes.Brfalse, check3);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill2); E(il, OpCodes.Ldfld, fSkillId); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Bne_Un, check3);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill2); E(il, OpCodes.Ret);
    il.Append(check3);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill3); E(il, OpCodes.Brfalse, retNull);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill3); E(il, OpCodes.Ldfld, fSkillId); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Bne_Un, retNull);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill3); E(il, OpCodes.Ret);
    il.Append(retNull); E(il, OpCodes.Ret);
    Console.WriteLine("PATCH PlantSave.GetSkill_ID: match actual local skill ID");
}

void PatchSkillCount(MethodDefinition m, bool unlockedOnly)
{
    var body = Fresh(m, true); var il = body.GetILProcessor();
    var countVar = new VariableDefinition(module.TypeSystem.Int32); body.Variables.Add(countVar);
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stloc, countVar);
    foreach (var field in new[] { fSkill1, fSkill2, fSkill3 })
    {
        var skip = Instruction.Create(OpCodes.Nop);
        E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, field); E(il, OpCodes.Brfalse, skip);
        E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, field); E(il, OpCodes.Ldfld, fSkillId); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ble, skip);
        if (unlockedOnly) { E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, field); E(il, OpCodes.Ldfld, fSkillUnlock); E(il, OpCodes.Brfalse, skip); }
        E(il, OpCodes.Ldloc, countVar); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Add); E(il, OpCodes.Stloc, countVar);
        il.Append(skip);
    }
    E(il, OpCodes.Ldloc, countVar); E(il, OpCodes.Ret);
    Console.WriteLine($"PATCH PlantSave.{m.Name}: count valid{(unlockedOnly ? " unlocked" : "")} skills");
}

void PatchGetSkillData(MethodDefinition m)
{
    var update = M(skillManager, "UpdataSkill", 3);
    var body = Fresh(m); var il = body.GetILProcessor();
    var ret = Instruction.Create(OpCodes.Ret);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Brfalse, ret);
    E(il, OpCodes.Ldarga, m.Parameters[0]);
    E(il, OpCodes.Ldarg_2); E(il, OpCodes.Ldc_I4_3); E(il, OpCodes.Mul);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, fSkillId); E(il, OpCodes.Add);
    E(il, OpCodes.Ldarg_3); E(il, OpCodes.Call, update);
    il.Append(ret);
    Console.WriteLine("PATCH SavesManager.GetSkillData: global skill index = plantID*3 + localSkillID");
}

void PatchGetSkillDataAll(MethodDefinition m)
{
    var get = M(savesManager, "GetSkillData", 3);
    var body = Fresh(m); var il = body.GetILProcessor(); var ret = Instruction.Create(OpCodes.Ret);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Brfalse, ret);
    foreach (var field in new[] { fSkill1, fSkill2, fSkill3 })
    {
        var skip = Instruction.Create(OpCodes.Nop);
        E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, field); E(il, OpCodes.Brfalse, skip);
        E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, field); E(il, OpCodes.Ldfld, fSkillId); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ble, skip);
        E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, field); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, fPsId); E(il, OpCodes.Ldarg_2); E(il, OpCodes.Call, get);
        il.Append(skip);
    }
    il.Append(ret);
    Console.WriteLine("PATCH SavesManager.GetSkillData_All: update three valid skills with plant ID");
}

TypeDefinition T(string name) => FindType(module, name);
FieldDefinition F(TypeDefinition t, string name) => t.Fields.Single(f => f.Name == name);
MethodDefinition M(TypeDefinition t, string name, int count)
{
    var ms=t.Methods.Where(x=>x.Name==name && x.Parameters.Count==count).ToList();
    return ms.Count==1?ms[0]:throw new InvalidOperationException($"Expected one {t.Name}.{name}/{count}, found {ms.Count}");
}
static TypeDefinition FindType(ModuleDefinition m,string name)=>All(m.Types).Single(t=>t.Name==name||t.FullName==name);
static IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> roots){foreach(var t in roots){yield return t;foreach(var n in All(t.NestedTypes))yield return n;}}
MethodBody Fresh(MethodDefinition m,bool init=false){var b=new MethodBody(m){InitLocals=init,MaxStackSize=16};m.Body=b;return b;}
MethodReference ListMethod(GenericInstanceType list,string name,TypeReference ret,params TypeReference[] args){var r=new MethodReference(name,ret,list){HasThis=true};foreach(var a in args)r.Parameters.Add(new ParameterDefinition(a));return module.ImportReference(r);}
void E(ILProcessor il,OpCode op,object? arg=null){Instruction i=arg switch{null=>Instruction.Create(op),int v=>Instruction.Create(op,v),float v=>Instruction.Create(op,v),string v=>Instruction.Create(op,v),Instruction v=>Instruction.Create(op,v),VariableDefinition v=>Instruction.Create(op,v),ParameterDefinition v=>Instruction.Create(op,v),MethodReference v=>Instruction.Create(op,v),FieldReference v=>Instruction.Create(op,v),TypeReference v=>Instruction.Create(op,v),_=>throw new NotSupportedException($"Unsupported operand {arg.GetType().FullName}")};il.Append(i);}
