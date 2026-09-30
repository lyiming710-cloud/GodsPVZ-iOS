set -e
cd /workspaces/GodsPVZ-native19
W=/workspaces/GodsPVZ-native19/.validation/native22
SC=/workspaces/GodsPVZ-native19/scripts/codespaces
AUD=/workspaces/GodsPVZ-native19/audit/native23

echo "=== git identity ==="
git config user.name || true
git config user.email || true

echo "=== promote batch-2 scripts into the tracked tree ==="
mkdir -p "$AUD"
cp -n "$W"/native23_*.py "$SC"/ 2>/dev/null || true
ls "$SC" | grep native23

echo "=== copy the evidence logs into the tracked tree ==="
cp "$W"/becal.log "$W"/becal2.log "$W"/behave17.log "$W"/behave17b.log \
   "$W"/counter.log "$W"/tiers.log "$AUD"/ 2>/dev/null || true
ls -la "$AUD"

echo "=== write manifest ==="
{
  echo "# Native23 batch 2 -- precise checkpoint"
  echo
  echo "branch: \`$(git rev-parse --abbrev-ref HEAD)\`"
  echo "parent commit: \`$(git rev-parse HEAD)\`"
  echo
  echo "## what this commit contains"
  echo
  echo "* the repair / verification / execution scripts listed below"
  echo "* the six evidence logs produced by them"
  echo "* this manifest"
  echo
  echo "The DLL artefacts themselves live in the git-ignored \`.validation/\`"
  echo "tree; their SHA-256 values are recorded here so the run can be matched."
  echo
  echo "## artefact hashes"
  echo
  echo '```'
  ( cd "$W" && sha256sum work/batch1.dll work/batch2.dll work/batch2a.dll \
      edits-batch2.json edits-batch2a.json \
      out/afam2-candidates.json out/afam3-accepted.json out/afam3-quarantine.json \
      out/behave-sites.json out/tiers.json pcnative/GameAssembly.dll )
  echo '```'
  echo
  echo "## evidence tiers (see audit/native23/tiers.log)"
  echo
  echo "| tier | meaning | methods |"
  echo "|---|---|---|"
  echo "| E1 | typed oracle FAIL->PASS, every flip individually necessary | 377 |"
  echo "| E2 | reference type vouched for by a metadata signature | 249 |"
  echo "| E3 | concrete execution reached every accepted site | 198 |"
  echo "| E4 | PC-native body located and corroborates at method level | 362 |"
  echo "| E5 | null-check site has site-level native evidence | 3 |"
  echo "| E6 | whole-method behaviour proven | 0 |"
  echo
  echo "## the claim that must NOT be over-read"
  echo
  echo "377 is the number of methods TOUCHED. It is not a number of methods"
  echo "PROVEN. Only E6 would be, and E6 = 0."
} > "$AUD/MANIFEST.md"
cat "$AUD/MANIFEST.md"

echo "=== commit ==="
git add -A scripts/codespaces audit/native23
git status --porcelain | head -40
git -c user.name=codex -c user.email=codex@local commit -q \
  -m "$(cat <<'MSG'
feat(native23): batch-2 provenance triage, concrete-execution tests and evidence tiers

Adds the scripts and the evidence logs behind the batch-2 candidate set
(377 methods / 1623 zero-literal sites), and records a checkpoint manifest.

New scripts
  native23_prov.py        multi-error typed oracle with value provenance
  native23_provcheck.py   parity gate: provenance oracle vs multi-error oracle
  native23_afam3.py       re-derives candidates and splits ACCEPT / QUARANTINE
  native23_afam3_check.py self-consistency of the ACCEPT-only subset (C1/C2/C3)
  native23_afam3_edits.py emits edits-batch2a.json (ACCEPT-only)
  native23_behave.py      concrete CIL interpreter: executes damaged vs repaired
  native23_shapes.py      IL-shape census of accepted and quarantined sites
  native23_tiers.py       corrected evidence tiering (E1..E6)
  native23_counter.py     counterexample / regression suite for the rule

Results recorded in audit/native23/MANIFEST.md and the six logs:
  accepted sites 865 (249 methods) / quarantined 758 (268 methods)
  concrete execution reached 747 of 865 sites, every one a reference,
  0 non-reference observations, 0 damaged-vs-repaired trace divergences
  real CLR calibration: at ceq the two forms of the constant are
  indistinguishable, so IL-level runtime tests cannot validate these flips
  E5 (site-level native evidence) = 3 methods; E6 (behaviour proven) = 0
MSG
)"
echo "=== after ==="
git rev-parse HEAD
git log --oneline -3
git status --porcelain | head -20
