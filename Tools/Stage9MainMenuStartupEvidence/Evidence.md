# Stage9.1 MainMenu / save-init original-PC evidence

Development-only recovery evidence. Formal cumulative recovery remains HF55. The current `c8d0fdf2...b425` DLL remains a Stage9 integration/diagnostic candidate, not a formal HF release.

## Locked primary authority

Validated locally from the fixed original PC package:

- PC ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Mapping tool: `Tools/MethodIndexDump/native_map.py`, using original metadata MethodDef RIDs and the original `Assembly-CSharp.dll` `Il2CppCodeGenModule` method-pointer table.

The original metadata maps 2316 native MethodDefs. This is the original-PC mapping domain; it does not change the recovered managed-assembly invariant that the Stage9 candidate keeps 2317 MethodDefs.

## Exact original-PC method pointers

- `GameStart::Start()` token `0x06000526` -> `0x1803756F0`
- `ResourceManager::Start()` token `0x06000216` -> `0x18033BF30`
- `ResourceManager::LoadAudioClips()` token `0x0600021B` -> `0x180330340`
- `SavesManager::.ctor()` token `0x06000250` -> `0x180340560`
- `SavesManager::Awake()` token `0x06000236` -> `0x18033E090`
- `SavesManager::Start()` token `0x06000237` -> `0x1803404E0`
- `SavesManager::LoadPlayerSaves()` token `0x06000247` -> `0x18033FB10`
- `SavesManager::TryCreateNewPlayerSave()` token `0x0600024F` -> `0x1803404F0`
- `SavesManager::LoadFirstSave()` token `0x06000244` -> `0x18033F650`
- `SavesManager::CreateNewPlayerSave(string)` token `0x0600023A` -> `0x18033E4B0`

## Startup causality proved by PC native

`GameStart::Start()` calls the startup components in this order:

1. construct `SavesManager` at `0x180375897`;
2. call `SavesManager::Awake()` at `0x1803758AA`;
3. call `ResourceManager::Start()` at `0x1803758EB`;
4. only after that returns, call `SavesManager::Start()` at `0x1803758FE`.

`SavesManager::Start()` is a tail jump to `SavesManager::LoadPlayerSaves()`.

Therefore an exception thrown inside `ResourceManager::Start()` prevents the original startup path from reaching `SavesManager::Start()/LoadPlayerSaves()` at all. The Stage9 runtime marker `saves=1 player=0` is compatible with this: the `SavesManager` object can already exist while save loading has not executed.

This proves that the current malformed generic call reached from `ResourceManager::Start()` is upstream of save initialization. It is not correct to infer from `playerSave == null` alone that `LoadPlayerSaves()` ran and failed.

## Original no-save semantics

`SavesManager::LoadPlayerSaves()` first tries the player-save path. On the no-file/failure path the PC native code:

- constructs and stores a new `SaveList`;
- opens the create-save window;
- returns without assigning `playerSave`.

The relevant native sequence is inside `0x18033FB10`; the new `SaveList` is stored around `0x18033FC1F-0x18033FC43`, and the create-save popup is invoked at `0x18033FCA3`.

`SavesManager::TryCreateNewPlayerSave()` (`0x1803404F0`) likewise contains no write to the `SavesManager` instance. It resolves the create-save UI and tail-jumps to `Window_I::PopupNewWindow(...)` at `0x18034054B`.

Actual save creation occurs in `SavesManager::CreateNewPlayerSave(string)` (`0x18033E4B0`):

- constructs `Save` at `0x18033E501`;
- stores it into the `SavesManager` instance at `0x18033E50A`;
- calls `InitializeSave` at `0x18033E520`;
- writes the supplied player name;
- calls `LoadBaseData` at `0x18033E547`;
- appends the new save to the save list;
- updates the default-player name;
- saves to disk at `0x18033E5EC`;
- refreshes `MainUIController` data.

So a clean original startup is allowed to have `playerSave == null` while the create-save UI is awaiting user input. A startup-path test must not require `player=1` immediately after passive MainMenu warmup in a clean persistent-data environment.

## Consequence for the Stage9 gate

The next gate should distinguish two phases:

1. **MainMenu startup integrity:** recover `ResourceManager` and other startup methods so the original startup sequence reaches save loading without managed exceptions.
2. **Save-selection/creation integrity:** if no existing save is present, drive the original create-save UI/callback path until `CreateNewPlayerSave(string)` establishes the save. Do not inject `playerSave`, `saveList`, `gLawnApp`, or other state directly.

Only after the original path establishes a non-null save should the gate enter `Board` and interpret the downstream Board/SeedChooser/PrepareUI failures.

## Current managed generic evidence linked to this causality

The exact `c8d0...b425` whole-assembly audit found 199 suspicious generic callsites across 45 distinct malformed MemberRefs and 54 caller methods. The startup-relevant examples include:

- `ResourceManager::Start()` -> malformed `List<Zombie_charred>.Add(Zombie_charred)` (`0x0A000173`);
- `ResourceManager::LoadAudioClips()` -> malformed `List<AudioClip>.Add(AudioClip)` (`0x0A000178`, 11 callsites);
- `MainUIController::PlayBGM(int)` -> malformed `List<AudioClip>.get_Item(int)` (`0x0A0001EF`).

These signatures are the same concrete-substitution corruption class already proven by earlier Stage9 generic repairs. They should be normalized only through a bounded, auditable generic-MemberRef repair using known-good `!0/!1` signatures; unrelated MemberRefs must not be modified.
