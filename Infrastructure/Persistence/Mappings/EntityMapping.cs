using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class EntityMapping<T> : IEntityTypeConfiguration<T> where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        ConfigureDescription(builder);
    }

    public virtual void ConfigureDescription(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.Description)
            .HasColumnName("description")
            .IsRequired();;
    }
}