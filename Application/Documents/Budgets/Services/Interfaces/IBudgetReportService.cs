using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Reports;
using Application.Shared.Services.Interfaces;

namespace Application.Documents.Budgets.Services.Interfaces
{
    public interface IBudgetReportService : IReportService<BudgetReportListing, BudgetReportFilter>
    {

    }
}
