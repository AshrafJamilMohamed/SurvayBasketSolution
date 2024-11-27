namespace SurvayBasket.Contracts.Result
{
    public class VotesPerQuestionResponse
    {
        public string Content { get; set; } = string.Empty;
        public IEnumerable<VotesPerAnswerResponse> SelectedAnswers { get; set; } = [];
    }
}
