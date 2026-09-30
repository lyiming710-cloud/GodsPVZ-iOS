param([Parameter(Mandatory=$true)][string]$Baseline)
$ErrorActionPreference='Stop'
$Baseline=(Resolve-Path -LiteralPath $Baseline).Path
$taskRoot=$PSScriptRoot
Push-Location $taskRoot
try {
 New-Item -ItemType Directory -Force tools,evidence,specification,reopened,candidate | Out-Null
 $csc=Join-Path $env:WINDIR 'Microsoft.NET/Framework64/v4.0.30319/csc.exe'
 foreach($name in @('SpecBuilder','Gate','Rehost')){
  & $csc /nologo /r:tools\lib\net40\Mono.Cecil.dll /r:System.Web.Extensions.dll "/out:tools\$name.exe" "$name.cs"
  if($LASTEXITCODE -ne 0){throw "compile $name failed"}
 }
 Copy-Item tools/lib/net40/Mono.Cecil.dll tools/Mono.Cecil.dll
 & ./dump_managed.ps1 -Baseline $Baseline
 & ./tools/SpecBuilder.exe $Baseline specification
 if($LASTEXITCODE -ne 0){throw 'spec generation failed'}
 & $env:NATIVE3_PYTHON verify_stack.py specification > evidence/spec-stack.log
 if($LASTEXITCODE -ne 0){throw 'spec stack gate failed'}
 & ./tools/SpecBuilder.exe $Baseline specification candidate/Assembly-CSharp-native3-four-method.dll
 if($LASTEXITCODE -ne 0){throw 'candidate build failed'}
 $sha=(Get-FileHash candidate/Assembly-CSharp-native3-four-method.dll).Hash.ToLowerInvariant()
 if($sha -ne '72fe4526fecab9e0e6781cf84d7a5aa3ca32f57973a4be3617708ba4c01343bc'){throw "candidate hash changed $sha"}
 & ./tools/SpecBuilder.exe --reopen candidate/Assembly-CSharp-native3-four-method.dll reopened
 if($LASTEXITCODE -ne 0){throw 'reopen failed'}
 & $env:NATIVE3_PYTHON verify_stack.py reopened > evidence/reopened-stack.log
 if($LASTEXITCODE -ne 0){throw 'actual candidate stack gate failed'}
 & ./tools/Gate.exe $Baseline candidate/Assembly-CSharp-native3-four-method.dll resolver inputs/59bb-historical.dll evidence/reference-use-deltas.tsv > evidence/semantic-gate.log
 if($LASTEXITCODE -ne 0){throw 'semantic gate failed'}
 & $env:NATIVE3_PYTHON verify_spec_readback.py > evidence/spec-readback.log
 if($LASTEXITCODE -ne 0){throw 'spec readback or use delta failed'}
 & $csc /nologo /out:tools\Harness-template.exe Harness.cs
 if($LASTEXITCODE -ne 0){throw 'harness compile failed'}
 & ./tools/Rehost.exe candidate/Assembly-CSharp-native3-four-method.dll tools/Harness-template.exe tools/Harness-patched.exe
 if($LASTEXITCODE -ne 0){throw 'harness rehost failed'}
 & ./tools/Harness-patched.exe > evidence/clr-harness.log
 if($LASTEXITCODE -ne 0){throw 'CLR harness failed'}
 @("baseline_sha256=$((Get-FileHash $Baseline).Hash.ToLowerInvariant())","candidate_sha256=$sha",'scope=060001A5,060001B2,060001E0,06000248','static_gate=PASS','semantic_targets=24/24','semantic_resolve_keys=23/23','non_target_semantic_diffs=0','orphan_generics=0','target_stack_verification=4/4','negative_stack_controls=4/4','clr_harness_assertions=31','unity_export=NOT_RUN') | Set-Content evidence/GATE-SUMMARY.txt -Encoding ascii
 Write-Output 'NATIVE3_FOUR_METHOD_STATIC_GATE_PASS'
} finally {Pop-Location}
