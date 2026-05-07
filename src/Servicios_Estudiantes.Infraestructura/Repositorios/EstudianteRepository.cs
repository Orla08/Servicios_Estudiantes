using Dapper;
using Microsoft.Data.SqlClient;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Infraestructura.Repositorios;

public sealed class EstudianteRepository : IEstudianteRepository
{
    private readonly string _connectionString;

    public EstudianteRepository(string connectionString) => _connectionString = connectionString;

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<Estudiante>> ObtenerTodosAsync()
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Estudiante>(
            "SP_ObtenerEstudiantes",
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Estudiante?> ObtenerPorIdAsync(int id)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Estudiante>(
            "SP_ObtenerEstudiantePorId",
            new { EstudianteId = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<int> CrearAsync(string nombre, string email, int programaCreditoId, int? usuarioId)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "SP_CrearEstudiante",
            new { Nombre = nombre, Email = email, ProgramaCreditoId = programaCreditoId, UsuarioId = usuarioId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(int id, string nombre, string email, int programaCreditoId)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_ActualizarEstudiante",
            new { EstudianteId = id, Nombre = nombre, Email = email, ProgramaCreditoId = programaCreditoId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task EliminarAsync(int id)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_EliminarEstudiante",
            new { EstudianteId = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
