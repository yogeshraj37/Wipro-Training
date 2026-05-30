namespace AdvancedMvcFilters.App.Services;

public class ApplicationAuditLogger(ILogger<ApplicationAuditLogger> logger) : IApplicationAuditLogger
{
    public void LogRequest(string method, string url)
    {
        logger.LogInformation("Request started: {Method} {Url}", method, url);
    }

    public void LogResponse(string method, string url, int statusCode)
    {
        logger.LogInformation("Request completed: {Method} {Url} responded {StatusCode}", method, url, statusCode);
    }

    public void LogError(Exception exception, string path)
    {
        logger.LogError(exception, "Unhandled exception while processing {Path}", path);
    }

    public void LogUserAction(string userId, string actionName, DateTimeOffset timestamp)
    {
        logger.LogInformation("User action: {UserId} performed {ActionName} at {Timestamp}", userId, actionName, timestamp);
    }
}
