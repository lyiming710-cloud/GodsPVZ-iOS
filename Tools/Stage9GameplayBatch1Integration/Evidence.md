# Stage9 gameplay batch 1 integration

Base: `1c62f969409ed16c9d7c7855f7b52e6b593701524e2b2d75ec0affbc3d05b209`.

This integration combines only repairs that already passed independent native-backed gates from the same base:

- `PrepareUIController.GameStart` token `0x0600067E`: replace the erroneous direct read of private `BoardManager.<Instance>k__BackingField` with the original singleton getter call `BoardManager.get_Instance()`. Independent gate run: `34775585332`; branch candidate SHA: `5dbeb70608ea5880f79140932133f2793f01787ec99059a545ba04013d8fdd06`.
- `Map.Copy` token `0x06000285`, `Map.GetMapX` token `0x06000289`, `Map.GetMapY` token `0x0600028A`: PC-native immediate gameplay-path recovery. Independent gate run: `34775850942`; branch candidate SHA: `6fb26689d20362531c5ccabbe89a8b90f8b80af9381b4581e95263b2692e0305`.

The integrated candidate is constructed from the validated Map branch and adds the already independently validated GameStart semantic correction. Final isolation is measured against the original `1c62` base and must contain exactly four changed MethodDefs and zero FieldDef changes. No unrelated static-scan findings are integrated here.
