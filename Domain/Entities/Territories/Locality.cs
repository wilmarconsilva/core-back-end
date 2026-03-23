using Domain.Utilities.Entities;

namespace Domain.Entities.Territories
{
    public class Locality : Entity
    {
        private string _description = null!;

        private Locality() { }

        public Locality(string description, long? stateId)
        {
            Fill(description, stateId);
        }

        public State? State { get; private set; }

        public long? StateId { get; set; }

        public override string Description { get => _description; protected set => SetDescription(value); }

        public void Fill(string description, long? stateId)
        {
            Description = description;
            StateId = stateId;
        }

        private void SetDescription(string description)
        {
            Validator.ValidateValue(description, nameof(Description));
            _description = description;
        }
    }
}