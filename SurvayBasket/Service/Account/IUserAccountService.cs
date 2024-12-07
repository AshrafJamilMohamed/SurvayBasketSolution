namespace SurvayBasket.Service.Account
{
    public interface IUserAccountService
    {
        public Task<UserProfileResponse> UserProfile(string UserId);

        public Task<bool> Update(string userId, UpdateUserProfileRequest request);
        public Task<(bool result, string? Message)> ChangePassword(string userId, ChangePasswordRequest request);
    }
}
