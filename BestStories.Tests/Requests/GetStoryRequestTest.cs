using BestStories.Request;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BestStories.Tests.Requests
{
    [TestFixture]
    public class GetStoryRequestTest
    {
        private IMemoryCache _memoryCache;
        private const int id = 21233041;

        [SetUp]
        public void Init()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [TearDown]
        public void Teardown()
        {
            _memoryCache.Dispose();
        }

        [Test]
        public async Task CanGetStoryFromHackerNews()
        {
            var request = new GetStoryRequest(id);
            var handler = new GetStoryHandler(_memoryCache);

            var cachedStory = _memoryCache.Get(id);

            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(cachedStory, Is.Null);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanGetBestStoriesIdsFromCache()
        {
            var request = new GetStoryRequest(id);
            var handler = new GetStoryHandler(_memoryCache);

            var cachedStory = new Story(id);
            _memoryCache.Set(id, cachedStory);

            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(cachedStory));
        }
    }
}
