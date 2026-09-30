param([string]$Baseline=(Join-Path $PSScriptRoot '../../Assembly-CSharp-59bb-native2.dll'))
$ErrorActionPreference='Stop'
$taskRoot=$PSScriptRoot
Add-Type -Path "$taskRoot/tools/lib/netstandard2.0/Mono.Cecil.dll"
$assembly=[Mono.Cecil.AssemblyDefinition]::ReadAssembly($Baseline)
$m=$assembly.MainModule
function Get-AllTypes($items){foreach($t in $items){$t; Get-AllTypes $t.NestedTypes}}
$types=@(Get-AllTypes $m.Types)
$methods=@($types.Methods)
$fields=@($types.Fields)
$lines=[System.Collections.Generic.List[string]]::new()
foreach($t in $types){
 $lines.Add("TYPE $($t.MetadataToken) $($t.FullName) base=$($t.BaseType)")
 foreach($f in $t.Fields){$lines.Add("FIELD $($f.MetadataToken) $($f.FullName) const=$($f.Constant)")}
 foreach($method in $t.Methods){$lines.Add("METHOD $($method.MetadataToken) $($method.FullName)")}
}
$lines | Set-Content "$taskRoot/evidence/managed-identities.txt" -Encoding utf8
$m.GetMemberReferences() | ForEach-Object {"$($_.MetadataToken) $($_.FullName)"} | Set-Content "$taskRoot/evidence/memberrefs.txt" -Encoding utf8
foreach($token in @(0x060001E0,0x060001B2,0x06000248,0x060001A5,0x060001AE,0x060001AD,0x060001DE,0x0600017F)){
 $md=$m.LookupToken([Mono.Cecil.MetadataToken]::new([uint32]$token))
 $lines=[System.Collections.Generic.List[string]]::new()
 $lines.Add("$($md.MetadataToken) $($md.FullName) RVA=$('{0:X8}' -f $md.RVA) CodeSize=$($md.Body.CodeSize) MaxStack=$($md.Body.MaxStackSize) InitLocals=$($md.Body.InitLocals) LocalSig=$($md.Body.LocalVarToken) EH=$($md.Body.ExceptionHandlers.Count)")
 foreach($v in $md.Body.Variables){$lines.Add("V_$($v.Index) $($v.VariableType.FullName)")}
 foreach($ins in $md.Body.Instructions){$lines.Add($ins.ToString())}
 foreach($eh in $md.Body.ExceptionHandlers){$lines.Add("EH $($eh.HandlerType) try=$($eh.TryStart)-$($eh.TryEnd) handler=$($eh.HandlerStart)-$($eh.HandlerEnd)")}
 $lines | Set-Content "$taskRoot/evidence/$($md.DeclaringType.Name).$($md.Name).baseline.il.txt" -Encoding utf8
}
"MethodDef=$($methods.Count) FieldDef=$($fields.Count) Kind=$($m.Kind)"
$assembly.Dispose()
