using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using System.Reflection;

namespace REST.Extensions;

/// <summary>
/// Extensões para configurar API documentation com Swagger.
/// </summary>
public static class SwaggerServiceExtensions
{
    private const string ApiTitle = "Core - Web Service - REST";
    private const string ApiVersion = "v1.0.0";

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSwaggerGen(setup =>
        {
            string appVersion = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? "desconhecida";

            setup.SwaggerDoc(ApiVersion, new OpenApiInfo
            {
                Title = ApiTitle,
                Version = ApiVersion,
                Description = $"Versão da aplicação: {appVersion}",
            });

            string xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            setup.IncludeXmlComments(xmlPath);

            OpenApiSecurityScheme securityScheme = new()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            };

            setup.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            setup.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, document)] = []
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", ApiTitle);
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}
