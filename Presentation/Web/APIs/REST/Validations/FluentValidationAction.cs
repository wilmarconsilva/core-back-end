using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Presentation.Web.APIs.REST.Validations;

public sealed class FluentValidationAction(
    IServiceProvider serviceProvider,
    IOptions<ApiBehaviorOptions> apiBehaviorOptions) : IAsyncActionFilter, IOrderedFilter
{
    public int Order => int.MinValue;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (object? argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            Type validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (serviceProvider.GetService(validatorType) is not IValidator validator)
                continue;

            ValidationContext<object> validationContext = new(argument);
            ValidationResult validationResult = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

            foreach (ValidationFailure error in validationResult.Errors)
            {
                context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = apiBehaviorOptions.Value.InvalidModelStateResponseFactory(context);
            return;
        }

        await next();
    }
}
