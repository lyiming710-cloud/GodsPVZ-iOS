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
        string outPath = args.Length > 1 ? args[1] : "/workspaces/GodsPVZ-iOS/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native5-textlink.dll";

        Console.WriteLine($"[*] Reading assembly: {inPath}");
        var rp = new ReaderParameters { ReadWrite = false };
        using var asm = AssemblyDefinition.ReadAssembly(inPath, rp);
        var mod = asm.MainModule;

        var textLinkType = mod.Types.FirstOrDefault(t => t.Name == "TextLink")
            ?? throw new Exception("TextLink type not found!");

        Console.WriteLine($"[+] Found TextLink type (Token: 0x{textLinkType.MetadataToken.ToInt32():X8})");

        var fTmpText = textLinkType.Fields.First(f => f.Name == "TMP_Text");
        var fOriginalColor = textLinkType.Fields.First(f => f.Name == "originalColor");
        var fHoverColor = textLinkType.Fields.First(f => f.Name == "hoverColor");

        Console.WriteLine($"[+] Found fields: TMP_Text, originalColor, hoverColor");

        var resetLinkMethod = textLinkType.Methods.FirstOrDefault(m => m.Name == "ResetLink")
            ?? throw new Exception("ResetLink method not found!");
        var setLinkMethod = textLinkType.Methods.FirstOrDefault(m => m.Name == "SetLink")
            ?? throw new Exception("SetLink method not found!");

        Console.WriteLine($"[+] ResetLink original: Token=0x{resetLinkMethod.MetadataToken.ToInt32():X8}, Locals={resetLinkMethod.Body.Variables.Count}, CodeSize={resetLinkMethod.Body.CodeSize}");
        Console.WriteLine($"[+] SetLink original:   Token=0x{setLinkMethod.MetadataToken.ToInt32():X8}, Locals={setLinkMethod.Body.Variables.Count}, CodeSize={setLinkMethod.Body.CodeSize}");

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

        var updateVertexData = mod.GetMemberReferences().OfType<MethodReference>().FirstOrDefault(m => m.DeclaringType.Name == "TMP_Text" && m.Name == "UpdateVertexData")
            ?? throw new Exception("MR TMP_Text::UpdateVertexData not found!");

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

        Console.WriteLine("[+] All types and member references defined cleanly via ECMA-335 / Cecil!");

        PatchMethod(resetLinkMethod, mod, fTmpText, fOriginalColor, getTextInfo, textInfoType, fLinkInfo, linkInfoType, fLinkTextLength, fLinkTextFirstChar, fCharInfo, charInfoType, fIsVisible, fMatRefIdx, fVertexIdx, fMeshInfo, meshInfoType, fColors32, color32Type, opImplicit, updateVertexData);
        PatchMethod(setLinkMethod, mod, fTmpText, fHoverColor, getTextInfo, textInfoType, fLinkInfo, linkInfoType, fLinkTextLength, fLinkTextFirstChar, fCharInfo, charInfoType, fIsVisible, fMatRefIdx, fVertexIdx, fMeshInfo, meshInfoType, fColors32, color32Type, opImplicit, updateVertexData);

        Console.WriteLine($"[+] ResetLink patched: Locals={resetLinkMethod.Body.Variables.Count}, CodeSize={resetLinkMethod.Body.CodeSize}, MaxStack={resetLinkMethod.Body.MaxStackSize}");
        Console.WriteLine($"[+] SetLink patched:   Locals={setLinkMethod.Body.Variables.Count}, CodeSize={setLinkMethod.Body.CodeSize}, MaxStack={setLinkMethod.Body.MaxStackSize}");

        Console.WriteLine($"[*] Writing patched assembly to: {outPath}");
        asm.Write(outPath);
        Console.WriteLine($"[SUCCESS] Successfully written {outPath}!");

        File.Copy("/workspaces/GodsPVZ-iOS/scripts/codespaces/Patcher/Program.cs", "/workspaces/GodsPVZ-iOS/scripts/codespaces/patch_textlink.cs", true);
        Console.WriteLine("[+] Saved copy to /workspaces/GodsPVZ-iOS/scripts/codespaces/patch_textlink.cs");
    }

    static void PatchMethod(
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
}
