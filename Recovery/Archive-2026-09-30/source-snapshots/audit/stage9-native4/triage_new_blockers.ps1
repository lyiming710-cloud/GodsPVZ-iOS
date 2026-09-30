$ErrorActionPreference='Stop'
$taskRoot=$PSScriptRoot
Add-Type -Path "$taskRoot/tools/lib/netstandard2.0/Mono.Cecil.dll"
$inputs=@{baseline=(Join-Path $taskRoot '../../Assembly-CSharp-59bb-native2.dll');candidate=(Join-Path $taskRoot 'candidate/Assembly-CSharp-native4-six-method.dll')}
$dump=@{}
foreach($kind in @('baseline','candidate')){
 $a=[Mono.Cecil.AssemblyDefinition]::ReadAssembly($inputs[$kind])
 foreach($token in @(0x060006E3,0x06000498)){
  $md=$a.MainModule.LookupToken([Mono.Cecil.MetadataToken]::new([uint32]$token))
  $lines=[System.Collections.Generic.List[string]]::new()
  $lines.Add("$($md.MetadataToken) $($md.FullName) CodeSize=$($md.Body.CodeSize) MaxStack=$($md.Body.MaxStackSize) InitLocals=$($md.Body.InitLocals) EH=$($md.Body.ExceptionHandlers.Count)")
  foreach($v in $md.Body.Variables){$lines.Add("V_$($v.Index) $($v.VariableType.FullName)")}
  foreach($ins in $md.Body.Instructions){$lines.Add($ins.ToString())}
  foreach($eh in $md.Body.ExceptionHandlers){$lines.Add("EH $($eh.HandlerType) try=$($eh.TryStart)-$($eh.TryEnd) handler=$($eh.HandlerStart)-$($eh.HandlerEnd)")}
  $dump["$kind-$token"]=$lines -join "`n"
  $lines | Set-Content "$taskRoot/evidence/$($md.DeclaringType.Name).$($md.Name).$kind.il.txt" -Encoding utf8
 }
 $a.Dispose()
}
foreach($token in @(0x060006E3,0x06000498)){
 if($dump["baseline-$token"] -cne $dump["candidate-$token"]){throw "Unexpected body difference $token"}
 'UNCHANGED_BODY {0:X8}' -f $token
}
