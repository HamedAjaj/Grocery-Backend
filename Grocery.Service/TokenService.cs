using Grocery.Domain.Entities.Identity;
using Grocery.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        //public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        //{
        //    // Validate configuration values
        //    var validIssuer = _configuration["JWT:ValidIssuer"];
        //    var validAudience = _configuration["JWT:ValidAudience"];
        //    var durationInDays = double.Parse(_configuration["JWT:DurationInDays"]);

        //        // Handle invalid configuration values
        //    if (string.IsNullOrEmpty(validIssuer) || string.IsNullOrEmpty(validAudience) || durationInDays <= 0)
        //    {
        //        throw new InvalidOperationException("Invalid JWT configuration values");
        //    }

        //    // Token claims
        //    var authClaims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.Name, user.DisplayName),
        //        new Claim(ClaimTypes.Email, user.Email),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id),
        //        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        //    };

        //    // Add user roles as claims
        //    var userRoles = await userManager.GetRolesAsync(user);
        //    authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        //    // Token signature key
        //    var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));

        //    var signingCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature);
        //    // Create JWT token
        //    var token = new JwtSecurityToken(
        //        issuer: validIssuer,
        //        audience: validAudience,
        //        expires: DateTime.Now.AddDays(durationInDays),
        //        claims: authClaims,
        //        signingCredentials: signingCredentials
        //    );

        //    // Generate token
        //    return new JwtSecurityTokenHandler().WriteToken(token);


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
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));

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
