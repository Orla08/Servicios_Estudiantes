using Dapper;
using Microsoft.Data.SqlClient;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Infraestructura.Repositorios;

public sealed class AuthRepository : IAuthRepository
{
    private readonly string _connectionString;

    public AuthRepository(string connectionString) => _connectionString = connectionString;

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<Usuario?> LoginAsync(string nombreUsuario, string passwordHash)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Usuario>(
            "SP_Login",
            new { NombreUsuario = nombreUsuario, PasswordHash = passwordHash },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task GuardarRefreshTokenAsync(int usuarioId, string tokenHash, DateTime expiresUtc)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_GuardarRefreshToken",
            new { UsuarioId = usuarioId, TokenHash = tokenHash, ExpiresUtc = expiresUtc },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Usuario?> ValidarRefreshTokenAsync(string tokenHash)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Usuario>(
            "SP_ValidarRefreshToken",
            new { TokenHash = tokenHash },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task RevocarRefreshTokenAsync(string tokenHash)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_RevocarRefreshToken",
            new { TokenHash = tokenHash },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
