// ==========================================
// Filters/GlobalExceptionFilter.cs
// ==========================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookstoreApp.Filters
{
    /// <summary>
    /// Global exception filter: logs all unhandled exceptions and returns
    /// a friendly error response instead of crashing the application.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception,
                "Unhandled exception on {Path} at {Time}",
                context.HttpContext.Request.Path,
                DateTime.UtcNow);

            // Redirect to a friendly error page
            context.Result = new RedirectToActionResult("Error", "Home", new
            {
                message = "An unexpected error occurred. Please try again."
            });

            context.ExceptionHandled = true;
        }
    }
}

// ==========================================
// Filters/RequestLoggingFilter.cs
// ==========================================
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookstoreApp.Filters
{
    /// <summary>
    /// Action filter that logs every incoming request with method, path,
    /// authenticated user, and execution duration. Applied globally.
    /// </summary>
    public class RequestLoggingFilter : IActionFilter
    {
        private readonly ILogger<RequestLoggingFilter> _logger;

        public RequestLoggingFilter(ILogger<RequestLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
            _logger.LogInformation(
                "[REQUEST] {Method} {Path} | User: {User} | {Time}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                user,
                DateTime.UtcNow);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation(
                "[RESPONSE] {Path} | Status: {Status}",
                context.HttpContext.Request.Path,
                context.HttpContext.Response.StatusCode);
        }
    }
}

// ==========================================
// Filters/AdminOnlyFilter.cs
// ==========================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookstoreApp.Filters
{
    /// <summary>
    /// Authorization filter that restricts access to admin-only actions.
    /// Returns 403 Forbidden if the user is not in the "Admin" role.
    /// Usage: [ServiceFilter(typeof(AdminOnlyFilter))]
    /// </summary>
    public class AdminOnlyFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", new
                {
                    returnUrl = context.HttpContext.Request.Path
                });
                return;
            }

            if (!user.IsInRole("Admin"))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}

// ==========================================
// Filters/SessionValidationFilter.cs
// ==========================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookstoreApp.Filters
{
    /// <summary>
    /// Action filter that validates the session is still active before
    /// executing actions that require an authenticated session.
    /// Usage: [ServiceFilter(typeof(SessionValidationFilter))]
    /// </summary>
    public class SessionValidationFilter : IActionFilter
    {
        private readonly ILogger<SessionValidationFilter> _logger;

        public SessionValidationFilter(ILogger<SessionValidationFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userId = session.GetInt32("UserId");

            if (userId == null)
            {
                _logger.LogWarning("Session expired for path: {Path}", context.HttpContext.Request.Path);
                context.Result = new RedirectToActionResult("Login", "Auth",
                    new { message = "Your session has expired. Please log in again." });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
