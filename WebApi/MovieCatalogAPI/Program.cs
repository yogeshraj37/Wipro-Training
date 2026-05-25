var builder = WebApplication.CreateBuilder(args);

// Add controller services
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

// Map controller routes
app.MapControllers();

app.Run();


// using Microsoft.OpenApi.Models;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container
// builder.Services.AddControllers();

// // Add Swagger/OpenAPI support
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(options =>
// {
//     options.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "Movie Catalog API",
//         Version = "v1",
//         Description = "RESTful API for managing Movies and Directors"
//     });
// });

// var app = builder.Build();

// // Configure the HTTP request pipeline
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Catalog API v1");
//     });
// }

// app.UseHttpsRedirection();

// // Enable Controller Routing
// app.MapControllers();

// app.Run();