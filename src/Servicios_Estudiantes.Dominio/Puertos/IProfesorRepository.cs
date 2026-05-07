using Servicios_Estudiantes.Dominio.Entidades;

namespace Servicios_Estudiantes.Dominio.Puertos;

public interface IProfesorRepository
{
    Task<IEnumerable<Profesor>> ObtenerTodosAsync();
    Task<Profesor?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(string nombre);
    Task ActualizarAsync(int id, string nombre);
}
