using FluentValidation;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Validators
{
    public abstract class DocumentSaveDtoValidator<TDocumentProductSaveDto> : Validator<DocumentSaveDto<TDocumentProductSaveDto>>
    {
        protected DocumentSaveDtoValidator(IValidator<TDocumentProductSaveDto> documentProductValidator)
        {
            RequiredNumber(b => b.UserId);
            RequiredNumber(b => b.CustomerId);
            RequiredObject(b => b.Payment);

            RuleForEach(b => b.Products).SetValidator(documentProductValidator);
        }
    }
}