using Domain.Territories.Entities;
using Infrastructure.Territories.Persistance.Mappings;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Territories.States.Persistance.Mappings
{
    public class StateMapping : TerritoryMapping<State>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<State> builder)
        {
            base.Configure(builder);

            builder.Property(s => s.CountryId)
                   .HasColumnName("country_id")
                   .IsRequired();

            builder
                .HasOne(s => s.Country)
                .WithMany()
                .HasForeignKey(s => s.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string GetTableName()
        {
            return "states";
        }
    }
}