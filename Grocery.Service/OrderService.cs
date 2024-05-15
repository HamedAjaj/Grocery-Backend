using Grocery.Domain;
using Grocery.Domain.Entities;
using Grocery.Domain.Entities.Order_Aggregate;
using Grocery.Domain.Repositories;
using Grocery.Domain.Services;
using Grocery.Domain.Specifications.Order_Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get basket from basket repo
            var basket = await _basketRepository.GetBasketAsync(basketId);
            // 2. Get selected items at basket from products repo
            var orderItems = new List<OrderItem>();
            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Respository<Product>().GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);

                }

            }
            // 3. Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get delivery method form deliveryMethods repo

            var deliveryMehtod = await _unitOfWork.Respository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 5. Create Order
            #region For check if order is exist , to prevent user to create more than order with the same PaymentIntentId , to prevent this hole
            var spec = new OrderWithPaymentIntentIdwithSpecification(basket.PaymentIntentId); // where in query
            var existingOrder =  await _unitOfWork.Respository<Order>().GetByIdWithSpecificationAsync(spec);

            if (existingOrder != null)
            {
                _unitOfWork.Respository<Order>().Delete(existingOrder);// order with paymentIntentId of order for basket with old order or updated order
                _paymentService.CreateOrUpdatePaymentIntent(basket.Id);// paymentIntentId of order for basket with new order or updated order
            }
          
            var order = new Order(buyerEmail, shippingAddress, deliveryMehtod, orderItems, subTotal,basket.PaymentIntentId);
            #endregion
            await _unitOfWork.Respository<Order>().Add(order);

            // 6. Save To databases
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.Respository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderSpecifications(orderId, buyerEmail);
            var order = await _unitOfWork.Respository<Order>().GetByIdWithSpecificationAsync(spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersforUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await _unitOfWork.Respository<Order>().GetAllWithSpecificationAsync(spec);
            return orders;
        }
    }
}
