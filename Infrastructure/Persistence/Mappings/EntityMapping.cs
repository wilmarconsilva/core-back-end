using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings
{
    public abstract class EntityMapping<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(GetTableName());

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            MapDescription(builder);
        }

        protected virtual void MapDescription(EntityTypeBuilder<T> builder)
        {
            builder.Property(e => e.Description)
                .HasColumnName("description")
                .IsRequired();
        }

        protected abstract string GetTableName();
    }
}