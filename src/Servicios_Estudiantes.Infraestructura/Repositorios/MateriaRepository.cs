using Dapper;
using Microsoft.Data.SqlClient;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Infraestructura.Repositorios;

public sealed class MateriaRepository : IMateriaRepository
{
    private readonly string _connectionString;

    public MateriaRepository(string connectionString) => _connectionString = connectionString;

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<Materia>> ObtenerTodasAsync()
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Materia>(
            "SP_ObtenerMaterias",
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Materia?> ObtenerPorIdAsync(int id)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Materia>(
            "SP_ObtenerMateriaPorId",
            new { MateriaId = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Materia>> ObtenerPorIdsAsync(IEnumerable<int> ids)
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Materia>(
            "SP_ObtenerMateriasPorIds",
            new { MateriaIds = string.Join(",", ids) },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<int> CrearAsync(string nombre, int creditos, int profesorId, int programaCreditoId)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "SP_CrearMateria",
            new { Nombre = nombre, Creditos = creditos, ProfesorId = profesorId, ProgramaCreditoId = programaCreditoId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(int id, string nombre, int creditos, int profesorId, int programaCreditoId)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_ActualizarMateria",
            new { MateriaId = id, Nombre = nombre, Creditos = creditos, ProfesorId = profesorId, ProgramaCreditoId = programaCreditoId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task EliminarAsync(int id)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_EliminarMateria",
            new { MateriaId = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
