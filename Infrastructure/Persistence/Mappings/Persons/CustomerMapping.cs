using Domain.Entities.Persons.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Persons
{
    public class CustomerMapping : EntityMapping<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.ToTable("customers");

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(c => c.PersonType)
                .HasColumnName("person_type")
                .IsRequired();

            builder.Property(c => c.Document)
                .HasColumnName("document")
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(c => c.DocumentType)
                .HasColumnName("document_type")
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnName("email")
                .HasMaxLength(320);

            builder.Property(c => c.StateRegistration)
                .HasColumnName("state_registration")
                .HasMaxLength(14);
        }

        protected override void MapDescription(EntityTypeBuilder<Customer> builder)
        {
            builder.Ignore(e => e.Description);
        }
    }
}
