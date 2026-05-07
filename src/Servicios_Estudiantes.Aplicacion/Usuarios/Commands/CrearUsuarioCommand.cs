using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Usuarios.Commands;

public record CrearUsuarioCommand(string NombreUsuario, string Email, string Contrasena, string Rol) : IRequest<Result<int>>;

public sealed class CrearUsuarioValidator : AbstractValidator<CrearUsuarioCommand>
{
    public CrearUsuarioValidator()
    {
        RuleFor(x => x.NombreUsuario).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Contrasena).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Rol).NotEmpty().Must(r => r == "Administrador" || r == "Estudiante")
            .WithMessage("El rol debe ser 'Administrador' o 'Estudiante'.");
    }
}

public sealed class CrearUsuarioHandler : IRequestHandler<CrearUsuarioCommand, Result<int>>
{
    private readonly IUsuarioRepository _repo;
    private readonly IPasswordHashService _hashService;

    public CrearUsuarioHandler(IUsuarioRepository repo, IPasswordHashService hashService)
    {
        _repo = repo;
        _hashService = hashService;
    }

    public async Task<Result<int>> Handle(CrearUsuarioCommand request, CancellationToken cancellationToken)
    {
        var hash = _hashService.Hash(request.Contrasena);
        var id = await _repo.CrearAsync(request.NombreUsuario, request.Email, hash, request.Rol);
        return Result<int>.Success(id);
    }
}
