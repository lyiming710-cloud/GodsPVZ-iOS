using Mono.Cecil;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using MD=Mono.Cecil.MethodDefinition;

static class Patcher63 {
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
        public void Store(byte index)=>Emit(0x13,index);public void Address(byte index)=>Emit(0x12,index);
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
            Require(Hash(input)=="b87b8139225cc4eabf86384bc82f9bfa37d7e5aa74ffbead7f7d4100fa2122c0","Pinned untested Native62 input");
            var result=(byte[])input.Clone();
            using var assembly=AssemblyDefinition.ReadAssembly(args[0]);
            using var pe=new PEReader(new MemoryStream(input));
            var module=assembly.MainModule;
            var rows=new List<object>();
            foreach(int token in new[]{0x060001C4,0x060003F1}) {
                var method=(MD)module.LookupToken(token);
                Require(method.HasThis&&method.Body.ExceptionHandlers.Count==0,"Target shape");
                FieldReference Field(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
                MethodReference Call(string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>().First(m=>m.Name==name);
                var il=new Code();
                void LoadField(string owner,string name) { il.Emit(0x02);il.Ref(0x7b,Field(owner,name)); }
                MethodReference Exact(string name)=>module.GetMemberReferences().OfType<MethodReference>().Single(m=>m.FullName==name);
                FieldReference Vec(string part)=>module.GetMemberReferences().OfType<FieldReference>().Single(f=>f.FullName=="System.Single UnityEngine.Vector3::"+part);
                var componentTransform=Exact("UnityEngine.Transform UnityEngine.Component::get_transform()");
                var gameObjectTransform=Exact("UnityEngine.Transform UnityEngine.GameObject::get_transform()");
                int max=token==0x060001C4?4:2;
                switch(token) {
                    case 0x060001C4:
                        Require(method.Body.Variables[6].VariableType.FullName=="UnityEngine.Vector3","Existing preview vector local");
                        LoadField("MouseManager","devicePrefabPreview");il.Emit(0x14);il.Ref(0x28,Call("op_Inequality"));il.Branch(0x39,"return");
                        LoadField("MouseManager","devicePrefabOnHand");LoadField("MouseManager","GridX");LoadField("MouseManager","GridY");
                        il.Ref(0x6f,Call("TestPlacing"));il.Branch(0x39,"inactive");
                        LoadField("MouseManager","devicePrefabPreview");il.Ref(0x6f,Call("get_gameObject"));il.Emit(0x17);il.Ref(0x6f,Call("SetActive"));
                        LoadField("MouseManager","board");il.Ref(0x7b,Field("Board","boardConfig"));
                        LoadField("MouseManager","GridX");LoadField("MouseManager","GridY");il.Ref(0x6f,Call("GetGridPosition"));il.Store(6);
                        // Capture preview before both adds, then preserve Vector3.z exactly.
                        LoadField("MouseManager","devicePrefabPreview");
                        il.Address(6);il.Load(6);il.Ref(0x7b,Vec("y"));il.Float(44);il.Emit(0x58);il.Ref(0x7d,Vec("y"));
                        il.Address(6);il.Load(6);il.Ref(0x7b,Vec("x"));il.Float(57);il.Emit(0x58);il.Ref(0x7d,Vec("x"));
                        il.Ref(0x6f,componentTransform);il.Load(6);il.Ref(0x6f,Call("set_position"));il.Branch(0x38,"return");
                        il.Label("inactive");LoadField("MouseManager","devicePrefabPreview");il.Ref(0x6f,Call("get_gameObject"));il.Emit(0x16);il.Ref(0x6f,Call("SetActive"));
                        il.Label("return");il.Emit(0x2a);
                        break;
                    case 0x060003F1:
                        foreach(int local in new[]{2,6,10}) Require(method.Body.Variables[local].VariableType.FullName=="UnityEngine.Vector3","Existing projectile vectors");
                        il.Emit(0x02);il.Ref(0x6f,componentTransform);il.Ref(0x6f,Call("get_position"));il.Store(2);
                        // fX/fY are captured before the second transform getter.
                        il.Address(2);LoadField("Projectile","fX");il.Ref(0x7d,Vec("x"));
                        il.Address(2);LoadField("Projectile","fY");il.Ref(0x7d,Vec("y"));
                        il.Emit(0x02);il.Ref(0x6f,componentTransform);il.Load(2);il.Ref(0x6f,Call("set_position"));
                        foreach(var(field,local,next) in new[]{("projectileSprite",(byte)6,"animation"),("projectileAnimation",(byte)10,"return")}) {
                            LoadField("Projectile",field);il.Emit(0x14);il.Ref(0x28,Call("op_Inequality"));il.Branch(0x39,next);
                            LoadField("Projectile",field);il.Ref(0x6f,gameObjectTransform);il.Ref(0x6f,Call("get_localPosition"));il.Store(local);
                            il.Address(local);LoadField("Projectile","fZ");il.Ref(0x7d,Vec("y"));
                            LoadField("Projectile",field);il.Ref(0x6f,gameObjectTransform);il.Load(local);il.Ref(0x6f,Call("set_localPosition"));
                            il.Label(next);
                        }
                        il.Emit(0x2a);
                        break;
                }
                var code=il.Finish();
                Require(code.Length<=method.Body.CodeSize,"Existing body slot: "+method.FullName);
                int header=Offset(pe,method.RVA);
                Require((input[header]&3)==3&&(BitConverter.ToUInt16(input,header)>>12)==3,"Fat method header");
                Array.Clear(result,header+12,method.Body.CodeSize);
                code.CopyTo(result,header+12);
                BitConverter.GetBytes((ushort)max).CopyTo(result,header+2);
                BitConverter.GetBytes(code.Length).CopyTo(result,header+4);
                rows.Add(new{token=$"0x{token:X8}",method=method.FullName,header,reserved_code_size=method.Body.CodeSize,new_code_size=code.Length,old_max=method.Body.MaxStackSize,new_max=max,old_local_sig=method.Body.LocalVarToken.ToInt32(),local_sig=method.Body.LocalVarToken.ToInt32(),code_sha256=Hash(code)});
            }
            File.WriteAllBytes(args[1],result);
            File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=Hash(input),candidate_sha256=Hash(result),methods=rows},new JsonSerializerOptions{WriteIndented=true}));
            Console.WriteLine(Hash(result));return 0;
        } catch(Exception error) { Console.Error.WriteLine(error);return 3; }
    }
}
