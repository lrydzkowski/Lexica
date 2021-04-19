#
# Script responsible for configuration file initialization (appsettings.json).
#

try
{
    $sampleAppSettingsPath = "$PSScriptRoot\..\..\appsettings.sample.json"
    $appSettingsPath = "$PSScriptRoot\..\..\appsettings.json"
    if (!(Test-Path $appSettingsPath))
    {
        Copy-Item -Path $sampleAppSettingsPath -Destination $appSettingsPath
    }
}
catch
{
	Write-Host "An error occured:" -ForegroundColor red;
    Write-Host $_ -ForegroundColor red;
    Write-Host $_.ErrorDetails -ForegroundColor red;
    Write-Host $_.ScriptStackTrace -ForegroundColor red;
    exit 1
}