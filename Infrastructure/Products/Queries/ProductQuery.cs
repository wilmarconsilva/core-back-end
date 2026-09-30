using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Domain.Products.Entities;
using Application.Products.DataTransferObjects;
using Application.Products.DataTransferObjects.Listings;
using Infrastructure.Shared.Persistance;
using Infrastructure.Shared;
using Infrastructure.Shared.Queries;
using Application.Shared.Queries.Interfaces;

namespace Infrastructure.Products.Queries
{
    public class ProductQuery : DetailDtoQuery<Product, ProductDetailDto>, IProductQuery
    {
        public ProductQuery(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<ProductListing>> GetAll(ProductListingFilter filter, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _query;

            if (filter.Description != null)
                query = query.WhereContains(p => p.Description, filter.Description);

            return await ApplyPaging(query, filter)
                .Select(product => new ProductListing
                {
                    Id = product.Id,
                    Description = product.Description,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
