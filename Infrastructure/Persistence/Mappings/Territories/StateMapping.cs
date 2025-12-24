using Domain.Entities.Territories;
namespace Infrastructure.Persistence.Mappings.Territories
{
    public class StateMapping : TerritoryMapping<State>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<State> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(s => s.Country)
                .WithMany()
                .HasForeignKey("countryId")
                .IsRequired();
        }
    }
}