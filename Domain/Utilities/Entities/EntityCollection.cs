using Domain.Entities;
using Domain.Interfaces.Entities;

namespace Domain.Utilities.Entities
{
    internal static class EntityCollection
    {
        public static void Sync<TEntity>(ICollection<TEntity> current, IEnumerable<TEntity> incoming, long fatherEntityId) where TEntity : Entity, ICopyable<TEntity>, IEntityChild
        {
            IEnumerable<long> incomingIds = incoming.Select(e => e.Id);

            TEntity[] entitiesToRemove = current.Where(e => e.Id > 0 && !incomingIds.Contains(e.Id)).ToArray();

            for (int i = 0; i < entitiesToRemove.Length; i++)
                current.Remove(entitiesToRemove[i]);

            foreach (TEntity entity in incoming)
            {
                if (entity.Id > 0)
                {
                    TEntity? existing = current.FirstOrDefault(c => c.Id == entity.Id);

                    if (existing != null)
                        existing.Copy(entity);
                    else
                        throw new Exceptions.BusinessRuleException($"O registro de: {nameof(TEntity)} com o código: {entity.Id} não foi encontrado.");
                }
                else
                {
                    if (fatherEntityId > 0)
                        entity.SetFatherEntityId(fatherEntityId);

                    current.Add(entity);
                }
            }
        }
    }
}
