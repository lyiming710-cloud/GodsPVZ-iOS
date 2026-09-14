# Board_PlantDetail_PlantData.LoadData PC native evidence

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- MethodDef token: `0x060005E1`
- RID: `1505`
- Assembly-CSharp method-pointer table: `0x181B82D60`
- Pointer entry: `0x181B85C60`
- Native entry: `0x180398B90`
- Next MethodDef (RID 1506) native entry: `0x1803992F0`
- Contiguous native span used for this large method: `0x180398B90..0x1803992F0` (1888 bytes)
- Contiguous span SHA256: `24d71c446fd08dfa73dc6551d29431b1fa4bb62af348e224e4adf60f7165a898`
- First `.pdata` runtime-function fragment: `0x180398B90..0x180398D50` (448 bytes), SHA256 `6cb8dd5f7dc74e7923a72279a21da1dc87448477b392d6408ab40653897ecf77`

The method is large enough that Windows unwind metadata splits it into multiple runtime-function fragments. For semantic recovery, the authoritative boundary is therefore the MethodDef entry through the next MethodDef entry, not only the first `.pdata` fragment.

Recovered semantics from the PC native body:

1. If `detaiPage.plant` is Unity-false, return. Otherwise assign `this.plant = detaiPage.plant`.
2. Set `textLv` to `"Lv." + plant.level.ToString()`. Native uses the `Plant.level` field at object offset `0x8C`; the damaged managed lift misread the native address calculation as `plant + 140`.
3. If `coverHP` is live, set `fillAmount = Max(Min(plant.healthPoint / plant.maxHealthPoint, 1f), 0f)`.
4. If `textHP` is live, display `Truncate(healthPoint) + "/" + Truncate(maxHealthPoint)`.
5. If present, update `textATK`, `textARM`, and `textDEF` from `Plant.GetATK()`, `GetARM()`, and `GetDEF()` with the original Chinese prefixes.
6. If `cliqueLogo` is live, set its sprite to `ResourceManager.cliqueLogos[(int)plant.clique]`.
7. If `plant.skill != null`, set `skillLogo.sprite = ResourceManager.LoadSkillLogo(skill.ID + 3 * plant.ID, skill.name)` and call `LoadSkillData(skill)`.
8. Set `characteristic` from `plant.characteristicText`.
9. For exactly three talent slots, append `：` to each non-empty talent name, write it to `talentNames[i]`, and write `plant.talents[i]` to `talentDescriptions[i]`.
10. For every entry in `stars`, set `stars[i].gameObject` active iff `i < plant.stars`.

CLR reconstruction rule: native IL2CPP may read the internal List size field directly, but restored managed IL must use the public `List<T>.Count` surface rather than a private `List<T>._size` FieldRef. No list mutation occurs in the loop, so this preserves the PC loop semantics without changing metadata visibility.
