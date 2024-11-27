using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Vote
{
    public class VoteAnswerRequest
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public int QuestionId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public int AnswerId { get; set; }
    }
}
