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

        public void SetAcronym(string acronym)
        {
            Validate(acronym, nameof(Acronym));
            _acronym = acronym;
        }
    }
}