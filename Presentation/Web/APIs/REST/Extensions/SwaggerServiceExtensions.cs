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
    private readonly static string _apiVersion = $"v{Assembly
        .GetEntryAssembly()?
        .GetName()
        .Version?
        .ToString(3) ?? "desconhecida"}";

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc(_apiVersion, new OpenApiInfo
            {
                Title = ApiTitle,
                Version = _apiVersion,
                Description = $"Versão detalhada: {Assembly
                .GetExecutingAssembly()?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion ?? "desconhecida"}"
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
            c.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", ApiTitle);
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}
