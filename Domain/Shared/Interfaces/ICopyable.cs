using Domain.Shared.Entities;

namespace Domain.Shared.Interfaces
{
    public interface ICopyable<in TEntity> where TEntity : Entity
    {
        void Copy(TEntity source);
    }
}