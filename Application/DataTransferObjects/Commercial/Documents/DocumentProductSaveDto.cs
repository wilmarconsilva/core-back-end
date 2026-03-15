namespace Application.DataTransferObjects.Commercial.Documents
{
    public abstract class DocumentProductSaveDto : EntitySaveDto
    {
        public required long ProductId { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal ValueUnit { get; set; }

        public required decimal ValueTotal { get; set; }
    }
}
