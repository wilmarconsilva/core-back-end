using Application.DataTransferObjects.Persons.Customers.Validators;
using Application.Interfaces.Queries.Persons.Customers;
using Application.Interfaces.Services.Persons;
using Application.Mappings.Persons.Customers;
using Application.Services.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Persistece;
using Infrastructure.Queries.Persons.Customers;
using Infrastructure.Repositories.Persons.Customers;
using Microsoft.EntityFrameworkCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

string tituloDefinicaoSwagger = "Core - Web API REST";
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = tituloDefinicaoSwagger,
        Version = "v1",
        Description = string.Empty,
    });

    string xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    setup.IncludeXmlComments(xmlPath);
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerSaveDtoValidator>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<ICustomerQuery, CustomerQuery>();

builder.Services.AddAutoMapper(typeof(CustomerAddressProfile));

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

app.MapControllers();

await app.RunAsync();
