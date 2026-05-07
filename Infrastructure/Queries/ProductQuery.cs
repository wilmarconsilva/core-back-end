using Application.DataTransferObjects.Products;
using Application.DataTransferObjects.Products.Listings;
using Application.Interfaces.Queries;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistece;
using Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
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
                query = query.Where(p => p.Description.Contains(filter.Description));

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
