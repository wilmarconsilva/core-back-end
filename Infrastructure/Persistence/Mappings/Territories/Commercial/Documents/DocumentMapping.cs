using Domain.Entities.Commercial.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Territories.Commercial.Documents
{
    /// <summary>
    /// Mapeamento para CommercialDocument com suporte a Value Objects aninhados em múltiplos níveis.
    /// 
    /// Hierarquia:
    /// CommercialDocument (Entidade)
    ///   └── Payment (VO - Owned Type)
    ///       └── Installment (VO aninhado - Owned Type dentro de Owned Type)
    ///           └── DownPayment (VO aninhado - Owned Type dentro de Owned Type dentro de Owned Type)
    /// </summary>
    public abstract class DocumentMapping<TEntity> : EntityMapping<TEntity>
        where TEntity : Document
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            // NÍVEL 1: Mapeando Payment como Owned Type (VO)
            builder.OwnsOne(d => d.Payment, payment =>
            {
                // Propriedades simples do Payment
                payment.Property(p => p.Method)
                    .HasColumnName("payment_method")
                    .IsRequired();

                payment.Property(p => p.DiscountValue)
                    .HasColumnName("payment_discount_value")
                    .HasPrecision(18, 2)
                    .IsRequired();

                payment.Property(p => p.HasInstallment)
                    .HasColumnName("payment_installment_has_installment")
                    .IsRequired();

                // NÍVEL 2: Installment dentro de Payment
                payment.OwnsOne(p => p.Installment, installment =>
                {
                    installment.Property(i => i.Value)
                        .HasColumnName("payment_installment_value")
                        .HasPrecision(18, 2);

                    installment.Property(i => i.Quantity)
                        .HasColumnName("payment_installment_quantity");

                    installment.Property(p => p.HasDownPayment)
                        .HasColumnName("payment_installment_has_down_payment");

                    // NÍVEL 3: DownPayment dentro de Installment dentro de Payment
                    installment.OwnsOne(i => i.DownPayment, downPayment =>
                    {
                        downPayment.Property(dp => dp.Value)
                            .HasColumnName("payment_installment_down_payment_value")
                            .HasPrecision(18, 2);

                        downPayment.Property(dp => dp.Percentage)
                            .HasColumnName("payment_installment_down_payment_percentage")
                            .HasPrecision(5, 2);

                        downPayment.Property(dp => dp.Method)
                            .HasColumnName("payment_installment_down_payment_method");
                    });
                });
            });

            // Propriedades simples da entidade
            builder.Property(d => d.ValueSubtotal)
                .HasColumnName("value_subtotal")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(d => d.ValueTotal)
                .HasColumnName("value_total")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(d => d.Observations)
                .HasColumnName("observations");

            builder.Property(d => d.CustomerSignatureImage)
                .HasColumnName("customer_signature_image")
                .IsRequired();

            builder.Property(d => d.CreationDateTime)
                .HasColumnName("creation_date_time")
                .IsRequired();

            builder.Property(d => d.LastUpdateDateTime)
                .HasColumnName("last_update_date_time")
                .IsRequired();

            // Relacionamentos
            builder.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .IsRequired();

            builder.HasOne(d => d.Customer)
                .WithMany()
                .HasForeignKey(d => d.CustomerId)
                .IsRequired();

            // Mapeamento da coleção de DocumentProducts (filhos) via campo privado
            builder
                .HasMany("_products")
                .WithOne(nameof(DocumentProduct.Document))
                .HasForeignKey(nameof(DocumentProduct.DocumentId))
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Navigation(d => d.Products)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
