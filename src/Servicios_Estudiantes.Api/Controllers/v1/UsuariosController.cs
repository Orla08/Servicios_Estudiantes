using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios_Estudiantes.Aplicacion.Usuarios.Commands;
using Servicios_Estudiantes.Aplicacion.Usuarios.Queries;

namespace Servicios_Estudiantes.Api.Controllers.v1;

[ApiController]
[Route("api/v1/usuarios")]
[Authorize(Roles = "Administrador")]
public sealed class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var result = await _mediator.Send(new ObtenerUsuariosQuery());
        return Ok(new { success = true, data = result.Value });
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearUsuarioCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, new { success = true, data = new { usuarioId = result.Value } });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarUsuarioRequest request)
    {
        var result = await _mediator.Send(new ActualizarUsuarioCommand(id, request.NombreUsuario, request.Email, request.Rol));
        return Ok(new { success = true });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var result = await _mediator.Send(new EliminarUsuarioCommand(id));
        return Ok(new { success = true });
    }
}

public record ActualizarUsuarioRequest(string NombreUsuario, string Email, string Rol);
