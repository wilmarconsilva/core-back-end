using Application.DataTransferObjects.Products;
using Application.Interfaces.Queries;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistece;

namespace Infrastructure.Queries
{
    public class ProductQuery : SaveDtoQuery<Product, ProductSaveDto>, IProductQuery
    {
        public ProductQuery(AppDbContext context, IMapper mapper) : base(context, mapper)
        {

        }
    }
}