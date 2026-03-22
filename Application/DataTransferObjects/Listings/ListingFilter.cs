using Domain.Exceptions;
namespace Application.DataTransferObjects.Listings
{
    public abstract class ListingFilter
    {
        private const int RegistersStartDefault = 0;
        private const int RegistersEndDefault = 50;
        private const int MaxRegistersLimit = 50;

        private int _registersStart = RegistersStartDefault;
        private int _registersEnd = RegistersEndDefault;

        protected ListingFilter()
        {

        }

        protected ListingFilter(int registersStart, int registersEnd)
        {
            SetRegistersStart(registersStart);
            SetRegistersEnd(registersEnd);
        }

        public int RegistersStart { get => _registersStart; init => SetRegistersStart(value); }

        public int RegistersEnd { get => _registersEnd; init => SetRegistersEnd(value); }

        public void SetRegistersStart(int registersStart)
        {
            if (registersStart >= RegistersStartDefault)
                _registersStart = registersStart;
            else
                throw new BusinessRuleException(GetMinValueExceptionMessage(registersStart, RegistersStartDefault));
        }

        public void SetRegistersEnd(int registersEnd)
        {
            if (registersEnd < 1)
                throw new BusinessRuleException(GetMinValueExceptionMessage(registersEnd, 1));
            else if (registersEnd > MaxRegistersLimit)
                throw new BusinessRuleException(GetMaxValueExceptionMessage(registersEnd, MaxRegistersLimit));
            else
                _registersEnd = registersEnd;
        }

        private static string GetMinValueExceptionMessage(int value, int minAllowwed)
        {
            return $"O valor: {value} é menor que o mínimo permitido. O mínimo permitido é: {minAllowwed}";
        }

        private static string GetMaxValueExceptionMessage(int value, int maxAllowwed)
        {
            return $"O valor: {value} é maior que o máximo permitido. O máximo permitido é: {maxAllowwed}";
        }
    }
}
