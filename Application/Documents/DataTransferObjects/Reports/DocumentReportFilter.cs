using Application.Shared.DataTransferObjects.Listings;

namespace Application.Documents.DataTransferObjects.Reports
{
    public abstract class DocumentReportFilter : ListingFilter
    {
        protected DocumentReportFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        protected DocumentReportFilter() : base() { }

        public DateTime? CreationDateTimeStart { get; init; }

        public DateTime? CreationDateTimeEnd { get; init; }

        public DateTime? LastUpdateDateTimeStart { get; init; }

        public DateTime? LastUpdateDateTimeEnd { get; init; }
    }
}
