"""Actual Windows WARP execution of original PC DXBC versus recovered HLSL.

Scope: shader mathematics and raster output in controlled D3D11 fixtures. Unity
binding, GLES/Metal precision, renderer selection and game pixels remain separate
gates. Original DXBC is executed directly, never reconstructed from disassembly.
"""
from pathlib import Path
import argparse
import ctypes as C
import hashlib
import json
import math
import random
import re
import struct
import traceback

W = Path(__file__).resolve().parent
parser=argparse.ArgumentParser()
parser.add_argument('--sources',type=Path,default=W.parents[1]/'audit/repo-native113/Recovery/Custom-Shaders-2026-10-24/Shaders')
parser.add_argument('--original',type=Path,default=W/'custom-shader-evidence/original-dxbc')
parser.add_argument('--output',type=Path,default=W/'shader-r1-validation')
settings=parser.parse_args()
S,O,OUT=settings.sources.resolve(),settings.original.resolve(),settings.output.resolve()
OUT.mkdir(exist_ok=True)
P, U, I, F = C.c_void_p, C.c_uint, C.c_int, C.c_float
HR = C.c_long

def method(obj, index, result, types, values):
    table = C.cast(obj, C.POINTER(C.POINTER(P))).contents
    return C.WINFUNCTYPE(result, P, *types)(table[index])(obj, *values)

def check(hr, label):
    if hr < 0:
        raise RuntimeError(label + ': ' + hex(hr & 0xffffffff))

def blob_bytes(blob):
    pointer = method(blob, 3, P, [], [])
    size = method(blob, 4, C.c_size_t, [], [])
    return C.string_at(pointer, size)

compiler = C.WinDLL('d3dcompiler_47.dll')
compiler.D3DCompile.argtypes = [P, C.c_size_t, C.c_char_p, P, P, C.c_char_p, C.c_char_p, U, U, C.POINTER(P), C.POINTER(P)]
compiler.D3DCompile.restype = HR

def compile_shader(source, entry, profile, label):
    raw = source.encode()
    code, error = P(), P()
    hr = compiler.D3DCompile(raw, len(raw), label.encode(), None, None, entry.encode(), profile.encode(), 1 << 15, 0, C.byref(code), C.byref(error))
    try:
        if error:
            (OUT / (label + '.compiler.txt')).write_bytes(blob_bytes(error).rstrip(b'\0') + b'\n')
        check(hr, 'D3DCompile ' + label)
        result = blob_bytes(code)
        (OUT / (label + '.hlsl')).write_bytes(raw)
        (OUT / (label + '.dxbc')).write_bytes(result)
        return result
    finally:
        for obj in (code, error):
            if obj:
                method(obj, 2, U, [], [])

class BufferDesc(C.Structure):
    _fields_ = [(x,U) for x in ('ByteWidth','Usage','BindFlags','CPUAccessFlags','MiscFlags','StructureByteStride')]
class SampleDesc(C.Structure):
    _fields_ = [('Count',U),('Quality',U)]
class TextureDesc(C.Structure):
    _fields_ = [(x,U) for x in ('Width','Height','MipLevels','ArraySize','Format')] + [('SampleDesc',SampleDesc)] + [(x,U) for x in ('Usage','BindFlags','CPUAccessFlags','MiscFlags')]
class Data(C.Structure):
    _fields_ = [('pSysMem',P),('SysMemPitch',U),('SysMemSlicePitch',U)]
class Mapped(C.Structure):
    _fields_ = [('pData',P),('RowPitch',U),('DepthPitch',U)]
class InputElement(C.Structure):
    _fields_ = [('SemanticName',C.c_char_p)] + [(x,U) for x in ('SemanticIndex','Format','InputSlot','AlignedByteOffset','InputSlotClass','InstanceDataStepRate')]
class Viewport(C.Structure):
    _fields_ = [(x,F) for x in ('TopLeftX','TopLeftY','Width','Height','MinDepth','MaxDepth')]
class SamplerDesc(C.Structure):
    _fields_ = [(x,U) for x in ('Filter','AddressU','AddressV','AddressW')] + [('MipLODBias',F),('MaxAnisotropy',U),('ComparisonFunc',U),('BorderColor',F*4),('MinLOD',F),('MaxLOD',F)]
class RasterDesc(C.Structure):
    _fields_ = [('FillMode',U),('CullMode',U),('FrontCounterClockwise',I),('DepthBias',I),('DepthBiasClamp',F),('SlopeScaledDepthBias',F)] + [(x,I) for x in ('DepthClipEnable','ScissorEnable','MultisampleEnable','AntialiasedLineEnable')]
class RTBlend(C.Structure):
    _fields_ = [('BlendEnable',I)] + [(x,U) for x in ('SrcBlend','DestBlend','BlendOp','SrcBlendAlpha','DestBlendAlpha','BlendOpAlpha')] + [('RenderTargetWriteMask',C.c_ubyte)]
class BlendDesc(C.Structure):
    _fields_ = [('AlphaToCoverageEnable',I),('IndependentBlendEnable',I),('RenderTarget',RTBlend*8)]

class Renderer:
    width = height = 32
    def __init__(self):
        self.objects = []
        self.device, self.context, self.level = P(), P(), U()
        api = C.WinDLL('d3d11.dll')
        api.D3D11CreateDevice.argtypes = [P,U,P,U,P,U,U,C.POINTER(P),C.POINTER(U),C.POINTER(P)]
        api.D3D11CreateDevice.restype = HR
        check(api.D3D11CreateDevice(None,5,None,0,None,0,7,C.byref(self.device),C.byref(self.level),C.byref(self.context)), 'Create WARP device')
        self.target = self.texture(self.width,self.height,None,0,0x20)
        self.staging = self.texture(self.width,self.height,None,3,0,0x20000)
        self.rtv = self.create(9,[P,P,C.POINTER(P)],[self.target,None])
        desc = SamplerDesc(0,3,3,3,0,1,8,(F*4)(0,0,0,0),0,3.402823466e38)
        self.sampler = self.create(23,[P,C.POINTER(P)],[C.byref(desc)])
        desc = RasterDesc(3,1,0,0,0,0,1,0,0,0)
        raster = self.create(22,[P,C.POINTER(P)],[C.byref(desc)])
        self.ctx(43,[P],[raster])
        viewport = Viewport(0,0,self.width,self.height,0,1)
        self.ctx(44,[U,P],[1,C.byref(viewport)])
        self.ctx(24,[U],[4])  # TRIANGLELIST
        self.bind_array(33,[self.rtv],suffix_types=[P],suffix=[None],start=False)
        self.blends = {}
        for name, alpha in [('off',None),('game',5),('brightness',2)]:
            desc = BlendDesc()
            rt = desc.RenderTarget[0]
            rt.RenderTargetWriteMask = 15
            rt.BlendEnable = alpha is not None
            rt.SrcBlend,rt.DestBlend,rt.BlendOp = 5,6,1
            rt.SrcBlendAlpha,rt.DestBlendAlpha,rt.BlendOpAlpha = alpha or 2,6,1
            self.blends[name] = self.create(20,[P,C.POINTER(P)],[C.byref(desc)])
        self.resources = []

    def ctx(self, slot, types, values):
        return method(self.context, slot, None, types, values)
    def create(self, slot, types, values):
        out = P()
        check(method(self.device,slot,HR,types,values+[C.byref(out)]),'device method '+str(slot))
        self.objects.append(out)
        return out
    def bind_array(self, slot, objects, suffix_types=None, suffix=None, start=True):
        values = (P*len(objects))(*[o.value for o in objects])
        types, args = ([U,U,P],[0,len(objects),values]) if start else ([U,P],[len(objects),values])
        self.ctx(slot,types+(suffix_types or []),args+(suffix or []))
    def buffer(self, raw, flags):
        storage = C.create_string_buffer(raw)
        desc = BufferDesc(len(raw),0,flags,0,0,0)
        initial = Data(C.cast(storage,P),0,0)
        return self.create(3,[P,P,C.POINTER(P)],[C.byref(desc),C.byref(initial)])
    def texture(self, width, height, raw, usage=0, bind=8, cpu=0):
        desc = TextureDesc(width,height,1,1,2,SampleDesc(1,0),usage,bind,cpu,0)
        storage = C.create_string_buffer(raw) if raw else None
        initial = Data(C.cast(storage,P),width*16,0) if raw else None
        return self.create(5,[P,P,C.POINTER(P)],[C.byref(desc),C.byref(initial) if initial else None])
    def texview(self, pixels, width=4, height=4):
        tex = self.texture(width,height,struct.pack('<'+'f'*len(pixels),*pixels))
        return self.create(7,[P,P,C.POINTER(P)],[tex,None])
    def program(self, vs, ps, brightness=False):
        vs_raw,ps_raw=C.create_string_buffer(vs),C.create_string_buffer(ps)
        vertex = self.create(12,[P,C.c_size_t,P,C.POINTER(P)],[vs_raw,len(vs),None])
        pixel = self.create(15,[P,C.c_size_t,P,C.POINTER(P)],[ps_raw,len(ps),None])
        specs = [(b'POSITION',2,0),(b'NORMAL',6,16),(b'TANGENT',2,28),(b'TEXCOORD',2,44),(b'COLOR',2,60)] if brightness else [(b'POSITION',2,0),(b'TEXCOORD',16,16),(b'COLOR',2,24)]
        elements=(InputElement*len(specs))(*[InputElement(n,0,fmt,0,off,0,0) for n,fmt,off in specs])
        layout=self.create(11,[P,U,P,C.c_size_t,C.POINTER(P)],[elements,len(elements),vs_raw,len(vs)])
        return vertex,pixel,layout
    def draw(self, program, vertices, vs_constants, ps_constants, textures, blend='off', bright=False):
        vertex,pixel,layout=program
        self.ctx(11,[P,P,U],[vertex,None,0])
        self.ctx(9,[P,P,U],[pixel,None,0])
        self.ctx(17,[P],[layout])
        self.ctx(35,[P,P,U],[self.blends[blend],None,0xffffffff])
        raw=struct.pack('<'+'f'*len(vertices),*vertices)
        vb=self.buffer(raw,1)
        stride,offset=U(76 if bright else 40),U(0)
        buffers=(P*1)(vb.value)
        self.ctx(18,[U,U,P,P,P],[0,1,buffers,C.byref(stride),C.byref(offset)])
        for slot,constants in [(7,vs_constants),(16,ps_constants)]:
            bs=[self.buffer(struct.pack('<'+'f'*len(v),*v),4) for v in constants]
            self.bind_array(slot,bs)
        self.bind_array(8,textures)
        self.bind_array(10,[self.sampler]*len(textures))
        clear=(F*4)(-0.125,0.125,0.25,0.375)
        self.ctx(50,[P,P],[self.rtv,clear])
        self.ctx(13,[U,U],[6,0])
        self.ctx(47,[P,P],[self.staging,self.target])
        mapped=Mapped()
        check(method(self.context,14,HR,[P,U,U,U,P],[self.staging,0,1,0,C.byref(mapped)]),'Map staging')
        try:
            raw=b''.join(C.string_at(mapped.pData+y*mapped.RowPitch,self.width*16) for y in range(self.height))
        finally:
            self.ctx(15,[P,U],[self.staging,0])
        return struct.unpack('<'+'f'*(self.width*self.height*4),raw)
    def close(self):
        self.ctx(110,[],[])  # ClearState
        for obj in reversed(self.objects):
            method(obj,2,U,[],[])
        method(self.context,2,U,[],[])
        method(self.device,2,U,[],[])

def matrix_identity():
    return [1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1]

def game_source(shared):
    return shared + '''
cbuffer Material : register(b0) { float4 material[6]; };
cbuffer Object : register(b1) { float4 object[4]; };
cbuffer Camera : register(b2) { float4 camera[21]; };
Texture2D mainTex : register(t0); SamplerState mainSampler : register(s0);
struct A { float4 position : POSITION; float2 uv : TEXCOORD0; float4 color : COLOR; };
struct V { float2 uv : TEXCOORD0; float4 position : SV_POSITION; float4 color : COLOR; };
V VS(A input) {
  V output;
  float2 xy=RecoveredGameUnitXY(input.position.xy,material[3].x,material[3].y,material[3].z,material[3].w);
  float4 world=object[0]*xy.x+object[1]*xy.y+object[3];
  output.position=camera[17]*world.x+camera[18]*world.y+camera[19]*world.z+camera[20]*world.w;
  output.uv=input.uv*material[2].xy+material[2].zw;
  output.color=input.color*material[5];
  return output;
}
float4 PS(V input) : SV_Target {
  return RecoveredGameUnitColor(mainTex.Sample(mainSampler,input.uv),input.color,material[4].w,material[4].y,material[4].z);
}
'''

def brightness_source(shared, enabled, normal=False, forward=False):
    defines='\n'.join('#define USE_SHAPE_LIGHT_TYPE_'+str(i)+' '+str(int(i in enabled)) for i in range(4))+'\n'
    # Uniform packing is taken from the original DXBC registers. Fixtures adapt
    # these controlled bindings; they do not claim to exercise Unity's binding.
    header='''
cbuffer Globals : register(b0) { float4 globals[160]; };
cbuffer Material : register(b1) { float4 material[4]; };
Texture2D mainTex : register(t0); SamplerState mainSampler : register(s0);
struct A { float4 position : POSITION; float3 normal : NORMAL; float4 tangent : TANGENT; float4 uv : TEXCOORD0; float4 color : COLOR; };
struct V { float4 position : SV_POSITION; float4 uv : INTERP0; float4 color : INTERP1; float4 screen : INTERP2; float3 world : INTERP3; };
V VS(A input) {
  V output;
  float3 world=material[0].xyz*input.position.x+material[1].xyz*input.position.y+material[2].xyz*input.position.z+material[3].xyz;
  output.position=globals[76]*world.x+globals[77]*world.y+globals[78]*world.z+globals[79];
  output.uv=input.uv; output.color=input.color*globals[129]; output.world=world;
  float4 p=output.position;
  output.screen=float4((float2(p.x,p.y*globals[21].x)+p.w)*0.5,p.zw);
  return output;
}
'''
    if normal:
        header+='''
struct N { float4 position : SV_POSITION; float4 tangent : INTERP0; float4 uv : INTERP1; float3 viewNormal : INTERP2; };
N NVS(A input) {
  N output;
  float3 world=material[0].xyz*input.position.x+material[1].xyz*input.position.y+material[2].xyz*input.position.z+material[3].xyz;
  output.position=globals[76]*world.x+globals[77]*world.y+globals[78]*world.z+globals[79];
  float3 tangent=material[0].xyz*input.tangent.x+material[1].xyz*input.tangent.y+material[2].xyz*input.tangent.z;
  output.tangent=float4(tangent*rsqrt(max(dot(tangent,tangent),1.17549435e-38)),input.tangent.w);
  output.uv=input.uv; output.viewNormal=float3(globals[64].z,globals[65].z,globals[66].z);
  return output;
}
float4 PS(N input) : SV_Target {
  return RecoveredBrightnessNormal(mainTex.SampleBias(mainSampler,input.uv.xy,globals[5].x),input.viewNormal,material[1].y);
}
'''
    else:
        for j,i in enumerate(enabled):
            header+=f'Texture2D light{i} : register(t{j+1}); SamplerState lightSampler{i} : register(s{j+1});\n'
        header+='''
float4 PS(V input) : SV_Target {
  float4 color=RecoveredBrightnessColor(mainTex.SampleBias(mainSampler,input.uv.xy,globals[5].x),input.color,material[1].x,material[1].y);
'''
        if forward:
            header+='return color;\n}\n'
        else:
            header+='half3 modulate=0, additive=0;\n'
            for j,i in enumerate(enabled):
                k=128+3*j
                header+=f'half3 l{i}=RecoveredBrightnessLight(light{i}.SampleBias(lightSampler{i},input.screen.xy/input.screen.w,globals[5].x).rgb,globals[{k+1}],globals[{k+2}]);\n'
                header+=f'modulate+=l{i}*globals[{k}].x; additive+=l{i}*globals[{k}].y;\n'
            k=128+3*len(enabled)
            header+=f'return RecoveredBrightnessLit(color,modulate,additive,globals[{k}].x,globals[{k}].y);\n}}\n'
    # Unity packs _RendererColor after variant-specific global light uniforms.
    # Readback of each original vertex DXBC confirms 129+3*N, or 128 forward.
    renderer_slot = 128 if forward else 129 + 3*len(enabled)
    header = header.replace('input.color*globals[129]', f'input.color*globals[{renderer_slot}]')
    return defines+shared+header

def actual_game_source(shared, mutation=None):
    """Compile the actual shader entry bodies with controlled original bindings."""
    shader=(S/'Unlit_GameUnitShader.shader').read_text()
    code=shader.split('CGPROGRAM',1)[1].split('ENDCG',1)[0]
    code=re.sub(r'^\s*#pragma[^\n]*', '', code, flags=re.M)
    code=code.replace('#include "UnityCG.cginc"','').replace('#include "RecoveredGameUnit.hlsl"',shared)
    for declaration in ('sampler2D _MainTex;','float4 _MainTex_ST;','float _AngleX, _AngleY, _ScaleX, _ScaleY;',
                        'float _Alpha, _IsVisible, _Exposure;','float4 _Color;'):
        assert code.count(declaration)==1
        code=code.replace(declaration,'')
    bindings='''
cbuffer Material : register(b0) { float4 material[6]; };
cbuffer Object : register(b1) { column_major float4x4 unity_ObjectToWorld; };
cbuffer Camera : register(b2) { float4 cameraPadding[17]; column_major float4x4 recoveredVP; };
#define UNITY_MATRIX_VP recoveredVP
#define _MainTex_ST material[2]
#define _AngleX material[3].x
#define _AngleY material[3].y
#define _ScaleX material[3].z
#define _ScaleY material[3].w
#define _Alpha material[4].y
#define _IsVisible material[4].z
#define _Exposure material[4].w
#define _Color material[5]
Texture2D _MainTex : register(t0); SamplerState sampler_MainTex : register(s0);
#define tex2D(textureName, uv) textureName.Sample(sampler_MainTex, uv)
'''
    if mutation=='uses-input-z':
        assert code.count('float4(xy, 0.0, 1.0)')==1
        code=code.replace('float4(xy, 0.0, 1.0)','float4(xy, input.position.z, 1.0)')
    return bindings+code

def actual_brightness_source(shared, enabled, normal=False, forward=False, mutation=None):
    shader=(S/'Shader Graphs_Brightness.shader').read_text()
    common=shader.split('HLSLINCLUDE',1)[1].split('ENDHLSL',1)[0]
    passes=re.findall(r'HLSLPROGRAM(.*?)ENDHLSL',shader,re.S)
    assert len(passes)==3
    body=passes[1 if normal else (2 if forward else 0)]
    code=common+body
    code=re.sub(r'^\s*#pragma[^\n]*', '', code, flags=re.M)
    code=re.sub(r'^\s*#include "Packages/[^\n]*', '', code, flags=re.M)
    code=code.replace('#include "RecoveredBrightness.hlsl"',shared)
    for declaration in ('float4 _MainTex_TexelSize;','float _Brightness;','float _Transparent;',
                        'float4 _RendererColor;','half _HDREmulationScale;','half _UseSceneLighting;'):
        code=code.replace(declaration,'')
    globals_slot=128 if forward else 129+3*len(enabled)
    hdr_slot=128+3*len(enabled)
    bindings='\n'.join('#define USE_SHAPE_LIGHT_TYPE_'+str(i)+' '+str(int(i in enabled)) for i in range(4))+'\n'
    bindings+='''
cbuffer Globals : register(b0) { float4 globals[160]; };
cbuffer Material : register(b1) { float4 material[4]; };
#define CBUFFER_START(name)
#define CBUFFER_END
#define TEXTURE2D(name) Texture2D name
#define SAMPLER(name) SamplerState name
#define SAMPLE_TEXTURE2D(tex, samp, uv) tex.SampleBias(samp, uv, globals[5].x)
#define SHAPE_LIGHT(index)
#define _Brightness material[1].x
#define _Transparent material[1].y
#define _ProjectionParams globals[21]
#define unity_ObjectToWorld transpose(float4x4(material[0],material[1],material[2],material[3]))
#define UNITY_MATRIX_V transpose(float4x4(globals[64],globals[65],globals[66],globals[67]))
float4 TransformObjectToHClip(float3 p) {
  float3 world=mul(unity_ObjectToWorld,float4(p,1)).xyz;
  return globals[76]*world.x+globals[77]*world.y+globals[78]*world.z+globals[79];
}
'''
    bindings+=f'#define _RendererColor globals[{globals_slot}]\n#define _HDREmulationScale globals[{hdr_slot}].x\n#define _UseSceneLighting globals[{hdr_slot}].y\n'
    # Texture/sampler declarations in the actual source use implicit registers.
    # Set the identical packed register locations explicitly for this adapter.
    code=code.replace('TEXTURE2D(_MainTex);','Texture2D _MainTex : register(t0);').replace('SAMPLER(sampler_MainTex);','SamplerState sampler_MainTex : register(s0);')
    for j,i in enumerate(enabled):
        k=128+3*j
        bindings+=f'Texture2D _ShapeLightTexture{i} : register(t{j+1}); SamplerState sampler_ShapeLightTexture{i} : register(s{j+1});\n'
        bindings+=f'#define _ShapeLightBlendFactors{i} globals[{k}].xy\n#define _ShapeLightMaskFilter{i} globals[{k+1}]\n#define _ShapeLightInvertedFilter{i} globals[{k+2}]\n'
    if mutation=='forward-clamps-negative':
        assert forward
        code=code.replace('return RecoveredBrightnessColor(SampleMain(input.uv.xy), input.color, _Brightness, _Transparent);',
                          'return max(0.0, RecoveredBrightnessColor(SampleMain(input.uv.xy), input.color, _Brightness, _Transparent));')
    return bindings+code

def vertices(bright=False, color=(1,1,1,1), z=0, w=1, shift=0):
    result=[]
    for x,y,u,v in [(-.8,-.8,0,1),(-.8,.8,0,0),(.8,.8,1,0),(-.8,-.8,0,1),(.8,.8,1,0),(.8,-.8,1,1)]:
        if bright:
            result.extend([x,y,z,w,0,0,-1,1,0,0,1,u,v,0,0,*color])
        else:
            result.extend([x+shift,y,z,w,u,v,*color])
    return result

def compare(a,b):
    bad=[i for i,(x,y) in enumerate(zip(a,b)) if not math.isfinite(x) or not math.isfinite(y) or abs(x-y)>2e-5+2e-5*max(abs(x),abs(y))]
    clear=(-.125,.125,.25,.375)
    return dict(pass_=not bad,failed_components=len(bad),max_abs_error=max(abs(x-y) for x,y in zip(a,b)),
                original_nonclear_components=sum(abs(x-clear[i%4])>1e-6 for i,x in enumerate(a)),
                original_sha256=hashlib.sha256(struct.pack('<'+'f'*len(a),*a)).hexdigest(),
                recovered_sha256=hashlib.sha256(struct.pack('<'+'f'*len(b),*b)).hexdigest(),
                first_differences=[dict(component=i,original=a[i],recovered=b[i]) for i in bad[:4]])

def main():
    report=dict(scope='ACTUAL_ORIGINAL_PC_DXBC_AND_RECOVERED_SHADER_ENTRIES_WARP_RASTER',
                renderer='D3D_DRIVER_TYPE_WARP',unity_binding=False,metal=False,gameplay=False,
                size=[32,32],tolerance='2e-5 + 2e-5*max(abs(a),abs(b)) per float32 component',
                adapters='Actual .shader vertex and fragment entry bodies plus exact shared HLSL. Original register layouts and controlled Unity matrix/sampling macro adapters. Unity binding, shader import and pass selection need independent Editor gate.',
                cases=[],negative_controls=[],pass_=False)
    renderer=None
    try:
        renderer=Renderer()
        report['feature_level']=hex(renderer.level.value)
        rng=random.Random(0x600d2026)
        pixels=[v for y in range(4) for x in range(4) for v in ((x+1)/5,(y+1)/5,(x+y+1)/9,(x+2*y)%5/4)]
        maintex=renderer.texview(pixels)
        lights=[renderer.texview([v for k in range(16) for v in (.15+i*.13,.3+(k%4)*.1,.5+i*.1,1)]) for i in range(4)]
        shared=(S/'RecoveredGameUnit.hlsl').read_text()
        source=actual_game_source(shared)
        gvs=compile_shader(source,'vert','vs_4_0','game-vs')
        gps=compile_shader(source,'frag','ps_4_0','game-ps')
        original=renderer.program((O/'Unlit_GameUnitShader-program1.dxbc').read_bytes(),(O/'Unlit_GameUnitShader-program3.dxbc').read_bytes())
        recovered=renderer.program(gvs,gps)
        contexts=[]
        for case in range(20):
            params=[0.0]*24
            params[8:12]=[1,1,0,0] if case<4 else [.8,1.1,.1,-.05]
            params[12:16]=[0,0,1,1] if case<4 else [rng.uniform(-.6,.6),rng.uniform(-.6,.6),rng.uniform(.5,1.1),rng.uniform(.5,1.1)]
            params[16:20]=[0, .65 if case%2 else 1, -1 if case in (1,5,9) else (-.5 if case==6 else 0), 1.3 if case>3 else 1]
            params[20:24]=[.7,1.2,.8,.6] if case>2 else [1,1,1,1]
            obj=matrix_identity()
            obj[12:15]=[.03,-.05,.25]
            camera=[0.0]*84;camera[68:84]=matrix_identity()
            if case>10:
                camera[68],camera[73]=.9,1.05
            v=vertices(color=(.8,1,.65,.75) if case>3 else (1,1,1,1),z=3 if case==2 else 0,w=2 if case==3 else 1)
            context=(v,[params,obj,camera],[params],[maintex])
            contexts.append(context)
            for blend in ('off','game'):
                a=renderer.draw(original,*context,blend=blend)
                b=renderer.draw(recovered,*context,blend=blend)
                result=compare(a,b);result.update(name=f'game-{case:02d}-{blend}')
                report['cases'].append(result)
        # Each physical mutant uses the same cases and original executable.
        mutants={
            'hidden-returns-zero-rgb':shared.replace('return float4(texel.rgb, 0.0);','return float4(0.0, 0.0, 0.0, 0.0);'),
            'exposure-omitted':shared.replace('float4(exposure, exposure, exposure, alpha)','float4(1.0, 1.0, 1.0, alpha)'),
            'wrong-angle-sign':shared.replace('sin(-angleY)','sin(angleY)').replace('cos(-angleY)','cos(angleY)'),
        }
        for name,text in mutants.items():
            assert text!=shared
            src=actual_game_source(text)
            prog=renderer.program(compile_shader(src,'vert','vs_4_0',name+'-vs'),compile_shader(src,'frag','ps_4_0',name+'-ps'))
            detected=0
            for context in contexts:
                a=renderer.draw(original,*context)
                b=renderer.draw(prog,*context)
                detected+=not compare(a,b)['pass_']
            valid=all(c['pass_'] for c in report['cases'] if c['name'].startswith('game-'))
            report['negative_controls'].append(dict(name=name,detected_cases=detected,baseline_pass=valid,pass_=detected>0 and valid))
        src=actual_game_source(shared,'uses-input-z')
        prog=renderer.program(compile_shader(src,'vert','vs_4_0','uses-input-z-vs'),compile_shader(src,'frag','ps_4_0','uses-input-z-ps'))
        context=contexts[2]
        detected=not compare(renderer.draw(original,*context),renderer.draw(prog,*context))['pass_']
        report['negative_controls'].append(dict(name='uses-input-z',detected_cases=int(detected),baseline_pass=valid,pass_=detected and valid))
        shared=(S/'RecoveredBrightness.hlsl').read_text()
        bright_contexts=[]
        for variant in range(18):
            normal=variant==16;forward=variant==17
            enabled=[i for i in range(4) if variant<16 and variant & (1<<(3-i))]
            source=actual_brightness_source(shared,enabled,normal,forward)
            vsentry='NormalVertex' if normal else 'SpriteVertex'
            psentry='NormalFragment' if normal else ('ForwardFragment' if forward else 'LitFragment')
            vs=compile_shader(source,vsentry,'vs_4_0',f'brightness-{variant:02d}-vs')
            ps=compile_shader(source,psentry,'ps_4_0',f'brightness-{variant:02d}-ps')
            ov,op=(54,56) if normal else ((58,60) if forward else (5+variant,37+variant))
            original=renderer.program((O/f'Shader_Graphs_Brightness-program{ov}.dxbc').read_bytes(),(O/f'Shader_Graphs_Brightness-program{op}.dxbc').read_bytes(),True)
            recovered=renderer.program(vs,ps,True)
            for case in range(8):
                globals_=[0.0]*640
                globals_[84]=1 if case%2==0 else -1
                globals_[304:320]=matrix_identity()
                globals_[258],globals_[262],globals_[266]=.2,-.4,.8
                renderer_slot=128 if forward else 129+3*len(enabled)
                globals_[4*renderer_slot:4*renderer_slot+4]=[.8,1,.7,.65]
                vs_constants=[globals_,matrix_identity()]
                # VS and PS original globals have different binding layouts.
                ps_globals=[0.0]*640
                for j,i in enumerate(enabled):
                    k=(128+3*j)*4
                    ps_globals[k:k+4]=[.8-j*.1,.2+j*.13,0,0]
                    ps_globals[k+4:k+8]=[0,0,0,0] if case%3==0 else [.2,.5,-.1,.6]
                    ps_globals[k+8:k+12]=[1,.2,.4,0]
                k=(128+3*len(enabled))*4
                ps_globals[k:k+4]=[1.35,case/7,0,0]
                material=[0,0,0,0, 1+case*.4,0 if case==1 else .75,0,0]+[0]*8
                col=(-.3,.8,.65,.7) if case==2 else ((.9,.7,.8,0) if case==3 else (.9,.7,.8,.6))
                v=vertices(True,col,z=.1*case)
                context=(v,vs_constants,[ps_globals,material],[maintex]+[lights[i] for i in enabled])
                for blend in ('off','brightness'):
                    a=renderer.draw(original,*context,blend=blend,bright=True)
                    b=renderer.draw(recovered,*context,blend=blend,bright=True)
                    result=compare(a,b);result.update(name=f'brightness-{variant:02d}-{case:02d}-{blend}')
                    report['cases'].append(result)
                if variant in (0,15,16,17):
                    bright_contexts.append((variant,enabled,normal,forward,original,context))
        mutants={
            'missing-alpha-discard':shared.replace('if (color.a == 0.0)','if (false)'),
            'normal-uses-vertex-alpha':shared.replace('texel.a * transparent','texel.a * transparent * 0.125'),
            'lighting-mask-omitted':shared.replace('light *= dot(1.0 - inverted, filter);','light *= 1.0;'),
            'brightness-omitted':shared.replace('float4(brightness, brightness, brightness, transparent)','float4(1.0, 1.0, 1.0, transparent)'),
        }
        for name,text in mutants.items():
            assert text!=shared
            detected=0;compiled={}
            for variant,enabled,normal,forward,original,context in bright_contexts:
                if variant not in compiled:
                    src=actual_brightness_source(text,enabled,normal,forward)
                    ve='NormalVertex' if normal else 'SpriteVertex'
                    pe='NormalFragment' if normal else ('ForwardFragment' if forward else 'LitFragment')
                    compiled[variant]=renderer.program(compile_shader(src,ve,'vs_4_0',name+f'-{variant}-vs'),compile_shader(src,pe,'ps_4_0',name+f'-{variant}-ps'),True)
                a=renderer.draw(original,*context,bright=True)
                b=renderer.draw(compiled[variant],*context,bright=True)
                detected+=not compare(a,b)['pass_']
            valid=all(c['pass_'] for c in report['cases'] if c['name'].startswith('brightness-'))
            report['negative_controls'].append(dict(name=name,detected_cases=detected,baseline_pass=valid,pass_=detected>0 and valid))
        variant,enabled,normal,forward,original,context=next(c for c in bright_contexts if c[0]==17 and c[-1][0][-4]<0)
        src=actual_brightness_source(shared,enabled,normal,forward,'forward-clamps-negative')
        prog=renderer.program(compile_shader(src,'SpriteVertex','vs_4_0','forward-clamps-vs'),compile_shader(src,'ForwardFragment','ps_4_0','forward-clamps-ps'),True)
        detected=not compare(renderer.draw(original,*context,bright=True),renderer.draw(prog,*context,bright=True))['pass_']
        report['negative_controls'].append(dict(name='forward-clamps-negative',detected_cases=int(detected),baseline_pass=valid,pass_=detected and valid))
        report['pass_']=all(c['pass_'] for c in report['cases']+report['negative_controls'])
        report['case_count']=len(report['cases'])
        report['failed_cases']=[c for c in report['cases'] if not c['pass_']]
        report['source_sha256']={p.name:hashlib.sha256(p.read_bytes()).hexdigest() for p in S.iterdir() if p.suffix in ('.hlsl','.shader')}
        report['nonempty_cases']=sum(c['original_nonclear_components']>0 for c in report['cases'])
        assert report['nonempty_cases']>=200
        report['original_manifest_sha256']=hashlib.sha256((O/'MANIFEST.json').read_bytes()).hexdigest()
    except Exception as exc:
        report['exception']=repr(exc)
        report['traceback']=traceback.format_exc()
    finally:
        if renderer:
            renderer.close()
        (OUT/'D3D11-ORACLE-RESULT.json').write_bytes((json.dumps(report,indent=2)+'\n').encode())
    print(json.dumps({k:v for k,v in report.items() if k not in ('cases','failed_cases')},indent=2))
    raise SystemExit(0 if report['pass_'] else 1)

if __name__=='__main__':
    main()
