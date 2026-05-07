using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios_Estudiantes.Aplicacion.Auth;

namespace Servicios_Estudiantes.Api.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request.NombreUsuario, request.Contrasena));
        return result.IsSuccess ? Ok(new { success = true, data = result.Value }) : BadRequest(new { success = false, error = result.Error });
    }

    [HttpPost("registro")]
    [AllowAnonymous]
    public async Task<IActionResult> Registro([FromBody] RegistroCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? StatusCode(201, new { success = true, data = new { estudianteId = result.Value } }) : BadRequest(new { success = false, error = result.Error });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(request.RefreshToken));
        return result.IsSuccess ? Ok(new { success = true, data = result.Value }) : Unauthorized(new { success = false, error = result.Error });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var result = await _mediator.Send(new LogoutCommand(request.RefreshToken));
        return Ok(new { success = true });
    }
}

public record LoginRequest(string NombreUsuario, string Contrasena);
public record RefreshRequest(string RefreshToken);
public record LogoutRequest(string RefreshToken);
