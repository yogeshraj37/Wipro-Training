var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Serve a custom error page for any unhandled exception.
app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync(System.IO.Path.Combine(builder.Environment.WebRootPath, "error.html"));
    });
});

if (!app.Environment.IsDevelopment())
{
    // Enforce HTTPS in production.
    app.UseHsts();
}

// Log request details and the final response status code.
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Add basic security headers for static content.
app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; style-src 'self'; script-src 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'none'";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    await next();
});

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
