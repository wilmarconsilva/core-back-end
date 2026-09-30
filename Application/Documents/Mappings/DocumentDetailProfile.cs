using Application.Documents.DataTransferObjects;
using Application.Shared.DataTransferObjects.Entities;
using AutoMapper;
using Domain.Documents.Entities;
using Domain.Documents.ValueObjects.Payments;

namespace Application.Documents.Mappings
{
    public abstract class DocumentDetailProfile<TEntity, TDocumentProduct, TDocumentPayment, TEntityDetailDto> : Profile
        where TEntity : Document<TDocumentProduct, TDocumentPayment>
        where TDocumentProduct : DocumentProduct
        where TDocumentPayment : Payment
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
