using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Servicios_Estudiantes.Dominio.Excepciones;

namespace Servicios_Estudiantes.Api.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex, _env.IsDevelopment());
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, bool isDevelopment)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, code, message) = exception switch
        {
            ValidationException ve => (
                HttpStatusCode.BadRequest,
                "VALIDATION_ERROR",
                string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))
            ),
            RecursoNoEncontradoException => (
                HttpStatusCode.NotFound,
                "NOT_FOUND",
                exception.Message
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Forbidden,
                "FORBIDDEN",
                exception.Message
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "INTERNAL_ERROR",
                isDevelopment ? $"{exception.GetType().Name}: {exception.Message}" : "Ocurrió un error interno en el servidor."
            )
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            success = false,
            error = new { code, message }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
