using Microsoft.AspNetCore.RateLimiting;
using SurvayBasket.Contracts.Common;

namespace SurvayBasket.Controllers
{
    [Route("api/polls/{pollid}/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("concurrency")]
   
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService questionService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public QuestionController(IQuestionService questionService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.questionService = questionService;
            this.mapper = mapper;
            this.userManager = userManager;
        }


        [HttpGet]
        public async Task<ActionResult<Pagination<QuestionResponse>>> GetAllAsync([FromRoute] int pollid, [FromQuery] RequestFilter requestFilter, CancellationToken cancellationToken)
        {
            var (questionResponse, Message) = await questionService.GetAllAsync(pollid,requestFilter, cancellationToken);
            if (!string.IsNullOrEmpty(Message))
                return BadRequest(new APIErrorResponse(400, Message));

            return Ok(questionResponse);
        }

        [HttpGet("Available")]
        
        public async Task<IActionResult> GetAllAvailableAsync([FromRoute] int pollid, CancellationToken cancellationToken)
        {
            var UserId = User.GetUserId();
            var (questionResponse, Message) = await questionService.GetAllAvailableAsync(pollid, UserId!, cancellationToken);
            if (!string.IsNullOrEmpty(Message))
                return BadRequest(new APIErrorResponse(400, Message));

            return Ok(questionResponse);
        }


        [HttpGet("{id:int}")]
        [DisableRateLimiting]
        public async Task<IActionResult> GetAsync([FromRoute] int pollid, int id, CancellationToken cancellationToken)
        {
            var (questionResponse, Message) = await questionService.GetAsync(pollid, id, cancellationToken);
            if (!string.IsNullOrEmpty(Message))
                return BadRequest(new APIErrorResponse(400, Message));

            return Ok(questionResponse);
        }


        [HttpPost]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> AddQuestion([FromRoute] int pollid, QuestionRequest request, CancellationToken cancellationToken)
        {
            var UserId = User.GetUserId();
            var user = await userManager.FindByIdAsync(UserId!);

            var Result = await questionService.AddAsync(pollid, request, user!, cancellationToken);

            if (!string.IsNullOrEmpty(Result.Message))
                return BadRequest(new APIErrorResponse(400, Result.Message));

            var questionResponse = mapper.Map<QuestionResponse>(Result.questionResponse);

            return Ok(questionResponse);
        }


        [HttpPut("{id:int}")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> UpdateQuestion([FromRoute] int pollid, [FromRoute] int id, QuestionRequest request, CancellationToken cancellationToken)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var CurrentUser = await userManager.FindByEmailAsync(Email!);

            if (CurrentUser is null)
                return BadRequest(new APIErrorResponse(401, null));

            var Result = await questionService.UpdateAsync(pollid, id, request, CurrentUser, cancellationToken);

            if (!string.IsNullOrEmpty(Result.Message))
                return BadRequest(new APIErrorResponse(400, Result.Message));

            return NoContent();
        }


        [HttpPut("{id:int}/ToggleStatus")]
        [Authorize(Roles = DefaultRoles.Admin)]

        public async Task<IActionResult> ToggleStatus(int pollid, int id, CancellationToken cancellationToken)
        {
            var (result, message) = await questionService.ToggleStatus(pollid, id, cancellationToken);
            if (!string.IsNullOrEmpty(message))
                return BadRequest(new APIErrorResponse(400, message));

            return NoContent();
        }
    }
}
