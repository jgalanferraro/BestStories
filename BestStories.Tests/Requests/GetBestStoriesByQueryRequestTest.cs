using BestStories.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BestStories.Tests.Requests
{
    [TestFixture]
    public class GetBestStoriesByQueryRequestTest
    {
        const int id1 = 38579899;
        const int id2 = 38607424;
        const int id3 = 38602575;
        Story _story1;
        Story _story2;
        Story _story3;
        private ODataQueryContext _oDataQueryContext;
        private HttpRequest _httpRequest;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Init()
        {
            _story1 = new Story(id1, Title: "story1");
            _story2 = new Story(id2, Title: "story2");
            _story3 = new Story(id3, Title: "story3");

            var ids = new List<int>() { id1, id2, id3 };
            _mediator = new Mock<IMediator>();
            _ = _mediator.Setup(mediator => mediator.Send(It.IsAny<GetBestStoriesIdsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(ids);
            _ = _mediator.Setup(mediator => mediator.Send(It.Is<GetStoryRequest>(request => request.Id == id1), It.IsAny<CancellationToken>())).ReturnsAsync(_story1);
            _ = _mediator.Setup(mediator => mediator.Send(It.Is<GetStoryRequest>(request => request.Id == id2), It.IsAny<CancellationToken>())).ReturnsAsync(_story2);
            _ = _mediator.Setup(mediator => mediator.Send(It.Is<GetStoryRequest>(request => request.Id == id3), It.IsAny<CancellationToken>())).ReturnsAsync(_story3);

            var model = CreateEdmModel<SortedStory>("stories");
            _oDataQueryContext = new ODataQueryContext(model, typeof(SortedStory), new ODataPath());

            _httpRequest = CreateDefaultHttpRequest();
        }

        [Test]
        public async Task CanGetBestStoriesByQueryTop2()
        {
            var apiStory1 = ApiStory.Create(_story1);
            var apiStory2 = ApiStory.Create(_story2);
            var expectedStories = new ApiStory[] { apiStory1, apiStory2 };
            var options = new ODataQueryOptions<SortedStory>(_oDataQueryContext, _httpRequest);
            var request = new GetBestStoriesByQueryRequest(options);
            var handler = new GetBestStoriesByQueryHandler(_mediator.Object);
            var result = await handler.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedStories));
        }

        private static IEdmModel CreateEdmModel<TEntity>(string entitySetName) where TEntity : class
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<TEntity>(entitySetName);
            return builder.GetEdmModel();
        }

        private HttpRequest CreateDefaultHttpRequest()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.QueryString = new QueryString("?$top=2");

            return httpContext.Request;
        }
    }
}
