using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.IServices.IResponseCaching
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cachKey, object response , TimeSpan liveTime );
        Task<string> GetCachedResponseAsync(string cachKey);
    }
}
