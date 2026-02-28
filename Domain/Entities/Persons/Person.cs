namespace Domain.Entities.Persons
{
    public abstract class Person : Entity
    {
        private string _name = null!;

        #region Constructors

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        protected Person() { }

        protected Person(string name)
        {
            Name = name;
        }

        #endregion Constructors

        public string Name { get => _name; set => SetName(value); }

        public override string Description { get => _name; protected set => SetName(value); }

        #region Protected methods

        protected void SetName(string name)
        {
            ValidateValue(name, nameof(Name));

            _name = name;
        }

        #endregion Protected methods
    }
}