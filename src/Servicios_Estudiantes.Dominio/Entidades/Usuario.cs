namespace Servicios_Estudiantes.Dominio.Entidades;

public class Usuario
{
    public int UsuarioId { get; private set; }
    public string NombreUsuario { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Rol { get; private set; } = "Estudiante";
    public int? EstudianteId { get; private set; }
    public DateTime FechaRegistro { get; private set; }
    public DateTime? FechaModificacion { get; private set; }
    public bool Estado { get; private set; } = true;

    private Usuario() { }

    public Usuario(string nombreUsuario, string email, string passwordHash, string rol = "Estudiante")
    {
        NombreUsuario = nombreUsuario;
        Email = email;
        PasswordHash = passwordHash;
        Rol = rol;
        FechaRegistro = DateTime.UtcNow;
        Estado = true;
    }
}
