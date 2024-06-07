using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Grocery.Domain.Entities;
using Grocery.Domain.Repositories;

namespace Grocery.Repository.Repository.BasketRepository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _redisDb;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }


        public async Task<bool> DeleteBasketAsync(string basketId) => await _redisDb.KeyDeleteAsync(basketId);
        
        public async Task<CustomerBasket?> GetBasketAsync(string basketId) //get or recreate
        {
            var basket = await _redisDb.StringGetAsync(basketId);
            return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var createOrUpdateBasket = await _redisDb.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(1));
            return createOrUpdateBasket ?  await GetBasketAsync(basket.Id) : null;
        }
    }
}
