using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Account
{
    public sealed class ConfirmEmailRequest
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
