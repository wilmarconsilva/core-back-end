using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Listings;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Orders;
using Infrastructure.Persistece;
using Infrastructure.Queries.Commercial.Documents;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries.Commercial.Documents.Orders
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
    }
}
