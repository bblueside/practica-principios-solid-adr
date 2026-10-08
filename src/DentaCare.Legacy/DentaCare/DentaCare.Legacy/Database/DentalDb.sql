-- =============================================================
-- DentalDb - Esquema para DentaCare.Legacy
-- Ejecutar sobre el contenedor sql1 (localhost,1435)
-- =============================================================

IF DB_ID('DentalDb') IS NULL
    CREATE DATABASE DentalDb;
GO

USE DentalDb;
GO

-- Catálogos ---------------------------------------------------

IF OBJECT_ID('dbo.Especialidades') IS NULL
CREATE TABLE dbo.Especialidades (
    Id            INT           NOT NULL CONSTRAINT PK_Especialidades PRIMARY KEY,
    Nombre        NVARCHAR(50)  NOT NULL,
    FactorCopago  DECIMAL(5,2)  NOT NULL   -- multiplicador sobre el costo base de consulta
);
GO

IF OBJECT_ID('dbo.TiposConvenio') IS NULL
CREATE TABLE dbo.TiposConvenio (
    Id               INT           NOT NULL CONSTRAINT PK_TiposConvenio PRIMARY KEY,
    Nombre           NVARCHAR(50)  NOT NULL,
    FactorDescuento  DECIMAL(5,2)  NOT NULL  -- porción del copago que paga el paciente
);
GO

-- Entidades ---------------------------------------------------

IF OBJECT_ID('dbo.Pacientes') IS NULL
CREATE TABLE dbo.Pacientes (
    Id              VARCHAR(50)    NOT NULL CONSTRAINT PK_Pacientes PRIMARY KEY,
    NombreCompleto  NVARCHAR(150)  NOT NULL,
    Correo          VARCHAR(150)   NULL,
    Celular         VARCHAR(20)    NULL,
    TipoConvenio    INT            NOT NULL CONSTRAINT FK_Pacientes_TiposConvenio
                                   REFERENCES dbo.TiposConvenio(Id),
    EsPrimeraVez    BIT            NOT NULL CONSTRAINT DF_Pacientes_EsPrimeraVez DEFAULT (1)
);
GO

IF OBJECT_ID('dbo.Odontologos') IS NULL
CREATE TABLE dbo.Odontologos (
    Id              VARCHAR(50)    NOT NULL CONSTRAINT PK_Odontologos PRIMARY KEY,
    Nombre          NVARCHAR(150)  NOT NULL,
    EspecialidadId  INT            NOT NULL CONSTRAINT FK_Odontologos_Especialidades
                                   REFERENCES dbo.Especialidades(Id),
    EstaDisponible  BIT            NOT NULL CONSTRAINT DF_Odontologos_EstaDisponible DEFAULT (1)
);
GO

-- Columnas alineadas con SqlServerEjecutor.cs:
--   INSERT INTO Citas (PacienteId, OdontologoId, Fecha, Copago)
--   UPDATE Citas SET Estado = 'CANCELADA', Penalizacion = @pen WHERE Id = @id
-- Id y Estado tienen DEFAULT porque el INSERT actual no los envía.
IF OBJECT_ID('dbo.Citas') IS NULL
CREATE TABLE dbo.Citas (
    Id            VARCHAR(36)    NOT NULL CONSTRAINT PK_Citas PRIMARY KEY
                                 CONSTRAINT DF_Citas_Id DEFAULT (LEFT(CONVERT(VARCHAR(36), NEWID()), 8)),
    PacienteId    VARCHAR(50)    NOT NULL CONSTRAINT FK_Citas_Pacientes
                                 REFERENCES dbo.Pacientes(Id),
    OdontologoId  VARCHAR(50)    NOT NULL CONSTRAINT FK_Citas_Odontologos
                                 REFERENCES dbo.Odontologos(Id),
    Fecha         DATETIME2(0)   NOT NULL,
    Copago        DECIMAL(10,2)  NOT NULL,
    Estado        VARCHAR(20)    NOT NULL CONSTRAINT DF_Citas_Estado DEFAULT ('PROGRAMADA')
                                 CONSTRAINT CK_Citas_Estado CHECK (Estado IN ('PROGRAMADA', 'CANCELADA')),
    Penalizacion  DECIMAL(10,2)  NOT NULL CONSTRAINT DF_Citas_Penalizacion DEFAULT (0)
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Citas_OdontologoId_Fecha')
    CREATE INDEX IX_Citas_OdontologoId_Fecha ON dbo.Citas (OdontologoId, Fecha);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Citas_PacienteId')
    CREATE INDEX IX_Citas_PacienteId ON dbo.Citas (PacienteId);
GO

-- Datos de catálogo (valores de GestorCitasOdontologicas.cs) ---

MERGE dbo.Especialidades AS t
USING (VALUES
    (1, N'Ortodoncia',      1.20),
    (2, N'Endodoncia',      1.80),
    (3, N'Cirugía',         2.50),
    (4, N'Odontopediatría', 1.10)
) AS s (Id, Nombre, FactorCopago)
ON t.Id = s.Id
WHEN MATCHED THEN UPDATE SET Nombre = s.Nombre, FactorCopago = s.FactorCopago
WHEN NOT MATCHED THEN INSERT (Id, Nombre, FactorCopago) VALUES (s.Id, s.Nombre, s.FactorCopago);
GO

MERGE dbo.TiposConvenio AS t
USING (VALUES
    (1, N'Particular', 1.00),
    (2, N'EPS',        0.30),
    (3, N'Prepagada',  0.10)
) AS s (Id, Nombre, FactorDescuento)
ON t.Id = s.Id
WHEN MATCHED THEN UPDATE SET Nombre = s.Nombre, FactorDescuento = s.FactorDescuento
WHEN NOT MATCHED THEN INSERT (Id, Nombre, FactorDescuento) VALUES (s.Id, s.Nombre, s.FactorDescuento);
GO

-- Datos semilla (los usados por Program.cs) -------------------
-- Citas.PacienteId / OdontologoId son FK: estos registros deben existir
-- antes de ejecutar GuardarCita, o el INSERT falla con FK_Citas_Pacientes.

MERGE dbo.Pacientes AS t
USING (VALUES
    ('PAC-101', N'Ana María Gómez', 'ana.gomez@email.com', '3001234567', 2, 1)
) AS s (Id, NombreCompleto, Correo, Celular, TipoConvenio, EsPrimeraVez)
ON t.Id = s.Id
WHEN MATCHED THEN UPDATE SET NombreCompleto = s.NombreCompleto, Correo = s.Correo,
                             Celular = s.Celular, TipoConvenio = s.TipoConvenio,
                             EsPrimeraVez = s.EsPrimeraVez
WHEN NOT MATCHED THEN INSERT (Id, NombreCompleto, Correo, Celular, TipoConvenio, EsPrimeraVez)
                      VALUES (s.Id, s.NombreCompleto, s.Correo, s.Celular, s.TipoConvenio, s.EsPrimeraVez);
GO

MERGE dbo.Odontologos AS t
USING (VALUES
    ('ODO-202', N'Dr. Roberto Martínez', 3, 1)
) AS s (Id, Nombre, EspecialidadId, EstaDisponible)
ON t.Id = s.Id
WHEN MATCHED THEN UPDATE SET Nombre = s.Nombre, EspecialidadId = s.EspecialidadId,
                             EstaDisponible = s.EstaDisponible
WHEN NOT MATCHED THEN INSERT (Id, Nombre, EspecialidadId, EstaDisponible)
                      VALUES (s.Id, s.Nombre, s.EspecialidadId, s.EstaDisponible);
GO
