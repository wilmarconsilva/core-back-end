namespace Domain.Entities.Territories
{
    public class Locality : Entity
    {
        public State? State { get; set; }

        public override required string Description { get; set; }
    }
}