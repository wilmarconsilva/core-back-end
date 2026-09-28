using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Reports;

namespace Application.Interfaces.Services.Commercial.Documents.Budgets
{
    public interface IBudgetReportService : IReportService<BudgetReportListing, BudgetReportFilter>
    {

    }
}
