using System.ComponentModel.DataAnnotations;

namespace Grocery.Service.Dtos.OTP
{
    public class SendOtpRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
