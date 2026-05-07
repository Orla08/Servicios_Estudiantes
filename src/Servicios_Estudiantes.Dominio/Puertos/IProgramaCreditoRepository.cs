using Servicios_Estudiantes.Dominio.Entidades;

namespace Servicios_Estudiantes.Dominio.Puertos;

public interface IProgramaCreditoRepository
{
    Task<IEnumerable<ProgramaCredito>> ObtenerTodosAsync();
    Task<ProgramaCredito?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(string nombre, int creditosPorMateria, int maxMateriasPorEstudiante);
}
