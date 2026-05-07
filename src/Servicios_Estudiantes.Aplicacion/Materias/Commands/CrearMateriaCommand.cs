using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Materias.Commands;

public record CrearMateriaCommand(string Nombre, int Creditos, int ProfesorId, int ProgramaCreditoId) : IRequest<Result<int>>;

public sealed class CrearMateriaValidator : AbstractValidator<CrearMateriaCommand>
{
    public CrearMateriaValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Creditos).GreaterThan(0);
        RuleFor(x => x.ProfesorId).GreaterThan(0);
        RuleFor(x => x.ProgramaCreditoId).GreaterThan(0);
    }
}

public sealed class CrearMateriaHandler : IRequestHandler<CrearMateriaCommand, Result<int>>
{
    private readonly IMateriaRepository _repo;

    public CrearMateriaHandler(IMateriaRepository repo) => _repo = repo;

    public async Task<Result<int>> Handle(CrearMateriaCommand request, CancellationToken cancellationToken)
    {
        var id = await _repo.CrearAsync(request.Nombre, request.Creditos, request.ProfesorId, request.ProgramaCreditoId);
        return Result<int>.Success(id);
    }
}
