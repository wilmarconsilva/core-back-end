using Domain.Exceptions;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Domain.Utilities.Entities
{
    internal static class Validator
    {
        private static readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        internal static void ValidateValue(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessRuleException($"{propertyName} não pode ser vazio.");
        }

        internal static void ValidateValue<TProperty>(TProperty value, string propertyName) where TProperty : INumber<TProperty>
        {
            if (value == TProperty.Zero)
                throw new BusinessRuleException($"{propertyName} não pode ser zero.");
        }

        internal static void ValidateEnum<TEnum>(TEnum value, string propertyName) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
                throw new BusinessRuleException($"{propertyName} contém um valor inválido.");
        }

        internal static void ValidateValue(DateTime value, string propertyName)
        {
            if (value == DateTime.MinValue)
                throw new BusinessRuleException($"{propertyName} contém uma data inválida.");
        }

        internal static void ValidateValue(IEnumerable<byte> value, string propertyName)
        {
            if (value == null || !value.Any())
                throw new BusinessRuleException($"{propertyName} não pode ser vazio.");
        }

        internal static void ValidateEmail(string email, string propertyName)
        {
            if (!_emailRegex.IsMatch(email))
                throw new BusinessRuleException($"{propertyName} não é um endereço de e-mail válido.");
        }
    }
}
