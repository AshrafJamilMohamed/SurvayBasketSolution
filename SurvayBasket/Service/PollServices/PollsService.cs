using Microsoft.EntityFrameworkCore;
using SurvayBasket.DbContextFolder;
using SurvayBasket.Models;

namespace SurvayBasket.Service.PollServices
{
    public class PollsService : IPollsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public PollsService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<Poll> Add(Poll Entity, CancellationToken cancellationToken)
        {

            await dbContext.AddAsync(Entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Entity;

        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            var Poll = GetPollById(id, cancellationToken);
            if (Poll is not null)
            {
                dbContext.Remove(Poll);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;


        }



        public async Task<IReadOnlyList<PollResponse>> GetAll()
        {

            var Polls = await dbContext.Polls.AsNoTracking().ToListAsync();
            var PollRespons = mapper.Map<IReadOnlyList<PollResponse>>(Polls);
            return PollRespons;
        }

        public async Task<IReadOnlyList<PollResponse>> GetCurrentAll()
        {
            var Polls = await dbContext.Polls
                .AsNoTracking() 
                .Where(x =>
                x.IsPublished
                &&
                x.StartsAt >= DateOnly.FromDateTime(DateTime.UtcNow)
                &&
                x.EndsAt <= DateOnly.FromDateTime(DateTime.UtcNow))
                .ToListAsync();

            var PollRespons = mapper.Map<IReadOnlyList<PollResponse>>(Polls);
            return PollRespons;
        }

        public async Task<Poll?> GetPollById(int id, CancellationToken cancellationToken)
        {
            var poll = await dbContext.Polls.FirstOrDefaultAsync(x => x.Id == id);
            return poll;
        }

        public async Task<bool> Update(int id, Poll entity, CancellationToken cancellationToken)
        {
            var currentPoll = await GetPollById(id, cancellationToken);
            if (currentPoll is null)
                return false;

            // Only update necessary fields
            currentPoll.Title = entity.Title;
            currentPoll.Summary = entity.Summary;
            currentPoll.EndsAt = entity.EndsAt;
            currentPoll.StartsAt = entity.StartsAt;
            currentPoll.IsPublished = entity.IsPublished;
            currentPoll.UpdatedAt = DateTime.UtcNow;
            currentPoll.UpdatedById = entity.UpdatedById;



            dbContext.Polls.Update(currentPoll);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;

        }


    }
}
