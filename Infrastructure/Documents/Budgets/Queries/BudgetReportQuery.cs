using Application.Documents.Budgets.DataTransferObjects.Reports;
using Application.Documents.Budgets.Queries.Interfaces;
using Infrastructure.Documents.Queries;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Documents.Budgets.Queries
{
    public class BudgetReportQuery : DocumentReportQuery<BudgetReport, BudgetReportFilter>, IBudgetReportQuery
    {
        public BudgetReportQuery(AppDbContext context) : base(context)
        {
        }

        protected override string GetTableName() => "budgets";
    }
}
