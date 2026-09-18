#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    private const string RuntimeDllPath = "Assets/Plugins/Assembly-CSharp.dll";
    private const string RuntimeDllOriginalSha256 = "047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee";
    private const string RuntimeDllPatchedSha256 = "9295bcb90857502af5ab802839b5c560f9e4d956b99dfaeae4350bd5fa5754a3";

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

    private static string Sha256(byte[] bytes)
    {
        using (var sha = SHA256.Create())
            return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
    }

    private static ushort ReadUInt16LE(byte[] bytes, int offset)
    {
        return (ushort)(bytes[offset] | (bytes[offset + 1] << 8));
    }

    private static int ReadInt32LE(byte[] bytes, int offset)
    {
        return bytes[offset]
            | (bytes[offset + 1] << 8)
            | (bytes[offset + 2] << 16)
            | (bytes[offset + 3] << 24);
    }

    private static void EnsureRuntimeDllKind()
    {
        if (!File.Exists(RuntimeDllPath))
            throw new FileNotFoundException("Stage9.1 runtime DLL is missing before PE-kind repair.", RuntimeDllPath);

        var bytes = File.ReadAllBytes(RuntimeDllPath);
        var beforeSha = Sha256(bytes);
        if (beforeSha == RuntimeDllPatchedSha256)
        {
            Debug.Log("STAGE9_IOS_DLL_KIND already=Dll sha256=" + beforeSha);
            return;
        }
        if (beforeSha != RuntimeDllOriginalSha256)
            throw new InvalidOperationException("Unexpected Stage9.1 runtime DLL before PE-kind repair: " + beforeSha);

        if (bytes.Length < 0x100)
            throw new InvalidDataException("Stage9.1 runtime DLL is too small to contain a valid PE/CLI header.");

        var pe = ReadInt32LE(bytes, 0x3c);
        if (pe < 0 || pe + 24 > bytes.Length || bytes[pe] != (byte)'P' || bytes[pe + 1] != (byte)'E' || bytes[pe + 2] != 0 || bytes[pe + 3] != 0)
            throw new InvalidDataException("Stage9.1 runtime DLL has an invalid PE signature.");

        var coff = pe + 4;
        var characteristicsOffset = coff + 18;
        var characteristics = ReadUInt16LE(bytes, characteristicsOffset);
        if (characteristics != 0x0122)
            throw new InvalidDataException($"Unexpected Stage9.1 COFF Characteristics before repair: 0x{characteristics:x4}");

        // Formal proof run 35363174000 established that this is the only byte
        // separating the recovered Console-kind assembly from a DLL-kind assembly.
        // The CLI EntryPoint remains null and all 2317 MethodDefs remain identical.
        bytes[characteristicsOffset + 1] = (byte)(bytes[characteristicsOffset + 1] | 0x20);

        var afterSha = Sha256(bytes);
        if (afterSha != RuntimeDllPatchedSha256)
            throw new InvalidOperationException("Stage9.1 one-byte PE-kind repair produced an unexpected SHA256: " + afterSha);

        File.WriteAllBytes(RuntimeDllPath, bytes);
        Debug.Log($"STAGE9_IOS_DLL_KIND Console->Dll offset=0x{characteristicsOffset + 1:x} sha256={afterSha}");
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

        // This runs in the dedicated migration Unity invocation. Repair the PE
        // kind here, then force an import before that process exits. The separate
        // final export invocation therefore starts with the verified DLL-kind file.
        EnsureRuntimeDllKind();

        Directory.CreateDirectory("Library");
        File.WriteAllText(Marker, $"resolved={resolved}/{expected}\nreferences={refs}\nfiles={files}\ndll_sha256={RuntimeDllPatchedSha256}\n");
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        Debug.Log($"GodsPVZ: migrated {refs} package script references in {files} files ({resolved}/{expected} types resolved); iOS DLL kind repaired.");
    }
}
#endif
