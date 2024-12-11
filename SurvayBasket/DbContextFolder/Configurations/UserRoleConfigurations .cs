using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurvayBasket.DbContextFolder.Configurations
{
    public class UserRoleConfigurations : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData([
                new IdentityUserRole<string>()
                {
                    RoleId = DefaultRoles.AdminRoleId,
                    UserId=DefaultUsers.AdminId
                },
                  new IdentityUserRole<string>()
                {
                    RoleId = DefaultRoles.MemberRoleId,
                    UserId= "5c490b1e-6fe9-4aaa-83e8-72a3e68ea280"
                },
                ]);
        }
    }
}

