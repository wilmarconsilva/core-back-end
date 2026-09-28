using Domain.Entities;
using Domain.Territories.Entities;
using Infrastructure.Shared.Persistance.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Territories.Persistance.Mappings
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