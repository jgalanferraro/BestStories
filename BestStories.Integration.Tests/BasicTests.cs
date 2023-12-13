using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BestStories.Integration.Tests
{
    public class BasicTests
    : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BasicTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_EndpointsReturnSuccessAndSortedStories()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/beststories/10");

            var apiStories = await response.Content.ReadFromJsonAsync<IList<ApiStory>>();
            var sortedApiStories = apiStories?.OrderByDescending(story => story.Score).ToList();

            response.EnsureSuccessStatusCode();
            Assert.Equal(apiStories, sortedApiStories);
        }
    }
}