using Dapper;
using Microsoft.Data.SqlClient;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Infraestructura.Repositorios;

public sealed class ProfesorRepository : IProfesorRepository
{
    private readonly string _connectionString;

    public ProfesorRepository(string connectionString) => _connectionString = connectionString;

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<Profesor>> ObtenerTodosAsync()
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Profesor>(
            "SP_ObtenerProfesores",
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Profesor?> ObtenerPorIdAsync(int id)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Profesor>(
            "SP_ObtenerProfesorPorId",
            new { ProfesorId = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<int> CrearAsync(string nombre)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "SP_CrearProfesor",
            new { Nombre = nombre },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(int id, string nombre)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_ActualizarProfesor",
            new { ProfesorId = id, Nombre = nombre },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
