namespace SurvayBasket.Service.Caching
{
    public interface ICachingService
    {
        public Task SetAsync<T>(string Key, T Value, CancellationToken cancellationToken);
        public Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken);
        public Task RemoveAsync(string Key, CancellationToken cancellationToken);
    }
}
