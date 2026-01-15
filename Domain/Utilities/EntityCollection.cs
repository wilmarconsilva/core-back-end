using Domain.Entities;
using Domain.Interfaces.Entities;

namespace Domain.Utilities
{
    internal static class EntityCollection
    {
        public static void Sync<TEntity>(ICollection<TEntity> current, IEnumerable<TEntity> incoming) where TEntity : Entity, ISyncable<TEntity>
        {
            IEnumerable<long> incomingIds = incoming
                .Where(e => e.Id > 0)
                .Select(e => e.Id);

            IEnumerable<TEntity> idsToRemove = current
                .Where(e => e.Id > 0 && !incomingIds.Contains(e.Id));

            foreach (var entity in idsToRemove)
                current.Remove(entity);

            foreach (var entity in incoming)
            {
                TEntity? existing = current.FirstOrDefault(c => c.Id == entity.Id);

                if (existing == null)
                    current.Add(entity);
                else
                    existing.Sync(entity);
            }
        }
    }
}
