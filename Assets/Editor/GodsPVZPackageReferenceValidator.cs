#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class GodsPVZPackageReferenceValidator
{
    [Serializable] private sealed class Root { public PackageEntry[] packages; }
    [Serializable] private sealed class PackageEntry { public string dll; public string guid; public MapEntry[] mapping; }
    [Serializable] private sealed class MapEntry { public long fileID; public string fullName; }

    private sealed class ScriptInfo
    {
        public string FullName;
        public string AssemblyName;
        public string Guid;
        public string Path;
    }

    private const string MapPath = "Recovery/package-script-map-editor.json";
    private const string ReportPath = "Library/GodsPVZ.package-reference-validation.txt";
    private static readonly string[] SerializedExtensions =
    {
        ".unity", ".prefab", ".asset", ".mat", ".controller", ".anim", ".overridecontroller"
    };

    [MenuItem("GodsPVZ/Recovery/Validate Package Script References 67/67")]
    public static void ValidateInteractive()
    {
        var result = Validate();
        if (!result.Ok) throw new InvalidOperationException(result.Report);
        Debug.Log(result.Report);
    }

    // Batch entry point:
    // Unity -batchmode -quit -projectPath <project> -executeMethod GodsPVZPackageReferenceValidator.ValidateBatch
    public static void ValidateBatch()
    {
        var result = Validate();
        Debug.Log(result.Report);
        if (!result.Ok)
        {
            Debug.LogError("GodsPVZ package reference gate failed.");
            EditorApplication.Exit(23);
        }
    }

    private sealed class Result
    {
        public bool Ok;
        public string Report;
    }

    private static Result Validate()
    {
        var sb = new StringBuilder();
        var failures = new List<string>();
        Directory.CreateDirectory("Library");

        if (!File.Exists(MapPath))
        {
            var missing = $"FAIL map missing: {MapPath}";
            File.WriteAllText(ReportPath, missing + "\n");
            return new Result { Ok = false, Report = missing };
        }

        var root = JsonUtility.FromJson<Root>(File.ReadAllText(MapPath));
        if (root == null || root.packages == null)
        {
            var malformed = "FAIL package script map is malformed.";
            File.WriteAllText(ReportPath, malformed + "\n");
            return new Result { Ok = false, Report = malformed };
        }

        var expectedCounts = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["UnityEngine.UI.dll"] = 19,
            ["Unity.TextMeshPro.dll"] = 9,
            ["Unity.RenderPipelines.Core.Runtime.dll"] = 32,
            ["Unity.RenderPipelines.Universal.Runtime.dll"] = 5,
            ["Unity.2D.Animation.Runtime.dll"] = 2,
        };

        int mapTotal = 0;
        foreach (var pkg in root.packages)
        {
            int count = pkg != null && pkg.mapping != null ? pkg.mapping.Length : 0;
            mapTotal += count;
            if (pkg == null || String.IsNullOrEmpty(pkg.dll))
            {
                failures.Add("map contains package entry with missing dll name");
                continue;
            }
            if (!expectedCounts.TryGetValue(pkg.dll, out var expected))
                failures.Add($"unexpected mapped assembly: {pkg.dll}");
            else if (count != expected)
                failures.Add($"{pkg.dll}: map count {count}, expected {expected}");
        }
        foreach (var kv in expectedCounts)
        {
            if (!root.packages.Any(p => p != null && p.dll == kv.Key))
                failures.Add($"missing mapped assembly: {kv.Key}");
        }
        if (mapTotal != 67) failures.Add($"map total {mapTotal}, expected 67");

        var scripts = new Dictionary<string, List<ScriptInfo>>(StringComparer.Ordinal);
        foreach (var path in AssetDatabase.GetAllAssetPaths())
        {
            if (!path.StartsWith("Packages/", StringComparison.Ordinal) ||
                !path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)) continue;

            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            if (script == null) continue;
            var type = script.GetClass();
            if (type == null || String.IsNullOrEmpty(type.FullName)) continue;
            var guid = AssetDatabase.AssetPathToGUID(path);
            if (String.IsNullOrEmpty(guid)) continue;

            if (!scripts.TryGetValue(type.FullName, out var list))
            {
                list = new List<ScriptInfo>();
                scripts.Add(type.FullName, list);
            }
            list.Add(new ScriptInfo
            {
                FullName = type.FullName,
                AssemblyName = type.Assembly.GetName().Name,
                Guid = guid,
                Path = path,
            });
        }

        int resolved = 0;
        var targetRefs = new Dictionary<string, string>(StringComparer.Ordinal);
        var oldRefs = new HashSet<string>(StringComparer.Ordinal);
        var detail = new List<string>();

        foreach (var pkg in root.packages)
        {
            if (pkg == null || pkg.mapping == null) continue;
            string expectedAssembly = Path.GetFileNameWithoutExtension(pkg.dll ?? String.Empty);
            foreach (var m in pkg.mapping)
            {
                if (m == null || String.IsNullOrEmpty(m.fullName))
                {
                    failures.Add($"{pkg.dll}: empty fullName mapping");
                    continue;
                }

                string oldRef = $"{{fileID: {m.fileID}, guid: {pkg.guid}, type: 3}}";
                oldRefs.Add(oldRef);

                if (!scripts.TryGetValue(m.fullName, out var candidates))
                {
                    failures.Add($"unresolved {pkg.dll} :: {m.fullName}: no Package MonoScript");
                    continue;
                }

                var exact = candidates.Where(x => x.AssemblyName == expectedAssembly).ToArray();
                if (exact.Length != 1)
                {
                    failures.Add($"unresolved {pkg.dll} :: {m.fullName}: exact assembly matches={exact.Length}");
                    continue;
                }

                resolved++;
                string newRef = $"{{fileID: 11500000, guid: {exact[0].Guid}, type: 3}}";
                targetRefs[m.fullName] = newRef;
                detail.Add($"PASS\t{pkg.dll}\t{m.fileID}\t{m.fullName}\t{exact[0].Guid}\t{exact[0].Path}");
            }
        }

        if (resolved != 67) failures.Add($"resolved package types {resolved}/67");

        int serializedFiles = 0;
        int oldRefHits = 0;
        var oldRefHitFiles = new HashSet<string>(StringComparer.Ordinal);
        var newRefHitTypes = new HashSet<string>(StringComparer.Ordinal);

        if (Directory.Exists("Assets"))
        {
            foreach (var path in Directory.EnumerateFiles("Assets", "*", SearchOption.AllDirectories))
            {
                string ext = Path.GetExtension(path).ToLowerInvariant();
                if (!SerializedExtensions.Contains(ext)) continue;
                string text;
                try { text = File.ReadAllText(path); }
                catch { continue; }
                serializedFiles++;

                foreach (var oldRef in oldRefs)
                {
                    int at = 0;
                    while ((at = text.IndexOf(oldRef, at, StringComparison.Ordinal)) >= 0)
                    {
                        oldRefHits++;
                        oldRefHitFiles.Add(path);
                        at += oldRef.Length;
                    }
                }
                foreach (var kv in targetRefs)
                {
                    if (text.IndexOf(kv.Value, StringComparison.Ordinal) >= 0)
                        newRefHitTypes.Add(kv.Key);
                }
            }
        }

        if (oldRefHits != 0)
            failures.Add($"unmigrated legacy package references remain: {oldRefHits} hits in {oldRefHitFiles.Count} files");

        sb.AppendLine("GodsPVZ package script reference validation");
        sb.AppendLine($"map={mapTotal}/67");
        sb.AppendLine($"resolved={resolved}/67");
        sb.AppendLine($"serialized_files_scanned={serializedFiles}");
        sb.AppendLine($"legacy_reference_hits={oldRefHits}");
        sb.AppendLine($"legacy_reference_files={oldRefHitFiles.Count}");
        sb.AppendLine($"resolved_target_types_observed_in_assets={newRefHitTypes.Count}/67");
        sb.AppendLine($"result={(failures.Count == 0 ? "PASS" : "FAIL")}");
        sb.AppendLine("---TYPE_MAP---");
        foreach (var row in detail.OrderBy(x => x, StringComparer.Ordinal)) sb.AppendLine(row);
        if (failures.Count > 0)
        {
            sb.AppendLine("---FAILURES---");
            foreach (var failure in failures) sb.AppendLine("FAIL\t" + failure);
        }

        var report = sb.ToString();
        File.WriteAllText(ReportPath, report);
        return new Result { Ok = failures.Count == 0, Report = report };
    }
}
#endif
