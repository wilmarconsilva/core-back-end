namespace Application.DataTransferObjects.Commercial.Documents.Budgets
{
    public class BudgetProductSaveDto : EntitySaveDto
    {
        public override long Id { get; set; }

        public required long ProductId { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal ValueUnit { get; set; }

        public required decimal ValueTotal { get; set; }
    }
}
