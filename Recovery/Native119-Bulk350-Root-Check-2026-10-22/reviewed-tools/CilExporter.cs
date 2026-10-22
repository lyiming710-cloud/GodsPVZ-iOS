using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Text.Json;

static class CilExporter {
    static DefaultAssemblyResolver? Resolver;
    static Dictionary<string, object> Types = new();

    static TypeReference Subst(TypeReference t, TypeReference? declType, MethodReference? methRef) {
        if (t is GenericParameter gp) {
            if (gp.Type == GenericParameterType.Type && declType is GenericInstanceType git && gp.Position < git.GenericArguments.Count) {
                var arg = git.GenericArguments[gp.Position];
                if (arg is GenericParameter gp2 && gp2.Type == gp.Type && gp2.Position == gp.Position) return arg;
                return Subst(arg, null, methRef);
            }
            if (gp.Type == GenericParameterType.Method && methRef is GenericInstanceMethod gim && gp.Position < gim.GenericArguments.Count) {
                var arg = gim.GenericArguments[gp.Position];
                if (arg is GenericParameter gp2 && gp2.Type == gp.Type && gp2.Position == gp.Position) return arg;
                return Subst(arg, declType, null);
            }
            return t;
        }
        if (t is ByReferenceType br)
            return new ByReferenceType(Subst(br.ElementType, declType, methRef));
        if (t is ArrayType ar)
            return new ArrayType(Subst(ar.ElementType, declType, methRef), ar.Rank);
        if (t is GenericInstanceType g) {
            var ng = new GenericInstanceType(g.ElementType);
            foreach (var a in g.GenericArguments)
                ng.GenericArguments.Add(Subst(a, declType, methRef));
            return ng;
        }
        return t;
    }

    static string RegType(TypeReference? tr, TypeReference? declType = null, MethodReference? methRef = null) {
        if (tr == null) return "System.Void";
        var t = Subst(tr, declType, methRef);
        string name = t.FullName;
        if (Types.ContainsKey(name)) return name;

        if (t is ByReferenceType br) {
            Types[name] = new { value = false, baseType = (string?)null, byref = true };
            RegType(br.ElementType, declType, methRef);
            return name;
        }
        if (t is ArrayType ar) {
            Types[name] = new { value = false, baseType = "System.Array", array = ar.ElementType.FullName };
            RegType(ar.ElementType, declType, methRef);
            return name;
        }
        if (t is GenericParameter) {
            Types[name] = new { value = false, baseType = (string?)null, unbound = true };
            return name;
        }

        TypeDefinition? td = null;
        try { td = t.Resolve(); } catch { td = null; }
        if (td == null) {
            Types[name] = new { value = t.IsValueType, baseType = t.IsValueType ? "System.ValueType" : (string?)null, enumType = false, unresolved = true };
            return name;
        }

        string? baseName = null;
        if (td.BaseType != null) {
            var bt = Subst(td.BaseType, t, null);
            baseName = bt.FullName;
        }
        bool isEnum = td.IsEnum;
        bool isVal = td.IsValueType || isEnum;
        var interfaceTypes=td.Interfaces.Select(i=>Subst(i.InterfaceType,t,null)).ToArray();
        Types[name] = new { value = isVal, baseType = baseName, enumType = isEnum, unresolved = false, interfaces=interfaceTypes.Select(i=>i.FullName).ToArray() };
        foreach(var it in interfaceTypes)RegType(it);
        if (td.BaseType != null) {
            RegType(Subst(td.BaseType, t, null));
        }
        return name;
    }

    static object? FormatOperand(Instruction ins, MethodDefinition m) {
        string op = ins.OpCode.Name;
        object? a = ins.Operand;
        if (a == null) return null;
        if (a is Instruction target) return new { target = target.Offset };
        if (a is Instruction[] targets) return new { targets = targets.Select(x => x.Offset).ToArray() };
        if (a is ParameterDefinition pd) {
            int ix = m.HasThis ? pd.Index + 1 : pd.Index;
            return new { index = ix };
        }
        if (a is VariableDefinition vd) return new { index = vd.Index };
        if (a is FieldReference fr) {
            string owner = RegType(fr.DeclaringType, m.DeclaringType, m);
            string ftype = RegType(fr.FieldType, fr.DeclaringType, m);
            return new { owner, type = ftype, identity = fr.FullName };
        }
        if (a is MethodReference mr) {
            string owner = RegType(mr.DeclaringType, m.DeclaringType, mr);
            var args = mr.Parameters.Select(p => RegType(p.ParameterType, mr.DeclaringType, mr)).ToArray();
            string ret = RegType(mr.ReturnType, mr.DeclaringType, mr);
            return new { owner, args, ret, hasThis = mr.HasThis, name = mr.Name, identity = mr.FullName };
        }
        if (a is TypeReference tr) {
            string tname = RegType(tr, m.DeclaringType, m);
            return new { type = tname };
        }
        if (a is sbyte sb) return (int)sb;
        if (a is byte b) return (int)b;
        if (a is int i4) return i4;
        if (a is long i8) return i8;
        if (a is float f4) return f4;
        if (a is double f8) return f8;
        if (a is string s) return s;
        return a.ToString();
    }

    static int Main(string[] args) {
        if (args.Length < 2) throw new Exception("Usage: CilExporter <dll> <out.json> [supportDir] [token1,token2...]");
        Resolver = new DefaultAssemblyResolver();
        string supportDir = args.Length >= 3 ? args[2] : (Environment.GetEnvironmentVariable("GODSPVZ_RESOLVER") ?? "references/managed-support");
        if (Directory.Exists(supportDir)) Resolver.AddSearchDirectory(supportDir);
        string? dllDir = Path.GetDirectoryName(Path.GetFullPath(args[0]));
        if (!string.IsNullOrEmpty(dllDir)) Resolver.AddSearchDirectory(dllDir);

        HashSet<int>? filterTokens = null;
        if (args.Length >= 4 && args[3] != "export-all") {
            filterTokens = args[3].Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x.Trim(), 16)).ToHashSet();
        }

        Types["System.Object"] = new { value = false, baseType = (string?)null, enumType = false, unresolved = false };
        Types["System.String"] = new { value = false, baseType = "System.Object", enumType = false, unresolved = false };
        Types["System.Array"] = new { value = false, baseType = "System.Object", enumType = false, unresolved = false };
        Types["System.ValueType"] = new { value = false, baseType = "System.Object", enumType = false, unresolved = false };
        Types["System.Enum"] = new { value = true, baseType = "System.ValueType", enumType = true, unresolved = false };
        using var asm = AssemblyDefinition.ReadAssembly(args[0], new ReaderParameters { AssemblyResolver = Resolver });
        foreach (var t in asm.MainModule.GetTypes()) RegType(t);

        var methods = new List<object>();
        foreach (var t in asm.MainModule.GetTypes()) {
            foreach (var m in t.Methods) {
                if (!m.HasBody) continue;
                int tok = m.MetadataToken.ToInt32();
                if (filterTokens != null && !filterTokens.Contains(tok)) continue;

                string owner = RegType(m.DeclaringType);
                string ret = RegType(m.ReturnType, m.DeclaringType, m);
                var margs = m.Parameters.Select(p => RegType(p.ParameterType, m.DeclaringType, m)).ToArray();
                var locals = m.Body.Variables.Select(v => RegType(v.VariableType, m.DeclaringType, m)).ToArray();
                var instrs = m.Body.Instructions.Select(ins => new {
                    offset = ins.Offset,
                    opcode = ins.OpCode.Name,
                    operand = FormatOperand(ins, m)
                }).ToArray();
                var handlers = m.Body.ExceptionHandlers.Select(h => new {
                    kind = h.HandlerType.ToString(),
                    tryStart = h.TryStart?.Offset ?? 0,
                    tryEnd = h.TryEnd?.Offset ?? 0,
                    handlerStart = h.HandlerStart?.Offset ?? 0,
                    handlerEnd = h.HandlerEnd?.Offset ?? 0,
                    catchType = h.CatchType != null ? RegType(h.CatchType, m.DeclaringType, m) : null
                }).ToArray();

                methods.Add(new {
                    name = m.FullName,
                    token = $"0x{tok:X8}",
                    owner,
                    ret,
                    hasThis = m.HasThis,
                    args = margs,
                    locals,
                    maxStack = m.Body.MaxStackSize,
                    instructions = instrs,
                    handlers
                });
            }
        }

        var root = new { types = Types, methods };
        File.WriteAllText(args[1], JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = true, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals }));
        return 0;
    }
}
