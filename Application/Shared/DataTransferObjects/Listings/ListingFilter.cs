using Domain.Shared.Exceptions;
namespace Application.Shared.DataTransferObjects.Listings
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

        public int RegistersStart { get => GetRegistersStart(); set => SetRegistersStart(value); }

        public int RegistersEnd { get => GetRegistersEnd(); set => SetRegistersEnd(value); }

        public int RegistersQuantity => GetRegistersQuantity();

        private int GetRegistersStart()
        {
            ValidateRegistersQuantity();

            return _registersStart;
        }

        private int GetRegistersEnd()
        {
            ValidateRegistersQuantity();

            return _registersEnd;
        }

        private void SetRegistersStart(int registersStart)
        {
            if (registersStart >= RegistersStartDefault)
                _registersStart = registersStart;
            else
                throw new BusinessRuleException(GetMinValueExceptionMessage(registersStart, RegistersStartDefault));
        }

        private void SetRegistersEnd(int registersEnd)
        {
            if (registersEnd < 1)
                throw new BusinessRuleException(GetMinValueExceptionMessage(registersEnd, 1));
            else
                _registersEnd = registersEnd;
        }

        private int ValidateRegistersQuantity()
        {
            int registersReturnedQuantity = Math.Abs(_registersEnd - _registersStart);
            if (registersReturnedQuantity > MaxRegistersLimit)
                throw new BusinessRuleException($"A quantidade de registros a ser retornada: {registersReturnedQuantity} excede a máxima permitida. A máxima permitida é de: {MaxRegistersLimit}");

            return registersReturnedQuantity;
        }

        private int GetRegistersQuantity()
        {
            int registersReturnedQuantity = ValidateRegistersQuantity();

            return registersReturnedQuantity;
        }

        private static string GetMinValueExceptionMessage(int value, int minAllowwed)
        {
            return $"O valor: {value} é menor que o mínimo permitido. O mínimo permitido é: {minAllowwed}";
        }
    }
}
