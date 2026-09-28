using Domain.Exceptions;
using Domain.Persons.Enums;
using Domain.Shared.Extensions;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Domain.Shared.Utilities
{
    internal static class ValueValidatorUtility
    {
        private static readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        internal static void ValidateValue(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessRuleException(GetInvalidationMessage(propertyName, "não pode ser vazio."));
        }

        internal static void ValidateValue<TProperty>(TProperty value, string propertyName) where TProperty : INumber<TProperty>
        {
            if (value == TProperty.Zero)
                throw new BusinessRuleException(GetInvalidationMessage(propertyName, "não pode ser zero."));
        }

        internal static void ValidateValue(DateTime value, string propertyName)
        {
            if (value == DateTime.MinValue)
                throw new BusinessRuleException(GetInvalidationMessage(propertyName, "contém uma data inválida."));
        }

        internal static void ValidateValue(IEnumerable<byte> value, string propertyName)
        {
            if (value == null || !value.Any())
                throw new BusinessRuleException(GetInvalidationMessage(propertyName, "não pode ser vazio."));
        }

        internal static void ValidateEnum<TEnum>(TEnum value, string propertyName) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
                throw new BusinessRuleException(GetInvalidationMessage(propertyName, "contém um valor inválido."));
        }

        internal static void ValidateEmail(string email, string propertyName)
        {
            if (!_emailRegex.IsMatch(email))
                throw new BusinessRuleException(GetInvalidationMessage(propertyName, "contém um endereço de e-mail inválido."));
        }

        internal static void ValidateCpfCnpj(string value, string propertyName, PersonType personType)
        {
            ValidateValue(value, propertyName);

            string numeric = value.OnlyNumbers();

            if (personType == PersonType.Natural)
            {
                if (!IsValidCpf(numeric))
                    throw new BusinessRuleException(GetInvalidationMessage(propertyName, "contém um CPF inválido."));
            }
            else if (personType == PersonType.Legal)
            {
                if (!IsValidCnpj(numeric))
                    throw new BusinessRuleException(GetInvalidationMessage(propertyName, "contém um CNPJ inválido."));
            }
        }

        private static string GetInvalidationMessage(string propertyName, string message)
        {
            return $"O campo: {propertyName} {message}";
        }

        private static bool IsValidCpf(string cpf)
        {
            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            int[]? numbers = cpf.Select(c => int.Parse(c.ToString())).ToArray();

            // Primeiro dígito
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += numbers[i] * (10 - i);

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            // Segundo dígito
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += numbers[i] * (11 - i);

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return numbers[9] == digit1 && numbers[10] == digit2;
        }

        private static bool IsValidCnpj(string cnpj)
        {
            //Contemplar a regra do alfanumérico.
            return true;
        }
    }
}
