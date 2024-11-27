namespace SurvayBasket.Contracts.Result
{
    public class VotesPerAnswerResponse
    {
        public string AnswerContent { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
