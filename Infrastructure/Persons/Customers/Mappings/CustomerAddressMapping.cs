using Domain.Persons.Customers.Entities;
using Infrastructure.Shared.Persistance.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persons.Customers.Mappings
{
    public class CustomerAddressMapping : EntityMapping<CustomerAddress>
    {
        public override void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.CustomerId)
                   .HasColumnName("customer_id")
                   .IsRequired();

            builder.Property(c => c.LocalityId)
                   .HasColumnName("locality_id")
                   .IsRequired();

            builder
                .HasOne(c => c.Locality)
                .WithMany()
                .HasForeignKey(c => c.LocalityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Neighborhood)
                .HasColumnName("neighborhood");

            builder.Property(c => c.Street)
                .HasColumnName("street");

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
