# Plant_DetaiPage.Initialize native recovery evidence

Input candidate: `1c62f969409ed16c9d7c7855f7b52e6b593701524e2b2d75ec0affbc3d05b209`.

This method is on the immediate real Board boot path: `BoardManager.LoadBoard -> Instantiate<Board> -> Board.Awake -> detailPage.Initialize(null)`.

Managed broken target:
- token `0x06000667`
- code size `830`
- 25 locals
- malformed Vector3/float/object lowering around skill UI position
- malformed Color/float lowering around `rangeIamge.color`

PC x86-64 IL2CPP authority:
- `Plant_DetaiPage.Initialize(Plant)`: VA `0x1803A3E30`
- next mapped method: `LoadSkillLogo`: VA `0x1803A41D0`
- exact position adjustment constant loaded from PC `.rdata`: `30.0f`
- exact final range color constant loaded from PC `.rdata`: `(1.0f,1.0f,1.0f,1.0f)`

Recovered source semantics:
1. assign `plant`;
2. null plant: hide skill object and clear `p_skill`; non-null plant: set plant name, copy skill, activate skill object based on `isOnField`;
3. for non-null plant, set skill object's position to `(plant.x, plant.y + 30, 0)`;
4. call `LoadSkillLogo()`;
5. configure manual-skill button/auto/key UI and charged-layer visibility;
6. call `dataPage.LoadData(this)` and `dataPage.CheckText(0)`;
7. set `skillRange=false` and `rangeIamge.color=(1,1,1,1)`.

No null-tolerant behavior beyond what native already performs is added.
