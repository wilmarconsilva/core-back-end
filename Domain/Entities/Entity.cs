using Domain.Exceptions;
using System.Numerics;

namespace Domain.Entities
{
    public abstract class Entity
    {
        #region Public properties

        public long Id { get; set; }

        public abstract string Description { get; set; }

        #endregion Public properties

        #region Protected methods

        protected static void Validate(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessRuleException($"{propertyName} não pode ser vazio.");
        }

        protected static void Validate<TProperty>(TProperty value, string propertyName) where TProperty : INumber<TProperty>
        {
            if (value == TProperty.Zero)
                throw new BusinessRuleException($"{propertyName} não pode ser zero.");
        }

        #endregion Protected methods
    }
}