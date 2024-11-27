using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Models
{
    public class Question : AuditableEntity
    {
        public int Id { get; set; }

        [Length(3, 300)]
        public string Content { get; set; } = string.Empty;

        public ICollection<Answer> Answer { get; set; } = new HashSet<Answer>();
        public ICollection<VoteAnswer> VoteAnswer { get; set; } = [];

        public bool IsActive { get; set; } = true;
        public int PollId { get; set; }
        public Poll Poll { get; set; } = default!;


    }
}
