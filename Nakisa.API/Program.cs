using Nakisa.Application.Mapping;
using Nakisa.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var services = builder.Services;

services.AddControllers();
services.AddOpenApi();

builder.Services.AddAutoMapper(cfg => { }, typeof(UserProfile).Assembly);

services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();