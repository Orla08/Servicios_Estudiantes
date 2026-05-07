namespace Servicios_Estudiantes.Dominio.Entidades;

public class Materia
{
    public int MateriaId { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public int Creditos { get; private set; }
    public int ProfesorId { get; private set; }
    public string NombreProfesor { get; private set; } = string.Empty;
    public int ProgramaCreditoId { get; private set; }
    public DateTime FechaRegistro { get; private set; }
    public DateTime? FechaModificacion { get; private set; }
    public bool Estado { get; private set; }

    private Materia() { }
}
