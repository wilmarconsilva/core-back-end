using Domain.Entities;

namespace Domain.Interfaces.Entities
{
    internal interface ISyncable<in TEntity> where TEntity : Entity
    {
        void Sync(TEntity source);
    }
}