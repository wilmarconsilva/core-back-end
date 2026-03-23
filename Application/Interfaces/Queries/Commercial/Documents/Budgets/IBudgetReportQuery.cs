using Application.DataTransferObjects.Commercial.Documents.Budgets.Reports;

namespace Application.Interfaces.Queries.Commercial.Documents.Budgets
{
    public interface IBudgetReportQuery : IReportQuery<BudgetReport, BudgetReportFilter>
    {
    }
}
