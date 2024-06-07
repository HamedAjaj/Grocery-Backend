using Grocery.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Domain.Specifications.Order_Spec;
using Product = Grocery.Domain.Entities.Product;
using Grocery.Domain.Entities;
using Grocery.Domain.Entities.Order_Aggregate;
using Grocery.Domain.IUnitOfWork;
using Grocery.Domain.IServices.IPaymentServices;

namespace Grocery.Service.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
       public PaymentService(IConfiguration configuration, IBasketRepository basketRepo,IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            //use secretkey in backend but, publishable key used in angular fronend
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Respository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value); //.DeliveryMethodId.Value value because is nullable
                basket.ShippingCostPrice = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;
            }
            
            if(basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Respository<Product>().GetByIdAsync(item.Id);
                    if(item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // create payment intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long) shippingPrice *100,
                    Currency="EGP",
                    PaymentMethodTypes = new List<string>() {"card"}
                };
                paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else   // / update payment intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100,
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatePaymentIntentToSucceededORFailed(string paymentIntentId, bool isSucceeded)
        {
            var spec = new OrderWithPaymentIntentIdwithSpecification(paymentIntentId);
            var orderSpec = await _unitOfWork.Respository<Order>().GetByIdWithSpecificationAsync(spec);

            if (isSucceeded)
                orderSpec.Status = OrderStatus.PaymentReceived;
            else
                orderSpec.Status = OrderStatus.PaymentFailed;

            _unitOfWork.Respository<Order>().Update(orderSpec);
            await  _unitOfWork.Complete();
           
            return orderSpec;
        }
    }
}
