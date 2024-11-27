using SurvayBasket.Contracts.Answer;
using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Question
{
    public sealed class QuestionResponse
    {
        public QuestionResponse()
        {
            
        }
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty!;

        public IEnumerable<AnswerResponse> Answers { get; set; } = [];


    }
}
