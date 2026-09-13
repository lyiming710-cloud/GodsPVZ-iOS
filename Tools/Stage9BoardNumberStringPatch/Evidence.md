# BoardManager.GetBoardNumberString PC-native evidence

Authoritative inputs are the exact original GodsPVZ 1.0.2 PC IL2CPP files:

- `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

The exact metadata/native method map gives:

- MethodDef `0x06000174`
- `BoardManager.GetBoardNumberString(ChallengeType,int)`
- native start `0x000000018030F6F0`
- following managed method `BoardManager.Pass` starts at `0x000000018030FE40`
- the actual GetBoardNumberString machine-code body returns by `0x000000018030F829`; padding begins at `0x000000018030F82A`.

Native control flow proves the following behavior:

- `ChallengeType.Adventure == 0`: compute `page=((level-1)/20)+1`, `stage=((level-1)%20)+1`, return `page + "-" + stage`.
- `ChallengeType.Rescue == 1`: return `"R-" + level`.
- `ChallengeType.HardAdventure == 2`: return `"H-" + level`.
- any other enum value: return `String.Empty`.

The multiplication-by-magic-constant sequence (`0x66666667`) in PC x86-64 native implements signed divide/remainder by 20. The reconstructed managed IL had replaced those operations with `Not implemented instruction: "imul ecx"` / `"imul r8d"`, and also declared V2 as `System.Object` even though native control flow uses the value as an integer enum discriminator. Those are reconstruction defects, not game semantics.

This patch replaces only MethodDef `0x06000174`; the static gate compares all other 2316 MethodDefs semantically before and after writing the candidate.
