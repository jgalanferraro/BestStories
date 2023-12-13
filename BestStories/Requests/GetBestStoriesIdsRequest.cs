using MediatR;
using Microsoft.Extensions.Caching.Memory;


namespace BestStories.Request
{
    public class GetBestStoriesIdsRequest: IRequest<IEnumerable<int>?>
    {
    }

    public class GetBestStoriesIdsHandler(IMemoryCache memoryCache, IHackerNewsApiClient apiClient) : IRequestHandler<GetBestStoriesIdsRequest, IEnumerable<int>?>
    {
        private const string key = "beststories";
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<IEnumerable<int>?> Handle(GetBestStoriesIdsRequest request, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreateAsync(key,
                async cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
                    return await apiClient.GetBestStoriesIdsAsync();
                });
        }
    }
}
