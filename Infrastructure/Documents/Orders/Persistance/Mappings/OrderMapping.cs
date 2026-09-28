using Domain.Documents.Orders.Entities;
using Infrastructure.Documents.Persistance.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Documents.Orders.Persistance.Mappings
{
    public class OrderMapping : DocumentMapping<Order, OrderProduct>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.Property(d => d.BudgetId)
                .HasColumnName("budget_id");

            builder.HasOne(o => o.Budget)
                .WithMany()
                .HasForeignKey(o => o.BudgetId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
        }

        protected override string GetTableName()
        {
            return "orders";
        }
    }
}
