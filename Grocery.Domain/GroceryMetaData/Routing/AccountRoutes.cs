using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.GroceryMetaData.Routing
{
    public static partial class ApiRouter
    {
        public static class AccountRoutes
        {
            private const string Prefix = $"{BaseRoute}account";
            public const string Register = $"{Prefix}/register";
            public const string Login = $"{Prefix}/login";
            public const string SendOtp = $"{Prefix}/sendOtp";
            public const string ActivateUser = $"{Prefix}/activateUser";
            public const string UpdatePassword = $"{Prefix}/update-password";
            public const string GetAddress = $"{Prefix}/address";
            public const string UpdateAddress = $"{Prefix}/address";

        }
    }
}
