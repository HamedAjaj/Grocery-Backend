using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Grocery.Domain.IServices.MailServices
{
    public interface IMailService
    {
        Task<bool> SendOtp(string email);
        Task SendEmailAsync(string mailTo, string subject, string body);
        Task<bool> VerifyOTPActivateAccountAsync(string email, string otp);

    }
}
