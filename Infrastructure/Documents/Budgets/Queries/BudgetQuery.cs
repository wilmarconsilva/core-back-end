using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.Documents.Budgets.DataTransferObjects.Listings;
using Application.Documents.DataTransferObjects;
using Application.Interfaces.Queries.Commercial.Documents.Budgets;
using AutoMapper;
using Domain.Documents.Budgets.Entities;
using Infrastructure.Documents.Queries;
using Infrastructure.Shared.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Documents.Budgets.Queries
{
    public class BudgetQuery : DocumentQuery<Budget, DocumentDetailDto, BudgetProduct, BudgetListing, BudgetListingFilter>, IBudgetQuery
    {
        public BudgetQuery(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Budget> ApplyIncludes(IQueryable<Budget> query)
        {
            return query
                .Include(b => b.Customer)
                .Include(b => b.Products)
                    .ThenInclude(p => p.Product);
        }
    }
}
