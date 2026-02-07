using Domain.Entities;

namespace Domain.Interfaces.Entities
{
    internal interface ICopyable<in TEntity> where TEntity : Entity
    {
        void Copy(TEntity source);
    }
}