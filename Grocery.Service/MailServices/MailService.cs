using Grocery.Domain.Entities;
using Grocery.Domain.Entities.Identity;
using Grocery.Domain.IServices.MailServices;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Grocery.Service.MailServices
{
    public class MailService : IMailService
    {

        private readonly MailSettings _mailSettings;
        private readonly UserManager<AppUser> _userManager;
        public MailService(IOptions<MailSettings> mailSettings, UserManager<AppUser> userManager)
        {
            _mailSettings = mailSettings.Value;
            _userManager = userManager;
        }

        public async Task SendEmailAsync(string mailTo, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));
            message.To.Add(new MailboxAddress("",mailTo));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder { HtmlBody= body };
            message.Body=bodyBuilder.ToMessageBody();
            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, false);
            await client.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password.ToString());
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task<bool> SendOtp(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)  return false;
            var otp = GenerateOTP();
            user.OTP = otp;
            user.OTPExpiration = DateTime.Now.AddMinutes(10);
            await _userManager.SetAuthenticationTokenAsync(user,"OTP","otp",otp);
            var subject = "Your OTP Code";
            var body = $"<html><body><h1>Your verification code is : <span>{otp}</span> </h1></body></html>";
            await SendEmailAsync(email, subject, body);
            return true;
        }

        public async Task<bool> VerifyOTPActivateAccountAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (!IsValidOTP(user,otp))  return false;
            UpdateUserEmailVerification(user);
            var result= await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> VerifyOTPForgetPasswordAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (!IsValidOTP(user,otp))  return false;
            user.OTP = null;
            user.OTPExpiration = null;
            var result= await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        private string GenerateOTP()
        {
            return new Random().Next(100000, 999999).ToString();
        }



        private bool IsValidOTP(AppUser user, string otp)
        {
            return user.OTP == otp && user.OTPExpiration > DateTime.Now;
        }

        private void UpdateUserEmailVerification(AppUser user)
        {
            user.IsEmailVerified = true;
            user.OTP = null;
            user.OTPExpiration = DateTime.MinValue;
        }
    }
}
