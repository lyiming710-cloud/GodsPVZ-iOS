# Stage9.1 Projectile::.ctor PC-native authority

Target managed method: `Projectile::.ctor()`

- token: `0x060003FD`
- RID: `1021`
- dc39 input DLL SHA256: `dc39b8bcbfcd61f04acf2c18c42e24583e9a6e75248bb24b6aae9e381b19f173`
- damaged managed body: `CodeSize=80`, `locals=3`
- first runtime-invalid instruction: `IL_0025 stloc 1` where local 1 is `System.IntPtr`
- damaged body also contains a direct private `UnityEngine.Vector3::zeroVector` reference at `IL_0036`

Primary authority is the original PC 1.0.2 package from Google Drive (`GodsPVZ_1.0.2.zip`, Drive ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`).

Authority hashes:

- PC archive SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

Assembly-CSharp CodeGenModule method pointer table begins at VA `0x181B82D60`. For RID 1021, the entry is:

`0x181B82D60 + (1021 - 1) * 8 = 0x181B84D40`

The original PC pointer stored there is `0x18037DF20`.

`.pdata` contains an exact runtime-function record:

- begin: `0x18037DF20`
- end: `0x18037DF7F`
- unwind info: `0x181A88EEC`

The native byte slice `[0x18037DF20, 0x18037DF7F)` is 95 bytes and has SHA256:

`e843a1237bfb1e0355a1efe9ce989a2a8b02dbc19c0eedf09fb9db0f00e5b7ff`

Relevant native semantics:

```text
0x18037DF26  mov dword ptr [rcx+0x38], 0x3F800000   ; scale = 1.0f
0x18037DF30  mov dword ptr [rcx+0x40], 0x3F800000   ; updateRate = 1.0f
...
0x18037DF53  load UnityEngine.Vector3 static fields
0x18037DF63  load first 8 bytes of zero vector
0x18037DF67  load final 4 bytes of zero vector
0x18037DF6D  store first 8 bytes at this+0x60
0x18037DF72  store final 4 bytes at this+0x68         ; previousPosition = Vector3.zero
...
0x18037DF7A  tail jump to UnityEngine.MonoBehaviour::.ctor
```

Recovery rule: rebuild only this MethodDef as `scale=1f; updateRate=1f; previousPosition=Vector3.zero; base..ctor();`. Use an existing `UnityEngine.Vector3::get_zero()` MemberRef from the input module instead of the inaccessible private `zeroVector` field. Do not change any field metadata or any other MethodDef.
