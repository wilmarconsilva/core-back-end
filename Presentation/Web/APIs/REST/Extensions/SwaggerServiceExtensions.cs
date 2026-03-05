using Microsoft.OpenApi.Models;

namespace Presentation.Web.APIs.REST.Extensions;

/// <summary>
/// Extensões para configurar API documentation com Swagger.
/// </summary>
public static class SwaggerServiceExtensions
{
    private const string ApiTitle = "Core - Web API REST";
    private const string ApiVersion = "v1";

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc(ApiVersion, new OpenApiInfo
            {
                Title = ApiTitle,
                Version = ApiVersion,
                Description = string.Empty,
            });

            string xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            setup.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", ApiTitle);
                c.RoutePrefix = string.Empty;
            });
        }

        return app;
    }
}
