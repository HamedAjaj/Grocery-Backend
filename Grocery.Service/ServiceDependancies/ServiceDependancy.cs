using FluentValidation.AspNetCore;
using Grocery.Domain.IServices.IOrderServices;
using Grocery.Domain.IServices.IPaymentServices;
using Grocery.Domain.IServices.IResponseCaching;
using Grocery.Domain.IServices.ITokenServices;
using Grocery.Domain.IServices.MailServices;
using Grocery.Domain.IUnitOfWork;
using Grocery.Repository.UnitOfWorks;
using Grocery.Service.CacheResponse;
using Grocery.Service.MailServices;
using Grocery.Service.OrderServices;
using Grocery.Service.Payment;
using Grocery.Service.TokenServices;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Grocery.Service.ServiceDependancies
{
    public static class ServiceDependancy
    {
        public static IServiceCollection AddServicesDependancies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped(typeof(IOrderService), typeof(OrderService));

            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IMailService ,MailService>();

            services.AddScoped<IResponseCacheService, ResponseCacheService>();

            // Fluent Validation
            // Use any type from your assembly
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));          
            return services;
        }

    }
}
