namespace Domain.Entities.Persons
{
    public abstract class Person : Entity
    {
        public required string Name { get; set; }
        
        public override required string Description { get { return Name; } set { Name = value; } }
    }
}