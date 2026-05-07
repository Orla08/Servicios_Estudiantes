using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Materias.Queries;

public record ObtenerMateriasQuery : IRequest<Result<IEnumerable<Materia>>>;

public sealed class ObtenerMateriasHandler : IRequestHandler<ObtenerMateriasQuery, Result<IEnumerable<Materia>>>
{
    private readonly IMateriaRepository _repo;

    public ObtenerMateriasHandler(IMateriaRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<Materia>>> Handle(ObtenerMateriasQuery request, CancellationToken cancellationToken)
    {
        var materias = await _repo.ObtenerTodasAsync();
        return Result<IEnumerable<Materia>>.Success(materias);
    }
}
