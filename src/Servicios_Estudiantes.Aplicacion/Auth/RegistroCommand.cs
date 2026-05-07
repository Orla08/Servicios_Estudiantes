using FluentValidation;
using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Auth;

public record RegistroCommand(
    string NombreUsuario,
    string Email,
    string Contrasena,
    string Nombre,
    int ProgramaCreditoId
) : IRequest<Result<int>>;

public sealed class RegistroValidator : AbstractValidator<RegistroCommand>
{
    public RegistroValidator()
    {
        RuleFor(x => x.NombreUsuario).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Contrasena).NotEmpty().MinimumLength(8)
            .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula.")
            .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un número.")
            .Matches(@"[!@#$%^&*]").WithMessage("La contraseña debe contener al menos un carácter especial.");
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ProgramaCreditoId).GreaterThan(0);
    }
}

public sealed class RegistroHandler : IRequestHandler<RegistroCommand, Result<int>>
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IEstudianteRepository _estudianteRepo;
    private readonly IPasswordHashService _hashService;

    public RegistroHandler(IUsuarioRepository usuarioRepo, IEstudianteRepository estudianteRepo, IPasswordHashService hashService)
    {
        _usuarioRepo = usuarioRepo;
        _estudianteRepo = estudianteRepo;
        _hashService = hashService;
    }

    public async Task<Result<int>> Handle(RegistroCommand request, CancellationToken cancellationToken)
    {
        var hash = _hashService.Hash(request.Contrasena);
        var usuarioId = await _usuarioRepo.CrearAsync(request.NombreUsuario, request.Email, hash, "Estudiante");
        var estudianteId = await _estudianteRepo.CrearAsync(request.Nombre, request.Email, request.ProgramaCreditoId, usuarioId);

        return Result<int>.Success(estudianteId);
    }
}
