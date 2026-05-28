using Application.Interfaces.Security;
using Infrastructure.Security;
using Application.Interfaces.Services;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Infrastructure.Email;
using Infrastructure.Persistece;
using Infrastructure.Repositories.Commercial.Documents.Budgets;
using Infrastructure.Repositories.Commercial.Documents.Orders;
using Infrastructure.Repositories.Persons;
using Infrastructure.Repositories.Persons.Customers;
using Infrastructure.Repositories.Security;
using Infrastructure.Security.JsonWebTokens;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Web.APIs.REST.Extensions;

/// <summary>
/// Extensões para registrar serviços de persistência e infraestrutura.
/// </summary>
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        AddPersistence(services, configuration);
        AddSecurity(services, configuration);
        AddEmail(services, configuration);
        AddRepositories(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddSecurity(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.Configure<Options>(configuration.GetSection(Options.SectionName));
        services.AddScoped<IJwtTokenGenerator, TokenGenerator>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IHttpContextCurrentUser, HttpContextCurrentUser>();
    }

    private static void AddEmail(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
        services.AddScoped<IEmailService, SmtpEmailService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
}
