using MediatR;
using Servicios_Estudiantes.Dominio.Comun;
using Servicios_Estudiantes.Dominio.Entidades;
using Servicios_Estudiantes.Dominio.Puertos;

namespace Servicios_Estudiantes.Aplicacion.Usuarios.Queries;

public record ObtenerUsuariosQuery : IRequest<Result<IEnumerable<Usuario>>>;

public sealed class ObtenerUsuariosHandler : IRequestHandler<ObtenerUsuariosQuery, Result<IEnumerable<Usuario>>>
{
    private readonly IUsuarioRepository _repo;

    public ObtenerUsuariosHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<Usuario>>> Handle(ObtenerUsuariosQuery request, CancellationToken cancellationToken)
    {
        var usuarios = await _repo.ObtenerTodosAsync();
        return Result<IEnumerable<Usuario>>.Success(usuarios);
    }
}
