using FluentAssertions;
using FluentValidation.TestHelper;
using Servicios_Estudiantes.Aplicacion.Inscripcion.Commands;

namespace Servicios_Estudiantes.Aplicacion.Tests.Inscripcion;

public class RegistrarInscripcionValidatorTests
{
    private readonly RegistrarInscripcionValidator _validator = new();

    [Fact]
    public void Validar_DeberiaFallar_CuandoEstudianteIdEsCero()
    {
        var command = new RegistrarInscripcionCommand(0, new List<int> { 1, 2, 3 });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EstudianteId);
    }

    [Fact]
    public void Validar_DeberiaFallar_CuandoMateriasExcedeTres()
    {
        var command = new RegistrarInscripcionCommand(1000, new List<int> { 1, 2, 3, 4 });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.MateriaIds);
    }

    [Fact]
    public void Validar_DeberiaFallar_CuandoMateriasSonDuplicadas()
    {
        var command = new RegistrarInscripcionCommand(1000, new List<int> { 1, 1, 3 });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.MateriaIds);
    }

    [Fact]
    public void Validar_DeberiaSerValido_CuandoSeleccionaUnaMateria()
    {
        var command = new RegistrarInscripcionCommand(1000, new List<int> { 1 });
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_DeberiaSerValido_CuandoSeleccionaDosMaterias()
    {
        var command = new RegistrarInscripcionCommand(1000, new List<int> { 1, 3 });
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_DeberiaSerValido_CuandoSeleccionaTresMaterias()
    {
        var command = new RegistrarInscripcionCommand(1000, new List<int> { 1, 3, 5 });
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
