using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.Entities.Order_Aggregate
{
    public class Order :BaseEntity
    {
        // Empty parameterLess constructor must be Exist => EF Core
        public Order() {}
        public Order(string puyerEmail,  Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
        {
            PuyerEmail = puyerEmail;
          //  OrderDate = orderDate;
            //Status = status;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string PuyerEmail { get; set; }
        public DateTimeOffset  OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        public DeliveryMethod  DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal+ DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; } 
    }
}
