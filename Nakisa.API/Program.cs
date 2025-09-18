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

services.AddAutoMapper(cfg => { }, typeof(UserProfile).Assembly);


#region Temporary!

var baseDir = Directory.GetCurrentDirectory();

var rootDir = Path.GetFullPath(Path.Combine(baseDir, ".."));

var destPath = Path.Combine(rootDir, "WTelegram.session");
builder.Configuration["TelegramClient:SessionPath"] =destPath;

#endregion


services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


//temp

// if (app.Environment.IsDevelopment())
// {
    app.MapOpenApi();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseStaticFiles();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("I'm alive"));

app.Run();