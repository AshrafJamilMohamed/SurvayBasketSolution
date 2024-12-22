namespace SurvayBasket.Contracts.Poll
{
    public class PollResponse
    {
        public PollResponse(int id, string title, string summary)
        {
            Id = id;
            Title = title;
            Summary = summary;
        }
        public PollResponse()
        {
            
        }

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }
}
