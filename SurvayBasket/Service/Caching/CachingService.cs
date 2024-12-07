
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SurvayBasket.Service.Caching
{
    public class CachingService(IDistributedCache distributedCache, ILogger<CachingService> logger) : ICachingService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly ILogger<CachingService> _logger = logger;

        public async Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Key))
                return default;

            var result = await _distributedCache.GetStringAsync(Key, cancellationToken);
            if (string.IsNullOrEmpty(result))
                return default;


            try
            {
                JsonSerializer.Deserialize<T>(result);

            }
            catch (Exception ex)
            {
                // Log Error
                _logger.LogError(ex.Message, "Failed To deserialize cache value for key : {key}", Key);
            }

            return default;
        }


        public async Task SetAsync<T>(string Key, T Value, CancellationToken cancellationToken)
          => await _distributedCache.SetStringAsync(Key, JsonSerializer.Serialize<T>(Value), cancellationToken);


        public async Task RemoveAsync(string Key, CancellationToken cancellationToken)
             => await _distributedCache.RemoveAsync(Key, cancellationToken);

    }
}
