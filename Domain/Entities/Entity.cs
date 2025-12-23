namespace Domain.Entities
{
    public abstract class Entity
    {
        public long Id { get; set; }

        public virtual required string Description { get; set; }
    }
}