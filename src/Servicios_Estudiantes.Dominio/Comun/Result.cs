namespace Servicios_Estudiantes.Dominio.Comun;

public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public ErrorInfo? Error { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(ErrorInfo error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string code, string message) => new(new ErrorInfo(code, message));
}

public sealed record ErrorInfo(string Code, string Message);
