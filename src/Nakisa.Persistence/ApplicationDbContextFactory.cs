using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Nakisa.Persistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configPath = Path.Combine("..", "Nakisa.API", "appsettings.json");
            var fullPath = Path.GetFullPath(configPath);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fullPath)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}