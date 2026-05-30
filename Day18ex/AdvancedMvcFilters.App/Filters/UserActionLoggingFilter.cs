using AdvancedMvcFilters.App.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdvancedMvcFilters.App.Filters;

public class UserActionLoggingFilter(
    ICurrentUserService currentUser,
    IApplicationAuditLogger auditLogger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (currentUser.IsAuthenticated)
        {
            auditLogger.LogUserAction(currentUser.UserId ?? "unknown", context.ActionDescriptor.DisplayName ?? "UnknownAction", DateTimeOffset.UtcNow);
        }

        await next();
    }
}
