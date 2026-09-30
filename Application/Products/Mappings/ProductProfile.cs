using Application.Products.DataTransferObjects;
using Application.Shared.Mappings;
using Domain.Products.Entities;

namespace Application.Products.Mappings
{
    public class ProductProfile : CrudProfile<ProductSaveDto, Product>
    {

    }
}