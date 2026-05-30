namespace AdvancedMvcFilters.App.Services;

public class SessionCurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public bool IsAuthenticated
    {
        get
        {
            var httpContext = accessor.HttpContext;
            return httpContext?.Session.GetString("UserId") is not null
                || httpContext?.User.Identity?.IsAuthenticated == true;
        }
    }

    public string? UserId
    {
        get
        {
            var httpContext = accessor.HttpContext;
            return httpContext?.Session.GetString("UserId")
                ?? httpContext?.User.Identity?.Name;
        }
    }

    public IReadOnlyCollection<string> Roles
    {
        get
        {
            var httpContext = accessor.HttpContext;
            var roles = httpContext?.Session.GetString("Roles");
            if (!string.IsNullOrWhiteSpace(roles))
            {
                return roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            return httpContext?.User.Claims
                .Where(claim => claim.Type.EndsWith("/role", StringComparison.OrdinalIgnoreCase) || claim.Type == "role")
                .Select(claim => claim.Value)
                .ToArray()
                ?? [];
        }
    }
}
