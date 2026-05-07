using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Inscripcion.Queries;

public record ObtenerInscripcionQuery(int EstudianteId) : IRequest<Result<IEnumerable<InscripcionEstudianteMateria>>>;

public sealed class ObtenerInscripcionHandler : IRequestHandler<ObtenerInscripcionQuery, Result<IEnumerable<InscripcionEstudianteMateria>>>
{
    private readonly IInscripcionRepository _repo;

    public ObtenerInscripcionHandler(IInscripcionRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<InscripcionEstudianteMateria>>> Handle(ObtenerInscripcionQuery request, CancellationToken cancellationToken)
    {
        var inscripciones = await _repo.ObtenerInscripcionAsync(request.EstudianteId);
        return Result<IEnumerable<InscripcionEstudianteMateria>>.Success(inscripciones);
    }
}
