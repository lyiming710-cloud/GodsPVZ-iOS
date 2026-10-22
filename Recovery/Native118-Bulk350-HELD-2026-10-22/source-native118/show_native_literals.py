"""Display saved native source with literal/constant annotations, no execution."""
from pathlib import Path
import json,re,sys
P=Path(__file__).resolve().parent
o=json.loads((P/'ORIGINAL-SOURCE.json').read_text('utf-8'))
j=json.loads((P/'JUMP-TABLES-AND-LITERALS.json').read_text('utf-8'))
cells={int(x['address'],16):x for x in j['string_cells']}
constants={int(k,16):x for k,x in o['constants'].items()}
for token in sys.argv[1:]:
    m=next(x for x in o['methods'] if int(x['token'],16)==int(token,16))
    print('\nTOKEN',m['token'])
    for line in (P/(m['token']+'-original.asm')).read_text('utf-8').splitlines():
        notes=[]
        for s in re.findall(r'0x[0-9a-f]+',line.partition('#')[2]):
            a=int(s,16)
            if a in cells:notes.append(json.dumps(cells[a],ensure_ascii=True))
            if a in constants:notes.append(json.dumps(constants[a],ensure_ascii=True))
        if notes:print(line,' '.join(notes))
