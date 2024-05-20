namespace Users.Api.Logging;

public interface ILoggerAdapter<TType>
{
    void LogInformation(string? message, params object?[] args);

    void LogError(Exception exception, string message, params object?[] args);
}
