using Domain.Entities.Territories;

namespace Infrastructure.Persistence.Mappings.Territories
{
    public class LocalityMapping : EntityMapping<Locality>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Locality> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(s => s.State)
                .WithMany()
                .HasForeignKey("stateId")
                .IsRequired();
        }
    }
}