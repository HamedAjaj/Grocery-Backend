using AutoMapper;
using Grocery.Domain.Entities;
using Grocery.Domain.Specifications;
using Grocery.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using Grocery.Helpers;
using Grocery.Domain.IUnitOfWork;
using Grocery.Helpers.Attributes;
using Grocery.Domain.GroceryMetaData.Routing;

namespace Grocery.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Cache(1000)]
        [HttpGet(ApiRouter.ProductRoutes.get)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(productParams);
            var products = await _unitOfWork.Respository<Product>().GetAllWithSpecificationAsync(spec);
            var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countSpec = new ProductWithFilterationForCountSpecefication(productParams);
            var Count = await _unitOfWork.Respository<Product>().GetCountWithSpecAsync(countSpec);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, Count, Data));
        }

        [Cache(1000)]
        [HttpGet(ApiRouter.ProductRoutes.getById)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.Respository<Product>().GetByIdWithSpecificationAsync(spec);
            if (product == null) return Ok(new { Message="Data not found"});
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [Cache(1000)]
        [HttpGet(ApiRouter.ProductRoutes.getBrands)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands() =>
            Ok(await _unitOfWork.Respository<ProductBrand>().GetAllAsync());
        

        [Cache(1000)]
        [HttpGet(ApiRouter.ProductRoutes.getTypes)]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes() =>
            Ok(await _unitOfWork.Respository<ProductType>().GetAllAsync());

    }
}
