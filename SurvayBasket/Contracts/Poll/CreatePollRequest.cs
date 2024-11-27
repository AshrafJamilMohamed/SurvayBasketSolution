using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Poll
{
    public class CreatePollRequest
    {
        [Length(5, 100, ErrorMessage = "Min 5 ,Max 100")]
        public string Title { get; set; } = string.Empty;

        [Length(5, 500, ErrorMessage = "Min 5 ,Max 500")]
        public string Summary { get; set; } = string.Empty;

        [DataType(DataType.Date)]

        public DateOnly StartsAt { get; set; }

        [DataType(DataType.Date)]
        public DateOnly EndsAt { get; set; }


    }
}
