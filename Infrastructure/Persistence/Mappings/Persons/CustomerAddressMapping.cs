using Domain.Entities.Persons.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Persons
{
    public class CustomerAddressMapping : EntityMapping<CustomerAddress>
    {
        public override void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey("customer_id")
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

            builder
                .HasOne(s => s.Locality)
                .WithMany()
                .HasForeignKey("locality_id")
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            builder.Property(c => c.Neighborhood)
                .HasColumnName("neighborhood")
                .IsRequired();

            builder.Property(c => c.Street)
                .HasColumnName("street")
                .IsRequired();

            builder.Property(c => c.Complement)
                .HasColumnName("complement");

            builder.Property(c => c.PostalCode)
                .HasColumnName("postal_code");

            builder.Property(c => c.Number)
                .HasColumnName("number");
        }

        protected override void MapDescription(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.Ignore(e => e.Description);
        }

        protected override string GetTableName()
        {
            return "customers_addresses";
        }
    }
}
