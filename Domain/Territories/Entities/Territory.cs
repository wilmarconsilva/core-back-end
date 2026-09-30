using Domain.Shared.Entities;
using Domain.Shared.Utilities;

namespace Domain.Territories.Entities
{
    public abstract class Territory : Entity
    {
        private string _acronym = null!;

        protected Territory(string acronym)
        {
            SetAcronym(acronym);
        }

        public string Acronym { get => _acronym; private set => SetAcronym(value); }

        private void SetAcronym(string acronym)
        {
            ValueValidatorUtility.ValidateValue(acronym, nameof(Acronym));
            _acronym = acronym;
        }
    }
}