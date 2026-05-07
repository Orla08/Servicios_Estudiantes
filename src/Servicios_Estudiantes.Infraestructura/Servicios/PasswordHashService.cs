using System.Security.Cryptography;
using System.Text;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Infraestructura.Servicios;

public sealed class PasswordHashService : IPasswordHashService
{
    public string Hash(string contrasena)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(contrasena));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public bool Verify(string contrasena, string hash) => Hash(contrasena) == hash;
}
