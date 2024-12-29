using SurvayBasket.Contracts.Vote;

namespace SurvayBasket.Controllers
{  
    [Route("api/polls/{pollId:int}/vote")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Member}")]
    [EnableRateLimiting("IPLimiter")]
    public class VotesController(IQuestionService questionService, IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionService questionService = questionService;
        private readonly IVoteService voteService = voteService;

        [HttpGet]
        public async Task<IActionResult> StartVoting([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var UserId = User.GetUserId();
            var (response, message) = await questionService.GetAllAvailableAsync(pollId, UserId!, cancellationToken);

            if (!string.IsNullOrEmpty(message))
                return BadRequest(new APIErrorResponse(400, message));

            return Ok(response);

        }

        [HttpPost]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Add([FromRoute] int pollId, VoteRequest voteRequest, CancellationToken cancellationToken)
        {
            var (Result, Message) = await voteService.AddAsync(pollId, User.GetUserId()!, voteRequest, cancellationToken);

            if (!string.IsNullOrEmpty(Message))
                return BadRequest(new APIErrorResponse(400, Message));
            if (Result)
                return Created();

            return BadRequest();

        }




    }
}
