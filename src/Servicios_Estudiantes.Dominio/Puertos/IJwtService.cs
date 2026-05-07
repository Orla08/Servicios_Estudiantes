using Servicios_Estudiantes.Dominio.Entidades;

namespace Servicios_Estudiantes.Dominio.Puertos;

public interface IJwtService
{
    string GenerarAccessToken(Usuario usuario);
    string GenerarRefreshToken();
}
