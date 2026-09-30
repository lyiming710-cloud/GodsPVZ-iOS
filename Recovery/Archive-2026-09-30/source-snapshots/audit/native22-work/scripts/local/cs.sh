#!/usr/bin/env bash
# Stable Codespace runner.
#   bash scripts/local/cs.sh <remote-script.sh> [outfile]
set -uo pipefail
export HTTPS_PROXY=http://127.0.0.1:10808 HTTP_PROXY=http://127.0.0.1:10808
export https_proxy=http://127.0.0.1:10808 http_proxy=http://127.0.0.1:10808
GH="/c/Program Files/GitHub CLI/gh.exe"
CS="glowing-train-p7j9gp74q6jwc76v6"
SCRIPT="$1"
OUT="${2:-$SCRIPT.out.txt}"
B64=$(base64 -w0 < "$SCRIPT")
"$GH" codespace ssh -c "$CS" -- "bash -lc 'echo $B64 | base64 -d | bash -s'" > "$OUT" 2>&1
echo "rc=$? -> $OUT ($(wc -c < "$OUT") bytes)"
