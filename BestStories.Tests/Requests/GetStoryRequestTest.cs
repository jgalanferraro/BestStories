using BestStories.Request;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BestStories.Tests.Requests
{
    [TestFixture]
    public class GetStoryRequestTest
    {
        private const int cachedId = 1;
        private const int httpId = 2;
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
        public async Task CanGetStoryFromHackerNews()
        {
            var expectedStory = new Story(httpId);
            _hackerNewsApiClient.Setup(client => client.GetStoryAsync(httpId)).ReturnsAsync(expectedStory);

            var request = new GetStoryRequest(httpId);
            var handler = new GetStoryHandler(_memoryCache, _hackerNewsApiClient.Object);

            var cachedStory = _memoryCache.Get(httpId);

            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(cachedStory, Is.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedStory));
        }

        [Test]
        public async Task CanGetStoryFromCache()
        {
            var request = new GetStoryRequest(cachedId);
            var handler = new GetStoryHandler(_memoryCache, _hackerNewsApiClient.Object);

            var expectedStory = new Story(cachedId);
            _memoryCache.Set(cachedId, expectedStory);

            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedStory));
        }
    }
}
