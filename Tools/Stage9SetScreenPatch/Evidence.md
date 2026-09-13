# GlobalStaticVars.SetScreen(bool) PC native authority

Authority inputs:
- GodsPVZ 1.0.2 PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

Assembly-CSharp mapping:
- `GlobalStaticVars::SetScreen(bool)` token `0x06000149`
- native VA `0x18031C080`, next method at `0x18031C2B0`

Native behavior recovered from that range:
- If `gLawnApp != null`, assign `gLawnApp.fullscreen = fullscreen`.
- `fullscreen == false`: first call `Screen.SetResolution(1440, 810, (FullScreenMode)3)`; reload/copy `resolutionScale`; return if the reloaded `gLawnApp.fullscreen` is true; then map scale 0/1/2/3/4 to 1280x720 / 1920x1080 / 2160x1080 / 2560x1440 / 2960x1440, default 1280x720, and call the 3-argument `Screen.SetResolution(..., mode 3)`.
- `fullscreen == true`: read `Screen.currentResolution`, call native `0x180D0E340`, then call the 4-argument `Screen.SetResolution(2560, 1440, (FullScreenMode)1, refreshRate)`.

The previously unresolved call `0x180D0E340` was independently mapped through the `UnityEngine.CoreModule.dll` IL2CPP codegen module:
- `UnityEngine.Resolution::get_refreshRateRatio()`
- token `0x0600034F`
- native VA `0x180D0E340`

The reconstruction therefore replaces the whole corrupted managed MethodDef rather than changing only the runtime-failing `V11` local.
