set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native4-ios"
E="$OUT/evidence"
mapfile -t R3_ARCHIVES < <(find "$OUT/r3-cache" -maxdepth 1 -type f -name '*.tar.zst' | sort)
[ "${#R3_ARCHIVES[@]}" = '1' ] || exit 45
ARCHIVE="${R3_ARCHIVES[0]}"
cat > "$OUT/r3-parts.tsv" <<'EOF'
00	e024b6d534a6eace43694a506ce8636c56ed17e8c21bde3bba61caab3d3bdf7a	67108864
01	1c10d0d87be6b65c3dfd0424e38515b53a8dd1158c5734b4397f2072e5004bb2	67108864
02	4943615fc153ff8539ab44818dde6981a16758a2c53184a858867d4932edd9c2	67108864
03	f8573c2ef3db3d65de3cb3b98f0892e633cdf89d499bde2d509b36f88306331f	67108864
04	3dc6a03ebb4a7bbd48f652b0c468911d6985f76f48c6ded991d40d1c9b7593bc	67108864
05	13284c2cd74583ccdd63cf6b736dcca443acbf6bda4c25d4b65193fc08c09263	67108864
06	d35ba0e632125749f581ad57cf950d8c6b264f9f4882a08adb9ca227f6b6e23c	67108864
07	0c9443797e24ddca500a54a21955e27fd2cb995ec7924147e7f5c244859c07ec	67108864
08	30c9a5330cdece142938150311e8acb87d00d48ab057aa39a2128a3fffdcece8	67108864
09	46bca3df6cbfc11f155038470a39b705bb028fa7a0e9f9eb3ba4810c06d5058a	67108864
10	3c8e312e65a72ac3072233f558b4d43970b3a4e521139d03328e9c27ec41d994	67108864
11	dc3d7aa732a19f237e5e72aa6f1e768c10bd360bc6fc832d29b9afc7489da325	48568114
EOF
python3 - "$ARCHIVE" "$OUT/r3-parts.tsv" "$E/r3-download.tsv" <<'PY'
import hashlib,sys
from pathlib import Path
archive=Path(sys.argv[1]); spec=Path(sys.argv[2]); out=Path(sys.argv[3])
rows=[]
for line in spec.read_text().splitlines():
    idx,sha,size=line.strip().split('\t')
    rows.append((idx,sha,int(size)))
with archive.open('rb') as f, out.open('w',encoding='utf-8') as w:
    for idx,expected_sha,expected_size in rows:
        remain=expected_size; h=hashlib.sha256(); got_size=0
        while remain:
            b=f.read(min(1024*1024,remain))
            if not b: raise SystemExit(45)
            h.update(b); n=len(b); remain-=n; got_size+=n
        got_sha=h.hexdigest()
        w.write(f'{idx}\t{got_sha}\t{got_size}\n')
        if got_sha!=expected_sha: raise SystemExit(45)
        if got_size!=expected_size: raise SystemExit(46)
    if f.read(1): raise SystemExit(45)
PY
ROOT="$OUT/r3"
mkdir -p "$ROOT"
GOT="$(sha256sum "$ARCHIVE" | awk '{print $1}')"
echo "$GOT" > "$E/r3-sha256.txt"
[ "$GOT" = "$R3_SHA256" ] || exit 47
zstd -t "$ARCHIVE" 2> "$E/r3-zstd-test.txt"
zstd -dc "$ARCHIVE" | tar -xf - -C "$ROOT"
PROJECT="$ROOT/UnityProject-AssetRipper-2.0.0/ExportedProject"
test -d "$PROJECT"
grep -qx 'm_EditorVersion: 2022.3.44f1c1' "$PROJECT/ProjectSettings/ProjectVersion.txt"
test -f "$PROJECT/Assets/Scenes/MainMenu.unity"
test -f "$PROJECT/Assets/Scenes/Board.unity"
[ "$(sha256sum "$PROJECT/Assets/Plugins/Assembly-CSharp.dll" | awk '{print $1}')" = 'dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655' ] || exit 48
printf '%s\n' "$PROJECT" > "$OUT/project-path.txt"
rm -rf "$OUT/r3-cache" "$ARCHIVE"
df -h "$RUNNER_TEMP" > "$E/disk-after-r3.txt"

