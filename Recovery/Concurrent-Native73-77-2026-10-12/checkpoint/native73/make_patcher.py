from pathlib import Path
import base64
p=Path('audit/native73-codex-2026-10-12')
s=Path('audit/native69-codex-2026-10-12/Patcher69.cs').read_text().replace('RepairNative69','RepairNative73')
s=s.replace('58e44b184e89ee3d587b7c7c8d9f57bf58740b96e9c8c71303414aac59cad7a0','a830ce9a66331c669cdc7d77e191855b2409a2f2ef9f6aa1448f0e6dadfdd0dc').replace('Pinned untested Native68 parent','Pinned untested Native72 parent')
s=s.replace('(byte[] Image,object Evidence) Metadata','(byte[] Image,object Evidence,Dictionary<string,int> UserTokens) Metadata')
s=s.replace('throw new Exception("Existing public member name required: "+name);','R(name=="GetAxis","Only public Input.GetAxis name may append");int ix=strings.Length;strings=strings.Concat(raw).ToArray();R(strings.Length<65536,"No string index widening");return ix;')
a=s.index('  int paths=');b=s.index('  ulong valid=',a)
s=s[:a]+'''  var inputType=module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.Input");
  R(inputType.MetadataToken.TokenType==TokenType.TypeRef&&inputType.Scope.Name=="UnityEngine.InputLegacyModule","Existing InputLegacy type reference");
  Member("GetAxis",checked((int)inputType.MetadataToken.RID*8+1),new byte[]{0,1,0x0C,0x0E});
  foreach(var(token,localCount,firstFloat)in new[]{(0x0600000F,9,2),(0x06000010,8,1)}){
   var target=(MD)module.LookupToken(token);byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)target.Body.LocalVarToken.RID)).Signature);
   R(old[0]==7&&old[1]==localCount&&target.Body.Variables.Count==localCount,"Pinned original camera local signature");
   byte[] sig=old.ToArray();int cursor=2;
   for(int i=0;i<localCount;i++){int start=cursor;byte kind=old[cursor++];if(kind==0x12||kind==0x11){byte coded=old[cursor++];if((coded&0xC0)==0x80)cursor++;else if((coded&0xE0)==0xC0)cursor+=3;else R((coded&0x80)==0,"Local coded index");}else R(kind==2||kind==0x1C||kind==0x0C,"Pinned simple local kind");
    if(i>=firstFloat&&i<firstFloat+3){R(kind==0x1C&&target.Body.Variables[i].VariableType.FullName=="System.Object","Only three damaged native scalar slots");sig[start]=0x0C;}
   }
   R(cursor==old.Length,"Local signature fully consumed");int ix=Blob(sig);localRows.AddRange(Words(ix));
   added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst+localRows.Count/2-1:X8}",target=$"0x{token:X8}",signature_index=ix,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig),retyped_float_locals=new[]{firstFloat,firstFloat+1,firstFloat+2}});
  }
  byte[] user=streams.Single(x=>x.Name=="#US").Data;int oldUser=user.Length;
  var userTokens=new Dictionary<string,int>();
  foreach(string text in new[]{"Mouse ScrollWheel","Horizontal","Vertical"}){
   int found=0;
   for(int pos=1;pos<user.Length;){int start=pos,len;byte first=user[pos++];
    if(first==0)continue;
    if((first&0x80)==0)len=first;
    else if((first&0xC0)==0x80){R(pos<user.Length,"US prefix width");len=((first&0x3F)<<8)|user[pos++];}
    else{R((first&0xE0)==0xC0&&pos+3<=user.Length,"US prefix kind");len=((first&0x1F)<<24)|(user[pos++]<<16)|(user[pos++]<<8)|user[pos++];}
    R(len>=1&&pos+len<=user.Length&&(len&1)==1,"US record bounds");
    if(Encoding.Unicode.GetString(user,pos,len-1)==text){found=start;break;}pos+=len;
   }
   if(found==0){byte[] chars=Encoding.Unicode.GetBytes(text);found=user.Length;user=user.Concat(Compress(chars.Length+1)).Concat(chars).Concat(new byte[]{0}).ToArray();R(user.Length<0x1000000,"No user token widening");}
   userTokens.Add(text,0x70000000+found);added.Add(new{kind="UserString",token=$"0x{0x70000000+found:X8}",text});
  }
  R(memberRows.Count==6&&localRows.Count==4,"One public GetAxis binding and two scratch signatures");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()}};
'''+s[b:]
s=s.replace('streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);','streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);streams[streams.FindIndex(s=>s.Name=="#Strings")]=new("#Strings",strings);streams[streams.FindIndex(s=>s.Name=="#US")]=new("#US",user);')
s=s.replace('string_heap_added_bytes=strings.Length-oldStrings});','string_heap_added_bytes=strings.Length-oldStrings,user_heap_added_bytes=user.Length-oldUser},userTokens);')
s=s.replace('var(after,metadata)=Metadata(before,pe,module);','var(after,metadata,userTokens)=Metadata(before,pe,module);')
a=s.index('  var m=M(0x06000463)');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  var vector=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)");
  foreach(var(token,dt,x,y,z)in new[]{(0x0600000F,5,2,3,4),(0x06000010,4,1,2,3)}){
   var camera=M(token);
   var cil=new IL().O(0x72).I(userTokens["Mouse ScrollWheel"]).Ref(0x28,first).S(x)
    .O(0x72).I(userTokens["Horizontal"]).Ref(0x28,first).L(x).Float(-400f).O(0x5A,0x6B,0x58,0x6B).S(x)
    .O(0x72).I(userTokens["Vertical"]).Ref(0x28,first).L(x).Float(400f).O(0x5A,0x6B).S(x).Float(400f).O(0x5A,0x6B).S(y)
    .Ref(0x28,C(camera,"get_deltaTime")).S(dt)
    .L(x).L(dt).O(0x5A,0x6B).S(x).L(y).L(dt).O(0x5A,0x6B).S(y).L(dt).Float(0f).O(0x5A,0x6B).S(z)
    .Ref(0x28,C(camera,"get_main")).Ref(0x6F,C(camera,"get_transform"))
    .L(x).L(y).L(z).Ref(0x73,vector).Ref(0x6F,C(camera,"Translate"));
   if(token==0x0600000F)cil.Arg(0).Ref(0x28,C(camera,"Update_Mouse"));
   cil.O(0x2A);Set(token,cil,4);
  }
  var m=M(0x0600039B);
  int str=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand=="SunFlower_head1").Offset;
  var bs=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int bh=bs.PointerToRawData+m.RVA-bs.VirtualAddress;
  int literal=BitConverter.ToInt32(before,bh+12+str+1);
  var il=new IL().Arg(0).Ref(0x7B,F("Plant","ID")).Int(1).Branch(0x33,"return")
   .Arg(0).Ref(0x7B,F("Plant","animationSprites")).O(0x72).I(literal).Ref(0x28,C(m,"GetAnimationSpritePosition")).S(3)
   .Arg(0).Ref(0x7B,F("Plant","board")).Ref(0x7B,F("Board","projectManager")).Int(1).Int(0).L(3).Ref(0x6F,C(m,"CreateProject")).S(5)
   .L(5).O(0x25).Branch(0x2D,"project-live").O(0x26,0x14,0x7A)
   .Label("project-live").Arg(0).Ref(0x7B,F("Plant","attackPoint"))
   .O(0x25).Float(-2147483648f).Branch(0x37,"cvtt-invalid")
   .O(0x25).Float(2147483648f).Branch(0x34,"cvtt-invalid")
   .O(0x69).Branch(0x2B,"sunset").Label("cvtt-invalid").O(0x26).Int(int.MinValue)
   .Label("sunset").Ref(0x6F,C(m,"SunSet"))
   .L(5).Arg(0).Ref(0x6F,C(m,"SetEndPosition")).L(5).Ref(0x6F,C(m,"SpeedStart"))
   .Arg(0).Ref(0x7B,F("Plant","order")).Int(1).Branch(0x32,"return")
   .Arg(0).Ref(0x28,C(m,"PC_SunflowerHeal")).Label("return").O(0x2A);
  Set(0x0600039B,il,4);
'''+s[b:]
s=s.replace('int oldLocal=m.Body.LocalVarToken.ToInt32(),newLocal=token==0x06000055?0x11000000+localFirst:oldLocal;', 'int oldLocal=m.Body.LocalVarToken.ToInt32(),newLocal=token==0x0600000F?0x11000000+localFirst:token==0x06000010?0x11000000+localFirst+1:oldLocal;')
s=s.replace('   m=M(token);R(m.Body.ExceptionHandlers.Count', '   var m=M(token);R(m.Body.ExceptionHandlers.Count')
# m is already declared above; keep assignment rather than shadowing.
s=s.replace('   var m=M(token);R(m.Body.ExceptionHandlers.Count', '   m=M(token);R(m.Body.ExceptionHandlers.Count')
s=s.replace('R(targets.Count==4,"Pinned scope 4")','R(targets.Count==3,"Pinned scope 3")')
a=s.index('scope="Four native-backed bodies;');b=s.index('"},new JsonSerializerOptions',a)
s=s[:a]+'scope="Three native-backed bodies; exact ScrollWheel/Horizontal/Vertical calls and float32 math, camera translation and conditional mouse update, sunflower production with SSE cvttss2si boundaries; one public Input.GetAxis reference, exact original user strings, two scoped local signatures each recovering three damaged scalar slots; unqualified parent chain pending'+s[b:]
(p/'Patcher73.cs').write_text(s)
remote='''from pathlib import Path
import subprocess,base64
w=Path('/workspaces/GodsPVZ-native24-codespace');n=Path('/tmp/godspvz-native73-codex-2026-10-12');n.mkdir(exist_ok=True)
s=w/'scripts/takeover/native73/Patcher';s.mkdir(parents=True,exist_ok=True)
'''+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))\n"+'''(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes());(s/'.gitignore').write_text('obj/\\nbin/\\n')
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
def produce(command,label):
 r=subprocess.run(command,capture_output=True,text=True);log=n/(label+'.log')
 if log.exists():
  i=1
  while(n/(label+'.attempt-'+str(i)+'.log')).exists():i+=1
  log.rename(n/(label+'.attempt-'+str(i)+'.log'))
 log.write_text(r.stdout+r.stderr+'\\nEXIT_CODE='+str(r.returncode)+'\\n')
 if r.returncode:raise RuntimeError(label+' '+r.stdout+r.stderr)
 return r.stdout
produce(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(n/'patcher-bin'),'-p:MonoCecilPath='+str(c)],'patcher-build')
print(produce(['dotnet',str(n/'patcher-bin/Patcher.dll'),'/tmp/godspvz-native72-restored-2026-10-12/snapshot/candidates/native72/candidate1.dll',str(n/'candidate1.dll'),str(n/'patch1.json')],'patcher-generate'))
print((n/'patch1.json').read_text())
'''
(p/'generate_remote.py').write_text(remote)
