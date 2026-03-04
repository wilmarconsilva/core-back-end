using Domain.Utilities.Entities;

namespace Domain.Entities.Territories
{
    public class Locality : Entity
    {
        private string _description = null!;

        public State? State { get; set; }

        public override string Description { get => _description; protected set => SetDescription(value); }

        private void SetDescription(string description)
        {
            Validator.ValidateValue(description, nameof(Description));
            _description = description;
        }
    }
}