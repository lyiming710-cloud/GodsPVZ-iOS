using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
static class RepairNative27 {
 static void R(bool b,string s){if(!b)throw new InvalidOperationException(s);}
 static string H(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
 static readonly (int Token,int Null,int Throw)[] Sites=new[]{(0x0600087E,45,38),(0x0600087F,45,38),(0x060008AB,4,65),(0x060008AD,4,65),(0x060008AF,4,65),(0x060008B0,4,65),(0x060008B2,4,65),(0x060008B4,4,65),(0x060008B8,4,65),(0x060008B1,4,69),(0x060008B3,4,69),(0x060008B5,4,69)};
 static int Main(string[] args){try{Run(args);return 0;}catch(Exception e){Console.Error.WriteLine("REJECT: "+e.Message);return 3;}}
 static void Run(string[] a){
  R(a.Length==3,"usage input new-output report");var input=Path.GetFullPath(a[0]);var output=Path.GetFullPath(a[1]);var reportPath=Path.GetFullPath(a[2]);R(input!=output&&!File.Exists(output),"New distinct output required");R(reportPath!=input&&reportPath!=output&&reportPath!=output+".tmp"&&!File.Exists(reportPath),"New distinct report required");var before=File.ReadAllBytes(input);R(H(before)=="affd9276454e70d6f39d9e28144ba2006021e818b1a617ed517eea90741ca878","Native26 parent hash mismatch");
  using var asm=AssemblyDefinition.ReadAssembly(input);using var fs=File.OpenRead(input);using var pe=new PEReader(fs);var after=(byte[])before.Clone();var allowed=new HashSet<int>();var records=new List<object>();
  foreach(var (token,n,t) in Sites){
   var m=(MethodDefinition)asm.MainModule.LookupToken(token);R(m.HasBody&&m.Body.ExceptionHandlers.Count==0,"Unsupported body");var z=m.Body.Instructions.Single(i=>i.Offset==n);var ret=m.Body.Instructions.Single(i=>i.Offset==t);
   R(z.OpCode==OpCodes.Ldc_I4&&z.Operand is int value&&value==0&&z.GetSize()==5,"Pinned zero mismatch");R(z.Next.OpCode==OpCodes.Ceq,"Pinned comparison mismatch");R(ret.OpCode==OpCodes.Ret,"Pinned exception exit mismatch");var load=ret.Previous;var store=load.Previous;var ctor=store.Previous;
   R(load.OpCode==OpCodes.Ldloc&&store.OpCode==OpCodes.Stloc&&load.Operand==store.Operand&&((VariableDefinition)load.Operand).VariableType.FullName=="System.NullReferenceException"&&ctor.OpCode==OpCodes.Newobj&&((MethodReference)ctor.Operand).FullName=="System.Void System.NullReferenceException::.ctor()","Exception producer mismatch");
   var interior=new HashSet<int>{n+1,n+2,n+3,n+4,store.Offset,load.Offset,t};foreach(var i in m.Body.Instructions){if(i.Operand is Instruction j)R(!interior.Contains(j.Offset),"Alternate branch entry");if(i.Operand is Instruction[] js)R(js.All(j=>!interior.Contains(j.Offset)),"Alternate switch entry");}
   var section=pe.PEHeaders.SectionHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData);int header=section.PointerToRawData+m.RVA-section.VirtualAddress;int size=(before[header]&3)==2?1:(BitConverter.ToUInt16(before,header)>>12)*4;R(size is 1 or 12,"Unexpected header");int start=header+size;
   R(before[start+n]==0x20&&BitConverter.ToInt32(before,start+n+1)==0&&before[start+t]==0x2A,"Raw opcode mismatch");after[start+n]=0x14;Array.Clear(after,start+n+1,4);after[start+t]=0x7A;allowed.Add(start+n);allowed.Add(start+t);records.Add(new{token=$"0x{token:X8}",method=m.FullName,header,start,code_size=m.Body.CodeSize,null_offset=n,throw_offset=t,reachable_nop_padding=4});
  }
  var changed=Enumerable.Range(0,before.Length).Where(i=>before[i]!=after[i]).ToArray();R(changed.Length==24&&changed.All(allowed.Contains),"Exact site isolation failed");var tmp=output+".tmp";R(!File.Exists(tmp),"Temporary output exists");File.WriteAllBytes(tmp,after);
  using(var reopened=AssemblyDefinition.ReadAssembly(tmp)){foreach(var(token,n,t)in Sites){var m=(MethodDefinition)reopened.MainModule.LookupToken(token);var parent=(MethodDefinition)asm.MainModule.LookupToken(token);R(m.RVA==parent.RVA&&m.Body.CodeSize==parent.Body.CodeSize&&m.Body.MaxStackSize==parent.Body.MaxStackSize,"Header drift");R(m.Body.Instructions.Single(i=>i.Offset==n).OpCode==OpCodes.Ldnull&&m.Body.Instructions.Single(i=>i.Offset==t).OpCode==OpCodes.Throw,"Reopen opcode mismatch");for(int j=1;j<=4;j++)R(m.Body.Instructions.Single(i=>i.Offset==n+j).OpCode==OpCodes.Nop,"Padding mismatch");var set=m.Body.Instructions.ToHashSet();foreach(var i in set){if(i.Operand is Instruction target)R(set.Contains(target),"Branch outside body");if(i.Operand is Instruction[] targets)R(targets.All(set.Contains),"Switch outside body");}}}
  File.Move(tmp,output);var report=new{input_sha256=H(before),candidate_sha256=H(after),methods=records,changed_bytes=changed.Length,positions=changed,metadata_and_headers_unchanged=true,scope="Twelve individually native-reviewed sites; no global rule; four reachable NOP bytes preserve each original branch offset"};File.WriteAllText(a[2],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(report));
 }
}
