using AutoMapper;
using AutoMapper.Execution;
using Microsoft.Extensions.Configuration;
using Grocery.Domain.Entities;
using Grocery.Service.Dtos;

namespace Grocery.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        =>!string.IsNullOrEmpty(source.PictureUrl) ? $"{Configuration["ApiUrl"]}{source.PictureUrl}" : string.Empty;
        
    }
}
