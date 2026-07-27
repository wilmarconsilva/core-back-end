using DotNetEnv;
using DotNetEnv.Configuration;
using Presentation.Web.APIs.REST.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Carrega o .env único do repositório. TraversePath localiza o .env na raiz mesmo rodando a partir desta pasta do projeto; NoClobber garante
// que uma variável de ambiente real do processo/deploy sempre prevalece sobre o valor do .env.
builder.Configuration.AddDotNetEnv(options: LoadOptions.TraversePath().NoClobber());

// Configure Logging
builder.AddLoggingConfiguration();

// Add Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services.AddSwaggerConfiguration();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

WebApplication app = builder.Build();

// Configure Middleware
app.UseSwaggerConfiguration();
app.UseCorsConfiguration(app.Configuration);
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();