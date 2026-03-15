using Serilog;
using Serilog.Events;

namespace Presentation.Web.APIs.REST.Extensions;

/// <summary>
/// Extensões para configurar logging com Serilog.
/// </summary>
public static class LoggingServiceExtensions
{
    public static WebApplicationBuilder AddLoggingConfiguration(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(
                "Logs/.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}
