using Domain.Products.Entities;
using Infrastructure.Shared.Persistance.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Products.Persistance.Mappings
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
