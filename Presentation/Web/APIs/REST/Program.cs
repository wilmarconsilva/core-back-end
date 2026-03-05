using Presentation.Web.APIs.REST.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.AddLoggingConfiguration();

// Add Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

WebApplication app = builder.Build();

// Configure Middleware
app.UseSwaggerConfiguration();
app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();
