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
    [Serializable] public class ShaderBinding { public string shader, old_guid, new_guid, target; public int references; }
    [Serializable] public class FontCheck { public string path; public int atlas_count; public bool textures_valid, clear_on_build_disabled; }
    [Serializable] public class Report
    {
        public string scope = "REAL_UNITY_SCENE_SCRIPT_AND_OFFICIAL_SHADER_BINDING_ONLY";
        public bool pass, gameplay_equivalence_proven = false, device_execution_performed = false;
        public int plugin_scripts, resolved_plugin_scripts, validated_game_script_references, prefab_assets, missing_components;
        public bool game_start_component, game_start_prefab, board_start_component, core_blit_keyword;
        public List<string> errors = new List<string>();
        public List<string> unresolved_custom_dummy_shaders = new List<string>();
        public List<ShaderBinding> official_shader_bindings = new List<ShaderBinding>();
        public List<FontCheck> fonts = new List<FontCheck>();
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
            ImportOfficialTmpResources();
            RebindOfficialShaders(report);
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

    private static void ImportOfficialTmpResources()
    {
        string path = "Packages/com.unity.textmeshpro/Package Resources/TMP Essential Resources.unitypackage";
        if (!File.Exists(path)) throw new FileNotFoundException("Exact package TMP essential shaders unavailable", path);
        if (!File.Exists("Assets/TextMesh Pro/Shaders/TMP_SDF.shader")) AssetDatabase.ImportPackage(path, false);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
    }

    private static void RebindOfficialShaders(Report report)
    {
        var declarations = new Regex("Shader\\s+\"([^\"]+)\"");
        var official = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        foreach (string path in AssetDatabase.GetAllAssetPaths())
        {
            if (!(path.StartsWith("Packages/", StringComparison.Ordinal) || path.StartsWith("Assets/TextMesh Pro/Shaders/", StringComparison.Ordinal)) || !path.EndsWith(".shader", StringComparison.OrdinalIgnoreCase) || !File.Exists(path)) continue;
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

    private static void PreserveFontAtlases(Report report)
    {
        foreach (string guid in AssetDatabase.FindAssets("t:TMP_FontAsset", new [] { "Assets" }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var font = AssetDatabase.LoadMainAssetAtPath(path);
            var check = new FontCheck { path=path, textures_valid=true };
            if (font == null) { report.errors.Add("Unreadable font: " + path); continue; }
            var serialized = new SerializedObject(font);
            var atlases = serialized.FindProperty("m_AtlasTextures");
            if (atlases == null || !atlases.isArray) { report.errors.Add("Font atlas property missing: " + path); continue; }
            check.atlas_count = atlases.arraySize;
            for (int i=0;i<atlases.arraySize;i++)
            {
                var texture = atlases.GetArrayElementAtIndex(i).objectReferenceValue as Texture2D;
                if (texture == null || texture.width <= 0 || texture.height <= 0) check.textures_valid = false;
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
