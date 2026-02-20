using Domain.ValueObjects.Enums;

namespace Application.DataTransferObjects.Products
{
    public class ProductSaveDto : EntitySaveDto
    {
        public required string Description { get; set; }

        public required MeasureUnit MeasureUnit { get; set; }
    }
}