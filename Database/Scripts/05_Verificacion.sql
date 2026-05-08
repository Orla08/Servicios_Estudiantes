-- ============================================================
-- Script 5: Verificación de integridad del esquema
-- Ejecutar en contexto: EstudiantesDB
-- Útil para validar que todos los scripts anteriores
-- se ejecutaron correctamente.
-- ============================================================

USE EstudiantesDB;
GO

PRINT '== Tablas existentes ==';
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
GO

PRINT '== Stored Procedures existentes ==';
SELECT ROUTINE_NAME
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_TYPE = 'PROCEDURE'
ORDER BY ROUTINE_NAME;
GO

PRINT '== Conteo de registros por tabla ==';
SELECT 'ProgramaCredito'           AS Tabla, COUNT(*) AS Total FROM ProgramaCredito
UNION ALL
SELECT 'Profesor',                            COUNT(*) FROM Profesor
UNION ALL
SELECT 'Materia',                             COUNT(*) FROM Materia
UNION ALL
SELECT 'Usuario',                             COUNT(*) FROM Usuario
UNION ALL
SELECT 'Estudiante',                          COUNT(*) FROM Estudiante
UNION ALL
SELECT 'InscripcionEstudianteMateria',        COUNT(*) FROM InscripcionEstudianteMateria
UNION ALL
SELECT 'RefreshToken',                        COUNT(*) FROM RefreshToken;
GO

PRINT '== Datos semilla: Programa de Crédito ==';
SELECT ProgramaCreditoId, Nombre, CreditosPorMateria, MaxMateriasPorEstudiante, Estado
FROM ProgramaCredito;
GO

PRINT '== Datos semilla: Profesores ==';
SELECT ProfesorId, Nombre, Estado FROM Profesor ORDER BY ProfesorId;
GO

PRINT '== Datos semilla: Materias ==';
SELECT m.MateriaId, m.Nombre, m.Creditos, p.Nombre AS Profesor, m.Estado
FROM Materia m
INNER JOIN Profesor p ON p.ProfesorId = m.ProfesorId
ORDER BY m.MateriaId;
GO

PRINT '== Datos semilla: Usuario administrador ==';
SELECT UsuarioId, NombreUsuario, Email, Rol, Estado
FROM Usuario
WHERE Rol = 'Administrador';
GO

PRINT '== Verificación de FK: Materias sin programa válido ==';
SELECT m.MateriaId, m.Nombre
FROM Materia m
LEFT JOIN ProgramaCredito pc ON pc.ProgramaCreditoId = m.ProgramaCreditoId
WHERE pc.ProgramaCreditoId IS NULL;
GO

PRINT '== Verificación de FK: Estudiantes sin programa válido ==';
SELECT e.EstudianteId, e.Nombre
FROM Estudiante e
LEFT JOIN ProgramaCredito pc ON pc.ProgramaCreditoId = e.ProgramaCreditoId
WHERE pc.ProgramaCreditoId IS NULL;
GO

PRINT '== Inscripciones activas ==';
SELECT
    e.Nombre AS Estudiante,
    m.Nombre AS Materia,
    p.Nombre AS Profesor,
    iem.FechaRegistro
FROM InscripcionEstudianteMateria iem
INNER JOIN Estudiante e ON e.EstudianteId = iem.EstudianteId
INNER JOIN Materia    m ON m.MateriaId    = iem.MateriaId
INNER JOIN Profesor   p ON p.ProfesorId   = m.ProfesorId
WHERE iem.Estado = 1
ORDER BY e.Nombre, m.Nombre;
GO

PRINT 'Script 05_Verificacion ejecutado correctamente.';
GO
