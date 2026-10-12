using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text.Json;
using Mono.Cecil;
using RO = System.Reflection.Emit.OpCodes;

public class Obj {}
public class GameObject : Obj {}
public class Component : Obj {
    public GameObject gameObject = new();
}
public class Window_T : Component {
    public int T;
}
public class Board : Obj {
    public bool continued;
    public void GameContinue() { continued = true; }
}
public class BoardManager : Obj {
    public static BoardManager? instance;
    public Board? board;
    public static BoardManager? get_Instance() => instance;
    public Board? Board_OnPlay() => board;
}
public class Mesh : Obj {}
public class Material : Obj {}
public class MeshFilter : Obj {
    public Mesh? sharedMesh;
    public void set_sharedMesh(Mesh? m) { sharedMesh = m; }
}
public class Renderer : Obj {
    public Material[]? sharedMaterials;
    public void set_sharedMaterials(Material[]? m) { sharedMaterials = m; }
}
public class Frame : Obj {
    public Mesh? mesh;
    public Material[]? Materials;
    public Mesh? get_CachedMesh() => mesh;
}
public class SwfClip : Obj {
    public MeshFilter? _meshFilter;
    public Renderer? _meshRenderer;
    public bool _dirtyMesh;
    public Frame? frame;
    public Frame? GetCurrentBakedFrame() => frame;
}
public class DeviceEvent : Obj {
    public Device? device;
    public float countdown;
}
public class Device : Obj {
    public List<DeviceEvent> deviceEvents_toAdd = new();
}
public class Plant : Obj {
    public List<int> parameter_ints = new();
}

public static class Test61 {
    public static int destroyedObjects = 0;
    public static void Destroy(Obj o) { destroyedObjects++; }
    public static bool OpEquality(Obj? a, Obj? b) => ReferenceEquals(a, b);
    public static bool OpImplicit(Obj? o) => o != null;
}

public static class Program {
    static Type T(TypeReference r) {
        if (r is ByReferenceType br) return T(br.ElementType).MakeByRefType();
        if (r.FullName.StartsWith("System.Collections.Generic.List`1")) return typeof(List<object>);
        if (r is ArrayType at) return T(at.ElementType).MakeArrayType();
        return r.FullName switch {
            "System.Void" => typeof(void),
            "System.Object" => typeof(object),
            "System.Boolean" => typeof(bool),
            "System.Single" => typeof(float),
            "System.Int32" => typeof(int),
            "System.IntPtr" => typeof(IntPtr),
            "Window_T" => typeof(Window_T),
            "Board" => typeof(Board),
            "BoardManager" => typeof(BoardManager),
            "FTRuntime.SwfClip" => typeof(SwfClip),
            "FTRuntime.SwfClipAsset/Frame" => typeof(Frame),
            "UnityEngine.MeshFilter" => typeof(MeshFilter),
            "UnityEngine.Renderer" or "UnityEngine.MeshRenderer" => typeof(Renderer),
            "UnityEngine.Mesh" => typeof(Mesh),
            "UnityEngine.Material" => typeof(Material),
            "DeviceEvent" => typeof(DeviceEvent),
            "Device" => typeof(Device),
            "Plant" => typeof(Plant),
            "UnityEngine.GameObject" => typeof(GameObject),
            "UnityEngine.Component" or "UnityEngine.Object" => typeof(Obj),
            _ => throw new Exception("Unmapped: " + r)
        };
    }

    static Delegate Emit(MethodDefinition m, string mut) {
        int rid = (int)m.MetadataToken.RID;
        var ret = T(m.ReturnType);
        var param = new[] { T(m.DeclaringType) }.Concat(m.Parameters.Select(p => T(p.ParameterType))).ToArray();
        var dm = new DynamicMethod(m.FullName + mut, ret, param, typeof(Program).Module, false) { InitLocals = m.Body.InitLocals };
        var il = dm.GetILGenerator();
        var labels = m.Body.Instructions.ToDictionary(i => i, i => il.DefineLabel());
        var ops = typeof(RO).GetFields().Where(f => f.FieldType == typeof(System.Reflection.Emit.OpCode)).Select(f => (System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o => o.Value);
        var locals = m.Body.Variables.Select(v => il.DeclareLocal(T(v.VariableType))).ToArray();

        foreach (var i in m.Body.Instructions) {
            il.MarkLabel(labels[i]);
            var op = ops[i.OpCode.Value];

            if (i.Operand is FieldReference f) {
                var fi = T(f.DeclaringType).GetField(f.Name) ?? throw new Exception("Field: " + f);
                il.Emit(op, fi);
                continue;
            }
            if (i.Operand is MethodReference r) {
                MethodInfo mi = r.Name switch {
                    "get_gameObject" => typeof(Program).GetMethod("GetGO")!,
                    "Destroy" => typeof(Test61).GetMethod("Destroy")!,
                    "get_Instance" => typeof(BoardManager).GetMethod("get_Instance")!,
                    "Board_OnPlay" => typeof(BoardManager).GetMethod("Board_OnPlay")!,
                    "op_Equality" => typeof(Test61).GetMethod("OpEquality")!,
                    "GameContinue" => typeof(Board).GetMethod("GameContinue")!,
                    "op_Implicit" => typeof(Test61).GetMethod("OpImplicit")!,
                    "GetCurrentBakedFrame" => typeof(SwfClip).GetMethod("GetCurrentBakedFrame")!,
                    "get_CachedMesh" => typeof(Frame).GetMethod("get_CachedMesh")!,
                    "set_sharedMesh" => typeof(MeshFilter).GetMethod("set_sharedMesh")!,
                    "set_sharedMaterials" => typeof(Renderer).GetMethod("set_sharedMaterials")!,
                    "Add" when r.DeclaringType.FullName.Contains("DeviceEvent") => typeof(List<DeviceEvent>).GetMethod("Add")!,
                    "Add" when r.DeclaringType.FullName.Contains("Int32") => typeof(List<int>).GetMethod("Add")!,
                    "get_Count" => typeof(List<int>).GetProperty("Count")!.GetGetMethod()!,
                    "set_Item" => typeof(List<int>).GetProperty("Item")!.GetSetMethod()!,
                    _ => throw new Exception("Method: " + r)
                };

                if (mut == "mut_376_wrong_bound" && r.Name == "get_Count") {
                    il.Emit(RO.Call, mi);
                    il.Emit(RO.Ldc_I4_1);
                    il.Emit(RO.Add);
                    continue;
                }

                il.Emit(RO.Call, mi);
                continue;
            }

            if (i.Operand is Mono.Cecil.Cil.Instruction j) { il.Emit(op, labels[j]); continue; }
            if (i.Operand is Mono.Cecil.Cil.VariableDefinition v) { il.Emit(op, locals[v.Index]); continue; }
            if (i.Operand is ParameterDefinition a) { il.Emit(op, (short)(a.Index + 1)); continue; }
            if (i.Operand is int k) { il.Emit(op, k); continue; }
            if (i.Operand is sbyte sb) { il.Emit(op, sb); continue; }
            if (i.Operand is float fl) { il.Emit(op, fl); continue; }
            if (i.Operand is Mono.Cecil.TypeReference tr) { il.Emit(op, T(tr)); continue; }
            il.Emit(op);
        }
        return dm.CreateDelegate(System.Linq.Expressions.Expression.GetDelegateType(param.Concat(new[] { ret }).ToArray()));
    }

    public static GameObject GetGO(Component c) => c.gameObject;

    public static void Main(string[] args) {
        var asm = AssemblyDefinition.ReadAssembly(args[0]);
        var m70A = (MethodDefinition)asm.MainModule.LookupToken(0x0600070A);
        var m83E = (MethodDefinition)asm.MainModule.LookupToken(0x0600083E);
        var m114 = (MethodDefinition)asm.MainModule.LookupToken(0x06000114);
        var m376 = (MethodDefinition)asm.MainModule.LookupToken(0x06000376);

        var del70A = Emit(m70A, "none");
        var del83E = Emit(m83E, "none");
        var del114 = Emit(m114, "none");
        var del376 = Emit(m376, "none");

        // Test 1: Window_T (0x0600070A)
        Test61.destroyedObjects = 0;
        var b1 = new Board();
        BoardManager.instance = new BoardManager { board = b1 };
        var win1 = new Window_T { T = 5 };
        del70A.DynamicInvoke(win1);
        bool p1 = (b1.continued == true && Test61.destroyedObjects == 2);

        Test61.destroyedObjects = 0;
        var b2 = new Board();
        BoardManager.instance = new BoardManager { board = b2 };
        var win2 = new Window_T { T = 3 };
        del70A.DynamicInvoke(win2);
        bool p2 = (b2.continued == false && Test61.destroyedObjects == 1);

        Test61.destroyedObjects = 0;
        var b3 = new Board();
        BoardManager.instance = new BoardManager { board = b3 };
        var win3 = new Window_T { T = 15 };
        del70A.DynamicInvoke(win3);
        bool p3 = (b3.continued == false && Test61.destroyedObjects == 0);

        // Test 2: SwfClip (0x0600083E)
        var clip = new SwfClip {
            _meshFilter = new MeshFilter(),
            _meshRenderer = new Renderer(),
            _dirtyMesh = true,
            frame = new Frame { mesh = new Mesh(), Materials = new Material[] { new Material() } }
        };
        del83E.DynamicInvoke(clip);
        bool p4 = (clip._dirtyMesh == false && clip._meshFilter.sharedMesh == clip.frame.mesh && clip._meshRenderer.sharedMaterials == clip.frame.Materials);

        // Test 3: DeviceEvent (0x06000114)
        var dev = new Device();
        var ev = new DeviceEvent();
        del114.DynamicInvoke(ev, dev, 2.5f);
        bool p5 = (dev.deviceEvents_toAdd.Contains(ev) && ev.device == dev && ev.countdown == 2.5f);

        // Test 4: Plant (0x06000376) - High fidelity boundary
        // Case A: Initial Count == 0 < index (3). Loop expands to Count == 3, then set_Item(3, 42) throws ArgumentOutOfRangeException!
        var plant = new Plant();
        bool threwOutOfRange = false;
        try {
            del376.DynamicInvoke(plant, 3, 42);
        } catch (TargetInvocationException tie) when (tie.InnerException is ArgumentOutOfRangeException) {
            threwOutOfRange = true;
        }
        bool p6a = (threwOutOfRange && plant.parameter_ints.Count == 3);

        // Case B: Initial Count == 5 >= index + 1 (3). Does not expand, sets parameter_ints[3] = 42
        var plant2 = new Plant { parameter_ints = new List<int> { 1, 2, 3, 4, 5 } };
        del376.DynamicInvoke(plant2, 3, 42);
        bool p6b = (plant2.parameter_ints.Count == 5 && plant2.parameter_ints[3] == 42);
        bool p6 = p6a && p6b;

        // Mutant on 376
        var del376_mut = Emit(m376, "mut_376_wrong_bound");
        var plant_mut = new Plant();
        bool mut_threw = false;
        try {
            del376_mut.DynamicInvoke(plant_mut, 3, 42);
        } catch (TargetInvocationException tie) when (tie.InnerException is ArgumentOutOfRangeException) {
            mut_threw = true;
        }
        bool mut_detected = (plant_mut.parameter_ints.Count != plant.parameter_ints.Count || mut_threw != threwOutOfRange);

        bool allPass = p1 && p2 && p3 && p4 && p5 && p6 && mut_detected;
        var rep = new {
            candidate_sha256 = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[0]))).ToLowerInvariant(),
            m70A_pass = p1 && p2 && p3,
            m83E_pass = p4,
            m114_pass = p5,
            m376_pass = p6,
            mutant_detected = mut_detected,
            all_positive_passed = allPass
        };
        File.WriteAllText(args[1], JsonSerializer.Serialize(rep, new JsonSerializerOptions { WriteIndented = true }));
        Console.WriteLine(JsonSerializer.Serialize(rep));
        if (!allPass) Environment.Exit(1);
    }
}
