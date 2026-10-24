#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class GodsPVZStartupAudit
{
    private const string Plugin = "Assets/Plugins/GodsPVZRuntime1.dll";
    private const string PluginGuid = "60b84a767c46ced9aacc93f38da4d2f3";
    private const string ProbeActive = "GodsPVZ.StartupProbe.Active";
    private const string ProbeStarted = "GodsPVZ.StartupProbe.StartTicks";
    private static readonly List<string> ProbeErrors = new List<string>();
    private static readonly List<string> ProbeAudioLimits = new List<string>();
    [Serializable] public class ShaderBinding { public string shader, old_guid, new_guid, target; public int references; }
    [Serializable] public class FontCheck { public string path; public int atlas_count; public bool textures_valid, clear_on_build_disabled, original_empty_dynamic_atlas; }
    [Serializable] public class AtlasSampler { public int m_FilterMode, m_Aniso, m_WrapU, m_WrapV, m_WrapW; public float m_MipBias; }
    [Serializable] public class EmptyAtlasSource { public int width, height, texture_format, image_data_bytes, stream_size, mip_count; public bool is_readable; public AtlasSampler texture_settings; }
    [Serializable] public class EmptyAtlasPlan { public string font_path, missing_atlas_guid, texture_name; public EmptyAtlasSource original_atlas; }
    [Serializable] public class EmptyAtlasEvidence { public EmptyAtlasPlan[] fonts; }
    [Serializable] public class EmptyAtlasRestoration { public string font_path, old_guid, new_guid; public int width, height; }
    [Serializable] public class Report
    {
        public string scope = "REAL_UNITY_SCENE_SCRIPT_AND_OFFICIAL_SHADER_BINDING_ONLY";
        public bool pass, gameplay_equivalence_proven = false, device_execution_performed = false;
        public int plugin_scripts, resolved_plugin_scripts, validated_game_script_references, prefab_assets, missing_components;
        public bool game_start_component, game_start_prefab, board_start_component, core_blit_keyword;
        public bool splash_unity_logo_canonical, splash_game_logo_preserved;
        public List<string> errors = new List<string>();
        public List<string> unresolved_custom_dummy_shaders = new List<string>();
        public List<ShaderBinding> official_shader_bindings = new List<ShaderBinding>();
        public List<FontCheck> fonts = new List<FontCheck>();
        public List<EmptyAtlasRestoration> restored_original_empty_atlases = new List<EmptyAtlasRestoration>();
    }

    [Serializable] public class PlayProbeReport
    {
        public string scope = "EDITOR_PLAYMODE_ACTUAL_STARTUP_CIL_DISPATCH_ONLY";
        public bool pass, device_execution = false, pixel_equivalence = false;
        public int frames, active_canvases, main_ui_components;
        public string active_scene, audio_scope = "NO_PHYSICAL_AUDIO_DEVICE";
        public List<string> errors = new List<string>();
        public List<string> environment_audio_limits = new List<string>();
    }

    // Called in a separate Editor invocation after the asset audit. This uses
    // the normal scene Start/Update dispatch and never supplies fake game data,
    // invokes GameStart manually, or suppresses gameplay exceptions.
    public static void BeginPlayProbe()
    {
        var folder = Environment.GetEnvironmentVariable("GODSPVZ_STARTUP_EVIDENCE");
        if (String.IsNullOrEmpty(folder)) throw new InvalidOperationException("Explicit startup evidence directory required");
        if (SessionState.GetBool(ProbeActive, false)) throw new InvalidOperationException("A startup probe is already active");
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity", OpenSceneMode.Single);
        SessionState.SetString(ProbeStarted, DateTime.UtcNow.Ticks.ToString());
        SessionState.SetBool(ProbeActive, true);
        AttachProbe();
        EditorApplication.isPlaying = true;
    }

    [InitializeOnLoadMethod]
    private static void ResumeProbeAfterDomainReload()
    {
        if (SessionState.GetBool(ProbeActive, false)) AttachProbe();
    }

    private static void AttachProbe()
    {
        EditorApplication.update -= ProbeTick;
        EditorApplication.update += ProbeTick;
        Application.logMessageReceived -= RecordProbeLog;
        Application.logMessageReceived += RecordProbeLog;
    }

    private static void RecordProbeLog(string message, string stack, LogType type)
    {
        if (!EditorApplication.isPlaying || !(type == LogType.Error || type == LogType.Exception || type == LogType.Assert)) return;
        if (message.StartsWith("FMOD failed to initialize the output device", StringComparison.Ordinal))
        {
            if (ProbeAudioLimits.Count < 10) ProbeAudioLimits.Add(message);
            return;
        }
        if (ProbeErrors.Count < 50) ProbeErrors.Add(message + "\n" + stack);
    }

    private static void ProbeTick()
    {
        if (!SessionState.GetBool(ProbeActive, false)) return;
        long ticks = Int64.Parse(SessionState.GetString(ProbeStarted, "0"));
        double elapsed = (DateTime.UtcNow.Ticks - ticks) / (double)TimeSpan.TicksPerSecond;
        if (elapsed < 20.0) return;
        if (EditorApplication.isPlaying && Time.frameCount < 3 && elapsed < 90.0) return;
        var report = new PlayProbeReport();
        report.errors.AddRange(ProbeErrors); report.environment_audio_limits.AddRange(ProbeAudioLimits);
        if (EditorApplication.isPlaying)
        {
            report.frames = Time.frameCount;
            report.active_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            report.active_canvases = UnityEngine.Object.FindObjectsOfType<Canvas>().Count(c => c.isActiveAndEnabled);
            report.main_ui_components = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().Count(c => c != null && c.GetType().FullName == "MainUIController");
        }
        if (!EditorApplication.isPlaying) report.errors.Add("Editor did not enter Play Mode");
        if (report.frames < 3) report.errors.Add("Startup did not advance enough player frames");
        if (report.active_scene != "MainMenu") report.errors.Add("Startup did not retain the MainMenu scene");
        if (report.main_ui_components < 1 || report.active_canvases < 1) report.errors.Add("Actual startup did not create the main UI and an active Canvas");
        report.pass = report.errors.Count == 0;
        string folder = Environment.GetEnvironmentVariable("GODSPVZ_STARTUP_EVIDENCE");
        File.WriteAllText(Path.Combine(folder, "startup-play-probe.json"), JsonUtility.ToJson(report, true));
        SessionState.SetBool(ProbeActive, false);
        EditorApplication.update -= ProbeTick;
        Application.logMessageReceived -= RecordProbeLog;
        Debug.Log("GODSPVZ_STARTUP_PLAY_PROBE=" + (report.pass ? "PASS" : "FAIL"));
        EditorApplication.Exit(report.pass ? 0 : 75);
    }

    public static void Run()
    {
        var report = new Report();
        var folder = Environment.GetEnvironmentVariable("GODSPVZ_STARTUP_EVIDENCE");
        if (String.IsNullOrEmpty(folder)) throw new InvalidOperationException("Explicit startup evidence directory required");
        Directory.CreateDirectory(folder);
        try
        {
            if (File.Exists("Assets/Plugins/Assembly-CSharp.dll")) throw new InvalidOperationException("Old mismatched plugin path still exists");
            if (AssetDatabase.AssetPathToGUID(Plugin) != PluginGuid) throw new InvalidOperationException("Plugin GUID changed");
            var runtime = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "GodsPVZRuntime1");
            var scripts = AssetDatabase.LoadAllAssetsAtPath(Plugin).OfType<MonoScript>().ToArray();
            var scriptIds = new HashSet<long>();
            report.plugin_scripts = scripts.Length;
            foreach (var script in scripts)
            {
                var type = script.GetClass();
                if (type == null) { report.errors.Add("Unresolved plugin MonoScript: " + script.name); continue; }
                if (type.Assembly != runtime) { report.errors.Add("Wrong script assembly: " + type.FullName); continue; }
                string guid; long id;
                if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(script, out guid, out id) || guid != PluginGuid)
                    throw new InvalidOperationException("Unbound plugin local identifier: " + script.name);
                scriptIds.Add(id); report.resolved_plugin_scripts++;
            }
            if (scripts.Length < 100) report.errors.Add("Incomplete imported plugin script inventory");
            foreach (var path in SerializedFiles())
            {
                foreach (Match m in Regex.Matches(File.ReadAllText(path), @"m_Script:\s*\{fileID:\s*(-?\d+),\s*guid:\s*" + PluginGuid + @",\s*type:\s*3\}"))
                {
                    long id = Int64.Parse(m.Groups[1].Value);
                    if (!scriptIds.Contains(id)) report.errors.Add("Unknown game script local ID " + id + " in " + path);
                    report.validated_game_script_references++;
                }
            }
            RestoreCanonicalSplashLogo(report);
            ImportOfficialTmpResources();
            RebindOfficialShaders(report);
            RestoreOriginalEmptyFontAtlases(report);
            PreserveFontAtlases(report);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            CheckScene("Assets/Scenes/MainMenu.unity", "GameStart", runtime.GetType("GameStart", true), report);
            CheckScene("Assets/Scenes/Board.unity", "BoardStart", runtime.GetType("BoardStart", true), report);
            foreach (var guid in AssetDatabase.FindAssets("t:Prefab", new [] { "Assets" }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null) { report.errors.Add("Unreadable prefab: " + path); continue; }
                report.prefab_assets++; CheckMissing(go, path, report);
            }
            var blit = report.official_shader_bindings.FirstOrDefault(x => x.shader == "Hidden/Universal/CoreBlit");
            if (blit == null) report.errors.Add("Original CoreBlit dummy was not explicitly rebound");
            else
            {
                string code = File.ReadAllText(blit.target);
                report.core_blit_keyword = code.Contains("BLIT_DECODE_HDR") && !code.Contains("DummyShaderTextExporter");
                if (!report.core_blit_keyword) report.errors.Add("Official CoreBlit keyword/source check failed");
            }
            report.pass = report.errors.Count == 0 && report.missing_components == 0 && report.game_start_component && report.game_start_prefab && report.board_start_component;
        }
        catch (Exception error) { report.errors.Add(error.ToString()); report.pass = false; }
        File.WriteAllText(Path.Combine(folder, "startup-asset-audit.json"), JsonUtility.ToJson(report, true));
        Debug.Log("GODSPVZ_STARTUP_ASSET_AUDIT=" + (report.pass ? "PASS" : "FAIL") + " scripts=" + report.resolved_plugin_scripts + "/" + report.plugin_scripts + " missing=" + report.missing_components);
        if (!report.pass) throw new InvalidOperationException("Startup asset gate failed: " + String.Join("; ", report.errors.ToArray()));
    }

    private static IEnumerable<string> SerializedFiles()
    {
        var allowed = new HashSet<string>(new [] { ".unity", ".prefab", ".asset", ".mat", ".shadervariants" });
        return Directory.EnumerateFiles("Assets", "*", SearchOption.AllDirectories).Where(p => allowed.Contains(Path.GetExtension(p).ToLowerInvariant()));
    }

    private static void RestoreCanonicalSplashLogo(Report report)
    {
        var logos = PlayerSettings.SplashScreen.logos;
        if (logos.Length != 2 || logos[0].logo == null || logos[0].logo.texture == null || logos[0].logo.texture.name != "Splash Screen Unity Logo")
            throw new InvalidOperationException("Unexpected recovered splash logo configuration");
        var gameLogo = logos[1].logo;
        float gameDuration = logos[1].duration;
        // The original PC first entry points to Unity's built-in logo itself.
        // AssetRipper exported a separate texture/sprite copy, which lost that
        // identity. Restore the official token while retaining the game logo.
        logos[0] = PlayerSettings.SplashScreenLogo.CreateWithUnityLogo(logos[0].duration);
        PlayerSettings.SplashScreen.show = true;
        PlayerSettings.SplashScreen.showUnityLogo = true;
        PlayerSettings.SplashScreen.drawMode = PlayerSettings.SplashScreen.DrawMode.AllSequential;
        PlayerSettings.SplashScreen.logos = logos;
        var actual = PlayerSettings.SplashScreen.logos;
        report.splash_unity_logo_canonical = actual[0].logo == PlayerSettings.SplashScreenLogo.CreateWithUnityLogo(actual[0].duration).logo;
        report.splash_game_logo_preserved = actual[1].logo == gameLogo && actual[1].duration == gameDuration;
        if (!report.splash_unity_logo_canonical || !report.splash_game_logo_preserved) report.errors.Add("Splash logo identity/preservation failed");
    }

    private static void ImportOfficialTmpResources()
    {
        string path = "Assets/Recovery/TMP-Official-Shaders";
        if (!Directory.Exists(path) || Directory.GetFiles(path, "*.shader", SearchOption.AllDirectories).Length < 7)
            throw new InvalidOperationException("Pinned TMP shader sources were not restored before Editor import");
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
    }

    private static void RebindOfficialShaders(Report report)
    {
        var declarations = new Regex("Shader\\s+\"([^\"]+)\"");
        var official = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        foreach (string path in AssetDatabase.GetAllAssetPaths())
        {
            if (!(path.StartsWith("Packages/", StringComparison.Ordinal) || path.StartsWith("Assets/Recovery/TMP-Official-Shaders/", StringComparison.Ordinal)) || !path.EndsWith(".shader", StringComparison.OrdinalIgnoreCase) || !File.Exists(path)) continue;
            string text = File.ReadAllText(path); var match = declarations.Match(text);
            if (!match.Success || text.Contains("DummyShaderTextExporter")) continue;
            string name = match.Groups[1].Value;
            if (!official.ContainsKey(name)) official.Add(name, new List<string>());
            official[name].Add(path);
        }
        foreach (string path in Directory.EnumerateFiles("Assets/Shader", "*.shader"))
        {
            string source = File.ReadAllText(path); if (!source.Contains("DummyShaderTextExporter")) continue;
            var match = declarations.Match(source); if (!match.Success) throw new InvalidOperationException("Shader declaration missing: " + path);
            string name = match.Groups[1].Value; List<string> options;
            if (!official.TryGetValue(name, out options)) { report.unresolved_custom_dummy_shaders.Add(name); continue; }
            if (options.Count != 1) throw new InvalidOperationException("Ambiguous official shader: " + name);
            string target = options[0];
            var binding = new ShaderBinding { shader=name,old_guid=AssetDatabase.AssetPathToGUID(path),new_guid=AssetDatabase.AssetPathToGUID(target),target=target };
            if (String.IsNullOrEmpty(binding.old_guid) || String.IsNullOrEmpty(binding.new_guid)) throw new InvalidOperationException("Shader GUID missing: " + name);
            report.official_shader_bindings.Add(binding);
        }
        foreach (string path in SerializedFiles())
        {
            string text = File.ReadAllText(path), original = text;
            foreach (var binding in report.official_shader_bindings)
            {
                var regex = new Regex(@"(\{fileID:\s*4800000,\s*guid:\s*)" + binding.old_guid + @"(,\s*type:\s*3\})");
                binding.references += regex.Matches(text).Count;
                text = regex.Replace(text, m => m.Groups[1].Value + binding.new_guid + m.Groups[2].Value);
            }
            if (text != original) File.WriteAllText(path, text);
        }
    }

    private static void RestoreOriginalEmptyFontAtlases(Report report)
    {
        string evidencePath = "Recovery/empty-dynamic-font-atlas-evidence.json";
        var plan = JsonUtility.FromJson<EmptyAtlasEvidence>(File.ReadAllText(evidencePath));
        if (plan.fonts == null || plan.fonts.Length != 6) throw new InvalidOperationException("Six original empty-atlas proofs required");
        string directory = "Assets/Recovery/OriginalEmptyDynamicAtlases";
        Directory.CreateDirectory(directory); AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        foreach (var row in plan.fonts)
        {
            var source = row.original_atlas;
            if (source == null || source.width != 0 || source.height != 0 || source.texture_format != 1 || source.image_data_bytes != 0 || source.stream_size != 0)
                throw new InvalidOperationException("Atlas proof does not describe an empty Alpha8 texture: " + row.font_path);
            if (!String.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(row.missing_atlas_guid)))
                throw new InvalidOperationException("The expected missing atlas GUID is already bound: " + row.font_path);
            if (!File.ReadAllText(row.font_path).Contains(row.missing_atlas_guid))
                throw new InvalidOperationException("Original missing-atlas pointer changed: " + row.font_path);
            var font = AssetDatabase.LoadMainAssetAtPath(row.font_path);
            if (font == null) throw new InvalidOperationException("Original empty-atlas font missing: " + row.font_path);
            var serialized = new SerializedObject(font);
            if (serialized.FindProperty("m_AtlasPopulationMode").intValue != 1 || serialized.FindProperty("m_GlyphTable").arraySize != 0 || serialized.FindProperty("m_CharacterTable").arraySize != 0)
                throw new InvalidOperationException("Refuse to replace nonempty font glyph data: " + row.font_path);
            var atlas = new Texture2D(0, 0, TextureFormat.Alpha8, false);
            atlas.name = row.texture_name;
            if (source.texture_settings == null || source.mip_count != 1 || !source.is_readable) throw new InvalidOperationException("Empty-atlas sampler/readability proof missing");
            atlas.filterMode = (FilterMode)source.texture_settings.m_FilterMode;
            atlas.anisoLevel = source.texture_settings.m_Aniso;
            atlas.mipMapBias = source.texture_settings.m_MipBias;
            atlas.wrapModeU = (TextureWrapMode)source.texture_settings.m_WrapU;
            atlas.wrapModeV = (TextureWrapMode)source.texture_settings.m_WrapV;
            atlas.wrapModeW = (TextureWrapMode)source.texture_settings.m_WrapW;
            string path = directory + "/" + Path.GetFileNameWithoutExtension(row.font_path) + " Atlas.asset";
            if (File.Exists(path)) throw new InvalidOperationException("Do not overwrite prior atlas restoration");
            AssetDatabase.CreateAsset(atlas, path);
            string guid; long id;
            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(atlas, out guid, out id) || id != 2800000)
                throw new InvalidOperationException("Unexpected native Texture2D local identifier");
            var atlasReferences = serialized.FindProperty("m_AtlasTextures");
            if (atlasReferences == null || atlasReferences.arraySize != 1) throw new InvalidOperationException("Expected original single-atlas field");
            atlasReferences.GetArrayElementAtIndex(0).objectReferenceValue = atlas;
            serialized.ApplyModifiedPropertiesWithoutUndo(); EditorUtility.SetDirty(font);
            var materialProperty = serialized.FindProperty("material");
            var material = materialProperty == null ? null : materialProperty.objectReferenceValue as Material;
            if (material != null && material.HasProperty("_MainTex") && material.GetTexture("_MainTex") == null)
            {
                string materialPath = AssetDatabase.GetAssetPath(material);
                if (!String.IsNullOrEmpty(materialPath) && File.ReadAllText(materialPath).Contains(row.missing_atlas_guid))
                {
                    material.SetTexture("_MainTex", atlas); EditorUtility.SetDirty(material);
                }
            }
            report.restored_original_empty_atlases.Add(new EmptyAtlasRestoration { font_path=row.font_path,old_guid=row.missing_atlas_guid,new_guid=guid,width=atlas.width,height=atlas.height });
        }
        AssetDatabase.SaveAssets();
        foreach (string path in SerializedFiles())
        {
            string text=File.ReadAllText(path),original=text;
            foreach (var change in report.restored_original_empty_atlases)
                text=Regex.Replace(text,@"\{fileID:\s*2800000,\s*guid:\s*"+change.old_guid+@",\s*type:\s*3\}","{fileID: 2800000, guid: "+change.new_guid+", type: 2}");
            if (text!=original) File.WriteAllText(path,text);
        }
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
    }

    private static void PreserveFontAtlases(Report report)
    {
        foreach (string guid in AssetDatabase.FindAssets("t:TMP_FontAsset", new [] { "Assets" }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var font = AssetDatabase.LoadMainAssetAtPath(path);
            var check = new FontCheck { path=path, textures_valid=true };
            check.original_empty_dynamic_atlas = report.restored_original_empty_atlases.Any(x => x.font_path == path);
            if (font == null) { report.errors.Add("Unreadable font: " + path); continue; }
            var serialized = new SerializedObject(font);
            var atlases = serialized.FindProperty("m_AtlasTextures");
            if (atlases == null || !atlases.isArray) { report.errors.Add("Font atlas property missing: " + path); continue; }
            check.atlas_count = atlases.arraySize;
            for (int i=0;i<atlases.arraySize;i++)
            {
                var texture = atlases.GetArrayElementAtIndex(i).objectReferenceValue as Texture2D;
                bool originalEmpty = check.original_empty_dynamic_atlas && texture != null && texture.width == 0 && texture.height == 0 && texture.format == TextureFormat.Alpha8;
                if (texture == null || (!originalEmpty && (texture.width <= 0 || texture.height <= 0))) check.textures_valid = false;
            }
            if (atlases.arraySize == 0) check.textures_valid = false;
            if (!check.textures_valid) report.errors.Add("Original font atlas reference invalid: " + path);
            // Keep the recovered glyphs/texture intact in the disposable build.
            // TMP's pre-build callback otherwise clears dynamic atlas data.
            var clear = serialized.FindProperty("m_ClearDynamicDataOnBuild");
            if (clear != null && clear.boolValue) { clear.boolValue=false; serialized.ApplyModifiedPropertiesWithoutUndo(); EditorUtility.SetDirty(font); }
            check.clear_on_build_disabled = clear == null || !clear.boolValue;
            report.fonts.Add(check);
        }
    }

    private static void CheckScene(string path, string name, Type type, Report report)
    {
        var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        var roots = scene.GetRootGameObjects();
        foreach (var root in roots) CheckMissing(root, path, report);
        var entry = roots.SelectMany(g => g.GetComponentsInChildren<Transform>(true)).Select(t => t.gameObject).Single(g => g.name == name);
        var component = entry.GetComponent(type);
        if (component == null) { report.errors.Add("Entry component unavailable: " + name); return; }
        if (name == "BoardStart") report.board_start_component = true;
        else
        {
            report.game_start_component = true;
            var property = new SerializedObject(component).FindProperty("mainMenuPrefab");
            var prefab = property == null ? null : property.objectReferenceValue as GameObject;
            report.game_start_prefab = prefab != null;
            if (prefab == null) report.errors.Add("GameStart.mainMenuPrefab was not restored");
        }
    }

    private static void CheckMissing(GameObject root, string path, Report report)
    {
        foreach (var t in root.GetComponentsInChildren<Transform>(true))
        {
            int missing = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject);
            report.missing_components += missing;
            if (missing > 0) report.errors.Add("Missing script " + path + " :: " + t.name + " count=" + missing);
        }
    }
}
#endif
