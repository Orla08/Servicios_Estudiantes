namespace Servicios_Estudiantes.Dominio.Entidades;

public class Profesor
{
    public int ProfesorId { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public DateTime FechaRegistro { get; private set; }
    public DateTime? FechaModificacion { get; private set; }
    public bool Estado { get; private set; }

    private Profesor() { }
}
