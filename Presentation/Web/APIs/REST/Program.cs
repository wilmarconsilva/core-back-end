using Presentation.Web.APIs.REST.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
