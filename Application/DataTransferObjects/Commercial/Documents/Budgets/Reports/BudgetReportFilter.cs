using Application.DataTransferObjects.Commercial.Documents.Reports;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Reports
{
    public class BudgetReportFilter : DocumentReportFilter
    {
        public BudgetReportFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public BudgetReportFilter() : base() { }
    }
}
