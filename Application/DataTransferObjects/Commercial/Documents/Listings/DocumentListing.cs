using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Commercial.Documents.Listings
{
    public abstract class DocumentListing : EntityListing
    {
        public required string CustormerName { get; init; }

        public required string CustomerCityState { get; init; }

        public required decimal ValueTotal { get; init; }
    }
}
