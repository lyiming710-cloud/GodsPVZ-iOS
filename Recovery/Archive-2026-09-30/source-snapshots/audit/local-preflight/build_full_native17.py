from pathlib import Path
import re
R=Path(__file__).resolve().parents[1]/'native15-work'
s=(R/'.github/workflows/stage9-native6-real-ios-xcode-gate.yml').read_text()
s=s.replace('name: Stage9.2 native6 REAL Unity iOS Xcode Gate','name: Stage9 native17 REAL Unity iOS Xcode Gate')
a=s.index('on:');b=s.index('permissions:')
s=s[:a]+'''on:
  push:
    branches: [repair/codespace-native14-validation-2026-09-27]
    paths: ['.github/workflows/stage9-native17-real-ios-xcode-gate.yml']
  workflow_dispatch:

concurrency:
  group: stage9-native17-real-ios-xcode-gate
  cancel-in-progress: false

'''+s[b:]
s=s.replace('  export:\n    runs-on:', '  export:\n    needs: qualification\n    runs-on:')
s=s.replace("'70a45b9add1e402ba8aeea5c1bf315cf62a0c7d0f39481528c88b0cc4c0688ee'","'74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2'")
s='\n'.join(l for l in s.splitlines() if not ('TEST_CANDIDATE_' in l or "echo 'formal_native4_" in l))+'\n'
a=s.index('      - name: Download materialized');b=s.index('      - name: Download exact cached R3 archive',a)
s=s[:a]+'''      - name: Download same-run conversion-qualified native17 candidate
        uses: actions/download-artifact@v4
        with:
          name: Stage9-native17-conversion-qualified
          path: ${{ runner.temp }}/stage9-native17-ios/native17-qualified

      - name: Verify native17 candidate provenance and exact hash
        shell: bash
        run: |
          set -euo pipefail
          OUT="$RUNNER_TEMP/stage9-native17-ios"
          python3 - "$OUT" "$TEST_DLL_SHA256" "$GITHUB_SHA" <<'PY'
          from pathlib import Path
          import sys,json,hashlib,shutil
          out=Path(sys.argv[1]);src=out/'native17-qualified'
          p=json.loads((src/'PROVENANCE.json').read_text())
          assert p['status']=='NATIVE17_DIRECT_CONVERSION_PASS'
          assert p['native17']['exit']==0 and p['native17']['methods']==[]
          assert p['source_commit']==sys.argv[3]
          dll=src/'Assembly-CSharp-native17.dll'
          assert hashlib.sha256(dll.read_bytes()).hexdigest()==sys.argv[2]
          assert p['outputs']['unlinked']['sha256']==sys.argv[2]
          name=(src/'candidate-assembly-name.txt').read_text().strip()
          assert name in ('Assembly-CSharp','GodsPVZRuntime1')
          shutil.copy2(dll,out/'candidate'/dll.name)
          for n in ('PROVENANCE.json','candidate-inspection.txt','candidate-assembly-name.txt'):
              shutil.copy2(src/n,out/'evidence'/n)
          PY
          echo 'NATIVE17_CONVERSION_QUALIFIED_INPUT_PASS'

'''+s[b:]
s=s.replace('stage9-native4-ios','stage9-native17-ios').replace('Assembly-CSharp-native6-ladder.dll','Assembly-CSharp-native17.dll').replace('native6','native17').replace('NATIVE6','NATIVE17').replace('Stage9.2-native17','Stage9-native17')
s=s.replace('"$X/Il2CppOutputProject/Source/il2cppOutput/Assembly-CSharp.cpp"','"$X/Il2CppOutputProject/Source/il2cppOutput/$ASSEMBLY.cpp"')
s=s.replace('          test -d "$X/Il2CppOutputProject"','          test -d "$X/Il2CppOutputProject"\n          ASSEMBLY="$(cat "$E/candidate-assembly-name.txt")"')
s=s.replace("            echo 'native17_semantic_formalization=NO'","            echo 'native17_runtime_qualification=NOT_YET_PERFORMED'\n            echo 'native17_direct_il2cpp_conversion=PASS'")
qualification='''  qualification:
    runs-on: ubuntu-latest
    timeout-minutes: 25
    env:
      GH_TOKEN: ${{ github.token }}
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-python@v5
        with:
          python-version: '3.12'
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - name: Restore hash-locked inputs and verify native14 control
        run: python3 scripts/codespaces/run_native14_validation.py
      - name: Execute native15 through native17 CLR behavior fixtures
        shell: bash
        run: |
          set -euo pipefail
          mkdir -p .validation/native17
          for n in 15 16 17; do
            python3 "scripts/codespaces/test_native${n}_fixture.py" | tee ".validation/native17/fixture${n}.log"
          done
      - name: Require zero-error exact native17 IL2CPP conversion
        shell: bash
        run: |
          set -euo pipefail
          python3 scripts/codespaces/validate_native17.py
          python3 scripts/codespaces/package_native17.py
      - name: Upload conversion-qualified candidate for this run
        uses: actions/upload-artifact@v4
        with:
          name: Stage9-native17-conversion-qualified
          path: .validation/native17/package/
          include-hidden-files: true
          if-no-files-found: error
      - name: Upload qualification evidence
        if: always()
        uses: actions/upload-artifact@v4
        with:
          name: Stage9-native17-qualification-evidence
          path: |
            .validation/native14/replay-*/*.log
            .validation/native14/replay-*/*result.json
            .validation/native17/*.log
            .validation/native17/latest-result.json
          include-hidden-files: true
          if-no-files-found: warn

'''
s=s.replace('jobs:\n','jobs:\n'+qualification,1)
p=R/'.github/workflows/stage9-native17-real-ios-xcode-gate.yml';p.write_text(s,newline='\n')
assert 'native6' not in s and 'TEST_CANDIDATE_' not in s and 'formal_native4' not in s
print('WORKFLOW_GENERATED',len(s.splitlines()))
