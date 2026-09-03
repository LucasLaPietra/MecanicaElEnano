# Database backups

The application currently stores its data in the SQL Server `master` database on
`.\SQLEXPRESS`. Backups run independently of the application, so the executable
does not need to be open at backup time.

## Install the daily backup

Open PowerShell as Administrator in the repository directory and run:

```powershell
.\InstalarBackupProgramado.ps1
```

The installer performs an immediate test backup and then registers
`\MecanicaElEnano\DatabaseBackup` in Windows Task Scheduler. By default, it runs
daily at 02:00, keeps 30 days of backups, and writes files to:

```text
C:\ProgramData\MecanicaElEnano\Backups
```

To select another time or retention period:

```powershell
.\InstalarBackupProgramado.ps1 -Hour 23 -Minute 30 -RetentionDays 60
```

Running the installer again updates the existing task. The Windows account that
installs the task must have permission to back up the database. The installer
grants the local SQL Server service permission to write into the backup folder.

## Monitoring

Review `backup.log` in the backup directory and the task's Last Run Result in
Windows Task Scheduler. A successful run creates a timestamped `.bak` file only
after SQL Server has completed `BACKUP DATABASE` and `RESTORE VERIFYONLY` with
checksums.

Store an additional copy on another physical device or protected off-site
storage. A backup that exists only on the database computer does not protect
against disk failure, theft, or ransomware.

## Restoring `master`

Restoring `master` is an administrator disaster-recovery operation. Do not try it
while the application or SQL Server is operating normally.

The replacement SQL Server instance must match the original version, edition,
patch level, features, and configuration as closely as possible. Start the SQL
Server instance in single-user mode, connect using `sqlcmd`, and execute:

```sql
RESTORE DATABASE [master]
FROM DISK = N'C:\ProgramData\MecanicaElEnano\Backups\master_YYYYMMDD_HHMMSS.bak'
WITH REPLACE;
```

SQL Server shuts down after restoring `master`. Remove the single-user startup
parameter and restart the service normally. Follow Microsoft's full procedure:
https://learn.microsoft.com/en-us/sql/relational-databases/backup-restore/restore-the-master-database-transact-sql
