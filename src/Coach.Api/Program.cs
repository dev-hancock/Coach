using Coach.Api.Endpoints.Athletes;
using Coach.Api.Endpoints.Auth;
using Coach.Api.Endpoints.Integrations;
using Coach.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApiServices();
builder.Services.AddApplicationLayers(builder.Configuration);

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Coach API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at app root
    });
}

app.UseHttpsRedirection();

// Authentication & Authorization (JWT)
app.UseAuthentication();
app.UseAuthorization();

// Map auth endpoints (JWT-based authentication with athlete registration)
app.MapCustomAuthEndpoints();

// Map athlete endpoints
app.MapAthletes();

// Map integration endpoints
app.MapIntegrations();

// Future modules:
// app.MapActivities();
// app.MapEquipment();
// app.MapGoals();
// app.MapTrainingPlans();

app.Run();
