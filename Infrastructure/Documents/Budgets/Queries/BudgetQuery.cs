using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Listings;
using Application.Documents.Budgets.Queries.Interfaces;
using AutoMapper;
using Domain.Documents.Budgets.Entities;
using Domain.Documents.Budgets.ValueObjects;
using Infrastructure.Documents.Queries;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Documents.Budgets.Queries
{
    public class BudgetQuery : DocumentQuery<Budget, BudgetDetailDto, BudgetProduct, BudgetPayment, BudgetListing, BudgetListingFilter>, IBudgetQuery
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
