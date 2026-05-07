-- ============================================================
-- Script 6: Stored Procedures CRUD para todas las entidades
-- Ejecutar en contexto: EstudiantesDB
-- ============================================================

USE EstudiantesDB;
GO

-- ============================================================
-- ESTUDIANTE
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ObtenerEstudiantes
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.EstudianteId, e.Nombre, e.Email, e.ProgramaCreditoId,
           pc.Nombre AS NombrePrograma, e.UsuarioId,
           e.FechaRegistro, e.FechaModificacion, e.Estado
    FROM Estudiante e
    INNER JOIN ProgramaCredito pc ON pc.ProgramaCreditoId = e.ProgramaCreditoId
    WHERE e.Estado = 1
    ORDER BY e.EstudianteId;
END
GO

CREATE OR ALTER PROCEDURE SP_ObtenerEstudiantePorId
    @EstudianteId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.EstudianteId, e.Nombre, e.Email, e.ProgramaCreditoId,
           pc.Nombre AS NombrePrograma, e.UsuarioId,
           e.FechaRegistro, e.FechaModificacion, e.Estado
    FROM Estudiante e
    INNER JOIN ProgramaCredito pc ON pc.ProgramaCreditoId = e.ProgramaCreditoId
    WHERE e.EstudianteId = @EstudianteId AND e.Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE SP_CrearEstudiante
    @Nombre           NVARCHAR(100),
    @Email            NVARCHAR(100),
    @ProgramaCreditoId INT,
    @UsuarioId        INT NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Estudiante (Nombre, Email, ProgramaCreditoId, UsuarioId, FechaRegistro, Estado)
    VALUES (@Nombre, @Email, @ProgramaCreditoId, @UsuarioId, GETUTCDATE(), 1);
    SELECT SCOPE_IDENTITY() AS EstudianteId;
END
GO

CREATE OR ALTER PROCEDURE SP_ActualizarEstudiante
    @EstudianteId     INT,
    @Nombre           NVARCHAR(100),
    @Email            NVARCHAR(100),
    @ProgramaCreditoId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Estudiante
    SET Nombre = @Nombre, Email = @Email,
        ProgramaCreditoId = @ProgramaCreditoId,
        FechaModificacion = GETUTCDATE()
    WHERE EstudianteId = @EstudianteId;
END
GO

CREATE OR ALTER PROCEDURE SP_EliminarEstudiante
    @EstudianteId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Estudiante
    SET Estado = 0, FechaModificacion = GETUTCDATE()
    WHERE EstudianteId = @EstudianteId;
END
GO

-- ============================================================
-- USUARIO
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ObtenerUsuarios
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UsuarioId, NombreUsuario, Email, Rol,
           FechaRegistro, FechaModificacion, Estado
    FROM Usuario
    WHERE Estado = 1
    ORDER BY UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE SP_ObtenerUsuarioPorId
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UsuarioId, NombreUsuario, Email, Rol,
           FechaRegistro, FechaModificacion, Estado
    FROM Usuario
    WHERE UsuarioId = @UsuarioId AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE SP_CrearUsuario
    @NombreUsuario NVARCHAR(50),
    @Email         NVARCHAR(100),
    @PasswordHash  NVARCHAR(255),
    @Rol           NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Usuario (NombreUsuario, Email, PasswordHash, Rol, FechaRegistro, Estado)
    VALUES (@NombreUsuario, @Email, @PasswordHash, @Rol, GETUTCDATE(), 1);
    SELECT SCOPE_IDENTITY() AS UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE SP_ActualizarUsuario
    @UsuarioId     INT,
    @NombreUsuario NVARCHAR(50),
    @Email         NVARCHAR(100),
    @Rol           NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario
    SET NombreUsuario = @NombreUsuario, Email = @Email,
        Rol = @Rol, FechaModificacion = GETUTCDATE()
    WHERE UsuarioId = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE SP_EliminarUsuario
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario
    SET Estado = 0, FechaModificacion = GETUTCDATE()
    WHERE UsuarioId = @UsuarioId;
END
GO

-- ============================================================
-- MATERIA
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ObtenerMaterias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.MateriaId, m.Nombre, m.Creditos, m.ProfesorId,
           p.Nombre AS NombreProfesor, m.ProgramaCreditoId,
           m.FechaRegistro, m.FechaModificacion, m.Estado
    FROM Materia m
    INNER JOIN Profesor p ON p.ProfesorId = m.ProfesorId
    WHERE m.Estado = 1
    ORDER BY m.MateriaId;
END
GO

CREATE OR ALTER PROCEDURE SP_ObtenerMateriaPorId
    @MateriaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.MateriaId, m.Nombre, m.Creditos, m.ProfesorId,
           p.Nombre AS NombreProfesor, m.ProgramaCreditoId,
           m.FechaRegistro, m.FechaModificacion, m.Estado
    FROM Materia m
    INNER JOIN Profesor p ON p.ProfesorId = m.ProfesorId
    WHERE m.MateriaId = @MateriaId AND m.Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE SP_ObtenerMateriasPorIds
    @MateriaIds NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.MateriaId, m.Nombre, m.Creditos, m.ProfesorId,
           p.Nombre AS NombreProfesor, m.ProgramaCreditoId,
           m.FechaRegistro, m.FechaModificacion, m.Estado
    FROM Materia m
    INNER JOIN Profesor p ON p.ProfesorId = m.ProfesorId
    WHERE m.MateriaId IN (
        SELECT CAST(LTRIM(RTRIM(value)) AS INT)
        FROM STRING_SPLIT(@MateriaIds, ',')
        WHERE LTRIM(RTRIM(value)) <> ''
    )
    AND m.Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE SP_CrearMateria
    @Nombre            NVARCHAR(100),
    @Creditos          INT,
    @ProfesorId        INT,
    @ProgramaCreditoId INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Materia (Nombre, Creditos, ProfesorId, ProgramaCreditoId, FechaRegistro, Estado)
    VALUES (@Nombre, @Creditos, @ProfesorId, @ProgramaCreditoId, GETUTCDATE(), 1);
    SELECT SCOPE_IDENTITY() AS MateriaId;
END
GO

CREATE OR ALTER PROCEDURE SP_ActualizarMateria
    @MateriaId         INT,
    @Nombre            NVARCHAR(100),
    @Creditos          INT,
    @ProfesorId        INT,
    @ProgramaCreditoId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Materia
    SET Nombre = @Nombre, Creditos = @Creditos, ProfesorId = @ProfesorId,
        ProgramaCreditoId = @ProgramaCreditoId, FechaModificacion = GETUTCDATE()
    WHERE MateriaId = @MateriaId;
END
GO

CREATE OR ALTER PROCEDURE SP_EliminarMateria
    @MateriaId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Materia
    SET Estado = 0, FechaModificacion = GETUTCDATE()
    WHERE MateriaId = @MateriaId;
END
GO

-- ============================================================
-- PROFESOR
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ObtenerProfesores
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProfesorId, Nombre, FechaRegistro, FechaModificacion, Estado
    FROM Profesor
    WHERE Estado = 1
    ORDER BY ProfesorId;
END
GO

CREATE OR ALTER PROCEDURE SP_ObtenerProfesorPorId
    @ProfesorId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProfesorId, Nombre, FechaRegistro, FechaModificacion, Estado
    FROM Profesor
    WHERE ProfesorId = @ProfesorId AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE SP_CrearProfesor
    @Nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Profesor (Nombre, FechaRegistro, Estado)
    VALUES (@Nombre, GETUTCDATE(), 1);
    SELECT SCOPE_IDENTITY() AS ProfesorId;
END
GO

CREATE OR ALTER PROCEDURE SP_ActualizarProfesor
    @ProfesorId INT,
    @Nombre     NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Profesor
    SET Nombre = @Nombre, FechaModificacion = GETUTCDATE()
    WHERE ProfesorId = @ProfesorId;
END
GO

-- ============================================================
-- PROGRAMA CREDITO
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ObtenerProgramasCredito
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProgramaCreditoId, Nombre, CreditosPorMateria,
           MaxMateriasPorEstudiante, FechaRegistro, FechaModificacion, Estado
    FROM ProgramaCredito
    WHERE Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE SP_ObtenerProgramaCreditoPorId
    @ProgramaCreditoId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProgramaCreditoId, Nombre, CreditosPorMateria,
           MaxMateriasPorEstudiante, FechaRegistro, FechaModificacion, Estado
    FROM ProgramaCredito
    WHERE ProgramaCreditoId = @ProgramaCreditoId AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE SP_CrearProgramaCredito
    @Nombre                   NVARCHAR(100),
    @CreditosPorMateria       INT,
    @MaxMateriasPorEstudiante INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ProgramaCredito (Nombre, CreditosPorMateria, MaxMateriasPorEstudiante, FechaRegistro, Estado)
    VALUES (@Nombre, @CreditosPorMateria, @MaxMateriasPorEstudiante, GETUTCDATE(), 1);
    SELECT SCOPE_IDENTITY() AS ProgramaCreditoId;
END
GO

-- ============================================================
-- AUTH — Revocar Refresh Token
-- ============================================================

CREATE OR ALTER PROCEDURE SP_RevocarRefreshToken
    @TokenHash NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE RefreshToken
    SET RevokedUtc = GETUTCDATE()
    WHERE TokenHash = @TokenHash;
END
GO

PRINT 'Script 06_SP_CRUD ejecutado correctamente.';
GO
