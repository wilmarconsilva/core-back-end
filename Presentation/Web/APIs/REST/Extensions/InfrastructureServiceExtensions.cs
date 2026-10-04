using Infrastructure.Security;
using Infrastructure.Email;
using Infrastructure.Authentication.JsonWebTokens;
using Microsoft.EntityFrameworkCore;
using Domain.Shared.Persistance.Interfaces;
using Domain.Documents.Budgets.Repositories.Interfaces;
using Domain.Documents.Orders.Repositories.Interfaces;
using Domain.Products.Repositories.Interfaces;
using Domain.Persons.Customers.Repositories.Interfaces;
using Domain.Authtentication.Repositories.Interfaces;
using Domain.Persons.Users.Repositories.Interfaces;
using Application.Authentication.Interfaces;
using Application.Authentication.Services.Interfaces;
using Infrastructure.Shared.Persistance;
using Application.Email.Services.Interfaces;
using Infrastructure.Documents.Budgets.Repositories;
using Infrastructure.Documents.Orders;
using Infrastructure.Persons.Customers;
using Infrastructure.Persons.Users;
using Infrastructure.Authentication;
using Infrastructure.Products;

namespace REST.Extensions;

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
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddSecurity(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IHttpContextCurrentUser, HttpContextCurrentUser>();
    }

    private static void AddEmail(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));

        services.AddOptions<EmailOptions>()
            .Bind(configuration.GetSection(EmailOptions.SectionName))
            .Validate(emailOptions =>
            {
                if (!emailOptions.Enabled)
                    return true;

                return !string.IsNullOrWhiteSpace(emailOptions.Host)
                    && emailOptions.Port.HasValue
                    && !string.IsNullOrWhiteSpace(emailOptions.SenderEmail);
            }, $"When {EmailOptions.SectionName}__{nameof(EmailOptions.Enabled)} is true, all required email settings must be provided by environment variables or .env file.")
            .ValidateOnStart();

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
