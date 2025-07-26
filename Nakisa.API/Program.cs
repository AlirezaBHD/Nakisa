using Microsoft.EntityFrameworkCore;
using Nakisa.Application.Interfaces;
using Nakisa.Application.Mapping;
using Nakisa.Infrastructure;
using Nakisa.Persistence;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddOpenApi();

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        )
        .UseSnakeCaseNamingConvention());

builder.Services.AddAutoMapper(cfg => { }, typeof(UserProfile).Assembly);

builder.Services.AddApplicationServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();