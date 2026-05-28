namespace Domain.Interfaces.Entities.Commercial
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
