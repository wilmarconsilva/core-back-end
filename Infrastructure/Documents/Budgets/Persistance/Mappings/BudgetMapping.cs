using Domain.Documents.Budgets.Entities;
using Domain.Documents.Budgets.ValueObjects;
using Infrastructure.Documents.Persistance.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Documents.Budgets.Persistance.Mappings
{
    public class BudgetMapping : DocumentMapping<Budget, BudgetProduct, BudgetPayment>
    {
        protected override string GetTableName()
        {
            return "budgets";
        }

        public override void Configure(EntityTypeBuilder<Budget> builder)
        {
            base.Configure(builder);

            builder.OwnsOne(d => d.Payment, payment =>
            {
                payment.Property(p => p.Method)
                    .HasColumnName("payment_method");
            });
        }
    }
}