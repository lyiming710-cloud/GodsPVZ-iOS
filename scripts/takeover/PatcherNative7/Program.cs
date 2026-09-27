using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const int ExpectedTypes = 320;
    const int ExpectedMethods = 2317;
    const int ExpectedFields = 2802;

    static readonly HashSet<uint> TargetTokens = new()
    {
        0x060006E3u, // TextLink.ResetLink
        0x060006E4u, // TextLink.SetLink
        0x06000498u, // Zombie.ZC_LadderPlaceEnd
        0x0600049Cu, // Zombie.ZC_LadderTestPlace
    };

    static void Main(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("usage: PatcherNative7 <formal-native4.dll> <native7-output.dll>");

        var input = Path.GetFullPath(args[0]);
        var output = Path.GetFullPath(args[1]);
        if (!File.Exists(input)) throw new FileNotFoundException("formal native4 input missing", input);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);

        using var asm = AssemblyDefinition.ReadAssembly(input, new ReaderParameters { ReadWrite = false });
        var mod = asm.MainModule;
        var inputMvid = mod.Mvid;
        AssertAssemblyIdentity(mod, "input");
        AssertTargetIdentity(mod);

        var before = SnapshotNonTargets(mod);

        var textLink = FindType(mod, "TextLink");
        var zombie = FindType(mod, "Zombie");

        PatchTextLink(mod, textLink, "ResetLink", "originalColor", 0x060006E3u);
        PatchTextLink(mod, textLink, "SetLink", "hoverColor", 0x060006E4u);
        PatchLadderTestPlace(mod, zombie);
        PatchLadderPlaceEnd(mod, zombie);

        AssertAssemblyIdentity(mod, "patched-memory");
        AssertTargetIdentity(mod);
        AssertNonTargetsUnchanged(mod, before, "patched-memory");

        asm.Write(output);

        using var reopened = AssemblyDefinition.ReadAssembly(output, new ReaderParameters { ReadWrite = false });
        if (reopened.MainModule.Mvid != inputMvid)
            throw new InvalidOperationException($"MVID changed: {inputMvid} -> {reopened.MainModule.Mvid}");
        AssertAssemblyIdentity(reopened.MainModule, "reopened");
        AssertTargetIdentity(reopened.MainModule);
        AssertNonTargetsUnchanged(reopened.MainModule, before, "reopened");

        Console.WriteLine($"NATIVE7_PATCH_PASS input_mvid={inputMvid} output_mvid={reopened.MainModule.Mvid}");
        foreach (var token in TargetTokens.OrderBy(x => x))
        {
            var m = MethodByToken(reopened.MainModule, token);
            Console.WriteLine($"TARGET 0x{token:X8} {m.FullName} code_size={m.Body.CodeSize} il={m.Body.Instructions.Count}");
        }
    }

    static IEnumerable<TypeDefinition> AllTypes(ModuleDefinition mod)
    {
        foreach (var t in mod.Types)
            foreach (var x in AllTypes(t))
                yield return x;
    }

    static IEnumerable<TypeDefinition> AllTypes(TypeDefinition root)
    {
        yield return root;
        foreach (var n in root.NestedTypes)
            foreach (var x in AllTypes(n))
                yield return x;
    }

    static void AssertAssemblyIdentity(ModuleDefinition mod, string stage)
    {
        var types = AllTypes(mod).ToArray();
        var methods = types.Sum(t => t.Methods.Count);
        var fields = types.Sum(t => t.Fields.Count);
        if (types.Length != ExpectedTypes || methods != ExpectedMethods || fields != ExpectedFields)
            throw new InvalidOperationException($"{stage}: metadata count mismatch types={types.Length} methods={methods} fields={fields}");
    }

    static TypeDefinition FindType(ModuleDefinition mod, string name) =>
        AllTypes(mod).Single(t => t.Name == name);

    static MethodDefinition MethodByToken(ModuleDefinition mod, uint token) =>
        AllTypes(mod).SelectMany(t => t.Methods).Single(m => m.MetadataToken.ToUInt32() == token);

    static void AssertTargetIdentity(ModuleDefinition mod)
    {
        var expected = new Dictionary<uint, string>
        {
            [0x060006E3u] = "ResetLink",
            [0x060006E4u] = "SetLink",
            [0x06000498u] = "ZC_LadderPlaceEnd",
            [0x0600049Cu] = "ZC_LadderTestPlace",
        };
        foreach (var kv in expected)
        {
            var m = MethodByToken(mod, kv.Key);
            if (m.Name != kv.Value || !m.HasBody)
                throw new InvalidOperationException($"target identity mismatch 0x{kv.Key:X8}: {m.FullName}");
        }
    }

    static Dictionary<uint, string> SnapshotNonTargets(ModuleDefinition mod)
    {
        var d = new Dictionary<uint, string>();
        foreach (var m in AllTypes(mod).SelectMany(t => t.Methods).Where(m => m.HasBody))
        {
            var token = m.MetadataToken.ToUInt32();
            if (!TargetTokens.Contains(token)) d[token] = BodyFingerprint(m);
        }
        return d;
    }

    static void AssertNonTargetsUnchanged(ModuleDefinition mod, Dictionary<uint, string> before, string stage)
    {
        var current = AllTypes(mod).SelectMany(t => t.Methods).Where(m => m.HasBody)
            .Where(m => !TargetTokens.Contains(m.MetadataToken.ToUInt32()))
            .ToDictionary(m => m.MetadataToken.ToUInt32(), BodyFingerprint);
        if (current.Count != before.Count) throw new InvalidOperationException($"{stage}: non-target body count changed");
        foreach (var kv in before)
            if (!current.TryGetValue(kv.Key, out var now) || now != kv.Value)
                throw new InvalidOperationException($"{stage}: non-target MethodDef changed: 0x{kv.Key:X8}");
        Console.WriteLine($"NON_TARGET_ISOLATION_PASS stage={stage} methods={before.Count}");
    }

    static string BodyFingerprint(MethodDefinition m)
    {
        var b = new StringBuilder();
        b.Append(m.Body.InitLocals).Append('|').Append(m.Body.MaxStackSize).Append('|');
        foreach (var v in m.Body.Variables) b.Append("V:").Append(v.VariableType.FullName).Append(';');
        foreach (var i in m.Body.Instructions)
        {
            b.Append(i.OpCode.Code).Append(':');
            switch (i.Operand)
            {
                case null: break;
                case Instruction x: b.Append('@').Append(x.Offset); break;
                case Instruction[] xs: foreach (var x2 in xs) b.Append('@').Append(x2.Offset).Append(','); break;
                case VariableDefinition v: b.Append("V").Append(v.Index).Append(':').Append(v.VariableType.FullName); break;
                case ParameterDefinition p: b.Append("P").Append(p.Index).Append(':').Append(p.ParameterType.FullName); break;
                case MemberReference mr: b.Append("M").Append(mr.MetadataToken.ToUInt32().ToString("X8")).Append(':').Append(mr.FullName); break;
                case TypeReference tr: b.Append("T").Append(tr.MetadataToken.ToUInt32().ToString("X8")).Append(':').Append(tr.FullName); break;
                default: b.Append(i.Operand); break;
            }
            b.Append(';');
        }
        foreach (var eh in m.Body.ExceptionHandlers)
            b.Append("EH:").Append(eh.HandlerType).Append(':').Append(eh.TryStart?.Offset).Append(':').Append(eh.TryEnd?.Offset)
             .Append(':').Append(eh.HandlerStart?.Offset).Append(':').Append(eh.HandlerEnd?.Offset).Append(':').Append(eh.FilterStart?.Offset)
             .Append(':').Append(eh.CatchType?.FullName).Append(';');
        return b.ToString();
    }

    static void ResetBody(MethodDefinition method, int maxStack)
    {
        method.Body.Instructions.Clear();
        method.Body.Variables.Clear();
        method.Body.ExceptionHandlers.Clear();
        method.Body.InitLocals = true;
        method.Body.MaxStackSize = maxStack;
    }

    static void PatchTextLink(ModuleDefinition mod, TypeDefinition textLink, string methodName, string colorFieldName, uint expectedToken)
    {
        var method = textLink.Methods.Single(m => m.Name == methodName && m.Parameters.Count == 1);
        if (method.MetadataToken.ToUInt32() != expectedToken) throw new InvalidOperationException($"{methodName} token mismatch");

        var fTmpText = textLink.Fields.Single(f => f.Name == "TMP_Text");
        var fColor = textLink.Fields.Single(f => f.Name == colorFieldName);
        var tmpScope = fTmpText.FieldType.Scope;
        var coreScope = fColor.FieldType.Scope;
        var tmpTextType = fTmpText.FieldType;
        var textInfoType = new TypeReference("TMPro", "TMP_TextInfo", mod, tmpScope);
        var linkInfoType = new TypeReference("TMPro", "TMP_LinkInfo", mod, tmpScope) { IsValueType = true };
        var charInfoType = new TypeReference("TMPro", "TMP_CharacterInfo", mod, tmpScope) { IsValueType = true };
        var meshInfoType = new TypeReference("TMPro", "TMP_MeshInfo", mod, tmpScope) { IsValueType = true };
        var color32Type = new TypeReference("UnityEngine", "Color32", mod, coreScope) { IsValueType = true };

        var getTextInfo = new MethodReference("get_textInfo", textInfoType, tmpTextType) { HasThis = true };
        var updateVertexData = mod.GetMemberReferences().OfType<MethodReference>()
            .First(m => m.DeclaringType.Name == "TMP_Text" && m.Name == "UpdateVertexData" && m.Parameters.Count == 1);
        var opImplicit = new MethodReference("op_Implicit", color32Type, color32Type) { HasThis = false };
        opImplicit.Parameters.Add(new ParameterDefinition(fColor.FieldType));

        var fLinkInfo = new FieldReference("linkInfo", new ArrayType(linkInfoType), textInfoType);
        var fLinkTextLength = new FieldReference("linkTextLength", mod.TypeSystem.Int32, linkInfoType);
        var fLinkTextFirstChar = new FieldReference("linkTextfirstCharacterIndex", mod.TypeSystem.Int32, linkInfoType);
        var fCharInfo = new FieldReference("characterInfo", new ArrayType(charInfoType), textInfoType);
        var fMatRefIdx = new FieldReference("materialReferenceIndex", mod.TypeSystem.Int32, charInfoType);
        var fVertexIdx = new FieldReference("vertexIndex", mod.TypeSystem.Int32, charInfoType);
        var fMeshInfo = new FieldReference("meshInfo", new ArrayType(meshInfoType), textInfoType);
        var fColors32 = new FieldReference("colors32", new ArrayType(color32Type), meshInfoType);

        ResetBody(method, 8);
        var body = method.Body;
        var vTextInfo = new VariableDefinition(textInfoType);
        var vLinkInfo = new VariableDefinition(linkInfoType);
        var vI = new VariableDefinition(mod.TypeSystem.Int32);
        var vCharIdx = new VariableDefinition(mod.TypeSystem.Int32);
        var vCharInfo = new VariableDefinition(charInfoType);
        var vMatIdx = new VariableDefinition(mod.TypeSystem.Int32);
        var vVertexIdx = new VariableDefinition(mod.TypeSystem.Int32);
        var vColors = new VariableDefinition(new ArrayType(color32Type));
        foreach (var v in new[] { vTextInfo, vLinkInfo, vI, vCharIdx, vCharInfo, vMatIdx, vVertexIdx, vColors }) body.Variables.Add(v);

        var il = body.GetILProcessor();
        var loopCondition = il.Create(OpCodes.Ldloc, vI);
        var loopStep = il.Create(OpCodes.Ldloc, vI);
        var loopBody = il.Create(OpCodes.Ldloca, vLinkInfo);
        var afterLoop = il.Create(OpCodes.Ldarg_0);

        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fTmpText); il.Emit(OpCodes.Callvirt, getTextInfo); il.Emit(OpCodes.Stloc, vTextInfo);
        il.Emit(OpCodes.Ldloc, vTextInfo); il.Emit(OpCodes.Ldfld, fLinkInfo); il.Emit(OpCodes.Ldarg_1); il.Emit(OpCodes.Ldelem_Any, linkInfoType); il.Emit(OpCodes.Stloc, vLinkInfo);
        il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Stloc, vI); il.Emit(OpCodes.Br, loopCondition);

        il.Append(loopBody); il.Emit(OpCodes.Ldfld, fLinkTextFirstChar); il.Emit(OpCodes.Ldloc, vI); il.Emit(OpCodes.Add); il.Emit(OpCodes.Stloc, vCharIdx);
        il.Emit(OpCodes.Ldloc, vTextInfo); il.Emit(OpCodes.Ldfld, fCharInfo); il.Emit(OpCodes.Ldloc, vCharIdx); il.Emit(OpCodes.Ldelem_Any, charInfoType); il.Emit(OpCodes.Stloc, vCharInfo);
        // PC native has no TMP_CharacterInfo.isVisible filter here.
        il.Emit(OpCodes.Ldloca, vCharInfo); il.Emit(OpCodes.Ldfld, fMatRefIdx); il.Emit(OpCodes.Stloc, vMatIdx);
        il.Emit(OpCodes.Ldloca, vCharInfo); il.Emit(OpCodes.Ldfld, fVertexIdx); il.Emit(OpCodes.Stloc, vVertexIdx);
        il.Emit(OpCodes.Ldloc, vTextInfo); il.Emit(OpCodes.Ldfld, fMeshInfo); il.Emit(OpCodes.Ldloc, vMatIdx); il.Emit(OpCodes.Ldelema, meshInfoType); il.Emit(OpCodes.Ldfld, fColors32); il.Emit(OpCodes.Stloc, vColors);

        for (int k = 0; k < 4; k++)
        {
            il.Emit(OpCodes.Ldloc, vColors); il.Emit(OpCodes.Ldloc, vVertexIdx);
            if (k != 0) { il.Emit(OpCodes.Ldc_I4, k); il.Emit(OpCodes.Add); }
            il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fColor); il.Emit(OpCodes.Call, opImplicit); il.Emit(OpCodes.Stelem_Any, color32Type);
        }

        il.Append(loopStep); il.Emit(OpCodes.Ldc_I4_1); il.Emit(OpCodes.Add); il.Emit(OpCodes.Stloc, vI);
        il.Append(loopCondition); il.Emit(OpCodes.Ldloca, vLinkInfo); il.Emit(OpCodes.Ldfld, fLinkTextLength); il.Emit(OpCodes.Blt, loopBody);
        il.Append(afterLoop); il.Emit(OpCodes.Ldfld, fTmpText); il.Emit(OpCodes.Ldc_I4, 16); il.Emit(OpCodes.Callvirt, updateVertexData); il.Emit(OpCodes.Ret);
    }

    static void PatchLadderTestPlace(ModuleDefinition mod, TypeDefinition zombie)
    {
        var method = zombie.Methods.Single(m => m.Name == "ZC_LadderTestPlace" && m.Parameters.Count == 0);
        if (method.MetadataToken.ToUInt32() != 0x0600049Cu) throw new InvalidOperationException("LadderTestPlace token mismatch");

        var fArmor2Type = zombie.Fields.Single(f => f.Name == "armor2Type");
        var fBoard = zombie.Fields.Single(f => f.Name == "board");
        var fX = zombie.Fields.Single(f => f.Name == "fX");
        var fY = zombie.Fields.Single(f => f.Name == "fY");
        var fPassablePoint = zombie.Fields.Single(f => f.Name == "passablePoint");
        var fDirection = zombie.Fields.Single(f => f.Name == "rDirection");
        var vectorType = fDirection.FieldType;
        var fVectorX = mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(f => f.DeclaringType.Name == "Vector3" && f.Name == "x") ?? new FieldReference("x", mod.TypeSystem.Single, vectorType);
        var fVectorY = mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(f => f.DeclaringType.Name == "Vector3" && f.Name == "y") ?? new FieldReference("y", mod.TypeSystem.Single, vectorType);

        var boardType = (TypeDefinition)fBoard.FieldType.Resolve();
        var fBoardConfig = boardType.Fields.Single(f => f.Name == "boardConfig");
        var mGetGrid = boardType.Methods.Single(m => m.Name == "GetGrid" && m.Parameters.Count == 2);
        var boardConfigType = (TypeDefinition)fBoardConfig.FieldType.Resolve();
        var mGetGridX = boardConfigType.Methods.Single(m => m.Name == "GetGridX" && m.Parameters.Count == 2);
        var mGetGridY = boardConfigType.Methods.Single(m => m.Name == "GetGridY" && m.Parameters.Count == 2);
        var gridType = (TypeDefinition)mGetGrid.ReturnType.Resolve();
        var mGetPassablePoint = gridType.Methods.Single(m => m.Name == "GetPassablePoint" && m.Parameters.Count == 0);

        ResetBody(method, 4);
        var vX = new VariableDefinition(mod.TypeSystem.Single);
        var vY = new VariableDefinition(mod.TypeSystem.Single);
        var vGridX = new VariableDefinition(mod.TypeSystem.Int32);
        var vGridY = new VariableDefinition(mod.TypeSystem.Int32);
        var vGrid = new VariableDefinition(mGetGrid.ReturnType);
        foreach (var v in new[] { vX, vY, vGridX, vGridY, vGrid }) method.Body.Variables.Add(v);
        var il = method.Body.GetILProcessor();
        var retFalse = il.Create(OpCodes.Ldc_I4_0);

        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fArmor2Type); il.Emit(OpCodes.Ldc_I4_4); il.Emit(OpCodes.Bne_Un, retFalse);

        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fX);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldflda, fDirection); il.Emit(OpCodes.Ldfld, fVectorX); il.Emit(OpCodes.Ldc_R4, 67f); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Sub); il.Emit(OpCodes.Stloc, vX);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fY);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldflda, fDirection); il.Emit(OpCodes.Ldfld, fVectorY); il.Emit(OpCodes.Ldc_R4, 67f); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Sub); il.Emit(OpCodes.Stloc, vY);

        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fBoard); il.Emit(OpCodes.Ldfld, fBoardConfig); il.Emit(OpCodes.Ldloc, vX); il.Emit(OpCodes.Ldloc, vY); il.Emit(OpCodes.Callvirt, mGetGridX); il.Emit(OpCodes.Stloc, vGridX);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fBoard); il.Emit(OpCodes.Ldfld, fBoardConfig); il.Emit(OpCodes.Ldloc, vX); il.Emit(OpCodes.Ldloc, vY); il.Emit(OpCodes.Callvirt, mGetGridY); il.Emit(OpCodes.Stloc, vGridY);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fBoard); il.Emit(OpCodes.Ldloc, vGridX); il.Emit(OpCodes.Ldloc, vGridY); il.Emit(OpCodes.Callvirt, mGetGrid); il.Emit(OpCodes.Stloc, vGrid);
        il.Emit(OpCodes.Ldloc, vGrid); il.Emit(OpCodes.Brfalse, retFalse);

        // Native calls Grid.GetPassablePoint twice; preserve that observable behavior.
        il.Emit(OpCodes.Ldloc, vGrid); il.Emit(OpCodes.Callvirt, mGetPassablePoint); il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fPassablePoint); il.Emit(OpCodes.Bgt, retFalse);
        il.Emit(OpCodes.Ldloc, vGrid); il.Emit(OpCodes.Callvirt, mGetPassablePoint); il.Emit(OpCodes.Ldc_I4_S, (sbyte)60); il.Emit(OpCodes.Ble, retFalse);
        il.Emit(OpCodes.Ldc_I4_1); il.Emit(OpCodes.Ret);
        il.Append(retFalse); il.Emit(OpCodes.Ret);
    }

    static void PatchLadderPlaceEnd(ModuleDefinition mod, TypeDefinition zombie)
    {
        var method = zombie.Methods.Single(m => m.Name == "ZC_LadderPlaceEnd" && m.Parameters.Count == 0);
        if (method.MetadataToken.ToUInt32() != 0x06000498u) throw new InvalidOperationException("LadderPlaceEnd token mismatch");

        var fLadderPlace = zombie.Fields.Single(f => f.Name == "ladderZombie_place");
        var fX = zombie.Fields.Single(f => f.Name == "fX");
        var fY = zombie.Fields.Single(f => f.Name == "fY");
        var fDirection = zombie.Fields.Single(f => f.Name == "rDirection");
        var fBoard = zombie.Fields.Single(f => f.Name == "board");
        var fCamp = zombie.Fields.Single(f => f.Name == "camp");
        var mTestPlace = zombie.Methods.Single(m => m.Name == "ZC_LadderTestPlace" && m.Parameters.Count == 0);
        var mDropArmor2 = zombie.Methods.Single(m => m.Name == "Drop_Armor2");
        var vectorType = fDirection.FieldType;
        var fVectorX = mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(f => f.DeclaringType.Name == "Vector3" && f.Name == "x") ?? new FieldReference("x", mod.TypeSystem.Single, vectorType);
        var fVectorY = mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(f => f.DeclaringType.Name == "Vector3" && f.Name == "y") ?? new FieldReference("y", mod.TypeSystem.Single, vectorType);

        var boardType = (TypeDefinition)fBoard.FieldType.Resolve();
        var fBoardConfig = boardType.Fields.Single(f => f.Name == "boardConfig");
        var fDeviceManager = boardType.Fields.Single(f => f.Name == "deviceManager");
        var boardConfigType = (TypeDefinition)fBoardConfig.FieldType.Resolve();
        var mGetGridX = boardConfigType.Methods.Single(m => m.Name == "GetGridX" && m.Parameters.Count == 2);
        var mGetGridY = boardConfigType.Methods.Single(m => m.Name == "GetGridY" && m.Parameters.Count == 2);

        var deviceDataType = FindType(mod, "DeviceData");
        var ctorDeviceData = deviceDataType.Methods.Single(m => m.IsConstructor && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == mod.TypeSystem.Int32.FullName);
        var fDeviceDataCamp = deviceDataType.Fields.Single(f => f.Name == "camp");
        var deviceManagerType = (TypeDefinition)fDeviceManager.FieldType.Resolve();
        var mPlaceDevice = deviceManagerType.Methods.Single(m => m.Name == "PlaceDevice");
        var deviceType = mPlaceDevice.ReturnType;

        var resourceManager = FindType(mod, "ResourceManager");
        var fZombieClips = resourceManager.Fields.Single(f => f.Name == "zombieClips");
        var globalStaticVars = FindType(mod, "GlobalStaticVars");
        var mCreateAudio = globalStaticVars.Methods.Single(m => m.Name == "CreateAudioAtPoint");
        var audioSourceType = mCreateAudio.ReturnType;
        var audioClipType = mCreateAudio.Parameters[0].ParameterType;
        var mGetItem = new MethodReference("get_Item", audioClipType, fZombieClips.FieldType) { HasThis = true };
        mGetItem.Parameters.Add(new ParameterDefinition(mod.TypeSystem.Int32));

        ResetBody(method, 4);
        var vX = new VariableDefinition(mod.TypeSystem.Single);
        var vY = new VariableDefinition(mod.TypeSystem.Single);
        var vGridX = new VariableDefinition(mod.TypeSystem.Int32);
        var vGridY = new VariableDefinition(mod.TypeSystem.Int32);
        var vDeviceData = new VariableDefinition(deviceDataType);
        var vDevice = new VariableDefinition(deviceType);
        var vClip = new VariableDefinition(audioClipType);
        var vAudio = new VariableDefinition(audioSourceType);
        foreach (var v in new[] { vX, vY, vGridX, vGridY, vDeviceData, vDevice, vClip, vAudio }) method.Body.Variables.Add(v);
        var il = method.Body.GetILProcessor();
        var ret = il.Create(OpCodes.Ret);

        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Stfld, fLadderPlace);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Call, mTestPlace); il.Emit(OpCodes.Brfalse, ret);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fX); il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldflda, fDirection); il.Emit(OpCodes.Ldfld, fVectorX); il.Emit(OpCodes.Ldc_R4, 67f); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Sub); il.Emit(OpCodes.Stloc, vX);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fY); il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldflda, fDirection); il.Emit(OpCodes.Ldfld, fVectorY); il.Emit(OpCodes.Ldc_R4, 67f); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Sub); il.Emit(OpCodes.Stloc, vY);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fBoard); il.Emit(OpCodes.Ldfld, fBoardConfig); il.Emit(OpCodes.Ldloc, vX); il.Emit(OpCodes.Ldloc, vY); il.Emit(OpCodes.Callvirt, mGetGridX); il.Emit(OpCodes.Stloc, vGridX);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fBoard); il.Emit(OpCodes.Ldfld, fBoardConfig); il.Emit(OpCodes.Ldloc, vX); il.Emit(OpCodes.Ldloc, vY); il.Emit(OpCodes.Callvirt, mGetGridY); il.Emit(OpCodes.Stloc, vGridY);
        il.Emit(OpCodes.Ldc_I4_S, (sbyte)11); il.Emit(OpCodes.Newobj, ctorDeviceData); il.Emit(OpCodes.Stloc, vDeviceData);
        il.Emit(OpCodes.Ldloc, vDeviceData); il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fCamp); il.Emit(OpCodes.Stfld, fDeviceDataCamp);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fBoard); il.Emit(OpCodes.Ldfld, fDeviceManager); il.Emit(OpCodes.Ldloc, vDeviceData); il.Emit(OpCodes.Ldloc, vGridX); il.Emit(OpCodes.Ldloc, vGridY); il.Emit(OpCodes.Callvirt, mPlaceDevice); il.Emit(OpCodes.Stloc, vDevice);
        il.Emit(OpCodes.Ldsfld, fZombieClips); il.Emit(OpCodes.Ldc_I4_5); il.Emit(OpCodes.Callvirt, mGetItem); il.Emit(OpCodes.Stloc, vClip);
        il.Emit(OpCodes.Ldloc, vClip); il.Emit(OpCodes.Call, mCreateAudio); il.Emit(OpCodes.Stloc, vAudio);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_1); il.Emit(OpCodes.Call, mDropArmor2);
        il.Append(ret);
    }
}
