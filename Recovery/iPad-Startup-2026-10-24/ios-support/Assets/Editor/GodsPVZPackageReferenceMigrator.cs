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
    private const string RuntimeDllDllKindSha256 = "9295bcb90857502af5ab802839b5c560f9e4d956b99dfaeae4350bd5fa5754a3";
    private const string RuntimeDllDamageRepairSha256 = "3a9b38e7b4c941b11d5683f74a547540fd2e5c891318f1f2ca79dbb88ecdafa8";
    private const string RuntimeDllIosPreBuffSha256 = "f47b1b37f1d4d3a1ddeb44bb6dbe399c58dc6dee3d4602618b2790f839d3e321";
    private const string RuntimeDllIosFinalSha256 = "f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433";

    private const int DamageAddElementRva = 0x14e70;
    private const int DamageLocalSigOldToken = unchecked((int)0x110000D7);
    private const int DamageLocalSigNewToken = unchecked((int)0x11000367);

    private const int DamageGetCurrentIlOffset = 0x19;
    private const int DamageGetCurrentOldToken = unchecked((int)0x0A0000C3);
    private const int DamageGetCurrentNewToken = unchecked((int)0x0A0000E7);

    private const int DamageMoveNextIlOffset = 0x76;
    private const int DamageMoveNextOldToken = unchecked((int)0x0A0000C4);
    private const int DamageMoveNextNewToken = unchecked((int)0x0A0000E9);

    private const int DamageDisposeIlOffset = 0x89;
    private const int DamageDisposeOldToken = unchecked((int)0x0A0000C5);
    private const int DamageDisposeNewToken = unchecked((int)0x0A000223);

    // Board.Start local signature 0x11000220 is:
    //   07 03 12 81 88 12 15 1e 00
    // The final 1e 00 is an ownerless MVAR 0 reconstructed where the method
    // actually stores Load<Sprite>() and passes it to SpriteRenderer.set_sprite.
    // The existing UnityEngine.Sprite TypeRef 0x01000017 encodes as CLASS 12 5d,
    // exactly the same two-byte width, so this is an in-place metadata repair.
    private const int BoardStartSpriteLocalBlobOffset = 0x11ac35;
    private const byte BoardStartSpriteLocalOld0 = 0x1e;
    private const byte BoardStartSpriteLocalOld1 = 0x00;
    private const byte BoardStartSpriteLocalNew0 = 0x12;
    private const byte BoardStartSpriteLocalNew1 = 0x5d;

    // Run 8 reached IL2CPP DataModel and failed resolving MemberRef 0x0A0000B4:
    // List<Buff>.GetEnumerator(). The Parent TypeSpec is already correct; only
    // the MemberRef signature points to a concrete Enumerator<Buff> return blob.
    // Reuse the canonical definition-level Enumerator<!0> blob already present
    // in this exact DLL by changing the two-byte #Blob index 0x1A81 -> 0x1C24.
    private const int BuffGetEnumeratorSignatureIndexOffset = 0xC99DE;
    private const byte BuffGetEnumeratorSignatureOld0 = 0x81;
    private const byte BuffGetEnumeratorSignatureOld1 = 0x1A;
    private const byte BuffGetEnumeratorSignatureNew0 = 0x24;
    private const byte BuffGetEnumeratorSignatureNew1 = 0x1C;

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

    private static void WriteInt32LE(byte[] bytes, int offset, int value)
    {
        bytes[offset] = (byte)value;
        bytes[offset + 1] = (byte)(value >> 8);
        bytes[offset + 2] = (byte)(value >> 16);
        bytes[offset + 3] = (byte)(value >> 24);
    }

    private static int RvaToFileOffset(byte[] bytes, int pe, int rva)
    {
        var coff = pe + 4;
        var sectionCount = ReadUInt16LE(bytes, coff + 2);
        var optionalSize = ReadUInt16LE(bytes, coff + 16);
        var sections = coff + 20 + optionalSize;
        for (var i = 0; i < sectionCount; i++)
        {
            var s = sections + i * 40;
            var virtualSize = ReadInt32LE(bytes, s + 8);
            var virtualAddress = ReadInt32LE(bytes, s + 12);
            var rawSize = ReadInt32LE(bytes, s + 16);
            var rawOffset = ReadInt32LE(bytes, s + 20);
            var span = Math.Max(virtualSize, rawSize);
            if (rva >= virtualAddress && rva < virtualAddress + span)
                return rawOffset + (rva - virtualAddress);
        }
        throw new InvalidDataException($"RVA 0x{rva:x} is outside all PE sections.");
    }

    private static int ValidateCallAndGetTokenOffset(byte[] bytes, int codeOffset, int ilOffset, int expectedToken, string label)
    {
        var opcodeOffset = codeOffset + ilOffset;
        if (bytes[opcodeOffset] != 0x28)
            throw new InvalidDataException($"{label} is not call (0x28): 0x{bytes[opcodeOffset]:x2}");

        var tokenOffset = opcodeOffset + 1;
        var actual = ReadInt32LE(bytes, tokenOffset);
        if (actual != expectedToken)
            throw new InvalidDataException($"Unexpected {label} MemberRef token: 0x{actual:x8}; expected 0x{expectedToken:x8}");
        return tokenOffset;
    }

    private static void EnsureRuntimeDllForIosLinker()
    {
        if (!File.Exists(RuntimeDllPath))
            throw new FileNotFoundException("Stage9.1 runtime DLL is missing before iOS metadata repair.", RuntimeDllPath);

        var bytes = File.ReadAllBytes(RuntimeDllPath);
        var beforeSha = Sha256(bytes);
        if (beforeSha == RuntimeDllIosFinalSha256)
        {
            Debug.Log("STAGE9_IOS_DLL_REPAIR already=complete sha256=" + beforeSha);
            return;
        }
        if (beforeSha != RuntimeDllOriginalSha256 &&
            beforeSha != RuntimeDllDllKindSha256 &&
            beforeSha != RuntimeDllDamageRepairSha256 &&
            beforeSha != RuntimeDllIosPreBuffSha256)
            throw new InvalidOperationException("Unexpected Stage9.1 runtime DLL before iOS metadata repair: " + beforeSha);

        if (bytes.Length <= Math.Max(BoardStartSpriteLocalBlobOffset + 1, BuffGetEnumeratorSignatureIndexOffset + 1))
            throw new InvalidDataException("Stage9.1 runtime DLL is too small for the proven metadata offsets.");

        var pe = ReadInt32LE(bytes, 0x3c);
        if (pe < 0 || pe + 24 > bytes.Length || bytes[pe] != (byte)'P' || bytes[pe + 1] != (byte)'E' || bytes[pe + 2] != 0 || bytes[pe + 3] != 0)
            throw new InvalidDataException("Stage9.1 runtime DLL has an invalid PE signature.");

        var coff = pe + 4;
        var characteristicsOffset = coff + 18;
        var characteristics = ReadUInt16LE(bytes, characteristicsOffset);

        if (beforeSha == RuntimeDllOriginalSha256)
        {
            if (characteristics != 0x0122)
                throw new InvalidDataException($"Unexpected Stage9.1 COFF Characteristics before DLL-kind repair: 0x{characteristics:x4}");

            // Formal proof: only IMAGE_FILE_DLL is needed to turn the recovered
            // null-entrypoint Console-kind assembly into a DLL-kind assembly.
            bytes[characteristicsOffset + 1] = (byte)(bytes[characteristicsOffset + 1] | 0x20);

            var dllKindSha = Sha256(bytes);
            if (dllKindSha != RuntimeDllDllKindSha256)
                throw new InvalidOperationException("Stage9.1 PE DLL-kind repair produced an unexpected SHA256: " + dllKindSha);
        }
        else if (characteristics != 0x2122)
        {
            throw new InvalidDataException($"Unexpected Stage9.1 COFF Characteristics for pre-repaired DLL: 0x{characteristics:x4}");
        }

        var currentSha = Sha256(bytes);

        // Run 35368894455 proved UnityLinker passes after the first Damage fix,
        // but IL2CPP then crashed while walking the remaining Enumerator<!0>.
        // Formal proof run 35411975011 selected canonical rows already present in
        // this DLL and established a seven-byte repair with 2317 MethodDefs intact.
        if (currentSha == RuntimeDllDllKindSha256)
        {
            var methodBody = RvaToFileOffset(bytes, pe, DamageAddElementRva);
            var flagsAndSize = ReadUInt16LE(bytes, methodBody);
            if ((flagsAndSize & 0x3) != 0x3)
                throw new InvalidDataException($"Unexpected Damage.AddElement method header: 0x{flagsAndSize:x4}");
            var methodHeaderSize = ((flagsAndSize >> 12) & 0xF) * 4;
            if (methodHeaderSize != 12)
                throw new InvalidDataException($"Unexpected Damage.AddElement fat-header size: {methodHeaderSize}");

            var localSigOffset = methodBody + 8;
            var oldLocalSig = ReadInt32LE(bytes, localSigOffset);
            if (oldLocalSig != DamageLocalSigOldToken)
                throw new InvalidDataException($"Unexpected Damage.AddElement LocalVarSig token: 0x{oldLocalSig:x8}");

            var code = methodBody + methodHeaderSize;
            var getCurrentTokenOffset = ValidateCallAndGetTokenOffset(bytes, code, DamageGetCurrentIlOffset, DamageGetCurrentOldToken, "Damage.AddElement IL_0019");
            var moveNextTokenOffset = ValidateCallAndGetTokenOffset(bytes, code, DamageMoveNextIlOffset, DamageMoveNextOldToken, "Damage.AddElement IL_0076");
            var disposeTokenOffset = ValidateCallAndGetTokenOffset(bytes, code, DamageDisposeIlOffset, DamageDisposeOldToken, "Damage.AddElement IL_0089");

            WriteInt32LE(bytes, localSigOffset, DamageLocalSigNewToken);
            WriteInt32LE(bytes, getCurrentTokenOffset, DamageGetCurrentNewToken);
            WriteInt32LE(bytes, moveNextTokenOffset, DamageMoveNextNewToken);
            WriteInt32LE(bytes, disposeTokenOffset, DamageDisposeNewToken);

            currentSha = Sha256(bytes);
            if (currentSha != RuntimeDllDamageRepairSha256)
                throw new InvalidOperationException("Stage9.1 seven-byte Damage metadata repair produced an unexpected SHA256: " + currentSha);
        }
        else if (currentSha != RuntimeDllDamageRepairSha256 && currentSha != RuntimeDllIosPreBuffSha256)
        {
            throw new InvalidOperationException("Unexpected Stage9.1 runtime DLL at Damage repair gate: " + currentSha);
        }

        // Run 35412031380 removed the generic-instance failure but IL2CPP still
        // crashed at AddGenericParameter directly from ProcessMethod. The only
        // remaining ownerless generic found in the recovered assembly was
        // Board.Start local2. Its actual IL is Load<Sprite>() -> stloc.2 ->
        // SpriteRenderer.set_sprite(Sprite). Formal proof run 35412364048 repairs
        // only the two same-width signature bytes below; the resulting nine-byte
        // candidate remains a DLL with 2317 MethodDefs and Board.Start locals
        // [Map, Camera, Sprite]. A full Cecil metadata audit of that candidate
        // reports orphan_hits=0.
        if (currentSha == RuntimeDllDamageRepairSha256)
        {
            if (bytes[BoardStartSpriteLocalBlobOffset] != BoardStartSpriteLocalOld0 ||
                bytes[BoardStartSpriteLocalBlobOffset + 1] != BoardStartSpriteLocalOld1)
                throw new InvalidDataException(
                    $"Unexpected Board.Start local signature bytes at 0x{BoardStartSpriteLocalBlobOffset:x}: " +
                    $"{bytes[BoardStartSpriteLocalBlobOffset]:x2} {bytes[BoardStartSpriteLocalBlobOffset + 1]:x2}");

            bytes[BoardStartSpriteLocalBlobOffset] = BoardStartSpriteLocalNew0;
            bytes[BoardStartSpriteLocalBlobOffset + 1] = BoardStartSpriteLocalNew1;
            currentSha = Sha256(bytes);
            if (currentSha != RuntimeDllIosPreBuffSha256)
                throw new InvalidOperationException("Stage9.1 nine-byte iOS metadata repair produced an unexpected SHA256: " + currentSha);

            Debug.Log(
                $"STAGE9_IOS_DLL_REPAIR complete sha256={currentSha}; " +
                $"PE_DLL=0x{characteristicsOffset + 1:x}; " +
                "Damage.LocalVarSig/Enumerator refs canonicalized; " +
                $"Board.Start local2 MVAR0->UnityEngine.Sprite at 0x{BoardStartSpriteLocalBlobOffset:x}");
        }

        currentSha = Sha256(bytes);
        if (currentSha != RuntimeDllIosPreBuffSha256)
            throw new InvalidOperationException("Unexpected Stage9.1 runtime DLL before Buff.GetEnumerator repair: " + currentSha);

        if (bytes[BuffGetEnumeratorSignatureIndexOffset] != BuffGetEnumeratorSignatureOld0 ||
            bytes[BuffGetEnumeratorSignatureIndexOffset + 1] != BuffGetEnumeratorSignatureOld1)
            throw new InvalidDataException(
                $"Unexpected Buff.GetEnumerator signature-index bytes at 0x{BuffGetEnumeratorSignatureIndexOffset:x}: " +
                $"{bytes[BuffGetEnumeratorSignatureIndexOffset]:x2} {bytes[BuffGetEnumeratorSignatureIndexOffset + 1]:x2}");

        bytes[BuffGetEnumeratorSignatureIndexOffset] = BuffGetEnumeratorSignatureNew0;
        bytes[BuffGetEnumeratorSignatureIndexOffset + 1] = BuffGetEnumeratorSignatureNew1;

        var afterSha = Sha256(bytes);
        if (afterSha != RuntimeDllIosFinalSha256)
            throw new InvalidOperationException("Stage9.1 Buff.GetEnumerator metadata repair produced an unexpected SHA256: " + afterSha);

        File.WriteAllBytes(RuntimeDllPath, bytes);
        Debug.Log(
            $"STAGE9_IOS_DLL_REPAIR complete sha256={afterSha}; " +
            "Buff.GetEnumerator MemberRef=0x0A0000B4 Signature=0x1A81->0x1C24; " +
            "byte_diff_from_run8=2");
    }

    private static void TryMigrate()
    {
        if (File.Exists(Marker))
        {
            if (!File.Exists(RuntimeDllPath))
                throw new FileNotFoundException("Stage9.1 runtime DLL is missing while migration marker exists.", RuntimeDllPath);

            var currentDllSha = Sha256(File.ReadAllBytes(RuntimeDllPath));
            if (currentDllSha == RuntimeDllIosFinalSha256)
                return;

            EnsureRuntimeDllForIosLinker();
            var markerText = File.ReadAllText(Marker);
            if (Regex.IsMatch(markerText, @"(?m)^dll_sha256=.*$"))
                markerText = Regex.Replace(markerText, @"(?m)^dll_sha256=.*$", "dll_sha256=" + RuntimeDllIosFinalSha256);
            else
                markerText += (markerText.EndsWith("\n", StringComparison.Ordinal) ? "" : "\n") + "dll_sha256=" + RuntimeDllIosFinalSha256 + "\n";
            File.WriteAllText(Marker, markerText);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            Debug.Log("GodsPVZ: upgraded existing package migration marker to the final iOS metadata repair.");
            return;
        }
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

        // The qualified runtime artifact remains untouched. Only the reconstructed
        // iOS project work copy receives the formally proven metadata repairs.
        EnsureRuntimeDllForIosLinker();

        Directory.CreateDirectory("Library");
        File.WriteAllText(Marker, $"resolved={resolved}/{expected}\nreferences={refs}\nfiles={files}\ndll_sha256={RuntimeDllIosFinalSha256}\n");
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        Debug.Log($"GodsPVZ: migrated {refs} package script references in {files} files ({resolved}/{expected} types resolved); iOS metadata repair applied.");
    }
}
#endif
