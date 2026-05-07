namespace Servicios_Estudiantes.Dominio.Excepciones;

public class ProfesorDuplicadoException : Exception
{
    public ProfesorDuplicadoException(string nombreProfesor, string materia1, string materia2)
        : base($"No es posible inscribir materias con el mismo profesor. Las materias '{materia1}' y '{materia2}' son dictadas por {nombreProfesor}.") { }
}
