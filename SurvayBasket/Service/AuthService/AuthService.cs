
using Microsoft.AspNetCore.WebUtilities;


namespace SurvayBasket.Service.AuthService
{
    public class AuthService(IHttpContextAccessor httpContextAccessor,
                             IEmailSender emailSender,
                             UserManager<ApplicationUser> userManager,
                             ILogger<AuthService> logger) : IAuthService
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender emailSender = emailSender;
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly ILogger<AuthService> logger = logger;

        public async Task<(bool result,string? message)> ResetPasswordAsync(string email)
        {
            if (await userManager.FindByEmailAsync(email) is not { } user)
                return (true,string.Empty);

            if(!user.EmailConfirmed) return  (false, "Email is not confirmed");
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            logger.LogInformation("Confirmation Code :{code}", code);
            //  send email 
            await ResendforgetPasswordEmail(user, code);
            return (true, string.Empty); ;
        }

        public async Task SendConfirmationEmail(ApplicationUser user, string code)
        {
            var origins = httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = await EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                new Dictionary<string, string>
                {
                    {"{{name}}",user.FristName },
                    {"{{action_url}}",$"{origins}/auth/emailConfirmation?userId={user.Id}&code={code}" }
                });
            await emailSender.SendEmailAsync(user.Email!, "Survay Basket :Email Confirmation ✔️", emailBody);
        }

        public async Task ResendforgetPasswordEmail(ApplicationUser user, string code)
        {
            var origins = httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = await EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                new Dictionary<string, string>
                {
                    {"{{name}}",user.FristName },
                    {"{{action_url}}",$"{origins}/auth/forget-passowrd?userEmail={user.Email}&code={code}" }
                });
            await emailSender.SendEmailAsync(user.Email!, "Survay Basket : Resent Forget Password ✔️", emailBody);
        }
    }
}
