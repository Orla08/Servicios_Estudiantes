using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Estudiantes.Queries;

public record ObtenerEstudiantesQuery : IRequest<Result<IEnumerable<Estudiante>>>;

public sealed class ObtenerEstudiantesHandler : IRequestHandler<ObtenerEstudiantesQuery, Result<IEnumerable<Estudiante>>>
{
    private readonly IEstudianteRepository _repo;

    public ObtenerEstudiantesHandler(IEstudianteRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<Estudiante>>> Handle(ObtenerEstudiantesQuery request, CancellationToken cancellationToken)
    {
        var estudiantes = await _repo.ObtenerTodosAsync();
        return Result<IEnumerable<Estudiante>>.Success(estudiantes);
    }
}
