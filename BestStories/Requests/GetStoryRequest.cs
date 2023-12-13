using MediatR;
using Microsoft.Extensions.Caching.Memory;


namespace BestStories.Request
{
    public class GetStoryRequest(int id) : IRequest<Story?>
    {
        public int Id { get; } = id;
    }

    public class GetStoryHandler(IMemoryCache memoryCache, IHackerNewsApiClient apiClient) : IRequestHandler<GetStoryRequest, Story?>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<Story?> Handle(GetStoryRequest request, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreateAsync(request.Id,
                async cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(300);
                    return await apiClient.GetStoryAsync(request.Id);
                });
        }
    }
}
