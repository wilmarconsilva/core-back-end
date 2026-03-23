using Application.DataTransferObjects.Commercial.Documents;
using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Listings;
using Application.Interfaces.Queries;
using Domain.Entities;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;
using Domain.ValueObjects.Commercial.Documents.Payments.Installments;

namespace Application.Services.Commercial.Documents
{
    public abstract class DocumentService<TEntity, TDocumentSaveDto, TDocumentProductSaveDto, TListing, TFilter> : CrudService<TEntity, TDocumentSaveDto, TListing, TFilter>
        where TEntity : Entity
        where TDocumentSaveDto : DocumentSaveDto<TDocumentProductSaveDto>
        where TDocumentProductSaveDto : DocumentProductSaveDto
        where TListing : EntityListing
        where TFilter : ListingFilter
    {
        protected DocumentService(IRepositoryCrud<TEntity> repository, IEntitySaveDtoQuery<TDocumentSaveDto> query, IUnitOfWork unitOfWork)
            : base(repository, query, unitOfWork)
        {

        }

        protected Payment CreatePayment(TDocumentSaveDto dto)
        {
            DownPayment? downPayment = null;
            if (dto.Payment.Installment?.DownPayment != null)
                downPayment = new DownPayment(dto.Payment.Installment.DownPayment.Value, dto.Payment.Installment.DownPayment.Percentage, dto.Payment.Installment.DownPayment.Method);

            Installment? installment = null;
            if (dto.Payment.Installment != null)
                installment = new Installment(dto.Payment.Installment.Quantity, downPayment);

            Payment payment = new Payment(dto.Payment.Method, dto.Payment.DiscountValue, installment);

            return payment;
        }
    }
}
