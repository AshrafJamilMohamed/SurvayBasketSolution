using SurvayBasket.Contracts.User;

namespace SurvayBasket.Service.Account
{
    public interface IUserAccountService
    {
        public Task<UserProfileResponse> UserProfile(string UserId);

        public Task<bool> Update(string userId, UpdateUserProfileRequest request);
        public Task<(bool result, string? Message)> ChangePassword(string userId, ChangePasswordRequest request);

        public Task<IEnumerable<UserResponse>> GetAllUsers(CancellationToken cancellationToken);
        public Task<UserResponse?> GetById(string id);
        public Task<(string? message, UserResponse? userResponse)> Add(CreateUserRequest userRequest, CancellationToken cancellationToken);

        public Task<(bool result, string? message)> UpdateAsync(string id, UpdateUserRequest userRequest, CancellationToken cancellationToken);

        public Task<bool> ToggleStatus(string id);

        public Task<bool> Unlock(string id);




    }
}
