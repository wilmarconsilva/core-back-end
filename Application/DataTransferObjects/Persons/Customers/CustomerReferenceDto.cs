namespace Application.DataTransferObjects.Persons.Customers
{
    /// <summary>Cliente agregado nos documentos (GET e POST/PUT).</summary>
    public class CustomerReferenceDto
    {
        public required long Id { get; set; }

        public string? Name { get; set; }

        public string? Document { get; set; }
    }
}
