using Grocery.Domain.Entities;
using Grocery.Domain.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.IServices.IPaymentServices
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdatePaymentIntentToSucceededORFailed(string paymentIntentId, bool isSucceeded);
    }
}
