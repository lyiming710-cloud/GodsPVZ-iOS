using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main(string[] args)
    {
        string inPath = args.Length > 0 ? args[0] : "/workspaces/GodsPVZ-iOS/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native4-six-method.dll";
        string outPath5 = "/workspaces/GodsPVZ-iOS/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native5-textlink.dll";
        string outPath6 = "/workspaces/GodsPVZ-iOS/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native6-ladder.dll";

        Console.WriteLine($"[*] Reading assembly: {inPath}");
        var rp = new ReaderParameters { ReadWrite = false };
        using var asm = AssemblyDefinition.ReadAssembly(inPath, rp);
        var mod = asm.MainModule;

        // ==========================================
        // 1 & 2: Patch TextLink::ResetLink & SetLink
        // ==========================================
        var textLinkType = mod.Types.FirstOrDefault(t => t.Name == "TextLink")
            ?? throw new Exception("TextLink type not found!");

        Console.WriteLine($"[+] Found TextLink type (Token: 0x{textLinkType.MetadataToken.ToInt32():X8})");

        var fTmpText = textLinkType.Fields.First(f => f.Name == "TMP_Text");
        var fOriginalColor = textLinkType.Fields.First(f => f.Name == "originalColor");
        var fHoverColor = textLinkType.Fields.First(f => f.Name == "hoverColor");

        var resetLinkMethod = textLinkType.Methods.First(m => m.Name == "ResetLink");
        var setLinkMethod = textLinkType.Methods.First(m => m.Name == "SetLink");

        var tmpAsmScope = fTmpText.FieldType.Scope;
        var coreAsmScope = fOriginalColor.FieldType.Scope;

        var tmpTextType = fTmpText.FieldType;
        var textInfoType = new TypeReference("TMPro", "TMP_TextInfo", mod, tmpAsmScope);
        var linkInfoType = new TypeReference("TMPro", "TMP_LinkInfo", mod, tmpAsmScope) { IsValueType = true };
        var charInfoType = new TypeReference("TMPro", "TMP_CharacterInfo", mod, tmpAsmScope) { IsValueType = true };
        var meshInfoType = new TypeReference("TMPro", "TMP_MeshInfo", mod, tmpAsmScope) { IsValueType = true };
        var color32Type = new TypeReference("UnityEngine", "Color32", mod, coreAsmScope) { IsValueType = true };

        var getTextInfo = new MethodReference("get_textInfo", textInfoType, tmpTextType)
        {
            HasThis = true,
            ExplicitThis = false,
            CallingConvention = MethodCallingConvention.Default
        };

        var updateVertexData = mod.GetMemberReferences().OfType<MethodReference>().First(m => m.DeclaringType.Name == "TMP_Text" && m.Name == "UpdateVertexData");

        var opImplicit = new MethodReference("op_Implicit", color32Type, color32Type)
        {
            HasThis = false,
            ExplicitThis = false,
            CallingConvention = MethodCallingConvention.Default
        };
        opImplicit.Parameters.Add(new ParameterDefinition(fOriginalColor.FieldType));

        var fLinkInfo = new FieldReference("linkInfo", new ArrayType(linkInfoType), textInfoType);
        var fLinkTextLength = new FieldReference("linkTextLength", mod.TypeSystem.Int32, linkInfoType);
        var fLinkTextFirstChar = new FieldReference("linkTextfirstCharacterIndex", mod.TypeSystem.Int32, linkInfoType);

        var fCharInfo = new FieldReference("characterInfo", new ArrayType(charInfoType), textInfoType);
        var fIsVisible = new FieldReference("isVisible", mod.TypeSystem.Boolean, charInfoType);
        var fMatRefIdx = new FieldReference("materialReferenceIndex", mod.TypeSystem.Int32, charInfoType);
        var fVertexIdx = new FieldReference("vertexIndex", mod.TypeSystem.Int32, charInfoType);

        var fMeshInfo = new FieldReference("meshInfo", new ArrayType(meshInfoType), textInfoType);
        var fColors32 = new FieldReference("colors32", new ArrayType(color32Type), meshInfoType);

        PatchTextLink(resetLinkMethod, mod, fTmpText, fOriginalColor, getTextInfo, textInfoType, fLinkInfo, linkInfoType, fLinkTextLength, fLinkTextFirstChar, fCharInfo, charInfoType, fIsVisible, fMatRefIdx, fVertexIdx, fMeshInfo, meshInfoType, fColors32, color32Type, opImplicit, updateVertexData);
        PatchTextLink(setLinkMethod, mod, fTmpText, fHoverColor, getTextInfo, textInfoType, fLinkInfo, linkInfoType, fLinkTextLength, fLinkTextFirstChar, fCharInfo, charInfoType, fIsVisible, fMatRefIdx, fVertexIdx, fMeshInfo, meshInfoType, fColors32, color32Type, opImplicit, updateVertexData);

        Console.WriteLine($"[+] TextLink::ResetLink patched: Locals={resetLinkMethod.Body.Variables.Count}, MaxStack={resetLinkMethod.Body.MaxStackSize}");
        Console.WriteLine($"[+] TextLink::SetLink patched:   Locals={setLinkMethod.Body.Variables.Count}, MaxStack={setLinkMethod.Body.MaxStackSize}");

        // ==========================================
        // 3: Patch Zombie::ZC_LadderPlaceEnd (0x06000498)
        // ==========================================
        var zombieType = mod.Types.FirstOrDefault(t => t.Name == "Zombie")
            ?? throw new Exception("Zombie type not found!");

        var ladderPlaceEndMethod = zombieType.Methods.FirstOrDefault(m => m.Name == "ZC_LadderPlaceEnd")
            ?? throw new Exception("Zombie::ZC_LadderPlaceEnd method not found!");

        Console.WriteLine($"[+] Found Zombie::ZC_LadderPlaceEnd (Token: 0x{ladderPlaceEndMethod.MetadataToken.ToInt32():X8}), original Locals={ladderPlaceEndMethod.Body.Variables.Count}, CodeSize={ladderPlaceEndMethod.Body.CodeSize}");

        PatchLadderPlaceEnd(ladderPlaceEndMethod, mod, zombieType);

        Console.WriteLine($"[+] Zombie::ZC_LadderPlaceEnd patched: Locals={ladderPlaceEndMethod.Body.Variables.Count}, MaxStack={ladderPlaceEndMethod.Body.MaxStackSize}");

        // Write outputs
        Console.WriteLine($"[*] Writing patched candidate assemblies...");
        asm.Write(outPath5);
        asm.Write(outPath6);
        Console.WriteLine($"[SUCCESS] Successfully written {outPath5} and {outPath6}!");

        File.Copy("/workspaces/GodsPVZ-iOS/scripts/codespaces/Patcher/Program.cs", "/workspaces/GodsPVZ-iOS/scripts/codespaces/patch_textlink_and_ladder.cs", true);
        Console.WriteLine("[+] Saved copy to /workspaces/GodsPVZ-iOS/scripts/codespaces/patch_textlink_and_ladder.cs");
    }

    static void PatchTextLink(
        MethodDefinition method,
        ModuleDefinition mod,
        FieldDefinition fTmpText,
        FieldDefinition fColor,
        MethodReference getTextInfo,
        TypeReference textInfoType,
        FieldReference fLinkInfo,
        TypeReference linkInfoType,
        FieldReference fLinkTextLength,
        FieldReference fLinkTextFirstChar,
        FieldReference fCharInfo,
        TypeReference charInfoType,
        FieldReference fIsVisible,
        FieldReference fMatRefIdx,
        FieldReference fVertexIdx,
        FieldReference fMeshInfo,
        TypeReference meshInfoType,
        FieldReference fColors32,
        TypeReference color32Type,
        MethodReference opImplicit,
        MethodReference updateVertexData)
    {
        var body = method.Body;
        body.Instructions.Clear();
        body.Variables.Clear();
        body.ExceptionHandlers.Clear();
        body.InitLocals = true;
        body.MaxStackSize = 8;

        var v0_textInfo = new VariableDefinition(textInfoType);
        var v1_linkInfo = new VariableDefinition(linkInfoType);
        var v2_i = new VariableDefinition(mod.TypeSystem.Int32);
        var v3_charIdx = new VariableDefinition(mod.TypeSystem.Int32);
        var v4_charInfo = new VariableDefinition(charInfoType);
        var v5_matIdx = new VariableDefinition(mod.TypeSystem.Int32);
        var v6_vIdx = new VariableDefinition(mod.TypeSystem.Int32);
        var v7_colors32 = new VariableDefinition(new ArrayType(color32Type));

        body.Variables.Add(v0_textInfo);
        body.Variables.Add(v1_linkInfo);
        body.Variables.Add(v2_i);
        body.Variables.Add(v3_charIdx);
        body.Variables.Add(v4_charInfo);
        body.Variables.Add(v5_matIdx);
        body.Variables.Add(v6_vIdx);
        body.Variables.Add(v7_colors32);

        var il = body.GetILProcessor();

        var loopCondition = il.Create(OpCodes.Ldloc_2);
        var loopStep = il.Create(OpCodes.Ldloc_2);
        var loopBody = il.Create(OpCodes.Ldloca_S, v1_linkInfo);
        var afterLoop = il.Create(OpCodes.Ldarg_0);

        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, fTmpText);
        il.Emit(OpCodes.Callvirt, getTextInfo);
        il.Emit(OpCodes.Stloc_0);

        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ldfld, fLinkInfo);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldelem_Any, linkInfoType);
        il.Emit(OpCodes.Stloc_1);

        il.Emit(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Stloc_2);
        il.Emit(OpCodes.Br, loopCondition);

        il.Append(loopBody);
        il.Emit(OpCodes.Ldfld, fLinkTextFirstChar);
        il.Emit(OpCodes.Ldloc_2);
        il.Emit(OpCodes.Add);
        il.Emit(OpCodes.Stloc_3);

        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ldfld, fCharInfo);
        il.Emit(OpCodes.Ldloc_3);
        il.Emit(OpCodes.Ldelem_Any, charInfoType);
        il.Emit(OpCodes.Stloc_S, v4_charInfo);

        il.Emit(OpCodes.Ldloca_S, v4_charInfo);
        il.Emit(OpCodes.Ldfld, fIsVisible);
        il.Emit(OpCodes.Brfalse, loopStep);

        il.Emit(OpCodes.Ldloca_S, v4_charInfo);
        il.Emit(OpCodes.Ldfld, fMatRefIdx);
        il.Emit(OpCodes.Stloc_S, v5_matIdx);

        il.Emit(OpCodes.Ldloca_S, v4_charInfo);
        il.Emit(OpCodes.Ldfld, fVertexIdx);
        il.Emit(OpCodes.Stloc_S, v6_vIdx);

        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ldfld, fMeshInfo);
        il.Emit(OpCodes.Ldloc_S, v5_matIdx);
        il.Emit(OpCodes.Ldelema, meshInfoType);
        il.Emit(OpCodes.Ldfld, fColors32);
        il.Emit(OpCodes.Stloc_S, v7_colors32);

        for (int k = 0; k < 4; k++)
        {
            il.Emit(OpCodes.Ldloc_S, v7_colors32);
            il.Emit(OpCodes.Ldloc_S, v6_vIdx);
            if (k > 0)
            {
                il.Emit(OpCodes.Ldc_I4, k);
                il.Emit(OpCodes.Add);
            }
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fColor);
            il.Emit(OpCodes.Call, opImplicit);
            il.Emit(OpCodes.Stelem_Any, color32Type);
        }

        il.Append(loopStep);
        il.Emit(OpCodes.Ldc_I4_1);
        il.Emit(OpCodes.Add);
        il.Emit(OpCodes.Stloc_2);

        il.Append(loopCondition);
        il.Emit(OpCodes.Ldloca_S, v1_linkInfo);
        il.Emit(OpCodes.Ldfld, fLinkTextLength);
        il.Emit(OpCodes.Blt, loopBody);

        il.Append(afterLoop);
        il.Emit(OpCodes.Ldfld, fTmpText);
        il.Emit(OpCodes.Ldc_I4, 16);
        il.Emit(OpCodes.Callvirt, updateVertexData);

        il.Emit(OpCodes.Ret);
    }

    static void PatchLadderPlaceEnd(MethodDefinition method, ModuleDefinition mod, TypeDefinition zombieType)
    {
        var fLadderPlace = zombieType.Fields.First(f => f.Name == "ladderZombie_place");
        var fX = zombieType.Fields.First(f => f.Name == "fX");
        var fY = zombieType.Fields.First(f => f.Name == "fY");
        var fRDirection = zombieType.Fields.First(f => f.Name == "rDirection");
        var fBoard = zombieType.Fields.First(f => f.Name == "board");
        var fCamp = zombieType.Fields.First(f => f.Name == "camp");

        var mTestPlace = zombieType.Methods.First(m => m.Name == "ZC_LadderTestPlace");
        var mDropArmor2 = zombieType.Methods.First(m => m.Name == "Drop_Armor2");

        var rDirectionType = fRDirection.FieldType;
        var boardType = (TypeDefinition)fBoard.FieldType.Resolve();
        var fBoardConfig = boardType.Fields.First(f => f.Name == "boardConfig");
        var fDeviceManager = boardType.Fields.First(f => f.Name == "deviceManager");

        var boardConfigType = (TypeDefinition)fBoardConfig.FieldType.Resolve();
        var mGetGridX = boardConfigType.Methods.First(m => m.Name == "GetGridX");
        var mGetGridY = boardConfigType.Methods.First(m => m.Name == "GetGridY");

        var deviceDataType = mod.Types.First(t => t.Name == "DeviceData");
        var ctorDeviceData = deviceDataType.Methods.First(m => m.IsConstructor && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == mod.TypeSystem.Int32.FullName);
        var fDeviceDataCamp = deviceDataType.Fields.First(f => f.Name == "camp");

        var deviceManagerType = (TypeDefinition)fDeviceManager.FieldType.Resolve();
        var mPlaceDevice = deviceManagerType.Methods.First(m => m.Name == "PlaceDevice");
        var deviceType = mPlaceDevice.ReturnType;

        var resourceManagerType = mod.Types.First(t => t.Name == "ResourceManager");
        var fZombieClips = resourceManagerType.Fields.First(f => f.Name == "zombieClips");

        var globalStaticVarsType = mod.Types.First(t => t.Name == "GlobalStaticVars");
        var mCreateAudio = globalStaticVarsType.Methods.First(m => m.Name == "CreateAudioAtPoint");
        var audioSourceType = mCreateAudio.ReturnType;
        var audioClipType = mCreateAudio.Parameters[0].ParameterType;

        var mGetItem = new MethodReference("get_Item", audioClipType, fZombieClips.FieldType) { HasThis = true };
        mGetItem.Parameters.Add(new ParameterDefinition(mod.TypeSystem.Int32));

        var fVector3X = mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(f => f.DeclaringType.Name == "Vector3" && f.Name == "x")
            ?? new FieldReference("x", mod.TypeSystem.Single, rDirectionType);
        var fVector3Y = mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(f => f.DeclaringType.Name == "Vector3" && f.Name == "y")
            ?? new FieldReference("y", mod.TypeSystem.Single, rDirectionType);

        var body = method.Body;
        body.Instructions.Clear();
        body.Variables.Clear();
        body.ExceptionHandlers.Clear();
        body.InitLocals = true;
        body.MaxStackSize = 4;

        body.Variables.Add(new VariableDefinition(mod.TypeSystem.Single));
        body.Variables.Add(new VariableDefinition(mod.TypeSystem.Single));
        body.Variables.Add(new VariableDefinition(mod.TypeSystem.Int32));
        body.Variables.Add(new VariableDefinition(mod.TypeSystem.Int32));
        body.Variables.Add(new VariableDefinition(deviceDataType));
        body.Variables.Add(new VariableDefinition(deviceType));
        body.Variables.Add(new VariableDefinition(audioClipType));
        body.Variables.Add(new VariableDefinition(audioSourceType));

        var il = body.GetILProcessor();
        var retLabel = il.Create(OpCodes.Ret);

        // ladderZombie_place = false;
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Stfld, fLadderPlace);

        // if (!ZC_LadderTestPlace()) return;
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Call, mTestPlace);
        il.Emit(OpCodes.Brfalse, retLabel);

        // x = fX - rDirection.x * 67f;
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, fX);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldflda, fRDirection);
        il.Emit(OpCodes.Ldfld, fVector3X);
        il.Emit(OpCodes.Ldc_R4, 67f);
        il.Emit(OpCodes.Mul);
        il.Emit(OpCodes.Sub);
        il.Emit(OpCodes.Stloc_0);

        // y = fY - rDirection.y * 67f;
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, fY);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldflda, fRDirection);
        il.Emit(OpCodes.Ldfld, fVector3Y);
        il.Emit(OpCodes.Ldc_R4, 67f);
        il.Emit(OpCodes.Mul);
        il.Emit(OpCodes.Sub);
        il.Emit(OpCodes.Stloc_1);

        // gridX = board.boardConfig.GetGridX(x, y);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, fBoard);
        il.Emit(OpCodes.Ldfld, fBoardConfig);
        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ldloc_1);
        il.Emit(OpCodes.Callvirt, mGetGridX);
        il.Emit(OpCodes.Stloc_2);

        // gridY = board.boardConfig.GetGridY(x, y);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, fBoard);
        il.Emit(OpCodes.Ldfld, fBoardConfig);
        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ldloc_1);
        il.Emit(OpCodes.Callvirt, mGetGridY);
        il.Emit(OpCodes.Stloc_3);

        // deviceData = new DeviceData(11);
        il.Emit(OpCodes.Ldc_I4_S, (sbyte)11);
        il.Emit(OpCodes.Newobj, ctorDeviceData);
        il.Emit(OpCodes.Stloc_S, body.Variables[4]);

        // deviceData.camp = camp;
        il.Emit(OpCodes.Ldloc_S, body.Variables[4]);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, fCamp);
        il.Emit(OpCodes.Stfld, fDeviceDataCamp);

        // device = board.deviceManager.PlaceDevice(deviceData, gridX, gridY);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, fBoard);
        il.Emit(OpCodes.Ldfld, fDeviceManager);
        il.Emit(OpCodes.Ldloc_S, body.Variables[4]);
        il.Emit(OpCodes.Ldloc_2);
        il.Emit(OpCodes.Ldloc_3);
        il.Emit(OpCodes.Callvirt, mPlaceDevice);
        il.Emit(OpCodes.Stloc_S, body.Variables[5]);

        // clip = ResourceManager.zombieClips[5];
        il.Emit(OpCodes.Ldsfld, fZombieClips);
        il.Emit(OpCodes.Ldc_I4_5);
        il.Emit(OpCodes.Callvirt, mGetItem);
        il.Emit(OpCodes.Stloc_S, body.Variables[6]);

        // audioSource = GlobalStaticVars.CreateAudioAtPoint(clip);
        il.Emit(OpCodes.Ldloc_S, body.Variables[6]);
        il.Emit(OpCodes.Call, mCreateAudio);
        il.Emit(OpCodes.Stloc_S, body.Variables[7]);

        // Drop_Armor2(true);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldc_I4_1);
        il.Emit(OpCodes.Call, mDropArmor2);

        // ret
        il.Append(retLabel);
    }
}
