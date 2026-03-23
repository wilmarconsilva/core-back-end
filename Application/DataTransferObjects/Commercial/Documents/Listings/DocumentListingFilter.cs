using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Commercial.Documents.Listings
{
    public abstract class DocumentListingFilter : EntityListingFilter
    {
        protected DocumentListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        protected DocumentListingFilter() : base() { }

        public DateTime? CreationDateTimeStart { get; init; }

        public DateTime? CreationDateTimeEnd { get; init; }

        public long? CustomerId { get; init; }

        public long? LocalityId { get; init; }
    }
}
