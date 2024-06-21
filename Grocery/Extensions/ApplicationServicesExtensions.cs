using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Runtime.CompilerServices;
using Grocery.Errors;
using Grocery.Helpers;
using Grocery.Domain.Repositories;
using Grocery.Repository.Data;
using Grocery.Service.Payment;
using Grocery.Domain.Entities.Identity;
using Grocery.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Grocery.Repository.Repository.GenericRepository;
using Grocery.Repository.Repository.BasketRepository;
using Grocery.Domain.IUnitOfWork;
using Grocery.Domain.IServices.IResponseCaching;
using Grocery.Service.CacheResponse;
using Grocery.Domain.IServices.IOrderServices;
using Grocery.Domain.IServices.IPaymentServices;
using Grocery.Domain.IServices.ITokenServices;
using Grocery.Repository.UnitOfWorks;
using Grocery.Service.OrderServices;
using Grocery.Service.TokenServices;
using System.Configuration;
using Grocery.Domain.IServices.MailServices;
using Grocery.Service.MailServices;
using FluentValidation.AspNetCore;

namespace Grocery.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
        
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

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
