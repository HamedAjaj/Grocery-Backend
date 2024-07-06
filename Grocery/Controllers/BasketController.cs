using AutoMapper;
using Grocery.Domain.Entities;
using Grocery.Domain.Repositories;
using Grocery.Service.Dtos;
using Grocery.Errors;
using Grocery.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Grocery.Domain.GroceryMetaData.Routing;

namespace Grocery.Controllers
{
    [ApiController]

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        // used to get or recreate basket by id
        [Cache(1000)]
        [HttpGet(ApiRouter.BasketRoutes.get)]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost(ApiRouter.BasketRoutes.AddOrUpdate)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var updatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            return Ok(updatedBasket !=null? updatedBasket : new ApiResponse(400));
        }

        [HttpDelete(ApiRouter.BasketRoutes.Delete)]
        public async Task DeleteBasket(string id) => await _basketRepository.DeleteBasketAsync(id);
        
    }
}
