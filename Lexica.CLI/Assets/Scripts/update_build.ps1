#
# Script responsible for updating a build number which is stored in build file in the main directory.
#
# Script requires PowerShell 7 (https://github.com/PowerShell/PowerShell)
#

try
{
    $build = (Get-Date -format "yyyy-MM-ddTHH:mm")
    $build | Set-Content -NoNewline -Path "$PSScriptRoot\..\..\build"
}
catch
{
	Write-Host "An error occured:" -ForegroundColor red;
    Write-Host $_ -ForegroundColor red;
    Write-Host $_.ErrorDetails -ForegroundColor red;
    Write-Host $_.ScriptStackTrace -ForegroundColor red;
}