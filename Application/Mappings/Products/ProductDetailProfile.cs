using Application.Products.DataTransferObjects;
using AutoMapper;
using Domain.Products.Entities;

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
