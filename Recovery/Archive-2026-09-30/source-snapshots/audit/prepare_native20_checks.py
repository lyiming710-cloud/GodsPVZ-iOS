from pathlib import Path
root=Path(__file__).parent/'native19-work'
p=root/'scripts/codespaces/qualify_native19_cpp.py'
s=p.read_text().replace("base=root/'.validation/native19'","base=root/'.validation/native20'").replace('cpp-final15','cpp-final73').replace('native19-linked','native20-linked')
(p.parent/'qualify_native20_cpp.py').write_bytes(s.encode())
p=root/'.gitignore'
with p.open('a',newline='\n') as f:f.write('\n# Native20 local build products\nscripts/takeover/PatcherNative20/bin/\nscripts/takeover/PatcherNative20/obj/\n')
