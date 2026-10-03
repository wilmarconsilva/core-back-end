using Domain.Territories.Localities.Repositories.Interfaces;
using Application.Authentication.Services.Interfaces;
using Application.Authentication.Services;
using Application.Documents.Budgets.Services;
using Application.Documents.Orders.Services;
using Application.Persons.Customers.Services;
using Application.Products.Services;
using Application.Territories.Localities.Services;
using Infrastructure.Documents.Budgets.Queries;
using Infrastructure.Documents.Orders.Queries;
using Infrastructure.Persons.Customers.Queries;
using Infrastructure.Territories.Localities.Queries;
using Infrastructure.Products.Queries;
using Application.Persons.Customers.Queries;
using Application.Documents.Orders.Queries;
using Application.Documents.Budgets.Queries.Interfaces;
using Application.Territories.Localities.Queries.Interfaces;
using Application.Shared.Queries.Interfaces;
using Application.Documents.Budgets.Services.Interfaces;
using Application.Documents.Orders.Services.Interfaces;
using Application.Persons.Customers.Services.Interfaces;
using Application.Territories.Localities.Services.Interfaces;
using Application.Products.Services.Interfaces;
using Application.Persons.Customers.Mappings;
using Infrastructure.Territories.Localities;
using Application.Persons.Users.Queries.Interfaces;
using Application.Persons.Users.Services;
using Application.Persons.Users.Services.Interfaces;
using Infrastructure.Persons.Users.Queries;

namespace REST.Extensions;

/// <summary>
/// Extensões para registrar serviços da camada Application.
/// </summary>
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        AddAutoMapper(services);
        AddCustomerServices(services);
        AddUserServices(services);
        AddProductServices(services);
        AddBudgetServices(services);
        AddOrderServices(services);
        AddLocalityServices(services);
        AddSecurityServices(services);

        return services;
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(CustomerProfile).Assembly);
    }

    private static void AddCustomerServices(IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICustomerQuery, CustomerQuery>();
    }

    private static void AddUserServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserQuery, UserQuery>();
    }

    private static void AddProductServices(IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductQuery, ProductQuery>();
    }

    private static void AddBudgetServices(IServiceCollection services)
    {
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IBudgetQuery, BudgetQuery>();
        services.AddScoped<IBudgetReportService, BudgetReportService>();
        services.AddScoped<IBudgetReportQuery, BudgetReportQuery>();
    }

    private static void AddOrderServices(IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderReportService, OrderReportService>();
        services.AddScoped<IOrderQuery, OrderQuery>();
        services.AddScoped<IOrderReportQuery, OrderReportQuery>();
    }

    private static void AddLocalityServices(IServiceCollection services)
    {
        services.AddScoped<ILocalityService, LocalityService>();
        services.AddScoped<ILocalityQuery, LocalityQuery>();
        services.AddScoped<ILocalityRepository, LocalityRepository>();
    }

    private static void AddSecurityServices(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}
