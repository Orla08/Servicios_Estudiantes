namespace Servicios_Estudiantes.Dominio.Entidades;

public class InscripcionEstudianteMateria
{
    public int EstudianteId { get; private set; }
    public int MateriaId { get; private set; }
    public string NombreMateria { get; private set; } = string.Empty;
    public string NombreProfesor { get; private set; } = string.Empty;
    public int Creditos { get; private set; }
    public DateTime FechaRegistro { get; private set; }
    public DateTime? FechaModificacion { get; private set; }
    public bool Estado { get; private set; }

    private InscripcionEstudianteMateria() { }
}
