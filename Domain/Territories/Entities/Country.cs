using Domain.Shared.Utilities;

namespace Domain.Territories.Entities
{
    public class Country : Territory
    {
        private string _description = null!;

        public Country(string acronym) : base(acronym) { }

        public override string Description { get => _description; protected set => SetDescription(value); }

        public void SetDescription(string description)
        {
            ValueValidatorUtility.ValidateValue(description, nameof(Description));
            _description = description;
        }
    }
}