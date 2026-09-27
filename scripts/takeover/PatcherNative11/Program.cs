using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x060007C1u;
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static void Main(string[] args)
    {
        if (args.Length < 2 || args.Length > 3) throw new ArgumentException("usage: PatcherNative11 <native10.dll> <native11.dll> [linked]");
        bool linked = args.Length == 3 && args[2] == "linked";
        var input = Path.GetFullPath(args[0]); var output = Path.GetFullPath(args[1]);
        if (!File.Exists(input)) throw new FileNotFoundException(input);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);
        using var asm = AssemblyDefinition.ReadAssembly(input);
        var mod = asm.MainModule; var mvid = mod.Mvid;
        if (!linked) CheckIdentity(mod, "input");
        var target = FindTarget(mod, linked);
        var before = Snapshot(mod, target);
        Patch(mod, target);
        if (!linked) CheckIdentity(mod, "memory");
        CheckNonTargets(mod, target, before, "memory");
        asm.Write(output);
        using var reopened = AssemblyDefinition.ReadAssembly(output);
        if (reopened.MainModule.Mvid != mvid) throw new InvalidOperationException($"MVID changed {mvid}->{reopened.MainModule.Mvid}");
        if (!linked) CheckIdentity(reopened.MainModule, "reopened");
        var rt = FindTarget(reopened.MainModule, linked);
        CheckNonTargets(reopened.MainModule, rt, before, "reopened");
        Console.WriteLine($"NATIVE11_PATCH_PASS linked={linked} input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
        Console.WriteLine($"TARGET token=0x{rt.MetadataToken.ToUInt32():X8} {rt.FullName} code_size={rt.Body.CodeSize} il={rt.Body.Instructions.Count} locals={rt.Body.Variables.Count} eh={rt.Body.ExceptionHandlers.Count}");
    }

    static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static MethodDefinition FindTarget(ModuleDefinition m,bool linked)
    {
        var t=Types(m).Single(x=>x.Namespace=="TMPro.Examples"&&x.Name=="TMP_TextSelector_A");
        var md=t.Methods.Single(x=>x.Name=="LateUpdate"&&x.Parameters.Count==0);
        if(!linked && md.MetadataToken.ToUInt32()!=TargetToken)throw new InvalidOperationException($"target token mismatch 0x{md.MetadataToken.ToUInt32():X8}");
        if(!md.HasBody)throw new InvalidOperationException("target body missing");
        return md;
    }
    static void CheckIdentity(ModuleDefinition m,string stage){var ts=Types(m).ToArray();int mc=ts.Sum(t=>t.Methods.Count),fc=ts.Sum(t=>t.Fields.Count);if(ts.Length!=ExpectedTypes||mc!=ExpectedMethods||fc!=ExpectedFields)throw new InvalidOperationException($"{stage}: counts {ts.Length}/{mc}/{fc}");}
    static Dictionary<uint,string> Snapshot(ModuleDefinition m,MethodDefinition target)=>Types(m).SelectMany(t=>t.Methods).Where(x=>x.HasBody&&x!=target).ToDictionary(x=>x.MetadataToken.ToUInt32(),Fingerprint);
    static void CheckNonTargets(ModuleDefinition m,MethodDefinition target,Dictionary<uint,string> before,string stage){var now=Snapshot(m,target);if(now.Count!=before.Count)throw new InvalidOperationException($"{stage}: non-target count {before.Count}->{now.Count}");foreach(var kv in before)if(!now.TryGetValue(kv.Key,out var v)||v!=kv.Value)throw new InvalidOperationException($"{stage}: non-target changed 0x{kv.Key:X8}");Console.WriteLine($"NON_TARGET_ISOLATION_PASS stage={stage} methods={before.Count}");}
    static string StableOp(Instruction i){var n=i.OpCode.Code.ToString();if((i.Operand is Instruction||i.Operand is Instruction[])&&n.EndsWith("_S",StringComparison.Ordinal))return n[..^2];return n;}
    static int Ix(MethodDefinition m,Instruction i)=>i==null?-1:m.Body.Instructions.IndexOf(i);
    static string Fingerprint(MethodDefinition m)
    {
        var b=new StringBuilder().Append(m.Body.InitLocals).Append('|').Append(m.Body.MaxStackSize).Append('|');
        foreach(var v in m.Body.Variables)b.Append("V:").Append(v.VariableType.FullName).Append(';');
        foreach(var i in m.Body.Instructions){b.Append(StableOp(i)).Append(':');switch(i.Operand){case null:break;case Instruction x:b.Append('@').Append(Ix(m,x));break;case Instruction[] xs:foreach(var x in xs)b.Append('@').Append(Ix(m,x)).Append(',');break;case VariableDefinition v:b.Append('V').Append(v.Index).Append(':').Append(v.VariableType.FullName);break;case ParameterDefinition p:b.Append('P').Append(p.Index).Append(':').Append(p.ParameterType.FullName);break;case MemberReference mr:b.Append('M').Append(mr.FullName).Append('@').Append(mr.DeclaringType?.Scope?.Name);break;default:b.Append(i.Operand);break;}b.Append(';');}
        foreach(var e in m.Body.ExceptionHandlers)b.Append("EH:").Append(e.HandlerType).Append(':').Append(Ix(m,e.TryStart)).Append(':').Append(Ix(m,e.TryEnd)).Append(':').Append(Ix(m,e.HandlerStart)).Append(':').Append(Ix(m,e.HandlerEnd)).Append(':').Append(Ix(m,e.FilterStart)).Append(':').Append(e.CatchType?.FullName).Append(';');
        return b.ToString();
    }

    static MethodReference MR(ModuleDefinition m,string decl,string name,int argc,string ret=null)
    {
        var q=m.GetMemberReferences().OfType<MethodReference>().Where(x=>x.DeclaringType.FullName==decl&&x.Name==name&&x.Parameters.Count==argc);
        if(ret!=null)q=q.Where(x=>x.ReturnType.FullName==ret);
        return q.First();
    }
    static FieldReference FR(ModuleDefinition m,string decl,string name)=>m.GetMemberReferences().OfType<FieldReference>().First(x=>x.DeclaringType.FullName==decl&&x.Name==name);
    static FieldReference FieldOrNew(ModuleDefinition m,TypeReference decl,string name,TypeReference type)=>m.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(x=>x.DeclaringType.FullName==decl.FullName&&x.Name==name)??new FieldReference(name,type,decl);
    static MethodReference MethodOrNew(ModuleDefinition m,TypeReference decl,string name,TypeReference ret,bool hasThis,params TypeReference[] args)
    {
        var hit=m.GetMemberReferences().OfType<MethodReference>().FirstOrDefault(x=>x.DeclaringType.FullName==decl.FullName&&x.Name==name&&x.Parameters.Count==args.Length&&x.ReturnType.FullName==ret.FullName&&x.Parameters.Select(p=>p.ParameterType.FullName).SequenceEqual(args.Select(a=>a.FullName)));
        if(hit!=null)return hit;
        var mr=new MethodReference(name,ret,decl){HasThis=hasThis};foreach(var a in args)mr.Parameters.Add(new ParameterDefinition(a));return mr;
    }
    static void Reset(MethodDefinition m,int max){m.Body.Instructions.Clear();m.Body.Variables.Clear();m.Body.ExceptionHandlers.Clear();m.Body.InitLocals=true;m.Body.MaxStackSize=max;}
    static void Ldc(ILProcessor il,int v){switch(v){case -1:il.Emit(OpCodes.Ldc_I4_M1);break;case 0:il.Emit(OpCodes.Ldc_I4_0);break;case 1:il.Emit(OpCodes.Ldc_I4_1);break;case 2:il.Emit(OpCodes.Ldc_I4_2);break;case 3:il.Emit(OpCodes.Ldc_I4_3);break;default:il.Emit(OpCodes.Ldc_I4,v);break;}}
    static void EmitRandomColor(ILProcessor il,VariableDefinition c,TypeReference colorType,FieldReference r,FieldReference g,FieldReference b,FieldReference a,MethodReference range)
    {
        il.Emit(OpCodes.Ldloca,c);il.Emit(OpCodes.Initobj,colorType);
        foreach(var f in new[]{r,g,b}){il.Emit(OpCodes.Ldloca,c);Ldc(il,0);Ldc(il,255);il.Emit(OpCodes.Call,range);il.Emit(OpCodes.Conv_U1);il.Emit(OpCodes.Stfld,f);}
        il.Emit(OpCodes.Ldloca,c);Ldc(il,255);il.Emit(OpCodes.Conv_U1);il.Emit(OpCodes.Stfld,a);
    }
    static void EmitColor4(ILProcessor il,VariableDefinition colors,VariableDefinition vertex,VariableDefinition c,TypeReference colorType)
    {
        for(int k=0;k<4;k++){il.Emit(OpCodes.Ldloc,colors);il.Emit(OpCodes.Ldloc,vertex);if(k!=0){Ldc(il,k);il.Emit(OpCodes.Add);}il.Emit(OpCodes.Ldloc,c);il.Emit(OpCodes.Stelem_Any,colorType);}
    }

    static void Patch(ModuleDefinition mod,MethodDefinition method)
    {
        var t=method.DeclaringType;
        var fText=t.Fields.Single(x=>x.Name=="m_TextMeshPro");
        var fCamera=t.Fields.Single(x=>x.Name=="m_Camera");
        var fHover=t.Fields.Single(x=>x.Name=="m_isHoveringObject");
        var fSelected=t.Fields.Single(x=>x.Name=="m_selectedLink");
        var fLastChar=t.Fields.Single(x=>x.Name=="m_lastCharIndex");
        var fLastWord=t.Fields.Single(x=>x.Name=="m_lastWordIndex");

        var getMouse=MR(mod,"UnityEngine.Input","get_mousePosition",0,"UnityEngine.Vector3");
        var getKey=MR(mod,"UnityEngine.Input","GetKeyInt",1,"System.Boolean");
        var getMain=MR(mod,"UnityEngine.Camera","get_main",0,"UnityEngine.Camera");
        var randomRange=MR(mod,"UnityEngine.Random","Range",2,"System.Int32");
        var getRect=MR(mod,"TMPro.TMP_Text","get_rectTransform",0,"UnityEngine.RectTransform");
        var getTextInfo=MR(mod,"TMPro.TMP_Text","get_textInfo",0,"TMPro.TMP_TextInfo");
        var isIntersect=MR(mod,"TMPro.TMP_TextUtilities","IsIntersectingRectTransform",3,"System.Boolean");
        var findChar=MR(mod,"TMPro.TMP_TextUtilities","FindIntersectingCharacter",4,"System.Int32");
        var findLink=MR(mod,"TMPro.TMP_TextUtilities","FindIntersectingLink",3,"System.Int32");
        var findWord=MR(mod,"TMPro.TMP_TextUtilities","FindIntersectingWord",3,"System.Int32");
        var getLinkId=MR(mod,"TMPro.TMP_LinkInfo","GetLinkID",0,"System.String");
        var screenPoint=MR(mod,"UnityEngine.RectTransformUtility","ScreenPointToWorldPointInRectangle",4,"System.Boolean");
        var transformPoint=MR(mod,"UnityEngine.Transform","TransformPoint",1,"UnityEngine.Vector3");
        var worldToScreen=MR(mod,"UnityEngine.Camera","WorldToScreenPoint",1,"UnityEngine.Vector3");
        var setMeshColors=MR(mod,"UnityEngine.Mesh","set_colors32",1,"System.Void");
        var stringEq=mod.GetMemberReferences().OfType<MethodReference>().First(x=>x.DeclaringType.FullName=="System.String"&&x.Name=="op_Equality"&&x.Parameters.Count==2);

        var fiChar=FR(mod,"TMPro.TMP_TextInfo","characterInfo");
        var fiMesh=FR(mod,"TMPro.TMP_TextInfo","meshInfo");
        var fiLink=FR(mod,"TMPro.TMP_TextInfo","linkInfo");
        var fiWord=FR(mod,"TMPro.TMP_TextInfo","wordInfo");
        var fcMat=FR(mod,"TMPro.TMP_CharacterInfo","materialReferenceIndex");
        var fcVertex=FR(mod,"TMPro.TMP_CharacterInfo","vertexIndex");
        var fmColors=FR(mod,"TMPro.TMP_MeshInfo","colors32");

        var vec3=getMouse.ReturnType;
        var vec2=screenPoint.Parameters[1].ParameterType;
        var cameraType=getMain.ReturnType;
        var textInfoType=getTextInfo.ReturnType;
        var charType=((ArrayType)fiChar.FieldType).ElementType;
        var meshInfoType=((ArrayType)fiMesh.FieldType).ElementType;
        var linkType=((ArrayType)fiLink.FieldType).ElementType;
        var wordType=((ArrayType)fiWord.FieldType).ElementType;
        var colorsType=(ArrayType)fmColors.FieldType; var colorType=colorsType.ElementType;
        var meshType=setMeshColors.DeclaringType;

        var wordFirst=FieldOrNew(mod,wordType,"firstCharacterIndex",mod.TypeSystem.Int32);
        var wordCount=FieldOrNew(mod,wordType,"characterCount",mod.TypeSystem.Int32);
        var bottomLeft=FieldOrNew(mod,charType,"bottomLeft",vec3);
        var meshField=FieldOrNew(mod,meshInfoType,"mesh",meshType);
        var cr=FieldOrNew(mod,colorType,"r",mod.TypeSystem.Byte);
        var cg=FieldOrNew(mod,colorType,"g",mod.TypeSystem.Byte);
        var cb=FieldOrNew(mod,colorType,"b",mod.TypeSystem.Byte);
        var ca=FieldOrNew(mod,colorType,"a",mod.TypeSystem.Byte);

        var componentType=mod.GetTypeReferences().First(x=>x.FullName=="UnityEngine.Component");
        var transformType=transformPoint.DeclaringType;
        var getTransform=MethodOrNew(mod,componentType,"get_transform",transformType,true);
        var getMesh=MethodOrNew(mod,fText.FieldType,"get_mesh",meshType,true);
        var vec3ToVec2=mod.GetMemberReferences().OfType<MethodReference>().FirstOrDefault(x=>x.Name=="op_Implicit"&&x.Parameters.Count==1&&x.Parameters[0].ParameterType.FullName==vec3.FullName&&x.ReturnType.FullName==vec2.FullName)
            ??MethodOrNew(mod,vec2,"op_Implicit",vec2,false,vec3);

        Reset(method,10);
        var textInfo=new VariableDefinition(textInfoType);var charIndex=new VariableDefinition(mod.TypeSystem.Int32);var meshIndex=new VariableDefinition(mod.TypeSystem.Int32);var vertexIndex=new VariableDefinition(mod.TypeSystem.Int32);
        var colors=new VariableDefinition(colorsType);var color=new VariableDefinition(colorType);var linkIndex=new VariableDefinition(mod.TypeSystem.Int32);var linkInfo=new VariableDefinition(linkType);var worldPoint=new VariableDefinition(vec3);var linkId=new VariableDefinition(mod.TypeSystem.String);
        var wordIndex=new VariableDefinition(mod.TypeSystem.Int32);var firstChar=new VariableDefinition(mod.TypeSystem.Int32);var charCount=new VariableDefinition(mod.TypeSystem.Int32);var wordPos=new VariableDefinition(vec3);var j=new VariableDefinition(mod.TypeSystem.Int32);
        foreach(var v in new[]{textInfo,charIndex,meshIndex,vertexIndex,colors,color,linkIndex,linkInfo,worldPoint,linkId,wordIndex,firstChar,charCount,wordPos,j})method.Body.Variables.Add(v);
        var il=method.Body.GetILProcessor();
        var hoverTrue=il.Create(OpCodes.Nop);var afterHoverTest=il.Create(OpCodes.Nop);var charSkip=il.Create(OpCodes.Nop);var charPaint=il.Create(OpCodes.Nop);
        var linkAfterClear=il.Create(OpCodes.Nop);var linkNewCheck=il.Create(OpCodes.Nop);var linkDone=il.Create(OpCodes.Nop);var linkIdDone=il.Create(OpCodes.Nop);
        var wordSkip=il.Create(OpCodes.Nop);var wordLoopCond=il.Create(OpCodes.Ldloc,j);var wordLoopBody=il.Create(OpCodes.Nop);var ret=il.Create(OpCodes.Ret);

        // Hover test.
        il.Emit(OpCodes.Ldarg_0);Ldc(il,0);il.Emit(OpCodes.Stfld,fHover);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getRect);
        il.Emit(OpCodes.Call,getMouse);il.Emit(OpCodes.Call,getMain);il.Emit(OpCodes.Call,isIntersect);il.Emit(OpCodes.Brtrue,hoverTrue);il.Emit(OpCodes.Br,afterHoverTest);
        il.Append(hoverTrue);il.Emit(OpCodes.Ldarg_0);Ldc(il,1);il.Emit(OpCodes.Stfld,fHover);
        il.Append(afterHoverTest);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fHover);il.Emit(OpCodes.Brfalse,ret);

        // Character selection.
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Call,getMouse);il.Emit(OpCodes.Call,getMain);Ldc(il,1);il.Emit(OpCodes.Call,findChar);il.Emit(OpCodes.Stloc,charIndex);
        il.Emit(OpCodes.Ldloc,charIndex);Ldc(il,-1);il.Emit(OpCodes.Beq,charSkip);
        il.Emit(OpCodes.Ldloc,charIndex);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fLastChar);il.Emit(OpCodes.Beq,charSkip);
        Ldc(il,304);il.Emit(OpCodes.Call,getKey);il.Emit(OpCodes.Brtrue,charPaint);Ldc(il,303);il.Emit(OpCodes.Call,getKey);il.Emit(OpCodes.Brfalse,charSkip);
        il.Append(charPaint);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldloc,charIndex);il.Emit(OpCodes.Stfld,fLastChar);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getTextInfo);il.Emit(OpCodes.Stloc,textInfo);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiChar);il.Emit(OpCodes.Ldloc,charIndex);il.Emit(OpCodes.Ldelema,charType);il.Emit(OpCodes.Ldfld,fcMat);il.Emit(OpCodes.Stloc,meshIndex);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiChar);il.Emit(OpCodes.Ldloc,charIndex);il.Emit(OpCodes.Ldelema,charType);il.Emit(OpCodes.Ldfld,fcVertex);il.Emit(OpCodes.Stloc,vertexIndex);
        EmitRandomColor(il,color,colorType,cr,cg,cb,ca,randomRange);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiMesh);il.Emit(OpCodes.Ldloc,meshIndex);il.Emit(OpCodes.Ldelema,meshInfoType);il.Emit(OpCodes.Ldfld,fmColors);il.Emit(OpCodes.Stloc,colors);
        EmitColor4(il,colors,vertexIndex,color,colorType);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiMesh);il.Emit(OpCodes.Ldloc,meshIndex);il.Emit(OpCodes.Ldelema,meshInfoType);il.Emit(OpCodes.Ldfld,meshField);il.Emit(OpCodes.Ldloc,colors);il.Emit(OpCodes.Callvirt,setMeshColors);
        il.Append(charSkip);

        // Link handling.
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Call,getMouse);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fCamera);il.Emit(OpCodes.Call,findLink);il.Emit(OpCodes.Stloc,linkIndex);
        il.Emit(OpCodes.Ldloc,linkIndex);Ldc(il,-1);il.Emit(OpCodes.Bne_Un,linkNewCheck);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fSelected);Ldc(il,-1);il.Emit(OpCodes.Beq,linkAfterClear);il.Emit(OpCodes.Ldarg_0);Ldc(il,-1);il.Emit(OpCodes.Stfld,fSelected);il.Emit(OpCodes.Br,linkAfterClear);
        il.Append(linkNewCheck);il.Emit(OpCodes.Ldloc,linkIndex);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fSelected);il.Emit(OpCodes.Beq,linkAfterClear);il.Emit(OpCodes.Ldarg_0);Ldc(il,-1);il.Emit(OpCodes.Stfld,fSelected);
        il.Append(linkAfterClear);
        il.Emit(OpCodes.Ldloc,linkIndex);Ldc(il,-1);il.Emit(OpCodes.Beq,linkDone);il.Emit(OpCodes.Ldloc,linkIndex);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fSelected);il.Emit(OpCodes.Beq,linkDone);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldloc,linkIndex);il.Emit(OpCodes.Stfld,fSelected);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getTextInfo);il.Emit(OpCodes.Stloc,textInfo);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiLink);il.Emit(OpCodes.Ldloc,linkIndex);il.Emit(OpCodes.Ldelem_Any,linkType);il.Emit(OpCodes.Stloc,linkInfo);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getRect);il.Emit(OpCodes.Call,getMouse);il.Emit(OpCodes.Call,vec3ToVec2);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fCamera);il.Emit(OpCodes.Ldloca,worldPoint);il.Emit(OpCodes.Call,screenPoint);il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ldloca,linkInfo);il.Emit(OpCodes.Call,getLinkId);il.Emit(OpCodes.Stloc,linkId);
        il.Emit(OpCodes.Ldloc,linkId);il.Emit(OpCodes.Ldstr,"id_01");il.Emit(OpCodes.Call,stringEq);il.Emit(OpCodes.Brtrue,linkIdDone);il.Emit(OpCodes.Ldloc,linkId);il.Emit(OpCodes.Ldstr,"id_02");il.Emit(OpCodes.Call,stringEq);il.Emit(OpCodes.Pop);il.Append(linkIdDone);
        il.Append(linkDone);

        // Word selection.
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Call,getMouse);il.Emit(OpCodes.Call,getMain);il.Emit(OpCodes.Call,findWord);il.Emit(OpCodes.Stloc,wordIndex);
        il.Emit(OpCodes.Ldloc,wordIndex);Ldc(il,-1);il.Emit(OpCodes.Beq,wordSkip);il.Emit(OpCodes.Ldloc,wordIndex);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fLastWord);il.Emit(OpCodes.Beq,wordSkip);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldloc,wordIndex);il.Emit(OpCodes.Stfld,fLastWord);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getTextInfo);il.Emit(OpCodes.Stloc,textInfo);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiWord);il.Emit(OpCodes.Ldloc,wordIndex);il.Emit(OpCodes.Ldelema,wordType);il.Emit(OpCodes.Ldfld,wordFirst);il.Emit(OpCodes.Stloc,firstChar);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiWord);il.Emit(OpCodes.Ldloc,wordIndex);il.Emit(OpCodes.Ldelema,wordType);il.Emit(OpCodes.Ldfld,wordCount);il.Emit(OpCodes.Stloc,charCount);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getTransform);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiChar);il.Emit(OpCodes.Ldloc,firstChar);il.Emit(OpCodes.Ldelema,charType);il.Emit(OpCodes.Ldfld,bottomLeft);il.Emit(OpCodes.Callvirt,transformPoint);il.Emit(OpCodes.Stloc,wordPos);
        il.Emit(OpCodes.Call,getMain);il.Emit(OpCodes.Ldloc,wordPos);il.Emit(OpCodes.Callvirt,worldToScreen);il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiMesh);Ldc(il,0);il.Emit(OpCodes.Ldelema,meshInfoType);il.Emit(OpCodes.Ldfld,fmColors);il.Emit(OpCodes.Stloc,colors);
        EmitRandomColor(il,color,colorType,cr,cg,cb,ca,randomRange);
        Ldc(il,0);il.Emit(OpCodes.Stloc,j);il.Emit(OpCodes.Br,wordLoopCond);
        il.Append(wordLoopBody);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fiChar);il.Emit(OpCodes.Ldloc,firstChar);il.Emit(OpCodes.Ldloc,j);il.Emit(OpCodes.Add);il.Emit(OpCodes.Ldelema,charType);il.Emit(OpCodes.Ldfld,fcVertex);il.Emit(OpCodes.Stloc,vertexIndex);
        EmitColor4(il,colors,vertexIndex,color,colorType);
        il.Emit(OpCodes.Ldloc,j);Ldc(il,1);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,j);
        il.Append(wordLoopCond);il.Emit(OpCodes.Ldloc,charCount);il.Emit(OpCodes.Blt,wordLoopBody);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getMesh);il.Emit(OpCodes.Ldloc,colors);il.Emit(OpCodes.Callvirt,setMeshColors);
        il.Append(wordSkip);il.Append(ret);
    }
}
