using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Excepciones;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Materias.Commands;

public record ActualizarMateriaCommand(int Id, string Nombre, int Creditos, int ProfesorId, int ProgramaCreditoId) : IRequest<Result<bool>>;

public sealed class ActualizarMateriaValidator : AbstractValidator<ActualizarMateriaCommand>
{
    public ActualizarMateriaValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Creditos).GreaterThan(0);
        RuleFor(x => x.ProfesorId).GreaterThan(0);
        RuleFor(x => x.ProgramaCreditoId).GreaterThan(0);
    }
}

public sealed class ActualizarMateriaHandler : IRequestHandler<ActualizarMateriaCommand, Result<bool>>
{
    private readonly IMateriaRepository _repo;

    public ActualizarMateriaHandler(IMateriaRepository repo) => _repo = repo;

    public async Task<Result<bool>> Handle(ActualizarMateriaCommand request, CancellationToken cancellationToken)
    {
        var existe = await _repo.ObtenerPorIdAsync(request.Id);
        if (existe is null)
            throw new RecursoNoEncontradoException("Materia", request.Id);

        await _repo.ActualizarAsync(request.Id, request.Nombre, request.Creditos, request.ProfesorId, request.ProgramaCreditoId);
        return Result<bool>.Success(true);
    }
}
