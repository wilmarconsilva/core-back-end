namespace Application.DataTransferObjects.Products
{
    /// <summary>Produto agregado nas linhas de documento (GET e POST/PUT).</summary>
    public class ProductReferenceDto
    {
        public required long Id { get; set; }

        public string? Description { get; set; }
    }
}
