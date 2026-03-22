using Application.DataTransferObjects.Commercial.Documents.Budgets.Validators;
using Application.DataTransferObjects.Commercial.Documents.Payments;
using Application.DataTransferObjects.Commercial.Documents.Payments.Installments;
using Application.DataTransferObjects.Persons.Customers.Validators;
using Application.Interfaces.Queries;
using Application.Interfaces.Queries.Commercial.Documents.Budgets;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Application.Interfaces.Queries.Persons.Customers;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Commercial.Documents;
using Application.Interfaces.Services.Persons;
using Application.Mappings.Persons.Customers;
using Application.Services.Commercial.Documents;
using Application.Services.Persons.Customers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Queries;
using Infrastructure.Queries.Commercial.Documents.Budgets;
using Infrastructure.Queries.Commercial.Documents.Orders;
using Infrastructure.Queries.Persons.Customers;
using Application.Services.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents.Orders;

namespace Presentation.Web.APIs.REST.Extensions;

/// <summary>
/// Extensões para registrar serviços da camada Application.
/// </summary>
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        AddValidation(services);
        AddAutoMapper(services);
        AddCustomerServices(services);
        AddProductServices(services);
        AddBudgetServices(services);
        AddOrderServices(services);

        return services;
    }

    private static void AddValidation(IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CustomerSaveDtoValidator>();
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CustomerProfile));
    }

    private static void AddCustomerServices(IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICustomerQuery, CustomerQuery>();
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
    }

    private static void AddOrderServices(IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderReportService, OrderReportService>();
        services.AddScoped<IOrderQuery, OrderQuery>();
        services.AddScoped<IOrderReportQuery, OrderReportQuery>();
    }
}
