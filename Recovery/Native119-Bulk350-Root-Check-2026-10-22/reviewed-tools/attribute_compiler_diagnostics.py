"""Re-attribute real Clang diagnostics with lexical brace-balanced method bodies.
Compiler attribution is still scoped evidence, never runtime/behavior equivalence.
"""
from pathlib import Path,PurePosixPath
import re,json,tarfile,bisect,hashlib,sys
V=Path(__file__).resolve().parent;A=V/'actions-39790796339';B=A/'bulk350-linux';texts={}
with tarfile.open(A/'bulk350-generated-cpp.tar.gz','r:gz') as tf:
    for entry in tf:
        path=PurePosixPath(entry.name)
        if entry.isfile() and path.name.startswith('GodsPVZRuntime1') and path.suffix in ('.cpp','.c'):
            texts[path.name]=tf.extractfile(entry).read().decode('utf-8')
codegen=texts['GodsPVZRuntime1_CodeGen.c'];block=re.search(r's_methodPointers\[\d+\]\s*=\s*\{(.*?)\};',codegen,re.S)[1]
symbol_tokens={}
for i,s in enumerate(x.strip() for x in block.split(',') if x.strip()):symbol_tokens.setdefault(s,[]).append(f'0x{0x06000001+i:08X}')
literal=re.compile(r'//[^\n]*|/\*.*?\*/|"(?:\\.|[^"\\])*"|\x27(?:\\.|[^\x27\\])*\x27',re.S)
ranges={};mapped=set();source_hashes={}
for name,text in texts.items():
    if not name.endswith('.cpp'):continue
    clean=literal.sub(lambda m:''.join('\n' if c=='\n' else ' ' for c in m[0]),text)
    stack=[];ends={}
    for i,c in enumerate(clean):
        if c=='{':stack.append(i)
        elif c=='}':
            if not stack:raise ValueError('Unbalanced C++ closing brace: '+name)
            ends[stack.pop()]=i
    if stack:raise ValueError('Unbalanced C++ opening brace: '+name)
    newline=[i for i,c in enumerate(clean) if c=='\n'];rows=[]
    for m in re.finditer(r'(?m)^IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR[^\n;]*?([A-Za-z0-9_]+_m[A-Fa-f0-9]{40})\s*\([^\n]*\)\s*\n\{',clean):
        opening=clean.find('{',m.start(),m.end());closing=ends[opening];tokens=symbol_tokens.get(m[1],[])
        start=bisect.bisect_left(newline,m.start())+1;end=bisect.bisect_left(newline,closing)+1
        rows.append(dict(start=start,end=end,symbol=m[1],tokens=tokens));mapped.update(tokens)
    ranges[name]=rows;source_hashes[name]=hashlib.sha256(text.encode()).hexdigest()
def owner(name,line):
    rows=[r for r in ranges.get(name,[]) if r['start']<=line<=r['end']]
    if len(rows)>1:raise ValueError('Overlapping method body intervals')
    return rows[0] if rows else None
direct=[];headers=[];unmapped=[]
for log in sorted((B/'results/clang_logs').glob('GodsPVZRuntime1*.cpp.log')):
    active=None;tu=log.name[:-4]
    for line in log.read_text('utf-8').splitlines():
        e=re.match(r'^([^:]+):(\d+):\d+: (?:fatal )?error: (.*)',line)
        if e:
            src=PurePosixPath(e[1]).name;ln=int(e[2]);row=dict(tu=tu,file=src,line=ln,message=e[3]);active=None
            if src==tu:
                o=owner(src,ln);row['owner']=o
                if o and o['tokens']:direct.append(row)
                else:unmapped.append(row)
            else:row['instantiation_sites']=[];headers.append(row);active=row
        elif active is not None:
            note=re.match(r'^([^:]+):(\d+):\d+: note: .*requested here',line)
            if note:
                src=PurePosixPath(note[1]).name;ln=int(note[2]);o=owner(src,ln);active['instantiation_sites'].append(dict(file=src,line=ln,owner=o))
expected={r['token'] for r in json.loads((V.parent/'native119-codex-2026-10-22/SOURCE-DECLARATIONS.json').read_text('utf-8'))['rows']}
bulk={r['token'] for r in json.loads((V/'frozen-source/IMPLEMENTATION-PLAN.json').read_text('utf-8'))['targets']}
direct_tokens={t for r in direct for t in r['owner']['tokens']};header_tokens=set();unresolved_headers=[]
for row in headers:
    sites=row['instantiation_sites']
    if not sites or any(not s['owner'] or not s['owner']['tokens'] for s in sites):unresolved_headers.append(row)
    for s in sites:
        if s['owner']:header_tokens.update(s['owner']['tokens'])
report=dict(scope='LINUX_CLANG_DIAGNOSTIC_ATTRIBUTION_ONLY_NOT_TU_OR_BEHAVIOR_QUALIFICATION',candidate_sha256='ef95567ce714146b2f7aed1a92e98798023090bb47757f6c5b377f9ce6d9c0f4',ci_run=39790796339,method_boundary='lexical_brace_balanced_ignoring_comments_and_string_char_literals',mapped_tokens=len(mapped),bulk350_targets_mapped=len(bulk&mapped),direct_error_count=len(direct),direct_error_methods=len(direct_tokens),included_error_count=len(headers),included_error_methods=len(header_tokens),unmapped_direct=unmapped,unresolved_header_instantiations=unresolved_headers,exact_direct_error_roster_matches_native11937=direct_tokens==expected,all_header_error_tokens_in_native11937=header_tokens<=expected,bulk350_direct_error_overlap=sorted(bulk&direct_tokens),bulk350_header_error_overlap=sorted(bulk&header_tokens),error_tokens=sorted(direct_tokens),source_hashes=source_hashes,header_attribution=headers)
(V/'52-INDEPENDENT-CPP-ATTRIBUTION.json').write_text(json.dumps(report,indent=2)+'\n','utf-8')
print({k:v for k,v in report.items() if k not in ['source_hashes','header_attribution','error_tokens']})
sys.exit(not(direct_tokens==expected and not unmapped and not unresolved_headers and header_tokens<=expected and len(bulk&mapped)==350))
