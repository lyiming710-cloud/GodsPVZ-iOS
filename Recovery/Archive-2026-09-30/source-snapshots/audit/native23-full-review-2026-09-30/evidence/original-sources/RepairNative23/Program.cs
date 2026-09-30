using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Mono.Cecil;
using Mono.Cecil.Cil;

// RepairNative23 -- strictly audited IL repairs.
//   * every edit names its token, IL offset and expected action
//   * if ANY target site is missing or ambiguous nothing is written
//   * branch targets are re-pointed by ILProcessor.Replace so control flow survives
internal static class Program
{
    static int Main(string[] a)
    {
        if (a.Length < 3)
        {
            Console.Error.WriteLine("usage: RepairNative23 <in.dll> <out.dll> <edits.json> [report.txt]");
            return 2;
        }
        var input = Path.GetFullPath(a[0]);
        var output = Path.GetFullPath(a[1]);
        var edits = Parse(File.ReadAllText(a[2]));
        var reportPath = a.Length > 3 ? a[3] : null;

        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(Path.GetDirectoryName(input));
        var extra = Environment.GetEnvironmentVariable("GODSPVZ_RESOLVER");
        if (!string.IsNullOrEmpty(extra)) resolver.AddSearchDirectory(extra);
        resolver.AddSearchDirectory("/workspaces/GodsPVZ-native19/Tools/Stage9Native4Recovery/resolver");

        using var asm = AssemblyDefinition.ReadAssembly(input, new ReaderParameters { AssemblyResolver = resolver });
        var mod = asm.MainModule;

        var byToken = new Dictionary<uint, MethodDefinition>();
        foreach (var t in AllTypes(mod))
            foreach (var m in t.Methods)
                byToken[m.MetadataToken.ToUInt32()] = m;

        var report = new StringBuilder();
        bool fail = false;

        foreach (var g in edits.GroupBy(e => e.Token))
        {
            uint tok;
            try { tok = Convert.ToUInt32(g.Key, 16); }
            catch { Console.Error.WriteLine("bad token " + g.Key); fail = true; continue; }
            if (!byToken.TryGetValue(tok, out var method))
            {
                Console.Error.WriteLine($"!! no MethodDefinition for token {g.Key}");
                fail = true; continue;
            }
            if (!method.HasBody)
            {
                Console.Error.WriteLine($"!! {g.Key} ({method.FullName}) has no body");
                fail = true; continue;
            }
            var body = method.Body;
            var il = body.GetILProcessor();
            var byOff = new Dictionary<int, Instruction>();
            foreach (var i in body.Instructions) byOff[i.Offset] = i;

            foreach (var e in g.OrderBy(x => x.Offset))
            {
                if (!byOff.TryGetValue(e.Offset, out var ins))
                {
                    Console.Error.WriteLine($"!! {g.Key} IL_{e.Offset:X4} not found");
                    fail = true; continue;
                }
                string before = Describe(ins);
                Instruction shown = ins;
                bool ok; string note;
                switch (e.Action)
                {
                    case "retype_call":
                        ok = RetypeCall(mod, ins, e.NewDeclaringType, out note); break;
                    case "to_ldnull":
                        ok = Swap(il, body, ins, Instruction.Create(OpCodes.Ldnull), out shown, out note); break;
                    case "ret_to_throw":
                        if (ins.OpCode != OpCodes.Ret) { note = "not a ret"; ok = false; }
                        else ok = Swap(il, body, ins, Instruction.Create(OpCodes.Throw), out shown, out note);
                        break;
                    default:
                        note = "unknown action " + e.Action; ok = false; break;
                }
                if (!ok)
                {
                    Console.Error.WriteLine($"!! {g.Key} IL_{e.Offset:X4} {e.Action} FAILED: {note}");
                    fail = true; continue;
                }
                report.AppendLine($"{g.Key}\tIL_{e.Offset:X4}\t{e.Action}\t{before}\t->\t{Describe(shown)}");
            }
        }

        if (fail)
        {
            Console.Error.WriteLine("ABORT: no output written");
            return 3;
        }
        Directory.CreateDirectory(Path.GetDirectoryName(output) ?? ".");
        asm.Write(output);
        Console.WriteLine($"wrote {output}");
        if (reportPath != null) File.WriteAllText(reportPath, report.ToString());
        Console.Write(report.ToString());
        return 0;
    }

    static bool RetypeCall(ModuleDefinition mod, Instruction ins, string typeName, out string note)
    {
        note = "";
        if (!(ins.Operand is MethodReference mr)) { note = "operand is not a MethodReference"; return false; }
        if (ins.OpCode != OpCodes.Callvirt && ins.OpCode != OpCodes.Call)
        { note = "opcode is " + ins.OpCode; return false; }
        var td = ResolveType(mod, typeName);
        if (td == null) { note = "cannot resolve type " + typeName; return false; }
        if (td.Methods.Count == 0 && Environment.GetEnvironmentVariable("REPAIR_DEBUG") == "1")
            Console.Error.WriteLine($"[debug] {typeName} came from module {td.Module.FileName} but has 0 methods");
        if (ReferenceEquals(td, mr.DeclaringType.Resolve()))
        { note = "declaring type already " + typeName; return false; }
        var picks = td.Methods.Where(c => SameShape(c, mr)).ToList();
        if (picks.Count != 1)
        {
            var names = string.Join(",", td.Methods.Select(x => x.Name).Distinct().Take(40));
            note = $"{picks.Count} overloads named {mr.Name} on {td.FullName} " +
                   $"[module={td.Module.FileName} methods={td.Methods.Count} hasThis={mr.HasThis} " +
                   $"generic={mr.HasGenericParameters} params={mr.Parameters.Count}] " +
                   $"available={names}";
            return false;
        }
        MethodReference nr = mod.ImportReference(picks[0]);
        if (mr is GenericInstanceMethod gim)
        {
            var gi = new GenericInstanceMethod(nr);
            foreach (var ga in gim.GenericArguments) gi.GenericArguments.Add(mod.ImportReference(ga));
            nr = gi;
        }
        ins.Operand = nr;
        return true;
    }

    static TypeDefinition ResolveType(ModuleDefinition mod, string name)
    {
        var t = mod.GetType(name) ?? mod.ExportedTypes.FirstOrDefault(x => x.FullName == name)?.Resolve();
        if (t != null) return t;
        foreach (var ar in mod.AssemblyReferences)
        {
            AssemblyDefinition ad;
            try { ad = mod.AssemblyResolver.Resolve(ar); } catch { continue; }
            if (ad == null) continue;
            var found = ad.MainModule.GetType(name);
            if (found != null) return found;
        }
        return AllTypes(mod).FirstOrDefault(x => x.FullName == name);
    }

    static bool SameShape(MethodDefinition c, MethodReference mr)
    {
        if (c.Name != mr.Name) return false;
        if (c.HasThis != mr.HasThis) return false;
        if (c.Parameters.Count != mr.Parameters.Count) return false;
        // Arity is decided by the CALL SITE: a GenericInstanceMethod carries its own
        // arguments, so the target must be a generic method of exactly that arity.
        if (mr is GenericInstanceMethod gim)
        {
            if (!c.HasGenericParameters) return false;
            if (c.GenericParameters.Count != gim.GenericArguments.Count) return false;
        }
        else if (c.HasGenericParameters)
        {
            return false;
        }
        for (int i = 0; i < c.Parameters.Count; i++)
            if (c.Parameters[i].ParameterType.FullName != mr.Parameters[i].ParameterType.FullName) return false;
        return true;
    }

    static bool Swap(ILProcessor il, MethodBody body, Instruction old, Instruction neu,
                     out Instruction shown, out string note)
    {
        note = "";
        shown = neu;
        var referrers = body.Instructions.Where(x => ReferenceEquals(x.Operand, old)).ToList();
        il.Replace(old, neu);
        foreach (var r in referrers) r.Operand = neu;
        return true;
    }

    static IEnumerable<TypeDefinition> AllTypes(ModuleDefinition mod)
    {
        foreach (var t in mod.Types)
        {
            yield return t;
            foreach (var n in Nested(t)) yield return n;
        }
    }
    static IEnumerable<TypeDefinition> Nested(TypeDefinition t)
    {
        foreach (var n in t.NestedTypes)
        {
            yield return n;
            foreach (var x in Nested(n)) yield return x;
        }
    }

    static string Describe(Instruction i)
    {
        if (i.Operand == null) return i.OpCode.Name;
        if (i.Operand is MethodReference m)
        {
            var s = m.FullName;
            if (i.Operand is GenericInstanceMethod g)
                s = g.ElementMethod.FullName + "<" + string.Join(",", g.GenericArguments.Select(x => x.FullName)) + ">";
            return i.OpCode.Name + " " + s;
        }
        if (i.Operand is FieldReference f) return i.OpCode.Name + " " + f.FullName;
        if (i.Operand is Instruction t) return i.OpCode.Name + " -> IL_" + t.Offset.ToString("X4");
        return i.OpCode.Name + " " + i.Operand;
    }

    sealed class Edit
    {
        public string Token { get; set; }
        public int Offset { get; set; }
        public string Action { get; set; }
        public string NewDeclaringType { get; set; }
    }

    static List<Edit> Parse(string json)
    {
        var doc = JsonDocument.Parse(json, new JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip });
        var list = new List<Edit>();
        foreach (var x in doc.RootElement.GetProperty("edits").EnumerateArray())
            list.Add(new Edit
            {
                Token = x.GetProperty("token").GetString(),
                Offset = x.GetProperty("offset").GetInt32(),
                Action = x.GetProperty("action").GetString(),
                NewDeclaringType = x.TryGetProperty("newDeclaringType", out var t) ? t.GetString() : null
            });
        return list;
    }
}
