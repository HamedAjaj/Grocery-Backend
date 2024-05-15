using Grocery.Domain.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.Specifications.Order_Spec
{
    public class OrderSpecifications : BaseSpecification<Order>
    {
        // get all orders for specific user 
        public OrderSpecifications(string email):base(ord=> ord.PuyerEmail == email)
        {
            Includes.Add(ord => ord.DeliveryMethod);
            Includes.Add(ord => ord.Items);

            AddOrderByDesc(ord => ord.OrderDate);

        }


        // get  specific order for Authorized user
        public OrderSpecifications( int id,string email) : base(ord => ord.PuyerEmail == email && ord.Id== id)
        {
            Includes.Add(ord => ord.DeliveryMethod);
            Includes.Add(ord => ord.Items);
        }
    }
}
