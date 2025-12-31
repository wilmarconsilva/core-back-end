using FluentValidation;
using System.Linq.Expressions;
using System.Numerics;

namespace Application.DataTransferObjects
{
    public abstract class Validator<T> : AbstractValidator<T>
    {
        #region Fields

        private const string _requiredMessage = "O campo: {PropertyName} deve ser preenchido.";

        #endregion Fields

        #region Protected methods

        protected IRuleBuilderOptions<T, string> RequiredString(Expression<Func<T, string>> expression)
        {
            return RuleFor(expression)
                    .NotEmpty()
                    .WithMessage(_requiredMessage);
        }

        protected void RequiredNumber<TProperty>(Expression<Func<T, TProperty>> expression) where TProperty : struct, INumber<TProperty>
        {
            RuleFor(expression)
                .Must(value => value > TProperty.Zero)
                .WithMessage(_requiredMessage);
        }

        #endregion Protected methods
    }
}
