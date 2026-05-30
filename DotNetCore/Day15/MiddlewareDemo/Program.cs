 var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Global Exception Middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "text/html";

        await context.Response.WriteAsync(@"
            <h1>Something went wrong!</h1>
            <p>Please try again later.</p>");
    });
});

// HTTPS Enforcement
app.UseHttpsRedirection();

// Request Logging Middleware
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");

    await next();

    Console.WriteLine($"Response Status: {context.Response.StatusCode}");
});

// Content Security Policy Header
app.Use(async (context, next) =>
{
    context.Response.Headers.Append(
        "Content-Security-Policy",
        "default-src 'self'; script-src 'self'; style-src 'self';");

    await next();
});

// Serve Static Files
app.UseStaticFiles();

app.Run();