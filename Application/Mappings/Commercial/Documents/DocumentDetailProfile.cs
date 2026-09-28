using Application.DataTransferObjects.Entities;
using Application.Documents.DataTransferObjects;
using AutoMapper;
using Domain.Documents.Entities;

namespace Application.Mappings.Commercial.Documents
{
    public abstract class DocumentDetailProfile<TEntity, TDocumentProduct, TEntityDetailDto> : Profile
        where TEntity : Document<TDocumentProduct>
        where TDocumentProduct : DocumentProduct
        where TEntityDetailDto : DocumentDetailDto
    {
        public DocumentDetailProfile()
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
