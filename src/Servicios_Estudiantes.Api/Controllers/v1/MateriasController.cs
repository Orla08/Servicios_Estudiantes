using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios_Estudiantes.Aplicacion.Materias.Commands;
using Servicios_Estudiantes.Aplicacion.Materias.Queries;

namespace Servicios_Estudiantes.Api.Controllers.v1;

[ApiController]
[Route("api/v1/materias")]
[Authorize]
public sealed class MateriasController : ControllerBase
{
    private readonly IMediator _mediator;

    public MateriasController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
    {
        var result = await _mediator.Send(new ObtenerMateriasQuery());
        return Ok(new { success = true, data = result.Value });
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] CrearMateriaCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, new { success = true, data = new { materiaId = result.Value } });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarMateriaRequest request)
    {
        var result = await _mediator.Send(new ActualizarMateriaCommand(id, request.Nombre, request.Creditos, request.ProfesorId, request.ProgramaCreditoId));
        return Ok(new { success = true });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var result = await _mediator.Send(new EliminarMateriaCommand(id));
        return Ok(new { success = true });
    }
}

public record ActualizarMateriaRequest(string Nombre, int Creditos, int ProfesorId, int ProgramaCreditoId);
