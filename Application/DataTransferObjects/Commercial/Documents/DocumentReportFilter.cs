namespace Application.DataTransferObjects.Commercial.Documents
{
    public class DocumentReportFilter
    {
        public DateTime? CreationDateTimeStart { get; set; }

        public DateTime? CreationDateTimeEnd { get; set; }

        public DateTime? LastUpdateDateTimeStart { get; set; }

        public DateTime? LastUpdateDateTimeEnd { get; set; }
    }
}
