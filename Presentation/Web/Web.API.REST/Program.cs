WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
string tituloDefinicaoSwagger = "Core - Web API REST";
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = tituloDefinicaoSwagger,
        Version = "v1",
        Description = string.Empty,
    });
});

WebApplication app = builder.Build();

if (true /*autenticar usuário*/)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", tituloDefinicaoSwagger);
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapGet("/Test", () =>
{
    return "oie";
});

app.Run();
