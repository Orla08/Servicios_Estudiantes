namespace Servicios_Estudiantes.Dominio.Entidades;

public class Estudiante
{
    public int EstudianteId { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public int ProgramaCreditoId { get; private set; }
    public string NombrePrograma { get; private set; } = string.Empty;
    public int? UsuarioId { get; private set; }
    public DateTime FechaRegistro { get; private set; }
    public DateTime? FechaModificacion { get; private set; }
    public bool Estado { get; private set; }

    private Estudiante() { }
}
