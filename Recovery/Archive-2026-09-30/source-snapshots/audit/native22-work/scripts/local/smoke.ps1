Write-Output "HELLO-OK"
Get-Date -Format o | Write-Output
$gh = Get-Command gh -ErrorAction SilentlyContinue
if ($gh) { Write-Output ("GH=" + $gh.Source) } else { Write-Output "GH=NOT-FOUND" }
Write-Output ("PATH2=" + $env:PATH.Substring(0, [Math]::Min(200, $env:PATH.Length)))
