using Domain.Entities.Commercial.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Territories.Commercial.Documents
{
    /// <summary>
    /// Mapeamento abstrato para DocumentProduct com suporte a relacionamentos com Document e Product.
    /// 
    /// Estrutura:
    /// DocumentProduct (Entidade)
    ///   ├── Document (Relacionamento - pai)
    ///   └── Product (Relacionamento - produto)
    /// </summary>
    public abstract class DocumentProductMapping<TEntity> : EntityMapping<TEntity>
        where TEntity : DocumentProduct
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            // Propriedades simples
            builder.Property(dp => dp.Quantity)
                .HasColumnName("quantity")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(dp => dp.ValueUnit)
                .HasColumnName("value_unit")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(dp => dp.ValueTotal)
                .HasColumnName("value_total")
                .HasPrecision(18, 2)
                .IsRequired();

            // Relacionamentos
            builder.Property(dp => dp.DocumentId)
                .HasColumnName("document_id")
                .IsRequired();

            builder.Property(dp => dp.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.HasOne(dp => dp.Product)
                .WithMany()
                .HasForeignKey(dp => dp.ProductId)
                .IsRequired();
        }

        protected override void MapDescription(EntityTypeBuilder<TEntity> builder)
        {
            builder.Ignore(e => e.Description);
        }
    }
}
