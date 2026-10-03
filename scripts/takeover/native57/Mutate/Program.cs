using Mono.Cecil;using Mono.Cecil.Cil;using System.Reflection.PortableExecutable;using System.Reflection.Metadata;using System.Reflection.Metadata.Ecma335;using MD=Mono.Cecil.MethodDefinition;
byte[]b=File.ReadAllBytes(args[0]);using var a=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);using var p=new PEReader(new MemoryStream(b));int off(int r){var s=p.PEHeaders.SectionHeaders.Single(s=>r>=s.VirtualAddress&&r<s.VirtualAddress+s.SizeOfRawData);return s.PointerToRawData+r-s.VirtualAddress;}MD method(int t)=>(MD)a.MainModule.LookupToken(t);void rf(int token,float old,float value){var m=method(token);var i=m.Body.Instructions.First(i=>i.OpCode==OpCodes.Ldc_R4&&(float)i.Operand==old);BitConverter.GetBytes(value).CopyTo(b,off(m.RVA)+12+i.Offset+1);}var first=method(0x06000045);int h=off(first.RVA),root=off(p.PEHeaders.CorHeader!.MetadataDirectory.RelativeVirtualAddress);
switch(args[2]){
case "enemy-branch":{var m=first;var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Brtrue_S);b[off(m.RVA)+12+i.Offset]=0x2c;break;}
case "board-index":{var m=method(0x060002C3);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldc_I4_S&&(sbyte)i.Operand==10);b[off(m.RVA)+12+i.Offset+1]=9;break;}
case "copy-num":{var m=method(0x06000301);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldfld&&((FieldReference)i.Operand).Name=="num");var f=a.MainModule.GetTypes().Single(t=>t.FullName=="DeviceContent/ContentDeviceData").Fields.Single(f=>f.Name=="camp");BitConverter.GetBytes(f.MetadataToken.ToInt32()).CopyTo(b,off(m.RVA)+12+i.Offset+1);break;}
case "die-return":{var m=method(0x0600035D);var i=m.Body.Instructions.Last(i=>i.OpCode==OpCodes.Ldc_I4_1);b[off(m.RVA)+12+i.Offset]=0x16;break;}
case "heal-y":rf(0x06000378,10,11);break;
case "cool-state":{var m=method(0x06000409);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Stfld&&((FieldReference)i.Operand).Name=="propState").Previous;b[off(m.RVA)+12+i.Offset]=0x17;break;}
case "pole-jump":{var m=method(0x060004A1);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Stfld&&((FieldReference)i.Operand).Name=="poleZombie_jump").Previous;b[off(m.RVA)+12+i.Offset]=0x17;break;}
case "rotate-angle":rf(0x06000669,-75,-74);break;
case "old-heap":b[root+p.GetMetadataReader().GetHeapMetadataOffset(HeapIndex.String)+2]^=1;break;
case "locals":BitConverter.GetBytes(method(0x06000378).Body.LocalVarToken.ToInt32()).CopyTo(b,h+8);break;
case "section-flags":int sh=p.PEHeaders.PEHeaderStartOffset+p.PEHeaders.CoffHeader.SizeOfOptionalHeader+(p.PEHeaders.CoffHeader.NumberOfSections-1)*40;b[sh+36]^=1;break;
case "body-padding":b[h+12+first.Body.CodeSize]=1;break;
case "max-stack":BitConverter.GetBytes((ushort)9).CopyTo(b,h+2);break;
case "non-target":b[off(method(0x06000091).RVA)+12]^=1;break;
default:throw new Exception();}File.WriteAllBytes(args[1],b);
