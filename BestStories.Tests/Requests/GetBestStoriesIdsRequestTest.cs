using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using NUnit.Framework.Internal;
using BestStories.Request;

namespace BestStories.Tests.Requests
{
    [TestFixture]
    public class GetBestStoriesIdsRequestTest
    {
        private IMemoryCache _memoryCache;
        private const string key = "beststories";

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
        public async Task CanGetBestStoriesIdsFromHackerNews()
        {
            var request = new GetBestStoriesIdsRequest();
            var handler = new GetBestStoriesIdsHandler(_memoryCache);

            var cachedIds = _memoryCache.Get(key);

            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(cachedIds, Is.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(200));
        }

        [Test]
        public async Task CanGetBestStoriesIdsFromCache()
        {
            var request = new GetBestStoriesIdsRequest();
            var handler = new GetBestStoriesIdsHandler(_memoryCache);

            var cachedIds = new List<int>() { 1, 2, 3 };
            _memoryCache.Set(key, cachedIds);

            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(cachedIds));
        }
    }
}
