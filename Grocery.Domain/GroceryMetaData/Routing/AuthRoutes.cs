using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.GroceryMetaData.Routing
{
    public static partial class ApiRouter
    {
        public static class AuthRoutes
        {

            private const string Prefix = $"{BaseRoute}auth";

            public const string Get = $"{Prefix}";
            public const string AuthWithGoogle = $"{Prefix}/signin-google";
            public const string AuthWithFacebook = $"{Prefix}/signin-facebook";
            public const string GoogleCallBack = $"{Prefix}/google-callback";
            public const string FcaebookCallBack = $"{Prefix}/facebook-callback";
            public const string Register = $"{Prefix}/register";
            public const string Login = $"{Prefix}/login";
            public const string SendOtp = $"{Prefix}/sendOtp";
            public const string ActivateUser = $"{Prefix}/activateUser";
            public const string UpdatePassword = $"{Prefix}/update-password";
        }
    }
}
