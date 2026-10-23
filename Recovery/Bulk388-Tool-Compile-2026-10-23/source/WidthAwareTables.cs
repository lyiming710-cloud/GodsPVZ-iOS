// Source proposal only. Not compiled or exercised by the author.
// Re-encode metadata columns explicitly when an appended heap crosses 64 KiB.
using System.Buffers.Binary;
using System.Reflection.Metadata.Ecma335;

internal static partial class Program
{
    private sealed class WidthAwareTables
    {
        private readonly record struct Column(char Kind, int Table = 0, string? Group = null);
        private static Column U2 => new('2');
        private static Column U4 => new('4');
        private static Column Str => new('S');
        private static Column Blob => new('B');
        private static Column Guid => new('G');
        private static Column T(int table) => new('T', table);
        private static Column C(string group) => new('C', 0, group);

        // ECMA-335 II.22 tables; table RID identities are preserved.
        private static readonly Column[][] Schemas =
        [
            [U2,Str,Guid,Guid,Guid],                       // 00 Module
            [C("ResolutionScope"),Str,Str],               // 01 TypeRef
            [U4,Str,Str,C("TypeDefOrRef"),T(4),T(6)],      // 02 TypeDef
            [T(4)],                                      // 03 FieldPtr
            [U2,Str,Blob],                                // 04 Field
            [T(6)],                                      // 05 MethodPtr
            [U4,U2,U2,Str,Blob,T(8)],                     // 06 MethodDef
            [T(8)],                                      // 07 ParamPtr
            [U2,U2,Str],                                 // 08 Param
            [T(2),C("TypeDefOrRef")],                    // 09 InterfaceImpl
            [C("MemberRefParent"),Str,Blob],             // 10 MemberRef
            [U2,C("HasConstant"),Blob],                  // 11 Constant, including reserved padding byte
            [C("HasCustomAttribute"),C("CustomAttributeType"),Blob], // 12
            [C("HasFieldMarshal"),Blob],                 // 13 FieldMarshal
            [U2,C("HasDeclSecurity"),Blob],              // 14 DeclSecurity
            [U2,U4,T(2)],                                // 15 ClassLayout
            [U4,T(4)],                                   // 16 FieldLayout
            [Blob],                                     // 17 StandAloneSig
            [T(2),T(20)],                                // 18 EventMap
            [T(20)],                                    // 19 EventPtr
            [U2,Str,C("TypeDefOrRef")],                  // 20 Event
            [T(2),T(23)],                                // 21 PropertyMap
            [T(23)],                                    // 22 PropertyPtr
            [U2,Str,Blob],                               // 23 Property
            [U2,T(6),C("HasSemantics")],                 // 24 MethodSemantics
            [T(2),C("MethodDefOrRef"),C("MethodDefOrRef")], // 25 MethodImpl
            [Str],                                      // 26 ModuleRef
            [Blob],                                     // 27 TypeSpec
            [U2,C("MemberForwarded"),Str,T(26)],         // 28 ImplMap
            [U4,T(4)],                                   // 29 FieldRVA
            [U4,U4],                                    // 30 ENCLog
            [U4],                                       // 31 ENCMap
            [U4,U2,U2,U2,U2,U4,Blob,Str,Str],            // 32 Assembly
            [U4],                                       // 33 AssemblyProcessor
            [U4,U4,U4],                                 // 34 AssemblyOS
            [U2,U2,U2,U2,U4,Blob,Str,Str,Blob],           // 35 AssemblyRef
            [U4,T(35)],                                  // 36 AssemblyRefProcessor
            [U4,U4,U4,T(35)],                            // 37 AssemblyRefOS
            [U4,Str,Blob],                               // 38 File
            [U4,U4,Str,Str,C("Implementation")],         // 39 ExportedType
            [U4,U4,Str,C("Implementation")],             // 40 ManifestResource
            [T(2),T(2)],                                 // 41 NestedClass
            [U2,U2,C("TypeOrMethodDef"),Str],            // 42 GenericParam
            [C("MethodDefOrRef"),Blob],                  // 43 MethodSpec
            [T(42),C("TypeDefOrRef")]                    // 44 GenericParamConstraint
        ];
        private static readonly Dictionary<string,(int Bits,int[] Tables)> Coded = new()
        {
            ["TypeDefOrRef"]=(2,[2,1,27]),
            ["HasConstant"]=(2,[4,8,23]),
            ["HasCustomAttribute"]=(5,[6,4,1,2,8,9,10,0,14,23,20,17,26,27,32,35,38,39,40,42,44,43]),
            ["HasFieldMarshal"]=(1,[4,8]),
            ["HasDeclSecurity"]=(2,[2,6,32]),
            ["MemberRefParent"]=(3,[2,1,26,6,27]),
            ["HasSemantics"]=(1,[20,23]),
            ["MethodDefOrRef"]=(1,[6,10]),
            ["MemberForwarded"]=(1,[4,6]),
            ["Implementation"]=(2,[38,35,39]),
            ["CustomAttributeType"]=(3,[6,10]), // legal tag values are 2 and 3
            ["ResolutionScope"]=(2,[0,26,35,1]),
            ["TypeOrMethodDef"]=(1,[2,6])
        };
        private readonly byte[] header;
        private readonly byte[] trailing;
        private readonly ulong valid;
        private readonly ulong sorted;
        private readonly byte oldHeapFlags;
        private readonly int[] oldCounts = new int[64];
        private readonly List<uint[]>[] rows = Enumerable.Range(0,64).Select(_=>new List<uint[]>()).ToArray();
        public int OldCount(int table) => oldCounts[table];
        public uint[] OriginalRow(int table,int rid)
        {
            Require(rid>0 && rid<=oldCounts[table],"Existing metadata row only");
            return (uint[])rows[table][rid-1].Clone();
        }
        private static int Width(Column c,byte heapFlags,int[] counts) => c.Kind switch
        {
            '2'=>2, '4'=>4,
            'S'=>(heapFlags&1)==0?2:4,
            'G'=>(heapFlags&2)==0?2:4,
            'B'=>(heapFlags&4)==0?2:4,
            'T'=>counts[c.Table]<65536?2:4,
            'C'=>Coded[c.Group!].Tables.Max(t=>counts[t])<(1<<(16-Coded[c.Group!].Bits))?2:4,
            _=>throw new InvalidOperationException("Unknown metadata column")
        };
        public WidthAwareTables(byte[] payload,System.Reflection.Metadata.MetadataReader md)
        {
            Require(payload.Length>=24,"Bounded table header");
            header=payload[..24];oldHeapFlags=header[6];
            Require((oldHeapFlags&0xF8)==0,"Explicit optimized metadata heaps; no delta/ExtraData format");
            valid=BinaryPrimitives.ReadUInt64LittleEndian(header.AsSpan(8,8));
            sorted=BinaryPrimitives.ReadUInt64LittleEndian(header.AsSpan(16,8));
            Require((valid>>45)==0,"Only explicitly modeled ECMA assembly tables");
            Require(new[]{3,5,7,19,22}.All(t=>(valid&(1UL<<t))==0),"Optimized #~ stream without pointer tables");
            int pos=24;
            for(int t=0;t<64;t++)
            {
                if((valid&(1UL<<t))==0)continue;
                Require(pos<=payload.Length-4,"Bounded row-count header");
                int count=Read32(payload,pos);pos=checked(pos+4);
                Require(count>=0 && count==md.GetTableRowCount((TableIndex)t),"Original row count");
                oldCounts[t]=count;
            }
            for(int t=0;t<Schemas.Length;t++)
            {
                if((valid&(1UL<<t))==0)continue;
                int rowSize=Schemas[t].Sum(c=>Width(c,oldHeapFlags,oldCounts));
                Require(rowSize==md.GetTableRowSize((TableIndex)t),"Original schema row width: "+t);
                Require(checked((long)oldCounts[t]*rowSize)<=payload.Length-pos,"Bounded original rows");
                for(int r=0;r<oldCounts[t];r++)
                {
                    uint[] values=new uint[Schemas[t].Length];
                    for(int c=0;c<values.Length;c++)
                    {
                        int width=Width(Schemas[t][c],oldHeapFlags,oldCounts);
                        values[c]=width==2?BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(pos,2)):
                            BinaryPrimitives.ReadUInt32LittleEndian(payload.AsSpan(pos,4));
                        pos=checked(pos+width);
                    }
                    rows[t].Add(values);
                }
            }
            trailing=payload[pos..];
        }
        public void Append(int table,int expectedToken,params uint[] values)
        {
            Require(table>=0 && table<Schemas.Length && (valid&(1UL<<table))!=0,"Append only to an existing modeled table");
            Require((sorted&(1UL<<table))==0,"No appending to a sorted table without a proved order");
            Require(values.Length==Schemas[table].Length,"New row column count");
            Require(expectedToken==checked((table<<24)+rows[table].Count+1),"Append-only token identity");
            rows[table].Add((uint[])values.Clone());
        }
        public byte[] Write(int stringHeapBytes,int blobHeapBytes,int guidHeapBytes,out object evidence)
        {
            Require(stringHeapBytes>=0 && blobHeapBytes>=0 && guidHeapBytes>=0,"Nonnegative heap extents");
            // Widen only; do not normalize/shrink historical index widths.
            byte newFlags=(byte)(oldHeapFlags|(stringHeapBytes>=65536?1:0)|(blobHeapBytes>=65536?4:0)|(guidHeapBytes>=65536?2:0));
            int[] counts=rows.Select(r=>r.Count).ToArray();
            byte[] newHeader=(byte[])header.Clone();newHeader[6]=newFlags;
            using var output=new MemoryStream();output.Write(newHeader);
            void Put(uint value,int width)
            {
                byte[] bytes=new byte[width];
                if(width==2)BinaryPrimitives.WriteUInt16LittleEndian(bytes,checked((ushort)value));
                else BinaryPrimitives.WriteUInt32LittleEndian(bytes,value);
                output.Write(bytes);
            }
            for(int t=0;t<64;t++)if((valid&(1UL<<t))!=0)Put(checked((uint)counts[t]),4);
            var changes=new List<object>();
            for(int t=0;t<Schemas.Length;t++)
            {
                if((valid&(1UL<<t))==0)continue;
                int oldWidth=Schemas[t].Sum(c=>Width(c,oldHeapFlags,oldCounts));
                int newWidth=Schemas[t].Sum(c=>Width(c,newFlags,counts));
                if(oldWidth!=newWidth || counts[t]!=oldCounts[t])
                    changes.Add(new{table=t,old_rows=oldCounts[t],new_rows=counts[t],old_row_bytes=oldWidth,new_row_bytes=newWidth});
                foreach(uint[] values in rows[t])
                    for(int c=0;c<values.Length;c++)Put(values[c],Width(Schemas[t][c],newFlags,counts));
            }
            output.Write(trailing);
            evidence=new{old_heap_flags=oldHeapFlags,new_heap_flags=newFlags,changes,old_tokens_preserved=true,
                strategy="Decode every modeled old column, append pinned rows, re-encode all rows at their resulting widths"};
            return output.ToArray();
        }
    }
}
