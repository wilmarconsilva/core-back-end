using Application.DataTransferObjects.Commercial.Documents.Payments;
using FluentValidation;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Validators
{
    public abstract class DocumentSaveDtoValidator<TDocumentSaveDto, TDocumentProductSaveDto> : Validator<TDocumentSaveDto> 
        where TDocumentSaveDto : DocumentSaveDto<TDocumentProductSaveDto> 
        where TDocumentProductSaveDto : DocumentProductSaveDto
    {
        protected DocumentSaveDtoValidator(IValidator<TDocumentProductSaveDto> documentProductValidator, IValidator<PaymentSaveDto> paymentSaveDtoValidator)
        {
            RequiredNumber(b => b.UserId);
            RequiredNumber(b => b.CustomerId);

            RuleFor(b => b.Payment)
                .NotNull()
                .WithMessage(_requiredMessage)
                .SetValidator(paymentSaveDtoValidator!);

            RuleForEach(b => b.Products).SetValidator(documentProductValidator);
        }
    }
}