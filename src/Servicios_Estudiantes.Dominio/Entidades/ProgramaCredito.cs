namespace Servicios_Estudiantes.Dominio.Entidades;

public class ProgramaCredito
{
    public int ProgramaCreditoId { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public int CreditosPorMateria { get; private set; }
    public int MaxMateriasPorEstudiante { get; private set; }
    public DateTime FechaRegistro { get; private set; }
    public DateTime? FechaModificacion { get; private set; }
    public bool Estado { get; private set; }

    private ProgramaCredito() { }
}
