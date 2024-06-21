using System.ComponentModel.DataAnnotations;

namespace Grocery.Service.Dtos
{
    public class UpdatePasswordDto
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
