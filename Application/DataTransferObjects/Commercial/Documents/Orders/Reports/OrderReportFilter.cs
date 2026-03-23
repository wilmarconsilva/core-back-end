using Application.DataTransferObjects.Commercial.Documents.Reports;

namespace Application.DataTransferObjects.Commercial.Documents.Orders.Reports
{
    public class OrderReportFilter : DocumentReportFilter
    {
        public OrderReportFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public OrderReportFilter() : base() { }
    }
}
