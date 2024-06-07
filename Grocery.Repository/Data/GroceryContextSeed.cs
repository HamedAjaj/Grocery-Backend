using Grocery.Domain.Entities;
using Grocery.Domain.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Grocery.Repository.Data
{
    public static class GroceryContextSeed
    {
        public static async Task SeedAsync(GroceryContext context)
        {
            if(!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Grocery.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if(brands?.Count > 0)
                {
                    var brands2 = brands.Select(b => new ProductBrand() { Name = b.Name });
                    foreach(var brand in brands2) 
                        await context.Set<ProductBrand>().AddAsync(brand);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.ProductTypes.Any())
            {
                var productTypeData = File.ReadAllText("../Grocery.Repository/Data/DataSeed/types.json");
                var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);

                if (productTypes?.Count > 0)
                {
                    var productTypes02 = productTypes.Select(b => new ProductType() { Name = b.Name });
                    foreach (var productType in productTypes02)
                    {
                        await context.Set<ProductType>().AddAsync(productType);
                    }
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../Grocery.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        await context.Set<Product>().AddAsync(product);
                    }
                    await context.SaveChangesAsync();

                }
            }
            if (!context.DeliveryMethods.Any())
            {
                var deliveryMethodsData = File.ReadAllText("../Grocery.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods?.Count > 0)
                {
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        await context.Set<DeliveryMethod>().AddAsync(deliveryMethod);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
