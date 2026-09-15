#!/usr/bin/env bash
set -euo pipefail

THRESHOLD_GIB="${1:-24}"
case "$THRESHOLD_GIB" in
  ''|*[!0-9]*) echo "invalid threshold GiB: $THRESHOLD_GIB" >&2; exit 2 ;;
esac

avail_kb=$(df --output=avail -k / | tail -1 | tr -d ' ')
threshold_kb=$((THRESHOLD_GIB * 1024 * 1024))
echo "STAGE9_DISK_BEFORE available_kb=$avail_kb threshold_kb=$threshold_kb"

if [ "$avail_kb" -ge "$threshold_kb" ]; then
  echo "STAGE9_DISK_CLEANUP skipped=1 reason=enough_space"
  df -h /
  exit 0
fi

# These hosted-runner toolchains are not used by Stage9's Unity/Mono recovery gates.
# Only remove them when the runner actually needs space; unconditional removal has
# previously cost roughly a minute even on runners that already had sufficient room.
sudo rm -rf /usr/local/lib/android /opt/ghc /usr/local/.ghcup /opt/hostedtoolcache/CodeQL || true
sudo apt-get clean || true

avail_after_kb=$(df --output=avail -k / | tail -1 | tr -d ' ')
echo "STAGE9_DISK_CLEANUP skipped=0 available_after_kb=$avail_after_kb"
if [ "$avail_after_kb" -lt "$threshold_kb" ]; then
  echo "insufficient disk after conditional cleanup" >&2
  df -h /
  exit 3
fi

df -h /
