from pathlib import Path
import subprocess,json
w=Path('/workspaces/GodsPVZ-native24-codespace');native=Path('/workspaces/GodsPVZ-native19/.validation/native22/pcnative')
scope={'__name__':'native28_helpers'};exec((w/'scripts/codespaces/native23_native.py').read_text(),scope);pe=scope['PE'](native/'GameAssembly.dll')
out=w/'.validation/native28-2026-10-01';out.mkdir(exist_ok=True)
mapping=json.loads((native/'native-method-map.json').read_text())
print('EQUALITY', [x for x in mapping if int(x['va'],16)==0x18131f760])
for va in [0x180cc4fc0]:
 matches=[end for begin,end in pe.pdata_entries() if begin==va]
 end=matches[0] if matches else va+128
 print('BOUNDARY',hex(va),hex(end),'pdata' if matches else '128-byte inspection window; function end unproved')
 p=out/(hex(va)+'.bin');p.write_bytes(pe.data[pe.va_to_offset(va):pe.va_to_offset(va)+end-va])
 asm=subprocess.check_output(['objdump','-D','-b','binary','-m','i386:x86-64','--adjust-vma='+hex(va),str(p)],text=True)
 (out/(hex(va)+'.asm')).write_text(asm);print(asm)
