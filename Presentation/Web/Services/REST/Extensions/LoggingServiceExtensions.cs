using Serilog;

namespace REST.Extensions;

/// <summary>
/// Extensões para configurar logging com Serilog.
/// </summary>
public static class LoggingServiceExtensions
{
    public static WebApplicationBuilder AddLoggingConfiguration(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (builder.Environment.IsDevelopment())
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }
        else
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                // Em container o arquivo é efêmero (perdido a cada recriação) e invisível para
                // "docker logs". O console é o canal padrão coletado pelo runtime.
                .WriteTo.Console()
                .WriteTo.File(
                "Logs/.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7, // 7 days
                fileSizeLimitBytes: 100_000_000, // 100 MB
                rollOnFileSizeLimit: true)
                .CreateLogger();
        }

        builder.Host.UseSerilog();

        return builder;
    }
}
