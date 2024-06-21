using System.ComponentModel.DataAnnotations;

namespace Grocery.Service.Dtos.OTP
{
    public class VerifyOTPDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(6 , ErrorMessage ="Count of numbers must be 6")]
        public string otp { get; set; }
    }
}
