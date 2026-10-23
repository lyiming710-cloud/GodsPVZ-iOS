// Source proposal only: old Bulk350 metadata appender remains a separate, unchanged stage.
using System.Buffers.Binary;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

internal static partial class Program
{
    private static byte[] AppendNative119WidthAware(byte[] before,out object evidence)
    {
        using var pe=new PEReader(new MemoryStream(before,false));
        var h=pe.PEHeaders;var md=pe.GetMetadataReader();
        Require(md.GetTableRowCount(TableIndex.MemberRef)==1280 &&
            md.GetTableRowCount(TableIndex.StandAloneSig)==2058 &&
            md.GetTableRowCount(TableIndex.TypeSpec)==178 &&
            md.GetTableRowCount(TableIndex.MethodSpec)==193 &&
            md.GetTableRowCount(TableIndex.TypeRef)==177,"Exact Bulk350 R6 metadata-stage counts");
        int rootAt=FileOffset(h,h.CorHeader!.MetadataDirectory.RelativeVirtualAddress);
        int rootBytes=h.CorHeader.MetadataDirectory.Size;
        byte[] old=before[rootAt..checked(rootAt+rootBytes)];
        Require(Read32(old,0)==0x424A5342,"Metadata root signature");
        int p=Align(checked(16+Read32(old,12)),4);
        int streamCount=BinaryPrimitives.ReadUInt16LittleEndian(old.AsSpan(p+2,2));p+=4;
        var streams=new List<(int Header,string Name,int Offset,int Size)>();
        for(int i=0;i<streamCount;i++)
        {
            Require(p<=old.Length-8,"Bounded stream header");
            int end=Array.IndexOf(old,(byte)0,p+8);Require(end>=p+8,"Terminated stream name");
            streams.Add((p,System.Text.Encoding.UTF8.GetString(old,p+8,end-p-8),Read32(old,p),Read32(old,p+4)));
            p=Align(end+1,4);
        }
        Require(streams.Select(s=>s.Name).Distinct().Count()==streamCount &&
            streams.All(s=>s.Size>=0 && s.Offset>=p && s.Offset<=old.Length-s.Size),"Unique bounded streams");
        var ordered=streams.OrderBy(s=>s.Offset).ToArray();
        for(int i=1;i<ordered.Length;i++)Require(ordered[i-1].Offset+ordered[i-1].Size<=ordered[i].Offset,"No overlapping streams");
        var payloads=streams.ToDictionary(s=>s.Name,s=>old[s.Offset..checked(s.Offset+s.Size)]);
        Require(payloads.ContainsKey("#~") && !payloads.ContainsKey("#-"),"Explicit optimized table path");
        var tables=new WidthAwareTables(payloads["#~"],md);
        var blobBytes=new List<byte>(payloads["#Blob"]);
        var nameBytes=new List<byte>(payloads["#Strings"]);
        var userBytes=new List<byte>(payloads["#US"]);
        Require(blobBytes.Count==65075,"Pinned Bulk350 R6 blob extent; retain old no-widening stage");
        uint AddBlob(string hex)
        {
            byte[] data=Convert.FromHexString(hex);Require(data.Length<0x20000000,"Bounded compressed blob length");
            int at=blobBytes.Count;var b=new BlobBuilder();b.WriteCompressedInteger(data.Length);b.WriteBytes(data);
            blobBytes.AddRange(b.ToArray());return checked((uint)at);
        }
        uint AddName(string value)
        {
            Require(!value.Contains('\0'),"No embedded null in a metadata name");
            int at=nameBytes.Count;nameBytes.AddRange(System.Text.Encoding.UTF8.GetBytes(value));nameBytes.Add(0);
            return checked((uint)at);
        }
        string TypeRefName(int rid)
        {
            if(rid<=tables.OldCount(1))
            {
                var t=md.GetTypeReference(MetadataTokens.TypeReferenceHandle(rid));
                return md.GetString(t.Namespace)+"."+md.GetString(t.Name);
            }
            var added=N119NewTypeRefs.Single(t=>(t.Token&0xFFFFFF)==rid);
            return added.Namespace+"."+added.Name;
        }
        string TypeSpecBlob(int rid)
        {
            if(rid<=tables.OldCount(27))
                return Convert.ToHexString(md.GetBlobBytes(md.GetTypeSpecification(MetadataTokens.TypeSpecificationHandle(rid)).Signature)).ToLowerInvariant();
            return N119NewTypeSpecs.Single(t=>(t.Token&0xFFFFFF)==rid).Blob;
        }
        foreach(var t in N119NewTypeRefs)
        {
            Require(t.ScopeTable==35 && t.ScopeRid>0 && t.ScopeRid<=md.GetTableRowCount(TableIndex.AssemblyRef),"Explicit AssemblyRef scope");
            var assembly=md.GetAssemblyReference(MetadataTokens.AssemblyReferenceHandle(t.ScopeRid));
            Require(md.GetString(assembly.Name)==t.ScopeName,"Pinned new TypeRef assembly name");
            tables.Append(1,t.Token,checked((uint)((t.ScopeRid<<2)|2)),AddName(t.Name),AddName(t.Namespace));
        }
        foreach(var t in N119NewTypeSpecs)tables.Append(27,t.Token,AddBlob(t.Blob));
        foreach(var d in N119NewMembers)
        {
            var template=md.GetMemberReference(MetadataTokens.MemberReferenceHandle(d.Template));
            Require(md.GetString(template.Name)==d.TemplateName &&
                md.GetBlobBytes(template.Signature).SequenceEqual(Convert.FromHexString(d.TemplateSignature)),"Pinned original template identity");
            int tag=d.ClassCode&7,rid=d.ClassCode>>3;
            Require(rid>0 && (tag==1 || tag==4),"Explicit MemberRefParent kind");
            Require((tag==1?TypeRefName(rid):TypeSpecBlob(rid))==d.OwnerPin,"Exact old or appended declaring type");
            string name=d.NewName??d.TemplateName;
            uint nameAt=d.NewName is null?checked((uint)MetadataTokens.GetHeapOffset(template.Name)):AddName(name);
            // Signatures are explicit new payloads, not assumed equal to the template's signature.
            tables.Append(10,d.Token,checked((uint)d.ClassCode),nameAt,AddBlob(d.Signature));
        }
        foreach(var local in N119NewLocals)
        {
            byte[] b=Convert.FromHexString(local.Blob);Require(b.Length>=2 && b[0]==7,"LOCAL_SIG calling convention");
            tables.Append(17,local.Token,AddBlob(local.Blob));
        }
        foreach(var entry in N119NewUserStrings)
        {
            Require(userBytes.Count<0x1000000 && entry.Token==0x70000000+userBytes.Count,"Append-only Native119 string token");
            var b=new BlobBuilder();b.WriteUserString(entry.Value);byte[] encoded=b.ToArray();
            Require(encoded.SequenceEqual(Convert.FromHexString(entry.EncodedBytes)),"Pinned UTF16 payload, length and terminal byte");
            Require(checked(userBytes.Count+encoded.Length)<=0x1000000,"24-bit user-string token extent");
            userBytes.AddRange(encoded);
        }
        byte[] tableBytes=tables.Write(nameBytes.Count,blobBytes.Count,payloads["#GUID"].Length,out var tableEvidence);
        payloads["#~"]=tableBytes;payloads["#Blob"]=blobBytes.ToArray();
        payloads["#Strings"]=nameBytes.ToArray();payloads["#US"]=userBytes.ToArray();
        using var rootStream=new MemoryStream();rootStream.Write(old,0,ordered[0].Offset);
        var newHeaders=new List<(int At,int Offset,int Size)>();
        for(int i=0;i<ordered.Length;i++)
        {
            var s=ordered[i];while(rootStream.Position%4!=0)rootStream.WriteByte(0);
            newHeaders.Add((s.Header,checked((int)rootStream.Position),payloads[s.Name].Length));rootStream.Write(payloads[s.Name]);
            int oldEnd=s.Offset+s.Size,next=i+1<ordered.Length?ordered[i+1].Offset:old.Length;
            rootStream.Write(old,oldEnd,next-oldEnd);
        }
        while(rootStream.Position%4!=0)rootStream.WriteByte(0);
        byte[] root=rootStream.ToArray();
        foreach(var s in newHeaders){Write32(root,s.At,s.Offset);Write32(root,s.At+4,s.Size);}
        int at=Align(before.Length,4),meaningfulEnd=checked(at+root.Length);
        byte[] image=new byte[Align(meaningfulEnd,h.PEHeader!.FileAlignment)];before.CopyTo(image,0);root.CopyTo(image,at);
        ExtendReadonlySection(image,h,meaningfulEnd);var section=h.SectionHeaders.Last();
        Write32(image,h.CorHeaderStartOffset+8,checked(section.VirtualAddress+at-section.PointerToRawData));
        Write32(image,h.CorHeaderStartOffset+12,root.Length);
        // Construction guard only; independent validators must also compare decoded values.
        using(var reopenedPe=new PEReader(new MemoryStream(image,false)))
        {
            var reopened=reopenedPe.GetMetadataReader();
            Require(reopened.GetTableRowCount(TableIndex.MethodDef)==2317 &&
                reopened.GetTableRowCount(TableIndex.TypeRef)==178 &&
                reopened.GetTableRowCount(TableIndex.MemberRef)==1318 &&
                reopened.GetTableRowCount(TableIndex.StandAloneSig)==2095 &&
                reopened.GetTableRowCount(TableIndex.TypeSpec)==181 &&
                reopened.GetTableRowCount(TableIndex.MethodSpec)==193,"Reopened append counts");
            int streamOffset=newHeaders.Single(s=>s.At==streams.Single(x=>x.Name=="#~").Header).Offset;
            var again=new WidthAwareTables(root[streamOffset..checked(streamOffset+tableBytes.Length)],reopened);
            for(int t=0;t<=44;t++)
                for(int rid=1;rid<=tables.OldCount(t);rid++)
                    Require(tables.OriginalRow(t,rid).SequenceEqual(again.OriginalRow(t,rid)),"Preserved preexisting numeric columns: "+t+":"+rid);
        }
        evidence=new{state="UNTESTED_CONSTRUCTION_ONLY",old_root_bytes=old.Length,new_root_bytes=root.Length,
            old_blob_bytes=65075,new_blob_bytes=blobBytes.Count,new_string_bytes=nameBytes.Count,
            appended_typerefs=N119NewTypeRefs.Length,appended_memberrefs=N119NewMembers.Length,
            appended_local_signatures=N119NewLocals.Length,appended_typespecs=N119NewTypeSpecs.Length,
            appended_user_strings=N119NewUserStrings.Length,original_heap_prefixes_preserved=true,
            original_metadata_slot_preserved=true,table_widths=tableEvidence};
        return image;
    }
}
