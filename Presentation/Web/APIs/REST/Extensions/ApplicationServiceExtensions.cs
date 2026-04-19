using Application.DataTransferObjects.Persons.Customers.Validators;
using Application.Interfaces.Queries;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Application.Interfaces.Queries.Commercial.Documents.Budgets;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Application.Interfaces.Queries.Persons.Customers;
using Application.Interfaces.Queries.Territories;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Commercial.Documents;
using Application.Interfaces.Services.Persons;
using Application.Interfaces.Services.Territories;
using Application.Mappings.Persons.Customers;
using Application.Services.Persons.Customers;
using Application.Services.Territories;
using FluentValidation;
using Infrastructure.Queries;
using Infrastructure.Queries.Commercial.Documents.Budgets;
using Infrastructure.Queries.Commercial.Documents.Orders;
using Infrastructure.Queries.Persons.Customers;
using Infrastructure.Queries.Territories;
using Infrastructure.Repositories.Territories;
using Application.Services.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents.Budgets;
using Application.Services.Commercial.Documents.Budgets;
using Domain.Interfaces.Repositories;
using Application.Interfaces.Services.Security;
using Application.Services.Security;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Presentation.Web.APIs.REST.Validations;

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
        AddLocalityServices(services);
        AddSecurityServices(services);

        return services;
    }

    private static void AddValidation(IServiceCollection services)
    {
        services.AddScoped<FluentValidationAction>();
        services.Configure<MvcOptions>(options => options.Filters.AddService<FluentValidationAction>());
        AddValidatorsFromAssemblyContaining<CustomerSaveDtoValidator>(services);
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(kvp => kvp.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(GetErrorMessage).ToArray()
                    );

                return new BadRequestObjectResult(new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest
                });
            };
        });
    }

    private static void AddValidatorsFromAssemblyContaining<TValidator>(IServiceCollection services)
    {
        Type validatorInterface = typeof(IValidator<>);

        IEnumerable<Type> validatorTypes = typeof(TValidator).Assembly
            .GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .Where(type => type.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == validatorInterface));

        foreach (Type validatorType in validatorTypes)
        {
            services.TryAddScoped(validatorType, validatorType);

            foreach (Type serviceType in validatorType.GetInterfaces()
                         .Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == validatorInterface))
            {
                services.TryAddScoped(serviceType, validatorType);
            }
        }
    }

    private static string GetErrorMessage(ModelError error)
    {
        Exception? exception = error.Exception;
        while (exception != null)
        {
            if (exception is BusinessRuleException bre)
                return bre.Message;
            exception = exception.InnerException;
        }
        return string.IsNullOrEmpty(error.ErrorMessage) ? "The input was not valid." : error.ErrorMessage;
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
