using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Length(3, 300)]
        public string Content { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = default!;
    }
}
