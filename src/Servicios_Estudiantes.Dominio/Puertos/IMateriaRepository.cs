using Servicios_Estudiantes.Dominio.Entidades;

namespace Servicios_Estudiantes.Dominio.Puertos;

public interface IMateriaRepository
{
    Task<IEnumerable<Materia>> ObtenerTodasAsync();
    Task<Materia?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<Materia>> ObtenerPorIdsAsync(IEnumerable<int> ids);
    Task<int> CrearAsync(string nombre, int creditos, int profesorId, int programaCreditoId);
    Task ActualizarAsync(int id, string nombre, int creditos, int profesorId, int programaCreditoId);
    Task EliminarAsync(int id);
}
