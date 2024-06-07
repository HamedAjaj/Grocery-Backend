using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.GroceryMetaData.Routing
{
    public static partial class ApiRouter
    {
        private const string Root = "Api";
        private const string Version = "V1";
        public const string BaseRoute = $"{Root}/{Version}/";
    }
}
