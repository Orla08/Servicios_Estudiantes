using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Excepciones;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Materias.Commands;

public record EliminarMateriaCommand(int Id) : IRequest<Result<bool>>;

public sealed class EliminarMateriaHandler : IRequestHandler<EliminarMateriaCommand, Result<bool>>
{
    private readonly IMateriaRepository _repo;

    public EliminarMateriaHandler(IMateriaRepository repo) => _repo = repo;

    public async Task<Result<bool>> Handle(EliminarMateriaCommand request, CancellationToken cancellationToken)
    {
        var existe = await _repo.ObtenerPorIdAsync(request.Id);
        if (existe is null)
            throw new RecursoNoEncontradoException("Materia", request.Id);

        await _repo.EliminarAsync(request.Id);
        return Result<bool>.Success(true);
    }
}
