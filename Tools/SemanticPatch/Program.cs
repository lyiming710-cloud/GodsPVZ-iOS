using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.SemanticPatch <input.dll> <output.dll>");
    return 2;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });

PatchBoardConfigPath(module);
PatchPlantPrefabPath(module);
PatchIdJsonPath(module, "GetZombieInfoPath", "zombieID", "ZombieData", "ZombieInfo");
PatchIdJsonPath(module, "GetDeviceInfoPath", "deviceID", "DeviceData", "DeviceInfo");
PatchBoardBackgroundPath(module);
PatchSkillCrosshairsPath(module);
PatchSkillDataPath(module);
PatchLoadFileJson(module);
PatchFirstIntToStringArgument(module, "Load_zombieInfo", "zombieID");
PatchFirstIntToStringArgument(module, "Load_deviceInfo", "deviceID");

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false });
VerifyNoUninitializedPathId(verify, "GetZombieInfoPath");
VerifyNoUninitializedPathId(verify, "GetDeviceInfoPath");
VerifyNoUninitializedPathId(verify, "GetSkillCrosshairsPath");
VerifyNoUninitializedPathId(verify, "GetSkillDataPath");
var loadJson = FindMethod(verify, "ResourceManager", "LoadFile_Json", 2);
if (!loadJson.Body.Instructions.Any(i => i.OpCode == OpCodes.Stobj))
    throw new InvalidOperationException("LoadFile_Json<T> verification failed: no stobj assignment to ref target.");

Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

static TypeDefinition FindType(ModuleDefinition module, string name)
    => AllTypes(module.Types).Single(t => t.Name == name || t.FullName == name);

static MethodDefinition FindMethod(ModuleDefinition module, string type, string name, int parameterCount)
{
    var matches = FindType(module, type).Methods.Where(m => m.Name == name && m.Parameters.Count == parameterCount).ToList();
    return matches.Count == 1 ? matches[0] : throw new InvalidOperationException($"Expected one {type}.{name}/{parameterCount}, found {matches.Count}");
}

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> types)
{
    foreach (var type in types)
    {
        yield return type;
        foreach (var nested in AllTypes(type.NestedTypes)) yield return nested;
    }
}

static MethodReference CallRef(MethodDefinition method, string declaringContains, string name, int parameterCount)
{
    var refs = method.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .Where(m => m.Name == name && m.Parameters.Count == parameterCount && m.DeclaringType.FullName.Contains(declaringContains, StringComparison.Ordinal))
        .ToList();
    if (refs.Count == 0)
        throw new InvalidOperationException($"Call ref not found in {method.FullName}: {declaringContains}::{name}/{parameterCount}");
    return refs[0];
}

static MethodBody FreshBody(MethodDefinition method, bool initLocals = false)
{
    var body = new MethodBody(method) { InitLocals = initLocals, MaxStackSize = 8 };
    method.Body = body;
    return body;
}

static void Emit(ILProcessor il, OpCode code) => il.Append(Instruction.Create(code));
static void Emit(ILProcessor il, OpCode code, string value) => il.Append(Instruction.Create(code, value));
static void Emit(ILProcessor il, OpCode code, int value) => il.Append(Instruction.Create(code, value));
static void Emit(ILProcessor il, OpCode code, MethodReference value) => il.Append(Instruction.Create(code, value));
static void Emit(ILProcessor il, OpCode code, TypeReference value) => il.Append(Instruction.Create(code, value));
static void Emit(ILProcessor il, OpCode code, FieldReference value) => il.Append(Instruction.Create(code, value));
static void Emit(ILProcessor il, OpCode code, ParameterDefinition value) => il.Append(Instruction.Create(code, value));
static void Emit(ILProcessor il, OpCode code, VariableDefinition value) => il.Append(Instruction.Create(code, value));
static void Emit(ILProcessor il, OpCode code, Instruction value) => il.Append(Instruction.Create(code, value));

static void PatchIdJsonPath(ModuleDefinition module, string methodName, string parameterName, string category, string leaf)
{
    var method = FindMethod(module, "ResourceManager", methodName, 1);
    var getStreaming = CallRef(method, "UnityEngine.Application", "get_streamingAssetsPath", 0);
    var combine4 = CallRef(method, "System.IO.Path", "Combine", 4);
    var combine2 = CallRef(method, "System.IO.Path", "Combine", 2);
    var toString = CallRef(method, "System.Int32", "ToString", 0);
    var concat2 = CallRef(method, "System.String", "Concat", 2);
    var p = method.Parameters.Single(x => x.Name == parameterName);
    var il = FreshBody(method).GetILProcessor();
    Emit(il, OpCodes.Call, getStreaming);
    Emit(il, OpCodes.Ldstr, "jsons");
    Emit(il, OpCodes.Ldstr, category);
    Emit(il, OpCodes.Ldstr, leaf);
    Emit(il, OpCodes.Call, combine4);
    Emit(il, OpCodes.Ldarga, p);
    Emit(il, OpCodes.Call, toString);
    Emit(il, OpCodes.Ldstr, ".json");
    Emit(il, OpCodes.Call, concat2);
    Emit(il, OpCodes.Call, combine2);
    Emit(il, OpCodes.Ret);
    Console.WriteLine($"PATCH ResourceManager.{methodName}: use {parameterName} in file name");
}

static void PatchPlantPrefabPath(ModuleDefinition module)
{
    var method = FindMethod(module, "ResourceManager", "GetPlantPrefabPath", 1);
    var combine2 = CallRef(method, "System.IO.Path", "Combine", 2);
    var getType = CallRef(method, "System.Type", "GetTypeFromHandle", 1);
    var getName = CallRef(method, "System.Enum", "GetName", 2);
    var plantType = FindType(module, "PlantType");
    var p = method.Parameters[0];
    var il = FreshBody(method).GetILProcessor();
    Emit(il, OpCodes.Ldstr, "prefabs");
    Emit(il, OpCodes.Ldstr, "Plant");
    Emit(il, OpCodes.Call, combine2);
    Emit(il, OpCodes.Ldtoken, plantType);
    Emit(il, OpCodes.Call, getType);
    Emit(il, OpCodes.Ldarg, p);
    Emit(il, OpCodes.Box, plantType);
    Emit(il, OpCodes.Call, getName);
    Emit(il, OpCodes.Call, combine2);
    Emit(il, OpCodes.Ret);
    Console.WriteLine("PATCH ResourceManager.GetPlantPrefabPath: enum name from plantID");
}

static void PatchBoardBackgroundPath(ModuleDefinition module)
{
    var method = FindMethod(module, "ResourceManager", "GetBoardBackgroundSpritePath", 1);
    var combine3 = CallRef(method, "System.IO.Path", "Combine", 3);
    var combine2 = CallRef(method, "System.IO.Path", "Combine", 2);
    var getType = CallRef(method, "System.Type", "GetTypeFromHandle", 1);
    var getName = CallRef(method, "System.Enum", "GetName", 2);
    var enumType = FindType(module, "SenarioState");
    var p = method.Parameters[0];
    var il = FreshBody(method).GetILProcessor();
    Emit(il, OpCodes.Ldstr, "sprites");
    Emit(il, OpCodes.Ldstr, "Board");
    Emit(il, OpCodes.Ldstr, "Background");
    Emit(il, OpCodes.Call, combine3);
    Emit(il, OpCodes.Ldtoken, enumType);
    Emit(il, OpCodes.Call, getType);
    Emit(il, OpCodes.Ldarg, p);
    Emit(il, OpCodes.Box, enumType);
    Emit(il, OpCodes.Call, getName);
    Emit(il, OpCodes.Call, combine2);
    Emit(il, OpCodes.Ret);
    Console.WriteLine("PATCH ResourceManager.GetBoardBackgroundSpritePath: enum name from senarioState");
}

static void PatchSkillCrosshairsPath(ModuleDefinition module)
{
    var method = FindMethod(module, "ResourceManager", "GetSkillCrosshairsPath", 2);
    var combine3 = CallRef(method, "System.IO.Path", "Combine", 3);
    var combine2 = CallRef(method, "System.IO.Path", "Combine", 2);
    var toString = CallRef(method, "System.Int32", "ToString", 0);
    var concat3 = CallRef(method, "System.String", "Concat", 3);
    var il = FreshBody(method).GetILProcessor();
    Emit(il, OpCodes.Ldstr, "sprites");
    Emit(il, OpCodes.Ldstr, "Skill");
    Emit(il, OpCodes.Ldstr, "Crosshairs");
    Emit(il, OpCodes.Call, combine3);
    Emit(il, OpCodes.Ldarga, method.Parameters[0]);
    Emit(il, OpCodes.Call, toString);
    Emit(il, OpCodes.Ldstr, "_");
    Emit(il, OpCodes.Ldarga, method.Parameters[1]);
    Emit(il, OpCodes.Call, toString);
    Emit(il, OpCodes.Call, concat3);
    Emit(il, OpCodes.Call, combine2);
    Emit(il, OpCodes.Ret);
    Console.WriteLine("PATCH ResourceManager.GetSkillCrosshairsPath: plantID_skillIndex");
}

static void PatchSkillDataPath(ModuleDefinition module)
{
    var method = FindMethod(module, "ResourceManager", "GetSkillDataPath", 1);
    var getStreaming = CallRef(method, "UnityEngine.Application", "get_streamingAssetsPath", 0);
    var combine4 = CallRef(method, "System.IO.Path", "Combine", 4);
    var combine2 = method.Module.Types.SelectMany(_ => Array.Empty<MethodReference>()).FirstOrDefault();
    // GetSkillDataPath originally only used Path.Combine(string[]); borrow the 2-arg overload from a sibling method.
    combine2 = CallRef(FindMethod(module, "ResourceManager", "GetPlayerSavePath", 0), "System.IO.Path", "Combine", 2);
    var toString = CallRef(method, "System.Int32", "ToString", 0);
    var concat2 = CallRef(method, "System.String", "Concat", 2);
    var il = FreshBody(method).GetILProcessor();
    Emit(il, OpCodes.Call, getStreaming);
    Emit(il, OpCodes.Ldstr, "jsons");
    Emit(il, OpCodes.Ldstr, "PlantData");
    Emit(il, OpCodes.Ldstr, "Skills");
    Emit(il, OpCodes.Call, combine4);
    Emit(il, OpCodes.Ldarga, method.Parameters[0]);
    Emit(il, OpCodes.Call, toString);
    Emit(il, OpCodes.Ldstr, ".json");
    Emit(il, OpCodes.Call, concat2);
    Emit(il, OpCodes.Call, combine2!);
    Emit(il, OpCodes.Ret);
    Console.WriteLine("PATCH ResourceManager.GetSkillDataPath: skillID.json");
}

static void PatchBoardConfigPath(ModuleDefinition module)
{
    var method = FindMethod(module, "ResourceManager", "GetBoardConfigPath", 2);
    var getStreaming = CallRef(method, "UnityEngine.Application", "get_streamingAssetsPath", 0);
    var combine4 = CallRef(method, "System.IO.Path", "Combine", 4);
    var combine3 = CallRef(method, "System.IO.Path", "Combine", 3);
    var toString = CallRef(method, "System.Int32", "ToString", 0);
    var concat2 = CallRef(method, "System.String", "Concat", 2);
    var body = FreshBody(method, true);
    var basePath = new VariableDefinition(module.TypeSystem.String);
    body.Variables.Add(basePath);
    var il = body.GetILProcessor();
    var adventure = Instruction.Create(OpCodes.Nop);
    var rescue = Instruction.Create(OpCodes.Nop);
    var hard = Instruction.Create(OpCodes.Nop);

    Emit(il, OpCodes.Call, getStreaming);
    Emit(il, OpCodes.Ldstr, "jsons");
    Emit(il, OpCodes.Ldstr, "BoardData");
    Emit(il, OpCodes.Ldstr, "Config");
    Emit(il, OpCodes.Call, combine4);
    Emit(il, OpCodes.Stloc, basePath);

    Emit(il, OpCodes.Ldarg, method.Parameters[0]);
    Emit(il, OpCodes.Ldc_I4_0);
    Emit(il, OpCodes.Beq, adventure);
    Emit(il, OpCodes.Ldarg, method.Parameters[0]);
    Emit(il, OpCodes.Ldc_I4_1);
    Emit(il, OpCodes.Beq, rescue);
    Emit(il, OpCodes.Ldarg, method.Parameters[0]);
    Emit(il, OpCodes.Ldc_I4_2);
    Emit(il, OpCodes.Beq, hard);
    Emit(il, OpCodes.Ldstr, string.Empty);
    Emit(il, OpCodes.Ret);

    il.Append(adventure);
    EmitBoardConfigReturn(il, basePath, "Adventure", method.Parameters[1], toString, concat2, combine3);
    il.Append(rescue);
    EmitBoardConfigReturn(il, basePath, "Rescue", method.Parameters[1], toString, concat2, combine3);
    il.Append(hard);
    EmitBoardConfigReturn(il, basePath, "Hard", method.Parameters[1], toString, concat2, combine3);
    Console.WriteLine("PATCH ResourceManager.GetBoardConfigPath: Adventure/Rescue/Hard switch + level.json");
}

static void EmitBoardConfigReturn(ILProcessor il, VariableDefinition basePath, string folder, ParameterDefinition level, MethodReference toString, MethodReference concat2, MethodReference combine3)
{
    Emit(il, OpCodes.Ldloc, basePath);
    Emit(il, OpCodes.Ldstr, folder);
    Emit(il, OpCodes.Ldarga, level);
    Emit(il, OpCodes.Call, toString);
    Emit(il, OpCodes.Ldstr, ".json");
    Emit(il, OpCodes.Call, concat2);
    Emit(il, OpCodes.Call, combine3);
    Emit(il, OpCodes.Ret);
}

static void PatchLoadFileJson(ModuleDefinition module)
{
    var method = FindMethod(module, "ResourceManager", "LoadFile_Json", 2);
    var exists = CallRef(method, "System.IO.File", "Exists", 1);
    var concat3 = CallRef(method, "System.String", "Concat", 3);
    var debugLog = CallRef(method, "UnityEngine.Debug", "Log", 1);
    var readAll = CallRef(method, "System.IO.File", "ReadAllText", 1);
    var fromJson = method.Body.Instructions.Select(i => i.Operand).OfType<GenericInstanceMethod>()
        .Single(m => m.Name == "FromJson");
    var body = FreshBody(method);
    var il = body.GetILProcessor();
    var load = Instruction.Create(OpCodes.Nop);

    Emit(il, OpCodes.Ldarg, method.Parameters[0]);
    Emit(il, OpCodes.Call, exists);
    Emit(il, OpCodes.Brtrue, load);
    Emit(il, OpCodes.Ldstr, "未找到路径为");
    Emit(il, OpCodes.Ldarg, method.Parameters[0]);
    Emit(il, OpCodes.Ldstr, "的json文件");
    Emit(il, OpCodes.Call, concat3);
    Emit(il, OpCodes.Call, debugLog);
    Emit(il, OpCodes.Ldc_I4_0);
    Emit(il, OpCodes.Ret);

    il.Append(load);
    Emit(il, OpCodes.Ldarg, method.Parameters[1]);
    Emit(il, OpCodes.Ldarg, method.Parameters[0]);
    Emit(il, OpCodes.Call, readAll);
    Emit(il, OpCodes.Call, fromJson);
    Emit(il, OpCodes.Stobj, method.GenericParameters[0]);
    Emit(il, OpCodes.Ldc_I4_1);
    Emit(il, OpCodes.Ret);
    Console.WriteLine("PATCH ResourceManager.LoadFile_Json<T>: assign JsonUtility result to ref T_this");
}

static void PatchFirstIntToStringArgument(ModuleDefinition module, string methodName, string parameterName)
{
    var method = FindMethod(module, "ResourceManager", methodName, 1);
    var callIndex = method.Body.Instructions.ToList().FindIndex(i => i.OpCode.Code is Code.Call or Code.Callvirt && i.Operand is MethodReference mr && mr.DeclaringType.FullName == "System.Int32" && mr.Name == "ToString" && mr.Parameters.Count == 0);
    if (callIndex <= 0) throw new InvalidOperationException($"Int32.ToString pattern not found in {methodName}");
    var previous = method.Body.Instructions[callIndex - 1];
    previous.OpCode = OpCodes.Ldarga;
    previous.Operand = method.Parameters.Single(p => p.Name == parameterName);
    Console.WriteLine($"PATCH ResourceManager.{methodName}: use {parameterName} for JSON filename");
}

static void VerifyNoUninitializedPathId(ModuleDefinition module, string methodName)
{
    var method = FindType(module, "ResourceManager").Methods.Single(m => m.Name == methodName);
    if (method.Body.Instructions.Any(i => i.OpCode.Code is Code.Ldloca or Code.Ldloca_S))
        throw new InvalidOperationException($"Verification failed: {methodName} still contains ldloca-based ID conversion.");
}
