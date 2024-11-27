using SurvayBasket.Contracts.Validations.AnswerValidation;
using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Vote
{
    public class VoteRequest
    {
        [Required]
       // [Range(1, double.MaxValue, ErrorMessage = "Answers not allowed to be empty!")]
        [MinLength(1,ErrorMessage = "Answers not allowed to be empty!")]
        public IEnumerable<VoteAnswerRequest> VoteAnswers { get; set; } = [];
    }
}
