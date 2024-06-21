using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.GroceryMetaData.Routing
{
    public static partial class ApiRouter
    {
        public static class PaymentRoutes
        {
            private const string Prefix = $"{BaseRoute}Payments";
            public const string AddOrUpdateBasketid = $"{Prefix}/{{basketid}}";
            public const string Webhook = $"{Prefix}/webhook";

        }
    }
}
