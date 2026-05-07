using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Profesores.Commands;

public record CrearProfesorCommand(string Nombre) : IRequest<Result<int>>;

public sealed class CrearProfesorValidator : AbstractValidator<CrearProfesorCommand>
{
    public CrearProfesorValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
    }
}

public sealed class CrearProfesorHandler : IRequestHandler<CrearProfesorCommand, Result<int>>
{
    private readonly IProfesorRepository _repo;

    public CrearProfesorHandler(IProfesorRepository repo) => _repo = repo;

    public async Task<Result<int>> Handle(CrearProfesorCommand request, CancellationToken cancellationToken)
    {
        var id = await _repo.CrearAsync(request.Nombre);
        return Result<int>.Success(id);
    }
}
