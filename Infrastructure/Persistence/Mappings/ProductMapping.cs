using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings.Persons.Customers
{
    public class ProductMapping : EntityMapping<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.MeasureUnit)
                .HasColumnName("measure_unit")
                .IsRequired();
        }

        protected override string GetTableName()
        {
            return "products";
        }
    }
}
