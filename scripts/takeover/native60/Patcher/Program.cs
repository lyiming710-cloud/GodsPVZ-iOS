using Mono.Cecil;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using MD=Mono.Cecil.MethodDefinition;

static class Patcher60 {
    static void Require(bool ok,string message) { if(!ok) throw new Exception(message); }
    static string Hash(byte[] data) => Convert.ToHexString(SHA256.HashData(data)).ToLowerInvariant();
    static int Offset(PEReader pe,int rva) {
        var s=pe.PEHeaders.SectionHeaders.Single(s=>rva>=s.VirtualAddress&&rva<s.VirtualAddress+s.SizeOfRawData);
        return s.PointerToRawData+rva-s.VirtualAddress;
    }
    sealed class Code {
        public List<byte> bytes=new();
        Dictionary<string,int> labels=new();
        List<(int position,string target)> branches=new();
        public void Emit(params byte[] b)=>bytes.AddRange(b);
        public void Ref(byte op,IMetadataTokenProvider reference) { Emit(op);bytes.AddRange(BitConverter.GetBytes(reference.MetadataToken.ToInt32())); }
        public void Float(float value) { Emit(0x22);bytes.AddRange(BitConverter.GetBytes(value)); }
        public void Load(byte index)=>Emit(0x11,index);
        public void Store(byte index)=>Emit(0x13,index);
        public void Label(string label)=>labels.Add(label,bytes.Count);
        public void Branch(byte longOpcode,string target) { Emit(longOpcode);branches.Add((bytes.Count,target));bytes.AddRange(new byte[4]); }
        public byte[] Finish() {
            var result=bytes.ToArray();
            foreach(var(position,target) in branches) BitConverter.GetBytes(labels[target]-position-4).CopyTo(result,position);
            return result;
        }
    }
    static int Main(string[] args) {
        try {
            Require(args.Length==3&&!File.Exists(args[1])&&!File.Exists(args[2]),"Fresh output paths are required");
            var input=File.ReadAllBytes(args[0]);
            Require(Hash(input)=="d198bb3f63a21d95d708a66546b3b12529d52aee810508712db6e32f43b021c0","Pinned Native59 input");
            var result=(byte[])input.Clone();
            using var assembly=AssemblyDefinition.ReadAssembly(args[0]);
            using var pe=new PEReader(new MemoryStream(input));
            var module=assembly.MainModule;
            var rows=new List<object>();
            foreach(int token in new[]{0x0600046D,0x060005B9}) {
                var method=(MD)module.LookupToken(token);
                Require(method.HasThis&&method.Body.ExceptionHandlers.Count==0,"Target shape");
                FieldReference Field(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
                MethodReference Call(string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>().First(m=>m.Name==name);
                var il=new Code();
                void LoadField(string owner,string name) { il.Emit(0x02);il.Ref(0x7b,Field(owner,name)); }
                switch(token) {
                    case 0x0600046D:
                        il.Emit(0x02);il.Ref(0x6f,Call("IsDisabled"));il.Branch(0x3a,"return");
                        LoadField("Zombie","elementManager");il.Emit(0x16);il.Ref(0x6f,Call("GetElement"));il.Store(4);
                        il.Load(4);il.Branch(0x39,"zero-point");
                        il.Load(4);il.Ref(0x7b,Field("Element","point"));il.Store(5);il.Branch(0x38,"point-ready");
                        il.Label("zero-point");il.Float(0);il.Store(5);
                        il.Label("point-ready");LoadField("Zombie","board");il.Store(7);
                        il.Load(5);il.Float(0);il.Branch(0x3d,"positive-point");
                        il.Load(7);il.Ref(0x7b,Field("Board","zombieManager"));il.Ref(0x7b,Field("ZombieManager","audioClip_zombie"));
                        il.Emit(0x1e,0x1f,10);il.Ref(0x28,Call("Range"));il.Ref(0x6f,Call("get_Item"));il.Branch(0x38,"clip-ready");
                        il.Label("positive-point");il.Load(7);il.Ref(0x7b,Field("Board","zombieManager"));il.Ref(0x7b,Field("ZombieManager","audioClip_zombie"));
                        il.Emit(0x1f,10);il.Ref(0x6f,Call("get_Item"));
                        il.Label("clip-ready");il.Ref(0x28,Call("get_main"));il.Ref(0x6f,Call("get_transform"));il.Ref(0x6f,Call("get_position"));
                        il.Ref(0x28,Call("AudioVolume"));il.Ref(0x28,Call("CreateAudioAtPoint"));il.Emit(0x26);
                        il.Label("return");il.Emit(0x2a);
                        break;
                    case 0x060005B9:
                        il.Emit(0x02,0x04);il.Ref(0x7d,Field("Almanac_ZombieWindow","zombieID"));
                        il.Emit(0x02,0x03);il.Ref(0x7b,Field("Save","almanac_ZombieLock"));il.Emit(0x04,0x91);il.Ref(0x6f,Call("SetState"));
                        il.Emit(0x04);il.Ref(0x28,Call("Load_zombieInfo"));il.Store(7);
                        il.Emit(0x02);il.Load(7);il.Ref(0x7d,Field("Almanac_ZombieWindow","zombieInfo"));
                        il.Load(7);il.Branch(0x39,"return");
                        LoadField("Almanac_ZombieWindow","eliteMark");il.Ref(0x6f,Call("get_gameObject"));
                        il.Load(7);il.Ref(0x7b,Field("ZombieInfo","isElite"));il.Ref(0x6f,Call("SetActive"));
                        LoadField("Almanac_ZombieWindow","bossMark");il.Ref(0x6f,Call("get_gameObject"));
                        il.Load(7);il.Ref(0x7b,Field("ZombieInfo","isBoss"));il.Ref(0x6f,Call("SetActive"));
                        il.Label("return");il.Emit(0x2a);
                        break;
                }
                var code=il.Finish();
                Require(code.Length<=method.Body.CodeSize,"Existing body slot: "+method.FullName);
                int header=Offset(pe,method.RVA);
                Require((input[header]&3)==3&&(BitConverter.ToUInt16(input,header)>>12)==3,"Fat method header");
                Array.Clear(result,header+12,method.Body.CodeSize);
                code.CopyTo(result,header+12);
                BitConverter.GetBytes((ushort)3).CopyTo(result,header+2);
                BitConverter.GetBytes(code.Length).CopyTo(result,header+4);
                rows.Add(new{token=$"0x{token:X8}",method=method.FullName,header,reserved_code_size=method.Body.CodeSize,new_code_size=code.Length,old_max=method.Body.MaxStackSize,new_max=3,old_local_sig=method.Body.LocalVarToken.ToInt32(),local_sig=method.Body.LocalVarToken.ToInt32(),code_sha256=Hash(code)});
            }
            File.WriteAllBytes(args[1],result);
            File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=Hash(input),candidate_sha256=Hash(result),methods=rows},new JsonSerializerOptions{WriteIndented=true}));
            Console.WriteLine(Hash(result));return 0;
        } catch(Exception error) { Console.Error.WriteLine(error);return 3; }
    }
}
