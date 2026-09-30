using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using REST.Swagger.Middlewares;
using REST.Swagger.Options;
using System.Reflection;

namespace REST.Swagger;

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

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .AddOptions<SwaggerAuthenticationOptions>()
            .Bind(configuration.GetSection(SwaggerAuthenticationOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Password),
                $"{SwaggerAuthenticationOptions.SectionName}__{nameof(SwaggerAuthenticationOptions.Password)} must be provided by environment variables or .env file.")
            .ValidateOnStart();

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

        app.UseWhen(IsSwaggerRequest, branch => branch.UseMiddleware<SwaggerAuthenticationMiddleware>());
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", ApiTitle);
            c.RoutePrefix = string.Empty;
        });

        return app;
    }

    // O Swagger UI é servido na raiz (RoutePrefix vazio), junto dos controllers; por isso protege-se apenas a página
    // e o documento OpenAPI. Os assets estáticos (js/css) da biblioteca são públicos e permanecem liberados.
    private static bool IsSwaggerRequest(HttpContext context)
    {
        PathString path = context.Request.Path;

        return path == "/"
            || path.Equals("/index.html", StringComparison.OrdinalIgnoreCase)
            || path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase);
    }
}
