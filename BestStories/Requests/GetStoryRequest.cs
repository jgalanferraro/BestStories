using MediatR;
using Microsoft.Extensions.Caching.Memory;
using BestStories;


namespace BestStories.Request
{
    public class GetStoryRequest(int id) : IRequest<Story?>
    {
        public int Id { get; } = id;
    }

    public class GetStoryHandler(IMemoryCache memoryCache) : IRequestHandler<GetStoryRequest, Story?>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<Story?> Handle(GetStoryRequest request, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreateAsync(request.Id,
                async cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(300);
                    return await GetStoryAsync(request.Id);
                });
        }

        private async Task<Story?> GetStoryAsync(int id)
        {
            string requestUri = $"https://hacker-news.firebaseio.com/v0/item/{id}.json";
            var client = new HttpClient();
            var result = await client.GetAsync(requestUri);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.ReasonPhrase);
            }

            return await result.Content.ReadFromJsonAsync<Story>();
        }
    }
}
