using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.Entities.Identity
{
    public class AppUser:IdentityUser
    {
        public string? OTP { get; set; }
        public DateTime? OTPExpiration { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public string DisplayName { get; set; }
        public Address Address { get; set; }  // Nav Prop [One]
    }
}
