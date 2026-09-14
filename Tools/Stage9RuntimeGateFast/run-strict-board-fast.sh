#!/usr/bin/env bash
set -euo pipefail
OUT="$1"; SRC="$2"; EXPECTED_SHA="$3"; E="$OUT/evidence"
mkdir -p "$OUT/r3" "$E"
[ -n "${UNITY_EMAIL:-}" ] && [ -n "${UNITY_PASSWORD:-}" ] || exit 61

R3_ARCHIVE="$OUT/r3.tar.zst"
R3_CACHE="$OUT/r3-cache/GodsPVZ-Stage9.1-preimport-r3.tar.zst"
[ -f "$R3_CACHE" ] || { echo 'FAST_R3_CACHE_MISSING'; exit 62; }
echo "R3_SOURCE exact-actions-cache"
cp "$R3_CACHE" "$R3_ARCHIVE"
[ "$(stat -c%s "$R3_ARCHIVE")" = 786765618 ] || exit 63
[ "$(sha256sum "$R3_ARCHIVE"|awk '{print $1}')" = 'd4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc' ] || exit 64
zstd -dc "$R3_ARCHIVE" | tar -xf - -C "$OUT/r3"
rm -f "$R3_ARCHIVE"
PROJECT="$OUT/r3/UnityProject-AssetRipper-2.0.0/ExportedProject"
OLD="$PROJECT/Assets/Plugins/Assembly-CSharp.dll"
[ "$(sha256sum "$OLD"|awk '{print $1}')" = 'dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655' ] || exit 65
[ "$(sha256sum "$SRC"|awk '{print $1}')" = "$EXPECTED_SHA" ] || exit 71
OLDMETA="$OLD.meta"; NEW="$PROJECT/Assets/Plugins/GodsPVZRuntime1.dll"; NEWMETA="$NEW.meta"
sed -i 's/^  isPreloaded: 0$/  isPreloaded: 1/' "$OLDMETA"
cp "$SRC" "$NEW"; mv "$OLDMETA" "$NEWMETA"; rm "$OLD"
grep -q '^guid: 60b84a767c46ced9aacc93f38da4d2f3$' "$NEWMETA"
grep -q '^  isPreloaded: 1$' "$NEWMETA"
printf 'candidate_sha256=%s\n' "$EXPECTED_SHA" > "$E/input-gate.txt"

T="$PROJECT/Assets/Stage9StrictBoardRuntime"; mkdir -p "$T"
cp Tools/Stage9RuntimeGate/StrictBoardRuntime.cs "$T/StrictBoardRuntime.cs"
cat > "$T/Stage9.StrictBoard.Runtime.asmdef" <<'EOF'
{"name":"Stage9.StrictBoard.Runtime","references":[],"overrideReferences":false,"autoReferenced":true,"optionalUnityReferences":["TestAssemblies"]}
EOF

P="$OUT/editor-parts"; D="$OUT/editor"; mkdir -p "$D"
[ "$(find "$P" -maxdepth 1 -type f -name 'Unity-China-2022.3.44f1c1.tar.xz.part-*'|wc -l)" = 15 ] || exit 81
[ "$(cat "$P"/Unity-China-2022.3.44f1c1.tar.xz.part-*|sha256sum|awk '{print $1}')" = '0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14' ] || exit 82
cat "$P"/Unity-China-2022.3.44f1c1.tar.xz.part-* | xz -dc | tar -xf - -C "$D"
rm -rf "$P"
U="$D/Editor/Unity"; chmod +x "$U"
"$U" -version > "$E/unity-version.txt" 2>&1
grep -q '2022.3.44f1c1' "$E/unity-version.txt"
C="$D/Editor/Data/Resources/Licensing/Client/Unity.Licensing.Client"
SEAT=0
cleanup(){ if [ "$SEAT" = 1 ]; then set +e; "$C" --deactivate-all > "$E/license-deactivate.log" 2>&1; echo $? > "$E/license-deactivate-exit.txt"; set -e; fi; }
trap cleanup EXIT
set +e
"$C" --activate-all --include-personal --username "$UNITY_EMAIL" --password "$UNITY_PASSWORD" > "$E/license-activate.log" 2>&1
RC=$?
set -e
echo "$RC" > "$E/activation-exit.txt"; [ "$RC" = 0 ] || exit 84; SEAT=1

# One Unity project launch: import candidate + compile strict test + execute PlayMode gate.
set +e
timeout 360 "$U" -batchmode -nographics -projectPath "$PROJECT" -runTests -testPlatform PlayMode -assemblyNames Stage9.StrictBoard.Runtime -testResults "$E/results.xml" -logFile "$E/playmode.log"
RC=$?
set -e
echo "$RC" > "$E/playmode-exit.txt"
! grep -qE '\berror CS[0-9]+\b|Scripts have compiler errors|Compilation failed' "$E/playmode.log" || exit 91
grep -E 'STAGE9_STRICT_' "$E/playmode.log" > "$E/markers.txt" || true
grep -E 'Exception|InvalidProgramException|MissingMethodException|FieldAccessException' "$E/playmode.log" > "$E/exceptions.txt" || true
grep -E 'MissingMethodException.*System\.Collections\.Generic\.(List|Dictionary)' "$E/playmode.log" > "$E/generic-missing-methods.txt" || true
grep -n -E 'BoardStart|BoardManager|BoardConfig|Map:\.ctor|Map::.ctor|Row:\.ctor|Row::.ctor|SeedChooserScreen|PrepareUIController|GameStart|LoadBoard|Exception|InvalidProgramException|FieldAccessException|MissingMethodException' "$E/playmode.log" > "$E/board-context.txt" || true
cat "$E/markers.txt" || true
[ "$RC" = 0 ] || exit "$RC"
[ -s "$E/results.xml" ] || exit 101
grep -q 'STAGE9_STRICT_CREATE_INVOKE ok=1' "$E/markers.txt" || exit 103
grep -q 'STAGE9_STRICT_SAVE_READY player=1 playerName=Stage9Test saveList=1 defaultName=Stage9Test' "$E/markers.txt" || exit 104
grep -Eq 'STAGE9_STRICT_BOARD_PRESTART scene=Board .*boardStart=0 board=0 .*boardManager=1 prepare=1 seedChooser=1' "$E/markers.txt" || exit 105
grep -q 'STAGE9_STRICT_GAMESTART_INVOKE ok=1' "$E/markers.txt" || exit 106
grep -Eq 'STAGE9_STRICT_BOARD scene=Board .*boardStart=0 .*board=[1-9][0-9]* .*activeBoard=1' "$E/markers.txt" || exit 107
echo 'FAST_RUNTIME_GATE_PASS unity_project_launches=1 lifecycle_assertions=preserved'
