using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Domain.Entities.Identity;

namespace Grocery.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Hamed Ajaj",
                    Email = "hamedajaj906@gmail.com",
                    UserName = "hamedajaj906",
                    PhoneNumber = "01033839067"
                };
                await userManager.CreateAsync(user, "Pa$$W0rd");
            }
        }
    }
}
