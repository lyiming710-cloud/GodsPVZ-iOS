#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class GodsPVZPackageReferenceMigrator
{
    [Serializable] private sealed class Root { public PackageEntry[] packages; }
    [Serializable] private sealed class PackageEntry { public string dll; public string guid; public MapEntry[] mapping; }
    [Serializable] private sealed class MapEntry { public long fileID; public string fullName; }
    private const string MapPath = "Recovery/package-script-map-editor.json";
    private const string Marker = "Library/GodsPVZ.package-reference-migration.done";

    static GodsPVZPackageReferenceMigrator()
    {
        EditorApplication.delayCall += TryMigrate;
    }

    [MenuItem("GodsPVZ/Recovery/Migrate Package Script References")]
    public static void RunBatch()
    {
        TryMigrate();
        if (!File.Exists(Marker))
            throw new InvalidOperationException("GodsPVZ package-reference migration did not produce its completion marker.");
    }

    private static void TryMigrate()
    {
        if (File.Exists(Marker)) return;
        if (!File.Exists(MapPath)) { Debug.LogWarning("GodsPVZ: package script map not found yet."); return; }

        var root = JsonUtility.FromJson<Root>(File.ReadAllText(MapPath));
        if (root == null || root.packages == null) return;

        var official = new Dictionary<string,string>(StringComparer.Ordinal);
        foreach (var path in AssetDatabase.GetAllAssetPaths())
        {
            if (!path.StartsWith("Packages/", StringComparison.Ordinal) || !path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)) continue;
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            if (script == null) continue;
            var type = script.GetClass();
            if (type == null) continue;
            var full = type.FullName;
            if (String.IsNullOrEmpty(full) || official.ContainsKey(full)) continue;
            var guid = AssetDatabase.AssetPathToGUID(path);
            if (!String.IsNullOrEmpty(guid)) official.Add(full, guid);
        }

        int expected = 0, resolved = 0;
        var replacements = new Dictionary<string,string>(StringComparer.Ordinal);
        foreach (var pkg in root.packages)
        {
            if (pkg.mapping == null) continue;
            foreach (var m in pkg.mapping)
            {
                expected++;
                if (m == null || String.IsNullOrEmpty(m.fullName) || !official.TryGetValue(m.fullName, out var targetGuid)) continue;
                var oldRef = $"{{fileID: {m.fileID}, guid: {pkg.guid}, type: 3}}";
                var newRef = $"{{fileID: 11500000, guid: {targetGuid}, type: 3}}";
                replacements[oldRef] = newRef;
                resolved++;
            }
        }
        if (resolved != expected)
        {
            Debug.LogWarning($"GodsPVZ: package references not ready: resolved {resolved}/{expected}. Waiting for Package Manager/import.");
            EditorApplication.delayCall += TryMigrate;
            return;
        }

        int refs = 0, files = 0;
        foreach (var path in Directory.EnumerateFiles("Assets", "*", SearchOption.AllDirectories))
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (ext != ".unity" && ext != ".prefab" && ext != ".asset" && ext != ".mat" && ext != ".controller" && ext != ".anim" && ext != ".overridecontroller") continue;
            string text;
            try { text = File.ReadAllText(path); } catch { continue; }
            var original = text;
            foreach (var kv in replacements)
            {
                int at = 0;
                while ((at = text.IndexOf(kv.Key, at, StringComparison.Ordinal)) >= 0) { refs++; text = text.Remove(at, kv.Key.Length).Insert(at, kv.Value); at += kv.Value.Length; }
            }
            if (!ReferenceEquals(text, original) && text != original) { File.WriteAllText(path, text); files++; }
        }
        Directory.CreateDirectory("Library");
        File.WriteAllText(Marker, $"resolved={resolved}/{expected}\nreferences={refs}\nfiles={files}\n");
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        Debug.Log($"GodsPVZ: migrated {refs} package script references in {files} files ({resolved}/{expected} types resolved).");
    }
}
#endif
