using Application.DataTransferObjects.Products;
using Application.Interfaces.Queries;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Persons.Customers
{
    public class ProductService : CrudService<Product, ProductSaveDto>, IProductService
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