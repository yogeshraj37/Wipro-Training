using BookstoreApp.Data;
using BookstoreApp.Filters;
using BookstoreApp.Repositories;
using BookstoreApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── 1. DATABASE (In-Memory EF Core) ──────────────────────────────────────────
builder.Services.AddDbContext<BookstoreDbContext>(options =>
    options.UseInMemoryDatabase("BookstoreDb"));

// ── 2. REPOSITORIES (Repository Pattern) ─────────────────────────────────────
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// ── 3. SERVICES ───────────────────────────────────────────────────────────────
builder.Services.AddScoped<CartService>();
builder.Services.AddHttpContextAccessor();

// ── 4. FILTERS (registered for ServiceFilter injection) ──────────────────────
builder.Services.AddScoped<AdminOnlyFilter>();
builder.Services.AddScoped<SessionValidationFilter>();
builder.Services.AddScoped<GlobalExceptionFilter>();
builder.Services.AddScoped<RequestLoggingFilter>();

// ── 5. AUTHENTICATION (Cookie-based) ─────────────────────────────────────────
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/auth/accessdenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// ── 6. SESSION ────────────────────────────────────────────────────────────────
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// ── 7. MVC + RAZOR PAGES + GLOBAL FILTERS ────────────────────────────────────
builder.Services.AddControllersWithViews(options =>
{
    // Apply logging and exception handling globally to all MVC actions
    options.Filters.AddService<RequestLoggingFilter>();
    options.Filters.AddService<GlobalExceptionFilter>();
});

builder.Services.AddRazorPages(options =>
{
    // Razor Pages auth: /Admin/* requires Admin role
    options.Conventions.AuthorizeFolder("/Admin", "AdminPolicy");
    options.Conventions.AllowAnonymousToPage("/Auth/Login");
    options.Conventions.AllowAnonymousToPage("/Auth/Register");
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));

var app = builder.Build();

// ── 8. SEED DATABASE ──────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookstoreDbContext>();
    db.Database.EnsureCreated();
}

// ── 9. MIDDLEWARE PIPELINE ────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();           // Session must come before Auth
app.UseAuthentication();
app.UseAuthorization();

// ── 10. ROUTING ───────────────────────────────────────────────────────────────
// Convention-based MVC route
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller}/{action}/{id?}",
    defaults: new { area = "Admin" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Attribute-routed controllers (BooksController, AuthController etc.)
app.MapControllers();

// Razor Pages
app.MapRazorPages();

app.Run();
