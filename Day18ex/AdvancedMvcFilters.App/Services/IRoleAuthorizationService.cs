namespace AdvancedMvcFilters.App.Services;

public interface IRoleAuthorizationService
{
    bool IsInRole(ICurrentUserService user, string role);
}
