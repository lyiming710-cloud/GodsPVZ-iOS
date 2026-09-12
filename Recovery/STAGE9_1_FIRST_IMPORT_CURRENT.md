# Stage9.1 FIRST FORMAL UNITY IMPORT — current status

Date: 2026-09-12
Branch: `high-fidelity`

## Current state

- `UNITY_LICENSE_GATE = PASS`
- `FIRST_IMPORT_PASS = NO`
- `FORMAL_R2_IMPORT_ATTEMPTED = NO`
- current classification: `INFRASTRUCTURE_GATE_R2_RUNNER_ACCESS`
- HF56 authorized: **NO**

The prior Unity-license blocker is resolved. The remaining prerequisite is to provide the same GitHub Actions runner with a private, integrity-verifiable path to the locked R2 input. Do not change HF55, packages, serialized assets, or gameplay code to address this infrastructure gate.

## Exact Editor gate

Formal Editor source: preserved GitHub Actions artifacts from run `34668863583`.

- Unity variant: China Linux Editor
- version: `2022.3.44f1c1`
- archive size: `3906640940`
- archive SHA256: `0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14`
- 15/15 preserved raw parts: SHA256 PASS
- reconstructed Editor version gate: PASS
- bundled Licensing Client supports `--include-personal`: PASS

## Personal license gate — PASS

Workflow: `.github/workflows/stage9-unity-license-gate-preserved-v3.yml`

- workflow run: `34675146453`
- job: `103503560660`
- workflow commit: `9606e1846190cf18fc048adb58bc015f9e34e53c`
- result: SUCCESS
- activation method: `Unity.Licensing.Client --activate-all --include-personal`
- account activation exit: `0`
- Personal seat assignment/update: PASS
- exact Editor licensed probe exit: `0`
- `Successfully resolved entitlement details`: observed repeatedly
- fatal `No valid Unity Editor license found` / `License is not active`: absent
- seat return is armed on step exit
- R2 touched: NO
- formal recovered-project import attempted: NO

The earlier Windows-Hub `.ulf` remains machine-bound and is not the final CI activation mechanism. The valid CI path is account-based Personal activation using `UNITY_EMAIL` and `UNITY_PASSWORD` stored only as GitHub repository secrets. `UNITY_LICENSE` may remain configured but is not required by the v3 account-based gate.

## License evidence

GitHub artifact:

- name: `Stage9.1-exact-editor-personal-license-gate-v3-evidence`
- artifact ID: `10292505723`
- artifact digest / locally reverified ZIP SHA256: `ef4ecd16e9141c19a8f30674447cfb9720727c3467a22be30b538b3f8da41846`
- retention: 90 days

Drive preservation:

- folder: `Stage9.1-PreImport-R2-2026-09-12/LicenseGate-PASS-2026-09-12`
- folder ID: `14Qxj9-Oo_H4so_fe98aw1CTP7_SNhxVX`
- evidence file ID: `1F8VI5uCE9J6R83GJWVy1QNTSO0Oh8RDN`

No raw password, email credential, access token, refresh token, or license XML is included in the uploaded evidence.

## Locked R2 input

- archive size: `786481679`
- archive SHA256: `99bc1ed7a713b627919fee8c3f63bbed5eb949ede72b32a7a5866a067d1b2a0e`
- project manifest: `19782 / 19782`
- HF55 SHA256: `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`
- project editor version: `2022.3.44f1c1`
- Drive folder ID: `1XE6CdcBq7P-__OV7ZfdD23FdYumYjIvG`
- R2 consists of 12 Drive parts; parts 00–10 are 67108864 bytes and part 11 is 48284175 bytes.

The R2 Drive folder is currently private (`shared=false`), so a GitHub-hosted runner cannot download it without an explicit transfer/authentication path.

## Next valid action

Provide the formal GitHub runner a private or explicitly authorized read path to the locked R2 parts, then perform in **one job**:

1. reconstruct and SHA-verify exact Unity China `2022.3.44f1c1`;
2. acquire Personal seat using the proven account-based path;
3. retrieve all 12 formal R2 parts;
4. verify every part, reconstructed archive size/SHA256, `zstd -t`, and the 19782-file manifest;
5. create a disposable work copy;
6. run FIRST FORMAL UNITY IMPORT on that unchanged copy;
7. preserve complete sanitized Editor/import evidence and post-import differential audit;
8. return the Personal seat.

Until step 6 is actually reached, do not classify any result as a Unity project import failure and do not open HF56.
