using Domain.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Persons
{
    public class UserMapping : EntityMapping<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(u => u.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            builder.Property(u => u.SignatureImage)
                .HasColumnName("signature_image");

            builder.HasIndex(u => u.Email)
                .IsUnique();
        }

        protected override void MapDescription(EntityTypeBuilder<User> builder)
        {
            builder.Ignore(e => e.Description);
        }

        protected override string GetTableName()
        {
            return "users";
        }
    }
}
