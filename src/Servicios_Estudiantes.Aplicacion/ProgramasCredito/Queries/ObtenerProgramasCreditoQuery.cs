using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.ProgramasCredito.Queries;

public record ObtenerProgramasCreditoQuery : IRequest<Result<IEnumerable<ProgramaCredito>>>;

public sealed class ObtenerProgramasCreditoHandler : IRequestHandler<ObtenerProgramasCreditoQuery, Result<IEnumerable<ProgramaCredito>>>
{
    private readonly IProgramaCreditoRepository _repo;

    public ObtenerProgramasCreditoHandler(IProgramaCreditoRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<ProgramaCredito>>> Handle(ObtenerProgramasCreditoQuery request, CancellationToken cancellationToken)
    {
        var programas = await _repo.ObtenerTodosAsync();
        return Result<IEnumerable<ProgramaCredito>>.Success(programas);
    }
}
