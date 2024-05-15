using Grocery.Domain.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Grocery.Repository.Data.Config
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(orderItem => orderItem.ProductItemOrdered, Product => Product.WithOwner());
            builder.Property(orderItem => orderItem.Price)
                    .HasColumnType("decimal(18,2)");
        }
    }
}
