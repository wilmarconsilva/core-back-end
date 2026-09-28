using Infrastructure.Shared.Persistance;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace REST.Extensions;

/// <summary>
/// Extensões para aplicar migrations na inicialização da aplicação.
/// </summary>
public static class MigrationServiceExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Log.Logger.Information("Aplicando migrations pendentes do banco de dados...");
        await dbContext.Database.MigrateAsync();
        Log.Logger.Information("Migrations aplicadas com sucesso.");
    }
}
