namespace Domain.Entities.Territories
{
    public class State : Territory
    {
        public State(string acronym) : base(acronym) { }

        public Country? Country { get; set; }

        public override required string Description { get; set; }
    }
}