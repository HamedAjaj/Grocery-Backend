using Grocery.Domain.Entities.Identity;
using Grocery.Domain.IServices.ITokenServices;
using Grocery.Service.Dtos.UserAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Service.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AppUser>  ValidateGoogleToken(string token)
        {
            var client = new HttpClient();
            var payload = JObject.Parse(await client.GetStringAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={token}"));

            var email = payload["email"]?.ToString();
            var name = payload["name"]?.ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name)) return null;
            return new AppUser { Email = email, DisplayName = name , IsEmailVerified = true };
        }

        public async Task<AppUser> ValidateFacebookToken(string token)
        {
            var client = new HttpClient();
            var payload = JObject.Parse(await client.GetStringAsync($"https://graph.facebook.com/me?access_token={token}&fields=id,name,email"));

            var email = payload["email"]?.ToString();
            var name = payload["name"]?.ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name)) return null;
            return new AppUser { Email = email, DisplayName = name, IsEmailVerified = true };
        }

        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {

            //private Claims
            var authClaims = new List<Claim>(){
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };


            // if user have roles
            var userRoles = await userManager.GetRolesAsync(user);
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // signature 
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            //creating token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                //Private Claims
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );
            // Generate token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }




   

    }
}
