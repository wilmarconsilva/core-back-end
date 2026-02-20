using Domain.Exceptions;
using System.Numerics;

namespace Domain.Entities
{
    public abstract class Entity
    {
        #region Public properties

        public long Id { get; set; }

        public abstract string Description { get; protected set; }

        #endregion Public properties

        #region Protected methods

        protected static void ValidateValue(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessRuleException($"{propertyName} não pode ser vazio.");
        }

        protected static void ValidateValue<TProperty>(TProperty value, string propertyName) where TProperty : INumber<TProperty>
        {
            if (value == TProperty.Zero)
                throw new BusinessRuleException($"{propertyName} não pode ser zero.");
        }

        protected static void ValidateEnum<TEnum>(TEnum value, string propertyName) where TEnum : Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
                throw new BusinessRuleException($"{propertyName} possui um valor inválido.");
        }

        #endregion Protected methods
    }
}