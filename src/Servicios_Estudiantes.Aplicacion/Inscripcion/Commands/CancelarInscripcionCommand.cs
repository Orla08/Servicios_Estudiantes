using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Inscripcion.Commands;

public record CancelarInscripcionCommand(int EstudianteId) : IRequest<Result<bool>>;

public sealed class CancelarInscripcionHandler : IRequestHandler<CancelarInscripcionCommand, Result<bool>>
{
    private readonly IInscripcionRepository _repo;

    public CancelarInscripcionHandler(IInscripcionRepository repo) => _repo = repo;

    public async Task<Result<bool>> Handle(CancelarInscripcionCommand request, CancellationToken cancellationToken)
    {
        await _repo.CancelarInscripcionAsync(request.EstudianteId);
        return Result<bool>.Success(true);
    }
}
