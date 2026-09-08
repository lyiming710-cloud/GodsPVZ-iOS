from pathlib import Path

p = Path('Tools/HF5Patch/Program.cs')
s = p.read_text(encoding='utf-8')
old = '''    // animationGroup.transform -> LoopAddAnimation
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimationGroup); E(il, OpCodes.Call, gameObjectGetTransform);
    E(il, OpCodes.Ldarg_0); // reorder to call instance(this, transform): stash transform in v1 is impossible (Vector3), use temporary Transform local below
    // replace the just-emitted order using a Transform local: pop and rebuild cleanly below is not valid; method is constructed in one pass.
    il.Body.Instructions.Clear();
    b.ExceptionHandlers.Clear();
    var transformLocal = new VariableDefinition(gameObjectGetTransform.ReturnType); b.Variables.Add(transformLocal);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimationGroup); E(il, OpCodes.Call, gameObjectGetTransform); E(il, OpCodes.Stloc, transformLocal);
'''
new = '''    // animationGroup.transform -> LoopAddAnimation
    var transformLocal = new VariableDefinition(gameObjectGetTransform.ReturnType); b.Variables.Add(transformLocal);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimationGroup); E(il, OpCodes.Call, gameObjectGetTransform); E(il, OpCodes.Stloc, transformLocal);
'''
if old not in s:
    raise SystemExit('HF5 source cleanup block not found')
s = s.replace(old, new, 1)
p.write_text(s, encoding='utf-8')
print('HF5 source normalized')
