using Grocery.Domain.Entities;
using Grocery.Domain.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Grocery.Repository.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options):base(options)
        {   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API

            //modelBuilder.ApplyConfiguration(new ProductConfiguration());
            //modelBuilder.ApplyConfiguration(new ProductBrandConfiguration());
            //modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());

            // make apply all configuration classes
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //this line execute exactly  the 3 lines of classes applied
        }
        public DbSet<ProductType>  ProductTypes { get; set; }
        public DbSet<ProductBrand>  ProductBrands { get; set; }
        public DbSet<Product>  Products { get; set; }
        public DbSet<Order>  Orders { get; set; }
        public DbSet<OrderItem>  OrderItems { get; set; }
        public DbSet<DeliveryMethod>  DeliveryMethods { get; set; }

    }
}
