using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Excepciones;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Profesores.Commands;

public record ActualizarProfesorCommand(int Id, string Nombre) : IRequest<Result<bool>>;

public sealed class ActualizarProfesorValidator : AbstractValidator<ActualizarProfesorCommand>
{
    public ActualizarProfesorValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
    }
}

public sealed class ActualizarProfesorHandler : IRequestHandler<ActualizarProfesorCommand, Result<bool>>
{
    private readonly IProfesorRepository _repo;

    public ActualizarProfesorHandler(IProfesorRepository repo) => _repo = repo;

    public async Task<Result<bool>> Handle(ActualizarProfesorCommand request, CancellationToken cancellationToken)
    {
        var existe = await _repo.ObtenerPorIdAsync(request.Id);
        if (existe is null)
            throw new RecursoNoEncontradoException("Profesor", request.Id);

        await _repo.ActualizarAsync(request.Id, request.Nombre);
        return Result<bool>.Success(true);
    }
}
