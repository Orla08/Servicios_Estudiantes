using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Servicios_Estudiantes.Dominio.Puertos;
using Servicios_Estudiantes.Infraestructura.Repositorios;
using Servicios_Estudiantes.Infraestructura.Servicios;

namespace Servicios_Estudiantes.Infraestructura;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructura(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Estudiantes")
            ?? throw new InvalidOperationException("ConnectionStrings:Estudiantes no configurada.");

        services.AddScoped<IEstudianteRepository>(_ => new EstudianteRepository(connectionString));
        services.AddScoped<IInscripcionRepository>(_ => new InscripcionRepository(connectionString));
        services.AddScoped<IMateriaRepository>(_ => new MateriaRepository(connectionString));
        services.AddScoped<IProfesorRepository>(_ => new ProfesorRepository(connectionString));
        services.AddScoped<IProgramaCreditoRepository>(_ => new ProgramaCreditoRepository(connectionString));
        services.AddScoped<IUsuarioRepository>(_ => new UsuarioRepository(connectionString));
        services.AddScoped<IAuthRepository>(_ => new AuthRepository(connectionString));

        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IPasswordHashService, PasswordHashService>();

        return services;
    }
}
