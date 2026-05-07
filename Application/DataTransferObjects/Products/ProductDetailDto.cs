using Application.DataTransferObjects.Entities;
using Domain.ValueObjects.Enums;

namespace Application.DataTransferObjects.Products
{
    public class ProductDetailDto : EntitySaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required string Description { get; set; }

        public required MeasureUnit MeasureUnit { get; set; }
    }
}
