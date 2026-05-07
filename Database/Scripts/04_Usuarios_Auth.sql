-- ============================================================
-- Script 4: Stored Procedures de autenticación y refresh tokens
-- Ejecutar en contexto: EstudiantesDB
-- ============================================================

USE EstudiantesDB;
GO

-- ============================================================
-- SP_Login
-- Valida credenciales y retorna los datos del usuario si son
-- correctas. Retorna vacío si son incorrectas o el usuario
-- está inactivo.
-- ============================================================
CREATE OR ALTER PROCEDURE SP_Login
    @NombreUsuario NVARCHAR(50),
    @PasswordHash  NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.UsuarioId,
        u.NombreUsuario,
        u.Email,
        u.PasswordHash,
        u.Rol,
        u.FechaRegistro,
        u.FechaModificacion,
        u.Estado,
        e.EstudianteId
    FROM Usuario u
    LEFT JOIN Estudiante e ON e.UsuarioId = u.UsuarioId AND e.Estado = 1
    WHERE u.NombreUsuario = @NombreUsuario
      AND u.PasswordHash  = @PasswordHash
      AND u.Estado        = 1;
END
GO

-- ============================================================
-- SP_GuardarRefreshToken
-- Revoca todos los tokens previos del usuario e inserta el nuevo.
-- ============================================================
CREATE OR ALTER PROCEDURE SP_GuardarRefreshToken
    @UsuarioId  INT,
    @TokenHash  NVARCHAR(500),
    @ExpiresUtc DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    -- Revocar tokens anteriores activos del mismo usuario
    UPDATE RefreshToken
    SET RevokedUtc = GETUTCDATE()
    WHERE UsuarioId   = @UsuarioId
      AND RevokedUtc  IS NULL;

    -- Insertar el nuevo token
    INSERT INTO RefreshToken (UsuarioId, TokenHash, ExpiresUtc, CreadoUtc)
    VALUES (@UsuarioId, @TokenHash, @ExpiresUtc, GETUTCDATE());
END
GO

-- ============================================================
-- SP_ValidarRefreshToken
-- Retorna los datos del usuario asociado si el token es válido
-- (no revocado y no expirado).
-- ============================================================
CREATE OR ALTER PROCEDURE SP_ValidarRefreshToken
    @TokenHash NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.UsuarioId,
        u.NombreUsuario,
        u.Email,
        u.PasswordHash,
        u.Rol,
        u.FechaRegistro,
        u.FechaModificacion,
        u.Estado,
        e.EstudianteId
    FROM RefreshToken rt
    INNER JOIN Usuario u ON u.UsuarioId = rt.UsuarioId
    LEFT JOIN Estudiante e ON e.UsuarioId = u.UsuarioId AND e.Estado = 1
    WHERE rt.TokenHash  = @TokenHash
      AND rt.RevokedUtc IS NULL
      AND rt.ExpiresUtc > GETUTCDATE()
      AND u.Estado      = 1;
END
GO

PRINT 'Script 04_Usuarios_Auth ejecutado correctamente.';
GO
