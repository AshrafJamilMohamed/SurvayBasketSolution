namespace SurvayBasket.Contracts.Result
{
    public sealed class VotesPerDayResponse
    {
        public DateOnly Date { get; set; }
        public int Count { get; set; }
    }
}
