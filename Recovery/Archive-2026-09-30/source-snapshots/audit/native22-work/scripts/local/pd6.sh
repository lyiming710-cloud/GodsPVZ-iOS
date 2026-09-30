cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import struct
exec(open('native22_native_probe.py').read().split('def read_cstr')[0].replace('def __init__(self, data):','def __init__(self, path=None, data=None):\n        if path: data=open(path,"rb").read()'))
pe=PE(path='pcnative/GameAssembly.dll')
sec=[s for s in pe.sections if s[0]=='.pdata'][0]
name,sva,span,roff,rsize=sec
ents=[]
for o in range(roff, roff+rsize-11, 12):
    b,e,u=struct.unpack_from('<III',pe.data,o)
    ents.append((pe.image_base+b, pe.image_base+e))
ents=sorted(set(ents))
VA=0x18035c4d0
print('=== .pdata entries overlapping [%08X, %08X] ==='% (VA, VA+0x1000))
for a,b in ents:
    if a < VA+0x1000 and b > VA-0x100:
        print('   %08X..%08X size=%d'%(a,b,b-a))
print()
# next entry after VA
nxt=[e for e in ents if e[0]>VA][:3]
print('next .pdata begins:', ['%08X'%a for a,b in nxt])
print()
off=pe.va_to_offset(VA)
sz=0x400
open('/tmp/pu.bin','wb').write(pe.data[off:off+sz])
print('dumped %d bytes to /tmp/pu.bin'%sz)
PY
which objdump || echo "no objdump"
objdump -D -b binary -m i386:x86-64 --adjust-vma=0x18035c4d0 /tmp/pu.bin 2>/dev/null | head -90
