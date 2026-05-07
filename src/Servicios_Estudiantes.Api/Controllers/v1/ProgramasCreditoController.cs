using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios_Estudiantes.Aplicacion.ProgramasCredito.Commands;
using Servicios_Estudiantes.Aplicacion.ProgramasCredito.Queries;

namespace Servicios_Estudiantes.Api.Controllers.v1;

[ApiController]
[Route("api/v1/programas-credito")]
[Authorize]
public sealed class ProgramasCreditoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgramasCreditoController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerTodos()
    {
        var result = await _mediator.Send(new ObtenerProgramasCreditoQuery());
        return Ok(new { success = true, data = result.Value });
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] CrearProgramaCreditoCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, new { success = true, data = new { programaId = result.Value } });
    }
}
