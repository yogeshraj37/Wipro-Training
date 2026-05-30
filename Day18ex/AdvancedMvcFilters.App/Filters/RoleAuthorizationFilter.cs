using AdvancedMvcFilters.App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdvancedMvcFilters.App.Filters;

public class RoleAuthorizationFilter(
    ICurrentUserService currentUser,
    IRoleAuthorizationService roleAuthorizationService,
    string requiredRole) : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!currentUser.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", new { returnUrl = context.HttpContext.Request.Path.Value });
        }
        else if (!roleAuthorizationService.IsInRole(currentUser, requiredRole))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        return Task.CompletedTask;
    }
}
