using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios_Estudiantes.Aplicacion.Estudiantes.Commands;
using Servicios_Estudiantes.Aplicacion.Estudiantes.Queries;
using Servicios_Estudiantes.Aplicacion.Inscripcion.Commands;
using Servicios_Estudiantes.Aplicacion.Inscripcion.Queries;

namespace Servicios_Estudiantes.Api.Controllers.v1;

[ApiController]
[Route("api/v1/estudiantes")]
[Authorize]
public sealed class EstudiantesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EstudiantesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var result = await _mediator.Send(new ObtenerEstudiantesQuery());
        return Ok(new { success = true, data = result.Value });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var result = await _mediator.Send(new ObtenerEstudiantePorIdQuery(id));
        return Ok(new { success = true, data = result.Value });
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] CrearEstudianteCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, new { success = true, data = new { estudianteId = result.Value } });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEstudianteRequest request)
    {
        var result = await _mediator.Send(new ActualizarEstudianteCommand(id, request.Nombre, request.Email, request.ProgramaCreditoId));
        return Ok(new { success = true });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var result = await _mediator.Send(new EliminarEstudianteCommand(id));
        return Ok(new { success = true });
    }

    [HttpGet("{id:int}/inscripcion")]
    [Authorize(Roles = "Estudiante,Administrador")]
    public async Task<IActionResult> ObtenerInscripcion(int id)
    {
        var result = await _mediator.Send(new ObtenerInscripcionQuery(id));
        return Ok(new { success = true, data = result.Value });
    }

    [HttpPost("{id:int}/inscripcion")]
    [Authorize(Roles = "Estudiante,Administrador")]
    public async Task<IActionResult> RegistrarInscripcion(int id, [FromBody] InscripcionRequest request)
    {
        var result = await _mediator.Send(new RegistrarInscripcionCommand(id, request.MateriaIds));
        return result.IsSuccess
            ? StatusCode(201, new { success = true })
            : BadRequest(new { success = false, error = result.Error });
    }

    [HttpDelete("{id:int}/inscripcion")]
    [Authorize(Roles = "Estudiante,Administrador")]
    public async Task<IActionResult> CancelarInscripcion(int id)
    {
        var result = await _mediator.Send(new CancelarInscripcionCommand(id));
        return Ok(new { success = true });
    }

    [HttpDelete("{id:int}/inscripcion/{materiaId:int}")]
    [Authorize(Roles = "Estudiante,Administrador")]
    public async Task<IActionResult> CancelarInscripcionPorMateria(int id, int materiaId)
    {
        var result = await _mediator.Send(new CancelarInscripcionPorMateriaCommand(id, materiaId));
        return Ok(new { success = true });
    }

    [HttpGet("{id:int}/companeros")]
    [Authorize(Roles = "Estudiante,Administrador")]
    public async Task<IActionResult> ObtenerCompaneros(int id)
    {
        var result = await _mediator.Send(new ObtenerCompanerosQuery(id));
        return Ok(new { success = true, data = result.Value });
    }
}

public record ActualizarEstudianteRequest(string Nombre, string Email, int ProgramaCreditoId);
public record InscripcionRequest(List<int> MateriaIds);
