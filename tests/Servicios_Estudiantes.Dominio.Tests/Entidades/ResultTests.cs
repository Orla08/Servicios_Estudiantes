using FluentAssertions;
using Servicios_Estudiantes.Dominio.Comun;

namespace Servicios_Estudiantes.Dominio.Tests.Entidades;

public class ResultTests
{
    [Fact]
    public void Success_DeberiaCrearResultadoExitoso()
    {
        var result = Result<int>.Success(42);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Failure_DeberiaCrearResultadoFallido()
    {
        var result = Result<int>.Failure("CODE", "Mensaje de error");

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("CODE");
        result.Error.Message.Should().Be("Mensaje de error");
        result.Value.Should().Be(default);
    }

    [Fact]
    public void Success_ConReferencia_DeberiaFuncionar()
    {
        var lista = new List<string> { "a", "b" };
        var result = Result<List<string>>.Success(lista);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(lista);
    }
}
