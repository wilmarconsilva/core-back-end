namespace Application.Shared.DataTransferObjects.Listings
{
    public abstract class Listing<TRegister, TSummary> where TRegister : class where TSummary : class
    {
        public required List<TRegister> Registers { get; set; }

        public TSummary? Summary { get; set; }
    }
}