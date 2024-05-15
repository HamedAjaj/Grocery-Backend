using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Domain.Entities;

namespace Grocery.Domain.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product>
    {
        // contructor used to get all products
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams specParams)
            :base(P =>
                (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
                (!specParams.brandId.HasValue || P.ProductBrandId == specParams.brandId.Value) &&
                (!specParams.typeId.HasValue || P.ProductTypeId == specParams.typeId.Value)
            )
        {
        Includes.Add(P => P.ProductBrand);
        Includes.Add(P => P.ProductType);

        AddOrderBy(P => P.Name);

        if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch(specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price); //this is more readable if it act as function
                      // Or   //OrderBy = p => p.Price; instead of function
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

            //Totalproducts =23
            //pages size = 5
            //pageIndex = 1
        ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }

        // contructor used to get product by Id
        public ProductWithBrandAndTypeSpecifications(int id):base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
