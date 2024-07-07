using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Domain.Entities.Identity;

namespace Grocery.Domain.IServices.ITokenServices
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
        Task<AppUser> ValidateGoogleToken(string token);
        Task<AppUser> ValidateFacebookToken(string token);
    }
}
