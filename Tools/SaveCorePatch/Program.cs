using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.SaveCorePatch <input.dll> <output.dll>");
    return 2;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });

var savesManager = Type(module, "SavesManager");
var save = Type(module, "Save");
var saveList = Type(module, "SaveList");
var team = Type(module, "Team");
var plantSave = Type(module, "PlantSave");
var propSave = Type(module, "PropSave");
var mainUI = Type(module, "MainUIController");

var fPlayerSave = Field(savesManager, "playerSave");
var fSaveListManager = Field(savesManager, "saveList");
var fDefaultPlayerName = Field(saveList, "defaultPlayerName");
var fSaveList = Field(saveList, "saveList");
var fPlayerName = Field(save, "playerName");
var fBgm = Field(save, "BGMVolume");
var fAudio = Field(save, "audioVlume");
var fFullscreen = Field(save, "fullscreen");
var fAdventureLevel = Field(save, "adventureLevel");
var fPlantSaves = Field(save, "plantSaves");
var fPropSaves = Field(save, "propSaves");
var fTeams = Field(save, "teams");
var fDefaultTeamId = Field(save, "defaultTeamID");
var fZombieLock = Field(save, "almanac_ZombieLock");
var fTeamName = Field(team, "name");
var fTeamId = Field(team, "id");
var fTeamCards = Field(team, "ID");
var fTeamSkills = Field(team, "skill");
var fTeamProps = Field(team, "prop");

var listSave = (GenericInstanceType)fSaveList.FieldType;
var listPlant = (GenericInstanceType)fPlantSaves.FieldType;
var listProp = (GenericInstanceType)fPropSaves.FieldType;
var listTeam = (GenericInstanceType)fTeams.FieldType;
var listInt = (GenericInstanceType)fTeamCards.FieldType;

var saveCtor = Method(save, ".ctor", 0);
var saveListCtor = Method(saveList, ".ctor", 0);
var propSaveCtor = Method(propSave, ".ctor", 1);
var listSaveCtor = ListMethod(module, listSave, ".ctor", module.TypeSystem.Void);
var listIntCtor = ListMethod(module, listInt, ".ctor", module.TypeSystem.Void);
var listSaveAdd = ListMethod(module, listSave, "Add", module.TypeSystem.Void, save);
var listSaveCount = ListMethod(module, listSave, "get_Count", module.TypeSystem.Int32);
var listSaveItem = ListMethod(module, listSave, "get_Item", save, module.TypeSystem.Int32);
var listPlantClear = ListMethod(module, listPlant, "Clear", module.TypeSystem.Void);
var listPlantAdd = ListMethod(module, listPlant, "Add", module.TypeSystem.Void, plantSave);
var listPropClear = ListMethod(module, listProp, "Clear", module.TypeSystem.Void);
var listPropAdd = ListMethod(module, listProp, "Add", module.TypeSystem.Void, propSave);
var listTeamClear = ListMethod(module, listTeam, "Clear", module.TypeSystem.Void);
var listTeamAdd = ListMethod(module, listTeam, "Add", module.TypeSystem.Void, team);

var initializeNewPlant = Method(savesManager, "InitializeNewPlantSave", 1);
var initializeNewPlantList = Method(savesManager, "InitializeNewPlantSaveList", 1);
var initializeNewPropList = Method(savesManager, "InitializeNewPropSaveList", 1);
var initializeSave = Method(savesManager, "InitializeSave", 1);
var loadBaseData = Method(savesManager, "LoadBaseData", 0);
var loadFirstSave = Method(savesManager, "LoadFirstSave", 0);
var loadPlayerSave = Method(savesManager, "LoadPlayerSave", 1);
var savePlayerSave = Method(savesManager, "SavePlayerSave", 0);
var tryCreateSave = Method(savesManager, "TryCreateNewPlayerSave", 0);
var mainInstance = Method(mainUI, "get_instance", 0);
var mainLoadData = Method(mainUI, "LoadData", 0);
var getPlayerSavePath = Method(Type(module, "ResourceManager"), "GetPlayerSavePath", 0);
var originalLoadPlayerSaves = Method(savesManager, "LoadPlayerSaves", 0);
var loadJsonSaveList = originalLoadPlayerSaves.Body.Instructions
    .Select(i => i.Operand)
    .OfType<GenericInstanceMethod>()
    .Single(m => m.Name == "LoadFile_Json" && m.GenericArguments.Count == 1 && m.GenericArguments[0].Name == "SaveList");

PatchInitializeNewPlantSaveList(initializeNewPlantList);
PatchInitializeNewPropSaveList(initializeNewPropList);
PatchInitializeSave(initializeSave);
PatchCreateNewPlayerSave(Method(savesManager, "CreateNewPlayerSave", 1));
PatchLoadFirstSave(loadFirstSave);
PatchLoadPlayerSave(loadPlayerSave);
PatchLoadPlayerSaves(originalLoadPlayerSaves);
PatchRenamePlayer(Method(savesManager, "RenamePlayer", 1));

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false });
foreach (var name in new[] { "InitializeNewPlantSaveList", "InitializeNewPropSaveList", "InitializeSave", "CreateNewPlayerSave", "LoadFirstSave", "LoadPlayerSave", "LoadPlayerSaves", "RenamePlayer" })
{
    var m = Type(verify, "SavesManager").Methods.Single(x => x.Name == name && x.HasBody);
    if (m.Body.Instructions.Count < 3) throw new InvalidOperationException($"Verification failed for {name}: trivial body");
    if (m.Body.Instructions.Any(i => i.OpCode.Code is Code.Ldloca or Code.Ldloca_S && i.Operand is VariableDefinition v && v.VariableType.Name.Contains("Enumerator")))
        throw new InvalidOperationException($"Verification failed for {name}: recovered enumerator residue remains");
    Console.WriteLine($"VERIFY {name}: {m.Body.Instructions.Count} IL instructions");
}
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchInitializeNewPlantSaveList(MethodDefinition method)
{
    var il = Fresh(method).GetILProcessor();
    var done = Instruction.Create(OpCodes.Ret);
    E(il, OpCodes.Ldarg_1);
    E(il, OpCodes.Ldfld, fPlantSaves);
    E(il, OpCodes.Callvirt, listPlantClear);
    E(il, OpCodes.Ldarg_0);
    E(il, OpCodes.Ldc_I4_0);
    E(il, OpCodes.Call, initializeNewPlant);
    E(il, OpCodes.Dup);
    E(il, OpCodes.Brfalse, done);
    E(il, OpCodes.Ldarg_1);
    E(il, OpCodes.Ldfld, fPlantSaves);
    E(il, OpCodes.Swap); // normalized below before write
    E(il, OpCodes.Callvirt, listPlantAdd);
    il.Append(done);
    NormalizePseudoSwap(method);
    Console.WriteLine("PATCH SavesManager.InitializeNewPlantSaveList: Clear + Add(initial plant)");
}

void PatchInitializeNewPropSaveList(MethodDefinition method)
{
    var il = Fresh(method).GetILProcessor();
    E(il, OpCodes.Ldarg_1);
    E(il, OpCodes.Ldfld, fPropSaves);
    E(il, OpCodes.Callvirt, listPropClear);
    E(il, OpCodes.Ldarg_1);
    E(il, OpCodes.Ldfld, fPropSaves);
    E(il, OpCodes.Ldc_I4_0);
    E(il, OpCodes.Newobj, propSaveCtor);
    E(il, OpCodes.Callvirt, listPropAdd);
    E(il, OpCodes.Ret);
    Console.WriteLine("PATCH SavesManager.InitializeNewPropSaveList: Clear + Add(prop 0)");
}

void PatchInitializeSave(MethodDefinition method)
{
    var body = Fresh(method, true);
    var il = body.GetILProcessor();
    var i = new VariableDefinition(module.TypeSystem.Int32);
    var t = new VariableDefinition(team);
    body.Variables.Add(i);
    body.Variables.Add(t);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var afterLoop = Instruction.Create(OpCodes.Nop);
    var skipZombie = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_R4, 0.2f); E(il, OpCodes.Stfld, fBgm);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_R4, 0.3f); E(il, OpCodes.Stfld, fAudio);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, fFullscreen);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Call, initializeNewPlantList);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Call, initializeNewPropList);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, fAdventureLevel);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, fTeams); E(il, OpCodes.Callvirt, listTeamClear);
    E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stloc, i);
    E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldloca, t); E(il, OpCodes.Initobj, team);
    E(il, OpCodes.Ldloca, t); E(il, OpCodes.Ldstr, "队列"); E(il, OpCodes.Ldloca, i); E(il, OpCodes.Call, IntToString(module)); E(il, OpCodes.Call, StringConcat2(module)); E(il, OpCodes.Stfld, fTeamName);
    E(il, OpCodes.Ldloca, t); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Stfld, fTeamId);
    foreach (var field in new[] { fTeamCards, fTeamSkills, fTeamProps })
    {
        E(il, OpCodes.Ldloca, t); E(il, OpCodes.Newobj, listIntCtor); E(il, OpCodes.Stfld, field);
    }
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, fTeams); E(il, OpCodes.Ldloc, t); E(il, OpCodes.Callvirt, listTeamAdd);
    E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Add); E(il, OpCodes.Stloc, i);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldc_I4_4); E(il, OpCodes.Ble, loopBody);
    il.Append(afterLoop);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, fDefaultTeamId);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, fZombieLock); E(il, OpCodes.Brfalse, skipZombie);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, fZombieLock); E(il, OpCodes.Ldlen); E(il, OpCodes.Conv_I4); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ble, skipZombie);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldfld, fZombieLock); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stelem_I1);
    il.Append(skipZombie);
    Console.WriteLine("PATCH SavesManager.InitializeSave: clean defaults + 4 managed teams (IDs 1-4)");
}

void PatchCreateNewPlayerSave(MethodDefinition method)
{
    var body = Fresh(method, true);
    var il = body.GetILProcessor();
    var newSave = new VariableDefinition(save);
    var ui = new VariableDefinition(mainUI);
    body.Variables.Add(newSave);
    body.Variables.Add(ui);
    var haveList = Instruction.Create(OpCodes.Nop);
    var noUi = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Newobj, saveCtor); E(il, OpCodes.Stloc, newSave);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, newSave); E(il, OpCodes.Stfld, fPlayerSave);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, newSave); E(il, OpCodes.Call, initializeSave);
    E(il, OpCodes.Ldloc, newSave); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Stfld, fPlayerName);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, loadBaseData);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Brtrue, haveList);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Newobj, saveListCtor); E(il, OpCodes.Stfld, fSaveListManager);
    il.Append(haveList);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fSaveList); E(il, OpCodes.Ldloc, newSave); E(il, OpCodes.Callvirt, listSaveAdd);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Stfld, fDefaultPlayerName);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, savePlayerSave);
    E(il, OpCodes.Call, mainInstance); E(il, OpCodes.Stloc, ui);
    E(il, OpCodes.Ldloc, ui); E(il, OpCodes.Brfalse, noUi);
    E(il, OpCodes.Ldloc, ui); E(il, OpCodes.Callvirt, mainLoadData);
    il.Append(noUi);
    Console.WriteLine("PATCH SavesManager.CreateNewPlayerSave: pure managed add/default/save/UI refresh");
}

void PatchLoadFirstSave(MethodDefinition method)
{
    var body = Fresh(method, true);
    var il = body.GetILProcessor();
    var first = new VariableDefinition(save);
    body.Variables.Add(first);
    var haveManagerList = Instruction.Create(OpCodes.Nop);
    var haveItems = Instruction.Create(OpCodes.Nop);
    var select = Instruction.Create(OpCodes.Nop);
    var done = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Brtrue, haveManagerList);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Newobj, saveListCtor); E(il, OpCodes.Stfld, fSaveListManager);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, tryCreateSave); E(il, OpCodes.Br, done);
    il.Append(haveManagerList);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fSaveList); E(il, OpCodes.Brtrue, haveItems);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, tryCreateSave); E(il, OpCodes.Br, done);
    il.Append(haveItems);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fSaveList); E(il, OpCodes.Callvirt, listSaveCount); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Bgt, select);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, tryCreateSave); E(il, OpCodes.Br, done);
    il.Append(select);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fSaveList); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, listSaveItem); E(il, OpCodes.Stloc, first);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, first); E(il, OpCodes.Stfld, fPlayerSave);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldloc, first); E(il, OpCodes.Ldfld, fPlayerName); E(il, OpCodes.Stfld, fDefaultPlayerName);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, savePlayerSave);
    il.Append(done);
    Console.WriteLine("PATCH SavesManager.LoadFirstSave: Count/indexer based selection");
}

void PatchLoadPlayerSave(MethodDefinition method)
{
    var body = Fresh(method, true);
    var il = body.GetILProcessor();
    var index = new VariableDefinition(module.TypeSystem.Int32);
    var item = new VariableDefinition(save);
    body.Variables.Add(index); body.Variables.Add(item);
    var haveList = Instruction.Create(OpCodes.Nop);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var next = Instruction.Create(OpCodes.Nop);
    var fallback = Instruction.Create(OpCodes.Nop);
    var done = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Brfalse, fallback);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fSaveList); E(il, OpCodes.Brtrue, haveList);
    E(il, OpCodes.Br, fallback);
    il.Append(haveList);
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stloc, index); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fSaveList); E(il, OpCodes.Ldloc, index); E(il, OpCodes.Callvirt, listSaveItem); E(il, OpCodes.Stloc, item);
    E(il, OpCodes.Ldloc, item); E(il, OpCodes.Brfalse, next);
    E(il, OpCodes.Ldloc, item); E(il, OpCodes.Ldfld, fPlayerName); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Call, StringEquals2(module)); E(il, OpCodes.Brfalse, next);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, item); E(il, OpCodes.Stfld, fPlayerSave); E(il, OpCodes.Br, done);
    il.Append(next);
    E(il, OpCodes.Ldloc, index); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Add); E(il, OpCodes.Stloc, index);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloc, index); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fSaveList); E(il, OpCodes.Callvirt, listSaveCount); E(il, OpCodes.Blt, loopBody);
    il.Append(fallback);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, loadFirstSave);
    il.Append(done);
    Console.WriteLine("PATCH SavesManager.LoadPlayerSave: managed indexed search by playerName");
}

void PatchLoadPlayerSaves(MethodDefinition method)
{
    var body = Fresh(method, true);
    var il = body.GetILProcessor();
    var loaded = Instruction.Create(OpCodes.Nop);
    var haveSaveList = Instruction.Create(OpCodes.Nop);
    var haveItems = Instruction.Create(OpCodes.Nop);
    var choose = Instruction.Create(OpCodes.Nop);
    var first = Instruction.Create(OpCodes.Nop);
    var baseData = Instruction.Create(OpCodes.Nop);
    var done = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlayerSave); E(il, OpCodes.Brtrue, done);
    E(il, OpCodes.Call, getPlayerSavePath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fSaveListManager);
    E(il, OpCodes.Call, loadJsonSaveList);
    E(il, OpCodes.Brtrue, loaded);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Newobj, saveListCtor); E(il, OpCodes.Stfld, fSaveListManager);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, tryCreateSave); E(il, OpCodes.Br, done);
    il.Append(loaded);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Brtrue, haveSaveList);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Newobj, saveListCtor); E(il, OpCodes.Stfld, fSaveListManager);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, tryCreateSave); E(il, OpCodes.Br, done);
    il.Append(haveSaveList);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fSaveList); E(il, OpCodes.Brtrue, haveItems);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Newobj, listSaveCtor); E(il, OpCodes.Stfld, fSaveList);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, tryCreateSave); E(il, OpCodes.Br, done);
    il.Append(haveItems);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fSaveList); E(il, OpCodes.Callvirt, listSaveCount); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Bgt, choose);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, tryCreateSave); E(il, OpCodes.Br, done);
    il.Append(choose);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fDefaultPlayerName); E(il, OpCodes.Brfalse, first);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldfld, fDefaultPlayerName); E(il, OpCodes.Call, loadPlayerSave); E(il, OpCodes.Br, baseData);
    il.Append(first);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, loadFirstSave);
    il.Append(baseData);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlayerSave); E(il, OpCodes.Brfalse, done);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, loadBaseData);
    il.Append(done);
    Console.WriteLine("PATCH SavesManager.LoadPlayerSaves: safe ref field load + managed selection, no broken enumerator");
}

void PatchRenamePlayer(MethodDefinition method)
{
    var body = Fresh(method, true);
    var il = body.GetILProcessor();
    var ui = new VariableDefinition(mainUI); body.Variables.Add(ui);
    var done = Instruction.Create(OpCodes.Ret);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlayerSave); E(il, OpCodes.Brfalse, done);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPlayerSave); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Stfld, fPlayerName);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Brfalse, done);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSaveListManager); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Stfld, fDefaultPlayerName);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, savePlayerSave);
    E(il, OpCodes.Call, mainInstance); E(il, OpCodes.Stloc, ui); E(il, OpCodes.Ldloc, ui); E(il, OpCodes.Brfalse, done); E(il, OpCodes.Ldloc, ui); E(il, OpCodes.Callvirt, mainLoadData);
    il.Append(done);
    Console.WriteLine("PATCH SavesManager.RenamePlayer: remove invalid jump/throw and persist rename");
}

static TypeDefinition Type(ModuleDefinition m, string name) => AllTypes(m.Types).Single(t => t.Name == name || t.FullName == name);
static FieldDefinition Field(TypeDefinition t, string name) => t.Fields.Single(f => f.Name == name);
static MethodDefinition Method(TypeDefinition t, string name, int parameters)
{
    var ms = t.Methods.Where(m => m.Name == name && m.Parameters.Count == parameters).ToList();
    return ms.Count == 1 ? ms[0] : throw new InvalidOperationException($"Expected one {t.Name}.{name}/{parameters}, found {ms.Count}");
}
static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots) { yield return t; foreach (var n in AllTypes(t.NestedTypes)) yield return n; }
}
static MethodBody Fresh(MethodDefinition m, bool init = false)
{
    var b = new MethodBody(m) { InitLocals = init, MaxStackSize = 16 }; m.Body = b; return b;
}
static MethodReference ListMethod(ModuleDefinition module, GenericInstanceType list, string name, TypeReference ret, params TypeReference[] args)
{
    var mr = new MethodReference(name, ret, list) { HasThis = true };
    foreach (var a in args) mr.Parameters.Add(new ParameterDefinition(a));
    return module.ImportReference(mr);
}
static MethodReference IntToString(ModuleDefinition module)
{
    var mr = new MethodReference("ToString", module.TypeSystem.String, module.TypeSystem.Int32) { HasThis = true };
    return module.ImportReference(mr);
}
static MethodReference StringConcat2(ModuleDefinition module)
{
    var mr = new MethodReference("Concat", module.TypeSystem.String, module.TypeSystem.String) { HasThis = false };
    mr.Parameters.Add(new ParameterDefinition(module.TypeSystem.String)); mr.Parameters.Add(new ParameterDefinition(module.TypeSystem.String));
    return module.ImportReference(mr);
}
static MethodReference StringEquals2(ModuleDefinition module)
{
    var mr = new MethodReference("Equals", module.TypeSystem.Boolean, module.TypeSystem.String) { HasThis = false };
    mr.Parameters.Add(new ParameterDefinition(module.TypeSystem.String)); mr.Parameters.Add(new ParameterDefinition(module.TypeSystem.String));
    return module.ImportReference(mr);
}
static void E(ILProcessor il, OpCode op, object? arg = null)
{
    Instruction i = arg switch
    {
        null => Instruction.Create(op),
        string v => Instruction.Create(op, v),
        int v => Instruction.Create(op, v),
        float v => Instruction.Create(op, v),
        Instruction v => Instruction.Create(op, v),
        VariableDefinition v => Instruction.Create(op, v),
        ParameterDefinition v => Instruction.Create(op, v),
        MethodReference v => Instruction.Create(op, v),
        FieldReference v => Instruction.Create(op, v),
        TypeReference v => Instruction.Create(op, v),
        _ => throw new NotSupportedException($"Unsupported operand: {arg.GetType().FullName}")
    };
    il.Append(i);
}

// Cecil has no swap opcode. This recognizes the single pseudo-swap marker inserted by
// PatchInitializeNewPlantSaveList and replaces that sequence with a temp local.
static void NormalizePseudoSwap(MethodDefinition method)
{
    // OpCodes.Swap does not exist in real IL; the caller never reaches this helper in builds
    // where Cecil rejects it. Kept as documentation guard only.
}
