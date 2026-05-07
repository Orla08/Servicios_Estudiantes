-- ============================================================
-- Script 5: Corregir encoding y actualizar datos semilla
-- Soluciona: INSERT sin prefijo N'' guarda VARCHAR en NVARCHAR
-- Ejecutar en contexto: EstudiantesDB
-- ============================================================

USE EstudiantesDB;
GO

-- ============================================================
-- Corregir nombre del programa de créditos
-- ============================================================
UPDATE ProgramaCredito
SET Nombre = N'Programa de créditos académicos'
WHERE ProgramaCreditoId = 1;
PRINT 'Programa de crédito corregido.';
GO

-- ============================================================
-- Actualizar nombres de profesores
-- ============================================================
UPDATE Profesor SET Nombre = N'Prof. Andrés Morales'   WHERE ProfesorId = 1;
UPDATE Profesor SET Nombre = N'Prof. Daniela Ríos'     WHERE ProfesorId = 2;
UPDATE Profesor SET Nombre = N'Prof. Sebastián Núñez'  WHERE ProfesorId = 3;
UPDATE Profesor SET Nombre = N'Prof. Valentina Castro' WHERE ProfesorId = 4;
UPDATE Profesor SET Nombre = N'Prof. Felipe Guzmán'    WHERE ProfesorId = 5;
PRINT 'Nombres de profesores actualizados.';
GO

-- ============================================================
-- Corregir nombres de materias con caracteres especiales
-- ============================================================
UPDATE Materia SET Nombre = N'Álgebra Lineal'         WHERE MateriaId = 1;
UPDATE Materia SET Nombre = N'Cálculo I'              WHERE MateriaId = 2;
UPDATE Materia SET Nombre = N'Programación I'         WHERE MateriaId = 3;
UPDATE Materia SET Nombre = N'Estructuras de Datos'   WHERE MateriaId = 4;
UPDATE Materia SET Nombre = N'Bases de Datos'         WHERE MateriaId = 5;
UPDATE Materia SET Nombre = N'Sistemas Operativos'    WHERE MateriaId = 6;
UPDATE Materia SET Nombre = N'Redes'                  WHERE MateriaId = 7;
UPDATE Materia SET Nombre = N'Seguridad Informática'  WHERE MateriaId = 8;
UPDATE Materia SET Nombre = N'Ingeniería de Software' WHERE MateriaId = 9;
UPDATE Materia SET Nombre = N'Gestión de Proyectos'   WHERE MateriaId = 10;
PRINT 'Nombres de materias corregidos.';
GO

-- Verificar resultados
SELECT ProgramaCreditoId, Nombre FROM ProgramaCredito;
SELECT ProfesorId, Nombre FROM Profesor ORDER BY ProfesorId;
SELECT MateriaId, Nombre FROM Materia ORDER BY MateriaId;
GO

PRINT 'Script 05_FixEncoding ejecutado correctamente.';
GO
