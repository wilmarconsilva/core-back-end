using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Commercial.Documents.Listings
{
    public abstract class DocumentListing : EntityListing
    {
        public string CustormerName { get; init; } = null!;

        public string CustomerLocalityState { get; init; } = null!;

        public decimal ValueTotal { get; init; }
    }
}
