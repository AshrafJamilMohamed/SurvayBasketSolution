
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.WebUtilities;
using SurvayBasket.Service.AuthService;

namespace SurvayBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationUser user;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService token;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;
        private readonly IAuthService authService;

        public AccountController(ApplicationUser user,
             UserManager<ApplicationUser> userManager,
             ITokenService token,
             SignInManager<ApplicationUser> signInManager,
             ILogger<AccountController> logger,
             IAuthService authService
            )
        {
            this.user = user;
            this.userManager = userManager;
            this.token = token;
            this.signInManager = signInManager;
            this.logger = logger;
            this.authService = authService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register model, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            // Check if Email is Found
            if (user is { }) return BadRequest(new APIErrorResponse(409, "This email already found before!"));

            var NewUser = new ApplicationUser()
            {
                FristName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0]
            };


            var Result = await userManager.CreateAsync(NewUser, model.Password);

            if (!Result.Succeeded)
                return BadRequest(new APIErrorResponse(400, Result.Errors.First().Description));



            var Code = await userManager.GenerateEmailConfirmationTokenAsync(NewUser);
            Code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Code));

            logger.LogInformation("Confirmation Code :{code}", Code);
            //   send email 
            await authService.SendConfirmationEmail(NewUser!, Code);

            return NoContent();

        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDTO LoginUser, CancellationToken cancellationToken = default)
        {


            var LoginedUser = await userManager.FindByEmailAsync(LoginUser.Email);
            ;
            if (LoginedUser is null) return Unauthorized();
            if (!LoginedUser.EmailConfirmed) return BadRequest(new APIErrorResponse(400, "Email is not Confirmed"));

            var Result = await signInManager.CheckPasswordSignInAsync(LoginedUser, LoginUser.Password, false);

            if (!Result.Succeeded) return Unauthorized();
          var Roles=  await userManager.GetRolesAsync(LoginedUser);

            var User = new UserDTO()
            {
                Email = LoginUser.Email,
                FirstName = LoginedUser.FristName,
                LastName = LoginedUser.LastName,
                Token =  token.CreateToken(LoginedUser, Roles)
            };
            return Ok(User);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest confirmEmail)
        {
            var user = await userManager.FindByIdAsync(confirmEmail.UserId);
            if (user is null)
                return BadRequest(new APIErrorResponse(400, "Invalid Code"));

            if (user.EmailConfirmed)
                return BadRequest(new APIErrorResponse(400, "Invalid Code"));

            var Code = confirmEmail.Code;
            try
            {
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
            }
            catch (FormatException ex)
            {
                return BadRequest(new APIErrorResponse(400, $"Invalid Code : {ex.Message}"));
            }

            var Result = await userManager.ConfirmEmailAsync(user, Code);

            if (Result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, DefaultRoles.Member);
                return Ok();
            }
            var error = Result.Errors.First();
            return BadRequest(new APIErrorResponse(400, error.Description));

        }


        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail(ResendConfirmEmailRequest resendconfirmEmail)
        {
            var user = await userManager.FindByEmailAsync(resendconfirmEmail.Email);
            if (user is null)
                return Ok();

            if (user.EmailConfirmed)
                return BadRequest(new APIErrorResponse(400, "Email is confirmed before!"));


            var Code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            Code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Code));

            logger.LogInformation("Confirmation Code :{code}", Code);
            //  send email 
            await authService.SendConfirmationEmail(user, Code);

            return Ok();

        }


        [HttpPost("forget-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var (result, message) = await authService.ResetPasswordAsync(request.Email);
            if (!string.IsNullOrEmpty(message))
                return BadRequest(new APIErrorResponse(400, message));
            return Ok();
        }

    }
}
