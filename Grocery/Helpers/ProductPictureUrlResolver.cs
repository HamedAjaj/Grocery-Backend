using AutoMapper;
using AutoMapper.Execution;
using Microsoft.Extensions.Configuration;
using Grocery.Domain.Entities;
using Grocery.Dtos;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{Configuration["ApiUrl"]}{source.PictureUrl}";
            return null;
        }
    }
}
