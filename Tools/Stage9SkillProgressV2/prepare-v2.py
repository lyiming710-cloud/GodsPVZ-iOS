#!/usr/bin/env python3
from pathlib import Path
import sys

if len(sys.argv) != 2:
    raise SystemExit("usage: prepare-v2.py <Program.cs>")

p = Path(sys.argv[1])
s = p.read_text()

def replace_once(old: str, new: str):
    global s
    n = s.count(old)
    if n != 1:
        raise SystemExit(f"expected exactly one match, found {n}: {old[:120]!r}")
    s = s.replace(old, new, 1)

replace_once(
'''if (args.Length != 3)\n{\n    Console.Error.WriteLine("Usage: Stage9PlantDetailSkillProgressPatch <input.dll> <output.dll> <expected-input-sha256>");\n    return 2;\n}''',
'''if (args.Length != 4)\n{\n    Console.Error.WriteLine("Usage: Stage9PlantDetailSkillProgressPatch <input.dll> <output.dll> <expected-input-sha256> <unityjit-linux-mscorlib.dll>");\n    return 2;\n}''')

replace_once(
'''const string NativeSpanSha = "5050f85b69db4e37dcf86232a9c388ac32e4b622972556a8bfa4340cf3f8dbe4";''',
'''const string NativeSpanSha = "5050f85b69db4e37dcf86232a9c388ac32e4b622972556a8bfa4340cf3f8dbe4";\nconst string ExpectedCorelibSha = "4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be";''')

replace_once(
'''    return hits[0];\n}\n\nvar input = Path.GetFullPath(args[0]);''',
'''    return hits[0];\n}\n\nstatic MethodReference MakeHostInstanceMethod(ModuleDefinition targetModule, MethodDefinition openDefinition, TypeReference closedHost)\n{\n    var open = targetModule.ImportReference(openDefinition);\n    var host = new MethodReference(open.Name, open.ReturnType, closedHost)\n    {\n        HasThis = open.HasThis,\n        ExplicitThis = open.ExplicitThis,\n        CallingConvention = open.CallingConvention\n    };\n    foreach (var p in open.Parameters)\n        host.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, p.ParameterType));\n    foreach (var gp in open.GenericParameters)\n        host.GenericParameters.Add(new GenericParameter(gp.Name, host));\n    return host;\n}\n\nstatic void AssertOpenListGetItemSignature(MethodReference getItem)\n{\n    if (getItem.Name != "get_Item" || getItem.Parameters.Count != 1 || getItem.Parameters[0].ParameterType.FullName != "System.Int32")\n        throw new InvalidDataException("List<T>.get_Item signature drift");\n    if (getItem.ReturnType is not GenericParameter gp || gp.Position != 0 || gp.Type != GenericParameterType.Type)\n        throw new InvalidDataException("List<T>.get_Item return lost open !0 generic variable");\n    if (getItem.DeclaringType is not GenericInstanceType host || host.ElementType.FullName != "System.Collections.Generic.List`1" ||\n        host.GenericArguments.Count != 1 || host.GenericArguments[0].FullName != "UnityEngine.UI.Image")\n        throw new InvalidDataException("List<Image>.get_Item closed host drift");\n}\n\nvar input = Path.GetFullPath(args[0]);''')

replace_once(
'''var expectedInputSha = args[2].Trim().ToLowerInvariant();\nif (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA is not 64 hex chars");\nif (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");\nConsole.WriteLine($"INPUT_SHA256 {Sha(input)}");''',
'''var expectedInputSha = args[2].Trim().ToLowerInvariant();\nvar corelib = Path.GetFullPath(args[3]);\nif (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA is not 64 hex chars");\nif (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");\nif (Sha(corelib) != ExpectedCorelibSha) throw new InvalidDataException("Unity corelib SHA mismatch");\nConsole.WriteLine($"INPUT_SHA256 {Sha(input)}");\nConsole.WriteLine($"UNITYJIT_LINUX_MSCORLIB_SHA256 {Sha(corelib)}");''')

replace_once(
'''string beforeRefs;\n\nusing (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))''',
'''string beforeRefs;\n\nusing var core = ModuleDefinition.ReadModule(corelib, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate });\nvar listOpen = core.GetType("System.Collections.Generic.List`1") ?? throw new InvalidDataException("List`1 not found in exact corelib");\nvar getItemDef = listOpen.Methods.Single(m => m.Name == "get_Item" && !m.IsStatic && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Int32");\nif (getItemDef.ReturnType is not GenericParameter gp || gp.Position != 0 || gp.Type != GenericParameterType.Type)\n    throw new InvalidDataException("exact corelib List<T>.get_Item return is not !0");\n\nusing (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))''')

replace_once(
'''    var listImageGetItem = FindRef(methods, m => m.Name == "get_Item" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Int32" && m.ReturnType.FullName == "UnityEngine.UI.Image" && m.DeclaringType.FullName.Contains("System.Collections.Generic.List`1", StringComparison.Ordinal), "List<Image>.get_Item");''',
'''    if (skillLogo.FieldType is not GenericInstanceType listImageType || listImageType.ElementType.FullName != "System.Collections.Generic.List`1" ||\n        listImageType.GenericArguments.Count != 1 || listImageType.GenericArguments[0].FullName != "UnityEngine.UI.Image")\n        throw new InvalidDataException("skillLogo field is not List<Image>");\n    var listImageGetItem = MakeHostInstanceMethod(module, getItemDef, listImageType);\n    AssertOpenListGetItemSignature(listImageGetItem);''')

replace_once(
'''    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 typed_array_ldelem_ref=1 public_Image_fillAmount_getter=1 no_private_field_access=1 metadata_changes=0 exception_swallowing=0");''',
'''    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 typed_array_ldelem_ref=1 public_Image_fillAmount_getter=1 exact_unityjit_linux_corelib_get_Item=1 open_var_signature=1 no_private_field_access=1 metadata_changes=0 exception_swallowing=0");''')

replace_once(
'''using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))''',
'''var resolver = new DefaultAssemblyResolver();\nresolver.AddSearchDirectory(Path.GetDirectoryName(corelib)!);\nresolver.AddSearchDirectory(Path.GetDirectoryName(output)!);\nusing (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate, AssemblyResolver=resolver }))''')

replace_once(
'''    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldelem_Ref) != 18) throw new InvalidDataException("typed TextMeshPro array access count mismatch");\n    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();''',
'''    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldelem_Ref) != 18) throw new InvalidDataException("typed TextMeshPro array access count mismatch");\n    var listGetItemRefs = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()\n        .Where(m => m.Name == "get_Item" && m.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1<", StringComparison.Ordinal)).ToList();\n    if (listGetItemRefs.Count != 21) throw new InvalidDataException($"List<Image>.get_Item call count {listGetItemRefs.Count}");\n    foreach (var r in listGetItemRefs) AssertOpenListGetItemSignature(r);\n    var resolvedGetItem = listGetItemRefs[0].Resolve() ?? throw new InvalidDataException("List<Image>.get_Item MemberRef did not resolve");\n    if (resolvedGetItem.Name != "get_Item" || resolvedGetItem.DeclaringType.FullName != "System.Collections.Generic.List`1")\n        throw new InvalidDataException("List<Image>.get_Item resolved to wrong MethodDef");\n    var resolvedCorelibPath = resolvedGetItem.Module.FileName;\n    if (string.IsNullOrEmpty(resolvedCorelibPath) || Sha(resolvedCorelibPath) != ExpectedCorelibSha)\n        throw new InvalidDataException("List<Image>.get_Item resolved against wrong corelib");\n    Console.WriteLine($"GENERIC_MEMBERREF_RESOLUTION_PASS List_Image_get_Item=21 corelib_sha256={ExpectedCorelibSha} open_var_signature=1");\n    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();''')

replace_once(
'''    Console.WriteLine($"REOPEN_SKILL_PROGRESS_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=1 object_locals=0 private_fill_refs=0 public_fill_get=1 fill_set=6 SetActive=23 TMP_set_text=7 TMP_set_color=2 typed_array_ldelem_ref=18 Color_ctor=2");''',
'''    Console.WriteLine($"REOPEN_SKILL_PROGRESS_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=1 object_locals=0 private_fill_refs=0 public_fill_get=1 fill_set=6 SetActive=23 TMP_set_text=7 TMP_set_color=2 typed_array_ldelem_ref=18 Color_ctor=2 ListImage_get_Item=21");''')

p.write_text(s)
print("SKILLPROGRESS_V2_SOURCE_PREP_PASS exact_corelib_get_Item=1 replacements=10")
