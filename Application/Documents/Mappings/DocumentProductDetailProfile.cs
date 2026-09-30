using Application.Shared.DataTransferObjects.Entities;
using Application.Documents.DataTransferObjects.DocumentsProducts;
using AutoMapper;
using Domain.Documents.Entities;

namespace Application.Documents.Mappings
{
    public abstract class DocumentProductDetailProfile<TEntity, TEntityDetailDto> : Profile
        where TEntity : DocumentProduct
        where TEntityDetailDto : DocumentProductDetailDto
    {
        protected DocumentProductDetailProfile()
        {
            CreateMap<TEntity, TEntityDetailDto>()
                .ForMember(d => d.Product, opt => opt.MapFrom(s =>
                    new EntityReferenceDto
                    {
                        Id = s.ProductId,
                        Description = s.Product!.Description,
                    }));
        }
    }
}
