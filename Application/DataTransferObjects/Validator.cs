using FluentValidation;
using System.Linq.Expressions;
using System.Numerics;

namespace Application.DataTransferObjects
{
    public abstract class Validator<T> : AbstractValidator<T>
    {
        #region Fields

        private const string _requiredMessage = "O campo: {PropertyName} deve ser preenchido com um valor válido.";

        protected const string _requiredEnumerableMessage = "É obrigatório informar ao menos um {PropertyName}";

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

        protected IRuleBuilderOptions<T, Enum> RequiredEnum(Expression<Func<T, Enum>> expression)
        {
            return RuleFor(expression)
                    .IsInEnum()
                    .WithMessage(_requiredMessage);
        }

        #endregion Protected methods
    }
}
