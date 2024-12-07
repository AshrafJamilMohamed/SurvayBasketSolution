
namespace SurvayBasket.Service.Account
{
    public class UserAccountService(UserManager<ApplicationUser> userManager) : IUserAccountService
    {


        public async Task<(bool result, string? Message)> ChangePassword(string userId, ChangePasswordRequest request)
        {
            var user = await userManager.FindByIdAsync(userId);
            var Result = await userManager.ChangePasswordAsync(user!, request.OldPassword, request.NewPassword);
            return Result.Succeeded ? (true, string.Empty) : (false, Result.Errors.First().Description);
        }

        public async Task<bool> Update(string userId, UpdateUserProfileRequest request)
        {
            var result = await userManager.Users
                   .Where(u => u.Id == userId)
                   .ExecuteUpdateAsync
                   (x => x
                       .SetProperty(y => y.FristName, request.FirstName)
                       .SetProperty(y => y.LastName, request.LastName)
                   );
            return result > 0 ? true : false;

        }

        public async Task<UserProfileResponse> UserProfile(string UserId)
        {
            var user = await userManager.Users.Where(u => u.Id == UserId)
                  .Select(n => new UserProfileResponse(n.Email!, n.UserName!, n.FristName, n.LastName))
                  .FirstAsync();
            return user;
        }
    }
}
