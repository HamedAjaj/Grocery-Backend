using AutoMapper;
using Microsoft.Extensions.Configuration;
using Grocery.Service.Dtos;
using Grocery.Domain.Entities.Order_Aggregate;

namespace Grocery.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public IConfiguration Configuration { get; }
        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        => !string.IsNullOrEmpty(source.ProductItemOrdered.PictureUrl) ?
            $"{Configuration["ApiUrl"]}{source.ProductItemOrdered.PictureUrl}": string.Empty;
    }
}
