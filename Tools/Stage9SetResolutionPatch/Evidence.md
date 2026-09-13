# Stage9.1 GlobalStaticVars.SetResolution PC-native recovery evidence

Authority: original GodsPVZ 1.0.2 PC IL2CPP build.

- original ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- original `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- original `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- target: `GlobalStaticVars::SetResolution(int)`
- original RID: 330
- original token: `0x0600014A`
- native VA: `0x000000018031BF70`
- next higher mapped method VA: `0x000000018031C080`

Native behavior in `0x18031BF70..0x18031C080`:

1. Store the argument into `gLawnApp.resolutionScale`.
2. Reload `gLawnApp`; if `fullscreen` is true, return without changing the OS/window resolution.
3. Map `resolutionScale` to the following dimensions:
   - `0` -> `1280 x 720`
   - `1` -> `1920 x 1080`
   - `2` -> `2160 x 1080`
   - `3` -> `2560 x 1440`
   - `4` -> `2960 x 1440`
   - any other value -> `1280 x 720`
4. Tail-call the Unity screen-resolution method with mode value `3` and the selected width/height.

Key native landmarks:

- `0x18031BFC6`: writes the incoming scale to the LawnApp field at object offset `0x20`.
- `0x18031BFE3`: reads the LawnApp fullscreen byte at offset `0x1c`; nonzero returns at `0x18031C073`.
- `0x18031BFEF..0x18031C05B`: branch table encoded as subtract/compare chains for scale values 0..4 plus fallback.
- width constants: `0x500=1280`, `0x780=1920`, `0x870=2160`, `0xA00=2560`, `0xB90=2960`.
- height constants: `0x2D0=720`, `0x438=1080`, `0x5A0=1440`.
- `0x18031C063`: mode argument `3`.
- `0x18031C06E`: tail jump to the Unity resolution setter.

The cf4c managed reconstruction is observably corrupt:

- MethodDef `0x0600014A`, 18 locals.
- `V7`, `V8`, `V9`, `V12`, `V13`, and `V15` are declared `object` despite being produced by integer subtraction/comparison chains.
- Runtime fails at `IL_006C: stloc 7` when storing `resolutionScale - 1` into the object local.
- Several later branches reuse unrelated boolean locals, so changing only V7 would not reproduce the native branch table.

The patch therefore replaces only MethodDef `0x0600014A` with the directly evidenced equivalent. No other MethodDef is modified.
