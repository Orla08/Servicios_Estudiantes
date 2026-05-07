using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Inscripcion.Commands;

public record CancelarInscripcionPorMateriaCommand(int EstudianteId, int MateriaId) : IRequest<Result<bool>>;

public sealed class CancelarInscripcionPorMateriaHandler : IRequestHandler<CancelarInscripcionPorMateriaCommand, Result<bool>>
{
    private readonly IInscripcionRepository _repo;

    public CancelarInscripcionPorMateriaHandler(IInscripcionRepository repo) => _repo = repo;

    public async Task<Result<bool>> Handle(CancelarInscripcionPorMateriaCommand request, CancellationToken cancellationToken)
    {
        await _repo.CancelarInscripcionPorMateriaAsync(request.EstudianteId, request.MateriaId);
        return Result<bool>.Success(true);
    }
}
