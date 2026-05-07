using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Inscripcion.Commands;

public record RegistrarInscripcionCommand(int EstudianteId, List<int> MateriaIds) : IRequest<Result<bool>>;

public sealed class RegistrarInscripcionValidator : AbstractValidator<RegistrarInscripcionCommand>
{
    public RegistrarInscripcionValidator()
    {
        RuleFor(x => x.EstudianteId).GreaterThan(0);
        RuleFor(x => x.MateriaIds)
            .NotEmpty()
            .Must(ids => ids.Count <= 3).WithMessage("No puede seleccionar más de 3 materias.")
            .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("No se pueden repetir materias.");
    }
}

public sealed class RegistrarInscripcionHandler : IRequestHandler<RegistrarInscripcionCommand, Result<bool>>
{
    private readonly IInscripcionRepository _inscripcionRepo;
    private readonly IMateriaRepository _materiaRepo;

    public RegistrarInscripcionHandler(IInscripcionRepository inscripcionRepo, IMateriaRepository materiaRepo)
    {
        _inscripcionRepo = inscripcionRepo;
        _materiaRepo = materiaRepo;
    }

    public async Task<Result<bool>> Handle(RegistrarInscripcionCommand request, CancellationToken cancellationToken)
    {
        var materias = (await _materiaRepo.ObtenerPorIdsAsync(request.MateriaIds)).ToList();

        if (materias.Count != request.MateriaIds.Count)
            return Result<bool>.Failure("MATERIAS_NO_ENCONTRADAS", "Una o más materias no existen o están inactivas.");

        var profesorDuplicado = materias
            .GroupBy(m => m.ProfesorId)
            .FirstOrDefault(g => g.Count() > 1);

        if (profesorDuplicado is not null)
        {
            var materiasConflicto = profesorDuplicado.ToList();
            return Result<bool>.Failure(
                "PROFESOR_DUPLICADO",
                $"No es posible inscribir materias con el mismo profesor. Las materias '{materiasConflicto[0].Nombre}' y '{materiasConflicto[1].Nombre}' son dictadas por {materiasConflicto[0].NombreProfesor}."
            );
        }

        var (resultado, mensaje) = await _inscripcionRepo.RegistrarInscripcionAsync(request.EstudianteId, request.MateriaIds);

        return resultado == 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure("INSCRIPCION_ERROR", mensaje);
    }
}
