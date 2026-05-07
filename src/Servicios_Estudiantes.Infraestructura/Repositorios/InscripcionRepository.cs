using Dapper;
using Microsoft.Data.SqlClient;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Excepciones;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Infraestructura.Repositorios;

public sealed class InscripcionRepository : IInscripcionRepository
{
    private readonly string _connectionString;

    public InscripcionRepository(string connectionString) => _connectionString = connectionString;

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<(int resultado, string mensaje)> RegistrarInscripcionAsync(int estudianteId, IEnumerable<int> materiaIds)
    {
        var materiaIdsStr = string.Join(",", materiaIds);
        using var conn = CreateConnection();

        var parametros = new DynamicParameters();
        parametros.Add("@EstudianteId", estudianteId);
        parametros.Add("@MateriaIds", materiaIdsStr);
        parametros.Add("@Resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
        parametros.Add("@Mensaje", dbType: System.Data.DbType.String, size: 500, direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync("SP_Inscripcion", parametros, commandType: System.Data.CommandType.StoredProcedure);

        return (parametros.Get<int>("@Resultado"), parametros.Get<string>("@Mensaje") ?? string.Empty);
    }

    public async Task<IEnumerable<InscripcionEstudianteMateria>> ObtenerInscripcionAsync(int estudianteId)
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<InscripcionEstudianteMateria>(
            "SP_ObtenerInscripcion",
            new { EstudianteId = estudianteId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<CompaneroMateria>> ObtenerCompanerosAsync(int estudianteId)
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<CompaneroMateria>(
            "SP_ObtenerCompaneros",
            new { EstudianteId = estudianteId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task CancelarInscripcionAsync(int estudianteId)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "SP_CancelarInscripcion",
            new { EstudianteId = estudianteId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task CancelarInscripcionPorMateriaAsync(int estudianteId, int materiaId)
    {
        using var conn = CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                "SP_CancelarInscripcionPorMateria",
                new { EstudianteId = estudianteId, MateriaId = materiaId },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
        catch (SqlException ex) when (ex.Class >= 16)
        {
            throw new RecursoNoEncontradoException("Inscripción", materiaId);
        }
    }
}
