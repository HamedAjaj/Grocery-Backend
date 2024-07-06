using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.GroceryMetaData.Routing
{
    public static partial class ApiRouter
    {
        public static class OrderRoutes
        {
            private const string Prefix = $"{BaseRoute}orders";
            public const string Create = $"{Prefix}/create";
         //   public const string Update = $"{Prefix}/update";
         //   public const string Delete = $"{Prefix}/delete";
            public const string getById = $"{Prefix}/i{{id}}";
            public const string getOrderForUser = $"{Prefix}/userOrder/{{id}}";
            public const string getdeliveryMethods = $"{Prefix}/deliveryMethods";

        }
    }
}
