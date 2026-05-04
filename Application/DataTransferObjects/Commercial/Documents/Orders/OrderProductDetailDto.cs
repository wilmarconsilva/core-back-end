using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Products;

namespace Application.DataTransferObjects.Commercial.Documents.Orders
{
    public class OrderProductDetailDto : EntitySaveDto
    {
        public required ProductReferenceDto Product { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal ValueUnit { get; set; }

        public required decimal ValueTotal { get; set; }
    }
}
