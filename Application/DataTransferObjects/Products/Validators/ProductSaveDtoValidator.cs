namespace Application.DataTransferObjects.Products.Validators
{
    public class ProductSaveDtoValidator : Validator<ProductSaveDto>
    {
        public ProductSaveDtoValidator() 
        {
            RequiredString(p => p.Description);
            RequiredEnum(p => p.MeasureUnit);
        }
    }
}