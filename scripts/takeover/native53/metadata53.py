from pathlib import Path
import json,struct,collections,subprocess,sys
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native53-2026-10-02'
a=w/'.validation/native52-2026-10-02/fresh/data/Metadata/global-metadata.dat';b=n/'fresh/data/Metadata/global-metadata.dat'
def literals(p):
 image=p.read_bytes();header=struct.unpack_from('<64I',image);off,length,start,size=header[2:6];assert length%8==0
 rows=[]
 for ix in range(length//8):
  count,pos=struct.unpack_from('<II',image,off+8*ix);assert pos+count<=size;rows.append(image[start+pos:start+pos+count].decode('utf8'))
 return rows
old=literals(a);new=literals(b);deleted=list((collections.Counter(old)-collections.Counter(new)).elements())
data=json.loads((n/'parent-all-methods.json').read_text());targets={m['token']for m in json.loads((n/'patch1.json').read_text())['methods']}
target_strings=[i['operand']for m in data['methods']if m['token']in targets for i in m['instructions']if i['opcode']=='ldstr']
assert collections.Counter(deleted)<=collections.Counter(target_strings)
assert all(s.startswith(('Unmanaged memory load:','Warning: Method ends with non empty stack'))for s in deleted),deleted
non_target_strings={i['operand']for m in data['methods']if m['token']not in targets for i in m['instructions']if i['opcode']=='ldstr'}
expected_removed={s for s in target_strings if s not in non_target_strings}
assert collections.Counter(deleted)==collections.Counter(expected_removed),deleted
expected=n/'EXPECTED-REMOVED-LITERALS.json';expected.write_text(json.dumps(deleted,indent=2))
p=subprocess.run([sys.executable,str(w/'scripts/takeover/native40/audit_generated_metadata.py'),str(a),str(b),str(expected)],capture_output=True,text=True);(n/'generated-metadata-audit.log').write_text(p.stdout+p.stderr);assert p.returncode==0,p.stdout+p.stderr
r=json.loads(p.stdout);r['original_target_ldstrs']=target_strings;r['scope']='All 29 non-literal v31 table payloads equal; deleted literal multiset restricted to exact old target diagnostic strings; retained literal order equal. Corrupted nonliteral control rejected.'
(n/'GENERATED-METADATA-AUDIT.json').write_text(json.dumps(r,indent=2));print(json.dumps(r))
