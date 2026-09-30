using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x0600081Cu;
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static void Main(string[] args)
    {
        if(args.Length < 2 || args.Length > 3) throw new ArgumentException("usage: PatcherNative10 <native9.dll> <native10.dll> [linked]");
        bool linked=args.Length==3 && args[2]=="linked";
        var input=Path.GetFullPath(args[0]); var output=Path.GetFullPath(args[1]);
        if(!File.Exists(input)) throw new FileNotFoundException(input);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);
        using var asm=AssemblyDefinition.ReadAssembly(input); var mod=asm.MainModule; var mvid=mod.Mvid;
        if(!linked) CheckIdentity(mod,"input");
        var target=FindTarget(mod,linked); var before=Snapshot(mod,target);
        Patch(mod,target);
        if(!linked) CheckIdentity(mod,"memory"); CheckNonTargets(mod,target,before,"memory");
        asm.Write(output);
        using var reopened=AssemblyDefinition.ReadAssembly(output);
        if(reopened.MainModule.Mvid!=mvid) throw new InvalidOperationException($"MVID changed {mvid}->{reopened.MainModule.Mvid}");
        if(!linked) CheckIdentity(reopened.MainModule,"reopened");
        var rt=FindTarget(reopened.MainModule,linked); CheckNonTargets(reopened.MainModule,rt,before,"reopened");
        Console.WriteLine($"NATIVE10_PATCH_PASS linked={linked} input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
        Console.WriteLine($"TARGET token=0x{rt.MetadataToken.ToUInt32():X8} {rt.FullName} code_size={rt.Body.CodeSize} il={rt.Body.Instructions.Count} locals={rt.Body.Variables.Count} eh={rt.Body.ExceptionHandlers.Count}");
    }

    static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static TypeDefinition Type(ModuleDefinition m,string ns,string name)=>Types(m).Single(t=>t.Namespace==ns&&t.Name==name);
    static MethodDefinition FindTarget(ModuleDefinition m,bool linked)
    {
        var p=Type(m,"TMPro.Examples","WarpTextExample");
        var it=p.NestedTypes.Single(t=>t.Name=="<WarpText>d__8");
        var md=it.Methods.Single(x=>x.Name=="MoveNext"&&x.Parameters.Count==0);
        if(!linked && md.MetadataToken.ToUInt32()!=TargetToken) throw new InvalidOperationException($"target token mismatch 0x{md.MetadataToken.ToUInt32():X8}");
        if(!md.HasBody) throw new InvalidOperationException("target body missing");
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

    static FieldDefinition DF(TypeDefinition t,string name)=>t.Fields.Single(f=>f.Name==name);
    static FieldReference RF(ModuleDefinition m,string declaring,string name)=>m.GetMemberReferences().OfType<FieldReference>().First(f=>f.DeclaringType.Name==declaring&&f.Name==name);
    static MethodReference RM(ModuleDefinition m,string declaring,string name,int argc)=>m.GetMemberReferences().OfType<MethodReference>().First(x=>x.DeclaringType.Name==declaring&&x.Name==name&&x.Parameters.Count==argc);
    static void Reset(MethodDefinition m,int max){m.Body.Instructions.Clear();m.Body.Variables.Clear();m.Body.ExceptionHandlers.Clear();m.Body.InitLocals=true;m.Body.MaxStackSize=max;}
    static void LdcI4(ILProcessor il,int v){switch(v){case -1:il.Emit(OpCodes.Ldc_I4_M1);break;case 0:il.Emit(OpCodes.Ldc_I4_0);break;case 1:il.Emit(OpCodes.Ldc_I4_1);break;case 2:il.Emit(OpCodes.Ldc_I4_2);break;case 3:il.Emit(OpCodes.Ldc_I4_3);break;default:il.Emit(OpCodes.Ldc_I4,v);break;}}
    static void LoadIndex(ILProcessor il,VariableDefinition idx,int add){il.Emit(OpCodes.Ldloc,idx);if(add!=0){LdcI4(il,add);il.Emit(OpCodes.Add);}}
    static void EmitVecCtor(ILProcessor il,MethodReference ctor,Action x,Action y,Action z){x();y();z();il.Emit(OpCodes.Newobj,ctor);}
    static void EmitVertexBinary(ILProcessor il,VariableDefinition vertices,VariableDefinition vi,int add,VariableDefinition rhs,MethodReference op,TypeReference vec,bool negate,MethodReference neg)
    {il.Emit(OpCodes.Ldloc,vertices);LoadIndex(il,vi,add);il.Emit(OpCodes.Ldelema,vec);il.Emit(OpCodes.Dup);il.Emit(OpCodes.Ldobj,vec);il.Emit(OpCodes.Ldloc,rhs);if(negate)il.Emit(OpCodes.Call,neg);il.Emit(OpCodes.Call,op);il.Emit(OpCodes.Stobj,vec);}
    static void EmitMatrixVertex(ILProcessor il,VariableDefinition vertices,VariableDefinition vi,int add,VariableDefinition matrix,MethodReference multiply,TypeReference vec)
    {il.Emit(OpCodes.Ldloc,vertices);LoadIndex(il,vi,add);il.Emit(OpCodes.Ldloca,matrix);il.Emit(OpCodes.Ldloc,vertices);LoadIndex(il,vi,add);il.Emit(OpCodes.Ldelem_Any,vec);il.Emit(OpCodes.Call,multiply);il.Emit(OpCodes.Stelem_Any,vec);}

    static void Patch(ModuleDefinition mod,MethodDefinition method)
    {
        var it=method.DeclaringType; var parent=it.DeclaringType;
        if(parent==null||parent.Name!="WarpTextExample")throw new InvalidOperationException("unexpected parent");
        var fState=DF(it,"<>1__state"); var fCurrent=DF(it,"<>2__current"); var fThis=DF(it,"<>4__this");
        var fOldScale=DF(it,"<old_CurveScale>5__2"); var fOldCurve=DF(it,"<old_curve>5__3");
        var fText=DF(parent,"m_TextComponent"); var fCurve=DF(parent,"VertexCurve"); var fScale=DF(parent,"CurveScale");
        var copyCurve=parent.Methods.Single(x=>x.Name=="CopyAnimationCurve"&&x.Parameters.Count==1);

        var setPre=RM(mod,"AnimationCurve","set_preWrapMode",1); var setPost=RM(mod,"AnimationCurve","set_postWrapMode",1);
        var getKeys=RM(mod,"AnimationCurve","get_keys",0); var evaluate=RM(mod,"AnimationCurve","Evaluate",1); var getKeyValue=RM(mod,"Keyframe","get_value",0);
        var getChanged=RM(mod,"TMP_Text","get_havePropertiesChanged",0); var setChanged=RM(mod,"TMP_Text","set_havePropertiesChanged",1);
        var forceMesh=RM(mod,"TMP_Text","ForceMeshUpdate",2); var getTextInfo=RM(mod,"TMP_Text","get_textInfo",0); var getBounds=RM(mod,"TMP_Text","get_bounds",0); var update=RM(mod,"TMP_Text","UpdateVertexData",0);
        var boundsMin=RM(mod,"Bounds","get_min",0); var boundsMax=RM(mod,"Bounds","get_max",0);
        var vecCtor=RM(mod,"Vector3",".ctor",3); var vecNeg=RM(mod,"Vector3","op_UnaryNegation",1); var vecAdd=RM(mod,"Vector3","op_Addition",2); var vecSub=RM(mod,"Vector3","op_Subtraction",2);
        var vecNorm=RM(mod,"Vector3","get_normalized",0); var vecDot=RM(mod,"Vector3","Dot",2); var vecCross=RM(mod,"Vector3","Cross",2); var vecOne=RM(mod,"Vector3","get_one",0);
        var acos=RM(mod,"Mathf","Acos",1); var quatEuler=RM(mod,"Quaternion","Euler",3); var trs=RM(mod,"Matrix4x4","TRS",3); var multiply=RM(mod,"Matrix4x4","MultiplyPoint3x4",1);
        var waitCtor=RM(mod,"WaitForSeconds",".ctor",1);

        var fCharacterCount=RF(mod,"TMP_TextInfo","characterCount"); var fCharacterInfo=RF(mod,"TMP_TextInfo","characterInfo"); var fMeshInfo=RF(mod,"TMP_TextInfo","meshInfo");
        var fVisible=RF(mod,"TMP_CharacterInfo","isVisible"); var fVertexIndex=RF(mod,"TMP_CharacterInfo","vertexIndex"); var fMaterialIndex=RF(mod,"TMP_CharacterInfo","materialReferenceIndex"); var fBaseline=RF(mod,"TMP_CharacterInfo","baseLine");
        var fVertices=RF(mod,"TMP_MeshInfo","vertices"); var fVecX=RF(mod,"Vector3","x"); var fVecZ=RF(mod,"Vector3","z");

        var textInfoType=getTextInfo.ReturnType; var charInfoType=((ArrayType)fCharacterInfo.FieldType).ElementType; var meshInfoType=((ArrayType)fMeshInfo.FieldType).ElementType;
        var verticesType=(ArrayType)fVertices.FieldType; var vecType=verticesType.ElementType; var boundsType=getBounds.ReturnType; var matrixType=trs.ReturnType;

        Reset(method,12);
        var state=new VariableDefinition(mod.TypeSystem.Int32); var self=new VariableDefinition(parent); var textInfo=new VariableDefinition(textInfoType); var charCount=new VariableDefinition(mod.TypeSystem.Int32);
        var bounds=new VariableDefinition(boundsType); var minX=new VariableDefinition(mod.TypeSystem.Single); var maxX=new VariableDefinition(mod.TypeSystem.Single); var i=new VariableDefinition(mod.TypeSystem.Int32);
        var ci=new VariableDefinition(charInfoType); var vi=new VariableDefinition(mod.TypeSystem.Int32); var mi=new VariableDefinition(mod.TypeSystem.Int32); var vertices=new VariableDefinition(verticesType); var offset=new VariableDefinition(vecType);
        var x0=new VariableDefinition(mod.TypeSystem.Single); var x1=new VariableDefinition(mod.TypeSystem.Single); var y0=new VariableDefinition(mod.TypeSystem.Single); var y1=new VariableDefinition(mod.TypeSystem.Single);
        var horizontal=new VariableDefinition(vecType); var tangent=new VariableDefinition(vecType); var normalized=new VariableDefinition(vecType); var dot=new VariableDefinition(mod.TypeSystem.Single); var cross=new VariableDefinition(vecType); var angle=new VariableDefinition(mod.TypeSystem.Single);
        var quat=new VariableDefinition(quatEuler.ReturnType); var matrix=new VariableDefinition(matrixType);
        foreach(var v in new[]{state,self,textInfo,charCount,bounds,minX,maxX,i,ci,vi,mi,vertices,offset,x0,x1,y0,y1,horizontal,tangent,normalized,dot,cross,angle,quat,matrix})method.Body.Variables.Add(v);

        var il=method.Body.GetILProcessor();
        var initial=il.Create(OpCodes.Nop); var resume1=il.Create(OpCodes.Nop); var resume2=il.Create(OpCodes.Nop); var loop=il.Create(OpCodes.Nop); var process=il.Create(OpCodes.Nop);
        var forCond=il.Create(OpCodes.Ldloc,i); var forBody=il.Create(OpCodes.Ldloc,textInfo); var forStep=il.Create(OpCodes.Ldloc,i);
        var angleElse=il.Create(OpCodes.Ldc_R4,360f); var angleDone=il.Create(OpCodes.Nop); var afterFor=il.Create(OpCodes.Ldloc,self); var invalid=il.Create(OpCodes.Ldc_I4_0);

        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fThis);il.Emit(OpCodes.Stloc,self);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fState);il.Emit(OpCodes.Stloc,state);
        il.Emit(OpCodes.Ldloc,state);LdcI4(il,0);il.Emit(OpCodes.Beq,initial);il.Emit(OpCodes.Ldloc,state);LdcI4(il,1);il.Emit(OpCodes.Beq,resume1);il.Emit(OpCodes.Ldloc,state);LdcI4(il,2);il.Emit(OpCodes.Beq,resume2);il.Emit(OpCodes.Br,invalid);

        il.Append(initial);il.Emit(OpCodes.Ldarg_0);LdcI4(il,-1);il.Emit(OpCodes.Stfld,fState);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fCurve);LdcI4(il,1);il.Emit(OpCodes.Callvirt,setPre);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fCurve);LdcI4(il,1);il.Emit(OpCodes.Callvirt,setPost);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fText);LdcI4(il,1);il.Emit(OpCodes.Callvirt,setChanged);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Dup);il.Emit(OpCodes.Ldfld,fScale);il.Emit(OpCodes.Ldc_R4,10f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Stfld,fScale);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fScale);il.Emit(OpCodes.Stfld,fOldScale);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fCurve);il.Emit(OpCodes.Call,copyCurve);il.Emit(OpCodes.Stfld,fOldCurve);il.Emit(OpCodes.Br,loop);
        il.Append(resume1);il.Emit(OpCodes.Ldarg_0);LdcI4(il,-1);il.Emit(OpCodes.Stfld,fState);il.Emit(OpCodes.Br,loop);
        il.Append(resume2);il.Emit(OpCodes.Ldarg_0);LdcI4(il,-1);il.Emit(OpCodes.Stfld,fState);il.Emit(OpCodes.Br,loop);

        il.Append(loop);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getChanged);il.Emit(OpCodes.Brtrue,process);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fOldScale);il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fScale);il.Emit(OpCodes.Bne_Un,process);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,fOldCurve);il.Emit(OpCodes.Callvirt,getKeys);LdcI4(il,1);il.Emit(OpCodes.Ldelema,((ArrayType)getKeys.ReturnType).ElementType);il.Emit(OpCodes.Call,getKeyValue);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fCurve);il.Emit(OpCodes.Callvirt,getKeys);LdcI4(il,1);il.Emit(OpCodes.Ldelema,((ArrayType)getKeys.ReturnType).ElementType);il.Emit(OpCodes.Call,getKeyValue);il.Emit(OpCodes.Bne_Un,process);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldnull);il.Emit(OpCodes.Stfld,fCurrent);il.Emit(OpCodes.Ldarg_0);LdcI4(il,1);il.Emit(OpCodes.Stfld,fState);LdcI4(il,1);il.Emit(OpCodes.Ret);

        il.Append(process);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fScale);il.Emit(OpCodes.Stfld,fOldScale);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fCurve);il.Emit(OpCodes.Call,copyCurve);il.Emit(OpCodes.Stfld,fOldCurve);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fText);LdcI4(il,0);LdcI4(il,0);il.Emit(OpCodes.Callvirt,forceMesh);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getTextInfo);il.Emit(OpCodes.Stloc,textInfo);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fCharacterCount);il.Emit(OpCodes.Stloc,charCount);il.Emit(OpCodes.Ldloc,charCount);il.Emit(OpCodes.Brfalse,loop);

        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getBounds);il.Emit(OpCodes.Stloc,bounds);il.Emit(OpCodes.Ldloca,bounds);il.Emit(OpCodes.Call,boundsMin);il.Emit(OpCodes.Stloc,offset);il.Emit(OpCodes.Ldloca,offset);il.Emit(OpCodes.Ldfld,fVecX);il.Emit(OpCodes.Stloc,minX);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,getBounds);il.Emit(OpCodes.Stloc,bounds);il.Emit(OpCodes.Ldloca,bounds);il.Emit(OpCodes.Call,boundsMax);il.Emit(OpCodes.Stloc,offset);il.Emit(OpCodes.Ldloca,offset);il.Emit(OpCodes.Ldfld,fVecX);il.Emit(OpCodes.Stloc,maxX);
        LdcI4(il,0);il.Emit(OpCodes.Stloc,i);il.Emit(OpCodes.Br,forCond);

        il.Append(forBody);il.Emit(OpCodes.Ldfld,fCharacterInfo);il.Emit(OpCodes.Ldloc,i);il.Emit(OpCodes.Ldelem_Any,charInfoType);il.Emit(OpCodes.Stloc,ci);
        il.Emit(OpCodes.Ldloca,ci);il.Emit(OpCodes.Ldfld,fVisible);il.Emit(OpCodes.Brfalse,forStep);
        il.Emit(OpCodes.Ldloca,ci);il.Emit(OpCodes.Ldfld,fVertexIndex);il.Emit(OpCodes.Stloc,vi);il.Emit(OpCodes.Ldloca,ci);il.Emit(OpCodes.Ldfld,fMaterialIndex);il.Emit(OpCodes.Stloc,mi);
        il.Emit(OpCodes.Ldloc,textInfo);il.Emit(OpCodes.Ldfld,fMeshInfo);il.Emit(OpCodes.Ldloc,mi);il.Emit(OpCodes.Ldelema,meshInfoType);il.Emit(OpCodes.Ldfld,fVertices);il.Emit(OpCodes.Stloc,vertices);
        EmitVecCtor(il,vecCtor,
            ()=>{il.Emit(OpCodes.Ldloc,vertices);LoadIndex(il,vi,0);il.Emit(OpCodes.Ldelema,vecType);il.Emit(OpCodes.Ldfld,fVecX);il.Emit(OpCodes.Ldloc,vertices);LoadIndex(il,vi,2);il.Emit(OpCodes.Ldelema,vecType);il.Emit(OpCodes.Ldfld,fVecX);il.Emit(OpCodes.Add);il.Emit(OpCodes.Ldc_R4,2f);il.Emit(OpCodes.Div);},
            ()=>{il.Emit(OpCodes.Ldloca,ci);il.Emit(OpCodes.Ldfld,fBaseline);},()=>il.Emit(OpCodes.Ldc_R4,0f));il.Emit(OpCodes.Stloc,offset);
        for(int k=0;k<4;k++)EmitVertexBinary(il,vertices,vi,k,offset,vecAdd,vecType,true,vecNeg);

        il.Emit(OpCodes.Ldloca,offset);il.Emit(OpCodes.Ldfld,fVecX);il.Emit(OpCodes.Ldloc,minX);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Ldloc,maxX);il.Emit(OpCodes.Ldloc,minX);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Div);il.Emit(OpCodes.Stloc,x0);
        il.Emit(OpCodes.Ldloc,x0);il.Emit(OpCodes.Ldc_R4,0.0001f);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,x1);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fCurve);il.Emit(OpCodes.Ldloc,x0);il.Emit(OpCodes.Callvirt,evaluate);il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fScale);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Stloc,y0);
        il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fCurve);il.Emit(OpCodes.Ldloc,x1);il.Emit(OpCodes.Callvirt,evaluate);il.Emit(OpCodes.Ldloc,self);il.Emit(OpCodes.Ldfld,fScale);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Stloc,y1);
        EmitVecCtor(il,vecCtor,()=>il.Emit(OpCodes.Ldc_R4,1f),()=>il.Emit(OpCodes.Ldc_R4,0f),()=>il.Emit(OpCodes.Ldc_R4,0f));il.Emit(OpCodes.Stloc,horizontal);
        EmitVecCtor(il,vecCtor,()=>{il.Emit(OpCodes.Ldloc,x1);il.Emit(OpCodes.Ldloc,maxX);il.Emit(OpCodes.Ldloc,minX);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Ldloc,minX);il.Emit(OpCodes.Add);},()=>il.Emit(OpCodes.Ldloc,y1),()=>il.Emit(OpCodes.Ldc_R4,0f));
        EmitVecCtor(il,vecCtor,()=>{il.Emit(OpCodes.Ldloca,offset);il.Emit(OpCodes.Ldfld,fVecX);},()=>il.Emit(OpCodes.Ldloc,y0),()=>il.Emit(OpCodes.Ldc_R4,0f));il.Emit(OpCodes.Call,vecSub);il.Emit(OpCodes.Stloc,tangent);
        il.Emit(OpCodes.Ldloca,tangent);il.Emit(OpCodes.Call,vecNorm);il.Emit(OpCodes.Stloc,normalized);il.Emit(OpCodes.Ldloc,horizontal);il.Emit(OpCodes.Ldloc,normalized);il.Emit(OpCodes.Call,vecDot);il.Emit(OpCodes.Call,acos);il.Emit(OpCodes.Ldc_R4,57.2957795f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Stloc,dot);
        il.Emit(OpCodes.Ldloc,horizontal);il.Emit(OpCodes.Ldloc,tangent);il.Emit(OpCodes.Call,vecCross);il.Emit(OpCodes.Stloc,cross);il.Emit(OpCodes.Ldloca,cross);il.Emit(OpCodes.Ldfld,fVecZ);il.Emit(OpCodes.Ldc_R4,0f);il.Emit(OpCodes.Ble_Un,angleElse);
        il.Emit(OpCodes.Ldloc,dot);il.Emit(OpCodes.Stloc,angle);il.Emit(OpCodes.Br,angleDone);il.Append(angleElse);il.Emit(OpCodes.Ldloc,dot);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,angle);il.Append(angleDone);
        EmitVecCtor(il,vecCtor,()=>il.Emit(OpCodes.Ldc_R4,0f),()=>il.Emit(OpCodes.Ldloc,y0),()=>il.Emit(OpCodes.Ldc_R4,0f));il.Emit(OpCodes.Ldc_R4,0f);il.Emit(OpCodes.Ldc_R4,0f);il.Emit(OpCodes.Ldloc,angle);il.Emit(OpCodes.Call,quatEuler);il.Emit(OpCodes.Stloc,quat);il.Emit(OpCodes.Ldloc,quat);il.Emit(OpCodes.Call,vecOne);il.Emit(OpCodes.Call,trs);il.Emit(OpCodes.Stloc,matrix);
        for(int k=0;k<4;k++)EmitMatrixVertex(il,vertices,vi,k,matrix,multiply,vecType);for(int k=0;k<4;k++)EmitVertexBinary(il,vertices,vi,k,offset,vecAdd,vecType,false,vecNeg);

        il.Append(forStep);LdcI4(il,1);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,i);il.Append(forCond);il.Emit(OpCodes.Ldloc,charCount);il.Emit(OpCodes.Blt,forBody);
        il.Append(afterFor);il.Emit(OpCodes.Ldfld,fText);il.Emit(OpCodes.Callvirt,update);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldc_R4,0.025f);il.Emit(OpCodes.Newobj,waitCtor);il.Emit(OpCodes.Stfld,fCurrent);il.Emit(OpCodes.Ldarg_0);LdcI4(il,2);il.Emit(OpCodes.Stfld,fState);LdcI4(il,1);il.Emit(OpCodes.Ret);
        il.Append(invalid);il.Emit(OpCodes.Ret);
    }
}
