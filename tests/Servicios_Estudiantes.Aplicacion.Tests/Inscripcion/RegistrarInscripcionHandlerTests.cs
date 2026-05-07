using FluentAssertions;
using Moq;
using Servicios_Estudiantes.Aplicacion.Inscripcion.Commands;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Tests.Inscripcion;

public class RegistrarInscripcionHandlerTests
{
    private readonly Mock<IInscripcionRepository> _inscripcionRepoMock = new();
    private readonly Mock<IMateriaRepository> _materiaRepoMock = new();

    private RegistrarInscripcionHandler CrearHandler() =>
        new(_inscripcionRepoMock.Object, _materiaRepoMock.Object);

    private static List<Materia> MateriasConProfesorRepetido()
    {
        // Materias 1 y 2 con el mismo ProfesorId
        var lista = new List<Materia>();
        var type = typeof(Materia);

        var m1 = (Materia)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
        SetPrivate(m1, "MateriaId", 1);
        SetPrivate(m1, "Nombre", "Álgebra Lineal");
        SetPrivate(m1, "ProfesorId", 1);
        SetPrivate(m1, "NombreProfesor", "Prof. Ana García");

        var m2 = (Materia)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
        SetPrivate(m2, "MateriaId", 2);
        SetPrivate(m2, "Nombre", "Cálculo I");
        SetPrivate(m2, "ProfesorId", 1);
        SetPrivate(m2, "NombreProfesor", "Prof. Ana García");

        var m3 = (Materia)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
        SetPrivate(m3, "MateriaId", 3);
        SetPrivate(m3, "Nombre", "Programación I");
        SetPrivate(m3, "ProfesorId", 2);
        SetPrivate(m3, "NombreProfesor", "Prof. Luis Martínez");

        lista.AddRange([m1, m2, m3]);
        return lista;
    }

    private static List<Materia> MateriasValidas()
    {
        var lista = new List<Materia>();
        var type = typeof(Materia);

        for (int i = 1; i <= 3; i++)
        {
            var m = (Materia)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
            SetPrivate(m, "MateriaId", i);
            SetPrivate(m, "Nombre", $"Materia {i}");
            SetPrivate(m, "ProfesorId", i);
            SetPrivate(m, "NombreProfesor", $"Profesor {i}");
            lista.Add(m);
        }
        return lista;
    }

    private static void SetPrivate(object obj, string propName, object value)
    {
        var prop = obj.GetType().GetProperty(propName)!;
        prop.SetValue(obj, value);
    }

    [Fact]
    public async Task Handle_DeberiaFallar_CuandoSeRepiteProfesor()
    {
        // Arrange
        var command = new RegistrarInscripcionCommand(1000, new List<int> { 1, 2, 3 });
        _materiaRepoMock
            .Setup(m => m.ObtenerPorIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(MateriasConProfesorRepetido());

        var handler = CrearHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("PROFESOR_DUPLICADO");
        result.Error.Message.Should().Contain("Prof. Ana García");
    }

    [Fact]
    public async Task Handle_DeberiaFallar_CuandoMateriaNoExiste()
    {
        // Arrange
        var command = new RegistrarInscripcionCommand(1000, new List<int> { 1, 2, 99 });
        _materiaRepoMock
            .Setup(m => m.ObtenerPorIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new List<Materia>()); // vacío = materias no encontradas

        var handler = CrearHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("MATERIAS_NO_ENCONTRADAS");
    }

    [Fact]
    public async Task Handle_DeberiaEjecutarSP_CuandoDatosValidos()
    {
        // Arrange
        var command = new RegistrarInscripcionCommand(1000, new List<int> { 1, 3, 5 });
        _materiaRepoMock
            .Setup(m => m.ObtenerPorIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(MateriasValidas());
        _inscripcionRepoMock
            .Setup(r => r.RegistrarInscripcionAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync((0, "Inscripción registrada exitosamente."));

        var handler = CrearHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _inscripcionRepoMock.Verify(r =>
            r.RegistrarInscripcionAsync(1000, It.IsAny<IEnumerable<int>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeberiaFallar_CuandoSPRetornaError()
    {
        // Arrange
        var command = new RegistrarInscripcionCommand(1000, new List<int> { 1, 3, 5 });
        _materiaRepoMock
            .Setup(m => m.ObtenerPorIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(MateriasValidas());
        _inscripcionRepoMock
            .Setup(r => r.RegistrarInscripcionAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync((5, "El estudiante ya tiene una inscripción activa."));

        var handler = CrearHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("INSCRIPCION_ERROR");
    }
}
