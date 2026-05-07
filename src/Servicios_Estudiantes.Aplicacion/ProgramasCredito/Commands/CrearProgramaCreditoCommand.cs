using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.ProgramasCredito.Commands;

public record CrearProgramaCreditoCommand(string Nombre, int CreditosPorMateria, int MaxMateriasPorEstudiante) : IRequest<Result<int>>;

public sealed class CrearProgramaCreditoValidator : AbstractValidator<CrearProgramaCreditoCommand>
{
    public CrearProgramaCreditoValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreditosPorMateria).GreaterThan(0);
        RuleFor(x => x.MaxMateriasPorEstudiante).GreaterThan(0);
    }
}

public sealed class CrearProgramaCreditoHandler : IRequestHandler<CrearProgramaCreditoCommand, Result<int>>
{
    private readonly IProgramaCreditoRepository _repo;

    public CrearProgramaCreditoHandler(IProgramaCreditoRepository repo) => _repo = repo;

    public async Task<Result<int>> Handle(CrearProgramaCreditoCommand request, CancellationToken cancellationToken)
    {
        var id = await _repo.CrearAsync(request.Nombre, request.CreditosPorMateria, request.MaxMateriasPorEstudiante);
        return Result<int>.Success(id);
    }
}
