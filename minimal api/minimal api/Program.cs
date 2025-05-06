using dotnet_starter.Configurations;
using Microsoft.EntityFrameworkCore;
using dotnet_starter.Features.Extensions;
using dotnet_starter.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.AddSerilog();
Log.Information("Starting application");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity and Authentication
builder.Services.AddAuth();

// Register repositories using extension method
builder.Services.AddRepositories();

// Register feature services
builder.Services.AddFeatures();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Add Serilog request logging
app.UseSerilogRequestLogging();

// Map all feature endpoints using the centralized extension method
app.MapFeatureEndpoints();

app.Run();
