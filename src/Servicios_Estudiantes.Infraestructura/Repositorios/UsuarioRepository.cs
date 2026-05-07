using Dapper;
using Microsoft.Data.SqlClient;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Infraestructura.Repositorios;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public UsuarioRepository(string connectionString) => _connectionString = connectionString;

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Usuario>(
            "SP_ObtenerUsuarios",
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Usuario>(
            "SP_ObtenerUsuarioPorId",
            new { UsuarioId = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<int> CrearAsync(string nombreUsuario, string email, string passwordHash, string rol)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "SP_CrearUsuario",
            new { NombreUsuario = nombreUsuario, Email = email, PasswordHash = passwordHash, Rol = rol },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(int id, string nombreUsuario, string email, string rol)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_ActualizarUsuario",
            new { UsuarioId = id, NombreUsuario = nombreUsuario, Email = email, Rol = rol },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task EliminarAsync(int id)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_EliminarUsuario",
            new { UsuarioId = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
