using Domain.Utilities.Entities;

namespace Domain.Entities.Territories
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
            Validator.ValidateValue(acronym, nameof(Acronym));
            _acronym = acronym;
        }
    }
}