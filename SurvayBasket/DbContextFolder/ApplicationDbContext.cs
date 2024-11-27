using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurvayBasket.Models;
using System.Reflection;

namespace SurvayBasket.DbContextFolder
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure All the Classes that implement IEntityTypeConfiguration Interface

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            // Edit All FKs in database that is On Deleting is CasCade 
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;



            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteAnswer> VoteAnswers { get; set; }

    }
}
