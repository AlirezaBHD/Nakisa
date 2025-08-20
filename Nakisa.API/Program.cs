using Microsoft.EntityFrameworkCore;
using Nakisa.Application.Mapping;
using Nakisa.Infrastructure;
using Nakisa.Persistence;

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


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("I'm alive"));

app.Run();