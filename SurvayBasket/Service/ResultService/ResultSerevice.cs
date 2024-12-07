using Microsoft.EntityFrameworkCore.ChangeTracking;
using SurvayBasket.Contracts.Result;

namespace SurvayBasket.Service.ResultService
{
    public class ResultSerevice : IResultSerevice
    {
        private readonly ApplicationDbContext dbContext;

        public ResultSerevice(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<(PollVoteResponse? Response, string? Message)> GetPollVotesAsync(int pollId, CancellationToken cancellationToken)
        {
            var Poll = await dbContext.Polls
                .FindAsync(pollId, cancellationToken);


            if (Poll is null)
                return (null, "Poll Not Found");

            var Response = await dbContext.Polls
                   .Where(p => p.Id == pollId)
                   .Select(v => new PollVoteResponse()
                   {
                       Title = v.Title,
                       Votes = v.Votes.Select(x => new VoteResponse()
                       {
                           VoterName = $"{x.User.FristName} {x.User.LastName}",
                           VotingDate = x.SubmittedOn,
                           SelectedAnswers = x.VoteAnswer
                                                    .Select(z => new QuestionAnswerResponse()
                                                    {
                                                        Answer = z.Answer.Content,
                                                        Question = z.Question.Content
                                                    })
                       })
                   }).SingleOrDefaultAsync(cancellationToken);

            return Response is null ? (null, "No Votes Found") : (Response, string.Empty);

        }

        public async Task<(IReadOnlyList<VotesPerDayResponse>? votesPerDayResponse, string? message)> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken)
        {
            var Poll = await dbContext.Polls
                .FindAsync(pollId, cancellationToken);

            if (Poll is null)
                return (null, "Poll Not Found");

            var votesPerDay = await dbContext.Votes
                  .Where(v => v.PollId == pollId)
                  .GroupBy(x => new { Date = DateOnly.FromDateTime(x.SubmittedOn) })
                  .Select(n => new VotesPerDayResponse()
                  {
                      Date = n.Key.Date,
                      Count = n.Count()

                  }).ToListAsync(cancellationToken);

            return (votesPerDay, string.Empty);
        }

        public async Task<(IReadOnlyList<VotesPerQuestionResponse>? votesPerDayResponse, string? message)> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken)
        {
            var Poll = await dbContext.Polls
              .FindAsync(pollId, cancellationToken);

            if (Poll is null)
                return (null, "Poll Not Found");

            var VotesPerQuestionResponse = await dbContext.VoteAnswers
                .Where(a => a.Vote.PollId == pollId)
                .Select(x => new VotesPerQuestionResponse()
                {
                    Content = x.Question.Content,
                    SelectedAnswers = x.Question.VoteAnswer
                                          .GroupBy(x => new { AnswerId = x.Answer.Id, AnswerContent = x.Answer.Content })
                                          .Select(v => new VotesPerAnswerResponse()
                                          {
                                              AnswerContent = v.Key.AnswerContent,
                                              Count = v.Count()

                                          })
                }).ToListAsync(cancellationToken);

            return (VotesPerQuestionResponse, string.Empty);
        }
    }
}
