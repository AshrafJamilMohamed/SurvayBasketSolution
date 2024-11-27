using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurvayBasket.DbContextFolder.Configurations
{
    public class VoteAnswersConfig : IEntityTypeConfiguration<VoteAnswer>
    {
        public void Configure(EntityTypeBuilder<VoteAnswer> builder)
        {
            builder.HasIndex(x => new { x.QuestionId, x.VoteId }).IsUnique();
        }
    }
}
