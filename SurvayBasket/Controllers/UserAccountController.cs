
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using SurvayBasket.Contracts.User;
using SurvayBasket.Service.Account;


namespace SurvayBasket.Controllers
{
    [Route("me/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("IPLimiter")]
    public class UserAccountController(IUserAccountService userService) : ControllerBase
    {
        private readonly IUserAccountService userService = userService;

        [HttpGet("info")]
        [ProducesDefaultResponseType(typeof(UserProfileResponse))]
        public async Task<IActionResult> UserProfile()
          => Ok(await userService.UserProfile(User.GetUserId()!));

        [HttpPut("Update")]
        public async Task<IActionResult> Update(string UserID, UpdateUserProfileRequest request)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!UserID.Equals(id)) return BadRequest(new APIErrorResponse(400, "Not Valid"));
            var result = await userService.Update(UserID, request);

            return result is true ? NoContent() : BadRequest(new APIErrorResponse(400, "BadRequest"));
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(string UserID, ChangePasswordRequest request)
        {
            if (request.OldPassword.Equals(request.NewPassword))
                return BadRequest(new APIErrorResponse(400, "the same password is not allowed!"));

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!UserID.Equals(id)) return BadRequest(new APIErrorResponse(400, "Not Valid"));

            var (result, Message) = await userService.ChangePassword(UserID, request);
            return result is true ? NoContent() : BadRequest(new APIErrorResponse(400, Message));
        }

        [HttpGet("all")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
           => Ok(await userService.GetAllUsers(cancellationToken));

        [HttpGet("user/{id}")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Get([FromRoute] string id)
          => await userService.GetById(id) is not { } user ? NotFound(404) : Ok(user);

        [HttpPost("add")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Add(CreateUserRequest userRequest, CancellationToken cancellationToken)
        {
            var (message, user) = await userService.Add(userRequest, cancellationToken);
            return string.IsNullOrEmpty(message) ? Ok(user) : BadRequest(new APIErrorResponse(400, message));
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> Update([FromRoute] string id, UpdateUserRequest userRequest, CancellationToken cancellationToken)
        {
            var (result, message) = await userService.UpdateAsync(id, userRequest, cancellationToken);
            return string.IsNullOrEmpty(message) ? NoContent() : BadRequest(new APIErrorResponse(400, message));
        }


        [HttpPut("ToggleStatus/{id}")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> ToggleStatus([FromRoute] string id)
        {
            var result = await userService.ToggleStatus(id); ;
            return result ? NoContent() : BadRequest(new APIErrorResponse(400));
        }

        [HttpPut("UnLock/{id}")]
        [Authorize(Roles = DefaultRoles.Admin)]
        public async Task<IActionResult> UnLock([FromRoute] string id)
        {
           
            var result = await userService.Unlock(id); ;
            return result ? NoContent() : BadRequest(new APIErrorResponse(400));
        }
    }
}
