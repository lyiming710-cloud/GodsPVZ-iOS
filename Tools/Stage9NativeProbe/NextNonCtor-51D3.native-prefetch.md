# Stage9.1 51D3 non-ctor PC native prefetch

Read-only prefetch only. This record does not select or patch any MethodDef and does not alter the sealed HF55 baseline. Exact-R3 runtime chronology remains authoritative.

Locked source:
- Original GodsPVZ 1.0.2 PC `GameAssembly.dll`
- SHA256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Method-pointer table base `0x181B82D60`
- Extraction self-check: Supplies RID604 -> entry `0x181B84038`, native `0x180341320–0x18034146C`, SHA256 `3688ac9e8b1ca81c33bb128a816e2a5ab536fd6e48126a5e093749dfd964c6f8` PASS.

Managed identities were read from static candidate `51d3a4ac4132fe8933f034287591ee80a6c884877b3ae4f23e799f8217e40b63`.

| Method | MethodDef / RID | managed RVA / size | pointer entry | PC native range | native SHA256 |
|---|---|---|---|---|---|
| `TextLink::Update()` | `0x060006DE` / 1758 | `0x93588` / 235 B | `0x181B86448` | `0x1803ADEB0–0x1803ADF93` (227 B) | `5bf83d68b7d1c545595f58f52527bf0d29dfb7da03f76220fc74e4ed4c48b0a6` |
| `Administrator::Start()` | `0x06000004` / 4 | `0x20B4` / 635 B | `0x181B82D78` | `0x1802FDD20–0x1802FE980` (3168 B) | `3d83a514d37c18b7d91df7ceba17803bb0ac3cbd670dab15e823af4be8b58d02` |
| `Administrator::Update()` | `0x06000006` / 6 | `0x235C` / 492 B | `0x181B82D88` | `0x1802FE980–0x1802FEAEE` (366 B) | `13f85136116347b993ef4733341830e2a3a9f63dca9c8203cbb960114193bd76` |
| `GlobalStaticVars::BGMVolume()` | `0x06000139` / 313 | `0x17788` / 78 B | `0x181B83720` | `0x18031AFC0–0x18031B04A` (138 B) | `2caeb3b868ea5be1e69abc3705506bb260d7cf0bd528a20806aa54514bd037bd` |

Important triage observations:

- `Administrator::Update()` managed IL contains `ldc.i8 4294967295` immediately before `stfld int32 Administrator::mode` at IL `0x00D4–0x00DD`. PC native `Administrator::Update()` contains 32-bit sentinel state logic at `[this+0x60]`, including `cmp dword ptr [this+0x60], -1`, `mov dword ptr [this+0x60], -1`, and the alternate `mov dword ptr [this+0x60], 0`. This strongly identifies the same 64-to-32 corruption family for this specific managed store, but no repair is authorized until runtime selects this MethodDef and the complete target-specific control flow is qualified.
- `Administrator::Start()` is not a simple sentinel-only case. Its damaged managed body contains two `ldc.i8 6442450944` values and is much smaller than the 3168-byte authoritative PC native body. It requires a dedicated native reconstruction/semantic analysis rather than the generic Int32-minus-one patcher.
- `TextLink::Update()` and `GlobalStaticVars::BGMVolume()` likewise require dedicated native comparison before any patch. Their metadata/native identities are now locked so later selection does not require rediscovery.
