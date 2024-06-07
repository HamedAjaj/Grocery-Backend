using Grocery.Domain.IServices.IResponseCaching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Grocery.Service.CacheResponse
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _redisDb;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string cachKey, object response, TimeSpan liveTime)
        {
            if (response == null) return;
            // conver response to camelCase to be understood by frontend
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializeResponse = JsonSerializer.Serialize(response,options);
            await _redisDb.StringSetAsync(cachKey, serializeResponse ,liveTime);
        }

        public async Task<string> GetCachedResponseAsync(string cachKey)
        {
            var cachedResponse = await _redisDb.StringGetAsync(cachKey);
            return cachedResponse.IsNullOrEmpty ? null : cachedResponse.ToString();
        }
    } 
}
