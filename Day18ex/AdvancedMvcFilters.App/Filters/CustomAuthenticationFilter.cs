using AdvancedMvcFilters.App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdvancedMvcFilters.App.Filters;

public class CustomAuthenticationFilter(ICurrentUserService currentUser) : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!currentUser.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", new { returnUrl = context.HttpContext.Request.Path.Value });
        }

        return Task.CompletedTask;
    }
}
