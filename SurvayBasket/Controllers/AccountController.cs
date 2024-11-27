
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

        public AccountController(ApplicationUser user,
             UserManager<ApplicationUser> userManager,
             ITokenService token,
             SignInManager<ApplicationUser> signInManager
            )
        {
            this.user = user;
            this.userManager = userManager;
            this.token = token;
            this.signInManager = signInManager;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register model, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            // Check if Email is Found
            if (user is { }) return BadRequest();

            var NewUser = new ApplicationUser()
            {
                FristName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0]
            };
            var Result = await userManager.CreateAsync(NewUser, model.Password);

            if (!Result.Succeeded) return BadRequest();
            var CreatedAccount = new UserDTO()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Token = token.CreateToken(NewUser),
                Email = model.Email
            };
            return Ok(CreatedAccount);

        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDTO LoginUser, CancellationToken cancellationToken)
        {


            var LoginedUser = await userManager.FindByEmailAsync(LoginUser.Email);
            if (LoginedUser is null) return Unauthorized();

            var Result = await signInManager.CheckPasswordSignInAsync(LoginedUser, LoginUser.Password, false);

            if (!Result.Succeeded) return Unauthorized();

            var User = new UserDTO()
            {
                Email = LoginUser.Email,
                FirstName = LoginedUser.FristName,
                LastName = LoginedUser.LastName,
                Token = token.CreateToken(LoginedUser)
            };
            return Ok(User);
        }
    }
}
