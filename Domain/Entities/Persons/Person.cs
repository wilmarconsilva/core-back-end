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
            SetName(name);
        }

        #endregion Constructors

        public string Name => _name;
        
        public override required string Description { get => _name; set => SetName(value); }

        #region Protected methods

        protected void SetName(string name)
        {
            Validate(name, nameof(Name));

            _name = name;
        }

        #endregion Protected methods
    }
}