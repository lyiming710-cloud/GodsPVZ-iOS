# Stage9.1 FIRST FORMAL UNITY IMPORT — current status

Date: 2026-09-12
Branch: `high-fidelity`

## Current state

- `UNITY_LICENSE_GATE = PASS`
- `FIRST_IMPORT_PASS = NO`
- `FORMAL_R2_IMPORT_ATTEMPTED = NO`
- current classification: `INFRASTRUCTURE_GATE_R2_RUNNER_ACCESS`
- HF56 authorized: **NO**

The prior Unity-license blocker is resolved. A formal GitHub Actions FIRST IMPORT workflow now exists, but its anonymous R2 preflight stopped before reconstruction because R2 part `00` is still not anonymously downloadable. No Unity project import, package resolution, compile, or serialized-asset import was reached.

Do not change HF55, packages, serialized assets, or gameplay code to address this infrastructure gate.

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

## FIRST IMPORT workflow and R2 access preflight

Workflow: `.github/workflows/stage9-first-formal-import.yml`

- workflow commit: `db92b2436758946fe6fb741dfeff3dd141bec868`
- workflow run: `34675644811`
- job: `103504915120`
- R2 download mode: anonymous Google Drive file-ID download via `gdown`
- first requested file: R2 part `00`, Drive ID `1Z8XaczeHLaActdzRFDo98l8pQvRm7hpx`
- result: `DOWNLOAD_FAIL`, gdown exit `1`
- GitHub runner diagnostic: `Cannot retrieve the public link of the file. You may need to change the permission to 'Anyone with the link'`
- R2 reconstruction attempted: NO
- formal pre-import manifest audit attempted: NO
- exact Editor reconstruction in this run: NO (safely skipped)
- Personal seat acquired in this run: NO (safely skipped)
- FIRST FORMAL UNITY IMPORT attempted: NO
- classification: `NOT_ATTEMPTED` due R2 access infrastructure gate

GitHub preflight evidence:

- artifact name: `Stage9.1-FIRST-FORMAL-UNITY-IMPORT-evidence`
- artifact ID: `10291773126`
- artifact digest: `sha256:40e975d5aa8fd69a9e0c97e7d2d315bd145a4ac9b5ee3a9f959a52109054bd5a`

Drive preservation:

- folder: `Stage9.1-PreImport-R2-2026-09-12/FirstImport-R2-Access-Preflight-2026-09-12`
- folder ID: `1dn759bbkscTBDcrpWac03s4TLD-Wb10U`
- evidence file ID: `1uaTCXbw6Ojc4ga3HjGXdCcGg-UAC4qNT`

Connector metadata immediately before the GitHub preflight showed `04.part` as shared but `00.part` and the other formal R2 parts as not shared. The GitHub runner independently confirmed that `00.part` is not available anonymously, so this is not merely a connector metadata lag.

## Next valid action

Make all 12 formal R2 `*.part` files anonymously readable (`Anyone with the link` / Viewer), preferably by selecting all 12 part files together inside the R2 folder and changing their General access in one operation. Then re-run workflow `34675644811` / `.github/workflows/stage9-first-formal-import.yml`.

The workflow is already prepared to perform in one job, only after 12/12 R2 parts download and hash-verify:

1. reconstruct R2 and verify archive size/SHA256 plus `zstd -t`;
2. verify the formal 19782-file pre-import manifest and HF55 SHA256;
3. reconstruct and SHA-verify exact Unity China `2022.3.44f1c1`;
4. acquire a Personal seat using the proven account-based path;
5. run FIRST FORMAL UNITY IMPORT against the verified R2 project;
6. classify compile/package/serialization/license diagnostics;
7. perform a post-import differential audit;
8. preserve evidence and return the Personal seat.

Until FIRST IMPORT is actually reached, do not classify any result as a Unity project import failure and do not open HF56.
