namespace Application.DataTransferObjects.Persons.Customers
{
    public class CustomerAddressSaveDto : EntitySaveDto
    {
        public required string Neighborhood { get; set; }

        public required string Street { get; set; }

        public required long LocalityId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public long CustomerId { get; set; }

        public string? Complement { get; set; }

        public string? PostalCode { get; set; }
        
        public string? Number { get; set; }
    }
}