using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurvayBasket.DbContextFolder.Configurations
{
    public class VoteConfig : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasIndex(x => new { x.PollId, x.UserId }).IsUnique();
        }
    }
}
