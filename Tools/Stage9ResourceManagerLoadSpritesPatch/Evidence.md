# Stage9 ResourceManager.LoadSprites PC-native recovery evidence

Target: `ResourceManager.LoadSprites()` token `0x0600021C`, PC x86-64 VA `0x0000000180333DB0`.

## Runtime causality

Exact R3 strict run `34765343442` with candidate `50015e74e2225c2b3a98c83837195bcc510ea0920bd1a619bcf43c415fe9e641` fails first in the ResourceManager startup chain with:

`InvalidProgramException: Invalid IL code in ResourceManager:LoadSprites (): IL_0113: ceq`

`ResourceManager.Start()` reaches `LoadSprites()` before save-list/window initialization, so this is the next causal blocker. No unrelated invalid method is repaired by this patch.

## PC-native structure

The original PC 1.0.2 function has the same four entry helper calls and the following call-site totals inside `LoadSprites`:

- `InternalResourceLoader.Load<Sprite>`: 88
- `List<Sprite>.Add` / native Add-or-grow paths: 85 total
- `Path.Combine(string,string)`: 92
- `Path.Combine(string,string,string)`: 4

The recovered high-level resource sequence reproduces those totals exactly.

## Resource sequence

- four Card/portrait helper calls;
- Dollar + two card backgrounds;
- 22 Plant sprites in original order;
- 43 Zombie sprites in original order;
- Supplies sprites in `suppliesInitialValue.suppliesInfos` order using `SuppliesInfo.name`;
- 7 VFX sprites;
- Clique icons for exact 1.0.2 enum values `None=0 .. Visitor=5`, retaining `Enum.GetName`, null-slot filling, missing-icon logging, and indexed assignment;
- 3 Device sprites;
- LevelInside lists: A 0..99, R 0..49, HA_A/HA_B/HA_C 0..49, BA 0..49;
- Prop sprites Shovel and Glove.

The original native uses enumerators for Supplies and Clique. The repaired managed body lowers these to deterministic index loops over the exact immutable startup list / exact contiguous 1.0.2 enum values while preserving ordering, names, list indices and observable resource-load behavior. The native call-site totals above remain exact.

## Isolation requirements

The gate must prove:

- exact input SHA `50015e74...`;
- target token/body fingerprint matches the known corrupt method;
- exactly one MethodDef changes (`0x0600021C`), 2316 remain semantically unchanged;
- all 2802 field definitions remain unchanged;
- the prior `ResourceManager.Start(): LoadAudioClips -> LoadSprites` repair remains present;
- all 17 restored `Card_Choose [SerializeField]` attributes remain present;
- reopened method has 88/85/92/4 call-site counts;
- independent fixed ILSpy produces a clean `LoadSprites` body with no `Expected ...` invalid-IL diagnostics.
