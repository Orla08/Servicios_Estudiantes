-- ============================================================
-- Script 3: Stored Procedures del dominio académico
-- Ejecutar en contexto: EstudiantesDB
-- ============================================================

USE EstudiantesDB;
GO

-- ============================================================
-- SP_Inscripcion
-- Registra la inscripción de un estudiante (máximo 3 materias en total).
-- Permite agregar materias de forma incremental hasta llegar a 3.
--
-- Parámetros entrada:
--   @EstudianteId  INT           — ID del estudiante
--   @MateriaIds    NVARCHAR(MAX) — IDs separados por coma: '1,3,7'
-- Parámetros salida:
--   @Resultado     INT           — 0=éxito, >0=código de error
--   @Mensaje       NVARCHAR      — descripción del resultado
-- ============================================================
CREATE OR ALTER PROCEDURE SP_Inscripcion
    @EstudianteId INT,
    @MateriaIds   NVARCHAR(MAX),
    @Resultado    INT           OUTPUT,
    @Mensaje      NVARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @Mensaje   = 'Inscripción registrada exitosamente.';

    -- 1. Estudiante existe y está activo
    IF NOT EXISTS (
        SELECT 1 FROM Estudiante
        WHERE EstudianteId = @EstudianteId AND Estado = 1
    )
    BEGIN
        SET @Resultado = 3;
        SET @Mensaje = 'El estudiante no fue encontrado o se encuentra inactivo.';
        RETURN;
    END

    -- 2. Entre 1 y 3 materias enviadas
    DECLARE @CantidadEnviada INT = (
        SELECT COUNT(*)
        FROM STRING_SPLIT(@MateriaIds, ',')
        WHERE LTRIM(RTRIM(value)) <> ''
    );

    IF @CantidadEnviada = 0 OR @CantidadEnviada > 3
    BEGIN
        SET @Resultado = 1;
        SET @Mensaje = 'Debe seleccionar entre 1 y 3 materias.';
        RETURN;
    END

    -- 3. Todas las materias enviadas existen y están activas
    IF (
        SELECT COUNT(*)
        FROM Materia
        WHERE MateriaId IN (
            SELECT CAST(LTRIM(RTRIM(value)) AS INT)
            FROM STRING_SPLIT(@MateriaIds, ',')
            WHERE LTRIM(RTRIM(value)) <> ''
        )
        AND Estado = 1
    ) <> @CantidadEnviada
    BEGIN
        SET @Resultado = 4;
        SET @Mensaje = 'Una o más materias no existen o se encuentran inactivas.';
        RETURN;
    END

    -- 4. El estudiante no está ya inscrito en alguna de las materias enviadas
    IF EXISTS (
        SELECT 1 FROM InscripcionEstudianteMateria
        WHERE EstudianteId = @EstudianteId
          AND Estado = 1
          AND MateriaId IN (
              SELECT CAST(LTRIM(RTRIM(value)) AS INT)
              FROM STRING_SPLIT(@MateriaIds, ',')
              WHERE LTRIM(RTRIM(value)) <> ''
          )
    )
    BEGIN
        SET @Resultado = 6;
        SET @Mensaje = 'Ya está inscrito en una o más de las materias seleccionadas.';
        RETURN;
    END

    -- 5. Total (actuales + nuevas) no supera 3
    DECLARE @InscrActuales INT = (
        SELECT COUNT(*) FROM InscripcionEstudianteMateria
        WHERE EstudianteId = @EstudianteId AND Estado = 1
    );

    IF @InscrActuales + @CantidadEnviada > 3
    BEGIN
        SET @Resultado = 5;
        SET @Mensaje = 'No puede superar el máximo de 3 materias inscritas. Tiene ' + CAST(@InscrActuales AS NVARCHAR) + ' y está intentando agregar ' + CAST(@CantidadEnviada AS NVARCHAR) + '.';
        RETURN;
    END

    -- 6. Sin profesor repetido (nuevas + ya inscritas activas)
    IF EXISTS (
        SELECT ProfesorId
        FROM Materia
        WHERE MateriaId IN (
            SELECT CAST(LTRIM(RTRIM(value)) AS INT)
            FROM STRING_SPLIT(@MateriaIds, ',')
            WHERE LTRIM(RTRIM(value)) <> ''
            UNION ALL
            SELECT MateriaId FROM InscripcionEstudianteMateria
            WHERE EstudianteId = @EstudianteId AND Estado = 1
        )
        AND Estado = 1
        GROUP BY ProfesorId
        HAVING COUNT(*) > 1
    )
    BEGIN
        SET @Resultado = 2;
        SET @Mensaje = 'No puede inscribirse a materias con el mismo profesor en una misma inscripción.';
        RETURN;
    END

    -- 7. Reactivar si existía cancelada, insertar si es nueva
    MERGE InscripcionEstudianteMateria AS destino
    USING (
        SELECT CAST(LTRIM(RTRIM(value)) AS INT) AS MateriaId
        FROM STRING_SPLIT(@MateriaIds, ',')
        WHERE LTRIM(RTRIM(value)) <> ''
    ) AS origen ON destino.EstudianteId = @EstudianteId AND destino.MateriaId = origen.MateriaId
    WHEN MATCHED THEN
        UPDATE SET Estado = 1, FechaRegistro = GETUTCDATE(), FechaModificacion = NULL
    WHEN NOT MATCHED THEN
        INSERT (EstudianteId, MateriaId, FechaRegistro, Estado)
        VALUES (@EstudianteId, origen.MateriaId, GETUTCDATE(), 1);

    RETURN;
END
GO

-- ============================================================
-- SP_ObtenerInscripcion
-- Retorna las materias activas en las que está inscrito un estudiante.
-- ============================================================
CREATE OR ALTER PROCEDURE SP_ObtenerInscripcion
    @EstudianteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        iem.EstudianteId,
        iem.MateriaId,
        m.Nombre       AS NombreMateria,
        p.Nombre       AS NombreProfesor,
        m.Creditos,
        iem.FechaRegistro,
        iem.FechaModificacion,
        iem.Estado
    FROM InscripcionEstudianteMateria iem
    INNER JOIN Materia  m ON m.MateriaId  = iem.MateriaId
    INNER JOIN Profesor p ON p.ProfesorId = m.ProfesorId
    WHERE iem.EstudianteId = @EstudianteId
      AND iem.Estado = 1
    ORDER BY m.Nombre;
END
GO

-- ============================================================
-- SP_ObtenerCompaneros
-- Por cada materia del estudiante, retorna los nombres de sus
-- compañeros (sin email — protección de datos).
-- ============================================================
CREATE OR ALTER PROCEDURE SP_ObtenerCompaneros
    @EstudianteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.Nombre AS NombreMateria,
        e.Nombre AS NombreCompanero
    FROM InscripcionEstudianteMateria iem_yo
    INNER JOIN Materia m
        ON m.MateriaId = iem_yo.MateriaId
    INNER JOIN InscripcionEstudianteMateria iem_comp
        ON  iem_comp.MateriaId    = iem_yo.MateriaId
        AND iem_comp.EstudianteId <> @EstudianteId
        AND iem_comp.Estado       = 1
    INNER JOIN Estudiante e
        ON e.EstudianteId = iem_comp.EstudianteId
    WHERE iem_yo.EstudianteId = @EstudianteId
      AND iem_yo.Estado       = 1
    ORDER BY m.Nombre, e.Nombre;
END
GO

-- ============================================================
-- SP_CancelarInscripcion
-- Baja lógica de todas las inscripciones activas del estudiante.
-- ============================================================
CREATE OR ALTER PROCEDURE SP_CancelarInscripcion
    @EstudianteId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE InscripcionEstudianteMateria
    SET Estado            = 0,
        FechaModificacion = GETUTCDATE()
    WHERE EstudianteId = @EstudianteId
      AND Estado       = 1;
END
GO

-- ============================================================
-- SP_CancelarInscripcionPorMateria
-- Baja lógica de UNA inscripción específica del estudiante.
-- Lanza error con severidad 16 si la inscripción no existe o
-- ya está cancelada (Estado = 0), para que el repositorio
-- pueda convertirlo en RecursoNoEncontradoException → HTTP 404.
-- ============================================================
CREATE OR ALTER PROCEDURE SP_CancelarInscripcionPorMateria
    @EstudianteId INT,
    @MateriaId    INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar que la inscripción existe y está activa
    IF NOT EXISTS (
        SELECT 1 FROM InscripcionEstudianteMateria
        WHERE EstudianteId = @EstudianteId
          AND MateriaId    = @MateriaId
          AND Estado       = 1
    )
    BEGIN
        RAISERROR('La inscripción en la materia indicada no fue encontrada o ya se encuentra cancelada.', 16, 1);
        RETURN;
    END

    UPDATE InscripcionEstudianteMateria
    SET Estado            = 0,
        FechaModificacion = GETUTCDATE()
    WHERE EstudianteId = @EstudianteId
      AND MateriaId    = @MateriaId
      AND Estado       = 1;
END
GO

PRINT 'Script 03_Procedimientos ejecutado correctamente.';
GO
