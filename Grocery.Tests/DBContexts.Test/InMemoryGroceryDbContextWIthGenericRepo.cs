using Grocery.Domain.Entities;
using Grocery.Domain.Specifications;
using Grocery.Repository.Data;
using Grocery.Repository.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Tests.DBContexts.Test
{
    public class InMemoryGroceryDbContextWIthGenericRepo : IDisposable
    {
        private readonly GroceryContext _groceryDB;

        // create Memory Db 
        public InMemoryGroceryDbContextWIthGenericRepo()
        {
            var options = new DbContextOptionsBuilder<GroceryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _groceryDB = new GroceryContext(options);
        }

        public void Dispose()
        {
            _groceryDB.Database.EnsureDeleted();
            _groceryDB.Dispose();

        }

        [Fact]
        public void GetAllProducts_WhenIGetProducts_AllProductsReturnedAsList()
        {

            //Arrange
            var repository = new GenericRepository<Product>(_groceryDB);
            var products = new Product[]
             {
                new Product
                {
                    Name = "White Chocolate Mocha Frappuccino",
                    Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
                    Price = 150,
                    PictureUrl = "images/products/sb-ang2.png",
                    ProductTypeId = 1,
                    ProductBrandId = 1
                },
                new Product
                {
                    Name = "Caramel Macchiato",
                    Description = "Donec odio justo, sollicitudin ut, suscipit a, feugiat et, eros.",
                    Price = 200,
                    PictureUrl = "images/products/sb-caramel.png",
                    ProductTypeId = 2,
                    ProductBrandId = 2
                },
                new Product
                {
                    Name = "Espresso",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                    Price = 100,
                    PictureUrl = "images/products/sb-espresso.png",
                    ProductTypeId = 3,
                    ProductBrandId = 3
                }
             };


            _groceryDB.Products.AddRange(products);
            _groceryDB.SaveChanges();
            //Act
            var result = repository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
        }


        [Fact]
        public void GetById_WhenGivingProductId_ReturnProductById() {
            //Arrange
            var repository = new GenericRepository<Product>(_groceryDB);
            var product = new Product
            {
                Id=1,
                Name = "White Chocolate Mocha Frappuccino",
                Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
                Price = 150,
                PictureUrl = "images/products/sb-ang2.png",
                ProductTypeId = 1,
                ProductBrandId = 1
            };
            _groceryDB.Products.Add(product);
            _groceryDB.SaveChanges();

            var spec = new ProductWithBrandAndTypeSpecifications(product.Id);

            //Act
            var result = repository.GetByIdWithSpecificationAsync(spec);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);

        }
    }
}
