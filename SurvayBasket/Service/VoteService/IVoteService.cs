using SurvayBasket.Contracts.Vote;

namespace SurvayBasket.Service.VoteService
{
    public interface IVoteService
    {
        public Task<(bool Result, string Message)> AddAsync(int pollId, string UserId, VoteRequest voteRequest  ,CancellationToken cancellationToken = default);
    }
}
