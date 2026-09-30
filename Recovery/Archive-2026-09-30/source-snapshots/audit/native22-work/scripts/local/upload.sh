#!/usr/bin/env bash
# Upload a local file into the Codespace over the ssh channel (base64 in argv).
#   bash scripts/local/upload.sh <localFile> <remotePath>
set -uo pipefail
export HTTPS_PROXY=http://127.0.0.1:10808 HTTP_PROXY=http://127.0.0.1:10808
export https_proxy=http://127.0.0.1:10808 http_proxy=http://127.0.0.1:10808
GH="/c/Program Files/GitHub CLI/gh.exe"
CS="glowing-train-p7j9gp74q6jwc76v6"
LOCAL="$1"
REMOTE="$2"
B64=$(base64 -w0 < "$LOCAL")
"$GH" codespace ssh -c "$CS" -- bash -lc "cat > $REMOTE" < "$LOCAL" 2>&1
echo "uploaded $LOCAL -> $REMOTE ($(wc -c < "$LOCAL") bytes)"
