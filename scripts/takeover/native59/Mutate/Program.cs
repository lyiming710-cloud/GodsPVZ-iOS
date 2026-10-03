using Mono.Cecil;using Mono.Cecil.Cil;using System.Reflection.PortableExecutable;using System.Reflection.Metadata;using System.Reflection.Metadata.Ecma335;using MD=Mono.Cecil.MethodDefinition;
byte[]b=File.ReadAllBytes(args[0]);using var a=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);using var p=new PEReader(new MemoryStream(b));int off(int r){var s=p.PEHeaders.SectionHeaders.Single(s=>r>=s.VirtualAddress&&r<s.VirtualAddress+s.SizeOfRawData);return s.PointerToRawData+r-s.VirtualAddress;}MD method(int t)=>(MD)a.MainModule.LookupToken(t);var first=method(0x06000356);int h=off(first.RVA),root=off(p.PEHeaders.CorHeader!.MetadataDirectory.RelativeVirtualAddress);
void Float(int token,float old,float value){var m=method(token);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldc_R4&&(float)i.Operand==old);BitConverter.GetBytes(value).CopyTo(b,off(m.RVA)+12+i.Offset+1);}
switch(args[2]){
case "scenario-rate":Float(0x06000356,25,24);break;
case "key-letter":{var m=method(0x06000408);var x=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand=="S");var y=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand=="A");Array.Copy(b,off(m.RVA)+12+y.Offset+1,b,off(m.RVA)+12+x.Offset+1,4);break;}
case "fall-range":{var m=method(0x0600046E);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldc_I4_S&&(sbyte)i.Operand==61);b[off(m.RVA)+12+i.Offset+1]=60;break;}
case "shoot-pitch":Float(0x0600046F,1,2);break;
case "digit-text":{var m=method(0x06000726);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Stobj&&((Mono.Cecil.TypeReference)i.Operand).FullName=="System.String");int pos=off(m.RVA)+12+i.Offset;b[pos]=0x26;b[pos+1]=0x26;b[pos+2]=b[pos+3]=b[pos+4]=0;break;}
case "digit-index":{var m=method(0x06000726);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldc_I4_1);b[off(m.RVA)+12+i.Offset]=0x16;break;}
case "mesh-flags":{var m=method(0x06000856);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldc_I4_S&&(sbyte)i.Operand==20);b[off(m.RVA)+12+i.Offset+1]=21;break;}
case "old-heap":b[root+p.GetMetadataReader().GetHeapMetadataOffset(HeapIndex.String)+2]^=1;break;
case "locals":BitConverter.GetBytes(method(0x06000408).Body.LocalVarToken.ToInt32()).CopyTo(b,h+8);break;
case "section-flags":int sh=p.PEHeaders.PEHeaderStartOffset+p.PEHeaders.CoffHeader.SizeOfOptionalHeader+(p.PEHeaders.CoffHeader.NumberOfSections-1)*40;b[sh+36]^=1;break;
case "body-padding":b[h+12+first.Body.CodeSize]=1;break;
case "max-stack":BitConverter.GetBytes((ushort)9).CopyTo(b,h+2);break;
case "non-target":b[off(method(0x06000091).RVA)+12]^=1;break;
default:throw new Exception();}File.WriteAllBytes(args[1],b);
