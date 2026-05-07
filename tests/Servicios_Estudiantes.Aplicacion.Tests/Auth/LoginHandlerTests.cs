using FluentAssertions;
using Moq;
using Servicios_Estudiantes.Aplicacion.Auth;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Tests.Auth;

public class LoginHandlerTests
{
    private readonly Mock<IAuthRepository> _authRepoMock = new();
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly Mock<IPasswordHashService> _hashServiceMock = new();

    private LoginHandler CrearHandler() =>
        new(_authRepoMock.Object, _jwtServiceMock.Object, _hashServiceMock.Object);

    private static Usuario CrearUsuario()
    {
        var usuario = (Usuario)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Usuario));
        var type = typeof(Usuario);
        type.GetProperty("UsuarioId")!.SetValue(usuario, 1);
        type.GetProperty("NombreUsuario")!.SetValue(usuario, "admin");
        type.GetProperty("Email")!.SetValue(usuario, "admin@local.test");
        type.GetProperty("Rol")!.SetValue(usuario, "Administrador");
        return usuario;
    }

    [Fact]
    public async Task Handle_DeberiaRetornarTokens_CuandoCredencialesValidas()
    {
        // Arrange
        var command = new LoginCommand("admin", "Admin123!");
        _hashServiceMock.Setup(h => h.Hash("Admin123!")).Returns("hash123");
        _authRepoMock.Setup(r => r.LoginAsync("admin", "hash123")).ReturnsAsync(CrearUsuario());
        _jwtServiceMock.Setup(j => j.GenerarAccessToken(It.IsAny<Usuario>())).Returns("access-token");
        _jwtServiceMock.Setup(j => j.GenerarRefreshToken()).Returns("refresh-token");
        _authRepoMock.Setup(r => r.GuardarRefreshTokenAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns(Task.CompletedTask);

        var handler = CrearHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.AccessToken.Should().Be("access-token");
        result.Value.RefreshToken.Should().Be("refresh-token");
        result.Value.NombreUsuario.Should().Be("admin");
        result.Value.Rol.Should().Be("Administrador");
    }

    [Fact]
    public async Task Handle_DeberiaFallar_CuandoCredencialesInvalidas()
    {
        // Arrange
        var command = new LoginCommand("admin", "wrongpassword");
        _hashServiceMock.Setup(h => h.Hash("wrongpassword")).Returns("badhash");
        _authRepoMock.Setup(r => r.LoginAsync("admin", "badhash")).ReturnsAsync((Usuario?)null);

        var handler = CrearHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("CREDENCIALES_INVALIDAS");
    }

    [Fact]
    public async Task Handle_DeberiaGuardarRefreshToken_CuandoLoginExitoso()
    {
        // Arrange
        var command = new LoginCommand("admin", "Admin123!");
        _hashServiceMock.Setup(h => h.Hash(It.IsAny<string>())).Returns("hash");
        _authRepoMock.Setup(r => r.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(CrearUsuario());
        _jwtServiceMock.Setup(j => j.GenerarAccessToken(It.IsAny<Usuario>())).Returns("token");
        _jwtServiceMock.Setup(j => j.GenerarRefreshToken()).Returns("refresh");

        var handler = CrearHandler();

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _authRepoMock.Verify(r =>
            r.GuardarRefreshTokenAsync(1, "refresh", It.IsAny<DateTime>()), Times.Once);
    }
}
