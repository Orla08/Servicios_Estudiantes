namespace Servicios_Estudiantes.Dominio.Entidades;

public class RefreshToken
{
    public int RefreshTokenId { get; private set; }
    public int UsuarioId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresUtc { get; private set; }
    public DateTime? RevokedUtc { get; private set; }
    public DateTime CreadoUtc { get; private set; }

    public bool EsValido => RevokedUtc is null && ExpiresUtc > DateTime.UtcNow;

    private RefreshToken() { }
}
