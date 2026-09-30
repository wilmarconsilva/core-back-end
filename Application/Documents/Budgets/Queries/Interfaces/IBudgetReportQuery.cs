using Application.Documents.Budgets.DataTransferObjects.Reports;
using Application.Shared.Queries.Interfaces;

namespace Application.Documents.Budgets.Queries.Interfaces
{
    public interface IBudgetReportQuery : IReportQuery<BudgetReport, BudgetReportFilter>
    {
    }
}
