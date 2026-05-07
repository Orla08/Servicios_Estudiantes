-- ============================================================
-- Script 0: Crear base de datos EstudiantesDB
-- Ejecutar en contexto: master
-- ============================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EstudiantesDB')
BEGIN
    CREATE DATABASE EstudiantesDB
        COLLATE SQL_Latin1_General_CP1_CI_AS;
    PRINT 'Base de datos EstudiantesDB creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos EstudiantesDB ya existe.';
END
GO
