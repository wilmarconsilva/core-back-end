using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Security
{
    public class RefreshTokenMapping : EntityMapping<RefreshToken>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            base.Configure(builder);

            builder.Property(refreshToken => refreshToken.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(refreshToken => refreshToken.TokenHash)
                .HasColumnName("token_hash")
                .IsRequired();

            builder.Property(refreshToken => refreshToken.CreatedDateTime)
                .HasColumnName("created_date_time")
                .IsRequired();

            builder.Property(refreshToken => refreshToken.ExpirationDateTime)
                .HasColumnName("expires_date_time")
                .IsRequired();

            builder.Property(refreshToken => refreshToken.RevokedDateTime)
                .HasColumnName("revoked_date_time");

            builder.Property(refreshToken => refreshToken.ReplacedByTokenHash)
                .HasColumnName("replaced_by_token_hash");

            builder.HasOne(refreshToken => refreshToken.User)
                .WithMany()
                .HasForeignKey(refreshToken => refreshToken.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(refreshToken => refreshToken.TokenHash)
                .IsUnique();

            builder.HasIndex(refreshToken => refreshToken.UserId);
        }

        protected override void MapDescription(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Ignore(refreshToken => refreshToken.Description);
        }

        protected override string GetTableName()
        {
            return "refresh_tokens";
        }
    }
}
