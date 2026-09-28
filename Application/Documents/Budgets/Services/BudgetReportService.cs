using Application.DataTransferObjects.Commercial.Documents.Reports;
using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Reports;
using Application.Documents.DataTransferObjects.Budgets.Reports;
using Application.Interfaces.Queries.Commercial.Documents.Budgets;
using Application.Interfaces.Services.Commercial.Documents.Budgets;

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
