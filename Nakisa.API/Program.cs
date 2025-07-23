using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Nakisa.Application.Interfaces;
using Nakisa.Application.Services;
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

services.AddHostedService<BotService>();
services.AddScoped<IBotFlowDispatcher, BotFlowDispatcher>();
services.AddScoped<IPlaylistBrowseFlowHandler, PlaylistBrowseFlowHandler>();
services.AddScoped<ISongSubmitFlowHandler,SongSubmitFlowHandler>();
services.AddSingleton<IUserSessionService, UserSessionService>();
services.AddScoped<IRegisterFlowHandler, RegisterFlowHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();