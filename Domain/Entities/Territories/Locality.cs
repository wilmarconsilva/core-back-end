namespace Domain.Entities.Territories
{
    public class Locality : Entity
    {
        public Locality() { }
        public Locality(long id, string description) 
        {
            Id = id;
            Description = description;
        }

        public State? State { get; set; }
    }
}