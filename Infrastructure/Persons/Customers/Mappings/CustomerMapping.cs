using Domain.Persons.Customers.Entities;
using Infrastructure.Shared.Persistance.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persons.Customers.Mappings
{
    public class CustomerMapping : EntityMapping<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(c => c.PersonType)
                .HasColumnName("person_type")
                .IsRequired();

            builder.Property(c => c.Document)
                .HasColumnName("document")
                .IsRequired();

            builder.Property(c => c.DocumentType)
                .HasColumnName("document_type")
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnName("email");

            builder.Property(c => c.StateRegistration)
                .HasColumnName("state_registration");

            builder
                .HasMany(c => c.Addresses)
                .WithOne()
                .HasForeignKey(nameof(CustomerAddress.CustomerId))
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Navigation(c => c.Addresses)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }

        protected override void MapDescription(EntityTypeBuilder<Customer> builder)
        {
            builder.Ignore(e => e.Description);
        }

        protected override string GetTableName()
        {
            return "customers";
        }
    }
}
