namespace SurvayBasket.Contracts.Common
{
    public class RequestFilter
    {
        [Range(1, 20)]
        public int PageNumber { get; set; }

        [Range(1, 20)]
        public int PageSize { get; set; }

        public string? SearchValue { get; set; }

        public string? OrderByAsc { get; set; }
        public string? OrderByDesc { get; set; }
    }

}
