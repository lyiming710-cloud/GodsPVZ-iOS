from pathlib import Path
import ast,json
s=Path('audit/native73-codex-2026-10-12/triage_and_state.py').read_text();a=s.index('done=')+5;b=s.index('\nfor m',a)
done=ast.literal_eval(s[a:b])|{0x61D,0x483,0x238,0x365,0x75F,0x3A0,0x206,0x1AC,0x3C5,0x87D,0x734,0xF,0x10,0x39B,0x27C,0x208,0x173,0x5CE,0x2B5,0x2B7,0x56F,0x6DB,0x39,0x322,0x6D4,0x703,0x3D9,0x491}
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text())
for m in sorted(d['methods'],key=lambda m:int(m['end'],16)-int(m['start'],16)):
 if int(m['token'],16)&0xffffff not in done:print(m['token'],m['method'],int(m['end'],16)-int(m['start'],16),m['diagnostics_before'])
