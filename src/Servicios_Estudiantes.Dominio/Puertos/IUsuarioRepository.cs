using Servicios_Estudiantes.Dominio.Entidades;

namespace Servicios_Estudiantes.Dominio.Puertos;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> ObtenerTodosAsync();
    Task<Usuario?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(string nombreUsuario, string email, string passwordHash, string rol);
    Task ActualizarAsync(int id, string nombreUsuario, string email, string rol);
    Task EliminarAsync(int id);
}
