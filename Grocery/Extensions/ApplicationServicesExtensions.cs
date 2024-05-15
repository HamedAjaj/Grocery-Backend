using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Runtime.CompilerServices;
using Grocery.Errors;
using Talabat.APIs.Helpers;
using Grocery.Domain.Repositories;
using Grocery.Domain.Services;
using Grocery.Repository.Data;
using Grocery.Service;
using Grocery.Service.Payment;
using Grocery.Repository;
using Grocery.Domain;

namespace Grocery.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            //services.AddScoped<IGenericRepository<>, GenericRepository<>>();
            services.AddScoped(typeof(IGenericRespository<>), typeof(GenericRepository<>));


            //services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            services.AddAutoMapper(typeof(MappingProfiles));


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count() > 0)
                                                         .SelectMany(M => M.Value.Errors)
                                                         .Select(E => E.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
