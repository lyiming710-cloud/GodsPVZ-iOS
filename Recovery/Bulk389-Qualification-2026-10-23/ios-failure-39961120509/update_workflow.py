from pathlib import Path

repo = Path(__file__).resolve().parents[1] / 'repo-native113'
path = repo / '.github/workflows/bulk389-real-ios-ipa.yml'
text = path.read_text('utf-8')
def replace(old, new):
    global text
    assert text.count(old) == 1, old
    text = text.replace(old, new)

replace('      - uses: actions/checkout@v4\n      - uses: actions/download-artifact@v4', '''      - uses: actions/checkout@v4
      - name: Run license-free pipeline regression controls
        run: python3 scripts/actions/check_bulk389_pipeline.py > "$RUNNER_TEMP/BULK389-PIPELINE-CONTROLS.json"
      - uses: actions/download-artifact@v4''')
replace('  export:\n    needs: conversion_binding', '''  apple_preflight:
    runs-on: macos-15
    timeout-minutes: 10
    steps:
      - name: Verify actual Xcode, physical iOS SDK and packaging tools early
        shell: bash
        run: |
          set -euo pipefail
          test -d /Applications/Xcode_16.4.app/Contents/Developer
          sudo xcode-select -s /Applications/Xcode_16.4.app/Contents/Developer
          xcodebuild -version
          [ "$(xcrun --sdk iphoneos --show-sdk-version)" = '18.5' ]
          xcrun --sdk iphoneos --find clang
          command -v ditto
          command -v python3
          python3 -c 'import plistlib,struct,zipfile,hashlib'

  export:
    needs: [conversion_binding, apple_preflight]''')
start = text.index('      - name: Prepare software graphics for original 8192 TMP atlas\n')
end = text.index('      - name: Export unsigned iOS Xcode project with bulk389 candidate\n', start)
graphics = text[start:end]
text = text[:start] + text[end:]
replace('      - name: Reclaim runner disk\n', graphics + '      - name: Reclaim runner disk\n')
replace('      - name: Acquire Personal seat\n', '''      - name: Compile both helper states and verify licensing CLI before activation
        shell: bash
        run: |
          set -euo pipefail
          OUT="$RUNNER_TEMP/stage9-bulk389-ios"
          E="$OUT/evidence"
          UNITY="$(cat "$OUT/unity-path.txt")"
          PROJECT="$(cat "$OUT/project-path.txt")"
          set +e
          python3 scripts/actions/preflight_bulk389_editor.py --editor "$UNITY" --project "$PROJECT" --output "$OUT/helper-preflight"
          RC=$?
          set -e
          cp "$OUT/helper-preflight/"*.log "$OUT/helper-preflight/"*.json "$OUT/helper-preflight/"*.rsp "$E/"
          [ "$RC" -eq 0 ] || exit 70
          CLIENT="$(dirname "$UNITY")/Data/Resources/Licensing/Client/Unity.Licensing.Client"
          "$CLIENT" --help > "$E/licensing-client-help.txt" 2>&1
          grep -q -- '--activate-all' "$E/licensing-client-help.txt"
          grep -q -- '--deactivate-all' "$E/licensing-client-help.txt"

      - name: Acquire Personal seat
''')
start = text.index('      - name: Inject materialized experimental bulk389 DLL after migration\n')
end = text.index('      - name: Reimport exact bulk389 work copy without migrator\n', start)
text = text[:start] + '''      - name: Inject fixed candidate and retire legacy migrator without deleting its source path
        shell: bash
        run: |
          set -euo pipefail
          OUT="$RUNNER_TEMP/stage9-bulk389-ios"
          PROJECT="$(cat "$OUT/project-path.txt")"
          python3 scripts/actions/prepare_bulk389_unity.py --project "$PROJECT" --candidate "$OUT/candidate/Assembly-CSharp-bulk389.dll" --evidence "$OUT/evidence"

''' + text[end:]
replace('Reimport exact bulk389 work copy without migrator', 'Reimport fixed candidate with inert migrator and rebuilt script caches')
replace('          X="$OUT/xcode"\n          PBX=', '''          X="$OUT/xcode"
          PROJECT="$(cat "$OUT/project-path.txt")"
          python3 scripts/actions/bulk389_export_binding.py --xcode-dir "$X" --plugin "$PROJECT/Assets/Plugins/Assembly-CSharp.dll"
          cp "$X/bulk389-export-binding.json" "$E/"
          PBX=''')
replace('            "$CLIENT" --return-ulf > "$E/license-return.log" 2>&1', '            "$CLIENT" --deactivate-all --username "$UNITY_EMAIL" --password "$UNITY_PASSWORD" > "$E/license-return.log" 2>&1')
replace('            echo "$RC" > "$E/license-return-exit.txt"\n          fi', '            echo "$RC" > "$E/license-return-exit.txt"\n            [ "$RC" -eq 0 ] || exit 71\n          fi')
replace('          zstd -dc "$ARCHIVE" | tar -xf - -C "${{ runner.temp }}/build"\n', '''          zstd -dc "$ARCHIVE" | tar -xf - -C "${{ runner.temp }}/build"
          python3 scripts/actions/bulk389_export_binding.py --xcode-dir "$RUNNER_TEMP/build/xcode"
''')
path.write_text(text, encoding='utf-8', newline='\n')
print('WORKFLOW_UPDATED')
