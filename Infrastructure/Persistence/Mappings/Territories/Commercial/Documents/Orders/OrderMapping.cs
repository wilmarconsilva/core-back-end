using Domain.Entities.Commercial.Documents.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Territories.Commercial.Documents.Orders
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
