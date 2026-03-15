using FluentValidation;
using System.Linq.Expressions;
using System.Numerics;

namespace Application.DataTransferObjects
{
    public abstract class Validator<T> : AbstractValidator<T>
    {
        #region Fields

        protected const string _requiredMessage = "O campo: {PropertyName} deve ser preenchido com um valor válido.";

        #endregion Fields

        #region Protected methods

        protected IRuleBuilderOptions<T, string> RequiredString(Expression<Func<T, string>> expression)
        {
            return RuleFor(expression)
                    .NotEmpty()
                    .WithMessage(_requiredMessage);
        }

        protected IRuleBuilderOptions<T, TProperty> RequiredNumber<TProperty>(Expression<Func<T, TProperty>> expression) where TProperty : INumber<TProperty>
        {
            return RuleFor(expression)
                    .NotEmpty()
                    .WithMessage(_requiredMessage);
        }

        protected IRuleBuilderOptions<T, TEnum> RequiredEnum<TEnum>(Expression<Func<T, TEnum>> expression) where TEnum : struct, Enum
        {
            return RuleFor(expression)
                    .IsInEnum()
                    .WithMessage(_requiredMessage);
        }

        #endregion Protected methods
    }
}
