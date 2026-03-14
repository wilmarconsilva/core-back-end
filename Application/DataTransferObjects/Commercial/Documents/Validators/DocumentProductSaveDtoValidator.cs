namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Validators
{
    public abstract class DocumentProductSaveDtoValidator<TDocumentProductSaveDto> : Validator<TDocumentProductSaveDto> where TDocumentProductSaveDto : DocumentProductSaveDto
    {
        protected DocumentProductSaveDtoValidator()
        {
            RequiredNumber(bp => bp.ProductId);
            RequiredNumber(bp => bp.Quantity);
            RequiredNumber(bp => bp.ValueUnit);
            RequiredNumber(bp => bp.ValueTotal);
        }
    }
}