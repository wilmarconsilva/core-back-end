using Application.DataTransferObjects.Commercial.Documents.Budgets.Reports;
using Application.Interfaces.Queries.Commercial.Documents.Budgets;
using Infrastructure.Persistece;

namespace Infrastructure.Queries.Commercial.Documents.Budgets
{
    public class BudgetReportQuery : DocumentReportQuery<BudgetReport, BudgetReportFilter>, IBudgetReportQuery
    {
        public BudgetReportQuery(AppDbContext context) : base(context)
        {
        }

        protected override string GetTableName() => "budgets";
    }
}
