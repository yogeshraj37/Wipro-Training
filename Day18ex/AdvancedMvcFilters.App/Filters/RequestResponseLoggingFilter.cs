using AdvancedMvcFilters.App.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdvancedMvcFilters.App.Filters;

public class RequestResponseLoggingFilter(IApplicationAuditLogger auditLogger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        var url = $"{request.Path}{request.QueryString}";

        auditLogger.LogRequest(request.Method, url);
        var executedContext = await next();
        auditLogger.LogResponse(request.Method, url, executedContext.HttpContext.Response.StatusCode);
    }
}
