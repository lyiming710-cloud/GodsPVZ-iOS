# HF4 — `Zombie.Awake()` native recovery evidence

## Identity

- Original MethodDef RID: `1051`
- Metadata token: `0x0600041B`
- PC x86-64 native address: `0x18035DAD0`
- Native function end: `0x18035DCF5` (jump tables/data follow)
- HF3 input SHA-256: `23014656af797490bc35d9feb8f78950bcfa79a167c6a1de3303e1d30251c32f`
- HF4 output SHA-256: `2abbc9eb02b93bcd0178091bbc38ca450b6162875df9dcb55e874d5b9af9f0a6`

Primary source is the original PC `GameAssembly.dll`; original metadata is used for MethodDef→native attribution. No gameplay values were inferred from the Cpp2IL body.

## Native field offsets

| Offset | Managed field |
|---:|---|
| `+0x84` | `armor1Type` |
| `+0x88` | `armor1Point` |
| `+0x8C` | `maxArmor1Point` |
| `+0x90` | `armor1Toughness` |
| `+0x94` | `armor1Defense` |
| `+0x9C` | `armor2Type` |
| `+0xA0` | `armor2Point` |
| `+0xA4` | `maxArmor2Point` |
| `+0xA8` | `armor2Toughness` |
| `+0xAC` | `armor2Defense` |

## Enum identities

`Armor1Type`: `none=0`, `cone=1`, `bucket=2`, `brick=3`, `iceCube=4`, `heavyHelmet=5`, `saboteursArmor=6`.

`Armor2Type`: `none=0`, `screendoor=1`, `newspaper=2`, `heavyShield=3`, `ladder=4`.

## Exact native tables

### Armor 1

| Type | Defense | HP / max HP | Toughness |
|---|---:|---:|---:|
| none | 0 | 0 | 0 |
| cone | 60 | 370 | 20 |
| bucket | 120 | 1100 | 35 |
| brick | 40 | 2580 | 15 |
| iceCube | 200 | 1680 | 15 |
| heavyHelmet | 320 | 1680 | 35 |
| saboteursArmor | 150 | 1980 | 15 |
| out of range | 0 | 0 | 0 |

The HP constants are read directly from PC float literals: `370`, `1100`, `2580`, `1680`, `1980`.

### Armor 2

| Type | Defense | HP / max HP | Toughness |
|---|---:|---:|---:|
| none | 0 | 0 | 0 |
| screendoor | 110 | 1100 | 30 |
| newspaper | 20 | 240 | 5 |
| heavyShield | 400 | 2200 | 40 |
| ladder | 120 | 1440 | 30 |
| out of range | 0 | 0 | 0 |

The HP constants are read directly from PC float literals: `1100`, `240`, `2200`, `1440`.

## Native write order

The native function performs no gameplay-level managed method calls. Its observable managed state mutation order is:

1. derive and write `armor1Defense`;
2. derive one Armor1 HP value, write `maxArmor1Point`, then `armor1Point`;
3. derive and write `armor1Toughness`;
4. derive and write `armor2Defense`;
5. derive one Armor2 HP value, write `maxArmor2Point`, then `armor2Point`;
6. derive and write `armor2Toughness`;
7. return.

HF4 preserves that order. Invalid/negative enum values fall through to zero, matching the native unsigned range checks/jump-table default paths.

## Validation

HF4 patcher self-check:

- token remains `0x0600041B`;
- `239` IL instructions, `933` code bytes;
- exactly six switch tables;
- no `call`, `callvirt`, or `newobj` in `Zombie.Awake`;
- all eight armor value fields are written;
- all PC HP constants are present.

Independent Mono.Cecil audit:

- output reopens successfully twice;
- `Zombie.Awake` is explicitly audited (`>=100` IL);
- HF3 `Zombie.InjuryStatusUpdate_Body` remains explicitly audited (`>=200` IL).

Independent ILSpyCmd 11.0.0.9375 readback:

- stderr is empty;
- no `Expected`, `Unknown result`, exception, or decompilation-error markers in `Zombie.Awake`;
- readback is six clean enum switch expressions with the exact PC values above.

Whole-assembly semantic isolation:

- export full IL for final HF3 and candidate HF4;
- normalize method RVA and `<PrivateImplementationDetails>` physical data placement addresses only;
- diff contains exactly one hunk;
- that hunk is entirely `Zombie.Awake`.

## Confidence

**Exact** — field identities, enum cases, constants, write order, and default behavior are all directly supported by PC native x86-64 and independently read back from the emitted managed DLL. No speculative gameplay logic is included.
