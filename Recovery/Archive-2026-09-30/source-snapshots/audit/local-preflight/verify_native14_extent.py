from pathlib import Path
import sys, hashlib
root = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(root/'stage9-native4/python-deps'))
import pefile
from capstone import Cs, CS_ARCH_X86, CS_MODE_64
src = root/'stage9-native4/inputs/GameAssembly.dll'
assert hashlib.sha256(src.read_bytes()).hexdigest() == '9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
pe = pefile.PE(str(src)); base = pe.OPTIONAL_HEADER.ImageBase
out = root/'codespace-native14-review'; out.mkdir(exist_ok=True)
for va,end,want in [(0x180439680,0x180439AB8,'191078e9a5434bd3f7d89c411ac132388ebef9799f154acf5889f9541291227e'),(0x1804390C0,0x18043967F,'64a719753c31e448656803a777561aeaf34e1197475ff6a4c5ce9b4c66627cb6')]:
    entry = next(e for e in pe.DIRECTORY_ENTRY_EXCEPTION if e.struct.BeginAddress == va-base)
    assert entry.struct.EndAddress == end-base
    data = pe.get_data(va-base,end-va)
    assert hashlib.sha256(data).hexdigest() == want
    ins = list(Cs(CS_ARCH_X86,CS_MODE_64).disasm(data,va))
    assert sum(i.size for i in ins)==len(data)
    (out/f'{va:x}.asm').write_text('\n'.join(f'{i.address:x} {i.mnemonic} {i.op_str}' for i in ins))
    print(hex(va),len(data),want,'EXTENT_HASH_DECODE_PASS')
