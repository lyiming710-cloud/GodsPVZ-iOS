using Mono.Cecil;using Mono.Cecil.Cil;using System.Reflection.PortableExecutable;using System.Reflection.Metadata;using System.Reflection.Metadata.Ecma335;using MD=Mono.Cecil.MethodDefinition;
byte[]b=File.ReadAllBytes(args[0]);using var a=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);using var p=new PEReader(new MemoryStream(b));int off(int r){var s=p.PEHeaders.SectionHeaders.Single(s=>r>=s.VirtualAddress&&r<s.VirtualAddress+s.SizeOfRawData);return s.PointerToRawData+r-s.VirtualAddress;}MD method(int t)=>(MD)a.MainModule.LookupToken(t);var first=method(0x0600017B);int h=off(first.RVA),root=off(p.PEHeaders.CorHeader!.MetadataDirectory.RelativeVirtualAddress);
switch(args[2]){
case "debug-reset":{var i=first.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldc_R4);BitConverter.GetBytes(1f).CopyTo(b,h+12+i.Offset+1);break;}
case "group-fallback":{var m=method(0x06000477);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldc_I4&&(int)i.Operand==int.MinValue);BitConverter.GetBytes(0).CopyTo(b,off(m.RVA)+12+i.Offset+1);break;}
case "local-branch":case "world-branch":{var m=method(args[2]=="local-branch"?0x06000837:0x06000838);var i=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Brfalse_S);b[off(m.RVA)+12+i.Offset]=0x2d;break;}
case "old-heap":b[root+p.GetMetadataReader().GetHeapMetadataOffset(HeapIndex.String)+2]^=1;break;
case "locals":BitConverter.GetBytes(method(0x06000477).Body.LocalVarToken.ToInt32()).CopyTo(b,h+8);break;
case "section-flags":int sh=p.PEHeaders.PEHeaderStartOffset+p.PEHeaders.CoffHeader.SizeOfOptionalHeader+(p.PEHeaders.CoffHeader.NumberOfSections-1)*40;b[sh+36]^=1;break;
case "body-padding":b[h+12+first.Body.CodeSize]=1;break;
case "max-stack":BitConverter.GetBytes((ushort)9).CopyTo(b,h+2);break;
case "non-target":b[off(method(0x06000091).RVA)+12]^=1;break;
default:throw new Exception();}File.WriteAllBytes(args[1],b);
