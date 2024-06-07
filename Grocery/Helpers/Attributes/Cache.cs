using Grocery.Domain.IServices.IResponseCaching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Grocery.Helpers.Attributes
{
    public class Cache : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public Cache(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedReponse = await cacheService.GetCachedResponseAsync(cacheKey);
            if(!string.IsNullOrEmpty(cachedReponse)) 
            {
                var contentResult = new ContentResult
                {
                    Content = cachedReponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var executedEndpointContext = await next();
            if (executedEndpointContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
