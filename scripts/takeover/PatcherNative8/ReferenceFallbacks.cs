using System;
using System.Linq;
using Mono.Cecil;

internal static class ReferenceFallbacks
{
    static TypeReference T(ModuleDefinition m, string name)
    {
        return name switch
        {
            "Void" => m.TypeSystem.Void,
            "Boolean" => m.TypeSystem.Boolean,
            "Int32" => m.TypeSystem.Int32,
            "Single" => m.TypeSystem.Single,
            "Object" => m.TypeSystem.Object,
            _ => m.GetTypeReferences().FirstOrDefault(t => t.Name == name)
                 ?? throw new InvalidOperationException($"No TypeRef available for {name}")
        };
    }

    static MethodReference M(ModuleDefinition m, string dt, string name, string ret, bool hasThis, params string[] ps)
    {
        var r = new MethodReference(name, T(m, ret), T(m, dt)) { HasThis = hasThis };
        foreach (var p in ps) r.Parameters.Add(new ParameterDefinition(T(m, p)));
        return r;
    }

    public static MethodReference Method(ModuleDefinition m, string dt, string name, int argc)
    {
        return (dt, name, argc) switch
        {
            ("AnimationCurve", "set_preWrapMode", 1) => M(m,dt,name,"Void",true,"WrapMode"),
            ("AnimationCurve", "set_postWrapMode", 1) => M(m,dt,name,"Void",true,"WrapMode"),
            ("AnimationCurve", "get_keys", 0) => new MethodReference(name, new ArrayType(T(m,"Keyframe")), T(m,dt)) { HasThis = true },
            ("AnimationCurve", "Evaluate", 1) => M(m,dt,name,"Single",true,"Single"),
            ("Keyframe", "get_value", 0) => M(m,dt,name,"Single",true),

            ("TMP_Text", "get_havePropertiesChanged", 0) => M(m,dt,name,"Boolean",true),
            ("TMP_Text", "set_havePropertiesChanged", 1) => M(m,dt,name,"Void",true,"Boolean"),
            ("TMP_Text", "ForceMeshUpdate", 2) => M(m,dt,name,"Void",true,"Boolean","Boolean"),
            ("TMP_Text", "get_textInfo", 0) => M(m,dt,name,"TMP_TextInfo",true),
            ("TMP_Text", "get_bounds", 0) => M(m,dt,name,"Bounds",true),
            ("TMP_Text", "UpdateVertexData", 0) => M(m,dt,name,"Void",true),

            ("Bounds", "get_min", 0) => M(m,dt,name,"Vector3",true),
            ("Bounds", "get_max", 0) => M(m,dt,name,"Vector3",true),

            ("Vector3", ".ctor", 3) => M(m,dt,name,"Void",true,"Single","Single","Single"),
            ("Vector3", "op_UnaryNegation", 1) => M(m,dt,name,"Vector3",false,"Vector3"),
            ("Vector3", "op_Addition", 2) => M(m,dt,name,"Vector3",false,"Vector3","Vector3"),
            ("Vector3", "op_Subtraction", 2) => M(m,dt,name,"Vector3",false,"Vector3","Vector3"),
            ("Vector3", "get_normalized", 0) => M(m,dt,name,"Vector3",true),
            ("Vector3", "Dot", 2) => M(m,dt,name,"Single",false,"Vector3","Vector3"),
            ("Vector3", "Cross", 2) => M(m,dt,name,"Vector3",false,"Vector3","Vector3"),
            ("Vector3", "get_one", 0) => M(m,dt,name,"Vector3",false),
            ("Mathf", "Acos", 1) => M(m,dt,name,"Single",false,"Single"),
            ("Quaternion", "Euler", 3) => M(m,dt,name,"Quaternion",false,"Single","Single","Single"),
            ("Matrix4x4", "TRS", 3) => M(m,dt,name,"Matrix4x4",false,"Vector3","Quaternion","Vector3"),
            ("Matrix4x4", "MultiplyPoint3x4", 1) => M(m,dt,name,"Vector3",true,"Vector3"),
            _ => throw new InvalidOperationException($"No method fallback for {dt}::{name}/{argc}")
        };
    }

    public static FieldReference Field(ModuleDefinition m, string dt, string name)
    {
        TypeReference ft = (dt, name) switch
        {
            ("TMP_TextInfo", "characterCount") => T(m,"Int32"),
            ("TMP_TextInfo", "characterInfo") => new ArrayType(T(m,"TMP_CharacterInfo")),
            ("TMP_TextInfo", "meshInfo") => new ArrayType(T(m,"TMP_MeshInfo")),
            ("TMP_CharacterInfo", "isVisible") => T(m,"Boolean"),
            ("TMP_CharacterInfo", "vertexIndex") => T(m,"Int32"),
            ("TMP_CharacterInfo", "materialReferenceIndex") => T(m,"Int32"),
            ("TMP_CharacterInfo", "baseLine") => T(m,"Single"),
            ("TMP_CharacterInfo", "topRight") => T(m,"Vector3"),
            ("TMP_CharacterInfo", "bottomRight") => T(m,"Vector3"),
            ("TMP_MeshInfo", "vertices") => new ArrayType(T(m,"Vector3")),
            ("Vector3", "x") => T(m,"Single"),
            ("Vector3", "y") => T(m,"Single"),
            ("Vector3", "z") => T(m,"Single"),
            _ => throw new InvalidOperationException($"No field fallback for {dt}::{name}")
        };
        return new FieldReference(name, ft, T(m,dt));
    }
}
