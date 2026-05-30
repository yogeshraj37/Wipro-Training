namespace AdvancedMvcFilters.App.Services;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    IReadOnlyCollection<string> Roles { get; }
}
