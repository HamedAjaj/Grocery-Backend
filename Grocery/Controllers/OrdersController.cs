using AutoMapper;
using Grocery.Domain.Entities.Order_Aggregate;
using Grocery.Domain.IServices.IOrderServices;
using Grocery.Service.Dtos;
using Grocery.Errors;
using Grocery.Helpers.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Grocery.Domain.GroceryMetaData.Routing;
namespace Grocery.Controllers
{
    [Authorize]    [ApiController]

    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost(ApiRouter.OrderRoutes.Create)]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            string buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orderAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, orderAddress);
            return order != null ? Ok(order) :
                 BadRequest(new ApiResponse(400, "An error occured during the creation of the order"));
        }


        [Cache(1000)]
        [HttpGet(ApiRouter.OrderRoutes.getOrderForUser)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            string buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersforUserAsync(buyerEmail);
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [Cache(1000)]
        [HttpGet(ApiRouter.OrderRoutes.getById)]
        public async Task<ActionResult<OrderToReturnDto>> GetUserOrder(int id)
        {
            string buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(id, buyerEmail);
            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [Cache(1000)]
        [HttpGet(ApiRouter.OrderRoutes.getdeliveryMethods)]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods() =>
             Ok(await _orderService.GetDeliveryMethodsAsync());
    }
}
