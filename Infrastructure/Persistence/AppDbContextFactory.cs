using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistece
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            string basePath = Directory.GetCurrentDirectory();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddDotNetEnv(options: LoadOptions.TraversePath().NoClobber())
                .Build();

            string keyConnectionString = "Postgres";
            string? connectionString = configuration.GetConnectionString(keyConnectionString);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new KeyNotFoundException(
                    $"Connection string: '{keyConnectionString}' is not defined as an environment variable or in the .env file.");

            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql(connectionString);

            AppDbContext context = new(optionsBuilder.Options);

            return context;
        }
    }
}