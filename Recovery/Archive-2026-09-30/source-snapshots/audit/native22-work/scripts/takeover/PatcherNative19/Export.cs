using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using Mono.Cecil;
using Mono.Cecil.Cil;
internal static partial class Program
{
    static void ExportTargets(ModuleDefinition module, MethodDefinition[] methods, string path)
    {
        var types=new Dictionary<string,object>();
        string Type(TypeReference t){
            if(t==null)return null;
            var name=t.FullName;if(types.ContainsKey(name))return name;
            types[name]=new{value=t.IsValueType,baseType=(string)null,unresolved=true};
            if(t is ByReferenceType br){Type(br.ElementType);types[name]=new{value=false,baseType=(string)null,byref=true};}
            else if(t is ArrayType ar){Type(ar.ElementType);types[name]=new{value=false,baseType="System.Array",array=true};}
            else if(t is GenericParameter){types[name]=new{value=false,baseType=(string)null,unbound=true};}
            else{
                if(t is GenericInstanceType gi)foreach(var a in gi.GenericArguments)Type(a);
                TypeDefinition definition=null;try{definition=t.Resolve();}catch{}
                var parent=definition?.BaseType;
                types[name]=new{value=t.IsValueType||definition?.IsValueType==true,baseType=parent?.FullName,enumType=definition?.IsEnum==true,unresolved=definition==null};
                if(parent!=null)Type(parent);
            }
            return name;
        }
        TypeReference Bind(TypeReference t,MethodReference method){
            if(t is GenericParameter gp){
                if(gp.Type==GenericParameterType.Method&&method is GenericInstanceMethod gm)return gm.GenericArguments[gp.Position];
                if(gp.Type==GenericParameterType.Type&&method.DeclaringType is GenericInstanceType gt)return gt.GenericArguments[gp.Position];
            }
            if(t is ByReferenceType br)return new ByReferenceType(Bind(br.ElementType,method));
            if(t is ArrayType ar)return new ArrayType(Bind(ar.ElementType,method),ar.Rank);
            if(t is GenericInstanceType gi){var bound=new GenericInstanceType(gi.ElementType);foreach(var a in gi.GenericArguments)bound.GenericArguments.Add(Bind(a,method));return bound;}
            return t;
        }
        object Operand(MethodDefinition owner,object arg){
            if(arg is Instruction i)return new{target=i.Offset};
            if(arg is Instruction[] ii)return new{targets=ii.Select(i=>i.Offset).ToArray()};
            if(arg is VariableDefinition v)return new{index=v.Index};
            if(arg is ParameterDefinition p)return new{index=p.Index+(owner.HasThis?1:0)};
            if(arg is MethodReference m)return new{owner=Type(m.DeclaringType),args=m.Parameters.Select(p=>Type(Bind(p.ParameterType,m))).ToArray(),ret=Type(Bind(m.ReturnType,m)),hasThis=m.HasThis,name=m.Name,identity=m.FullName};
            if(arg is FieldReference f)return new{owner=Type(f.DeclaringType),type=Type(Bind(f.FieldType,new MethodReference("fieldContext",module.TypeSystem.Void,f.DeclaringType))),identity=f.FullName};
            if(arg is TypeReference t)return new{type=Type(t)};
            return arg;
        }
        var docs=methods.Select(m=>new{
            name=m.FullName,token=$"0x{m.MetadataToken.ToUInt32():X8}",owner=Type(m.DeclaringType),ret=Type(m.ReturnType),hasThis=m.HasThis,
            args=m.Parameters.Select(p=>Type(p.ParameterType)).ToArray(),locals=m.Body.Variables.Select(v=>Type(v.VariableType)).ToArray(),maxStack=m.Body.MaxStackSize,
            instructions=m.Body.Instructions.Select(i=>new{offset=i.Offset,opcode=i.OpCode.Name,operand=Operand(m,i.Operand)}).ToArray(),
            handlers=m.Body.ExceptionHandlers.Select(h=>new{kind=h.HandlerType.ToString(),tryStart=h.TryStart.Offset,tryEnd=h.TryEnd?.Offset??m.Body.CodeSize,handlerStart=h.HandlerStart.Offset,handlerEnd=h.HandlerEnd?.Offset??m.Body.CodeSize,catchType=Type(h.CatchType)}).ToArray()
        }).ToArray();
        File.WriteAllText(path,JsonSerializer.Serialize(new{types,methods=docs},new JsonSerializerOptions{WriteIndented=true,NumberHandling=System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals}));
    }
}
