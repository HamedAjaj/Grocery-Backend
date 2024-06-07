using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Domain.Entities.Order_Aggregate;

namespace Grocery.Domain.IServices.IOrderServices
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersforUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
