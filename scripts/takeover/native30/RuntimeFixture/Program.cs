using Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using CI = Mono.Cecil.Cil.Instruction;
using RO = System.Reflection.Emit.OpCodes;

public class InfrastructureFault : Exception { public InfrastructureFault(string s) : base(s) { } }

public static class Tr {
    public static List<string> Events = new();
    public static string Fail = "";
    public static Action? Truth, Sequence, Rewind;
    public static object? Made;
    public static Vec Position;
    public static void Reset() {
        Events.Clear();
        Fail = "";
        Truth = Sequence = Rewind = null;
        Made = null;
        Position = new(3, -5, 7);
    }
    public static void E(string s) {
        Events.Add(s);
        if (Fail == s) throw new ApplicationException(s);
    }
}

// Explicit helper doubles: no Unity player or original engine/helper bodies run here.
public class Obj {
    public bool Alive = true;
    public static bool op_Implicit(Obj? o) {
        bool v = o != null && o.Alive;
        var cb = Tr.Truth;
        Tr.Truth = null;
        cb?.Invoke();
        return v;
    }
    public static bool op_Equality(Obj? a, Obj? b) => (a == null || !a.Alive) == (b == null || !b.Alive) && ((a == null || !a.Alive) || ReferenceEquals(a, b));
    public static bool op_Inequality(Obj? a, Obj? b) => !op_Equality(a, b);
    public static void Destroy(Obj? o) { Tr.E("destroy"); if (o != null) o.Alive = false; }
    public static T Instantiate<T>(T original) where T : Obj {
        if (original == null) throw new NullReferenceException();
        Tr.E("instantiate");
        return original;
    }
}

public struct Vec {
    public float x, y, z;
    public Vec(float a, float b, float c) { x = a; y = b; z = c; }
    public static Vec get_forward() => new Vec(0, 0, 1);
    public static Vec get_zero() => new Vec(0, 0, 0);
}

public struct Color {
    public float r, g, b, a;
    public Color(float x, float y, float z, float w) { r = x; g = y; b = z; a = w; }
}

public struct SettingsData {
    public int MaxAtlasSize, AtlasPadding;
    public float PixelsPerUnit;
    public bool BitmapTrimming, GenerateMipMaps, AtlasPowerOfTwo, AtlasForceSquare;
    public int AtlasTextureFilter, AtlasTextureFormat;
}

public class SwfSettings { public SettingsData Settings; }
public class Texture : Obj { }
public class Asset {
    public byte[]? Data;
    public string? Hash;
    public Texture? Atlas;
    public SettingsData Settings, Overridden;
}

public class Clip : Obj {
    public Color _tint;
    public void UpdatePropBlock() { Tr.E("props"); }
    public void set_sequence(string? s) { Tr.E("sequence:" + (s ?? "<null>")); Tr.Sequence?.Invoke(); }
}

public class Controller {
    public Clip? _clip;
    public bool _isPlaying;
    public float _tickTimer;
    public Action<Controller>? OnPlayStoppedEvent, OnStopPlayingEvent;
    public void Rewind() { Tr.E("rewind:" + (_isPlaying ? 1 : 0) + ":" + BitConverter.SingleToInt32Bits(_tickTimer)); Tr.Rewind?.Invoke(); }
}

public class Wait {
    public Controller? Subscribed;
    public Wait(Controller? c) { Tr.E("ctor:" + (c == null ? "null" : "controller")); Tr.Made = this; }
}
public class WaitStop : Wait { public WaitStop(Controller? c) : base(c) { } public WaitStop Subscribe(Controller c) { Tr.E("subscribe"); Subscribed = c; return null!; } }
public class WaitRewind : Wait { public WaitRewind(Controller? c) : base(c) { } public WaitRewind Subscribe(Controller c) { Tr.E("subscribe"); Subscribed = c; return null!; } }
public class WaitEither : Wait { public WaitEither(Controller? c) : base(c) { } public WaitEither Subscribe(Controller c) { Tr.E("subscribe"); Subscribed = c; return null!; } }
public class WaitPlay : Wait { public WaitPlay(Controller? c) : base(c) { } public WaitPlay Subscribe(Controller c) { Tr.E("subscribe"); Subscribed = c; return null!; } }

public class GameObject : Obj {
    public Transform transform = new();
    public Transform get_transform() => transform;
    public void SetActive(bool a) { Tr.E("active:" + a); }
}

public class Transform {
    public Vec localPosition;
    public Vec get_position() { Tr.E("position"); return Tr.Position; }
    public void set_position(Vec v) { Tr.E("set_pos"); Tr.Position = v; }
    public void set_localPosition(Vec v) { Tr.E("set_local_pos"); localPosition = v; }
    public void Rotate(Vec axis, float angle, int space) { Tr.E($"rotate:{BitConverter.SingleToInt32Bits(axis.x)}:{BitConverter.SingleToInt32Bits(axis.y)}:{BitConverter.SingleToInt32Bits(axis.z)}:{BitConverter.SingleToInt32Bits(angle)}:{space}"); }
}

public class Component : Obj {
    public static Transform? Transform = new();
    public static GameObject? GameObject = new();
    public Transform? get_transform() { Tr.E("transform"); if (this == null) throw new NullReferenceException(); return Transform; }
    public GameObject? get_gameObject() { Tr.E("gameobject"); if (this == null) throw new NullReferenceException(); return GameObject; }
}

public class Camera : Component {
    public static Camera? Main = new();
    public static Camera? get_main() { Tr.E("camera"); return Main; }
}

public class BoardConfig {
    public Vec GetGridPosition(int x, int y) { Tr.E("grid:" + x + ":" + y); return Tr.Position; }
    public Vec GetGridCenterPosition(int x, int y) { Tr.E($"grid_center:{x}:{y}"); return new Vec(x * 10, y * 10, 0); }
}

public class Range { }
public class SwfManager { public HashSet<string>? _groupPauses, _groupUnscales; }
public class AudioClip : Obj { }
public class AudioSource : Obj {
    public static AudioClip? LastClip;
    public static Vec LastPosition;
    public static float LastVolume, LastPitch;
    public static void PlayClipAtPoint(AudioClip? c, Vec p, float v) { Tr.E("play"); LastClip = c; LastPosition = p; LastVolume = v; }
}

public class Lawn { public float audioVolume; }
public static class Global {
    public static Lawn? gLawnApp;
    public static float AudioVolume() { Tr.E("volume"); return .375f; }
    public static AudioSource CreateAudioAtPoint(AudioClip? c, Vec p, float v) { Tr.E("create3"); AudioSource.LastClip = c; AudioSource.LastPosition = p; AudioSource.LastVolume = v; return new(); }
    public static AudioSource CreateAudioAtPoint(AudioClip? c, Vec p, float v, float f) { Tr.E("create4"); AudioSource.LastClip = c; AudioSource.LastPosition = p; AudioSource.LastVolume = v; AudioSource.LastPitch = f; return new(); }
}

public static class RandomDouble { public static float Range(float a, float b) { Tr.E("random:" + BitConverter.SingleToInt32Bits(a) + ":" + BitConverter.SingleToInt32Bits(b)); return 1.025f; } }

public class Project : Component {
    public int projectType;
    public static object? End;
    public void SetEndPosition<T>(T z) { Tr.E("end"); End = z; }
    public void SetNatureSunInitialPostion() { Tr.E("nature_sun"); }
}

public class ProjectManager {
    public static Project? Result = new();
    public Project? CreateProject(int type, int id, Vec p) { Tr.E("project:" + type + ":" + id); AudioSource.LastPosition = p; return Result; }
}

public class Board : Component {
    public bool gameStart;
    public ProjectManager? projectManager;
    public void GamePause(bool p) { Tr.E("pause:" + p); }
    public void PopMenu() { Tr.E("pop_menu"); }
    public void ButtonDown_Menu() { }
    public void PlayAudio_Button(AudioClip c) { }
}

public class Dialogue { public int dialogueTriggerType; }
public class DialogueManager { public Board? board; public int dialogueTriggerType; }

public class TMP_Text : Obj {
    public string text = "";
    public void set_text(string s) { Tr.E("text:" + s); text = s; }
}

public class SunManager : Component {
    public Board? board;
    public TMP_Text? sunPointText;
    public int sunPoint;
    public int sunFallCount;
    public void SunFall() { }
    public void SunPointTextUpdate() { }
}

public class Animator : Obj {
    public void SetTrigger(string s) { Tr.E("trigger:" + s); }
}

public class Element { public float point; }
public class ElementManager {
    public static Element? Element;
    public Element? GetElement(int type) { Tr.E("get_element:" + type); return Element; }
}

public class Zombie : Component {
    public ElementManager? elementManager;
    public Animator? animator;
    public float fX, fY;
    public void Update_PreviousPosition() { Tr.E("prev_pos"); }
    public void PlaySound(AudioClip c) { }
    public void TeleportTo(Vec v, float f, bool b) { }
    public float GetElementPoint(int t) => 0f;
    public bool TryShooting() => false;
}

public class Supplies { public int ID; }
public class SuppliesInfo { public int id; }
public class Warehouse_SuppliesInfoPage : Obj {
    public Supplies? supplies;
    public SuppliesInfo? suppliesInfo;
    public void SetInfo() { Tr.E("set_info"); }
}

public class WarehouseUIController : Component {
    public Warehouse_SuppliesInfoPage? suppliesInfoPage;
    public void CheckSupplies(Supplies s) { }
}

public class Popup : Component {
    public int P;
    public int info0_int;
    public static Popup PopupPopup(int p) => null!;
    public static Popup PopupPopup(int p, int i) => null!;
}

public class Window_Q : Component {
    public int info0_int;
    public static Window_Q? PopupNewWindow(int q, Transform p) { Tr.E($"popup_q:{q}"); return new Window_Q(); }
    public static Window_Q PopupNewWindow(int q, Transform p, int i) => null!;
}

public class Prop : Component {
    public GameObject? prop;
    public Vec originalPosition;
    public int propState;
    public GameObject? propImagex;
    public void PropBankStateSet() { Tr.E("prop_bank"); }
    public void DropDown() { }
}

public class ResourceManager {
    public static List<AudioClip>? boardClips;
    public static List<AudioClip>? mouseClips;
    public static Popup? prefab_Popup;
    public static SuppliesInfo? GetSuppliesInfo(int id) { Tr.E("get_sup_info:" + id); return new SuppliesInfo { id = id }; }
}

public static class TimeDouble {
    public static float get_deltaTime() => 0.02f;
}

public class SwfUtils {
    public static void UnpackUV(uint pack, ref float u, ref float v) { }
}
public class MouseManager : Component { public void AudioPlay(int id) { } }
public class ProjectAnimationEvent : Component { public void PlayWinAudio() { } }
public class Admin_system2_boardEdior : Component { public BoardConfig? boardConfig; public void Return() { } }
public class EnemyPath { public int gridX, gridY; }
public class Enemy_boardEditor { public Admin_system2_boardEdior? boardEdior; }
public class Path_enemyEditor : Component { public Enemy_boardEditor? enemyEditor; public EnemyPath? enemyPath; public void LoadData() { } }

static class Native30Fixture {
    static readonly Dictionary<int, string> Contracts = new() {
        // Inherited 31 methods from Native29
        [0x0600019B] = "System.Boolean DialogueManager_OnBoard::TestLogTrigger(BoardDialogue)",
        [0x060008AA] = "FTRuntime.Yields.SwfWaitStopPlaying FTRuntime.Yields.SwfWaitExtensions::PlayAndWaitStop(FTRuntime.SwfClipController,System.Boolean)",
        [0x060008AC] = "FTRuntime.Yields.SwfWaitRewindPlaying FTRuntime.Yields.SwfWaitExtensions::PlayAndWaitRewind(FTRuntime.SwfClipController,System.Boolean)",
        [0x060008AE] = "FTRuntime.Yields.SwfWaitStopOrRewindPlaying FTRuntime.Yields.SwfWaitExtensions::PlayAndWaitStopOrRewind(FTRuntime.SwfClipController,System.Boolean)",
        [0x060008B6] = "FTRuntime.Yields.SwfWaitPlayStopped FTRuntime.Yields.SwfWaitExtensions::StopAndWaitPlay(FTRuntime.SwfClipController,System.Boolean)",
        [0x060008B7] = "FTRuntime.Yields.SwfWaitPlayStopped FTRuntime.Yields.SwfWaitExtensions::StopAndWaitPlay(FTRuntime.SwfClipController,System.String)",
        [0x0600082C] = "UnityEngine.Color FTRuntime.SwfClip::get_tint()",
        [0x0600082D] = "System.Void FTRuntime.SwfClip::set_tint(UnityEngine.Color)",
        [0x060008A3] = "System.Void FTRuntime.SwfSettings::Reset()",
        [0x060008A1] = "FTRuntime.SwfSettingsData FTRuntime.SwfSettingsData::get_identity()",
        [0x06000820] = "System.Void FTRuntime.SwfAsset::Reset()",
        [0x06000160] = "UnityEngine.Vector3 BoardConfig::Position_GridToDevice(UnityEngine.Vector3)",
        [0x06000161] = "UnityEngine.Vector3 BoardConfig::Position_GridToPlanting(UnityEngine.Vector3)",
        [0x06000162] = "UnityEngine.Vector3 BoardConfig::Position_GridToZombie(UnityEngine.Vector3)",
        [0x0600015D] = "UnityEngine.Vector3 BoardConfig::GetDevicePosition(System.Int32,System.Int32)",
        [0x0600015E] = "UnityEngine.Vector3 BoardConfig::GetZombiePosition(System.Int32,System.Int32)",
        [0x0600015F] = "UnityEngine.Vector3 BoardConfig::GetPlantingPosition(System.Int32,System.Int32)",
        [0x060000DF] = "UnityEngine.Vector3 AttackRange::RestraintTargetPosition_Rect(UnityEngine.Vector3)",
        [0x0600013C] = "UnityEngine.AudioSource GlobalStaticVars::CreateAudioAtPoint(UnityEngine.AudioClip,UnityEngine.Vector3)",
        [0x060000A8] = "System.Void AnimationEvent::PlayAudio(UnityEngine.AudioClip)",
        [0x060000AF] = "System.Void DeviceAnimationEvent::PlayAudio(UnityEngine.AudioClip)",
        [0x060000BB] = "System.Void PlantAnimationEvent::PlayAudio(UnityEngine.AudioClip)",
        [0x060002C6] = "System.Void Board::PlayAudio_Button(UnityEngine.AudioClip)",
        [0x0600046C] = "System.Void Zombie::PlaySound(UnityEngine.AudioClip)",
        [0x060006D3] = "System.Void SettlementUIController::PlayAudio(UnityEngine.AudioClip)",
        [0x06000203] = "System.Void ProjectManager::DropSpawnLevelAward(Zombie,UnityEngine.Vector3)",
        [0x0600088B] = "System.Void FTRuntime.SwfManager::ResumeGroup(System.String)",
        [0x0600088C] = "System.Boolean FTRuntime.SwfManager::IsGroupPaused(System.String)",
        [0x0600088D] = "System.Boolean FTRuntime.SwfManager::IsGroupPlaying(System.String)",
        [0x0600088E] = "System.Void FTRuntime.SwfManager::SetGroupUseUnscaledDt(System.String,System.Boolean)",
        [0x0600088F] = "System.Boolean FTRuntime.SwfManager::IsGroupUseUnscaledDt(System.String)",

        // 17 Native30 methods
        [0x060000C1] = "System.Void ProjectAnimationEvent::PlayWinAudio()",
        [0x060001C7] = "System.Void MouseManager::AudioPlay(System.Int32)",
        [0x060002B4] = "System.Void Board::ButtonDown_Menu()",
        [0x06000480] = "System.Void Zombie::TeleportTo(UnityEngine.Vector3,System.Single,System.Boolean)",
        [0x0600001F] = "System.Void Admin_system2_boardEdior::Return()",
        [0x06000060] = "System.Void Path_enemyEditor::LoadData()",
        [0x060003C9] = "System.Void Project::Rotating()",
        [0x0600026A] = "System.Void SunManager::SunFall()",
        [0x0600026B] = "System.Void SunManager::SunPointTextUpdate()",
        [0x06000448] = "System.Single Zombie::GetElementPoint(ElementType)",
        [0x06000488] = "System.Boolean Zombie::TryShooting()",
        [0x060006E9] = "System.Void WarehouseUIController::CheckSupplies(Supplies)",
        [0x06000675] = "Popup Popup::PopupPopup(System.Int32)",
        [0x06000673] = "Popup Popup::PopupPopup(System.Int32,System.Int32)",
        [0x06000704] = "Window_Q Window_Q::PopupNewWindow(System.Int32,UnityEngine.Transform,System.Int32)",
        [0x060008F2] = "System.Void FTRuntime.Internal.SwfUtils::UnpackUV(System.UInt32,System.Single&,System.Single&)",
        [0x06000401] = "System.Void Prop::DropDown()"
    };

    static readonly Dictionary<string, Type> Types = new() {
        ["System.Object"] = typeof(object),
        ["System.Int32"] = typeof(int),
        ["System.Int64"] = typeof(long),
        ["System.IntPtr"] = typeof(nint),
        ["System.Single"] = typeof(float),
        ["System.UInt32"] = typeof(uint),
        ["System.Boolean"] = typeof(bool),
        ["System.Void"] = typeof(void),
        ["System.String"] = typeof(string),
        ["System.Byte"] = typeof(byte),
        ["System.NullReferenceException"] = typeof(NullReferenceException),
        ["UnityEngine.Object"] = typeof(Obj),
        ["UnityEngine.GameObject"] = typeof(GameObject),
        ["UnityEngine.Vector3"] = typeof(Vec),
        ["UnityEngine.Color"] = typeof(Color),
        ["FTRuntime.SwfSettingsData"] = typeof(SettingsData),
        ["FTRuntime.SwfSettingsData/AtlasFilter"] = typeof(int),
        ["FTRuntime.SwfSettingsData/AtlasFormat"] = typeof(int),
        ["FTRuntime.SwfSettings"] = typeof(SwfSettings),
        ["FTRuntime.SwfAsset"] = typeof(Asset),
        ["UnityEngine.Texture2D"] = typeof(Texture),
        ["FTRuntime.SwfClip"] = typeof(Clip),
        ["FTRuntime.SwfClipController"] = typeof(Controller),
        ["FTRuntime.Yields.SwfWaitStopPlaying"] = typeof(WaitStop),
        ["FTRuntime.Yields.SwfWaitRewindPlaying"] = typeof(WaitRewind),
        ["FTRuntime.Yields.SwfWaitStopOrRewindPlaying"] = typeof(WaitEither),
        ["FTRuntime.Yields.SwfWaitPlayStopped"] = typeof(WaitPlay),
        ["Board"] = typeof(Board),
        ["BoardDialogue"] = typeof(Dialogue),
        ["DialogueTriggerType"] = typeof(int),
        ["DialogueManager_OnBoard"] = typeof(DialogueManager),
        ["BoardConfig"] = typeof(BoardConfig),
        ["AttackRange"] = typeof(Range),
        ["FTRuntime.SwfManager"] = typeof(SwfManager),
        ["UnityEngine.AudioClip"] = typeof(AudioClip),
        ["UnityEngine.AudioSource"] = typeof(AudioSource),
        ["UnityEngine.Transform"] = typeof(Transform),
        ["UnityEngine.Component"] = typeof(Component),
        ["UnityEngine.Camera"] = typeof(Camera),
        ["UnityEngine.Random"] = typeof(RandomDouble),
        ["GlobalStaticVars"] = typeof(Global),
        ["GlobalStaticVars/LawnApp"] = typeof(Lawn),
        ["AnimationEvent"] = typeof(Component),
        ["DeviceAnimationEvent"] = typeof(Component),
        ["PlantAnimationEvent"] = typeof(Component),
        ["Zombie"] = typeof(Zombie),
        ["SettlementUIController"] = typeof(Component),
        ["ProjectManager"] = typeof(ProjectManager),
        ["Project"] = typeof(Project),
        ["TMPro.TextMeshProUGUI"] = typeof(TMP_Text),
        ["TMPro.TMP_Text"] = typeof(TMP_Text),
        ["SunManager"] = typeof(SunManager),
        ["UnityEngine.Animator"] = typeof(Animator),
        ["ElementType"] = typeof(int),
        ["Element"] = typeof(Element),
        ["ElementManager"] = typeof(ElementManager),
        ["Supplies"] = typeof(Supplies),
        ["SuppliesInfo"] = typeof(SuppliesInfo),
        ["Warehouse_SuppliesInfoPage"] = typeof(Warehouse_SuppliesInfoPage),
        ["WarehouseUIController"] = typeof(WarehouseUIController),
        ["Popup"] = typeof(Popup),
        ["Window_Q"] = typeof(Window_Q),
        ["Prop"] = typeof(Prop),
        ["ResourceManager"] = typeof(ResourceManager),
        ["UnityEngine.Time"] = typeof(TimeDouble),
        ["UnityEngine.Space"] = typeof(int),
        ["FTRuntime.Internal.SwfUtils"] = typeof(SwfUtils),
        ["MouseManager"] = typeof(MouseManager),
        ["ProjectAnimationEvent"] = typeof(ProjectAnimationEvent),
        ["Admin_system2_boardEdior"] = typeof(Admin_system2_boardEdior),
        ["Path_enemyEditor"] = typeof(Path_enemyEditor),
        ["Enemy_boardEditor"] = typeof(Enemy_boardEditor),
        ["EnemyPath"] = typeof(EnemyPath)
    };

    static Type T(TypeReference t, GenericInstanceType? context = null) {
        if (t is ByReferenceType br) return T(br.ElementType, context).MakeByRefType();
        if (t is GenericParameter gp) {
            if (context == null || gp.Type != GenericParameterType.Type) throw new InfrastructureFault("Unbound generic variable");
            return T(context.GenericArguments[gp.Position]);
        }
        if (t is ArrayType a) return T(a.ElementType).MakeArrayType();
        if (t is GenericInstanceType g) return g.ElementType.FullName switch {
            "System.Action`1" => typeof(Action<>).MakeGenericType(T(g.GenericArguments[0])),
            "System.Collections.Generic.HashSet`1" => typeof(HashSet<>).MakeGenericType(T(g.GenericArguments[0])),
            "System.Collections.Generic.List`1" => typeof(List<>).MakeGenericType(T(g.GenericArguments[0])),
            _ => throw new InfrastructureFault("Unmapped generic " + g.FullName)
        };
        return Types.TryGetValue(t.FullName, out var r) ? r : throw new InfrastructureFault("Unmapped " + t.FullName);
    }

    static AssemblyDefinition? Assembly;
    static bool InvokeFault;
    static int Changes;

    static DynamicMethod Emit(MethodDefinition m, string mutant = "none") {
        if (m.FullName != Contracts[m.MetadataToken.ToInt32()]) throw new InfrastructureFault($"Pinned signature mismatch for 0x{m.MetadataToken.ToInt32():X8}:\nExpected: {Contracts[m.MetadataToken.ToInt32()]}\nActual:   {m.FullName}");
        if (mutant == "fault_emit") throw new InfrastructureFault("Injected emitter failure");
        var dm = new DynamicMethod(m.Name + mutant, T(m.ReturnType), (m.HasThis ? new[] { T(m.DeclaringType) } : Array.Empty<Type>()).Concat(m.Parameters.Select(p => T(p.ParameterType))).ToArray(), typeof(Native30Fixture).Module, true) { InitLocals = m.Body.InitLocals };
        var il = dm.GetILGenerator();
        var locals = m.Body.Variables.Select(v => il.DeclareLocal(T(v.VariableType))).ToArray();
        var labels = m.Body.Instructions.ToDictionary(i => i, i => il.DefineLabel());
        var ops = typeof(RO).GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.FieldType == typeof(System.Reflection.Emit.OpCode)).Select(f => (System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o => o.Value);
        bool changed = false;
        int priorLoads = 0;
        void Changed() { changed = true; Changes++; }
        void Zero(Type t) { var v = il.DeclareLocal(t); il.Emit(RO.Ldloca, v); il.Emit(RO.Initobj, t); il.Emit(RO.Ldloc, v); }

        if (mutant == "illegal_stack") il.Emit(RO.Pop);
        foreach (var i in m.Body.Instructions) {
            il.MarkLabel(labels[i]);
            var op = ops[i.OpCode.Value];
            var a = i.Operand;

            if (mutant == "post_state" && (op == RO.Ldloc_1 || op == RO.Ldloc_2)) {
                priorLoads++;
                if (priorLoads == 2) {
                    il.Emit(RO.Ldarg_0);
                    il.Emit(RO.Ldfld, typeof(Controller).GetField("_isPlaying")!);
                    Changed();
                    continue;
                }
            }
            if (mutant == "identity_zero" && op == RO.Ldarg_1) { Zero(typeof(Vec)); Changed(); continue; }
            if (mutant == "drop_write" && !changed && op == RO.Stfld) { il.Emit(RO.Pop); il.Emit(RO.Pop); Changed(); continue; }
            if (mutant == "wrong_dispatch" && !changed && op == RO.Ldc_I4_1) { il.Emit(RO.Ldc_I4_2); Changed(); continue; }
            if (mutant == "wrong_constant" && !changed && op == RO.Ldc_I4 && a is int z && z == 2048) { il.Emit(RO.Ldc_I4, 1024); Changed(); continue; }
            if (mutant == "wrong_constant" && !changed && op == RO.Ldc_R4 && a is float f0 && f0 == 0.0f) { il.Emit(RO.Ldc_R4, 1.0f); Changed(); continue; }
            if (mutant == "wrong_offset" && !changed && op == RO.Ldc_R4 && a is float f) { il.Emit(op, f + 1); Changed(); continue; }
            if (mutant == "wrong_shift" && !changed && op == RO.Ldc_I4_S && a is sbyte sb && sb == 16) { il.Emit(RO.Ldc_I4_S, (sbyte)8); Changed(); continue; }
            if (mutant == "wrong_sub" && !changed && op == RO.Add) { il.Emit(RO.Sub); Changed(); continue; }
            if (mutant == "return_subscribe" && op == RO.Pop && i.Previous?.Operand is MethodReference pm && pm.Name == "Subscribe") {
                var returned = il.DeclareLocal(T(pm.ReturnType));
                il.Emit(RO.Stloc, returned);
                il.Emit(RO.Pop);
                il.Emit(RO.Ldloc, returned);
                Changed();
                continue;
            }
            if (a is MethodReference c) {
                if (mutant == "wrong_axis" && m.MetadataToken.ToInt32() == 0x060003C9 && c.Name == "get_forward") { Zero(typeof(Vec)); Changed(); continue; }
                if (mutant == "plain_null_check" && m.MetadataToken.ToInt32() == 0x06000401 && c.Name == "op_Inequality") { il.Emit(RO.Pop); il.Emit(RO.Ldnull); il.Emit(RO.Cgt_Un); Changed(); continue; }
                if (mutant == "drop_callback" && c.Name == "Invoke") { il.Emit(RO.Pop); il.Emit(RO.Pop); Changed(); continue; }
                if (mutant == "drop_set_text" && c.Name == "set_text") { il.Emit(RO.Pop); il.Emit(RO.Pop); Changed(); continue; }
                if (mutant == "drop_trigger" && c.Name == "SetTrigger") { il.Emit(RO.Pop); il.Emit(RO.Pop); Changed(); continue; }
                if (mutant == "drop_set_info" && c.Name == "SetInfo") { il.Emit(RO.Pop); Changed(); continue; }
                var context = c.DeclaringType as GenericInstanceType;
                var owner = T(c.DeclaringType);
                var ps = c.Parameters.Select(p => p.ParameterType is GenericParameter gp && gp.Type == GenericParameterType.Method && c is GenericInstanceMethod gi ? T(gi.GenericArguments[gp.Position]) : T(p.ParameterType, context)).ToArray();
                if (c.Name == "get_identity" && c.DeclaringType.FullName == "FTRuntime.SwfSettingsData") {
                    il.Emit(op, Emit((MethodDefinition)Assembly!.MainModule.LookupToken(0x060008A1)));
                    continue;
                }
                if (c.Name == ".ctor") il.Emit(op, owner.GetConstructor(ps) ?? throw new InfrastructureFault("Ctor unmapped " + c.FullName));
                else if (c is GenericInstanceMethod gm) {
                    var mi = owner.GetMethods().Single(x => x.Name == c.Name && x.IsGenericMethodDefinition).MakeGenericMethod(gm.GenericArguments.Select(x => T(x)).ToArray());
                    il.Emit(op, mi);
                } else {
                    var mi = owner.GetMethod(mutant == "wrong_set" && c.Name == "Remove" ? "Add" : c.Name, ps) ?? throw new InfrastructureFault("Call unmapped " + c.FullName);
                    il.Emit(op, mi);
                    if (mutant == "wrong_set" && c.Name == "Remove") Changed();
                }
                if (mutant == "wrong_vector" && (c.Name == "get_position" || c.Name == "GetGridCenterPosition")) { il.Emit(RO.Pop); Zero(typeof(Vec)); Changed(); }
                if (mutant == "wrong_set" && c.Name == "Contains") { il.Emit(RO.Ldc_I4_0); il.Emit(RO.Ceq); Changed(); }
                continue;
            }
            if (a is FieldReference field) {
                il.Emit(op, T(field.DeclaringType).GetField(field.Name) ?? throw new InfrastructureFault("Field unmapped " + field.FullName));
                if (mutant == "default_color" && op == RO.Ldfld && field.Name == "_tint") { il.Emit(RO.Pop); Zero(typeof(Color)); Changed(); }
                continue;
            }
            if (a is CI j) { il.Emit(op, labels[j]); continue; }
            if (a is CI[] js) { il.Emit(op, js.Select(j => labels[j]).ToArray()); continue; }
            if (a is Mono.Cecil.Cil.VariableDefinition v) { il.Emit(op, locals[v.Index]); continue; }
            if (a is ParameterDefinition p) {
                int ix = p.Index + (m.HasThis ? 1 : 0);
                if (mutant == "wrong_vector" && T(p.ParameterType) == typeof(Vec) && op == RO.Ldarg) { Zero(typeof(Vec)); Changed(); }
                else il.Emit(op, (short)ix);
                continue;
            }
            if (a is TypeReference rt) { il.Emit(op, T(rt)); continue; }
            if (a is int x) { il.Emit(op, x); continue; }
            if (a is float f32) { il.Emit(op, f32); continue; }
            if (a is sbyte b) { il.Emit(op, b); continue; }
            if (a is string s) { il.Emit(op, s); continue; }
            if (a == null) { il.Emit(op); continue; }
            throw new InfrastructureFault("Unknown operand");
        }
        if (mutant != "none" && mutant != "illegal_stack" && !changed) throw new InfrastructureFault("Mutation not applied " + mutant);
        return dm;
    }

    static (object? Value, string? Error) Invoke(DynamicMethod dm, object?[] args) {
        if (InvokeFault) throw new InfrastructureFault("Injected invoke failure");
        try {
            return (dm.Invoke(null, args), null);
        } catch (TargetInvocationException e) when (e.InnerException is NullReferenceException or ApplicationException or InvalidProgramException or System.Security.VerificationException) {
            return (null, e.InnerException!.GetType().Name);
        } catch (InvalidProgramException e) {
            return (null, e.GetType().Name);
        } catch (System.Security.VerificationException e) {
            return (null, e.GetType().Name);
        }
    }

    static bool Bits(Vec a, Vec b) => BitConverter.SingleToInt32Bits(a.x) == BitConverter.SingleToInt32Bits(b.x) && BitConverter.SingleToInt32Bits(a.y) == BitConverter.SingleToInt32Bits(b.y) && BitConverter.SingleToInt32Bits(a.z) == BitConverter.SingleToInt32Bits(b.z);
    static bool Bits(Color a, Color b) => new[] { a.r, a.g, a.b, a.a }.Select(BitConverter.SingleToInt32Bits).SequenceEqual(new[] { b.r, b.g, b.b, b.a }.Select(BitConverter.SingleToInt32Bits));
    static bool DefaultSettings(SettingsData s) => s.MaxAtlasSize == 2048 && s.AtlasPadding == 1 && s.PixelsPerUnit == 100 && s.BitmapTrimming && !s.GenerateMipMaps && s.AtlasPowerOfTwo && s.AtlasForceSquare && s.AtlasTextureFilter == 1 && s.AtlasTextureFormat == 2;

    record Case(string Name, bool Pass, string? Error, string[] Events, string[] Expected);

    static List<Case> Check(int token, DynamicMethod dm) {
        var rows = new List<Case>();
        void Test(string name, Func<object?[]> setup, Func<object?, bool> expected, string? error = null, params string[] events) {
            Tr.Reset();
            var args = setup();
            var (v, e) = Invoke(dm, args);
            rows.Add(new(name, e == error && (e != null || expected(v)) && Tr.Events.SequenceEqual(events), e, Tr.Events.ToArray(), events));
        }

        // --- Inherited 31 methods ---
        if (token == 0x0600019B) {
            foreach (int board in new[] { 0, 1, 2 })
                foreach (bool started in new[] { false, true })
                    foreach (int kind in new[] { -1, 0, 1, 2, int.MaxValue })
                        foreach (bool nil in new[] { false, true }) {
                            var d = new DialogueManager { board = board == 0 ? null : new Board { Alive = board == 1, gameStart = started }, dialogueTriggerType = 17 };
                            bool active = board == 1;
                            Test($"dialogue:{board}:{started}:{kind}:{nil}", () => new object?[] { d, nil ? null : new Dialogue { dialogueTriggerType = kind } }, v => Equals(v, active && (kind == 0 || kind == 1 && started)) && d.dialogueTriggerType == (active ? kind : 17), active && nil ? "NullReferenceException" : null);
                        }
            Test("null-this", () => new object?[] { null, null }, _ => false, "NullReferenceException");
            return rows;
        }

        if (token is 0x060008AA or 0x060008AC or 0x060008AE or 0x060008B6 or 0x060008B7) {
            bool play = token is 0x060008AA or 0x060008AC or 0x060008AE;
            bool seq = token == 0x060008B7;
            foreach (bool before in new[] { false, true })
                foreach (bool rewind in new[] { false, true })
                    foreach (int actions in new[] { 0, 1, 2 })
                        foreach (int change in new[] { 0, 1, 2 })
                            foreach (int fail in new[] { 0, 1, 2, 3 }) {
                                var c = new Controller { _clip = new Clip(), _isPlaying = before, _tickTimer = 9 };
                                bool prior = seq && change == 1 ? !before : before;
                                bool write = play ? !prior : prior;
                                bool after = write ? play : prior;
                                float tick = write ? 0 : 9;
                                bool doRewind = seq || rewind;
                                bool callback = play ? !prior : prior;
                                var events = new List<string>();
                                if (seq) events.Add("sequence:run");
                                if (doRewind) events.Add("rewind:" + (after ? 1 : 0) + ":" + BitConverter.SingleToInt32Bits(tick));
                                if (callback && actions > 0) {
                                    events.Add("callback1");
                                    if (actions == 2 && fail != 2) events.Add("callback2");
                                }
                                events.Add("ctor:null");
                                events.Add("subscribe");
                                string? error = null;
                                if (fail == 1 && doRewind) {
                                    error = "ApplicationException";
                                    events = events.Take(seq ? 2 : 1).ToList();
                                } else if (fail == 2 && callback && actions > 0) {
                                    error = "ApplicationException";
                                    events = events.Take(events.IndexOf("callback1") + 1).ToList();
                                } else if (fail == 3) {
                                    error = "ApplicationException";
                                    events.RemoveAt(events.Count - 1);
                                }
                                Test($"wait:{before}:{rewind}:{actions}:{change}:{fail}", () => {
                                    Action<Controller> a = x => {
                                        if (!ReferenceEquals(x, c)) throw new InfrastructureFault("Wrong event arg");
                                        Tr.E("callback1");
                                    };
                                    if (actions == 2) a += x => Tr.E("callback2");
                                    if (actions > 0) {
                                        if (play) c.OnPlayStoppedEvent = a;
                                        else c.OnStopPlayingEvent = a;
                                    }
                                    if (seq && change == 1) Tr.Sequence = () => c._isPlaying = !before;
                                    if (change == 2) Tr.Rewind = () => { c._isPlaying = !after; c._tickTimer = 13; };
                                    Tr.Fail = fail == 1 ? "rewind:" + (after ? 1 : 0) + ":" + BitConverter.SingleToInt32Bits(tick) : fail == 2 ? "callback1" : fail == 3 ? "ctor:null" : "";
                                    return new object?[] { c, seq ? "run" : rewind };
                                }, v => ReferenceEquals(v, Tr.Made) && v is Wait w && ReferenceEquals(w.Subscribed, c) && c._isPlaying == (doRewind && change == 2 ? !after : after) && c._tickTimer == (doRewind && change == 2 ? 13 : tick), error, events.ToArray());
                                bool callbackChanged = doRewind && change == 2 && !(fail == 1);
                                rows[^1] = rows[^1] with { Pass = rows[^1].Pass && c._isPlaying == (callbackChanged ? !after : after) && c._tickTimer == (callbackChanged ? 13 : tick) };
                            }
            Test("null-controller", () => new object?[] { null, seq ? "run" : false }, _ => false, "NullReferenceException");
            if (seq) foreach (int state in new[] { 0, 2 }) {
                var c = new Controller { _clip = state == 0 ? null : new Clip { Alive = false }, _tickTimer = 9 };
                Test("dead-clip:" + state, () => new object?[] { c, "run" }, v => ReferenceEquals(v, Tr.Made), null, "rewind:0:" + BitConverter.SingleToInt32Bits(9f), "ctor:null", "subscribe");
            }
            if (seq) Test("clip-reloaded-null", () => { var c = new Controller { _clip = new Clip() }; Tr.Truth = () => c._clip = null; return new object?[] { c, "run" }; }, _ => false, "NullReferenceException");
            {
                var c = new Controller { _isPlaying = !play };
                Test("event-replaced-by-rewind", () => {
                    Action<Controller> old = x => Tr.E("old"), replacement = x => Tr.E("replacement");
                    if (play) c.OnPlayStoppedEvent = old; else c.OnStopPlayingEvent = old;
                    Tr.Rewind = () => { if (play) c.OnPlayStoppedEvent = replacement; else c.OnStopPlayingEvent = replacement; };
                    return new object?[] { c, seq ? "run" : true };
                }, v => ReferenceEquals(v, Tr.Made), null, "rewind:" + (play ? 1 : 0) + ":0", "replacement", "ctor:null", "subscribe");
            }
            return rows;
        }

        float[] vals = { -3f, -0f, 0f, 4f, float.NegativeInfinity, float.PositiveInfinity, BitConverter.Int32BitsToSingle(unchecked((int)0x7FC12345)) };
        if (token is 0x0600082C or 0x0600082D) {
            foreach (float a in vals)
                foreach (float b in vals) {
                    var value = new Color(a, b, -0f, BitConverter.Int32BitsToSingle(unchecked((int)0x7FC54321)));
                    var c = new Clip { _tint = token == 0x0600082C ? value : new Color(1, 2, 3, 4) };
                    Test($"color:{a}:{b}", () => token == 0x0600082C ? new object?[] { c } : new object?[] { c, value }, v => token == 0x0600082C ? v is Color col && Bits(col, value) : Bits(c._tint, value), null, token == 0x0600082D ? new[] { "props" } : Array.Empty<string>());
                }
            Test("null-this", () => token == 0x0600082C ? new object?[] { null } : new object?[] { null, new Color() }, _ => false, "NullReferenceException");
            if (token == 0x0600082D) {
                var c = new Clip();
                var v = new Color(5, 6, 7, 8);
                Tr.Reset();
                Tr.Fail = "props";
                var (_, e) = Invoke(dm, new object?[] { c, v });
                rows.Add(new("field-written-before-throw", e == "ApplicationException" && Bits(c._tint, v) && Tr.Events.SequenceEqual(new[] { "props" }), e, Tr.Events.ToArray(), new[] { "props" }));
            }
            return rows;
        }

        if (token is 0x060008A1 or 0x060008A3 or 0x06000820) {
            if (token == 0x060008A1) {
                for (int i = 0; i < 3; i++) Test("identity:" + i, () => Array.Empty<object?>(), v => v is SettingsData s && DefaultSettings(s));
            } else if (token == 0x060008A3) {
                var s = new SwfSettings();
                Test("reset", () => new object?[] { s }, _ => DefaultSettings(s.Settings));
                Test("null-this", () => new object?[] { null }, _ => false, "NullReferenceException");
            } else {
                var a = new Asset { Data = new byte[] { 9 }, Hash = "old", Atlas = new Texture() };
                byte[]? old = null;
                Test("asset-reset", () => new object?[] { a }, _ => a.Data != null && a.Data.Length == 0 && a.Hash == string.Empty && a.Atlas == null && DefaultSettings(a.Settings) && DefaultSettings(a.Overridden));
                old = a.Data;
                Test("new-array-each-reset", () => new object?[] { a }, _ => a.Data != null && !ReferenceEquals(a.Data, old) && DefaultSettings(a.Settings) && DefaultSettings(a.Overridden));
                Test("null-this", () => new object?[] { null }, _ => false, "NullReferenceException");
            }
            return rows;
        }

        if (token is 0x06000160 or 0x06000161 or 0x06000162 or 0x0600015D or 0x0600015E or 0x0600015F or 0x060000DF) {
            bool direct = token >= 0x06000160 || token == 0x060000DF;
            foreach (float x in vals)
                foreach (float y in vals) {
                    var pos = new Vec(x, y, BitConverter.Int32BitsToSingle(unchecked((int)0x7FC76543)));
                    var expected = token == 0x060000DF ? pos : new Vec(x + 57, y + (token is 0x06000162 or 0x0600015E ? 28 : 44), pos.z);
                    foreach (bool nil in new[] { false, true })
                        Test($"vector:{x}:{y}:{nil}", () => {
                            Tr.Position = pos;
                            return direct ? new object?[] { nil ? null : token == 0x060000DF ? new Range() : new BoardConfig(), pos } : new object?[] { nil ? null : new BoardConfig(), -7, 4 };
                        }, v => v is Vec got && Bits(got, expected) && Bits(pos, new Vec(x, y, pos.z)), null, direct ? Array.Empty<string>() : new[] { "grid:-7:4" });
                }
            if (!direct) Test("grid-throws", () => { Tr.Fail = "grid:1:2"; return new object?[] { new BoardConfig(), 1, 2 }; }, _ => false, "ApplicationException", "grid:1:2");
            return rows;
        }

        if (token is 0x0600088B or 0x0600088C or 0x0600088D or 0x0600088E or 0x0600088F) {
            foreach (string? group in new string?[] { null, "", "x", "different" })
                foreach (bool present in new[] { false, true })
                    foreach (bool nil in new[] { false, true })
                        foreach (bool arg in new[] { false, true }) {
                            var set = nil ? null : new HashSet<string>();
                            if (present) set?.Add(group!);
                            var s = new SwfManager { _groupPauses = set, _groupUnscales = set };
                            bool guard = (token == 0x0600088B || token == 0x0600088E) && string.IsNullOrEmpty(group);
                            bool query = token is 0x0600088C or 0x0600088D or 0x0600088F;
                            Test($"set:{group}:{present}:{nil}:{arg}", () => token == 0x0600088E ? new object?[] { s, group, arg } : new object?[] { s, group }, v => query ? Equals(v, token == 0x0600088D ? !present : present) : guard ? set == null || set.Contains(group!) == present : set!.Contains(group!) == (token == 0x0600088E && arg), nil && !guard ? "NullReferenceException" : null);
                        }
            Test("null-this-empty", () => token == 0x0600088E ? new object?[] { null, "", false } : new object?[] { null, "" }, _ => true, token == 0x0600088B || token == 0x0600088E ? null : "NullReferenceException");
            return rows;
        }

        if (token == 0x06000203) {
            foreach (bool nil in new[] { false, true })
                foreach (bool zombieNil in new[] { false, true }) {
                    var z = zombieNil ? null : new Zombie();
                    var pos = new Vec(3, -5, 7);
                    Test($"award:{nil}:{zombieNil}", () => {
                        ProjectManager.Result = nil ? null : new Project();
                        return new object?[] { new ProjectManager(), z, pos };
                    }, _ => Bits(AudioSource.LastPosition, pos) && ReferenceEquals(Project.End, z), nil ? "NullReferenceException" : null, nil ? new[] { "project:2:4" } : new[] { "project:2:4", "end" });
                }
            Test("create-throws", () => { Tr.Fail = "project:2:4"; return new object?[] { new ProjectManager(), null, new Vec() }; }, _ => false, "ApplicationException", "project:2:4");
            return rows;
        }

        if (token is 0x0600013C or 0x060000A8 or 0x060000AF or 0x060000BB or 0x060002C6 or 0x0600046C or 0x060006D3) {
            bool staticCall = token == 0x0600013C, self = token is 0x060000A8 or 0x060000AF, pitch = token == 0x060000BB, settle = token == 0x060006D3;
            foreach (bool clipNil in new[] { false, true })
                foreach (int mode in staticCall ? new[] { 0 } : new[] { 0, 1, 2 })
                    foreach (string fail in new[] { "", "position", settle ? "play" : "volume", settle ? "play" : pitch || staticCall ? "create4" : "create3" }) {
                        var clip = clipNil ? null : new AudioClip();
                        var p = new Vec(3, -5, 7);
                        var events = new List<string>();
                        if (pitch) events.Add("random:" + BitConverter.SingleToInt32Bits(.95f) + ":" + BitConverter.SingleToInt32Bits(1.1f));
                        if (!staticCall) {
                            if (!self) events.Add("camera");
                            if (!(mode == 1 && !self)) {
                                events.Add("transform");
                                if (mode != 2) events.Add("position");
                            }
                        }
                        if (staticCall || mode == 0 || mode == 1 && self) {
                            if (!settle) events.Add("volume");
                            events.Add(settle ? "play" : pitch || staticCall ? "create4" : "create3");
                        }
                        string? error = !staticCall && ((mode == 1 && !self) || mode == 2) ? "NullReferenceException" : null;
                        int fi = events.IndexOf(fail);
                        if (fail != "" && fi >= 0) {
                            events = events.Take(fi + 1).ToList();
                            error = "ApplicationException";
                        }
                        Test($"audio:{clipNil}:{mode}:{fail}", () => {
                            Camera.Main = mode == 1 ? null : new Camera();
                            Component.Transform = mode == 2 ? null : new Transform();
                            Global.gLawnApp = new Lawn { audioVolume = .625f };
                            Tr.Fail = fail;
                            Tr.Position = p;
                            return staticCall ? new object?[] { clip, p } : new object?[] { token == 0x060002C6 ? (object)new Board() : token == 0x0600046C ? new Zombie() : new Component(), clip };
                        }, v => ReferenceEquals(AudioSource.LastClip, clip) && Bits(AudioSource.LastPosition, p) && AudioSource.LastVolume == (settle ? .625f : .375f) && (!(pitch || staticCall) || AudioSource.LastPitch == (pitch ? 1.025f : 1)), error, events.ToArray());
                    }
            if (settle) Test("null-lawn", () => { Camera.Main = new(); Component.Transform = new(); Global.gLawnApp = null; return new object?[] { new Component(), null }; }, _ => false, "NullReferenceException", "camera", "transform", "position");
            return rows;
        }

        // --- 17 Native30 methods ---
        // 1. 0x060000C1: ProjectAnimationEvent::PlayWinAudio()
        if (token == 0x060000C1) {
            foreach (bool clipNil in new[] { false, true }) {
                var clip = clipNil ? null : new AudioClip();
                var p = new Vec(3, -5, 7);
                Test($"playwin:{clipNil}", () => {
                    ResourceManager.boardClips = Enumerable.Range(0, 10).Select(_ => clip!).ToList();
                    Camera.Main = new Camera();
                    Component.Transform = new Transform();
                    Global.gLawnApp = new Lawn { audioVolume = 0.8f };
                    Tr.Position = p;
                    return new object?[] { new ProjectAnimationEvent() };
                }, _ => ReferenceEquals(AudioSource.LastClip, clip) && Bits(AudioSource.LastPosition, p) && AudioSource.LastVolume == 0.8f, null, "camera", "transform", "position", "play");
            }
            return rows;
        }

        // 2. 0x060001C7: MouseManager::AudioPlay(int)
        if (token == 0x060001C7) {
            foreach (int id in new[] { 0, 1 }) {
                var clip = new AudioClip();
                var p = new Vec(4, 5, 6);
                Test($"audioplay:{id}", () => {
                    ResourceManager.mouseClips = Enumerable.Range(0, 10).Select(_ => clip).ToList();
                    Camera.Main = new Camera();
                    Component.Transform = new Transform();
                    Tr.Position = p;
                    return new object?[] { new MouseManager(), id };
                }, _ => ReferenceEquals(AudioSource.LastClip, clip) && Bits(AudioSource.LastPosition, p) && AudioSource.LastVolume == .375f, null, "camera", "transform", "position", "volume", "create4");
            }
            return rows;
        }

        // 3. 0x060002B4: Board::ButtonDown_Menu()
        if (token == 0x060002B4) {
            var clip = new AudioClip();
            var p = new Vec(1, 2, 3);
            Test("buttondown_menu", () => {
                ResourceManager.boardClips = Enumerable.Range(0, 15).Select(_ => clip).ToList();
                Camera.Main = new Camera();
                Component.Transform = new Transform();
                Tr.Position = p;
                return new object?[] { new Board() };
            }, _ => ReferenceEquals(AudioSource.LastClip, clip) && Bits(AudioSource.LastPosition, p) && AudioSource.LastVolume == .375f, null, "camera", "transform", "position", "volume", "create3", "pause:True", "pop_menu");
            return rows;
        }

        // 4. 0x06000480: Zombie::TeleportTo(Vector3, float, bool)
        if (token == 0x06000480) {
            foreach (var p in new[] { new Vec(10, 20, 30), new Vec(-5, 0, 15) }) {
                var z = new Zombie();
                Test($"teleport:{p.x}:{p.y}", () => {
                    Component.Transform = new Transform();
                    return new object?[] { z, p, 1.5f, true };
                }, _ => Bits(Tr.Position, p) && z.fX == p.x && z.fY == p.y, null, "transform", "set_pos", "prev_pos");
            }
            return rows;
        }

        // 5. 0x0600001F: Admin_system2_boardEdior::Return()
        if (token == 0x0600001F) {
            Test("return", () => {
                Camera.Main = new Camera();
                Component.Transform = new Transform();
                Component.GameObject = new GameObject();
                Tr.Position = new Vec(1, 2, 3);
                return new object?[] { new Admin_system2_boardEdior() };
            }, _ => Bits(Tr.Position, new Vec(1, 2, 3)), null, "gameobject", "active:False", "camera", "transform", "camera", "transform", "position", "set_pos");
            return rows;
        }

        // 6. 0x06000060: Path_enemyEditor::LoadData()
        if (token == 0x06000060) {
            Test("loaddata", () => {
                Component.Transform = new Transform();
                var p = new Path_enemyEditor {
                    enemyEditor = new Enemy_boardEditor {
                        boardEdior = new Admin_system2_boardEdior {
                            boardConfig = new BoardConfig()
                        }
                    },
                    enemyPath = new EnemyPath { gridX = 3, gridY = 4 }
                };
                return new object?[] { p };
            }, _ => Bits(Tr.Position, new Vec(30, 40, 0)), null, "transform", "grid_center:3:4", "set_pos");
            return rows;
        }

        // 7. 0x060003C9: Project::Rotating()
        if (token == 0x060003C9) {
            foreach (int pt in new[] { 0, 1, 2 }) {
                Test($"rotating:{pt}", () => {
                    Component.Transform = new Transform();
                    return new object?[] { new Project { projectType = pt } };
                }, _ => true, null, pt == 0 ? new[] {
                    "transform",
                    $"rotate:{BitConverter.SingleToInt32Bits(0f)}:{BitConverter.SingleToInt32Bits(0f)}:{BitConverter.SingleToInt32Bits(1f)}:{BitConverter.SingleToInt32Bits(-18.0f * 0.02f)}:0"
                } : Array.Empty<string>());
            }
            return rows;
        }

        // 8. 0x0600026A: SunManager::SunFall()
        if (token == 0x0600026A) {
            foreach (int count in new[] { 0, 5, 10 }) {
                ProjectManager.Result = new Project();
                var s = new SunManager {
                    board = new Board { projectManager = new ProjectManager() },
                    sunFallCount = count
                };
                Test($"sunfall:{count}", () => new object?[] { s }, _ => s.sunFallCount == count + 1, null, "project:0:0", "nature_sun");
            }
            return rows;
        }

        // 9. 0x0600026B: SunManager::SunPointTextUpdate()
        if (token == 0x0600026B) {
            foreach (int pts in new[] { 0, 50, 150, 9999 }) {
                var txt = new TMP_Text();
                var s = new SunManager { sunPointText = txt, sunPoint = pts };
                Test($"sunpoint:{pts}", () => new object?[] { s }, _ => txt.text == pts.ToString(), null, $"text:{pts}");
            }
            Test("null-this", () => new object?[] { null }, _ => false, "NullReferenceException");
            return rows;
        }

        // 10. 0x06000448: Zombie::GetElementPoint(ElementType)
        if (token == 0x06000448) {
            foreach (bool has in new[] { false, true })
                foreach (int elemType in new[] { 0, 1, 2 }) {
                    var z = new Zombie { elementManager = new ElementManager() };
                    ElementManager.Element = has ? new Element { point = 3.5f } : null;
                    Test($"element_pt:{has}:{elemType}", () => new object?[] { z, elemType }, v => Equals(v, has ? 3.5f : 0.0f), null, $"get_element:{elemType}");
                }
            return rows;
        }

        // 11. 0x06000488: Zombie::TryShooting()
        if (token == 0x06000488) {
            foreach (bool hasAnim in new[] { false, true }) {
                var z = new Zombie { animator = hasAnim ? new Animator() : null };
                Test($"tryshooting:{hasAnim}", () => new object?[] { z }, v => Equals(v, true), hasAnim ? null : "NullReferenceException", hasAnim ? new[] { "trigger:ShootingTrigger" } : Array.Empty<string>());
            }
            return rows;
        }

        // 12. 0x060006E9: WarehouseUIController::CheckSupplies(Supplies)
        if (token == 0x060006E9) {
            foreach (bool hasSup in new[] { false, true }) {
                var page = new Warehouse_SuppliesInfoPage();
                var w = new WarehouseUIController { suppliesInfoPage = page };
                var s = hasSup ? new Supplies { ID = 42 } : null;
                Test($"checksupplies:{hasSup}", () => new object?[] { w, s }, _ => (hasSup ? page.supplies == s && page.suppliesInfo?.id == 42 : page.supplies == null && page.suppliesInfo == null), null, hasSup ? new[] { "get_sup_info:42", "set_info" } : new[] { "set_info" });
            }
            return rows;
        }

        // 13. 0x06000675: Popup::PopupPopup(int)
        if (token == 0x06000675) {
            foreach (bool hasPrefab in new[] { false, true }) {
                ResourceManager.prefab_Popup = hasPrefab ? new Popup() : null;
                Test($"popuppopup1:{hasPrefab}", () => new object?[] { 7 }, v => v is Popup p && p.P == 7, hasPrefab ? null : "NullReferenceException", hasPrefab ? new[] { "instantiate" } : Array.Empty<string>());
            }
            return rows;
        }

        // 14. 0x06000673: Popup::PopupPopup(int, int)
        if (token == 0x06000673) {
            foreach (bool hasPrefab in new[] { false, true }) {
                ResourceManager.prefab_Popup = hasPrefab ? new Popup() : null;
                Test($"popuppopup2:{hasPrefab}", () => new object?[] { 10, 20 }, v => v is Popup p && p.P == 10 && p.info0_int == 20, hasPrefab ? null : "NullReferenceException", hasPrefab ? new[] { "instantiate" } : Array.Empty<string>());
            }
            return rows;
        }

        // 15. 0x06000704: Window_Q::PopupNewWindow(int, Transform, int)
        if (token == 0x06000704) {
            var tr = new Transform();
            Test("popupnewwindow", () => new object?[] { 3, tr, 99 }, v => v is Window_Q w && w.info0_int == 99, null, "popup_q:3");
            return rows;
        }

        // 16. 0x060008F2: SwfUtils::UnpackUV(uint, ref float, ref float)
        if (token == 0x060008F2) {
            uint[] packs = { 0u, 0xFFFFFFFFu, (16383u << 16) | 32767u, (65535u << 16), 65535u, (12345u << 16) | 54321u };
            foreach (var pack in packs) {
                float expectedU = (float)(pack >> 16) / 65535.0f;
                float expectedV = (float)(pack & 0xFFFF) / 65535.0f;
                object?[] invokeArgs = new object?[] { pack, 0.0f, 0.0f };
                Test($"unpackuv:{pack:X8}", () => invokeArgs, _ => {
                    float gotU = (float)invokeArgs[1]!;
                    float gotV = (float)invokeArgs[2]!;
                    return Math.Abs(gotU - expectedU) < 1e-6f && Math.Abs(gotV - expectedV) < 1e-6f;
                });
            }
            return rows;
        }

        // 17. 0x06000401: Prop::DropDown()
        if (token == 0x06000401) {
            // Case 1: Managed reference null
            var pNull = new Prop {
                prop = new GameObject(),
                originalPosition = new Vec(11, 22, 33),
                propState = 0,
                propImagex = null
            };
            Test("dropdown:null", () => new object?[] { pNull }, _ => pNull.propState == 1 && Bits(pNull.prop.transform.localPosition, new Vec(11, 22, 33)), null, "set_local_pos", "prop_bank");

            // Case 2: Live Unity object (Alive == true)
            var imgLive = new GameObject { Alive = true };
            var pLive = new Prop {
                prop = new GameObject(),
                originalPosition = new Vec(11, 22, 33),
                propState = 0,
                propImagex = imgLive
            };
            Test("dropdown:live", () => new object?[] { pLive }, _ => pLive.propState == 1 && Bits(pLive.prop.transform.localPosition, new Vec(11, 22, 33)) && !imgLive.Alive, null, "set_local_pos", "destroy", "prop_bank");

            // Case 3: Destroyed Unity object (Alive == false, managed reference non-null)
            var imgDead = new GameObject { Alive = false };
            var pDead = new Prop {
                prop = new GameObject(),
                originalPosition = new Vec(11, 22, 33),
                propState = 0,
                propImagex = imgDead
            };
            Test("dropdown:destroyed", () => new object?[] { pDead }, _ => pDead.propState == 1 && Bits(pDead.prop.transform.localPosition, new Vec(11, 22, 33)) && !imgDead.Alive, null, "set_local_pos", "prop_bank");

            // Case 4: Null this
            Test("dropdown:null-this", () => new object?[] { null }, _ => false, "NullReferenceException");

            return rows;
        }

        throw new InfrastructureFault("No oracle " + token.ToString("X8"));
    }

    record Mutation(string Token, string Name, string Status, bool Detected, int Failed, int ClrRejected, string? ToolError);

    static Mutation Eval(MethodDefinition m, string name) {
        try {
            Changes = 0;
            var rows = Check(m.MetadataToken.ToInt32(), Emit(m, name));
            int clr = rows.Count(r => r.Error is "InvalidProgramException" or "VerificationException"), failed = rows.Count(r => !r.Pass);
            return new($"0x{m.MetadataToken.ToInt32():X8}", name, clr > 0 ? "CLR_REJECTED" : failed > 0 ? "BEHAVIOR_MISMATCH" : "MISSED", failed > 0, failed, clr, null);
        } catch (Exception e) {
            return new($"0x{m.MetadataToken.ToInt32():X8}", name, "TOOL_ERROR", false, 0, 0, e.ToString());
        }
    }

    static int Main(string[] args) {
        try {
            using var a = AssemblyDefinition.ReadAssembly(args[0]);
            Assembly = a;
            var positives = new List<object>();
            var mutants = new List<Mutation>();
            bool fault = args.Length > 2;

            foreach (var (token, name) in Contracts) {
                var m = (MethodDefinition)a.MainModule.LookupToken(token);
                if (fault) {
                    InvokeFault = args[2] == "fault_invoke";
                    mutants.Add(Eval(m, args[2] == "fault_emit" ? "fault_emit" : "none"));
                    break;
                }
                var rows = Check(token, Emit(m));
                positives.Add(new { token = $"0x{token:X8}", signature = name, cases = rows });

                // Choose mutation
                string mutation = "drop_write";
                if (token == 0x0600019B) mutation = "wrong_dispatch";
                else if (token is 0x060008AA or 0x060008AC or 0x060008AE or 0x060008B6 or 0x060008B7) mutation = "drop_callback";
                else if (token == 0x0600082C) mutation = "default_color";
                else if (token == 0x060008A1) mutation = "wrong_constant";
                else if (token is 0x0600088B or 0x0600088C or 0x0600088D or 0x0600088E or 0x0600088F) mutation = "wrong_set";
                else if (token == 0x060000DF) mutation = "identity_zero";
                else if (token is 0x0600013C) mutation = "wrong_offset";
                else if (token is 0x060000A8 or 0x060000AF or 0x060000BB or 0x060002C6 or 0x0600046C or 0x060006D3 or 0x06000203) mutation = "wrong_vector";
                else if (token is 0x060000C1 or 0x060001C7 or 0x060002B4 or 0x06000480 or 0x0600001F or 0x06000060) mutation = "wrong_vector";
                else if (token == 0x060003C9) mutation = "wrong_offset";
                else if (token == 0x0600026A) mutation = "wrong_sub";
                else if (token == 0x0600026B) mutation = "drop_set_text";
                else if (token == 0x06000448) mutation = "wrong_constant";
                else if (token == 0x06000488) mutation = "drop_trigger";
                else if (token == 0x060006E9) mutation = "drop_set_info";
                else if (token == 0x060008F2) mutation = "wrong_shift";
                else if (token == 0x06000401) mutation = "plain_null_check";

                mutants.Add(Eval(m, mutation));
                if (token is 0x060008AA or 0x060008AC or 0x060008AE or 0x060008B6 or 0x060008B7) {
                    mutants.Add(Eval(m, "return_subscribe"));
                    mutants.Add(Eval(m, "post_state"));
                }
                if (token == 0x060003C9) {
                    mutants.Add(Eval(m, "wrong_axis"));
                }
                if (token == 0x06000401) {
                    mutants.Add(Eval(m, "drop_write"));
                }
            }

            if (!fault) {
                foreach (int t in new[] { 0x0600019B, 0x0600088C, 0x060008F2 }) {
                    mutants.Add(Eval((MethodDefinition)a.MainModule.LookupToken(t), "illegal_stack"));
                }
            }

            var json = JsonSerializer.SerializeToElement(positives);
            int count = 0, bad = 0;
            foreach (var row in json.EnumerateArray()) {
                foreach (var c in row.GetProperty("cases").EnumerateArray()) {
                    count++;
                    if (!c.GetProperty("Pass").GetBoolean()) bad++;
                }
            }

            bool gate = !fault && bad == 0 && mutants.All(m => m.Detected && m.ToolError == null);
            var report = new {
                runtime = Environment.Version.ToString(),
                methods = positives.Count,
                positive_cases = count,
                positive_failures = bad,
                negative_controls = mutants.Count,
                tool_errors = mutants.Count(m => m.ToolError != null),
                gate_pass = gate,
                scope = "Actual emitted candidate outer CIL, native-pinned independent expected values; engine/helper doubles; real CLR delegates and HashSet<string>; Settings Reset executes candidate identity body; not Unity/device acceptance",
                positives,
                mutations = mutants
            };
            File.WriteAllText(args[1], JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine(JsonSerializer.Serialize(new {
                report.methods,
                report.positive_cases,
                report.positive_failures,
                report.negative_controls,
                report.tool_errors,
                report.gate_pass
            }));
            return gate ? 0 : 2;
        } catch (Exception e) {
            Console.Error.WriteLine(e);
            return 2;
        }
    }
}
