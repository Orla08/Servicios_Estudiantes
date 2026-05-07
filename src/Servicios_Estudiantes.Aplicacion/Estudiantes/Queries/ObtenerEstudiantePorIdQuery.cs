using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Excepciones;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Estudiantes.Queries;

public record ObtenerEstudiantePorIdQuery(int Id) : IRequest<Result<Estudiante>>;

public sealed class ObtenerEstudiantePorIdHandler : IRequestHandler<ObtenerEstudiantePorIdQuery, Result<Estudiante>>
{
    private readonly IEstudianteRepository _repo;

    public ObtenerEstudiantePorIdHandler(IEstudianteRepository repo) => _repo = repo;

    public async Task<Result<Estudiante>> Handle(ObtenerEstudiantePorIdQuery request, CancellationToken cancellationToken)
    {
        var estudiante = await _repo.ObtenerPorIdAsync(request.Id);
        if (estudiante is null)
            throw new RecursoNoEncontradoException("Estudiante", request.Id);

        return Result<Estudiante>.Success(estudiante);
    }
}
