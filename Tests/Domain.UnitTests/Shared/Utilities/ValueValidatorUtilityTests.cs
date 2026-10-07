using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Persons.Enums;
using Domain.Shared.Exceptions;
using Domain.Shared.Utilities;

namespace Domain.UnitTests.Shared.Utilities
{
    public class ValueValidatorUtilityTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateValue_EmptyString_ThrowsBusinessRuleException(string? value)
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateValue(value!, "Nome"));

            exception.Message.ShouldBe("O campo: Nome não pode ser vazio.");
        }

        [Fact]
        public void ValidateValue_FilledString_DoesNotThrow()
        {
            Should.NotThrow(() => ValueValidatorUtility.ValidateValue("Valor", "Nome"));
        }

        [Fact]
        public void ValidateValue_ZeroNumber_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateValue(0m, "Valor"));

            exception.Message.ShouldBe("O campo: Valor não pode ser zero.");
        }

        [Fact]
        public void ValidateValue_NonZeroNumber_DoesNotThrow()
        {
            Should.NotThrow(() => ValueValidatorUtility.ValidateValue(1L, "Código"));
        }

        [Fact]
        public void ValidateNotNegative_NegativeNumber_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateNotNegative(-0.01m, "Desconto"));

            exception.Message.ShouldBe("O campo: Desconto não pode ser negativo.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void ValidateNotNegative_ZeroOrPositiveNumber_DoesNotThrow(decimal value)
        {
            Should.NotThrow(() => ValueValidatorUtility.ValidateNotNegative(value, "Desconto"));
        }

        [Theory]
        [InlineData(0, "O campo: Quantidade não pode ser zero.")]
        [InlineData(-1, "O campo: Quantidade não pode ser negativo.")]
        public void ValidatePositive_ZeroOrNegativeNumber_ThrowsBusinessRuleException(decimal value, string expectedMessage)
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidatePositive(value, "Quantidade"));

            exception.Message.ShouldBe(expectedMessage);
        }

        [Fact]
        public void ValidatePositive_PositiveNumber_DoesNotThrow()
        {
            Should.NotThrow(() => ValueValidatorUtility.ValidatePositive(0.01m, "Quantidade"));
        }

        [Fact]
        public void ValidateValue_MinDateTime_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateValue(DateTime.MinValue, "Data"));

            exception.Message.ShouldBe("O campo: Data contém uma data inválida.");
        }

        [Fact]
        public void ValidateValue_EmptyBytes_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateValue(Array.Empty<byte>(), "Imagem"));
        }

        [Fact]
        public void ValidateValue_NullBytes_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateValue((IEnumerable<byte>)null!, "Imagem"));
        }

        [Fact]
        public void ValidateEnum_UndefinedValue_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateEnum((PaymentMethod)0, "Método"));

            exception.Message.ShouldBe("O campo: Método contém um valor inválido.");
        }

        [Fact]
        public void ValidateEnum_DefinedValue_DoesNotThrow()
        {
            Should.NotThrow(() => ValueValidatorUtility.ValidateEnum(PaymentMethod.PIX, "Método"));
        }

        [Theory]
        [InlineData("cliente@email.com")]
        [InlineData("nome.sobrenome@empresa.com.br")]
        public void ValidateEmail_ValidEmail_DoesNotThrow(string email)
        {
            Should.NotThrow(() => ValueValidatorUtility.ValidateEmail(email, "E-mail"));
        }

        [Theory]
        [InlineData("cliente")]
        [InlineData("cliente@")]
        [InlineData("cliente@email")]
        [InlineData("cli ente@email.com")]
        public void ValidateEmail_InvalidEmail_ThrowsBusinessRuleException(string email)
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateEmail(email, "E-mail"));

            exception.Message.ShouldBe("O campo: E-mail contém um endereço de e-mail inválido.");
        }

        [Theory]
        [InlineData("52998224725")]
        [InlineData("529.982.247-25")]
        [InlineData("111.444.777-35")]
        public void ValidateCpfCnpj_ValidCpf_DoesNotThrow(string cpf)
        {
            Should.NotThrow(() => ValueValidatorUtility.ValidateCpfCnpj(cpf, "CPF", PersonType.Natural));
        }

        [Theory]
        [InlineData("52998224724")]
        [InlineData("11111111111")]
        [InlineData("5299822472")]
        [InlineData("529982247250")]
        public void ValidateCpfCnpj_InvalidCpf_ThrowsBusinessRuleException(string cpf)
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateCpfCnpj(cpf, "CPF", PersonType.Natural));

            exception.Message.ShouldBe("O campo: CPF contém um CPF inválido.");
        }

        [Fact]
        public void ValidateCpfCnpj_EmptyDocument_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => ValueValidatorUtility.ValidateCpfCnpj("", "CNPJ", PersonType.Legal));
        }

        [Fact]
        public void ValidateCpfCnpj_LegalPerson_DoesNotValidateCnpjDigits()
        {
            // Comportamento atual: o CNPJ não tem os dígitos verificadores validados.
            Should.NotThrow(() => ValueValidatorUtility.ValidateCpfCnpj("12345", "CNPJ", PersonType.Legal));
        }
    }
}
