using MediatR;


namespace BestStories.Request
{
    public class GetBestStoriesRequest(int top) : IRequest<IEnumerable<ApiStory>>
    {
        public int Top { get; } = top;
    }

    public class GetBestStoriesHandler(IMediator mediator) : IRequestHandler<GetBestStoriesRequest, IEnumerable<ApiStory>>
    {
        private readonly IMediator _mediator = mediator;

        public async Task<IEnumerable<ApiStory>> Handle(GetBestStoriesRequest request, CancellationToken cancellationToken)
        {
            var ids = await _mediator.Send(new GetBestStoriesIdsRequest()) ?? Enumerable.Empty<int>();

            ids = ids.Take(request.Top);

            return await GetStoriesAsync(ids);
        }

        private async Task<IEnumerable<ApiStory>> GetStoriesAsync(IEnumerable<int> ids)
        {
            var apiStories = new List<ApiStory>(ids.Count());
            foreach (var id in ids)
            {
                var story = await _mediator.Send(new GetStoryRequest(id));

                if (story is not null)
                {
                    apiStories.Add(ApiStory.Create(story));
                }
            }

            return apiStories;
        }
    }
}
