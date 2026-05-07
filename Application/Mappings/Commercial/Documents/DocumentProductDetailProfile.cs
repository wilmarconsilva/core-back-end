using Application.DataTransferObjects.Commercial.Documents.DocumentsProducts;
using Application.DataTransferObjects.Entities;
using AutoMapper;
using Domain.Entities.Commercial.Documents;

namespace Application.Mappings.Commercial.Documents
{
    public abstract class DocumentProductDetailProfile<TEntity, TEntityDetailDto> : Profile
        where TEntity : DocumentProduct
        where TEntityDetailDto : DocumentProductDetailDto
    {
        public DocumentProductDetailProfile()
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
