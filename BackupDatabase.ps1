[CmdletBinding()]
param(
    [string]$ServerInstance = '.\SQLEXPRESS',
    [string]$Database = 'master',
    [string]$BackupDirectory = "$env:ProgramData\MecanicaElEnano\Backups",
    [ValidateRange(1, 3650)]
    [int]$RetentionDays = 30
)

$ErrorActionPreference = 'Stop'

if ($Database -notmatch '^[A-Za-z0-9_-]+$') {
    throw "Database contains unsupported characters: $Database"
}

$sqlcmd = Get-Command sqlcmd.exe -ErrorAction SilentlyContinue
if (-not $sqlcmd) {
    throw 'sqlcmd.exe was not found. Install Microsoft SQL Server command-line utilities.'
}

New-Item -ItemType Directory -Path $BackupDirectory -Force | Out-Null

$timestamp = Get-Date -Format 'yyyyMMdd_HHmmss'
$backupFile = Join-Path $BackupDirectory "${Database}_${timestamp}.bak"
$logFile = Join-Path $BackupDirectory 'backup.log'
$sqlBackupFile = $backupFile.Replace("'", "''")
$query = @"
BACKUP DATABASE [$Database]
TO DISK = N'$sqlBackupFile'
WITH INIT, CHECKSUM, STATS = 10;

RESTORE VERIFYONLY
FROM DISK = N'$sqlBackupFile'
WITH CHECKSUM;
"@

"[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] Starting backup of [$Database] to $backupFile" |
    Add-Content -LiteralPath $logFile

$commandOutput = & $sqlcmd.Source -S $ServerInstance -E -C -b -V 16 -Q $query 2>&1
$commandExitCode = $LASTEXITCODE
$commandOutput | Add-Content -LiteralPath $logFile

if ($commandExitCode -ne 0) {
    "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] Backup failed with exit code $commandExitCode." |
        Add-Content -LiteralPath $logFile
    throw "Database backup failed. See $logFile"
}

$cutoff = (Get-Date).AddDays(-$RetentionDays)
$expiredBackups = Get-ChildItem -LiteralPath $BackupDirectory -Filter "${Database}_*.bak" -File |
    Where-Object LastWriteTime -LT $cutoff

foreach ($expiredBackup in $expiredBackups) {
    Remove-Item -LiteralPath $expiredBackup.FullName -Force
}

"[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] Backup verified successfully. Removed $($expiredBackups.Count) expired backup(s)." |
    Add-Content -LiteralPath $logFile

Write-Host "Verified backup created: $backupFile" -ForegroundColor Green
