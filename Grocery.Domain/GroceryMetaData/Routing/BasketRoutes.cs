using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.GroceryMetaData.Routing
{
    public static partial class ApiRouter
    {
        public static class BasketRoutes
        {
            private const string Prefix = $"{BaseRoute}basket";
            public const string Add = $"{Prefix}/add";
            public const string Update = $"{Prefix}/update";
            public const string Delete = $"{Prefix}/delete";
            public const string get = $"{Prefix}";
        }
    }
}
