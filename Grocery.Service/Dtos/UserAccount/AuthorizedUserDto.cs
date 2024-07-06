using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Service.Dtos.UserAccount
{
    public class AuthorizedUserDto
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsVerified { get; set; }
    }
}
