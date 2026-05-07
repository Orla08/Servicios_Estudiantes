-- ============================================================
-- Script 1: Crear tablas
-- Ejecutar en contexto: EstudiantesDB
-- ============================================================

USE EstudiantesDB;
GO

-- ============================================================
-- SEGURIDAD
-- ============================================================

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuario')
BEGIN
    CREATE TABLE Usuario (
        UsuarioId         INT IDENTITY(1,1)  PRIMARY KEY,
        NombreUsuario     NVARCHAR(50)       NOT NULL UNIQUE,
        Email             NVARCHAR(100)      NOT NULL UNIQUE,
        PasswordHash      NVARCHAR(255)      NOT NULL,
        Rol               NVARCHAR(20)       NOT NULL DEFAULT 'Estudiante'
                          CONSTRAINT CK_Usuario_Rol CHECK (Rol IN ('Administrador', 'Estudiante')),
        FechaRegistro     DATETIME2          NOT NULL DEFAULT GETUTCDATE(),
        FechaModificacion DATETIME2          NULL,
        Estado            BIT                NOT NULL DEFAULT 1
    );
    PRINT 'Tabla Usuario creada.';
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RefreshToken')
BEGIN
    CREATE TABLE RefreshToken (
        RefreshTokenId INT IDENTITY(1,1) PRIMARY KEY,
        UsuarioId      INT            NOT NULL REFERENCES Usuario(UsuarioId),
        TokenHash      NVARCHAR(500)  NOT NULL UNIQUE,
        ExpiresUtc     DATETIME2      NOT NULL,
        RevokedUtc     DATETIME2      NULL,
        CreadoUtc      DATETIME2      NOT NULL DEFAULT GETUTCDATE()
    );
    PRINT 'Tabla RefreshToken creada.';
END
GO

-- ============================================================
-- ACADÉMICO
-- ============================================================

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProgramaCredito')
BEGIN
    CREATE TABLE ProgramaCredito (
        ProgramaCreditoId        INT IDENTITY(1,1) PRIMARY KEY,
        Nombre                   NVARCHAR(100) NOT NULL,
        CreditosPorMateria       INT           NOT NULL DEFAULT 3,
        MaxMateriasPorEstudiante INT           NOT NULL DEFAULT 3,
        FechaRegistro            DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
        FechaModificacion        DATETIME2     NULL,
        Estado                   BIT           NOT NULL DEFAULT 1
    );
    PRINT 'Tabla ProgramaCredito creada.';
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Profesor')
BEGIN
    CREATE TABLE Profesor (
        ProfesorId        INT IDENTITY(1,1) PRIMARY KEY,
        Nombre            NVARCHAR(100) NOT NULL,
        FechaRegistro     DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
        FechaModificacion DATETIME2     NULL,
        Estado            BIT           NOT NULL DEFAULT 1
    );
    PRINT 'Tabla Profesor creada.';
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Materia')
BEGIN
    CREATE TABLE Materia (
        MateriaId         INT IDENTITY(1,1) PRIMARY KEY,
        Nombre            NVARCHAR(100) NOT NULL,
        Creditos          INT           NOT NULL DEFAULT 3,
        ProfesorId        INT           NOT NULL REFERENCES Profesor(ProfesorId),
        ProgramaCreditoId INT           NOT NULL REFERENCES ProgramaCredito(ProgramaCreditoId),
        FechaRegistro     DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
        FechaModificacion DATETIME2     NULL,
        Estado            BIT           NOT NULL DEFAULT 1
    );
    PRINT 'Tabla Materia creada.';
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Estudiante')
BEGIN
    CREATE TABLE Estudiante (
        EstudianteId      INT IDENTITY(1000,1) PRIMARY KEY,
        Nombre            NVARCHAR(100) NOT NULL,
        Email             NVARCHAR(100) NOT NULL UNIQUE,
        ProgramaCreditoId INT           NOT NULL REFERENCES ProgramaCredito(ProgramaCreditoId),
        UsuarioId         INT           NULL     UNIQUE REFERENCES Usuario(UsuarioId),
        FechaRegistro     DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
        FechaModificacion DATETIME2     NULL,
        Estado            BIT           NOT NULL DEFAULT 1
    );
    PRINT 'Tabla Estudiante creada.';
END
GO

-- ============================================================
-- TRANSACCIONAL
-- ============================================================

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'InscripcionEstudianteMateria')
BEGIN
    CREATE TABLE InscripcionEstudianteMateria (
        EstudianteId      INT       NOT NULL REFERENCES Estudiante(EstudianteId),
        MateriaId         INT       NOT NULL REFERENCES Materia(MateriaId),
        FechaRegistro     DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        FechaModificacion DATETIME2 NULL,
        Estado            BIT       NOT NULL DEFAULT 1,
        CONSTRAINT PK_Inscripcion PRIMARY KEY (EstudianteId, MateriaId)
    );
    PRINT 'Tabla InscripcionEstudianteMateria creada.';
END
GO

-- ============================================================
-- ÍNDICES DE RENDIMIENTO
-- ============================================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Inscripcion_EstudianteId')
    CREATE INDEX IX_Inscripcion_EstudianteId ON InscripcionEstudianteMateria(EstudianteId, Estado);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Materia_ProfesorId')
    CREATE INDEX IX_Materia_ProfesorId ON Materia(ProfesorId, Estado);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RefreshToken_UsuarioId')
    CREATE INDEX IX_RefreshToken_UsuarioId ON RefreshToken(UsuarioId, RevokedUtc, ExpiresUtc);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Estudiante_Email')
    CREATE INDEX IX_Estudiante_Email ON Estudiante(Email, Estado);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Usuario_NombreUsuario')
    CREATE INDEX IX_Usuario_NombreUsuario ON Usuario(NombreUsuario, Estado);
GO

PRINT 'Script 01_Tablas ejecutado correctamente.';
GO
