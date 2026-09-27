using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;
    static readonly HashSet<uint> Targets = new() { 0x060006E3u, 0x060006E4u, 0x06000498u, 0x0600049Cu };

    static void Main(string[] a)
    {
        if (a.Length != 2) throw new ArgumentException("usage: PatcherNative7 <formal-native4.dll> <native7-output.dll>");
        var input = Path.GetFullPath(a[0]); var output = Path.GetFullPath(a[1]);
        if (!File.Exists(input)) throw new FileNotFoundException(input);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);
        using var asm = AssemblyDefinition.ReadAssembly(input);
        var mod = asm.MainModule; var mvid = mod.Mvid;
        CheckIdentity(mod, "input"); CheckTargets(mod); var before = Snapshot(mod);
        var textLink = Type(mod, "TextLink"); var zombie = Type(mod, "Zombie");
        PatchTextLink(mod, textLink, "ResetLink", "originalColor", 0x060006E3u);
        PatchTextLink(mod, textLink, "SetLink", "hoverColor", 0x060006E4u);
        PatchLadderTestPlace(mod, zombie); PatchLadderPlaceEnd(mod, zombie);
        CheckIdentity(mod, "memory"); CheckTargets(mod); CheckNonTargets(mod, before, "memory");
        asm.Write(output);
        using var r = AssemblyDefinition.ReadAssembly(output);
        if (r.MainModule.Mvid != mvid) throw new InvalidOperationException($"MVID changed {mvid} -> {r.MainModule.Mvid}");
        CheckIdentity(r.MainModule, "reopened"); CheckTargets(r.MainModule); CheckNonTargets(r.MainModule, before, "reopened");
        Console.WriteLine($"NATIVE7_PATCH_PASS input_mvid={mvid} output_mvid={r.MainModule.Mvid}");
        foreach (var t in Targets.OrderBy(x => x)) { var m = Method(r.MainModule, t); Console.WriteLine($"TARGET 0x{t:X8} {m.FullName} code_size={m.Body.CodeSize} il={m.Body.Instructions.Count}"); }
    }

    static IEnumerable<TypeDefinition> Types(TypeDefinition t) { yield return t; foreach (var n in t.NestedTypes) foreach (var x in Types(n)) yield return x; }
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m) { foreach (var t in m.Types) foreach (var x in Types(t)) yield return x; }
    static TypeDefinition Type(ModuleDefinition m, string n) => Types(m).Single(t => t.Name == n);
    static MethodDefinition Method(ModuleDefinition m, uint tok) => Types(m).SelectMany(t => t.Methods).Single(x => x.MetadataToken.ToUInt32() == tok);
    static void CheckIdentity(ModuleDefinition m, string s) { var ts = Types(m).ToArray(); var mc = ts.Sum(t => t.Methods.Count); var fc = ts.Sum(t => t.Fields.Count); if (ts.Length != ExpectedTypes || mc != ExpectedMethods || fc != ExpectedFields) throw new InvalidOperationException($"{s}: counts {ts.Length}/{mc}/{fc}"); }
    static void CheckTargets(ModuleDefinition m) { var e = new Dictionary<uint,string>{{0x060006E3u,"ResetLink"},{0x060006E4u,"SetLink"},{0x06000498u,"ZC_LadderPlaceEnd"},{0x0600049Cu,"ZC_LadderTestPlace"}}; foreach (var x in e) { var md = Method(m,x.Key); if (md.Name != x.Value || !md.HasBody) throw new InvalidOperationException($"bad target 0x{x.Key:X8}"); } }

    static Dictionary<uint,string> Snapshot(ModuleDefinition m) => Types(m).SelectMany(t=>t.Methods).Where(x=>x.HasBody && !Targets.Contains(x.MetadataToken.ToUInt32())).ToDictionary(x=>x.MetadataToken.ToUInt32(), Fingerprint);
    static void CheckNonTargets(ModuleDefinition m, Dictionary<uint,string> before, string stage) { var now=Snapshot(m); if(now.Count!=before.Count) throw new InvalidOperationException($"{stage}: non-target count changed"); foreach(var x in before) if(!now.TryGetValue(x.Key,out var y)||y!=x.Value) throw new InvalidOperationException($"{stage}: non-target changed 0x{x.Key:X8}"); Console.WriteLine($"NON_TARGET_ISOLATION_PASS stage={stage} methods={before.Count}"); }
    static string Fingerprint(MethodDefinition m)
    {
        var b=new StringBuilder().Append(m.Body.InitLocals).Append('|').Append(m.Body.MaxStackSize).Append('|');
        foreach(var v in m.Body.Variables) b.Append("V:").Append(v.VariableType.FullName).Append(';');
        foreach(var i in m.Body.Instructions) { b.Append(i.OpCode.Code).Append(':'); switch(i.Operand) { case null: break; case Instruction x: b.Append('@').Append(x.Offset); break; case Instruction[] xs: foreach(var x in xs)b.Append('@').Append(x.Offset).Append(','); break; case VariableDefinition v: b.Append('V').Append(v.Index).Append(':').Append(v.VariableType.FullName); break; case ParameterDefinition p: b.Append('P').Append(p.Index).Append(':').Append(p.ParameterType.FullName); break; case MemberReference mr: b.Append('M').Append(mr.MetadataToken.ToUInt32().ToString("X8")).Append(':').Append(mr.FullName); break; default: b.Append(i.Operand); break;} b.Append(';'); }
        foreach(var e in m.Body.ExceptionHandlers) b.Append("EH:").Append(e.HandlerType).Append(':').Append(e.TryStart?.Offset).Append(':').Append(e.TryEnd?.Offset).Append(':').Append(e.HandlerStart?.Offset).Append(':').Append(e.HandlerEnd?.Offset).Append(':').Append(e.FilterStart?.Offset).Append(':').Append(e.CatchType?.FullName).Append(';');
        return b.ToString();
    }
    static void Clear(MethodDefinition m,int stack){m.Body.Instructions.Clear();m.Body.Variables.Clear();m.Body.ExceptionHandlers.Clear();m.Body.InitLocals=true;m.Body.MaxStackSize=stack;}

    static void PatchTextLink(ModuleDefinition mod, TypeDefinition t, string name, string colorName, uint token)
    {
        var m=t.Methods.Single(x=>x.Name==name&&x.Parameters.Count==1); if(m.MetadataToken.ToUInt32()!=token)throw new InvalidOperationException(name+" token");
        var fText=t.Fields.Single(x=>x.Name=="TMP_Text"); var fColor=t.Fields.Single(x=>x.Name==colorName); var tmpScope=fText.FieldType.Scope; var coreScope=fColor.FieldType.Scope;
        var text=fText.FieldType; var ti=new TypeReference("TMPro","TMP_TextInfo",mod,tmpScope); var li=new TypeReference("TMPro","TMP_LinkInfo",mod,tmpScope){IsValueType=true}; var ci=new TypeReference("TMPro","TMP_CharacterInfo",mod,tmpScope){IsValueType=true}; var mi=new TypeReference("TMPro","TMP_MeshInfo",mod,tmpScope){IsValueType=true}; var c32=new TypeReference("UnityEngine","Color32",mod,coreScope){IsValueType=true};
        var getInfo=new MethodReference("get_textInfo",ti,text){HasThis=true}; var update=mod.GetMemberReferences().OfType<MethodReference>().First(x=>x.DeclaringType.Name=="TMP_Text"&&x.Name=="UpdateVertexData"&&x.Parameters.Count==1); var conv=new MethodReference("op_Implicit",c32,c32){HasThis=false}; conv.Parameters.Add(new ParameterDefinition(fColor.FieldType));
        var fLinks=new FieldReference("linkInfo",new ArrayType(li),ti); var fLen=new FieldReference("linkTextLength",mod.TypeSystem.Int32,li); var fFirst=new FieldReference("linkTextfirstCharacterIndex",mod.TypeSystem.Int32,li); var fChars=new FieldReference("characterInfo",new ArrayType(ci),ti); var fMat=new FieldReference("materialReferenceIndex",mod.TypeSystem.Int32,ci); var fVert=new FieldReference("vertexIndex",mod.TypeSystem.Int32,ci); var fMesh=new FieldReference("meshInfo",new ArrayType(mi),ti); var fColors=new FieldReference("colors32",new ArrayType(c32),mi);
        Clear(m,8); var vs=new VariableDefinition[]{new(ti),new(li),new(mod.TypeSystem.Int32),new(mod.TypeSystem.Int32),new(ci),new(mod.TypeSystem.Int32),new(mod.TypeSystem.Int32),new(new ArrayType(c32))}; foreach(var v in vs)m.Body.Variables.Add(v); var il=m.Body.GetILProcessor(); var cond=il.Create(OpCodes.Ldloc,vs[2]); var step=il.Create(OpCodes.Ldloc,vs[2]); var loop=il.Create(OpCodes.Ldloca,vs[1]); var done=il.Create(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getInfo);il.Emit(OpCodes.Stloc,vs[0]); il.Emit(OpCodes.Ldloc,vs[0]);il.Emit(OpCodes.Ldfld,fLinks);il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ldelem_Any,li);il.Emit(OpCodes.Stloc,vs[1]); il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Stloc,vs[2]);il.Emit(OpCodes.Br,cond);
        il.Append(loop);il.Emit(OpCodes.Ldfld,fFirst);il.Emit(OpCodes.Ldloc,vs[2]);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,vs[3]); il.Emit(OpCodes.Ldloc,vs[0]);il.Emit(OpCodes.Ldfld,fChars);il.Emit(OpCodes.Ldloc,vs[3]);il.Emit(OpCodes.Ldelem_Any,ci);il.Emit(OpCodes.Stloc,vs[4]);
        // Exact PC loop has no TMP_CharacterInfo.isVisible filter.
        il.Emit(OpCodes.Ldloca,vs[4]);il.Emit(OpCodes.Ldfld,fMat);il.Emit(OpCodes.Stloc,vs[5]);il.Emit(OpCodes.Ldloca,vs[4]);il.Emit(OpCodes.Ldfld,fVert);il.Emit(OpCodes.Stloc,vs[6]); il.Emit(OpCodes.Ldloc,vs[0]);il.Emit(OpCodes.Ldfld,fMesh);il.Emit(OpCodes.Ldloc,vs[5]);il.Emit(OpCodes.Ldelema,mi);il.Emit(OpCodes.Ldfld,fColors);il.Emit(OpCodes.Stloc,vs[7]);
        for(int k=0;k<4;k++){il.Emit(OpCodes.Ldloc,vs[7]);il.Emit(OpCodes.Ldloc,vs[6]);if(k!=0){il.Emit(OpCodes.Ldc_I4,k);il.Emit(OpCodes.Add);}il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fColor);il.Emit(OpCodes.Call,conv);il.Emit(OpCodes.Stelem_Any,c32);} il.Append(step);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,vs[2]); il.Append(cond);il.Emit(OpCodes.Ldloca,vs[1]);il.Emit(OpCodes.Ldfld,fLen);il.Emit(OpCodes.Blt,loop); il.Append(done);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Ldc_I4,16);il.Emit(OpCodes.Callvirt,update);il.Emit(OpCodes.Ret);
    }

    static void PatchLadderTestPlace(ModuleDefinition mod,TypeDefinition z)
    {
        var m=z.Methods.Single(x=>x.Name=="ZC_LadderTestPlace"&&x.Parameters.Count==0);if(m.MetadataToken.ToUInt32()!=0x0600049Cu)throw new InvalidOperationException("LadderTest token");
        var armor=z.Fields.Single(x=>x.Name=="armor2Type");var board=z.Fields.Single(x=>x.Name=="board");var fx=z.Fields.Single(x=>x.Name=="fX");var fy=z.Fields.Single(x=>x.Name=="fY");var pass=z.Fields.Single(x=>x.Name=="passablePoint");var dir=z.Fields.Single(x=>x.Name=="rDirection"); var vx=mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(x=>x.DeclaringType.Name=="Vector3"&&x.Name=="x")??new FieldReference("x",mod.TypeSystem.Single,dir.FieldType);var vy=mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(x=>x.DeclaringType.Name=="Vector3"&&x.Name=="y")??new FieldReference("y",mod.TypeSystem.Single,dir.FieldType);
        var bt=(TypeDefinition)board.FieldType.Resolve();var bc=bt.Fields.Single(x=>x.Name=="boardConfig");var getGrid=bt.Methods.Single(x=>x.Name=="GetGrid"&&x.Parameters.Count==2);var bct=(TypeDefinition)bc.FieldType.Resolve();var gx=bct.Methods.Single(x=>x.Name=="GetGridX"&&x.Parameters.Count==2);var gy=bct.Methods.Single(x=>x.Name=="GetGridY"&&x.Parameters.Count==2);var gt=(TypeDefinition)getGrid.ReturnType.Resolve();var gp=gt.Methods.Single(x=>x.Name=="GetPassablePoint"&&x.Parameters.Count==0);
        Clear(m,4);var vs=new VariableDefinition[]{new(mod.TypeSystem.Single),new(mod.TypeSystem.Single),new(mod.TypeSystem.Int32),new(mod.TypeSystem.Int32),new(getGrid.ReturnType)};foreach(var v in vs)m.Body.Variables.Add(v);var il=m.Body.GetILProcessor();var no=il.Create(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,armor);il.Emit(OpCodes.Ldc_I4_4);il.Emit(OpCodes.Bne_Un,no); il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fx);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,dir);il.Emit(OpCodes.Ldfld,vx);il.Emit(OpCodes.Ldc_R4,67f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,vs[0]); il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fy);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,dir);il.Emit(OpCodes.Ldfld,vy);il.Emit(OpCodes.Ldc_R4,67f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,vs[1]);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldfld,bc);il.Emit(OpCodes.Ldloc,vs[0]);il.Emit(OpCodes.Ldloc,vs[1]);il.Emit(OpCodes.Callvirt,gx);il.Emit(OpCodes.Stloc,vs[2]); il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldfld,bc);il.Emit(OpCodes.Ldloc,vs[0]);il.Emit(OpCodes.Ldloc,vs[1]);il.Emit(OpCodes.Callvirt,gy);il.Emit(OpCodes.Stloc,vs[3]); il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldloc,vs[2]);il.Emit(OpCodes.Ldloc,vs[3]);il.Emit(OpCodes.Callvirt,getGrid);il.Emit(OpCodes.Stloc,vs[4]);il.Emit(OpCodes.Ldloc,vs[4]);il.Emit(OpCodes.Brfalse,no);
        il.Emit(OpCodes.Ldloc,vs[4]);il.Emit(OpCodes.Callvirt,gp);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,pass);il.Emit(OpCodes.Bgt,no); il.Emit(OpCodes.Ldloc,vs[4]);il.Emit(OpCodes.Callvirt,gp);il.Emit(OpCodes.Ldc_I4_S,(sbyte)60);il.Emit(OpCodes.Ble,no);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Ret);il.Append(no);il.Emit(OpCodes.Ret);
    }

    static void PatchLadderPlaceEnd(ModuleDefinition mod,TypeDefinition z)
    {
        var m=z.Methods.Single(x=>x.Name=="ZC_LadderPlaceEnd"&&x.Parameters.Count==0);if(m.MetadataToken.ToUInt32()!=0x06000498u)throw new InvalidOperationException("LadderPlaceEnd token");
        var placing=z.Fields.Single(x=>x.Name=="ladderZombie_place");var fx=z.Fields.Single(x=>x.Name=="fX");var fy=z.Fields.Single(x=>x.Name=="fY");var dir=z.Fields.Single(x=>x.Name=="rDirection");var board=z.Fields.Single(x=>x.Name=="board");var camp=z.Fields.Single(x=>x.Name=="camp");var test=z.Methods.Single(x=>x.Name=="ZC_LadderTestPlace"&&x.Parameters.Count==0);var drop=z.Methods.Single(x=>x.Name=="Drop_Armor2");var vx=mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(x=>x.DeclaringType.Name=="Vector3"&&x.Name=="x")??new FieldReference("x",mod.TypeSystem.Single,dir.FieldType);var vy=mod.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(x=>x.DeclaringType.Name=="Vector3"&&x.Name=="y")??new FieldReference("y",mod.TypeSystem.Single,dir.FieldType);
        var bt=(TypeDefinition)board.FieldType.Resolve();var bc=bt.Fields.Single(x=>x.Name=="boardConfig");var dm=bt.Fields.Single(x=>x.Name=="deviceManager");var bct=(TypeDefinition)bc.FieldType.Resolve();var gx=bct.Methods.Single(x=>x.Name=="GetGridX"&&x.Parameters.Count==2);var gy=bct.Methods.Single(x=>x.Name=="GetGridY"&&x.Parameters.Count==2);var dd=Type(mod,"DeviceData");var ddctor=dd.Methods.Single(x=>x.IsConstructor&&x.Parameters.Count==1&&x.Parameters[0].ParameterType.FullName==mod.TypeSystem.Int32.FullName);var ddcamp=dd.Fields.Single(x=>x.Name=="camp");var dmt=(TypeDefinition)dm.FieldType.Resolve();var place=dmt.Methods.Single(x=>x.Name=="PlaceDevice");var rm=Type(mod,"ResourceManager");var clips=rm.Fields.Single(x=>x.Name=="zombieClips");var gsv=Type(mod,"GlobalStaticVars");var audio=gsv.Methods.Single(x=>x.Name=="CreateAudioAtPoint");var clipType=audio.Parameters[0].ParameterType;var getItem=new MethodReference("get_Item",clipType,clips.FieldType){HasThis=true};getItem.Parameters.Add(new ParameterDefinition(mod.TypeSystem.Int32));
        Clear(m,4);var vs=new VariableDefinition[]{new(mod.TypeSystem.Single),new(mod.TypeSystem.Single),new(mod.TypeSystem.Int32),new(mod.TypeSystem.Int32),new(dd),new(place.ReturnType),new(clipType),new(audio.ReturnType)};foreach(var v in vs)m.Body.Variables.Add(v);var il=m.Body.GetILProcessor();var ret=il.Create(OpCodes.Ret);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Stfld,placing);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Call,test);il.Emit(OpCodes.Brfalse,ret); il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fx);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,dir);il.Emit(OpCodes.Ldfld,vx);il.Emit(OpCodes.Ldc_R4,67f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,vs[0]);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fy);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldflda,dir);il.Emit(OpCodes.Ldfld,vy);il.Emit(OpCodes.Ldc_R4,67f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,vs[1]);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldfld,bc);il.Emit(OpCodes.Ldloc,vs[0]);il.Emit(OpCodes.Ldloc,vs[1]);il.Emit(OpCodes.Callvirt,gx);il.Emit(OpCodes.Stloc,vs[2]);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldfld,bc);il.Emit(OpCodes.Ldloc,vs[0]);il.Emit(OpCodes.Ldloc,vs[1]);il.Emit(OpCodes.Callvirt,gy);il.Emit(OpCodes.Stloc,vs[3]); il.Emit(OpCodes.Ldc_I4_S,(sbyte)11);il.Emit(OpCodes.Newobj,ddctor);il.Emit(OpCodes.Stloc,vs[4]);il.Emit(OpCodes.Ldloc,vs[4]);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,camp);il.Emit(OpCodes.Stfld,ddcamp);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldfld,dm);il.Emit(OpCodes.Ldloc,vs[4]);il.Emit(OpCodes.Ldloc,vs[2]);il.Emit(OpCodes.Ldloc,vs[3]);il.Emit(OpCodes.Callvirt,place);il.Emit(OpCodes.Stloc,vs[5]);il.Emit(OpCodes.Ldsfld,clips);il.Emit(OpCodes.Ldc_I4_5);il.Emit(OpCodes.Callvirt,getItem);il.Emit(OpCodes.Stloc,vs[6]);il.Emit(OpCodes.Ldloc,vs[6]);il.Emit(OpCodes.Call,audio);il.Emit(OpCodes.Stloc,vs[7]);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Call,drop);il.Append(ret);
    }
}
