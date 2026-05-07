using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Excepciones;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Usuarios.Commands;

public record ActualizarUsuarioCommand(int Id, string NombreUsuario, string Email, string Rol) : IRequest<Result<bool>>;

public sealed class ActualizarUsuarioValidator : AbstractValidator<ActualizarUsuarioCommand>
{
    public ActualizarUsuarioValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.NombreUsuario).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Rol).NotEmpty().Must(r => r == "Administrador" || r == "Estudiante")
            .WithMessage("El rol debe ser 'Administrador' o 'Estudiante'.");
    }
}

public sealed class ActualizarUsuarioHandler : IRequestHandler<ActualizarUsuarioCommand, Result<bool>>
{
    private readonly IUsuarioRepository _repo;

    public ActualizarUsuarioHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<Result<bool>> Handle(ActualizarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var existe = await _repo.ObtenerPorIdAsync(request.Id);
        if (existe is null)
            throw new RecursoNoEncontradoException("Usuario", request.Id);

        await _repo.ActualizarAsync(request.Id, request.NombreUsuario, request.Email, request.Rol);
        return Result<bool>.Success(true);
    }
}
