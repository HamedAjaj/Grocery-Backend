using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; }  // Forigen key =>int -> not allow null vs know this as fk of ProductPrand
        public int ProductTypeId { get; set; }  // Forigen key =>int -> not allow null vs know this as fk of ProductType
        public ProductBrand ProductBrand { get; set; }  // Navigation property [ One ]
        public ProductType ProductType { get; set; }  // Navigation property [ One ]

    }
}
