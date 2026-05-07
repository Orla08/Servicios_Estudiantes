namespace Servicios_Estudiantes.Dominio.Puertos;

public interface IPasswordHashService
{
    string Hash(string contrasena);
    bool Verify(string contrasena, string hash);
}
