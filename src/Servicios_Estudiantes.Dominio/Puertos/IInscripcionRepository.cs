using Servicios_Estudiantes.Dominio.Entidades;

namespace Servicios_Estudiantes.Dominio.Puertos;

public interface IInscripcionRepository
{
    Task<(int resultado, string mensaje)> RegistrarInscripcionAsync(int estudianteId, IEnumerable<int> materiaIds);
    Task<IEnumerable<InscripcionEstudianteMateria>> ObtenerInscripcionAsync(int estudianteId);
    Task<IEnumerable<CompaneroMateria>> ObtenerCompanerosAsync(int estudianteId);
    Task CancelarInscripcionAsync(int estudianteId);
    Task CancelarInscripcionPorMateriaAsync(int estudianteId, int materiaId);
}

public record CompaneroMateria(string NombreMateria, string NombreCompanero);
