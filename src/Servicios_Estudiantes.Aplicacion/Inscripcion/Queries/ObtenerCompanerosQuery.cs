using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Inscripcion.Queries;

public record ObtenerCompanerosQuery(int EstudianteId) : IRequest<Result<IEnumerable<CompaneroMateria>>>;

public sealed class ObtenerCompanerosHandler : IRequestHandler<ObtenerCompanerosQuery, Result<IEnumerable<CompaneroMateria>>>
{
    private readonly IInscripcionRepository _repo;

    public ObtenerCompanerosHandler(IInscripcionRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<CompaneroMateria>>> Handle(ObtenerCompanerosQuery request, CancellationToken cancellationToken)
    {
        var companeros = await _repo.ObtenerCompanerosAsync(request.EstudianteId);
        return Result<IEnumerable<CompaneroMateria>>.Success(companeros);
    }
}
