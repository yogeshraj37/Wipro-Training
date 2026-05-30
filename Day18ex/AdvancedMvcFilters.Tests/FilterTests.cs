using AdvancedMvcFilters.App.Filters;
using AdvancedMvcFilters.App.Models;
using AdvancedMvcFilters.App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace AdvancedMvcFilters.Tests;

public class FilterTests
{
    [Fact]
    public async Task LoggingFilter_RecordsRequestAndResponseDetails()
    {
        var logger = new CapturingAuditLogger();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "POST";
        httpContext.Request.Path = "/orders";
        httpContext.Request.QueryString = new QueryString("?id=1001");
        httpContext.Response.StatusCode = StatusCodes.Status201Created;

        var actionContext = CreateActionContext(httpContext);
        var executingContext = new ActionExecutingContext(actionContext, [], new Dictionary<string, object?>(), controller: new object());
        var executedContext = new ActionExecutedContext(actionContext, [], controller: new object());
        var filter = new RequestResponseLoggingFilter(logger);

        await filter.OnActionExecutionAsync(executingContext, () => Task.FromResult(executedContext));

        Assert.Equal(("POST", "/orders?id=1001"), logger.Requests.Single());
        Assert.Equal(("POST", "/orders?id=1001", StatusCodes.Status201Created), logger.Responses.Single());
    }

    [Fact]
    public async Task AuthenticationFilter_RedirectsAnonymousUsersToLogin()
    {
        var filter = new CustomAuthenticationFilter(new FakeCurrentUser(false));
        var context = new AuthorizationFilterContext(CreateActionContext(new DefaultHttpContext()), []);

        await filter.OnAuthorizationAsync(context);

        var redirect = Assert.IsType<RedirectToActionResult>(context.Result);
        Assert.Equal("Login", redirect.ActionName);
        Assert.Equal("Auth", redirect.ControllerName);
    }

    [Fact]
    public async Task RoleAuthorizationFilter_ForbidsAuthenticatedUserWithoutRole()
    {
        var user = new FakeCurrentUser(true, roles: ["Customer"]);
        var filter = new RoleAuthorizationFilter(user, new RoleAuthorizationService(), "Admin");
        var context = new AuthorizationFilterContext(CreateActionContext(new DefaultHttpContext()), []);

        await filter.OnAuthorizationAsync(context);

        var statusCode = Assert.IsType<StatusCodeResult>(context.Result);
        Assert.Equal(StatusCodes.Status403Forbidden, statusCode.StatusCode);
    }

    [Fact]
    public async Task ErrorFilter_LogsExceptionAndReturnsFriendlyErrorView()
    {
        var logger = new CapturingAuditLogger();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/orders/simulateerror";
        httpContext.TraceIdentifier = "trace-123";
        var exception = new InvalidOperationException("Boom");
        var context = new ExceptionContext(CreateActionContext(httpContext), [])
        {
            Exception = exception
        };
        var filter = new GlobalExceptionFilter(logger);

        await filter.OnExceptionAsync(context);

        Assert.True(context.ExceptionHandled);
        Assert.Equal((exception, "/orders/simulateerror"), logger.Errors.Single());
        var view = Assert.IsType<ViewResult>(context.Result);
        Assert.Equal("Error", view.ViewName);
        var model = Assert.IsType<ErrorViewModel>(view.Model);
        Assert.Equal("trace-123", model.RequestId);
    }

    private static ActionContext CreateActionContext(HttpContext httpContext)
    {
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }

    private sealed class FakeCurrentUser(bool isAuthenticated, string? userId = "user1", IReadOnlyCollection<string>? roles = null) : ICurrentUserService
    {
        public bool IsAuthenticated { get; } = isAuthenticated;
        public string? UserId { get; } = userId;
        public IReadOnlyCollection<string> Roles { get; } = roles ?? [];
    }

    private sealed class CapturingAuditLogger : IApplicationAuditLogger
    {
        public List<(string Method, string Url)> Requests { get; } = [];
        public List<(string Method, string Url, int StatusCode)> Responses { get; } = [];
        public List<(Exception Exception, string Path)> Errors { get; } = [];
        public List<(string UserId, string ActionName, DateTimeOffset Timestamp)> UserActions { get; } = [];

        public void LogRequest(string method, string url)
        {
            Requests.Add((method, url));
        }

        public void LogResponse(string method, string url, int statusCode)
        {
            Responses.Add((method, url, statusCode));
        }

        public void LogError(Exception exception, string path)
        {
            Errors.Add((exception, path));
        }

        public void LogUserAction(string userId, string actionName, DateTimeOffset timestamp)
        {
            UserActions.Add((userId, actionName, timestamp));
        }
    }
}
