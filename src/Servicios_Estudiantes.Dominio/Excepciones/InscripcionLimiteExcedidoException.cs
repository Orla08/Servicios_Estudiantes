namespace Servicios_Estudiantes.Dominio.Excepciones;

public class InscripcionLimiteExcedidoException : Exception
{
    public InscripcionLimiteExcedidoException(int limite)
        : base($"No se puede inscribir más de {limite} materias por estudiante.") { }
}
