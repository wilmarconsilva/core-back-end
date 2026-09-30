namespace Domain.Documents.Interfaces
{
    public interface IDocument
    {
        long CustomerId { get; }

        decimal ValueSubtotal { get; }

        decimal ValueTotal { get; }

        string? Observations { get; }

        DateTime CreationDateTime { get; }

        public byte[]? PDF { get; }
    }
}
