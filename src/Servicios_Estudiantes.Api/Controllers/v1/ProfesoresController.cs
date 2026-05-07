using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios_Estudiantes.Aplicacion.Profesores.Commands;
using Servicios_Estudiantes.Aplicacion.Profesores.Queries;

namespace Servicios_Estudiantes.Api.Controllers.v1;

[ApiController]
[Route("api/v1/profesores")]
[Authorize]
public sealed class ProfesoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfesoresController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var result = await _mediator.Send(new ObtenerProfesoresQuery());
        return Ok(new { success = true, data = result.Value });
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] CrearProfesorCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, new { success = true, data = new { profesorId = result.Value } });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProfesorRequest request)
    {
        var result = await _mediator.Send(new ActualizarProfesorCommand(id, request.Nombre));
        return Ok(new { success = true });
    }
}

public record ActualizarProfesorRequest(string Nombre);
