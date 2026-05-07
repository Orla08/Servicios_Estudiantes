using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Auth;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<LoginResponse>>;

public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    private readonly IAuthRepository _authRepo;
    private readonly IJwtService _jwtService;

    public RefreshTokenHandler(IAuthRepository authRepo, IJwtService jwtService)
    {
        _authRepo = authRepo;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _authRepo.ValidarRefreshTokenAsync(request.RefreshToken);

        if (usuario is null)
            return Result<LoginResponse>.Failure("REFRESH_TOKEN_INVALIDO", "El refresh token no es válido o ha expirado.");

        await _authRepo.RevocarRefreshTokenAsync(request.RefreshToken);

        var newAccessToken = _jwtService.GenerarAccessToken(usuario);
        var newRefreshToken = _jwtService.GenerarRefreshToken();
        var expiration = DateTime.UtcNow.AddDays(7);

        await _authRepo.GuardarRefreshTokenAsync(usuario.UsuarioId, newRefreshToken, expiration);

        return Result<LoginResponse>.Success(new LoginResponse(
            newAccessToken,
            newRefreshToken,
            expiration,
            usuario.NombreUsuario,
            usuario.Rol,
            usuario.EstudianteId
        ));
    }
}
