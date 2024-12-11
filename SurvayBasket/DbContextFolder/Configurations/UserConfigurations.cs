using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace SurvayBasket.DbContextFolder.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                new ApplicationUser
                {
                    Id = DefaultUsers.AdminId,
                    Email = DefaultUsers.AdminEmail,
                    NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                    ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                    SecurityStamp = DefaultUsers.AdminSecurityStamp,
                    FristName = "Admin",
                    LastName = "Demo",
                    EmailConfirmed = true,
                    UserName = DefaultUsers.AdminEmail.Split('@')[0],
                    NormalizedUserName = DefaultUsers.AdminEmail.Split('@')[0].ToUpper(),
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
                }

                );
        }
    }
}
