using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Excepciones;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Estudiantes.Commands;

public record EliminarEstudianteCommand(int Id) : IRequest<Result<bool>>;

public sealed class EliminarEstudianteHandler : IRequestHandler<EliminarEstudianteCommand, Result<bool>>
{
    private readonly IEstudianteRepository _repo;

    public EliminarEstudianteHandler(IEstudianteRepository repo) => _repo = repo;

    public async Task<Result<bool>> Handle(EliminarEstudianteCommand request, CancellationToken cancellationToken)
    {
        var existe = await _repo.ObtenerPorIdAsync(request.Id);
        if (existe is null)
            throw new RecursoNoEncontradoException("Estudiante", request.Id);

        await _repo.EliminarAsync(request.Id);
        return Result<bool>.Success(true);
    }
}
