using Application.DataTransferObjects.Commercial.Documents.DocumentsReports;

namespace Application.DataTransferObjects.Commercial.Documents.Orders.Reports
{
    public class OrderReportFilter : DocumentReportFilter
    {
        protected OrderReportFilter() : base()
        {

        }

        protected OrderReportFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd)
        {

        }
    }
}
