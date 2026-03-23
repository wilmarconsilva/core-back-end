namespace Domain.Entities
{
    public abstract class Entity
    {
        #region Public properties

        public long Id { get; set; }

        public abstract string Description { get; protected set; }

        #endregion Public properties
    }
}