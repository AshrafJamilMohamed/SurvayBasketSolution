namespace SurvayBasket.Service.AuthService
{
    public interface IAuthService
    {
        //public Task<bool> ConfirmEmail(ConfirmEmailRequest confirmEmail);

        public Task SendConfirmationEmail(ApplicationUser user, string code);
        public Task<(bool result, string? message)> ResetPasswordAsync(string  email);
    }
}
