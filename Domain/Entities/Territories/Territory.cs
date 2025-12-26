namespace Domain.Entities.Territories
{
    public abstract class Territory : Entity
    {
        public required string Acronym { get; set; }
    }
}