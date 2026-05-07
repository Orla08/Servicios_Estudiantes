using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Auth;

public sealed class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IAuthRepository _authRepo;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHashService _hashService;

    public LoginHandler(IAuthRepository authRepo, IJwtService jwtService, IPasswordHashService hashService)
    {
        _authRepo = authRepo;
        _jwtService = jwtService;
        _hashService = hashService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var hash = _hashService.Hash(request.Contrasena);
        var usuario = await _authRepo.LoginAsync(request.NombreUsuario, hash);

        if (usuario is null)
            return Result<LoginResponse>.Failure("CREDENCIALES_INVALIDAS", "Usuario o contraseña incorrectos.");

        var accessToken = _jwtService.GenerarAccessToken(usuario);
        var refreshToken = _jwtService.GenerarRefreshToken();
        var expiration = DateTime.UtcNow.AddDays(7);

        await _authRepo.GuardarRefreshTokenAsync(usuario.UsuarioId, refreshToken, expiration);

        return Result<LoginResponse>.Success(new LoginResponse(
            accessToken,
            refreshToken,
            expiration,
            usuario.NombreUsuario,
            usuario.Rol,
            usuario.EstudianteId
        ));
    }
}
