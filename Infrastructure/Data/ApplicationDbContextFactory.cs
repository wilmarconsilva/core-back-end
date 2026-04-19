using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string basePath = Directory.GetCurrentDirectory();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            string keyConnectionString = "Default";
            string connectionString = configuration.GetConnectionString(keyConnectionString) ?? throw new KeyNotFoundException($"Connection string: '{keyConnectionString}' not found.");

            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql(connectionString);

            ApplicationDbContext context = new(optionsBuilder.Options);

            return context;
        }
    }
}