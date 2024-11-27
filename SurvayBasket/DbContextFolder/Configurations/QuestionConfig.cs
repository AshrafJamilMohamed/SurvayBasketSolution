using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurvayBasket.DbContextFolder.Configurations
{
    public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasIndex(X => new { X.Id, X.Content });
        }
    }
}
