using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Grocery.Domain.Entities.Identity;
using Grocery.Repository.Identity;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Grocery.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<AppIdentityDbContext>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ValidateLifetime = true,
                };
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                googleOptions.CallbackPath = new PathString("/signin-google");
            })
            .AddFacebook(facebookOptions =>
             {
                facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.CallbackPath = new PathString("/signin-facebook"); 
             });


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //     .AddJwtBearer(options =>
            //     {
            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuer = true,
            //             ValidIssuer = configuration["JWT:ValidIssuer"],
            //             ValidateAudience = true,
            //             ValidAudience = configuration["JWT:ValidAudience"],
            //             ValidateIssuerSigningKey = true,
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
            //             ValidateLifetime = true,
            //         };
            //     })
            //     .AddGoogle(googleOptions =>
            //     {
            //         googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            //         googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            //         googleOptions.CallbackPath = "/signin-google";
            //     }).AddFacebook(facebookOptions =>
            //    {
            //        facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
            //        facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            //        facebookOptions.SaveTokens = true;
            //        facebookOptions.Events.OnTicketReceived = context =>
            //        {
            //            return Task.CompletedTask;
            //        };
            //        facebookOptions.Events.OnRemoteFailure = context =>
            //        {
            //            context.Response.Redirect("/error?FailureMessage=" + context.Failure.Message);
            //            context.HandleResponse();
            //            return Task.CompletedTask;
            //        };
            //    });
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.Lax;
            //});
            return services;
        }
    }
}
