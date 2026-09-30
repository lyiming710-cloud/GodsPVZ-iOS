set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native4-ios"
E="$OUT/evidence"
if ! command -v xvfb-run >/dev/null 2>&1 || ! command -v glxinfo >/dev/null 2>&1; then
  sudo apt-get update -qq
  sudo apt-get install -y --no-install-recommends xvfb mesa-utils libgl1-mesa-dri
fi
export LIBGL_ALWAYS_SOFTWARE=1
xvfb-run -a -s '-screen 0 1280x720x24' glxinfo -B > "$E/software-gl-renderer.txt" 2>&1
xvfb-run -a -s '-screen 0 1280x720x24' glxinfo -l > "$E/software-gl-limits.txt" 2>&1
MAX_TEXTURE_SIZE="$(sed -n 's/.*GL_MAX_TEXTURE_SIZE[^0-9]*\([0-9][0-9]*\).*/\1/p' "$E/software-gl-limits.txt" | head -n1)"
echo "GL_MAX_TEXTURE_SIZE=$MAX_TEXTURE_SIZE" | tee "$E/software-gl-max-texture.txt"
[ -n "$MAX_TEXTURE_SIZE" ] || exit 66
[ "$MAX_TEXTURE_SIZE" -ge 8192 ] || exit 67

