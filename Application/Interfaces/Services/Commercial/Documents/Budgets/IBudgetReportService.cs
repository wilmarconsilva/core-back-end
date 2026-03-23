using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Reports;

namespace Application.Interfaces.Services.Commercial.Documents.Budgets
{
    public interface IBudgetReportService : IReportService<BudgetReportListing, BudgetReportFilter>
    {

    }
}
