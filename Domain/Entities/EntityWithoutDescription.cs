namespace Domain.Entities
{
    public abstract class EntityWithoutDescription : Entity
    {
        public override required string Description
        {
            get
            {
                return GetDescription();
            }

            set { throw new InvalidOperationException("This property should not be filled in this class."); }
        }

        protected abstract string GetDescription();
    }
}