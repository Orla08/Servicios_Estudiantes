using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Profesores.Queries;

public record ObtenerProfesoresQuery : IRequest<Result<IEnumerable<Profesor>>>;

public sealed class ObtenerProfesoresHandler : IRequestHandler<ObtenerProfesoresQuery, Result<IEnumerable<Profesor>>>
{
    private readonly IProfesorRepository _repo;

    public ObtenerProfesoresHandler(IProfesorRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<Profesor>>> Handle(ObtenerProfesoresQuery request, CancellationToken cancellationToken)
    {
        var profesores = await _repo.ObtenerTodosAsync();
        return Result<IEnumerable<Profesor>>.Success(profesores);
    }
}
