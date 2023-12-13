using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using NUnit.Framework.Internal;
using BestStories.Request;
using Moq;

namespace BestStories.Tests.Requests
{
    [TestFixture]
    public class GetBestStoriesIdsRequestTest
    {
        private const string key = "beststories";

        private IMemoryCache _memoryCache;
        private Mock<IHackerNewsApiClient> _hackerNewsApiClient;

        [SetUp]
        public void Init()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _hackerNewsApiClient = new Mock<IHackerNewsApiClient>();
        }

        [TearDown]
        public void Teardown()
        {
            _memoryCache.Dispose();
        }

        [Test]
        public async Task CanGetBestStoriesIdsFromHackerNews()
        {
            var expectedIds = new List<int>() { 1, 2, 3 };
            _hackerNewsApiClient.Setup(client => client.GetBestStoriesIdsAsync()).ReturnsAsync(expectedIds);

            var request = new GetBestStoriesIdsRequest();
            var handler = new GetBestStoriesIdsHandler(_memoryCache, _hackerNewsApiClient.Object);

            var cachedIds = _memoryCache.Get(key);

            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(cachedIds, Is.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedIds));
        }

        [Test]
        public async Task CanGetBestStoriesIdsFromCache()
        {
            var request = new GetBestStoriesIdsRequest();
            var handler = new GetBestStoriesIdsHandler(_memoryCache, _hackerNewsApiClient.Object);

            var expectedIds = new List<int>() { 1, 2, 3 };
            _memoryCache.Set(key, expectedIds);

            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedIds));
        }
    }
}
