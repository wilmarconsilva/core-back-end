using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Listings;
using Application.Interfaces.Queries.Commercial.Documents.Budgets;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Budgets;
using Infrastructure.Persistece;
using Infrastructure.Queries.Commercial.Documents;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries.Commercial.Documents.Budgets
{
    public class BudgetQuery : DocumentQuery<Budget, BudgetSaveDto, BudgetProduct, BudgetListing, BudgetListingFilter>, IBudgetQuery
    {
        public BudgetQuery(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Budget> ApplyIncludes(IQueryable<Budget> query)
        {
            return query
                .Include(b => b.Products);
        }
    }
}
