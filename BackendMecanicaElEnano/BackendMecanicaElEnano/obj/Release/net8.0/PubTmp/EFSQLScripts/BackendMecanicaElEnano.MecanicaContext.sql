IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE TABLE [vehiculo] (
        [IdVehiculo] uniqueidentifier NOT NULL,
        [Patente] nvarchar(7) NOT NULL,
        [Modelo] nvarchar(max) NOT NULL,
        [NumeroChasis] nvarchar(max) NOT NULL,
        [Cliente] nvarchar(max) NOT NULL,
        [Telefono] nvarchar(max) NOT NULL,
        [Direccion] nvarchar(60) NOT NULL,
        [Mail] nvarchar(60) NOT NULL,
        [Cuit] nvarchar(11) NOT NULL,
        CONSTRAINT [PK_vehiculo] PRIMARY KEY ([IdVehiculo])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE TABLE [presupuesto] (
        [IdPresupuesto] uniqueidentifier NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [ValidoHasta] datetime2 NOT NULL,
        [Km] int NOT NULL,
        [TrabajoARealizar] nvarchar(max) NOT NULL,
        [VehiculoIdVehiculo] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_presupuesto] PRIMARY KEY ([IdPresupuesto]),
        CONSTRAINT [FK_presupuesto_vehiculo_VehiculoIdVehiculo] FOREIGN KEY ([VehiculoIdVehiculo]) REFERENCES [vehiculo] ([IdVehiculo]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE TABLE [trabajo] (
        [IdPresupuesto] uniqueidentifier NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [Km] int NOT NULL,
        [TrabajosRealizados] nvarchar(max) NOT NULL,
        [TrabajosPendientes] nvarchar(max) NOT NULL,
        [VehiculoIdVehiculo] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_trabajo] PRIMARY KEY ([IdPresupuesto]),
        CONSTRAINT [FK_trabajo_vehiculo_VehiculoIdVehiculo] FOREIGN KEY ([VehiculoIdVehiculo]) REFERENCES [vehiculo] ([IdVehiculo]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE TABLE [repuesto] (
        [IdRepuesto] uniqueidentifier NOT NULL,
        [Cantidad] int NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [precio] real NOT NULL,
        [Tipo] int NOT NULL,
        [PresupuestoIdPresupuesto] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_repuesto] PRIMARY KEY ([IdRepuesto]),
        CONSTRAINT [FK_repuesto_presupuesto_PresupuestoIdPresupuesto] FOREIGN KEY ([PresupuestoIdPresupuesto]) REFERENCES [presupuesto] ([IdPresupuesto]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE TABLE [repuestoTrabajo] (
        [IdRepuestoTrabajo] uniqueidentifier NOT NULL,
        [Cantidad] int NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [precio] real NOT NULL,
        [Tipo] int NOT NULL,
        [PresupuestoIdPresupuesto] uniqueidentifier NOT NULL,
        [TrabajoIdPresupuesto] uniqueidentifier NULL,
        CONSTRAINT [PK_repuestoTrabajo] PRIMARY KEY ([IdRepuestoTrabajo]),
        CONSTRAINT [FK_repuestoTrabajo_presupuesto_PresupuestoIdPresupuesto] FOREIGN KEY ([PresupuestoIdPresupuesto]) REFERENCES [presupuesto] ([IdPresupuesto]) ON DELETE CASCADE,
        CONSTRAINT [FK_repuestoTrabajo_trabajo_TrabajoIdPresupuesto] FOREIGN KEY ([TrabajoIdPresupuesto]) REFERENCES [trabajo] ([IdPresupuesto])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE INDEX [IX_presupuesto_VehiculoIdVehiculo] ON [presupuesto] ([VehiculoIdVehiculo]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE INDEX [IX_repuesto_PresupuestoIdPresupuesto] ON [repuesto] ([PresupuestoIdPresupuesto]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE INDEX [IX_repuestoTrabajo_PresupuestoIdPresupuesto] ON [repuestoTrabajo] ([PresupuestoIdPresupuesto]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE INDEX [IX_repuestoTrabajo_TrabajoIdPresupuesto] ON [repuestoTrabajo] ([TrabajoIdPresupuesto]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    CREATE INDEX [IX_trabajo_VehiculoIdVehiculo] ON [trabajo] ([VehiculoIdVehiculo]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221011224915_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221011224915_InitialCreate', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [presupuesto] DROP CONSTRAINT [FK_presupuesto_vehiculo_VehiculoIdVehiculo];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [repuesto] DROP CONSTRAINT [FK_repuesto_presupuesto_PresupuestoIdPresupuesto];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [repuestoTrabajo] DROP CONSTRAINT [FK_repuestoTrabajo_presupuesto_PresupuestoIdPresupuesto];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [repuestoTrabajo] DROP CONSTRAINT [FK_repuestoTrabajo_trabajo_TrabajoIdPresupuesto];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [trabajo] DROP CONSTRAINT [FK_trabajo_vehiculo_VehiculoIdVehiculo];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[vehiculo].[IdVehiculo]', N'VehiculoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[trabajo].[VehiculoIdVehiculo]', N'VehiculoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[trabajo].[IdPresupuesto]', N'TrabajoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[trabajo].[IX_trabajo_VehiculoIdVehiculo]', N'IX_trabajo_VehiculoId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[repuestoTrabajo].[TrabajoIdPresupuesto]', N'TrabajoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[repuestoTrabajo].[PresupuestoIdPresupuesto]', N'PresupuestoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[repuestoTrabajo].[IdRepuestoTrabajo]', N'RepuestoTrabajoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[repuestoTrabajo].[IX_repuestoTrabajo_TrabajoIdPresupuesto]', N'IX_repuestoTrabajo_TrabajoId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[repuestoTrabajo].[IX_repuestoTrabajo_PresupuestoIdPresupuesto]', N'IX_repuestoTrabajo_PresupuestoId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[repuesto].[PresupuestoIdPresupuesto]', N'PresupuestoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[repuesto].[IdRepuesto]', N'RepuestoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[repuesto].[IX_repuesto_PresupuestoIdPresupuesto]', N'IX_repuesto_PresupuestoId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[presupuesto].[VehiculoIdVehiculo]', N'VehiculoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[presupuesto].[IdPresupuesto]', N'PresupuestoId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    EXEC sp_rename N'[presupuesto].[IX_presupuesto_VehiculoIdVehiculo]', N'IX_presupuesto_VehiculoId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [presupuesto] ADD CONSTRAINT [FK_presupuesto_vehiculo_VehiculoId] FOREIGN KEY ([VehiculoId]) REFERENCES [vehiculo] ([VehiculoId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [repuesto] ADD CONSTRAINT [FK_repuesto_presupuesto_PresupuestoId] FOREIGN KEY ([PresupuestoId]) REFERENCES [presupuesto] ([PresupuestoId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [repuestoTrabajo] ADD CONSTRAINT [FK_repuestoTrabajo_presupuesto_PresupuestoId] FOREIGN KEY ([PresupuestoId]) REFERENCES [presupuesto] ([PresupuestoId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [repuestoTrabajo] ADD CONSTRAINT [FK_repuestoTrabajo_trabajo_TrabajoId] FOREIGN KEY ([TrabajoId]) REFERENCES [trabajo] ([TrabajoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    ALTER TABLE [trabajo] ADD CONSTRAINT [FK_trabajo_vehiculo_VehiculoId] FOREIGN KEY ([VehiculoId]) REFERENCES [vehiculo] ([VehiculoId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014021906_idFix')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221014021906_idFix', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230301190620_nullablecolpresupuesto')
BEGIN
    ALTER TABLE [presupuesto] DROP CONSTRAINT [FK_presupuesto_vehiculo_VehiculoId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230301190620_nullablecolpresupuesto')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[presupuesto]') AND [c].[name] = N'VehiculoId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [presupuesto] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [presupuesto] ALTER COLUMN [VehiculoId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230301190620_nullablecolpresupuesto')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[presupuesto]') AND [c].[name] = N'ValidoHasta');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [presupuesto] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [presupuesto] ALTER COLUMN [ValidoHasta] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230301190620_nullablecolpresupuesto')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[presupuesto]') AND [c].[name] = N'TrabajoARealizar');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [presupuesto] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [presupuesto] ALTER COLUMN [TrabajoARealizar] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230301190620_nullablecolpresupuesto')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[presupuesto]') AND [c].[name] = N'Km');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [presupuesto] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [presupuesto] ALTER COLUMN [Km] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230301190620_nullablecolpresupuesto')
BEGIN
    ALTER TABLE [presupuesto] ADD CONSTRAINT [FK_presupuesto_vehiculo_VehiculoId] FOREIGN KEY ([VehiculoId]) REFERENCES [vehiculo] ([VehiculoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230301190620_nullablecolpresupuesto')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230301190620_nullablecolpresupuesto', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230815015804_turno')
BEGIN
    CREATE TABLE [turno] (
        [TurnoId] uniqueidentifier NOT NULL,
        [FechayHora] datetime2 NOT NULL,
        [VehiculoId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_turno] PRIMARY KEY ([TurnoId]),
        CONSTRAINT [FK_turno_vehiculo_VehiculoId] FOREIGN KEY ([VehiculoId]) REFERENCES [vehiculo] ([VehiculoId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230815015804_turno')
BEGIN
    CREATE INDEX [IX_turno_VehiculoId] ON [turno] ([VehiculoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230815015804_turno')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230815015804_turno', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    ALTER TABLE [repuestoTrabajo] DROP CONSTRAINT [FK_repuestoTrabajo_presupuesto_PresupuestoId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    ALTER TABLE [repuestoTrabajo] DROP CONSTRAINT [FK_repuestoTrabajo_trabajo_TrabajoId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    ALTER TABLE [trabajo] DROP CONSTRAINT [FK_trabajo_vehiculo_VehiculoId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    DROP INDEX [IX_repuestoTrabajo_PresupuestoId] ON [repuestoTrabajo];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[repuestoTrabajo]') AND [c].[name] = N'PresupuestoId');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [repuestoTrabajo] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [repuestoTrabajo] DROP COLUMN [PresupuestoId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[trabajo]') AND [c].[name] = N'VehiculoId');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [trabajo] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [trabajo] ALTER COLUMN [VehiculoId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    DROP INDEX [IX_repuestoTrabajo_TrabajoId] ON [repuestoTrabajo];
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[repuestoTrabajo]') AND [c].[name] = N'TrabajoId');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [repuestoTrabajo] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [repuestoTrabajo] ALTER COLUMN [TrabajoId] uniqueidentifier NOT NULL;
    ALTER TABLE [repuestoTrabajo] ADD DEFAULT '00000000-0000-0000-0000-000000000000' FOR [TrabajoId];
    CREATE INDEX [IX_repuestoTrabajo_TrabajoId] ON [repuestoTrabajo] ([TrabajoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    ALTER TABLE [repuestoTrabajo] ADD CONSTRAINT [FK_repuestoTrabajo_trabajo_TrabajoId] FOREIGN KEY ([TrabajoId]) REFERENCES [trabajo] ([TrabajoId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    ALTER TABLE [trabajo] ADD CONSTRAINT [FK_trabajo_vehiculo_VehiculoId] FOREIGN KEY ([VehiculoId]) REFERENCES [vehiculo] ([VehiculoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230819001025_trabajo')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230819001025_trabajo', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230828194828_turnodetalle')
BEGIN
    ALTER TABLE [turno] ADD [Detalle] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230828194828_turnodetalle')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230828194828_turnodetalle', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230902002745_ordenTrabajo')
BEGIN
    ALTER TABLE [turno] DROP CONSTRAINT [FK_turno_vehiculo_VehiculoId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230902002745_ordenTrabajo')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[turno]') AND [c].[name] = N'VehiculoId');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [turno] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [turno] ALTER COLUMN [VehiculoId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230902002745_ordenTrabajo')
BEGIN
    CREATE TABLE [ordenTrabajo] (
        [OrdenTrabajoId] uniqueidentifier NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [Km] int NOT NULL,
        [Manifiesto] nvarchar(max) NOT NULL,
        [Mecanico] nvarchar(max) NOT NULL,
        [VehiculoId] uniqueidentifier NULL,
        CONSTRAINT [PK_ordenTrabajo] PRIMARY KEY ([OrdenTrabajoId]),
        CONSTRAINT [FK_ordenTrabajo_vehiculo_VehiculoId] FOREIGN KEY ([VehiculoId]) REFERENCES [vehiculo] ([VehiculoId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230902002745_ordenTrabajo')
BEGIN
    CREATE INDEX [IX_ordenTrabajo_VehiculoId] ON [ordenTrabajo] ([VehiculoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230902002745_ordenTrabajo')
BEGIN
    ALTER TABLE [turno] ADD CONSTRAINT [FK_turno_vehiculo_VehiculoId] FOREIGN KEY ([VehiculoId]) REFERENCES [vehiculo] ([VehiculoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230902002745_ordenTrabajo')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230902002745_ordenTrabajo', N'6.0.10');
END;
GO

COMMIT;
GO

