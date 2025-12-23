namespace Domain.Entities.Persons.Localities
{
    public class State : Territory
    {
        public required Country Country { get; set; }
    }
}