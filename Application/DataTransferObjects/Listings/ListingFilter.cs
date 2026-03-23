using Domain.Exceptions;
using System.ComponentModel;
namespace Application.DataTransferObjects.Listings
{
    public abstract class ListingFilter
    {
        private const int RegistersStartDefault = 0;
        private const int RegistersEndDefault = 50;
        private const int MaxRegistersLimit = 50;

        private int _registersStart;
        private int _registersEnd;

        protected ListingFilter(int registersStart, int registersEnd)
        {
            RegistersStart = registersStart;
            RegistersEnd = registersEnd;
        }

        protected ListingFilter() : this(RegistersStartDefault, RegistersEndDefault)
        {

        }

        [DefaultValue(RegistersStartDefault)]
        public int RegistersStart { get => _registersStart; set => SetRegistersStart(value); }

        [DefaultValue(RegistersEndDefault)]
        public int RegistersEnd { get => _registersEnd; set => SetRegistersEnd(value); }

        public void SetRegistersStart(int registersStart)
        {
            if (registersStart >= RegistersStartDefault)
                _registersStart = registersStart;
            else
                throw new BusinessRuleException(GetMinValueExceptionMessage(registersStart, RegistersStartDefault));

            ValidateRegistersQuantity();
        }

        public void SetRegistersEnd(int registersEnd)
        {
            if (registersEnd < 1)
                throw new BusinessRuleException(GetMinValueExceptionMessage(registersEnd, 1));
            else
                _registersEnd = registersEnd;

            ValidateRegistersQuantity();
        }

        private void ValidateRegistersQuantity()
        {
            int registersReturnedQuantity = Math.Abs(RegistersEnd - RegistersStart);
            if (registersReturnedQuantity > MaxRegistersLimit)
                throw new BusinessRuleException($"A quantidade de registros a ser retornada: {registersReturnedQuantity} excede a máxima permitida. A máxima permitida é de: {MaxRegistersLimit}");
        }

        private static string GetMinValueExceptionMessage(int value, int minAllowwed)
        {
            return $"O valor: {value} é menor que o mínimo permitido. O mínimo permitido é: {minAllowwed}";
        }
    }
}
