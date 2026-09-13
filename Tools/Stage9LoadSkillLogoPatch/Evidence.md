# Stage9.1 ResourceManager.LoadSkillLogo native evidence

Target managed method: `ResourceManager.LoadSkillLogo(int,string)` / MethodDef `0x0600021E`.

Input candidate is exactly `5a1b4e0554fa0163283a477efb606685588012397c39d877ea2179a4de07fcca`.

Primary authority is original PC GodsPVZ 1.0.2 x86-64 IL2CPP:
- GameAssembly SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- metadata SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- native entry `0x1803339E0`, normal body ending before `0x180333DA1`.

Native behavior:
1. Read static `skillLogos` and test its logical count against `id`; negative ids bypass the cache lookup.
2. When `id` is in range, get `skillLogos[id]`; if the Unity Object is non-null, return the cached entry.
3. Otherwise build `Path.Combine("sprites","Skill","SkillLogo",skillName)` and call `InternalResourceLoader.Load<Sprite>`.
4. If the loaded Unity Object is false/null, log `未找到技能<skillName>的图标`.
5. For `id >= 0`, repeatedly append null to `skillLogos` until `Count > id`, then assign `skillLogos[id] = loadedSprite`.
6. Return the loaded sprite.

The original x86-64 body inlines `List<Sprite>.Add` implementation details (`_version`, `_items`, `_size`, growth helper). Those private internals are compiler/runtime implementation, not gameplay-level managed accesses. Cpp2IL incorrectly emitted them as direct managed private-field accesses. It also emitted the final setter as `List<object>.set_Item`, which is a malformed generic MemberRef for the actual `List<Sprite>` receiver.

The managed reconstruction therefore uses only public generic List APIs with CLR-correct generic signatures: `List<Sprite>.get_Count()`, `get_Item(int)->!0`, `Add(!0)`, and `set_Item(int,!0)`. No other MethodDef is changed.
