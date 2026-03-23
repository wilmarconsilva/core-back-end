using Domain.Utilities.Entities;

namespace Domain.Entities.Territories
{
    public class Country : Territory
    {
        private string _description = null!;

        public Country(string acronym) : base(acronym) { }

        public override string Description { get => _description; protected set => SetDescription(value); }

        public void SetDescription(string description)
        {
            Validator.ValidateValue(description, nameof(Description));
            _description = description;
        }
    }
}