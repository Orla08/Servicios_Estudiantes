using Servicios_Estudiantes.Dominio.Entidades;

namespace Servicios_Estudiantes.Dominio.Puertos;

public interface IEstudianteRepository
{
    Task<IEnumerable<Estudiante>> ObtenerTodosAsync();
    Task<Estudiante?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(string nombre, string email, int programaCreditoId, int? usuarioId);
    Task ActualizarAsync(int id, string nombre, string email, int programaCreditoId);
    Task EliminarAsync(int id);
}
