using Servicios_Estudiantes.Dominio.Entidades;

namespace Servicios_Estudiantes.Dominio.Puertos;

public interface IAuthRepository
{
    Task<Usuario?> LoginAsync(string nombreUsuario, string passwordHash);
    Task GuardarRefreshTokenAsync(int usuarioId, string tokenHash, DateTime expiresUtc);
    Task<Usuario?> ValidarRefreshTokenAsync(string tokenHash);
    Task RevocarRefreshTokenAsync(string tokenHash);
}
