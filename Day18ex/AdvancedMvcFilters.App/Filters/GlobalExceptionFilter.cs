using AdvancedMvcFilters.App.Models;
using AdvancedMvcFilters.App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AdvancedMvcFilters.App.Filters;

public class GlobalExceptionFilter(IApplicationAuditLogger auditLogger) : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        auditLogger.LogError(context.Exception, context.HttpContext.Request.Path);

        context.Result = new ViewResult
        {
            ViewName = "Error",
            ViewData = new ViewDataDictionary<ErrorViewModel>(
                new EmptyModelMetadataProvider(),
                context.ModelState)
            {
                Model = new ErrorViewModel { RequestId = context.HttpContext.TraceIdentifier }
            }
        };
        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }
}
