using Application.Products.DataTransferObjects;
using Application.Products.DataTransferObjects.Listings;
using Application.Products.Services.Interfaces;
using Application.Shared.Queries.Interfaces;
using Application.Shared.Services;
using Domain.Products.Entities;
using Domain.Products.Repositories.Interfaces;
using Domain.Shared.Persistance.Interfaces;

namespace Application.Products.Services
{
    public class ProductService : CrudService<Product, ProductSaveDto, ProductDetailDto, ProductListing, ProductListingFilter>, IProductService
    {
        public ProductService(IProductRepository repository, IProductQuery query, IUnitOfWork unitOfWork) : base(repository, query, unitOfWork)
        {
        }

        protected override Product CreateEntity(ProductSaveDto dto)
        {
            return new Product(dto.Description, dto.MeasureUnit);
        }

        protected override void UpdateEntity(ProductSaveDto dto, Product entity)
        {
            entity.Fill(dto.Description, dto.MeasureUnit);
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}