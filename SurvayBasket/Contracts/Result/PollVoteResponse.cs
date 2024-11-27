namespace SurvayBasket.Contracts.Result
{
    public sealed class PollVoteResponse
    {
        public string Title { get; set; } = string.Empty;
        public IEnumerable<VoteResponse> Votes { get; set; } = [];
    }
}
