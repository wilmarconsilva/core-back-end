using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;
using Infrastructure.Repositories.Commercial.Documents.Budgets;
using Infrastructure.Repositories.Persons.Customers;
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
        AddRepositories(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
    }
}
