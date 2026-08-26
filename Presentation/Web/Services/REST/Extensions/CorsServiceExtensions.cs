namespace REST.Extensions;

/// <summary>
/// Extensões para configurar CORS.
/// </summary>
public static class CorsServiceExtensions
{
    private const string FrontendPolicyName = "Frontend";
    private const string OriginsSectionName = "Cors:Origins";
    private const string OriginSuffixesSectionName = "Cors:OriginSuffixes";

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);

        string[] origins = GetValues(configuration, OriginsSectionName);
        string[] originSuffixes = GetValues(configuration, OriginSuffixesSectionName);

        services.AddCors(options =>
        {
            options.AddPolicy(FrontendPolicyName, policy =>
            {
                policy.SetIsOriginAllowed(origin => IsOriginAllowed(origin, origins, originSuffixes, environment))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseCors(FrontendPolicyName);

        return app;
    }

    private static bool IsOriginAllowed(string origin, string[] origins, string[] originSuffixes, IHostEnvironment environment)
    {
        if (!Uri.TryCreate(origin, UriKind.Absolute, out Uri? originUri))
            return false;

        if (environment.IsDevelopment() && originUri.Host is "localhost" or "127.0.0.1" or "::1")
            return true;

        if (origins.Contains(origin, StringComparer.OrdinalIgnoreCase))
            return true;

        return originSuffixes.Any(suffix =>
            originUri.Host.EndsWith(suffix, StringComparison.OrdinalIgnoreCase));
    }

    private static string[] GetValues(IConfiguration configuration, string sectionName)
    {
        return configuration.GetSection(sectionName)
            .Get<string[]>()?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? [];
    }
}
