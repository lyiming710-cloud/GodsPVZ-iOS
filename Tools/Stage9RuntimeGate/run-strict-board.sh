#!/usr/bin/env bash
set -euo pipefail
OUT="$1"; SRC="$2"; EXPECTED_SHA="$3"; E="$OUT/evidence"
mkdir -p "$OUT/parts" "$OUT/r3" "$E"
[ -n "${UNITY_EMAIL:-}" ] && [ -n "${UNITY_PASSWORD:-}" ] || exit 61

R3_ARCHIVE="$OUT/r3.tar.zst"
R3_CACHE="$OUT/r3-cache/GodsPVZ-Stage9.1-preimport-r3.tar.zst"
if [ -f "$R3_CACHE" ]; then
  echo "R3_SOURCE exact-actions-cache"
  cp "$R3_CACHE" "$R3_ARCHIVE"
else
  echo "R3_SOURCE google-drive-parts"
  python3 -m pip install --disable-pip-version-check -q gdown==5.2.0
  cat > "$OUT/parts.tsv" <<'EOF'
00	111hFaHbkXIkSyRoAEdushS_qZ5sPFqUI	e024b6d534a6eace43694a506ce8636c56ed17e8c21bde3bba61caab3d3bdf7a
01	1nYIm3Bk9yxckfbwVlk6ijSUcaid6aorm	1c10d0d87be6b65c3dfd0424e38515b53a8dd1158c5734b4397f2072e5004bb2
02	1Y9pMN1FvEpXTFzoKWAx3frra-1-O4s-y	4943615fc153ff8539ab44818dde6981a16758a2c53184a858867d4932edd9c2
03	1I3yfH8qb4nrAvEErO-KaZLX4Ds77x_5C	f8573c2ef3db3d65de3cb3b98f0892e633cdf89d499bde2d509b36f88306331f
04	1QUYoGuNGP83hTo6_hoeKlosAonRJCJ-n	3dc6a03ebb4a7bbd48f652b0c468911d6985f76f48c6ded991d40d1c9b7593bc
05	1Dp8BsTeRmJ9K2yni834KMrxN_Zp18Pe2	13284c2cd74583ccdd63cf6b736dcca443acbf6bda4c25d4b65193fc08c09263
06	1pfqVCotdu3GMYp8Vl1Bbxp7ls8SPGLsW	d35ba0e632125749f581ad57cf950d8c6b264f9f4882a08adb9ca227f6b6e23c
07	1fgUDh-crdy3oNjYAwGr50kdAY7BTwF2J	0c9443797e24ddca500a54a21955e27fd2cb995ec7924147e7f5c244859c07ec
08	1mhL4lliMszPy7ITjfAlSkKQEh19UjhAu	30c9a5330cdece142938150311e8acb87d00d48ab057aa39a2128a3fffdcece8
09	12sojmPtnZPiWmnyfW-c5MhQjvR2DZMJg	46bca3df6cbfc11f155038470a39b705bb028fa7a0e9f9eb3ba4810c06d5058a
10	1P-vcBUiPmSH3gR3l_zkgjA63J8UnsVpG	3c8e312e65a72ac3072233f558b4d43970b3a4e521139d03328e9c27ec41d994
11	1EGPUP5nBjKqvIMft0TAQXvgqpxSVpsyI	dc3d7aa732a19f237e5e72aa6f1e768c10bd360bc6fc832d29b9afc7489da325
EOF
  while IFS=$'\t' read -r idx fid sha; do
    [ "${#sha}" = 64 ] || { echo "R3_PART invalid_expected_sha idx=$idx len=${#sha}"; exit 60; }
    f="$OUT/parts/r3.$idx.part"
    ok=0
    for attempt in 1 2 3; do
      rm -f "$f"
      set +e
      python3 -m gdown --id "$fid" --output "$f" --quiet
      rc=$?
      set -e
      if [ "$rc" = 0 ] && [ -f "$f" ]; then
        got="$(sha256sum "$f" | awk '{print $1}')"
        size="$(stat -c%s "$f")"
        echo "R3_PART idx=$idx attempt=$attempt size=$size sha=$got"
        if [ "$got" = "$sha" ]; then ok=1; break; fi
      else
        echo "R3_PART idx=$idx attempt=$attempt download_rc=$rc"
      fi
      sleep $((attempt * 5))
    done
    [ "$ok" = 1 ] || exit 62
  done < "$OUT/parts.tsv"
  cat "$OUT/parts"/*.part > "$R3_ARCHIVE"
fi

[ "$(stat -c%s "$R3_ARCHIVE")" = 786765618 ] || exit 63
[ "$(sha256sum "$R3_ARCHIVE"|awk '{print $1}')" = 'd4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc' ] || exit 64
zstd -dc "$R3_ARCHIVE" | tar -xf - -C "$OUT/r3"
rm -rf "$OUT/parts" "$R3_ARCHIVE"
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

"$U" -batchmode -nographics -quit -projectPath "$PROJECT" -logFile "$E/first-import.log"
! grep -qE '\berror CS[0-9]+\b|Scripts have compiler errors|Compilation failed' "$E/first-import.log"
T="$PROJECT/Assets/Stage9StrictBoardRuntime"; mkdir -p "$T"
cp Tools/Stage9RuntimeGate/StrictBoardRuntime.cs "$T/StrictBoardRuntime.cs"
cat > "$T/Stage9.StrictBoard.Runtime.asmdef" <<'EOF'
{"name":"Stage9.StrictBoard.Runtime","references":[],"overrideReferences":false,"autoReferenced":true,"optionalUnityReferences":["TestAssemblies"]}
EOF
"$U" -batchmode -nographics -quit -projectPath "$PROJECT" -logFile "$E/test-compile.log"
! grep -qE '\berror CS[0-9]+\b|Scripts have compiler errors|Compilation failed' "$E/test-compile.log"

set +e
timeout 360 "$U" -batchmode -nographics -projectPath "$PROJECT" -runTests -testPlatform PlayMode -assemblyNames Stage9.StrictBoard.Runtime -testResults "$E/results.xml" -logFile "$E/playmode.log"
RC=$?
set -e
echo "$RC" > "$E/playmode-exit.txt"
grep -E 'STAGE9_STRICT_' "$E/playmode.log" > "$E/markers.txt" || true
grep -E 'Exception|InvalidProgramException|MissingMethodException|FieldAccessException' "$E/playmode.log" > "$E/exceptions.txt" || true
grep -E 'MissingMethodException.*System\.Collections\.Generic\.(List|Dictionary)' "$E/playmode.log" > "$E/generic-missing-methods.txt" || true
grep -n -E 'BoardStart|BoardManager|BoardConfig|Map:\.ctor|Map::.ctor|Row:\.ctor|Row::.ctor|SeedChooserScreen|PrepareUIController|GameStart|LoadBoard|Exception|InvalidProgramException|FieldAccessException|MissingMethodException' "$E/playmode.log" > "$E/board-context.txt" || true
cat "$E/markers.txt" || true
[ "$RC" = 0 ] || exit "$RC"
grep -q 'STAGE9_STRICT_CREATE_INVOKE ok=1' "$E/markers.txt" || exit 103
grep -q 'STAGE9_STRICT_SAVE_READY player=1 playerName=Stage9Test saveList=1 defaultName=Stage9Test' "$E/markers.txt" || exit 104
grep -Eq 'STAGE9_STRICT_BOARD_PRESTART scene=Board .*boardStart=0 board=0 .*boardManager=1 prepare=1 seedChooser=1' "$E/markers.txt" || exit 105
grep -q 'STAGE9_STRICT_GAMESTART_INVOKE ok=1' "$E/markers.txt" || exit 106
grep -Eq 'STAGE9_STRICT_BOARD scene=Board .*boardStart=0 .*board=[1-9][0-9]* .*activeBoard=1' "$E/markers.txt" || exit 107
