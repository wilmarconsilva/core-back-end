namespace Application.DataTransferObjects.Localities
{
    /// <summary>Referência agregada à localidade (GET e POST/PUT).</summary>
    public class LocalityReferenceDto
    {
        public required long Id { get; set; }

        public string? Description { get; set; }
    }
}
