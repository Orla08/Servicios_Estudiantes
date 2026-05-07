using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Excepciones;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Estudiantes.Commands;

public record ActualizarEstudianteCommand(int Id, string Nombre, string Email, int ProgramaCreditoId) : IRequest<Result<bool>>;

public sealed class ActualizarEstudianteValidator : AbstractValidator<ActualizarEstudianteCommand>
{
    public ActualizarEstudianteValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.ProgramaCreditoId).GreaterThan(0);
    }
}

public sealed class ActualizarEstudianteHandler : IRequestHandler<ActualizarEstudianteCommand, Result<bool>>
{
    private readonly IEstudianteRepository _repo;

    public ActualizarEstudianteHandler(IEstudianteRepository repo) => _repo = repo;

    public async Task<Result<bool>> Handle(ActualizarEstudianteCommand request, CancellationToken cancellationToken)
    {
        var existe = await _repo.ObtenerPorIdAsync(request.Id);
        if (existe is null)
            throw new RecursoNoEncontradoException("Estudiante", request.Id);

        await _repo.ActualizarAsync(request.Id, request.Nombre, request.Email, request.ProgramaCreditoId);
        return Result<bool>.Success(true);
    }
}
