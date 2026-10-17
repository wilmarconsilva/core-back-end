using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Domain.Documents.Orders.Entities;
using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Listings;
using Infrastructure.Shared.Persistance;
using Infrastructure.Documents.Queries;
using Application.Documents.Orders.Queries;

namespace Infrastructure.Documents.Orders.Queries
{
    public class OrderQuery : DocumentQuery<Order, OrderDetailDto, OrderProduct, OrderListing, OrderListingFilter>, IOrderQuery
    {
        public OrderQuery(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Order> ApplyIncludes(IQueryable<Order> query)
        {
            return query
                .Include(o => o.Customer)
                .Include(o => o.Products)
                    .ThenInclude(p => p.Product);
        }

        public override async Task<List<OrderListing>> GetAll(OrderListingFilter filter, CancellationToken cancellationToken)
        {
            IQueryable<Order> query = _query;

            if (filter.Id.HasValue)
                query = query.Where(order => order.Id == filter.Id.Value);

            if (filter.CustomerId.HasValue)
                query = query.Where(order => order.CustomerId == filter.CustomerId.Value);

            if (filter.CreationDateTimeStart.HasValue)
                query = query.Where(order => order.CreationDateTime >= filter.CreationDateTimeStart.Value);

            if (filter.CreationDateTimeEnd.HasValue)
                query = query.Where(order => order.CreationDateTime <= filter.CreationDateTimeEnd.Value);

            return await ApplyPaging(query, filter)
                .Select(order => new OrderListing
                {
                    Id = order.Id,
                    BudgetId = order.BudgetId,
                    CustormerName = order.Customer!.Name,
                    CustomerLocalityState = order.Customer!.Addresses
                            .OrderBy(address => address.Id)
                            .Select(address => address.Locality!.Description + (address.Locality.State == null ? string.Empty : $" - {address.Locality.State.Acronym}"))
                            .FirstOrDefault() ?? string.Empty,
                    ValueTotal = order.ValueTotal,
                    CreationDateTime = order.CreationDateTime
                })
                .ToListAsync(cancellationToken);
        }
    }
}
