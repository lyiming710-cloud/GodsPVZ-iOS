#!/usr/bin/env python3
from pathlib import Path
import re,sys,collections
if len(sys.argv)!=2: raise SystemExit('usage: first-invalid.py <playmode.log>')
lines=Path(sys.argv[1]).read_text(errors='replace').splitlines()
rx=re.compile(r'InvalidProgramException:\s*Invalid IL code in\s+([^:]+):([^\s(]+)')
hits=[]
for i,line in enumerate(lines,1):
    m=rx.search(line)
    if m: hits.append((i,m.group(1),m.group(2),line.strip()))
print('INVALID_IL_TOTAL',len(hits))
if hits:
    i,t,m,line=hits[0]
    print(f'FIRST_INVALID_IL line={i} type={t} method={m}')
    print('FIRST_INVALID_IL_TEXT',line)
counts=collections.Counter((t,m) for _,t,m,_ in hits)
for (t,m),n in counts.most_common(): print(f'INVALID_IL_METHOD count={n} type={t} method={m}')
