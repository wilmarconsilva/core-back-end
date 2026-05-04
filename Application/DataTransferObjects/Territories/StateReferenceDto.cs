namespace Application.DataTransferObjects.Territories
{
    /// <summary>Estado agregado na localidade (GET e POST/PUT).</summary>
    public class StateReferenceDto
    {
        public required long Id { get; set; }

        public string? Acronym { get; set; }

        public string? Description { get; set; }
    }
}
