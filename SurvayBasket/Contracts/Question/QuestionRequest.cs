using SurvayBasket.Contracts.Answer;
using SurvayBasket.Contracts.Validations.AnswerValidation;
using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Question
{
    public sealed class QuestionRequest
    {
        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string Content { get; set; } = string.Empty;

        [Required]
        //[Range(2, int.MaxValue, ErrorMessage = "Answers must be at least 2")]
        [MinimumCount(3, ErrorMessage = "Answers must be at least 2")]
        public List<AnswerResponse> Answers { get; set; } = new List<AnswerResponse>();
    }
}
