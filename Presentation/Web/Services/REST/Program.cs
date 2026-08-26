using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using REST.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Carrega o .env único do repositório. TraversePath localiza o .env na raiz mesmo rodando a partir desta pasta do projeto; NoClobber garante
// que uma variável de ambiente real do processo/deploy sempre prevalece sobre o valor do .env.
builder.Configuration.AddDotNetEnv(options: LoadOptions.TraversePath().NoClobber());

// Configure Logging
builder.AddLoggingConfiguration();

// Add Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddCorsConfiguration(builder.Configuration, builder.Environment);
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

// Aplica migrations pendentes antes de aceitar requisições.
await app.ApplyMigrationsAsync();

// Configure Middleware

// Deploy roda atrás de um proxy que termina o TLS (ex.: Render) e repassa HTTP puro ao Kestrel
// sem isso, Request.IsHttps fica sempre false e o cookie do refresh token nunca recebe Secure=true.
if (app.Environment.IsProduction())
{
    ForwardedHeadersOptions forwardedHeadersOptions = new()
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    };
    forwardedHeadersOptions.KnownIPNetworks.Clear();
    forwardedHeadersOptions.KnownProxies.Clear();
    app.UseForwardedHeaders(forwardedHeadersOptions);
}

app.UseSwaggerConfiguration();
app.UseCorsConfiguration();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health check.
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

await app.RunAsync();