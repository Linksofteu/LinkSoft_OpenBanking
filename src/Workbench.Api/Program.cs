global using FluentValidation;
global using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Swagger;
using Workbench;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints(config =>
    {
        config.Endpoints.RoutePrefix = "api";
        config.Endpoints.ShortNames = true;
    }
);
app.UseSwaggerGen();

app.Run();