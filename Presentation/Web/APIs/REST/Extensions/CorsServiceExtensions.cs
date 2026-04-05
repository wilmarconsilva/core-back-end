namespace Presentation.Web.APIs.REST.Extensions;

public static class CorsServiceExtensions
{
    private const string FrontendPolicyName = "Frontend";
    private const string OriginsSectionName = "Cors:Origins";

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        string[] origins = GetOrigins(configuration);
        if (origins.Length == 0)
            return services;

        services.AddCors(options =>
        {
            options.AddPolicy(FrontendPolicyName, policy =>
            {
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(configuration);

        if (GetOrigins(configuration).Length > 0)
            app.UseCors(FrontendPolicyName);

        return app;
    }

    private static string[] GetOrigins(IConfiguration configuration)
    {
        return configuration.GetSection(OriginsSectionName)
            .Get<string[]>()?
            .Where(origin => !string.IsNullOrWhiteSpace(origin))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? [];
    }
}
