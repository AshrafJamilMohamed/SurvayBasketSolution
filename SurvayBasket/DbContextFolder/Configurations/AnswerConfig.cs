using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurvayBasket.DbContextFolder.Configurations
{
    public class AnswerConfig : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasIndex(X => new { X.Id, X.Content });
        }
    }
}
