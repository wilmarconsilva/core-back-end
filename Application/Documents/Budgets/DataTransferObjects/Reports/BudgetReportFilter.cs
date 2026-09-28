using Application.Documents.DataTransferObjects.Reports;

namespace Application.Documents.Budgets.DataTransferObjects.Reports
{
    public class BudgetReportFilter : DocumentReportFilter
    {
        public BudgetReportFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public BudgetReportFilter() : base() { }
    }
}
