using Dapper;
using Microsoft.Data.SqlClient;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Infraestructura.Repositorios;

public sealed class ProgramaCreditoRepository : IProgramaCreditoRepository
{
    private readonly string _connectionString;

    public ProgramaCreditoRepository(string connectionString) => _connectionString = connectionString;

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<ProgramaCredito>> ObtenerTodosAsync()
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<ProgramaCredito>(
            "SP_ObtenerProgramasCredito",
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<ProgramaCredito?> ObtenerPorIdAsync(int id)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<ProgramaCredito>(
            "SP_ObtenerProgramaCreditoPorId",
            new { ProgramaCreditoId = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<int> CrearAsync(string nombre, int creditosPorMateria, int maxMateriasPorEstudiante)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "SP_CrearProgramaCredito",
            new { Nombre = nombre, CreditosPorMateria = creditosPorMateria, MaxMateriasPorEstudiante = maxMateriasPorEstudiante },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
