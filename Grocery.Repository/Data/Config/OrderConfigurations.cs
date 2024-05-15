using Grocery.Domain.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Grocery.Repository.Data.Config
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

            builder.Property(O => O.Status)
                .HasConversion(
                    OStatus => OStatus.ToString(),
                    OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
                 );
            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(O => O.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
