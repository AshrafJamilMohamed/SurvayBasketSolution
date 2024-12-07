
using SurvayBasket.Service.Account;


namespace SurvayBasket.Controllers
{
    [Route("me/[controller]")]
    [ApiController]
    [Authorize]
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


    }
}
