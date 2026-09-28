using Application.Shared.DataTransferObjects.Entities;
using Domain.Products.Enums;

namespace Application.Products.DataTransferObjects
{
    public class ProductDetailDto : EntitySaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required string Description { get; set; }

        public required MeasureUnit MeasureUnit { get; set; }
    }
}
