using Coach.Api.Endpoints.Athletes;
using Coach.Api.Endpoints.Auth;
using Coach.Api.Endpoints.Integrations;
using Coach.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuth();
app.MapAthletes();
app.MapIntegrations();

// Future modules:
// app.MapActivities();
// app.MapEquipment();
// app.MapGoals();
// app.MapTrainingPlans();

app.Run();
