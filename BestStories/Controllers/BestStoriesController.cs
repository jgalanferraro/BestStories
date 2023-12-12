using BestStories.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Net;

namespace BestStories.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BestStoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BestStoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{top:int}")]
        public async Task<IActionResult> GetAsync(int top)
        {
            try
            {
                if (top < 0)
                {
                    return BadRequest();
                }

                var apiStories = await _mediator.Send(new GetBestStoriesRequest(top));
                return Ok(apiStories);
            } 
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Detail = $"An error occurred while processing the request. {ex.Message}",
                };
                return new ObjectResult(problemDetails)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByQueryAsync(ODataQueryOptions<SortedStory> options)
        {
            try
            {
                var apiStories = await _mediator.Send(new GetBestStoriesByQueryRequest(options));
                return Ok(apiStories);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Detail = $"An error occurred while processing the request. {ex.Message}",
                };
                return new ObjectResult(problemDetails)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
