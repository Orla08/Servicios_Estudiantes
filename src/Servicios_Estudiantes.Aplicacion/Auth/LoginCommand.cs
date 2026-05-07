using MediatR;
using Servicios_Estudiantes.Dominio.Comun;

namespace Servicios_Estudiantes.Aplicacion.Auth;

public record LoginCommand(string NombreUsuario, string Contrasena) : IRequest<Result<LoginResponse>>;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime Expiration,
    string NombreUsuario,
    string Rol,
    int? EstudianteId = null
);
