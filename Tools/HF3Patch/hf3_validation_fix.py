from pathlib import Path

p = Path('Tools/HF3Patch/Program.cs')
s = p.read_text(encoding='utf-8')
old = '''// Shared death check for generic/ID17/ID18. Native condition is ordered health <= 0 and !isDied.
il.Append(deathCheck);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDied); E(il, OpCodes.Brtrue, retDisabled);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble, doDie);
E(il, OpCodes.Br, retDisabled);
il.Append(doDie);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg, noResidue); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, die);
E(il, OpCodes.Br, retDisabled);'''
new = '''// Shared death check for generic/ID17/ID18. Match PC native order exactly: health first, then isDied.
il.Append(deathCheck);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Bgt_Un, retDisabled);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDied); E(il, OpCodes.Brtrue, retDisabled);
il.Append(doDie);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg, noResidue); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, die);
E(il, OpCodes.Br, retDisabled);'''
if old not in s:
    raise SystemExit('HF3 shared death block not found')
s = s.replace(old, new, 1)
p.write_text(s, encoding='utf-8')
print('HF3 validation fix applied: native health-before-isDied death ordering')
