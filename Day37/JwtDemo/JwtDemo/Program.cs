using Microsoft.AspNetCore.Authentication.JwtBearer; // Added for JWT Authentication
using Microsoft.IdentityModel.Tokens;                // Added for Token Validation
using Microsoft.OpenApi.Models;                      // Added for Swagger JWT Support
using System.Text;                                   // Added for Encoding.UTF8

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


// =======================
// SWAGGER CONFIGURATION
// =======================
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWT Demo API",
        Version = "v1"
    });

    // Added JWT Authentication Support in Swagger
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT Token"
        });

    // Makes Swagger send JWT token with requests
    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});


// =======================
// JWT AUTHENTICATION
// =======================
builder.Services.AddAuthentication(options =>
{
    // Default authentication scheme
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    // Default challenge scheme
    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            // Not validating issuer for demo
            ValidateIssuer = false,

            // Not validating audience for demo
            ValidateAudience = false,

            // Check token expiration
            ValidateLifetime = true,

            // Validate signature
            ValidateIssuerSigningKey = true,

            // Secret key from appsettings.json
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["JwtKey"]!))
        };
});


// Added Authorization Service
builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// =======================
// IMPORTANT MIDDLEWARES
// =======================

// Added JWT Authentication Middleware
app.UseAuthentication();

// Added Authorization Middleware
app.UseAuthorization();

app.MapControllers();

app.Run();