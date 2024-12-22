
using SurvayBasket.Contracts.User;
using SurvayBasket.Service.RolesService;

namespace SurvayBasket.Service.Account
{
    public class UserAccountService(
                                    UserManager<ApplicationUser> userManager,
                                    ApplicationDbContext dbContext,
                                    IRoleService roleService,
                                    RoleManager<ApplicationRole> roleManager
                                                                  ) : IUserAccountService
    {
        private readonly ApplicationDbContext dbContext = dbContext;
        private readonly IRoleService roleService = roleService;
        private readonly RoleManager<ApplicationRole> roleManager = roleManager;

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

        public async Task<IEnumerable<UserResponse>> GetAllUsers(CancellationToken cancellationToken)

            => await (from u in dbContext.Users
                      join ur in dbContext.UserRoles
                      on u.Id equals ur.UserId
                      join r in dbContext.Roles
                      on ur.RoleId equals r.Id into roles
                      where !roles.Any(x => x.Name == DefaultRoles.Member)
                      select new
                      {
                          u.Id,
                          u.FristName,
                          u.LastName,
                          u.Email,
                          u.IsDisabled,
                          Roles = roles.Select(x => x.Name!).ToList()
                      }
                      )
            .GroupBy(u => new { u.Id, u.FristName, u.LastName, u.IsDisabled, u.Email })
            .Select
                (
                    u => new UserResponse
                            (
                                u.Key.Id,
                                u.Key.FristName,
                                u.Key.LastName,
                                u.Key.Email,
                                u.Key.IsDisabled,
                                u.SelectMany(r => r.Roles)
                            )
                )
            .ToListAsync(cancellationToken);

        public async Task<UserResponse?> GetById(string id)
        {
            if (await userManager.FindByIdAsync(id) is not { } user)
                return default;

            var UserRoles = await userManager.GetRolesAsync(user);
            var response = new UserResponse
                (
                        user.Id,
                        user.FristName,
                        user.LastName,
                        user.Email!,
                        user.IsDisabled,
                        UserRoles
                );
            return response;
        }

        public async Task<(string? message, UserResponse? userResponse)> Add(CreateUserRequest userRequest, CancellationToken cancellationToken)
        {
            var IsExits = await userManager.Users.AnyAsync(e => e.Email == userRequest.Email, cancellationToken);
            if (IsExits)
                return ("Email is found before", default);

            var AllowedRoles = await roleService.GetAll(cancellationToken);
            if (userRequest.Roles.Except(AllowedRoles.Select(e => e.name)).Any())
                return ("Invalid Roles", default);


            var user = new ApplicationUser()
            {
                Email = userRequest.Email,
                FristName = userRequest.FirstName,
                LastName = userRequest.LastName,
                UserName = userRequest.Email.Split('@')[0],
                EmailConfirmed = true

            };

            var Result = await userManager.CreateAsync(user, userRequest.Password);
            if (Result.Succeeded)
            {
                await userManager.AddToRolesAsync(user, userRequest.Roles);

                return (string.Empty, new UserResponse
                                                    (user.Id,
                                                         user.FristName,
                                                         user.LastName,
                                                         user.Email,
                                                         user.IsDisabled,
                                                         userRequest.Roles
                                                    )
                        );
            }
            return ("Can not create this user", default);

        }


        public async Task<(bool result, string? message)> UpdateAsync(string id, UpdateUserRequest userRequest, CancellationToken cancellationToken)
        {
            // Check if email with another user 
            var IsExitsEmail = await userManager.Users.AnyAsync(u => u.Email == userRequest.Email && u.Id != id, cancellationToken);

            if (IsExitsEmail) return (false, "Email is used!");

            // Check on user if exits
            var user = await userManager.FindByIdAsync(id);
            if (user is not { })
                return (false, "User is not found!");

            // Check on Roles if it right or not
            var CurrentRoles = await roleService.GetAll(cancellationToken);
            var Values = userRequest.Roles.Except(CurrentRoles.Select(r => r.name)).Any();
            if (Values)
                return (false, "Invalid Roles!");

            // Update user
            user.FristName = userRequest.FirstName;
            user.LastName = userRequest.LastName;
            user.Email = userRequest.Email;
            user.UserName = userRequest.Email.Split('@')[0];
            user.NormalizedEmail = userRequest.Email.ToUpper();
            user.NormalizedUserName = userRequest.Email.ToUpper().Split('@')[0];

            var Result = await userManager.UpdateAsync(user);

            if (Result.Succeeded)
            {
                try
                {
                    // Update the roles
                    var OldUserRoles = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, OldUserRoles);

                    await userManager.AddToRolesAsync(user, userRequest.Roles);

                    return (true, string.Empty);

                }
                catch (Exception ex)
                {
                    return (false, ex.Message);

                }

            }
            return (false, "Can not update this user");

        }

        public async Task<bool> ToggleStatus(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null) return false;
            user.IsDisabled = !user.IsDisabled;
            await userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> Unlock(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null) return false;

            var result = await userManager.SetLockoutEndDateAsync(user, null);
            if (result.Succeeded) return true;

            return false;
        }

    }
}
