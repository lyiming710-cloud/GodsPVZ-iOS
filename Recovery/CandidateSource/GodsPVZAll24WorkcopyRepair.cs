using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

// CANDIDATE ONLY. This helper is intentionally stored under Recovery/CandidateSource
// and is not compiled by Unity. It is the production-integration draft for the
// post-B001 Stage9.1 work-copy repair. Promotion requires the independent 24/24
// Mono.Cecil Resolve gate plus orphan_generic=0 and a real Unity/IL2CPP rerun.
internal static class GodsPVZAll24WorkcopyRepair
{
    internal const string B001Sha256 = "f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433";
    internal const string All24Sha256 = "b07254d5c4077e013bbc934734f7b0949c0942a4e667d6d2b23d2af6c830f720";
    private const int ExpectedSize = 1201152;
    private const int ExpectedMethodDefs = 2317;
    private const int ExpectedDiffFromB001 = 48;
    private const int B001SignatureOffset = 0xC99DE;

    private static readonly int[] GetEnumeratorTokens =
    {
        unchecked((int)0x0A0000B4), unchecked((int)0x0A0000C9), unchecked((int)0x0A0000D0),
        unchecked((int)0x0A0000D6), unchecked((int)0x0A0000E4), unchecked((int)0x0A0000FB),
        unchecked((int)0x0A000141), unchecked((int)0x0A000162), unchecked((int)0x0A00021D),
    };

    private static readonly int[] GetCurrentTokens =
    {
        unchecked((int)0x0A0000B5), unchecked((int)0x0A0000C3), unchecked((int)0x0A0000CA),
        unchecked((int)0x0A0000D1), unchecked((int)0x0A0000D7), unchecked((int)0x0A0000E5),
        unchecked((int)0x0A0000E7), unchecked((int)0x0A0000FC), unchecked((int)0x0A000142),
        unchecked((int)0x0A000163), unchecked((int)0x0A00021E),
    };

    private static readonly int[] InsertTokens =
    {
        unchecked((int)0x0A000140), unchecked((int)0x0A00022B),
    };

    private sealed class TransformRow
    {
        internal int BadToken;
        internal int CanonicalToken;
        internal TransformRow(int bad, int canonical) { BadToken = bad; CanonicalToken = canonical; }
    }

    private static readonly TransformRow[] TransformRows =
    {
        new TransformRow(unchecked((int)0x0A000149), unchecked((int)0x0A000008)),
        new TransformRow(unchecked((int)0x0A000284), unchecked((int)0x0A00002C)),
    };

    private sealed class TransformIlSite
    {
        internal int FileOffset;
        internal int Token;
        internal string Label;
        internal TransformIlSite(int fileOffset, int token, string label)
        {
            FileOffset = fileOffset; Token = token; Label = label;
        }
    }

    private static readonly TransformIlSite[] TransformIlSites =
    {
        new TransformIlSite(0x1E569, unchecked((int)0x0A000149), "EnemyManager.PlayBoardAudio IL_0019"),
        new TransformIlSite(0x83E4E, unchecked((int)0x0A000149), "FlagMeter.Update IL_002A"),
        new TransformIlSite(0x83E9B, unchecked((int)0x0A000284), "FlagMeter.Update IL_0077"),
    };

    private static readonly byte[] GetEnumeratorBlob = Hex("20 00 15 11 55 01 13 00");
    private static readonly byte[] GetCurrentBlob = Hex("20 00 13 00");
    private static readonly byte[] InsertBlob = Hex("20 02 01 08 13 00");
    private static readonly byte[] GetTransformBlob = Hex("20 00 12 11");

    internal static byte[] Apply(byte[] input)
    {
        if (input == null) throw new ArgumentNullException("input");
        if (input.Length != ExpectedSize)
            throw new InvalidDataException("Unexpected Stage9.1 DLL size: " + input.Length);

        var beforeSha = Sha256(input);
        if (String.Equals(beforeSha, All24Sha256, StringComparison.OrdinalIgnoreCase))
            return (byte[])input.Clone();
        if (!String.Equals(beforeSha, B001Sha256, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("All24 candidate helper only accepts the locked B001 work copy. SHA256=" + beforeSha);

        var original = (byte[])input.Clone();
        var bytes = (byte[])input.Clone();
        var md = new MetadataLayout(bytes);

        if (md.MethodDefCount != ExpectedMethodDefs)
            throw new InvalidDataException("MethodDef drift: " + md.MethodDefCount);

        var b001 = md.GetMemberRef(unchecked((int)0x0A0000B4));
        if (b001.SignatureOffset != B001SignatureOffset)
            throw new InvalidDataException(String.Format("B001 parser anchor mismatch: 0x{0:X} != 0x{1:X}", b001.SignatureOffset, B001SignatureOffset));
        if (md.ReadString(b001.NameIndex) != "GetEnumerator")
            throw new InvalidDataException("B001 MemberRef name mismatch.");

        md.RequireBlob(0x1C24, GetEnumeratorBlob, "GetEnumerator");
        md.RequireBlob(0x4630, GetCurrentBlob, "get_Current");
        md.RequireBlob(0x0976, InsertBlob, "Insert");

        SetSignatures(md, GetEnumeratorTokens, "GetEnumerator", 0x1C24);
        SetSignatures(md, GetCurrentTokens, "get_Current", 0x4630);
        SetSignatures(md, InsertTokens, "Insert", 0x0976);

        for (var i = 0; i < TransformRows.Length; i++)
        {
            var rule = TransformRows[i];
            var bad = md.GetMemberRef(rule.BadToken);
            var canonical = md.GetMemberRef(rule.CanonicalToken);
            if (bad.Parent != canonical.Parent)
                throw new InvalidDataException(String.Format("Transform Parent mismatch bad=0x{0:X8} canonical=0x{1:X8}", rule.BadToken, rule.CanonicalToken));
            if (md.ReadString(canonical.NameIndex) != "get_transform")
                throw new InvalidDataException(String.Format("Canonical transform name mismatch at 0x{0:X8}", rule.CanonicalToken));
            if (!BytesEqual(md.ReadBlob(canonical.SignatureIndex), GetTransformBlob))
                throw new InvalidDataException(String.Format("Canonical transform signature mismatch at 0x{0:X8}", rule.CanonicalToken));

            var oldName = md.ReadString(bad.NameIndex);
            if (oldName != "transform" && oldName != "get_transform")
                throw new InvalidDataException(String.Format("Unexpected transform row name at 0x{0:X8}: {1}", rule.BadToken, oldName));

            md.WriteHeapIndex(bad.NameOffset, md.StringIndexSize, canonical.NameIndex);
            md.WriteHeapIndex(bad.SignatureOffset, md.BlobIndexSize, canonical.SignatureIndex);
        }

        for (var i = 0; i < TransformIlSites.Length; i++)
        {
            var site = TransformIlSites[i];
            var opcode = bytes[site.FileOffset];
            var token = ReadInt32LE(bytes, site.FileOffset + 1);
            if (opcode != 0x7B && opcode != 0x6F)
                throw new InvalidDataException(String.Format("{0}: unexpected opcode 0x{1:X2}", site.Label, opcode));
            if (token != site.Token)
                throw new InvalidDataException(String.Format("{0}: operand 0x{1:X8} != expected 0x{2:X8}", site.Label, token, site.Token));
            bytes[site.FileOffset] = 0x6F;
        }

        var afterSha = Sha256(bytes);
        if (!String.Equals(afterSha, All24Sha256, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("All24 output hash mismatch: " + afterSha);

        var diff = 0;
        for (var i = 0; i < bytes.Length; i++)
            if (bytes[i] != original[i]) diff++;
        if (diff != ExpectedDiffFromB001)
            throw new InvalidDataException("All24 diff-count mismatch from B001: " + diff + " != " + ExpectedDiffFromB001);

        return bytes;
    }

    private static void SetSignatures(MetadataLayout md, int[] tokens, string expectedName, int blobIndex)
    {
        for (var i = 0; i < tokens.Length; i++)
        {
            var row = md.GetMemberRef(tokens[i]);
            var name = md.ReadString(row.NameIndex);
            if (name != expectedName)
                throw new InvalidDataException(String.Format("MemberRef 0x{0:X8} name {1} != {2}", tokens[i], name, expectedName));
            md.WriteHeapIndex(row.SignatureOffset, md.BlobIndexSize, blobIndex);
        }
    }

    private sealed class MemberRefRow
    {
        internal int Parent;
        internal int NameIndex;
        internal int SignatureIndex;
        internal int NameOffset;
        internal int SignatureOffset;
    }

    private sealed class MetadataLayout
    {
        private readonly byte[] bytes;
        private readonly int stringsOffset;
        private readonly int blobOffset;
        private readonly int[] rows = new int[64];
        private readonly int memberRefParentSize;
        private readonly int memberRefRowSize;
        private readonly int memberRefTableOffset;

        internal int StringIndexSize { get; private set; }
        internal int BlobIndexSize { get; private set; }
        internal int GuidIndexSize { get; private set; }
        internal int MethodDefCount { get { return rows[6]; } }

        internal MetadataLayout(byte[] bytes)
        {
            this.bytes = bytes;
            if (bytes.Length < 0x100 || bytes[0] != (byte)'M' || bytes[1] != (byte)'Z')
                throw new InvalidDataException("Not an MZ image.");

            var pe = ReadInt32LE(bytes, 0x3C);
            if (pe < 0 || pe + 24 >= bytes.Length || bytes[pe] != (byte)'P' || bytes[pe + 1] != (byte)'E' || bytes[pe + 2] != 0 || bytes[pe + 3] != 0)
                throw new InvalidDataException("Invalid PE signature.");

            var optional = pe + 24;
            var magic = ReadUInt16LE(bytes, optional);
            int dataDirectory;
            if (magic == 0x10B) dataDirectory = optional + 96;
            else if (magic == 0x20B) dataDirectory = optional + 112;
            else throw new InvalidDataException(String.Format("Unsupported PE optional-header magic 0x{0:X}", magic));

            var cliRva = ReadInt32LE(bytes, dataDirectory + 14 * 8);
            if (cliRva == 0) throw new InvalidDataException("Missing CLI data directory.");
            var cli = RvaToFileOffset(bytes, pe, cliRva);
            var metadataRva = ReadInt32LE(bytes, cli + 8);
            var metadata = RvaToFileOffset(bytes, pe, metadataRva);
            if (ReadInt32LE(bytes, metadata) != unchecked((int)0x424A5342))
                throw new InvalidDataException("Invalid CLI metadata signature.");

            var versionLength = ReadInt32LE(bytes, metadata + 12);
            var p = metadata + 16 + Align4(versionLength);
            var streamCount = ReadUInt16LE(bytes, p + 2);
            p += 4;

            var streams = new Dictionary<string, int>(StringComparer.Ordinal);
            for (var i = 0; i < streamCount; i++)
            {
                var relativeOffset = ReadInt32LE(bytes, p);
                var nameStart = p + 8;
                var nameEnd = nameStart;
                while (nameEnd < bytes.Length && bytes[nameEnd] != 0) nameEnd++;
                if (nameEnd >= bytes.Length) throw new InvalidDataException("Unterminated metadata stream name.");
                var name = Encoding.ASCII.GetString(bytes, nameStart, nameEnd - nameStart);
                streams[name] = metadata + relativeOffset;
                p = nameStart + Align4((nameEnd - nameStart) + 1);
            }

            int tablesOffset;
            int strings;
            int blobs;
            if (!streams.TryGetValue("#~", out tablesOffset)) throw new InvalidDataException("Missing #~ stream.");
            if (!streams.TryGetValue("#Strings", out strings)) throw new InvalidDataException("Missing #Strings stream.");
            if (!streams.TryGetValue("#Blob", out blobs)) throw new InvalidDataException("Missing #Blob stream.");
            stringsOffset = strings;
            blobOffset = blobs;

            var heapSizes = bytes[tablesOffset + 6];
            var valid = ReadUInt64LE(bytes, tablesOffset + 8);
            var q = tablesOffset + 24;
            for (var table = 0; table < 64; table++)
            {
                if ((valid & (1UL << table)) == 0) continue;
                rows[table] = ReadInt32LE(bytes, q);
                q += 4;
            }

            StringIndexSize = (heapSizes & 0x01) != 0 ? 4 : 2;
            GuidIndexSize = (heapSizes & 0x02) != 0 ? 4 : 2;
            BlobIndexSize = (heapSizes & 0x04) != 0 ? 4 : 2;

            var resolutionScopeSize = CodedIndexSize(rows, new[] { 0, 26, 35, 1 }, 2);
            var typeDefOrRefSize = CodedIndexSize(rows, new[] { 2, 1, 27 }, 2);
            memberRefParentSize = CodedIndexSize(rows, new[] { 2, 1, 26, 6, 27 }, 3);
            memberRefRowSize = memberRefParentSize + StringIndexSize + BlobIndexSize;

            var sizes = new int[10];
            sizes[0] = 2 + StringIndexSize + 3 * GuidIndexSize;
            sizes[1] = resolutionScopeSize + 2 * StringIndexSize;
            sizes[2] = 4 + 2 * StringIndexSize + typeDefOrRefSize + TableIndexSize(rows, 4) + TableIndexSize(rows, 6);
            sizes[3] = TableIndexSize(rows, 4);
            sizes[4] = 2 + StringIndexSize + BlobIndexSize;
            sizes[5] = TableIndexSize(rows, 6);
            sizes[6] = 4 + 2 + 2 + StringIndexSize + BlobIndexSize + TableIndexSize(rows, 8);
            sizes[7] = TableIndexSize(rows, 8);
            sizes[8] = 2 + 2 + StringIndexSize;
            sizes[9] = TableIndexSize(rows, 2) + typeDefOrRefSize;

            var memberRefStart = q;
            for (var table = 0; table < 10; table++)
                memberRefStart += rows[table] * sizes[table];
            memberRefTableOffset = memberRefStart;
        }

        internal MemberRefRow GetMemberRef(int token)
        {
            if ((token & unchecked((int)0xFF000000)) != unchecked((int)0x0A000000))
                throw new ArgumentException(String.Format("Not a MemberRef token: 0x{0:X8}", token));
            var rid = token & 0x00FFFFFF;
            if (rid < 1 || rid > rows[10])
                throw new InvalidDataException("MemberRef RID out of range: " + rid);
            var rowOffset = memberRefTableOffset + (rid - 1) * memberRefRowSize;
            var nameOffset = rowOffset + memberRefParentSize;
            var signatureOffset = nameOffset + StringIndexSize;
            return new MemberRefRow
            {
                Parent = ReadHeapIndex(rowOffset, memberRefParentSize),
                NameIndex = ReadHeapIndex(nameOffset, StringIndexSize),
                SignatureIndex = ReadHeapIndex(signatureOffset, BlobIndexSize),
                NameOffset = nameOffset,
                SignatureOffset = signatureOffset,
            };
        }

        internal string ReadString(int index)
        {
            var start = stringsOffset + index;
            var end = start;
            while (end < bytes.Length && bytes[end] != 0) end++;
            if (end >= bytes.Length) throw new InvalidDataException("Unterminated #Strings entry.");
            return Encoding.UTF8.GetString(bytes, start, end - start);
        }

        internal byte[] ReadBlob(int index)
        {
            var start = blobOffset + index;
            int prefix;
            var length = ReadCompressedUInt(bytes, start, out prefix);
            if (length < 0 || start + prefix + length > bytes.Length)
                throw new InvalidDataException("#Blob entry exceeds file bounds.");
            var result = new byte[length];
            Buffer.BlockCopy(bytes, start + prefix, result, 0, length);
            return result;
        }

        internal void RequireBlob(int index, byte[] expected, string label)
        {
            var actual = ReadBlob(index);
            if (!BytesEqual(actual, expected))
                throw new InvalidDataException(label + " canonical blob mismatch at index 0x" + index.ToString("X"));
        }

        internal int ReadHeapIndex(int offset, int width)
        {
            if (width == 2) return ReadUInt16LE(bytes, offset);
            if (width == 4) return ReadInt32LE(bytes, offset);
            throw new ArgumentOutOfRangeException("width");
        }

        internal void WriteHeapIndex(int offset, int width, int value)
        {
            if (width == 2)
            {
                if (value < 0 || value > UInt16.MaxValue) throw new InvalidDataException("Heap index does not fit 2 bytes: " + value);
                bytes[offset] = (byte)value;
                bytes[offset + 1] = (byte)(value >> 8);
                return;
            }
            if (width == 4)
            {
                WriteInt32LE(bytes, offset, value);
                return;
            }
            throw new ArgumentOutOfRangeException("width");
        }
    }

    private static int Align4(int n) { return (n + 3) & ~3; }

    private static int TableIndexSize(int[] rows, int table)
    {
        return rows[table] < 0x10000 ? 2 : 4;
    }

    private static int CodedIndexSize(int[] rows, int[] tables, int tagBits)
    {
        var max = 0;
        for (var i = 0; i < tables.Length; i++)
            if (rows[tables[i]] > max) max = rows[tables[i]];
        var limit = 1 << (16 - tagBits);
        return max < limit ? 2 : 4;
    }

    private static int ReadCompressedUInt(byte[] bytes, int offset, out int prefix)
    {
        if (offset < 0 || offset >= bytes.Length) throw new InvalidDataException("Compressed integer offset outside file.");
        var a = bytes[offset];
        if ((a & 0x80) == 0)
        {
            prefix = 1;
            return a;
        }
        if ((a & 0xC0) == 0x80)
        {
            if (offset + 1 >= bytes.Length) throw new InvalidDataException("Truncated 2-byte compressed integer.");
            prefix = 2;
            return ((a & 0x3F) << 8) | bytes[offset + 1];
        }
        if ((a & 0xE0) == 0xC0)
        {
            if (offset + 3 >= bytes.Length) throw new InvalidDataException("Truncated 4-byte compressed integer.");
            prefix = 4;
            return ((a & 0x1F) << 24) | (bytes[offset + 1] << 16) | (bytes[offset + 2] << 8) | bytes[offset + 3];
        }
        throw new InvalidDataException(String.Format("Invalid compressed integer lead 0x{0:X2}", a));
    }

    private static byte[] Hex(string text)
    {
        var parts = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var result = new byte[parts.Length];
        for (var i = 0; i < parts.Length; i++) result[i] = Convert.ToByte(parts[i], 16);
        return result;
    }

    private static bool BytesEqual(byte[] a, byte[] b)
    {
        if (a == null || b == null || a.Length != b.Length) return false;
        for (var i = 0; i < a.Length; i++) if (a[i] != b[i]) return false;
        return true;
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

    private static ulong ReadUInt64LE(byte[] bytes, int offset)
    {
        var lo = unchecked((uint)ReadInt32LE(bytes, offset));
        var hi = unchecked((uint)ReadInt32LE(bytes, offset + 4));
        return ((ulong)lo) | ((ulong)hi << 32);
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
        throw new InvalidDataException(String.Format("RVA 0x{0:X} outside all PE sections.", rva));
    }

    private static string Sha256(byte[] bytes)
    {
        using (var sha = SHA256.Create())
            return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
    }
}
