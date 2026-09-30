using Domain.Shared.Utilities;

namespace Domain.Territories.Entities
{
    public class State : Territory
    {
        private string _description = null!;

        public State(string acronym) : base(acronym) { }

        public Country? Country { get; private set; }

        public long? CountryId { get; set; }

        public override string Description { get => _description; protected set => SetDescription(value); }

        private void SetDescription(string description)
        {
            ValueValidatorUtility.ValidateValue(description, nameof(Description));
            _description = description;
        }
    }
}