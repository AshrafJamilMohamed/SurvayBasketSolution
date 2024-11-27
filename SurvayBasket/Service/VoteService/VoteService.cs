
using Microsoft.EntityFrameworkCore;
using SurvayBasket.Contracts.Vote;
using SurvayBasket.Models;

namespace SurvayBasket.Service.VoteService
{
    public class VoteService(ApplicationDbContext context, IMapper mapper) : IVoteService
    {
        private readonly ApplicationDbContext dbContext = context;
        private readonly IMapper mapper = mapper;

        public async Task<(bool Result, string Message)> AddAsync(int pollId, string UserId, VoteRequest voteRequest, CancellationToken cancellationToken = default)
        {
            // check on Poll if exist or not and is Available for Voting

            var Poll = dbContext.Polls.Where(p =>
                p.Id == pollId
                &&
                p.IsPublished
                &&
                p.StartsAt >= DateOnly.FromDateTime(DateTime.UtcNow)
                &&
                p.EndsAt <= DateOnly.FromDateTime(DateTime.UtcNow));

            if (Poll is null)
                return (false, "Not Avaliabel For Now");

            // Check if this user already voted before
            var Voted = await dbContext.Votes.AnyAsync(v => v.UserId == UserId && v.PollId == pollId, cancellationToken);
            if (Voted)
                return (false, "You have voted before :)");

            // check if threr are diff questions

            var availableQuestions = await dbContext.Questions
                .Where(q => q.PollId == pollId && q.IsActive)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            if (!voteRequest.VoteAnswers.Select(x => x.QuestionId).SequenceEqual(availableQuestions))
                return (false, "Invalid Questions!");




            var vote = new Vote()
            {
                PollId = pollId,
                UserId = UserId,
                VoteAnswer = mapper.Map<IEnumerable<VoteAnswer>>(voteRequest.VoteAnswers).ToList()

            };


            await dbContext.AddAsync(vote, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            //var voteId = vote.Id;

            ////var VoteID = await dbContext.Votes
            ////    .Where(v => v.PollId == pollId && v.UserId == UserId)
            ////    .Select(x => x.Id).SingleOrDefaultAsync(cancellationToken);


            //foreach (var item in voteRequest.VoteAnswers)
            //{
            //    var NewVotedAnswer = new VoteAnswer()
            //    {
            //        AnswerId = item.AnswerId,
            //        QuestionId = item.QuestionId,
            //        VoteId = voteId

            //    };
            //    await dbContext.AddAsync(NewVotedAnswer, cancellationToken);
            //}

            //await dbContext.SaveChangesAsync(cancellationToken);





            return (true, string.Empty);



        }
    }
}
