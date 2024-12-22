using Microsoft.AspNetCore.RateLimiting;
using SurvayBasket.Service.ResultService;

namespace SurvayBasket.Controllers
{
    [Route("api/polls/{pollId:int}/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("IPLimiter")]
    public class ResultController : ControllerBase
    {
        private readonly IResultSerevice resultSerevice;

        public ResultController(IResultSerevice resultSerevice)
        {
            this.resultSerevice = resultSerevice;
        }

        [HttpGet("row-data")]

        public async Task<IActionResult> PollVotes([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var (Response, message) = await resultSerevice.GetPollVotesAsync(pollId, cancellationToken);

            return !string.IsNullOrEmpty(message) ? BadRequest(new APIErrorResponse(400, message)) : Ok(Response);

        }

        [HttpGet("votes-per-day")]

        public async Task<IActionResult> VotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var (Response, message) = await resultSerevice.GetVotesPerDayAsync(pollId, cancellationToken);

            return !string.IsNullOrEmpty(message) ? BadRequest(new APIErrorResponse(400, message)) : Ok(Response);

        }

        [HttpGet("votes-per-question")]

        public async Task<IActionResult> VotesPerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var (Response, message) = await resultSerevice.GetVotesPerQuestionAsync(pollId, cancellationToken);

            return !string.IsNullOrEmpty(message) ? BadRequest(new APIErrorResponse(400, message)) : Ok(Response);

        }


    }
}
