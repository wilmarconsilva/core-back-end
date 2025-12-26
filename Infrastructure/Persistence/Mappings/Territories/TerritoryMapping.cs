using Domain.Entities;
using Domain.Entities.Territories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Territories
{
    public abstract class TerritoryMapping<T> : EntityMapping<T> where T : Territory
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Acronym)
                .HasColumnName("acronym")
                .HasMaxLength(4);
        }
    }
}