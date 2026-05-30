namespace AdvancedMvcFilters.App.Services;

public interface IApplicationAuditLogger
{
    void LogRequest(string method, string url);
    void LogResponse(string method, string url, int statusCode);
    void LogError(Exception exception, string path);
    void LogUserAction(string userId, string actionName, DateTimeOffset timestamp);
}
