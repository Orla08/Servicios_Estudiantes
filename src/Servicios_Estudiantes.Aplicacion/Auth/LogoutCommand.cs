using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Auth;

public record LogoutCommand(string RefreshToken) : IRequest<Result<bool>>;

public sealed class LogoutHandler : IRequestHandler<LogoutCommand, Result<bool>>
{
    private readonly IAuthRepository _authRepo;

    public LogoutHandler(IAuthRepository authRepo) => _authRepo = authRepo;

    public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _authRepo.RevocarRefreshTokenAsync(request.RefreshToken);
        return Result<bool>.Success(true);
    }
}
