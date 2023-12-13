namespace BestStories
{
    public class HackerNewsApiClient : IHackerNewsApiClient
    {
        private const string baseUri = "https://hacker-news.firebaseio.com/v0";
        private readonly HttpClient _httpClient;

        public HackerNewsApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<int>?> GetBestStoriesIdsAsync()
        {
            string requestUri = $"{baseUri}/beststories.json";
            var result = await _httpClient.GetAsync(requestUri);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.ReasonPhrase);
            }

            return await result.Content.ReadFromJsonAsync<IEnumerable<int>>();
        }

        public async Task<Story?> GetStoryAsync(int id)
        {
            string requestUri = $"{baseUri}/item/{id}.json";
            var result = await _httpClient.GetAsync(requestUri);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.ReasonPhrase);
            }

            return await result.Content.ReadFromJsonAsync<Story>();
        }
    }

    public interface IHackerNewsApiClient
    {
        Task<IEnumerable<int>?> GetBestStoriesIdsAsync();
        Task<Story?> GetStoryAsync(int id);
    }
}
