namespace AdvancedMvcFilters.App.Services;

public class RoleAuthorizationService : IRoleAuthorizationService
{
    public bool IsInRole(ICurrentUserService user, string role)
    {
        return user.Roles.Any(existingRole => string.Equals(existingRole, role, StringComparison.OrdinalIgnoreCase));
    }
}
