using System.ComponentModel.DataAnnotations.Schema;

namespace SurvayBasket.Models
{
    [NotMapped]
    public class AuditableEntity
    {

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        [ForeignKey("CreatedBy")]
        public string CreatedById { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("UpdatedBy")]
        public string? UpdatedById { get; set; }

        public ApplicationUser CreatedBy { get; set; } = default!;
        public ApplicationUser? UpdatedBy { get; set; }


    }
}
