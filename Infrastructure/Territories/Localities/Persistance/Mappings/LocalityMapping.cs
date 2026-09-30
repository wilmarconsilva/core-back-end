using Domain.Territories.Localities.Entities;
using Infrastructure.Shared.Persistance.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Territories.Localities.Persistance.Mappings
{
    public class LocalityMapping : EntityMapping<Locality>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Locality> builder)
        {
            base.Configure(builder);

            builder.Property(l => l.StateId)
                   .HasColumnName("state_id")
                   .IsRequired();

            builder
                .HasOne(l => l.State)
                .WithMany()
                .HasForeignKey(l => l.StateId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string GetTableName()
        {
            return "localities";
        }
    }
}