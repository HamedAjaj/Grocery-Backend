using AutoMapper;
using Grocery.Domain.Entities;
using Grocery.Domain.Specifications;
using Grocery.Dtos;
using Grocery.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Grocery.Helpers;
using Grocery.Domain.IUnitOfWork;
using Grocery.Helpers.Attributes;

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
        [HttpGet]
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
        [HttpGet("id")] // GET: /api/Products/id
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.Respository<Product>().GetByIdWithSpecificationAsync(spec);
            if (product == null) return Ok(new { Message="Data not found"});
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [Cache(1000)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Respository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }

        [Cache(1000)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Respository<ProductType>().GetAllAsync();
            return Ok(types);
        }
    }
}
