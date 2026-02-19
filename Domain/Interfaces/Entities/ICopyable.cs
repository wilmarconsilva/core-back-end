using Domain.Entities;

namespace Domain.Interfaces.Entities
{
    public interface ICopyable<in TEntity> where TEntity : Entity
    {
        void Copy(TEntity source);
    }
}