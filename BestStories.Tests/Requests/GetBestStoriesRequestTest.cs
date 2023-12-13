using BestStories.Request;
using MediatR;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BestStories.Tests.Requests
{
    [TestFixture]
    public class GetBestStoriesRequestTest
    {
        const int id1 = 38579899;
        const int id2 = 38607424;
        const int id3 = 38602575;

        private static Story story1 = new Story(id1, Title: "story1");
        private static Story story2 = new Story(id2, Title: "story2");
        private static Story story3 = new Story(id3, Title: "story3");

        private Mock<IMediator> _mediator;

        [SetUp]
        public void Init()
        {
            var ids = new List<int>() { id1, id2, id3 };
            _mediator = new Mock<IMediator>();
            _ = _mediator.Setup(mediator => mediator.Send(It.IsAny<GetBestStoriesIdsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(ids);
            _ = _mediator.Setup(mediator => mediator.Send(It.Is<GetStoryRequest>(request => request.Id == id1), It.IsAny<CancellationToken>())).ReturnsAsync(story1);
            _ = _mediator.Setup(mediator => mediator.Send(It.Is<GetStoryRequest>(request => request.Id == id2), It.IsAny<CancellationToken>())).ReturnsAsync(story2);
            _ = _mediator.Setup(mediator => mediator.Send(It.Is<GetStoryRequest>(request => request.Id == id3), It.IsAny<CancellationToken>())).ReturnsAsync(story3);
        }

        [Test, TestCaseSource(nameof(Top1))]
        public async Task CanGetBestStories(int top, ApiStory[] expectedStories)
        {
            var request = new GetBestStoriesRequest(top);
            var handler = new GetBestStoriesHandler(_mediator.Object);
            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedStories));
        }

        private static object[] Top1 =
        {
            new object[] { 1, new ApiStory[] { ApiStory.Create(story1) } },
            new object[] { 2, new ApiStory[] { ApiStory.Create(story1), ApiStory.Create(story2) } },
            new object[] { 3, new ApiStory[] { ApiStory.Create(story1), ApiStory.Create(story2), ApiStory.Create(story3) } }
        };
    }
}
