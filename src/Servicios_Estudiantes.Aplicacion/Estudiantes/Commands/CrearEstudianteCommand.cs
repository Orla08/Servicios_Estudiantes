using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Estudiantes.Commands;

public record CrearEstudianteCommand(string Nombre, string Email, int ProgramaCreditoId) : IRequest<Result<int>>;

public sealed class CrearEstudianteValidator : AbstractValidator<CrearEstudianteCommand>
{
    public CrearEstudianteValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.ProgramaCreditoId).GreaterThan(0);
    }
}

public sealed class CrearEstudianteHandler : IRequestHandler<CrearEstudianteCommand, Result<int>>
{
    private readonly IEstudianteRepository _repo;

    public CrearEstudianteHandler(IEstudianteRepository repo) => _repo = repo;

    public async Task<Result<int>> Handle(CrearEstudianteCommand request, CancellationToken cancellationToken)
    {
        var id = await _repo.CrearAsync(request.Nombre, request.Email, request.ProgramaCreditoId, null);
        return Result<int>.Success(id);
    }
}
