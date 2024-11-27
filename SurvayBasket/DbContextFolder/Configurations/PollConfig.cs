using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurvayBasket.Models;

namespace SurvayBasket.DbContextFolder.Configurations
{
    public class PollConfig : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x=>x.Title).IsUnique();

            builder.Property(x => x.Title).HasMaxLength(100);
            builder.Property(x => x.Summary).HasMaxLength(100);
            
        }
    }
}
