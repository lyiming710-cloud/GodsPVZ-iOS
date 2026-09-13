using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9ResourceManagerLoadSpritesPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "50015e74e2225c2b3a98c83837195bcc510ea0920bd1a619bcf43c415fe9e641";
const int ExpectedMethodDefCount = 2317;
const int ExpectedFieldCount = 2802;
const uint TargetToken = 0x0600021C;
const uint StartToken = 0x06000216;
const string TargetType = "ResourceManager";

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha256(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}";
    if (operand is TypeReference tr) return $"T:{tr.FullName}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{vr.VariableType.FullName}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{pr.ParameterType.FullName}";
    if (operand is string s) return $"S:{Convert.ToBase64String(Encoding.UTF8.GetBytes(s))}";
    if (operand is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (operand is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return $"C:{Convert.ToString(operand, CultureInfo.InvariantCulture)}";
}

static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':')
          .Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':')
          .Append(Idx(h.FilterStart)).Append(':').Append(h.CatchType?.FullName ?? "").Append(';');
    }
    return sb.ToString();
}

static string FieldSemantic(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var constant = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{f.FieldType.FullName}|const={constant}|ca={attrs}";
}

static MethodReference FindCallRef(MethodDefinition m, Func<MethodReference, bool> predicate, string label)
{
    foreach (var ins in m.Body.Instructions)
        if ((ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) && ins.Operand is MethodReference mr && predicate(mr))
            return mr;
    throw new InvalidDataException($"required method reference missing: {label}");
}

static MethodReference FindAnyCallRef(IEnumerable<MethodDefinition> methods, Func<MethodReference, bool> predicate, string label)
{
    foreach (var m in methods)
        if (m.HasBody)
            foreach (var ins in m.Body.Instructions)
                if ((ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) && ins.Operand is MethodReference mr && predicate(mr))
                    return mr;
    throw new InvalidDataException($"required module method reference missing: {label}");
}

static MethodReference MakeInstanceMethod(ModuleDefinition module, TypeReference declaringType, string name, TypeReference returnType, params TypeReference[] parameters)
{
    var mr = new MethodReference(name, returnType, declaringType) { HasThis = true };
    foreach (var p in parameters) mr.Parameters.Add(new ParameterDefinition(p));
    return module.ImportReference(mr);
}

static bool IsGenericListOf(MethodReference mr, string elementFullName) =>
    mr.DeclaringType is GenericInstanceType git &&
    git.ElementType.FullName == "System.Collections.Generic.List`1" &&
    git.GenericArguments.Count == 1 && git.GenericArguments[0].FullName == elementFullName;

static bool IsLoadSprite(MethodReference mr, string spriteFullName)
{
    if (mr is not GenericInstanceMethod gim || gim.GenericArguments.Count != 1 || gim.GenericArguments[0].FullName != spriteFullName) return false;
    return gim.ElementMethod.DeclaringType.FullName == "InternalResourceLoader" && gim.ElementMethod.Name == "Load" && gim.ElementMethod.Parameters.Count == 1;
}

static int CountCalls(MethodDefinition m, Func<MethodReference, bool> predicate) =>
    m.Body.Instructions.Count(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference mr && predicate(mr));

static FieldDefinition Field(TypeDefinition t, string name) => t.Fields.SingleOrDefault(f => f.Name == name)
    ?? throw new InvalidDataException($"field missing: {t.FullName}.{name}");

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha256(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

var beforeMethodSemantics = new Dictionary<uint, string>();
var beforeFieldSemantics = new Dictionary<uint, string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
    if (fields.Count != ExpectedFieldCount) throw new InvalidDataException($"Field count drifted: {fields.Count}");
    foreach (var m in methods) beforeMethodSemantics[Raw(m)] = MethodSemantic(m);
    foreach (var f in fields) beforeFieldSemantics[Raw(f)] = FieldSemantic(f);

    var rm = types.SingleOrDefault(t => t.FullName == TargetType) ?? throw new InvalidDataException("ResourceManager missing");
    var target = rm.Methods.SingleOrDefault(m => m.Name == "LoadSprites" && m.IsStatic && m.Parameters.Count == 0)
        ?? throw new InvalidDataException("ResourceManager.LoadSprites() missing/ambiguous");
    if (Raw(target) != TargetToken) throw new InvalidDataException($"target token mismatch: 0x{Raw(target):X8}");
    if (!target.HasBody || target.Body.CodeSize != 14561) throw new InvalidDataException($"unexpected prepatch LoadSprites body size={target.Body.CodeSize}");

    var ins = target.Body.Instructions;
    bool brokenFingerprint = false;
    for (int i = 0; i + 2 < ins.Count; i++)
    {
        if (ins[i].OpCode == OpCodes.Ldsfld && ins[i].Operand is FieldReference fr && fr.Name == "plantSprites" &&
            (ins[i + 1].OpCode == OpCodes.Ldc_I4 || ins[i + 1].OpCode == OpCodes.Ldc_I4_0) &&
            ins[i + 2].OpCode == OpCodes.Ceq)
        { brokenFingerprint = true; break; }
    }
    if (!brokenFingerprint) throw new InvalidDataException("prepatch corrupt plantSprites/0/ceq fingerprint missing");

    var spriteType = Field(rm, "moneySprite").FieldType;
    const string SpriteName = "UnityEngine.Sprite";
    if (spriteType.FullName != SpriteName) throw new InvalidDataException($"Sprite type drift: {spriteType.FullName}");

    var helperSprites = rm.Methods.Single(m => m.Name == "Load_card_Choose_Sprites" && m.IsStatic && m.Parameters.Count == 0);
    var helperPlantPortraits = rm.Methods.Single(m => m.Name == "Load_card_Choose_PlantPortraits" && m.IsStatic && m.Parameters.Count == 0);
    var helperDevicePortraits = rm.Methods.Single(m => m.Name == "Load_card_Choose_DevicePortraits" && m.IsStatic && m.Parameters.Count == 0);
    var helperWindowPortraits = rm.Methods.Single(m => m.Name == "Load_devicePortraits_Window" && m.IsStatic && m.Parameters.Count == 0);

    var loadSprite = FindCallRef(target, mr => IsLoadSprite(mr, SpriteName), "InternalResourceLoader.Load<Sprite>");
    var combine2 = FindCallRef(target, mr => mr.DeclaringType.FullName == "System.IO.Path" && mr.Name == "Combine" && mr.Parameters.Count == 2, "Path.Combine/2");
    var combine3 = FindCallRef(target, mr => mr.DeclaringType.FullName == "System.IO.Path" && mr.Name == "Combine" && mr.Parameters.Count == 3, "Path.Combine/3");
    var intToString = FindCallRef(target, mr => mr.DeclaringType.FullName == "System.Int32" && mr.Name == "ToString" && mr.Parameters.Count == 0, "Int32.ToString");
    var concat2 = FindCallRef(target, mr => mr.DeclaringType.FullName == "System.String" && mr.Name == "Concat" && mr.Parameters.Count == 2 && mr.Parameters.All(p => p.ParameterType.FullName == "System.String"), "String.Concat/2");
    var concat3 = FindCallRef(target, mr => mr.DeclaringType.FullName == "System.String" && mr.Name == "Concat" && mr.Parameters.Count == 3 && mr.Parameters.All(p => p.ParameterType.FullName == "System.String"), "String.Concat/3");
    var typeFromHandle = FindCallRef(target, mr => mr.DeclaringType.FullName == "System.Type" && mr.Name == "GetTypeFromHandle", "Type.GetTypeFromHandle");
    var enumGetName = FindCallRef(target, mr => mr.DeclaringType.FullName == "System.Enum" && mr.Name == "GetName" && mr.Parameters.Count == 2, "Enum.GetName");
    var objectEquality = FindCallRef(target, mr => mr.DeclaringType.FullName == "UnityEngine.Object" && mr.Name == "op_Equality", "UnityEngine.Object.op_Equality");
    var objectImplicit = FindCallRef(target, mr => mr.DeclaringType.FullName == "UnityEngine.Object" && mr.Name == "op_Implicit", "UnityEngine.Object.op_Implicit");
    var debugLog = FindCallRef(target, mr => mr.DeclaringType.FullName == "UnityEngine.Debug" && mr.Name == "Log" && mr.Parameters.Count == 1, "Debug.Log");

    var plantSprites = Field(rm, "plantSprites");
    var zombieSprites = Field(rm, "zombieSprites");
    var itemSprites = Field(rm, "itemSprites");
    var vfxSprites = Field(rm, "vfxSprites");
    var cliqueLogos = Field(rm, "cliqueLogos");
    var deviceSprites = Field(rm, "deviceSprites");
    var levelA = Field(rm, "levelSprites_A");
    var levelR = Field(rm, "levelSprites_R");
    var levelHA = Field(rm, "levelSprites_H_A");
    var levelHB = Field(rm, "levelSprites_H_B");
    var levelHC = Field(rm, "levelSprites_H_C");
    var levelBA = Field(rm, "levelSprites_BA");
    var propSprites = Field(rm, "propSprites");
    var moneySprite = Field(rm, "moneySprite");
    var cardPlant = Field(rm, "cardBackground_plant");
    var cardDevice = Field(rm, "cardBackground_device");
    var suppliesInitialValue = Field(rm, "suppliesInitialValue");

    var suppliesInitialType = types.Single(t => t.FullName == "SuppliesInitialValue");
    var suppliesInfoType = types.Single(t => t.FullName == "SuppliesInfo");
    var suppliesInfos = Field(suppliesInitialType, "suppliesInfos");
    var suppliesName = Field(suppliesInfoType, "name");
    var suppliesListType = suppliesInfos.FieldType;
    var suppliesGetCount = MakeInstanceMethod(module, suppliesListType, "get_Count", module.TypeSystem.Int32);
    var suppliesGetItem = FindAnyCallRef(methods, mr => mr.Name == "get_Item" && IsGenericListOf(mr, suppliesInfoType.FullName) && mr.Parameters.Count == 1, "List<SuppliesInfo>.get_Item");

    var spriteAdd = FindAnyCallRef(methods, mr => mr.Name == "Add" && IsGenericListOf(mr, SpriteName) && mr.Parameters.Count == 1, "List<Sprite>.Add");
    var spriteGetCount = FindAnyCallRef(methods, mr => mr.Name == "get_Count" && IsGenericListOf(mr, SpriteName) && mr.Parameters.Count == 0, "List<Sprite>.get_Count");
    var spriteGetItem = FindAnyCallRef(methods, mr => mr.Name == "get_Item" && IsGenericListOf(mr, SpriteName) && mr.Parameters.Count == 1, "List<Sprite>.get_Item");
    var spriteSetItem = FindAnyCallRef(methods, mr => mr.Name == "set_Item" && IsGenericListOf(mr, SpriteName) && mr.Parameters.Count == 2, "List<Sprite>.set_Item");

    var cliqueType = types.Single(t => t.FullName == "Clique");
    var cliqueConstants = cliqueType.Fields.Where(f => f.IsStatic && f.HasConstant)
        .ToDictionary(f => f.Name, f => Convert.ToInt32(f.Constant, CultureInfo.InvariantCulture));
    var expectedClique = new Dictionary<string,int> { ["None"] = 0, ["Technological"] = 1, ["Historical"] = 2, ["Mystical"] = 3, ["ChineseMystical"] = 4, ["Visitor"] = 5 };
    if (cliqueConstants.Count != expectedClique.Count || expectedClique.Any(kv => !cliqueConstants.TryGetValue(kv.Key, out var v) || v != kv.Value))
        throw new InvalidDataException("Clique enum values drifted from exact 1.0.2 0..5 authority");

    Console.WriteLine($"TARGET_METHOD token=0x{Raw(target):X8} type=ResourceManager method=LoadSprites code_size={target.Body.CodeSize} broken_fingerprint=plantSprites_ldc0_ceq");
    Console.WriteLine("NATIVE_AUTHORITY token=0x0600021C address=0x0000000180333DB0 load_sprite_calls=88 list_add_calls=85 path_combine2_calls=92 path_combine3_calls=4 helper_calls=4");
    Console.WriteLine("RUNTIME_CAUSAL_EVIDENCE strict_run=34765343442 candidate=50015e74e2225c2b3a98c83837195bcc510ea0920bd1a619bcf43c415fe9e641 exception=InvalidProgramException method=ResourceManager.LoadSprites il_offset=0x0113 opcode=ceq");

    target.Body.Instructions.Clear(); target.Body.Variables.Clear(); target.Body.ExceptionHandlers.Clear(); target.Body.InitLocals = true; target.Body.MaxStackSize = 8;
    var basePath=new VariableDefinition(module.TypeSystem.String); var index=new VariableDefinition(module.TypeSystem.Int32); var cliqueName=new VariableDefinition(module.TypeSystem.String); var cliqueSprite=new VariableDefinition(spriteType);
    target.Body.Variables.Add(basePath); target.Body.Variables.Add(index); target.Body.Variables.Add(cliqueName); target.Body.Variables.Add(cliqueSprite);
    var il=target.Body.GetILProcessor();
    Instruction Make(OpCode op,object? operand=null)=>operand switch{null=>il.Create(op),string s=>il.Create(op,s),int n=>il.Create(op,n),Instruction d=>il.Create(op,d),MethodReference mr=>il.Create(op,mr),FieldReference fr=>il.Create(op,fr),TypeReference tr=>il.Create(op,tr),VariableDefinition vr=>il.Create(op,vr),_=>throw new InvalidDataException($"unsupported IL operand {operand.GetType().FullName}")};
    void Emit(OpCode op,object? operand=null)=>il.Append(Make(op,operand));
    Emit(OpCodes.Call,helperSprites); Emit(OpCodes.Call,helperPlantPortraits); Emit(OpCodes.Pop); Emit(OpCodes.Call,helperDevicePortraits); Emit(OpCodes.Pop); Emit(OpCodes.Call,helperWindowPortraits); Emit(OpCodes.Pop);
    void LF2(FieldDefinition d,string a,string b){Emit(OpCodes.Ldstr,a);Emit(OpCodes.Ldstr,b);Emit(OpCodes.Call,combine2);Emit(OpCodes.Call,loadSprite);Emit(OpCodes.Stsfld,d);} void LF3(FieldDefinition d,string a,string b,string c){Emit(OpCodes.Ldstr,a);Emit(OpCodes.Ldstr,b);Emit(OpCodes.Ldstr,c);Emit(OpCodes.Call,combine3);Emit(OpCodes.Call,loadSprite);Emit(OpCodes.Stsfld,d);} void B2(string a,string b){Emit(OpCodes.Ldstr,a);Emit(OpCodes.Ldstr,b);Emit(OpCodes.Call,combine2);Emit(OpCodes.Stloc,basePath);} void B3(string a,string b,string c){Emit(OpCodes.Ldstr,a);Emit(OpCodes.Ldstr,b);Emit(OpCodes.Ldstr,c);Emit(OpCodes.Call,combine3);Emit(OpCodes.Stloc,basePath);} void Add(FieldDefinition f,string n){Emit(OpCodes.Ldsfld,f);Emit(OpCodes.Ldloc,basePath);Emit(OpCodes.Ldstr,n);Emit(OpCodes.Call,combine2);Emit(OpCodes.Call,loadSprite);Emit(OpCodes.Callvirt,spriteAdd);}
    LF2(moneySprite,"sprites","Dollar"); LF3(cardPlant,"sprites","Card","SeedPacket_Larger_0"); LF3(cardDevice,"sprites","Card","SeedPacket_Larger_1");
    string[] pn={"KnightWallnut_body","KnightWallnut_body2","KnightWallnut_cedun1_1","KnightWallnut_cedun1_2","KnightWallnut_cedun1_3","KnightWallnut_dunpai1_1","KnightWallnut_dunpai1_2","KnightWallnut_dunpai1_3","KnightWallnut_pifeng1_1","KnightWallnut_pifeng1_2","KnightWallnut_pifeng2_1","KnightWallnut_pifeng2_2","KnightWallnut_pifeng3_1","KnightWallnut_pifeng3_2","KnightWallnut_toukui1_1","KnightWallnut_toukui1_2","KnightWallnut_toukui2_1","KnightWallnut_toukui2_2","KnightWallnut_toukui2_3","VikingChomper_Barrels","VikingChomper_Barrels1","VikingChomper_Barrels2"}; B2("sprites","Plant"); foreach(var n in pn)Add(plantSprites,n);
    string[] zn={"Zombie_cone1","Zombie_cone2","Zombie_cone3","Zombie_bucket1","Zombie_bucket2","Zombie_bucket3","Zombie_brick1","Zombie_brick2","Zombie_brick3","Zombie_screendoor1","Zombie_screendoor2","Zombie_screendoor3","Zombie_outerarm_upper2","Zombie_flag3","IceCube1","IceCube2","IceCube2_1","IceCube3","IceCube3_1","Wallnut_body","Wallnut_cracked1","Wallnut_cracked2","Wallnut_boom_body","Wallnut_boom_cracked1","Wallnut_boom_cracked2","Zombie_polevaulter_outerarm_upper2","Zombie_HeavyInfantry_bucket1","Zombie_HeavyInfantry_bucket2","Zombie_HeavyInfantry_bucket3","Zombie_HeavyInfantry_door1","Zombie_HeavyInfantry_door2","Zombie_HeavyInfantry_door3","Zombie_poleCommander_outerarm_upper2","LadderSaboteurs_ladder_1","LadderSaboteurs_ladder_2","LadderSaboteurs_ladder_3","LadderSaboteurs_helmet1","LadderSaboteurs_helmet2","LadderSaboteurs_helmet3","LadderSaboteurs_mask1","LadderSaboteurs_mask2","LadderSaboteurs_mask3","LadderSaboteurs_outerarm_upper2"}; B2("sprites","Zombie"); foreach(var n in zn)Add(zombieSprites,n);
    B2("sprites","Supplies"); Emit(OpCodes.Ldc_I4_0);Emit(OpCodes.Stloc,index);var sb=il.Create(OpCodes.Nop);var scn=il.Create(OpCodes.Nop);Emit(OpCodes.Br,scn);il.Append(sb);Emit(OpCodes.Ldsfld,itemSprites);Emit(OpCodes.Ldloc,basePath);Emit(OpCodes.Ldsfld,suppliesInitialValue);Emit(OpCodes.Ldfld,suppliesInfos);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Callvirt,suppliesGetItem);Emit(OpCodes.Ldfld,suppliesName);Emit(OpCodes.Call,combine2);Emit(OpCodes.Call,loadSprite);Emit(OpCodes.Callvirt,spriteAdd);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Ldc_I4_1);Emit(OpCodes.Add);Emit(OpCodes.Stloc,index);il.Append(scn);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Ldsfld,suppliesInitialValue);Emit(OpCodes.Ldfld,suppliesInfos);Emit(OpCodes.Callvirt,suppliesGetCount);Emit(OpCodes.Blt,sb);
    B2("sprites","VFX");foreach(var n in new[]{"IceCube_Large","IceCube1","IceCube2","IceCube2_1","IceCube3","IceCube3_1","icetrap"})Add(vfxSprites,n);
    B3("sprites","UI","CliqueLogo");Emit(OpCodes.Ldc_I4_0);Emit(OpCodes.Stloc,index);var cb=il.Create(OpCodes.Nop);var cn=il.Create(OpCodes.Nop);var cc=il.Create(OpCodes.Nop);var fc=il.Create(OpCodes.Nop);var fd=il.Create(OpCodes.Nop);var nl=il.Create(OpCodes.Nop);var hs=il.Create(OpCodes.Nop);Emit(OpCodes.Br,cc);il.Append(cb);il.Append(fc);Emit(OpCodes.Ldsfld,cliqueLogos);Emit(OpCodes.Callvirt,spriteGetCount);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Bgt,fd);Emit(OpCodes.Ldsfld,cliqueLogos);Emit(OpCodes.Ldnull);Emit(OpCodes.Callvirt,spriteAdd);Emit(OpCodes.Br,fc);il.Append(fd);Emit(OpCodes.Ldsfld,cliqueLogos);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Callvirt,spriteGetItem);Emit(OpCodes.Ldnull);Emit(OpCodes.Call,objectEquality);Emit(OpCodes.Brtrue,nl);Emit(OpCodes.Br,cn);il.Append(nl);Emit(OpCodes.Ldtoken,cliqueType);Emit(OpCodes.Call,typeFromHandle);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Box,cliqueType);Emit(OpCodes.Call,enumGetName);Emit(OpCodes.Stloc,cliqueName);Emit(OpCodes.Ldloc,basePath);Emit(OpCodes.Ldloc,cliqueName);Emit(OpCodes.Call,combine2);Emit(OpCodes.Call,loadSprite);Emit(OpCodes.Stloc,cliqueSprite);Emit(OpCodes.Ldloc,cliqueSprite);Emit(OpCodes.Call,objectImplicit);Emit(OpCodes.Brtrue,hs);Emit(OpCodes.Ldstr,"加载派系图标");Emit(OpCodes.Ldloc,cliqueName);Emit(OpCodes.Ldstr,"失败");Emit(OpCodes.Call,concat3);Emit(OpCodes.Call,debugLog);Emit(OpCodes.Br,cn);il.Append(hs);Emit(OpCodes.Ldsfld,cliqueLogos);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Ldloc,cliqueSprite);Emit(OpCodes.Callvirt,spriteSetItem);il.Append(cn);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Ldc_I4_1);Emit(OpCodes.Add);Emit(OpCodes.Stloc,index);il.Append(cc);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Ldc_I4_6);Emit(OpCodes.Blt,cb);
    B2("sprites","Device");foreach(var n in new[]{"Roadblock","Roadblock1","Roadblock2"})Add(deviceSprites,n);B3("sprites","UI","LevelInside");
    void LA(FieldDefinition f,string p){Emit(OpCodes.Ldsfld,f);Emit(OpCodes.Ldloc,basePath);Emit(OpCodes.Ldstr,p);Emit(OpCodes.Ldloca,index);Emit(OpCodes.Call,intToString);Emit(OpCodes.Call,concat2);Emit(OpCodes.Call,combine2);Emit(OpCodes.Call,loadSprite);Emit(OpCodes.Callvirt,spriteAdd);} void LL(int lim,params(FieldDefinition f,string p)[] x){Emit(OpCodes.Ldc_I4_0);Emit(OpCodes.Stloc,index);var b=il.Create(OpCodes.Nop);var c=il.Create(OpCodes.Nop);Emit(OpCodes.Br,c);il.Append(b);foreach(var z in x)LA(z.f,z.p);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Ldc_I4_1);Emit(OpCodes.Add);Emit(OpCodes.Stloc,index);il.Append(c);Emit(OpCodes.Ldloc,index);Emit(OpCodes.Ldc_I4,lim);Emit(OpCodes.Blt,b);} LL(100,(levelA,"A-"));LL(50,(levelR,"R-"));LL(50,(levelHA,"HA_A_"),(levelHB,"HA_B_"),(levelHC,"HA_C_"));LL(50,(levelBA,"BA-"));B2("sprites","Prop");Add(propSprites,"Shovel");Add(propSprites,"Glove");Emit(OpCodes.Ret);
    module.Write(output); Console.WriteLine("PATCH_RESOURCE_MANAGER_LOADSPRITES method_body_changes=1 field_metadata_changes=0 source=pc_native_plus_exact_resource_sequence");
}

var outputSha=Sha256(output);Console.WriteLine($"OUTPUT_SHA256 {outputSha}");if(outputSha==ExpectedInputSha256)throw new InvalidDataException("output SHA unexpectedly identical to input");
using(var after=ModuleDefinition.ReadModule(output,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate}))
{
    var types=AllTypes(after.Types).ToList();var methods=types.SelectMany(t=>t.Methods).ToList();var fields=types.SelectMany(t=>t.Fields).ToList();if(methods.Count!=ExpectedMethodDefCount||fields.Count!=ExpectedFieldCount)throw new InvalidDataException($"postwrite counts methods={methods.Count} fields={fields.Count}");var rm=types.Single(t=>t.FullName==TargetType);var target=rm.Methods.Single(m=>Raw(m)==TargetToken);const string SpriteName="UnityEngine.Sprite";
    int loadCount=CountCalls(target,mr=>IsLoadSprite(mr,SpriteName));int addCount=CountCalls(target,mr=>mr.Name=="Add"&&IsGenericListOf(mr,SpriteName));int c2=CountCalls(target,mr=>mr.DeclaringType.FullName=="System.IO.Path"&&mr.Name=="Combine"&&mr.Parameters.Count==2);int c3=CountCalls(target,mr=>mr.DeclaringType.FullName=="System.IO.Path"&&mr.Name=="Combine"&&mr.Parameters.Count==3);if(loadCount!=88||addCount!=85||c2!=92||c3!=4)throw new InvalidDataException($"native call-shape mismatch load={loadCount} add={addCount} combine2={c2} combine3={c3}");foreach(var h in new[]{"Load_card_Choose_Sprites","Load_card_Choose_PlantPortraits","Load_card_Choose_DevicePortraits","Load_devicePortraits_Window"})if(CountCalls(target,mr=>mr.DeclaringType.FullName==TargetType&&mr.Name==h)!=1)throw new InvalidDataException($"helper mismatch {h}");
    var strings=target.Body.Instructions.Where(i=>i.OpCode==OpCodes.Ldstr).Select(i=>(string)i.Operand).ToList();foreach(var r in new[]{"Dollar","SeedPacket_Larger_0","SeedPacket_Larger_1","KnightWallnut_body","VikingChomper_Barrels2","Zombie_cone1","LadderSaboteurs_outerarm_upper2","Supplies","IceCube_Large","icetrap","CliqueLogo","Roadblock2","LevelInside","A-","R-","HA_A_","HA_B_","HA_C_","BA-","Shovel","Glove"})if(!strings.Contains(r))throw new InvalidDataException($"resource missing {r}");
    var start=rm.Methods.Single(m=>Raw(m)==StartToken);var audio=rm.Methods.Single(m=>m.Name=="LoadAudioClips"&&m.IsStatic&&m.Parameters.Count==0);var ac=start.Body.Instructions.Where(i=>(i.OpCode==OpCodes.Call||i.OpCode==OpCodes.Callvirt)&&i.Operand is MethodReference mr&&mr.FullName==audio.FullName).ToList();var sc=start.Body.Instructions.Where(i=>(i.OpCode==OpCodes.Call||i.OpCode==OpCodes.Callvirt)&&i.Operand is MethodReference mr&&mr.FullName==target.FullName).ToList();if(ac.Count!=1||sc.Count!=1||start.Body.Instructions.IndexOf(sc[0])!=start.Body.Instructions.IndexOf(ac[0])+1)throw new InvalidDataException("Start preservation failed");
    int changed=0,untouched=0;foreach(var m in methods){if(!beforeMethodSemantics.TryGetValue(Raw(m),out var old))throw new InvalidDataException($"new method 0x{Raw(m):X8}");if(old==MethodSemantic(m))untouched++;else{changed++;if(Raw(m)!=TargetToken)throw new InvalidDataException($"unexpected method drift 0x{Raw(m):X8}");}}if(changed!=1||untouched!=2316)throw new InvalidDataException($"isolation mismatch untouched={untouched} changed={changed}");foreach(var f in fields)if(!beforeFieldSemantics.TryGetValue(Raw(f),out var old)||old!=FieldSemantic(f))throw new InvalidDataException($"field drift 0x{Raw(f):X8}");var card=types.Single(t=>t.FullName=="Card_Choose");if(card.Fields.Count(f=>f.CustomAttributes.Any(a=>a.AttributeType.FullName=="UnityEngine.SerializeField"))!=17)throw new InvalidDataException("Card_Choose SerializeField preservation failed");
    Console.WriteLine($"REOPEN_RESOURCE_MANAGER_LOADSPRITES_PASS token=0x{TargetToken:X8} load_sprite_calls={loadCount} list_add_calls={addCount} path_combine2_calls={c2} path_combine3_calls={c3}");Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouched} changed_methods={changed} target_token=0x{TargetToken:X8}");Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={fields.Count} changed_fields=0");Console.WriteLine("START_RECOVERY_PRESERVATION_PASS load_audio_calls=1 load_sprites_calls=1 adjacent=1");Console.WriteLine("CARD_CHOOSE_SERIALIZEFIELD_PRESERVATION_PASS fields=17");
}
return 0;
