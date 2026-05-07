using Application.DataTransferObjects.Products;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.Products
{
    public class ProductDetailProfile : Profile
    {
        public ProductDetailProfile()
        {
            CreateMap<Product, ProductDetailDto>();
        }
    }
}
