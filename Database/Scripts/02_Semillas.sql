-- ============================================================
-- Script 2: Datos semilla
-- Ejecutar en contexto: EstudiantesDB
-- IMPORTANTE: Usar prefijo N'' en todas las cadenas Unicode
-- ============================================================

USE EstudiantesDB;
GO

-- ============================================================
-- Programa de Créditos
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM ProgramaCredito WHERE ProgramaCreditoId = 1)
BEGIN
    INSERT INTO ProgramaCredito (Nombre, CreditosPorMateria, MaxMateriasPorEstudiante)
    VALUES (N'Programa de créditos académicos', 3, 3);
    PRINT 'Programa de crédito insertado.';
END
GO

-- ============================================================
-- Profesores (5)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM Profesor WHERE ProfesorId = 1)
BEGIN
    INSERT INTO Profesor (Nombre) VALUES
        (N'Prof. Andrés Morales'),
        (N'Prof. Daniela Ríos'),
        (N'Prof. Sebastián Núñez'),
        (N'Prof. Valentina Castro'),
        (N'Prof. Felipe Guzmán');
    PRINT '5 profesores insertados.';
END
GO

-- ============================================================
-- Materias (10 — 2 por profesor)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM Materia WHERE MateriaId = 1)
BEGIN
    DECLARE @ProgramaId INT = (SELECT TOP 1 ProgramaCreditoId FROM ProgramaCredito WHERE Estado = 1);
    DECLARE @P1 INT = (SELECT TOP 1 ProfesorId FROM Profesor WHERE ProfesorId = 1);
    DECLARE @P2 INT = (SELECT TOP 1 ProfesorId FROM Profesor WHERE ProfesorId = 2);
    DECLARE @P3 INT = (SELECT TOP 1 ProfesorId FROM Profesor WHERE ProfesorId = 3);
    DECLARE @P4 INT = (SELECT TOP 1 ProfesorId FROM Profesor WHERE ProfesorId = 4);
    DECLARE @P5 INT = (SELECT TOP 1 ProfesorId FROM Profesor WHERE ProfesorId = 5);

    INSERT INTO Materia (Nombre, Creditos, ProfesorId, ProgramaCreditoId) VALUES
        (N'Álgebra Lineal',         3, @P1, @ProgramaId),
        (N'Cálculo I',              3, @P1, @ProgramaId),
        (N'Programación I',         3, @P2, @ProgramaId),
        (N'Estructuras de Datos',   3, @P2, @ProgramaId),
        (N'Bases de Datos',         3, @P3, @ProgramaId),
        (N'Sistemas Operativos',    3, @P3, @ProgramaId),
        (N'Redes',                  3, @P4, @ProgramaId),
        (N'Seguridad Informática',  3, @P4, @ProgramaId),
        (N'Ingeniería de Software', 3, @P5, @ProgramaId),
        (N'Gestión de Proyectos',   3, @P5, @ProgramaId);

    PRINT '10 materias insertadas.';
END
GO

-- ============================================================
-- Usuario Administrador
-- Contraseña: Admin123!
-- Hash: SHA256('Admin123!') en hex minúsculas
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM Usuario WHERE NombreUsuario = N'admin')
BEGIN
    INSERT INTO Usuario (NombreUsuario, Email, PasswordHash, Rol)
    VALUES (
        N'admin',
        N'admin@local.test',
        '3eb3fe66b31e3b4d10fa70b5cad49c7112294af6ae4e476a1c405155d45aa121',
        N'Administrador'
    );
    PRINT 'Usuario administrador insertado. Credenciales: admin / Admin123!';
END
GO

PRINT 'Script 02_Semillas ejecutado correctamente.';
GO
