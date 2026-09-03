[CmdletBinding()]
param(
    [string]$ServerInstance = '.\SQLEXPRESS',
    [string]$Database = 'master',
    [string]$BackupDirectory = "$env:ProgramData\MecanicaElEnano\Backups",
    [ValidateRange(1, 3650)]
    [int]$RetentionDays = 30,
    [ValidateRange(0, 23)]
    [int]$Hour = 2,
    [ValidateRange(0, 59)]
    [int]$Minute = 0
)

$ErrorActionPreference = 'Stop'

$identity = [Security.Principal.WindowsIdentity]::GetCurrent()
$principal = [Security.Principal.WindowsPrincipal]::new($identity)
$isAdministrator = $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdministrator) {
    throw 'Run this script from PowerShell as Administrator.'
}

$sourceScript = Join-Path $PSScriptRoot 'BackupDatabase.ps1'
if (-not (Test-Path -LiteralPath $sourceScript -PathType Leaf)) {
    throw "Backup script not found: $sourceScript"
}

if (-not (Get-Command sqlcmd.exe -ErrorAction SilentlyContinue)) {
    throw 'sqlcmd.exe was not found. Install Microsoft SQL Server command-line utilities.'
}

$installDirectory = Join-Path $env:ProgramData 'MecanicaElEnano'
$installedScript = Join-Path $installDirectory 'BackupDatabase.ps1'
New-Item -ItemType Directory -Path $installDirectory -Force | Out-Null
New-Item -ItemType Directory -Path $BackupDirectory -Force | Out-Null
Copy-Item -LiteralPath $sourceScript -Destination $installedScript -Force

$instanceName = ($ServerInstance -split '\\')[-1]
$sqlServiceAccount = if ($instanceName -eq '.' -or $instanceName -eq $env:COMPUTERNAME) {
    'NT SERVICE\MSSQLSERVER'
} else {
    "NT SERVICE\MSSQL`$$instanceName"
}

& icacls.exe $BackupDirectory /grant "${sqlServiceAccount}:(OI)(CI)M" | Out-Null
if ($LASTEXITCODE -ne 0) {
    throw "Could not grant backup-directory access to $sqlServiceAccount."
}

# Perform a real backup before registering the task, validating SQL and filesystem permissions.
& $installedScript `
    -ServerInstance $ServerInstance `
    -Database $Database `
    -BackupDirectory $BackupDirectory `
    -RetentionDays $RetentionDays

$taskName = 'DatabaseBackup'
$taskPath = '\MecanicaElEnano\'
$startTime = (Get-Date).Date.AddHours($Hour).AddMinutes($Minute)
$currentUser = $identity.Name
$arguments = @(
    '-NoProfile'
    '-NonInteractive'
    '-ExecutionPolicy Bypass'
    "-File `"$installedScript`""
    "-ServerInstance `"$ServerInstance`""
    "-Database `"$Database`""
    "-BackupDirectory `"$BackupDirectory`""
    "-RetentionDays $RetentionDays"
) -join ' '

$action = New-ScheduledTaskAction -Execute 'powershell.exe' -Argument $arguments
$trigger = New-ScheduledTaskTrigger -Daily -At $startTime
$taskPrincipal = New-ScheduledTaskPrincipal -UserId $currentUser -LogonType S4U -RunLevel Highest
$settings = New-ScheduledTaskSettingsSet `
    -StartWhenAvailable `
    -ExecutionTimeLimit (New-TimeSpan -Hours 1) `
    -AllowStartIfOnBatteries `
    -DontStopIfGoingOnBatteries

Register-ScheduledTask `
    -TaskName $taskName `
    -TaskPath $taskPath `
    -Action $action `
    -Trigger $trigger `
    -Principal $taskPrincipal `
    -Settings $settings `
    -Description "Daily verified SQL Server backup of database [$Database]." `
    -Force | Out-Null

Write-Host "Scheduled backup installed for $($startTime.ToString('HH:mm')) every day." -ForegroundColor Green
Write-Host "Backups: $BackupDirectory"
Write-Host "Task: $taskPath$taskName"
