using SurvayBasket.Contracts.Result;

namespace SurvayBasket.Service
{
    public interface IResultSerevice
    {
        public Task<(PollVoteResponse? Response, string? Message)> GetPollVotesAsync(int pollId, CancellationToken cancellationToken);

        public Task<(IReadOnlyList<VotesPerDayResponse>? votesPerDayResponse, string? message)> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken);
        public Task<(IReadOnlyList<VotesPerQuestionResponse>? votesPerDayResponse, string? message)> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken);
    }
}
