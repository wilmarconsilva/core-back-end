using Domain.Shared.Entities;
using Domain.Shared.Utilities;
using Domain.Territories.Entities;

namespace Domain.Territories.Localities.Entities
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
            ValueValidatorUtility.ValidateValue(description, nameof(Description));
            _description = description;
        }
    }
}