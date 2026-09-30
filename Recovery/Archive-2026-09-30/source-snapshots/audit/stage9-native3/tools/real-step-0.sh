set -euo pipefail
df -h
sudo rm -rf /usr/local/lib/android || true
sudo rm -rf /usr/share/dotnet || true
sudo rm -rf /opt/ghc || true
sudo rm -rf /usr/local/.ghcup || true
docker system prune -af || true
df -h

