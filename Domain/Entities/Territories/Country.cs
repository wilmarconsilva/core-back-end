namespace Domain.Entities.Territories
{
    public class Country : Territory
    {
        private string _description = null!;

        public Country(string acronym) : base(acronym) { }

        public override string Description { get => _description; protected set => SetDescription(value); }

        private void SetDescription(string description)
        {
            ValidateValue(description, nameof(Description));
            _description = description;
        }
    }
}