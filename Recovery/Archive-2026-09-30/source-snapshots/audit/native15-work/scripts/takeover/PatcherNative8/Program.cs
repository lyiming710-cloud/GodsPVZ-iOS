using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x06000776u;
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static void Main(string[] args)
    {
        if (args.Length < 2 || args.Length > 3)
            throw new ArgumentException("usage: PatcherNative8 <native7-input.dll> <native8-output.dll> [linked]");
        var linked = args.Length == 3 && args[2] == "linked";
        var input = Path.GetFullPath(args[0]);
        var output = Path.GetFullPath(args[1]);
        if (!File.Exists(input)) throw new FileNotFoundException(input);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);

        using var asm = AssemblyDefinition.ReadAssembly(input);
        var mod = asm.MainModule;
        var mvid = mod.Mvid;
        if (!linked) CheckIdentity(mod, "input");
        var target = FindTarget(mod, linked);
        var before = SnapshotNonTargets(mod, target);
        PatchMoveNext(mod, target);
        if (!linked) CheckIdentity(mod, "memory");
        CheckNonTargets(mod, target, before, "memory");
        asm.Write(output);

        using var reopened = AssemblyDefinition.ReadAssembly(output);
        if (reopened.MainModule.Mvid != mvid)
            throw new InvalidOperationException($"MVID changed: {mvid} -> {reopened.MainModule.Mvid}");
        if (!linked) CheckIdentity(reopened.MainModule, "reopened");
        var rt = FindTarget(reopened.MainModule, linked);
        CheckNonTargets(reopened.MainModule, rt, before, "reopened");
        Console.WriteLine($"NATIVE8_PATCH_PASS linked={linked} input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
        Console.WriteLine($"TARGET token=0x{rt.MetadataToken.ToUInt32():X8} {rt.FullName} code_size={rt.Body.CodeSize} il={rt.Body.Instructions.Count} locals={rt.Body.Variables.Count} eh={rt.Body.ExceptionHandlers.Count}");
    }

    static IEnumerable<TypeDefinition> AllTypes(TypeDefinition t)
    {
        yield return t;
        foreach (var n in t.NestedTypes)
            foreach (var x in AllTypes(n)) yield return x;
    }
    static IEnumerable<TypeDefinition> AllTypes(ModuleDefinition m)
    {
        foreach (var t in m.Types)
            foreach (var x in AllTypes(t)) yield return x;
    }

    static TypeDefinition FindType(ModuleDefinition m, string ns, string name) =>
        AllTypes(m).Single(t => t.Namespace == ns && t.Name == name);

    static MethodDefinition FindTarget(ModuleDefinition m, bool linked)
    {
        var parent = FindType(m, "TMPro.Examples", "SkewTextExample");
        var it = parent.NestedTypes.Single(t => t.Name == "<WarpText>d__7");
        var md = it.Methods.Single(x => x.Name == "MoveNext" && x.Parameters.Count == 0);
        if (!linked && md.MetadataToken.ToUInt32() != TargetToken)
            throw new InvalidOperationException($"MoveNext token mismatch: 0x{md.MetadataToken.ToUInt32():X8}");
        if (!md.HasBody) throw new InvalidOperationException("MoveNext body missing");
        return md;
    }

    static void CheckIdentity(ModuleDefinition m, string stage)
    {
        var ts = AllTypes(m).ToArray();
        var mc = ts.Sum(t => t.Methods.Count);
        var fc = ts.Sum(t => t.Fields.Count);
        if (ts.Length != ExpectedTypes || mc != ExpectedMethods || fc != ExpectedFields)
            throw new InvalidOperationException($"{stage}: metadata count mismatch {ts.Length}/{mc}/{fc}");
    }

    static Dictionary<uint,string> SnapshotNonTargets(ModuleDefinition m, MethodDefinition target) =>
        AllTypes(m).SelectMany(t => t.Methods).Where(x => x.HasBody && x != target)
            .ToDictionary(x => x.MetadataToken.ToUInt32(), Fingerprint);

    static void CheckNonTargets(ModuleDefinition m, MethodDefinition target, Dictionary<uint,string> before, string stage)
    {
        var now = SnapshotNonTargets(m, target);
        if (now.Count != before.Count) throw new InvalidOperationException($"{stage}: non-target count changed {before.Count}->{now.Count}");
        foreach (var kv in before)
            if (!now.TryGetValue(kv.Key, out var s) || s != kv.Value)
                throw new InvalidOperationException($"{stage}: non-target changed 0x{kv.Key:X8}");
        Console.WriteLine($"NON_TARGET_ISOLATION_PASS stage={stage} methods={before.Count}");
    }

    static string StableOp(Instruction i)
    {
        var n = i.OpCode.Code.ToString();
        if ((i.Operand is Instruction || i.Operand is Instruction[]) && n.EndsWith("_S", StringComparison.Ordinal))
            return n[..^2];
        return n;
    }
    static int Ix(MethodDefinition m, Instruction i) => i == null ? -1 : m.Body.Instructions.IndexOf(i);
    static string Fingerprint(MethodDefinition m)
    {
        var b = new StringBuilder().Append(m.Body.InitLocals).Append('|').Append(m.Body.MaxStackSize).Append('|');
        foreach (var v in m.Body.Variables) b.Append("V:").Append(v.VariableType.FullName).Append(';');
        foreach (var i in m.Body.Instructions)
        {
            b.Append(StableOp(i)).Append(':');
            switch (i.Operand)
            {
                case null: break;
                case Instruction x: b.Append('@').Append(Ix(m,x)); break;
                case Instruction[] xs: foreach (var x in xs) b.Append('@').Append(Ix(m,x)).Append(','); break;
                case VariableDefinition v: b.Append('V').Append(v.Index).Append(':').Append(v.VariableType.FullName); break;
                case ParameterDefinition p: b.Append('P').Append(p.Index).Append(':').Append(p.ParameterType.FullName); break;
                case MemberReference mr: b.Append('M').Append(mr.FullName).Append('@').Append(mr.DeclaringType?.Scope?.Name); break;
                default: b.Append(i.Operand); break;
            }
            b.Append(';');
        }
        foreach (var e in m.Body.ExceptionHandlers)
            b.Append("EH:").Append(e.HandlerType).Append(':').Append(Ix(m,e.TryStart)).Append(':').Append(Ix(m,e.TryEnd))
             .Append(':').Append(Ix(m,e.HandlerStart)).Append(':').Append(Ix(m,e.HandlerEnd)).Append(':').Append(Ix(m,e.FilterStart))
             .Append(':').Append(e.CatchType?.FullName).Append(';');
        return b.ToString();
    }

    static FieldDefinition DF(TypeDefinition t, string name) => t.Fields.Single(f => f.Name == name);
    static FieldReference RF(ModuleDefinition m, string declaringName, string name) =>
        m.GetMemberReferences().OfType<FieldReference>().First(f => f.DeclaringType.Name == declaringName && f.Name == name);
    static MethodReference RM(ModuleDefinition m, string declaringName, string name, int argc) =>
        m.GetMemberReferences().OfType<MethodReference>().First(x => x.DeclaringType.Name == declaringName && x.Name == name && x.Parameters.Count == argc);
    static MethodReference RM(ModuleDefinition m, string declaringName, string name, int argc, string returnName) =>
        m.GetMemberReferences().OfType<MethodReference>().First(x => x.DeclaringType.Name == declaringName && x.Name == name && x.Parameters.Count == argc && x.ReturnType.Name == returnName);

    static void Reset(MethodDefinition m, int maxStack)
    {
        m.Body.Instructions.Clear();
        m.Body.Variables.Clear();
        m.Body.ExceptionHandlers.Clear();
        m.Body.InitLocals = true;
        m.Body.MaxStackSize = maxStack;
    }

    static void LdcI4(ILProcessor il, int v)
    {
        switch (v)
        {
            case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
            case 0: il.Emit(OpCodes.Ldc_I4_0); break;
            case 1: il.Emit(OpCodes.Ldc_I4_1); break;
            case 2: il.Emit(OpCodes.Ldc_I4_2); break;
            case 3: il.Emit(OpCodes.Ldc_I4_3); break;
            default: il.Emit(OpCodes.Ldc_I4, v); break;
        }
    }
    static void LoadIndex(ILProcessor il, VariableDefinition idx, int add)
    {
        il.Emit(OpCodes.Ldloc, idx);
        if (add != 0) { LdcI4(il, add); il.Emit(OpCodes.Add); }
    }
    static void EmitVecCtor(ILProcessor il, MethodReference ctor, Action x, Action y, Action z)
    {
        x(); y(); z(); il.Emit(OpCodes.Newobj, ctor);
    }
    static void EmitVertexBinary(ILProcessor il, VariableDefinition vertices, VariableDefinition vertexIndex, int add,
        VariableDefinition rhs, MethodReference op, TypeReference vecType, bool negate, MethodReference unaryNeg)
    {
        il.Emit(OpCodes.Ldloc, vertices); LoadIndex(il, vertexIndex, add); il.Emit(OpCodes.Ldelema, vecType);
        il.Emit(OpCodes.Dup); il.Emit(OpCodes.Ldobj, vecType); il.Emit(OpCodes.Ldloc, rhs);
        if (negate) il.Emit(OpCodes.Call, unaryNeg);
        il.Emit(OpCodes.Call, op); il.Emit(OpCodes.Stobj, vecType);
    }
    static void EmitMatrixVertex(ILProcessor il, VariableDefinition vertices, VariableDefinition vertexIndex, int add,
        VariableDefinition matrix, MethodReference multiply, TypeReference vecType)
    {
        il.Emit(OpCodes.Ldloc, vertices); LoadIndex(il, vertexIndex, add);
        il.Emit(OpCodes.Ldloca, matrix);
        il.Emit(OpCodes.Ldloc, vertices); LoadIndex(il, vertexIndex, add); il.Emit(OpCodes.Ldelem_Any, vecType);
        il.Emit(OpCodes.Call, multiply); il.Emit(OpCodes.Stelem_Any, vecType);
    }

    static void PatchMoveNext(ModuleDefinition mod, MethodDefinition method)
    {
        var iterator = method.DeclaringType;
        var parent = iterator.DeclaringType;
        if (parent == null || parent.Name != "SkewTextExample") throw new InvalidOperationException("unexpected iterator parent");

        // Generated iterator and captured-parent fields are preserved by metadata; only MoveNext is rebuilt.
        var fState = DF(iterator, "<>1__state");
        var fCurrent = DF(iterator, "<>2__current");
        var fThis = DF(iterator, "<>4__this");
        var fOldScale = DF(iterator, "<old_CurveScale>5__2");
        var fOldShear = DF(iterator, "<old_ShearValue>5__3");
        var fOldCurve = DF(iterator, "<old_curve>5__4");
        var fText = DF(parent, "m_TextComponent");
        var fCurve = DF(parent, "VertexCurve");
        var fScale = DF(parent, "CurveScale");
        var fShear = DF(parent, "ShearAmount");
        var copyCurve = parent.Methods.Single(x => x.Name == "CopyAnimationCurve" && x.Parameters.Count == 1);

        // Existing target module references are reused, avoiding synthetic signature guesses.
        var setPreWrap = RM(mod, "AnimationCurve", "set_preWrapMode", 1);
        var setPostWrap = RM(mod, "AnimationCurve", "set_postWrapMode", 1);
        var getKeys = RM(mod, "AnimationCurve", "get_keys", 0);
        var evaluate = RM(mod, "AnimationCurve", "Evaluate", 1);
        var getKeyValue = RM(mod, "Keyframe", "get_value", 0);
        var getChanged = RM(mod, "TMP_Text", "get_havePropertiesChanged", 0);
        var setChanged = RM(mod, "TMP_Text", "set_havePropertiesChanged", 1);
        var forceMesh = RM(mod, "TMP_Text", "ForceMeshUpdate", 0);
        var getTextInfo = RM(mod, "TMP_Text", "get_textInfo", 0);
        var getBounds = RM(mod, "TMP_Text", "get_bounds", 0);
        var updateVertexData = RM(mod, "TMP_Text", "UpdateVertexData", 0);
        var getBoundsMin = RM(mod, "Bounds", "get_min", 0);
        var getBoundsMax = RM(mod, "Bounds", "get_max", 0);
        var vecCtor = RM(mod, "Vector3", ".ctor", 3);
        var vecNeg = RM(mod, "Vector3", "op_UnaryNegation", 1);
        var vecAdd = RM(mod, "Vector3", "op_Addition", 2);
        var vecSub = RM(mod, "Vector3", "op_Subtraction", 2);
        var vecNorm = RM(mod, "Vector3", "get_normalized", 0);
        var vecDot = RM(mod, "Vector3", "Dot", 2);
        var vecCross = RM(mod, "Vector3", "Cross", 2);
        var vecOne = RM(mod, "Vector3", "get_one", 0);
        var acos = RM(mod, "Mathf", "Acos", 1);
        var quatEuler = RM(mod, "Quaternion", "Euler", 3);
        var trs = RM(mod, "Matrix4x4", "TRS", 3);
        var multiplyPoint = RM(mod, "Matrix4x4", "MultiplyPoint3x4", 1);

        var fCharacterCount = RF(mod, "TMP_TextInfo", "characterCount");
        var fCharacterInfo = RF(mod, "TMP_TextInfo", "characterInfo");
        var fMeshInfo = RF(mod, "TMP_TextInfo", "meshInfo");
        var fVisible = RF(mod, "TMP_CharacterInfo", "isVisible");
        var fVertexIndex = RF(mod, "TMP_CharacterInfo", "vertexIndex");
        var fMaterialIndex = RF(mod, "TMP_CharacterInfo", "materialReferenceIndex");
        var fBaseline = RF(mod, "TMP_CharacterInfo", "baseLine");
        var fTopRight = RF(mod, "TMP_CharacterInfo", "topRight");
        var fBottomRight = RF(mod, "TMP_CharacterInfo", "bottomRight");
        var fVertices = RF(mod, "TMP_MeshInfo", "vertices");
        var fVecX = RF(mod, "Vector3", "x");
        var fVecY = RF(mod, "Vector3", "y");
        var fVecZ = RF(mod, "Vector3", "z");

        var textInfoType = getTextInfo.ReturnType;
        var charArrayType = (ArrayType)fCharacterInfo.FieldType;
        var charInfoType = charArrayType.ElementType;
        var meshArrayType = (ArrayType)fMeshInfo.FieldType;
        var meshInfoType = meshArrayType.ElementType;
        var verticesType = (ArrayType)fVertices.FieldType;
        var vecType = verticesType.ElementType;
        var boundsType = getBounds.ReturnType;
        var matrixType = trs.ReturnType;
        var parentType = parent;

        Reset(method, 12);
        var state = new VariableDefinition(mod.TypeSystem.Int32);
        var self = new VariableDefinition(parentType);
        var textInfo = new VariableDefinition(textInfoType);
        var charCount = new VariableDefinition(mod.TypeSystem.Int32);
        var bounds = new VariableDefinition(boundsType);
        var boundsMinX = new VariableDefinition(mod.TypeSystem.Single);
        var boundsMaxX = new VariableDefinition(mod.TypeSystem.Single);
        var i = new VariableDefinition(mod.TypeSystem.Int32);
        var charInfo = new VariableDefinition(charInfoType);
        var vertexIndex = new VariableDefinition(mod.TypeSystem.Int32);
        var materialIndex = new VariableDefinition(mod.TypeSystem.Int32);
        var vertices = new VariableDefinition(verticesType);
        var offset = new VariableDefinition(vecType);
        var shear = new VariableDefinition(mod.TypeSystem.Single);
        var topShear = new VariableDefinition(vecType);
        var bottomShear = new VariableDefinition(vecType);
        var x0 = new VariableDefinition(mod.TypeSystem.Single);
        var x1 = new VariableDefinition(mod.TypeSystem.Single);
        var y0 = new VariableDefinition(mod.TypeSystem.Single);
        var y1 = new VariableDefinition(mod.TypeSystem.Single);
        var horizontal = new VariableDefinition(vecType);
        var tangent = new VariableDefinition(vecType);
        var normalized = new VariableDefinition(vecType);
        var dot = new VariableDefinition(mod.TypeSystem.Single);
        var cross = new VariableDefinition(vecType);
        var angle = new VariableDefinition(mod.TypeSystem.Single);
        var quat = new VariableDefinition(quatEuler.ReturnType);
        var matrix = new VariableDefinition(matrixType);
        foreach (var v in new[] { state,self,textInfo,charCount,bounds,boundsMinX,boundsMaxX,i,charInfo,vertexIndex,materialIndex,vertices,offset,shear,topShear,bottomShear,x0,x1,y0,y1,horizontal,tangent,normalized,dot,cross,angle,quat,matrix })
            method.Body.Variables.Add(v);

        var il = method.Body.GetILProcessor();
        var initial = il.Create(OpCodes.Nop);
        var resume1 = il.Create(OpCodes.Nop);
        var resume2 = il.Create(OpCodes.Nop);
        var loopTop = il.Create(OpCodes.Nop);
        var process = il.Create(OpCodes.Nop);
        var forCond = il.Create(OpCodes.Ldloc, i);
        var forBody = il.Create(OpCodes.Ldloc, textInfo);
        var forStep = il.Create(OpCodes.Ldloc, i);
        var angleElse = il.Create(OpCodes.Ldc_R4, 360f);
        var angleDone = il.Create(OpCodes.Nop);
        var afterFor = il.Create(OpCodes.Ldloc, self);
        var invalidState = il.Create(OpCodes.Ldc_I4_0);

        // Cache captured parent and dispatch exact iterator states 0/1/2.
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fThis); il.Emit(OpCodes.Stloc, self);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fState); il.Emit(OpCodes.Stloc, state);
        il.Emit(OpCodes.Ldloc, state); LdcI4(il,0); il.Emit(OpCodes.Beq, initial);
        il.Emit(OpCodes.Ldloc, state); LdcI4(il,1); il.Emit(OpCodes.Beq, resume1);
        il.Emit(OpCodes.Ldloc, state); LdcI4(il,2); il.Emit(OpCodes.Beq, resume2);
        il.Emit(OpCodes.Br, invalidState);

        il.Append(initial);
        il.Emit(OpCodes.Ldarg_0); LdcI4(il,-1); il.Emit(OpCodes.Stfld, fState);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fCurve); LdcI4(il,1); il.Emit(OpCodes.Callvirt, setPreWrap);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fCurve); LdcI4(il,1); il.Emit(OpCodes.Callvirt, setPostWrap);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); LdcI4(il,1); il.Emit(OpCodes.Callvirt, setChanged);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Dup); il.Emit(OpCodes.Ldfld, fScale); il.Emit(OpCodes.Ldc_R4,10f); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Stfld, fScale);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fScale); il.Emit(OpCodes.Stfld, fOldScale);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fShear); il.Emit(OpCodes.Stfld, fOldShear);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fCurve); il.Emit(OpCodes.Call, copyCurve); il.Emit(OpCodes.Stfld, fOldCurve);
        il.Emit(OpCodes.Br, loopTop);

        il.Append(resume1);
        il.Emit(OpCodes.Ldarg_0); LdcI4(il,-1); il.Emit(OpCodes.Stfld, fState); il.Emit(OpCodes.Br, loopTop);
        il.Append(resume2);
        il.Emit(OpCodes.Ldarg_0); LdcI4(il,-1); il.Emit(OpCodes.Stfld, fState); il.Emit(OpCodes.Br, loopTop);

        il.Append(loopTop);
        // if properties / scale / curve key / shear changed -> process; otherwise yield null.
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); il.Emit(OpCodes.Callvirt, getChanged); il.Emit(OpCodes.Brtrue, process);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fOldScale); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fScale); il.Emit(OpCodes.Bne_Un, process);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fOldCurve); il.Emit(OpCodes.Callvirt, getKeys); LdcI4(il,1); il.Emit(OpCodes.Ldelema, getKeys.ReturnType is ArrayType ka ? ka.ElementType : throw new InvalidOperationException("AnimationCurve.keys not array")); il.Emit(OpCodes.Call, getKeyValue);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fCurve); il.Emit(OpCodes.Callvirt, getKeys); LdcI4(il,1); il.Emit(OpCodes.Ldelema, ((ArrayType)getKeys.ReturnType).ElementType); il.Emit(OpCodes.Call, getKeyValue); il.Emit(OpCodes.Bne_Un, process);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, fOldShear); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fShear); il.Emit(OpCodes.Bne_Un, process);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Stfld, fCurrent);
        il.Emit(OpCodes.Ldarg_0); LdcI4(il,1); il.Emit(OpCodes.Stfld, fState); LdcI4(il,1); il.Emit(OpCodes.Ret);

        il.Append(process);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fScale); il.Emit(OpCodes.Stfld, fOldScale);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fCurve); il.Emit(OpCodes.Call, copyCurve); il.Emit(OpCodes.Stfld, fOldCurve);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fShear); il.Emit(OpCodes.Stfld, fOldShear);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); il.Emit(OpCodes.Callvirt, forceMesh);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); il.Emit(OpCodes.Callvirt, getTextInfo); il.Emit(OpCodes.Stloc, textInfo);
        il.Emit(OpCodes.Ldloc, textInfo); il.Emit(OpCodes.Ldfld, fCharacterCount); il.Emit(OpCodes.Stloc, charCount);
        il.Emit(OpCodes.Ldloc, charCount); il.Emit(OpCodes.Brfalse, loopTop);

        // boundsMinX = text.bounds.min.x; boundsMaxX = text.bounds.max.x (two property reads as source).
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); il.Emit(OpCodes.Callvirt, getBounds); il.Emit(OpCodes.Stloc, bounds);
        il.Emit(OpCodes.Ldloca, bounds); il.Emit(OpCodes.Call, getBoundsMin); il.Emit(OpCodes.Stloc, offset);
        il.Emit(OpCodes.Ldloca, offset); il.Emit(OpCodes.Ldfld, fVecX); il.Emit(OpCodes.Stloc, boundsMinX);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); il.Emit(OpCodes.Callvirt, getBounds); il.Emit(OpCodes.Stloc, bounds);
        il.Emit(OpCodes.Ldloca, bounds); il.Emit(OpCodes.Call, getBoundsMax); il.Emit(OpCodes.Stloc, offset);
        il.Emit(OpCodes.Ldloca, offset); il.Emit(OpCodes.Ldfld, fVecX); il.Emit(OpCodes.Stloc, boundsMaxX);

        LdcI4(il,0); il.Emit(OpCodes.Stloc, i); il.Emit(OpCodes.Br, forCond);

        il.Append(forBody);
        il.Emit(OpCodes.Ldfld, fCharacterInfo); il.Emit(OpCodes.Ldloc, i); il.Emit(OpCodes.Ldelem_Any, charInfoType); il.Emit(OpCodes.Stloc, charInfo);
        il.Emit(OpCodes.Ldloca, charInfo); il.Emit(OpCodes.Ldfld, fVisible); il.Emit(OpCodes.Brfalse, forStep);
        il.Emit(OpCodes.Ldloca, charInfo); il.Emit(OpCodes.Ldfld, fVertexIndex); il.Emit(OpCodes.Stloc, vertexIndex);
        il.Emit(OpCodes.Ldloca, charInfo); il.Emit(OpCodes.Ldfld, fMaterialIndex); il.Emit(OpCodes.Stloc, materialIndex);
        il.Emit(OpCodes.Ldloc, textInfo); il.Emit(OpCodes.Ldfld, fMeshInfo); il.Emit(OpCodes.Ldloc, materialIndex); il.Emit(OpCodes.Ldelema, meshInfoType); il.Emit(OpCodes.Ldfld, fVertices); il.Emit(OpCodes.Stloc, vertices);

        // offsetToMidBaseline = new Vector3((v0.x + v2.x)/2, baseline, 0).
        EmitVecCtor(il, vecCtor,
            () => { il.Emit(OpCodes.Ldloc, vertices); LoadIndex(il, vertexIndex,0); il.Emit(OpCodes.Ldelema, vecType); il.Emit(OpCodes.Ldfld, fVecX); il.Emit(OpCodes.Ldloc, vertices); LoadIndex(il, vertexIndex,2); il.Emit(OpCodes.Ldelema, vecType); il.Emit(OpCodes.Ldfld, fVecX); il.Emit(OpCodes.Add); il.Emit(OpCodes.Ldc_R4,2f); il.Emit(OpCodes.Div); },
            () => { il.Emit(OpCodes.Ldloca, charInfo); il.Emit(OpCodes.Ldfld, fBaseline); },
            () => il.Emit(OpCodes.Ldc_R4,0f));
        il.Emit(OpCodes.Stloc, offset);
        for (int k=0;k<4;k++) EmitVertexBinary(il,vertices,vertexIndex,k,offset,vecAdd,vecType,true,vecNeg);

        // shear and top/bottom shear vectors.
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fShear); il.Emit(OpCodes.Ldc_R4,0.01f); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Stloc, shear);
        EmitVecCtor(il,vecCtor,
            () => { il.Emit(OpCodes.Ldloc, shear); il.Emit(OpCodes.Ldloca, charInfo); il.Emit(OpCodes.Ldflda, fTopRight); il.Emit(OpCodes.Ldfld, fVecY); il.Emit(OpCodes.Ldloca, charInfo); il.Emit(OpCodes.Ldfld, fBaseline); il.Emit(OpCodes.Sub); il.Emit(OpCodes.Mul); },
            () => il.Emit(OpCodes.Ldc_R4,0f), () => il.Emit(OpCodes.Ldc_R4,0f));
        il.Emit(OpCodes.Stloc, topShear);
        EmitVecCtor(il,vecCtor,
            () => { il.Emit(OpCodes.Ldloc, shear); il.Emit(OpCodes.Ldloca, charInfo); il.Emit(OpCodes.Ldfld, fBaseline); il.Emit(OpCodes.Ldloca, charInfo); il.Emit(OpCodes.Ldflda, fBottomRight); il.Emit(OpCodes.Ldfld, fVecY); il.Emit(OpCodes.Sub); il.Emit(OpCodes.Mul); },
            () => il.Emit(OpCodes.Ldc_R4,0f), () => il.Emit(OpCodes.Ldc_R4,0f));
        il.Emit(OpCodes.Stloc, bottomShear);
        EmitVertexBinary(il,vertices,vertexIndex,0,bottomShear,vecAdd,vecType,true,vecNeg);
        EmitVertexBinary(il,vertices,vertexIndex,1,topShear,vecAdd,vecType,false,vecNeg);
        EmitVertexBinary(il,vertices,vertexIndex,2,topShear,vecAdd,vecType,false,vecNeg);
        EmitVertexBinary(il,vertices,vertexIndex,3,bottomShear,vecAdd,vecType,true,vecNeg);

        // Curve-space x/y values.
        il.Emit(OpCodes.Ldloca, offset); il.Emit(OpCodes.Ldfld, fVecX); il.Emit(OpCodes.Ldloc, boundsMinX); il.Emit(OpCodes.Sub);
        il.Emit(OpCodes.Ldloc, boundsMaxX); il.Emit(OpCodes.Ldloc, boundsMinX); il.Emit(OpCodes.Sub); il.Emit(OpCodes.Div); il.Emit(OpCodes.Stloc, x0);
        il.Emit(OpCodes.Ldloc, x0); il.Emit(OpCodes.Ldc_R4,0.0001f); il.Emit(OpCodes.Add); il.Emit(OpCodes.Stloc, x1);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fCurve); il.Emit(OpCodes.Ldloc, x0); il.Emit(OpCodes.Callvirt, evaluate); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fScale); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Stloc, y0);
        il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fCurve); il.Emit(OpCodes.Ldloc, x1); il.Emit(OpCodes.Callvirt, evaluate); il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fScale); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Stloc, y1);

        EmitVecCtor(il,vecCtor,() => il.Emit(OpCodes.Ldc_R4,1f),() => il.Emit(OpCodes.Ldc_R4,0f),() => il.Emit(OpCodes.Ldc_R4,0f)); il.Emit(OpCodes.Stloc, horizontal);
        EmitVecCtor(il,vecCtor,
            () => { il.Emit(OpCodes.Ldloc, x1); il.Emit(OpCodes.Ldloc, boundsMaxX); il.Emit(OpCodes.Ldloc, boundsMinX); il.Emit(OpCodes.Sub); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Ldloc, boundsMinX); il.Emit(OpCodes.Add); },
            () => il.Emit(OpCodes.Ldloc, y1), () => il.Emit(OpCodes.Ldc_R4,0f));
        EmitVecCtor(il,vecCtor,() => { il.Emit(OpCodes.Ldloca, offset); il.Emit(OpCodes.Ldfld, fVecX); },() => il.Emit(OpCodes.Ldloc, y0),() => il.Emit(OpCodes.Ldc_R4,0f));
        il.Emit(OpCodes.Call, vecSub); il.Emit(OpCodes.Stloc, tangent);
        il.Emit(OpCodes.Ldloca, tangent); il.Emit(OpCodes.Call, vecNorm); il.Emit(OpCodes.Stloc, normalized);
        il.Emit(OpCodes.Ldloc, horizontal); il.Emit(OpCodes.Ldloc, normalized); il.Emit(OpCodes.Call, vecDot); il.Emit(OpCodes.Call, acos); il.Emit(OpCodes.Ldc_R4,57.2957795f); il.Emit(OpCodes.Mul); il.Emit(OpCodes.Stloc, dot);
        il.Emit(OpCodes.Ldloc, horizontal); il.Emit(OpCodes.Ldloc, tangent); il.Emit(OpCodes.Call, vecCross); il.Emit(OpCodes.Stloc, cross);
        il.Emit(OpCodes.Ldloca, cross); il.Emit(OpCodes.Ldfld, fVecZ); il.Emit(OpCodes.Ldc_R4,0f); il.Emit(OpCodes.Ble_Un, angleElse);
        il.Emit(OpCodes.Ldloc, dot); il.Emit(OpCodes.Stloc, angle); il.Emit(OpCodes.Br, angleDone);
        il.Append(angleElse); il.Emit(OpCodes.Ldloc, dot); il.Emit(OpCodes.Sub); il.Emit(OpCodes.Stloc, angle);
        il.Append(angleDone);

        // matrix = Matrix4x4.TRS(new Vector3(0,y0,0), Quaternion.Euler(0,0,angle), Vector3.one)
        EmitVecCtor(il,vecCtor,() => il.Emit(OpCodes.Ldc_R4,0f),() => il.Emit(OpCodes.Ldloc,y0),() => il.Emit(OpCodes.Ldc_R4,0f));
        il.Emit(OpCodes.Ldc_R4,0f); il.Emit(OpCodes.Ldc_R4,0f); il.Emit(OpCodes.Ldloc, angle); il.Emit(OpCodes.Call, quatEuler); il.Emit(OpCodes.Stloc, quat);
        il.Emit(OpCodes.Ldloc, quat); il.Emit(OpCodes.Call, vecOne); il.Emit(OpCodes.Call, trs); il.Emit(OpCodes.Stloc, matrix);
        for(int k=0;k<4;k++) EmitMatrixVertex(il,vertices,vertexIndex,k,matrix,multiplyPoint,vecType);
        for(int k=0;k<4;k++) EmitVertexBinary(il,vertices,vertexIndex,k,offset,vecAdd,vecType,false,vecNeg);

        il.Append(forStep); LdcI4(il,1); il.Emit(OpCodes.Add); il.Emit(OpCodes.Stloc, i);
        il.Append(forCond); il.Emit(OpCodes.Ldloc, charCount); il.Emit(OpCodes.Blt, forBody);
        il.Append(afterFor); il.Emit(OpCodes.Ldfld, fText); il.Emit(OpCodes.Callvirt, updateVertexData);
        il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Stfld, fCurrent);
        il.Emit(OpCodes.Ldarg_0); LdcI4(il,2); il.Emit(OpCodes.Stfld, fState); LdcI4(il,1); il.Emit(OpCodes.Ret);

        il.Append(invalidState); il.Emit(OpCodes.Ret);
    }
}
