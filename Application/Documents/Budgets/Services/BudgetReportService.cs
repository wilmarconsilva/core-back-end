using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Reports;
using Application.Documents.Budgets.Queries.Interfaces;
using Application.Documents.Budgets.Services.Interfaces;

namespace Application.Documents.Budgets.Services
{
    public class BudgetReportService : IBudgetReportService
    {
        private readonly IBudgetReportQuery _query;

        public BudgetReportService(IBudgetReportQuery budgetReportQuery)
        {
            _query = budgetReportQuery;
        }

        public async Task<BudgetReportListing> Get(BudgetReportFilter filter, CancellationToken cancellationToken)
        {
            List<BudgetReport> reports = await _query.GetAsync(filter, cancellationToken);

            return new BudgetReportListing
            {
                Registers = reports,
                Summary = new BudgetReportSummary
                {
                    ValueTotal = reports.Sum(r => r.ValueTotal),
                }
            };
        }
    }
}
