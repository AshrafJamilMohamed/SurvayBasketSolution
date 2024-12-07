

namespace SurvayBasket.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PollsController : ControllerBase
    {
        private readonly IPollsService pollsService;
        private readonly IMapper mapper;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public PollsController(IPollsService _pollsService,
            IMapper _mapper,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            )
        {
            pollsService = _pollsService;
            mapper = _mapper;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet("GetAll")]
        // [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetAll() => Ok(await pollsService.GetAll());

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentAll() => Ok(await pollsService.GetCurrentAll());

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PollResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIErrorResponse))]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var poll = await pollsService.GetPollById(id, cancellationToken);
            if (poll is not null)
            { var MappedEntity = mapper.Map<Poll, PollResponse>(poll); return Ok(MappedEntity); }

            return NotFound(new APIErrorResponse(404, null));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PollResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIErrorResponse))]

        public async Task<IActionResult> Add(CreatePollRequest poll, CancellationToken cancellationToken)
        {
            var IsDateValid = DateChecker.IsDateValid(poll.StartsAt, poll.EndsAt);
            if (!IsDateValid)
            {
                return BadRequest(new APIErrorResponse(400, "Date Is Not Valid"));
            }
            var MappedEntity = mapper.Map<CreatePollRequest, Poll>(poll);

            var userEmail = User.FindFirstValue(ClaimTypes.Email)!;
            var CurrentUser = await userManager.FindByEmailAsync(userEmail);

            MappedEntity.CreatedById = CurrentUser?.Id!;
            var NewPoll = await pollsService.Add(MappedEntity, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = NewPoll.Id }, NewPoll);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIErrorResponse))]
        public async Task<IActionResult> Update(int id, CreatePollRequest pollRequest, CancellationToken cancellationToken)
        {
            var IsDateValid = DateChecker.IsDateValid(pollRequest.StartsAt, pollRequest.EndsAt);
            if (!IsDateValid)
            {
                return BadRequest(new APIErrorResponse(400, "Date Is Not Valid"));
            }
            var MappedEntity = mapper.Map<Poll>(pollRequest);

            var CurrentUser = await userManager.FindByIdAsync(User.GetUserId()!);

            var Result = await pollsService.Update(id, MappedEntity, cancellationToken);
            return Result is true ? Ok() : NotFound(new APIErrorResponse(404));
        }

        [HttpDelete]
       
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var Result = await pollsService.Delete(id, cancellationToken);
            return Result is true ? Ok(Result) : NotFound(new APIErrorResponse(404, null));
        }

        [HttpGet]
        [DisableCors] // Deny Any other Origins out of my domain to call this endpoint
        public IActionResult PrivatEndPointForTest()
        {
            var Response = "DummyString";
            return Ok(Response);
        }
    }
}
