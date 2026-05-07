namespace Servicios_Estudiantes.Dominio.Excepciones;

public class RecursoNoEncontradoException : Exception
{
    public RecursoNoEncontradoException(string recurso, object id)
        : base($"{recurso} con id '{id}' no fue encontrado.") { }
}
