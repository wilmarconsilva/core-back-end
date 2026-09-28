namespace Domain.Shared.Entities
{
    public abstract class EntityWithoutDescription : Entity
    {
        public override string Description
        {
            get
            {
                return GetDescription();
            }

            protected set { throw new InvalidOperationException("This property should not be filled in this class."); }
        }

        protected abstract string GetDescription();
    }
}