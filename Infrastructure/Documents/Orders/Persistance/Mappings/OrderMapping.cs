using Domain.Documents.Orders.Entities;
using Domain.Documents.Orders.ValueObjects;
using Infrastructure.Documents.Persistance.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Documents.Orders.Persistance.Mappings
{
    public class OrderMapping : DocumentMapping<Order, OrderProduct, OrderPayment>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.Property(d => d.BudgetId)
                .HasColumnName("budget_id");

            builder.HasOne(o => o.Budget)
                .WithMany()
                .HasForeignKey(o => o.BudgetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(d => d.Payment, payment =>
            {
                payment.Property(p => p.Method)
                    .HasColumnName("payment_method")
                    .IsRequired();
            });
        }

        protected override string GetTableName()
        {
            return "orders";
        }
    }
}
