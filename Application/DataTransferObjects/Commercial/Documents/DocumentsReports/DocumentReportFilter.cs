using Application.DataTransferObjects.Listings;

namespace Application.DataTransferObjects.Commercial.Documents.DocumentsReports
{
    public abstract class DocumentReportFilter : ListingFilter
    {
        protected DocumentReportFilter() : base()
        {

        }

        protected DocumentReportFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd)
        {

        }

        public DateTime? CreationDateTimeStart { get; init; }

        public DateTime? CreationDateTimeEnd { get; init; }

        public DateTime? LastUpdateDateTimeStart { get; init; }

        public DateTime? LastUpdateDateTimeEnd { get; init; }
    }
}
