var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<AdvancedMvcFilters.App.Services.ICurrentUserService, AdvancedMvcFilters.App.Services.SessionCurrentUserService>();
builder.Services.AddScoped<AdvancedMvcFilters.App.Services.IRoleAuthorizationService, AdvancedMvcFilters.App.Services.RoleAuthorizationService>();
builder.Services.AddScoped<AdvancedMvcFilters.App.Services.IApplicationAuditLogger, AdvancedMvcFilters.App.Services.ApplicationAuditLogger>();

builder.Services.AddScoped<AdvancedMvcFilters.App.Filters.RequestResponseLoggingFilter>();
builder.Services.AddScoped<AdvancedMvcFilters.App.Filters.CustomAuthenticationFilter>();
builder.Services.AddScoped<AdvancedMvcFilters.App.Filters.UserActionLoggingFilter>();
builder.Services.AddScoped<AdvancedMvcFilters.App.Filters.GlobalExceptionFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<AdvancedMvcFilters.App.Filters.RequestResponseLoggingFilter>();
    options.Filters.AddService<AdvancedMvcFilters.App.Filters.GlobalExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
