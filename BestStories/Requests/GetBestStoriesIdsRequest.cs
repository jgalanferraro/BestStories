using MediatR;
using Microsoft.Extensions.Caching.Memory;


namespace BestStories.Request
{
    public class GetBestStoriesIdsRequest: IRequest<IEnumerable<int>?>
    {
    }

    public class GetBestStoriesIdsHandler(IMemoryCache memoryCache) : IRequestHandler<GetBestStoriesIdsRequest, IEnumerable<int>?>
    {
        private const string key = "beststories";
        private const string requestUri = "https://hacker-news.firebaseio.com/v0/beststories.json";
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<IEnumerable<int>?> Handle(GetBestStoriesIdsRequest request, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreateAsync(key,
                async cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
                    return await GetBestStoriesIdsAsync();
                });
        }

        private async Task<IEnumerable<int>?> GetBestStoriesIdsAsync()
        {
            var client = new HttpClient();
            var result = await client.GetAsync(requestUri);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.ReasonPhrase);
            }

            return await result.Content.ReadFromJsonAsync<IEnumerable<int>>();
        }
    }
}
