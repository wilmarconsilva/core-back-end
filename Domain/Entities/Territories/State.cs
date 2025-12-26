namespace Domain.Entities.Territories
{
    public class State : Territory
    {
        public Country? Country { get; set; }
    }
}