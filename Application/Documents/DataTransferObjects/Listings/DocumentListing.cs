using Application.Shared.DataTransferObjects.Entities;

namespace Application.Documents.DataTransferObjects.Listings
{
    public abstract class DocumentListing : EntityListing
    {
        public string CustormerName { get; init; } = null!;

        public string CustomerLocalityState { get; init; } = null!;

        public DateTime CreationDateTime { get; init; }

        public decimal ValueTotal { get; init; }
    }
}
