namespace SurvayBasket.Contracts.Result
{
    public sealed class VoteResponse
    {
        public string VoterName { get; set; } = string.Empty;
        public DateTime VotingDate { get; set; }
        public IEnumerable<QuestionAnswerResponse> SelectedAnswers { get; set; } = [];
    }
}
