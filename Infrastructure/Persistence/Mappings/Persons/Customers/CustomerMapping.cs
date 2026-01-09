using Domain.Entities.Persons.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Persons.Customers
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

            builder.HasMany(c => c.Addresses)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
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
