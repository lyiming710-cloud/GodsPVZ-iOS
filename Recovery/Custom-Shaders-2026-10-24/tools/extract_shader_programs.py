from pathlib import Path
import sys,zipfile,json,hashlib,struct
work=Path(__file__).resolve().parent;sys.path.insert(0,str(work/'python-deps'))
import UnityPy
from UnityPy.helpers.CompressionHelper import decompress_lz4
from UnityPy.export.ShaderConverter import ShaderSubProgram
from UnityPy.streams import EndianBinaryReader
out=work/'custom-shader-evidence';records=[]
with zipfile.ZipFile(work.parent.parent/'1.0.2Android/GodsPVZ_1.0.2_Android.apk') as z:
    env=UnityPy.load(z.read('assets/bin/Data/data.unity3d'))
    for obj in env.objects:
        if obj.type.name!='Shader':continue
        shader=obj.read();name=shader.m_ParsedForm.m_Name
        if name not in ('Unlit/GameUnitShader','Shader Graphs/Brightness'):continue
        prefix=name.replace('/','_').replace(' ','_')
        print(name,'platforms',shader.platforms,'offsets',shader.offsets,'compressed',shader.compressedLengths,'decompressed',shader.decompressedLengths,flush=True)
        for pi,platform in enumerate(shader.platforms):
            if platform!=9:continue # Other platform data remains in the raw Shader evidence.
            segments=[]
            for offset,compressed,size in zip(shader.offsets[pi],shader.compressedLengths[pi],shader.decompressedLengths[pi]):
                segments.append(decompress_lz4(bytes(shader.compressedBlob)[offset:offset+compressed],size))
            count=struct.unpack_from('<I',segments[0])[0]
            for i in range(count):
                offset,size,segment=struct.unpack_from('<III',segments[0],4+12*i)
                assert segment<len(segments)
                raw=segments[segment][offset:offset+size];assert len(raw)==size
                reader=EndianBinaryReader(raw,endian='<');program=ShaderSubProgram(reader)
                code=program.m_ProgramCode
                header_supported=True
                if platform==9 and not code.startswith((b'#ifdef VERTEX',b'#ifdef FRAGMENT')):
                    # Some ShaderGraph header fields exceed UnityPy's legacy
                    # reader. Recover only an exact length-prefixed GLSL field
                    # wholly inside this original indexed subprogram entry.
                    header_supported=False
                    positions=[raw.find(p) for p in (b'#ifdef VERTEX',b'#ifdef FRAGMENT')]
                    positions=[n for n in positions if n>=4]
                    if not positions:continue # Indexed parameter blocks are not GPU program code.
                    assert len(positions)==1,(name,i,'GLSL field is ambiguous')
                    start=positions[0];length=struct.unpack_from('<I',raw,start-4)[0]
                    assert 100<length<=len(raw)-start
                    code=raw[start:start+length]
                    assert code.rstrip().endswith(b'#endif') and b'void main()' in code and b'\0' not in code
                suffix='.glsl' if platform==9 else '.bin'
                filename=prefix+'-platform'+str(platform)+'-program'+str(i)+suffix
                (out/filename).write_bytes(code)
                records.append(dict(shader=name,platform=platform,index=i,gpu_program_type=int(program.m_ProgramType),
                    legacy_header_supported=header_supported,
                    keywords=program.m_Keywords if header_supported else 'UNSUPPORTED_HEADER_NOT_INTERPRETED',
                    file=filename,bytes=len(code),sha256=hashlib.sha256(code).hexdigest()))
            print(name,platform,'programs',count,flush=True)
(out/'PROGRAM-MANIFEST.json').write_text(json.dumps(records,indent=2)+'\n','utf-8')
