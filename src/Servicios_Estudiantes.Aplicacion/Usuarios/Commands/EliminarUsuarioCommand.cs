using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Excepciones;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Usuarios.Commands;

public record EliminarUsuarioCommand(int Id) : IRequest<Result<bool>>;

public sealed class EliminarUsuarioHandler : IRequestHandler<EliminarUsuarioCommand, Result<bool>>
{
    private readonly IUsuarioRepository _repo;

    public EliminarUsuarioHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<Result<bool>> Handle(EliminarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var existe = await _repo.ObtenerPorIdAsync(request.Id);
        if (existe is null)
            throw new RecursoNoEncontradoException("Usuario", request.Id);

        await _repo.EliminarAsync(request.Id);
        return Result<bool>.Success(true);
    }
}
