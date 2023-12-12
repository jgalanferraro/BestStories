using MediatR;
using Microsoft.AspNetCore.OData.Query;


namespace BestStories.Request
{
    public class GetBestStoriesByQueryRequest(ODataQueryOptions<SortedStory> options) : IRequest<IEnumerable<ApiStory>>
    {
        public ODataQueryOptions<SortedStory> Options { get; } = options;
    }

    public class GetBestStoriesByQueryHandler(IMediator mediator) : IRequestHandler<GetBestStoriesByQueryRequest, IEnumerable<ApiStory>>
    {
        private readonly IMediator _mediator = mediator;

        public async Task<IEnumerable<ApiStory>> Handle(GetBestStoriesByQueryRequest request, CancellationToken cancellationToken)
        {
            var ids = await _mediator.Send(new GetBestStoriesIdsRequest()) ?? Enumerable.Empty<int>();

            var sortedStories = ids.Select((id, index) => new SortedStory(index, id)).AsQueryable();

            sortedStories = (IQueryable<SortedStory>)request.Options.ApplyTo(sortedStories);

            return await GetStoriesAsync(sortedStories);
        }

        private async Task<IEnumerable<ApiStory>> GetStoriesAsync(IQueryable<SortedStory> sortedStories)
        {
            var apiStories = new List<ApiStory>(sortedStories.Count());
            foreach (var sortedStory in sortedStories)
            {
                var story = await _mediator.Send(new GetStoryRequest(sortedStory.StoryId));

                if (story is not null)
                {
                    apiStories.Add(new ApiStory(story));
                }
            }

            return apiStories;
        }

       
    }
}
