using Application.Documents.DataTransferObjects;
using Application.Shared.DataTransferObjects.Entities;
using AutoMapper;
using Domain.Documents.Entities;

namespace Application.Documents.Mappings
{
    public abstract class DocumentDetailProfile<TEntity, TDocumentProduct, TEntityDetailDto> : Profile
        where TEntity : Document<TDocumentProduct>
        where TDocumentProduct : DocumentProduct
        where TEntityDetailDto : DocumentDetailDto
    {
        protected DocumentDetailProfile()
        {
            CreateMap<TEntity, TEntityDetailDto>()
                .ForMember(d => d.Customer, opt => opt.MapFrom(s =>
                    new EntityReferenceDto
                    {
                        Id = s.CustomerId,
                        Description = s.Customer!.Name,
                    }));
        }
    }
}
