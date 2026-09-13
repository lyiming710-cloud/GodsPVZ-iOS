# Stage9.1 GlobalStaticVars.QualificationTest PC-native recovery evidence

Authority is the original GodsPVZ 1.0.2 PC IL2CPP build, not the reconstructed managed DLL.

- original ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- original `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- original `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- metadata version: 31
- `Assembly-CSharp.dll` original MethodDef count: 2316
- target: `GlobalStaticVars::QualificationTest(string)`
- original RID: 328
- original token: `0x06000148`
- native VA: `0x000000018031BDD0`
- next higher mapped method VA: `0x000000018031BF70`

The original x86-64 body proves this flow:

1. Create the SHA256 instance and retain it for cleanup.
2. Read `Encoding.UTF8` and encode the input key.
3. Compute the hash through the retained SHA256/HashAlgorithm instance.
4. Convert the byte array with `BitConverter.ToString`.
5. Replace `"-"` with `""`.
6. Call `ToLower()`.
7. Compare the normalized string with `Administrator.storedHash` using string equality.
8. Dispose the retained hash object on both normal and exceptional exit through the native finally path.

Key native landmarks inside `0x18031BDD0..0x18031BF70`:

- `0x18031BE20` call creates the retained hash object; result is stored at `[rsp+0x68]`.
- `0x18031BE3F` obtains UTF8 encoding.
- `0x18031BE64` invokes `Encoding.GetBytes(key)`.
- `0x18031BE66` reloads the same retained hash object from `[rsp+0x68]` before the hash call at `0x18031BE7A`.
- `0x18031BE84` calls `BitConverter.ToString`.
- `0x18031BEA6` performs `Replace("-", "")`.
- `0x18031BEB9` performs `ToLower()`.
- `0x18031BEE7..0x18031BEF6` compares against `Administrator.storedHash` and records the boolean result.
- `0x18031BEFD..0x18031BF10` and `0x18031BF17..0x18031BF2A` are the cleanup paths for the retained hash object.

The eec5 managed reconstruction is observably corrupt rather than merely stylistically different:

- `SHA256.Create()` stores into `V5 : SHA256`.
- `ComputeHash` loads unassigned `V6 : HashAlgorithm` instead of the retained object.
- four reference-null tests use illegal reference-vs-int32 `ceq` shapes.
- the cleanup call is replaced by the literal placeholder `"Method not found @180002D90"`.
- runtime therefore fails before the native semantics can execute.

The Stage9 patch replaces only MethodDef `0x06000148` with the directly evidenced managed equivalent, including a `finally` that calls `IDisposable.Dispose`. It does not modify any other MethodDef and does not alter unrelated `GlobalStaticVars` methods.
