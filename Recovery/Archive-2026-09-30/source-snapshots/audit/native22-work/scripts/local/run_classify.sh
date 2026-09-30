set -e
cd /workspaces/GodsPVZ-native19/.validation/native22
echo "=== uploaded size: $(wc -c < native22_classify.py) bytes"
python3 native22_classify.py 2>&1 | tail -120
