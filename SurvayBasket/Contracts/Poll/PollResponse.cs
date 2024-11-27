namespace SurvayBasket.Contracts.Poll
{
    public class PollResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }
}
