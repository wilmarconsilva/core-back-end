using Domain.Utilities.Entities;

namespace Domain.Entities.Territories
{
    public class State : Territory
    {
        private string _description = null!;

        public State(string acronym) : base(acronym) { }

        public Country? Country { get; set; }

        public override string Description { get => _description; protected set => SetDescription(value); }

        private void SetDescription(string description)
        {
            Validator.ValidateValue(description, nameof(Description));
            _description = description;
        }
    }
}