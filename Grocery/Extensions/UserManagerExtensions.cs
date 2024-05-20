﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Grocery.Domain.Entities.Identity;

namespace Grocery.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var userWithAddress = await userManager.Users.Include(U => U.Address).SingleOrDefaultAsync(U => U.Email == email);

            return userWithAddress;
        }
    }
}
