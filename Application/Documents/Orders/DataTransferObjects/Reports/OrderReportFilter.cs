using Application.Documents.DataTransferObjects.Reports;

namespace Application.Documents.Orders.DataTransferObjects.Reports
{
    public class OrderReportFilter : DocumentReportFilter
    {
        public OrderReportFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public OrderReportFilter() : base() { }
    }
}
