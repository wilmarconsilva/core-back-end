namespace Application.DataTransferObjects.Commercial.Documents
{
    public abstract class DocumentProductReport
    {
        public long Id { get; set; }

        public long ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal ValueUnit { get; set; }

        public decimal ValueTotal { get; set; }
    }
}
